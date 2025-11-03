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

namespace CriptoGame_Online.GUI
{
    public partial class Scambia_Diamanti : Form
    {
        public Scambia_Diamanti()
        {
            InitializeComponent();
            this.ActiveControl = btn_Scambia; // assegna il focus al bottone
        }

        private void btn_Scambia_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Scambia_Diamanti|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{txt_Diamond_Viola.Text}");
        }

        private void pictureBox_Più_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Variabili_Client.Utente_Risorse.Diamond_Viola) > Convert.ToInt32(txt_Diamond_Viola.Text))
                txt_Diamond_Viola.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) + 1).ToString();

            txt_Diamond_Blu.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) * Convert.ToInt32(Variabili_Client.D_Viola_D_Blu)).ToString();
        }

        private void pictureBox_Meno_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txt_Diamond_Viola.Text) > 0 )
            {
                txt_Diamond_Viola.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) - 1).ToString();
                txt_Diamond_Blu.Text = (Convert.ToInt32(txt_Diamond_Viola.Text) * Convert.ToInt32(Variabili_Client.D_Viola_D_Blu)).ToString();
            }
        }
    }
}
