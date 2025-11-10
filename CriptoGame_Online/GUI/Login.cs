
using Strategico_V2;

namespace CriptoGame_Online
{
    public partial class Login : Form
    {
        public static string login_data = "";
        public Login()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void Gioco_Load(object sender, EventArgs e)
        {
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
            txt_Username_Login.Text = "Username";
            txt_Username_Login.ForeColor = Color.Gray;
            txt_Username_Login.Font = new Font("Cinzel Decorative", 9, FontStyle.Regular);

            // txt_Password
            txt_Password_Login.BackColor = Color.FromArgb(229, 208, 181);
            txt_Password_Login.Text = "Password";
            txt_Password_Login.ForeColor = Color.Gray;
            txt_Password_Login.Font = new Font("Cinzel Decorative", 9, FontStyle.Regular);

            txt_Ip.BackColor = Color.FromArgb(229, 208, 181);
            txt_Ip.Text = "IP: AUTO";
            txt_Ip.ForeColor = Color.Gray;
            txt_Ip.Font = new Font("Cinzel Decorative", 9, FontStyle.Regular);

            txt_Log.BackColor = Color.FromArgb(229, 208, 181);
            txt_Log.Text = "LOG";
            txt_Log.ForeColor = Color.Gray;
            txt_Log.Font = new Font("Cinzel Decorative", 8, FontStyle.Regular);

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

        private void txt_Log_MouseClick(object sender, MouseEventArgs e)
        {
            txt_Log.Text = "";
            txt_Log.ForeColor = Color.Black;
        }

        private async void Btn_Login_Click(object sender, EventArgs e)
        {
            Btn_Login.Enabled = false;
            Btn_New_Game.Enabled = false;
            txt_Log.Text = "Connessione...";
            ClientConnection.TestClient.InitializeClient(); // Connessione server

            if (txt_Ip.Text != "IP: AUTO")
                ClientConnection.TestClient._ServerIp = txt_Ip.Text;

            string username = txt_Username_Login.Text;
            string password = txt_Password_Login.Text;

            username = "adlos";
            password = "123";

            await Sleep(2);
            txt_Log.Text = "Login...";
            await Sleep(2);
            ClientConnection.TestClient.Send($"Login|{username}|{password}");
            await Loop_Login(5);
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
            Btn_New_Game.Enabled = false;
            Btn_Login.Enabled = false;
            txt_Log.Text = "Connessione...";
            ClientConnection.TestClient.InitializeClient(); // Connessione server

            if (txt_Ip.Text != "IP: AUTO")
                ClientConnection.TestClient._ServerIp = txt_Ip.Text;

            await Sleep(2);
            txt_Log.Text = "Cotattando il server...";
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
    }
}
