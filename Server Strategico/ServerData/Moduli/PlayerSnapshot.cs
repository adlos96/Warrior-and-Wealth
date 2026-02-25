using Server_Strategico.Gioco;
using Server_Strategico.Manager;
using System.Text;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Gioco.Strutture;

namespace Server_Strategico.ServerData.Moduli
{
    public class PlayerSnapshot
    {
        private Dictionary<string, string> _lastSent = new(250);
        private Dictionary<string, string> _currentState = new(250); // ← riusato, non ricreato ogni volta

        // Costruisci lo stato attuale come dizionario
        public Dictionary<string, string> BuildCurrentState(Player player)
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
            _currentState["livello"] = player.Livello.ToString();
            _currentState["esperienza"] = player.Esperienza.ToString();
            _currentState["vip"] = player.Vip.ToString();
            _currentState["vip_Tempo"] = player.FormatTime(player.Vip_Tempo);
            _currentState["Scudo_Tempo"] = player.FormatTime(player.ScudoDellaPace);
            _currentState["Costruttori_Tempo"] = player.FormatTime(player.Costruttori);
            _currentState["Reclutatori_Tempo"] = player.FormatTime(player.Reclutatori);
            _currentState["GamePass_Base"] = player.GamePass_Base.ToString();
            _currentState["GamePass_Base_Tempo"] = player.FormatTime(player.GamePass_Base_Tempo);
            _currentState["GamePass_Avanzato"] = player.GamePass_Avanzato.ToString();
            _currentState["GamePass_Avanzato_Tempo"] = player.FormatTime(player.GamePass_Avanzato_Tempo);

            _currentState["cibo"] = player.Cibo.ToString("#,0");
            _currentState["legna"] = player.Legno.ToString("#,0");
            _currentState["pietra"] = player.Pietra.ToString("#,0");
            _currentState["ferro"] = player.Ferro.ToString("#,0");
            _currentState["oro"] = player.Oro.ToString("#,0");
            _currentState["popolazione"] = player.Popolazione.ToString("#,0.00");
            _currentState["dollari_virtuali"] = player.Dollari_Virtuali.ToString("#,0.0000000000");
            _currentState["diamanti_blu"] = player.Diamanti_Blu.ToString("#,0");
            _currentState["diamanti_viola"] = player.Diamanti_Viola.ToString("#,0");

            _currentState["cibo_max"] = Edifici.Fattoria.Limite.ToString("#,0");
            _currentState["legno_max"] = Edifici.Segheria.Limite.ToString("#,0");
            _currentState["pietra_max"] = Edifici.CavaPietra.Limite.ToString("#,0");
            _currentState["ferro_max"] = Edifici.MinieraFerro.Limite.ToString("#,0");
            _currentState["oro_max"] = Edifici.MinieraOro.Limite.ToString("#,0");
            _currentState["popolazione_max"] = Edifici.Case.Limite.ToString("#,0");

            _currentState["cibo_s"] = (player.Fattoria * (Strutture.Edifici.Fattoria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Cibo * (1 + player.Bonus_Produzione_Risorse))).ToString("#,0.00");
            _currentState["legna_s"] = (player.Segheria * (Strutture.Edifici.Segheria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Legno * (1 + player.Bonus_Produzione_Risorse))).ToString("#,0.00");
            _currentState["pietra_s"] = (player.CavaPietra * (Strutture.Edifici.CavaPietra.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Pietra * (1 + player.Bonus_Produzione_Risorse))).ToString("#,0.00");
            _currentState["ferro_s"] = (player.MinieraFerro * (Strutture.Edifici.MinieraFerro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Ferro * (1 + player.Bonus_Produzione_Risorse))).ToString("#,0.00");
            _currentState["oro_s"] = (player.MinieraOro * (Strutture.Edifici.MinieraOro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Oro * (1 + player.Bonus_Produzione_Risorse))).ToString("#,0.00");
            _currentState["popolazione_s"] = (player.Abitazioni * (Strutture.Edifici.Case.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Popolazione)).ToString("#,0.0000");

            _currentState["spade_s"] = (player.Workshop_Spade * (Strutture.Edifici.ProduzioneSpade.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Spade)).ToString("#,0.000");
            _currentState["lance_s"] = (player.Workshop_Lance * (Strutture.Edifici.ProduzioneLance.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Lance)).ToString("#,0.000");
            _currentState["archi_s"] = (player.Workshop_Archi * (Strutture.Edifici.ProduzioneArchi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Archi)).ToString("#,0.000");
            _currentState["scudi_s"] = (player.Workshop_Scudi * (Strutture.Edifici.ProduzioneScudi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Scudi)).ToString("#,0.000");
            _currentState["armature_s"] = (player.Workshop_Armature * (Strutture.Edifici.ProduzioneArmature.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Armature)).ToString("#,0.000");
            _currentState["frecce_s"] = (player.Workshop_Frecce * (Strutture.Edifici.ProduzioneFrecce.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Popolazione)).ToString("#,0.000");

            _currentState["consumo_cibo_s"] = Cibo.ToString("#,0.00");
            _currentState["consumo_oro_s"] = Oro.ToString("#,0.00");
            _currentState["consumo_cibo_strutture"] = Cibo_Strutture.ToString("#,0.00");
            _currentState["consumo_legno_strutture"] = Legno_Strutture.ToString("#,0.00");
            _currentState["consumo_pietra_strutture"] = Pietra_Strutture.ToString("#,0.00");
            _currentState["consumo_ferro_strutture"] = Ferro_Strutture.ToString("#,0.00");
            _currentState["consumo_oro_strutture"] = Oro_Strutture.ToString("#,0.00");

