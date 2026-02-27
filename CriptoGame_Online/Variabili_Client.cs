using Warrior_and_Wealth.GUI;
using System.Collections.Generic;

namespace Strategico_V2
{
    internal class Variabili_Client
    {
        public class VillaggioClient
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
        public class PacchettoVillaggi
        {
            public string Type { get; set; }
            public List<VillaggioClient> Dati { get; set; }
        }
        public class dati
        {
            public int StatoTutorial { get; set; }
            public string Obiettivo { get; set; }
            public string Descrizione { get; set; }
        }

        public static string versione_Client_Necessario = "0";
        public static string versione_Client_Attuale = "0.1.13.2";

        public static string Ricerca_1_Bottone_Cliccato = "";
        public static string Server = "0";
        public static string Versione = "0";

        public static string Esperienza_Desc = "";
        public static string Livello_Desc = "";
        public static string D_Viola_D_Blu = "";
        public static string Tributi_D_Viola = "";
        public static string Tempo_D_Blu = "";

        public static string Giocatore_Desc = "";
        public static string Diamanti_Blu_Desc = "";
        public static string Diamanti_Viola_Desc = "";
        public static string Dollari_VIrtuali_Desc = "";

        public static string Cibo_Desc = "";
        public static string Legno_Desc = "";
        public static string Pietra_Desc = "";
        public static string Ferro_Desc = "";
        public static string Oro_Desc = "";
        public static string Popolazione_Desc = "";

        public static string Spade_Desc = "";
        public static string Lance_Desc = "";
        public static string Archi_Desc = "";
        public static string Scudi_Desc = "";
        public static string Armature_Desc = "";
        public static string Frecce_Desc = "";

        public static string Ricerca_Addestramento_Desc = "";
        public static string Ricerca_Produzione_Desc = "";
        public static string Ricerca_Costruzione_Desc = "";
        public static string Ricerca_Popolazione_Desc = "";
        public static string Ricerca_Trasporto_Desc = "";
        public static string Ricerca_Riparazione_Desc = "";

        public static string Ricerca_Guerrieri_Livello_Desc = "";
        public static string Ricerca_Guerrieri_Salute_Desc = "";
        public static string Ricerca_Guerrieri_Attacco_Desc = "";
        public static string Ricerca_Guerrieri_Difesa_Desc = "";

        public static string Ricerca_Lanceri_Livello_Desc = "";
        public static string Ricerca_Lanceri_Salute_Desc = "";
        public static string Ricerca_Lanceri_Attacco_Desc = "";
        public static string Ricerca_Lanceri_Difesa_Desc = "";

        public static string Ricerca_Arceri_Livello_Desc = "";
        public static string Ricerca_Arceri_Salute_Desc = "";
        public static string Ricerca_Arceri_Attacco_Desc = "";
        public static string Ricerca_Arceri_Difesa_Desc = "";

        public static string Ricerca_Catapulte_Livello_Desc = "";
        public static string Ricerca_Catapulte_Salute_Desc = "";
        public static string Ricerca_Catapulte_Attacco_Desc = "";
        public static string Ricerca_Catapulte_Difesa_Desc = "";

        public static string Ricerca_Ingresso_Livello_Desc = "";
        public static string Ricerca_Ingresso_Guarnigione_Desc = "";

        public static string Ricerca_Citta_Livello_Desc = "";
        public static string Ricerca_Citta_Guarnigione_Desc = "";

        public static string Ricerca_Mura_Livello_Desc = "";
        public static string Ricerca_Mura_Salute_Desc = "";
        public static string Ricerca_Mura_Difesa_Desc = "";
        public static string Ricerca_Mura_Guarnigione_Desc = "";

        public static string Ricerca_Cancello_Livello_Desc = "";
        public static string Ricerca_Cancello_Salute_Desc = "";
        public static string Ricerca_Cancello_Difesa_Desc = "";
        public static string Ricerca_Cancello_Guarnigione_Desc = "";

        public static string Ricerca_Torri_Livello_Desc = "";
        public static string Ricerca_Torri_Salute_Desc = "";
        public static string Ricerca_Torri_Difesa_Desc = "";
        public static string Ricerca_Torri_Guarnigione_Desc = "";

        public static string Ricerca_Castello_Livello_Desc = "";
        public static string Ricerca_Castello_Salute_Desc = "";
        public static string Ricerca_Castello_Difesa_Desc = "";
        public static string Ricerca_Castello_Guarnigione_Desc = "";

        //Sblocco Esercito
        public static string truppe_II = "0";
        public static string truppe_III = "0";
        public static string truppe_IV = "0";
        public static string truppe_V = "0";
        public static string unlock_Città_Barbare = "0";
        public static string unlock_PVP = "0";

        public static List<string> Giocatori_PVP = new List<string>();
        public static List<string> Raduni_Creati = new List<string>(); //Raduni pubblici
        public static List<string> Raduni_InCorso = new List<string>(); //Raduni a cui si partecipa inviando truppe

        public static List<VillaggioClient> VillaggiPersonali = new();
        public static List<VillaggioClient> CittaGlobali = new();

