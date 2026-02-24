using Warrior_and_Wealth.Strumenti;
using Strategico_V2;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Warrior_and_Wealth.GUI
{
    public partial class Citta_V2 : Form
    {
        private StatBar barravitaCancello;
        private StatBar defenseBarCancello;
        private StatBar soldierBarCancello;

        private StatBar barravitaMura;
        private StatBar defenseBarMura;
        private StatBar soldierBarMura;

        private StatBar barravitaTorri;
        private StatBar defenseBarTorri;
        private StatBar soldierBarTorri;

        private StatBar barravitaCastello;
        private StatBar defenseBarCastello;
        private StatBar soldierBarCastello;

        private StatBar soldierBarIngresso;
        private StatBar soldierBarCitta;

        private Dictionary<StatBar, int> lastValues = new Dictionary<StatBar, int>();
        private Dictionary<StatBar, int> lastValuesMax = new Dictionary<StatBar, int>();
        private List<(StatBar bar, Panel panel)> barreCitta;

        private CancellationTokenSource cts = new CancellationTokenSource();

        public static bool[] tutorial = new bool[2];

        public Citta_V2()
        {
            InitializeComponent();

            txt_Testo.BackColor = Color.FromArgb(235, 221, 192);
            txt_Testo.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            barravitaCancello = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "HP",
                BarColor = Color.LimeGreen,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Cancello.Salute_Max,
                Value = Variabili_Client.Citta.Cancello.Salute,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };
            defenseBarCancello = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "DEF",
                BarColor = Color.SteelBlue,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Cancello.Difesa_Max,
                Value = Variabili_Client.Citta.Cancello.Difesa,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };
            soldierBarCancello = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "Guarnigione",
                BarColor = Color.OrangeRed,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Cancello.Guarnigione_Max,
                Value = Variabili_Client.Citta.Cancello.Guarnigione,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };

            barravitaMura = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "HP",
                BarColor = Color.LimeGreen,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Mura.Salute_Max,
                Value = Variabili_Client.Citta.Mura.Salute,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };
            defenseBarMura = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "DEF",
                BarColor = Color.SteelBlue,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Mura.Difesa_Max,
                Value = Variabili_Client.Citta.Mura.Difesa,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };
            soldierBarMura = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "Guarnigione",
                BarColor = Color.OrangeRed,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Mura.Guarnigione_Max,
                Value = Variabili_Client.Citta.Mura.Guarnigione,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };

            barravitaTorri = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "HP",
                BarColor = Color.LimeGreen,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Torri.Salute_Max,
                Value = Variabili_Client.Citta.Torri.Salute,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };
            defenseBarTorri = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "DEF",
                BarColor = Color.SteelBlue,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Torri.Difesa_Max,
                Value = Variabili_Client.Citta.Torri.Difesa,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };
            soldierBarTorri = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "Guarnigione",
                BarColor = Color.OrangeRed,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Torri.Guarnigione_Max,
                Value = Variabili_Client.Citta.Torri.Guarnigione,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };

            barravitaCastello = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "HP",
                BarColor = Color.LimeGreen,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Castello.Salute_Max,
                Value = Variabili_Client.Citta.Castello.Salute,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };
            defenseBarCastello = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "DEF",
                BarColor = Color.SteelBlue,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Castello.Difesa_Max,
                Value = Variabili_Client.Citta.Castello.Difesa,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };
            soldierBarCastello = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "Guarnigione",
                BarColor = Color.OrangeRed,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Castello.Guarnigione_Max,
                Value = Variabili_Client.Citta.Castello.Guarnigione,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };

            soldierBarIngresso = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "Guarnigione",
                BarColor = Color.OrangeRed,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Ingresso.Guarnigione_Max,
                Value = Variabili_Client.Citta.Ingresso.Guarnigione,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };

            soldierBarCitta = new StatBar()
            {
                Location = new Point(20, 140),
                Size = new Size(250, 25),
                Label = "Guarnigione",
                BarColor = Color.OrangeRed,
                BackColorBar = Color.FromArgb(40, 40, 40),
                BorderColor = Color.Black,
                ForeColor = Color.Gainsboro,
                MaxValue = Variabili_Client.Citta.Città.Guarnigione_Max,
                Value = Variabili_Client.Citta.Città.Guarnigione,
                Radius_Border = 12,  // Raggio bordo esterno
                Radius_Internal = 10, // Raggio barra interna
                ShowText = true       // Mostra il testo
            };

            barreCitta = new List<(StatBar, Panel)>
            {
                (barravitaCancello, panel_Hp_Cancello),
                (defenseBarCancello, panel_Def_Cancello),
                (soldierBarCancello, panel_Soldier_Cancello),

                (barravitaMura, panel_Hp_Mura),
                (defenseBarMura, panel_Def_Mura),
                (soldierBarMura, panel_Soldier_Mura),

                (barravitaTorri, panel_Hp_Torri),
                (defenseBarTorri, panel_Def_Torri),
                (soldierBarTorri, panel_Soldier_Torri),

                (barravitaCastello, panel_Hp_Castello),
                (defenseBarCastello, panel_Def_Castello),
                (soldierBarCastello, panel_Soldier_Castello),

                (soldierBarIngresso, panel_Soldier_Ingresso),
                (soldierBarCitta, panel_Soldier_Citta)
            };

            foreach (var (bar, panel) in barreCitta)
            {
                lastValues[bar] = bar.Value;
            }
            // Aggiunta al form o al panel
            //this.Controls.Add(barravita);
            //this.Controls.Add(defenseBar);
            //this.Controls.Add(attackBar);
            //this.Controls.Add(ciao);

        }

        private void Citta_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            pictureBox_Castello_Salute.BackgroundImageLayout = ImageLayout.Stretch;
            
            Task.Run(() => Gui_Update(cts.Token), cts.Token);
        }
        async void Gui_Update(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (panel1.IsHandleCreated && !panel1.IsDisposed)
                {
                    int daRiparare = 0;
                    panel1.BeginInvoke((Action)(async() =>
                    {
                        if (await Tutorial.TutorialPrecedentiCompletati(23) && Variabili_Client.tutorial[22] == false) ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{23}");
                        Gioco.toolTip1.SetToolTip(this.pictureBox_Cancello_Salute, $"{Variabili_Client.Citta.Cancello.Descrizione}");
                        Gioco.toolTip1.SetToolTip(this.pictureBox_Cancello_Difesa, $"{Variabili_Client.Citta.Cancello.DescrizioneB}");

                        Gioco.toolTip1.SetToolTip(this.pictureBox_Mura_Salute, $"{Variabili_Client.Citta.Mura.Descrizione}");
                        Gioco.toolTip1.SetToolTip(this.pictureBox_Mura_Difesa, $"{Variabili_Client.Citta.Mura.DescrizioneB}");

                        Gioco.toolTip1.SetToolTip(this.pictureBox_Torri_Salute, $"{Variabili_Client.Citta.Torri.Descrizione}");
                        Gioco.toolTip1.SetToolTip(this.pictureBox_Torri_Difesa, $"{Variabili_Client.Citta.Torri.DescrizioneB}");

                        Gioco.toolTip1.SetToolTip(this.pictureBox_Castello_Salute, $"{Variabili_Client.Citta.Castello.Descrizione}");
                        Gioco.toolTip1.SetToolTip(this.pictureBox_Castello_Difesa, $"{Variabili_Client.Citta.Castello.DescrizioneB}");

                        soldierBarCancello.MaxValue = Variabili_Client.Citta.Cancello.Guarnigione_Max;
                        soldierBarCancello.Value = Variabili_Client.Citta.Cancello.Guarnigione;
                        defenseBarCancello.MaxValue = Variabili_Client.Citta.Cancello.Difesa_Max;
                        defenseBarCancello.Value = Variabili_Client.Citta.Cancello.Difesa;
                        barravitaCancello.MaxValue = Variabili_Client.Citta.Cancello.Salute_Max;
                        barravitaCancello.Value = Variabili_Client.Citta.Cancello.Salute;

                        soldierBarMura.MaxValue = Variabili_Client.Citta.Mura.Guarnigione_Max;
                        soldierBarMura.Value = Variabili_Client.Citta.Mura.Guarnigione;
                        defenseBarMura.MaxValue = Variabili_Client.Citta.Mura.Difesa_Max;
                        defenseBarMura.Value = Variabili_Client.Citta.Mura.Difesa;
                        barravitaMura.MaxValue = Variabili_Client.Citta.Mura.Salute_Max;
                        barravitaMura.Value = Variabili_Client.Citta.Mura.Salute;

                        soldierBarTorri.MaxValue = Variabili_Client.Citta.Torri.Guarnigione_Max;
                        soldierBarTorri.Value = Variabili_Client.Citta.Torri.Guarnigione;
                        defenseBarTorri.MaxValue = Variabili_Client.Citta.Torri.Difesa_Max;
                        defenseBarTorri.Value = Variabili_Client.Citta.Torri.Difesa;
                        barravitaTorri.MaxValue = Variabili_Client.Citta.Torri.Salute_Max;
                        barravitaTorri.Value = Variabili_Client.Citta.Torri.Salute;

                        soldierBarCastello.MaxValue = Variabili_Client.Citta.Castello.Guarnigione_Max;
                        soldierBarCastello.Value = Variabili_Client.Citta.Castello.Guarnigione;
                        defenseBarCastello.MaxValue = Variabili_Client.Citta.Castello.Difesa_Max;
                        defenseBarCastello.Value = Variabili_Client.Citta.Castello.Difesa;
                        barravitaCastello.MaxValue = Variabili_Client.Citta.Castello.Salute_Max;
                        barravitaCastello.Value = Variabili_Client.Citta.Castello.Salute;

                        soldierBarIngresso.MaxValue = Variabili_Client.Citta.Ingresso.Guarnigione_Max;
                        soldierBarIngresso.Value = Variabili_Client.Citta.Ingresso.Guarnigione;

                        soldierBarCitta.MaxValue = Variabili_Client.Citta.Città.Guarnigione_Max;
                        soldierBarCitta.Value = Variabili_Client.Citta.Città.Guarnigione;

                        if (Variabili_Client.Citta.Mura.Salute < Variabili_Client.Citta.Mura.Salute_Max)
                        {
                            pictureBox_Mura_Salute.Visible = true;
                            daRiparare++;
                        }
                        else pictureBox_Mura_Salute.Visible = false;
                        if (Variabili_Client.Citta.Mura.Difesa < Variabili_Client.Citta.Mura.Difesa_Max)
                        {
                            pictureBox_Mura_Difesa.Visible = true;
                            daRiparare++;
                        }
                        else pictureBox_Mura_Difesa.Visible = false;

                        if (Variabili_Client.Citta.Cancello.Salute < Variabili_Client.Citta.Cancello.Salute_Max)
                        {
                            pictureBox_Cancello_Salute.Visible = true;
                            daRiparare++;
                        }
                        else pictureBox_Cancello_Salute.Visible = false;
                        if (Variabili_Client.Citta.Cancello.Difesa < Variabili_Client.Citta.Cancello.Difesa_Max)
                        {
                            pictureBox_Cancello_Difesa.Visible = true;
                            daRiparare++;
                        }
                        else pictureBox_Cancello_Difesa.Visible = false;

                        if (Variabili_Client.Citta.Torri.Salute < Variabili_Client.Citta.Torri.Salute_Max)
                        {
                            pictureBox_Torri_Salute.Visible = true;
                            daRiparare++;
                        }
                        else pictureBox_Torri_Salute.Visible = false;
                        if (Variabili_Client.Citta.Torri.Difesa < Variabili_Client.Citta.Torri.Difesa_Max)
                        {
                            pictureBox_Torri_Difesa.Visible = true;
                            daRiparare++;
                        }
                        else pictureBox_Torri_Difesa.Visible = false;

                        if (Variabili_Client.Citta.Castello.Salute < Variabili_Client.Citta.Castello.Salute_Max)
                        {
                            pictureBox_Castello_Salute.Visible = true;
                            daRiparare++;
                        }
                        else pictureBox_Castello_Salute.Visible = false;
                        if (Variabili_Client.Citta.Castello.Difesa < Variabili_Client.Citta.Castello.Difesa_Max)
                        {
                            pictureBox_Castello_Difesa.Visible = true;
                            daRiparare++;
                        }
                        else pictureBox_Castello_Difesa.Visible = false;

                        if (daRiparare >= 2) btn_Ripara_Tutto.Visible = true;
                        else btn_Ripara_Tutto.Visible = false;

                        //Colori
                        if (barravitaCancello.Value >= barravitaCancello.MaxValue * 0.66)
                            barravitaCancello.BarColor = Color.LimeGreen;
                        else if (barravitaCancello.Value < barravitaCancello.MaxValue * 0.66 && barravitaCancello.Value > barravitaCancello.MaxValue * 0.33)
                            barravitaCancello.BarColor = Color.Orange;
                        else if (barravitaCancello.Value <= barravitaCancello.MaxValue * 0.33)
                            barravitaCancello.BarColor = Color.Red;

                        if (barravitaMura.Value >= barravitaMura.MaxValue * 0.66)
                            barravitaMura.BarColor = Color.LimeGreen;
                        else if (barravitaMura.Value < barravitaMura.MaxValue * 0.66 && barravitaMura.Value > barravitaMura.MaxValue * 0.33)
                            barravitaMura.BarColor = Color.Orange;
                        else if (barravitaMura.Value <= barravitaMura.MaxValue * 0.33)
                            barravitaMura.BarColor = Color.Red;

                        if (barravitaTorri.Value >= barravitaTorri.MaxValue * 0.66)
                            barravitaTorri.BarColor = Color.LimeGreen;
                        else if (barravitaTorri.Value < barravitaTorri.MaxValue * 0.66 && barravitaTorri.Value > barravitaTorri.MaxValue * 0.33)
                            barravitaTorri.BarColor = Color.Orange;
                        else if (barravitaTorri.Value <= barravitaTorri.MaxValue * 0.33)
                            barravitaTorri.BarColor = Color.Red;

                        if (barravitaCastello.Value >= barravitaCastello.MaxValue * 0.66)
                            barravitaCastello.BarColor = Color.LimeGreen;
                        else if (barravitaCastello.Value < barravitaCastello.MaxValue * 0.66 && barravitaCastello.Value > barravitaCastello.MaxValue * 0.33)
                            barravitaCastello.BarColor = Color.Orange;
                        else if (barravitaCastello.Value <= barravitaCastello.MaxValue * 0.33)
                            barravitaCastello.BarColor = Color.Red;

                        //Ridisegna solo se il valore è cambiato
                        foreach (var (bar, panel) in barreCitta)
                        {
                            var a = bar.Text;
                            int currentValue = bar.Value; // valore attuale
                            int ActualValueMax = bar.MaxValue; // valore attuale
                            if (!lastValues.TryGetValue(bar, out int lastValue) || lastValue != currentValue)
                            {
                                panel.Invalidate();
                                lastValues[bar] = currentValue;
                            }
                            int currentValueMax = bar.MaxValue; // valore attuale
                            if (!lastValuesMax.TryGetValue(bar, out int lastValueMax) || lastValue != ActualValueMax)
                            {
                                panel.Invalidate();
                                lastValuesMax[bar] = currentValueMax;
                            }
                        }

                        lbl_Mura.Text = $"Mura      [2]      Lv: {Variabili_Client.Citta.Mura.Livello}";
                        lbl_Cancello.Text = $"Cancello      [3]      Lv: {Variabili_Client.Citta.Cancello.Livello}";
                        lbl_Torri.Text = $"Torri      [4]      Lv: {Variabili_Client.Citta.Torri.Livello}";
                        lbl_Castello.Text = $"Castello      [5]      Lv: {Variabili_Client.Citta.Castello.Livello}";
                    }));
                }
                await Task.Delay(1000); // meglio di Thread.Sleep
            }
        }

        private void panel_Soldier_Cancello_Paint(object sender, PaintEventArgs e)
        {
            soldierBarCancello.DrawOn(e.Graphics, panel_Soldier_Cancello.ClientRectangle);
        }

        private void panel_Def_Cancello_Paint(object sender, PaintEventArgs e)
        {
            defenseBarCancello.DrawOn(e.Graphics, panel_Def_Cancello.ClientRectangle);
        }

        private void panel_Hp_Cancello_Paint(object sender, PaintEventArgs e)
        {
            barravitaCancello.DrawOn(e.Graphics, panel_Hp_Cancello.ClientRectangle);
        }

        private void panel_Soldier_Mura_Paint(object sender, PaintEventArgs e)
        {
            soldierBarMura.DrawOn(e.Graphics, panel_Soldier_Mura.ClientRectangle);
        }

        private void panel_Def_Mura_Paint(object sender, PaintEventArgs e)
        {
            defenseBarMura.DrawOn(e.Graphics, panel_Def_Mura.ClientRectangle);
        }

        private void panel_Hp_Mura_Paint(object sender, PaintEventArgs e)
        {
            barravitaMura.DrawOn(e.Graphics, panel_Hp_Mura.ClientRectangle);
        }

        private void panel_Soldier_Torri_Paint(object sender, PaintEventArgs e)
        {
            soldierBarTorri.DrawOn(e.Graphics, panel_Soldier_Torri.ClientRectangle);
        }

        private void panel_Def_Torri_Paint(object sender, PaintEventArgs e)
        {
            defenseBarTorri.DrawOn(e.Graphics, panel_Def_Torri.ClientRectangle);
        }

        private void panel_Hp_Torri_Paint(object sender, PaintEventArgs e)
        {
            barravitaTorri.DrawOn(e.Graphics, panel_Hp_Torri.ClientRectangle);
        }

        private void panel_Soldier_Castello_Paint(object sender, PaintEventArgs e)
        {
            soldierBarCastello.DrawOn(e.Graphics, panel_Soldier_Castello.ClientRectangle);
        }

        private void panel_Def_Castello_Paint(object sender, PaintEventArgs e)
        {
            defenseBarCastello.DrawOn(e.Graphics, panel_Def_Castello.ClientRectangle);
        }

        private void panel_Hp_Castello_Paint(object sender, PaintEventArgs e)
        {
            barravitaCastello.DrawOn(e.Graphics, panel_Hp_Castello.ClientRectangle);
        }

        private void panel_Soldier_Ingresso_Paint(object sender, PaintEventArgs e)
        {
            soldierBarIngresso.DrawOn(e.Graphics, panel_Soldier_Ingresso.ClientRectangle);
        }

        private void panel_Soldier_Citta_Paint(object sender, PaintEventArgs e)
        {
            soldierBarCitta.DrawOn(e.Graphics, panel_Soldier_Citta.ClientRectangle);
        }

        private void Citta_V2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Variabili_Client.tutorial_Attivo)
            {
                GameAudio.StopMusic();
                GameAudio.PlayMenuMusic("Gioco");
                MusicManager.SetVolume(0.2f);
            }
            cts.Cancel();
        }

        private void btn_Ingresso_Click(object sender, EventArgs e)
        {
            Spostamento_Truppe.struttura = "Ingresso";
            Spostamento_Truppe form_Gioco = new Spostamento_Truppe();
            form_Gioco.ShowDialog();
        }

        private void btn_Mura_Click(object sender, EventArgs e)
        {
            Spostamento_Truppe.struttura = "Mura";
            Spostamento_Truppe form_Gioco = new Spostamento_Truppe();
            form_Gioco.ShowDialog();
        }

        private void btn_Cancello_Click(object sender, EventArgs e)
        {
            Spostamento_Truppe.struttura = "Cancello";
            Spostamento_Truppe form_Gioco = new Spostamento_Truppe();
            form_Gioco.ShowDialog();
        }

        private void btn_Torri_Click(object sender, EventArgs e)
        {
            Spostamento_Truppe.struttura = "Torri";
            Spostamento_Truppe form_Gioco = new Spostamento_Truppe();
            form_Gioco.ShowDialog();
        }

        private void btn_Citta_Click(object sender, EventArgs e)
        {
            Spostamento_Truppe.struttura = "Citta";
            Spostamento_Truppe form_Gioco = new Spostamento_Truppe();
            form_Gioco.ShowDialog();
        }

        private async void btn_Castello_Click(object sender, EventArgs e)
        {
            Spostamento_Truppe.struttura = "Castello";
            if (Variabili_Client.tutorial_Attivo == true && await Tutorial.TutorialPrecedentiCompletati(26))
                ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{26}");
            Spostamento_Truppe form_Gioco = new Spostamento_Truppe();
            form_Gioco.ShowDialog();
        }

        private async void pictureBox_Mura_Salute_Click(object sender, EventArgs e)
        {
            tutorial[0] = true;
            if (Variabili_Client.tutorial_Attivo == true && await Tutorial.TutorialPrecedentiCompletati(25) && tutorial[0] && tutorial[1])
                ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{25}");
            ClientConnection.TestClient.Send($"Ripara|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Mura|Salute");
        }

        private async void pictureBox_Mura_Difesa_Click(object sender, EventArgs e)
        {
            tutorial[1] = true;
            if (Variabili_Client.tutorial_Attivo == true && await Tutorial.TutorialPrecedentiCompletati(25) && tutorial[0] && tutorial[1])
                ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{25}");
            ClientConnection.TestClient.Send($"Ripara|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Mura|Difesa");
        }

        private void pictureBox_Cancello_Salute_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ripara|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Cancello|Salute");
        }

        private void pictureBox_Cancello_Difesa_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ripara|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Cancello|Difesa");
        }

        private void pictureBox_Torri_Salute_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ripara|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Torri|Salute");
        }

        private void pictureBox_Torri_Difesa_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ripara|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Torri|Difesa");
        }

        private void pictureBox_Castello_Salute_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ripara|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Castello|Salute");
        }

        private void pictureBox_Castello_Difesa_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ripara|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Castello|Difesa");
        }

        private void btn_Ripara_Tutto_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Ripara|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Ripara Tutto");
        }
    }

}
