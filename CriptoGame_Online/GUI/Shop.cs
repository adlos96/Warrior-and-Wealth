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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Warrior_and_Wealth
{
    public partial class Shop : Form
    {
        int pagina = 1; // pagina iniziale
        public static CustomToolTip toolTip1;
        public Shop()
        {
            InitializeComponent();

            toolTip1 = new CustomToolTip();

            // Imposta qualche proprietà opzionale
            toolTip1.InitialDelay = 150;
            toolTip1.AutoPopDelay = 15000;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            txt_Image_1.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Image_2.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Image_3.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Image_4.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Image_5.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Image_6.ForeColor = Color.FromArgb(205, 175, 0);

            txt_Image_1.BackColor = Color.FromArgb(91, 45, 45);
            txt_Image_2.BackColor = Color.FromArgb(91, 45, 45);
            txt_Image_3.BackColor = Color.FromArgb(91, 45, 45);
            txt_Image_4.BackColor = Color.FromArgb(91, 45, 45);
            txt_Image_5.BackColor = Color.FromArgb(91, 45, 45);
            txt_Image_6.BackColor = Color.FromArgb(91, 45, 45);

            txt_Shop_1.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Shop_2.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Shop_3.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Shop_4.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Shop_5.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Shop_6.ForeColor = Color.FromArgb(205, 175, 0);

            txt_Shop_1.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Shop_2.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Shop_3.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Shop_4.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Shop_5.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Shop_6.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);

            txt_Shop_1.BackColor = Color.FromArgb(124, 62, 63);
            txt_Shop_2.BackColor = Color.FromArgb(124, 62, 63);
            txt_Shop_3.BackColor = Color.FromArgb(124, 62, 63);
            txt_Shop_4.BackColor = Color.FromArgb(124, 62, 63);
            txt_Shop_5.BackColor = Color.FromArgb(124, 62, 63);
            txt_Shop_6.BackColor = Color.FromArgb(124, 62, 63);

            txt_Pacchetto_Desc_1.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Pacchetto_Desc_2.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Pacchetto_Desc_3.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Pacchetto_Desc_4.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Pacchetto_Desc_5.ForeColor = Color.FromArgb(205, 175, 0);
            txt_Pacchetto_Desc_6.ForeColor = Color.FromArgb(205, 175, 0);

            txt_Pacchetto_Desc_1.BackColor = Color.FromArgb(64, 29, 28);
            txt_Pacchetto_Desc_2.BackColor = Color.FromArgb(64, 29, 28);
            txt_Pacchetto_Desc_3.BackColor = Color.FromArgb(64, 29, 28);
            txt_Pacchetto_Desc_4.BackColor = Color.FromArgb(64, 29, 28);
            txt_Pacchetto_Desc_5.BackColor = Color.FromArgb(64, 29, 28);
            txt_Pacchetto_Desc_6.BackColor = Color.FromArgb(64, 29, 28);

            panel_Diamond_Image_1.BackColor = Color.Transparent;
            panel_Diamond_Image_2.BackColor = Color.Transparent;
            panel_Diamond_Image_3.BackColor = Color.Transparent;
            panel_Diamond_Image_4.BackColor = Color.Transparent;
            panel_Diamond_Image_5.BackColor = Color.Transparent;
            panel_Diamond_Image_6.BackColor = Color.Transparent;

            panel_Image_1.BackColor = Color.Transparent;
            panel_Image_2.BackColor = Color.Transparent;
            panel_Image_3.BackColor = Color.Transparent;
            panel_Image_4.BackColor = Color.Transparent;
            panel_Image_5.BackColor = Color.Transparent;
            panel_Image_6.BackColor = Color.Transparent;

            Update_UI();
        }

        private void Shop_Load(object sender, EventArgs e)
        {
            this.ActiveControl = panel_Diamond_Image_2; // assegna il focus al bottone
            panel1.BackColor = Color.FromArgb(100, 229, 208, 181);
            panel1.BackColor = Color.Transparent;
            Update_UI();
        }

        private void panel_Prossimo_Click(object sender, EventArgs e)
        {
            if (pagina >= 1 && pagina < 3)
                pagina++;
            Update_UI();
        }

        private void panel_Precedente_Click(object sender, EventArgs e)
        {
            if (pagina > 1 && pagina <= 3)
                pagina--;
            Update_UI();
        }
        void Update_UI()
        {
            if (pagina == 1)
            {
                txt_Pacchetto_Desc_1.Visible = true;
                txt_Pacchetto_Desc_2.Visible = true;
                txt_Pacchetto_Desc_3.Visible = false;
                txt_Pacchetto_Desc_4.Visible = false;
                txt_Pacchetto_Desc_5.Visible = false;
                txt_Pacchetto_Desc_6.Visible = false;

                panel_Bottone_1.Enabled = true;
                panel_Bottone_2.Enabled = true;
                panel_Bottone_3.Enabled = true;
                panel_Bottone_4.Enabled = true;
                panel_Bottone_5.Enabled = true;
                panel_Bottone_6.Enabled = true;

                panel_Image_1.BackgroundImage = Properties.Resources.Vip_Photoroom_1_; //Immagine principale
                panel_Image_2.BackgroundImage = Properties.Resources.Vip_Photoroom_1_;
                panel_Image_3.BackgroundImage = Properties.Resources.diamond_2;
                panel_Image_4.BackgroundImage = Properties.Resources.diamond_2;
                panel_Image_5.BackgroundImage = Properties.Resources.diamond_2;
                panel_Image_6.BackgroundImage = Properties.Resources.diamond_2;

                panel_Diamond_Image_1.BackgroundImage = Properties.Resources.diamond_2; //Immagine costo
                panel_Diamond_Image_2.BackgroundImage = Properties.Resources.USDT_Logo_removebg_preview;
                panel_Diamond_Image_3.BackgroundImage = Properties.Resources.USDT_Logo_removebg_preview;
                panel_Diamond_Image_4.BackgroundImage = Properties.Resources.USDT_Logo_removebg_preview;
                panel_Diamond_Image_5.BackgroundImage = Properties.Resources.USDT_Logo_removebg_preview;
                panel_Diamond_Image_6.BackgroundImage = Properties.Resources.USDT_Logo_removebg_preview;

                txt_Shop_1.Text = Variabili_Client.Shop.Vip_1.Costo.ToString(); //Costo effettivo
                txt_Shop_2.Text = Variabili_Client.Shop.Vip_2.Costo.ToString();
                txt_Shop_3.Text = Variabili_Client.Shop.Pacchetto_Diamanti_1.Costo.ToString();
                txt_Shop_4.Text = Variabili_Client.Shop.Pacchetto_Diamanti_2.Costo.ToString();
                txt_Shop_5.Text = Variabili_Client.Shop.Pacchetto_Diamanti_3.Costo.ToString();
                txt_Shop_6.Text = Variabili_Client.Shop.Pacchetto_Diamanti_4.Costo.ToString();

                txt_Image_1.Text = Variabili_Client.Shop.Vip_1.Reward.ToString() + " H"; //Reward all'acquisto
                txt_Image_2.Text = Variabili_Client.Shop.Vip_2.Reward.ToString() + " H";
                txt_Image_3.Text = Variabili_Client.Shop.Pacchetto_Diamanti_1.Reward.ToString();
                txt_Image_4.Text = Variabili_Client.Shop.Pacchetto_Diamanti_2.Reward.ToString();
                txt_Image_5.Text = Variabili_Client.Shop.Pacchetto_Diamanti_3.Reward.ToString();
                txt_Image_6.Text = Variabili_Client.Shop.Pacchetto_Diamanti_4.Reward.ToString();

                txt_Pacchetto_Desc_1.Text = "VIP";
                txt_Pacchetto_Desc_2.Text = "VIP";

                toolTip1.SetToolTip(this.panel_Image_1, Variabili_Client.Shop.Vip_1.desc);
                toolTip1.SetToolTip(this.panel_Image_2, Variabili_Client.Shop.Vip_2.desc);
            }
            if (pagina == 2)
            {
                txt_Pacchetto_Desc_1.Visible = true;
                txt_Pacchetto_Desc_2.Visible = true;
                txt_Pacchetto_Desc_3.Visible = true;
                txt_Pacchetto_Desc_4.Visible = true;
                txt_Pacchetto_Desc_5.Visible = true;
                txt_Pacchetto_Desc_6.Visible = true;

                panel_Bottone_1.Enabled = true;
                panel_Bottone_2.Enabled = true;
                panel_Bottone_3.Enabled = true;
                panel_Bottone_4.Enabled = true;
                panel_Bottone_5.Enabled = true;
                panel_Bottone_6.Enabled = true;

                panel_Image_1.BackgroundImage = Properties.Resources.Costruttori_24H;
                panel_Image_2.BackgroundImage = Properties.Resources.Costruttori_48H;
                panel_Image_3.BackgroundImage = Properties.Resources.Addestratori_24H_removebg_preview;
                panel_Image_4.BackgroundImage = Properties.Resources.Addestratori_48H_removebg_preview;
                panel_Image_5.BackgroundImage = Properties.Resources.Scudo_Pace_1;
                panel_Image_6.BackgroundImage = Properties.Resources.Scudo_Pace_1;

                panel_Image_1.BackgroundImageLayout = ImageLayout.Zoom;
                panel_Image_2.BackgroundImageLayout = ImageLayout.Zoom;
                panel_Image_3.BackgroundImageLayout = ImageLayout.Zoom;
                panel_Image_4.BackgroundImageLayout = ImageLayout.Zoom;
                panel_Image_5.BackgroundImageLayout = ImageLayout.Zoom;
                panel_Image_6.BackgroundImageLayout = ImageLayout.Zoom;

                panel_Diamond_Image_1.BackgroundImage = Properties.Resources.diamond_1;
                panel_Diamond_Image_2.BackgroundImage = Properties.Resources.diamond_1;
                panel_Diamond_Image_3.BackgroundImage = Properties.Resources.diamond_1;
                panel_Diamond_Image_4.BackgroundImage = Properties.Resources.diamond_1;
                panel_Diamond_Image_5.BackgroundImage = Properties.Resources.diamond_1;
                panel_Diamond_Image_6.BackgroundImage = Properties.Resources.diamond_1;

                txt_Shop_1.Text = Variabili_Client.Shop.Costruttore_24h.Costo.ToString(); //Costo effettivo
                txt_Shop_2.Text = Variabili_Client.Shop.Costruttore_48h.Costo.ToString();
                txt_Shop_3.Text = Variabili_Client.Shop.Reclutatore_24h.Costo.ToString();
                txt_Shop_4.Text = Variabili_Client.Shop.Reclutatore_24h.Costo.ToString();
                txt_Shop_5.Text = Variabili_Client.Shop.Scudo_Pace_8h.Costo.ToString();
                txt_Shop_6.Text = Variabili_Client.Shop.Scudo_Pace_24h.Costo.ToString();

                txt_Image_1.Text = Variabili_Client.Shop.Costruttore_24h.Reward.ToString() + " H"; //Reward all'acquisto
                txt_Image_2.Text = Variabili_Client.Shop.Costruttore_48h.Reward.ToString() + " H";
                txt_Image_3.Text = Variabili_Client.Shop.Reclutatore_24h.Reward.ToString() + " H";
                txt_Image_4.Text = Variabili_Client.Shop.Reclutatore_48h.Reward.ToString() + " H";
                txt_Image_5.Text = Variabili_Client.Shop.Scudo_Pace_8h.Reward.ToString() + " H";
                txt_Image_6.Text = Variabili_Client.Shop.Scudo_Pace_24h.Reward.ToString() + " H";

                txt_Pacchetto_Desc_1.Text = "Costruttori";
                txt_Pacchetto_Desc_2.Text = "Costruttori";
                txt_Pacchetto_Desc_3.Text = "Reclutatori";
                txt_Pacchetto_Desc_4.Text = "Reclutatori";
                txt_Pacchetto_Desc_5.Text = "Scudo della pace";
                txt_Pacchetto_Desc_6.Text = "Scudo della pace";

                toolTip1.SetToolTip(this.panel_Image_1, Variabili_Client.Shop.Costruttore_24h.desc);
                toolTip1.SetToolTip(this.panel_Image_2, Variabili_Client.Shop.Costruttore_48h.desc);
                toolTip1.SetToolTip(this.panel_Image_3, Variabili_Client.Shop.Reclutatore_24h.desc);
                toolTip1.SetToolTip(this.panel_Image_4, Variabili_Client.Shop.Reclutatore_48h.desc);
                toolTip1.SetToolTip(this.panel_Image_5, Variabili_Client.Shop.Scudo_Pace_8h.desc);
                toolTip1.SetToolTip(this.panel_Image_6, Variabili_Client.Shop.Scudo_Pace_24h.desc);
            }
            if (pagina == 3)
            {
                txt_Pacchetto_Desc_2.Visible = true;
                txt_Pacchetto_Desc_3.Visible = true;
                txt_Pacchetto_Desc_4.Visible = true;
                txt_Pacchetto_Desc_5.Visible = true;
                txt_Pacchetto_Desc_6.Visible = true;

                panel_Bottone_1.Enabled = true;
                panel_Bottone_2.Enabled = true;
                panel_Bottone_3.Enabled = false;
                panel_Bottone_4.Enabled = false;
                panel_Bottone_5.Enabled = false;
                panel_Bottone_6.Enabled = false;

                panel_Image_1.BackgroundImage = Properties.Resources.Scudo_Pace_1;
                panel_Image_2.BackgroundImage = Properties.Resources.GamePass_Base;
                panel_Image_3.BackgroundImage = Properties.Resources.GamePass_Avanzato;
                panel_Image_4.BackgroundImage = Properties.Resources.Pacchetto_Risorse;
                panel_Image_5.BackgroundImage = null;
                panel_Image_6.BackgroundImage = null;

                panel_Image_1.BackgroundImageLayout = ImageLayout.Zoom;
                panel_Image_2.BackgroundImageLayout = ImageLayout.Zoom;
                panel_Image_3.BackgroundImageLayout = ImageLayout.Zoom;
                panel_Image_4.BackgroundImageLayout = ImageLayout.Zoom;
                panel_Image_5.BackgroundImageLayout = ImageLayout.Zoom;
                panel_Image_6.BackgroundImageLayout = ImageLayout.Zoom;

                panel_Diamond_Image_1.BackgroundImage = Properties.Resources.diamond_1;
                panel_Diamond_Image_2.BackgroundImage = Properties.Resources.USDT_Logo_removebg_preview;
                panel_Diamond_Image_3.BackgroundImage = Properties.Resources.USDT_Logo_removebg_preview;
                panel_Diamond_Image_4.BackgroundImage = Properties.Resources.diamond_1;
                panel_Diamond_Image_5.BackgroundImage = null;
                panel_Diamond_Image_6.BackgroundImage = null;

                txt_Shop_1.Text = Variabili_Client.Shop.Scudo_Pace_72h.Costo.ToString(); //Costo effettivo
                txt_Shop_2.Text = Variabili_Client.Shop.GamePass_Base.Costo.ToString();
                txt_Shop_3.Text = Variabili_Client.Shop.GamePass_Avanzato.Costo.ToString();
                txt_Shop_4.Text = "";
                txt_Shop_5.Text = "";
                txt_Shop_6.Text = "";

                txt_Image_1.Text = Variabili_Client.Shop.Scudo_Pace_72h.Reward.ToString() + " H"; //Reward all'acquisto
                txt_Image_2.Text = Variabili_Client.Shop.GamePass_Base.Reward.ToString();
                txt_Image_3.Text = Variabili_Client.Shop.GamePass_Avanzato.Reward.ToString();
                txt_Image_4.Text = "";
                txt_Image_5.Text = "";
                txt_Image_6.Text = "";

                txt_Pacchetto_Desc_1.Text = "Scudo della pace";
                txt_Pacchetto_Desc_2.Text = "GamePass Silver";
                txt_Pacchetto_Desc_3.Text = "GamePass Gold";
                txt_Pacchetto_Desc_4.Text = "";
                txt_Pacchetto_Desc_5.Text = "";
                txt_Pacchetto_Desc_6.Text = "";

                toolTip1.SetToolTip(this.panel_Image_1, Variabili_Client.Shop.Scudo_Pace_72h.desc);
                toolTip1.SetToolTip(this.panel_Image_2, Variabili_Client.Shop.GamePass_Base.desc);
                toolTip1.SetToolTip(this.panel_Image_3, Variabili_Client.Shop.GamePass_Avanzato.desc);
            }
        }

        private async void panel_Bottone_1_MouseClick(object sender, MouseEventArgs e)
        {
            // Messaggio di conferma chiaro
            var result = MessageBox.Show(
                $"Sei sicuro di voler acquistare questo oggetto?\n",
                $"Conferma acquisto",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (pagina == 1 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip_1");
            
            if (pagina == 2 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Costruttori_24H");
            
            if (pagina == 3 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Scudo_Pace_72H");
            
            panel_Bottone_1.Enabled = false;
            await Sleep(2); // meglio di Thread.Sleep
            panel_Bottone_1.Enabled = true;
        }
        private async void panel_Bottone_2_MouseClick(object sender, MouseEventArgs e)
        {
            var result = MessageBox.Show(
                $"Sei sicuro di voler acquistare questo oggetto?\n",
                $"Conferma acquisto",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (pagina == 1 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip_2");

            if (pagina == 2 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Costruttori_48H");
            if (pagina == 3)

            panel_Bottone_2.Enabled = false;
            await Sleep(2); // meglio di Thread.Sleep
            panel_Bottone_2.Enabled = true;
        }
        private async void panel_Bottone_3_MouseClick(object sender, MouseEventArgs e)
        {
            var result = MessageBox.Show(
                $"Sei sicuro di voler acquistare questo oggetto?\n",
                $"Conferma acquisto",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (pagina == 1 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Pacchetto_1");

            if (pagina == 2 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Reclutatori_24H");
            
            
            panel_Bottone_3.Enabled = false;
            await Sleep(2); // meglio di Thread.Sleep
            panel_Bottone_3.Enabled = true;
        }

        private async void panel_Bottone_4_MouseClick(object sender, MouseEventArgs e)
        {
            var result = MessageBox.Show(
                $"Sei sicuro di voler acquistare questo oggetto?\n",
                $"Conferma acquisto",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (pagina == 1 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Pacchetto_2");

            if (pagina == 2 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Reclutatori_48H");


            panel_Bottone_4.Enabled = false;
            await Sleep(2); // meglio di Thread.Sleep
            panel_Bottone_4.Enabled = true;
        }

        private async void panel_Bottone_5_MouseClick(object sender, MouseEventArgs e)
        {
            var result = MessageBox.Show(
                $"Sei sicuro di voler acquistare questo oggetto?\n",
                $"Conferma acquisto",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (pagina == 1 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Pacchetto_3");

            if (pagina == 2 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Scudo_Pace_8H");

            
            panel_Bottone_5.Enabled = false;
            await Sleep(2); // meglio di Thread.Sleep
            panel_Bottone_5.Enabled = true;
        }

        private async void panel_Bottone_6_MouseClick(object sender, MouseEventArgs e)
        {
            var result = MessageBox.Show(
                $"Sei sicuro di voler acquistare questo oggetto?\n",
                $"Conferma acquisto",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (pagina == 1 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Pacchetto_4");
            if (pagina == 2 && result == DialogResult.Yes)
                    ClientConnection.TestClient.Send($"Shop|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Scudo_Pace_24H");

            panel_Bottone_6.Enabled = false;
            await Sleep(2); // meglio di Thread.Sleep
            panel_Bottone_6.Enabled = true;
        }

        public static async Task<bool> Sleep(int secondi)
        {
            Task.Delay(1000 * secondi);
            return true;
        }
    }
}
