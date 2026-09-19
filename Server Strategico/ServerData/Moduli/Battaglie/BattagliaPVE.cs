using Server_Strategico.Gioco;
using static Server_Strategico.Manager.QuestManager;
using static Server_Strategico.ServerData.Moduli.Battaglie.Battaglia;

namespace Server_Strategico.ServerData.Moduli.Battaglie
{
    // Modulo PVE (Villaggi Barbari / Città Barbare), riscritto il 2026-09-14 con la stessa struttura di BattagliaPVP.cs
    // (stessi tipi condivisi da Battaglia.cs: UnitGroup, Report, RisultatoBattaglia, RisultatoFase, BattagliaDistanza,
    // RisorseRaccolte) in modo che i client possano gestire i report PVE esattamente come quelli PVP.
    //
    // Differenze di meccanica rispetto al PVP (vedi Wiki/Game/Battaglie/PVE.md e Difesa.md):
    //  - Non esistono strati difensivi multipli (Ingresso/Mura/Cancello/Torri/Castello): un Villaggio o una Città
    //    Barbara è una singola guarnigione con una sola Salute/Difesa propria (Barbari.cs), usata nella fase corpo a
    //    corpo esattamente come lo strato "Ingresso"/"Centro Villaggio" del PVP (30% danno assorbito da Difesa, poi
    //    20% del resto da Salute — vedi Battaglia_Fase, 19/09/2026). Quindi un'unica RisultatoFase per battaglia.
    //  - Il difensore non è un giocatore: usa le statistiche di Esercito.EsercitoNemico (GetEnemyUnitStats) e non ha
    //    scorte di frecce da gestire (semplificazione: i barbari hanno sempre "frecce infinite").
    //  - Non c'è saccheggio del 50%: in caso di vittoria si raccoglie il bottino totale del Villaggio/Città (nei
    //    limiti della capacità di trasporto), che poi si rigenera con Barbari.RigeneraBarbari().
    //  - Il "livello" del bersaglio seleziona sia il tier delle statistiche nemiche (GetTierIndex, 5 fasce su 20
    //    livelli) sia, per le Città Barbare, quale Città Barbara globale (Barbari.CittaGlobali) viene attaccata.
    public class BattagliaPVE
    {
        // SendClient, GetPlayerUnitStats, GetUnitStats, CalcoloFrecce, CapacitàCarico, RidurreNumeroSoldati,
        // ApplicaDanni, ApplicaDanniDistanza_, CalcolaForza e RaccoliRisorseEquamente sono stati spostati in
        // Battaglia.cs il 2026-09-14 (erano duplicati letteralmente identici tra BattagliaPVP.cs e BattagliaPVE.cs,
        // e servivano comunque anche a Raduni.cs). Restano richiamabili qui senza prefisso grazie a
        // "using static ...Battaglia;" in cima al file.

        // ═══════════════════════════════════════════════════════════════
        // STATISTICHE UNITÀ
        // ═══════════════════════════════════════════════════════════════

