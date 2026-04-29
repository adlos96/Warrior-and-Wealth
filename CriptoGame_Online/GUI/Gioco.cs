using Warrior_and_Wealth.GUI;
using Warrior_and_Wealth.Strumenti;
using Strategico_V2;

namespace Warrior_and_Wealth
{
    public partial class Gioco : Form
    {
        public static Gioco? Instance { get; private set; }
        public static GameTextBox? logBox;
        public static CustomToolTip toolTip1;
        static string strutture = "Civile";
        static string tipo_Risorse = "Civile";
        static string Caserme = "Esercito";
        static int livello_Esercito = 1;

        private CancellationTokenSource cts = new CancellationTokenSource();

        public Gioco()
        {
            //Main.ScaleAllControls(this);
            InitializeComponent();
            Instance = this;

            logBox = new GameTextBox()
            {
                Dock = DockStyle.Fill
            };
            panel_Log.Controls.Add(logBox);// texbox personallizata
        }

        public static void Log_Update(string messaggio)
        {
            if (logBox != null) logBox.Invoke(new Action(() => logBox.AddLineFromServer(messaggio)));
        }

        private void Gioco_Load(object sender, EventArgs e)
        {
            if (!Variabili_Client.tutorial_Attivo)
            {
                GameAudio.PlayMenuMusic("Gioco");
                MusicManager.SetVolume(0.3f);
            } 

            // Pannelli e Cose
            panel_2.BackColor = Color.FromArgb(100, 229, 208, 181);
            panel_Sfondo_Bottoni.BackColor = Color.FromArgb(50, 180, 150, 100);

            panel_Image_2.BackColor = Color.FromArgb(100, 218, 193, 163); //Sfondo immagini diamanti
            panel_Image_3.BackColor = Color.FromArgb(100, 218, 193, 163); //Sfondo immagini diamanti
            btn_Acquista_Terreni.BackColor = Color.FromArgb(218, 193, 163);

            groupBox_Strutture.BackColor = Color.FromArgb(100, 229, 208, 181);
            groupBox_Esercito.BackColor = Color.FromArgb(100, 229, 208, 181);

            //Load Guid
            GUI();

            Task.Run(() => Gui_Update(cts.Token), cts.Token);
            Log_Update($"[info]Benvenuto[/info] giocatore: [title]{Variabili_Client.Utente.Username}");
            Tutorial_Start();
        }
        void Tutorial_Start()
        {
            if (!Variabili_Client.tutorial_Attivo) return;

            groupBox_Terreni.Visible = false;
            groupBox_Strutture.Visible = false;
            groupBox_Esercito.Visible = false;

            btn_Acquista_Terreni.Visible = false;
            btn_Mappa.Visible = false;
            ico_Notifiche.Visible = false;

            btn_Scambia.Visible = false;
            btn_Acquista_Terreni.Visible = false;
            panel2.Visible = false; //Pannello log/chat

            //Feudi
            btn_Acquista_Terreni.Visible = false;
            btn_Scambia.Visible = false;
            panel_Image_2.Visible = false;
            panel_Image_3.Visible = false;
        }
        async void Gui_Update(CancellationToken token)
        {
            toolTip1 = new CustomToolTip();

            // Imposta qualche proprietà opzionale
            toolTip1.InitialDelay = 150;
            toolTip1.AutoPopDelay = 15000;

            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(250); // poco piu di 30 fps
                if (panel2.IsHandleCreated && !panel2.IsDisposed)
                {
                    panel2.BeginInvoke((Action)(async () =>
                    {
                        //Tutorial
                        if (Variabili_Client.tutorial_Attivo)
                        {
                            if (Variabili_Client.tutorial[5]) //Feudi
                            {
                                groupBox_Terreni.Visible = true;
                            }
                            if (Variabili_Client.tutorial[6]) //Acquista feudo
                            {
                                btn_Acquista_Terreni.Visible = true;
                                panel2.Visible = true;
                                panel_Image_2.Visible = true;
                            }
                            if (Variabili_Client.tutorial[8]) //Strutture Civili Militari
                            {
                                groupBox_Strutture.Visible = true;
                            }
                            if (Variabili_Client.tutorial[10]) //scambia
                            {
                                btn_Scambia.Visible = true;
                                panel_Image_3.Visible = true;
                            }
                            if (Variabili_Client.tutorial[19]) //Esercito
                            {
                                groupBox_Esercito.Visible = true;
                            }
                            if (Variabili_Client.tutorial[29]) //Battaglie
                            {
                                btn_Mappa.Visible = false;
                                ico_Notifiche.Visible = false;
                            }
                            if (Variabili_Client.tutorial[31]) //Visualizza il resto al termine... 
                            {
                                //btn_Mappa.Visible = false; //Mappa (Non funziona)
                                //ico_Notifiche.Visible = false; //Notifiche, non ancora implementato.
                            }
                        }

                        UnlockSoldierTier(livello_Esercito); //Controlla lo sblocco delle truppe in base al livello esercito ed in base al livello selezionato per la visualizzazione

                        toolTip1.SetToolTip(this.btn_I, $"Il livello II si blocca raggiunto il livello: {Variabili_Client.truppe_II}");
                        toolTip1.SetToolTip(this.btn_II, $"Il livello III si blocca raggiunto il livello: {Variabili_Client.truppe_III}");
                        toolTip1.SetToolTip(this.btn_III, $"Il livello IV si blocca raggiunto il livello: {Variabili_Client.truppe_IV}");
                        toolTip1.SetToolTip(this.btn_IV, $"Il livello V si blocca raggiunto il livello: {Variabili_Client.truppe_V}");

                        txt_Terreno_1.Text = Variabili_Client.Terreni_Virtuali.Comune.Quantità;
                        txt_Terreno_2.Text = Variabili_Client.Terreni_Virtuali.NonComune.Quantità;
                        txt_Terreno_3.Text = Variabili_Client.Terreni_Virtuali.Raro.Quantità;
                        txt_Terreno_4.Text = Variabili_Client.Terreni_Virtuali.Epico.Quantità;
                        txt_Terreno_5.Text = Variabili_Client.Terreni_Virtuali.Leggendario.Quantità;

                        lbl_Guerrieri_Max.Text = $"Max: {Variabili_Client.Reclutamento.Guerrieri_Max.Quantità}";
                        lbl_Lanceri_Max.Text = $"Max: {Variabili_Client.Reclutamento.Lanceri_Max.Quantità}";
                        lbl_Arceri_Max.Text = $"Max: {Variabili_Client.Reclutamento.Arceri_Max.Quantità}";
                        lbl_Catapulte_Max.Text = $"Max: {Variabili_Client.Reclutamento.Catapulte_Max.Quantità}";

                        if (strutture == "Militare") //Strutture
                        {
                            toolTip1.SetToolTip(this.ico_Structure_1, Variabili_Client.Costruzione.Workshop_Spade.Descrizione);
                            toolTip1.SetToolTip(this.ico_Structure_2, Variabili_Client.Costruzione.Workshop_Lance.Descrizione);
                            toolTip1.SetToolTip(this.ico_Structure_3, Variabili_Client.Costruzione.Workshop_Archi.Descrizione);
                            toolTip1.SetToolTip(this.ico_Structure_4, Variabili_Client.Costruzione.Workshop_Scudi.Descrizione);
                            toolTip1.SetToolTip(this.ico_Structure_5, Variabili_Client.Costruzione.Workshop_Armature.Descrizione);
                            toolTip1.SetToolTip(this.ico_Structure_6, Variabili_Client.Costruzione.Workshop_Frecce.Descrizione);

                            txt_Structure_1.Text = Variabili_Client.Costruzione.Workshop_Spade.Quantità;
                            txt_Structure_2.Text = Variabili_Client.Costruzione.Workshop_Lance.Quantità;
                            txt_Structure_3.Text = Variabili_Client.Costruzione.Workshop_Archi.Quantità;
                            txt_Structure_4.Text = Variabili_Client.Costruzione.Workshop_Scudi.Quantità;
                            txt_Structure_5.Text = Variabili_Client.Costruzione.Workshop_Armature.Quantità;
                            txt_Structure_6.Text = Variabili_Client.Costruzione.Workshop_Frecce.Quantità;

                            txt_Structure_Coda_1.Text = Variabili_Client.Costruzione_Coda.Workshop_Spade.Quantità;
                            txt_Structure_Coda_2.Text = Variabili_Client.Costruzione_Coda.Workshop_Lance.Quantità;
                            txt_Structure_Coda_3.Text = Variabili_Client.Costruzione_Coda.Workshop_Archi.Quantità;
                            txt_Structure_Coda_4.Text = Variabili_Client.Costruzione_Coda.Workshop_Scudi.Quantità;
                            txt_Structure_Coda_5.Text = Variabili_Client.Costruzione_Coda.Workshop_Armature.Quantità;
                            txt_Structure_Coda_6.Text = Variabili_Client.Costruzione_Coda.Workshop_Frecce.Quantità;
                        }
                        else
                        {
                            toolTip1.SetToolTip(this.ico_Structure_1, Variabili_Client.Costruzione.Fattorie.Descrizione);
                            toolTip1.SetToolTip(this.ico_Structure_2, Variabili_Client.Costruzione.Segherie.Descrizione);
                            toolTip1.SetToolTip(this.ico_Structure_3, Variabili_Client.Costruzione.CaveDiPietra.Descrizione);
                            toolTip1.SetToolTip(this.ico_Structure_4, Variabili_Client.Costruzione.Miniera_Ferro.Descrizione);
                            toolTip1.SetToolTip(this.ico_Structure_5, Variabili_Client.Costruzione.Miniera_Oro.Descrizione);
                            toolTip1.SetToolTip(this.ico_Structure_6, Variabili_Client.Costruzione.Case.Descrizione);

                            txt_Structure_1.Text = Variabili_Client.Costruzione.Fattorie.Quantità;
                            txt_Structure_2.Text = Variabili_Client.Costruzione.Segherie.Quantità;
                            txt_Structure_3.Text = Variabili_Client.Costruzione.CaveDiPietra.Quantità;
                            txt_Structure_4.Text = Variabili_Client.Costruzione.Miniera_Ferro.Quantità;
                            txt_Structure_5.Text = Variabili_Client.Costruzione.Miniera_Oro.Quantità;
                            txt_Structure_6.Text = Variabili_Client.Costruzione.Case.Quantità;

                            txt_Structure_Coda_1.Text = Variabili_Client.Costruzione_Coda.Fattorie.Quantità;
                            txt_Structure_Coda_2.Text = Variabili_Client.Costruzione_Coda.Segherie.Quantità;
                            txt_Structure_Coda_3.Text = Variabili_Client.Costruzione_Coda.CaveDiPietra.Quantità;
                            txt_Structure_Coda_4.Text = Variabili_Client.Costruzione_Coda.Miniera_Ferro.Quantità;
                            txt_Structure_Coda_5.Text = Variabili_Client.Costruzione_Coda.Miniera_Oro.Quantità;
                            txt_Structure_Coda_6.Text = Variabili_Client.Costruzione_Coda.Case.Quantità;
                        }

                       

                        if (Caserme == "Caserme") //Caserme
                        {
                            txt_Unit_1.Text = Variabili_Client.Costruzione.Caserme_Guerrieri.Quantità;
                            txt_Unit_2.Text = Variabili_Client.Costruzione.Caserme_Lanceri.Quantità;
                            txt_Unit_3.Text = Variabili_Client.Costruzione.Caserme_arceri.Quantità;
                            txt_Unit_4.Text = Variabili_Client.Costruzione.Caserme_Catapulte.Quantità;

                            txt_Unit_Coda_1.Text = Variabili_Client.Costruzione_Coda.Caserme_Guerrieri.Quantità;
                            txt_Unit_Coda_2.Text = Variabili_Client.Costruzione_Coda.Caserme_Lanceri.Quantità;
                            txt_Unit_Coda_3.Text = Variabili_Client.Costruzione_Coda.Caserme_arceri.Quantità;
                            txt_Unit_Coda_4.Text = Variabili_Client.Costruzione_Coda.Caserme_Catapulte.Quantità;

                            toolTip1.SetToolTip(this.ico_Unit_1, Variabili_Client.Costruzione.Caserme_Guerrieri.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_2, Variabili_Client.Costruzione.Caserme_Lanceri.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_3, Variabili_Client.Costruzione.Caserme_arceri.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_4, Variabili_Client.Costruzione.Caserme_Catapulte.Descrizione);
                        }
                        if (Caserme == "Esercito" && livello_Esercito == 1)
                        {
                            txt_Unit_1.Text = Variabili_Client.Reclutamento.Guerrieri_1.Quantità;
                            txt_Unit_2.Text = Variabili_Client.Reclutamento.Lanceri_1.Quantità;
                            txt_Unit_3.Text = Variabili_Client.Reclutamento.Arceri_1.Quantità;
                            txt_Unit_4.Text = Variabili_Client.Reclutamento.Catapulte_1.Quantità;
                            txt_Unit_Coda_1.Text = Variabili_Client.Reclutamento_Coda.Guerrieri_1.Quantità;
                            txt_Unit_Coda_2.Text = Variabili_Client.Reclutamento_Coda.Lanceri_1.Quantità;
                            txt_Unit_Coda_3.Text = Variabili_Client.Reclutamento_Coda.Arceri_1.Quantità;
                            txt_Unit_Coda_4.Text = Variabili_Client.Reclutamento_Coda.Catapulte_1.Quantità;

                            toolTip1.SetToolTip(this.ico_Unit_1, Variabili_Client.Reclutamento.Guerrieri_1.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_2, Variabili_Client.Reclutamento.Lanceri_1.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_3, Variabili_Client.Reclutamento.Arceri_1.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_4, Variabili_Client.Reclutamento.Catapulte_1.Descrizione);
                        }
                        if (Caserme == "Esercito" && livello_Esercito == 2)
                        {
                            // Sostituisci questo blocco per aggiornare le unità da livello 1 a livello 2
                            txt_Unit_1.Text = Variabili_Client.Reclutamento.Guerrieri_2.Quantità;
                            txt_Unit_2.Text = Variabili_Client.Reclutamento.Lanceri_2.Quantità;
                            txt_Unit_3.Text = Variabili_Client.Reclutamento.Arceri_2.Quantità;
                            txt_Unit_4.Text = Variabili_Client.Reclutamento.Catapulte_2.Quantità;
                            txt_Unit_Coda_1.Text = Variabili_Client.Reclutamento_Coda.Guerrieri_2.Quantità;
                            txt_Unit_Coda_2.Text = Variabili_Client.Reclutamento_Coda.Lanceri_2.Quantità;
                            txt_Unit_Coda_3.Text = Variabili_Client.Reclutamento_Coda.Arceri_2.Quantità;
                            txt_Unit_Coda_4.Text = Variabili_Client.Reclutamento_Coda.Catapulte_2.Quantità;

                            toolTip1.SetToolTip(this.ico_Unit_1, Variabili_Client.Reclutamento.Guerrieri_2.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_2, Variabili_Client.Reclutamento.Lanceri_2.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_3, Variabili_Client.Reclutamento.Arceri_2.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_4, Variabili_Client.Reclutamento.Catapulte_2.Descrizione);
                        }
                        if (Caserme == "Esercito" && livello_Esercito == 3)
                        {
                            // Sostituisci questo blocco per aggiornare le unità da livello 2 a livello 3
                            txt_Unit_1.Text = Variabili_Client.Reclutamento.Guerrieri_3.Quantità;
                            txt_Unit_2.Text = Variabili_Client.Reclutamento.Lanceri_3.Quantità;
                            txt_Unit_3.Text = Variabili_Client.Reclutamento.Arceri_3.Quantità;
                            txt_Unit_4.Text = Variabili_Client.Reclutamento.Catapulte_3.Quantità;
                            txt_Unit_Coda_1.Text = Variabili_Client.Reclutamento_Coda.Guerrieri_3.Quantità;
                            txt_Unit_Coda_2.Text = Variabili_Client.Reclutamento_Coda.Lanceri_3.Quantità;
                            txt_Unit_Coda_3.Text = Variabili_Client.Reclutamento_Coda.Arceri_3.Quantità;
                            txt_Unit_Coda_4.Text = Variabili_Client.Reclutamento_Coda.Catapulte_3.Quantità;

                            toolTip1.SetToolTip(this.ico_Unit_1, Variabili_Client.Reclutamento.Guerrieri_3.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_2, Variabili_Client.Reclutamento.Lanceri_3.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_3, Variabili_Client.Reclutamento.Arceri_3.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_4, Variabili_Client.Reclutamento.Catapulte_3.Descrizione);
                        }
                        if (Caserme == "Esercito" && livello_Esercito == 4)
                        {
                            // Sostituisci questo blocco per aggiornare le unità da livello 3 a livello 4
                            txt_Unit_1.Text = Variabili_Client.Reclutamento.Guerrieri_4.Quantità;
                            txt_Unit_2.Text = Variabili_Client.Reclutamento.Lanceri_4.Quantità;
                            txt_Unit_3.Text = Variabili_Client.Reclutamento.Arceri_4.Quantità;
                            txt_Unit_4.Text = Variabili_Client.Reclutamento.Catapulte_4.Quantità;
                            txt_Unit_Coda_1.Text = Variabili_Client.Reclutamento_Coda.Guerrieri_4.Quantità;
                            txt_Unit_Coda_2.Text = Variabili_Client.Reclutamento_Coda.Lanceri_4.Quantità;
                            txt_Unit_Coda_3.Text = Variabili_Client.Reclutamento_Coda.Arceri_4.Quantità;
                            txt_Unit_Coda_4.Text = Variabili_Client.Reclutamento_Coda.Catapulte_4.Quantità;

                            toolTip1.SetToolTip(this.ico_Unit_1, Variabili_Client.Reclutamento.Guerrieri_4.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_2, Variabili_Client.Reclutamento.Lanceri_4.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_3, Variabili_Client.Reclutamento.Arceri_4.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_4, Variabili_Client.Reclutamento.Catapulte_4.Descrizione);
                        }
                        if (Caserme == "Esercito" && livello_Esercito == 5)
                        {
                            // Sostituisci questo blocco per aggiornare le unità da livello 4 a livello 5
                            txt_Unit_1.Text = Variabili_Client.Reclutamento.Guerrieri_5.Quantità;
                            txt_Unit_2.Text = Variabili_Client.Reclutamento.Lanceri_5.Quantità;
                            txt_Unit_3.Text = Variabili_Client.Reclutamento.Arceri_5.Quantità;
                            txt_Unit_4.Text = Variabili_Client.Reclutamento.Catapulte_5.Quantità;
                            txt_Unit_Coda_1.Text = Variabili_Client.Reclutamento_Coda.Guerrieri_5.Quantità;
                            txt_Unit_Coda_2.Text = Variabili_Client.Reclutamento_Coda.Lanceri_5.Quantità;
                            txt_Unit_Coda_3.Text = Variabili_Client.Reclutamento_Coda.Arceri_5.Quantità;
                            txt_Unit_Coda_4.Text = Variabili_Client.Reclutamento_Coda.Catapulte_5.Quantità;

                            toolTip1.SetToolTip(this.ico_Unit_1, Variabili_Client.Reclutamento.Guerrieri_5.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_2, Variabili_Client.Reclutamento.Lanceri_5.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_3, Variabili_Client.Reclutamento.Arceri_5.Descrizione);
                            toolTip1.SetToolTip(this.ico_Unit_4, Variabili_Client.Reclutamento.Catapulte_5.Descrizione);
                        }

                        lbl_Timer_Costruzione.Text = "Build: " + Variabili_Client.Utente.Tempo_Costruzione;
                        lbl_Timer_Addestramento.Text = "Recruit: " + Variabili_Client.Utente.Tempo_Reclutamento;

                        if (lbl_Timer_Costruzione.Text == "Build: 0s")
                        {
                            pictureBox_Speed_Costruzione.Visible = false;
                            lbl_Coda_Costruzione.Text = $"Code disponibili: {Variabili_Client.Utente.Code_Costruzione}/{Variabili_Client.Utente.Code_Costruzione}";
                        }
                        else
                        {
                            pictureBox_Speed_Costruzione.Visible = true;
                            lbl_Coda_Costruzione.Text = $"Code disponibili: {Convert.ToInt32(Variabili_Client.Utente.Code_Costruzione) - Convert.ToInt32(Variabili_Client.Utente.Code_Costruzione_Disponibili)}/{Variabili_Client.Utente.Code_Costruzione}";
                        }

                        if (lbl_Timer_Addestramento.Text == "Recruit: 0s" || Caserme == "Caserme")
                        {
                            pictureBox_Speed_Reclutamento.Visible = false;
                            lbl_Coda_Reclutamento.Text = $"Code disponibili: {Convert.ToInt32(Variabili_Client.Utente.Code_Reclutamento) - Convert.ToInt32(Variabili_Client.Utente.Code_Reclutamento_Disponibili)}/{Variabili_Client.Utente.Code_Reclutamento}";
                        }
                        else if (Caserme == "Esercito") // Non serve la coda, finisce in building
                        {
                            pictureBox_Speed_Reclutamento.Visible = true;
                            lbl_Coda_Reclutamento.Text = $"Code disponibili: {Convert.ToInt32(Variabili_Client.Utente.Code_Reclutamento) - Convert.ToInt32(Variabili_Client.Utente.Code_Reclutamento_Disponibili)}/{Variabili_Client.Utente.Code_Reclutamento}";
                        }
                    }));

                }
            }
        }

