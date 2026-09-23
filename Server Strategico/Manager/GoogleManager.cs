using Google.Apis.AndroidPublisher.v3;
using Google.Apis.AndroidPublisher.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Server_Strategico.ServerData.Moduli;
using System.Collections.Concurrent;
using System.Text.Json;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Server.Server;

namespace Server_Strategico.Manager
{
    /// <summary>
    /// Google Play Billing — acquisti in-app "veri" (la finestra nativa di Google che chiede
    /// impronta/password), verificati lato server con la Play Developer API prima di accreditare
    /// qualunque cosa, stesso principio "mai fidarsi del client" di BlockchainManager.
    ///
    /// STATO (24/09/2026): SCHELETRO, non ancora operativo. Perché possa fare qualcosa di reale
    /// mancano ancora, tutte cose che vanno preparate FUORI da questo file, non nel codice:
    ///
    ///   1. Un'app pubblicata su Google Play. WinForms non può usare Google Play Billing in alcun
    ///      modo. Il client Web invece può arrivarci "avvolgendolo" in una Trusted Web Activity
    ///      (TWA) — un piccolo progetto Android che fa da cornice al sito già esistente, pubblicato
    ///      su Play con la Digital Goods API per i pagamenti — ma è comunque un progetto a parte
    ///      da costruire (manifest PWA, service worker, firma dell'app, revisione di Google) prima
    ///      che esista un client capace di generare un purchase token da verificare qui.
    ///   2. PACKAGE_NAME qui sotto compilato con il package name reale di quell'app (es.
    ///      "com.warriorandwealth.app") — assegnato quando l'app viene creata nel Play Console.
    ///   3. Un service account Google Cloud collegato al Play Console (Impostazioni API → Crea
    ///      service account) con i permessi "Visualizza informazioni finanziarie" e "Gestisci
    ///      ordini e abbonamenti", con il file JSON delle sue credenziali scaricato e il suo
    ///      percorso messo in SERVICE_ACCOUNT_JSON_PATH — fuori dal repo, stesso trattamento del
    ///      file della seed phrase (vedi BlockchainManager.SeedFilePath).
    ///   4. I prodotti in-app (SKU) creati nel Play Console, con ProductIdMapping qui sotto
    ///      compilato per far corrispondere ogni SKU di Google alla voce giusta in
    ///      Variabili_Server.Shop.Catalogo (stesso ricompenso, prezzo deciso però direttamente nel
    ///      Play Console, non da questo server).
    ///   5. Un Payments Profile configurato nel Play Console (dati bancari — IBAN — per ricevere i
    ///      pagamenti da Google): NON serve una carta di credito personale, quella non c'entra qui.
    ///      È Google che incassa dal giocatore e poi gira i soldi (meno la sua commissione, 15-30%
    ///      a seconda del fatturato) al tuo conto, con cadenza mensile.
    ///
    /// Finché PACKAGE_NAME o SERVICE_ACCOUNT_JSON_PATH restano vuoti/non validi,
    /// VerificaEAccreditaAcquistoAsync si ferma subito senza tentare nulla (vedi guardia in cima),
    /// quindi questo file può restare agganciato al wire protocol senza rischi anche prima che le
    /// cose sopra siano pronte.
    /// </summary>
    internal class GoogleManager
    {
        #region Configurazione — da compilare quando l'app Android/TWA e il Play Console sono pronti

        // TODO: package name dell'app pubblicata su Google Play (es. "com.warriorandwealth.app").
        public const string PACKAGE_NAME = "";

        // TODO: percorso assoluto al file JSON delle credenziali del service account (fuori dal
        // repo — vedi .gitignore, stesso trattamento del file della seed phrase).
        public const string SERVICE_ACCOUNT_JSON_PATH = "";