        public static bool tutorial_Attivo = true;
        public static bool[] tutorial = new bool[32];
        public static string[] GamePass_Premi = new string[90];
        public static bool[] GamePass_Premi_Completati = new bool[90];
        public static int Giorni_Accessi_Consecutivi = 0;
        public static List<dati> tutorial_dati = new List<dati>();

        public class Dati
        {
            public string Costo_terreni_Virtuali { get; set; }

            public string Username { get; set; }
            public string Password { get; set; }
            public string Montly_Quest_Point { get; set; }
            public string Montly_Quest_Tempo { get; set; }
            public string Barbari_Tempo { get; set; }
            public bool User_Vip { get; set; }
            public string User_Vip_Tempo { get; set; }
            public bool User_GamePass_Base { get; set; }
            public string User_GamePass_Base_Tempo { get; set; }
            public bool User_GamePass_Avanzato { get; set; }
            public string User_GamePass_Avanzato_Tempo { get; set; }
            public bool Ricerca_Attiva { get; set; }
            public bool User_Login { get; set; }

            public bool Scudo_Pace { get; set; }
            public string Scudo_Pace_Tempo { get; set; }
            public string Costruttori_Tempo { get; set; }
            public string Reclutatori_Tempo { get; set; }
            public string Code_Costruzione { get; set; }
            public string Code_Reclutamento { get; set; }
            public string Code_Costruzione_Disponibili { get; set; }
            public string Code_Reclutamento_Disponibili { get; set; }
            public string Tempo_Costruzione { get; set; }
            public string Tempo_Reclutamento { get; set; }
            public string Tempo_Ricerca { get; set; }

            public string Livello { get; set; }
            public string Esperienza { get; set; }
            public int Salute { get; set; }
            public int Difesa { get; set; }
            public int Guarnigione { get; set; }
            public int Salute_Max { get; set; }
            public int Difesa_Max { get; set; }
            public int Guarnigione_Max { get; set; }
            public string Quantità { get; set; }
            public string Descrizione { get; set; }
            public string DescrizioneB { get; set; }
            public string Riparazione { get; set; }

            public string Costo_Cibo { get; set; }
            public string Costo_Legna { get; set; }
            public string Costo_Pietra { get; set; }
            public string Costo_Ferro { get; set; }
            public string Costo_Oro { get; set; }

            public string Costo_Spade { get; set; }
            public string Costo_Lance { get; set; }
            public string Costo_Archi { get; set; }
            public string Costo_Scudi { get; set; }
            public string Costo_Armature { get; set; }
            public string Consumo_Frecce { get; set; }

            public string Mantenimento_Cibo { get; set; }
            public string Mantenimento_Oro { get; set; }

            public string Strutture_Cibo { get; set; }
            public string Strutture_Legna { get; set; }
            public string Strutture_Pietra { get; set; }
            public string Strutture_Ferro { get; set; }
            public string Strutture_Oro { get; set; }

            public string Cibo { get; set; }
            public string Legna { get; set; }
            public string Pietra { get; set; }
            public string Ferro { get; set; }
            public string Oro { get; set; }
            public string Popolazione { get; set; }

            public string Diamond_Blu { get; set; }
            public string Diamond_Viola { get; set; }
            public string Virtual_Dolla { get; set; }

            public string Spade { get; set; }
            public string Lance { get; set; }
            public string Archi { get; set; }
            public string Scudi { get; set; }
            public string Armature { get; set; }
            public string Frecce { get; set; }

            //Produzione
            public string Cibo_s { get; set; }
            public string Legna_s { get; set; }
            public string Pietra_s { get; set; }
            public string Ferro_s { get; set; }
            public string Oro_s { get; set; }
            public string Popolazione_s { get; set; }

            public string Spade_s { get; set; }
            public string Lance_s { get; set; }
            public string Archi_s { get; set; }
            public string Scudi_s { get; set; }
            public string Armature_s { get; set; }
            public string Frecce_s { get; set; }

            // Limite risorse
            public string Cibo_Limite { get; set; }
            public string Legna_Limite { get; set; }
            public string Pietra_Limite { get; set; }
            public string Ferro_Limite { get; set; }
            public string Oro_Limite { get; set; }
            public string Popolazione_Limite { get; set; }

            public string Spade_Limite { get; set; }
            public string Lance_Limite { get; set; }
            public string Archi_Limite { get; set; }
            public string Scudi_Limite { get; set; }
            public string Armature_Limite { get; set; }
            public string Frecce_Limite { get; set; }

            public int Guerrieri_1 { get; set; }
            public int Guerrieri_2 { get; set; }
            public int Guerrieri_3 { get; set; }
            public int Guerrieri_4 { get; set; }
            public int Guerrieri_5 { get; set; }

            public int Lanceri_1 { get; set; }
            public int Lanceri_2 { get; set; }
            public int Lanceri_3 { get; set; }
            public int Lanceri_4 { get; set; }
            public int Lanceri_5 { get; set; }

            public int Arceri_1 { get; set; }
            public int Arceri_2 { get; set; }
            public int Arceri_3 { get; set; }
            public int Arceri_4 { get; set; }
            public int Arceri_5 { get; set; }

