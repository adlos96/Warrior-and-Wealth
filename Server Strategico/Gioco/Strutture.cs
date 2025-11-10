namespace Server_Strategico.Gioco
{
    public class Strutture
    {
        public class Edifici
        {
            public int Cibo { get; set; }
            public int Legno { get; set; }
            public int Pietra { get; set; }
            public int Ferro { get; set; }
            public int Oro { get; set; }
            public int Popolazione { get; set; }

            public int Diamanti_Viola { get; set; }
            public double Produzione { get; set; }
            public double Consumo_Legno { get; set; }
            public double Consumo_Pietra { get; set; }
            public double Consumo_Ferro { get; set; }

            public int Salute { get; set; }
            public int Difesa { get; set; }
            public int Attacco { get; set; }
            public int Guarnigione { get; set; }

            public int Limite { get; set; }
            public double Incremento { get; set; }
            public double TempoCostruzione { get; set; }

            public static Edifici Terreni_Virtuali = new Edifici
            {
                Diamanti_Viola = 150,
                TempoCostruzione = 10
            };

            // Edifici Civili
            public static Edifici Fattoria = new Edifici
            {
                Cibo = 100,
                Legno = 100,
                Pietra = 100,
                Ferro = 100,
                Oro = 100,
                Popolazione = 5,
                Produzione = 1.12,
                TempoCostruzione = 101,
                Limite = 2000
            };
            public static Edifici Segheria = new Edifici
            {
                Cibo = 175,
                Legno = 175,
                Pietra = 175,
                Ferro = 175,
                Oro = 175,
                Popolazione = 7,
                Produzione = 0.96,
                TempoCostruzione = 117,
                Limite = 2000
            };
            public static Edifici CavaPietra = new Edifici
            {
                Cibo = 250,
                Legno = 250,
                Pietra = 250,
                Ferro = 250,
                Oro = 250,
                Popolazione = 10,
                Produzione = 0.83,
                TempoCostruzione = 136,
                Limite = 2000
            };
            public static Edifici MinieraFerro = new Edifici
            {
                Cibo = 325,
                Legno = 325,
                Pietra = 325,
                Ferro = 325,
                Oro = 325,
                Popolazione = 10,
                Produzione = 0.70,
                TempoCostruzione = 148,
                Limite = 2000
            };
            public static Edifici MinieraOro = new Edifici
            {
                Cibo = 400,
                Legno = 400,
                Pietra = 400,
                Ferro = 400,
                Oro = 400,
                Popolazione = 10,
                Produzione = 0.57,
                TempoCostruzione = 172,
                Limite = 2000
            };
            public static Edifici Case = new Edifici
            {
                Cibo = 2500,
                Legno = 2500,
                Pietra = 2500,
                Ferro = 2500,
                Oro = 2500,
                Produzione = 0.001,
                TempoCostruzione = 221,
                Limite = 100
            };
            // Produzione Militari
            public static Edifici ProduzioneSpade = new Edifici
            {
                Cibo = 1750,
                Legno = 1750,
                Pietra = 1750,
                Ferro = 1750,
                Oro = 1750,
                Popolazione = 10,
                Produzione = 0.008,
                Consumo_Legno = 0.15,
                Consumo_Ferro = 0.25,
                TempoCostruzione = 152,
                Limite = 2000
            };
            public static Edifici ProduzioneLance = new Edifici
            {
                Cibo = 2000,
                Legno = 2000,
                Pietra = 2000,
                Ferro = 2000,
                Oro = 2000,
                Popolazione = 10,
                Produzione = 0.007,
                Consumo_Legno = 0.35,
                Consumo_Ferro = 0.18,
                TempoCostruzione = 181,
                Limite = 2000
            };
            public static Edifici ProduzioneArchi = new Edifici
            {
                Cibo = 2250,
                Legno = 2250,
                Pietra = 2250,
                Ferro = 2250,
                Oro = 2250,
                Popolazione = 10,
                Consumo_Legno = 0.45,
                Produzione = 0.0075,
                TempoCostruzione = 214,
                Limite = 2000
            };
            public static Edifici ProduzioneScudi = new Edifici
            {
                Cibo = 2500,
                Legno = 2500,
                Pietra = 2500,
                Ferro = 2500,
                Oro = 2500,
                Popolazione = 10,
                Produzione = 0.008,
                Consumo_Legno = 0.22,
                Consumo_Ferro = 0.35,
                TempoCostruzione = 229,
                Limite = 2000
            };
            public static Edifici ProduzioneArmature = new Edifici
            {
                Cibo = 2750,
                Legno = 2750,
                Pietra = 2750,
                Ferro = 2750,
                Oro = 2750,
                Popolazione = 10,
                Produzione = 0.0065,
                Consumo_Ferro = 0.50,
                TempoCostruzione = 253,
                Limite = 2000
            };
            public static Edifici ProduzioneFrecce = new Edifici
            {
                Cibo = 3750,
                Legno = 3750,
                Pietra = 3750,
                Ferro = 3750,
                Oro = 3750,
                Popolazione = 10,
                Produzione = 0.005,
                Consumo_Legno = 0.25,
                Consumo_Ferro = 0.18,
                Consumo_Pietra = 0.40,
                TempoCostruzione = 294,
                Limite = 2000
            };

            public static Edifici CasermaGuerrieri = new Edifici
            {
                Cibo = 2450,
                Legno = 2450,
                Pietra = 2450,
                Ferro = 2450,
                Oro = 2450,
                Popolazione = 25,
                TempoCostruzione = 274,
                Limite = 15
            };
            public static Edifici CasermaLanceri = new Edifici
            {
                Cibo = 2650,
                Legno = 2650,
                Pietra = 2650,
                Ferro = 2650,
                Oro = 2650,
                Popolazione = 30,
                TempoCostruzione = 295,
                Limite = 10
            };
            public static Edifici CasermaArceri = new Edifici
            {
                Cibo = 3850,
                Legno = 3850,
                Pietra = 3850,
                Ferro = 3850,
                Oro = 3850,
                Popolazione = 35,
                TempoCostruzione = 316,
                Limite = 5
            };
            public static Edifici CasermaCatapulte = new Edifici
            {
                Cibo = 4550,
                Legno = 4550,
                Pietra = 4550,
                Ferro = 4550,
                Oro = 4550,
                Popolazione = 50,
                TempoCostruzione = 337,
                Limite = 3
            };

            public static Edifici Ingresso = new Edifici
            {
                Guarnigione = 100
            };
            public static Edifici Citta = new Edifici
            {
                Guarnigione = 200
            };
            public static Edifici Cancello = new Edifici
            {
                Salute = 50,
                Difesa = 50,
                Guarnigione = 50
            };
            public static Edifici Mura = new Edifici
            {
                Salute = 50,
                Difesa = 50,
                Guarnigione = 50
            };
            public static Edifici Torri = new Edifici
            {
                Salute = 50,
                Difesa = 50,
                Guarnigione = 50
            };
            public static Edifici Castello = new Edifici
            {
                Salute = 75,
                Difesa = 75,
                Guarnigione = 75
            };
        }
    }
}
