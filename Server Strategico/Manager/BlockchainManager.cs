using NBitcoin;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using Nethereum.HdWallet;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using QRCoder;
using Server_Strategico.Server;
using Server_Strategico.ServerData.Moduli;
using Strategico_V2.Manager; // EmailManager — sì, namespace diverso dal resto (residuo del nome precedente del progetto)
using System.Collections.Concurrent;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Server.Server;

namespace Server_Strategico.Manager
{
    /// <summary>
    /// Gestisce pagamenti e prelievi in USDT sulla rete Polygon (EVM, chainId 137 — PoS mainnet).
    /// Copre: seed phrase del wallet server, emissione "bill" (scontrini) per i pagamenti in
    /// entrata, monitoraggio on-chain dei depositi, richieste/esecuzione dei prelievi, controlli
    /// di base (indirizzo valido, importo minimo, limite giornaliero, conferme minime).
    ///
    /// Richiede i pacchetti NuGet: Nethereum.Web3, Nethereum.HdWallet (che porta con sé NBitcoin
    /// per la generazione/validazione della seed phrase BIP39 — non serve aggiungerlo a mano).
    ///
    /// AVVIO: chiamare "await BlockchainManager.InizializzaAsync();" una sola volta all'avvio del
    /// server (già agganciato in GameSave.cs, subito dopo TokenManager.LoadRefreshTokens()).
    ///
    /// NOTA — scritto senza un compilatore C# a disposizione: dopo i primi errori di build (23/09,
    /// using mancanti per gli attributi ABI e il parametro "seedPassword" del costruttore Wallet)
    /// ogni firma usata qui (Wallet, GetEvent/CreateFilterInput, GetContractTransactionHandler/
    /// QueryHandler) è stata ricontrollata riga per riga contro il codice sorgente ufficiale di
    /// Nethereum 6.1.0 su GitHub. Fai comunque una build appena tiri giù i pacchetti: se compare
    /// ancora qualche errore dovrebbe trattarsi solo di un dettaglio minore.
    ///
    /// DESIGN "pool" (23/09/2026, su richiesta esplicita): niente scansione continua. Ogni
    /// INTERVALLO_SCANSIONE_SECONDI si interrogano i blocchi non ancora analizzati (da
    /// _ultimoBloccoScansionato+1 al blocco attuale, salvato su disco a ogni giro), si cerca un
    /// deposito che combaci con una bill "pending", e si passa a "confirming" salvando l'hash
    /// trovato — resta "confirming" finché non arriva a CONFERME_MINIME. Se il giocatore fornisce
    /// subito l'hash (DichiaraPagamentoAsync) si salta del tutto la scansione a blocchi: si
    /// verifica direttamente quella transazione. In entrambi i casi l'hash usato entra in una
    /// blacklist (HashUsati) che impedisce di riutilizzare la stessa transazione per un'altra bill.
    /// </summary>
    // 25/09/2026: classe divisa in due file (partial) su richiesta esplicita — questo file
    // (BlockchainManager.cs) contiene wallet/seed, bill/prelievi, wire protocol e tutto ciò che usa
    // _web3/Nethereum (letture legate alla firma, invio di transazioni reali); il file
    // BlockchainManager.PolygonScan.cs contiene solo le letture via HTTP a PolygonScan (blocco
    // attuale, scansione depositi, saldi). Stessa classe, stesso stato condiviso (Bills, Prelievi,
    // HashUsati, ecc.), solo separata per non mescolare più i due approcci nello stesso file.
    public static partial class BlockchainManager
    {
        #region Configurazione rete

        private const bool USA_TESTNET = false; // Cambia a true per puntare alla testnet Polygon Amoy durante i test, invece della mainnet.
        private const int CHAIN_ID_MAINNET = 137;   // Polygon PoS
        private const int CHAIN_ID_TESTNET = 80002;  // Polygon Amoy

        // Endpoint di fallback — usati solo se Password.KeyStorePassword è vuoto (vedi RpcUrl
        // sotto). 25/09/2026: sostituito il vecchio pubblico polygon-rpc.com (quello che dava
        // HttpRequestException/RpcClientUnknownException ripetute su eth_blockNumber sotto polling
        // continuo) con dRPC (drpc.org), testato manualmente via curl e confermato funzionante —
        // resta comunque un endpoint pubblico condiviso, non ha le garanzie di uno dedicato come
        // Ankr, ma va usato solo come rete di sicurezza se la chiave Ankr non è impostata. Per la
        // testnet Amoy non è stato confermato un endpoint dRPC dedicato: lasciato quello pubblico
        // Polygon originale, da verificare se USA_TESTNET viene attivato.
        private const string RPC_MAINNET = "https://polygon.drpc.org";
        private const string RPC_TESTNET = "https://rpc-amoy.polygon.technology";
        private const string USDT_MAINNET = "0xc2132D05D31c914a87C6611C10748AEb04B58e8"; // USDT (PoS) su Polygon mainnet — contratto ufficiale Bitfinex/Tether.
        private const string USDT_TESTNET = "0x0000000000000000000000000000000000dEaD"; // TODO: indirizzo mock di test

        public const int USDT_DECIMALS = 6;
        private const int CONFERME_MINIME = 256; //Conferme validazione transazione (Polygon: 128)
        private const int INTERVALLO_SCANSIONE_SECONDI = 90; //Tempo tra un giro di scansione e l'altro (non blocca il server, gira in background)

        public static int ChainId => USA_TESTNET ? CHAIN_ID_TESTNET : CHAIN_ID_MAINNET;

        // 25/09/2026, su richiesta esplicita: endpoint RPC dedicato (Ankr), letto da
        // Password.KeyStorePassword invece che da una variabile d'ambiente — stesso file/schema
        // già usato per SMTP_USER/SMTP_PASS/ADMIN_ALERT_EMAIL, mai hardcoded qui né committato su
        // Git. FORMATO CONFERMATO: Password.KeyStorePassword contiene solo la chiave Ankr nuda
        // (es. "1a2b3c4d..."), non l'URL completo — il primo tentativo con l'URL diretto ha dato
        // "Invalid URI: The format of the URI could not be determined." Qui sotto viene costruito
        // l'URL completo attorno alla chiave (endpoint diverso per mainnet/testnet, coerente con
        // USA_TESTNET). Se la chiave è vuota/non impostata, si torna al pubblico
        // RPC_MAINNET/RPC_TESTNET sopra — il server continua comunque a partire, solo con lo
        // stesso rischio di rate-limit di prima.
        private static string RpcUrl =>
            !string.IsNullOrWhiteSpace(Password.AnkrApiKey)
                ? $"https://rpc.ankr.com/{(USA_TESTNET ? "polygon_amoy" : "polygon")}/{Password.AnkrApiKey}"
                : (USA_TESTNET ? RPC_TESTNET : RPC_MAINNET);

        private static string UsdtContract => USA_TESTNET ? USDT_TESTNET : USDT_MAINNET;

        #endregion

        #region Wallet del server (seed phrase)

        // Percorso della seed phrase, cifrata a riposo (AES-256-GCM). Stesso schema di
        // TokenManager.SecretKey: file dedicato fuori dal repo (vedi .gitignore), permessi
        // ristretti sul filesystem del sistema operativo.
        //
        // 24/09/2026: su Windows era tornato un percorso RELATIVO ("seed.enc", finisce nella
        // cartella di lavoro del processo, non in quella salvataggi) dopo una correzione già fatta
        // in precedenza — ripristinata su richiesta esplicita. Stessa cartella "Blockchain" di
        // bills/prelievi/stato (vedi DataPath più sotto), coerente col resto della classe.
        private static readonly string SeedFilePath = OperatingSystem.IsLinux()
            ? "/opt/Warrior-and-Wealth/blockchain_seed.enc"
            : Path.Combine(GameSave.SavePath, "Blockchain", "blockchain_seed.enc");

        // Passphrase usata per cifrare la seed phrase su disco (vedi DerivaChiave) — spostata in
        // Password.cs (Server_Strategico.Server.Password.SEED_ENCRYPTION_PASSPHRASE) il 24/09/2026,
        // stesso posto di SMTP_USER/SMTP_PASS. In produzione va comunque preferita la variabile
        // d'ambiente WW_SEED_PASSPHRASE (ha sempre la precedenza, vedi DerivaChiave) — il const in
        // Password.cs resta solo un fallback/placeholder di sviluppo, mai il valore vero in produzione.

