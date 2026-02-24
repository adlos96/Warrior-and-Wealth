using Warrior_and_Wealth.Strumenti;
using Strategico_V2;

namespace Warrior_and_Wealth.GUI
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

            logBox.MouseClick += LogBox_MouseDown; //Creare l'evento MouseDown per il logBox
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
                        if (Variabili_Client.tutorial[31])
                        {
                            cts.Cancel();
                            this.BeginInvoke((Action)(() => this.Close()));
                            return;
                        }

                        if (quest_Attiva) return; // già una quest in corso, non fare nulla

                        // Trova il primo tutorial non completato
                        var prossima = Variabili_Client.tutorial_dati
                            .OrderBy(i => i.StatoTutorial)
                            .FirstOrDefault(i =>
                            {
                                int idx = i.StatoTutorial;

                                // Controlla che sia ancora da fare
                                if (idx <= 0 || idx > Variabili_Client.tutorial.Count() - 1) return false;
                                if (Variabili_Client.tutorial[idx - 1] == true) return false; // già completata

                                // Controlla che tutti i precedenti siano completati
                                // idx == 1 → nessun prerequisito, idx >= 2 → il precedente deve essere true
                                if (idx >= 2 && Variabili_Client.tutorial[idx - 2] == false) return false;

                                return true;
                            });

                        if (prossima == null) return;

                        logBox.Clear();
                        Log_Update("Tutorial: " + prossima.StatoTutorial.ToString());
                        Log_Update("Obiettivo: " + prossima.Obiettivo);
                        Log_Update("");
                        Log_Update(prossima.Descrizione);
                        GameAudio.PlayMenuMusic($"Tutorial - {prossima.StatoTutorial}");
                        MusicManager.SetVolume(1.0f);
                        quest_Attiva = true;
                        quest_Id = prossima.StatoTutorial;
                    }));
                }

                await Task.Delay(1000);
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
                if (Variabili_Client.tutorial[quest_Id - 1] == false) quest_Completata = true;
                if (quest_Completata == true
                    && quest_Id != 8 && quest_Id != 10 && quest_Id != 11 && quest_Id != 12 && quest_Id != 13 && quest_Id != 14 && quest_Id != 15 && quest_Id != 16 && quest_Id != 17
                    && quest_Id != 18 && quest_Id != 19 && quest_Id != 21 && quest_Id != 22 && quest_Id != 23 && quest_Id != 24 && quest_Id != 25 && quest_Id != 26 && quest_Id != 27
                    && quest_Id != 28 && quest_Id != 29 && quest_Id != 30 && quest_Id != 31) //Sono tutte le quest che si completano in altro modo purtroppo....
                    ClientConnection.TestClient.Send($"Tutorial Update|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{quest_Id}");

                quest_Attiva = false;
                await Login.Sleep(1);
            }
        }
    }
}
