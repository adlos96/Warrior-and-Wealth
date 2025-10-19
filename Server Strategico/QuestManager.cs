using System.Text.Json;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Variabili_Server;

namespace Server_Strategico
{
    internal class QuestManager
    {
        public enum QuestEventType // Tipi di eventi triggerati per le quest
        {
            Costruzione,
            Addestramento,
            Eliminazione,
            Acquisto,
            Miglioramento
        }

        // ✅ Inizializza (se serve) – ad esempio puoi caricare dati o collegare eventi
        public static void Initialize()
        {
            Console.WriteLine("QuestManager inizializzato con " + QuestDatabase.Quests.Count + " quest totali."); //Debug
        }

        // 🔸 Metodo principale: riceve un evento dal gioco
        public static void OnEvent(Player player, QuestEventType eventType, string targetName, int amount = 1)
        {
            //Richiamo evento --> QuestManager.OnEvent(player, QuestEventType.Costruzione, "Fattoria", 1);
            if (player == null) return;

            switch (eventType)
            {
                case QuestEventType.Costruzione:
                    GestisciCostruzione(player, targetName, amount);
                    break;

                case QuestEventType.Addestramento:
                    GestisciAddestramento(player, targetName, amount);
                    break;

                case QuestEventType.Eliminazione:
                    GestisciEliminazione(player, targetName, amount);
                    break;

                case QuestEventType.Acquisto:
                    GestisciAcquisto(player, targetName, amount);
                    break;

                case QuestEventType.Miglioramento:
                    GestisciMiglioramento(player, targetName, amount);
                    break;
            }

            // Dopo ogni evento → invia aggiornamento al client
            QuestUpdate(player);
        }

        // 🔸 GESTIONE SPECIFICA PER OGNI TIPO DI QUEST

        private static void GestisciCostruzione(Player player, string tipo, int quantita)
        {
            // Costruzioni civili specifiche
            switch (tipo)
            {
                case "Fattoria":
                    player.QuestProgress.AddProgress(11, quantita);
                    player.QuestProgress.AddProgress(10, quantita); // qualsiasi struttura civile
                    break;
                    case "Segheria":
                    player.QuestProgress.AddProgress(12, quantita);
                    player.QuestProgress.AddProgress(10, quantita); // qualsiasi struttura civile
                    break;
                    case "Cava di Pietra":
                    player.QuestProgress.AddProgress(13, quantita);
                    player.QuestProgress.AddProgress(10, quantita); // qualsiasi struttura civile
                    break;
                    case "Miniera Ferro":
                    player.QuestProgress.AddProgress(14, quantita);
                    player.QuestProgress.AddProgress(10, quantita); // qualsiasi struttura civile
                    break;
                    case "Miniera Oro":
                    player.QuestProgress.AddProgress(15, quantita);
                    player.QuestProgress.AddProgress(10, quantita); // qualsiasi struttura civile
                    break;
                    case "Casa":
                    player.QuestProgress.AddProgress(16, quantita);
                    player.QuestProgress.AddProgress(10, quantita); // qualsiasi struttura civile
                    break;

                case "ProduzioneSpade":
                    player.QuestProgress.AddProgress(17, quantita); // struttura militare generica
                    break;
                case "ProduzioneLance":
                    player.QuestProgress.AddProgress(17, quantita); // struttura militare generica
                    break;
                case "ProduzioneArchi":
                    player.QuestProgress.AddProgress(17, quantita); // struttura militare generica
                    break;
                case "ProduzioneScudi":
                    player.QuestProgress.AddProgress(17, quantita); // struttura militare generica
                    break;
                case "ProduzioneArmature":
                    player.QuestProgress.AddProgress(17, quantita); // struttura militare generica
                    break;
                case "ProduzioneFrecce":
                    player.QuestProgress.AddProgress(17, quantita); // struttura militare generica
                    break;

                case "CasermaGuerrieri":
                    player.QuestProgress.AddProgress(17, quantita); // struttura militare generica
                    break;
                case "CasermaLancieri":
                    player.QuestProgress.AddProgress(17, quantita); // struttura militare generica
                    break;
                case "CasermaArcieri":
                    player.QuestProgress.AddProgress(17, quantita); // struttura militare generica
                    break;
                case "CasermaCatapulte":
                    player.QuestProgress.AddProgress(17, quantita); // struttura militare generica
                    break;

            }
        }

        private static void GestisciAddestramento(Player player, string tipo, int quantita)
        {
            if (tipo == "Guerrieri_1") player.QuestProgress.AddProgress(1, quantita);
            else if (tipo == "Lanceri_1") player.QuestProgress.AddProgress(2, quantita);
            else if (tipo == "Arceri_1") player.QuestProgress.AddProgress(3, quantita);
            else if (tipo == "Catapulta") player.QuestProgress.AddProgress(4, quantita);
        }

        private static void GestisciEliminazione(Player player, string tipo, int quantita)
        {
            if (tipo == "Guerrieri_1") player.QuestProgress.AddProgress(5, quantita);
            else if (tipo == "Lanceri_1") player.QuestProgress.AddProgress(6, quantita);
            else if (tipo == "Arceri_1") player.QuestProgress.AddProgress(7, quantita);
            else if (tipo == "Catapulta") player.QuestProgress.AddProgress(8, quantita);

            // Quest generica
            player.QuestProgress.AddProgress(9, quantita);
        }

        private static void GestisciAcquisto(Player player, string tipo, int quantita)
        {
            if (tipo == "Terreno") player.QuestProgress.AddProgress(0, quantita);
        }

        private static void GestisciMiglioramento(Player player, string tipo, int quantita)
        {
            player.QuestProgress.AddProgress(19, quantita);
        }

        // 🔸 INVIO AL CLIENT

        public static void QuestUpdate(Player player)
        {
            var questData = QuestDatabase.Quests.Values.Select(q =>
            {
                int completata = player.QuestProgress.Completions[q.Id];
                int progress = player.QuestProgress.CurrentProgress[q.Id];

                int experienceBase = q.Experience;
                int experienceBonus = experienceBase + (completata * 10); // esempio +10 exp per ogni completamento
                int requireDinamico = q.Require + (completata * 3);

                return new
                {
                    q.Id,
                    q.Quest_Description,
                    Experience = experienceBonus,
                    Require = requireDinamico,
                    Progress = progress,
                    q.Max_Complete,
                    Completata = completata
                };
            }).ToList();

            var questUpdate = new
            {
                Type = "QuestUpdate",
                Quests = questData
            };

            string json = JsonSerializer.Serialize(questUpdate);
            Server.Server.Send(player.guid_Player, json);
        }
        public static void QuestRewardUpdate(Player player)
        {
            try
            {
                // Prendi tutti i set
                var rewardsNormali = QuestRewardSet.Normali_Monthly.Rewards;
                var rewardsVip = QuestRewardSet.Vip_Monthly.Rewards;
                var points = QuestRewardSet.Normali_Monthly.Points;

                // Crea il pacchetto
                var packet = new
                {
                    Type = "QuestRewards",
                    Rewards_Normali = rewardsNormali,
                    Rewards_VIP = rewardsVip,
                    Points = points,
                    VipPlayer = player.Vip // Info utile lato client
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(packet, options);
                Server.Server.Send(player.guid_Player, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRORE] Invio QuestRewards fallito per {player.Username}: {ex.Message}");
            }
        }
    }
}
