using System;
using System.Text.Json;
using static Server_Strategico.BuildingManager;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Variabili_Server;
using static Server_Strategico.QuestManager;
using static Server_Strategico.Server.Server;

namespace Server_Strategico.Gioco
{
    public class Dati
    {
        public static string Difficoltà = "1";
        public static string Versione = "0.1.3";
        public static string Server = "Italy";

        public static double forza_Esercito_Att_PVP = 0;
    }

    public class Giocatori
    {
        public class Barbari
        {
            public int Guerrieri { get; set; }
            public int Lancieri { get; set; }
            public int Arceri { get; set; }
            public int Catapulte { get; set; }
            public int Livello { get; set; }
            public static Barbari PVP = new Barbari
            {
                Guerrieri = 0,
                Lancieri = 0,
                Arceri = 0,
                Catapulte = 0
            };
        }
        public class Player
        {
            #region Variabili giocatore
            //Quest
            public PlayerQuestProgress QuestProgress { get; set; } = new();
            public List<Gioco.Barbari.VillaggioBarbaro> VillaggiPersonali { get; set; } = new();

            // Giocatori
            public string Username { get; set; }
            public string Password { get; set; }
            public Guid guid_Player { get; set; }

            // Esperienza e VIP
            public int Esperienza { get; set; }
            public int Livello { get; set; }
            public int Punti_Quest { get; set; }
            public bool Vip { get; set; }
            public bool Ricerca_Attiva { get; set; }
            // Coda e scudi
            public int Code_Reclutamento { get; set; }
            public int Code_Costruzione { get; set; }
            public int Code_Ricerca { get; set; }
            public int ScudoDellaPace { get; set; }

            // Forza esercito
            public double forza_Esercito { get; set; }

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

            // Risorse civili
            public double Cibo { get; set; }
            public double Legno { get; set; }
            public double Pietra { get; set; }
            public double Ferro { get; set; }
            public double Oro { get; set; }
            public double Popolazione { get; set; }

            // Risorse speciali
            public int Diamanati_Blu { get; set; }
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
            public int Unità_Uccise { get; set; }
            public int Guerrieri_Uccisi { get; set; }
            public int Lanceri_Uccisi { get; set; }
            public int Arceri_Uccisi { get; set; }
            public int Catapulte_Uccisi { get; set; }
            public int Unità_Perse { get; set; }
            public int Guerrieri_Persi { get; set; }
            public int Lanceri_Persi { get; set; }
            public int Arceri_Persi { get; set; }
            public int Catapulte_Persi { get; set; }
            public int Risorse_Razziate { get; set; }

            public int Frecce_Utilizzate { get; set; }
            public int Battaglie_Vinte { get; set; }
            public int Battaglie_Perse { get; set; }
            public int Barbari_Sconfitti { get; set; }
            public int Accampamenti_Barbari_Sconfitti { get; set; }
            public int Città_Barbare_Sconfitte { get; set; }
            public int Missioni_Completate { get; set; }
            public int Attacchi_Subiti_PVP { get; set; }
            public int Attacchi_Effettuati_PVP { get; set; }

            public int Unità_Addestrate { get; set; }
            public int Risorse_Utilizzate { get; set; }
            public int Tempo_Addestramento_Risparmiato { get; set; }
            public int Tempo_Costruzione_Risparmiato { get; set; }

            public int Consumo_Cibo_Esercito { get; set; }
            public int Consumo_Oro_Esercito { get; set; }

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
            public int Ricerca_Riparazione { get; set; }

            //Ricerca Città

            public int Ricerca_Ingresso_Guarnigione { get; set; }
            public int Ricerca_Citta_Guarnigione { get; set; }

            public int Ricerca_Cancello_Salute { get; set; }
            public int Ricerca_Cancello_Difesa { get; set; }
            public int Ricerca_Cancello_Guarnigione { get; set; }

            public int Ricerca_Mura_Salute { get; set; }
            public int Ricerca_Mura_Difesa { get; set; }
            public int Ricerca_Mura_Guarnigione { get; set; }

            public int Ricerca_Torri_Salute { get; set; }
            public int Ricerca_Torri_Difesa { get; set; }
            public int Ricerca_Torri_Guarnigione { get; set; }

            public int Ricerca_Castello_Salute { get; set; }
            public int Ricerca_Castello_Difesa { get; set; }
            public int Ricerca_Castello_Guarnigione { get; set; }

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

            // Premi
            public bool[] PremiNormali { get; set; } = new bool[20];
            public bool[] PremiVIP { get; set; } = new bool[20];

