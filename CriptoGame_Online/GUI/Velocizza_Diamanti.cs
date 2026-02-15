using Strategico_V2;

namespace CriptoGame_Online.GUI
{
    public partial class Velocizza_Diamanti : Form
    {
        public static string tipo = "";
        public Velocizza_Diamanti()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

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
            if (Convert.ToInt32(Variabili_Client.Utente_Risorse.Diamond_Blu.Replace(".", "")) > Convert.ToInt32(txt_Diamond_Blu.Text))
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control) // controlla se Ctrl è premuto al momento del click
                    txt_Diamond_Blu.Text = (Convert.ToInt32(txt_Diamond_Blu.Text) + 5).ToString();
                else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    txt_Diamond_Blu.Text = (Convert.ToInt32(txt_Diamond_Blu.Text) + 10).ToString();
                else txt_Diamond_Blu.Text = (Convert.ToInt32(txt_Diamond_Blu.Text) + 1).ToString();

            if (Convert.ToInt32(txt_Diamond_Blu.Text) > Convert.ToInt32(Variabili_Client.Utente_Risorse.Diamond_Blu.Replace(".", "")))
                txt_Diamond_Blu.Text = Variabili_Client.Utente_Risorse.Diamond_Blu;
        }

        private void pictureBox_Meno_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txt_Diamond_Blu.Text) > 0)
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control) // controlla se Ctrl è premuto al momento del click
                    txt_Diamond_Blu.Text = (Convert.ToInt32(txt_Diamond_Blu.Text) - 5).ToString();
                else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    txt_Diamond_Blu.Text = (Convert.ToInt32(txt_Diamond_Blu.Text) - 10).ToString();
                else
                    txt_Diamond_Blu.Text = (Convert.ToInt32(txt_Diamond_Blu.Text) - 1).ToString();
            if (Convert.ToInt32(txt_Diamond_Blu.Text) < 0) txt_Diamond_Blu.Text = "0";
        }           

        private async void btn_Velocizza_Click(object sender, EventArgs e)
        {
            this.ActiveControl = ico_12; // assegna il focus al bottone

            // Messaggio di conferma chiaro
            var result = MessageBox.Show(
                $"Sei sicuro di voler utilizzare i diamanti blu?\n" +
                $"Diamanti Blu: {txt_Diamond_Blu.Text}\n" +
                $"Diamanti attuali: {Variabili_Client.Utente_Risorse.Diamond_Blu}\n",
                "Conferma velocizzazione",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                // Esegui l'acquisto
                if (tipo == "Costruzione")
                    ClientConnection.TestClient.Send($"Velocizza_Diamanti|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Costruzione|{txt_Diamond_Blu.Text}");
                if (tipo == "Reclutamento")
                    ClientConnection.TestClient.Send($"Velocizza_Diamanti|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Reclutamento|{txt_Diamond_Blu.Text}");
                if (tipo == "Ricerca")
                    ClientConnection.TestClient.Send($"Velocizza_Diamanti|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Ricerca|{txt_Diamond_Blu.Text}");
                btn_Velocizza.Enabled = false;
                await Sleep();
                btn_Velocizza.Enabled = true;
            }
        }

        async Task Sleep()
        {
            await Task.Delay(5000);
        }

        private void Velocizza_Diamanti_Load(object sender, EventArgs e)
        {
            txt_Diamond_Blu.Text = "0";
        }
    }
}
