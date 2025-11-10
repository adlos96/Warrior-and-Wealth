
namespace Server_Strategico.Gioco
{
    internal class Variabili_Server
    {
        public static int moltiplicatore_Quest = 1;
        public static int moltiplicatore_Esperienza = 10;

        public static int D_Viola_To_Blu = 3; // Numero di diamanti blu ottenuti per ogni diamante viola
        public static int Velocizzazione_Tempo = 36; // per ogni diamante blu speso quanti secondi vengono velocizzati
        public static int numero_Code_Base = 1; // Ogni giocatore parte con questo numero di esecuzioni parallele massime (costruttori, Riclutatori, Ricerca)
        public static int numero_Code_Base_Vip = 1; // quante code aggiunge il vip

        public class Shop
        {
            public double Costo { get; set; }
            public int Reward { get; set; }

            public static Shop GamePass_Base = new Shop
            {
                Costo = 17.99, // USDT
                Reward = 1 //VIP
            };
            public static Shop GamePass_Avanzato = new Shop
            {
                Costo = 62.99, // USDT
                Reward = 1 //VIP
            };

            public static Shop Vip_1 = new Shop
            {
                Costo = 750, //Diamanti_Viola
                Reward = 1 //VIP
            };
            public static Shop Vip_2 = new Shop
            {
                Costo = 14.99, //USDT
                Reward = 1 //VIP
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
                Reward = 84600 //24 ore in secondi
            };
            public static Shop Scudo_Pace_72h = new Shop
            {
                Costo = 1450,
                Reward = 253800 //72 ore in secondi
            };

            public static Shop Costruttore_24h = new Shop
            {
                Costo = 1200,
                Reward = 28800 //24 ore in secondi
            };
            public static Shop Costruttore_48h = new Shop
            {
                Costo = 2300,
                Reward = 57600 //48 ore in secondi
            };

            public static Shop Reclutatore_24h = new Shop
            {
                Costo = 1500,
                Reward = 28800 //24 ore in secondi
            };
            public static Shop Reclutatore_48h = new Shop
            {
                Costo = 2900,
                Reward = 57600 //48 ore in secondi
            };
        }
        public class Terreni_Virtuali
        {
            public double Produzione { get; set; }
            public int Rarita { get; set; }
            public static Terreni_Virtuali Comune = new Terreni_Virtuali
            {
                Produzione = 0.00000000111,
                Rarita = 50
            };
            public static Terreni_Virtuali NonComune = new Terreni_Virtuali
            {
                Produzione = 0.00000000222,
                Rarita = 20
            };
            public static Terreni_Virtuali Raro = new Terreni_Virtuali
            {
                Produzione = 0.00000000333,
                Rarita = 15
            };
            public static Terreni_Virtuali Epico = new Terreni_Virtuali
            {
                Produzione = 0.00000000444,
                Rarita = 10
            };

            public static Terreni_Virtuali Leggendario = new Terreni_Virtuali
            {
                Produzione = 0.00000000555,
                Rarita = 5
            };
        }
    }
}
