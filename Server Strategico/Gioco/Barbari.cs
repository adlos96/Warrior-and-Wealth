using static Server_Strategico.Gioco.Giocatori;

namespace Server_Strategico.Gioco
{
    public class Barbari
    {
        public static bool start = false;
        public static List<CittaBarbara> CittaGlobali = new(); // 🌍 Lista globale delle città barbariche (visibili da tutti)
        private static Random rnd = new(); // 🔒 Random condiviso 

        // 🧱 Classe base per villaggi e città
        public abstract class BarbarianBase
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public int Livello { get; set; }
            public bool Sconfitto { get; set; }
            public bool Esplorato { get; set; }
            public int Esperienza { get; set; }

            public int Diamanti_Viola { get; set; }
            public int Diamanti_Blu { get; set; }

            public int Cibo { get; set; }
            public int Legno { get; set; }
            public int Pietra { get; set; }
            public int Ferro { get; set; }
            public int Oro { get; set; }

            public int Guerrieri { get; set; }
            public int Lancieri { get; set; }
            public int Arcieri { get; set; }
            public int Catapulte { get; set; }

            public abstract bool IsGlobal { get; }
        }
        public class VillaggioBarbaro : BarbarianBase // 🏚️ Villaggio personale (solo per il giocatore)
        {
            public override bool IsGlobal => false;
        }
        public class CittaBarbara : BarbarianBase  // 🏰 Città globale (visibile a tutti)
        {
            public override bool IsGlobal => true;
        }

        public static VillaggioBarbaro GeneraVillaggio(int livello) // 🔹 Generazione villaggio barbaro personale
        {
            int baseTruppe = 50 * livello;
            return new VillaggioBarbaro
            {
                Id = Guid.NewGuid().GetHashCode(),
                Nome = $"Villaggio Barbaro Lv{livello}",
                Livello = livello,
                Sconfitto = false,
                Esplorato = false,
                Esperienza = 20 * livello,
                Diamanti_Viola = 0 * livello,
                Diamanti_Blu = 1 * livello,
                Cibo = 2300 * livello,
                Legno = 2150 * livello,
                Pietra = 2000 * livello,
                Ferro = 1800 * livello,
                Oro = 1050 * livello,
                Guerrieri = baseTruppe,
                Lancieri = (int)(baseTruppe * 0.98),
                Arcieri = (int)(baseTruppe * 0.70),
                Catapulte = (int)(baseTruppe* 0.58)
            };
        }
        public static CittaBarbara GeneraCitta(int livello) // 🔹 Generazione città barbarica globale
        {
            int baseTruppe = 130 * livello;
            return new CittaBarbara
            {
                Id = Guid.NewGuid().GetHashCode(),
                Nome = $"Citta Barbare Lv{livello}",
                Livello = livello,
                Sconfitto = false,
                Esplorato = false,
                Esperienza = 20 * livello,
                Diamanti_Viola = 1 * livello,
                Diamanti_Blu = 1 * livello,
                Cibo = 23000 * livello,
                Legno = 21500 * livello,
                Pietra = 20000 * livello,
                Ferro = 18000 * livello,
                Oro = 10500 * livello,
                Guerrieri = baseTruppe,
                Lancieri = (int)(baseTruppe * 0.98),
                Arcieri = (int)(baseTruppe * 0.70),
                Catapulte = (int)(baseTruppe * 0.58)
            };
        }
        public static void GeneraVillaggiPerGiocatore(Player player)
        {
            if (player.VillaggiPersonali == null)
                player.VillaggiPersonali = new List<VillaggioBarbaro>();
            else
                player.VillaggiPersonali.Clear();

            for (int lv = 1; lv <= 20; lv++)
                player.VillaggiPersonali.Add(GeneraVillaggio(lv));

            Console.WriteLine($"[Barbari] Generati {player.VillaggiPersonali.Count} villaggi per {player.Username}");
            int diamanti_Viola = 0, diamanti_Blu = 0;
            int guerrieri = 0;
            int lancieri = 0;
            int arcieri = 0;
            int catapulte = 0;

            foreach (var data in player.VillaggiPersonali)
            {
                diamanti_Viola += data.Diamanti_Viola;
                diamanti_Blu += data.Diamanti_Blu;
                guerrieri += data.Guerrieri;
                lancieri += data.Lancieri;
                arcieri += data.Arcieri;
                catapulte += data.Catapulte;
            }
            Console.WriteLine($"[Barbari] Stats Villaggi Barbare: {diamanti_Viola} D_V, {diamanti_Blu} D_B, {guerrieri} G, {lancieri} L, {arcieri} A, {catapulte} C");
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

            foreach (var data in CittaGlobali)
            {
                diamanti_Viola += data.Diamanti_Viola;
                diamanti_Blu += data.Diamanti_Blu;
                guerrieri += data.Guerrieri;
                lancieri += data.Lancieri;
                arcieri += data.Arcieri;
                catapulte += data.Catapulte;
            }
            Console.WriteLine($"[Barbari] Stats Città Barbare: {diamanti_Viola} D_V, {diamanti_Blu} D_B, {guerrieri} G, {lancieri} L, {arcieri} A, {catapulte} C");
        }

        public static void RigeneraBarbari() // 🔁 Rigenera città globali e villaggi personali
        {
            Console.WriteLine($"[Barbari] Rigenerazione giornaliera iniziata ({DateTime.Now:HH:mm:ss})");

            int città = CittaGlobali.Count;
            CittaGlobali.Clear();
            for (int i = 1; i <= città; i++) // ✅ Rigenera città globali
                CittaGlobali.Add(GeneraCitta(i));

            foreach (var player in Server.Server.servers_.players.Values) // ✅ Rigenera villaggi per ogni giocatore
            {
                if (player.VillaggiPersonali == null)
                    player.VillaggiPersonali = new List<VillaggioBarbaro>();

                int villaggi = player.VillaggiPersonali.Count;
                player.VillaggiPersonali.Clear();
                for (int lv = 1; lv <= villaggi; lv++)
                    player.VillaggiPersonali.Add(GeneraVillaggio(lv));
            }
            Console.WriteLine($"[Barbari] Rigenerazione completata: {CittaGlobali.Count} città e villaggi per {Server.Server.servers_.players.Count} giocatori.");
        }
         
        public static (int, int, int, int) StimaTruppe(BarbarianBase target) // 🔍 Esplorazione — stima truppe (±20%)
        {
            int Deviazione(int val) => (int)(val * (1 + rnd.Next(-20, 21) / 100.0));
            return (Deviazione(target.Guerrieri), Deviazione(target.Lancieri),
                    Deviazione(target.Arcieri), Deviazione(target.Catapulte));
        }

        public static (int G, int L, int A, int C) EsploraTruppe(Player g, BarbarianBase target)  // 💰 Esplorazione con costo in oro
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

