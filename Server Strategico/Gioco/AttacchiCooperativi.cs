using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Server.Server;

namespace Server_Strategico.Gioco
{
    public class AttacchiCooperativi
    {
        // Dizionario che contiene tutti gli attacchi cooperativi in corso
        public static Dictionary<string, AttaccoCooperativo> AttacchiInCorso = new Dictionary<string, AttaccoCooperativo>();
        public static Dictionary<string, AttaccoCooperativo> AttacchiInPlayer = new Dictionary<string, AttaccoCooperativo>();

        // Classe per rappresentare un singolo attacco cooperativo
        public class AttaccoCooperativo
        {
            public string IdAttacco { get; private set; }
            public Dictionary<string, TruppeContribuite> GiocatoriPartecipanti { get; private set; }
            public DateTime OraPrevista { get; set; }
            public bool AttaccoInCorso { get; set; }
            public int TempoRimanente { get; set; }
            public string CreatoreUsername { get; private set; }
            
            public AttaccoCooperativo(string id, string creatore)
            {
                IdAttacco = id;
                CreatoreUsername = creatore;
                GiocatoriPartecipanti = new Dictionary<string, TruppeContribuite>();
                OraPrevista = DateTime.Now.AddMinutes(30); // Default 30 minuti
                AttaccoInCorso = false;
                TempoRimanente = 1800; // 30 minuti in secondi
            }

            public bool AggiungiGiocatore(string username, TruppeContribuite truppe)
            {
                if (AttaccoInCorso)
                    return false;
                
                if (GiocatoriPartecipanti.ContainsKey(username))
                {
                    var truppeEsistenti = GiocatoriPartecipanti[username];
                    truppeEsistenti.Player = username;
                    truppeEsistenti.Guerrieri[0] += truppe.Guerrieri[0];
                    truppeEsistenti.Lanceri[0] += truppe.Lanceri[0];
                    truppeEsistenti.Arceri[0] += truppe.Arceri[0];
                    truppeEsistenti.Catapulte[0] += truppe.Catapulte[0];
                }
                else
                    GiocatoriPartecipanti.Add(username, truppe);
                
                return true;
            }

            public bool RimuoviGiocatore(string username)
            {
                if (AttaccoInCorso)
                    return false;
                
                return GiocatoriPartecipanti.Remove(username);
            }
        }

        // Classe per memorizzare le truppe che un giocatore contribuisce
        public class TruppeContribuite
        {
            public string Player { get; set; }
            // Esercito
            public int[] Guerrieri = new int[5];
            public int[] Lanceri = new int[5];
            public int[] Arceri = new int[5];
            public int[] Catapulte = new int[5];

            public TruppeContribuite(int[] guerrieri, int[] lanceri, int[] arceri, int[] catapulte, string username)
            {
                Player = username;
                Guerrieri[0] = guerrieri[0];
                Lanceri[0] = lanceri[0];
                Arceri[0] = arceri[0];
                Catapulte[0] = catapulte[0];
            }
        }

        // Metodo per creare un nuovo attacco cooperativo
        public static string CreaAttaccoCooperativo(string username)
        {
            string idAttacco = Guid.NewGuid().ToString().Substring(0, 8);
            var nuovoAttacco = new AttaccoCooperativo(idAttacco, username);
            AttacchiInCorso.Add(idAttacco, nuovoAttacco);
            AttacchiInPlayer.Add(idAttacco, nuovoAttacco);
            return idAttacco;
        }

