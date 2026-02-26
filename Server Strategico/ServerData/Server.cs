using Server_Strategico.Gioco;
using Server_Strategico.Manager;
using Server_Strategico.ServerData.Moduli;
using System.Collections.Concurrent;
using System.Diagnostics;
using WatsonTcp;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Server.Server;

namespace Server_Strategico.Server
{
    internal class Server
    {
        public static List<Guid> Client_Connessi = new List<Guid>();
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

        private Server()
        {
            string subjectName = Environment.MachineName; //Ottine il nome della macchina (hostname)

            if (OperatingSystem.IsWindows())
            {
                Console.WriteLine("Siamo su Windows");
                if (subjectName == "DESKTOP-DOBLVTI" || subjectName == "ADLO") serverIp = "0.0.0.0";
            }
            else if (OperatingSystem.IsLinux())
            {
                Console.WriteLine("Siamo su Linux");
                GameSave.SavePath = "/opt/warriorandwealth/Saves_Test";
            }
            GameSave.Initialize();

            if (!_Ssl) server = new WatsonTcpServer(serverIp, serverPort);
            else
            {
                _CertPass = Password.pasword;
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
            Task task = StartGame();

            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("Info Comandi: \"?\"");
                var userInput = Console.ReadLine() ?? string.Empty;

                switch (userInput)
                {
                    case "?":
                        Console.WriteLine("");
                        Console.WriteLine("                         *** Command ***");
                        Console.WriteLine("----------------------------------------------------------------------");
                        Console.WriteLine("Comando vuoto:                 [player]");                      // 
                        Console.WriteLine("Comando vuoto:                 [client]");                      // 

                        Console.WriteLine("----------------------------------------------------------------------");
                        break;
                    case "player":
                        servers_.Player_Creati();
                        break;
                    case "client":
                        ClientConnessi();
                        break;

                    default: Console.WriteLine("[Server] >> Comando sconosciuto"); break;
                }
            }
        }
        void ClientConnessi()
        {
            foreach (var item in Client_Connessi)
                Console.WriteLine($"Client: {item}");
        }
        private async Task StartGame()
        {
            cts = new CancellationTokenSource();
            gameLoopTask = servers_.RunGameLoopAsync(cts.Token);
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
            if (Client_Connessi.Contains(guid) && guid != Guid.Empty)
            {
                server.SendAsync(guid, msg);
                if (!msg.Contains("Update_Data") && !msg.Contains("QuestRewards") && !msg.Contains("QuestUpdate")) 
                    Console.WriteLine($"[SERVER|LOG] > {msg}");
                Console.WriteLine($"[SERVER|LOG] > {msg}");
            }
        }

