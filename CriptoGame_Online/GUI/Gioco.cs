using CriptoGame_Online.GUI;
using CriptoGame_Online.Strumenti;
using Strategico_V2;

namespace CriptoGame_Online
{
    public partial class Gioco : Form
    {
        public static Gioco Instance { get; private set; }
        public static GameTextBox logBox;
        public ToolTip toolTip1;
        static string strutture = "Civile";
        static string tipo_Risorse = "Civile";
        static string Caserme = "Esercito";
        static int livello_Esercito = 1;

        public Gioco()
        {
            InitializeComponent();
            Instance = this;

            logBox = new GameTextBox()
            {
                Dock = DockStyle.Fill
            };

            panel_Log.Controls.Add(logBox);

            // Riga semplice
            logBox.AddLine("Hai trovato una pozione!", Color.LightGreen);
            // Riga con segmenti colorati
            logBox.AddLine(new[]
            {
            ("[Sistema] ", Color.Cyan),
            ("Benvenuto nell'arena!", Color.White)
        });

            // Riga con nickname colorato + messaggio
            logBox.AddLine(new[]
            {
            ("Player1: ", Color.Orange),
            ("Attacco critico! Hai ricevuto 1", Color.Red)
        }, Properties.Resources.diamond_1);

            logBox.AddLine(new[]
            {
            ("Player1: ", Color.Orange),
            ("Attacco critico!", Color.Red)
        }, Properties.Resources.diamond_1);

        }

        public static void Log_Update(string messaggio)
        {
            if (logBox != null)
            {
                logBox.Invoke(new Action(() => logBox.AddLine($"{messaggio}", Color.White)));
            }
        }

        private void Gioco_Load(object sender, EventArgs e)
        {

            ToolTip toolTip1 = new ToolTip();

            // Imposta qualche proprietà opzionale
            toolTip1.AutoPopDelay = 5000;   // tempo massimo di visualizzazione (ms)
            toolTip1.InitialDelay = 500;    // ritardo prima che appaia
            toolTip1.ReshowDelay = 200;     // ritardo tra un controllo e l'altro
            toolTip1.ShowAlways = true;     // mostra anche se la finestra non è attiva

            // Pannelli e Cose
            panel_1.BackColor = Color.FromArgb(100, 229, 208, 181);
            panel_2.BackColor = Color.FromArgb(100, 229, 208, 181);
            panel_3.BackColor = Color.FromArgb(100, 229, 208, 181);
            panel_Sfondo_Bottoni.BackColor = Color.FromArgb(50, 180, 150, 100);

            panel_Image_2.BackColor = Color.FromArgb(100, 218, 193, 163);
            btn_Acquista_Terreni.BackColor = Color.FromArgb(218, 193, 163);

            groupBox_Strutture.BackColor = Color.FromArgb(100, 229, 208, 181);
            groupBox_Esercito.BackColor = Color.FromArgb(100, 229, 208, 181);

            ico_2.BackColor = Color.FromArgb(229, 208, 181);
            ico_9.BackColor = Color.FromArgb(100, 229, 208, 181);

            txt_Tipi_Risorse.BackColor = Color.FromArgb(229, 208, 181);
            txt_Virtual_Dolla.BackColor = Color.FromArgb(229, 208, 181);

            //Load Guid
            GUI();
            Update();
            Task.Run(() => Gui_Update());
        }

