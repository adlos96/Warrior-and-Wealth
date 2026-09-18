using Server_Strategico.Gioco;
using Server_Strategico.Manager;
using Server_Strategico.ServerData.Moduli;
using Server_Strategico.ServerData.Moduli.Battaglie;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WatsonTcp;
using static Server_Strategico.Gioco.Giocatori;

namespace Server_Strategico.Server
{
    internal class Server
    {
        public static List<Guid> Client_Connessi = new List<Guid>(); // va rimossa, è pericolosa con il multithread
        public static List<string> Utenti_PVP = new List<string>();

        public static System.Collections.Concurrent.ConcurrentDictionary<Guid, string> Client_Connessi_Map =
        new System.Collections.Concurrent.ConcurrentDictionary<Guid, string>(); //Mappa client x multithread

        private string? serverIp = null; // "null" will open the tcp server on addr 0.0.0.0 on windows (127.0.0.1 on linux)
        private const int serverPort = 8443;
        private static Guid lastGuid = Guid.Empty;
        public static WatsonTcpServer? server = null;

        private static string _CertFile = "";
        private static string _CertPass = "";

        private static bool _Ssl = false;
        private static bool _AcceptInvalidCerts = true;
        private static bool _MutualAuth = false;

        private CancellationTokenSource cts;
        private Task gameLoopTask;
        static public GameServer servers_ = new GameServer();

        public static double totale_Stats = 0, media_Stats = 0, min_Stats = 0, max_Stats = 0, numero_Stats = 0;
        static bool avviato = false;

        private Server()
        {
            string subjectName = Environment.MachineName; //Ottine il nome della macchina (hostname)

            // Path di log specifico Linux, stesso trattamento di SavePath sotto — va impostato
            // PRIMA di InitializeLogging() (altrimenti il log finirebbe nella cartella di default
            // Windows anche su Linux), quindi il check OS è separato e anticipato rispetto al
            // blocco if/else con i Console.WriteLine qualche riga più sotto.
            if (OperatingSystem.IsLinux())
                GameSave.LogPath = "/opt/Warrior-and-Wealth/Log";
            GameSave.InitializeLogging(); // prima di qualsiasi Console.WriteLine, per non perdere le primissime righe

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (OperatingSystem.IsWindows())
            {
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("[OS] Siamo su Windows");
                if (subjectName == "DESKTOP-DOBLVTI" || subjectName == "ADLO") serverIp = "0.0.0.0";
            }
            else if (OperatingSystem.IsLinux())
            {
                Console.WriteLine("Siamo su Linux");
                GameSave.SavePath = "/opt/Warrior-and-Wealth/Saves_Test";
            }
            GameSave.Initialize();

            if (!_Ssl) server = new WatsonTcpServer(serverIp, serverPort);
            else
            {
                _CertPass = Password.password;
                _AcceptInvalidCerts = true;
                _MutualAuth = true;

                server = new WatsonTcpServer(serverIp, serverPort, _CertFile, _CertPass);
                server.Settings.AcceptInvalidCertificates = _AcceptInvalidCerts;
                server.Settings.MutuallyAuthenticate = _MutualAuth;
            }

            server.Events.ClientConnected += ClientConnected;
            server.Events.MessageReceived += MessageReceived;
            server.Events.ClientDisconnected += ClientDisconnected;
            server.Events.ExceptionEncountered += ExceptionEncountered;

            server.Settings.Logger = Logger;
            server.Settings.NoDelay = true;
            server.Keepalive.EnableTcpKeepAlives = true;
            server.Keepalive.TcpKeepAliveInterval = 1;
            server.Keepalive.TcpKeepAliveTime = 1;
            server.Keepalive.TcpKeepAliveRetryCount = 3;
            server.Start();

            Console.WriteLine("[SERVER|LOG] (Info) > [WatsonTcpServer] Server Inizializzato");
            Console.WriteLine("");

            // Gateway WebSocket: processo/porta separati, avviato solo se
            // abilitato (Variabili_Server.WebGatewayEnabled) o a runtime col
            // comando "webstart". Il try/catch isola qualsiasi problema del
            // layer web (porta occupata, ecc.) dal resto del server: se
            // fallisce, il gioco e le connessioni WatsonTcp non ne risentono.
            if (Variabili_Server.WebGatewayEnabled)
            {
                try { WebSocketGateway.Start(Variabili_Server.WebGatewayPort); }
                catch (Exception ex) { Console.WriteLine($"[SERVER|LOG] (Errore) > WebSocketGateway non avviato: {ex.Message}"); }
            }

            Task task = StartGame();
            bool avviso = false;

            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("Info Comandi: \"?\"");
                var userInput = string.Empty;
                try
                {
                    userInput = Console.ReadLine() ?? string.Empty;
                }
                catch (Exception ex)
                {
                    if (!avviso)
                    {
                        Console.WriteLine($"[SERVER|LOG] (Errore) > Console non più leggibile, comandi da tastiera disabilitati: {ex.Message}");
                        avviso = true;
                    }
                }

                switch (userInput)
                {
                    case "?":
                        Console.WriteLine("");
                        Console.WriteLine("                         *** Command ***");
                        Console.WriteLine("----------------------------------------------------------------------");
                        Console.WriteLine("Comando vuoto:                 [player]");                      // 
                        Console.WriteLine("Comando vuoto:                 [client]");                      // 
                        Console.WriteLine("Comando vuoto:                 [battaglia]");                      // 
                        Console.WriteLine("Comando vuoto:                 [spionaggio]");                      // 
                        Console.WriteLine("Comando vuoto:                 [disconnetti]");                      //

                        Console.WriteLine(" --------------------- Web Client (WebSocket) ---------------------");                      //
                        Console.WriteLine("Comando vuoto:                 [webstart]  (avvia il gateway WebSocket per il client web)");
                        Console.WriteLine("Comando vuoto:                 [webstop]   (ferma il gateway WebSocket)");

                        Console.WriteLine(" --------------------- Server Stats ---------------------");                      //
                        Console.WriteLine("Comando vuoto:                 [clear stats]");                      // 
                        Console.WriteLine("Comando vuoto:                 [clear stats max]");                      //

                        Console.WriteLine("----------------------------------------------------------------------");
                        break;
                    case "player":
                        servers_.Player_Creati();
                        break;
                    case "client":
                        ClientConnessi();
                        break;
                    case "battaglia":
                        BattagliaPVP.TestBattaglia();
                        break;
                    case "spionaggio":
                        Spionaggio.EseguiSpionaggioTEST();
                        break;
                    case "disconnetti":
                        Console.Write("Username del giocatore da disconnettere: ");
                        string usernameTarget = Console.ReadLine() ?? string.Empty;
                        _ = DisconnettiGiocatore(usernameTarget); // fire-and-forget, dato che siamo in un metodo sync
                        break;
                    case "webstart":
                        try { WebSocketGateway.Start(Variabili_Server.WebGatewayPort); }
                        catch (Exception ex) { Console.WriteLine($"[Server] Errore avvio WebSocketGateway: {ex.Message}"); }
                        break;
                    case "webstop":
                        try { WebSocketGateway.Stop(); }
                        catch (Exception ex) { Console.WriteLine($"[Server] Errore arresto WebSocketGateway: {ex.Message}"); }
                        break;

                    case "clear stats": max_Stats = 0; break;
                    case "clear stats max":
                        totale_Stats = 0;
                        media_Stats = 0;
                        min_Stats = 0;
                        max_Stats = 0;
                        numero_Stats = 0;
                        break;
                    case "":
                        break;

                    default: Console.WriteLine("[Server] >> Comando sconosciuto"); break;
                }
            }
        }
        void ClientConnessi()
        {
            if (Client_Connessi.Count() == 0) Console.WriteLine("Client connessi: 0");
            foreach (var item in Client_Connessi)
                Console.WriteLine($"Client: {item}");
        }