        // Metodo per partecipare a un attacco cooperativo
        public static bool PartecipaDiAttacco(string idAttacco, string username, int guerrieri, int lancieri, int arcieri, int catapulte, Guid clientGuid)
        {
            if (!AttacchiInCorso.ContainsKey(idAttacco))
            {
                Send(clientGuid, $"Log_Server|Attacco con ID {idAttacco} non trovato.");
                return false;
            }
            
            var player = servers_.GetPlayer_Data(username);
            if (player == null)
            {
                Send(clientGuid, $"Log_Server|Giocatore non trovato.");
                return false;
            }
            
            // Verifico che i valori non siano negativi
            if (guerrieri < 0 || lancieri < 0 || arcieri < 0 || catapulte < 0)
            {
                Send(clientGuid, $"Log_Server|Non puoi inviare un numero negativo di truppe.");
                return false;
            }
            
            // Verifico che venga inviata almeno una truppa
            if (guerrieri == 0 && lancieri == 0 && arcieri == 0 && catapulte == 0)
            {
                Send(clientGuid, $"Log_Server|Devi inviare almeno una truppa.");
                return false;
            }
            
            // Verifica disponibilità truppe (controllo importante)
            if (player.Guerrieri[0] < guerrieri || player.Lanceri[0] < lancieri || 
                player.Arceri[0] < arcieri || player.Catapulte[0] < catapulte)
            {
                Send(clientGuid, $"Log_Server|Non hai abbastanza truppe disponibili.");
                return false;
            }
            int[] Guerrieri = { 0, 0, 0, 0, 0 };
            int[] Lanceri = { 0, 0, 0, 0, 0 };
            int[] Arceri = { 0, 0, 0, 0, 0 };
            int[] Catapulte = { 0, 0, 0, 0, 0 };
            //Assegnavo il valore corretto di unità
            Guerrieri[0] = guerrieri;
            Lanceri[0] = lancieri;
            Arceri[0] = arcieri;
            Catapulte[0] = catapulte;

            // Crea l'oggetto globale truppe per aggiunegre le unità dei partecipanti
            var truppe = new TruppeContribuite(Guerrieri, Lanceri, Arceri, Catapulte, player.Username);
            AttacchiInCorso[idAttacco].AggiungiGiocatore(username, truppe); // Aggiungi le truppe all'attacco (con il metodo corretto che abbiamo già sistemato)

            // IMPORTANTE: Rimuovi le truppe dal giocatore DOPO aver confermato che l'aggiunta è avvenuta con successo
            player.Guerrieri[0] -= guerrieri;
            player.Lanceri[0] -= lancieri;
            player.Arceri[0] -= arcieri;
            player.Catapulte[0] -= catapulte;
            
            // Calcola e invia informazioni sulle truppe totali dell'attacco
            var attacco = AttacchiInCorso[idAttacco];
            int totGuerrieri = 0, totLancieri = 0, totArcieri = 0, totCatapulte = 0;
            
            foreach (var part in attacco.GiocatoriPartecipanti)
            {
                totGuerrieri += part.Value.Guerrieri[0];
                totLancieri += part.Value.Lanceri[0];
                totArcieri += part.Value.Arceri[0];
                totCatapulte += part.Value.Catapulte[0];
            }
            
            // Invia conferma al giocatore
            Send(clientGuid, $"Log_Server|Hai contribuito all'attacco #{idAttacco} con: {guerrieri} Guerrieri, {lancieri} Lancieri, {arcieri} Arcieri, {catapulte} Catapulte.");
            Send(clientGuid, $"Log_Server|Forze totali: {totGuerrieri} Guerrieri, {totLancieri} Lancieri, {totArcieri} Arcieri, {totCatapulte} Catapulte.");
            
            // Invia il messaggio di RadunoPartecipo per aggiornare l'interfaccia client
            Send(clientGuid, $"RadunoPartecipo|{attacco.CreatoreUsername}|{idAttacco}|{attacco.GiocatoriPartecipanti.Count}|{guerrieri}|{lancieri}|{arcieri}|{catapulte}|{attacco.TempoRimanente / 60}");

            foreach (var partecipante in attacco.GiocatoriPartecipanti) // Notifica tutti i partecipanti dell'aggiornamento
            {
                var giocatore = servers_.GetPlayer_Data(partecipante.Key);
                if (giocatore != null && giocatore.guid_Player != Guid.Empty && giocatore.guid_Player != clientGuid)
                {
                    Send(giocatore.guid_Player, $"Log_Server|{player.Username} ha inviato truppe all'attacco #{idAttacco}: {guerrieri} Guerrieri, {lancieri} Lancieri, {arcieri} Arcieri, {catapulte} Catapulte.");
                    Send(giocatore.guid_Player, $"Log_Server|Forze totali: {totGuerrieri} Guerrieri, {totLancieri} Lancieri, {totArcieri} Arcieri, {totCatapulte} Catapulte.");
                }
            }
            return true;
        }

