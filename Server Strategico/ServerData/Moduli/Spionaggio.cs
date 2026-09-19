using Server_Strategico.Gioco;
using Server_Strategico.ServerData.Moduli.Battaglie;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.ServerData.Moduli.Battaglie.Battaglia;

namespace Server_Strategico.ServerData.Moduli
{
    internal class Spionaggio
    {
        static float valForza = 45f;
        static int costo_Spionaggio_Villaggio_Base = 238;
        static int costo_Spionaggio_Citta_Base = 3328;
        static int costo_Spionaggio_Player_Base = 35800;

        public async static void SpionaggioPVP(Giocatori.Player difensore, Giocatori.Player attaccante)
        {
            var report = new Battaglia.Report();
            Battaglia.SpionaggioFase fase = null;

            report.Tipo = "Spionaggio";
            report.Data = DateTime.UtcNow.ToString("o"); // formato ISO 8601 ("o"): DateTime.UtcNow.ToString() senza formato usa la cultura del server e new Date(...) in JS non lo interpreta, quindi la data spariva dalla lista Report nel client
            report.Aperto = false;
            report.Spionaggio = new Battaglia.RisultatoSpionaggio();
            report.Spionaggio.Tipo_Battaglia = "PVP"; // si spia sempre un altro giocatore, mai un barbaro: "PVE" era un copia-incolla dal template battaglia
            report.Spionaggio.Giocatore.Nome = difensore.Username;
            report.Spionaggio.Giocatore.Esperienza = difensore.Esperienza;
            report.Spionaggio.Giocatore.Livello = difensore.Livello;

            int forza = CalcolaValoreSpionaggio(attaccante, difensore);
            int precisione = CalcolaPrecisioneSpionaggio(forza);
            int livello = CalcolaLivelloSpionaggio(forza);
            /// Servono 13 punti di differenza per raggiungere LV: 6, con precisione: 812

            var spy = report.Spionaggio;
            report.Spionaggio.Forza_Spionaggio = forza; // era attaccante.Ricerca_Spionaggio: mostrava la ricerca grezza invece della forza netta (già scontato il Contro-Spionaggio del difensore), la stessa usata per calcolare precisione e stadio
            report.Spionaggio.Stadio = livello;
            // 14/09/2026, richiesto dall'utente: quando lo stadio è sufficiente a mostrare una categoria (es.
            // Caserme) ma la precisione non basta a dare il valore esatto (quindi "????"/range al posto del
            // numero), il client deve poter avvisare "aumenta la forza per migliorare la precisione" — una
            // volta sola per report, non per ogni singolo valore. Calcolato qui (unica fonte di verità sulla
            // soglia 900), il client si limita a leggere questo bool.
            report.Spionaggio.Precisione_Insufficiente = precisione < 900;

            for (int i = 0; i <= 6; i++) spy.Fasi.Add(new SpionaggioFase()); //Aggiunge le fasi vuote da popolare

            // Ogni livello include tutto quello del livello precedente
            if (livello >= 2) // Le truppe per fase si caricano solo se abbiamo livello sufficiente
                for (int i = 0; i <= 6; i++)
                {
                    var defenderUnits = BattagliaPVP.CaricaDatiStruttureDifensore(difensore, i + 1);
                    Load_Truppe(defenderUnits, spy, precisione, i);
                }
            if (livello <= 0)
            {
                report.Spionaggio.Spionaggio_Riuscito = false;
            }
            else
                report.Spionaggio.Spionaggio_Riuscito = true;
            if (livello >= 1)
            {
                Load_Risorse_Civili(difensore, spy);
                Load_Risorse_Militari(difensore, spy);
            }
            if (livello >= 2)
                Load_Villaggio(difensore, spy, precisione);

            if (livello >= 3)
            {
                Load_Edifici_Civili(difensore, spy, precisione);
                Load_Edifici_Militari(difensore, spy, precisione);
                Load_Caserme(difensore, spy, precisione);
            }
            if (livello >= 4)
                Load_Ricerca_Civile(difensore, spy);

            if (livello >= 5)
                Load_Ricerca_Militare(difensore, spy);

            if (livello >= 6)
            {
                Load_Bonus(difensore, spy);
                Load_Truppe_Stats(difensore, spy);
            }

            attaccante.Report.Add(report);
        }

