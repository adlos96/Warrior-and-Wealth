using Server_Strategico.Gioco;

namespace Server_Strategico.ServerData.Localization
{
    internal interface ILocalization
    {
        string GetQuestDescription(int questId);
        string GetTutorialObiettivo(int statoTutorial);
        string GetTutorialDescrizione(int statoTutorial);

        // Etichette UI riusabili
        string Label_CostoCostruzione();
        string Label_CostoAddestramento();
        string Label_Statistiche();
        string Label_Cibo();
        string Label_Legno();
        string Label_Pietra();
        string Label_Ferro();
        string Label_Oro();
        string Label_Popolazione();
        string Label_Spade();
        string Label_Lancie();
        string Label_Archi();
        string Label_Scudi();
        string Label_Armature();
        string Label_Frecce();
        string Label_Costruzione();
        string Label_Addestramento();
        string Label_Ricerca();
        string Label_Produzione();

        //Mantenimento
        string Label_Mantenimento_Cibo();
        string Label_Mantenimento_Legno();
        string Label_Mantenimento_Pietra();
        string Label_Mantenimento_Ferro();
        string Label_Mantenimento_Oro();

        //Unità
        string Label_Livello();
        string Label_Salute();
        string Label_Difesa();
        string Label_Attacco();

        string Label_Limite_Magazzino();
        string Label_Limite_Unità();
        string Label_Limite_Strutture();

        //Costruzione
        string Costruzione_RisorseUtilizzate(int count, string buildingType, Strutture.Edifici cost);
        string Costruzione_RisorseInsufficienti(int count, string buildingType);
        string Costruzione_Pausa(string struttura);
        string Costruzione_Avvio(string struttura, string tempo);
        string Costruzione_Completata(string struttura);
        string Costruzione_EdificioNonValido(string struttura);

        //Accelerazione
        string NumeroDiamantiNonValido();
        string DiamantiInsufficienti();
        string Costruzione_NessunaCostruzione();
        string Costruzione_Velocizzazione(int diamanti, string tempo);

        //Terreni
        string Terreni_DiamantiInsufficienti();
        string Terreni_DiamantiUtilizzati();
        string Terreni_Ottenuto(string terreno);

        //Addestramento
        string Addestramento_RisorseUtilizzate(int count, string buildingType, Esercito.CostoReclutamento cost);
        string Addestramento_RisorseInsufficienti(int count, string buildingType);
        string Addestramento_Pausa(string unità);
        string Addestramento_Avvio(string unità, string tempo);
        string Addestramento_Completata(string unità);
        string Addestramento_UnitàNonValido(string unità);

        string Addestramento_NessunaUnità();
        string Addestramento_Velocizzazione(int diamanti, string tempo);

        //Ricerca
        string Ricerca_LivelloRichiesto(string ricerca, int livelloRicerca, string msg, int richiesto);
        string Ricerca_Start(string ricerca, string tempo);
        string Ricerca_Completata(string ricerca);
        string Ricerca_NessunaRicerca();
        string Ricerca_Velocizzazione(int diamanti, string tempo);

        string Desc_Fattoria();
        string Desc_Segheria();
        string Desc_CavaPietra();
        string Desc_MinieraFerro();
        string Desc_MinieraOro();
        string Desc_Case();

        string Desc_ProduzioneSpade();
        string Desc_ProduzioneLance();
        string Desc_ProduzioneArchi();
        string Desc_ProduzioneScudi();
        string Desc_ProduzioneArmature();
        string Desc_ProduzioneFrecce();

        string Desc_CasermaGuerrieri();
        string Desc_CasermaLanceri();
        string Desc_CasermaArceri();
        string Desc_CasermaCatapulte();

        string Desc_Guerriero(int numero);
        string Desc_Lancere(int numero);
        string Desc_Arcere(int numero);
        string Desc_Catapulta(int numero);


        string Desc_Esperienza(string esperienza);
        string Desc_Livello();
        string Desc_Statistiche();
        string Desc_DiamantiBlu();
        string Desc_DiamantiViola();
        string Desc_DollariVirtuali();
        string Desc_Cibo();
        string Desc_Legno();
        string Desc_Pietra();
        string Desc_Ferro();
        string Desc_Oro();
        string Desc_Popolazione();

        string Desc_Spade();
        string Desc_Lance();
        string Desc_Archi();
        string Desc_Scudi();
        string Desc_Armature();
        string Desc_Frecce();

        string Desc_Shop_GamePassBase();
        string Desc_Shop_GamePassAvanzato();
        string Desc_Shop_Vip1();
        string Desc_Shop_Vip2();
        string Desc_Shop_Costruttore(string durata);
        string Desc_Shop_Reclutatore(string durata);
        string Desc_Shop_ScudoPace(string durata);

        //Ricerca
        string Desc_SaluteMura();
        string Desc_DifesaMura();
        string Desc_SaluteCancello();
        string Desc_DifesaCancello();
        string Desc_SaluteTorri();
        string Desc_DifesaTorri();
        string Desc_CastelloSalute();
        string Desc_CastelloDifesa();
        string Desc_RicercaAddestramento(int livello);
        string Desc_RicercaCostruzione(int livello);
        string Desc_RicercaProduzione(int livello);
        string Desc_RicercaPopolazione(int livello);
        string Desc_RicercaTrasporto(int livello);
        string Desc_RicercaRiparazione(int livello);
        string Desc_RicercaGuerrieroLivello(int livello);
        string Desc_RicercaGuerrieroSalute(int livello);
        string Desc_RicercaGuerrieroAttacco(int livello);
        string Desc_RicercaGuerrieroDifesa(int livello);
        string Desc_RicercaLancereLivello(int livello);
        string Desc_RicercaLancereSalute(int livello);
        string Desc_RicercaLancereAttacco(int livello);
        string Desc_RicercaLancereDifesa(int livello);
        string Desc_RicercaArcereLivelloe(int livello);
        string Desc_RicercaArcereSalute(int livello);
        string Desc_RicercaArcereAttacco(int livello);
        string Desc_RicercaArcereDifesa(int livello);
        string Desc_RicercaCatapultaLivello(int livello);
        string Desc_RicercaCatapultaSalute(int livello);
        string Desc_RicercaCatapultaAttacco(int livello);
        string Desc_RicercaCatapultaDifesa(int livello);
        string Desc_RicercaIngressoGuarnigione(int livello);
        string Desc_RicercaCittaGuarnigione(int livello);
        string Desc_RicercaMuraLivello(int livello);
        string Desc_RicercaMuraGuarnigione(int livello);
        string Desc_RicercaMuraSalute(int livello);
        string Desc_RicercaMuraDifesa(int livello);

        string Desc_RicercaCancelloLivello(int livello);
        string Desc_RicercaCancelloGuarnigione(int livello);
        string Desc_RicercaCancelloSalute(int livello);
        string Desc_RicercaCancelloDifesa(int livello);
        string Desc_RicercaTorriLivello(int livello);
        string Desc_RicercaTorriGuarnigione(int livello);
        string Desc_RicercaTorriSalute(int livello);
        string Desc_RicercaTorriDifesa(int livello);
        string Desc_RicercaCastelloLivello(int livello);
        string Desc_RicercaCastelloGuarnigione(int livello);
        string Desc_RicercaCastelloSalute(int livello);
        string Desc_RicercaCastelloDifesa(int livello);

        string Desc_Feudi_Testo();
        string Desc_Città_Testo();
        string Desc_Ricerca_Testo();
        string Label_Comune();
        string Label_NonComune();
        string Label_Raro();
        string Label_Epico();
        string Label_Leggendario();
    }
}
