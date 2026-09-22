using Server_Strategico.ServerData.Moduli.Battaglie;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Server.Server;
using static Server_Strategico.ServerData.Moduli.Battaglie.Battaglia;

namespace Server_Strategico.Gioco
{
    // Riscritto il 2026-09-14 (rinominato da AttacchiCooperativi.cs). La logica di battaglia (EseguiBattagliaCooperativa)
    // è stata riscritta da zero riusando gli helper "internal" di BattagliaPVE.cs invece del vecchio Gioco/Battaglie.cs,
    // per non duplicare per la quarta volta le formule di danno/statistiche. Decisioni di design confermate dall'utente:
    //  - Ogni truppa combatte con le statistiche del SUO proprietario (niente "giocatore virtuale" con stats mediate/pool).
    //  - Il contributo di truppe supporta tutti e 5 i tier (non solo il tier 1 come nel vecchio codice) — protocollo
    //    "Raduno|Partecipa|..." esteso di conseguenza (vedi GestisciComando).
    //  - Report: ogni partecipante riceve un Report personale (stesso RisultatoBattaglia/RisultatoFase di PVP/PVE,
    //    gestibile dal client come già fa) più un riepilogo di tutto il raduno in RisultatoBattaglia.Partecipanti
    //    (nuovo campo aggiunto in Battaglia.cs, additivo/non-breaking per PVP e PVE).
    // 21/09/2026: comandi/risposte lato wire unificati sotto il prefisso "Raduno" (prima erano usati tre nomi
    // diversi: "AttaccoCooperativo" in entrata, "RadunoPartecipo" e "AttacchiCooperativi|MieiAttacchi" in uscita).
    // La classe C# resta "AttacchiCooperativi" per non allargare il diff — solo le stringhe sul wire sono cambiate.
    // Aggiunto anche il flag Alleanza (vedi AttaccoCooperativo) e la sua Opzione B di enforcement lato server.
    //
    // 22/09/2026, su richiesta dell'utente: il bersaglio di un raduno può ora essere anche un altro
    // giocatore (PVP), non solo una Città Barbara — vedi AttaccoCooperativo.TipoBersaglio/BersaglioUsername
    // e il nuovo metodo EseguiBattagliaCooperativaPVP più sotto. Riusa gli stessi due principi già confermati
    // sopra (statistiche proprie, mai un pool mediato) MA applicati alle 7 fasi sequenziali del PVP singolo
    // (BattagliaPVP.cs: Ingresso/Mura/Cancello/Torri/Città/Castello/Villaggio — decisione confermata
    // dall'utente il 22/09/2026: niente scorciatoia "un colpo solo", stesso modello del PVP singolo) invece
    // della singola fase usata contro le Città Barbare. Le regole di chi può essere creatore/bersaglio sono
    // le stesse del PVP singolo (ServerConnection.Battaglia, case "PVP"): Livello >= PVP_Unlock e nessuno
    // Scudo della Pace attivo, né per il creatore né per il bersaglio (stessi due filtri con cui Server.cs
    // costruisce Utenti_PVP).
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
            public int LivelloTarget { get; set; }
            public bool Alleanza { get; set; }

            // NUOVO (22/09/2026): "Barbaro" (default, come prima) usa LivelloTarget; "PVP" usa
            // BersaglioUsername e ignora LivelloTarget (resta 0).
            public string TipoBersaglio { get; set; } = "Barbaro";
            public string BersaglioUsername { get; set; }

            public AttaccoCooperativo(string id, string creatore, string tipoBersaglio, int livelloTarget, string bersaglioUsername, bool alleanza = false)
            {
                IdAttacco = id;
                CreatoreUsername = creatore;
                TipoBersaglio = tipoBersaglio;
                LivelloTarget = livelloTarget;
                BersaglioUsername = bersaglioUsername;
                Alleanza = alleanza;
                GiocatoriPartecipanti = new Dictionary<string, TruppeContribuite>();
                OraPrevista = DateTime.Now.AddMinutes(30); // Default 30 minuti
                AttaccoInCorso = false;
                TempoRimanente = 1800; // 30 minuti in secondi
            }

            public bool AggiungiGiocatore(string username, TruppeContribuite truppe)
            {
                if (AttaccoInCorso) return false;

                if (GiocatoriPartecipanti.ContainsKey(username))
                {
                    var truppeEsistenti = GiocatoriPartecipanti[username];
                    truppeEsistenti.Player = username;
                    for (int i = 0; i < 5; i++)
                    {
                        truppeEsistenti.Guerrieri[i] += truppe.Guerrieri[i];
                        truppeEsistenti.Lanceri[i] += truppe.Lanceri[i];
                        truppeEsistenti.Arceri[i] += truppe.Arceri[i];
                        truppeEsistenti.Catapulte[i] += truppe.Catapulte[i];
                    }
                }
                else GiocatoriPartecipanti.Add(username, truppe);
                return true;
            }