        // Corrispondenza SKU Google Play -> chiave in Variabili_Server.Shop.Catalogo. Tenerle
        // separate (invece di riusare direttamente le chiavi del catalogo come product id su
        // Google) perché il Play Console impone le sue regole sul formato degli id prodotto
        // (minuscolo, senza spazi) e potrebbe non coincidere 1:1 con le chiavi C# esistenti.
        // TODO: popolare quando i prodotti vengono creati nel Play Console, es.:
        //   ["starter_usdt_test"] = "Test_USDT",
        private static readonly Dictionary<string, string> ProductIdMapping = new();

        #endregion

        #region Persistenza (stesso schema "scrittura atomica" di BlockchainManager)

        private static readonly string DataPath = Path.Combine(GameSave.SavePath, "GooglePlay");
        private static readonly string TokenProcessatiPath = Path.Combine(DataPath, "token_processati.json");

        // Blacklist anti-riuso/anti-doppio-credito: ogni purchaseToken va accreditato una volta
        // sola, anche se il client (per un problema di rete, un doppio tap, ecc.) lo rimanda più
        // volte. Chiave: purchaseToken, valore: itemId accreditato.
        private static readonly ConcurrentDictionary<string, string> TokenProcessati = new();

        static GoogleManager()
        {
            Directory.CreateDirectory(DataPath);
            CaricaTokenProcessati();
        }

        private static void SalvaTokenProcessati()
        {
            try
            {
                Directory.CreateDirectory(DataPath);
                string tempPath = TokenProcessatiPath + ".tmp";
                File.WriteAllText(tempPath, JsonSerializer.Serialize(new Dictionary<string, string>(TokenProcessati), new JsonSerializerOptions { WriteIndented = true }));
                File.Move(tempPath, TokenProcessatiPath, overwrite: true); // scrittura atomica: mai un file a metà in caso di crash
            }
            catch (Exception ex)
            {
                Log($"[ERRORE] Salvataggio token_processati.json fallito: {ex.Message}");
            }
        }

        private static void CaricaTokenProcessati()
        {
            if (!File.Exists(TokenProcessatiPath)) return;
            try
            {
                var dizionario = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(TokenProcessatiPath)) ?? new();
                foreach (var kv in dizionario) TokenProcessati[kv.Key] = kv.Value;
                Log($"Caricati {TokenProcessati.Count} purchaseToken già processati (blacklist anti-doppio-credito)");
            }
            catch (Exception ex) { Log($"[ERRORE] Caricamento token_processati.json fallito: {ex.Message}"); }
        }

        #endregion

        #region Verifica lato server (Play Developer API)

        private static AndroidPublisherService _service;

