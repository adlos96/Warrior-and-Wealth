
using Warrior_and_Wealth.Strumenti;
using Strategico_V2;
using System.Diagnostics;

namespace Warrior_and_Wealth
{
    public partial class Login : Form
    {
        public static string login_data = "";
        static bool avviso_Aggiornamento = false;
        public Login()
        {
            InitializeComponent();
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Size = new Size(251, 331);
        }

        private void Gioco_Load(object sender, EventArgs e)
        {
            GameAudio.PlayMenuMusic("Login");
            MusicManager.SetVolume(0.3f);

            // Resto
            this.ActiveControl = Btn_Login; // assegna il focus al bottone
            panel1.BackColor = Color.FromArgb(100, 229, 208, 181);
            banner_1.BackColor = Color.FromArgb(100, 229, 208, 181);
            banner_2.BackColor = Color.FromArgb(100, 229, 208, 181);
            banner_1.BringToFront();
            banner_2.BringToFront();


            lbl_Titolo.Font = new Font("Cinzel Decorative", 12, FontStyle.Bold);
            lbl_Username_Login.Font = new Font("Cinzel Decorative", 9, FontStyle.Regular);
            lbl_Password_Login.Font = new Font("Cinzel Decorative", 9, FontStyle.Regular);

            // btn_Login
            Btn_Login.BackColor = Color.FromArgb(100, 229, 208, 181);
            Btn_Login.Font = new Font("Old English Text MT", 9, FontStyle.Bold);

            // Btn_New_Game
            Btn_New_Game.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            Btn_New_Game.BackColor = Color.FromArgb(100, 229, 208, 181);

            // txt_Username
            txt_Username_Login.BackColor = Color.FromArgb(229, 208, 181);
            txt_Username_Login.Text = "Inserisci Nome utente";
            txt_Username_Login.Font = new Font("Cinzel Decorative", 9, FontStyle.Regular);

            // txt_Password
            txt_Password_Login.BackColor = Color.FromArgb(229, 208, 181);
            txt_Password_Login.Text = "Inserisci Password";
            txt_Password_Login.Font = new Font("Cinzel Decorative", 9, FontStyle.Regular);

            txt_Ip.BackColor = Color.FromArgb(229, 208, 181);
            txt_Ip.Text = "IP: AUTO";
            txt_Ip.Font = new Font("Cinzel Decorative", 9, FontStyle.Regular);

            txt_Log.BackColor = Color.FromArgb(229, 208, 181);
            txt_Log.Text = "LOG";
            txt_Log.ForeColor = Color.Black;
            txt_Log.Font = new Font("Cinzel Decorative", 8, FontStyle.Regular);

            txt_Versione_Attuale.BackColor = Color.FromArgb(229, 208, 181);
            txt_Versione_Attuale.Text = "Versione attuale: " + Variabili_Client.versione_Client_Attuale;
            txt_Versione_Attuale.ForeColor = Color.Black;
            txt_Versione_Attuale.Font = new Font("Cinzel Decorative", 8, FontStyle.Regular);

        }

        private void txt_Username_Login_MouseClick(object sender, MouseEventArgs e)
        {
            txt_Username_Login.Text = "";
            txt_Username_Login.ForeColor = Color.Black;
        }

        private void txt_Password_Login_MouseClick(object sender, MouseEventArgs e)
        {
            txt_Password_Login.Text = "";
            txt_Password_Login.ForeColor = Color.Black;
        }

        private void txt_Username_Login_TextChanged(object sender, EventArgs e)
        {
            txt_Username_Login.ForeColor = Color.Black;
        }

        private void txt_Password_Login_TextChanged(object sender, EventArgs e)
        {
            txt_Password_Login.ForeColor = Color.Black;
        }

        private void txt_Ip_MouseClick(object sender, MouseEventArgs e)
        {
            txt_Ip.Text = "";
            txt_Ip.ForeColor = Color.Black;
        }

        private async void Btn_Login_Click(object sender, EventArgs e)
        {
            txt_Log.Font = new Font("Cinzel Decorative", 8, FontStyle.Regular);
            this.ActiveControl = lbl_Titolo;
            Btn_Login.Enabled = false;
            Btn_New_Game.Enabled = false;
            txt_Log.Text = "Connessione...";

            //Controlla se siamo in locale... 
            if (txt_Ip.Text != "IP: AUTO") ClientConnection.TestClient._ServerIp = txt_Ip.Text;
            else
            {
                string subjectName = Environment.MachineName; //Ottine il nome della macchina (hostname)
                if (subjectName == "DESKTOP-DOBLVTI" || subjectName == "ADLO") ClientConnection.TestClient._ServerIp = "localhost";
            }


            await ClientConnection.TestClient.InitializeClient(); // Connessione server
            await Sleep(1);
            if (!await VersioneDisponibile()) return;
            
            if (txt_Username_Login.Text == "Inserisci Nome utente")
            {
                txt_Log.Text = "Inserisci un nome utente valido!";
                Btn_Login.Enabled = true;
                Btn_New_Game.Enabled = true;
                txt_Log.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);
                return;
            }
            else if (txt_Password_Login.Text == "Inserisci Password")
            {
                txt_Log.Text = "Inserisci una password valida!";
                Btn_Login.Enabled = true;
                Btn_New_Game.Enabled = true;
                txt_Log.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);
                return;
            }

