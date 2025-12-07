using Server_Strategico.Gioco;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Server_Strategico.Gioco.Giocatori;

public class BattaglieV2
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
    } // Classe per rappresentare le unità militari
    public class BattleResult // Classe per i risultati della battaglia
    {
        public UnitGroup PlayerSurvivors { get; set; }
        public UnitGroup PlayerCasualties { get; set; }
        public UnitGroup EnemySurvivors { get; set; }
        public UnitGroup EnemyCasualties { get; set; }
        public double PlayerDamage { get; set; }
        public double EnemyDamage { get; set; }
        public int ExperienceGained { get; set; }
        public bool Victory { get; set; }
        public RangedBattleResult RangedPhase { get; set; }  // ✨ Risultati pre-battaglia
    }
    public class RangedBattleResult
{
    public UnitGroup PlayerUnitsAfter { get; set; }
    public UnitGroup EnemyUnitsAfter { get; set; }
    
    public int[] PlayerKills_Guerrieri { get; set; }  // Array di 5 elementi
    public int[] PlayerKills_Lancieri { get; set; }    // Array di 5 elementi
    public int[] EnemyKills_Guerrieri { get; set; }    // Array di 5 elementi
    public int[] EnemyKills_Lancieri { get; set; }     // Array di 5 elementi
    
    public int PlayerArrowsUsed { get; set; }
    public int EnemyArrowsUsed { get; set; }
    public int ExperienceGained { get; set; }
    public bool PlayerHadLowArrows { get; set; }
    public bool EnemyHadLowArrows { get; set; }

    public RangedBattleResult()
    {
        PlayerKills_Guerrieri = new int[5];
        PlayerKills_Lancieri = new int[5];
        EnemyKills_Guerrieri = new int[5];
        EnemyKills_Lancieri = new int[5];
    }

