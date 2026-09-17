using Server_Strategico.Gioco;
using Server_Strategico.Manager;
using System.Text;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Strutture;

namespace Server_Strategico.ServerData.Moduli
{
    public class PlayerSnapshot
    {
        // 16/09/2026: sostituite le due Dictionary<string,string> per giocatore con due
        // array a lunghezza fissa indicizzati da Field sotto (vedi analisi memoria del
        // 16/09/2026 a 500k giocatori simulati: le sole due Dictionary<string,string> per
        // giocatore, con i loro array interni di bucket/entry, pesavano da sole circa il
        // 75% dell'intero heap gestito del server). Field è l'unica fonte di verità per i
        // nomi dei campi: Keys viene generato UNA VOLTA per tutto il processo leggendo i
        // nomi dell'enum via riflessione, non duplicato a mano, quindi Keys[i] corrisponde
        // sempre a Field con valore i.
        //
        // Per aggiungere un campo nuovo quando il gioco si espande: aggiungi un membro in
        // fondo all'enum Field, poi UNA riga dentro BuildCurrentState qui sotto che scrive
        // _currentState[(int)Field.<NomeNuovoCampo>] con il valore del nuovo campo.
        // Nient'altro da toccare: Keys si aggiorna da solo. _lastSent/_currentState non
        // vengono mai salvati su disco (sono solo cache di sincronizzazione col client,
        // azzerate ad ogni login da Reset()), quindi non c'è nessun problema di
        // compatibilità con salvataggi vecchi nel farlo.
        private enum Field
        {
            livello,
            esperienza,
            vip,
            vip_Tempo,
            Scudo_Tempo,
            Costruttori_Tempo,
            Reclutatori_Tempo,
            GamePass_Base,
            GamePass_Base_Tempo,
            GamePass_Avanzato,
            GamePass_Avanzato_Tempo,
            QuestMensili_Tempo,
            Barbari_Tempo,
            Giorni_Consecutivi,
            cibo,
            legna,
            pietra,
            ferro,
            oro,
            popolazione,
            dollari_virtuali,
            diamanti_blu,
            diamanti_viola,
            cibo_max,
            legno_max,
            pietra_max,
            ferro_max,
            oro_max,
            popolazione_max,
            cibo_s,
            legna_s,
            pietra_s,
            ferro_s,
            oro_s,
            popolazione_s,
            spade_s,
            lance_s,
            archi_s,
            scudi_s,
            armature_s,
            frecce_s,
            consumo_cibo_s,
            consumo_oro_s,
            consumo_cibo_strutture,
            consumo_legno_strutture,
            consumo_pietra_strutture,
            consumo_ferro_strutture,
            consumo_oro_strutture,
            cibo_limite,
            legna_limite,
            pietra_limite,
            ferro_limite,
            oro_limite,
            popolazione_limite,
            spade_limite,
            lance_limite,
            archi_limite,
            scudi_limite,
            armature_limite,
            frecce_limite,
            spade,
            lance,
            archi,
            scudi,
            armature,
            frecce,
            spade_max,
            lance_max,
            archi_max,
            scudi_max,
            armature_max,
            frecce_max,
            fattorie,
            segherie,
            cave_pietra,
            miniere_ferro,
            miniere_oro,
            @case,
            comune,
            noncomune,
            raro,
            epico,
            leggendario,
            workshop_spade,
            workshop_lance,
            workshop_archi,
            workshop_scudi,
            workshop_armature,
            workshop_frecce,
            caserma_guerrieri,
            caserma_lanceri,
            caserma_arceri,
            caserma_catapulte,
            guerrieri_max,
            lanceri_max,
            arceri_max,
            catapulte_max,
            fattoria_coda,
            segheria_coda,
            cavapietra_coda,
            minieraferro_coda,
            minieraoro_coda,
            casa_coda,
            workshop_spade_coda,
            workshop_lance_coda,
            workshop_archi_coda,
            workshop_scudi_coda,
            workshop_armature_coda,
            workshop_frecce_coda,
            caserma_guerrieri_coda,
            caserma_lanceri_coda,
            caserma_arceri_coda,
            caserma_catapulte_coda,
            guerrieri_1,
            guerrieri_2,
            guerrieri_3,
            guerrieri_4,
            guerrieri_5,
            lanceri_1,
            lanceri_2,
            lanceri_3,
            lanceri_4,
            lanceri_5,
            arceri_1,
            arceri_2,
            arceri_3,
            arceri_4,
            arceri_5,
            catapulte_1,
            catapulte_2,
            catapulte_3,
            catapulte_4,
            catapulte_5,
            guerrieri_1_coda,
            guerrieri_2_coda,
            guerrieri_3_coda,
            guerrieri_4_coda,
            guerrieri_5_coda,
            lanceri_1_coda,
            lanceri_2_coda,
            lanceri_3_coda,
            lanceri_4_coda,
            lanceri_5_coda,
            arceri_1_coda,
            arceri_2_coda,
            arceri_3_coda,
            arceri_4_coda,
            arceri_5_coda,
            catapulte_1_coda,
            catapulte_2_coda,
            catapulte_3_coda,
            catapulte_4_coda,
            catapulte_5_coda,
            forza_esercito,
            ricerca_produzione,
            ricerca_costruzione,
            ricerca_addestramento,
            ricerca_popolazione,
            ricerca_riparazione,
            ricerca_trasporto,
            ricerca_Spionaggio,
            ricerca_Contro_Spionaggio,
            ricerca_ingresso_livello,
            ricerca_ingresso_guarnigione,
            ricerca_citta_livello,
            ricerca_citta_guarnigione,
            ricerca_cancello_livello,
            ricerca_cancello_salute,
            ricerca_cancello_difesa,
            ricerca_cancello_guarnigione,
            ricerca_mura_livello,
            ricerca_mura_salute,
            ricerca_mura_difesa,
            ricerca_mura_guarnigione,
            ricerca_torri_livello,
            ricerca_torri_salute,
            ricerca_torri_difesa,
            ricerca_torri_guarnigione,
            ricerca_castello_livello,
            ricerca_castello_salute,
            ricerca_castello_difesa,
            ricerca_castello_guarnigione,
            Ricerca_Attiva,
            guerriero_salute,
            guerriero_difesa,
            guerriero_attacco,
            guerriero_livello,
            lancere_salute,
            lancere_difesa,
            lancere_attacco,
            lancere_livello,
            arcere_salute,
            arcere_difesa,
            arcere_attacco,
            arcere_livello,
            catapulta_salute,
            catapulta_difesa,
            catapulta_attacco,
            catapulta_livello,
            Code_Costruzioni,
            Code_Reclutamenti,
            Code_Costruzioni_Disponibili,
            Code_Reclutamenti_Disponibili,
            Tempo_Costruzione,
            Tempo_Reclutamento,
            Tempo_Ricerca_Citta,
            Tempo_Ricerca_Globale,
            Potenza_Totale,
            Potenza_Strutture,
            Potenza_Ricerca,
            Potenza_Esercito,
            Unità_Eliminate,
            Guerrieri_Eliminate,
            Lanceri_Eliminate,
            Arceri_Eliminate,
            Catapulte_Eliminate,
            Unità_Perse,
            Guerrieri_Persi,
            Lanceri_Persi,
            Arceri_Persi,
            Catapulte_Persi,
            Risorse_Razziate,
            Strutture_Civili_Costruite,
            Strutture_Militari_Costruite,
            Caserme_Costruite,
            Frecce_Utilizzate,
            Battaglie_Vinte,
            Battaglie_Perse,
            Quest_Completate,
            Attacchi_Subiti_PVP,
            Attacchi_Effettuati_PVP,
            Barbari_Sconfitti,
            Accampamenti_Barbari_Sconfitti,
            Città_Barbare_Sconfitte,
            Danno_HP_Barbaro,
            Danno_DEF_Barbaro,
            Unità_Addestrate,
            Risorse_Utilizzate,
            Tempo_Addestramento_Risparmiato,
            Tempo_Costruzione_Risparmiato,
            Tempo_Ricerca_Risparmiato,
            Tempo_Sottratto_Diamanti,
            Bonus_Costruzione,
            Bonus_Addestramento,
            Bonus_Ricerca,
            Bonus_Riparazione,
            Bonus_Produzione_Risorse,
            Bonus_Capacità_Trasporto,
            Bonus_Salute_Strutture,
            Bonus_Difesa_Strutture,
            Bonus_Guarnigione_Strutture,
            Bonus_Attacco_Guerrieri,
            Bonus_Salute_Guerrieri,
            Bonus_Difesa_Guerrieri,
            Bonus_Attacco_Lanceri,
            Bonus_Salute_Lanceri,
            Bonus_Difesa_Lanceri,
            Bonus_Attacco_Arceri,
            Bonus_Salute_Arceri,
            Bonus_Difesa_Arceri,
            Bonus_Attacco_Catapulte,
            Bonus_Salute_Catapulte,
            Bonus_Difesa_Catapulte,
            punti_quest,
            Guarnigione_Ingresso,
            Guarnigione_IngressoMax,
            Guerrieri_1_Ingresso,
            Guerrieri_2_Ingresso,
            Guerrieri_3_Ingresso,
            Guerrieri_4_Ingresso,
            Guerrieri_5_Ingresso,
            Lanceri_1_Ingresso,
            Lanceri_2_Ingresso,
            Lanceri_3_Ingresso,
            Lanceri_4_Ingresso,
            Lanceri_5_Ingresso,
            Arceri_1_Ingresso,
            Arceri_2_Ingresso,
            Arceri_3_Ingresso,
            Arceri_4_Ingresso,
            Arceri_5_Ingresso,
            Catapulte_1_Ingresso,
            Catapulte_2_Ingresso,
            Catapulte_3_Ingresso,
            Catapulte_4_Ingresso,
            Catapulte_5_Ingresso,
            Salute_Cancello,
            Salute_CancelloMax,
            Difesa_Cancello,
            Difesa_CancelloMax,
            Riparazione_Cancello_Salute,
            Riparazione_Cancello_Difesa,
            Guarnigione_Cancello,
            Guarnigione_CancelloMax,
            Guerrieri_1_Cancello,
            Guerrieri_2_Cancello,
            Guerrieri_3_Cancello,
            Guerrieri_4_Cancello,
            Guerrieri_5_Cancello,
            Lanceri_1_Cancello,
            Lanceri_2_Cancello,
            Lanceri_3_Cancello,
            Lanceri_4_Cancello,
            Lanceri_5_Cancello,
            Arceri_1_Cancello,
            Arceri_2_Cancello,
            Arceri_3_Cancello,
            Arceri_4_Cancello,
            Arceri_5_Cancello,
            Catapulte_1_Cancello,
            Catapulte_2_Cancello,
            Catapulte_3_Cancello,
            Catapulte_4_Cancello,
            Catapulte_5_Cancello,
            Salute_Mura,
            Salute_MuraMax,
            Difesa_Mura,
            Difesa_MuraMax,
            Riparazione_Mura_Salute,
            Riparazione_Mura_Difesa,
            Guarnigione_Mura,
            Guarnigione_MuraMax,
            Guerrieri_1_Mura,
            Guerrieri_2_Mura,
            Guerrieri_3_Mura,
            Guerrieri_4_Mura,
            Guerrieri_5_Mura,
            Lanceri_1_Mura,
            Lanceri_2_Mura,
            Lanceri_3_Mura,
            Lanceri_4_Mura,
            Lanceri_5_Mura,
            Arceri_1_Mura,
            Arceri_2_Mura,
            Arceri_3_Mura,
            Arceri_4_Mura,
            Arceri_5_Mura,
            Catapulte_1_Mura,
            Catapulte_2_Mura,
            Catapulte_3_Mura,
            Catapulte_4_Mura,
            Catapulte_5_Mura,
            Salute_Torri,
            Salute_TorriMax,
            Difesa_Torri,
            Difesa_TorriMax,
            Riparazione_Torri_Salute,
            Riparazione_Torri_Difesa,
            Guarnigione_Torri,
            Guarnigione_TorriMax,
            Guerrieri_1_Torri,
            Guerrieri_2_Torri,
            Guerrieri_3_Torri,
            Guerrieri_4_Torri,
            Guerrieri_5_Torri,
            Lanceri_1_Torri,
            Lanceri_2_Torri,
            Lanceri_3_Torri,
            Lanceri_4_Torri,
            Lanceri_5_Torri,
            Arceri_1_Torri,
            Arceri_2_Torri,
            Arceri_3_Torri,
            Arceri_4_Torri,
            Arceri_5_Torri,
            Catapulte_1_Torri,
            Catapulte_2_Torri,
            Catapulte_3_Torri,
            Catapulte_4_Torri,
            Catapulte_5_Torri,
            Salute_Castello,
            Salute_CastelloMax,
            Difesa_Castello,
            Difesa_CastelloMax,
            Riparazione_Castello_Salute,
            Riparazione_Castello_Difesa,
            Guarnigione_Castello,
            Guarnigione_CastelloMax,
            Guerrieri_1_Castello,
            Guerrieri_2_Castello,
            Guerrieri_3_Castello,
            Guerrieri_4_Castello,
            Guerrieri_5_Castello,
            Lanceri_1_Castello,
            Lanceri_2_Castello,
            Lanceri_3_Castello,
            Lanceri_4_Castello,
            Lanceri_5_Castello,
            Arceri_1_Castello,
            Arceri_2_Castello,
            Arceri_3_Castello,
            Arceri_4_Castello,
            Arceri_5_Castello,
            Catapulte_1_Castello,
            Catapulte_2_Castello,
            Catapulte_3_Castello,
            Catapulte_4_Castello,
            Catapulte_5_Castello,
            Guarnigione_Citta,
            Guarnigione_CittaMax,
            Guerrieri_1_Citta,
            Guerrieri_2_Citta,
            Guerrieri_3_Citta,
            Guerrieri_4_Citta,
            Guerrieri_5_Citta,
            Lanceri_1_Citta,
            Lanceri_2_Citta,
            Lanceri_3_Citta,
            Lanceri_4_Citta,
            Lanceri_5_Citta,
            Arceri_1_Citta,
            Arceri_2_Citta,
            Arceri_3_Citta,
            Arceri_4_Citta,
            Arceri_5_Citta,
            Catapulte_1_Citta,
            Catapulte_2_Citta,
            Catapulte_3_Citta,
            Catapulte_4_Citta,
            Catapulte_5_Citta
        }

