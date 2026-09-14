
using Server_Strategico.Gioco;
using Server_Strategico.ServerData.Moduli.Player;

namespace Server_Strategico.ServerData.Moduli.Battaglie
{
    public class Battaglia
    {
        public class UnitGroup
        {
            public int[] Guerrieri { get; set; }
            public int[] Lancieri { get; set; }
            public int[] Arcieri { get; set; }
            public int[] Catapulte { get; set; }

            public UnitGroup()
            {
                Guerrieri = new int[5];
                Lancieri = new int[5];
                Arcieri = new int[5];
                Catapulte = new int[5];
            }
            public UnitGroup Clone()
            {
                return new UnitGroup
                {
                    Guerrieri = (int[])Guerrieri.Clone(),
                    Lancieri = (int[])Lancieri.Clone(),
                    Arcieri = (int[])Arcieri.Clone(),
                    Catapulte = (int[])Catapulte.Clone()
                };
            }
            public int TotalUnits()
            {
                return Guerrieri.Sum() + Lancieri.Sum() + Arcieri.Sum() + Catapulte.Sum();
            }
            public int CountUnitTypes()
            {
                int count = 0;
                if (Guerrieri.Any(g => g > 0)) count++;
                if (Lancieri.Any(l => l > 0)) count++;
                if (Arcieri.Any(a => a > 0)) count++;
                if (Catapulte.Any(c => c > 0)) count++;
                return Math.Max(count, 1); // Evita divisione per zero
            }
            //Sottrae le unità di un gruppo da questo gruppo, restituendo un nuovo gruppo con i risultati (non modifica il gruppo originale)
            public UnitGroup Subtract(UnitGroup group)
            {
                return new UnitGroup
                {
                    Guerrieri = Guerrieri.Zip(group.Guerrieri, (a, b) => Math.Max(0, a - b)).ToArray(),
                    Lancieri = Lancieri.Zip(group.Lancieri, (a, b) => Math.Max(0, a - b)).ToArray(),
                    Arcieri = Arcieri.Zip(group.Arcieri, (a, b) => Math.Max(0, a - b)).ToArray(),
                    Catapulte = Catapulte.Zip(group.Catapulte, (a, b) => Math.Max(0, a - b)).ToArray()
                };
            }
        }
        public class Report
        {
            public string Tipo { get; set; } = "";
            public string Data { get; set; } = "";
            public bool Aperto { get; set; } = false;
            public RisultatoBattaglia Battaglia { get; set; }
            public RisultatoSpionaggio Spionaggio { get; set; }
        }