        public async static void EseguiSpionaggioPVP(Giocatori.Player difensore, Giocatori.Player attaccante, string modalità)
        {
            //Aggiornare valore guarnigione... farlo sempre non conviene...
            Server.Server.GameServer.GuerrieriCitta(difensore);
            Server.Server.GameServer.GuerrieriCitta(attaccante);

            //Costo in risorse per lo spionaggio: [ORO]

            //if (attaccante.Oro >= costo_Spionaggio_Player_Base)
            //{
            //    attaccante.Oro -= costo_Spionaggio_Player_Base;
            //    SpionaggioPVP(difensore, attaccante); //Spionaggio
            //}
            //else
            //{
            //    Server.Server.Send(attaccante.guid_Player, "Log_Server|Oro insufficiente per lo spionaggio.");
            //    return;
            //}
            SpionaggioPVP(difensore, attaccante); //Spionaggio
        }

        // 16/09/2026: overload PVE — spionaggio contro Città/Villaggi Barbari. A differenza del PVP non c'è un Player "difensore" (GuerrieriCitta non si applica:
        // i barbari non hanno una guarnigione distribuita per struttura, solo i 4 totali piatti su BarbarianBase).
        public async static void EseguiSpionaggioPVE(Barbari.BarbarianBase target, Giocatori.Player attaccante)
        {
            //Costo in risorse per lo spionaggio: [ORO] — vedi nota sopra, stesso discorso del PVP
            int costo = 0;
            string name = "";
            if (target.Nome.Contains("Villaggio"))
            {
                costo = costo_Spionaggio_Villaggio_Base * target.Livello;
                name = "Villaggio Barbaro";
            }
            if (target.Nome.Contains("Citta"))
            {
                costo = costo_Spionaggio_Villaggio_Base * target.Livello;
                name = "Citta Barbaro";
            }
            
            if (attaccante.Oro >= costo)
            {
                attaccante.Oro -= costo;
                SpionaggioPVE(target, attaccante);
            }
            else
                Server.Server.Send(attaccante.guid_Player, $"Log_Server|Oro insufficiente per lo spionaggio. Costo richiesto: [icon:oro]{costo}");

            SpionaggioPVE(target, attaccante);
        }
        public async static void SpionaggioPVE(Barbari.BarbarianBase target, Giocatori.Player attaccante)
        {
            var report = new Battaglia.Report();

            report.Tipo = "Spionaggio";
            report.Data = DateTime.UtcNow.ToString("o"); // vedi nota in SpionaggioPVP sul formato ISO 8601
            report.Aperto = false;
            report.Spionaggio = new Battaglia.RisultatoSpionaggio();
            // Il client distingue un report di spionaggio PVP da uno PVE-barbaro (e, tra i due barbari, Città globale da Villaggio personale) leggendo Tipo_Battaglia.
            report.Spionaggio.Tipo_Battaglia = target.IsGlobal ? "PVE_Citta" : "PVE_Villaggio";

            // Riusiamo DatiGiocatore per identificare il bersaglio barbaro (non un vero giocatore).
            report.Spionaggio.Giocatore.Nome = target.Nome;
            report.Spionaggio.Giocatore.Livello = target.Livello;
            report.Spionaggio.Giocatore.Esperienza = target.Esperienza;

            int forza = Math.Max(0, attaccante.Ricerca_Spionaggio - target.Contro_Spionaggio);
            int precisione = CalcolaPrecisioneSpionaggio(forza);
            int livello = CalcolaLivelloSpionaggio(forza);

            var spy = report.Spionaggio;
            spy.Forza_Spionaggio = forza;
            spy.Stadio = livello;
            spy.Precisione_Insufficiente = precisione < 900;
            spy.Spionaggio_Riuscito = livello > 0;

            // Un barbaro non ha edifici, ricerche o bonus da spiare: queste sezioni della struttura
            // esistente (pensata per un Player) non si applicano e restano null, come richiesto.
            spy.Strutture_Civili = null;
            spy.Workshop = null;
            spy.Caserme = null;
            spy.Ricerca_Civile = null;
            spy.Ricerca_Militare = null;
            spy.Bonus = null;
            spy.Risorse_Militari = null; // i barbari non producono risorse militari lavorate (Spade/Lance/Archi/...)

            // Le "Fasi" ricalcano lo scontro reale contro un barbaro (2 fasi: a distanza, corpo a corpo — vedi Battaglie.Battaglia_Distanza),
            // non le 7 fasi strutturali (Ingresso/Mura/ Cancello/Torri/...) dello spionaggio PVP tra giocatori, che per un barbaro non hanno un vero corrispettivo difensivo.
            spy.Fasi = new List<SpionaggioFase> { new SpionaggioFase(), new SpionaggioFase() };

            // Struttura.Nome è usato dal client SIA come etichetta del tab SIA come titolo della
            // sezione (vedi 14-battaglia.js, renderSpiaFaseContent) — qui non è il nome del
            // bersaglio ma il nome della fase stessa. Salute/Difesa del bersaglio (19/09/2026, su
            // richiesta dell'utente) sono ora mostrate SOLO sulla fase Corpo a Corpo: sono la vera
            // Salute/Difesa del barbaro (Barbari.cs), che ora la battaglia usa davvero (Battaglia_Fase
            // in BattagliaPVE.cs) ma soltanto lì — la fase A Distanza non le tocca mai, quindi
            // mostrarle anche lì avrebbe fatto pensare che contassero anche a distanza.
            // I campi di SpionaggioVillaggio non pertinenti ai barbari (ricerche, guarnigione) restano ai valori di default: qui non esistono equivalenti da mostrare.
            spy.Fasi[0].Struttura = new SpionaggioVillaggio
            {
                Nome = "A Distanza",
            };
            spy.Fasi[1].Struttura = new SpionaggioVillaggio
            {
                Nome = "Corpo a Corpo",
                Salute = target.Salute,
                Difesa = target.Difesa
            };

            if (livello >= 2) // Le truppe si vedono solo da stadio 2 in su, come nel PVP
            {
                // Fase 0 = A Distanza (Arcieri + Catapulte)
                ApplicaPrecisione(spy.Fasi[0].Arcieri[0], target.Arcieri, precisione);
                ApplicaPrecisione(spy.Fasi[0].Catapulte[0], target.Catapulte, precisione);

                // Fase 1 = Corpo a Corpo (Guerrieri + Lancieri)
                ApplicaPrecisione(spy.Fasi[1].Guerrieri[0], target.Guerrieri, precisione);
                ApplicaPrecisione(spy.Fasi[1].Lanceri[0], target.Lancieri, precisione);
            }

            if (livello >= 1)
            {
                spy.Risorse_Civili.Cibo = target.Cibo;
                spy.Risorse_Civili.Legno = target.Legno;
                spy.Risorse_Civili.Pietra = target.Pietra;
                spy.Risorse_Civili.Ferro = target.Ferro;
                spy.Risorse_Civili.Oro = target.Oro;
                // Popolazione non esiste per i barbari: resta 0 (non applicabile).

                spy.Risorse_Speciali.Diamanti_Viola = target.Diamanti_Viola;
                spy.Risorse_Speciali.Diamanti_Blu = target.Diamanti_Blu;
            }

            attaccante.Report.Add(report);
        }
        public async static void EseguiSpionaggioTEST()
        {
            bool test1 = await Server.ServerConnection.New_Player("adlos", "123", "adly@example.com", Guid.Empty);
            var attaccante = Server.Server.servers_.GetPlayer("adlos");
            attaccante.Ricerca_Spionaggio = 14;

            bool test2 = await Server.ServerConnection.New_Player("TEST", "123", "test@example.com", Guid.Empty);
            var difensore = Server.Server.servers_.GetPlayer("TEST");
            difensore.Ricerca_Contro_Spionaggio = 1;

            BattagliaPVP.AddTroops(difensore);
            BattagliaPVP.AddTroops(attaccante);

            //Aggiornare valore guarnigione... farlo sempre non conviene...
            Server.Server.GameServer.GuerrieriCitta(difensore);
            Server.Server.GameServer.GuerrieriCitta(attaccante);

            SpionaggioPVP(difensore, attaccante);
        }

