using static Server_Strategico.Gioco.Esercito;
using static Server_Strategico.Gioco.Ricerca;
using static Server_Strategico.Gioco.Strutture;
using static Server_Strategico.Server.Server;

namespace Server_Strategico.Gioco
{
    internal class Ricerca
    {
        public static int Guerrieri_Riduzione = 3;
        public static int Lanceri_Riduzione = 3;
        public static int Arceri_Riduzione = 6;
        public static int Catapulte_Riduzione = 9;
        public class dati
        {
            public double Cibo { get; set; }
            public double Legno { get; set; }
            public double Pietra { get; set; }
            public double Ferro { get; set; }
            public double Oro { get; set; }
            public double Popolazione { get; set; }

            public double Salute { get; set; }
            public double Difesa { get; set; }
            public double Attacco { get; set; }
            public double Guarnigione { get; set; }

            public double Incremento { get; set; }
            public double TempoRicerca { get; set; }
        }
        public class Tipi
        {
            public static dati Costruzione = new dati
            {
                Cibo = 23000,
                Legno = 21640,
                Pietra = 20735,
                Ferro = 20159,
                Oro = 19021,
                Popolazione = 25,
                TempoRicerca = 3600
            };
            public static dati Produzione = new dati
            {
                Cibo = 28000,
                Legno = 26344,
                Pietra = 25243,
                Ferro = 24541,
                Oro = 23156,
                Popolazione = 30,
                TempoRicerca = 4500
            };
            public static dati Addestramento = new dati
            {
                Cibo = 36000,
                Legno = 33871,
                Pietra = 32455,
                Ferro = 31553,
                Oro = 29772,
                Popolazione = 30,
                TempoRicerca = 5400
            };
            public static dati Popolazione = new dati
            {
                Cibo = 42000,
                Legno = 39517,
                Pietra = 37864,
                Ferro = 36812,
                Oro = 34734,
                Popolazione = 50,
                TempoRicerca = 7200
            };
            public static dati Trasporto = new dati
            {
                Cibo = 62000,
                Legno = 58334,
                Pietra = 55894,
                Ferro = 54342,
                Oro = 51274,
                Popolazione = 40,
                TempoRicerca = 129600
            };
            public static dati Riparazione = new dati
            {
                Cibo = 93000,
                Legno = 87501,
                Pietra = 83842,
                Ferro = 81512,
                Oro = 76911,
                Popolazione = 60,
                TempoRicerca = 345600
            };

            public static CostoReclutamento Incremento = new CostoReclutamento
            {
                Cibo = 0.05,
                Legno = 0.04,
                Pietra = 0.03,
                Ferro = 0.02,
                Oro = 0.01,
                Popolazione = 0.0001, //Popolazione e frecce
                Spade = 0.001,
                Lance = 0.003,
                Archi = 0.004,
                Scudi = 0.005,
                Armature = 0.006,
                Trasporto = 20,
                Salute_Repair = 1,
                Difesa_Repair = 1
            };

        }
        public class Soldati
        {
            public static dati Salute = new dati
            {
                Cibo = 13000,
                Legno = 12231,
                Pietra = 11720,
                Ferro = 11394,
                Oro = 10751,
                Popolazione = 10,
                TempoRicerca = 12600
            };
            public static dati Difesa = new dati
            {
                Cibo = 9000,
                Legno = 8468,
                Pietra = 8114,
                Ferro = 7888,
                Oro = 7443,
                Popolazione = 15,
                TempoRicerca = 14400
            };
            public static dati Attacco = new dati
            {
                Cibo = 16000,
                Legno = 15054,
                Pietra = 14424,
                Ferro = 14024,
                Oro = 13232,
                Popolazione = 20,
                TempoRicerca = 16200
            };
            public static dati Livello = new dati
            {
                Cibo = 21000,
                Legno = 19758,
                Pietra = 18932,
                Ferro = 18406,
                Oro = 17367,
                Popolazione = 25,
                TempoRicerca = 21600
            };

            public static dati Incremento = new dati
            {
                Salute = 1,
                Difesa = 1,
                Attacco = 1
            };

        }
        public class Citta
        {
            public static dati Ingresso = new dati
            {
                Cibo = 13000,
                Legno = 12231,
                Pietra = 11720,
                Ferro = 11394,
                Oro = 10751,
                TempoRicerca = 7200,
                Popolazione = 25

            };
            public static dati Città = new dati
            {
                Cibo = 13000,
                Legno = 12750,
                Pietra = 12500,
                Ferro = 12500,
                Oro = 12250,
                TempoRicerca = 18000,
                Popolazione = 45
            };

