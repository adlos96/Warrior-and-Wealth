using Server_Strategico.Gioco;
using Server_Strategico.Manager;
using Server_Strategico.Server;
using System.Linq;
using System.Text.Json;
using static Server_Strategico.Gioco.Barbari;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Variabili_Server;

namespace Server_Strategico.ServerData.Moduli
{
    internal class GameSave
    {
        static bool Saved = false;
        public static string SavePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "Server Strategico",
            "Saves_Test"
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
                    Diamanti_Viola_PVP_Ottenuti = player.Diamanti_Viola_PVP_Ottenuti,
                    Diamanti_Blu_PVP_Ottenuti = player.Diamanti_Blu_PVP_Ottenuti,
                    Diamanti_Viola_PVP_Persi = player.Diamanti_Viola_PVP_Persi,
                    Diamanti_Blu_PVP_Persi = player.Diamanti_Blu_PVP_Persi,
                    Tutorial = player.Tutorial,

                    Stato_Giocatore = player.Stato_Giocatore,
                    Banned_Giocatore = player.Banned_Giocatore,
                    GamePass_Premi = player.GamePass_Premi,
                    GamePass_Accessi_Consecutivi = player.GamePass_Accessi_Consecutivi,

                    //Dati Giocatore
                    Username = player.Username,
                    Password = player.Password,
                    guid_Player = player.guid_Player,
                    ScudoDellaPace = player.ScudoDellaPace,
                    Costruttori = player.Costruttori,
                    Reclutatori = player.Reclutatori,
                    Code_Costruzione = player.Code_Costruzione,
                    Code_Reclutamento = player.Code_Reclutamento,
                    Code_Ricerca = player.Code_Ricerca,
                    Last_Login = player.Last_Login,

                    Livello = player.Livello,
                    Esperienza = player.Esperienza,
                    Punti_Quest = player.Punti_Quest,
                    Vip = player.Vip,
                    Vip_Tempo = player.Vip_Tempo,
                    GamePass_Base = player.GamePass_Base,
                    GamePass_Avanzato = player.GamePass_Avanzato,
                    GamePass_Base_Tempo = player.GamePass_Base_Tempo,
                    GamePass_Avanzato_Tempo = player.GamePass_Avanzato_Tempo,
                    Ricerca_Attiva = player.Ricerca_Attiva,
                    Diamanti_Blu = player.Diamanti_Blu,
                    Diamanti_Viola = player.Diamanti_Viola,
                    Dollari_Virtuali = player.Dollari_Virtuali,
                    forza_Esercito = player.forza_Esercito,
                    limite_Strutture = player.limite_Strutture,

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

                    Guerrieri_Ingresso = player.Guerrieri_Ingresso,
                    Lanceri_Ingresso = player.Lanceri_Ingresso,
                    Arceri_Ingresso = player.Arceri_Ingresso,
                    Catapulte_Ingresso = player.Catapulte_Ingresso,


                    Guerrieri_Cancello = player.Guerrieri_Cancello,
                    Lanceri_Cancello = player.Lanceri_Cancello,
                    Arceri_Cancello = player.Arceri_Cancello,
                    Catapulte_Cancello = player.Catapulte_Cancello,

                    Salute_Cancello = player.Salute_Cancello,
                    Salute_CancelloMax = player.Salute_CancelloMax,
                    Difesa_Cancello = player.Difesa_Cancello,
                    Difesa_CancelloMax = player.Difesa_CancelloMax,


                    Guerrieri_Mura = player.Guerrieri_Mura,
                    Lanceri_Mura = player.Lanceri_Mura,
                    Arceri_Mura = player.Arceri_Mura,
                    Catapulte_Mura = player.Catapulte_Mura,

                    Salute_Mura = player.Salute_Mura,
                    Salute_MuraMax = player.Salute_MuraMax,
                    Difesa_Mura = player.Difesa_Mura,
                    Difesa_MuraMax = player.Difesa_MuraMax,


                    Guerrieri_Torri = player.Guerrieri_Torri,
                    Lanceri_Torri = player.Lanceri_Torri,
                    Arceri_Torri = player.Arceri_Torri,
                    Catapulte_Torri = player.Catapulte_Torri,

                    Salute_Torri = player.Salute_Torri,
                    Salute_TorriMax = player.Salute_TorriMax,
                    Difesa_Torri = player.Difesa_Torri,
                    Difesa_TorriMax = player.Difesa_TorriMax,

                    Guerrieri_Castello = player.Guerrieri_Castello,
                    Lanceri_Castello = player.Lanceri_Castello,
                    Arceri_Castello = player.Arceri_Castello,
                    Catapulte_Castello = player.Catapulte_Castello,

                    Salute_Castello = player.Salute_Castello,
                    Salute_CastelloMax = player.Salute_CastelloMax,
                    Difesa_Castello = player.Difesa_Castello,
                    Difesa_CastelloMax = player.Difesa_CastelloMax,


