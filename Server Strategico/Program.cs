using Server_Strategico.ServerData.Moduli;
using static Server_Strategico.Server.Server;

namespace Server_Strategico
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameSave.Initialize(); // Inizializza il sistema di salvataggio
            GetInstance(); // Starta il server
        }
    }
}
