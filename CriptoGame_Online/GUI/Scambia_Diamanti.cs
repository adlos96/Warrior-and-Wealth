using Strategico_V2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warrior_and_Wealth.GUI
{
    public partial class Scambia_Diamanti : Form
    {
        public Scambia_Diamanti()
        {
            InitializeComponent();
            this.ActiveControl = btn_Scambia; // assegna il focus al bottone

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            txt_Testo.BackColor = Color.FromArgb(229, 208, 181);
            txt_Diamond_Blu.BackColor = Color.FromArgb(229, 208, 181);
            txt_Diamond_Viola.BackColor = Color.FromArgb(229, 208, 181);
            ico_11.BackColor = Color.FromArgb(229, 208, 181);
            ico_12.BackColor = Color.FromArgb(229, 208, 181);
        }

        private  async void btn_Scambia_Click(object sender, EventArgs e)
        {
            this.ActiveControl = ico_12;

            // Messaggio di conferma chiaro
            var result = MessageBox.Show(
                $"Sei sicuro di voler scambiare i diamanti viola?\n" +
                $"Diamanti Viola: {txt_Diamond_Viola.Text}\n" +
                $"Diamanti Blu: {txt_Diamond_Blu.Text}" +
                $"Diamanti attuali: {Variabili_Client.Utente_Risorse.Diamond_Viola}",
                "Conferma scambio",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                // Esegui l'acquisto
                ClientConnection.TestClient.Send($"Scambia_Diamanti|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{txt_Diamond_Viola.Text}");
                btn_Scambia.Enabled = false;
                await Sleep();
                btn_Scambia.Enabled = true;
            }
        }

        async Task Sleep()
        {
            await Task.Delay(5000);
        }

        private void pictureBox_Più_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Variabili_Client.Utente_Risorse.Diamond_Viola.Replace(".", "")) > Convert.ToInt32(txt_Diamond_Viola.Text))
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (Control.ModifierKeys & Keys.Shift) == Keys.Shift) // controlla se Ctrl è premuto al momento del click
                    txt_Diamond_Viola.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) + 50).ToString();
                else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    txt_Diamond_Viola.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) + 10).ToString();
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                    txt_Diamond_Viola.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) + 5).ToString();
                else
                    txt_Diamond_Viola.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) + 1).ToString();

            if (Convert.ToInt32(txt_Diamond_Viola.Text) > Convert.ToInt32(Variabili_Client.Utente_Risorse.Diamond_Viola.Replace(".", "")))
                txt_Diamond_Viola.Text = Variabili_Client.Utente_Risorse.Diamond_Viola;

            txt_Diamond_Blu.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) * Convert.ToInt32(Variabili_Client.D_Viola_D_Blu)).ToString();
        }

        private void pictureBox_Meno_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txt_Diamond_Viola.Text) > 0)
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (Control.ModifierKeys & Keys.Shift) == Keys.Shift) // controlla se Ctrl è premuto al momento del click
                    txt_Diamond_Viola.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) - 50).ToString();
                else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    txt_Diamond_Viola.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) - 10).ToString();
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                    txt_Diamond_Viola.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) - 5).ToString();
                else
                    txt_Diamond_Viola.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) - 1).ToString();
            if (Convert.ToInt32(txt_Diamond_Viola.Text) < 0) txt_Diamond_Viola.Text = "0";
            txt_Diamond_Blu.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) * Convert.ToInt32(Variabili_Client.D_Viola_D_Blu)).ToString();
        }

        private void Scambia_Diamanti_Load(object sender, EventArgs e)
        {

        }
    }
}