        // Statistiche della guarnigione barbara (Esercito.EsercitoNemico). Attenzione alla dicitura "Lanceri" (non
        // "Lancieri") usata in Esercito.EsercitoNemico — refuso storico nel codice originale, non lo correggo qui
        // per non rompere i nomi dei campi statici già in uso altrove.
        internal static (double GuerrieriAttacco, double GuerrieriDifesa, double GuerrieriSalute, int GuerrieriEsperienza,
                   double LancieriAttacco, double LancieriDifesa, double LancieriSalute, int LancieriEsperienza,
                   double ArcieriAttacco, double ArcieriDifesa, double ArcieriSalute, int ArcieriEsperienza,
                   double CatapulteAttacco, double CatapulteDifesa, double CatapulteSalute, int CatapulteEsperienza)
        GetEnemyUnitStats(int level)
        {
            return level switch
            {
                0 => (Esercito.EsercitoNemico.Guerrieri_1.Attacco, Esercito.EsercitoNemico.Guerrieri_1.Difesa,
                      Esercito.EsercitoNemico.Guerrieri_1.Salute, Esercito.EsercitoNemico.Guerrieri_1.Esperienza,
                      Esercito.EsercitoNemico.Lanceri_1.Attacco, Esercito.EsercitoNemico.Lanceri_1.Difesa,
                      Esercito.EsercitoNemico.Lanceri_1.Salute, Esercito.EsercitoNemico.Lanceri_1.Esperienza,
                      Esercito.EsercitoNemico.Arceri_1.Attacco, Esercito.EsercitoNemico.Arceri_1.Difesa,
                      Esercito.EsercitoNemico.Arceri_1.Salute, Esercito.EsercitoNemico.Arceri_1.Esperienza,
                      Esercito.EsercitoNemico.Catapulte_1.Attacco, Esercito.EsercitoNemico.Catapulte_1.Difesa,
                      Esercito.EsercitoNemico.Catapulte_1.Salute, Esercito.EsercitoNemico.Catapulte_1.Esperienza),

                1 => (Esercito.EsercitoNemico.Guerrieri_2.Attacco, Esercito.EsercitoNemico.Guerrieri_2.Difesa,
                      Esercito.EsercitoNemico.Guerrieri_2.Salute, Esercito.EsercitoNemico.Guerrieri_2.Esperienza,
                      Esercito.EsercitoNemico.Lanceri_2.Attacco, Esercito.EsercitoNemico.Lanceri_2.Difesa,
                      Esercito.EsercitoNemico.Lanceri_2.Salute, Esercito.EsercitoNemico.Lanceri_2.Esperienza,
                      Esercito.EsercitoNemico.Arceri_2.Attacco, Esercito.EsercitoNemico.Arceri_2.Difesa,
                      Esercito.EsercitoNemico.Arceri_2.Salute, Esercito.EsercitoNemico.Arceri_2.Esperienza,
                      Esercito.EsercitoNemico.Catapulte_2.Attacco, Esercito.EsercitoNemico.Catapulte_2.Difesa,
                      Esercito.EsercitoNemico.Catapulte_2.Salute, Esercito.EsercitoNemico.Catapulte_2.Esperienza),

                2 => (Esercito.EsercitoNemico.Guerrieri_3.Attacco, Esercito.EsercitoNemico.Guerrieri_3.Difesa,
                      Esercito.EsercitoNemico.Guerrieri_3.Salute, Esercito.EsercitoNemico.Guerrieri_3.Esperienza,
                      Esercito.EsercitoNemico.Lanceri_3.Attacco, Esercito.EsercitoNemico.Lanceri_3.Difesa,
                      Esercito.EsercitoNemico.Lanceri_3.Salute, Esercito.EsercitoNemico.Lanceri_3.Esperienza,
                      Esercito.EsercitoNemico.Arceri_3.Attacco, Esercito.EsercitoNemico.Arceri_3.Difesa,
                      Esercito.EsercitoNemico.Arceri_3.Salute, Esercito.EsercitoNemico.Arceri_3.Esperienza,
                      Esercito.EsercitoNemico.Catapulte_3.Attacco, Esercito.EsercitoNemico.Catapulte_3.Difesa,
                      Esercito.EsercitoNemico.Catapulte_3.Salute, Esercito.EsercitoNemico.Catapulte_3.Esperienza),

                3 => (Esercito.EsercitoNemico.Guerrieri_4.Attacco, Esercito.EsercitoNemico.Guerrieri_4.Difesa,
                      Esercito.EsercitoNemico.Guerrieri_4.Salute, Esercito.EsercitoNemico.Guerrieri_4.Esperienza,
                      Esercito.EsercitoNemico.Lanceri_4.Attacco, Esercito.EsercitoNemico.Lanceri_4.Difesa,
                      Esercito.EsercitoNemico.Lanceri_4.Salute, Esercito.EsercitoNemico.Lanceri_4.Esperienza,
                      Esercito.EsercitoNemico.Arceri_4.Attacco, Esercito.EsercitoNemico.Arceri_4.Difesa,
                      Esercito.EsercitoNemico.Arceri_4.Salute, Esercito.EsercitoNemico.Arceri_4.Esperienza,
                      Esercito.EsercitoNemico.Catapulte_4.Attacco, Esercito.EsercitoNemico.Catapulte_4.Difesa,
                      Esercito.EsercitoNemico.Catapulte_4.Salute, Esercito.EsercitoNemico.Catapulte_4.Esperienza),

                4 => (Esercito.EsercitoNemico.Guerrieri_5.Attacco, Esercito.EsercitoNemico.Guerrieri_5.Difesa,
                      Esercito.EsercitoNemico.Guerrieri_5.Salute, Esercito.EsercitoNemico.Guerrieri_5.Esperienza,
                      Esercito.EsercitoNemico.Lanceri_5.Attacco, Esercito.EsercitoNemico.Lanceri_5.Difesa,
                      Esercito.EsercitoNemico.Lanceri_5.Salute, Esercito.EsercitoNemico.Lanceri_5.Esperienza,
                      Esercito.EsercitoNemico.Arceri_5.Attacco, Esercito.EsercitoNemico.Arceri_5.Difesa,
                      Esercito.EsercitoNemico.Arceri_5.Salute, Esercito.EsercitoNemico.Arceri_5.Esperienza,
                      Esercito.EsercitoNemico.Catapulte_5.Attacco, Esercito.EsercitoNemico.Catapulte_5.Difesa,
                      Esercito.EsercitoNemico.Catapulte_5.Salute, Esercito.EsercitoNemico.Catapulte_5.Esperienza),

                _ => (0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
            };
        }

        // ═══════════════════════════════════════════════════════════════
        // BERSAGLIO: caricamento/aggiornamento Villaggio o Città Barbara
        // ═══════════════════════════════════════════════════════════════

        internal static int GetTierIndex(int livello)
        {
            if (livello >= 1 && livello <= 4) return 0;
            if (livello <= 8) return 1;
            if (livello <= 12) return 2;
            if (livello <= 16) return 3;
            if (livello <= 20) return 4;
            return 0;
        }

        internal static UnitGroup CaricaUnitaNemiche(int livello, string tipo, Giocatori.Player player)
        {
            var units = new UnitGroup();
            int tierIndex = GetTierIndex(livello);
            if (tierIndex < 0 || tierIndex >= 5) return units;

            if (tipo == "Città Barbaro")
            {
                // BUGFIX: il vecchio codice cercava sempre "c.Livello == 1" indipendentemente dal livello scelto,
                // quindi qualunque attacco a una Città Barbara leggeva/aggiornava sempre la città di livello 1.
                var citta = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == livello);
                if (citta == null) return units;

                units.Guerrieri[tierIndex] = citta.Guerrieri;
                units.Lancieri[tierIndex] = citta.Lancieri;
                units.Arcieri[tierIndex] = citta.Arcieri;
                units.Catapulte[tierIndex] = citta.Catapulte;
            }
            else if (tipo == "Villaggio Barbaro")
            {
                if (player.VillaggiPersonali == null || livello - 1 < 0 || livello - 1 >= player.VillaggiPersonali.Count) return units;
                var villaggio = player.VillaggiPersonali[livello - 1];
                if (villaggio == null) return units;

                units.Guerrieri[tierIndex] = villaggio.Guerrieri;
                units.Lancieri[tierIndex] = villaggio.Lancieri;
                units.Arcieri[tierIndex] = villaggio.Arcieri;
                units.Catapulte[tierIndex] = villaggio.Catapulte;
            }
            return units;
        }

