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

                Guerrieri[1] = guerrieri[1];
                Lanceri[1] = lanceri[1];
                Arceri[1] = arceri[1];
                Catapulte[1] = catapulte[1];

                Guerrieri[2] = guerrieri[2];
                Lanceri[2] = lanceri[2];
                Arceri[2] = arceri[2];
                Catapulte[2] = catapulte[2];

                Guerrieri[3] = guerrieri[3];
                Lanceri[3] = lanceri[3];
                Arceri[3] = arceri[3];
                Catapulte[3] = catapulte[3];

                Guerrieri[4] = guerrieri[4];
                Lanceri[4] = lanceri[4];
                Arceri[4] = arceri[4];
                Catapulte[4] = catapulte[4];
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
            await EseguiBattagliaCooperativa(idAttacco, clientGuid, "1");
            
            return true;
        }

        // Esegui la battaglia cooperativa contro i barbari PVP
        private static async Task EseguiBattagliaCooperativa(string idAttacco, Guid clientGuid, string livello)
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

            await Battaglie.Battaglia_Distanza("Barbari_PVP", playerVirtuale, clientGuid, livello); //Pre battaglia, attaccano le unità a distanza ed i mezzi d'assedio

            // Party di giocatori (Somma delle unità dei giocatori)
            int[] guerrieri = playerVirtuale.Guerrieri;
            int[] picchieri = playerVirtuale.Lanceri;
            int[] arcieri = playerVirtuale.Arceri;
            int[] catapulte = playerVirtuale.Catapulte;

            int[] guerrieri_Temp = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Temp = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Temp = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Temp = new int[] { 0, 0, 0, 0, 0 };

            int[] guerrieri_Temp_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Temp_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Temp_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Temp_Morti = new int[] { 0, 0, 0, 0, 0 };

            //??
            int[] guerrieri_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Morti = new int[] { 0, 0, 0, 0, 0 };

            //Città Barbaro PVP
            int[] guerrieri_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Enemy = new int[] { 0, 0, 0, 0, 0 };

            int[] guerrieri_Temp_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Temp_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Temp_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Temp_Enemy = new int[] { 0, 0, 0, 0, 0 };

            int[] guerrieri_Temp_Enemy_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Temp_Enemy_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Temp_Enemy_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Temp_Enemy_Morti = new int[] { 0, 0, 0, 0, 0 };

            var citta = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == 1); // Supponiamo che stiamo attaccando la città di livello 1
            var vilaggio = playerVirtuale.VillaggiPersonali.FirstOrDefault(c => c.Livello == 1); // Supponiamo che stiamo attaccando la città di livello 1
            int liv = Convert.ToInt32(livello);

            if (liv >= 1 && liv <= 4)
            {
                guerrieri_Enemy[0] = citta.Guerrieri;
                picchieri_Enemy[0] = citta.Lancieri;
                arcieri_Enemy[0] = citta.Arcieri;
                catapulte_Enemy[0] = citta.Catapulte;
            }
            if (liv > 4 && liv <= 8)
            {
                guerrieri_Enemy[1] = citta.Guerrieri;
                picchieri_Enemy[1] = citta.Lancieri;
                arcieri_Enemy[1] = citta.Arcieri;
                catapulte_Enemy[1] = citta.Catapulte;
            }
            if (liv > 8 && liv <= 12)
            {
                guerrieri_Enemy[2] = citta.Guerrieri;
                picchieri_Enemy[2] = citta.Lancieri;
                arcieri_Enemy[2] = citta.Arcieri;
                catapulte_Enemy[2] = citta.Catapulte;
            }
            if (liv > 12 && liv <= 16)
            {
                guerrieri_Enemy[3] = citta.Guerrieri;
                picchieri_Enemy[3] = citta.Lancieri;
                arcieri_Enemy[3] = citta.Arcieri;
                catapulte_Enemy[3] = citta.Catapulte;
            }
            if (liv > 16 && liv <= 20)
            {
                guerrieri_Enemy[4] = citta.Guerrieri;
                picchieri_Enemy[4] = citta.Lancieri;
                arcieri_Enemy[4] = citta.Arcieri;
                catapulte_Enemy[4] = citta.Catapulte;
            }

            int tipi_Di_Unità = Battaglie.ContareTipiDiUnità(guerrieri, picchieri, arcieri, catapulte);
            int tipi_Di_Unità_Att = Battaglie.ContareTipiDiUnità(guerrieri_Enemy, picchieri_Enemy, arcieri_Enemy, catapulte_Enemy);

            // Calcolo del danno per il giocatore e il nemico
            double dannoInflittoDalNemico = Battaglie.CalcolareDanno_Invasore(arcieri_Enemy, catapulte_Enemy, guerrieri_Enemy, picchieri_Enemy, playerVirtuale) / tipi_Di_Unità;
            double dannoInflitto = 0;

            dannoInflitto += Battaglie.CalcolareDanno_Giocatore(arcieri[0], catapulte[0], guerrieri[0], picchieri[0], playerVirtuale, clientGuid, 1) / tipi_Di_Unità_Att;
            dannoInflitto += Battaglie.CalcolareDanno_Giocatore(arcieri[1], catapulte[1], guerrieri[1], picchieri[1], playerVirtuale, clientGuid, 2) / tipi_Di_Unità_Att;
            dannoInflitto += Battaglie.CalcolareDanno_Giocatore(arcieri[2], catapulte[2], guerrieri[2], picchieri[2], playerVirtuale, clientGuid, 3) / tipi_Di_Unità_Att;
            dannoInflitto += Battaglie.CalcolareDanno_Giocatore(arcieri[3], catapulte[3], guerrieri[3], picchieri[3], playerVirtuale, clientGuid, 4) / tipi_Di_Unità_Att;
            dannoInflitto += Battaglie.CalcolareDanno_Giocatore(arcieri[4], catapulte[4], guerrieri[4], picchieri[4], playerVirtuale, clientGuid, 5) / tipi_Di_Unità_Att;

            // Applicare il danno alle unità del giocatore
            guerrieri_Temp[0] = Battaglie.RidurreNumeroSoldati(guerrieri[0], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_1.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Guerriero_Difesa) * guerrieri[0], Esercito.Unità.Guerriero_1.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Guerriero_Salute);
            picchieri_Temp[0] = Battaglie.RidurreNumeroSoldati(picchieri[0], dannoInflittoDalNemico, (Esercito.Unità.Lancere_1.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Lancere_Difesa) * picchieri[0], Esercito.Unità.Lancere_1.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Lancere_Salute);
            arcieri_Temp[0] = Battaglie.RidurreNumeroSoldati(arcieri[0], dannoInflittoDalNemico * 0.70, (Esercito.Unità.Arcere_1.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Arcere_Difesa) * arcieri[0], Esercito.Unità.Arcere_1.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Arcere_Salute);
            catapulte_Temp[0] = Battaglie.RidurreNumeroSoldati(catapulte[0], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_1.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Catapulta_Difesa) * catapulte[0], Esercito.Unità.Catapulta_1.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Catapulta_Salute);

            guerrieri_Temp[1] = Battaglie.RidurreNumeroSoldati(guerrieri[1], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_2.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Guerriero_Difesa) * guerrieri[1], Esercito.Unità.Guerriero_2.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Guerriero_Salute);
            picchieri_Temp[1] = Battaglie.RidurreNumeroSoldati(picchieri[1], dannoInflittoDalNemico, (Esercito.Unità.Lancere_2.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Lancere_Difesa) * picchieri[1], Esercito.Unità.Lancere_2.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Lancere_Salute);
            arcieri_Temp[1] = Battaglie.RidurreNumeroSoldati(arcieri[1], dannoInflittoDalNemico * 0.70, (Esercito.Unità.Arcere_2.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Arcere_Difesa) * arcieri[1], Esercito.Unità.Arcere_2.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Arcere_Salute);
            catapulte_Temp[1] = Battaglie.RidurreNumeroSoldati(catapulte[1], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_2.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Catapulta_Difesa) * catapulte[1], Esercito.Unità.Catapulta_2.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Catapulta_Salute);

            guerrieri_Temp[2] = Battaglie.RidurreNumeroSoldati(guerrieri[2], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_3.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Guerriero_Difesa) * guerrieri[2], Esercito.Unità.Guerriero_3.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Guerriero_Salute);
            picchieri_Temp[2] = Battaglie.RidurreNumeroSoldati(picchieri[2], dannoInflittoDalNemico, (Esercito.Unità.Lancere_3.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Lancere_Difesa) * picchieri[2], Esercito.Unità.Lancere_3.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Lancere_Salute);
            arcieri_Temp[2] = Battaglie.RidurreNumeroSoldati(arcieri[2], dannoInflittoDalNemico * 0.70, (Esercito.Unità.Arcere_3.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Arcere_Difesa) * arcieri[2], Esercito.Unità.Arcere_3.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Arcere_Salute);
            catapulte_Temp[2] = Battaglie.RidurreNumeroSoldati(catapulte[2], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_3.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Catapulta_Difesa) * catapulte[2], Esercito.Unità.Catapulta_3.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Catapulta_Salute);

            guerrieri_Temp[3] = Battaglie.RidurreNumeroSoldati(guerrieri[3], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_4.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Guerriero_Difesa) * guerrieri[3], Esercito.Unità.Guerriero_4.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Guerriero_Salute);
            picchieri_Temp[3] = Battaglie.RidurreNumeroSoldati(picchieri[3], dannoInflittoDalNemico, (Esercito.Unità.Lancere_4.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Lancere_Difesa) * picchieri[3], Esercito.Unità.Lancere_4.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Lancere_Salute);
            arcieri_Temp[3] = Battaglie.RidurreNumeroSoldati(arcieri[3], dannoInflittoDalNemico * 0.70, (Esercito.Unità.Arcere_4.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Arcere_Difesa) * arcieri[3], Esercito.Unità.Arcere_4.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Arcere_Salute);
            catapulte_Temp[3] = Battaglie.RidurreNumeroSoldati(catapulte[3], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_4.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Catapulta_Difesa) * catapulte[3], Esercito.Unità.Catapulta_4.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Catapulta_Salute);

            guerrieri_Temp[4] = Battaglie.RidurreNumeroSoldati(guerrieri[4], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_5.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Guerriero_Difesa) * guerrieri[4], Esercito.Unità.Guerriero_5.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Guerriero_Salute);
            picchieri_Temp[4] = Battaglie.RidurreNumeroSoldati(picchieri[4], dannoInflittoDalNemico, (Esercito.Unità.Lancere_5.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Lancere_Difesa) * picchieri[4], Esercito.Unità.Lancere_5.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Lancere_Salute);
            arcieri_Temp[4] = Battaglie.RidurreNumeroSoldati(arcieri[4], dannoInflittoDalNemico * 0.70, (Esercito.Unità.Arcere_5.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Arcere_Difesa) * arcieri[4], Esercito.Unità.Arcere_5.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Arcere_Salute);
            catapulte_Temp[4] = Battaglie.RidurreNumeroSoldati(catapulte[4], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_5.Difesa + Ricerca.Soldati.Incremento.Difesa * playerVirtuale.Catapulta_Difesa) * catapulte[4], Esercito.Unità.Catapulta_5.Salute + Ricerca.Soldati.Incremento.Salute * playerVirtuale.Catapulta_Salute);


            // Applicare il danno alle unità nemiche
            guerrieri_Temp_Enemy[0] = Battaglie.RidurreNumeroSoldati(guerrieri_Enemy[0], dannoInflitto, Esercito.EsercitoNemico.Guerrieri_1.Difesa * guerrieri_Enemy[0], Esercito.EsercitoNemico.Guerrieri_1.Salute);
            picchieri_Temp_Enemy[0] = Battaglie.RidurreNumeroSoldati(picchieri_Enemy[1], dannoInflitto, Esercito.EsercitoNemico.Lanceri_2.Difesa * picchieri_Enemy[1], Esercito.EsercitoNemico.Lanceri_2.Salute);
            arcieri_Temp_Enemy[0] = Battaglie.RidurreNumeroSoldati(arcieri_Enemy[2], dannoInflitto, Esercito.EsercitoNemico.Arceri_3.Difesa * arcieri_Enemy[2], Esercito.EsercitoNemico.Arceri_3.Salute);
            catapulte_Temp_Enemy[0] = Battaglie.RidurreNumeroSoldati(catapulte_Enemy[3], dannoInflitto, Esercito.EsercitoNemico.Catapulte_4.Difesa * catapulte_Enemy[3], Esercito.EsercitoNemico.Catapulte_4.Salute);
            catapulte_Temp_Enemy[0] = Battaglie.RidurreNumeroSoldati(catapulte_Enemy[4], dannoInflitto, Esercito.EsercitoNemico.Catapulte_5.Difesa * catapulte_Enemy[4], Esercito.EsercitoNemico.Catapulte_5.Salute);

            for (int i = 0; i < 5; i++)// Calcola le perdite totali
            {
                //Giocatore
                if (guerrieri[i] > 0) guerrieri_Temp_Morti[i] = guerrieri[i] - guerrieri_Temp[i];
                if (guerrieri[i] > 0) picchieri_Temp_Morti[i] = picchieri[i] - picchieri_Temp[i];
                if (arcieri[i] > 0) arcieri_Temp_Morti[i] = arcieri[i] - arcieri_Temp[i];
                if (catapulte[i] > 0) catapulte_Temp_Morti[i] = catapulte[i] - catapulte_Temp[i];

                //Barbaro (Villaggio/Città)
                if (guerrieri_Enemy[i] > 0) guerrieri_Temp_Enemy_Morti[i] = guerrieri_Enemy[i] - guerrieri_Temp_Enemy[i];
                if (guerrieri_Enemy[i] > 0) picchieri_Temp_Enemy_Morti[i] = picchieri_Enemy[i] - picchieri_Temp_Enemy[i];
                if (arcieri_Enemy[i] > 0) arcieri_Temp_Enemy_Morti[i] = arcieri_Enemy[i] - arcieri_Temp_Enemy[i];
                if (catapulte_Enemy[i] > 0) catapulte_Temp_Enemy_Morti[i] = catapulte_Enemy[i] - catapulte_Temp_Enemy[i];
            }
            double frecceConsumate = totaleFrecce - playerVirtuale.Frecce;
            
            // Invia i risultati a tutti i partecipanti
            foreach (var partecipante in attacco.GiocatoriPartecipanti)
            {
                var giocatore = servers_.GetPlayer_Data(partecipante.Key);
                if (giocatore != null && giocatore.guid_Player != Guid.Empty)
                {
                    Send(giocatore.guid_Player, $"Log_Server|Danno subito: {(dannoInflittoDalNemico * tipi_Di_Unità).ToString("0.00")}");
                    Send(giocatore.guid_Player, $"Log_Server|Danno inflitto: {(dannoInflitto * tipi_Di_Unità_Att).ToString("0.00")}");
                    Send(giocatore.guid_Player, $"Log_Server|Risultato dell'attacco cooperativo #{idAttacco}:");
                    
                    Send(giocatore.guid_Player, $"Log_Server|" +
                         $"Guerrieri: {guerrieri_Temp_Morti[0]}/{guerrieri[0]} lv1 {guerrieri_Temp_Morti[1]}/{guerrieri[1]} lv2 {guerrieri_Temp_Morti[2]}/{guerrieri[2]} lv3 {guerrieri_Temp_Morti[3]}/{guerrieri[3]} lv4 {guerrieri_Temp_Morti[4]}/{guerrieri[4]} lv5\r\n " +
                         $"Lancieri: {picchieri_Temp_Morti[0]}/{picchieri[0]} lv1 {picchieri_Temp_Morti[1]}/{picchieri[1]} lv2 {picchieri_Temp_Morti[2]}/{picchieri[2]} lv3 {picchieri_Temp_Morti[3]}/{picchieri[3]} lv4 {picchieri_Temp_Morti[4]}/{picchieri[4]} lv5\r\n " +
                         $"Arcieri: {arcieri_Temp_Morti}/{arcieri[0]} lv1 {arcieri_Temp_Morti[1]}/{arcieri[1]} lv2 {arcieri_Temp_Morti[2]}/{arcieri[2]} lv3 {arcieri_Temp_Morti[3]}/{arcieri[3]} lv4 {arcieri_Temp_Morti[4]}/{arcieri[4]} lv5\r\n " +
                         $"Catapulte: {catapulte_Temp_Morti[0]}/{catapulte[0]} lv1 {catapulte_Temp_Morti[1]}/{catapulte[1]} lv2 {catapulte_Temp_Morti[2]}/{catapulte[2]} lv3 {catapulte_Temp_Morti[3]}/{catapulte[3]} lv4 {catapulte_Temp_Morti[4]}/{catapulte[4]} lv5\r\n");
                    Send(giocatore.guid_Player, $"Log_Server|Truppe perse in totale:");
                    
                    Send(giocatore.guid_Player, $"Log_Server" +
                        $"Guerrieri: {guerrieri_Temp_Enemy_Morti}/{guerrieri_Enemy[0]} lv1 {guerrieri_Temp_Enemy_Morti[1]}/{guerrieri_Enemy[1]} lv2 {guerrieri_Temp_Enemy_Morti[2]}/{guerrieri_Enemy[2]} lv3 {guerrieri_Temp_Enemy_Morti[3]}/{guerrieri_Enemy[3]} lv4 {guerrieri_Temp_Enemy_Morti[4]}/{guerrieri_Enemy[4]} lv5\r\n " +
                $"Lancieri: {picchieri_Temp_Enemy_Morti[0]}/{picchieri_Enemy[0]} lv1 {picchieri_Temp_Enemy_Morti[1]}/{picchieri_Enemy[1]} lv2 {picchieri_Temp_Enemy_Morti[2]}/{picchieri_Enemy[2]} lv3 {picchieri_Temp_Enemy_Morti[3]}/{picchieri_Enemy[3]} lv4 {picchieri_Temp_Enemy_Morti[4]}/{picchieri_Enemy[4]} lv5\r\n " +
                $"Arcieri: {arcieri_Temp_Enemy_Morti[0]}/{arcieri_Enemy[0]} lv1 {arcieri_Temp_Enemy_Morti[1]}/{arcieri_Enemy[1]} lv2 {arcieri_Temp_Enemy_Morti[2]}/{arcieri_Enemy[2]} lv3 {arcieri_Temp_Enemy_Morti[3]}/{arcieri_Enemy[3]} lv4 {arcieri_Temp_Enemy_Morti[4]}/{arcieri_Enemy[4]} lv5\r\n " +
                $"Catapulte: {catapulte_Temp_Enemy_Morti[0]}/{catapulte_Enemy[0]} lv1 {catapulte_Temp_Enemy_Morti[1]}/{catapulte_Enemy[1]} lv2 {catapulte_Temp_Enemy_Morti[2]}/{catapulte_Enemy[2]} lv3 {catapulte_Temp_Enemy_Morti[3]}/{catapulte_Enemy[3]} lv4 {catapulte_Temp_Enemy_Morti[4]}/{catapulte_Enemy[4]} lv5\r\n");
                    Send(giocatore.guid_Player, $"Log_Server|Truppe perse dai barbari:");

                    // Calcolo delle perdite proporzionali per questo giocatore
                    double rapportoGuerrieri = totaleGuerrieri.Sum() > 0 ? (double)partecipante.Value.Guerrieri.Sum() / totaleGuerrieri.Sum() : 0;
                    double rapportoLancieri = totaleLancieri.Sum() > 0 ? (double)partecipante.Value.Lanceri.Sum() / totaleLancieri.Sum() : 0;
                    double rapportoArcieri = totaleArcieri.Sum() > 0 ? (double)partecipante.Value.Arceri.Sum() / totaleArcieri.Sum() : 0;
                    double rapportoCatapulte = totaleCatapulte.Sum() > 0 ? (double)partecipante.Value.Catapulte.Sum() / totaleCatapulte.Sum() : 0;
                    double rapportoFrecce = totaleFrecce > 0 ? (double)playerVirtuale.Frecce / totaleFrecce : 0;

                    // Calcolo perdite per ogni tipo di unità
                    guerrieri_Morti[0] = (int)Math.Ceiling(rapportoGuerrieri * guerrieri_Temp_Morti[0]);
                    picchieri_Temp[0] = (int)Math.Ceiling(rapportoLancieri * picchieri_Temp_Morti[0]);
                    arcieri_Temp[0] = (int)Math.Ceiling(rapportoArcieri * arcieri_Temp_Morti[0]);
                    catapulte_Temp[0] = (int)Math.Ceiling(rapportoCatapulte * catapulte_Temp_Morti[0]);
                    int freccePersiGiocatore = (int)Math.Ceiling(rapportoFrecce * frecceConsumate);

                    // Calcola l'esperienza in base al contributo
                    double contributo = (rapportoGuerrieri + rapportoLancieri + rapportoArcieri + rapportoCatapulte) / 4.0;
                    int esperienza = 0;

                    esperienza += guerrieri_Temp_Enemy_Morti[0] * Esercito.EsercitoNemico.Guerrieri_1.Esperienza +
                                 picchieri_Temp_Enemy_Morti[0] * Esercito.EsercitoNemico.Lanceri_1.Esperienza +
                                 arcieri_Temp_Enemy_Morti[0] * Esercito.EsercitoNemico.Arceri_1.Esperienza +
                                 catapulte_Temp_Enemy_Morti[0] * Esercito.EsercitoNemico.Arceri_1.Esperienza;

                    esperienza += guerrieri_Temp_Enemy_Morti[1] * Esercito.EsercitoNemico.Guerrieri_2.Esperienza +
                                         picchieri_Temp_Enemy_Morti[1] * Esercito.EsercitoNemico.Lanceri_2.Esperienza +
                                         arcieri_Temp_Enemy_Morti[1] * Esercito.EsercitoNemico.Arceri_2.Esperienza +
                                         catapulte_Temp_Enemy_Morti[1] * Esercito.EsercitoNemico.Arceri_2.Esperienza;

                    esperienza += guerrieri_Temp_Enemy_Morti[2] * Esercito.EsercitoNemico.Guerrieri_3.Esperienza +
                                         picchieri_Temp_Enemy_Morti[2] * Esercito.EsercitoNemico.Lanceri_3.Esperienza +
                                         arcieri_Temp_Enemy_Morti[2] * Esercito.EsercitoNemico.Arceri_3.Esperienza +
                                         catapulte_Temp_Enemy_Morti[2] * Esercito.EsercitoNemico.Arceri_3.Esperienza;

                    esperienza += guerrieri_Temp_Enemy_Morti[3] * Esercito.EsercitoNemico.Guerrieri_4.Esperienza +
                                         picchieri_Temp_Enemy_Morti[3] * Esercito.EsercitoNemico.Lanceri_4.Esperienza +
                                         arcieri_Temp_Enemy_Morti[3] * Esercito.EsercitoNemico.Arceri_4.Esperienza +
                                         catapulte_Temp_Enemy_Morti[3] * Esercito.EsercitoNemico.Arceri_4.Esperienza;

                    esperienza += guerrieri_Temp_Enemy_Morti[4] * Esercito.EsercitoNemico.Guerrieri_5.Esperienza +
                                         picchieri_Temp_Enemy_Morti[4] * Esercito.EsercitoNemico.Lanceri_5.Esperienza +
                                         arcieri_Temp_Enemy_Morti[4] * Esercito.EsercitoNemico.Arceri_5.Esperienza +
                                         catapulte_Temp_Enemy_Morti[4] * Esercito.EsercitoNemico.Arceri_5.Esperienza;

                    int esperienzaGuadagnata = (int)(contributo * esperienza);
                    giocatore.Esperienza += esperienzaGuadagnata;
                    
                    // Restituisci le truppe sopravvissute al giocatore
                    int[] guerrieriRestituiti = new int[] { 0, 0, 0, 0, 0 };
                    int[] lancieriRestituiti = new int[] { 0, 0, 0, 0, 0 };
                    int[] arcieriRestituiti = new int[] { 0, 0, 0, 0, 0 };
                    int[] catapulteRestituite = new int[] { 0, 0, 0, 0, 0 };
                    double frecceRestituite = giocatore.Frecce - freccePersiGiocatore;

                    // Dopo
                    for (int i = 0; i < 5; i++)
                    {
                        guerrieriRestituiti[i] = partecipante.Value.Guerrieri[i] - guerrieri_Morti[i];
                        lancieriRestituiti[i] = partecipante.Value.Lanceri[i] - picchieri_Morti[i];
                        arcieriRestituiti[i] = partecipante.Value.Arceri[i] - arcieri_Morti[i];
                        catapulteRestituite[i] = partecipante.Value.Catapulte[i] - catapulte_Morti[i];
                    }
                    giocatore.Guerrieri[0] += guerrieriRestituiti[0];
                    giocatore.Lanceri[0] += lancieriRestituiti[0];
                    giocatore.Arceri[0] += arcieriRestituiti[0];
                    giocatore.Catapulte[0] += catapulteRestituite[0];
                    giocatore.Frecce += frecceRestituite;
                    
                    Send(giocatore.guid_Player, $"Log_Server|Truppe restituite: Guerrieri: {guerrieriRestituiti}, Lancieri: {lancieriRestituiti}, Arcieri: {arcieriRestituiti}, Catapulte: {catapulteRestituite}");
                    Send(giocatore.guid_Player, $"Log_Server|Esperienza guadagnata: {esperienzaGuadagnata}");
                    Send(giocatore.guid_Player, $"Log_Server|" +
                        $"Guerrieri: {guerrieri_Temp_Morti[0]}/{guerrieri[0]} lv1 {guerrieri_Temp_Morti[1]}/{guerrieri[1]} lv2 {guerrieri_Temp_Morti[2]}/{guerrieri[2]} lv3 {guerrieri_Temp_Morti[3]}/{guerrieri[3]} lv4 {guerrieri_Temp_Morti[4]}/{guerrieri[4]} lv5\r\n " +
                        $"Lancieri: {picchieri_Temp_Morti[0]}/{picchieri[0]} lv1 {picchieri_Temp_Morti[1]}/{picchieri[1]} lv2 {picchieri_Temp_Morti[2]}/{picchieri[2]} lv3 {picchieri_Temp_Morti[3]}/{picchieri[3]} lv4 {picchieri_Temp_Morti[4]}/{picchieri[4]} lv5\r\n " +
                        $"Arcieri: {arcieri_Temp_Morti}/{arcieri[0]} lv1 {arcieri_Temp_Morti[1]}/{arcieri[1]} lv2 {arcieri_Temp_Morti[2]}/{arcieri[2]} lv3 {arcieri_Temp_Morti[3]}/{arcieri[3]} lv4 {arcieri_Temp_Morti[4]}/{arcieri[4]} lv5\r\n " +
                        $"Catapulte: {catapulte_Temp_Morti[0]}/{catapulte[0]} lv1 {catapulte_Temp_Morti[1]}/{catapulte[1]} lv2 {catapulte_Temp_Morti[2]}/{catapulte[2]} lv3 {catapulte_Temp_Morti[3]}/{catapulte[3]} lv4 {catapulte_Temp_Morti[4]}/{catapulte[4]} lv5\r\n");
                    Send(giocatore.guid_Player, $"Log_Server|Le tue perdite personali: [{giocatore.Username}]");
                }
            }

            // Aggiorna i barbari PVP
            if (liv >= 1 && liv <= 4)
            {
                citta.Guerrieri = guerrieri_Temp_Enemy[0];
                citta.Lancieri = picchieri_Temp_Enemy[0];
                citta.Arcieri = arcieri_Temp_Enemy[0];
                citta.Catapulte = catapulte_Temp_Enemy[0];
            }
            if (liv > 4 && liv <= 8)
            {
                citta.Guerrieri = guerrieri_Temp_Enemy[1];
                citta.Lancieri = picchieri_Temp_Enemy[1];
                citta.Arcieri = arcieri_Temp_Enemy[1];
                citta.Catapulte = catapulte_Temp_Enemy[1];
            }
            if (liv > 8 && liv <= 12)
            {
                citta.Guerrieri = guerrieri_Temp_Enemy[2];
                citta.Lancieri = picchieri_Temp_Enemy[2];
                citta.Arcieri = arcieri_Temp_Enemy[2];
                citta.Catapulte = catapulte_Temp_Enemy[2];
            }
            if (liv > 12 && liv <= 16)
            {
                citta.Guerrieri = guerrieri_Temp_Enemy[3];
                citta.Lancieri = picchieri_Temp_Enemy[3];
                citta.Arcieri = arcieri_Temp_Enemy[3];
                citta.Catapulte = catapulte_Temp_Enemy[3];
            }
            if (liv > 16 && liv <= 20)
            {
                citta.Guerrieri = guerrieri_Temp_Enemy[4];
                citta.Lancieri = picchieri_Temp_Enemy[4];
                citta.Arcieri = arcieri_Temp_Enemy[4];
                citta.Catapulte = catapulte_Temp_Enemy[4];
            }

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