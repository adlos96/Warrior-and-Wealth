using Server_Strategico.Gioco;
using WatsonTcp;
using static Server_Strategico.Gioco.Giocatori;

namespace Server_Strategico.Server
{
    internal class Server
    {
        public static List<Guid> Client_Connessi = new List<Guid>();
        public static List<string> Utenti_PVP = new List<string>();
        public static List<string> Utenti_Online = new List<string>();

        private string? serverIp = null; // "null" will open the tcp server on addr 0.0.0.0 on windows (127.0.0.1 on linux)
        private const int serverPort = 8443;
        private static Guid lastGuid = Guid.Empty;
        public static WatsonTcpServer server = null;

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
            StartGame();

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
            //servers_.AddPlayer("Player1", "Password1");
            //servers_.AddPlayer("Player2", "Password2");
            //var player1 = servers_.GetPlayer("Player1", "Password1");
            //var player2 = servers_.GetPlayer("Player2", "Password2");
            //
            //player1.QueueBuildConstruction("Fattoria", 1); // Costruisci 10 fattorie
            //player1.QueueTrainUnits("Arciere", 1); // Avvia l'addestramento di 5 arcieri
            //
            //player2.QueueBuildConstruction("Fattoria", 1); // Costruisci 10 fattorie,
            //player2.QueueTrainUnits("Arciere", 1); // Avvia l'addestramento di 5 arcieri

            // Simula il passare del tempo sul server



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
            else
                Console.WriteLine("Il gioco non è attualmente in esecuzione.");
            
        }
        public static void Send(Guid guid, string msg)
        {

            if (guid != Guid.Empty)
                server.SendAsync(guid, msg);            
        }

        public static async Task NewPlayer(string player, string password)
        {
            var player1 = servers_.GetPlayer(player, password);
            player1.Cibo = 40000;
            player1.Legno = 40000;
            player1.Pietra = 40000;
            player1.Ferro = 40000;
            player1.Oro = 40000;
            player1.Popolazione = 200;

            player1.Spade = 200;
            player1.Lance = 200;
            player1.Archi = 200;
            player1.Scudi = 200;
            player1.Armature = 200;
            player1.Frecce = 200;

            player1.Diamanti_Blu = 1500;
            player1.Diamanti_Viola = 1500000;

        }
        // ----------------------- Client Connessione --------------------------
        static void ClientConnected(object? sender, ConnectionEventArgs args)
        {
            lastGuid = args.Client.Guid;
            string lasIpPort = args.Client.IpPort;
            Console.WriteLine("[SERVER|LOG] > Client connesso: " + args.Client.ToString());
            if (!Client_Connessi.Contains(lastGuid))
                Client_Connessi.Add(lastGuid);

            var ciao = lasIpPort.Split(":");
        }
        static void ClientDisconnected(object? sender, DisconnectionEventArgs args)
        {
            lastGuid = args.Client.Guid;
            Console.WriteLine("[SERVER|LOG] > Client disconnesso: " + args.Client.ToString() + ": " + args.Reason.ToString());
            
            // Trova il giocatore associato a questo GUID e salva i suoi dati
            foreach (var player in servers_.GetAllPlayers())
            {
                GameSave.SavePlayer(player);
            }
            
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
            public async Task<bool> AddFakePlayer(string username, string password)
            {
                if (!players.ContainsKey(username))
                {
                    players.Add(username, new Player(username, password, Guid.Empty));
                    await NewPlayer(username, password);
                    return true;
                }
                else
                {
                    return false;
                    throw new ArgumentException($"Fake Player già presente con questo username {username} e password {password}.");
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
                    Console.WriteLine($"Giocatore: {item.Value.Username} Guid: {item.Value.guid_Player}, Livello: {item.Value.Livello}, Esperienza: {item.Value.Esperienza}");
            }
            public void Lista_Player_Auto()
            {
                // Crea una lista temporanea di utenti da aggiungere
                var utentiDaAggiungere = new List<string>();

                foreach (var item in players)
                {
                    bool utentePresente = false;
                    if (Utenti_PVP.Count == 0 && item.Value.Vip) // Se la lista è vuota, aggiungi direttamente l'utente
                    {
                        utentiDaAggiungere.Add($"{item.Value.Username}, Livello: {item.Value.Livello}, Esperienza: {item.Value.Esperienza}");
                        continue;
                    }
                    foreach (var utentePVP in Utenti_PVP) // Controlla se l'utente è già presente nella lista
                    {
                        var parti = utentePVP.Split(",");
                        if (parti[0].Trim() == item.Value.Username)
                        {
                            utentePresente = true;
                            break;
                        }
                    }
                    if (!utentePresente && item.Value.Vip)  // Se l'utente non è presente, aggiungilo alla lista temporanea
                        utentiDaAggiungere.Add($"{item.Value.Username}, Livello: {item.Value.Livello}, Esperienza: {item.Value.Esperienza}");
                }
                foreach (var utente in utentiDaAggiungere) // Dopo aver terminato l'enumerazione, aggiungi tutti gli utenti dalla lista temporanea
                    Utenti_PVP.Add(utente);
            }
            public async Task<bool> Check_Username_Player(string username)
            {
                foreach (var item in players)
                    if (item.Value.Username == username)
                        return false;
                return true;
            }
            public async Task<bool> Auto_Update_Clients()
            {
                foreach (var client in Client_Connessi)
                    foreach (var item in players)
                    {
                        if (item.Value.guid_Player == client)
                            ServerConnection.Update_Data(item.Value.guid_Player, item.Value.Username, item.Value.Password);
                    }
                return false;
            }
            public async Task RunGameLoopAsync(CancellationToken cancellationToken)
            {
                int saveCounter = 0;  // Contatore per il salvataggio

                await GameSave.LoadBarbariPVP();
                await GameSave.Load_Player_Data_Auto();
                QuestManager.AvviaTimerReset();
                servers_.Lista_Player_Auto();

                Gioco.Barbari.Inizializza();   // <--- aggiungi qui
                while (!cancellationToken.IsCancellationRequested)
                {
                    saveCounter++;
                    foreach (var player in players.Values)
                    {
                        BuildingManager.CompleteBuilds(player.guid_Player, player);
                        UnitManager.CompleteRecruitment(player.guid_Player, player);
                        ResearchManager.CompleteResearch(player.guid_Player, player);

                        player.SetupVillaggioGiocatore(); // Aggiusta il valore Max per rimanere coerente con i client...
                        player.ProduceResources();
                        player.ManutenzioneEsercito();
                        player.VIP();

                        if (saveCounter >= 120) await GameSave.SavePlayer(player); // Salva i dati ogni 60 secondi

                        await Auto_Update_Clients();
                        await Esperienza.LevelUp(player);

                        //Stats
                        if (player.currentTasks_Building.Count > 0)  player.Tempo_Addestramento++;
                        if (player.currentTasks_Recruit.Count > 0) player.Tempo_Addestramento++;
                        if (player.currentTasks_Research.Count > 0) player.Tempo_Ricerca++;
                    }
                    if (saveCounter >= 120)
                    {
                        saveCounter = 0;
                        await GameSave.SaveGameServer();
                        servers_.Lista_Player_Auto();
                    }
                    await Task.Delay(1000); // Ciclo ogni secondo, o regola il ritardo come necessario
                    AttacchiCooperativi.AggiornaAttacchi();
                }
            }
            public IEnumerable<Player> GetAllPlayers()
            {
                return players.Values;
            }
        }
    }
}