        public static async Task NewPlayer(string player, string password)
        {
            var player1 = servers_.GetPlayer(player, password);

            player1.Tutorial = true;
            //player1.Cibo = 20000;
            //player1.Legno = 20000;
            //player1.Pietra = 20000;
            //player1.Ferro = 20000;
            //player1.Oro = 20000;
            //player1.Popolazione = 1000;

            //player1.Spade = 2000;
            //player1.Lance = 2000;
            //player1.Archi = 2000;
            //player1.Scudi = 2000;
            //player1.Armature = 2000;
            //player1.Frecce = 200;

            //player1.Caserma_Guerrieri = 5;
            //player1.Caserma_Lancieri = 5;
            //player1.Caserma_Arceri = 5;
            //player1.Caserma_Catapulte = 5;

            //player1.Diamanti_Blu = 15000;
            //player1.Diamanti_Viola = 30000;

            //Test battaglie
            //if (!player1.Tutorial) //Senza tutorial, per test truppe e battaglie
            //{
            //    player1.Guerrieri = [25, 0, 0, 0, 0];
            //    player1.Lanceri = [25, 0, 0, 0, 0];
            //    player1.Arceri = [25, 0, 0, 0, 0];
            //    player1.Catapulte = [25, 0, 0, 0, 0];
            //
            //    player1.Guerrieri_Ingresso = [5, 0, 0, 0, 0];
            //    player1.Lanceri_Ingresso = [5, 0, 0, 0, 0];
            //    player1.Arceri_Ingresso = [5, 0, 0, 0, 0];
            //    player1.Catapulte_Ingresso = [0, 0, 0, 0, 0];
            //
            //    player1.Guerrieri_Cancello = [5, 0, 0, 0, 0];
            //    player1.Lanceri_Cancello = [5, 0, 0, 0, 0];
            //    player1.Arceri_Cancello = [5, 0, 0, 0, 0];
            //    player1.Catapulte_Cancello = [5, 0, 0, 0, 0];
            //}

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
            public async Task<bool> AddPlayer(string username, string password, Guid guid)
            {
                if (!players.ContainsKey(username))
                {
                    players.Add(username, new Player(username, password, guid));
                    await NewPlayer(username, password);
                    return true;
                }
                else
                {
                    return false;
                    throw new ArgumentException($"Player già presente con questo username {username} e password {password}.");
                }
            }

            public Player GetPlayer(string username, string password)
            {
                if (players.TryGetValue(username, out Player player))
                {
                    if (player.ValidatePassword(password))
                        return player;
                    else
                    {
                        return null;
                        throw new UnauthorizedAccessException("Invalid password.");
                    }
                }
                else
                {
                    return null;
                    throw new KeyNotFoundException("Player not found.");
                }
            }
            public Player GetPlayer_Data(string username)
            {
                if (players.TryGetValue(username, out Player player))
                    return player;
                else
                {
                    return null;
                    throw new KeyNotFoundException("Player not found.");
                }
            }
            public void Player_Creati()
            {
                Console.WriteLine($"Numero Giocatori: {players.Count()}");
                foreach (var item in players)
                    Console.WriteLine($"Giocatore: {item.Value.Username} Guid: {item.Value.guid_Player}, Livello: {item.Value.Livello}, Last: {item.Value.Last_Login}");
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
                double ramMb = proc.WorkingSet64 / 1024.0 / 1024.0; // RAM in Byte
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
                Console.WriteLine($"[Server Resources] Totale - RAM: {ramMb:F2} MB | CPU: {cpuPercent:F2} %");
                Console.WriteLine($"[Server Resources] X player - RAM: {(ramMb - Variabili_Server._Server_Consumo_RAM) / players.Count()*1024:F2} KB");
                await Task.CompletedTask;
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
                    await AddPlayer($"Fake{i}", "123", Guid.Empty);
            }
            public async Task RunGameLoopAsync(CancellationToken cancellationToken)
            {
                int saveCounterPlayer = 0, riparazioni = 0, _firstStart = 0, stats = 0, update_5s = 0;
                double totale_Stats = 0, media_Stats = 0, min_Stats = 0, max_Stats = 0, numero_Stats = 0;

                // --- BLOCCO RIPRISTINATO: GENERAZIONE GIOCATORI ---
                if (Variabili_Server._Server_Consumo_RAM == 0)
                {
                    Process proc = Process.GetCurrentProcess();
                    Variabili_Server._Server_Consumo_RAM = (int)(proc.WorkingSet64 / 1024.0 / 1024.0);
                    Console.WriteLine($"[Server] Baseline RAM impostata: {Variabili_Server._Server_Consumo_RAM:F2} MB");
                }
                //await addBOT(500000);
                // --- BLOCCO RIPRISTINATO: INIZIALIZZAZIONE ---
                await GameSave.LoadServerData();
                await GameSave.Load_Player_Data_Auto();
                servers_.AggiornaListaPVP();
                await Gioco.Barbari.Inizializza();
                _ = Task.Run(() => CompleteTask(cancellationToken));
                ScheduleManager.AvvioReset();
                // ----------------------------------------------
                int maxConcurrentTasks = Math.Max(1, Environment.ProcessorCount); //Core disponibili
                var workers = new Task[maxConcurrentTasks]; // N task = N core
                var queue = new ConcurrentQueue<Player>(players.Values);  // Pool condiviso (consuma molta cpu e tempo...)
                while (!cancellationToken.IsCancellationRequested)
                {
                    queue.Clear();
                    foreach (var p in players.Values) queue.Enqueue(p);

                    Stopwatch taskStopwatch = Stopwatch.StartNew();
                    saveCounterPlayer++;
                    riparazioni++;
                    stats++;
                    update_5s++;

                    for (int i = 0; i < maxConcurrentTasks; i++)
                        workers[i] = Task.Run(() =>
                        {
                            while (queue.TryDequeue(out var player))
                            {
                                //Eseguire qualche funzione obbligatoria anche se il giocatore non è più attivo
                                if (player.Stato_Giocatore == false)
                                {
                                    player.ProduceResources();
                                    player.ManutenzioneEsercito();
                                    continue; // Se il giocatore è inattivo, salta tutte le operazioni e passa al successivo.
                                }

                                if (_firstStart == 0) // X "Scaldare" i thread
                                {
                                    player.SetupVillaggioGiocatore(player); //Richiamare solo quando effettivamente c'è bisogno +- 145 ms in più su 50000 player
                                    player.BonusPacchetti();
                                    Ripara(player);
                                    CalcoloPotenza(player);
                                    Esperienza.LevelUp(player);
                                    _firstStart++;
                                }

                                if (riparazioni >= Variabili_Server.tempo_Riparazione)
                                {
                                    Ripara(player);
                                    CalcoloPotenza(player);
                                    Esperienza.LevelUp(player);
                                }

                                //GuerrieriCitta(player);
                                player.ProduceResources();
                                //if (update_5s >= 5) player.ManutenzioneEsercito();
                                player.ServerTimer();
                                player.ResetGiornaliero();
                            }
                        });
                    await Task.WhenAll(workers); // Attendiamo il completamento di tutti i Task... qui avvienecl'esecuzione del codice sopra

                    if (riparazioni >= Variabili_Server.tempo_Riparazione)
                    {
                        riparazioni = 0;
                        AttacchiCooperativi.AggiornaAttacchi();
                        servers_.AggiornaListaPVP();
                    }
                    //Auto_Update_Clients();

                    if (update_5s >= 5) update_5s = 0;
                    if (Variabili_Server.timer_Reset_Quest > 0) Variabili_Server.timer_Reset_Quest--;
                    if (Variabili_Server.timer_Reset_Quest == 0) QuestManager.RigeneraQuest();
                    if (Variabili_Server.timer_Reset_Barbari > 0) Variabili_Server.timer_Reset_Barbari--;
                    if (Variabili_Server.timer_Reset_Barbari == 0) Barbari.RigeneraBarbari();

                    //// Log del tempo totale
                    taskStopwatch.Stop();
                    TimeSpan tempoImpiegato_2 = taskStopwatch.Elapsed;//Tempo strascorso
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
                        Console.WriteLine($"[MONITOR] GC Gen0: {GC.CollectionCount(0)}");
                        Console.WriteLine($"[MONITOR] GC Gen1: {GC.CollectionCount(1)}");
                        Console.WriteLine($"[MONITOR] GC Gen2: {GC.CollectionCount(2)}");
                        // Memoria gestita da .NET
                        Console.WriteLine($"[MONITOR] Heap totale: {GC.GetTotalMemory(false) / 1024 / 1024} MB");

                        // Quanti thread sta usando il server
                        Console.WriteLine($"[MONITOR] Thread attivi: {System.Diagnostics.Process.GetCurrentProcess().Threads.Count}");

                        // Quante connessioni WatsonTcp risultano aperte
                        Console.WriteLine($"[MONITOR] WatsonTcp clients: {server.Connections}");

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

                    // Regolazione dinamica del ritardo (per mantenere 1000ms come limite)
                    double tempoRimanente = 1000.0 - tempoImpiegato_2.TotalMilliseconds;
                    if (tempoRimanente <= 0) tempoRimanente = 50;
                    if (tempoRimanente > 0) await Task.Delay((int)tempoRimanente);
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
            public async Task CompleteTask(CancellationToken cancellationToken) //Task parallelo, andrebbe usato x richiamare cose, costruzioni, tempo, ecc...
            {
                int tempo_1 = 0, execute_2s = 0, saveServer = 0, savePlayer = 0, update_5s = 0;
                while (!cancellationToken.IsCancellationRequested)
                {
                    foreach (var player in players.Values)
                    {
                        // Vecchio codice completamento
                        //BuildingManager.CompleteBuilds(player.guid_Player, player);
                        //UnitManager.CompleteRecruitment(player.guid_Player, player);
                        //ResearchManager.CompleteResearch(player.guid_Player, player);

                        // -- V2 --
                        BuildingManagerV2.CompleteBuilds(player.guid_Player, player);
                        UnitManagerV2.CompleteRecruitment(player.guid_Player, player);
                        if (tempo_1 >= 4)
                        {
                            //if (execute_2s >= 2) QuestManager.QuestUpdate(player);
                            ResearchManager.CompleteResearch(player.guid_Player, player);

                            if (player.Vip || player.GamePass_Base || player.GamePass_Avanzato) player.BonusPacchetti();
                            if (player.task_Attuale_Costruzioni.Count > 0) player.Tempo_Costruzione++;
                            if (player.task_Attuale_Recutamento.Count > 0) player.Tempo_Addestramento++;
                            if (player.currentTasks_Research.Count > 0) player.Tempo_Ricerca++;

                            if (update_5s >= 5)
                            {
                                player.ManutenzioneEsercito();
                                //player.SetupVillaggioGiocatore(player);
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

                    if (saveServer >= 1200)
                    {
                        await GameSave.SaveServerData();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                    }
                    if (tempo_1 >= 4)
                    {
                        await Auto_Update_Clients();
                        tempo_1 = 0;
                    }
                    if (savePlayer >= 80) await SaveSomePlayersAsync(100); //Salva 50 player per volta...
                    if (saveServer >= 1200) saveServer = 0;
                    if (savePlayer >= 80) savePlayer = 0;
                    tempo_1++;
                    saveServer++;
                    savePlayer++;

                    await Task.Delay(250); // Ciclo ogni secondo, o regola il ritardo come necessario
                }
            }
            public static void Ripara(Player player)
            {
                // Array di strutture da riparare
                var strutture = new[]
                {
                    new {
                        Index = 0,
                        SaluteAttuale = player.Salute_Cancello,
                        SaluteMax = player.Salute_CancelloMax,
                        Riparazione = Strutture.Riparazione.Cancello,
                        SetSalute = new Action<int>(val => player.Salute_Cancello = val),
                        Tipo = "Salute"
                    },
                    new {
                        Index = 1,
                        SaluteAttuale = player.Difesa_Cancello,
                        SaluteMax = player.Difesa_CancelloMax,
                        Riparazione = Strutture.Riparazione.Cancello,
                        SetSalute = new Action<int>(val => player.Difesa_Cancello = val),
                        Tipo = "Difesa"
                    },
                    new {
                        Index = 2,
                        SaluteAttuale = player.Salute_Mura,
                        SaluteMax = player.Salute_MuraMax,
                        Riparazione = Strutture.Riparazione.Mura,
                        SetSalute = new Action<int>(val => player.Salute_Mura = val),
                        Tipo = "Salute"
                    },
                    new {
                        Index = 3,
                        SaluteAttuale = player.Difesa_Mura,
                        SaluteMax = player.Difesa_MuraMax,
                        Riparazione = Strutture.Riparazione.Mura,
                        SetSalute = new Action<int>(val => player.Difesa_Mura = val),
                        Tipo = "Difesa"
                    },
                    new {
                        Index = 4,
                        SaluteAttuale = player.Salute_Torri,
                        SaluteMax = player.Salute_TorriMax,
                        Riparazione = Strutture.Riparazione.Torri,
                        SetSalute = new Action<int>(val => player.Salute_Torri = val),
                        Tipo = "Salute"
                    },
                    new {
                        Index = 5,
                        SaluteAttuale = player.Difesa_Torri,
                        SaluteMax = player.Difesa_TorriMax,
                        Riparazione = Strutture.Riparazione.Torri,
                        SetSalute = new Action<int>(val => player.Difesa_Torri = val),
                        Tipo = "Difesa"
                    },
                    new {
                        Index = 6,
                        SaluteAttuale = player.Salute_Castello,
                        SaluteMax = player.Salute_CastelloMax,
                        Riparazione = Strutture.Riparazione.Castello,
                        SetSalute = new Action<int>(val => player.Salute_Castello = val),
                        Tipo = "Salute"
                    },
                    new {
                        Index = 7,
                        SaluteAttuale = player.Difesa_Castello,
                        SaluteMax = player.Difesa_CastelloMax,
                        Riparazione = Strutture.Riparazione.Castello,
                        SetSalute = new Action<int>(val => player.Difesa_Castello = val),
                        Tipo = "Difesa"
                    }
                };

                foreach (var struttura in strutture)
                {
                    if (!player.Riparazioni[struttura.Index] || struttura.SaluteAttuale >= struttura.SaluteMax)
                    {
                        player.Riparazioni[struttura.Index] = false;
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

                        struttura.SetSalute(struttura.SaluteAttuale + incremento);
                    }
                }
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
