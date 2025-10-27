using Strategico_V2;


namespace CriptoGame_Online.GUI
{
    public partial class AttaccoCoordinato : Form
    {
        public AttaccoCoordinato()
        {
            InitializeComponent();
            this.Size = new Size(1178, 258);
            groupBox_Raduno.Visible = false;
        }
        private async void AttaccoCoordinato_Load(object sender, EventArgs e)
        {
            Load_Guid();

            comboBox_Villaggi.Items.Clear();
            foreach (var item in Variabili_Client.VillaggiPersonali)
                comboBox_Villaggi.Items.Add("Villaggio Barbaro - Livello: " + item.Livello);

            comboBox_Città.Items.Clear();
            foreach (var item in Variabili_Client.CittaGlobali)
                comboBox_Città.Items.Add("Citta Barbara - Livello: " + item.Livello);
            txt_Villaggio_B_Desc.Text = "Esplora il barbaro per avere una stima delle sue truppe";
            txt_Città_B_Desc.Text = "Raggiungi il livello 5 per sbloccare le città. \n\rEsplora il barbaro per avere una stima delle sue truppe";

            this.ActiveControl = btn_Crea; // assegna il focus al bottone
        }

        private async Task Load_Guid()
        {
            comboBox_PVP.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox_PVP.DrawItem += (s, e) =>
            {
                comboBox_PVP.DrawMode = DrawMode.OwnerDrawFixed;
                e.DrawBackground();
                if (e.Index < 0)
                    using (var brush = new SolidBrush(SystemColors.GrayText)) // Nessun elemento selezionato → disegno il placeholder
                    {
                        e.Graphics.DrawString("Seleziona giocatore", e.Font, brush, e.Bounds);
                    }
                else
                {
                    e.Graphics.DrawString(comboBox_PVP.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds); // Disegno normalmente gli elementi
                }
            };

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

            txt_Guerriero_Esercito.Text = Variabili_Client.Reclutamento.Guerrieri_1.Quantità.ToString();
            txt_Lanciere_Esercito.Text = Variabili_Client.Reclutamento.Lanceri_1.Quantità.ToString();
            txt_Arciere_Esercito.Text = Variabili_Client.Reclutamento.Arceri_1.Quantità.ToString();
            txt_Catapulta_Esercito.Text = Variabili_Client.Reclutamento.Catapulte_1.Quantità.ToString();

            string g = Variabili_Client.Reclutamento.Guerrieri_1.Quantità;
            string l = Variabili_Client.Reclutamento.Lanceri_1.Quantità;
            string a = Variabili_Client.Reclutamento.Arceri_1.Quantità;
            string c = Variabili_Client.Reclutamento.Catapulte_1.Quantità;

            if (Convert.ToInt32(g) + Convert.ToInt32(l) + Convert.ToInt32(a) + Convert.ToInt32(c) > 0)
            {
                trackBar_Guerriero.Maximum = Convert.ToInt32(g);
                trackBar_Lanciere.Maximum = Convert.ToInt32(l);
                trackBar_Arciere.Maximum = Convert.ToInt32(a);
                trackBar_Catapulta.Maximum = Convert.ToInt32(c);
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

            var items = comboBox_PVP.Items;
            if (Variabili_Client.Giocatori_PVP.Count > 0)
                foreach (var i in Variabili_Client.Giocatori_PVP)
                    if (!items.Contains(i) && !i.Contains(Variabili_Client.Utente.Username))
                        comboBox_PVP.Items.Add(i);

            lbl_Giocatori_PVP.Text = "Giocatori: " + comboBox_PVP.Items.Count;

            int index = 0;
            if (comboBox_Villaggi.SelectedIndex < 1)
                index = 1;
            else index = comboBox_Villaggi.SelectedIndex;

            //Update truppe Villaggi Barbari
            var villaggio = Variabili_Client.VillaggiPersonali.FirstOrDefault(v => v.Livello == index);
            if (comboBox_Villaggi.SelectedIndex != -1)
            {
                int T_Truppe = villaggio.Guerrieri + villaggio.Lancieri + villaggio.Arcieri + villaggio.Catapulte;
                if (villaggio != null && T_Truppe > 0)
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

                if (villaggio.Esplorato == true)
                    btn_Esplora_PVE_Villaggio_B.Enabled = false;
                else
                if (comboBox_Villaggi.SelectedIndex < 0)
                    btn_Esplora_PVE_Villaggio_B.Enabled = false;
                else
                    btn_Esplora_PVE_Villaggio_B.Enabled = true;
                btn_Attacco_PVE_Villaggio_B.Enabled = true;
            }

            //Update truppe Citta Barbara
            var Città = Variabili_Client.CittaGlobali.FirstOrDefault(v => v.Livello == index);
            if (comboBox_Città.SelectedIndex != -1)
            {
                int T_Truppe = Città.Guerrieri + Città.Lancieri + Città.Arcieri + Città.Catapulte;
                if (Città != null && T_Truppe > 0)
                {
                    txt_Guerriero_Città.Text = Città.Guerrieri.ToString();
                    txt_Lancere_Città.Text = Città.Lancieri.ToString();
                    txt_Arcere_Città.Text = Città.Arcieri.ToString();
                    txt_Catapulta_Città.Text = Città.Catapulte.ToString();
                }
                else
                {
                    txt_Guerriero_Città.Text = "????";
                    txt_Lancere_Città.Text = "????";
                    txt_Arcere_Città.Text = "????";
                    txt_Catapulta_Città.Text = "????";
                }

                if (Città.Esplorato == true)
                    btn_Esplora_PVE_Città_B.Enabled = false;
                else
                    if (comboBox_Città.SelectedIndex < 0)
                    btn_Esplora_PVE_Città_B.Enabled = false;
                else
                    btn_Esplora_PVE_Città_B.Enabled = true;
                btn_Attacco_PVE_Città_B.Enabled = true;
            }

            //Descrizioni Villaggi e Città
            if (Città.Esplorato == true)
                txt_Città_B_Desc.Text = $"Hai esplorato con successo la città barbara, Bottino;\r\n Exp: {Città.Esperienza} Liv: {Città.Livello}\r\n Diamanti Blu: {Città.Diamanti_Blu} Diamanti Viola: {Città.Diamanti_Viola}\r\n" +
                    $"Cibo: {Città.Cibo} Legno: {Città.Legno} Pietra: {Città.Pietra}\r\nFerro: {Città.Ferro} Oro: {Città.Oro}";
            if (villaggio.Esplorato == true)
                txt_Villaggio_B_Desc.Text = $"Hai esplorato con successo il villaggio barbaro, Bottino;\r\n Exp: {villaggio.Esperienza} Liv: {villaggio.Livello}\r\n Diamanti Blu: {villaggio.Diamanti_Blu} Diamanti Viola: {villaggio.Diamanti_Viola}\r\n" +
                    $"Cibo: {villaggio.Cibo} Legno: {villaggio.Legno} Pietra: {villaggio.Pietra}\r\nFerro: {villaggio.Ferro} Oro: {villaggio.Oro}";

            this.ActiveControl = btn_Crea; // assegna il focus al bottone
        }

        private async void btn_Crea_Click(object sender, EventArgs e)
        {
            btn_Crea.Enabled = false;
            btn_Attacco_PVE_Città_B.Text = "Raid";
            btn_Esplora_PVE_Città_B.Text = "Esplora";


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
                            txt_Lanciere_Spedizione.Text = attacco.Lanceri_1;
                            txt_Arciere_Spedizione.Text = attacco.Arceri_1;
                            txt_Catapulta_Spedizione.Text = attacco.Catapulte_1;
                        }
                    }
            }
            else
            {
                txt_Guerriero_Spedizione.Text = "0";
                txt_Lanciere_Spedizione.Text = "0";
                txt_Arciere_Spedizione.Text = "0";
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
            btn_Attacco_PVP.Enabled = false;

            ClientConnection.TestClient.Send($"Battaglia|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|PVP|{comboBox_PVP.Text}");
            await Login.Sleep(20);
            btn_Attacco_PVP.Enabled = true;
        }
        public static async Task<bool> Sleep(int secondi)
        {
            await Task.Delay(1000 * secondi);
            return true;
        }