            public int Catapulte_1 { get; set; }
            public int Catapulte_2 { get; set; }
            public int Catapulte_3 { get; set; }
            public int Catapulte_4 { get; set; }
            public int Catapulte_5 { get; set; }

            public int Guerrieri_Max { get; set; }
            public int Lanceri_Max { get; set; }
            public int Arceri_Max { get; set; }
            public int Catapulte_Max { get; set; }

            // Statistiche
            #region Stats

            public string Potenza_Totale { get; set; }
            public string Potenza_Edifici { get; set; }
            public string Potenza_Ricerca { get; set; }
            public string Potenza_Esercito { get; set; }

            public string Unità_Eliminate { get; set; }
            public string Guerrieri_Eliminate { get; set; }
            public string Lanceri_Eliminate { get; set; }
            public string Arceri_Eliminate { get; set; }
            public string Catapulte_Eliminate { get; set; }

            public string Unità_Perse { get; set; }
            public string Guerrieri_Persi { get; set; }
            public string Lanceri_Persi { get; set; }
            public string Arceri_Persi { get; set; }
            public string Catapulte_Perse { get; set; }
            public string Risorse_Razziate { get; set; }

            public string Strutture_Civili_Costruite { get; set; }
            public string Strutture_Militari_Costruite { get; set; }
            public string Caserme_Costruite { get; set; }

            public string Frecce_Utilizzate { get; set; }
            public string Battaglie_Vinte { get; set; }
            public string Battaglie_Perse { get; set; }
            public string Quest_Completate { get; set; }
            public string Attacchi_Subiti_PVP { get; set; }
            public string Attacchi_Effettuati_PVP { get; set; }

            public string Barbari_Sconfitti { get; set; } //Totale uomini barbari sconfitti (villaggi e città)
            public string Accampamenti_Barbari_Sconfitti { get; set; } //Villaggi barbari sconfitti
            public string Città_Barbare_Sconfitte { get; set; }
            public string Danno_HP_Barbaro { get; set; }
            public string Danno_DEF_Barbaro { get; set; }

            public string Unità_Addestrate { get; set; }
            public string Risorse_Utilizzate { get; set; }
            public string Tempo_Addestramento_Totale { get; set; }
            public string Tempo_Costruzione_Totale { get; set; }
            public string Tempo_Ricerca_Totale { get; set; }
            public string Tempo_Sottratto_Diamanti { get; set; } //Tempo risparmiato usando diamanti

            public string Consumo_Cibo_Esercito { get; set; }
            public string Consumo_Oro_Esercito { get; set; }
                   
            public string Diamanti_Viola_Utilizzati { get; set; }
            public string Diamanti_Blu_Utilizzati { get; set; }
            #endregion

            // Bonus
            public string Bonus_Salute_Guerrieri { get; set; }
            public string Bonus_Salute_Lanceri { get; set; }
            public string Bonus_Salute_Arceri { get; set; }
            public string Bonus_Salute_Catapulte { get; set; }
            public string Bonus_Difesa_Guerrieri { get; set; }
            public string Bonus_Difesa_Lanceri { get; set; }
            public string Bonus_Difesa_Arceri { get; set; }
            public string Bonus_Difesa_Catapulte { get; set; }
            public string Bonus_Attacco_Guerrieri { get; set; }
            public string Bonus_Attacco_Lanceri { get; set; }
            public string Bonus_Attacco_Arceri { get; set; }
            public string Bonus_Attacco_Catapulte { get; set; }

            public string Bonus_Salute_Strutture { get; set; }
            public string Bonus_Difesa_Strutture { get; set; }
            public string Bonus_Guarnigione_Strutture { get; set; }

            public string Bonus_Produzione_Risorse { get; set; }
            public string Bonus_Capacità_Trasporto { get; set; }
            public string Bonus_Costruzione { get; set; }
            public string Bonus_Addestramento { get; set; }
            public string Bonus_Ricerca { get; set; }
            public string Bonus_Riparazione { get; set; }
        }
        public class Bonus_Ricerca
        {
            public string Ricerca_Produzione { get; set; }
            public string Ricerca_Costruzione { get; set; }
            public string Ricerca_Addestramento { get; set; }
            public string Ricerca_Popolazione { get; set; }
            public string Ricerca_Trasporto { get; set; }
            public string Ricerca_Riparazione { get; set; }
            public string Ricerca_Spionaggio { get; set; }
            public string Ricerca_ControSpionaggio { get; set; }

            public string Bonus_Esperienza { get; set; }
            public string Bonus_Salute_Spadaccini { get; set; }
            public string Bonus_Difesa_Spadaccini { get; set; }
            public string Bonus_Attacco_Spadaccini { get; set; }
            public string Bonus_Salute_Lanceri { get; set; }
            public string Bonus_Difesa_Lanceri { get; set; }
            public string Bonus_Attacco_Lanceri { get; set; }
            public string Bonus_Salute_Arcieri { get; set; }
            public string Bonus_Difesa_Arcieri { get; set; }
            public string Bonus_Attacco_Arcieri { get; set; }
            public string Bonus_Salute_Catapulte { get; set; }
            public string Bonus_Difesa_Catapulte { get; set; }
            public string Bonus_Attacco_Catapulte { get; set; }

