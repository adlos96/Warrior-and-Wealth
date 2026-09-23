namespace Server_Strategico.Server
{
    internal class Password
    {
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $@"/server.pfx";
        public static string password = "yourPassword";

        public const string SMTP_USER = "userSMTP"; // casella mail (indirizzo completo)
        public const string SMTP_PASS = "passSMTP"; // password della casella

        public const string SEED_ENCRYPTION_PASSPHRASE = "PSS_Phrase"; // password della casella

        // 24/09/2026: casella dove arrivano gli avvisi di tesoreria (BlockchainManager) — fondi
        // USDT scarsi, POL (gas) scarso, trasferimenti verso il wallet secondario eseguiti/falliti.
        // TODO: inserisci l'indirizzo mail su cui vuoi ricevere questi avvisi. Finché resta vuota
        // gli avvisi finiscono solo nei log del server, nessuna mail viene inviata.
        public const string ADMIN_ALERT_EMAIL = "E-mail";
    }
}