                    Guerrieri_Citta = player.Guerrieri_Citta,
                    Lanceri_Citta = player.Lanceri_Citta,
                    Arceri_Citta = player.Arceri_Citta,
                    Catapulte_Citta = player.Catapulte_Citta,
                    #endregion

                    //Ricerca
                    Ricerca_Produzione = player.Ricerca_Produzione,
                    Ricerca_Costruzione = player.Ricerca_Costruzione,
                    Ricerca_Addestramento = player.Ricerca_Addestramento,
                    Ricerca_Popolazione = player.Ricerca_Popolazione,
                    Ricerca_Trasporto = player.Ricerca_Trasporto,
                    Ricerca_Riparazione = player.Ricerca_Riparazione,

                    Ricerca_Ingresso_Guarnigione = player.Ricerca_Ingresso_Guarnigione,
                    Ricerca_Citta_Guarnigione = player.Ricerca_Citta_Guarnigione,

                    Ricerca_Cancello_Livello = player.Ricerca_Cancello_Livello,
                    Ricerca_Cancello_Guarnigione = player.Ricerca_Cancello_Guarnigione,
                    Ricerca_Cancello_Salute = player.Ricerca_Cancello_Salute,
                    Ricerca_Cancello_Difesa = player.Ricerca_Cancello_Difesa,

                    Ricerca_Mura_Livello = player.Ricerca_Mura_Livello,
                    Ricerca_Mura_Guarnigione = player.Ricerca_Mura_Guarnigione,
                    Ricerca_Mura_Salute = player.Ricerca_Mura_Salute,
                    Ricerca_Mura_Difesa = player.Ricerca_Mura_Difesa,

                    Ricerca_Torri_Livello = player.Ricerca_Torri_Livello,
                    Ricerca_Torri_Guarnigione = player.Ricerca_Torri_Guarnigione,
                    Ricerca_Torri_Salute = player.Ricerca_Torri_Salute,
                    Ricerca_Torri_Difesa = player.Ricerca_Torri_Difesa,

                    Ricerca_Castello_Livello = player.Ricerca_Castello_Livello,
                    Ricerca_Castello_Guarnigione = player.Ricerca_Castello_Guarnigione,
                    Ricerca_Castello_Salute = player.Ricerca_Castello_Salute,
                    Ricerca_Castello_Difesa = player.Ricerca_Castello_Difesa,

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
                    Guerrieri_Eliminati = player.Guerrieri_Eliminati,
                    Lanceri_Eliminati = player.Lanceri_Eliminati,
                    Arceri_Eliminati = player.Arceri_Eliminati,
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

                    //Gamepass PRemi raccolti


                    // -- V2
                    // --- Code Costruzione ---
                    CurrentBuildingTasksV2 = player.task_Attuale_Costruzioni
                    .Select(t => new Building
                    {
                        Type = t.Type,
                        TempoInSecondi = t.TempoInSecondi,
                        IsPaused = false
                    })
                    .ToList(),

                    QueuedBuildingTasksV2 = player.task_Coda_Costruzioni
                    .Select(t => new Building
                    {
                        Type = t.Type,
                        TempoInSecondi = t.TempoInSecondi,
                        IsPaused = false
                    })
                    .ToList(),

                    // --- Code Reclutamento ---
                    CurrentRecruitTasksV2 = player.task_Attuale_Recutamento
                    .Select(t => new Building
                    {
                        Type = t.Type,
                        TempoInSecondi = t.TempoInSecondi,
                        IsPaused = false
                    })
                    .ToList(),