            _currentState["cibo_limite"] = (player.Fattoria * Strutture.Edifici.Fattoria.Limite).ToString("#,0");
            _currentState["legna_limite"] = (player.Segheria * Strutture.Edifici.Segheria.Limite).ToString("#,0");
            _currentState["pietra_limite"] = (player.CavaPietra * Strutture.Edifici.CavaPietra.Limite).ToString("#,0");
            _currentState["ferro_limite"] = (player.MinieraFerro * Strutture.Edifici.MinieraFerro.Limite).ToString("#,0");
            _currentState["oro_limite"] = (player.MinieraOro * Strutture.Edifici.MinieraOro.Limite).ToString("#,0");
            _currentState["popolazione_limite"] = (player.Abitazioni * Strutture.Edifici.Case.Limite).ToString("#,0");

            _currentState["spade_limite"] = (player.Workshop_Spade * Strutture.Edifici.ProduzioneSpade.Limite).ToString("#,0");
            _currentState["lance_limite"] = (player.Workshop_Lance * Strutture.Edifici.ProduzioneLance.Limite).ToString("#,0");
            _currentState["archi_limite"] = (player.Workshop_Archi * Strutture.Edifici.ProduzioneArchi.Limite).ToString("#,0");
            _currentState["scudi_limite"] = (player.Workshop_Scudi * Strutture.Edifici.ProduzioneScudi.Limite).ToString("#,0");
            _currentState["armature_limite"] = (player.Workshop_Armature * Strutture.Edifici.ProduzioneArmature.Limite).ToString("#,0");
            _currentState["frecce_limite"] = (player.Workshop_Frecce * Strutture.Edifici.ProduzioneFrecce.Limite).ToString("#,0");

            _currentState["spade"] = player.Spade.ToString("#,0.00");
            _currentState["lance"] = player.Lance.ToString("#,0.00");
            _currentState["archi"] = player.Archi.ToString("#,0.00");
            _currentState["scudi"] = player.Scudi.ToString("#,0.00");
            _currentState["armature"] = player.Armature.ToString("#,0.00");
            _currentState["frecce"] = player.Frecce.ToString("#,0.00");

            _currentState["spade_max"] = Edifici.ProduzioneSpade.Limite.ToString("#,0");
            _currentState["lance_max"] = Edifici.ProduzioneLance.Limite.ToString("#,0");
            _currentState["archi_max"] = Edifici.ProduzioneArchi.Limite.ToString("#,0");
            _currentState["scudi_max"] = Edifici.ProduzioneScudi.Limite.ToString("#,0");
            _currentState["armature_max"] = Edifici.ProduzioneArmature.Limite.ToString("#,0");
            _currentState["frecce_max"] = Edifici.ProduzioneFrecce.Limite.ToString("#,0");

            _currentState["fattorie"] = player.Fattoria.ToString("#,0");
            _currentState["segherie"] = player.Segheria.ToString("#,0");
            _currentState["cave_pietra"] = player.CavaPietra.ToString("#,0");
            _currentState["miniere_ferro"] = player.MinieraFerro.ToString("#,0");
            _currentState["miniere_oro"] = player.MinieraOro.ToString("#,0");
            _currentState["case"] = player.Abitazioni.ToString("#,0");

            _currentState["comune"] = player.Terreno_Comune.ToString();
            _currentState["noncomune"] = player.Terreno_NonComune.ToString();
            _currentState["raro"] = player.Terreno_Raro.ToString();
            _currentState["epico"] = player.Terreno_Epico.ToString();
            _currentState["leggendario"] = player.Terreno_Leggendario.ToString();

            _currentState["workshop_spade"] = player.Workshop_Spade.ToString();
            _currentState["workshop_lance"] = player.Workshop_Lance.ToString();
            _currentState["workshop_archi"] = player.Workshop_Archi.ToString();
            _currentState["workshop_scudi"] = player.Workshop_Scudi.ToString();
            _currentState["workshop_armature"] = player.Workshop_Armature.ToString();
            _currentState["workshop_frecce"] = player.Workshop_Frecce.ToString();

            _currentState["caserma_guerrieri"] = player.Caserma_Guerrieri.ToString();
            _currentState["caserma_lanceri"] = player.Caserma_Lancieri.ToString();
            _currentState["caserma_arceri"] = player.Caserma_Arceri.ToString();
            _currentState["caserma_catapulte"] = player.Caserma_Catapulte.ToString();

            _currentState["guerrieri_max"] = player.GuerrieriMax.ToString("#,0");
            _currentState["lanceri_max"] = player.LancieriMax.ToString("#,0");
            _currentState["arceri_max"] = player.ArceriMax.ToString("#,0");
            _currentState["catapulte_max"] = player.CatapulteMax.ToString("#,0");

