using Server_Strategico.Gioco;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Server_Strategico.Gioco.Esercito;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.QuestManager;
using static Server_Strategico.Gioco.Ricerca;

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
        public RangedBattleResult FaseDistanza { get; set; }  // ✨ Risultati pre-battaglia
        public UnitGroup AttaccanteSopravvisuti { get; set; }
        public UnitGroup AttaccantePerdite { get; set; }
        public UnitGroup DifensoreSopravvisuti { get; set; }
        public UnitGroup DifensorePerdite { get; set; }
        public UnitGroup DifensoreCrollo { get; set; }
        public double PlayerDamage { get; set; }
        public double EnemyDamage { get; set; }
        public int ArrowsUsedAttacker { get; set; }
        public int FrecceNecessarieAttacker { get; set; }
        public int FrecceNecessarieDefender { get; set; }
        public int ArrowsUsedDefender { get; set; }
        public int Xp_Attacker { get; set; }
        public int Xp_Defender { get; set; }
        public string Struttura { get; set; }
        public int Salute { get; set; }
        public int Difesa { get; set; }
        public bool Victory { get; set; }
        public bool Unità_Presenti { get; set; }

        public bool PocheFrecceAttacker { get; set; }
        public bool PocheFrecceDefender { get; set; }
    }
    public class RangedBattleResult
    {
        public UnitGroup AttaccanteUnitsAfter { get; set; }
        public UnitGroup DifensoreUnitsAfter { get; set; }
        
        public int[] DefenderKills_Guerrieri { get; set; }  // Array di 5 elementi
        public int[] DefenderKills_Lancieri { get; set; }    // Array di 5 elementi
        public int[] AttackerKills_Guerrieri { get; set; }    // Array di 5 elementi
        public int[] AttackerKills_Lancieri { get; set; }     // Array di 5 elementi
        
        public int ArrowsUsedAttacker { get; set; }
        public int ArrowsUsedDefender { get; set; }
        public int Xp_Attacker { get; set; }
        public int Xp_Defender { get; set; }
        public bool PlayerHadLowArrows { get; set; }
        public bool EnemyHadLowArrows { get; set; }
        public bool Unità_Presenti { get; set; }

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
            baseStats.Item1.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Guerriero_Attacco * (1 + player.Bonus_Attacco_Guerrieri),
            baseStats.Item1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa * (1 + player.Bonus_Difesa_Guerrieri),
            baseStats.Item1.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute * (1 + player.Bonus_Salute_Guerrieri),

            baseStats.Item2.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Lancere_Attacco * (1 + player.Bonus_Attacco_Lanceri),
            baseStats.Item2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa * (1 + player.Bonus_Difesa_Lanceri),
            baseStats.Item2.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute * (1 + player.Bonus_Salute_Lanceri),

            baseStats.Item3.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Arcere_Attacco * (1 + player.Bonus_Attacco_Arceri),
            baseStats.Item3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa * (1 + player.Bonus_Difesa_Arceri),
            baseStats.Item3.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute * (1 + player.Bonus_Salute_Arceri),

            baseStats.Item4.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Catapulta_Attacco * (1 + player.Bonus_Attacco_Catapulte),
            baseStats.Item4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa * (1 + player.Bonus_Difesa_Catapulte),
            baseStats.Item4.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute * (1 + player.Bonus_Salute_Catapulte)
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

    public static async Task<BattleResult> Battaglia_Barbari(Player player, Guid clientGuid, string tipo, string livello, int[] guerriero, int[] lancere, int[] arcere, int[] catapulte, bool includiPreBattaglia = true)  // ✨ Nuovo parametro
    {
        //Quest
        if (tipo == "Villaggio Barbaro") OnEvent(player, QuestEventType.Battaglie, "Attacco Villaggio Barbaro", 1);
        if (tipo == "Città Barbaro") OnEvent(player, QuestEventType.Battaglie, "Attacco Citta Barbaro", 1);

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
            playerUnits = rangedResult.AttaccanteUnitsAfter;
            enemyUnits = rangedResult.DifensoreUnitsAfter;
        }

        var result = EseguiBattaglia(playerUnits, enemyUnits, player, clientGuid, liv); //  FASE 2: Battaglia corpo a corpo
        if (result != null) // Aggiungi i dati della pre-battaglia al risultato finale
        {
            result.FaseDistanza = rangedResult;
            result.Xp_Attacker += rangedResult.Xp_Attacker;
        }

        for (int i = 0; i < player.Guerrieri.Count(); i++) // Aggiorna lo stato del giocatore
        {
            // Aggiorna lo stato del giocatore
            player.Guerrieri[i] -= result.AttaccantePerdite.Guerrieri[i] + result.FaseDistanza.DefenderKills_Guerrieri[i];
            player.Lanceri[i] -= result.AttaccantePerdite.Lancieri[i] + result.FaseDistanza.DefenderKills_Lancieri[i];
            player.Arceri[i] -= result.AttaccantePerdite.Arcieri[i];
            player.Catapulte[i] -= result.AttaccantePerdite.Catapulte[i];

            //Statistiche - Attacco Distanza
            perdite_Giocatore += result.FaseDistanza.DefenderKills_Guerrieri[i];
            perdite_Giocatore += result.FaseDistanza.DefenderKills_Lancieri[i];
            perdite_Nemico += result.FaseDistanza.AttackerKills_Guerrieri[i];
            perdite_Nemico += result.FaseDistanza.AttackerKills_Lancieri[i];

            player.Guerrieri_Eliminati += result.FaseDistanza.AttackerKills_Guerrieri[i];
            player.Lanceri_Eliminati += result.FaseDistanza.AttackerKills_Lancieri[i];
            player.Guerrieri_Persi += result.FaseDistanza.DefenderKills_Guerrieri[i];
            player.Lanceri_Persi += result.FaseDistanza.DefenderKills_Lancieri[i];

            player.Unità_Eliminate += result.FaseDistanza.AttackerKills_Guerrieri[i];
            player.Unità_Eliminate += result.FaseDistanza.AttackerKills_Lancieri[i];
            player.Unità_Perse += result.FaseDistanza.DefenderKills_Guerrieri[i];
            player.Unità_Perse += result.FaseDistanza.DefenderKills_Lancieri[i];

            //Corpo a corpo
            perdite_Giocatore += result.AttaccantePerdite.Guerrieri[i];
            perdite_Giocatore += result.AttaccantePerdite.Lancieri[i];
            perdite_Giocatore += result.AttaccantePerdite.Arcieri[i];
            perdite_Giocatore += result.AttaccantePerdite.Catapulte[i];

            perdite_Nemico += result.DifensorePerdite.Guerrieri[i];
            perdite_Nemico += result.DifensorePerdite.Lancieri[i];
            perdite_Nemico += result.DifensorePerdite.Arcieri[i];
            perdite_Nemico += result.DifensorePerdite.Catapulte[i];

            OnEvent(player, QuestEventType.Uccisioni, "Guerrieri", result.FaseDistanza.AttackerKills_Guerrieri[i]);
            OnEvent(player, QuestEventType.Uccisioni, "Lanceri", result.FaseDistanza.AttackerKills_Lancieri[i]);
            OnEvent(player, QuestEventType.Uccisioni, "Guerrieri", result.DifensorePerdite.Guerrieri[i]);
            OnEvent(player, QuestEventType.Uccisioni, "Lanceri", result.DifensorePerdite.Lancieri[i]);
            OnEvent(player, QuestEventType.Uccisioni, "Arceri", result.DifensorePerdite.Arcieri[i]);
            OnEvent(player, QuestEventType.Uccisioni, "Catapulte", result.DifensorePerdite.Catapulte[i]);

            player.Guerrieri_Eliminati += result.DifensorePerdite.Guerrieri[i];
            player.Lanceri_Eliminati += result.DifensorePerdite.Lancieri[i];
            player.Arceri_Eliminati += result.DifensorePerdite.Arcieri[i];
            player.Catapulte_Eliminate += result.DifensorePerdite.Catapulte[i];

            player.Barbari_Sconfitti += result.DifensorePerdite.Guerrieri[i];
            player.Barbari_Sconfitti += result.DifensorePerdite.Lancieri[i];
            player.Barbari_Sconfitti += result.DifensorePerdite.Arcieri[i];
            player.Barbari_Sconfitti += result.DifensorePerdite.Catapulte[i];

            player.Unità_Perse += result.AttaccantePerdite.Guerrieri[i];
            player.Unità_Perse += result.AttaccantePerdite.Lancieri[i];
            player.Unità_Perse += result.AttaccantePerdite.Arcieri[i];
            player.Unità_Perse += result.AttaccantePerdite.Catapulte[i];

            player.Unità_Eliminate += result.DifensorePerdite.Guerrieri[i];
            player.Unità_Eliminate += result.DifensorePerdite.Lancieri[i];
            player.Unità_Eliminate += result.DifensorePerdite.Arcieri[i];
            player.Unità_Eliminate += result.DifensorePerdite.Catapulte[i];

            player.Guerrieri_Persi += result.AttaccantePerdite.Guerrieri[i];
            player.Lanceri_Persi += result.AttaccantePerdite.Lancieri[i];
            player.Arceri_Persi += result.AttaccantePerdite.Arcieri[i];
            player.Catapulte_Perse += result.AttaccantePerdite.Catapulte[i];
        }
        player.Esperienza += result.Xp_Attacker;
        int barbari_Vivi = result.DifensoreSopravvisuti.TotalUnits();

        if (perdite_Nemico > perdite_Giocatore && barbari_Vivi == 0) // Vittoria del giocatore se le sue perdite sono inferiori a quelle del nemico
        {
            SendClient(clientGuid, "Log_Server|Il nemico si è ritirato prima del previsto");
            result.Victory = true;
        }
        if (result.Victory)
        {
            AssegnaRisorseVittoria(player, clientGuid, tipo, liv, result.AttaccanteSopravvisuti); // Assegna risorse
            player.Battaglie_Vinte++;

            var citta = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == liv);
            var villaggio = player.VillaggiPersonali[liv - 1];
            int valore = enemyUnits.Guerrieri.Sum() + enemyUnits.Lancieri.Sum() + enemyUnits.Arcieri.Sum() + enemyUnits.Catapulte.Sum() + citta.Cibo + citta.Legno + citta.Pietra + citta.Ferro + citta.Oro + citta.Diamanti_Blu + citta.Diamanti_Viola + citta.Esperienza;
            int valore2 = enemyUnits.Guerrieri.Sum() + enemyUnits.Lancieri.Sum() + enemyUnits.Arcieri.Sum() + enemyUnits.Catapulte.Sum() + villaggio.Cibo + villaggio.Legno + villaggio.Pietra + villaggio.Ferro + villaggio.Oro + villaggio.Diamanti_Blu + villaggio.Diamanti_Viola + villaggio.Esperienza;

            if (tipo == "Villaggio Barbaro" && valore == 0) player.Accampamenti_Barbari_Sconfitti++;
            if (tipo == "Città Barbaro" && valore2 == 0) player.Città_Barbare_Sconfitte++;
        }
        else player.Battaglie_Perse++;

        AggiornaBarbari(liv, tipo, player, result.DifensoreSopravvisuti); // Aggiorna i barbari
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
        int capacitàCarico = CapacitàCarico(sopravvissuti, player);
        int capacitàOriginale = capacitàCarico;
        int cibo = 0, legno = 0, pietra = 0, ferro = 0, oro = 0, exp = 0, diamBlu = 0, diamViola = 0;

        if (tipo == "Città Barbaro")
        {
            var citta = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == livello);
            if (citta == null)
            { SendClient(clientGuid, "Log_Server|[error]Città barbaro non trovata!"); return; }

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
            { SendClient(clientGuid, "Log_Server|[error]Villaggio barbaro non trovato!"); return; }

            cibo = villaggio.Cibo;
            legno = villaggio.Legno;
            pietra = villaggio.Pietra;
            ferro = villaggio.Ferro;
            oro = villaggio.Oro;
            exp = villaggio.Esperienza;
            diamBlu = villaggio.Diamanti_Blu;
            diamViola = villaggio.Diamanti_Viola;
        }
        var raccolte = RaccoliRisorseEquamente(capacitàCarico, cibo, legno, pietra, ferro, oro, exp, diamBlu, diamViola); // ⭐ RACCOGLI LE RISORSE

        // Assegna al giocatore
        player.Cibo += raccolte.Cibo;
        player.Legno += raccolte.Legno;
        player.Pietra += raccolte.Pietra;
        player.Ferro += raccolte.Ferro;
        player.Oro += raccolte.Oro;
        player.Esperienza += exp;
        player.Diamanti_Blu += raccolte.Diamanti_Blu;
        player.Diamanti_Viola += raccolte.Diamanti_Viola;
        player.Risorse_Razziate += raccolte.Cibo + raccolte.Legno + raccolte.Pietra + raccolte.Ferro + raccolte.Oro + raccolte.Diamanti_Blu + raccolte.Diamanti_Viola;

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
            raccolte.Cibo * Variabili_Server.peso_Risorse_Cibo + 
            raccolte.Legno * Variabili_Server.peso_Risorse_Legno + 
            raccolte.Pietra * Variabili_Server.peso_Risorse_Pietra +               
            raccolte.Ferro * Variabili_Server.peso_Risorse_Ferro + 
            raccolte.Oro * Variabili_Server.peso_Risorse_Oro +           
            raccolte.Diamanti_Blu * Variabili_Server.peso_Risorse_Diamante_Blu +
            raccolte.Diamanti_Viola * Variabili_Server.peso_Risorse_Diamante_Viola;

        // Log dettagliato
        SendClient(clientGuid, "Log_Server|");
        SendClient(clientGuid, "Log_Server|╔══════════════════════════════════════════════════╗");
        SendClient(clientGuid, $"Log_Server|║  CONQUISTA {(tipo == "Città Barbaro" ? "CITTÀ" : "VILLAGGIO")} - RISORSE RACCOLTE        ║");
        SendClient(clientGuid, "Log_Server|╚══════════════════════════════════════════════════╝");
        SendClient(clientGuid, $"Log_Server|[highlight]Capacità di carico: [info]{capacitàOriginale:N0}");
        SendClient(clientGuid, $"Log_Server|[highlight]Capacità utilizzata: [info]{pesoUtilizzato:N0}");
        SendClient(clientGuid, "Log_Server|");

        if (raccolte.Cibo > 0) SendClient(clientGuid, $"Log_Server|[cibo]Cibo: +{raccolte.Cibo:N0}[/cibo][icon:cibo] (peso: {raccolte.Cibo * Variabili_Server.peso_Risorse_Cibo})");
        if (raccolte.Legno > 0) SendClient(clientGuid, $"Log_Server|[legno]Legno: +{raccolte.Legno:N0}[/legno][icon:legno] (peso: {raccolte.Legno * Variabili_Server.peso_Risorse_Legno})");
        if (raccolte.Pietra > 0) SendClient(clientGuid, $"Log_Server|[pietra]Pietra: +{raccolte.Pietra:N0}[/pietra][icon:pietra] (peso: {raccolte.Pietra * Variabili_Server.peso_Risorse_Pietra})");
        if (raccolte.Ferro > 0) SendClient(clientGuid, $"Log_Server|[ferro]Ferro: +{raccolte.Ferro:N0}[/ferro][icon:ferro] (peso: {raccolte.Ferro * Variabili_Server.peso_Risorse_Ferro})");
        if (raccolte.Oro > 0) SendClient(clientGuid, $"Log_Server|[oro]Oro: +{raccolte.Oro:N0}[/oro][icon:oro] (peso: {raccolte.Oro * Variabili_Server.peso_Risorse_Oro})");
        if (raccolte.Diamanti_Blu > 0) SendClient(clientGuid, $"Log_Server|[blu]Diamanti Blu: +{raccolte.Diamanti_Blu:N0}[/blu][icon:diamanteBlu] (peso: {raccolte.Diamanti_Blu * Variabili_Server.peso_Risorse_Diamante_Blu})");
        if (raccolte.Diamanti_Viola > 0) SendClient(clientGuid, $"Log_Server|[viola]Diamanti Viola: +{raccolte.Diamanti_Viola:N0}[/viola][icon:DiamanteViola] (peso: {raccolte.Diamanti_Viola * Variabili_Server.peso_Risorse_Diamante_Viola})");

        SendClient(clientGuid, "Log_Server|════════════════════════════════════════════════════\n");
    }
    static int CapacitàCarico(UnitGroup playerUnits, Player player)
    {
        int capacitàCarico = 0;
        capacitàCarico += (int)(playerUnits.Guerrieri[0] * Esercito.Unità.Guerriero_1.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Guerrieri[1] * Esercito.Unità.Guerriero_2.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Guerrieri[2] * Esercito.Unità.Guerriero_3.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Guerrieri[3] * Esercito.Unità.Guerriero_4.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Guerrieri[4] * Esercito.Unità.Guerriero_5.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));

        capacitàCarico += (int)(playerUnits.Lancieri[0] * Esercito.Unità.Lancere_1.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Lancieri[1] * Esercito.Unità.Lancere_2.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Lancieri[2] * Esercito.Unità.Lancere_3.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Lancieri[3] * Esercito.Unità.Lancere_4.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Lancieri[4] * Esercito.Unità.Lancere_5.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));

        capacitàCarico += (int)(playerUnits.Arcieri[0] * Esercito.Unità.Arcere_1.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Arcieri[1] * Esercito.Unità.Arcere_2.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Arcieri[2] * Esercito.Unità.Arcere_3.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Arcieri[3] * Esercito.Unità.Arcere_4.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Arcieri[4] * Esercito.Unità.Arcere_5.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));

        capacitàCarico += (int)(playerUnits.Catapulte[0] * Esercito.Unità.Catapulta_1.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Catapulte[1] * Esercito.Unità.Catapulta_2.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Catapulte[2] * Esercito.Unità.Catapulta_3.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Catapulte[3] * Esercito.Unità.Catapulta_4.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        capacitàCarico += (int)(playerUnits.Catapulte[4] * Esercito.Unità.Catapulta_5.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
        return (int)(capacitàCarico * (1 + player.Bonus_Capacità_Trasporto));//Aggiunge bonus trasporto
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
    } // Struttura per contenere le risorse raccolte
    private static RisorseRaccolte RaccoliRisorseEquamente(double capacitàCarico,
    double cibo, double legno, double pietra, double ferro, double oro, int exp, int diamBlu, int diamViola)
    {
        var risultato = new RisorseRaccolte();
        int tipiRisorse = 0; // Conta quante risorse sono disponibili
        if (cibo > 0) tipiRisorse++;
        if (legno > 0) tipiRisorse++;
        if (pietra > 0) tipiRisorse++;
        if (ferro > 0) tipiRisorse++;
        if (oro > 0) tipiRisorse++;
        if (diamBlu > 0) tipiRisorse++;
        if (diamViola > 0) tipiRisorse++;
        if (tipiRisorse == 0) return risultato;
        double capacitàPerRisorsa = capacitàCarico / tipiRisorse; // Dividi equamente la capacità tra i tipi di risorse disponibili

        // FASE 1: Distribuisci equamente (dalle più leggere alle più pesanti)
        if (cibo > 0)
        {
            risultato.Cibo = (int)Math.Min(cibo, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Cibo);
            capacitàCarico -= risultato.Cibo * Variabili_Server.peso_Risorse_Cibo;
        }
        if (legno > 0)
        {
            risultato.Legno = (int)Math.Min(legno, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Legno);
            capacitàCarico -= risultato.Legno * Variabili_Server.peso_Risorse_Legno;
        }
        if (pietra > 0)
        {
            risultato.Pietra = (int)Math.Min(pietra, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Pietra);
            capacitàCarico -= risultato.Pietra * Variabili_Server.peso_Risorse_Pietra;
        }
        if (ferro > 0)
        {
            risultato.Ferro = (int)Math.Min(ferro, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Ferro);
            capacitàCarico -= risultato.Ferro * Variabili_Server.peso_Risorse_Ferro;
        }
        if (exp > 0) risultato.Esperienza = (int)Math.Min(exp, 0);
        if (oro > 0)
        {
            risultato.Oro = (int)Math.Min(oro, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Oro);
            capacitàCarico -= risultato.Oro * Variabili_Server.peso_Risorse_Oro;
        }
        if (diamBlu > 0)
        {
            risultato.Diamanti_Blu = (int)Math.Min(diamBlu, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Diamante_Blu);
            capacitàCarico -= risultato.Diamanti_Blu * Variabili_Server.peso_Risorse_Diamante_Blu;
        }
        if (diamViola > 0)
        {
            risultato.Diamanti_Viola = (int)Math.Min(diamViola, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Diamante_Viola);
            capacitàCarico -= risultato.Diamanti_Viola * Variabili_Server.peso_Risorse_Diamante_Viola;
        }
        // FASE 2: Cicla finché c'è spazio disponibile e risorse da raccogliere
        bool haRaccolto = true;
        while (capacitàCarico >= Variabili_Server.peso_Risorse_Cibo && haRaccolto) // Minimo peso è 3
        {
            haRaccolto = false;

            if (cibo > risultato.Cibo && capacitàCarico >= Variabili_Server.peso_Risorse_Cibo)
            {
                int extra = (int)Math.Min(cibo - risultato.Cibo, capacitàCarico / Variabili_Server.peso_Risorse_Cibo);
                if (extra > 0)
                {
                    risultato.Cibo += extra;
                    capacitàCarico -= extra * Variabili_Server.peso_Risorse_Cibo;
                    haRaccolto = true;
                }
            }
            if (legno > risultato.Legno && capacitàCarico >= Variabili_Server.peso_Risorse_Legno)
            {
                int extra = (int)Math.Min(legno - risultato.Legno, capacitàCarico / Variabili_Server.peso_Risorse_Legno);
                if (extra > 0)
                {
                    risultato.Legno += extra;
                    capacitàCarico -= extra * Variabili_Server.peso_Risorse_Legno;
                    haRaccolto = true;
                }
            }
            if (pietra > risultato.Pietra && capacitàCarico >= Variabili_Server.peso_Risorse_Pietra)
            {
                int extra = (int)Math.Min(pietra - risultato.Pietra, capacitàCarico / Variabili_Server.peso_Risorse_Pietra);
                if (extra > 0)
                {
                    risultato.Pietra += extra;
                    capacitàCarico -= extra * Variabili_Server.peso_Risorse_Pietra;
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
    } // Distribuzione equa

    // ═══════════════════════════════════════════════════════════════
    // PRE-BATTAGLIA A DISTANZA - Arcieri e Catapulte attaccano per primi
    // ═══════════════════════════════════════════════════════════════
    private static BattleResult EseguiBattaglia(UnitGroup playerUnits, UnitGroup enemyUnits, Player player, Guid clientGuid, int livello)
    {
        var result = new BattleResult
        {
            AttaccanteSopravvisuti = playerUnits.Clone(),
            DifensoreSopravvisuti = enemyUnits.Clone(),
            AttaccantePerdite = new UnitGroup(),
            DifensorePerdite = new UnitGroup()
        };

        // Conta tipi di unità
        int playerUnitTypes = playerUnits.CountUnitTypes();
        int enemyUnitTypes = enemyUnits.CountUnitTypes();

        int truppeNemico = enemyUnits.Guerrieri.Sum() + enemyUnits.Lancieri.Sum() + enemyUnits.Arcieri.Sum() + enemyUnits.Catapulte.Sum();
        bool Frecce = false;
        if (truppeNemico > 0) Frecce = true;
        // Calcola danno
        result.EnemyDamage = CalcolaDannoNemico(enemyUnits);
        result.PlayerDamage = CalcolaDannoGiocatore(playerUnits, player, clientGuid, Frecce, true, result);

        // Distribuisci il danno tra i tipi di unità
        double dannoPerTipoPlayer = result.EnemyDamage / playerUnitTypes;
        double dannoPerTipoEnemy = result.PlayerDamage / enemyUnitTypes;

        var resutl = ApplicaDanniGiocatore(result, playerUnits, player, dannoPerTipoPlayer, 1, true); // Applica danni al giocatore
        ApplicaDanniNemico(result, dannoPerTipoEnemy); // Applica danni al nemico
        result.Xp_Attacker = CalcolaEsperienza(result.DifensorePerdite); // Calcola esperienza
        result.Victory = result.DifensoreSopravvisuti.TotalUnits() == 0; // Determina vittoria
        return result;
    }
    private static RangedBattleResult EseguiPreBattaglia(UnitGroup playerUnits, UnitGroup enemyUnits, Player player, Guid clientGuid)
    {
        var result = new RangedBattleResult
        {
            AttaccanteUnitsAfter = playerUnits.Clone(),
            DifensoreUnitsAfter = enemyUnits.Clone()
        };

        // ATTACCO DEL GIOCATORE
        var playerRangedAttack = CalcolaAttaccoDistanza(playerUnits, player, clientGuid, true);
        result.ArrowsUsedAttacker = playerRangedAttack.FrecceUsate;
        result.PlayerHadLowArrows = playerRangedAttack.FrecceInsufficienti;

        // Applica danni e ottieni morti per livello
        var playerMorti = ApplicaDanniDistanza(result.DifensoreUnitsAfter, playerRangedAttack.DannoGuerrieri, playerRangedAttack.DannoLancieri);

        // Estrai i morti per tipo
        Array.Copy(playerMorti, 0, result.AttackerKills_Guerrieri, 0, 5);
        Array.Copy(playerMorti, 5, result.AttackerKills_Lancieri, 0, 5);

        // CONTRATTACCO DEL NEMICO
        int totalEnemyRanged = enemyUnits.Arcieri.Sum() + enemyUnits.Catapulte.Sum();
        if (totalEnemyRanged > 0)
        {
            var enemyRangedAttack = CalcolaAttaccoDistanzaNemico(result.DifensoreUnitsAfter, clientGuid );
            result.ArrowsUsedAttacker = enemyRangedAttack.FrecceUsate;
            var enemyMorti = ApplicaDanniDistanza(result.AttaccanteUnitsAfter, enemyRangedAttack.DannoGuerrieri, enemyRangedAttack.DannoLancieri); // Applica danni e ottieni morti per livello

            Array.Copy(enemyMorti, 0, result.DefenderKills_Guerrieri, 0, 5);
            Array.Copy(enemyMorti, 5, result.DefenderKills_Lancieri, 0, 5);
        }

        // Calcola esperienza con i dati per livello
        result.Xp_Attacker = CalcolaEsperienzaDistanza(result);
        return result;
    }
    private static RangedAttackResult CalcolaAttaccoDistanza(UnitGroup units, Player player, Guid clientGuid, bool isPlayer)
    {
        var result = new RangedAttackResult();
        int totaleArchieri = units.Arcieri.Sum();
        int totaleCatapulte = units.Catapulte.Sum();

        if (totaleArchieri == 0 && totaleCatapulte == 0)
        {
            SendClient(clientGuid, "Log_Server|Nessuna unità a distanza disponibile.");
            return result;
        }

        // Calcola frecce necessarie
        int frecceNecessarie = CalcoloFrecce(units);

        // Calcola potenza d'attacco base
        int arcieriEffettivi = totaleArchieri * 3 / 7;
        int catapulteEffettive = totaleCatapulte * 3 / 6;

        // Bonus per poche unità (sono più precise)
        if (totaleArchieri > 0 && totaleArchieri <= 15) arcieriEffettivi = totaleArchieri * 3 / 6;
        if (totaleCatapulte > 0 && totaleCatapulte <= 10) catapulteEffettive = totaleCatapulte * 3 / 5;

        if (isPlayer && player.Frecce < frecceNecessarie) // Gestione frecce insufficienti
        {
            arcieriEffettivi /= 4;
            catapulteEffettive /= 5;
            player.Frecce_Utilizzate += (int)player.Frecce;
            result.FrecceUsate = (int)player.Frecce;
            player.Frecce = 0;

            result.FrecceUsate = (int)player.Frecce;
            result.FrecceInsufficienti = true;
        }
        else if (isPlayer)
        {
            player.Frecce -= frecceNecessarie;
            player.Frecce_Utilizzate += frecceNecessarie;
            result.FrecceUsate = frecceNecessarie;
        }

        int attaccoTotale = (arcieriEffettivi + catapulteEffettive); // Calcola danno totale
        if (attaccoTotale > 0)
        {
            // Distribuisci il danno tra guerrieri e lancieri (2/3 al tipo più numeroso)
            result.DannoGuerrieri = attaccoTotale * 3 / 5;
            result.DannoLancieri = attaccoTotale * 2 / 5;
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
        int arcieriEffettivi = totaleArchieri * 3 / 6;
        int catapulteEffettive = totaleCatapulte * 3 / 5;

        // Bonus per poche unità
        if (totaleArchieri > 0 && totaleArchieri <= 10) arcieriEffettivi = totaleArchieri * 3 / 4;
        if (totaleCatapulte > 0 && totaleCatapulte <= 5) catapulteEffettive = totaleCatapulte * 3 / 4;

        int attaccoTotale = (arcieriEffettivi + catapulteEffettive) * 4 / 5;
        if (attaccoTotale > 0)
        {
            result.DannoGuerrieri = attaccoTotale * 2 / 5;
            result.DannoLancieri = attaccoTotale * 2 / 5;
        }
        result.FrecceUsate = CalcoloFrecce(enemyUnits);
        return result;
    }
    private static int[] ApplicaDanniDistanza(UnitGroup units, int dannoGuerrieri, int dannoLancieri)
    {
        int[] mortiGuerrieri = new int[5], mortiLancieri = new int[5];
        int dannoRimanente = dannoGuerrieri;

        for (int i = 0; i < 5 && dannoRimanente > 0; i++) // Applica danni ai guerrieri (dal livello più basso al più alto)
            if (units.Guerrieri[i] > 0)
            {
                int morti = Math.Min(units.Guerrieri[i], dannoRimanente);
                units.Guerrieri[i] -= morti;
                mortiGuerrieri[i] = morti;
                dannoRimanente -= morti;
            }
        dannoRimanente = dannoLancieri;
        for (int i = 0; i < 5 && dannoRimanente > 0; i++) // Applica danni ai lancieri (dal livello più basso al più alto)
            if (units.Lancieri[i] > 0)
            {
                int morti = Math.Min(units.Lancieri[i], dannoRimanente);
                units.Lancieri[i] -= morti;
                mortiLancieri[i] = morti;
                dannoRimanente -= morti;
            }

        // Restituisce un array concatenato: [5 guerrieri, 5 lancieri]
        int[] tuttiIMorti = new int[10];
        Array.Copy(mortiGuerrieri, 0, tuttiIMorti, 0, 5);
        Array.Copy(mortiLancieri, 0, tuttiIMorti, 5, 5);
        return tuttiIMorti;
    }
    private static BattleResult ApplicaDanniGiocatore(BattleResult battle, UnitGroup units, Player player, double dannoPerTipo, double bonusUnità, bool attacco)
    {
        for (int i = 0; i < 5; i++)
        {
            var stats = GetPlayerUnitStats(i, player); //Qui ci sono stats unità con bonus applicati

            // Guerrieri
            int guerrieriIniziali = units.Guerrieri[i];
            units.Guerrieri[i] = RidurreNumeroSoldati(guerrieriIniziali, dannoPerTipo, stats.GuerrieriDifesa * guerrieriIniziali * bonusUnità, stats.GuerrieriSalute * bonusUnità);
            if (attacco == true)
            {
                battle.AttaccanteSopravvisuti.Guerrieri[i] = units.Guerrieri[i];
                battle.AttaccantePerdite.Guerrieri[i] += guerrieriIniziali - units.Guerrieri[i];
            }
            if (attacco == false)
            {
                battle.DifensoreSopravvisuti.Guerrieri[i] = units.Guerrieri[i];
                battle.DifensorePerdite.Guerrieri[i] += guerrieriIniziali - units.Guerrieri[i];
            }

            // Lancieri
            int lancieriIniziali = units.Lancieri[i];
            units.Lancieri[i] = RidurreNumeroSoldati(lancieriIniziali, dannoPerTipo, stats.LancieriDifesa * lancieriIniziali * bonusUnità, stats.LancieriSalute * bonusUnità);
            if (attacco == true)
            {
                battle.AttaccanteSopravvisuti.Lancieri[i] = units.Lancieri[i];
                battle.AttaccantePerdite.Lancieri[i] += lancieriIniziali - units.Lancieri[i];
            }
            if (attacco == false)
            {
                battle.DifensoreSopravvisuti.Lancieri[i] = units.Guerrieri[i];
                battle.DifensorePerdite.Lancieri[i] += lancieriIniziali - units.Lancieri[i];
            }

            // Arcieri
            int arcieriIniziali = units.Arcieri[i];
            units.Arcieri[i] = RidurreNumeroSoldati(arcieriIniziali, dannoPerTipo, stats.ArcieriDifesa * arcieriIniziali * bonusUnità, stats.ArcieriSalute * bonusUnità);
            if (attacco == true)
            {
                battle.AttaccanteSopravvisuti.Arcieri[i] = units.Arcieri[i];
                battle.AttaccantePerdite.Arcieri[i] += arcieriIniziali - units.Arcieri[i];
            }
            if (attacco == false)
            {
                battle.DifensoreSopravvisuti.Arcieri[i] = units.Arcieri[i];
                battle.DifensorePerdite.Arcieri[i] += arcieriIniziali - units.Arcieri[i];
            }

            // Catapulte
            int catapulteIniziali = units.Catapulte[i];
            units.Catapulte[i] = RidurreNumeroSoldati(catapulteIniziali, dannoPerTipo, stats.CatapulteDifesa * catapulteIniziali * bonusUnità, stats.CatapulteSalute * bonusUnità);
            if (attacco == true)
            {
                battle.AttaccanteSopravvisuti.Catapulte[i] = units.Catapulte[i];
                battle.AttaccantePerdite.Catapulte[i] += catapulteIniziali - units.Catapulte[i];
            }
            if (attacco == false)
            {
                battle.DifensoreSopravvisuti.Catapulte[i] = units.Catapulte[i];
                battle.DifensorePerdite.Catapulte[i] += catapulteIniziali - units.Catapulte[i];
            }
        }
        return battle;
    }
    private static void ApplicaDanniNemico(BattleResult result, double dannoPerTipo)
    {
        for (int i = 0; i < 5; i++)
        {
            var stats = GetEnemyUnitStats(i);

            // Guerrieri
            int guerrieriIniziali = result.DifensoreSopravvisuti.Guerrieri[i];
            result.DifensoreSopravvisuti.Guerrieri[i] = RidurreNumeroSoldati(guerrieriIniziali, dannoPerTipo, stats.GuerrieriDifesa * guerrieriIniziali, stats.GuerrieriSalute);
            result.DifensorePerdite.Guerrieri[i] = guerrieriIniziali - result.DifensoreSopravvisuti.Guerrieri[i];

            // Lancieri
            int lancieriIniziali = result.DifensoreSopravvisuti.Lancieri[i];
            result.DifensoreSopravvisuti.Lancieri[i] = RidurreNumeroSoldati(lancieriIniziali, dannoPerTipo, stats.LancieriDifesa * lancieriIniziali, stats.LancieriSalute);
            result.DifensorePerdite.Lancieri[i] = lancieriIniziali - result.DifensoreSopravvisuti.Lancieri[i];

            // Arcieri
            int arcieriIniziali = result.DifensoreSopravvisuti.Arcieri[i];
            result.DifensoreSopravvisuti.Arcieri[i] = RidurreNumeroSoldati(arcieriIniziali, dannoPerTipo, stats.ArcieriDifesa * arcieriIniziali, stats.ArcieriSalute);
            result.DifensorePerdite.Arcieri[i] = arcieriIniziali - result.DifensoreSopravvisuti.Arcieri[i];

            // Catapulte
            int catapulteIniziali = result.DifensoreSopravvisuti.Catapulte[i];
            result.DifensoreSopravvisuti.Catapulte[i] = RidurreNumeroSoldati(catapulteIniziali, dannoPerTipo, stats.CatapulteDifesa * catapulteIniziali, stats.CatapulteSalute);
            result.DifensorePerdite.Catapulte[i] = catapulteIniziali - result.DifensoreSopravvisuti.Catapulte[i];
        }
    }
    private static int RidurreNumeroSoldati(int numeroSoldati, double danno, double difesa, double salutePerSoldato)
    {
        if (numeroSoldati == 0 || salutePerSoldato == 0) return 0;
        double dannoEffettivo = Math.Max(0, danno - difesa);
        int soldatiPersi = (int)Math.Ceiling(dannoEffettivo / salutePerSoldato);
        return Math.Max(0, numeroSoldati - soldatiPersi);
    }
    private static double CalcolaDannoGiocatore(UnitGroup units, Player player, Guid clientGuid, bool usaFrecce, bool attaccante , BattleResult result)
    {
        double dannoTotale = 0, moltiplicatoreDistanza = 1.0;
        int frecceNecessarie = CalcoloFrecce(units);

        if (usaFrecce)
            if (player.Frecce < frecceNecessarie)
            {
                moltiplicatoreDistanza = 0.33;
                player.Frecce_Utilizzate += (int)player.Frecce;
                if (attaccante)
                {
                    result.FrecceNecessarieAttacker = (int)player.Frecce;
                    result.PocheFrecceAttacker = true;
                }
                else
                {
                    result.FrecceNecessarieDefender = (int)player.Frecce;
                    result.PocheFrecceDefender = true;
                }
                player.Frecce = 0;
            }
            else
            {
                player.Frecce -= frecceNecessarie;
                player.Frecce_Utilizzate += (int)frecceNecessarie;
                if (attaccante)
                {
                    result.FrecceNecessarieAttacker = frecceNecessarie;
                    result.PocheFrecceAttacker = true;
                }
                else
                {
                    result.FrecceNecessarieDefender = frecceNecessarie;
                    result.PocheFrecceDefender = true;
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
    private static int CalcolaEsperienzaDistanza(RangedBattleResult result)
    {
        int exp = 0;
        for (int i = 0; i < 5; i++)
        {
            var stats = GetEnemyUnitStats(i);
            exp += result.AttackerKills_Guerrieri[i] * stats.GuerrieriEsperienza;
            exp += result.AttackerKills_Lancieri[i] * stats.LancieriEsperienza;
        }
        return exp;
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
    private static void InviaLogBattaglia_PvP(Guid attackerGuid, Guid defenderGuid, Player attaccante, Player difensore, BattleResult result,UnitGroup attackerUnitsOriginali, UnitGroup defenderUnitsOriginali)
    {
        // Log per l'attaccante
        SendClient(attackerGuid, "Log_Server|╔══════════════════════════════════════════════════╗");
        SendClient(attackerGuid, $"Log_Server|║     [highlight]RESOCONTO BATTAGLIA [verde]vs [highlight]{difensore.Username,-25} - Fase: {result.Struttura} ║");
        SendClient(attackerGuid, "Log_Server|╚══════════════════════════════════════════════════╝\n");

        // Fase distanza
        if (result.FaseDistanza != null)
        {
            SendClient(attackerGuid, "Log_Server|------------------ FASE 'I' A [highlight]DISTANZA[/highlight] ------------------");
            if (result.FaseDistanza.Unità_Presenti == false)// Controlliamo se presenti unità
                SendClient(attackerGuid, $"Log_Server|[highlight]{difensore.Username}[/highlight] non ha unità presenti in difesa...");
            else
            {
                SendClient(attackerGuid, $"Log_Server|[highlight]Frecce utilizzate: [error][icon:frecce]{result.FaseDistanza.ArrowsUsedAttacker}");
                SendClient(attackerGuid, $"Log_Server|[highlight]Nemici eliminati: [verde]{result.FaseDistanza.GetTotalPlayerKills()} [highlight]unità.");
                for (int i = 0; i < 5; i++)
                {
                    int g = result.FaseDistanza.AttackerKills_Guerrieri[i];
                    int l = result.FaseDistanza.AttackerKills_Lancieri[i];
                    if (g > 0 || l > 0)
                        SendClient(attackerGuid, $"Log_Server|[highlight]Lv{i + 1}: [verde]{g} [highlight]Guerrieri + [verde]{l} [highlight]Lanceri");
                }
                SendClient(attackerGuid, $"Log_Server|[highlight]Tue perdite: [error]{result.FaseDistanza.GetTotalEnemyKills()} [highlight]unità.");
                for (int i = 0; i < 5; i++)
                {
                    int g = result.FaseDistanza.DefenderKills_Guerrieri[i];
                    int l = result.FaseDistanza.DefenderKills_Lancieri[i];
                    if (g > 0 || l > 0)
                        SendClient(attackerGuid, $"Log_Server|[highlight]Lv{i + 1}: [error]{g} [highlight]Guerrieri + [error]{l} [highlight]Lanceri");
                }
            }
            SendClient(attackerGuid, "Log_Server|");
        }

        SendClient(attackerGuid, "Log_Server|------------------ FASE 'II' [highlight]CORPO A CORPO[/highlight] ------------------");
        if (result.PocheFrecceAttacker) SendClient(attackerGuid, $"Log_Server|Riduzione danno [highlight]{attaccante.Username}[/highlight] per mancanza di frecce: [error][icon:frecce]{result.FrecceNecessarieAttacker}[/error]/[verde]{attaccante.Frecce}");
        else SendClient(attackerGuid, $"Log_Server|[highlight]Frecce utilizzate: [error][icon:frecce]{result.ArrowsUsedAttacker}");
        SendClient(attackerGuid, $"Log_Server|[highlight]Danno inflitto: [verde]{result.PlayerDamage:F2}");
        SendClient(attackerGuid, $"Log_Server|[highlight]Danno subito: [error]{result.EnemyDamage:F2}\n");

        if (result.AttaccantePerdite.TotalUnits() > 0)
        {
            SendClient(attackerGuid, $"Log_Server|[highlight]Perdite giocatore: [verde]{attaccante.Username}");
            LogPerdite(attackerGuid, result.AttaccantePerdite, attackerUnitsOriginali);
        }
        else SendClient(attackerGuid, $"Log_Server|[verde]{attaccante.Username} nessuna perdita subita!");
        if (result.DifensorePerdite.TotalUnits() > 0)
        {
            SendClient(attackerGuid, $"Log_Server|[highlight]Perdite giocatore: [error]{difensore.Username}:");
            LogPerdite(attackerGuid, result.DifensorePerdite, defenderUnitsOriginali);
        }
        else SendClient(attackerGuid, $"Log_Server|[error]{difensore.Username} nessuna perdita subita!");
        SendClient(attackerGuid, "Log_Server|");

        int expAttaccante = CalcolaEsperienzaPVP(result.DifensorePerdite);
        var defenderTemp = new UnitGroup
        {
            Guerrieri = result.FaseDistanza.AttackerKills_Guerrieri,
            Lancieri = result.FaseDistanza.AttackerKills_Lancieri,
            Arcieri = [0, 0, 0, 0, 0],
            Catapulte = [0, 0, 0, 0, 0]
        };
        expAttaccante += CalcolaEsperienzaPVP(defenderTemp);
        SendClient(attackerGuid, $"Log_Server|[highlight]Esito: {(result.Victory ? "[verde]VITTORIA!" : "[error]SCONFITTA")}");
        SendClient(attackerGuid, $"Log_Server|[highlight]Esperienza guadagnata: [verde]{expAttaccante}XP");
        if (result.Victory == true)
        {
            if (result.Struttura == "Ingresso")
                SendClient(attackerGuid, $"Log_Server|La battaglia inizia nella vallata l' '[highlight]{result.Struttura}[/highlight]' è il primo punto difensivo di [highlight]{difensore.Username}[/highlight]. Nonostante la ferocia difesa le tue truppe sono riuscite ad avanzare verso le [highlight]mura[/highlight] di [highlight]{difensore.Username}[/highlight].");

            if (result.Struttura == "Mura" && result.Salute == 0)
                SendClient(attackerGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa[/blu] delle [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Le [highlight]{result.Struttura}[/highlight] sono [highlight]crollate[/highlight]... Tutte le unità al suo interno sono decedute.");
            if (result.Struttura == "Mura" && result.Salute > 0)
                SendClient(attackerGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa[/blu] delle [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Le [highlight]{result.Struttura}[/highlight] hanno resistito, ma le tue truppe sono riuscite ad avanzare verso il [highlight]cancello[/highlight] di [highlight]{difensore.Username}[/highlight].");

            if (result.Struttura == "Cancello" && result.Salute == 0)
                SendClient(attackerGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa[/blu] del [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Il [highlight]{result.Struttura}/[highlight] è [highlight]crollato[/highlight]... Tutte le unità al suo interno sono decedute.");
            if (result.Struttura == "Cancello" && result.Salute > 0)
                SendClient(attackerGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa[/blu] del [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Il [highlight]{result.Struttura}[/highlight] ha resistito, ma le tue truppe sono riuscite ad avanzare verso le [highlight]torri[/highlight] di [highlight]{difensore.Username}[/highlight].");

            if (result.Struttura == "Torri" && result.Salute == 0)
                SendClient(attackerGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa[/blu] delle [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Le [highlight]{result.Struttura}[/highlight] sono [highlight]crollate[/highlight]... Tutte le unità al suo interno sono decedute.");
            if (result.Struttura == "Torri" && result.Salute > 0)
                SendClient(attackerGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa[/blu] delle [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Le [highlight]{result.Struttura}[/highlight] ha resistito, ma le tue truppe sono riuscite ad avanzare verso il [highlight]centro villaggio[/highlight] di [highlight]{difensore.Username}[/highlight].");
            
            if (result.Struttura == "Centro Villaggio")
                SendClient(attackerGuid, $"Log_Server|Il [highlight]{result.Struttura}[/highlight] ha combattuto fino alla fine, ma le tue truppe sono riuscite ad avanzare verso il [highlight]castello[/highlight] di [highlight]{difensore.Username}[/highlight].");

            if (result.Struttura == "Castello" && result.Salute == 0)
                SendClient(attackerGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa[/blu] del [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Il [highlight]{result.Struttura}[/highlight] è [highlight]crollato[/highlight]... Tutte le unità al suo interno sono decedute.");
            if (result.Struttura == "Castello" && result.Salute > 0)
                SendClient(attackerGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa[/blu] del [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Il [highlight]{result.Struttura}[/highlight] ha resistito, ma le tue truppe sono riuscite ad avanzare verso il resto del [highlight]villaggio[/highlight] di [highlight]{difensore.Username}[/highlight]... \n[highlight] L'ultima battaglia[/highlight], prima di poter saccheggiare le risorse!!");
        }

        // Log per il difensore (speculare)
        SendClient(defenderGuid, "Log_Server|╔══════════════════════════════════════════════════╗");
        SendClient(defenderGuid, $"Log_Server|║     [error]ATTACCO SUBITO [highlight]da {attaccante.Username,-13}[/highlight] Fase: [highlight]{result.Struttura}[/highlight] ║");
        SendClient(defenderGuid, "Log_Server|╚══════════════════════════════════════════════════╝\n");

        if (result.FaseDistanza != null)
        {
            SendClient(defenderGuid, "Log_Server|------------------ FASE 'I' A [highlight]DISTANZA[/highlight] ------------------");
            if (result.FaseDistanza.Unità_Presenti == false)// Controlliamo se presenti unità
                SendClient(defenderGuid, $"Log_Server|Non hai unità presenti in difesa...");
            else
            {
                SendClient(defenderGuid, $"Log_Server|[highlight]Frecce utilizzate: [error][icon:frecce]{result.FaseDistanza.ArrowsUsedAttacker}");
                SendClient(defenderGuid, $"Log_Server|[highlight]Nemici eliminati: [verde]{result.FaseDistanza.GetTotalEnemyKills()} unità.");
                for (int i = 0; i < 5; i++)
                {
                    int g = result.FaseDistanza.AttackerKills_Guerrieri[i];
                    int l = result.FaseDistanza.AttackerKills_Lancieri[i];
                    if (g > 0 || l > 0)
                        SendClient(defenderGuid, $"Log_Server|[highlight]Lv{i + 1}: [verde]{g} [highlight]Guerrieri + [verde]{l} [highlight]Lanceri");
                }
                SendClient(defenderGuid, $"Log_Server|[highlight]Tue perdite: [error]{result.FaseDistanza.GetTotalPlayerKills()} [highlight]unità.\n");
                for (int i = 0; i < 5; i++)
                {
                    int g = result.FaseDistanza.DefenderKills_Guerrieri[i];
                    int l = result.FaseDistanza.DefenderKills_Lancieri[i];
                    if (g > 0 || l > 0)
                        SendClient(defenderGuid, $"Log_Server|[highlight]Lv{i + 1}: [error]{g} [highlight]Guerrieri + [error]{l} [highlight]Lanceri");
                }
            }
        }

        SendClient(defenderGuid, "Log_Server|------------------ FASE 'II' [highlight]CORPO A CORPO[/highlight] ------------------");
        if (result.PocheFrecceDefender) SendClient(defenderGuid, $"Log_Server|Riduzione danno [highlight]{difensore.Username}[/highlight] per mancanza di frecce: [error][icon:frecce]{result.FrecceNecessarieDefender}[/error]/[verde]{difensore.Frecce}");
        else SendClient(defenderGuid, $"Log_Server|[highlight]Frecce utilizzate: [error][icon:frecce]{result.ArrowsUsedDefender}");
        SendClient(defenderGuid, $"Log_Server|[highlight]Danno inflitto: [verde]{result.EnemyDamage:F2} [highlight]([verde]+20% [highlight]bonus difesa)");
        SendClient(defenderGuid, $"Log_Server|[highlight]Danno subito: [error]{result.PlayerDamage:F2}\n");

        if (result.DifensorePerdite.TotalUnits() > 0)
        {
            SendClient(defenderGuid, $"Log_Server|[highlight]Perdite giocatore: [error]{difensore.Username}");
            LogPerdite(defenderGuid, result.DifensorePerdite, defenderUnitsOriginali);
        }
        else SendClient(defenderGuid, "Log_Server|[verde]Nessuna perdita subita!");
        if (result.AttaccantePerdite.TotalUnits() > 0)
        {
            SendClient(defenderGuid, $"Log_Server|[highlight]Perdite giocatore: [verde]{attaccante.Username}");
            LogPerdite(defenderGuid, result.AttaccantePerdite, attackerUnitsOriginali);
        }
        else SendClient(defenderGuid, "Log_Server|[error]Nessuna perdita subita!");
        SendClient(attackerGuid, "Log_Server|");

        int expDifensore = CalcolaEsperienzaPVP(result.AttaccantePerdite);
        var attackerTemp = new UnitGroup
        {
            Guerrieri = result.FaseDistanza.DefenderKills_Guerrieri,
            Lancieri = result.FaseDistanza.DefenderKills_Lancieri,
            Arcieri = [0, 0, 0, 0, 0],
            Catapulte = [0, 0, 0, 0, 0]
        };
        expDifensore += CalcolaEsperienzaPVP(attackerTemp);
        SendClient(defenderGuid, $"Log_Server|\n[highlight]Esperienza guadagnata: [verde]{expDifensore}XP");
        SendClient(defenderGuid, $"Log_Server|[highlight]Esito: {(!result.Victory ? $"[verde]Difesa [highlight]{result.Struttura} riuscita!" : $"[error]Difesa fallita!: [highlight]{result.Struttura}")}");
        if (result.Victory == true)
        {
            if (result.Struttura == "Ingresso")
                SendClient(defenderGuid, $"Log_Server|La battaglia inizia nella vallata l' '[highlight]{result.Struttura}[/highlight]' è il primo punto difensivo di [highlight]{difensore.Username}[/highlight]. Nonostante la ferocia difesa le tue truppe sono riuscite ad avanzare verso le tue [highlight]mura[/highlight].");

            if (result.Struttura == "Mura" && result.Salute == 0)
                SendClient(defenderGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa [/blu]delle [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Le [highlight]{result.Struttura}[/highlight] sono [highlight]crollate[/highlight]... Tutte le unità al suo interno sono decedute.");
            if (result.Struttura == "Mura" && result.Salute > 0)
                SendClient(defenderGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa [/blu]delle [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Le [highlight]{result.Struttura}[/highlight] hanno resistito, ma le tue truppe sono riuscite ad avanzare verso il [highlight]cancello[/highlight].");

            if (result.Struttura == "Cancello" && result.Salute == 0)
                SendClient(defenderGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa [/blu]del [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Il [highlight]{result.Struttura}[/highlight] è [highlight]crollato[/highlight]... Tutte le unità al suo interno sono decedute.");
            if (result.Struttura == "Cancello" && result.Salute > 0)
                SendClient(defenderGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa [/blu]del [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Il [highlight]{result.Struttura}[/highlight] ha resistito, ma le tue truppe sono riuscite ad avanzare verso le [highlight]torri[/highlight].");

            if (result.Struttura == "Torri" && result.Salute == 0)
                SendClient(defenderGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa [/blu]delle [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Le [highlight]{result.Struttura}[/highlight] sono [highlight]crollate[highlight]... Tutte le unità al suo interno sono decedute.");
            if (result.Struttura == "Torri" && result.Salute > 0)
                SendClient(defenderGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa [/blu]delle [highlight]{result.Struttura}[/highlight] è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Le [highlight]{result.Struttura}[/highlight] ha resistito, ma le tue truppe sono riuscite ad avanzare verso il centro [highlight]villaggio[/highlight].");

            if (result.Struttura == "Centro Villaggio")
                SendClient(defenderGuid, $"Log_Server|Il [highlight]{result.Struttura}[/highlight] ha combattuto fino alla fine, ma le tue truppe sono riuscite ad avanzare verso il castello.");

            if (result.Struttura == "Castello" && result.Salute == 0)
                SendClient(defenderGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa [blu]del [highlight]{result.Struttura} è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Il [highlight]{result.Struttura}[/highlight] è [highlight]crollato[/highlight]... Tutte le unità al suo interno sono decedute.");
            if (result.Struttura == "Castello" && result.Salute > 0)
                SendClient(defenderGuid, $"Log_Server|La [verde]salute[/verde] e la [blu]difesa [/blu]del [highlight]{result.Struttura} è: [verde]{result.Salute}[/verde] e [blu]{result.Difesa}[/blu]. Il [highlight]{result.Struttura}[/highlight] ha resistito, ma le tue truppe sono riuscite ad avanzare verso il resto del villaggio... \n [highlight]L'ultima battaglia decisiva[/highlight], le tue risorse sono a rischio!!");
        }
    }
    private static void InviaLogBattaglia(Guid clientGuid, Player player, BattleResult result, string tipo, UnitGroup unitàGiocatore, UnitGroup unitàNemico)
    {
        SendClient(clientGuid, "Log_Server|");
        SendClient(clientGuid, $"Log_Server|╔══════════════════════════════════════════════════╗");
        SendClient(clientGuid, $"Log_Server|║        [highlight]RESOCONTO BATTAGLIA [verde]vs [highlight]{tipo}            ║");
        SendClient(clientGuid, $"Log_Server|╚══════════════════════════════════════════════════╝\n");

        // Log fase a distanza se presente
        if (result.FaseDistanza != null)
        {
            SendClient(clientGuid, $"Log_Server|------------------ [highlight]FASE [title]I [highlight]A DISTANZA ------------------");
            SendClient(clientGuid, $"Log_Server|[highlight]Frecce utilizzate: [error]{result.FaseDistanza.ArrowsUsedAttacker}");

            // ✨ NUOVO: Log dettagliato per livello
            SendClient(clientGuid, $"Log_Server|[highlight]Nemici eliminati:");
            int totGuerrieri = 0, totLancieri = 0;
            for (int i = 0; i < 5; i++)
            {
                int g = result.FaseDistanza.AttackerKills_Guerrieri[i];
                int l = result.FaseDistanza.AttackerKills_Lancieri[i];
                if (g > 0 || l > 0)
                {
                    SendClient(clientGuid, $"Log_Server|[highlight]Lv{i + 1}: [error]{g} [highlight]Guerrieri + [error]{l} [highlight]Lanceri");
                    totGuerrieri += g;
                    totLancieri += l;
                }
            }
            SendClient(clientGuid, $"Log_Server|[highlight]TOT: [error]{totGuerrieri} [highlight]Guerrieri + [error]{totLancieri} [highlight]Lanceri");

            SendClient(clientGuid, $"Log_Server|\n[highlight]Perdite giocatore:");
            int totTueG = 0, totTueL = 0;
            for (int i = 0; i < 5; i++)
            {
                int g = result.FaseDistanza.DefenderKills_Guerrieri[i];
                int l = result.FaseDistanza.DefenderKills_Lancieri[i];
                if (g > 0 || l > 0)
                {
                    SendClient(clientGuid, $"Log_Server|[highlight]Lv{i + 1}: [error]{g} [highlight]Guerrieri + [error]{l} [highlight]Lanceri");
                    totTueG += g;
                    totTueL += l;
                }
            }
            SendClient(clientGuid, $"Log_Server|[highlight]TOT: [error]{totTueG} [highlight]Guerrieri + [error]{totTueL} [highlight]Lanceri");
            SendClient(clientGuid, "Log_Server|");
        }
        if (true) // Solo per non cambiare nomi alle variabili g,l,a,c
        {
            SendClient(clientGuid, $"Log_Server|------ [highlight]FASE [tile]II [highlight]CORPO A CORPO ------");
            SendClient(clientGuid, "Log_Server|");
            SendClient(clientGuid, $"Log_Server|[highlight]Danno inflitto: [info]{result.PlayerDamage:F2}");
            SendClient(clientGuid, $"Log_Server|[highlight]Danno subito: [info]{result.EnemyDamage:F2}");

            SendClient(clientGuid, "Log_Server|");
            if (result.AttaccantePerdite.TotalUnits() > 0)
            {
                SendClient(clientGuid, $"Log_Server|[highlight]Perdite giocatore: [info][{player.Username}]");
                LogPerdite(clientGuid, result.AttaccantePerdite, unitàGiocatore);
            }
            else SendClient(clientGuid, $"Log_Server|[highlight]Nessuna perdita subita dal giocatore: [info][{player.Username}]");

            SendClient(clientGuid, "Log_Server|");
            if (result.DifensorePerdite.TotalUnits() > 0)
            {
                SendClient(clientGuid, $"Log_Server|[highlight]Perdite barbaro:");
                LogPerdite(clientGuid, result.DifensorePerdite, unitàNemico);
            }
            else SendClient(clientGuid, $"Log_Server|[highlight]Nessuna perdita subita dal barbaro");

            SendClient(clientGuid, "Log_Server|");
            SendClient(clientGuid, $"Log_Server|[highlight]Esperienza TOTALE guadagnata: [title]{result.Xp_Attacker}");
            SendClient(clientGuid, $"Log_Server|[highlight]Esito: {(result.Victory ? "[verde]VITTORIA!" : "[error]SCONFITTA")}");
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
            SendClient(clientGuid, $"Log_Server|{catapulteLog}\n");
    }
    private static string FormatUnitLog(string nomeUnita, int[] perdite, int[] sopravvissuti)
    {
        var parts = new System.Collections.Generic.List<string>();
        for (int i = 0; i < 5; i++)
        {
            int totale = sopravvissuti[i];
            if (totale > 0)
                parts.Add($"[highlight]Lv{i + 1}: [error]{perdite[i]}[/error]/[highlight]{totale}");
        }

        if (parts.Count == 0) return string.Empty;
        return $"{nomeUnita,-12} - {string.Join(" - ", parts)}";
    }
    private static void SendClient(Guid clientGuid, string message)
    {
        Server_Strategico.Server.Server.Send(clientGuid, message);
        Console.WriteLine(message.Replace("Log_Server|", ""));
    }

    // ═══════════════════════════════════════════════════════════════
    // PVP - Battaglia tra giocatori
    // ═══════════════════════════════════════════════════════════════

    public static async Task<BattleResult> Battaglia_PvP(Player attaccante, Player difensore, Guid attackerGuid, Guid defenderGuid, int[] guerriero, int[] lancere, int[] arcere, int[] catapulte,
    bool includiPreBattaglia = true)
    {
        int perdite_Attaccante = 0, perdite_Difensore = 0;
        var attackerUnits = new UnitGroup // Assegna unità attaccante
        {
            Guerrieri = guerriero,
            Lancieri = lancere,
            Arcieri = arcere,
            Catapulte = catapulte
        };
        var defenderUnits = new UnitGroup // Carica unità difensore
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
        if (includiPreBattaglia) rangedResult = EseguiPreBattaglia_PvP(attackerUnits, defenderUnits, attaccante, difensore, attackerGuid, defenderGuid);

        var attackerAfterRanged = rangedResult.AttaccanteUnitsAfter;  //Serve solo per avere le truppe aggiorante per l'attacco corpo a corpo...
        var defenderAfterRanged = rangedResult.DifensoreUnitsAfter;   //Serve solo per avere le truppe aggiorante per l'attacco corpo a corpo...

        // FASE 2: Battaglia corpo a corpo
        var result = EseguiBattaglia_PvP(attackerAfterRanged, defenderAfterRanged, attaccante, difensore, attackerGuid, defenderGuid, 0, 0, 0);
        if (result != null) result.FaseDistanza = rangedResult;

        for (int i = 0; i < 5; i++)// Aggiorna stato
        {
            result.AttaccantePerdite.Guerrieri[i] += rangedResult.DefenderKills_Guerrieri[i];
            result.AttaccantePerdite.Lancieri[i] += rangedResult.DefenderKills_Lancieri[i];
            result.DifensorePerdite.Guerrieri[i] += rangedResult.AttackerKills_Guerrieri[i];
            result.DifensorePerdite.Lancieri[i] += rangedResult.AttackerKills_Lancieri[i];
        }
        for (int i = 0; i < 5; i++)// Aggiorna stato attaccante
        {
            //Attaccante con 0 unità? bene fine attaco...
            if (result.AttaccantePerdite.TotalUnits() > 0)
            {
                attaccante.Guerrieri[i] -= result.AttaccanteSopravvisuti.Guerrieri[i];
                attaccante.Lanceri[i] -= result.AttaccanteSopravvisuti.Lancieri[i];
                attaccante.Arceri[i] -= result.AttaccanteSopravvisuti.Arcieri[i];
                attaccante.Catapulte[i] -= result.AttaccanteSopravvisuti.Catapulte[i];
            }

            // Statistiche attaccante
            attaccante.Guerrieri_Eliminati += result.DifensorePerdite.Guerrieri[i];
            attaccante.Lanceri_Eliminati += result.DifensorePerdite.Lancieri[i];
            attaccante.Arceri_Eliminati += result.DifensorePerdite.Arcieri[i];
            attaccante.Catapulte_Eliminate += result.DifensorePerdite.Catapulte[i];

            attaccante.Guerrieri_Persi += result.AttaccantePerdite.Guerrieri[i];
            attaccante.Lanceri_Persi += result.AttaccantePerdite.Lancieri[i];
            attaccante.Arceri_Persi += result.AttaccantePerdite.Arcieri[i];
            attaccante.Catapulte_Perse += result.AttaccantePerdite.Catapulte[i];
        }
        perdite_Attaccante += result.AttaccantePerdite.TotalUnits();
        attaccante.Unità_Eliminate += result.DifensorePerdite.TotalUnits();
        attaccante.Unità_Perse += result.AttaccantePerdite.TotalUnits();
        perdite_Difensore += result.DifensorePerdite.TotalUnits();
        difensore.Unità_Eliminate += result.AttaccantePerdite.TotalUnits();
        difensore.Unità_Perse += result.DifensorePerdite.TotalUnits();

        for (int i = 0; i < 5; i++) // Aggiorna stato difensore
        {
            difensore.Guerrieri_Eliminati += result.AttaccantePerdite.Guerrieri[i];
            difensore.Lanceri_Eliminati += result.AttaccantePerdite.Lancieri[i];
            difensore.Arceri_Eliminati += result.AttaccantePerdite.Arcieri[i];
            difensore.Catapulte_Eliminate += result.AttaccantePerdite.Catapulte[i];

            difensore.Guerrieri_Persi += result.DifensorePerdite.Guerrieri[i];
            difensore.Lanceri_Persi += result.DifensorePerdite.Lancieri[i];
            difensore.Arceri_Persi += result.DifensorePerdite.Arcieri[i];
            difensore.Catapulte_Perse += result.DifensorePerdite.Catapulte[i];
        }

        //Quest
        OnEvent(attaccante, QuestEventType.Uccisioni, "Guerrieri", result.DifensorePerdite.Guerrieri.Sum());
        OnEvent(attaccante, QuestEventType.Uccisioni, "Lanceri", result.DifensorePerdite.Lancieri.Sum());
        OnEvent(attaccante, QuestEventType.Uccisioni, "Arceri", result.DifensorePerdite.Arcieri.Sum());
        OnEvent(attaccante, QuestEventType.Uccisioni, "Catapulte", result.DifensorePerdite.Catapulte.Sum());

        OnEvent(difensore, QuestEventType.Uccisioni, "Guerrieri", result.AttaccantePerdite.Guerrieri.Sum());
        OnEvent(difensore, QuestEventType.Uccisioni, "Lanceri", result.AttaccantePerdite.Lancieri.Sum());
        OnEvent(difensore, QuestEventType.Uccisioni, "Arceri", result.AttaccantePerdite.Arcieri.Sum());
        OnEvent(difensore, QuestEventType.Uccisioni, "Catapulte", result.AttaccantePerdite.Catapulte.Sum());

        // Esperienza
        attaccante.Esperienza += CalcolaEsperienzaPVP(result.DifensorePerdite);
        difensore.Esperienza += CalcolaEsperienzaPVP(result.AttaccantePerdite);

        if (perdite_Difensore > perdite_Attaccante || (perdite_Difensore == 0 && perdite_Attaccante == 0)) // Determina vittoria (l'attaccante vince se causa più perdite del difensore)
            result.Victory = true;
        else result.Victory = false;

        InviaLogBattaglia_PvP(attackerGuid, defenderGuid, attaccante, difensore, result, attackerUnitsOriginali, defenderUnitsOriginali);
        if (result.Victory) AssegnaRisorseVittoria_PvP(attaccante, difensore, attackerGuid, result.AttaccanteSopravvisuti);
        return result;
    }
    public static async Task<BattleResult>Battaglia_Strutture_PvP(Player attaccante, Player difensore, Guid attackerGuid, Guid defenderGuid, UnitGroup attackerUnits, bool PreBattaglia = true)
    {
        bool terminaBattaglia = false;
        BattleResult result = null;
        OnEvent(attaccante, QuestEventType.Battaglie, "Attacca Giocatore", 1);

        for (int struttura = 1; struttura <= 6; struttura++)
        {
            int perdite_Attaccante = 0, perdite_Difensore = 0;
            double salute = 0, difesa = 0;
            bool valido = false;

            var defenderUnits = new UnitGroup
            {
                Guerrieri = [0, 0, 0, 0, 0],
                Lancieri = [0, 0, 0, 0, 0],
                Arcieri = [0, 0, 0, 0, 0],
                Catapulte = [0, 0, 0, 0, 0]
            };
            defenderUnits = CaricaDatiStruttureDifensore(defenderUnits, difensore, struttura);

            if (struttura == 2) { salute = difensore.Salute_Mura; difesa = difensore.Difesa_Mura; }
            if (struttura == 3) { salute = difensore.Salute_Cancello; difesa = difensore.Difesa_Cancello; }
            if (struttura == 4) { salute = difensore.Salute_Torri; difesa = difensore.Difesa_Torri; }
            if (struttura == 6) { salute = difensore.Salute_Castello; difesa = difensore.Difesa_Castello; }

            if (salute == 0 && difesa == 0 || defenderUnits.TotalUnits() == 0) valido = true;

            // Salva unità originali per i log
            var attackerUnitsOriginali = attackerUnits.Clone();
            var defenderUnitsOriginali = defenderUnits.Clone();
            RangedBattleResult rangedResult = null;

            if (PreBattaglia) rangedResult = EseguiPreBattaglia_PvP(attackerUnits, defenderUnits, attaccante, difensore, attackerGuid, defenderGuid);

            // FASE 2: Battaglia corpo a corpo (con bonus difesa 20%)
            result = EseguiBattaglia_PvP(rangedResult.AttaccanteUnitsAfter, rangedResult.DifensoreUnitsAfter, attaccante, difensore, attackerGuid, defenderGuid, salute, difesa, struttura);
            result.FaseDistanza = rangedResult;

            if (result != null) result.FaseDistanza = rangedResult;

            for (int i = 0; i < 5; i++)// Aggiorna stato
            {
                result.AttaccantePerdite.Guerrieri[i] += rangedResult.DefenderKills_Guerrieri[i];
                result.AttaccantePerdite.Lancieri[i] += rangedResult.DefenderKills_Lancieri[i];

                result.DifensorePerdite.Guerrieri[i] += rangedResult.AttackerKills_Guerrieri[i];
                result.DifensorePerdite.Lancieri[i] += rangedResult.AttackerKills_Lancieri[i];

                if (result.AttaccantePerdite.TotalUnits() > 0) //Attaccante con 0 unità? bene fine attaco... ed aggiornamento truppe perse.
                {
                    attaccante.Guerrieri[i] -= result.AttaccantePerdite.Guerrieri[i];
                    attaccante.Lanceri[i] -= result.AttaccantePerdite.Lancieri[i];
                    attaccante.Arceri[i] -= result.AttaccantePerdite.Arcieri[i];
                    attaccante.Catapulte[i] -= result.AttaccantePerdite.Catapulte[i];
                    terminaBattaglia = true;
                }
            }
            for (int i = 0; i < 5; i++)// Aggiorna stato attaccante
            {
                // Statistiche attaccante
                attaccante.Guerrieri_Eliminati += result.DifensorePerdite.Guerrieri[i];
                attaccante.Lanceri_Eliminati += result.DifensorePerdite.Lancieri[i];
                attaccante.Arceri_Eliminati += result.DifensorePerdite.Arcieri[i];
                attaccante.Catapulte_Eliminate += result.DifensorePerdite.Catapulte[i];

                attaccante.Guerrieri_Persi += result.AttaccantePerdite.Guerrieri[i];
                attaccante.Lanceri_Persi += result.AttaccantePerdite.Lancieri[i];
                attaccante.Arceri_Persi += result.AttaccantePerdite.Arcieri[i];
                attaccante.Catapulte_Perse += result.AttaccantePerdite.Catapulte[i];
            }
            perdite_Attaccante += result.AttaccantePerdite.TotalUnits();
            attaccante.Unità_Eliminate += result.DifensorePerdite.TotalUnits();
            attaccante.Unità_Perse += result.AttaccantePerdite.TotalUnits();
            perdite_Difensore += result.DifensorePerdite.TotalUnits();
            difensore.Unità_Eliminate += result.AttaccantePerdite.TotalUnits();
            difensore.Unità_Perse += result.DifensorePerdite.TotalUnits();
            AggiornaDatiStruttureDifensore(struttura, difensore, result); //Aggiorna dati struttura difensore

            for (int i = 0; i < 5; i++) // Aggiorna stato difensore
            {
                difensore.Guerrieri_Eliminati += result.AttaccantePerdite.Guerrieri[i];
                difensore.Lanceri_Eliminati += result.AttaccantePerdite.Lancieri[i];
                difensore.Arceri_Eliminati += result.AttaccantePerdite.Arcieri[i];
                difensore.Catapulte_Eliminate += result.AttaccantePerdite.Catapulte[i];

                difensore.Guerrieri_Persi += result.DifensorePerdite.Guerrieri[i];
                difensore.Lanceri_Persi += result.DifensorePerdite.Lancieri[i];
                difensore.Arceri_Persi += result.DifensorePerdite.Arcieri[i];
                difensore.Catapulte_Perse += result.DifensorePerdite.Catapulte[i];
            }

            //Quest
            OnEvent(attaccante, QuestEventType.Uccisioni, "Guerrieri", result.DifensorePerdite.Guerrieri.Sum());
            OnEvent(attaccante, QuestEventType.Uccisioni, "Lanceri", result.DifensorePerdite.Lancieri.Sum());
            OnEvent(attaccante, QuestEventType.Uccisioni, "Arceri", result.DifensorePerdite.Arcieri.Sum());
            OnEvent(attaccante, QuestEventType.Uccisioni, "Catapulte", result.DifensorePerdite.Catapulte.Sum());
            OnEvent(attaccante, QuestEventType.Risorse, "Frecce", result.FrecceNecessarieAttacker);

            OnEvent(difensore, QuestEventType.Uccisioni, "Guerrieri", result.AttaccantePerdite.Guerrieri.Sum());
            OnEvent(difensore, QuestEventType.Uccisioni, "Lanceri", result.AttaccantePerdite.Lancieri.Sum());
            OnEvent(difensore, QuestEventType.Uccisioni, "Arceri", result.AttaccantePerdite.Arcieri.Sum());
            OnEvent(difensore, QuestEventType.Uccisioni, "Catapulte", result.AttaccantePerdite.Catapulte.Sum());
            OnEvent(difensore, QuestEventType.Risorse, "Frecce", result.FrecceNecessarieDefender);

            // Esperienza
            attaccante.Esperienza += CalcolaEsperienzaPVP(result.DifensorePerdite);
            difensore.Esperienza += CalcolaEsperienzaPVP(result.AttaccantePerdite);

            if (perdite_Difensore > perdite_Attaccante || (perdite_Difensore == 0 && perdite_Attaccante == 0) || valido == true) // Determina vittoria (l'attaccante vince se causa più perdite del difensore)
                result.Victory = true;
            else  result.Victory = false;

            InviaLogBattaglia_PvP(attackerGuid, defenderGuid, attaccante, difensore, result, attackerUnitsOriginali, defenderUnitsOriginali);
            if (result.Victory == true && struttura == 6) //Se la battaglia "fallisce" ritira le truppe.
            {
                attaccante.Attacchi_Effettuati_PVP++;
                attaccante.Battaglie_Perse++;
                difensore.Attacchi_Subiti_PVP++;
                difensore.Battaglie_Vinte++;
                return result;
            }
            if (terminaBattaglia) return result; // Termina la battaglia per mancanza di truppe attaccante.
        }
        return result;
    }
    static int CalcoloFrecce(UnitGroup unità)
    {
        int frecce = unità.Arcieri[0] * Esercito.Unità.Arcere_1.Componente_Lancio + unità.Catapulte[0] * Esercito.Unità.Catapulta_1.Componente_Lancio;
        frecce += unità.Arcieri[1] * Esercito.Unità.Arcere_2.Componente_Lancio + unità.Catapulte[1] * Esercito.Unità.Catapulta_2.Componente_Lancio;
        frecce += unità.Arcieri[2] * Esercito.Unità.Arcere_3.Componente_Lancio + unità.Catapulte[2] * Esercito.Unità.Catapulta_3.Componente_Lancio;
        frecce += unità.Arcieri[3] * Esercito.Unità.Arcere_4.Componente_Lancio + unità.Catapulte[3] * Esercito.Unità.Catapulta_4.Componente_Lancio;
        frecce += unità.Arcieri[4] * Esercito.Unità.Arcere_5.Componente_Lancio + unità.Catapulte[4] * Esercito.Unità.Catapulta_5.Componente_Lancio;
        return frecce;
    }
    static UnitGroup CaricaDatiStruttureDifensore(UnitGroup defenderUnits, Player difensore, int struttura)
    {
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
        return defenderUnits;
    }
    static void AggiornaDatiStruttureDifensore(int struttura, Player difensore, BattleResult result)
    {
        if (struttura == 1)
        {
            difensore.Guerrieri_Ingresso = result.DifensoreSopravvisuti.Guerrieri;
            difensore.Lanceri_Ingresso = result.DifensoreSopravvisuti.Lancieri;
            difensore.Arceri_Ingresso = result.DifensoreSopravvisuti.Arcieri;
            difensore.Catapulte_Ingresso = result.DifensoreSopravvisuti.Catapulte;
            result.Struttura = "Ingresso";
        }
        if (struttura == 2)
        {
            difensore.Salute_Mura = result.Salute;
            difensore.Difesa_Mura = result.Difesa;
            result.Struttura = "Mura";
            if (result.Salute == 0)
            {
                result.DifensoreCrollo.Guerrieri = difensore.Guerrieri_Mura;
                result.DifensoreCrollo.Lancieri = difensore.Lanceri_Mura;
                result.DifensoreCrollo.Arcieri = difensore.Arceri_Mura;
                result.DifensoreCrollo.Catapulte = difensore.Catapulte_Mura;

                difensore.Guerrieri_Mura = [0, 0, 0, 0, 0];
                difensore.Lanceri_Mura = [0, 0, 0, 0, 0];
                difensore.Arceri_Mura = [0, 0, 0, 0, 0];
                difensore.Catapulte_Mura = [0, 0, 0, 0, 0];
            }
            else
            {
                difensore.Guerrieri_Mura = result.DifensoreSopravvisuti.Guerrieri;
                difensore.Lanceri_Mura = result.DifensoreSopravvisuti.Lancieri;
                difensore.Arceri_Mura = result.DifensoreSopravvisuti.Arcieri;
                difensore.Catapulte_Mura = result.DifensoreSopravvisuti.Catapulte;
            }
        }
        if (struttura == 3)
        {
            difensore.Salute_Cancello = result.Salute;
            difensore.Difesa_Cancello = result.Difesa;
            result.Struttura = "Cancello";
            if (result.Salute == 0) //Distruzione struttura
            {
                result.DifensoreCrollo.Guerrieri = difensore.Guerrieri_Cancello;
                result.DifensoreCrollo.Lancieri = difensore.Lanceri_Cancello;
                result.DifensoreCrollo.Arcieri = difensore.Arceri_Cancello;
                result.DifensoreCrollo.Catapulte = difensore.Catapulte_Cancello;

                difensore.Guerrieri_Cancello = [0, 0, 0, 0, 0];
                difensore.Lanceri_Cancello = [0, 0, 0, 0, 0];
                difensore.Arceri_Cancello = [0, 0, 0, 0, 0];
                difensore.Arceri_Cancello = [0, 0, 0, 0, 0];
                difensore.Catapulte_Cancello = [0, 0, 0, 0, 0];
            }
            else
            {
                difensore.Guerrieri_Cancello = result.DifensoreSopravvisuti.Guerrieri;
                difensore.Lanceri_Cancello = result.DifensoreSopravvisuti.Lancieri;
                difensore.Arceri_Cancello = result.DifensoreSopravvisuti.Arcieri;
                difensore.Catapulte_Cancello = result.DifensoreSopravvisuti.Catapulte;
            }
        }
        if (struttura == 4)
        {
            difensore.Salute_Torri = result.Salute;
            difensore.Difesa_Torri = result.Difesa;
            result.Struttura = "Torri";
            if (result.Salute == 0)
            {
                result.DifensoreCrollo.Guerrieri = difensore.Guerrieri_Torri;
                result.DifensoreCrollo.Lancieri = difensore.Lanceri_Torri;
                result.DifensoreCrollo.Arcieri = difensore.Arceri_Torri;
                result.DifensoreCrollo.Catapulte = difensore.Catapulte_Torri;

                difensore.Guerrieri_Torri = [0, 0, 0, 0, 0];
                difensore.Lanceri_Torri = [0, 0, 0, 0, 0];
                difensore.Arceri_Torri = [0, 0, 0, 0, 0];
                difensore.Catapulte_Torri = [0, 0, 0, 0, 0];
            }
            else
            {
                difensore.Guerrieri_Torri = result.DifensoreSopravvisuti.Guerrieri;
                difensore.Lanceri_Torri = result.DifensoreSopravvisuti.Lancieri;
                difensore.Arceri_Torri = result.DifensoreSopravvisuti.Arcieri;
                difensore.Catapulte_Torri = result.DifensoreSopravvisuti.Catapulte;
            }
        }
        if (struttura == 5)
        {
            difensore.Guerrieri_Citta = result.DifensoreSopravvisuti.Guerrieri;
            difensore.Lanceri_Citta = result.DifensoreSopravvisuti.Lancieri;
            difensore.Arceri_Citta = result.DifensoreSopravvisuti.Arcieri;
            difensore.Catapulte_Citta = result.DifensoreSopravvisuti.Catapulte;
            result.Struttura = "Centro Villaggio";
        }
        if (struttura == 6)
        {
            difensore.Salute_Castello = result.Salute;
            difensore.Difesa_Castello = result.Difesa;
            result.Struttura = "Castello";
            if (result.Salute == 0)
            {
                result.DifensoreCrollo.Guerrieri = difensore.Guerrieri_Castello;
                result.DifensoreCrollo.Lancieri = difensore.Lanceri_Castello;
                result.DifensoreCrollo.Arcieri = difensore.Arceri_Castello;
                result.DifensoreCrollo.Catapulte = difensore.Catapulte_Castello;

                difensore.Guerrieri_Castello = [0, 0, 0, 0, 0];
                difensore.Lanceri_Castello = [0, 0, 0, 0, 0];
                difensore.Arceri_Castello = [0, 0, 0, 0, 0];
                difensore.Catapulte_Castello = [0, 0, 0, 0, 0];
            }
            else
            {
                difensore.Guerrieri_Castello = result.DifensoreSopravvisuti.Guerrieri;
                difensore.Lanceri_Castello = result.DifensoreSopravvisuti.Lancieri;
                difensore.Arceri_Castello = result.DifensoreSopravvisuti.Arcieri;
                difensore.Catapulte_Castello = result.DifensoreSopravvisuti.Catapulte;
            }
        }
    }
    private static RangedBattleResult EseguiPreBattaglia_PvP(UnitGroup attackerUnits, UnitGroup defenderUnits, Player attaccante, Player difensore, Guid attackerGuid, Guid defenderGuid)
    {
        var result = new RangedBattleResult
        {
            AttaccanteUnitsAfter = attackerUnits.Clone(),
            DifensoreUnitsAfter = defenderUnits.Clone(),
        };

        if (defenderUnits.TotalUnits() == 0) result.Unità_Presenti = false;
        else result.Unità_Presenti = true;

        // ATTACCO DELL'ATTACCANTE
        var attackerRangedAttack = CalcolaAttaccoDistanza(attackerUnits, attaccante, attackerGuid, true);
        result.ArrowsUsedAttacker = attackerRangedAttack.FrecceUsate;
        result.PlayerHadLowArrows = attackerRangedAttack.FrecceInsufficienti;

        var defenderRangedAttack = CalcolaAttaccoDistanza(defenderUnits, difensore, defenderGuid, true);
        result.ArrowsUsedDefender = defenderRangedAttack.FrecceUsate;
        result.EnemyHadLowArrows = defenderRangedAttack.FrecceInsufficienti;

        // Applica danni con tracciamento livelli
        var defenderMorti = ApplicaDanniDistanza(defenderUnits, attackerRangedAttack.DannoGuerrieri, attackerRangedAttack.DannoLancieri);
        Array.Copy(defenderMorti, 0, result.AttackerKills_Guerrieri, 0, 5);
        Array.Copy(defenderMorti, 5, result.AttackerKills_Lancieri, 0, 5);

        var attackerMorti = ApplicaDanniDistanza(attackerUnits, defenderRangedAttack.DannoGuerrieri, defenderRangedAttack.DannoLancieri);
        Array.Copy(attackerMorti, 0, result.DefenderKills_Guerrieri, 0, 5);
        Array.Copy(attackerMorti, 5, result.DefenderKills_Lancieri, 0, 5);

        for (int i = 0; i < 5; i++)
        {
            result.AttaccanteUnitsAfter.Guerrieri[i] -= result.DefenderKills_Guerrieri[i];
            result.AttaccanteUnitsAfter.Lancieri[i] -= result.DefenderKills_Lancieri[i];
            result.DifensoreUnitsAfter.Guerrieri[i] -= result.AttackerKills_Guerrieri[i];
            result.DifensoreUnitsAfter.Lancieri[i] -= result.AttackerKills_Lancieri[i];

            var stats = GetUnitStats(i);
            result.Xp_Attacker += result.AttackerKills_Guerrieri[i] * stats.GuerrieriEsperienza; // Attaccante guadagna exp per unità difensore uccise
            result.Xp_Attacker += result.AttackerKills_Lancieri[i] * stats.LancieriEsperienza; // Attaccante guadagna exp per unità difensore uccise
            result.Xp_Defender += result.DefenderKills_Guerrieri[i] * stats.GuerrieriEsperienza; // Difensore guadagna exp per unità attaccante uccise
            result.Xp_Defender += result.DefenderKills_Lancieri[i] * stats.LancieriEsperienza; // Difensore guadagna exp per unità attaccante uccise
        }
        return result;
    }
    private static BattleResult EseguiBattaglia_PvP(UnitGroup attackerUnits, UnitGroup defenderUnits, Player attaccante, Player difensore, Guid attackerGuid, 
        Guid defenderGuid, double salute, double difesa, int struttura)
    {
        OnEvent(attaccante, QuestEventType.Battaglie, "Attacco Giocatore", 1); //Quest
        var result = new BattleResult
        {
            AttaccanteSopravvisuti = attackerUnits.Clone(),
            DifensoreSopravvisuti = defenderUnits.Clone(),
            DifensoreCrollo = defenderUnits.Clone(),
            AttaccantePerdite = new UnitGroup(),
            DifensorePerdite = new UnitGroup()
        };
        if (defenderUnits.TotalUnits() == 0) result.Unità_Presenti = false;
        else result.Unità_Presenti = true;

        int attackerTypes = attackerUnits.CountUnitTypes();
        int defenderTypes = defenderUnits.CountUnitTypes();

        int truppeAttaccante = attackerUnits.TotalUnits(); //Numero truppe
        int truppeDifensore = defenderUnits.TotalUnits();
        bool Frecce = false;

        if (truppeDifensore > 0 && salute > 5) Frecce = true;
        double dannoAttaccante = CalcolaDannoGiocatore(attackerUnits, attaccante, attackerGuid, Frecce, true, result); // Calcola danno
        Frecce = false; //reset, per riutilizzo

        if (truppeAttaccante > 0 && struttura != 1 && struttura != 5) Frecce = true;
        else Frecce = false;
        double dannoDifensore = CalcolaDannoGiocatore(defenderUnits, difensore, defenderGuid, Frecce, false, result); // Calcola danno

        result.PlayerDamage = dannoAttaccante;
        result.EnemyDamage = dannoDifensore;
        double dannotempDifesa = dannoAttaccante * 0.30; //Se struttura == 0, non serve a nulla questa variabile.  
        double dannotempSalute = 0; //Se struttura == 0, non serve a nulla questa variabile.
        double bonusUnità = 1;
        if (struttura != 0 && struttura != 5 && salute > 5)
        {
            if (dannotempDifesa >= difesa)
            {
                dannotempDifesa -= difesa;
                dannoAttaccante -= difesa;
                difesa = 0;
            }
            else
            {
                difesa -= dannotempDifesa;
                dannoAttaccante -= dannotempDifesa;
            }
            dannotempSalute = (dannoAttaccante - dannotempDifesa) * 0.20;
            if (dannotempSalute >= salute)
            {
                dannotempSalute -= salute;
                dannoAttaccante -= salute;
                salute = 0;
            }
            else
            {
                salute -= dannotempSalute;
                dannoAttaccante -= dannotempSalute;
            }
        }      
        if (struttura == 1)
        {
            if (difensore.Guarnigione_Cancello >= difensore.Guarnigione_CancelloMax * 0.40 || difensore.Guarnigione_Mura >= difensore.Guarnigione_MuraMax * 0.40) bonusUnità += 0.10;
            if (difensore.Guarnigione_Cancello >= difensore.Guarnigione_CancelloMax * 0.80 && difensore.Guarnigione_Mura > difensore.Guarnigione_MuraMax * 0.80) bonusUnità += 0.15;
            if (difensore.Guarnigione_Cancello == difensore.Guarnigione_CancelloMax && difensore.Guarnigione_Mura == difensore.Guarnigione_MuraMax) bonusUnità += 0.20;
        }
        if (struttura == 0) result.Struttura = "Villaggio"; //Imposta il tipo di struttura per il titolo del log

        double dannoPerTipoAttacker = dannoDifensore / attackerTypes;
        double dannoPerTipoDefender = dannoAttaccante / defenderTypes;
        var battleResult = ApplicaDanniGiocatore(result, attackerUnits, attaccante, dannoPerTipoAttacker, 1, true); // Applica danni all'attaccante
        battleResult = ApplicaDanniGiocatore(battleResult, defenderUnits, difensore, dannoPerTipoDefender, bonusUnità, false); //Difensore con bonus guarnigione

        result.DifensoreCrollo = battleResult.DifensoreCrollo;
        result.Salute = (int)salute;
        result.Difesa = (int)difesa;

        // Calcola esperienza corpo a corpo per entrambi
        int expAttaccante = CalcolaEsperienzaPVP(result.DifensorePerdite);
        int expDifensore = CalcolaEsperienzaPVP(result.AttaccantePerdite);
        attaccante.Esperienza += expAttaccante;
        difensore.Esperienza += expDifensore;
        result.Xp_Attacker = expAttaccante; // Per il log
        result.Xp_Defender = expDifensore;

        result.Victory = result.DifensoreSopravvisuti.TotalUnits() == 0; // Determina vittoria provvisoria (sarà confermata in base alle perdite totali)
        return result;
    }
    private static void AssegnaRisorseVittoria_PvP(Player attaccante, Player difensore, Guid attackerGuid, UnitGroup sopravvissuti)
    {
        int capacitàCarico = CapacitàCarico(sopravvissuti, attaccante);
        int capacitàOriginale = capacitàCarico;

        // Il 50% delle risorse del difensore può essere rubato
        double cibo = difensore.Cibo / 2;
        double legno = difensore.Legno / 2;
        double pietra = difensore.Pietra / 2;
        double ferro = difensore.Ferro / 2;
        double oro = difensore.Oro / 2;
        int diamantiViola = Variabili_Server.Max_Diamanti_Viola_PVP_Giocatore - difensore.Diamanti_Viola_PVP_Persi;
        int diamantiBlu = Variabili_Server.Max_Diamanti_Blu_PVP_Giocatore - difensore.Diamanti_Blu_PVP_Persi;

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
    }
}