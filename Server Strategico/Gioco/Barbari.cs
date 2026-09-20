using static Server_Strategico.Gioco.Giocatori;
using Server_Strategico.ServerData.Moduli.Battaglie;

namespace Server_Strategico.Gioco
{
    public class Barbari
    {
        public static bool start = false;
        public static List<CittaBarbara> CittaGlobali = new(); // Lista globale delle città barbariche (visibili da tutti)
        private static Random rnd = new(); // Random condiviso 

        // Classe base per villaggi e città
        public abstract class BarbarianBase
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public int Livello { get; set; }
            public bool Sconfitto { get; set; }
            public bool Esplorato { get; set; }
            public bool Saccheggiato { get; set; }
            public int Esperienza { get; set; }

            public int Salute { get; set; }
            public int Difesa { get; set; }
            public int Contro_Spionaggio { get; set; }

            public int Diamanti_Viola { get; set; }
            public int Diamanti_Blu { get; set; }

            public int Cibo { get; set; }
            public int Legno { get; set; }
            public int Pietra { get; set; }
            public int Ferro { get; set; }
            public int Oro { get; set; }

            public int[] Guerrieri { get; set; }
            public int[] Lancieri { get; set; }
            public int[] Arcieri { get; set; }
            public int[] Catapulte { get; set; }

            public abstract bool IsGlobal { get; }
        }
        public class VillaggioBarbaro : BarbarianBase // Villaggio personale (solo per il giocatore)
        {
            public override bool IsGlobal => false;
        }
        public class CittaBarbara : BarbarianBase  // Città globale (visibile a tutti)
        {
            public override bool IsGlobal => true;
        }
        public static void RiparaCittàBarbare()
        {
            foreach (var citta in CittaGlobali)
            {
                citta.Salute++;
                citta.Difesa++;
            }
        }
        public static void RiparaVillaggiBarbari(Player player)
        {
            foreach (var citta in player.VillaggiPersonali)
            {
                citta.Salute++;
                citta.Difesa++;
            }
        }
        public static int truppeVillaggio = 20;
        public static int truppeCittà = 110;

        // 20/09/2026, su richiesta dell'utente: le truppe di villaggi/città barbare ora sono
        // array a 5 tier come quelle del giocatore (Guerrieri/Lancieri/Arcieri/Catapulte), solo
        // per uniformità di struttura con UnitGroup (necessaria per riusare le formule di
        // BattagliaPVE.cs) — NON per avere un esercito "misto" su più tier. Il livello del
        // barbaro (1-20) determina due cose separate, come chiarito dall'utente:
        //  - la FORZA delle truppe: tutto il totale finisce su un solo tier, quello di
        //    BattagliaPVE.GetTierIndex(livello) — più alto è il livello, più alto (quindi più
        //    forte, vedi GetEnemyUnitStats) è quel tier;
        //  - il NUMERO di truppe: resta la formula "baseTruppe" già esistente e invariata
        //    (scala col livello del villaggio/città e, per i villaggi personali, anche col
        //    livello del giocatore) — nessuna suddivisione del totale su più tier.
        private static int[] TruppeSulProprioTier(int totale, int tierIndex)
        {
            var arr = new int[5];
            arr[Math.Clamp(tierIndex, 0, 4)] = totale;
            return arr;
        }

        public static VillaggioBarbaro GeneraVillaggio(int livello, int livello_Player) // Generazione villaggio barbaro personale
        {
            int baseTruppe = (int)(truppeVillaggio * livello + (truppeVillaggio * (livello_Player - 1) * 0.5)); //Non mi torna.... (Edit: ORa dovrebbe ignorare il "liv 1" del giocatore)
            int tierIndex = BattagliaPVE.GetTierIndex(livello);
            return new VillaggioBarbaro
            {
                Id = Guid.NewGuid().GetHashCode(),
                Nome = $"Villaggio Barbaro Lv{livello}",
                Livello = livello,
                Saccheggiato = false,
                Sconfitto = false,
                Contro_Spionaggio = 1 * livello,
                Esperienza = 20 * livello,
                Diamanti_Viola = 0 * livello,
                Diamanti_Blu = 3 * livello,
                Cibo = 2300 * livello,
                Legno = 2150 * livello,
                Pietra = 2000 * livello,
                Ferro = 1800 * livello,
                Oro = 1050 * livello,
                Guerrieri = TruppeSulProprioTier(baseTruppe, tierIndex),
                Lancieri = TruppeSulProprioTier((int)(baseTruppe * 0.97), tierIndex),
                Arcieri = TruppeSulProprioTier((int)(baseTruppe * 0.69), tierIndex),
                Catapulte = TruppeSulProprioTier((int)(baseTruppe * 0.57), tierIndex),
                Salute = 40 * livello,
                Difesa = 25 * livello
            };
        }
        public static CittaBarbara GeneraCitta(int livello) // Generazione città barbarica globale
        {
            int baseTruppe = truppeCittà * livello;
            int tierIndex = BattagliaPVE.GetTierIndex(livello);
            return new CittaBarbara
            {
                Id = Guid.NewGuid().GetHashCode(),
                Nome = $"Citta Barbare Lv{livello}",
                Livello = livello,
                Sconfitto = false,
                Saccheggiato = false,
                Esplorato = false, //Deprecato - Vecchio metodo
                Contro_Spionaggio = 1 * livello,
                Esperienza = 200 * livello,
                Diamanti_Viola = 15 * livello,
                Diamanti_Blu = 50 * livello,
                Cibo = 23000 * livello,
                Legno = 21500 * livello,
                Pietra = 20000 * livello,
                Ferro = 18000 * livello,
                Oro = 10500 * livello,
                Guerrieri = TruppeSulProprioTier(baseTruppe, tierIndex),
                Lancieri = TruppeSulProprioTier((int)(baseTruppe * 0.98), tierIndex),
                Arcieri = TruppeSulProprioTier((int)(baseTruppe * 0.70), tierIndex),
                Catapulte = TruppeSulProprioTier((int)(baseTruppe * 0.58), tierIndex),
                Salute = 100 * livello,
                Difesa = 50 * livello
            };
        }
        public static void GeneraVillaggiPerGiocatore(Player player)
        {
            if (player.VillaggiPersonali == null)
                player.VillaggiPersonali = new List<VillaggioBarbaro>();
            else
                player.VillaggiPersonali.Clear();

            for (int lv = 1; lv <= 20; lv++)
                player.VillaggiPersonali.Add(GeneraVillaggio(lv, player.Livello));

            // 16/09/2026, su richiesta dell'utente: tolti i due Console.WriteLine per-giocatore
            // che c'erano qui ("Generati N villaggi per X"/"Stats Villaggi Barbare: ...") — con
            // molti account salvati (anche solo di test) intasavano la console con due righe
            // IDENTICHE per ognuno ad ogni avvio del server (i 20 villaggi hanno sempre la
            // stessa formula, quindi le "stats" sono sempre le stesse). Il caricamento
            // complessivo resta comunque visibile nel riepilogo di GameSave.LoadAllPlayersData
            // ("[GameLoad] Caricati N giocatori..."), che è il segnale che conta davvero.
        }