            _currentState["fattoria_coda"] = buildingsQueue.GetValueOrDefault("Fattoria", 0).ToString();
            _currentState["segheria_coda"] = buildingsQueue.GetValueOrDefault("Segheria", 0).ToString();
            _currentState["cavapietra_coda"] = buildingsQueue.GetValueOrDefault("CavaPietra", 0).ToString();
            _currentState["minieraferro_coda"] = buildingsQueue.GetValueOrDefault("MinieraFerro", 0).ToString();
            _currentState["minieraoro_coda"] = buildingsQueue.GetValueOrDefault("MinieraOro", 0).ToString();
            _currentState["casa_coda"] = buildingsQueue.GetValueOrDefault("Case", 0).ToString();
            _currentState["workshop_spade_coda"] = buildingsQueue.GetValueOrDefault("ProduzioneSpade", 0).ToString();
            _currentState["workshop_lance_coda"] = buildingsQueue.GetValueOrDefault("ProduzioneLance", 0).ToString();
            _currentState["workshop_archi_coda"] = buildingsQueue.GetValueOrDefault("ProduzioneArchi", 0).ToString();
            _currentState["workshop_scudi_coda"] = buildingsQueue.GetValueOrDefault("ProduzioneScudi", 0).ToString();
            _currentState["workshop_armature_coda"] = buildingsQueue.GetValueOrDefault("ProduzioneArmature", 0).ToString();
            _currentState["workshop_frecce_coda"] = buildingsQueue.GetValueOrDefault("ProduzioneFrecce", 0).ToString();
            _currentState["caserma_guerrieri_coda"] = buildingsQueue.GetValueOrDefault("CasermaGuerrieri", 0).ToString();
            _currentState["caserma_lanceri_coda"] = buildingsQueue.GetValueOrDefault("CasermaLanceri", 0).ToString();
            _currentState["caserma_arceri_coda"] = buildingsQueue.GetValueOrDefault("CasermaArceri", 0).ToString();
            _currentState["caserma_catapulte_coda"] = buildingsQueue.GetValueOrDefault("CasermaCatapulte", 0).ToString();

            _currentState["guerrieri_1"] = player.Guerrieri[0].ToString("#,0");
            _currentState["guerrieri_2"] = player.Guerrieri[1].ToString("#,0");
            _currentState["guerrieri_3"] = player.Guerrieri[2].ToString("#,0");
            _currentState["guerrieri_4"] = player.Guerrieri[3].ToString("#,0");
            _currentState["guerrieri_5"] = player.Guerrieri[4].ToString("#,0");
            _currentState["lanceri_1"] = player.Lanceri[0].ToString("#,0");
            _currentState["lanceri_2"] = player.Lanceri[1].ToString("#,0");
            _currentState["lanceri_3"] = player.Lanceri[2].ToString("#,0");
            _currentState["lanceri_4"] = player.Lanceri[3].ToString("#,0");
            _currentState["lanceri_5"] = player.Lanceri[4].ToString("#,0");
            _currentState["arceri_1"] = player.Arceri[0].ToString("#,0");
            _currentState["arceri_2"] = player.Arceri[1].ToString("#,0");
            _currentState["arceri_3"] = player.Arceri[2].ToString("#,0");
            _currentState["arceri_4"] = player.Arceri[3].ToString("#,0");
            _currentState["arceri_5"] = player.Arceri[4].ToString("#,0");
            _currentState["catapulte_1"] = player.Catapulte[0].ToString("#,0");
            _currentState["catapulte_2"] = player.Catapulte[1].ToString("#,0");
            _currentState["catapulte_3"] = player.Catapulte[2].ToString("#,0");
            _currentState["catapulte_4"] = player.Catapulte[3].ToString("#,0");
            _currentState["catapulte_5"] = player.Catapulte[4].ToString("#,0");

            _currentState["guerrieri_1_coda"] = unitsQueue.GetValueOrDefault("Guerrieri_1", 0).ToString("#,0");
            _currentState["guerrieri_2_coda"] = unitsQueue.GetValueOrDefault("Guerrieri_2", 0).ToString("#,0");
            _currentState["guerrieri_3_coda"] = unitsQueue.GetValueOrDefault("Guerrieri_3", 0).ToString("#,0");
            _currentState["guerrieri_4_coda"] = unitsQueue.GetValueOrDefault("Guerrieri_4", 0).ToString("#,0");
            _currentState["guerrieri_5_coda"] = unitsQueue.GetValueOrDefault("Guerrieri_5", 0).ToString("#,0");
            _currentState["lanceri_1_coda"] = unitsQueue.GetValueOrDefault("Lanceri_1", 0).ToString("#,0");
            _currentState["lanceri_2_coda"] = unitsQueue.GetValueOrDefault("Lanceri_2", 0).ToString("#,0");
            _currentState["lanceri_3_coda"] = unitsQueue.GetValueOrDefault("Lanceri_3", 0).ToString("#,0");
            _currentState["lanceri_4_coda"] = unitsQueue.GetValueOrDefault("Lanceri_4", 0).ToString("#,0");
            _currentState["lanceri_5_coda"] = unitsQueue.GetValueOrDefault("Lanceri_5", 0).ToString("#,0");
            _currentState["arceri_1_coda"] = unitsQueue.GetValueOrDefault("Arceri_1", 0).ToString("#,0");
            _currentState["arceri_2_coda"] = unitsQueue.GetValueOrDefault("Arceri_2", 0).ToString("#,0");
            _currentState["arceri_3_coda"] = unitsQueue.GetValueOrDefault("Arceri_3", 0).ToString("#,0");
            _currentState["arceri_4_coda"] = unitsQueue.GetValueOrDefault("Arceri_4", 0).ToString("#,0");
            _currentState["arceri_5_coda"] = unitsQueue.GetValueOrDefault("Arceri_5", 0).ToString("#,0");
            _currentState["catapulte_1_coda"] = unitsQueue.GetValueOrDefault("Catapulte_1", 0).ToString("#,0");
            _currentState["catapulte_2_coda"] = unitsQueue.GetValueOrDefault("Catapulte_2", 0).ToString("#,0");
            _currentState["catapulte_3_coda"] = unitsQueue.GetValueOrDefault("Catapulte_3", 0).ToString("#,0");
            _currentState["catapulte_4_coda"] = unitsQueue.GetValueOrDefault("Catapulte_4", 0).ToString("#,0");
            _currentState["catapulte_5_coda"] = unitsQueue.GetValueOrDefault("Catapulte_5", 0).ToString("#,0");