        // Abbandona un attacco cooperativo e recupera le truppe
        public static bool AbbandoaDiAttacco(string idAttacco, string username, Guid clientGuid)
        {
            if (!AttacchiInCorso.ContainsKey(idAttacco))
            {
                Send(clientGuid, $"Log_Server|Attacco con ID {idAttacco} non trovato.");
                return false;
            }
            
            var attacco = AttacchiInCorso[idAttacco];
            if (!attacco.GiocatoriPartecipanti.ContainsKey(username))
            {
                Send(clientGuid, $"Log_Server|Non stai partecipando a questo attacco.");
                return false;
            }
            
            if (attacco.AttaccoInCorso)
            {
                Send(clientGuid, $"Log_Server|Non puoi abbandonare un attacco già iniziato.");
                return false;
            }
            
            // Recupera le truppe contribuite
            var truppeContribuite = attacco.GiocatoriPartecipanti[username];
            var player = servers_.GetPlayer_Data(username);
            
            if (player != null)
            {
                player.Guerrieri[0] += truppeContribuite.Guerrieri[0];
                player.Lanceri[0] += truppeContribuite.Lanceri[0];
                player.Arceri[0] += truppeContribuite.Arceri[0];
                player.Catapulte[0] += truppeContribuite.Catapulte[0];
            }
            
            // Rimuovi il giocatore dall'attacco
            attacco.RimuoviGiocatore(username);
            
            Send(clientGuid, $"Log_Server|Hai abbandonato l'attacco #{idAttacco} e recuperato le tue truppe.");
            
            // Se non ci sono più partecipanti, rimuovi l'attacco
            if (attacco.GiocatoriPartecipanti.Count == 0)
            {
                AttacchiInCorso.Remove(idAttacco);
                Send(clientGuid, $"Log_Server|L'attacco #{idAttacco} è stato cancellato perché non ci sono più partecipanti.");
            }
            else
            {
                // Notifica gli altri partecipanti
                foreach (var partecipante in attacco.GiocatoriPartecipanti)
                {
                    var giocatore = servers_.GetPlayer_Data(partecipante.Key);
                    if (giocatore != null && giocatore.guid_Player != Guid.Empty)
                    {
                        Send(giocatore.guid_Player, $"Log_Server|{player.Username} ha abbandonato l'attacco #{idAttacco}.");
                    }
                }
            }
            
            return true;
        }

        // Ottieni la lista degli attacchi cooperativi disponibili
        public static void GetListaAttacchi(Guid clientGuid)
        {
            if (AttacchiInCorso.Count == 0)
            {
                Send(clientGuid, $"Log_Server|Non ci sono attacchi cooperativi in preparazione.");
                return;
            }
            
            Send(clientGuid, $"Log_Server|Attacchi cooperativi in preparazione:");
            foreach (var attaccoInfo in AttacchiInCorso)
            {
                int gTot = 0, lTot = 0, aTot = 0, cTot = 0;
                foreach (var part in attaccoInfo.Value.GiocatoriPartecipanti)
                {
                    gTot += part.Value.Guerrieri[0];
                    lTot += part.Value.Lanceri[0];
                    aTot += part.Value.Arceri[0];
                    cTot += part.Value.Catapulte[0];
                }
                
                Send(clientGuid, $"Log_Server|ID: {attaccoInfo.Key} - Partecipanti: {attaccoInfo.Value.GiocatoriPartecipanti.Count} - Truppe: G:{gTot}, L:{lTot}, A:{aTot}, C:{cTot} - Tempo: {attaccoInfo.Value.TempoRimanente/60} min");
            }
        }

        // Inizia un attacco cooperativo
        public static async Task<bool> IniziaAttaccoCooperativo(string idAttacco, Guid clientGuid)
        {
            if (!AttacchiInCorso.ContainsKey(idAttacco))
            {
                Send(clientGuid, $"Log_Server|Attacco con ID {idAttacco} non trovato.");
                return false;
            }
            
            var attacco = AttacchiInCorso[idAttacco];
            
            // Recupera il nome utente dal clientGuid
            string username = null;
            foreach (var player in servers_.GetAllPlayers())
            {
                if (player.guid_Player == clientGuid)
                {
                    username = player.Username;
                    break;
                }
            }
            
            // Verifica se l'utente è il creatore
            if (username == null || username != attacco.CreatoreUsername)
            {
                Send(clientGuid, $"Log_Server|Solo il creatore dell'attacco ({attacco.CreatoreUsername}) può avviarlo.");
                return false;
            }
            
            if (attacco.GiocatoriPartecipanti.Count == 0)
            {
                Send(clientGuid, $"Log_Server|L'attacco non può iniziare perché non ci sono partecipanti.");
                return false;
            }
            
            if (attacco.AttaccoInCorso)
            {
                Send(clientGuid, $"Log_Server|L'attacco è già in corso.");
                return false;
            }
            
            attacco.AttaccoInCorso = true;
            
            // Notifica tutti i partecipanti che l'attacco sta iniziando
            foreach (var partecipante in attacco.GiocatoriPartecipanti)
            {
                var giocatore = servers_.GetPlayer_Data(partecipante.Key);
                if (giocatore != null && giocatore.guid_Player != Guid.Empty)
                {
                    Send(giocatore.guid_Player, $"Log_Server|L'attacco cooperativo #{idAttacco} sta iniziando!");
                }
            }
            
            // Esegui la battaglia
            await EseguiBattagliaCooperativa(idAttacco, clientGuid);
            
            return true;
        }