        // ─── Calcoli ────────────────────────────────────────────────────────────────

        public static int CalcolaValoreSpionaggio(Giocatori.Player attaccante, Giocatori.Player difensore)
        {
            return Math.Max(0, attaccante.Ricerca_Spionaggio - difensore.Ricerca_Contro_Spionaggio);
        }

        /// <summary>
        /// Ogni punto di forza vale 62.5 punti di precisione, cap a 1000.
        /// Forza >= 20 garantisce il valore esatto (precisione >= 900).
        /// </summary>
        public static int CalcolaPrecisioneSpionaggio(int forza)
        {
            if (forza <= 0) return 0;
            return Math.Min((int)(forza * valForza), 1000);
        }

        /// <summary>
        /// Converte la forza in livello di sblocco (0–6).
        /// Determina COSA si vede, indipendentemente dalla precisione.
        /// </summary>
        public static int CalcolaLivelloSpionaggio(int forza)
        {
            return forza switch
            {
                <= 0 => 0,  // Fallito
                <= 2 => 1,  // Risorse civili + militari
                <= 4 => 2,  // + Truppe
                <= 6 => 3,  // + Villaggio / difese
                <= 9 => 4,  // + Strutture civili + militari + caserme
                <= 12 => 5,  // + Ricerche
                _ => 6   // + Bonus / tutto
            };
        }

