using Microsoft.VisualBasic;
using Server_Strategico.Manager;
using Server_Strategico.ServerData.Moduli;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Variabili_Server;
using static Server_Strategico.Manager.QuestManager;
using static Server_Strategico.Server.Server;

namespace Server_Strategico.Gioco
{
    public class Giocatori
    {
        public class Player
        {
            #region Variabili giocatore
            // Giocatori
            public string Username { get; set; }
            public string Password { get; set; }
            public Guid guid_Player { get; set; }
            public bool Tutorial { get; set; }
            public bool Stato_Giocatore { get; set; } //Giocatore attivo?
            public bool Banned_Giocatore { get; set; } //Giocatore bannato?
            public bool[] Tutorial_Stato { get; set; } = new bool[20];
            public bool[] Tutorial_Premi { get; set; } = new bool[20];
            public bool[] GamePass_Premi { get; set; } = new bool[90];
            public Int16 GamePass_Accessi_Consecutivi { get; set; }


            // Esperienza e VIP
            public int Esperienza { get; set; }
            public int Livello { get; set; }
            public int Punti_Quest { get; set; }
            public bool Vip { get; set; }
            public bool Ricerca_Attiva { get; set; }
            public int Vip_Tempo { get; set; }
            public int GamePass_Base_Tempo { get; set; }
            public int GamePass_Avanzato_Tempo { get; set; }
            public bool GamePass_Base { get; set; }
            public bool GamePass_Avanzato { get; set; }
            public DateTime Last_Login { get; set; }
            // Coda e scudi
            public int Code_Reclutamento { get; set; }
            public int Code_Costruzione { get; set; }
            public int Code_Ricerca { get; set; }
            public int Costruttori { get; set; }
            public int Reclutatori { get; set; }
            public int ScudoDellaPace { get; set; }
            public int limite_Strutture { get; set; }
            // Forza esercito
            public double forza_Esercito { get; set; }
            #region Edifici e Terreni
            // Terreni virtuali
            public int Terreno_Comune { get; set; }
            public int Terreno_NonComune { get; set; }
            public int Terreno_Raro { get; set; }
            public int Terreno_Epico { get; set; }
            public int Terreno_Leggendario { get; set; }

            // Edifici civili
            public int Fattoria { get; set; }
            public int Segheria { get; set; }
            public int CavaPietra { get; set; }
            public int MinieraFerro { get; set; }
            public int MinieraOro { get; set; }
            public int Abitazioni { get; set; }

            // Edifici militari
            public int Workshop_Spade { get; set; }
            public int Workshop_Lance { get; set; }
            public int Workshop_Archi { get; set; }
            public int Workshop_Scudi { get; set; }
            public int Workshop_Armature { get; set; }
            public int Workshop_Frecce { get; set; }
            #endregion
            // Risorse civili
            public double Cibo { get; set; }
            public double Legno { get; set; }
            public double Pietra { get; set; }
            public double Ferro { get; set; }
            public double Oro { get; set; }
            public double Popolazione { get; set; }

            // Risorse speciali
            public int Diamanti_Blu { get; set; }
            public int Diamanti_Viola { get; set; }
            public decimal Dollari_Virtuali { get; set; }

            // Risorse militari
            public double Spade { get; set; }
            public double Lance { get; set; }
            public double Archi { get; set; }
            public double Scudi { get; set; }
            public double Armature { get; set; }
            public double Frecce { get; set; }

            // Statistiche
            #region Stats
            public int Unità_Eliminate { get; set; }
            public int Guerrieri_Eliminati { get; set; }
            public int Lanceri_Eliminati { get; set; }
            public int Arceri_Eliminati { get; set; }
            public int Catapulte_Eliminate { get; set; }

            public int Unità_Perse { get; set; }
            public int Guerrieri_Persi { get; set; }
            public int Lanceri_Persi { get; set; }
            public int Arceri_Persi { get; set; }
            public int Catapulte_Perse { get; set; }
            public int Risorse_Razziate { get; set; }

            public int Strutture_Civili_Costruite { get; set; }
            public int Strutture_Militari_Costruite { get; set; }
            public int Caserme_Costruite { get; set; }

            public int Frecce_Utilizzate { get; set; }
            public int Battaglie_Vinte { get; set; }
            public int Battaglie_Perse { get; set; }
            public int Quest_Completate { get; set; }
            public int Attacchi_Subiti_PVP { get; set; }
            public int Attacchi_Effettuati_PVP { get; set; }

            public int Barbari_Sconfitti { get; set; } //Totale uomini barbari sconfitti (villaggi e città)
            public int Accampamenti_Barbari_Sconfitti { get; set; } //Villaggi barbari sconfitti
            public int Città_Barbare_Sconfitte { get; set; }
            public int Danno_HP_Barbaro { get; set; }
            public int Danno_DEF_Barbaro { get; set; }

            public int Unità_Addestrate { get; set; }
            public int Risorse_Utilizzate { get; set; }
            public int Tempo_Addestramento { get; set; }
            public int Tempo_Costruzione { get; set; }
            public int Tempo_Ricerca { get; set; }
            public int Tempo_Sottratto_Diamanti { get; set; } //Tempo risparmiato usando diamanti

            public int Consumo_Cibo_Esercito { get; set; }
            public int Consumo_Oro_Esercito { get; set; }

            public int Diamanti_Viola_Utilizzati { get; set; }
            public int Diamanti_Blu_Utilizzati { get; set; }
            #endregion

            // Esercito
            public int[] Guerrieri = new int[5];
            public int[] Lanceri = new int[5];
            public int[] Arceri = new int[5];
            public int[] Catapulte = new int[5];

            public int Caserma_Guerrieri { get; set; }
            public int Caserma_Lancieri { get; set; }
            public int Caserma_Arceri { get; set; }
            public int Caserma_Catapulte { get; set; }

            public int GuerrieriMax { get; set; }
            public int LancieriMax { get; set; }
            public int ArceriMax { get; set; }
            public int CatapulteMax { get; set; }

            // Ricerche
            public int Ricerca_Produzione { get; set; }
            public int Ricerca_Costruzione { get; set; }
            public int Ricerca_Addestramento { get; set; }
            public int Ricerca_Popolazione { get; set; }
            public int Ricerca_Trasporto { get; set; }
            public int Ricerca_Riparazione { get; set; }


            //Ricerca Città
            #region Ricerca
            public int Ricerca_Ingresso_Guarnigione { get; set; }
            public int Ricerca_Citta_Guarnigione { get; set; }

