using static Server_Strategico.Gioco.Ricerca;

namespace Server_Strategico.Gioco
{
    public class Strutture
    {
        public class Edifici
        {
            #region Dati
            public int Cibo { get; set; }
            public int Legno { get; set; }
            public int Pietra { get; set; }
            public int Ferro { get; set; }
            public int Oro { get; set; }
            public int Popolazione { get; set; }

            public int Diamanti_Viola { get; set; }
            public double Produzione { get; set; }
            public double Consumo_Cibo { get; set; }
            public double Consumo_Legno { get; set; }
            public double Consumo_Pietra { get; set; }
            public double Consumo_Ferro { get; set; }
            public double Consumo_Oro { get; set; }

            public int Salute { get; set; }
            public int Difesa { get; set; }
            public int Attacco { get; set; }
            public int Guarnigione { get; set; }

            public int Limite { get; set; }
            public double Incremento { get; set; }
            public double TempoCostruzione { get; set; }
            #endregion

            public static Edifici Terreni_Virtuali = new Edifici
            {
                Diamanti_Viola = 150,
                TempoCostruzione = 10
            };

            // Edifici Civili - Produzione Risorse
            public static Edifici Fattoria = new Edifici
            {
                Cibo = 700,                 //Costo
                Legno = 630,                //Costo 
                Pietra = 580,               //Costo
                Ferro = 530,                //Costo    
                Oro = 480,                  //Costo
                Popolazione = 4,            //Costo
                Produzione = 0.32,          //Produzione Risorsa (tick)
                TempoCostruzione = 7200,    //Tempo costruzione in secondi
                Limite = 3400               //Spazio magazzino per la risorsa
            };
            public static Edifici Segheria = new Edifici
            {
                Cibo = 850,
                Legno = 780,
                Pietra = 730,
                Ferro = 680,
                Oro = 630,
                Popolazione = 5,
                Produzione = 0.28,
                TempoCostruzione = 7650,
                Limite = 3100
            };
            public static Edifici CavaPietra = new Edifici
            {
                Cibo = 1000,
                Legno = 920,
                Pietra = 850,
                Ferro = 780,
                Oro = 710,
                Popolazione = 7,
                Produzione = 0.24,
                TempoCostruzione = 8200,
                Limite = 2800
            };
            public static Edifici MinieraFerro = new Edifici
            {
                Cibo = 1150,
                Legno = 1090,
                Pietra = 1010,
                Ferro = 940,
                Oro = 850,
                Popolazione = 7,
                Produzione = 0.19,
                TempoCostruzione = 8850,
                Limite = 2500
            };
            public static Edifici MinieraOro = new Edifici
            {
                Cibo = 1300,
                Legno = 1230,
                Pietra = 1150,
                Ferro = 1080,
                Oro = 1000,
                Popolazione = 7,
                Produzione = 0.16,
                TempoCostruzione = 9600,
                Limite = 2200
            };
            public static Edifici Case = new Edifici
            {
                Cibo = 1900,
                Legno = 1820,
                Pietra = 1750,
                Ferro = 1680,
                Oro = 1600,
                Produzione = 0.00043,
                TempoCostruzione = 10450,
                Limite = 10
            };
            // Produzione Militari
            public static Edifici ProduzioneSpade = new Edifici
            {
                Cibo = 2200,
                Legno = 2110,
                Pietra = 2040,
                Ferro = 1950,
                Oro = 1880,
                Popolazione = 15,
                Produzione = 0.00027,
                Consumo_Legno = 0.080,
                Consumo_Ferro = 0.097,
                Consumo_Oro = 0.080,
                TempoCostruzione = 10800,
                Limite = 30
            };
            public static Edifici ProduzioneLance = new Edifici
            {
                Cibo = 2450,
                Legno = 2380,
                Pietra = 2300,
                Ferro = 2230,
                Oro = 1160,
                Popolazione = 15,
                Produzione = 0.00023,
                Consumo_Legno = 0.157,
                Consumo_Ferro = 0.07,
                Consumo_Oro = 0.097,
                TempoCostruzione = 11188,
                Limite = 25
            };
            public static Edifici ProduzioneArchi = new Edifici
            {
                Cibo = 2700,
                Legno = 2630,
                Pietra = 2550,
                Ferro = 2470,
                Oro = 2380,
                Popolazione = 15,
                Produzione = 0.0020,
                Consumo_Legno = 0.0163,
                Consumo_Oro = 0.130,
                TempoCostruzione = 11610,
                Limite = 20
            };
            public static Edifici ProduzioneScudi = new Edifici
            {
                Cibo = 3000,
                Legno = 2900,
                Pietra = 2820,
                Ferro = 2730,
                Oro = 2600,
                Popolazione = 15,
                Produzione = 0.0023,
                Consumo_Legno = 0.13,
                Consumo_Ferro = 0.10,
                Consumo_Oro = 0.09,
                TempoCostruzione = 12054,
                Limite = 20
            };
            public static Edifici ProduzioneArmature = new Edifici
            {
                Cibo = 3350,
                Legno = 3240,
                Pietra = 3160,
                Ferro = 3080,
                Oro = 3000,
                Popolazione = 15,
                Produzione = 0.0018,
                Consumo_Ferro = 0.17,
                Consumo_Oro = 0.14,
                TempoCostruzione = 12520,
                Limite = 20
            };
            public static Edifici ProduzioneFrecce = new Edifici
            {
                Cibo = 3950,
                Legno = 3850,
                Pietra = 3730,
                Ferro = 3620,
                Oro = 3550,
                Popolazione = 20,
                Produzione = 0.0013,
                Consumo_Legno = 0.10,
                Consumo_Pietra = 0.14,
                Consumo_Ferro = 0.09,
                Consumo_Oro = 0.13,
                TempoCostruzione = 13008,
                Limite = 200
            };

