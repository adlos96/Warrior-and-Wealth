
using NAudio.Wave;
using Strategico_V2;

namespace Warrior_and_Wealth.GUI
{
    public partial class Spostamento_Truppe : Form
    {
        public static string struttura = "";
        public static bool inverso = false;
        static int livello_Unità = 1;
        private CancellationTokenSource cts = new CancellationTokenSource();
        public Spostamento_Truppe()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            lbl_Struttura.Text = struttura;
            btn_I_Struttura.Enabled = true;
            btn_II_Struttura.Enabled = false;
            btn_III_Struttura.Enabled = false;
            btn_IV_Struttura.Enabled = false;
            btn_V_Struttura.Enabled = false;
        }

        private void Spostamento_Truppe_Load(object sender, EventArgs e)
        {
            Task.Run(() => Gui_Update(cts.Token), cts.Token);
        }

        async void Gui_Update(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (panel1.IsHandleCreated && !panel1.IsDisposed)
                {
                    panel1.BeginInvoke((Action)(() =>
                    {
                        int g_Esercito = 0, g_Citta = 0;
                        int l_Esercito = 0, l_Citta = 0;
                        int a_Esercito = 0, a_Citta = 0;
                        int c_Esercito = 0, c_Citta = 0;
                        int maxGuarnigione = 0, scelta = 0, presenti_Esercito = 0, presenti_Citta;

                        if (struttura == "Ingresso")
                        {
                            if (livello_Unità == 1)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Guerrieri_1);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Lanceri_1);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Arceri_1);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Catapulte_1);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Guerrieri_1);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Lanceri_1);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Arceri_1);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Catapulte_1);
                                }
                            }
                            if (livello_Unità == 2)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Guerrieri_2);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Lanceri_2);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Arceri_2);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Catapulte_2);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);

                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Guerrieri_2);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Lanceri_2);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Arceri_2);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Catapulte_2);
                                }
                            }
                            if (livello_Unità == 3)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Guerrieri_3);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Lanceri_3);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Arceri_3);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Catapulte_3);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Guerrieri_3);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Lanceri_3);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Arceri_3);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Catapulte_3);
                                }
                            }
                            if (livello_Unità == 4)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Guerrieri_4);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Lanceri_4);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Arceri_4);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Catapulte_5);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Guerrieri_4);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Lanceri_4);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Arceri_4);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Catapulte_5);
                                }
                            }
                            if (livello_Unità == 5)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Guerrieri_5);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Lanceri_5);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Arceri_5);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Catapulte_5);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Guerrieri_5);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Lanceri_5);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Arceri_5);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Ingresso.Catapulte_5);
                                }
                            }
                            maxGuarnigione = Variabili_Client.Citta.Ingresso.Guarnigione_Max;
                        }
                        if (struttura == "Citta")
                        {
                            lbl_Truppe_Massime.Text = $"Max truppe: {Variabili_Client.Citta.Città.Guarnigione_Max}";
                            if (livello_Unità == 1)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Guerrieri_1);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Lanceri_1);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Arceri_1);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Catapulte_1);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Guerrieri_1);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Lanceri_1);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Arceri_1);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Catapulte_1);
                                }
                            }
                            if (livello_Unità == 2)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Guerrieri_2);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Lanceri_2);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Arceri_2);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Catapulte_2);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);

                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Guerrieri_2);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Lanceri_2);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Arceri_2);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Catapulte_2);
                                }
                            }
                            if (livello_Unità == 3)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Guerrieri_3);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Lanceri_3);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Arceri_3);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Catapulte_3);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Guerrieri_3);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Lanceri_3);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Arceri_3);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Catapulte_3);
                                }
                            }
                            if (livello_Unità == 4)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Guerrieri_4);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Lanceri_4);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Arceri_4);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Catapulte_5);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Guerrieri_4);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Lanceri_4);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Arceri_4);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Catapulte_5);
                                }
                            }
                            if (livello_Unità == 5)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Guerrieri_5);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Lanceri_5);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Arceri_5);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Città.Catapulte_5);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Guerrieri_5);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Lanceri_5);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Arceri_5);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Città.Catapulte_5);
                                }
                            }
                            maxGuarnigione = Variabili_Client.Citta.Città.Guarnigione_Max;
                        }
                        if (struttura == "Cancello")
                        {
                            if (livello_Unità == 1)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Guerrieri_1);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Lanceri_1);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Arceri_1);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Catapulte_1);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Guerrieri_1);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Lanceri_1);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Arceri_1);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Catapulte_1);
                                }
                            }
                            if (livello_Unità == 2)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Guerrieri_2);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Lanceri_2);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Arceri_2);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Catapulte_2);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);

                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Guerrieri_2);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Lanceri_2);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Arceri_2);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Catapulte_2);
                                }
                            }
                            if (livello_Unità == 3)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Guerrieri_3);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Lanceri_3);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Arceri_3);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Catapulte_3);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Guerrieri_3);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Lanceri_3);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Arceri_3);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Catapulte_3);
                                }
                            }
                            if (livello_Unità == 4)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Guerrieri_4);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Lanceri_4);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Arceri_4);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Catapulte_5);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Guerrieri_4);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Lanceri_4);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Arceri_4);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Catapulte_5);
                                }
                            }
                            if (livello_Unità == 5)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Guerrieri_5);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Lanceri_5);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Arceri_5);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Cancello.Catapulte_5);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Guerrieri_5);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Lanceri_5);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Arceri_5);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Cancello.Catapulte_5);
                                }
                            }
                            maxGuarnigione = Variabili_Client.Citta.Cancello.Guarnigione_Max;
                        }
                        if (struttura == "Mura")
                        {
                            if (livello_Unità == 1)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Guerrieri_1);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Lanceri_1);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Arceri_1);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Catapulte_1);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Guerrieri_1);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Lanceri_1);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Arceri_1);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Catapulte_1);
                                }
                            }
                            if (livello_Unità == 2)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Guerrieri_2);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Lanceri_2);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Arceri_2);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Catapulte_2);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);

                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Guerrieri_2);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Lanceri_2);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Arceri_2);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Catapulte_2);
                                }
                            }
                            if (livello_Unità == 3)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Guerrieri_3);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Lanceri_3);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Arceri_3);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Catapulte_3);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Guerrieri_3);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Lanceri_3);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Arceri_3);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Catapulte_3);
                                }
                            }
                            if (livello_Unità == 4)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Guerrieri_4);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Lanceri_4);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Arceri_4);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Catapulte_5);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Guerrieri_4);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Lanceri_4);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Arceri_4);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Catapulte_5);
                                }
                            }
                            if (livello_Unità == 5)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Guerrieri_5);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Lanceri_5);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Arceri_5);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Mura.Catapulte_5);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Guerrieri_5);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Lanceri_5);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Arceri_5);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Mura.Catapulte_5);
                                }
                            }
                            maxGuarnigione = Variabili_Client.Citta.Mura.Guarnigione_Max;
                        }
                        if (struttura == "Torri")
                        {
                            if (livello_Unità == 1)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Guerrieri_1);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Lanceri_1);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Arceri_1);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Catapulte_1);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Guerrieri_1);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Lanceri_1);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Arceri_1);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Catapulte_1);
                                }
                            }
                            if (livello_Unità == 2)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Guerrieri_2);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Lanceri_2);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Arceri_2);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Catapulte_2);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);

                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Guerrieri_2);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Lanceri_2);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Arceri_2);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Catapulte_2);
                                }
                            }
                            if (livello_Unità == 3)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Guerrieri_3);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Lanceri_3);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Arceri_3);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Catapulte_3);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Guerrieri_3);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Lanceri_3);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Arceri_3);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Catapulte_3);
                                }
                            }
                            if (livello_Unità == 4)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Guerrieri_4);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Lanceri_4);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Arceri_4);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Catapulte_5);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Guerrieri_4);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Lanceri_4);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Arceri_4);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Catapulte_5);
                                }
                            }
                            if (livello_Unità == 5)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Guerrieri_5);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Lanceri_5);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Arceri_5);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Torri.Catapulte_5);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Guerrieri_5);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Lanceri_5);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Arceri_5);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Torri.Catapulte_5);
                                }
                            }
                            maxGuarnigione = Variabili_Client.Citta.Torri.Guarnigione_Max;
                        }
                        if (struttura == "Castello")
                        {
                            if (livello_Unità == 1)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Guerrieri_1);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Lanceri_1);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Arceri_1);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Catapulte_1);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_1.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_1.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_1.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_1.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Guerrieri_1);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Lanceri_1);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Arceri_1);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Catapulte_1);
                                }
                            }
                            if (livello_Unità == 2)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Guerrieri_2);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Lanceri_2);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Arceri_2);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Catapulte_2);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);

                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_2.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_2.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_2.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_2.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Guerrieri_2);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Lanceri_2);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Arceri_2);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Catapulte_2);
                                }
                            }
                            if (livello_Unità == 3)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Guerrieri_3);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Lanceri_3);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Arceri_3);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Catapulte_3);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_3.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_3.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_3.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_3.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Guerrieri_3);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Lanceri_3);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Arceri_3);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Catapulte_3);
                                }
                            }
                            if (livello_Unità == 4)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Guerrieri_4);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Lanceri_4);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Arceri_4);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Catapulte_5);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_4.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_4.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_4.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_4.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Guerrieri_4);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Lanceri_4);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Arceri_4);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Catapulte_5);
                                }
                            }
                            if (livello_Unità == 5)
                            {
                                g_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Guerrieri_5);
                                l_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Lanceri_5);
                                a_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Arceri_5);
                                c_Citta = Convert.ToInt32(Variabili_Client.Citta.Castello.Catapulte_5);

                                g_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                                l_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                                a_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                                c_Esercito = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);
                                if (inverso)
                                {
                                    g_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Guerrieri_5.Quantità);
                                    l_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Lanceri_5.Quantità);
                                    a_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Arceri_5.Quantità);
                                    c_Citta = Convert.ToInt32(Variabili_Client.Reclutamento.Catapulte_5.Quantità);

                                    g_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Guerrieri_5);
                                    l_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Lanceri_5);
                                    a_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Arceri_5);
                                    c_Esercito = Convert.ToInt32(Variabili_Client.Citta.Castello.Catapulte_5);
                                }
                            }
                            maxGuarnigione = Variabili_Client.Citta.Castello.Guarnigione_Max;
                        }

                        txt_Guerriero_Esercito.Text = g_Esercito.ToString();
                        txt_Lanciere_Esercito.Text = l_Esercito.ToString();
                        txt_Arciere_Esercito.Text = a_Esercito.ToString();
                        txt_Catapulta_Esercito.Text = c_Esercito.ToString();

                        txt_Guerriero_Struttura.Text = g_Citta.ToString();
                        txt_Lanciere_Struttura.Text = l_Citta.ToString();
                        txt_Arciere_Struttura.Text = a_Citta.ToString();
                        txt_Catapulta_Struttura.Text = c_Citta.ToString();

                        scelta = trackBar_Guerriero.Value + trackBar_Lanciere.Value + trackBar_Arciere.Value + trackBar_Catapulta.Value;
                        presenti_Esercito = g_Citta + l_Citta + a_Citta + c_Citta;
                        presenti_Citta = g_Esercito + l_Esercito + a_Esercito + c_Esercito;
                        int spazioRimasto = 0;

                        if (inverso)
                        {
                            spazioRimasto = presenti_Citta;
                            lbl_Truppe_Massime.Text = $"Max truppe: {presenti_Citta}";
                        }
                        else
                        {
                            spazioRimasto = maxGuarnigione - scelta - presenti_Esercito;
                            lbl_Truppe_Massime.Text = $"Max truppe: {maxGuarnigione - scelta - presenti_Esercito}";
                        }

                        trackBar_Guerriero.Maximum = Math.Min(g_Esercito, trackBar_Guerriero.Value + spazioRimasto);
                        if (trackBar_Guerriero.Maximum <= trackBar_Guerriero.Value)
                            trackBar_Guerriero.Value = trackBar_Guerriero.Maximum;

                        trackBar_Lanciere.Maximum = Math.Min(l_Esercito, trackBar_Lanciere.Value + spazioRimasto);
                        if (trackBar_Lanciere.Maximum <= trackBar_Lanciere.Value)
                            trackBar_Lanciere.Value = trackBar_Lanciere.Maximum;

                        trackBar_Arciere.Maximum = Math.Min(a_Esercito, trackBar_Arciere.Value + spazioRimasto);
                        if (trackBar_Arciere.Maximum <= trackBar_Arciere.Value)
                            trackBar_Arciere.Value = trackBar_Arciere.Maximum;

                        trackBar_Catapulta.Maximum = Math.Min(c_Esercito, trackBar_Catapulta.Value + spazioRimasto);
                        if (trackBar_Catapulta.Maximum <= trackBar_Catapulta.Value)
                            trackBar_Catapulta.Value = trackBar_Catapulta.Maximum;

                        if (maxGuarnigione - scelta >= 0) lbl_Guerriero.Text = trackBar_Guerriero.Value.ToString();
                        if (maxGuarnigione - scelta >= 0) lbl_Lanciere.Text = trackBar_Lanciere.Value.ToString();
                        if (maxGuarnigione - scelta >= 0) lbl_Arciere.Text = trackBar_Arciere.Value.ToString();
                        if (maxGuarnigione - scelta >= 0) lbl_Catapulta.Text = trackBar_Catapulta.Value.ToString();


                    }));
                }
                await Task.Delay(250);
            }

        }

        private void btn_Spostamento_Truppe_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"SpostamentoTruppe|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{lbl_Esercito.Text}|{lbl_Struttura.Text}|{lbl_Guerriero.Text}|{lbl_Lanciere.Text}|{lbl_Arciere.Text}|{lbl_Catapulta.Text}|{livello_Unità}");
            trackBar_Guerriero.Value = 0;
            trackBar_Lanciere.Value = 0;
            trackBar_Arciere.Value = 0;
            trackBar_Catapulta.Value = 0;

        }
        private void btn_Scambia_Click(object sender, EventArgs e)
        {
            if (inverso == true)
            {
                lbl_Esercito.Text = "Esercito Villaggio";
                lbl_Struttura.Text = struttura;
                inverso = !inverso;
            }
            else
            {
                lbl_Esercito.Text = struttura;
                lbl_Struttura.Text = "Esercito Villaggio";
                inverso = true;
            }
        }
        private void AdjustTrackBars(TrackBar currentTrackBar)
        {
            // Calcola il totale attuale
            int totale = trackBar_Guerriero.Value + trackBar_Lanciere.Value + trackBar_Arciere.Value + trackBar_Catapulta.Value;
            int maxGuarnigione = 0; // Ottieni il max della guarnigione

            if (struttura == "Ingresso") maxGuarnigione = Variabili_Client.Citta.Ingresso.Guarnigione_Max;
            else if (struttura == "Citta") maxGuarnigione = Variabili_Client.Citta.Città.Guarnigione_Max;
            else if (struttura == "Cancello") maxGuarnigione = Variabili_Client.Citta.Cancello.Guarnigione_Max;
            else if (struttura == "Mura") maxGuarnigione = Variabili_Client.Citta.Mura.Guarnigione_Max;
            else if (struttura == "Torri") maxGuarnigione = Variabili_Client.Citta.Torri.Guarnigione_Max;
            else if (struttura == "Castello") maxGuarnigione = Variabili_Client.Citta.Castello.Guarnigione_Max;

            if (totale > maxGuarnigione) // Se supera il limite, riduci il valore della trackbar corrente
            {
                int eccesso = totale - maxGuarnigione;
                currentTrackBar.Value = Math.Max(0, currentTrackBar.Value - eccesso);
            }
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

            trackBar_Guerriero.Value = 0;
            trackBar_Lanciere.Value = 0;
            trackBar_Arciere.Value = 0;
            trackBar_Catapulta.Value = 0;

            this.ActiveControl = btn_I_Esercito;
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

            trackBar_Guerriero.Value = 0;
            trackBar_Lanciere.Value = 0;
            trackBar_Arciere.Value = 0;
            trackBar_Catapulta.Value = 0;

            this.ActiveControl = btn_II_Esercito;
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

            trackBar_Guerriero.Value = 0;
            trackBar_Lanciere.Value = 0;
            trackBar_Arciere.Value = 0;
            trackBar_Catapulta.Value = 0;

            this.ActiveControl = btn_III_Esercito;
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

            trackBar_Guerriero.Value = 0;
            trackBar_Lanciere.Value = 0;
            trackBar_Arciere.Value = 0;
            trackBar_Catapulta.Value = 0;

            this.ActiveControl = btn_IV_Esercito;
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

            trackBar_Guerriero.Value = 0;
            trackBar_Lanciere.Value = 0;
            trackBar_Arciere.Value = 0;
            trackBar_Catapulta.Value = 0;

            this.ActiveControl = btn_V_Esercito; // assegna il focus al bottone
        }

        private void trackBar_Guerriero_Scroll(object sender, EventArgs e)
        {
            AdjustTrackBars(trackBar_Guerriero);
        }
        private void trackBar_Lanciere_Scroll(object sender, EventArgs e)
        {
            AdjustTrackBars(trackBar_Lanciere);
        }
        private void trackBar_Arciere_Scroll(object sender, EventArgs e)
        {
            AdjustTrackBars(trackBar_Arciere);
        }
        private void trackBar_Catapulta_Scroll(object sender, EventArgs e)
        {
            AdjustTrackBars(trackBar_Catapulta);
        }

        private void Spostamento_Truppe_FormClosing(object sender, FormClosingEventArgs e)
        {
            livello_Unità = 1;
            inverso = false;
        }
    }
}
