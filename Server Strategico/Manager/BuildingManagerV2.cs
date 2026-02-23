using Server_Strategico.Gioco;
using Server_Strategico.Server;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Giocatori.Player;
using static Server_Strategico.Manager.QuestManager;
using static Server_Strategico.Server.ServerConnection;

namespace Server_Strategico.Manager
{
    public class BuildingManagerV2
    {
        public static void Costruzione(string buildingType, int count, Guid clientGuid, Player player)
        {
            var manager = new BuildingManagerV2();
            manager.QueueConstruction(buildingType, count, clientGuid, player);
        }

        static int LimiteStrutture(Player player)
        {
            int limite = 0;

            limite += player.Terreno_Comune * Variabili_Server.Terreni_Virtuali.Comune.Limite_Strutture +
                player.Terreno_NonComune * Variabili_Server.Terreni_Virtuali.NonComune.Limite_Strutture +
                player.Terreno_Raro * Variabili_Server.Terreni_Virtuali.Raro.Limite_Strutture +
                player.Terreno_Epico * Variabili_Server.Terreni_Virtuali.Epico.Limite_Strutture +
                player.Terreno_Leggendario * Variabili_Server.Terreni_Virtuali.Leggendario.Limite_Strutture;

            limite -= player.Fattoria + player.Segheria + player.CavaPietra + player.MinieraFerro + player.MinieraOro + player.Abitazioni +
                player.Workshop_Spade + player.Workshop_Lance + player.Workshop_Archi + player.Workshop_Scudi + player.Workshop_Armature + player.Workshop_Frecce +
                player.Caserma_Guerrieri + player.Caserma_Lancieri + player.Caserma_Arceri + player.Caserma_Catapulte;
            return limite;
        }

