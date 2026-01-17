using Server_Strategico.Gioco;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using WatsonTcp;
using static Server_Strategico.Gioco.Giocatori;
using static System.Net.Mime.MediaTypeNames;

namespace Server_Strategico.Server
{
    internal class Server
    {
        public static List<Guid> Client_Connessi = new List<Guid>();
        public static List<string> Utenti_PVP = new List<string>();
        public static List<string> Utenti_Online = new List<string>();

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
            if (subjectName == "DESKTOP-DOBLVTI") serverIp = "0.0.0.0";

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
                server.SendAsync(guid, msg);
        }

        public static async Task NewPlayer(string player, string password)
        {
            var player1 = servers_.GetPlayer(player, password);
            player1.Cibo = 400000;
            player1.Legno = 400000;
            player1.Pietra = 400000;
            player1.Ferro = 400000;
            player1.Oro = 400000;
            player1.Popolazione = 20000;

            player1.Spade = 2000;
            player1.Lance = 2000;
            player1.Archi = 2000;
            player1.Scudi = 2000;
            player1.Armature = 2000;
            player1.Frecce = 200;

            player1.Diamanti_Blu = 1500;
            player1.Diamanti_Viola = 1500000;

            //Test battaglie
            player1.Guerrieri = [25, 0, 0, 0, 0];
            player1.Lanceri = [25, 0, 0, 0, 0];
            player1.Arceri = [25, 0, 0, 0, 0];
            player1.Catapulte = [25, 0, 0, 0, 0];

            //player1.Guerrieri_Ingresso = [25, 0, 0, 0, 0];
            //player1.Lanceri_Ingresso  = [25, 0, 0, 0, 0];
            //player1.Arceri_Ingresso = [25, 0, 0, 0, 0];
            //player1.Catapulte_Ingresso = [25, 0, 0, 0, 0];

            player1.Guerrieri_Cancello = [5, 0, 0, 0, 0];
            player1.Lanceri_Cancello = [5, 0, 0, 0, 0];
            player1.Arceri_Cancello = [5, 0, 0, 0, 0];
            player1.Catapulte_Cancello = [5, 0, 0, 0, 0];

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
        }
        static void ClientDisconnected(object? sender, DisconnectionEventArgs args)
        {
            lastGuid = args.Client.Guid;
            Console.WriteLine("[SERVER|LOG] > Client disconnesso: " + args.Client.ToString() + ": " + args.Reason.ToString());

            // AGGIUNTA FONDAMENTALE: Rimuovi dalla mappa thread-safe
            Client_Connessi_Map.TryRemove(lastGuid, out _);

            Client_Connessi.Remove(lastGuid);
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
                    Console.WriteLine($"Giocatore: {item.Value.Username} Guid: {item.Value.guid_Player}, Livello: {item.Value.Livello}");
            }
            public void AggiornaListaPVP()
            {
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
                    if (player.ScudoDellaPace != 0) continue;
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
                double ramMb = proc.WorkingSet64 / 1024.0 / 1024.0; // RAM in MB
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
                Console.WriteLine($"[Server Resources] X player - RAM: {ramMb/players.Count()*1024:F2} KB");
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
                int saveCounter = 0, saveCounterPlayer = 0, riparazioni = 0, _firstStart = 0;
                double totale_Stats = 0, media_Stats = 0, min_Stats = 0, max_Stats = 0, numero_Stats = 0;

                // --- BLOCCO RIPRISTINATO: GENERAZIONE GIOCATORI ---
                await addBOT(50000);

                // --- BLOCCO RIPRISTINATO: INIZIALIZZAZIONE ---
                await GameSave.LoadServerData();
                await GameSave.Load_Player_Data_Auto();
                servers_.AggiornaListaPVP();
                await Gioco.Barbari.Inizializza();
                //CompleteTask(cancellationToken); //Avvolte genera un'eccezione... la collezione è stata modificata.
                ScheduleManager.AvvioReset();
                // ----------------------------------------------

                while (!cancellationToken.IsCancellationRequested)
                {
                    int maxConcurrentTasks = Math.Max(1, Environment.ProcessorCount); //Core disponibili
                    int totalPlayers = players.Values.Count;
                    var queue = new ConcurrentQueue<Player>(players.Values);  // Pool condiviso
                    var workers = new Task[maxConcurrentTasks]; // N task = N core

                    Stopwatch taskStopwatch = Stopwatch.StartNew();
                    saveCounter++;
                    riparazioni++;

                    for (int i = 0; i < maxConcurrentTasks; i++)
                    {
                        workers[i] = Task.Run(() =>
                        {
                            while (queue.TryDequeue(out var player))
                            {
                                // === IDENTICA LOGICA PLAYER ===
                                if (player.building_Queue.Count != 0) BuildingManager.CompleteBuilds(player.guid_Player, player);
                                if (player.recruit_Queue.Count != 0)UnitManager.CompleteRecruitment(player.guid_Player, player);
                                if (player.currentTasks_Research.Count != 0) ResearchManager.CompleteResearch(player.guid_Player, player);

                                //player.Esperienza += 235;

                                if (_firstStart == 0)
                                {
                                    player.SetupVillaggioGiocatore(); //Richiamare solo quando effettivamente c'è bisogno +- 145 ms in più su 50000 player
                                    player.SetupCaserme();
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

                                GuerrieriCitta(player);
                                player.ProduceResources();
                                player.ManutenzioneEsercito();
                                player.ServerTimer();
                                player.ResetGiornaliero();

                                if (player.Vip || player.GamePass_Base || player.GamePass_Avanzato) player.BonusPacchetti();
                                if (player.currentTasks_Building.Count > 0) player.Tempo_Costruzione++;
                                if (player.currentTasks_Recruit.Count > 0) player.Tempo_Addestramento++;
                                if (player.currentTasks_Research.Count > 0) player.Tempo_Ricerca++;
                            }
                        });
                    }
                    await Task.WhenAll(workers); // Attendiamo il completamento di tutti i Task... qui avvienecl'esecuzione del codice sopra

                    TimeSpan tempoImpiegato_1 = taskStopwatch.Elapsed;
                    Console.WriteLine($"[PERF] 1 - Server elaborato in:    [{tempoImpiegato_1.TotalMilliseconds:F4} ms]");

                    if (saveCounter >= 300)
                    {
                        saveCounter = 0;
                        GameSave.SaveServerData();

                    }
                    if (saveCounterPlayer >= 20)
                    {
                        saveCounterPlayer = 0;
                        SaveSomePlayersAsync(100); //Salva 50 player per volta...
                    }

                    TimeSpan tempoImpiegato_3 = taskStopwatch.Elapsed;
                    Console.WriteLine($"[PERF] 3 - Server elaborato in:    [{tempoImpiegato_3.TotalMilliseconds:F4} ms]");

                    if (riparazioni >= Variabili_Server.tempo_Riparazione)
                    {
                        riparazioni = 0;
                        AttacchiCooperativi.AggiornaAttacchi();
                        servers_.AggiornaListaPVP();
                    }

                    TimeSpan tempoImpiegato_4 = taskStopwatch.Elapsed;
                    Console.WriteLine($"[PERF] 4 - Server elaborato in:    [{tempoImpiegato_4.TotalMilliseconds:F4} ms]");

                    Auto_Update_Clients();
                    TimeSpan tempoImpiegato_5 = taskStopwatch.Elapsed;
                    Console.WriteLine($"[PERF] 5 - Server elaborato in:    [{tempoImpiegato_5.TotalMilliseconds:F4} ms]");

                    if (Variabili_Server.timer_Reset_Quest > 0) Variabili_Server.timer_Reset_Quest--;
                    if (Variabili_Server.timer_Reset_Quest == 0) QuestManager.RigeneraQuest();
                    if (Variabili_Server.timer_Reset_Barbari > 0) Variabili_Server.timer_Reset_Barbari--;
                    if (Variabili_Server.timer_Reset_Barbari == 0) Barbari.RigeneraBarbari();
                    taskStopwatch.Stop();

                    // Log del tempo totale
                    TimeSpan tempoImpiegato_2 = taskStopwatch.Elapsed;
                    if (numero_Stats < 3) numero_Stats += 1;
                    else
                    {
                        numero_Stats += 1;
                        totale_Stats += tempoImpiegato_2.TotalMilliseconds;
                        media_Stats = totale_Stats / numero_Stats;

                        if (tempoImpiegato_2.TotalMilliseconds > max_Stats) max_Stats = tempoImpiegato_2.TotalMilliseconds;
                        if (tempoImpiegato_2.TotalMilliseconds < min_Stats || min_Stats == 0) min_Stats = tempoImpiegato_2.TotalMilliseconds;

                        Console.WriteLine("Core: " + maxConcurrentTasks + " Giocatori: " + players.Count());
                        Console.WriteLine($"[PERF] A - Server elaborato in:    [{tempoImpiegato_2.TotalMilliseconds:F4} ms]");
                        Console.WriteLine($"[PERF] B - Min:                    [{min_Stats:F4} ms]");
                        Console.WriteLine($"[PERF] C - Med:                    [{media_Stats:F4} ms]");
                        Console.WriteLine($"[PERF] E - Max:                    [{max_Stats:F4} ms]");
                        Console.WriteLine($"[PERF] D - X player:               [{(media_Stats / players.Count()):F5} ms]\n");
                    }

                    // Regolazione dinamica del ritardo (per tornare a 1000ms totali)
                    double tempoRimanente = 1000.0 - tempoImpiegato_2.TotalMilliseconds;
                    if (tempoRimanente <= 0) tempoRimanente = 50;
                    if (tempoRimanente > 0) await Task.Delay((int)tempoRimanente);
                }
            }
            public async Task SaveSomePlayersAsync(int count)
            {
                if (players.Count == 0) return;
                var list = players.Values.ToList();

                for (int i = 0; i < count; i++)
                {
                    var player = list[_saveIndex];
                    _saveIndex++;
                    if (_saveIndex >= list.Count) _saveIndex = 0;

                    await GameSave.SavePlayer(player);
                }
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
            public async Task CompleteTask(CancellationToken cancellationToken)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    foreach (var player in players.Values)
                    {
                        BuildingManager.CompleteBuilds(player.guid_Player, player);
                        UnitManager.CompleteRecruitment(player.guid_Player, player);
                        ResearchManager.CompleteResearch(player.guid_Player, player);
                    }
                    await Task.Delay(100); // Ciclo ogni secondo, o regola il ritardo come necessario
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
            public void GuerrieriCitta(Player player)
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
