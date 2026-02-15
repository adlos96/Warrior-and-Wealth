using CriptoGame_Online.Strumenti;
using Strategico_V2;

namespace CriptoGame_Online.GUI
{
    public partial class Tutorial : Form
    {
        public static GameTextBox? logBox;
        private CancellationTokenSource cts = new CancellationTokenSource();

        static bool quest_Attiva = false;
        public static bool quest_Completata = false;
        static int quest_Id = 0;
        public Tutorial()
        {
            InitializeComponent();
        }

        private void Tutorial_Load(object sender, EventArgs e)
        {
            this.ActiveControl = panel_Log;
            this.ControlBox = false; //Rimuove i pulsanti in alto a sinistra del form
            this.MaximizeBox = false;
            logBox = new GameTextBox()
            {
                Dock = DockStyle.Fill
            };

            logBox.MouseDown += LogBox_MouseDown; //Creare l'evento MouseDown per il logBox
            panel_Log.Controls.Add(logBox);// texbox personallizata

            Task.Run(() => Gui_Update(cts.Token), cts.Token);
        }
        async void Gui_Update(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (panel2.IsHandleCreated && !panel2.IsDisposed)
                {
                    panel2.BeginInvoke((Action)(() =>
                    {
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

                            this.BeginInvoke((Action)(() =>
                            {
                                this.Close();
                            }) );
                        }
                    }));
                }
                await Task.Delay(250); // meglio di Thread.Sleep
            }
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
                if (Variabili_Client.tutorial_dati[0].Obiettivo == "Premere click sinistro sul testo") quest_Completata = true;
                if (quest_Completata == true 
                    && quest_Id != 8 && quest_Id != 10 && quest_Id != 11 && quest_Id != 12 && quest_Id != 13 && quest_Id != 14 && quest_Id != 15 && quest_Id != 16 && quest_Id != 17
                    && quest_Id != 18 && quest_Id != 19 && quest_Id != 21 && quest_Id != 22 && quest_Id != 23 && quest_Id != 24 && quest_Id != 25 && quest_Id != 26 && quest_Id != 27
                    && quest_Id != 28 && quest_Id != 29 && quest_Id != 30 && quest_Id != 31) //Sono tutte le quest che si completano in altro modo purtroppo....
                    ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{quest_Id}");

                await Login.Sleep(1);
                quest_Attiva = false;

            }
        }
    }
}
