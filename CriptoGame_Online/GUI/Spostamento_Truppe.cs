
using Strategico_V2;

namespace CriptoGame_Online.GUI
{
    public partial class Spostamento_Truppe : Form
    {
        public static string struttura = "";
        static int livello_Unità = 1;
        private CancellationTokenSource cts = new CancellationTokenSource();
        public Spostamento_Truppe()
        {
            InitializeComponent();
            lbl_Struttura.Text = struttura;

            btn_I_Esercito.Enabled = false;

            btn_I_Struttura.Enabled = true;
            btn_II_Struttura.Enabled = false;
            btn_III_Struttura.Enabled = false;
            btn_IV_Struttura.Enabled = false;
            btn_V_Struttura.Enabled = false;

            txt_Guerriero_Esercito.Text = Variabili_Client.Citta.Ingresso.Guerrieri_1.ToString();
            txt_Lanciere_Esercito.Text = Variabili_Client.Citta.Ingresso.Lanceri_1.ToString();
            txt_Arciere_Esercito.Text = Variabili_Client.Citta.Ingresso.Arceri_1.ToString();
            txt_Catapulta_Esercito.Text = Variabili_Client.Citta.Ingresso.Catapulte_1.ToString();
        }

        private void Spostamento_Truppe_Load(object sender, EventArgs e)
        {
            Task.Run(() => Gui_Update(cts.Token), cts.Token);
        }