        /// <summary>
        /// Dato un valore reale e la precisione (0–1000), restituisce Min e Max.
        /// Il seed è deterministico: stessa precisione + stesso valore = stesso range.
        /// </summary>
        public static (int Min, int Max) CalcolaRangeSpionaggio(int valoreReale, int precisione)
        {
            float erroreMax = 1.0f - (precisione / 1000.0f);
            float erroreMin = erroreMax * 0.5f;

            int seed = precisione * 17 + valoreReale;
            var rng = new Random(seed);
            float errore = erroreMin + (float)rng.NextDouble() * (erroreMax - erroreMin);

            int min = Math.Max(0, (int)(valoreReale * (1f - errore)));
            int max = (int)(valoreReale * (1f + errore));

            return (min, max);
        }

        /// <summary>
        /// Precisione >= 900 → popola Reale (valore esatto).
        /// Precisione < 900  → popola Min e Max (stima).
        /// Il client mostra Reale se > 0, altrimenti il range.
        /// </summary>
        public static void ApplicaPrecisione(TripleValue target, int valoreReale, int precisione)
        {
            if (precisione >= 900)
            {
                target.Reale = valoreReale;
            }
            else
            {
                var (min, max) = CalcolaRangeSpionaggio(valoreReale, precisione);
                target.Min = min;
                target.Max = max;
                target.Reale = -1;
            }
        }