        // Indice di derivazione BIP44 riservato al wallet "caldo" del server: è l'indirizzo che
        // riceve tutti i pagamenti (campo "recipient" di ogni Bill) ed è da qui che partono i
        // prelievi verso i giocatori. Percorso derivato: m/44'/60'/0'/0/0 (standard Ethereum/EVM,
        // valido su qualunque chain EVM-compatibile incluso Polygon).
        private const int TREASURY_INDEX = 0;

        private static Wallet _wallet;
        private static Account _treasuryAccount;
        private static Web3 _web3;

        /// <summary>Indirizzo del wallet di tesoreria: è il "recipient" a cui i giocatori inviano i pagamenti.</summary>
        public static string TreasuryAddress => _treasuryAccount?.Address;

        /// <summary>
        /// Da chiamare una sola volta all'avvio del server (vedi GameSave.LoadData). Carica — o
        /// genera al primissimo avvio — la seed phrase, prepara il client Web3 e mette in moto il
        /// ciclo di controllo in background (depositi + conferme prelievi).
        /// </summary>
        public static async Task InizializzaAsync()
        {
            Directory.CreateDirectory(DataPath);

            string mnemonic = CaricaOGeneraSeedPhrase();
            _wallet = new Wallet(mnemonic, seedPassword: null);
            _treasuryAccount = _wallet.GetAccount(TREASURY_INDEX, ChainId);
            _web3 = new Web3(_treasuryAccount, RpcUrl);

            CaricaBills();
            CaricaPrelievi();
            CaricaStato();
            CaricaHashUsati();

            Log($"Wallet server pronto — indirizzo di tesoreria/deposito: {TreasuryAddress} (chainId {ChainId}, {(USA_TESTNET ? "TESTNET Amoy" : "MAINNET")})");

            _ = Task.Run(CicloMonitoraggioAsync); // non blocca l'avvio del server: gira in background per tutta la vita del processo
        }

        private static string CaricaOGeneraSeedPhrase()
        {
            if (File.Exists(SeedFilePath))
                return DecifraSeed(File.ReadAllBytes(SeedFilePath));

            // Primo avvio: genera una nuova seed phrase a 24 parole (256 bit di entropia, BIP39) —
            // su richiesta esplicita (24/09/2026), invece delle 12 di default.
            var mnemonic = new Mnemonic(Wordlist.English, WordCount.TwentyFour);
            string parole = mnemonic.ToString();

            File.WriteAllBytes(SeedFilePath, CifraSeed(parole));

            Log("========================================================================");
            Log(" NUOVA SEED PHRASE GENERATA — TRASCRIVILA SU CARTA E CONSERVALA OFFLINE!");
            Log($" {parole}");
            Log(" Questo messaggio compare SOLO ora, al primo avvio. Sul disco la seed è");
            Log(" salvata cifrata (mai in chiaro): se perdi sia il file cifrato sia questa");
            Log(" trascrizione, i fondi nel wallet sono persi per sempre.");
            Log("========================================================================");

            return parole;
        }

        private static byte[] CifraSeed(string testoInChiaro)
        {
            using var aes = new AesGcm(DerivaChiave(), AesGcm.TagByteSizes.MaxSize);
            byte[] nonce = RandomNumberGenerator.GetBytes(12);
            byte[] plainBytes = Encoding.UTF8.GetBytes(testoInChiaro);
            byte[] cipherBytes = new byte[plainBytes.Length];
            byte[] tag = new byte[16];

            aes.Encrypt(nonce, plainBytes, cipherBytes, tag);

            // Formato su disco: nonce (12 byte) | tag (16 byte) | ciphertext
            byte[] output = new byte[nonce.Length + tag.Length + cipherBytes.Length];
            Buffer.BlockCopy(nonce, 0, output, 0, nonce.Length);
            Buffer.BlockCopy(tag, 0, output, nonce.Length, tag.Length);
            Buffer.BlockCopy(cipherBytes, 0, output, nonce.Length + tag.Length, cipherBytes.Length);
            return output;
        }

        private static string DecifraSeed(byte[] datiCifrati)
        {
            byte[] nonce = datiCifrati[..12];
            byte[] tag = datiCifrati[12..28];
            byte[] cipherBytes = datiCifrati[28..];
            byte[] plainBytes = new byte[cipherBytes.Length];

            using var aes = new AesGcm(DerivaChiave(), AesGcm.TagByteSizes.MaxSize);
            aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

            return Encoding.UTF8.GetString(plainBytes);
        }

        private static byte[] DerivaChiave()
        {
            string passphrase = Environment.GetEnvironmentVariable("WW_SEED_PASSPHRASE") ?? Password.SEED_ENCRYPTION_PASSPHRASE;
            return SHA256.HashData(Encoding.UTF8.GetBytes(passphrase)); // passphrase testuale -> chiave AES-256 a 32 byte
        }

        #endregion

        #region ABI / funzioni tipizzate ERC20 (transfer, balanceOf, evento Transfer)

        [Function("transfer", "bool")]
        public class TransferFunction : FunctionMessage
        {
            [Parameter("address", "_to", 1)]
            public string To { get; set; }

            [Parameter("uint256", "_value", 2)]
            public BigInteger Value { get; set; }
        }

        [Function("balanceOf", "uint256")]
        public class BalanceOfFunction : FunctionMessage
        {
            [Parameter("address", "_owner", 1)]
            public string Owner { get; set; }
        }

        [Event("Transfer")]
        public class TransferEventDTO : IEventDTO
        {
            [Parameter("address", "from", 1, true)]
            public string From { get; set; }

            [Parameter("address", "to", 2, true)]
            public string To { get; set; }

            [Parameter("uint256", "value", 3, false)]
            public BigInteger Value { get; set; }
        }

        #endregion

        #region Modelli — Bill (pagamenti) e Prelievo

        /// <summary>
        /// Scontrino di un pagamento in USDT. Stesso schema pensato per GoogleManager (ricevute
        /// Play Store): orderId/playerId/itemId/status sono gli stessi campi, così le due fonti di
        /// acquisto (crypto e Play Store) restano facili da confrontare/loggare insieme.
        /// </summary>
        public class Bill
        {
            [JsonPropertyName("orderId")] public string OrderId { get; set; }
            [JsonPropertyName("playerId")] public string PlayerId { get; set; }
            [JsonPropertyName("itemId")] public string ItemId { get; set; }
            [JsonPropertyName("amount")] public decimal Amount { get; set; }          // prezzo "nominale" mostrato al giocatore, es. 5.00
            [JsonPropertyName("exactAmount")] public decimal ImportoEsatto { get; set; } // importo REALE da inviare, con salt nei decimali (vedi CreaBill)
            [JsonPropertyName("currency")] public string Currency { get; set; } = "USDT";
            [JsonPropertyName("chainId")] public int ChainId { get; set; }
            [JsonPropertyName("recipient")] public string Recipient { get; set; }
            // Wallet dichiarato dal giocatore come mittente del pagamento. Facoltativo: se noto (a
            // creazione bill o dichiarato insieme all'hash) aiuta a risalire alla transazione giusta
            // e a scartare depositi "sospetti" (stesso importo ma mittente diverso da quello atteso).
            [JsonPropertyName("walletMittente")] public string WalletMittente { get; set; }
            [JsonPropertyName("expiresAt")] public DateTimeOffset ExpiresAt { get; set; }
            [JsonPropertyName("status")] public string Status { get; set; } = "pending"; // pending | confirming | paid | expired | cancelled
            [JsonPropertyName("txHash")] public string TxHash { get; set; }
            [JsonPropertyName("createdAt")] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
            [JsonPropertyName("paidAt")] public DateTimeOffset? PaidAt { get; set; }
        }

        public enum StatoPrelievo { Pending, Inviato, Confermato, Fallito }

        public class Prelievo
        {
            [JsonPropertyName("id")] public string Id { get; set; }
            [JsonPropertyName("playerId")] public string PlayerId { get; set; }
            [JsonPropertyName("destinatario")] public string Destinatario { get; set; }
            [JsonPropertyName("amount")] public decimal Amount { get; set; }
            [JsonPropertyName("stato")] public StatoPrelievo Stato { get; set; } = StatoPrelievo.Pending;
            [JsonPropertyName("txHash")] public string TxHash { get; set; }
            [JsonPropertyName("errore")] public string Errore { get; set; }
            [JsonPropertyName("createdAt")] public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
            [JsonPropertyName("completatoAt")] public DateTimeOffset? CompletatoAt { get; set; }
        }