            string username = txt_Username_Login.Text;
            string password = txt_Password_Login.Text;

            //username = "adlos";
            //password = "123";

            await Sleep(2);
            txt_Log.Text = "Login...";
            await Sleep(2);
            ClientConnection.TestClient.Send($"Login|{username}|{password}");
            await Loop_Login(4);
            await Sleep(2);

            if (Variabili_Client.Utente.User_Login == true)
            {
                Variabili_Client.Utente.Username = username;
                Variabili_Client.Utente.Password = password;
                this.DialogResult = DialogResult.OK; // Se il login riesce
            }
            else
            {
                Btn_Login.Enabled = true;
                Btn_New_Game.Enabled = true;
            }
            if (login_data != "") txt_Log.Text = login_data;
        }
        async Task<bool> VersioneDisponibile()
        {
            if (Variabili_Client.versione_Client_Necessario != Variabili_Client.versione_Client_Attuale)
            {
                var versioneNecessaria = Variabili_Client.versione_Client_Necessario.Split('.');
                var versioneAttuale = Variabili_Client.versione_Client_Attuale.Split('.');

                if (versioneNecessaria[0] != versioneAttuale[0] || versioneNecessaria[1] != versioneAttuale[1] || versioneNecessaria[2] != versioneAttuale[2])
                {
                    Btn_Login.Enabled = false;
                    Btn_New_Game.Enabled = false;
                    lbl_Aggiornamento_Disponibile.Visible = true;
                    btn_Aggiorna.Visible = true;

                    lbl_Aggiornamento_Disponibile.Text = "Necessario aggiornamento: " + Variabili_Client.versione_Client_Necessario;
                    this.Size = new Size(251, 377);
                    return false;
                }
                else if (versioneNecessaria[3] != versioneAttuale[3])
                {
                    lbl_Aggiornamento_Disponibile.Visible = true;
                    btn_Aggiorna.Visible = true;

                    lbl_Aggiornamento_Disponibile.Text = "Disponibile aggiornamento: " + Variabili_Client.versione_Client_Necessario;
                    this.Size = new Size(251, 377);
                    if (!avviso_Aggiornamento)
                    {
                        avviso_Aggiornamento = true;
                        return false;
                    }
                    return true;
                }
            }
            else if (Variabili_Client.versione_Client_Necessario == Variabili_Client.versione_Client_Attuale) return true;
            return false;
        }

        public static async Task<bool> Sleep(int secondi)
        {
            await Task.Delay(1000 * secondi);
            return true;
        }
        public async Task<bool> Loop_Login(int tentativi_Max)
        {
            int tentativi = 1;
            while (Variabili_Client.Utente.User_Login == false)
            {
                if (tentativi >= tentativi_Max) return false;
                txt_Log.Text = $"Tentativo Login... [{tentativi}/{tentativi_Max}]";
                await Task.Delay(2000);
                tentativi++;
            }
            if (Variabili_Client.Utente.User_Login == true)
                txt_Log.Text = $"Login completato con successo, buon game!";
            else txt_Log.Text = $"Login fallito!";
            return true;
        }

        private async void Btn_New_Game_Click(object sender, EventArgs e)
        {
            txt_Log.Font = new Font("Cinzel Decorative", 8, FontStyle.Regular);
            this.ActiveControl = lbl_Titolo;
            Btn_New_Game.Enabled = false;
            Btn_Login.Enabled = false;
            txt_Log.Text = "Connessione...";

            if (txt_Ip.Text != "IP: AUTO") ClientConnection.TestClient._ServerIp = txt_Ip.Text;
            await ClientConnection.TestClient.InitializeClient(); // Connessione server
            await Sleep(1);
            if (!await VersioneDisponibile()) return;

            if (txt_Username_Login.Text == "Inserisci Nome utente")
            {
                txt_Log.Text = "Inserisci un nome utente valido!";
                Btn_Login.Enabled = true;
                Btn_New_Game.Enabled = true;
                txt_Log.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);
                return;
            }
            else if (txt_Password_Login.Text == "Inserisci Password")
            {
                txt_Log.Text = "Inserisci una password valida!";
                Btn_Login.Enabled = true;
                Btn_New_Game.Enabled = true;
                txt_Log.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);
                return;
            }

            await Sleep(2);
            txt_Log.Text = "Contattando il server...";
            await Sleep(2);
            ClientConnection.TestClient.Send($"New Player|{txt_Username_Login.Text}|{txt_Password_Login.Text}");
            await Sleep(2);

            if (Variabili_Client.Utente.User_Login == true)
            {
                Variabili_Client.Utente.Username = txt_Username_Login.Text;
                Variabili_Client.Utente.Password = txt_Password_Login.Text;
                this.DialogResult = DialogResult.OK; // Se il login riesce
            }
            else
            {
                Btn_Login.Enabled = true;
                Btn_New_Game.Enabled = true;
            }
            if (login_data != "") txt_Log.Text = login_data;
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            MusicManager.Stop();
        }

        private void btn_Aggiorna_Click(object sender, EventArgs e)
        {

            // Apre la pagina GitHub Releases nel browser
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/adlos96/Warrior-and-Wealth/releases/latest",
                UseShellExecute = true
            });
            Close();
        }
    }
}
