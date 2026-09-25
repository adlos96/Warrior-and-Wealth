namespace Server_Strategico.Server
{
    internal class Password
    {
        //Server - Server
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $@"/server.pfx";
        public static string password = "yourPassword";

        //Server - Dominio
        public const string SMTP_USER = "userSMTP"; // casella mail (indirizzo completo)
        public const string SMTP_PASS = "passSMTP"; // password della casella

        //Blockchain
        public const string SEED_ENCRYPTION_PASSPHRASE = "PSS_Phrase"; // password della casella
        public const string ADMIN_ALERT_EMAIL = "E-mail";

        public const string AnkrApiKey = "AnkrApiKeyHOLD"; // API ankr
        public const string PolygonScanApiKey = "PolygonScanApiKeyHOLD"; // API Polygon Scan
        public const string EtherScanApiKey = "EtherScanApiKeyHOLD"; // API Polygon Scan

    }
}