            public static dati Mura_Livello = new dati
            {
                Cibo = 26000,
                Legno = 24463,
                Pietra = 23440,
                Ferro = 22788,
                Oro = 21502,
                TempoRicerca = 18000,
                Popolazione = 40
            };
            public static dati Mura_Salute = new dati
            {
                Cibo = 22000,
                Legno = 20699,
                Pietra = 19833,
                Ferro = 19283,
                Oro = 18194,
                TempoRicerca = 10800,
                Popolazione = 20
            };
            public static dati Mura_Difesa = new dati
            {
                Cibo = 18000,
                Legno = 16936,
                Pietra = 16227,
                Ferro = 15777,
                Oro = 14886,
                TempoRicerca = 12600,
                Popolazione = 25
            };
            public static dati Mura_Guarnigione = new dati
            {
                Cibo = 15000,
                Legno = 14113,
                Pietra = 13523,
                Ferro = 13147,
                Oro = 12405,
                TempoRicerca = 14400,
                Popolazione = 30
            };

            public static dati Cancello_Livello = new dati
            {
                Cibo = 29000,
                Legno = 27285,
                Pietra = 26144,
                Ferro = 25418,
                Oro = 23983,
                TempoRicerca = 21600,
                Popolazione = 50
            };
            public static dati Cancello_Salute = new dati
            {
                Cibo = 25000,
                Legno = 23522,
                Pietra = 22538,
                Ferro = 21912,
                Oro = 20675,
                TempoRicerca = 14400,
                Popolazione = 30
            };
            public static dati Cancello_Difesa = new dati
            {
                Cibo = 21000,
                Legno = 19758,
                Pietra = 18932,
                Ferro = 18406,
                Oro = 17367,
                TempoRicerca = 16200,
                Popolazione = 35
            };
            public static dati Cancello_Guarnigione = new dati
            {
                Cibo = 18000,
                Legno = 16936,
                Pietra = 16227,
                Ferro = 15777,
                Oro = 14886,
                TempoRicerca = 18000,
                Popolazione = 40
            };

            public static dati Torri_Livello = new dati
            {
                Cibo = 31000,
                Legno = 29167,
                Pietra = 27947,
                Ferro = 27171,
                Oro = 25637,
                TempoRicerca = 25200,
                Popolazione = 60

            };
            public static dati Torri_Salute = new dati
            {
                Cibo = 27000,
                Legno = 25403,
                Pietra = 24341,
                Ferro = 23665,
                Oro = 22329,
                TempoRicerca = 18000,
                Popolazione = 40
            };
            public static dati Torri_Difesa = new dati
            {
                Cibo = 24000,
                Legno = 22581,
                Pietra = 21637,
                Ferro = 21035,
                Oro = 19848,
                TempoRicerca = 19800,
                Popolazione = 45
            };
            public static dati Torri_Guarnigione = new dati
            {
                Cibo = 20000,
                Legno = 18817,
                Pietra = 18030,
                Ferro = 17530,
                Oro = 16540,
                TempoRicerca = 21600,
                Popolazione = 50
            };

            public static dati Castello_Livello = new dati
            {
                Cibo = 33000,
                Legno = 31049,
                Pietra = 29750,
                Ferro = 28924,
                Oro = 27291,
                TempoRicerca = 28800,
                Popolazione = 70
            };
            public static dati Castello_Salute = new dati
            {
                Cibo = 29000,
                Legno = 27285,
                Pietra = 26144,
                Ferro = 25418,
                Oro = 23983,
                TempoRicerca = 21600,
                Popolazione = 50
            };
            public static dati Castello_Difesa = new dati
            {
                Cibo = 26000,
                Legno = 24463,
                Pietra = 23440,
                Ferro = 22788,
                Oro = 21502,
                TempoRicerca = 23400,
                Popolazione = 55
            };
            public static dati Castello_Guarnigione = new dati
            {
                Cibo = 22000,
                Legno = 20699,
                Pietra = 19833,
                Ferro = 19283,
                Oro = 18194,
                TempoRicerca = 25200,
                Popolazione = 60
            };
        }
    }
}
