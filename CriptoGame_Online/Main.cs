using Strategico_V2;
using Warrior_and_Wealth.GUI;
using Warrior_and_Wealth.Strumenti;

namespace Warrior_and_Wealth
{
    public partial class Main : Form
    {
        //Elementi Form Principale
        private Form _formCorrente = null;
        public static CustomToolTip toolTip1;

        static string tipo_Risorse = "Civile";
        bool taskStart = false;

        //Elementi Tutorial
        public static GameTextBox? logBox;
        private CancellationTokenSource cts = new CancellationTokenSource();

        static bool quest_Attiva = false;
        public static bool quest_Completata = false;
        static int quest_Id = 0;


        public static void ScaleAllControls(Form target)
        {
            target.WindowState = FormWindowState.Maximized;

            float scaleX = (float)Screen.PrimaryScreen.Bounds.Width / 1920f;
            float scaleY = (float)Screen.PrimaryScreen.Bounds.Height / 1080f;
            float scale = Math.Min(scaleX, scaleY);
            scale = Math.Max(scale, 0.82f);

            // Offset per centrare orizzontalmente se lo schermo è più stretto
            int offsetX = (Screen.PrimaryScreen.Bounds.Width - (int)(1920 * scale)) / 2;
            int offsetY = (Screen.PrimaryScreen.Bounds.Height - (int)(1080 * scale)) / 2;

            ScaleControlsRecursive(target.Controls, scale, offsetX, offsetY);
        }
        private static void ScaleControlsRecursive(Control.ControlCollection controls, float scale, int offsetX, int offsetY)
        {
            foreach (Control ctrl in controls)
            {
                if (scale != 1)
                    ctrl.Location = new Point(
                        (int)(ctrl.Left * scale) + offsetX + 140,
                        (int)(ctrl.Top * scale) + offsetY + 3
                    );
                else
                    ctrl.Location = new Point(
                    (int)(ctrl.Left * scale) + offsetX,
                    (int)(ctrl.Top * scale) + offsetY
                );
                ctrl.Size = new Size(
                    (int)(ctrl.Width * scale),
                    (int)(ctrl.Height * scale)
                );
                if (scale != 1)
                    ctrl.Font = new Font(
                        ctrl.Font.FontFamily,
                        ctrl.Font.Size * (scale - 0.15f),
                        ctrl.Font.Style
                    );
                else
                    ctrl.Font = new Font(
                        ctrl.Font.FontFamily,
                        ctrl.Font.Size * scale,
                        ctrl.Font.Style
                    );

                if (ctrl.Controls.Count > 0)
                    // I figli NON ricevono l'offset, lo ha già il contenitore
                    ScaleControlsRecursive(ctrl.Controls, scale, 0, 0);
            }
        }
        private void CaricaForm(Form form, object sender, EventArgs e)
        {
            if (_formCorrente != null)
            {
                _formCorrente.FormClosing += (s, args) => _formCorrente.Dispose();
                _formCorrente.Close();
                _formCorrente.Dispose();
            }

            form.FormBorderStyle = FormBorderStyle.None;
            form.TopLevel = false;
            //form.Dock = DockStyle.Fill;

            panel_Gioco.BackColor = Color.Gray;
            panel_Gioco.Controls.Clear();
            panel_Gioco.Controls.Add(form);

            form.Show();

            _formCorrente = form;
            Main_Load(sender, e);
        }
        public Main()
        {
            InitializeComponent();
            ScaleAllControls(this);
            Risorse_1();
            this.ActiveControl = ico_Player; // assegna il focus al bottone

            logBox = new GameTextBox()
            {
                Dock = DockStyle.Fill
            };

            logBox.MouseClick += LogBox_MouseDown; //Creare l'evento MouseDown per il logBox
            panel_Tutorial.Controls.Add(logBox);// texbox personallizata
        }