            public int Ricerca_Cancello_Livello { get; set; }
            public int Ricerca_Cancello_Salute { get; set; }
            public int Ricerca_Cancello_Difesa { get; set; }
            public int Ricerca_Cancello_Guarnigione { get; set; }

            public int Ricerca_Mura_Livello { get; set; }
            public int Ricerca_Mura_Salute { get; set; }
            public int Ricerca_Mura_Difesa { get; set; }
            public int Ricerca_Mura_Guarnigione { get; set; }

            public int Ricerca_Torri_Livello { get; set; }
            public int Ricerca_Torri_Salute { get; set; }
            public int Ricerca_Torri_Difesa { get; set; }
            public int Ricerca_Torri_Guarnigione { get; set; }

            public int Ricerca_Castello_Livello { get; set; }
            public int Ricerca_Castello_Salute { get; set; }
            public int Ricerca_Castello_Difesa { get; set; }
            public int Ricerca_Castello_Guarnigione { get; set; }
            #endregion

            // Livelli unità
            public int Guerriero_Livello { get; set; }
            public int Guerriero_Salute { get; set; }
            public int Guerriero_Difesa { get; set; }
            public int Guerriero_Attacco { get; set; }

            public int Lancere_Livello { get; set; }
            public int Lancere_Salute { get; set; }
            public int Lancere_Difesa { get; set; }
            public int Lancere_Attacco { get; set; }

            public int Arcere_Livello { get; set; }
            public int Arcere_Salute { get; set; }
            public int Arcere_Difesa { get; set; }
            public int Arcere_Attacco { get; set; }

            public int Catapulta_Livello { get; set; }
            public int Catapulta_Salute { get; set; }
            public int Catapulta_Difesa { get; set; }
            public int Catapulta_Attacco { get; set; }

            public bool[] Riparazioni { get; set; } = new bool[8];

            // Premi
            public bool[] PremiNormali { get; set; } = new bool[20];
            public bool[] PremiVIP { get; set; } = new bool[20];

            // Città - Ingresso
            #region Città
            public int Guarnigione_Ingresso { get; set; }
            public int Guarnigione_IngressoMax { get; set; }
            public int[] Guerrieri_Ingresso { get; set; } = new int[5];
            public int[] Lanceri_Ingresso { get; set; } = new int[5];
            public int[] Arceri_Ingresso { get; set; } = new int[5];
            public int[] Catapulte_Ingresso { get; set; } = new int[5];

            // Cancello
            public int Guarnigione_Cancello { get; set; }
            public int Guarnigione_CancelloMax { get; set; }
            public int[] Guerrieri_Cancello { get; set; } = new int[5];
            public int[] Lanceri_Cancello { get; set; } = new int[5];
            public int[] Arceri_Cancello { get; set; } = new int[5];
            public int[] Catapulte_Cancello { get; set; } = new int[5];
            public int Salute_Cancello { get; set; }
            public int Salute_CancelloMax { get; set; }
            public int Difesa_Cancello { get; set; }
            public int Difesa_CancelloMax { get; set; }

            // Mura
            public int Guarnigione_Mura { get; set; }
            public int Guarnigione_MuraMax { get; set; }
            public int[] Guerrieri_Mura { get; set; } = new int[5];
            public int[] Lanceri_Mura { get; set; } = new int[5];
            public int[] Arceri_Mura { get; set; } = new int[5];
            public int[] Catapulte_Mura { get; set; } = new int[5];
            public int Salute_Mura { get; set; }
            public int Salute_MuraMax { get; set; }
            public int Difesa_Mura { get; set; }
            public int Difesa_MuraMax { get; set; }

            // Torri
            public int Guarnigione_Torri { get; set; }
            public int Guarnigione_TorriMax { get; set; }
            public int[] Guerrieri_Torri { get; set; } = new int[5];
            public int[] Lanceri_Torri { get; set; } = new int[5];
            public int[] Arceri_Torri { get; set; } = new int[5];
            public int[] Catapulte_Torri { get; set; } = new int[5];
            public int Salute_Torri { get; set; }
            public int Salute_TorriMax { get; set; }
            public int Difesa_Torri { get; set; }
            public int Difesa_TorriMax { get; set; }

            // Castello
            public int Guarnigione_Castello { get; set; }
            public int Guarnigione_CastelloMax { get; set; }
            public int[] Guerrieri_Castello { get; set; } = new int[5];
            public int[] Lanceri_Castello { get; set; } = new int[5];
            public int[] Arceri_Castello { get; set; } = new int[5];
            public int[] Catapulte_Castello { get; set; } = new int[5];
            public int Salute_Castello { get; set; }
            public int Salute_CastelloMax { get; set; }
            public int Difesa_Castello { get; set; }
            public int Difesa_CastelloMax { get; set; }

            // Città
            public int Guarnigione_Citta { get; set; }
            public int Guarnigione_CittaMax { get; set; }
            public int[] Guerrieri_Citta { get; set; } = new int[5];
            public int[] Lanceri_Citta { get; set; } = new int[5];
            public int[] Arceri_Citta { get; set; } = new int[5];
            public int[] Catapulte_Citta { get; set; } = new int[5];
            #endregion

            //Bonus
            public double Bonus_Salute_Guerrieri { get; set; }
            public double Bonus_Salute_Lanceri { get; set; }
            public double Bonus_Salute_Arceri { get; set; }
            public double Bonus_Salute_Catapulte { get; set; }
            public double Bonus_Difesa_Guerrieri { get; set; }
            public double Bonus_Difesa_Lanceri { get; set; }
            public double Bonus_Difesa_Arceri { get; set; }
            public double Bonus_Difesa_Catapulte { get; set; }
            public double Bonus_Attacco_Guerrieri { get; set; }
            public double Bonus_Attacco_Lanceri { get; set; }
            public double Bonus_Attacco_Arceri { get; set; }
            public double Bonus_Attacco_Catapulte { get; set; }

            public double Bonus_Salute_Strutture { get; set; }
            public double Bonus_Difesa_Strutture { get; set; }
            public double Bonus_Guarnigione_Strutture { get; set; }

            public double Bonus_Produzione_Risorse { get; set; }
            public double Bonus_Capacità_Trasporto { get; set; }
            public double Bonus_Costruzione { get; set; }
            public double Bonus_Addestramento { get; set; }
            public double Bonus_Ricerca { get; set; }
            public double Bonus_Riparazione { get; set; }

            //Potenza
            public double Potenza_Totale { get; set; }
            public double Potenza_Esercito { get; set; }
            public double Potenza_Strutture { get; set; }
            public double Potenza_Ricerca { get; set; }

