
namespace Server_Strategico.Gioco
{
    internal class Variabili_Server
    {
        public static int moltiplicatore_Quest = 1;
        public static int moltiplicatore_Esperienza = 10;

        public class QuestRewardPacket
        {
            public string Type { get; set; } = "QuestRewards";
            public List<int> Rewards { get; set; }
            public List<int> Points { get; set; }
        }

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
                { 0, new Quest_Template { Id = 0, Quest_Description = "Acquista un terreno virtuale", Experience = 10, Require = 1, Max_Complete = 4 } },

                // --- ADDESTRAMENTO TRUPPE ---
                { 1, new Quest_Template { Id = 1, Quest_Description = "Addestra guerrieri", Experience = 10, Require = 50, Max_Complete = 3 } },
                { 2, new Quest_Template { Id = 2, Quest_Description = "Addestra lancieri", Experience = 10, Require = 50, Max_Complete = 3 } },
                { 3, new Quest_Template { Id = 3, Quest_Description = "Addestra arcieri", Experience = 10, Require = 50, Max_Complete = 3 } },
                { 4, new Quest_Template { Id = 4, Quest_Description = "Addestra catapulte", Experience = 10, Require = 50, Max_Complete = 3 } },

                // --- ELIMINA TRUPPE ---
                { 5, new Quest_Template { Id = 5, Quest_Description = "Elimina guerrieri", Experience = 10, Require = 50, Max_Complete = 3 } },
                { 6, new Quest_Template { Id = 6, Quest_Description = "Elimina lancieri", Experience = 10, Require = 50, Max_Complete = 3 } },
                { 7, new Quest_Template { Id = 7, Quest_Description = "Elimina arcieri", Experience = 10, Require = 50, Max_Complete = 3 } },
                { 8, new Quest_Template { Id = 8, Quest_Description = "Elimina catapulte", Experience = 10, Require = 50, Max_Complete = 3 } },
                { 9, new Quest_Template { Id = 9, Quest_Description = "Elimina qualsiasi unità", Experience = 10, Require = 500, Max_Complete = 3 } },

                // --- COSTRUZIONI CIVILI ---
                { 10, new Quest_Template { Id = 10, Quest_Description = "Costruisci qualsiasi struttura civile", Experience = 10, Require = 15, Max_Complete = 4 } },
                { 11, new Quest_Template { Id = 11, Quest_Description = "Costruisci fattorie", Experience = 10, Require = 5, Max_Complete = 4 } },
                { 12, new Quest_Template { Id = 12, Quest_Description = "Costruisci segherie", Experience = 10, Require = 5, Max_Complete = 4 } },
                { 13, new Quest_Template { Id = 13, Quest_Description = "Costruisci cave di pietra", Experience = 10, Require = 5, Max_Complete = 4 } },
                { 14, new Quest_Template { Id = 14, Quest_Description = "Costruisci miniere di ferro", Experience = 10, Require = 5, Max_Complete = 4 } },
                { 15, new Quest_Template { Id = 15, Quest_Description = "Costruisci miniere d'oro", Experience = 10, Require = 5, Max_Complete = 4 } },
                { 16, new Quest_Template { Id = 16, Quest_Description = "Costruisci case", Experience = 10, Require = 5, Max_Complete = 4 } },

                // --- COSTRUZIONI MILITARI ---
                { 17, new Quest_Template { Id = 17, Quest_Description = "Costruisci qualsiasi struttura militare", Experience = 10, Require = 15, Max_Complete = 4 } },

                // --- COSTRUZIONI AVANZATE / SPECIALI ---
                { 18, new Quest_Template { Id = 18, Quest_Description = "Completa tutte le quest iniziali", Experience = 50, Require = 17, Max_Complete = 1 } },
            };
        }

        public class QuestRewardSet
        {
            public List<int> Rewards { get; set; } = new();
            public List<int> Points { get; set; } = new();

            public static QuestRewardSet Normali_Monthly = new QuestRewardSet
            {
                Rewards = new List<int> { 5, 10, 7, 15, 9, 13, 19, 17, 25, 30, 23, 40, 50, 27, 60, 80, 34, 110, 130, 150 },
                Points = new List<int> { 20, 60, 120, 180, 250, 320, 450, 680, 820, 980, 1130, 1250, 1400, 1680, 1810, 1920, 2150, 2375, 2500, 3000 }
            };

            public static QuestRewardSet Vip_Monthly = new QuestRewardSet
            {
                Rewards = new List<int> { 1, 2, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
        }

        public class Shop
        {
            public double Costo { get; set; }
            public int Reward { get; set; }

            public static Shop Vip_1 = new Shop
            {
                Costo = 500, //Diamanti_Viola
                Reward = 1 //VIP
            };
            public static Shop Vip_2 = new Shop
            {
                Costo = 14.99, //USDT
                Reward = 1 //VIP
            };

            public static Shop Pacchetto_1 = new Shop
            {
                Costo = 5.99, //USDT
                Reward = 150 //Diamanti_Viola
            };
            public static Shop Pacchetto_2 = new Shop
            {
                Costo = 14.99,
                Reward = 475
            };
            public static Shop Pacchetto_3 = new Shop
            {
                Costo = 24.99,
                Reward = 800
            };
            public static Shop Pacchetto_4 = new Shop
            {
                Costo = 49.99,
                Reward = 1500
            };


        }
        public class Terreni_Virtuali
        {
            public double Produzione { get; set; }
            public int Rarita { get; set; }
            public static Terreni_Virtuali Comune = new Terreni_Virtuali
            {
                Produzione = 0.00000000111,
                Rarita = 50
            };
            public static Terreni_Virtuali NonComune = new Terreni_Virtuali
            {
                Produzione = 0.00000000222,
                Rarita = 20
            };
            public static Terreni_Virtuali Raro = new Terreni_Virtuali
            {
                Produzione = 0.00000000333,
                Rarita = 15
            };
            public static Terreni_Virtuali Epico = new Terreni_Virtuali
            {
                Produzione = 0.00000000444,
                Rarita = 10
            };

            public static Terreni_Virtuali Leggendario = new Terreni_Virtuali
            {
                Produzione = 0.00000000555,
                Rarita = 5
            };
        }
    }
}