        public class RisultatoBattaglia
        {
            public string Tipo_Battaglia { get; set; } = "";
            public string Nome_Attaccante { get; set; } = "";
            public string Nome_Difensore { get; set; } = "";
            public double Forza_Attaccante { get; set; }
            public double Forza_Attaccante_Finale { get; set; }
            public double Forza_Difensore { get; set; }
            public double Forza_Difensore_Finale { get; set; }
            public bool Vittoria_Attaccante { get; set; }
            public int Xp_Attaccante { get; set; } = 0;
            public int Xp_Difensore { get; set; } = 0;
            public List<RisultatoFase> Fasi { get; set; } = new List<RisultatoFase>();
            public BonusRicerca Bonus_Ricerca_Difesa { get; set; } = new BonusRicerca();
            public BonusRicerca Bonus_Ricerca_Attacco { get; set; } = new BonusRicerca();
            public RisorseRaccolte Risorse_Raccolte { get; set; } = new RisorseRaccolte();
            // Riepilogo di tutti i partecipanti a un Raduno (AttacchiCooperativi.cs/Raduni.cs): ogni
            // partecipante riceve un Report personale (stesso RisultatoBattaglia di PVP/PVE) più questo
            // elenco completo — campo additivo/non-breaking, resta null/vuoto per PVP e PVE normali.
            // Anche questo campo è andato perso il 14/09/2026 nella stessa cancellazione accidentale
            // delle funzioni di supporto — ricostruito dai punti di chiamata in Raduni.cs.
            public List<PartecipanteRaduno> Partecipanti { get; set; } = new List<PartecipanteRaduno>();
        }
        public class PartecipanteRaduno
        {
            public string Username { get; set; } = "";
            public UnitGroup Truppe_Inviate { get; set; } = new UnitGroup();
            public UnitGroup Truppe_Sopravvissute { get; set; } = new UnitGroup();
            public UnitGroup Truppe_Perse { get; set; } = new UnitGroup();
            public int Esperienza_Guadagnata { get; set; } = 0;
            public RisorseRaccolte Risorse_Raccolte { get; set; } = new RisorseRaccolte();
        }
        public class RisultatoFase
        {
            public bool Vittoria_Attaccante { get; set; }
            public bool Unità_Presenti_Difensore { get; set; }
            public bool Struttura_Crollata { get; set; }
            public int Xp_Attaccante { get; set; } = 0;
            public int Xp_Difensore { get; set; } = 0;
            public Villaggio Struttura { get; set; } = new Villaggio();
            public BattagliaDistanza Fase_Distanza { get; set; } = new BattagliaDistanza();
            public Unità Attaccante { get; set; } = new Unità();
            public Unità Difensore { get; set; } = new Unità();
            public UnitGroup Perdite_Crollo { get; set; } = new UnitGroup();
        }
        public class Unità
        {
            public UnitGroup Schierati { get; set; } = new UnitGroup();
            public UnitGroup Sopravvisuti { get; set; } = new UnitGroup();
            public UnitGroup Perdite { get; set; } = new UnitGroup();
        }
        public class Villaggio
        {
            public string Nome { get; set; } = "";
            public int Salute { get; set; }
            public int Difesa { get; set; }
            public int SaluteMax { get; set; }
            public int DifesaMax { get; set; }
            public int Guarnigione { get; set; }
            // Snapshot di Salute/Difesa PRIMA del combattimento (14/09/2026, richiesto
            // dall'utente: "mostra il passaggio di stato, es. da 30/30 hp a 15/30 hp").
            // Salute/Difesa qui sopra vengono decrementati IN PLACE durante
            // Battaglia_corpo_a_Corpo (BattagliaPVP.cs), quindi a fine combattimento
            // rappresentano già il valore FINALE — senza questi due campi, popolati
            // una volta sola alla creazione della fase e mai più toccati, il valore
            // "prima" andrebbe perso.
            public int SaluteIniziale { get; set; }
            public int DifesaIniziale { get; set; }
        }
        public class BonusRicerca
        {
            public string Bonus_Salute_Unità { get; set; } = "0";
            public string Bonus_Difesa_Unità { get; set; } = "0";
            public string Bonus_Attacco_Unità { get; set; } = "0";
            public string Bonus_Salute_Strutture { get; set; } = "0";
            public string Bonus_Difesa_Strutture { get; set; } = "0";
            public string Bonus_Guarnigione_Strutture { get; set; } = "0";
        }
        public class BattagliaDistanza
        {
            public bool Attaccante_Poche_Frecce { get; set; }
            public bool Difensore_Poche_Frecce { get; set; }
            public UnitGroup Attaccante_Schierati { get; set; } = new UnitGroup();
            public UnitGroup Attaccante_Sopravvisuti { get; set; } = new UnitGroup();
            public UnitGroup Attaccante_Morti { get; set; } = new UnitGroup();
            public UnitGroup Difensore_Schierati { get; set; } = new UnitGroup();
            public UnitGroup Difensore_Sopravvisuti { get; set; } = new UnitGroup();
            public UnitGroup Difensore_Morti { get; set; } = new UnitGroup();
            public int Attaccante_XP { get; set; } = 0;
            public int Attaccante_Danno_Guerrieri { get; set; }
            public int Attaccante_Danno_Lancieri { get; set; }
            public int Attaccante_Frecce_Usate { get; set; }
            public int Attaccante_Frecce_Necessarie { get; set; }
            public int Difensore_XP { get; set; } = 0;
            public int Difensore_Danno_Guerrieri { get; set; }
            public int Difensore_Danno_Lancieri { get; set; }
            public int Difensore_Frecce_Usate { get; set; }
            public int Difensore_Frecce_Necessarie { get; set; }
            public bool Difensore_Unità_Presenti { get; set; }
        }
        public class RangedBattleResult
        {
            public UnitGroup Attaccante_Sopravvisuti { get; set; }
            public UnitGroup Difensore_Sopravvisuti { get; set; }

            public int[] DefenderKills_Guerrieri { get; set; }  // Array di 5 elementi
            public int[] DefenderKills_Lancieri { get; set; }    // Array di 5 elementi
            public int[] AttackerKills_Guerrieri { get; set; }    // Array di 5 elementi
            public int[] AttackerKills_Lancieri { get; set; }     // Array di 5 elementi

            public int Attaccante_Danno_Guerrieri { get; set; }
            public int Attaccante_Danno_Lancieri { get; set; }
            public int Difensore_Danno_Guerrieri { get; set; }
            public int Difensore_Danno_Lancieri { get; set; }

            public int Attaccante_Frecce_Usate { get; set; }
            public int Attaccante_Frecce_Necessarie { get; set; }
            public bool Attaccante_Poche_Frecce { get; set; }
            public int Attaccante_XP { get; set; }
            public int Difensore_Frecce_Usate { get; set; }
            public int Difensore_Frecce_Necessarie { get; set; }
            public int Difensore_XP { get; set; }
            public bool Difensore_Poche_Frecce { get; set; }
            public bool Difensore_Unità_Presenti { get; set; }

            public RangedBattleResult()
            {
                AttackerKills_Guerrieri = new int[5];
                AttackerKills_Lancieri = new int[5];
                DefenderKills_Guerrieri = new int[5];
                DefenderKills_Lancieri = new int[5];
            }

            // Helper per ottenere totali (utile per log)
            public int GetTotalPlayerKills() => AttackerKills_Guerrieri.Sum() + AttackerKills_Lancieri.Sum();
            public int GetTotalEnemyKills() => DefenderKills_Guerrieri.Sum() + DefenderKills_Lancieri.Sum();
        }
        public class RisorseRaccolte
        {
            public int Cibo { get; set; }
            public int Legno { get; set; }
            public int Pietra { get; set; }
            public int Ferro { get; set; }
            public int Oro { get; set; }
            public int Esperienza { get; set; }
            public int Diamanti_Blu { get; set; }
            public int Diamanti_Viola { get; set; }
            public int Capacità_Carico { get; set; }
            public int Capacità_Carico_Usata { get; set; }
        }

