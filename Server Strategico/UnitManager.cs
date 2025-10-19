using Server_Strategico.Gioco;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Giocatori.Player;
using static Server_Strategico.QuestManager;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server_Strategico
{
    public class UnitManager
    {
        public static void Reclutamento(string buildingType, int count, Guid clientGuid, Player player)
        {
            var manager = new UnitManager();
            manager.QueueTrainUnits(buildingType, count, clientGuid, player);
        }

        public static void Reclutamento_1(string buildingType, int count, Player player) //Caricamento dati da fil db
        {
            var manager = new UnitManager();
            manager.QueueTrainUnits(buildingType, count, player.guid_Player, player);
        }

        public void QueueTrainUnits(string unitType, int count, Guid clientGuid, Player player)
        {
            var unitCost = GetUnitCost(unitType);
            if (unitCost == null) return;

            int ridurre_Addestramento = unitType switch
            {
                "Guerrieri_1" => 1,
                "Lanceri_1" => 1,
                "Arceri_1" => 2,
                "Catapulta" => 3,
                _ => 0
            };

            // Controllo risorse
            if (player.Cibo < unitCost.Cibo * count ||
                player.Legno < unitCost.Legno * count ||
                player.Pietra < unitCost.Pietra * count ||
                player.Ferro < unitCost.Ferro * count ||
                player.Oro < unitCost.Oro * count ||
                player.Popolazione < unitCost.Popolazione * count ||
                player.Spade < unitCost.Spade * count ||
                player.Lance < unitCost.Lance * count ||
                player.Archi < unitCost.Archi * count ||
                player.Scudi < unitCost.Scudi * count ||
                player.Armature < unitCost.Armature * count) return;

            // Deduzione risorse
            player.Cibo -= unitCost.Cibo * count;
            player.Legno -= unitCost.Legno * count;
            player.Pietra -= unitCost.Pietra * count;
            player.Ferro -= unitCost.Ferro * count;
            player.Oro -= unitCost.Oro * count;
            player.Popolazione -= unitCost.Popolazione * count;
            player.Spade -= unitCost.Spade * count;
            player.Lance -= unitCost.Lance * count;
            player.Archi -= unitCost.Archi * count;
            player.Scudi -= unitCost.Scudi * count;
            player.Armature -= unitCost.Armature * count;

            int tempoAddestramento = Math.Max(1, Convert.ToInt32(unitCost.TempoReclutamento - (player.Ricerca_Addestramento * ridurre_Addestramento)));

            // Inizializza coda se nulla
            if (player.recruit_Queue == null)
                player.recruit_Queue = new Queue<BuildingManager.ConstructionTask>();

            // Inserisci ogni unità come singolo task
            for (int i = 0; i < count; i++)
                player.recruit_Queue.Enqueue(new BuildingManager.ConstructionTask(unitType, tempoAddestramento));

            StartNextRecruitments(player, clientGuid);

            Server.Server.Send(clientGuid, $"Log_Server|Addestramento di {count} {unitType} messo in coda.");
        }
        private static void StartNextRecruitments(Player player, Guid clientGuid)
        {
            int maxSlots = player.Code_Reclutamento;

            while (player.currentTasks_Recruit.Count < maxSlots && player.recruit_Queue.Count > 0)
            {
                var nextTask = player.recruit_Queue.Dequeue();
                nextTask.Start();
                player.currentTasks_Recruit.Add(nextTask);

                Console.WriteLine($"Addestramento di {nextTask.Type} iniziato, completamento previsto in {nextTask.DurationInSeconds} secondi.");
                Server.Server.Send(clientGuid, $"Log_Server|Addestramento di {nextTask.Type} iniziato.");
            }
        }
        public static void CompleteRecruitment(Guid clientGuid, Player player)
        {
            for (int i = player.currentTasks_Recruit.Count - 1; i >= 0; i--)
            {
                var task = player.currentTasks_Recruit[i];
                if (task.IsComplete())
                {
                    switch (task.Type)
                    {
                        case "Guerrieri_1": 
                            player.Guerrieri[0]++;
                            QuestManager.OnEvent(player, QuestEventType.Addestramento, "Guerrieri_1", 1);
                            break;
                        case "Lanceri_1": 
                            player.Lanceri[0]++;
                            QuestManager.OnEvent(player, QuestEventType.Addestramento, "Lanceri_1", 1);
                            break;
                        case "Arceri_1": 
                            player.Arceri[0]++;
                            QuestManager.OnEvent(player, QuestEventType.Addestramento, "Arceri_1", 1);
                            break;
                        case "Catapulta": 
                            player.Catapulte[0]++;
                            QuestManager.OnEvent(player, QuestEventType.Addestramento, "Catapulta", 1);
                            break;
                    }

                    Server.Server.Send(clientGuid, $"Log_Server|{task.Type} addestrato!");
                    player.currentTasks_Recruit.RemoveAt(i);
                }
            }

            StartNextRecruitments(player, clientGuid);
        }
        public static string Get_Total_Recruit_Time(Player player)
        {
            double total = 0;

            foreach (var task in player.recruit_Queue)
                total += task.DurationInSeconds;

            foreach (var task in player.currentTasks_Recruit)
                total += task.GetRemainingTime();

            return player.FormatTime(total);
        }
        public static Dictionary<string, int> GetQueuedUnits(Player player)
        {
            var queuedUnits = new Dictionary<string, int>();
            foreach (var task in player.recruit_Queue)
            {
                if (!queuedUnits.ContainsKey(task.Type))
                    queuedUnits[task.Type] = 0;
                queuedUnits[task.Type]++;
            }
            return queuedUnits;
        }

        // Recupera costo unità
        private Esercito.CostoReclutamento GetUnitCost(string unitType)
        {
            return unitType switch
            {
                "Guerrieri_1" => Esercito.CostoReclutamento.Guerrieri_1,
                "Lanceri_1" => Esercito.CostoReclutamento.Lanceri_1,
                "Arceri_1" => Esercito.CostoReclutamento.Arceri_1,
                "Catapulta_1" => Esercito.CostoReclutamento.Catapulte_1,
                _ => null,
            };
        }
        public class RecruitTask // Classe privata per rappresentare un task di reclutamento
        {
            public string Type { get; }
            public int DurationInSeconds { get; }
            private DateTime startTime;

            public RecruitTask(string type, int durationInSeconds)
            {
                Type = type;
                DurationInSeconds = durationInSeconds;
            }

            public void Start()
            {
                startTime = DateTime.Now;
            }
            public bool IsComplete()
            {
                return DateTime.Now >= startTime.AddSeconds(DurationInSeconds);
            }
            public double GetRemainingTime()
            {
                if (startTime == default) return DurationInSeconds; // se non è ancora partito
                double elapsed = (DateTime.Now - startTime).TotalSeconds;
                return Math.Max(0, DurationInSeconds - elapsed);
            }
        }
    }
}