            _currentState["forza_esercito"] = player.forza_Esercito.ToString("#,0.00");

            _currentState["ricerca_produzione"] = player.Ricerca_Produzione.ToString();
            _currentState["ricerca_costruzione"] = player.Ricerca_Costruzione.ToString();
            _currentState["ricerca_addestramento"] = player.Ricerca_Addestramento.ToString();
            _currentState["ricerca_popolazione"] = player.Ricerca_Popolazione.ToString();
            _currentState["ricerca_riparazione"] = player.Ricerca_Riparazione.ToString();
            _currentState["ricerca_trasporto"] = player.Ricerca_Trasporto.ToString();

            _currentState["guerriero_salute"] = player.Guerriero_Salute.ToString();
            _currentState["guerriero_difesa"] = player.Guerriero_Difesa.ToString();
            _currentState["guerriero_attacco"] = player.Guerriero_Attacco.ToString();
            _currentState["guerriero_livello"] = player.Guerriero_Livello.ToString();
            _currentState["lancere_salute"] = player.Lancere_Salute.ToString();
            _currentState["lancere_difesa"] = player.Lancere_Difesa.ToString();
            _currentState["lancere_attacco"] = player.Lancere_Attacco.ToString();
            _currentState["lancere_livello"] = player.Lancere_Livello.ToString();
            _currentState["arcere_salute"] = player.Arcere_Salute.ToString();
            _currentState["arcere_difesa"] = player.Arcere_Difesa.ToString();
            _currentState["arcere_attacco"] = player.Arcere_Attacco.ToString();
            _currentState["arcere_livello"] = player.Arcere_Livello.ToString();
            _currentState["catapulta_salute"] = player.Catapulta_Salute.ToString();
            _currentState["catapulta_difesa"] = player.Catapulta_Difesa.ToString();
            _currentState["catapulta_attacco"] = player.Catapulta_Attacco.ToString();
            _currentState["catapulta_livello"] = player.Catapulta_Livello.ToString();

            _currentState["ricerca_ingresso_guarnigione"] = player.Ricerca_Ingresso_Guarnigione.ToString();
            _currentState["ricerca_citta_guarnigione"] = player.Ricerca_Citta_Guarnigione.ToString();
            _currentState["ricerca_cancello_salute"] = player.Ricerca_Cancello_Salute.ToString();
            _currentState["ricerca_cancello_difesa"] = player.Ricerca_Cancello_Difesa.ToString();
            _currentState["ricerca_cancello_guarnigione"] = player.Ricerca_Cancello_Guarnigione.ToString();
            _currentState["ricerca_mura_salute"] = player.Ricerca_Mura_Salute.ToString();
            _currentState["ricerca_mura_difesa"] = player.Ricerca_Mura_Difesa.ToString();
            _currentState["ricerca_mura_guarnigione"] = player.Ricerca_Mura_Guarnigione.ToString();
            _currentState["ricerca_torri_salute"] = player.Ricerca_Torri_Salute.ToString();
            _currentState["ricerca_torri_difesa"] = player.Ricerca_Torri_Difesa.ToString();
            _currentState["ricerca_torri_guarnigione"] = player.Ricerca_Torri_Guarnigione.ToString();
            _currentState["ricerca_castello_salute"] = player.Ricerca_Castello_Salute.ToString();
            _currentState["ricerca_castello_difesa"] = player.Ricerca_Castello_Difesa.ToString();
            _currentState["ricerca_castello_guarnigione"] = player.Ricerca_Castello_Guarnigione.ToString();
            _currentState["Ricerca_Attiva"] = player.Ricerca_Attiva.ToString();

            _currentState["Code_Costruzioni"] = player.Code_Costruzione.ToString();
            _currentState["Code_Reclutamenti"] = player.Code_Reclutamento.ToString();
            _currentState["Code_Costruzioni_Disponibili"] = player.task_Attuale_Costruzioni.Count.ToString();
            _currentState["Code_Reclutamenti_Disponibili"] = player.task_Attuale_Recutamento.Count.ToString();
            _currentState["Tempo_Costruzione"] = BuildingManagerV2.Get_Total_Building_Time(player).ToString();
            _currentState["Tempo_Reclutamento"] = UnitManagerV2.Get_Total_Recruit_Time(player).ToString();
            _currentState["Tempo_Ricerca_Citta"] = ResearchManager.GetTotalResearchTime(player).ToString();
            _currentState["Tempo_Ricerca_Globale"] = 1.ToString();