            public string Livello_Spadaccini { get; set; }
            public string Salute_Spadaccini { get; set; }
            public string Difesa_Spadaccini { get; set; }
            public string Attacco_Spadaccini { get; set; }

            public string Livello_Lanceri { get; set; }
            public string Salute_Lanceri { get; set; }
            public string Difesa_Lanceri { get; set; }
            public string Attacco_Lanceri { get; set; }

            public string Livello_Arceri { get; set; }
            public string Salute_Arceri { get; set; }
            public string Difesa_Arceri { get; set; }
            public string Attacco_Arceri { get; set; }

            public string Livello_Catapulte { get; set; }
            public string Salute_Catapulte { get; set; }
            public string Difesa_Catapulte { get; set; }
            public string Attacco_Catapulte { get; set; }

            public string Ricerca_Ingresso_Guarnigione { get; set; }
            public string Ricerca_Citta_Guarnigione { get; set; }

            public string Ricerca_Cancello_Livello { get; set; }
            public string Ricerca_Cancello_Salute { get; set; }
            public string Ricerca_Cancello_Difesa { get; set; }
            public string Ricerca_Cancello_Guarnigione { get; set; }

            public string Ricerca_Mura_Livello { get; set; }
            public string Ricerca_Mura_Salute { get; set; }
            public string Ricerca_Mura_Difesa { get; set; }
            public string Ricerca_Mura_Guarnigione { get; set; }

            public string Ricerca_Torri_Livello { get; set; }
            public string Ricerca_Torri_Salute { get; set; }
            public string Ricerca_Torri_Difesa { get; set; }
            public string Ricerca_Torri_Guarnigione { get; set; }

            public string Ricerca_Castello_Livello { get; set; }
            public string Ricerca_Castello_Salute { get; set; }
            public string Ricerca_Castello_Difesa { get; set; }
            public string Ricerca_Castello_Guarnigione { get; set; }
        }

