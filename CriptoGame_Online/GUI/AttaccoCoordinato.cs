using CriptoGame_Online.Strumenti;
using Strategico_V2;
using System;


namespace CriptoGame_Online.GUI
{
    public partial class AttaccoCoordinato : Form
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        public static string tipo_Attacco = "";
        public static string tipo_Barbaro = "Villaggi Barbari";
        static int livello_Esercito = 1;

        static bool attacco_Premuto = false;

        int[] guerrieri_Temp = new int[] { 0, 0, 0, 0, 0 };
        int[] picchieri_Temp = new int[] { 0, 0, 0, 0, 0 };
        int[] arcieri_Temp = new int[] { 0, 0, 0, 0, 0 };
        int[] catapulte_Temp = new int[] { 0, 0, 0, 0, 0 };
        public AttaccoCoordinato()
        {
            InitializeComponent();
            this.Size = new Size(794, 293);
            groupBox_Raduno.Visible = false;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            groupBox4.Text = tipo_Barbaro;

            btn_I_Spedizione.Enabled = true;
            btn_II_Spedizione.Enabled = false;
            btn_III_Spedizione.Enabled = false;
            btn_IV_Spedizione.Enabled = false;
            btn_V_Spedizione.Enabled = false;
        }
        private async void AttaccoCoordinato_Load(object sender, EventArgs e)
        {
            await Load_Guid();
            UpdateCombobox();

            this.ActiveControl = btn_Crea; // assegna il focus al bottone
            Task.Run(() => Gui_Update(cts.Token), cts.Token);
        }
        void UpdateCombobox()
        {
            if (tipo_Barbaro == "Villaggi Barbari")
            {
                comboBox_Villaggi.Items.Clear();
                foreach (var item in Variabili_Client.VillaggiPersonali)
                {
                    int risorse = item.Cibo + item.Legno + item.Pietra + item.Ferro + item.Oro + item.Diamanti_Blu + item.Diamanti_Viola;
                    if (risorse > 0)
                        comboBox_Villaggi.Items.Add("Villaggio Barbaro - Livello: " + item.Livello);
                }
                txt_Villaggio_B_Desc.Text = "Esplora il barbaro per avere una stima delle sue truppe";
            }
            if (tipo_Barbaro == "Città Barbare")
            {
                comboBox_Villaggi.Items.Clear();
                foreach (var item in Variabili_Client.CittaGlobali)
                {
                    int risorse = item.Cibo + item.Legno + item.Pietra + item.Ferro + item.Oro + item.Diamanti_Blu + item.Diamanti_Viola;
                    if (risorse > 0)
                        comboBox_Villaggi.Items.Add("Citta Barbare - Livello: " + item.Livello);
                }
                txt_Villaggio_B_Desc.Text = "Raggiungi il livello 5 per sbloccare le città. \n\rEsplora il barbaro per avere una stima delle sue truppe";
            }
        }
        void Raduni_Guid()
        {
            comboBox_Raduni_Creati.Items.Clear();
            comboBox_Raduni_InCorso.Items.Clear();
            if (Variabili_Client.Raduni_Creati.Count != 0)
                foreach (string attaccoStr in Variabili_Client.Raduni_Creati)
                {
                    var attacco = Variabili_Client.AttaccoInfo.FromString(attaccoStr);
                    if (attacco != null)
                    {
                        comboBox_Raduni_Creati.Items.Add($"{attacco.Creatore} - {attacco.ID} - {attacco.TempoRimanente}");
                    }
                }
            if (comboBox_Raduni_Creati.Items.Count > 0) comboBox_Raduni_Creati.Text = comboBox_Raduni_Creati.Items[0].ToString();

            if (Variabili_Client.Raduni_InCorso.Count != 0)
                foreach (string attaccoStr in Variabili_Client.Raduni_InCorso)
                {
                    var attacco = Variabili_Client.PartecipanteAttacco.FromString(attaccoStr);
                    if (attacco != null)
                    {
                        comboBox_Raduni_InCorso.Items.Add($"{attacco.Giocatore} - {attacco.ID} - {attacco.TempoRimanente}");
                    }
                }
            if (comboBox_Raduni_InCorso.Items.Count > 0)
                comboBox_Raduni_InCorso.Text = comboBox_Raduni_InCorso.Items[0].ToString();

            if (comboBox_Raduni_Creati.Items.Count == 0)
            {
                comboBox_Raduni_InCorso.Enabled = false;
                comboBox_Raduni_Creati.Enabled = false;
            }
            else
            {
                comboBox_Raduni_InCorso.Enabled = true;
                comboBox_Raduni_Creati.Enabled = true;
            }

            if (comboBox_Raduni_Creati.Text == "") //Disabilita i pulsanti se non ci sono raduni selezionati o presenti
            {
                btn_Inizia.Enabled = false;
                btn_Partecipa.Enabled = false;
            }
            else
            {
                btn_Inizia.Enabled = true;
                btn_Partecipa.Enabled = true;
            }

            if (comboBox_Raduni_InCorso.Text == "") //Disabilita i pulsanti se non ci sono raduni in corso selezionati o presenti
                btn_Abbandona.Enabled = false;
            else
                btn_Abbandona.Enabled = true;
        }
        private async Task Load_Guid()
        {
            Raduni_Guid();
            if (livello_Esercito == 1)
            {
                txt_Guerriero_Spedizione.Text = guerrieri_Temp[0].ToString();
                txt_Lancere_Spedizione.Text = picchieri_Temp[0].ToString();
                txt_Arcere_Spedizione.Text = arcieri_Temp[0].ToString();
                txt_Catapulta_Spedizione.Text = catapulte_Temp[0].ToString();

                txt_Guerriero_Esercito.Text = Variabili_Client.Reclutamento.Guerrieri_1.Quantità.ToString();
                txt_Lanciere_Esercito.Text = Variabili_Client.Reclutamento.Lanceri_1.Quantità.ToString();
                txt_Arciere_Esercito.Text = Variabili_Client.Reclutamento.Arceri_1.Quantità.ToString();
                txt_Catapulta_Esercito.Text = Variabili_Client.Reclutamento.Catapulte_1.Quantità.ToString();
            }
            if (livello_Esercito == 2)
            {
                txt_Guerriero_Spedizione.Text = guerrieri_Temp[1].ToString();
                txt_Lancere_Spedizione.Text = picchieri_Temp[1].ToString();
                txt_Arcere_Spedizione.Text = arcieri_Temp[1].ToString();
                txt_Catapulta_Spedizione.Text = catapulte_Temp[1].ToString();

                txt_Guerriero_Esercito.Text = Variabili_Client.Reclutamento.Guerrieri_2.Quantità.ToString();
                txt_Lanciere_Esercito.Text = Variabili_Client.Reclutamento.Lanceri_2.Quantità.ToString();
                txt_Arciere_Esercito.Text = Variabili_Client.Reclutamento.Arceri_2.Quantità.ToString();
                txt_Catapulta_Esercito.Text = Variabili_Client.Reclutamento.Catapulte_2.Quantità.ToString();
            }
            if (livello_Esercito == 3)
            {
                txt_Guerriero_Spedizione.Text = guerrieri_Temp[2].ToString();
                txt_Lancere_Spedizione.Text = picchieri_Temp[2].ToString();
                txt_Arcere_Spedizione.Text = arcieri_Temp[2].ToString();
                txt_Catapulta_Spedizione.Text = catapulte_Temp[2].ToString();

                txt_Guerriero_Esercito.Text = Variabili_Client.Reclutamento.Guerrieri_3.Quantità.ToString();
                txt_Lanciere_Esercito.Text = Variabili_Client.Reclutamento.Lanceri_3.Quantità.ToString();
                txt_Arciere_Esercito.Text = Variabili_Client.Reclutamento.Arceri_3.Quantità.ToString();
                txt_Catapulta_Esercito.Text = Variabili_Client.Reclutamento.Catapulte_3.Quantità.ToString();
            }
            if (livello_Esercito == 4)
            {
                txt_Guerriero_Spedizione.Text = guerrieri_Temp[3].ToString();
                txt_Lancere_Spedizione.Text = picchieri_Temp[3].ToString();
                txt_Arcere_Spedizione.Text = arcieri_Temp[3].ToString();
                txt_Catapulta_Spedizione.Text = catapulte_Temp[3].ToString();

                txt_Guerriero_Esercito.Text = Variabili_Client.Reclutamento.Guerrieri_4.Quantità.ToString();
                txt_Lanciere_Esercito.Text = Variabili_Client.Reclutamento.Lanceri_4.Quantità.ToString();
                txt_Arciere_Esercito.Text = Variabili_Client.Reclutamento.Arceri_4.Quantità.ToString();
                txt_Catapulta_Esercito.Text = Variabili_Client.Reclutamento.Catapulte_4.Quantità.ToString();
            }
            if (livello_Esercito == 5)
            {
                txt_Guerriero_Spedizione.Text = guerrieri_Temp[4].ToString();
                txt_Lancere_Spedizione.Text = picchieri_Temp[4].ToString();
                txt_Arcere_Spedizione.Text = arcieri_Temp[4].ToString();
                txt_Catapulta_Spedizione.Text = catapulte_Temp[4].ToString();

                txt_Guerriero_Esercito.Text = Variabili_Client.Reclutamento.Guerrieri_5.Quantità.ToString();
                txt_Lanciere_Esercito.Text = Variabili_Client.Reclutamento.Lanceri_5.Quantità.ToString();
                txt_Arciere_Esercito.Text = Variabili_Client.Reclutamento.Arceri_5.Quantità.ToString();
                txt_Catapulta_Esercito.Text = Variabili_Client.Reclutamento.Catapulte_5.Quantità.ToString();
            }

            string g = txt_Guerriero_Esercito.Text;
            string l = txt_Lanciere_Esercito.Text;
            string a = txt_Arciere_Esercito.Text;
            string c = txt_Catapulta_Esercito.Text;

            if (Convert.ToInt32(g) + Convert.ToInt32(l) + Convert.ToInt32(a) + Convert.ToInt32(c) >= 0)
            {
                trackBar_Guerriero.Maximum = Convert.ToInt32(g);
                trackBar_Lanciere.Maximum = Convert.ToInt32(l);
                trackBar_Arciere.Maximum = Convert.ToInt32(a);
                trackBar_Catapulta.Maximum = Convert.ToInt32(c);
            }

            var items = comboBox_PVP.Items;
            if (Variabili_Client.Giocatori_PVP.Count > 0)
                foreach (var i in Variabili_Client.Giocatori_PVP)
                    comboBox_PVP.Items.Add(i);

            lbl_Giocatori_PVP.Text = "Giocatori: " + comboBox_PVP.Items.Count;

            trackBar_Guerriero.Value = 0;
            trackBar_Lanciere.Value = 0;
            trackBar_Arciere.Value = 0;
            trackBar_Catapulta.Value = 0;

            lbl_Guerriero.Text = "0";
            lbl_Lanciere.Text = "0";
            lbl_Arciere.Text = "0";
            lbl_Catapulta.Text = "0";

            this.ActiveControl = btn_Crea; // assegna il focus al bottone
        }
        async void Gui_Update(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (groupBox1.IsHandleCreated && !groupBox1.IsDisposed)
                {
                    groupBox1.Invoke(new Action(() =>
                    {
                        int index_Villaggi_Città = 0;
                        if (comboBox_Villaggi.SelectedIndex < 0)
                            index_Villaggi_Città = 0;

                        if (tipo_Barbaro == "Villaggi Barbari")
                        {
                            if (comboBox_Villaggi.SelectedIndex != -1)
                            {
                                index_Villaggi_Città = Convert.ToInt32(comboBox_Villaggi.Text.Replace("Villaggio Barbaro - Livello: ", "")) - 1;

                                var villaggio = Variabili_Client.VillaggiPersonali[index_Villaggi_Città]; //Update truppe Villaggi Barbari
                                int T_Truppe = villaggio.Guerrieri + villaggio.Lancieri + villaggio.Arcieri + villaggio.Catapulte;
                                if (villaggio != null && T_Truppe > 0 || villaggio.Esplorato == true)
                                {
                                    txt_Guerriero_Villaggio.Text = villaggio.Guerrieri.ToString();
                                    txt_Lancere_Villaggio.Text = villaggio.Lancieri.ToString();
                                    txt_Arcere_Villaggio.Text = villaggio.Arcieri.ToString();
                                    txt_Catapulta_Villaggio.Text = villaggio.Catapulte.ToString();
                                }
                                else
                                {
                                    txt_Guerriero_Villaggio.Text = "????";
                                    txt_Lancere_Villaggio.Text = "????";
                                    txt_Arcere_Villaggio.Text = "????";
                                    txt_Catapulta_Villaggio.Text = "????";
                                }

                                txt_Villaggio_B_Desc.Text = "";
                                if (villaggio.Esplorato == true)//Descrizion
                                {
                                    txt_Villaggio_B_Desc.Text = $"Hai esplorato con successo il villaggio barbaro, Bottino;\r\n Exp: {villaggio.Esperienza} Liv: {villaggio.Livello}\r\n Diamanti Blu: {villaggio.Diamanti_Blu} Diamanti Viola: {villaggio.Diamanti_Viola}\r\n" +
                                        $"Cibo: {villaggio.Cibo} Legno: {villaggio.Legno} Pietra: {villaggio.Pietra}\r\nFerro: {villaggio.Ferro} Oro: {villaggio.Oro}";
                                    if (villaggio.Esplorato == true) btn_Attacco_PVE_Villaggio_B.Enabled = true;
                                }
                                if (villaggio.Esplorato == false) btn_Esplora_PVE_Villaggio_B.Enabled = true;
                            }
                        }
                        if (tipo_Barbaro == "Città Barbare")
                        {
                            if (comboBox_Villaggi.SelectedIndex != -1)
                            {
                                index_Villaggi_Città = Convert.ToInt32(comboBox_Villaggi.Text.Replace("Citta Barbare - Livello: ", "")) - 1;

                                var Città = Variabili_Client.CittaGlobali[index_Villaggi_Città]; //Update truppe Citta Barbaro
                                int T_Truppe = Città.Guerrieri + Città.Lancieri + Città.Arcieri + Città.Catapulte;
                                if (Città != null && T_Truppe > 0 || Città.Sconfitto == true)
                                {
                                    txt_Guerriero_Villaggio.Text = Città.Guerrieri.ToString();
                                    txt_Lancere_Villaggio.Text = Città.Lancieri.ToString();
                                    txt_Arcere_Villaggio.Text = Città.Arcieri.ToString();
                                    txt_Catapulta_Villaggio.Text = Città.Catapulte.ToString();
                                }
                                else
                                {
                                    txt_Guerriero_Villaggio.Text = "????";
                                    txt_Lancere_Villaggio.Text = "????";
                                    txt_Arcere_Villaggio.Text = "????";
                                    txt_Catapulta_Villaggio.Text = "????";
                                }

                                txt_Villaggio_B_Desc.Text = "";
                                if (Città.Esplorato == true) //Descrizione
                                    txt_Villaggio_B_Desc.Text = $"Hai esplorato con successo la città barbara, Bottino;\r\n Exp: {Città.Esperienza} Liv: {Città.Livello}\r\n Diamanti Blu: {Città.Diamanti_Blu} Diamanti Viola: {Città.Diamanti_Viola}\r\n" +
                                        $"Cibo: {Città.Cibo} Legno: {Città.Legno} Pietra: {Città.Pietra}\r\nFerro: {Città.Ferro} Oro: {Città.Oro}";
                                if (Città.Esplorato == true) btn_Attacco_PVE_Villaggio_B.Enabled = true;
                                if (Città.Esplorato == false) btn_Esplora_PVE_Villaggio_B.Enabled = true;
                                if (Città.Esplorato == true && btn_Esplora_PVE_Villaggio_B.Text == "Attacco") btn_Esplora_PVE_Villaggio_B.Enabled = true;
                            }
                        }
                        if (comboBox_PVP.SelectedIndex < 0)
                            btn_Attacco_PVP.Enabled = false;
                        else
                            btn_Attacco_PVP.Enabled = true;
                    }));
                }
                await Task.Delay(1000); // meglio di Thread.Sleep
            }

        }
        private void MostraCountdownAttacco()
        {
            TimeSpan tempo = TimeSpan.FromSeconds(60 * 3);
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;

            timer.Tick += (s, e) =>
            {
                if (tempo.TotalSeconds <= 0)
                {
                    timer.Stop();
                    timer.Dispose();
                    btn_Attacca.Text = "Attacca";
                    btn_Sposta.Text = $"Sposta";
                    if (!btn_Attacca.Enabled) btn_Attacca.Enabled = true;
                    if (!btn_Sposta.Enabled) btn_Sposta.Enabled = true;
                    return;
                }
                if (btn_Attacca.Enabled) btn_Attacca.Enabled = false;
                if (btn_Sposta.Enabled) btn_Sposta.Enabled = false;

                btn_Attacca.Text = $"Attacca {tempo:mm\\:ss}";
                btn_Sposta.Text = $"Sposta {tempo:mm\\:ss}";
                tempo = tempo.Subtract(TimeSpan.FromSeconds(1));
            };

            if (!btn_Attacca.Enabled) btn_Attacca.Enabled = true;
            if (!btn_Sposta.Enabled) btn_Sposta.Enabled = true;
            // mostra subito "Attacca 3:00"
            btn_Attacca.Text = $"Attacca {tempo:mm\\:ss}";
            btn_Sposta.Text = $"Sposta {tempo:mm\\:ss}";
            timer.Start();
        }
        private async void btn_Crea_Click(object sender, EventArgs e)
        {
            btn_Crea.Enabled = false;

            ClientConnection.TestClient.Send($"AttaccoCooperativo|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Crea|");
            await Login.Sleep(3);
            Load_Guid();
            btn_Crea.Enabled = true;
        }

        private async void btn_Partecipa_Click(object sender, EventArgs e)
        {
            btn_Partecipa.Enabled = false;
            if (comboBox_Raduni_Creati.Text != null)
            {
                var dati = comboBox_Raduni_Creati.Text.Replace(" ", "").Split('-');
                ClientConnection.TestClient.Send($"AttaccoCooperativo|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Partecipa|{dati[1]}|{lbl_Guerriero.Text}|{lbl_Lanciere.Text}|{lbl_Arciere.Text}|{lbl_Catapulta.Text}");
            }
            await Login.Sleep(2);
            Load_Guid();
            trackBar_Guerriero.Value = 0;
            trackBar_Lanciere.Value = 0;
            trackBar_Arciere.Value = 0;
            trackBar_Catapulta.Value = 0;
            lbl_Guerriero.Text = "0";
            lbl_Lanciere.Text = "0";
            lbl_Arciere.Text = "0";
            lbl_Catapulta.Text = "0";
        }

        private async void btn_Abbandona_Click(object sender, EventArgs e)
        {
            btn_Abbandona.Enabled = false;
            if (comboBox_Raduni_InCorso.Text != null)
            {
                var dati = comboBox_Raduni_InCorso.Text.Replace(" ", "").Split('-');
                ClientConnection.TestClient.Send($"AttaccoCooperativo|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Abbandona|{dati[1]}");
            }
            await Login.Sleep(2);
            Load_Guid();
        }

        private async void btn_Inizia_Click(object sender, EventArgs e)
        {
            btn_Inizia.Enabled = false;
            var dati = comboBox_Raduni_Creati.Text.Replace(" ", "").Split('-');
            ClientConnection.TestClient.Send($"AttaccoCooperativo|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Inizia|{dati[1]}");
            await Login.Sleep(3);
            Load_Guid();
            UpdateCombobox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"AttaccoCooperativo|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|MieiAttacchi|");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Load_Guid();
        }

        private void comboBox_Raduni_InCorso_TextChanged(object sender, EventArgs e)
        {
            if (comboBox_Raduni_InCorso.Text != "")
            {
                if (Variabili_Client.Raduni_InCorso.Count != 0)
                    foreach (string attaccoStr in Variabili_Client.Raduni_InCorso)
                    {
                        var attacco = Variabili_Client.PartecipanteAttacco.FromString(attaccoStr);
                        var testo = comboBox_Raduni_InCorso.Text.Replace(" ", "").Split('-');

                        if (attacco != null && testo[1] == attacco.ID)
                        {
                            txt_Guerriero_Spedizione.Text = attacco.Guerrieri_1;
                            txt_Lancere_Spedizione.Text = attacco.Lanceri_1;
                            txt_Arcere_Spedizione.Text = attacco.Arceri_1;
                            txt_Catapulta_Spedizione.Text = attacco.Catapulte_1;
                        }
                    }
            }
            else
            {
                txt_Guerriero_Spedizione.Text = "0";
                txt_Lancere_Spedizione.Text = "0";
                txt_Arcere_Spedizione.Text = "0";
                txt_Catapulta_Spedizione.Text = "0";
            }
        }

        private void trackBar_Guerriero_Scroll(object sender, EventArgs e)
        {
            lbl_Guerriero.Text = trackBar_Guerriero.Value.ToString();
        }
        private void trackBar_Lanciere_Scroll(object sender, EventArgs e)
        {
            lbl_Lanciere.Text = trackBar_Lanciere.Value.ToString();
        }
        private void trackBar_Arciere_Scroll(object sender, EventArgs e)
        {
            lbl_Arciere.Text = trackBar_Arciere.Value.ToString();
        }
        private void trackBar_Catapulta_Scroll(object sender, EventArgs e)
        {
            lbl_Catapulta.Text = trackBar_Catapulta.Value.ToString();
        }

        private async void btn_Attacco_PVP_Click(object sender, EventArgs e)
        {
            this.ActiveControl = lbl_Arciere;
            btn_Attacco_PVP.Enabled = false;
            this.Size = new Size(794, 550); //1178, 454
            groupBox_Raduno.Visible = true;
            btn_Sposta.Enabled = true;
            btn_Attacca.Enabled = false;
            tipo_Attacco = "PVP";
            await Login.Sleep(20);
            btn_Attacco_PVP.Enabled = true;
        }
        private async void btn_Esplora_PVE_Villaggio_B_Click(object sender, EventArgs e)
        {
            if (tipo_Barbaro == "Città Barbare")
            {
                groupBox_Raduno.Visible = true;

                if (btn_Esplora_PVE_Villaggio_B.Text == "Attacco") //Attacco con truppe
                {
                    this.Size = new Size(794, 550); //1178, 454
                    tipo_Attacco = "Città Barbaro";
                    btn_Sposta.Visible = true;
                    btn_Attacca.Visible = true;
                }
                else
                {
                    ClientConnection.TestClient.Send($"Esplora|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta Barbaro|{comboBox_Villaggi.Text.Replace("Citta Barbara - Livello: ", "")}");
                    await Login.Sleep(5);
                }
            }
            if (tipo_Barbaro == "Villaggi Barbari")
            {
                ClientConnection.TestClient.Send($"Esplora|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Villaggio Barbaro|{comboBox_Villaggi.Text.Replace("Villaggio Barbaro - Livello: ", "")}");
                txt_Guerriero_Spedizione.Text = "0";
                txt_Lancere_Spedizione.Text = "0";
                txt_Arcere_Spedizione.Text = "0";
                txt_Catapulta_Spedizione.Text = "0";

                await Login.Sleep(5);
            }
            this.ActiveControl = lbl_Arciere;
            btn_Esplora_PVE_Villaggio_B.Enabled = true;
            btn_Attacco_PVE_Villaggio_B.Enabled = true;
            Load_Guid();
        }
        private async void btn_Attacco_PVE_Villaggio_B_Click(object sender, EventArgs e)
        {
            if (tipo_Barbaro == "Città Barbare")
            {
                if (btn_Attacco_PVE_Villaggio_B.Text == "Raid")
                {
                    btn_Attacco_PVE_Villaggio_B.Text = "Raduno";
                    btn_Esplora_PVE_Villaggio_B.Text = "Attacco";
                    btn_Esplora_PVE_Villaggio_B.Enabled = true;
                    groupBox_Raduno.Visible = true;
                    btn_Sposta.Visible = false;
                    btn_Attacca.Visible = false;
                    return;
                }
                if (btn_Attacco_PVE_Villaggio_B.Text == "Raduno")
                {
                    this.Size = new Size(794, 655);
                    tipo_Attacco = "Città Barbaro";
                    btn_Attacca.Visible = false;
                    btn_Sposta.Visible = false;

                    txt_Guerriero_Spedizione.Text = "0";
                    txt_Lancere_Spedizione.Text = "0";
                    txt_Arcere_Spedizione.Text = "0";
                    txt_Catapulta_Spedizione.Text = "0";
                    return;
                }
            }
            if (tipo_Barbaro == "Villaggi Barbari")
            {
                tipo_Attacco = "Villaggio Barbaro";
                this.ActiveControl = lbl_Arciere;
                btn_Esplora_PVE_Villaggio_B.Enabled = false;
                btn_Attacco_PVE_Villaggio_B.Enabled = false;

                groupBox_Raduno.Visible = true;
                this.Size = new Size(794, 550);
                await Login.Sleep(5);
                if (txt_Guerriero_Villaggio.Text == "????")
                    btn_Esplora_PVE_Villaggio_B.Enabled = true;
                return;
            }
        }
        private void btn_Attacco_PVE_Città_B_Click(object sender, EventArgs e)
        {
            if (btn_Attacco_PVE_Villaggio_B.Text == "Raid")
            {
                btn_Attacco_PVE_Villaggio_B.Text = "Raduno";
                btn_Esplora_PVE_Villaggio_B.Text = "Attacco";
                btn_Esplora_PVE_Villaggio_B.Enabled = true;
                groupBox_Raduno.Visible = true;
                return;
            }
            if (btn_Attacco_PVE_Villaggio_B.Text == "Raduno")
            {
                this.Size = new Size(794, 655);
                tipo_Attacco = "Città Barbaro";
                btn_Attacca.Visible = false;
                btn_Sposta.Visible = false;
                return;
            }
        }

        public async Task<bool> Sleep(int secondi)
        {
            comboBox_Villaggi.Enabled = false;
            await Task.Delay(1000 * secondi);
            comboBox_Villaggi.Enabled = true;
            return true;
        }
        private void comboBox_Villaggi_TextChanged(object sender, EventArgs e)
        {
            Load_Guid();
        }
        private void comboBox_Città_TextChanged(object sender, EventArgs e)
        {
            Load_Guid();
        }

        private void comboBox_PVP_TextChanged(object sender, EventArgs e)
        {
            Load_Guid();
        }

        private void btn_Esplora_PVP_Click(object sender, EventArgs e)
        {

        }

        private async void btn_Attacca_Click(object sender, EventArgs e)
        {
            btn_Attacca.Enabled = false;
            btn_Sposta.Enabled = false;
            var Dati = comboBox_Villaggi.Text.Replace(" ", "").Split('-');
            var Dati_PVP = comboBox_PVP.Text.Replace(" ", "").Split(',');

            if (tipo_Attacco == "Villaggio Barbaro")
                ClientConnection.TestClient.Send($"" +
                    $"Battaglia|" +
                    $"{Variabili_Client.Utente.Username}|" +
                    $"{Variabili_Client.Utente.Password}|" +
                    $"{tipo_Attacco}|" +
                    $"{Dati[1].Replace("Livello:", "")}|" +
                    $"{guerrieri_Temp[0]}|{guerrieri_Temp[1]}|{guerrieri_Temp[2]}|{guerrieri_Temp[3]}|{guerrieri_Temp[4]}|" +
                    $"{picchieri_Temp[0]}|{picchieri_Temp[1]}|{picchieri_Temp[2]}|{picchieri_Temp[3]}|{picchieri_Temp[4]}|" +
                    $"{arcieri_Temp[0]}|{arcieri_Temp[1]}|{arcieri_Temp[2]}|{arcieri_Temp[3]}|{arcieri_Temp[4]}|" +
                    $"{catapulte_Temp[0]}|{catapulte_Temp[1]}|{catapulte_Temp[2]}|{catapulte_Temp[3]}|{catapulte_Temp[4]}");

            if (tipo_Attacco == "Città Barbaro")
                ClientConnection.TestClient.Send($"" +
                    $"Battaglia|" +
                    $"{Variabili_Client.Utente.Username}|" +
                    $"{Variabili_Client.Utente.Password}|" +
                    $"{tipo_Attacco}|" +
                    $"{Dati[1].Replace("Livello:", "")}|" +
                    $"{guerrieri_Temp[0]}|{guerrieri_Temp[1]}|{guerrieri_Temp[2]}|{guerrieri_Temp[3]}|{guerrieri_Temp[4]}|" +
                    $"{picchieri_Temp[0]}|{picchieri_Temp[1]}|{picchieri_Temp[2]}|{picchieri_Temp[3]}|{picchieri_Temp[4]}|" +
                    $"{arcieri_Temp[0]}|{arcieri_Temp[1]}|{arcieri_Temp[2]}|{arcieri_Temp[3]}|{arcieri_Temp[4]}|" +
                    $"{catapulte_Temp[0]}|{catapulte_Temp[1]}|{catapulte_Temp[2]}|{catapulte_Temp[3]}|{catapulte_Temp[4]}");

            if (tipo_Attacco == "PVP")
                ClientConnection.TestClient.Send($"Battaglia|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{tipo_Attacco}|{Dati_PVP[0]}|" +
                    $"{guerrieri_Temp[0]}|{guerrieri_Temp[1]}|{guerrieri_Temp[2]}|{guerrieri_Temp[3]}|{guerrieri_Temp[4]}|" +
                    $"{picchieri_Temp[0]}|{picchieri_Temp[1]}|{picchieri_Temp[2]}|{picchieri_Temp[3]}|{picchieri_Temp[4]}|" +
                    $"{arcieri_Temp[0]}|{arcieri_Temp[1]}|{arcieri_Temp[2]}|{arcieri_Temp[3]}|{arcieri_Temp[4]}|" +
                    $"{catapulte_Temp[0]}|{catapulte_Temp[1]}|{catapulte_Temp[2]}|{catapulte_Temp[3]}|{catapulte_Temp[4]}");

            guerrieri_Temp = new int[] { 0, 0, 0, 0, 0 };
            picchieri_Temp = new int[] { 0, 0, 0, 0, 0 };
            arcieri_Temp = new int[] { 0, 0, 0, 0, 0 };
            catapulte_Temp = new int[] { 0, 0, 0, 0, 0 };

            livello_Esercito = 1;
            AggiornaSfondoEsercito();
            await Sleep(4);
            await Load_Guid();
            UpdateCombobox();
            btn_Attacco_PVE_Villaggio_B.Enabled = false;
            this.Size = new Size(794, 293);
            btn_Sposta.Enabled = true;
            MostraCountdownAttacco();
        }

        private void btn_Sposta_Click(object sender, EventArgs e)
        {
            int guerriero = Convert.ToInt32(lbl_Guerriero.Text);
            int lancere = Convert.ToInt32(lbl_Lanciere.Text);
            int arcere = Convert.ToInt32(lbl_Arciere.Text);
            int catapulta = Convert.ToInt32(lbl_Catapulta.Text);

            if (livello_Esercito == 1)
            {
                if (guerrieri_Temp[0] < guerriero) guerrieri_Temp[0] += guerriero;
                if (picchieri_Temp[0] < lancere) picchieri_Temp[0] += lancere;
                if (arcieri_Temp[0] < arcere) arcieri_Temp[0] += arcere;
                if (catapulte_Temp[0] < catapulta) catapulte_Temp[0] += catapulta;
            }
            if (livello_Esercito == 2)
            {
                if (guerrieri_Temp[1] < guerriero) guerrieri_Temp[1] += guerriero;
                if (picchieri_Temp[1] < lancere) picchieri_Temp[1] += lancere;
                if (arcieri_Temp[1] < arcere) arcieri_Temp[1] += arcere;
                if (catapulte_Temp[1] < catapulta) catapulte_Temp[1] += catapulta;
            }
            if (livello_Esercito == 3)
            {
                if (guerrieri_Temp[2] < guerriero) guerrieri_Temp[2] += guerriero;
                if (picchieri_Temp[2] < lancere) picchieri_Temp[2] += lancere;
                if (arcieri_Temp[2] < arcere) arcieri_Temp[2] += arcere;
                if (catapulte_Temp[2] < catapulta) catapulte_Temp[2] += catapulta;
            }
            if (livello_Esercito == 4)
            {
                if (guerrieri_Temp[3] < guerriero) guerrieri_Temp[3] += guerriero;
                if (picchieri_Temp[3] < lancere) picchieri_Temp[3] += lancere;
                if (arcieri_Temp[3] < arcere) arcieri_Temp[3] += arcere;
                if (catapulte_Temp[3] < catapulta) catapulte_Temp[3] += catapulta;
            }
            if (livello_Esercito == 5)
            {
                if (guerrieri_Temp[4] < guerriero) guerrieri_Temp[4] += guerriero;
                if (picchieri_Temp[4] < lancere) picchieri_Temp[4] += lancere;
                if (arcieri_Temp[4] < arcere) arcieri_Temp[4] += arcere;
                if (catapulte_Temp[4] < catapulta) catapulte_Temp[4] += catapulta;
            }
            trackBar_Guerriero.Maximum = guerriero - Convert.ToInt32(txt_Guerriero_Spedizione.Text);
            trackBar_Lanciere.Maximum = lancere - Convert.ToInt32(txt_Lancere_Spedizione.Text);
            trackBar_Arciere.Maximum = arcere - Convert.ToInt32(txt_Arcere_Spedizione.Text);
            trackBar_Catapulta.Maximum = catapulta - Convert.ToInt32(txt_Catapulta_Spedizione.Text);
            if (!attacco_Premuto) btn_Attacca.Enabled = true;
            Load_Guid();
            attacco_Premuto = true;
        }

        void AggiornaSfondoEsercito()
        {
            btn_I_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_II_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_III_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_IV_Esercito.BackColor = Color.FromArgb(32, 36, 47);
            btn_V_Esercito.BackColor = Color.FromArgb(32, 36, 47);

            btn_I_Spedizione.BackColor = Color.FromArgb(32, 36, 47);
            btn_II_Spedizione.BackColor = Color.FromArgb(32, 36, 47);
            btn_III_Spedizione.BackColor = Color.FromArgb(32, 36, 47);
            btn_IV_Spedizione.BackColor = Color.FromArgb(32, 36, 47);
            btn_V_Spedizione.BackColor = Color.FromArgb(32, 36, 47);

            btn_I_Esercito.Enabled = true;
            btn_II_Esercito.Enabled = true;
            btn_III_Esercito.Enabled = true;
            btn_IV_Esercito.Enabled = true;
            btn_V_Esercito.Enabled = true;

            btn_I_Spedizione.Enabled = false;
            btn_II_Spedizione.Enabled = false;
            btn_III_Spedizione.Enabled = false;
            btn_IV_Spedizione.Enabled = false;
            btn_V_Spedizione.Enabled = false;

            if (livello_Esercito == 1)
            {
                btn_I_Esercito.BackColor = Color.DimGray;
                btn_I_Spedizione.BackColor = Color.DimGray;

                btn_I_Esercito.Enabled = false;
                btn_I_Spedizione.Enabled = true;

                txt_Guerriero_Esercito.Text = Variabili_Client.Reclutamento.Guerrieri_1.Quantità.ToString();
                txt_Lanciere_Esercito.Text = Variabili_Client.Reclutamento.Lanceri_1.Quantità.ToString();
                txt_Arciere_Esercito.Text = Variabili_Client.Reclutamento.Arceri_1.Quantità.ToString();
                txt_Catapulta_Esercito.Text = Variabili_Client.Reclutamento.Catapulte_1.Quantità.ToString();

                txt_Guerriero_Spedizione.Text = guerrieri_Temp[0].ToString();
                txt_Lancere_Spedizione.Text = picchieri_Temp[0].ToString();
                txt_Arcere_Spedizione.Text = arcieri_Temp[0].ToString();
                txt_Catapulta_Spedizione.Text = catapulte_Temp[0].ToString();
            }
            if (livello_Esercito == 2)
            {
                btn_II_Esercito.BackColor = Color.DimGray;
                btn_II_Spedizione.BackColor = Color.DimGray;

                btn_II_Esercito.Enabled = false;
                btn_II_Spedizione.Enabled = true;

                txt_Guerriero_Esercito.Text = Variabili_Client.Reclutamento.Guerrieri_2.Quantità.ToString();
                txt_Lanciere_Esercito.Text = Variabili_Client.Reclutamento.Lanceri_2.Quantità.ToString();
                txt_Arciere_Esercito.Text = Variabili_Client.Reclutamento.Arceri_2.Quantità.ToString();
                txt_Catapulta_Esercito.Text = Variabili_Client.Reclutamento.Catapulte_2.Quantità.ToString();

                txt_Guerriero_Spedizione.Text = guerrieri_Temp[1].ToString();
                txt_Lancere_Spedizione.Text = picchieri_Temp[1].ToString();
                txt_Arcere_Spedizione.Text = arcieri_Temp[1].ToString();
                txt_Catapulta_Spedizione.Text = catapulte_Temp[1].ToString();
            }
            if (livello_Esercito == 3)
            {
                btn_III_Esercito.BackColor = Color.DimGray;
                btn_III_Spedizione.BackColor = Color.DimGray;

                btn_III_Esercito.Enabled = false;
                btn_III_Spedizione.Enabled = true;

                txt_Guerriero_Esercito.Text = Variabili_Client.Reclutamento.Guerrieri_3.Quantità.ToString();
                txt_Lanciere_Esercito.Text = Variabili_Client.Reclutamento.Lanceri_3.Quantità.ToString();
                txt_Arciere_Esercito.Text = Variabili_Client.Reclutamento.Arceri_3.Quantità.ToString();
                txt_Catapulta_Esercito.Text = Variabili_Client.Reclutamento.Catapulte_3.Quantità.ToString();

                txt_Guerriero_Spedizione.Text = guerrieri_Temp[2].ToString();
                txt_Lancere_Spedizione.Text = picchieri_Temp[2].ToString();
                txt_Arcere_Spedizione.Text = arcieri_Temp[2].ToString();
                txt_Catapulta_Spedizione.Text = catapulte_Temp[2].ToString();
            }
            if (livello_Esercito == 4)
            {
                btn_IV_Esercito.BackColor = Color.DimGray;
                btn_IV_Spedizione.BackColor = Color.DimGray;

                btn_IV_Esercito.Enabled = false;
                btn_IV_Spedizione.Enabled = true;

                txt_Guerriero_Esercito.Text = Variabili_Client.Reclutamento.Guerrieri_4.Quantità.ToString();
                txt_Lanciere_Esercito.Text = Variabili_Client.Reclutamento.Lanceri_4.Quantità.ToString();
                txt_Arciere_Esercito.Text = Variabili_Client.Reclutamento.Arceri_4.Quantità.ToString();
                txt_Catapulta_Esercito.Text = Variabili_Client.Reclutamento.Catapulte_4.Quantità.ToString();

                txt_Guerriero_Spedizione.Text = guerrieri_Temp[3].ToString();
                txt_Lancere_Spedizione.Text = picchieri_Temp[3].ToString();
                txt_Arcere_Spedizione.Text = arcieri_Temp[3].ToString();
                txt_Catapulta_Spedizione.Text = catapulte_Temp[3].ToString();
            }
            if (livello_Esercito == 5)
            {
                btn_V_Esercito.BackColor = Color.DimGray;
                btn_V_Spedizione.BackColor = Color.DimGray;

                btn_V_Esercito.Enabled = false;
                btn_V_Spedizione.Enabled = true;

                txt_Guerriero_Esercito.Text = Variabili_Client.Reclutamento.Guerrieri_5.Quantità.ToString();
                txt_Lanciere_Esercito.Text = Variabili_Client.Reclutamento.Lanceri_5.Quantità.ToString();
                txt_Arciere_Esercito.Text = Variabili_Client.Reclutamento.Arceri_5.Quantità.ToString();
                txt_Catapulta_Esercito.Text = Variabili_Client.Reclutamento.Catapulte_5.Quantità.ToString();

                txt_Guerriero_Spedizione.Text = guerrieri_Temp[4].ToString();
                txt_Lancere_Spedizione.Text = picchieri_Temp[4].ToString();
                txt_Arcere_Spedizione.Text = arcieri_Temp[4].ToString();
                txt_Catapulta_Spedizione.Text = catapulte_Temp[4].ToString();
            }
        }

        private void btn_I_Esercito_Click(object sender, EventArgs e)
        {
            livello_Esercito = 1;
            AggiornaSfondoEsercito();
            Load_Guid();
            this.ActiveControl = btn_I_Esercito;
        }
        private void btn_II_Esercito_Click(object sender, EventArgs e)
        {
            livello_Esercito = 2;
            AggiornaSfondoEsercito();
            Load_Guid();
            this.ActiveControl = btn_II_Esercito;
        }
        private void btn_III_Esercito_Click(object sender, EventArgs e)
        {
            livello_Esercito = 3;
            AggiornaSfondoEsercito();
            Load_Guid();
            this.ActiveControl = btn_III_Esercito;
        }
        private void btn_IV_Esercito_Click(object sender, EventArgs e)
        {
            livello_Esercito = 4;
            AggiornaSfondoEsercito();
            Load_Guid();
            this.ActiveControl = btn_IV_Esercito;
        }
        private void btn_V_Esercito_Click(object sender, EventArgs e)
        {
            livello_Esercito = 5;
            AggiornaSfondoEsercito();
            Load_Guid();
            this.ActiveControl = btn_V_Esercito;
        }

        private void AttaccoCoordinato_FormClosing(object sender, FormClosingEventArgs e)
        {
            GameAudio.StopMusic();
            GameAudio.PlayMenuMusic("Gioco");
            MusicManager.SetVolume(0.2f);
        }

        private void btn_Villaggi_Citta_Barbare_Click(object sender, EventArgs e)
        {
            if (tipo_Barbaro == "Villaggi Barbari")
            {
                groupBox4.Text = "Città Barbare";
                tipo_Barbaro = "Città Barbare";
                btn_Attacco_PVE_Villaggio_B.Text = "Raid";
            }
            else
            {
                groupBox4.Text = "Villaggi Barbari";
                tipo_Barbaro = "Villaggi Barbari";
                btn_Attacco_PVE_Villaggio_B.Text = "Attacco";
            }
            UpdateCombobox();
        }
    }

}