            _currentState["Potenza_Totale"] = player.Potenza_Totale.ToString();
            _currentState["Potenza_Strutture"] = player.Potenza_Strutture.ToString();
            _currentState["Potenza_Ricerca"] = player.Potenza_Ricerca.ToString();
            _currentState["Potenza_Esercito"] = player.Potenza_Esercito.ToString();

            _currentState["Unità_Eliminate"] = player.Unità_Eliminate.ToString();
            _currentState["Guerrieri_Eliminate"] = player.Guerrieri_Eliminati.ToString();
            _currentState["Lanceri_Eliminate"] = player.Lanceri_Eliminati.ToString();
            _currentState["Arceri_Eliminate"] = player.Arceri_Eliminati.ToString();
            _currentState["Catapulte_Eliminate"] = player.Catapulte_Eliminate.ToString();
            _currentState["Unità_Perse"] = player.Unità_Perse.ToString();
            _currentState["Guerrieri_Persi"] = player.Guerrieri_Persi.ToString();
            _currentState["Lanceri_Persi"] = player.Lanceri_Persi.ToString();
            _currentState["Arceri_Persi"] = player.Arceri_Persi.ToString();
            _currentState["Catapulte_Persi"] = player.Catapulte_Perse.ToString();
            _currentState["Risorse_Razziate"] = player.Risorse_Razziate.ToString();
            _currentState["Strutture_Civili_Costruite"] = player.Strutture_Civili_Costruite.ToString();
            _currentState["Strutture_Militari_Costruite"] = player.Strutture_Militari_Costruite.ToString();
            _currentState["Caserme_Costruite"] = player.Caserme_Costruite.ToString();
            _currentState["Frecce_Utilizzate"] = player.Frecce_Utilizzate.ToString();
            _currentState["Battaglie_Vinte"] = player.Battaglie_Vinte.ToString();
            _currentState["Battaglie_Perse"] = player.Battaglie_Perse.ToString();
            _currentState["Quest_Completate"] = player.Quest_Completate.ToString();
            _currentState["Attacchi_Subiti_PVP"] = player.Attacchi_Subiti_PVP.ToString();
            _currentState["Attacchi_Effettuati_PVP"] = player.Attacchi_Effettuati_PVP.ToString();
            _currentState["Barbari_Sconfitti"] = player.Barbari_Sconfitti.ToString();
            _currentState["Accampamenti_Barbari_Sconfitti"] = player.Accampamenti_Barbari_Sconfitti.ToString();
            _currentState["Città_Barbare_Sconfitte"] = player.Città_Barbare_Sconfitte.ToString();
            _currentState["Danno_HP_Barbaro"] = player.Danno_HP_Barbaro.ToString();
            _currentState["Danno_DEF_Barbaro"] = player.Danno_DEF_Barbaro.ToString();
            _currentState["Unità_Addestrate"] = player.Unità_Addestrate.ToString();
            _currentState["Risorse_Utilizzate"] = player.Risorse_Utilizzate.ToString();
            _currentState["Tempo_Addestramento_Risparmiato"] = player.FormatTime(player.Tempo_Addestramento).ToString();
            _currentState["Tempo_Costruzione_Risparmiato"] = player.FormatTime(player.Tempo_Costruzione).ToString();
            _currentState["Tempo_Ricerca_Risparmiato"] = player.FormatTime(player.Tempo_Ricerca).ToString();
            _currentState["Tempo_Sottratto_Diamanti"] = player.FormatTime(player.Tempo_Sottratto_Diamanti).ToString();

            _currentState["Bonus_Costruzione"] = (player.Bonus_Costruzione * 100).ToString() + "%";
            _currentState["Bonus_Addestramento"] = (player.Bonus_Addestramento * 100).ToString() + "%";
            _currentState["Bonus_Ricerca"] = (player.Bonus_Ricerca * 100).ToString() + "%";
            _currentState["Bonus_Riparazione"] = (player.Bonus_Riparazione * 100).ToString() + "%";
            _currentState["Bonus_Produzione_Risorse"] = (player.Bonus_Produzione_Risorse * 100).ToString() + "%";
            _currentState["Bonus_Capacità_Trasporto"] = (player.Bonus_Capacità_Trasporto * 100).ToString() + "%";
            _currentState["Bonus_Salute_Strutture"] = (player.Bonus_Salute_Strutture * 100).ToString() + "%";
            _currentState["Bonus_Difesa_Strutture"] = (player.Bonus_Difesa_Strutture * 100).ToString() + "%";
            _currentState["Bonus_Guarnigione_Strutture"] = (player.Bonus_Guarnigione_Strutture * 100).ToString() + "%";
            _currentState["Bonus_Attacco_Guerrieri"] = (player.Bonus_Attacco_Guerrieri * 100).ToString() + "%";
            _currentState["Bonus_Salute_Guerrieri"] = (player.Bonus_Salute_Guerrieri * 100).ToString() + "%";
            _currentState["Bonus_Difesa_Guerrieri"] = (player.Bonus_Difesa_Guerrieri * 100).ToString() + "%";
            _currentState["Bonus_Attacco_Lanceri"] = (player.Bonus_Attacco_Lanceri * 100).ToString() + "%";
            _currentState["Bonus_Salute_Lanceri"] = (player.Bonus_Salute_Lanceri * 100).ToString() + "%";
            _currentState["Bonus_Difesa_Lanceri"] = (player.Bonus_Difesa_Lanceri * 100).ToString() + "%";
            _currentState["Bonus_Attacco_Arceri"] = (player.Bonus_Attacco_Arceri * 100).ToString() + "%";
            _currentState["Bonus_Salute_Arceri"] = (player.Bonus_Salute_Arceri * 100).ToString() + "%";
            _currentState["Bonus_Difesa_Arceri"] = (player.Bonus_Difesa_Arceri * 100).ToString() + "%";
            _currentState["Bonus_Attacco_Catapulte"] = (player.Bonus_Attacco_Catapulte * 100).ToString() + "%";
            _currentState["Bonus_Salute_Catapulte"] = (player.Bonus_Salute_Catapulte * 100).ToString() + "%";
            _currentState["Bonus_Difesa_Catapulte"] = (player.Bonus_Difesa_Catapulte * 100).ToString() + "%";