        // Esegui la battaglia cooperativa contro i barbari PVP
        private static async Task EseguiBattagliaCooperativa(string idAttacco, Guid clientGuid)
        {
            var attacco = AttacchiInCorso[idAttacco];

            // Calcola il totale delle truppe che partecipano all'attacco
            int[] totaleGuerrieri = new int[5];
            int[] totaleLancieri = new int[5];
            int[] totaleArcieri = new int[5];
            int[] totaleCatapulte = new int[5];
            int totaleFrecce = 0; // rimane int se non hai più livelli per le frecce

            foreach (var partecipante in attacco.GiocatoriPartecipanti)
            {
                var player = servers_.GetPlayer_Data(partecipante.Key);
                if (player != null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        totaleGuerrieri[i] += partecipante.Value.Guerrieri[i];
                        totaleLancieri[i] += partecipante.Value.Lanceri[i];
                        totaleArcieri[i] += partecipante.Value.Arceri[i];
                        totaleCatapulte[i] += partecipante.Value.Catapulte[i];
                    }

                    totaleFrecce += (int)player.Frecce;
                }
            }

            // Crea un "giocatore virtuale" con array di truppe
            var playerVirtuale = new Player("Cooperativo", "password", Guid.Empty)
            {
                Guerrieri = totaleGuerrieri,
                Lanceri = totaleLancieri,
                Arceri = totaleArcieri,
                Catapulte = totaleCatapulte,
                Frecce = totaleFrecce
            };

            await Battaglie.Battaglia_Distanza("Barbari_PVP", playerVirtuale, clientGuid); //Pre battaglia, attaccano le unità a distanza ed i mezzi d'assedio

            // Eseguire la battaglia contro i barbari
            int guerrieri = playerVirtuale.Guerrieri[0];
            int picchieri = playerVirtuale.Lanceri[0];
            int arcieri = playerVirtuale.Arceri[0];
            int catapulte = playerVirtuale.Catapulte[0];

            int guerrieri_Enemy = Giocatori.Barbari.PVP.Guerrieri;
            int picchieri_Enemy = Giocatori.Barbari.PVP.Lancieri;
            int arcieri_Enemy = Giocatori.Barbari.PVP.Arceri;
            int catapulte_Enemy = Giocatori.Barbari.PVP.Catapulte;

            int tipi_Di_Unità = Battaglie.ContareTipiDiUnità(guerrieri, picchieri, arcieri, catapulte);
            int tipi_Di_Unità_Att = Battaglie.ContareTipiDiUnità(guerrieri_Enemy, picchieri_Enemy, arcieri_Enemy, catapulte_Enemy);

            // Calcolo del danno per il giocatore e il nemico
            double dannoInflittoDalNemico = Battaglie.CalcolareDanno_Invasore(arcieri_Enemy, catapulte_Enemy, guerrieri_Enemy, picchieri_Enemy, playerVirtuale) / tipi_Di_Unità;
            double dannoInflitto = Battaglie.CalcolareDanno_Giocatore(arcieri, catapulte, guerrieri, picchieri, playerVirtuale, clientGuid) / tipi_Di_Unità_Att;

            // Applicare il danno alle unità del giocatore
            int guerrieri_Temp = Battaglie.RidurreNumeroSoldati(guerrieri, dannoInflittoDalNemico, Esercito.Unità.Guerrieri_1.Difesa * guerrieri, Esercito.Unità.Guerrieri_1.Salute);
            int picchieri_Temp = Battaglie.RidurreNumeroSoldati(picchieri, dannoInflittoDalNemico, Esercito.Unità.Lanceri_1.Difesa * picchieri, Esercito.Unità.Lanceri_1.Salute);
            int arcieri_Temp = Battaglie.RidurreNumeroSoldati(arcieri, dannoInflittoDalNemico * 0.70, Esercito.Unità.Arceri_1.Difesa * arcieri, Esercito.Unità.Arceri_1.Salute);
            int catapulte_Temp = Battaglie.RidurreNumeroSoldati(catapulte, dannoInflittoDalNemico, Esercito.Unità.Catapulte_1.Difesa * catapulte, Esercito.Unità.Catapulte_1.Salute);