        async void Gui_Update()
        {
            ToolTip toolTip1 = new ToolTip();
            while (true)
            {
                Thread.Sleep(1000);
                logBox.Invoke((Action)(async () =>
                {
                    toolTip1.SetToolTip(this.ico_11, $"Diamanti Blu, fondamentali per una migliore gestione della città.");
                    toolTip1.SetToolTip(this.ico_12, $"Diamanti Viola, utilizzato principalmente per l'acquito di terreni virtuali.");
                    toolTip1.SetToolTip(this.ico_13, $"Dollari Virtuali, risorsa ottenuta tramite i terreni virtuali.");

                    toolTip1.SetToolTip(this.ico_8, Variabili_Client.Esperienza_Desc);
                    toolTip1.SetToolTip(this.ico_9, Variabili_Client.Livello_Desc);

                    txt_Username.Text = Variabili_Client.Utente.Username;
                    txt_Livello.Text = Variabili_Client.Utente.Livello;
                    txt_Esperienza.Text = Variabili_Client.Utente.Esperienza + " XP";

                    txt_Diamond_1.Text = Variabili_Client.Utente_Risorse.Diamond_Blu;
                    txt_Diamond_2.Text = Variabili_Client.Utente_Risorse.Diamond_Viola;
                    txt_Virtual_Dolla.Text = Variabili_Client.Utente_Risorse.Virtual_Dolla;

                    txt_Terreno_1.Text = Variabili_Client.Terreni_Virtuali.Comune.Quantità;
                    txt_Terreno_2.Text = Variabili_Client.Terreni_Virtuali.NonComune.Quantità;
                    txt_Terreno_3.Text = Variabili_Client.Terreni_Virtuali.Raro.Quantità;
                    txt_Terreno_4.Text = Variabili_Client.Terreni_Virtuali.Epico.Quantità;
                    txt_Terreno_5.Text = Variabili_Client.Terreni_Virtuali.Leggendario.Quantità;

                    if (strutture == "Militare")
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


                    if (tipo_Risorse == "Militare")
                    {
                        toolTip1.SetToolTip(this.ico_1, $"Spade, utilissimo per l'addestramento delle unità. \r\n Produzione: {Variabili_Client.Utente_Risorse.Spade_s} \r\n Limite: {Variabili_Client.Utente_Risorse.Spade_Limite}");
                        toolTip1.SetToolTip(this.ico_2, $"Lance, utilissimo per l'addestramento delle unità. \r\n Produzione: {Variabili_Client.Utente_Risorse.Lance_s} \r\n Limite: {Variabili_Client.Utente_Risorse.Lance_Limite}");
                        toolTip1.SetToolTip(this.ico_3, $"Archi, utilissimo per l'addestramento delle unità. \r\n Produzione: {Variabili_Client.Utente_Risorse.Archi_s} \r\n Limite: {Variabili_Client.Utente_Risorse.Archi_Limite}");
                        toolTip1.SetToolTip(this.ico_4, $"Scudi, utilissimo per l'addestramento delle unità. \r\n Produzione: {Variabili_Client.Utente_Risorse.Scudi_s} \r\n Limite: {Variabili_Client.Utente_Risorse.Scudi_Limite}");
                        toolTip1.SetToolTip(this.ico_5, $"Armature, utilissimo per l'addestramento delle unità. \r\n Produzione: {Variabili_Client.Utente_Risorse.Armature_s} \r\n Limite: {Variabili_Client.Utente_Risorse.Armature_Limite}");
                        toolTip1.SetToolTip(this.ico_6, $"Frecce, utilissimo per l'addestramento delle unità. \r\n Produzione: {Variabili_Client.Utente_Risorse.Frecce_s} \r\n Limite: {Variabili_Client.Utente_Risorse.Frecce_Limite}");

                        txt_Risorsa1.Text = Variabili_Client.Utente_Risorse.Spade;
                        txt_Risorsa2.Text = Variabili_Client.Utente_Risorse.Lance;
                        txt_Risorsa3.Text = Variabili_Client.Utente_Risorse.Archi;
                        txt_Risorsa4.Text = Variabili_Client.Utente_Risorse.Scudi;
                        txt_Risorsa5.Text = Variabili_Client.Utente_Risorse.Armature;
                        txt_Risorsa6.Text = Variabili_Client.Utente_Risorse.Frecce;
                    }
                    else
                    {
                        toolTip1.SetToolTip(this.ico_1, $"Cibo, utilissimo per il funzionamento della città. \r\n Produzione: {Variabili_Client.Utente_Risorse.Cibo_s} \r\n Consumo: {Variabili_Client.Utente_Risorse.Mantenimento_Cibo} \r\n Limite: {Variabili_Client.Utente_Risorse.Cibo_Limite}");
                        toolTip1.SetToolTip(this.ico_2, $"Legno, utilissimo per il funzionamento della città. \r\n Produzione: {Variabili_Client.Utente_Risorse.Legna_s} \r\n Limite: {Variabili_Client.Utente_Risorse.Legna_Limite}");
                        toolTip1.SetToolTip(this.ico_3, $"Pietra, utilissimo per il funzionamento della città. \r\n Produzione: {Variabili_Client.Utente_Risorse.Pietra_s} \r\n Limite: {Variabili_Client.Utente_Risorse.Pietra_Limite}");
                        toolTip1.SetToolTip(this.ico_4, $"Ferro, utilissimo per il funzionamento della città. \r\n Produzione: {Variabili_Client.Utente_Risorse.Ferro_s} \r\n Limite: {Variabili_Client.Utente_Risorse.Ferro_Limite}");
                        toolTip1.SetToolTip(this.ico_5, $"Oro, utilissimo per il funzionamento della città. \r\n Produzione: {Variabili_Client.Utente_Risorse.Oro_s} \r\n Consumo: {Variabili_Client.Utente_Risorse.Mantenimento_Oro} \r\n Limite: {Variabili_Client.Utente_Risorse.Oro_Limite}");
                        toolTip1.SetToolTip(this.ico_6, $"Popolazione, utilissimo per il funzionamento della città. \r\n Produzione: {Variabili_Client.Utente_Risorse.Popolazione_s} \r\n Limite: {Variabili_Client.Utente_Risorse.Popolazione_Limite}");

                        txt_Risorsa1.Text = Variabili_Client.Utente_Risorse.Cibo;
                        txt_Risorsa2.Text = Variabili_Client.Utente_Risorse.Legna;
                        txt_Risorsa3.Text = Variabili_Client.Utente_Risorse.Pietra;
                        txt_Risorsa4.Text = Variabili_Client.Utente_Risorse.Ferro;
                        txt_Risorsa5.Text = Variabili_Client.Utente_Risorse.Oro;
                        txt_Risorsa6.Text = Variabili_Client.Utente_Risorse.Popolazione;
                    }

                    //Caserme
                    if (Caserme == "Caserme")
                    {
                        txt_Unit_1.Text = Variabili_Client.Costruzione.Caserme_Guerrieri.Quantità;
                        txt_Unit_2.Text = Variabili_Client.Costruzione.Caserme_Lanceri.Quantità;
                        txt_Unit_3.Text = Variabili_Client.Costruzione.Caserme_arceri.Quantità;
                        txt_Unit_4.Text = Variabili_Client.Costruzione.Caserme_Catapulte.Quantità;

                        txt_Unit_Coda_1.Text = Variabili_Client.Costruzione_Coda.Caserme_Guerrieri.Quantità;
                        txt_Unit_Coda_2.Text = Variabili_Client.Costruzione_Coda.Caserme_Lanceri.Quantità;
                        txt_Unit_Coda_3.Text = Variabili_Client.Costruzione_Coda.Caserme_arceri.Quantità;
                        txt_Unit_Coda_4.Text = Variabili_Client.Costruzione_Coda.Caserme_Catapulte.Quantità;
                    }

                    if (btn_I.Enabled == false && Caserme == "Esercito")
                    {
                        txt_Unit_1.Text = Variabili_Client.Reclutamento.Guerrieri_1.Quantità;
                        txt_Unit_2.Text = Variabili_Client.Reclutamento.Lanceri_1.Quantità;
                        txt_Unit_3.Text = Variabili_Client.Reclutamento.Arceri_1.Quantità;
                        txt_Unit_4.Text = Variabili_Client.Reclutamento.Catapulte_1.Quantità;
                        txt_Unit_Coda_1.Text = Variabili_Client.Reclutamento_Coda.Guerrieri_1.Quantità;
                        txt_Unit_Coda_2.Text = Variabili_Client.Reclutamento_Coda.Lanceri_1.Quantità;
                        txt_Unit_Coda_3.Text = Variabili_Client.Reclutamento_Coda.Arceri_1.Quantità;
                        txt_Unit_Coda_4.Text = Variabili_Client.Reclutamento_Coda.Catapulte_1.Quantità;
                    }
                    if (btn_II.Enabled == false)
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
                    }
                    if (btn_III.Enabled == false)
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
                    }
                    if (btn_IV.Enabled == false)
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
                    }
                    if (btn_V.Enabled == false)
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
                        lbl_Coda_Costruzione.Text = $"Code disponibili: {0}/{Variabili_Client.Utente.Code_Costruzione}";
                    }

                    if (lbl_Timer_Addestramento.Text == "Recruit: 0s" || Caserme == "Caserme")
                    {
                        pictureBox_Speed_Reclutamento.Visible = false;
                        lbl_Coda_Reclutamento.Text = $"Code disponibili: {Variabili_Client.Utente.Code_Reclutamento}/{Variabili_Client.Utente.Code_Reclutamento}";
                    }
                    else if (Caserme == "Esercito") // Non serve la coda, finisce in building
                        pictureBox_Speed_Reclutamento.Visible = true;
                    else
                        lbl_Coda_Reclutamento.Text = $"Code disponibili: {0}/{Variabili_Client.Utente.Code_Reclutamento}";
                }));
            }
        }

        private void GUI()
        {
            btn_Acquista_Terreni.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);

            Btn_Costruzione.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);
            btn_Citta.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);
            btn_Shop.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);
            btn_Cambia_Risorse.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);
            btn_Quest_Mensile.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);


            lbl_Timer_Addestramento.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);
            lbl_Timer_Costruzione.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);
            txt_Tipi_Risorse.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);

            Banner();
            Edifici_1();
            Risorse_1();
            Esercito_1();
        }
        private void Update()
        {
            Risorse();
            Edifici();
            Esercito();
        }
        private void Risorse()
        {
            txt_Livello.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Esperienza.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Username.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Diamond_2.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Diamond_1.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Virtual_Dolla.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);

            txt_Risorsa1.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Risorsa2.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Risorsa3.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Risorsa4.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Risorsa5.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Risorsa6.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);

        }
        private void Edifici()
        {
            txt_Terreno_1.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Terreno_2.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Terreno_3.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Terreno_4.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Terreno_5.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);

            txt_Structure_1.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Structure_2.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Structure_3.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Structure_4.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Structure_5.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Structure_6.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);

            txt_Structure_Coda_1.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Structure_Coda_2.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Structure_Coda_3.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Structure_Coda_4.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Structure_Coda_5.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Structure_Coda_6.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);

        }
        private void Esercito()
        {
            txt_Unit_1.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Unit_2.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Unit_3.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Unit_4.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);

            txt_Unit_Coda_1.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Unit_Coda_2.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Unit_Coda_3.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            txt_Unit_Coda_4.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);

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
        private void Edifici_1()
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
        }
        private void Risorse_1()
        {
            // Risorse
            txt_Risorsa1.BackColor = Color.FromArgb(229, 208, 181);
            txt_Risorsa2.BackColor = Color.FromArgb(229, 208, 181);
            txt_Risorsa3.BackColor = Color.FromArgb(229, 208, 181);
            txt_Risorsa4.BackColor = Color.FromArgb(229, 208, 181);
            txt_Risorsa5.BackColor = Color.FromArgb(229, 208, 181);
            txt_Risorsa6.BackColor = Color.FromArgb(229, 208, 181);

            txt_Diamond_1.BackColor = Color.FromArgb(229, 208, 181);
            txt_Diamond_2.BackColor = Color.FromArgb(229, 208, 181);
            txt_Username.BackColor = Color.FromArgb(229, 208, 181);
            txt_Livello.BackColor = Color.FromArgb(229, 208, 181);
            txt_Esperienza.BackColor = Color.FromArgb(229, 208, 181);

            txt_Terreno_1.BackColor = Color.FromArgb(229, 208, 181);
            txt_Terreno_2.BackColor = Color.FromArgb(229, 208, 181);
            txt_Terreno_3.BackColor = Color.FromArgb(229, 208, 181);
            txt_Terreno_4.BackColor = Color.FromArgb(229, 208, 181);
            txt_Terreno_5.BackColor = Color.FromArgb(229, 208, 181);
        }
        private void Esercito_1()
        {
            //Bottoni livelli esercito
            btn_I.BackgroundImage = Properties.Resources.Texture_Wood_1;
            btn_II.BackgroundImage = Properties.Resources.Texture_Wood_1;
            btn_III.BackgroundImage = Properties.Resources.Texture_Wood_1;
            btn_IV.BackgroundImage = Properties.Resources.Texture_Wood_1;
            btn_V.BackgroundImage = Properties.Resources.Texture_Wood_1;

            btn_I.BackColor = Color.FromArgb(229, 208, 181);
            btn_II.BackColor = Color.FromArgb(229, 208, 181);
            btn_III.BackColor = Color.FromArgb(229, 208, 181);
            btn_IV.BackColor = Color.FromArgb(229, 208, 181);
            btn_V.BackColor = Color.FromArgb(229, 208, 181);

            btn_I.Enabled = false;
            btn_II.Enabled = true;
            btn_III.Enabled = true;
            btn_IV.Enabled = true;
            btn_V.Enabled = true;

            btn_I.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            btn_II.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            btn_III.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            btn_IV.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            btn_V.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);

        }

        private void btn_Cambia_Risorse_Click(object sender, EventArgs e)
        {
            if (txt_Tipi_Risorse.Text == "Militare")
            {
                txt_Tipi_Risorse.Text = "Civile";
                tipo_Risorse = "Civile";
                ico_1.BackgroundImage = Properties.Resources.wheat_sack;
                ico_2.BackgroundImage = Properties.Resources.wood_log_6;
                ico_3.BackgroundImage = Properties.Resources.Stone_2;
                ico_4.BackgroundImage = Properties.Resources.Ingot__Iron_icon_icon;
                ico_5.BackgroundImage = Properties.Resources.Gold_Ingot_icon_icon;
                ico_6.BackgroundImage = Properties.Resources.manpower_2;
                return;
            }

            txt_Tipi_Risorse.Text = "Militare";
            tipo_Risorse = "Militare";
            ico_1.BackgroundImage = Properties.Resources.Sword_1;
            ico_2.BackgroundImage = Properties.Resources.spears;
            ico_3.BackgroundImage = Properties.Resources.icons8_tiro_con_l_arco_48_1_;
            ico_4.BackgroundImage = Properties.Resources.icons8_scudo_48_2_;
            ico_5.BackgroundImage = Properties.Resources.icons8_armor_48_1_;
            ico_6.BackgroundImage = Properties.Resources.icons8_freccia_di_arcieri_48;

        }
        private void btn_Civile_Militare_Click(object sender, EventArgs e)
        {
            if (groupBox_Strutture.Text == "Strutture Civili")
            {
                groupBox_Strutture.Text = "Strutture Militari";
                strutture = "Militare";
                ico_Structure_1.BackgroundImage = Properties.Resources.Workshop_Spade;
                ico_Structure_2.BackgroundImage = Properties.Resources.Workshop_Lance;
                ico_Structure_3.BackgroundImage = Properties.Resources.Workshop_Archi;
                ico_Structure_4.BackgroundImage = Properties.Resources.Workshop_Scudi;
                ico_Structure_5.BackgroundImage = Properties.Resources.Workshop_Armature;
                ico_Structure_6.BackgroundImage = Properties.Resources.Workshop_Frecce;
                return;
            }

            groupBox_Strutture.Text = "Strutture Civili";
            strutture = "Civile";
            ico_Structure_1.BackgroundImage = Properties.Resources.wheat;
            ico_Structure_2.BackgroundImage = Properties.Resources.wood_cutting;
            ico_Structure_3.BackgroundImage = Properties.Resources.icons8_carrello_da_miniera_48_3_;
            ico_Structure_4.BackgroundImage = Properties.Resources.icons8_carrello_da_miniera_48_2_;
            ico_Structure_5.BackgroundImage = Properties.Resources.icons8_carrello_da_miniera_48_1_;
            ico_Structure_6.BackgroundImage = Properties.Resources.medieval_house_1_;
        }
        private void btn_Info_Terreni_Virtuali_Click(object sender, EventArgs e)
        {
            Terreni_Virtuali form_Gioco = new Terreni_Virtuali();
            form_Gioco.ShowDialog();
        }
        private void btn_Citta_Click(object sender, EventArgs e)
        {
            Citta_V2 form_Gioco = new Citta_V2();
            form_Gioco.Show();
        }
        private void Btn_Costruzione_Click(object sender, EventArgs e)
        {
            Costruzione form_Gioco = new Costruzione();
            form_Gioco.ShowDialog();

            logBox.AddLine(new[]
            {
            ("[Sistema] ", Color.Cyan),
            ("Aggironamento in corso... Attendere!", Color.White)
        });
        }

        private void btn_Shop_Click(object sender, EventArgs e)
        {
            Shop form_Gioco = new Shop();
            form_Gioco.ShowDialog();
        }

        private void btn_Ricerca_Click(object sender, EventArgs e)
        {
            Ricerca_1 form_Gioco = new Ricerca_1();
            form_Gioco.ShowDialog();
        }

        private void btn_Quest_Mensile_Click(object sender, EventArgs e)
        {
            MontlyQuest form_Gioco = new MontlyQuest();
            form_Gioco.Show();

        }

        private void btn_I_Click(object sender, EventArgs e)
        {
            btn_I.Enabled = false;
            btn_II.Enabled = true;
            btn_III.Enabled = true;
            btn_IV.Enabled = true;
            btn_V.Enabled = true;
            livello_Esercito = 1;
            this.ActiveControl = Btn_Costruzione; // assegna il focus al bottone
        }

        private void btn_II_Click(object sender, EventArgs e)
        {
            btn_I.Enabled = true;
            btn_II.Enabled = false;
            btn_III.Enabled = true;
            btn_IV.Enabled = true;
            btn_V.Enabled = true;
            livello_Esercito = 2;
            this.ActiveControl = Btn_Costruzione; // assegna il focus al bottone
        }

        private void btn_III_Click(object sender, EventArgs e)
        {
            btn_I.Enabled = true;
            btn_II.Enabled = true;
            btn_III.Enabled = false;
            btn_IV.Enabled = true;
            btn_V.Enabled = true;
            livello_Esercito = 3;
            this.ActiveControl = Btn_Costruzione; // assegna il focus al bottone
        }

        private void btn_IV_Click(object sender, EventArgs e)
        {
            btn_I.Enabled = true;
            btn_II.Enabled = true;
            btn_III.Enabled = true;
            btn_IV.Enabled = false;
            btn_V.Enabled = true;
            livello_Esercito = 4;
            this.ActiveControl = Btn_Costruzione; // assegna il focus al bottone
        }

        private void btn_V_Click(object sender, EventArgs e)
        {
            btn_I.Enabled = true;
            btn_II.Enabled = true;
            btn_III.Enabled = true;
            btn_IV.Enabled = true;
            btn_V.Enabled = false;
            livello_Esercito = 5;
            this.ActiveControl = Btn_Costruzione; // assegna il focus al bottone
        }

        private void pictureBox_Speed_Costruzione_Click(object sender, EventArgs e)
        {
            Velocizza_Diamanti form_Gioco = new Velocizza_Diamanti();
            Velocizza_Diamanti.tipo = "Costruzione";
            form_Gioco.ShowDialog();
        }

        private void pictureBox_Speed_Reclutamento_Click(object sender, EventArgs e)
        {
            Velocizza_Diamanti form_Gioco = new Velocizza_Diamanti();
            Velocizza_Diamanti.tipo = "Reclutamento";
            form_Gioco.ShowDialog();
        }

        private void btn_Esercito_Caserme_Click(object sender, EventArgs e)
        {
            if (Caserme == "Esercito")
            {
                lbl_Esercito.Text = "Caserme";
                panel_Sfondo_Bottoni.Visible = false;
                lbl_Coda_Reclutamento.Visible = false;
                lbl_Timer_Addestramento.Visible = false;

                ico_Unit_1.BackgroundImage = Properties.Resources.cross;
                ico_Unit_2.BackgroundImage = Properties.Resources.cross;
                ico_Unit_3.BackgroundImage = Properties.Resources.cross;
                ico_Unit_4.BackgroundImage = Properties.Resources.cross;

                Caserme = "Caserme";
            }
            else
            {
                lbl_Esercito.Text = "Esercito";
                panel_Sfondo_Bottoni.Visible = true;
                lbl_Coda_Reclutamento.Visible = true;
                lbl_Timer_Addestramento.Visible = true;
                ico_Unit_1.BackgroundImage = Properties.Resources.Un_guerriero_medievale_1_;
                ico_Unit_2.BackgroundImage = Properties.Resources.picchiere;
                ico_Unit_3.BackgroundImage = Properties.Resources.Un_arciere_medievale_a_figura_intera__stile_videogame_fantasy__con_arco_teso_e_faretra__abbigliamento_da_cacciatore__sfondo_trasparente__icona_PNG;
                ico_Unit_4.BackgroundImage = Properties.Resources.icons8_medieval_48;

                Caserme = "Esercito";
            }

        }

        private void btn_Acquista_Terreni_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Costruzione_Terreni|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|");
        }

        private void PVP_PVE_Click(object sender, EventArgs e)
        {
            AttaccoCoordinato form_Gioco = new AttaccoCoordinato();
            form_Gioco.ShowDialog();
        }

        private void btn_Scambia_Click(object sender, EventArgs e)
        {
            Scambia_Diamanti form_Gioco = new Scambia_Diamanti();
            form_Gioco.ShowDialog();
        }
    }
}
