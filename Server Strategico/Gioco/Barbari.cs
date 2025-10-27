using Server_Strategico.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using static Server_Strategico.Gioco.Giocatori;

namespace Server_Strategico.Gioco
{
    public class Barbari
    {
        public static bool start = false;

        private static System.Timers.Timer timerReset;
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

            public double Cibo { get; set; }
            public double Legno { get; set; }
            public double Pietra { get; set; }
            public double Ferro { get; set; }
            public double Oro { get; set; }

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
                Cibo = 100 * livello,
                Legno = 100 * livello,
                Pietra = 100 * livello,
                Ferro = 100 * livello,
                Oro = 50 * livello,
                Guerrieri = baseTruppe,
                Lancieri = baseTruppe,
                Arcieri = baseTruppe,
                Catapulte = baseTruppe
            };
        }

        public static CittaBarbara GeneraCitta(int livello) // 🔹 Generazione città barbarica globale
        {
            int baseTruppe = 125 * livello;
            return new CittaBarbara
            {
                Id = Guid.NewGuid().GetHashCode(),
                Nome = $"Citta Barbara Lv{livello}",
                Livello = livello,
                Sconfitto = false,
                Esplorato = false,
                Esperienza = 20 * livello,
                Diamanti_Viola = 1 * livello,
                Diamanti_Blu = 1 * livello,
                Cibo = 100 * livello,
                Legno = 100 * livello,
                Pietra = 100 * livello,
                Ferro = 100 * livello,
                Oro = 50 * livello,
                Guerrieri = baseTruppe,
                Lancieri = baseTruppe,
                Arcieri = baseTruppe,
                Catapulte = baseTruppe
            };
        }
        public static void GeneraVillaggiPerGiocatore(Player player)
        {
            if (player.VillaggiPersonali == null)
                player.VillaggiPersonali = new List<VillaggioBarbaro>();

            player.VillaggiPersonali.Clear();

            for (int lv = 1; lv <= 20; lv++)
                player.VillaggiPersonali.Add(GeneraVillaggio(lv));

            Console.WriteLine($"[Barbari] Generati {player.VillaggiPersonali.Count} villaggi per {player.Username}");
        }

        public static void Inizializza() // Inizializzazione globale (da chiamare all’avvio del server)
        {
            if (start) return;
            start = true;

            for (int i = 1; i <= 20; i++) // Genera 20 città barbariche globali
                CittaGlobali.Add(GeneraCitta(i));

            foreach (var player in Server.Server.servers_.players.Values) // Genera villaggi per tutti i giocatori esistenti
                GeneraVillaggiPerGiocatore(player);

            Console.WriteLine($"[Barbari] Generate {CittaGlobali.Count} città globali iniziali.");
            AvviaTimerReset(); // Avvia il timer giornaliero
        }

        private static void AvviaTimerReset() // Avvia timer per rigenerare barbari ogni giorno
        {
            // Calcola il tempo mancante alla mezzanotte
            DateTime ora = DateTime.Now;
            DateTime prossimaMezzanotte = ora.Date.AddDays(1);
            double tempoIniziale = (prossimaMezzanotte - ora).TotalMilliseconds;

            // Primo timer fino alla mezzanotte
            timerReset = new System.Timers.Timer(tempoIniziale);
            timerReset.Elapsed += (s, e) =>
            {
                RigeneraBarbari();
                timerReset.Interval = TimeSpan.FromDays(1).TotalMilliseconds; // dopo la prima volta, ripete ogni 24 ore
            };
            timerReset.AutoReset = true;
            timerReset.Start();

            Console.WriteLine($"[Barbari] Timer di rigenerazione impostato: primo reset tra {(int)(tempoIniziale / 1000 / 60)} minuti.");
        }

        public static void RigeneraBarbari() // 🔁 Rigenera città globali e villaggi personali
        {
            Console.WriteLine($"[Barbari] Rigenerazione giornaliera iniziata ({DateTime.Now:HH:mm:ss})");

            // ✅ Rigenera città globali
            int città = CittaGlobali.Count;
            CittaGlobali.Clear();
            for (int i = 1; i <= città; i++)
                CittaGlobali.Add(GeneraCitta(i));

            // ✅ Rigenera villaggi per ogni giocatore
            foreach (var player in Server.Server.servers_.players.Values)
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
            int costo = target.IsGlobal ? 2 : 1; // 500 : 100
            if (g.Oro < costo)
                return (-1, -1, -1, -1); // indicatore di errore

            g.Oro -= costo;
            target.Esplorato = true;
            return StimaTruppe(target); // restituisce (guerrieri, lancieri, arcieri, catapulte)
        }
    }
}

