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
                "Guerrieri" => Ricerca.Guerrieri_Riduzione,
                "Lanceri" => Ricerca.Lanceri_Riduzione,
                "Arceri" => Ricerca.Arceri_Riduzione,
                "Catapulte" => Ricerca.Catapulte_Riduzione,
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
                    Server.Server.Send(clientGuid, $"Log_Server|[title]Limite truppe raggiunto per i Guerrieri.[icon:guerriero] [warning]{unitàGuerrieri}/{player.Caserma_Guerrieri * Strutture.Edifici.CasermaGuerrieri.Limite}");
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
                    Server.Server.Send(clientGuid, $"Log_Server|[tile]Limite truppe raggiunto per i Lanceri.[icon:lancere] [warning]{unitàLanceri}/{player.Caserma_Lancieri * Strutture.Edifici.CasermaLanceri.Limite}");
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
                    Server.Server.Send(clientGuid, $"Log_Server|[tile]Limite truppe raggiunto per gli Arceri.[icon:arciere] [warning]{unitàArceri}/{player.Caserma_Arceri * Strutture.Edifici.CasermaArceri.Limite}");
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
                    Server.Server.Send(clientGuid, $"Log_Server|[tile]Limite truppe raggiunto per le Catapulte.[icon:catapulta] [warning]{unitàCatapulta}/{player.Caserma_Catapulte * Strutture.Edifici.CasermaCatapulte.Limite}");
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

            OnEvent(player, QuestEventType.Risorse, "Spade", Convert.ToInt32(unitCost.Spade * count));
            OnEvent(player, QuestEventType.Risorse, "Lance", Convert.ToInt32(unitCost.Lance * count));
            OnEvent(player, QuestEventType.Risorse, "Archi", Convert.ToInt32(unitCost.Archi * count));
            OnEvent(player, QuestEventType.Risorse, "Scudi", Convert.ToInt32(unitCost.Scudi * count));
            OnEvent(player, QuestEventType.Risorse, "Armature", Convert.ToInt32(unitCost.Armature * count));

            int tempoAddestramento = Math.Max(1, Convert.ToInt32(unitCost.TempoReclutamento - (player.Ricerca_Addestramento * ridurre_Addestramento) - (unitCost.TempoReclutamento * player.Bonus_Addestramento)));

            if (player.recruit_Queue == null) // Inizializza coda se nulla
                player.recruit_Queue = new Queue<RecruitTask>();

            for (int i = 0; i < count; i++) // Inserisci ogni unità come singolo task
                player.recruit_Queue.Enqueue(new RecruitTask(unitType + livello, tempoAddestramento));

            StartNextRecruitments(player, clientGuid);
            Server.Server.Send(clientGuid,
                $"Log_Server|[info]Risorse utilizzate[/info] per l'ddestramento di [warning]{count} {(unitType + livello).Replace("_", " LV ")}[/warning]:\r\n " +
                $"[cibo][icon:cibo]-{(unitCost.Cibo * count):N0}[/cibo] " +
                $"[legno][icon:legno]]-{(unitCost.Legno * count):N0}[/legno] " +
                $"[pietra][icon:pietra]-{(unitCost.Pietra * count):N0}[/pietra] " +
                $"[ferro][icon:ferro]-{(unitCost.Ferro * count):N0}[/ferro] " +
                $"[oro][icon:oro]-{(unitCost.Oro * count):N0}[/oro] " +
                $"[popolazione][icon:popolazione]-{(unitCost.Popolazione * count):N0}[/oro]");
        }
        private static void StartNextRecruitments(Player player, Guid clientGuid)
        {
            int maxSlots = player.Code_Reclutamento;
            if (player.currentTasks_Recruit.Count > maxSlots) // 1) Se ci sono più costruzioni attive del consentito -> metti in pausa le eccedenze (ultime avviate)
            {
                // Ordina per l'ordine in lista (assumendo che l'ultima aggiunta sia l'ultima avviata)
                // Metti in pausa le eccedenze partendo dalle ultime
                var extras = player.currentTasks_Recruit
                    .Skip(maxSlots)
                    .ToList(); // prende quelle oltre maxSlots

                foreach (var t in extras)
                {
                    t.Pause();
                    player.currentTasks_Recruit.Remove(t);
                    player.pausedTasks_Recruit.Enqueue(t);

                    Console.WriteLine($"Costruzione di {t.Type} messa in pausa (slot ridotto)");
                    Server.Server.Send(clientGuid, $"Log_Server|[tile]Reclutamento di [warning]{t.Type.Replace("_", " Lv ")} [tile]messa in pausa per riduzione slot.");
                }
                return; // usciamo: prima delle pause non facciamo altro
            }

            while (player.currentTasks_Recruit.Count < maxSlots) // 2) Se ci sono slot liberi, prima riprendi le costruzioni in pausa (FIFO)
            {
                if (player.pausedTasks_Recruit != null && player.pausedTasks_Recruit.Count > 0)
                {
                    var resumed = player.pausedTasks_Recruit.Dequeue();
                    resumed.Resume();
                    player.currentTasks_Recruit.Add(resumed);

                    Console.WriteLine($"Reclutamento di {resumed.Type} ripresa.");
                    Server.Server.Send(clientGuid, $"Log_Server|[tile]Reclutamento di [/tile][warning]{resumed.Type.Replace("_", " Lv ")}[/warning] [tile]ripresa.");
                    continue;
                }
                if (player.recruit_Queue.Count > 0)  // 3) Se non ci sono sospese, avvia dalla coda normale
                {
                    var nextTask = player.recruit_Queue.Dequeue();
                    if (nextTask != null)
                        nextTask.Start();
                    player.currentTasks_Recruit.Add(nextTask);

                    Console.WriteLine($"Reclutamento di {nextTask.Type} iniziata, durata {player.FormatTime(nextTask.DurationInSeconds)}s");
                    Server.Server.Send(clientGuid, $"Log_Server|[title]Reclutamento di[/title] {nextTask.Type.Replace("_", " Lv ")} [title]iniziata. Durata[/title] [icon:tempo]{player.FormatTime(nextTask.DurationInSeconds)}");
                }
                else break;
            }
        }
        public static void CompleteRecruitment(Guid clientGuid, Player player)
        {
            if (player.currentTasks_Recruit == null || player.currentTasks_Recruit.Count == 0)
                return;

            // Usa una lista temporanea per raccogliere i task completati
            var completedTasks = new List<RecruitTask>();

            foreach (var task in player.currentTasks_Recruit)
                if (task.IsComplete())
                    completedTasks.Add(task);

            // Ora processa e rimuovi i task completati
            foreach (var task in completedTasks)
            {
                player.Unità_Addestrate++;

                // Assicura che gli array esistano
                if (player.Guerrieri == null || player.Guerrieri.Length < 5)
                    player.Guerrieri = new int[5];
                if (player.Lanceri == null || player.Lanceri.Length < 5)
                    player.Lanceri = new int[5];
                if (player.Arceri == null || player.Arceri.Length < 5)
                    player.Arceri = new int[5];
                if (player.Catapulte == null || player.Catapulte.Length < 5)
                    player.Catapulte = new int[5];

                switch (task.Type)
                {
                    case "Guerrieri_1":
                        player.Guerrieri[0]++;
                        OnEvent(player, QuestEventType.Addestramento, "Guerrieri", 1);
                        break;
                    case "Lanceri_1":
                        player.Lanceri[0]++;
                        OnEvent(player, QuestEventType.Addestramento, "Lanceri", 1);
                        break;
                    case "Arceri_1":
                        player.Arceri[0]++;
                        OnEvent(player, QuestEventType.Addestramento, "Arceri", 1);
                        break;
                    case "Catapulte_1":
                        player.Catapulte[0]++;
                        OnEvent(player, QuestEventType.Addestramento, "Catapulta", 1);
                        break;

                    case "Guerrieri_2":
                        player.Guerrieri[1]++;
                        OnEvent(player, QuestEventType.Addestramento, "Guerrieri", 1);
                        break;
                    case "Lanceri_2":
                        player.Lanceri[1]++;
                        OnEvent(player, QuestEventType.Addestramento, "Lanceri", 1);
                        break;
                    case "Arceri_2":
                        player.Arceri[1]++;
                        OnEvent(player, QuestEventType.Addestramento, "Arceri", 1);
                        break;
                    case "Catapulte_2":
                        player.Catapulte[1]++;
                        OnEvent(player, QuestEventType.Addestramento, "Catapulta", 1);
                        break;

                    case "Guerrieri_3":
                        player.Guerrieri[2]++;
                        OnEvent(player, QuestEventType.Addestramento, "Guerrieri", 1);
                        break;
                    case "Lanceri_3":
                        player.Lanceri[2]++;
                        OnEvent(player, QuestEventType.Addestramento, "Lanceri", 1);
                        break;
                    case "Arceri_3":
                        player.Arceri[2]++;
                        OnEvent(player, QuestEventType.Addestramento, "Arceri", 1);
                        break;
                    case "Catapulte_3":
                        player.Catapulte[2]++;
                        OnEvent(player, QuestEventType.Addestramento, "Catapulta", 1);
                        break;

                    case "Guerrieri_4":
                        player.Guerrieri[3]++;
                        OnEvent(player, QuestEventType.Addestramento, "Guerrieri", 1);
                        break;
                    case "Lanceri_4":
                        player.Lanceri[3]++;
                        OnEvent(player, QuestEventType.Addestramento, "Lanceri", 1);
                        break;
                    case "Arceri_4":
                        player.Arceri[3]++;
                        OnEvent(player, QuestEventType.Addestramento, "Arceri", 1);
                        break;
                    case "Catapulte_4":
                        player.Catapulte[3]++;
                        OnEvent(player, QuestEventType.Addestramento, "Catapulta", 1);
                        break;

                    case "Guerrieri_5":
                        player.Guerrieri[4]++;
                        OnEvent(player, QuestEventType.Addestramento, "Guerrieri", 1);
                        break;
                    case "Lanceri_5":
                        player.Lanceri[4]++;
                        OnEvent(player, QuestEventType.Addestramento, "Lanceri", 1);
                        break;
                    case "Arceri_5":
                        player.Arceri[4]++;
                        OnEvent(player, QuestEventType.Addestramento, "Arceri", 1);
                        break;
                    case "Catapulte_5":
                        player.Catapulte[4]++;
                        OnEvent(player, QuestEventType.Addestramento, "Catapulta", 1);
                        break;
                }

                Server.Server.Send(clientGuid, $"Log_Server|[warning]{task.Type.Replace("_", " Lv ")}[/warning] addestrato!");
                player.currentTasks_Recruit.Remove(task); // Rimuovi il task dalla lista corrente in modo sicuro
            }

            StartNextRecruitments(player, clientGuid);
        }
        public static string Get_Total_Recruit_Time(Player player)
        {
            double total = 0;
            foreach (var task in player.recruit_Queue)
                total += task.DurationInSeconds;

            foreach (var task in player.pausedTasks_Recruit)
                total += task.GetRemainingTime();

            foreach (var task in player.currentTasks_Recruit)
                total += task.GetRemainingTime();

            return player.FormatTime(total);
        }
        public static Dictionary<string, int> GetQueuedUnits(Player player)
        {
            var queuedUnits = new Dictionary<string, int>();
            if (player.recruit_Queue.Count() > 0)
                foreach (var task in player.recruit_Queue)
                {
                    if (!queuedUnits.ContainsKey(task.Type))
                        queuedUnits[task.Type] = 0;
                    queuedUnits[task.Type]++;
                }
            if (player.currentTasks_Recruit.Count() > 0)
                foreach (var task in player.currentTasks_Recruit)
                {
                    if (!queuedUnits.ContainsKey(task.Type))
                        queuedUnits[task.Type] = 0;
                    queuedUnits[task.Type]++;
                }
            if (player.pausedTasks_Recruit.Count() > 0)
                foreach (var task in player.pausedTasks_Recruit)
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
            if (diamantiBluDaUsare <= 0 || player.Diamanti_Blu < diamantiBluDaUsare)
            {
                Server.Server.Send(clientGuid, "Log_Server|[error]Non hai abbastanza [blu]Diamanti Blu[/blu]![icon:diamanteBlu]");
                return;
            }

            double tempoTotale = 0;

            if (player.currentTasks_Recruit != null)
                foreach (var task in player.currentTasks_Recruit)
                    tempoTotale += Math.Max(0, task.GetRemainingTime());

            if (player.pausedTasks_Recruit != null)
                foreach (var task in player.pausedTasks_Recruit)
                    tempoTotale += Math.Max(0, task.GetRemainingTime());

            if (player.recruit_Queue != null)
                foreach (var task in player.recruit_Queue)
                    tempoTotale += Math.Max(0, task.DurationInSeconds);

            if (tempoTotale <= 0)
            {
                Server.Server.Send(clientGuid, "Log_Server|[warning]Non ci sono unità da velocizzare.");
                return;
            }

            int riduzioneTotale = diamantiBluDaUsare * Variabili_Server.Velocizzazione_Tempo;
            int maxDiamantiUtili = (int)Math.Ceiling(tempoTotale / Variabili_Server.Velocizzazione_Tempo);

            if (diamantiBluDaUsare > maxDiamantiUtili)
            {
                diamantiBluDaUsare = maxDiamantiUtili;
                riduzioneTotale = diamantiBluDaUsare * Variabili_Server.Velocizzazione_Tempo;
            }

            int riduzioneTotaleOriginale = riduzioneTotale;

            // 1) Processa currentTasks
            if (player.currentTasks_Recruit != null && player.currentTasks_Recruit.Count > 0)
            {
                var tasksList = player.currentTasks_Recruit.ToList();
                foreach (var task in tasksList)
                {
                    if (riduzioneTotale <= 0) break;

                    double rimanente = task.GetRemainingTime();
                    if (rimanente <= 0) continue;

                    if (riduzioneTotale >= rimanente)
                    {
                        int riduzione = (int)Math.Ceiling(rimanente);
                        task.ForzaCompletamento();
                        player.Tempo_Sottratto_Diamanti += riduzione;
                        player.Tempo_Addestramento += riduzione;
                        riduzioneTotale -= riduzione;
                    }
                    else
                    {
                        task.RiduciTempo(riduzioneTotale);
                        player.Tempo_Sottratto_Diamanti += riduzioneTotale;
                        player.Tempo_Addestramento += riduzioneTotale;
                        riduzioneTotale = 0;
                    }
                }
            }

            // 2) Processa recruit_Queue
            if (riduzioneTotale > 0 && player.recruit_Queue != null && player.recruit_Queue.Count > 0)
            {
                var queueList = player.recruit_Queue.ToList();

                foreach (var task in queueList)
                {
                    if (riduzioneTotale <= 0) break;

                    double rimanente = task.DurationInSeconds;
                    if (rimanente <= 0) continue;

                    if (riduzioneTotale >= rimanente)
                    {
                        int riduzione = (int)Math.Ceiling(rimanente);
                        task.ForzaCompletamento();
                        player.Tempo_Sottratto_Diamanti += riduzione;
                        player.Tempo_Addestramento += riduzione;
                        riduzioneTotale -= riduzione;
                    }
                    else
                    {
                        task.RiduciTempo(riduzioneTotale);
                        player.Tempo_Sottratto_Diamanti += riduzioneTotale;
                        player.Tempo_Addestramento += riduzioneTotale;
                        riduzioneTotale = 0;
                    }
                }

                player.recruit_Queue = new Queue<RecruitTask>(queueList);
            }

            // 3) Processa pausedTasks
            if (riduzioneTotale > 0 && player.pausedTasks_Recruit != null && player.pausedTasks_Recruit.Count > 0)
            {
                var pausedList = player.pausedTasks_Recruit.ToList();

                foreach (var task in pausedList)
                {
                    if (riduzioneTotale <= 0) break;

                    double rimanente = task.GetRemainingTime();
                    if (rimanente <= 0) continue;

                    if (riduzioneTotale >= rimanente)
                    {
                        int riduzione = (int)Math.Ceiling(rimanente);
                        task.ForzaCompletamento();
                        player.Tempo_Sottratto_Diamanti += riduzione;
                        player.Tempo_Addestramento += riduzione;
                        riduzioneTotale -= riduzione;
                    }
                    else
                    {
                        task.RiduciTempo(riduzioneTotale);
                        player.Tempo_Sottratto_Diamanti += riduzioneTotale;
                        player.Tempo_Addestramento += riduzioneTotale;
                        riduzioneTotale = 0;
                    }
                }

                player.pausedTasks_Recruit = new Queue<RecruitTask>(pausedList);
            }

            player.Diamanti_Blu -= diamantiBluDaUsare;
            player.Diamanti_Blu_Utilizzati += diamantiBluDaUsare;

            OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", diamantiBluDaUsare);

            int tempoEffettivamenteRidotto = riduzioneTotaleOriginale - riduzioneTotale;
            OnEvent(player, QuestEventType.Velocizzazione, "Addestramento", tempoEffettivamenteRidotto);
            OnEvent(player, QuestEventType.Velocizzazione, "Qualsiasi", tempoEffettivamenteRidotto);

            CompleteRecruitment(clientGuid, player);
            Server.Server.Send(clientGuid, $"Log_Server|[title]Hai usato [warning][icon:diamanteBlu]{diamantiBluDaUsare} [blu]Diamanti Blu [title]per velocizzare l'addestramento! [icon:tempo]{player.FormatTime(tempoEffettivamenteRidotto)}");
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
                // avvia (o riavvia) il conto alla rovescia da DurationInSeconds
                IsPaused = false;
                if (forceComplete != true)
                    forceComplete = false;
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
            public void Pause()
            {
                if (IsPaused) return;
                pausedRemainingSeconds = GetRemainingTime();
                IsPaused = true;
                // "ferma" il timer semplicemente azzerando startTime
                startTime = default;
            }
            public void Resume()
            {
                if (!IsPaused) return;
                IsPaused = false;
                // ricopia la durata rimanente nella DurationInSeconds e riavvia il timer da adesso
                DurationInSeconds = (int)Math.Ceiling(pausedRemainingSeconds);
                startTime = DateTime.Now;
                pausedRemainingSeconds = 0;
            }
            public bool IsComplete()
            {
                if (forceComplete) return true;
                if (IsPaused) return false;
                if (startTime == default) return false;
                return DateTime.Now >= startTime.AddSeconds(DurationInSeconds);
            }
            public double GetRemainingTime()
            {
                if (forceComplete) return 0;
                if (IsPaused) return Math.Max(0, pausedRemainingSeconds);
                if (startTime == default) return Math.Max(0, DurationInSeconds);

                double elapsed = (DateTime.Now - startTime).TotalSeconds;
                double rem = Math.Max(0, DurationInSeconds - elapsed);
                return rem;
            }
            public void ForzaCompletamento()
            {
                forceComplete = true;
                IsPaused = false;
                pausedRemainingSeconds = 0;
            }

            public void RiduciTempo(int secondi)
            {
                if (secondi <= 0) return;

                if (IsPaused)
                {
                    pausedRemainingSeconds = Math.Max(0, pausedRemainingSeconds - secondi);
                    return;
                }

                double rem = GetRemainingTime();
                if (rem <= 0) return; // Già completo

                double newRem = Math.Max(0, rem - secondi);

                if (newRem <= 0.5) // Usa 0.5 invece di 0 per evitare problemi di arrotondamento
                {
                    // Imposta come completato: tempo passato
                    DurationInSeconds = 1; // Minimo 1 secondo di durata
                    startTime = DateTime.Now.AddSeconds(-2); // 2 secondi nel passato = già completato
                }
                else
                {
                    // Calcola quanto tempo è già trascorso
                    double tempoGiaTrascorso = DurationInSeconds - rem;

                    // Evita valori negativi
                    tempoGiaTrascorso = Math.Max(0, tempoGiaTrascorso);

                    // Calcola la nuova durata totale
                    double nuovoDurationTotal = tempoGiaTrascorso + newRem;

                    if (nuovoDurationTotal < 1)
                    {
                        // Se troppo piccolo, marca come completato
                        DurationInSeconds = 1;
                        startTime = DateTime.Now.AddSeconds(-2);
                    }
                    else
                    {
                        // Aggiorna startTime per riflettere il tempo risparmiato
                        startTime = DateTime.Now.AddSeconds(-tempoGiaTrascorso);
                        DurationInSeconds = (int)Math.Ceiling(nuovoDurationTotal);

                        // Sanity check: assicura che DurationInSeconds sia positivo
                        if (DurationInSeconds < 1)
                            DurationInSeconds = 1;
                    }
                }
            }
        }
    }
}
