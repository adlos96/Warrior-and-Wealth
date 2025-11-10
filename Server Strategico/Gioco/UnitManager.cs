using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Giocatori.Player;
using static Server_Strategico.Gioco.QuestManager;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server_Strategico.Gioco
{
    public class UnitManager
    {
        public static void Reclutamento(string unitType, string livello, int count, Guid clientGuid, Player player)
        {
            var manager = new UnitManager();
            manager.QueueTrainUnits(unitType, livello, count, clientGuid, player);
        }

        public static void Reclutamento_1(string unitType, string livello, int count, Player player) //Caricamento dati da fil db
        {
            var manager = new UnitManager();
            manager.QueueTrainUnits(unitType, livello, count, player.guid_Player, player);
        }

        public void QueueTrainUnits(string unitType, string livello, int count, Guid clientGuid, Player player)
        {
            var unitCost = GetUnitCost(unitType, livello);
            if (unitCost == null) return;

            int ridurre_Addestramento = unitType switch
            {
                "Guerrieri" => 1,
                "Lanceri" => 1,
                "Arceri" => 2,
                "Catapulte" => 3,
                _ => 0
            };

            if (unitType == "Guerrieri")
            {
                int unitàGuerrieri = player.Guerrieri.Sum();
                unitàGuerrieri += player.recruit_Queue.Count(t => t.Type.StartsWith("Guerrieri"));
                unitàGuerrieri += player.currentTasks_Recruit.Count(t => t.Type.StartsWith("Guerrieri"));
                unitàGuerrieri += player.pausedTasks_Recruit.Count(t => t.Type.StartsWith("Guerrieri"));

                unitàGuerrieri += player.Guerrieri_Ingresso.Sum();
                unitàGuerrieri += player.Guerrieri_Citta.Sum();
                unitàGuerrieri += player.Guerrieri_Cancello.Sum();
                unitàGuerrieri += player.Guerrieri_Mura.Sum();
                unitàGuerrieri += player.Guerrieri_Torri.Sum();
                unitàGuerrieri += player.Guerrieri_Castello.Sum();

                if (unitàGuerrieri + count > player.Caserma_Guerrieri * Strutture.Edifici.CasermaGuerrieri.Limite)
                {
                    Server.Server.Send(clientGuid, $"Log_Server|Limite truppe raggiunto per i Guerrieri. [{unitàGuerrieri}/{player.Caserma_Guerrieri * Strutture.Edifici.CasermaGuerrieri.Limite}]");
                    return;
                }
            }
            if (unitType == "Lanceri")
            {
                int unitàLanceri = player.Lanceri.Sum();
                unitàLanceri += player.recruit_Queue.Count(t => t.Type.StartsWith("Lanceri"));
                unitàLanceri += player.currentTasks_Recruit.Count(t => t.Type.StartsWith("Lanceri"));
                unitàLanceri += player.pausedTasks_Recruit.Count(t => t.Type.StartsWith("Lanceri"));

                unitàLanceri += player.Lanceri_Ingresso.Sum();
                unitàLanceri += player.Lanceri_Citta.Sum();
                unitàLanceri += player.Lanceri_Cancello.Sum();
                unitàLanceri += player.Lanceri_Mura.Sum();
                unitàLanceri += player.Lanceri_Torri.Sum();
                unitàLanceri += player.Lanceri_Castello.Sum();

                if (unitàLanceri + count > player.Caserma_Lancieri * Strutture.Edifici.CasermaLanceri.Limite)
                {
                    Server.Server.Send(clientGuid, $"Log_Server|Limite truppe raggiunto per i Lanceri. [{unitàLanceri}/{player.Caserma_Lancieri * Strutture.Edifici.CasermaLanceri.Limite}]");
                    return;
                }
            }
            if (unitType == "Arceri")
            {
                int unitàArceri = player.Arceri.Sum();
                unitàArceri += player.recruit_Queue.Count(t => t.Type.StartsWith("Arceri"));
                unitàArceri += player.currentTasks_Recruit.Count(t => t.Type.StartsWith("Arceri"));
                unitàArceri += player.pausedTasks_Recruit.Count(t => t.Type.StartsWith("Arceri"));

                unitàArceri += player.Arceri_Ingresso.Sum();
                unitàArceri += player.Arceri_Citta.Sum();
                unitàArceri += player.Arceri_Cancello.Sum();
                unitàArceri += player.Arceri_Mura.Sum();
                unitàArceri += player.Arceri_Torri.Sum();
                unitàArceri += player.Arceri_Castello.Sum();

                if (unitàArceri + count > player.Caserma_Arceri * Strutture.Edifici.CasermaArceri.Limite)
                {
                    Server.Server.Send(clientGuid, $"Log_Server|Limite truppe raggiunto per i Arceri. [{unitàArceri}/{player.Caserma_Arceri * Strutture.Edifici.CasermaArceri.Limite}]");
                    return;
                }
            }
            if (unitType == "Catapulte")
            {
                int unitàCatapulta = player.Catapulte.Sum();
                unitàCatapulta += player.recruit_Queue.Count(t => t.Type.StartsWith("Catapulte"));
                unitàCatapulta += player.currentTasks_Recruit.Count(t => t.Type.StartsWith("Catapulte"));
                unitàCatapulta += player.pausedTasks_Recruit.Count(t => t.Type.StartsWith("Catapulte"));

                unitàCatapulta += player.Catapulte_Ingresso.Sum();
                unitàCatapulta += player.Catapulte_Citta.Sum();
                unitàCatapulta += player.Catapulte_Cancello.Sum();
                unitàCatapulta += player.Catapulte_Mura.Sum();
                unitàCatapulta += player.Catapulte_Torri.Sum();
                unitàCatapulta += player.Catapulte_Castello.Sum();

                if (unitàCatapulta + count > player.Caserma_Catapulte * Strutture.Edifici.CasermaCatapulte.Limite)
                {
                    Server.Server.Send(clientGuid, $"Log_Server|Limite truppe raggiunto per i Catapulte. [{unitàCatapulta}/{player.Caserma_Catapulte * Strutture.Edifici.CasermaCatapulte.Limite}]");
                    return;
                }
            }

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

            int risorse = Convert.ToInt32((unitCost.Cibo + unitCost.Legno + unitCost.Pietra + unitCost.Ferro + unitCost.Oro) * count);
            int risorse_M = Convert.ToInt32((unitCost.Spade + unitCost.Lance + unitCost.Archi + unitCost.Scudi + unitCost.Armature) * count);
            player.Risorse_Utilizzate += risorse;
            player.Risorse_Utilizzate += risorse_M;

            OnEvent(player, QuestEventType.Risorse, "Cibo", Convert.ToInt32(unitCost.Cibo * count));
            OnEvent(player, QuestEventType.Risorse, "Legno", Convert.ToInt32(unitCost.Legno * count));
            OnEvent(player, QuestEventType.Risorse, "Pietra", Convert.ToInt32(unitCost.Pietra * count));
            OnEvent(player, QuestEventType.Risorse, "Ferro", Convert.ToInt32(unitCost.Ferro * count));
            OnEvent(player, QuestEventType.Risorse, "Oro", Convert.ToInt32(unitCost.Oro * count));
            OnEvent(player, QuestEventType.Risorse, "Popolazione", Convert.ToInt32(unitCost.Popolazione * count));

            OnEvent(player, QuestEventType.Risorse, "Spade", Convert.ToInt32(unitCost.Popolazione * count));
            OnEvent(player, QuestEventType.Risorse, "Lance", Convert.ToInt32(unitCost.Popolazione * count));
            OnEvent(player, QuestEventType.Risorse, "Archi", Convert.ToInt32(unitCost.Popolazione * count));
            OnEvent(player, QuestEventType.Risorse, "Scudi", Convert.ToInt32(unitCost.Popolazione * count));
            OnEvent(player, QuestEventType.Risorse, "Armature", Convert.ToInt32(unitCost.Popolazione * count));

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

            int tempoAddestramento = Math.Max(1, Convert.ToInt32(unitCost.TempoReclutamento - player.Ricerca_Addestramento * ridurre_Addestramento));

            if (player.recruit_Queue == null) // Inizializza coda se nulla
                player.recruit_Queue = new Queue<RecruitTask>();

            for (int i = 0; i < count; i++) // Inserisci ogni unità come singolo task
                player.recruit_Queue.Enqueue(new RecruitTask(unitType + livello, tempoAddestramento));

            StartNextRecruitments(player, clientGuid);

            Server.Server.Send(clientGuid, $"Log_Server|Risorse utilizzate per l'ddestramento di {count} {unitType + livello}\r\n" +
                $"Cibo= {unitCost.Cibo}, Legno= {unitCost.Legno}, Pietra= {unitCost.Pietra}, Ferro= {unitCost.Ferro},\r\n Oro= {unitCost.Oro}, Popolazione= {unitCost.Popolazione}\r\n" +
                $"Spade= {unitCost.Cibo}, Lance= {unitCost.Legno}, Archi= {unitCost.Pietra}, Scudi= {unitCost.Ferro}, Armature= {unitCost.Oro}");
        }
        private static void StartNextRecruitments(Player player, Guid clientGuid)
        {
            int maxSlots = player.Code_Reclutamento;
            bool ciSonoAddestramentiInCorso = player.currentTasks_Recruit.Any(t => !t.IsComplete());

            if (ciSonoAddestramentiInCorso) // 🔒 Se esistono costruzioni ancora in corso, NON iniziare nuove
                return;

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
                    player.Unità_Addestrate++;
                    switch (task.Type)
                    {
                        case "Guerrieri_1": 
                            player.Guerrieri[0]++;
                            OnEvent(player, QuestEventType.Addestramento, "Guerrieri_1", 1);
                            break;
                        case "Lanceri_1": 
                            player.Lanceri[0]++;
                            OnEvent(player, QuestEventType.Addestramento, "Lanceri_1", 1);
                            break;
                        case "Arceri_1": 
                            player.Arceri[0]++;
                            OnEvent(player, QuestEventType.Addestramento, "Arceri_1", 1);
                            break;
                        case "Catapulta_1": 
                            player.Catapulte[0]++;
                            OnEvent(player, QuestEventType.Addestramento, "Catapulta_1", 1);
                            break;

                        case "Guerrieri_2":
                            player.Guerrieri[1]++;
                            OnEvent(player, QuestEventType.Addestramento, "Guerrieri_2", 1);
                            break;
                        case "Lanceri_2":
                            player.Lanceri[1]++;
                            OnEvent(player, QuestEventType.Addestramento, "Lanceri_2", 1);
                            break;
                        case "Arceri_2":
                            player.Arceri[1]++;
                            OnEvent(player, QuestEventType.Addestramento, "Arceri_2", 1);
                            break;
                        case "Catapulta_2":
                            player.Catapulte[1]++;
                            OnEvent(player, QuestEventType.Addestramento, "Catapulta_2", 1);
                            break;

                        case "Guerrieri_3":
                            player.Guerrieri[2]++;
                            OnEvent(player, QuestEventType.Addestramento, "Guerrieri_3", 1);
                            break;
                        case "Lanceri_3":
                            player.Lanceri[2]++;
                            OnEvent(player, QuestEventType.Addestramento, "Lanceri_3", 1);
                            break;
                        case "Arceri_3":
                            player.Arceri[2]++;
                            OnEvent(player, QuestEventType.Addestramento, "Arceri_3", 1);
                            break;
                        case "Catapulta_3":
                            player.Catapulte[2]++;
                            OnEvent(player, QuestEventType.Addestramento, "Catapulta_3", 1);
                            break;

                        case "Guerrieri_4":
                            player.Guerrieri[3]++;
                            OnEvent(player, QuestEventType.Addestramento, "Guerrieri_4", 1);
                            break;
                        case "Lanceri_4":
                            player.Lanceri[3]++;
                            OnEvent(player, QuestEventType.Addestramento, "Lanceri_4", 1);
                            break;
                        case "Arceri_4":
                            player.Arceri[3]++;
                            OnEvent(player, QuestEventType.Addestramento, "Arceri_4", 1);
                            break;
                        case "Catapulta_4":
                            player.Catapulte[3]++;
                            OnEvent(player, QuestEventType.Addestramento, "Catapulta_4", 1);
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
        private Esercito.CostoReclutamento GetUnitCost(string unitType, string level)
        {
            string unit = unitType + level;
            return unit switch
            {
                "Guerrieri_1" => Esercito.CostoReclutamento.Guerriero_1,
                "Lanceri_1" => Esercito.CostoReclutamento.Lancere_1,
                "Arceri_1" => Esercito.CostoReclutamento.Arcere_1,
                "Catapulte_1" => Esercito.CostoReclutamento.Catapulta_1,

                "Guerrieri_2" => Esercito.CostoReclutamento.Guerriero_2,
                "Lanceri_2" => Esercito.CostoReclutamento.Lancere_2,
                "Arceri_2" => Esercito.CostoReclutamento.Arcere_2,
                "Catapulte_2" => Esercito.CostoReclutamento.Catapulta_2,

                "Guerrieri_3" => Esercito.CostoReclutamento.Guerriero_3,
                "Lanceri_3" => Esercito.CostoReclutamento.Lancere_3,
                "Arceri_3" => Esercito.CostoReclutamento.Arcere_3,
                "Catapulte_3" => Esercito.CostoReclutamento.Catapulta_3,

                "Guerrieri_4" => Esercito.CostoReclutamento.Guerriero_4,
                "Lanceri_4" => Esercito.CostoReclutamento.Lancere_4,
                "Arceri_4" => Esercito.CostoReclutamento.Arcere_4,
                "Catapulte_4" => Esercito.CostoReclutamento.Catapulta_4,

                "Guerrieri_5" => Esercito.CostoReclutamento.Guerriero_5,
                "Lanceri_5" => Esercito.CostoReclutamento.Lancere_5,
                "Arceri_5" => Esercito.CostoReclutamento.Arcere_5,
                "Catapulte_5" => Esercito.CostoReclutamento.Catapulta_5,
                _ => null,
            };
        }
        public static void UsaDiamantiPerVelocizzareReclutamento(Guid clientGuid, Player player, int diamantiBluDaUsare)
        {
            if (diamantiBluDaUsare <= 0)
            {
                Server.Server.Send(clientGuid, "Log_Server|Numero diamanti non valido.");
                return;
            }

            if (player.Diamanti_Blu < diamantiBluDaUsare)
            {
                Server.Server.Send(clientGuid, "Log_Server|Non hai abbastanza Diamanti Blu!");
                return;
            }

            double tempoTotale = 0; // Calcola il tempo totale ancora necessario
            foreach (var task in player.currentTasks_Recruit) //Task in corso
                tempoTotale += task.GetRemainingTime();
            foreach (var task in player.recruit_Queue) //Task in coda
                tempoTotale += task.GetRemainingTime();

            if (tempoTotale <= 0)
            {
                Server.Server.Send(clientGuid, "Log_Server|Non ci sono costruzioni da velocizzare.");
                return;
            }
            int riduzioneTotale = diamantiBluDaUsare * Variabili_Server.Velocizzazione_Tempo;             // Ogni diamante riduce 30 secondi
            int maxDiamantiUtili = (int)Math.Ceiling(tempoTotale / Variabili_Server.Velocizzazione_Tempo); // Se si usano più diamanti del necessario, limita alla quantità utile

            if (diamantiBluDaUsare > maxDiamantiUtili)
            {
                diamantiBluDaUsare = maxDiamantiUtili;
                riduzioneTotale = diamantiBluDaUsare * Variabili_Server.Velocizzazione_Tempo;
            }
            player.Diamanti_Blu -= diamantiBluDaUsare;
            player.Diamanti_Blu_Utilizzati += diamantiBluDaUsare;

            foreach (var task in player.recruit_Queue) //Task in coda
            {
                double rimanente = task.GetRemainingTime();
                if (rimanente <= 0) continue;

                if (riduzioneTotale >= rimanente)
                {
                    task.ForzaCompletamento();
                    riduzioneTotale -= (int)rimanente;
                }
                else
                {
                    if (rimanente - riduzioneTotale < 1)
                        riduzioneTotale -= 1;
                    task.RiduciTempo(riduzioneTotale);
                    riduzioneTotale = 0;
                }
                if (riduzioneTotale <= 0)
                    break;
            }

            foreach (var task in player.currentTasks_Recruit) //Task in corso
            {
                double rimanente = task.GetRemainingTime();
                if (rimanente <= 0) continue;

                if (riduzioneTotale >= rimanente)
                {
                    task.ForzaCompletamento();
                    riduzioneTotale -= (int)rimanente;
                }
                else
                {
                    if (rimanente - riduzioneTotale < 1)
                        riduzioneTotale -= 1;
                    task.RiduciTempo(riduzioneTotale);
                    riduzioneTotale = 0;
                }
                if (riduzioneTotale <= 0)
                    break;
            }
            CompleteRecruitment(clientGuid, player);
            Server.Server.Send(clientGuid, $"Log_Server|Hai usato {diamantiBluDaUsare} 💎 Diamanti Blu per velocizzare l'addestramento!");
        }

        public class RecruitTask // Classe privata per rappresentare un task di reclutamento
        {
            public string Type { get; }
            public int DurationInSeconds { get; private set; } // durata totale (aggiornabile)
            private DateTime startTime;
            private bool forceComplete = false;

            // gestione pausa
            public bool IsPaused { get; private set; } = false;
            private double pausedRemainingSeconds = 0;

            public RecruitTask(string type, int durationInSeconds)
            {
                Type = type;
                DurationInSeconds = durationInSeconds;
            }

            public void Start()
            {
                startTime = DateTime.Now;
            }
            public void RestoreProgress(double remainingSeconds)
            {
                if (remainingSeconds <= 0)
                {
                    ForzaCompletamento();
                    return;
                }
                double elapsed = Math.Max(0, DurationInSeconds - remainingSeconds); // calcola elapsed = durata_totale - remaining
                startTime = DateTime.UtcNow.AddSeconds(-elapsed); // startTime = ora - elapsed

                // reset stati pausa / forza
                IsPaused = false;
                pausedRemainingSeconds = 0;
                forceComplete = false;
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
            public void ForzaCompletamento()
            {
                startTime = DateTime.Now.AddSeconds(-DurationInSeconds);
            }

            public void RiduciTempo(int secondi)
            {
                startTime = startTime.AddSeconds(-secondi);
            }
        }
    }
}