        private async Task StartGame()
        {
            cts = new CancellationTokenSource();
            gameLoopTask = servers_.RunGameLoopAsync(cts.Token);

            Console.WriteLine($"[Server] Attesa avvio server....");
            while (!avviato)
            {
                Thread.Sleep(1000);
            }
            Console.WriteLine($"[Server] Server avviato!");
            //Start WebSocketGateway
            try { WebSocketGateway.Start(Variabili_Server.WebGatewayPort); }
            catch (Exception ex) { Console.WriteLine($"[Server] Errore avvio WebSocketGateway: {ex.Message}"); }
            Console.WriteLine("-----------------------------------------------------------");

        }
        private async Task StopGame()
        {
            if (cts != null)
            {
                cts.Cancel(); // Ferma il loop di gioco
                await gameLoopTask; // Attende che il loop si fermi completamente
                Console.WriteLine("Il gioco è terminato.");
            }
            else Console.WriteLine("Il gioco non è attualmente in esecuzione.");
        }
        public static void Send(Guid guid, string msg)
        {
            if (guid == Guid.Empty) return;

            // Instradamento per trasporto: i client del gateway web (vedi
            // WebSocketGateway.cs) non sono client WatsonTcp, quindi vanno
            // inviati con il loro socket. Nessuna modifica alla logica di
            // gioco: da qui in giù il resto del server continua a chiamare
            // solo Server.Send(guid, msg) senza sapere quale trasporto c'è
            // dietro al guid.
            bool inviato = false;
            if (WebSocketGateway.IsWebSocketClient(guid))
            {
                WebSocketGateway.Send(guid, msg);
                inviato = true;
            }
            else if (Client_Connessi.Contains(guid))
            {
                server.SendAsync(guid, msg);
                inviato = true;
            }

            // 16/09/2026, su richiesta dell'utente: la Cronologia (pannello "Log_Server" del
            // client) ora viene anche salvata lato server, non solo mostrata "al volo" — così
            // sopravvive a un ricollegamento o a un riavvio del server invece di sparire ogni
            // volta. Risale al giocatore dal guid tramite la stessa mappa già usata per il
            // routing dei messaggi, invece di aggiungere un parametro Player a ogni singola
            // chiamata a Send sparsa in centinaia di punti del codice. Il controllo
            // StartsWith è economico e riguarda solo i messaggi Log_Server (rari rispetto
            // agli Update_Data di ogni tick), quindi non pesa sul percorso più frequente.
            string ora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            if (inviato && msg.StartsWith("Log_Server|") && Client_Connessi_Map.TryGetValue(guid, out string usernameLog))
            {
                var giocatoreLog = servers_.GetPlayer(usernameLog);
                if (giocatoreLog != null)
                {
                    giocatoreLog.Cronologia.Add(msg.Substring("Log_Server|".Length));
                    if (giocatoreLog.Cronologia.Count > 300)
                        giocatoreLog.Cronologia.RemoveAt(0);
                }
            }

            if (inviato && !msg.Contains("Update_Data") && !msg.Contains("QuestRewards") && !msg.Contains("QuestUpdate") && !msg.Contains("Descrizione"))
                Console.WriteLine($"[{ora}][SERVER|LOG] > {msg}");
        }

