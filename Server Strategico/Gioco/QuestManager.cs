using System.Text.Json;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Variabili_Server;

namespace Server_Strategico.Gioco
{
    public class QuestManager
    {
        public class PlayerQuestProgress
        {
            public int[] Completions { get; set; } = new int[QuestDatabase.Quests.Count]; // Indica quante volte ogni quest è stata completata
            public int[] CurrentProgress { get; set; } = new int[QuestDatabase.Quests.Count]; // Puoi anche tenere traccia di progressi parziali

            public bool IsQuestFullyCompleted(int questId) // Restituisce true se la quest è completata il numero massimo di volte
            {
                return Completions[questId] >= QuestDatabase.Quests[questId].Max_Complete;
            }

            public bool AddProgress(int questId, int amount, Player player)
            {
                var quest = QuestDatabase.Quests[questId];

                // Se la quest è già completata il numero massimo di volte, non fare nulla
                if (IsQuestFullyCompleted(questId))
                {
                    Console.WriteLine($"Quest '{quest.Quest_Description}' è già completata al massimo ({quest.Max_Complete} volte).");
                    return false;
                }

                int completata = Completions[questId];  // Quante volte è stata completata finora
                int requireDinamico = quest.Require + (completata * Variabili_Server.moltiplicatore_Quest); // Aumenta il requisito di 5 per ogni completamento
                player.Punti_Quest += quest.Experience;
                CurrentProgress[questId] += amount; // Aggiungi il progresso

                if (CurrentProgress[questId] >= requireDinamico) // 🔹 Se completata
                {
                    CurrentProgress[questId] = 0; // Resetta il progresso per la prossima volta
                    Completions[questId]++; // Incrementa il conteggio delle completazioni
                    Console.WriteLine($"Quest '{quest.Quest_Description}' completata {Completions[questId]} / {quest.Max_Complete} volte.");

                    if (Completions[questId] == quest.Max_Complete)
                        QuestManager.OnEvent(player, QuestEventType.Miglioramento, "", 1); // Per ogni quest completata, aggiorna la quest
                    return true; // Quest completata
                }
                return false;
            }
        }

        public enum QuestEventType // Tipi di eventi triggerati per le quest
        {
            Costruzione,
            Addestramento,
            Eliminazione,
            Acquisto,
            Miglioramento
        }

        public static void OnEvent(Player player, QuestEventType eventType, string targetName, int amount = 1) // 🔸 Metodo principale: riceve un evento dal gioco
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
            QuestUpdate(player); // Dopo ogni evento → invia aggiornamento al client
        }