        async void Gui_Update(CancellationToken token)
        {
            // Mappa tra nome struttura e oggetto corrispondente
            var strutture = new Dictionary<string, dynamic>
            {
                ["Ingresso"] = Variabili_Client.Citta.Ingresso,
                ["Mura"] = Variabili_Client.Citta.Mura,
                ["Cancello"] = Variabili_Client.Citta.Cancello,
                ["Torri"] = Variabili_Client.Citta.Torri,
                ["Città"] = Variabili_Client.Citta.Città,
                ["Castello"] = Variabili_Client.Citta.Castello
            };

            while (!token.IsCancellationRequested)
            {
                if (panel1.IsHandleCreated && !panel1.IsDisposed)
                {
                    panel1.BeginInvoke((Action)(() =>
                    {
                        if (!strutture.TryGetValue(struttura, out var s))
                            return; // struttura non trovata

                        int livello = livello_Unità switch // Determina il livello e accedi dinamicamente
                        {
                            1 => 1,
                            2 => 2,
                            3 => 3,
                            4 => 4,
                            5 => 5,
                            _ => 1
                        };

                        string suffix = $"_{livello}"; // Costruisce dinamicamente il nome del campo
                        void SetText(TextBox box, string baseName) // Usa reflection per leggere i campi
                        {
                            var prop = s.GetType().GetProperty(baseName + suffix);
                            if (prop != null)
                            {
                                var value = prop.GetValue(s)?.ToString() ?? "0";
                                box.Text = value;
                            }
                        }

                        SetText(txt_Guerriero_Struttura, "Guerrieri");
                        SetText(txt_Lanciere_Struttura, "Lanceri");
                        SetText(txt_Arciere_Struttura, "Arceri");
                        SetText(txt_Catapulta_Struttura, "Catapulte");

                        int g = 0;
                        int l = 0;
                        int a = 0;
                        int c = 0;

                        if (livello_Unità == 1)
                        {
                            g = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                            l = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                            a = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                            c = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                            txt_Guerriero_Esercito.Text = g.ToString();
                            txt_Lanciere_Esercito.Text  = l.ToString();
                            txt_Arciere_Esercito.Text   = a.ToString();
                            txt_Catapulta_Esercito.Text = c.ToString();

                            trackBar_Guerriero.Maximum  = g;
                            trackBar_Lanciere.Maximum   = l;
                            trackBar_Arciere.Maximum    = a;
                            trackBar_Catapulta.Maximum  = c;

                            lbl_Guerriero.Text = g.ToString();
                            lbl_Lanciere.Text  = l.ToString();
                            lbl_Arciere.Text   = a.ToString();
                            lbl_Catapulta.Text = c.ToString();
                        }
                        if (livello_Unità == 2)
                        {
                            g = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                            l = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                            a = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                            c = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);
                        }
                        if (livello_Unità == 3)
                        {
                            g = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                            l = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                            a = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                            c = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);
                        }
                        if (livello_Unità == 4)
                        {
                            g = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                            l = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                            a = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                            c = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);
                        }
                        if (livello_Unità == 5)
                        {
                            g = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                            l = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                            a = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                            c = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);
                        }
                        txt_Guerriero_Esercito.Text = g.ToString();
                        txt_Lanciere_Esercito.Text = l.ToString();
                        txt_Arciere_Esercito.Text = a.ToString();
                        txt_Catapulta_Esercito.Text = c.ToString();

                        trackBar_Guerriero.Maximum = g;
                        trackBar_Lanciere.Maximum = l;
                        trackBar_Arciere.Maximum = a;
                        trackBar_Catapulta.Maximum = c;

                        lbl_Guerriero.Text = trackBar_Guerriero.Value.ToString();
                        lbl_Lanciere.Text = trackBar_Lanciere.Value.ToString();
                        lbl_Arciere.Text = trackBar_Arciere.Value.ToString();
                        lbl_Catapulta.Text = trackBar_Catapulta.Value.ToString();
                    }));
                }
                await Task.Delay(250); // meglio di Thread.Sleep
            }

        }

        private void btn_Spostamento_Truppe_Click(object sender, EventArgs e)
        {
            int guerriero = Convert.ToInt32(txt_Guerriero_Esercito.Text);
            ClientConnection.TestClient.Send($"SpostamentoTruppe|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Addestramento");
        }

        private void btn_I_Esercito_Click(object sender, EventArgs e)
        {
            livello_Unità = 1;
            btn_I_Esercito.BackColor = Color.DimGray;
            btn_II_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_III_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_IV_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_V_Esercito.BackColor = Color.FromArgb(32, 36, 47);

            btn_I_Struttura.BackColor = Color.DimGray;
            btn_II_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_III_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_IV_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_V_Struttura.BackColor = Color.FromArgb(32, 36, 47);

            btn_I_Esercito.Enabled = false;
            btn_II_Esercito.Enabled = true;
            btn_III_Esercito.Enabled = true;
            btn_IV_Esercito.Enabled = true;
            btn_V_Esercito.Enabled = true;

            btn_I_Struttura.Enabled = true;
            btn_II_Struttura.Enabled = false;
            btn_III_Struttura.Enabled = false;
            btn_IV_Struttura.Enabled = false;
            btn_V_Struttura.Enabled = false;
        }

        private void btn_II_Esercito_Click(object sender, EventArgs e)
        {
            livello_Unità = 2;
            btn_I_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_II_Esercito.BackColor = Color.DimGray;
            btn_III_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_IV_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_V_Esercito.BackColor = Color.FromArgb(32, 36, 47);

            btn_I_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_II_Struttura.BackColor = Color.DimGray;
            btn_III_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_IV_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_V_Struttura.BackColor = Color.FromArgb(32, 36, 47);

            btn_I_Esercito.Enabled = true;
            btn_II_Esercito.Enabled = false;
            btn_III_Esercito.Enabled = true;
            btn_IV_Esercito.Enabled = true;
            btn_V_Esercito.Enabled = true;

            btn_I_Struttura.Enabled = false;
            btn_II_Struttura.Enabled = true;
            btn_III_Struttura.Enabled = false;
            btn_IV_Struttura.Enabled = false;
            btn_V_Struttura.Enabled = false;
        }

        private void btn_III_Esercito_Click(object sender, EventArgs e)
        {
            livello_Unità = 3;
            btn_I_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_II_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_III_Esercito.BackColor = Color.DimGray;
            btn_IV_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_V_Esercito.BackColor = Color.FromArgb(32, 36, 47);

            btn_I_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_II_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_III_Struttura.BackColor = Color.DimGray;
            btn_IV_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_V_Struttura.BackColor = Color.FromArgb(32, 36, 47);

            btn_I_Esercito.Enabled = true;
            btn_II_Esercito.Enabled = true;
            btn_III_Esercito.Enabled = false;
            btn_IV_Esercito.Enabled = true;
            btn_V_Esercito.Enabled = true;

            btn_I_Struttura.Enabled = false;
            btn_II_Struttura.Enabled = false;
            btn_III_Struttura.Enabled = true;
            btn_IV_Struttura.Enabled = false;
            btn_V_Struttura.Enabled = false;
        }

        private void btn_IV_Esercito_Click(object sender, EventArgs e)
        {
            livello_Unità = 4;
            btn_I_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_II_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_III_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_IV_Esercito.BackColor = Color.DimGray;
            btn_V_Esercito.BackColor = Color.FromArgb(32, 36, 47);

            btn_I_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_II_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_III_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_IV_Struttura.BackColor = Color.DimGray;
            btn_V_Struttura.BackColor = Color.FromArgb(32, 36, 47);

            btn_I_Esercito.Enabled = true;
            btn_II_Esercito.Enabled = true;
            btn_III_Esercito.Enabled = true;
            btn_IV_Esercito.Enabled = false;
            btn_V_Esercito.Enabled = true;

            btn_I_Struttura.Enabled = false;
            btn_II_Struttura.Enabled = false;
            btn_III_Struttura.Enabled = false;
            btn_IV_Struttura.Enabled = true;
            btn_V_Struttura.Enabled = false;
        }

        private void btn_V_Esercito_Click(object sender, EventArgs e)
        {
            livello_Unità = 5;
            btn_I_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_II_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_III_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_IV_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_V_Esercito.BackColor = Color.DimGray;

            btn_I_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_II_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_III_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_IV_Struttura.BackColor = Color.FromArgb(32, 36, 47);
            btn_V_Struttura.BackColor = Color.DimGray;

            btn_I_Esercito.Enabled = true;
            btn_II_Esercito.Enabled = true;
            btn_III_Esercito.Enabled = true;
            btn_IV_Esercito.Enabled = true;
            btn_V_Esercito.Enabled = false;

            btn_I_Struttura.Enabled = false;
            btn_II_Struttura.Enabled = false;
            btn_III_Struttura.Enabled = false;
            btn_IV_Struttura.Enabled = false;
            btn_V_Struttura.Enabled = true;
        }
    }
}
