using Nethereum.Util;
using Server_Strategico.Server;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Server_Strategico.Manager
{
    // 25/09/2026, su richiesta esplicita: file separato dal resto di BlockchainManager (vedi
    // commento in cima a BlockchainManager.cs) per tenere ben distinto tutto ciò che parla con
    // PolygonScan via HTTP da tutto ciò che parla con la blockchain via RPC/Nethereum (_web3) —
    // quest'ultimo resta indispensabile per FIRMARE E INVIARE transazioni reali (prelievi verso i
    // giocatori, svuotamento tesoreria), perché PolygonScan è un'API di sola lettura, non un nodo
    // che firma/trasmette. Qui dentro: numero di blocco attuale, scansione dei depositi USDT in
    // arrivo, saldi (USDT e nativo POL). La conferma dei depositi via ricevuta di transazione
    // (ConfermaBillInCorsoAsync, in BlockchainManager.cs) resta per ora su _web3/RPC.
    public static partial class BlockchainManager
    {
        // 25/09/2026: PolygonScan non ha più un'API separata — l'URL "api.polygonscan.com/api"
        // ora reindirizza alla documentazione di Etherscan (confermato aprendolo nel browser: porta
        // a docs.etherscan.io). Da maggio 2025 Etherscan ha unificato le API di tutti i suoi
        // explorer (PolygonScan, BscScan, Arbiscan, ecc.) in un'unica API "V2" multi-chain su un
        // solo dominio, selezionando la chain con il parametro "chainid" invece di un dominio
        // dedicato — e serve una chiave API Etherscan, le vecchie chiavi PolygonScan non
        // funzionano più lì. Fonti: docs.etherscan.io/v2-migration, info.etherscan.com/switch-to-
        // etherscan-api-v2-by-may-31-2025.
        private static readonly HttpClient _polygonScanHttp = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };

        private const string POLYGONSCAN_BASE = "https://api.etherscan.io/v2/api";
        private const int POLYGON_CHAIN_ID_ETHERSCAN = 137;   // Polygon PoS mainnet
        private const int POLYGON_CHAIN_ID_AMOY_ETHERSCAN = 80002; // Amoy — verificare che Etherscan V2 lo copra se USA_TESTNET viene attivato

        // Chiave API — ora una chiave ETHERSCAN (non più PolygonScan, vedi commento sopra). Il nome
        // del campo in Password.cs resta invariato per non dover ritoccare quel file una terza
        // volta: basta sostituire il VALORE con una chiave presa da etherscan.io (My Account → API
        // Keys), non più da polygonscan.com.
        private static string PolygonScanApiKey => Password.PolygonScanApiKey;

        private class PolygonScanEnvelope<T>
        {
            [JsonPropertyName("status")] public string Status { get; set; }
            [JsonPropertyName("message")] public string Message { get; set; }
            [JsonPropertyName("result")] public T Result { get; set; }
        }

        // Risposta "proxy" (stile JSON-RPC, niente campo "status") usata da action=eth_blockNumber.
        private class PolygonScanRpcEnvelope
        {
            [JsonPropertyName("result")] public string Result { get; set; }
        }

        private class PolygonScanTokenTx
        {
            [JsonPropertyName("blockNumber")] public string BlockNumber { get; set; }
            [JsonPropertyName("hash")] public string Hash { get; set; }
            [JsonPropertyName("from")] public string From { get; set; }
            [JsonPropertyName("to")] public string To { get; set; }
            [JsonPropertyName("value")] public string Value { get; set; }
            [JsonPropertyName("tokenDecimal")] public string TokenDecimal { get; set; }
        }

        private static async Task<string> PolygonScanGetAsync(string queryString)
        {
            int chainId = USA_TESTNET ? POLYGON_CHAIN_ID_AMOY_ETHERSCAN : POLYGON_CHAIN_ID_ETHERSCAN;
            string url = $"{POLYGONSCAN_BASE}?chainid={chainId}&{queryString}&apikey={PolygonScanApiKey}";
            var risposta = await _polygonScanHttp.GetAsync(url);
            risposta.EnsureSuccessStatusCode();

            string corpo = await risposta.Content.ReadAsStringAsync();

            // 25/09/2026: già capitato che l'endpoint rispondesse con una pagina HTML invece che
            // JSON (chiave mancante/non valida, o endpoint sbagliato) — JsonSerializer in quel caso
            // lancia un errore poco chiaro ("'<' is an invalid start of a value"). Meglio
            // intercettarlo subito qui con un messaggio comprensibile.
            if (corpo.TrimStart().StartsWith("<"))
                throw new Exception($"Etherscan/PolygonScan ha risposto con HTML invece che JSON (chiave API mancante/non valida?) — richiesta: {queryString}");

            return corpo;
        }

        private static async Task<ulong> ObtieniBloccoAttualeAsync()
        {
            string json = await PolygonScanGetAsync("module=proxy&action=eth_blockNumber");
            var envelope = JsonSerializer.Deserialize<PolygonScanRpcEnvelope>(json);
            if (string.IsNullOrWhiteSpace(envelope?.Result))
                throw new Exception($"PolygonScan (eth_blockNumber) ha risposto senza un risultato valido: {json}");

            // Risultato esadecimale ("0x..."), come da JSON-RPC.
            return Convert.ToUInt64(envelope.Result, 16);
        }

        private static async Task ScansionaDepositiAsync()
        {
            ulong bloccoAttuale = await ObtieniBloccoAttualeAsync();

            // Primo avvio: non riscansionare tutta la storia della chain, si parte da "adesso".
            if (_ultimoBloccoScansionato == 0)
            {
                _ultimoBloccoScansionato = bloccoAttuale;
                SalvaStato();
                return;
            }

            if (bloccoAttuale <= _ultimoBloccoScansionato)
            {
                await ConfermaBillInCorsoAsync(bloccoAttuale);
                return;
            }

            ulong daBlocco = _ultimoBloccoScansionato + 1;
            ulong aBlocco = bloccoAttuale;

            // 25/09/2026: prima si usava un filtro eventi via RPC diretto (Nethereum GetEvent +
            // GetAllChangesAsync), con lo spezzettamento manuale del range a ~2000 blocchi per volta
            // (limite tipico di getLogs sugli RPC pubblici). tokentx di PolygonScan non ha questo
            // limite (pagina da sola), quindi qui non serve più — l'unico limite pratico è
            // l'offset/page qui sotto, pensato per non richiedere risposte enormi se la scansione
            // resta ferma a lungo (server spento per giorni): in quel caso, se ci sono più di 1000
            // depositi nel range, quelli oltre la prima pagina vengono ripresi al giro successivo.
            string json = await PolygonScanGetAsync(
                $"module=account&action=tokentx&address={TreasuryAddress}&contractaddress={UsdtContract}" +
                $"&startblock={daBlocco}&endblock={aBlocco}&page=1&offset=1000&sort=asc");

            var envelope = JsonSerializer.Deserialize<PolygonScanEnvelope<List<PolygonScanTokenTx>>>(json);

            // PolygonScan risponde status="0" sia per un errore vero, sia semplicemente quando non
            // ci sono transazioni nel range interrogato ("No transactions found") — vanno distinti,
            // altrimenti ogni giro senza depositi finirebbe loggato come [ERRORE] a torto.
            List<PolygonScanTokenTx> transazioni;
            if (envelope?.Status == "1")
                transazioni = envelope.Result ?? new List<PolygonScanTokenTx>();
            else if (string.Equals(envelope?.Message, "No transactions found", StringComparison.OrdinalIgnoreCase))
                transazioni = new List<PolygonScanTokenTx>();
            else
                throw new Exception($"PolygonScan (tokentx) ha risposto con errore: {envelope?.Message ?? json}");

            foreach (var tx in transazioni)
            {
                // tokentx restituisce sia i depositi in entrata che le uscite (sweep, prelievi) del
                // wallet di tesoreria — qui interessano solo quelli VERSO la tesoreria.
                if (!string.Equals(tx.To, TreasuryAddress, StringComparison.OrdinalIgnoreCase)) continue;

                string txHash = tx.Hash;

                // Se l'hash è già in blacklist (consumato da un'altra bill, o già assegnato a questa
                // stessa dalla scansione precedente/da una dichiarazione manuale) non fare nulla.
                if (HashUsati.ContainsKey(txHash)) continue;

                int decimali = int.TryParse(tx.TokenDecimal, out var d) ? d : USDT_DECIMALS;
                decimal importoRicevuto = UnitConversion.Convert.FromWei(BigInteger.Parse(tx.Value), decimali);

                // L'importo "salato" (vedi GeneraImportoUnivoco) di norma basta da solo a individuare
                // la bill giusta; se la bill ha anche dichiarato un wallet mittente, lo richiediamo
                // come ulteriore controllo — così un deposito "somigliante" ma da un wallet diverso
                // da quello atteso non viene scambiato per quello giusto.
                var bill = Bills.Values.FirstOrDefault(b =>
                    b.Status == "pending" &&
                    b.ImportoEsatto == importoRicevuto &&
                    (b.WalletMittente == null || string.Equals(b.WalletMittente, tx.From, StringComparison.OrdinalIgnoreCase)));

                if (bill == null) continue; // deposito non riconducibile a nessuna bill aperta (importo diverso, mittente diverso, o bill già scaduta)

                // TryAdd è atomico: se nel frattempo lo stesso hash è già stato "riservato" da una
                // dichiarazione manuale del giocatore (DichiaraPagamentoAsync) o da un'altra bill,
                // questa scansione lo scarta invece di sovrascrivere.
                if (!HashUsati.TryAdd(txHash, bill.OrderId)) continue;

                bill.TxHash = txHash;
                bill.WalletMittente ??= tx.From;
                bill.Status = "confirming"; // diventa "paid" solo dopo CONFERME_MINIME, vedi sotto
                Log($"Deposito rilevato per bill {bill.OrderId}: {importoRicevuto} USDT da {tx.From}, tx {txHash} — in attesa di {CONFERME_MINIME} conferme");
            }

            SalvaHashUsati();

            _ultimoBloccoScansionato = aBlocco;
            SalvaStato();
            SalvaBills();

            await ConfermaBillInCorsoAsync(bloccoAttuale);
        }

        // 25/09/2026: saldi letti via PolygonScan (HTTP) invece che via query RPC diretta
        // (Nethereum GetContractQueryHandler/GetBalance) — stesso cambio di approccio applicato a
        // ScansionaDepositiAsync qui sopra.
        public static async Task<decimal> SaldoUsdtAsync(string indirizzo)
        {
            string json = await PolygonScanGetAsync(
                $"module=account&action=tokenbalance&contractaddress={UsdtContract}&address={indirizzo}&tag=latest");
            var envelope = JsonSerializer.Deserialize<PolygonScanEnvelope<string>>(json);
            if (envelope?.Status != "1" || string.IsNullOrWhiteSpace(envelope.Result))
                throw new Exception($"PolygonScan (tokenbalance) ha risposto con errore: {envelope?.Message ?? json}");

            BigInteger saldoWei = BigInteger.Parse(envelope.Result);
            return UnitConversion.Convert.FromWei(saldoWei, USDT_DECIMALS);
        }

        public static Task<decimal> SaldoTesoreriaAsync() => SaldoUsdtAsync(TreasuryAddress);

        /// <summary>Saldo nel token nativo della rete (POL su Polygon), quello che paga il gas — diverso dal saldo USDT (token ERC20 a parte).</summary>
        public static async Task<decimal> SaldoNativoAsync()
        {
            string json = await PolygonScanGetAsync($"module=account&action=balance&address={TreasuryAddress}&tag=latest");
            var envelope = JsonSerializer.Deserialize<PolygonScanEnvelope<string>>(json);
            if (envelope?.Status != "1" || string.IsNullOrWhiteSpace(envelope.Result))
                throw new Exception($"PolygonScan (balance) ha risposto con errore: {envelope?.Message ?? json}");

            BigInteger saldoWei = BigInteger.Parse(envelope.Result);
            return UnitConversion.Convert.FromWei(saldoWei); // 18 decimali, default del token nativo
        }
    }
}