        // ─── Loader ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Valori strutturali del villaggio: visibili fisicamente dall'esterno, sempre esatti.
        /// </summary>
        public static void Load_Villaggio(Giocatori.Player difensore, RisultatoSpionaggio spionaggio, int precisione)
        {
            if (spionaggio.Fasi.Count == 7)
            {
                spionaggio.Fasi[0].Struttura = new SpionaggioVillaggio { 
                    Nome = "Ingresso",
                    Guarnigione = new TripleValue
                    {
                        Min = 0,
                        Reale = difensore.Guarnigione_Ingresso,
                        Max = 0
                    },
                    Guarnigione_Max = difensore.Guarnigione_IngressoMax,
                    Ricerca_Guarnigione = difensore.Ricerca_Ingresso_Guarnigione
                };
                spionaggio.Fasi[1].Struttura = new SpionaggioVillaggio
                {
                    Nome = "Mura",
                    Guarnigione = new TripleValue
                    {
                        Min = 0,
                        Reale = difensore.Guarnigione_Mura,
                        Max = 0
                    },
                    Guarnigione_Max = difensore.Guarnigione_MuraMax,
                    Salute = difensore.Salute_Mura,
                    SaluteMax = difensore.Salute_MuraMax,
                    Difesa = difensore.Difesa_Mura,
                    DifesaMax = difensore.Difesa_MuraMax,
                    Ricerca_Salute = difensore.Ricerca_Mura_Salute,
                    Ricerca_Difesa = difensore.Ricerca_Mura_Difesa,
                    Ricerca_Guarnigione = difensore.Ricerca_Mura_Guarnigione,
                    Ricerca_Livello = difensore.Ricerca_Mura_Livello
                };
                spionaggio.Fasi[2].Struttura = new SpionaggioVillaggio
                {
                    Nome = "Cancello",
                    Guarnigione = new TripleValue
                    {
                        Min = 0,
                        Reale = difensore.Guarnigione_Cancello,
                        Max = 0
                    },
                    Guarnigione_Max = difensore.Guarnigione_CancelloMax,
                    Salute = difensore.Salute_Cancello,
                    SaluteMax = difensore.Difesa_CancelloMax,
                    Difesa = difensore.Difesa_Cancello,
                    DifesaMax = difensore.Difesa_CancelloMax,
                    Ricerca_Salute = difensore.Ricerca_Cancello_Salute,
                    Ricerca_Difesa = difensore.Ricerca_Cancello_Difesa,
                    Ricerca_Guarnigione = difensore.Ricerca_Cancello_Guarnigione,
                    Ricerca_Livello = difensore.Ricerca_Cancello_Livello
                };
                spionaggio.Fasi[3].Struttura = new SpionaggioVillaggio
                {
                    Nome = "Torri",
                    Guarnigione = new TripleValue
                    {
                        Min = 0,
                        Reale = difensore.Guarnigione_Torri,
                        Max = 0
                    },
                    Guarnigione_Max = difensore.Guarnigione_TorriMax,
                    Salute = difensore.Salute_Torri,
                    SaluteMax = difensore.Salute_TorriMax,
                    Difesa = difensore.Difesa_Torri,
                    DifesaMax = difensore.Difesa_TorriMax,
                    Ricerca_Salute = difensore.Ricerca_Torri_Salute,
                    Ricerca_Difesa = difensore.Ricerca_Torri_Difesa,
                    Ricerca_Guarnigione = difensore.Ricerca_Torri_Guarnigione,
                    Ricerca_Livello = difensore.Ricerca_Torri_Livello
                };
                spionaggio.Fasi[4].Struttura = new SpionaggioVillaggio 
                { 
                    Nome = "Centro Villaggio",
                    Guarnigione = new TripleValue
                    {
                        Min = 0,
                        Reale = difensore.Guarnigione_Citta,
                        Max = 0
                    },
                    Guarnigione_Max = difensore.Guarnigione_CittaMax 
                };
                spionaggio.Fasi[5].Struttura = new SpionaggioVillaggio
                {
                    Nome = "Castello",
                    Guarnigione = new TripleValue
                    {
                        Min = 0,
                        Reale = difensore.Guarnigione_Castello,
                        Max = 0
                    },
                    Guarnigione_Max = difensore.Guarnigione_CastelloMax,
                    Salute = difensore.Salute_Castello,
                    SaluteMax = difensore.Salute_CastelloMax,
                    Difesa = difensore.Difesa_Castello,
                    DifesaMax = difensore.Difesa_CastelloMax,
                    Ricerca_Salute = difensore.Ricerca_Castello_Salute,
                    Ricerca_Difesa = difensore.Ricerca_Castello_Difesa,
                    Ricerca_Guarnigione = difensore.Ricerca_Castello_Guarnigione,
                    Ricerca_Livello = difensore.Ricerca_Castello_Livello
                };
                int guarnigione = difensore.Guerrieri.Sum() + difensore.Lanceri.Sum() + difensore.Arceri.Sum() + difensore.Catapulte.Sum();
                int guarnigioneMax = difensore.Caserma_Guerrieri * Strutture.Edifici.CasermaGuerrieri.Limite +
                                    difensore.Caserma_Lancieri * Strutture.Edifici.CasermaLanceri.Limite +
                                    difensore.Caserma_Arceri * Strutture.Edifici.CasermaArceri.Limite +
                                    difensore.Caserma_Catapulte * Strutture.Edifici.CasermaCatapulte.Limite;
                spionaggio.Fasi[6].Struttura = new SpionaggioVillaggio 
                { 
                    Nome = "Villaggio",
                    Guarnigione = new TripleValue
                    {
                        Min = 0,
                        Reale = guarnigione,
                        Max = 0
                    },
                    Guarnigione_Max = guarnigioneMax
                };
            }

        }

