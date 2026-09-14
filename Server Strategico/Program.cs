using Server_Strategico.Gioco;
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

            // 14/09/2026: su una VPS Linux headless (nessuna console interattiva
            // comoda per digitare "webstart" dopo ogni riavvio) il gateway
            // WebSocket per il client web puo' essere abilitato automaticamente
            // impostando la variabile d'ambiente WW_WEB_GATEWAY=true prima di
            // avviare il server (vedi Compilatore/setup-web.sh). Il default resta
            // invariato (WebGatewayEnabled=false, comando "webstart" a mano) per
            // non cambiare il comportamento su Windows/desktop.
            var webGatewayEnv = Environment.GetEnvironmentVariable("WW_WEB_GATEWAY");
            if (!string.IsNullOrEmpty(webGatewayEnv) && bool.TryParse(webGatewayEnv, out bool webGatewayEnabled))
                Variabili_Server.WebGatewayEnabled = webGatewayEnabled;

            GetInstance();
        }
    }
}
