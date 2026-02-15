using static Server_Strategico.Gioco.Esercito;
using static Server_Strategico.Gioco.Ricerca;
using static Server_Strategico.Gioco.Strutture;
using static Server_Strategico.Server.Server;

namespace Server_Strategico.Gioco
{
    internal class Ricerca
    {
        public static int Guerrieri_Riduzione = 1;
        public static int Lanceri_Riduzione = 1;
        public static int Arceri_Riduzione = 2;
        public static int Catapulte_Riduzione = 3;
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
                Cibo = 5500,
                Legno = 5250,
                Pietra = 5000,
                Ferro = 4750,
                Oro = 4550,
                Popolazione = 25,
                TempoRicerca = 3600
            };
            public static dati Produzione = new dati
            {
                Cibo = 6500,
                Legno = 6250,
                Pietra = 6000,
                Ferro = 5750,
                Oro = 5500,
                Popolazione = 30,
                TempoRicerca = 4500
            };
            public static dati Addestramento = new dati
            {
                Cibo = 7500,
                Legno = 7000,
                Pietra = 6500,
                Ferro = 6250,
                Oro = 6000,
                Popolazione = 25,
                TempoRicerca = 5400
            };
            public static dati Popolazione = new dati
            {
                Cibo = 8500,
                Legno = 8000,
                Pietra = 7500,
                Ferro = 7250,
                Oro = 7000,
                Popolazione = 50,
                TempoRicerca = 7200
            };
            public static dati Trasporto = new dati
            {
                Cibo = 17000,
                Legno = 16000,
                Pietra = 15000,
                Ferro = 14500,
                Oro = 14250,
                Popolazione = 40,
                TempoRicerca = 129600
            };
            public static dati Riparazione = new dati
            {
                Cibo = 28500,
                Legno = 27000,
                Pietra = 26500,
                Ferro = 24750,
                Oro = 24250,
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
                Popolazione = 0.0004, //Popolazione e frecce
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
                Cibo = 6000,
                Legno = 5750,
                Pietra = 5500,
                Ferro = 5250,
                Oro = 5000,
                Popolazione = 10,
                TempoRicerca = 12600
            };
            public static dati Difesa = new dati
            {
                Cibo = 4000,
                Legno = 3750,
                Pietra = 3500,
                Ferro = 3500,
                Oro = 3250,
                Popolazione = 15,
                TempoRicerca = 14400
            };
            public static dati Attacco = new dati
            {
                Cibo = 7000,
                Legno = 6500,
                Pietra = 6000,
                Ferro = 5750,
                Oro = 5500,
                Popolazione = 20,
                TempoRicerca = 16200
            };
            public static dati Livello = new dati
            {
                Cibo = 8000,
                Legno = 7750,
                Pietra = 7500,
                Ferro = 7250,
                Oro = 7000,
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
                Cibo = 5000,
                Legno = 4750,
                Pietra = 4500,
                Ferro = 4250,
                Oro = 4000,
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
                Cibo = 7000,
                Legno = 6750,
                Pietra = 6500,
                Ferro = 6250,
                Oro = 6000,
                TempoRicerca = 9000,
                Popolazione = 30
            };
            public static dati Mura_Salute = new dati
            {
                Cibo = 7000,
                Legno = 6750,
                Pietra = 6500,
                Ferro = 6250,
                Oro = 6000,
                TempoRicerca = 9000,
                Popolazione = 30
            };
            public static dati Mura_Difesa = new dati
            {
                Cibo = 7000,
                Legno = 6750,
                Pietra = 6500,
                Ferro = 6250,
                Oro = 6000,
                TempoRicerca = 9000,
                Popolazione = 30
            };
            public static dati Mura_Guarnigione = new dati
            {
                Cibo = 7000,
                Legno = 6750,
                Pietra = 6500,
                Ferro = 6250,
                Oro = 6000,
                TempoRicerca = 9000,
                Popolazione = 30
            };

            public static dati Cancello_Livello = new dati
            {
                Cibo = 9000,
                Legno = 8500,
                Pietra = 8000,
                Ferro = 8750,
                Oro = 8500,
                TempoRicerca = 10800,
                Popolazione = 35
            };
            public static dati Cancello_Salute = new dati
            {
                Cibo = 9000,
                Legno = 8500,
                Pietra = 8000,
                Ferro = 8750,
                Oro = 8500,
                TempoRicerca = 10800,
                Popolazione = 35
            };
            public static dati Cancello_Difesa = new dati
            {
                Cibo = 9000,
                Legno = 8500,
                Pietra = 8000,
                Ferro = 8750,
                Oro = 8500,
                TempoRicerca = 10800,
                Popolazione = 35
            };
            public static dati Cancello_Guarnigione = new dati
            {
                Cibo = 9000,
                Legno = 8500,
                Pietra = 8000,
                Ferro = 8750,
                Oro = 8500,
                TempoRicerca = 10800,
                Popolazione = 35
            };

            public static dati Torri_Livello = new dati
            {
                Cibo = 11000,
                Legno = 10750,
                Pietra = 10500,
                Ferro = 10250,
                Oro = 10000,
                TempoRicerca = 14400,
                Popolazione = 40
            };
            public static dati Torri_Salute = new dati
            {
                Cibo = 11000,
                Legno = 10750,
                Pietra = 10500,
                Ferro = 10250,
                Oro = 10000,
                TempoRicerca = 14400,
                Popolazione = 40
            };
            public static dati Torri_Difesa = new dati
            {
                Cibo = 11000,
                Legno = 10750,
                Pietra = 10500,
                Ferro = 10250,
                Oro = 10000,
                TempoRicerca = 14400,
                Popolazione = 40
            };
            public static dati Torri_Guarnigione = new dati
            {
                Cibo = 11000,
                Legno = 10750,
                Pietra = 10500,
                Ferro = 10250,
                Oro = 10000,
                TempoRicerca = 14400,
                Popolazione = 40
            };

            public static dati Castello_Livello = new dati
            {
                Cibo = 15000,
                Legno = 14750,
                Pietra = 14500,
                Ferro = 14250,
                Oro = 14000,
                TempoRicerca = 21600,
                Popolazione = 60
            };
            public static dati Castello_Salute = new dati
            {
                Cibo = 15000,
                Legno = 14750,
                Pietra = 14500,
                Ferro = 14250,
                Oro = 14000,
                TempoRicerca = 21600,
                Popolazione = 60
            };
            public static dati Castello_Difesa = new dati
            {
                Cibo = 15000,
                Legno = 14750,
                Pietra = 14500,
                Ferro = 14250,
                Oro = 14000,
                TempoRicerca = 21600,
                Popolazione = 60
            };
            public static dati Castello_Guarnigione = new dati
            {
                Cibo = 15000,
                Legno = 14750,
                Pietra = 14500,
                Ferro = 14250,
                Oro = 14000,
                TempoRicerca = 21600,
                Popolazione = 60
            };
        }
    }
}