        public static void Load_Risorse_Civili(Giocatori.Player difensore, RisultatoSpionaggio spionaggio)
        {
            spionaggio.Risorse_Civili.Cibo = (int)difensore.Cibo;
            spionaggio.Risorse_Civili.Legno = (int)difensore.Legno;
            spionaggio.Risorse_Civili.Pietra = (int)difensore.Pietra;
            spionaggio.Risorse_Civili.Ferro = (int)difensore.Ferro;
            spionaggio.Risorse_Civili.Oro = (int)difensore.Oro;
            spionaggio.Risorse_Civili.Popolazione = (int)difensore.Popolazione;
        }

        public static void Load_Risorse_Militari(Giocatori.Player difensore, RisultatoSpionaggio spionaggio)
        {
            spionaggio.Risorse_Militari.Spade = (int)difensore.Spade;
            spionaggio.Risorse_Militari.Lance = (int)difensore.Lance;
            spionaggio.Risorse_Militari.Archi = (int)difensore.Archi;
            spionaggio.Risorse_Militari.Scudi = (int)difensore.Scudi;
            spionaggio.Risorse_Militari.Armature = (int)difensore.Armature;
            spionaggio.Risorse_Militari.Frecce = (int)difensore.Frecce;
        }

        public static void Load_Edifici_Civili(Giocatori.Player difensore, RisultatoSpionaggio spionaggio, int precisione)
        {
            ApplicaPrecisione(spionaggio.Strutture_Civili.Fattoria, difensore.Fattoria, precisione);
            ApplicaPrecisione(spionaggio.Strutture_Civili.Segheria, difensore.Segheria, precisione);
            ApplicaPrecisione(spionaggio.Strutture_Civili.Cava, difensore.CavaPietra, precisione);
            ApplicaPrecisione(spionaggio.Strutture_Civili.Miniera_Ferrro, difensore.MinieraFerro, precisione);
            ApplicaPrecisione(spionaggio.Strutture_Civili.Miniera_Oro, difensore.MinieraOro, precisione);
            ApplicaPrecisione(spionaggio.Strutture_Civili.Abitazioni, difensore.Abitazioni, precisione);
        }

        public static void Load_Edifici_Militari(Giocatori.Player difensore, RisultatoSpionaggio spionaggio, int precisione)
        {
            ApplicaPrecisione(spionaggio.Workshop.Spade, difensore.Workshop_Spade, precisione);
            ApplicaPrecisione(spionaggio.Workshop.Lance, difensore.Workshop_Lance, precisione);
            ApplicaPrecisione(spionaggio.Workshop.Archi, difensore.Workshop_Archi, precisione);
            ApplicaPrecisione(spionaggio.Workshop.Scudi, difensore.Workshop_Scudi, precisione);
            ApplicaPrecisione(spionaggio.Workshop.Armature, difensore.Workshop_Armature, precisione);
            ApplicaPrecisione(spionaggio.Workshop.Frecce, difensore.Workshop_Frecce, precisione);
        }