            // Città - Ingresso
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

            public List<BuildingManager.ConstructionTask> currentTasks_Building = new(); // Lista dei task attualmente in costruzione (slot globali, max = Code_Costruzione)
            public Queue<BuildingManager.ConstructionTask> building_Queue = new(); // Coda globale di attesa (quando tutti gli slot sono occupati)

            public List<BuildingManager.ConstructionTask> currentTasks_Recruit = new(); // Lista dei task attualmente in costruzione (slot globali, max = Code_Reclutamento)
            public Queue<BuildingManager.ConstructionTask> recruit_Queue = new(); // Coda globale di attesa (quando tutti gli slot sono occupati)

            public List<BuildingManager.ConstructionTask> currentTasks_Research = new(); // Lista dei task attualmente in costruzione (slot globali, max = 1)
            public Queue<BuildingManager.ConstructionTask> research_Queue = new(); // Coda globale di attesa (quando tutti gli slot sono occupati)

            public Player(string username, string password, Guid guid_Client)
            {
                // Inizializza lista villaggi barbarici
                VillaggiPersonali = new List<Gioco.Barbari.VillaggioBarbaro>();

                //Statistiche
                Tempo_Addestramento_Risparmiato = 0;
                Tempo_Costruzione_Risparmiato = 0;

                //Dati Giocatore
                Username = username;
                Password = password;
                guid_Player = guid_Client;
                ScudoDellaPace = 0;
                Code_Costruzione = 1;
                Code_Reclutamento = 1;
                Code_Ricerca = 1;

                Livello = 1;
                Esperienza = 0;
                Punti_Quest = 0;
                Vip = false;
                Ricerca_Attiva = false;
                Diamanati_Blu = 0;
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
                GuerrieriMax = 35;
                LancieriMax = 25;
                ArceriMax = 10;
                CatapulteMax = 5;

                //Città
                Guarnigione_Ingresso = 0;
                Guarnigione_IngressoMax = 100;

                for (int i = 0; i < 5; i++)
                {
                    Guerrieri_Ingresso[i] = 0;
                    Lanceri_Ingresso[i] = 0;
                    Arceri_Ingresso[i] = 0;
                    Catapulte_Ingresso[i] = 0;
                }

                Guarnigione_Cancello = 0;
                Guarnigione_CancelloMax = 50;
                for (int i = 0; i < 5; i++)
                {
                    Guerrieri_Cancello[i] = 0;
                    Lanceri_Cancello[i] = 0;
                    Arceri_Cancello[i] = 0;
                    Catapulte_Cancello[i] = 0;
                }
                Salute_Cancello = 50;
                Salute_CancelloMax = 50;
                Difesa_Cancello = 50;
                Difesa_CancelloMax = 50;

                Guarnigione_Mura = 0;
                Guarnigione_MuraMax = 50;
                for (int i = 0; i < 5; i++)
                {
                    Guerrieri_Mura[i] = 0;
                    Lanceri_Mura[i] = 0;
                    Arceri_Mura[i] = 0;
                    Catapulte_Mura[i] = 0;
                }
                Salute_Mura = 50;
                Salute_MuraMax = 50;
                Difesa_Mura = 50;
                Difesa_MuraMax = 50;

                Guarnigione_Torri = 0;
                Guarnigione_TorriMax = 50;
                for (int i = 0; i < 5; i++)
                {
                    Guerrieri_Torri[i] = 0;
                    Lanceri_Torri[i] = 0;
                    Arceri_Torri[i] = 0;
                    Catapulte_Torri[i] = 0;
                }
                Salute_Torri = 50;
                Salute_TorriMax = 50;
                Difesa_Torri = 50;
                Difesa_TorriMax = 50;

                Guarnigione_Castello = 0;
                Guarnigione_CastelloMax = 75;
                for (int i = 0; i < 5; i++)
                {
                    Guerrieri_Castello[i] = 0;
                    Lanceri_Castello[i] = 0;
                    Arceri_Castello[i] = 0;
                    Catapulte_Castello[i] = 0;
                }
                Salute_Castello = 75;
                Salute_CastelloMax = 75;
                Difesa_Castello = 75;
                Difesa_CastelloMax = 75;

                Guarnigione_Citta = 0;
                Guarnigione_CittaMax = 200;
                for (int i = 0; i < 5; i++)
                {
                    Guerrieri_Citta[i] = 0;
                    Lanceri_Citta[i] = 0;
                    Arceri_Citta[i] = 0;
                    Catapulte_Citta[i] = 0;
                }

                //Ricerche
                Ricerca_Produzione = 0;
                Ricerca_Costruzione = 0;
                Ricerca_Riparazione = 0;
                Ricerca_Addestramento = 0;
                Ricerca_Popolazione = 0;

                //Ricerca CIttà
                Ricerca_Ingresso_Guarnigione = 0;
                Ricerca_Citta_Guarnigione = 0;

                Ricerca_Cancello_Salute = 0;
                Ricerca_Cancello_Difesa = 0;
                Ricerca_Cancello_Guarnigione = 0;

                Ricerca_Mura_Salute = 0;
                Ricerca_Mura_Difesa = 0;
                Ricerca_Mura_Guarnigione = 0;

                Ricerca_Torri_Salute = 0;
                Ricerca_Torri_Difesa = 0;
                Ricerca_Torri_Guarnigione = 0;

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
            }