    // Helper per ottenere totali (utile per log)
    public int GetTotalPlayerKills() => PlayerKills_Guerrieri.Sum() + PlayerKills_Lancieri.Sum();
    public int GetTotalEnemyKills() => EnemyKills_Guerrieri.Sum() + EnemyKills_Lancieri.Sum();
}
    private class RangedAttackResult
    {
        public int DannoGuerrieri { get; set; }
        public int DannoLancieri { get; set; }

        // ✨ NUOVO: Traccia morti per livello
        public int[] MortiGuerrieri { get; set; }  // Array di 5 elementi
        public int[] MortiLancieri { get; set; }   // Array di 5 elementi

        public int FrecceUsate { get; set; }
        public bool FrecceInsufficienti { get; set; }

        public RangedAttackResult()
        {
            MortiGuerrieri = new int[5];
            MortiLancieri = new int[5];
        }
    }
    public class UnitStats // Struttura per le statistiche delle unità
    {
        public double Attacco { get; set; }
        public double Difesa { get; set; }
        public double Salute { get; set; }
        public int Esperienza { get; set; }
        public int ComponenteLancio { get; set; }
    }

    public static async Task<BattleResult> Battaglia_Barbari( Player player, Guid clientGuid, string tipo, string livello, int[] guerriero, int[] lancere, int[] arcere, int[] catapulte, bool includiPreBattaglia = true)  // ✨ Nuovo parametro
    {
        int liv = Convert.ToInt32(livello);
        int perdite_Giocatore = 0, perdite_Nemico = 0;
        var playerUnits = new UnitGroup // Prepara le unità
        {
            Guerrieri = guerriero,
            Lancieri = lancere,
            Arcieri = arcere,
            Catapulte = catapulte
        };
        var enemyUnits = CaricaUnitaNemiche(liv, tipo, player);

        RangedBattleResult rangedResult = null; 
        if (includiPreBattaglia) // FASE 1: Pre-battaglia a distanza (se abilitata)
        {
            rangedResult = EseguiPreBattaglia(playerUnits, enemyUnits, player, clientGuid);

            // Applica le perdite della pre-battaglia
            playerUnits = rangedResult.PlayerUnitsAfter;
            enemyUnits = rangedResult.EnemyUnitsAfter;
        }

        var result = EseguiBattaglia(playerUnits, enemyUnits, player, clientGuid, liv); //  FASE 2: Battaglia corpo a corpo
        if (result != null) // Aggiungi i dati della pre-battaglia al risultato finale
        {
            result.RangedPhase = rangedResult;
            result.ExperienceGained += rangedResult.ExperienceGained;
        }

        for (int i = 0; i < player.Guerrieri.Count(); i++) // Aggiorna lo stato del giocatore
        {
            // Aggiorna lo stato del giocatore
            player.Guerrieri[i] -= result.PlayerCasualties.Guerrieri[i] + result.RangedPhase.EnemyKills_Guerrieri[i];
            player.Lanceri[i] -= result.PlayerCasualties.Lancieri[i] + result.RangedPhase.EnemyKills_Lancieri[i];
            player.Arceri[i] -= result.PlayerCasualties.Arcieri[i];
            player.Catapulte[i] -= result.PlayerCasualties.Catapulte[i];

            //Statistiche - Attacco Distanza
            perdite_Giocatore += result.RangedPhase.EnemyKills_Guerrieri[i];
            perdite_Giocatore += result.RangedPhase.EnemyKills_Lancieri[i];
            perdite_Nemico += result.RangedPhase.PlayerKills_Guerrieri[i];
            perdite_Nemico += result.RangedPhase.PlayerKills_Lancieri[i];

            player.Guerrieri_Eliminate += result.RangedPhase.PlayerKills_Guerrieri[i];
            player.Lanceri_Eliminate += result.RangedPhase.PlayerKills_Lancieri[i];
            player.Guerrieri_Persi += result.RangedPhase.EnemyKills_Guerrieri[i];
            player.Lanceri_Persi += result.RangedPhase.EnemyKills_Lancieri[i];

            player.Unità_Eliminate += result.RangedPhase.PlayerKills_Guerrieri[i];
            player.Unità_Eliminate += result.RangedPhase.PlayerKills_Lancieri[i];
            player.Unità_Perse += result.RangedPhase.EnemyKills_Guerrieri[i];
            player.Unità_Perse += result.RangedPhase.EnemyKills_Lancieri[i];

            //Corpo a corpo
            perdite_Giocatore += result.EnemyCasualties.Guerrieri[i];
            perdite_Giocatore += result.EnemyCasualties.Lancieri[i];
            perdite_Giocatore += result.EnemyCasualties.Arcieri[i];
            perdite_Giocatore += result.EnemyCasualties.Catapulte[i];

            perdite_Nemico += result.PlayerCasualties.Guerrieri[i];
            perdite_Nemico += result.PlayerCasualties.Lancieri[i];
            perdite_Nemico += result.PlayerCasualties.Arcieri[i];
            perdite_Nemico += result.PlayerCasualties.Catapulte[i];

            player.Guerrieri_Eliminate += result.EnemyCasualties.Guerrieri[i];
            player.Lanceri_Eliminate += result.EnemyCasualties.Lancieri[i];
            player.Arceri_Eliminate += result.EnemyCasualties.Arcieri[i];
            player.Catapulte_Eliminate += result.EnemyCasualties.Catapulte[i];

            player.Barbari_Sconfitti += result.EnemyCasualties.Guerrieri[i];
            player.Barbari_Sconfitti += result.EnemyCasualties.Lancieri[i];
            player.Barbari_Sconfitti += result.EnemyCasualties.Arcieri[i];
            player.Barbari_Sconfitti += result.EnemyCasualties.Catapulte[i];

            player.Unità_Perse += result.PlayerCasualties.Guerrieri[i];
            player.Unità_Perse += result.PlayerCasualties.Lancieri[i];
            player.Unità_Perse += result.PlayerCasualties.Arcieri[i];
            player.Unità_Perse += result.PlayerCasualties.Catapulte[i];

            player.Guerrieri_Persi += result.PlayerCasualties.Guerrieri[i];
            player.Lanceri_Persi += result.PlayerCasualties.Lancieri[i];
            player.Arceri_Persi += result.PlayerCasualties.Arcieri[i];
            player.Catapulte_Perse += result.PlayerCasualties.Catapulte[i];
        }
        player.Frecce_Utilizzate += result.RangedPhase.PlayerArrowsUsed; //Statistiche giocatore
        player.Esperienza += result.ExperienceGained;

        if (perdite_Giocatore < perdite_Nemico) // Vittoria del giocatore se le sue perdite sono inferiori a quelle del nemico
        {
            SendClient(clientGuid, "Log_Server|Il nemico si è ritirato prima del previsto");
            //Log "Il nemico si è ritirato"
            result.Victory = true;
        }

        if (result.Victory)
        {
            player.Battaglie_Vinte++;
            if (tipo == "Villaggio Barbaro") player.Accampamenti_Barbari_Sconfitti++;
            if (tipo == "Città Barbaro") player.Città_Barbare_Sconfitte++;
            AssegnaRisorseVittoria(player, clientGuid, tipo, liv, result.PlayerSurvivors); // Assegna risorse
        }
        else player.Battaglie_Perse++;

        AggiornaBarbari(liv, tipo, player, result.EnemySurvivors); // Aggiorna i barbari
        InviaLogBattaglia(clientGuid, player, result, tipo, playerUnits, enemyUnits); // Invia i log
        return result;
    }
    private static UnitGroup CaricaUnitaNemiche(int livello, string tipo, Player player)
    {
        var citta = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == 1);
        var villaggio = player.VillaggiPersonali[livello - 1];

        if (citta == null) return new UnitGroup();
        if (villaggio == null) return new UnitGroup();

        var units = new UnitGroup();
        int tierIndex = GetTierIndex(livello);

        if (tipo == "Città Barbaro")
            if (tierIndex >= 0 && tierIndex < 5)
            {
                units.Guerrieri[tierIndex] = citta.Guerrieri;
                units.Lancieri[tierIndex] = citta.Lancieri;
                units.Arcieri[tierIndex] = citta.Arcieri;
                units.Catapulte[tierIndex] = citta.Catapulte;
            }
        
        if (tipo == "Villaggio Barbaro")
            if (tierIndex >= 0 && tierIndex < 5)
            {
                units.Guerrieri[tierIndex] = villaggio.Guerrieri;
                units.Lancieri[tierIndex] = villaggio.Lancieri;
                units.Arcieri[tierIndex] = villaggio.Arcieri;
                units.Catapulte[tierIndex] = villaggio.Catapulte;
            }
        return units;
    }
    private static int GetTierIndex(int livello)
    {
        if (livello >= 1 && livello <= 4) return 0;
        if (livello <= 8) return 1;
        if (livello <= 12) return 2;
        if (livello <= 16) return 3;
        if (livello <= 20) return 4;
        return 0;
    }
    private static void AssegnaRisorseVittoria(Player player, Guid clientGuid, string tipo, int livello, UnitGroup sopravvissuti)
    {
        int capacitàCarico = CapacitàCarico(sopravvissuti);
        int capacitàOriginale = capacitàCarico;

        // Ottieni le risorse disponibili
        int cibo = 0, legno = 0, pietra = 0, ferro = 0, oro = 0, exp = 0, diamBlu = 0, diamViola = 0;

        if (tipo == "Città Barbaro")
        {
            var citta = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == livello);
            if (citta == null)
            {
                SendClient(clientGuid, "Log_Server|[ERRORE] Città barbaro non trovata!");
                return;
            }

            cibo = citta.Cibo;
            legno = citta.Legno;
            pietra = citta.Pietra;
            ferro = citta.Ferro;
            oro = citta.Oro;
            exp = citta.Esperienza;
            diamBlu = citta.Diamanti_Blu;
            diamViola = citta.Diamanti_Viola;
        }
        else if (tipo == "Villaggio Barbaro")
        {
            var villaggio = player.VillaggiPersonali[livello - 1];
            if (villaggio == null)
            {
                SendClient(clientGuid, "Log_Server|[ERRORE] Villaggio barbaro non trovato!");
                return;
            }

            cibo = villaggio.Cibo;
            legno = villaggio.Legno;
            pietra = villaggio.Pietra;
            ferro = villaggio.Ferro;
            oro = villaggio.Oro;
            exp = villaggio.Esperienza;
            diamBlu = villaggio.Diamanti_Blu;
            diamViola = villaggio.Diamanti_Viola;
        }

        // ⭐ RACCOGLI LE RISORSE
        var raccolte = RaccoliRisorseEquamente(capacitàCarico, cibo, legno, pietra, ferro, oro, exp, diamBlu, diamViola);

        // Assegna al giocatore
        player.Cibo += raccolte.Cibo;
        player.Legno += raccolte.Legno;
        player.Pietra += raccolte.Pietra;
        player.Ferro += raccolte.Ferro;
        player.Oro += raccolte.Oro;
        player.Esperienza += exp;
        player.Diamanti_Blu += raccolte.Diamanti_Blu;
        player.Diamanti_Viola += raccolte.Diamanti_Viola;

        // Rimuovi dalle risorse originali
        if (tipo == "Città Barbaro")
        {
            var citta = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == livello);
            citta.Cibo -= raccolte.Cibo;
            citta.Legno -= raccolte.Legno;
            citta.Pietra -= raccolte.Pietra;
            citta.Ferro -= raccolte.Ferro;
            citta.Oro -= raccolte.Oro;
            citta.Esperienza -= exp;
            citta.Diamanti_Blu -= raccolte.Diamanti_Blu;
            citta.Diamanti_Viola -= raccolte.Diamanti_Viola;
        }
        else if (tipo == "Villaggio Barbaro")
        {
            var villaggio = player.VillaggiPersonali[livello - 1];
            villaggio.Cibo -= raccolte.Cibo;
            villaggio.Legno -= raccolte.Legno;
            villaggio.Pietra -= raccolte.Pietra;
            villaggio.Ferro -= raccolte.Ferro;
            villaggio.Oro -= raccolte.Oro;
            villaggio.Esperienza -= exp;
            villaggio.Diamanti_Blu -= raccolte.Diamanti_Blu;
            villaggio.Diamanti_Viola -= raccolte.Diamanti_Viola;
        }

        // Calcola peso utilizzato
        int pesoUtilizzato = 
            raccolte.Cibo * Variabili_Server.peso_Risorse_Civile + 
            raccolte.Legno * Variabili_Server.peso_Risorse_Civile + 
            raccolte.Pietra * Variabili_Server.peso_Risorse_Civile +               
            raccolte.Ferro * Variabili_Server.peso_Risorse_Ferro + 
            raccolte.Oro * Variabili_Server.peso_Risorse_Oro +           
            raccolte.Diamanti_Blu * Variabili_Server.peso_Risorse_Diamante_Blu +
            raccolte.Diamanti_Viola * Variabili_Server.peso_Risorse_Diamante_Viola;

        // Log dettagliato
        SendClient(clientGuid, "Log_Server|");
        SendClient(clientGuid, "Log_Server|╔══════════════════════════════════════════════════╗");
        SendClient(clientGuid, $"Log_Server|║  {(tipo == "Città Barbaro" ? "CITTÀ" : "VILLAGGIO")} CONQUISTATA - RISORSE RACCOLTE        ║");
        SendClient(clientGuid, "Log_Server|╚══════════════════════════════════════════════════╝");
        SendClient(clientGuid, $"Log_Server|Capacità di carico: {capacitàOriginale:N0}");
        SendClient(clientGuid, $"Log_Server|Capacità utilizzata: {pesoUtilizzato:N0}");
        SendClient(clientGuid, "Log_Server|");

        if (raccolte.Cibo > 0) SendClient(clientGuid, $"Log_Server|  Cibo:           +{raccolte.Cibo:N0} (peso: {raccolte.Cibo * Variabili_Server.peso_Risorse_Civile})");
        if (raccolte.Legno > 0) SendClient(clientGuid, $"Log_Server|  Legno:          +{raccolte.Legno:N0} (peso: {raccolte.Legno * Variabili_Server.peso_Risorse_Civile})");
        if (raccolte.Pietra > 0) SendClient(clientGuid, $"Log_Server|  Pietra:         +{raccolte.Pietra:N0} (peso: {raccolte.Pietra * Variabili_Server.peso_Risorse_Civile})");
        if (raccolte.Ferro > 0) SendClient(clientGuid, $"Log_Server|  Ferro:          +{raccolte.Ferro:N0} (peso: {raccolte.Ferro * Variabili_Server.peso_Risorse_Ferro})");
        if (raccolte.Oro > 0) SendClient(clientGuid, $"Log_Server|  Oro:            +{raccolte.Oro:N0} (peso: {raccolte.Oro * Variabili_Server.peso_Risorse_Oro})");
        if (raccolte.Diamanti_Blu > 0) SendClient(clientGuid, $"Log_Server|  Diamanti Blu:   +{raccolte.Diamanti_Blu:N0} (peso: {raccolte.Diamanti_Blu * Variabili_Server.peso_Risorse_Diamante_Blu})");
        if (raccolte.Diamanti_Viola > 0) SendClient(clientGuid, $"Log_Server|  Diamanti Viola: +{raccolte.Diamanti_Viola:N0} (peso: {raccolte.Diamanti_Viola * Variabili_Server.peso_Risorse_Diamante_Viola})");

        SendClient(clientGuid, "Log_Server|════════════════════════════════════════════════════\n");
    }
    static int CapacitàCarico(UnitGroup playerUnits)
    {
        int capacitàCarico = 0;
        capacitàCarico += playerUnits.Guerrieri[0] * Esercito.Unità.Guerriero_1.Trasporto;
        capacitàCarico += playerUnits.Guerrieri[1] * Esercito.Unità.Guerriero_2.Trasporto;
        capacitàCarico += playerUnits.Guerrieri[2] * Esercito.Unità.Guerriero_3.Trasporto;
        capacitàCarico += playerUnits.Guerrieri[3] * Esercito.Unità.Guerriero_4.Trasporto;
        capacitàCarico += playerUnits.Guerrieri[4] * Esercito.Unità.Guerriero_5.Trasporto;

        capacitàCarico += playerUnits.Lancieri[0] * Esercito.Unità.Lancere_1.Trasporto;
        capacitàCarico += playerUnits.Lancieri[1] * Esercito.Unità.Lancere_2.Trasporto;
        capacitàCarico += playerUnits.Lancieri[2] * Esercito.Unità.Lancere_3.Trasporto;
        capacitàCarico += playerUnits.Lancieri[3] * Esercito.Unità.Lancere_4.Trasporto;
        capacitàCarico += playerUnits.Lancieri[4] * Esercito.Unità.Lancere_5.Trasporto;

        capacitàCarico += playerUnits.Arcieri[0] * Esercito.Unità.Arcere_1.Trasporto;
        capacitàCarico += playerUnits.Arcieri[1] * Esercito.Unità.Arcere_2.Trasporto;
        capacitàCarico += playerUnits.Arcieri[2] * Esercito.Unità.Arcere_3.Trasporto;
        capacitàCarico += playerUnits.Arcieri[3] * Esercito.Unità.Arcere_4.Trasporto;
        capacitàCarico += playerUnits.Arcieri[4] * Esercito.Unità.Arcere_5.Trasporto;

        capacitàCarico += playerUnits.Catapulte[0] * Esercito.Unità.Catapulta_1.Trasporto;
        capacitàCarico += playerUnits.Catapulte[1] * Esercito.Unità.Catapulta_2.Trasporto;
        capacitàCarico += playerUnits.Catapulte[2] * Esercito.Unità.Catapulta_3.Trasporto;
        capacitàCarico += playerUnits.Catapulte[3] * Esercito.Unità.Catapulta_4.Trasporto;
        capacitàCarico += playerUnits.Catapulte[4] * Esercito.Unità.Catapulta_5.Trasporto;
        return capacitàCarico;
    }
    
    // Struttura per contenere le risorse raccolte
    private class RisorseRaccolte
    {
        public int Cibo { get; set; }
        public int Legno { get; set; }
        public int Pietra { get; set; }
        public int Ferro { get; set; }
        public int Oro { get; set; }
        public int Esperienza { get; set; }
        public int Diamanti_Blu { get; set; }
        public int Diamanti_Viola { get; set; }
    }

    // ═══════════════════════════════════════════════════════════════
    // FUNZIONE DI RACCOLTA RISORSE - Distribuzione equa
    // ═══════════════════════════════════════════════════════════════
    private static RisorseRaccolte RaccoliRisorseEquamente(double capacitàCarico,
    double cibo, double legno, double pietra, double ferro, double oro, int exp, int diamBlu, int diamViola)
    {
        var risultato = new RisorseRaccolte();

        // Conta quante risorse sono disponibili
        int tipiRisorse = 0;
        if (cibo > 0) tipiRisorse++;
        if (legno > 0) tipiRisorse++;
        if (pietra > 0) tipiRisorse++;
        if (ferro > 0) tipiRisorse++;
        if (oro > 0) tipiRisorse++;
        if (exp > 0) tipiRisorse++;
        if (diamBlu > 0) tipiRisorse++;
        if (diamViola > 0) tipiRisorse++;

        if (tipiRisorse == 0) return risultato;

        // Dividi equamente la capacità tra i tipi di risorse disponibili
        double capacitàPerRisorsa = capacitàCarico / tipiRisorse;

        // FASE 1: Distribuisci equamente (dalle più leggere alle più pesanti)
        if (cibo > 0)
        {
            risultato.Cibo = (int)Math.Min(cibo, capacitàPerRisorsa / 3);
            capacitàCarico -= risultato.Cibo * 3;
        }
        if (legno > 0)
        {
            risultato.Legno = (int)Math.Min(legno, capacitàPerRisorsa / 3);
            capacitàCarico -= risultato.Legno * 3;
        }
        if (pietra > 0)
        {
            risultato.Pietra = (int)Math.Min(pietra, capacitàPerRisorsa / 3);
            capacitàCarico -= risultato.Pietra * 3;
        }
        if (ferro > 0)
        {
            risultato.Ferro = (int)Math.Min(ferro, capacitàPerRisorsa / 6);
            capacitàCarico -= risultato.Ferro * 6;
        }
        if (exp > 0)
        {
            risultato.Esperienza = (int)Math.Min(exp, capacitàPerRisorsa / 9);
            capacitàCarico -= risultato.Esperienza * 9;
        }
        if (oro > 0)
        {
            risultato.Oro = (int)Math.Min(oro, capacitàPerRisorsa / 11);
            capacitàCarico -= risultato.Oro * 11;
        }
        if (diamBlu > 0)
        {
            risultato.Diamanti_Blu = (int)Math.Min(diamBlu, capacitàPerRisorsa / 500);
            capacitàCarico -= risultato.Diamanti_Blu * 500;
        }
        if (diamViola > 0)
        {
            risultato.Diamanti_Viola = (int)Math.Min(diamViola, capacitàPerRisorsa / 1500);
            capacitàCarico -= risultato.Diamanti_Viola * 1500;
        }
        // FASE 2: Cicla finché c'è spazio disponibile e risorse da raccogliere
        bool haRaccolto = true;
        while (capacitàCarico >= Variabili_Server.peso_Risorse_Civile && haRaccolto) // Minimo peso è 3
        {
            haRaccolto = false;

            if (cibo > risultato.Cibo && capacitàCarico >= Variabili_Server.peso_Risorse_Civile)
            {
                int extra = (int)Math.Min(cibo - risultato.Cibo, capacitàCarico / Variabili_Server.peso_Risorse_Civile);
                if (extra > 0)
                {
                    risultato.Cibo += extra;
                    capacitàCarico -= extra * Variabili_Server.peso_Risorse_Civile;
                    haRaccolto = true;
                }
            }
            if (legno > risultato.Legno && capacitàCarico >= Variabili_Server.peso_Risorse_Civile)
            {
                int extra = (int)Math.Min(legno - risultato.Legno, capacitàCarico / Variabili_Server.peso_Risorse_Civile);
                if (extra > 0)
                {
                    risultato.Legno += extra;
                    capacitàCarico -= extra * Variabili_Server.peso_Risorse_Civile;
                    haRaccolto = true;
                }
            }
            if (pietra > risultato.Pietra && capacitàCarico >= Variabili_Server.peso_Risorse_Civile)
            {
                int extra = (int)Math.Min(pietra - risultato.Pietra, capacitàCarico / Variabili_Server.peso_Risorse_Civile);
                if (extra > 0)
                {
                    risultato.Pietra += extra;
                    capacitàCarico -= extra * Variabili_Server.peso_Risorse_Civile;
                    haRaccolto = true;
                }
            }
            if (ferro > risultato.Ferro && capacitàCarico >= Variabili_Server.peso_Risorse_Ferro)
            {
                int extra = (int)Math.Min(ferro - risultato.Ferro, capacitàCarico / Variabili_Server.peso_Risorse_Ferro);
                if (extra > 0)
                {
                    risultato.Ferro += extra;
                    capacitàCarico -= extra * Variabili_Server.peso_Risorse_Ferro;
                    haRaccolto = true;
                }
            }
            if (oro > risultato.Oro && capacitàCarico >= Variabili_Server.peso_Risorse_Oro)
            {
                int extra = (int)Math.Min(oro - risultato.Oro, capacitàCarico / Variabili_Server.peso_Risorse_Oro);
                if (extra > 0)
                {
                    risultato.Oro += extra;
                    capacitàCarico -= extra * Variabili_Server.peso_Risorse_Oro;
                    haRaccolto = true;
                }
            }
            if (diamBlu > risultato.Diamanti_Blu && capacitàCarico >= Variabili_Server.peso_Risorse_Diamante_Blu)
            {
                int extra = (int)Math.Min(diamBlu - risultato.Diamanti_Blu, capacitàCarico / Variabili_Server.peso_Risorse_Diamante_Blu);
                if (extra > 0)
                {
                    risultato.Diamanti_Blu += extra;
                    capacitàCarico -= extra * Variabili_Server.peso_Risorse_Diamante_Blu;
                    haRaccolto = true;
                }
            }
            if (diamViola > risultato.Diamanti_Viola && capacitàCarico >= Variabili_Server.peso_Risorse_Diamante_Viola)
            {
                int extra = (int)Math.Min(diamViola - risultato.Diamanti_Viola, capacitàCarico / Variabili_Server.peso_Risorse_Diamante_Viola);
                if (extra > 0)
                {
                    risultato.Diamanti_Viola += extra;
                    capacitàCarico -= extra * Variabili_Server.peso_Risorse_Diamante_Viola;
                    haRaccolto = true;
                }
            }
        }

        return risultato;
    }

    // ═══════════════════════════════════════════════════════════════
    // 🎯 PRE-BATTAGLIA A DISTANZA - Arcieri e Catapulte attaccano per primi
    // ═══════════════════════════════════════════════════════════════
    private static BattleResult EseguiBattaglia(UnitGroup playerUnits, UnitGroup enemyUnits, Player player, Guid clientGuid, int livello)
    {
        var result = new BattleResult
        {
            PlayerSurvivors = playerUnits.Clone(),
            EnemySurvivors = enemyUnits.Clone(),
            PlayerCasualties = new UnitGroup(),
            EnemyCasualties = new UnitGroup()
        };

        // Conta tipi di unità
        int playerUnitTypes = playerUnits.CountUnitTypes();
        int enemyUnitTypes = enemyUnits.CountUnitTypes();

        // Calcola danno
        result.EnemyDamage = CalcolaDannoNemico(enemyUnits);
        result.PlayerDamage = CalcolaDannoGiocatore(playerUnits, player, clientGuid);

        // Distribuisci il danno tra i tipi di unità
        double dannoPerTipoPlayer = result.EnemyDamage / playerUnitTypes;
        double dannoPerTipoEnemy = result.PlayerDamage / enemyUnitTypes;

        ApplicaDanniGiocatore(result, player, dannoPerTipoPlayer); // Applica danni al giocatore
        ApplicaDanniNemico(result, dannoPerTipoEnemy); // Applica danni al nemico
        result.ExperienceGained = CalcolaEsperienza(result.EnemyCasualties); // Calcola esperienza
        result.Victory = result.EnemySurvivors.TotalUnits() == 0; // Determina vittoria
        return result;
    }
    private static RangedBattleResult EseguiPreBattaglia(UnitGroup playerUnits, UnitGroup enemyUnits, Player player, Guid clientGuid)
    {
        var result = new RangedBattleResult
        {
            PlayerUnitsAfter = playerUnits.Clone(),
            EnemyUnitsAfter = enemyUnits.Clone()
        };

        Send(clientGuid, "Log_Server|═══════════════════════════════════════");
        Send(clientGuid, "Log_Server| FASE 1: ATTACCO A DISTANZA");
        Send(clientGuid, "Log_Server|═══════════════════════════════════════\n");

        // ⚔️ ATTACCO DEL GIOCATORE
        var playerRangedAttack = CalcolaAttaccoDistanza(playerUnits, player, clientGuid, true );
        result.PlayerArrowsUsed = playerRangedAttack.FrecceUsate;
        result.PlayerHadLowArrows = playerRangedAttack.FrecceInsufficienti;

        // Applica danni e ottieni morti per livello
        var playerMorti = ApplicaDanniDistanza(result.EnemyUnitsAfter, playerRangedAttack.DannoGuerrieri, playerRangedAttack.DannoLancieri);

        // Estrai i morti per tipo
        Array.Copy(playerMorti, 0, result.PlayerKills_Guerrieri, 0, 5);
        Array.Copy(playerMorti, 5, result.PlayerKills_Lancieri, 0, 5);

        // Log dettagliato per livello
        Send(clientGuid, "Log_Server|Nemici eliminati:");
        for (int i = 0; i < 5; i++)
            if (result.PlayerKills_Guerrieri[i] > 0 || result.PlayerKills_Lancieri[i] > 0)                
                Send(clientGuid, $"Log_Server|Lv{i + 1}: {result.PlayerKills_Guerrieri[i]} guerrieri, {result.PlayerKills_Lancieri[i]} lancieri");
            
        Send(clientGuid, $"Log_Server|TOTALE: {result.GetTotalPlayerKills()} unità\n");

        // ⚔ CONTRATTACCO DEL NEMICO
        int totalEnemyRanged = enemyUnits.Arcieri.Sum() + enemyUnits.Catapulte.Sum();
        if (totalEnemyRanged > 0)
        {
            Send(clientGuid, "Log_Server|Il nemico contrattacca a distanza...");
            var enemyRangedAttack = CalcolaAttaccoDistanzaNemico(result.EnemyUnitsAfter, clientGuid );
            result.EnemyArrowsUsed = enemyRangedAttack.FrecceUsate;

            // Applica danni e ottieni morti per livello
            var enemyMorti = ApplicaDanniDistanza(result.PlayerUnitsAfter, enemyRangedAttack.DannoGuerrieri, enemyRangedAttack.DannoLancieri);

            Array.Copy(enemyMorti, 0, result.EnemyKills_Guerrieri, 0, 5);
            Array.Copy(enemyMorti, 5, result.EnemyKills_Lancieri, 0, 5);

            // Log dettagliato per livello
            Send(clientGuid, "Log_Server|Tue perdite:");
            for (int i = 0; i < 5; i++)
                if (result.EnemyKills_Guerrieri[i] > 0 || result.EnemyKills_Lancieri[i] > 0)
                    Send(clientGuid, $"Log_Server|Lv{i + 1}: {result.EnemyKills_Guerrieri[i]} guerrieri, {result.EnemyKills_Lancieri[i]} lancieri");

            Send(clientGuid, $"Log_Server|TOTALE: {result.GetTotalEnemyKills()} unità");
        }

        // Calcola esperienza con i dati per livello
        result.ExperienceGained = CalcolaEsperienzaDistanza(result);
        Send(clientGuid, "Log_Server|═══════════════════════════════════════");
        Send(clientGuid, $"Log_Server| Esperienza fase a distanza: +{result.ExperienceGained}");
        Send(clientGuid, "Log_Server|═══════════════════════════════════════\n");
        return result;
    }
    private static RangedAttackResult CalcolaAttaccoDistanza(UnitGroup units, Player player, Guid clientGuid, bool isPlayer)
    {
        var result = new RangedAttackResult();
        int totaleArchieri = units.Arcieri.Sum();
        int totaleCatapulte = units.Catapulte.Sum();

        if (totaleArchieri == 0 && totaleCatapulte == 0)
        {
            Send(clientGuid, "Log_Server|Nessuna unità a distanza disponibile.");
            return result;
        }

        // Calcola frecce necessarie
        int frecceNecessarie = totaleArchieri * Esercito.Unità.Arcere_1.Componente_Lancio + totaleCatapulte * Esercito.Unità.Catapulta_1.Componente_Lancio;
        result.FrecceUsate = frecceNecessarie;

        // Calcola potenza d'attacco base
        int arcieriEffettivi = totaleArchieri * 2 / 3;
        int catapulteEffettive = totaleCatapulte * 2 / 3;

        // Bonus per poche unità (sono più precise)
        if (totaleArchieri > 0 && totaleArchieri <= 10) arcieriEffettivi = totaleArchieri * 2;
        if (totaleCatapulte > 0 && totaleCatapulte <= 5) catapulteEffettive = totaleCatapulte * 6;

        if (isPlayer && player.Frecce < frecceNecessarie) // Gestione frecce insufficienti
        {
            result.FrecceInsufficienti = true;
            Send(clientGuid, $"Log_Server|FRECCE INSUFFICIENTI! [{player.Frecce}/{frecceNecessarie}]");
            Send(clientGuid, "Log_Server|Efficacia attacco ridotta al 33%\n");

            arcieriEffettivi /= 3;
            catapulteEffettive /= 3;
            player.Frecce = 0;
        }
        else if (isPlayer)
        {
            player.Frecce -= frecceNecessarie;
            Send(clientGuid, $"Log_Server|Frecce utilizzate: {frecceNecessarie}");
            Send(clientGuid, $"Log_Server|Frecce rimanenti: {player.Frecce}\n");
        }

        int attaccoTotale = (arcieriEffettivi + catapulteEffettive) * 4 / 5; // Calcola danno totale
        if (attaccoTotale > 0)
        {
            // Distribuisci il danno tra guerrieri e lancieri (2/3 al tipo più numeroso)
            result.DannoGuerrieri = attaccoTotale * 2 / 3;
            result.DannoLancieri = attaccoTotale / 3;
        }
        return result;
    }
    private static RangedAttackResult CalcolaAttaccoDistanzaNemico(UnitGroup enemyUnits, Guid clientGuid)
    {
        var result = new RangedAttackResult();

        int totaleArchieri = enemyUnits.Arcieri.Sum();
        int totaleCatapulte = enemyUnits.Catapulte.Sum();

        if (totaleArchieri == 0 && totaleCatapulte == 0)
            return result;

        // I nemici hanno sempre frecce infinite (semplificazione)
        int arcieriEffettivi = totaleArchieri * 2 / 3;
        int catapulteEffettive = totaleCatapulte * 2 / 3;

        // Bonus per poche unità
        if (totaleArchieri > 0 && totaleArchieri <= 10) arcieriEffettivi = totaleArchieri * 2;
        if (totaleCatapulte > 0 && totaleCatapulte <= 5) catapulteEffettive = totaleCatapulte * 6;

        int attaccoTotale = (arcieriEffettivi + catapulteEffettive) * 4 / 5;
        if (attaccoTotale > 0)
        {
            result.DannoGuerrieri = attaccoTotale * 2 / 3;
            result.DannoLancieri = attaccoTotale / 3;
        }
        result.FrecceUsate = totaleArchieri * Esercito.Unità.Arcere_1.Componente_Lancio + totaleCatapulte * Esercito.Unità.Catapulta_1.Componente_Lancio;
        return result;
    }
    private static int[] ApplicaDanniDistanza(UnitGroup units, int dannoGuerrieri, int dannoLancieri)
    {
        int[] mortiGuerrieri = new int[5];
        int[] mortiLancieri = new int[5];

        int dannoRimanente = dannoGuerrieri;
        for (int i = 0; i < 5 && dannoRimanente > 0; i++) // Applica danni ai guerrieri (dal livello più basso al più alto)
        {
            if (units.Guerrieri[i] > 0)
            {
                int morti = Math.Min(units.Guerrieri[i], dannoRimanente);
                units.Guerrieri[i] -= morti;
                mortiGuerrieri[i] = morti;
                dannoRimanente -= morti;
            }
        }

        dannoRimanente = dannoLancieri;
        for (int i = 0; i < 5 && dannoRimanente > 0; i++) // Applica danni ai lancieri (dal livello più basso al più alto)
        {
            if (units.Lancieri[i] > 0)
            {
                int morti = Math.Min(units.Lancieri[i], dannoRimanente);
                units.Lancieri[i] -= morti;
                mortiLancieri[i] = morti;
                dannoRimanente -= morti;
            }
        }

        // ✨ Restituisce un array concatenato: [5 guerrieri, 5 lancieri]
        int[] tuttiIMorti = new int[10];
        Array.Copy(mortiGuerrieri, 0, tuttiIMorti, 0, 5);
        Array.Copy(mortiLancieri, 0, tuttiIMorti, 5, 5);

        return tuttiIMorti;
    }
    private static int CalcolaEsperienzaDistanza(RangedBattleResult result)
    {
        int exp = 0;
        for (int i = 0; i < 5; i++)
        {
            var stats = GetEnemyUnitStats(i);
            exp += result.PlayerKills_Guerrieri[i] * stats.GuerrieriEsperienza;
            exp += result.PlayerKills_Lancieri[i] * stats.LancieriEsperienza;
        }
        return exp;
    }
    private static void ApplicaDanniGiocatore( BattleResult result, Player player, double dannoPerTipo)
    {
        for (int i = 0; i < 5; i++)
        {
            var stats = GetPlayerUnitStats(i, player);

            // Guerrieri
            int guerrieriIniziali = result.PlayerSurvivors.Guerrieri[i];
            result.PlayerSurvivors.Guerrieri[i] = RidurreNumeroSoldati(guerrieriIniziali, dannoPerTipo, stats.GuerrieriDifesa * guerrieriIniziali, stats.GuerrieriSalute);
            result.PlayerCasualties.Guerrieri[i] = guerrieriIniziali - result.PlayerSurvivors.Guerrieri[i];

            // Lancieri
            int lancieriIniziali = result.PlayerSurvivors.Lancieri[i];
            result.PlayerSurvivors.Lancieri[i] = RidurreNumeroSoldati( lancieriIniziali, dannoPerTipo, stats.LancieriDifesa * lancieriIniziali, stats.LancieriSalute);
            result.PlayerCasualties.Lancieri[i] = lancieriIniziali - result.PlayerSurvivors.Lancieri[i];

            // Arcieri
            int arcieriIniziali = result.PlayerSurvivors.Arcieri[i];
            result.PlayerSurvivors.Arcieri[i] = RidurreNumeroSoldati( arcieriIniziali, dannoPerTipo, stats.ArcieriDifesa * arcieriIniziali, stats.ArcieriSalute );
            result.PlayerCasualties.Arcieri[i] = arcieriIniziali - result.PlayerSurvivors.Arcieri[i];

            // Catapulte
            int catapulteIniziali = result.PlayerSurvivors.Catapulte[i];
            result.PlayerSurvivors.Catapulte[i] = RidurreNumeroSoldati( catapulteIniziali, dannoPerTipo, stats.CatapulteDifesa * catapulteIniziali, stats.CatapulteSalute);
            result.PlayerCasualties.Catapulte[i] = catapulteIniziali - result.PlayerSurvivors.Catapulte[i];
        }
    }
    private static void ApplicaDanniNemico(BattleResult result, double dannoPerTipo)
    {
        for (int i = 0; i < 5; i++)
        {
            var stats = GetEnemyUnitStats(i);

            // Guerrieri
            int guerrieriIniziali = result.EnemySurvivors.Guerrieri[i];
            result.EnemySurvivors.Guerrieri[i] = RidurreNumeroSoldati(
                guerrieriIniziali,
                dannoPerTipo,
                stats.GuerrieriDifesa * guerrieriIniziali,
                stats.GuerrieriSalute
            );
            result.EnemyCasualties.Guerrieri[i] = guerrieriIniziali - result.EnemySurvivors.Guerrieri[i];

            // Lancieri
            int lancieriIniziali = result.EnemySurvivors.Lancieri[i];
            result.EnemySurvivors.Lancieri[i] = RidurreNumeroSoldati(
                lancieriIniziali,
                dannoPerTipo,
                stats.LancieriDifesa * lancieriIniziali,
                stats.LancieriSalute
            );
            result.EnemyCasualties.Lancieri[i] = lancieriIniziali - result.EnemySurvivors.Lancieri[i];

            // Arcieri
            int arcieriIniziali = result.EnemySurvivors.Arcieri[i];
            result.EnemySurvivors.Arcieri[i] = RidurreNumeroSoldati(
                arcieriIniziali,
                dannoPerTipo,
                stats.ArcieriDifesa * arcieriIniziali,
                stats.ArcieriSalute
            );
            result.EnemyCasualties.Arcieri[i] = arcieriIniziali - result.EnemySurvivors.Arcieri[i];

            // Catapulte
            int catapulteIniziali = result.EnemySurvivors.Catapulte[i];
            result.EnemySurvivors.Catapulte[i] = RidurreNumeroSoldati(
                catapulteIniziali,
                dannoPerTipo,
                stats.CatapulteDifesa * catapulteIniziali,
                stats.CatapulteSalute
            );
            result.EnemyCasualties.Catapulte[i] = catapulteIniziali - result.EnemySurvivors.Catapulte[i];
        }
    }
    private static double CalcolaDannoGiocatore( UnitGroup units, Player player, Guid clientGuid)
    {
        double dannoTotale = 0;
        for (int i = 0; i < 5; i++)
        {
            var stats = GetPlayerUnitStats(i, player);
            int frecceNecessarie = units.Arcieri[i] * Esercito.Unità.Arcere_1.Componente_Lancio + units.Catapulte[i] * Esercito.Unità.Catapulta_1.Componente_Lancio;

            double moltiplicatoreDistanza = 1.0;
            if (player.Frecce < frecceNecessarie)
            {
                Send(clientGuid, $"Log_Server|Riduzione danno livello {i + 1} per mancanza di frecce [{frecceNecessarie}/{player.Frecce}]");
                moltiplicatoreDistanza = 0.33;
                player.Frecce = 0;
            }
            else
            {
                player.Frecce -= frecceNecessarie;
                Send(clientGuid, $"Log_Server|Frecce utilizzate livello {i + 1}: {frecceNecessarie}");
            }

            dannoTotale += units.Arcieri[i] * stats.ArcieriAttacco * moltiplicatoreDistanza;
            dannoTotale += units.Catapulte[i] * stats.CatapulteAttacco * moltiplicatoreDistanza;
            dannoTotale += units.Guerrieri[i] * stats.GuerrieriAttacco;
            dannoTotale += units.Lancieri[i] * stats.LancieriAttacco;
        }
        return dannoTotale;
    }
    private static double CalcolaDannoNemico(UnitGroup units)
    {
        double dannoTotale = 0;
        for (int i = 0; i < 5; i++)
        {
            var stats = GetEnemyUnitStats(i);

            dannoTotale += units.Arcieri[i] * stats.ArcieriAttacco;
            dannoTotale += units.Catapulte[i] * stats.CatapulteAttacco;
            dannoTotale += units.Guerrieri[i] * stats.GuerrieriAttacco;
            dannoTotale += units.Lancieri[i] * stats.LancieriAttacco;
        }
        return dannoTotale;
    }
    private static int CalcolaEsperienza(UnitGroup casualties)
    {
        int esperienza = 0;
        for (int i = 0; i < 5; i++)
        {
            var stats = GetEnemyUnitStats(i);
            esperienza += casualties.Guerrieri[i] * stats.GuerrieriEsperienza;
            esperienza += casualties.Lancieri[i] * stats.LancieriEsperienza;
            esperienza += casualties.Arcieri[i] * stats.ArcieriEsperienza;
            esperienza += casualties.Catapulte[i] * stats.CatapulteEsperienza;
        }
        return esperienza;
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
    private static int RidurreNumeroSoldati(int numeroSoldati, double danno, double difesa, double salutePerSoldato)
    {
        if (numeroSoldati == 0 || salutePerSoldato == 0) return 0;
        double dannoEffettivo = Math.Max(0, danno - difesa);
        int soldatiPersi = (int)Math.Ceiling(dannoEffettivo / salutePerSoldato);
        return Math.Max(0, numeroSoldati - soldatiPersi);
    }

    // Metodi helper per ottenere le statistiche
    private static (double GuerrieriAttacco, double GuerrieriDifesa, double GuerrieriSalute,
                   double LancieriAttacco, double LancieriDifesa, double LancieriSalute,
                   double ArcieriAttacco, double ArcieriDifesa, double ArcieriSalute,
                   double CatapulteAttacco, double CatapulteDifesa, double CatapulteSalute)
        GetPlayerUnitStats(int level, Player player)
    {

        var baseStats = level switch // Ottieni le statistiche base in base al livello
        {
            0 => (Esercito.Unità.Guerriero_1, Esercito.Unità.Lancere_1, Esercito.Unità.Arcere_1, Esercito.Unità.Catapulta_1),
            1 => (Esercito.Unità.Guerriero_2, Esercito.Unità.Lancere_2, Esercito.Unità.Arcere_2, Esercito.Unità.Catapulta_2),
            2 => (Esercito.Unità.Guerriero_3, Esercito.Unità.Lancere_3, Esercito.Unità.Arcere_3, Esercito.Unità.Catapulta_3),
            3 => (Esercito.Unità.Guerriero_4, Esercito.Unità.Lancere_4, Esercito.Unità.Arcere_4, Esercito.Unità.Catapulta_4),
            4 => (Esercito.Unità.Guerriero_5, Esercito.Unità.Lancere_5, Esercito.Unità.Arcere_5, Esercito.Unità.Catapulta_5),
            _ => (Esercito.Unità.Guerriero_1, Esercito.Unità.Lancere_1, Esercito.Unità.Arcere_1, Esercito.Unità.Catapulta_1)
        };

        return ( // Applica i bonus delle ricerche
            baseStats.Item1.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Guerriero_Attacco,
            baseStats.Item1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa,
            baseStats.Item1.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute,

            baseStats.Item2.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Lancere_Attacco,
            baseStats.Item2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa,
            baseStats.Item2.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute,

            baseStats.Item3.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Arcere_Attacco,
            baseStats.Item3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa,
            baseStats.Item3.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute,

            baseStats.Item4.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Catapulta_Attacco,
            baseStats.Item4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa,
            baseStats.Item4.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute
        );
    }
    private static (double GuerrieriAttacco, double GuerrieriDifesa, double GuerrieriSalute, int GuerrieriEsperienza,
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

    private static (double GuerrieriAttacco, double GuerrieriDifesa, double GuerrieriSalute, int GuerrieriEsperienza,
                   double LancieriAttacco, double LancieriDifesa, double LancieriSalute, int LancieriEsperienza,
                   double ArcieriAttacco, double ArcieriDifesa, double ArcieriSalute, int ArcieriEsperienza,
                   double CatapulteAttacco, double CatapulteDifesa, double CatapulteSalute, int CatapulteEsperienza)
        GetUnitStats(int level)
    {
        return level switch
        {
            0 => (Esercito.Unità.Guerriero_1.Attacco, Esercito.Unità.Guerriero_1.Difesa,
                  Esercito.Unità.Guerriero_1.Salute, Esercito.Unità.Guerriero_1.Esperienza,
                  Esercito.Unità.Lancere_1.Attacco, Esercito.Unità.Lancere_1.Difesa,
                  Esercito.Unità.Lancere_1.Salute, Esercito.Unità.Lancere_1.Esperienza,
                  Esercito.Unità.Arcere_1.Attacco, Esercito.Unità.Arcere_1.Difesa,
                  Esercito.Unità.Arcere_1.Salute, Esercito.Unità.Arcere_1.Esperienza,
                  Esercito.Unità.Catapulta_1.Attacco, Esercito.Unità.Catapulta_1.Difesa,
                  Esercito.Unità.Catapulta_1.Salute, Esercito.Unità.Catapulta_1.Esperienza),

            1 => (Esercito.Unità.Guerriero_2.Attacco, Esercito.Unità.Guerriero_2.Difesa,
                  Esercito.Unità.Guerriero_2.Salute, Esercito.Unità.Guerriero_2.Esperienza,
                  Esercito.Unità.Lancere_2.Attacco, Esercito.Unità.Lancere_2.Difesa,
                  Esercito.Unità.Lancere_2.Salute, Esercito.Unità.Lancere_2.Esperienza,
                  Esercito.Unità.Arcere_2.Attacco, Esercito.Unità.Arcere_2.Difesa,
                  Esercito.Unità.Arcere_2.Salute, Esercito.Unità.Arcere_2.Esperienza,
                  Esercito.Unità.Catapulta_2.Attacco, Esercito.Unità.Catapulta_2.Difesa,
                  Esercito.Unità.Catapulta_2.Salute, Esercito.Unità.Catapulta_2.Esperienza),

            2 => (Esercito.Unità.Guerriero_3.Attacco, Esercito.Unità.Guerriero_3.Difesa,
                  Esercito.Unità.Guerriero_3.Salute, Esercito.Unità.Guerriero_3.Esperienza,
                  Esercito.Unità.Lancere_3.Attacco, Esercito.Unità.Lancere_3.Difesa,
                  Esercito.Unità.Lancere_3.Salute, Esercito.Unità.Lancere_3.Esperienza,
                  Esercito.Unità.Arcere_3.Attacco, Esercito.Unità.Arcere_3.Difesa,
                  Esercito.Unità.Arcere_3.Salute, Esercito.Unità.Arcere_3.Esperienza,
                  Esercito.Unità.Catapulta_3.Attacco, Esercito.Unità.Catapulta_3.Difesa,
                  Esercito.Unità.Catapulta_3.Salute, Esercito.Unità.Catapulta_3.Esperienza),

            3 => (Esercito.Unità.Guerriero_4.Attacco, Esercito.Unità.Guerriero_4.Difesa,
                  Esercito.Unità.Guerriero_4.Salute, Esercito.Unità.Guerriero_4.Esperienza,
                  Esercito.Unità.Lancere_4.Attacco, Esercito.Unità.Lancere_4.Difesa,
                  Esercito.Unità.Lancere_4.Salute, Esercito.Unità.Lancere_4.Esperienza,
                  Esercito.Unità.Arcere_4.Attacco, Esercito.Unità.Arcere_4.Difesa,
                  Esercito.Unità.Arcere_4.Salute, Esercito.Unità.Arcere_4.Esperienza,
                  Esercito.Unità.Catapulta_4.Attacco, Esercito.Unità.Catapulta_4.Difesa,
                  Esercito.Unità.Catapulta_4.Salute, Esercito.Unità.Catapulta_4.Esperienza),

            4 => (Esercito.Unità.Guerriero_5.Attacco, Esercito.Unità.Guerriero_5.Difesa,
                  Esercito.Unità.Guerriero_5.Salute, Esercito.Unità.Guerriero_5.Esperienza,
                  Esercito.Unità.Lancere_5.Attacco, Esercito.Unità.Lancere_5.Difesa,
                  Esercito.Unità.Lancere_5.Salute, Esercito.Unità.Lancere_5.Esperienza,
                  Esercito.Unità.Arcere_5.Attacco, Esercito.Unità.Arcere_5.Difesa,
                  Esercito.Unità.Arcere_5.Salute, Esercito.Unità.Arcere_5.Esperienza,
                  Esercito.Unità.Catapulta_5.Attacco, Esercito.Unità.Catapulta_5.Difesa,
                  Esercito.Unità.Catapulta_5.Salute, Esercito.Unità.Catapulta_5.Esperienza),

            _ => (0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
        };
    }
    private static void AggiornaBarbari(int livello, string tipo, Player player, UnitGroup survivors)
    {
        int tierIndex = GetTierIndex(livello);

        if (tipo == "Città Barbaro")
        {
            var citta = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == 1);
            if (citta == null) return;

            int truppe = citta.Guerrieri + citta.Lancieri + citta.Arcieri + citta.Catapulte;
            if (truppe == 0)
                citta.Sconfitto = true;

            // Aggiorna con i sopravvissuti finali (dopo entrambe le fasi)
            citta.Guerrieri = survivors.Guerrieri[tierIndex];
            citta.Lancieri = survivors.Lancieri[tierIndex];
            citta.Arcieri = survivors.Arcieri[tierIndex];
            citta.Catapulte = survivors.Catapulte[tierIndex];
        }
        else if (tipo == "Villaggio Barbaro")
        {
            var villaggio = player.VillaggiPersonali[livello - 1];
            if (villaggio == null) return;

            int truppe = villaggio.Guerrieri + villaggio.Lancieri + villaggio.Arcieri + villaggio.Catapulte;
            if ( truppe == 0)
                villaggio.Sconfitto = true;
            
            // Aggiorna con i sopravvissuti finali (dopo entrambe le fasi)
            villaggio.Guerrieri = survivors.Guerrieri[tierIndex];
            villaggio.Lancieri = survivors.Lancieri[tierIndex];
            villaggio.Arcieri = survivors.Arcieri[tierIndex];
            villaggio.Catapulte = survivors.Catapulte[tierIndex];
        }
    }
    private static void InviaLogBattaglia(Guid clientGuid, Player player, BattleResult result, string tipo, UnitGroup unitàGiocatore, UnitGroup unitàNemico)
    {
        SendClient(clientGuid, "Log_Server|");
        SendClient(clientGuid, $"Log_Server|╔══════════════════════════════════════════════════╗");
        SendClient(clientGuid, $"Log_Server|║        RESOCONTO BATTAGLIA vs {tipo}            ║");
        SendClient(clientGuid, $"Log_Server|╚══════════════════════════════════════════════════╝\n");

        // Log fase a distanza se presente
        if (result.RangedPhase != null)
        {
            SendClient(clientGuid, $"Log_Server|------ FASE 'I' A DISTANZA ------");
            SendClient(clientGuid, "Log_Server|");
            SendClient(clientGuid, $"Log_Server|Frecce utilizzate: {result.RangedPhase.PlayerArrowsUsed}");

            // ✨ NUOVO: Log dettagliato per livello
            SendClient(clientGuid, $"Log_Server|Nemici eliminati:");
            int totGuerrieri = 0, totLancieri = 0;
            for (int i = 0; i < 5; i++)
            {
                int g = result.RangedPhase.PlayerKills_Guerrieri[i];
                int l = result.RangedPhase.PlayerKills_Lancieri[i];
                if (g > 0 || l > 0)
                {
                    SendClient(clientGuid, $"Log_Server|Lv{i + 1}: {g} Guerrieri + {l} Lanceri");
                    totGuerrieri += g;
                    totLancieri += l;
                }
            }
            SendClient(clientGuid, $"Log_Server|TOT: {totGuerrieri} Guerrieri + {totLancieri} Lanceri");

            SendClient(clientGuid, $"Log_Server|\nPerdite giocatore:");
            int totTueG = 0, totTueL = 0;
            for (int i = 0; i < 5; i++)
            {
                int g = result.RangedPhase.EnemyKills_Guerrieri[i];
                int l = result.RangedPhase.EnemyKills_Lancieri[i];
                if (g > 0 || l > 0)
                {
                    SendClient(clientGuid, $"Log_Server|Lv{i + 1}: {g} Guerrieri + {l} Lanceri");
                    totTueG += g;
                    totTueL += l;
                }
            }
            SendClient(clientGuid, $"Log_Server|TOT: {totTueG} Guerrieri + {totTueL} Lanceri");
            SendClient(clientGuid, "Log_Server|");
        }
        if (true) // Solo per non cambiare nomi alle variabili g,l,a,c
        {
            SendClient(clientGuid, $"Log_Server|------ FASE 'II' CORPO A CORPO ------");
            SendClient(clientGuid, "Log_Server|");
            SendClient(clientGuid, $"Log_Server|Danno inflitto: {result.PlayerDamage:F2}");
            SendClient(clientGuid, $"Log_Server|Danno subito: {result.EnemyDamage:F2}");

            SendClient(clientGuid, "Log_Server|");
            if (result.PlayerCasualties.Guerrieri.Sum() + result.PlayerCasualties.Lancieri.Sum() + result.PlayerCasualties.Arcieri.Sum() + result.PlayerCasualties.Catapulte.Sum() > 0)
            {
                SendClient(clientGuid, $"Log_Server|Perdite giocatore: [{player.Username}]");
                LogPerdite(clientGuid, result.PlayerCasualties, unitàGiocatore);
            }
            else SendClient(clientGuid, $"Log_Server|Nessuna perdita subita dal giocatore: [{player.Username}]");

            SendClient(clientGuid, "Log_Server|");
            if (result.EnemyCasualties.Guerrieri.Sum() + result.EnemyCasualties.Lancieri.Sum() + result.EnemyCasualties.Arcieri.Sum() + result.EnemyCasualties.Catapulte.Sum() > 0)
            {
                SendClient(clientGuid, $"Log_Server|Perdite barbaro:");
                LogPerdite(clientGuid, result.EnemyCasualties, unitàNemico);
            }
            else SendClient(clientGuid, $"Log_Server|Nessuna perdita subita dal barbaro");

            SendClient(clientGuid, "Log_Server|");
            SendClient(clientGuid, $"Log_Server|Esperienza TOTALE guadagnata: {result.ExperienceGained}");
            SendClient(clientGuid, $"Log_Server|Esito: {(result.Victory ? "VITTORIA!" : "SCONFITTA")}");
            SendClient(clientGuid, $"Log_Server|════════════════════════════════════════════════════\n");
        } 
    }
    private static void LogPerdite(Guid clientGuid, UnitGroup perdite, UnitGroup totale)
    {
        // Guerrieri
        string guerrieriLog = FormatUnitLog("Guerrieri", perdite.Guerrieri, totale.Guerrieri);
        if (!string.IsNullOrEmpty(guerrieriLog))
            SendClient(clientGuid, $"Log_Server|{guerrieriLog}");

        // Lancieri
        string lancieriLog = FormatUnitLog("Lanceri", perdite.Lancieri, totale.Lancieri);
        if (!string.IsNullOrEmpty(lancieriLog))
            SendClient(clientGuid, $"Log_Server|{lancieriLog}");

        // Arcieri
        string arcieriLog = FormatUnitLog("Arceri", perdite.Arcieri, totale.Arcieri);
        if (!string.IsNullOrEmpty(arcieriLog))
            SendClient(clientGuid, $"Log_Server|{arcieriLog}");

        // Catapulte
        string catapulteLog = FormatUnitLog("Catapulte", perdite.Catapulte, totale.Catapulte);
        if (!string.IsNullOrEmpty(catapulteLog))
            SendClient(clientGuid, $"Log_Server|{catapulteLog}");
    }
    private static string FormatUnitLog(string nomeUnita, int[] perdite, int[] sopravvissuti)
    {
        var parts = new System.Collections.Generic.List<string>();
        for (int i = 0; i < 5; i++)
        {
            int totale = sopravvissuti[i];
            if (totale > 0)
                parts.Add($"Lv{i + 1}: {perdite[i]}/{totale}");
            
        }

        if (parts.Count == 0) return string.Empty;
        return $"{nomeUnita,-12} - {string.Join(" - ", parts)}";
    }
    private static void Send(Guid clientGuid, string message)
    {
        Console.WriteLine(message.Replace("Log_Server|", ""));
    }
    private static void SendClient(Guid clientGuid, string message)
    {
        Server_Strategico.Server.Server.Send(clientGuid, message);
        Console.WriteLine(message.Replace("Log_Server|", ""));
    }

    // ═══════════════════════════════════════════════════════════════
    // ESTENSIONE PER PVP - Battaglia tra giocatori
    // ═══════════════════════════════════════════════════════════════

    public static async Task<BattleResult> Battaglia_PvP(Player attaccante, Player difensore, Guid attackerGuid, Guid defenderGuid, int[] guerriero, int[] lancere, int[] arcere, int[] catapulte,
    bool includiPreBattaglia = true)
    {
        int perdite_Attaccante = 0, perdite_Difensore = 0;

        var attackerUnits = new UnitGroup
        {
            Guerrieri = guerriero,
            Lancieri = lancere,
            Arcieri = arcere,
            Catapulte = catapulte
        };

        var defenderUnits = new UnitGroup
        {
            Guerrieri = (int[])difensore.Guerrieri.Clone(),
            Lancieri = (int[])difensore.Lanceri.Clone(),
            Arcieri = (int[])difensore.Arceri.Clone(),
            Catapulte = (int[])difensore.Catapulte.Clone()
        };

        // Salva unità originali per i log
        var attackerUnitsOriginali = attackerUnits.Clone();
        var defenderUnitsOriginali = defenderUnits.Clone();

        RangedBattleResult rangedResult = null;
        if (includiPreBattaglia)
        {
            rangedResult = EseguiPreBattaglia_PvP(attackerUnits, defenderUnits, attaccante, difensore, attackerGuid, defenderGuid);
            attackerUnits = rangedResult.PlayerUnitsAfter;
            defenderUnits = rangedResult.EnemyUnitsAfter;
        }

        // FASE 2: Battaglia corpo a corpo (con bonus difesa 20%)
        var result = EseguiBattaglia_PvP(attackerUnits, defenderUnits, attaccante, difensore, attackerGuid, defenderGuid);

        if (result != null)
        {
            result.RangedPhase = rangedResult;
            result.ExperienceGained += rangedResult.ExperienceGained;
        }

        // Aggiorna stato attaccante
        for (int i = 0; i < 5; i++)
        {
            attaccante.Guerrieri[i] -= result.PlayerCasualties.Guerrieri[i] + result.RangedPhase.EnemyKills_Guerrieri[i];
            attaccante.Lanceri[i] -= result.PlayerCasualties.Lancieri[i] + result.RangedPhase.EnemyKills_Lancieri[i];
            attaccante.Arceri[i] -= result.PlayerCasualties.Arcieri[i];
            attaccante.Catapulte[i] -= result.PlayerCasualties.Catapulte[i];

            // Statistiche attaccante - Fase distanza
            perdite_Attaccante += result.RangedPhase.EnemyKills_Guerrieri[i];
            perdite_Attaccante += result.RangedPhase.EnemyKills_Lancieri[i];
            perdite_Difensore += result.RangedPhase.PlayerKills_Guerrieri[i];
            perdite_Difensore += result.RangedPhase.PlayerKills_Lancieri[i];

            attaccante.Guerrieri_Eliminate += result.RangedPhase.PlayerKills_Guerrieri[i];
            attaccante.Lanceri_Eliminate += result.RangedPhase.PlayerKills_Lancieri[i];
            attaccante.Guerrieri_Persi += result.RangedPhase.EnemyKills_Guerrieri[i];
            attaccante.Lanceri_Persi += result.RangedPhase.EnemyKills_Lancieri[i];
            attaccante.Unità_Eliminate += result.RangedPhase.PlayerKills_Guerrieri[i] + result.RangedPhase.PlayerKills_Lancieri[i];
            attaccante.Unità_Perse += result.RangedPhase.EnemyKills_Guerrieri[i] + result.RangedPhase.EnemyKills_Lancieri[i];

            // Corpo a corpo attaccante
            perdite_Difensore += result.EnemyCasualties.Guerrieri[i] + result.EnemyCasualties.Lancieri[i] +
                                result.EnemyCasualties.Arcieri[i] + result.EnemyCasualties.Catapulte[i];
            perdite_Attaccante += result.PlayerCasualties.Guerrieri[i] + result.PlayerCasualties.Lancieri[i] +
                                 result.PlayerCasualties.Arcieri[i] + result.PlayerCasualties.Catapulte[i];

            attaccante.Guerrieri_Eliminate += result.EnemyCasualties.Guerrieri[i];
            attaccante.Lanceri_Eliminate += result.EnemyCasualties.Lancieri[i];
            attaccante.Arceri_Eliminate += result.EnemyCasualties.Arcieri[i];
            attaccante.Catapulte_Eliminate += result.EnemyCasualties.Catapulte[i];
            attaccante.Unità_Eliminate += result.EnemyCasualties.Guerrieri[i] + result.EnemyCasualties.Lancieri[i] +
                                          result.EnemyCasualties.Arcieri[i] + result.EnemyCasualties.Catapulte[i];

            attaccante.Guerrieri_Persi += result.PlayerCasualties.Guerrieri[i];
            attaccante.Lanceri_Persi += result.PlayerCasualties.Lancieri[i];
            attaccante.Arceri_Persi += result.PlayerCasualties.Arcieri[i];
            attaccante.Catapulte_Perse += result.PlayerCasualties.Catapulte[i];
            attaccante.Unità_Perse += result.PlayerCasualties.Guerrieri[i] + result.PlayerCasualties.Lancieri[i] +
                                      result.PlayerCasualties.Arcieri[i] + result.PlayerCasualties.Catapulte[i];
        }

        // Aggiorna stato difensore
        for (int i = 0; i < 5; i++)
        {
            difensore.Guerrieri[i] -= result.EnemyCasualties.Guerrieri[i] + result.RangedPhase.PlayerKills_Guerrieri[i];
            difensore.Lanceri[i] -= result.EnemyCasualties.Lancieri[i] + result.RangedPhase.PlayerKills_Lancieri[i];
            difensore.Arceri[i] -= result.EnemyCasualties.Arcieri[i];
            difensore.Catapulte[i] -= result.EnemyCasualties.Catapulte[i];

            // Statistiche difensore corpo a corpo
            difensore.Guerrieri_Eliminate += result.PlayerCasualties.Guerrieri[i];
            difensore.Lanceri_Eliminate += result.PlayerCasualties.Lancieri[i];
            difensore.Arceri_Eliminate += result.PlayerCasualties.Arcieri[i];
            difensore.Catapulte_Eliminate += result.PlayerCasualties.Catapulte[i];
            difensore.Unità_Eliminate += result.PlayerCasualties.Guerrieri[i] + result.PlayerCasualties.Lancieri[i] +
                                         result.PlayerCasualties.Arcieri[i] + result.PlayerCasualties.Catapulte[i];

            difensore.Guerrieri_Persi += result.EnemyCasualties.Guerrieri[i];
            difensore.Lanceri_Persi += result.EnemyCasualties.Lancieri[i];
            difensore.Arceri_Persi += result.EnemyCasualties.Arcieri[i];
            difensore.Catapulte_Perse += result.EnemyCasualties.Catapulte[i];
            difensore.Unità_Perse += result.EnemyCasualties.Guerrieri[i] + result.EnemyCasualties.Lancieri[i] +
                                     result.EnemyCasualties.Arcieri[i] + result.EnemyCasualties.Catapulte[i];
        }

        // Frecce utilizzate
        attaccante.Frecce_Utilizzate += result.RangedPhase.PlayerArrowsUsed;
        difensore.Frecce_Utilizzate += result.RangedPhase.EnemyArrowsUsed;

        // Esperienza
        attaccante.Esperienza += result.ExperienceGained;
        difensore.Esperienza += CalcolaEsperienzaPVP(result.PlayerCasualties);

        // Determina vittoria (l'attaccante vince se causa più perdite del difensore)
        if (perdite_Difensore > perdite_Attaccante)
        {
            SendClient(attackerGuid, "Log_Server|Il difensore si è arreso!");
            SendClient(defenderGuid, "Log_Server|Le tue difese sono cadute!");
            result.Victory = true;
        }
        else
        {
            result.Victory = false;
        }

        if (result.Victory)
        {
            attaccante.Battaglie_Vinte++;
            difensore.Battaglie_Perse++;
            AssegnaRisorseVittoria_PvP(attaccante, difensore, attackerGuid, result.PlayerSurvivors);
        }
        else
        {
            attaccante.Battaglie_Perse++;
            difensore.Battaglie_Vinte++;
        }
        InviaLogBattaglia_PvP(attackerGuid, defenderGuid, attaccante, difensore, result, attackerUnitsOriginali, defenderUnitsOriginali);
        return result;
    }
    private static RangedBattleResult EseguiPreBattaglia_PvP(UnitGroup attackerUnits, UnitGroup defenderUnits, Player attaccante, Player difensore, Guid attackerGuid, Guid defenderGuid)
    {
        var result = new RangedBattleResult
        {
            PlayerUnitsAfter = attackerUnits.Clone(),
            EnemyUnitsAfter = defenderUnits.Clone()
        };

        Send(attackerGuid, "Log_Server|═══════════════════════════════════════");
        Send(attackerGuid, "Log_Server|  FASE 1: ATTACCO A DISTANZA");
        Send(attackerGuid, "Log_Server|═══════════════════════════════════════\n");

        Send(defenderGuid, "Log_Server|═══════════════════════════════════════");
        Send(defenderGuid, "Log_Server|  FASE 1: ATTACCO A DISTANZA");
        Send(defenderGuid, "Log_Server|═══════════════════════════════════════\n");

        // ATTACCO DELL'ATTACCANTE
        var attackerRangedAttack = CalcolaAttaccoDistanza(attackerUnits, attaccante, attackerGuid, true);
        result.PlayerArrowsUsed = attackerRangedAttack.FrecceUsate;
        result.PlayerHadLowArrows = attackerRangedAttack.FrecceInsufficienti;

        // Applica danni con tracciamento livelli
        var attackerMorti = ApplicaDanniDistanza(result.EnemyUnitsAfter, attackerRangedAttack.DannoGuerrieri, attackerRangedAttack.DannoLancieri);
        Array.Copy(attackerMorti, 0, result.PlayerKills_Guerrieri, 0, 5);
        Array.Copy(attackerMorti, 5, result.PlayerKills_Lancieri, 0, 5);

        // Log per attaccante
        Send(attackerGuid, "Log_Server|Hai eliminato:");
        Send(defenderGuid, $"Log_Server|{attaccante.Username} ha eliminato:");

        for (int i = 0; i < 5; i++)
            if (result.PlayerKills_Guerrieri[i] > 0 || result.PlayerKills_Lancieri[i] > 0)
            {
                string msg = $"Log_Server|Lv{i + 1}: {result.PlayerKills_Guerrieri[i]}G + {result.PlayerKills_Lancieri[i]}L";
                Send(attackerGuid, msg);
                Send(defenderGuid, msg);
            }
        
        Send(attackerGuid, $"Log_Server|TOTALE: {result.GetTotalPlayerKills()} unità\n");
        Send(defenderGuid, $"Log_Server|TOTALE: {result.GetTotalPlayerKills()} unità\n");

        // CONTRATTACCO DEL DIFENSORE
        Send(attackerGuid, $"Log_Server|{difensore.Username} contrattacca a distanza...\n");
        Send(defenderGuid, $"Log_Server|Contrattacchi a distanza...\n");

        var defenderRangedAttack = CalcolaAttaccoDistanza(defenderUnits, difensore, defenderGuid, true);
        result.EnemyArrowsUsed = defenderRangedAttack.FrecceUsate;
        result.EnemyHadLowArrows = defenderRangedAttack.FrecceInsufficienti;

        var defenderMorti = ApplicaDanniDistanza(result.PlayerUnitsAfter, defenderRangedAttack.DannoGuerrieri, defenderRangedAttack.DannoLancieri);
        Array.Copy(defenderMorti, 0, result.EnemyKills_Guerrieri, 0, 5);
        Array.Copy(defenderMorti, 5, result.EnemyKills_Lancieri, 0, 5);

        // Log per difensore
        Send(attackerGuid, "Log_Server|Tue perdite:");
        Send(defenderGuid, "Log_Server|Hai eliminato:");

        for (int i = 0; i < 5; i++)
            if (result.EnemyKills_Guerrieri[i] > 0 || result.EnemyKills_Lancieri[i] > 0)
            {
                string msg = $"Log_Server|   Lv{i + 1}: {result.EnemyKills_Guerrieri[i]}G + {result.EnemyKills_Lancieri[i]}L";
                Send(attackerGuid, msg);
                Send(defenderGuid, msg);
            }
        
        Send(attackerGuid, $"Log_Server|TOTALE: {result.GetTotalEnemyKills()} unità\n");
        Send(defenderGuid, $"Log_Server|TOTALE: {result.GetTotalEnemyKills()} unità\n");

        // Calcola esperienza fase a distanza per entrambi
        int expAttaccante = 0;
        int expDifensore = 0;

        for (int i = 0; i < 5; i++)
        {
            var stats = GetUnitStats(i);

            // Attaccante guadagna exp per unità difensore uccise
            expAttaccante += result.PlayerKills_Guerrieri[i] * stats.GuerrieriEsperienza;
            expAttaccante += result.PlayerKills_Lancieri[i] * stats.LancieriEsperienza;

            // Difensore guadagna exp per unità attaccante uccise
            expDifensore += result.EnemyKills_Guerrieri[i] * stats.GuerrieriEsperienza;
            expDifensore += result.EnemyKills_Lancieri[i] * stats.LancieriEsperienza;
        }

        attaccante.Esperienza += expAttaccante;
        difensore.Esperienza += expDifensore;
        result.ExperienceGained = expAttaccante; // Per il log dell'attaccante

        Send(attackerGuid, $"Log_Server|Esperienza fase a distanza: +{expAttaccante}");
        Send(defenderGuid, $"Log_Server|Esperienza fase a distanza: +{expDifensore}");
        Send(attackerGuid, "Log_Server|═══════════════════════════════════════\n");
        Send(defenderGuid, "Log_Server|═══════════════════════════════════════\n");

        Send(attackerGuid, "Log_Server|═══════════════════════════════════════\n");
        Send(defenderGuid, "Log_Server|═══════════════════════════════════════\n");

        return result;
    }
    private static BattleResult EseguiBattaglia_PvP(UnitGroup attackerUnits, UnitGroup defenderUnits, Player attaccante, Player difensore, Guid attackerGuid, Guid defenderGuid)
    {
        var result = new BattleResult
        {
            PlayerSurvivors = attackerUnits.Clone(),
            EnemySurvivors = defenderUnits.Clone(),
            PlayerCasualties = new UnitGroup(),
            EnemyCasualties = new UnitGroup()
        };

        int attackerTypes = attackerUnits.CountUnitTypes();
        int defenderTypes = defenderUnits.CountUnitTypes();
        // Calcola danno
        double dannoAttaccante = CalcolaDannoGiocatore(attackerUnits, attaccante, attackerGuid);
        double dannoDifensore = CalcolaDannoGiocatore(defenderUnits, difensore, defenderGuid);

        // ⭐ BONUS DIFESA 20% per il difensore
        dannoDifensore *= 1.15;
        result.PlayerDamage = dannoAttaccante;
        result.EnemyDamage = dannoDifensore;
        double dannoPerTipoAttacker = dannoDifensore / attackerTypes;
        double dannoPerTipoDefender = dannoAttaccante / defenderTypes;

        ApplicaDanniGiocatore(result, attaccante, dannoPerTipoAttacker); // Applica danni all'attaccante

        var tempResult = new BattleResult // Applica danni al difensore(usando struttura temporanea per riutilizzare la funzione)
        {
            PlayerSurvivors = result.EnemySurvivors,
            PlayerCasualties = result.EnemyCasualties
        };
        ApplicaDanniGiocatore(tempResult, difensore, dannoPerTipoDefender);
        result.EnemySurvivors = tempResult.PlayerSurvivors;
        result.EnemyCasualties = tempResult.PlayerCasualties;

        // Calcola esperienza corpo a corpo per entrambi
        int expAttaccante = CalcolaEsperienzaPVP(result.EnemyCasualties);
        int expDifensore = CalcolaEsperienzaPVP(result.PlayerCasualties);

        attaccante.Esperienza += expAttaccante;
        difensore.Esperienza += expDifensore;

        result.ExperienceGained = expAttaccante; // Per il log

        result.Victory = result.EnemySurvivors.TotalUnits() == 0; // Determina vittoria provvisoria (sarà confermata in base alle perdite totali)
        return result;
    }
    private static void AssegnaRisorseVittoria_PvP(Player attaccante, Player difensore, Guid attackerGuid, UnitGroup sopravvissuti)
    {
        int capacitàCarico = CapacitàCarico(sopravvissuti);
        int capacitàOriginale = capacitàCarico;

        // Il 50% delle risorse del difensore può essere rubato
        double cibo = difensore.Cibo / 2;
        double legno = difensore.Legno / 2;
        double pietra = difensore.Pietra / 2;
        double ferro = difensore.Ferro / 2;
        double oro = difensore.Oro / 2;

        // Raccogli risorse
        var raccolte = RaccoliRisorseEquamente(capacitàCarico, cibo, legno, pietra, ferro, oro, 0, 0, 0);

        // Assegna all'attaccante
        attaccante.Cibo += raccolte.Cibo;
        attaccante.Legno += raccolte.Legno;
        attaccante.Pietra += raccolte.Pietra;
        attaccante.Ferro += raccolte.Ferro;
        attaccante.Oro += raccolte.Oro;

        // Rimuovi dal difensore
        difensore.Cibo -= raccolte.Cibo;
        difensore.Legno -= raccolte.Legno;
        difensore.Pietra -= raccolte.Pietra;
        difensore.Ferro -= raccolte.Ferro;
        difensore.Oro -= raccolte.Oro;

        // Calcola peso utilizzato
        int pesoUtilizzato =
            raccolte.Cibo * Variabili_Server.peso_Risorse_Civile +
            raccolte.Legno * Variabili_Server.peso_Risorse_Civile +
            raccolte.Pietra * Variabili_Server.peso_Risorse_Civile +
            raccolte.Ferro * Variabili_Server.peso_Risorse_Ferro +
            raccolte.Oro * Variabili_Server.peso_Risorse_Oro;

        // Log dettagliato
        SendClient(attackerGuid, "Log_Server|");
        SendClient(attackerGuid, "Log_Server|╔══════════════════════════════════════════════════╗");
        SendClient(attackerGuid, "Log_Server|║        VITTORIA - RISORSE SACCHEGGIATE          ║");
        SendClient(attackerGuid, "Log_Server|╚══════════════════════════════════════════════════╝");
        SendClient(attackerGuid, $"Log_Server|Capacità di carico: {capacitàOriginale:N0}");
        SendClient(attackerGuid, $"Log_Server|Capacità utilizzata: {pesoUtilizzato:N0}");
        SendClient(attackerGuid, "Log_Server|");

        if (raccolte.Cibo > 0) SendClient(attackerGuid, $"Log_Server|  Cibo:   +{raccolte.Cibo:N0}");
        if (raccolte.Legno > 0) SendClient(attackerGuid, $"Log_Server|  Legno:  +{raccolte.Legno:N0}");
        if (raccolte.Pietra > 0) SendClient(attackerGuid, $"Log_Server|  Pietra: +{raccolte.Pietra:N0}");
        if (raccolte.Ferro > 0) SendClient(attackerGuid, $"Log_Server|  Ferro:  +{raccolte.Ferro:N0}");
        if (raccolte.Oro > 0) SendClient(attackerGuid, $"Log_Server|  Oro:    +{raccolte.Oro:N0}");

        SendClient(attackerGuid, "Log_Server|════════════════════════════════════════════════════\n");
    }
    private static void InviaLogBattaglia_PvP(Guid attackerGuid, Guid defenderGuid, Player attaccante, Player difensore, BattleResult result,UnitGroup attackerUnitsOriginali, UnitGroup defenderUnitsOriginali)
    {
        // Log per l'attaccante
        SendClient(attackerGuid, "Log_Server|");
        SendClient(attackerGuid, "Log_Server|╔══════════════════════════════════════════════════╗");
        SendClient(attackerGuid, $"Log_Server|║     RESOCONTO BATTAGLIA vs {difensore.Username,-20}║");
        SendClient(attackerGuid, "Log_Server|╚══════════════════════════════════════════════════╝\n");

        // Fase distanza
        if (result.RangedPhase != null)
        {
            SendClient(attackerGuid, "Log_Server|------ FASE 'I' A DISTANZA ------");
            SendClient(attackerGuid, "Log_Server|");
            SendClient(attackerGuid, $"Log_Server|Frecce utilizzate: {result.RangedPhase.PlayerArrowsUsed}");
            SendClient(attackerGuid, $"Log_Server|Nemici eliminati: {result.RangedPhase.GetTotalPlayerKills()} unità");
            SendClient(attackerGuid, $"Log_Server|Tue perdite: {result.RangedPhase.GetTotalEnemyKills()} unità");
            SendClient(attackerGuid, "Log_Server|");
        }

        SendClient(attackerGuid, "Log_Server|------ FASE 'II' CORPO A CORPO ------");
        SendClient(attackerGuid, "Log_Server|");
        SendClient(attackerGuid, $"Log_Server|Danno inflitto: {result.PlayerDamage:F2}");
        SendClient(attackerGuid, $"Log_Server|Danno subito: {result.EnemyDamage:F2}");
        SendClient(attackerGuid, "Log_Server|");

        if (result.PlayerCasualties.TotalUnits() > 0)
        {
            SendClient(attackerGuid, "Log_Server|Tue perdite:");
            LogPerdite(attackerGuid, result.PlayerCasualties, attackerUnitsOriginali);
        }
        else
        {
            SendClient(attackerGuid, "Log_Server|Nessuna perdita subita!");
        }

        SendClient(attackerGuid, "Log_Server|");
        if (result.EnemyCasualties.TotalUnits() > 0)
        {
            SendClient(attackerGuid, "Log_Server|Perdite nemiche:");
            LogPerdite(attackerGuid, result.EnemyCasualties, defenderUnitsOriginali);
        }

        SendClient(attackerGuid, "Log_Server|");
        SendClient(attackerGuid, $"Log_Server|Esperienza guadagnata: {result.ExperienceGained}");
        SendClient(attackerGuid, $"Log_Server|Esito: {(result.Victory ? "VITTORIA!" : "SCONFITTA")}");
        SendClient(attackerGuid, "Log_Server|════════════════════════════════════════════════════\n");

        // Log per il difensore (speculare)
        SendClient(defenderGuid, "Log_Server|");
        SendClient(defenderGuid, "Log_Server|╔══════════════════════════════════════════════════╗");
        SendClient(defenderGuid, $"Log_Server|║     ATTACCO SUBITO da {attaccante.Username,-23}║");
        SendClient(defenderGuid, "Log_Server|╚══════════════════════════════════════════════════╝\n");

        if (result.RangedPhase != null)
        {
            SendClient(defenderGuid, "Log_Server|------ FASE 'I' A DISTANZA ------");
            SendClient(defenderGuid, "Log_Server|");
            SendClient(defenderGuid, $"Log_Server|Frecce utilizzate: {result.RangedPhase.EnemyArrowsUsed}");
            SendClient(defenderGuid, $"Log_Server|Nemici eliminati: {result.RangedPhase.GetTotalEnemyKills()} unità");
            SendClient(defenderGuid, $"Log_Server|Tue perdite: {result.RangedPhase.GetTotalPlayerKills()} unità");
            SendClient(defenderGuid, "Log_Server|");
        }

        SendClient(defenderGuid, "Log_Server|------ FASE 'II' CORPO A CORPO ------");
        SendClient(defenderGuid, "Log_Server|");
        SendClient(defenderGuid, $"Log_Server|Danno inflitto: {result.EnemyDamage:F2} (+20% bonus difesa)");
        SendClient(defenderGuid, $"Log_Server|Danno subito: {result.PlayerDamage:F2}");
        SendClient(defenderGuid, "Log_Server|");

        if (result.EnemyCasualties.TotalUnits() > 0)
        {
            SendClient(defenderGuid, "Log_Server|Tue perdite:");
            LogPerdite(defenderGuid, result.EnemyCasualties, defenderUnitsOriginali);
        }
        else
        {
            SendClient(defenderGuid, "Log_Server|Nessuna perdita subita!");
        }

        SendClient(defenderGuid, "Log_Server|");
        if (result.PlayerCasualties.TotalUnits() > 0)
        {
            SendClient(defenderGuid, "Log_Server|Perdite nemiche:");
            LogPerdite(defenderGuid, result.PlayerCasualties, attackerUnitsOriginali);
        }

        SendClient(defenderGuid, "Log_Server|");
        int expDifensore = CalcolaEsperienzaPVP(result.PlayerCasualties);
        SendClient(defenderGuid, $"Log_Server|Esperienza guadagnata: {expDifensore}");
        SendClient(defenderGuid, $"Log_Server|Esito: {(!result.Victory ? "DIFESA RIUSCITA!" : "CITTÀ CONQUISTATA")}");
        SendClient(defenderGuid, "Log_Server|════════════════════════════════════════════════════\n");
    }
}