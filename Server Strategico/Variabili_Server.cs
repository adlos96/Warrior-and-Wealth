
namespace Server_Strategico.Gioco
{
    internal class Variabili_Server
    {
        public static int moltiplicatore_Quest = 1;
        public static int moltiplicatore_Esperienza = 10;

        public static int D_Viola_To_Blu = 3; // Numero di diamanti blu ottenuti per ogni diamante viola
        public static int Velocizzazione_Tempo = 36; // per ogni diamante blu speso quanti secondi vengono velocizzati

        public class Shop
        {
            public double Costo { get; set; }
            public int Reward { get; set; }

            public static Shop Vip_1 = new Shop
            {
                Costo = 500, //Diamanti_Viola
                Reward = 1 //VIP
            };
            public static Shop Vip_2 = new Shop
            {
                Costo = 14.99, //USDT
                Reward = 1 //VIP
            };

            public static Shop Pacchetto_1 = new Shop
            {
                Costo = 5.99, //USDT
                Reward = 150 //Diamanti_Viola
            };
            public static Shop Pacchetto_2 = new Shop
            {
                Costo = 14.99,
                Reward = 475
            };
            public static Shop Pacchetto_3 = new Shop
            {
                Costo = 24.99,
                Reward = 800
            };
            public static Shop Pacchetto_4 = new Shop
            {
                Costo = 49.99,
                Reward = 1500
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