            //Limiti giocatore [DD - MM - AA]
            public int Diamanti_Viola_PVP_Ottenuti { get; set; }
            public int Diamanti_Blu_PVP_Ottenuti { get; set; }
            public int Diamanti_Viola_PVP_Persi { get; set; }
            public int Diamanti_Blu_PVP_Persi { get; set; }


            #endregion

            //Quest
            public QuestManager.PlayerQuestProgress QuestProgress { get; set; } = new();
            public List<Gioco.Barbari.VillaggioBarbaro> VillaggiPersonali { get; set; } = new();

            public List<BuildingManagerV2.ConstructionTaskV2> task_Attuale_Costruzioni = new(); // Lista di costruzioni attive... Possono esserci anche edifici in pausa... 
            public Queue<BuildingManagerV2.ConstructionTaskV2> task_Coda_Costruzioni = new(); // Coda globale di attesa (quando tutti gli slot sono occupati)

            public List<UnitManagerV2.UnitTaskV2> task_Attuale_Recutamento = new(); // Lista di costruzioni attive... Possono esserci anche edifici in pausa... 
            public Queue<UnitManagerV2.UnitTaskV2> task_Coda_Recutamento = new(); // Coda globale di attesa (quando tutti gli slot sono occupati)

            public List<ResearchManager.ResearchTask> currentTasks_Research = new(); // Lista dei task attualmente in costruzione (slot globali, max = 1)
            public Queue<ResearchManager.ResearchTask> research_Queue = new(); // Coda globale di attesa (quando tutti gli slot sono occupati)

            public PlayerSnapshot Snapshot = new PlayerSnapshot();

            public readonly object LockCostruzione = new object();
            public readonly object LockReclutamento = new object();