        private static readonly string[] Keys = Enum.GetNames(typeof(Field));

        private string[] _lastSent = new string[Keys.Length];
        private string[] _currentState = new string[Keys.Length]; // ← riusato, non ricreato ogni volta

        // Numero di referti (player.Report.Count) inviati l'ultima volta al client
        // (16/09/2026, su richiesta dell'utente: i referti devono arrivare "in
        // diretta" come il resto dei dati di gioco, senza che ogni punto del
        // codice che ne crea uno — battaglie PVP/PVE, spionaggio, ecc. — debba
        // ricordarsi di inviarlo esplicitamente). Il JSON di player.Report NON
        // passa da _currentState/BuildDelta sotto: può contenere il carattere
        // "|", usato come separatore di campo nel protocollo, e mischiato lì
        // dentro corromperebbe il messaggio. Va invece tenuto a parte e inviato
        // con lo stesso formato "Update_Data|Report_Lista|<json>" già usato da
        // Update_Data_OneTime — vedi ReportCountChanged/SyncReportCount sotto,
        // usati da ServerConnection.Update_Data/Update_Data_OneTime.
        // -1 = mai sincronizzato, forza l'invio al primo tick utile.
        private int _lastReportCount = -1;

        // Ultimo JSON delle quest inviato al client (16/09/2026, su richiesta
        // dell'utente: stesso spirito di _lastReportCount sopra, ma qui il
        // confronto è per contenuto, non per conteggio — le quest non crescono di
        // numero come i referti, cambiano invece i valori dentro a quelle esistenti
        // (progresso, completamenti). Usato da QuestManager.QuestUpdateSeCambiato,
        // chiamato da OnEvent ad ogni variazione di quest invece che ogni 5 secondi
        // a prescindere dal game loop (Server.cs). null = mai sincronizzato.
        private string _lastQuestJson;

