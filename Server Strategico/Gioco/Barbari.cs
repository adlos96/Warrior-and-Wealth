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
        // Sostituisci l'uso di System.Threading.Timer con System.Timers.Timer
        private static System.Timers.Timer timerReset;

        // 🌍 Lista globale delle città barbariche (visibili da tutti)
        public static List<CittaBarbara> CittaGlobali = new();

        // 🔒 Random condiviso
        private static Random rnd = new();

        // 🧱 Classe base per villaggi e città
        public abstract class BarbarianBase
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public int Livello { get; set; }
            public bool Sconfitto { get; set; }
            public bool Esplorato { get; set; }

            public int Guerrieri { get; set; }
            public int Lancieri { get; set; }
            public int Arcieri { get; set; }
            public int Catapulte { get; set; }

            public abstract bool IsGlobal { get; }
        }

        // 🏚️ Villaggio personale (solo per il giocatore)
        public class VillaggioBarbaro : BarbarianBase
        {
            public override bool IsGlobal => false;
        }

        // 🏰 Città globale (visibile a tutti)
        public class CittaBarbara : BarbarianBase
        {
            public override bool IsGlobal => true;

            public bool AttaccoCooperativoAperto { get; set; } = false;
            public List<PartecipazioneCoop> Partecipanti { get; set; } = new();
        }

        // 👥 Partecipazioni cooperative
        public class PartecipazioneCoop
        {
            public string Giocatore { get; set; } = "";
            public int Guerrieri { get; set; }
            public int Lancieri { get; set; }
            public int Arcieri { get; set; }
            public int Catapulte { get; set; }
        }

        // 🔹 Generazione villaggio barbaro personale
        public static VillaggioBarbaro GeneraVillaggio(int livello)
        {
            int baseTruppe = 50 * livello;
            return new VillaggioBarbaro
            {
                Id = Guid.NewGuid().GetHashCode(),
                Nome = $"Villaggio Barbaro Lv{livello}",
                Livello = livello,
                Sconfitto = false,
                Esplorato = false,
                Guerrieri = baseTruppe,
                Lancieri = baseTruppe,
                Arcieri = baseTruppe,
                Catapulte = baseTruppe
            };
        }

        // 🔹 Generazione città barbarica globale
        public static CittaBarbara GeneraCitta(int livello)
        {
            int baseTruppe = 125 * livello;
            return new CittaBarbara
            {
                Id = Guid.NewGuid().GetHashCode(),
                Nome = $"Città Barbara Lv{livello}",
                Livello = livello,
                Sconfitto = false,
                Esplorato = false,
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

        // ⚙️ Inizializzazione globale (da chiamare all’avvio del server)
        public static void Inizializza()
        {
            if (start) return;
            start = true;

            // 🔹 Genera 5 città barbariche globali
            for (int i = 1; i <= 20; i++)
                CittaGlobali.Add(GeneraCitta(i));

            Console.WriteLine($"[Barbari] Generate {CittaGlobali.Count} città globali iniziali.");

            // 🔹 Genera villaggi per tutti i giocatori esistenti
            foreach (var player in Server.Server.servers_.players.Values)
                GeneraVillaggiPerGiocatore(player);

            // Avvia il timer giornaliero
            AvviaTimerReset();
        }

        // 🕒 Avvia timer per rigenerare barbari ogni giorno
        private static void AvviaTimerReset()
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

        // 🔁 Rigenera città globali e villaggi personali
        public static void RigeneraBarbari()
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

        // 🔍 Esplorazione — stima truppe (±20%)
        public static (int, int, int, int) StimaTruppe(BarbarianBase target)
        {
            int Deviazione(int val) => (int)(val * (1 + rnd.Next(-20, 21) / 100.0));
            return (Deviazione(target.Guerrieri), Deviazione(target.Lancieri),
                    Deviazione(target.Arcieri), Deviazione(target.Catapulte));
        }

        // 💰 Esplorazione con costo in oro
        public static (int G, int L, int A, int C) EsploraTruppe(Player g, BarbarianBase target)
        {
            int costo = target.IsGlobal ? 2 : 1; // 500 : 100
            if (g.Oro < costo)
                return (-1, -1, -1, -1); // indicatore di errore

            g.Oro -= costo;
            target.Esplorato = true;

            return StimaTruppe(target); // restituisce (guerrieri, lancieri, arcieri, catapulte)
        }

        // ⚔️ Attacco cooperativo città
        public static string AggiungiPartecipante(int idCitta, string username, int g, int l, int a, int c)
        {
            var citta = CittaGlobali.FirstOrDefault(c => c.Id == idCitta);
            if (citta == null) return "Città non trovata.";

            if (!citta.AttaccoCooperativoAperto)
                return "L'attacco cooperativo non è attualmente aperto per questa città.";

            citta.Partecipanti.Add(new PartecipazioneCoop
            {
                Giocatore = username,
                Guerrieri = g,
                Lancieri = l,
                Arcieri = a,
                Catapulte = c
            });

            return $"{username} ha inviato truppe per attaccare {citta.Nome}!";
        }

        // 🧨 Avvia un attacco cooperativo su una città
        public static string ApriAttaccoCoop(int idCitta)
        {
            var citta = CittaGlobali.FirstOrDefault(c => c.Id == idCitta);
            if (citta == null) return "Città non trovata.";

            if (citta.AttaccoCooperativoAperto)
                return "L'attacco cooperativo è già aperto.";

            citta.AttaccoCooperativoAperto = true;
            citta.Partecipanti.Clear();

            return $"Attacco cooperativo aperto contro {citta.Nome}!";
        }
    }
}

