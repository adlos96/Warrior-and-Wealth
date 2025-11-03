using Server_Strategico.Server;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Giocatori.Player;
using static Server_Strategico.Gioco.QuestManager;

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
                Server.Server.Send(clientGuid, $"Log_Server|Tipo edificio {buildingType} non valido!");
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
                player.Risorse_Utilizzate += risorse;
                OnEvent(player, QuestEventType.Risorse, "Cibo", buildingCost.Cibo * count);
                OnEvent(player, QuestEventType.Risorse, "Legno", buildingCost.Legno * count);
                OnEvent(player, QuestEventType.Risorse, "Pietra", buildingCost.Pietra * count);
                OnEvent(player, QuestEventType.Risorse, "Ferro", buildingCost.Ferro * count);
                OnEvent(player, QuestEventType.Risorse, "Oro", buildingCost.Oro * count);
                OnEvent(player, QuestEventType.Risorse, "Popolazione", buildingCost.Popolazione * count);

                Server.Server.Send(clientGuid, $"Log_Server|Risorse utilizzate per {count} costruzione/i di {buildingType}...");
                int tempoCostruzioneInSecondi = Math.Max(1, Convert.ToInt32(buildingCost.TempoCostruzione - player.Ricerca_Costruzione));

                for (int i = 0; i < count; i++)
                    player.building_Queue.Enqueue(new ConstructionTask(buildingType, tempoCostruzioneInSecondi));

                StartNextConstructions(player, clientGuid);
            }
            else
            {
                Server.Server.Send(clientGuid, $"Log_Server|Risorse insufficienti per costruire {count} {buildingType}.");
            }
        }

        private static void StartNextConstructions(Player player, Guid clientGuid)
        {
            int maxSlots = player.Code_Costruzione;

            while (player.currentTasks_Building.Count < maxSlots && player.building_Queue.Count > 0)
            {
                var nextTask = player.building_Queue.Dequeue();
                nextTask.Start();
                player.currentTasks_Building.Add(nextTask);

                Console.WriteLine($"Costruzione di {nextTask.Type} iniziata, durata {nextTask.DurationInSeconds}s");
                Server.Server.Send(clientGuid, $"Log_Server|Costruzione di {nextTask.Type} iniziata.");
            }
        }

        public static void CompleteBuilds(Guid clientGuid, Player player)
        {
            for (int i = player.currentTasks_Building.Count - 1; i >= 0; i--)
            {
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
                        case "CasermaLancieri":
                            player.Caserma_Lancieri++;
                            player.Strutture_Militari_Costruite++;
                            player.Caserme_Costruite++;
                            OnEvent(player, QuestEventType.Costruzione, "CasermaLancieri", 1);
                            break;
                        case "CasermaArcieri":
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
                    Server.Server.Send(clientGuid, $"Log_Server|Costruzione completata: {task.Type}");
                    player.currentTasks_Building.RemoveAt(i);
                }
            }
            StartNextConstructions(player, clientGuid);
        }
        public static void UsaDiamantiPerVelocizzare(Guid clientGuid, Player player, int diamantiBluDaUsare)
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

            //Potrebbe essere riutilizzato per tutti i task e code, "Costr. Addestr. Ricerca", Tanto cambia solo il riferimento alla coda
            foreach (var task in player.currentTasks_Building) //Task in corso
                tempoTotale += task.GetRemainingTime();
            foreach (var task in player.building_Queue) //Task in coda
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

            foreach (var task in player.building_Queue) //Task in coda
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
            foreach (var task in player.currentTasks_Building) //Task in corso
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

            CompleteBuilds(clientGuid, player);
            Server.Server.Send(clientGuid, $"Log_Server|Hai usato {diamantiBluDaUsare} 💎 Diamanti Blu per velocizzare le costruzioni!");
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
                "CasermaLancieri" => Strutture.Edifici.CasermaLanceri,
                "CasermaArcieri" => Strutture.Edifici.CasermaArceri,
                "CasermaCatapulte" => Strutture.Edifici.CasermaCatapulte,
                _ => null,
            };
        }
        public static void Terreni_Virtuali(Guid clientGuid, Player player)
        {
            if (player.Diamanti_Viola >= Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola)
            {
                player.Diamanti_Viola -= Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola;
                Console.WriteLine($"Log_Server|Diamanti Viola utilizzati per un terreno virtuale...");
            }
            else
            {
                Console.WriteLine($"Log_Server|Non hai abbastanza Diamanti Viola per un terreno virtuale.");
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

            OnEvent(player, QuestEventType.Costruzione, "Terreno", 1); //Aggiungi terreno quest
            player.Diamanti_Viola += Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola; //Aggiunge la spesa al totale dei diamanti viola spesi
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
            Server.Server.Send(clientGuid, $"Log_Server|Terreno ottenuto: {terrenoOttenuto}");
        }

        public class ConstructionTask
        {
            public string Type { get; }
            public int DurationInSeconds { get; private set; }
            private DateTime startTime;
            private bool forceComplete = false;

            public ConstructionTask(string type, int durationInSeconds)
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
                if (forceComplete) return true;
                return DateTime.Now >= startTime.AddSeconds(DurationInSeconds);
            }

            public double GetRemainingTime()
            {
                if (forceComplete) return 0;
                if (startTime == default) return DurationInSeconds;

                double elapsed = (DateTime.Now - startTime).TotalSeconds;
                return Math.Max(0, DurationInSeconds - elapsed);
            }

            // 👉 Forza il completamento immediato
            public void ForzaCompletamento()
            {
                forceComplete = true;
            }

            // 👉 Riduce il tempo rimanente di "secondi"
            public void RiduciTempo(int secondi)
            {
                double rimanente = GetRemainingTime();

                if (rimanente <= 0)
                    return;

                // Riduce la durata totale per accorciare il tempo rimanente
                DurationInSeconds -= secondi;
                if (DurationInSeconds < (DateTime.Now - startTime).TotalSeconds)
                {
                    // Se è diventata minore del tempo già trascorso, forza completamento
                    ForzaCompletamento();
                }
            }
        }
    }
}