        // Costruisci lo stato attuale come dizionario
        public string[] BuildCurrentState(Giocatori.Player player)
        {
            var buildingsQueue = BuildingManagerV2.GetQueuedBuildings(player);
            var unitsQueue = UnitManagerV2.GetQueuedUnits(player);

            double Cibo = 0, Oro = 0;
            double Cibo_Strutture = 0, Legno_Strutture = 0, Ferro_Strutture = 0, Pietra_Strutture = 0, Oro_Strutture = 0;

            Cibo -= player.Guerrieri[0] * Esercito.Unità.Guerriero_1.Cibo + player.Lanceri[0] * Esercito.Unità.Lancere_1.Cibo + player.Arceri[0] * Esercito.Unità.Arcere_1.Cibo + player.Catapulte[0] * Esercito.Unità.Catapulta_1.Cibo;
            Cibo -= player.Guerrieri[1] * Esercito.Unità.Guerriero_2.Cibo + player.Lanceri[1] * Esercito.Unità.Lancere_2.Cibo + player.Arceri[1] * Esercito.Unità.Arcere_2.Cibo + player.Catapulte[1] * Esercito.Unità.Catapulta_2.Cibo;
            Cibo -= player.Guerrieri[2] * Esercito.Unità.Guerriero_3.Cibo + player.Lanceri[2] * Esercito.Unità.Lancere_3.Cibo + player.Arceri[2] * Esercito.Unità.Arcere_3.Cibo + player.Catapulte[2] * Esercito.Unità.Catapulta_3.Cibo;
            Cibo -= player.Guerrieri[3] * Esercito.Unità.Guerriero_4.Cibo + player.Lanceri[3] * Esercito.Unità.Lancere_4.Cibo + player.Arceri[3] * Esercito.Unità.Arcere_4.Cibo + player.Catapulte[3] * Esercito.Unità.Catapulta_4.Cibo;
            Cibo -= player.Guerrieri[4] * Esercito.Unità.Guerriero_5.Cibo + player.Lanceri[4] * Esercito.Unità.Lancere_5.Cibo + player.Arceri[4] * Esercito.Unità.Arcere_5.Cibo + player.Catapulte[4] * Esercito.Unità.Catapulta_5.Cibo;

            Oro -= player.Guerrieri[0] * Esercito.Unità.Guerriero_1.Salario + player.Lanceri[0] * Esercito.Unità.Lancere_1.Salario + player.Arceri[0] * Esercito.Unità.Arcere_1.Salario + player.Catapulte[0] * Esercito.Unità.Catapulta_1.Salario;
            Oro -= player.Guerrieri[1] * Esercito.Unità.Guerriero_2.Salario + player.Lanceri[1] * Esercito.Unità.Lancere_2.Salario + player.Arceri[1] * Esercito.Unità.Arcere_2.Salario + player.Catapulte[1] * Esercito.Unità.Catapulta_2.Salario;
            Oro -= player.Guerrieri[2] * Esercito.Unità.Guerriero_3.Salario + player.Lanceri[2] * Esercito.Unità.Lancere_3.Salario + player.Arceri[2] * Esercito.Unità.Arcere_3.Salario + player.Catapulte[2] * Esercito.Unità.Catapulta_3.Salario;
            Oro -= player.Guerrieri[3] * Esercito.Unità.Guerriero_4.Salario + player.Lanceri[3] * Esercito.Unità.Lancere_4.Salario + player.Arceri[3] * Esercito.Unità.Arcere_4.Salario + player.Catapulte[3] * Esercito.Unità.Catapulta_4.Salario;
            Oro -= player.Guerrieri[4] * Esercito.Unità.Guerriero_5.Salario + player.Lanceri[4] * Esercito.Unità.Lancere_5.Salario + player.Arceri[4] * Esercito.Unità.Arcere_5.Salario + player.Catapulte[4] * Esercito.Unità.Catapulta_5.Salario;

            Cibo_Strutture -= player.Caserma_Guerrieri * Strutture.Edifici.CasermaGuerrieri.Consumo_Cibo;
            Oro_Strutture -= player.Caserma_Guerrieri * Strutture.Edifici.CasermaGuerrieri.Consumo_Oro;
            Cibo_Strutture -= player.Caserma_Lancieri * Strutture.Edifici.CasermaLanceri.Consumo_Cibo;
            Oro_Strutture -= player.Caserma_Lancieri * Strutture.Edifici.CasermaLanceri.Consumo_Oro;
            Cibo_Strutture -= player.Caserma_Arceri * Strutture.Edifici.CasermaArceri.Consumo_Cibo;
            Oro_Strutture -= player.Caserma_Arceri * Strutture.Edifici.CasermaArceri.Consumo_Oro;
            Cibo_Strutture -= player.Caserma_Catapulte * Strutture.Edifici.CasermaCatapulte.Consumo_Cibo;
            Oro_Strutture -= player.Caserma_Catapulte * Strutture.Edifici.CasermaCatapulte.Consumo_Oro;

            Legno_Strutture -= player.Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Consumo_Legno;
            Ferro_Strutture -= player.Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Consumo_Ferro;
            Oro_Strutture -= player.Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Consumo_Oro;
            Legno_Strutture -= player.Workshop_Lance * Strutture.Edifici.ProduzioneLance.Consumo_Legno;
            Ferro_Strutture -= player.Workshop_Lance * Strutture.Edifici.ProduzioneLance.Consumo_Ferro;
            Oro_Strutture -= player.Workshop_Lance * Strutture.Edifici.ProduzioneLance.Consumo_Oro;
            Legno_Strutture -= player.Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Consumo_Legno;
            Oro_Strutture -= player.Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Consumo_Oro;
            Legno_Strutture -= player.Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Consumo_Legno;
            Ferro_Strutture -= player.Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Consumo_Ferro;
            Oro_Strutture -= player.Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Consumo_Oro;
            Ferro_Strutture -= player.Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Consumo_Ferro;
            Oro_Strutture -= player.Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Consumo_Oro;
            Legno_Strutture -= player.Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Consumo_Legno;
            Pietra_Strutture -= player.Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Consumo_Pietra;
            Ferro_Strutture -= player.Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Consumo_Ferro;
            Oro_Strutture -= player.Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Consumo_Oro;

            // ← Aggiorna i valori nel dizionario esistente invece di crearne uno nuovo
            _currentState[(int)Field.livello] = player.Livello.ToString();
            _currentState[(int)Field.esperienza] = player.Esperienza.ToString();
            _currentState[(int)Field.vip] = player.Vip.ToString();
            _currentState[(int)Field.vip_Tempo] = player.FormatTime(player.Vip_Tempo);
            _currentState[(int)Field.Scudo_Tempo] = player.FormatTime(player.ScudoDellaPace);
            _currentState[(int)Field.Costruttori_Tempo] = player.FormatTime(player.Costruttori);
            _currentState[(int)Field.Reclutatori_Tempo] = player.FormatTime(player.Reclutatori);
            _currentState[(int)Field.GamePass_Base] = player.GamePass_Base.ToString();
            _currentState[(int)Field.GamePass_Base_Tempo] = player.FormatTime(player.GamePass_Base_Tempo);
            _currentState[(int)Field.GamePass_Avanzato] = player.GamePass_Avanzato.ToString();
            _currentState[(int)Field.GamePass_Avanzato_Tempo] = player.FormatTime(player.GamePass_Avanzato_Tempo);
            _currentState[(int)Field.QuestMensili_Tempo] = player.FormatTime(Variabili_Server.timer_Reset_Quest);
            _currentState[(int)Field.Barbari_Tempo] = player.FormatTime(Variabili_Server.timer_Reset_Barbari);
            _currentState[(int)Field.Giorni_Consecutivi] = player.GamePass_Accessi_Consecutivi.ToString();

            _currentState[(int)Field.cibo] = player.Cibo.ToString("#,0");
            _currentState[(int)Field.legna] = player.Legno.ToString("#,0");
            _currentState[(int)Field.pietra] = player.Pietra.ToString("#,0");
            _currentState[(int)Field.ferro] = player.Ferro.ToString("#,0");
            _currentState[(int)Field.oro] = player.Oro.ToString("#,0");
            _currentState[(int)Field.popolazione] = player.Popolazione.ToString("#,0.00");
            _currentState[(int)Field.dollari_virtuali] = player.Dollari_Virtuali.ToString("#,0.0000000000");
            _currentState[(int)Field.diamanti_blu] = player.Diamanti_Blu.ToString("#,0");
            _currentState[(int)Field.diamanti_viola] = player.Diamanti_Viola.ToString("#,0");

            _currentState[(int)Field.cibo_max] = Edifici.Fattoria.Limite.ToString("#,0");
            _currentState[(int)Field.legno_max] = Edifici.Segheria.Limite.ToString("#,0");
            _currentState[(int)Field.pietra_max] = Edifici.CavaPietra.Limite.ToString("#,0");
            _currentState[(int)Field.ferro_max] = Edifici.MinieraFerro.Limite.ToString("#,0");
            _currentState[(int)Field.oro_max] = Edifici.MinieraOro.Limite.ToString("#,0");
            _currentState[(int)Field.popolazione_max] = Edifici.Case.Limite.ToString("#,0");

            _currentState[(int)Field.cibo_s] = (player.Fattoria * (Strutture.Edifici.Fattoria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Cibo * (1 + player.Bonus_Produzione_Risorse))).ToString("#,0.00");
            _currentState[(int)Field.legna_s] = (player.Segheria * (Strutture.Edifici.Segheria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Legno * (1 + player.Bonus_Produzione_Risorse))).ToString("#,0.00");
            _currentState[(int)Field.pietra_s] = (player.CavaPietra * (Strutture.Edifici.CavaPietra.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Pietra * (1 + player.Bonus_Produzione_Risorse))).ToString("#,0.00");
            _currentState[(int)Field.ferro_s] = (player.MinieraFerro * (Strutture.Edifici.MinieraFerro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Ferro * (1 + player.Bonus_Produzione_Risorse))).ToString("#,0.00");
            _currentState[(int)Field.oro_s] = (player.MinieraOro * (Strutture.Edifici.MinieraOro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Oro * (1 + player.Bonus_Produzione_Risorse))).ToString("#,0.00");
            _currentState[(int)Field.popolazione_s] = (player.Abitazioni * (Strutture.Edifici.Case.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Popolazione)).ToString("#,0.0000");

            _currentState[(int)Field.spade_s] = (player.Workshop_Spade * (Strutture.Edifici.ProduzioneSpade.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Spade)).ToString("#,0.000");
            _currentState[(int)Field.lance_s] = (player.Workshop_Lance * (Strutture.Edifici.ProduzioneLance.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Lance)).ToString("#,0.000");
            _currentState[(int)Field.archi_s] = (player.Workshop_Archi * (Strutture.Edifici.ProduzioneArchi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Archi)).ToString("#,0.000");
            _currentState[(int)Field.scudi_s] = (player.Workshop_Scudi * (Strutture.Edifici.ProduzioneScudi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Scudi)).ToString("#,0.000");
            _currentState[(int)Field.armature_s] = (player.Workshop_Armature * (Strutture.Edifici.ProduzioneArmature.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Armature)).ToString("#,0.000");
            _currentState[(int)Field.frecce_s] = (player.Workshop_Frecce * (Strutture.Edifici.ProduzioneFrecce.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Popolazione)).ToString("#,0.000");

            _currentState[(int)Field.consumo_cibo_s] = Cibo.ToString("#,0.00");
            _currentState[(int)Field.consumo_oro_s] = Oro.ToString("#,0.00");
            _currentState[(int)Field.consumo_cibo_strutture] = Cibo_Strutture.ToString("#,0.00");
            _currentState[(int)Field.consumo_legno_strutture] = Legno_Strutture.ToString("#,0.00");
            _currentState[(int)Field.consumo_pietra_strutture] = Pietra_Strutture.ToString("#,0.00");
            _currentState[(int)Field.consumo_ferro_strutture] = Ferro_Strutture.ToString("#,0.00");
            _currentState[(int)Field.consumo_oro_strutture] = Oro_Strutture.ToString("#,0.00");

            _currentState[(int)Field.cibo_limite] = (player.Fattoria * Strutture.Edifici.Fattoria.Limite).ToString("#,0");
            _currentState[(int)Field.legna_limite] = (player.Segheria * Strutture.Edifici.Segheria.Limite).ToString("#,0");
            _currentState[(int)Field.pietra_limite] = (player.CavaPietra * Strutture.Edifici.CavaPietra.Limite).ToString("#,0");
            _currentState[(int)Field.ferro_limite] = (player.MinieraFerro * Strutture.Edifici.MinieraFerro.Limite).ToString("#,0");
            _currentState[(int)Field.oro_limite] = (player.MinieraOro * Strutture.Edifici.MinieraOro.Limite).ToString("#,0");
            _currentState[(int)Field.popolazione_limite] = (player.Abitazioni * Strutture.Edifici.Case.Limite).ToString("#,0");

            _currentState[(int)Field.spade_limite] = (player.Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Limite).ToString("#,0");
            _currentState[(int)Field.lance_limite] = (player.Workshop_Lance * Strutture.Edifici.ProduzioneLance.Limite).ToString("#,0");
            _currentState[(int)Field.archi_limite] = (player.Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Limite).ToString("#,0");
            _currentState[(int)Field.scudi_limite] = (player.Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Limite).ToString("#,0");
            _currentState[(int)Field.armature_limite] = (player.Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Limite).ToString("#,0");
            _currentState[(int)Field.frecce_limite] = (player.Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Limite).ToString("#,0");

            _currentState[(int)Field.spade] = player.Spade.ToString("#,0.00");
            _currentState[(int)Field.lance] = player.Lance.ToString("#,0.00");
            _currentState[(int)Field.archi] = player.Archi.ToString("#,0.00");
            _currentState[(int)Field.scudi] = player.Scudi.ToString("#,0.00");
            _currentState[(int)Field.armature] = player.Armature.ToString("#,0.00");
            _currentState[(int)Field.frecce] = player.Frecce.ToString("#,0.00");

            _currentState[(int)Field.spade_max] = Edifici.ProduzioneSpade.Limite.ToString("#,0");
            _currentState[(int)Field.lance_max] = Edifici.ProduzioneLance.Limite.ToString("#,0");
            _currentState[(int)Field.archi_max] = Edifici.ProduzioneArchi.Limite.ToString("#,0");
            _currentState[(int)Field.scudi_max] = Edifici.ProduzioneScudi.Limite.ToString("#,0");
            _currentState[(int)Field.armature_max] = Edifici.ProduzioneArmature.Limite.ToString("#,0");
            _currentState[(int)Field.frecce_max] = Edifici.ProduzioneFrecce.Limite.ToString("#,0");

            _currentState[(int)Field.fattorie] = player.Fattoria.ToString("#,0");
            _currentState[(int)Field.segherie] = player.Segheria.ToString("#,0");
            _currentState[(int)Field.cave_pietra] = player.CavaPietra.ToString("#,0");
            _currentState[(int)Field.miniere_ferro] = player.MinieraFerro.ToString("#,0");
            _currentState[(int)Field.miniere_oro] = player.MinieraOro.ToString("#,0");
            _currentState[(int)Field.@case] = player.Abitazioni.ToString("#,0");

            _currentState[(int)Field.comune] = player.Terreno_Comune.ToString();
            _currentState[(int)Field.noncomune] = player.Terreno_NonComune.ToString();
            _currentState[(int)Field.raro] = player.Terreno_Raro.ToString();
            _currentState[(int)Field.epico] = player.Terreno_Epico.ToString();
            _currentState[(int)Field.leggendario] = player.Terreno_Leggendario.ToString();

            _currentState[(int)Field.workshop_spade] = player.Workshop_Spade.ToString();
            _currentState[(int)Field.workshop_lance] = player.Workshop_Lance.ToString();
            _currentState[(int)Field.workshop_archi] = player.Workshop_Archi.ToString();
            _currentState[(int)Field.workshop_scudi] = player.Workshop_Scudi.ToString();
            _currentState[(int)Field.workshop_armature] = player.Workshop_Armature.ToString();
            _currentState[(int)Field.workshop_frecce] = player.Workshop_Frecce.ToString();

            _currentState[(int)Field.caserma_guerrieri] = player.Caserma_Guerrieri.ToString();
            _currentState[(int)Field.caserma_lanceri] = player.Caserma_Lancieri.ToString();
            _currentState[(int)Field.caserma_arceri] = player.Caserma_Arceri.ToString();
            _currentState[(int)Field.caserma_catapulte] = player.Caserma_Catapulte.ToString();

            _currentState[(int)Field.guerrieri_max] = player.GuerrieriMax.ToString("#,0");
            _currentState[(int)Field.lanceri_max] = player.LancieriMax.ToString("#,0");
            _currentState[(int)Field.arceri_max] = player.ArceriMax.ToString("#,0");
            _currentState[(int)Field.catapulte_max] = player.CatapulteMax.ToString("#,0");

            _currentState[(int)Field.fattoria_coda] = buildingsQueue.GetValueOrDefault("Fattoria", 0).ToString();
            _currentState[(int)Field.segheria_coda] = buildingsQueue.GetValueOrDefault("Segheria", 0).ToString();
            _currentState[(int)Field.cavapietra_coda] = buildingsQueue.GetValueOrDefault("CavaPietra", 0).ToString();
            _currentState[(int)Field.minieraferro_coda] = buildingsQueue.GetValueOrDefault("MinieraFerro", 0).ToString();
            _currentState[(int)Field.minieraoro_coda] = buildingsQueue.GetValueOrDefault("MinieraOro", 0).ToString();
            _currentState[(int)Field.casa_coda] = buildingsQueue.GetValueOrDefault("Abitazioni", 0).ToString();
            _currentState[(int)Field.workshop_spade_coda] = buildingsQueue.GetValueOrDefault("ProduzioneSpade", 0).ToString();
            _currentState[(int)Field.workshop_lance_coda] = buildingsQueue.GetValueOrDefault("ProduzioneLance", 0).ToString();
            _currentState[(int)Field.workshop_archi_coda] = buildingsQueue.GetValueOrDefault("ProduzioneArchi", 0).ToString();
            _currentState[(int)Field.workshop_scudi_coda] = buildingsQueue.GetValueOrDefault("ProduzioneScudi", 0).ToString();
            _currentState[(int)Field.workshop_armature_coda] = buildingsQueue.GetValueOrDefault("ProduzioneArmature", 0).ToString();
            _currentState[(int)Field.workshop_frecce_coda] = buildingsQueue.GetValueOrDefault("ProduzioneFrecce", 0).ToString();
            _currentState[(int)Field.caserma_guerrieri_coda] = buildingsQueue.GetValueOrDefault("CasermaGuerrieri", 0).ToString();
            _currentState[(int)Field.caserma_lanceri_coda] = buildingsQueue.GetValueOrDefault("CasermaLanceri", 0).ToString();
            _currentState[(int)Field.caserma_arceri_coda] = buildingsQueue.GetValueOrDefault("CasermaArceri", 0).ToString();
            _currentState[(int)Field.caserma_catapulte_coda] = buildingsQueue.GetValueOrDefault("CasermaCatapulte", 0).ToString();

            _currentState[(int)Field.guerrieri_1] = player.Guerrieri[0].ToString("#,0");
            _currentState[(int)Field.guerrieri_2] = player.Guerrieri[1].ToString("#,0");
            _currentState[(int)Field.guerrieri_3] = player.Guerrieri[2].ToString("#,0");
            _currentState[(int)Field.guerrieri_4] = player.Guerrieri[3].ToString("#,0");
            _currentState[(int)Field.guerrieri_5] = player.Guerrieri[4].ToString("#,0");
            _currentState[(int)Field.lanceri_1] = player.Lanceri[0].ToString("#,0");
            _currentState[(int)Field.lanceri_2] = player.Lanceri[1].ToString("#,0");
            _currentState[(int)Field.lanceri_3] = player.Lanceri[2].ToString("#,0");
            _currentState[(int)Field.lanceri_4] = player.Lanceri[3].ToString("#,0");
            _currentState[(int)Field.lanceri_5] = player.Lanceri[4].ToString("#,0");
            _currentState[(int)Field.arceri_1] = player.Arceri[0].ToString("#,0");
            _currentState[(int)Field.arceri_2] = player.Arceri[1].ToString("#,0");
            _currentState[(int)Field.arceri_3] = player.Arceri[2].ToString("#,0");
            _currentState[(int)Field.arceri_4] = player.Arceri[3].ToString("#,0");
            _currentState[(int)Field.arceri_5] = player.Arceri[4].ToString("#,0");
            _currentState[(int)Field.catapulte_1] = player.Catapulte[0].ToString("#,0");
            _currentState[(int)Field.catapulte_2] = player.Catapulte[1].ToString("#,0");
            _currentState[(int)Field.catapulte_3] = player.Catapulte[2].ToString("#,0");
            _currentState[(int)Field.catapulte_4] = player.Catapulte[3].ToString("#,0");
            _currentState[(int)Field.catapulte_5] = player.Catapulte[4].ToString("#,0");

            _currentState[(int)Field.guerrieri_1_coda] = unitsQueue.GetValueOrDefault("Guerrieri_1", 0).ToString("#,0");
            _currentState[(int)Field.guerrieri_2_coda] = unitsQueue.GetValueOrDefault("Guerrieri_2", 0).ToString("#,0");
            _currentState[(int)Field.guerrieri_3_coda] = unitsQueue.GetValueOrDefault("Guerrieri_3", 0).ToString("#,0");
            _currentState[(int)Field.guerrieri_4_coda] = unitsQueue.GetValueOrDefault("Guerrieri_4", 0).ToString("#,0");
            _currentState[(int)Field.guerrieri_5_coda] = unitsQueue.GetValueOrDefault("Guerrieri_5", 0).ToString("#,0");
            _currentState[(int)Field.lanceri_1_coda] = unitsQueue.GetValueOrDefault("Lanceri_1", 0).ToString("#,0");
            _currentState[(int)Field.lanceri_2_coda] = unitsQueue.GetValueOrDefault("Lanceri_2", 0).ToString("#,0");
            _currentState[(int)Field.lanceri_3_coda] = unitsQueue.GetValueOrDefault("Lanceri_3", 0).ToString("#,0");
            _currentState[(int)Field.lanceri_4_coda] = unitsQueue.GetValueOrDefault("Lanceri_4", 0).ToString("#,0");
            _currentState[(int)Field.lanceri_5_coda] = unitsQueue.GetValueOrDefault("Lanceri_5", 0).ToString("#,0");
            _currentState[(int)Field.arceri_1_coda] = unitsQueue.GetValueOrDefault("Arceri_1", 0).ToString("#,0");
            _currentState[(int)Field.arceri_2_coda] = unitsQueue.GetValueOrDefault("Arceri_2", 0).ToString("#,0");
            _currentState[(int)Field.arceri_3_coda] = unitsQueue.GetValueOrDefault("Arceri_3", 0).ToString("#,0");
            _currentState[(int)Field.arceri_4_coda] = unitsQueue.GetValueOrDefault("Arceri_4", 0).ToString("#,0");
            _currentState[(int)Field.arceri_5_coda] = unitsQueue.GetValueOrDefault("Arceri_5", 0).ToString("#,0");
            _currentState[(int)Field.catapulte_1_coda] = unitsQueue.GetValueOrDefault("Catapulte_1", 0).ToString("#,0");
            _currentState[(int)Field.catapulte_2_coda] = unitsQueue.GetValueOrDefault("Catapulte_2", 0).ToString("#,0");
            _currentState[(int)Field.catapulte_3_coda] = unitsQueue.GetValueOrDefault("Catapulte_3", 0).ToString("#,0");
            _currentState[(int)Field.catapulte_4_coda] = unitsQueue.GetValueOrDefault("Catapulte_4", 0).ToString("#,0");
            _currentState[(int)Field.catapulte_5_coda] = unitsQueue.GetValueOrDefault("Catapulte_5", 0).ToString("#,0");

            _currentState[(int)Field.forza_esercito] = player.forza_Esercito.ToString("#,0.00");

            _currentState[(int)Field.ricerca_produzione] = player.Ricerca_Produzione.ToString();
            _currentState[(int)Field.ricerca_costruzione] = player.Ricerca_Costruzione.ToString();
            _currentState[(int)Field.ricerca_addestramento] = player.Ricerca_Addestramento.ToString();
            _currentState[(int)Field.ricerca_popolazione] = player.Ricerca_Popolazione.ToString();
            _currentState[(int)Field.ricerca_riparazione] = player.Ricerca_Riparazione.ToString();
            _currentState[(int)Field.ricerca_trasporto] = player.Ricerca_Trasporto.ToString();
            _currentState[(int)Field.ricerca_Spionaggio] = player.Ricerca_Spionaggio.ToString();
            _currentState[(int)Field.ricerca_Contro_Spionaggio] = player.Ricerca_Contro_Spionaggio.ToString();

            _currentState[(int)Field.ricerca_ingresso_livello] = player.Ricerca_Ingresso_Livello.ToString();
            _currentState[(int)Field.ricerca_ingresso_guarnigione] = player.Ricerca_Ingresso_Guarnigione.ToString();
            _currentState[(int)Field.ricerca_citta_livello] = player.Ricerca_Citta_Livello.ToString();
            _currentState[(int)Field.ricerca_citta_guarnigione] = player.Ricerca_Citta_Guarnigione.ToString();

            _currentState[(int)Field.ricerca_cancello_livello] = player.Ricerca_Cancello_Livello.ToString();
            _currentState[(int)Field.ricerca_cancello_salute] = player.Ricerca_Cancello_Salute.ToString();
            _currentState[(int)Field.ricerca_cancello_difesa] = player.Ricerca_Cancello_Difesa.ToString();
            _currentState[(int)Field.ricerca_cancello_guarnigione] = player.Ricerca_Cancello_Guarnigione.ToString();
            _currentState[(int)Field.ricerca_mura_livello] = player.Ricerca_Mura_Livello.ToString();
            _currentState[(int)Field.ricerca_mura_salute] = player.Ricerca_Mura_Salute.ToString();
            _currentState[(int)Field.ricerca_mura_difesa] = player.Ricerca_Mura_Difesa.ToString();
            _currentState[(int)Field.ricerca_mura_guarnigione] = player.Ricerca_Mura_Guarnigione.ToString();
            _currentState[(int)Field.ricerca_torri_livello] = player.Ricerca_Torri_Livello.ToString();
            _currentState[(int)Field.ricerca_torri_salute] = player.Ricerca_Torri_Salute.ToString();
            _currentState[(int)Field.ricerca_torri_difesa] = player.Ricerca_Torri_Difesa.ToString();
            _currentState[(int)Field.ricerca_torri_guarnigione] = player.Ricerca_Torri_Guarnigione.ToString();
            _currentState[(int)Field.ricerca_castello_livello] = player.Ricerca_Castello_Livello.ToString();
            _currentState[(int)Field.ricerca_castello_salute] = player.Ricerca_Castello_Salute.ToString();
            _currentState[(int)Field.ricerca_castello_difesa] = player.Ricerca_Castello_Difesa.ToString();
            _currentState[(int)Field.ricerca_castello_guarnigione] = player.Ricerca_Castello_Guarnigione.ToString();
            _currentState[(int)Field.Ricerca_Attiva] = player.Ricerca_Attiva.ToString();

            _currentState[(int)Field.guerriero_salute] = player.Guerriero_Salute.ToString();
            _currentState[(int)Field.guerriero_difesa] = player.Guerriero_Difesa.ToString();
            _currentState[(int)Field.guerriero_attacco] = player.Guerriero_Attacco.ToString();
            _currentState[(int)Field.guerriero_livello] = player.Guerriero_Livello.ToString();
            _currentState[(int)Field.lancere_salute] = player.Lancere_Salute.ToString();
            _currentState[(int)Field.lancere_difesa] = player.Lancere_Difesa.ToString();
            _currentState[(int)Field.lancere_attacco] = player.Lancere_Attacco.ToString();
            _currentState[(int)Field.lancere_livello] = player.Lancere_Livello.ToString();
            _currentState[(int)Field.arcere_salute] = player.Arcere_Salute.ToString();
            _currentState[(int)Field.arcere_difesa] = player.Arcere_Difesa.ToString();
            _currentState[(int)Field.arcere_attacco] = player.Arcere_Attacco.ToString();
            _currentState[(int)Field.arcere_livello] = player.Arcere_Livello.ToString();
            _currentState[(int)Field.catapulta_salute] = player.Catapulta_Salute.ToString();
            _currentState[(int)Field.catapulta_difesa] = player.Catapulta_Difesa.ToString();
            _currentState[(int)Field.catapulta_attacco] = player.Catapulta_Attacco.ToString();
            _currentState[(int)Field.catapulta_livello] = player.Catapulta_Livello.ToString();

            _currentState[(int)Field.Code_Costruzioni] = player.Code_Costruzione.ToString();
            _currentState[(int)Field.Code_Reclutamenti] = player.Code_Reclutamento.ToString();
            _currentState[(int)Field.Code_Costruzioni_Disponibili] = player.task_Attuale_Costruzioni.Count.ToString();
            _currentState[(int)Field.Code_Reclutamenti_Disponibili] = player.task_Attuale_Recutamento.Count.ToString();
            _currentState[(int)Field.Tempo_Costruzione] = BuildingManagerV2.Get_Total_Building_Time(player).ToString();
            _currentState[(int)Field.Tempo_Reclutamento] = UnitManagerV2.Get_Total_Recruit_Time(player).ToString();
            _currentState[(int)Field.Tempo_Ricerca_Citta] = ResearchManager.GetTotalResearchTime(player).ToString();
            _currentState[(int)Field.Tempo_Ricerca_Globale] = 1.ToString("#,0");

            _currentState[(int)Field.Potenza_Totale] = player.Potenza_Totale.ToString("#,0");
            _currentState[(int)Field.Potenza_Strutture] = player.Potenza_Strutture.ToString("#,0");
            _currentState[(int)Field.Potenza_Ricerca] = player.Potenza_Ricerca.ToString("#,0");
            _currentState[(int)Field.Potenza_Esercito] = player.Potenza_Esercito.ToString("#,0");

            _currentState[(int)Field.Unità_Eliminate] = player.Unità_Eliminate.ToString("#,0");
            _currentState[(int)Field.Guerrieri_Eliminate] = player.Guerrieri_Eliminati.ToString("#,0");
            _currentState[(int)Field.Lanceri_Eliminate] = player.Lanceri_Eliminati.ToString("#,0");
            _currentState[(int)Field.Arceri_Eliminate] = player.Arceri_Eliminati.ToString("#,0");
            _currentState[(int)Field.Catapulte_Eliminate] = player.Catapulte_Eliminate.ToString("#,0");
            _currentState[(int)Field.Unità_Perse] = player.Unità_Perse.ToString("#,0");
            _currentState[(int)Field.Guerrieri_Persi] = player.Guerrieri_Persi.ToString("#,0");
            _currentState[(int)Field.Lanceri_Persi] = player.Lanceri_Persi.ToString("#,0");
            _currentState[(int)Field.Arceri_Persi] = player.Arceri_Persi.ToString("#,0");
            _currentState[(int)Field.Catapulte_Persi] = player.Catapulte_Perse.ToString("#,0");
            _currentState[(int)Field.Risorse_Razziate] = player.Risorse_Razziate.ToString("#,0");
            _currentState[(int)Field.Strutture_Civili_Costruite] = player.Strutture_Civili_Costruite.ToString("#,0");
            _currentState[(int)Field.Strutture_Militari_Costruite] = player.Strutture_Militari_Costruite.ToString("#,0");
            _currentState[(int)Field.Caserme_Costruite] = player.Caserme_Costruite.ToString("#,0");
            _currentState[(int)Field.Frecce_Utilizzate] = player.Frecce_Utilizzate.ToString("#,0");
            _currentState[(int)Field.Battaglie_Vinte] = player.Battaglie_Vinte.ToString("#,0");
            _currentState[(int)Field.Battaglie_Perse] = player.Battaglie_Perse.ToString("#,0");
            _currentState[(int)Field.Quest_Completate] = player.Quest_Completate.ToString("#,0");
            _currentState[(int)Field.Attacchi_Subiti_PVP] = player.Attacchi_Subiti_PVP.ToString("#,0");
            _currentState[(int)Field.Attacchi_Effettuati_PVP] = player.Attacchi_Effettuati_PVP.ToString("#,0");
            _currentState[(int)Field.Barbari_Sconfitti] = player.Barbari_Sconfitti.ToString("#,0");
            _currentState[(int)Field.Accampamenti_Barbari_Sconfitti] = player.Accampamenti_Barbari_Sconfitti.ToString("#,0");
            _currentState[(int)Field.Città_Barbare_Sconfitte] = player.Città_Barbare_Sconfitte.ToString("#,0");
            _currentState[(int)Field.Danno_HP_Barbaro] = player.Danno_HP_Barbaro.ToString("#,0");
            _currentState[(int)Field.Danno_DEF_Barbaro] = player.Danno_DEF_Barbaro.ToString("#,0");
            _currentState[(int)Field.Unità_Addestrate] = player.Unità_Addestrate.ToString("#,0");
            _currentState[(int)Field.Risorse_Utilizzate] = player.Risorse_Utilizzate.ToString("#,0");
            _currentState[(int)Field.Tempo_Addestramento_Risparmiato] = player.FormatTime(player.Tempo_Addestramento).ToString();
            _currentState[(int)Field.Tempo_Costruzione_Risparmiato] = player.FormatTime(player.Tempo_Costruzione).ToString();
            _currentState[(int)Field.Tempo_Ricerca_Risparmiato] = player.FormatTime(player.Tempo_Ricerca).ToString();
            _currentState[(int)Field.Tempo_Sottratto_Diamanti] = player.FormatTime(player.Tempo_Sottratto_Diamanti).ToString();

            _currentState[(int)Field.Bonus_Costruzione] = (player.Bonus_Costruzione * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Addestramento] = (player.Bonus_Addestramento * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Ricerca] = (player.Bonus_Ricerca * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Riparazione] = (player.Bonus_Riparazione * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Produzione_Risorse] = (player.Bonus_Produzione_Risorse * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Capacità_Trasporto] = (player.Bonus_Capacità_Trasporto * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Salute_Strutture] = (player.Bonus_Salute_Strutture * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Difesa_Strutture] = (player.Bonus_Difesa_Strutture * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Guarnigione_Strutture] = (player.Bonus_Guarnigione_Strutture * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Attacco_Guerrieri] = (player.Bonus_Attacco_Guerrieri * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Salute_Guerrieri] = (player.Bonus_Salute_Guerrieri * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Difesa_Guerrieri] = (player.Bonus_Difesa_Guerrieri * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Attacco_Lanceri] = (player.Bonus_Attacco_Lanceri * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Salute_Lanceri] = (player.Bonus_Salute_Lanceri * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Difesa_Lanceri] = (player.Bonus_Difesa_Lanceri * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Attacco_Arceri] = (player.Bonus_Attacco_Arceri * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Salute_Arceri] = (player.Bonus_Salute_Arceri * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Difesa_Arceri] = (player.Bonus_Difesa_Arceri * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Attacco_Catapulte] = (player.Bonus_Attacco_Catapulte * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Salute_Catapulte] = (player.Bonus_Salute_Catapulte * 100).ToString() + "%";
            _currentState[(int)Field.Bonus_Difesa_Catapulte] = (player.Bonus_Difesa_Catapulte * 100).ToString() + "%";

            _currentState[(int)Field.punti_quest] = player.Punti_Quest.ToString();

            _currentState[(int)Field.Guarnigione_Ingresso] = player.Guarnigione_Ingresso.ToString();
            _currentState[(int)Field.Guarnigione_IngressoMax] = player.Guarnigione_IngressoMax.ToString();
            for (int i = 0; i < 5; i++)
            {
                _currentState[(int)Field.Guerrieri_1_Ingresso + i] = player.Guerrieri_Ingresso[i].ToString();
                _currentState[(int)Field.Lanceri_1_Ingresso + i] = player.Lanceri_Ingresso[i].ToString();
                _currentState[(int)Field.Arceri_1_Ingresso + i] = player.Arceri_Ingresso[i].ToString();
                _currentState[(int)Field.Catapulte_1_Ingresso + i] = player.Catapulte_Ingresso[i].ToString();
            }

            _currentState[(int)Field.Salute_Cancello] = player.Salute_Cancello.ToString();
            _currentState[(int)Field.Salute_CancelloMax] = player.Salute_CancelloMax.ToString();
            _currentState[(int)Field.Difesa_Cancello] = player.Difesa_Cancello.ToString();
            _currentState[(int)Field.Difesa_CancelloMax] = player.Difesa_CancelloMax.ToString();
            // 15/09/2026, su richiesta dell'utente: il client Web vuole distinguere
            // "danneggiata" da "in riparazione" (pallino della mappa Città che
            // lampeggia di colore diverso) — Riparazioni[] esisteva già lato server
            // (Set_Riparazioni/Server.Ripara) ma non veniva mai esposto al client.
            // Indici 0/1 = Cancello Salute/Difesa (vedi Server.cs, il loop Ripara()).
            _currentState[(int)Field.Riparazione_Cancello_Salute] = player.Riparazioni[0].ToString();
            _currentState[(int)Field.Riparazione_Cancello_Difesa] = player.Riparazioni[1].ToString();
            _currentState[(int)Field.Guarnigione_Cancello] = player.Guarnigione_Cancello.ToString();
            _currentState[(int)Field.Guarnigione_CancelloMax] = player.Guarnigione_CancelloMax.ToString();
            for (int i = 0; i < 5; i++)
            {
                _currentState[(int)Field.Guerrieri_1_Cancello + i] = player.Guerrieri_Cancello[i].ToString();
                _currentState[(int)Field.Lanceri_1_Cancello + i] = player.Lanceri_Cancello[i].ToString();
                _currentState[(int)Field.Arceri_1_Cancello + i] = player.Arceri_Cancello[i].ToString();
                _currentState[(int)Field.Catapulte_1_Cancello + i] = player.Catapulte_Cancello[i].ToString();
            }

            _currentState[(int)Field.Salute_Mura] = player.Salute_Mura.ToString();
            _currentState[(int)Field.Salute_MuraMax] = player.Salute_MuraMax.ToString();
            _currentState[(int)Field.Difesa_Mura] = player.Difesa_Mura.ToString();
            _currentState[(int)Field.Difesa_MuraMax] = player.Difesa_MuraMax.ToString();
            // Indici 2/3 = Mura Salute/Difesa (vedi commento su Riparazione_Cancello_*).
            _currentState[(int)Field.Riparazione_Mura_Salute] = player.Riparazioni[2].ToString();
            _currentState[(int)Field.Riparazione_Mura_Difesa] = player.Riparazioni[3].ToString();
            _currentState[(int)Field.Guarnigione_Mura] = player.Guarnigione_Mura.ToString();
            _currentState[(int)Field.Guarnigione_MuraMax] = player.Guarnigione_MuraMax.ToString();
            for (int i = 0; i < 5; i++)
            {
                _currentState[(int)Field.Guerrieri_1_Mura + i] = player.Guerrieri_Mura[i].ToString();
                _currentState[(int)Field.Lanceri_1_Mura + i] = player.Lanceri_Mura[i].ToString();
                _currentState[(int)Field.Arceri_1_Mura + i] = player.Arceri_Mura[i].ToString();
                _currentState[(int)Field.Catapulte_1_Mura + i] = player.Catapulte_Mura[i].ToString();
            }

            _currentState[(int)Field.Salute_Torri] = player.Salute_Torri.ToString();
            _currentState[(int)Field.Salute_TorriMax] = player.Salute_TorriMax.ToString();
            _currentState[(int)Field.Difesa_Torri] = player.Difesa_Torri.ToString();
            _currentState[(int)Field.Difesa_TorriMax] = player.Difesa_TorriMax.ToString();
            // Indici 4/5 = Torri Salute/Difesa (vedi commento su Riparazione_Cancello_*).
            _currentState[(int)Field.Riparazione_Torri_Salute] = player.Riparazioni[4].ToString();
            _currentState[(int)Field.Riparazione_Torri_Difesa] = player.Riparazioni[5].ToString();
            _currentState[(int)Field.Guarnigione_Torri] = player.Guarnigione_Torri.ToString();
            _currentState[(int)Field.Guarnigione_TorriMax] = player.Guarnigione_TorriMax.ToString();
            for (int i = 0; i < 5; i++)
            {
                _currentState[(int)Field.Guerrieri_1_Torri + i] = player.Guerrieri_Torri[i].ToString();
                _currentState[(int)Field.Lanceri_1_Torri + i] = player.Lanceri_Torri[i].ToString();
                _currentState[(int)Field.Arceri_1_Torri + i] = player.Arceri_Torri[i].ToString();
                _currentState[(int)Field.Catapulte_1_Torri + i] = player.Catapulte_Torri[i].ToString();
            }

            _currentState[(int)Field.Salute_Castello] = player.Salute_Castello.ToString();
            _currentState[(int)Field.Salute_CastelloMax] = player.Salute_CastelloMax.ToString();
            _currentState[(int)Field.Difesa_Castello] = player.Difesa_Castello.ToString();
            _currentState[(int)Field.Difesa_CastelloMax] = player.Difesa_CastelloMax.ToString();
            // Indici 6/7 = Castello Salute/Difesa (vedi commento su Riparazione_Cancello_*).
            _currentState[(int)Field.Riparazione_Castello_Salute] = player.Riparazioni[6].ToString();
            _currentState[(int)Field.Riparazione_Castello_Difesa] = player.Riparazioni[7].ToString();
            _currentState[(int)Field.Guarnigione_Castello] = player.Guarnigione_Castello.ToString();
            _currentState[(int)Field.Guarnigione_CastelloMax] = player.Guarnigione_CastelloMax.ToString();
            for (int i = 0; i < 5; i++)
            {
                _currentState[(int)Field.Guerrieri_1_Castello + i] = player.Guerrieri_Castello[i].ToString();
                _currentState[(int)Field.Lanceri_1_Castello + i] = player.Lanceri_Castello[i].ToString();
                _currentState[(int)Field.Arceri_1_Castello + i] = player.Arceri_Castello[i].ToString();
                _currentState[(int)Field.Catapulte_1_Castello + i] = player.Catapulte_Castello[i].ToString();
            }

            _currentState[(int)Field.Guarnigione_Citta] = player.Guarnigione_Citta.ToString();
            _currentState[(int)Field.Guarnigione_CittaMax] = player.Guarnigione_CittaMax.ToString();
            for (int i = 0; i < 5; i++)
            {
                _currentState[(int)Field.Guerrieri_1_Citta + i] = player.Guerrieri_Citta[i].ToString();
                _currentState[(int)Field.Lanceri_1_Citta + i] = player.Lanceri_Citta[i].ToString();
                _currentState[(int)Field.Arceri_1_Citta + i] = player.Arceri_Citta[i].ToString();
                _currentState[(int)Field.Catapulte_1_Citta + i] = player.Catapulte_Citta[i].ToString();
            }

            return _currentState;
        }

