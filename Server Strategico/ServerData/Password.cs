namespace Server_Strategico.Server
{
    internal class Password
    {
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $@"/server.pfx";
        public static string password = "yourPassword";

        public const string SMTP_USER = "userSMTP"; // casella mail (indirizzo completo)
        public const string SMTP_PASS = "passSMTP"; // password della casella
    }
}