        public static void Load_Caserme(Giocatori.Player difensore, RisultatoSpionaggio spionaggio, int precisione)
        {
            ApplicaPrecisione(spionaggio.Caserme.Guerrieri, difensore.Caserma_Guerrieri, precisione);
            ApplicaPrecisione(spionaggio.Caserme.Lanceri, difensore.Caserma_Lancieri, precisione);
            ApplicaPrecisione(spionaggio.Caserme.Arcieri, difensore.Caserma_Arceri, precisione);
            ApplicaPrecisione(spionaggio.Caserme.Catapulte, difensore.Caserma_Catapulte, precisione);
        }
        public static void Load_Truppe(UnitGroup difensore, RisultatoSpionaggio spionaggio, int precisione, int fase)
        {
            for (int i = 0; i <= 4; i++)
            {
                ApplicaPrecisione(spionaggio.Fasi[fase].Guerrieri[i], difensore.Guerrieri[i], precisione);
                ApplicaPrecisione(spionaggio.Fasi[fase].Lanceri[i], difensore.Lancieri[i], precisione);
                ApplicaPrecisione(spionaggio.Fasi[fase].Arcieri[i], difensore.Arcieri[i], precisione);
                ApplicaPrecisione(spionaggio.Fasi[fase].Catapulte[i], difensore.Catapulte[i], precisione);
            }
        }
        public static void Load_Truppe_Stats(Giocatori.Player difensore, RisultatoSpionaggio spionaggio)
        {
            for (int i = 0; i <= 4; i++)
            {
                var stats = Battaglia.GetPlayerUnitStats(i, difensore);

                spionaggio.Stats_Unità.Guerrieri[i].Salute = (int)stats.GuerrieriSalute;
                spionaggio.Stats_Unità.Guerrieri[i].Difesa = (int)stats.GuerrieriDifesa;
                spionaggio.Stats_Unità.Guerrieri[i].Attacco = (int)stats.GuerrieriAttacco;

                spionaggio.Stats_Unità.Lanceri[i].Salute = (int)stats.LancieriSalute;
                spionaggio.Stats_Unità.Lanceri[i].Difesa = (int)stats.LancieriDifesa;
                spionaggio.Stats_Unità.Lanceri[i].Attacco = (int)stats.LancieriAttacco;

                spionaggio.Stats_Unità.Arcieri[i].Salute = (int)stats.ArcieriSalute;
                spionaggio.Stats_Unità.Arcieri[i].Difesa = (int)stats.ArcieriDifesa;
                spionaggio.Stats_Unità.Arcieri[i].Attacco = (int)stats.ArcieriAttacco;

                spionaggio.Stats_Unità.Catapulte[i].Salute = (int)stats.CatapulteSalute;
                spionaggio.Stats_Unità.Catapulte[i].Difesa = (int)stats.CatapulteDifesa;
                spionaggio.Stats_Unità.Catapulte[i].Attacco = (int)stats.CatapulteAttacco;
            }
        }
        public static void Load_Bonus(Giocatori.Player difensore, RisultatoSpionaggio spionaggio)
        {
            spionaggio.Bonus.Guerrieri.Salute = difensore.Bonus_Salute_Guerrieri;
            spionaggio.Bonus.Guerrieri.Difesa = difensore.Bonus_Difesa_Guerrieri;
            spionaggio.Bonus.Guerrieri.Attacco = difensore.Bonus_Attacco_Guerrieri;

            spionaggio.Bonus.Lanceri.Salute = difensore.Bonus_Salute_Lanceri;
            spionaggio.Bonus.Lanceri.Difesa = difensore.Bonus_Difesa_Lanceri;
            spionaggio.Bonus.Lanceri.Attacco = difensore.Bonus_Attacco_Lanceri;

            spionaggio.Bonus.Arceri.Salute = difensore.Bonus_Salute_Arceri;
            spionaggio.Bonus.Arceri.Difesa = difensore.Bonus_Difesa_Arceri;
            spionaggio.Bonus.Arceri.Attacco = difensore.Bonus_Attacco_Arceri;

            spionaggio.Bonus.Catapulte.Salute = difensore.Bonus_Salute_Catapulte;
            spionaggio.Bonus.Catapulte.Difesa = difensore.Bonus_Difesa_Catapulte;
            spionaggio.Bonus.Catapulte.Attacco = difensore.Bonus_Attacco_Catapulte;

            spionaggio.Bonus.Salute_Strutture = difensore.Bonus_Salute_Strutture;
            spionaggio.Bonus.Difesa_Strutture = difensore.Bonus_Difesa_Strutture;
            spionaggio.Bonus.Guarnigione_Strutture = difensore.Bonus_Guarnigione_Strutture;

            spionaggio.Bonus.Produzione_Risorse = difensore.Bonus_Produzione_Risorse;
            spionaggio.Bonus.Costruzione = difensore.Bonus_Costruzione;
            spionaggio.Bonus.Addestramento = difensore.Bonus_Addestramento;
            spionaggio.Bonus.Capacità_Trasporto = difensore.Bonus_Capacità_Trasporto;
            spionaggio.Bonus.Ricerca = difensore.Bonus_Ricerca;
            spionaggio.Bonus.Spionaggio = difensore.Bonus_Spionaggio;
            spionaggio.Bonus.Contro_Spionaggio = difensore.Bonus_Contro_Spionaggio;
        }
        public static void Load_Ricerca_Militare(Giocatori.Player difensore, RisultatoSpionaggio spionaggio)
        {
            spionaggio.Ricerca_Militare.Guerrieri.Salute = difensore.Guerriero_Salute;
            spionaggio.Ricerca_Militare.Guerrieri.Difesa = difensore.Guerriero_Difesa;
            spionaggio.Ricerca_Militare.Guerrieri.Attacco = difensore.Guerriero_Attacco;
            spionaggio.Ricerca_Militare.Guerrieri.Livello = difensore.Guerriero_Livello;

            spionaggio.Ricerca_Militare.Lanceri.Salute = difensore.Lancere_Salute;
            spionaggio.Ricerca_Militare.Lanceri.Difesa = difensore.Lancere_Difesa;
            spionaggio.Ricerca_Militare.Lanceri.Attacco = difensore.Lancere_Attacco;
            spionaggio.Ricerca_Militare.Lanceri.Livello = difensore.Lancere_Livello;

            spionaggio.Ricerca_Militare.Arcieri.Salute = difensore.Arcere_Salute;
            spionaggio.Ricerca_Militare.Arcieri.Difesa = difensore.Arcere_Difesa;
            spionaggio.Ricerca_Militare.Arcieri.Attacco = difensore.Arcere_Attacco;
            spionaggio.Ricerca_Militare.Arcieri.Livello = difensore.Arcere_Livello;

            spionaggio.Ricerca_Militare.Catapulte.Salute = difensore.Catapulta_Salute;
            spionaggio.Ricerca_Militare.Catapulte.Difesa = difensore.Catapulta_Difesa;
            spionaggio.Ricerca_Militare.Catapulte.Attacco = difensore.Catapulta_Attacco;
            spionaggio.Ricerca_Militare.Catapulte.Livello = difensore.Catapulta_Livello;
        }
        public static void Load_Ricerca_Civile(Giocatori.Player difensore, RisultatoSpionaggio spionaggio)
        {
            spionaggio.Ricerca_Civile.Produzione = difensore.Ricerca_Produzione;
            spionaggio.Ricerca_Civile.Costruzione = difensore.Ricerca_Costruzione;
            spionaggio.Ricerca_Civile.Addestramento = difensore.Ricerca_Addestramento;
            spionaggio.Ricerca_Civile.Popolazione = difensore.Ricerca_Popolazione;

            spionaggio.Ricerca_Civile.Trasporto = difensore.Ricerca_Trasporto;
            spionaggio.Ricerca_Civile.Riparazione = difensore.Ricerca_Riparazione;
            spionaggio.Ricerca_Civile.Spionaggio = difensore.Ricerca_Spionaggio;
            spionaggio.Ricerca_Civile.Contro_Spionaggio = difensore.Ricerca_Contro_Spionaggio;
        }
    }
}