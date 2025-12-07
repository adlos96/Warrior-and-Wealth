namespace Server_Strategico.Gioco
{
    internal class Variabili_Server
    {
        //ServerData
        public static int moltiplicatore_Esperienza = 10; //Moltiplicatore esperienza (10 + 1 * 10 == 20 -- 10 + 2 * 10 == 30 -- 10 + 3 * 10 == 40)
        public static int D_Viola_To_Blu = 3; // Numero di diamanti blu ottenuti per ogni diamante viola
        public static int Velocizzazione_Tempo = 36; // per ogni diamante blu speso quanti secondi vengono velocizzati
        public static decimal prelievo_Minimo = 5.00m;
        public static int numero_Code_Base = 1; // Ogni giocatore parte con questo numero di esecuzioni parallele massime (costruttori, Riclutatori, Ricerca)
        public static int numero_Code_Base_Vip = 1; // quante code aggiunge il vip
        public static int timer_Reset_Barbari = 0;
        public static int timer_Reset_Quest = 0;

        //Trasporto - Pesi -

        public static int peso_Risorse_Civile = 3; //peso base per ogni risorsa
        public static int peso_Risorse_Militare = 9; //peso base per ogni risorsa
        public static int peso_Risorse_Ferro = 6;
        public static int peso_Risorse_Oro = 11;
        public static int peso_Risorse_Diamante_Blu = 500;
        public static int peso_Risorse_Diamante_Viola = 1500;

        public class PartialTimerData
        {
            public double Interval { get; set; }
            public bool Enabled { get; set; }
        }
        public class Shop
        {
            public double Costo { get; set; }
            public int Reward { get; set; }

            public static Shop GamePass_Base = new Shop
            {
                Costo = 20.99, // USDT
                Reward = 1 //GamePass
            };
            public static Shop GamePass_Avanzato = new Shop
            {
                Costo = 62.99, // USDT
                Reward = 1 //GamePass
            };

            public static Shop Vip_1 = new Shop
            {
                Costo = 750, //Diamanti_Viola
                Reward = 86400 //VIP
            };
            public static Shop Vip_2 = new Shop
            {
                Costo = 14.99, //USDT
                Reward = 86400 //VIP 24H
            };

            public static Shop Pacchetto_Diamanti_1 = new Shop
            {
                Costo = 5.99, //USDT
                Reward = 150 //Diamanti_Viola
            };
            public static Shop Pacchetto_Diamanti_2 = new Shop
            {
                Costo = 14.99,
                Reward = 475
            };
            public static Shop Pacchetto_Diamanti_3 = new Shop
            {
                Costo = 24.99,
                Reward = 800
            };
            public static Shop Pacchetto_Diamanti_4 = new Shop
            {
                Costo = 49.99,
                Reward = 1500
            };
            public static Shop Scudo_Pace_8h = new Shop
            {
                Costo = 200,
                Reward = 28800 //8 ore in secondi
            };
            public static Shop Scudo_Pace_24h = new Shop
            {
                Costo = 520,
                Reward = 86400 //24 ore in secondi
            };
            public static Shop Scudo_Pace_72h = new Shop
            {
                Costo = 1450,
                Reward = 259200 //72 ore in secondi
            };

            public static Shop Costruttore_24h = new Shop
            {
                Costo = 1200,
                Reward = 86400 //24 ore in secondi
            };
            public static Shop Costruttore_48h = new Shop
            {
                Costo = 2300,
                Reward = 172800 //48 ore in secondi
            };

            public static Shop Reclutatore_24h = new Shop
            {
                Costo = 1500,
                Reward = 86400 //24 ore in secondi
            };
            public static Shop Reclutatore_48h = new Shop
            {
                Costo = 2900,
                Reward = 172800 //48 ore in secondi
            };
        }
        public class Terreni_Virtuali
        {
            public decimal Produzione { get; set; }
            public int Rarita { get; set; }
            public static Terreni_Virtuali Comune = new Terreni_Virtuali
            {
                Produzione = 0.00000000111m,
                Rarita = 50
            };
            public static Terreni_Virtuali NonComune = new Terreni_Virtuali
            {
                Produzione = 0.00000000222m,
                Rarita = 20
            };
            public static Terreni_Virtuali Raro = new Terreni_Virtuali
            {
                Produzione = 0.00000000333m,
                Rarita = 15
            };
            public static Terreni_Virtuali Epico = new Terreni_Virtuali
            {
                Produzione = 0.00000000444m,
                Rarita = 10
            };

            public static Terreni_Virtuali Leggendario = new Terreni_Virtuali
            {
                Produzione = 0.00000000555m,
                Rarita = 5
            };
        }
    }
}