        // 19/09/2026, su richiesta dell'utente: stessa identica ricerca di CaricaUnitaNemiche qui sopra, ma restituisce
        // l'oggetto Villaggio/Città Barbara stesso invece delle sole truppe — serve a Battaglia_Fase per leggere e
        // ridurre Salute/Difesa nella fase corpo a corpo (vedi sotto).
        internal static Barbari.BarbarianBase CaricaBarbaro(int livello, string tipo, Giocatori.Player player)
        {
            if (tipo == "Città Barbaro")
                return Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == livello);

            if (tipo == "Villaggio Barbaro")
            {
                if (player.VillaggiPersonali == null || livello - 1 < 0 || livello - 1 >= player.VillaggiPersonali.Count) return null;
                return player.VillaggiPersonali[livello - 1];
            }
            return null;
        }

        // Riporta sul Villaggio/Città Barbara i sopravvissuti della guarnigione dopo la battaglia.
        internal static void AggiornaBarbari(int livello, string tipo, Giocatori.Player player, UnitGroup survivors)
        {
            int tierIndex = GetTierIndex(livello);

            if (tipo == "Città Barbaro")
            {
                // BUGFIX: stesso "Livello == 1" hardcoded del caricamento, corretto in "Livello == livello".
                var citta = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == livello);
                if (citta == null) return;

                int truppe = citta.Guerrieri + citta.Lancieri + citta.Arcieri + citta.Catapulte;
                if (truppe == 0) citta.Sconfitto = true;

                citta.Guerrieri = survivors.Guerrieri[tierIndex];
                citta.Lancieri = survivors.Lancieri[tierIndex];
                citta.Arcieri = survivors.Arcieri[tierIndex];
                citta.Catapulte = survivors.Catapulte[tierIndex];
            }
            else if (tipo == "Villaggio Barbaro")
            {
                if (player.VillaggiPersonali == null || livello - 1 < 0 || livello - 1 >= player.VillaggiPersonali.Count) return;
                var villaggio = player.VillaggiPersonali[livello - 1];
                if (villaggio == null) return;

                int truppe = villaggio.Guerrieri + villaggio.Lancieri + villaggio.Arcieri + villaggio.Catapulte;
                if (truppe == 0) villaggio.Sconfitto = true;

                villaggio.Guerrieri = survivors.Guerrieri[tierIndex];
                villaggio.Lancieri = survivors.Lancieri[tierIndex];
                villaggio.Arcieri = survivors.Arcieri[tierIndex];
                villaggio.Catapulte = survivors.Catapulte[tierIndex];
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // FRECCE / CAPACITÀ DI TRASPORTO
        // ═══════════════════════════════════════════════════════════════

        // ═══════════════════════════════════════════════════════════════
        // FASE A DISTANZA
        // ═══════════════════════════════════════════════════════════════

        // Attacco a distanza del GIOCATORE: consuma frecce reali, esattamente come in BattagliaPVP.CalcolaAttaccoDistanza_.
        internal static BattagliaDistanza CalcolaAttaccoDistanzaGiocatore(UnitGroup units, Giocatori.Player player, BattagliaDistanza result)
        {
            int totaleArcieri = units.Arcieri.Sum();
            int totaleCatapulte = units.Catapulte.Sum();
            if (totaleArcieri == 0 && totaleCatapulte == 0) return result;

            int frecceNecessarie = CalcoloFrecce(units);
            bool frecceInsufficienti = false;

            int arcieriEffettivi = totaleArcieri * 3 / 7;
            int catapulteEffettive = totaleCatapulte * 3 / 6;
            if (totaleArcieri > 0 && totaleArcieri <= 15) arcieriEffettivi = totaleArcieri * 3 / 6;
            if (totaleCatapulte > 0 && totaleCatapulte <= 10) catapulteEffettive = totaleCatapulte * 3 / 5;

            if (player.Frecce < frecceNecessarie)
            {
                frecceInsufficienti = true;
                arcieriEffettivi /= 3;
                catapulteEffettive /= 4;
            }

            int danno = Math.Max(0, arcieriEffettivi + catapulteEffettive);
            if (danno > 0)
            {
                result.Attaccante_Danno_Guerrieri = danno * 3 / 5;
                result.Attaccante_Danno_Lancieri = danno * 2 / 5;
            }

            int frecceUsate = frecceInsufficienti ? (int)player.Frecce : frecceNecessarie;
            player.Frecce_Utilizzate += frecceUsate;
            player.Frecce -= frecceUsate;

            result.Attaccante_Frecce_Necessarie = frecceNecessarie;
            result.Attaccante_Frecce_Usate = frecceUsate;
            result.Attaccante_Poche_Frecce = frecceInsufficienti;
            return result;
        }

        // Contrattacco a distanza della guarnigione BARBARA: frecce infinite per semplificazione (i barbari non hanno
        // una scorta da gestire, scelta di design mantenuta identica al vecchio codice, non è un bug).
        internal static BattagliaDistanza CalcolaAttaccoDistanzaBarbari(UnitGroup enemyUnits, BattagliaDistanza result)
        {
            int totaleArcieri = enemyUnits.Arcieri.Sum();
            int totaleCatapulte = enemyUnits.Catapulte.Sum();
            if (totaleArcieri == 0 && totaleCatapulte == 0) return result;

            int arcieriEffettivi = totaleArcieri * 3 / 6;
            int catapulteEffettive = totaleCatapulte * 3 / 5;
            if (totaleArcieri > 0 && totaleArcieri <= 10) arcieriEffettivi = totaleArcieri * 3 / 4;
            if (totaleCatapulte > 0 && totaleCatapulte <= 5) catapulteEffettive = totaleCatapulte * 3 / 4;

            int danno = (arcieriEffettivi + catapulteEffettive) * 4 / 5;
            if (danno > 0)
            {
                result.Difensore_Danno_Guerrieri = danno * 2 / 5;
                result.Difensore_Danno_Lancieri = danno * 2 / 5;
            }

            result.Difensore_Frecce_Necessarie = CalcoloFrecce(enemyUnits);
            result.Difensore_Frecce_Usate = result.Difensore_Frecce_Necessarie; // solo per il report: i barbari non esauriscono mai le frecce
            result.Difensore_Poche_Frecce = false;
            return result;
        }

        private static BattagliaDistanza BattagliaDistanzaPVE(UnitGroup attackerUnits, UnitGroup enemyUnits, Giocatori.Player player)
        {
            var result = new BattagliaDistanza();
            result.Attaccante_Schierati = attackerUnits;
            result.Difensore_Schierati = enemyUnits;
            result.Difensore_Unità_Presenti = enemyUnits.TotalUnits() > 0;

            result = CalcolaAttaccoDistanzaGiocatore(attackerUnits, player, result);
            result = CalcolaAttaccoDistanzaBarbari(enemyUnits, result);

            // Il danno inflitto dall'attaccante colpisce il difensore e viceversa (vedi bugfix del 2026-09-14 in BattagliaPVP.cs).
            var defenderMorti = ApplicaDanniDistanza_(enemyUnits.Clone(), result.Attaccante_Danno_Guerrieri, result.Attaccante_Danno_Lancieri);
            result.Difensore_Morti.Guerrieri = defenderMorti.Guerrieri;
            result.Difensore_Morti.Lancieri = defenderMorti.Lancieri;

            var attackerMorti = ApplicaDanniDistanza_(attackerUnits.Clone(), result.Difensore_Danno_Guerrieri, result.Difensore_Danno_Lancieri);
            result.Attaccante_Morti.Guerrieri = attackerMorti.Guerrieri;
            result.Attaccante_Morti.Lancieri = attackerMorti.Lancieri;

            for (int i = 0; i < 5; i++)
            {
                result.Attaccante_Sopravvisuti.Guerrieri[i] = result.Attaccante_Schierati.Guerrieri[i] - result.Attaccante_Morti.Guerrieri[i];
                result.Attaccante_Sopravvisuti.Lancieri[i] = result.Attaccante_Schierati.Lancieri[i] - result.Attaccante_Morti.Lancieri[i];
                result.Attaccante_Sopravvisuti.Arcieri[i] = result.Attaccante_Schierati.Arcieri[i];
                result.Attaccante_Sopravvisuti.Catapulte[i] = result.Attaccante_Schierati.Catapulte[i];

                result.Difensore_Sopravvisuti.Guerrieri[i] = result.Difensore_Schierati.Guerrieri[i] - result.Difensore_Morti.Guerrieri[i];
                result.Difensore_Sopravvisuti.Lancieri[i] = result.Difensore_Schierati.Lancieri[i] - result.Difensore_Morti.Lancieri[i];
                result.Difensore_Sopravvisuti.Arcieri[i] = result.Difensore_Schierati.Arcieri[i];
                result.Difensore_Sopravvisuti.Catapulte[i] = result.Difensore_Schierati.Catapulte[i];
            }

            result.Attaccante_XP = CalcolaEsperienzaPVE(result.Difensore_Morti); // XP solo per le perdite inflitte ai barbari
            return result;
        }

        // ═══════════════════════════════════════════════════════════════
        // FASE CORPO A CORPO
        // ═══════════════════════════════════════════════════════════════

        internal static double CalcolaDannoGiocatore(UnitGroup units, Giocatori.Player player, bool usaFrecce, RisultatoFase fase)
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

                fase.Fase_Distanza.Attaccante_Frecce_Usate += frecceUsate;
                fase.Fase_Distanza.Attaccante_Poche_Frecce = !frecceSufficienti;
                if (!frecceSufficienti)
                    fase.Fase_Distanza.Attaccante_Frecce_Necessarie = frecceNecessarie;
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
        internal static double CalcolaDannoBarbari(UnitGroup units)
        {
            double dannoTotale = 0;
            for (int i = 0; i < 5; i++)
            {
                var stats = GetEnemyUnitStats(i);
                dannoTotale += units.Guerrieri[i] * stats.GuerrieriAttacco;
                dannoTotale += units.Lancieri[i] * stats.LancieriAttacco;
                dannoTotale += units.Arcieri[i] * stats.ArcieriAttacco;
                dannoTotale += units.Catapulte[i] * stats.CatapulteAttacco;
            }
            return dannoTotale;
        }
        internal static void ApplicaDanniAttaccante(RisultatoFase fase, UnitGroup units, Giocatori.Player player, double dannoPerTipo)
        {
            for (int i = 0; i < 5; i++)
            {
                var stats = GetPlayerUnitStats(i, player);

                int guerrieriIniziali = units.Guerrieri[i];
                units.Guerrieri[i] = RidurreNumeroSoldati(guerrieriIniziali, dannoPerTipo, stats.GuerrieriDifesa * guerrieriIniziali, stats.GuerrieriSalute);
                fase.Attaccante.Sopravvisuti.Guerrieri[i] = units.Guerrieri[i];
                fase.Attaccante.Perdite.Guerrieri[i] += guerrieriIniziali - units.Guerrieri[i];

                int lancieriIniziali = units.Lancieri[i];
                units.Lancieri[i] = RidurreNumeroSoldati(lancieriIniziali, dannoPerTipo, stats.LancieriDifesa * lancieriIniziali, stats.LancieriSalute);
                fase.Attaccante.Sopravvisuti.Lancieri[i] = units.Lancieri[i];
                fase.Attaccante.Perdite.Lancieri[i] += lancieriIniziali - units.Lancieri[i];

                int arcieriIniziali = units.Arcieri[i];
                units.Arcieri[i] = RidurreNumeroSoldati(arcieriIniziali, dannoPerTipo, stats.ArcieriDifesa * arcieriIniziali, stats.ArcieriSalute);
                fase.Attaccante.Sopravvisuti.Arcieri[i] = units.Arcieri[i];
                fase.Attaccante.Perdite.Arcieri[i] += arcieriIniziali - units.Arcieri[i];

                int catapulteIniziali = units.Catapulte[i];
                units.Catapulte[i] = RidurreNumeroSoldati(catapulteIniziali, dannoPerTipo, stats.CatapulteDifesa * catapulteIniziali, stats.CatapulteSalute);
                fase.Attaccante.Sopravvisuti.Catapulte[i] = units.Catapulte[i];
                fase.Attaccante.Perdite.Catapulte[i] += catapulteIniziali - units.Catapulte[i];
            }
        }
        internal static void ApplicaDanniBarbari(RisultatoFase fase, UnitGroup units, double dannoPerTipo)
        {
            for (int i = 0; i < 5; i++)
            {
                var stats = GetEnemyUnitStats(i);

                int guerrieriIniziali = units.Guerrieri[i];
                units.Guerrieri[i] = RidurreNumeroSoldati(guerrieriIniziali, dannoPerTipo, stats.GuerrieriDifesa * guerrieriIniziali, stats.GuerrieriSalute);
                fase.Difensore.Sopravvisuti.Guerrieri[i] = units.Guerrieri[i];
                fase.Difensore.Perdite.Guerrieri[i] += guerrieriIniziali - units.Guerrieri[i];

                int lancieriIniziali = units.Lancieri[i];
                units.Lancieri[i] = RidurreNumeroSoldati(lancieriIniziali, dannoPerTipo, stats.LancieriDifesa * lancieriIniziali, stats.LancieriSalute);
                fase.Difensore.Sopravvisuti.Lancieri[i] = units.Lancieri[i];
                fase.Difensore.Perdite.Lancieri[i] += lancieriIniziali - units.Lancieri[i];

                int arcieriIniziali = units.Arcieri[i];
                units.Arcieri[i] = RidurreNumeroSoldati(arcieriIniziali, dannoPerTipo, stats.ArcieriDifesa * arcieriIniziali, stats.ArcieriSalute);
                fase.Difensore.Sopravvisuti.Arcieri[i] = units.Arcieri[i];
                fase.Difensore.Perdite.Arcieri[i] += arcieriIniziali - units.Arcieri[i];

                int catapulteIniziali = units.Catapulte[i];
                units.Catapulte[i] = RidurreNumeroSoldati(catapulteIniziali, dannoPerTipo, stats.CatapulteDifesa * catapulteIniziali, stats.CatapulteSalute);
                fase.Difensore.Sopravvisuti.Catapulte[i] = units.Catapulte[i];
                fase.Difensore.Perdite.Catapulte[i] += catapulteIniziali - units.Catapulte[i];
            }
        }
        internal static int CalcolaEsperienzaPVE(UnitGroup perditeBarbare)
        {
            int esperienza = 0;
            for (int i = 0; i < 5; i++)
            {
                var stats = GetEnemyUnitStats(i);
                esperienza += perditeBarbare.Guerrieri[i] * stats.GuerrieriEsperienza;
                esperienza += perditeBarbare.Lancieri[i] * stats.LancieriEsperienza;
                esperienza += perditeBarbare.Arcieri[i] * stats.ArcieriEsperienza;
                esperienza += perditeBarbare.Catapulte[i] * stats.CatapulteEsperienza;
            }
            return esperienza;
        }

        // CalcolaForza è stata spostata in Battaglia.cs il 2026-09-14 (identica a quella usata da BattagliaPVP.cs).
        internal static double CalcolaForzaBarbari(UnitGroup units)
        {
            double forza = 0;
            for (int i = 0; i < 5; i++)
            {
                var stats = GetEnemyUnitStats(i);
                forza += units.Guerrieri[i] * (stats.GuerrieriAttacco * 0.8 + stats.GuerrieriDifesa * 0.5 + stats.GuerrieriSalute * 0.3);
                forza += units.Lancieri[i] * (stats.LancieriAttacco * 0.8 + stats.LancieriDifesa * 0.5 + stats.LancieriSalute * 0.3);
                forza += units.Arcieri[i] * (stats.ArcieriAttacco * 0.8 + stats.ArcieriDifesa * 0.5 + stats.ArcieriSalute * 0.3);
                forza += units.Catapulte[i] * (stats.CatapulteAttacco * 0.8 + stats.CatapulteDifesa * 0.5 + stats.CatapulteSalute * 0.3);
            }
            return Math.Round(forza);
        }

        // ═══════════════════════════════════════════════════════════════
        // BOTTINO
        // ═══════════════════════════════════════════════════════════════

        // RaccoliRisorseEquamente è stata spostata in Battaglia.cs il 2026-09-14 (identica a quella usata da BattagliaPVP.cs).

        private static Report AssegnaRisorseVittoria_PVE(Giocatori.Player player, Guid clientGuid, string tipo, int livello, UnitGroup sopravvissuti, Report report)
        {
            // Bilanciamento: stesso nerf /5 applicato al saccheggio PVP, per coerenza tra le due modalità (confermato dall'utente).
            int capacitàCarico = CapacitàCarico(sopravvissuti, player) / 5;
            int capacitàOriginale = capacitàCarico;

            int cibo = 0, legno = 0, pietra = 0, ferro = 0, oro = 0, exp = 0, diamBlu = 0, diamViola = 0;
            Barbari.CittaBarbara citta = null;
            Barbari.VillaggioBarbaro villaggio = null;

            if (tipo == "Città Barbaro")
            {
                citta = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == livello);
                if (citta == null) { SendClient(clientGuid, "Log_Server|[error]Città barbara non trovata!"); return report; }
                (cibo, legno, pietra, ferro, oro, exp, diamBlu, diamViola) = (citta.Cibo, citta.Legno, citta.Pietra, citta.Ferro, citta.Oro, citta.Esperienza, citta.Diamanti_Blu, citta.Diamanti_Viola);
            }
            else if (tipo == "Villaggio Barbaro")
            {
                if (player.VillaggiPersonali == null || livello - 1 < 0 || livello - 1 >= player.VillaggiPersonali.Count)
                { SendClient(clientGuid, "Log_Server|[error]Villaggio barbaro non trovato!"); return report; }
                villaggio = player.VillaggiPersonali[livello - 1];
                if (villaggio == null) { SendClient(clientGuid, "Log_Server|[error]Villaggio barbaro non trovato!"); return report; }
                (cibo, legno, pietra, ferro, oro, exp, diamBlu, diamViola) = (villaggio.Cibo, villaggio.Legno, villaggio.Pietra, villaggio.Ferro, villaggio.Oro, villaggio.Esperienza, villaggio.Diamanti_Blu, villaggio.Diamanti_Viola);
            }

            var raccolte = RaccoliRisorseEquamente(capacitàCarico, cibo, legno, pietra, ferro, oro, exp, diamBlu, diamViola);

            player.Cibo += raccolte.Cibo;
            player.Legno += raccolte.Legno;
            player.Pietra += raccolte.Pietra;
            player.Ferro += raccolte.Ferro;
            player.Oro += raccolte.Oro;
            player.Diamanti_Blu += raccolte.Diamanti_Blu;
            player.Diamanti_Viola += raccolte.Diamanti_Viola;
            player.Risorse_Razziate += raccolte.Cibo + raccolte.Legno + raccolte.Pietra + raccolte.Ferro + raccolte.Oro + raccolte.Diamanti_Blu + raccolte.Diamanti_Viola;
            Esperienza.AddExp(player, exp); // l'esperienza di conquista non è soggetta al peso della capacità di trasporto

            if (citta != null)
            {
                citta.Cibo -= raccolte.Cibo; citta.Legno -= raccolte.Legno; citta.Pietra -= raccolte.Pietra;
                citta.Ferro -= raccolte.Ferro; citta.Oro -= raccolte.Oro; citta.Esperienza -= exp;
                citta.Diamanti_Blu -= raccolte.Diamanti_Blu; citta.Diamanti_Viola -= raccolte.Diamanti_Viola;
            }
            else if (villaggio != null)
            {
                villaggio.Cibo -= raccolte.Cibo; villaggio.Legno -= raccolte.Legno; villaggio.Pietra -= raccolte.Pietra;
                villaggio.Ferro -= raccolte.Ferro; villaggio.Oro -= raccolte.Oro; villaggio.Esperienza -= exp;
                villaggio.Diamanti_Blu -= raccolte.Diamanti_Blu; villaggio.Diamanti_Viola -= raccolte.Diamanti_Viola;
            }

            int pesoUtilizzato =
                raccolte.Cibo * Variabili_Server.peso_Risorse_Cibo + raccolte.Legno * Variabili_Server.peso_Risorse_Legno +
                raccolte.Pietra * Variabili_Server.peso_Risorse_Pietra + raccolte.Ferro * Variabili_Server.peso_Risorse_Ferro +
                raccolte.Oro * Variabili_Server.peso_Risorse_Oro + raccolte.Diamanti_Blu * Variabili_Server.peso_Risorse_Diamante_Blu +
                raccolte.Diamanti_Viola * Variabili_Server.peso_Risorse_Diamante_Viola;

            SendClient(clientGuid, "Log_Server|╔══════════════════════════════════════════════════╗");
            SendClient(clientGuid, $"Log_Server|║  CONQUISTA {(tipo == "Città Barbaro" ? "CITTÀ" : "VILLAGGIO")} - RISORSE RACCOLTE        ║");
            SendClient(clientGuid, "Log_Server|╚══════════════════════════════════════════════════╝");
            SendClient(clientGuid, $"Log_Server|[highlight]Capacità di carico: [info]{capacitàOriginale:N0}");
            SendClient(clientGuid, $"Log_Server|[highlight]Capacità utilizzata: [info]{pesoUtilizzato:N0}");
            if (raccolte.Cibo > 0) SendClient(clientGuid, $"Log_Server|[cibo]Cibo: +{raccolte.Cibo:N0}[/cibo][icon:cibo]");
            if (raccolte.Legno > 0) SendClient(clientGuid, $"Log_Server|[legno]Legno: +{raccolte.Legno:N0}[/legno][icon:legno]");
            if (raccolte.Pietra > 0) SendClient(clientGuid, $"Log_Server|[pietra]Pietra: +{raccolte.Pietra:N0}[/pietra][icon:pietra]");
            if (raccolte.Ferro > 0) SendClient(clientGuid, $"Log_Server|[ferro]Ferro: +{raccolte.Ferro:N0}[/ferro][icon:ferro]");
            if (raccolte.Oro > 0) SendClient(clientGuid, $"Log_Server|[oro]Oro: +{raccolte.Oro:N0}[/oro][icon:oro]");
            if (raccolte.Diamanti_Blu > 0) SendClient(clientGuid, $"Log_Server|[blu]Diamanti Blu: +{raccolte.Diamanti_Blu:N0}[/blu][icon:diamanteBlu]");
            if (raccolte.Diamanti_Viola > 0) SendClient(clientGuid, $"Log_Server|[viola]Diamanti Viola: +{raccolte.Diamanti_Viola:N0}[/viola][icon:diamanteViola]");
            SendClient(clientGuid, "Log_Server|════════════════════════════════════════════════════\n");

            report.Battaglia.Risorse_Raccolte = raccolte;
            report.Battaglia.Risorse_Raccolte.Capacità_Carico = capacitàOriginale;
            report.Battaglia.Risorse_Raccolte.Capacità_Carico_Usata = pesoUtilizzato;
            return report;
        }