        // Calcola e restituisce solo i campi cambiati
        public string BuildDelta(string[] current)
        {
            var delta = new StringBuilder("Update_Data|");
            bool hasChanges = false;

            for (int i = 0; i < current.Length; i++)
            {
                if (_lastSent[i] != current[i])
                {
                    delta.Append($"{Keys[i]}={current[i]}|");
                    _lastSent[i] = current[i]; // ← aggiorna solo il campo cambiato, niente new array
                    hasChanges = true;
                }
            }

            if (!hasChanges) return null;
            return delta.ToString();
        }

        // Forza l'invio completo (es. al login)
        public void Reset() => Array.Clear(_lastSent, 0, _lastSent.Length);

        // True se il numero di referti è cambiato rispetto all'ultima volta che è
        // stato inviato: aggiorna subito il valore interno, quindi va chiamato
        // solo da chi si impegna a inviare davvero il nuovo Report_Lista se torna
        // true (vedi ServerConnection.Update_Data).
        public bool ReportCountChanged(int currentCount)
        {
            if (_lastReportCount == currentCount) return false;
            _lastReportCount = currentCount;
            return true;
        }

        // Chiamato da Update_Data_OneTime dopo aver mandato il Report_Lista
        // iniziale al login: sincronizza il contatore così il primo tick
        // periodico successivo non rimanda subito una seconda volta lo stesso
        // identico contenuto appena inviato.
        public void SyncReportCount(int currentCount) => _lastReportCount = currentCount;

        // True se il JSON delle quest è cambiato rispetto all'ultima volta che è
        // stato inviato: aggiorna subito il valore interno, quindi va chiamato solo
        // da chi si impegna a inviare davvero il nuovo pacchetto se torna true (vedi
        // QuestManager.QuestUpdateSeCambiato).
        public bool QuestJsonChanged(string currentJson)
        {
            if (_lastQuestJson == currentJson) return false;
            _lastQuestJson = currentJson;
            return true;
        }

        // Chiamato da QuestManager.QuestUpdate (invio "a forza": login, riscatto
        // premio) per allineare il tracking, così QuestUpdateSeCambiato non rimanda
        // subito una seconda volta lo stesso identico contenuto appena inviato.
        public void SyncQuestJson(string currentJson) => _lastQuestJson = currentJson;
    }
}