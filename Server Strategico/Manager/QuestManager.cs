using System.ComponentModel;
using System.Text.Json;
using static Server_Strategico.Gioco.Barbari;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Variabili_Server;

namespace Server_Strategico.Manager
{
    public class QuestManager
    {
        public class Quest_Template
        {
            public int Id { get; set; }                     // ID univoco per identificare la quest
            public string Quest_Description { get; set; }   // Descrizione
            public int Experience { get; set; }             // Exp guadagnata
            public int Require { get; set; }                // Requisito per completare
            public int Max_Complete { get; set; }           // Quante volte può essere completata
        }
        public static class QuestDatabase
        {
            public static readonly Dictionary<int, Quest_Template> Quests = new()
            {
                // --- ACQUISTI E GENERALI ---
                { 0, new Quest_Template { Id = 0, Quest_Description = "Acquista un feudo", Experience = 15, Require = 1, Max_Complete = 5 } },

                // --- ADDESTRAMENTO TRUPPE ---
                { 1, new Quest_Template { Id = 1, Quest_Description = "Addestra guerrieri", Experience = 10, Require = 50, Max_Complete = 3 } },
                { 2, new Quest_Template { Id = 2, Quest_Description = "Addestra lancieri", Experience = 10, Require = 50, Max_Complete = 3 } },
                { 3, new Quest_Template { Id = 3, Quest_Description = "Addestra arcieri", Experience = 10, Require = 50, Max_Complete = 3 } },
                { 4, new Quest_Template { Id = 4, Quest_Description = "Addestra catapulte", Experience = 10, Require = 50, Max_Complete = 3 } },

                // --- ELIMINA TRUPPE --- da fare
                { 5, new Quest_Template { Id = 5, Quest_Description = "Elimina guerrieri", Experience = 10, Require = 80, Max_Complete = 4 } },
                { 6, new Quest_Template { Id = 6, Quest_Description = "Elimina lancieri", Experience = 10, Require = 80, Max_Complete = 4 } },
                { 7, new Quest_Template { Id = 7, Quest_Description = "Elimina arcieri", Experience = 10, Require = 80, Max_Complete = 4 } },
                { 8, new Quest_Template { Id = 8, Quest_Description = "Elimina catapulte", Experience = 10, Require = 80, Max_Complete = 4 } },
                { 9, new Quest_Template { Id = 9, Quest_Description = "Elimina qualsiasi unità", Experience = 10, Require = 400, Max_Complete = 5 } },

                // --- COSTRUZIONI CIVILI ---
                { 10, new Quest_Template { Id = 10, Quest_Description = "Costruisci qualsiasi struttura civile", Experience = 15, Require = 50, Max_Complete = 4 } },
                { 11, new Quest_Template { Id = 11, Quest_Description = "Costruisci fattorie", Experience = 5, Require = 10, Max_Complete = 4 } },
                { 12, new Quest_Template { Id = 12, Quest_Description = "Costruisci segherie", Experience = 5, Require = 10, Max_Complete = 4 } },
                { 13, new Quest_Template { Id = 13, Quest_Description = "Costruisci cave di pietra", Experience = 5, Require = 10, Max_Complete = 4 } },
                { 14, new Quest_Template { Id = 14, Quest_Description = "Costruisci miniere di ferro", Experience = 5, Require = 10, Max_Complete = 4 } },
                { 15, new Quest_Template { Id = 15, Quest_Description = "Costruisci miniere d'oro", Experience = 5, Require = 10, Max_Complete = 4 } },
                { 16, new Quest_Template { Id = 16, Quest_Description = "Costruisci case", Experience = 10, Require = 10, Max_Complete = 4 } },

                { 17, new Quest_Template { Id = 17, Quest_Description = "Costruisci workshop spade", Experience = 5, Require = 5, Max_Complete = 3 } },
                { 18, new Quest_Template { Id = 18, Quest_Description = "Costruisci workshop lancie", Experience = 5, Require = 5, Max_Complete = 3 } },
                { 19, new Quest_Template { Id = 19, Quest_Description = "Costruisci workshop archi", Experience = 5, Require = 5, Max_Complete = 3 } },
                { 20, new Quest_Template { Id = 20, Quest_Description = "Costruisci workshop scudi", Experience = 5, Require = 5, Max_Complete = 3 } },
                { 21, new Quest_Template { Id = 21, Quest_Description = "Costruisci workshop armature", Experience = 5, Require = 5, Max_Complete = 3 } },
                { 22, new Quest_Template { Id = 22, Quest_Description = "Costruisci workshop frecce", Experience = 5, Require = 5, Max_Complete = 3 } },

                { 23, new Quest_Template { Id = 23, Quest_Description = "Costruisci caserme guerrieri", Experience = 10, Require = 5, Max_Complete = 3 } },
                { 24, new Quest_Template { Id = 24, Quest_Description = "Costruisci caserme lancieri", Experience = 10, Require = 5, Max_Complete = 3 } },
                { 25, new Quest_Template { Id = 25, Quest_Description = "Costruisci caserme arcieri", Experience = 10, Require = 5, Max_Complete = 3 } },
                { 26, new Quest_Template { Id = 26, Quest_Description = "Costruisci caserme catapulte", Experience = 10, Require = 5, Max_Complete = 3 } },

                { 27, new Quest_Template { Id = 27, Quest_Description = "Utilizza diamanti blu", Experience = 15, Require = 300, Max_Complete = 4 } },
                { 28, new Quest_Template { Id = 28, Quest_Description = "Utilizza diamanti viola", Experience = 20, Require = 400, Max_Complete = 4 } },

                { 29, new Quest_Template { Id = 29, Quest_Description = "Velocizza costruzioni", Experience = 20, Require = 10800, Max_Complete = 3 } }, //Minuti
                { 30, new Quest_Template { Id = 30, Quest_Description = "Velocizza addestramento", Experience = 20, Require = 14400, Max_Complete = 3 } },
                { 31, new Quest_Template { Id = 31, Quest_Description = "Velocizza qualsiasi cosa", Experience = 25, Require = 28800, Max_Complete = 3 } },

                { 32, new Quest_Template { Id = 32, Quest_Description = "Attacca giocatori", Experience = 15, Require = 5, Max_Complete = 4 } },
                { 33, new Quest_Template { Id = 33, Quest_Description = "Attacca villaggi barbari", Experience = 10, Require = 5, Max_Complete = 4 } },
                { 34, new Quest_Template { Id = 34, Quest_Description = "Attacca città barbare", Experience = 10, Require = 5, Max_Complete = 4 } },

                // --- COSTRUZIONI MILITARI ---
                { 35, new Quest_Template { Id = 35, Quest_Description = "Costruisci qualsiasi struttura militare", Experience = 20, Require = 25, Max_Complete = 4 } },
                { 36, new Quest_Template { Id = 36, Quest_Description = "Addestra qualsiasi unità militare", Experience = 15, Require = 400, Max_Complete = 4 } },

                //Risorse
                { 37, new Quest_Template { Id = 37, Quest_Description = "Utilizza cibo", Experience = 5, Require = 50000, Max_Complete = 4 } },
                { 38, new Quest_Template { Id = 38, Quest_Description = "Utilizza legno", Experience = 5, Require = 42000, Max_Complete = 4 } },
                { 39, new Quest_Template { Id = 39, Quest_Description = "Utilizza pietra", Experience = 5, Require = 38000, Max_Complete = 4 } },
                { 40, new Quest_Template { Id = 40, Quest_Description = "Utilizza ferro", Experience = 5, Require = 35000, Max_Complete = 4 } },
                { 41, new Quest_Template { Id = 41, Quest_Description = "Utilizza oro", Experience = 5, Require = 31000, Max_Complete = 4 } },
                { 42, new Quest_Template { Id = 42, Quest_Description = "Utilizza popolazione", Experience = 5, Require = 320, Max_Complete = 4 } },

                { 43, new Quest_Template { Id = 43, Quest_Description = "Utilizza spade", Experience = 5, Require = 200, Max_Complete = 4 } },
                { 44, new Quest_Template { Id = 44, Quest_Description = "Utilizza lancie", Experience = 5, Require = 200, Max_Complete = 4 } },
                { 45, new Quest_Template { Id = 45, Quest_Description = "Utilizza archi", Experience = 5, Require = 200, Max_Complete = 4 } },
                { 46, new Quest_Template { Id = 46, Quest_Description = "Utilizza scudi", Experience = 5, Require = 200, Max_Complete = 4 } },
                { 47, new Quest_Template { Id = 47, Quest_Description = "Utilizza armature", Experience = 5, Require = 200, Max_Complete = 4 } },
                { 48, new Quest_Template { Id = 48, Quest_Description = "Utilizza frecce", Experience = 5, Require = 1250, Max_Complete = 4 } },

                // --- COSTRUZIONI AVANZATE / SPECIALI ---
                { 49, new Quest_Template { Id = 49, Quest_Description = "Completa tutte le quest iniziali", Experience = 75, Require = 59, Max_Complete = 1 } },

                { 50, new Quest_Template { Id = 50, Quest_Description = "Esplora villaggio barbaro", Experience = 5, Require = 5, Max_Complete = 2 } },
                { 51, new Quest_Template { Id = 51, Quest_Description = "Esplora città barbare", Experience = 5, Require = 5, Max_Complete = 2 } },
                { 52, new Quest_Template { Id = 52, Quest_Description = "Esplora giocatori", Experience = 5, Require = 5, Max_Complete = 2 } },
                { 53, new Quest_Template { Id = 53, Quest_Description = "Esegui qualsiasi esplorazione", Experience = 10, Require = 10, Max_Complete = 2 } },

                { 54, new Quest_Template { Id = 54, Quest_Description = "Raggiungi il livello 5", Experience = 10, Require = 5, Max_Complete = 1 } },
                { 55, new Quest_Template { Id = 55, Quest_Description = "Raggiungi il livello 10", Experience = 15, Require = 10, Max_Complete = 1 } },
                { 56, new Quest_Template { Id = 56, Quest_Description = "Raggiungi il livello 25", Experience = 20, Require = 25, Max_Complete = 1 } },
                { 57, new Quest_Template { Id = 57, Quest_Description = "Raggiungi il livello 50", Experience = 30, Require = 50, Max_Complete = 1 } },
                { 58, new Quest_Template { Id = 58, Quest_Description = "Raggiungi il livello 75", Experience = 40, Require = 75, Max_Complete = 1 } },
                { 59, new Quest_Template { Id = 59, Quest_Description = "Raggiungi il livello 100", Experience = 50, Require = 100, Max_Complete = 1 } },

            };
        }
        public class QuestRewardSet
        {
            public List<int> Rewards { get; set; } = new();
            public List<int> Points { get; set; } = new();

