using static BattaglieV2;
using static Server_Strategico.Gioco.Battaglie;

namespace Server_Strategico.ServerData.Moduli
{
    internal class Esplorazioni
    {
        public class EsplorazioneReport
        {
            public bool Successo { get; set; }
            public string Giocatore { get; set; }
            public string Nemico { get; set; }
            public Risorse Risorse { get; set; }
            public int Xp_Ottenibile { get; set; }
            public UnitGroup Truppe { get; set; }
            public Struttura[] Strutture { get; set; } = new Struttura[7];
        }

        public class Risorse
        {
            public int Legno { get; set; }
            public int Pietra { get; set; }
            public int Ferro { get; set; }
            public int Cibo { get; set; }
            public int Oro { get; set; }
            public int Diamanti_Viola { get; set; }
            public int Diamanti_Ble { get; set; }
        }
        public class Struttura
        {
            public string Nome { get; set; }
            public int Livello { get; set; }
            public int Salute { get; set; }
            public int Difesa { get; set; }
            public int Salute_Max { get; set; }
            public int Difesa_Max { get; set; }
            public UnitGroup Truppe { get; set; }
        }
    }
}