        // ═══════════════════════════════════════════════════════════════
        // FASE UNICA (i barbari non hanno strati multipli come nel PVP)
        // ═══════════════════════════════════════════════════════════════

        // 19/09/2026, su richiesta dell'utente ("i barbari hanno anche Difesa e Salute"): Villaggi/Città Barbare hanno
        // sempre avuto questi due campi (Barbari.cs, scalano col livello e si "riparano" di 1/giorno — vedi
        // RiparaVillaggiBarbari/RiparaCittàBarbare), ma finora la battaglia li ignorava del tutto: la riparazione
        // giornaliera non serviva a nulla perché niente li faceva mai scendere. Ora la fase corpo a corpo li usa
        // esattamente come lo strato Ingresso/Centro nel PVP (vedi BattagliaPVP.cs, Battaglia_corpo_a_Corpo): 30%
        // del danno assorbito dalla Difesa, poi il 20% di quel che resta assorbito dalla Salute, il resto va alle
        // truppe — e Difesa/Salute restano scalati sull'oggetto Barbari stesso (danno permanente finché non si
        // rigenerano da soli). "target" può essere null (barbaro non trovato): in quel caso si comporta come prima.
        private static RisultatoFase Battaglia_Fase(Giocatori.Player player, UnitGroup attackerUnits, UnitGroup enemyUnits, Barbari.BarbarianBase target)
        {
            var fase = new RisultatoFase
            {
                Struttura = new Villaggio
                {
                    Nome = "Guarnigione Barbara",
                    Guarnigione = enemyUnits.TotalUnits(),
                    Salute = target?.Salute ?? 0,
                    Difesa = target?.Difesa ?? 0,
                    SaluteIniziale = target?.Salute ?? 0,
                    DifesaIniziale = target?.Difesa ?? 0,
                }
            };

            // Fase a distanza (Salute/Difesa del barbaro non intervengono qui, solo nel corpo a corpo — vedi sotto)
            var rangedResult = BattagliaDistanzaPVE(attackerUnits, enemyUnits, player);
            fase.Attaccante.Schierati = rangedResult.Attaccante_Sopravvisuti;
            fase.Difensore.Schierati = rangedResult.Difensore_Sopravvisuti;
            fase.Fase_Distanza = rangedResult;
            fase.Unità_Presenti_Difensore = rangedResult.Difensore_Unità_Presenti;

            // Fase corpo a corpo
            var attaccantiVivi = fase.Attaccante.Schierati;
            var difensoriVivi = fase.Difensore.Schierati;

            bool usaFrecce = difensoriVivi.TotalUnits() > 0;
            double dannoAttaccante = CalcolaDannoGiocatore(attaccantiVivi, player, usaFrecce, fase);
            double dannoDifensore = CalcolaDannoBarbari(difensoriVivi);

            // Assorbimento Difesa/Salute del barbaro (stessa proporzione 30%/20% del PVP, vedi commento sopra).
            if (target != null && fase.Struttura.Salute > 5)
            {
                double dannotempDifesa = dannoAttaccante * 0.30;
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

                double dannotempSalute = (dannoAttaccante - dannotempDifesa) * 0.20;
                if (dannotempSalute >= fase.Struttura.Salute)
                {
                    dannoAttaccante -= fase.Struttura.Salute;
                    fase.Struttura.Salute = 0;
                }
                else
                {
                    fase.Struttura.Salute -= (int)dannotempSalute;
                    dannoAttaccante -= dannotempSalute;
                }

                // Danno permanente: resta scalato finché RiparaVillaggiBarbari/RiparaCittàBarbare
                // (Barbari.cs, +1/giorno) non lo recupera pian piano.
                target.Difesa = fase.Struttura.Difesa;
                target.Salute = fase.Struttura.Salute;
                player.Danno_HP_Barbaro += fase.Struttura.Difesa;
                player.Danno_HP_Barbaro += fase.Struttura.Salute;
            }

            double dannoPerTipoAttaccante = dannoDifensore / attaccantiVivi.CountUnitTypes();
            double dannoPerTipoDifensore = dannoAttaccante / difensoriVivi.CountUnitTypes();

            ApplicaDanniAttaccante(fase, attaccantiVivi.Clone(), player, dannoPerTipoAttaccante);
            ApplicaDanniBarbari(fase, difensoriVivi.Clone(), dannoPerTipoDifensore);

            fase.Xp_Attaccante = CalcolaEsperienzaPVE(fase.Difensore.Perdite);
            fase.Vittoria_Attaccante = fase.Difensore.Sopravvisuti.TotalUnits() == 0;
            return fase;
        }