            // Applicare il danno alle unità nemiche
            int guerrieri_Enemy_Temp = Battaglie.RidurreNumeroSoldati(guerrieri_Enemy, dannoInflitto, Esercito.EsercitoNemico.Guerrieri_1.Difesa * guerrieri_Enemy, Esercito.EsercitoNemico.Guerrieri_1.Salute);
            int picchieri_Enemy_Temp = Battaglie.RidurreNumeroSoldati(picchieri_Enemy, dannoInflitto, Esercito.EsercitoNemico.Lanceri_1.Difesa * picchieri_Enemy, Esercito.EsercitoNemico.Lanceri_1.Salute);
            int arcieri_Enemy_Temp = Battaglie.RidurreNumeroSoldati(arcieri_Enemy, dannoInflitto, Esercito.EsercitoNemico.Arceri_1.Difesa * arcieri_Enemy, Esercito.EsercitoNemico.Arceri_1.Salute);
            int catapulte_Enemy_Temp = Battaglie.RidurreNumeroSoldati(catapulte_Enemy, dannoInflitto, Esercito.EsercitoNemico.Catapulte_1.Difesa * catapulte_Enemy, Esercito.EsercitoNemico.Catapulte_1.Salute);
            
            // Calcola le perdite totali
            int guerrieriPersi = guerrieri_Temp;
            int picchieriPersi = picchieri_Temp;
            int arcieriPersi = arcieri_Temp;
            int catapultePersi = catapulte_Temp;
            double frecceConsumate = totaleFrecce - playerVirtuale.Frecce;
            
            // Calcola le perdite dei barbari
            int guerrieri_Enemy_Persi = guerrieri_Enemy_Temp;
            int picchieri_Enemy_Persi = picchieri_Enemy_Temp;
            int arcieri_Enemy_Persi = arcieri_Enemy_Temp;
            int catapulte_Enemy_Persi = catapulte_Enemy_Temp;
            