        public static Dati Utente = new Dati
        {
            Username = "0",
            Livello = "0",
            Esperienza = "0",
            Montly_Quest_Point = "0",
            User_Vip = false,
            User_Vip_Tempo = "0",
            User_GamePass_Base = false,
            User_GamePass_Base_Tempo = "0",
            User_GamePass_Avanzato = false,
            User_GamePass_Avanzato_Tempo = "0",
            Montly_Quest_Tempo = "0",
            Barbari_Tempo = "0",
            User_Login = false,
            Code_Costruzione = "0",
            Code_Reclutamento = "0",
            Code_Costruzione_Disponibili = "0",
            Code_Reclutamento_Disponibili = "0",
            Tempo_Costruzione = "0",
            Tempo_Reclutamento = "0",
            Tempo_Ricerca = "0",
            Costo_terreni_Virtuali = "0"
        };
        public static Dati Utente_Risorse = new Dati
        {
            Cibo = "0",
            Legna = "0",
            Pietra = "0",
            Ferro = "0",
            Oro = "0",
            Popolazione = "0",

            Spade = "0",
            Lance = "0",
            Archi = "0",
            Scudi = "0",
            Armature = "0",
            Frecce = "0",

            Cibo_s = "0",
            Legna_s = "0",
            Pietra_s = "0",
            Ferro_s = "0",
            Oro_s = "0",
            Popolazione_s = "0",

            Spade_s = "0",
            Lance_s = "0",
            Archi_s = "0",
            Scudi_s = "0",
            Armature_s = "0",
            Frecce_s = "0",

            Cibo_Limite = "0",
            Legna_Limite = "0",
            Pietra_Limite = "0",
            Ferro_Limite = "0",
            Oro_Limite = "0",
            Popolazione_Limite = "0",

            Spade_Limite = "0",
            Lance_Limite = "0",
            Archi_Limite = "0",
            Scudi_Limite = "0",
            Armature_Limite = "0",
            Frecce_Limite = "0",

            Mantenimento_Cibo = "0",
            Mantenimento_Oro = "0",

            Strutture_Cibo = "0",
            Strutture_Legna = "0",
            Strutture_Pietra = "0",
            Strutture_Ferro = "0",
            Strutture_Oro = "0",

            Guerrieri_Max = 0,
            Lanceri_Max = 0,
            Arceri_Max = 0,
            Catapulte_Max = 0,

            Virtual_Dolla = "0",
            Diamond_Viola = "0",
            Diamond_Blu = "0"

        };
        public static Bonus_Ricerca Utente_Bonus = new Bonus_Ricerca
        {
            Bonus_Esperienza = "0",
            Bonus_Salute_Spadaccini = "0",
            Bonus_Difesa_Spadaccini = "0",
            Bonus_Attacco_Spadaccini = "0",

            Bonus_Salute_Lanceri = "0",
            Bonus_Difesa_Lanceri = "0",
            Bonus_Attacco_Lanceri = "0",

            Bonus_Salute_Arcieri = "0",
            Bonus_Difesa_Arcieri = "0",
            Bonus_Attacco_Arcieri = "0",

            Bonus_Salute_Catapulte = "0",
            Bonus_Difesa_Catapulte = "0",
            Bonus_Attacco_Catapulte = "0"
        };
        public static Bonus_Ricerca Utente_Ricerca = new Bonus_Ricerca
        {
            Ricerca_Produzione = "0",
            Ricerca_Costruzione = "0",
            Ricerca_Addestramento = "0",
            Ricerca_Popolazione = "0",
            Ricerca_Trasporto = "0",
            Ricerca_Riparazione = "0",
            Ricerca_Spionaggio = "0",
            Ricerca_ControSpionaggio = "0",

            Ricerca_Ingresso_Guarnigione = "0",
            Ricerca_Citta_Guarnigione = "0",

            Ricerca_Cancello_Livello = "0",
            Ricerca_Cancello_Salute = "0",
            Ricerca_Cancello_Difesa = "0",
            Ricerca_Cancello_Guarnigione = "0",

            Ricerca_Mura_Livello = "0",
            Ricerca_Mura_Salute = "0",
            Ricerca_Mura_Difesa = "0",
            Ricerca_Mura_Guarnigione = "0",

            Ricerca_Torri_Livello = "0",
            Ricerca_Torri_Salute = "0",
            Ricerca_Torri_Difesa = "0",
            Ricerca_Torri_Guarnigione = "0",

            Ricerca_Castello_Livello = "0",
            Ricerca_Castello_Salute = "0",
            Ricerca_Castello_Difesa = "0",
            Ricerca_Castello_Guarnigione = "0",

            Livello_Spadaccini = "0",
            Salute_Spadaccini = "0",
            Difesa_Spadaccini = "0",
            Attacco_Spadaccini = "0",

            Livello_Lanceri = "0",
            Salute_Lanceri = "0",
            Difesa_Lanceri = "0",
            Attacco_Lanceri = "0",

            Livello_Arceri = "0",
            Salute_Arceri = "0",
            Difesa_Arceri = "0",
            Attacco_Arceri = "0",

            Livello_Catapulte = "0",
            Salute_Catapulte = "0",
            Difesa_Catapulte = "0",
            Attacco_Catapulte = "0"
        };
        public static Dati Statistiche = new Dati
        {
            Potenza_Totale = "0",
            Potenza_Edifici = "0",
            Potenza_Ricerca = "0",
            Potenza_Esercito = "0",

            Tempo_Sottratto_Diamanti = "0",
            Diamanti_Viola_Utilizzati = "0",
            Diamanti_Blu_Utilizzati = "0",
            Unità_Eliminate = "0",
            Guerrieri_Eliminate = "0",
            Lanceri_Eliminate = "0",
            Arceri_Eliminate = "0",
            Catapulte_Eliminate = "0",
            Unità_Perse = "0",
            Guerrieri_Persi = "0",
            Lanceri_Persi = "0",
            Arceri_Persi = "0",
            Catapulte_Perse = "0",
            Risorse_Razziate = "0",
            Strutture_Civili_Costruite = "0",
            Strutture_Militari_Costruite = "0",
            Caserme_Costruite = "0",
            Frecce_Utilizzate = "0",
            Battaglie_Vinte = "0",
            Battaglie_Perse = "0",
            Quest_Completate = "0",
            Attacchi_Subiti_PVP = "0",
            Attacchi_Effettuati_PVP = "0",
            Barbari_Sconfitti = "0",
            Accampamenti_Barbari_Sconfitti = "0",
            Città_Barbare_Sconfitte = "0",
            Danno_HP_Barbaro = "0",
            Danno_DEF_Barbaro = "0",
            Unità_Addestrate = "0",
            Risorse_Utilizzate = "0",
            Tempo_Addestramento_Totale = "0",
            Tempo_Costruzione_Totale = "0",
            Tempo_Ricerca_Totale = "0"
        };
        public static Dati Bonus = new Dati
        {
        Bonus_Salute_Guerrieri = "0",
        Bonus_Salute_Lanceri = "0",
        Bonus_Salute_Arceri = "0",
        Bonus_Salute_Catapulte = "0",
        Bonus_Difesa_Guerrieri = "0",
        Bonus_Difesa_Lanceri = "0",
        Bonus_Difesa_Arceri = "0",
        Bonus_Difesa_Catapulte = "0",
        Bonus_Attacco_Guerrieri = "0",
        Bonus_Attacco_Lanceri = "0",
        Bonus_Attacco_Arceri = "0",
        Bonus_Attacco_Catapulte = "0",

        Bonus_Salute_Strutture = "0",
        Bonus_Difesa_Strutture = "0",
        Bonus_Guarnigione_Strutture = "0",

        Bonus_Produzione_Risorse = "0",
        Bonus_Capacità_Trasporto = "0",
        Bonus_Costruzione = "0",
        Bonus_Addestramento = "0",
        Bonus_Ricerca = "0",
        Bonus_Riparazione = "0"
    };

        public class Shop
        {
            public string Costo { get; set; }
            public string Reward { get; set; }
            public string desc { get; set; }

