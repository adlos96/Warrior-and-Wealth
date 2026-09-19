using Server_Strategico.Gioco;
using static Server_Strategico.Manager.QuestManager;
using static Server_Strategico.ServerData.Moduli.Battaglie.Battaglia;

namespace Server_Strategico.ServerData.Moduli.Battaglie
{
    public class BattagliaPVP
    {
        // SendClient, GetPlayerUnitStats, GetUnitStats, CalcoloFrecce, CapacitàCarico, RidurreNumeroSoldati,
        // ApplicaDanni, ApplicaDanniDistanza_, CalcolaForza e RaccoliRisorseEquamente sono stati spostati in
        // Battaglia.cs il 2026-09-14 (erano duplicati letteralmente identici tra BattagliaPVP.cs e BattagliaPVE.cs).
        // Restano richiamabili qui senza prefisso grazie a "using static ...Battaglia;" in cima al file.
        public static UnitGroup CaricaDatiStruttureDifensore(Giocatori.Player difensore, int struttura)
        {
            var defenderUnits = new UnitGroup
            {
                Guerrieri = [0, 0, 0, 0, 0],
                Lancieri = [0, 0, 0, 0, 0],
                Arcieri = [0, 0, 0, 0, 0],
                Catapulte = [0, 0, 0, 0, 0]
            };
            if (struttura == 1)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri_Ingresso;
                defenderUnits.Lancieri = difensore.Lanceri_Ingresso;
                defenderUnits.Arcieri = difensore.Arceri_Ingresso;
                defenderUnits.Catapulte = difensore.Catapulte_Ingresso;
            }
            if (struttura == 2)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri_Mura;
                defenderUnits.Lancieri = difensore.Lanceri_Mura;
                defenderUnits.Arcieri = difensore.Arceri_Mura;
                defenderUnits.Catapulte = difensore.Catapulte_Mura;
            }
            if (struttura == 3)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri_Cancello;
                defenderUnits.Lancieri = difensore.Lanceri_Cancello;
                defenderUnits.Arcieri = difensore.Arceri_Cancello;
                defenderUnits.Catapulte = difensore.Catapulte_Cancello;
            }
            if (struttura == 4)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri_Torri;
                defenderUnits.Lancieri = difensore.Lanceri_Torri;
                defenderUnits.Arcieri = difensore.Arceri_Torri;
                defenderUnits.Catapulte = difensore.Catapulte_Torri;
            }
            if (struttura == 5)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri_Citta;
                defenderUnits.Lancieri = difensore.Lanceri_Citta;
                defenderUnits.Arcieri = difensore.Arceri_Citta;
                defenderUnits.Catapulte = difensore.Catapulte_Citta;
            }
            if (struttura == 6)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri_Castello;
                defenderUnits.Lancieri = difensore.Lanceri_Castello;
                defenderUnits.Arcieri = difensore.Arceri_Castello;
                defenderUnits.Catapulte = difensore.Catapulte_Castello;
            }
            if (struttura == 7)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri;
                defenderUnits.Lancieri = difensore.Lanceri;
                defenderUnits.Arcieri = difensore.Arceri;
                defenderUnits.Catapulte = difensore.Catapulte;
            }
            return defenderUnits;
        }
        static void AggiornaDatiStruttureDifensore(int struttura, Giocatori.Player difensore, RisultatoFase result)
        {
            if (struttura == 1)
            {
                difensore.Guerrieri_Ingresso = result.Difensore.Sopravvisuti.Guerrieri;
                difensore.Lanceri_Ingresso = result.Difensore.Sopravvisuti.Lancieri;
                difensore.Arceri_Ingresso = result.Difensore.Sopravvisuti.Arcieri;
                difensore.Catapulte_Ingresso = result.Difensore.Sopravvisuti.Catapulte;
            }
            if (struttura == 2)
            {
                difensore.Salute_Mura = result.Struttura.Salute;
                difensore.Difesa_Mura = result.Struttura.Difesa;
                if (result.Struttura.Salute == 0)
                {
                    result.Perdite_Crollo.Guerrieri = difensore.Guerrieri_Mura;
                    result.Perdite_Crollo.Lancieri = difensore.Lanceri_Mura;
                    result.Perdite_Crollo.Arcieri = difensore.Arceri_Mura;
                    result.Perdite_Crollo.Catapulte = difensore.Catapulte_Mura;

                    difensore.Guerrieri_Mura = [0, 0, 0, 0, 0];
                    difensore.Lanceri_Mura = [0, 0, 0, 0, 0];
                    difensore.Arceri_Mura = [0, 0, 0, 0, 0];
                    difensore.Catapulte_Mura = [0, 0, 0, 0, 0];
                }
                else
                {
                    difensore.Guerrieri_Mura = result.Difensore.Sopravvisuti.Guerrieri;
                    difensore.Lanceri_Mura = result.Difensore.Sopravvisuti.Lancieri;
                    difensore.Arceri_Mura = result.Difensore.Sopravvisuti.Arcieri;
                    difensore.Catapulte_Mura = result.Difensore.Sopravvisuti.Catapulte;
                }
            }
            if (struttura == 3)
            {
                difensore.Salute_Cancello = result.Struttura.Salute;
                difensore.Difesa_Cancello = result.Struttura.Difesa;
                if (result.Struttura.Salute == 0) //Distruzione struttura
                {
                    result.Perdite_Crollo.Guerrieri = difensore.Guerrieri_Cancello;
                    result.Perdite_Crollo.Lancieri = difensore.Lanceri_Cancello;
                    result.Perdite_Crollo.Arcieri = difensore.Arceri_Cancello;
                    result.Perdite_Crollo.Catapulte = difensore.Catapulte_Cancello;

                    difensore.Guerrieri_Cancello = [0, 0, 0, 0, 0];
                    difensore.Lanceri_Cancello = [0, 0, 0, 0, 0];
                    difensore.Arceri_Cancello = [0, 0, 0, 0, 0];
                    difensore.Catapulte_Cancello = [0, 0, 0, 0, 0];
                }
                else
                {
                    difensore.Guerrieri_Cancello = result.Difensore.Sopravvisuti.Guerrieri;
                    difensore.Lanceri_Cancello = result.Difensore.Sopravvisuti.Lancieri;
                    difensore.Arceri_Cancello = result.Difensore.Sopravvisuti.Arcieri;
                    difensore.Catapulte_Cancello = result.Difensore.Sopravvisuti.Catapulte;
                }
            }
            if (struttura == 4)
            {
                difensore.Salute_Torri = result.Struttura.Salute;
                difensore.Difesa_Torri = result.Struttura.Difesa;

                if (result.Struttura.Salute == 0)
                {
                    result.Perdite_Crollo.Guerrieri = difensore.Guerrieri_Torri;
                    result.Perdite_Crollo.Lancieri = difensore.Lanceri_Torri;
                    result.Perdite_Crollo.Arcieri = difensore.Arceri_Torri;
                    result.Perdite_Crollo.Catapulte = difensore.Catapulte_Torri;

                    difensore.Guerrieri_Torri = [0, 0, 0, 0, 0];
                    difensore.Lanceri_Torri = [0, 0, 0, 0, 0];
                    difensore.Arceri_Torri = [0, 0, 0, 0, 0];
                    difensore.Catapulte_Torri = [0, 0, 0, 0, 0];
                }
                else
                {
                    difensore.Guerrieri_Torri = result.Difensore.Sopravvisuti.Guerrieri;
                    difensore.Lanceri_Torri = result.Difensore.Sopravvisuti.Lancieri;
                    difensore.Arceri_Torri = result.Difensore.Sopravvisuti.Arcieri;
                    difensore.Catapulte_Torri = result.Difensore.Sopravvisuti.Catapulte;
                }
            }
            if (struttura == 5)
            {
                difensore.Guerrieri_Citta = result.Difensore.Sopravvisuti.Guerrieri;
                difensore.Lanceri_Citta = result.Difensore.Sopravvisuti.Lancieri;
                difensore.Arceri_Citta = result.Difensore.Sopravvisuti.Arcieri;
                difensore.Catapulte_Citta = result.Difensore.Sopravvisuti.Catapulte;
            }
            if (struttura == 6)
            {
                difensore.Salute_Castello = result.Struttura.Salute;
                difensore.Difesa_Castello = result.Struttura.Difesa;

                if (result.Struttura.Salute == 0)
                {
                    result.Perdite_Crollo.Guerrieri = difensore.Guerrieri_Castello;
                    result.Perdite_Crollo.Lancieri = difensore.Lanceri_Castello;
                    result.Perdite_Crollo.Arcieri = difensore.Arceri_Castello;
                    result.Perdite_Crollo.Catapulte = difensore.Catapulte_Castello;

                    difensore.Guerrieri_Castello = [0, 0, 0, 0, 0];
                    difensore.Lanceri_Castello = [0, 0, 0, 0, 0];
                    difensore.Arceri_Castello = [0, 0, 0, 0, 0];
                    difensore.Catapulte_Castello = [0, 0, 0, 0, 0];
                }
                else
                {
                    difensore.Guerrieri_Castello = result.Difensore.Sopravvisuti.Guerrieri;
                    difensore.Lanceri_Castello = result.Difensore.Sopravvisuti.Lancieri;
                    difensore.Arceri_Castello = result.Difensore.Sopravvisuti.Arcieri;
                    difensore.Catapulte_Castello = result.Difensore.Sopravvisuti.Catapulte;
                }
            }
            if (struttura == 7)
            {
                difensore.Guerrieri = result.Difensore.Sopravvisuti.Guerrieri;
                difensore.Lanceri = result.Difensore.Sopravvisuti.Lancieri;
                difensore.Arceri = result.Difensore.Sopravvisuti.Arcieri;
                difensore.Catapulte = result.Difensore.Sopravvisuti.Catapulte;
            }
        }
        private static int CalcolaEsperienzaPVP(UnitGroup casualties)
        {
            int esperienza = 0;
            for (int i = 0; i < 5; i++)
            {
                var stats = GetUnitStats(i);
                esperienza += casualties.Guerrieri[i] * stats.GuerrieriEsperienza;
                esperienza += casualties.Lancieri[i] * stats.LancieriEsperienza;
                esperienza += casualties.Arcieri[i] * stats.ArcieriEsperienza;
                esperienza += casualties.Catapulte[i] * stats.CatapulteEsperienza;
            }
            return esperienza;
        }
        private static BattagliaDistanza CalcolaAttaccoDistanza_(UnitGroup units, Giocatori.Player player, BattagliaDistanza result, bool difensore)
        {
            int totaleArceri = units.Arcieri.Sum();
            int totaleCatapulte = units.Catapulte.Sum();
            bool frecce_Insufficienti = false;

            if (totaleArceri == 0 && totaleCatapulte == 0)
            {
                //SendClient(clientGuid, "Log_Server|Nessuna unità a distanza disponibile.");
                return result;
            }

            int frecceNecessarie = CalcoloFrecce(units); // Calcola frecce necessarie

            // Calcola potenza d'attacco base
            int arcieriEffettivi = totaleArceri * 3 / 7;
            int catapulteEffettive = totaleCatapulte * 3 / 6;

            // Bonus per poche unità (sono più precise)
            if (totaleArceri > 0 && totaleArceri <= 15) arcieriEffettivi = totaleArceri * 3 / 6;
            if (totaleCatapulte > 0 && totaleCatapulte <= 10) catapulteEffettive = totaleCatapulte * 3 / 5;

            if (player.Frecce < frecceNecessarie) // Gestione frecce insufficienti
            {
                frecce_Insufficienti = true;
                arcieriEffettivi /= 3;
                catapulteEffettive /= 4;
            }

            int danno = arcieriEffettivi + catapulteEffettive;
            int attaccoTotale = danno > 0 ? danno : 0;
            if (attaccoTotale > 0)
            {
                int dannoGuerrieri = attaccoTotale * 3 / 5;
                int dannoLancieri = attaccoTotale * 2 / 5;

                if (difensore)
                {
                    result.Difensore_Danno_Guerrieri = dannoGuerrieri;
                    result.Difensore_Danno_Lancieri = dannoLancieri;
                }
                else
                {
                    result.Attaccante_Danno_Guerrieri = dannoGuerrieri;
                    result.Attaccante_Danno_Lancieri = dannoLancieri;
                }
            }

            int frecceUsate = frecce_Insufficienti ? (int)player.Frecce : frecceNecessarie; // Calcola frecce usate (tutte quelle disponibili se insufficienti)
            player.Frecce_Utilizzate += frecceUsate;
            player.Frecce -= frecceUsate;

            if (difensore)
            {
                result.Difensore_Frecce_Necessarie = frecceNecessarie;
                result.Difensore_Frecce_Usate = frecceUsate;
            }
            else
            {
                result.Attaccante_Frecce_Necessarie = frecceNecessarie;
                result.Attaccante_Frecce_Usate = frecceUsate;
            }
            return result;
        }
        private static double CalcolaDannoGiocatore(UnitGroup units, Giocatori.Player player, bool usaFrecce, bool attaccante, RisultatoFase result)
        {
            double dannoTotale = 0, moltiplicatoreDistanza = 1.0;
            int frecceNecessarie = CalcoloFrecce(units);

            if (usaFrecce)
            {
                bool frecceSufficienti = player.Frecce >= frecceNecessarie;
                int frecceUsate = frecceSufficienti ? frecceNecessarie : (int)player.Frecce;

                moltiplicatoreDistanza = frecceSufficienti ? 1.0 : 0.33;
                player.Frecce_Utilizzate += frecceUsate;
                player.Frecce = frecceSufficienti ? player.Frecce - frecceNecessarie : 0;

                if (attaccante)
                {
                    result.Fase_Distanza.Attaccante_Frecce_Usate = frecceUsate;
                    result.Fase_Distanza.Attaccante_Poche_Frecce = !frecceSufficienti;
                    if (!frecceSufficienti)
                        result.Fase_Distanza.Attaccante_Frecce_Necessarie = frecceNecessarie;
                }
                else
                {
                    result.Fase_Distanza.Difensore_Frecce_Usate = frecceUsate;
                    result.Fase_Distanza.Difensore_Poche_Frecce = !frecceSufficienti;
                    if (!frecceSufficienti)
                        result.Fase_Distanza.Difensore_Frecce_Necessarie = frecceNecessarie;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                var stats = GetPlayerUnitStats(i, player);
                dannoTotale += units.Guerrieri[i] * stats.GuerrieriAttacco;
                dannoTotale += units.Lancieri[i] * stats.LancieriAttacco;
                dannoTotale += units.Arcieri[i] * stats.ArcieriAttacco * moltiplicatoreDistanza;
                dannoTotale += units.Catapulte[i] * stats.CatapulteAttacco * moltiplicatoreDistanza;
            }
            return dannoTotale;
        }
        private static RisultatoFase ApplicaDanniGiocatore(RisultatoFase battle, UnitGroup units, Giocatori.Player player, double dannoPerTipo, double bonusUnità, bool attacco)
        {
            for (int i = 0; i < 5; i++)
            {
                var stats = GetPlayerUnitStats(i, player); //Qui ci sono stats unità con bonus applicati

                // Guerrieri
                int guerrieriIniziali = units.Guerrieri[i];
                units.Guerrieri[i] = RidurreNumeroSoldati(guerrieriIniziali, dannoPerTipo, stats.GuerrieriDifesa * guerrieriIniziali * bonusUnità, stats.GuerrieriSalute * bonusUnità);
                if (attacco == true)
                {
                    battle.Attaccante.Sopravvisuti.Guerrieri[i] = units.Guerrieri[i];
                    battle.Attaccante.Perdite.Guerrieri[i] += guerrieriIniziali - units.Guerrieri[i];
                }
                if (attacco == false)
                {
                    battle.Difensore.Sopravvisuti.Guerrieri[i] = units.Guerrieri[i];
                    battle.Difensore.Perdite.Guerrieri[i] += guerrieriIniziali - units.Guerrieri[i];
                }

                // Lancieri
                int lancieriIniziali = units.Lancieri[i];
                units.Lancieri[i] = RidurreNumeroSoldati(lancieriIniziali, dannoPerTipo, stats.LancieriDifesa * lancieriIniziali * bonusUnità, stats.LancieriSalute * bonusUnità);
                if (attacco == true)
                {
                    battle.Attaccante.Sopravvisuti.Lancieri[i] = units.Lancieri[i];
                    battle.Attaccante.Perdite.Lancieri[i] += lancieriIniziali - units.Lancieri[i];
                }
                if (attacco == false)
                {
                    battle.Difensore.Sopravvisuti.Lancieri[i] = units.Lancieri[i];
                    battle.Difensore.Perdite.Lancieri[i] += lancieriIniziali - units.Lancieri[i];
                }

                // Arcieri
                int arcieriIniziali = units.Arcieri[i];
                units.Arcieri[i] = RidurreNumeroSoldati(arcieriIniziali, dannoPerTipo, stats.ArcieriDifesa * arcieriIniziali * bonusUnità, stats.ArcieriSalute * bonusUnità);
                if (attacco == true)
                {
                    battle.Attaccante.Sopravvisuti.Arcieri[i] = units.Arcieri[i];
                    battle.Attaccante.Perdite.Arcieri[i] += arcieriIniziali - units.Arcieri[i];
                }
                if (attacco == false)
                {
                    battle.Difensore.Sopravvisuti.Arcieri[i] = units.Arcieri[i];
                    battle.Difensore.Perdite.Arcieri[i] += arcieriIniziali - units.Arcieri[i];
                }

                // Catapulte
                int catapulteIniziali = units.Catapulte[i];
                units.Catapulte[i] = RidurreNumeroSoldati(catapulteIniziali, dannoPerTipo, stats.CatapulteDifesa * catapulteIniziali * bonusUnità, stats.CatapulteSalute * bonusUnità);
                if (attacco == true)
                {
                    battle.Attaccante.Sopravvisuti.Catapulte[i] = units.Catapulte[i];
                    battle.Attaccante.Perdite.Catapulte[i] += catapulteIniziali - units.Catapulte[i];
                }
                if (attacco == false)
                {
                    battle.Difensore.Sopravvisuti.Catapulte[i] = units.Catapulte[i];
                    battle.Difensore.Perdite.Catapulte[i] += catapulteIniziali - units.Catapulte[i];
                }
            }
            return battle;
        }
        private static async Task<Report> AssegnaRisorseVittoria_PvP(Giocatori.Player attaccante, Giocatori.Player difensore, Guid attackerGuid, UnitGroup sopravvissuti, Report report)
        {
            // Bilanciamento: nel saccheggio PVP le truppe sopravvissute trasportano solo 1/5 della loro capacità di carico
            // totale (a differenza del PVE, dove presumibilmente si sfrutta la capacità piena). Nerf intenzionale.
            int capacitàCarico = CapacitàCarico(sopravvissuti, attaccante) / 5;
            int capacitàOriginale = capacitàCarico;

            // Il 50% delle risorse del difensore può essere rubato
            double cibo = difensore.Cibo / 2;
            double legno = difensore.Legno / 2;
            double pietra = difensore.Pietra / 2;
            double ferro = difensore.Ferro / 2;
            double oro = difensore.Oro / 2;
            // BUGFIX: il margine va calcolato come minimo tra il residuo del bersaglio E il residuo giornaliero dell'attaccante,
            // altrimenti un attaccante vicino al tetto giornaliero può comunque superarlo in un singolo raid.
            int diamantiVioleResiduiBersaglio = Math.Max(0, Variabili_Server.Max_Diamanti_Viola_PVP_Giocatore - difensore.Diamanti_Viola_PVP_Persi);
            int diamantiVioleResiduiAttaccante = Math.Max(0, Variabili_Server.Max_Diamanti_Viola_PVP - attaccante.Diamanti_Viola_PVP_Ottenuti);
            int diamantiViola = Math.Min(diamantiVioleResiduiBersaglio, diamantiVioleResiduiAttaccante);

            int diamantiBluResiduiBersaglio = Math.Max(0, Variabili_Server.Max_Diamanti_Blu_PVP_Giocatore - difensore.Diamanti_Blu_PVP_Persi);
            int diamantiBluResiduiAttaccante = Math.Max(0, Variabili_Server.max_Diamanti_Blu_PVP - attaccante.Diamanti_Blu_PVP_Ottenuti);
            int diamantiBlu = Math.Min(diamantiBluResiduiBersaglio, diamantiBluResiduiAttaccante);

            var raccolte = RaccoliRisorseEquamente(capacitàCarico, cibo, legno, pietra, ferro, oro, 0, diamantiBlu, diamantiViola); // Raccogli risorse

            // Log dettagliato
            SendClient(attackerGuid, "Log_Server|╔══════════════════════════════════════════════════╗");
            SendClient(attackerGuid, "Log_Server|║        [highlight]RISORSE SACCHEGGIATE          ║");
            SendClient(attackerGuid, "Log_Server|╚══════════════════════════════════════════════════╝");

            //Controllo se è possibile rubare diamanti, se i limiti non sono stati superati
            if (attaccante.Diamanti_Viola_PVP_Ottenuti >= Variabili_Server.Max_Diamanti_Viola_PVP ||
                difensore.Diamanti_Viola_PVP_Persi >= Variabili_Server.Max_Diamanti_Viola_PVP_Giocatore)
            {
                raccolte.Diamanti_Viola = 0;
                SendClient(attackerGuid, $"Log_Server|Non è possibile saccheggiare ulteriori diamanti viola.\n" +
                    $"Limite giornaliero: [viola][icon:diamanteViola]{attaccante.Diamanti_Viola_PVP_Ottenuti}/{Variabili_Server.Max_Diamanti_Viola_PVP:N0}" +
                    $"Limite giocatore: [viola][icon:diamanteViola]{difensore.Diamanti_Viola_PVP_Persi}/{Variabili_Server.Max_Diamanti_Viola_PVP_Giocatore:N0}");

            }
            if (attaccante.Diamanti_Blu_PVP_Ottenuti >= Variabili_Server.max_Diamanti_Blu_PVP ||
                difensore.Diamanti_Blu_PVP_Persi >= Variabili_Server.Max_Diamanti_Blu_PVP_Giocatore)
            {
                raccolte.Diamanti_Blu = 0;
                SendClient(attackerGuid, $"Log_Server|Non è possibile saccheggiare ulteriori diamanti blu.\n" +
                    $"Limite giornaliero: [blu][icon:diamanteBlu]{attaccante.Diamanti_Blu_PVP_Ottenuti}/{Variabili_Server.max_Diamanti_Blu_PVP:N0}" +
                    $"Limite giocatore: [blu][icon:diamanteBlu]{difensore.Diamanti_Blu_PVP_Persi}/{Variabili_Server.Max_Diamanti_Blu_PVP_Giocatore:N0}");

            }

            // Assegna all'attaccante
            attaccante.Cibo += raccolte.Cibo;
            attaccante.Legno += raccolte.Legno;
            attaccante.Pietra += raccolte.Pietra;
            attaccante.Ferro += raccolte.Ferro;
            attaccante.Oro += raccolte.Oro;
            attaccante.Diamanti_Blu += raccolte.Diamanti_Blu;
            attaccante.Diamanti_Viola += raccolte.Diamanti_Viola;
            attaccante.Risorse_Razziate += raccolte.Cibo + raccolte.Legno + raccolte.Pietra + raccolte.Ferro + raccolte.Oro + raccolte.Diamanti_Blu + raccolte.Diamanti_Viola;

            // Rimuovi dal difensore
            difensore.Cibo -= raccolte.Cibo;
            difensore.Legno -= raccolte.Legno;
            difensore.Pietra -= raccolte.Pietra;
            difensore.Ferro -= raccolte.Ferro;
            difensore.Oro -= raccolte.Oro;
            difensore.Diamanti_Blu -= raccolte.Diamanti_Blu;
            difensore.Diamanti_Viola -= raccolte.Diamanti_Viola;

            difensore.Diamanti_Blu_PVP_Persi += raccolte.Diamanti_Blu;
            difensore.Diamanti_Viola_PVP_Persi += raccolte.Diamanti_Viola;
            attaccante.Diamanti_Viola_PVP_Ottenuti += raccolte.Diamanti_Viola;
            attaccante.Diamanti_Blu_PVP_Ottenuti += raccolte.Diamanti_Blu;

            // Calcola peso utilizzato
            int pesoUtilizzato =
                raccolte.Cibo * Variabili_Server.peso_Risorse_Cibo +
                raccolte.Legno * Variabili_Server.peso_Risorse_Legno +
                raccolte.Pietra * Variabili_Server.peso_Risorse_Pietra +
                raccolte.Ferro * Variabili_Server.peso_Risorse_Ferro +
                raccolte.Oro * Variabili_Server.peso_Risorse_Oro +
                raccolte.Diamanti_Blu * Variabili_Server.peso_Risorse_Diamante_Blu +
                raccolte.Diamanti_Viola * Variabili_Server.peso_Risorse_Diamante_Viola;

            SendClient(attackerGuid, $"Log_Server|Capacità di carico: [highlight]{capacitàOriginale:N0}");
            SendClient(attackerGuid, $"Log_Server|Capacità utilizzata: [verde]{pesoUtilizzato:N0}\n");

            if (raccolte.Cibo > 0) SendClient(attackerGuid, $"Log_Server|Cibo:   +[cibo][icon:cibo]{raccolte.Cibo:N0}[/cibo]");
            if (raccolte.Legno > 0) SendClient(attackerGuid, $"Log_Server|Legno:  +[legno][icon:legno]{raccolte.Legno:N0}[/legno]");
            if (raccolte.Pietra > 0) SendClient(attackerGuid, $"Log_Server|Pietra: +[pietra][icon:pietra]{raccolte.Pietra:N0}[/pietra]");
            if (raccolte.Ferro > 0) SendClient(attackerGuid, $"Log_Server|Ferro:  +[ferro][icon:ferro]{raccolte.Ferro:N0}[/ferro]");
            if (raccolte.Oro > 0) SendClient(attackerGuid, $"Log_Server|Oro:    +[oro][icon:oro]{raccolte.Oro:N0}[/oro]");
            if (raccolte.Diamanti_Blu > 0) SendClient(attackerGuid, $"Log_Server|Diamanti Blu:    +[blu][icon:diamanteBlu]{raccolte.Diamanti_Blu:N0}[/blu]");
            if (raccolte.Diamanti_Viola > 0) SendClient(attackerGuid, $"Log_Server|Diamanti Viola:    +[viola][icon:diamanteViola]{raccolte.Diamanti_Viola:N0}[/viola]");
            SendClient(attackerGuid, "Log_Server|════════════════════════════════════════════════════\n");

            report.Battaglia.Risorse_Raccolte = raccolte;
            report.Battaglia.Risorse_Raccolte.Capacità_Carico = capacitàOriginale;
            report.Battaglia.Risorse_Raccolte.Capacità_Carico_Usata = pesoUtilizzato;
            return report;
        }
        public static async Task<bool> Battaglia(Giocatori.Player attaccante, Giocatori.Player difensore, UnitGroup attackerUnits)
        {
            var report = new Battaglia.Report();
            RisultatoFase fase = null;

            report.Tipo = "Battaglia";
            report.Data = DateTime.UtcNow.ToString("o"); // formato ISO 8601 ("o"), vedi Spionaggio.cs: senza formato new Date(...) in JS non riesce a interpretare la data
            report.Aperto = false;
            report.Battaglia = new RisultatoBattaglia();
            report.Battaglia.Nome_Attaccante = attaccante.Username;
            report.Battaglia.Nome_Difensore = difensore.Username;
            report.Battaglia.Tipo_Battaglia = "PVP";

            int numeroFasi = 0;
            //1: Fase Battaglia
            for (int struttura = 1; struttura <= 6; struttura++)
            {
                fase = new RisultatoFase();
                numeroFasi = report.Battaglia.Fasi.Count;
                if (numeroFasi > 0) attackerUnits = report.Battaglia.Fasi[numeroFasi - 1].Attaccante.Sopravvisuti;

                fase = await Battaglia_Fase(attaccante, difensore, attackerUnits, fase, struttura);
                //Calcolo esperienza dal report...
                report.Battaglia.Xp_Attaccante += fase.Xp_Attaccante + fase.Fase_Distanza.Attaccante_XP;
                report.Battaglia.Xp_Difensore += fase.Xp_Difensore + fase.Fase_Distanza.Difensore_XP;
                report.Battaglia.Fasi.Add(fase);
            }

            //2: Villaggio Battaglia
            var fase_ultima = new RisultatoFase();
            fase_ultima = await Battaglia_Fase(attaccante, difensore, fase.Attaccante.Sopravvisuti, fase_ultima, 7);
            report.Battaglia.Fasi.Add(fase_ultima);
            report.Battaglia.Vittoria_Attaccante = fase_ultima.Vittoria_Attaccante;

            //Calcolo forza attaccante/difensore...
            report.Battaglia.Forza_Attaccante = CalcolaForza(attackerUnits);
            report.Battaglia.Forza_Attaccante_Finale = CalcolaForza(fase_ultima.Attaccante.Sopravvisuti);
            foreach (var f in report.Battaglia.Fasi)
            {
                report.Battaglia.Forza_Difensore += CalcolaForza(f.Fase_Distanza.Difensore_Schierati);
                report.Battaglia.Forza_Difensore_Finale += CalcolaForza(f.Difensore.Sopravvisuti);
            }

            //3: Risorse Battaglia
            if (report.Battaglia.Vittoria_Attaccante) report = await AssegnaRisorseVittoria_PvP(attaccante, difensore, attaccante.guid_Player, fase.Attaccante.Sopravvisuti, report);

            //Aggiornamento dati attaccante
            numeroFasi = report.Battaglia.Fasi.Count;
            for (int i = 0; i < 5; i++)// aggiorna unità attaccante sottraendo le perdite totali (schierati - sopravvissuti) (Schierati inizio - Sopravvissuti finale)
            {
                attaccante.Guerrieri[i] -= report.Battaglia.Fasi[0].Attaccante.Schierati.Subtract(report.Battaglia.Fasi[numeroFasi - 1].Attaccante.Sopravvisuti).Guerrieri[i];
                attaccante.Lanceri[i] -= report.Battaglia.Fasi[0].Attaccante.Schierati.Subtract(report.Battaglia.Fasi[numeroFasi - 1].Attaccante.Sopravvisuti).Lancieri[i];
                attaccante.Arceri[i] -= report.Battaglia.Fasi[0].Attaccante.Schierati.Subtract(report.Battaglia.Fasi[numeroFasi - 1].Attaccante.Sopravvisuti).Arcieri[i];
                attaccante.Catapulte[i] -= report.Battaglia.Fasi[0].Attaccante.Schierati.Subtract(report.Battaglia.Fasi[numeroFasi - 1].Attaccante.Sopravvisuti).Catapulte[i];
            }
            int frecceAttaccante = report.Battaglia.Fasi.Sum(f => f.Fase_Distanza.Attaccante_Frecce_Usate);
            int frecceDifensore = report.Battaglia.Fasi.Sum(f => f.Fase_Distanza.Difensore_Frecce_Usate);

            //Salvare report dei giocatori interessati
            attaccante.Report.Add(report);
            difensore.Report.Add(report);

            // Invio live del report: fino al 15/09/2026 veniva mandato subito qui a mano
            // (stesso formato usato da Update_Data_OneTime al login), perché prima veniva
            // inviato solo una volta al login e un client aperto durante la battaglia non
            // vedeva mai il nuovo referto senza riconnettersi. Dal 16/09/2026 questo invio
            // esplicito non serve più: ServerConnection.Update_Data (il tick di gioco,
            // circa ogni secondo) rileva da solo il cambio di player.Report.Count e manda
            // il Report_Lista aggiornato — vedi PlayerSnapshot.ReportCountChanged. Così
            // qualunque futura fonte di referti (non solo le battaglie) funziona senza
            // doversene ricordare qui.

            AggiornaDatiGiocatori(attaccante, difensore, report); //Statistiche 

            //Quest
            OnEvent(attaccante, QuestEventType.Battaglie, "Attacco Giocatore", 1); //Quest

            OnEvent(attaccante, QuestEventType.Uccisioni, "Guerrieri", fase.Difensore.Perdite.Guerrieri.Sum());
            OnEvent(attaccante, QuestEventType.Uccisioni, "Lanceri", fase.Difensore.Perdite.Lancieri.Sum());
            OnEvent(attaccante, QuestEventType.Uccisioni, "Arceri", fase.Difensore.Perdite.Arcieri.Sum());
            OnEvent(attaccante, QuestEventType.Uccisioni, "Catapulte", fase.Difensore.Perdite.Catapulte.Sum());
            OnEvent(attaccante, QuestEventType.Risorse, "Frecce", frecceAttaccante);
            OnEvent(difensore, QuestEventType.Uccisioni, "Guerrieri", fase.Attaccante.Perdite.Guerrieri.Sum());
            OnEvent(difensore, QuestEventType.Uccisioni, "Lanceri", fase.Attaccante.Perdite.Lancieri.Sum());
            OnEvent(difensore, QuestEventType.Uccisioni, "Arceri", fase.Attaccante.Perdite.Arcieri.Sum());
            OnEvent(difensore, QuestEventType.Uccisioni, "Catapulte", fase.Attaccante.Perdite.Catapulte.Sum());
            OnEvent(difensore, QuestEventType.Risorse, "Frecce", frecceDifensore);

            return true;
        }
        static async void AggiornaDatiGiocatori(Giocatori.Player attaccante, Giocatori.Player difensore, Battaglia.Report report)
        {
            if (report.Battaglia.Vittoria_Attaccante)
            {
                attaccante.Battaglie_Vinte++; 
                difensore.Battaglie_Perse++;
            }
            else
            {
                attaccante.Battaglie_Perse++;
                difensore.Battaglie_Vinte++;
            }

            foreach (var item in report.Battaglia.Fasi) //Corpo a corpo + Distanza
            {
                attaccante.Guerrieri_Eliminati += item.Difensore.Perdite.Guerrieri.Sum() + item.Fase_Distanza.Difensore_Morti.Guerrieri.Sum();
                attaccante.Lanceri_Eliminati += item.Difensore.Perdite.Lancieri.Sum() + item.Fase_Distanza.Difensore_Morti.Lancieri.Sum();
                attaccante.Arceri_Eliminati += item.Difensore.Perdite.Arcieri.Sum() + item.Fase_Distanza.Difensore_Morti.Arcieri.Sum();
                attaccante.Catapulte_Eliminate += item.Difensore.Perdite.Catapulte.Sum() + item.Fase_Distanza.Difensore_Morti.Catapulte.Sum();
                attaccante.Guerrieri_Persi += item.Attaccante.Perdite.Guerrieri.Sum() + item.Fase_Distanza.Attaccante_Morti.Guerrieri.Sum();
                attaccante.Lanceri_Persi += item.Attaccante.Perdite.Lancieri.Sum() + item.Fase_Distanza.Attaccante_Morti.Lancieri.Sum();
                attaccante.Arceri_Persi += item.Attaccante.Perdite.Arcieri.Sum() + item.Fase_Distanza.Attaccante_Morti.Arcieri.Sum();
                attaccante.Catapulte_Perse += item.Attaccante.Perdite.Catapulte.Sum() + item.Fase_Distanza.Attaccante_Morti.Catapulte.Sum();
                attaccante.Unità_Eliminate += item.Difensore.Perdite.TotalUnits() + item.Fase_Distanza.Difensore_Morti.TotalUnits();
                attaccante.Unità_Perse += item.Attaccante.Perdite.TotalUnits() + item.Fase_Distanza.Attaccante_Morti.TotalUnits();
                attaccante.Frecce_Utilizzate += item.Fase_Distanza.Attaccante_Frecce_Usate;

                difensore.Guerrieri_Eliminati += item.Attaccante.Perdite.Guerrieri.Sum() + item.Fase_Distanza.Attaccante_Morti.Guerrieri.Sum();
                difensore.Lanceri_Eliminati += item.Attaccante.Perdite.Lancieri.Sum() + item.Fase_Distanza.Attaccante_Morti.Lancieri.Sum();
                difensore.Arceri_Eliminati += item.Attaccante.Perdite.Arcieri.Sum() + item.Fase_Distanza.Attaccante_Morti.Arcieri.Sum();
                difensore.Catapulte_Eliminate += item.Attaccante.Perdite.Catapulte.Sum() + item.Fase_Distanza.Attaccante_Morti.Catapulte.Sum();
                difensore.Guerrieri_Persi += item.Difensore.Perdite.Guerrieri.Sum() + item.Fase_Distanza.Difensore_Schierati.Guerrieri.Sum();
                difensore.Lanceri_Persi += item.Difensore.Perdite.Lancieri.Sum() + item.Fase_Distanza.Difensore_Schierati.Lancieri.Sum();
                difensore.Arceri_Persi += item.Difensore.Perdite.Arcieri.Sum() + item.Fase_Distanza.Difensore_Schierati.Arcieri.Sum();
                difensore.Catapulte_Perse += item.Difensore.Perdite.Catapulte.Sum() + item.Fase_Distanza.Difensore_Schierati.Catapulte.Sum();
                difensore.Unità_Eliminate += item.Attaccante.Perdite.TotalUnits() + item.Fase_Distanza.Attaccante_Morti.TotalUnits();
                difensore.Unità_Perse += item.Difensore.Perdite.TotalUnits() + item.Fase_Distanza.Difensore_Morti.TotalUnits();
                difensore.Frecce_Utilizzate += item.Fase_Distanza.Difensore_Frecce_Usate;
            }

            attaccante.Attacchi_Effettuati_PVP++;
            difensore.Attacchi_Subiti_PVP++;
        }
        static async Task<Battaglia.RisultatoFase> Battaglia_Fase(Giocatori.Player attaccante, Giocatori.Player difensore, UnitGroup attackerUnits, Battaglia.RisultatoFase fase, int struttura)
        {
            var defenderUnits = CaricaDatiStruttureDifensore(difensore, struttura);
            fase = new RisultatoFase
            {
                Struttura = struttura switch
                {
                    1 => new Villaggio { Nome = "Ingresso", Guarnigione = defenderUnits.TotalUnits() },
                    2 => new Villaggio { Nome = "Mura", Salute = difensore.Salute_Mura, SaluteMax = difensore.Salute_MuraMax, Difesa = difensore.Difesa_Mura, DifesaMax = difensore.Difesa_MuraMax, SaluteIniziale = difensore.Salute_Mura, DifesaIniziale = difensore.Difesa_Mura, Guarnigione = defenderUnits.TotalUnits() },
                    3 => new Villaggio { Nome = "Cancello", Salute = difensore.Salute_Cancello, SaluteMax = difensore.Salute_CancelloMax, Difesa = difensore.Difesa_Cancello, DifesaMax = difensore.Difesa_CancelloMax, SaluteIniziale = difensore.Salute_Cancello, DifesaIniziale = difensore.Difesa_Cancello, Guarnigione = defenderUnits.TotalUnits() }, // BUGFIX: SaluteMax puntava erroneamente a Difesa_CancelloMax
                    4 => new Villaggio { Nome = "Torri", Salute = difensore.Salute_Torri, SaluteMax = difensore.Salute_TorriMax, Difesa = difensore.Difesa_Torri, DifesaMax = difensore.Difesa_TorriMax, SaluteIniziale = difensore.Salute_Torri, DifesaIniziale = difensore.Difesa_Torri, Guarnigione = defenderUnits.TotalUnits() },
                    5 => new Villaggio { Nome = "Centro Villaggio", Guarnigione = defenderUnits.TotalUnits() },
                    6 => new Villaggio { Nome = "Castello", Salute = difensore.Salute_Castello, SaluteMax = difensore.Salute_CastelloMax, Difesa = difensore.Difesa_Castello, DifesaMax = difensore.Difesa_CastelloMax, SaluteIniziale = difensore.Salute_Castello, DifesaIniziale = difensore.Difesa_Castello, Guarnigione = defenderUnits.TotalUnits() },
                    7 => new Villaggio { Nome = "Villaggio", Guarnigione = defenderUnits.TotalUnits() }
                }
            };

            //Fase Distanza
            var rangedResult = BattagliaDistanza(attackerUnits, defenderUnits, attaccante, difensore);
            fase.Attaccante.Schierati = rangedResult.Attaccante_Sopravvisuti;
            fase.Difensore.Schierati = rangedResult.Difensore_Sopravvisuti;
            fase.Fase_Distanza = rangedResult;

            //Battaglia corpo a corpo
            fase = Battaglia_corpo_a_Corpo(fase.Attaccante.Schierati, fase.Difensore.Schierati, attaccante, difensore, struttura, fase);

            //Vittoria?
            if (fase.Struttura.Salute == 0 && fase.Struttura.Difesa == 0 || defenderUnits.TotalUnits() == 0) fase.Vittoria_Attaccante = true;

            AggiornaDatiStruttureDifensore(struttura, difensore, fase); //Aggiorna dati struttura difensore

            return fase;
        }
        private static BattagliaDistanza BattagliaDistanza(UnitGroup attackerUnits, UnitGroup defenderUnits, Giocatori.Player attaccante, Giocatori.Player difensore)
        {
            var result = new BattagliaDistanza();
            result.Attaccante_Schierati = attackerUnits;
            result.Difensore_Schierati = defenderUnits;

            if (defenderUnits.TotalUnits() == 0) result.Difensore_Unità_Presenti = false;
            else result.Difensore_Unità_Presenti = true;

            // ATTACCO DELL'ATTACCANTE
            result = CalcolaAttaccoDistanza_(attackerUnits, attaccante, result, false);
            result = CalcolaAttaccoDistanza_(defenderUnits, difensore, result, true);

            // BUGFIX (parte 2, trovato durante il porting PVE del 2026-09-14): "Difensore_Danno_*" è il danno INFLITTO
            // dal difensore (con le sue unità a distanza) e deve colpire le unità dell'ATTACCANTE, non le proprie;
            // simmetricamente "Attaccante_Danno_*" deve colpire il DIFENSORE. La versione precedente applicava a
            // ciascun lato il proprio danno inflitto sulle proprie unità (autolesionismo), azzerando di fatto
            // l'effetto reale del tiro con l'arco/catapulte sull'avversario.
            var defenderMorti = ApplicaDanniDistanza_(defenderUnits.Clone(), result.Attaccante_Danno_Guerrieri, result.Attaccante_Danno_Lancieri);
            result.Difensore_Morti.Guerrieri = defenderMorti.Guerrieri;
            result.Difensore_Morti.Lancieri = defenderMorti.Lancieri;

            var attackerMorti = ApplicaDanniDistanza_(attackerUnits.Clone(), result.Difensore_Danno_Guerrieri, result.Difensore_Danno_Lancieri);
            result.Attaccante_Morti.Guerrieri = attackerMorti.Guerrieri;
            result.Attaccante_Morti.Lancieri = attackerMorti.Lancieri;

            for (int i = 0; i < 5; i++)
            {
                result.Attaccante_Sopravvisuti.Guerrieri[i] = result.Attaccante_Schierati.Guerrieri[i] - result.Attaccante_Morti.Guerrieri[i];
                result.Attaccante_Sopravvisuti.Lancieri[i] = result.Attaccante_Schierati.Lancieri[i] - result.Attaccante_Morti.Lancieri[i];
                result.Attaccante_Sopravvisuti.Arcieri[i] = result.Attaccante_Schierati.Arcieri[i] - result.Attaccante_Morti.Arcieri[i];
                result.Attaccante_Sopravvisuti.Catapulte[i] = result.Attaccante_Schierati.Catapulte[i] - result.Attaccante_Morti.Catapulte[i];

                result.Difensore_Sopravvisuti.Guerrieri[i] = result.Difensore_Schierati.Guerrieri[i] - result.Difensore_Morti.Guerrieri[i];
                result.Difensore_Sopravvisuti.Lancieri[i] = result.Difensore_Schierati.Lancieri[i] - result.Difensore_Morti.Lancieri[i];
                result.Difensore_Sopravvisuti.Arcieri[i] = result.Difensore_Schierati.Arcieri[i] - result.Difensore_Morti.Arcieri[i];
                result.Difensore_Sopravvisuti.Catapulte[i] = result.Difensore_Schierati.Catapulte[i] - result.Difensore_Morti.Catapulte[i];
            }
            result.Attaccante_Sopravvisuti.Arcieri = result.Attaccante_Schierati.Arcieri;
            result.Attaccante_Sopravvisuti.Catapulte = result.Attaccante_Schierati.Catapulte;

            result.Attaccante_XP = CalcolaEsperienzaPVP(result.Difensore_Morti);
            result.Difensore_XP = CalcolaEsperienzaPVP(result.Attaccante_Morti);

            return result;
        }
        private static RisultatoFase Battaglia_corpo_a_Corpo(UnitGroup attackerUnits, UnitGroup defenderUnits, Giocatori.Player attaccante, Giocatori.Player difensore, int struttura,
        RisultatoFase fase)
        {
            if (defenderUnits.TotalUnits() == 0) fase.Unità_Presenti_Difensore = false;
            else fase.Unità_Presenti_Difensore = true;

            int truppeAttaccante = attackerUnits.TotalUnits(); //Numero truppe
            int truppeDifensore = defenderUnits.TotalUnits();
            bool Frecce = false;

            // BUGFIX: Ingresso (1) e Centro Villaggio (5) non hanno Salute/Difesa proprie, quindi fase.Struttura.Salute
            // resta sempre a 0 per costruzione. La condizione "Salute > 5" li escludeva sempre dal consumo di frecce,
            // permettendo danno a distanza gratuito (senza frecce) solo su quei due strati. Ora il requisito di
            // "struttura non ancora crollata" si applica solo agli strati che hanno davvero Salute/Difesa.
            bool struttura_Con_Salute = struttura == 2 || struttura == 3 || struttura == 4 || struttura == 6;
            if (truppeDifensore > 0 && (!struttura_Con_Salute || fase.Struttura.Salute > 5)) Frecce = true;
            double dannoAttaccante = CalcolaDannoGiocatore(attackerUnits, attaccante, Frecce, true, fase); // Calcola danno
            Frecce = false; //reset, per riutilizzo

            if (truppeAttaccante > 0 && struttura != 1 && struttura != 5) Frecce = true;
            else Frecce = false;
            double dannoDifensore = CalcolaDannoGiocatore(defenderUnits, difensore, Frecce, false, fase); // Calcola danno

            //Circa il 60% del danno dell'attaccante viene assorbito dalla struttura (difesa e salute), il resto va alle unità difensive.
            double dannotempDifesa = dannoAttaccante * 0.30; //Se struttura == 0, non serve a nulla questa variabile.  
            double dannotempSalute = 0; //Se struttura == 0, non serve a nulla questa variabile.
            double bonusUnità = 1;
            if (struttura != 0 && struttura != 5 && fase.Struttura.Salute > 5)
            {
                if (dannotempDifesa >= fase.Struttura.Difesa)
                {
                    dannotempDifesa -= fase.Struttura.Difesa;
                    dannoAttaccante -= fase.Struttura.Difesa;
                    fase.Struttura.Difesa = 0;
                }
                else
                {
                    fase.Struttura.Difesa -= (int)dannotempDifesa;
                    dannoAttaccante -= dannotempDifesa;
                }
                dannotempSalute = (dannoAttaccante - dannotempDifesa) * 0.20;
                if (dannotempSalute >= fase.Struttura.Salute)
                {
                    dannotempSalute -= fase.Struttura.Salute;
                    dannoAttaccante -= fase.Struttura.Salute;
                    fase.Struttura.Salute = 0;
                }
                else
                {
                    fase.Struttura.Salute -= (int)dannotempSalute;
                    dannoAttaccante -= dannotempSalute;
                }
            }
            if (struttura == 1) //Bonus guarnigione ingresso in base alle truppe presenti in Cancello e Mura
            {
                if (difensore.Guarnigione_Cancello >= difensore.Guarnigione_CancelloMax * 0.40 || difensore.Guarnigione_Mura >= difensore.Guarnigione_MuraMax * 0.40) bonusUnità += 0.10; //10% con almeno il 40% della guarnigione occupata in Cancello o Mura
                if (difensore.Guarnigione_Cancello >= difensore.Guarnigione_CancelloMax * 0.80 && difensore.Guarnigione_Mura > difensore.Guarnigione_MuraMax * 0.80) bonusUnità += 0.15; //15% con almeno l'80% della guarnigione occupata in Cancello e Mura
                if (difensore.Guarnigione_Cancello == difensore.Guarnigione_CancelloMax && difensore.Guarnigione_Mura == difensore.Guarnigione_MuraMax) bonusUnità += 0.20; //20% con guarnigione piena in Cancello e Mura
            }

            double dannoPerTipoAttacker = dannoDifensore / attackerUnits.CountUnitTypes();
            double dannoPerTipoDefender = dannoAttaccante / defenderUnits.CountUnitTypes();
            fase = ApplicaDanniGiocatore(fase, attackerUnits.Clone(), attaccante, dannoPerTipoAttacker, 1, true); // Applica danni all'attaccante
            fase = ApplicaDanniGiocatore(fase, defenderUnits.Clone(), difensore, dannoPerTipoDefender, bonusUnità, false); //Difensore con bonus guarnigione

            // Calcola esperienza corpo a corpo per entrambi
            fase.Xp_Attaccante = CalcolaEsperienzaPVP(fase.Difensore.Perdite); // Per il log
            fase.Xp_Difensore = CalcolaEsperienzaPVP(fase.Attaccante.Perdite);

            fase.Vittoria_Attaccante = fase.Difensore.Sopravvisuti.TotalUnits() == 0; // Determina vittoria provvisoria (sarà confermata in base alle perdite totali)
            return fase;
        }

        public static async void TestBattaglia()
        {
            //Aggiungere giocatore x test
            bool test2 = await Server.ServerConnection.New_Player("TEST", "123", "test@example.com", Guid.Empty);
            var attaccante = Server.Server.servers_.GetPlayer("TEST");

            bool test1 = await Server.ServerConnection.New_Player("adlos", "123", "adly@example.com", Guid.Empty);
            var difensore = Server.Server.servers_.GetPlayer("adlos");

            int[] guerrieri = new int[] { 60, 0, 0, 0, 0 };
            int[] picchieri = new int[] { 50, 0, 0, 0, 0 };
            int[] arcieri = new int[] { 15, 0, 0, 0, 0 };
            int[] catapulte = new int[] { 10, 0, 0, 0, 0 };

            var attackerUnits = new UnitGroup
            {
                Guerrieri = guerrieri,
                Lancieri = picchieri,
                Arcieri = arcieri,
                Catapulte = catapulte
            };
            AddTroops(difensore);
            AddTroops(attaccante);
            await Battaglia(attaccante, difensore, attackerUnits);
        }
        public static void AddTroops(Giocatori.Player difensore)
        {
            int[] guerrieri = new int[] { 60, 0, 0, 0, 0 };
            int[] picchieri = new int[] { 50, 0, 0, 0, 0 };
            int[] arcieri = new int[] { 15, 0, 0, 0, 0 };
            int[] catapulte = new int[] { 10, 0, 0, 0, 0 };

            var unitàStrutture = new UnitGroup
            {
                Guerrieri = new int[] { 5, 0, 0, 0, 0 },
                Lancieri = new int[] { 5, 0, 0, 0, 0 },
                Arcieri = new int[] { 5, 0, 0, 0, 0 },
                Catapulte = new int[] { 0, 0, 0, 0, 0 }
            };
            var unitàDifensore = new UnitGroup
            {
                Guerrieri = new int[] { 25, 0, 0, 0, 0 },
                Lancieri = new int[] { 20, 0, 0, 0, 0 },
                Arcieri = new int[] { 15, 0, 0, 0, 0 },
                Catapulte = new int[] { 10, 0, 0, 0, 0 }
            };

            difensore.Guerrieri_Ingresso = unitàStrutture.Guerrieri;
            difensore.Lanceri_Ingresso = unitàStrutture.Lancieri;
            difensore.Arceri_Ingresso = unitàStrutture.Arcieri;

            difensore.Guerrieri_Mura = unitàStrutture.Guerrieri;
            difensore.Lanceri_Mura = unitàStrutture.Lancieri;
            difensore.Arceri_Mura = unitàStrutture.Arcieri;

            difensore.Guerrieri_Cancello = unitàStrutture.Guerrieri;
            difensore.Lanceri_Cancello = unitàStrutture.Lancieri;
            difensore.Arceri_Cancello = unitàStrutture.Arcieri;

            difensore.Guerrieri_Torri = unitàStrutture.Guerrieri;
            difensore.Lanceri_Torri = unitàStrutture.Lancieri;
            difensore.Arceri_Torri = unitàStrutture.Arcieri;

            difensore.Guerrieri_Citta = unitàStrutture.Guerrieri;
            difensore.Lanceri_Citta = unitàStrutture.Lancieri;
            difensore.Arceri_Citta = unitàStrutture.Arcieri;

            difensore.Guerrieri_Castello = unitàStrutture.Guerrieri;
            difensore.Lanceri_Castello = unitàStrutture.Lancieri;
            difensore.Arceri_Castello = unitàStrutture.Arcieri;

            difensore.Guerrieri = unitàDifensore.Guerrieri;
            difensore.Lanceri = unitàDifensore.Lancieri;
            difensore.Arceri = unitàDifensore.Arcieri;
            difensore.Catapulte = unitàDifensore.Catapulte;

            difensore.Salute_Mura = difensore.Salute_MuraMax;
            difensore.Difesa_Mura = difensore.Difesa_MuraMax;

            difensore.Salute_Cancello = difensore.Salute_CancelloMax;
            difensore.Difesa_Cancello = difensore.Difesa_CancelloMax;

            difensore.Salute_Torri = difensore.Salute_TorriMax;
            difensore.Difesa_Torri = difensore.Difesa_TorriMax;

            difensore.Salute_Castello = difensore.Salute_CastelloMax;
            difensore.Difesa_Castello = difensore.Difesa_CastelloMax;

            difensore.Frecce = 5000;
            
            var DefenderUnits = new UnitGroup()
            {
                Guerrieri = guerrieri,
                Lancieri = picchieri,
                Arcieri = arcieri,
                Catapulte = catapulte
            };
        }
        public static void AddTroops(Giocatori.Player difensore, int ricerca)
        {
            difensore.Livello = 10;

            difensore.Ricerca_Cancello_Salute = ricerca;
            difensore.Ricerca_Cancello_Difesa = ricerca;

            difensore.Ricerca_Mura_Salute = ricerca;
            difensore.Ricerca_Mura_Difesa = ricerca;

            difensore.Ricerca_Torri_Salute = ricerca;
            difensore.Ricerca_Torri_Difesa = ricerca;

            difensore.Ricerca_Castello_Salute = ricerca;
            difensore.Ricerca_Castello_Difesa = ricerca;

            difensore.Salute_Mura = difensore.Salute_MuraMax;
            difensore.Difesa_Mura = difensore.Difesa_MuraMax;

            difensore.Salute_Cancello = difensore.Salute_CancelloMax;
            difensore.Difesa_Cancello = difensore.Difesa_CancelloMax;

            difensore.Salute_Torri = difensore.Salute_TorriMax;
            difensore.Difesa_Torri = difensore.Difesa_TorriMax;

            difensore.Salute_Castello = difensore.Salute_CastelloMax;
            difensore.Difesa_Castello = difensore.Difesa_CastelloMax;

        }
    }
}
