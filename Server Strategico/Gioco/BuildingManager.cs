using Newtonsoft.Json;
using Server_Strategico.Server;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Giocatori.Player;
using static Server_Strategico.Gioco.QuestManager;
using static Server_Strategico.Server.ServerConnection;

namespace Server_Strategico.Gioco
{
    public class BuildingManager
    {
        public static void Costruzione(string buildingType, int count, Guid clientGuid, Player player)
        {
            var manager = new BuildingManager();
            manager.QueueConstruction(buildingType, count, clientGuid, player);
        }

        public static void Costruzione_1(string buildingType, int count, Player player)
        {
            var manager = new BuildingManager();
            manager.QueueConstruction(buildingType, count, player.guid_Player, player);
        }

        public void QueueConstruction(string buildingType, int count, Guid clientGuid, Player player)
        {
            var buildingCost = GetBuildingCost(buildingType);
            if (buildingCost == null)
            {
                Server.Server.Send(clientGuid, $"Log_Server|[error]Tipo edificio[/error] [title]{buildingType}[/title][icon:{buildingType}] [error]non valido![/error]");
                return;
            }

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
                player.Risorse_Utilizzate += (int)risorse;
                OnEvent(player, QuestEventType.Risorse, "Cibo", buildingCost.Cibo * count);
                OnEvent(player, QuestEventType.Risorse, "Legno", buildingCost.Legno * count);
                OnEvent(player, QuestEventType.Risorse, "Pietra", buildingCost.Pietra * count);
                OnEvent(player, QuestEventType.Risorse, "Ferro", buildingCost.Ferro * count);
                OnEvent(player, QuestEventType.Risorse, "Oro", buildingCost.Oro * count);
                OnEvent(player, QuestEventType.Risorse, "Popolazione", buildingCost.Popolazione * count);

                Server.Server.Send(clientGuid,
                $"Log_Server|[info]Risorse utilizzate per la costruzione di {count} [/info] [title]{buildingType}[/title]:\r\n " +
                $"[cibo][icon:cibo]-{(buildingCost.Cibo * count):N0}[/cibo]  " +
                $"[legno][icon:legno]-{(buildingCost.Legno * count):N0}[/legno]  " +
                $"[pietra][icon:pietra]-{(buildingCost.Pietra * count):N0}[/pietra]   " +
                $"[ferro][icon:ferro]-{(buildingCost.Ferro * count):N0}[/ferro] " +
                $"[oro][icon:oro]-{(buildingCost.Oro * count):N0}[/oro]  " +
                $"[popolazione][icon:popolazione]-{(buildingCost.Popolazione * count):N0}[/oro]");

                int tempoCostruzioneInSecondi = Math.Max(1, Convert.ToInt32(buildingCost.TempoCostruzione - player.Ricerca_Costruzione));
                for (int i = 0; i < count; i++)
                    player.building_Queue.Enqueue(new ConstructionTask(buildingType, tempoCostruzioneInSecondi));

                StartNextConstructions(player, clientGuid);
            }
            else
            {
                Server.Server.Send(clientGuid, $"Log_Server|[error]Risorse insufficienti per costruire[/error] [title]{count} {buildingType}[/title].");
            }
        }
        private static void StartNextConstructions(Player player, Guid clientGuid)
        {
            int maxSlots = player.Code_Costruzione;
            if (player.currentTasks_Building.Count > maxSlots) // 1) Se ci sono più costruzioni attive del consentito -> metti in pausa le eccedenze (ultime avviate)
            {
                // Ordina per l'ordine in lista (assumendo che l'ultima aggiunta sia l'ultima avviata)
                // Metti in pausa le eccedenze partendo dalle ultime
                var extras = player.currentTasks_Building
                    .Skip(maxSlots)
                    .ToList(); // prende quelle oltre maxSlots

                foreach (var t in extras)
                {
                    t.Pause();
                    player.currentTasks_Building.Remove(t);
                    player.pausedTasks_Building.Enqueue(t);

                    Console.WriteLine($"Costruzione di {t.Type} messa in pausa (slot ridotto)");
                    Server.Server.Send(clientGuid, $"Log_Server|[warning]Costruzione di[/warning] [title]{t.Type}[/title] [warning]messa in pausa per riduzione slot[/warning].");
                }
                return; // usciamo: prima delle pause non facciamo altro
            }

            while (player.currentTasks_Building.Count < maxSlots) // 2) Se ci sono slot liberi, prima riprendi le costruzioni in pausa (FIFO)
            {
                if (player.pausedTasks_Building != null && player.pausedTasks_Building.Count > 0)
                {
                    var resumed = player.pausedTasks_Building.Dequeue();
                    resumed.Resume();
                    player.currentTasks_Building.Add(resumed);

                    Console.WriteLine($"Costruzione di {resumed.Type} ripresa.");
                    Server.Server.Send(clientGuid, $"Log_Server|[info]Ripresa la costruzione di[/info] [title]{resumed.Type}[/title][icon:{resumed.Type}]");
                    continue;
                }
                if (player.building_Queue.Count > 0)  // 3) Se non ci sono sospese, avvia dalla coda normale
                {
                    var nextTask = player.building_Queue.Dequeue();
                    nextTask.Start();
                    player.currentTasks_Building.Add(nextTask);

                    Console.WriteLine($"Costruzione di {nextTask.Type} iniziata, durata {nextTask.DurationInSeconds}s");
                    Server.Server.Send(clientGuid, $"Log_Server|[title]Costruzione di[/title] {nextTask.Type} [title]iniziata, durata [/title]{nextTask.DurationInSeconds}[icon:tempo]");
                }
                else break;
            }
        }
        public static void CompleteBuilds(Guid clientGuid, Player player)
        {
            for (int i = player.currentTasks_Building.Count - 1; i >= 0; i--)
            {
                //Probabile qui bisogna controllare il numero di costruttori disponibili, suppongo errori se si passa da 2 a 1 costruttore e ci sono strutture in entrambe le code...

                var task = player.currentTasks_Building[i];
                if (task.IsComplete())
                {
                    switch (task.Type)
                    {
                        case "Fattoria": 
                            player.Fattoria++;
                            player.Strutture_Civili_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "Fattoria", 1);
                            break;
                        case "Segheria": 
                            player.Segheria++;
                            player.Strutture_Civili_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "Segheria", 1);
                            break;
                        case "CavaPietra": 
                            player.CavaPietra++;
                            player.Strutture_Civili_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "CavaPietra", 1);
                            break;
                        case "MinieraFerro": 
                            player.MinieraFerro++;
                            player.Strutture_Civili_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "MinieraFerro", 1);
                            break;
                        case "MinieraOro": 
                            player.MinieraOro++;
                            player.Strutture_Civili_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "MinieraOro", 1);
                            break;
                        case "Case": 
                            player.Abitazioni++;
                            player.Strutture_Civili_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "Case", 1);
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
                            OnEvent(player, QuestEventType.Costruzione, "CasermaGuerrieri", 1);
                            break;
                        case "CasermaLanceri":
                            player.Caserma_Lancieri++;
                            player.Strutture_Militari_Costruite++;
                            player.Caserme_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "CasermaLancieri", 1);
                            break;
                        case "CasermaArceri":
                            player.Caserma_Arceri++;
                            player.Strutture_Militari_Costruite++;
                            player.Caserme_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "CasermaArcieri", 1);
                            break;
                        case "CasermaCatapulte":
                            player.Caserma_Catapulte++;
                            player.Strutture_Militari_Costruite++;
                            player.Caserme_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "CasermaCatapulte", 1);
                            break;

                        default: Console.WriteLine($"Costruzione {task.Type} non valida!"); break;
                    }

                    Console.WriteLine($"Costruzione completata: {task.Type}");
                    Server.Server.Send(clientGuid, $"Log_Server|[success]Costruzione completata:[/success] [title]{task.Type}[/title][icon:{task.Type}]");
                    player.currentTasks_Building.RemoveAt(i);
                }
            }
            StartNextConstructions(player, clientGuid);
        }
        public static void UsaDiamantiPerVelocizzare(Guid clientGuid, Player player, int diamantiBluDaUsare)
        {
            if (diamantiBluDaUsare <= 0)
            {
                Server.Server.Send(clientGuid, "Log_Server|[title]Numero diamanti[icon:diamantiBlu] non valido.[/title]");
                return;
            }

            if (player.Diamanti_Blu < diamantiBluDaUsare)
            {
                Server.Server.Send(clientGuid, "Log_Server|[error]Non hai abbastanza[/error] [blu]Diamanti Blu![/blu][icon:diamanteBlu]");
                return;
            }

            double tempoTotale = 0; // Calcola il tempo totale ancora necessario

            foreach (var task in player.currentTasks_Building) //Task in corso
                tempoTotale += task.GetRemainingTime();
            foreach (var task in player.building_Queue) //Task in coda
                tempoTotale += task.GetRemainingTime();
            if (player.pausedTasks_Building != null)
                foreach (var task in player.pausedTasks_Building) //Task in pausa
                    tempoTotale += task.GetRemainingTime();

            if (tempoTotale <= 0)
            {
                Server.Server.Send(clientGuid, "Log_Server|[title]Non ci sono costruzioni da velocizzare.[/title]");
                return;
            }
            int riduzioneTotale = diamantiBluDaUsare * Variabili_Server.Velocizzazione_Tempo;             // Ogni diamante riduce X secondi
            int maxDiamantiUtili = (int)Math.Ceiling(tempoTotale / Variabili_Server.Velocizzazione_Tempo);

            if (diamantiBluDaUsare > maxDiamantiUtili)
            {
                diamantiBluDaUsare = maxDiamantiUtili;
                riduzioneTotale = diamantiBluDaUsare * Variabili_Server.Velocizzazione_Tempo;
            }

            foreach (var task in player.currentTasks_Building) // Poi alle costruzioni in corso
            {
                double rimanente = task.GetRemainingTime();
                if (rimanente <= 0) continue;

                if (riduzioneTotale >= rimanente)
                {
                    task.ForzaCompletamento();
                    player.Tempo_Sottratto_Diamanti += (int)rimanente;
                    player.Tempo_Costruzione += (int)rimanente;
                    riduzioneTotale -= (int)rimanente;
                }
                else
                {
                    if (rimanente - riduzioneTotale < 1)
                        riduzioneTotale -= 1;
                    task.RiduciTempo(riduzioneTotale);
                    player.Tempo_Sottratto_Diamanti += (int)riduzioneTotale;
                    player.Tempo_Costruzione += (int)riduzioneTotale;
                    riduzioneTotale = 0;
                }
                if (riduzioneTotale <= 0)
                    break;
            }

            foreach (var task in player.building_Queue) // Applica prima alla coda
            {
                double rimanente = task.GetRemainingTime();
                if (rimanente <= 0) continue;

                if (riduzioneTotale >= rimanente)
                {
                    task.ForzaCompletamento();
                    player.Tempo_Sottratto_Diamanti += (int)rimanente;
                    player.Tempo_Costruzione += (int)rimanente;
                    riduzioneTotale -= (int)rimanente;
                }
                else
                {
                    if (rimanente - riduzioneTotale < 1)
                        riduzioneTotale -= 1;
                    task.RiduciTempo(riduzioneTotale);
                    player.Tempo_Sottratto_Diamanti += (int)riduzioneTotale;
                    player.Tempo_Costruzione += (int)riduzioneTotale;
                    riduzioneTotale = 0;
                }
                if (riduzioneTotale <= 0)
                    break;
            }

            if (player.pausedTasks_Building != null) // Infine anche le costruzioni in pausa (riduce il RemainingSeconds memorizzato)
            {
                var pausedArray = player.pausedTasks_Building.ToArray();
                for (int i = 0; i < pausedArray.Length; i++)
                {
                    var task = pausedArray[i];
                    double rimanente = task.GetRemainingTime();
                    if (rimanente <= 0) continue;

                    if (riduzioneTotale >= rimanente)
                    {
                        task.ForzaCompletamento();
                        player.Tempo_Sottratto_Diamanti += (int)rimanente;
                        player.Tempo_Costruzione += (int)rimanente;
                        riduzioneTotale -= (int)rimanente;
                    }
                    else
                    {
                        if (rimanente - riduzioneTotale < 1)
                            riduzioneTotale -= 1;
                        task.RiduciTempo(riduzioneTotale);
                        player.Tempo_Sottratto_Diamanti += (int)riduzioneTotale;
                        player.Tempo_Costruzione += (int)riduzioneTotale;
                        riduzioneTotale = 0;
                    }
                    if (riduzioneTotale <= 0)
                        break;
                }
            }

            player.Diamanti_Blu -= diamantiBluDaUsare;
            player.Diamanti_Blu_Utilizzati += diamantiBluDaUsare;
            OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", diamantiBluDaUsare);
            OnEvent(player, QuestEventType.Velocizzazione, "Costruzione", (int)riduzioneTotale);
            OnEvent(player, QuestEventType.Velocizzazione, "Qualsiasi", (int)riduzioneTotale);

            // Dopo la modifica dei tempi, prova a completare (completa quelle forzate)
            CompleteBuilds(clientGuid, player);
            Server.Server.Send(clientGuid, $"Log_Server|[warning]Hai usato[/warning] [blu]{diamantiBluDaUsare} Diamanti Blu [icon:diamanteBlu][/blu] [warning]per velocizzare le costruzioni!");
        }
        public static Dictionary<string, int> GetQueuedBuildings(Player player) 
        { 
            var queuedByType = new Dictionary<string, int>();
            foreach (var task in player.building_Queue) 
            { 
                if (!queuedByType.ContainsKey(task.Type)) queuedByType[task.Type] = 0; queuedByType[task.Type]++; 
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
            foreach (var task in player.currentTasks_Building)
                total += task.GetRemainingTime();

            foreach (var task in player.pausedTasks_Building)
                total += task.GetRemainingTime();

            foreach (var task in player.building_Queue)
                total += task.DurationInSeconds;

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
                Server.Server.Send(clientGuid, $"Log_Server|[viola]{Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola} Diamanti Viola[/viola][icon:diamanteViola] [title]utilizzati per un terreno virtuale...[/title]");
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
                    break;
                case "Terreno Noncomune": 
                    player.Terreno_NonComune++; 
                    break;
                case "Terreno Raro": 
                    player.Terreno_Raro++; 
                    break;
                case "Terreno Epico": 
                    player.Terreno_Epico++; 
                    break;
                case "Terreno Leggendario": 
                    player.Terreno_Leggendario++; 
                    break;
            }
            Console.WriteLine($"Terreno generato: {terrenoOttenuto}");
            Server.Server.Send(clientGuid, $"Log_Server|[title]Terreno ottenuto:[/title] [{terrenoOttenuto.Replace(" ", "")}]{terrenoOttenuto}[/{terrenoOttenuto.Replace(" ", "")}][icon:{terrenoOttenuto.Replace(" ", "")}]");
        }

        public class ConstructionTask
        {
            public string Type { get; }
            public int DurationInSeconds { get; private set; } // durata totale (aggiornabile)
            private DateTime startTime;
            private bool forceComplete = false;

            // gestione pausa
            public bool IsPaused { get; private set; } = false;
            private double pausedRemainingSeconds = 0;

            public ConstructionTask(string type, int durationInSeconds)
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
                if (IsPaused) return pausedRemainingSeconds;
                if (startTime == default) return DurationInSeconds;
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
                    if (pausedRemainingSeconds <= 0) ForzaCompletamento();
                    return;
                }

                double rem = GetRemainingTime();  // se in corso
                if (rem <= 0) return;

                // riduci il tempo rimanente aggiornando DurationInSeconds rispetto al tempo già trascorso
                double elapsed = (startTime == default) ? 0 : (DateTime.Now - startTime).TotalSeconds;
                double newRem = Math.Max(0, rem - secondi);

                if (newRem <= 0) ForzaCompletamento();
                else DurationInSeconds = (int)Math.Ceiling(elapsed + newRem); // impostiamo DurationInSeconds in modo che Duration - elapsed = newRem
            }
        }
    }
}