        private static AndroidPublisherService OttieniServizio()
        {
            if (_service != null) return _service;

            var credential = GoogleCredential.FromFile(SERVICE_ACCOUNT_JSON_PATH)
                .CreateScoped(AndroidPublisherService.Scope.Androidpublisher);

            _service = new AndroidPublisherService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Warrior and Wealth",
            });
            return _service;
        }

        /// <summary>
        /// Verifica un acquisto Google Play con la Play Developer API e, se valido, lo accredita
        /// tramite Shop.AccreditaAcquisto — la STESSA funzione che usa BlockchainManager per gli
        /// acquisti USDT confermati, così la logica di ricompensa resta in un solo posto per ogni
        /// item, indipendentemente dal metodo di pagamento usato per arrivarci.
        /// </summary>
        public static async Task VerificaEAccreditaAcquistoAsync(Guid clientGuid, Player player, string itemId, string purchaseToken)
        {
            if (string.IsNullOrWhiteSpace(PACKAGE_NAME) || string.IsNullOrWhiteSpace(SERVICE_ACCOUNT_JSON_PATH) || !File.Exists(SERVICE_ACCOUNT_JSON_PATH))
            {
                Log("Non configurato (PACKAGE_NAME o service account mancanti in GoogleManager) — acquisto NON verificato, nessun credito.");
                Send(clientGuid, "Log_Server|[Shop] Gli acquisti Google Play non sono ancora attivi su questo server.");
                return;
            }

            if (!ProductIdMapping.TryGetValue(itemId, out string googleProductId))
            {
                Log($"[ERRORE] Nessun product id Google Play mappato per l'item '{itemId}' — vedi ProductIdMapping.");
                Send(clientGuid, "Log_Server|[Shop] Questo item non è ancora disponibile su Google Play.");
                return;
            }

            if (TokenProcessati.ContainsKey(purchaseToken))
            {
                Log($"purchaseToken già processato, ignorato (probabile doppio invio dal client): {purchaseToken}");
                return;
            }

            try
            {
                var service = OttieniServizio();
                ProductPurchase acquisto = await service.Purchases.Products.Get(PACKAGE_NAME, googleProductId, purchaseToken).ExecuteAsync();

                // PurchaseState: 0 = Acquistato, 1 = Annullato, 2 = In sospeso (es. pagamento in
                // verifica) — accreditiamo solo lo stato 0, esattamente come i depositi USDT
                // vengono accreditati solo dopo le conferme sulla rete, mai prima.
                if (acquisto.PurchaseState != 0)
                {
                    Log($"Acquisto non valido (purchaseState={acquisto.PurchaseState}) — giocatore {player.Username}, item {itemId}.");
                    Send(clientGuid, "Log_Server|[Shop] Pagamento Google Play non ancora confermato.");
                    return;
                }

                TokenProcessati[purchaseToken] = itemId;
                SalvaTokenProcessati();

                // Un acquisto non "acknowledged" entro 3 giorni viene rimborsato automaticamente da
                // Google — va confermato subito che il server lo ha ricevuto ed elaborato.
                if (acquisto.AcknowledgementState == 0)
                {
                    await service.Purchases.Products.Acknowledge(
                        new ProductPurchasesAcknowledgeRequest(), PACKAGE_NAME, googleProductId, purchaseToken).ExecuteAsync();
                }

                Shop.AccreditaAcquisto(player.Username, itemId);
                Log($"Acquisto confermato e accreditato: {player.Username} → {itemId} (token {purchaseToken}).");
            }
            catch (Exception ex)
            {
                Log($"[ERRORE] Verifica fallita — giocatore {player.Username}, item {itemId}: {ex.Message}");
                Send(clientGuid, "Log_Server|[Shop] Errore nella verifica del pagamento Google Play. Riprova o contatta il supporto.");
            }
        }

        #endregion

        #region Wire protocol — comando "GooglePlay" dal client

        /// <summary>
        /// Dispatch per il comando "GooglePlay" (da agganciare in ServerConnection.cs, case
        /// "GooglePlay", stesso schema di "Pagamento"/"Prelievo"). Sotto-azioni: Verifica.
        ///
        /// NOTA: questo comando ha senso solo dentro l'app Android/TWA pubblicata su Google Play —
        /// è lì che gira l'SDK di Google Play Billing che produce il purchaseToken da mandare qui.
        /// Sul client Web "normale" (browser) o su WinForms desktop non arriverà mai nulla, perché
        /// quell'SDK lì non esiste (vedi il TODO #1 in cima al file).
        /// </summary>
        public static async Task GestisciComando(string[] msgArgs, Guid clientGuid, Player player)
        {
            if (msgArgs.Length < 4)
            {
                Send(clientGuid, "Log_Server|Comando non valido. Usa: GooglePlay|<azione>|<parametri>");
                return;
            }

            switch (msgArgs[3])
            {
                case "Verifica":
                {
                    // GooglePlay|token|Verifica|<itemId>|<purchaseToken>
                    if (msgArgs.Length < 6)
                    {
                        Send(clientGuid, "Log_Server|Parametri insufficienti. Usa: GooglePlay|Verifica|<itemId>|<purchaseToken>");
                        return;
                    }

                    string itemId = msgArgs[4];
                    string purchaseToken = msgArgs[5];
                    await VerificaEAccreditaAcquistoAsync(clientGuid, player, itemId, purchaseToken);
                    break;
                }

                default:
                    Send(clientGuid, $"Log_Server|Azione Google Play non riconosciuta: {msgArgs[3]}");
                    break;
            }
        }

        #endregion

        private static void Log(string msg) => Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [GooglePlay] {msg}");
    }
}