            public static Shop Vip_1 = new Shop
            {
                Costo = "500", //Diamanti_Viola
                Reward = "24", //VIP
                desc = ""
            };
            public static Shop Vip_2 = new Shop
            {
                Costo = "14.99", //USDT
                Reward = "24", //VIP
                desc = ""
            };

            public static Shop GamePass_Base = new Shop
            {
                Costo = "14.99", //USDT
                Reward = "1", //VIP
                desc = ""
            };
            public static Shop GamePass_Avanzato = new Shop
            {
                Costo = "14.99", //USDT
                Reward = "1", //VIP
                desc = ""
            };

            public static Shop Pacchetto_Diamanti_1 = new Shop
            {
                Costo = "5.99", //USDT
                Reward = "150", //Diamanti_Viola
                desc = ""
            };
            public static Shop Pacchetto_Diamanti_2 = new Shop
            {
                Costo = "14.99",
                Reward = "475",
                desc = ""
            };
            public static Shop Pacchetto_Diamanti_3 = new Shop
            {
                Costo = "24.99",
                Reward = "800",
                desc = ""
            };
            public static Shop Pacchetto_Diamanti_4 = new Shop
            {
                Costo = "49.99",
                Reward = "1500",
                desc = ""
            };
            public static Shop Scudo_Pace_8h = new Shop
            {
                Costo = "0",
                Reward = "0", //8 ore in secondi
                desc = ""
            };
            public static Shop Scudo_Pace_24h = new Shop
            {
                Costo = "0",
                Reward = "0", //24 ore in secondi
                desc = ""
            };
            public static Shop Scudo_Pace_72h = new Shop
            {
                Costo = "0",
                Reward = "0", //72 ore in secondi
                desc = ""
            };

            public static Shop Costruttore_24h = new Shop
            {
                Costo = "0",
                Reward = "0", //24 ore in secondi
                desc = ""
            };
            public static Shop Costruttore_48h = new Shop
            {
                Costo = "0",
                Reward = "0", //48 ore in secondi
                desc = ""
            };

            public static Shop Reclutatore_24h = new Shop
            {
                Costo = "0",
                Reward = "0", //24 ore in secondi
                desc = ""
            };
            public static Shop Reclutatore_48h = new Shop
            {
                Costo = "1120",
                Reward = "48", //48 ore in secondi
                desc = ""
            };
        }
        