        private static void GestisciCostruzione(Player player, string tipo, int quantita) // 🔸 GESTIONE SPECIFICA PER OGNI TIPO DI QUEST
        {
            // Costruzioni civili specifiche
            switch (tipo)
            {
                case "Fattoria":
                    player.QuestProgress.AddProgress(11, quantita, player);
                    player.QuestProgress.AddProgress(10, quantita, player); // qualsiasi struttura civile
                    break;
                case "Segheria":
                    player.QuestProgress.AddProgress(12, quantita, player);
                    player.QuestProgress.AddProgress(10, quantita, player); // qualsiasi struttura civile
                    break;
                case "Cava di Pietra":
                    player.QuestProgress.AddProgress(13, quantita, player);
                    player.QuestProgress.AddProgress(10, quantita, player); // qualsiasi struttura civile
                    break;
                case "Miniera Ferro":
                    player.QuestProgress.AddProgress(14, quantita, player);
                    player.QuestProgress.AddProgress(10, quantita, player); // qualsiasi struttura civile
                    break;
                case "Miniera Oro":
                    player.QuestProgress.AddProgress(15, quantita, player);
                    player.QuestProgress.AddProgress(10, quantita, player); // qualsiasi struttura civile
                    break;
                case "Casa":
                    player.QuestProgress.AddProgress(16, quantita, player);
                    player.QuestProgress.AddProgress(10, quantita, player); // qualsiasi struttura civile
                    break;

                case "ProduzioneSpade":
                    player.QuestProgress.AddProgress(17, quantita, player);
                    player.QuestProgress.AddProgress(35, quantita, player); // struttura militare generica
                    break;
                case "ProduzioneLance":
                    player.QuestProgress.AddProgress(18, quantita, player);
                    player.QuestProgress.AddProgress(35, quantita, player); // struttura militare generica
                    break;
                case "ProduzioneArchi":
                    player.QuestProgress.AddProgress(19, quantita, player);
                    player.QuestProgress.AddProgress(35, quantita, player); // struttura militare generica
                    break;
                case "ProduzioneScudi":
                    player.QuestProgress.AddProgress(20, quantita, player);
                    player.QuestProgress.AddProgress(35, quantita, player); // struttura militare generica
                    break;
                case "ProduzioneArmature":
                    player.QuestProgress.AddProgress(21, quantita, player);
                    player.QuestProgress.AddProgress(35, quantita, player); // struttura militare generica
                    break;
                case "ProduzioneFrecce":
                    player.QuestProgress.AddProgress(22, quantita, player);
                    player.QuestProgress.AddProgress(35, quantita, player); // struttura militare generica
                    break;

                case "CasermaGuerrieri":
                    player.QuestProgress.AddProgress(23, quantita, player);
                    player.QuestProgress.AddProgress(35, quantita, player); // struttura militare generica
                    break;
                case "CasermaLancieri":
                    player.QuestProgress.AddProgress(24, quantita, player);
                    player.QuestProgress.AddProgress(35, quantita, player); // struttura militare generica
                    break;
                case "CasermaArcieri":
                    player.QuestProgress.AddProgress(25, quantita, player);
                    player.QuestProgress.AddProgress(35, quantita, player); // struttura militare generica
                    break;
                case "CasermaCatapulte":
                    player.QuestProgress.AddProgress(26, quantita, player);
                    player.QuestProgress.AddProgress(35, quantita, player); // struttura militare generica
                    break;

            }
        }

        private static void GestisciAddestramento(Player player, string tipo, int quantita)
        {
            if (tipo == "Guerrieri_1") player.QuestProgress.AddProgress(1, quantita, player);
            else if (tipo == "Lanceri_1") player.QuestProgress.AddProgress(2, quantita, player);
            else if (tipo == "Arceri_1") player.QuestProgress.AddProgress(3, quantita, player);
            else if (tipo == "Catapulta") player.QuestProgress.AddProgress(4, quantita, player);

            player.QuestProgress.AddProgress(36, quantita, player); //Addestramento truppe generico
        }

        private static void GestisciEliminazione(Player player, string tipo, int quantita)
        {
            if (tipo == "Guerrieri_1") player.QuestProgress.AddProgress(5, quantita, player);
            else if (tipo == "Lanceri_1") player.QuestProgress.AddProgress(6, quantita, player);
            else if (tipo == "Arceri_1") player.QuestProgress.AddProgress(7, quantita, player);
            else if (tipo == "Catapulta") player.QuestProgress.AddProgress(8, quantita, player);

            player.QuestProgress.AddProgress(9, quantita, player); //Eliminazione truppe generico
        }

        private static void GestisciAcquisto(Player player, string tipo, int quantita)
        {
            if (tipo == "Terreno") player.QuestProgress.AddProgress(0, quantita, player);
        }

        private static void GestisciMiglioramento(Player player, string tipo, int quantita) //Completa tutte le quest.
        {
            player.QuestProgress.AddProgress(47, quantita, player);
        }

        public static void QuestUpdate(Player player) // 🔸 INVIO AL CLIENT
        {
            var questData = QuestDatabase.Quests.Values
                // 🔹 Filtra solo le quest non completamente completate
                .Where(q => player.QuestProgress.Completions[q.Id] < q.Max_Complete)
                .Select(q =>
                {
                    int completata = player.QuestProgress.Completions[q.Id]; // Quante volte è stata completata
                    int progress = player.QuestProgress.CurrentProgress[q.Id]; // Progresso attuale

                    int experienceBase = q.Experience;
                    int experienceBonus = experienceBase + completata * moltiplicatore_Esperienza;
                    int requireDinamico = q.Require + completata * moltiplicatore_Quest;

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
                })
                .ToList();

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

                var packet = new // Crea il pacchetto
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