        // ═══════════════════════════════════════════════════════════════
        // FUNZIONI DI SUPPORTO CONDIVISE (PVP/PVE/Spionaggio) — RICOSTRUITE
        // il 14/09/2026 dopo che una mia modifica le aveva cancellate per errore
        // (vedi conversazione: la copia locale usata per l'edit era rimasta
        // indietro rispetto all'accorpamento fatto lo stesso giorno). Erano
        // duplicate letteralmente identiche tra BattagliaPVP.cs/BattagliaPVE.cs
        // (da cui restano richiamabili senza prefisso grazie a
        // "using static ...Battaglia;") e servono anche a Spionaggio.cs
        // (GetPlayerUnitStats). Ricostruite usando: i punti di chiamata rimasti
        // in BattagliaPVP.cs/BattagliaPVE.cs (per firma e tipi esatti),
        // CalcolaForzaBarbari/RidurreNumeroSoldati tuttora presenti nel codice
        // (con commento "identica a quella usata da BattagliaPVP.cs") come
        // riferimento diretto, e la logica delle vecchie Gioco/BattaglieV2.cs
        // per il resto — quindi con LO STESSO RISCHIO di qualunque ricostruzione:
        // controllare che i numeri restino quelli attesi, specialmente
        // CapacitàCarico (vedi nota sotto, un fattore della vecchia formula non
        // esiste più nell'attuale Ricerca.cs ed è stato omesso).
        public static int RidurreNumeroSoldati(int numeroSoldati, double danno, double difesa, double salutePerSoldato)
        {
            double dannoEffettivo = Math.Max(0, danno - difesa);
            int soldatiPersi = Convert.ToInt32(dannoEffettivo / salutePerSoldato);
            numeroSoldati -= soldatiPersi;
            return numeroSoldati < 0 ? 0 : numeroSoldati;
        }

        public static void SendClient(Guid clientGuid, string message)
        {
            Server_Strategico.Server.Server.Send(clientGuid, message);
            Console.WriteLine(message.Replace("Log_Server|", ""));
        }

        public static int CalcoloFrecce(UnitGroup unità)
        {
            int frecce = unità.Arcieri[0] * Esercito.Unità.Arcere_1.Componente_Lancio + unità.Catapulte[0] * Esercito.Unità.Catapulta_1.Componente_Lancio;
            frecce += unità.Arcieri[1] * Esercito.Unità.Arcere_2.Componente_Lancio + unità.Catapulte[1] * Esercito.Unità.Catapulta_2.Componente_Lancio;
            frecce += unità.Arcieri[2] * Esercito.Unità.Arcere_3.Componente_Lancio + unità.Catapulte[2] * Esercito.Unità.Catapulta_3.Componente_Lancio;
            frecce += unità.Arcieri[3] * Esercito.Unità.Arcere_4.Componente_Lancio + unità.Catapulte[3] * Esercito.Unità.Catapulta_4.Componente_Lancio;
            frecce += unità.Arcieri[4] * Esercito.Unità.Arcere_5.Componente_Lancio + unità.Catapulte[4] * Esercito.Unità.Catapulta_5.Componente_Lancio;
            return frecce;
        }

        // Applica "danno" (punti-truppa) a Guerrieri/Lancieri dal tier più basso al più alto, mutando "units" e
        // restituendo un UnitGroup con i soli morti (Arcieri/Catapulte non sono bersaglio del tiro a distanza).
        public static UnitGroup ApplicaDanniDistanza_(UnitGroup units, int dannoGuerrieri, int dannoLancieri)
        {
            var morti = new UnitGroup();
            int dannoRimanente = dannoGuerrieri;
            for (int i = 0; i < 5 && dannoRimanente > 0; i++)
                if (units.Guerrieri[i] > 0)
                {
                    int persi = Math.Min(units.Guerrieri[i], dannoRimanente);
                    units.Guerrieri[i] -= persi;
                    morti.Guerrieri[i] = persi;
                    dannoRimanente -= persi;
                }
            dannoRimanente = dannoLancieri;
            for (int i = 0; i < 5 && dannoRimanente > 0; i++)
                if (units.Lancieri[i] > 0)
                {
                    int persi = Math.Min(units.Lancieri[i], dannoRimanente);
                    units.Lancieri[i] -= persi;
                    morti.Lancieri[i] = persi;
                    dannoRimanente -= persi;
                }
            return morti;
        }

        public static double CalcolaForza(UnitGroup units)
        {
            double forza = 0;
            for (int i = 0; i < 5; i++)
            {
                var stats = GetUnitStats(i);
                forza += units.Guerrieri[i] * (stats.GuerrieriAttacco * 0.8 + stats.GuerrieriDifesa * 0.5 + stats.GuerrieriSalute * 0.3);
                forza += units.Lancieri[i] * (stats.LancieriAttacco * 0.8 + stats.LancieriDifesa * 0.5 + stats.LancieriSalute * 0.3);
                forza += units.Arcieri[i] * (stats.ArcieriAttacco * 0.8 + stats.ArcieriDifesa * 0.5 + stats.ArcieriSalute * 0.3);
                forza += units.Catapulte[i] * (stats.CatapulteAttacco * 0.8 + stats.CatapulteDifesa * 0.5 + stats.CatapulteSalute * 0.3);
            }
            return Math.Round(forza);
        }