        private async void btn_Esplora_PVE_Villaggio_B_Click(object sender, EventArgs e)
        {
            btn_Esplora_PVE_Villaggio_B.Enabled = false;
            btn_Attacco_PVE_Villaggio_B.Enabled = false;
            ClientConnection.TestClient.Send($"Esplora|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Villaggio Barbaro|{comboBox_Villaggi.Text.Replace("Villaggio Barbaro - Livello: ", "")}");
            await Login.Sleep(5);
            btn_Esplora_PVE_Villaggio_B.Enabled = true;
            btn_Attacco_PVE_Villaggio_B.Enabled = true;
            Load_Guid();
        }
        private async void btn_Esplora_PVE_Città_B_Click(object sender, EventArgs e)
        {
            btn_Esplora_PVE_Città_B.Enabled = false;
            btn_Attacco_PVE_Città_B.Enabled = false;
            groupBox_Raduno.Visible = true;

            if (btn_Esplora_PVE_Città_B.Text == "Attacco") //Attacco con truppe
            {
                this.Size = new Size(1178, 488); //1178, 454


                //Lettura truppe da inviare

                //Invio comando attacco
                //IL server esegue l'attacco ed invia indietro qualcosa
            }
            else
            {
                //Semplice esplorazione
                ClientConnection.TestClient.Send($"Esplora|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Citta Barbara|{comboBox_Città.Text.Replace("Citta Barbara - Livello: ", "")}");
                await Login.Sleep(5);
            }

            btn_Esplora_PVE_Città_B.Enabled = true;
            btn_Attacco_PVE_Città_B.Enabled = true;
            Load_Guid();
        }

        private void comboBox_Villaggi_TextChanged(object sender, EventArgs e)
        {
            Load_Guid();
        }
        private void comboBox_Città_TextChanged(object sender, EventArgs e)
        {
            Load_Guid();
        }

        private async void btn_Attacco_PVE_Villaggio_B_Click(object sender, EventArgs e)
        {
            btn_Esplora_PVE_Villaggio_B.Enabled = false;
            btn_Attacco_PVE_Villaggio_B.Enabled = false;

            if (groupBox_Raduno.Visible == true)
            {

            }

            ClientConnection.TestClient.Send($"Battaglia|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Villaggio Barbaro|{comboBox_Villaggi.Text.Replace("Villaggio Barbaro - Livello: ", "")}");
            await Login.Sleep(5);
            btn_Esplora_PVE_Villaggio_B.Enabled = true;
            btn_Attacco_PVE_Villaggio_B.Enabled = true;
        }

        private void btn_Attacco_PVE_Città_B_Click(object sender, EventArgs e)
        {
            if (btn_Attacco_PVE_Città_B.Text == "Raid")
            {
                btn_Attacco_PVE_Città_B.Text = "Raduno";
                btn_Esplora_PVE_Città_B.Text = "Attacco";
                groupBox_Raduno.Visible = true;
                return;
            }
            if (btn_Attacco_PVE_Città_B.Text == "Raduno")
            {
                this.Size = new Size(1178, 592);

                return;
            }
        }
    }
}