        private void GUI()
        {
            btn_Acquista_Terreni.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);
            lbl_Guerrieri_Max.Font = new Font("Cinzel Decorative", 7);
            lbl_Lanceri_Max.Font = new Font("Cinzel Decorative", 7);
            lbl_Arceri_Max.Font = new Font("Cinzel Decorative", 7);
            lbl_Catapulte_Max.Font = new Font("Cinzel Decorative", 7);

            lbl_Timer_Addestramento.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            lbl_Timer_Costruzione.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);

            Banner();
            Feudi();
            Edifici();
            Esercito();
            UnlockSoldierTier(livello_Esercito);
        }
        private void Edifici()
        {
            // Strutture
            txt_Structure_1.BackColor = Color.FromArgb(229, 208, 181);
            txt_Structure_2.BackColor = Color.FromArgb(229, 208, 181);
            txt_Structure_3.BackColor = Color.FromArgb(229, 208, 181);
            txt_Structure_4.BackColor = Color.FromArgb(229, 208, 181);
            txt_Structure_5.BackColor = Color.FromArgb(229, 208, 181);
            txt_Structure_6.BackColor = Color.FromArgb(229, 208, 181);

            txt_Structure_Coda_1.BackColor = Color.FromArgb(229, 208, 181);
            txt_Structure_Coda_2.BackColor = Color.FromArgb(229, 208, 181);
            txt_Structure_Coda_3.BackColor = Color.FromArgb(229, 208, 181);
            txt_Structure_Coda_4.BackColor = Color.FromArgb(229, 208, 181);
            txt_Structure_Coda_5.BackColor = Color.FromArgb(229, 208, 181);
            txt_Structure_Coda_6.BackColor = Color.FromArgb(229, 208, 181);

            txt_Unit_1.BackColor = Color.FromArgb(229, 208, 181);
            txt_Unit_2.BackColor = Color.FromArgb(229, 208, 181);
            txt_Unit_3.BackColor = Color.FromArgb(229, 208, 181);
            txt_Unit_4.BackColor = Color.FromArgb(229, 208, 181);

            txt_Unit_Coda_1.BackColor = Color.FromArgb(229, 208, 181);
            txt_Unit_Coda_2.BackColor = Color.FromArgb(229, 208, 181);
            txt_Unit_Coda_3.BackColor = Color.FromArgb(229, 208, 181);
            txt_Unit_Coda_4.BackColor = Color.FromArgb(229, 208, 181);

            txt_Terreno_1.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Terreno_2.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Terreno_3.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Terreno_4.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Terreno_5.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);

            txt_Structure_1.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Structure_2.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Structure_3.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Structure_4.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Structure_5.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Structure_6.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);

            txt_Structure_Coda_1.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Structure_Coda_2.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Structure_Coda_3.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Structure_Coda_4.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Structure_Coda_5.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Structure_Coda_6.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);

        }
        private void Esercito()
        {
            btn_I.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            btn_II.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            btn_III.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            btn_IV.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            btn_V.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);

            txt_Unit_1.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Unit_2.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Unit_3.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Unit_4.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);

            txt_Unit_Coda_1.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Unit_Coda_2.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Unit_Coda_3.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
            txt_Unit_Coda_4.Font = new Font("Cinzel Decorative", 7.5f, FontStyle.Bold);
        }
        private void Banner()
        {
            banner_4.BackColor = Color.FromArgb(100, 229, 208, 181);
            banner_4.BringToFront();
            banner_5.BackColor = Color.FromArgb(100, 229, 208, 181);
            banner_5.BringToFront();
            banner_6.BackColor = Color.FromArgb(100, 229, 208, 181);
            banner_6.BringToFront();
            banner_7.BackColor = Color.FromArgb(100, 229, 208, 181);
            banner_7.BringToFront();
            banner_8.BackColor = Color.FromArgb(100, 229, 208, 181);
            banner_8.BringToFront();
            banner_9.BackColor = Color.FromArgb(100, 229, 208, 181);
            banner_9.BringToFront();
            banner_10.BackColor = Color.FromArgb(100, 229, 208, 181);
            banner_10.BringToFront();
        }
        private void Feudi()
        {
            // Risorse
            txt_Terreno_1.BackColor = Color.FromArgb(229, 208, 181);
            txt_Terreno_2.BackColor = Color.FromArgb(229, 208, 181);
            txt_Terreno_3.BackColor = Color.FromArgb(229, 208, 181);
            txt_Terreno_4.BackColor = Color.FromArgb(229, 208, 181);
            txt_Terreno_5.BackColor = Color.FromArgb(229, 208, 181);

        }

        private async void btn_Civile_Militare_Click(object sender, EventArgs e)
        {
            if (groupBox_Strutture.Text == "Strutture Civili")
            {
                groupBox_Strutture.Text = "Strutture Militari";
                strutture = "Militare";
                ico_Structure_1.BackgroundImage = Properties.Resources.Workshop_Spade_V2;
                ico_Structure_2.BackgroundImage = Properties.Resources.Workshop_Lance_V2;
                ico_Structure_3.BackgroundImage = Properties.Resources.Workshop_Archi_V2;
                ico_Structure_4.BackgroundImage = Properties.Resources.Workshop_Scudi_V2;
                ico_Structure_5.BackgroundImage = Properties.Resources.Workshop_Armature_V2;
                ico_Structure_6.BackgroundImage = Properties.Resources.Workshop_Frecce_V2;
                if (Variabili_Client.tutorial_Attivo == true && await Main.TutorialPrecedentiCompletati(10))
                    ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{10}");
                return;
            }

            groupBox_Strutture.Text = "Strutture Civili";
            strutture = "Civile";
            ico_Structure_1.BackgroundImage = Properties.Resources.Fattoria_V2;
            ico_Structure_2.BackgroundImage = Properties.Resources.Segheria_V2;
            ico_Structure_3.BackgroundImage = Properties.Resources.CavaDiPietra_V2;
            ico_Structure_4.BackgroundImage = Properties.Resources.MinieraFerro_V2;
            ico_Structure_5.BackgroundImage = Properties.Resources.MinieraOro_V2;
            ico_Structure_6.BackgroundImage = Properties.Resources.Abitazioni_V2;
        }
        private void btn_Info_Terreni_Virtuali_Click(object sender, EventArgs e)
        {
            Terreni_Virtuali form_Gioco = new Terreni_Virtuali();
            form_Gioco.ShowDialog();
        }
        private void btn_Scambia_Click(object sender, EventArgs e)
        {
            Scambia_Diamanti.nome_Form = "Scambia_Diamanti";
            Scambia_Diamanti form_Gioco = new Scambia_Diamanti();
            form_Gioco.ShowDialog();
        }
        private async void ico_10_MouseClick(object sender, MouseEventArgs e)
        {
            if (Variabili_Client.tutorial_Attivo == true && await Main.TutorialPrecedentiCompletati(27))
                ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{27}");
            Statistiche form_Gioco = new Statistiche();
            form_Gioco.Show();
        }
        void UnlockSoldierTier(int tier)
        {
            int livello = Convert.ToInt32(Variabili_Client.Utente.Livello);

            btn_I.BackColor = Color.FromArgb(229, 208, 181);
            btn_II.BackColor = Color.FromArgb(229, 208, 181);
            btn_III.BackColor = Color.FromArgb(229, 208, 181);
            btn_IV.BackColor = Color.FromArgb(229, 208, 181);
            btn_V.BackColor = Color.FromArgb(229, 208, 181);

            //Controllo se il livello è maggiore abilita il tier specifico dei soldati
            if (livello_Esercito != 1)
            {
                btn_I.Enabled = true;
                btn_I.BackgroundImage = Properties.Resources.Texture_Wood_1;
            }
            else btn_I.Enabled = false;

            if (livello < Convert.ToInt32(Variabili_Client.truppe_II))
            {
                btn_II.Enabled = false;
                btn_II.BackColor = Color.FromArgb(206, 206, 206);
            }
            else
            {
                if (livello_Esercito != 2) btn_II.Enabled = true;
                else btn_II.Enabled = false;

                btn_II.BackgroundImage = Properties.Resources.Texture_Wood_1;
            }
            if (livello < Convert.ToInt32(Variabili_Client.truppe_III))
            {
                btn_III.Enabled = false;
                btn_III.BackColor = Color.FromArgb(206, 206, 206);
            }
            else
            {
                if (livello_Esercito != 3) btn_III.Enabled = true;
                else btn_III.Enabled = false;
                btn_III.BackgroundImage = Properties.Resources.Texture_Wood_1;
            }
            if (livello < Convert.ToInt32(Variabili_Client.truppe_IV))
            {
                btn_IV.Enabled = false;
                btn_IV.BackColor = Color.FromArgb(206, 206, 206);
            }
            else
            {
                if (livello_Esercito != 4) btn_IV.Enabled = true;
                else btn_IV.Enabled = false;
                btn_IV.BackgroundImage = Properties.Resources.Texture_Wood_1;
            }
            if (livello < Convert.ToInt32(Variabili_Client.truppe_V))
            {
                btn_V.Enabled = false;
                btn_V.BackColor = Color.FromArgb(206, 206, 206);
            }
            else
            {
                if (livello_Esercito != 5) btn_V.Enabled = true;
                else btn_V.Enabled = false;
                btn_V.BackgroundImage = Properties.Resources.Texture_Wood_1;
            }
        }
        private void btn_I_Click(object sender, EventArgs e)
        {
            livello_Esercito = 1;
            UnlockSoldierTier(livello_Esercito);
            this.ActiveControl = panel_Log; // assegna il focus al bottone
        }
        private void btn_II_Click(object sender, EventArgs e)
        {
            livello_Esercito = 2;
            UnlockSoldierTier(livello_Esercito);
            this.ActiveControl = panel_Log; // assegna il focus al bottone
        }
        private void btn_III_Click(object sender, EventArgs e)
        {
            livello_Esercito = 3;
            UnlockSoldierTier(livello_Esercito);
            this.ActiveControl = panel_Log; // assegna il focus al bottone
        }
        private void btn_IV_Click(object sender, EventArgs e)
        {
            livello_Esercito = 4;
            UnlockSoldierTier(livello_Esercito);
            this.ActiveControl = panel_Log; // assegna il focus al bottone
        }
        private void btn_V_Click(object sender, EventArgs e)
        {
            livello_Esercito = 5;
            UnlockSoldierTier(livello_Esercito);
            this.ActiveControl = panel_Log; // assegna il focus al bottone
        }

        private void pictureBox_Speed_Costruzione_Click(object sender, EventArgs e)
        {
            Velocizza_Diamanti.tipo = "Costruzione";
            Velocizza_Diamanti form_Gioco = new Velocizza_Diamanti();
            form_Gioco.ShowDialog();
        }
        private void pictureBox_Speed_Reclutamento_Click(object sender, EventArgs e)
        {
            Velocizza_Diamanti.tipo = "Reclutamento";
            Velocizza_Diamanti form_Gioco = new Velocizza_Diamanti();
            form_Gioco.ShowDialog();
        }

        private void btn_Esercito_Caserme_Click(object sender, EventArgs e)
        {
            if (Caserme == "Esercito")
            {
                groupBox_Esercito.Text = "Caserme";
                panel_Sfondo_Bottoni.Visible = false;
                lbl_Coda_Reclutamento.Visible = false;
                lbl_Timer_Addestramento.Visible = false;

                ico_Unit_1.BackgroundImage = Properties.Resources.Caserma_Guerieri_V2;
                ico_Unit_2.BackgroundImage = Properties.Resources.Caserma_Lanceri_V2;
                ico_Unit_3.BackgroundImage = Properties.Resources.Caserma_Arcieri_V2;
                ico_Unit_4.BackgroundImage = Properties.Resources.Caserma_Catapulte_V2;

                lbl_Guerrieri_Max.Visible = false;
                lbl_Lanceri_Max.Visible = false;
                lbl_Arceri_Max.Visible = false;
                lbl_Catapulte_Max.Visible = false;

                Caserme = "Caserme";
            }
            else
            {
                groupBox_Esercito.Text = "Esercito";
                panel_Sfondo_Bottoni.Visible = true;
                lbl_Coda_Reclutamento.Visible = true;
                lbl_Timer_Addestramento.Visible = true;
                ico_Unit_1.BackgroundImage = Properties.Resources.Spade_V2;
                ico_Unit_2.BackgroundImage = Properties.Resources.Spade_V2;
                ico_Unit_3.BackgroundImage = Properties.Resources.Spade_V2;
                ico_Unit_4.BackgroundImage = Properties.Resources.Spade_V2;

                lbl_Guerrieri_Max.Visible = true;
                lbl_Lanceri_Max.Visible = true;
                lbl_Arceri_Max.Visible = true;
                lbl_Catapulte_Max.Visible = true;

                Caserme = "Esercito";
            }

        }

        private async void btn_Acquista_Terreni_Click(object sender, EventArgs e)
        {
            // Messaggio di conferma chiaro
            var result = MessageBox.Show(
                $"Sei sicuro di voler acquistare un feudo?\n" +
                $"Costo: {Variabili_Client.Utente.Costo_terreni_Virtuali} Diamanti Viola\n" +
                $"Diamanti attuali: {Variabili_Client.Utente_Risorse.Diamond_Viola}",
                "Conferma acquisto",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                // Esegui l'acquisto
                ClientConnection.TestClient.Send($"Costruzione_Terreni|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|");
                if (Variabili_Client.tutorial_Attivo == true && await Main.TutorialPrecedentiCompletati(8))
                    ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{8}");
            }
        }

        private void btn_Mappa_Click(object sender, EventArgs e)
        {
            Mappa form_Gioco = new Mappa();
            form_Gioco.ShowDialog();
        }

        private void ico_Notifiche_MouseClick(object sender, MouseEventArgs e)
        {
            Notifiche form_Gioco = new Notifiche();
            form_Gioco.ShowDialog();
        }

        private void ico_13_MouseClick(object sender, MouseEventArgs e)
        {
            Scambia_Diamanti.nome_Form = "Scambia_Tributi";
            Scambia_Diamanti form_Gioco = new Scambia_Diamanti();
            form_Gioco.ShowDialog();
        }

        private void Gioco_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts.Cancel();
        }
    }
}