            public static QuestRewardSet Normali_Monthly = new QuestRewardSet
            {
                // V-B-V-B-V-V-B-V-V-V-B-V-V-B-V-V-B-V-V-V --Z nomali
                // V-V-B-V-V-V-V-V-V-V-V-V-V-V-V-V-V-V-V-V

                Rewards = new List<int> { 3, 5, 5, 10, 9, 13, 15, 17, 21, 26, 22, 31, 35, 30, 40, 44, 35, 49, 55, 60 }, //Reward normali per tutti i giocatori
                //Points = new List<int> { 20, 60, 120, 180, 250, 320, 450, 680, 820, 980, 1130, 1250, 1400, 1680, 1810, 1920, 2150, 2375, 2500, 3000 } //Punti richiesti
                Points = new List<int> { 148, 230, 348, 506, 703, 933, 1184, 1440, 1681, 1889, 2057, 2187, 2285, 2359, 2417, 2464, 2504, 2537, 2564, 3000 }
            };

            public static QuestRewardSet Vip_Monthly = new QuestRewardSet
            {
                Rewards = new List<int> { 3, 6, 40, 9, 13, 16, 19, 25, 30, 36, 41, 45, 49, 54, 59, 65, 71, 76, 81, 1 }, //Reward solo per i vip
            };
        }
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
                if (IsQuestFullyCompleted(questId)) // Se la quest è già completata il numero massimo di volte, non fare nulla
                {
                    return false;
                }
                int completata = Completions[questId];  // Quante volte è stata completata finora
                int requireDinamico = quest.Require + completata * quest.Require; // Aumenta il requisito
                CurrentProgress[questId] += amount; // Aggiungi il progresso
                
