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
            Gioco.Barbari.GeneraVillaggiPerGiocatore(player1);
        }
        // ----------------------- Client Connessione --------------------------
        static void ClientConnected(object? sender, ConnectionEventArgs args)
        {
            lastGuid = args.Client.Guid;
            string lasIpPort = args.Client.IpPort;
            Console.WriteLine("[SERVER|LOG] > Client connesso: " + args.Client.ToString());
            if (!Client_Connessi.Contains(lastGuid))
                Client_Connessi.Add(lastGuid);
        }
        static void ClientDisconnected(object? sender, DisconnectionEventArgs args)
        {
            lastGuid = args.Client.Guid;
            Console.WriteLine("[SERVER|LOG] > Client disconnesso: " + args.Client.ToString() + ": " + args.Reason.ToString());
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
                    Console.WriteLine($"Giocatore: {item.Value.Username} Guid: {item.Value.guid_Player}, Livello: {item.Value.Livello}");
            }
            public void AggiornaListaPVP()
            {
                var utentiDaAggiungere = new List<string>(); // Crea una lista temporanea di utenti da aggiungere
                foreach (var item in players)
                {
                    bool utentePresente = false;
                    if (Utenti_PVP.Count == 0 && item.Value.ScudoDellaPace == 0) // Se la lista è vuota, aggiungi direttamente l'utente
                    {
                        utentiDaAggiungere.Add($"{item.Value.Username}, Livello: {item.Value.Livello}");
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
                    if (!utentePresente && item.Value.ScudoDellaPace > 0)  // Se l'utente non è presente, aggiungilo alla lista temporanea
                        utentiDaAggiungere.Add($"{item.Value.Username}, Livello: {item.Value.Livello}");
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
                if (Client_Connessi.Count == 0)
                    foreach (var item in players)
                        item.Value.guid_Player = Guid.Empty;

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

                await GameSave.Load_Player_Data_Auto();
                servers_.AggiornaListaPVP();
                await GameSave.LoadServerData();
                await Gioco.Barbari.Inizializza();

                while (!cancellationToken.IsCancellationRequested)
                {
                    saveCounter++;
                    foreach (var player in players.Values)
                    {
                        BuildingManager.CompleteBuilds(player.guid_Player, player);
                        UnitManager.CompleteRecruitment(player.guid_Player, player);
                        ResearchManager.CompleteResearch(player.guid_Player, player);
                        await Esperienza.LevelUp(player);
                        Citta(player);

                        player.SetupVillaggioGiocatore(); // Aggiusta il valore Max per rimanere coerente con i client...
                        player.ProduceResources();
                        player.ManutenzioneEsercito();
                        await player.ServerTimer();

                        if (saveCounter >= 120) await GameSave.SavePlayer(player); // Salva i dati ogni 60 secondi

                        //Stats
                        if (player.currentTasks_Building.Count > 0)  player.Tempo_Costruzione++;
                        if (player.currentTasks_Recruit.Count > 0) player.Tempo_Addestramento++;
                        if (player.currentTasks_Research.Count > 0) player.Tempo_Ricerca++;
                    }
                    if (saveCounter >= 120)
                    {
                        saveCounter = 0;
                        await GameSave.SaveServerData();
                        servers_.AggiornaListaPVP();
                    }

                    AttacchiCooperativi.AggiornaAttacchi();
                    await Auto_Update_Clients();

                    if (Variabili_Server.timer_Reset_Quest > 0) Variabili_Server.timer_Reset_Quest--;
                    if (Variabili_Server.timer_Reset_Quest == 0) QuestManager.RigeneraQuest();
                    if (Variabili_Server.timer_Reset_Barbari > 0) Variabili_Server.timer_Reset_Barbari--;
                    if (Variabili_Server.timer_Reset_Barbari == 0) Barbari.RigeneraBarbari();

                    await Task.Delay(1000); // Ciclo ogni secondo, o regola il ritardo come necessario
                }
            }
            public void Citta(Player player)
            {
                player.Guarnigione_Ingresso = player.Guerrieri_Ingresso.Sum();
                player.Guarnigione_Ingresso += player.Lanceri_Ingresso.Sum();
                player.Guarnigione_Ingresso += player.Arceri_Ingresso.Sum();
                player.Guarnigione_Ingresso += player.Catapulte_Ingresso.Sum();

                player.Guarnigione_IngressoMax = (player.Ricerca_Ingresso_Guarnigione + 1) * player.Guarnigione_IngressoMax;

                player.Guarnigione_Citta = player.Guerrieri_Citta.Sum();
                player.Guarnigione_Citta += player.Lanceri_Citta.Sum();
                player.Guarnigione_Citta += player.Arceri_Citta.Sum();
                player.Guarnigione_Citta += player.Catapulte_Citta.Sum();

                player.Guarnigione_CittaMax = (player.Ricerca_Citta_Guarnigione + 1) * player.Guarnigione_CittaMax;

                player.Guarnigione_Cancello = player.Guerrieri_Cancello.Sum();
                player.Guarnigione_Cancello += player.Lanceri_Cancello.Sum();
                player.Guarnigione_Cancello += player.Arceri_Cancello.Sum();
                player.Guarnigione_Cancello += player.Catapulte_Cancello.Sum();

                player.Guarnigione_CancelloMax = (player.Ricerca_Cancello_Guarnigione + 1) * player.Guarnigione_CancelloMax;
                player.Salute_CancelloMax = (player.Ricerca_Cancello_Salute + 1) * player.Salute_CancelloMax;
                player.Difesa_CancelloMax = (player.Ricerca_Cancello_Difesa + 1) * player.Difesa_CancelloMax;

                player.Guarnigione_Mura = player.Guerrieri_Mura.Sum();
                player.Guarnigione_Mura += player.Lanceri_Mura.Sum();
                player.Guarnigione_Mura += player.Arceri_Mura.Sum();
                player.Guarnigione_Mura += player.Catapulte_Mura.Sum();

                player.Guarnigione_MuraMax = (player.Ricerca_Mura_Guarnigione + 1) * player.Guarnigione_MuraMax;
                player.Salute_MuraMax = (player.Ricerca_Mura_Salute + 1) * player.Salute_MuraMax;
                player.Difesa_MuraMax = (player.Ricerca_Mura_Difesa + 1) * player.Difesa_MuraMax;

                player.Guarnigione_Torri = player.Guerrieri_Torri.Sum();
                player.Guarnigione_Torri += player.Lanceri_Torri.Sum();
                player.Guarnigione_Torri += player.Arceri_Torri.Sum();
                player.Guarnigione_Torri += player.Catapulte_Torri.Sum();

                player.Guarnigione_TorriMax = (player.Ricerca_Torri_Guarnigione + 1) * player.Guarnigione_TorriMax;
                player.Salute_TorriMax = (player.Ricerca_Torri_Salute + 1) * player.Salute_TorriMax;
                player.Difesa_TorriMax = (player.Ricerca_Torri_Difesa + 1) * player.Difesa_TorriMax;

                player.Guarnigione_Castello = player.Guerrieri_Castello.Sum();
                player.Guarnigione_Castello += player.Lanceri_Cancello.Sum();
                player.Guarnigione_Castello += player.Arceri_Cancello.Sum();
                player.Guarnigione_Castello += player.Catapulte_Cancello.Sum();

                player.Guarnigione_CastelloMax = (player.Ricerca_Castello_Guarnigione + 1) * player.Guarnigione_CastelloMax;
                player.Salute_CastelloMax = (player.Ricerca_Castello_Salute + 1) * player.Salute_CastelloMax;
                player.Difesa_CastelloMax = (player.Ricerca_Castello_Difesa + 1) * player.Difesa_CastelloMax;
            }
            public IEnumerable<Player> GetAllPlayers()
            {
                return players.Values;
            }
        }
    }
}