            public class PlayerQuestProgress
            {
                public int[] Completions { get; set; } = new int[QuestDatabase.Quests.Count]; // Indica quante volte ogni quest è stata completata
                public int[] CurrentProgress { get; set; } = new int[QuestDatabase.Quests.Count]; // Puoi anche tenere traccia di progressi parziali

                public bool IsQuestFullyCompleted(int questId) // Restituisce true se la quest è completata il numero massimo di volte
                {
                    return Completions[questId] >= QuestDatabase.Quests[questId].Max_Complete;
                }

                public bool AddProgress(int questId, int amount, Player player)
                {
                    var quest = QuestDatabase.Quests[questId];

                    // Se la quest è già completata il numero massimo di volte, non fare nulla
                    if (IsQuestFullyCompleted(questId))
                    {
                        Console.WriteLine($"Quest '{quest.Quest_Description}' è già completata al massimo ({quest.Max_Complete} volte).");
                        return false;
                    }

                    int completata = Completions[questId];  // Quante volte è stata completata finora
                    int requireDinamico = quest.Require + (completata * Variabili_Server.moltiplicatore_Quest); // Aumenta il requisito di 5 per ogni completamento

                    player.Punti_Quest += quest.Experience;
                    CurrentProgress[questId] += amount; // Aggiungi il progresso

                    // 🔹 Se completata
                    if (CurrentProgress[questId] >= requireDinamico)
                    {
                        CurrentProgress[questId] = 0; // Resetta il progresso per la prossima volta
                        Completions[questId]++; // Incrementa il conteggio delle completazioni
                        Console.WriteLine($"Quest '{quest.Quest_Description}' completata {Completions[questId]} / {quest.Max_Complete} volte.");

                        if (Completions[questId] == quest.Max_Complete)
                            QuestManager.OnEvent(player, QuestEventType.Miglioramento, "", 1); // Per ogni quest completata, aggiorna la quest

                        return true; // Quest completata
                    }

                    return false;
                }
            }

            public bool ValidatePassword(string password)
            {
                return Password == password;
            }

