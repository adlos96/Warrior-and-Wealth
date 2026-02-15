using Server_Strategico.Gioco;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Manager.QuestManager;
using static Server_Strategico.Manager.BuildingManagerV2;

namespace Server_Strategico.Manager
{
    public class UnitManagerV2
    {
        public static void Reclutamento(string unitType, string livello, int count, Guid clientGuid, Player player)
        {
            var manager = new UnitManagerV2();
            manager.QueueTrainUnits(unitType, livello, count, clientGuid, player);
        }

        public void QueueTrainUnits(string unitType, string livello, int count, Guid clientGuid, Player player)
        {
            var unitCost = GetUnitCost(unitType, livello);
            if (unitCost == null) return;
            if (player.Livello < Variabili_Server.truppe_II && livello.Replace("_","") != "1") return;
            else if (player.Livello < Variabili_Server.truppe_III && Convert.ToInt32(livello.Replace("_", "")) > 2) return;
            else if (player.Livello < Variabili_Server.truppe_IV && Convert.ToInt32(livello.Replace("_", "")) > 3) return;
            else if (player.Livello < Variabili_Server.truppe_V && Convert.ToInt32(livello.Replace("_", "")) > 4) return;

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
                unitàGuerrieri += player.task_Attuale_Recutamento.Count(t => t.Type.StartsWith("Guerrieri"));
                unitàGuerrieri += player.task_Coda_Recutamento.Count(t => t.Type.StartsWith("Guerrieri"));

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
                unitàLanceri += player.task_Attuale_Recutamento.Count(t => t.Type.StartsWith("Lanceri"));
                unitàLanceri += player.task_Coda_Recutamento.Count(t => t.Type.StartsWith("Lanceri"));

                unitàLanceri += player.Lanceri_Ingresso.Sum();
                unitàLanceri += player.Lanceri_Citta.Sum();
                unitàLanceri += player.Lanceri_Cancello.Sum();
                unitàLanceri += player.Lanceri_Mura.Sum();
                unitàLanceri += player.Lanceri_Torri.Sum();
                unitàLanceri += player.Lanceri_Castello.Sum();

                if (unitàLanceri + count > player.Caserma_Lancieri * Strutture.Edifici.CasermaLanceri.Limite)
                {
                    Server.Server.Send(clientGuid, $"Log_Server|[tile]Limite truppe raggiunto per i Lancieri.[icon:lancere] [warning]{unitàLanceri}/{player.Caserma_Lancieri * Strutture.Edifici.CasermaLanceri.Limite}");
                    return;
                }
            }
            if (unitType == "Arceri")
            {
                int unitàArceri = player.Arceri.Sum();
                unitàArceri += player.task_Attuale_Recutamento.Count(t => t.Type.StartsWith("Arceri"));
                unitàArceri += player.task_Coda_Recutamento.Count(t => t.Type.StartsWith("Arceri"));

                unitàArceri += player.Arceri_Ingresso.Sum();
                unitàArceri += player.Arceri_Citta.Sum();
                unitàArceri += player.Arceri_Cancello.Sum();
                unitàArceri += player.Arceri_Mura.Sum();
                unitàArceri += player.Arceri_Torri.Sum();
                unitàArceri += player.Arceri_Castello.Sum();

                if (unitàArceri + count > player.Caserma_Arceri * Strutture.Edifici.CasermaArceri.Limite)
                {
                    Server.Server.Send(clientGuid, $"Log_Server|[tile]Limite truppe raggiunto per gli Arcieri.[icon:arciere] [warning]{unitàArceri}/{player.Caserma_Arceri * Strutture.Edifici.CasermaArceri.Limite}");
                    return;
                }
            }
            if (unitType == "Catapulte")
            {
                int unitàCatapulta = player.Catapulte.Sum();
                unitàCatapulta += player.task_Attuale_Recutamento.Count(t => t.Type.StartsWith("Catapulte"));
                unitàCatapulta += player.task_Coda_Recutamento.Count(t => t.Type.StartsWith("Catapulte"));

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

            lock (player.LockReclutamento)
            {
                if (player.task_Coda_Recutamento == null) // Inizializza coda se nulla
                    player.task_Coda_Recutamento = new Queue<UnitTaskV2>();

                for (int i = 0; i < count; i++) // Inserisci ogni unità come singolo task
                    player.task_Coda_Recutamento.Enqueue(new UnitTaskV2(unitType + livello, tempoAddestramento));
            }

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
            lock (player.LockReclutamento)
            {
                int maxSlots = player.Code_Reclutamento;
                if (player.task_Attuale_Recutamento.Count > maxSlots) // 1) Se ci sono più costruzioni attive del consentito -> metti in pausa le eccedenze (ultime avviate)
                {
                    // Ordina per l'ordine in lista (assumendo che l'ultima aggiunta sia l'ultima avviata)
                    // Metti in pausa le eccedenze partendo dalle ultime
                    var extras = player.task_Attuale_Recutamento
                        .Skip(maxSlots)
                        .ToList(); // prende quelle oltre maxSlots

                    foreach (var t in extras)
                    {
                        t.Pause();
                        player.task_Attuale_Recutamento.Remove(t);
                        player.task_Coda_Recutamento.Enqueue(t);

                        Console.WriteLine($"Costruzione di {t.Type} messa in pausa (slot ridotto)");
                        Server.Server.Send(clientGuid, $"Log_Server|[tile]Reclutamento di [warning]{t.Type.Replace("_", " Lv ")} [tile]messa in pausa per riduzione slot.");
                    }
                    return; // usciamo: prima delle pause non facciamo altro
                }

                while (player.task_Attuale_Recutamento.Count < maxSlots) // 2) Se ci sono slot liberi, prima riprendi le costruzioni in pausa (FIFO)
                {
                    if (player.task_Coda_Recutamento.Count > 0)  // 3) Se non ci sono sospese, avvia dalla coda normale
                    {
                        var nextTask = player.task_Coda_Recutamento.Dequeue();
                        if (nextTask != null) nextTask.Start();
                        player.task_Attuale_Recutamento.Add(nextTask);

                        Console.WriteLine($"Reclutamento di {nextTask.Type} iniziata, durata {player.FormatTime(nextTask.TempoInSecondi)}s");
                        Server.Server.Send(clientGuid, $"Log_Server|[title]Reclutamento di[/title] {nextTask.Type.Replace("_", " Lv ")} [title]iniziata. Durata[/title] [icon:tempo]{player.FormatTime(nextTask.TempoInSecondi)}");
                    }
                    else break;
                }
            }
        }
        public static void CompleteRecruitment(Guid clientGuid, Player player)
        {
            lock (player.LockReclutamento)
            {
                if (player.task_Attuale_Recutamento == null || player.task_Attuale_Recutamento.Count == 0)
                    return;

                var completedTasks = new List<UnitTaskV2>(); // Lista temporanea, per evitare modifiche durante l'iterazione... per i task completati, processarli dopo.
                var task_Attuale = new List<UnitTaskV2>();
                var coda_Attuale = new List<UnitTaskV2>();

                foreach (var task in player.task_Attuale_Recutamento) // Edifici correnti ed in pausa
                    if (task.IsComplete()) completedTasks.Add(task);
                    else task_Attuale.Add(task);

                foreach (var task in player.task_Coda_Recutamento) // edifici in coda
                    if (task.IsComplete()) completedTasks.Add(task);
                    else coda_Attuale.Add(task);

                if (completedTasks.Count != 0) // Se vuoto non fare nulla
                {
                    player.task_Attuale_Recutamento.Clear();
                    player.task_Coda_Recutamento.Clear();

                    player.task_Attuale_Recutamento = task_Attuale;
                    foreach (var task in coda_Attuale) // edifici in coda
                        player.task_Coda_Recutamento.Enqueue(task);
                }

                foreach (var task in completedTasks) // processa e rimuovi i task completati
                {
                    player.Unità_Addestrate++;
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
                }
            }
            StartNextRecruitments(player, clientGuid);
        }
        public static string Get_Total_Recruit_Time(Player player)
        {
            double total = 0;
            lock (player.LockReclutamento)
            {
                foreach (var task in player.task_Attuale_Recutamento) total += task.TempoInSecondi;
                foreach (var task in player.task_Coda_Recutamento) total += task.TempoInSecondi;
            }
            return player.FormatTime(total);
        }
        public static Dictionary<string, int> GetQueuedUnits(Player player)
        {
            var queuedUnits = new Dictionary<string, int>();
            lock (player.LockReclutamento)
            {
                if (player.task_Attuale_Recutamento.Count() > 0)
                    foreach (var task in player.task_Attuale_Recutamento)
                    {
                        if (!queuedUnits.ContainsKey(task.Type))
                            queuedUnits[task.Type] = 0;
                        queuedUnits[task.Type]++;
                    }
                if (player.task_Coda_Recutamento.Count() > 0)
                    foreach (var task in player.task_Coda_Recutamento)
                    {
                        if (!queuedUnits.ContainsKey(task.Type))
                            queuedUnits[task.Type] = 0;
                        queuedUnits[task.Type]++;
                    }
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
            int tempoEffettivamenteRidotto = 0;
            lock (player.LockReclutamento)
            {
                if (player.task_Attuale_Recutamento != null)
                    foreach (var task in player.task_Attuale_Recutamento) tempoTotale += Math.Max(0, task.GetRemainingTime());
                if (player.task_Coda_Recutamento != null)
                    foreach (var task in player.task_Coda_Recutamento) tempoTotale += Math.Max(0, task.GetRemainingTime());

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
                if (player.task_Attuale_Recutamento != null && player.task_Attuale_Recutamento.Count > 0)
                {
                    var tasksList = player.task_Attuale_Recutamento.ToList();
                    foreach (var task in tasksList)
                    {
                        if (riduzioneTotale <= 0) break;

                        double rimanente = task.TempoInSecondi;
                        if (rimanente <= 0) continue;

                        if (riduzioneTotale >= rimanente)
                        {
                            int riduzione = (int)Math.Ceiling(rimanente);
                            player.Tempo_Sottratto_Diamanti += riduzione;
                            player.Tempo_Addestramento += riduzione;
                            task.TempoInSecondi = 0;
                            riduzioneTotale -= riduzione;
                        }
                        else
                        {
                            player.Tempo_Sottratto_Diamanti += riduzioneTotale;
                            player.Tempo_Addestramento += riduzioneTotale;
                            task.TempoInSecondi -= riduzioneTotale;
                            riduzioneTotale = 0;
                        }
                    }
                }
                // 2) Processa coda
                if (riduzioneTotale > 0 && player.task_Coda_Recutamento != null && player.task_Coda_Recutamento.Count > 0)
                {
                    var queueList = player.task_Coda_Recutamento.ToList();

                    foreach (var task in queueList)
                    {
                        if (riduzioneTotale <= 0) break;

                        double rimanente = task.TempoInSecondi;
                        if (rimanente <= 0) continue;

                        if (riduzioneTotale >= rimanente)
                        {
                            int riduzione = (int)Math.Ceiling(rimanente);
                            player.Tempo_Sottratto_Diamanti += riduzione;
                            player.Tempo_Addestramento += riduzione;
                            task.TempoInSecondi = 0;
                            riduzioneTotale -= riduzione;
                        }
                        else
                        {
                            player.Tempo_Sottratto_Diamanti += riduzioneTotale;
                            player.Tempo_Addestramento += riduzioneTotale;
                            task.TempoInSecondi -= riduzioneTotale;
                            riduzioneTotale = 0;
                        }
                    }
                }
                tempoEffettivamenteRidotto = riduzioneTotaleOriginale - riduzioneTotale;
            }

            player.Diamanti_Blu -= diamantiBluDaUsare;
            player.Diamanti_Blu_Utilizzati += diamantiBluDaUsare;

            OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", diamantiBluDaUsare);
            OnEvent(player, QuestEventType.Velocizzazione, "Addestramento", tempoEffettivamenteRidotto);
            OnEvent(player, QuestEventType.Velocizzazione, "Qualsiasi", tempoEffettivamenteRidotto);

            CompleteRecruitment(clientGuid, player);
            Server.Server.Send(clientGuid, $"Log_Server|[title]Hai usato [warning][icon:diamanteBlu]{diamantiBluDaUsare} [blu]Diamanti Blu [title]per velocizzare l'addestramento! [icon:tempo]{player.FormatTime(tempoEffettivamenteRidotto)}");
        }

        public class UnitTaskV2 // Classe per rappresentare un task di costruzione
        {
            public string Type { get; }
            public double TempoInSecondi { get; set; } // Tempo totale e rimanente (viene aggiornato quando si riduce il tempo)
            public bool IsPaused { get; set; } = false;

            public UnitTaskV2(string type, double tempo)
            {
                Type = type;
                TempoInSecondi = tempo;
            }

            public void Start() // avvia (o riavvia) il conto alla rovescia da DurationInSeconds
            {
                IsPaused = false;
            }
            public void RestoreProgress(double remainingSeconds)
            {
                TempoInSecondi = remainingSeconds;
            }
            public void Pause()
            {
                if (IsPaused) return;
                IsPaused = true;
                TempoInSecondi = GetRemainingTime();
            }
            public void Resume()
            {
                if (!IsPaused) return;
                IsPaused = false;
            }
            public bool IsComplete()
            {
                if (TempoInSecondi <= 0) return true;
                else return false;
            }
            public double GetRemainingTime()
            {
                return TempoInSecondi;
            }
        }
    }
}
