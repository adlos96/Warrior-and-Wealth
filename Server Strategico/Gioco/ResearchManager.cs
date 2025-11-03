using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.QuestManager;
using static Server_Strategico.Gioco.Ricerca;

namespace Server_Strategico.Gioco
{
    public class ResearchManager
    {
        public static void Ricerca(string research, Guid clientGuid, Player player)
        {
            var manager = new ResearchManager();
            manager.StartResearch(research, clientGuid, player);
        }
        public void StartResearch(string researchType, Guid clientGuid, Player player)
        {
            // Calcola il livello successivo
            int livello = researchType switch
            {
                "Addestramento" => player.Ricerca_Addestramento + 1,
                "Costruzione" => player.Ricerca_Costruzione + 1,
                "Produzione" => player.Ricerca_Produzione + 1,
                "Popolazione" => player.Ricerca_Popolazione + 1,
                _ => 1
            };

            // Controllo livello per Addestramento
            //if (researchType == "Addestramento" && player.Livello < livello * 3)
            //{
            //    Server.Server.Send(clientGuid, $"Log_Server|La ricerca [Addestramento {livello}] richiede livello minimo {livello * 3}");
            //    return;
            //}

            // Ottieni costi dinamici
            var researchCost = GetResearchCost(researchType, livello);
            if (researchCost == null)
            {
                Server.Server.Send(clientGuid, $"Log_Server|Tipo ricerca {researchType} non valido!");
                return;
            }

            // Controllo risorse
            if (player.Cibo >= researchCost.Cibo &&
                player.Legno >= researchCost.Legno &&
                player.Pietra >= researchCost.Pietra &&
                player.Ferro >= researchCost.Ferro &&
                player.Oro >= researchCost.Oro)
            {
                // Scala risorse
                player.Cibo -= researchCost.Cibo;
                player.Legno -= researchCost.Legno;
                player.Pietra -= researchCost.Pietra;
                player.Ferro -= researchCost.Ferro;
                player.Oro -= researchCost.Oro;

                int risorse = Convert.ToInt32(researchCost.Cibo + researchCost.Legno + researchCost.Pietra + researchCost.Ferro + researchCost.Oro);
                player.Risorse_Utilizzate += risorse;
                OnEvent(player, QuestEventType.Risorse, "Cibo", (int)researchCost.Cibo);
                OnEvent(player, QuestEventType.Risorse, "Legno", (int)researchCost.Legno);
                OnEvent(player, QuestEventType.Risorse, "Pietra", (int)researchCost.Pietra);
                OnEvent(player, QuestEventType.Risorse, "Ferro", (int)researchCost.Ferro);
                OnEvent(player, QuestEventType.Risorse, "Oro", (int)researchCost.Oro);

                Server.Server.Send(clientGuid, $"Log_Server|Risorse utilizzate per la ricerca {researchType} livello {livello}...");
                Console.WriteLine($"Risorse utilizzate per {researchType} livello {livello}: " +
                                  $"Cibo={researchCost.Cibo}, Legno={researchCost.Legno}, Pietra={researchCost.Pietra}, " +
                                  $"Ferro={researchCost.Ferro}, Oro={researchCost.Oro}");

                int tempoRicercaInSecondi = Math.Max(1, Convert.ToInt32(researchCost.TempoRicerca));

                // Inserisci nella coda
                if (player.Code_Ricerca > 1 || player.research_Queue.Count == 0 && player.currentTasks_Research.Count == 0)
                    player.research_Queue.Enqueue(new ResearchManager.ResearchTask(researchType, tempoRicercaInSecondi));
                else
                {
                    Server.Server.Send(clientGuid, $"Log_Server|Una singola ricerca è possibile. Ricerca {researchType} annullata.");
                    return;
                }
                StartNextResearch(player, clientGuid);
            }
            else
                Server.Server.Send(clientGuid, $"Log_Server|Risorse insufficienti per la ricerca {researchType} livello {livello}.");
            
        }

        // Avvia i task finché ci sono slot liberi
        private static void StartNextResearch(Player player, Guid clientGuid)
        {
            int maxSlots = player.Code_Ricerca; // Parametrizzabile
            while (player.currentTasks_Research.Count < maxSlots && player.research_Queue.Count > 0)
            {
                player.Ricerca_Attiva = true;// blocca i pulsanti ricerca del client
                var nextTask = player.research_Queue.Dequeue();
                nextTask.Start();
                player.currentTasks_Research.Add(nextTask);

                Console.WriteLine($"Ricerca di {nextTask.Type} iniziata, durata {nextTask.DurationInSeconds}s");
                Server.Server.Send(clientGuid, $"Log_Server|Ricerca di {nextTask.Type} iniziata.");
            }
        }

        // Completa le ricerche terminate
        public static void CompleteResearch(Guid clientGuid, Player player)
        {
            for (int i = player.currentTasks_Research.Count - 1; i >= 0; i--)
            {
                var task = player.currentTasks_Research[i];
                if (task.IsComplete())
                {
                    ApplyResearchEffects(task.Type, player);
                    player.Ricerca_Attiva = false; // sblocca i pulsanti ricerca del client
                    Console.WriteLine($"Ricerca completata: {task.Type}");
                    Server.Server.Send(clientGuid, $"Log_Server|Ricerca completata: {task.Type}");

                    player.currentTasks_Research.RemoveAt(i);
                }
            }

            StartNextResearch(player, clientGuid);
        }