            _currentState["punti_quest"] = player.Punti_Quest.ToString();

            _currentState["Guarnigione_Ingresso"] = player.Guarnigione_Ingresso.ToString();
            _currentState["Guarnigione_IngressoMax"] = player.Guarnigione_IngressoMax.ToString();
            _currentState["Guerrieri_1_Ingresso"] = player.Guerrieri_Ingresso[0].ToString();
            _currentState["Lanceri_1_Ingresso"] = player.Lanceri_Ingresso[0].ToString();
            _currentState["Arceri_1_Ingresso"] = player.Arceri_Ingresso[0].ToString();
            _currentState["Catapulte_1_Ingresso"] = player.Catapulte_Ingresso[0].ToString();
            _currentState["Guerrieri_2_Ingresso"] = player.Guerrieri_Ingresso[1].ToString();
            _currentState["Lanceri_2_Ingresso"] = player.Lanceri_Ingresso[1].ToString();
            _currentState["Arceri_2_Ingresso"] = player.Arceri_Ingresso[1].ToString();
            _currentState["Catapulte_2_Ingresso"] = player.Catapulte_Ingresso[1].ToString();
            _currentState["Guerrieri_3_Ingresso"] = player.Guerrieri_Ingresso[2].ToString();
            _currentState["Lanceri_3_Ingresso"] = player.Lanceri_Ingresso[2].ToString();
            _currentState["Arceri_3_Ingresso"] = player.Arceri_Ingresso[2].ToString();
            _currentState["Catapulte_3_Ingresso"] = player.Catapulte_Ingresso[2].ToString();
            _currentState["Guerrieri_4_Ingresso"] = player.Guerrieri_Ingresso[3].ToString();
            _currentState["Lanceri_4_Ingresso"] = player.Lanceri_Ingresso[3].ToString();
            _currentState["Arceri_4_Ingresso"] = player.Arceri_Ingresso[3].ToString();
            _currentState["Catapulte_4_Ingresso"] = player.Catapulte_Ingresso[3].ToString();
            _currentState["Guerrieri_5_Ingresso"] = player.Guerrieri_Ingresso[4].ToString();
            _currentState["Lanceri_5_Ingresso"] = player.Lanceri_Ingresso[4].ToString();
            _currentState["Arceri_5_Ingresso"] = player.Arceri_Ingresso[4].ToString();
            _currentState["Catapulte_5_Ingresso"] = player.Catapulte_Ingresso[4].ToString();

            _currentState["Guarnigione_Cancello"] = player.Guarnigione_Cancello.ToString();
            _currentState["Guarnigione_CancelloMax"] = player.Guarnigione_CancelloMax.ToString();
            _currentState["Guerrieri_1_Cancello"] = player.Guerrieri_Cancello[0].ToString();
            _currentState["Lanceri_1_Cancello"] = player.Lanceri_Cancello[0].ToString();
            _currentState["Arceri_1_Cancello"] = player.Arceri_Cancello[0].ToString();
            _currentState["Catapulte_1_Cancello"] = player.Catapulte_Cancello[0].ToString();
            _currentState["Guerrieri_2_Cancello"] = player.Guerrieri_Cancello[1].ToString();
            _currentState["Lanceri_2_Cancello"] = player.Lanceri_Cancello[1].ToString();
            _currentState["Arceri_2_Cancello"] = player.Arceri_Cancello[1].ToString();
            _currentState["Catapulte_2_Cancello"] = player.Catapulte_Cancello[1].ToString();
            _currentState["Guerrieri_3_Cancello"] = player.Guerrieri_Cancello[2].ToString();
            _currentState["Lanceri_3_Cancello"] = player.Lanceri_Cancello[2].ToString();
            _currentState["Arceri_3_Cancello"] = player.Arceri_Cancello[2].ToString();
            _currentState["Catapulte_3_Cancello"] = player.Catapulte_Cancello[2].ToString();
            _currentState["Guerrieri_4_Cancello"] = player.Guerrieri_Cancello[3].ToString();
            _currentState["Lanceri_4_Cancello"] = player.Lanceri_Cancello[3].ToString();
            _currentState["Arceri_4_Cancello"] = player.Arceri_Cancello[3].ToString();
            _currentState["Catapulte_4_Cancello"] = player.Catapulte_Cancello[3].ToString();
            _currentState["Guerrieri_5_Cancello"] = player.Guerrieri_Cancello[4].ToString();
            _currentState["Lanceri_5_Cancello"] = player.Lanceri_Cancello[4].ToString();
            _currentState["Arceri_5_Cancello"] = player.Arceri_Cancello[4].ToString();
            _currentState["Catapulte_5_Cancello"] = player.Catapulte_Cancello[4].ToString();
            _currentState["Salute_Cancello"] = player.Salute_Cancello.ToString();
            _currentState["Salute_CancelloMax"] = player.Salute_CancelloMax.ToString();
            _currentState["Difesa_Cancello"] = player.Difesa_Cancello.ToString();
            _currentState["Difesa_CancelloMax"] = player.Difesa_CancelloMax.ToString();