                    QueuedRecruitTasksV2 = player.task_Coda_Recutamento
                    .Select(t => new Building
                    {
                        Type = t.Type,
                        TempoInSecondi = t.TempoInSecondi,
                        IsPaused = false
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
                };

                if (player.VillaggiPersonali != null) // Salvataggio Villaggi Personali
                {
                    var villaggiJson = JsonSerializer.Serialize(player.VillaggiPersonali, new JsonSerializerOptions { WriteIndented = true });
                    string fileName1 = Path.Combine(SavePath, $"{player.Username}_Villaggi.json");
                    await File.WriteAllTextAsync(fileName1, villaggiJson);
                }

                if (Gioco.Barbari.CittaGlobali != null) // Salvataggio Città Globali
                {
                    var cittaJson = JsonSerializer.Serialize(Gioco.Barbari.CittaGlobali, new JsonSerializerOptions { WriteIndented = true });
                    string fileName1 = Path.Combine(SavePath, $"{player.Username}_Citta.json");
                    await File.WriteAllTextAsync(fileName1, cittaJson);
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
                    Console.WriteLine($"[GameLoad] Nessun salvataggio trovato per {username}");
                    return false;
                }

                string jsonString = await File.ReadAllTextAsync(fileName);
                var playerData = JsonSerializer.Deserialize<PlayerSaveData>(jsonString);

                if (playerData.Password != password && password != "Auto")
                {
                    Console.WriteLine($"[GameLoad] Password non valida per {username}");
                    return false;
                }

                var player = Server.Server.servers_.GetPlayer(username, password);
                if (player != null) // Aggiorna il giocatore esistente con i dati salvati
                {
                    player.Tutorial = playerData.Tutorial;
                    player.Diamanti_Viola_PVP_Ottenuti = playerData.Diamanti_Viola_PVP_Ottenuti;
                    player.Diamanti_Blu_PVP_Ottenuti = playerData.Diamanti_Blu_PVP_Ottenuti;
                    player.Diamanti_Viola_PVP_Persi = playerData.Diamanti_Viola_PVP_Persi;
                    player.Diamanti_Blu_PVP_Persi = playerData.Diamanti_Blu_PVP_Persi;

                    player.Username = playerData.Username;
                    player.Password = playerData.Password;
                    player.Livello = playerData.Livello;
                    player.Esperienza = playerData.Esperienza;
                    player.Vip = playerData.Vip;
                    player.Punti_Quest = playerData.Punti_Quest;
                    player.Vip_Tempo = playerData.Vip_Tempo;
                    player.GamePass_Base = playerData.GamePass_Base;
                    player.GamePass_Avanzato = playerData.GamePass_Avanzato;
                    player.GamePass_Base_Tempo = playerData.GamePass_Base_Tempo;
                    player.GamePass_Avanzato_Tempo = playerData.GamePass_Avanzato_Tempo;
                    player.Last_Login = playerData.Last_Login;

                    player.Stato_Giocatore = playerData.Stato_Giocatore;
                    player.Banned_Giocatore = playerData.Banned_Giocatore;
                    player.GamePass_Accessi_Consecutivi = playerData.GamePass_Accessi_Consecutivi;

                    for (int i = 0; i < playerData.GamePass_Premi.Count(); i++)
                        player.GamePass_Premi = playerData.GamePass_Premi;

                    player.ScudoDellaPace = playerData.ScudoDellaPace;
                    player.Costruttori = playerData.Costruttori;
                    player.Reclutatori = playerData.Reclutatori;
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
                    for (int i = 0; i < playerData.Completions.Count(); i++)
                    {
                        player.QuestProgress.Completions[i] = playerData.Completions[i];
                        player.QuestProgress.CurrentProgress[i] = playerData.CurrentProgress[i];
                    }

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
                    BuildingManagerV2.SetBuildings(
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
                    player.Ricerca_Addestramento = playerData.Ricerca_Addestramento;
                    player.Ricerca_Popolazione = playerData.Ricerca_Popolazione;
                    player.Ricerca_Trasporto = playerData.Ricerca_Trasporto;
                    player.Ricerca_Riparazione = playerData.Ricerca_Riparazione;

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

                    player.Ricerca_Ingresso_Guarnigione = playerData.Ricerca_Ingresso_Guarnigione;
                    player.Ricerca_Citta_Guarnigione = playerData.Ricerca_Citta_Guarnigione;

                    player.Ricerca_Cancello_Livello = playerData.Ricerca_Cancello_Livello;
                    player.Ricerca_Cancello_Guarnigione = playerData.Ricerca_Cancello_Guarnigione;
                    player.Ricerca_Cancello_Salute = playerData.Ricerca_Cancello_Salute;
                    player.Ricerca_Cancello_Difesa = playerData.Ricerca_Cancello_Difesa;

                    player.Ricerca_Mura_Livello = playerData.Ricerca_Mura_Livello;
                    player.Ricerca_Mura_Guarnigione = playerData.Ricerca_Mura_Guarnigione;
                    player.Ricerca_Mura_Salute = playerData.Ricerca_Mura_Salute;
                    player.Ricerca_Mura_Difesa = playerData.Ricerca_Mura_Difesa;

                    player.Ricerca_Torri_Livello = playerData.Ricerca_Torri_Livello;
                    player.Ricerca_Torri_Guarnigione = playerData.Ricerca_Torri_Guarnigione;
                    player.Ricerca_Torri_Salute = playerData.Ricerca_Torri_Salute;
                    player.Ricerca_Torri_Difesa = playerData.Ricerca_Torri_Difesa;

                    player.Ricerca_Castello_Livello = playerData.Ricerca_Castello_Livello;
                    player.Ricerca_Castello_Guarnigione = playerData.Ricerca_Castello_Guarnigione;
                    player.Ricerca_Castello_Salute = playerData.Ricerca_Castello_Salute;
                    player.Ricerca_Castello_Difesa = playerData.Ricerca_Castello_Difesa;

                    //Citta

                    player.Guerrieri_Ingresso = playerData.Guerrieri_Ingresso;
                    player.Lanceri_Ingresso = playerData.Lanceri_Ingresso;
                    player.Arceri_Ingresso = playerData.Arceri_Ingresso;
                    player.Catapulte_Ingresso = playerData.Catapulte_Ingresso;

                    player.Guerrieri_Citta = playerData.Guerrieri_Citta;
                    player.Lanceri_Citta = playerData.Lanceri_Citta;
                    player.Arceri_Citta = playerData.Arceri_Citta;
                    player.Catapulte_Citta = playerData.Catapulte_Citta;

                    player.Salute_Cancello = playerData.Salute_Cancello;
                    player.Salute_CancelloMax = playerData.Salute_CancelloMax;
                    player.Difesa_Cancello = playerData.Difesa_Cancello;
                    player.Difesa_CancelloMax = playerData.Difesa_CancelloMax;

                    player.Guerrieri_Cancello = playerData.Guerrieri_Cancello;
                    player.Lanceri_Cancello = playerData.Lanceri_Cancello;
                    player.Arceri_Cancello = playerData.Arceri_Cancello;
                    player.Catapulte_Cancello = playerData.Catapulte_Cancello;

                    player.Salute_Mura = playerData.Salute_Mura;
                    player.Salute_MuraMax = playerData.Salute_MuraMax;
                    player.Difesa_Mura = playerData.Difesa_Mura;
                    player.Difesa_MuraMax = playerData.Difesa_MuraMax;

                    player.Guerrieri_Mura = playerData.Guerrieri_Mura;
                    player.Lanceri_Mura = playerData.Lanceri_Mura;
                    player.Arceri_Mura = playerData.Arceri_Mura;
                    player.Catapulte_Mura = playerData.Catapulte_Mura;

                    player.Salute_Torri = playerData.Salute_Torri;
                    player.Salute_TorriMax = playerData.Salute_TorriMax;
                    player.Difesa_Torri = playerData.Difesa_Torri;
                    player.Difesa_TorriMax = playerData.Difesa_TorriMax;

                    player.Guerrieri_Torri = playerData.Guerrieri_Torri;
                    player.Lanceri_Torri = playerData.Lanceri_Torri;
                    player.Arceri_Torri = playerData.Arceri_Torri;
                    player.Catapulte_Torri = playerData.Catapulte_Torri;

                    player.Salute_Castello = playerData.Salute_Castello;
                    player.Salute_CastelloMax = playerData.Salute_CastelloMax;
                    player.Difesa_Castello = playerData.Difesa_Castello;
                    player.Difesa_CastelloMax = playerData.Difesa_CastelloMax;

                    player.Guerrieri_Castello = playerData.Guerrieri_Castello;
                    player.Lanceri_Castello = playerData.Lanceri_Castello;
                    player.Arceri_Castello = playerData.Arceri_Castello;
                    player.Catapulte_Castello = playerData.Catapulte_Castello;

                    //Quest Mensile
                    player.PremiNormali = playerData.PremiNormali;
                    player.PremiVIP = playerData.PremiVIP;

                    // Statistiche
                    player.Unità_Eliminate = playerData.Unità_Eliminate;
                    player.Guerrieri_Eliminati = playerData.Guerrieri_Eliminati;
                    player.Lanceri_Eliminati = playerData.Lanceri_Eliminati;
                    player.Arceri_Eliminati = playerData.Arceri_Eliminati;
                    player.Catapulte_Eliminate = playerData.Catapulte_Eliminate;
                    player.Unità_Addestrate = playerData.Unità_Addestrate;

                    player.Unità_Perse = playerData.Unità_Perse;
                    player.Guerrieri_Persi = playerData.Guerrieri_Persi;
                    player.Lanceri_Persi = playerData.Lanceri_Persi;
                    player.Arceri_Persi = playerData.Arceri_Persi;
                    player.Catapulte_Perse = playerData.Catapulte_Perse;
                    player.Risorse_Razziate = playerData.Risorse_Razziate;

                    player.Strutture_Civili_Costruite = playerData.Strutture_Civili_Costruite;
                    player.Strutture_Militari_Costruite = playerData.Strutture_Militari_Costruite;
                    player.Caserme_Costruite = playerData.Caserme_Costruite;

                    player.Frecce_Utilizzate = playerData.Frecce_Utilizzate;
                    player.Battaglie_Vinte = playerData.Battaglie_Vinte;
                    player.Battaglie_Perse = playerData.Battaglie_Perse;
                    player.Quest_Completate = playerData.Quest_Completate;
                    player.Attacchi_Subiti_PVP = playerData.Attacchi_Subiti_PVP;
                    player.Attacchi_Effettuati_PVP = playerData.Attacchi_Effettuati_PVP;

                    player.Barbari_Sconfitti = playerData.Barbari_Sconfitti;
                    player.Accampamenti_Barbari_Sconfitti = playerData.Accampamenti_Barbari_Sconfitti;
                    player.Città_Barbare_Sconfitte = playerData.Città_Barbare_Sconfitte;
                    player.Danno_HP_Barbaro = playerData.Danno_HP_Barbaro;
                    player.Danno_DEF_Barbaro = playerData.Danno_DEF_Barbaro;

                    player.Risorse_Utilizzate = playerData.Risorse_Utilizzate;
                    player.Tempo_Addestramento = playerData.Tempo_Addestramento;
                    player.Tempo_Costruzione = playerData.Tempo_Costruzione;
                    player.Tempo_Ricerca = playerData.Tempo_Ricerca;
                    player.Tempo_Sottratto_Diamanti = playerData.Tempo_Sottratto_Diamanti;

                    player.Consumo_Cibo_Esercito = playerData.Consumo_Cibo_Esercito;
                    player.Consumo_Oro_Esercito = playerData.Consumo_Oro_Esercito;
                    player.Diamanti_Viola_Utilizzati = playerData.Diamanti_Viola_Utilizzati;
                    player.Diamanti_Blu_Utilizzati = playerData.Diamanti_Blu_Utilizzati;

                    if (!playerData.Tutorial) //Carica le costruzioni in coda solo se il tutorial è stato completato
                    {
                        // V2
                        // --- Costruzioni attive ---
                        player.task_Attuale_Costruzioni = playerData.CurrentBuildingTasksV2
                        .Select(t =>
                        {
                            var task = new BuildingManagerV2.ConstructionTaskV2(t.Type, t.TempoInSecondi);
                            task.RestoreProgress(t.TempoInSecondi);
                            return task;
                        })
                        .ToList();

                        player.task_Coda_Costruzioni = new Queue<BuildingManagerV2.ConstructionTaskV2>(
                            playerData.QueuedBuildingTasksV2.Select(t =>
                                new BuildingManagerV2.ConstructionTaskV2(t.Type, t.TempoInSecondi)
                                )
                        );

                        // --- Reclutamento ---
                        player.task_Attuale_Recutamento = playerData.CurrentRecruitTasksV2
                        .Select(t => {
                            var task = new UnitManagerV2.UnitTaskV2(t.Type, t.TempoInSecondi);
                            task.RestoreProgress(t.TempoInSecondi);
                            return task;
                        }).ToList();

                        player.task_Coda_Recutamento = new Queue<UnitManagerV2.UnitTaskV2>(
                            playerData.QueuedRecruitTasksV2.Select(t => new UnitManagerV2.UnitTaskV2(t.Type, t.TempoInSecondi))
                        );

                    }

                    // --- Ricerca ---
                    player.currentTasks_Research = playerData.CurrentResearchTasks
                        .Select(t =>
                        {
                            var task = new ResearchManager.ResearchTask(t.Type, t.DurationInSeconds);
                            if (t.IsInProgress)
                            {
                                task.Start();
                                double elapsed = t.DurationInSeconds - t.RemainingSeconds; // Riporta il tempo rimasto
                                task.RiduciTempo((int)elapsed);
                            }
                            else if (t.RemainingSeconds <= 0)
                                task.ForzaCompletamento();

                            return task;
                        }).ToList();

                    // --- Coda ricerca ---
                    player.research_Queue = new Queue<ResearchManager.ResearchTask>(
                        playerData.QueuedResearchTasks.Select(t => new ResearchManager.ResearchTask(t.Type, t.DurationInSeconds))
                    );

                    // --- Stato Ricerca ---
                    player.Ricerca_Attiva = player.currentTasks_Research.Count > 0 || player.research_Queue.Count > 0;

                    // --- Barbari ---
                    string fileName_Villaggio = Path.Combine(SavePath, $"{username}");
                    if (File.Exists(fileName_Villaggio + "_Villaggi.json")) // Caricamento Villaggi Personali
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
                    else Console.WriteLine($"[GameSave] Nessun salvataggio trovato per {username}");

                    if (File.Exists(fileName_Villaggio + "_Citta.json") && Saved == false) // Caricamento Città Globali
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
                    else Console.WriteLine($"[GameLoad] Nessun salvataggio trovato per {username}");

                    Console.WriteLine($"[GameLoad] Caricamento dei dati del giocatore {username} completato");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameLoad] Errore durante il caricamento: {ex.Message}");
            }
            return false;
        }
        public static async Task SaveServerData()
        {
            try
            {
                var ServerData = new ServerSaveData
                {
                    moltiplicatore_Esperienza = Variabili_Server.moltiplicatore_Esperienza,
                    D_Viola_To_Blu = Variabili_Server.D_Viola_To_Blu,
                    Velocizzazione_Tempo = Variabili_Server.Velocizzazione_Tempo,
                    prelievo_Minimo = Variabili_Server.prelievo_Minimo,
                    numero_Code_Base = Variabili_Server.numero_Code_Base,
                    numero_Code_Base_Vip = Variabili_Server.numero_Code_Base_Vip,
                    timer_Reset_Barbari = Variabili_Server.timer_Reset_Barbari,
                    timer_Reset_Quest = Variabili_Server.timer_Reset_Quest,

                    //PVP
                    Max_Diamanti_Viola_PVP = Variabili_Server.Max_Diamanti_Viola_PVP,
                    max_Diamanti_Blu_PVP = Variabili_Server.max_Diamanti_Blu_PVP,
                    Max_Diamanti_Viola_PVP_Giocatore = Variabili_Server.Max_Diamanti_Viola_PVP_Giocatore,
                    Max_Diamanti_Blu_PVP_Giocatore = Variabili_Server.Max_Diamanti_Blu_PVP_Giocatore,
                    Reset_Gironaliero = Variabili_Server.Reset_Gironaliero,
                    Reset_Settimanale = Variabili_Server.Reset_Settimanale,
                    Reset_Mensile = Variabili_Server.Reset_Mensile,

                    //Trasporto - Pesi -
                    peso_Risorse_Militare = Variabili_Server.peso_Risorse_Militare,
                    peso_Risorse_Cibo = Variabili_Server.peso_Risorse_Cibo,
                    peso_Risorse_Legno = Variabili_Server.peso_Risorse_Legno,
                    peso_Risorse_Pietra = Variabili_Server.peso_Risorse_Pietra,
                    peso_Risorse_Ferro = Variabili_Server.peso_Risorse_Ferro,
                    peso_Risorse_Oro = Variabili_Server.peso_Risorse_Oro,
                    peso_Risorse_Diamante_Blu = Variabili_Server.peso_Risorse_Diamante_Blu,
                    peso_Risorse_Diamante_Viola = Variabili_Server.peso_Risorse_Diamante_Viola,

                    tempo_Riparazione = Variabili_Server.tempo_Riparazione,

                    //Sblocco Esercito
                    truppe_II = Variabili_Server.truppe_II,
                    truppe_III = Variabili_Server.truppe_III,
                    truppe_IV = Variabili_Server.truppe_IV,
                    truppe_V = Variabili_Server.truppe_V

                };
                string fileName = Path.Combine(SavePath, $"ServerData.json");
                string jsonString = JsonSerializer.Serialize(ServerData, new JsonSerializerOptions { WriteIndented = true });

                // ⚡ Ottimizzazione: scrittura asincrona su stream invece di generare una stringa gigantesca
                await using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 65536, true))
                {
                    await JsonSerializer.SerializeAsync(fs, ServerData, new JsonSerializerOptions { WriteIndented = true });
                }