        private static void ApplyResearchEffects(string researchType, Player player) // Funzione dove applichi gli effetti della ricerca
        {
            switch (researchType)
            {
                case "Addestramento":
                    player.Ricerca_Addestramento++;
                    break;
                case "Costruzione":
                    player.Ricerca_Costruzione++;
                    break;
                case "Produzione":
                    player.Ricerca_Produzione++;
                    break;
                case "Popolazione":
                    player.Ricerca_Popolazione++;
                    break;
                default:
                    Console.WriteLine($"Ricerca {researchType} non definita negli effetti!");
                    break;
            }
        }

        public static string GetTotalResearchTime(Player player) // Totale tempo ricerche in coda + in corso
        {
            double total = 0;
            foreach (var task in player.currentTasks_Research)
                total += task.GetRemainingTime();
            foreach (var task in player.research_Queue)
                total += task.DurationInSeconds;
            return TimeSpan.FromSeconds(total).ToString(@"hh\:mm\:ss");
        }
        private ResearchCost GetResearchCost(string researchType, int livello)
        {
            return researchType switch
            {
                "Addestramento" => new ResearchCost
                {
                    Cibo = Tipi.Addestramento.Cibo * livello,
                    Legno = Tipi.Addestramento.Legno * livello,
                    Pietra = Tipi.Addestramento.Pietra * livello,
                    Ferro = Tipi.Addestramento.Ferro * livello,
                    Oro = Tipi.Addestramento.Oro * livello,
                    TempoRicerca = 60 * livello // o calcola dinamicamente
                },
                "Costruzione" => new ResearchCost
                {
                    Cibo = Tipi.Costruzione.Cibo * livello,
                    Legno = Tipi.Costruzione.Legno * livello,
                    Pietra = Tipi.Costruzione.Pietra * livello,
                    Ferro = Tipi.Costruzione.Ferro * livello,
                    Oro = Tipi.Costruzione.Oro * livello,
                    TempoRicerca = 60 * livello
                },
                "Produzione" => new ResearchCost
                {
                    Cibo = Tipi.Produzione.Cibo * livello,
                    Legno = Tipi.Produzione.Legno * livello,
                    Pietra = Tipi.Produzione.Pietra * livello,
                    Ferro = Tipi.Produzione.Ferro * livello,
                    Oro = Tipi.Produzione.Oro * livello,
                    TempoRicerca = 60 * livello
                },
                "Popolazione" => new ResearchCost
                {
                    Cibo = Tipi.Popolazione.Cibo * livello,
                    Legno = Tipi.Popolazione.Legno * livello,
                    Pietra = Tipi.Popolazione.Pietra * livello,
                    Ferro = Tipi.Popolazione.Ferro * livello,
                    Oro = Tipi.Popolazione.Oro * livello,
                    TempoRicerca = 60 * livello // più tempo per livelli più alti
                },
                _ => null
            };
        }

        public static void UsaDiamantiPerVelocizzareRicerca(Guid clientGuid, Player player, int diamantiBluDaUsare)
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

            int riduzioneTotale = diamantiBluDaUsare * Variabili_Server.Velocizzazione_Tempo;             // Ogni diamante riduce 30 secondi
            int riduzioneResidua = riduzioneTotale;
            double tempoTotaleRimanente = player.currentTasks_Recruit.Sum(t => t.GetRemainingTime());
            int maxDiamantiUtili = (int)Math.Ceiling(tempoTotaleRimanente / Variabili_Server.Velocizzazione_Tempo); // Se si usano più diamanti del necessario, limita alla quantità utile

            if (diamantiBluDaUsare > maxDiamantiUtili)
            {
                diamantiBluDaUsare = maxDiamantiUtili;
                riduzioneTotale = diamantiBluDaUsare * Variabili_Server.Velocizzazione_Tempo;
                riduzioneResidua = riduzioneTotale;
            }

            player.Diamanti_Blu -= diamantiBluDaUsare;
            player.Diamanti_Blu_Utilizzati += diamantiBluDaUsare;

            foreach (var task in player.currentTasks_Research)
            {
                double rimanente = task.GetRemainingTime();
                if (rimanente <= 0) continue;

                if (riduzioneResidua >= rimanente)
                {
                    task.ForzaCompletamento();
                    riduzioneResidua -= (int)rimanente;
                }
                else
                {
                    task.RiduciTempo(riduzioneResidua);
                    riduzioneResidua = 0;
                }

                if (riduzioneResidua <= 0)
                    break;
            }

            Server.Server.Send(clientGuid, $"Log_Server|Hai usato {diamantiBluDaUsare} 💎 Diamanti Blu per velocizzare le ricerche!");
        }
        public class ResearchCost
        {
            public double Cibo { get; set; }
            public double Legno { get; set; }
            public double Pietra { get; set; }
            public double Ferro { get; set; }
            public double Oro { get; set; }
            public double TempoRicerca { get; set; } = 60; // default 60 secondi


        }
        public class ResearchTask
        {
            public string Type { get; }
            public int DurationInSeconds { get; private set; }
            private DateTime startTime;
            private bool forceComplete = false;

            public ResearchTask(string type, int durationInSeconds)
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
