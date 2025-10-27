using Server_Strategico.Gioco;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using WatsonTcp;
using static Server_Strategico.Gioco.Barbari;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Strutture;
using static Server_Strategico.Gioco.Variabili_Server;

namespace Server_Strategico.Server
{
    internal class ServerConnection
    {
        public static string stringa_Base = "";
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
            switch (msgArgs[0])
            {
                case "New Player":
                    Console.WriteLine($"[Server] Richiesta nuovo utente ID: {clientGuid}");
                    if (await New_Player(msgArgs[1], msgArgs[2], clientGuid))
                        Server.Send(clientGuid, "Login|true");
                    else
                        Server.Send(clientGuid, $"Login|false|Questo nome utente è già presente: [{msgArgs[1]}]");
                    break;
                case "Login":
                    bool login = await Login(msgArgs[1], msgArgs[2], clientGuid);
                    if (login == true)
                    {
                        Server.Send(clientGuid, "Login|true");
                        AggiornaVillaggiClient(player);
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
                    if (Convert.ToInt32(msgArgs[16]) > 0) BuildingManager.Costruzione("CasermaLancieri", Convert.ToInt32(msgArgs[16]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[17]) > 0) BuildingManager.Costruzione("CasermaArcieri", Convert.ToInt32(msgArgs[17]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[18]) > 0) BuildingManager.Costruzione("CasermaCatapulte", Convert.ToInt32(msgArgs[18]), clientGuid, player); // Costruisci fattorie
                    break;
                case "Reclutamento":
                    if (Convert.ToInt32(msgArgs[4]) > 0) UnitManager.Reclutamento("Guerrieri_1", Convert.ToInt32(msgArgs[4]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[5]) > 0) UnitManager.Reclutamento("Lanceri_1", Convert.ToInt32(msgArgs[5]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[6]) > 0) UnitManager.Reclutamento("Arceri_1", Convert.ToInt32(msgArgs[6]), clientGuid, player); // Costruisci fattorie
                    if (Convert.ToInt32(msgArgs[7]) > 0) UnitManager.Reclutamento("Catapulte_1", Convert.ToInt32(msgArgs[7]), clientGuid, player); // Costruisci fattorie
                    break;
                case "Costruzione_Terreni":
                    BuildingManager.Terreni_Virtuali(clientGuid, player); // Costruisci fattorie
                    break;
                case "Esplora":
                    Esplora(player, Convert.ToInt32(msgArgs[4]), msgArgs[3]);
                    break;
                case "Battaglia":
                    if (msgArgs[3] == "Barbari_PVE") Battaglie.Battaglia_Barbari(player, clientGuid, "Barbari_PVE");
                    if (msgArgs[3] == "Barbari_PVP") Battaglie.Battaglia_Barbari(player, clientGuid, "Barbari_PVP");
                    if (msgArgs[3] == "PVP")
                    {
                        var temp = msgArgs[4].Split(",");
                        var player2 = Server.servers_.GetPlayer_Data(temp[0]);
                        Battaglie.Battaglia_PVP(player, clientGuid, player2, player2.guid_Player);
                    }
                    if (msgArgs[3] == "Villaggio_Barbaro")
                    {
                        var temp = msgArgs[4].Split(",");
                        var player2 = Server.servers_.GetPlayer_Data(temp[0]);
                        Battaglie.Battaglia_PVP(player, clientGuid, player2, player2.guid_Player);
                    }
                    break;
                case "Ricerca":
                    if (msgArgs[3] == "Produzione") ResearchManager.Ricerca(msgArgs[3], clientGuid, player);
                    if (msgArgs[3] == "Costruzione") ResearchManager.Ricerca(msgArgs[3], clientGuid, player);
                    if (msgArgs[3] == "Addestramento") ResearchManager.Ricerca(msgArgs[3], clientGuid, player);
                    if (msgArgs[3] == "Truppe") Ricerca.Ricerca_Truppe(player, clientGuid, msgArgs[4], msgArgs[5]);
                    break;
                case "Descrizione":
                    switch (msgArgs[3])
                    {
                        case "Fattoria":
                            Server.Send(clientGuid, $"Descrizione|La fattoria è la struttura principale per la produzione di Cibo, fondamentale anche per la costruzione di strutture, " +
                                $"l'addestramento delle unità militari ed il loro mantenimento. Indispensabile anche per la ricerca tecnologica e la produzione di componenti militari\r\n \r\n" +
                                $"Costo Costruzione:\r\n" +
                                $"Cibo: {Strutture.Edifici.Fattoria.Cibo.ToString("#,0")}\r\n" +
                                $"Legno: {Strutture.Edifici.Fattoria.Legno.ToString("#,0")}\r\n" +
                                $"Pietra: {Strutture.Edifici.Fattoria.Pietra.ToString("#,0")}\r\n" +
                                $"Ferro: {Strutture.Edifici.Fattoria.Ferro.ToString("#,0")}\r\n" +
                                $"Oro: {Strutture.Edifici.Fattoria.Oro.ToString("#,0")}\r\n" +
                                $"Tempo di costruzione: {Strutture.Edifici.Fattoria.TempoCostruzione.ToString()} s\r\n" +
                                $"Produzione risorse: {Strutture.Edifici.Fattoria.Produzione.ToString()}");
                            break;
                        case "Segheria":
                            Server.Send(clientGuid, $"Descrizione|La Segheria è la struttura principale per la produzione di Legna, fondamentale per la costruzione di strutture e " +
                                $"l'addestramento delle unità militari. Indispensabile anche per la ricerca tecnologica e la produzione di componenti militari\r\n \r\n" +
                                $"Costo Costruzione:\r\n" +
                                $"Cibo: {Strutture.Edifici.Segheria.Cibo.ToString("#,0")}\r\n" +
                                $"Legno: {Strutture.Edifici.Segheria.Legno.ToString("#,0")}\r\n" +
                                $"Pietra: {Strutture.Edifici.Segheria.Pietra.ToString("#,0")}\r\n" +
                                $"Ferro: {Strutture.Edifici.Segheria.Ferro.ToString("#,0")}\r\n" +
                                $"Oro: {Strutture.Edifici.Segheria.Oro.ToString("#,0")}\r\n" +
                                $"Tempo di costruzione: {Strutture.Edifici.Segheria.TempoCostruzione.ToString()} s\r\n" +
                                $"Produzione risorse: {Strutture.Edifici.Segheria.Produzione.ToString()}");
                            break;
                        case "Cava Pietra":
                            Server.Send(clientGuid, $"Descrizione|La cava di pietra è la struttura principale per la produzione di Pietra, fondamentale per la costruzione di strutture e " +
                                $"l'addestramento delle unità militari. Indispensabile anche per la ricerca tecnologica e la produzione di componenti militari\r\n \r\n" +
                                $"Costo Costruzione:\r\n" +
                                $"Cibo: {Strutture.Edifici.CavaPietra.Cibo.ToString("#,0")}\r\n" +
                                $"Legno: {Strutture.Edifici.CavaPietra.Legno.ToString("#,0")}\r\n" +
                                $"Pietra: {Strutture.Edifici.CavaPietra.Pietra.ToString("#,0")}\r\n" +
                                $"Ferro: {Strutture.Edifici.CavaPietra.Ferro.ToString("#,0")}\r\n" +
                                $"Oro: {Strutture.Edifici.CavaPietra.Oro.ToString("#,0")}\r\n" +
                                $"Tempo di costruzione: {Strutture.Edifici.CavaPietra.TempoCostruzione.ToString()} s\r\n" +
                                $"Produzione risorse: {Strutture.Edifici.CavaPietra.Produzione.ToString()}");
                            break;
                        case "Miniera Ferro":
                            Server.Send(clientGuid, $"Descrizione|La Miniera di ferro è la struttura principale per la produzione di Ferro, fondamentale per la costruzione di strutture e " +
                                $"l'addestramento delle unità militari. Indispensabile anche per la ricerca tecnologica e la produzione di componenti militari\r\n \r\n" +
                                $"Costo Costruzione:\r\n" +
                                $"Cibo: {Strutture.Edifici.MinieraFerro.Cibo.ToString("#,0")}\r\n" +
                                $"Legno: {Strutture.Edifici.MinieraFerro.Legno.ToString("#,0")}\r\n" +
                                $"Pietra: {Strutture.Edifici.MinieraFerro.Pietra.ToString("#,0")}\r\n" +
                                $"Ferro: {Strutture.Edifici.MinieraFerro.Ferro.ToString("#,0")}\r\n" +
                                $"Oro: {Strutture.Edifici.MinieraFerro.Oro.ToString("#,0")}\r\n" +
                                $"Tempo di costruzione: {Strutture.Edifici.MinieraFerro.TempoCostruzione.ToString()} s\r\n" +
                                $"Produzione risorse: {Strutture.Edifici.MinieraFerro.Produzione.ToString()}");
                            break;
                        case "Miniera Oro":
                            Server.Send(clientGuid, $"Descrizione|La miniera d'oro è la struttura principale per la produzione dell'Oro, fondamentale per la costruzione di strutture e " +
                                $"l'addestramento delle unità militari. Indispensabile anche per la ricerca tecnologica e la produzione di componenti militari\r\n \r\n" +
                                $"Costo Costruzione:\r\n" +
                                $"Cibo: {Strutture.Edifici.MinieraOro.Cibo.ToString("#,0")}\r\n" +
                                $"Legno: {Strutture.Edifici.MinieraOro.Legno.ToString("#,0")}\r\n" +
                                $"Pietra: {Strutture.Edifici.MinieraOro.Pietra.ToString("#,0")}\r\n" +
                                $"Ferro: {Strutture.Edifici.MinieraOro.Ferro.ToString("#,0")}\r\n" +
                                $"Oro: {Strutture.Edifici.MinieraOro.Oro.ToString("#,0")}\r\n" +
                                $"Tempo di costruzione: {Strutture.Edifici.MinieraOro.TempoCostruzione.ToString()} s\r\n" +
                                $"Produzione risorse: {Strutture.Edifici.MinieraOro.Produzione.ToString()}");
                            break;
                        case "Case":
                            Server.Send(clientGuid, $"Descrizione|Le Case sono necessarie per invogliare sempre più cittadini presso il vostro villaggio, " +
                                $"sono fondamentali per addestrare le unità militari.\r\n \r\n" +
                                $"Costo Costruzione:\r\n" +
                                $"Cibo: {Strutture.Edifici.Case.Cibo.ToString("#,0")}\r\n" +
                                $"Legno: {Strutture.Edifici.Case.Legno.ToString("#,0")}\r\n" +
                                $"Pietra: {Strutture.Edifici.Case.Pietra.ToString("#,0")}\r\n" +
                                $"Ferro: {Strutture.Edifici.Case.Ferro.ToString("#,0")}\r\n" +
                                $"Oro: {Strutture.Edifici.Case.Oro.ToString("#,0")}\r\n" +
                                $"Tempo di costruzione: {Strutture.Edifici.Case.TempoCostruzione.ToString()} s\r\n" +
                                $"Produzione risorse: {Strutture.Edifici.Case.Produzione.ToString()}");
                            break;

                        case "Produzione Spade":
                            Server.Send(clientGuid, $"Descrizione|Questa struttura è attrezzata in modo da produrre equipaggiamento militare specifico, " +
                                $"essenziali per l'addestramento di unità militari, questa struttura produce Spade.\r\n \r\n" +
                                $"Costo Costruzione:\r\n" +
                                $"Cibo: {Strutture.Edifici.ProduzioneSpade.Cibo.ToString("#,0")}\r\n" +
                                $"Legno: {Strutture.Edifici.ProduzioneSpade.Legno.ToString("#,0")}\r\n" +
                                $"Pietra: {Strutture.Edifici.ProduzioneSpade.Pietra.ToString("#,0")}\r\n" +
                                $"Ferro: {Strutture.Edifici.ProduzioneSpade.Ferro.ToString("#,0")}\r\n" +
                                $"Oro: {Strutture.Edifici.ProduzioneSpade.Oro.ToString("#,0")}\r\n" +
                                $"Tempo di costruzione: {Strutture.Edifici.ProduzioneSpade.TempoCostruzione.ToString()} s\r\n" +
                                $"Produzione risorse: {Strutture.Edifici.ProduzioneSpade.Produzione.ToString()}");
                            break;
                        case "Produzione Lance":
                            Server.Send(clientGuid, $"Descrizione|Questa struttura è attrezzata in modo da produrre equipaggiamento militare specifico, " +
                                $"essenziali per l'addestramento di unità militari, questa struttura produce Lance.\r\n \r\n" +
                                $"Costo Costruzione:\r\n" +
                                $"Cibo: {Strutture.Edifici.ProduzioneLance.Cibo.ToString("#,0")}\r\n" +
                                $"Legno: {Strutture.Edifici.ProduzioneLance.Legno.ToString("#,0")}\r\n" +
                                $"Pietra: {Strutture.Edifici.ProduzioneLance.Pietra.ToString("#,0")}\r\n" +
                                $"Ferro: {Strutture.Edifici.ProduzioneLance.Ferro.ToString("#,0")}\r\n" +
                                $"Oro: {Strutture.Edifici.ProduzioneLance.Oro.ToString("#,0")}\r\n" +
                                $"Tempo di costruzione: {Strutture.Edifici.ProduzioneLance.TempoCostruzione.ToString()} s\r\n" +
                                $"Produzione risorse: {Strutture.Edifici.ProduzioneLance.Produzione.ToString()}");
                            break;
                        case "Produzione Archi":
                            Server.Send(clientGuid, $"Descrizione|Questa struttura è attrezzata in modo da produrre equipaggiamento militare specifico, " +
                                $"essenziali per l'addestramento di unità militari, questa struttura produce Archi.\r\n \r\n" +
                                $"Costo Costruzione:\r\n" +
                                $"Cibo: {Strutture.Edifici.ProduzioneArchi.Cibo.ToString("#,0")}\r\n" +
                                $"Legno: {Strutture.Edifici.ProduzioneArchi.Legno.ToString("#,0")}\r\n" +
                                $"Pietra: {Strutture.Edifici.ProduzioneArchi.Pietra.ToString("#,0")}\r\n" +
                                $"Ferro: {Strutture.Edifici.ProduzioneArchi.Ferro.ToString("#,0")}\r\n" +
                                $"Oro: {Strutture.Edifici.ProduzioneArchi.Oro.ToString("#,0")}\r\n" +
                                $"Tempo di costruzione: {Strutture.Edifici.ProduzioneArchi.TempoCostruzione.ToString()} s\r\n" +
                                $"Produzione risorse: {Strutture.Edifici.ProduzioneArchi.Produzione.ToString()}");
                            break;
                        case "Produzione Scudi":
                            Server.Send(clientGuid, $"Descrizione|Questa struttura è attrezzata in modo da produrre equipaggiamento militare specifico, " +
                                $"essenziali per l'addestramento di unità militari, questa struttura produce Scudi.\r\n \r\n" +
                                $"Costo Costruzione:\r\n" +
                                $"Cibo: {Strutture.Edifici.ProduzioneScudi.Cibo.ToString("#,0")}\r\n" +
                                $"Legno: {Strutture.Edifici.ProduzioneScudi.Legno.ToString("#,0")}\r\n" +
                                $"Pietra: {Strutture.Edifici.ProduzioneScudi.Pietra.ToString("#,0")}\r\n" +
                                $"Ferro: {Strutture.Edifici.ProduzioneScudi.Ferro.ToString("#,0")}\r\n" +
                                $"Oro: {Strutture.Edifici.ProduzioneScudi.Oro.ToString("#,0")}\r\n" +
                                $"Tempo di costruzione: {Strutture.Edifici.ProduzioneScudi.TempoCostruzione.ToString()} s\r\n" +
                                $"Produzione risorse: {Strutture.Edifici.ProduzioneScudi.Produzione.ToString()}");
                            break;
                        case "Produzione Armature":
                            Server.Send(clientGuid, $"Descrizione|Questa struttura è attrezzata in modo da produrre equipaggiamento militare specifico, " +
                                $"essenziali per l'addestramento di unità militari, questa struttura produce Armature.\r\n \r\n" +
                                $"Costo Costruzione:\r\n" +
                                $"Cibo: {Strutture.Edifici.ProduzioneArmature.Cibo.ToString("#,0")}\r\n" +
                                $"Legno: {Strutture.Edifici.ProduzioneArmature.Legno.ToString("#,0")}\r\n" +
                                $"Pietra: {Strutture.Edifici.ProduzioneArmature.Pietra.ToString("#,0")}\r\n" +
                                $"Ferro: {Strutture.Edifici.ProduzioneArmature.Ferro.ToString("#,0")}\r\n" +
                                $"Oro: {Strutture.Edifici.ProduzioneArmature.Oro.ToString("#,0")}\r\n" +
                                $"Tempo di costruzione: {Strutture.Edifici.ProduzioneArmature.TempoCostruzione.ToString()} s\r\n" +
                                $"Produzione risorse: {Strutture.Edifici.ProduzioneArmature.Produzione.ToString()}");
                            break;
                        case "Produzione Frecce":
                            Server.Send(clientGuid, $"Descrizione|Questa struttura è attrezzata in modo da produrre equipaggiamento militare specifico, " +
                                $"essenziali per l'addestramento di unità militari, questa struttura produce Frecce.\r\n \r\n" +
                                $"Costo Costruzione:\r\n" +
                                $"Cibo: {Strutture.Edifici.ProduzioneFrecce.Cibo.ToString("#,0")}\r\n" +
                                $"Legno: {Strutture.Edifici.ProduzioneFrecce.Legno.ToString("#,0")}\r\n" +
                                $"Pietra: {Strutture.Edifici.ProduzioneFrecce.Pietra.ToString("#,0")}\r\n" +
                                $"Ferro: {Strutture.Edifici.ProduzioneFrecce.Ferro.ToString("#,0")}\r\n" +
                                $"Oro: {Strutture.Edifici.ProduzioneFrecce.Oro.ToString("#,0")}\r\n" +
                                $"Tempo di costruzione: {Strutture.Edifici.ProduzioneFrecce.TempoCostruzione.ToString()} s\r\n" +
                                $"Produzione risorse: {Strutture.Edifici.ProduzioneFrecce.Produzione.ToString()}");
                            break;

                        case "Guerriero":
                            Server.Send(clientGuid, $"Descrizione|I guerrieri sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono sa prina dorsale di ogni esercito,  " +
                                $"sono facili da reclutare e non chiedono molta manutenzione in cibo ed oro.\r\n \r\n" +
                                $"Costo Addestramento:\r\n" +
                                $"Cibo: {Esercito.CostoReclutamento.Guerrieri_1.Cibo.ToString("#,0")}                  Spade: {Esercito.CostoReclutamento.Guerrieri_1.Spade.ToString("#,0")}\r\n" +
                                $"Legno: {Esercito.CostoReclutamento.Guerrieri_1.Legno.ToString("#,0")}               Lancie: {Esercito.CostoReclutamento.Guerrieri_1.Lance.ToString("#,0")}\r\n" +
                                $"Pietra: {Esercito.CostoReclutamento.Guerrieri_1.Pietra.ToString("#,0")}                Archi: {Esercito.CostoReclutamento.Guerrieri_1.Archi.ToString("#,0")}\r\n" +
                                $"Ferro: {Esercito.CostoReclutamento.Guerrieri_1.Ferro.ToString("#,0")}                 Scudi: {Esercito.CostoReclutamento.Guerrieri_1.Scudi.ToString("#,0")}\r\n" +
                                $"Oro: {Esercito.CostoReclutamento.Guerrieri_1.Oro.ToString("#,0")}                    Armature: {Esercito.CostoReclutamento.Guerrieri_1.Armature.ToString("#,0")}\r\n \r\n" +
                                $"Popolazione: {Esercito.CostoReclutamento.Guerrieri_1.Popolazione}\r\n" +
                                $"Tempo di Addestramento: {Esercito.CostoReclutamento.Guerrieri_1.TempoReclutamento.ToString()} s\r\n" +
                                $"Mantenimento Cibo: {Esercito.Unità.Guerrieri_1.Cibo.ToString()} s\r\n" +
                                $"Mantenimento Oro: {Esercito.Unità.Guerrieri_1.Salario.ToString()} s\r\n \r\n" +
                                $"Statistiche:\r\n" +
                                $"Livello: {player.Guerriero_Livello.ToString("#,0")}\r\n" +
                                $"Salute:  {(Esercito.Unità.Guerrieri_1.Salute + player.Guerriero_Livello).ToString("#,0")}\r\n" +
                                $"Difesa:  {(Esercito.Unità.Guerrieri_1.Difesa + player.Guerriero_Livello).ToString("#,0")}\r\n" +
                                $"Attacco: {(Esercito.Unità.Guerrieri_1.Attacco + player.Guerriero_Livello).ToString("#,0")}\r\n \r\n");
                            break;
                        case "Lanciere":
                            Server.Send(clientGuid, $"Descrizione|I Lancieri sono la spina dorsale di ogni esercito ben organizzato. Armati di lance, " +
                                $"questi soldati costituiscono un baluardo formidabile contro gli assalti nemici.\r\n \r\n" +
                                $"Costo Addestramento:\r\n" +
                                $"Cibo: {Esercito.CostoReclutamento.Lanceri_1.Cibo.ToString("#,0")}                  Spade: {Esercito.CostoReclutamento.Lanceri_1.Spade.ToString("#,0")}\r\n" +
                                $"Legno: {Esercito.CostoReclutamento.Lanceri_1.Legno.ToString("#,0")}               Lancie: {Esercito.CostoReclutamento.Lanceri_1.Lance.ToString("#,0")}\r\n" +
                                $"Pietra: {Esercito.CostoReclutamento.Lanceri_1.Pietra.ToString("#,0")}                Archi: {Esercito.CostoReclutamento.Lanceri_1.Archi.ToString("#,0")}\r\n" +
                                $"Ferro: {Esercito.CostoReclutamento.Lanceri_1.Ferro.ToString("#,0")}                 Scudi: {Esercito.CostoReclutamento.Lanceri_1.Scudi.ToString("#,0")}\r\n" +
                                $"Oro: {Esercito.CostoReclutamento.Lanceri_1.Oro.ToString("#,0")}                    Armature: {Esercito.CostoReclutamento.Lanceri_1.Armature.ToString("#,0")}\r\n \r\n" +
                                $"Popolazione: {Esercito.CostoReclutamento.Lanceri_1.Popolazione}\r\n" +
                                $"Tempo di Addestramento: {Esercito.CostoReclutamento.Lanceri_1.TempoReclutamento.ToString()} s\r\n" +
                                $"Mantenimento Cibo: {Esercito.Unità.Lanceri_1.Cibo.ToString()} s\r\n" +
                                $"Mantenimento Oro: {Esercito.Unità.Lanceri_1.Salario.ToString()} s\r\n \r\n" +
                                $"Statistiche:\r\n" +
                                $"Livello: {player.Lancere_Livello.ToString("#,0")}\r\n" +
                                $"Salute:  {(Esercito.Unità.Lanceri_1.Salute + player.Lancere_Livello).ToString("#,0")}\r\n" +
                                $"Difesa:  {(Esercito.Unità.Lanceri_1.Difesa + player.Lancere_Livello).ToString("#,0")}\r\n" +
                                $"Attacco: {(Esercito.Unità.Lanceri_1.Attacco + player.Lancere_Livello).ToString("#,0")}\r\n \r\n");
                            break;
                        case "Arciere":
                            Server.Send(clientGuid, $"Descrizione|Gli Arcieri armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza, " +
                                $"lanciando frecce mortali sulle linee nemiche, prima che possano avvicinarsi.\r\n \r\n" +
                                $"Costo Addestramento:\r\n" +
                                $"Cibo: {Esercito.CostoReclutamento.Arceri_1.Cibo.ToString("#,0")}                  Spade: {Esercito.CostoReclutamento.Arceri_1.Spade.ToString("#,0")}\r\n" +
                                $"Legno: {Esercito.CostoReclutamento.Arceri_1.Legno.ToString("#,0")}               Lancie: {Esercito.CostoReclutamento.Arceri_1.Lance.ToString("#,0")}\r\n" +
                                $"Pietra: {Esercito.CostoReclutamento.Arceri_1.Pietra.ToString("#,0")}                Archi: {Esercito.CostoReclutamento.Arceri_1.Archi.ToString("#,0")}\r\n" +
                                $"Ferro: {Esercito.CostoReclutamento.Arceri_1.Ferro.ToString("#,0")}                 Scudi: {Esercito.CostoReclutamento.Arceri_1.Scudi.ToString("#,0")}\r\n" +
                                $"Oro: {Esercito.CostoReclutamento.Arceri_1.Oro.ToString("#,0")}                    Armature: {Esercito.CostoReclutamento.Arceri_1.Armature.ToString("#,0")}\r\n \r\n" +
                                $"Popolazione: {Esercito.CostoReclutamento.Arceri_1.Popolazione}\r\n" +
                                $"Tempo di Addestramento: {Esercito.CostoReclutamento.Arceri_1.TempoReclutamento.ToString()} s\r\n" +
                                $"Mantenimento Cibo: {Esercito.Unità.Arceri_1.Cibo.ToString()} s\r\n" +
                                $"Mantenimento Oro: {Esercito.Unità.Arceri_1.Salario.ToString()} s\r\n \r\n" +
                                $"Statistiche:\r\n" +
                                $"Livello: {player.Arcere_Livello.ToString("#,0")}\r\n" +
                                $"Salute:  {(Esercito.Unità.Arceri_1.Salute + player.Arcere_Livello).ToString("#,0")}\r\n" +
                                $"Difesa:  {(Esercito.Unità.Arceri_1.Difesa + player.Arcere_Livello).ToString("#,0")}\r\n" +
                                $"Attacco: {(Esercito.Unità.Arceri_1.Attacco + player.Arcere_Livello).ToString("#,0")}\r\n \r\n");
                            break;
                        case "Catapulta":
                            Server.Send(clientGuid, $"Descrizione|Le Catapulte sono potenti macchine d'assedio che cambiano le sorti delle battaglie, " +
                                $"scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche\r\n \r\n" +
                                $"Costo Addestramento:\r\n" +
                                $"Cibo: {Esercito.CostoReclutamento.Catapulte_1.Cibo.ToString("#,0")}                  Spade: {Esercito.CostoReclutamento.Catapulte_1.Spade.ToString("#,0")}\r\n" +
                                $"Legno: {Esercito.CostoReclutamento.Catapulte_1.Legno.ToString("#,0")}               Lancie: {Esercito.CostoReclutamento.Catapulte_1.Lance.ToString("#,0")}\r\n" +
                                $"Pietra: {Esercito.CostoReclutamento.Catapulte_1.Pietra.ToString("#,0")}                Archi: {Esercito.CostoReclutamento.Catapulte_1.Archi.ToString("#,0")}\r\n" +
                                $"Ferro: {Esercito.CostoReclutamento.Catapulte_1.Ferro.ToString("#,0")}                 Scudi: {Esercito.CostoReclutamento.Catapulte_1.Scudi.ToString("#,0")}\r\n" +
                                $"Oro: {Esercito.CostoReclutamento.Catapulte_1.Oro.ToString("#,0")}                    Armature: {Esercito.CostoReclutamento.Catapulte_1.Armature.ToString("#,0")}\r\n \r\n" +
                                $"Popolazione: {Esercito.CostoReclutamento.Catapulte_1.Popolazione}\r\n" +
                                $"Tempo di Addestramento: {Esercito.CostoReclutamento.Catapulte_1.TempoReclutamento.ToString()} s\r\n" +
                                $"Mantenimento Cibo: {Esercito.Unità.Catapulte_1.Cibo.ToString("0.00")} s\r\n" +
                                $"Mantenimento Oro: {Esercito.Unità.Catapulte_1.Salario.ToString("0.00")} s\r\n \r\n" +
                                $"Statistiche:\r\n" +
                                $"Livello: {player.Catapulta_Livello.ToString("#,0")}\r\n" +
                                $"Salute:  {(Esercito.Unità.Catapulte_1.Salute + player.Catapulta_Livello).ToString("#,0")}\r\n" +
                                $"Difesa:  {(Esercito.Unità.Catapulte_1.Difesa + player.Catapulta_Livello).ToString("#,0")}\r\n" +
                                $"Attacco: {(Esercito.Unità.Catapulte_1.Attacco + player.Catapulta_Livello).ToString("#,0")}\r\n \r\n");
                            break;
                        case "Raduno":
                            Server.Send(clientGuid, $"Descrizione|Il Raduno ti permette di creare e gestire attacchi coordinati con altri giocatori, " +
                                $"verso il barbaro PVP disponibile. Essendo che il barbaro riesce a reclutare molte unità nel suo campo, può essere saggio " +
                                $"chiedere aiuto ad altri giocatori nell'impresa.\r\n \r\n" +
                                $"- Solo colui che crea il raduno potrà iniziare l'attacco.\r\n" +
                                $"- Gli attacchi aperti sono pubblici e chiunque potra partecipare\r\n" +
                                $"- Nel caso in cui il tempo disponibile termina, il raduno verrà annullato e le unità dei giocatori partecipanti torneranno indietro\r\n" +
                                $"- Il livello delle unità non verrà mantenuto, perciò sia i giocatori che il barbaro avranno unità LV 0");
                            break;
                        case "Costruzione":
                            Server.Send(clientGuid, $"Descrizione|Permette la costruzione di Strutture Militari, Civili, Caserme ed unità Militari");
                            break;
                        case "Ricerca":
                            Server.Send(clientGuid, $"Descrizione|La Ricerca è fondamentale per ogni città... Per il miglioramento delle strutture, la loro produzione, fino " +
                                $"al reclutamento delle unità, le stesse possono subire un miglioramento delle loro caratteristiche e del loro livello.");
                            break;
                    }
                    break;
                case "AttaccoCooperativo":
                    await AttacchiCooperativi.GestisciComando(msgArgs, clientGuid, player);
                    break;
                
                default: Console.WriteLine($"Messaggio: [{msgArgs}]"); break;
            }
           
        }
        public static void Esplora(Player player, int livello_Barbaro, string globale)
        {
            BarbarianBase target;

            if (globale == "Citta Barbara")
                target = Gioco.Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == livello_Barbaro); // Cerca la città globale del livello richiesto
            else if (globale == "Villaggio Barbaro")
                target = player.VillaggiPersonali.FirstOrDefault(v => v.Livello == livello_Barbaro); // Cerca il villaggio personale del giocatore
            else             
                target = null;

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
                await GameSave.LoadPlayer(username, password);
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

                return true;
            }
            if (await Server.servers_.Check_Username_Player(username)) // Controlla se il nome utente è disponibile
            {
                if (await GameSave.LoadPlayer(username, password)) // Poi prova a caricare i dati salvati
                    return false;
            }
            return false;
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

        public static void DescUpdate(Player player)
        {
            Server.Send(player.guid_Player, $"Descrizione|Fattoria|La fattoria è la struttura principale per la produzione di Cibo, fondamentale anche per la costruzione di strutture, \r\n" +
                 $"l'addestramento delle unità militari ed il loro mantenimento. Indispensabile anche per la ricerca tecnologica e la produzione di componenti militari\r\n\r\n" +
                 $"Costo Costruzione:\r\n" +
                 $"Cibo: {Strutture.Edifici.Fattoria.Cibo.ToString("#,0")}\r\n" +
                 $"Legno: {Strutture.Edifici.Fattoria.Legno.ToString("#,0")}\r\n" +
                 $"Pietra: {Strutture.Edifici.Fattoria.Pietra.ToString("#,0")}\r\n" +
                 $"Ferro: {Strutture.Edifici.Fattoria.Ferro.ToString("#,0")}\r\n" +
                 $"Oro: {Strutture.Edifici.Fattoria.Oro.ToString("#,0")}\r\n" +
                 $"Popolazione: {Strutture.Edifici.Fattoria.Popolazione.ToString("#,0")}\r\n" +
                 $"Tempo di costruzione: {Strutture.Edifici.Fattoria.TempoCostruzione.ToString()} s\r\n" +
                 $"Produzione risorse: {Strutture.Edifici.Fattoria.Produzione.ToString()}");

             Server.Send(player.guid_Player, $"Descrizione|Segheria|La Segheria è la struttura principale per la produzione di Legna, fondamentale per la costruzione di strutture e " +
                 $"l'addestramento delle unità militari.\r\nIndispensabile anche per la ricerca tecnologica e la produzione di componenti militari\r\n\r\n" +
                 $"Costo Costruzione:\r\n" +
                 $"Cibo: {Strutture.Edifici.Segheria.Cibo.ToString("#,0")}\r\n" +
                 $"Legno: {Strutture.Edifici.Segheria.Legno.ToString("#,0")}\r\n" +
                 $"Pietra: {Strutture.Edifici.Segheria.Pietra.ToString("#,0")}\r\n" +
                 $"Ferro: {Strutture.Edifici.Segheria.Ferro.ToString("#,0")}\r\n" +
                 $"Oro: {Strutture.Edifici.Segheria.Oro.ToString("#,0")}\r\n" +
                 $"Popolazione: {Strutture.Edifici.Segheria.Popolazione.ToString("#,0")}\r\n" +
                 $"Tempo di costruzione: {Strutture.Edifici.Segheria.TempoCostruzione.ToString()} s\r\n" +
                 $"Produzione risorse: {Strutture.Edifici.Segheria.Produzione.ToString()}");

             Server.Send(player.guid_Player, $"Descrizione|Cava di Pietra|La cava di pietra è la struttura principale per la produzione di Pietra, fondamentale per la costruzione di strutture e " +
                 $"l'addestramento delle unità militari.\r\nIndispensabile anche per la ricerca tecnologica e la produzione di componenti militari\r\n\r\n" +
                 $"Costo Costruzione:\r\n" +
                 $"Cibo: {Strutture.Edifici.CavaPietra.Cibo.ToString("#,0")}\r\n" +
                 $"Legno: {Strutture.Edifici.CavaPietra.Legno.ToString("#,0")}\r\n" +
                 $"Pietra: {Strutture.Edifici.CavaPietra.Pietra.ToString("#,0")}\r\n" +
                 $"Ferro: {Strutture.Edifici.CavaPietra.Ferro.ToString("#,0")}\r\n" +
                 $"Oro: {Strutture.Edifici.CavaPietra.Oro.ToString("#,0")}\r\n" +
                 $"Popolazione: {Strutture.Edifici.CavaPietra.Popolazione.ToString("#,0")}\r\n" +
                 $"Tempo di costruzione: {Strutture.Edifici.CavaPietra.TempoCostruzione.ToString()} s\r\n" +
                 $"Produzione risorse: {Strutture.Edifici.CavaPietra.Produzione.ToString()}");

             Server.Send(player.guid_Player, $"Descrizione|Miniera di Ferro|La Miniera di ferro è la struttura principale per la produzione di Ferro, fondamentale per la costruzione di strutture e " +
                 $"l'addestramento delle unità militari.\r\nIndispensabile anche per la ricerca tecnologica e la produzione di componenti militari\r\n\r\n" +
                 $"Costo Costruzione:\r\n" +
                 $"Cibo: {Strutture.Edifici.MinieraFerro.Cibo.ToString("#,0")}\r\n" +
                 $"Legno: {Strutture.Edifici.MinieraFerro.Legno.ToString("#,0")}\r\n" +
                 $"Pietra: {Strutture.Edifici.MinieraFerro.Pietra.ToString("#,0")}\r\n" +
                 $"Ferro: {Strutture.Edifici.MinieraFerro.Ferro.ToString("#,0")}\r\n" +
                 $"Oro: {Strutture.Edifici.MinieraFerro.Oro.ToString("#,0")}\r\n" +
                 $"Popolazione: {Strutture.Edifici.MinieraFerro.Popolazione.ToString("#,0")}\r\n" +
                 $"Tempo di costruzione: {Strutture.Edifici.MinieraFerro.TempoCostruzione.ToString()} s\r\n" +
                 $"Produzione risorse: {Strutture.Edifici.MinieraFerro.Produzione.ToString()}");

             Server.Send(player.guid_Player, $"Descrizione|Miniera d'Oro|La miniera d'oro è la struttura principale per la produzione dell'Oro, fondamentale per la costruzione di strutture e " +
                 $"l'addestramento delle unità militari.\r\nIndispensabile anche per la ricerca tecnologica e la produzione di componenti militari\r\n\r\n" +
                 $"Costo Costruzione:\r\n" +
                 $"Cibo: {Strutture.Edifici.MinieraOro.Cibo.ToString("#,0")}\r\n" +
                 $"Legno: {Strutture.Edifici.MinieraOro.Legno.ToString("#,0")}\r\n" +
                 $"Pietra: {Strutture.Edifici.MinieraOro.Pietra.ToString("#,0")}\r\n" +
                 $"Ferro: {Strutture.Edifici.MinieraOro.Ferro.ToString("#,0")}\r\n" +
                 $"Oro: {Strutture.Edifici.MinieraOro.Oro.ToString("#,0")}\r\n" +
                 $"Popolazione: {Strutture.Edifici.MinieraOro.Popolazione.ToString("#,0")}\r\n" +
                 $"Tempo di costruzione: {Strutture.Edifici.MinieraOro.TempoCostruzione.ToString()} s\r\n" +
                 $"Produzione risorse: {Strutture.Edifici.MinieraOro.Produzione.ToString()}");

             Server.Send(player.guid_Player, $"Descrizione|Case|Le Case sono necessarie per invogliare sempre più cittadini presso il vostro villaggio,\r\n" +
                 $"sono fondamentali per addestrare le unità militari.\r\n\r\n" +
                 $"Costo Costruzione:\r\n" +
                 $"Cibo: {Strutture.Edifici.Case.Cibo.ToString("#,0")}\r\n" +
                 $"Legno: {Strutture.Edifici.Case.Legno.ToString("#,0")}\r\n" +
                 $"Pietra: {Strutture.Edifici.Case.Pietra.ToString("#,0")}\r\n" +
                 $"Ferro: {Strutture.Edifici.Case.Ferro.ToString("#,0")}\r\n" +
                 $"Oro: {Strutture.Edifici.Case.Oro.ToString("#,0")}\r\n" +
                 $"Popolazione: {Strutture.Edifici.Case.Popolazione.ToString("#,0")}\r\n" +
                 $"Tempo di costruzione: {Strutture.Edifici.Case.TempoCostruzione.ToString()} s\r\n" +
                 $"Produzione risorse: {Strutture.Edifici.Case.Produzione.ToString()}");

             Server.Send(player.guid_Player, $"Descrizione|Produzione Spade|Workshop Spade questa struttura è attrezzata in modo da produrre equipaggiamento militare specifico,\r\n " +
                 $"essenziali per l'addestramento di unità militari, questa struttura produce Spade.\r\n\r\n" +
                 $"Costo Costruzione:\r\n" +
                 $"Cibo: {Strutture.Edifici.ProduzioneSpade.Cibo.ToString("#,0")}\r\n" +
                 $"Legno: {Strutture.Edifici.ProduzioneSpade.Legno.ToString("#,0")}\r\n" +
                 $"Pietra: {Strutture.Edifici.ProduzioneSpade.Pietra.ToString("#,0")}\r\n" +
                 $"Ferro: {Strutture.Edifici.ProduzioneSpade.Ferro.ToString("#,0")}\r\n" +
                 $"Oro: {Strutture.Edifici.ProduzioneSpade.Oro.ToString("#,0")}\r\n" +
                 $"Popolazione: {Strutture.Edifici.ProduzioneSpade.Popolazione.ToString("#,0")}\r\n" +
                 $"Tempo di costruzione: {Strutture.Edifici.ProduzioneSpade.TempoCostruzione.ToString()} s\r\n" +
                 $"Produzione risorse: {Strutture.Edifici.ProduzioneSpade.Produzione.ToString()}");

             Server.Send(player.guid_Player, $"Descrizione|Produzione Lance|Workshop Lance questa struttura è attrezzata in modo da produrre equipaggiamento militare specifico,\r\n" +
                 $"essenziali per l'addestramento di unità militari, questa struttura produce Lance.\r\n\r\n" +
                 $"Costo Costruzione:\r\n" +
                 $"Cibo: {Strutture.Edifici.ProduzioneLance.Cibo.ToString("#,0")}\r\n" +
                 $"Legno: {Strutture.Edifici.ProduzioneLance.Legno.ToString("#,0")}\r\n" +
                 $"Pietra: {Strutture.Edifici.ProduzioneLance.Pietra.ToString("#,0")}\r\n" +
                 $"Ferro: {Strutture.Edifici.ProduzioneLance.Ferro.ToString("#,0")}\r\n" +
                 $"Oro: {Strutture.Edifici.ProduzioneLance.Oro.ToString("#,0")}\r\n" +
                 $"Popolazione: {Strutture.Edifici.ProduzioneLance.Popolazione.ToString("#,0")}\r\n" +
                 $"Tempo di costruzione: {Strutture.Edifici.ProduzioneLance.TempoCostruzione.ToString()} s\r\n" +
                 $"Produzione risorse: {Strutture.Edifici.ProduzioneLance.Produzione.ToString()}");

             Server.Send(player.guid_Player, $"Descrizione|Produzione Archi|Workshop Archi questa struttura è attrezzata in modo da produrre equipaggiamento militare specifico,\r\n" +
                 $"essenziali per l'addestramento di unità militari, questa struttura produce Archi.\r\n\r\n" +
                 $"Costo Costruzione:\r\n" +
                 $"Cibo: {Strutture.Edifici.ProduzioneArchi.Cibo.ToString("#,0")}\r\n" +
                 $"Legno: {Strutture.Edifici.ProduzioneArchi.Legno.ToString("#,0")}\r\n" +
                 $"Pietra: {Strutture.Edifici.ProduzioneArchi.Pietra.ToString("#,0")}\r\n" +
                 $"Ferro: {Strutture.Edifici.ProduzioneArchi.Ferro.ToString("#,0")}\r\n" +
                 $"Oro: {Strutture.Edifici.ProduzioneArchi.Oro.ToString("#,0")}\r\n" +
                 $"Popolazione: {Strutture.Edifici.ProduzioneArchi.Popolazione.ToString("#,0")}\r\n" +
                 $"Tempo di costruzione: {Strutture.Edifici.ProduzioneArchi.TempoCostruzione.ToString()} s\r\n" +
                 $"Produzione risorse: {Strutture.Edifici.ProduzioneArchi.Produzione.ToString()}");

             Server.Send(player.guid_Player, $"Descrizione|Produzione Scudi|Workshop Scudi questa struttura è attrezzata in modo da produrre equipaggiamento militare specifico,\r\n" +
                 $"essenziali per l'addestramento di unità militari, questa struttura produce Scudi.\r\n\r\n" +
                 $"Costo Costruzione:\r\n" +
                 $"Cibo: {Strutture.Edifici.ProduzioneScudi.Cibo.ToString("#,0")}\r\n" +
                 $"Legno: {Strutture.Edifici.ProduzioneScudi.Legno.ToString("#,0")}\r\n" +
                 $"Pietra: {Strutture.Edifici.ProduzioneScudi.Pietra.ToString("#,0")}\r\n" +
                 $"Ferro: {Strutture.Edifici.ProduzioneScudi.Ferro.ToString("#,0")}\r\n" +
                 $"Oro: {Strutture.Edifici.ProduzioneScudi.Oro.ToString("#,0")}\r\n" +
                 $"Popolazione: {Strutture.Edifici.ProduzioneScudi.Popolazione.ToString("#,0")}\r\n" +
                 $"Tempo di costruzione: {Strutture.Edifici.ProduzioneScudi.TempoCostruzione.ToString()} s\r\n" +
                 $"Produzione risorse: {Strutture.Edifici.ProduzioneScudi.Produzione.ToString()}");

             Server.Send(player.guid_Player, $"Descrizione|Produzione Armature|Workshop Armature questa struttura è attrezzata in modo da produrre equipaggiamento militare specifico,\r\n" +
                 $"essenziali per l'addestramento di unità militari, questa struttura produce Armature.\r\n\r\n" +
                 $"Costo Costruzione:\r\n" +
                 $"Cibo: {Strutture.Edifici.ProduzioneArmature.Cibo.ToString("#,0")}\r\n" +
                 $"Legno: {Strutture.Edifici.ProduzioneArmature.Legno.ToString("#,0")}\r\n" +
                 $"Pietra: {Strutture.Edifici.ProduzioneArmature.Pietra.ToString("#,0")}\r\n" +
                 $"Ferro: {Strutture.Edifici.ProduzioneArmature.Ferro.ToString("#,0")}\r\n" +
                 $"Oro: {Strutture.Edifici.ProduzioneArmature.Oro.ToString("#,0")}\r\n" +
                 $"Popolazione: {Strutture.Edifici.ProduzioneArmature.Popolazione.ToString("#,0")}\r\n" +
                 $"Tempo di costruzione: {Strutture.Edifici.ProduzioneArmature.TempoCostruzione.ToString()} s\r\n" +
                 $"Produzione risorse: {Strutture.Edifici.ProduzioneArmature.Produzione.ToString()}");

             Server.Send(player.guid_Player, $"Descrizione|Produzione Frecce|Workshop Frecce questa struttura è attrezzata in modo da produrre equipaggiamento militare specifico,\r\n" +
                 $"essenziali per l'addestramento di unità militari, questa struttura produce Frecce.\r\n\r\n" +
                 $"Costo Costruzione:\r\n" +
                 $"Cibo: {Strutture.Edifici.ProduzioneFrecce.Cibo.ToString("#,0")}\r\n" +
                 $"Legno: {Strutture.Edifici.ProduzioneFrecce.Legno.ToString("#,0")}\r\n" +
                 $"Pietra: {Strutture.Edifici.ProduzioneFrecce.Pietra.ToString("#,0")}\r\n" +
                 $"Ferro: {Strutture.Edifici.ProduzioneFrecce.Ferro.ToString("#,0")}\r\n" +
                 $"Oro: {Strutture.Edifici.ProduzioneFrecce.Oro.ToString("#,0")}\r\n" +
                 $"Popolazione: {Strutture.Edifici.ProduzioneFrecce.Popolazione.ToString("#,0")}\r\n" +
                 $"Tempo di costruzione: {Strutture.Edifici.ProduzioneFrecce.TempoCostruzione.ToString()} s\r\n" +
                 $"Produzione risorse: {Strutture.Edifici.ProduzioneFrecce.Produzione.ToString()}");

            Server.Send(player.guid_Player, $"Descrizione|Guerrieri|I guerrieri sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono,\r\n " +
                $"sono facili da reclutare e non chiedono molta manutenzione in cibo ed oro.\r\n\r\n" +
                $"Costo Addestramento:\r\n" +
                $"Cibo: {Esercito.CostoReclutamento.Guerrieri_1.Cibo.ToString("#,0")}                  Spade: {Esercito.CostoReclutamento.Guerrieri_1.Spade.ToString("#,0")}\r\n" +
                $"Legno: {Esercito.CostoReclutamento.Guerrieri_1.Legno.ToString("#,0")}               Lancie: {Esercito.CostoReclutamento.Guerrieri_1.Lance.ToString("#,0")}\r\n" +
                $"Pietra: {Esercito.CostoReclutamento.Guerrieri_1.Pietra.ToString("#,0")}                Archi: {Esercito.CostoReclutamento.Guerrieri_1.Archi.ToString("#,0")}\r\n" +
                $"Ferro: {Esercito.CostoReclutamento.Guerrieri_1.Ferro.ToString("#,0")}                 Scudi: {Esercito.CostoReclutamento.Guerrieri_1.Scudi.ToString("#,0")}\r\n" +
                $"Oro: {Esercito.CostoReclutamento.Guerrieri_1.Oro.ToString("#,0")}                    Armature: {Esercito.CostoReclutamento.Guerrieri_1.Armature.ToString("#,0")}\r\n \r\n" +
                $"Popolazione: {Esercito.CostoReclutamento.Guerrieri_1.Popolazione}\r\n" +
                $"Tempo di Addestramento: {Esercito.CostoReclutamento.Guerrieri_1.TempoReclutamento.ToString()} s\r\n" +
                $"Mantenimento Cibo: {Esercito.Unità.Guerrieri_1.Cibo.ToString()} s\r\n" +
                $"Mantenimento Oro: {Esercito.Unità.Guerrieri_1.Salario.ToString()} s\r\n \r\n" +
                $"Statistiche:\r\n" +
                $"Livello: {player.Guerriero_Livello.ToString("#,0")}\r\n" +
                $"Salute:  {(Esercito.Unità.Guerrieri_1.Salute + player.Guerriero_Livello).ToString("#,0")}\r\n" +
                $"Difesa:  {(Esercito.Unità.Guerrieri_1.Difesa + player.Guerriero_Livello).ToString("#,0")}\r\n" +
                $"Attacco: {(Esercito.Unità.Guerrieri_1.Attacco + player.Guerriero_Livello).ToString("#,0")}\r\n \r\n");

            Server.Send(player.guid_Player, $"Descrizione|Lanceri|I lanceri sono la spina dorsale di ogni esercito ben organizzato. Armati di lance,\r\n " +
                $"questi soldati costituiscono un baluardo formidabile contro gli assalti nemici.\r\n\r\n" +
                $"Costo Addestramento:\r\n" +
                $"Cibo: {Esercito.CostoReclutamento.Lanceri_1.Cibo.ToString("#,0")}                  Spade: {Esercito.CostoReclutamento.Lanceri_1.Spade.ToString("#,0")}\r\n" +
                $"Legno: {Esercito.CostoReclutamento.Lanceri_1.Legno.ToString("#,0")}               Lancie: {Esercito.CostoReclutamento.Lanceri_1.Lance.ToString("#,0")}\r\n" +
                $"Pietra: {Esercito.CostoReclutamento.Lanceri_1.Pietra.ToString("#,0")}                Archi: {Esercito.CostoReclutamento.Lanceri_1.Archi.ToString("#,0")}\r\n" +
                $"Ferro: {Esercito.CostoReclutamento.Lanceri_1.Ferro.ToString("#,0")}                 Scudi: {Esercito.CostoReclutamento.Lanceri_1.Scudi.ToString("#,0")}\r\n" +
                $"Oro: {Esercito.CostoReclutamento.Lanceri_1.Oro.ToString("#,0")}                    Armature: {Esercito.CostoReclutamento.Lanceri_1.Armature.ToString("#,0")}\r\n \r\n" +
                $"Popolazione: {Esercito.CostoReclutamento.Lanceri_1.Popolazione}\r\n" +
                $"Tempo di Addestramento: {Esercito.CostoReclutamento.Lanceri_1.TempoReclutamento.ToString()} s\r\n" +
                $"Mantenimento Cibo: {Esercito.Unità.Lanceri_1.Cibo.ToString()} s\r\n" +
                $"Mantenimento Oro: {Esercito.Unità.Lanceri_1.Salario.ToString()} s\r\n \r\n" +
                $"Statistiche:\r\n" +
                $"Livello: {player.Lancere_Livello.ToString("#,0")}\r\n" +
                $"Salute:  {(Esercito.Unità.Lanceri_1.Salute + player.Lancere_Livello).ToString("#,0")}\r\n" +
                $"Difesa:  {(Esercito.Unità.Lanceri_1.Difesa + player.Lancere_Livello).ToString("#,0")}\r\n" +
                $"Attacco: {(Esercito.Unità.Lanceri_1.Attacco + player.Lancere_Livello).ToString("#,0")}\r\n \r\n");

            Server.Send(player.guid_Player, $"Descrizione|Arceri|Gli arceri armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza,\r\n " +
                $"lanciando frecce mortali sulle linee nemiche, prima che possano avvicinarsi.\r\n\r\n" +
                $"Costo Addestramento:\r\n" +
                $"Cibo: {Esercito.CostoReclutamento.Arceri_1.Cibo.ToString("#,0")}                  Spade: {Esercito.CostoReclutamento.Arceri_1.Spade.ToString("#,0")}\r\n" +
                $"Legno: {Esercito.CostoReclutamento.Arceri_1.Legno.ToString("#,0")}               Lancie: {Esercito.CostoReclutamento.Arceri_1.Lance.ToString("#,0")}\r\n" +
                $"Pietra: {Esercito.CostoReclutamento.Arceri_1.Pietra.ToString("#,0")}                Archi: {Esercito.CostoReclutamento.Arceri_1.Archi.ToString("#,0")}\r\n" +
                $"Ferro: {Esercito.CostoReclutamento.Arceri_1.Ferro.ToString("#,0")}                 Scudi: {Esercito.CostoReclutamento.Arceri_1.Scudi.ToString("#,0")}\r\n" +
                $"Oro: {Esercito.CostoReclutamento.Arceri_1.Oro.ToString("#,0")}                    Armature: {Esercito.CostoReclutamento.Arceri_1.Armature.ToString("#,0")}\r\n \r\n" +
                $"Popolazione: {Esercito.CostoReclutamento.Arceri_1.Popolazione}\r\n" +
                $"Tempo di Addestramento: {Esercito.CostoReclutamento.Arceri_1.TempoReclutamento.ToString()} s\r\n" +
                $"Mantenimento Cibo: {Esercito.Unità.Arceri_1.Cibo.ToString()} s\r\n" +
                $"Mantenimento Oro: {Esercito.Unità.Arceri_1.Salario.ToString()} s\r\n \r\n" +
                $"Statistiche:\r\n" +
                $"Livello: {player.Arcere_Livello.ToString("#,0")}\r\n" +
                $"Salute:  {(Esercito.Unità.Arceri_1.Salute + player.Arcere_Livello).ToString("#,0")}\r\n" +
                $"Difesa:  {(Esercito.Unità.Arceri_1.Difesa + player.Arcere_Livello).ToString("#,0")}\r\n" +
                $"Attacco: {(Esercito.Unità.Arceri_1.Attacco + player.Arcere_Livello).ToString("#,0")}\r\n \r\n");

            Server.Send(player.guid_Player, $"Descrizione|Catapulte|Le catapulte sono potenti macchine d'assedio che cambiano le sorti delle battaglie,\r\n " +
                $"scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche\r\n\r\n" +
                $"Costo Addestramento:\r\n" +
                $"Cibo: {Esercito.CostoReclutamento.Catapulte_1.Cibo.ToString("#,0")}                  Spade: {Esercito.CostoReclutamento.Catapulte_1.Spade.ToString("#,0")}\r\n" +
                $"Legno: {Esercito.CostoReclutamento.Catapulte_1.Legno.ToString("#,0")}               Lancie: {Esercito.CostoReclutamento.Catapulte_1.Lance.ToString("#,0")}\r\n" +
                $"Pietra: {Esercito.CostoReclutamento.Catapulte_1.Pietra.ToString("#,0")}                Archi: {Esercito.CostoReclutamento.Catapulte_1.Archi.ToString("#,0")}\r\n" +
                $"Ferro: {Esercito.CostoReclutamento.Catapulte_1.Ferro.ToString("#,0")}                 Scudi: {Esercito.CostoReclutamento.Catapulte_1.Scudi.ToString("#,0")}\r\n" +
                $"Oro: {Esercito.CostoReclutamento.Catapulte_1.Oro.ToString("#,0")}                    Armature: {Esercito.CostoReclutamento.Catapulte_1.Armature.ToString("#,0")}\r\n \r\n" +
                $"Popolazione: {Esercito.CostoReclutamento.Catapulte_1.Popolazione}\r\n" +
                $"Tempo di Addestramento: {Esercito.CostoReclutamento.Catapulte_1.TempoReclutamento.ToString()} s\r\n" +
                $"Mantenimento Cibo: {Esercito.Unità.Catapulte_1.Cibo.ToString("0.00")} s\r\n" +
                $"Mantenimento Oro: {Esercito.Unità.Catapulte_1.Salario.ToString("0.00")} s\r\n \r\n" +
                $"Statistiche:\r\n" +
                $"Livello: {player.Catapulta_Livello.ToString("#,0")}\r\n" +
                $"Salute:  {(Esercito.Unità.Catapulte_1.Salute + player.Catapulta_Livello).ToString("#,0")}\r\n" +
                $"Difesa:  {(Esercito.Unità.Catapulte_1.Difesa + player.Catapulta_Livello).ToString("#,0")}\r\n" +
                $"Attacco: {(Esercito.Unità.Catapulte_1.Attacco + player.Catapulta_Livello).ToString("#,0")}\r\n");

            Server.Send(player.guid_Player, $"Descrizione|Esperienza|L’esperienza rappresenta la crescita del giocatore nel tempo.\r\nAccumulare esperienza permette di salire di livello\r\n\r\n");
            Server.Send(player.guid_Player, $"Descrizione|Livello|Il livello indica il grado di avanzamento del giocatore.\r\nLivelli più alti richiedono quantità crescenti di esperienza, ma garantiscono vantaggi permanenti e prestigio.\r\n\r\n");
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


        public static async Task<bool> Update_Data(Guid guid, string username, string password)
        {
            var player = Server.servers_.GetPlayer(username, password);
            var buildingsQueue = BuildingManager.GetQueuedBuildings(player);
            var unitsQueue = UnitManager.GetQueuedUnits(player);

            double Cibo = 0;
            double Oro = 0;

            Cibo -= player.Guerrieri[0] * Esercito.Unità.Guerrieri_1.Cibo + player.Lanceri[0] * Esercito.Unità.Lanceri_1.Cibo + player.Arceri[0] * Esercito.Unità.Arceri_1.Cibo + player.Catapulte[0] * Esercito.Unità.Catapulte_1.Cibo;
            Cibo -= player.Guerrieri[1] * Esercito.Unità.Guerriero_2.Cibo + player.Lanceri[1] * Esercito.Unità.Lancere_2.Cibo + player.Arceri[1] * Esercito.Unità.Arcere_2.Cibo + player.Catapulte[1] * Esercito.Unità.Catapulta_2.Cibo;
            Cibo -= player.Guerrieri[2] * Esercito.Unità.Guerriero_3.Cibo + player.Lanceri[2] * Esercito.Unità.Lancere_3.Cibo + player.Arceri[2] * Esercito.Unità.Arcere_3.Cibo + player.Catapulte[2] * Esercito.Unità.Catapulta_3.Cibo;
            Cibo -= player.Guerrieri[3] * Esercito.Unità.Guerriero_4.Cibo + player.Lanceri[3] * Esercito.Unità.Lancere_4.Cibo + player.Arceri[3] * Esercito.Unità.Arcere_4.Cibo + player.Catapulte[3] * Esercito.Unità.Catapulta_4.Cibo;
            Cibo -= player.Guerrieri[4] * Esercito.Unità.Guerriero_5.Cibo + player.Lanceri[4] * Esercito.Unità.Lancere_5.Cibo + player.Arceri[4] * Esercito.Unità.Arcere_5.Cibo + player.Catapulte[4] * Esercito.Unità.Catapulta_5.Cibo;

            Oro -= player.Guerrieri[0] * Esercito.Unità.Guerrieri_1.Salario + player.Lanceri[0] * Esercito.Unità.Lanceri_1.Salario + player.Arceri[0] * Esercito.Unità.Arceri_1.Salario + player.Catapulte[0] * Esercito.Unità.Catapulte_1.Salario;
            Oro -= player.Guerrieri[1] * Esercito.Unità.Guerriero_2.Salario + player.Lanceri[1] * Esercito.Unità.Lancere_2.Salario + player.Arceri[1] * Esercito.Unità.Arcere_2.Salario + player.Catapulte[1] * Esercito.Unità.Catapulta_2.Salario;
            Oro -= player.Guerrieri[2] * Esercito.Unità.Guerriero_3.Salario + player.Lanceri[2] * Esercito.Unità.Lancere_3.Salario + player.Arceri[2] * Esercito.Unità.Arcere_3.Salario + player.Catapulte[2] * Esercito.Unità.Catapulta_3.Salario;
            Oro -= player.Guerrieri[3] * Esercito.Unità.Guerriero_4.Salario + player.Lanceri[3] * Esercito.Unità.Lancere_4.Salario + player.Arceri[3] * Esercito.Unità.Arcere_4.Salario + player.Catapulte[3] * Esercito.Unità.Catapulta_4.Salario;
            Oro -= player.Guerrieri[4] * Esercito.Unità.Guerriero_5.Salario + player.Lanceri[4] * Esercito.Unità.Lancere_5.Salario + player.Arceri[4] * Esercito.Unità.Arcere_5.Salario + player.Catapulte[4] * Esercito.Unità.Catapulta_5.Salario;

            QuestManager.QuestUpdate(player);
            QuestManager.QuestRewardUpdate(player);
            DescUpdate(player);

            string data =
            "Update_Data|" +

            //Dati utente
            $"livello={player.Livello}|" +
            $"esperienza={player.Esperienza}|" +
            $"vip={player.Vip}|" +
            $"punti_quest={player.Punti_Quest}|" +
            $"costo_terreni_Virtuali={Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola}|" +

            //Risorse
            $"cibo={player.Cibo:#,0}|" +
            $"legna={player.Legno:#,0}|" +
            $"pietra={player.Pietra:#,0}|" +
            $"ferro={player.Ferro:#,0}|" +
            $"oro={player.Oro:#,0}|" +
            $"popolazione={player.Popolazione:#,0}|" +

            $"dollari_virtuali={player.Dollari_Virtuali}|" +
            $"diamanti_blu={player.Diamanati_Blu}|" +
            $"diamanti_viola={player.Diamanti_Viola}|" +

            $"cibo_max={Edifici.Fattoria.Limite:#,0}|" +
            $"legno_max={Edifici.Segheria.Limite:#,0}|" +
            $"pietra_max={Edifici.CavaPietra.Limite:#,0}|" +
            $"ferro_max={Edifici.MinieraFerro.Limite:#,0}|" +
            $"oro_max={Edifici.MinieraOro.Limite:#,0}|" +
            $"popolazione_max={Edifici.Case.Limite:#,0}|" +

            //Produzione Risorse
            $"cibo_s={player.Fattoria * (Strutture.Edifici.Fattoria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Cibo):#,0.000}|" +
            $"legna_s={player.Segheria * (Strutture.Edifici.Segheria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Legno):#,0.000}|" +
            $"pietra_s={player.CavaPietra * (Strutture.Edifici.CavaPietra.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Pietra):#,0.000}|" +
            $"ferro_s={player.MinieraFerro * (Strutture.Edifici.MinieraFerro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Ferro):#,0.000}|" +
            $"oro_s={player.MinieraOro * (Strutture.Edifici.MinieraOro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Oro):#,0.00}|" +
            $"popolazione_s={player.Abitazioni * (Strutture.Edifici.Case.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Popolazione):#,0.000}|" +

            $"spade_s={player.Workshop_Spade * (Strutture.Edifici.ProduzioneSpade.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Spade):#,0.000}|" +
            $"lance_s={player.Workshop_Lance * (Strutture.Edifici.ProduzioneLance.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Lance):#,0.000}|" +
            $"archi_s={player.Workshop_Archi * (Strutture.Edifici.ProduzioneArchi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Archi):#,0.000}|" +
            $"scudi_s={player.Workshop_Scudi * (Strutture.Edifici.ProduzioneScudi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Scudi):#,0.000}|" +
            $"armature_s={player.Workshop_Armature * (Strutture.Edifici.ProduzioneArmature.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Armature):#,0.000}|" +
            $"frecce_s={player.Workshop_Frecce * (Strutture.Edifici.ProduzioneFrecce.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Popolazione):#,0.000}|" +

            $"consumo_cibo_s={Cibo:#,0.00}|" + //Esercito
            $"consumo_oro_s={Oro:#,0.00}|" + //Esercito

            //Limiti Risorse
            $"cibo_limite={player.Fattoria * Strutture.Edifici.Fattoria.Limite:#,0.00}|" +
            $"legna_limite={player.Segheria * Strutture.Edifici.Segheria.Limite:#,0.00}|" +
            $"pietra_limite={player.CavaPietra * Strutture.Edifici.CavaPietra.Limite :#,0.00}|" +
            $"ferro_limite={player.MinieraFerro * Strutture.Edifici.MinieraFerro.Limite :#,0.00}|" +
            $"oro_limite={player.MinieraOro * Strutture.Edifici.MinieraOro.Limite:#,0.00}|" +
            $"popolazione_limite={player.Abitazioni * Strutture.Edifici.Case.Limite:#,0.00}|" +

            $"spade_limite={player.Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Limite :#,0.00}|" +
            $"lance_limite={player.Workshop_Lance * Strutture.Edifici.ProduzioneLance.Limite:#,0.00}|" +
            $"archi_limite={player.Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Limite:#,0.00}|" +
            $"scudi_limite={player.Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Limite:#,0.00}|" +
            $"armature_limite={player.Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Limite:#,0.00}|" +
            $"frecce_limite={player.Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Limite:#,0.00}|" +

            //Risorse Militari
            $"spade={player.Spade:#,0.00}|" +
            $"lance={player.Lance:#,0.00}|" +
            $"archi={player.Archi:#,0.00}|" +
            $"scudi={player.Scudi:#,0.00}|" +
            $"armature={player.Armature:#,0.00}|" +
            $"frecce={player.Frecce:#,0}|" +

            $"spade_max={Edifici.Fattoria.Limite:#,0}|" +
            $"lance_max={Edifici.Fattoria.Limite:#,0}|" +
            $"archi_max={Edifici.Fattoria.Limite:#,0}|" +
            $"scudi_max={Edifici.Fattoria.Limite:#,0}|" +
            $"armature_max={player.Armature:#,0}|" +
            $"frecce_max={player.Frecce:#,0}|" +

            //Strutture Civili
            $"fattorie={player.Fattoria}|" +
            $"segherie={player.Segheria}|" +
            $"cave_pietra={player.CavaPietra}|" +
            $"miniere_ferro={player.MinieraFerro}|" +
            $"miniere_oro={player.MinieraOro}|" +
            $"case={player.Abitazioni}|" +

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
            $"caserma_lancieri={player.Caserma_Lancieri}|" +
            $"caserma_arceri={player.Caserma_Arceri}|" +
            $"caserma_catapulte={player.Caserma_Catapulte}|" +

            $"guerrieri_max={player.GuerrieriMax}|" +
            $"lanceri_max={player.LancieriMax}|" +
            $"arceri_max={player.ArceriMax}|" +
            $"catapulte_max={player.CatapulteMax}|" +

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

            $"caserme_guerrieri_coda={buildingsQueue.GetValueOrDefault("CasermaGuerrieri", 0)}|" +
            $"caserme_lanceri_coda={buildingsQueue.GetValueOrDefault("CasermaLanceri", 0)}|" +
            $"caserme_arceri_coda={buildingsQueue.GetValueOrDefault("CasermaArceri", 0)}|" +
            $"caserme_catapulte_coda={buildingsQueue.GetValueOrDefault("CasermaCatapulte", 0)}|" +

            // Code unità
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

            $"Tempo_Costruzione={BuildingManager.Get_Total_Building_Time(player)}|" +
            $"Tempo_Reclutamento={UnitManager.Get_Total_Recruit_Time(player)}|" +
            $"Tempo_Ricerca_Citta={ResearchManager.GetTotalResearchTime(player)}|" +
            $"Tempo_Ricerca_Globale={1}|" +

            $"Ricerca_Attiva={player.Ricerca_Attiva}|" +

            //Statistiche
            $"Unità_Uccise={player.Unità_Uccise}|" +
            $"Guerrieri_Uccisi={player.Guerrieri_Uccisi}|" +
            $"Lanceri_Uccisi={player.Lanceri_Uccisi}|" +
            $"Arceri_Uccisi={player.Arceri_Uccisi}|" +
            $"Catapulte_Uccisi={player.Catapulte_Uccisi}|" +
            $"Unità_Perse={player.Unità_Perse}|" +
            $"Guerrieri_Persi={player.Guerrieri_Persi}|" +
            $"Lanceri_Persi={player.Lanceri_Persi}|" +
            $"Arceri_Persi={player.Arceri_Persi}|" +
            $"Catapulte_Persi={player.Catapulte_Persi}|" +
            $"Risorse_Razziate={player.Risorse_Razziate}|" +
            $"Frecce_Utilizzate={player.Frecce_Utilizzate}|" +
            $"Battaglie_Vinte={player.Battaglie_Vinte}|" +
            $"Battaglie_Perse={player.Battaglie_Perse}|" +
            $"Barbari_Sconfitti={player.Barbari_Sconfitti}|" +
            $"Accampamenti_Barbari_Sconfitti={player.Accampamenti_Barbari_Sconfitti}|" +
            $"Città_Barbare_Sconfitte={player.Città_Barbare_Sconfitte}|" +
            $"Missioni_Completate={player.Missioni_Completate}|" +
            $"Attacchi_Subiti_PVP={player.Attacchi_Subiti_PVP}|" +
            $"Attacchi_Effettuati_PVP={player.Attacchi_Effettuati_PVP}|" +
            $"Unità_Addestrate={player.Unità_Addestrate}|" +
            $"Risorse_Utilizzate={player.Risorse_Utilizzate}|" +
            $"Tempo_Addestramento_Risparmiato={player.Tempo_Addestramento_Risparmiato}|" +
            $"Tempo_Costruzione_Risparmiato={player.Tempo_Costruzione_Risparmiato}|";

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


            stringa_Base = "";
            stringa_Base = $"{Server.Utenti_PVP.Count}";
            string stringa = "";
            foreach (var item in Server.Utenti_PVP)
                stringa = await costruisci_stringa(item);
            if (stringa != "")
                Server.Send(guid, $"Update_PVP_Player|" +
                    $"{stringa}|");
            return true;
        }
        static async Task<string> costruisci_stringa (string dato)
        {
            stringa_Base = stringa_Base + "|" + dato;
            return stringa_Base;
        }
    }
}