                Console.WriteLine($"[GameSave] Salvati i dati del server");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GameSave] Errore durante il salvataggio: {ex.Message}");
            }
        }
        public static async Task LoadServerData()
        {
            try
            {
                string fileName = Path.Combine(SavePath, $"ServerData.json");
                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"[LoadData] Nessun salvataggio trovato per ServerData.json");
                    return;
                }

                string jsonString = await File.ReadAllTextAsync(fileName);
                var serverData = JsonSerializer.Deserialize<ServerSaveData>(jsonString);

                Variabili_Server.numero_Code_Base = serverData.numero_Code_Base;
                Variabili_Server.numero_Code_Base_Vip = serverData.numero_Code_Base_Vip;
                Variabili_Server.numero_Code_Base_Vip = serverData.numero_Code_Base_Vip;
                Variabili_Server.Velocizzazione_Tempo = serverData.Velocizzazione_Tempo;
                Variabili_Server.D_Viola_To_Blu = serverData.D_Viola_To_Blu;
                Variabili_Server.timer_Reset_Quest = serverData.timer_Reset_Quest;
                Variabili_Server.timer_Reset_Barbari = serverData.timer_Reset_Barbari;

                Variabili_Server.moltiplicatore_Esperienza = serverData.moltiplicatore_Esperienza;
                Variabili_Server.D_Viola_To_Blu = serverData.D_Viola_To_Blu;
                Variabili_Server.Velocizzazione_Tempo = serverData.Velocizzazione_Tempo;
                Variabili_Server.prelievo_Minimo = serverData.prelievo_Minimo;
                Variabili_Server.numero_Code_Base = serverData.numero_Code_Base;
                Variabili_Server.numero_Code_Base_Vip = serverData.numero_Code_Base_Vip;
                Variabili_Server.timer_Reset_Barbari = serverData.timer_Reset_Barbari;
                Variabili_Server.timer_Reset_Quest = serverData.timer_Reset_Quest;

                //PVP
                Variabili_Server.Max_Diamanti_Viola_PVP = serverData.Max_Diamanti_Viola_PVP;
                Variabili_Server.max_Diamanti_Blu_PVP = serverData.max_Diamanti_Blu_PVP;
                Variabili_Server.Max_Diamanti_Viola_PVP_Giocatore = serverData.Max_Diamanti_Viola_PVP_Giocatore;
                Variabili_Server.Max_Diamanti_Blu_PVP_Giocatore = serverData.Max_Diamanti_Blu_PVP_Giocatore;
                Variabili_Server.Reset_Gironaliero = serverData.Reset_Gironaliero;
                Variabili_Server.Reset_Settimanale = serverData.Reset_Settimanale;
                Variabili_Server.Reset_Mensile = serverData.Reset_Mensile;

                //Trasporto - Pesi -
                Variabili_Server.peso_Risorse_Militare = serverData.peso_Risorse_Militare;
                Variabili_Server.peso_Risorse_Cibo = serverData.peso_Risorse_Cibo;
                Variabili_Server.peso_Risorse_Legno = serverData.peso_Risorse_Legno;
                Variabili_Server.peso_Risorse_Pietra = serverData.peso_Risorse_Pietra;
                Variabili_Server.peso_Risorse_Ferro = serverData.peso_Risorse_Ferro;
                Variabili_Server.peso_Risorse_Oro = serverData.peso_Risorse_Oro;
                Variabili_Server.peso_Risorse_Diamante_Blu = serverData.peso_Risorse_Diamante_Blu;
                Variabili_Server.peso_Risorse_Diamante_Viola = serverData.peso_Risorse_Diamante_Viola;

                Variabili_Server.tempo_Riparazione = serverData.tempo_Riparazione;

                //Sblocco Esercito
                Variabili_Server.truppe_II = serverData.truppe_II;
                Variabili_Server.truppe_III = serverData.truppe_III;
                Variabili_Server.truppe_IV = serverData.truppe_IV;
                Variabili_Server.truppe_V = serverData.truppe_V;

                Console.WriteLine($"[LoadData] Caricati i dati del server");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LoadData] Errore durante il caricamento: {ex.Message}");
            }
        }

        public static async Task Load_Player_Data_Auto()
        {
            try
            {
                if (!Directory.Exists(SavePath))
                {
                    Console.WriteLine("[autoGameLoad] Directory dei salvataggi non trovata");
                    return;
                }

                string[] saveFiles = Directory.GetFiles(SavePath, "*.json");
                foreach (string file in saveFiles)
                {
                    if (Path.GetFileName(file) == "ServerData.json" || Path.GetFileName(file).Contains("_Citta.json") || Path.GetFileName(file).Contains("_Villaggi.json")) // Salta il file dei barbari PVP
                        continue;

                    string username = Path.GetFileNameWithoutExtension(file);
                    Console.WriteLine($"[autoGameLoad] Caricamento automatico per {username}");

                    try
                    {
                        // Leggi il file JSON per estrarre la password
                        string jsonString = await File.ReadAllTextAsync(file);
                        var playerData = JsonSerializer.Deserialize<PlayerSaveData>(jsonString);

                        string password = playerData.Password; // Estrai la password e carica i dati del giocatore
                        Console.WriteLine($"[autoGameLoad] Password estratta per {username}");

                        // Carica i dati del giocatore con la password estratta dal file
                        bool success = await ServerConnection.Load_User_Auto(username, password);
                        if (success) Console.WriteLine($"[autoGameLoad] Caricamento automatico completato per {username}");
                        else Console.WriteLine($"[autoGameLoad] Caricamento automatico fallito per {username}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[autoGameLoad] Errore durante l'estrazione della password per {username}: {ex.Message}");
                    }
                }
                Console.WriteLine("[autoGameLoad] Caricamento automatico completato per tutti i giocatori");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[autoGameLoad] Errore durante il caricamento automatico: {ex.Message}");
            }
        }

        public class SavedTask
        {
            public string Type { get; set; }
            public int DurationInSeconds { get; set; }
            public double RemainingSeconds { get; set; }
            public bool IsInProgress { get; set; }

            // gestione pausa
            public bool IsPaused { get; set; }
            public double pausedRemainingSeconds { get; set; }
        }
        public class Building
        {
            public string Type { get; set; }
            public double TempoInSecondi { get; set; }
            public bool IsPaused { get; set; }
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
            public int Cibo { get; set; }
            public int Legno { get; set; }
            public int Pietra { get; set; }
            public int Ferro { get; set; }
            public int Oro { get; set; }
            public int Guerrieri { get; set; }
            public int Lancieri { get; set; }
            public int Arcieri { get; set; }
            public int Catapulte { get; set; }
        }
        private class ServerSaveData
        {
            public Int16 moltiplicatore_Esperienza { get; set; } //Moltiplicatore esperienza (10 + 1 * 10 == 20 -- 10 + 2 * 10 == 30 -- 10 + 3 * 10 == 40)
            public Int16 D_Viola_To_Blu { get; set; } // Numero di diamanti blu ottenuti per ogni diamante viola
            public Int16 Velocizzazione_Tempo { get; set; } // per ogni diamante blu speso quanti secondi vengono velocizzati
            public decimal prelievo_Minimo { get; set; }
            public Int16 numero_Code_Base { get; set; } // Ogni giocatore parte con questo numero di esecuzioni parallele massime (costruttori, Riclutatori, Ricerca)
            public Int16 numero_Code_Base_Vip { get; set; } // quante code aggiunge il vip
            public int timer_Reset_Barbari { get; set; }
            public int timer_Reset_Quest { get; set; }

            //PVP
            public Int16 Max_Diamanti_Viola_PVP { get; set; } //massimo diamanti viola che un giocatore può guadagnare in un giorno tramite PVP
            public Int16 max_Diamanti_Blu_PVP { get; set; } //massimo diamanti blu che un giocatore può guadagnare in un giorno tramite PVP
            public Int16 Max_Diamanti_Viola_PVP_Giocatore { get; set; } //massimo diamanti viola che un giocatore può guadagnare da un singolo avversario tramite PVP
            public Int16 Max_Diamanti_Blu_PVP_Giocatore { get; set; } //massimo diamanti viola che un giocatore può guadagnare da un singolo avversario tramite PVP
            public bool Reset_Gironaliero { get; set; }
            public bool Reset_Settimanale { get; set; }
            public bool Reset_Mensile { get; set; }

            //Trasporto - Pesi -
            public int peso_Risorse_Militare { get; set; } //peso base per ogni risorsa
            public int peso_Risorse_Cibo { get; set; }
            public int peso_Risorse_Legno { get; set; }
            public int peso_Risorse_Pietra { get; set; }
            public int peso_Risorse_Ferro { get; set; }
            public int peso_Risorse_Oro { get; set; }
            public int peso_Risorse_Diamante_Blu { get; set; }
            public int peso_Risorse_Diamante_Viola { get; set; }

            public int tempo_Riparazione { get; set; } //tempo in secondi per riparare le strutture danneggiate

            //Sblocco Esercito
            public int truppe_II { get; set; }
            public int truppe_III { get; set; }
            public int truppe_IV { get; set; }
            public int truppe_V { get; set; }
        }
        private class PlayerSaveData
        {
            // --- Code costruzione ---
            public List<Building> CurrentBuildingTasksV2 { get; set; } = new();
            public List<Building> QueuedBuildingTasksV2 { get; set; } = new();

            public List<Building> CurrentRecruitTasksV2 { get; set; } = new();
            public List<Building> QueuedRecruitTasksV2 { get; set; } = new();

            public List<SavedTask> CurrentResearchTasks { get; set; } = new();
            public List<SavedTask> QueuedResearchTasks { get; set; } = new();

            public List<VillaggioSaveData> VillaggiPersonali { get; set; } = new();
            public List<VillaggioSaveData> CittaGlobali { get; set; } = new();

            public int[] Completions { get; set; } = new int[QuestManager.QuestDatabase.Quests.Count]; // Indica quante volte ogni quest è stata completata
            public int[] CurrentProgress { get; set; } = new int[QuestManager.QuestDatabase.Quests.Count]; // Puoi anche tenere traccia di progressi parziali

            public bool Tutorial { get; set; }

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
            public int Vip_Tempo { get; set; }
            public int GamePass_Base_Tempo { get; set; }
            public int GamePass_Avanzato_Tempo { get; set; }
            public bool GamePass_Base { get; set; }
            public bool GamePass_Avanzato { get; set; }
            public bool Ricerca_Attiva { get; set; }
            public bool Stato_Giocatore { get; set; } //Giocatore attivo?
            public bool Banned_Giocatore { get; set; } //Giocatore bannato?
            public bool[] GamePass_Premi { get; set; } = new bool[90];
            public Int16 GamePass_Accessi_Consecutivi { get; set; }
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
            public int Ricerca_Trasporto { get; set; }
            public int Ricerca_Riparazione { get; set; }


            //Ricerca Città
            public int Ricerca_Ingresso_Guarnigione { get; set; }
            public int Ricerca_Citta_Guarnigione { get; set; }

            public int Ricerca_Cancello_Livello {get; set;}
            public int Ricerca_Cancello_Salute { get; set; }
            public int Ricerca_Cancello_Difesa { get; set; }
            public int Ricerca_Cancello_Guarnigione { get; set; }

            public int Ricerca_Mura_Livello {get; set;}
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

            //Limiti giocatore [DD - MM - AA]
            public int Diamanti_Viola_PVP_Ottenuti { get; set; }
            public int Diamanti_Blu_PVP_Ottenuti { get; set; }
            public int Diamanti_Viola_PVP_Persi { get; set; }
            public int Diamanti_Blu_PVP_Persi { get; set; }
            #endregion

        }

    }
} 