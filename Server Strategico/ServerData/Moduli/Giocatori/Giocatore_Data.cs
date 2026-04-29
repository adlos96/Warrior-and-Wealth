
namespace Server_Strategico.ServerData.Moduli.Player
{
    public class Giocatore_Data
    {
        public class Player_Base
        {
            public string username { get; set; } = "";
            public string password { get; set; } = "";
            public string lingua { get; set; } = "";
            public Guid guid_Player { get; set; } = Guid.Empty;
            public Risorse_Civili risorse_Civili { get; set; } = new Risorse_Civili();
            public Risorse_Civili risorse_Militari { get; set; } = new Risorse_Civili();
            public Feudi feudi { get; set; } = new Feudi();
            public Strutture_Civili strutture_Civili { get; set; } = new Strutture_Civili();
            public Strutture_Militari strutture_Militari { get; set; } = new Strutture_Militari();
            public Caserme caserme { get; set; } = new Caserme();
            public Esercito esercito { get; set; } = new Esercito();
            public Villaggio villaggio { get; set; } = new Villaggio();
            public Ricerca_Giocatore ricerca_Giocatore { get; set; } = new Ricerca_Giocatore();
            public Ricerca_Città ricerca_Città { get; set; } = new Ricerca_Città();
            public Livelli livello_Unità { get; set; } = new Livelli();
            public Potenza potenza { get; set; } = new Potenza();
            public Bonus bonus { get; set; } = new Bonus();
            public Statistiche statistiche { get; set; } = new Statistiche();
            public Limiti limiti { get; set; } = new Limiti();

        }
        public class Risorse_Civili
        {
            public double Cibo { get; set; } = 0;
            public double Legno { get; set; } = 0;
            public double Pietra { get; set; } = 0;
            public double Ferro { get; set; } = 0;
            public double Oro { get; set; } = 0;
            public double Popolazione { get; set; } = 0;
        }
        public class Risorse_Speciali
        {
            public int Diamanti_Blu { get; set; } = 0;
            public int Diamanti_Viola { get; set; } = 0;
            public decimal Dollari_Virtuali { get; set; } = 0;
        }
        public class Bonus
        {
            public double Salute_Guerrieri { get; set; } = 0;
            public double Salute_Lanceri { get; set; } = 0;
            public double Salute_Arceri { get; set; } = 0;
            public double Salute_Catapulte { get; set; } = 0;
            public double Difesa_Guerrieri { get; set; } = 0;
            public double Difesa_Lanceri { get; set; } = 0;
            public double Difesa_Arceri { get; set; } = 0;
            public double Difesa_Catapulte { get; set; } = 0;
            public double Attacco_Guerrieri { get; set; } = 0;
            public double Attacco_Lanceri { get; set; } = 0;
            public double Attacco_Arceri { get; set; } = 0;
            public double Attacco_Catapulte { get; set; } = 0;

            public double Salute_Strutture { get; set; } = 0;
            public double Difesa_Strutture { get; set; } = 0;
            public double Guarnigione_Strutture { get; set; } = 0;

            public double Produzione_Risorse { get; set; } = 0;
            public double Capacità_Trasporto { get; set; } = 0;
            public double Costruzione { get; set; } = 0;
            public double Addestramento { get; set; } = 0;
            public double Ricerca { get; set; } = 0;
            public double Riparazione { get; set; } = 0;
        }
        public class Villaggio
        {
           public Struttura ingresso { get; set; } = new Struttura();
            public Struttura mura { get; set; } = new Struttura();
            public Struttura cancello { get; set; } = new Struttura();
            public Struttura torri { get; set; } = new Struttura();
            public Struttura centro { get; set; } = new Struttura();
            public Struttura castello { get; set; } = new Struttura();
        }
        public class Struttura
        {
            public int salute { get; set; } = 0;
            public int difesa { get; set; } = 0;
            public int salute_Max { get; set; } = 0;
            public int difesa_Max { get; set; } = 0;
            public Esercito guarnigione { get; set; } = new Esercito();
        }
        public class Feudi
        {
            public int Comune { get; set; } = 0;
            public int NonComune { get; set; } = 0;
            public int Raro { get; set; } = 0;
            public int Epico { get; set; } = 0;
            public int Leggendario { get; set; } = 0;
        }
        public class Strutture_Civili
        {
            public int Fattoria { get; set; } = 0;
            public int Segheria { get; set; } = 0;
            public int CavaPietra { get; set; } = 0;
            public int MinieraFerro { get; set; } = 0;
            public int MinieraOro { get; set; } = 0;
            public int Abitazioni { get; set; } = 0;
        }
        public class Strutture_Militari
        {
            public int Workshop_Spade { get; set; } = 0;
            public int Workshop_Lance { get; set; } = 0;
            public int Workshop_Archi { get; set; } = 0;
            public int Workshop_Scudi { get; set; } = 0;
            public int Workshop_Armature { get; set; } = 0;
            public int Workshop_Frecce { get; set; } = 0;
        }
        public class Caserme
        {
            public int Guerrieri { get; set; } = 0;
            public int Lanceri { get; set; } = 0;
            public int Arceri { get; set; } = 0;
            public int Catapulte { get; set; } = 0;
        }
        public class Esercito
        {
            public int[] Guerrieri = new int[5];
            public int[] Lanceri = new int[5];
            public int[] Arceri = new int[5];
            public int[] Catapulte = new int[5];
        }
        public class Limiti
        {
            public int Diamanti_Viola_PVP_Ottenuti { get; set; } = 0;
            public int Diamanti_Blu_PVP_Ottenuti { get; set; } = 0;
            public int Diamanti_Viola_PVP_Persi { get; set; } = 0;
            public int Diamanti_Blu_PVP_Persi { get; set; } = 0;
            public int GuerrieriMax { get; set; } = 0;
            public int LancieriMax { get; set; } = 0;
            public int ArceriMax { get; set; } = 0;
            public int CatapulteMax { get; set; } = 0;
        }
        public class Potenza
        {
            public double Totale { get; set; } = 0;
            public double Esercito { get; set; } = 0;
            public double Strutture { get; set; } = 0;
            public double Ricerca { get; set; } = 0;
        }
        public class Livelli
        {
            public Livello_Unità guerriero { get; set; } = new Livello_Unità();
            public Livello_Unità lancere { get; set; } = new Livello_Unità();
            public Livello_Unità arcere { get; set; } = new Livello_Unità();
            public Livello_Unità catapulta { get; set; } = new Livello_Unità();
        }
        public class Livello_Unità
        {
            public int Livello { get; set; } = 0;
            public int Salute { get; set; } = 0;
            public int Difesa { get; set; } = 0;
            public int Attacco { get; set; } = 0;
        }
        public class Ricerca_Giocatore
        {
            public int Produzione { get; set; } = 0;
            public int Costruzione { get; set; } = 0;
            public int Addestramento { get; set; } = 0;
            public int Popolazione { get; set; } = 0;
            public int Trasporto { get; set; } = 0;
            public int Riparazione { get; set; } = 0;
            public int Spionaggio { get; set; } = 0;
            public int Contro_Spionaggio { get; set; } = 0;
        }
        public class Ricerca_Città
        {
            public int Ingresso_Guarnigione { get; set; } = 0;
            public int Citta_Guarnigione { get; set; } = 0;