        public class Citta
        {
            public static Dati Castello = new Dati
            {
                Guerrieri_1 = 0,
                Guerrieri_2 = 0,
                Guerrieri_3 = 0,
                Guerrieri_4 = 0,
                Guerrieri_5 = 0,
                Lanceri_1 = 0,
                Lanceri_2 = 0,
                Lanceri_3 = 0,
                Lanceri_4 = 0,
                Lanceri_5 = 0,
                Arceri_1 = 0,
                Arceri_2 = 0,
                Arceri_3 = 0,
                Arceri_4 = 0,
                Arceri_5 = 0,
                Catapulte_1 = 0,
                Catapulte_2 = 0,
                Catapulte_3 = 0,
                Catapulte_4 = 0,
                Catapulte_5 = 0,

                Livello = "0",
                Salute = 0,
                Salute_Max = 0,
                Difesa = 0,
                Difesa_Max = 0,
                Guarnigione = 0,
                Guarnigione_Max = 0,
                Riparazione = "0",
                Descrizione = "",
                DescrizioneB = ""
            };
            public static Dati Torri = new Dati
            {
                Guerrieri_1 = 0,
                Guerrieri_2 = 0,
                Guerrieri_3 = 0,
                Guerrieri_4 = 0,
                Guerrieri_5 = 0,
                Lanceri_1 = 0,
                Lanceri_2 = 0,
                Lanceri_3 = 0,
                Lanceri_4 = 0,
                Lanceri_5 = 0,
                Arceri_1 = 0,
                Arceri_2 = 0,
                Arceri_3 = 0,
                Arceri_4 = 0,
                Arceri_5 = 0,
                Catapulte_1 = 0,
                Catapulte_2 = 0,
                Catapulte_3 = 0,
                Catapulte_4 = 0,
                Catapulte_5 = 0,

                Livello = "0",
                Salute = 0,
                Salute_Max = 0,
                Difesa = 0,
                Difesa_Max = 0,
                Guarnigione = 0,
                Guarnigione_Max = 0,
                Riparazione = "0",
                Descrizione = "",
                DescrizioneB = ""
            };
            public static Dati Cancello = new Dati
            {
                Guerrieri_1 = 0,
                Guerrieri_2 = 0,
                Guerrieri_3 = 0,
                Guerrieri_4 = 0,
                Guerrieri_5 = 0,
                Lanceri_1 = 0,
                Lanceri_2 = 0,
                Lanceri_3 = 0,
                Lanceri_4 = 0,
                Lanceri_5 = 0,
                Arceri_1 = 0,
                Arceri_2 = 0,
                Arceri_3 = 0,
                Arceri_4 = 0,
                Arceri_5 = 0,
                Catapulte_1 = 0,
                Catapulte_2 = 0,
                Catapulte_3 = 0,
                Catapulte_4 = 0,
                Catapulte_5 = 0,

                Livello = "0",
                Salute = 0,
                Salute_Max = 0,
                Difesa = 0,
                Difesa_Max = 0,
                Guarnigione = 0,
                Guarnigione_Max = 0,
                Riparazione = "0",
                Descrizione = "",
                DescrizioneB = ""
            };
            public static Dati Mura = new Dati
            {
                Guerrieri_1 = 0,
                Guerrieri_2 = 0,
                Guerrieri_3 = 0,
                Guerrieri_4 = 0,
                Guerrieri_5 = 0,
                Lanceri_1 = 0,
                Lanceri_2 = 0,
                Lanceri_3 = 0,
                Lanceri_4 = 0,
                Lanceri_5 = 0,
                Arceri_1 = 0,
                Arceri_2 = 0,
                Arceri_3 = 0,
                Arceri_4 = 0,
                Arceri_5 = 0,
                Catapulte_1 = 0,
                Catapulte_2 = 0,
                Catapulte_3 = 0,
                Catapulte_4 = 0,
                Catapulte_5 = 0,

                Livello = "0",
                Salute = 0,
                Salute_Max = 0,
                Difesa = 0,
                Difesa_Max = 0,
                Guarnigione = 0,
                Guarnigione_Max = 0,
                Riparazione = "0",
                Descrizione = "",
                DescrizioneB = ""
            };
            public static Dati Ingresso = new Dati
            {
                Guerrieri_1 = 0,
                Guerrieri_2 = 0,
                Guerrieri_3 = 0,
                Guerrieri_4 = 0,
                Guerrieri_5 = 0,
                Lanceri_1 = 0,
                Lanceri_2 = 0,
                Lanceri_3 = 0,
                Lanceri_4 = 0,
                Lanceri_5 = 0,
                Arceri_1 = 0,
                Arceri_2 = 0,
                Arceri_3 = 0,
                Arceri_4 = 0,
                Arceri_5 = 0,
                Catapulte_1 = 0,
                Catapulte_2 = 0,
                Catapulte_3 = 0,
                Catapulte_4 = 0,
                Catapulte_5 = 0,

                Livello = "0",
                Guarnigione = 0,
                Guarnigione_Max = 0,
                Descrizione = ""
            };
            public static Dati Città = new Dati
            {
                Guerrieri_1 = 0,
                Guerrieri_2 = 0,
                Guerrieri_3 = 0,
                Guerrieri_4 = 0,
                Guerrieri_5 = 0,
                Lanceri_1 = 0,
                Lanceri_2 = 0,
                Lanceri_3 = 0,
                Lanceri_4 = 0,
                Lanceri_5 = 0,
                Arceri_1 = 0,
                Arceri_2 = 0,
                Arceri_3 = 0,
                Arceri_4 = 0,
                Arceri_5 = 0,
                Catapulte_1 = 0,
                Catapulte_2 = 0,
                Catapulte_3 = 0,
                Catapulte_4 = 0,
                Catapulte_5 = 0,

                Livello = "0",
                Guarnigione = 0,
                Guarnigione_Max = 0,
                Descrizione = ""
            };
        }
        public class Reclutamento
        {
            public static Dati Guerrieri_Max = new Dati
            {
                Quantità = "0"
            };
            public static Dati Lanceri_Max = new Dati
            {
                Quantità = "0"
            };
            public static Dati Arceri_Max = new Dati
            {
                Quantità = "0"
            };
            public static Dati Catapulte_Max = new Dati
            {
                Quantità = "0"
            };
            public static Dati Guerrieri_1 = new Dati
            {
                Descrizione = "",
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Lanceri_1 = new Dati
            {
                Descrizione = "",
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Arceri_1 = new Dati
            {
                Descrizione = "",
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Catapulte_1 = new Dati
            {
                Descrizione = "",
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };

            public static Dati Guerrieri_2 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Lanceri_2 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Arceri_2 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Catapulte_2 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };

            public static Dati Guerrieri_3 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Lanceri_3 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Arceri_3 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Catapulte_3 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };

            public static Dati Guerrieri_4 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Lanceri_4 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Arceri_4 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Catapulte_4 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };

            public static Dati Guerrieri_5 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Lanceri_5 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Arceri_5 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
            public static Dati Catapulte_5 = new Dati
            {
                Livello = "0",
                Salute = 0,
                Difesa = 0,
                Quantità = "0",

                Costo_Cibo = "0",
                Costo_Legna = "0",
                Costo_Pietra = "0",
                Costo_Ferro = "0",
                Costo_Oro = "0",

                Costo_Spade = "0",
                Costo_Lance = "0",
                Costo_Archi = "0",
                Costo_Scudi = "0",
                Costo_Armature = "0",
                Consumo_Frecce = "0",

                Mantenimento_Cibo = "0",
                Mantenimento_Oro = "0"
            };
        }
        public class Reclutamento_Coda
        {
            public static Dati Guerrieri_1 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Lanceri_1 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Arceri_1 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Catapulte_1 = new Dati
            {
                Quantità = "0"
            };

