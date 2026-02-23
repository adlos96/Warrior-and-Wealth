using Server_Strategico.ServerData.Moduli;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Manager.QuestManager;
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
                "Trasporto" => player.Ricerca_Trasporto + 1,
                "Riparazione" => player.Ricerca_Riparazione + 1,

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

                "Cancello Livello" => player.Ricerca_Cancello_Livello + 1,
                "Cancello Salute" => player.Ricerca_Cancello_Salute + 1,
                "Cancello Difesa" => player.Ricerca_Cancello_Difesa + 1,
                "Cancello Guarnigione" => player.Ricerca_Cancello_Guarnigione + 1,

                "Mura Livello" => player.Ricerca_Mura_Livello + 1,
                "Mura Salute" => player.Ricerca_Mura_Salute + 1,
                "Mura Difesa" => player.Ricerca_Mura_Difesa + 1,
                "Mura Guarnigione" => player.Ricerca_Mura_Guarnigione + 1,

                "Torri Livello" => player.Ricerca_Torri_Livello + 1,
                "Torri Salute" => player.Ricerca_Torri_Salute + 1,
                "Torri Difesa" => player.Ricerca_Torri_Difesa + 1,
                "Torri Guarnigione" => player.Ricerca_Torri_Guarnigione + 1,

                "Castello Livello" => player.Ricerca_Castello_Livello + 1,
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
                Server.Server.Send(clientGuid, $"Log_Server|[error]Tipo ricerca[tile] {researchType} [error]non valido!");
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
                OnEvent(player, QuestEventType.Risorse, "Popolazione", (int)researchCost.Popolazione);

                Server.Server.Send(clientGuid,
                $"Log_Server|[info]Risorse utilizzate[/info] per la ricerca [warning]{researchType} [info]livello: [warning]{livello}[/warning]\r\n " +
                $"[cibo][icon:cibo]-{(researchCost.Cibo):N0}[/cibo] " +
                $"[legno][icon:legno]-{(researchCost.Legno):N0}[/legno] " +
                $"[pietra][icon:pietra]-{(researchCost.Pietra):N0}[/pietra] " +
                $"[ferro][icon:ferro]-{(researchCost.Ferro):N0}[/ferro] " +
                $"[oro][icon:oro]-{(researchCost.Oro):N0}[/oro] " +
                $"[dark][icon:popolazione]{(researchCost.Popolazione):N0}[/dark]");

                Console.WriteLine($"Risorse utilizzate per {researchType} livello {livello}: " +
                                  $"Cibo={researchCost.Cibo}, Legno={researchCost.Legno}, Pietra={researchCost.Pietra}, " +
                                  $"Ferro={researchCost.Ferro}, Oro={researchCost.Oro}");

                int tempoRicercaInSecondi = Math.Max(1, Convert.ToInt32(researchCost.TempoRicerca - (researchCost.TempoRicerca * player.Bonus_Ricerca)));

                // Inserisci nella coda
                if (player.Code_Ricerca > 1 || player.research_Queue.Count == 0 && player.currentTasks_Research.Count == 0)
                    player.research_Queue.Enqueue(new ResearchManager.ResearchTask(researchType, tempoRicercaInSecondi));
                else
                {
                    Server.Server.Send(clientGuid, $"Log_Server|[error]Una singola ricerca è possibile. Ricerca[title] {researchType} [error]]annullata.");
                    return;
                }
                StartNextResearch(player, clientGuid);
            }
            else
                Server.Server.Send(clientGuid,
                $"Log_Server|[error]Risorse insufficenti per la ricerca [warning]{researchType} [/warning]livello: [title]{livello}\r\n " +
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
            int richiesto = 0;
            string msg = "";

            int livelloRichiesto = (int)Math.Ceiling((double)livelloRicerca / 3.0) * 2;

            if ((ricerca == "Produzione" || ricerca == "Costruzione" || ricerca == "Addestramento" || ricerca == "Popolazione") && player.Livello < livelloRichiesto)
            { returnValue = true; richiesto = livelloRichiesto; msg = "giocatore"; }
            if ((ricerca == "Trasporto") && player.Livello < livelloRichiesto * 2)
            { returnValue = true; richiesto = livelloRichiesto; msg = "giocatore"; }
            if ((ricerca == "Riparazione") && player.Livello < livelloRichiesto * 15)
            { returnValue = true; richiesto = livelloRichiesto; msg = "giocatore"; }

            if (ricerca == "Guerriero Livello" && player.Livello <= (player.Guerriero_Livello + 1) * 2) {returnValue = true; richiesto = (player.Guerriero_Livello + 1) * 2; msg = "giocatore";}
            if (ricerca == "Guerriero Salute" && player.Guerriero_Salute >= (player.Guerriero_Livello + 1) * 2) {returnValue = true; richiesto = (player.Guerriero_Livello + 1) * 2; msg = "guerriero";}
            if (ricerca == "Guerriero Attacco" && player.Guerriero_Attacco >= (player.Guerriero_Livello + 1) * 2) {returnValue = true; richiesto = (player.Guerriero_Livello + 1) * 2;  msg = "guerriero";}
            if (ricerca == "Guerriero Difesa" && player.Guerriero_Difesa >= (player.Guerriero_Livello + 1) * 2) {returnValue = true; richiesto = (player.Guerriero_Livello + 1) * 2; msg = "guerriero"; }
                   
            if (ricerca == "Lancere Livello" && player.Livello <= (player.Lancere_Livello + 1) * 2) {returnValue = true; richiesto = (player.Lancere_Livello + 1) * 2; msg = "giocatore";}
            if (ricerca == "Lancere Salute" && player.Lancere_Salute >= (player.Lancere_Livello + 1) * 2) {returnValue = true; richiesto = (player.Lancere_Livello + 1) * 2; msg = "lancere";}
            if (ricerca == "Lancere Attacco" && player.Lancere_Attacco >= (player.Lancere_Livello + 1) * 2) {returnValue = true; richiesto = (player.Lancere_Livello + 1) * 2; msg = "lancere"; }
            if (ricerca == "Lancere Difesa" && player.Lancere_Difesa >= (player.Lancere_Livello + 1) * 2) {returnValue = true; richiesto = (player.Lancere_Livello + 1) * 2; msg = "lancere"; }
                   
            if (ricerca == "Arcere Livello" && player.Livello <= (player.Arcere_Livello + 1) * 2) {returnValue = true; richiesto = (player.Arcere_Livello + 1) * 2; msg = "giocatore";}
            if (ricerca == "Arcere Salute" && player.Arcere_Salute >= (player.Arcere_Livello + 1) * 2) {returnValue = true; richiesto = (player.Arcere_Livello + 1) * 2; msg = "Arcere";}
            if (ricerca == "Arcere Attacco" && player.Arcere_Attacco >= (player.Arcere_Livello + 1) * 2) {returnValue = true; richiesto = (player.Arcere_Livello + 1) * 2; msg = "Arcere"; }
            if (ricerca == "Arcere Difesa" && player.Arcere_Difesa >= (player.Arcere_Livello + 1) * 2) {returnValue = true; richiesto = (player.Arcere_Livello + 1) * 2; msg = "Arcere"; }
                   
            if (ricerca == "Catapulta Livello" && player.Livello <= (player.Catapulta_Livello + 1) * 2) {returnValue = true; richiesto = (player.Catapulta_Livello + 1) * 2; msg = "giocatore";}
            if (ricerca == "Catapulta Salute" && player.Catapulta_Salute >= (player.Catapulta_Livello + 1) * 2) {returnValue = true; richiesto = (player.Catapulta_Livello + 1) * 2; msg = "catapulta";}
            if (ricerca == "Catapulta Attacco" && player.Catapulta_Attacco >= (player.Catapulta_Livello + 1) * 2) {returnValue = true; richiesto = (player.Catapulta_Livello + 1) * 2; msg = "catapulta"; }
            if (ricerca == "Catapulta Difesa" && player.Catapulta_Difesa >= (player.Catapulta_Livello + 1) * 2) {returnValue = true; richiesto = (player.Catapulta_Livello + 1) * 2; msg = "catapulta"; }
                   
            if (ricerca == "Ingresso Guarnigione" && player.Livello <= (player.Ricerca_Ingresso_Guarnigione + 1) * 4) {returnValue = true; richiesto = (player.Ricerca_Ingresso_Guarnigione + 1) * 4; msg = "giocatore";}
            if (ricerca == "Citta Guarnigione" && player.Livello <= (player.Ricerca_Citta_Guarnigione + 1) * 4) {returnValue = true; richiesto = (player.Ricerca_Citta_Guarnigione + 1) * 4; msg = "giocatore";}

            if (ricerca == "Cancello Livello" && player.Livello <= (player.Ricerca_Cancello_Livello + 1) * 4) { returnValue = true; richiesto = (player.Ricerca_Cancello_Livello + 1) * 4; msg = "giocatore"; }
            if (ricerca == "Cancello Salute" && player.Ricerca_Cancello_Livello * 2 <= player.Ricerca_Cancello_Salute) {returnValue = true; richiesto = (player.Ricerca_Cancello_Salute + 1) * 4; msg = "cancello"; }
            if (ricerca == "Cancello Difesa" && player.Ricerca_Cancello_Livello * 2 <= player.Ricerca_Cancello_Difesa) {returnValue = true; richiesto = (player.Ricerca_Cancello_Difesa + 1) * 4; msg = "cancello"; }
            if (ricerca == "Cancello Guarnigione" && player.Ricerca_Cancello_Livello * 2 <= player.Ricerca_Cancello_Guarnigione) {returnValue = true; richiesto = (player.Ricerca_Cancello_Guarnigione + 1) * 4; msg = "cancello"; }

            if (ricerca == "Mura Livello" && player.Livello <= (player.Ricerca_Mura_Livello + 1) * 4) { returnValue = true; richiesto = (player.Ricerca_Mura_Livello + 1) * 4; msg = "giocatore"; }
            if (ricerca == "Mura Salute" && player.Ricerca_Mura_Livello * 2 <= player.Ricerca_Mura_Salute) {returnValue = true; richiesto = (player.Ricerca_Mura_Salute + 1) * 4; msg = "mura"; }
            if (ricerca == "Mura Difesa" && player.Ricerca_Mura_Livello * 2 <= player.Ricerca_Mura_Difesa) {returnValue = true; richiesto = (player.Ricerca_Mura_Difesa + 1) * 4; msg = "mura"; }
            if (ricerca == "Mura Guarnigione" && player.Ricerca_Mura_Livello * 2 <= player.Ricerca_Mura_Guarnigione) {returnValue = true; richiesto = (player.Ricerca_Mura_Guarnigione + 1) * 4; msg = "mura"; }

            if (ricerca == "Torri Livello" && player.Livello <= (player.Ricerca_Torri_Livello + 1) * 4) { returnValue = true; richiesto = (player.Ricerca_Torri_Livello + 1) * 4; msg = "giocatore"; }
            if (ricerca == "Torri Salute" && player.Ricerca_Torri_Livello * 2 <= player.Ricerca_Torri_Salute) {returnValue = true; richiesto = (player.Ricerca_Torri_Salute + 1) * 4; msg = "torri";}
            if (ricerca == "Torri Difesa" && player.Ricerca_Torri_Livello * 2 <= player.Ricerca_Torri_Difesa) {returnValue = true; richiesto = (player.Ricerca_Torri_Difesa + 1) * 4; msg = "torri"; }
            if (ricerca == "Torri Guarnigione" && player.Ricerca_Torri_Livello * 2 <= player.Ricerca_Torri_Guarnigione) {returnValue = true; richiesto = (player.Ricerca_Torri_Guarnigione + 1) * 4; msg = "torri"; }

            if (ricerca == "Castello Livello" && player.Livello <= (player.Ricerca_Castello_Livello + 1) * 4) { returnValue = true; richiesto = (player.Ricerca_Castello_Livello + 1) * 4; msg = "giocacatore"; }
            if (ricerca == "Castello Salute" && player.Ricerca_Castello_Livello * 2 == player.Ricerca_Castello_Salute) {returnValue = true; richiesto = (player.Ricerca_Castello_Livello + 1) * 4; msg = "castello"; }
            if (ricerca == "Castello Difesa" && player.Ricerca_Castello_Livello * 2 == player.Ricerca_Castello_Difesa) {returnValue = true; richiesto =  (player.Ricerca_Castello_Difesa + 1) * 4; msg = "castello"; }
            if (ricerca == "Castello Guarnigione" && player.Ricerca_Castello_Livello * 2 == player.Ricerca_Castello_Guarnigione) {returnValue = true; richiesto = (player.Ricerca_Castello_Guarnigione + 1) * 4; msg = "castello"; }

            if (returnValue == true)
                Server.Server.Send(player.guid_Player, $"Log_Server|[error]La ricerca [title]{ricerca} {livelloRicerca} [error]richiede che il livello del [title]{msg}[error] sia almeno lv [title]{richiesto}");

            return returnValue;
        }
        private static void StartNextResearch(Player player, Guid clientGuid) // Avvia i task finché ci sono slot liberi
        {
            int maxSlots = player.Code_Ricerca; // Parametrizzabile
            bool ciSonoRicercheInCorso = player.currentTasks_Research.Any(t => !t.IsComplete());
            if (ciSonoRicercheInCorso) return; // Se esistono costruzioni ancora in corso, NON iniziarne nuove

            while (player.currentTasks_Research.Count < maxSlots && player.research_Queue.Count > 0)
            {
                player.Ricerca_Attiva = true;// blocca i pulsanti ricerca del client
                var nextTask = player.research_Queue.Dequeue();
                if (nextTask != null)
                    nextTask.Start();
                player.currentTasks_Research.Add(nextTask);

                Console.WriteLine($"Ricerca di {nextTask.Type} iniziata, durata {player.FormatTime(nextTask.DurationInSeconds)}s");
                Server.Server.Send(clientGuid, $"Log_Server|[info]Ricerca di [title]{nextTask.Type} [title]iniziata. Durata [icon:tempo]{player.FormatTime(nextTask.DurationInSeconds)}");
            }
        }
        public static void CompleteResearch(Guid clientGuid, Player player)
        {
            if (player.currentTasks_Research == null || player.currentTasks_Research.Count == 0) return;

            // STEP 1: Raccogli tutti i task completati
            var completedTasks = new List<ResearchTask>();
            foreach (var task in player.currentTasks_Research)
                if (task.IsComplete()) completedTasks.Add(task);

            // STEP 2: Processa e rimuovi i task completati
            foreach (var task in completedTasks)
            {
                ApplyResearchEffects(task.Type, player);
                player.Ricerca_Attiva = false; // sblocca i pulsanti ricerca del client

                Console.WriteLine($"Ricerca completata: {task.Type}");
                Server.Server.Send(clientGuid, $"Log_Server|[success]Ricerca completata: [title]{task.Type}");
                player.currentTasks_Research.Remove(task); // Rimozione sicura
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
                case "Trasporto":
                    player.Ricerca_Trasporto++;
                    break;
                case "Riparazione":
                    player.Ricerca_Riparazione++;
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

                case "Cancello Livello":
                    player.Ricerca_Cancello_Livello++;
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

                case "Mura Livello":
                    player.Ricerca_Mura_Livello++;
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

                case "Torri Livello":
                    player.Ricerca_Torri_Livello++;
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

                case "Castello Livello":
                    player.Ricerca_Castello_Livello++;
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
                    Console.WriteLine($"[error]Ricerca [title]{researchType} [error]non definita negli effetti!");
                    break;
            }
            player.SetupVillaggioGiocatore(player);
            Descrizioni.DescUpdate(player); //Aggiornare le descrizioni per i nuovi dati
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
                "Produzione" => new ResearchCost
                {
                    Cibo = Tipi.Produzione.Cibo * livello,
                    Legno = Tipi.Produzione.Legno * livello,
                    Pietra = Tipi.Produzione.Pietra * livello,
                    Ferro = Tipi.Produzione.Ferro * livello,
                    Oro = Tipi.Produzione.Oro * livello,
                    Popolazione = 30,
                    TempoRicerca = Tipi.Addestramento.TempoRicerca * livello
                },
                "Costruzione" => new ResearchCost
                {
                    Cibo = Tipi.Costruzione.Cibo * livello,
                    Legno = Tipi.Costruzione.Legno * livello,
                    Pietra = Tipi.Costruzione.Pietra * livello,
                    Ferro = Tipi.Costruzione.Ferro * livello,
                    Oro = Tipi.Costruzione.Oro * livello,
                    Popolazione = 25,
                    TempoRicerca = Tipi.Addestramento.TempoRicerca * livello
                },
                "Addestramento" => new ResearchCost
                {
                    Cibo = Tipi.Addestramento.Cibo * livello,
                    Legno = Tipi.Addestramento.Legno * livello,
                    Pietra = Tipi.Addestramento.Pietra * livello,
                    Ferro = Tipi.Addestramento.Ferro * livello,
                    Oro = Tipi.Addestramento.Oro * livello,
                    Popolazione = 25,
                    TempoRicerca = Tipi.Addestramento.TempoRicerca * livello // o calcola dinamicamente
                },
                "Popolazione" => new ResearchCost
                {
                    Cibo = Tipi.Popolazione.Cibo * livello,
                    Legno = Tipi.Popolazione.Legno * livello,
                    Pietra = Tipi.Popolazione.Pietra * livello,
                    Ferro = Tipi.Popolazione.Ferro * livello,
                    Oro = Tipi.Popolazione.Oro * livello,
                    Popolazione = 50,
                    TempoRicerca = Tipi.Addestramento.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Trasporto" => new ResearchCost
                {
                    Cibo = Tipi.Trasporto.Cibo * livello,
                    Legno = Tipi.Trasporto.Legno * livello,
                    Pietra = Tipi.Trasporto.Pietra * livello,
                    Ferro = Tipi.Trasporto.Ferro * livello,
                    Oro = Tipi.Trasporto.Oro * livello,
                    Popolazione = 40,
                    TempoRicerca = Tipi.Addestramento.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Riparazione" => new ResearchCost
                {
                    Cibo = Tipi.Riparazione.Cibo * livello,
                    Legno = Tipi.Riparazione.Legno * livello,
                    Pietra = Tipi.Riparazione.Pietra * livello,
                    Ferro = Tipi.Riparazione.Ferro * livello,
                    Oro = Tipi.Riparazione.Oro * livello,
                    Popolazione = 60,
                    TempoRicerca = Tipi.Addestramento.TempoRicerca * livello // più tempo per livelli più alti
                },

                "Guerriero Salute" => new ResearchCost
                {
                    Cibo = Soldati.Salute.Cibo * livello,
                    Legno = Soldati.Salute.Legno * livello,
                    Pietra = Soldati.Salute.Pietra * livello,
                    Ferro = Soldati.Salute.Ferro * livello,
                    Oro = Soldati.Salute.Oro * livello,
                    Popolazione = Soldati.Salute.Popolazione * livello,
                    TempoRicerca = Soldati.Salute.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Guerriero Attacco" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = Soldati.Attacco.Popolazione * livello,
                    TempoRicerca = Soldati.Attacco.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Guerriero Difesa" => new ResearchCost
                {
                    Cibo = Soldati.Difesa.Cibo * livello,
                    Legno = Soldati.Difesa.Legno * livello,
                    Pietra = Soldati.Difesa.Pietra * livello,
                    Ferro = Soldati.Difesa.Ferro * livello,
                    Oro = Soldati.Difesa.Oro * livello,
                    Popolazione = Soldati.Difesa.Popolazione * livello,
                    TempoRicerca = Soldati.Difesa.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Guerriero Livello" => new ResearchCost
                {
                    Cibo = Soldati.Livello.Cibo * livello,
                    Legno = Soldati.Livello.Legno * livello,
                    Pietra = Soldati.Livello.Pietra * livello,
                    Ferro = Soldati.Livello.Ferro * livello,
                    Oro = Soldati.Livello.Oro * livello,
                    Popolazione = Soldati.Livello.Popolazione * livello,
                    TempoRicerca = Soldati.Livello.TempoRicerca * livello // più tempo per livelli più alti
                },

                "Lancere Salute" => new ResearchCost
                {
                    Cibo = Soldati.Salute.Cibo * livello,
                    Legno = Soldati.Salute.Legno * livello,
                    Pietra = Soldati.Salute.Pietra * livello,
                    Ferro = Soldati.Salute.Ferro * livello,
                    Oro = Soldati.Salute.Oro * livello,
                    Popolazione = Soldati.Salute.Popolazione * livello,
                    TempoRicerca = Soldati.Salute.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Lancere Attacco" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = Soldati.Attacco.Popolazione * livello,
                    TempoRicerca = Soldati.Attacco.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Lancere Difesa" => new ResearchCost
                {
                    Cibo = Soldati.Difesa.Cibo * livello,
                    Legno = Soldati.Difesa.Legno * livello,
                    Pietra = Soldati.Difesa.Pietra * livello,
                    Ferro = Soldati.Difesa.Ferro * livello,
                    Oro = Soldati.Difesa.Oro * livello,
                    Popolazione = Soldati.Difesa.Popolazione * livello,
                    TempoRicerca = Soldati.Difesa.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Lancere Livello" => new ResearchCost
                {
                    Cibo = Soldati.Livello.Cibo * livello,
                    Legno = Soldati.Livello.Legno * livello,
                    Pietra = Soldati.Livello.Pietra * livello,
                    Ferro = Soldati.Livello.Ferro * livello,
                    Oro = Soldati.Livello.Oro * livello,
                    Popolazione = Soldati.Livello.Popolazione * livello,
                    TempoRicerca = Soldati.Livello.TempoRicerca * livello // più tempo per livelli più alti
                },

                "Arcere Salute" => new ResearchCost
                {
                    Cibo = Soldati.Salute.Cibo * livello,
                    Legno = Soldati.Salute.Legno * livello,
                    Pietra = Soldati.Salute.Pietra * livello,
                    Ferro = Soldati.Salute.Ferro * livello,
                    Oro = Soldati.Salute.Oro * livello,
                    Popolazione = Soldati.Salute.Popolazione * livello,
                    TempoRicerca = Soldati.Salute.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Arcere Attacco" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = Soldati.Attacco.Popolazione * livello,
                    TempoRicerca = Soldati.Attacco.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Arcere Difesa" => new ResearchCost
                {
                    Cibo = Soldati.Difesa.Cibo * livello,
                    Legno = Soldati.Difesa.Legno * livello,
                    Pietra = Soldati.Difesa.Pietra * livello,
                    Ferro = Soldati.Difesa.Ferro * livello,
                    Oro = Soldati.Difesa.Oro * livello,
                    Popolazione = Soldati.Difesa.Popolazione * livello,
                    TempoRicerca = Soldati.Difesa.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Arcere Livello" => new ResearchCost
                {
                    Cibo = Soldati.Livello.Cibo * livello,
                    Legno = Soldati.Livello.Legno * livello,
                    Pietra = Soldati.Livello.Pietra * livello,
                    Ferro = Soldati.Livello.Ferro * livello,
                    Oro = Soldati.Livello.Oro * livello,
                    Popolazione = Soldati.Livello.Popolazione * livello,
                    TempoRicerca = Soldati.Livello.TempoRicerca * livello // più tempo per livelli più alti
                },

                "Catapulta Salute" => new ResearchCost
                {
                    Cibo = Soldati.Salute.Cibo * livello,
                    Legno = Soldati.Salute.Legno * livello,
                    Pietra = Soldati.Salute.Pietra * livello,
                    Ferro = Soldati.Salute.Ferro * livello,
                    Oro = Soldati.Salute.Oro * livello,
                    Popolazione = Soldati.Salute.Popolazione * livello,
                    TempoRicerca = Soldati.Salute.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Catapulta Attacco" => new ResearchCost
                {
                    Cibo = Soldati.Attacco.Cibo * livello,
                    Legno = Soldati.Attacco.Legno * livello,
                    Pietra = Soldati.Attacco.Pietra * livello,
                    Ferro = Soldati.Attacco.Ferro * livello,
                    Oro = Soldati.Attacco.Oro * livello,
                    Popolazione = Soldati.Attacco.Popolazione * livello,
                    TempoRicerca = Soldati.Attacco.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Catapulta Difesa" => new ResearchCost
                {
                    Cibo = Soldati.Difesa.Cibo * livello,
                    Legno = Soldati.Difesa.Legno * livello,
                    Pietra = Soldati.Difesa.Pietra * livello,
                    Ferro = Soldati.Difesa.Ferro * livello,
                    Oro = Soldati.Difesa.Oro * livello,
                    Popolazione = Soldati.Difesa.Popolazione * livello,
                    TempoRicerca = Soldati.Difesa.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Catapulta Livello" => new ResearchCost
                {
                    Cibo = Soldati.Livello.Cibo * livello,
                    Legno = Soldati.Livello.Legno * livello,
                    Pietra = Soldati.Livello.Pietra * livello,
                    Ferro = Soldati.Livello.Ferro * livello,
                    Oro = Soldati.Livello.Oro * livello,
                    Popolazione = Soldati.Livello.Popolazione * livello,
                    TempoRicerca = Soldati.Livello.TempoRicerca * livello // più tempo per livelli più alti
                },

                "Ingresso Guarnigione" => new ResearchCost
                {
                    Cibo = Citta.Ingresso.Cibo * livello,
                    Legno = Citta.Ingresso.Legno * livello,
                    Pietra = Citta.Ingresso.Pietra * livello,
                    Ferro = Citta.Ingresso.Ferro * livello,
                    Oro = Citta.Ingresso.Oro * livello,
                    Popolazione = Citta.Ingresso.Popolazione * livello,
                    TempoRicerca = Citta.Ingresso.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Citta Guarnigione" => new ResearchCost
                {
                    Cibo = Citta.Città.Cibo * livello,
                    Legno = Citta.Città.Legno * livello,
                    Pietra = Citta.Città.Pietra * livello,
                    Ferro = Citta.Città.Ferro * livello,
                    Oro = Citta.Città.Oro * livello,
                    Popolazione = Citta.Città.Popolazione * livello,
                    TempoRicerca = Citta.Città.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Cancello Salute" => new ResearchCost
                {
                    Cibo = Citta.Cancello_Salute.Cibo * livello,
                    Legno = Citta.Cancello_Salute.Legno * livello,
                    Pietra = Citta.Cancello_Salute.Pietra * livello,
                    Ferro = Citta.Cancello_Salute.Ferro * livello,
                    Oro = Citta.Cancello_Salute.Oro * livello,
                    Popolazione = Citta.Cancello_Salute.Popolazione * livello,
                    TempoRicerca = Citta.Cancello_Salute.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Cancello Difesa" => new ResearchCost
                {
                    Cibo = Citta.Cancello_Difesa.Cibo * livello,
                    Legno = Citta.Cancello_Difesa.Legno * livello,
                    Pietra = Citta.Cancello_Difesa.Pietra * livello,
                    Ferro = Citta.Cancello_Difesa.Ferro * livello,
                    Oro = Citta.Cancello_Difesa.Oro * livello,
                    Popolazione = Citta.Cancello_Difesa.Popolazione * livello,
                    TempoRicerca = Citta.Cancello_Difesa.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Cancello Guarnigione" => new ResearchCost
                {
                    Cibo = Citta.Cancello_Guarnigione.Cibo * livello,
                    Legno = Citta.Cancello_Guarnigione.Legno * livello,
                    Pietra = Citta.Cancello_Guarnigione.Pietra * livello,
                    Ferro = Citta.Cancello_Guarnigione.Ferro * livello,
                    Oro = Citta.Cancello_Guarnigione.Oro * livello,
                    Popolazione = Citta.Cancello_Guarnigione.Popolazione * livello,
                    TempoRicerca = Citta.Cancello_Guarnigione.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Mura Salute" => new ResearchCost
                {
                    Cibo = Citta.Mura_Salute.Cibo * livello,
                    Legno = Citta.Mura_Salute.Legno * livello,
                    Pietra = Citta.Mura_Salute.Pietra * livello,
                    Ferro = Citta.Mura_Salute.Ferro * livello,
                    Oro = Citta.Mura_Salute.Oro * livello,
                    Popolazione = Citta.Mura_Salute.Popolazione * livello,
                    TempoRicerca = Citta.Mura_Salute.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Mura Difesa" => new ResearchCost
                {
                    Cibo = Citta.Mura_Difesa.Cibo * livello,
                    Legno = Citta.Mura_Difesa.Legno * livello,
                    Pietra = Citta.Mura_Difesa.Pietra * livello,
                    Ferro = Citta.Mura_Difesa.Ferro * livello,
                    Oro = Citta.Mura_Difesa.Oro * livello,
                    Popolazione = Citta.Mura_Difesa.Popolazione * livello,
                    TempoRicerca = Citta.Mura_Difesa.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Mura Guarnigione" => new ResearchCost
                {
                    Cibo = Citta.Mura_Guarnigione.Cibo * livello,
                    Legno = Citta.Mura_Guarnigione.Legno * livello,
                    Pietra = Citta.Mura_Guarnigione.Pietra * livello,
                    Ferro = Citta.Mura_Guarnigione.Ferro * livello,
                    Oro = Citta.Mura_Guarnigione.Oro * livello,
                    Popolazione = Citta.Mura_Guarnigione.Popolazione * livello,
                    TempoRicerca = Citta.Mura_Guarnigione.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Torri Salute" => new ResearchCost
                {
                    Cibo = Citta.Torri_Salute.Cibo * livello,
                    Legno = Citta.Torri_Salute.Legno * livello,
                    Pietra = Citta.Torri_Salute.Pietra * livello,
                    Ferro = Citta.Torri_Salute.Ferro * livello,
                    Oro = Citta.Torri_Salute.Oro * livello,
                    Popolazione = Citta.Torri_Salute.Popolazione * livello,
                    TempoRicerca = Citta.Torri_Salute.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Torri Difesa" => new ResearchCost
                {
                    Cibo = Citta.Torri_Difesa.Cibo * livello,
                    Legno = Citta.Torri_Difesa.Legno * livello,
                    Pietra = Citta.Torri_Difesa.Pietra * livello,
                    Ferro = Citta.Torri_Difesa.Ferro * livello,
                    Oro = Citta.Torri_Difesa.Oro * livello,
                    Popolazione = Citta.Torri_Difesa.Popolazione * livello,
                    TempoRicerca = Citta.Torri_Difesa.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Torri Guarnigione" => new ResearchCost
                {
                    Cibo = Citta.Torri_Guarnigione.Cibo * livello,
                    Legno = Citta.Torri_Guarnigione.Legno * livello,
                    Pietra = Citta.Torri_Guarnigione.Pietra * livello,
                    Ferro = Citta.Torri_Guarnigione.Ferro * livello,
                    Oro = Citta.Torri_Guarnigione.Oro * livello,
                    Popolazione = Citta.Torri_Guarnigione.Popolazione * livello,
                    TempoRicerca = Citta.Torri_Guarnigione.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Castello Salute" => new ResearchCost
                {
                    Cibo = Citta.Castello_Salute.Cibo * livello,
                    Legno = Citta.Castello_Salute.Legno * livello,
                    Pietra = Citta.Castello_Salute.Pietra * livello,
                    Ferro = Citta.Castello_Salute.Ferro * livello,
                    Oro = Citta.Castello_Salute.Oro * livello,
                    Popolazione = Citta.Castello_Salute.Popolazione * livello,
                    TempoRicerca = Citta.Castello_Salute.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Castello Difesa" => new ResearchCost
                {
                    Cibo = Citta.Castello_Difesa.Cibo * livello,
                    Legno = Citta.Castello_Difesa.Legno * livello,
                    Pietra = Citta.Castello_Difesa.Pietra * livello,
                    Ferro = Citta.Castello_Difesa.Ferro * livello,
                    Oro = Citta.Castello_Difesa.Oro * livello,
                    Popolazione = Citta.Castello_Difesa.Popolazione * livello,
                    TempoRicerca = Citta.Castello_Difesa.TempoRicerca * livello // più tempo per livelli più alti
                },
                "Castello Guarnigione" => new ResearchCost
                {
                    Cibo = Citta.Castello_Guarnigione.Cibo * livello,
                    Legno = Citta.Castello_Guarnigione.Legno * livello,
                    Pietra = Citta.Castello_Guarnigione.Pietra * livello,
                    Ferro = Citta.Castello_Guarnigione.Ferro * livello,
                    Oro = Citta.Castello_Guarnigione.Oro * livello,
                    Popolazione = Citta.Castello_Guarnigione.Popolazione * livello,
                    TempoRicerca = Citta.Castello_Guarnigione.TempoRicerca * livello // più tempo per livelli più alti
                },
                _ => null
            };
        }
        public static void UsaDiamantiPerVelocizzareRicerca(Guid clientGuid, Player player, int diamantiBluDaUsare)
        {
            if (diamantiBluDaUsare <= 0)
            {
                Server.Server.Send(clientGuid, "Log_Server|[error]Numero [blu][icon:diamanteBlu]diamanti [error]non valido.");
                return;
            }

            if (player.Diamanti_Blu < diamantiBluDaUsare)
            {
                Server.Server.Send(clientGuid, "Log_Server|[error]Non hai abbastanza [blu][icon:diamanteBlu]Diamanti Blu[error]!");
                return;
            }

            // Calcola il tempo totale rimanente
            double tempoTotaleRimanente = 0;
            if (player.currentTasks_Research != null)
            {
                foreach (var task in player.currentTasks_Research)
                    tempoTotaleRimanente += Math.Max(0, task.GetRemainingTime());
            }

            if (tempoTotaleRimanente <= 0)
            {
                Server.Server.Send(clientGuid, "Log_Server|[warning]Non ci sono ricerche da velocizzare.");
                return;
            }

            // Calcola riduzione e limita ai diamanti utili
            int riduzioneTotale = diamantiBluDaUsare * Variabili_Server.Velocizzazione_Tempo;
            int maxDiamantiUtili = (int)Math.Ceiling(tempoTotaleRimanente / Variabili_Server.Velocizzazione_Tempo);

            if (diamantiBluDaUsare > maxDiamantiUtili)
            {
                diamantiBluDaUsare = maxDiamantiUtili;
                riduzioneTotale = diamantiBluDaUsare * Variabili_Server.Velocizzazione_Tempo;
            }

            int riduzioneTotaleOriginale = riduzioneTotale;
            int riduzioneResidua = riduzioneTotale;

            // Processa le ricerche in corso (usa ToList per evitare modifiche durante iterazione)
            if (player.currentTasks_Research != null && player.currentTasks_Research.Count > 0)
            {
                var tasksList = player.currentTasks_Research.ToList();

                foreach (var task in tasksList)
                {
                    if (riduzioneResidua <= 0) break;

                    double rimanente = task.GetRemainingTime();
                    if (rimanente <= 0) continue;

                    if (riduzioneResidua >= rimanente)
                    {
                        int riduzione = (int)Math.Ceiling(rimanente);
                        task.ForzaCompletamento();
                        player.Tempo_Sottratto_Diamanti += riduzione;
                        player.Tempo_Ricerca += riduzione;
                        riduzioneResidua -= riduzione;
                    }
                    else
                    {
                        task.RiduciTempo(riduzioneResidua);
                        player.Tempo_Sottratto_Diamanti += riduzioneResidua;
                        player.Tempo_Ricerca += riduzioneResidua;
                        riduzioneResidua = 0;
                    }
                }
            }

            // Deduzione diamanti
            player.Diamanti_Blu -= diamantiBluDaUsare;
            player.Diamanti_Blu_Utilizzati += diamantiBluDaUsare;

            // Eventi
            OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", diamantiBluDaUsare);
            int tempoEffettivamenteRidotto = riduzioneTotaleOriginale - riduzioneResidua;
            OnEvent(player, QuestEventType.Velocizzazione, "Qualsiasi", tempoEffettivamenteRidotto);

            Server.Server.Send(clientGuid, $"Log_Server|[title]Hai usato [icon:diamanteBlu][warning]{diamantiBluDaUsare} [blu]Diamanti Blu [title]per velocizzare la ricerca! [icon:tempo]{player.FormatTime(tempoEffettivamenteRidotto)}");
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
        public class ResearchTask // Classe privata per rappresentare un task di reclutamento
        {
            public string Type { get; }
            public int DurationInSeconds { get; private set; } // durata totale (aggiornabile)
            private DateTime startTime;
            private bool forceComplete = false;

            // gestione pausa
            public bool IsPaused { get; private set; } = false;
            private double pausedRemainingSeconds = 0;

            public ResearchTask(string type, int durationInSeconds)
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