            _currentState["Guarnigione_Mura"] = player.Guarnigione_Mura.ToString();
            _currentState["Guarnigione_MuraMax"] = player.Guarnigione_MuraMax.ToString();
            _currentState["Guerrieri_1_Mura"] = player.Guerrieri_Mura[0].ToString();
            _currentState["Lanceri_1_Mura"] = player.Lanceri_Mura[0].ToString();
            _currentState["Arceri_1_Mura"] = player.Arceri_Mura[0].ToString();
            _currentState["Catapulte_1_Mura"] = player.Catapulte_Mura[0].ToString();
            _currentState["Guerrieri_2_Mura"] = player.Guerrieri_Mura[1].ToString();
            _currentState["Lanceri_2_Mura"] = player.Lanceri_Mura[1].ToString();
            _currentState["Arceri_2_Mura"] = player.Arceri_Mura[1].ToString();
            _currentState["Catapulte_2_Mura"] = player.Catapulte_Mura[1].ToString();
            _currentState["Guerrieri_3_Mura"] = player.Guerrieri_Mura[2].ToString();
            _currentState["Lanceri_3_Mura"] = player.Lanceri_Mura[2].ToString();
            _currentState["Arceri_3_Mura"] = player.Arceri_Mura[2].ToString();
            _currentState["Catapulte_3_Mura"] = player.Catapulte_Mura[2].ToString();
            _currentState["Guerrieri_4_Mura"] = player.Guerrieri_Mura[3].ToString();
            _currentState["Lanceri_4_Mura"] = player.Lanceri_Mura[3].ToString();
            _currentState["Arceri_4_Mura"] = player.Arceri_Mura[3].ToString();
            _currentState["Catapulte_4_Mura"] = player.Catapulte_Mura[3].ToString();
            _currentState["Guerrieri_5_Mura"] = player.Guerrieri_Mura[4].ToString();
            _currentState["Lanceri_5_Mura"] = player.Lanceri_Mura[4].ToString();
            _currentState["Arceri_5_Mura"] = player.Arceri_Mura[4].ToString();
            _currentState["Catapulte_5_Mura"] = player.Catapulte_Mura[4].ToString();
            _currentState["Salute_Mura"] = player.Salute_Mura.ToString();
            _currentState["Salute_MuraMax"] = player.Salute_MuraMax.ToString();
            _currentState["Difesa_Mura"] = player.Difesa_Mura.ToString();
            _currentState["Difesa_MuraMax"] = player.Difesa_MuraMax.ToString();

            _currentState["Guarnigione_Torri"] = player.Guarnigione_Torri.ToString();
            _currentState["Guarnigione_TorriMax"] = player.Guarnigione_TorriMax.ToString();
            _currentState["Guerrieri_1_Torri"] = player.Guerrieri_Torri[0].ToString();
            _currentState["Lanceri_1_Torri"] = player.Lanceri_Torri[0].ToString();
            _currentState["Arceri_1_Torri"] = player.Arceri_Torri[0].ToString();
            _currentState["Catapulte_1_Torri"] = player.Catapulte_Torri[0].ToString();
            _currentState["Guerrieri_2_Torri"] = player.Guerrieri_Torri[1].ToString();
            _currentState["Lanceri_2_Torri"] = player.Lanceri_Torri[1].ToString();
            _currentState["Arceri_2_Torri"] = player.Arceri_Torri[1].ToString();
            _currentState["Catapulte_2_Torri"] = player.Catapulte_Torri[1].ToString();
            _currentState["Guerrieri_3_Torri"] = player.Guerrieri_Torri[2].ToString();
            _currentState["Lanceri_3_Torri"] = player.Lanceri_Torri[2].ToString();
            _currentState["Arceri_3_Torri"] = player.Arceri_Torri[2].ToString();
            _currentState["Catapulte_3_Torri"] = player.Catapulte_Torri[2].ToString();
            _currentState["Guerrieri_4_Torri"] = player.Guerrieri_Torri[3].ToString();
            _currentState["Lanceri_4_Torri"] = player.Lanceri_Torri[3].ToString();
            _currentState["Arceri_4_Torri"] = player.Arceri_Torri[3].ToString();
            _currentState["Catapulte_4_Torri"] = player.Catapulte_Torri[3].ToString();
            _currentState["Guerrieri_5_Torri"] = player.Guerrieri_Torri[4].ToString();
            _currentState["Lanceri_5_Torri"] = player.Lanceri_Torri[4].ToString();
            _currentState["Arceri_5_Torri"] = player.Arceri_Torri[4].ToString();
            _currentState["Catapulte_5_Torri"] = player.Catapulte_Torri[4].ToString();
            _currentState["Salute_Torri"] = player.Salute_Torri.ToString();
            _currentState["Salute_TorriMax"] = player.Salute_TorriMax.ToString();
            _currentState["Difesa_Torri"] = player.Difesa_Torri.ToString();
            _currentState["Difesa_TorriMax"] = player.Difesa_TorriMax.ToString();

