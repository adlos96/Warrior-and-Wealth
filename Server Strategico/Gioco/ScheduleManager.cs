using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Server_Strategico.Gioco
{
    internal class ScheduleManager
    {
        private Timer _timer;
        private Dictionary<string, DateTime> _lastResets;
        private DayOfWeek _weeklyResetDay = DayOfWeek.Monday; // Giorno del reset settimanale
        private int _dailyResetHour = 6; // Ora del reset giornaliero

        public ScheduleManager()
        {
            _lastResets = new Dictionary<string, DateTime>();
            _timer = new Timer(60000); // Controlla ogni minuto
            _timer.Elapsed += CheckResets;
            _timer.Start();
        }

        private void CheckResets(object sender, ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;

            // Reset giornaliero alle 00:00
            if (ShouldResetDaily("daily", now, _dailyResetHour, 0))
                ResetGiornaliero();

            // Reset settimanale (lunedì alle 00:00)
            if (ShouldResetWeekly("weekly", now, _weeklyResetDay, _dailyResetHour, 0))
                ResetSettimanale();

            // Reset mensile (primo giorno del mese alle 00:00)
            if (ShouldResetMonthly("monthly", now, _dailyResetHour, 0))
                ResetMensile();
            
        }

        private bool ShouldResetDaily(string key, DateTime now, int hour, int minute)
        {
            if (!_lastResets.ContainsKey(key)) _lastResets[key] = DateTime.MinValue;

            DateTime lastReset = _lastResets[key];
            DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);

            if (lastReset.Date < now.Date && now >= targetTime) // Esegui se è un nuovo giorno e abbiamo passato l'orario target
            {
                _lastResets[key] = now;
                return true;
            }
            return false;
        }

        private bool ShouldResetWeekly(string key, DateTime now, DayOfWeek resetDay, int hour, int minute)
        {
            if (!_lastResets.ContainsKey(key))  _lastResets[key] = DateTime.MinValue;

            DateTime lastReset = _lastResets[key];

            // Calcola l'inizio della settimana corrente e precedente
            int daysFromMonday = ((int)now.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            DateTime weekStart = now.Date.AddDays(-daysFromMonday);

            int lastDaysFromMonday = ((int)lastReset.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            DateTime lastWeekStart = lastReset.Date.AddDays(-lastDaysFromMonday);
            DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);

            // Esegui se siamo in una nuova settimana, è il giorno giusto e abbiamo passato l'orario
            if (weekStart > lastWeekStart && now.DayOfWeek == resetDay && now >= targetTime)
            {
                _lastResets[key] = now;
                return true;
            }
            return false;
        }

        private bool ShouldResetMonthly(string key, DateTime now, int hour, int minute)
        {
            if (!_lastResets.ContainsKey(key)) _lastResets[key] = DateTime.MinValue;

            DateTime lastReset = _lastResets[key];
            DateTime targetTime = new DateTime(now.Year, now.Month, 1, hour, minute, 0);

            // Esegui se siamo in un nuovo mese, è il primo giorno e abbiamo passato l'orario
            bool isNewMonth = (now.Year > lastReset.Year) || (now.Year == lastReset.Year && now.Month > lastReset.Month);
            if (isNewMonth && now.Day == 1 && now >= targetTime)
            {
                _lastResets[key] = now;
                return true;
            }
            return false;
        }

        // ===== FUNZIONI DI RESET - PERSONALIZZALE QUI =====

        private void ResetGiornaliero()
        {
            Console.WriteLine($"[{DateTime.Now}] RESET GIORNALIERO ATTIVO");
            Variabili_Server.Reset_Gironaliero = true;
        }

        private void ResetSettimanale()
        {
            Console.WriteLine($"[{DateTime.Now}] RESET SETTIMANALE ATTIVO");
            Variabili_Server.Reset_Settimanale = true;
        }

        private void ResetMensile()
        {
            Console.WriteLine($"[{DateTime.Now}] RESET MENSILE ATTIVO");
            Variabili_Server.Reset_Mensile = true;
        }

        // ===== COMANDI MANUALI (OPZIONALI) =====

        public void ForzaResetGiornaliero()
        {
            ResetGiornaliero();
            _lastResets["daily"] = DateTime.Now;
        }

        public void ForzaResetSettimanale()
        {
            ResetSettimanale();
            _lastResets["weekly"] = DateTime.Now;
        }

        public void ForzaResetMensile()
        {
            ResetMensile();
            _lastResets["monthly"] = DateTime.Now;
        }

        public void Stop()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }

        // ===== OTTIENI TEMPO MANCANTE AI RESET =====

        public TimeSpan GetTempoAlResetGiornaliero()
        {
            DateTime now = DateTime.Now;
            DateTime nextReset = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0); // Mezzanotte

            if (now >= nextReset)
            {
                nextReset = nextReset.AddDays(1); // Domani
            }

            return nextReset - now;
        }

        public TimeSpan GetTempoAlResetSettimanale()
        {
            DateTime now = DateTime.Now;
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)now.DayOfWeek + 7) % 7;

            if (daysUntilMonday == 0 && now.Hour >= 0) // Se è lunedì ma dopo mezzanotte
                daysUntilMonday = 7;

            DateTime nextReset = now.Date.AddDays(daysUntilMonday);
            nextReset = new DateTime(nextReset.Year, nextReset.Month, nextReset.Day, 0, 0, 0);
            return nextReset - now;
        }

        public TimeSpan GetTempoAlResetMensile()
        {
            DateTime now = DateTime.Now;
            DateTime nextReset;

            if (now.Day == 1 && now.Hour < 0) // Se è il primo giorno ma prima di mezzanotte
               nextReset = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
            else // Prossimo mese
            {
                if (now.Month == 12) nextReset = new DateTime(now.Year + 1, 1, 1, 0, 0, 0);
                else nextReset = new DateTime(now.Year, now.Month + 1, 1, 0, 0, 0);
            }
            return nextReset - now;
        }

        // Formatta il tempo in modo leggibile
        public string FormatTempo(TimeSpan tempo)
        {
            if (tempo.TotalDays >= 1)  return $"{(int)tempo.TotalDays}g {tempo.Hours}h {tempo.Minutes}m";
            else if (tempo.TotalHours >= 1)  return $"{(int)tempo.TotalHours}h {tempo.Minutes}m";
            else return $"{tempo.Minutes}m {tempo.Seconds}s";
        }

        // Stampa tutti i tempi a console
        public void PrintTempiReset()
        {
            Console.WriteLine("\n=== TEMPI PROSSIMI RESET ===");
            Console.WriteLine($"Reset Giornaliero: {FormatTempo(GetTempoAlResetGiornaliero())}");
            Console.WriteLine($"Reset Settimanale: {FormatTempo(GetTempoAlResetSettimanale())}");
            Console.WriteLine($"Reset Mensile: {FormatTempo(GetTempoAlResetMensile())}");
            Console.WriteLine("============================\n");
        }

        // Per inviare ai giocatori (restituisce un dizionario)
        public Dictionary<string, string> GetTempiResetPerGiocatori()
        {
            return new Dictionary<string, string>
            {
                { "daily", FormatTempo(GetTempoAlResetGiornaliero()) },
                { "weekly", FormatTempo(GetTempoAlResetSettimanale()) },
                { "monthly", FormatTempo(GetTempoAlResetMensile()) }
            };
        }

        public static void AvvioReset() // ESEMPIO DI USO 
        {
            // Avvia il sistema di reset
            var resetManager = new ScheduleManager();
            Console.WriteLine("Sistema reset avviato...");

            // Stampa i tempi rimanenti
            resetManager.PrintTempiReset();

            // Ottieni i tempi per inviarli ai giocatori
            var tempi = resetManager.GetTempiResetPerGiocatori();
            Console.WriteLine($"Messaggio per i giocatori:");
            Console.WriteLine($"Reset giornaliero tra: {tempi["daily"]}");
            Console.WriteLine($"Reset settimanale tra: {tempi["weekly"]}");
            Console.WriteLine($"Reset mensile tra: {tempi["monthly"]}");
        }
    }    
}