            // Invia i risultati a tutti i partecipanti
            foreach (var partecipante in attacco.GiocatoriPartecipanti)
            {
                var giocatore = servers_.GetPlayer_Data(partecipante.Key);
                if (giocatore != null && giocatore.guid_Player != Guid.Empty)
                {
                    Send(giocatore.guid_Player, $"Log_Server|Danno subito: {(dannoInflittoDalNemico * tipi_Di_Unità).ToString("0.00")}");
                    Send(giocatore.guid_Player, $"Log_Server|Danno inflitto: {(dannoInflitto * tipi_Di_Unità_Att).ToString("0.00")}");
                    Send(giocatore.guid_Player, $"Log_Server|Risultato dell'attacco cooperativo #{idAttacco}:");
                    
                    Send(giocatore.guid_Player, $"Log_Server|Guerrieri: {guerrieriPersi}\r\n Lancieri: {picchieriPersi}\r\n Arcieri: {arcieriPersi}\r\n Catapulte: {catapultePersi}\r\n");
                    Send(giocatore.guid_Player, $"Log_Server|Truppe perse in totale:");
                    
                    Send(giocatore.guid_Player, $"Log_Server|Guerrieri: {guerrieri_Enemy_Persi}\r\n Lancieri: {picchieri_Enemy_Persi}\r\n Arcieri: {arcieri_Enemy_Persi}\r\n Catapulte: {catapulte_Enemy_Persi}\r\n");
                    Send(giocatore.guid_Player, $"Log_Server|Truppe perse dai barbari:");

                    // Calcolo delle perdite proporzionali per questo giocatore
                    double rapportoGuerrieri = totaleGuerrieri.Sum() > 0 ? (double)partecipante.Value.Guerrieri.Sum() / totaleGuerrieri.Sum() : 0;
                    double rapportoLancieri = totaleLancieri.Sum() > 0 ? (double)partecipante.Value.Lanceri.Sum() / totaleLancieri.Sum() : 0;
                    double rapportoArcieri = totaleArcieri.Sum() > 0 ? (double)partecipante.Value.Arceri.Sum() / totaleArcieri.Sum() : 0;
                    double rapportoCatapulte = totaleCatapulte.Sum() > 0 ? (double)partecipante.Value.Catapulte.Sum() / totaleCatapulte.Sum() : 0;
                    double rapportoFrecce = totaleFrecce > 0 ? (double)playerVirtuale.Frecce / totaleFrecce : 0;

                    // Calcolo perdite per ogni tipo di unità
                    int guerrieriPersiGiocatore = (int)Math.Ceiling(rapportoGuerrieri * guerrieriPersi);
                    int lancieriPersiGiocatore = (int)Math.Ceiling(rapportoLancieri * picchieriPersi);
                    int arcieriPersiGiocatore = (int)Math.Ceiling(rapportoArcieri * arcieriPersi);
                    int catapultePersiGiocatore = (int)Math.Ceiling(rapportoCatapulte * catapultePersi);
                    int freccePersiGiocatore = (int)Math.Ceiling(rapportoFrecce * frecceConsumate);

                    // Calcola l'esperienza in base al contributo
                    double contributo = (rapportoGuerrieri + rapportoLancieri + rapportoArcieri + rapportoCatapulte) / 4.0;
                    int esperienzaGuadagnata = (int)(contributo * (
                        guerrieri_Enemy_Persi * Esercito.EsercitoNemico.Guerrieri_1.Esperienza +
                        picchieri_Enemy_Persi * Esercito.EsercitoNemico.Lanceri_1.Esperienza +
                        arcieri_Enemy_Persi * Esercito.EsercitoNemico.Arceri_1.Esperienza +
                        catapulte_Enemy_Persi * Esercito.EsercitoNemico.Catapulte_1.Esperienza
                    ));
                    
                    giocatore.Esperienza += esperienzaGuadagnata;
                    
                    // Restituisci le truppe sopravvissute al giocatore
                    int guerrieriRestituiti = partecipante.Value.Guerrieri[0] - guerrieriPersiGiocatore;
                    int lancieriRestituiti = partecipante.Value.Lanceri[0] - lancieriPersiGiocatore;
                    int arcieriRestituiti = partecipante.Value.Arceri[0] - arcieriPersiGiocatore;
                    int catapulteRestituite = partecipante.Value.Catapulte[0] - catapultePersiGiocatore;
                    double frecceRestituite = giocatore.Frecce - freccePersiGiocatore;

                    giocatore.Guerrieri[0] += guerrieriRestituiti;
                    giocatore.Lanceri[0] += lancieriRestituiti;
                    giocatore.Arceri[0] += arcieriRestituiti;
                    giocatore.Catapulte[0] += catapulteRestituite;
                    giocatore.Frecce += frecceRestituite;
                    
                    Send(giocatore.guid_Player, $"Log_Server|Truppe restituite: Guerrieri: {guerrieriRestituiti}, Lancieri: {lancieriRestituiti}, Arcieri: {arcieriRestituiti}, Catapulte: {catapulteRestituite}");
                    Send(giocatore.guid_Player, $"Log_Server|Esperienza guadagnata: {esperienzaGuadagnata}");
                    Send(giocatore.guid_Player, $"Log_Server|Guerrieri: {guerrieriPersiGiocatore}\r\n Lancieri: {lancieriPersiGiocatore}\r\n Arcieri: {arcieriPersiGiocatore}\r\n Catapulte: {catapultePersiGiocatore}\r\n");
                    Send(giocatore.guid_Player, $"Log_Server|Le tue perdite personali: [{giocatore.Username}]");
                }
            }
            
            // Aggiorna i barbari PVP
            Giocatori.Barbari.PVP.Guerrieri = guerrieri_Enemy - guerrieri_Enemy_Temp;
            Giocatori.Barbari.PVP.Lancieri = picchieri_Enemy - picchieri_Enemy_Temp;
            Giocatori.Barbari.PVP.Arceri = arcieri_Enemy - arcieri_Enemy_Temp;
            Giocatori.Barbari.PVP.Catapulte = catapulte_Enemy - catapulte_Enemy_Temp;
            
            // Rimuovi l'attacco dalla lista
            AttacchiInCorso.Remove(idAttacco);
            AttacchiInPlayer.Remove(idAttacco);
        }

