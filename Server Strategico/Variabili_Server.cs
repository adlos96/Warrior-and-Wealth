namespace Server_Strategico.Gioco
{
    internal class Variabili_Server
    {
        public static string versione_Client_Necessario = "0.1.14.5";
        public static string[] lingue_Supportate = {"it", "en" };

        //ServerData
        public static Int16 moltiplicatore_Esperienza = 10; //Moltiplicatore esperienza (10 + 1 * 10 == 20, 10 + 2 * 10 == 30, 10 + 3 * 10 == 40)
        public static Int16 D_Viola_To_Blu = 3; // Numero di diamanti blu ottenuti per ogni diamante viola
        public static Int16 Tributi_To_D_Viola = 45; // Numero di diamanti blu ottenuti per ogni diamante viola
        public static Int16 Velocizzazione_Tempo = 36; // per ogni diamante blu speso quanti secondi vengono velocizzati
        public static decimal prelievo_Minimo = 5.00m;
        public static Int16 numero_Code_Base = 1; // Ogni giocatore parte con questo numero di esecuzioni parallele massime (costruttori, Riclutatori, Ricerca)
        public static Int16 numero_Code_Base_Vip = 1; // quante code aggiunge il vip
        public static int timer_Reset_Barbari = 0;
        public static int timer_Reset_Quest = 0;

        // Gateway WebSocket per il client web (vedi ServerData/WebSocketGateway.cs).
        // Stesso protocollo testuale del client WinForms, solo trasporto diverso:
        // porta separata da quella WatsonTcp (8443) cosi' i due listener non si
        // toccano. Con WebGatewayEnabled=false il server si comporta esattamente
        // come prima; si puo' comunque avviarlo/fermarlo a runtime dalla console
        // del server coi comandi "webstart"/"webstop".
        public static bool WebGatewayEnabled = false;
        public static int WebGatewayPort = 8444;

        //PVP
        public static Int16 Max_Diamanti_Viola_PVP = 150; //massimo diamanti viola che un giocatore può guadagnare in un giorno tramite PVP
        public static Int16 max_Diamanti_Blu_PVP = 300; //massimo diamanti blu che un giocatore può guadagnare in un giorno tramite PVP
        public static Int16 Max_Diamanti_Viola_PVP_Giocatore = 10; //massimo diamanti viola che un giocatore può guadagnare da un singolo avversario tramite PVP
        public static Int16 Max_Diamanti_Blu_PVP_Giocatore = 20; //massimo diamanti blu che un giocatore può guadagnare da un singolo avversario tramite PVP
        public static bool Reset_Gironaliero = false;
        public static bool Reset_Settimanale = false;
        public static bool Reset_Mensile = false;

        //Trasporto - Pesi -
        public static Int16 peso_Risorse_Militare = 8; //peso base per ogni risorsa
        public static Int16 peso_Risorse_Cibo = 3;
        public static Int16 peso_Risorse_Legno = 5;
        public static Int16 peso_Risorse_Pietra = 8;
        public static Int16 peso_Risorse_Ferro = 11;
        public static Int16 peso_Risorse_Oro = 15;
        public static Int16 peso_Risorse_Diamante_Blu = 1000;
        public static Int16 peso_Risorse_Diamante_Viola = 2000;

        public static int tempo_Riparazione = 12; //tempo in secondi necessario per aggiungere una unità di HP o DEF (Base 60s test 12s)

        //Sblocco Esercito
        public static int truppe_II = 9;
        public static int truppe_III = 19;
        public static int truppe_IV = 38;
        public static int truppe_V = 50;

        public static int citta_Barbare_Unlock = 5;  //Sblocco Città barbare
        public static int PVP_Unlock = 10;  //Sblocco pvp

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

        public class Avatar
        {
            public string Nome { get; set; }
            public int Costo { get; set; }

            public static Avatar Lord_1 = new Avatar
            {
                Nome = "Lord_1", //GamePass
                Costo = 600, // USDT
            };
            public static Avatar Lord_2 = new Avatar
            {
                Nome = "Lord_2", //GamePass
                Costo = 400, // USDT
            };
            public static Avatar Lord_3 = new Avatar
            {
                Nome = "Lord_3", //GamePass
                Costo = 0, // USDT
            };
            public static Avatar Lord_4 = new Avatar
            {
                Nome = "Lord_4", //GamePass
                Costo = 400, // USDT
            };
            public static Avatar Lord_5 = new Avatar
            {
                Nome = "Lord_5", //GamePass
                Costo = 400, // USDT
            };
            public static Avatar Lord_6 = new Avatar
            {
                Nome = "Lord_6", //GamePass
                Costo = 400, // USDT
            };
            public static Avatar Lord_7 = new Avatar
            {
                Nome = "Lord_7", //GamePass
                Costo = 400, // USDT
            };

            public static Avatar Lady_1 = new Avatar
            {
                Nome = "Lady_1", //GamePass
                Costo = 0, // USDT
            };
            public static Avatar Lady_2 = new Avatar
            {
                Nome = "Lady_2", //GamePass
                Costo = 400, // USDT
            };
            public static Avatar Lady_3 = new Avatar
            {
                Nome = "Lady_3", //GamePass
                Costo = 400, // USDT
            };
            public static Avatar Lady_4 = new Avatar
            {
                Nome = "Lady_4", //GamePass
                Costo = 400, // USDT
            };
            public static Avatar Lady_5 = new Avatar
            {
                Nome = "Lady_5", //GamePass
                Costo = 400, // USDT
            };
            public static Avatar Lady_6 = new Avatar
            {
                Nome = "Lady_6", //GamePass
                Costo = 400, // USDT
            };
        }
        public class Shop
        {
            public double Costo { get; set; }
            public int Reward { get; set; }

            public static Shop GamePass_Base = new Shop
            {
                Costo = 20.99, // USDT
                Reward = 2592000 //GamePass
            };
            public static Shop GamePass_Avanzato = new Shop
            {
                Costo = 62.99, // USDT
                Reward = 2592000 //GamePass
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
                Costo = 250,
                Reward = 28800 //8 ore in secondi
            };
            public static Shop Scudo_Pace_24h = new Shop
            {
                Costo = 650,
                Reward = 86400 //24 ore in secondi
            };
            public static Shop Scudo_Pace_72h = new Shop
            {
                Costo = 1600,
                Reward = 259200 //72 ore in secondi
            };

            public static Shop Costruttore_24h = new Shop
            {
                Costo = 1700,
                Reward = 86400 //24 ore in secondi
            };
            public static Shop Costruttore_48h = new Shop
            {
                Costo = 3100,
                Reward = 172800 //48 ore in secondi
            };

            public static Shop Reclutatore_24h = new Shop
            {
                Costo = 2200,
                Reward = 86400 //24 ore in secondi
            };
            public static Shop Reclutatore_48h = new Shop
            {
                Costo = 4100,
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