        public static async Task Inizializza() // Inizializzazione globale (da chiamare all’avvio del server)
        {
            if (start) return;
            start = true;

            if (Variabili_Server.timer_Reset_Barbari == 0)
                Variabili_Server.timer_Reset_Barbari = 30 * 24 * 60 * 60;

            if (Gioco.Barbari.CittaGlobali.Count() == 0)
                for (int i = 1; i <= 20; i++) // Genera 20 città barbariche globali
                    CittaGlobali.Add(GeneraCitta(i));
           
            foreach (var player in Server.Server.servers_.players.Values) // Genera villaggi per tutti i giocatori esistenti
                if (player.VillaggiPersonali.Count() == 0)
                    GeneraVillaggiPerGiocatore(player);

            Console.WriteLine($"[Barbari] Generate {CittaGlobali.Count} città iniziali.");
            int diamanti_Viola = 0, diamanti_Blu = 0;
            int guerrieri = 0;
            int lancieri = 0;
            int arcieri = 0;
            int catapulte = 0;

            foreach (var citta in CittaGlobali)
            {
                diamanti_Viola += citta.Diamanti_Viola;
                diamanti_Blu += citta.Diamanti_Blu;
                guerrieri += citta.Guerrieri.Sum();
                lancieri += citta.Lancieri.Sum();
                arcieri += citta.Arcieri.Sum();
                catapulte += citta.Catapulte.Sum();
            }
            Console.WriteLine($"[Barbari] Stats Città Barbare: {diamanti_Viola} D_V, {diamanti_Blu} D_B, {guerrieri} G, {lancieri} L, {arcieri} A, {catapulte} C");
        }

        public static void RigeneraBarbari() // Rigenera città globali e villaggi personali
        {
            Console.WriteLine($"[Barbari] Rigenerazione giornaliera iniziata ({DateTime.Now:HH:mm:ss})");

            int città = CittaGlobali.Count;
            CittaGlobali.Clear();
            for (int i = 1; i <= città; i++) // Rigenera città globali
                CittaGlobali.Add(GeneraCitta(i));

            foreach (var player in Server.Server.servers_.players.Values) // Rigenera villaggi per ogni giocatore
            {
                if (player.VillaggiPersonali == null)
                    player.VillaggiPersonali = new List<VillaggioBarbaro>();

                int villaggi = player.VillaggiPersonali.Count;
                player.VillaggiPersonali.Clear();
                for (int lv = 1; lv <= villaggi; lv++)
                    player.VillaggiPersonali.Add(GeneraVillaggio(lv, player.Livello));
            }
            Console.WriteLine($"[Barbari] Rigenerazione completata: {CittaGlobali.Count} città e villaggi per {Server.Server.servers_.players.Count} giocatori.");
        }
         
        // 20/09/2026: target.Guerrieri/ecc. sono ora array a 5 tier — la stima mandata al client
        // resta un totale singolo per tipo (stesso contratto di prima, .Sum() sui 5 tier) per non
        // dover cambiare il protocollo client/Esplora.
        public static (int, int, int, int) StimaTruppe(BarbarianBase target) // Esplorazione — stima truppe (±20%)
        {
            int Deviazione(int val) => (int)(val * (1 + rnd.Next(-20, 21) / 100.0));
            return (Deviazione(target.Guerrieri.Sum()), Deviazione(target.Lancieri.Sum()),
                    Deviazione(target.Arcieri.Sum()), Deviazione(target.Catapulte.Sum()));
        }

        public static (int G, int L, int A, int C) EsploraTruppe(Player g, BarbarianBase target)  // Esplorazione con costo in oro
        {
            int costo = target.IsGlobal ? 2 : 1; // 500 : 100 -- Costo in oro per esplorare
            if (g.Oro < costo)
                return (-1, -1, -1, -1); // indicatore di errore

            g.Oro -= costo;
            target.Esplorato = true;
            return StimaTruppe(target); // restituisce (guerrieri, lancieri, arcieri, catapulte)
        }
    }
}