        // Aggiorna lo stato di tutti gli attacchi (da chiamare nel ciclo di gioco)
        public static void AggiornaAttacchi()
        {
            foreach (var attaccoCooperativo in AttacchiInCorso.Values.ToList())
            {
                if (!attaccoCooperativo.AttaccoInCorso)
                {
                    attaccoCooperativo.TempoRimanente--;

                    // Se il tempo è scaduto, cancella l'attacco
                    if (attaccoCooperativo.TempoRimanente <= 0)
                    {
                        foreach (var partecipante in attaccoCooperativo.GiocatoriPartecipanti)
                        {
                            var giocatore = servers_.GetPlayer_Data(partecipante.Key);
                            if (giocatore != null)
                            {
                                // Restituisci truppe per ogni livello
                                for (int i = 0; i < 5; i++)
                                {
                                    giocatore.Guerrieri[i] += partecipante.Value.Guerrieri[i];
                                    giocatore.Lanceri[i] += partecipante.Value.Lanceri[i];
                                    giocatore.Arceri[i] += partecipante.Value.Arceri[i];
                                    giocatore.Catapulte[i] += partecipante.Value.Catapulte[i];
                                }

                                if (giocatore.guid_Player != Guid.Empty)
                                {
                                    Send(giocatore.guid_Player, $"Log_Server|L'attacco cooperativo #{attaccoCooperativo.IdAttacco} è stato cancellato perché il tempo è scaduto.");
                                    Send(giocatore.guid_Player, $"Log_Server|Le tue truppe sono state restituite:");

                                    // Log dettagliato per livello
                                    for (int livello = 0; livello < 5; livello++)
                                    {
                                        Send(giocatore.guid_Player,
                                            $"Livello {livello + 1}: Guerrieri={partecipante.Value.Guerrieri[livello]}, " +
                                            $"Lanceri={partecipante.Value.Lanceri[livello]}, " +
                                            $"Arcieri={partecipante.Value.Arceri[livello]}, " +
                                            $"Catapulte={partecipante.Value.Catapulte[livello]}");
                                    }
                                }
                            }
                        }

                        // Rimuovi l'attacco dalla lista
                        AttacchiInCorso.Remove(attaccoCooperativo.IdAttacco);
                    }
                    // Aggiorna i partecipanti ogni minuto
                    else if (attaccoCooperativo.TempoRimanente % 60 == 0)
                    {
                        foreach (var partecipante in attaccoCooperativo.GiocatoriPartecipanti)
                        {
                            var giocatore = servers_.GetPlayer_Data(partecipante.Key);
                            if (giocatore != null && giocatore.guid_Player != Guid.Empty)
                            {
                                Send(giocatore.guid_Player, $"Log_Server|Tempo rimanente per l'attacco cooperativo #{attaccoCooperativo.IdAttacco}: {attaccoCooperativo.TempoRimanente / 60} minuti.");
                            }
                        }
                    }
                }
            }
        }

        // Gestisci i comandi dell'attacco cooperativo
        public static async Task GestisciComando(string[] msgArgs, Guid clientGuid, Player player)
        {
            if (msgArgs.Length < 4)
            {
                Send(clientGuid, $"Log_Server|Comando non valido. Usa: AttaccoCooperativo|<azione>|<parametri>");
                return;
            }

            switch (msgArgs[3])
            {
                case "Crea":
                    string idNuovoAttacco = CreaAttaccoCooperativo(player.Username);
                    Send(clientGuid, $"Log_Server|Nuovo attacco cooperativo creato! ID: {idNuovoAttacco}");
                    Send(clientGuid, $"AttaccoCooperativo|Creato|{idNuovoAttacco}");
                    break;
                    
                case "Partecipa":
                    if (msgArgs.Length < 9)
                    {
                        Send(clientGuid, $"Log_Server|Parametri insufficienti. Usa: AttaccoCooperativo|Partecipa|<idAttacco>|<guerrieri>|<lancieri>|<arcieri>|<catapulte>");
                        return;
                    }
                    
                    string idAttacco = msgArgs[4];
                    int guerrieri = Convert.ToInt32(msgArgs[5]);
                    int lancieri = Convert.ToInt32(msgArgs[6]);
                    int arcieri = Convert.ToInt32(msgArgs[7]);
                    int catapulte = Convert.ToInt32(msgArgs[8]);
                    
                    await Task.Run(() => PartecipaDiAttacco(idAttacco, player.Username, guerrieri, lancieri, arcieri, catapulte, clientGuid));
                    break;
                    
                case "Abbandona":
                    if (msgArgs.Length < 5)
                    {
                        Send(clientGuid, $"Log_Server|Parametri insufficienti. Usa: AttaccoCooperativo|Abbandona|<idAttacco>");
                        return;
                    }
                    
                    string idAttaccoAbbandona = msgArgs[4];
                    await Task.Run(() => AbbandoaDiAttacco(idAttaccoAbbandona, player.Username, clientGuid));
                    break;
                    
                case "Inizia":
                    if (msgArgs.Length < 5)
                    {
                        Send(clientGuid, $"Log_Server|Parametri insufficienti. Usa: AttaccoCooperativo|Inizia|<idAttacco>");
                        return;
                    }
                    
                    string idAttaccoInizio = msgArgs[4];
                    await IniziaAttaccoCooperativo(idAttaccoInizio, clientGuid);
                    break;
                    
                case "Lista":
                    GetListaAttacchi(clientGuid);
                    break;
                    
                case "MieiAttacchi":
                    GetMieiAttacchi(player.Username, clientGuid);
                    break;
                    
                default:
                    Send(clientGuid, $"Log_Server|Azione non riconosciuta. Azioni disponibili: Crea, Partecipa, Abbandona, Inizia, Lista, MieiAttacchi");
                    break;
            }
        }