        #endregion

        #region Persistenza (stesso schema "scrittura atomica" di TokenManager)

        private static readonly string DataPath = Path.Combine(GameSave.SavePath, "Blockchain");
        private static readonly string BillsPath = Path.Combine(DataPath, "bills.json");
        private static readonly string PrelieviPath = Path.Combine(DataPath, "prelievi.json");
        private static readonly string StatoScansionePath = Path.Combine(DataPath, "stato_scansione.json");
        private static readonly string HashUsatiPath = Path.Combine(DataPath, "hash_usati.json");

        private static readonly ConcurrentDictionary<string, Bill> Bills = new();              // chiave: OrderId
        private static readonly ConcurrentDictionary<string, Prelievo> PrelieviAttivi = new();  // chiave: Id prelievo

        // Blacklist anti-riuso: ogni transazione, una volta accettata come pagamento di UNA bill,
        // non può più essere riutilizzata per confermarne un'altra (reclamo duplicato/manipolato
        // con lo stesso hash). Chiave: txHash, valore: OrderId della bill che l'ha "consumata".
        private static readonly ConcurrentDictionary<string, string> HashUsati = new();

        private static ulong _ultimoBloccoScansionato = 0;

        private static void SalvaJson<T>(string percorso, T dati)
        {
            try
            {
                string tempPath = percorso + ".tmp";
                File.WriteAllText(tempPath, JsonSerializer.Serialize(dati, new JsonSerializerOptions { WriteIndented = true }));
                File.Move(tempPath, percorso, overwrite: true); // scrittura atomica: mai un file a metà in caso di crash
            }
            catch (Exception ex)
            {
                Log($"[ERRORE] Salvataggio '{Path.GetFileName(percorso)}' fallito: {ex.Message}");
            }
        }

        private static void SalvaBills() => SalvaJson(BillsPath, Bills.Values.ToList());
        private static void SalvaPrelievi() => SalvaJson(PrelieviPath, PrelieviAttivi.Values.ToList());
        private static void SalvaStato() => SalvaJson(StatoScansionePath, new { UltimoBlocco = _ultimoBloccoScansionato });
        private static void SalvaHashUsati() => SalvaJson(HashUsatiPath, new Dictionary<string, string>(HashUsati));

        private static void CaricaBills()
        {
            if (!File.Exists(BillsPath)) return;
            try
            {
                var lista = JsonSerializer.Deserialize<List<Bill>>(File.ReadAllText(BillsPath)) ?? new();
                foreach (var b in lista) Bills[b.OrderId] = b;
                Log($"Caricate {Bills.Count} bill da disco");
            }
            catch (Exception ex) { Log($"[ERRORE] Caricamento bills.json fallito: {ex.Message}"); }
        }

        private static void CaricaPrelievi()
        {
            if (!File.Exists(PrelieviPath)) return;
            try
            {
                var lista = JsonSerializer.Deserialize<List<Prelievo>>(File.ReadAllText(PrelieviPath)) ?? new();
                foreach (var p in lista) PrelieviAttivi[p.Id] = p;
                Log($"Caricati {PrelieviAttivi.Count} prelievi da disco");
            }
            catch (Exception ex) { Log($"[ERRORE] Caricamento prelievi.json fallito: {ex.Message}"); }
        }

