using Server_Strategico.ServerData.Moduli;
using System.Globalization;
using static Server_Strategico.Server.Server;

namespace Server_Strategico
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var culture = new CultureInfo("it-IT");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            GetInstance();
        }
    }
}