        // Ottieni le partecipazioni di un giocatore agli attacchi cooperativi
        public static void GetMieiAttacchi(string username, Guid clientGuid)
        {
            bool partecipazioniTrovate = false;
            int totaleAttacchi = 0;
            
            // Contatori per il totale delle truppe impegnate
            int totGuerrieri = 0;
            int totLancieri = 0;
            int totArcieri = 0;
            int totCatapulte = 0;
            
            Send(clientGuid, $"Log_Server|Le tue partecipazioni ai raduni:\n\r");
            
            foreach (var attacco in AttacchiInCorso)
                if (attacco.Value.GiocatoriPartecipanti.ContainsKey(username))
                {
                    partecipazioniTrovate = true;
                    totaleAttacchi++;
                    
                    var truppe = attacco.Value.GiocatoriPartecipanti[username];
                    totGuerrieri += truppe.Guerrieri[0];
                    totLancieri += truppe.Lanceri[0];
                    totArcieri += truppe.Arceri[0];
                    totCatapulte += truppe.Catapulte[0];
                    
                    // Calcola il tempo rimanente in formato leggibile
                    int minuti = attacco.Value.TempoRimanente / 60;
                    int secondi = attacco.Value.TempoRimanente % 60;
                    
                    // Informazioni sull'attacco
                    Send(clientGuid, $"Log_Server|ID: {attacco.Key} - Creato da: {attacco.Value.CreatoreUsername}");
                    Send(clientGuid, $"Log_Server|Le tue truppe: G:{truppe.Guerrieri}, L:{truppe.Lanceri}, A:{truppe.Arceri}, C:{truppe.Catapulte}");
                    
                    // Calcola totale truppe per questo attacco
                    int attaccoTotG = 0, attaccoTotL = 0, attaccoTotA = 0, attaccoTotC = 0;
                    foreach (var part in attacco.Value.GiocatoriPartecipanti)
                    {
                        attaccoTotG += part.Value.Guerrieri[0];
                        attaccoTotL += part.Value.Lanceri[0];
                        attaccoTotA += part.Value.Arceri[0];
                        attaccoTotC += part.Value.Catapulte[0];
                    }
                    
                    Send(clientGuid, $"Log_Server|Truppe totali: G:{attaccoTotG}, L:{attaccoTotL}, A:{attaccoTotA}, C:{attaccoTotC}");
                    Send(clientGuid, $"Log_Server|Partecipanti: {attacco.Value.GiocatoriPartecipanti.Count} - Tempo rimanente: {minuti}m {secondi}s");
                    Send(clientGuid, $"Log_Server|-------------------------------------------");
                    //Send(clientGuid, $"RadunoPartecipo|{attacco.Value.CreatoreUsername}|{attacco.Key}|{attacco.GiocatoriPartecipanti.Count}|{totG}|{totL}|{totA}|{totC}|{}");

                }
            
            if (partecipazioniTrovate)
            {
                // Invia anche un riepilogo generale
                Send(clientGuid, $"Log_Server|RIEPILOGO:");
                Send(clientGuid, $"Log_Server|Partecipazione a {totaleAttacchi} raduni");
                Send(clientGuid, $"Log_Server|Totale truppe impegnate: G:{totGuerrieri}, L:{totLancieri}, A:{totArcieri}, C:{totCatapulte}");
                
                // Invia anche i dati in formato strutturato per l'interfaccia
                Send(clientGuid, $"AttacchiCooperativi|MieiAttacchi|{totaleAttacchi}|{totGuerrieri}|{totLancieri}|{totArcieri}|{totCatapulte}");
            }
            else
            {
                Send(clientGuid, $"Log_Server|Non stai partecipando a nessun raduno.");
                Send(clientGuid, $"AttacchiCooperativi|MieiAttacchi|0|0|0|0|0");
            }
        }

        // Metodo helper per ottenere tutti gli attacchi con un determinato giocatore
        public static List<AttaccoCooperativo> GetAttacchiConGiocatore(string username)
        {
            var risultato = new List<AttaccoCooperativo>();
            
            foreach (var attacco in AttacchiInCorso.Values)
            {
                if (attacco.GiocatoriPartecipanti.ContainsKey(username))
                {
                    risultato.Add(attacco);
                }
            }
            
            return risultato;
        }
    }
} 