            public void ProduceResources() //produzione risorse
            {
                if (Cibo < Fattoria * Strutture.Edifici.Fattoria.Limite)
                    Cibo += Fattoria * (Strutture.Edifici.Fattoria.Produzione + Ricerca_Produzione * Ricerca.Tipi.Incremento.Cibo);
                if (Legno < Segheria * Strutture.Edifici.Segheria.Limite)
                    Legno += Segheria * (Strutture.Edifici.Segheria.Produzione + Ricerca_Produzione * Ricerca.Tipi.Incremento.Legno);
                if (Pietra < CavaPietra * Strutture.Edifici.CavaPietra.Limite)
                    Pietra += CavaPietra * (Strutture.Edifici.CavaPietra.Produzione + Ricerca_Produzione * Ricerca.Tipi.Incremento.Pietra);
                if (Ferro < MinieraFerro * Strutture.Edifici.MinieraFerro.Limite)
                    Ferro += MinieraFerro * (Strutture.Edifici.MinieraFerro.Produzione + Ricerca_Produzione * Ricerca.Tipi.Incremento.Ferro);
                if (Oro < MinieraOro * Strutture.Edifici.MinieraOro.Limite)
                    Oro += MinieraOro * (Strutture.Edifici.MinieraOro.Produzione + Ricerca_Produzione * Ricerca.Tipi.Incremento.Oro);
                if (Popolazione < Abitazioni * Strutture.Edifici.Case.Limite)
                    Popolazione += Abitazioni * (Strutture.Edifici.Case.Produzione + Ricerca_Produzione * Ricerca.Tipi.Incremento.Popolazione);

                Dollari_Virtuali += (decimal)(Terreno_Comune * Variabili_Server.Terreni_Virtuali.Comune.Produzione) +
                    (decimal)(Terreno_NonComune * Variabili_Server.Terreni_Virtuali.NonComune.Produzione) +
                    (decimal)(Terreno_Raro * Variabili_Server.Terreni_Virtuali.Raro.Produzione) +
                    (decimal)(Terreno_Epico * Variabili_Server.Terreni_Virtuali.Epico.Produzione) +
                    (decimal)(Terreno_Leggendario * Variabili_Server.Terreni_Virtuali.Leggendario.Produzione);

                if (Spade < Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Limite)
                    Spade += Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Produzione;
                if (Lance < Workshop_Lance * Strutture.Edifici.ProduzioneLance.Limite)
                    Lance += Workshop_Lance * Strutture.Edifici.ProduzioneLance.Produzione;
                if (Archi < Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Limite)
                    Archi += Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Produzione;
                if (Scudi < Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Limite)
                    Scudi += Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Produzione;
                if (Armature < Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Limite)
                    Armature += Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Produzione;
                if (Frecce < Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Limite)
                    Frecce += Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Produzione;
            }
            public void ManutenzioneEsercito() //produzione risorse
            {
                Cibo -= Guerrieri[0] * Esercito.Unità.Guerrieri_1.Cibo + Lanceri[0] * Esercito.Unità.Lanceri_1.Cibo + Arceri[0] * Esercito.Unità.Arceri_1.Cibo + Catapulte[0] * Esercito.Unità.Catapulte_1.Cibo;
                Cibo -= Guerrieri[1] * Esercito.Unità.Guerriero_2.Cibo + Lanceri[1] * Esercito.Unità.Lancere_2.Cibo + Arceri[1] * Esercito.Unità.Arcere_2.Cibo + Catapulte[1] * Esercito.Unità.Catapulta_2.Cibo;
                Cibo -= Guerrieri[2] * Esercito.Unità.Guerriero_3.Cibo + Lanceri[2] * Esercito.Unità.Lancere_3.Cibo + Arceri[2] * Esercito.Unità.Arcere_3.Cibo + Catapulte[2] * Esercito.Unità.Catapulta_3.Cibo;
                Cibo -= Guerrieri[3] * Esercito.Unità.Guerriero_4.Cibo + Lanceri[3] * Esercito.Unità.Lancere_4.Cibo + Arceri[3] * Esercito.Unità.Arcere_4.Cibo + Catapulte[3] * Esercito.Unità.Catapulta_4.Cibo;
                Cibo -= Guerrieri[4] * Esercito.Unità.Guerriero_5.Cibo + Lanceri[4] * Esercito.Unità.Lancere_5.Cibo + Arceri[4] * Esercito.Unità.Arcere_5.Cibo + Catapulte[4] * Esercito.Unità.Catapulta_5.Cibo;

                Oro -= Guerrieri[0] * Esercito.Unità.Guerrieri_1.Salario + Lanceri[0] * Esercito.Unità.Lanceri_1.Salario + Arceri[0] * Esercito.Unità.Arceri_1.Salario + Catapulte[0] * Esercito.Unità.Catapulte_1.Salario;
                Oro -= Guerrieri[1] * Esercito.Unità.Guerriero_2.Salario + Lanceri[1] * Esercito.Unità.Lancere_2.Salario + Arceri[1] * Esercito.Unità.Arcere_2.Salario + Catapulte[1] * Esercito.Unità.Catapulta_2.Salario;
                Oro -= Guerrieri[2] * Esercito.Unità.Guerriero_3.Salario + Lanceri[2] * Esercito.Unità.Lancere_3.Salario + Arceri[2] * Esercito.Unità.Arcere_3.Salario + Catapulte[2] * Esercito.Unità.Catapulta_3.Salario;
                Oro -= Guerrieri[3] * Esercito.Unità.Guerriero_4.Salario + Lanceri[3] * Esercito.Unità.Lancere_4.Salario + Arceri[3] * Esercito.Unità.Arcere_4.Salario + Catapulte[3] * Esercito.Unità.Catapulta_4.Salario;
                Oro -= Guerrieri[4] * Esercito.Unità.Guerriero_5.Salario + Lanceri[4] * Esercito.Unità.Lancere_5.Salario + Arceri[4] * Esercito.Unità.Arcere_5.Salario + Catapulte[4] * Esercito.Unità.Catapulta_5.Salario;

                if (Cibo <= 0) Cibo = 0;
                if (Oro <= 0) Oro = 0;
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