        private static void CaricaStato()
        {
            if (!File.Exists(StatoScansionePath)) return;
            try
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(StatoScansionePath));
                if (doc.RootElement.TryGetProperty("UltimoBlocco", out var el))
                    _ultimoBloccoScansionato = el.GetUInt64();
            }
            catch (Exception ex) { Log($"[ERRORE] Caricamento stato_scansione.json fallito: {ex.Message}"); }
        }

        private static void CaricaHashUsati()
        {
            if (!File.Exists(HashUsatiPath)) return;
            try
            {
                var dizionario = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(HashUsatiPath)) ?? new();
                foreach (var kv in dizionario) HashUsati[kv.Key] = kv.Value;
                Log($"Caricati {HashUsati.Count} hash già utilizzati (blacklist anti-riuso)");
            }
            catch (Exception ex) { Log($"[ERRORE] Caricamento hash_usati.json fallito: {ex.Message}"); }
        }

        #endregion

        #region Emissione Bill (pagamenti in entrata)

        // Quante "unità minime" di salt usare negli ultimi decimali dell'importo per renderlo
        // univoco (es. 5.00 -> 5.000123). Con 6 decimali USDT e un intervallo di 1-999 micro-unità
        // c'è margine per migliaia di bill "pending" contemporanee sullo stesso importo nominale
        // senza collisioni, senza dover appoggiarsi a uno smart contract di pagamento dedicato.
        private const int SALT_MIN = 1;
        private const int SALT_MAX = 999;

        // Tempo a disposizione del giocatore per completare il pagamento prima che la bill scada
        // (24/09/2026, su richiesta esplicita: 10 minuti erano pochi). Il countdown mostrato nel
        // popup di pagamento lato client parte da questo stesso valore (arriva con "ExpiresAt").
        private static readonly TimeSpan DURATA_BILL_DEFAULT = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Crea una nuova bill (scontrino) per un acquisto in USDT su Polygon. L'importo "esatto"
        /// (ImportoEsatto/exactAmount) è quello da mostrare e far inviare al giocatore per intero —
        /// è così che il monitoraggio dei depositi riconosce a quale bill appartiene un pagamento
        /// in arrivo, senza bisogno di un contratto di pagamento con memo on-chain.
        ///
        /// walletMittente è facoltativo: se il client lo conosce già (es. wallet collegato/Metamask)
        /// va passato subito, restringe la ricerca del deposito a un solo indirizzo mittente e rende
        /// più difficile che un pagamento "somigliante" (stesso importo, wallet diverso) sia
        /// scambiato per quello giusto. Se non lo si ha ancora, si può aggiungere dopo con
        /// DichiaraPagamentoAsync.
        /// </summary>
        public static Bill CreaBill(string playerId, string itemId, decimal amount, string walletMittente = null, TimeSpan? validita = null)
        {
            string orderId = "ord_" + Guid.NewGuid().ToString("N")[..12];

            var bill = new Bill
            {
                OrderId = orderId,
                PlayerId = playerId,
                ItemId = itemId,
                Amount = amount,
                ImportoEsatto = GeneraImportoUnivoco(amount),
                Currency = "USDT",
                ChainId = ChainId,
                Recipient = TreasuryAddress,
                WalletMittente = walletMittente,
                ExpiresAt = DateTimeOffset.UtcNow.Add(validita ?? DURATA_BILL_DEFAULT),
                Status = "pending"
            };

            Bills[orderId] = bill;
            SalvaBills();

            Log($"Bill creata: {orderId} — {playerId} deve inviare esattamente {bill.ImportoEsatto} USDT a {bill.Recipient} entro {bill.ExpiresAt:HH:mm:ss} UTC");
            return bill;
        }

        /// <summary>
        /// Punto d'ingresso unico per avviare un pagamento crypto legato a un item dello shop.
        /// L'importo NON arriva più da un catalogo interno a questa classe: lo decide Shop.cs,
        /// leggendolo da Variabili_Server.Shop.Catalogo (unica fonte di verità sui prezzi, sia per
        /// gli item in Diamanti che per quelli in USDT) — qui ci si fida solo dell'amount che Shop.cs
        /// ha già risolto server-side, mai di uno mandato dal client. Crea la bill, costruisce
        /// l'URI EIP-681 + il QR code, e manda tutto al client.
        /// </summary>
        public static void AvviaPagamentoItem(Guid clientGuid, Player player, string itemId, decimal amount, string walletMittente = null)
        {
            // 25/09/2026: se il wallet di tesoreria non si è inizializzato all'avvio (vedi
            // InizializzaAsync / GameSave.LoadData — un'eventuale eccezione lì viene solo loggata in
            // console, il server continua comunque a girare), TreasuryAddress resta null. Senza
            // questo controllo una bill con Recipient nullo passava comunque senza errori visibili in
            // superficie (l'URI mostrava solo un campo vuoto), salvo poi mandare in crash
            // GeneraQrCodeBase64 più sotto (QRCoder non tollera un testo nullo) — già successo,
            // corretto qui alla radice invece che tamponare solo il crash del QR.
            if (string.IsNullOrWhiteSpace(TreasuryAddress))
            {
                Log("[Pagamento] Impossibile avviare il pagamento: wallet di tesoreria non inizializzato — controlla i log di avvio del server per l'errore da InizializzaAsync (di solito una riga \"[LoadData] Errore durante il caricamento: ...\").");
                Send(clientGuid, "Log_Server|Il sistema di pagamento non è al momento disponibile. Riprova più tardi o contatta il supporto.");
                return;
            }

            var bill = CreaBill(player.Username, itemId, amount, walletMittente);

            string uri = CostruisciUriPagamento(bill);

            // 24-25/09/2026, su segnalazione esplicita dell'utente: l'app "ULT" di Crypto.com (e la
            // maggior parte dei wallet "custodial" da exchange — Binance, Coinbase, ecc.) scansiona
            // il QR aspettandosi un INDIRIZZO semplice per il loro flusso "Invia", non un URI
            // EIP-681 con chiamata al contratto ("ethereum:0xContratto@137/transfer?address=...").
            // Vedendo l'indirizzo del CONTRATTO USDT al posto di un indirizzo wallet riconoscibile,
            // questi wallet rifiutano il QR invece di ignorare educatamente i parametri che non
            // capiscono ("non siamo in grado di identificare l'indirizzo del wallet...", l'errore
            // segnalato). MetaMask/Trust Wallet & co. invece leggono bene l'URI completo e
            // precompilano anche importo e token. Non essendoci un QR che vada bene per entrambe le
            // categorie, il server genera ENTRAMBI: il client mostra quello completo per default
            // (più comodo quando funziona) con un pulsante per passare a quello "solo indirizzo" se
            // il wallet del giocatore si lamenta, invece di dover scegliere a priori.
            string qrCompletoBase64 = GeneraQrCodeBase64(uri);
            string qrSempliceBase64 = GeneraQrCodeBase64(bill.Recipient);

            Send(clientGuid,
                $"Pagamento|Creato|{bill.OrderId}|{bill.ItemId}|{bill.ImportoEsatto.ToString(CultureInfo.InvariantCulture)}|{bill.Recipient}|{bill.ChainId}|{bill.ExpiresAt:o}|{uri}|{qrCompletoBase64}|{qrSempliceBase64}");
        }

        private static decimal GeneraImportoUnivoco(decimal baseAmount)
        {
            decimal candidato;
            int tentativi = 0;
            do
            {
                int salt = Random.Shared.Next(SALT_MIN, SALT_MAX + 1);
                candidato = baseAmount + (salt / 1_000_000m); // salt nella 4a-6a cifra decimale (micro-USDT)
                tentativi++;
            }
            while (tentativi < 50 && Bills.Values.Any(b => b.Status == "pending" && b.ImportoEsatto == candidato));

            return candidato;
        }

        /// <summary>Annulla manualmente una bill ancora in sospeso (es. il giocatore ha cambiato idea, o l'item non è più disponibile).</summary>
        public static bool AnnullaBill(string orderId)
        {
            if (!Bills.TryGetValue(orderId, out var bill) || bill.Status != "pending") return false;
            bill.Status = "cancelled";
            SalvaBills();
            return true;
        }

        public static Bill OttieniBill(string orderId) => Bills.GetValueOrDefault(orderId);

        public static List<Bill> ListaBillGiocatore(string playerId) =>
            Bills.Values.Where(b => b.PlayerId == playerId).OrderByDescending(b => b.CreatedAt).ToList();

        #endregion

        #region QR code e dati per il pagamento manuale

        /// <summary>
        /// URI standard EIP-681 per un trasferimento ERC20 (es. "ethereum:0xContratto@137/transfer
        /// ?address=0xDestinatario&amp;uint256=5000123"). Diversi wallet mobile (MetaMask, Trust
        /// Wallet, tra gli altri) lo riconoscono via QR o deep-link e precompilano destinatario e
        /// importo — il wallet mostra comunque sempre i dati prima di far firmare la transazione,
        /// quindi l'utente può sempre ricontrollarli e completare (o annullare) manualmente. Non
        /// tutti i wallet lo supportano: per questo la bill riporta anche i dati "grezzi"
        /// (destinatario/importo/rete) per un invio del tutto manuale.
        /// </summary>
        public static string CostruisciUriPagamento(Bill bill)
        {
            BigInteger importoUnitaMinime = UnitConversion.Convert.ToWei(bill.ImportoEsatto, USDT_DECIMALS);
            return $"ethereum:{UsdtContract}@{bill.ChainId}/transfer?address={bill.Recipient}&uint256={importoUnitaMinime}";
        }

        /// <summary>Genera un QR (PNG) per il testo indicato, restituito come stringa base64 — pronto per un &lt;img src="data:image/png;base64,..."&gt; lato client.</summary>
        public static string GeneraQrCodeBase64(string testo)
        {
            using var generatore = new QRCodeGenerator();
            using var datiQr = generatore.CreateQrCode(testo, QRCodeGenerator.ECCLevel.Q);
            var qr = new PngByteQRCode(datiQr);
            byte[] png = qr.GetGraphic(10); // 10px per modulo: leggibile anche su schermi piccoli, senza essere enorme in byte
            return Convert.ToBase64String(png);
        }

        #endregion

        // La region "PolygonScan" (letture HTTP: blocco attuale, depositi, saldi) è stata spostata
        // nel file BlockchainManager.PolygonScan.cs — stessa classe (partial), solo file diverso,
        // per tenere separato il codice HTTP (PolygonScan) da quello RPC/firma (_web3/Nethereum)
        // qui sotto, su richiesta esplicita dopo la confusione fatta dal mescolare i due approcci.

        #region Pagamenti — monitoraggio depositi on-chain

        /// <summary>
        /// Percorso "rapido": il giocatore dichiara subito playerId (tramite la bill), wallet
        /// mittente e hash della transazione appena inviata — evita di dover aspettare il prossimo
        /// giro di scansione a blocchi. L'hash viene verificato davvero on-chain (non ci si fida
        /// del solo numero passato dal client) e riservato in blacklist in modo atomico, così due
        /// bill non possono mai essere confermate con lo stesso hash (reclamo duplicato/manipolato).
        /// Se il giocatore non può fornire subito l'hash, non serve chiamare questo metodo: prima o
        /// poi ci arriva comunque ScansionaDepositiAsync cercando per wallet mittente + importo.
        /// </summary>
        public static async Task<(bool ok, string errore)> DichiaraPagamentoAsync(string orderId, string txHash, string walletMittente = null)
        {
            if (!Bills.TryGetValue(orderId, out var bill) || bill.Status != "pending")
                return (false, "Bill non trovata o non più in attesa di pagamento");

            if (string.IsNullOrWhiteSpace(txHash) || !txHash.StartsWith("0x") || txHash.Length != 66)
                return (false, "Hash di transazione non valido");

            // Riserva atomica: se l'hash è già stato usato per un'altra bill (o è già "in verifica"
            // per un'altra richiesta concorrente), questo fallisce subito — è qui che si blocca un
            // tentativo di riutilizzare la stessa transazione per reclamare più acquisti.
            if (!HashUsati.TryAdd(txHash, orderId))
                return (false, "Questo hash di transazione è già stato usato per un altro pagamento");

            try
            {
                var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txHash);
                if (receipt == null)
                {
                    HashUsati.TryRemove(txHash, out _);
                    return (false, "Transazione non trovata sulla rete (non ancora propagata/minata?)");
                }

                var eventoTransfer = _web3.Eth.GetEvent<TransferEventDTO>(UsdtContract);
                var decodificati = eventoTransfer.DecodeAllEventsForEvent(receipt.Logs);

                var match = decodificati.FirstOrDefault(e =>
                    string.Equals(e.Event.To, TreasuryAddress, StringComparison.OrdinalIgnoreCase) &&
                    UnitConversion.Convert.FromWei(e.Event.Value, USDT_DECIMALS) == bill.ImportoEsatto &&
                    (walletMittente == null || string.Equals(e.Event.From, walletMittente, StringComparison.OrdinalIgnoreCase)));

                if (match == null)
                {
                    HashUsati.TryRemove(txHash, out _);
                    return (false, "La transazione non corrisponde a questa bill (destinatario, importo o mittente non combaciano)");
                }

                bill.TxHash = txHash;
                bill.WalletMittente ??= match.Event.From;
                bill.Status = "confirming"; // diventa "paid" solo dopo CONFERME_MINIME, come i depositi trovati dalla scansione
                SalvaBills();
                SalvaHashUsati();

                Log($"Pagamento dichiarato per bill {bill.OrderId}: tx {txHash} verificata on-chain — in attesa di {CONFERME_MINIME} conferme");
                return (true, null);
            }
            catch (Exception ex)
            {
                HashUsati.TryRemove(txHash, out _);
                Log($"[ERRORE] Verifica dichiarazione pagamento {orderId} fallita: {ex.Message}");
                return (false, $"Errore durante la verifica on-chain: {ex.Message}");
            }
        }

        private static async Task CicloMonitoraggioAsync()
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(INTERVALLO_SCANSIONE_SECONDI));
            while (await timer.WaitForNextTickAsync())
            {
                try { await ScansionaDepositiAsync(); }
                catch (Exception ex) { Log($"[ERRORE] Scansione depositi: {ex.Message}"); }

                try { await ConfermaPrelieviAsync(); }
                catch (Exception ex) { Log($"[ERRORE] Conferma prelievi: {ex.Message}"); }

                ScadiBillVecchie();
            }
        }

        // ScansionaDepositiAsync è spostata in BlockchainManager.PolygonScan.cs (usa solo chiamate
        // HTTP a PolygonScan, non tocca _web3/RPC) — cerca lì se stai seguendo il flusso di
        // CicloMonitoraggioAsync qui sopra.

        // Le bill "confirming" diventano "paid" solo dopo CONFERME_MINIME blocchi, per proteggersi
        // da una reorg che "cancellerebbe" un deposito già considerato buono.
        private static async Task ConfermaBillInCorsoAsync(ulong bloccoAttuale)
        {
            bool modificato = false;

            foreach (var bill in Bills.Values.Where(b => b.Status == "confirming"))
            {
                var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(bill.TxHash);
                if (receipt?.BlockNumber == null) continue;

                ulong conferme = bloccoAttuale - (ulong)receipt.BlockNumber.Value;
                if (conferme < CONFERME_MINIME) continue;

                bill.Status = "paid";
                bill.PaidAt = DateTimeOffset.UtcNow;
                modificato = true;

                Log($"Bill {bill.OrderId} CONFERMATA ({conferme} blocchi) — accredito: {bill.ItemId} a {bill.PlayerId}");

                // La parte "blockchain" finisce qui: il pagamento è confermato ed ormai irreversibile.
                // L'accredito vero e proprio (quale valuta/oggetto, con quale logica) resta deciso da
                // Shop.cs, unica fonte di verità sul catalogo — qui ci si limita a notificarlo.
                try
                {
                    Shop.AccreditaAcquisto(bill.PlayerId, bill.ItemId);
                }
                catch (Exception ex)
                {
                    Log($"[ERRORE] Accredito fallito per bill {bill.OrderId} ({bill.PlayerId}, {bill.ItemId}): {ex.Message}");
                }

                // Su richiesta esplicita: il controllo di svuotamento/buffer del wallet va fatto
                // subito dopo la validazione di ogni transazione in ingresso, non su un timer a parte.
                await ControllaESvuotaTesoreriaAsync();
            }

            if (modificato) SalvaBills();
        }

        private static void ScadiBillVecchie()
        {
            bool modificato = false;
            foreach (var bill in Bills.Values.Where(b => b.Status == "pending" && b.ExpiresAt < DateTimeOffset.UtcNow))
            {
                bill.Status = "expired";
                modificato = true;
            }
            if (modificato) SalvaBills();
        }

        #endregion

        #region Prelievi (pagamenti in uscita)

        private const decimal PRELIEVO_MINIMO = 1.00m;           // USDT
        private const decimal LIMITE_GIORNALIERO_PLAYER = 500m;  // USDT, per singolo giocatore

        /// <summary>
        /// Registra una richiesta di prelievo dopo i controlli di base — incluso, dal 23/09/2026,
        /// il controllo/scalo dei Tributi (Dollari_Virtuali) del giocatore: 1 Tributo = 1 USDT
        /// (stesso presupposto usato altrove, vedi CalcolaBufferObiettivo). Lo scalo avviene qui,
        /// SUBITO, prima di autorizzare qualunque invio reale — se poi l'invio fallisce davvero
        /// (EseguiPrelievoAsync), i Tributi vengono restituiti al giocatore (vedi RimborsaPrelievoFallito).
        /// NON invia subito la transazione: la firma/invio avviene in EseguiPrelievoAsync, così è
        /// possibile inserire (fuori da questa classe) una revisione manuale per gli importi più
        /// grandi prima che partano davvero i fondi.
        /// </summary>
        public static (bool ok, string errore, Prelievo prelievo) RichiediPrelievo(Player player, string destinatario, decimal amount)
        {
            if (!IndirizzoValido(destinatario))
                return (false, "Indirizzo Polygon non valido", null);

            if (amount < PRELIEVO_MINIMO)
                return (false, $"Importo minimo di prelievo: {PRELIEVO_MINIMO} USDT", null);

            string playerId = player.Username;

            decimal giaPrelevatoOggi = TotalePrelevatoOggi(playerId);
            if (giaPrelevatoOggi + amount > LIMITE_GIORNALIERO_PLAYER)
                return (false, $"Limite giornaliero di prelievo superato ({LIMITE_GIORNALIERO_PLAYER} USDT/giorno)", null);

            if (player.Dollari_Virtuali < amount)
                return (false, $"Tributi insufficienti: disponibili {player.Dollari_Virtuali.ToString(CultureInfo.InvariantCulture)}, richiesti {amount.ToString(CultureInfo.InvariantCulture)}", null);

            // Scalati subito — non aspettiamo la conferma on-chain, altrimenti nella finestra fra
            // richiesta ed esecuzione il giocatore potrebbe spendere/richiedere di nuovo gli stessi Tributi.
            player.Dollari_Virtuali -= amount;

            var prelievo = new Prelievo
            {
                Id = "wd_" + Guid.NewGuid().ToString("N")[..12],
                PlayerId = playerId,
                Destinatario = destinatario,
                Amount = amount,
                Stato = StatoPrelievo.Pending
            };

            PrelieviAttivi[prelievo.Id] = prelievo;
            SalvaPrelievi();

            Log($"Richiesta prelievo {prelievo.Id}: {playerId} → {destinatario}, {amount} USDT (Tributi scalati)");
            return (true, null, prelievo);
        }

        /// <summary>Restituisce al giocatore i Tributi scalati da RichiediPrelievo quando l'invio reale poi fallisce (saldo tesoreria insufficiente, errore di rete/contratto, ecc.).</summary>
        private static void RimborsaPrelievoFallito(Prelievo prelievo)
        {
            var giocatore = servers_.GetPlayer(prelievo.PlayerId);
            if (giocatore != null)
            {
                giocatore.Dollari_Virtuali += prelievo.Amount;
                Log($"Tributi rimborsati a {prelievo.PlayerId}: +{prelievo.Amount.ToString(CultureInfo.InvariantCulture)} (prelievo {prelievo.Id} fallito)");
            }
            else
            {
                // Giocatore non in memoria (non dovrebbe capitare: i Player restano nel dizionario
                // servers_ anche da disconnessi) — lo segnaliamo forte perché senza questo log
                // il giocatore perderebbe Tributi veri senza che nessuno se ne accorga.
                Log($"[ATTENZIONE] Rimborso Tributi fallito per il prelievo {prelievo.Id}: giocatore \"{prelievo.PlayerId}\" non trovato in memoria. Rimborso manuale necessario.");
            }
        }

        /// <summary>
        /// Firma e invia realmente il prelievo sulla rete. Il controllo/scalo dei Tributi del
        /// giocatore è già avvenuto in RichiediPrelievo, prima che questo metodo venga chiamato;
        /// se l'invio reale fallisce qui (saldo tesoreria insufficiente o errore), i Tributi
        /// vengono restituiti al giocatore (RimborsaPrelievoFallito) — altrimenti li perderebbe
        /// senza aver ricevuto nulla in cambio.
        /// </summary>
        public static async Task<bool> EseguiPrelievoAsync(string prelievoId)
        {
            if (!PrelieviAttivi.TryGetValue(prelievoId, out var prelievo) || prelievo.Stato != StatoPrelievo.Pending)
                return false;

            try
            {
                decimal saldoTesoreria = await SaldoTesoreriaAsync();
                if (saldoTesoreria < prelievo.Amount)
                {
                    prelievo.Stato = StatoPrelievo.Fallito;
                    prelievo.Errore = "Saldo insufficiente nel wallet di tesoreria";
                    SalvaPrelievi();
                    RimborsaPrelievoFallito(prelievo);
                    Log($"[ERRORE] Prelievo {prelievoId} fallito: saldo tesoreria insufficiente ({saldoTesoreria} USDT)");
                    return false;
                }

                BigInteger importoWei = UnitConversion.Convert.ToWei(prelievo.Amount, USDT_DECIMALS);

                var handler = _web3.Eth.GetContractTransactionHandler<TransferFunction>();
                var receipt = await handler.SendRequestAndWaitForReceiptAsync(UsdtContract, new TransferFunction
                {
                    To = prelievo.Destinatario,
                    Value = importoWei,
                    FromAddress = TreasuryAddress
                });

                prelievo.TxHash = receipt.TransactionHash;
                prelievo.Stato = StatoPrelievo.Inviato;
                SalvaPrelievi();

                Log($"Prelievo {prelievoId} inviato: tx {receipt.TransactionHash}");
                return true;
            }
            catch (Exception ex)
            {
                prelievo.Stato = StatoPrelievo.Fallito;
                prelievo.Errore = ex.Message;
                SalvaPrelievi();
                RimborsaPrelievoFallito(prelievo);
                Log($"[ERRORE] Prelievo {prelievoId} fallito: {ex.Message}");
                return false;
            }
        }

        // Le richieste già "Inviato" (transazione mandata, receipt ottenuto) diventano "Confermato"
        // solo dopo CONFERME_MINIME blocchi aggiuntivi, stessa logica anti-reorg dei depositi.
        private static async Task ConfermaPrelieviAsync()
        {
            var inviati = PrelieviAttivi.Values.Where(p => p.Stato == StatoPrelievo.Inviato).ToList();
            if (inviati.Count == 0) return;

            ulong bloccoAttuale = (ulong)(await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync()).Value;
            bool modificato = false;

            foreach (var prelievo in inviati)
            {
                var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(prelievo.TxHash);
                if (receipt?.BlockNumber == null) continue;

                if (receipt.Status != null && receipt.Status.Value == 0) // transazione fallita on-chain (revert)
                {
                    prelievo.Stato = StatoPrelievo.Fallito;
                    prelievo.Errore = "Transazione fallita on-chain (revert)";
                    modificato = true;
                    Log($"[ERRORE] Prelievo {prelievo.Id}: tx {prelievo.TxHash} revertita on-chain");
                    continue;
                }

                ulong conferme = bloccoAttuale - (ulong)receipt.BlockNumber.Value;
                if (conferme < CONFERME_MINIME) continue;

                prelievo.Stato = StatoPrelievo.Confermato;
                prelievo.CompletatoAt = DateTimeOffset.UtcNow;
                modificato = true;
                Log($"Prelievo {prelievo.Id} CONFERMATO ({conferme} blocchi)");
            }

            if (modificato) SalvaPrelievi();
        }

        private static decimal TotalePrelevatoOggi(string playerId)
        {
            var oggi = DateTimeOffset.UtcNow.Date;
            return PrelieviAttivi.Values
                .Where(p => p.PlayerId == playerId && p.CreatedAt.Date == oggi && p.Stato != StatoPrelievo.Fallito)
                .Sum(p => p.Amount);
        }

        public static List<Prelievo> ListaPrelieviGiocatore(string playerId) =>
            PrelieviAttivi.Values.Where(p => p.PlayerId == playerId).OrderByDescending(p => p.CreatedAt).ToList();

        #endregion

        #region Controlli

        public static bool IndirizzoValido(string indirizzo)
        {
            if (string.IsNullOrWhiteSpace(indirizzo)) return false;
            return new AddressUtil().IsValidEthereumAddressHexFormat(indirizzo);
        }

        // SaldoUsdtAsync / SaldoTesoreriaAsync / SaldoNativoAsync sono spostati in
        // BlockchainManager.PolygonScan.cs (usano solo HTTP, non _web3/RPC) — stessa classe
        // (partial), solo file diverso.

        #endregion

        #region Tesoreria — buffer obiettivo, svuotamento automatico, notifiche mail

        // Wallet "freddo"/secondario a cui spostare l'eccedenza rispetto al buffer obiettivo.
        // Lasciato vuoto di proposito: finché non viene impostato, SvuotaEccedenzaAsync si limita a
        // loggare (nessun trasferimento automatico) — imposta qui l'indirizzo scelto quando lo hai.
        private const string WALLET_SECONDARIO = "0xCF10CaA8e699B8089e408a6980d47672fFA99b3b";

        // Margine di sicurezza aggiunto sopra il buffer obiettivo "grezzo" (vedi CalcolaBufferObiettivo), es. 0.20 = +20%.
        private const decimal MARGINE_SICUREZZA_BUFFER = 0.15m;

        // Il volume di riferimento per il buffer è la media generata nelle ultime 24h moltiplicata
        // per questi giorni di autonomia — copre qualche giorno di traffico senza intervento manuale.
        private const int GIORNI_MARGINE_VOLUME = 5;

        private const decimal SOGLIA_MINIMA_SWEEP = 20m; // Sotto questa eccedenza non vale la pena spostare fondi (rumore/gas inutile per pochi USDT).

        // Soglia di allarme sul gas nativo (POL): sotto questo valore il wallet rischia di non
        // riuscire più a pagare il gas dei prelievi/sweep (i depositi in arrivo non costano gas al
        // wallet di tesoreria, solo le transazioni che MANDA). Margine ampio di partenza, da
        // aggiustare in base al traffico reale una volta visto il gas medio speso.
        private const decimal SOGLIA_POL_BASSA = 5m;

        // Cooldown anti-spam: ControllaESvuotaTesoreriaAsync gira ad ogni bill confermata, quindi
        // senza un limite lo stesso alert (se la condizione resta vera) partirebbe ad ogni deposito.
        private static readonly TimeSpan COOLDOWN_ALERT = TimeSpan.FromHours(6);
        private static DateTime _ultimoAlertFondiScarsi = DateTime.MinValue;
        private static DateTime _ultimoAlertPolBasso = DateTime.MinValue;

        /// <summary>Somma degli importi delle bill USDT confermate ("paid") con PaidAt nelle ultime 24 ore — unico dato affidabile sul traffico reale (le "pending"/"confirming" potrebbero non concludersi mai).</summary>
        public static decimal VolumeGenerato24h() =>
            Bills.Values.Where(b => b.Status == "paid" && b.PaidAt.HasValue && b.PaidAt.Value >= DateTimeOffset.UtcNow.AddHours(-24))
                        .Sum(b => b.ImportoEsatto);

        /// <summary>
        /// Somma dei Tributi (Player.Dollari_Virtuali) di TUTTI i giocatori registrati: è il massimo
        /// che potrebbero prelevare tutti insieme in questo momento. Presuppone 1 Dollaro Virtuale =
        /// 1 USDT — stesso presupposto già implicito in Variabili_Server.prelievo_Minimo confrontato
        /// direttamente con i Tributi; se la conversione reale è diversa, va corretta qui. Restituisce
        /// anche la media per giocatore, solo per log/diagnostica (vedi richiesta esplicita).
        /// </summary>
        public static (decimal totale, decimal media, int giocatori) TributiGiocatori()
        {
            var giocatori = servers_.GetAllPlayers().ToList();
            if (giocatori.Count == 0) return (0m, 0m, 0);
            decimal totale = giocatori.Sum(p => p.Dollari_Virtuali);
            return (totale, totale / giocatori.Count, giocatori.Count);
        }

        /// <summary>
        /// Buffer "obiettivo" da tenere nel wallet caldo: il più alto tra (a) la liability totale dei
        /// Tributi di tutti i giocatori — se tutti prelevassero insieme — e (b) il volume medio delle
        /// ultime 24h × GIORNI_MARGINE_VOLUME, con sopra MARGINE_SICUREZZA_BUFFER. Copre sia un "bank
        /// run" sia un picco di traffico, senza tenere fermo più del necessario nel wallet più esposto
        /// (quello che firma da solo — vedi discorso fatto sul rischio di una VPS compromessa).
        /// </summary>
        public static (decimal target, decimal liabilityTributi, decimal mediaTributi, decimal volume24h) CalcolaBufferObiettivo()
        {
            var (liabilityTributi, mediaTributi, _) = TributiGiocatori();
            decimal volume24h = VolumeGenerato24h();
            decimal baseBuffer = Math.Max(liabilityTributi, volume24h * GIORNI_MARGINE_VOLUME);
            decimal target = baseBuffer * (1 + MARGINE_SICUREZZA_BUFFER);
            return (target, liabilityTributi, mediaTributi, volume24h);
        }

        /// <summary>
        /// Da chiamare subito dopo che una bill in entrata viene confermata ("paid") — vedi
        /// ConfermaBillInCorsoAsync. Confronta il saldo del wallet di tesoreria col buffer obiettivo:
        /// se c'è eccedenza sopra SOGLIA_MINIMA_SWEEP la sposta verso WALLET_SECONDARIO (quando
        /// impostato) e avvisa via mail; se il saldo è SOTTO il buffer avvisa che i fondi sono scarsi.
        /// Controlla anche il gas nativo (POL). Non propaga mai eccezioni: un problema qui non deve
        /// interrompere la conferma del deposito che lo ha innescato.
        /// </summary>
        public static async Task ControllaESvuotaTesoreriaAsync()
        {
            try
            {
                var (target, liabilityTributi, mediaTributi, volume24h) = CalcolaBufferObiettivo();
                decimal saldoAttuale = await SaldoTesoreriaAsync();

                Log($"[Tesoreria] Saldo: {saldoAttuale} USDT — buffer obiettivo: {target:0.00} USDT (liability Tributi: {liabilityTributi:0.00}, media/giocatore: {mediaTributi:0.0000}, volume 24h: {volume24h:0.00})");

                decimal eccedenza = saldoAttuale - target;

                if (eccedenza > SOGLIA_MINIMA_SWEEP)
                    await SvuotaEccedenzaAsync(eccedenza, saldoAttuale, target);
                else if (saldoAttuale < target)
                    await AvvisaFondiScarsiAsync(saldoAttuale, target, liabilityTributi, mediaTributi, volume24h);

                await ControllaGasNativoAsync();
            }
            catch (Exception ex)
            {
                Log($"[ERRORE] Controllo tesoreria fallito: {ex.Message}");
            }
        }

        private static async Task SvuotaEccedenzaAsync(decimal eccedenza, decimal saldoPrimaTransfer, decimal target)
        {
            if (string.IsNullOrWhiteSpace(WALLET_SECONDARIO) || !IndirizzoValido(WALLET_SECONDARIO))
            {
                Log($"[Tesoreria] Eccedenza di {eccedenza:0.00} USDT sopra il buffer obiettivo, ma WALLET_SECONDARIO non è ancora impostato: nessun trasferimento automatico.");
                return;
            }

            try
            {
                BigInteger importoWei = UnitConversion.Convert.ToWei(eccedenza, USDT_DECIMALS);
                var handler = _web3.Eth.GetContractTransactionHandler<TransferFunction>();
                var receipt = await handler.SendRequestAndWaitForReceiptAsync(UsdtContract, new TransferFunction
                {
                    To = WALLET_SECONDARIO,
                    Value = importoWei,
                    FromAddress = TreasuryAddress
                });

                Log($"[Tesoreria] Svuotamento eseguito: {eccedenza:0.00} USDT → {WALLET_SECONDARIO} (tx {receipt.TransactionHash})");

                await InviaAlertAdminAsync(
                    "✅ Warrior and Wealth — Fondi spostati verso il wallet secondario",
                    $@"<h2>Svuotamento eseguito</h2>
                    <p>Il wallet di gioco aveva un'eccedenza sopra il buffer obiettivo ed è stata spostata automaticamente.</p>
                    <ul>
                      <li>Importo trasferito: <strong>{eccedenza:0.00} USDT</strong></li>
                      <li>Saldo prima del trasferimento: {saldoPrimaTransfer:0.00} USDT</li>
                      <li>Buffer obiettivo mantenuto nel wallet di gioco: {target:0.00} USDT</li>
                      <li>Destinatario: <code>{WALLET_SECONDARIO}</code></li>
                      <li>Transazione: <code>{receipt.TransactionHash}</code></li>
                    </ul>");
            }
            catch (Exception ex)
            {
                Log($"[ERRORE] Svuotamento tesoreria fallito: {ex.Message}");
                await InviaAlertAdminAsync(
                    "⚠️ Warrior and Wealth — Svuotamento tesoreria FALLITO",
                    $@"<h2>Svuotamento fallito</h2>
                    <p>Tentativo di spostare {eccedenza:0.00} USDT verso il wallet secondario non riuscito.</p>
                    <p>Errore: {EscapeHtmlLocale(ex.Message)}</p>");
            }
        }

        private static async Task AvvisaFondiScarsiAsync(decimal saldoAttuale, decimal target, decimal liabilityTributi, decimal mediaTributi, decimal volume24h)
        {
            if (DateTime.UtcNow - _ultimoAlertFondiScarsi < COOLDOWN_ALERT) return; // niente spam ad ogni deposito
            _ultimoAlertFondiScarsi = DateTime.UtcNow;

            decimal saldoNativo = await SaldoNativoAsync();

            await InviaAlertAdminAsync(
                "⚠️ Warrior and Wealth — Fondi USDT sotto il buffer consigliato",
                $@"<h2>Fondi USDT scarsi</h2>
                <p>Il saldo USDT del wallet di gioco è sotto il buffer obiettivo consigliato (calcolato dal traffico e dai Tributi dei giocatori).</p>
                <ul>
                  <li>Saldo attuale: <strong>{saldoAttuale:0.00} USDT</strong></li>
                  <li>Buffer consigliato: <strong>{target:0.00} USDT</strong></li>
                  <li>Importo consigliato da aggiungere: <strong>{Math.Max(0, target - saldoAttuale):0.00} USDT</strong></li>
                  <li>Liability Tributi totale giocatori: {liabilityTributi:0.00} USDT</li>
                  <li>Media Tributi per giocatore: {mediaTributi:0.0000} USDT</li>
                  <li>Volume generato ultime 24h: {volume24h:0.00} USDT</li>
                  <li>Saldo POL (gas) attuale: {saldoNativo:0.0000} POL</li>
                </ul>
                <p>Trasferisci USDT dal wallet freddo verso l'indirizzo di tesoreria: <code>{TreasuryAddress}</code></p>");
        }

        private static async Task ControllaGasNativoAsync()
        {
            decimal saldoNativo = await SaldoNativoAsync();
            if (saldoNativo >= SOGLIA_POL_BASSA) return;
            if (DateTime.UtcNow - _ultimoAlertPolBasso < COOLDOWN_ALERT) return;
            _ultimoAlertPolBasso = DateTime.UtcNow;

            await InviaAlertAdminAsync(
                "⚠️ Warrior and Wealth — POL (gas) scarso nel wallet di gioco",
                $@"<h2>Gas (POL) scarso</h2>
                <p>Il wallet di tesoreria ha meno di {SOGLIA_POL_BASSA} POL: senza gas non può più firmare prelievi o svuotamenti verso il wallet secondario.</p>
                <ul>
                  <li>Saldo POL attuale: <strong>{saldoNativo:0.0000} POL</strong></li>
                  <li>Soglia minima consigliata: {SOGLIA_POL_BASSA} POL</li>
                  <li>Indirizzo: <code>{TreasuryAddress}</code></li>
                </ul>
                <p>Invia POL all'indirizzo sopra il prima possibile.</p>");
        }

        private static Task InviaAlertAdminAsync(string oggetto, string corpoHtml)
        {
            if (string.IsNullOrWhiteSpace(Password.ADMIN_ALERT_EMAIL))
            {
                Log("[Tesoreria] ADMIN_ALERT_EMAIL non impostata in Password.cs: alert non inviato via mail (solo su log).");
                return Task.CompletedTask;
            }
            return EmailManager.SendEmailAsync(Password.ADMIN_ALERT_EMAIL, "Warrior and Wealth — Tesoreria", oggetto, corpoHtml);
        }

        private static string EscapeHtmlLocale(string s) => s?.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;") ?? "";

        #endregion

        #region Wire protocol — comandi "Pagamento" e "Prelievo" dal client

        /// <summary>
        /// Dispatch per il comando "Pagamento" (vedi ServerConnection.cs, case "Pagamento").
        /// Sotto-azioni: Crea | DichiaraHash | Stato | Annulla | Lista.
        /// </summary>
        public static async Task GestisciComando(string[] msgArgs, Guid clientGuid, Player player)
        {
            if (msgArgs.Length < 4)
            {
                Send(clientGuid, "Log_Server|Comando non valido. Usa: Pagamento|<azione>|<parametri>");
                return;
            }

            switch (msgArgs[3])
            {
                case "Crea":
                {
                    // Pagamento|token|Crea|<itemId>|<walletMittente?>
                    // La richiesta d'acquisto passa ora sempre da Shop.cs (Shop_Call), che distingue
                    // con un if se l'item è in Diamanti o in USDT (vedi Variabili_Server.Shop.Catalogo
                    // + Valuta) e instrada di conseguenza — questo comando non tocca più prezzi o
                    // piattaforma direttamente, per avere un solo punto che decide come vendere un item.
                    if (msgArgs.Length < 5)
                    {
                        Send(clientGuid, "Log_Server|Parametri insufficienti. Usa: Pagamento|Crea|<itemId>|<walletMittente?>");
                        return;
                    }

                    string itemId = msgArgs[4];
                    string walletMittente = msgArgs.Length > 5 && !string.IsNullOrWhiteSpace(msgArgs[5]) ? msgArgs[5] : null;

                    Shop.Shop_Call(clientGuid, player, itemId, walletMittente);
                    break;
                }

                case "DichiaraHash":
                {
                    // Pagamento|token|DichiaraHash|<orderId>|<txHash>|<walletMittente?>
                    if (msgArgs.Length < 6)
                    {
                        Send(clientGuid, "Log_Server|Parametri insufficienti. Usa: Pagamento|DichiaraHash|<orderId>|<txHash>|<walletMittente?>");
                        return;
                    }

                    string orderId = msgArgs[4];
                    string txHash = msgArgs[5];
                    string walletMittente = msgArgs.Length > 6 && !string.IsNullOrWhiteSpace(msgArgs[6]) ? msgArgs[6] : null;

                    var bill = OttieniBill(orderId);
                    if (bill == null || bill.PlayerId != player.Username)
                    {
                        Send(clientGuid, "Log_Server|Bill non trovata.");
                        return;
                    }

                    var (ok, errore) = await DichiaraPagamentoAsync(orderId, txHash, walletMittente);
                    Send(clientGuid, ok ? $"Pagamento|Verificato|{orderId}" : $"Log_Server|{errore}");
                    break;
                }

                case "Stato":
                {
                    // Pagamento|token|Stato|<orderId>
                    if (msgArgs.Length < 5) return;
                    var bill = OttieniBill(msgArgs[4]);
                    if (bill == null || bill.PlayerId != player.Username)
                    {
                        Send(clientGuid, "Log_Server|Bill non trovata.");
                        return;
                    }
                    Send(clientGuid, $"Pagamento|Stato|{bill.OrderId}|{bill.Status}|{bill.TxHash}");
                    break;
                }

                case "Annulla":
                {
                    // Pagamento|token|Annulla|<orderId>
                    if (msgArgs.Length < 5) return;
                    string orderId = msgArgs[4];
                    var bill = OttieniBill(orderId);
                    if (bill == null || bill.PlayerId != player.Username)
                    {
                        Send(clientGuid, "Log_Server|Bill non trovata.");
                        return;
                    }
                    Send(clientGuid, AnnullaBill(orderId) ? $"Pagamento|Annullato|{orderId}" : "Log_Server|Impossibile annullare questa bill.");
                    break;
                }

                case "Lista":
                    foreach (var bill in ListaBillGiocatore(player.Username))
                        Send(clientGuid, $"Pagamento|Bill|{bill.OrderId}|{bill.ItemId}|{bill.Status}|{bill.Amount.ToString(CultureInfo.InvariantCulture)}|{bill.ExpiresAt:o}");
                    break;

                default:
                    Send(clientGuid, $"Log_Server|Azione pagamento non riconosciuta: {msgArgs[3]}");
                    break;
            }
        }

        /// <summary>
        /// Dispatch per il comando "Prelievo" (vedi ServerConnection.cs, case "Prelievo").
        /// Sotto-azioni: Richiedi | Lista.
        /// </summary>
        public static async Task GestisciComandoPrelievo(string[] msgArgs, Guid clientGuid, Player player)
        {
            if (msgArgs.Length < 4)
            {
                Send(clientGuid, "Log_Server|Comando non valido. Usa: Prelievo|<azione>|<parametri>");
                return;
            }

            switch (msgArgs[3])
            {
                case "Richiedi":
                {
                    // Prelievo|token|Richiedi|<destinatario>|<importo>
                    if (msgArgs.Length < 6 || !decimal.TryParse(msgArgs[5], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
                    {
                        // "Prelievo|Errore|..." oltre al solito "Log_Server" (24/09/2026): il popup
                        // di prelievo lato client ha bisogno di un esito dedicato per riabilitare il
                        // pulsante e mostrare l'errore inline, non solo nel log generico di gioco.
                        Send(clientGuid, "Prelievo|Errore|Parametri insufficienti. Usa: Prelievo|Richiedi|<destinatario>|<importo>");
                        Send(clientGuid, "Log_Server|Parametri insufficienti. Usa: Prelievo|Richiedi|<destinatario>|<importo>");
                        return;
                    }

                    string destinatario = msgArgs[4];

                    // 23/09/2026: RichiediPrelievo scala subito i Tributi (Dollari_Virtuali) del
                    // giocatore, PRIMA di autorizzare l'invio reale — vedi commento lì per i
                    // dettagli (incluso il rimborso automatico se l'invio on-chain fallisce poi).
                    var (ok, errore, prelievo) = RichiediPrelievo(player, destinatario, amount);
                    if (!ok)
                    {
                        Send(clientGuid, $"Prelievo|Errore|{errore}");
                        Send(clientGuid, $"Log_Server|{errore}");
                        return;
                    }

                    Send(clientGuid, $"Prelievo|Richiesto|{prelievo.Id}|{prelievo.Amount.ToString(CultureInfo.InvariantCulture)}");

                    // Mail di conferma al giocatore che ha effettuato il prelievo — su richiesta
                    // esplicita. Inviata alla richiesta (non aspetta la conferma on-chain, che può
                    // richiedere diversi minuti con CONFERME_MINIME alto): conferma solo che la
                    // richiesta è stata presa in carico, non che i fondi sono già arrivati.
                    if (!string.IsNullOrWhiteSpace(player.Email))
                    {
                        _ = EmailManager.SendEmailAsync(player.Email, player.Username,
                            "⚔️ Warrior and Wealth — Richiesta di prelievo ricevuta",
                            $@"<h2>Richiesta di prelievo ricevuta</h2>
                            <p>Ciao <strong>{player.Username}</strong>,</p>
                            <p>Abbiamo ricevuto la tua richiesta di prelievo:</p>
                            <ul>
                              <li>Importo: <strong>{prelievo.Amount.ToString(CultureInfo.InvariantCulture)} USDT</strong></li>
                              <li>Destinatario: <code>{destinatario}</code></li>
                              <li>Rete: Polygon</li>
                            </ul>
                            <p>La transazione verrà inviata a breve e diventerà definitiva dopo le conferme di sicurezza sulla rete — può richiedere qualche minuto.</p>
                            <p class='warning'>Non hai richiesto tu questo prelievo? Scrivici subito a <strong>support@warriorsandwealth.com</strong>.</p>");
                    }

                    _ = Task.Run(() => EseguiPrelievoAsync(prelievo.Id)); // firma/invio reale in background, non blocca la risposta al client
                    break;
                }

                case "Lista":
                    foreach (var prelievo in ListaPrelieviGiocatore(player.Username))
                        Send(clientGuid, $"Prelievo|Voce|{prelievo.Id}|{prelievo.Stato}|{prelievo.Amount.ToString(CultureInfo.InvariantCulture)}|{prelievo.TxHash}");
                    break;

                default:
                    Send(clientGuid, $"Log_Server|Azione prelievo non riconosciuta: {msgArgs[3]}");
                    break;
            }

            await Task.CompletedTask; // firma async coerente col resto (stile Raduno) anche quando il ramo eseguito è sincrono
        }

        #endregion

        #region Utilità

        private static void Log(string msg) => Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [Blockchain] {msg}");

        #endregion
    }
}
