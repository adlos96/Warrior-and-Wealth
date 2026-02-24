namespace Server_Strategico.Gioco
{
    internal class Variabili_Server
    {
        //ServerData
        public static Int16 moltiplicatore_Esperienza = 10; //Moltiplicatore esperienza (10 + 1 * 10 == 20 -- 10 + 2 * 10 == 30 -- 10 + 3 * 10 == 40)
        public static Int16 D_Viola_To_Blu = 3; // Numero di diamanti blu ottenuti per ogni diamante viola
        public static Int16 Velocizzazione_Tempo = 36; // per ogni diamante blu speso quanti secondi vengono velocizzati
        public static decimal prelievo_Minimo = 5.00m;
        public static Int16 numero_Code_Base = 1; // Ogni giocatore parte con questo numero di esecuzioni parallele massime (costruttori, Riclutatori, Ricerca)
        public static Int16 numero_Code_Base_Vip = 1; // quante code aggiunge il vip
        public static int timer_Reset_Barbari = 0;
        public static int timer_Reset_Quest = 0;

        //PVP
        public static Int16 Max_Diamanti_Viola_PVP = 150; //massimo diamanti viola che un giocatore può guadagnare in un giorno tramite PVP
        public static Int16 max_Diamanti_Blu_PVP = 300; //massimo diamanti blu che un giocatore può guadagnare in un giorno tramite PVP
        public static Int16 Max_Diamanti_Viola_PVP_Giocatore = 10; //massimo diamanti viola che un giocatore può guadagnare da un singolo avversario tramite PVP
        public static Int16 Max_Diamanti_Blu_PVP_Giocatore = 20; //massimo diamanti viola che un giocatore può guadagnare da un singolo avversario tramite PVP
        public static bool Reset_Gironaliero = false;
        public static bool Reset_Settimanale = false;
        public static bool Reset_Mensile = false;

        //Trasporto - Pesi -
        public static int peso_Risorse_Militare = 9; //peso base per ogni risorsa
        public static int peso_Risorse_Cibo = 4;
        public static int peso_Risorse_Legno = 6;
        public static int peso_Risorse_Pietra = 9;
        public static int peso_Risorse_Ferro = 12;
        public static int peso_Risorse_Oro = 16;
        public static int peso_Risorse_Diamante_Blu = 1200;
        public static int peso_Risorse_Diamante_Viola = 2200;

        public static int tempo_Riparazione = 12; //tempo in secondi per riparare le strutture danneggiate

        //Sblocco Esercito
        public static int truppe_II = 9;
        public static int truppe_III = 19;
        public static int truppe_IV = 38;
        public static int truppe_V = 50;

        public static int citta_Barbare_Unlock = 5;  //Sblocco Città barbare
        public static int PVP_Unlock = 10;  //Sblocco pvp

        public static string versione_Client_Necessario = "0.1.0";

        public static int _Server_Consumo_RAM = 0;
        public static int[] gamePass_DailyReward = {
            135, 135, 135, 135, 135,
            135, 282, 135, 135, 135,
            135, 135, 135, 135, 525,
            135, 135, 135, 135, 135,
            135, 135, 135, 135, 135,
            135, 135, 135, 135, 825,

            135, 135, 135, 135, 135,
            135, 135, 135, 135, 135,
            135, 135, 135, 135, 135,
            135, 135, 135, 135, 135,
            135, 135, 135, 135, 135,
            135, 135, 135, 135, 1095,

            135, 135, 135, 135, 135,
            135, 135, 135, 135, 135,
            135, 135, 135, 135, 135,
            135, 135, 135, 135, 135,
            135, 135, 135, 135, 135,
            135, 135, 135, 135, 2100,
        };

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
                Reward = 1700
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
            public int Limite_Strutture { get; set; }
            public static Terreni_Virtuali Comune = new Terreni_Virtuali
            {
                Produzione = 0.00000000111m,    //Reward ogni tick
                Rarita = 50,                    //percentuale di probabilità di trovarlo
                Limite_Strutture = 5            //Aumenta il limite di strutture massime costruibili nel proprio villaggio (Regno)
            };
            public static Terreni_Virtuali NonComune = new Terreni_Virtuali
            {
                Produzione = 0.00000000222m,
                Rarita = 20,
                Limite_Strutture = 10
            };
            public static Terreni_Virtuali Raro = new Terreni_Virtuali
            {
                Produzione = 0.00000000333m,
                Rarita = 15,
                Limite_Strutture = 15
            };
            public static Terreni_Virtuali Epico = new Terreni_Virtuali
            {
                Produzione = 0.00000000444m,
                Rarita = 10,
                Limite_Strutture = 20
            };

            public static Terreni_Virtuali Leggendario = new Terreni_Virtuali
            {
                Produzione = 0.00000000555m,
                Rarita = 5,
                Limite_Strutture = 25
            };
        }
    }
}