        public static async Task NewPlayer(string player, string password)
        {
            var player1 = servers_.GetPlayer(player, password);

            //Tutorial off
            //player1.Tutorial = true;

            //Tutorial off + risorse per test
            player1.Tutorial = false;
            player1.Cibo = 30000;
            player1.Legno = 30000;
            player1.Pietra = 30000;
            player1.Ferro = 30000;
            player1.Oro = 30000;
            player1.Popolazione = 1200;

            player1.Spade = 2000;
            player1.Lance = 2000;
            player1.Archi = 2000;
            player1.Scudi = 2000;
            player1.Armature = 2000;
            player1.Frecce = 200;

            player1.Diamanti_Blu = 600000;
            player1.Diamanti_Viola = 450000;

            Gioco.Barbari.GeneraVillaggiPerGiocatore(player1);
        }
        // ----------------------- Client Connessione --------------------------
        static void ClientConnected(object? sender, ConnectionEventArgs args)
        {
            lastGuid = args.Client.Guid;
            string lasIpPort = args.Client.IpPort;
            Console.WriteLine("[SERVER|LOG] > Client connesso: " + args.Client.ToString());

            // AGGIUNTA FONDAMENTALE: Aggiungi il GUID del client alla mappa.
            // Il valore è provvisorio finché il client non fa il login. 
            // Lo usiamo per l'iterazione O(M).
            Client_Connessi_Map.TryAdd(lastGuid, args.Client.IpPort);

            // Manteniamo la lista per compatibilità, ma usiamo la mappa per l'aggiornamento
            if (!Client_Connessi.Contains(lastGuid))
                Client_Connessi.Add(lastGuid);
            
            Send(lastGuid, $"Update_Data|versione_Client_Necessario={Variabili_Server.versione_Client_Necessario}");
        }
        static async void ClientDisconnected(object? sender, DisconnectionEventArgs args)
        {
            lastGuid = args.Client.Guid;
            Console.WriteLine("[SERVER|LOG] > Client disconnesso: " + args.Client.ToString() + ": " + args.Reason.ToString());

            Client_Connessi_Map.TryRemove(lastGuid, out _);
            Client_Connessi.Remove(lastGuid);

            // Forza pulizia del client su WatsonTcp
            try
            {
                await server.DisconnectClientAsync(lastGuid);
            }
            catch { }
        }
        public static async Task<bool> DisconnettiGiocatore(string username)
        {
            var player = servers_.GetPlayer(username);
            if (player == null)
            {
                Console.WriteLine($"[SERVER|LOG] > Giocatore '{username}' non trovato.");
                return false;
            }

            if (player.guid_Player == Guid.Empty || !Client_Connessi.Contains(player.guid_Player))
            {
                Console.WriteLine($"[SERVER|LOG] > Giocatore '{username}' non è attualmente connesso.");
                return false;
            }

            try
            {
                Console.WriteLine($"[SERVER|LOG] > Disconnessione forzata di '{username}' (GUID: {player.guid_Player})");
                if (WebSocketGateway.IsWebSocketClient(player.guid_Player))
                    WebSocketGateway.Disconnect(player.guid_Player);
                else
                    await server.DisconnectClientAsync(player.guid_Player);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SERVER|LOG] > Errore durante la disconnessione di '{username}': {ex.Message}");
                return false;
            }
        }
        private static void ExceptionEncountered(object sender, ExceptionEventArgs e)
        {
            Console.WriteLine(server.SerializationHelper.SerializeJson(e.Exception, true));
        }
        
        // ----------------------- Server --------------------------
        public static WatsonTcpServer GetInstance()
        {
            if (server == null) new Server();
            return server;
        }
        static void MessageReceived(object? sender, MessageReceivedEventArgs args)
        {
            Console.Write("[SERVER|LOG] > " + args.Data.Length + " byte message from " + args.Client + ": " + "\r");
            if (args.Data != null || args.Data.Length != 0) ServerConnection.HandleClientRequest(args);
            else Console.WriteLine("[SERVER|LOG] > [null]");
        }
        static void Logger(Severity sev, string msg)
        {
            Console.WriteLine("[SERVER|LOG] (" + sev.ToString() + ") > " + msg);
        }

        public class GameServer
        {
            public Dictionary<string, Player> players = new Dictionary<string, Player>();
            int _saveIndex = 0;
            public async Task<bool> AddPlayer(string username, string password, string email, Guid guid)
            {
                var newPlayer = new Player(username, password, email, guid);
                if (!players.TryAdd(username, newPlayer))
                    return false; // username già esistente, nessuna eccezione, nessuna race

                await NewPlayer(username, password);
                return true;
            }

            public Player GetPlayer(string username, string password)
            {
                if (!players.TryGetValue(username, out Player player)) return null;
                return player.ValidatePassword(password) ? player : null;
            }

            public Player GetPlayer(string username)
            {
                players.TryGetValue(username, out Player player);
                return player;
            }
            // Riformattato (14/09/2026, su richiesta dell'utente: "visivamente è
            // molto brutto") in una tabella allineata a colonne fisse, invece
            // di una riga di testo libero per giocatore: prima ogni riga aveva
            // lunghezza diversa (username di lunghezza variabile) e conteneva
            // anche il Guid, che per la stragrande maggioranza dei giocatori è
            // sempre 00000000-0000-0000-0000-000000000000 (nessun client mai
            // connesso con quell'account) — pura confusione visiva senza alcuna
            // informazione utile, quindi rimosso dalla stampa.
            public void Player_Creati()
            {
                const int larghUsername = 22;
                const int larghLivello = 9;
                const int larghPotenza = 10;

                string intestazione =
                    "   " +
                    "Username".PadRight(larghUsername) +
                    "Livello".PadRight(larghLivello) +
                    "Potenza".PadRight(larghPotenza) +
                    "Ultimo accesso";
                string separatore = new string('─', intestazione.Length);

                Console.WriteLine();
                Console.WriteLine($"Giocatori registrati: {players.Count()}");
                Console.WriteLine(separatore);
                Console.WriteLine(intestazione);
                Console.WriteLine(separatore);

                foreach (var item in players)
                {
                    var player = item.Value;
                    bool connesso = player.guid_Player != Guid.Empty && Client_Connessi.Contains(player.guid_Player);

                    Console.ForegroundColor = connesso ? ConsoleColor.Green : ConsoleColor.DarkGray;
                    Console.Write(connesso ? " ● " : " ○ ");
                    Console.ResetColor();

                    string username = string.IsNullOrWhiteSpace(player.Username) ? "(senza nome)" : player.Username;
                    if (username.Length > larghUsername - 1) username = username.Substring(0, larghUsername - 4) + "...";

                    string ultimoAccesso = player.Last_Login == DateTime.MinValue
                        ? "mai"
                        : player.Last_Login.ToString("dd/MM/yyyy");

                    Console.WriteLine(
                        username.PadRight(larghUsername) +
                        player.Livello.ToString().PadRight(larghLivello) +
                        player.Potenza_Totale.ToString("#,0").PadRight(larghPotenza) +
                        ultimoAccesso);
                }

                Console.WriteLine(separatore);
                Console.WriteLine();
            }
            public void AggiornaListaPVP()
            {
                if (Client_Connessi.Count == 0) return;
                var utentiDaAggiungere = new List<string>();
                var indexCache = new Dictionary<string, int>(Utenti_PVP.Count); // CACHE LOCALE: username → indice

                if (players.Count() != indexCache.Count())
                    for (int i = 0; i < Utenti_PVP.Count; i++)
                    {
                        var s = Utenti_PVP[i];
                        int idx = s.IndexOf(',');
                        if (idx > 0)
                        {
                            var username = s.Substring(0, idx).Trim();
                            indexCache[username] = i;
                        }
                    }

                foreach (var kv in players)
                {
                    var player = kv.Value;
                    if (player.ScudoDellaPace != 0 || player.Livello < Variabili_Server.PVP_Unlock) continue; //Continue salta il codice sottostante? riparte con un nuovo ciclo?
                    if (indexCache.TryGetValue(player.Username, out int idx))
                    {
                        var utentePVP = Utenti_PVP[idx];
                        if (!utentePVP.Contains($"{player.Username}") || !utentePVP.Contains($"Livello: {player.Livello}") || !utentePVP.Contains($"Potenza: {player.Potenza_Totale}"))
                            Utenti_PVP[idx] = $"{player.Username}, Livello: {player.Livello}, Potenza: {player.Potenza_Totale}";
                    }
                    else utentiDaAggiungere.Add($"{player.Username}, Livello: {player.Livello}, Potenza: {player.Potenza_Totale}");
                }
                for (int i = 0; i < utentiDaAggiungere.Count; i++) Utenti_PVP.Add(utentiDaAggiungere[i]);
            }
            private TimeSpan _lastCpu = TimeSpan.Zero;
            private DateTime _lastTime = DateTime.UtcNow;
            public async Task PrintResourcesAsync()
            {
                Process proc = Process.GetCurrentProcess();
                proc.Refresh(); // FIX: senza questo, su Linux i valori restano quelli della prima lettura

                double ramMb = GetAccurateRamMb(proc);
                TimeSpan currentCpu = proc.TotalProcessorTime; // CPU
                DateTime now = DateTime.UtcNow;

                double cpuPercent = 0;
                double intervalSec = (now - _lastTime).TotalSeconds;
                if (intervalSec > 0)
                {
                    double deltaCpuMs = (currentCpu - _lastCpu).TotalMilliseconds;
                    double deltaWallMs = intervalSec * 1000.0;
                    cpuPercent = (deltaCpuMs / deltaWallMs) * 100.0 / Environment.ProcessorCount;
                }

                _lastCpu = currentCpu;
                _lastTime = now;

                int playerCount = players.Count();
                double ramPerPlayerKb = playerCount > 0
                    ? (ramMb - Variabili_Server._Server_Consumo_RAM) / playerCount * 1024.0
                    : 0;

                Console.WriteLine($"[Server Resources] Totale - RAM: {ramMb:F2} MB | CPU: {cpuPercent:F2} %");
                Console.WriteLine($"[Server Resources] X player - RAM: {ramPerPlayerKb:F2} KB");

                await Task.CompletedTask;
            }

            private static double GetAccurateRamMb(Process proc)
            {
                // Su Linux leggiamo VmRSS direttamente da /proc/self/status: più preciso di WorkingSet64
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    try
                    {
                        foreach (string line in File.ReadLines("/proc/self/status"))
                        {
                            if (line.StartsWith("VmRSS:"))
                            {
                                // formato: "VmRSS:      123456 kB"
                                string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                if (parts.Length >= 2 && long.TryParse(parts[1], out long kb))
                                {
                                    return kb / 1024.0; // kB -> MB
                                }
                            }
                        }
                    }
                    catch
                    {
                        // fallback sotto in caso di errore di lettura
                    }
                }

                // Windows (o fallback Linux se /proc/self/status non leggibile)
                return proc.WorkingSet64 / 1024.0 / 1024.0;
            }


            public async Task<bool> Check_Username_Player(string username)
            {
                foreach (var item in players)
                    if (item.Value.Username == username)
                        return false;
                return true;
            }
            // All'interno della classe GameServer

            public async Task Auto_Update_Clients() // Sostituisce il metodo esistente
            {
                // --- PARTE 1: Gestione Disconnessioni/Cleanup (Seriale) ---
                if (Client_Connessi_Map.Count == 0)
                {
                    // Se non ci sono client connessi, azzera i GUID nei giocatori non connessi.
                    // Questa iterazione O(N) è accettabile perché avviene solo quando Client_Connessi_Map.Count == 0
                    // e ripulisce lo stato.
                    foreach (var player in players.Values.Where(p => p.guid_Player != Guid.Empty))
                        player.guid_Player = Guid.Empty;
                    return;
                }

                // --- PARTE 2: Aggiornamento Multicore (I/O Parallelizzato in O(M)) ---

                // Iteriamo SOLO sui client connessi (M=5), non sui 58.000 giocatori!
                var updateTasks = Client_Connessi_Map.Keys // Client_Connessi_Map.Keys contiene i GUID dei client (M=7)
                .Select(clientGuid =>
                {
                    // 1. Lookup O(1): Usiamo Client_Connessi_Map (GUID -> Username) per trovare l'username.
                    if (Client_Connessi_Map.TryGetValue(clientGuid, out string username))
                    {
                        // 2. Lookup O(1): Usiamo il dizionario globale 'players' (Username -> Player Object) per trovare l'oggetto Player.
                        // Sostituisce la vecchia, lenta chiamata players.Values.FirstOrDefault(...)
                        if (players.TryGetValue(username, out Player player))
                            ServerConnection.Update_Data(player.guid_Player, player); // L'oggetto Player è stato trovato in modo istantaneo
                    }
                    return Task.CompletedTask; // Se fallisce il lookup (giocatore disconnesso/non trovato), ritorniamo un Task completato.
                })
                .ToList();
                await Task.WhenAll(updateTasks); // 3. Attendiamo che tutti gli aggiornamenti di rete siano completati in parallelo.
            }
            async Task addBOT(int b)
            {
                for (int i = 0; i < b; i++)
                    await AddPlayer($"Fake{i}", "123", "fake@example.com", Guid.Empty);
            }
            public async Task RunGameLoopAsync(CancellationToken cancellationToken)
            {
                int stats = 0;

                if (Variabili_Server._Server_Consumo_RAM == 0)
                {
                    Process proc = Process.GetCurrentProcess();
                    // Prima usava proc.WorkingSet64 diretto anche su Linux, mentre PrintResourcesAsync
                    // calcola il valore "attuale" con GetAccurateRamMb (che su Linux legge VmRSS da
                    // /proc/self/status, una metrica diversa da WorkingSet64) — la sottrazione tra le due
                    // mescolava due misure incompatibili, dando i numeri "a caso" per player su Linux.
                    // Ora la baseline usa la stessa funzione, quindi la stessa metrica, di ogni lettura successiva.
                    Variabili_Server._Server_Consumo_RAM = (int)GetAccurateRamMb(proc);
                    Console.WriteLine($"[Server] Baseline RAM impostata: {Variabili_Server._Server_Consumo_RAM:F2} MB");
                }
                //await addBOT(500000);

                await GameSave.LoadServerData();
                await GameSave.LoadAllPlayersData();
                servers_.AggiornaListaPVP();
                await Gioco.Barbari.Inizializza();
                _ = Task.Run(() => RunGameLoopSecondarioAsync(cancellationToken));
                ScheduleManager.AvvioReset();


                int maxConcurrentTasks = Math.Max(1, Environment.ProcessorCount);
                var options = new ParallelOptions { MaxDegreeOfParallelism = maxConcurrentTasks };

                while (!cancellationToken.IsCancellationRequested)
                {
                    Stopwatch taskStopwatch = Stopwatch.StartNew();

                    await Task.Run(() =>
                        Parallel.ForEach(players.Values, options, player =>
                        {
                            if (player.Stato_Giocatore == false)
                            {
                                player.ProduceResources();
                                player.ManutenzioneEsercito();
                                return;
                            }
                            
                            player.ProduceResources();
                            player.ServerTimer();
                            //player.ResetGiornaliero();
                        })
                    );

                    if (Variabili_Server.timer_Reset_Quest > 0) Variabili_Server.timer_Reset_Quest--;
                    if (Variabili_Server.timer_Reset_Quest == 0) QuestManager.RigeneraQuest();
                    if (Variabili_Server.timer_Reset_Barbari > 0) Variabili_Server.timer_Reset_Barbari--;
                    if (Variabili_Server.timer_Reset_Barbari == 0) Barbari.RigeneraBarbari();

                    #region STATS SERVER
                    taskStopwatch.Stop();
                    TimeSpan tempoImpiegato_2 = taskStopwatch.Elapsed;

                    if (stats >= 60)
                    {
                        PrintResourcesAsync();
                        Console.WriteLine("Core: " + maxConcurrentTasks + " Giocatori: " + players.Count());
                        Console.WriteLine($"[PERF] A - Server elaborato in:    [{tempoImpiegato_2.TotalMilliseconds:F4} ms]");
                        Console.WriteLine($"[PERF] B - Min:                    [{min_Stats:F4} ms]");
                        Console.WriteLine($"[PERF] C - Med:                    [{media_Stats:F4} ms]");
                        Console.WriteLine($"[PERF] D - Max:                    [{max_Stats:F4} ms]");
                        Console.WriteLine($"[PERF] E - X player:               [{(media_Stats / players.Count()):F6} ms]\n");

                        Console.WriteLine($"[MONITOR] Client connessi: {Client_Connessi.Count}");
                        Console.WriteLine($"[MONITOR] Client map: {Client_Connessi_Map.Count}");
                        Console.WriteLine($"[MONITOR] Players: {players.Count}");
                        Console.WriteLine($"[MONITOR] PVP: {Utenti_PVP.Count}");
                        Console.WriteLine($"[MONITOR] GC Gen0: {GC.CollectionCount(0)}");
                        Console.WriteLine($"[MONITOR] GC Gen1: {GC.CollectionCount(1)}");
                        Console.WriteLine($"[MONITOR] GC Gen2: {GC.CollectionCount(2)}");
                        Console.WriteLine($"[MONITOR] Heap totale: {GC.GetTotalMemory(false) / 1024 / 1024} MB");
                        Console.WriteLine($"[MONITOR] Thread attivi: {System.Diagnostics.Process.GetCurrentProcess().Threads.Count}");
                        Console.WriteLine($"[MONITOR] WatsonTcp clients: {server.Connections}");
                        Console.WriteLine($"------------------------------------");

                        stats = 0;
                    }

                    if (numero_Stats < 10) numero_Stats += 1;
                    else
                    {
                        numero_Stats += 1;
                        totale_Stats += tempoImpiegato_2.TotalMilliseconds;
                        media_Stats = totale_Stats / numero_Stats;

                        if (tempoImpiegato_2.TotalMilliseconds > max_Stats) max_Stats = tempoImpiegato_2.TotalMilliseconds;
                        if (tempoImpiegato_2.TotalMilliseconds < min_Stats || min_Stats == 0) min_Stats = tempoImpiegato_2.TotalMilliseconds;
                    }
                    #endregion

                    //Tempo reale di attesa....
                    double tempoRimanente = 1000.0 - tempoImpiegato_2.TotalMilliseconds;
                    if (stats >= 60)
                        Console.WriteLine($"[STATS] Tempo rimanente: {tempoRimanente} -- Deve essere <25");
                    if (tempoRimanente <= 0) tempoRimanente = 25;
                    if (tempoRimanente > 0) await Task.Delay((int)tempoRimanente);

                    stats++;
                    if (!avviato) avviato = true;
                }
            }
            public async Task SaveSomePlayersAsync(int count)
            {
                if (players.Count == 0) return;
                if (players.Count < count) count = players.Count;
                var list = players.Values.ToList();

                for (int i = 0; i < count; i++)
                {
                    var player = list[_saveIndex];
                    _saveIndex++;
                    if (_saveIndex >= list.Count) _saveIndex = 0;

                    await GameSave.SavePlayer(player);
                }
                Console.WriteLine(GameSave.SavePath);
            }
            public static void CalcoloPotenza(Player player)
            {
                const int p_Strutture = 25;
                const int p_Ricerca = 1000;

                // Potenza Strutture
                player.Potenza_Strutture = (
                    player.Fattoria + player.Segheria + player.CavaPietra +
                    player.MinieraFerro + player.MinieraOro + player.Abitazioni +
                    player.Workshop_Spade + player.Workshop_Lance + player.Workshop_Archi +
                    player.Workshop_Scudi + player.Workshop_Armature + player.Workshop_Frecce +
                    player.Caserma_Guerrieri + player.Caserma_Lancieri +
                    player.Caserma_Arceri + player.Caserma_Catapulte
                ) * p_Strutture;

                // Potenza Esercito (con loop invece di ripetizioni)
                int[] moltiplicatoriLivello = { 40, 70, 100, 130, 160 };
                player.Potenza_Esercito = 0;

                for (int i = 0; i < 5; i++)
                {
                    int totaleUnita = player.Guerrieri[i] + player.Lanceri[i] + player.Arceri[i] + player.Catapulte[i];
                    player.Potenza_Esercito += totaleUnita * moltiplicatoriLivello[i];
                }

                // Potenza Ricerca
                player.Potenza_Ricerca = (
                    player.Ricerca_Addestramento + player.Ricerca_Costruzione +
                    player.Ricerca_Produzione + player.Ricerca_Popolazione +
                    player.Ricerca_Trasporto + player.Ricerca_Riparazione
                ) * p_Ricerca * 2;

                // Ricerche difensive
                player.Potenza_Ricerca += (
                    player.Ricerca_Cancello_Guarnigione + player.Ricerca_Cancello_Salute +
                    player.Ricerca_Cancello_Difesa + player.Ricerca_Citta_Guarnigione +
                    player.Ricerca_Mura_Guarnigione + player.Ricerca_Mura_Salute +
                    player.Ricerca_Mura_Difesa + player.Ricerca_Torri_Guarnigione +
                    player.Ricerca_Torri_Salute + player.Ricerca_Torri_Difesa
                ) * p_Ricerca;

                // Ricerche unità
                player.Potenza_Ricerca += (
                    player.Guerriero_Livello + player.Guerriero_Attacco +
                    player.Guerriero_Salute + player.Guerriero_Difesa +
                    player.Lancere_Livello + player.Lancere_Attacco +
                    player.Lancere_Salute + player.Lancere_Difesa +
                    player.Arcere_Livello + player.Arcere_Attacco +
                    player.Arcere_Salute + player.Arcere_Difesa +
                    player.Catapulta_Livello + player.Catapulta_Attacco +
                    player.Catapulta_Salute + player.Catapulta_Difesa
                ) * p_Ricerca;

                // Totale
                player.Potenza_Totale = player.Potenza_Strutture + player.Potenza_Esercito + player.Potenza_Ricerca;
            }
            public async Task RunGameLoopSecondarioAsync(CancellationToken cancellationToken) //Task parallelo, andrebbe usato x richiamare cose, costruzioni, tempo, ecc...
            {
                int tempo_1 = 0, execute_2s = 0, saveServer = 0, savePlayer = 0, update_5s = 0, riparazioni = 0;
                bool start = true;
                while (!cancellationToken.IsCancellationRequested)
                {
                    foreach (var player in players.Values)
                    {
                        // -- V2 --
                        BuildingManagerV2.CompleteBuilds(player.guid_Player, player);
                        UnitManagerV2.CompleteRecruitment(player.guid_Player, player);
                        if (tempo_1 >= 2)
                        {
                            if (start)
                            {
                                player.SetupVillaggioGiocatore(player);
                                player.BonusPacchetti();
                                Ripara(player); //Inizializza le riparazioni, se i bool sono true allora ripara. (se ci sono risorse)
                                CalcoloPotenza(player);
                                Esperienza.LevelUp(player);
                                start = false;
                            }

                            if (execute_2s >= 2)
                            {

                            }
                            ResearchManager.CompleteResearch(player.guid_Player, player);

                            if (player.Vip || player.GamePass_Base || player.GamePass_Avanzato) player.BonusPacchetti();
                            if (player.task_Attuale_Costruzioni.Count > 0) player.Tempo_Costruzione++;
                            if (player.task_Attuale_Recutamento.Count > 0) player.Tempo_Addestramento++;
                            if (player.currentTasks_Research.Count > 0) player.Tempo_Ricerca++;

                            if (update_5s >= 5)
                            {
                                player.ManutenzioneEsercito();
                                // 16/09/2026, su richiesta dell'utente: QuestManager.QuestUpdate(player) qui
                                // mandava l'intera struttura delle quest ogni 5 secondi per OGNI giocatore
                                // connesso, a prescindere che fosse cambiato qualcosa o meno (era proprio
                                // questo lo "spam" per cui esisteva il filtro di log in Server.Send più sotto
                                // — vedi il case "Update_Data"/"QuestUpdate"/"QuestRewards"/"Descrizione").
                                // Rimosso: QuestManager.OnEvent (l'UNICO punto da cui passa ogni variazione
                                // di progresso quest) ora chiama da sé QuestUpdateSeCambiato subito quando
                                // qualcosa cambia davvero — il giocatore vede il progresso aggiornarsi
                                // all'istante, invece che aspettare fino a 5 secondi, e non arriva più nulla
                                // quando non è successo nulla.
                                //QuestManager.QuestRewardUpdate(player);
                                player.SetupVillaggioGiocatore(player);
                            }

                            if (riparazioni >= Variabili_Server.tempo_Riparazione)
                            {
                                Ripara(player);
                                CalcoloPotenza(player);
                                //Esperienza.LevelUp(player); //In teoria quando l'esperienza viene aggiunta al giocatore, viene controllato se il giocatore può salire di livello... non penso sia necessario
                            }

                            lock (player.LockCostruzione)
                            {
                                if (player.task_Attuale_Costruzioni.Count > 0)
                                    foreach (var task in player.task_Attuale_Costruzioni)
                                    {
                                        if (player.task_Attuale_Costruzioni[0].IsPaused) task.Resume();
                                        if (!task.IsComplete() && !task.IsPaused)  task.TempoInSecondi -= 1;
                                    }
                                
                                if (player.task_Attuale_Costruzioni.Count == 0)
                                    for (int i = 0; i <= player.Code_Costruzione; i++)
                                        if (player.task_Coda_Costruzioni.Count() > 0)
                                            player.task_Attuale_Costruzioni.Add(player.task_Coda_Costruzioni.Dequeue());
                            }
                            lock (player.LockReclutamento)
                            {
                                if (player.task_Attuale_Recutamento.Count > 0)
                                    foreach (var task in player.task_Attuale_Recutamento)
                                    {
                                        if (player.task_Attuale_Recutamento[0].IsPaused) task.Resume();
                                        if (!task.IsComplete() && !task.IsPaused) task.TempoInSecondi -= 1;
                                    }

                                if (player.task_Attuale_Recutamento.Count == 0)
                                    for (int i = 0; i <= player.Code_Costruzione; i++)
                                        if (player.task_Coda_Recutamento.Count() > 0)
                                            player.task_Attuale_Recutamento.Add(player.task_Coda_Recutamento.Dequeue());
                            }
                            if (player.Tutorial == true && Server.Client_Connessi.Contains(player.guid_Player))
                            {
                                string tutorialData =
                                "Update_Data|" +
                                $"Tutorial_1={player.Tutorial_Stato[0]}|" +
                                $"Tutorial_2={player.Tutorial_Stato[1]}|" +
                                $"Tutorial_3={player.Tutorial_Stato[2]}|" +
                                $"Tutorial_4={player.Tutorial_Stato[3]}|" +
                                $"Tutorial_5={player.Tutorial_Stato[4]}|" +
                                $"Tutorial_6={player.Tutorial_Stato[5]}|" +
                                $"Tutorial_7={player.Tutorial_Stato[6]}|" +
                                $"Tutorial_8={player.Tutorial_Stato[7]}|" +
                                $"Tutorial_9={player.Tutorial_Stato[8]}|" +
                                $"Tutorial_10={player.Tutorial_Stato[9]}|" +
                                $"Tutorial_11={player.Tutorial_Stato[10]}|" +
                                $"Tutorial_12={player.Tutorial_Stato[11]}|" +
                                $"Tutorial_13={player.Tutorial_Stato[12]}|" +
                                $"Tutorial_14={player.Tutorial_Stato[13]}|" +
                                $"Tutorial_15={player.Tutorial_Stato[14]}|" +
                                $"Tutorial_16={player.Tutorial_Stato[15]}|" +
                                $"Tutorial_17={player.Tutorial_Stato[16]}|" +
                                $"Tutorial_18={player.Tutorial_Stato[17]}|" +
                                $"Tutorial_19={player.Tutorial_Stato[18]}|" +
                                $"Tutorial_20={player.Tutorial_Stato[19]}|" +
                                $"Tutorial_21={player.Tutorial_Stato[20]}|" +
                                $"Tutorial_22={player.Tutorial_Stato[21]}|" +
                                $"Tutorial_23={player.Tutorial_Stato[22]}|" +
                                $"Tutorial_24={player.Tutorial_Stato[23]}|" +
                                $"Tutorial_25={player.Tutorial_Stato[24]}|" +
                                $"Tutorial_26={player.Tutorial_Stato[25]}|" +
                                $"Tutorial_27={player.Tutorial_Stato[26]}|" +
                                $"Tutorial_28={player.Tutorial_Stato[27]}|" +
                                $"Tutorial_29={player.Tutorial_Stato[28]}|" +
                                $"Tutorial_30={player.Tutorial_Stato[29]}|" +
                                $"Tutorial_31={player.Tutorial_Stato[30]}|" +
                                $"Tutorial_32={player.Tutorial_Stato[31]}";

                                Server.Send(player.guid_Player, tutorialData);
                                if (player.Tutorial_Stato[31]) player.Tutorial = false;
                            }
                            if (execute_2s >= 2) execute_2s = 0;
                            execute_2s++;
                            update_5s++;
                        }
                    }

                    if (riparazioni >= Variabili_Server.tempo_Riparazione)
                    {
                        AttacchiCooperativi.AggiornaAttacchi();
                        servers_.AggiornaListaPVP();
                        riparazioni = 0;
                    }

                    if (savePlayer >= 80) await SaveSomePlayersAsync(100); //Salva 50 player per volta...
                    if (saveServer >= 1200)
                    {
                        await GameSave.SaveServerData();
                        if (server.Connections > Client_Connessi.Count)
                        {
                            Console.WriteLine($"[ALERT] Client fantasma rilevati! Watson:{server.Connections} vs Lista:{Client_Connessi.Count}");
                            // Disconnetti tutti i client non nella lista
                            foreach (var clientId in server.ListClients())
                                if (!Client_Connessi.Contains(clientId.Guid))
                                {
                                    Console.WriteLine($"[ALERT] Disconnetto client fantasma: {clientId}");
                                    server.DisconnectClientAsync(clientId.Guid);
                                }
                        }
                    }
                    if (tempo_1 >= 2)
                    {
                        await Auto_Update_Clients();
                        tempo_1 = 0;
                    }
                    if (saveServer >= 1200) saveServer = 0;
                    if (savePlayer >= 80) savePlayer = 0;
                    tempo_1++;
                    saveServer++;
                    savePlayer++;
                    riparazioni++;

                    await Task.Delay(500); // Ciclo ogni secondo, o regola il ritardo come necessario
                }
            }
            public static void Ripara(Player player)
            {
                int i = 0, salute = 0, difesa = 0;
                foreach (var item in player.Riparazioni)
                    if (item == true) i++;
                
                if (i == 0) return;
                    
                // Array di strutture da riparare
                var strutture = new[]
                {
                    new {
                        Index = 0,
                        Nome = "Cancello",
                        SaluteAttuale = player.Salute_Cancello,
                        SaluteMax = player.Salute_CancelloMax,
                        Riparazione = Strutture.Riparazione.Cancello,
                        SetSalute = new Action<int>(val => player.Salute_Cancello = val),
                        Tipo = "Salute"
                    },
                    new {
                        Index = 1,
                        Nome = "Cancello",
                        SaluteAttuale = player.Difesa_Cancello,
                        SaluteMax = player.Difesa_CancelloMax,
                        Riparazione = Strutture.Riparazione.Cancello,
                        SetSalute = new Action<int>(val => player.Difesa_Cancello = val),
                        Tipo = "Difesa"
                    },
                    new {
                        Index = 2,
                        Nome = "Mura",
                        SaluteAttuale = player.Salute_Mura,
                        SaluteMax = player.Salute_MuraMax,
                        Riparazione = Strutture.Riparazione.Mura,
                        SetSalute = new Action<int>(val => player.Salute_Mura = val),
                        Tipo = "Salute"
                    },
                    new {
                        Index = 3,
                        Nome = "Mura",
                        SaluteAttuale = player.Difesa_Mura,
                        SaluteMax = player.Difesa_MuraMax,
                        Riparazione = Strutture.Riparazione.Mura,
                        SetSalute = new Action<int>(val => player.Difesa_Mura = val),
                        Tipo = "Difesa"
                    },
                    new {
                        Index = 4,
                        Nome = "Torri",
                        SaluteAttuale = player.Salute_Torri,
                        SaluteMax = player.Salute_TorriMax,
                        Riparazione = Strutture.Riparazione.Torri,
                        SetSalute = new Action<int>(val => player.Salute_Torri = val),
                        Tipo = "Salute"
                    },
                    new {
                        Index = 5,
                        Nome = "Torri",
                        SaluteAttuale = player.Difesa_Torri,
                        SaluteMax = player.Difesa_TorriMax,
                        Riparazione = Strutture.Riparazione.Torri,
                        SetSalute = new Action<int>(val => player.Difesa_Torri = val),
                        Tipo = "Difesa"
                    },
                    new {
                        Index = 6,
                        Nome = "Castello",
                        SaluteAttuale = player.Salute_Castello,
                        SaluteMax = player.Salute_CastelloMax,
                        Riparazione = Strutture.Riparazione.Castello,
                        SetSalute = new Action<int>(val => player.Salute_Castello = val),
                        Tipo = "Salute"
                    },
                    new {
                        Index = 7,
                        Nome = "Castello",
                        SaluteAttuale = player.Difesa_Castello,
                        SaluteMax = player.Difesa_CastelloMax,
                        Riparazione = Strutture.Riparazione.Castello,
                        SetSalute = new Action<int>(val => player.Difesa_Castello = val),
                        Tipo = "Difesa"
                    }
                };

                foreach (var struttura in strutture)
                {
                    if (player.Riparazioni[struttura.Index] == false) continue;
                    if (struttura.SaluteAttuale >= struttura.SaluteMax)
                    {
                        player.Riparazioni[struttura.Index] = false;
                        struttura.SetSalute(struttura.SaluteMax);
                        continue;
                    }

                    // Verifica risorse disponibili
                    if (player.Cibo >= struttura.Riparazione.Consumo_Cibo &&
                        player.Legno >= struttura.Riparazione.Consumo_Legno &&
                        player.Pietra >= struttura.Riparazione.Consumo_Pietra &&
                        player.Ferro >= struttura.Riparazione.Consumo_Ferro &&
                        player.Oro >= struttura.Riparazione.Consumo_Oro)
                    {
                        // Consuma risorse
                        player.Cibo -= struttura.Riparazione.Consumo_Cibo;
                        player.Legno -= struttura.Riparazione.Consumo_Legno;
                        player.Pietra -= struttura.Riparazione.Consumo_Pietra;
                        player.Ferro -= struttura.Riparazione.Consumo_Ferro;
                        player.Oro -= struttura.Riparazione.Consumo_Oro;

                        // Ripara (Salute o Difesa)
                        int incremento = struttura.Tipo == "Salute"
                            ? (int)(struttura.Riparazione.Salute * (1 + player.Bonus_Riparazione))
                            : (int)(struttura.Riparazione.Difesa * (1 + player.Bonus_Riparazione));
                        if (struttura.Tipo == "Salute") salute++;
                        else difesa++;

                        struttura.SetSalute(struttura.SaluteAttuale + incremento);
                    } 
                    else
                    {
                        player.Riparazioni[struttura.Index] = false;
                        Console.WriteLine($"[Riparazioni] Giocatore: {player.Username}. Riparazione interrotta {struttura.Nome}");
                    }
                }
                if (salute + difesa == 0) Console.WriteLine($"[Riparazioni] Giocatore: {player.Username}. Risorse insufficienti");
                else Console.WriteLine($"[Riparazioni] Giocatore: {player.Username} HP: +{salute} DEF +{difesa}");
            }
            public static void GuerrieriCitta(Player player)
            {
                player.Guarnigione_Ingresso = player.Guerrieri_Ingresso.Sum() + player.Lanceri_Ingresso.Sum() + player.Arceri_Ingresso.Sum() + player.Catapulte_Ingresso.Sum();
                player.Guarnigione_Citta = player.Guerrieri_Citta.Sum() + player.Lanceri_Citta.Sum() + player.Arceri_Citta.Sum() + player.Catapulte_Citta.Sum();
                player.Guarnigione_Cancello = player.Guerrieri_Cancello.Sum() + player.Lanceri_Cancello.Sum() + player.Arceri_Cancello.Sum() + player.Catapulte_Cancello.Sum();
                player.Guarnigione_Mura = player.Guerrieri_Mura.Sum() + player.Lanceri_Mura.Sum() + player.Arceri_Mura.Sum() + player.Catapulte_Mura.Sum();
                player.Guarnigione_Torri = player.Guerrieri_Torri.Sum() + player.Lanceri_Torri.Sum() + player.Arceri_Torri.Sum() + player.Catapulte_Torri.Sum();
                player.Guarnigione_Castello = player.Guerrieri_Castello.Sum() + player.Lanceri_Castello.Sum() + player.Arceri_Castello.Sum() + player.Catapulte_Castello.Sum();
            }
            public IEnumerable<Player> GetAllPlayers()
            {
                return players.Values;
            }
        }
    }
}
