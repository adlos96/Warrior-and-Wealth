using static Server_Strategico.Gioco.Giocatori;
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

                Server.Server.Send(clientGuid, $"Log_Server|Risorse utilizzate per la ricerca {researchType} livello {livello}...");
                Console.WriteLine($"Risorse utilizzate per {researchType} livello {livello}: " +
                                  $"Cibo={researchCost.Cibo}, Legno={researchCost.Legno}, Pietra={researchCost.Pietra}, " +
                                  $"Ferro={researchCost.Ferro}, Oro={researchCost.Oro}");

                int tempoRicercaInSecondi = Math.Max(1, Convert.ToInt32(researchCost.TempoRicerca));

                // Inserisci nella coda
                if (player.Code_Ricerca > 1 || player.research_Queue.Count == 0 && player.currentTasks_Research.Count == 0)
                    player.research_Queue.Enqueue(new BuildingManager.ConstructionTask(researchType, tempoRicercaInSecondi));
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
        public class ResearchCost
        {
            public double Cibo { get; set; }
            public double Legno { get; set; }
            public double Pietra { get; set; }
            public double Ferro { get; set; }
            public double Oro { get; set; }
            public double TempoRicerca { get; set; } = 60; // default 60 secondi
        }
    }
}
