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
                Cibo = 200,
                Legno = 200,
                Pietra = 200,
                Ferro = 200,
                Oro = 200,
                Popolazione = 5,
                Produzione = 1.06,
                TempoCostruzione = 1800,
                Limite = 1600
            };
            public static Edifici Segheria = new Edifici
            {
                Cibo = 275,
                Legno = 275,
                Pietra = 275,
                Ferro = 275,
                Oro = 275,
                Popolazione = 7,
                Produzione = 0.90,
                TempoCostruzione = 2044,
                Limite = 1500
            };
            public static Edifici CavaPietra = new Edifici
            {
                Cibo = 350,
                Legno = 350,
                Pietra = 350,
                Ferro = 350,
                Oro = 350,
                Popolazione = 10,
                Produzione = 0.77,
                TempoCostruzione = 2310,
                Limite = 1400
            };
            public static Edifici MinieraFerro = new Edifici
            {
                Cibo = 425,
                Legno = 425,
                Pietra = 425,
                Ferro = 425,
                Oro = 425,
                Popolazione = 10,
                Produzione = 0.64,
                TempoCostruzione = 2598,
                Limite = 1300
            };
            public static Edifici MinieraOro = new Edifici
            {
                Cibo = 500,
                Legno = 500,
                Pietra = 500,
                Ferro = 500,
                Oro = 500,
                Popolazione = 10,
                Produzione = 0.52,
                TempoCostruzione = 2920,
                Limite = 1200
            };
            public static Edifici Case = new Edifici
            {
                Cibo = 2600,
                Legno = 2200,
                Pietra = 2700,
                Ferro = 2400,
                Oro = 2500,
                Produzione = 0.0008,
                TempoCostruzione = 3264,
                Limite = 10
            };
            // Produzione Militari
            public static Edifici ProduzioneSpade = new Edifici
            {
                Cibo = 1750,
                Legno = 1750,
                Pietra = 1750,
                Ferro = 1950,
                Oro = 1950,
                Popolazione = 15,
                Produzione = 0.008,
                Consumo_Legno = 0.24,
                Consumo_Ferro = 0.29,
                Consumo_Oro = 0.24,
                TempoCostruzione = 2150,
                Limite = 30
            };
            public static Edifici ProduzioneLance = new Edifici
            {
                Cibo = 2200,
                Legno = 2300,
                Pietra = 2000,
                Ferro = 2000,
                Oro = 2200,
                Popolazione = 15,
                Produzione = 0.007,
                Consumo_Legno = 0.47,
                Consumo_Ferro = 0.21,
                Consumo_Oro = 0.29,
                TempoCostruzione = 2538,
                Limite = 25
            };
            public static Edifici ProduzioneArchi = new Edifici
            {
                Cibo = 2450,
                Legno = 2550,
                Pietra = 2250,
                Ferro = 2250,
                Oro = 2350,
                Popolazione = 15,
                Produzione = 0.006,
                Consumo_Legno = 0.49,
                Consumo_Oro = 0.39,
                TempoCostruzione = 2960,
                Limite = 20
            };
            public static Edifici ProduzioneScudi = new Edifici
            {
                Cibo = 2800,
                Legno = 2500,
                Pietra = 2600,
                Ferro = 2500,
                Oro = 2700,
                Popolazione = 15,
                Produzione = 0.007,
                Consumo_Legno = 0.22,
                Consumo_Ferro = 0.35,
                Consumo_Oro = 0.34,
                TempoCostruzione = 3404,
                Limite = 20
            };
            public static Edifici ProduzioneArmature = new Edifici
            {
                Cibo = 2750,
                Legno = 2750,
                Pietra = 2750,
                Ferro = 2950,
                Oro = 2850,
                Popolazione = 15,
                Produzione = 0.0055,
                Consumo_Ferro = 0.50,
                Consumo_Oro = 0.41,
                TempoCostruzione = 3870,
                Limite = 20
            };
            public static Edifici ProduzioneFrecce = new Edifici
            {
                Cibo = 3950,
                Legno = 4150,
                Pietra = 3850,
                Ferro = 3750,
                Oro = 3550,
                Popolazione = 20,
                Produzione = 0.0040,
                Consumo_Legno = 0.31,
                Consumo_Pietra = 0.43,
                Consumo_Ferro = 0.26,
                Consumo_Oro = 0.39,
                TempoCostruzione = 4358,
                Limite = 200
            };

            public static Edifici CasermaGuerrieri = new Edifici
            {
                Cibo = 2950,
                Legno = 2950,
                Pietra = 2950,
                Ferro = 2950,
                Oro = 2950,
                Popolazione = 30,
                TempoCostruzione = 2600,
                Consumo_Cibo = 0.45,
                Consumo_Oro = 0.25,
                Limite = 15
            };
            public static Edifici CasermaLanceri = new Edifici
            {
                Cibo = 3150,
                Legno = 3150,
                Pietra = 3150,
                Ferro = 3150,
                Oro = 3150,
                Popolazione = 35,
                TempoCostruzione = 2944,
                Consumo_Cibo = 0.55,
                Consumo_Oro = 0.30,
                Limite = 10
            };
            public static Edifici CasermaArceri = new Edifici
            {
                Cibo = 4350,
                Legno = 4350,
                Pietra = 4350,
                Ferro = 4350,
                Oro = 4350,
                Popolazione = 40,
                TempoCostruzione = 3410,
                Consumo_Cibo = 0.65,
                Consumo_Oro = 0.35,
                Limite = 5
            };
            public static Edifici CasermaCatapulte = new Edifici
            {
                Cibo = 5050,
                Legno = 5050,
                Pietra = 5050,
                Ferro = 5050,
                Oro = 5050,
                Popolazione = 55,
                TempoCostruzione = 3998,
                Consumo_Cibo = 0.95,
                Consumo_Oro = 0.55,
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
