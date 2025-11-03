using Strategico_V2;

namespace CriptoGame_Online.GUI
{
    public partial class Velocizza_Diamanti : Form
    {
        public static string tipo = "";
        public Velocizza_Diamanti()
        {
            InitializeComponent();

            if (tipo == "Costruzione")
                btn_Velocizza.Text = "Velocizza Costruzione";
            if (tipo == "Reclutamento")
                btn_Velocizza.Text = "Velocizza Reclutamento";
            if (tipo == "Ricerca")
                btn_Velocizza.Text = "Velocizza Ricerca";

            txt_Testo.BackColor = Color.FromArgb(229, 208, 181);
            txt_Diamond_Blu.BackColor = Color.FromArgb(229, 208, 181);
            pictureBox_Meno.BackColor = Color.FromArgb(229, 208, 181);
            pictureBox_Più.BackColor = Color.FromArgb(229, 208, 181);
            ico_12.BackColor = Color.FromArgb(229, 208, 181);

            this.ActiveControl = btn_Velocizza; // assegna il focus al bottone
        }

        private void pictureBox_Più_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Variabili_Client.Utente_Risorse.Diamond_Blu) > Convert.ToInt32(txt_Diamond_Blu.Text))
                txt_Diamond_Blu.Text = (Convert.ToInt32(txt_Diamond_Blu.Text) + 1).ToString();
        }

        private void pictureBox_Meno_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txt_Diamond_Blu.Text) > 0)
                txt_Diamond_Blu.Text = (Convert.ToInt32(txt_Diamond_Blu.Text) - 1).ToString();
        }

        private void btn_Velocizza_Click(object sender, EventArgs e)
        {
            if (tipo == "Costruzione")
                ClientConnection.TestClient.Send($"Velocizza_Diamanti|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Costruzione|{txt_Diamond_Blu.Text}");
            if (tipo == "Reclutamento")
                ClientConnection.TestClient.Send($"Velocizza_Diamanti|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Reclutamento|{txt_Diamond_Blu.Text}");
            if (tipo == "Ricerca")
                ClientConnection.TestClient.Send($"Velocizza_Diamanti|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Ricerca|{txt_Diamond_Blu.Text}");
        }

        private void Velocizza_Diamanti_Load(object sender, EventArgs e)
        {

        }
    }
}
