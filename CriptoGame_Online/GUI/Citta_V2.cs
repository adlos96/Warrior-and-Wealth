using CriptoGame_Online.Strumenti;
using Strategico_V2;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CriptoGame_Online.GUI
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
        private List<(StatBar bar, Panel panel)> barreCitta;

        private CancellationTokenSource cts = new CancellationTokenSource();

        public Citta_V2()
        {
            InitializeComponent();

            txt_Testo.BackColor = Color.FromArgb(235, 221, 192);
            txt_Testo.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);

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
            GUI();
            //Update();
            Task.Run(() => Gui_Update(cts.Token), cts.Token);
        }
        private void GUI()
        {

        }

        async void Gui_Update(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (panel1.IsHandleCreated && !panel1.IsDisposed)
                {
                    panel1.BeginInvoke((Action)(() =>
                    {
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
                            int currentValue = bar.Value; // valore attuale
                            if (!lastValues.TryGetValue(bar, out int lastValue) || lastValue != currentValue)
                            {
                                panel.Invalidate();
                                lastValues[bar] = currentValue;
                            }
                        }
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
            Spostamento_Truppe.struttura = "Città";
            Spostamento_Truppe form_Gioco = new Spostamento_Truppe();
            form_Gioco.ShowDialog();
        }

        private void btn_Castello_Click(object sender, EventArgs e)
        {
            Spostamento_Truppe.struttura = "Castello";
            Spostamento_Truppe form_Gioco = new Spostamento_Truppe();
            form_Gioco.ShowDialog();
        }
    }

}
