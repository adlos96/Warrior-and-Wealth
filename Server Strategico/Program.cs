using Server_Strategico.ServerData.Moduli;
using System.Globalization;
using static Server_Strategico.Server.Server;

namespace Server_Strategico
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            GetInstance(); // Starta il server
        }
    }
}