            public static Edifici CasermaGuerrieri = new Edifici
            {
                Cibo = 3400,
                Legno = 3142,
                Pietra = 3011,
                Ferro = 2900,
                Oro = 2850,
                Popolazione = 30,
                TempoCostruzione = 10800,
                Consumo_Cibo = 0.15,
                Consumo_Oro = 0.08,
                Limite = 14
            };
            public static Edifici CasermaLanceri = new Edifici
            {
                Cibo = 3750,
                Legno = 3547,
                Pietra = 3402,
                Ferro = 3323,
                Oro = 3207,
                Popolazione = 35,
                TempoCostruzione = 19800,
                Consumo_Cibo = 0.18,
                Consumo_Oro = 0.1,
                Limite = 9
            };
            public static Edifici CasermaArceri = new Edifici
            {
                Cibo = 4350,
                Legno = 4206,
                Pietra = 4099,
                Ferro = 3984,
                Oro = 3891,
                Popolazione = 40,
                TempoCostruzione = 22000,
                Consumo_Cibo = 0.21,
                Consumo_Oro = 0.11,
                Limite = 5
            };
            public static Edifici CasermaCatapulte = new Edifici
            {
                Cibo = 5350,
                Legno = 5200,
                Pietra = 5106,
                Ferro = 5025,
                Oro = 4912,
                Popolazione = 55,
                TempoCostruzione = 24600,
                Consumo_Cibo = 0.31,
                Consumo_Oro = 0.18,
                Limite = 3
            };
            // Valori statistiche edifici difensivi villaggio
            public static Edifici Ingresso = new Edifici
            {
                Guarnigione = 15,
            };
            public static Edifici Citta = new Edifici
            {
                Guarnigione = 25
            };
            public static Edifici Cancello = new Edifici
            {
                Salute = 35,
                Difesa = 25,
                Guarnigione = 20
            };
            public static Edifici Mura = new Edifici
            {
                Salute = 30,
                Difesa = 20,
                Guarnigione = 20
            };
            public static Edifici Torri = new Edifici
            {
                Salute = 40,
                Difesa = 30,
                Guarnigione = 20
            };
            public static Edifici Castello = new Edifici
            {
                Salute = 50,
                Difesa = 40,
                Guarnigione = 25
            };
        }
        public class Riparazione // Costi di riparazione delle strutture
        {
            public static Edifici Cancello = new Edifici
            {
                Salute = 1,
                Difesa = 1,

                Consumo_Cibo = 380,
                Consumo_Legno = 180,
                Consumo_Pietra = 320,
                Consumo_Ferro = 400,
                Consumo_Oro = 340,
            };
            public static Edifici Mura = new Edifici
            {
                Salute = 1,
                Difesa = 1,

                Consumo_Cibo = 570,
                Consumo_Legno = 240,
                Consumo_Pietra = 480,
                Consumo_Ferro = 260,
                Consumo_Oro = 410,
            };
            public static Edifici Torri = new Edifici
            {
                Salute = 1,
                Difesa = 1,

                Consumo_Cibo = 830,
                Consumo_Legno = 280,
                Consumo_Pietra = 755,
                Consumo_Ferro = 480,
                Consumo_Oro = 550,
            };
            public static Edifici Castello = new Edifici
            {
                Salute = 1,
                Difesa = 1,

                Consumo_Cibo = 1500,
                Consumo_Legno = 680,
                Consumo_Pietra = 1363,
                Consumo_Ferro = 940,
                Consumo_Oro = 820,
            };
        }
    }
}