        private void Main_Load(object sender, EventArgs e)
        {
            panel_Gioco.BackColor = Color.Gray;
            panel_Tutorial.BackColor = Color.Gray;

            if (_formCorrente != null) DisableButton();
            if (!taskStart) Task.Run(() => Gui_Update());
            if (!Variabili_Client.tutorial_Attivo)
            {
                GameAudio.PlayMenuMusic("Gioco");
                MusicManager.SetVolume(0.3f);
                panel_Tutorial.Visible = false;
                panel_Gioco.Dock = DockStyle.Fill;
            }
            if (Variabili_Client.tutorial_Attivo)
            {
                Task.Run(() => Tutorial_Update(cts.Token), cts.Token);
                Tutorial_Start();
            }
        }
        #region TUTORIAL
        void Tutorial_Start()
        {
            if (Variabili_Client.tutorial_Attivo) //Abilita la prima fase del tutorial se non è già stata completata... Il tutorial deve essere completato in toto, non si può saltare ne salvare lo stato...
                Task.Run(() => Tutorial_Update(cts.Token), cts.Token);
            else return;

            //Risorse
            panel_Tutorial.Visible = true;
            panel_Risorse_1.Visible = false;
            ico_Player.Visible = false;
            ico_DiamantiV.Visible = false;
            ico_DiamantiB.Visible = false;
            ico_Tributi.Visible = false;
            txt_Username.Visible = false;
            txt_Diamond_1.Visible = false;
            txt_Diamond_2.Visible = false;
            txt_Virtual_Dolla.Visible = false;

            Btn_Costruzione.Visible = false;
            btn_Citta.Visible = false;
            btn_Shop.Visible = false;
            btn_Ricerca.Visible = false;
            btn_Quest_Mensile.Visible = false;
            btn_PVP_PVE.Visible = false;
            btn_GamePass_Reward.Visible = false;
        }
        async void Tutorial_Update(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (panel_Tutorial.IsHandleCreated && !panel_Tutorial.IsDisposed)
                {
                    panel_Tutorial.BeginInvoke((Action)(() =>
                    {
                        //Tutorial
                        if (Variabili_Client.tutorial_Attivo)
                        {
                            if (Variabili_Client.tutorial[1]) panel_Risorse_1.Visible = true;
                            if (Variabili_Client.tutorial[2]) //D_Viola
                            {
                                ico_DiamantiV.Visible = true;
                                txt_Diamond_2.Visible = true;
                            }
                            if (Variabili_Client.tutorial[3]) //D_Blu
                            {
                                ico_DiamantiB.Visible = true;
                                txt_Diamond_1.Visible = true;
                            }
                            if (Variabili_Client.tutorial[4]) //Tributi
                            {
                                ico_Tributi.Visible = true;
                                txt_Virtual_Dolla.Visible = true;
                            }


                            if (Variabili_Client.tutorial[9]) //Costruzione
                            {
                                Btn_Costruzione.Visible = true;
                            }

                            if (Variabili_Client.tutorial[22]) //Città
                            {
                                btn_Citta.Visible = true;
                            }
                            if (Variabili_Client.tutorial[25]) //Giocatore / Statisctiche
                            {
                                txt_Username.Visible = true;
                                ico_Player.Visible = true;
                            }
                            if (Variabili_Client.tutorial[26]) //Shop
                            {
                                btn_Shop.Visible = true;
                            }
                            if (Variabili_Client.tutorial[27]) //Ricerca
                            {
                                btn_Ricerca.Visible = true;
                            }
                            if (Variabili_Client.tutorial[28]) //Quest Mensili
                            {
                                btn_Quest_Mensile.Visible = true;
                            }
                            if (Variabili_Client.tutorial[29]) //Battaglie
                            {
                                btn_PVP_PVE.Visible = true;
                                btn_GamePass_Reward.Visible = false;
                            }
                            if (Variabili_Client.tutorial[31]) //Visualizza il resto al termine... 
                            {
                                btn_GamePass_Reward.Visible = true;
                                //btn_Mappa.Visible = false; //Mappa (Non funziona)
                                //ico_Notifiche.Visible = false; //Notifiche, non ancora implementato.
                            }
                        }

                        foreach (var i in Variabili_Client.tutorial_dati)
                        {
                            if (Variabili_Client.tutorial[0] == false && quest_Attiva == false)
                            {
                                Log_Update("Tutorial: " + i.StatoTutorial.ToString());
                                Log_Update("Oiettivo: " + i.Obiettivo);
                                Log_Update("");
                                Log_Update(i.Descrizione);
                                GameAudio.PlayMenuMusic($"Tutorial - {i.StatoTutorial}");
                                MusicManager.SetVolume(1.0f);
                                quest_Attiva = true;
                                quest_Id = i.StatoTutorial;
                            }
                            if (i.StatoTutorial >= 2)
                                if (Variabili_Client.tutorial[i.StatoTutorial - 1] == false && Variabili_Client.tutorial[i.StatoTutorial - 2] == true && quest_Attiva == false)
                                {
                                    logBox.Clear();
                                    Log_Update("Tutorial: " + i.StatoTutorial.ToString());
                                    Log_Update("Oiettivo: " + i.Obiettivo);
                                    Log_Update("");
                                    Log_Update(i.Descrizione);
                                    GameAudio.PlayMenuMusic($"Tutorial - {i.StatoTutorial}");
                                    MusicManager.SetVolume(1.0f);
                                    quest_Attiva = true;
                                    quest_Id = i.StatoTutorial;
                                }
                        }

                        if (Variabili_Client.tutorial[31])
                        {
                            cts.Cancel();
                            this.BeginInvoke((Action)(() => this.Close()));
                            return;
                        }

                    }));
                }

                await Task.Delay(250);
            }
        }
        public static async Task<bool> TutorialPrecedentiCompletati(int step)
        {
            if (step <= 1) return true; // step 1 non ha prerequisiti
            if (step - 1 > Variabili_Client.tutorial.Length) return false; // Controllo sicurezza indice

            for (int i = 0; i < step - 1; i++) // Controlla tutti gli step precedenti
            {
                if (!Variabili_Client.tutorial[i])
                    return false;
            }
            return true;
        }
        public static void Log_Update(string messaggio)
        {
            if (logBox != null) logBox.Invoke(new Action(() => logBox.AddLineFromServer(messaggio)));
        }