                if (quest.Id == 1)// solo x i terreni virtuali
                {
                    if (completata == 1) requireDinamico = 3;
                    if (completata == 2) requireDinamico = 5;
                    if (completata == 3) requireDinamico = 7;
                    if (completata == 4) requireDinamico = 10;
                    if (completata == 4) requireDinamico = 14;
                }

                if (CurrentProgress[questId] >= requireDinamico) // 🔹 Se completata
                {
                    player.Punti_Quest += quest.Experience + completata * moltiplicatore_Esperienza; //Aggiunge i punti guadagnati
                    CurrentProgress[questId] = 0; // Resetta il progresso per la prossima volta
                    Completions[questId]++; // Incrementa il conteggio delle completazioni
                    Console.WriteLine($"Quest '{quest.Quest_Description}' completata {Completions[questId]} / {quest.Max_Complete} volte.");

                    if (Completions[questId] == quest.Max_Complete)
                    {
                        OnEvent(player, QuestEventType.Miglioramento, "", 1); // Per ogni quest completata, aggiorna la quest
                        player.Quest_Completate++;
                        QuestRewardUpdate(player);
                    }
                    return true; // Quest completata
                }
                return false;
            }
        }

        public enum QuestEventType // Tipi di eventi triggerati per le quest
        {
            Costruzione,
            Addestramento,
            Uccisioni,
            Risorse,
            Battaglie,
            Miglioramento,
            Velocizzazione,
            Livelli
        }
        public static void OnEvent(Player player, QuestEventType eventType, string targetName, int amount = 1) // 🔸 Metodo principale: riceve un evento dal gioco
        {
            if (player == null) return; //Richiamo evento --> QuestManager.OnEvent(player, QuestEventType.Costruzione, "Fattoria", 1);

            switch (eventType)
            {
                case QuestEventType.Costruzione:
                    GestisciCostruzione(player, targetName, amount);
                    break;
                case QuestEventType.Addestramento:
                    GestisciAddestramento(player, targetName, amount);
                    break;
                case QuestEventType.Uccisioni:
                    GestisciUccisioni(player, targetName, amount);
                    break;
                case QuestEventType.Risorse:
                    GestisciRisorse(player, targetName, amount);
                    break;
                case QuestEventType.Battaglie:
                    GestisciBattaglie(player, targetName, amount);
                    break;
                case QuestEventType.Miglioramento:
                    GestisciMiglioramento(player, targetName, amount);
                    break;
                case QuestEventType.Velocizzazione:
                    GestisciVelocizzazione(player, targetName, amount);
                    break;
                case QuestEventType.Livelli:
                    GestisciLivelli(player, targetName, amount);
                    break;
            }
        }

        private static void GestisciCostruzione(Player player, string tipo, int quantita) // 🔸 GESTIONE SPECIFICA PER OGNI TIPO DI QUEST
        {
            // Costruzioni civili specifiche
            switch (tipo)
            {
                case "Terreno":
                    player.QuestProgress.AddProgress(0, quantita, player);
                    break;

                case "Fattoria":
                    player.QuestProgress.AddProgress(11, quantita, player);
                    player.QuestProgress.AddProgress(10, quantita, player); // qualsiasi struttura civile
                    break;
                case "Segheria":
                    player.QuestProgress.AddProgress(12, quantita, player);
                    player.QuestProgress.AddProgress(10, quantita, player); // qualsiasi struttura civile
                    break;
                case "CavaPietra":
                    player.QuestProgress.AddProgress(13, quantita, player);
                    player.QuestProgress.AddProgress(10, quantita, player); // qualsiasi struttura civile
                    break;
                case "MinieraFerro":
                    player.QuestProgress.AddProgress(14, quantita, player);
                    player.QuestProgress.AddProgress(10, quantita, player); // qualsiasi struttura civile
                    break;
                case "MinieraOro":
                    player.QuestProgress.AddProgress(15, quantita, player);
                    player.QuestProgress.AddProgress(10, quantita, player); // qualsiasi struttura civile
                    break;
                case "Case":
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
            switch (tipo)
            {
                case "Guerrieri":
                    player.QuestProgress.AddProgress(1, quantita, player);
                    break;
                case "Lanceri":
                    player.QuestProgress.AddProgress(2, quantita, player);
                    break;
                case "Arceri":
                    player.QuestProgress.AddProgress(3, quantita, player);
                    break;
                case "Catapulta":
                    player.QuestProgress.AddProgress(4, quantita, player);
                    break;
            }
            player.QuestProgress.AddProgress(36, quantita, player); //Addestra qualsiasi unità
        }
        private static void GestisciBattaglie(Player player, string tipo, int quantita)
        {
            switch (tipo)
            {
                case "Attacca Giocatore":
                    player.QuestProgress.AddProgress(32, quantita, player);
                    break;
                case "Attacco Villaggio Barbaro":
                    player.QuestProgress.AddProgress(33, quantita, player);
                    break;
                case "Attacco Citta Barbaro":
                    player.QuestProgress.AddProgress(34, quantita, player);
                    break;

                case "Esplora Giocatore":
                    player.QuestProgress.AddProgress(50, quantita, player);
                    break;
                case "Esplora Villaggio Barbaro":
                    player.QuestProgress.AddProgress(51, quantita, player);
                    break;
                case "Esplora Citta Barbaro":
                    player.QuestProgress.AddProgress(52, quantita, player);
                    break;
                case "Esplora Qualsiasi":
                    player.QuestProgress.AddProgress(53, quantita, player);
                    break;
            }
        }
        private static void GestisciUccisioni(Player player, string tipo, int quantita)
        {
            switch (tipo)
            {
                case "Guerrieri":
                    player.QuestProgress.AddProgress(5, quantita, player);
                    break;
                case "Lanceri":
                    player.QuestProgress.AddProgress(6, quantita, player);
                    break;
                case "Arceri":
                    player.QuestProgress.AddProgress(7, quantita, player);
                    break;
                case "Catapulta":
                    player.QuestProgress.AddProgress(8, quantita, player);
                    break;
            }

            player.QuestProgress.AddProgress(9, quantita, player); //Uccisione truppe generico
        }
        private static void GestisciRisorse(Player player, string tipo, int quantita)
        {
            switch (tipo)
            {
                case "Cibo":
                    player.QuestProgress.AddProgress(37, quantita, player);
                    break;
                case "Legno":
                    player.QuestProgress.AddProgress(38, quantita, player);
                    break;
                case "Pietra":
                    player.QuestProgress.AddProgress(39, quantita, player);
                    break;
                case "Ferro":
                    player.QuestProgress.AddProgress(40, quantita, player);
                    break;
                case "Oro":
                    player.QuestProgress.AddProgress(41, quantita, player);
                    break;
                case "Popolazione":
                    player.QuestProgress.AddProgress(42, quantita, player);
                    break;

                case "Spade":
                    player.QuestProgress.AddProgress(43, quantita, player);
                    break;
                case "Lance":
                    player.QuestProgress.AddProgress(44, quantita, player);
                    break;
                case "Archi":
                    player.QuestProgress.AddProgress(45, quantita, player);
                    break;
                case "Scudi":
                    player.QuestProgress.AddProgress(46, quantita, player);
                    break;
                case "Armature":
                    player.QuestProgress.AddProgress(47, quantita, player);
                    break;
                case "Frecce":
                    player.QuestProgress.AddProgress(48, quantita, player);
                    break;

                case "Diamanti Viola":
                    player.QuestProgress.AddProgress(28, quantita, player);
                    break;
                case "Diamanti Blu":
                    player.QuestProgress.AddProgress(27, quantita, player);
                    break;
            }
        }
        private static void GestisciVelocizzazione(Player player, string tipo, int quantita)
        {
            switch (tipo)
            {
                case "Costruzione":
                    player.QuestProgress.AddProgress(29, quantita, player);
                    break;
                case "Addestramento":
                    player.QuestProgress.AddProgress(30, quantita, player);
                    break;
                case "Qualsiasi":
                    player.QuestProgress.AddProgress(31, quantita, player);
                    break;
            }
        }
        private static void GestisciMiglioramento(Player player, string tipo, int quantita) //Completa tutte le quest.
        {
            player.QuestProgress.AddProgress(49, quantita, player);
        }
        private static void GestisciLivelli(Player player, string tipo, int quantita)
        {
            player.QuestProgress.AddProgress(54, quantita, player);
            player.QuestProgress.AddProgress(55, quantita, player);
            player.QuestProgress.AddProgress(56, quantita, player);
            player.QuestProgress.AddProgress(57, quantita, player);
            player.QuestProgress.AddProgress(58, quantita, player);
            player.QuestProgress.AddProgress(59, quantita, player);
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
                    int requireDinamico = q.Require + completata * q.Require;

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
                var completo = player.PremiNormali;
                var completo_vip = player.PremiVIP;

                var packet = new // Crea il pacchetto
                {
                    Type = "QuestRewards",
                    Rewards_Normali = rewardsNormali,
                    Rewards_VIP = rewardsVip,
                    Points = points,
                    Completo = completo,
                    Completo_Vip = completo_vip
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

        public static void RigeneraQuest() // 🔁 Rigenera città globali e villaggi personali
        {
            Console.WriteLine($"[QuestMensile] Reset quest mensile iniziata ({DateTime.Now:HH:mm:ss})");
            if (timer_Reset_Quest == 0)
                timer_Reset_Quest = 30 * 24 * 60 * 60; //30 giorni in secondi

            foreach (var player in Server.Server.servers_.players.Values) // Rigenera quest mensili per ogni giocatore
            {
                for (int i = 0; i < player.QuestProgress.Completions.Length; i++) // Resetta il numero di completamenti delle quest mensili
                    player.QuestProgress.Completions[i] = 0;

                for (int i = 0; i < player.QuestProgress.CurrentProgress.Length; i++) // Resetta i progressi delle quest mensili
                    player.QuestProgress.CurrentProgress[i] = 0;

                for (int i = 0; i < player.PremiNormali.Length; i++) // Resetta la raccolta dei premi normali
                    player.PremiNormali[i] = false;

                for (int i = 0; i < player.PremiNormali.Length; i++) // Resetta la raccolta dei premi vip
                    player.PremiVIP[i] = false;

                player.Punti_Quest = 0;
                Console.WriteLine($"[QuestMensile] Reset completato: {player.Username} quest mensili e progressi per {Server.Server.servers_.players.Count} giocatori.");
            }
        }
    }
}
