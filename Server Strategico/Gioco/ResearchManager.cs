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
            int livello = researchType switch
            {
                "Addestramento" => player.Ricerca_Addestramento + 1,
                "Costruzione" => player.Ricerca_Costruzione + 1,
                "Produzione" => player.Ricerca_Produzione + 1,
                "Popolazione" => player.Ricerca_Popolazione + 1,

                "Guerriero Salute" => player.Guerriero_Salute + 1,
                "Guerriero Attacco" => player.Guerriero_Attacco + 1,
                "Guerriero Difesa" => player.Guerriero_Difesa + 1,                                                         
                "Guerriero Livello" => player.Guerriero_Livello + 1,                                                       
                                                                                                                           
                "Lancere Salute" => player.Lancere_Salute + 1,                                                             
                "Lancere Attacco" => player.Lancere_Attacco + 1,                                                           
                "Lancere Difesa" => player.Lancere_Difesa + 1,                                                             
                "Lancere Livello" => player.Lancere_Livello + 1,                                                           
                                                                                                                           
                "Arcere Salute" => player.Arcere_Salute + 1,                                                               
                "Arcere Attacco" => player.Arcere_Attacco + 1,                                                             
                "Arcere Difesa" => player.Arcere_Difesa + 1,                                                               
                "Arcere Livello" => player.Arcere_Livello + 1,                                                             
                                                                                                                           
                "Catapulta Salute" => player.Catapulta_Salute + 1,                                                         
                "Catapulta Attacco" => player.Catapulta_Attacco + 1,                                                       
                "Catapulta Difesa" => player.Catapulta_Difesa + 1,                                                         
                "Catapulta Livello" => player.Catapulta_Livello + 1,                                                       
                                                                                                                           
                "Ingresso Guarnigione" => player.Ricerca_Ingresso_Guarnigione + 1,                                         
                "Citta Guarnigione" => player.Ricerca_Citta_Guarnigione + 1,                                               
                                                                                                                           
                "Cancello Salute" => player.Ricerca_Cancello_Salute + 1,
                "Cancello Difesa" => player.Ricerca_Cancello_Difesa + 1,
                "Cancello Guarnigione" => player.Ricerca_Cancello_Guarnigione + 1,

                "Mura Salute" => player.Ricerca_Mura_Salute + 1,
                "Mura Difesa" => player.Ricerca_Mura_Difesa + 1,
                "Mura Guarnigione" => player.Ricerca_Mura_Guarnigione + 1,

                "Torri Salute" => player.Ricerca_Torri_Salute + 1,
                "Torri Difesa" => player.Ricerca_Torri_Difesa + 1,
                "Torri Guarnigione" => player.Ricerca_Torri_Guarnigione + 1,

                "Castello Salute" => player.Ricerca_Castello_Salute + 1,
                "Castello Difesa" => player.Ricerca_Castello_Difesa + 1,
                "Castello Guarnigione" => player.Ricerca_Castello_Guarnigione + 1,
                _ => 1
            }; // Carica il livello della ricerca
            if (ControlloLivello(player, researchType, livello)) // Controllo livello per Addestramento
                return;

            var researchCost = GetResearchCost(researchType, livello); // Ottieni costi
            if (researchCost == null)
            {
                Server.Server.Send(clientGuid, $"Log_Server|[error]Tipo ricerca[/error][tile] {researchType} [/tile][error]non valido!");
                return;
            }

            // Controllo risorse
            if (player.Cibo >= researchCost.Cibo &&
                player.Legno >= researchCost.Legno &&
                player.Pietra >= researchCost.Pietra &&
                player.Ferro >= researchCost.Ferro &&
                player.Oro >= researchCost.Oro &&
                player.Popolazione >= researchCost.Popolazione)
            {
                player.Cibo -= researchCost.Cibo;
                player.Legno -= researchCost.Legno;
                player.Pietra -= researchCost.Pietra;
                player.Ferro -= researchCost.Ferro;
                player.Oro -= researchCost.Oro;
                player.Popolazione -= researchCost.Popolazione;

                int risorse = Convert.ToInt32(researchCost.Cibo + researchCost.Legno + researchCost.Pietra + researchCost.Ferro + researchCost.Oro);
                player.Risorse_Utilizzate += risorse;
                OnEvent(player, QuestEventType.Risorse, "Cibo", (int)researchCost.Cibo);
                OnEvent(player, QuestEventType.Risorse, "Legno", (int)researchCost.Legno);
                OnEvent(player, QuestEventType.Risorse, "Pietra", (int)researchCost.Pietra);
                OnEvent(player, QuestEventType.Risorse, "Ferro", (int)researchCost.Ferro);
                OnEvent(player, QuestEventType.Risorse, "Oro", (int)researchCost.Oro);

                Server.Server.Send(clientGuid,
                $"Log_Server|[info]Risorse utilizzate per la ricerca [/info] [title]{researchCost}[/title] [info]livello:[/info] [title]{livello}[/title]\r\n " +
                $"[cibo]-{(researchCost.Cibo):N0}[/cibo] [icon:cibo] " +
                $"[legno]-{(researchCost.Legno):N0}[/legno] [icon:legno] " +
                $"[pietra]-{(researchCost.Pietra):N0}[/pietra] [icon:pietra] " +
                $"[ferro]-{(researchCost.Ferro):N0}[/ferro] [icon:ferro] " +
                $"[oro]-{(researchCost.Oro):N0}[/oro] [icon:oro] " +
                $"[dark]{(researchCost.Popolazione):N0}[/dark] [icon:popolazione]");

                Console.WriteLine($"Risorse utilizzate per {researchType} livello {livello}: " +
                                  $"Cibo={researchCost.Cibo}, Legno={researchCost.Legno}, Pietra={researchCost.Pietra}, " +
                                  $"Ferro={researchCost.Ferro}, Oro={researchCost.Oro}");

                int tempoRicercaInSecondi = Math.Max(1, Convert.ToInt32(researchCost.TempoRicerca));

                // Inserisci nella coda
                if (player.Code_Ricerca > 1 || player.research_Queue.Count == 0 && player.currentTasks_Research.Count == 0)
                    player.research_Queue.Enqueue(new ResearchManager.ResearchTask(researchType, tempoRicercaInSecondi));
                else
                {
                    Server.Server.Send(clientGuid, $"Log_Server|[error]Una singola ricerca è possibile. Ricerca[/error][title] {researchType} [/title][error]]annullata.[/error]");
                    return;
                }
                StartNextResearch(player, clientGuid);
            }
            else
                Server.Server.Send(clientGuid,
                $"Log_Server|[info]Risorse insufficenti per la ricerca [/info] [title]{researchCost}[/title] [info]livello:[/info] [title]{livello}[/title]\r\n " +
                $"[cibo]{(researchCost.Cibo):N0}[/cibo] [icon:cibo] " +
                $"[legno]{(researchCost.Legno):N0}[/legno] [icon:legno] " +
                $"[pietra]{(researchCost.Pietra):N0}[/pietra] [icon:pietra] " +
                $"[ferro]{(researchCost.Ferro):N0}[/ferro] [icon:ferro] " +
                $"[oro]{(researchCost.Oro):N0}[/oro] [icon:oro] " +
                $"[dark]{(researchCost.Popolazione):N0}[/dark] [icon:popolazione]");
            
        }
        static bool ControlloLivello(Player player, string ricerca, int livelloRicerca)
        {
            bool returnValue = false;
            string msg = "";

            if (ricerca == "Produzione" || ricerca == "Costruzione" || ricerca == "Addestramento" && player.Livello < livelloRicerca * 3)
                {returnValue = true; msg = "giocatore";}

            if (ricerca == "Guerriero Livello" && player.Guerriero_Livello < player.Livello * 3) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Guerriero Salute" && player.Guerriero_Salute < livelloRicerca * 3) {returnValue = true; msg = "guerriero";}
            if (ricerca == "Guerriero Attacco" && player.Guerriero_Attacco < livelloRicerca * 3) {returnValue = true; msg = "guerriero";}
            if (ricerca == "Guerriero Difesa" && player.Guerriero_Difesa < livelloRicerca * 3) {returnValue = true; msg = "guerriero"; }
                        
            if (ricerca == "Lancere Livello" && player.Lancere_Livello < player.Livello * 3) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Lancere Salute" && player.Lancere_Salute < livelloRicerca * 3) {returnValue = true; msg = "lancere";}
            if (ricerca == "Lancere Attacco" && player.Lancere_Attacco < livelloRicerca * 3) {returnValue = true; msg = "lancere"; }
            if (ricerca == "Lancere Difesa" && player.Lancere_Difesa < livelloRicerca * 3) {returnValue = true; msg = "lancere"; }
                        
            if (ricerca == "Arcere Livello" && player.Arcere_Livello < player.Livello * 3) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Arcere Salute" && player.Arcere_Salute < livelloRicerca * 3) {returnValue = true; msg = "Arcere";}
            if (ricerca == "Arcere Attacco" && player.Arcere_Attacco < livelloRicerca * 3) {returnValue = true; msg = "Arcere"; }
            if (ricerca == "Arcere Difesa" && player.Arcere_Difesa < livelloRicerca * 3) {returnValue = true; msg = "Arcere"; }
                        
            if (ricerca == "Catapulta Livello" && player.Catapulta_Livello < player.Livello * 3) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Catapulta Salute" && player.Catapulta_Salute < livelloRicerca * 3) {returnValue = true; msg = "Catapulta";}
            if (ricerca == "Catapulta Attacco" && player.Catapulta_Attacco < livelloRicerca * 3) {returnValue = true; msg = "Catapulta"; }
            if (ricerca == "Catapulta Difesa" && player.Catapulta_Difesa < livelloRicerca * 3) {returnValue = true; msg = "Catapulta"; }
                        
            if (ricerca == "Ingresso Guarnigione" && player.Ricerca_Ingresso_Guarnigione < player.Livello * 5) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Citta Guarnigione" && player.Ricerca_Citta_Guarnigione < player.Livello * 5) {returnValue = true; msg = "giocatore";}
                        
            if (ricerca == "Cancello Salute" && player.Ricerca_Cancello_Salute < player.Livello * 5) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Cancello Difesa" && player.Ricerca_Cancello_Difesa < player.Livello * 5) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Cancello Guarnigione" && player.Ricerca_Cancello_Guarnigione < player.Livello * 5) {returnValue = true; msg = "giocatore";}
                        
            if (ricerca == "Mura Salute" && player.Ricerca_Mura_Salute < player.Livello * 5) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Mura Difesa" && player.Ricerca_Mura_Difesa < player.Livello * 5) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Mura Guarnigione" && player.Ricerca_Mura_Guarnigione < player.Livello * 5) {returnValue = true; msg = "giocatore";}

            if (ricerca == "Torri Salute" && player.Ricerca_Torri_Salute < player.Livello * 5) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Torri Difesa" && player.Ricerca_Torri_Difesa < player.Livello * 5) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Torri Guarnigione" && player.Ricerca_Torri_Guarnigione < player.Livello * 5) {returnValue = true; msg = "giocatore";}

            if (ricerca == "Castello Salute" && player.Ricerca_Castello_Salute < player.Livello * 5) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Castello Difesa" && player.Ricerca_Castello_Difesa < player.Livello * 5) {returnValue = true; msg = "giocatore";}
            if (ricerca == "Castello Guarnigione" && player.Ricerca_Castello_Guarnigione < player.Livello * 5) {returnValue = true; msg = "giocatore";}

            if (returnValue == true)
                Server.Server.Send(player.guid_Player, $"Log_Server|[error]La ricerca [/error][title]{ricerca} {livelloRicerca} [/title][error]richiede che il livello del [title]{msg}[/title][error] sia almeno lv [/error][title]{livelloRicerca * 3}");

            return returnValue;
        }

        // Avvia i task finché ci sono slot liberi
        private static void StartNextResearch(Player player, Guid clientGuid)
        {
            int maxSlots = player.Code_Ricerca; // Parametrizzabile
            bool ciSonoRicercheInCorso = player.currentTasks_Research.Any(t => !t.IsComplete());

            if (ciSonoRicercheInCorso) // 🔒 Se esistono costruzioni ancora in corso, NON iniziarne nuove
                return;

            while (player.currentTasks_Research.Count < maxSlots && player.research_Queue.Count > 0)
            {
                player.Ricerca_Attiva = true;// blocca i pulsanti ricerca del client
                var nextTask = player.research_Queue.Dequeue();
                nextTask.Start();
                player.currentTasks_Research.Add(nextTask);

                Console.WriteLine($"Ricerca di {nextTask.Type} iniziata, durata {nextTask.DurationInSeconds}s");
                Server.Server.Send(clientGuid, $"Log_Server|[info]Ricerca di[/info][title] {nextTask.Type} [/title][info]iniziata.");
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
                    Server.Server.Send(clientGuid, $"Log_Server|[success]Ricerca completata: [/success][title]{task.Type}");

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

                case "Guerriero Salute":
                    player.Guerriero_Salute++;
                    break;
                case "Guerriero Attacco":
                    player.Guerriero_Attacco++;
                    break;
                case "Guerriero Difesa":
                    player.Guerriero_Difesa++;
                    break;
                case "Guerriero Livello":
                    player.Guerriero_Livello++;
                    break;
                case "Lancere Salute":
                    player.Lancere_Salute++;
                    break;
                case "Lancere Attacco":
                    player.Lancere_Attacco++;
                    break;
                case "Lancere Difesa":
                    player.Lancere_Difesa++;
                    break;
                case "Lancere Livello":
                    player.Lancere_Livello++;
                    break;
                case "Arcere Salute":
                    player.Arcere_Salute++;
                    break;
                case "Arcere Attacco":
                    player.Arcere_Attacco++;
                    break;
                case "Arcere Difesa":
                    player.Arcere_Difesa++;
                    break;
                case "Arcere Livello":
                    player.Arcere_Livello++;
                    break;
                case "Catapulta Salute":
                    player.Catapulta_Salute++;
                    break;
                case "Catapulta Attacco":
                    player.Catapulta_Attacco++;
                    break;
                case "Catapulta Difesa":
                    player.Catapulta_Difesa++;
                    break;
                case "Catapulta Livello":
                    player.Catapulta_Livello++;
                    break;

                case "Ingresso Guarnigione":
                    player.Ricerca_Ingresso_Guarnigione++;
                    break;
                case "Citta Guarnigione":
                    player.Ricerca_Citta_Guarnigione++;
                    break;
                case "Cancello Salute":
                    player.Ricerca_Cancello_Salute++;
                    break;
                case "Cancello Difesa":
                    player.Ricerca_Cancello_Difesa++;
                    break;
                case "Cancello Guarnigione":
                    player.Ricerca_Cancello_Guarnigione++;
                    break;
                case "Mura Salute":
                    player.Ricerca_Mura_Salute++;
                    break;
                case "Mura Difesa":
                    player.Ricerca_Mura_Difesa++;
                    break;
                case "Mura Guarnigione":
                    player.Ricerca_Mura_Guarnigione++;
                    break;
                case "Torri Salute":
                    player.Ricerca_Torri_Salute++;
                    break;
                case "Torri Difesa":
                    player.Ricerca_Torri_Difesa++;
                    break;
                case "Torri Guarnigione":
                    player.Ricerca_Torri_Guarnigione++;
                    break;
                case "Castello Salute":
                    player.Ricerca_Castello_Salute++;
                    break;
                case "Castello Difesa":
                    player.Ricerca_Castello_Difesa++;
                    break;
                case "Castello Guarnigione":
                    player.Ricerca_Castello_Guarnigione++;
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
                    Popolazione = 0,
                    TempoRicerca = Tipi.Addestramento.TempoCostruzione * livello // o calcola dinamicamente
                },
                "Costruzione" => new ResearchCost
                {
                    Cibo = Tipi.Costruzione.Cibo * livello,
                    Legno = Tipi.Costruzione.Legno * livello,
                    Pietra = Tipi.Costruzione.Pietra * livello,
                    Ferro = Tipi.Costruzione.Ferro * livello,
                    Oro = Tipi.Costruzione.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Tipi.Addestramento.TempoCostruzione * livello
                },
                "Produzione" => new ResearchCost
                {
                    Cibo = Tipi.Produzione.Cibo * livello,
                    Legno = Tipi.Produzione.Legno * livello,
                    Pietra = Tipi.Produzione.Pietra * livello,
                    Ferro = Tipi.Produzione.Ferro * livello,
                    Oro = Tipi.Produzione.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Tipi.Addestramento.TempoCostruzione * livello
                },
                "Popolazione" => new ResearchCost
                {
                    Cibo = Tipi.Popolazione.Cibo * livello,
                    Legno = Tipi.Popolazione.Legno * livello,
                    Pietra = Tipi.Popolazione.Pietra * livello,
                    Ferro = Tipi.Popolazione.Ferro * livello,
                    Oro = Tipi.Popolazione.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Tipi.Addestramento.TempoCostruzione * livello // più tempo per livelli più alti
                },

                "Guerriero Salute" => new ResearchCost
                {
                    Cibo = Soldati.Salute.Cibo * livello,
                    Legno = Soldati.Salute.Legno * livello,
                    Pietra = Soldati.Salute.Pietra * livello,
                    Ferro = Soldati.Salute.Ferro * livello,
                    Oro = Soldati.Salute.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Salute.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Guerriero Attacco" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Attacco.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Guerriero Difesa" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Attacco.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Guerriero Livello" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Attacco.TempoCostruzione * livello // più tempo per livelli più alti
                },

                "Lancere Salute" => new ResearchCost
                {
                    Cibo = Soldati.Salute.Cibo * livello,
                    Legno = Soldati.Salute.Legno * livello,
                    Pietra = Soldati.Salute.Pietra * livello,
                    Ferro = Soldati.Salute.Ferro * livello,
                    Oro = Soldati.Salute.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Salute.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Lancere Attacco" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Attacco.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Lancere Difesa" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Attacco.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Lancere Livello" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Attacco.TempoCostruzione * livello // più tempo per livelli più alti
                },

                "Arcere Salute" => new ResearchCost
                {
                    Cibo = Soldati.Salute.Cibo * livello,
                    Legno = Soldati.Salute.Legno * livello,
                    Pietra = Soldati.Salute.Pietra * livello,
                    Ferro = Soldati.Salute.Ferro * livello,
                    Oro = Soldati.Salute.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Salute.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Arcere Attacco" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Attacco.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Arcere Difesa" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Attacco.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Arcere Livello" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Attacco.TempoCostruzione * livello // più tempo per livelli più alti
                },

                "Catapulta Salute" => new ResearchCost
                {
                    Cibo = Soldati.Salute.Cibo * livello,
                    Legno = Soldati.Salute.Legno * livello,
                    Pietra = Soldati.Salute.Pietra * livello,
                    Ferro = Soldati.Salute.Ferro * livello,
                    Oro = Soldati.Salute.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Salute.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Catapulta Attacco" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Attacco.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Catapulta Difesa" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Attacco.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Catapulta Livello" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = 0,
                    TempoRicerca = Soldati.Attacco.TempoCostruzione * livello // più tempo per livelli più alti
                },

                "Ingresso Guarnigione" => new ResearchCost
                {
                    Cibo = Citta.Ingresso.Cibo * livello,
                    Legno = Citta.Ingresso.Legno * livello,
                    Pietra = Citta.Ingresso.Pietra * livello,
                    Ferro = Citta.Ingresso.Ferro * livello,
                    Oro = Citta.Ingresso.Oro * livello,
                    Popolazione = Citta.Ingresso.Popolazione * livello,
                    TempoRicerca = Citta.Ingresso.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Citta Guarnigione" => new ResearchCost
                {
                    Cibo = Citta.Città.Cibo * livello,
                    Legno = Citta.Città.Legno * livello,
                    Pietra = Citta.Città.Pietra * livello,
                    Ferro = Citta.Città.Ferro * livello,
                    Oro = Citta.Città.Oro * livello,
                    Popolazione = Citta.Città.Popolazione * livello,
                    TempoRicerca = Citta.Città.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Cancello Salute" => new ResearchCost
                {
                    Cibo = Citta.Cancello.Cibo * livello,
                    Legno = Citta.Cancello.Legno * livello,
                    Pietra = Citta.Cancello.Pietra * livello,
                    Ferro = Citta.Cancello.Ferro * livello,
                    Oro = Citta.Cancello.Oro * livello,
                    Popolazione = Citta.Cancello.Popolazione * livello,
                    TempoRicerca = Citta.Cancello.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Cancello Difesa" => new ResearchCost
                {
                    Cibo = Citta.Cancello.Cibo * livello,
                    Legno = Citta.Cancello.Legno * livello,
                    Pietra = Citta.Cancello.Pietra * livello,
                    Ferro = Citta.Cancello.Ferro * livello,
                    Oro = Citta.Cancello.Oro * livello,
                    Popolazione = Citta.Cancello.Popolazione * livello,
                    TempoRicerca = Citta.Cancello.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Cancello Guarnigione" => new ResearchCost
                {
                    Cibo = Citta.Cancello.Cibo * livello,
                    Legno = Citta.Cancello.Legno * livello,
                    Pietra = Citta.Cancello.Pietra * livello,
                    Ferro = Citta.Cancello.Ferro * livello,
                    Oro = Citta.Cancello.Oro * livello,
                    Popolazione = Citta.Cancello.Popolazione * livello,
                    TempoRicerca = Citta.Cancello.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Mura Salute" => new ResearchCost
                {
                    Cibo = Citta.Mura.Cibo * livello,
                    Legno = Citta.Mura.Legno * livello,
                    Pietra = Citta.Mura.Pietra * livello,
                    Ferro = Citta.Mura.Ferro * livello,
                    Oro = Citta.Mura.Oro * livello,
                    Popolazione = Citta.Mura.Popolazione * livello,
                    TempoRicerca = Citta.Mura.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Mura Difesa" => new ResearchCost
                {
                    Cibo = Citta.Mura.Cibo * livello,
                    Legno = Citta.Mura.Legno * livello,
                    Pietra = Citta.Mura.Pietra * livello,
                    Ferro = Citta.Mura.Ferro * livello,
                    Oro = Citta.Mura.Oro * livello,
                    Popolazione = Citta.Mura.Popolazione * livello,
                    TempoRicerca = Citta.Mura.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Mura Guarnigione" => new ResearchCost
                {
                    Cibo = Citta.Mura.Cibo * livello,
                    Legno = Citta.Mura.Legno * livello,
                    Pietra = Citta.Mura.Pietra * livello,
                    Ferro = Citta.Mura.Ferro * livello,
                    Oro = Citta.Mura.Oro * livello,
                    Popolazione = Citta.Mura.Popolazione * livello,
                    TempoRicerca = Citta.Mura.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Torri Salute" => new ResearchCost
                {
                    Cibo = Citta.Torri.Cibo * livello,
                    Legno = Citta.Torri.Legno * livello,
                    Pietra = Citta.Torri.Pietra * livello,
                    Ferro = Citta.Torri.Ferro * livello,
                    Oro = Citta.Torri.Oro * livello,
                    Popolazione = Citta.Torri.Popolazione * livello,
                    TempoRicerca = Citta.Torri.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Torri Difesa" => new ResearchCost
                {
                    Cibo = Citta.Torri.Cibo * livello,
                    Legno = Citta.Torri.Legno * livello,
                    Pietra = Citta.Torri.Pietra * livello,
                    Ferro = Citta.Torri.Ferro * livello,
                    Oro = Citta.Torri.Oro * livello,
                    Popolazione = Citta.Torri.Popolazione * livello,
                    TempoRicerca = Citta.Torri.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Torri Guarnigione" => new ResearchCost
                {
                    Cibo = Citta.Torri.Cibo * livello,
                    Legno = Citta.Torri.Legno * livello,
                    Pietra = Citta.Torri.Pietra * livello,
                    Ferro = Citta.Torri.Ferro * livello,
                    Oro = Citta.Torri.Oro * livello,
                    Popolazione = Citta.Torri.Popolazione * livello,
                    TempoRicerca = Citta.Torri.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Castello Salute" => new ResearchCost
                {
                    Cibo = Citta.Castello.Cibo * livello,
                    Legno = Citta.Castello.Legno * livello,
                    Pietra = Citta.Castello.Pietra * livello,
                    Ferro = Citta.Castello.Ferro * livello,
                    Oro = Citta.Castello.Oro * livello,
                    Popolazione = Citta.Castello.Popolazione * livello,
                    TempoRicerca = Citta.Castello.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Castello Difesa" => new ResearchCost
                {
                    Cibo = Citta.Castello.Cibo * livello,
                    Legno = Citta.Castello.Legno * livello,
                    Pietra = Citta.Castello.Pietra * livello,
                    Ferro = Citta.Castello.Ferro * livello,
                    Oro = Citta.Castello.Oro * livello,
                    Popolazione = Citta.Castello.Popolazione * livello,
                    TempoRicerca = Citta.Castello.TempoCostruzione * livello // più tempo per livelli più alti
                },
                "Castello Guarnigione" => new ResearchCost
                {
                    Cibo = Citta.Castello.Cibo * livello,
                    Legno = Citta.Castello.Legno * livello,
                    Pietra = Citta.Castello.Pietra * livello,
                    Ferro = Citta.Castello.Ferro * livello,
                    Oro = Citta.Castello.Oro * livello,
                    Popolazione = Citta.Castello.Popolazione * livello,
                    TempoRicerca = Citta.Castello.TempoCostruzione * livello // più tempo per livelli più alti
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
            public double Popolazione { get; set; }
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