        private void Tutorial_FormClosing(object sender, FormClosingEventArgs e)
        {
            GameAudio.StopMusic();
            cts.Cancel();
        }

        private async void LogBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && quest_Id != 0)
            {
                if (Variabili_Client.tutorial[quest_Id - 1] == false) quest_Completata = true;
                if (quest_Completata == true
                    && quest_Id != 8 && quest_Id != 10 && quest_Id != 11 && quest_Id != 12 && quest_Id != 13 && quest_Id != 14 && quest_Id != 15 && quest_Id != 16 && quest_Id != 17
                    && quest_Id != 18 && quest_Id != 19 && quest_Id != 21 && quest_Id != 22 && quest_Id != 23 && quest_Id != 24 && quest_Id != 25 && quest_Id != 26 && quest_Id != 27
                    && quest_Id != 28 && quest_Id != 29 && quest_Id != 30 && quest_Id != 31 && await Main.TutorialPrecedentiCompletati(quest_Id)) //Sono tutte le quest che si completano in altro modo purtroppo....
                    ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{quest_Id}");

                await Login.Sleep(1);
                quest_Attiva = false;
            }
        }
        #endregion
        async void Gui_Update()
        {
            toolTip1 = new CustomToolTip();

            // Imposta qualche proprietà opzionale
            toolTip1.InitialDelay = 150;
            toolTip1.AutoPopDelay = 15000;

            while (true)
            {
                bool taskStart = true;
                Thread.Sleep(250); // poco piu di 30 fps
                if (panel1.IsHandleCreated && !panel1.IsDisposed)
                {
                    panel1.BeginInvoke((Action)(async () =>
                    {
                        toolTip1.SetToolTip(this.ico_Player, $"{Variabili_Client.Giocatore_Desc}");
                        toolTip1.SetToolTip(this.ico_Tributi, $"{Variabili_Client.Dollari_VIrtuali_Desc}");
                        toolTip1.SetToolTip(this.ico_DiamantiB, $"{Variabili_Client.Diamanti_Blu_Desc}");
                        toolTip1.SetToolTip(this.ico_DiamantiV, $"{Variabili_Client.Diamanti_Viola_Desc}");
                        toolTip1.SetToolTip(this.ico_Esperienza, Variabili_Client.Esperienza_Desc);
                        toolTip1.SetToolTip(this.ico_Livello, Variabili_Client.Livello_Desc);

                        txt_Username.Text = Variabili_Client.Utente.Username;
                        txt_Livello.Text = Variabili_Client.Utente.Livello;
                        txt_Esperienza.Text = Variabili_Client.Utente.Esperienza + " XP";

                        txt_Diamond_1.Text = Variabili_Client.Utente_Risorse.Diamond_Blu;
                        txt_Diamond_2.Text = Variabili_Client.Utente_Risorse.Diamond_Viola;
                        txt_Virtual_Dolla.Text = Variabili_Client.Utente_Risorse.Virtual_Dolla;

                        if (tipo_Risorse == "Militare") //risorse
                        {
                            toolTip1.SetToolTip(this.ico_Cibo, $"{Variabili_Client.Spade_Desc}Produzione: [verde]{Variabili_Client.Utente_Risorse.Spade_s}[/verde][black]s\r\n Limite: [ferroScuro]{Variabili_Client.Utente_Risorse.Spade_Limite}");
                            toolTip1.SetToolTip(this.ico_Legno, $"{Variabili_Client.Lance_Desc}Produzione: [verde]{Variabili_Client.Utente_Risorse.Lance_s}[/verde][black]s\r\n Limite: [ferroScuro]{Variabili_Client.Utente_Risorse.Lance_Limite}");
                            toolTip1.SetToolTip(this.ico_Pietra, $"{Variabili_Client.Archi_Desc}Produzione: [verde]{Variabili_Client.Utente_Risorse.Archi_s}[/verde][black]s\r\n Limite: [ferroScuro]{Variabili_Client.Utente_Risorse.Archi_Limite}");
                            toolTip1.SetToolTip(this.ico_Ferro, $"{Variabili_Client.Scudi_Desc}Produzione: [verde]{Variabili_Client.Utente_Risorse.Scudi_s}[/verde][black]s\r\n Limite: [ferroScuro]{Variabili_Client.Utente_Risorse.Scudi_Limite}");
                            toolTip1.SetToolTip(this.ico_Oro, $"{Variabili_Client.Armature_Desc}Produzione: [verde]{Variabili_Client.Utente_Risorse.Armature_s}[/verde][black]s\r\n Limite: [ferroScuro]{Variabili_Client.Utente_Risorse.Armature_Limite}");
                            toolTip1.SetToolTip(this.ico_Popolazione, $"{Variabili_Client.Frecce_Desc}Produzione: [verde]{Variabili_Client.Utente_Risorse.Frecce_s}[/verde][black]s\r\n Limite: [ferroScuro]{Variabili_Client.Utente_Risorse.Frecce_Limite}");

                            txt_Risorsa1.Text = Variabili_Client.Utente_Risorse.Spade;
                            txt_Risorsa2.Text = Variabili_Client.Utente_Risorse.Lance;
                            txt_Risorsa3.Text = Variabili_Client.Utente_Risorse.Archi;
                            txt_Risorsa4.Text = Variabili_Client.Utente_Risorse.Scudi;
                            txt_Risorsa5.Text = Variabili_Client.Utente_Risorse.Armature;
                            txt_Risorsa6.Text = Variabili_Client.Utente_Risorse.Frecce;
                        }
                        else
                        {
                            double valore = Convert.ToDouble(Variabili_Client.Utente_Risorse.Cibo_s) - Convert.ToDouble(Variabili_Client.Utente_Risorse.Strutture_Cibo) - Convert.ToDouble(Variabili_Client.Utente_Risorse.Mantenimento_Cibo);
                            toolTip1.SetToolTip(this.ico_Cibo, $"{Variabili_Client.Cibo_Desc}" +
                                $"Produzione: [icon:cibo][arancione]{valore.ToString("0.00")}[black]/[verde]{Variabili_Client.Utente_Risorse.Cibo_s}[/verde][black]s\r\n" +
                                $"Edifici: [icon:cibo][rosso]{Variabili_Client.Utente_Risorse.Strutture_Cibo}[/rosso][black]s\r\n" +
                                $"Esercito: [icon:cibo][rosso]{Variabili_Client.Utente_Risorse.Mantenimento_Cibo}[/rosso][black]s\r\n" +
                                $"Limite: [icon:cibo][ferroScuro]{Variabili_Client.Utente_Risorse.Cibo_Limite}\n");

                            toolTip1.SetToolTip(this.ico_Legno, $"{Variabili_Client.Legno_Desc}" +
                                $"Produzione: [icon:legno][arancione]{(Convert.ToDouble(Variabili_Client.Utente_Risorse.Legna_s) - Convert.ToDouble(Variabili_Client.Utente_Risorse.Strutture_Legna)).ToString("0.00")}[black]/[verde]{Variabili_Client.Utente_Risorse.Legna_s}[/verde][black]s\r\n" +
                                $"Edifici: [icon:legno][rosso]{Variabili_Client.Utente_Risorse.Strutture_Legna}[/rosso][black]s\r\n" +
                                $"Limite: [icon:legno][ferroScuro]{Variabili_Client.Utente_Risorse.Legna_Limite}\n");

                            toolTip1.SetToolTip(this.ico_Pietra, $"{Variabili_Client.Pietra_Desc}" +
                                $"Produzione: [icon:pietra][arancione]{(Convert.ToDouble(Variabili_Client.Utente_Risorse.Pietra_s) - Convert.ToDouble(Variabili_Client.Utente_Risorse.Strutture_Pietra)).ToString("0.00")}[black]/[verde]{Variabili_Client.Utente_Risorse.Pietra_s}[/verde][black]s\r\n" +
                                $"Edifici: [icon:pietra][rosso]{Variabili_Client.Utente_Risorse.Strutture_Pietra}[/rosso][black]s\r\n" +
                                $"Limite: [icon:pietra][ferroScuro]{Variabili_Client.Utente_Risorse.Pietra_Limite}");

                            toolTip1.SetToolTip(this.ico_Ferro, $"{Variabili_Client.Ferro_Desc}" +
                                $"Produzione: [icon:ferro][arancione]{(Convert.ToDouble(Variabili_Client.Utente_Risorse.Ferro_s) - Convert.ToDouble(Variabili_Client.Utente_Risorse.Strutture_Ferro)).ToString("0.00")}[black]/[verde]{Variabili_Client.Utente_Risorse.Ferro_s}[/verde][black]s\n" +
                                $"Edifici: [icon:ferro][rosso]{Variabili_Client.Utente_Risorse.Strutture_Ferro}[/rosso][black]s\r\n" +
                                $"Limite: [icon:ferro][ferroScuro]{Variabili_Client.Utente_Risorse.Ferro_Limite}");

                            toolTip1.SetToolTip(this.ico_Oro, $"{Variabili_Client.Oro_Desc}" +
                                $"Produzione: [icon:oro][arancione]{(Convert.ToDouble(Variabili_Client.Utente_Risorse.Oro_s) - Convert.ToDouble(Variabili_Client.Utente_Risorse.Strutture_Oro) - Convert.ToDouble(Variabili_Client.Utente_Risorse.Mantenimento_Oro)).ToString("0.00")}[black]/[verde]{Variabili_Client.Utente_Risorse.Oro_s}[/verde][black]s\r\n" +
                                $"Edifici: [icon:oro][rosso]{Variabili_Client.Utente_Risorse.Strutture_Oro}[/rosso][black]s\r\n" +
                                $"Esercito: [icon:oro][rosso]{Variabili_Client.Utente_Risorse.Mantenimento_Oro}[/rosso][black]s\r\n" +
                                $"Limite: [icon:oro][ferroScuro]{Variabili_Client.Utente_Risorse.Oro_Limite}");

                            toolTip1.SetToolTip(this.ico_Popolazione, $"{Variabili_Client.Popolazione_Desc}" +
                                $"Produzione: [icon:popolazione][verde]{Variabili_Client.Utente_Risorse.Popolazione_s}[/verde][black]s\r\n" +
                                $"Limite: [icon:popolazione][ferroScuro]{Variabili_Client.Utente_Risorse.Popolazione_Limite}\n");

                            txt_Risorsa1.Text = Variabili_Client.Utente_Risorse.Cibo;
                            txt_Risorsa2.Text = Variabili_Client.Utente_Risorse.Legna;
                            txt_Risorsa3.Text = Variabili_Client.Utente_Risorse.Pietra;
                            txt_Risorsa4.Text = Variabili_Client.Utente_Risorse.Ferro;
                            txt_Risorsa5.Text = Variabili_Client.Utente_Risorse.Oro;
                            txt_Risorsa6.Text = Variabili_Client.Utente_Risorse.Popolazione;
                        }


                    }));

                }
            }
        }
        void DisableButton()
        {
            if (_formCorrente.Text == "Warrior & Wealth")
                btn_Home.Enabled = false;
            else btn_Home.Enabled = true;

            if (_formCorrente.Text == "Costruzione")
                Btn_Costruzione.Enabled = false;
            else Btn_Costruzione.Enabled = true;

            if (_formCorrente.Text == "Citta")
                btn_Citta.Enabled = false;
            else btn_Citta.Enabled = true;

            if (_formCorrente.Text == "Shop")
                btn_Shop.Enabled = false;
            else btn_Shop.Enabled = true;

            if (_formCorrente.Text == "Ricerca")
                btn_Ricerca.Enabled = false;
            else btn_Ricerca.Enabled = true;

            if (_formCorrente.Text == "PVE-PVP")
                btn_PVP_PVE.Enabled = false;
            else btn_PVP_PVE.Enabled = true;

            if (_formCorrente.Text == "GamePass")
                btn_GamePass_Reward.Enabled = false;
            else btn_GamePass_Reward.Enabled = true;

            if (_formCorrente.Text == "MontlyQuest")
                btn_Quest_Mensile.Enabled = false;
            else btn_Quest_Mensile.Enabled = true;
        }
        private void Risorse_1()
        {
            panel_Risorse_1.BackColor = Color.FromArgb(229, 208, 181);

            // Risorse
            txt_Risorsa1.BackColor = Color.FromArgb(229, 208, 181);
            txt_Risorsa2.BackColor = Color.FromArgb(229, 208, 181);
            txt_Risorsa3.BackColor = Color.FromArgb(229, 208, 181);
            txt_Risorsa4.BackColor = Color.FromArgb(229, 208, 181);
            txt_Risorsa5.BackColor = Color.FromArgb(229, 208, 181);
            txt_Risorsa6.BackColor = Color.FromArgb(229, 208, 181);

            txt_Username.BackColor = Color.FromArgb(229, 208, 181);
            txt_Esperienza.BackColor = Color.FromArgb(229, 208, 181);
            txt_Livello.BackColor = Color.FromArgb(229, 208, 181);
            txt_Diamond_1.BackColor = Color.FromArgb(229, 208, 181);
            txt_Diamond_2.BackColor = Color.FromArgb(229, 208, 181);

            txt_Virtual_Dolla.BackColor = Color.FromArgb(229, 208, 181);
            txt_Tipi_Risorse.BackColor = Color.FromArgb(229, 208, 181);

            ico_Cibo.BackColor = Color.FromArgb(229, 208, 181);
            ico_Legno.BackColor = Color.FromArgb(229, 208, 181);
            ico_Pietra.BackColor = Color.FromArgb(229, 208, 181);
            ico_Ferro.BackColor = Color.FromArgb(229, 208, 181);
            ico_Oro.BackColor = Color.FromArgb(229, 208, 181);
            ico_Popolazione.BackColor = Color.FromArgb(229, 208, 181);
            ico_DiamantiB.BackColor = Color.FromArgb(229, 208, 181);
            ico_Esperienza.BackColor = Color.FromArgb(229, 208, 181);
            ico_Livello.BackColor = Color.FromArgb(100, 229, 208, 181);
            ico_Player.BackColor = Color.FromArgb(100, 229, 208, 181);

            txt_Risorsa1.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Risorsa2.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Risorsa3.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Risorsa4.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Risorsa5.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Risorsa6.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);

            txt_Username.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Esperienza.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Livello.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Diamond_2.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Diamond_1.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Virtual_Dolla.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);

        }

        private async void Btn_Costruzione_Click(object sender, EventArgs e)
        {
            if (Variabili_Client.tutorial_Attivo == true && await Main.TutorialPrecedentiCompletati(11))
                ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{11}");
            CaricaForm(new Costruzione(), sender, e);
            Main_Load(sender, e);
        }

        private async void btn_Citta_Click(object sender, EventArgs e)
        {
            if (Variabili_Client.tutorial_Attivo == true && await Main.TutorialPrecedentiCompletati(24))
                ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{24}");
            CaricaForm(new Citta_V2(), sender, e);
        }

        private async void btn_Shop_Click(object sender, EventArgs e)
        {
            if (Variabili_Client.tutorial_Attivo == true && await Main.TutorialPrecedentiCompletati(28))
                ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{28}");
            CaricaForm(new Shop(), sender, e);
        }

        private async void btn_Ricerca_Click(object sender, EventArgs e)
        {
            if (Variabili_Client.tutorial_Attivo == true && await Main.TutorialPrecedentiCompletati(29))
                ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{29}");
            CaricaForm(new Ricerca_1(), sender, e);
        }

        private async void btn_PVP_PVE_Click(object sender, EventArgs e)
        {
            if (!Variabili_Client.tutorial_Attivo)
            {
                GameAudio.PlayMenuMusic("PVP");
                MusicManager.SetVolume(0.3f);
            }

            if (Variabili_Client.tutorial_Attivo == true && await Main.TutorialPrecedentiCompletati(31))
                ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{31}");
            CaricaForm(new AttaccoCoordinato(), sender, e);
        }

        private void btn_GamePass_Reward_Click(object sender, EventArgs e)
        {
            CaricaForm(new GamePassReward(), sender, e);
        }

        private async void btn_Quest_Mensile_Click(object sender, EventArgs e)
        {
            if (Variabili_Client.tutorial_Attivo == true && await Main.TutorialPrecedentiCompletati(30))
                ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{30}");
            CaricaForm(new MontlyQuest(), sender, e);
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            CaricaForm(new Gioco(), sender, e);
        }

        private async void ico_Player_MouseClick(object sender, MouseEventArgs e)
        {
            if (Variabili_Client.tutorial_Attivo == true && await Main.TutorialPrecedentiCompletati(27))
                ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{27}");
            CaricaForm(new Statistiche(), sender, e);
        }

        private void btn_Cambia_Risorse_Click(object sender, EventArgs e)
        {
            if (txt_Tipi_Risorse.Text == "Militare")
            {
                txt_Tipi_Risorse.Text = "Civile";
                tipo_Risorse = "Civile";
                ico_Cibo.BackgroundImage = Properties.Resources.Grano_V2;
                ico_Legno.BackgroundImage = Properties.Resources.Legna_V2;
                ico_Pietra.BackgroundImage = Properties.Resources.Pietra_V2;
                ico_Ferro.BackgroundImage = Properties.Resources.Ferro_V2;
                ico_Oro.BackgroundImage = Properties.Resources.Oro_V2;
                ico_Popolazione.BackgroundImage = Properties.Resources.Popolazione_V2;
                return;
            }

            txt_Tipi_Risorse.Text = "Militare";
            tipo_Risorse = "Militare";
            ico_Cibo.BackgroundImage = Properties.Resources.Spade_V2;
            ico_Legno.BackgroundImage = Properties.Resources.Lance_V2;
            ico_Pietra.BackgroundImage = Properties.Resources.Archi_V2;
            ico_Ferro.BackgroundImage = Properties.Resources.Scudi_V2;
            ico_Oro.BackgroundImage = Properties.Resources.Armature_V2;
            ico_Popolazione.BackgroundImage = Properties.Resources.Frecce_V2;
        }
    }
}
