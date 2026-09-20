using MailKit.Security;
using MimeKit;
using Server_Strategico.Server;

namespace Strategico_V2.Manager
{
    /// <summary>
    /// Gestisce l'invio di email tramite SMTP Aruba (MailKit).
    /// Richiede il pacchetto NuGet: MailKit
    /// </summary>
    public static class EmailManager
    {
        // ── Configurazione SMTP Aruba ──────────────────────────────────────────
        // TODO: sposta SMTP_USER e SMTP_PASS in variabili d'ambiente / appsettings
        //       prima di committare su Git o pubblicare il progetto.
        private const string SMTP_HOST = "smtps.aruba.it";
        private const int SMTP_PORT = 465;                            // SSL implicito

        private const string SENDER_EMAIL = Password.SMTP_USER;                // deve coincidere con la casella autenticata
        private const string SENDER_NAME = "Warrior & Wealth";

        // ── Template base ──────────────────────────────────────────────────────
        // Le graffe CSS sono singole: il template passa solo da Replace("{BODY_CONTENT}", ...)
        private const string HTML_TEMPLATE = @"
            <!DOCTYPE html>
            <html>
            <head>
              <meta charset='UTF-8'>
              <style>
                body      { font-family: Georgia, serif; background: #1a1208; margin: 0; padding: 20px; }
                .container{ max-width: 600px; margin: auto; background: #2d1f0e;
                             border: 2px solid #8b6914; border-radius: 8px; overflow: hidden; }
                .header   { background: #3d2b0a; padding: 24px; text-align: center;
                             border-bottom: 2px solid #8b6914; }
                .header h1{ color: #d4a843; margin: 0; font-size: 26px; letter-spacing: 2px; }
                .header p { color: #a07830; margin: 6px 0 0; font-size: 13px; }
                .body     { padding: 30px 36px; color: #d8c9a0; line-height: 1.7; }
                .body h2  { color: #d4a843; margin-top: 0; }
                .code-box { background: #1a1208; border: 1px solid #8b6914; border-radius: 6px;
                             padding: 16px; text-align: center; margin: 24px 0; }
                .code-box span { font-size: 32px; font-weight: bold; color: #f0c040;
                                  letter-spacing: 6px; font-family: monospace; }
                .footer   { background: #1e1508; padding: 16px; text-align: center;
                             color: #6b5030; font-size: 12px; border-top: 1px solid #4a3510; }
                .warning  { color: #c07020; font-size: 13px; margin-top: 16px; }
              </style>
            </head>
            <body>
              <div class='container'>
                <div class='header'>
                  <h1>⚔️ Warrior and Wealth ⚔️</h1>
                  <p>Il gioco di strategia medievale</p>
                </div>
                <div class='body'>
                  {BODY_CONTENT}
                </div>
                <div class='footer'>
                  © Warrior and Wealth — Email automatica, non rispondere a questo messaggio.
                </div>
              </div>
            </body>
            </html>";

        // ══════════════════════════════════════════════════════════════════════
        // METODO PRINCIPALE — invio generico via SMTP
        // ══════════════════════════════════════════════════════════════════════

        public static async Task<bool> SendEmailAsync(string toEmail, string toName, string subject, string htmlBody)
        {
            try
            {
                var html = HTML_TEMPLATE.Replace("{BODY_CONTENT}", htmlBody);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(SENDER_NAME, SENDER_EMAIL));
                message.To.Add(new MailboxAddress(toName, toEmail));
                message.Subject = subject;
                message.Body = new BodyBuilder { HtmlBody = html }.ToMessageBody();

                // Nome completo qualificato: evita l'ambiguità con System.Net.Mail.SmtpClient
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Timeout = 15000; // 15 secondi

                await smtp.ConnectAsync(SMTP_HOST, SMTP_PORT, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(Password.SMTP_USER, Password.SMTP_PASS);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                Log($"[EMAIL] Inviata a {toEmail} — {subject}");
                return true;
            }
            catch (Exception ex)
            {
                Log($"[EMAIL][ERRORE] {ex.Message}");
                return false;
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        // RECUPERO PASSWORD — OTP
        // ══════════════════════════════════════════════════════════════════════

        public static async Task<string?> SendPasswordRecoveryAsync( string toEmail, string username, string code)
        {
            var subject = "⚔️ Warrior and Wealth — Codice per reimpostare la password";

            var body = $@"
            <h2>Reimposta la tua password</h2>
            <p>Ciao <strong>{EscapeHtml(username)}</strong>,</p>
            <p>Abbiamo ricevuto una richiesta di recupero della password per il tuo account di
            <em>Warrior and Wealth</em>.</p>
            <p>Inserisci questo codice nel gioco per scegliere una nuova password:</p>
            <div class='code-box'>
              <span>{EscapeHtml(code)}</span>
            </div>
            <p>Il codice è valido per <strong>15 minuti</strong> e può essere usato una sola volta.</p>
            <p><strong>Non condividere mai questo codice con nessuno.</strong> Il nostro team
            non ti chiederà mai di comunicarlo.</p>
            <p class='warning'>⚠️ Non hai richiesto tu il recupero? Puoi ignorare questa email: finché non
            userai il codice, la tua password resterà invariata. Se ricevi più richieste che non hai fatto
            tu, scrivici a <strong>support@warriorsandwealth.com</strong>.</p>";

            var ok = await SendEmailAsync(toEmail, username, subject, body);
            return ok ? code : null;
        }

        // ══════════════════════════════════════════════════════════════════════
        // BENVENUTO
        // ══════════════════════════════════════════════════════════════════════

        public static Task<bool> SendWelcomeAsync(string toEmail, string username)
        {
            var subject = "⚔️ Benvenuto in Warrior and Wealth!";
            var body = $@"
            <h2>Benvenuto nel regno, {EscapeHtml(username)}!</h2>
            <p>Il tuo account è stato creato con successo e le porte del castello sono ora aperte per te.</p>
            <p>D'ora in avanti il destino delle tue terre è nelle tue mani: costruisci il tuo regno,
            recluta eserciti valorosi e accumula ricchezze per imporre il tuo dominio sul mondo medievale.</p>
            <p>Ricorda che ogni grande impero è nato da piccoli inizi. Pianifica con astuzia,
            scegli bene i tuoi alleati e non sottovalutare mai i tuoi rivali.</p>
            <p>Se hai bisogno di aiuto o vuoi segnalarci qualcosa, il nostro team è a tua disposizione
            all'indirizzo <strong>support@warriorsandwealth.com</strong>.</p>
            <p>Che la fortuna ti accompagni, valoroso condottiero! ⚔️</p>
            <p><em>Il team di Warrior and Wealth</em></p>";

            return SendEmailAsync(toEmail, username, subject, body);
        }

        // ══════════════════════════════════════════════════════════════════════
        // NOTIFICA LOGIN
        // ══════════════════════════════════════════════════════════════════════

        public static Task<bool> SendLoginAlertAsync(string toEmail,string username,string ipAddress)
        {
            var subject = "⚠️ Warrior and Wealth — Nuovo accesso al tuo account";
            var body = $@"
            <h2>Nuovo accesso rilevato</h2>
            <p>Ciao <strong>{EscapeHtml(username)}</strong>,</p>
            <p>Abbiamo registrato un accesso al tuo account di <em>Warrior and Wealth</em> con questi dettagli:</p>
            <div class='code-box'>
              <span style='font-size:20px'>{EscapeHtml(ipAddress)}</span>
            </div>
            <p>Indirizzo IP e orario dell'accesso: <strong>{DateTime.UtcNow:dd/MM/yyyy HH:mm} UTC</strong></p>
            <p><strong>Sei stato tu?</strong> Non devi fare nulla: puoi ignorare questa email.</p>
            <p class='warning'>⚠️ <strong>Non riconosci questo accesso?</strong> Cambia subito la password
            del tuo account e scrivici a <strong>support@warriorsandwealth.com</strong>,
            così potremo aiutarti a mettere in sicurezza il tuo regno.</p>";

            return SendEmailAsync(toEmail, username, subject, body);
        }

        // ══════════════════════════════════════════════════════════════════════
        // UTILITÀ PRIVATE
        // ══════════════════════════════════════════════════════════════════════

        private static string EscapeHtml(string s) => s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        private static void Log(string msg) => Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {msg}");
    }
}