            public static Dati Guerrieri_2 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Lanceri_2 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Arceri_2 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Catapulte_2 = new Dati
            {
                Quantità = "0"
            };

            public static Dati Guerrieri_3 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Lanceri_3 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Arceri_3 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Catapulte_3 = new Dati
            {
                Quantità = "0"
            };

            public static Dati Guerrieri_4 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Lanceri_4 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Arceri_4 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Catapulte_4 = new Dati
            {
                Quantità = "0"
            };

            public static Dati Guerrieri_5 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Lanceri_5 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Arceri_5 = new Dati
            {
                Quantità = "0"
            };
            public static Dati Catapulte_5 = new Dati
            {
                Quantità = "0"
            };
        }
        public class Costruzione
        {
            public static Dati Fattorie = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati Segherie = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati CaveDiPietra = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati Miniera_Ferro = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };

            public static Dati Miniera_Oro = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati Case = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };

            public static Dati Workshop_Spade = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati Workshop_Lance = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati Workshop_Archi = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati Workshop_Scudi = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };

            public static Dati Workshop_Armature = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati Workshop_Frecce = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };

            public static Dati Caserme_Guerrieri = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati Caserme_Lanceri = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati Caserme_arceri = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };

            public static Dati Caserme_Catapulte = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
        }
        public class Costruzione_Coda
        {
            public static Dati Fattorie = new Dati
            {
                Quantità = "0"
            };
            public static Dati Segherie = new Dati
            {
                Quantità = "0"
            };
            public static Dati CaveDiPietra = new Dati
            {
                Quantità = "0"
            };
            public static Dati Miniera_Ferro = new Dati
            {
                Quantità = "0"
            };

            public static Dati Miniera_Oro = new Dati
            {
                Quantità = "0"
            };
            public static Dati Case = new Dati
            {
                Quantità = "0"
            };

            public static Dati Workshop_Spade = new Dati
            {
                Quantità = "0"
            };
            public static Dati Workshop_Lance = new Dati
            {
                Quantità = "0"
            };
            public static Dati Workshop_Archi = new Dati
            {
                Quantità = "0"
            };
            public static Dati Workshop_Scudi = new Dati
            {
                Quantità = "0"
            };

            public static Dati Workshop_Armature = new Dati
            {
                Quantità = "0"
            };
            public static Dati Workshop_Frecce = new Dati
            {
                Quantità = "0"
            };

            public static Dati Caserme_Guerrieri = new Dati
            {
                Quantità = "0"
            };
            public static Dati Caserme_Lanceri = new Dati
            {
                Quantità = "0"
            };
            public static Dati Caserme_arceri = new Dati
            {
                Quantità = "0"
            };

            public static Dati Caserme_Catapulte = new Dati
            {
                Quantità = "0"
            };
        }
        public class Terreni_Virtuali
        {
            public static Dati Comune = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati NonComune = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati Raro = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
            public static Dati Epico = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };

            public static Dati Leggendario = new Dati
            {
                Quantità = "0",
                Descrizione = ""
            };
        }

        public class AttaccoInfo
        {
            public string Creatore { get; set; }
            public string ID { get; set; }
            public int TempoRimanente { get; set; }
            
            public override string ToString()
            {
                return $"ID: {ID} - Creato da: {Creatore}  - {TempoRimanente} min";
            }
            
            public static AttaccoInfo FromString(string data)
            {
                string[] parts = data.Replace(" ", "").Split('-');
                if (parts.Length >= 3)
                {
                    return new AttaccoInfo
                    {
                        Creatore = parts[0],
                        ID = parts[1],
                        TempoRimanente = int.Parse(parts[2])
                    };
                }
                return null;
            }
        }
        public class PartecipanteAttacco
        {
            public string Giocatore { get; set; }
            public string ID { get; set; }
            public string Guerrieri_1 { get; set; }
            public string Lanceri_1 { get; set; }
            public string Arceri_1 { get; set; }
            public string Catapulte_1 { get; set; }
            public int TempoRimanente { get; set; }

            public override string ToString()
            {
                return $"ID: {ID} - Creato da: {Giocatore}  - {TempoRimanente} min";
            }

            public static PartecipanteAttacco FromString(string data)
            {
                string[] parts = data.Replace(" ", "").Split('-');
                if (parts.Length >= 3)
                {
                    return new PartecipanteAttacco
                    {
                        //"adlos - 30e2fb60 - 29 - 1 - 1 - 1 - 1"
                        Giocatore = parts[0],
                        ID = parts[1],
                        TempoRimanente = int.Parse(parts[2]),
                        Guerrieri_1 = parts[3],
                        Lanceri_1 = parts[4],
                        Arceri_1 = parts[5],
                        Catapulte_1 = parts[6]
                    };
                }
                return null;
            }
        }
        public class AttaccoPartecipazione
        {
            public string Creatore { get; set; }
            public string ID { get; set; }
            public int NumPartecipanti { get; set; }
            public int MieiGuerrieri { get; set; }
            public int MieiLancieri { get; set; }
            public int MieiArcieri { get; set; }
            public int MieiCatapulte { get; set; }
            public int TempoRimanente { get; set; }
        }

    }
}
