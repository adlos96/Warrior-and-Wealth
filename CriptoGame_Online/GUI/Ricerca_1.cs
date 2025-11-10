using CriptoGame_Online.GUI;
using Strategico_V2;

namespace CriptoGame_Online
{
    public partial class Ricerca_1 : Form
    {
        private CancellationTokenSource cts = new CancellationTokenSource();

        public Ricerca_1()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }
        private void Ricerca_1_Load(object sender, EventArgs e)
        {
            btn_Costruzione.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Risorse.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Addestramento.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Popolazione.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);

            btn_Livello_Guerrieri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Attacco_Guerrieri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Difesa_Guerrieri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Salute_Guerrieri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);

            btn_Livello_Lanceri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Attacco_Lanceri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Difesa_Lanceri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Salute_Lanceri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);

            btn_Livello_Arceri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Attacco_Arceri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Difesa_Arceri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Salute_Arceri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);

            btn_Livello_Catapulte.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Attacco_Catapulte.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Difesa_Catapulte.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Salute_Catapulte.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);

            btn_Guarnigione_Castello.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Difesa_Castello.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Salute_Castello.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);

            btn_Guarnigione_Torri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Difesa_Torri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Salute_Torri.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);

            btn_Guarnigione_Mura.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Difesa_Mura.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Salute_Mura.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);

            btn_Guarnigione_Cancello.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Difesa_Cancello.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Salute_Cancello.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);

            btn_Guarnigione_Ingresso.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);
            btn_Guarnigione_Citta.Font = new Font("Cinzel Decorative", 6.75f, FontStyle.Bold);

            Task.Run(() => Gui_Update(cts.Token), cts.Token);
        }
        async void Gui_Update(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (btn_Addestramento.IsHandleCreated && !btn_Addestramento.IsDisposed)
                {
                    btn_Addestramento.BeginInvoke((Action)(() =>
                    {
                        lbl_Tempo_Ricerca.Text = "Tempo ricerca: " + Variabili_Client.Utente.Tempo_Ricerca + "s";

                        if (lbl_Tempo_Ricerca.Text == "Tempo ricerca: 00:00:00s")
                            pictureBox_Speed.Visible = false;
                        else
                            pictureBox_Speed.Visible = true;

                        btn_Livello_Guerrieri.Text = $"Livello: {Variabili_Client.Utente_Ricerca.Livello_Spadaccini}";
                        btn_Attacco_Guerrieri.Text = $"Attacco: {Variabili_Client.Utente_Ricerca.Attacco_Spadaccini}";
                        btn_Difesa_Guerrieri.Text = $"Difesa: {Variabili_Client.Utente_Ricerca.Difesa_Spadaccini}";
                        btn_Salute_Guerrieri.Text = $"Salute: {Variabili_Client.Utente_Ricerca.Salute_Spadaccini}";

                        btn_Livello_Lanceri.Text = $"Livello: {Variabili_Client.Utente_Ricerca.Livello_Lanceri}";
                        btn_Attacco_Lanceri.Text = $"Attacco: {Variabili_Client.Utente_Ricerca.Attacco_Lanceri}";
                        btn_Difesa_Lanceri.Text = $"Difesa: {Variabili_Client.Utente_Ricerca.Difesa_Lanceri}";
                        btn_Salute_Lanceri.Text = $"Salute: {Variabili_Client.Utente_Ricerca.Salute_Lanceri}";

                        btn_Livello_Arceri.Text = $"Livello: {Variabili_Client.Utente_Ricerca.Livello_Arceri}";
                        btn_Attacco_Arceri.Text = $"Attacco: {Variabili_Client.Utente_Ricerca.Attacco_Arceri}";
                        btn_Difesa_Arceri.Text = $"Difesa: {Variabili_Client.Utente_Ricerca.Difesa_Arceri}";
                        btn_Salute_Arceri.Text = $"Salute: {Variabili_Client.Utente_Ricerca.Salute_Arceri}";

                        btn_Livello_Catapulte.Text = $"Livello: {Variabili_Client.Utente_Ricerca.Livello_Catapulte}";
                        btn_Attacco_Catapulte.Text = $"Attacco: {Variabili_Client.Utente_Ricerca.Attacco_Catapulte}";
                        btn_Difesa_Catapulte.Text = $"Difesa: {Variabili_Client.Utente_Ricerca.Difesa_Catapulte}";
                        btn_Salute_Catapulte.Text = $"Salute: {Variabili_Client.Utente_Ricerca.Salute_Catapulte}";

                        btn_Costruzione.Text = $"Costruzione: {Variabili_Client.Utente_Ricerca.Ricerca_Costruzione}";
                        btn_Risorse.Text = $"Produzione: {Variabili_Client.Utente_Ricerca.Ricerca_Produzione}";
                        btn_Addestramento.Text = $"Addestramento: {Variabili_Client.Utente_Ricerca.Ricerca_Addestramento}";
                        btn_Popolazione.Text = $"Popolazione: {Variabili_Client.Utente_Ricerca.Ricerca_Popolazione}";

                        btn_Guarnigione_Ingresso.Text = $"Guarnigione: {Variabili_Client.Utente_Ricerca.Ricerca_Ingresso_Guarnigione}";
                        btn_Guarnigione_Citta.Text = $"Guernigione: {Variabili_Client.Utente_Ricerca.Ricerca_Citta_Guarnigione}";

                        btn_Salute_Cancello.Text = $"Salute: {Variabili_Client.Utente_Ricerca.Ricerca_Cancello_Salute}";
                        btn_Difesa_Cancello.Text = $"Difesa: {Variabili_Client.Utente_Ricerca.Ricerca_Cancello_Difesa}";
                        btn_Guarnigione_Cancello.Text = $"Guarnigione: {Variabili_Client.Utente_Ricerca.Ricerca_Cancello_Guarnigione}";

                        btn_Salute_Mura.Text = $"Salute: {Variabili_Client.Utente_Ricerca.Ricerca_Mura_Salute}";
                        btn_Difesa_Mura.Text = $"Difesa: {Variabili_Client.Utente_Ricerca.Ricerca_Mura_Difesa}";
                        btn_Guarnigione_Mura.Text = $"Guarnigione: {Variabili_Client.Utente_Ricerca.Ricerca_Mura_Guarnigione}";

                        btn_Salute_Torri.Text = $"Salute: {Variabili_Client.Utente_Ricerca.Ricerca_Torri_Salute}";
                        btn_Difesa_Torri.Text = $"Difesa: {Variabili_Client.Utente_Ricerca.Ricerca_Torri_Difesa}";
                        btn_Guarnigione_Torri.Text = $"Guarnigione: {Variabili_Client.Utente_Ricerca.Ricerca_Torri_Guarnigione}";

                        btn_Salute_Castello.Text = $"Salute: {Variabili_Client.Utente_Ricerca.Ricerca_Castello_Salute}";
                        btn_Difesa_Castello.Text = $"Difesa: {Variabili_Client.Utente_Ricerca.Ricerca_Castello_Difesa}";
                        btn_Guarnigione_Castello.Text = $"Guarnigione: {Variabili_Client.Utente_Ricerca.Ricerca_Castello_Guarnigione}";

                        if (Variabili_Client.Utente.Ricerca_Attiva == false)
                        {
                            btn_Livello_Guerrieri.Enabled = true;
                            btn_Attacco_Guerrieri.Enabled = true;
                            btn_Difesa_Guerrieri.Enabled = true;
                            btn_Salute_Guerrieri.Enabled = true;

                            btn_Livello_Lanceri.Enabled = true;
                            btn_Attacco_Lanceri.Enabled = true;
                            btn_Difesa_Lanceri.Enabled = true;
                            btn_Salute_Lanceri.Enabled = true;

                            btn_Livello_Arceri.Enabled = true;
                            btn_Attacco_Arceri.Enabled = true;
                            btn_Difesa_Arceri.Enabled = true;
                            btn_Salute_Arceri.Enabled = true;

                            btn_Livello_Catapulte.Enabled = true;
                            btn_Attacco_Catapulte.Enabled = true;
                            btn_Difesa_Catapulte.Enabled = true;
                            btn_Salute_Catapulte.Enabled = true;

                            btn_Costruzione.Enabled = true;
                            btn_Risorse.Enabled = true;
                            btn_Addestramento.Enabled = true;
                            btn_Popolazione.Enabled = true;

                            btn_Guarnigione_Citta.Enabled = true;
                            btn_Guarnigione_Ingresso.Enabled = true;

                            btn_Salute_Cancello.Enabled = true;
                            btn_Difesa_Cancello.Enabled = true;
                            btn_Guarnigione_Cancello.Enabled = true;

                            btn_Salute_Mura.Enabled = true;
                            btn_Difesa_Mura.Enabled = true;
                            btn_Guarnigione_Mura.Enabled = true;

                            btn_Salute_Torri.Enabled = true;
                            btn_Difesa_Torri.Enabled = true;
                            btn_Guarnigione_Torri.Enabled = true;

                            btn_Salute_Castello.Enabled = true;
                            btn_Difesa_Castello.Enabled = true;
                            btn_Guarnigione_Castello.Enabled = true;
                        }
                        else
                        {
                            btn_Livello_Guerrieri.Enabled = false;
                            btn_Attacco_Guerrieri.Enabled = false;
                            btn_Difesa_Guerrieri.Enabled = false;
                            btn_Salute_Guerrieri.Enabled = false;

                            btn_Livello_Lanceri.Enabled = false;
                            btn_Attacco_Lanceri.Enabled = false;
                            btn_Difesa_Lanceri.Enabled = false;
                            btn_Salute_Lanceri.Enabled = false;

                            btn_Livello_Arceri.Enabled = false;
                            btn_Attacco_Arceri.Enabled = false;
                            btn_Difesa_Arceri.Enabled = false;
                            btn_Salute_Arceri.Enabled = false;

                            btn_Livello_Catapulte.Enabled = false;
                            btn_Attacco_Catapulte.Enabled = false;
                            btn_Difesa_Catapulte.Enabled = false;
                            btn_Salute_Catapulte.Enabled = false;

                            btn_Costruzione.Enabled = false;
                            btn_Risorse.Enabled = false;
                            btn_Addestramento.Enabled = false;
                            btn_Popolazione.Enabled = false;

                            btn_Guarnigione_Citta.Enabled = false;
                            btn_Guarnigione_Ingresso.Enabled = false;

                            btn_Salute_Cancello.Enabled = false;
                            btn_Difesa_Cancello.Enabled = false;
                            btn_Guarnigione_Cancello.Enabled = false;

                            btn_Salute_Mura.Enabled = false;
                            btn_Difesa_Mura.Enabled = false;
                            btn_Guarnigione_Mura.Enabled = false;

                            btn_Salute_Torri.Enabled = false;
                            btn_Difesa_Torri.Enabled = false;
                            btn_Guarnigione_Torri.Enabled = false;

                            btn_Salute_Castello.Enabled = false;
                            btn_Difesa_Castello.Enabled = false;
                            btn_Guarnigione_Castello.Enabled = false;
                        }

                        if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Costruzione" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Costruzione.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Produzione" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Risorse.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Addestramento" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Addestramento.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Popolazione" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Popolazione.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;

                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Livello_Guerrieri" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Livello_Guerrieri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Salute_Guerrieri" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Salute_Guerrieri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Difesa_Guerrieri" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Difesa_Guerrieri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Attacco_Guerrieri" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Attacco_Guerrieri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;

                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Livello_Lanceri" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Livello_Lanceri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Salute_Lanceri" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Salute_Lanceri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Difesa_Lanceri" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Difesa_Lanceri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Attacco_Lanceri" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Attacco_Lanceri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;

                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Livello_Arceri" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Livello_Arceri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Salute_Arceri" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Salute_Arceri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Difesa_Arceri" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Difesa_Arceri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Attacco_Arceri" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Attacco_Arceri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;

                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Livello_Catapulte" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Livello_Catapulte.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Salute_Catapulte" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Salute_Catapulte.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Difesa_Catapulte" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Difesa_Catapulte.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Attacco_Catapulte" && Variabili_Client.Utente.Ricerca_Attiva == true)
                            panel_Attacco_Catapulte.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;


                        if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Costruzione" && Variabili_Client.Utente.Ricerca_Attiva == false)
                            panel_Costruzione.BackgroundImage = Properties.Resources.Bottone___Sfondo_1_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Produzione" && Variabili_Client.Utente.Ricerca_Attiva == false)
                            panel_Risorse.BackgroundImage = Properties.Resources.Bottone___Sfondo_1_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Addestramento" && Variabili_Client.Utente.Ricerca_Attiva == false)
                            panel_Addestramento.BackgroundImage = Properties.Resources.Bottone___Sfondo_1_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Popolazione" && Variabili_Client.Utente.Ricerca_Attiva == false)
                            panel_Popolazione.BackgroundImage = Properties.Resources.Bottone___Sfondo_1_A_removebg_preview;

                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Livello_Guerrieri" && Variabili_Client.Utente.Ricerca_Attiva == false)
                            panel_Livello_Guerrieri.BackgroundImage = Properties.Resources.Bottone___Sfondo_1_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Salute_Guerrieri" && Variabili_Client.Utente.Ricerca_Attiva == false)
                            panel_Salute_Guerrieri.BackgroundImage = Properties.Resources.Bottone___Sfondo_1_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Difesa_Guerrieri" && Variabili_Client.Utente.Ricerca_Attiva == false)
                            panel_Difesa_Guerrieri.BackgroundImage = Properties.Resources.Bottone___Sfondo_1_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Attacco_Guerrieri" && Variabili_Client.Utente.Ricerca_Attiva == false)
                            panel_Attacco_Guerrieri.BackgroundImage = Properties.Resources.Bottone___Sfondo_1_A_removebg_preview;

                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Livello_Lanceri" && Variabili_Client.Utente.Ricerca_Attiva == false)
                            panel_Livello_Lanceri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Salute_Lanceri" && Variabili_Client.Utente.Ricerca_Attiva == false)
                            panel_Salute_Lanceri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Difesa_Lanceri" && Variabili_Client.Utente.Ricerca_Attiva == false)
                            panel_Difesa_Lanceri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                        else if (Variabili_Client.Ricerca_1_Bottone_Cliccato == "btn_Attacco_Lanceri" && Variabili_Client.Utente.Ricerca_Attiva == false)
                            panel_Attacco_Lanceri.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
                    }));
                }
                await Task.Delay(1000); // meglio di Thread.Sleep
            }

        }
        private void panel_Sfondo_Scroll(object sender, ScrollEventArgs e)
        {
            //Rectangle visibile = new Rectangle(
            //    panel_Sfondo.AutoScrollPosition.X * -1,
            //    panel_Sfondo.AutoScrollPosition.Y * -1,
            //    panel_Sfondo.ClientSize.Width,
            //    panel_Sfondo.ClientSize.Height);
            //
            //foreach (Control ctrl in panel_Sfondo.Controls)
            //{
            //    ctrl.Visible = visibile.IntersectsWith(new Rectangle(ctrl.Location, ctrl.Size));
            //}
            //panel_Costruzione.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
        }
        private void Ricerca_1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts.Cancel();
        }
        private void pictureBox_Speed_Click(object sender, EventArgs e)
        {
            Velocizza_Diamanti form_Gioco = new Velocizza_Diamanti();
            Velocizza_Diamanti.tipo = "Ricerca";
            form_Gioco.ShowDialog();
        }
        private void bnt_Costruzione_Click(object sender, EventArgs e)
        {
            panel_Costruzione.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Costruzione");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Costruzione";
        }
        private void btn_Risorse_Click(object sender, EventArgs e)
        {
            panel_Risorse.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Produzione");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Produzione";
        }
        private void btn_Addestramento_Click(object sender, EventArgs e)
        {
            panel_Addestramento.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Addestramento");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Addestramento";
        }
        private void btn_Popolazione_Click(object sender, EventArgs e)
        {
            panel_Popolazione.BackgroundImage = Properties.Resources.Bottone___Sfondo_2_A_removebg_preview;
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Popolazione");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Popolazione";
        }

        private void btn_Livello_Guerrieri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Livello|Guerriero");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Livello_Guerrieri";
        }
        private void btn_Attacco_Guerrieri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Attacco|Guerriero");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Attacco_Guerrieri";
        }
        private void btn_Difesa_Guerrieri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Difesa|Guerriero");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Difesa_Guerrieri";
        }
        private void btn_Salute_Guerrieri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Salute|Guerriero");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Salute_Guerrieri";
        }

        private void btn_Attacco_Lanceri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Attacco|Lanceri");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Attacco_Lanceri";
        }
        private void btn_Difesa_Lanceri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Difesa|Lanceri");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Difesa_Lanceri";
        }
        private void btn_Salute_Lanceri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Salute|Lanceri");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Salute_Lanceri";
        }
        private void btn_Livello_Lanceri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Livello|Lanceri");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Livello_Lanceri";
        }

        private void btn_Attacco_Arceri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Attacco|Arceri");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Attacco_Arceri";
        }
        private void btn_Difesa_Arceri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Difesa|Arceri");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Difesa_Arceri";
        }
        private void btn_Salute_Arceri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Salute|Arceri");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Salute_Arceri";
        }
        private void btn_Livello_Arceri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Livello|Arceri");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Livello_Arceri";
        }

        private void btn_Attacco_Catapulte_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Attacco|Catapulte");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Attacco_Catapulte";
        }
        private void btn_Difesa_Catapulte_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Difesa|Catapulte");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Difesa_Catapulte";
        }
        private void btn_Salute_Catapulte_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Salute|Catapulte");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Salute_Catapulte";
        }
        private void btn_Livello_Catapulte_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Truppe|Livello|Catapulte");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Livello_Catapulte";
        }

        private void btn_Guarnigione_Ingresso_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Guarnigione|Ingresso");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Guarnigione_Ingresso";
        }
        private void btn_Guarnigione_Citta_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Guarnigione|Citta");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Guarnigione_Citta";
        }

        private void btn_Salute_Castello_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Salute|Castello");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Salute_Ingresso";
        }
        private void btn_Difesa_Castello_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Difesa|Castello");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Difesa_Ingresso";
        }
        private void btn_Guarnigione_Castello_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Guarnigione|Castello");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Guarnigione_Ingresso";
        }

        private void btn_Salute_Torri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Salute|Torri");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Salute_Ingresso";
        }
        private void btn_Difesa_Torri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Difesa|Torri");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Difesa_Ingresso";
        }
        private void btn_Guarnigione_Torri_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Guarnigione|Torri");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Guarnigione_Ingresso";
        }

        private void btn_Salute_Mura_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Salute|Mura");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Salute_Ingresso";
        }
        private void btn_Difesa_Mura_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Difesa|Mura");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Difesa_Ingresso";
        }
        private void btn_Guarnigione_Mura_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Guarnigione|Mura");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Guarnigione_Ingresso";
        }

        private void btn_Salute_Cancello_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Salute|Cancello");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Salute_Ingresso";
        }
        private void btn_Difesa_Cancello_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Difesa|Cancello");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Difesa_Ingresso";
        }
        private void btn_Guarnigione_Cancello_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ricerca|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta|Guarnigione|Cancello");
            Variabili_Client.Ricerca_1_Bottone_Cliccato = "btn_Guarnigione_Ingresso";
        }
    }
}
