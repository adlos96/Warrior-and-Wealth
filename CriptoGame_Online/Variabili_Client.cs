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

        public static string Ricerca_1_Bottone_Cliccato = "";
        public static string Server = "0";
        public static string Versione = "0";

        public static string Esperienza_Desc = "0";
        public static string Livello_Desc = "0";


        public static List<string> Giocatori_PVP = new List<string>();
        public static List<string> Raduni_Creati = new List<string>(); //Raduni pubblici
        public static List<string> Raduni_InCorso = new List<string>(); //Raduni a cui si partecipa inviando truppe

        public static List<VillaggioClient> VillaggiPersonali = new();
        public static List<VillaggioClient> CittaGlobali = new();

        public class Dati
        {
            public string Costo_terreni_Virtuali { get; set; }

            public string Username { get; set; }
            public string Password { get; set; }
            public string Montly_Quest_Point { get; set; }
            public bool User_Vip { get; set; }
            public bool Ricerca_Attiva { get; set; }
            public bool User_Login { get; set; }

            public bool Scudo_Pace { get; set; }
            public string Code_Costruzione { get; set; }
            public string Code_Reclutamento { get; set; }
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

        }
        public class Bonus_Ricerca
        {
            public string Ricerca_Produzione { get; set; }
            public string Ricerca_Costruzione { get; set; }
            public string Ricerca_Addestramento { get; set; }
            public string Ricerca_Popolazione { get; set; }

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
                   
            public string Ricerca_Cancello_Salute { get; set; }
            public string Ricerca_Cancello_Difesa { get; set; }
            public string Ricerca_Cancello_Guarnigione { get; set; }
                   
            public string Ricerca_Mura_Salute { get; set; }
            public string Ricerca_Mura_Difesa { get; set; }
            public string Ricerca_Mura_Guarnigione { get; set; }
                   
            public string Ricerca_Torri_Salute { get; set; }
            public string Ricerca_Torri_Difesa { get; set; }
            public string Ricerca_Torri_Guarnigione { get; set; }
                   
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
            User_Login = false,
            Code_Costruzione = "0",
            Code_Reclutamento = "0",
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

            Ricerca_Ingresso_Guarnigione = "0",
            Ricerca_Citta_Guarnigione = "0",

            Ricerca_Cancello_Salute = "0",
            Ricerca_Cancello_Difesa = "0",
            Ricerca_Cancello_Guarnigione = "0",

            Ricerca_Mura_Salute = "0",
            Ricerca_Mura_Difesa = "0",
            Ricerca_Mura_Guarnigione = "0",

            Ricerca_Torri_Salute = "0",
            Ricerca_Torri_Difesa = "0",
            Ricerca_Torri_Guarnigione = "0",

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

        public class Shop
        {
            public string Costo { get; set; }
            public string Reward { get; set; }

            public static Shop Vip_1 = new Shop
            {
                Costo = "0",
                Reward = "0"
            };
            public static Shop Vip_2 = new Shop
            {
                Costo = "0",
                Reward = "0"
            };

            public static Shop Pacchetto_1 = new Shop
            {
                Costo = "0",
                Reward = "0"
            };
            public static Shop Pacchetto_2 = new Shop
            {
                Costo = "0",
                Reward = "0"
            };
            public static Shop Pacchetto_3 = new Shop
            {
                Costo = "0",
                Reward = "0"
            };
            public static Shop Pacchetto_4 = new Shop
            {
                Costo = "0",
                Reward = "0"
            };


        }
        public class Quest_Reward
        {
            #region Reward
            public string Reward_1 { get; set; }
            public string Reward_2 { get; set; }
            public string Reward_3 { get; set; }
            public string Reward_4 { get; set; }
            public string Reward_5 { get; set; }
            public string Reward_6 { get; set; }
            public string Reward_7 { get; set; }
            public string Reward_8 { get; set; }
            public string Reward_9 { get; set; }
            public string Reward_10 { get; set; }
            public string Reward_11{ get; set; }
            public string Reward_12 { get; set; }
            public string Reward_13 { get; set; }
            public string Reward_14 { get; set; }
            public string Reward_15 { get; set; }
            public string Reward_16 { get; set; }
            public string Reward_17 { get; set; }
            public string Reward_18 { get; set; }
            public string Reward_19 { get; set; }
            public string Reward_20 { get; set; }
            #endregion
            #region Punti reward
            public string Points_1 { get; set; }
            public string Points_2 { get; set; }
            public string Points_3 { get; set; }
            public string Points_4 { get; set; }
            public string Points_5 { get; set; }
            public string Points_6 { get; set; }
            public string Points_7 { get; set; }
            public string Points_8 { get; set; }
            public string Points_9 { get; set; }
            public string Points_10 { get; set; }
            public string Points_11 { get; set; }
            public string Points_12 { get; set; }
            public string Points_13 { get; set; }
            public string Points_14 { get; set; }
            public string Points_15 { get; set; }
            public string Points_16 { get; set; }
            public string Points_17 { get; set; }
            public string Points_18 { get; set; }
            public string Points_19 { get; set; }
            public string Points_20 { get; set; }
            #endregion
            #region Reward Claimed
            public bool Reward_Claim_1 { get; set; }
            public bool Reward_Claim_2 { get; set; }
            public bool Reward_Claim_3 { get; set; }
            public bool Reward_Claim_4 { get; set; }
            public bool Reward_Claim_5 { get; set; }
            public bool Reward_Claim_6 { get; set; }
            public bool Reward_Claim_7 { get; set; }
            public bool Reward_Claim_8 { get; set; }
            public bool Reward_Claim_9 { get; set; }
            public bool Reward_Claim_10 { get; set; }
            public bool Reward_Claim_11 { get; set; }
            public bool Reward_Claim_12 { get; set; }
            public bool Reward_Claim_13 { get; set; }
            public bool Reward_Claim_14 { get; set; }
            public bool Reward_Claim_15 { get; set; }
            public bool Reward_Claim_16 { get; set; }
            public bool Reward_Claim_17 { get; set; }
            public bool Reward_Claim_18 { get; set; }
            public bool Reward_Claim_19 { get; set; }
            public bool Reward_Claim_20 { get; set; }
            #endregion
            public static Quest_Reward Normali_Montly = new Quest_Reward //Quantità di ricompense x livello F2P. Es 1 = 15, 2 = 20, 3 = 25
            {
                Reward_1  = "0",
                Reward_2  = "0",
                Reward_3  = "0",
                Reward_4  = "0",
                Reward_5  = "0",
                Reward_6  = "0",
                Reward_7  = "0",
                Reward_8  = "0",
                Reward_9  = "0",
                Reward_10  = "0",
                Reward_11  = "0",
                Reward_12  = "0",
                Reward_13  = "0",
                Reward_14  = "0",
                Reward_15  = "0",
                Reward_16  = "0",
                Reward_17  = "0",
                Reward_18  = "0",
                Reward_19  = "0",
                Reward_20  = "0"
            };
            public static Quest_Reward Vip_Montly = new Quest_Reward //Quantità di ricompense x livello VIP. Es 1 = 15, 2 = 20, 3 = 25
            {
                Reward_1 = "0",
                Reward_2 = "0",
                Reward_3 = "0",
                Reward_4 = "0",
                Reward_5 = "0",
                Reward_6 = "0",
                Reward_7 = "0",
                Reward_8 = "0",
                Reward_9 = "0",
                Reward_10 = "0",
                Reward_11 = "0",
                Reward_12 = "0",
                Reward_13 = "0",
                Reward_14 = "0",
                Reward_15 = "0",
                Reward_16 = "0",
                Reward_17 = "0",
                Reward_18 = "0",
                Reward_19 = "0",
                Reward_20 = "0"
            };
            public static Quest_Reward Point_Montly = new Quest_Reward //Quantità punti richiesti per livello
            {
                Points_1 = "0",
                Points_2 = "0",
                Points_3 = "0",
                Points_4 = "0",
                Points_5 = "0",
                Points_6 = "0",
                Points_7 = "0",
                Points_8 = "0",
                Points_9 = "0",
                Points_10 = "0",
                Points_11 = "0",
                Points_12 = "0",
                Points_13 = "0",
                Points_14 = "0",
                Points_15 = "0",
                Points_16 = "0",
                Points_17 = "0",
                Points_18 = "0",
                Points_19 = "0",
                Points_20 = "0"
            };
            public static Quest_Reward Montly_Claim_Normal = new Quest_Reward //Quantità di ricompense x livello F2P. Es 1 = 15, 2 = 20, 3 = 25
            {
                Reward_Claim_1 = false,
                Reward_Claim_2 = false,
                Reward_Claim_3 = false,
                Reward_Claim_4 = false,
                Reward_Claim_5 = false,
                Reward_Claim_6 = false,
                Reward_Claim_7 = false,
                Reward_Claim_8 = false,
                Reward_Claim_9 = false,
                Reward_Claim_10 = false,
                Reward_Claim_11 = false,
                Reward_Claim_12 = false,
                Reward_Claim_13 = false,
                Reward_Claim_14 = false,
                Reward_Claim_15 = false,
                Reward_Claim_16 = false,
                Reward_Claim_17 = false,
                Reward_Claim_18 = false,
                Reward_Claim_19 = false,
                Reward_Claim_20 = false
            };
            public static Quest_Reward Montly_Claim_Vip = new Quest_Reward //Quantità di ricompense x livello F2P. Es 1 = 15, 2 = 20, 3 = 25
            {
                Reward_Claim_1 = false,
                Reward_Claim_2 = false,
                Reward_Claim_3 = false,
                Reward_Claim_4 = false,
                Reward_Claim_5 = false,
                Reward_Claim_6 = false,
                Reward_Claim_7 = false,
                Reward_Claim_8 = false,
                Reward_Claim_9 = false,
                Reward_Claim_10 = false,
                Reward_Claim_11 = false,
                Reward_Claim_12 = false,
                Reward_Claim_13 = false,
                Reward_Claim_14 = false,
                Reward_Claim_15 = false,
                Reward_Claim_16 = false,
                Reward_Claim_17 = false,
                Reward_Claim_18 = false,
                Reward_Claim_19 = false,
                Reward_Claim_20 = false
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
                Riparazione = "0"
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
                Riparazione = "0"
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
                Riparazione = "0"
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
                Riparazione = "0"
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
                Guarnigione_Max = 0
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
                Guarnigione_Max = 0
            };
        }
        public class Reclutamento
        {
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
                Quantità = "0"
            };
            public static Dati NonComune = new Dati
            {
                Quantità = "0"
            };
            public static Dati Raro = new Dati
            {
                Quantità = "0"
            };
            public static Dati Epico = new Dati
            {
                Quantità = "0"
            };

            public static Dati Leggendario = new Dati
            {
                Quantità = "0"
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