            _currentState["Guarnigione_Castello"] = player.Guarnigione_Castello.ToString();
            _currentState["Guarnigione_CastelloMax"] = player.Guarnigione_CastelloMax.ToString();
            _currentState["Guerrieri_1_Castello"] = player.Guerrieri_Castello[0].ToString();
            _currentState["Lanceri_1_Castello"] = player.Lanceri_Castello[0].ToString();
            _currentState["Arceri_1_Castello"] = player.Arceri_Castello[0].ToString();
            _currentState["Catapulte_1_Castello"] = player.Catapulte_Castello[0].ToString();
            _currentState["Guerrieri_2_Castello"] = player.Guerrieri_Castello[1].ToString();
            _currentState["Lanceri_2_Castello"] = player.Lanceri_Castello[1].ToString();
            _currentState["Arceri_2_Castello"] = player.Arceri_Castello[1].ToString();
            _currentState["Catapulte_2_Castello"] = player.Catapulte_Castello[1].ToString();
            _currentState["Guerrieri_3_Castello"] = player.Guerrieri_Castello[2].ToString();
            _currentState["Lanceri_3_Castello"] = player.Lanceri_Castello[2].ToString();
            _currentState["Arceri_3_Castello"] = player.Arceri_Castello[2].ToString();
            _currentState["Catapulte_3_Castello"] = player.Catapulte_Castello[2].ToString();
            _currentState["Guerrieri_4_Castello"] = player.Guerrieri_Castello[3].ToString();
            _currentState["Lanceri_4_Castello"] = player.Lanceri_Castello[3].ToString();
            _currentState["Arceri_4_Castello"] = player.Arceri_Castello[3].ToString();
            _currentState["Catapulte_4_Castello"] = player.Catapulte_Castello[3].ToString();
            _currentState["Guerrieri_5_Castello"] = player.Guerrieri_Castello[4].ToString();
            _currentState["Lanceri_5_Castello"] = player.Lanceri_Castello[4].ToString();
            _currentState["Arceri_5_Castello"] = player.Arceri_Castello[4].ToString();
            _currentState["Catapulte_5_Castello"] = player.Catapulte_Castello[4].ToString();
            _currentState["Salute_Castello"] = player.Salute_Castello.ToString();
            _currentState["Salute_CastelloMax"] = player.Salute_CastelloMax.ToString();
            _currentState["Difesa_Castello"] = player.Difesa_Castello.ToString();
            _currentState["Difesa_CastelloMax"] = player.Difesa_CastelloMax.ToString();

            _currentState["Guarnigione_Citta"] = player.Guarnigione_Citta.ToString();
            _currentState["Guarnigione_CittaMax"] = player.Guarnigione_CittaMax.ToString();
            _currentState["Guerrieri_1_Citta"] = player.Guerrieri_Citta[0].ToString();
            _currentState["Lanceri_1_Citta"] = player.Lanceri_Citta[0].ToString();
            _currentState["Arceri_1_Citta"] = player.Arceri_Citta[0].ToString();
            _currentState["Catapulte_1_Citta"] = player.Catapulte_Citta[0].ToString();
            _currentState["Guerrieri_2_Citta"] = player.Guerrieri_Citta[1].ToString();
            _currentState["Lanceri_2_Citta"] = player.Lanceri_Citta[1].ToString();
            _currentState["Arceri_2_Citta"] = player.Arceri_Citta[1].ToString();
            _currentState["Catapulte_2_Citta"] = player.Catapulte_Citta[1].ToString();
            _currentState["Guerrieri_3_Citta"] = player.Guerrieri_Citta[2].ToString();
            _currentState["Lanceri_3_Citta"] = player.Lanceri_Citta[2].ToString();
            _currentState["Arceri_3_Citta"] = player.Arceri_Citta[2].ToString();
            _currentState["Catapulte_3_Citta"] = player.Catapulte_Citta[2].ToString();
            _currentState["Guerrieri_4_Citta"] = player.Guerrieri_Citta[3].ToString();
            _currentState["Lanceri_4_Citta"] = player.Lanceri_Citta[3].ToString();
            _currentState["Arceri_4_Citta"] = player.Arceri_Citta[3].ToString();
            _currentState["Catapulte_4_Citta"] = player.Catapulte_Citta[3].ToString();
            _currentState["Guerrieri_5_Citta"] = player.Guerrieri_Citta[4].ToString();
            _currentState["Lanceri_5_Citta"] = player.Lanceri_Citta[4].ToString();
            _currentState["Arceri_5_Citta"] = player.Arceri_Citta[4].ToString();
            _currentState["Catapulte_5_Citta"] = player.Catapulte_Citta[4].ToString();

            return _currentState;
        }

        // Calcola e restituisce solo i campi cambiati
        public string BuildDelta(Dictionary<string, string> current)
        {
            var delta = new StringBuilder("Update_Data|");
            bool hasChanges = false;

            foreach (var kv in current)
                if (!_lastSent.TryGetValue(kv.Key, out var old) || old != kv.Value)
                {
                    delta.Append($"{kv.Key}={kv.Value}|");
                    _lastSent[kv.Key] = kv.Value; // ← aggiorna solo il campo cambiato, niente new Dictionary
                    hasChanges = true;
                }

            if (!hasChanges) return null;
            return delta.ToString();
        }

        // Forza l'invio completo (es. al login)
        public void Reset() => _lastSent.Clear();
    }
}