            public bool RimuoviGiocatore(string username)
            {
                if (AttaccoInCorso) return false;
                return GiocatoriPartecipanti.Remove(username);
            }
        }

        // Classe per memorizzare le truppe che un giocatore contribuisce (tutti e 5 i tier)
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
                for (int i = 0; i < 5; i++)
                {
                    Guerrieri[i] = guerrieri[i];
                    Lanceri[i] = lanceri[i];
                    Arceri[i] = arceri[i];
                    Catapulte[i] = catapulte[i];
                }
            }
        }

        // Metodo per creare un nuovo attacco cooperativo, contro la Città Barbara del livello indicato (1-20,
        // tipoBersaglio="Barbaro") oppure contro un giocatore (tipoBersaglio="PVP", bersaglioUsername valorizzato).
        // La validazione (livello 1-20 / esistenza e attaccabilità del giocatore bersaglio) è già stata fatta
        // dal chiamante (GestisciComando, case "Crea") prima di arrivare qui.
        public static string CreaAttaccoCooperativo(string username, string tipoBersaglio, int livelloTarget, string bersaglioUsername, bool alleanza = false)
        {
            string idAttacco = Guid.NewGuid().ToString().Substring(0, 8);
            var nuovoAttacco = new AttaccoCooperativo(idAttacco, username, tipoBersaglio, livelloTarget, bersaglioUsername, alleanza);
            AttacchiInCorso.Add(idAttacco, nuovoAttacco);
            AttacchiInPlayer.Add(idAttacco, nuovoAttacco);
            return idAttacco;
        }

        // Metodo per partecipare a un attacco cooperativo, con truppe di tutti e 5 i tier
        public static bool PartecipaDiAttacco(string idAttacco, string username, int[] guerrieri, int[] lancieri, int[] arcieri, int[] catapulte, Guid clientGuid)
        {
            if (!AttacchiInCorso.ContainsKey(idAttacco))
            {
                Send(clientGuid, $"Log_Server|Attacco con ID {idAttacco} non trovato.");
                return false;
            }

            // Opzione B (alleanze non ancora implementate): un raduno Alleanza=true è riservato al suo creatore.
            var attaccoCheck = AttacchiInCorso[idAttacco];
            if (attaccoCheck.Alleanza && username != attaccoCheck.CreatoreUsername)
            {
                Send(clientGuid, $"Log_Server|Questo raduno è riservato all'alleanza del creatore (non ancora disponibile per te).");
                return false;
            }

            var player = servers_.GetPlayer(username);
            if (player == null)
            {
                Send(clientGuid, $"Log_Server|Giocatore non trovato.");
                return false;
            }

            if (guerrieri.Length != 5 || lancieri.Length != 5 || arcieri.Length != 5 || catapulte.Length != 5)
            {
                Send(clientGuid, $"Log_Server|Formato truppe non valido (attesi 5 tier per tipo).");
                return false;
            }

            // Verifico che i valori non siano negativi
            for (int i = 0; i < 5; i++)
            {
                if (guerrieri[i] < 0 || lancieri[i] < 0 || arcieri[i] < 0 || catapulte[i] < 0)
                {
                    Send(clientGuid, $"Log_Server|Non puoi inviare un numero negativo di truppe.");
                    return false;
                }
            }

            // Verifico che venga inviata almeno una truppa
            if (guerrieri.Sum() == 0 && lancieri.Sum() == 0 && arcieri.Sum() == 0 && catapulte.Sum() == 0)
            {
                Send(clientGuid, $"Log_Server|Devi inviare almeno una truppa.");
                return false;
            }

            // Verifica disponibilità truppe per ogni tier (controllo importante)
            for (int i = 0; i < 5; i++)
            {
                if (player.Guerrieri[i] < guerrieri[i] || player.Lanceri[i] < lancieri[i] ||
                    player.Arceri[i] < arcieri[i] || player.Catapulte[i] < catapulte[i])
                {
                    Send(clientGuid, $"Log_Server|Non hai abbastanza truppe di livello {i + 1} disponibili.");
                    return false;
                }
            }

            // Crea l'oggetto globale truppe per aggiungere le unità dei partecipanti
            var truppe = new TruppeContribuite(guerrieri, lancieri, arcieri, catapulte, player.Username);
            AttacchiInCorso[idAttacco].AggiungiGiocatore(username, truppe);

            // IMPORTANTE: Rimuovi le truppe dal giocatore DOPO aver confermato che l'aggiunta è avvenuta con successo
            for (int i = 0; i < 5; i++)
            {
                player.Guerrieri[i] -= guerrieri[i];
                player.Lanceri[i] -= lancieri[i];
                player.Arceri[i] -= arcieri[i];
                player.Catapulte[i] -= catapulte[i];
            }

            // Calcola e invia informazioni sulle truppe totali dell'attacco
            var attacco = AttacchiInCorso[idAttacco];
            int totGuerrieri = 0, totLancieri = 0, totArcieri = 0, totCatapulte = 0;

            foreach (var part in attacco.GiocatoriPartecipanti)
            {
                totGuerrieri += part.Value.Guerrieri.Sum();
                totLancieri += part.Value.Lanceri.Sum();
                totArcieri += part.Value.Arceri.Sum();
                totCatapulte += part.Value.Catapulte.Sum();
            }

            int gInviati = guerrieri.Sum(), lInviati = lancieri.Sum(), aInviati = arcieri.Sum(), cInviati = catapulte.Sum();

            // Invia conferma al giocatore
            Send(clientGuid, $"Log_Server|Hai contribuito all'attacco #{idAttacco} con: {gInviati} Guerrieri, {lInviati} Lancieri, {aInviati} Arcieri, {cInviati} Catapulte.");
            Send(clientGuid, $"Log_Server|Forze totali: {totGuerrieri} Guerrieri, {totLancieri} Lancieri, {totArcieri} Arcieri, {totCatapulte} Catapulte.");
            Send(clientGuid, $"Raduno|Partecipato|{attacco.CreatoreUsername}|{idAttacco}|{attacco.GiocatoriPartecipanti.Count}|{gInviati}|{lInviati}|{aInviati}|{cInviati}|{attacco.TempoRimanente / 60}");

            foreach (var partecipante in attacco.GiocatoriPartecipanti) // Notifica tutti i partecipanti dell'aggiornamento
            {
                var giocatore = servers_.GetPlayer(partecipante.Key);
                if (giocatore != null && giocatore.guid_Player != Guid.Empty && giocatore.guid_Player != clientGuid)
                {
                    Send(giocatore.guid_Player, $"Log_Server|{player.Username} ha inviato truppe all'attacco #{idAttacco}: {gInviati} Guerrieri, {lInviati} Lancieri, {aInviati} Arcieri, {cInviati} Catapulte.");
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
            var player = servers_.GetPlayer(username);

            if (player != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    player.Guerrieri[i] += truppeContribuite.Guerrieri[i];
                    player.Lanceri[i] += truppeContribuite.Lanceri[i];
                    player.Arceri[i] += truppeContribuite.Arceri[i];
                    player.Catapulte[i] += truppeContribuite.Catapulte[i];
                }
            }

            // Rimuovi il giocatore dall'attacco
            attacco.RimuoviGiocatore(username);

            Send(clientGuid, $"Log_Server|Hai abbandonato l'attacco #{idAttacco} e recuperato le tue truppe.");

            // Se non ci sono più partecipanti, rimuovi l'attacco
            if (attacco.GiocatoriPartecipanti.Count == 0)
            {
                AttacchiInCorso.Remove(idAttacco);
                AttacchiInPlayer.Remove(idAttacco);
                Send(clientGuid, $"Log_Server|L'attacco #{idAttacco} è stato cancellato perché non ci sono più partecipanti.");
            }
            else
            {
                // Notifica gli altri partecipanti
                foreach (var partecipante in attacco.GiocatoriPartecipanti)
                {
                    var giocatore = servers_.GetPlayer(partecipante.Key);
                    if (giocatore != null && giocatore.guid_Player != Guid.Empty)
                    {
                        Send(giocatore.guid_Player, $"Log_Server|{player.Username} ha abbandonato l'attacco #{idAttacco}.");
                    }
                }
            }

            return true;
        }

        // Ottieni la lista degli attacchi cooperativi disponibili. Filtra i raduni Alleanza=true creati da altri
        // giocatori (Opzione B: finché non esiste un sistema di alleanze reale, sono visibili solo al creatore).
        public static void GetListaAttacchi(Guid clientGuid, string username)
        {
            var attacchiVisibili = AttacchiInCorso.Where(kv => !kv.Value.Alleanza || kv.Value.CreatoreUsername == username).ToList();

            if (attacchiVisibili.Count == 0)
            {
                Send(clientGuid, $"Log_Server|Non ci sono attacchi cooperativi in preparazione.");
                return;
            }

            Send(clientGuid, $"Log_Server|Attacchi cooperativi in preparazione:");
            foreach (var attaccoInfo in attacchiVisibili)
            {
                int gTot = 0, lTot = 0, aTot = 0, cTot = 0;
                foreach (var part in attaccoInfo.Value.GiocatoriPartecipanti)
                {
                    gTot += part.Value.Guerrieri.Sum();
                    lTot += part.Value.Lanceri.Sum();
                    aTot += part.Value.Arceri.Sum();
                    cTot += part.Value.Catapulte.Sum();
                }

                string descrizioneBersaglio = attaccoInfo.Value.TipoBersaglio == "PVP"
                    ? $"Giocatore {attaccoInfo.Value.BersaglioUsername}"
                    : $"Città Barbaro Lv.{attaccoInfo.Value.LivelloTarget}";
                Send(clientGuid, $"Log_Server|ID: {attaccoInfo.Key} - Bersaglio: {descrizioneBersaglio} - Partecipanti: {attaccoInfo.Value.GiocatoriPartecipanti.Count} - Truppe: G:{gTot}, L:{lTot}, A:{aTot}, C:{cTot} - Tempo: {attaccoInfo.Value.TempoRimanente / 60} min");
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
                var giocatore = servers_.GetPlayer(partecipante.Key);
                if (giocatore != null && giocatore.guid_Player != Guid.Empty)
                {
                    Send(giocatore.guid_Player, $"Log_Server|L'attacco cooperativo #{idAttacco} sta iniziando!");
                }
            }

            // Esegui la battaglia (22/09/2026: instrada al percorso PVP o Barbaro a seconda del bersaglio)
            if (attacco.TipoBersaglio == "PVP") await EseguiBattagliaCooperativaPVP(idAttacco, clientGuid);
            else await EseguiBattagliaCooperativa(idAttacco, clientGuid);

            return true;
        }

        private static Dictionary<string, int> DistribuisciProporzionalmente(int totale, Dictionary<string, int> pesi)
        {
            var risultato = new Dictionary<string, int>();
            int pesoTotale = pesi.Values.Sum();
            if (totale <= 0 || pesoTotale <= 0)
            {
                foreach (var k in pesi.Keys) risultato[k] = 0;
                return risultato;
            }

            var partiFrazionarie = new Dictionary<string, double>();
            int sommaBase = 0;
            foreach (var kv in pesi)
            {
                double quota = (double)totale * kv.Value / pesoTotale;
                int baseAmount = (int)Math.Floor(quota);
                risultato[kv.Key] = baseAmount;
                partiFrazionarie[kv.Key] = quota - baseAmount;
                sommaBase += baseAmount;
            }

            int resto = totale - sommaBase;
            foreach (var key in partiFrazionarie.OrderByDescending(kv => kv.Value).Select(kv => kv.Key))
            {
                if (resto <= 0) break;
                risultato[key]++;
                resto--;
            }
            return risultato;
        }

        // Esegue la battaglia cooperativa contro la Città Barbara bersaglio dell'attacco. Ogni partecipante combatte
        // con le proprie truppe usando le PROPRIE statistiche (ricerca/bonus del proprio profilo), riusando gli helper
        // internal di BattagliaPVE.cs. Meccanica (vedi anche i commenti nei singoli passaggi):
        //  - Fase a distanza: ogni partecipante spara per conto proprio (proprie frecce/statistiche); il danno in
        //    USCITA verso il nemico (condiviso) si somma e si applica una volta sola. Il danno in ENTRATA dai barbari
        //    è calcolato una sola volta sulla forza totale schierata, poi le perdite (per tier) sono ridistribuite
        //    proporzionalmente tra i partecipanti in base a quante unità di quel tipo/tier ciascuno ha in campo —
        //    applicare lo stesso "conteggio corpi" a ciascun partecipante indipendentemente lo moltiplicherebbe per il
        //    numero di partecipanti.

        //  - Fase corpo a corpo: il danno in USCITA (verso il nemico, condiviso) si somma dai singoli partecipanti e si
        //    applica una volta sola al pool nemico. Il danno in ENTRATA (dai barbari) è lo stesso "dannoPerTipo" applicato
        //    indipendentemente a ciascun partecipante — la difesa/salute PROPRIA di ciascuno (che scala col proprio
        //    numero di truppe) fa naturalmente da moltiplicatore, esattamente come BattagliaPVE già fa per dividere un
        //    singolo danno tra i 4 tipi di unità di UN gruppo; qui lo stesso meccanismo è esteso ai partecipanti.
        private static async Task EseguiBattagliaCooperativa(string idAttacco, Guid clientGuid)
        {
            var attacco = AttacchiInCorso[idAttacco];
            int livello = attacco.LivelloTarget;

            // Bersaglio: SOLO Città Barbaro in questo primo passaggio (i raduni contro giocatori sono rimandati,
            // come deciso con l'utente il 2026-09-14).
            var citta = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == livello);
            var enemyUnits = BattagliaPVE.CaricaUnitaNemiche(livello, "Città Barbaro", null);
            if (citta == null || enemyUnits.TotalUnits() == 0)
            {
                Send(clientGuid, "Log_Server|[error]Città Barbara non trovata o già sconfitta: il raduno viene annullato e le truppe restituite.");
                RestituisciTruppeECancella(attacco, "la Città Barbara bersaglio non è più disponibile");
                return;
            }
            var enemyUnitsIniziali = enemyUnits.Clone();

            // Carica giocatori e truppe inviate da ciascun partecipante (convertite da TruppeContribuite a UnitGroup).
            var giocatori = new Dictionary<string, Player>();
            var truppeInviate = new Dictionary<string, UnitGroup>();
            foreach (var kv in attacco.GiocatoriPartecipanti)
            {
                var player = servers_.GetPlayer(kv.Key);
                if (player == null) continue;
                giocatori[kv.Key] = player;
                truppeInviate[kv.Key] = new UnitGroup
                {
                    Guerrieri = (int[])kv.Value.Guerrieri.Clone(),
                    Lancieri = (int[])kv.Value.Lanceri.Clone(),
                    Arcieri = (int[])kv.Value.Arceri.Clone(),
                    Catapulte = (int[])kv.Value.Catapulte.Clone()
                };
            }

            if (giocatori.Count == 0)
            {
                Send(clientGuid, "Log_Server|[error]Nessun partecipante valido, raduno annullato.");
                AttacchiInCorso.Remove(idAttacco);
                AttacchiInPlayer.Remove(idAttacco);
                return;
            }

            // ═══ FASE A DISTANZA ═══
            var risultatiDistanza = new Dictionary<string, BattagliaDistanza>();
            int dannoGuerrieriTotale = 0, dannoLancieriTotale = 0;
            var truppePooled = new UnitGroup();
            foreach (var kv in truppeInviate)
            {
                var player = giocatori[kv.Key];
                var result = new BattagliaDistanza
                {
                    Attaccante_Schierati = kv.Value,
                    Difensore_Unità_Presenti = enemyUnits.TotalUnits() > 0
                };
                result = BattagliaPVE.CalcolaAttaccoDistanzaGiocatore(kv.Value, player, result);
                risultatiDistanza[kv.Key] = result;
                dannoGuerrieriTotale += result.Attaccante_Danno_Guerrieri;
                dannoLancieriTotale += result.Attaccante_Danno_Lancieri;

                for (int i = 0; i < 5; i++)
                {
                    truppePooled.Guerrieri[i] += kv.Value.Guerrieri[i];
                    truppePooled.Lancieri[i] += kv.Value.Lancieri[i];
                    truppePooled.Arcieri[i] += kv.Value.Arcieri[i];
                    truppePooled.Catapulte[i] += kv.Value.Catapulte[i];
                }
            }

            // Danno in uscita verso il nemico (unico, condiviso): applicato una volta sola.
            var enemyMortiDistanza = ApplicaDanniDistanza_(enemyUnits.Clone(), dannoGuerrieriTotale, dannoLancieriTotale);
            for (int i = 0; i < 5; i++)
            {
                enemyUnits.Guerrieri[i] -= enemyMortiDistanza.Guerrieri[i];
                enemyUnits.Lancieri[i] -= enemyMortiDistanza.Lancieri[i];
            }

            // Contrattacco a distanza dei barbari: calcolato una sola volta sulla forza nemica totale.
            var barbariDistanzaResult = BattagliaPVE.CalcolaAttaccoDistanzaBarbari(enemyUnits, new BattagliaDistanza());
            var mortiPooledDistanza = ApplicaDanniDistanza_(truppePooled.Clone(), barbariDistanzaResult.Difensore_Danno_Guerrieri, barbariDistanzaResult.Difensore_Danno_Lancieri);

            // Ridistribuzione proporzionale delle perdite (a distanza) tra i partecipanti, tier per tier. Arcieri e
            // Catapulte non subiscono perdite nella fase a distanza (stessa convenzione di BattagliaPVE/PVP: sono
            // "dietro le linee" e vengono colpiti solo nella fase corpo a corpo).
            var truppeDopoDistanza = new Dictionary<string, UnitGroup>();
            foreach (var key in truppeInviate.Keys) truppeDopoDistanza[key] = truppeInviate[key].Clone();

            for (int tier = 0; tier < 5; tier++)
            {
                var pesiGuerrieri = truppeInviate.ToDictionary(kv => kv.Key, kv => kv.Value.Guerrieri[tier]);
                foreach (var kv in DistribuisciProporzionalmente(mortiPooledDistanza.Guerrieri[tier], pesiGuerrieri))
                    truppeDopoDistanza[kv.Key].Guerrieri[tier] -= kv.Value;

                var pesiLancieri = truppeInviate.ToDictionary(kv => kv.Key, kv => kv.Value.Lancieri[tier]);
                foreach (var kv in DistribuisciProporzionalmente(mortiPooledDistanza.Lancieri[tier], pesiLancieri))
                    truppeDopoDistanza[kv.Key].Lancieri[tier] -= kv.Value;
            }

            // ═══ FASE CORPO A CORPO ═══
            var fasiPersonali = new Dictionary<string, RisultatoFase>();
            double dannoAttaccanteTotale = 0;
            foreach (var kv in truppeDopoDistanza)
            {
                var player = giocatori[kv.Key];
                var fasePersonale = new RisultatoFase
                {
                    Fase_Distanza = risultatiDistanza[kv.Key],
                    Struttura = new Villaggio { Nome = $"Guarnigione Città Barbaro Lv.{livello} (Raduno #{idAttacco})", Guarnigione = enemyUnitsIniziali.TotalUnits() }
                };
                fasePersonale.Attaccante.Schierati = truppeInviate[kv.Key];
                bool usaFrecce = enemyUnits.TotalUnits() > 0;
                dannoAttaccanteTotale += BattagliaPVE.CalcolaDannoGiocatore(kv.Value, player, usaFrecce, fasePersonale);
                fasiPersonali[kv.Key] = fasePersonale;
            }

            double dannoBarbariTotale = BattagliaPVE.CalcolaDannoBarbari(enemyUnits);

            // Danno in uscita verso il nemico: condiviso, applicato una volta sola sul pool nemico.
            var faseCondivisaNemico = new RisultatoFase();
            double dannoPerTipoDifensore = dannoAttaccanteTotale / Math.Max(1, enemyUnits.CountUnitTypes());
            BattagliaPVE.ApplicaDanniBarbari(faseCondivisaNemico, enemyUnits.Clone(), dannoPerTipoDifensore);

            // Danno in entrata dai barbari: stesso dannoPerTipo applicato a ciascun partecipante (vedi commento sul
            // metodo).
            double dannoPerTipoAttaccante = dannoBarbariTotale / Math.Max(1, truppePooled.CountUnitTypes());
            foreach (var kv in truppeDopoDistanza)
            {
                var player = giocatori[kv.Key];
                BattagliaPVE.ApplicaDanniAttaccante(fasiPersonali[kv.Key], kv.Value.Clone(), player, dannoPerTipoAttaccante);
            }

            bool vittoria = faseCondivisaNemico.Difensore.Sopravvisuti.TotalUnits() == 0;

            // Aggiorna la guarnigione della Città Barbara (riusa l'helper già corretto in BattagliaPVE, che gestisce
            // anche il bugfix del livello hardcoded).
            BattagliaPVE.AggiornaBarbari(livello, "Città Barbaro", null, faseCondivisaNemico.Difensore.Sopravvisuti);

            // ═══ ESPERIENZA (da combattimento, sempre) ═══
            var perditeBarbareTotali = new UnitGroup();
            for (int i = 0; i < 5; i++)
            {
                perditeBarbareTotali.Guerrieri[i] = enemyMortiDistanza.Guerrieri[i] + faseCondivisaNemico.Difensore.Perdite.Guerrieri[i];
                perditeBarbareTotali.Lancieri[i] = enemyMortiDistanza.Lancieri[i] + faseCondivisaNemico.Difensore.Perdite.Lancieri[i];
                perditeBarbareTotali.Arcieri[i] = faseCondivisaNemico.Difensore.Perdite.Arcieri[i];
                perditeBarbareTotali.Catapulte[i] = faseCondivisaNemico.Difensore.Perdite.Catapulte[i];
            }
            int espCombattimentoTotale = BattagliaPVE.CalcolaEsperienzaPVE(perditeBarbareTotali);
            var pesiContributo = truppeInviate.ToDictionary(kv => kv.Key, kv => kv.Value.TotalUnits());
            var espCombattimentoPerPartecipante = DistribuisciProporzionalmente(espCombattimentoTotale, pesiContributo);

            // ═══ BOTTINO (solo in caso di vittoria) — capacità di trasporto e bottino calcolati per partecipante,
            // in proporzione alla capacità di carico di ciascuno (che dipende dalle proprie truppe sopravvissute e
            // dai propri bonus di ricerca/trasporto). ═══
            var capacitaPerPartecipante = new Dictionary<string, int>();
            foreach (var kv in fasiPersonali)
                capacitaPerPartecipante[kv.Key] = CapacitàCarico(kv.Value.Attaccante.Sopravvisuti, giocatori[kv.Key]);
            int capacitaCaricoTotale = capacitaPerPartecipante.Values.Sum();

            var bottinoTotale = new RisorseRaccolte();
            int espConquistaTotale = 0;
            if (vittoria)
            {
                // Bilanciamento: stesso nerf /5 applicato al saccheggio PVP/PVE (confermato dall'utente).
                int capacitaEffettiva = capacitaCaricoTotale / 3;
                espConquistaTotale = citta.Esperienza;
                bottinoTotale = RaccoliRisorseEquamente(capacitaEffettiva, citta.Cibo, citta.Legno, citta.Pietra, citta.Ferro, citta.Oro, espConquistaTotale, citta.Diamanti_Blu, citta.Diamanti_Viola);

                citta.Cibo -= bottinoTotale.Cibo; citta.Legno -= bottinoTotale.Legno; citta.Pietra -= bottinoTotale.Pietra;
                citta.Ferro -= bottinoTotale.Ferro; citta.Oro -= bottinoTotale.Oro; citta.Esperienza -= espConquistaTotale;
                citta.Diamanti_Blu -= bottinoTotale.Diamanti_Blu; citta.Diamanti_Viola -= bottinoTotale.Diamanti_Viola;
            }

            var bottinoPerPartecipante = capacitaPerPartecipante.Keys.ToDictionary(k => k, k => new RisorseRaccolte());
            void DistribuisciRisorsa(Func<RisorseRaccolte, int> leggi, Action<RisorseRaccolte, int> scrivi)
            {
                foreach (var kv in DistribuisciProporzionalmente(leggi(bottinoTotale), capacitaPerPartecipante))
                    scrivi(bottinoPerPartecipante[kv.Key], kv.Value);
            }
            DistribuisciRisorsa(r => r.Cibo, (r, v) => r.Cibo = v);
            DistribuisciRisorsa(r => r.Legno, (r, v) => r.Legno = v);
            DistribuisciRisorsa(r => r.Pietra, (r, v) => r.Pietra = v);
            DistribuisciRisorsa(r => r.Ferro, (r, v) => r.Ferro = v);
            DistribuisciRisorsa(r => r.Oro, (r, v) => r.Oro = v);
            DistribuisciRisorsa(r => r.Diamanti_Blu, (r, v) => r.Diamanti_Blu = v);
            DistribuisciRisorsa(r => r.Diamanti_Viola, (r, v) => r.Diamanti_Viola = v);
            var espConquistaPerPartecipante = DistribuisciProporzionalmente(espConquistaTotale, capacitaPerPartecipante);

            // ═══ ASSEGNAZIONE FINALE PER PARTECIPANTE: truppe, esperienza, bottino, report ═══
            var riepilogoPartecipanti = new List<PartecipanteRaduno>();
            var reportsPerPartecipante = new Dictionary<string, Report>();

            foreach (var kv in giocatori)
            {
                string username = kv.Key;
                var player = kv.Value;
                var fase = fasiPersonali[username];
                var truppeSchierate = truppeInviate[username];
                var truppeSopravvissute = fase.Attaccante.Sopravvisuti;
                var truppePerse = truppeSchierate.Subtract(truppeSopravvissute);

                // Restituisci le truppe sopravvissute
                for (int i = 0; i < 5; i++)
                {
                    player.Guerrieri[i] += truppeSopravvissute.Guerrieri[i];
                    player.Lanceri[i] += truppeSopravvissute.Lancieri[i];
                    player.Arceri[i] += truppeSopravvissute.Arcieri[i];
                    player.Catapulte[i] += truppeSopravvissute.Catapulte[i];
                }

                int espCombattimento = espCombattimentoPerPartecipante.TryGetValue(username, out var ec) ? ec : 0;
                int espConquista = vittoria && espConquistaPerPartecipante.TryGetValue(username, out var eq) ? eq : 0;
                int espTotalePartecipante = espCombattimento + espConquista;
                var risorseRaccolte = vittoria ? bottinoPerPartecipante[username] : new RisorseRaccolte();

                if (vittoria)
                {
                    player.Cibo += risorseRaccolte.Cibo;
                    player.Legno += risorseRaccolte.Legno;
                    player.Pietra += risorseRaccolte.Pietra;
                    player.Ferro += risorseRaccolte.Ferro;
                    player.Oro += risorseRaccolte.Oro;
                    player.Diamanti_Blu += risorseRaccolte.Diamanti_Blu;
                    player.Diamanti_Viola += risorseRaccolte.Diamanti_Viola;
                    player.Risorse_Razziate += risorseRaccolte.Cibo + risorseRaccolte.Legno + risorseRaccolte.Pietra + risorseRaccolte.Ferro + risorseRaccolte.Oro + risorseRaccolte.Diamanti_Blu + risorseRaccolte.Diamanti_Viola;
                    player.Battaglie_Vinte++;
                    player.Città_Barbare_Sconfitte++;
                }
                else player.Battaglie_Perse++;

                Esperienza.AddExp(player, espTotalePartecipante);

                fase.Vittoria_Attaccante = vittoria;
                fase.Xp_Attaccante = espTotalePartecipante;
                fase.Unità_Presenti_Difensore = enemyUnitsIniziali.TotalUnits() > 0;

                var report = new Report
                {
                    Tipo = "Battaglia",
                    Data = DateTime.UtcNow.ToString(),
                    Aperto = false,
                    Battaglia = new RisultatoBattaglia
                    {
                        Nome_Attaccante = player.Username,
                        Nome_Difensore = $"Città Barbaro Lv.{livello} (Raduno #{idAttacco})",
                        Tipo_Battaglia = "PVE_Raduno",
                        Vittoria_Attaccante = vittoria,
                        Xp_Attaccante = espTotalePartecipante,
                        Forza_Attaccante = CalcolaForza(truppeSchierate),
                        Forza_Attaccante_Finale = CalcolaForza(truppeSopravvissute),
                        Risorse_Raccolte = risorseRaccolte
                    }
                };
                report.Battaglia.Fasi.Add(fase);
                player.Report.Add(report);
                reportsPerPartecipante[username] = report;

                riepilogoPartecipanti.Add(new PartecipanteRaduno
                {
                    Username = username,
                    Truppe_Inviate = truppeSchierate,
                    Truppe_Sopravvissute = truppeSopravvissute,
                    Truppe_Perse = truppePerse,
                    Esperienza_Guadagnata = espTotalePartecipante,
                    Risorse_Raccolte = risorseRaccolte
                });

                if (player.guid_Player != Guid.Empty)
                    Send(player.guid_Player, $"Log_Server|{(vittoria ? "[verde]RADUNO VITTORIOSO!" : "[error]RADUNO FALLITO")} contro Città Barbaro Lv.{livello} (#{idAttacco}) — Esperienza guadagnata: {espTotalePartecipante}");
            }

            // Aggiungi a ogni Report personale il riepilogo completo del raduno (tutti i partecipanti), la Forza
            // difensore complessiva e le Fasi condivise, ora che sono disponibili.
            double forzaDifensoreIniziale = BattagliaPVE.CalcolaForzaBarbari(enemyUnitsIniziali);
            double forzaDifensoreFinale = BattagliaPVE.CalcolaForzaBarbari(faseCondivisaNemico.Difensore.Sopravvisuti);
            foreach (var report in reportsPerPartecipante.Values)
            {
                report.Battaglia.Partecipanti = riepilogoPartecipanti;
                report.Battaglia.Forza_Difensore = forzaDifensoreIniziale;
                report.Battaglia.Forza_Difensore_Finale = forzaDifensoreFinale;
            }

            // Rimuovi l'attacco dalla lista
            AttacchiInCorso.Remove(idAttacco);
            AttacchiInPlayer.Remove(idAttacco);
        }

        // Esegue il raduno contro UN GIOCATORE (22/09/2026, su richiesta dell'utente): le stesse 7 fasi
        // sequenziali dell'attacco PVP singolo (BattagliaPVP.cs — Ingresso, Mura, Cancello, Torri, Città,
        // Castello, Villaggio, ciascuna con la propria Salute/Difesa da abbattere prima della successiva),
        // ma con più attaccanti "pool-ati" contro un solo difensore, invece di un solo attaccante. Stessa
        // filosofia già confermata per i raduni contro le Città Barbare: ogni truppa combatte con le
        // statistiche del SUO proprietario, mai un "giocatore virtuale" con stats mediate/pool.
        //   - Fase a distanza di ogni struttura: il danno in USCITA di ciascun partecipante (proprie frecce/
        //     statistiche) si somma e colpisce il difensore una volta sola; il danno in ENTRATA (unico, dalle
        //     unità a distanza del difensore) è ridistribuito proporzionalmente tra i partecipanti in base a
        //     quante unità di quel tipo/tier ciascuno ha ancora in campo (stesso principio già usato contro
        //     le Città Barbare).
        //   - Fase corpo a corpo di ogni struttura: il danno in USCITA (verso struttura+difensore) si somma e
        //     si applica una volta sola; il danno in ENTRATA dal difensore è lo stesso "dannoPerTipo" applicato
        //     INDIPENDENTEMENTE a ciascun partecipante (la difesa/salute PROPRIA di ognuno scala col proprio
        //     numero di truppe — stesso principio già usato contro le Città Barbare).
        //   - I sopravvissuti PERSONALI di ogni partecipante, fase dopo fase, sono l'attaccante della fase
        //     successiva — esattamente come "attackerUnits = fase precedente" nel PVP singolo.
        //   - SEMPLIFICAZIONE nota (da tenere a mente/testare): a differenza del PVP singolo, qui l'XP e il
        //     bottino non sono calcolati fase per fase ma UNA VOLTA SOLA a fine raduno, dalle perdite totali
        //     accumulate sulle 7 fasi — stesso approccio già usato contro le Città Barbare. I campi Xp_Attaccante/
        //     Xp_Difensore delle singole RisultatoFase nel report personale di ogni partecipante restano quindi
        //     a 0 (dettaglio "per fase" non popolato); il totale complessivo è corretto ed è quello mostrato/
        //     applicato al giocatore.
        private static async Task EseguiBattagliaCooperativaPVP(string idAttacco, Guid clientGuid)
        {
            var attacco = AttacchiInCorso[idAttacco];

            // Ri-verifica il bersaglio al momento dell'esecuzione (non solo alla creazione): può essersi
            // eliminato nel frattempo, oppure aver attivato lo Scudo della Pace dopo la creazione del raduno.
            var difensore = servers_.GetPlayer(attacco.BersaglioUsername);
            if (difensore == null)
            {
                Send(clientGuid, "Log_Server|[error]Il giocatore bersaglio non esiste più: il raduno viene annullato e le truppe restituite.");
                RestituisciTruppeECancella(attacco, "il giocatore bersaglio non esiste più");
                return;
            }
            if (difensore.ScudoDellaPace > 0)
            {
                Send(clientGuid, "Log_Server|[error]Il bersaglio ha attivato lo Scudo della Pace: il raduno viene annullato e le truppe restituite.");
                RestituisciTruppeECancella(attacco, "il bersaglio ha attivato lo Scudo della Pace");
                return;
            }

            // Carica giocatori e truppe inviate da ciascun partecipante (convertite da TruppeContribuite a UnitGroup).
            var giocatori = new Dictionary<string, Player>();
            var truppeInviate = new Dictionary<string, UnitGroup>();
            foreach (var kv in attacco.GiocatoriPartecipanti)
            {
                var player = servers_.GetPlayer(kv.Key);
                if (player == null) continue;
                giocatori[kv.Key] = player;
                truppeInviate[kv.Key] = new UnitGroup
                {
                    Guerrieri = (int[])kv.Value.Guerrieri.Clone(),
                    Lancieri = (int[])kv.Value.Lanceri.Clone(),
                    Arcieri = (int[])kv.Value.Arceri.Clone(),
                    Catapulte = (int[])kv.Value.Catapulte.Clone()
                };
            }

            if (giocatori.Count == 0)
            {
                Send(clientGuid, "Log_Server|[error]Nessun partecipante valido, raduno annullato.");
                AttacchiInCorso.Remove(idAttacco);
                AttacchiInPlayer.Remove(idAttacco);
                return;
            }

            // Stato "corrente" per partecipante, aggiornato fase dopo fase: i sopravvissuti PERSONALI della
            // fase precedente sono l'attaccante della fase successiva (esattamente come nel PVP singolo).
            var truppeCorrenti = truppeInviate.ToDictionary(kv => kv.Key, kv => kv.Value.Clone());

            var fasiCondivise = new List<RisultatoFase>(); // una per struttura (7), per il report del DIFENSORE
            var fasiPersonaliPerPartecipante = new Dictionary<string, List<RisultatoFase>>(); // 7 fasi per ciascun ATTACCANTE
            foreach (var key in giocatori.Keys) fasiPersonaliPerPartecipante[key] = new List<RisultatoFase>();

            // Perdite totali accumulate sulle 7 fasi (per l'XP finale, vedi nota "SEMPLIFICAZIONE" sopra).
            var perditeDifensoreTotali = new UnitGroup();
            var perditeAttaccantiTotali = new UnitGroup(); // somma di tutti i partecipanti, per l'XP del difensore

            RisultatoFase ultimaFaseCondivisa = null;

            for (int struttura = 1; struttura <= 7; struttura++)
            {
                var defenderUnits = BattagliaPVP.CaricaDatiStruttureDifensore(difensore, struttura);
                var faseCondivisa = new RisultatoFase
                {
                    Struttura = struttura switch
                    {
                        1 => new Villaggio { Nome = "Ingresso", Guarnigione = defenderUnits.TotalUnits() },
                        2 => new Villaggio { Nome = "Mura", Salute = difensore.Salute_Mura, SaluteMax = difensore.Salute_MuraMax, Difesa = difensore.Difesa_Mura, DifesaMax = difensore.Difesa_MuraMax, SaluteIniziale = difensore.Salute_Mura, DifesaIniziale = difensore.Difesa_Mura, Guarnigione = defenderUnits.TotalUnits() },
                        3 => new Villaggio { Nome = "Cancello", Salute = difensore.Salute_Cancello, SaluteMax = difensore.Salute_CancelloMax, Difesa = difensore.Difesa_Cancello, DifesaMax = difensore.Difesa_CancelloMax, SaluteIniziale = difensore.Salute_Cancello, DifesaIniziale = difensore.Difesa_Cancello, Guarnigione = defenderUnits.TotalUnits() },
                        4 => new Villaggio { Nome = "Torri", Salute = difensore.Salute_Torri, SaluteMax = difensore.Salute_TorriMax, Difesa = difensore.Difesa_Torri, DifesaMax = difensore.Difesa_TorriMax, SaluteIniziale = difensore.Salute_Torri, DifesaIniziale = difensore.Difesa_Torri, Guarnigione = defenderUnits.TotalUnits() },
                        5 => new Villaggio { Nome = "Centro Villaggio", Guarnigione = defenderUnits.TotalUnits() },
                        6 => new Villaggio { Nome = "Castello", Salute = difensore.Salute_Castello, SaluteMax = difensore.Salute_CastelloMax, Difesa = difensore.Difesa_Castello, DifesaMax = difensore.Difesa_CastelloMax, SaluteIniziale = difensore.Salute_Castello, DifesaIniziale = difensore.Difesa_Castello, Guarnigione = defenderUnits.TotalUnits() },
                        7 => new Villaggio { Nome = "Villaggio", Guarnigione = defenderUnits.TotalUnits() },
                        _ => new Villaggio()
                    }
                };

                // ═══ FASE A DISTANZA (pooled) ═══
                int dannoGuerrieriTotale = 0, dannoLancieriTotale = 0;
                var truppePooled = new UnitGroup();
                var risultatiDistanzaPersonali = new Dictionary<string, BattagliaDistanza>();
                foreach (var kv in truppeCorrenti)
                {
                    var player = giocatori[kv.Key];
                    var result = BattagliaPVP.CalcolaAttaccoDistanza_(kv.Value, player, new BattagliaDistanza(), false);
                    risultatiDistanzaPersonali[kv.Key] = result;
                    dannoGuerrieriTotale += result.Attaccante_Danno_Guerrieri;
                    dannoLancieriTotale += result.Attaccante_Danno_Lancieri;

                    for (int i = 0; i < 5; i++)
                    {
                        truppePooled.Guerrieri[i] += kv.Value.Guerrieri[i];
                        truppePooled.Lancieri[i] += kv.Value.Lancieri[i];
                        truppePooled.Arcieri[i] += kv.Value.Arcieri[i];
                        truppePooled.Catapulte[i] += kv.Value.Catapulte[i];
                    }
                }

                // Danno in uscita verso il difensore (unico, condiviso): applicato una volta sola.
                var defenderMortiDistanza = ApplicaDanniDistanza_(defenderUnits.Clone(), dannoGuerrieriTotale, dannoLancieriTotale);
                var defenderUnitsDopoDistanza = defenderUnits.Clone();
                for (int i = 0; i < 5; i++)
                {
                    defenderUnitsDopoDistanza.Guerrieri[i] -= defenderMortiDistanza.Guerrieri[i];
                    defenderUnitsDopoDistanza.Lancieri[i] -= defenderMortiDistanza.Lancieri[i];
                }

                // Contrattacco a distanza del difensore: calcolato una sola volta sulla sua forza totale in questa struttura.
                var difensoreDistanzaResult = BattagliaPVP.CalcolaAttaccoDistanza_(defenderUnits, difensore, new BattagliaDistanza(), true);
                var mortiPooledDistanza = ApplicaDanniDistanza_(truppePooled.Clone(), difensoreDistanzaResult.Difensore_Danno_Guerrieri, difensoreDistanzaResult.Difensore_Danno_Lancieri);

                // Ridistribuzione proporzionale delle perdite (a distanza) tra i partecipanti, tier per tier.
                // Arcieri e Catapulte non subiscono perdite nella fase a distanza (stessa convenzione già usata
                // contro le Città Barbare/nel PVP singolo: sono "dietro le linee").
                var truppeDopoDistanza = new Dictionary<string, UnitGroup>();
                foreach (var key in truppeCorrenti.Keys) truppeDopoDistanza[key] = truppeCorrenti[key].Clone();

                for (int tier = 0; tier < 5; tier++)
                {
                    var pesiGuerrieri = truppeCorrenti.ToDictionary(kv => kv.Key, kv => kv.Value.Guerrieri[tier]);
                    foreach (var kv in DistribuisciProporzionalmente(mortiPooledDistanza.Guerrieri[tier], pesiGuerrieri))
                        truppeDopoDistanza[kv.Key].Guerrieri[tier] -= kv.Value;

                    var pesiLancieri = truppeCorrenti.ToDictionary(kv => kv.Key, kv => kv.Value.Lancieri[tier]);
                    foreach (var kv in DistribuisciProporzionalmente(mortiPooledDistanza.Lancieri[tier], pesiLancieri))
                        truppeDopoDistanza[kv.Key].Lancieri[tier] -= kv.Value;
                }

                faseCondivisa.Fase_Distanza = new BattagliaDistanza
                {
                    Attaccante_Schierati = truppePooled,
                    Difensore_Schierati = defenderUnits,
                    Attaccante_Danno_Guerrieri = dannoGuerrieriTotale,
                    Attaccante_Danno_Lancieri = dannoLancieriTotale,
                    Difensore_Danno_Guerrieri = difensoreDistanzaResult.Difensore_Danno_Guerrieri,
                    Difensore_Danno_Lancieri = difensoreDistanzaResult.Difensore_Danno_Lancieri,
                    Difensore_Unità_Presenti = defenderUnits.TotalUnits() > 0
                };

                // ═══ FASE CORPO A CORPO (pooled) ═══
                var fasiPersonaliStruttura = new Dictionary<string, RisultatoFase>();
                double dannoAttaccanteTotale = 0;
                var truppePooledDopoDistanza = new UnitGroup();
                bool usaFrecceAttaccante = defenderUnitsDopoDistanza.TotalUnits() > 0 &&
                    (!(struttura == 2 || struttura == 3 || struttura == 4 || struttura == 6) || faseCondivisa.Struttura.Salute > 5);
                foreach (var kv in truppeDopoDistanza)
                {
                    var player = giocatori[kv.Key];
                    var fasePersonale = new RisultatoFase
                    {
                        Fase_Distanza = risultatiDistanzaPersonali[kv.Key],
                        Struttura = faseCondivisa.Struttura
                    };
                    fasePersonale.Attaccante.Schierati = truppeInviate[kv.Key]; // schierati "storici" (invariati per raduno)
                    dannoAttaccanteTotale += BattagliaPVP.CalcolaDannoGiocatore(kv.Value, player, usaFrecceAttaccante, true, fasePersonale);
                    fasiPersonaliStruttura[kv.Key] = fasePersonale;

                    for (int i = 0; i < 5; i++)
                    {
                        truppePooledDopoDistanza.Guerrieri[i] += kv.Value.Guerrieri[i];
                        truppePooledDopoDistanza.Lancieri[i] += kv.Value.Lancieri[i];
                        truppePooledDopoDistanza.Arcieri[i] += kv.Value.Arcieri[i];
                        truppePooledDopoDistanza.Catapulte[i] += kv.Value.Catapulte[i];
                    }
                }

                bool usaFrecceDifensore = truppePooledDopoDistanza.TotalUnits() > 0 && struttura != 1 && struttura != 5;
                double dannoDifensoreTotale = BattagliaPVP.CalcolaDannoGiocatore(defenderUnitsDopoDistanza, difensore, usaFrecceDifensore, false, faseCondivisa);

                // Assorbimento strutture (Mura/Cancello/Torri/Castello): stessa formula di
                // BattagliaPVP.Battaglia_corpo_a_Corpo — parte del danno attaccante viene assorbita da
                // Difesa/Salute della struttura prima di raggiungere le unità difensive.
                double dannotempDifesa = dannoAttaccanteTotale * 0.30;
                double bonusUnità = 1;
                if (struttura != 5 && faseCondivisa.Struttura.Salute > 5)
                {
                    if (dannotempDifesa >= faseCondivisa.Struttura.Difesa)
                    {
                        dannotempDifesa -= faseCondivisa.Struttura.Difesa;
                        dannoAttaccanteTotale -= faseCondivisa.Struttura.Difesa;
                        faseCondivisa.Struttura.Difesa = 0;
                    }
                    else
                    {
                        faseCondivisa.Struttura.Difesa -= (int)dannotempDifesa;
                        dannoAttaccanteTotale -= dannotempDifesa;
                    }
                    double dannotempSalute = (dannoAttaccanteTotale - dannotempDifesa) * 0.20;
                    if (dannotempSalute >= faseCondivisa.Struttura.Salute)
                    {
                        dannotempSalute -= faseCondivisa.Struttura.Salute;
                        dannoAttaccanteTotale -= faseCondivisa.Struttura.Salute;
                        faseCondivisa.Struttura.Salute = 0;
                    }
                    else
                    {
                        faseCondivisa.Struttura.Salute -= (int)dannotempSalute;
                        dannoAttaccanteTotale -= dannotempSalute;
                    }
                }
                if (struttura == 1) // Bonus guarnigione ingresso in base alle truppe presenti in Cancello e Mura (identico al PVP singolo)
                {
                    if (difensore.Guarnigione_Cancello >= difensore.Guarnigione_CancelloMax * 0.40 || difensore.Guarnigione_Mura >= difensore.Guarnigione_MuraMax * 0.40) bonusUnità += 0.10;
                    if (difensore.Guarnigione_Cancello >= difensore.Guarnigione_CancelloMax * 0.80 && difensore.Guarnigione_Mura > difensore.Guarnigione_MuraMax * 0.80) bonusUnità += 0.15;
                    if (difensore.Guarnigione_Cancello == difensore.Guarnigione_CancelloMax && difensore.Guarnigione_Mura == difensore.Guarnigione_MuraMax) bonusUnità += 0.20;
                }

                // Danno in uscita verso il difensore: condiviso, applicato una volta sola.
                double dannoPerTipoDefender = dannoAttaccanteTotale / defenderUnitsDopoDistanza.CountUnitTypes();
                BattagliaPVP.ApplicaDanniGiocatore(faseCondivisa, defenderUnitsDopoDistanza.Clone(), difensore, dannoPerTipoDefender, bonusUnità, false);

                // Danno in entrata dal difensore: stesso "dannoPerTipo" applicato indipendentemente a
                // ciascun partecipante (vedi commento in cima al metodo).
                double dannoPerTipoAttacker = dannoDifensoreTotale / truppePooledDopoDistanza.CountUnitTypes();
                var truppeDopoCorpoACorpo = new Dictionary<string, UnitGroup>();
                foreach (var kv in truppeDopoDistanza)
                {
                    BattagliaPVP.ApplicaDanniGiocatore(fasiPersonaliStruttura[kv.Key], kv.Value.Clone(), giocatori[kv.Key], dannoPerTipoAttacker, 1, true);
                    truppeDopoCorpoACorpo[kv.Key] = fasiPersonaliStruttura[kv.Key].Attaccante.Sopravvisuti;
                    fasiPersonaliPerPartecipante[kv.Key].Add(fasiPersonaliStruttura[kv.Key]);

                    for (int i = 0; i < 5; i++)
                    {
                        perditeAttaccantiTotali.Guerrieri[i] += fasiPersonaliStruttura[kv.Key].Attaccante.Perdite.Guerrieri[i];
                        perditeAttaccantiTotali.Lancieri[i] += fasiPersonaliStruttura[kv.Key].Attaccante.Perdite.Lancieri[i];
                        perditeAttaccantiTotali.Arcieri[i] += fasiPersonaliStruttura[kv.Key].Attaccante.Perdite.Arcieri[i];
                        perditeAttaccantiTotali.Catapulte[i] += fasiPersonaliStruttura[kv.Key].Attaccante.Perdite.Catapulte[i];
                    }
                }
                // Perdite del difensore in questa struttura (a distanza + corpo a corpo), per l'XP finale.
                for (int i = 0; i < 5; i++)
                {
                    perditeDifensoreTotali.Guerrieri[i] += defenderMortiDistanza.Guerrieri[i] + faseCondivisa.Difensore.Perdite.Guerrieri[i];
                    perditeDifensoreTotali.Lancieri[i] += defenderMortiDistanza.Lancieri[i] + faseCondivisa.Difensore.Perdite.Lancieri[i];
                    perditeDifensoreTotali.Arcieri[i] += faseCondivisa.Difensore.Perdite.Arcieri[i];
                    perditeDifensoreTotali.Catapulte[i] += faseCondivisa.Difensore.Perdite.Catapulte[i];
                }

                if ((faseCondivisa.Struttura.Salute == 0 && faseCondivisa.Struttura.Difesa == 0) || defenderUnits.TotalUnits() == 0)
                    faseCondivisa.Vittoria_Attaccante = true;

                BattagliaPVP.AggiornaDatiStruttureDifensore(struttura, difensore, faseCondivisa);

                fasiCondivise.Add(faseCondivisa);
                ultimaFaseCondivisa = faseCondivisa;
                truppeCorrenti = truppeDopoCorpoACorpo;
            }

            bool vittoria = ultimaFaseCondivisa.Vittoria_Attaccante;

            // ═══ ESPERIENZA (da combattimento, sempre — vedi nota "SEMPLIFICAZIONE" in cima al metodo) ═══
            int espCombattimentoTotale = BattagliaPVP.CalcolaEsperienzaPVP(perditeDifensoreTotali);
            var pesiContributo = truppeInviate.ToDictionary(kv => kv.Key, kv => kv.Value.TotalUnits());
            var espCombattimentoPerPartecipante = DistribuisciProporzionalmente(espCombattimentoTotale, pesiContributo);
            int espDifensore = BattagliaPVP.CalcolaEsperienzaPVP(perditeAttaccantiTotali);

            // ═══ BOTTINO (solo in caso di vittoria) — stesse regole del PVP singolo (BattagliaPVP.
            // AssegnaRisorseVittoria_PvP): 50% delle risorse ATTUALI del difensore, capacità di carico
            // nerfata a 1/5, tetto giornaliero sui diamanti sottratti al difensore. Distribuito tra i
            // partecipanti in proporzione alla propria capacità di carico residua (stesso schema già usato
            // per il bottino dei raduni contro le Città Barbare). SEMPLIFICAZIONE: il tetto giornaliero sui
            // diamanti OTTENUTI si applica al TOTALE raccolto (come se fosse un solo attaccante); la quota di
            // ciascun partecipante viene poi limitata al SUO residuo personale (l'eccedenza va persa, non
            // ridistribuita agli altri) — da verificare se questo comportamento ti va bene con più partecipanti.
            var capacitaPerPartecipante = new Dictionary<string, int>();
            foreach (var kv in fasiPersonaliPerPartecipante)
                capacitaPerPartecipante[kv.Key] = CapacitàCarico(truppeCorrenti.TryGetValue(kv.Key, out var soprav) ? soprav : new UnitGroup(), giocatori[kv.Key]) / 5;
            int capacitaCaricoTotale = capacitaPerPartecipante.Values.Sum();

            var bottinoTotale = new RisorseRaccolte();
            if (vittoria && capacitaCaricoTotale > 0)
            {
                double cibo = difensore.Cibo / 2, legno = difensore.Legno / 2, pietra = difensore.Pietra / 2, ferro = difensore.Ferro / 2, oro = difensore.Oro / 2;
                int diamantiVioleResiduiBersaglio = Math.Max(0, Variabili_Server.Max_Diamanti_Viola_PVP_Giocatore - difensore.Diamanti_Viola_PVP_Persi);
                int diamantiBluResiduiBersaglio = Math.Max(0, Variabili_Server.Max_Diamanti_Blu_PVP_Giocatore - difensore.Diamanti_Blu_PVP_Persi);

                bottinoTotale = RaccoliRisorseEquamente(capacitaCaricoTotale, cibo, legno, pietra, ferro, oro, 0, diamantiBluResiduiBersaglio, diamantiVioleResiduiBersaglio);

                difensore.Cibo -= bottinoTotale.Cibo; difensore.Legno -= bottinoTotale.Legno; difensore.Pietra -= bottinoTotale.Pietra;
                difensore.Ferro -= bottinoTotale.Ferro; difensore.Oro -= bottinoTotale.Oro;
                difensore.Diamanti_Blu -= bottinoTotale.Diamanti_Blu; difensore.Diamanti_Viola -= bottinoTotale.Diamanti_Viola;
                difensore.Diamanti_Blu_PVP_Persi += bottinoTotale.Diamanti_Blu;
                difensore.Diamanti_Viola_PVP_Persi += bottinoTotale.Diamanti_Viola;
            }

            var bottinoPerPartecipante = capacitaPerPartecipante.Keys.ToDictionary(k => k, k => new RisorseRaccolte());
            void DistribuisciRisorsa(Func<RisorseRaccolte, int> leggi, Action<RisorseRaccolte, int> scrivi)
            {
                foreach (var kv in DistribuisciProporzionalmente(leggi(bottinoTotale), capacitaPerPartecipante))
                    scrivi(bottinoPerPartecipante[kv.Key], kv.Value);
            }
            DistribuisciRisorsa(r => r.Cibo, (r, v) => r.Cibo = v);
            DistribuisciRisorsa(r => r.Legno, (r, v) => r.Legno = v);
            DistribuisciRisorsa(r => r.Pietra, (r, v) => r.Pietra = v);
            DistribuisciRisorsa(r => r.Ferro, (r, v) => r.Ferro = v);
            DistribuisciRisorsa(r => r.Oro, (r, v) => r.Oro = v);
            DistribuisciRisorsa(r => r.Diamanti_Blu, (r, v) => r.Diamanti_Blu = v);
            DistribuisciRisorsa(r => r.Diamanti_Viola, (r, v) => r.Diamanti_Viola = v);

            // Forza del difensore (iniziale/finale): stessa formula di BattagliaPVP.Battaglia, sommata sulle
            // 7 fasi CONDIVISE — uguale per il report del difensore e per quello di ogni partecipante.
            double forzaDifensoreIniziale = 0, forzaDifensoreFinale = 0;
            foreach (var f in fasiCondivise)
            {
                forzaDifensoreIniziale += CalcolaForza(f.Fase_Distanza.Difensore_Schierati);
                forzaDifensoreFinale += CalcolaForza(f.Difensore.Sopravvisuti);
            }

            // ═══ ASSEGNAZIONE FINALE PER PARTECIPANTE: truppe, esperienza, bottino, report ═══
            var riepilogoPartecipanti = new List<PartecipanteRaduno>();
            var reportsPerPartecipante = new Dictionary<string, Report>();

            foreach (var kv in giocatori)
            {
                string username = kv.Key;
                var player = kv.Value;
                var truppeSchierate = truppeInviate[username];
                var truppeSopravvissute = truppeCorrenti.TryGetValue(username, out var s) ? s : new UnitGroup();
                var truppePerse = truppeSchierate.Subtract(truppeSopravvissute);

                // Restituisci le truppe sopravvissute
                for (int i = 0; i < 5; i++)
                {
                    player.Guerrieri[i] += truppeSopravvissute.Guerrieri[i];
                    player.Lanceri[i] += truppeSopravvissute.Lancieri[i];
                    player.Arceri[i] += truppeSopravvissute.Arcieri[i];
                    player.Catapulte[i] += truppeSopravvissute.Catapulte[i];
                }

                // Un giocatore non risulta "sotto attacco PVP" mentre limita i danni al proprio bottino —
                // la cattura di diamanti al bersaglio è già limitata sopra dal tetto giornaliero DEL
                // BERSAGLIO; qui applichiamo anche il tetto giornaliero personale di CIASCUN partecipante
                // (stessa idea di AssegnaRisorseVittoria_PvP nel PVP singolo, per evitare che un partecipante
                // superi da solo il proprio limite giornaliero anche se il totale del raduno restava nei limiti).
                var risorseRaccolte = vittoria ? bottinoPerPartecipante[username] : new RisorseRaccolte();
                if (vittoria)
                {
                    int violaResiduiAttaccante = Math.Max(0, Variabili_Server.Max_Diamanti_Viola_PVP - player.Diamanti_Viola_PVP_Ottenuti);
                    int bluResiduiAttaccante = Math.Max(0, Variabili_Server.max_Diamanti_Blu_PVP - player.Diamanti_Blu_PVP_Ottenuti);
                    risorseRaccolte.Diamanti_Viola = Math.Min(risorseRaccolte.Diamanti_Viola, violaResiduiAttaccante);
                    risorseRaccolte.Diamanti_Blu = Math.Min(risorseRaccolte.Diamanti_Blu, bluResiduiAttaccante);
                }

                int espCombattimento = espCombattimentoPerPartecipante.TryGetValue(username, out var ec) ? ec : 0;

                if (vittoria)
                {
                    player.Cibo += risorseRaccolte.Cibo;
                    player.Legno += risorseRaccolte.Legno;
                    player.Pietra += risorseRaccolte.Pietra;
                    player.Ferro += risorseRaccolte.Ferro;
                    player.Oro += risorseRaccolte.Oro;
                    player.Diamanti_Blu += risorseRaccolte.Diamanti_Blu;
                    player.Diamanti_Viola += risorseRaccolte.Diamanti_Viola;
                    player.Diamanti_Viola_PVP_Ottenuti += risorseRaccolte.Diamanti_Viola;
                    player.Diamanti_Blu_PVP_Ottenuti += risorseRaccolte.Diamanti_Blu;
                    player.Risorse_Razziate += risorseRaccolte.Cibo + risorseRaccolte.Legno + risorseRaccolte.Pietra + risorseRaccolte.Ferro + risorseRaccolte.Oro + risorseRaccolte.Diamanti_Blu + risorseRaccolte.Diamanti_Viola;
                    player.Battaglie_Vinte++;
                }
                else player.Battaglie_Perse++;
                player.Attacchi_Effettuati_PVP++;

                Esperienza.AddExp(player, espCombattimento);

                var report = new Report
                {
                    Tipo = "Battaglia",
                    Data = DateTime.UtcNow.ToString("o"),
                    Aperto = false,
                    Battaglia = new RisultatoBattaglia
                    {
                        Nome_Attaccante = player.Username,
                        Nome_Difensore = $"{difensore.Username} (Raduno #{idAttacco})",
                        Tipo_Battaglia = "PVP_Raduno",
                        Vittoria_Attaccante = vittoria,
                        Xp_Attaccante = espCombattimento,
                        Forza_Attaccante = CalcolaForza(truppeSchierate),
                        Forza_Attaccante_Finale = CalcolaForza(truppeSopravvissute),
                        Forza_Difensore = forzaDifensoreIniziale,
                        Forza_Difensore_Finale = forzaDifensoreFinale,
                        Risorse_Raccolte = risorseRaccolte,
                        Fasi = fasiPersonaliPerPartecipante[username]
                    }
                };
                player.Report.Add(report);
                reportsPerPartecipante[username] = report;

                riepilogoPartecipanti.Add(new PartecipanteRaduno
                {
                    Username = username,
                    Truppe_Inviate = truppeSchierate,
                    Truppe_Sopravvissute = truppeSopravvissute,
                    Truppe_Perse = truppePerse,
                    Esperienza_Guadagnata = espCombattimento,
                    Risorse_Raccolte = risorseRaccolte
                });

                if (player.guid_Player != Guid.Empty)
                    Send(player.guid_Player, $"Log_Server|{(vittoria ? "[verde]RADUNO VITTORIOSO!" : "[error]RADUNO FALLITO")} contro {difensore.Username} (#{idAttacco}) — Esperienza guadagnata: {espCombattimento}");
            }

            // Aggiorna anche il DIFENSORE (statistiche + il suo Report personale, con le 7 fasi CONDIVISE
            // — quelle con i suoi Schierati/Sopravvisuti/Perdite reali — e il riepilogo dei partecipanti).
            if (vittoria) difensore.Battaglie_Perse++; else difensore.Battaglie_Vinte++;
            difensore.Attacchi_Subiti_PVP++;
            Esperienza.AddExp(difensore, espDifensore);

            var reportDifensore = new Report
            {
                Tipo = "Battaglia",
                Data = DateTime.UtcNow.ToString("o"),
                Aperto = false,
                Battaglia = new RisultatoBattaglia
                {
                    Nome_Attaccante = $"Raduno #{idAttacco} ({giocatori.Count} giocatori)",
                    Nome_Difensore = difensore.Username,
                    Tipo_Battaglia = "PVP_Raduno",
                    Vittoria_Attaccante = vittoria,
                    Xp_Difensore = espDifensore,
                    Forza_Difensore = forzaDifensoreIniziale,
                    Forza_Difensore_Finale = forzaDifensoreFinale,
                    Fasi = fasiCondivise,
                    Partecipanti = riepilogoPartecipanti
                }
            };
            difensore.Report.Add(reportDifensore);
            if (difensore.guid_Player != Guid.Empty)
                Send(difensore.guid_Player, $"Log_Server|{(vittoria ? "[error]Sei stato sconfitto in un raduno!" : "[verde]Hai respinto un raduno!")} (#{idAttacco}, {giocatori.Count} attaccanti)");

            foreach (var report in reportsPerPartecipante.Values)
                report.Battaglia.Partecipanti = riepilogoPartecipanti;

            // Rimuovi l'attacco dalla lista
            AttacchiInCorso.Remove(idAttacco);
            AttacchiInPlayer.Remove(idAttacco);
        }

        // Restituisce le truppe di tutti i partecipanti e cancella l'attacco (usato quando il raduno non può più
        // essere eseguito, es. bersaglio non più disponibile).
        private static void RestituisciTruppeECancella(AttaccoCooperativo attacco, string motivo)
        {
            foreach (var partecipante in attacco.GiocatoriPartecipanti)
            {
                var giocatore = servers_.GetPlayer(partecipante.Key);
                if (giocatore == null) continue;
                for (int i = 0; i < 5; i++)
                {
                    giocatore.Guerrieri[i] += partecipante.Value.Guerrieri[i];
                    giocatore.Lanceri[i] += partecipante.Value.Lanceri[i];
                    giocatore.Arceri[i] += partecipante.Value.Arceri[i];
                    giocatore.Catapulte[i] += partecipante.Value.Catapulte[i];
                }
                if (giocatore.guid_Player != Guid.Empty)
                    Send(giocatore.guid_Player, $"Log_Server|Il raduno #{attacco.IdAttacco} è stato annullato ({motivo}) e le tue truppe sono state restituite.");
            }
            AttacchiInCorso.Remove(attacco.IdAttacco);
            AttacchiInPlayer.Remove(attacco.IdAttacco);
        }

        // Aggiorna lo stato di tutti gli attacchi (da chiamare nel ciclo di gioco)
        public static void AggiornaAttacchi()
        {
            if (AttacchiInCorso.Count() == 0) return;

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
                            var giocatore = servers_.GetPlayer(partecipante.Key);
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
                                            $"Arceri={partecipante.Value.Arceri[livello]}, " +
                                            $"Catapulte={partecipante.Value.Catapulte[livello]}");
                                    }
                                }
                            }
                        }

                        // Rimuovi l'attacco dalla lista
                        AttacchiInCorso.Remove(attaccoCooperativo.IdAttacco);
                        AttacchiInPlayer.Remove(attaccoCooperativo.IdAttacco);
                    }
                    // Aggiorna i partecipanti ogni minuto
                    else if (attaccoCooperativo.TempoRimanente % 60 == 0)
                    {
                        foreach (var partecipante in attaccoCooperativo.GiocatoriPartecipanti)
                        {
                            var giocatore = servers_.GetPlayer(partecipante.Key);
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
                Send(clientGuid, $"Log_Server|Comando non valido. Usa: Raduno|<azione>|<parametri>");
                return;
            }

            switch (msgArgs[3])
            {
                case "Crea":
                    // Formato (esteso il 22/09/2026 su richiesta dell'utente): Raduno|Crea|<tipo>|<bersaglio>|<alleanza>
                    //  - <tipo> = "Barbaro" -> <bersaglio> è il livello (1-20) della Città Barbara.
                    //  - <tipo> = "PVP" -> <bersaglio> è lo username del giocatore da attaccare.
                    // <alleanza> è opzionale (true/false, default false) — raduno riservato all'alleanza del
                    // creatore (Opzione B: finché non esiste un sistema di alleanze, solo il creatore lo vede/usa).
                    if (msgArgs.Length < 6)
                    {
                        Send(clientGuid, $"Log_Server|Parametri insufficienti. Usa: Raduno|Crea|<Barbaro|PVP>|<bersaglio>|<alleanza (opzionale, true/false)>");
                        return;
                    }

                    string tipoBersaglioCrea = msgArgs[4];
                    string bersaglioGrezzoCrea = msgArgs[5];
                    bool alleanza = msgArgs.Length >= 7 && bool.TryParse(msgArgs[6], out bool alleanzaParsed) && alleanzaParsed;

                    if (tipoBersaglioCrea == "Barbaro")
                    {
                        if (!int.TryParse(bersaglioGrezzoCrea, out int livelloTarget) || livelloTarget < 1 || livelloTarget > 20)
                        {
                            Send(clientGuid, $"Log_Server|Livello bersaglio non valido (1-20).");
                            return;
                        }
                        string idNuovoAttaccoBarbaro = CreaAttaccoCooperativo(player.Username, "Barbaro", livelloTarget, null, alleanza);
                        Send(clientGuid, $"Log_Server|Nuovo raduno creato contro Città Barbaro Lv.{livelloTarget}! ID: {idNuovoAttaccoBarbaro}");
                        Send(clientGuid, $"Raduno|Creato|{idNuovoAttaccoBarbaro}");
                    }
                    else if (tipoBersaglioCrea == "PVP")
                    {
                        // Stesse regole del PVP singolo (confermato dall'utente il 22/09/2026), vedi
                        // ServerConnection.Battaglia (case "PVP") e Server.cs (costruzione di Utenti_PVP):
                        // il creatore deve aver sbloccato il PVP e non può avere lo Scudo della Pace attivo;
                        // il bersaglio deve esistere, non essere se stesso, non avere lo Scudo della Pace
                        // attivo ed avere anch'esso sbloccato il PVP.
                        if (player.Livello < Variabili_Server.PVP_Unlock)
                        {
                            Send(clientGuid, $"Log_Server|[error]Devi sbloccare il PVP (livello {Variabili_Server.PVP_Unlock}) per creare un raduno contro un giocatore.");
                            return;
                        }
                        if (player.ScudoDellaPace > 0)
                        {
                            Send(clientGuid, $"Log_Server|[title]Non puoi attaccare un giocatore... Hai lo scudo della pace attivo");
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(bersaglioGrezzoCrea) || bersaglioGrezzoCrea == player.Username)
                        {
                            Send(clientGuid, $"Log_Server|Bersaglio non valido.");
                            return;
                        }
                        var bersaglioPlayer = servers_.GetPlayer(bersaglioGrezzoCrea);
                        if (bersaglioPlayer == null)
                        {
                            Send(clientGuid, $"Log_Server|Giocatore bersaglio non trovato.");
                            return;
                        }
                        if (bersaglioPlayer.ScudoDellaPace > 0)
                        {
                            Send(clientGuid, $"Log_Server|Questo giocatore ha lo Scudo della Pace attivo, non può essere attaccato.");
                            return;
                        }
                        if (bersaglioPlayer.Livello < Variabili_Server.PVP_Unlock)
                        {
                            Send(clientGuid, $"Log_Server|Questo giocatore non ha ancora sbloccato il PVP, non può essere attaccato.");
                            return;
                        }

                        string idNuovoAttaccoPvp = CreaAttaccoCooperativo(player.Username, "PVP", 0, bersaglioPlayer.Username, alleanza);
                        Send(clientGuid, $"Log_Server|Nuovo raduno creato contro il giocatore {bersaglioPlayer.Username}! ID: {idNuovoAttaccoPvp}");
                        Send(clientGuid, $"Raduno|Creato|{idNuovoAttaccoPvp}");
                    }
                    else
                    {
                        Send(clientGuid, $"Log_Server|Tipo di bersaglio non riconosciuto. Usa \"Barbaro\" o \"PVP\".");
                    }
                    break;

                case "Partecipa":
                    // Formato: Raduno|Partecipa|<idAttacco>|<G1>|<G2>|<G3>|<G4>|<G5>|<L1>..<L5>|<A1>..<A5>|<C1>..<C5>
                    // (20 valori, uno per ciascun tipo/tier di unità — esteso il 2026-09-14 dal vecchio formato a 4
                    // valori che supportava solo il tier 1).
                    // BUGFIX (2026-09-22): il controllo era "< 24" ma l'ultimo valore letto sotto è
                    // msgArgs[24] (catapulteTier[4] = msgArgs[20+4]) — con esattamente 24 elementi il
                    // controllo passava comunque e msgArgs[24] lanciava un'eccezione non gestita.
                    if (msgArgs.Length < 25)
                    {
                        Send(clientGuid, $"Log_Server|Parametri insufficienti. Usa: Raduno|Partecipa|<idAttacco>|<G1..G5>|<L1..L5>|<A1..A5>|<C1..C5>");
                        return;
                    }

                    string idAttacco = msgArgs[4];
                    int[] guerrieriTier = new int[5];
                    int[] lancieriTier = new int[5];
                    int[] arcieriTier = new int[5];
                    int[] catapulteTier = new int[5];
                    try
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            guerrieriTier[i] = Convert.ToInt32(msgArgs[5 + i]);
                            lancieriTier[i] = Convert.ToInt32(msgArgs[10 + i]);
                            arcieriTier[i] = Convert.ToInt32(msgArgs[15 + i]);
                            catapulteTier[i] = Convert.ToInt32(msgArgs[20 + i]);
                        }
                    }
                    catch (FormatException)
                    {
                        Send(clientGuid, $"Log_Server|Formato truppe non valido.");
                        return;
                    }

                    await Task.Run(() => PartecipaDiAttacco(idAttacco, player.Username, guerrieriTier, lancieriTier, arcieriTier, catapulteTier, clientGuid));
                    break;

                case "Abbandona":
                    if (msgArgs.Length < 5)
                    {
                        Send(clientGuid, $"Log_Server|Parametri insufficienti. Usa: Raduno|Abbandona|<idAttacco>");
                        return;
                    }

                    string idAttaccoAbbandona = msgArgs[4];
                    await Task.Run(() => AbbandoaDiAttacco(idAttaccoAbbandona, player.Username, clientGuid));
                    break;

                case "Inizia":
                    if (msgArgs.Length < 5)
                    {
                        Send(clientGuid, $"Log_Server|Parametri insufficienti. Usa: Raduno|Inizia|<idAttacco>");
                        return;
                    }

                    string idAttaccoInizio = msgArgs[4];
                    await IniziaAttaccoCooperativo(idAttaccoInizio, clientGuid);
                    break;

                case "Lista":
                    GetListaAttacchi(clientGuid, player.Username);
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
                    totGuerrieri += truppe.Guerrieri.Sum();
                    totLancieri += truppe.Lanceri.Sum();
                    totArcieri += truppe.Arceri.Sum();
                    totCatapulte += truppe.Catapulte.Sum();

                    // Calcola il tempo rimanente in formato leggibile
                    int minuti = attacco.Value.TempoRimanente / 60;
                    int secondi = attacco.Value.TempoRimanente % 60;

                    // Informazioni sull'attacco
                    string descrizioneBersaglioMio = attacco.Value.TipoBersaglio == "PVP"
                        ? $"Giocatore {attacco.Value.BersaglioUsername}"
                        : $"Città Barbaro Lv.{attacco.Value.LivelloTarget}";
                    Send(clientGuid, $"Log_Server|ID: {attacco.Key} - Bersaglio: {descrizioneBersaglioMio} - Creato da: {attacco.Value.CreatoreUsername}");
                    Send(clientGuid, $"Log_Server|Le tue truppe: G:{truppe.Guerrieri.Sum()}, L:{truppe.Lanceri.Sum()}, A:{truppe.Arceri.Sum()}, C:{truppe.Catapulte.Sum()}");

                    // Calcola totale truppe per questo attacco
                    int attaccoTotG = 0, attaccoTotL = 0, attaccoTotA = 0, attaccoTotC = 0;
                    foreach (var part in attacco.Value.GiocatoriPartecipanti)
                    {
                        attaccoTotG += part.Value.Guerrieri.Sum();
                        attaccoTotL += part.Value.Lanceri.Sum();
                        attaccoTotA += part.Value.Arceri.Sum();
                        attaccoTotC += part.Value.Catapulte.Sum();
                    }

                    Send(clientGuid, $"Log_Server|Truppe totali: G:{attaccoTotG}, L:{attaccoTotL}, A:{attaccoTotA}, C:{attaccoTotC}");
                    Send(clientGuid, $"Log_Server|Partecipanti: {attacco.Value.GiocatoriPartecipanti.Count} - Tempo rimanente: {minuti}m {secondi}s");
                    Send(clientGuid, $"Log_Server|-------------------------------------------");
                }

            if (partecipazioniTrovate)
            {
                // Invia anche un riepilogo generale
                Send(clientGuid, $"Log_Server|RIEPILOGO:");
                Send(clientGuid, $"Log_Server|Partecipazione a {totaleAttacchi} raduni");
                Send(clientGuid, $"Log_Server|Totale truppe impegnate: G:{totGuerrieri}, L:{totLancieri}, A:{totArcieri}, C:{totCatapulte}");

                // Invia anche i dati in formato strutturato per l'interfaccia (nome comando unificato a "Raduno")
                Send(clientGuid, $"Raduno|MieiAttacchi|{totaleAttacchi}|{totGuerrieri}|{totLancieri}|{totArcieri}|{totCatapulte}");
            }
            else
            {
                Send(clientGuid, $"Log_Server|Non stai partecipando a nessun raduno.");
                Send(clientGuid, $"Raduno|MieiAttacchi|0|0|0|0|0");
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
