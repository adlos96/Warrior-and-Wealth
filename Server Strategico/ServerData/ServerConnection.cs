using Server_Strategico.Gioco;
using Server_Strategico.Manager;
using Server_Strategico.ServerData.Moduli;
using System.Text;
using System.Text.Json;
using WatsonTcp;
using static BattaglieV2;
using static Server_Strategico.Gioco.Barbari;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Manager.QuestManager;
using static Server_Strategico.Gioco.Strutture;

namespace Server_Strategico.Server
{
    internal class ServerConnection
    {
        public static async void HandleClientRequest(MessageReceivedEventArgs requestData)
        {
            var client = requestData.Client.ToString();
            var clientGuid = requestData.Client.Guid;
            var messaggioRicevuto = Encoding.UTF8.GetString(requestData.Data);

            //Variabili.server_Log.Add()
           Console.WriteLine("             ** Comunicazione Client **  ");
           Console.WriteLine("-----------------------------", "standard");
           Console.WriteLine($"[ServerConnection|ClientRequest] > Client:      {client}");
           Console.WriteLine($"[ServerConnection|ClientRequest] > Guid:        [{clientGuid}]");
           Console.WriteLine($"[ServerConnection|ClientRequest] > Messaggio:   [{messaggioRicevuto}]");

            if (!messaggioRicevuto.Contains("|"))
            {
                Console.WriteLine($"[Errore|ServerConnection] >> Messaggio ricevuto: {messaggioRicevuto}");
                return;
            }
            var msgArgs = messaggioRicevuto.Split('|'); // Composto da 3 part1 0|1|2 -> 0 = percorso file
            if (msgArgs.Length == 0)
            {
                Console.WriteLine("[Errore|ServerConnection] >> needed 1 args");
                return;
            }
            var player = Server.servers_.GetPlayer(msgArgs[1], msgArgs[2]);
            var dati = Server.servers_.players;
            switch (msgArgs[0])
            {
                case "New Player":
                    Console.WriteLine($"[Server] Richiesta nuovo utente ID: {clientGuid}");
                    if (await New_Player(msgArgs[1], msgArgs[2], clientGuid))
                    {
                        player = Server.servers_.GetPlayer(msgArgs[1], msgArgs[2]);
                        Server.Send(clientGuid, "Login|true");

                        Server.Client_Connessi_Map.TryRemove(clientGuid, out _);
                        Server.Client_Connessi_Map.TryAdd(clientGuid, player.Username);

                        Descrizioni.DescUpdate(player);
                        QuestManager.QuestUpdate(player);
                        QuestManager.QuestRewardUpdate(player);
                        AggiornaVillaggiClient(player);
                        Server.servers_.AggiornaListaPVP();
                        Tutorial(player);
                        player.SetupCaserme();
                    }
                    else
                        Server.Send(clientGuid, $"Login|false|Questo nome utente è già presente: [{msgArgs[1]}]");
                    break;
                case "Login":
                    if (await Login(msgArgs[1], msgArgs[2], clientGuid))
                    {
                        Server.Send(clientGuid, "Login|true");
                        Descrizioni.DescUpdate(player);
                        QuestManager.QuestUpdate(player);
                        QuestManager.QuestRewardUpdate(player);
                        AggiornaVillaggiClient(player);
                        Server.servers_.AggiornaListaPVP();
                        Tutorial(player);
                        player.SetupCaserme();
                        if (player.Stato_Giocatore == false) player.Stato_Giocatore = true;
                    }
                    else
                        Server.Send(clientGuid, $"Login|false|Username o password non corrispondono. User: [{msgArgs[1]}] psw: [{msgArgs[2]}]");
                    break;
                case "Costruzione":
                    if (Convert.ToInt32(msgArgs[3]) > 0) BuildingManagerV2.Costruzione("Fattoria", Convert.ToInt32(msgArgs[3]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[4]) > 0) BuildingManagerV2.Costruzione("Segheria", Convert.ToInt32(msgArgs[4]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[5]) > 0) BuildingManagerV2.Costruzione("CavaPietra", Convert.ToInt32(msgArgs[5]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[6]) > 0) BuildingManagerV2.Costruzione("MinieraFerro", Convert.ToInt32(msgArgs[6]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[7]) > 0) BuildingManagerV2.Costruzione("MinieraOro", Convert.ToInt32(msgArgs[7]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[8]) > 0) BuildingManagerV2.Costruzione("Case", Convert.ToInt32(msgArgs[8]), clientGuid, player); // Costruisci fattorie
                    
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
                    Esplora(player, Convert.ToInt32(msgArgs[4]), msgArgs[3]);
                    break;
                case "Battaglia":
                    int[] guerrieri = new int[] { 0, 0, 0, 0, 0 };
                    int[] picchieri = new int[] { 0, 0, 0, 0, 0 };
                    int[] arcieri = new int[] { 0, 0, 0, 0, 0 };
                    int[] catapulte = new int[] { 0, 0, 0, 0, 0 };

                    guerrieri[0] = Convert.ToInt32(msgArgs[5]);
                    guerrieri[1] = Convert.ToInt32(msgArgs[6]);
                    guerrieri[2] = Convert.ToInt32(msgArgs[7]);
                    guerrieri[3] = Convert.ToInt32(msgArgs[8]);
                    guerrieri[4] = Convert.ToInt32(msgArgs[9]);

                    picchieri[0] = Convert.ToInt32(msgArgs[10]);
                    picchieri[1] = Convert.ToInt32(msgArgs[11]);
                    picchieri[2] = Convert.ToInt32(msgArgs[12]);
                    picchieri[3] = Convert.ToInt32(msgArgs[13]);
                    picchieri[4] = Convert.ToInt32(msgArgs[14]);

                    arcieri[0] = Convert.ToInt32(msgArgs[15]);
                    arcieri[1] = Convert.ToInt32(msgArgs[16]);
                    arcieri[2] = Convert.ToInt32(msgArgs[17]);
                    arcieri[3] = Convert.ToInt32(msgArgs[18]);
                    arcieri[4] = Convert.ToInt32(msgArgs[19]);

                    catapulte[0] = Convert.ToInt32(msgArgs[20]);
                    catapulte[1] = Convert.ToInt32(msgArgs[21]);
                    catapulte[2] = Convert.ToInt32(msgArgs[22]);
                    catapulte[3] = Convert.ToInt32(msgArgs[23]);
                    catapulte[4] = Convert.ToInt32(msgArgs[24]);

                    if (msgArgs[3] == "Villaggio Barbaro")
                        await BattaglieV2.Battaglia_Barbari(player, clientGuid, "Villaggio Barbaro", msgArgs[4], guerrieri, picchieri, arcieri, catapulte);
                    if (msgArgs[3] == "Città Barbaro")
                        await BattaglieV2.Battaglia_Barbari(player, clientGuid, "Città Barbaro", msgArgs[4], guerrieri, picchieri, arcieri, catapulte);

                    AggiornaVillaggiClient(player);
                    if (msgArgs[3] == "PVP")
                    {
                        var datisss = msgArgs[4].Split(',');
                        var difensore = Server.servers_.GetPlayer_Data(datisss[0]);
                        var attackerUnits = new UnitGroup
                        {
                            Guerrieri = guerrieri,
                            Lancieri = picchieri,
                            Arcieri = arcieri,
                            Catapulte = catapulte
                        };
                        BattaglieV2.BattleResult result = await BattaglieV2.Battaglia_Strutture_PvP(player, difensore, clientGuid, difensore.guid_Player, attackerUnits);
                        if (result.Struttura == "Castello" && result.Victory == true) 
                            BattaglieV2.Battaglia_PvP(player, difensore, clientGuid, difensore.guid_Player, result.AttaccantePerdite.Guerrieri, result.AttaccantePerdite.Lancieri, result.AttaccantePerdite.Arcieri, result.AttaccantePerdite.Catapulte);

                        Server.servers_.AggiornaListaPVP();
                    }
                    
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
                case "Shop":
                    Shop.Shop_Call(clientGuid, player, msgArgs[3]); //Shop
                    break;
                case "SpostamentoTruppe":
                    SpostamentoTruppe(clientGuid, player, msgArgs); //sposta le troppe tra il "giocatore" e la città/cancello/ingresso
                    break;
                case "Ripara":
                    Set_Riparazioni(player, msgArgs);
                    break;
                case "Ripara Tutto":
                    Set_Riparazioni(player, msgArgs);
                    break;
                case "Tutorial Update":
                    TutorialUpdate(player, msgArgs);
                    break;

                default: Console.WriteLine($"Messaggio: [{msgArgs}]"); break;
            }
           
        }
        public static void TutorialUpdate(Player player, string[] Dati)
        {
            if (Dati[3] == "1") { player.Tutorial_Stato[0] = true; player.Tutorial_Premi[0] = true;
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

            }
            else if (Dati[3] == "2" && !player.Tutorial_Stato[1]) { player.Tutorial_Stato[1] = true; player.Tutorial_Premi[1] = true; }
            else if (Dati[3] == "3" && !player.Tutorial_Stato[2]) { player.Tutorial_Stato[2] = true; player.Tutorial_Premi[2] = true; }
            else if (Dati[3] == "4" && !player.Tutorial_Stato[3]) { player.Tutorial_Stato[3] = true; player.Tutorial_Premi[3] = true; }
            else if (Dati[3] == "5" && !player.Tutorial_Stato[4]) { player.Tutorial_Stato[4] = true; player.Tutorial_Premi[4] = true; }
            else if (Dati[3] == "6" && !player.Tutorial_Stato[5]) { player.Tutorial_Stato[5] = true; player.Tutorial_Premi[5] = true; }
            else if (Dati[3] == "7" && !player.Tutorial_Stato[6]) { player.Tutorial_Stato[6] = true; player.Tutorial_Premi[6] = true; player.Diamanti_Viola = 150; }
            else if (Dati[3] == "8" && !player.Tutorial_Stato[7]) { player.Tutorial_Stato[7] = true; player.Tutorial_Premi[7] = true; }
            else if (Dati[3] == "9" && !player.Tutorial_Stato[8]) { player.Tutorial_Stato[8] = true; player.Tutorial_Premi[8] = true; }
            else if (Dati[3] == "10" && !player.Tutorial_Stato[9]) { player.Tutorial_Stato[9] = true; player.Tutorial_Premi[9] = true; }

            else if (Dati[3] == "11" && !player.Tutorial_Stato[10]) 
            { 
                player.Tutorial_Stato[10] = true;
                if (!player.Tutorial_Premi[10])
                {
                    player.Cibo = Strutture.Edifici.Fattoria.Cibo;
                    player.Legno = Strutture.Edifici.Fattoria.Legno;
                    player.Pietra = Strutture.Edifici.Fattoria.Pietra;
                    player.Ferro = Strutture.Edifici.Fattoria.Ferro;
                    player.Oro = Strutture.Edifici.Fattoria.Oro;
                    player.Popolazione = Strutture.Edifici.Fattoria.Popolazione;
                    player.Tutorial_Premi[10] = true;
                }
            }
            else if (Dati[3] == "12" && !player.Tutorial_Stato[11])
            {
                player.Tutorial_Stato[11] = true;
                player.Tutorial_Premi[11] = true;
                player.Diamanti_Viola = 20;
                player.Diamanti_Blu = 0;
                player.Punti_Quest = 0;
            }
            else if (Dati[3] == "13" && !player.Tutorial_Stato[12]) 
            {   
                player.Tutorial_Stato[12] = true; 
                player.Tutorial_Premi[12] = true;
            }
            else if (Dati[3] == "14" && !player.Tutorial_Stato[13]) 
            { 
                player.Tutorial_Stato[13] = true;
                if (!player.Tutorial_Premi[13])
                {
                    player.Cibo = Strutture.Edifici.Segheria.Cibo;
                    player.Legno = Strutture.Edifici.Segheria.Legno;
                    player.Pietra = Strutture.Edifici.Segheria.Pietra;
                    player.Ferro = Strutture.Edifici.Segheria.Ferro;
                    player.Oro = Strutture.Edifici.Segheria.Oro;
                    player.Popolazione = Strutture.Edifici.Segheria.Popolazione;
                    player.Diamanti_Viola = 27;
                    player.Diamanti_Blu = 10;
                    player.Tutorial_Premi[13] = true;
                }
            }
            else if (Dati[3] == "15" && !player.Tutorial_Stato[14])
            {
                player.Tutorial_Stato[14] = true;
                if (!player.Tutorial_Premi[14])
                {
                    player.Cibo = Strutture.Edifici.CavaPietra.Cibo;
                    player.Legno = Strutture.Edifici.CavaPietra.Legno;
                    player.Pietra = Strutture.Edifici.CavaPietra.Pietra;
                    player.Ferro = Strutture.Edifici.CavaPietra.Ferro;
                    player.Oro = Strutture.Edifici.CavaPietra.Oro;
                    player.Popolazione = Strutture.Edifici.CavaPietra.Popolazione;
                    player.Diamanti_Viola = 35;
                    player.Diamanti_Blu = 15;
                    player.Tutorial_Premi[14] = true;
                }
            }
            else if (Dati[3] == "16" && !player.Tutorial_Stato[15]) 
            {
                player.Tutorial_Stato[15] = true;
                if (!player.Tutorial_Premi[15])
                {
                    player.Cibo = Strutture.Edifici.MinieraFerro.Cibo;
                    player.Legno = Strutture.Edifici.MinieraFerro.Legno;
                    player.Pietra = Strutture.Edifici.MinieraFerro.Pietra;
                    player.Ferro = Strutture.Edifici.MinieraFerro.Ferro;
                    player.Oro = Strutture.Edifici.MinieraFerro.Oro;
                    player.Popolazione = Strutture.Edifici.MinieraFerro.Popolazione;
                    player.Diamanti_Viola = 35;
                    player.Diamanti_Blu = 15;
                    player.Tutorial_Premi[15] = true;
                }
            }
            else if (Dati[3] == "17" && !player.Tutorial_Stato[16]) 
            {
                player.Tutorial_Stato[16] = true;
                if (!player.Tutorial_Premi[16])
                {
                    player.Cibo = Strutture.Edifici.MinieraOro.Cibo;
                    player.Legno = Strutture.Edifici.MinieraOro.Legno;
                    player.Pietra = Strutture.Edifici.MinieraOro.Pietra;
                    player.Ferro = Strutture.Edifici.MinieraOro.Ferro;
                    player.Oro = Strutture.Edifici.MinieraOro.Oro;
                    player.Popolazione = Strutture.Edifici.MinieraOro.Popolazione;
                    player.Diamanti_Viola = 35;
                    player.Diamanti_Blu = 20;
                    player.Tutorial_Premi[16] = true;
                }
            }
            else if (Dati[3] == "18" && !player.Tutorial_Stato[17]) 
            {
                player.Tutorial_Stato[17] = true;
                if (!player.Tutorial_Premi[17])
                {
                    player.Cibo = Strutture.Edifici.Case.Cibo;
                    player.Legno = Strutture.Edifici.Case.Legno;
                    player.Pietra = Strutture.Edifici.Case.Pietra;
                    player.Ferro = Strutture.Edifici.Case.Ferro;
                    player.Oro = Strutture.Edifici.Case.Oro;
                    player.Popolazione = Strutture.Edifici.Case.Popolazione;
                    player.Diamanti_Viola = 40;
                    player.Diamanti_Blu = 20;
                    player.Tutorial_Premi[17] = true;
                }
            }
            else if (Dati[3] == "19" && !player.Tutorial_Stato[18])
            {
                player.Tutorial_Stato[18] = true;
                player.Tutorial_Premi[18] = true;
            }
            else if (Dati[3] == "20" && !player.Tutorial_Stato[19]) { player.Tutorial_Stato[19] = true; player.Tutorial_Premi[19] = true; }
            else if (Dati[3] == "21" && !player.Tutorial_Stato[20]) { player.Tutorial_Stato[20] = true; player.Tutorial_Premi[20] = true; }
            else if (Dati[3] == "22" && !player.Tutorial_Stato[21]) { player.Tutorial_Stato[21] = true; player.Tutorial_Premi[21] = true; }
            else if (Dati[3] == "23" && !player.Tutorial_Stato[22]) { player.Tutorial_Stato[22] = true; player.Tutorial_Premi[22] = true; }
            else if (Dati[3] == "24" && !player.Tutorial_Stato[23]) 
            { 
                player.Tutorial_Stato[23] = true; 
                
                if (!player.Tutorial_Premi[23])
                {
                    player.Tutorial_Premi[23] = true;
                    player.Salute_Mura = player.Salute_MuraMax;
                    player.Salute_Mura -= 5;
                    player.Difesa_Mura = player.Difesa_MuraMax;
                    player.Difesa_Mura -= 5;
                    player.Cibo += Riparazione.Mura.Consumo_Cibo * 10;
                    player.Legno += Riparazione.Mura.Consumo_Legno * 10;
                    player.Pietra += Riparazione.Mura.Consumo_Pietra * 10;
                    player.Ferro += Riparazione.Mura.Consumo_Ferro * 10;
                    player.Oro += Riparazione.Mura.Consumo_Oro * 10;
                }
            }
            else if (Dati[3] == "25" && !player.Tutorial_Stato[24]) { player.Tutorial_Stato[24] = true; player.Tutorial_Premi[24] = true; }
            else if (Dati[3] == "26" && !player.Tutorial_Stato[25]) { player.Tutorial_Stato[25] = true; player.Tutorial_Premi[25] = true; }
            else if (Dati[3] == "27" && !player.Tutorial_Stato[26]) { player.Tutorial_Stato[26] = true; player.Tutorial_Premi[26] = true; }
            else if (Dati[3] == "28" && !player.Tutorial_Stato[27]) { player.Tutorial_Stato[27] = true; player.Tutorial_Premi[27] = true; }
            else if (Dati[3] == "29" && !player.Tutorial_Stato[28]) { player.Tutorial_Stato[28] = true; player.Tutorial_Premi[28] = true; }
            else if (Dati[3] == "30" && !player.Tutorial_Stato[29]) { player.Tutorial_Stato[29] = true; player.Tutorial_Premi[29] = true; }
            else if (Dati[3] == "31" && !player.Tutorial_Stato[30]) { player.Tutorial_Stato[30] = true; player.Tutorial_Premi[30] = true; }
            else if (Dati[3] == "32" && !player.Tutorial_Stato[31]) 
            { 
                player.Tutorial_Stato[31] = true;
                player.Tutorial_Premi[31] = true;
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
        public static void SpostamentoTruppe(Guid guid, Player player, string[] dati)
        {
            string edificio_From = dati[3];
            string edificio_To = dati[4];
            int g = Convert.ToInt32(dati[5]);
            int l = Convert.ToInt32(dati[6]);
            int a = Convert.ToInt32(dati[7]);
            int c = Convert.ToInt32(dati[8]);
            int livello = Convert.ToInt32(dati[9]) - 1;
            int cas_G_Max = 0, cas_L_Max = 0, cas_A_Max = 0, cas_C_Max = 0;

            if (g + l + a + c == 0) return;

            cas_G_Max = player.Caserma_Guerrieri * Edifici.CasermaGuerrieri.Limite;
            cas_L_Max = player.Caserma_Lancieri * Edifici.CasermaLanceri.Limite;
            cas_A_Max = player.Caserma_Arceri * Edifici.CasermaArceri.Limite;
            cas_C_Max = player.Caserma_Catapulte * Edifici.CasermaCatapulte.Limite;

            if (edificio_From == "Esercito Villaggio") //Aggiunge o rimuove le unita dal giocatore.
            {
                if (player.Guerrieri[livello] >= g) player.Guerrieri[livello] -= g;
                if (player.Lanceri[livello] >= l) player.Lanceri[livello] -= l;
                if (player.Arceri[livello] >= a) player.Arceri[livello] -= a;
                if (player.Catapulte[livello] >= c) player.Catapulte[livello] -= c;
            }else
            {
                player.Guerrieri[livello] += g;
                player.Lanceri[livello] += l;
                player.Arceri[livello] += a;
                player.Catapulte[livello] += c;
            }

            //Aggiunge o rimuove le unità dalle strutture del giocatore
            if (edificio_From == "Esercito Villaggio" && edificio_To == "Ingresso")
            {
                if (g + l + a + c > player.Guarnigione_IngressoMax)
                {
                    player.Guerrieri_Citta[livello] += g;
                    player.Lanceri_Citta[livello] += l;
                    player.Arceri_Citta[livello] += a;
                    player.Catapulte_Citta[livello] += c;
                    return;
                }

                player.Guerrieri_Ingresso[livello] += g;
                player.Lanceri_Ingresso[livello] += l;
                player.Arceri_Ingresso[livello] += a;
                player.Catapulte_Ingresso[livello] += c;
                return;
            }
            if (edificio_From == "Ingresso" && edificio_To == "Esercito Villaggio")
            {
                if (player.Guerrieri_Ingresso[livello] >= g) player.Guerrieri_Ingresso[livello] -= g;
                if (player.Lanceri_Ingresso[livello] >= l)   player.Lanceri_Ingresso[livello] -= l;
                if (player.Arceri_Ingresso[livello] >= a)    player.Arceri_Ingresso[livello] -= a;
                if (player.Catapulte_Ingresso[livello] >= c) player.Catapulte_Ingresso[livello] -= c;
                return;
            }
            if (edificio_From == "Esercito Villaggio" && edificio_To == "Citta")
            {
                if (g + l + a + c > player.Guarnigione_IngressoMax) //Se maggiore annulla e ripristina le truppe spostate.
                {
                    player.Guerrieri_Citta[livello] += g;
                    player.Lanceri_Citta[livello] += l;
                    player.Arceri_Citta[livello] += a;
                    player.Catapulte_Citta[livello] += c;
                    return;
                }

                if (player.Guerrieri[livello] >= g) player.Guerrieri_Citta[livello] += g;
                if (player.Lanceri[livello] >= l)   player.Lanceri_Citta[livello] += l;
                if (player.Arceri[livello] >= a)    player.Arceri_Citta[livello] += a;
                if (player.Catapulte[livello] >= c) player.Catapulte_Citta[livello] += c;
                return;
            }
            if (edificio_From == "Citta" && edificio_To == "Esercito Villaggio")
            {
                if (player.Guerrieri_Citta[livello] >= g) player.Guerrieri_Citta[livello] -= g;
                if (player.Lanceri_Citta[livello] >= l)   player.Lanceri_Citta[livello] -= l;
                if (player.Arceri_Citta[livello] >= a)    player.Arceri_Citta[livello] -= a;
                if (player.Catapulte_Citta[livello] >= c) player.Catapulte_Citta[livello] -= c;
                return;
            }
            if (edificio_From == "Esercito Villaggio" && edificio_To == "Cancello")
            {
                if (g + l + a + c > player.Guarnigione_CancelloMax)
                {
                    player.Guerrieri_Citta[livello] += g;
                    player.Lanceri_Citta[livello] += l;
                    player.Arceri_Citta[livello] += a;
                    player.Catapulte_Citta[livello] += c;
                    return;
                }

                if (player.Guerrieri[livello] >= g) player.Guerrieri_Cancello[livello] += g;
                if (player.Lanceri[livello] >= l)   player.Lanceri_Cancello[livello] += l;
                if (player.Arceri[livello] >= a)    player.Arceri_Cancello[livello] += a;
                if (player.Catapulte[livello] >= c) player.Catapulte_Cancello[livello] += c;
                return;
            }
            if (edificio_From == "Cancello" && edificio_To == "Esercito Villaggio")
            {
                if (player.Guerrieri_Cancello[livello] >= g) player.Guerrieri_Cancello[livello] -= g;
                if (player.Lanceri_Cancello[livello] >= l)   player.Lanceri_Cancello[livello] -= l;
                if (player.Arceri_Cancello[livello] >= a)    player.Arceri_Cancello[livello] -= a;
                if (player.Catapulte_Cancello[livello] >= c) player.Catapulte_Cancello[livello] -= c;
                return;
            }
            if (edificio_From == "Esercito Villaggio" && edificio_To == "Mura")
            {
                if (g + l + a + c > player.Guarnigione_MuraMax)
                {
                    player.Guerrieri_Citta[livello] += g;
                    player.Lanceri_Citta[livello] += l;
                    player.Arceri_Citta[livello] += a;
                    player.Catapulte_Citta[livello] += c;
                    return;
                }

                if (player.Guerrieri[livello] >= g) player.Guerrieri_Mura[livello] += g;
                if (player.Lanceri[livello] >= l)   player.Lanceri_Mura[livello] += l;
                if (player.Arceri[livello] >= a)    player.Arceri_Mura[livello] += a;
                if (player.Catapulte[livello] >= c) player.Catapulte_Mura[livello] += c;
                return;
            }
            if (edificio_From == "Mura" && edificio_To == "Esercito Villaggio")
            {
                if (player.Guerrieri_Mura[livello] >= g) player.Guerrieri_Mura[livello] -= g;
                if (player.Lanceri_Mura[livello] >= l)   player.Lanceri_Mura[livello] -= l;
                if (player.Arceri_Mura[livello] >= a)    player.Arceri_Mura[livello] -= a;
                if (player.Catapulte_Mura[livello] >= c) player.Catapulte_Mura[livello] -= c;
                return;
            }
            if (edificio_From == "Esercito Villaggio" &&  edificio_To == "Torri")
            {
                if (g + l + a + c > player.Guarnigione_TorriMax)
                {
                    player.Guerrieri_Citta[livello] += g;
                    player.Lanceri_Citta[livello] += l;
                    player.Arceri_Citta[livello] += a;
                    player.Catapulte_Citta[livello] += c;
                    return;
                }

                if (player.Guerrieri[livello] >= g) player.Guerrieri_Torri[livello] += g;
                if (player.Lanceri[livello] >= l)   player.Lanceri_Torri[livello] += l;
                if (player.Arceri[livello] >= a)    player.Arceri_Torri[livello] += a;
                if (player.Catapulte[livello] >= c) player.Catapulte_Torri[livello] += c;
                return;
            }
            if (edificio_From == "Torri" && edificio_To == "Esercito Villaggio")
            {
                if (player.Guerrieri_Torri[livello] >= g) player.Guerrieri_Torri[livello] -= g;
                if (player.Lanceri_Torri[livello] >= l)   player.Lanceri_Torri[livello] -= l;
                if (player.Arceri_Torri[livello] >= a)    player.Arceri_Torri[livello] -= a;
                if (player.Catapulte_Torri[livello] >= c) player.Catapulte_Torri[livello] -= c;
                return;
            }
            if (edificio_From == "Esercito Villaggio" &&  edificio_To == "Castello")
            {
                if (g + l + a + c > player.Guarnigione_CastelloMax)
                {
                    player.Guerrieri_Citta[livello] += g;
                    player.Lanceri_Citta[livello] += l;
                    player.Arceri_Citta[livello] += a;
                    player.Catapulte_Citta[livello] += c;
                    return;
                }

                if (player.Guerrieri[livello] >= g) player.Guerrieri_Castello[livello] += g;
                if (player.Lanceri[livello] >= l)   player.Lanceri_Castello[livello] += l;
                if (player.Arceri[livello] >= a)    player.Arceri_Castello[livello] += a;
                if (player.Catapulte[livello] >= c) player.Catapulte_Castello[livello] += c;
                return;
            }
            if (edificio_From == "Esercito Villaggio" &&  edificio_To == "Esercito Villaggio")
            {
                if (player.Guerrieri_Castello[livello] >= g) player.Guerrieri_Castello[livello] -= g;
                if (player.Lanceri_Castello[livello] >= l)   player.Lanceri_Castello[livello] -= l;
                if (player.Arceri_Castello[livello] >= a)    player.Arceri_Castello[livello] -= a;
                if (player.Catapulte_Castello[livello] >= c) player.Catapulte_Castello[livello] -= c;
                return;
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
                            return;
                        }else
                        {
                            player.PremiNormali[reward] = true;
                            player.Diamanti_Viola += QuestManager.QuestRewardSet.Normali_Monthly.Rewards[reward];
                        }
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
                    }
                    break;

            }
            QuestManager.QuestRewardUpdate(player);
        }
        static async Task<bool> New_Player(string username, string password, Guid guid)
        {
            var existingPlayer = Server.servers_.GetPlayer(username, password);
            if (existingPlayer != null) // Controlla se il giocatore esiste già
            {
                existingPlayer.guid_Player = guid; //Assegna il guid aggiornato
                Console.WriteLine("New Player: Il giocatore già esiste");
                return false;
            }
            if (await Server.servers_.Check_Username_Player(username)) // Controlla se il nome utente è disponibile
            {
                await Server.servers_.AddPlayer(username, password, guid);
                //await GameSave.LoadPlayer(username, password);
                return true;
            }
            return false;
        }
        static async Task<bool> Login(string username, string password, Guid guid)
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
        public static async Task<bool> Load_User_Auto(string username, string password)
        {
            var existingPlayer = Server.servers_.GetPlayer_Data(username);
            if (existingPlayer != null) // Controlla se il giocatore esiste già
            {
                Console.WriteLine("Login: Il giocatore già esiste");
                return true;
            }

            // Controlla se il nome utente è disponibile
            if (await Server.servers_.Check_Username_Player(username))
            {
                await Server.servers_.AddPlayer(username, password, Guid.Empty); // Prima crea il nuovo giocatore
                if (await GameSave.LoadPlayer(username, password)) // Poi prova a caricare i dati salvati
                    return true;
            }
            return true;
        }
        public static void Tutorial(Player player)
        {
            List<Tutorial.dati> tutorial = new List<Tutorial.dati>
            {
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Introduzione_1.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Introduzione_1.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Introduzione_1.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Introduzione_2.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Introduzione_2.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Introduzione_2.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Risorse.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Risorse.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Risorse.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Diamanti_Viola.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Diamanti_Viola.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Diamanti_Viola.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Diamanti_Blu.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Diamanti_Blu.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Diamanti_Blu.Descrizione },
                
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.TributiFeudo.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.TributiFeudo.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.TributiFeudo.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Feudi.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Feudi.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Feudi.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.AcquistaFeudo.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.AcquistaFeudo.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.AcquistaFeudo.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Costruzione_1.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Costruzione_1.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Costruzione_1.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Civile_Militare.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Civile_Militare.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Civile_Militare.Descrizione },

                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Costruzione_2.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Costruzione_2.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Costruzione_2.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Costruisci_Fattoria.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Costruisci_Fattoria.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Costruisci_Fattoria.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Scambia.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Scambia.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Scambia.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Velocizza.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Velocizza.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Velocizza.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Costruisci_Segheria.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Costruisci_Segheria.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Costruisci_Segheria.Descrizione },

                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Costruisci_Cava.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Costruisci_Cava.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Costruisci_Cava.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Costruisci_Miniera_Ferro.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Costruisci_Miniera_Ferro.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Costruisci_Miniera_Ferro.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Costruisci_Miniera_Oro.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Costruisci_Miniera_Oro.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Costruisci_Miniera_Oro.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Costruisci_Abitazioni.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Costruisci_Abitazioni.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Costruisci_Abitazioni.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Strutture_Militari.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Strutture_Militari.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Strutture_Militari.Descrizione },

                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Unita_Militari.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Unita_Militari.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Unita_Militari.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Caserme.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Caserme.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Caserme.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Addestramento.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Addestramento.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Addestramento.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Citta.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Citta.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Citta.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Riparazione.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Riparazione.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Riparazione.Descrizione },

                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Guranigione.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Guranigione.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Guranigione.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Statistiche.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Statistiche.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Statistiche.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Shop.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Shop.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Shop.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Ricerca.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Ricerca.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Ricerca.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Quest_Mensili.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Quest_Mensili.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Quest_Mensili.Descrizione },

                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Battaglia.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Battaglia.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Battaglia.Descrizione },
                new Tutorial.dati { StatoTutorial = ServerData.Moduli.Tutorial.Parti.Finale.StatoTutorial, Obiettivo = ServerData.Moduli.Tutorial.Parti.Finale.Obiettivo, Descrizione = ServerData.Moduli.Tutorial.Parti.Finale.Descrizione },
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
        public static void Update_Data(Guid guid, Player player)
        {
            var buildingsQueue = BuildingManagerV2.GetQueuedBuildings(player);
            var unitsQueue = UnitManagerV2.GetQueuedUnits(player);

            double Cibo = 0, Oro = 0;
            double Cibo_Strutture = 0, Legno_Strutture = 0, Ferro_Strutture = 0, Pietra_Strutture = 0, Oro_Strutture = 0;

            Cibo -= player.Guerrieri[0] * Esercito.Unità.Guerriero_1.Cibo + player.Lanceri[0] * Esercito.Unità.Lancere_1.Cibo + player.Arceri[0] * Esercito.Unità.Arcere_1.Cibo + player.Catapulte[0] * Esercito.Unità.Catapulta_1.Cibo;
            Cibo -= player.Guerrieri[1] * Esercito.Unità.Guerriero_2.Cibo + player.Lanceri[1] * Esercito.Unità.Lancere_2.Cibo + player.Arceri[1] * Esercito.Unità.Arcere_2.Cibo + player.Catapulte[1] * Esercito.Unità.Catapulta_2.Cibo;
            Cibo -= player.Guerrieri[2] * Esercito.Unità.Guerriero_3.Cibo + player.Lanceri[2] * Esercito.Unità.Lancere_3.Cibo + player.Arceri[2] * Esercito.Unità.Arcere_3.Cibo + player.Catapulte[2] * Esercito.Unità.Catapulta_3.Cibo;
            Cibo -= player.Guerrieri[3] * Esercito.Unità.Guerriero_4.Cibo + player.Lanceri[3] * Esercito.Unità.Lancere_4.Cibo + player.Arceri[3] * Esercito.Unità.Arcere_4.Cibo + player.Catapulte[3] * Esercito.Unità.Catapulta_4.Cibo;
            Cibo -= player.Guerrieri[4] * Esercito.Unità.Guerriero_5.Cibo + player.Lanceri[4] * Esercito.Unità.Lancere_5.Cibo + player.Arceri[4] * Esercito.Unità.Arcere_5.Cibo + player.Catapulte[4] * Esercito.Unità.Catapulta_5.Cibo;

            Oro -= player.Guerrieri[0] * Esercito.Unità.Guerriero_1.Salario + player.Lanceri[0] * Esercito.Unità.Lancere_1.Salario + player.Arceri[0] * Esercito.Unità.Arcere_1.Salario + player.Catapulte[0] * Esercito.Unità.Catapulta_1.Salario;
            Oro -= player.Guerrieri[1] * Esercito.Unità.Guerriero_2.Salario + player.Lanceri[1] * Esercito.Unità.Lancere_2.Salario + player.Arceri[1] * Esercito.Unità.Arcere_2.Salario + player.Catapulte[1] * Esercito.Unità.Catapulta_2.Salario;
            Oro -= player.Guerrieri[2] * Esercito.Unità.Guerriero_3.Salario + player.Lanceri[2] * Esercito.Unità.Lancere_3.Salario + player.Arceri[2] * Esercito.Unità.Arcere_3.Salario + player.Catapulte[2] * Esercito.Unità.Catapulta_3.Salario;
            Oro -= player.Guerrieri[3] * Esercito.Unità.Guerriero_4.Salario + player.Lanceri[3] * Esercito.Unità.Lancere_4.Salario + player.Arceri[3] * Esercito.Unità.Arcere_4.Salario + player.Catapulte[3] * Esercito.Unità.Catapulta_4.Salario;
            Oro -= player.Guerrieri[4] * Esercito.Unità.Guerriero_5.Salario + player.Lanceri[4] * Esercito.Unità.Lancere_5.Salario + player.Arceri[4] * Esercito.Unità.Arcere_5.Salario + player.Catapulte[4] * Esercito.Unità.Catapulta_5.Salario;

            Cibo_Strutture -= player.Caserma_Guerrieri * Strutture.Edifici.CasermaGuerrieri.Consumo_Cibo;
            Oro_Strutture -= player.Caserma_Guerrieri * Strutture.Edifici.CasermaGuerrieri.Consumo_Oro;

            Cibo_Strutture -= player.Caserma_Lancieri * Strutture.Edifici.CasermaLanceri.Consumo_Cibo;
            Oro_Strutture -= player.Caserma_Lancieri * Strutture.Edifici.CasermaLanceri.Consumo_Oro;

            Cibo_Strutture -= player.Caserma_Arceri * Strutture.Edifici.CasermaArceri.Consumo_Cibo;
            Oro_Strutture -= player.Caserma_Arceri * Strutture.Edifici.CasermaArceri.Consumo_Oro;

            Cibo_Strutture -= player.Caserma_Catapulte * Strutture.Edifici.CasermaCatapulte.Consumo_Cibo;
            Oro_Strutture -= player.Caserma_Catapulte * Strutture.Edifici.CasermaCatapulte.Consumo_Oro;

            Legno_Strutture -= player.Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Consumo_Legno;
            Ferro_Strutture -= player.Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Consumo_Ferro;
            Oro_Strutture -= player.Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Consumo_Oro;

            Legno_Strutture -= player.Workshop_Lance * Strutture.Edifici.ProduzioneLance.Consumo_Legno;
            Ferro_Strutture -= player.Workshop_Lance * Strutture.Edifici.ProduzioneLance.Consumo_Ferro;
            Oro_Strutture -= player.Workshop_Lance * Strutture.Edifici.ProduzioneLance.Consumo_Oro;

            Legno_Strutture -= player.Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Consumo_Legno;
            Oro_Strutture -= player.Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Consumo_Oro;

            Legno_Strutture -= player.Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Consumo_Legno;
            Ferro_Strutture -= player.Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Consumo_Ferro;
            Oro_Strutture -= player.Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Consumo_Oro;

            Ferro_Strutture -= player.Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Consumo_Ferro;
            Oro_Strutture -= player.Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Consumo_Oro;

            Legno_Strutture -= player.Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Consumo_Legno;
            Pietra_Strutture -= player.Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Consumo_Pietra;
            Ferro_Strutture -= player.Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Consumo_Ferro;
            Oro_Strutture -= player.Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Consumo_Oro;

            //QuestManager.QuestUpdate(player);
            //QuestManager.QuestRewardUpdate(player);
            //Descrizioni.DescUpdate(player);
            //AggiornaVillaggiClient(player);

            string data =
            "Update_Data|" +

            //Dati utente
            $"livello={player.Livello}|" +
            $"esperienza={player.Esperienza}|" +
            $"vip={player.Vip}|" +
            $"vip_Tempo={player.FormatTime(player.Vip_Tempo)}|" +

            $"Scudo_Tempo={player.FormatTime(player.ScudoDellaPace)}|" +
            $"Costruttori_Tempo={player.FormatTime(player.Costruttori)}|" +
            $"Reclutatori_Tempo={player.FormatTime(player.Reclutatori)}|" +

            $"GamePass_Base={player.GamePass_Base}|" +
            $"GamePass_Base_Tempo={player.FormatTime(player.GamePass_Base_Tempo)}|" +
            $"GamePass_Avanzato={player.GamePass_Avanzato}|" +
            $"GamePass_Avanzato_Tempo={player.FormatTime(player.GamePass_Avanzato_Tempo)}|" +

            $"QuestMensili_Tempo={player.FormatTime(Variabili_Server.timer_Reset_Quest)}|" +
            $"Barbari_Tempo={player.FormatTime(Variabili_Server.timer_Reset_Barbari)}|" +

            $"punti_quest={player.Punti_Quest}|" +
            $"costo_terreni_Virtuali={Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola}|" +
            $"Tutorial={player.Tutorial}|" +

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

            //Risorse
            $"cibo={player.Cibo:#,0}|" +
            $"legna={player.Legno:#,0}|" +
            $"pietra={player.Pietra:#,0}|" +
            $"ferro={player.Ferro:#,0}|" +
            $"oro={player.Oro:#,0}|" +
            $"popolazione={player.Popolazione:#,0.00}|" +

            $"dollari_virtuali={player.Dollari_Virtuali:#,0.0000000000}|" +
            $"diamanti_blu={player.Diamanti_Blu:#,0}|" +
            $"diamanti_viola={player.Diamanti_Viola:#,0}|" +

            $"cibo_max={Edifici.Fattoria.Limite:#,0}|" +
            $"legno_max={Edifici.Segheria.Limite:#,0}|" +
            $"pietra_max={Edifici.CavaPietra.Limite:#,0}|" +
            $"ferro_max={Edifici.MinieraFerro.Limite:#,0}|" +
            $"oro_max={Edifici.MinieraOro.Limite:#,0}|" +
            $"popolazione_max={Edifici.Case.Limite:#,0}|" +

            //Produzione Risorse
            $"cibo_s={player.Fattoria * (Strutture.Edifici.Fattoria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Cibo * (1 + player.Bonus_Produzione_Risorse)):#,0.00}|" +
            $"legna_s={player.Segheria * (Strutture.Edifici.Segheria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Legno * (1 + player.Bonus_Produzione_Risorse)):#,0.00}|" +
            $"pietra_s={player.CavaPietra * (Strutture.Edifici.CavaPietra.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Pietra * (1 + player.Bonus_Produzione_Risorse)):#,0.00}|" +
            $"ferro_s={player.MinieraFerro * (Strutture.Edifici.MinieraFerro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Ferro * (1 + player.Bonus_Produzione_Risorse)):#,0.00}|" +
            $"oro_s={player.MinieraOro * (Strutture.Edifici.MinieraOro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Oro * (1 + player.Bonus_Produzione_Risorse)):#,0.00}|" +
            $"popolazione_s={player.Abitazioni * (Strutture.Edifici.Case.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Popolazione):#,0.000}|" +

            $"spade_s={player.Workshop_Spade * (Strutture.Edifici.ProduzioneSpade.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Spade):#,0.000}|" +
            $"lance_s={player.Workshop_Lance * (Strutture.Edifici.ProduzioneLance.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Lance):#,0.000}|" +
            $"archi_s={player.Workshop_Archi * (Strutture.Edifici.ProduzioneArchi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Archi):#,0.000}|" +
            $"scudi_s={player.Workshop_Scudi * (Strutture.Edifici.ProduzioneScudi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Scudi):#,0.000}|" +
            $"armature_s={player.Workshop_Armature * (Strutture.Edifici.ProduzioneArmature.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Armature):#,0.000}|" +
            $"frecce_s={player.Workshop_Frecce * (Strutture.Edifici.ProduzioneFrecce.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Popolazione):#,0.000}|" +

            $"consumo_cibo_s={Cibo:#,0.00}|" + //Esercito
            $"consumo_oro_s={Oro:#,0.00}|" + //Esercito

            $"consumo_cibo_strutture={Cibo_Strutture:#,0.00}|" + //Strutture
            $"consumo_legno_strutture={Legno_Strutture:#,0.00}|" + //Strutture
            $"consumo_pietra_strutture={Pietra_Strutture:#,0.00}|" + //Strutture
            $"consumo_ferro_strutture={Ferro_Strutture:#,0.00}|" + //Strutture
            $"consumo_oro_strutture={Oro_Strutture:#,0.00}|" + //Strutture

            //Limiti Risorse
            $"cibo_limite={player.Fattoria * Strutture.Edifici.Fattoria.Limite:#,0}|" +
            $"legna_limite={player.Segheria * Strutture.Edifici.Segheria.Limite:#,0}|" +
            $"pietra_limite={player.CavaPietra * Strutture.Edifici.CavaPietra.Limite:#,0}|" +
            $"ferro_limite={player.MinieraFerro * Strutture.Edifici.MinieraFerro.Limite:#,0}|" +
            $"oro_limite={player.MinieraOro * Strutture.Edifici.MinieraOro.Limite:#,0}|" +
            $"popolazione_limite={player.Abitazioni * Strutture.Edifici.Case.Limite:#,0}|" +

            $"spade_limite={player.Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Limite:#,0}|" +
            $"lance_limite={player.Workshop_Lance * Strutture.Edifici.ProduzioneLance.Limite:#,0}|" +
            $"archi_limite={player.Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Limite:#,0}|" +
            $"scudi_limite={player.Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Limite:#,0}|" +
            $"armature_limite={player.Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Limite:#,0}|" +
            $"frecce_limite={player.Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Limite:#,0}|" +

            //Risorse Militari
            $"spade={player.Spade:#,0.00}|" +
            $"lance={player.Lance:#,0.00}|" +
            $"archi={player.Archi:#,0.00}|" +
            $"scudi={player.Scudi:#,0.00}|" +
            $"armature={player.Armature:#,0.00}|" +
            $"frecce={player.Frecce:#,0}|" +

            $"spade_max={Edifici.ProduzioneSpade.Limite:#,0}|" +
            $"lance_max={Edifici.ProduzioneLance.Limite:#,0}|" +
            $"archi_max={Edifici.ProduzioneArchi.Limite:#,0}|" +
            $"scudi_max={Edifici.ProduzioneScudi.Limite:#,0}|" +
            $"armature_max={Edifici.ProduzioneArmature.Limite:#,0}|" +
            $"frecce_max={Edifici.ProduzioneFrecce.Limite:#,0}|" +

            //Strutture Civili
            $"fattorie={player.Fattoria:#,0}|" +
            $"segherie={player.Segheria:#,0}|" +
            $"cave_pietra={player.CavaPietra:#,0}|" +
            $"miniere_ferro={player.MinieraFerro:#,0}|" +
            $"miniere_oro={player.MinieraOro:#,0}|" +
            $"case={player.Abitazioni:#,0}|" +

            //Terreni Virtuali
            $"comune={player.Terreno_Comune:#,0}|" +
            $"noncomune={player.Terreno_NonComune:#,0}|" +
            $"raro={player.Terreno_Raro:#,0}|" +
            $"epico={player.Terreno_Epico:#,0}|" +
            $"leggendario={player.Terreno_Leggendario:#,0}|" +

            //Strutture Militari
            $"workshop_spade={player.Workshop_Spade}|" +
            $"workshop_lance={player.Workshop_Lance}|" +
            $"workshop_archi={player.Workshop_Archi}|" +
            $"workshop_scudi={player.Workshop_Scudi}|" +
            $"workshop_armature={player.Workshop_Armature}|" +
            $"workshop_frecce={player.Workshop_Frecce}|" +

            $"caserma_guerrieri={player.Caserma_Guerrieri}|" +
            $"caserma_lanceri={player.Caserma_Lancieri}|" +
            $"caserma_arceri={player.Caserma_Arceri}|" +
            $"caserma_catapulte={player.Caserma_Catapulte}|" +

            $"guerrieri_max={player.GuerrieriMax:#,0}|" +
            $"lanceri_max={player.LancieriMax:#,0}|" +
            $"arceri_max={player.ArceriMax:#,0}|" +
            $"catapulte_max={player.CatapulteMax:#,0}|" +

            // Code edifici in formato key=valore (chiavi minuscole)
            $"fattoria_coda={buildingsQueue.GetValueOrDefault("Fattoria", 0)}|" +
            $"segheria_coda={buildingsQueue.GetValueOrDefault("Segheria", 0)}|" +
            $"cavapietra_coda={buildingsQueue.GetValueOrDefault("CavaPietra", 0)}|" +
            $"minieraferro_coda={buildingsQueue.GetValueOrDefault("MinieraFerro", 0)}|" +
            $"minieraoro_coda={buildingsQueue.GetValueOrDefault("MinieraOro", 0)}|" +
            $"casa_coda={buildingsQueue.GetValueOrDefault("Case", 0)}|" +

            $"workshop_spade_coda={buildingsQueue.GetValueOrDefault("ProduzioneSpade", 0)}|" +
            $"workshop_lance_coda={buildingsQueue.GetValueOrDefault("ProduzioneLance", 0)}|" +
            $"workshop_archi_coda={buildingsQueue.GetValueOrDefault("ProduzioneArchi", 0)}|" +
            $"workshop_scudi_coda={buildingsQueue.GetValueOrDefault("ProduzioneScudi", 0)}|" +
            $"workshop_armature_coda={buildingsQueue.GetValueOrDefault("ProduzioneArmature", 0)}|" +
            $"workshop_frecce_coda={buildingsQueue.GetValueOrDefault("ProduzioneFrecce", 0)}|" +

            $"caserma_guerrieri_coda={buildingsQueue.GetValueOrDefault("CasermaGuerrieri", 0)}|" +
            $"caserma_lanceri_coda={buildingsQueue.GetValueOrDefault("CasermaLanceri", 0)}|" +
            $"caserma_arceri_coda={buildingsQueue.GetValueOrDefault("CasermaArceri", 0)}|" +
            $"caserma_catapulte_coda={buildingsQueue.GetValueOrDefault("CasermaCatapulte", 0)}|" +

            // unità
            $"guerrieri_1={player.Guerrieri[0]}|" +
            $"guerrieri_2={player.Guerrieri[1]}|" +
            $"guerrieri_3={player.Guerrieri[2]}|" +
            $"guerrieri_4={player.Guerrieri[3]}|" +
            $"guerrieri_5={player.Guerrieri[4]}|" +

            $"lanceri_1={player.Lanceri[0]}|" +
            $"lanceri_2={player.Lanceri[1]}|" +
            $"lanceri_3={player.Lanceri[2]}|" +
            $"lanceri_4={player.Lanceri[3]}|" +
            $"lanceri_5={player.Lanceri[4]}|" +

            $"arceri_1={player.Arceri[0]}|" +
            $"arceri_2={player.Arceri[1]}|" +
            $"arceri_3={player.Arceri[2]}|" +
            $"arceri_4={player.Arceri[3]}|" +
            $"arceri_5={player.Arceri[4]}|" +

            $"catapulte_1={player.Catapulte[0]}|" +
            $"catapulte_2={player.Catapulte[1]}|" +
            $"catapulte_3={player.Catapulte[2]}|" +
            $"catapulte_4={player.Catapulte[3]}|" +
            $"catapulte_5={player.Catapulte[4]}|" +

            // Code unità
            $"guerrieri_1_coda={unitsQueue.GetValueOrDefault("Guerrieri_1", 0)}|" +
            $"guerrieri_2_coda={unitsQueue.GetValueOrDefault("Guerrieri_2", 0)}|" +
            $"guerrieri_3_coda={unitsQueue.GetValueOrDefault("Guerrieri_3", 0)}|" +
            $"guerrieri_4_coda={unitsQueue.GetValueOrDefault("Guerrieri_4", 0)}|" +
            $"guerrieri_5_coda={unitsQueue.GetValueOrDefault("Guerrieri_5", 0)}|" +

            $"lanceri_1_coda={unitsQueue.GetValueOrDefault("Lanceri_1", 0)}|" +
            $"lanceri_2_coda={unitsQueue.GetValueOrDefault("Lanceri_2", 0)}|" +
            $"lanceri_3_coda={unitsQueue.GetValueOrDefault("Lanceri_3", 0)}|" +
            $"lanceri_4_coda={unitsQueue.GetValueOrDefault("Lanceri_4", 0)}|" +
            $"lanceri_5_coda={unitsQueue.GetValueOrDefault("Lanceri_5", 0)}|" +

            $"arceri_1_coda={unitsQueue.GetValueOrDefault("Arceri_1", 0)}|" +
            $"arceri_2_coda={unitsQueue.GetValueOrDefault("Arceri_2", 0)}|" +
            $"arceri_3_coda={unitsQueue.GetValueOrDefault("Arceri_3", 0)}|" +
            $"arceri_4_coda={unitsQueue.GetValueOrDefault("Arceri_4", 0)}|" +
            $"arceri_5_coda={unitsQueue.GetValueOrDefault("Arceri_5", 0)}|" +

            $"catapulte_1_coda={unitsQueue.GetValueOrDefault("Catapulte_1", 0)}|" +
            $"catapulte_2_coda={unitsQueue.GetValueOrDefault("Catapulte_2", 0)}|" +
            $"catapulte_3_coda={unitsQueue.GetValueOrDefault("Catapulte_3", 0)}|" +
            $"catapulte_4_coda={unitsQueue.GetValueOrDefault("Catapulte_4", 0)}|" +
            $"catapulte_5_coda={unitsQueue.GetValueOrDefault("Catapulte_5", 0)}|" +

            $"forza_esercito={player.forza_Esercito:#,0.00}|" +

            //Terreni Virtuali
            $"terreni_comune={player.Terreno_Comune}|" +
            $"terreni_noncomune={player.Terreno_NonComune}|" +
            $"terreni_raro={player.Terreno_Raro}|" +
            $"terreni_epico={player.Terreno_Epico}|" +
            $"terreni_leggendario={player.Terreno_Leggendario}|" +

            //Città Ingresso
            $"Guarnigione_Ingresso={player.Guarnigione_Ingresso}|" +
            $"Guarnigione_IngressoMax={player.Guarnigione_IngressoMax}|" +

            //Ingresso
            $"Guerrieri_1_Ingresso={player.Guerrieri_Ingresso[0]}|" +
            $"Lanceri_1_Ingresso={player.Lanceri_Ingresso[0]}|" +
            $"Arceri_1_Ingresso={player.Arceri_Ingresso[0]}|" +
            $"Catapulte_1_Ingresso={player.Catapulte_Ingresso[0]}|" +
            $"Guerrieri_2_Ingresso={player.Guerrieri_Ingresso[1]}|" +
            $"Lanceri_2_Ingresso={player.Lanceri_Ingresso[1]}|" +
            $"Arceri_2_Ingresso={player.Arceri_Ingresso[1]}|" +
            $"Catapulte_2_Ingresso={player.Catapulte_Ingresso[1]}|" +
            $"Guerrieri_3_Ingresso={player.Guerrieri_Ingresso[2]}|" +
            $"Lanceri_3_Ingresso={player.Lanceri_Ingresso[2]}|" +
            $"Arceri_3_Ingresso={player.Arceri_Ingresso[2]}|" +
            $"Catapulte_3_Ingresso={player.Catapulte_Ingresso[2]}|" +
            $"Guerrieri_4_Ingresso={player.Guerrieri_Ingresso[3]}|" +
            $"Lanceri_4_Ingresso={player.Lanceri_Ingresso[3]}|" +
            $"Arceri_4_Ingresso={player.Arceri_Ingresso[3]}|" +
            $"Catapulte_4_Ingresso={player.Catapulte_Ingresso[3]}|" +
            $"Guerrieri_5_Ingresso={player.Guerrieri_Ingresso[4]}|" +
            $"Lanceri_5_Ingresso={player.Lanceri_Ingresso[4]}|" +
            $"Arceri_5_Ingresso={player.Arceri_Ingresso[4]}|" +
            $"Catapulte_5_Ingresso={player.Catapulte_Ingresso[4]}|" +

            //Città Cancello
            $"Guarnigione_Cancello={player.Guarnigione_Cancello}|" +
            $"Guarnigione_CancelloMax={player.Guarnigione_CancelloMax}|" +
            $"Guerrieri_1_Cancello={player.Guerrieri_Cancello[0]}|" +
            $"Lanceri_1_Cancello={player.Lanceri_Cancello[0]}|" +
            $"Arceri_1_Cancello={player.Arceri_Cancello[0]}|" +
            $"Catapulte_1_Cancello={player.Catapulte_Cancello[0]}|" +
            $"Guerrieri_2_Cancello={player.Guerrieri_Cancello[1]}|" +
            $"Lanceri_2_Cancello={player.Lanceri_Cancello[1]}|" +
            $"Arceri_2_Cancello={player.Arceri_Cancello[1]}|" +
            $"Catapulte_2_Cancello={player.Catapulte_Cancello[1]}|" +
            $"Guerrieri_3_Cancello={player.Guerrieri_Cancello[2]}|" +
            $"Lanceri_3_Cancello={player.Lanceri_Cancello[2]}|" +
            $"Arceri_3_Cancello={player.Arceri_Cancello[2]}|" +
            $"Catapulte_3_Cancello={player.Catapulte_Cancello[2]}|" +
            $"Guerrieri_4_Cancello={player.Guerrieri_Cancello[3]}|" +
            $"Lanceri_4_Cancello={player.Lanceri_Cancello[3]}|" +
            $"Arceri_4_Cancello={player.Arceri_Cancello[3]}|" +
            $"Catapulte_4_Cancello={player.Catapulte_Cancello[3]}|" +
            $"Guerrieri_5_Cancello={player.Guerrieri_Cancello[4]}|" +
            $"Lanceri_5_Cancello={player.Lanceri_Cancello[4]}|" +
            $"Arceri_5_Cancello={player.Arceri_Cancello[4]}|" +
            $"Catapulte_5_Cancello={player.Catapulte_Cancello[4]}|" +

            $"Salute_Cancello={player.Salute_Cancello}|" +
            $"Salute_CancelloMax={player.Salute_CancelloMax}|" +
            $"Difesa_Cancello={player.Difesa_Cancello}|" +
            $"Difesa_CancelloMax={player.Difesa_CancelloMax}|" +

            //Città Mura
            $"Guarnigione_Mura={player.Guarnigione_Mura}|" +
            $"Guarnigione_MuraMax={player.Guarnigione_MuraMax}|" +
            $"Guerrieri_1_Mura={player.Guerrieri_Mura[0]}|" +
            $"Lanceri_1_Mura={player.Lanceri_Mura[0]}|" +
            $"Arceri_1_Mura={player.Arceri_Mura[0]}|" +
            $"Catapulte_1_Mura={player.Catapulte_Mura[0]}|" +
            $"Guerrieri_2_Mura={player.Guerrieri_Mura[1]}|" +
            $"Lanceri_2_Mura={player.Lanceri_Mura[1]}|" +
            $"Arceri_2_Mura={player.Arceri_Mura[1]}|" +
            $"Catapulte_2_Mura={player.Catapulte_Mura[1]}|" +
            $"Guerrieri_3_Mura={player.Guerrieri_Mura[2]}|" +
            $"Lanceri_3_Mura={player.Lanceri_Mura[2]}|" +
            $"Arceri_3_Mura={player.Arceri_Mura[2]}|" +
            $"Catapulte_3_Mura={player.Catapulte_Mura[2]}|" +
            $"Guerrieri_4_Mura={player.Guerrieri_Mura[3]}|" +
            $"Lanceri_4_Mura={player.Lanceri_Mura[3]}|" +
            $"Arceri_4_Mura={player.Arceri_Mura[3]}|" +
            $"Catapulte_4_Mura={player.Catapulte_Mura[3]}|" +
            $"Guerrieri_5_Mura={player.Guerrieri_Mura[4]}|" +
            $"Lanceri_5_Mura={player.Lanceri_Mura[4]}|" +
            $"Arceri_5_Mura={player.Arceri_Mura[4]}|" +
            $"Catapulte_5_Mura={player.Catapulte_Mura[4]}|" +
            $"Salute_Mura={player.Salute_Mura}|" +
            $"Salute_MuraMax={player.Salute_MuraMax}|" +
            $"Difesa_Mura={player.Difesa_Mura}|" +
            $"Difesa_MuraMax={player.Difesa_MuraMax}|" +

            //Città Torri
            $"Guarnigione_Torri={player.Guarnigione_Torri}|" +
            $"Guarnigione_TorriMax={player.Guarnigione_TorriMax}|" +
            $"Guerrieri_1_Torri={player.Guerrieri_Torri[0]}|" +
            $"Lanceri_1_Torri={player.Lanceri_Torri[0]}|" +
            $"Arceri_1_Torri={player.Arceri_Torri[0]}|" +
            $"Catapulte_1_Torri={player.Catapulte_Torri[0]}|" +
            $"Guerrieri_2_Torri={player.Guerrieri_Torri[1]}|" +
            $"Lanceri_2_Torri={player.Lanceri_Torri[1]}|" +
            $"Arceri_2_Torri={player.Arceri_Torri[1]}|" +
            $"Catapulte_2_Torri={player.Catapulte_Torri[1]}|" +
            $"Guerrieri_3_Torri={player.Guerrieri_Torri[2]}|" +
            $"Lanceri_3_Torri={player.Lanceri_Torri[2]}|" +
            $"Arceri_3_Torri={player.Arceri_Torri[2]}|" +
            $"Catapulte_3_Torri={player.Catapulte_Torri[2]}|" +
            $"Guerrieri_4_Torri={player.Guerrieri_Torri[3]}|" +
            $"Lanceri_4_Torri={player.Lanceri_Torri[3]}|" +
            $"Arceri_4_Torri={player.Arceri_Torri[3]}|" +
            $"Catapulte_4_Torri={player.Catapulte_Torri[3]}|" +
            $"Guerrieri_5_Torri={player.Guerrieri_Torri[4]}|" +
            $"Lanceri_5_Torri={player.Lanceri_Torri[4]}|" +
            $"Arceri_5_Torri={player.Arceri_Torri[4]}|" +
            $"Catapulte_5_Torri={player.Catapulte_Torri[4]}|" +

            $"Salute_Torri={player.Salute_Torri}|" +
            $"Salute_TorriMax={player.Salute_TorriMax}|" +
            $"Difesa_Torri={player.Difesa_Torri}|" +
            $"Difesa_TorriMax={player.Difesa_TorriMax}|" +

            //Città Castello
            $"Guarnigione_Castello={player.Guarnigione_Castello}|" +
            $"Guarnigione_CastelloMax={player.Guarnigione_CastelloMax}|" +
            $"Guerrieri_1_Castello={player.Guerrieri_Castello[0]}|" +
            $"Lanceri_1_Castello={player.Lanceri_Castello[0]}|" +
            $"Arceri_1_Castello={player.Arceri_Castello[0]}|" +
            $"Catapulte_1_Castello={player.Catapulte_Castello[0]}|" +
            $"Guerrieri_2_Castello={player.Guerrieri_Castello[1]}|" +
            $"Lanceri_2_Castello={player.Lanceri_Castello[1]}|" +
            $"Arceri_2_Castello={player.Arceri_Castello[1]}|" +
            $"Catapulte_2_Castello={player.Catapulte_Castello[1]}|" +
            $"Guerrieri_3_Castello={player.Guerrieri_Castello[2]}|" +
            $"Lanceri_3_Castello={player.Lanceri_Castello[2]}|" +
            $"Arceri_3_Castello={player.Arceri_Castello[2]}|" +
            $"Catapulte_3_Castello={player.Catapulte_Castello[2]}|" +
            $"Guerrieri_4_Castello={player.Guerrieri_Castello[3]}|" +
            $"Lanceri_4_Castello={player.Lanceri_Castello[3]}|" +
            $"Arceri_4_Castello={player.Arceri_Castello[3]}|" +
            $"Catapulte_4_Castello={player.Catapulte_Castello[3]}|" +
            $"Guerrieri_5_Castello={player.Guerrieri_Castello[4]}|" +
            $"Lanceri_5_Castello={player.Lanceri_Castello[4]}|" +
            $"Arceri_5_Castello={player.Arceri_Castello[4]}|" +
            $"Catapulte_5_Castello={player.Catapulte_Castello[4]}|" +
            $"Salute_Castello={player.Salute_Castello}|" +
            $"Salute_CastelloMax={player.Salute_CastelloMax}|" +
            $"Difesa_Castello={player.Difesa_Castello}|" +
            $"Difesa_CastelloMax={player.Difesa_CastelloMax}|" +

            //Città Piazza
            $"Guarnigione_Citta={player.Guarnigione_Citta}|" +
            $"Guarnigione_CittaMax={player.Guarnigione_CittaMax}|" +
            $"Guerrieri_1_Citta={player.Guerrieri_Citta[0]}|" +
            $"Lanceri_1_Citta={player.Lanceri_Citta[0]}|" +
            $"Arceri_1_Citta={player.Arceri_Citta[0]}|" +
            $"Catapulte_1_Citta={player.Catapulte_Citta[0]}|" +
            $"Guerrieri_2_Citta={player.Guerrieri_Citta[1]}|" +
            $"Lanceri_2_Citta={player.Lanceri_Citta[1]}|" +
            $"Arceri_2_Citta={player.Arceri_Citta[1]}|" +
            $"Catapulte_2_Citta={player.Catapulte_Citta[1]}|" +
            $"Guerrieri_3_Citta={player.Guerrieri_Citta[2]}|" +
            $"Lanceri_3_Citta={player.Lanceri_Citta[2]}|" +
            $"Arceri_3_Citta={player.Arceri_Citta[2]}|" +
            $"Catapulte_3_Citta={player.Catapulte_Citta[2]}|" +
            $"Guerrieri_4_Citta={player.Guerrieri_Citta[3]}|" +
            $"Lanceri_4_Citta={player.Lanceri_Citta[3]}|" +
            $"Arceri_4_Citta={player.Arceri_Citta[3]}|" +
            $"Catapulte_4_Citta={player.Catapulte_Citta[3]}|" +
            $"Guerrieri_5_Citta={player.Guerrieri_Citta[4]}|" +
            $"Lanceri_5_Citta={player.Lanceri_Citta[4]}|" +
            $"Arceri_5_Citta={player.Arceri_Citta[4]}|" +
            $"Catapulte_5_Citta={player.Catapulte_Citta[4]}|" +

            //Quest claim "normali"
            $"punti_quest={player.Punti_Quest}|" +

            //Ricerca I (Infinita)
            $"guerriero_salute={player.Guerriero_Salute}|" +
            $"guerriero_difesa={player.Guerriero_Difesa}|" +
            $"guerriero_attacco={player.Guerriero_Attacco}|" +
            $"guerriero_livello={player.Guerriero_Livello}|" +

            $"lanciere_salute={player.Lancere_Salute}|" +
            $"lanciere_difesa={player.Lancere_Difesa}|" +
            $"lanciere_attacco={player.Lancere_Attacco}|" +
            $"lanciere_livello={player.Lancere_Livello}|" +

            $"arciere_salute={player.Arcere_Salute}|" +
            $"arciere_difesa={player.Arcere_Difesa}|" +
            $"arciere_attacco={player.Arcere_Attacco}|" +
            $"arciere_livello={player.Arcere_Livello}|" +

            $"catapulta_salute={player.Catapulta_Salute}|" +
            $"catapulta_difesa={player.Catapulta_Difesa}|" +
            $"catapulta_attacco={player.Catapulta_Attacco}|" +
            $"catapulta_livello={player.Catapulta_Livello}|" +

            $"ricerca_produzione={player.Ricerca_Produzione}|" +
            $"ricerca_costruzione={player.Ricerca_Costruzione}|" +
            $"ricerca_addestramento={player.Ricerca_Addestramento}|" +
            $"ricerca_popolazione={player.Ricerca_Popolazione}|" +

            //Ricerca citta
            $"ricerca_ingresso_guarnigione={player.Ricerca_Ingresso_Guarnigione}|" +
            $"ricerca_citta_guarnigione={player.Ricerca_Citta_Guarnigione}|" +

            $"ricerca_cancello_salute={player.Ricerca_Cancello_Salute}|" +
            $"ricerca_cancello_difesa={player.Ricerca_Cancello_Difesa}|" +
            $"ricerca_cancello_guarnigione={player.Ricerca_Cancello_Guarnigione}|" +

            $"ricerca_mura_salute={player.Ricerca_Mura_Salute}|" +
            $"ricerca_mura_difesa={player.Ricerca_Mura_Difesa}|" +
            $"ricerca_mura_guarnigione={player.Ricerca_Mura_Guarnigione}|" +

            $"ricerca_torri_salute={player.Ricerca_Torri_Salute}|" +
            $"ricerca_torri_difesa={player.Ricerca_Torri_Difesa}|" +
            $"ricerca_torri_guarnigione={player.Ricerca_Torri_Guarnigione}|" +

            $"ricerca_castello_salute={player.Ricerca_Castello_Salute}|" +
            $"ricerca_castello_difesa={player.Ricerca_Castello_Difesa}|" +
            $"ricerca_castello_guarnigione={player.Ricerca_Castello_Guarnigione}|" +

            //Tempi
            $"Code_Costruzioni={player.Code_Costruzione}|" +
            $"Code_Reclutamenti={player.Code_Reclutamento}|" +
            $"Code_Costruzioni_Disponibili={player.task_Attuale_Costruzioni.Count}|" +
            $"Code_Reclutamenti_Disponibili={player.task_Attuale_Recutamento.Count}|" +

            $"Tempo_Costruzione={BuildingManagerV2.Get_Total_Building_Time(player)}|" +
            $"Tempo_Reclutamento={UnitManagerV2.Get_Total_Recruit_Time(player)}|" +
            $"Tempo_Ricerca_Citta={ResearchManager.GetTotalResearchTime(player)}|" +
            $"Tempo_Ricerca_Globale={1}|" +

            $"Ricerca_Attiva={player.Ricerca_Attiva}|" +

            $"D_Viola_D_Blu={Variabili_Server.D_Viola_To_Blu}|" +
            $"Tempo_D_Blu={Variabili_Server.Velocizzazione_Tempo}|" +

            // Statistiche
            $"Potenza_Totale={player.Potenza_Totale:#,0}|" +
            $"Potenza_Strutture={player.Potenza_Strutture:#,0}|" +
            $"Potenza_Ricerca={player.Potenza_Ricerca:#,0}|" +
            $"Potenza_Esercito={player.Potenza_Esercito:#,0}|" +

            $"Unità_Eliminate={player.Unità_Eliminate:#,0}|" +
            $"Guerrieri_Eliminate={player.Guerrieri_Eliminati:#,0}|" +
            $"Lanceri_Eliminate={player.Lanceri_Eliminati:#,0}|" +
            $"Arceri_Eliminate={player.Arceri_Eliminati:#,0}|" +
            $"Catapulte_Eliminate={player.Catapulte_Eliminate:#,0}|" +

            $"Unità_Perse={player.Unità_Perse:#,0}|" +
            $"Guerrieri_Persi={player.Guerrieri_Persi:#,0}|" +
            $"Lanceri_Persi={player.Lanceri_Persi:#,0}|" +
            $"Arceri_Persi={player.Arceri_Persi:#,0}|" +
            $"Catapulte_Persi={player.Catapulte_Perse:#,0}|" +
            $"Risorse_Razziate={player.Risorse_Razziate:#,0}|" +

            $"Strutture_Civili_Costruite={player.Strutture_Civili_Costruite:#,0}|" +
            $"Strutture_Militari_Costruite={player.Strutture_Militari_Costruite:#,0}|" +
            $"Caserme_Costruite={player.Caserme_Costruite:#,0}|" +

            $"Frecce_Utilizzate={player.Frecce_Utilizzate:#,0}|" +
            $"Battaglie_Vinte={player.Battaglie_Vinte:#,0}|" +
            $"Battaglie_Perse={player.Battaglie_Perse:#,0}|" +
            $"Quest_Completate={player.Quest_Completate:#,0}|" +
            $"Attacchi_Subiti_PVP={player.Attacchi_Subiti_PVP:#,0}|" +
            $"Attacchi_Effettuati_PVP={player.Attacchi_Effettuati_PVP:#,0}|" +

            $"Barbari_Sconfitti={player.Barbari_Sconfitti:#,0}|" +
            $"Accampamenti_Barbari_Sconfitti={player.Accampamenti_Barbari_Sconfitti:#,0}|" +
            $"Città_Barbare_Sconfitte={player.Città_Barbare_Sconfitte:#,0}|" +
            $"Danno_HP_Barbaro={player.Danno_HP_Barbaro:#,0}|" +
            $"Danno_DEF_Barbaro={player.Danno_DEF_Barbaro:#,0}|" +

            $"Unità_Addestrate={player.Unità_Addestrate:#,0}|" +
            $"Risorse_Utilizzate={player.Risorse_Utilizzate:#,0}|" +
            $"Tempo_Addestramento_Risparmiato={player.FormatTime(player.Tempo_Addestramento)}|" +
            $"Tempo_Costruzione_Risparmiato={player.FormatTime(player.Tempo_Costruzione)}|" +
            $"Tempo_Ricerca_Risparmiato={player.FormatTime(player.Tempo_Ricerca)}|" +
            $"Tempo_Sottratto_Diamanti={player.FormatTime(player.Tempo_Sottratto_Diamanti)}|" +

            $"Bonus_Costruzione={player.Bonus_Costruzione * 100}%|" +
            $"Bonus_Addestramento={player.Bonus_Addestramento * 100}%|" +
            $"Bonus_Ricerca={player.Bonus_Ricerca * 100}%|" +
            $"Bonus_Riparazione={player.Bonus_Riparazione * 100}%|" +
            $"Bonus_Produzione_Risorse={player.Bonus_Produzione_Risorse * 100}%|" +
            $"Bonus_Capacità_Trasporto={player.Bonus_Capacità_Trasporto * 100}%|" +

            $"Bonus_Salute_Strutture={player.Bonus_Salute_Strutture * 100}%|" +
            $"Bonus_Difesa_Strutture={player.Bonus_Difesa_Strutture * 100}%|" +
            $"Bonus_Guarnigione_Strutture={player.Bonus_Guarnigione_Strutture * 100}%|" +

            $"Bonus_Attacco_Guerrieri={player.Bonus_Attacco_Guerrieri * 100}%|" +
            $"Bonus_Salute_Guerrieri={player.Bonus_Salute_Guerrieri * 100}%|" +
            $"Bonus_Difesa_Guerrieri={player.Bonus_Difesa_Guerrieri * 100}%|" +
            $"Bonus_Attacco_Lanceri={player.Bonus_Attacco_Lanceri * 100}%|" +
            $"Bonus_Salute_Lanceri={player.Bonus_Salute_Lanceri * 100}%|" +
            $"Bonus_Difesa_Lanceri={player.Bonus_Difesa_Lanceri * 100}%|" +
            $"Bonus_Attacco_Arceri={player.Bonus_Attacco_Arceri * 100}%|" +
            $"Bonus_Salute_Arceri={player.Bonus_Salute_Arceri * 100}%|" +
            $"Bonus_Difesa_Arceri={player.Bonus_Difesa_Arceri * 100}%|" +
            $"Bonus_Attacco_Catapulte={player.Bonus_Attacco_Catapulte * 100}%|" +
            $"Bonus_Salute_Catapulte={player.Bonus_Salute_Catapulte * 100}%|" +
            $"Bonus_Difesa_Catapulte={player.Bonus_Difesa_Catapulte * 100}%|";

            Server.Send(guid, data);
            if (player.Tutorial == true)
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

                Server.Send(guid, tutorialData);
                if (player.Tutorial_Stato[31]) player.Tutorial = false;
            }

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
                                    raduno_Player += $"{item}|{idAttacco}|{attacco.TempoRimanente / 60}|{items.Guerrieri[0]}|{items.Lanceri[0]}|{items.Arceri[0]}|{items.Catapulte[0]}-";
                    }
                }
            else raduno_Player = "";
            if (raduno != "") Server.Send(guid, raduno); //Invia i raduni aperti
            if (raduno_Player != "") Server.Send(guid, raduno_Player); //Invia i raduni aperti

            int potenzaPlayer = (int)player.Potenza_Totale;
            int maxPlayers = 80; // Numero massimo di giocatori da inviare

            // Filtra giocatori con potenza simile (ad esempio ±20%)
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
