using Server_Strategico.Gioco;
using System;
using System.Text;
using System.Text.Json;
using WatsonTcp;
using static BattaglieV2;
using static Server_Strategico.Gioco.Barbari;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.QuestManager;
using static Server_Strategico.Gioco.Strutture;
using static Server_Strategico.Gioco.Variabili_Server;

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

                        await Inizializza();
                        Server.Client_Connessi_Map.TryRemove(clientGuid, out _);
                        Server.Client_Connessi_Map.TryAdd(clientGuid, player.Username);

                        DescUpdate(player);
                        QuestManager.QuestUpdate(player);
                        AggiornaVillaggiClient(player);
                        Server.servers_.AggiornaListaPVP();
                    }
                    else
                        Server.Send(clientGuid, $"Login|false|Questo nome utente è già presente: [{msgArgs[1]}]");
                    break;
                case "Login":
                    bool login = await Login(msgArgs[1], msgArgs[2], clientGuid);
                    if (login == true)
                    {
                        Server.Send(clientGuid, "Login|true");
                        DescUpdate(player);
                        QuestManager.QuestUpdate(player);
                        AggiornaVillaggiClient(player);
                        Server.servers_.AggiornaListaPVP();
                    }
                    else
                        Server.Send(clientGuid, $"Login|false|Username o password non corrispondono. User: [{msgArgs[1]}] psw: [{msgArgs[2]}]");
                    break;
                case "Costruzione":
                    if (Convert.ToInt32(msgArgs[3]) > 0) BuildingManager.Costruzione("Fattoria", Convert.ToInt32(msgArgs[3]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[4]) > 0) BuildingManager.Costruzione("Segheria", Convert.ToInt32(msgArgs[4]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[5]) > 0) BuildingManager.Costruzione("CavaPietra", Convert.ToInt32(msgArgs[5]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[6]) > 0) BuildingManager.Costruzione("MinieraFerro", Convert.ToInt32(msgArgs[6]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[7]) > 0) BuildingManager.Costruzione("MinieraOro", Convert.ToInt32(msgArgs[7]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[8]) > 0) BuildingManager.Costruzione("Case", Convert.ToInt32(msgArgs[8]), clientGuid, player); // Costruisci fattorie
                    
                    if (Convert.ToInt32(msgArgs[9]) > 0) BuildingManager.Costruzione("ProduzioneSpade", Convert.ToInt32(msgArgs[9]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[10]) > 0) BuildingManager.Costruzione("ProduzioneLance", Convert.ToInt32(msgArgs[10]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[11]) > 0) BuildingManager.Costruzione("ProduzioneArchi", Convert.ToInt32(msgArgs[11]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[12]) > 0) BuildingManager.Costruzione("ProduzioneScudi", Convert.ToInt32(msgArgs[12]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[13]) > 0) BuildingManager.Costruzione("ProduzioneArmature", Convert.ToInt32(msgArgs[13]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[14]) > 0) BuildingManager.Costruzione("ProduzioneFrecce", Convert.ToInt32(msgArgs[14]), clientGuid, player); // Costruisci fattorie

                    if (Convert.ToInt32(msgArgs[15]) > 0) BuildingManager.Costruzione("CasermaGuerrieri", Convert.ToInt32(msgArgs[15]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[16]) > 0) BuildingManager.Costruzione("CasermaLanceri", Convert.ToInt32(msgArgs[16]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[17]) > 0) BuildingManager.Costruzione("CasermaArceri", Convert.ToInt32(msgArgs[17]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[18]) > 0) BuildingManager.Costruzione("CasermaCatapulte", Convert.ToInt32(msgArgs[18]), clientGuid, player); // Costruisci fattorie
                    break;
                case "Reclutamento":
                    if (Convert.ToInt32(msgArgs[4]) > 0) UnitManager.Reclutamento("Guerrieri", $"_{msgArgs[3]}", Convert.ToInt32(msgArgs[4]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[5]) > 0) UnitManager.Reclutamento("Lanceri", $"_{msgArgs[3]}", Convert.ToInt32(msgArgs[5]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[6]) > 0) UnitManager.Reclutamento("Arceri", $"_{msgArgs[3]}", Convert.ToInt32(msgArgs[6]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[7]) > 0) UnitManager.Reclutamento("Catapulte", $"_{msgArgs[3]}", Convert.ToInt32(msgArgs[7]), clientGuid, player); // Costruisci fattorie
                    break;
                case "Costruzione_Terreni":
                    BuildingManager.Terreni_Virtuali(clientGuid, player); // Costruisci fattorie
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
                        BuildingManager.UsaDiamantiPerVelocizzare(clientGuid, player, Convert.ToInt32(msgArgs[4]));
                    if (msgArgs[3] == "Reclutamento")
                        UnitManager.UsaDiamantiPerVelocizzareReclutamento(clientGuid, player, Convert.ToInt32(msgArgs[4]));
                    if (msgArgs[3] == "Ricerca")
                        ResearchManager.UsaDiamantiPerVelocizzareRicerca(clientGuid, player, Convert.ToInt32(msgArgs[4]));
                    break;
                case "Scambia_Diamanti":
                      Scambia_Diamanti(clientGuid, player, msgArgs[3]); //Diamanti viola --> blu
                    break;
                case "Shop":
                    Shop(clientGuid, player, msgArgs[3]); //Shop
                    break;
                case "SpostamentoTruppe":
                    SpostamentoTruppe(clientGuid, player, msgArgs); //Diamanti viola --> blu
                    break;
                case "Ripara":
                    Set_Riparazioni(player, msgArgs); //Diamanti viola --> blu
                    break;
                case "Ripara Tutto":
                    Set_Riparazioni(player, msgArgs); //Diamanti viola --> blu
                    break;

                default: Console.WriteLine($"Messaggio: [{msgArgs}]"); break;
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
            if (edificio_To == "Ingresso" && edificio_From == "Esercito Villaggio")
            {
                if (g + l + a + c > player.Guarnigione_IngressoMax) return;
                
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
            if (edificio_To == "Citta" && edificio_From == "Esercito Villaggio")
            {
                if (g + l + a + c > player.Guarnigione_IngressoMax) return;

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
            if (edificio_To == "Cancello" && edificio_From == "Esercito Villaggio")
            {
                if (g + l + a + c > player.Guarnigione_IngressoMax) return;

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
            if (edificio_To == "Mura" && edificio_From == "Esercito Villaggio")
            {
                if (g + l + a + c > player.Guarnigione_IngressoMax) return;

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
            if (edificio_To == "Torri" && edificio_From == "Esercito Villaggio")
            {
                if (g + l + a + c > player.Guarnigione_IngressoMax) return;

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
            if (edificio_To == "Castello" && edificio_From == "Esercito Villaggio")
            {
                if (g + l + a + c > player.Guarnigione_IngressoMax) return;

                if (player.Guerrieri[livello] >= g) player.Guerrieri_Castello[livello] += g;
                if (player.Lanceri[livello] >= l)   player.Lanceri_Castello[livello] += l;
                if (player.Arceri[livello] >= a)    player.Arceri_Castello[livello] += a;
                if (player.Catapulte[livello] >= c) player.Catapulte_Castello[livello] += c;
                return;
            }
            //if (edificio_From == "Castello" && edificio_To == "Esercito Villaggio")
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
            if (player.Diamanti_Viola >= diamanti_Viola)
            {
                player.Diamanti_Viola -= diamanti_Viola;
                player.Diamanti_Blu += diamanti_Viola * Variabili_Server.D_Viola_To_Blu;
                OnEvent(player, QuestEventType.Risorse, "Diamanti Viola", diamanti_Viola);
                Server.Send(player.guid_Player, $"Log_Server|Scambiati Diamanti Viola {diamanti_Viola} --> {diamanti_Viola * Variabili_Server.D_Viola_To_Blu} Diamanti Blu");
            }
        }

        public static void Shop(Guid guid, Player player, string comando)
        {
            int diamanti_Viola = player.Diamanti_Viola;
            int diamanti_Blu = player.Diamanti_Blu;
            decimal Dollari_Virtuali = player.Dollari_Virtuali;
            bool payment_Status_Blockchain = false;

            int numero_Code_Base = Variabili_Server.numero_Code_Base;
            int numero_Code_Vip = Variabili_Server.numero_Code_Base_Vip;
            int coda_Costr_Player = player.Code_Costruzione;
            int coda_Reclut_Player = player.Code_Reclutamento;

            bool conferma_Transazione = false; //Impostare via funzioni

            switch (comando)
            {
                case "Vip_1":
                    if (diamanti_Viola < Variabili_Server.Shop.Vip_1.Costo)
                    {
                        Console.WriteLine($"[Shop] Diamanti Viola insufficienti per vip 24H... Richiesta annullata..");
                        Server.Send(player.guid_Player, $"Log_Server|[Shop] Diamanti Viola insufficenti {Variabili_Server.Shop.Vip_1.Costo} per l'acquisto del VIP");
                        return;
                    }
                    if (player.Vip_Tempo + 24 * 60 * 60 > 2 * 24 * 60 * 60) //Max 2gg di accumolo
                    {
                        Console.WriteLine($"[Shop]Tempo VIP oltre il limite massimo di 2 giorni... Richiesta annullata..");
                        Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Vip_Tempo}[/highlight] [highlight]VIP[/highlight] [error]oltre il limite massimo di [highlight]2 giorni[/highlight]... Richiesta annullata..");
                        return;
                    }

                    player.Diamanti_Viola -= (int)Variabili_Server.Shop.Vip_1.Costo;
                    OnEvent(player, QuestEventType.Risorse, "Diamanti Viola", (int)Variabili_Server.Shop.Vip_1.Costo);
                    player.Vip_Tempo += 24 * 60 * 60;
                    player.Vip = true;
                    Console.WriteLine($"[Shop]Acquisto pacchetto VIP 24H... Tempo disponibile: {player.FormatTime(player.Vip_Tempo)}");
                    Server.Send(player.guid_Player, $"Log_Server|[Shop] Hai usato {Variabili_Server.Shop.Vip_1.Costo} Diamanti Viola per l'acquisto del VIP 24H, Tempo disponibile: {player.FormatTime(player.Vip_Tempo)}");
                    player.SetupVillaggioGiocatore();
                    break;
                case "Vip_2":
                    if (player.Vip_Tempo + 24 * 60 * 60 > 2 * 24 * 60 * 60) //Max 2gg di accumolo
                    {
                        Console.WriteLine($"[Shop]Tempo VIP oltre il limite massimo di 5 giorni... Richiesta annullata..");
                        Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Vip_Tempo}[/highlight] [highlight]VIP[/highlight] [error]oltre il limite massimo di [highlight]2 giorni[/highlight]... Richiesta annullata..");
                        return;
                    }

                    //payment_Status_Blockchain = BlockchainManager.Verifica_Pagamento_Vip(player, 2);
                    if (payment_Status_Blockchain == true)
                    {
                        player.Vip_Tempo += 24 * 60 * 60;
                        player.Vip = true;
                        Console.WriteLine($"[Shop] Acquisto Vip 24H... Tempo disponibile: {player.FormatTime(player.Vip_Tempo)}");
                    }
                    break;

                case "GamePass_Base":
                    if (player.GamePass_Base_Tempo + 30 * 24 * 60 * 60 > 6 * 30 * 24 * 60 * 60) //Max 6 mesi di accumolo
                    {
                        Console.WriteLine($"[Shop]Tempo VIP oltre il limite massimo di 5 giorni... Richiesta annullata..");
                        Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Vip_Tempo}[/highlight] [highlight]VIP[/highlight] [error]oltre il limite massimo di [highlight]6 mesi[/highlight]... Richiesta annullata..");
                        return;
                    }
                    DescUpdate(player);
                    player.SetupVillaggioGiocatore();
                    break;
                case "GamePass_Avanzato":
                    if (player.GamePass_Avanzato_Tempo + 30 * 24 * 60 * 60 > 6 * 30 * 24 * 60 * 60) //Max 6 mesi di accumolo
                    {
                        Console.WriteLine($"[Shop]Tempo VIP oltre il limite massimo di 5 giorni... Richiesta annullata..");
                        Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Vip_Tempo}[/highlight] [highlight]VIP[/highlight] [error]oltre il limite massimo di [highlight]6 mesi[/highlight]... Richiesta annullata..");
                        return;
                    }
                    DescUpdate(player);
                    player.SetupVillaggioGiocatore();
                    break;

                case "Costruttori_24H":
                    if (diamanti_Blu < Variabili_Server.Shop.Costruttore_24h.Costo)
                    {
                        Console.WriteLine($"[Shop] Diamanti Blu insufficienti per costruttori 24H... Richiesta annullata..");
                        Server.Send(player.guid_Player, $"Log_Server|[Shop] Diamanti Blu {Variabili_Server.Shop.Costruttore_24h.Costo} insufficienti per 'Costruttori 24H'... Richiesta annullata..");
                        return;
                    }                
                    if ((coda_Costr_Player == numero_Code_Base && player.Vip == false) ||
                        (coda_Costr_Player == numero_Code_Base + numero_Code_Vip && player.Vip == true)) // 
                    {
                        if (player.Costruttori + 24 * 60 * 60 > 4 * 24 * 60 * 60) //Max 5gg di accumolo
                        {
                            Console.WriteLine($"[Shop]Tempo costruttori oltre il limite massimo di 4 giorni... Richiesta annullata..");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Costruttori}[/highlight] [highlight]costruttori[/highlight] [error]oltre il limite massimo di [highlight]4 giorni[/highlight]... Richiesta annullata..");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Costruttore_24h.Costo;
                        player.Costruttori += 24 * 60 * 60;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Costruttore_24h.Costo);

                        if (player.Costruttori > 0)
                        {
                            Console.WriteLine($"[Shop] Tempo costruttori aumentato a: {player.Costruttori} s");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop] Acquisto 'Costruttori 24H' completato {Variabili_Server.Shop.Costruttore_48h.Costo} Diamanti Blu spesi " +
                                $"Tempo costruttori aumentato a: {player.Costruttori} s");
                        }
                        else
                        {
                            if (player.Costruttori == 0 || player.Vip == false) player.Code_Costruzione += 1;
                            Console.WriteLine($"[Shop] Aquisto costruttori completato..." +
                                $"Coda costruzioni aumentato a: {player.Code_Costruzione} per: {player.FormatTime(player.Costruttori)}");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop] Acquisto 'Costruttori 24H' completato {Variabili_Server.Shop.Vip_1.Costo} Diamanti Blu spesi " +
                                $"Coda costruzioni aumentato a: {player.Code_Costruzione} per: {player.FormatTime(player.Costruttori)}");
                        }
                    }                    
                    break;
                case "Costruttori_48H":
                    if (diamanti_Blu < Variabili_Server.Shop.Costruttore_48h.Costo)
                    {
                        Console.WriteLine($"[Shop] Diamanti Blu insufficienti per costruttori 48H... Richiesta annullata..");
                        Server.Send(player.guid_Player, $"Log_Server|[Shop] Diamanti Blu insufficienti {Variabili_Server.Shop.Costruttore_48h.Costo} per 'Costruttori 24H'... Richiesta annullata..");
                        return;
                    }
                    if ((coda_Costr_Player == numero_Code_Base && player.Vip == false) ||
                        (coda_Costr_Player == numero_Code_Base + numero_Code_Vip && player.Vip == true)) // 
                    {
                        if (player.Costruttori + 48 * 60 * 60 > 4 * 24 * 60 * 60) //Max 5gg di accumolo
                        {
                            Console.WriteLine($"[Shop]Tempo costruttori oltre il limite massimo di 4 giorni... Richiesta annullata..");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Costruttori}[/highlight] [highlight]costruttori[/highlight] [error]oltre il limite massimo di [highlight]4 giorni[/highlight]... Richiesta");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Costruttore_24h.Costo;
                        player.Costruttori += 48 * 60 * 60;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Costruttore_48h.Costo);
                        if (player.Costruttori > 0)
                        {
                            Console.WriteLine($"[Shop] Tempo costruttori aumentato a: {player.Costruttori} s");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop]Acquisto 'Costruttori 48H' completato {Variabili_Server.Shop.Costruttore_48h.Costo} Diamanti Blu spesi " +
                                $"Tempo costruttori aumentato a: {player.Costruttori} s");
                        }
                        else
                        {
                            if (player.Costruttori == 0 || player.Vip == false) player.Code_Costruzione += 1;
                            Console.WriteLine($"[Shop] Aquisto costruttori completato 48H..." +
                                $"Coda costruzioni aumentato a: {player.Code_Costruzione} per: {player.FormatTime(player.Costruttori)}");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop]Acquisto 'Costruttori 48H' completato {Variabili_Server.Shop.Costruttore_48h.Costo} Diamanti Blu spesi " +
                                $"Coda costruzioni aumentato a: {player.Code_Costruzione} per: {player.FormatTime(player.Costruttori)}");
                        }
                    }
                    break;
                case "Reclutatori_24H":
                    if (diamanti_Blu < Variabili_Server.Shop.Reclutatore_24h.Costo)
                    {
                        Console.WriteLine($"[Shop] Diamanti Blu insufficienti per reclutatori 24H... Richiesta annullata..");
                        Server.Send(player.guid_Player, $"Log_Server|[Shop] Diamanti Blu insufficienti {Variabili_Server.Shop.Reclutatore_24h.Costo} per 'Reclutatori 24H'... Richiesta annullata..");
                        return;
                    }
                    if ((coda_Reclut_Player == numero_Code_Base && player.Vip == false) ||
                        (coda_Reclut_Player == numero_Code_Base + numero_Code_Vip && player.Vip == true)) // 
                    {
                        if (player.Reclutatori + 24 * 60 * 60 > 4 * 24 * 60 * 60) //Max 5gg di accumolo
                        {
                            Console.WriteLine($"[Shop]Tempo costruttori oltre il limite massimo di 4 giorni... Richiesta annullata..");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Reclutatori}[/highlight] [highlight]reclutatori[/highlight] [error]oltre il limite massimo di [highlight]4 giorni[/highlight]... Richiesta");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Reclutatore_24h.Costo;
                        player.Reclutatori += 24 * 60 * 60;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Reclutatore_24h.Costo);

                        if (player.Reclutatori > 0)
                        {
                            Console.WriteLine($"[Shop] Tempo reclutatori aumentato a: {player.Reclutatori} s");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop] Acquisto 'Reclutatori 24H' completato {Variabili_Server.Shop.Reclutatore_24h.Costo} Diamanti Blu spesi " +
                                $"Tempo reclutatori aumentato a: {player.Reclutatori} s");
                        }
                        else
                        {
                            if (player.Reclutatori == 0 || player.GamePass_Base == false) player.Code_Reclutamento += 1;
                            Console.WriteLine($"[Shop] Aquisto reclutatori completato..." +
                                $"Coda reclutatori aumentato a: {player.Code_Reclutamento} per: {player.FormatTime(player.Reclutatori)}");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop] Acquisto 'Reclutatori 24H' completato {Variabili_Server.Shop.Reclutatore_24h.Costo} Diamanti Blu spesi " +
                                $"Coda reclutatori aumentato a: {player.Code_Reclutamento} per: {player.FormatTime(player.Reclutatori)}");
                        }
                    }
                    break;
                case "Reclutatori_48H":
                    if (diamanti_Blu < Variabili_Server.Shop.Reclutatore_48h.Costo)
                    {
                        Console.WriteLine($"[Shop] Diamanti Blu insufficienti per reclutatori 48H... Richiesta annullata..");
                        Server.Send(player.guid_Player, $"Log_Server|[Shop]  Diamanti Blu insufficienti {Variabili_Server.Shop.Reclutatore_48h.Costo} per 'Reclutatori 48H'... Richiesta annullata..");
                        return;
                    }
                    if ((coda_Reclut_Player == numero_Code_Base && player.Vip == false) ||
                        (coda_Reclut_Player == numero_Code_Base + numero_Code_Vip && player.Vip == true)) // 
                    {
                        if (player.Reclutatori + 48 * 60 * 60 > 4 * 24 * 60 * 60) //Max 5gg di accumolo
                        {
                            Console.WriteLine($"[Shop]Tempo costruttori oltre il limite massimo di 5 giorni... Richiesta annullata..");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Reclutatori}[/highlight] [highlight]reclutatori[/highlight] [error]oltre il limite massimo di [highlight]4 giorni[/highlight]... Richiesta");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Reclutatore_48h.Costo;
                        player.Reclutatori += 48 * 60 * 60;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Reclutatore_48h.Costo);

                        if (player.Reclutatori > 0)
                        {
                            Console.WriteLine($"[Shop] Tempo reclutatori aumentato a: {player.FormatTime(player.Reclutatori)}");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop] Acquisto 'Reclutatori 48H' completato {Variabili_Server.Shop.Reclutatore_48h.Costo} Diamanti Blu spesi " +
                                $"Tempo reclutatori aumentato a: {player.FormatTime(player.Reclutatori)}");
                        }
                        else
                        {
                            if (player.Reclutatori == 0 || player.GamePass_Base == false) player.Code_Reclutamento += 1;
                            Console.WriteLine($"[Shop] Aquisto reclutatori completato..." +
                                $"Coda reclutatori aumentato a: {player.Code_Reclutamento} per: {player.FormatTime(player.Reclutatori)}");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop] Acquisto 'Reclutatori 48H' completato {Variabili_Server.Shop.Reclutatore_48h.Costo} Diamanti Blu spesi " +
                                $"Coda reclutatori aumentato a: {player.Code_Reclutamento} per: {player.FormatTime(player.Reclutatori)}");
                        }
                    }
                    break;

                case "Scudo_Pace_8H":
                    if (diamanti_Blu >= Variabili_Server.Shop.Scudo_Pace_8h.Costo)
                    {
                        if (player.ScudoDellaPace + 8 * 60 * 60 > 7 * 24 * 60 * 60) //Max 5gg di accumolo
                        {
                            Console.WriteLine($"[Shop]Tempo costruttori oltre il limite massimo di 5 giorni... Richiesta annullata..");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.ScudoDellaPace}[/highlight] [highlight]scudo della pace[/highlight] [error]oltre il limite massimo di [highlight]7 giorni[/highlight]... Richiesta");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Scudo_Pace_8h.Costo;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Scudo_Pace_8h.Costo);
                        player.ScudoDellaPace += 8 * 60 * 60; //Riduzione ogni tick (1000 = 1s) --> potrebbe essere simile anche per gli altri due timer (costr. addestr.)
                        Console.WriteLine($"[Shop] Scudo della pace attivo, Diamanti Blu tulizzati {Variabili_Server.Shop.Scudo_Pace_8h.Costo}, Nuovo tempo: [{player.FormatTime(player.ScudoDellaPace)}]");
                        Server.Send(player.guid_Player, $"Log_Server|[Shop] Scudo della pace attivo, Diamanti Blu tulizzati {Variabili_Server.Shop.Scudo_Pace_8h.Costo}, Nuovo tempo: [{player.FormatTime(player.ScudoDellaPace)}]");
                    }
                    break;
                case "Scudo_Pace_24H":
                    if (diamanti_Blu >= Variabili_Server.Shop.Scudo_Pace_24h.Costo)
                    {
                        if (player.ScudoDellaPace + 24 * 60 * 60 > 7 * 24 * 60 * 60) //Max 5gg di accumolo
                        {
                            Console.WriteLine($"[Shop]Tempo costruttori oltre il limite massimo di 5 giorni... Richiesta annullata..");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.ScudoDellaPace}[/highlight] [highlight]scudo della pace[/highlight] [error]oltre il limite massimo di [highlight]7 giorni[/highlight]... Richiesta");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Scudo_Pace_24h.Costo;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Scudo_Pace_24h.Costo);
                        player.ScudoDellaPace += 24 * 60 * 60;
                        Console.WriteLine($"[Shop] Scudo della pace attivo, Diamanti Blu tulizzati {Variabili_Server.Shop.Scudo_Pace_24h.Costo}, Nuovo tempo: [Player.FormatTime({player.FormatTime(player.ScudoDellaPace)}]");
                        Server.Send(player.guid_Player, $"Log_Server|[Shop] Scudo della pace attivo, Diamanti Blu tulizzati {Variabili_Server.Shop.Scudo_Pace_24h.Costo}, Nuovo tempo: [{player.FormatTime(player.ScudoDellaPace)}]");
                    }
                    break;
                case "Scudo_Pace_72H":
                    if (diamanti_Blu >= Variabili_Server.Shop.Scudo_Pace_72h.Costo)
                    {
                        if (player.ScudoDellaPace + 72 * 60 * 60 > 7 * 24 * 60 * 60) //Max 5gg di accumolo
                        {
                            Console.WriteLine($"[Shop]Tempo costruttori oltre il limite massimo di 5 giorni... Richiesta annullata..");
                            Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.ScudoDellaPace}[/highlight] [highlight]scudo della pace[/highlight] [error]oltre il limite massimo di [highlight]7 giorni[/highlight]... Richiesta");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Scudo_Pace_72h.Costo;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Scudo_Pace_72h.Costo);
                        player.ScudoDellaPace += 72 * 60 * 60;
                        Console.WriteLine($"[Shop] Scudo della pace attivo, Diamanti Blu tulizzati {Variabili_Server.Shop.Scudo_Pace_72h.Costo}, Nuovo tempo: [{player.FormatTime(player.ScudoDellaPace)}]");
                        Server.Send(player.guid_Player, $"Log_Server|[Shop] Scudo della pace attivo, Diamanti Blu tulizzati {Variabili_Server.Shop.Scudo_Pace_72h.Costo}, Nuovo tempo: [{player.FormatTime(player.ScudoDellaPace)}]");
                    }
                    break;
                case "Pacchetto_1":

                    // Procedere alla richiesta transazione USDT da parte dell'utente

                    //Confermare l'acquisto
                    //Accreditare i diamanti
                    if (conferma_Transazione == true)
                        player.Diamanti_Viola += Variabili_Server.Shop.Pacchetto_Diamanti_1.Reward;

                    break;
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
        public static async void DescUpdate(Player player)
        {
            Server.Send(player.guid_Player, $"Descrizione|Fattoria|[black]" +
                $"La fattoria è la struttura principale per la produzione di Cibo, fondamentale per la costruzione di edifici militari e civili, " +
                $"l'addestramento delle unità militari ed il loro mantenimento. Indispensabile per la ricerca tecnologica e di componenti militari\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.Fattoria.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.Fattoria.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.Fattoria.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.Fattoria.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.Fattoria.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.Fattoria.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.Fattoria.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.Fattoria.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.Fattoria.TempoCostruzione)})\n" +
                $"Produzione risorse: [icon:cibo]{(Strutture.Edifici.Fattoria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Cibo * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"Limite stoccaggio: [icon:cibo][ferroScuro]{Strutture.Edifici.Fattoria.Limite.ToString()}");

             Server.Send(player.guid_Player, $"Descrizione|Segheria|[black]" +
                $"La Segheria è la struttura principale per la produzione di Legna, fondamentale per la costruzione di strutture militari, civili e " +
                $"l'addestramento delle unità militari. Indispensabile per la ricerca tecnologica e di componenti militari\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.Segheria.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.Segheria.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.Segheria.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.Segheria.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.Segheria.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.Segheria.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.Segheria.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.Segheria.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.Segheria.TempoCostruzione)})\n" +
                $"Produzione risorse: [icon:legno]{(Strutture.Edifici.Segheria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Legno * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"Limite stoccaggio: [icon:legno][ferroScuro]{Strutture.Edifici.Segheria.Limite.ToString()}");

            Server.Send(player.guid_Player, $"Descrizione|Cava di Pietra|[black]" +
                $"La cava di pietra è la struttura principale per la produzione di Pietra, fondamentale per la costruzione di strutture militari, cilivi e " +
                $"l'addestramento delle unità militari. Indispensabile per la ricerca tecnologica e di componenti militari\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.CavaPietra.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.CavaPietra.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.CavaPietra.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.CavaPietra.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.CavaPietra.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.CavaPietra.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.CavaPietra.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.CavaPietra.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.CavaPietra.TempoCostruzione)})\n" +
                $"Produzione risorse: [icon:pietra]{(Strutture.Edifici.CavaPietra.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Pietra * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"Limite stoccaggio: [icon:pietra][ferroScuro]{Strutture.Edifici.CavaPietra.Limite.ToString()}\n");

            Server.Send(player.guid_Player, $"Descrizione|Miniera di Ferro|[black]" +
                $"La Miniera di ferro è la struttura principale per la produzione di Ferro, fondamentale per la costruzione di strutture militari, cilivi e " +
                $"l'addestramento delle unità militari. Indispensabile per la ricerca tecnologica e di componenti militari\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.MinieraFerro.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.MinieraFerro.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.MinieraFerro.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.MinieraFerro.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.MinieraFerro.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.MinieraFerro.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.MinieraFerro.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.MinieraFerro.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.MinieraFerro.TempoCostruzione)})\n" +
                $"Produzione risorse: [icon:ferro]{(Strutture.Edifici.MinieraFerro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Ferro * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"Limite stoccaggio: [icon:ferro][ferroScuro]{Strutture.Edifici.MinieraFerro.Limite.ToString()}");

            Server.Send(player.guid_Player, $"Descrizione|Miniera d'Oro|[black]" +
                $"La miniera d'oro è la struttura principale per la produzione dell'Oro, fondamentale per la costruzione di strutture militari, civili e " +
                $"l'addestramento delle unità militari. Indispensabile per la ricerca tecnologica e di componenti militari\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.MinieraOro.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.MinieraOro.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.MinieraOro.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.MinieraOro.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.MinieraOro.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.MinieraOro.Popolazione.ToString("#,0")}\n" +
               $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.MinieraOro.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.MinieraOro.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.MinieraOro.TempoCostruzione)})\n" +
                $"Produzione risorse: [icon:oro]{(Strutture.Edifici.MinieraOro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Oro * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"Limite stoccaggio: [icon:oro][ferroScuro]{Strutture.Edifici.MinieraOro.Limite.ToString()}\n");

            Server.Send(player.guid_Player, $"Descrizione|Case|[black]" +
                $"Le Case sono necessarie per attirare sempre più cittadini presso il vostro villaggio, " +
                $"sono fondamentali per la costruzione di strutture militari e civili, oltre che per addestrare le unità militari.\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.Case.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.Case.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.Case.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.Case.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.Case.Oro.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.Case.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.Case.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.Case.TempoCostruzione)})\n" +
                $"Produzione risorse: [icon:popolazione]{(Strutture.Edifici.Case.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Popolazione).ToString("0.00")} s\n" +
                $"Limite abitanti: [icon:popolazione][ferroScuro]{Strutture.Edifici.Case.Limite.ToString()}\n");

            Server.Send(player.guid_Player, $"Descrizione|Produzione Spade|[black]" +
                 $"Workshop Spade questa struttura produce equipaggiamento militare specifico, " +
                 $"essenziali per l'addestramento di unità militari, questa struttura produce Spade.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.ProduzioneSpade.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.ProduzioneSpade.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.ProduzioneSpade.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.ProduzioneSpade.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.ProduzioneSpade.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.ProduzioneSpade.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.ProduzioneSpade.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.ProduzioneSpade.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.ProduzioneSpade.TempoCostruzione)})\n" +
                 $"Produzione risorse: [icon:spade]{(Strutture.Edifici.ProduzioneSpade.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Spade).ToString("0.00")} s\n" +
                 $"Limite spade [icon:spade][ferroScuro]{Strutture.Edifici.ProduzioneSpade.Limite.ToString()}[black]\n" +
                 $"Consumo legno: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneSpade.Consumo_Legno}[black] s\n" +
                 $"Consumo ferro: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneSpade.Consumo_Ferro}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneSpade.Consumo_Oro}[black] s\n");

             Server.Send(player.guid_Player, $"Descrizione|Produzione Lance|[black]" +
                 $"Workshop Lance questa struttura produce equipaggiamento militare specifico, " +
                 $"essenziali per l'addestramento di unità militari, questa struttura produce Lance.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.ProduzioneLance.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.ProduzioneLance.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.ProduzioneLance.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.ProduzioneLance.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.ProduzioneLance.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.ProduzioneLance.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.ProduzioneLance.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.ProduzioneLance.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.ProduzioneLance.TempoCostruzione)})\n" +
                 $"Produzione risorse: [icon:lance]{(Strutture.Edifici.ProduzioneLance.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Lance).ToString("0.00")} s\n" +
                 $"Limite lance [icon:lance][ferroScuro]{Strutture.Edifici.ProduzioneLance.Limite.ToString()}[black]\n" +
                 $"Consumo legno: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneLance.Consumo_Legno}[black] s\n" +
                 $"Consumo ferro: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneLance.Consumo_Ferro}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneLance.Consumo_Oro}[black] s\n");

             Server.Send(player.guid_Player, $"Descrizione|Produzione Archi|[black]" +
                 $"Workshop Archi questa struttura produce equipaggiamento militare specifico, " +
                 $"essenziali per l'addestramento di unità militari, questa struttura produce Archi.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.ProduzioneArchi.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.ProduzioneArchi.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.ProduzioneArchi.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.ProduzioneArchi.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.ProduzioneArchi.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:archi]{Strutture.Edifici.ProduzioneArchi.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.ProduzioneArchi.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.ProduzioneArchi.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.ProduzioneArchi.TempoCostruzione)})\n" +
                 $"Produzione risorse: [icon:archi]{(Strutture.Edifici.ProduzioneArchi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Archi).ToString("0.00")} s\n" +
                 $"Limite archi [icon:archi][ferroScuro]{Strutture.Edifici.ProduzioneArchi.Limite.ToString()}[black]\n" +
                 $"Consumo legno: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneArchi.Consumo_Legno}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneArchi.Consumo_Oro}[black] s\n");

             Server.Send(player.guid_Player, $"Descrizione|Produzione Scudi|[black]" +
                 $"Workshop Scudi questa struttura produce equipaggiamento militare specifico, " +
                 $"essenziali per l'addestramento di unità militari, questa struttura produce Scudi.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.ProduzioneScudi.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.ProduzioneScudi.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.ProduzioneScudi.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.ProduzioneScudi.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.ProduzioneScudi.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.ProduzioneScudi.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.ProduzioneScudi.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.ProduzioneScudi.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.ProduzioneScudi.TempoCostruzione)})\n" +
                 $"Produzione risorse: [icon:scudi]{(Strutture.Edifici.ProduzioneScudi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Scudi).ToString("0.00")} s\n" +
                 $"Limite scudi [icon:scudi][ferroScuro]{Strutture.Edifici.ProduzioneScudi.Limite.ToString()}[black]\n" +
                 $"Consumo legno: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneScudi.Consumo_Legno}[black] s\n" +
                 $"Consumo ferro: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneScudi.Consumo_Ferro}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneScudi.Consumo_Oro}[black] s\n");

             Server.Send(player.guid_Player, $"Descrizione|Produzione Armature|[black]" +
                 $"Workshop Armature questa struttura produce equipaggiamento militare specifico, " +
                 $"essenziali per l'addestramento di unità militari, questa struttura produce Armature.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.ProduzioneArmature.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.ProduzioneArmature.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.ProduzioneArmature.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.ProduzioneArmature.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.ProduzioneArmature.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.ProduzioneArmature.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.ProduzioneArmature.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.ProduzioneArmature.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.ProduzioneArmature.TempoCostruzione)})\n" +
                 $"Produzione risorse: [icon:armature]{(Strutture.Edifici.ProduzioneArmature.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Armature).ToString("0.00")} s\n" +
                 $"Limite armature: [icon:armature][ferroScuro]{Strutture.Edifici.ProduzioneArmature.Limite.ToString()}[black]\n" +
                 $"Consumo ferro: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneArmature.Consumo_Ferro}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneArmature.Consumo_Oro}[black] s");

             Server.Send(player.guid_Player, $"Descrizione|Produzione Frecce|[black]" +
                 $"Workshop Frecce questa struttura produce equipaggiamento militare specifico, " +
                 $"essenziali per l'addestramento di unità militari, questa struttura produce Frecce.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.ProduzioneFrecce.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.ProduzioneFrecce.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.ProduzioneFrecce.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.ProduzioneFrecce.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.ProduzioneFrecce.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.ProduzioneFrecce.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.ProduzioneFrecce.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.ProduzioneFrecce.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.ProduzioneFrecce.TempoCostruzione)})\n" +
                 $"Produzione risorse: [icon:frecce]{(Strutture.Edifici.ProduzioneFrecce.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Oro).ToString("0.00")} s\n" +
                 $"Limite frecce [icon:frecce][ferroScuro]{Strutture.Edifici.ProduzioneFrecce.Limite.ToString()}[black]\n" +
                 $"Consumo legno: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneFrecce.Consumo_Legno.ToString()}[black] s\n" +
                 $"Consumo pietra: [icon:pietra][rosso]-{Strutture.Edifici.ProduzioneFrecce.Consumo_Pietra.ToString()}[black] s\n" +
                 $"Consumo ferro: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneFrecce.Consumo_Ferro.ToString()}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneFrecce.Consumo_Ferro.ToString()}[black] s\n");

            //Esercito
            Server.Send(player.guid_Player, $"Descrizione|Guerrieri 1|[black]" +
                $"I guerrieri I sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono, " +
                $"sono facili da reclutare e non sono molto dispendioni in cibo ed oro.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Guerriero_1.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Guerriero_1.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_1.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_1.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Guerriero_1.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Guerriero_1.Spade.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Guerriero_1.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_1.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Guerriero_1.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Guerriero_1.TempoReclutamento.ToString()}) \n" +
                $"Mantenimento cibo: [icon:cibo][rosso]-{Esercito.Unità.Guerriero_1.Cibo.ToString()}[black] s\n" +
                $"Mantenimento oro: [icon:oro][rosso]-{Esercito.Unità.Guerriero_1.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Guerriero_1.Salute.ToString("#,0")} [black]+ [verde]{player.Guerriero_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Guerriero_1.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Guerriero_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Guerriero_1.Attacco.ToString("#,0")} [black]+ [rosso]{player.Guerriero_Attacco}[black]\n");

            Server.Send(player.guid_Player, $"Descrizione|Guerrieri 2|[black]" +
                $"I guerrieri II sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono, " +
                $"sono facili da reclutare e non sono molto dispendioni in cibo ed oro.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Guerriero_2.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Guerriero_2.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_2.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_2.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Guerriero_2.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Guerriero_2.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Guerriero_2.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Guerriero_2.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Guerriero_2.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Guerriero_2.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_2.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Guerriero_2.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Guerriero_2.TempoReclutamento.ToString()})\n" +
                $"Mantenimento cibo: [icon:cibo][rosso]-{Esercito.Unità.Guerriero_2.Cibo.ToString()}[black] s\n" +
                $"Mantenimento oro: [icon:oro][rosso]-{Esercito.Unità.Guerriero_2.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Guerriero_2.Salute.ToString("#,0")} [black]+ [verde]{player.Guerriero_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Guerriero_2.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Guerriero_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Guerriero_2.Attacco.ToString("#,0")} [black]+ [rosso]{player.Guerriero_Attacco}[black]\n");

            Server.Send(player.guid_Player, $"Descrizione|Guerrieri 3|[black]" +
                $"I guerrieri III sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono, " +
                $"sono facili da reclutare e non sono molto dispendioni in cibo ed oro.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Guerriero_3.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Guerriero_3.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_3.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_3.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Guerriero_3.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Guerriero_3.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Guerriero_3.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Guerriero_3.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Guerriero_3.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Guerriero_3.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_3.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Guerriero_3.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Guerriero_3.TempoReclutamento.ToString()})\n" +
                $"Mantenimento cibo: [icon:cibo][rosso]-{Esercito.Unità.Guerriero_3.Cibo.ToString()}[black] s\n" +
                $"Mantenimento oro: [icon:oro][rosso]-{Esercito.Unità.Guerriero_3.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Guerriero_3.Salute.ToString("#,0")} [black]+ [verde]{player.Guerriero_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Guerriero_3.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Guerriero_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Guerriero_3.Attacco.ToString("#,0")} [black]+ [rosso]{player.Guerriero_Attacco}[black]\n");

            Server.Send(player.guid_Player, $"Descrizione|Guerrieri 4|[black]" +
                $"I guerrieri IV sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono, " +
                $"sono facili da reclutare e non sono molto dispendioni in cibo ed oro.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Guerriero_4.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Guerriero_4.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_4.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_4.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Guerriero_4.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Guerriero_4.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Guerriero_4.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Guerriero_4.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Guerriero_4.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Guerriero_4.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_4.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Guerriero_4.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Guerriero_4.TempoReclutamento.ToString()})\n" +
                $"Mantenimento cibo: [icon:cibo][rosso]-{Esercito.Unità.Guerriero_4.Cibo.ToString()}[black] s\n" +
                $"Mantenimento oro: [icon:oro][rosso]-{Esercito.Unità.Guerriero_4.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Guerriero_4.Salute.ToString("#,0")} [black]+ [verde]{player.Guerriero_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Guerriero_4.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Guerriero_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Guerriero_4.Attacco.ToString("#,0")} [black]+ [rosso]{player.Guerriero_Attacco}[black]\n");

            Server.Send(player.guid_Player, $"Descrizione|Guerrieri 5|[black]" +
                $"I guerrieri V sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono, " +
                $"sono facili da reclutare e non sono molto dispendioni in cibo ed oro.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Guerriero_5.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Guerriero_5.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_5.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_5.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Guerriero_5.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Guerriero_5.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Guerriero_5.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Guerriero_5.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Guerriero_5.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Guerriero_5.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_5.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Guerriero_5.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Guerriero_5.TempoReclutamento.ToString()})\n" +
                $"Mantenimento cibo: [icon:cibo][rosso]-{Esercito.Unità.Guerriero_5.Cibo.ToString()}[black] s\n" +
                $"Mantenimento oro: [icon:oro][rosso]-{Esercito.Unità.Guerriero_5.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Guerriero_5.Salute.ToString("#,0")} [black]+ [verde]{player.Guerriero_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Guerriero_5.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Guerriero_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Guerriero_5.Attacco.ToString("#,0")} [black]+ [rosso]{player.Guerriero_Attacco}[black]\n");

            Server.Send(player.guid_Player, $"Descrizione|Lanceri 1|[black]" +
                $"I lanceri I sono la spina dorsale di ogni esercito ben organizzato. Armati di lance, " +
                $"questi soldati costituiscono un baluardo formidabile contro gli assalti nemici.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Lancere_1.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Lancere_1.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Lancere_1.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Lancere_1.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Lancere_1.Oro.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Lancere_1.Lance.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Lancere_1.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Lancere_1.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_1.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Lancere_1.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Lancere_1.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Lancere_1.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Lancere_1.Salario.ToString()}[black] s\n \n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Lancere_1.Salute.ToString("#,0")} [black]+ [verde]{player.Lancere_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Lancere_1.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Lancere_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Lancere_1.Attacco.ToString("#,0")} [black]+ [rosso]{player.Lancere_Attacco}[black]\n");

            Server.Send(player.guid_Player, $"Descrizione|Lanceri 2|[black]" +
                $"I lanceri II sono la spina dorsale di ogni esercito ben organizzato. Armati di lance, " +
                $"questi soldati costituiscono un baluardo formidabile contro gli assalti nemici.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Lancere_2.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:cilegnobo]{Esercito.CostoReclutamento.Lancere_2.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Lancere_2.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Lancere_2.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Lancere_2.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Lancere_2.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Lancere_2.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Lancere_2.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Lancere_2.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Lancere_2.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_2.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Lancere_2.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Lancere_2.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Lancere_2.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Lancere_2.Salario.ToString()}[black] s\n \n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Lancere_2.Salute.ToString("#,0")} [black]+ [verde]{player.Lancere_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Lancere_2.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Lancere_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Lancere_2.Attacco.ToString("#,0")} [black]+ [rosso]{player.Lancere_Attacco}[black]\n");

            Server.Send(player.guid_Player, $"Descrizione|Lanceri 3|[black]" +
                $"I lanceri III sono la spina dorsale di ogni esercito ben organizzato. Armati di lance, " +
                $"questi soldati costituiscono un baluardo formidabile contro gli assalti nemici.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Lancere_3.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:cilegnobo]{Esercito.CostoReclutamento.Lancere_3.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Lancere_3.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Lancere_3.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Lancere_3.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Lancere_3.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Lancere_3.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Lancere_3.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Lancere_3.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Lancere_3.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_3.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Lancere_3.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Lancere_3.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Lancere_3.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Lancere_3.Salario.ToString()}[black] s\n \n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Lancere_3.Salute.ToString("#,0")} [black]+ [verde]{player.Lancere_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Lancere_3.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Lancere_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Lancere_3.Attacco.ToString("#,0")} [black]+ [rosso]{player.Lancere_Attacco}[black]\n");

            Server.Send(player.guid_Player, $"Descrizione|Lanceri 4|[black]" +
                $"I lanceri IV sono la spina dorsale di ogni esercito ben organizzato. Armati di lance, " +
                $"questi soldati costituiscono un baluardo formidabile contro gli assalti nemici.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Lancere_4.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:cilegnobo]{Esercito.CostoReclutamento.Lancere_4.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Lancere_4.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Lancere_4.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Lancere_4.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Lancere_4.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Lancere_4.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Lancere_4.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Lancere_4.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Lancere_4.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_1.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Lancere_4.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Lancere_4.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Lancere_4.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Lancere_4.Salario.ToString()}[black] s\n \n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Lancere_4.Salute.ToString("#,0")} [black]+ [verde]{player.Lancere_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Lancere_4.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Lancere_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Lancere_4.Attacco.ToString("#,0")} [black]+ [rosso]{player.Lancere_Attacco}[black]\n");

            Server.Send(player.guid_Player, $"Descrizione|Lanceri 5|[black]" +
                $"I lanceri V sono la spina dorsale di ogni esercito ben organizzato. Armati di lance, " +
                $"questi soldati costituiscono un baluardo formidabile contro gli assalti nemici.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Lancere_5.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:cilegnobo]{Esercito.CostoReclutamento.Lancere_5.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Lancere_5.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Lancere_5.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Lancere_5.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Lancere_5.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Lancere_5.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Lancere_5.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Lancere_5.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Lancere_5.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_5.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Lancere_5.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Lancere_5.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Lancere_5.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Lancere_5.Salario.ToString()}[black] s\n \n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Lancere_5.Salute.ToString("#,0")} [black]+ [verde]{player.Lancere_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Lancere_5.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Lancere_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Lancere_5.Attacco.ToString("#,0")} [black]+ [rosso]{player.Lancere_Attacco}[black]\n");

            Server.Send(player.guid_Player, $"Descrizione|Arceri 1|[black]" +
                $"Gli arceri I armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza, " +
                $"scagliando frecce mortali sulle linee nemiche, prima che possano avvicinarsi.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Arcere_1.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Arcere_1.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Arcere_1.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Arcere_1.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Arcere_1.Oro.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Arcere_1.Archi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Arcere_1.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_1.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Arcere_1.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Arcere_1.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Arcere_1.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Arcere_1.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Arcere_1.Salute.ToString("#,0")} [black]+ [verde]{player.Arcere_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Arcere_1.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Arcere_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Arcere_1.Attacco.ToString("#,0")} [black]+ [rosso]{player.Arcere_Attacco}[black]");

            Server.Send(player.guid_Player, $"Descrizione|Arceri 2|[black]" +
                $"Gli arceri II armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza, " +
                $"scagliando frecce mortali sulle linee nemiche, prima che possano avvicinarsi.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Arcere_2.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Arcere_2.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Arcere_2.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Arcere_2.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Arcere_2.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Arcere_2.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Arcere_2.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Arcere_2.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Arcere_2.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Arcere_2.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_2.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Arcere_2.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Arcere_2.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Arcere_2.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Arcere_2.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Arcere_2.Salute.ToString("#,0")} [black]+ [verde]{player.Arcere_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Arcere_2.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Arcere_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Arcere_2.Attacco.ToString("#,0")} [black]+ [rosso]{player.Arcere_Attacco}[black]");

            Server.Send(player.guid_Player, $"Descrizione|Arceri 3|[black]" +
                $"Gli arceri III armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza, " +
                $"scagliando frecce mortali sulle linee nemiche, prima che possano avvicinarsi.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Arcere_3.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Arcere_3.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Arcere_3.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Arcere_3.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Arcere_3.Oro.ToString("#,0")}" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Arcere_3.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Arcere_3.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Arcere_3.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Arcere_3.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Arcere_3.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_3.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Arcere_3.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Arcere_3.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Arcere_3.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Arcere_3.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Arcere_3.Salute.ToString("#,0")} [black]+ [verde]{player.Arcere_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Arcere_3.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Arcere_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Arcere_3.Attacco.ToString("#,0")} [black]+ [rosso]{player.Arcere_Attacco}[black]");

            Server.Send(player.guid_Player, $"Descrizione|Arceri 4|[black]" +
                $"Gli arceri IV armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza, " +
                $"scagliando frecce mortali sulle linee nemiche, prima che possano avvicinarsi.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Arcere_4.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Arcere_4.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Arcere_4.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Arcere_4.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Arcere_4.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Arcere_4.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Arcere_4.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Arcere_4.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Arcere_4.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Arcere_4.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_4.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Arcere_4.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Arcere_4.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Arcere_4.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Arcere_4.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Arcere_4.Salute.ToString("#,0")} [black]+ [verde]{player.Arcere_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Arcere_4.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Arcere_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Arcere_4.Attacco.ToString("#,0")} [black]+ [rosso]{player.Arcere_Attacco}[black]");

            Server.Send(player.guid_Player, $"Descrizione|Arceri 5|[black]" +
                $"Gli arceri V armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza, " +
                $"scagliando frecce mortali sulle linee nemiche, prima che possano avvicinarsi.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Arcere_5.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Arcere_5.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Arcere_5.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Arcere_5.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Arcere_5.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Arcere_5.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Arcere_5.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Arcere_5.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Arcere_5.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Arcere_5.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_5.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Arcere_5.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Arcere_5.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Arcere_5.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Arcere_5.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Arcere_5.Salute.ToString("#,0")} [black]+ [verde]{player.Arcere_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Arcere_5.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Arcere_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Arcere_5.Attacco.ToString("#,0")} [black]+ [rosso]{player.Arcere_Attacco}[black]");

            Server.Send(player.guid_Player, $"Descrizione|Catapulte 1|[black]" +
                $"Le catapulte I sono potenti macchine d'assedio che cambiano le sorti delle battaglie, " +
                $"scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Catapulta_1.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Catapulta_1.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:cipietrabo]{Esercito.CostoReclutamento.Catapulta_1.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_1.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Catapulta_1.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Catapulta_1.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Catapulta_1.Lance.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_1.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Catapulta_1.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_1.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Catapulta_1.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Catapulta_1.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Catapulta_1.Cibo.ToString("0.00")}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Catapulta_1.Salario.ToString("0.00")}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Catapulta_1.Salute.ToString("#,0")} [black]+ [verde]{player.Catapulta_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Catapulta_1.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Catapulta_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Catapulta_1.Attacco.ToString("#,0")} [black]+ [rosso]{player.Catapulta_Attacco}[black]");

            Server.Send(player.guid_Player, $"Descrizione|Catapulte 2|[black]" +
                $"Le catapulte II sono potenti macchine d'assedio che cambiano le sorti delle battaglie, " +
                $"scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Catapulta_2.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Catapulta_2.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:cipietrabo]{Esercito.CostoReclutamento.Catapulta_2.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_2.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Catapulta_2.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Catapulta_2.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Catapulta_2.Lance.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_2.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Catapulta_2.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_2.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Catapulta_2.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Catapulta_2.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Catapulta_2.Cibo.ToString("0.00")}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Catapulta_2.Salario.ToString("0.00")}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Catapulta_2.Salute.ToString("#,0")} [black]+ [verde]{player.Catapulta_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Catapulta_2.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Catapulta_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Catapulta_2.Attacco.ToString("#,0")} [black]+ [rosso]{player.Catapulta_Attacco}[black]");

            Server.Send(player.guid_Player, $"Descrizione|Catapulte 3|[black]" +
                $"Le catapulte III sono potenti macchine d'assedio che cambiano le sorti delle battaglie, " +
                $"scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Catapulta_3.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Catapulta_3.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:cipietrabo]{Esercito.CostoReclutamento.Catapulta_3.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_3.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Catapulta_3.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Catapulta_3.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Catapulta_3.Lance.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_3.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Catapulta_3.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_3.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Catapulta_3.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Catapulta_3.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Catapulta_3.Cibo.ToString("0.00")}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Catapulta_3.Salario.ToString("0.00")}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Catapulta_3.Salute.ToString("#,0")} [black]+ [verde]{player.Catapulta_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Catapulta_3.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Catapulta_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Catapulta_3.Attacco.ToString("#,0")} [black]+ [rosso]{player.Catapulta_Attacco}[black]");

            Server.Send(player.guid_Player, $"Descrizione|Catapulte 4|[black]" +
                $"Le catapulte IV sono potenti macchine d'assedio che cambiano le sorti delle battaglie, " +
                $"scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Catapulta_4.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Catapulta_4.Legno.ToString("#,0")}" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Catapulta_4.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_4.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Catapulta_4.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Catapulta_4.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Catapulta_4.Lance.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_4.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Catapulta_4.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_4.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Catapulta_4.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Catapulta_4.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Catapulta_4.Cibo.ToString("0.00")}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Catapulta_4.Salario.ToString("0.00")}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Catapulta_4.Salute.ToString("#,0")} [black]+ [verde]{player.Catapulta_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Catapulta_4.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Catapulta_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Catapulta_4.Attacco.ToString("#,0")} [black]+ [rosso]{player.Catapulta_Attacco}[black]");

            Server.Send(player.guid_Player, $"Descrizione|Catapulte 5|[black]" +
                $"Le catapulte V sono potenti macchine d'assedio che cambiano le sorti delle battaglie, " +
                $"scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Catapulta_5.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Catapulta_5.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Catapulta_5.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_5.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Catapulta_5.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Catapulta_5.Spade.ToString("#,0")}\n" +
                $"Lance: [icon:lance]{Esercito.CostoReclutamento.Catapulta_5.Lance.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_5.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Catapulta_5.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_5.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{(Esercito.CostoReclutamento.Catapulta_5.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1)).ToString()} s ({Esercito.CostoReclutamento.Catapulta_5.TempoReclutamento.ToString()})\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Catapulta_5.Cibo.ToString("0.00")}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Catapulta_5.Salario.ToString("0.00")}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"Salute:  [verde]{Esercito.Unità.Catapulta_5.Salute.ToString("#,0")} [black]+ [verde]{player.Catapulta_Salute}[black]\n" +
                $"Difesa:  [bluGotico]{Esercito.Unità.Catapulta_5.Difesa.ToString("#,0")} [black]+ [bluGotico]{player.Catapulta_Difesa}[black]\n" +
                $"Attacco: [rosso]{Esercito.Unità.Catapulta_5.Attacco.ToString("#,0")} [black]+ [rosso]{player.Catapulta_Attacco}[black]");

            Server.Send(player.guid_Player, $"Descrizione|Caserma Guerrieri|[black]" +
                 $"Caserma guerrieri questa struttura militare di fondamentale presenza per ogni villaggio, permette l'addestramento ed il mantenimento di unità militari specifiche.\n\n" +
                 $"Ogni caserma è attrezzata per ospitare un certo numero di uomini, raccomandati di averne un numero sufficiente.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.CasermaGuerrieri.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.CasermaGuerrieri.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.CasermaGuerrieri.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.CasermaGuerrieri.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.CasermaGuerrieri.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.CasermaGuerrieri.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.CasermaGuerrieri.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.CasermaGuerrieri.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.CasermaGuerrieri.TempoCostruzione)})\n" +
                 $"Limite Guerrieri: [icon:guerrieri]{Strutture.Edifici.CasermaGuerrieri.Limite.ToString("#,0")}\n" +
                 $"Consumo cibo: [icon:cibo][rosso]-{Strutture.Edifici.CasermaGuerrieri.Consumo_Cibo}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.CasermaGuerrieri.Consumo_Oro}[black] s\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Caserma Lanceri|[black]" +
                 $"Caserma lanceri questa struttura militare di fondamentale presenza per ogni villaggio, permette l'addestramento ed il mantenimento di unità militari specifiche.\n\n" +
                 $"Ogni caserma è attrezzata per ospitare un certo numero di uomini, raccomandati di averne un numero sufficiente.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.CasermaLanceri.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.CasermaLanceri.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.CasermaLanceri.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.CasermaLanceri.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.CasermaLanceri.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.CasermaLanceri.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.CasermaLanceri.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.CasermaLanceri.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.CasermaLanceri.TempoCostruzione)})\n" +
                 $"Limite Lanceri: [icon:lanceri]{Strutture.Edifici.CasermaLanceri.Limite.ToString("#,0")}\n" +
                 $"Consumo cibo: [icon:cibo][rosso]-{Strutture.Edifici.CasermaLanceri.Consumo_Cibo}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.CasermaLanceri.Consumo_Oro}[black] s\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Caserma Arceri|[black]" +
                 $"Caserma arceri questa struttura militare di fondamentale presenza per ogni villaggio permette l'addestramento ed il mantenimento di unità militari specifiche.\n\n" +
                 $"Ogni caserma è attrezzata per ospitare un certo numero di uomini, raccomandati di averne un numero sufficiente.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.CasermaArceri.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.CasermaArceri.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.CasermaArceri.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.CasermaArceri.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.CasermaArceri.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.CasermaArceri.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.CasermaArceri.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.CasermaArceri.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.CasermaArceri.TempoCostruzione)})\n" +
                 $"Limite Arceri: [icon:arceri]{Strutture.Edifici.CasermaArceri.Limite.ToString("#,0")}\n" +
                 $"Consumo cibo: [icon:cibo][rosso]-{Strutture.Edifici.CasermaArceri.Consumo_Cibo}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.CasermaArceri.Consumo_Oro}[black] s\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Caserma Catapulte|[black]" +
                 $"Caserma catapulte questa struttura militare di fondamentale presenza per ogni villaggio, permette l'addestramento ed il mantenimento di unità militari specifiche.\n\n" +
                 $"Ogni caserma è attrezzata per ospitare un certo numero di uomini, raccomandati di averne un numero sufficiente.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.CasermaCatapulte.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.CasermaCatapulte.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.CasermaCatapulte.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.CasermaCatapulte.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.CasermaCatapulte.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.CasermaCatapulte.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(Strutture.Edifici.CasermaCatapulte.TempoCostruzione - player.Ricerca_Costruzione - Strutture.Edifici.CasermaCatapulte.TempoCostruzione * player.Bonus_Costruzione)} ({player.FormatTime(Strutture.Edifici.CasermaCatapulte.TempoCostruzione)})\n" +
                 $"Limite Catapulte: [icon:catapulte]{Strutture.Edifici.CasermaCatapulte.Limite.ToString("#,0")}\n" +
                 $"Consumo cibo: [icon:cibo][rosso]-{Strutture.Edifici.CasermaCatapulte.Consumo_Cibo}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.CasermaCatapulte.Consumo_Oro}[black] s\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Esperienza|[black]" +
                $"L’esperienza[icon:xp] rappresenta la crescita del giocatore nel tempo.\nAccumulare esperienza permette di salire di livello.\n\n Esperienza prossimo livello: [icon:xp][acciaioBlu]{Esperienza.LevelUp(player)}[black]xp");

            Server.Send(player.guid_Player, $"Descrizione|Livello|[black]" +
                $"Il livello[icon:lv] indica il grado di avanzamento del giocatore, fondamentale per raggiungere le vette nella ricerca, poter migliorare unità e strutture.\n\n" +
                $"Necessaria per avanzare nel 'PVP/PVE', migliorare le strutture difensive del proprio villaggio, oltre che per lo sblocco di unità militari avanzate.\n " +
                $"Attualmente non è presente un limite al livello.");

            Server.Send(player.guid_Player, $"Descrizione|Giocatore|[black]" +
                $"Scheda statistiche, qui è possibile visualizzare le proprie statistiche di gioco cliccando l'icona, insieme a ulteriori informazioni molto utili durante l'avanzamento.\n");

            Server.Send(player.guid_Player, $"Descrizione|Diamanti Blu|[black]" +
                $"I [blu]Diamanti Blu[/blu][black][icon:diamanteBlu] possono essere utilizzati all'interno dello shop per l'acquisto di pacchetti, per una migliore gestione della città.\n\n" +
                $"Inoltre possono essere richiesti in alcune quest, velocizzare i tempi d'attesa per strutture e unità militari.\n");

            Server.Send(player.guid_Player, $"Descrizione|Diamanti Viola|[black]" +
                $"I [viola]Diamanti Viola[/viola][black][icon:diamanteViola] fondamentali per l'acquisto di terreni virtuali[leggendario], sono alla base dell'economia.\n\nPossono essere scambiati per [blu]Diamanti Blu[/blu][black][icon:diamanteBlu] ed utilizzati " +
                $"all'interno dello shop per l'acquisto di pacchetti o per una migliore gestione della città.\n\nOltre ad essere richiesti in alcune quest, " +
                $"dovrebbero essere sempre presenti nelle casse della città.");

            Server.Send(player.guid_Player, $"Descrizione|Dollari Virtuali|[black]" +
                $"I Dollari[icon:dollariVirtuali] Virituali vengono generati tramite i terreni virtuali del giocatore.\n\nPossono essere prelevati raggiunta la soglia di [verde]{Variabili_Server.prelievo_Minimo}$[/verde][black][icon:dollariVirtuali] " +
                $"oppure utilizzati all'interno dello shop per l'acquisto di pacchetti.");

            Server.Send(player.guid_Player, $"Descrizione|Cibo|[black]" +
                $"Il Cibo[icon:cibo] è fondamentale per il mantenimento delle unità militari, necessario per l'addestramento di unità militari e la costruzione di strutture.\n\n" +
                $"Necessario per la costruzione di edifici e per la ricerca.\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Legno|[black]" +
                $"Il Legno[icon:legno] è necessario per l'addestramento di unità militari e la costruzione di strutture.\n\n" +
                $"Necessario per la costruzione di edifici e per la ricerca.\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Pietra|[black]" +
                $"La Pietra[icon:pietra], fondamentale per la riparazione delle strutture difensive.\n\n" +
                $"Necessario per la costruzione di edifici e per la ricerca.\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ferro|[black]" +
                $"Il Ferro[icon:ferro], fondamentale per la costruzione di edifici e la riparazione delle strutture difensive.\n\n" +
                $"E' necessiamo per la produzione di armamento militare e per la ricerca.\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Oro|[black]" +
                $"L'Oro[icon:oro] è una risorsa primaria per le casse della città, necessario per la costruzione di edifici civili e militari, oltre che per il reclutamento delle unità.\n\n" +
                $"E' fondamentale per il mantenimento di unità e strutture belliche oltre che per la ricerca.\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Popolazione|[black]" +
                $"La Popolazione[icon:popolazione] è fondamentale per la costruzione di strutture civili e militari, oltre al reclutamento di unità.\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Spade|[black]Le Spade[icon:spade] sono necessarie per l'addestramento dei [cuoioScuro]guerrieri[icon:guerriero][black].\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Lance|[black]Le Lance[icon:lance] sono necessarie per l'addestramento dei [cuoioScuro]lanceri[icon:lancere][black].\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Archi|[black]Gli Archi[icon:archi] sono necessari per l'addestramento degli [cuoioScuro]arceri[icon:arcere][black].\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Scudi|[black]Gli Scudi[icon:scudi] sono necessari per l'addestramento delle unità.\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Armature|[black]Le Armature[icon:armature] sono necessarie per l'addestramento delle unità.\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Frecce|[black]Le Frecce[icon:frecce] sono fondamentali per le [cuoioScuro]unità a distanza[black], senza di esse sono praticamente inutili.\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Mura Salute|[black]Ripara la [verdeF]salute[black] delle [porporaReale]mura[black] al massimo.\nCosto riparazioni per [verdeF]1 HP[/verdeF][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Mura.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Mura.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Mura.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Mura.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Mura.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Mura Difesa|[black]Ripara la [blu]difesa[black] delle [porporaReale]mura[black] al massimo.\nCosto riparazioni per [blu]1 DEF[/blu][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Mura.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Mura.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Mura.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Mura.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Mura.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Cancello Salute|[black]Ripara la [verdeF]salute[black] del [porporaReale]cancello[black] al massimo.\nCosto riparazioni per [verdeF]1 HP[/verdeF][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Cancello.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Cancello.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Cancello.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Cancello.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Cancello.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Cancello Difesa|[black]Ripara la [blu]difesa[black] del [porporaReale]cancello[black] al massimo.\nCosto riparazioni per [blu]1 DEF[/blu][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Cancello.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Cancello.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Cancello.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Cancello.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Cancello.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Torri Salute|[black]Ripara la [verdeF]salute[black] delle [porporaReale]torri[black] al massimo.\nCosto riparazioni per [verdeF]1 HP[/verdeF][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Torri.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Torri.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Torri.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Torri.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Torri.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Torri Difesa|[black]Ripara la [blu]difesa[black] delle [porporaReale]torri[black]al massimo.\nCosto riparazioni per [blu]1 DEF[/blu][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Torri.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Torri.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Torri.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Torri.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Torri.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Castello Salute|[black]Ripara la [verdeF]salute[black] del [porporaReale]castello[black]al massimo.\nCosto riparazioni per [verdeF]1 HP[/verdeF][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Castello.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Castello.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Castello.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Castello.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Castello.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Castello Difesa|[black]Ripara la [blu]difesa[black] del [porporaReale]castello[black]al massimo.\nCosto riparazioni per [blu]1 DEF[/blu][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Castello.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Castello.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Castello.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Castello.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Castello.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Addestramento|[black]Costo ricerca addestramento lv {player.Ricerca_Addestramento + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Tipi.Addestramento.Cibo * ( player.Ricerca_Addestramento + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Tipi.Addestramento.Legno * ( player.Ricerca_Addestramento + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Tipi.Addestramento.Pietra * ( player.Ricerca_Addestramento + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Tipi.Addestramento.Ferro * ( player.Ricerca_Addestramento + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Tipi.Addestramento.Oro * ( player.Ricerca_Addestramento + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Addestramento.TempoRicerca * (player.Ricerca_Addestramento + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Costruzione|[black]Costo ricerca costruzione lv {player.Ricerca_Costruzione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Tipi.Costruzione.Cibo * ( player.Ricerca_Costruzione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Tipi.Costruzione.Legno * ( player.Ricerca_Costruzione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Tipi.Costruzione.Pietra * ( player.Ricerca_Costruzione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Tipi.Costruzione.Ferro * ( player.Ricerca_Costruzione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Tipi.Costruzione.Oro * ( player.Ricerca_Costruzione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Costruzione.TempoRicerca * (player.Ricerca_Costruzione + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Produzione|[black]Costo ricerca produzione lv {player.Ricerca_Produzione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Tipi.Produzione.Cibo * ( player.Ricerca_Produzione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Tipi.Produzione.Legno * ( player.Ricerca_Produzione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Tipi.Produzione.Pietra * ( player.Ricerca_Produzione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Tipi.Produzione.Ferro * ( player.Ricerca_Produzione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Tipi.Produzione.Oro * ( player.Ricerca_Produzione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Produzione.TempoRicerca * (player.Ricerca_Produzione + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Popolazione|[black]Costo ricerca popolazione lv {player.Ricerca_Popolazione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Tipi.Popolazione.Cibo * ( player.Ricerca_Popolazione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Tipi.Popolazione.Legno * ( player.Ricerca_Popolazione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Tipi.Popolazione.Pietra * ( player.Ricerca_Popolazione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Tipi.Popolazione.Ferro * ( player.Ricerca_Popolazione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Tipi.Popolazione.Oro * ( player.Ricerca_Popolazione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Popolazione.TempoRicerca * (player.Ricerca_Popolazione + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Trasporto|[black]Costo ricerca trasporto lv {player.Ricerca_Trasporto + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Tipi.Trasporto.Cibo * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Tipi.Trasporto.Legno * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Tipi.Trasporto.Pietra * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Tipi.Trasporto.Ferro * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Tipi.Trasporto.Oro * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Trasporto.TempoRicerca * (player.Ricerca_Trasporto + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Riparazione|[black]Costo ricerca riparazione lv {player.Ricerca_Riparazione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Tipi.Riparazione.Cibo * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Tipi.Riparazione.Legno * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Tipi.Riparazione.Pietra * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Tipi.Riparazione.Ferro * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Tipi.Riparazione.Oro * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Riparazione.TempoRicerca * (player.Ricerca_Riparazione + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Guerriero Livello|[black]Costo ricerca livello guerriero lv {player.Guerriero_Livello + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Livello.Cibo * ( player.Guerriero_Livello + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Livello.Legno * ( player.Guerriero_Livello + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Livello.Pietra * ( player.Guerriero_Livello + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Livello.Ferro * ( player.Guerriero_Livello + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Livello.Oro * ( player.Guerriero_Livello + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Livello.TempoRicerca * (player.Guerriero_Livello + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Guerriero Salute|[black]Costo ricerca salute guerriero lv {player.Guerriero_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Salute.Cibo * ( player.Guerriero_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Salute.Legno * ( player.Guerriero_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Salute.Pietra * ( player.Guerriero_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Salute.Ferro * ( player.Guerriero_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Salute.Oro * ( player.Guerriero_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Salute.TempoRicerca * (player.Guerriero_Salute + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Guerriero Attacco|[black]Costo ricerca difesa guerriero lv {player.Guerriero_Attacco + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Attacco.Cibo * ( player.Guerriero_Attacco + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Attacco.Legno * ( player.Guerriero_Attacco + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Attacco.Pietra * ( player.Guerriero_Attacco + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Attacco.Ferro * ( player.Guerriero_Attacco + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Attacco.Oro * ( player.Guerriero_Attacco + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Attacco.TempoRicerca * (player.Guerriero_Attacco + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Guerriero Difesa|[black]Costo ricerca difesa guerriero lv {player.Guerriero_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Difesa.Cibo * ( player.Guerriero_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Difesa.Legno * ( player.Guerriero_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Difesa.Pietra * ( player.Guerriero_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Difesa.Ferro * ( player.Guerriero_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Difesa.Oro * ( player.Guerriero_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Difesa.TempoRicerca * (player.Guerriero_Difesa + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Lancere Livello|[black]Costo ricerca livello lancere lv {player.Lancere_Livello + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Livello.Cibo * ( player.Lancere_Livello + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Livello.Legno * ( player.Lancere_Livello + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Livello.Pietra * ( player.Lancere_Livello + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Livello.Ferro * ( player.Lancere_Livello + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Livello.Oro * ( player.Lancere_Livello + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Livello.TempoRicerca * (player.Lancere_Livello + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Lancere Salute|[black]Costo ricerca salute lancere lv {player.Lancere_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Salute.Cibo * ( player.Lancere_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Salute.Legno * ( player.Lancere_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Salute.Pietra * ( player.Lancere_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Salute.Ferro * ( player.Lancere_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Salute.Oro * ( player.Lancere_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Salute.TempoRicerca * (player.Lancere_Salute + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Lancere Attacco|[black]Costo ricerca difesa lancere lv {player.Lancere_Attacco + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Attacco.Cibo * ( player.Lancere_Attacco + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Attacco.Legno * ( player.Lancere_Attacco + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Attacco.Pietra * ( player.Lancere_Attacco + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Attacco.Ferro * ( player.Lancere_Attacco + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Attacco.Oro * ( player.Lancere_Attacco + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Attacco.TempoRicerca * (player.Lancere_Attacco + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Lancere Difesa|[black]Costo ricerca difesa lancere lv {player.Lancere_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Difesa.Cibo * ( player.Lancere_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Difesa.Legno * ( player.Lancere_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Difesa.Pietra * ( player.Lancere_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Difesa.Ferro * ( player.Lancere_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Difesa.Oro * ( player.Lancere_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Difesa.TempoRicerca * (player.Lancere_Difesa + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Arcere Livello|[black]Costo ricerca livello arcere lv {player.Arcere_Livello + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Livello.Cibo * ( player.Arcere_Livello + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Livello.Legno * ( player.Arcere_Livello + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Livello.Pietra * ( player.Arcere_Livello + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Livello.Ferro * ( player.Arcere_Livello + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Livello.Oro * ( player.Arcere_Livello + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Livello.TempoRicerca * (player.Arcere_Livello + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Arcere Salute|[black]Costo ricerca salute arcere lv {player.Arcere_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Salute.Cibo * ( player.Arcere_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Salute.Legno * ( player.Arcere_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Salute.Pietra * ( player.Arcere_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Salute.Ferro * ( player.Arcere_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Salute.Oro * ( player.Arcere_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Salute.TempoRicerca * (player.Arcere_Salute + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Arcere Attacco|[black]Costo ricerca difesa arcere lv {player.Arcere_Attacco + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Attacco.Cibo * ( player.Arcere_Attacco + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Attacco.Legno * ( player.Arcere_Attacco + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Attacco.Pietra * ( player.Arcere_Attacco + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Attacco.Ferro * ( player.Arcere_Attacco + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Attacco.Oro * ( player.Arcere_Attacco + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Attacco.TempoRicerca * (player.Arcere_Attacco + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Arcere Difesa|[black]Costo ricerca difesa arcere lv {player.Arcere_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Difesa.Cibo * ( player.Arcere_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Difesa.Legno * ( player.Arcere_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Difesa.Pietra * ( player.Arcere_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Difesa.Ferro * ( player.Arcere_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Difesa.Oro * ( player.Arcere_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Difesa.TempoRicerca * (player.Arcere_Difesa + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Catapulta Livello|[black]Costo ricerca livello catapulte lv {player.Catapulta_Livello + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Livello.Cibo * ( player.Catapulta_Livello + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Livello.Legno * ( player.Catapulta_Livello + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Livello.Pietra * ( player.Catapulta_Livello + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Livello.Ferro * ( player.Catapulta_Livello + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Livello.Oro * ( player.Catapulta_Livello + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Livello.TempoRicerca * (player.Catapulta_Livello + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Catapulta Salute|[black]Costo ricerca salute catapulta lv {player.Catapulta_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Salute.Cibo * ( player.Catapulta_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Salute.Legno * ( player.Catapulta_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Salute.Pietra * ( player.Catapulta_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Salute.Ferro * ( player.Catapulta_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Salute.Oro * ( player.Catapulta_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Salute.TempoRicerca * (player.Catapulta_Salute + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Catapulta Attacco|[black]Costo ricerca difesa catapulta lv {player.Catapulta_Attacco + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Attacco.Cibo * ( player.Catapulta_Attacco + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Attacco.Legno * ( player.Catapulta_Attacco + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Attacco.Pietra * ( player.Catapulta_Attacco + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Attacco.Ferro * ( player.Catapulta_Attacco + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Attacco.Oro * ( player.Catapulta_Attacco + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Attacco.TempoRicerca * (player.Catapulta_Attacco + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Catapulta Difesa|[black]Costo ricerca difesa catapulta lv {player.Catapulta_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Difesa.Cibo * ( player.Catapulta_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Difesa.Legno * ( player.Catapulta_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Difesa.Pietra * ( player.Catapulta_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Difesa.Ferro * ( player.Catapulta_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Difesa.Oro * ( player.Catapulta_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Difesa.TempoRicerca * (player.Catapulta_Difesa + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Ingresso Guarnigione|[black]Costo ricerca ingresso guarnigione lv {player.Ricerca_Ingresso_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Ingresso.Cibo * ( player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Ingresso.Legno * ( player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Ingresso.Pietra * ( player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Ingresso.Ferro * ( player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Ingresso.Oro * ( player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Ingresso.TempoRicerca * (player.Ricerca_Ingresso_Guarnigione + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Citta Guarnigione|[black]Costo ricerca citta guarnigione lv {player.Ricerca_Citta_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Città.Cibo * ( player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Città.Legno * ( player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Città.Pietra * ( player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Città.Ferro * ( player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Città.Oro * ( player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Città.TempoRicerca * (player.Ricerca_Citta_Guarnigione + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Mura Guarnigione|[black]Costo ricerca mura guarnigione lv {player.Ricerca_Mura_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Mura.Cibo * ( player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Mura.Legno * ( player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Mura.Pietra * ( player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Mura.Ferro * ( player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Mura.Oro * ( player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Mura.TempoRicerca * (player.Ricerca_Mura_Guarnigione + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Mura Salute|[black]Costo ricerca mura salute lv {player.Ricerca_Mura_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Mura.Cibo * ( player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Mura.Legno * ( player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Mura.Pietra * ( player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Mura.Ferro * ( player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Mura.Oro * ( player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Mura.TempoRicerca * (player.Ricerca_Mura_Salute + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Mura Difesa|[black]Costo ricerca mura difesa lv {player.Ricerca_Mura_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Mura.Cibo * ( player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Mura.Legno * ( player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Mura.Pietra * ( player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Mura.Ferro * ( player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Mura.Oro * ( player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Mura.TempoRicerca * (player.Ricerca_Mura_Difesa + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Cancello Guarnigione|[black]Costo ricerca cancello guarnigione lv {player.Ricerca_Cancello_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Cancello.Cibo * ( player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Cancello.Legno * ( player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Cancello.Pietra * ( player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Cancello.Ferro * ( player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Cancello.Oro * ( player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Cancello.TempoRicerca * (player.Ricerca_Cancello_Guarnigione + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Cancello Salute|[black]Costo ricerca cancello salute lv {player.Ricerca_Cancello_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Cancello.Cibo * ( player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Cancello.Legno * ( player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Cancello.Pietra * ( player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Cancello.Ferro * ( player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Cancello.Oro * ( player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Cancello.TempoRicerca * (player.Ricerca_Cancello_Salute + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Cancello Difesa|[black]Costo ricerca cancello difesa lv {player.Ricerca_Cancello_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Cancello.Cibo * ( player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Cancello.Legno * ( player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Cancello.Pietra * ( player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Cancello.Ferro * ( player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Cancello.Oro * ( player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Cancello.TempoRicerca * (player.Ricerca_Cancello_Difesa + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Torri Guarnigione|[black]Costo ricerca torri guarnigione lv {player.Ricerca_Torri_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Torri.Cibo * ( player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Torri.Legno * ( player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Torri.Pietra * ( player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Torri.Ferro * ( player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Torri.Oro * ( player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Torri.TempoRicerca * ( player.Ricerca_Torri_Guarnigione + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Torri Salute|[black]Costo ricerca torri salute lv {player.Ricerca_Torri_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Torri.Cibo * ( player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Torri.Legno * ( player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Torri.Pietra * ( player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Torri.Ferro * ( player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Torri.Oro * ( player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Torri.TempoRicerca * ( player.Ricerca_Torri_Salute + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Torri Difesa|[black]Costo ricerca torri difesa lv {player.Ricerca_Torri_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Torri.Cibo * ( player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Torri.Legno * ( player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Torri.Pietra * ( player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Torri.Ferro * ( player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Torri.Oro * ( player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Torri.TempoRicerca * ( player.Ricerca_Torri_Difesa + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Ricerca Castello Guarnigione|[black]Costo ricerca castello guarnigione lv {player.Ricerca_Castello_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Castello.Cibo * ( player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Castello.Legno * ( player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Castello.Pietra * ( player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Castello.Ferro * ( player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Castello.Oro * ( player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Castello.TempoRicerca * ( player.Ricerca_Castello_Guarnigione + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Castello Salute|[black]Costo ricerca castello salute lv {player.Ricerca_Castello_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Castello.Cibo * ( player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Castello.Legno * ( player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Castello.Pietra * ( player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Castello.Ferro * ( player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Castello.Oro * ( player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Castello.TempoRicerca * ( player.Ricerca_Castello_Salute + 1))}\n\n");
            Server.Send(player.guid_Player, $"Descrizione|Ricerca Castello Difesa|[black]Costo ricerca castello difesa lv {player.Ricerca_Castello_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Castello.Cibo * ( player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Castello.Legno * ( player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Castello.Pietra * ( player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Castello.Ferro * ( player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Castello.Oro * ( player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Castello.TempoRicerca * ( player.Ricerca_Castello_Difesa + 1))}\n\n");

            Server.Send(player.guid_Player, $"Descrizione|Shop Diamanti Viola 1|[black]Tramite l'acquito di questo pacchetto diamanti 1 dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.Pacchetto_Diamanti_1.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Pacchetto_Diamanti_1.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");
            Server.Send(player.guid_Player, $"Descrizione|Shop Diamanti Viola 2|[black]Tramite l'acquito di questo pacchetto diamanti 2 dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.Pacchetto_Diamanti_2.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Pacchetto_Diamanti_2.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");
            Server.Send(player.guid_Player, $"Descrizione|Shop Diamanti Viola 3|[black]Tramite l'acquito di questo pacchetto diamanti 3 dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.Pacchetto_Diamanti_3.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Pacchetto_Diamanti_3.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");
            Server.Send(player.guid_Player, $"Descrizione|Shop Diamanti Viola 4|[black]Tramite l'acquito di questo pacchetto diamanti 4 dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.Pacchetto_Diamanti_4.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Pacchetto_Diamanti_4.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");

            Server.Send(player.guid_Player, $"Descrizione|Shop GamePass Base|[black]Tramite l'acquito di questo pacchetto GamePass Base dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.GamePass_Base.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.GamePass_Base.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");
            Server.Send(player.guid_Player, $"Descrizione|Shop GamePass Avanzato|[black]Tramite l'acquito di questo pacchetto GamePass Avanzato dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.GamePass_Base.Costo:#,0}[black]," +
            $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.GamePass_Base.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");

            Server.Send(player.guid_Player, $"Descrizione|Shop Vip 1|[black]Tramite l'acquito di questo pacchetto Vip 1 dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.Vip_1.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Vip_1.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");
            Server.Send(player.guid_Player, $"Descrizione|Shop Vip 2|[black]Tramite l'acquito di questo pacchetto Vip 2 dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.Vip_2.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Vip_2.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");

            Server.Send(player.guid_Player, $"Descrizione|Shop Costruttore 24h|[black]Tramite l'acquito di questo pacchetto GamePass Base dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.GamePass_Base.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Costruttore_48h.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");
            Server.Send(player.guid_Player, $"Descrizione|Shop Costruttore 48h|[black]Tramite l'acquito di questo pacchetto GamePass Base dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.GamePass_Base.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Costruttore_24h.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");

            Server.Send(player.guid_Player, $"Descrizione|Shop Reclutatore 24h|[black]Tramite l'acquito di questo pacchetto Reclutatore 24h Base dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.Reclutatore_24h.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Reclutatore_24h.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");
            Server.Send(player.guid_Player, $"Descrizione|Shop Reclutatore 48h|[black]Tramite l'acquito di questo pacchetto Reclutatore 48h dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.Reclutatore_48h.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Reclutatore_48h.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");

            Server.Send(player.guid_Player, $"Descrizione|Shop Scudo Pace 8h|[black]Tramite l'acquito di questo pacchetto scudo della pace 8h dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.Scudo_Pace_8h.Costo:#,0}[black]," +
                    $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Scudo_Pace_8h.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");
            Server.Send(player.guid_Player, $"Descrizione|Shop Scudo Pace 24h|[black]Tramite l'acquito di questo pacchetto scudo della pace 24h dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.Scudo_Pace_24h.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Scudo_Pace_24h.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");
            Server.Send(player.guid_Player, $"Descrizione|Shop Scudo Pace 72h|[black]Tramite l'acquito di questo pacchetto scudo della pace 72h dal costo di: [icon:usdt][verde]{Variabili_Server.Shop.Scudo_Pace_72h.Costo:#,0}[black]," +
                $"verranno accreditato [icon:diamanteViola][viola]{Variabili_Server.Shop.Scudo_Pace_72h.Reward}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n");
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
            var buildingsQueue = BuildingManager.GetQueuedBuildings(player);
            var unitsQueue = UnitManager.GetQueuedUnits(player);

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
            //DescUpdate(player);
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

            //Livelli Sblocco
            $"Unlock_Truppe_II={Variabili_Server.truppe_II}|" +
            $"Unlock_Truppe_III={Variabili_Server.truppe_III}|" +
            $"Unlock_Truppe_IV={Variabili_Server.truppe_IV}|" +
            $"Unlock_Truppe_V={Variabili_Server.truppe_V}|" +

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
            $"Code_Costruzioni_Disponibili={player.currentTasks_Building.Count}|" +
            $"Code_Reclutamenti_Disponibili={player.currentTasks_Recruit.Count}|" +

            $"Tempo_Costruzione={BuildingManager.Get_Total_Building_Time(player)}|" +
            $"Tempo_Reclutamento={UnitManager.Get_Total_Recruit_Time(player)}|" +
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

            string testo = $"Raduno|";
            string testo2 = $"Raduni_Player|";
            if (AttacchiCooperativi.AttacchiInCorso.Keys.Count() > 0)
                foreach (string idAttacco in AttacchiCooperativi.AttacchiInCorso.Keys)
                {
                    var attacco = AttacchiCooperativi.AttacchiInCorso[idAttacco];
                    testo += $"{attacco.CreatoreUsername}|{idAttacco}|{attacco.TempoRimanente / 60}-";
                }

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
                                    testo2 += $"{item}|{idAttacco}|{attacco.TempoRimanente / 60}|{items.Guerrieri[0]}|{items.Lanceri[0]}|{items.Arceri[0]}|{items.Catapulte[0]}-";
                    }
                }
            Server.Send(guid, testo); //Invia i raduni aperti
            Server.Send(guid, testo2); //Invia i raduni aperti

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