            public int Cancello_Livello { get; set; } = 0;
            public int Cancello_Salute { get; set; } = 0;
            public int Cancello_Difesa { get; set; } = 0;
            public int Cancello_Guarnigione { get; set; } = 0;

            public int Mura_Livello { get; set; } = 0;
            public int Mura_Salute { get; set; } = 0;
            public int Mura_Difesa { get; set; } = 0;
            public int Mura_Guarnigione { get; set; } = 0;

            public int Torri_Livello { get; set; } = 0;
            public int Torri_Salute { get; set; } = 0;
            public int Torri_Difesa { get; set; } = 0;
            public int Torri_Guarnigione { get; set; } = 0;

            public int Castello_Livello { get; set; } = 0;
            public int Castello_Salute { get; set; } = 0;
            public int Castello_Difesa { get; set; } = 0;
            public int Castello_Guarnigione { get; set; } = 0;
        }
        public class Risorse_Militari
        {
            public double Spade { get; set; } = 0;
            public double Lance { get; set; } = 0;
            public double Archi { get; set; } = 0;
            public double Scudi { get; set; } = 0;
            public double Armature { get; set; } = 0;
            public double Frecce { get; set; } = 0;
        }
        public class Statistiche
        {
            public int Unità_Eliminate { get; set; } = 0;
            public int Guerrieri_Eliminati { get; set; } = 0;
            public int Lanceri_Eliminati { get; set; } = 0;
            public int Arceri_Eliminati { get; set; } = 0;
            public int Catapulte_Eliminate { get; set; } = 0;

            public int Unità_Perse { get; set; } = 0;
            public int Guerrieri_Persi { get; set; } = 0;
            public int Lanceri_Persi { get; set; } = 0;
            public int Arceri_Persi { get; set; } = 0;
            public int Catapulte_Perse { get; set; } = 0;
            public int Risorse_Razziate { get; set; } = 0;

            public int Strutture_Civili_Costruite { get; set; } = 0;
            public int Strutture_Militari_Costruite { get; set; } = 0;
            public int Caserme_Costruite { get; set; } = 0;

            public int Frecce_Utilizzate { get; set; } = 0;
            public int Battaglie_Vinte { get; set; } = 0;
            public int Battaglie_Perse { get; set; } = 0;
            public int Quest_Completate { get; set; } = 0;
            public int Attacchi_Subiti_PVP { get; set; } = 0;
            public int Attacchi_Effettuati_PVP { get; set; } = 0;

            public int Barbari_Sconfitti { get; set; } = 0; //Totale uomini barbari sconfitti (villaggi e città)
            public int Accampamenti_Barbari_Sconfitti { get; set; } = 0; //Villaggi barbari sconfitti
            public int Città_Barbare_Sconfitte { get; set; } = 0;
            public int Danno_HP_Barbaro { get; set; } = 0;
            public int Danno_DEF_Barbaro { get; set; } = 0;

            public int Unità_Addestrate { get; set; } = 0;
            public int Risorse_Utilizzate { get; set; } = 0;
            public int Tempo_Addestramento { get; set; } = 0;
            public int Tempo_Costruzione { get; set; } = 0;
            public int Tempo_Ricerca { get; set; } = 0;
            public int Tempo_Sottratto_Diamanti { get; set; } = 0; //Tempo risparmiato usando diamanti

            public int Consumo_Cibo_Esercito { get; set; } = 0;
            public int Consumo_Oro_Esercito { get; set; } = 0;

            public int Diamanti_Viola_Utilizzati { get; set; } = 0;
            public int Diamanti_Blu_Utilizzati { get; set; } = 0;
        }

    }
}