            public Player(string username, string password, Guid guid_Client)
            {
                // Inizializza lista villaggi barbarici
                VillaggiPersonali = new List<Gioco.Barbari.VillaggioBarbaro>();

                Riparazioni = new bool[8];
                PremiNormali = new bool[20];
                PremiVIP = new bool[20];
                Tutorial = true;
                Banned_Giocatore = false;
                Stato_Giocatore = true;
                Tutorial_Stato = new bool[32];
                Tutorial_Premi = new bool[32];
                GamePass_Premi = new bool[90];
                GamePass_Accessi_Consecutivi = 0;
                Last_Login = DateTime.Now.Date;

                //Limiti giocatore [DD]
                Diamanti_Viola_PVP_Ottenuti = 0;
                Diamanti_Blu_PVP_Ottenuti = 0;
                Diamanti_Viola_PVP_Persi = 0;
                Diamanti_Blu_PVP_Persi = 0;

                //Dati Giocatore
                Username = username;
                Password = password;
                guid_Player = guid_Client;
                ScudoDellaPace = 0;
                Costruttori = 0;
                Reclutatori = 0;
                Code_Costruzione = 1;
                Code_Reclutamento = 1;
                Code_Ricerca = 1;
                limite_Strutture = 75;

                Livello = 1;
                Esperienza = 0;
                Punti_Quest = 0;
                Vip = false;
                Vip_Tempo = 0;
                GamePass_Base = false;
                GamePass_Avanzato = false;
                GamePass_Base_Tempo = 0;
                GamePass_Avanzato_Tempo = 0;
                Ricerca_Attiva = false;
                Diamanti_Blu = 0;
                Diamanti_Viola = 0;
                Dollari_Virtuali = 0;
                forza_Esercito = 0;

                //Terreni Virtuali
                Terreno_Comune = 0;
                Terreno_NonComune = 0;
                Terreno_Raro = 0;
                Terreno_Epico = 0;
                Terreno_Leggendario = 0;

                //Strutture Civile
                Fattoria = 0; //Produce cibo
                Segheria = 0;
                CavaPietra = 0;
                MinieraFerro = 0;
                MinieraOro = 0;
                Abitazioni = 0; //Aumenta il numero abitanti/s

                //Strutture Militare
                Workshop_Spade = 0; //Produce Spade
                Workshop_Lance = 0;
                Workshop_Archi = 0;
                Workshop_Scudi = 0;
                Workshop_Armature = 0;
                Workshop_Frecce = 0;

                Caserma_Guerrieri = 0; //Numero Caserme
                Caserma_Lancieri = 0;
                Caserma_Arceri = 0;
                Caserma_Catapulte = 0;

                //Risorse Civile
                Cibo = 0;
                Legno = 0;
                Pietra = 0;
                Ferro = 0;
                Oro = 0;
                Popolazione = 0;

                //Risorse Militare
                Spade = 0;
                Lance = 0;
                Archi = 0;
                Scudi = 0;
                Armature = 0;
                Frecce = 0;

                //Esercito
                for (int i = 0; i < 5; i++)
                {
                    Guerrieri[i] = 0;
                    Lanceri[i] = 0;
                    Arceri[i] = 0;
                    Catapulte[i] = 0;
                }

                //Limite x caserma
                GuerrieriMax = Strutture.Edifici.CasermaGuerrieri.Limite;
                LancieriMax = Strutture.Edifici.CasermaLanceri.Limite;
                ArceriMax = Strutture.Edifici.CasermaArceri.Limite;
                CatapulteMax = Strutture.Edifici.CasermaCatapulte.Limite;

                //Città
                #region Città
                Guarnigione_Ingresso = 0;
                Guarnigione_IngressoMax = Strutture.Edifici.Ingresso.Guarnigione;

                for (int i = 0; i < 5; i++)
                {
                    Guerrieri_Ingresso[i] = 0;
                    Lanceri_Ingresso[i] = 0;
                    Arceri_Ingresso[i] = 0;
                    Catapulte_Ingresso[i] = 0;
                }

                Guarnigione_Cancello = 0;
                Guarnigione_CancelloMax = Strutture.Edifici.Cancello.Guarnigione;
                for (int i = 0; i < 5; i++)
                {
                    Guerrieri_Cancello[i] = 0;
                    Lanceri_Cancello[i] = 0;
                    Arceri_Cancello[i] = 0;
                    Catapulte_Cancello[i] = 0;
                }
                Salute_Cancello = Strutture.Edifici.Cancello.Salute;
                Salute_CancelloMax = Strutture.Edifici.Cancello.Salute;
                Difesa_Cancello = Strutture.Edifici.Cancello.Difesa;
                Difesa_CancelloMax = Strutture.Edifici.Cancello.Difesa;

                Guarnigione_Mura = 0;
                Guarnigione_MuraMax = Strutture.Edifici.Mura.Guarnigione;
                for (int i = 0; i < 5; i++)
                {
                    Guerrieri_Mura[i] = 0;
                    Lanceri_Mura[i] = 0;
                    Arceri_Mura[i] = 0;
                    Catapulte_Mura[i] = 0;
                }
                Salute_Mura = Strutture.Edifici.Mura.Salute;
                Salute_MuraMax = Strutture.Edifici.Mura.Salute;
                Difesa_Mura = Strutture.Edifici.Mura.Difesa;
                Difesa_MuraMax = Strutture.Edifici.Mura.Difesa;

                Guarnigione_Torri = 0;
                Guarnigione_TorriMax = Strutture.Edifici.Torri.Guarnigione;
                for (int i = 0; i < 5; i++)
                {
                    Guerrieri_Torri[i] = 0;
                    Lanceri_Torri[i] = 0;
                    Arceri_Torri[i] = 0;
                    Catapulte_Torri[i] = 0;
                }
                Salute_Torri = Strutture.Edifici.Torri.Salute;
                Salute_TorriMax = Strutture.Edifici.Torri.Salute;
                Difesa_Torri = Strutture.Edifici.Torri.Difesa;
                Difesa_TorriMax = Strutture.Edifici.Torri.Difesa;

                Guarnigione_Castello = 0;
                Guarnigione_CastelloMax = Strutture.Edifici.Castello.Guarnigione;
                for (int i = 0; i < 5; i++)
                {
                    Guerrieri_Castello[i] = 0;
                    Lanceri_Castello[i] = 0;
                    Arceri_Castello[i] = 0;
                    Catapulte_Castello[i] = 0;
                }
                Salute_Castello = Strutture.Edifici.Castello.Salute;
                Salute_CastelloMax = Strutture.Edifici.Castello.Salute;
                Difesa_Castello = Strutture.Edifici.Castello.Difesa;
                Difesa_CastelloMax = Strutture.Edifici.Castello.Difesa;

                Guarnigione_Citta = 0;
                Guarnigione_CittaMax = Strutture.Edifici.Citta.Guarnigione;
                for (int i = 0; i < 5; i++)
                {
                    Guerrieri_Citta[i] = 0;
                    Lanceri_Citta[i] = 0;
                    Arceri_Citta[i] = 0;
                    Catapulte_Citta[i] = 0;
                }
                #endregion

                //Ricerche
                Ricerca_Produzione = 0;
                Ricerca_Costruzione = 0;
                Ricerca_Addestramento = 0;
                Ricerca_Popolazione = 0;
                Ricerca_Trasporto = 0;
                Ricerca_Riparazione = 0;

                //Ricerca CIttà
                Ricerca_Ingresso_Guarnigione = 0;
                Ricerca_Citta_Guarnigione = 0;

                Ricerca_Cancello_Livello = 0;
                Ricerca_Cancello_Salute = 0;
                Ricerca_Cancello_Difesa = 0;
                Ricerca_Cancello_Guarnigione = 0;

                Ricerca_Mura_Livello = 0;
                Ricerca_Mura_Salute = 0;
                Ricerca_Mura_Difesa = 0;
                Ricerca_Mura_Guarnigione = 0;

                Ricerca_Torri_Livello = 0;
                Ricerca_Torri_Salute = 0;
                Ricerca_Torri_Difesa = 0;
                Ricerca_Torri_Guarnigione = 0;

                Ricerca_Castello_Livello = 0;
                Ricerca_Castello_Salute = 0;
                Ricerca_Castello_Difesa = 0;
                Ricerca_Castello_Guarnigione = 0;

                //Livelli unità
                Guerriero_Livello = 0;
                Guerriero_Salute = 0;
                Guerriero_Difesa = 0;
                Guerriero_Attacco = 0;

                Lancere_Livello = 0;
                Lancere_Salute = 0;
                Lancere_Difesa = 0;
                Lancere_Attacco = 0;

                Arcere_Livello = 0;
                Arcere_Salute = 0;
                Arcere_Difesa = 0;
                Arcere_Attacco = 0;

                Catapulta_Livello = 0;
                Catapulta_Salute = 0;
                Catapulta_Difesa = 0;
                Catapulta_Attacco = 0;

                // Statistiche
                Unità_Eliminate = 0;        //Fatto
                Guerrieri_Eliminati = 0;    //Fatto
                Lanceri_Eliminati = 0;      //Fatto
                Arceri_Eliminati = 0;       //Fatto
                Catapulte_Eliminate = 0;    //Fatto
                Unità_Addestrate = 0;       //Fatto

                Unità_Perse = 0;            //Fatto
                Guerrieri_Persi = 0;        //Fatto
                Lanceri_Persi = 0;          //Fatto
                Arceri_Persi = 0;           //Fatto
                Catapulte_Perse = 0;        //Fatto
                Risorse_Razziate = 0;       //Fatto

                Strutture_Civili_Costruite = 0;     //Fatto
                Strutture_Militari_Costruite = 0;   //Fatto
                Caserme_Costruite = 0;              //Fatto

                Frecce_Utilizzate = 0;      //Fatto
                Battaglie_Vinte = 0;        //Fatto
                Battaglie_Perse = 0;        //Fatto         
                Quest_Completate = 0;       //Fatto
                Attacchi_Subiti_PVP = 0;
                Attacchi_Effettuati_PVP = 0;

                Barbari_Sconfitti = 0; //Totale uomini barbari sconfitti (villaggi e città)
                Accampamenti_Barbari_Sconfitti = 0; //Villaggi barbari sconfitti
                Città_Barbare_Sconfitte = 0;     
                Danno_HP_Barbaro = 0;           
                Danno_DEF_Barbaro = 0;          

                Risorse_Utilizzate = 0;         //Fatto
                Tempo_Addestramento = 0;        //Fatto
                Tempo_Costruzione = 0;          //Fatto
                Tempo_Ricerca = 0;              //Fatto
                Tempo_Sottratto_Diamanti = 0;   //Tempo risparmiato usando diamanti

                Consumo_Cibo_Esercito = 0;      //Fatto
                Consumo_Oro_Esercito = 0;       //Fatto
                Diamanti_Viola_Utilizzati = 0;  //Fatto
                Diamanti_Blu_Utilizzati = 0;    //Fatto

                //Bonus
                Bonus_Salute_Guerrieri = 0;
                Bonus_Salute_Lanceri = 0;
                Bonus_Salute_Arceri = 0;
                Bonus_Salute_Catapulte = 0;
                Bonus_Difesa_Guerrieri = 0;
                Bonus_Difesa_Lanceri = 0;
                Bonus_Difesa_Arceri = 0;
                Bonus_Difesa_Catapulte = 0;
                Bonus_Attacco_Guerrieri = 0;
                Bonus_Attacco_Lanceri = 0;
                Bonus_Attacco_Arceri = 0;
                Bonus_Attacco_Catapulte = 0;

                Bonus_Salute_Strutture = 0;
                Bonus_Difesa_Strutture = 0;
                Bonus_Guarnigione_Strutture = 0;

                Bonus_Produzione_Risorse = 0;
                Bonus_Capacità_Trasporto = 0;
                Bonus_Costruzione = 0;
                Bonus_Addestramento = 0;
                Bonus_Ricerca = 0;
                Bonus_Riparazione = 0;
        }