        public void QueueConstruction(string buildingType, int count, Guid clientGuid, Player player)
        {
            lock (player.LockCostruzione)
            {
                var buildingCost = GetBuildingCost(buildingType);
                if (buildingCost == null)
                {
                    Server.Server.Send(clientGuid, $"Log_Server|[error]Tipo edificio [title]{buildingType}[icon:{buildingType}] [error]non valido!");
                    return;
                }
                //!!!!! limite strutture !!!!! - I terreni ampliano il limite delle strutture costruibili
                int slot_Dispionibili = LimiteStrutture(player);
                //if (slot_Dispionibili < count)
                //{
                //    Server.Server.Send(clientGuid, $"Log_Server|[error]Limite strutture raggiunto, spazio disponibile: {slot_Dispionibili}, Richiesto: {count}, per [title]{buildingType}[icon:{buildingType}][error].\n" +
                //        $"Acquista altri terreni virtuali per aumentare il limite di costruzione.");
                //    return;
                //}

                // Controllo risorse
                if (player.Cibo >= buildingCost.Cibo * count &&
                    player.Legno >= buildingCost.Legno * count &&
                    player.Pietra >= buildingCost.Pietra * count &&
                    player.Ferro >= buildingCost.Ferro * count &&
                    player.Oro >= buildingCost.Oro * count &&
                    player.Popolazione >= buildingCost.Popolazione * count)
                {
                    // Scala risorse
                    player.Cibo -= buildingCost.Cibo * count;
                    player.Legno -= buildingCost.Legno * count;
                    player.Pietra -= buildingCost.Pietra * count;
                    player.Ferro -= buildingCost.Ferro * count;
                    player.Oro -= buildingCost.Oro * count;
                    player.Popolazione -= buildingCost.Popolazione * count;

                    int risorse = (buildingCost.Cibo + buildingCost.Legno + buildingCost.Pietra + buildingCost.Ferro + buildingCost.Oro) * count;
                    player.Risorse_Utilizzate += risorse;
                    OnEvent(player, QuestEventType.Risorse, "Cibo", buildingCost.Cibo * count);
                    OnEvent(player, QuestEventType.Risorse, "Legno", buildingCost.Legno * count);
                    OnEvent(player, QuestEventType.Risorse, "Pietra", buildingCost.Pietra * count);
                    OnEvent(player, QuestEventType.Risorse, "Ferro", buildingCost.Ferro * count);
                    OnEvent(player, QuestEventType.Risorse, "Oro", buildingCost.Oro * count);
                    OnEvent(player, QuestEventType.Risorse, "Popolazione", buildingCost.Popolazione * count);

                    Server.Server.Send(clientGuid,
                    $"Log_Server|[info]Risorse utilizzate[/info] per la costruzione di [warning]{count} {buildingType}[/warning]:\r\n " +
                    $"[cibo][icon:cibo]-{buildingCost.Cibo * count:N0}[/cibo]  " +
                    $"[legno][icon:legno]-{buildingCost.Legno * count:N0}[/legno]  " +
                    $"[pietra][icon:pietra]-{buildingCost.Pietra * count:N0}[/pietra]   " +
                    $"[ferro][icon:ferro]-{buildingCost.Ferro * count:N0}[/ferro] " +
                    $"[oro][icon:oro]-{buildingCost.Oro * count:N0}[/oro]  " +
                    $"[popolazione][icon:popolazione]-{buildingCost.Popolazione * count:N0}[/popolazione]");

                    int tempoCostruzioneInSecondi = Math.Max(1, Convert.ToInt32(buildingCost.TempoCostruzione - player.Ricerca_Costruzione - buildingCost.TempoCostruzione * player.Bonus_Costruzione));
                    for (int i = 0; i < count; i++)
                        player.task_Coda_Costruzioni.Enqueue(new ConstructionTaskV2(buildingType, tempoCostruzioneInSecondi));

                    StartNextConstructions(player, clientGuid);
                }
                else
                {
                    Server.Server.Send(clientGuid, $"Log_Server|[error]Risorse insufficienti per costruire [title]{count} {buildingType}.");
                }
            }
        }
        private static void StartNextConstructions(Player player, Guid clientGuid)
        {
            int maxSlots = player.Code_Costruzione;
            if (player.task_Attuale_Costruzioni.Count > maxSlots) // 1) Se ci sono più costruzioni attive del consentito -> metti in pausa le eccedenze (ultime avviate)
            {
                // Ordina per l'ordine in lista (assumendo che l'ultima aggiunta sia l'ultima avviata)
                // Metti in pausa le eccedenze partendo dalle ultime
                var extras = player.task_Attuale_Costruzioni
                    .Skip(maxSlots)
                    .ToList(); // prende quelle oltre maxSlots

                foreach (var t in extras)
                {
                    t.Pause();
                    player.task_Attuale_Costruzioni.Remove(t);
                    player.task_Coda_Costruzioni.Enqueue(t);

                    Console.WriteLine($"Costruzione di {t.Type} messa in pausa (slot ridotto)");
                    Server.Server.Send(clientGuid, $"Log_Server|[warning]Costruzione di [title]{t.Type} [warning]messa in pausa per riduzione slot.");
                }
            }

            while (player.task_Attuale_Costruzioni.Count < maxSlots) // 2) Se ci sono slot liberi, prima riprendi le costruzioni in pausa (FIFO)
            {
                if (player.task_Coda_Costruzioni.Count > 0)  // 3) Se non ci sono sospese, avvia dalla coda normale
                {
                    var nextTask = player.task_Coda_Costruzioni.Dequeue();
                    if (nextTask != null)
                        nextTask.Start();
                    player.task_Attuale_Costruzioni.Add(nextTask);

                    Console.WriteLine($"Costruzione di {nextTask.Type} iniziata, durata {nextTask.TempoInSecondi}s");
                    Server.Server.Send(clientGuid, $"Log_Server|[title]Costruzione di [info]{nextTask.Type} [title]iniziata, durata: [icon:tempo]{player.FormatTime(nextTask.TempoInSecondi)}");
                }
                else break;
            }
        }
        public static void CompleteBuilds(Guid clientGuid, Player player)
        {
            lock (player.LockCostruzione)
            {
                if (player.task_Attuale_Costruzioni == null || player.task_Attuale_Costruzioni.Count == 0)
                    return;

                var completedTasks = new List<ConstructionTaskV2>(); // Lista temporanea, per evitare modifiche durante l'iterazione... per i task completati, processarli dopo.
                var task_Attuale = new List<ConstructionTaskV2>();
                var coda_Attuale = new List<ConstructionTaskV2>();

                foreach (var task in player.task_Attuale_Costruzioni) // Edifici correnti ed in pausa
                    if (task.IsComplete()) completedTasks.Add(task);
                    else task_Attuale.Add(task);

                foreach (var task in player.task_Coda_Costruzioni) // edifici in coda
                    if (task.IsComplete()) completedTasks.Add(task);
                    else coda_Attuale.Add(task);

                if (completedTasks.Count != 0) // Se vuoto non fare nulla
                {
                    player.task_Attuale_Costruzioni.Clear();
                    player.task_Coda_Costruzioni.Clear();

                    player.task_Attuale_Costruzioni = task_Attuale;
                    foreach (var task in coda_Attuale) // edifici in coda
                        player.task_Coda_Costruzioni.Enqueue(task);
                }

                var msgArgs = "0|1|2|3".Split('|'); // x il tutorial

                foreach (var task in completedTasks) // Processa e rimuovi i task completati
                {
                    switch (task.Type)
                    {
                        case "Fattoria":
                            player.Fattoria++;
                            player.Strutture_Civili_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "Fattoria", 1);
                            if (player.Tutorial)
                            {
                                msgArgs[3] = "14";
                                TutorialUpdate(player, msgArgs);
                            }
                            break;
                        case "Segheria":
                            player.Segheria++;
                            player.Strutture_Civili_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "Segheria", 1);
                            if (player.Tutorial)
                            {
                                msgArgs[3] = "15";
                                TutorialUpdate(player, msgArgs);
                            }
                            break;
                        case "CavaPietra":
                            player.CavaPietra++;
                            player.Strutture_Civili_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "CavaPietra", 1);
                            if (player.Tutorial)
                            {
                                msgArgs[3] = "16";
                                TutorialUpdate(player, msgArgs);
                            }
                            break;
                        case "MinieraFerro":
                            player.MinieraFerro++;
                            player.Strutture_Civili_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "MinieraFerro", 1);
                            if (player.Tutorial)
                            {
                                msgArgs[3] = "17";
                                TutorialUpdate(player, msgArgs);
                            }
                            break;
                        case "MinieraOro":
                            player.MinieraOro++;
                            player.Strutture_Civili_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "MinieraOro", 1);
                            if (player.Tutorial)
                            {
                                msgArgs[3] = "18";
                                TutorialUpdate(player, msgArgs);
                            }
                            break;
                        case "Case":
                            player.Abitazioni++;
                            player.Strutture_Civili_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "Case", 1);
                            if (player.Tutorial)
                            {
                                msgArgs[3] = "19";
                                TutorialUpdate(player, msgArgs);
                            }
                            break;

                        case "ProduzioneSpade":
                            player.Workshop_Spade++;
                            player.Strutture_Militari_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "ProduzioneSpade", 1);
                            break;
                        case "ProduzioneLance":
                            player.Workshop_Lance++;
                            player.Strutture_Militari_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "ProduzioneLance", 1);
                            break;
                        case "ProduzioneArchi":
                            player.Workshop_Archi++;
                            player.Strutture_Militari_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "ProduzioneArchi", 1);
                            break;
                        case "ProduzioneScudi":
                            player.Workshop_Scudi++;
                            player.Strutture_Militari_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "ProduzioneScudi", 1);
                            break;
                        case "ProduzioneArmature":
                            player.Workshop_Armature++;
                            player.Strutture_Militari_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "ProduzioneArmature", 1);
                            break;
                        case "ProduzioneFrecce":
                            player.Workshop_Frecce++;
                            player.Strutture_Militari_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "ProduzioneFrecce", 1);
                            break;

                        case "CasermaGuerrieri":
                            player.Caserma_Guerrieri++;
                            player.Strutture_Militari_Costruite++;
                            player.Caserme_Costruite++;
                            player.SetupCaserme();
                            OnEvent(player, QuestEventType.Costruzione, "CasermaGuerrieri", 1);
                            break;
                        case "CasermaLanceri":
                            player.Caserma_Lancieri++;
                            player.Strutture_Militari_Costruite++;
                            player.Caserme_Costruite++;
                            player.SetupCaserme();
                            OnEvent(player, QuestEventType.Costruzione, "CasermaLancieri", 1);
                            break;
                        case "CasermaArceri":
                            player.Caserma_Arceri++;
                            player.Strutture_Militari_Costruite++;
                            player.Caserme_Costruite++;
                            player.SetupCaserme();
                            OnEvent(player, QuestEventType.Costruzione, "CasermaArcieri", 1);
                            break;
                        case "CasermaCatapulte":
                            player.Caserma_Catapulte++;
                            player.Strutture_Militari_Costruite++;
                            player.Caserme_Costruite++;
                            player.SetupCaserme();
                            OnEvent(player, QuestEventType.Costruzione, "CasermaCatapulte", 1);
                            break;

                        default:
                            Console.WriteLine($"Costruzione {task.Type} non valida!");
                            break;
                    }

                    Console.WriteLine($"Costruzione completata: {task.Type}");
                    Server.Server.Send(clientGuid, $"Log_Server|[success]Costruzione completata: [title]{task.Type}[icon:{task.Type}]");
                }
                StartNextConstructions(player, clientGuid);
            }
        }
        public static void UsaDiamantiPerVelocizzare(Guid clientGuid, Player player, int diamantiBluDaUsare)
        {
            lock (player.LockCostruzione)
            {
                if (diamantiBluDaUsare <= 0)
                {
                    Server.Server.Send(clientGuid, "Log_Server|[error]Numero [blu][icon:diamanteBlu]diamanti [error]non valido.");
                    return;
                }

                if (player.Diamanti_Blu < diamantiBluDaUsare)
                {
                    Server.Server.Send(clientGuid, "Log_Server|[error]Non hai abbastanza [blu]Diamanti Blu![icon:diamanteBlu]");
                    return;
                }

                // Calcola il tempo totale rimanente
                double tempoTotale = 0;

                if (player.task_Attuale_Costruzioni != null)
                    foreach (var task in player.task_Attuale_Costruzioni)
                        tempoTotale += Math.Max(0, task.GetRemainingTime());

                if (player.task_Coda_Costruzioni != null)
                    foreach (var task in player.task_Coda_Costruzioni)
                        tempoTotale += Math.Max(0, task.GetRemainingTime());

                if (tempoTotale <= 0)
                {
                    Server.Server.Send(clientGuid, "Log_Server|[warning]Non ci sono costruzioni da velocizzare.");
                    return;
                }

                // Calcola riduzione e limita ai diamanti utili
                int riduzioneTotale = diamantiBluDaUsare * Variabili_Server.Velocizzazione_Tempo;
                int maxDiamantiUtili = (int)Math.Ceiling(tempoTotale / Variabili_Server.Velocizzazione_Tempo);

                if (diamantiBluDaUsare > maxDiamantiUtili)
                {
                    diamantiBluDaUsare = maxDiamantiUtili;
                    riduzioneTotale = diamantiBluDaUsare * Variabili_Server.Velocizzazione_Tempo;
                }

                int riduzioneTotaleOriginale = riduzioneTotale;

                // 2) Processa currentTasks_Building
                if (player.task_Attuale_Costruzioni != null && player.task_Attuale_Costruzioni.Count > 0)
                {
                    var tasksList = player.task_Attuale_Costruzioni.ToList();

                    foreach (var task in tasksList)
                    {
                        if (riduzioneTotale <= 0) break;

                        double rimanente = task.GetRemainingTime();
                        if (rimanente <= 0) continue;

                        if (riduzioneTotale >= rimanente)
                        {
                            int riduzione = (int)Math.Ceiling(rimanente);
                            player.Tempo_Sottratto_Diamanti += riduzione;
                            player.Tempo_Costruzione += riduzione;
                            riduzioneTotale -= riduzione;
                            task.TempoInSecondi = 0;
                        }
                        else
                        {
                            player.Tempo_Sottratto_Diamanti += riduzioneTotale;
                            player.Tempo_Costruzione += riduzioneTotale;
                            task.TempoInSecondi -= riduzioneTotale;
                            riduzioneTotale = 0;
                        }
                    }
                }

                // 3) Processa building_Queue
                if (riduzioneTotale > 0 && player.task_Coda_Costruzioni != null && player.task_Coda_Costruzioni.Count > 0)
                {
                    var queueList = player.task_Coda_Costruzioni.ToList();

                    foreach (var task in queueList)
                    {
                        if (riduzioneTotale <= 0) break;

                        double rimanente = task.GetRemainingTime();
                        if (rimanente <= 0) continue;

                        if (riduzioneTotale >= rimanente)
                        {
                            int riduzione = (int)Math.Ceiling(rimanente);
                            player.Tempo_Sottratto_Diamanti += riduzione;
                            player.Tempo_Costruzione += riduzione;
                            riduzioneTotale -= riduzione;
                            task.TempoInSecondi = 0;
                        }
                        else
                        {
                            player.Tempo_Sottratto_Diamanti += riduzioneTotale;
                            player.Tempo_Costruzione += riduzioneTotale;
                            task.TempoInSecondi -= riduzioneTotale;
                            riduzioneTotale = 0;
                        }
                    }
                }

                // Deduzione diamanti
                player.Diamanti_Blu -= diamantiBluDaUsare;
                player.Diamanti_Blu_Utilizzati += diamantiBluDaUsare;

                //Tutorial
                var msgArgs = "0|1|2|3".Split('|');
                if (player.Tutorial && !player.Tutorial_Stato[13])
                {
                    msgArgs[3] = "14";
                    TutorialUpdate(player, msgArgs);
                }

                // Eventi
                OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", diamantiBluDaUsare);

                int tempoEffettivamenteRidotto = riduzioneTotaleOriginale - riduzioneTotale;
                OnEvent(player, QuestEventType.Velocizzazione, "Costruzione", tempoEffettivamenteRidotto);
                OnEvent(player, QuestEventType.Velocizzazione, "Qualsiasi", tempoEffettivamenteRidotto);

                Server.Server.Send(clientGuid, $"Log_Server|[title]Hai usato [blu][icon:diamanteBlu][warning]{diamantiBluDaUsare} [blu]Diamanti Blu [title]per velocizzare le costruzioni! [icon:tempo]{player.FormatTime(tempoEffettivamenteRidotto)}");
            }
        }
        public static Dictionary<string, int> GetQueuedBuildings(Player player)
        {
            var queuedByType = new Dictionary<string, int>();
            if (player.task_Coda_Costruzioni.Count() > 0)
                foreach (var task in player.task_Coda_Costruzioni)
                {
                    if (!queuedByType.ContainsKey(task.Type)) 
                        queuedByType[task.Type] = 0; 
                    
                    queuedByType[task.Type]++;
                }
            if (player.task_Attuale_Costruzioni.Count() > 0)
                foreach (var task in player.task_Attuale_Costruzioni)
                {
                    if (!queuedByType.ContainsKey(task.Type)) 
                        queuedByType[task.Type] = 0; 
                    
                    queuedByType[task.Type]++;
                }
            return queuedByType;
        }
        //caricamento dati da file giocatore
        public static void SetBuildings(int fattoria, int segheria, int cavaPietra, int mineraFerro, int mineraOro, int abitazioni, int ProdSp, int ProdLan, int ProdArc, int ProdScud, int ProdArmat, int ProdFrecce, int cas_Gu, int cas_Lan, int cas_Arc, int cas_Cat, Player player)
        {
            player.Fattoria = fattoria;
            player.Segheria = segheria;
            player.CavaPietra = cavaPietra;
            player.MinieraFerro = mineraFerro;
            player.MinieraOro = mineraOro;
            player.Abitazioni = abitazioni;
            player.Workshop_Spade = ProdSp;
            player.Workshop_Lance = ProdLan;
            player.Workshop_Archi = ProdArc;
            player.Workshop_Scudi = ProdScud;
            player.Workshop_Armature = ProdArmat;
            player.Workshop_Frecce = ProdFrecce;
            player.Caserma_Guerrieri = cas_Gu;
            player.Caserma_Lancieri = cas_Lan;
            player.Caserma_Arceri = cas_Arc;
            player.Caserma_Catapulte = cas_Cat;
        }
        public static string Get_Total_Building_Time(Player player)
        {
            double total = 0;
            foreach (var task in player.task_Attuale_Costruzioni)
                total += task.GetRemainingTime();

            foreach (var task in player.task_Coda_Costruzioni)
                total += task.GetRemainingTime();

            return player.FormatTime(total);
        }
        public Strutture.Edifici GetBuildingCost(string buildingType)
        {
            return buildingType switch
            {
                "Fattoria" => Strutture.Edifici.Fattoria,
                "Segheria" => Strutture.Edifici.Segheria,
                "CavaPietra" => Strutture.Edifici.CavaPietra,
                "MinieraFerro" => Strutture.Edifici.MinieraFerro,
                "MinieraOro" => Strutture.Edifici.MinieraOro,
                "Case" => Strutture.Edifici.Case,
                "ProduzioneSpade" => Strutture.Edifici.ProduzioneSpade,
                "ProduzioneLance" => Strutture.Edifici.ProduzioneLance,
                "ProduzioneArchi" => Strutture.Edifici.ProduzioneArchi,
                "ProduzioneScudi" => Strutture.Edifici.ProduzioneScudi,
                "ProduzioneArmature" => Strutture.Edifici.ProduzioneArmature,
                "ProduzioneFrecce" => Strutture.Edifici.ProduzioneFrecce,
                "CasermaGuerrieri" => Strutture.Edifici.CasermaGuerrieri,
                "CasermaLanceri" => Strutture.Edifici.CasermaLanceri,
                "CasermaArceri" => Strutture.Edifici.CasermaArceri,
                "CasermaCatapulte" => Strutture.Edifici.CasermaCatapulte,
                _ => null,
            };
        }
        public static void Terreni_Virtuali(Guid clientGuid, Player player)
        {
            if (player.Diamanti_Viola >= Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola)
            {
                player.Diamanti_Viola -= Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola;
                player.Diamanti_Viola_Utilizzati += Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola; //Aggiunge la spesa al totale dei diamanti viola spesi
                OnEvent(player, QuestEventType.Costruzione, "Terreno", 1); //Aggiungi terreno quest
                Console.WriteLine($"Diamanti Viola utilizzati per un terreno virtuale...");
                Server.Server.Send(clientGuid, $"Log_Server|[warning][icon:diamanteViola]{Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola}[viola] Diamanti Viola[/viola] [title]utilizzati per un terreno virtuale...[/title]");
            }
            else
            {
                Console.WriteLine($"Non hai abbastanza Diamanti Viola per un terreno virtuale.");
                Server.Server.Send(clientGuid, $"Log_Server|[title]Non hai abbastanza[/title] [viola]Diamanti Viola[/viola][icon:diamanteViola] [title]per un terreno virtuale.[/title]");
                return;
            }

            Random rng = new Random(); // Random
            Dictionary<string, int> probabilita = new() // Tabella probabilità terreni
            {
                { "Terreno Comune", Variabili_Server.Terreni_Virtuali.Comune.Rarita },
                { "Terreno Noncomune", Variabili_Server.Terreni_Virtuali.NonComune.Rarita },
                { "Terreno Raro", Variabili_Server.Terreni_Virtuali.Raro.Rarita },
                { "Terreno Epico", Variabili_Server.Terreni_Virtuali.Epico.Rarita },
                { "Terreno Leggendario", Variabili_Server.Terreni_Virtuali.Leggendario.Rarita }
            };

            // Estrazione casuale
            int roll = rng.Next(1, 101); // da 1 a 100
            int cumulativo = 0;
            string terrenoOttenuto = "Terreno Comune"; // default

            foreach (var kvp in probabilita)
            {
                cumulativo += kvp.Value;
                if (roll <= cumulativo)
                {
                    terrenoOttenuto = kvp.Key;
                    break;
                }
            }
            switch (terrenoOttenuto) // Aggiorna player terreno
            {
                case "Terreno Comune":
                    player.Terreno_Comune++;
                    player.limite_Strutture += Variabili_Server.Terreni_Virtuali.Comune.Limite_Strutture;
                    break;
                case "Terreno Noncomune":
                    player.Terreno_NonComune++;
                    player.limite_Strutture += Variabili_Server.Terreni_Virtuali.NonComune.Limite_Strutture;
                    break;
                case "Terreno Raro":
                    player.Terreno_Raro++;
                    player.limite_Strutture += Variabili_Server.Terreni_Virtuali.Raro.Limite_Strutture;
                    break;
                case "Terreno Epico":
                    player.Terreno_Epico++;
                    player.limite_Strutture += Variabili_Server.Terreni_Virtuali.Epico.Limite_Strutture;
                    break;
                case "Terreno Leggendario":
                    player.Terreno_Leggendario++;
                    player.limite_Strutture += Variabili_Server.Terreni_Virtuali.Leggendario.Limite_Strutture;
                    break;
            }
            Console.WriteLine($"Terreno generato: {terrenoOttenuto}");
            Server.Server.Send(clientGuid, $"Log_Server|[warning]Terreno ottenuto:[/warning] [{terrenoOttenuto.Replace(" ", "")}]{terrenoOttenuto}[/{terrenoOttenuto.Replace(" ", "")}][icon:{terrenoOttenuto.Replace(" ", "")}]");
        }

        public class ConstructionTaskV2 // Classe per rappresentare un task di costruzione
        {
            public string Type { get; }
            public double TempoInSecondi { get; set; } // Tempo totale e rimanente (viene aggiornato quando si riduce il tempo)
            public bool IsPaused { get; set; } = false;

            public ConstructionTaskV2(string type, double tempo)
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