        // Statistiche base per tier, SENZA bonus giocatore (usata per CalcolaForza e per l'esperienza data in
        // battaglia) — identica nella forma a CaricaDatiStruttureDifensore/GetEnemyUnitStats in BattagliaPVE.cs.
        public static (double GuerrieriAttacco, double GuerrieriDifesa, double GuerrieriSalute, int GuerrieriEsperienza,
                       double LancieriAttacco, double LancieriDifesa, double LancieriSalute, int LancieriEsperienza,
                       double ArcieriAttacco, double ArcieriDifesa, double ArcieriSalute, int ArcieriEsperienza,
                       double CatapulteAttacco, double CatapulteDifesa, double CatapulteSalute, int CatapulteEsperienza)
            GetUnitStats(int level)
        {
            return level switch
            {
                0 => (Esercito.Unità.Guerriero_1.Attacco, Esercito.Unità.Guerriero_1.Difesa, Esercito.Unità.Guerriero_1.Salute, Esercito.Unità.Guerriero_1.Esperienza,
                      Esercito.Unità.Lancere_1.Attacco, Esercito.Unità.Lancere_1.Difesa, Esercito.Unità.Lancere_1.Salute, Esercito.Unità.Lancere_1.Esperienza,
                      Esercito.Unità.Arcere_1.Attacco, Esercito.Unità.Arcere_1.Difesa, Esercito.Unità.Arcere_1.Salute, Esercito.Unità.Arcere_1.Esperienza,
                      Esercito.Unità.Catapulta_1.Attacco, Esercito.Unità.Catapulta_1.Difesa, Esercito.Unità.Catapulta_1.Salute, Esercito.Unità.Catapulta_1.Esperienza),
                1 => (Esercito.Unità.Guerriero_2.Attacco, Esercito.Unità.Guerriero_2.Difesa, Esercito.Unità.Guerriero_2.Salute, Esercito.Unità.Guerriero_2.Esperienza,
                      Esercito.Unità.Lancere_2.Attacco, Esercito.Unità.Lancere_2.Difesa, Esercito.Unità.Lancere_2.Salute, Esercito.Unità.Lancere_2.Esperienza,
                      Esercito.Unità.Arcere_2.Attacco, Esercito.Unità.Arcere_2.Difesa, Esercito.Unità.Arcere_2.Salute, Esercito.Unità.Arcere_2.Esperienza,
                      Esercito.Unità.Catapulta_2.Attacco, Esercito.Unità.Catapulta_2.Difesa, Esercito.Unità.Catapulta_2.Salute, Esercito.Unità.Catapulta_2.Esperienza),
                2 => (Esercito.Unità.Guerriero_3.Attacco, Esercito.Unità.Guerriero_3.Difesa, Esercito.Unità.Guerriero_3.Salute, Esercito.Unità.Guerriero_3.Esperienza,
                      Esercito.Unità.Lancere_3.Attacco, Esercito.Unità.Lancere_3.Difesa, Esercito.Unità.Lancere_3.Salute, Esercito.Unità.Lancere_3.Esperienza,
                      Esercito.Unità.Arcere_3.Attacco, Esercito.Unità.Arcere_3.Difesa, Esercito.Unità.Arcere_3.Salute, Esercito.Unità.Arcere_3.Esperienza,
                      Esercito.Unità.Catapulta_3.Attacco, Esercito.Unità.Catapulta_3.Difesa, Esercito.Unità.Catapulta_3.Salute, Esercito.Unità.Catapulta_3.Esperienza),
                3 => (Esercito.Unità.Guerriero_4.Attacco, Esercito.Unità.Guerriero_4.Difesa, Esercito.Unità.Guerriero_4.Salute, Esercito.Unità.Guerriero_4.Esperienza,
                      Esercito.Unità.Lancere_4.Attacco, Esercito.Unità.Lancere_4.Difesa, Esercito.Unità.Lancere_4.Salute, Esercito.Unità.Lancere_4.Esperienza,
                      Esercito.Unità.Arcere_4.Attacco, Esercito.Unità.Arcere_4.Difesa, Esercito.Unità.Arcere_4.Salute, Esercito.Unità.Arcere_4.Esperienza,
                      Esercito.Unità.Catapulta_4.Attacco, Esercito.Unità.Catapulta_4.Difesa, Esercito.Unità.Catapulta_4.Salute, Esercito.Unità.Catapulta_4.Esperienza),
                4 => (Esercito.Unità.Guerriero_5.Attacco, Esercito.Unità.Guerriero_5.Difesa, Esercito.Unità.Guerriero_5.Salute, Esercito.Unità.Guerriero_5.Esperienza,
                      Esercito.Unità.Lancere_5.Attacco, Esercito.Unità.Lancere_5.Difesa, Esercito.Unità.Lancere_5.Salute, Esercito.Unità.Lancere_5.Esperienza,
                      Esercito.Unità.Arcere_5.Attacco, Esercito.Unità.Arcere_5.Difesa, Esercito.Unità.Arcere_5.Salute, Esercito.Unità.Arcere_5.Esperienza,
                      Esercito.Unità.Catapulta_5.Attacco, Esercito.Unità.Catapulta_5.Difesa, Esercito.Unità.Catapulta_5.Salute, Esercito.Unità.Catapulta_5.Esperienza),
                _ => (0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
            };
        }

        // Statistiche base + bonus del giocatore (ricerche/edifici). Usata anche da Spionaggio.cs
        // (Battaglia.GetPlayerUnitStats) per stimare le statistiche truppe del bersaglio.
        public static (double GuerrieriAttacco, double GuerrieriDifesa, double GuerrieriSalute,
                       double LancieriAttacco, double LancieriDifesa, double LancieriSalute,
                       double ArcieriAttacco, double ArcieriDifesa, double ArcieriSalute,
                       double CatapulteAttacco, double CatapulteDifesa, double CatapulteSalute)
            GetPlayerUnitStats(int level, Giocatori.Player player)
        {
            var baseStats = level switch
            {
                0 => (Esercito.Unità.Guerriero_1, Esercito.Unità.Lancere_1, Esercito.Unità.Arcere_1, Esercito.Unità.Catapulta_1),
                1 => (Esercito.Unità.Guerriero_2, Esercito.Unità.Lancere_2, Esercito.Unità.Arcere_2, Esercito.Unità.Catapulta_2),
                2 => (Esercito.Unità.Guerriero_3, Esercito.Unità.Lancere_3, Esercito.Unità.Arcere_3, Esercito.Unità.Catapulta_3),
                3 => (Esercito.Unità.Guerriero_4, Esercito.Unità.Lancere_4, Esercito.Unità.Arcere_4, Esercito.Unità.Catapulta_4),
                4 => (Esercito.Unità.Guerriero_5, Esercito.Unità.Lancere_5, Esercito.Unità.Arcere_5, Esercito.Unità.Catapulta_5),
                _ => (Esercito.Unità.Guerriero_1, Esercito.Unità.Lancere_1, Esercito.Unità.Arcere_1, Esercito.Unità.Catapulta_1)
            };

            return (
                baseStats.Item1.Attacco + Ricerca.Soldati.Incremento.Attacco + (baseStats.Item1.Attacco * (1 + player.Bonus_Attacco_Guerrieri)),
                baseStats.Item1.Difesa + Ricerca.Soldati.Incremento.Difesa + (baseStats.Item1.Difesa * (1 + player.Bonus_Difesa_Guerrieri)),
                baseStats.Item1.Salute + Ricerca.Soldati.Incremento.Salute + (baseStats.Item1.Salute * (1 + player.Bonus_Salute_Guerrieri)),

                baseStats.Item2.Attacco + Ricerca.Soldati.Incremento.Attacco + (baseStats.Item2.Attacco * (1 + player.Bonus_Attacco_Lanceri)),
                baseStats.Item2.Difesa + Ricerca.Soldati.Incremento.Difesa + (baseStats.Item2.Difesa * (1 + player.Bonus_Difesa_Lanceri)),
                baseStats.Item2.Salute + Ricerca.Soldati.Incremento.Salute + (baseStats.Item2.Salute * (1 + player.Bonus_Salute_Lanceri)),

                baseStats.Item3.Attacco + Ricerca.Soldati.Incremento.Attacco + (baseStats.Item3.Attacco * (1 + player.Bonus_Attacco_Arceri)),
                baseStats.Item3.Difesa + Ricerca.Soldati.Incremento.Difesa + (baseStats.Item3.Difesa * (1 + player.Bonus_Difesa_Arceri)),
                baseStats.Item3.Salute + Ricerca.Soldati.Incremento.Salute + (baseStats.Item3.Salute * (1 + player.Bonus_Salute_Arceri)),

                baseStats.Item4.Attacco + Ricerca.Soldati.Incremento.Attacco + (baseStats.Item4.Attacco * (1 + player.Bonus_Attacco_Catapulte)),
                baseStats.Item4.Difesa + Ricerca.Soldati.Incremento.Difesa + (baseStats.Item4.Difesa * (1 + player.Bonus_Difesa_Catapulte)),
                baseStats.Item4.Salute + Ricerca.Soldati.Incremento.Salute + (baseStats.Item4.Salute * (1 + player.Bonus_Salute_Catapulte))
            );
        }

        // ATTENZIONE: la vecchia versione (Gioco/BattaglieV2.cs) moltiplicava anche per
        // "Ricerca.Tipi.Incremento.Trasporto" — un campo che nell'attuale Ricerca.cs non esiste più
        // (Ricerca.Tipi.Trasporto è oggi solo la tabella costi/tempo della ricerca, senza moltiplicatore).
        // L'ho omesso invece di inventare un valore: la capacità di trasporto qui sotto usa solo
        // Ricerca_Trasporto (livello) e Bonus_Capacità_Trasporto (bonus edifici/eventi) del giocatore.
        // Controlla che i numeri di carico ti sembrino giusti — è l'unico punto rimasto incerto.
        public static int CapacitàCarico(UnitGroup playerUnits, Giocatori.Player player)
        {
            double moltiplicatore = 1 + player.Ricerca_Trasporto;
            int capacitàCarico = 0;
            capacitàCarico += (int)(playerUnits.Guerrieri[0] * Esercito.Unità.Guerriero_1.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Guerrieri[1] * Esercito.Unità.Guerriero_2.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Guerrieri[2] * Esercito.Unità.Guerriero_3.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Guerrieri[3] * Esercito.Unità.Guerriero_4.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Guerrieri[4] * Esercito.Unità.Guerriero_5.Trasporto * moltiplicatore);

            capacitàCarico += (int)(playerUnits.Lancieri[0] * Esercito.Unità.Lancere_1.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Lancieri[1] * Esercito.Unità.Lancere_2.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Lancieri[2] * Esercito.Unità.Lancere_3.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Lancieri[3] * Esercito.Unità.Lancere_4.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Lancieri[4] * Esercito.Unità.Lancere_5.Trasporto * moltiplicatore);

            capacitàCarico += (int)(playerUnits.Arcieri[0] * Esercito.Unità.Arcere_1.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Arcieri[1] * Esercito.Unità.Arcere_2.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Arcieri[2] * Esercito.Unità.Arcere_3.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Arcieri[3] * Esercito.Unità.Arcere_4.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Arcieri[4] * Esercito.Unità.Arcere_5.Trasporto * moltiplicatore);

            capacitàCarico += (int)(playerUnits.Catapulte[0] * Esercito.Unità.Catapulta_1.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Catapulte[1] * Esercito.Unità.Catapulta_2.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Catapulte[2] * Esercito.Unità.Catapulta_3.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Catapulte[3] * Esercito.Unità.Catapulta_4.Trasporto * moltiplicatore);
            capacitàCarico += (int)(playerUnits.Catapulte[4] * Esercito.Unità.Catapulta_5.Trasporto * moltiplicatore);

            return (int)(capacitàCarico * (1 + player.Bonus_Capacità_Trasporto));
        }

        public static RisorseRaccolte RaccoliRisorseEquamente(double capacitàCarico, double cibo, double legno, double pietra, double ferro, double oro, int exp, int diamBlu, int diamViola)
        {
            var risultato = new RisorseRaccolte();
            int tipiRisorse = 0;
            if (cibo > 0) tipiRisorse++;
            if (legno > 0) tipiRisorse++;
            if (pietra > 0) tipiRisorse++;
            if (ferro > 0) tipiRisorse++;
            if (oro > 0) tipiRisorse++;
            if (diamBlu > 0) tipiRisorse++;
            if (diamViola > 0) tipiRisorse++;
            if (tipiRisorse == 0) return risultato;
            double capacitàPerRisorsa = capacitàCarico / tipiRisorse;

            if (cibo > 0) { risultato.Cibo = (int)Math.Min(cibo, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Cibo); capacitàCarico -= risultato.Cibo * Variabili_Server.peso_Risorse_Cibo; }
            if (legno > 0) { risultato.Legno = (int)Math.Min(legno, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Legno); capacitàCarico -= risultato.Legno * Variabili_Server.peso_Risorse_Legno; }
            if (pietra > 0) { risultato.Pietra = (int)Math.Min(pietra, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Pietra); capacitàCarico -= risultato.Pietra * Variabili_Server.peso_Risorse_Pietra; }
            if (ferro > 0) { risultato.Ferro = (int)Math.Min(ferro, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Ferro); capacitàCarico -= risultato.Ferro * Variabili_Server.peso_Risorse_Ferro; }
            if (exp > 0) risultato.Esperienza = (int)Math.Min(exp, 0);
            if (oro > 0) { risultato.Oro = (int)Math.Min(oro, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Oro); capacitàCarico -= risultato.Oro * Variabili_Server.peso_Risorse_Oro; }
            if (diamBlu > 0) { risultato.Diamanti_Blu = (int)Math.Min(diamBlu, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Diamante_Blu); capacitàCarico -= risultato.Diamanti_Blu * Variabili_Server.peso_Risorse_Diamante_Blu; }
            if (diamViola > 0) { risultato.Diamanti_Viola = (int)Math.Min(diamViola, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Diamante_Viola); capacitàCarico -= risultato.Diamanti_Viola * Variabili_Server.peso_Risorse_Diamante_Viola; }

            bool haRaccolto = true;
            while (capacitàCarico >= Variabili_Server.peso_Risorse_Cibo && haRaccolto)
            {
                haRaccolto = false;
                if (cibo > risultato.Cibo && capacitàCarico >= Variabili_Server.peso_Risorse_Cibo)
                {
                    int extra = (int)Math.Min(cibo - risultato.Cibo, capacitàCarico / Variabili_Server.peso_Risorse_Cibo);
                    if (extra > 0) { risultato.Cibo += extra; capacitàCarico -= extra * Variabili_Server.peso_Risorse_Cibo; haRaccolto = true; }
                }
                if (legno > risultato.Legno && capacitàCarico >= Variabili_Server.peso_Risorse_Legno)
                {
                    int extra = (int)Math.Min(legno - risultato.Legno, capacitàCarico / Variabili_Server.peso_Risorse_Legno);
                    if (extra > 0) { risultato.Legno += extra; capacitàCarico -= extra * Variabili_Server.peso_Risorse_Legno; haRaccolto = true; }
                }
                if (pietra > risultato.Pietra && capacitàCarico >= Variabili_Server.peso_Risorse_Pietra)
                {
                    int extra = (int)Math.Min(pietra - risultato.Pietra, capacitàCarico / Variabili_Server.peso_Risorse_Pietra);
                    if (extra > 0) { risultato.Pietra += extra; capacitàCarico -= extra * Variabili_Server.peso_Risorse_Pietra; haRaccolto = true; }
                }
                if (ferro > risultato.Ferro && capacitàCarico >= Variabili_Server.peso_Risorse_Ferro)
                {
                    int extra = (int)Math.Min(ferro - risultato.Ferro, capacitàCarico / Variabili_Server.peso_Risorse_Ferro);
                    if (extra > 0) { risultato.Ferro += extra; capacitàCarico -= extra * Variabili_Server.peso_Risorse_Ferro; haRaccolto = true; }
                }
                if (oro > risultato.Oro && capacitàCarico >= Variabili_Server.peso_Risorse_Oro)
                {
                    int extra = (int)Math.Min(oro - risultato.Oro, capacitàCarico / Variabili_Server.peso_Risorse_Oro);
                    if (extra > 0) { risultato.Oro += extra; capacitàCarico -= extra * Variabili_Server.peso_Risorse_Oro; haRaccolto = true; }
                }
                if (diamBlu > risultato.Diamanti_Blu && capacitàCarico >= Variabili_Server.peso_Risorse_Diamante_Blu)
                {
                    int extra = (int)Math.Min(diamBlu - risultato.Diamanti_Blu, capacitàCarico / Variabili_Server.peso_Risorse_Diamante_Blu);
                    if (extra > 0) { risultato.Diamanti_Blu += extra; capacitàCarico -= extra * Variabili_Server.peso_Risorse_Diamante_Blu; haRaccolto = true; }
                }
                if (diamViola > risultato.Diamanti_Viola && capacitàCarico >= Variabili_Server.peso_Risorse_Diamante_Viola)
                {
                    int extra = (int)Math.Min(diamViola - risultato.Diamanti_Viola, capacitàCarico / Variabili_Server.peso_Risorse_Diamante_Viola);
                    if (extra > 0) { risultato.Diamanti_Viola += extra; capacitàCarico -= extra * Variabili_Server.peso_Risorse_Diamante_Viola; haRaccolto = true; }
                }
            }
            return risultato;
        }

        //Spionaggio
        public class RisultatoSpionaggio
        {
            public string Tipo_Battaglia { get; set; } = "";
            public bool Spionaggio_Riuscito { get; set; }
            public int Forza_Spionaggio { get; set; }
            public int Stadio { get; set; }
            // true quando la Precisione (derivata dalla Forza, soglia 900) non basta a dare valori esatti
            // in almeno una categoria già sbloccata dallo Stadio — il client la usa per mostrare un'unica
            // nota "aumenta la forza per migliorare la precisione" nel report (14/09/2026).
            public bool Precisione_Insufficiente { get; set; }
            public DatiGiocatore Giocatore { get; set; } = new DatiGiocatore();
            public RisorseCivili Risorse_Civili { get; set; } = new RisorseCivili();
            public RisorseMilitari Risorse_Militari { get; set; } = new RisorseMilitari();
            public RisorseSpeciali Risorse_Speciali { get; set; } = new RisorseSpeciali();
            public Edifici_Civili Strutture_Civili { get; set; } = new Edifici_Civili();
            public Workshop Workshop { get; set; } = new Workshop();
            public Caserme Caserme { get; set; } = new Caserme();
            public StatsUnità Stats_Unità { get; set; } = new StatsUnità();
            public List<SpionaggioFase> Fasi { get; set; } = new List<SpionaggioFase>();
            public RicercaCivile Ricerca_Civile { get; set; } = new RicercaCivile();
            public RicercaMilitare Ricerca_Militare { get; set; } = new RicercaMilitare();
            public Bonus Bonus { get; set; } = new Bonus();
        }
        public class RisorseCivili
        {
            public int Cibo { get; set; }
            public int Legno { get; set; }
            public int Pietra { get; set; }
            public int Ferro { get; set; }
            public int Oro { get; set; }
            public int Popolazione { get; set; }
        }
        public class RisorseMilitari
        {
            public int Spade { get; set; }
            public int Lance { get; set; }
            public int Archi { get; set; }
            public int Scudi { get; set; }
            public int Armature { get; set; }
            public int Frecce { get; set; }
        }
        public class RisorseSpeciali
        {
            public int Diamanti_Blu { get; set; }
            public int Diamanti_Viola { get; set; }
        }
        public class DatiGiocatore
        {
            public string Nome { get; set; } = "";
            public int Forza_Attaccante { get; set; }
            public int Livello { get; set; }
            public int Esperienza { get; set; }
        }
        public class Edifici_Civili
        {
            public TripleValue Fattoria { get; set; } = new TripleValue();
            public TripleValue Segheria { get; set; } = new TripleValue();
            public TripleValue Cava { get; set; } = new TripleValue();
            public TripleValue Miniera_Ferrro { get; set; } = new TripleValue();
            public TripleValue Miniera_Oro { get; set; } = new TripleValue();
            public TripleValue Abitazioni { get; set; } = new TripleValue();
        }
        public class Workshop
        {
            public TripleValue Spade { get; set; } = new TripleValue();
            public TripleValue Lance { get; set; } = new TripleValue();
            public TripleValue Archi { get; set; } = new TripleValue();
            public TripleValue Scudi { get; set; } = new TripleValue();
            public TripleValue Armature { get; set; } = new TripleValue();
            public TripleValue Frecce { get; set; } = new TripleValue();
        }
        public class Caserme
        {
            public TripleValue Guerrieri { get; set; } = new TripleValue();
            public TripleValue Lanceri { get; set; } = new TripleValue();
            public TripleValue Arcieri { get; set; } = new TripleValue();
            public TripleValue Catapulte { get; set; } = new TripleValue();
        }
        public class TripleValue
        {
            public int Min { get; set; }
            public int Reale { get; set; }
            public int Max { get; set; }
        }
        public class SpionaggioFase
        {
            public SpionaggioVillaggio Struttura { get; set; } = new SpionaggioVillaggio();
            public TripleValue[] Guerrieri { get; set; } = Enumerable.Range(0, 5).Select(_ => new TripleValue()).ToArray();
            public TripleValue[] Lanceri { get; set; } = Enumerable.Range(0, 5).Select(_ => new TripleValue()).ToArray();
            public TripleValue[] Arcieri { get; set; } = Enumerable.Range(0, 5).Select(_ => new TripleValue()).ToArray();
            public TripleValue[] Catapulte { get; set; } = Enumerable.Range(0, 5).Select(_ => new TripleValue()).ToArray();
        }
        public class TipiStatistiche
        {
            public double Salute { get; set; } = new double();
            public double Difesa { get; set; } = new double();
            public double Attacco { get; set; } = new double();
            public double Livello { get; set; } = new double();

        }
        public class StatsUnità
        {
            public TipiStatistiche[] Guerrieri { get; set; } = Enumerable.Range(0, 5).Select(_ => new TipiStatistiche()).ToArray();
            public TipiStatistiche[] Lanceri { get; set; } = Enumerable.Range(0, 5).Select(_ => new TipiStatistiche()).ToArray();
            public TipiStatistiche[] Arcieri { get; set; } = Enumerable.Range(0, 5).Select(_ => new TipiStatistiche()).ToArray();
            public TipiStatistiche[] Catapulte { get; set; } = Enumerable.Range(0, 5).Select(_ => new TipiStatistiche()).ToArray();
        }
        public class SpionaggioVillaggio
        {
            public string Nome { get; set; } = "";
            public int Salute { get; set; }
            public int SaluteMin { get; set; }
            public int SaluteMax { get; set; }
            public int Difesa { get; set; }
            public int DifesaMin { get; set; }
            public int DifesaMax { get; set; }
            public int Ricerca_Salute { get; set; }
            public int Ricerca_Difesa { get; set; }
            public int Ricerca_Guarnigione { get; set; }
            public int Ricerca_Livello { get; set; }
            public TripleValue Guarnigione { get; set; } = new TripleValue();
            public int Guarnigione_Max { get; set; }
        }
        public class RicercaCivile
        {
            public int Produzione { get; set; }
            public int Costruzione { get; set; }
            public int Addestramento { get; set; }
            public int Popolazione { get; set; }
            public int Trasporto { get; set; }
            public int Riparazione { get; set; }
            public int Spionaggio { get; set; }
            public int Contro_Spionaggio { get; set; }

        }
        public class RicercaMilitare
        {
            public TipiStatistiche Guerrieri { get; set; } = new TipiStatistiche();
            public TipiStatistiche Lanceri { get; set; } = new TipiStatistiche();
            public TipiStatistiche Arcieri { get; set; } = new TipiStatistiche();
            public TipiStatistiche Catapulte { get; set; } = new TipiStatistiche();

        }
        public class Bonus
        {
            public TipiStatistiche Guerrieri { get; set; } = new TipiStatistiche();
            public TipiStatistiche Lanceri { get; set; } = new TipiStatistiche();
            public TipiStatistiche Arceri { get; set; } = new TipiStatistiche();
            public TipiStatistiche Catapulte { get; set; } = new TipiStatistiche();

            public double Salute_Strutture { get; set; }
            public double Difesa_Strutture { get; set; }
            public double Guarnigione_Strutture { get; set; }

            public double Produzione_Risorse { get; set; }
            public double Costruzione { get; set; }
            public double Addestramento { get; set; }
            public double Capacità_Trasporto { get; set; }
            public double Ricerca { get; set; }
            public double Riparazione { get; set; }
            public double Spionaggio { get; set; }
            public double Contro_Spionaggio { get; set; }
        }
    }
}