            public bool ValidatePassword(string password)
            {
                return Password == password;
            }
            public void ResetGiornaliero()
            {
                if (Variabili_Server.Reset_Gironaliero)
                {
                    Diamanti_Viola_PVP_Ottenuti = 0;
                    Diamanti_Blu_PVP_Ottenuti = 0;
                    Diamanti_Viola_PVP_Persi = 0;
                    Diamanti_Blu_PVP_Persi = 0;
                    for (int i = 0; i == VillaggiPersonali.Count(); i++)
                        VillaggiPersonali[0].Esplorato = false;
                    for (int i = 0; i == Barbari.CittaGlobali.Count(); i++)
                        Barbari.CittaGlobali[0].Esplorato = false;
                    Variabili_Server.Reset_Gironaliero = false;
                }
            }

            public void ProduceResources() //produzione risorse
            {
                if (Cibo < Fattoria * Strutture.Edifici.Fattoria.Limite)
                    Cibo += Fattoria * (Strutture.Edifici.Fattoria.Produzione + Ricerca_Produzione * Ricerca.Tipi.Incremento.Cibo) * (1 + Bonus_Produzione_Risorse);

                if (Legno < Segheria * Strutture.Edifici.Segheria.Limite)
                    Legno += Segheria * (Strutture.Edifici.Segheria.Produzione + Ricerca_Produzione * Ricerca.Tipi.Incremento.Legno) * (1 + Bonus_Produzione_Risorse);

                if (Pietra < CavaPietra * Strutture.Edifici.CavaPietra.Limite)
                    Pietra += CavaPietra * (Strutture.Edifici.CavaPietra.Produzione + Ricerca_Produzione * Ricerca.Tipi.Incremento.Pietra) * (1 + Bonus_Produzione_Risorse);

                if (Ferro < MinieraFerro * Strutture.Edifici.MinieraFerro.Limite)
                    Ferro += MinieraFerro * (Strutture.Edifici.MinieraFerro.Produzione + Ricerca_Produzione * Ricerca.Tipi.Incremento.Ferro) * (1 + Bonus_Produzione_Risorse);

                if (Oro < MinieraOro * Strutture.Edifici.MinieraOro.Limite)
                    Oro += MinieraOro * (Strutture.Edifici.MinieraOro.Produzione + Ricerca_Produzione * Ricerca.Tipi.Incremento.Oro) * (1 + Bonus_Produzione_Risorse);

                if (Popolazione < Abitazioni * Strutture.Edifici.Case.Limite)
                    Popolazione += Abitazioni * (Strutture.Edifici.Case.Produzione + Ricerca_Popolazione * Ricerca.Tipi.Incremento.Popolazione);

                Dollari_Virtuali += ((decimal)Terreno_Comune * Variabili_Server.Terreni_Virtuali.Comune.Produzione) +
                    ((decimal)Terreno_NonComune * Variabili_Server.Terreni_Virtuali.NonComune.Produzione) +
                    ((decimal)Terreno_Raro * Variabili_Server.Terreni_Virtuali.Raro.Produzione) +
                    ((decimal)Terreno_Epico * Variabili_Server.Terreni_Virtuali.Epico.Produzione) +
                    ((decimal)Terreno_Leggendario * Variabili_Server.Terreni_Virtuali.Leggendario.Produzione);

                if (Spade < Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Limite)
                    Spade += Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Produzione * (1 + Bonus_Produzione_Risorse);

                if (Lance < Workshop_Lance * Strutture.Edifici.ProduzioneLance.Limite)
                    Lance += Workshop_Lance * Strutture.Edifici.ProduzioneLance.Produzione * (1 + Bonus_Produzione_Risorse);

                if (Archi < Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Limite)
                    Archi += Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Produzione * (1 + Bonus_Produzione_Risorse);

                if (Scudi < Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Limite)
                    Scudi += Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Produzione * (1 + Bonus_Produzione_Risorse);

                if (Armature < Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Limite)
                    Armature += Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Produzione * (1 + Bonus_Produzione_Risorse);

                if (Frecce < Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Limite)
                    Frecce += Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Produzione * (1 + Bonus_Produzione_Risorse);
            }
            public void SetupVillaggioGiocatore(Player player)
            {
                // Salva i valori MAX precedenti PRIMA di ricalcolarli
                int oldSalute_CancelloMax = Salute_CancelloMax;
                int oldDifesa_CancelloMax = Difesa_CancelloMax;
                int oldSalute_MuraMax = Salute_MuraMax;
                int oldDifesa_MuraMax = Difesa_MuraMax;
                int oldSalute_TorriMax = Salute_TorriMax;
                int oldDifesa_TorriMax = Difesa_TorriMax;
                int oldSalute_CastelloMax = Salute_CastelloMax;
                int oldDifesa_CastelloMax = Difesa_CastelloMax;

                // Calcola i nuovi valori MAX
                Guarnigione_IngressoMax = (int)((Strutture.Edifici.Ingresso.Guarnigione + (Ricerca_Ingresso_Guarnigione * Strutture.Edifici.Ingresso.Guarnigione)) * (1 + Bonus_Guarnigione_Strutture));
                Guarnigione_CittaMax = (int)((Strutture.Edifici.Citta.Guarnigione + (Ricerca_Citta_Guarnigione * Strutture.Edifici.Citta.Guarnigione)) * (1 + Bonus_Guarnigione_Strutture));
                Guarnigione_CancelloMax = (int)((Strutture.Edifici.Cancello.Guarnigione + (Ricerca_Cancello_Guarnigione * Strutture.Edifici.Cancello.Guarnigione)) * (1 + Bonus_Guarnigione_Strutture));
                Salute_CancelloMax = (int)((Strutture.Edifici.Cancello.Salute + (Ricerca_Cancello_Salute * Strutture.Edifici.Cancello.Salute)) * (1 + Bonus_Salute_Strutture));
                Difesa_CancelloMax = (int)((Strutture.Edifici.Cancello.Difesa + (Ricerca_Cancello_Difesa * Strutture.Edifici.Cancello.Difesa)) * (1 + Bonus_Difesa_Strutture));
                Guarnigione_MuraMax = (int)((Strutture.Edifici.Mura.Guarnigione + (Ricerca_Mura_Guarnigione * Strutture.Edifici.Mura.Guarnigione)) * (1 + Bonus_Guarnigione_Strutture));
                Salute_MuraMax = (int)((Strutture.Edifici.Mura.Salute + (Ricerca_Mura_Salute * Strutture.Edifici.Mura.Salute)) * (1 + Bonus_Salute_Strutture));
                Difesa_MuraMax = (int)((Strutture.Edifici.Mura.Difesa + (Ricerca_Mura_Difesa * Strutture.Edifici.Mura.Difesa)) * (1 + Bonus_Difesa_Strutture));
                Guarnigione_TorriMax = (int)((Strutture.Edifici.Torri.Guarnigione + (Ricerca_Torri_Guarnigione * Strutture.Edifici.Torri.Guarnigione)) * (1 + Bonus_Guarnigione_Strutture));
                Salute_TorriMax = (int)((Strutture.Edifici.Torri.Salute + (Ricerca_Torri_Salute * Strutture.Edifici.Torri.Salute)) * (1 + Bonus_Salute_Strutture));
                Difesa_TorriMax = (int)((Strutture.Edifici.Torri.Difesa + (Ricerca_Torri_Difesa * Strutture.Edifici.Torri.Difesa)) * (1 + Bonus_Difesa_Strutture));
                Guarnigione_CastelloMax = (int)((Strutture.Edifici.Castello.Guarnigione + (Ricerca_Castello_Guarnigione * Strutture.Edifici.Castello.Guarnigione)) * (1 + Bonus_Guarnigione_Strutture));
                Salute_CastelloMax = (int)((Strutture.Edifici.Castello.Salute + (Ricerca_Castello_Salute * Strutture.Edifici.Castello.Salute)) * (1 + Bonus_Salute_Strutture));
                Difesa_CastelloMax = (int)((Strutture.Edifici.Castello.Difesa + (Ricerca_Castello_Difesa * Strutture.Edifici.Castello.Difesa)) * (1 + Bonus_Difesa_Strutture));

                // Aggiorna i valori attuali SOLO se il MAX è aumentato
                // Aggiunge la DIFFERENZA tra nuovo e vecchio MAX
                if (Salute_CancelloMax > oldSalute_CancelloMax)
                    Salute_Cancello += Salute_CancelloMax - oldSalute_CancelloMax;
                if (Difesa_CancelloMax > oldDifesa_CancelloMax)
                    Difesa_Cancello += Difesa_CancelloMax - oldDifesa_CancelloMax;

                if (Salute_MuraMax > oldSalute_MuraMax)
                    Salute_Mura += Salute_MuraMax - oldSalute_MuraMax;
                if (Difesa_MuraMax > oldDifesa_MuraMax)
                    Difesa_Mura += Difesa_MuraMax - oldDifesa_MuraMax;

                if (Salute_TorriMax > oldSalute_TorriMax)
                    Salute_Torri += Salute_TorriMax - oldSalute_TorriMax;
                if (Difesa_TorriMax > oldDifesa_TorriMax)
                    Difesa_Torri += Difesa_TorriMax - oldDifesa_TorriMax;

                if (Salute_CastelloMax > oldSalute_CastelloMax)
                    Salute_Castello += Salute_CastelloMax - oldSalute_CastelloMax;
                if (Difesa_CastelloMax > oldDifesa_CastelloMax)
                    Difesa_Castello += Difesa_CastelloMax - oldDifesa_CastelloMax;

                Server.Server.GameServer.GuerrieriCitta(player);
            }
            public void SetupCaserme()
            {
                GuerrieriMax = Caserma_Guerrieri * Strutture.Edifici.CasermaGuerrieri.Limite;
                LancieriMax = Caserma_Lancieri * Strutture.Edifici.CasermaLanceri.Limite;
                ArceriMax = Caserma_Arceri * Strutture.Edifici.CasermaArceri.Limite;
                CatapulteMax = Caserma_Catapulte * Strutture.Edifici.CasermaCatapulte.Limite;
            }
            public void ManutenzioneEsercito() //produzione risorse
            {
                double cibo = 0, legno = 0, pietra = 0, ferro = 0, oro = 0;

                double[] guerriero_Cibo = new double[5];
                double[] lancere_cibo = new double[5];
                double[] arcere_cibo = new double[5];
                double[] catapulta_cibo = new double[5];

                double[] guerriero_oro = new double[5];
                double[] lancere_oro = new double[5];
                double[] arcere_oro = new double[5];
                double[] catapulta_oro = new double[5];

                guerriero_Cibo[0] = Esercito.Unità.Guerriero_1.Cibo;
                guerriero_Cibo[1] = Esercito.Unità.Guerriero_2.Cibo;
                guerriero_Cibo[2] = Esercito.Unità.Guerriero_3.Cibo;
                guerriero_Cibo[3] = Esercito.Unità.Guerriero_4.Cibo;
                guerriero_Cibo[4] = Esercito.Unità.Guerriero_5.Cibo;

                lancere_cibo[0] = Esercito.Unità.Lancere_1.Cibo;
                lancere_cibo[1] = Esercito.Unità.Lancere_2.Cibo;
                lancere_cibo[2] = Esercito.Unità.Lancere_3.Cibo;
                lancere_cibo[3] = Esercito.Unità.Lancere_4.Cibo;
                lancere_cibo[4] = Esercito.Unità.Lancere_5.Cibo;

                arcere_cibo[0] = Esercito.Unità.Arcere_1.Cibo;
                arcere_cibo[1] = Esercito.Unità.Arcere_2.Cibo;
                arcere_cibo[2] = Esercito.Unità.Arcere_3.Cibo;
                arcere_cibo[3] = Esercito.Unità.Arcere_4.Cibo;
                arcere_cibo[4] = Esercito.Unità.Arcere_5.Cibo;

                catapulta_cibo[0] = Esercito.Unità.Catapulta_1.Cibo;
                catapulta_cibo[1] = Esercito.Unità.Catapulta_2.Cibo;
                catapulta_cibo[2] = Esercito.Unità.Catapulta_3.Cibo;
                catapulta_cibo[3] = Esercito.Unità.Catapulta_4.Cibo;
                catapulta_cibo[4] = Esercito.Unità.Catapulta_5.Cibo;

                guerriero_oro[0] = Esercito.Unità.Guerriero_1.Salario;
                guerriero_oro[1] = Esercito.Unità.Guerriero_2.Salario;
                guerriero_oro[2] = Esercito.Unità.Guerriero_3.Salario;
                guerriero_oro[3] = Esercito.Unità.Guerriero_4.Salario;
                guerriero_oro[4] = Esercito.Unità.Guerriero_5.Salario;

                lancere_oro[0] = Esercito.Unità.Lancere_1.Salario;
                lancere_oro[1] = Esercito.Unità.Lancere_2.Salario;
                lancere_oro[2] = Esercito.Unità.Lancere_3.Salario;
                lancere_oro[3] = Esercito.Unità.Lancere_4.Salario;
                lancere_oro[4] = Esercito.Unità.Lancere_5.Salario;

                arcere_oro[0] = Esercito.Unità.Arcere_1.Salario;
                arcere_oro[1] = Esercito.Unità.Arcere_2.Salario;
                arcere_oro[2] = Esercito.Unità.Arcere_3.Salario;
                arcere_oro[3] = Esercito.Unità.Arcere_4.Salario;
                arcere_oro[4] = Esercito.Unità.Arcere_5.Salario;

                catapulta_oro[0] = Esercito.Unità.Catapulta_1.Salario;
                catapulta_oro[1] = Esercito.Unità.Catapulta_2.Salario;
                catapulta_oro[2] = Esercito.Unità.Catapulta_3.Salario;
                catapulta_oro[3] = Esercito.Unità.Catapulta_4.Salario;
                catapulta_oro[4] = Esercito.Unità.Catapulta_5.Salario;

                for (int i = 0; i < 5; i++) //Manutenzione esercito
                {
                    cibo += Guerrieri[i] * guerriero_Cibo[i] + Lanceri[i] * lancere_cibo[i] + Arceri[i] * arcere_cibo[i] + Catapulte[i] * catapulta_cibo[i];

                    cibo += Guerrieri_Ingresso[i] * guerriero_Cibo[i] + Lanceri_Ingresso[i] * lancere_cibo[i] + Arceri_Ingresso[i] * arcere_cibo[i] + Catapulte_Ingresso[i] * catapulta_cibo[i];
                    cibo += Guerrieri_Cancello[i] * guerriero_Cibo[i] + Lanceri_Cancello[i] * lancere_cibo[i] + Arceri_Cancello[i] * arcere_cibo[i] + Catapulte_Cancello[i] * catapulta_cibo[i];
                    cibo += Guerrieri_Mura[i] * guerriero_Cibo[i] + Lanceri_Mura[i] * lancere_cibo[i] + Arceri_Mura[i] * arcere_cibo[i] + Catapulte_Mura[i] * catapulta_cibo[i];
                    cibo += Guerrieri_Torri[i] * guerriero_Cibo[i] + Lanceri_Torri[i] * lancere_cibo[i] + Arceri_Torri[i] * arcere_cibo[i] + Catapulte_Torri[i] * catapulta_cibo[i];
                    cibo += Guerrieri_Castello[i] * guerriero_Cibo[i] + Lanceri_Castello[i] * lancere_cibo[i] + Arceri_Castello[i] * arcere_cibo[i] + Catapulte_Castello[i] * catapulta_cibo[i];
                    cibo += Guerrieri_Citta[i] * guerriero_Cibo[i] + Lanceri_Citta[i] * lancere_cibo[i] + Arceri_Citta[i] * arcere_cibo[i] + Catapulte_Citta[i] * catapulta_cibo[i];

                    oro += Guerrieri[i] * guerriero_oro[i] + Lanceri[i] * lancere_oro[i] + Arceri[i] * arcere_oro[i] + Catapulte[i] * catapulta_oro[i];
                    oro += Guerrieri_Ingresso[i] * guerriero_oro[i] + Lanceri_Ingresso[i] * lancere_oro[i] + Arceri_Ingresso[i] * arcere_oro[i] + Catapulte_Ingresso[i] * catapulta_oro[i];
                    oro += Guerrieri_Cancello[i] * guerriero_oro[i] + Lanceri_Cancello[i] * lancere_oro[i] + Arceri_Cancello[i] * arcere_oro[i] + Catapulte_Cancello[i] * catapulta_oro[i];
                    oro += Guerrieri_Mura[i] * guerriero_oro[i] + Lanceri_Mura[i] * lancere_oro[i] + Arceri_Mura[i] * arcere_oro[i] + Catapulte_Mura[i] * catapulta_oro[i];
                    oro += Guerrieri_Torri[i] * guerriero_oro[i] + Lanceri_Torri[i] * lancere_oro[i] + Arceri_Torri[i] * arcere_oro[i] + Catapulte_Torri[i] * catapulta_oro[i];
                    oro += Guerrieri_Castello[i] * guerriero_oro[i] + Lanceri_Castello[i] * lancere_oro[i] + Arceri_Castello[i] * arcere_oro[i] + Catapulte_Castello[i] * catapulta_oro[i];
                    oro += Guerrieri_Citta[i] * guerriero_oro[i] + Lanceri_Citta[i] * lancere_oro[i] + Arceri_Citta[i] * arcere_oro[i] + Catapulte_Citta[i] * catapulta_oro[i];
                }
                // Aggiorna statistiche consumo esercito -- per non creare altre variabili...
                Consumo_Cibo_Esercito += (int)cibo;
                Consumo_Oro_Esercito += (int)oro;

                //Manutenzione edifici militari
                cibo += Caserma_Guerrieri * Strutture.Edifici.CasermaGuerrieri.Consumo_Cibo;
                cibo += Caserma_Lancieri * Strutture.Edifici.CasermaLanceri.Consumo_Cibo;
                cibo += Caserma_Arceri * Strutture.Edifici.CasermaArceri.Consumo_Cibo;
                cibo += Caserma_Catapulte * Strutture.Edifici.CasermaCatapulte.Consumo_Cibo;

                legno += Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Consumo_Legno;
                ferro += Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Consumo_Ferro;
                oro += Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Consumo_Oro;

                legno += Workshop_Lance * Strutture.Edifici.ProduzioneLance.Consumo_Legno;
                ferro += Workshop_Lance * Strutture.Edifici.ProduzioneLance.Consumo_Ferro;
                oro += Workshop_Lance * Strutture.Edifici.ProduzioneLance.Consumo_Oro;

                legno += Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Consumo_Legno;
                oro += Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Consumo_Oro;

                legno += Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Consumo_Legno;
                ferro += Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Consumo_Ferro;
                oro += Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Consumo_Oro;

                ferro += Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Consumo_Ferro;
                oro += Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Consumo_Oro;

                legno += Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Consumo_Legno;
                pietra += Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Consumo_Pietra;
                ferro += Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Consumo_Ferro;
                oro += Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Consumo_Oro;

                Cibo -= cibo * 5;
                Legno -= legno * 5;
                Pietra -= pietra * 5;
                Ferro -= ferro * 5;
                Oro -= oro * 5;

                if (Cibo <= 0) Cibo = 0;
                if (Legno <= 0) Legno = 0;
                if (Pietra <= 0) Pietra = 0;
                if (Ferro <= 0) Ferro = 0;
                if (Oro <= 0) Oro = 0;
            }
            public void ServerTimer() //Timers del server
            {
                if (Vip_Tempo > 0) Vip_Tempo--;
                if (Vip_Tempo == 0) Vip = false;
                if (GamePass_Base_Tempo > 0) GamePass_Base_Tempo--;
                if (GamePass_Base_Tempo == 0) GamePass_Base = false;
                if (GamePass_Avanzato_Tempo > 0) GamePass_Avanzato_Tempo--;
                if (GamePass_Avanzato_Tempo == 0) GamePass_Avanzato = false;
                
                if (ScudoDellaPace > 0) ScudoDellaPace--;

                if (Costruttori > 0) Costruttori--;
                if (Vip == false && Costruttori == 0) Code_Costruzione = 1;
                if (Vip == true && Costruttori == 0) Code_Costruzione = 2;
                if (Vip == true && Costruttori > 0) Code_Costruzione = 3;

                if (Reclutatori > 0) Reclutatori--;
                if (GamePass_Base == false && Reclutatori == 0) Code_Reclutamento = 1;
                if (GamePass_Base == true && Reclutatori == 0) Code_Reclutamento = 2;
                if (GamePass_Base == true && Reclutatori > 0) Code_Reclutamento = 3;
            }
            public async Task BonusPacchetti()
            {
                double bonusAttack = 0, bonusHealth = 0, bonusDefense = 0;

                if (Vip)
                {
                    bonusAttack = 0.02; bonusHealth = 0.02; bonusDefense = 0.02;

                    // + 1 Costruttori fino alla scadenza.

                    Bonus_Attacco_Guerrieri = bonusAttack;
                    Bonus_Attacco_Lanceri = bonusAttack;
                    Bonus_Attacco_Arceri = bonusAttack;
                    Bonus_Attacco_Catapulte = bonusAttack;

                    Bonus_Salute_Guerrieri = bonusHealth;
                    Bonus_Salute_Lanceri = bonusHealth;
                    Bonus_Salute_Arceri = bonusHealth;
                    Bonus_Salute_Catapulte = bonusHealth;

                    Bonus_Difesa_Guerrieri = bonusDefense;
                    Bonus_Difesa_Lanceri = bonusDefense;
                    Bonus_Difesa_Arceri = bonusDefense;
                    Bonus_Difesa_Catapulte = bonusDefense;

                    Bonus_Salute_Strutture = 0.02;
                    Bonus_Difesa_Strutture = 0.02;
                    Bonus_Guarnigione_Strutture = 0.02;
                }
                if (GamePass_Base)
                {
                    bonusAttack = 0.04; bonusHealth = 0.04; bonusDefense = 0.04;

                    // + 1 Reclutatori fino alla scadenza.

                    Bonus_Costruzione = 0.02;
                    Bonus_Addestramento = 0.02;

                    Bonus_Attacco_Guerrieri = bonusAttack;
                    Bonus_Attacco_Lanceri = bonusAttack;
                    Bonus_Attacco_Arceri = bonusAttack;
                    Bonus_Attacco_Catapulte = bonusAttack;

                    Bonus_Salute_Guerrieri = bonusHealth;
                    Bonus_Salute_Lanceri = bonusHealth;
                    Bonus_Salute_Arceri = bonusHealth;
                    Bonus_Salute_Catapulte = bonusHealth;

                    Bonus_Difesa_Guerrieri = bonusDefense;
                    Bonus_Difesa_Lanceri = bonusDefense;
                    Bonus_Difesa_Arceri = bonusDefense;
                    Bonus_Difesa_Catapulte = bonusDefense;

                    Bonus_Salute_Strutture = 0.04;
                    Bonus_Difesa_Strutture = 0.04;
                    Bonus_Guarnigione_Strutture = 0.04;
                }
                if (GamePass_Avanzato)
                {
                    bonusAttack = 6; bonusHealth = 0.06; bonusDefense = 0.06;

                    Bonus_Costruzione = 0.04;
                    Bonus_Addestramento = 0.04;
                    Bonus_Ricerca = 0.02;
                    Bonus_Riparazione = 0.02;
                    Bonus_Produzione_Risorse = 0.04;
                    Bonus_Capacità_Trasporto = 0.04;

                    Bonus_Attacco_Guerrieri = bonusAttack;
                    Bonus_Attacco_Lanceri = bonusAttack;
                    Bonus_Attacco_Arceri = bonusAttack;
                    Bonus_Attacco_Catapulte = bonusAttack;

                    Bonus_Salute_Guerrieri = bonusHealth;
                    Bonus_Salute_Lanceri = bonusHealth;
                    Bonus_Salute_Arceri = bonusHealth;
                    Bonus_Salute_Catapulte = bonusHealth;

                    Bonus_Difesa_Guerrieri = bonusDefense;
                    Bonus_Difesa_Lanceri = bonusDefense;
                    Bonus_Difesa_Arceri = bonusDefense;
                    Bonus_Difesa_Catapulte = bonusDefense;

                    Bonus_Salute_Strutture = 0.06;
                    Bonus_Difesa_Strutture = 0.06;
                    Bonus_Guarnigione_Strutture = 0.06;
                }
            }
            public string FormatTime(double seconds)
            {
                var ts = TimeSpan.FromSeconds(seconds);
                int days = ts.Days;
                int hours = ts.Hours;
                int minutes = ts.Minutes;
                int secs = ts.Seconds;

                string result = "";
                if (days > 0) result += $"{days}d ";
                if (hours > 0 || days > 0) result += $"{hours}h ";
                if (minutes > 0 || hours > 0 || days > 0) result += $"{minutes}m ";
                result += $"{secs}s";

                return result.Trim();
            }
        }
        
    }
}