        // ═══════════════════════════════════════════════════════════════
        // PUNTO D'INGRESSO
        // ═══════════════════════════════════════════════════════════════

        public static async Task<Report> Battaglia(Giocatori.Player player, Guid clientGuid, string tipo, int livello, UnitGroup attackerUnits)
        {
            if (tipo == "Villaggio Barbaro") OnEvent(player, QuestEventType.Battaglie, "Attacco Villaggio Barbaro", 1);
            if (tipo == "Città Barbaro") OnEvent(player, QuestEventType.Battaglie, "Attacco Citta Barbaro", 1);

            var enemyUnits = CaricaUnitaNemiche(livello, tipo, player);
            var target = CaricaBarbaro(livello, tipo, player); // per Salute/Difesa nella fase corpo a corpo, vedi Battaglia_Fase

            var report = new Report
            {
                Tipo = "Battaglia",
                Data = DateTime.UtcNow.ToString("o"), // formato ISO 8601 ("o"), vedi Spionaggio.cs: senza formato new Date(...) in JS non riesce a interpretare la data
                Aperto = false,
                Battaglia = new RisultatoBattaglia
                {
                    Nome_Attaccante = player.Username,
                    Nome_Difensore = $"{tipo} Lv.{livello}",
                    Tipo_Battaglia = "PVE"
                }
            };

            var fase = Battaglia_Fase(player, attackerUnits, enemyUnits, target);
            report.Battaglia.Fasi.Add(fase);
            report.Battaglia.Vittoria_Attaccante = fase.Vittoria_Attaccante;
            report.Battaglia.Xp_Attaccante = fase.Xp_Attaccante + fase.Fase_Distanza.Attaccante_XP;

            report.Battaglia.Forza_Attaccante = CalcolaForza(attackerUnits);
            report.Battaglia.Forza_Attaccante_Finale = CalcolaForza(fase.Attaccante.Sopravvisuti);
            report.Battaglia.Forza_Difensore = CalcolaForzaBarbari(enemyUnits);
            report.Battaglia.Forza_Difensore_Finale = CalcolaForzaBarbari(fase.Difensore.Sopravvisuti);

            // Assegna risorse su vittoria
            if (fase.Vittoria_Attaccante)
            {
                report = AssegnaRisorseVittoria_PVE(player, clientGuid, tipo, livello, fase.Attaccante.Sopravvisuti, report);
                player.Battaglie_Vinte++;
                if (tipo == "Villaggio Barbaro") player.Accampamenti_Barbari_Sconfitti++;
                if (tipo == "Città Barbaro") player.Città_Barbare_Sconfitte++;
            }
            else player.Battaglie_Perse++;

            // Aggiorna esercito del giocatore (perdite totali = schierati - sopravvissuti finali)
            var perditeAttaccante = attackerUnits.Subtract(fase.Attaccante.Sopravvisuti);
            for (int i = 0; i < 5; i++)
            {
                player.Guerrieri[i] -= perditeAttaccante.Guerrieri[i];
                player.Lanceri[i] -= perditeAttaccante.Lancieri[i];
                player.Arceri[i] -= perditeAttaccante.Arcieri[i];
                player.Catapulte[i] -= perditeAttaccante.Catapulte[i];
            }

            Esperienza.AddExp(player, report.Battaglia.Xp_Attaccante);
            AggiornaBarbari(livello, tipo, player, fase.Difensore.Sopravvisuti);

            player.Report.Add(report);

            // Invio live del report: fino al 15/09/2026 veniva mandato subito qui a mano
            // (stesso schema di BattagliaPVP.cs), perché prima il referto arrivava al
            // client solo al login. Dal 16/09/2026 questo invio esplicito non serve più:
            // ServerConnection.Update_Data (il tick di gioco, circa ogni secondo) rileva
            // da solo il cambio di player.Report.Count e manda il Report_Lista aggiornato
            // — vedi PlayerSnapshot.ReportCountChanged.

            // Statistiche/Quest
            player.Guerrieri_Eliminati += fase.Difensore.Perdite.Guerrieri.Sum() + fase.Fase_Distanza.Difensore_Morti.Guerrieri.Sum();
            player.Lanceri_Eliminati += fase.Difensore.Perdite.Lancieri.Sum() + fase.Fase_Distanza.Difensore_Morti.Lancieri.Sum();
            player.Arceri_Eliminati += fase.Difensore.Perdite.Arcieri.Sum();
            player.Catapulte_Eliminate += fase.Difensore.Perdite.Catapulte.Sum();
            player.Barbari_Sconfitti += fase.Difensore.Perdite.TotalUnits() + fase.Fase_Distanza.Difensore_Morti.TotalUnits();

            player.Guerrieri_Persi += perditeAttaccante.Guerrieri.Sum();
            player.Lanceri_Persi += perditeAttaccante.Lancieri.Sum();
            player.Arceri_Persi += perditeAttaccante.Arcieri.Sum();
            player.Catapulte_Perse += perditeAttaccante.Catapulte.Sum();

            player.Unità_Eliminate += fase.Difensore.Perdite.TotalUnits() + fase.Fase_Distanza.Difensore_Morti.TotalUnits();
            player.Unità_Perse += perditeAttaccante.TotalUnits();

            OnEvent(player, QuestEventType.Uccisioni, "Guerrieri", fase.Difensore.Perdite.Guerrieri.Sum() + fase.Fase_Distanza.Difensore_Morti.Guerrieri.Sum());
            OnEvent(player, QuestEventType.Uccisioni, "Lanceri", fase.Difensore.Perdite.Lancieri.Sum() + fase.Fase_Distanza.Difensore_Morti.Lancieri.Sum());
            OnEvent(player, QuestEventType.Uccisioni, "Arceri", fase.Difensore.Perdite.Arcieri.Sum());
            OnEvent(player, QuestEventType.Uccisioni, "Catapulte", fase.Difensore.Perdite.Catapulte.Sum());
            OnEvent(player, QuestEventType.Risorse, "Frecce", fase.Fase_Distanza.Attaccante_Frecce_Usate);

            SendClient(clientGuid, $"Log_Server|{(fase.Vittoria_Attaccante ? "[verde]VITTORIA!" : "[error]SCONFITTA")} contro {tipo} Lv.{livello} — Esperienza guadagnata: {report.Battaglia.Xp_Attaccante}");

            return report;
        }
    }
}
