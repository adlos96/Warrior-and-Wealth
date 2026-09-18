using Newtonsoft.Json;
using Server_Strategico.Gioco;
using Server_Strategico.Manager;
using Server_Strategico.ServerData.Moduli;
using Strategico_V2.Manager;
using System.Text;
using WatsonTcp;
using static Server_Strategico.Gioco.Barbari;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Strutture;
using static Server_Strategico.Manager.QuestManager;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Server_Strategico.Server
{
    public class ServerConnection
    {
        // Punto di ingresso storico per i client WatsonTcp: estrae dal
        // MessageReceivedEventArgs (tipo specifico di WatsonTcp) i tre dati
        // che contano davvero — guid, stringa del messaggio, descrizione
        // del client — e li passa al core condiviso qui sotto. In questo
        // modo qualsiasi altro trasporto (es. il gateway WebSocket) può
        // richiamare la stessa logica di gioco senza dipendere da WatsonTcp.
        public static void HandleClientRequest(MessageReceivedEventArgs requestData)
        {
            var clientDescription = requestData.Client.ToString();
            var clientGuid = requestData.Client.Guid;
            var messaggioRicevuto = Encoding.UTF8.GetString(requestData.Data);
            HandleClientMessage(clientGuid, messaggioRicevuto, clientDescription);
        }

        // Core condiviso, agnostico rispetto al trasporto (WatsonTcp o
        // WebSocket): stesso protocollo testuale "comando|arg1|arg2|...",
        // stessa logica di gioco. Nessuna modifica qui sotto rispetto al
        // comportamento originale.
        public static async void HandleClientMessage(Guid clientGuid, string messaggioRicevuto, string client)
        {
            //Variabili.server_Log.Add()
           Console.WriteLine("             ** Comunicazione Client **  ");
           Console.WriteLine("-----------------------------", "standard");
           Console.WriteLine($"[ServerConnection|ClientRequest] > Client:      [{client}]");
           Console.WriteLine($"[ServerConnection|ClientRequest] > Guid:        [{clientGuid}]");
           Console.WriteLine($"[ServerConnection|ClientRequest] > Messaggio:   [{messaggioRicevuto}]");

            if (!messaggioRicevuto.Contains("|"))
            {
                Console.WriteLine($"[Errore|ServerConnection] >> Messaggio ricevuto: {messaggioRicevuto}");
                return;
            }

            var msgArgsRicevuti = messaggioRicevuto.Split('|');
            string comando = msgArgsRicevuti[0];

            string user = "", password = "", email = "", lang = "";
            string[] msgArgs;
            Player player = null;

            // "AutoLogin" deve passare senza il controllo generico qui sotto
            // per lo stesso motivo di Login/New Player: il suo intero scopo è
            // autenticare (o ri-autenticare) il client, quindi non può essere
            // già bloccato da un pre-controllo che si aspetta un access token
            // GIA' valido. Prima di questa correzione, un access token scaduto
            // (caso normalissimo: il client riapre l'app dopo ore) veniva
            // intercettato qui con TOKEN_SCADUTO/TOKEN_NON_VALIDO e la logica
            // di fallback sul refresh token nel case "AutoLogin" più sotto non
            // veniva mai raggiunta.
            if (comando == "Login" || comando == "New Player" || comando == "AutoLogin")
            {
                msgArgs = msgArgsRicevuti;
                if (comando != "AutoLogin")
                {
                    user = msgArgsRicevuti[2];
                    password = msgArgsRicevuti[3];
                    lang = msgArgsRicevuti[4];
                    email = msgArgsRicevuti[5];
                }
            }
            else
            {
                string accessToken = msgArgsRicevuti[1];

                if (!TokenManager.ValidateAccessToken(accessToken, out string username, out bool isExpired))
                {
                    if (isExpired)
                    {
                        Server.Send(clientGuid, "TOKEN_SCADUTO"); //Invio per triggherare il refresh del token da parte del client
                        Console.WriteLine("[ServerConnection] >> TOKEN_SCADUTO");
                    }
                    else
                    {
                        Server.Send(clientGuid, "TOKEN_NON_VALIDO"); //Non serve a nulla... il client lo vede ma non c'è il codice
                        Console.WriteLine("[ServerConnection] >> TOKEN_NON_VALIDO");
                    }
                    return;
                }

                player = Server.servers_.GetPlayer(username);
                if (player == null)
                {
                    Server.Send(clientGuid, "TOKEN_NON_VALIDO_PLAYER_NON_TROVATO"); //Non serve a nulla... il client lo vede ma non c'è il codice
                    Console.WriteLine("[ServerConnection] >> TOKEN_NON_VALIDO_PLAYER_NON_TROVATO");
                    return;
                }

                // [0]=comando, [1]=username, [2]=vuoto (era la password), [3+]=dati
                msgArgs = new[] { comando, username, string.Empty }
                    .Concat(msgArgsRicevuti.Skip(2))
                    .ToArray();
            }
            if (msgArgs.Length == 0)
            {
                Console.WriteLine("[Errore|ServerConnection] >> needed 1 args");
                return;
            }
            var dati = Server.servers_.players;
            if (player == null) player = Server.servers_.GetPlayer(user, password);
            
            switch (msgArgs[0])
            {
                case "Refresh_Access_Token":
                    Console.WriteLine($"[Server] Richiesta nuovo Access Token ID: {player.Username}, Guid: {clientGuid}");
                    if (player == null)
                    {
                        Console.WriteLine("[Refresh_Access_Token] Player risulta null");
                        return;
                    }
                    else
                    {
                        TokenManager.RevokeRefreshToken(msgArgs[1]);
                        string accessToken = TokenManager.GenerateAccessToken(email, user, TimeSpan.FromHours(8));
                        Server.Send(clientGuid, $"Update_AccessToken|{accessToken}");
                    }
                    break;
                case "New Player":
                    Console.WriteLine($"[Server] Richiesta nuovo utente ID: {clientGuid}");
                    if (await New_Player(user, password, email, clientGuid))
                    {
                        player = Server.servers_.GetPlayer(user, password);
                        if (player == null)
                        {
                            Console.WriteLine("[NewPlayer] Player risulta null");
                            return;
                        }
                        // Pulisce eventuali refresh token residui di sessioni precedenti
                        TokenManager.RevokeAllRefreshTokensForUser(email);

                        // Genero i token qui, subito dopo l'auth con username/password
                        string accessToken = TokenManager.GenerateAccessToken(email, user, TimeSpan.FromHours(8));
                        string refreshToken = TokenManager.GenerateRefreshToken(email, user, TimeSpan.FromDays(10));
                        Server.Send(clientGuid, $"Login|true|{accessToken}|{refreshToken}");

                        Server.Client_Connessi_Map.TryRemove(clientGuid, out _);
                        Server.Client_Connessi_Map.TryAdd(clientGuid, user);

                        Lingua(player, lang);

                        //Avatar
                        player.Avatar_Sbloccati = new string[] { "Lord_3", "Lady_1", "", "", "", "", "", "", "", "" };

                        Descrizioni.DescUpdate(player);
                        Descrizioni.DescLabelUpdate(player);
                        Descrizioni.DescTestoUpdate(player);

                        QuestManager.QuestUpdate(player);
                        QuestManager.QuestRewardUpdate(player);
                        AggiornaVillaggiClient(player);
                        Tutorial(player);
                        player.SetupCaserme();
                        GamePass_Premi_Send(player);
                        Update_Data_OneTime(clientGuid, player);
                        await EmailManager.SendWelcomeAsync(player.Email, player.Username);
                    }
                    else
                        Server.Send(clientGuid, $"Login|false|Questo nome utente è già presente: [{user}");
                    break;
                case "Login":
                    if (await Login(user, password, email, clientGuid)) //Comando, Username, Password, Lingua
                    {
                        if (player == null)
                        {
                            Console.WriteLine("[Login] Player risulta null");
                            return;
                        }
                        // Pulisce eventuali refresh token residui di sessioni precedenti
                        TokenManager.RevokeAllRefreshTokensForUser(player.Email);

                        // Genero i token qui, subito dopo l'auth con username/password
                        string accessToken = TokenManager.GenerateAccessToken(player.Email, player.Username, TimeSpan.FromHours(8));
                        string refreshToken = TokenManager.GenerateRefreshToken(player.Email, player.Username, TimeSpan.FromDays(10));
                        Server.Send(clientGuid, $"Login|true|{accessToken}|{refreshToken}");

                        Server.Client_Connessi_Map.TryRemove(clientGuid, out _);
                        Server.Client_Connessi_Map.TryAdd(clientGuid, player.Username);

                        if (player.Stato_Giocatore == false) player.Stato_Giocatore = true; //Riattiva il giocatore se non entra da molto

                        Accesso_Giornaliero(player);//GamePass Gold
                        Lingua(player, lang);

                        Descrizioni.DescUpdate(player);
                        Descrizioni.DescLabelUpdate(player);
                        Descrizioni.DescTestoUpdate(player);

                        QuestManager.QuestUpdate(player);
                        QuestManager.QuestRewardUpdate(player);
                        AggiornaVillaggiClient(player);
                        Server.servers_.AggiornaListaPVP();
                        Tutorial(player);
                        player.SetupCaserme();
                        GamePass_Premi_Send(player);
                        Update_Data_OneTime(clientGuid, player);
                        if (player.Snapshot != null) player.Snapshot.Reset();
                    }
                    else
                        Server.Send(clientGuid, $"Login|false|Username o password non corrispondono. User: [{msgArgs[1]}] psw: [{msgArgs[2]}]");
                    break;
                case "AutoLogin":
                    // Formato: "AutoLogin|accessToken|refreshToken|lingua" (msgArgs
                    // qui è l'array grezzo, vedi il bypass del pre-controllo sopra:
                    // [0]=comando, [1]=accessToken, [2]=refreshToken, [3]=lingua).
                    string accessToken_A = msgArgs[1];
                    string refreshToken_A = msgArgs[2];
                    string lang_A = msgArgs.Length > 3 ? msgArgs[3] : "it";

                    string usernameAutoLogin = null;

                    if (TokenManager.ValidateAccessToken(accessToken_A, out string usernameDaAccessToken, out bool accessTokenScaduto))
                    {
                        // Access token ancora valido: nessun bisogno di toccare il refresh token.
                        usernameAutoLogin = usernameDaAccessToken;
                    }
                    else
                    {
                        // Access token scaduto o non valido: è il caso normale per cui
                        // esiste l'AutoLogin, quindi si prova col refresh token invece
                        // di arrendersi subito.
                        if (!TokenManager.ValidateRefreshToken(refreshToken_A, out string usernameDaRefreshToken, out bool refreshTokenScaduto))
                        {
                            Server.Send(clientGuid, refreshTokenScaduto ? "TOKEN_SCADUTO" : "TOKEN_NON_VALIDO");
                            Console.WriteLine($"[ServerConnection] >> AutoLogin - Refresh Token {(refreshTokenScaduto ? "scaduto" : "non valido")}");
                            return;
                        }
                        usernameAutoLogin = usernameDaRefreshToken;
                    }

                    player = Server.servers_.GetPlayer(usernameAutoLogin);
                    if (player != null) Console.WriteLine($"[ServerConnection|ClientRequest] > IP:          [{client} || Caricato: {player.Username}]\n");
                    if (player == null)
                    {
                        Console.WriteLine("[AutoLogin] Player risulta null");
                        Server.Send(clientGuid, "TOKEN_NON_VALIDO_PLAYER_NON_TROVATO");
                        return;
                    }

                    // FONDAMENTALE (mancava): senza aggiornare guid_Player, il game
                    // loop (GameServer.Auto_Update_Clients, che manda gli Update_Data
                    // ad ogni tick) continua a usare il guid della sessione precedente
                    // — quindi il client che ha appena fatto AutoLogin non riceve mai
                    // aggiornamenti, pur senza errori lato server. Login/New Player lo
                    // fanno già (vedi ServerConnection.Login/New_Player).
                    player.guid_Player = clientGuid;

                    // Rotazione token (come su Login/New Player): il refresh token
                    // usato viene revocato e se ne genera uno nuovo insieme al nuovo
                    // access token, invece di continuare a riusare lo stesso all'infinito.
                    TokenManager.RevokeRefreshToken(refreshToken_A);
                    accessToken_A = TokenManager.GenerateAccessToken(player.Email, player.Username, TimeSpan.FromHours(8));
                    refreshToken_A = TokenManager.GenerateRefreshToken(player.Email, player.Username, TimeSpan.FromDays(10));
                    Server.Send(clientGuid, $"Login|true|{accessToken_A}|{refreshToken_A}");

                    Server.Client_Connessi_Map.TryRemove(clientGuid, out _);
                    Server.Client_Connessi_Map.TryAdd(clientGuid, player.Username);

                    if (player.Stato_Giocatore == false) player.Stato_Giocatore = true; //Riattiva il giocatore se non entra da molto
                    if (player.Last_Login != DateTime.Now.Date) //Accesso giornaliero - Incremento e controllo accessi consecutivi per GamePass
                    {
                        var daysDiff = (DateTime.Now.Date - player.Last_Login.Date).Days;
                        if (daysDiff == 1 && player.GamePass_Avanzato)
                        {
                            player.GamePass_Accessi_Consecutivi += 1;
                            player.Last_Login = DateTime.Now.Date;
                            Console.WriteLine("Gamepass: Giorno incrementato");
                        }
                        else if (daysDiff > 1)
                        {
                            Console.WriteLine("Gamepass: Giorni resettati");
                            player.GamePass_Accessi_Consecutivi = 0;
                            player.Last_Login = DateTime.Now.Date;
                            player.GamePass_Premi = new bool[Variabili_Server.gamePass_DailyReward.Count()]; //Reset premi giornalieri
                        }
                        if (!player.GamePass_Avanzato && player.GamePass_Accessi_Consecutivi != 0)
                        {
                            player.GamePass_Accessi_Consecutivi = 0;
                            Console.WriteLine("Gamepass: Gamepass scaduto, reset giorni");
                        }
                    }
                    Accesso_Giornaliero(player);//GamePass Gold
                    Lingua(player, lang_A);

                    Descrizioni.DescUpdate(player);
                    Descrizioni.DescLabelUpdate(player);
                    Descrizioni.DescTestoUpdate(player);

                    QuestManager.QuestUpdate(player);
                    QuestManager.QuestRewardUpdate(player);
                    AggiornaVillaggiClient(player);
                    Server.servers_.AggiornaListaPVP();
                    Tutorial(player);
                    player.SetupCaserme();
                    GamePass_Premi_Send(player);
                    Update_Data_OneTime(clientGuid, player);
                    player.Snapshot.Reset();

                    break;
                case "Password change":
                    Console.WriteLine($"[Server] Richiesta cambio password per l'utente: {msgArgs[1]}");
                    Cambia_Password(clientGuid, player, msgArgs);
                    break;


                case "Costruzione":
                    if (Convert.ToInt32(msgArgs[3]) > 0) BuildingManagerV2.Costruzione("Fattoria", Convert.ToInt32(msgArgs[3]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[4]) > 0) BuildingManagerV2.Costruzione("Segheria", Convert.ToInt32(msgArgs[4]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[5]) > 0) BuildingManagerV2.Costruzione("CavaPietra", Convert.ToInt32(msgArgs[5]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[6]) > 0) BuildingManagerV2.Costruzione("MinieraFerro", Convert.ToInt32(msgArgs[6]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[7]) > 0) BuildingManagerV2.Costruzione("MinieraOro", Convert.ToInt32(msgArgs[7]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[8]) > 0) BuildingManagerV2.Costruzione("Abitazioni", Convert.ToInt32(msgArgs[8]), clientGuid, player); // Costruisci fattorie
                    
                    if (Convert.ToInt32(msgArgs[9]) > 0) BuildingManagerV2.Costruzione("ProduzioneSpade", Convert.ToInt32(msgArgs[9]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[10]) > 0) BuildingManagerV2.Costruzione("ProduzioneLance", Convert.ToInt32(msgArgs[10]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[11]) > 0) BuildingManagerV2.Costruzione("ProduzioneArchi", Convert.ToInt32(msgArgs[11]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[12]) > 0) BuildingManagerV2.Costruzione("ProduzioneScudi", Convert.ToInt32(msgArgs[12]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[13]) > 0) BuildingManagerV2.Costruzione("ProduzioneArmature", Convert.ToInt32(msgArgs[13]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[14]) > 0) BuildingManagerV2.Costruzione("ProduzioneFrecce", Convert.ToInt32(msgArgs[14]), clientGuid, player); // Costruisci fattorie

                    if (Convert.ToInt32(msgArgs[15]) > 0) BuildingManagerV2.Costruzione("CasermaGuerrieri", Convert.ToInt32(msgArgs[15]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[16]) > 0) BuildingManagerV2.Costruzione("CasermaLanceri", Convert.ToInt32(msgArgs[16]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[17]) > 0) BuildingManagerV2.Costruzione("CasermaArceri", Convert.ToInt32(msgArgs[17]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[18]) > 0) BuildingManagerV2.Costruzione("CasermaCatapulte", Convert.ToInt32(msgArgs[18]), clientGuid, player); // Costruisci fattorie
                    break;
                case "Reclutamento":
                    if (Convert.ToInt32(msgArgs[4]) > 0) UnitManagerV2.Reclutamento("Guerrieri", $"_{msgArgs[3]}", Convert.ToInt32(msgArgs[4]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[5]) > 0) UnitManagerV2.Reclutamento("Lanceri", $"_{msgArgs[3]}", Convert.ToInt32(msgArgs[5]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[6]) > 0) UnitManagerV2.Reclutamento("Arceri", $"_{msgArgs[3]}", Convert.ToInt32(msgArgs[6]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[7]) > 0) UnitManagerV2.Reclutamento("Catapulte", $"_{msgArgs[3]}", Convert.ToInt32(msgArgs[7]), clientGuid, player); // Costruisci fattorie
                    break;
                case "Costruzione_Terreni":
                    BuildingManagerV2.Terreni_Virtuali(clientGuid, player); // Costruisci fattorie
                    break;
                case "Esplora":
                    // 16/09/2026: nuovo protocollo, sostituisce gradualmente il vecchio Esplora(...).
                    // PVP: "Esplora|Token|PVP|Bersaglio"              -> msgArgs[3]=PVP, [4]=username
                    // PVE: "Esplora|Token|PVE|Bersaglio|Livello"      -> msgArgs[3]=PVE, [4]=globale, [5]=livello
                    //public static void EseguiSpionaggioRichiesta(Player attaccante, string modalità, string bersaglio, string livelloStr)
                    EseguiSpionaggioRichiesta(player, msgArgs[3], msgArgs[4], msgArgs.Length > 5 ? msgArgs[5] : null);
                    break;
                case "Battaglia":
                    Battaglia(player.guid_Player, player, msgArgs);
                    break;
                case "Ricerca":
                    ResearchManager.Ricerca(msgArgs[3], clientGuid, player);
                    break;
                case "AttaccoCooperativo":
                    await AttacchiCooperativi.GestisciComando(msgArgs, clientGuid, player);
                    break;
                case "Quest_Reward":
                    Quest_Reward(msgArgs, player, clientGuid);
                    break;
                case "Velocizza_Diamanti":
                    if (msgArgs[3] == "Costruzione")
                        BuildingManagerV2.UsaDiamantiPerVelocizzare(clientGuid, player, Convert.ToInt32(msgArgs[4]));
                    if (msgArgs[3] == "Reclutamento")
                        UnitManagerV2.UsaDiamantiPerVelocizzareReclutamento(clientGuid, player, Convert.ToInt32(msgArgs[4]));
                    if (msgArgs[3] == "Ricerca")
                        ResearchManager.UsaDiamantiPerVelocizzareRicerca(clientGuid, player, Convert.ToInt32(msgArgs[4]));
                    break;
                case "Scambia_Diamanti":
                      Scambia_Diamanti(clientGuid, player, msgArgs[3]); //Diamanti viola --> blu
                    break;
                case "Scambia_Tributi":
                    Scambia_Tributi(clientGuid, player, msgArgs[3]); //Diamanti viola --> blu
                    break;
                case "Shop":
                    Shop.Shop_Call(clientGuid, player, msgArgs[3]); //Shop
                    break;
                case "Elimina_Report":
                    // 16/09/2026, su richiesta dell'utente: il giocatore può eliminare un
                    // singolo referto dalla propria lista. Formato: "Elimina_Report|Token|Indice"
                    // -> msgArgs[3] = indice nell'array player.Report così come l'ha ricevuto
                    // l'ultima volta dal client (vedi Web/js/14-battaglia.js, data-report-index).
                    Elimina_Report(player, clientGuid, msgArgs[3]);
                    break;
                case "Elimina_Cronologia":
                    // 16/09/2026, su richiesta dell'utente: il giocatore può svuotare la
                    // propria cronologia messaggi. Formato: "Elimina_Cronologia|Token"
                    // (nessun indice: cancella tutta la cronologia salvata lato server).
                    Elimina_Cronologia(player, clientGuid);
                    break;
                case "SpostamentoTruppe":
                    SpostamentoTruppe(clientGuid, player, msgArgs); //sposta le troppe tra il "giocatore" e la città/cancello/ingresso
                    Server.GameServer.GuerrieriCitta(player);
                    break;
                case "Ripara":
                    Set_Riparazioni(player, msgArgs);
                    break;
                case "Ripara Tutto":
                    Set_Riparazioni(player, msgArgs);
                    break;
                case "Ripara Stop":
                    Stop_Riparazione(player);
                    break;
                case "Tutorial Update":
                    TutorialUpdate(player, msgArgs);
                    break;
                case "GamePass DailyReward":
                    GamePass_Premi(player);
                    break;
                case "Acquista Avatar":
                    Acquista_Avatar(player, msgArgs);
                    break;
                case "Seleziona Avatar":
                    Seleziona_Avatar(player, msgArgs);
                    break;


                default: Console.WriteLine($"Messaggio: [{msgArgs}]"); break;
            }
           
        }
        static int Avatar(Player player, string avatar)
        {
            int prezzo = -1;
            if (avatar == "Lord_1") prezzo = Variabili_Server.Avatar.Lord_1.Costo;
            else if (avatar == "Lord_2") prezzo = Variabili_Server.Avatar.Lord_2.Costo;
            else if (avatar == "Lord_3") prezzo = Variabili_Server.Avatar.Lord_3.Costo;
            else if (avatar == "Lord_4") prezzo = Variabili_Server.Avatar.Lord_4.Costo;
            else if (avatar == "Lord_5") prezzo = Variabili_Server.Avatar.Lord_5.Costo;
            else if (avatar == "Lord_6") prezzo = Variabili_Server.Avatar.Lord_6.Costo;
            else if (avatar == "Lord_7") prezzo = Variabili_Server.Avatar.Lord_7.Costo;
            else if (avatar == "Lady_1") prezzo = Variabili_Server.Avatar.Lady_1.Costo;
            else if (avatar == "Lady_2") prezzo = Variabili_Server.Avatar.Lady_2.Costo;
            else if (avatar == "Lady_3") prezzo = Variabili_Server.Avatar.Lady_3.Costo;
            else if (avatar == "Lady_4") prezzo = Variabili_Server.Avatar.Lady_4.Costo;
            else if (avatar == "Lady_5") prezzo = Variabili_Server.Avatar.Lady_5.Costo;
            else if (avatar == "Lady_6") prezzo = Variabili_Server.Avatar.Lady_6.Costo;
            else Server.Send(player.guid_Player, $"Server_Log|Avatar non trovato");
            return prezzo;
        }
        static void Acquista_Avatar(Player player, string[] msgArgs)
        {
            int prezzo = -1;
            string avatar = msgArgs[3];

            if (player.Avatar_Sbloccati.Contains(avatar))
            {
                Server.Send(player.guid_Player, $"Server_Log|Avatar già sbloccato");
                return;
            }
            prezzo = Avatar(player, avatar);
            if (prezzo == -1)
            {
                Server.Send(player.guid_Player, $"Server_Log|Avatar non trovato");
                return;
            }


            if (player.Diamanti_Viola >= prezzo)
            {
                player.Diamanti_Viola -= prezzo;
                player.Avatar_Sbloccati = player.Avatar_Sbloccati.Append(avatar).ToArray();
                Server.Send(player.guid_Player, $"Server_Log|Avatar acquistato con successo");
                Server.Send(player.guid_Player, $"Update_Data|avatar_Sbloccati={string.Join(",", player.Avatar_Sbloccati.Where(a => !string.IsNullOrEmpty(a)))}");
            }
            else
            {
                Server.Send(player.guid_Player, $"Server_Log|Diamanti viola insufficienti");
            }
        }
        async static void Seleziona_Avatar(Player player, string[] msgArgs)
        {
            string avatar = msgArgs[3];
            int prezzo = -1;

            prezzo = Avatar(player, avatar);

            if (prezzo == -1)
            {
                Server.Send(player.guid_Player, $"Log_Server|Avatar non trovato");
                return;
            }

            // Gratuito (prezzo 0) oppure già acquistato: selezionabile subito.
            if (prezzo > 0 && !player.Avatar_Sbloccati.Contains(avatar))
            {
                Server.Send(player.guid_Player, $"Log_Server|Avatar bloccato");
                return;
            }

            player.Avatar = avatar;
            Server.Send(player.guid_Player, $"Update_Data|avatar={player.Avatar}");
        }
        async static void Accesso_Giornaliero(Player player)
        {
            if (player.Last_Login != DateTime.Now.Date) //Accesso giornaliero - Incremento e controllo accessi consecutivi per GamePass
            {
                var daysDiff = (DateTime.Now.Date - player.Last_Login.Date).Days;
                if (daysDiff == 1 && player.GamePass_Avanzato)
                {
                    player.GamePass_Accessi_Consecutivi += 1;
                    player.Last_Login = DateTime.Now.Date;
                    Console.WriteLine("Gamepass: Giorno incrementato");
                }
                else if (daysDiff > 1)
                {
                    Console.WriteLine("Gamepass: Giorni resettati");
                    player.GamePass_Accessi_Consecutivi = 0;
                    player.Last_Login = DateTime.Now.Date;
                    player.GamePass_Premi = new bool[Variabili_Server.gamePass_DailyReward.Count()]; //Reset premi giornalieri
                }
                if (!player.GamePass_Avanzato && player.GamePass_Accessi_Consecutivi != 0)
                {
                    player.GamePass_Accessi_Consecutivi = 0;
                    Console.WriteLine("Gamepass: Gamepass scaduto, reset giorni");
                }
            }
        }
        async static void Lingua(Player player, string lang)
        {
            if (Variabili_Server.lingue_Supportate.Contains(lang)) player.Lingua = lang; //Imposta la lingua preferita del giocatore
            else player.Lingua = "it"; //Default Italiano
            Console.WriteLine($"[Server] Lingua selezionata: {lang}");
        }
        public async static void Cambia_Password(Guid clientGuid, Player player, string[] msgArgs)
        {
            switch (msgArgs[5])
            {
                case "mail": //Conferma email
                    if (player.Email == msgArgs[4])
                    {
                        int code = Random.Shared.Next(100000, 999999);
                        player.Email_Code = code;
                        player.Email_Code_Time = 15 * 3600;
                        //Invia il codice via email
                        //await EmailManager.SendPasswordRecoveryAsync(player.Email, player.Username, code.ToString());
                        //await EmailManager.SendPasswordRecoveryAsync("thechannelofadlos@gmail.com", player.Username, code.ToString());
                    }
                    else Console.WriteLine($"[Server] L'email non corrisponde per l'utente: {msgArgs[1]}");
                break;
                case "Change": //Cambio password se codice corrisponde!
                    if (player.Email_Code == Convert.ToInt32(msgArgs[2]) && player.Email_Code_Time > 0)
                    {
                        player.Password = msgArgs[3];
                        Server.Send(clientGuid, $"Password change|Change|true");
                    }
                    else
                    {
                        Server.Send(clientGuid, $"Password change|Change|false");
                    }
                break;
            }
        }
        public async static void Battaglia(Guid clientGuid, Player player, string[] dati)
        {
            int[] guerrieri = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte = new int[] { 0, 0, 0, 0, 0 };

            guerrieri[0] = Convert.ToInt32(dati[5]);
            guerrieri[1] = Convert.ToInt32(dati[6]);
            guerrieri[2] = Convert.ToInt32(dati[7]);
            guerrieri[3] = Convert.ToInt32(dati[8]);
            guerrieri[4] = Convert.ToInt32(dati[9]);

            picchieri[0] = Convert.ToInt32(dati[10]);
            picchieri[1] = Convert.ToInt32(dati[11]);
            picchieri[2] = Convert.ToInt32(dati[12]);
            picchieri[3] = Convert.ToInt32(dati[13]);
            picchieri[4] = Convert.ToInt32(dati[14]);

            arcieri[0] = Convert.ToInt32(dati[15]);
            arcieri[1] = Convert.ToInt32(dati[16]);
            arcieri[2] = Convert.ToInt32(dati[17]);
            arcieri[3] = Convert.ToInt32(dati[18]);
            arcieri[4] = Convert.ToInt32(dati[19]);

            catapulte[0] = Convert.ToInt32(dati[20]);
            catapulte[1] = Convert.ToInt32(dati[21]);
            catapulte[2] = Convert.ToInt32(dati[22]);
            catapulte[3] = Convert.ToInt32(dati[23]);
            catapulte[4] = Convert.ToInt32(dati[24]);

            if (dati[3] == "Villaggio Barbaro" || dati[3] == "Città Barbaro")
            {
                // --- PERCORSO ATTIVO (2026-09-14): BattagliaPVE.cs, con i bugfix applicati (vedi audit-battaglie.md) ---
                var attackerUnitsPVE = new Server_Strategico.ServerData.Moduli.Battaglie.Battaglia.UnitGroup
                {
                    Guerrieri = guerrieri,
                    Lancieri = picchieri,
                    Arcieri = arcieri,
                    Catapulte = catapulte
                };
                await Server_Strategico.ServerData.Moduli.Battaglie.BattagliaPVE.Battaglia(player, clientGuid, dati[3], Convert.ToInt32(dati[4]), attackerUnitsPVE);

                /* --- PERCORSO LEGACY (disattivato il 2026-09-14, tenuto come riferimento finché i test sul nuovo percorso
                   non danno l'ok — poi va eliminato insieme a BattaglieV2.Battaglia_Barbari e i suoi helper) ---
                if (dati[3] == "Villaggio Barbaro")
                    await BattaglieV2.Battaglia_Barbari(player, clientGuid, "Villaggio Barbaro", dati[4], guerrieri, picchieri, arcieri, catapulte);
                if (dati[3] == "Città Barbaro")
                    await BattaglieV2.Battaglia_Barbari(player, clientGuid, "Città Barbaro", dati[4], guerrieri, picchieri, arcieri, catapulte);
                */
            }

            AggiornaVillaggiClient(player);
            if (dati[3] == "PVP")
            {
                if (player.ScudoDellaPace > 0)
                {
                    Server.Send(clientGuid, "Log_Server|[title]Non puoi attaccare un giocatore... Hai lo scudo della pace attivo");
                    Console.WriteLine($"[Battaglia] [{player.Username}] Non puoi attaccare un giocatore... Hai lo scudo della pace attivo");
                    return;
                }

                var datisss = dati[4].Split(',');
                var difensore = Server.servers_.GetPlayer(datisss[0]);

                // --- PERCORSO ATTIVO (2026-09-14): BattagliaPVP.cs, con i bugfix applicati (vedi audit-battaglie.md) ---
                var attackerUnitsNuovo = new Server_Strategico.ServerData.Moduli.Battaglie.Battaglia.UnitGroup
                {
                    Guerrieri = guerrieri,
                    Lancieri = picchieri,
                    Arcieri = arcieri,
                    Catapulte = catapulte
                };
                await Server_Strategico.ServerData.Moduli.Battaglie.BattagliaPVP.Battaglia(player, difensore, attackerUnitsNuovo);

                /* --- PERCORSO LEGACY (disattivato il 2026-09-14, tenuto come riferimento finché i test sul nuovo percorso
                   non danno l'ok — poi va eliminato insieme a BattaglieV2.Battaglia_Strutture_PvP/Battaglia_PvP) ---
                var attackerUnits = new BattaglieV2.UnitGroup
                {
                    Guerrieri = guerrieri,
                    Lancieri = picchieri,
                    Arcieri = arcieri,
                    Catapulte = catapulte
                };
                BattaglieV2.BattleResult result = await BattaglieV2.Battaglia_Strutture_PvP(player, difensore, clientGuid, difensore.guid_Player, attackerUnits);
                if (result.Struttura == "Castello" && result.Victory == true)
                    BattaglieV2.Battaglia_PvP(player, difensore, clientGuid, difensore.guid_Player, result.AttaccantePerdite.Guerrieri, result.AttaccantePerdite.Lancieri, result.AttaccantePerdite.Arcieri, result.AttaccantePerdite.Catapulte);
                */

                Server.GameServer.GuerrieriCitta(player);
            }
        }
        public static void GamePass_Premi(Player player)
        {
            if (player.GamePass_Avanzato)
                for (int i = 0; i < Variabili_Server.gamePass_DailyReward.Count(); i++) //Forse utilizzando il numero dei giorni consecutivi posso evitare il for? da testare
                    if (player.GamePass_Accessi_Consecutivi == i && player.GamePass_Premi[i] == false)
                    {
                        player.Diamanti_Viola += Variabili_Server.gamePass_DailyReward[i];
                        player.GamePass_Premi[i] = true;
                    }
            GamePass_Premi_Send(player);
        }
        public static void TutorialUpdate(Player player, string[] Dati)
        {
            bool precedenti_Completati = true;
            int missione = Convert.ToInt32(Dati[3]) - 1;
            for (int i = 0; i < missione; i++)
                if (!player.Tutorial_Stato[i]) // se uno NON è completato
                {
                    precedenti_Completati = false;
                    break;
                }
            
            if (Dati[3] == "1" && player.Tutorial_Stato[missione] != true) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true;
                player.Cibo = 0;
                player.Legno = 0;
                player.Pietra = 0;
                player.Ferro = 0;
                player.Oro = 0;
                player.Popolazione = 0;
                player.Diamanti_Viola = 0;
                player.Diamanti_Blu = 0;
                player.Dollari_Virtuali = 0;

                player.Spade = 0;
                player.Lance = 0;
                player.Archi = 0;
                player.Scudi = 0;
                player.Armature = 0;
                player.Frecce = 0;

                player.Terreno_NonComune = 0;
                player.Terreno_Comune = 0;
                player.Terreno_Raro = 0;
                player.Terreno_Epico = 0;
                player.Terreno_Leggendario = 0;

                player.Fattoria = 0;
                player.Segheria = 0;
                player.CavaPietra = 0;
                player.MinieraFerro = 0;
                player.MinieraOro = 0;
                player.Abitazioni = 0;

                //Statistiche
                player.Unità_Eliminate = 0;      
                player.Guerrieri_Eliminati = 0;  
                player.Lanceri_Eliminati = 0;    
                player.Arceri_Eliminati = 0;     
                player.Catapulte_Eliminate = 0;  
                player.Unità_Addestrate = 0;     

                player.Unità_Perse = 0;          
                player.Guerrieri_Persi = 0;      
                player.Lanceri_Persi = 0;        
                player.Arceri_Persi = 0;         
                player.Catapulte_Perse = 0;      
                player.Risorse_Razziate = 0;     

                player.Strutture_Civili_Costruite = 0;     
                player.Strutture_Militari_Costruite = 0;   
                player.Caserme_Costruite = 0;              

                player.Frecce_Utilizzate = 0;      
                player.Battaglie_Vinte = 0;        
                player.Battaglie_Perse = 0;         
                player.Quest_Completate = 0;       
                player.Attacchi_Subiti_PVP = 0;
                player.Attacchi_Effettuati_PVP = 0;

                player.Barbari_Sconfitti = 0; 
                player.Accampamenti_Barbari_Sconfitti = 0;
                player.Città_Barbare_Sconfitte = 0;
                player.Danno_HP_Barbaro = 0;
                player.Danno_DEF_Barbaro = 0;

                player.Risorse_Utilizzate = 0;       
                player.Tempo_Addestramento = 0;      
                player.Tempo_Costruzione = 0;        
                player.Tempo_Ricerca = 0;            
                player.Tempo_Sottratto_Diamanti = 0; 

                player.Consumo_Cibo_Esercito = 0;    
                player.Consumo_Oro_Esercito = 0;     
                player.Diamanti_Viola_Utilizzati = 0;
                player.Diamanti_Blu_Utilizzati = 0;  
                Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");

            }
            else if (Dati[3] == "2" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "3" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "4" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "5" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "6" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "7" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; player.Diamanti_Viola = 150; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "8" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");}
            else if (Dati[3] == "9" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "10" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }

            else if (Dati[3] == "11" && player.Tutorial_Stato[missione] != true && precedenti_Completati) 
            { 
                player.Tutorial_Stato[missione] = true;
                if (!player.Tutorial_Premi[missione])
                {
                    player.Cibo = Strutture.Edifici.Fattoria.Cibo;
                    player.Legno = Strutture.Edifici.Fattoria.Legno;
                    player.Pietra = Strutture.Edifici.Fattoria.Pietra;
                    player.Ferro = Strutture.Edifici.Fattoria.Ferro;
                    player.Oro = Strutture.Edifici.Fattoria.Oro;
                    player.Popolazione = Strutture.Edifici.Fattoria.Popolazione;
                    player.Tutorial_Premi[10] = true;
                    Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");
                }
            }
            else if (Dati[3] == "12" && player.Tutorial_Stato[missione] != true && precedenti_Completati)
            {
                player.Tutorial_Stato[missione] = true;
                if (!player.Tutorial_Premi[missione])
                {
                    player.Tutorial_Premi[missione] = true;
                    player.Diamanti_Viola = (int)Strutture.Edifici.Fattoria.TempoCostruzione / Variabili_Server.D_Viola_To_Blu / Variabili_Server.Velocizzazione_Tempo + 5;
                    Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");
                }
            }
            else if (Dati[3] == "13" && player.Tutorial_Stato[missione] != true && precedenti_Completati) //Scambia diamanti
            {   
                player.Tutorial_Stato[missione] = true; 
                player.Tutorial_Premi[missione] = true;
                Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");
            }
            else if (Dati[3] == "14" && player.Tutorial_Stato[missione] != true && precedenti_Completati) 
            { 
                player.Tutorial_Stato[missione] = true;
                if (!player.Tutorial_Premi[missione])
                {
                    player.Cibo = Strutture.Edifici.Segheria.Cibo;
                    player.Legno = Strutture.Edifici.Segheria.Legno;
                    player.Pietra = Strutture.Edifici.Segheria.Pietra;
                    player.Ferro = Strutture.Edifici.Segheria.Ferro;
                    player.Oro = Strutture.Edifici.Segheria.Oro;
                    player.Popolazione = Strutture.Edifici.Segheria.Popolazione;
                    player.Diamanti_Viola += (int)Strutture.Edifici.Segheria.TempoCostruzione / Variabili_Server.D_Viola_To_Blu / Variabili_Server.Velocizzazione_Tempo + 10;
                    player.Diamanti_Blu += 20;
                    player.Tutorial_Premi[missione] = true;
                    Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");
                }
            }
            else if (Dati[3] == "15" && player.Tutorial_Stato[missione] != true && precedenti_Completati)
            {
                player.Tutorial_Stato[missione] = true;
                if (!player.Tutorial_Premi[missione])
                {
                    player.Cibo = Strutture.Edifici.CavaPietra.Cibo;
                    player.Legno = Strutture.Edifici.CavaPietra.Legno;
                    player.Pietra = Strutture.Edifici.CavaPietra.Pietra;
                    player.Ferro = Strutture.Edifici.CavaPietra.Ferro;
                    player.Oro = Strutture.Edifici.CavaPietra.Oro;
                    player.Popolazione = Strutture.Edifici.CavaPietra.Popolazione;
                    player.Diamanti_Viola += 5;
                    player.Diamanti_Blu += (int)Strutture.Edifici.MinieraFerro.TempoCostruzione / Variabili_Server.Velocizzazione_Tempo + 10;
                    player.Tutorial_Premi[missione] = true;
                    Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");
                }
            }
            else if (Dati[3] == "16" && player.Tutorial_Stato[missione] != true && precedenti_Completati) 
            {
                player.Tutorial_Stato[missione] = true;
                if (!player.Tutorial_Premi[missione])
                {
                    player.Cibo = Strutture.Edifici.MinieraFerro.Cibo;
                    player.Legno = Strutture.Edifici.MinieraFerro.Legno;
                    player.Pietra = Strutture.Edifici.MinieraFerro.Pietra;
                    player.Ferro = Strutture.Edifici.MinieraFerro.Ferro;
                    player.Oro = Strutture.Edifici.MinieraFerro.Oro;
                    player.Popolazione = Strutture.Edifici.MinieraFerro.Popolazione;
                    player.Diamanti_Viola += 5;
                    player.Diamanti_Blu += (int)Strutture.Edifici.MinieraFerro.TempoCostruzione / Variabili_Server.Velocizzazione_Tempo + 20;
                    player.Tutorial_Premi[missione] = true;
                    Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");
                }
            }
            else if (Dati[3] == "17" && player.Tutorial_Stato[missione] != true && precedenti_Completati) 
            {
                player.Tutorial_Stato[missione] = true;
                if (!player.Tutorial_Premi[missione])
                {
                    player.Cibo = Strutture.Edifici.MinieraOro.Cibo;
                    player.Legno = Strutture.Edifici.MinieraOro.Legno;
                    player.Pietra = Strutture.Edifici.MinieraOro.Pietra;
                    player.Ferro = Strutture.Edifici.MinieraOro.Ferro;
                    player.Oro = Strutture.Edifici.MinieraOro.Oro;
                    player.Popolazione = Strutture.Edifici.MinieraOro.Popolazione;
                    player.Diamanti_Viola += 5;
                    player.Diamanti_Blu += (int)Strutture.Edifici.MinieraOro.TempoCostruzione / Variabili_Server.Velocizzazione_Tempo + 30;
                    player.Tutorial_Premi[16] = true;
                    Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");
                }
            }
            else if (Dati[3] == "18" && player.Tutorial_Stato[missione] != true && precedenti_Completati) 
            {
                player.Tutorial_Stato[missione] = true;
                if (!player.Tutorial_Premi[missione])
                {
                    player.Cibo = Strutture.Edifici.Case.Cibo;
                    player.Legno = Strutture.Edifici.Case.Legno;
                    player.Pietra = Strutture.Edifici.Case.Pietra;
                    player.Ferro = Strutture.Edifici.Case.Ferro;
                    player.Oro = Strutture.Edifici.Case.Oro;
                    player.Popolazione = Strutture.Edifici.Case.Popolazione;
                    player.Diamanti_Viola += 5;
                    player.Diamanti_Blu += (int)Strutture.Edifici.Case.TempoCostruzione / Variabili_Server.Velocizzazione_Tempo + 40;
                    player.Tutorial_Premi[missione] = true;
                    Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");
                }
            }
            else if (Dati[3] == "19" && player.Tutorial_Stato[missione] != true && precedenti_Completati)
            {
                player.Tutorial_Stato[missione] = true;
                player.Tutorial_Premi[missione] = true;
                Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");
                player.SetupVillaggioGiocatore(player);
            }
            else if (Dati[3] == "20" && player.Tutorial_Stato[missione] != true&& precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "21" && player.Tutorial_Stato[missione] != true&& precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "22" && player.Tutorial_Stato[missione] != true&& precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "23" && player.Tutorial_Stato[missione] != true&& precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "24" && player.Tutorial_Stato[missione] != true && precedenti_Completati) 
            { 
                player.Tutorial_Stato[missione] = true; 
                if (!player.Tutorial_Premi[missione])
                {
                    player.Tutorial_Premi[missione] = true;
                    player.Salute_Mura = player.Salute_MuraMax;
                    player.Salute_Mura -= 5;
                    player.Difesa_Mura = player.Difesa_MuraMax;
                    player.Difesa_Mura -= 5;
                    player.Cibo += Riparazione.Mura.Consumo_Cibo * 10;
                    player.Legno += Riparazione.Mura.Consumo_Legno * 10;
                    player.Pietra += Riparazione.Mura.Consumo_Pietra * 10;
                    player.Ferro += Riparazione.Mura.Consumo_Ferro * 10;
                    player.Oro += Riparazione.Mura.Consumo_Oro * 10;
                    Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");
                }
            }
            else if (Dati[3] == "25" && player.Tutorial_Stato[missione] != true && precedenti_Completati) // Riparazione mura
            { 
                player.Tutorial_Stato[missione] = true;
                if (!player.Tutorial_Premi[missione])
                {
                    Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");
                }
            }
            else if (Dati[3] == "26" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "27" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "28" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "29" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "30" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "31" && player.Tutorial_Stato[missione] != true && precedenti_Completati) { player.Tutorial_Stato[missione] = true; player.Tutorial_Premi[missione] = true; Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}"); }
            else if (Dati[3] == "32" && player.Tutorial_Stato[missione] != true && precedenti_Completati) 
            { 
                player.Tutorial_Stato[missione] = true;
                player.Tutorial_Premi[missione] = true;
                player.Punti_Quest = 0;
                player.Diamanti_Viola += 150;
                player.Diamanti_Viola += 200;
                Console.WriteLine($"[Tutorial] >> Giocatore: {player.Username} - Missione {Dati[3]} Completata - Stato: {player.Tutorial_Stato[missione]}");
                //Andrebbe aggiunto un salvataggio dei dati dell'utente a questo pounto...
            }

        }
        public static void Set_Riparazioni(Player player, string[] Dati)
        {
            if (Dati[3] == "Ripara Tutto")
            {
                if (player.Salute_Cancello < player.Salute_CancelloMax) player.Riparazioni[0] = true;
                if (player.Difesa_Cancello < player.Difesa_CancelloMax) player.Riparazioni[1] = true;
                if (player.Salute_Mura < player.Salute_MuraMax) player.Riparazioni[2] = true;
                if (player.Difesa_Mura < player.Difesa_MuraMax) player.Riparazioni[3] = true;
                if (player.Salute_Torri < player.Salute_TorriMax) player.Riparazioni[4] = true;
                if (player.Difesa_Torri < player.Difesa_TorriMax) player.Riparazioni[5] = true;
                if (player.Salute_Castello < player.Salute_CastelloMax) player.Riparazioni[6] = true;
                if (player.Difesa_Castello < player.Difesa_CastelloMax) player.Riparazioni[7] = true;
                return;
            }

            if (Dati[3] == "Cancello")
            {
                if (Dati[4] == "Salute" && player.Salute_Cancello < player.Salute_CancelloMax) player.Riparazioni[0] = true;
                if (Dati[4] == "Difesa" && player.Difesa_Cancello < player.Difesa_CancelloMax) player.Riparazioni[1] = true;
            }
            if (Dati[3] == "Mura")
            {
                if (Dati[4] == "Salute" && player.Salute_Mura < player.Salute_MuraMax) player.Riparazioni[2] = true;
                if (Dati[4] == "Difesa" && player.Difesa_Mura < player.Difesa_MuraMax) player.Riparazioni[3] = true;
            }
            if (Dati[3] == "Torri")
            {
                if (Dati[4] == "Salute" && player.Salute_Torri < player.Salute_TorriMax) player.Riparazioni[4] = true;
                if (Dati[4] == "Difesa" && player.Difesa_Torri < player.Difesa_TorriMax) player.Riparazioni[5] = true;
            }
            if (Dati[3] == "Castello")
            {
                if (Dati[4] == "Salute" && player.Salute_Castello < player.Salute_CastelloMax) player.Riparazioni[6] = true;
                if (Dati[4] == "Difesa" && player.Difesa_Castello < player.Difesa_CastelloMax) player.Riparazioni[7] = true;
            }
        }
        public static void Stop_Riparazione (Player player)
        {
            int i = 0;
            foreach (var strutture in player.Riparazioni)
            {
                player.Riparazioni[i] = false;
                i++;
            }

        }

        public static void SpostamentoTruppe(Guid guid, Player player, string[] dati)
        {
            string edificio_From = dati[3];
            string edificio_To = dati[4];
            int g = Convert.ToInt32(dati[5]);
            int l = Convert.ToInt32(dati[6]);
            int a = Convert.ToInt32(dati[7]);
            int c = Convert.ToInt32(dati[8]);
            int livello = Convert.ToInt32(dati[9]) - 1;

            if (g + l + a + c == 0) return;
            if (livello < 0 || livello > 4) return; // tier fuori dai 5 validi (0-4): dato non attendibile, non si tocca nulla.

            bool fromVillaggio = edificio_From == "Esercito Villaggio";
            bool toVillaggio = edificio_To == "Esercito Villaggio";
            if (fromVillaggio && toVillaggio) return; // il client non lo invia mai: nessuna struttura reale coinvolta.

            int[] srcG, srcL, srcA, srcC;
            if (fromVillaggio) { srcG = player.Guerrieri; srcL = player.Lanceri; srcA = player.Arceri; srcC = player.Catapulte; }
            else if (!TruppeStruttura(player, edificio_From, out srcG, out srcL, out srcA, out srcC, out _)) return; // nome struttura sconosciuto

            int[] destG, destL, destA, destC;
            int guarnigioneMax = 0;
            if (toVillaggio) { destG = player.Guerrieri; destL = player.Lanceri; destA = player.Arceri; destC = player.Catapulte; }
            else if (!TruppeStruttura(player, edificio_To, out destG, out destL, out destA, out destC, out guarnigioneMax)) return;

            int mg = srcG[livello] >= g ? g : 0;
            int ml = srcL[livello] >= l ? l : 0;
            int ma = srcA[livello] >= a ? a : 0;
            int mc = srcC[livello] >= c ? c : 0;
            if (mg + ml + ma + mc == 0) return; // niente da spostare: la sorgente non ha nulla di richiesto

            if (!toVillaggio && mg + ml + ma + mc > guarnigioneMax) return;

            srcG[livello] -= mg; destG[livello] += mg;
            srcL[livello] -= ml; destL[livello] += ml;
            srcA[livello] -= ma; destA[livello] += ma;
            srcC[livello] -= mc; destC[livello] += mc;
        }

        private static bool TruppeStruttura(Player player, string struttura, out int[] guerrieri, out int[] lanceri, out int[] arceri, out int[] catapulte, out int guarnigioneMax)
        {
            switch (struttura)
            {
                case "Ingresso":
                    guerrieri = player.Guerrieri_Ingresso; lanceri = player.Lanceri_Ingresso; arceri = player.Arceri_Ingresso; catapulte = player.Catapulte_Ingresso;
                    guarnigioneMax = player.Guarnigione_IngressoMax;
                    return true;
                case "Cancello":
                    guerrieri = player.Guerrieri_Cancello; lanceri = player.Lanceri_Cancello; arceri = player.Arceri_Cancello; catapulte = player.Catapulte_Cancello;
                    guarnigioneMax = player.Guarnigione_CancelloMax;
                    return true;
                case "Mura":
                    guerrieri = player.Guerrieri_Mura; lanceri = player.Lanceri_Mura; arceri = player.Arceri_Mura; catapulte = player.Catapulte_Mura;
                    guarnigioneMax = player.Guarnigione_MuraMax;
                    return true;
                case "Torri":
                    guerrieri = player.Guerrieri_Torri; lanceri = player.Lanceri_Torri; arceri = player.Arceri_Torri; catapulte = player.Catapulte_Torri;
                    guarnigioneMax = player.Guarnigione_TorriMax;
                    return true;
                case "Castello":
                    guerrieri = player.Guerrieri_Castello; lanceri = player.Lanceri_Castello; arceri = player.Arceri_Castello; catapulte = player.Catapulte_Castello;
                    guarnigioneMax = player.Guarnigione_CastelloMax;
                    return true;
                case "Citta":
                    guerrieri = player.Guerrieri_Citta; lanceri = player.Lanceri_Citta; arceri = player.Arceri_Citta; catapulte = player.Catapulte_Citta;
                    guarnigioneMax = player.Guarnigione_CittaMax; // prima, per un bug copia-incolla, veniva sempre confrontata con Guarnigione_IngressoMax
                    return true;
                default:
                    guerrieri = lanceri = arceri = catapulte = null;
                    guarnigioneMax = 0;
                    return false;
            }
        }
        public static void Scambia_Diamanti(Guid guid, Player player, string Quantità)
        {
            int diamanti_Viola = Convert.ToInt32(Quantità);
            if (diamanti_Viola == 0) return;
            if (player.Diamanti_Viola >= diamanti_Viola)
            {
                player.Diamanti_Viola -= diamanti_Viola;
                player.Diamanti_Blu += diamanti_Viola * Variabili_Server.D_Viola_To_Blu;
                OnEvent(player, QuestEventType.Risorse, "Diamanti Viola", diamanti_Viola);
                Server.Send(player.guid_Player, $"Log_Server|Scambiati [warning][icon:diamanteViola]{diamanti_Viola}[viola] Diamanti Viola[/viola] --> [icon:diamanteBlu][warning]{diamanti_Viola * Variabili_Server.D_Viola_To_Blu}[blu] Diamanti Blu");

                //Tutorial
                var msgArgs = "0|1|2|3".Split('|');
                if (player.Tutorial && !player.Tutorial_Stato[12])
                {
                    msgArgs[3] = "13";
                    ServerConnection.TutorialUpdate(player, msgArgs);
                }
            }
        }
        public static void Scambia_Tributi(Guid guid, Player player, string Quantità)
        {
            int tributi = Convert.ToInt32(Quantità);
            if (tributi == 0) return;
            if (player.Dollari_Virtuali >= tributi)
            {
                player.Dollari_Virtuali -= tributi;
                player.Diamanti_Viola += tributi * Variabili_Server.Tributi_To_D_Viola;
                Server.Send(player.guid_Player, $"Log_Server|Scambiati [warning][icon:dollariVirtuali]{tributi} Tributi --> [icon:diamanteViola][warning]{tributi * Variabili_Server.D_Viola_To_Blu}[viola] Diamanti Viola");
            }
        }
        // 16/09/2026: nuovo punto di ingresso per lo spionaggio, chiamato dal case "Esplora" —
        // il nome del comando client resta invariato, cambia solo il payload (vedi il case sopra).
        // PVP: bersaglio = username del giocatore da spiare, livelloStr = null.
        // PVE: bersaglio = "Citta Barbaro" / "Villaggio Barbaro" (stesso valore che aveva "globale"
        //      nel vecchio Esplora), livelloStr = livello del barbaro bersaglio.
        public static void EseguiSpionaggioRichiesta(Player attaccante, string modalità, string bersaglio, string livelloStr)
        {
            if (modalità == "PVP")
            {
                var difensore = Server.servers_.GetPlayer(bersaglio);
                if (difensore == null)
                {
                    Server.Send(attaccante.guid_Player, "Log_Server|[warning]Giocatore da spiare non trovato.");
                    return;
                }
                Spionaggio.EseguiSpionaggioPVP(difensore, attaccante, "PVP");
                return;
            }

            // PVE
            if (!int.TryParse(livelloStr, out int livello))
            {
                Server.Send(attaccante.guid_Player, "Log_Server|[warning]Livello del bersaglio non valido.");
                return;
            }

            BarbarianBase target;
            if (bersaglio == "Citta Barbaro")
                target = Gioco.Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == livello);
            else if (bersaglio == "Villaggio Barbaro")
                target = attaccante.VillaggiPersonali.FirstOrDefault(v => v.Livello == livello);
            else
                target = null;

            if (target == null)
            {
                Server.Send(attaccante.guid_Player, "Log_Server|[warning]Barbaro non trovato.");
                return;
            }

            Spionaggio.EseguiSpionaggioPVE(target, attaccante);
        }

        // 16/09/2026: deprecato a favore del nuovo spionaggio (vedi Spionaggio.EseguiSpionaggio/
        // SpionaggioPVE), che sostituisce la stima ±20% con la stessa meccanica forza/precisione/
        // stadio già usata nel PVP e produce un vero Report invece di un semplice aggiornamento
        // della lista. Lasciato qui invariato: non più raggiungibile dal case "Esplora" (che ora
        // chiama EseguiSpionaggioRichiesta sopra), ma il metodo resta finché non si è sicuri che
        // nient'altro lo richiami.
        [Obsolete("Sostituito da EseguiSpionaggioRichiesta / Spionaggio.EseguiSpionaggio (PVE).")]
        public static void Esplora(Player player, int livello_Barbaro, string globale)
        {
            BarbarianBase target;

            if (globale == "Citta Barbaro")
                target = Gioco.Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == livello_Barbaro); // Cerca la città globale del livello richiesto
            else if (globale == "Villaggio Barbaro")
                target = player.VillaggiPersonali.FirstOrDefault(v => v.Livello == livello_Barbaro); // Cerca il villaggio personale del giocatore
            else target = null;

            if (target == null)
            {
                var errore = new
                {
                    Type = "ErroreEsplorazione",
                    Messaggio = "Barbaro non trovato."
                };
                Server.Send(player.guid_Player, JsonSerializer.Serialize(errore));
                return;
            }

            var (gw, ln, ar, ct) = Gioco.Barbari.EsploraTruppe(player, target); // Chiama la funzione che restituisce le truppe stimate
            if (gw == -1) // Controlla se c'è abbastanza oro
            {
                var errore = new
                {
                    Type = "ErroreEsplorazione",
                    Messaggio = "Non hai abbastanza oro per esplorare."
                };
                Server.Send(player.guid_Player, JsonSerializer.Serialize(errore));
                return;
            }

            if (globale == "Villaggio Barbaro") // --- Villaggi personali ---
            {
                OnEvent(player, QuestEventType.Battaglie, "Esplora Villaggio Barbaro", 1); //Quest
                OnEvent(player, QuestEventType.Battaglie, "Esplora Qualsiasi", 1); //Quest
                var villaggiDaInviare = new List<object>();
                for (int i = 0; i < player.VillaggiPersonali.Count; i++)
                {
                    var v = player.VillaggiPersonali[i];
                    if (i == 0 || player.VillaggiPersonali[i - 1].Sconfitto) // Primo villaggio sempre inviato, gli altri solo se il precedente è sconfitto
                        villaggiDaInviare.Add(new
                        {
                            v.Id,
                            v.Nome,
                            v.Livello,
                            Esperienza = v.Esperienza,
                            Diamanti_Viola = v.Diamanti_Viola,
                            Diamanti_Blu = v.Diamanti_Blu,
                            Esplorato = v.Esplorato,
                            Sconfitto = v.Sconfitto,
                            Cibo = v.Cibo,
                            Legno = v.Legno,
                            Pietra = v.Pietra,
                            Ferro = v.Ferro,
                            Oro = v.Oro,
                            Guerrieri = gw,
                            Lancieri = ln,
                            Arcieri = ar,
                            Catapulte = ct
                        });
                    else break;
                }

                var villaggiUpdate = new
                {
                    Type = "VillaggiPersonali",
                    Dati = villaggiDaInviare
                };
                Server.Send(player.guid_Player, JsonSerializer.Serialize(villaggiUpdate));
            }else
            {
                var cittaDaInviare = new List<object>();
                OnEvent(player, QuestEventType.Battaglie, "Esplora Villaggio Barbaro", 1); //Quest
                OnEvent(player, QuestEventType.Battaglie, "Esplora Qualsiasi", 1); //Quest
                for (int i = 0; i < CittaGlobali.Count; i++)
                {
                    var v = CittaGlobali[i];
                    if (i == 0 || CittaGlobali[i - 1].Sconfitto) // Primo villaggio sempre inviato, gli altri solo se il precedente è sconfitto
                        cittaDaInviare.Add(new
                        {
                            v.Id,
                            v.Nome,
                            v.Livello,
                            Esperienza = v.Esperienza,
                            Diamanti_Viola = v.Diamanti_Viola,
                            Diamanti_Blu = v.Diamanti_Blu,
                            Esplorato = v.Esplorato,
                            Sconfitto = v.Sconfitto,
                            Cibo = v.Cibo,
                            Legno = v.Legno,
                            Pietra = v.Pietra,
                            Ferro = v.Ferro,
                            Oro = v.Oro,
                            Guerrieri = gw,
                            Lancieri = ln,
                            Arcieri = ar,
                            Catapulte = ct
                        });
                    else break;
                }

                var cittaUpdate = new
                {
                    Type = "CittaGlobali",
                    Dati = cittaDaInviare
                };
                Server.Send(player.guid_Player, JsonSerializer.Serialize(cittaUpdate));
            }
        }
        static async void Quest_Reward(string[] msgArgs, Player player, Guid guid)
        {
            bool premioRaccolto = false;
            int quest = Convert.ToInt32(msgArgs[4]);
            int reward = Convert.ToInt32(msgArgs[4]) - 1;
            switch (msgArgs[3])
            {
                case "Normale":
                    {
                        if (player.PremiNormali[reward] == true) return;
                        if (quest == 2 || quest == 4 || quest == 7 || quest == 11 || quest == 14 || quest == 17)
                        {
                            player.PremiNormali[reward] = true;
                            player.Diamanti_Blu += QuestManager.QuestRewardSet.Normali_Monthly.Rewards[reward];
                            QuestManager.QuestRewardUpdate(player);
                            QuestManager.QuestUpdate(player);
                            return;
                        }else
                        {
                            player.PremiNormali[reward] = true;
                            player.Diamanti_Viola += QuestManager.QuestRewardSet.Normali_Monthly.Rewards[reward];
                        }
                        premioRaccolto = true;
                    }
                    break;
                case "Vip":
                    {
                        if (player.GamePass_Base == false) return;
                        if (player.PremiVIP[reward] == true) return;
                        if (quest == 3)
                        {
                            player.PremiVIP[2] = true;
                            player.Diamanti_Blu += QuestManager.QuestRewardSet.Vip_Monthly.Rewards[2];
                        }
                        else if (quest == 20)
                        {
                            player.PremiVIP[19] = true;
                            player.Terreno_Leggendario += 1;
                            return;
                        }else
                        {
                            player.PremiVIP[reward] = true;
                            player.Diamanti_Viola += QuestManager.QuestRewardSet.Vip_Monthly.Rewards[reward];
                        }
                        premioRaccolto = true;
                        QuestManager.QuestRewardUpdate(player);
                        QuestManager.QuestUpdate(player);
                    }
                    break;

            }
            if (premioRaccolto)
            {
                QuestManager.QuestRewardUpdate(player);
                QuestManager.QuestUpdate(player);
                premioRaccolto = false;
            }
        }
        public static async Task<bool> New_Player(string username, string password, string email, Guid guid)
        {
            var existingPlayer = Server.servers_.GetPlayer(username, password);

            if (username == "")
            {
                Console.WriteLine($"New Player: Username non valido ({username})");
                return false;
            }
            if (password == "")
            {
                Console.WriteLine($"New Player: Pasword non valido ({password})");
                return false;
            }
            if (email == "")
            {
                Console.WriteLine($"New Player: Email non valido ({email})");
                return false;
            }

            if (existingPlayer != null) // Controlla se il giocatore esiste già
            {
                existingPlayer.guid_Player = guid; //Assegna il guid aggiornato
                Console.WriteLine("New Player: Il giocatore già esiste");
                return false;
            }
            if (await Server.servers_.Check_Username_Player(username)) // Controlla se il nome utente è disponibile
            {
                await Server.servers_.AddPlayer(username, password, email, guid);
                //await GameSave.LoadPlayer(username, password);
                return true;
            }
            return false;
        }
        static async Task<bool> Login(string username, string password, string email, Guid guid)
        {
            var existingPlayer = Server.servers_.GetPlayer(username, password);
            if (existingPlayer != null) // Controlla se il giocatore esiste già
            {
                existingPlayer.guid_Player = guid; //Assegna il guid aggiornato
                Console.WriteLine("Login: Il giocatore già esiste");

                Server.Client_Connessi_Map.TryRemove(guid, out _);
                Server.Client_Connessi_Map.TryAdd(guid, username);
                return true;
            }
            else return false;
        }
        public static void GamePass_Premi_Send(Player player)
        {
            string gamePass_DailyReward_Ottenuti = "";
            foreach (var item in player.GamePass_Premi)
                gamePass_DailyReward_Ottenuti += $"|{item}";
            Server.Send(player.guid_Player, "Gamepass_Premi_Ottenuti" + gamePass_DailyReward_Ottenuti);

            string gamePass_DailyReward = "";
            foreach (var item in Variabili_Server.gamePass_DailyReward)
                gamePass_DailyReward += $"|{item}";
            Server.Send(player.guid_Player, "Gamepass_Premi" + gamePass_DailyReward);

        }
        public static void Tutorial(Player player)
        {
            if (player.Tutorial == true) //Reset tutorial se non completato, causa disconnessione, chiusura, bug o altro...
                for (int i = 0; i < player.Tutorial_Stato.Count(); i++)
                {
                    if (player.Tutorial_Stato[i] == false) break;
                    player.Tutorial_Stato[i] = false;
                }

            var L = LocalizationManager.Get(player); // ← unica riga aggiunta

            List<Tutorial.dati> tutorial = new List<Tutorial.dati>
            {
                new Tutorial.dati { StatoTutorial = 1,  Obiettivo = L.GetTutorialObiettivo(1),  Descrizione = L.GetTutorialDescrizione(1)  },
                new Tutorial.dati { StatoTutorial = 2,  Obiettivo = L.GetTutorialObiettivo(2),  Descrizione = L.GetTutorialDescrizione(2)  },
                new Tutorial.dati { StatoTutorial = 3,  Obiettivo = L.GetTutorialObiettivo(3),  Descrizione = L.GetTutorialDescrizione(3)  },
                new Tutorial.dati { StatoTutorial = 4,  Obiettivo = L.GetTutorialObiettivo(4),  Descrizione = L.GetTutorialDescrizione(4)  },
                new Tutorial.dati { StatoTutorial = 5,  Obiettivo = L.GetTutorialObiettivo(5),  Descrizione = L.GetTutorialDescrizione(5)  },
                new Tutorial.dati { StatoTutorial = 6,  Obiettivo = L.GetTutorialObiettivo(6),  Descrizione = L.GetTutorialDescrizione(6)  },
                new Tutorial.dati { StatoTutorial = 7,  Obiettivo = L.GetTutorialObiettivo(7),  Descrizione = L.GetTutorialDescrizione(7)  },
                new Tutorial.dati { StatoTutorial = 8,  Obiettivo = L.GetTutorialObiettivo(8),  Descrizione = L.GetTutorialDescrizione(8)  },
                new Tutorial.dati { StatoTutorial = 9,  Obiettivo = L.GetTutorialObiettivo(9),  Descrizione = L.GetTutorialDescrizione(9)  },
                new Tutorial.dati { StatoTutorial = 10, Obiettivo = L.GetTutorialObiettivo(10), Descrizione = L.GetTutorialDescrizione(10) },
                new Tutorial.dati { StatoTutorial = 11, Obiettivo = L.GetTutorialObiettivo(11), Descrizione = L.GetTutorialDescrizione(11) },
                new Tutorial.dati { StatoTutorial = 12, Obiettivo = L.GetTutorialObiettivo(12), Descrizione = L.GetTutorialDescrizione(12) },
                new Tutorial.dati { StatoTutorial = 13, Obiettivo = L.GetTutorialObiettivo(13), Descrizione = L.GetTutorialDescrizione(13) },
                new Tutorial.dati { StatoTutorial = 14, Obiettivo = L.GetTutorialObiettivo(14), Descrizione = L.GetTutorialDescrizione(14) },
                new Tutorial.dati { StatoTutorial = 15, Obiettivo = L.GetTutorialObiettivo(15), Descrizione = L.GetTutorialDescrizione(15) },
                new Tutorial.dati { StatoTutorial = 16, Obiettivo = L.GetTutorialObiettivo(16), Descrizione = L.GetTutorialDescrizione(16) },
                new Tutorial.dati { StatoTutorial = 17, Obiettivo = L.GetTutorialObiettivo(17), Descrizione = L.GetTutorialDescrizione(17) },
                new Tutorial.dati { StatoTutorial = 18, Obiettivo = L.GetTutorialObiettivo(18), Descrizione = L.GetTutorialDescrizione(18) },
                new Tutorial.dati { StatoTutorial = 19, Obiettivo = L.GetTutorialObiettivo(19), Descrizione = L.GetTutorialDescrizione(19) },
                new Tutorial.dati { StatoTutorial = 20, Obiettivo = L.GetTutorialObiettivo(20), Descrizione = L.GetTutorialDescrizione(20) },
                new Tutorial.dati { StatoTutorial = 21, Obiettivo = L.GetTutorialObiettivo(21), Descrizione = L.GetTutorialDescrizione(21) },
                new Tutorial.dati { StatoTutorial = 22, Obiettivo = L.GetTutorialObiettivo(22), Descrizione = L.GetTutorialDescrizione(22) },
                new Tutorial.dati { StatoTutorial = 23, Obiettivo = L.GetTutorialObiettivo(23), Descrizione = L.GetTutorialDescrizione(23) },
                new Tutorial.dati { StatoTutorial = 24, Obiettivo = L.GetTutorialObiettivo(24), Descrizione = L.GetTutorialDescrizione(24) },
                new Tutorial.dati { StatoTutorial = 25, Obiettivo = L.GetTutorialObiettivo(25), Descrizione = L.GetTutorialDescrizione(25) },
                new Tutorial.dati { StatoTutorial = 26, Obiettivo = L.GetTutorialObiettivo(26), Descrizione = L.GetTutorialDescrizione(26) },
                new Tutorial.dati { StatoTutorial = 27, Obiettivo = L.GetTutorialObiettivo(27), Descrizione = L.GetTutorialDescrizione(27) },
                new Tutorial.dati { StatoTutorial = 28, Obiettivo = L.GetTutorialObiettivo(28), Descrizione = L.GetTutorialDescrizione(28) },
                new Tutorial.dati { StatoTutorial = 29, Obiettivo = L.GetTutorialObiettivo(29), Descrizione = L.GetTutorialDescrizione(29) },
                new Tutorial.dati { StatoTutorial = 30, Obiettivo = L.GetTutorialObiettivo(30), Descrizione = L.GetTutorialDescrizione(30) },
                new Tutorial.dati { StatoTutorial = 31, Obiettivo = L.GetTutorialObiettivo(31), Descrizione = L.GetTutorialDescrizione(31) },
                new Tutorial.dati { StatoTutorial = 32, Obiettivo = L.GetTutorialObiettivo(32), Descrizione = L.GetTutorialDescrizione(32) },
            };
            Server.Send(player.guid_Player, "Tutorial|Dati|" + JsonSerializer.Serialize(tutorial));
        }
        public static void AggiornaVillaggiClient(Player player)
        {
            if (player.VillaggiPersonali == null || player.VillaggiPersonali.Count == 0)
                return;

            // --- Villaggi personali ---
            var villaggiDaInviare = new List<object>();
            for (int i = 0; i < player.VillaggiPersonali.Count; i++)
            {
                var v = player.VillaggiPersonali[i];
                if (i == 0 || player.VillaggiPersonali[i - 1].Sconfitto) // Primo villaggio sempre inviato, gli altri solo se il precedente è sconfitto
                    villaggiDaInviare.Add(new
                    {
                        v.Id,
                        v.Nome,
                        v.Livello,
                        Esperienza = v.Esperienza,
                        Esplorato = v.Esplorato,
                        Sconfitto = v.Sconfitto,
                        Diamanti_Viola = v.Diamanti_Viola,
                        Diamanti_Blu = v.Diamanti_Blu,
                        Cibo = v.Cibo,
                        Legno = v.Legno,
                        Pietra = v.Pietra,
                        Ferro = v.Ferro,
                        Oro = v.Oro,
                        Guerrieri = v.Esplorato ? v.Guerrieri : 0,
                        Lancieri = v.Esplorato ? v.Lancieri : 0,
                        Arcieri = v.Esplorato ? v.Arcieri : 0,
                        Catapulte = v.Esplorato ? v.Catapulte : 0
                    });
                else break;
            }

            var villaggiUpdate = new
            {
                Type = "VillaggiPersonali",
                Dati = villaggiDaInviare
            };
            Server.Send(player.guid_Player, JsonSerializer.Serialize(villaggiUpdate));

            // --- Città globali ---
            var cittaDaInviare = new List<object>();
            for (int i = 0; i < Gioco.Barbari.CittaGlobali.Count; i++)
            {
                var c = Gioco.Barbari.CittaGlobali[i];
                if (i == 0 || Gioco.Barbari.CittaGlobali[i - 1].Sconfitto)
                    cittaDaInviare.Add(new
                    {
                        c.Id,
                        c.Nome,
                        c.Livello,
                        Esperienza = c.Esperienza,
                        Esplorato = c.Esplorato,
                        Sconfitto = c.Sconfitto,
                        Diamanti_Viola = c.Diamanti_Viola,
                        Diamanti_Blu = c.Diamanti_Blu,
                        Cibo = c.Cibo,
                        Legno = c.Legno,
                        Pietra = c.Pietra,
                        Ferro = c.Ferro,
                        Oro = c.Oro,
                        Guerrieri = c.Esplorato ? c.Guerrieri : 0,
                        Lancieri = c.Esplorato ? c.Lancieri : 0,
                        Arcieri = c.Esplorato ? c.Arcieri : 0,
                        Catapulte = c.Esplorato ? c.Catapulte : 0
                    });
                else break;
            }

            var cittaUpdate = new
            {
                Type = "CittaGlobali",
                Dati = cittaDaInviare
            };
            Server.Send(player.guid_Player, JsonSerializer.Serialize(cittaUpdate));
        }
        public static void Update_Data_OneTime(Guid guid, Player player)
        {
            string data =
            "Update_Data|" +
            $"D_Viola_D_Blu={Variabili_Server.D_Viola_To_Blu}|" +
            $"Tributi_D_Viola={Variabili_Server.Tributi_To_D_Viola}|" +
            $"Tempo_D_Blu={Variabili_Server.Velocizzazione_Tempo}|" +

            $"QuestMensili_Tempo={player.FormatTime(Variabili_Server.timer_Reset_Quest)}|" +
            $"Barbari_Tempo={player.FormatTime(Variabili_Server.timer_Reset_Barbari)}|" +

            $"punti_quest={player.Punti_Quest}|" +
            $"costo_terreni_Virtuali={Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola}|" +
            $"Tutorial={player.Tutorial}|" +
            $"Giorni_Consecutivi={player.GamePass_Accessi_Consecutivi}|" +

            //Livelli Sblocco
            $"Unlock_Truppe_II={Variabili_Server.truppe_II}|" +
            $"Unlock_Truppe_III={Variabili_Server.truppe_III}|" +
            $"Unlock_Truppe_IV={Variabili_Server.truppe_IV}|" +
            $"Unlock_Truppe_V={Variabili_Server.truppe_V}|" +
            $"Unlock_Città_Barbare={Variabili_Server.citta_Barbare_Unlock}|" +
            $"Unlock_PVP={Variabili_Server.PVP_Unlock}|" +

            //Shop
            $"Pacchetto_Vip_1_Costo={Variabili_Server.Shop.Vip_1.Costo}|" +
            $"Pacchetto_Vip_1_Reward={Variabili_Server.Shop.Vip_1.Reward / 60 / 60}|" +
            $"Pacchetto_Vip_2_Costo={Variabili_Server.Shop.Vip_2.Costo}|" +
            $"Pacchetto_Vip_2_Reward={Variabili_Server.Shop.Vip_2.Reward / 60 / 60}|" +

            $"Pacchetto_Diamanti_1_Costo={Variabili_Server.Shop.Pacchetto_Diamanti_1.Costo}|" +
            $"Pacchetto_Diamanti_1_Reward={Variabili_Server.Shop.Pacchetto_Diamanti_1.Reward}|" +
            $"Pacchetto_Diamanti_2_Costo={Variabili_Server.Shop.Pacchetto_Diamanti_2.Costo}|" +
            $"Pacchetto_Diamanti_2_Reward={Variabili_Server.Shop.Pacchetto_Diamanti_2.Reward}|" +
            $"Pacchetto_Diamanti_3_Costo={Variabili_Server.Shop.Pacchetto_Diamanti_3.Costo}|" +
            $"Pacchetto_Diamanti_3_Reward={Variabili_Server.Shop.Pacchetto_Diamanti_3.Reward}|" +
            $"Pacchetto_Diamanti_4_Costo={Variabili_Server.Shop.Pacchetto_Diamanti_4.Costo}|" +
            $"Pacchetto_Diamanti_4_Reward={Variabili_Server.Shop.Pacchetto_Diamanti_4.Reward}|" +
            $"Pacchetto_Scudo_Pace_8h_Costo={Variabili_Server.Shop.Scudo_Pace_8h.Costo}|" +
            $"Pacchetto_Scudo_Pace_8h_Reward={Variabili_Server.Shop.Scudo_Pace_8h.Reward / 60 / 60}|" +
            $"Pacchetto_Scudo_Pace_24h_Costo={Variabili_Server.Shop.Scudo_Pace_24h.Costo}|" +
            $"Pacchetto_Scudo_Pace_24h_Reward={Variabili_Server.Shop.Scudo_Pace_24h.Reward / 60 / 60}|" +
            $"Pacchetto_Scudo_Pace_72h_Costo={Variabili_Server.Shop.Scudo_Pace_72h.Costo}|" +
            $"Pacchetto_Scudo_Pace_72h_Reward={Variabili_Server.Shop.Scudo_Pace_72h.Reward / 60 / 60}|" +
            $"Pacchetto_Costruttore_24h_Costo={Variabili_Server.Shop.Costruttore_24h.Costo}|" +
            $"Pacchetto_Costruttore_24h_Reward={Variabili_Server.Shop.Costruttore_24h.Reward / 60 / 60}|" +
            $"Pacchetto_Costruttore_48h_Costo={Variabili_Server.Shop.Costruttore_48h.Costo}|" +
            $"Pacchetto_Costruttore_48h_Reward={Variabili_Server.Shop.Costruttore_48h.Reward / 60 / 60}|" +
            $"Pacchetto_Reclutatore_24h_Costo={Variabili_Server.Shop.Reclutatore_24h.Costo}|" +
            $"Pacchetto_Reclutatore_24h_Reward={Variabili_Server.Shop.Reclutatore_24h.Reward / 60 / 60}|" +
            $"Pacchetto_Reclutatore_48h_Costo={Variabili_Server.Shop.Reclutatore_48h.Costo}|" +
            $"Pacchetto_Reclutatore_48h_Reward={Variabili_Server.Shop.Reclutatore_48h.Reward / 60 / 60}|" +

            $"Pacchetto_GamePass_Base_Costo={Variabili_Server.Shop.GamePass_Base.Costo}|" +
            $"Pacchetto_GamePass_Base_Reward={Variabili_Server.Shop.GamePass_Base.Reward}|" +
            $"Pacchetto_GamePass_Avanzato_Costo={Variabili_Server.Shop.GamePass_Avanzato.Costo}|" +
            $"Pacchetto_GamePass_Avanzato_Reward={Variabili_Server.Shop.GamePass_Avanzato.Reward}|" +
            $"Terreno_NonComune_Produzione={Variabili_Server.Terreni_Virtuali.NonComune.Produzione}|" +
            $"Terreno_Comune_Produzione={Variabili_Server.Terreni_Virtuali.Comune.Produzione}|" +
            $"Terreno_Raro_Produzione={Variabili_Server.Terreni_Virtuali.Raro.Produzione}|" +
            $"Terreno_Epico_Produzione={Variabili_Server.Terreni_Virtuali.Epico.Produzione}|" +
            $"Terreno_Leggendario_Produzione={Variabili_Server.Terreni_Virtuali.Leggendario.Produzione}|" +

            $"Feudi_NonComune_Rarita={Variabili_Server.Terreni_Virtuali.NonComune.Rarita}|" +
            $"Feudi_Comune_Rarita={Variabili_Server.Terreni_Virtuali.Comune.Rarita}|" +
            $"Feudi_Raro_Rarita={Variabili_Server.Terreni_Virtuali.Raro.Rarita}|" +
            $"Feudi_Epico_Rarita={Variabili_Server.Terreni_Virtuali.Epico.Rarita}|" +
            $"Feudi_Leggendario_Rarita={Variabili_Server.Terreni_Virtuali.Leggendario.Rarita}";

            Server.Send(player.guid_Player, $"Update_Data|" +
            $"Avatar_Costo_Lord_1={Variabili_Server.Avatar.Lord_1.Costo}|" +
            $"Avatar_Costo_Lord_2={Variabili_Server.Avatar.Lord_2.Costo}|" +
            $"Avatar_Costo_Lord_3={Variabili_Server.Avatar.Lord_3.Costo}|" +
            $"Avatar_Costo_Lord_4={Variabili_Server.Avatar.Lord_4.Costo}|" +
            $"Avatar_Costo_Lord_5={Variabili_Server.Avatar.Lord_5.Costo}|" +
            $"Avatar_Costo_Lord_6={Variabili_Server.Avatar.Lord_6.Costo}|" +
            $"Avatar_Costo_Lord_7={Variabili_Server.Avatar.Lord_7.Costo}|" +
            $"Avatar_Costo_Lady_1={Variabili_Server.Avatar.Lady_1.Costo}|" +
            $"Avatar_Costo_Lady_2={Variabili_Server.Avatar.Lady_2.Costo}|" +
            $"Avatar_Costo_Lady_3={Variabili_Server.Avatar.Lady_3.Costo}|" +
            $"Avatar_Costo_Lady_4={Variabili_Server.Avatar.Lady_4.Costo}|" +
            $"Avatar_Costo_Lady_5={Variabili_Server.Avatar.Lady_5.Costo}|" +
            $"Avatar_Costo_Lady_6={Variabili_Server.Avatar.Lady_6.Costo}|");

            Server.Send(player.guid_Player, $"Update_Data|" +
            $"Avatar_Nome_Lord_1={Variabili_Server.Avatar.Lord_1.Nome}|" +
            $"Avatar_Nome_Lord_2={Variabili_Server.Avatar.Lord_2.Nome}|" +
            $"Avatar_Nome_Lord_3={Variabili_Server.Avatar.Lord_3.Nome}|" +
            $"Avatar_Nome_Lord_4={Variabili_Server.Avatar.Lord_4.Nome}|" +
            $"Avatar_Nome_Lord_5={Variabili_Server.Avatar.Lord_5.Nome}|" +
            $"Avatar_Nome_Lord_6={Variabili_Server.Avatar.Lord_6.Nome}|" +
            $"Avatar_Nome_Lord_7={Variabili_Server.Avatar.Lord_7.Nome}|" +
            $"Avatar_Nome_Lady_1={Variabili_Server.Avatar.Lady_1.Nome}|" +
            $"Avatar_Nome_Lady_2={Variabili_Server.Avatar.Lady_2.Nome}|" +
            $"Avatar_Nome_Lady_3={Variabili_Server.Avatar.Lady_3.Nome}|" +
            $"Avatar_Nome_Lady_4={Variabili_Server.Avatar.Lady_4.Nome}|" +
            $"Avatar_Nome_Lady_5={Variabili_Server.Avatar.Lady_5.Nome}|" +
            $"Avatar_Nome_Lady_6={Variabili_Server.Avatar.Lady_6.Nome}");

            Server.Send(guid, data);

            string payload = JsonConvert.SerializeObject(player.Report);
            Server.Send(guid, $"Update_Data|Report_Lista|{payload}");
            // 16/09/2026: sincronizza il contatore usato da Update_Data per capire
            // se sono arrivati nuovi referti da mandare "in diretta" — altrimenti
            // il primo tick periodico dopo il login rimanderebbe subito lo stesso
            // identico Report_Lista appena inviato qui sopra.
            player.Snapshot.SyncReportCount(player.Report.Count);

            // 16/09/2026, su richiesta dell'utente: la Cronologia (player.Cronologia,
            // popolata da Server.Send ad ogni Log_Server) viene rimandata al login come
            // blocco unico via JSON, stesso schema di Report_Lista qui sopra — non tramite
            // singoli messaggi "Log_Server|..." individuali, che verrebbero ri-registrati
            // in Cronologia da Server.Send creando duplicati ad ogni ricollegamento.
            string cronologiaPayload = JsonConvert.SerializeObject(player.Cronologia);
            Server.Send(guid, $"Update_Data|Cronologia_Lista|{cronologiaPayload}");
        }

        // 16/09/2026, su richiesta dell'utente: elimina un referto dalla lista personale
        // del giocatore (player.Report). L'indice arriva dal client riferito all'ultimo
        // Report_Lista ricevuto — non è un Id persistente sul referto, quindi va sempre
        // validato per intero e per range prima di usarlo (un vecchio indice rimasto nel
        // client dopo che la lista è già cambiata non deve poter mai andare fuori dai
        // limiti o cancellare il referto sbagliato).
        public static void Elimina_Report(Player player, Guid clientGuid, string indiceStr)
        {
            if (!int.TryParse(indiceStr, out int indice) || indice < 0 || indice >= player.Report.Count)
            {
                Server.Send(clientGuid, "Log_Server|[warning]Referto non trovato.");
                return;
            }

            player.Report.RemoveAt(indice);

            // Rimanda subito la lista aggiornata (stesso formato di Update_Data_OneTime/
            // Update_Data) invece di aspettare il prossimo tick del game loop, così il
            // referto sparisce subito dalla UI del giocatore che l'ha eliminato.
            string payloadAggiornato = JsonConvert.SerializeObject(player.Report);
            Server.Send(clientGuid, $"Update_Data|Report_Lista|{payloadAggiornato}");
            player.Snapshot.SyncReportCount(player.Report.Count);
        }

        // 16/09/2026, su richiesta dell'utente: svuota la cronologia messaggi salvata
        // lato server per questo giocatore (player.Cronologia). Azione totale, non a
        // singolo indice: il client cancella la propria vista locale in ottimistico e
        // il server è la fonte di verità che verrà persistita/ricaricata al prossimo
        // salvataggio (vedi GameSave.cs, "_Cronologia.json").
        public static void Elimina_Cronologia(Player player, Guid clientGuid)
        {
            player.Cronologia.Clear();

            string payloadAggiornato = JsonConvert.SerializeObject(player.Cronologia);
            Server.Send(clientGuid, $"Update_Data|Cronologia_Lista|{payloadAggiornato}");
        }

        public static void Update_Data(Guid guid, Player player)
        {
            if (!Server.Client_Connessi.Contains(player.guid_Player)) return; //Se non è presente nella lista connesso...

            var current = player.Snapshot.BuildCurrentState(player);
            var delta = player.Snapshot.BuildDelta(current);
            if (delta != null) Server.Send(guid, delta);

            // 16/09/2026, su richiesta dell'utente: i referti (player.Report) vengono
            // rilevati e inviati "in diretta" automaticamente ad ogni tick, invece di
            // richiedere che ogni punto del codice che ne crea uno (battaglie PVP/PVE,
            // spionaggio, e in futuro altri) se ne ricordi da solo — prima, ad esempio,
            // lo spionaggio non lo faceva e il referto arrivava al client solo al
            // prossimo login. Non passa dal delta sopra: vedi il commento su
            // _lastReportCount in PlayerSnapshot.cs per il perché (il carattere "|"
            // nel JSON corromperebbe il protocollo). Stesso formato già usato da
            // Update_Data_OneTime al login.
            if (player.Snapshot.ReportCountChanged(player.Report.Count))
            {
                string reportPayload = JsonConvert.SerializeObject(player.Report);
                Server.Send(guid, $"Update_Data|Report_Lista|{reportPayload}");
            }

            if (player.Livello >= Variabili_Server.PVP_Unlock)
            {
                string raduno = $"Raduno|";
                string raduno_Player = $"Raduni_Player|";
                if (AttacchiCooperativi.AttacchiInCorso.Keys.Count() > 0)
                    foreach (string idAttacco in AttacchiCooperativi.AttacchiInCorso.Keys)
                    {
                        var attacco = AttacchiCooperativi.AttacchiInCorso[idAttacco];
                        raduno += $"{attacco.CreatoreUsername}|{idAttacco}|{attacco.TempoRimanente / 60}-";
                    }
                else raduno = "";

                if (AttacchiCooperativi.AttacchiInPlayer.Keys.Count() > 0)
                    foreach (string idAttacco in AttacchiCooperativi.AttacchiInPlayer.Keys)
                    {
                        var attacco = AttacchiCooperativi.AttacchiInPlayer[idAttacco];
                        var user = attacco.GiocatoriPartecipanti.Keys;
                        var users = attacco.GiocatoriPartecipanti.Values;

                        foreach (var item in attacco.GiocatoriPartecipanti.Keys)
                        {
                            if (player.Username == item)
                                foreach (var items in attacco.GiocatoriPartecipanti.Values)
                                    if (items.Player == player.Username)
                                        // BUGFIX (2026-09-14): Raduni.cs ora supporta truppe su tutti e 5 i tier, non solo il tier 1 —
                                        // qui si invia la somma di tutti i tier per non "perdere" dalla vista client le truppe di livello > 1.
                                        raduno_Player += $"{item}|{idAttacco}|{attacco.TempoRimanente / 60}|{items.Guerrieri.Sum()}|{items.Lanceri.Sum()}|{items.Arceri.Sum()}|{items.Catapulte.Sum()}-";
                        }
                    }
                else raduno_Player = "";
                if (raduno != "") Server.Send(guid, raduno); //Invia i raduni aperti
                if (raduno_Player != "") Server.Send(guid, raduno_Player); //Invia i raduni aperti
            }

            // Filtra giocatori con potenza simile (ad esempio ±20%)
            if (player.Livello >= Variabili_Server.PVP_Unlock)
            {
                int potenzaPlayer = (int)player.Potenza_Totale;
                int maxPlayers = 20; // Numero massimo di giocatori da inviare
                var giocatoriFiltrati = Server.Utenti_PVP
                .Where(username => {
                    var p = player.Potenza_Totale;
                    return p >= potenzaPlayer * 0.8 && p <= potenzaPlayer * 1.2;
                })
                .Take(maxPlayers) // prendi al massimo 50
                .Select(username => costruisci_stringa(username)) // costruisci la stringa
                .ToList();
                string stringa_finale = string.Join("|", giocatoriFiltrati);

                if (!string.IsNullOrEmpty(stringa_finale))
                    Server.Send(guid, $"Update_PVP_Player|{giocatoriFiltrati.Count}|{stringa_finale}");
            }

            //var stringhePVP = Server.Utenti_PVP
            //.Select(username => costruisci_stringa(username))
            //.ToList();
            //
            //string stringa_finale = string.Join("|", stringhePVP);
            //if (!string.IsNullOrEmpty(stringa_finale))
            //    Server.Send(guid, $"Update_PVP_Player|{Server.Utenti_PVP.Count}|{stringa_finale}");
        }
        static string costruisci_stringa(string username)
        {
            return username;
        }
    }
}
