using Server_Strategico.Gioco;
using System.Text.Json;
using static Server_Strategico.Gioco.Barbari;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Variabili_Server;

namespace Server_Strategico.Server
{
    internal class GameSave
    {
        static bool Saved = false;
        private static readonly string SavePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "Server Strategico",
            "Saves"
        );

        public static void Initialize()
        {
            if (!Directory.Exists(SavePath)) // Crea la directory se non esiste
                Directory.CreateDirectory(SavePath);
        }

        public static async Task SavePlayer(Player player)
        {
            try
            {
                var playerData = new PlayerSaveData
                {
                    //Dati Giocatore
                    Username = player.Username,
                    Password = player.Password,
                    guid_Player = player.guid_Player,
                    ScudoDellaPace = player.ScudoDellaPace,
                    Code_Costruzione = player.Code_Costruzione,
                    Code_Reclutamento = player.Code_Reclutamento,
                    Code_Ricerca = player.Code_Ricerca,

                    Livello = player.Livello,
                    Esperienza = player.Esperienza,
                    Punti_Quest = player.Punti_Quest,
                    Vip = player.Vip,
                    Ricerca_Attiva = player.Ricerca_Attiva,
                    Diamanti_Blu = player.Diamanti_Blu,
                    Diamanti_Viola = player.Diamanti_Viola,
                    Dollari_Virtuali = player.Dollari_Virtuali,
                    forza_Esercito = player.forza_Esercito,

                    //Terreni Virtuali
                    Terreno_Comune = player.Terreno_Comune,
                    Terreno_NonComune = player.Terreno_NonComune,
                    Terreno_Raro = player.Terreno_Raro,
                    Terreno_Epico = player.Terreno_Epico,
                    Terreno_Leggendario = player.Terreno_Leggendario,

                    //Strutture Civile
                    Fattoria = player.Fattoria,
                    Segheria = player.Segheria,
                    CavaPietra = player.CavaPietra,
                    MinieraFerro = player.MinieraFerro,
                    MinieraOro = player.MinieraOro,
                    Abitazioni = player.Abitazioni,

                    //Strutture Militare
                    Workshop_Spade = player.Workshop_Spade,
                    Workshop_Lance = player.Workshop_Lance,
                    Workshop_Archi = player.Workshop_Archi,
                    Workshop_Scudi = player.Workshop_Scudi,
                    Workshop_Armature = player.Workshop_Armature,
                    Workshop_Frecce = player.Workshop_Frecce,

                    Caserma_Guerrieri = player.Caserma_Guerrieri,
                    Caserma_Lancieri = player.Caserma_Lancieri,
                    Caserma_Arceri = player.Caserma_Arceri,
                    Caserma_Catapulte = player.Caserma_Catapulte,

                    //Risorse Civile
                    Cibo = player.Cibo,
                    Legno = player.Legno,
                    Pietra = player.Pietra,
                    Ferro = player.Ferro,
                    Oro = player.Oro,
                    Popolazione = player.Popolazione,

                    //Risorse Militare
                    Spade = player.Spade,
                    Lance = player.Lance,
                    Archi = player.Archi,
                    Scudi = player.Scudi,
                    Armature = player.Armature,
                    Frecce = player.Frecce,

                    // Esercito
                    Guerrieri = player.Guerrieri,
                    Lanceri = player.Lanceri,
                    Arceri = player.Arceri,
                    Catapulte = player.Catapulte,

                    //Limite x caserma
                    GuerrieriMax = player.GuerrieriMax,
                    LancieriMax = player.LancieriMax,
                    ArceriMax = player.ArceriMax,
                    CatapulteMax = player.CatapulteMax,

                    //Città
                    #region Città
                    Guarnigione_Ingresso = player.Guarnigione_Ingresso,
                    Guarnigione_IngressoMax = player.Guarnigione_IngressoMax,
                
                    Guerrieri_Ingresso = player.Guerrieri_Ingresso,
                    Lanceri_Ingresso = player.Lanceri_Ingresso,
                    Arceri_Ingresso = player.Arceri_Ingresso,
                    Catapulte_Ingresso = player.Catapulte_Ingresso,
                

                    Guarnigione_Cancello = player.Guarnigione_Cancello,
                    Guarnigione_CancelloMax = player.Guarnigione_CancelloMax,

                    Guerrieri_Cancello = player.Guerrieri_Cancello,
                    Lanceri_Cancello = player.Lanceri_Cancello,
                    Arceri_Cancello = player.Arceri_Cancello,
                    Catapulte_Cancello = player.Catapulte_Cancello,
                    
                    Salute_Cancello = player.Salute_Cancello,
                    Salute_CancelloMax = player.Salute_CancelloMax,
                    Difesa_Cancello = player.Difesa_Cancello,
                    Difesa_CancelloMax = player.Difesa_CancelloMax,

                    Guarnigione_Mura = player.Guarnigione_Mura,
                    Guarnigione_MuraMax = player.Guarnigione_MuraMax,

                    Guerrieri_Mura = player.Guerrieri_Mura,
                    Lanceri_Mura = player.Lanceri_Mura,
                    Arceri_Mura = player.Arceri_Mura,
                    Catapulte_Mura = player.Catapulte_Mura,

                    Salute_Mura = player.Salute_Mura,
                    Salute_MuraMax = player.Salute_MuraMax,
                    Difesa_Mura = player.Difesa_Mura,
                    Difesa_MuraMax = player.Difesa_MuraMax,

                    Guarnigione_Torri = player.Guarnigione_Torri,
                    Guarnigione_TorriMax = player.Guarnigione_TorriMax,

                    Guerrieri_Torri = player.Guerrieri_Torri,
                    Lanceri_Torri = player.Lanceri_Torri,
                    Arceri_Torri = player.Arceri_Torri,
                    Catapulte_Torri = player.Catapulte_Torri,
                    
                    Salute_Torri = player.Salute_Torri,
                    Salute_TorriMax = player.Salute_TorriMax,
                    Difesa_Torri = player.Difesa_Torri,
                    Difesa_TorriMax = player.Difesa_TorriMax,

                    Guarnigione_Castello = player.Guarnigione_Castello,
                    Guarnigione_CastelloMax = player.Guarnigione_CastelloMax,

                    Guerrieri_Castello = player.Guerrieri_Castello,
                    Lanceri_Castello = player.Lanceri_Castello,
                    Arceri_Castello = player.Arceri_Castello,
                    Catapulte_Castello = player.Catapulte_Castello,
                    
                    Salute_Castello = player.Salute_Castello,
                    Salute_CastelloMax = player.Salute_CastelloMax,
                    Difesa_Castello = player.Difesa_Castello,
                    Difesa_CastelloMax = player.Difesa_CastelloMax,

                    Guarnigione_Citta = player.Guarnigione_Citta,
                    Guarnigione_CittaMax = player.Guarnigione_CittaMax,

                    Guerrieri_Citta = player.Guerrieri_Citta,
                    Lanceri_Citta = player.Lanceri_Citta,
                    Arceri_Citta = player.Arceri_Citta,
                    Catapulte_Citta = player.Catapulte_Citta,
                    #endregion

                    //Ricerca
                    Ricerca_Produzione = player.Ricerca_Produzione,
                    Ricerca_Costruzione = player.Ricerca_Costruzione,
                    Ricerca_Riparazione = player.Ricerca_Riparazione,
                    Ricerca_Addestramento = player.Ricerca_Addestramento,
                    Ricerca_Popolazione = player.Ricerca_Popolazione,

                    //Livelli unità
                    Guerriero_Livello = player.Guerriero_Livello,
                    Guerriero_Salute = player.Guerriero_Salute,
                    Guerriero_Difesa = player.Guerriero_Difesa,
                    Guerriero_Attacco = player.Guerriero_Attacco,

                    Lancere_Livello = player.Lancere_Livello,
                    Lancere_Salute = player.Lancere_Salute,
                    Lancere_Difesa = player.Lancere_Difesa,
                    Lancere_Attacco = player.Lancere_Attacco,

                    Arcere_Livello = player.Arcere_Livello,
                    Arcere_Salute = player.Arcere_Salute,
                    Arcere_Difesa = player.Arcere_Difesa,
                    Arcere_Attacco = player.Arcere_Attacco,

                    Catapulta_Livello = player.Catapulta_Livello,
                    Catapulta_Salute = player.Catapulta_Salute,
                    Catapulta_Difesa = player.Catapulta_Difesa,
                    Catapulta_Attacco = player.Catapulta_Attacco,

                    // Statistiche
                    Unità_Eliminate = player.Unità_Eliminate,
                    Guerrieri_Eliminate = player.Guerrieri_Eliminate,
                    Lanceri_Eliminate = player.Lanceri_Eliminate,
                    Arceri_Eliminate = player.Arceri_Eliminate,
                    Catapulte_Eliminate = player.Catapulte_Eliminate,
                    Unità_Addestrate = player.Unità_Addestrate,

                    Unità_Perse = player.Unità_Perse,
                    Guerrieri_Persi = player.Guerrieri_Persi,
                    Lanceri_Persi = player.Lanceri_Persi,
                    Arceri_Persi = player.Arceri_Persi,
                    Catapulte_Perse = player.Catapulte_Perse,
                    Risorse_Razziate = player.Risorse_Razziate,

                    Strutture_Civili_Costruite = player.Strutture_Civili_Costruite,
                    Strutture_Militari_Costruite = player.Strutture_Militari_Costruite,
                    Caserme_Costruite = player.Caserme_Costruite,

                    Frecce_Utilizzate = player.Frecce_Utilizzate,
                    Battaglie_Vinte = player.Battaglie_Vinte,
                    Battaglie_Perse = player.Battaglie_Perse,
                    Quest_Completate = player.Quest_Completate,
                    Attacchi_Subiti_PVP = player.Attacchi_Subiti_PVP,
                    Attacchi_Effettuati_PVP = player.Attacchi_Effettuati_PVP,

                    Barbari_Sconfitti = player.Barbari_Sconfitti,
                    Accampamenti_Barbari_Sconfitti = player.Accampamenti_Barbari_Sconfitti,
                    Città_Barbare_Sconfitte = player.Città_Barbare_Sconfitte,
                    Danno_HP_Barbaro = player.Danno_HP_Barbaro,
                    Danno_DEF_Barbaro = player.Danno_DEF_Barbaro,

                    Risorse_Utilizzate = player.Risorse_Utilizzate,
                    Tempo_Addestramento = player.Tempo_Addestramento,
                    Tempo_Costruzione = player.Tempo_Costruzione,
                    Tempo_Ricerca = player.Tempo_Ricerca,
                    Tempo_Sottratto_Diamanti = player.Tempo_Sottratto_Diamanti,

                    Consumo_Cibo_Esercito = player.Consumo_Cibo_Esercito,
                    Consumo_Oro_Esercito = player.Consumo_Oro_Esercito,
                    Diamanti_Viola_Utilizzati = player.Diamanti_Viola_Utilizzati,
                    Diamanti_Blu_Utilizzati = player.Diamanti_Blu_Utilizzati,

                    //Quest
                    Completions = player.QuestProgress.Completions,
                    CurrentProgress = player.QuestProgress.CurrentProgress,

                    PremiNormali = player.PremiNormali,
                    PremiVIP = player.PremiVIP,

                    // --- Code Costruzione ---
                    CurrentBuildingTasks = player.currentTasks_Building
                    .Select(t => new SavedTask
                    {
                        Type = t.Type,
                        DurationInSeconds = t.DurationInSeconds,
                        RemainingSeconds = t.GetRemainingTime(),
                        IsInProgress = true
                    })
                    .ToList(),

                    QueuedBuildingTasks = player.building_Queue
                    .Select(t => new SavedTask
                    {
                        Type = t.Type,
                        DurationInSeconds = t.DurationInSeconds,
                        RemainingSeconds = t.DurationInSeconds,
                        IsInProgress = false
                    })
                    .ToList(),

                    // --- Reclutamento ---
                    CurrentRecruitTasks = player.currentTasks_Recruit
                    .Select(t => new SavedTask
                    {
                        Type = t.Type,
                        DurationInSeconds = t.DurationInSeconds,
                        RemainingSeconds = t.GetRemainingTime(),
                        IsInProgress = true
                    })
                    .ToList(),

                    QueuedRecruitTasks = player.recruit_Queue
                    .Select(t => new SavedTask
                    {
                        Type = t.Type,
                        DurationInSeconds = t.DurationInSeconds,
                        RemainingSeconds = t.DurationInSeconds,
                        IsInProgress = false
                    })
                    .ToList(),

                    // --- Ricerca ---
                    CurrentResearchTasks = player.currentTasks_Research
                    .Select(t => new SavedTask
                    {
                        Type = t.Type,
                        DurationInSeconds = t.DurationInSeconds,
                        RemainingSeconds = t.GetRemainingTime(),
                        IsInProgress = true
                    })
                    .ToList(),

                   QueuedResearchTasks = player.research_Queue
                    .Select(t => new SavedTask
                    {
                        Type = t.Type,
                        DurationInSeconds = t.DurationInSeconds,
                        RemainingSeconds = t.DurationInSeconds,
                        IsInProgress = false
                    })
                    .ToList(),

                    //// ---  Villaggi ---
                    //VillaggiPersonali = player.VillaggiPersonali
                    //?.Select(v => new VillaggioSaveData
                    //{
                    //    Id = v.Id,
                    //    Nome = v.Nome,
                    //    Livello = v.Livello,
                    //    Sconfitto = v.Sconfitto,
                    //    Esplorato = v.Esplorato,
                    //    Guerrieri = v.Guerrieri,
                    //    Lancieri = v.Lancieri,
                    //    Arcieri = v.Arcieri,
                    //    Catapulte = v.Catapulte
                    //})
                    //.ToList(),

                    //CittaGlobali = Gioco.Barbari.CittaGlobali
                    //?.Select(c => new VillaggioSaveData
                    //{
                    //    Id = c.Id,
                    //    Nome = c.Nome,
                    //    Livello = c.Livello,
                    //    Sconfitto = c.Sconfitto,
                    //    Esplorato = c.Esplorato,
                    //    Guerrieri = c.Guerrieri,
                    //    Lancieri = c.Lancieri,
                    //    Arcieri = c.Arcieri,
                    //    Catapulte = c.Catapulte
                    //})
                    //.ToList(),
                };


                // Salvataggio Villaggi Personali
                if (player.VillaggiPersonali != null)
                {
                    var villaggiJson = JsonSerializer.Serialize(player.VillaggiPersonali, new JsonSerializerOptions { WriteIndented = true });
                    string fileName1 = Path.Combine(SavePath, $"{player.Username}_Villaggi.json");
                    await File.WriteAllTextAsync(fileName1, villaggiJson);
                    File.WriteAllText(fileName1, villaggiJson);
                }

                // Salvataggio Città Globali
                if (Gioco.Barbari.CittaGlobali != null)
                {
                    var cittaJson = JsonSerializer.Serialize(Gioco.Barbari.CittaGlobali, new JsonSerializerOptions { WriteIndented = true });
                    string fileName1 = Path.Combine(SavePath, $"{player.Username}_Citta.json");
                    await File.WriteAllTextAsync(fileName1, cittaJson);
                    File.WriteAllText(fileName1, cittaJson);
                }

                string fileName = Path.Combine(SavePath, $"{player.Username}.json");
                string jsonString = JsonSerializer.Serialize(playerData, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(fileName, jsonString);

                Console.WriteLine($"[GameSave] Salvati i dati del giocatore {player.Username}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameSave] Errore durante il salvataggio: {ex.Message}");
            }
        }

        public static async Task<bool> LoadPlayer(string username, string password)
        {
            try
            {
                string fileName = Path.Combine(SavePath, $"{username}.json");
                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"[GameSave] Nessun salvataggio trovato per {username}");
                    return false;
                }

                string jsonString = await File.ReadAllTextAsync(fileName);
                var playerData = JsonSerializer.Deserialize<PlayerSaveData>(jsonString);

                if (playerData.Password != password && password != "Auto")
                {
                    Console.WriteLine($"[GameSave] Password non valida per {username}");
                    return false;
                }

                // Aggiorna il giocatore esistente con i dati salvati
                var player = Server.servers_.GetPlayer(username, password);
                if (player != null)
                {

                    //Giocatori
                    player.Username = playerData.Username;
                    player.Password = playerData.Password;
                    player.Livello = playerData.Livello;
                    player.Esperienza = playerData.Esperienza;
                    player.Vip = playerData.Vip;
                    player.ScudoDellaPace = playerData.ScudoDellaPace;
                    player.Punti_Quest = playerData.Punti_Quest;
                    player.Code_Costruzione = playerData.Code_Costruzione;
                    player.Code_Reclutamento = playerData.Code_Reclutamento;
                    player.Code_Ricerca = playerData.Code_Ricerca;

                    player.Diamanti_Blu = playerData.Diamanti_Blu;
                    player.Diamanti_Viola = playerData.Diamanti_Viola;
                    player.Dollari_Virtuali = playerData.Dollari_Virtuali;

                    player.Terreno_Comune = playerData.Terreno_Comune;
                    player.Terreno_NonComune = playerData.Terreno_NonComune;
                    player.Terreno_Raro = playerData.Terreno_Raro;
                    player.Terreno_Epico = playerData.Terreno_Epico;
                    player.Terreno_Leggendario = playerData.Terreno_Leggendario;

                    //Quest
                    player.QuestProgress.Completions = playerData.Completions;
                    player.QuestProgress.CurrentProgress = playerData.CurrentProgress;

                    // Risorse
                    player.Cibo = playerData.Cibo;
                    player.Legno = playerData.Legno;
                    player.Pietra = playerData.Pietra;
                    player.Ferro = playerData.Ferro;
                    player.Oro = playerData.Oro;
                    player.Popolazione = playerData.Popolazione;

                    player.Spade = playerData.Spade;
                    player.Lance = playerData.Lance;
                    player.Archi = playerData.Archi;
                    player.Scudi = playerData.Scudi;
                    player.Armature = playerData.Armature;
                    player.Frecce = playerData.Frecce;

                    // Edifici
                    BuildingManager.SetBuildings(
                        playerData.Fattoria,
                        playerData.Segheria, 
                        playerData.CavaPietra,
                        playerData.MinieraFerro,
                        playerData.MinieraOro,
                        playerData.Abitazioni,
                        playerData.Workshop_Spade,
                        playerData.Workshop_Lance,
                        playerData.Workshop_Archi,
                        playerData.Workshop_Scudi,
                        playerData.Workshop_Armature,
                        playerData.Workshop_Frecce,
                        playerData.Caserma_Guerrieri,
                        playerData.Caserma_Lancieri,
                        playerData.Caserma_Arceri,
                        playerData.Caserma_Catapulte, 
                        player
                    );

                    // Esercito
                    player.Guerrieri = playerData.Guerrieri;
                    player.Lanceri = playerData.Lanceri;
                    player.Arceri = playerData.Arceri;
                    player.Catapulte = playerData.Catapulte;

                    // Caserme
                    player.GuerrieriMax = playerData.GuerrieriMax;
                    player.LancieriMax = playerData.LancieriMax;
                    player.ArceriMax = playerData.ArceriMax;
                    player.CatapulteMax = playerData.CatapulteMax;

                    //Ricerche
                    player.Ricerca_Produzione = playerData.Ricerca_Produzione;
                    player.Ricerca_Costruzione = playerData.Ricerca_Costruzione;
                    player.Ricerca_Riparazione = playerData.Ricerca_Riparazione;
                    player.Ricerca_Addestramento = playerData.Ricerca_Addestramento;

                    player.Guerriero_Livello = playerData.Guerriero_Livello;
                    player.Guerriero_Salute = playerData.Guerriero_Salute;
                    player.Guerriero_Difesa = playerData.Guerriero_Difesa;
                    player.Guerriero_Attacco = playerData.Guerriero_Attacco;

                    player.Lancere_Livello = playerData.Lancere_Livello;
                    player.Lancere_Salute = playerData.Lancere_Salute;
                    player.Lancere_Difesa = playerData.Lancere_Difesa;
                    player.Lancere_Attacco = playerData.Lancere_Attacco;

                    player.Arcere_Livello = playerData.Arcere_Livello;
                    player.Arcere_Salute = playerData.Arcere_Salute;
                    player.Arcere_Difesa = playerData.Arcere_Difesa;
                    player.Arcere_Attacco = playerData.Arcere_Attacco;

                    player.Catapulta_Livello = playerData.Catapulta_Livello;
                    player.Catapulta_Salute = playerData.Catapulta_Salute;
                    player.Catapulta_Difesa = playerData.Catapulta_Difesa;
                    player.Catapulta_Attacco = playerData.Catapulta_Attacco;

                    //Castello
                    player.Salute_Cancello = playerData.Salute_Cancello;
                    player.Salute_CancelloMax = playerData.Salute_CancelloMax;
                    player.Salute_Mura = playerData.Salute_Mura;
                    player.Salute_MuraMax = playerData.Salute_MuraMax;
                    player.Salute_Torri = playerData.Salute_Torri;
                    player.Salute_TorriMax = playerData.Salute_TorriMax;
                    player.Salute_Castello = playerData.Salute_Castello;
                    player.Salute_CastelloMax = playerData.Salute_CastelloMax;

                    //Quest Mensile
                    player.PremiNormali = playerData.PremiNormali;
                    player.PremiVIP = playerData.PremiVIP;

                    // --- Ripristino costruzioni in coda ---
                    player.building_Queue = new Queue<BuildingManager.ConstructionTask>(
                        playerData.QueuedBuildingTasks.Select(t =>
                            new BuildingManager.ConstructionTask(t.Type, t.DurationInSeconds)
                        )
                    );

                    // --- Reclutamento ---
                    player.currentTasks_Recruit = playerData.CurrentRecruitTasks
                        .Select(t => {
                            var task = new UnitManager.RecruitTask(t.Type, t.DurationInSeconds);
                            if (t.IsInProgress) task.Start();
                            return task;
                        }).ToList();

                    player.recruit_Queue = new Queue<UnitManager.RecruitTask>(
                        playerData.QueuedRecruitTasks.Select(t => new UnitManager.RecruitTask(t.Type, t.DurationInSeconds))
                    );

                    // --- Ricerca ---
                    player.currentTasks_Research = playerData.CurrentResearchTasks
                        .Select(t =>
                        {
                            var task = new ResearchManager.ResearchTask(t.Type, t.DurationInSeconds);

                            if (t.IsInProgress)
                            {
                                task.Start();

                                // Riporta il tempo rimasto
                                double elapsed = t.DurationInSeconds - t.RemainingSeconds;
                                task.RiduciTempo((int)elapsed);
                            }
                            else if (t.RemainingSeconds <= 0)
                            {
                                task.ForzaCompletamento();
                            }

                            return task;
                        }).ToList();

                    // --- Coda ricerca ---
                    player.research_Queue = new Queue<ResearchManager.ResearchTask>(
                        playerData.QueuedResearchTasks.Select(t => new ResearchManager.ResearchTask(t.Type, t.DurationInSeconds))
                    );

                    // --- Stato Ricerca ---
                    player.Ricerca_Attiva = player.currentTasks_Research.Count > 0 || player.research_Queue.Count > 0;


                    string fileName_Villaggio = Path.Combine(SavePath, $"{username}");

                    // Caricamento Villaggi Personali
                    if (File.Exists(fileName_Villaggio + "_Villaggi.json"))
                    {
                        var villaggiJson = File.ReadAllText(fileName_Villaggio + "_Villaggi.json");
                        var savedVillaggi = JsonSerializer.Deserialize<List<VillaggioSaveData>>(villaggiJson);

                        player.VillaggiPersonali = savedVillaggi
                         .Select(v => new VillaggioBarbaro
                         {
                             Id             = v.Id,
                             Nome           = v.Nome,
                             Livello        = v.Livello,
                             Sconfitto      = v.Sconfitto,
                             Esplorato      = v.Esplorato,
                             Esperienza     = v.Esperienza,
                             Diamanti_Viola = v.Diamanti_Viola,
                             Diamanti_Blu   = v.Diamanti_Blu,
                             Cibo           = v.Cibo,
                             Legno          = v.Legno,
                             Pietra         = v.Pietra,
                             Ferro          = v.Ferro,
                             Oro            = v.Oro,
                             Guerrieri      = v.Guerrieri,
                             Lancieri       = v.Lancieri,
                             Arcieri        = v.Arcieri,
                             Catapulte      = v.Catapulte
                         })
                         .ToList();
                    }
                    else
                        Console.WriteLine($"[GameSave] Nessun salvataggio trovato per {username}");

                    // Caricamento Città Globali
                    if (File.Exists(fileName_Villaggio + "_Citta.json") && Saved == false)
                    {
                        var cittaJson = File.ReadAllText(fileName_Villaggio + "_Citta.json");
                        var savedCitta = JsonSerializer.Deserialize<List<VillaggioSaveData>>(cittaJson);

                        Gioco.Barbari.CittaGlobali = savedCitta
                        .Select(v => new CittaBarbara
                        {
                            Id = v.Id,
                            Nome = v.Nome,
                            Livello = v.Livello,
                            Sconfitto = v.Sconfitto,
                            Esplorato = v.Esplorato,
                            Esperienza = v.Esperienza,
                            Diamanti_Viola = v.Diamanti_Viola,
                            Diamanti_Blu = v.Diamanti_Blu,
                            Cibo = v.Cibo,
                            Legno = v.Legno,
                            Pietra = v.Pietra,
                            Ferro = v.Ferro,
                            Oro = v.Oro,
                            Guerrieri = v.Guerrieri,
                            Lancieri = v.Lancieri,
                            Arcieri = v.Arcieri,
                            Catapulte = v.Catapulte
                        })
                        .ToList();
                        Saved = true; // carica 1 volta sola e non per ogni giocatore
                    }
                    else
                        Console.WriteLine($"[GameSave] Nessun salvataggio trovato per {username}");


                    Console.WriteLine($"[GameSave] Caricati i dati del giocatore {username}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameSave] Errore durante il caricamento: {ex.Message}");
            }
            return false;
        }
        public static async Task Load_Player_Data_Auto()
        {
            try
            {
                if (!Directory.Exists(SavePath))
                {
                    Console.WriteLine("[GameSave] Directory dei salvataggi non trovata");
                    return;
                }

                string[] saveFiles = Directory.GetFiles(SavePath, "*.json");

                foreach (string file in saveFiles)
                {
                    if (Path.GetFileName(file) == "BarbariPVP.json" || Path.GetFileName(file).Contains("_Citta.json") || Path.GetFileName(file).Contains("_Villaggi.json")) // Salta il file dei barbari PVP
                        continue;

                    string username = Path.GetFileNameWithoutExtension(file);
                    Console.WriteLine($"[GameSave] Caricamento automatico per {username}");

                    try
                    {
                        // Leggi il file JSON per estrarre la password
                        string jsonString = await File.ReadAllTextAsync(file);
                        var playerData = JsonSerializer.Deserialize<PlayerSaveData>(jsonString);

                        string password = playerData.Password; // Estrai la password e carica i dati del giocatore
                        Console.WriteLine($"[GameSave] Password estratta per {username}");

                        // Carica i dati del giocatore con la password estratta dal file
                        bool success = await ServerConnection.Load_User_Auto(username, password);
                        if (success) Console.WriteLine($"[GameSave] Caricamento automatico completato per {username}");
                        else Console.WriteLine($"[GameSave] Caricamento automatico fallito per {username}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[GameSave] Errore durante l'estrazione della password per {username}: {ex.Message}");
                    }
                }

                Console.WriteLine("[GameSave] Caricamento automatico completato per tutti i giocatori");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameSave] Errore durante il caricamento automatico: {ex.Message}");
            }
        }
        public static async Task SaveBarbariPVP()
        {
            var barbariData = new
            {
                Giocatori.Barbari.PVP.Guerrieri,
                Giocatori.Barbari.PVP.Lancieri,
                Giocatori.Barbari.PVP.Arceri,
                Giocatori.Barbari.PVP.Catapulte
            };

            string fileName = Path.Combine(SavePath, "BarbariPVP.json");
            string jsonString = JsonSerializer.Serialize(barbariData, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(fileName, jsonString);

            Console.WriteLine("[GameSave] Dati dei barbari PVP salvati.");
        }
        public static async Task<bool> LoadBarbariPVP()
        {
            try
            {
                string fileName = Path.Combine(SavePath, "BarbariPVP.json");
                if (!File.Exists(fileName))
                {
                    Console.WriteLine("[GameSave] Nessun salvataggio trovato per i Barbari PVP");
                    return false;
                }

                string jsonString = await File.ReadAllTextAsync(fileName);
                var barbariData = JsonSerializer.Deserialize<BarbariPVPData>(jsonString);

                // Aggiorna i dati dei barbari con i dati caricati
                Giocatori.Barbari.PVP.Guerrieri = barbariData.Guerrieri;
                Giocatori.Barbari.PVP.Lancieri = barbariData.Lancieri;
                Giocatori.Barbari.PVP.Arceri = barbariData.Arceri;
                Giocatori.Barbari.PVP.Catapulte = barbariData.Catapulte;
                Giocatori.Barbari.PVP.Livello = barbariData.Livello;

                Console.WriteLine("[GameSave] Dati dei barbari PVP caricati.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameSave] Errore durante il caricamento dei barbari PVP: {ex.Message}");
            }
            return false;
        }
        public class SavedTask
        {
            public string Type { get; set; }
            public int DurationInSeconds { get; set; }
            public double RemainingSeconds { get; set; }
            public bool IsInProgress { get; set; }
        }
        private class BarbariPVPData
        {
            public int Guerrieri { get; set; }
            public int Lancieri { get; set; }
            public int Arceri { get; set; }
            public int Catapulte { get; set; }
            public int Livello { get; set; }
        }

        public class VillaggioSaveData
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public int Livello { get; set; }
            public bool Sconfitto { get; set; }
            public bool Esplorato { get; set; }
            public int Esperienza { get; set; }
            public int Diamanti_Viola { get; set; }
            public int Diamanti_Blu { get; set; }
            public double Cibo { get; set; }
            public double Legno { get; set; }
            public double Pietra { get; set; }
            public double Ferro { get; set; }
            public double Oro { get; set; }
            public int Guerrieri { get; set; }
            public int Lancieri { get; set; }
            public int Arcieri { get; set; }
            public int Catapulte { get; set; }
        }
        private class PlayerSaveData
        {
            // --- Code costruzione ---
            public List<SavedTask> CurrentBuildingTasks { get; set; } = new();
            public List<SavedTask> QueuedBuildingTasks { get; set; } = new();

            public List<SavedTask> CurrentRecruitTasks { get; set; } = new();
            public List<SavedTask> QueuedRecruitTasks { get; set; } = new();

            public List<SavedTask> CurrentResearchTasks { get; set; } = new();
            public List<SavedTask> QueuedResearchTasks { get; set; } = new();

            public List<VillaggioSaveData> VillaggiPersonali { get; set; } = new();
            public List<VillaggioSaveData> CittaGlobali { get; set; } = new();

            public int[] Completions { get; set; } = new int[QuestManager.QuestDatabase.Quests.Count]; // Indica quante volte ogni quest è stata completata
            public int[] CurrentProgress { get; set; } = new int[QuestManager.QuestDatabase.Quests.Count]; // Puoi anche tenere traccia di progressi parziali

            #region Variabili giocatore
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
            public int Guerrieri_Eliminate { get; set; }
            public int Lanceri_Eliminate { get; set; }
            public int Arceri_Eliminate { get; set; }
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
            public int[] Guerrieri { get; set; } = new int[5];
            public int[] Lanceri { get; set; } = new int[5];
            public int[] Arceri { get; set; } = new int[5];
            public int[] Catapulte { get; set; } = new int[5];

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
            #endregion

        }

    }
} 