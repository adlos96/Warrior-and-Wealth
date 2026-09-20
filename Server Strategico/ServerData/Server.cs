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
    using System;
    using System.Collections.Generic;
    using System.Threading;

    
    internal class Server
    {
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

        private CancellationTokenSource cts_1, cts_2;
        private Task primaryGameLoopTask, secondaryGameLoopTask;
        static public GameServer servers_ = new GameServer();

        public static double totale_Stats = 0, media_Stats = 0, min_Stats = 0, max_Stats = 0, numero_Stats = 0;
        static bool avviato = false;

        private Server()
        {
            string subjectName = Environment.MachineName; //Ottine il nome della macchina (hostname)
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
                _CertPass = "";
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

            //Web
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
                if (Admin.adminStart == true)
                {
                    Console.WriteLine("Info Comandi: \"?\"");
                    Console.WriteLine($"/comandi per la lista");
                }
                else Console.WriteLine("Info Comandi: \"?\"");

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
                if (userInput.Contains("/") && Admin.adminStart == true)
                {
                    Admin.AvviaConsoleAdmin(userInput);
                }

                switch (userInput)
                {
                    case "?":
                        Console.WriteLine("");
                        Console.WriteLine("                         *** Command ***");
                        Console.WriteLine("----------------------------------------------------------------------");
                        Console.WriteLine("Comando vuoto:                       [player]");                      // 
                        Console.WriteLine("Comando vuoto:                       [battaglia]");                      // 
                        Console.WriteLine("Comando vuoto:                       [spionaggio]");                      // 
                        Console.WriteLine("Abilita i comandi admin con '/':     [adminstart]");                      //
                        Console.WriteLine("Comando vuoto:                       [adminstop]");                      //

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
                    case "battaglia":
                        BattagliaPVP.TestBattaglia();
                        break;
                    case "spionaggio":
                        Spionaggio.EseguiSpionaggioTEST();
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
                    case "adminstart":
                        Admin.adminStart = true;
                        break;
                    case "adminstop":
                        Admin.adminStart = false;
                        break;

                    default: Console.WriteLine("[Server] >> Comando sconosciuto"); break;
                }
            }
        }
        
        public async static Task<Player> PlayerID(int id)
        {
            Player player = null;
            var tempPlayer = Server.servers_.players.Values;
            foreach (var giocatori in tempPlayer)
                if (giocatori.ID == id) player = giocatori;
            return player;
        }

        private async Task StartGame()
        {
            cts_1 = new CancellationTokenSource();
            cts_2 = new CancellationTokenSource();

            primaryGameLoopTask = servers_.RunGameLoopAsync(cts_1.Token);
            _ = primaryGameLoopTask.ContinueWith(t => Console.WriteLine($"[FATAL] Loop primario terminato: {t.Exception}"), TaskContinuationOptions.OnlyOnFaulted);

            secondaryGameLoopTask = Task.Run(() => servers_.RunGameLoopSecondarioAsync(cts_2.Token));
            _ = secondaryGameLoopTask.ContinueWith(t => Console.WriteLine($"[FATAL] Loop secondario terminato: {t.Exception}"), TaskContinuationOptions.OnlyOnFaulted);

            Console.WriteLine("[Server] Attesa avvio server....");
            while (!avviato)
            {
                if (primaryGameLoopTask.IsFaulted)
                {
                    Console.WriteLine("[FATAL] Avvio fallito, esco (vedi errore sopra).");
                    Environment.Exit(1);   // meglio uscire che restare in piedi senza dati; systemd/Docker riavviano
                }
                Thread.Sleep(1000);
            }
            Console.WriteLine("[Server] Server avviato!");

            try { WebSocketGateway.Start(Variabili_Server.WebGatewayPort); }
            catch (Exception ex) { Console.WriteLine($"[Server] Errore avvio WebSocketGateway: {ex.Message}"); }
            Console.WriteLine("-----------------------------------------------------------");
        }
        private async Task StopGame()
        {
            if (cts_1 != null)
            {
                cts_1.Cancel(); // Ferma il loop di gioco
                await primaryGameLoopTask; // Attende che il loop si fermi completamente
                Console.WriteLine("Il gioco è terminato.");
            }
            else Console.WriteLine("Il gioco non è attualmente in esecuzione.");

            if (cts_2 != null)
            {
                cts_2.Cancel(); // Ferma il loop di gioco
                await secondaryGameLoopTask; // Attende che il loop si fermi completamente
                Console.WriteLine("Il gioco è terminato.");
            }
            else Console.WriteLine("Il gioco non è attualmente in esecuzione.");
        }
        public static void Send(Guid guid, string msg)
        {
            if (guid == Guid.Empty) return;

            // Instradamento per trasporto: i client del gateway web (vedi
            // WebSocketGateway.cs) non sono client WatsonTcp, quindi vanno
            // inviati con il loro socket.
            bool inviato = false;
            if (WebSocketGateway.IsWebSocketClient(guid))
            {
                WebSocketGateway.Send(guid, msg);
                inviato = true;
            }
            else if (Client_Connessi_Map.ContainsKey(guid))
            {
                server.SendAsync(guid, msg);
                inviato = true;
            }

            // 16/09/2026, su richiesta dell'utente: la Cronologia (pannello "Log_Server" del
            // client) ora viene anche salvata lato server, non solo mostrata "al volo" — così
            // sopravvive a un ricollegamento o a un riavvio del server invece di sparire ogni
            // volta. Risale al giocatore dal guid tramite la stessa mappa già usata per il
            // routing dei messaggi, invece di aggiungere un parametro Player a ogni singola
            // chiamata a Send sparsa in centinaia di punti del codice.
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
            try
            {
                Guid guid = args.Client.Guid;   // locale: lastGuid è static e condiviso tra thread
                Console.WriteLine("[SERVER|LOG] > Client connesso: " + args.Client);

                Client_Connessi_Map.TryAdd(guid, args.Client.IpPort);
                Send(guid, $"Update_Data|versione_Client_Necessario={Variabili_Server.versione_Client_Necessario}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SERVER|LOG] (Errore) > ClientConnected: {ex}");
            }
        }
        static async void ClientDisconnected(object? sender, DisconnectionEventArgs args)
        {
            try
            {
                Guid guid = args.Client.Guid;
                Console.WriteLine("[SERVER|LOG] > Client disconnesso: " + args.Client + ": " + args.Reason);

                Client_Connessi_Map.TryRemove(guid, out _);
                await server.DisconnectClientAsync(guid);   // forza pulizia su WatsonTcp
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SERVER|LOG] (Errore) > ClientDisconnected: {ex.Message}");
            }
        }
        public static async Task<bool> DisconnettiGiocatore(string username)
        {
            var player = servers_.GetPlayer(username);
            if (player == null)
            {
                Console.WriteLine($"[SERVER|LOG] > Giocatore '{username}' non trovato.");
                return false;
            }

            if (player.guid_Player == Guid.Empty || !Client_Connessi_Map.ContainsKey(player.guid_Player))
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
            try
            {
                if (args.Data == null || args.Data.Length == 0)
                {
                    Console.WriteLine("[SERVER|LOG] > [null]");
                    return;
                }
                Console.Write("[SERVER|LOG] > " + args.Data.Length + " byte message from " + args.Client + ": \r");
                ServerConnection.HandleClientRequest(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SERVER|LOG] (Errore) > Messaggio da {args.Client}: {ex}");
            }
        }

        static void Logger(Severity sev, string msg)
        {
            Console.WriteLine("[SERVER|LOG] (" + sev.ToString() + ") > " + msg);
        }

        public class GameServer
        {
            public System.Collections.Concurrent.ConcurrentDictionary<string, Player> players = new();
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

            private static bool IsConnesso(Player p) => p.guid_Player != Guid.Empty && (Client_Connessi_Map.ContainsKey(p.guid_Player) || WebSocketGateway.IsWebSocketClient(p.guid_Player));
            public void Player_Creati(bool soloConnessi = false, int massimo = 100)
            {
                try
                {
                    const int larghUsername = 22;
                    const int larghLivello = 9;
                    const int larghPotenza = 10;

                    // Snapshot: immune alle modifiche degli altri thread
                    Player[] snapshot = players.Values.ToArray();
                    int totale = snapshot.Length;
                    int totaleConnessi = snapshot.Count(IsConnesso);

                    IEnumerable<Player> filtrati = soloConnessi ? snapshot.Where(IsConnesso) : snapshot;
                    int totaleFiltrati = soloConnessi ? totaleConnessi : totale;
                    Player[] daMostrare = filtrati.OrderBy(p => p.ID).Take(massimo).ToArray();

                    string intestazione =
                        "   " +
                        "ID".PadRight(larghLivello) +
                        "Username".PadRight(larghUsername) +
                        "Livello".PadRight(larghLivello) +
                        "Potenza".PadRight(larghPotenza) +
                        "Ultimo accesso";
                    string separatore = new string('─', intestazione.Length);

                    Console.WriteLine();
                    Console.WriteLine($"Giocatori registrati: {totale} | connessi: {totaleConnessi}");
                    Console.WriteLine(separatore);
                    Console.WriteLine(intestazione);
                    Console.WriteLine(separatore);

                    foreach (var player in daMostrare)
                    {
                        try
                        {
                            bool connesso = IsConnesso(player);

                            Console.ForegroundColor = connesso ? ConsoleColor.Green : ConsoleColor.DarkGray;
                            Console.Write(connesso ? " ● " : " ○ ");
                            Console.ResetColor();

                            string username = string.IsNullOrWhiteSpace(player.Username) ? "(senza nome)" : player.Username;
                            if (username.Length > larghUsername - 1)
                                username = username.Substring(0, larghUsername - 4) + "...";

                            string ultimoAccesso = player.Last_Login == DateTime.MinValue
                                ? "mai"
                                : player.Last_Login.ToString("dd/MM/yyyy");

                            Console.WriteLine(
                                player.ID.ToString().PadRight(larghLivello) +
                                username.PadRight(larghUsername) +
                                player.Livello.ToString().PadRight(larghLivello) +
                                player.Potenza_Totale.ToString("#,0").PadRight(larghPotenza) +
                                ultimoAccesso);
                        }
                        catch (Exception ex)
                        {
                            Console.ResetColor();
                            Console.WriteLine($" ! Errore stampa giocatore {player?.Username}: {ex.Message}");
                        }
                    }

                    Console.WriteLine(separatore);
                    if (daMostrare.Length < totaleFiltrati)
                        Console.WriteLine($"Mostrati {daMostrare.Length} di {totaleFiltrati}. Usa 'player tutti' oppure 'player <numero>'.");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.ResetColor();
                    Console.WriteLine($"[SERVER|LOG] (Errore) > Player_Creati: {ex}");
                }
            }
            public void AggiornaListaPVP()
            {
                if (Client_Connessi_Map.Count == 0) return;
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
                var tempPlayer = players;
                foreach (var kv in tempPlayer)
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
                try
                {
                    var tempPlayer = players;
                    foreach (var item in tempPlayer)
                        if (item.Value.Username == username)
                            return false;
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[LOOP1] Errore su Check_Username_Player: {ex}");
                    return false;
                }
            }
            // All'interno della classe GameServer

            public async Task Auto_Update_Clients() // Sostituisce il metodo esistente
            {
                try
                {
                    // --- PARTE 1: Gestione Disconnessioni/Cleanup (Seriale) ---
                    if (Client_Connessi_Map.Count == 0)
                    {
                        // Se non ci sono client connessi, azzera i GUID nei giocatori non connessi.
                        // Questa iterazione O(N) è accettabile perché avviene solo quando Client_Connessi_Map.Count == 0
                        // e ripulisce lo stato.
                        var tempPlayer = players;
                        foreach (var player in tempPlayer.Values.Where(p => p.guid_Player != Guid.Empty))
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
                catch (Exception ex)
                {
                    Console.WriteLine($"[LOOP1] Errore su Check_Username_Player: {ex}");
                }
                
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
                    Variabili_Server._Server_Consumo_RAM = (int)GetAccurateRamMb(proc);
                    Console.WriteLine($"[Server] Baseline RAM impostata: {Variabili_Server._Server_Consumo_RAM:F2} MB");
                }
                //await addBOT(500000);

                await GameSave.LoadServerData();
                await GameSave.LoadAllPlayersData();
                servers_.AggiornaListaPVP();
                await Gioco.Barbari.Inizializza();
                ScheduleManager.AvvioReset();

                int maxConcurrentTasks = Math.Max(1, Environment.ProcessorCount);
                var options = new ParallelOptions { MaxDegreeOfParallelism = maxConcurrentTasks };

                while (!cancellationToken.IsCancellationRequested)
                {
                    Stopwatch taskStopwatch = Stopwatch.StartNew();
                    try
                    {
                        await Task.Run(() =>
                            Parallel.ForEach(players.Values, options, player =>
                            {
                                try
                                {
                                    if (player.Stato_Giocatore == false)
                                    {
                                        player.ProduceResources();
                                        player.ManutenzioneEsercito();
                                        return;
                                    }
                                    if (player.Email_Code_Time > 0) player.Email_Code_Time--;

                                    player.ProduceResources();
                                    player.ServerTimer();
                                    //player.ResetGiornaliero();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"[LOOP1] Errore su {player.Username}: {ex}");
                                }
                            })
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[LOOP1] Errore su GameloopPrimario: {ex}");
                    }

                    try
                    {
                        if (Variabili_Server.timer_Reset_Quest > 0) Variabili_Server.timer_Reset_Quest--;
                        if (Variabili_Server.timer_Reset_Quest == 0) QuestManager.RigeneraQuest();
                        if (Variabili_Server.timer_Reset_Barbari > 0) Variabili_Server.timer_Reset_Barbari--;
                        if (Variabili_Server.timer_Reset_Barbari == 0) Barbari.RigeneraBarbari();
                    }
                    catch (Exception ex) { Console.WriteLine($"[LOOP1] Errore timer quest/barbari: {ex}"); }

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
                    {
                        stats = 0;
                        Console.WriteLine($"[STATS] Tempo rimanente: {tempoRimanente} -- Deve essere < 25");
                    }
                    if (tempoRimanente <= 0) tempoRimanente = 25;
                    if (tempoRimanente > 0) await Task.Delay((int)tempoRimanente);

                    stats++;
                    if (!avviato) avviato = true;
                }
            }
            public async Task RunGameLoopSecondarioAsync(CancellationToken cancellationToken) //Task parallelo, andrebbe usato x richiamare cose, costruzioni, tempo, ecc...
            {
                int tempo_1 = 0, saveServer = 0, savePlayer = 0, update_5s = 0, riparazioni = 0;
                bool start = true;

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        foreach (var player in players.Values)
                        {
                            try
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
                                    ResearchManager.CompleteResearch(player.guid_Player, player);

                                    if (player.Vip || player.GamePass_Base || player.GamePass_Avanzato) player.BonusPacchetti();
                                    if (player.task_Attuale_Costruzioni.Count > 0) player.Tempo_Costruzione++;
                                    if (player.task_Attuale_Recutamento.Count > 0) player.Tempo_Addestramento++;
                                    if (player.currentTasks_Research.Count > 0) player.Tempo_Ricerca++;

                                    if (update_5s >= 10)
                                    {
                                        player.ManutenzioneEsercito();
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
                                                if (!task.IsComplete() && !task.IsPaused) task.TempoInSecondi -= 1;
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
                                    if (player.Tutorial == true && Server.Client_Connessi_Map.ContainsKey(player.guid_Player))
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
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[LOOP1] Errore su {player.Username}: {ex}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[LOOP1] Errore su LoopSecondario: {ex}");
                    }

                    try
                    {
                        if (riparazioni > 5)
                        {
                            riparazioni = 0;
                            AttacchiCooperativi.AggiornaAttacchi();
                            servers_.AggiornaListaPVP();
                        }
                    }
                    catch (Exception ex) { Console.WriteLine($"[LOOP2] Errore attacchi/PVP: {ex}"); }

                    if (savePlayer >= 180)
                    {
                        savePlayer = 0;
                        try { await SaveSomePlayersAsync(500); }
                        catch (Exception ex) { Console.WriteLine($"[LOOP2] Errore salvataggio giocatori: {ex}"); }
                    }

                    if (saveServer >= 600)
                    {
                        saveServer = 0;
                        try
                        {
                            await GameSave.SaveServerData();

                            if (server.Connections > Client_Connessi_Map.Count)
                            {
                                Console.WriteLine($"[ALERT] Client fantasma: Watson {server.Connections} vs mappa {Client_Connessi_Map.Count}");
                                foreach (var c in server.ListClients())
                                    if (!Client_Connessi_Map.ContainsKey(c.Guid))
                                    {
                                        Console.WriteLine($"[ALERT] Disconnetto client fantasma: {c}");
                                        await server.DisconnectClientAsync(c.Guid);
                                    }
                            }
                        }
                        catch (Exception ex) { Console.WriteLine($"[LOOP2] Errore salvataggio server/fantasmi: {ex}"); }
                    }
                    
                    if (tempo_1 >= 2)
                    {
                        tempo_1 = 0;
                        await Auto_Update_Clients();
                    }
                    if (update_5s >= 10) update_5s = 0;
                    
                    tempo_1++;
                    saveServer++;
                    savePlayer++;
                    riparazioni++;
                    update_5s++;

                    await Task.Delay(500); // Ciclo ogni secondo, o regola il ritardo come necessario
                }
            }
            public async Task SaveSomePlayersAsync(int count)
            {
                int giocatoriServer = players.Count;
                int giocatoriSalvati = 0;
                int giocatoriFalliti = 0;
                if (giocatoriServer == 0) return;
                if (giocatoriServer < count) count = giocatoriServer;
                var list = players.Values.ToList();

                for (int i = 0; i < count; i++)
                {
                    var player = list[_saveIndex];
                    _saveIndex++;
                    if (_saveIndex >= list.Count) _saveIndex = 0;

                    if (!await GameSave.SavePlayer(player)) 
                        giocatoriFalliti++;
                    giocatoriSalvati++;
                }
                Console.WriteLine($"[SERVER] Salvataggio completato per {giocatoriSalvati}/{count} giocatori su {giocatoriServer} totali. (Falliti: {giocatoriFalliti})");
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
            public static void Ripara(Player player)
            {
                int i = 0, salute = 0, difesa = 0;
                var playerRiparazioni = player.Riparazioni;
                foreach (var item in playerRiparazioni)
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
