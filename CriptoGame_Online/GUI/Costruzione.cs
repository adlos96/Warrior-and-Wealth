using Strategico_V2;

namespace CriptoGame_Online
{
    public partial class Costruzione : Form
    {
        int livello_Esercito = 1;
        public static CustomToolTip toolTip1;
        public Costruzione()
        {
            toolTip1 = new CustomToolTip();

            // Imposta qualche proprietà opzionale
            toolTip1.InitialDelay = 150;
            toolTip1.AutoPopDelay = 8000;

            InitializeComponent();
            this.ActiveControl = Btn_Costruzione; // assegna il focus al bottone

            btn_I.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            btn_II.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            btn_III.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            btn_IV.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);
            btn_V.Font = new Font("Cinzel Decorative", 7, FontStyle.Bold);

            toolTip1.SetToolTip(this.ico_Structure_1, "[black]Fattorie");
            toolTip1.SetToolTip(this.ico_Structure_2, "[black]Segherie");
            toolTip1.SetToolTip(this.ico_Structure_3, "[black]Cave di pietra");
            toolTip1.SetToolTip(this.ico_Structure_4, "[black]Miniere di ferro");
            toolTip1.SetToolTip(this.ico_Structure_5, "[black]Miniere d'oro");
            toolTip1.SetToolTip(this.ico_Structure_6, "[black]Abitazioni");

            toolTip1.SetToolTip(this.ico_Structure_7, "[black]Workshop spade");
            toolTip1.SetToolTip(this.ico_Structure_8, "[black]Workshop lance");
            toolTip1.SetToolTip(this.ico_Structure_9, "[black]Workshop archi");
            toolTip1.SetToolTip(this.ico_Structure_10, "[black]Workshop scudi");
            toolTip1.SetToolTip(this.ico_Structure_11, "[black]Workshop armature");
            toolTip1.SetToolTip(this.ico_Structure_12, "[black]Workshop frecce");

            toolTip1.SetToolTip(this.ico_Unita_1, "[black]Guerrieri");
            toolTip1.SetToolTip(this.ico_Unita_2, "[black]Lanceri");
            toolTip1.SetToolTip(this.ico_Unita_3, "[black]Arceri");
            toolTip1.SetToolTip(this.ico_Unita_4, "[black]Catapulte");

            toolTip1.SetToolTip(this.ico_Caserma_1, "[black]Caserma guerrieri");
            toolTip1.SetToolTip(this.ico_Caserma_2, "[black]Caserma lanceri");
            toolTip1.SetToolTip(this.ico_Caserma_3, "[black]Caserma arceri");
            toolTip1.SetToolTip(this.ico_Caserma_4, "[black]Caserma catapulte");

            UnlockSoldierTier(livello_Esercito);
        }

        private void Costruzione_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
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

        #region Bottoni livelli esercito
        private void btn_I_Click(object sender, EventArgs e)
        {
            livello_Esercito = 1;
            UnlockSoldierTier(livello_Esercito);
            this.ActiveControl = btn_Reclutamento; // assegna il focus al bottone
        }
        private void btn_II_Click(object sender, EventArgs e)
        {
            livello_Esercito = 2;
            UnlockSoldierTier(livello_Esercito);
            this.ActiveControl = btn_Reclutamento; // assegna il focus al bottone
        }
        private void btn_III_Click(object sender, EventArgs e)
        {
            livello_Esercito = 3;
            UnlockSoldierTier(livello_Esercito);
            this.ActiveControl = btn_Reclutamento; // assegna il focus al bottone
        }
        private void btn_IV_Click(object sender, EventArgs e)
        {
            livello_Esercito = 4;
            UnlockSoldierTier(livello_Esercito);
            this.ActiveControl = btn_Reclutamento; // assegna il focus al bottone
        }
        private void btn_V_Click(object sender, EventArgs e)
        {
            livello_Esercito = 5;
            UnlockSoldierTier(livello_Esercito);
            this.ActiveControl = btn_Reclutamento; // assegna il focus al bottone
        }
        #endregion

        private void Btn_Costruzione_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"Costruzione|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|" +
                $"{txt_Fattoria_Costruzione.Text}|" +
                $"{txt_Segheria_Costruzione.Text}|" +
                $"{txt_Cava_Costruzione.Text}|" +
                $"{txt_MinieraFerro_Costruzione.Text}|" +
                $"{txt_MinieraOro_Costruzione.Text}|" +
                $"{txt_Case_Costruzione.Text}|" +
                $"{txt_Workshop_Spade_Costruzione.Text}|" +
                $"{txt_Workshop_Lance_Costruzione.Text}|" +
                $"{txt_Workshop_Archi_Costruzione.Text}|" +
                $"{txt_Workshop_Scudi_Costruzione.Text}|" +
                $"{txt_Workshop_Armature_Costruzione.Text}|" +
                $"{txt_Workshop_Frecce_Costruzione.Text}|" +
                $"{txt_Caserme_Guerrieri_Costruzione.Text}|" +
                $"{txt_Caserme_Arceri_Costruzione.Text}|" +
                $"{txt_Caserme_Lanceri_Costruzione.Text}|" +
                $"{txt_Caserme_Catapulte_Costruzione.Text}");

            txt_Fattoria_Costruzione.Text = "0";
            txt_Segheria_Costruzione.Text = "0";
            txt_Cava_Costruzione.Text = "0";
            txt_MinieraFerro_Costruzione.Text = "0";
            txt_MinieraOro_Costruzione.Text = "0";
            txt_Case_Costruzione.Text = "0";
            txt_Workshop_Spade_Costruzione.Text = "0";
            txt_Workshop_Lance_Costruzione.Text = "0";
            txt_Workshop_Archi_Costruzione.Text = "0";
            txt_Workshop_Scudi_Costruzione.Text = "0";
            txt_Workshop_Armature_Costruzione.Text = "0";
            txt_Workshop_Frecce_Costruzione.Text = "0";

            txt_Caserme_Guerrieri_Costruzione.Text = "0";
            txt_Caserme_Arceri_Costruzione.Text = "0";
            txt_Caserme_Lanceri_Costruzione.Text = "0";
            txt_Caserme_Catapulte_Costruzione.Text = "0";
        }
        private void btn_Reclutamento_Click(object sender, EventArgs e)
        {
            string livello = "1";
            if (btn_II.Enabled == false) livello = "2";
            if (btn_III.Enabled == false) livello = "3";
            if (btn_IV.Enabled == false) livello = "4";
            if (btn_V.Enabled == false) livello = "5";

            ClientConnection.TestClient.Send($"Reclutamento|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|{livello}|" +
                $"{txt_Guerriero_Reclutamento.Text}|" +
                $"{txt_Lancere_Reclutamento.Text}|" +
                $"{txt_Arcere_Reclutamento.Text}|" +
                $"{txt_Catapulta_Reclutamento.Text}|");

            txt_Guerriero_Reclutamento.Text = "0";
            txt_Lancere_Reclutamento.Text = "0";
            txt_Arcere_Reclutamento.Text = "0";
            txt_Catapulta_Reclutamento.Text = "0";
        }

        #region Costruzioni Civili
        private void Set_0_Fattoria_Click(object sender, EventArgs e)
        {
            txt_Fattoria_Costruzione.Text = "0";
        }
        private void add_1_Fattoria_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Fattoria_Costruzione.Text = (Convert.ToInt32(txt_Fattoria_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Fattoria_Costruzione.Text = (Convert.ToInt32(txt_Fattoria_Costruzione.Text) + 5).ToString();
            else
                txt_Fattoria_Costruzione.Text = (Convert.ToInt32(txt_Fattoria_Costruzione.Text) + 1).ToString();
        }

        private void Set_0_Segheria_Click(object sender, EventArgs e)
        {
            txt_Segheria_Costruzione.Text = "0";
        }
        private void add_1_Segheria_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Segheria_Costruzione.Text = (Convert.ToInt32(txt_Segheria_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Segheria_Costruzione.Text = (Convert.ToInt32(txt_Segheria_Costruzione.Text) + 5).ToString();
            else
                txt_Segheria_Costruzione.Text = (Convert.ToInt32(txt_Segheria_Costruzione.Text) + 1).ToString();
        }

        private void Set_0_Cava_Click(object sender, EventArgs e)
        {
            txt_Cava_Costruzione.Text = "0";
        }
        private void add_1_Cava_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Cava_Costruzione.Text = (Convert.ToInt32(txt_Cava_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Cava_Costruzione.Text = (Convert.ToInt32(txt_Cava_Costruzione.Text) + 5).ToString();
            else
                txt_Cava_Costruzione.Text = (Convert.ToInt32(txt_Cava_Costruzione.Text) + 1).ToString();
        }

        private void Set_0_MinieraFerro_Click(object sender, EventArgs e)
        {
            txt_MinieraFerro_Costruzione.Text = "0";
        }
        private void add_1_MinieraFerro_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_MinieraFerro_Costruzione.Text = (Convert.ToInt32(txt_MinieraFerro_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_MinieraFerro_Costruzione.Text = (Convert.ToInt32(txt_MinieraFerro_Costruzione.Text) + 5).ToString();
            else
                txt_MinieraFerro_Costruzione.Text = (Convert.ToInt32(txt_MinieraFerro_Costruzione.Text) + 1).ToString();
        }

        private void Set_0_MinieraOro_Click(object sender, EventArgs e)
        {
            txt_MinieraOro_Costruzione.Text = "0";
        }
        private void add_1_MinieraOro_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_MinieraOro_Costruzione.Text = (Convert.ToInt32(txt_MinieraOro_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_MinieraOro_Costruzione.Text = (Convert.ToInt32(txt_MinieraOro_Costruzione.Text) + 5).ToString();
            else
                txt_MinieraOro_Costruzione.Text = (Convert.ToInt32(txt_MinieraOro_Costruzione.Text) + 1).ToString();
        }

        private void Set_0_Case_Click(object sender, EventArgs e)
        {
            txt_Case_Costruzione.Text = "0";
        }
        private void add_1_Case_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Case_Costruzione.Text = (Convert.ToInt32(txt_Case_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Case_Costruzione.Text = (Convert.ToInt32(txt_Case_Costruzione.Text) + 5).ToString();
            else
                txt_Case_Costruzione.Text = (Convert.ToInt32(txt_Case_Costruzione.Text) + 1).ToString();
        }
        #endregion
        #region Costruzioni Militari
        private void Set_0_Workshop_Spade_Click(object sender, EventArgs e)
        {
            txt_Workshop_Spade_Costruzione.Text = "0";
        }
        private void Add_1_Workshop_Spade_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Workshop_Spade_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Spade_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Workshop_Spade_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Spade_Costruzione.Text) + 5).ToString();
            else
                txt_Workshop_Spade_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Spade_Costruzione.Text) + 1).ToString();
        }

        private void Set_0_Workshop_Lance_Click(object sender, EventArgs e)
        {
            txt_Workshop_Lance_Costruzione.Text = "0";
        }
        private void Add_1_Workshop_Lance_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Workshop_Lance_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Lance_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Workshop_Lance_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Lance_Costruzione.Text) + 5).ToString();
            else
                txt_Workshop_Lance_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Lance_Costruzione.Text) + 1).ToString();
        }

        private void Set_0_Workshop_Archi_Click(object sender, EventArgs e)
        {
            txt_Workshop_Archi_Costruzione.Text = "0";
        }
        private void Add_1_Workshop_Archi_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Workshop_Archi_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Archi_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Workshop_Archi_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Archi_Costruzione.Text) + 5).ToString();
            else
                txt_Workshop_Archi_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Archi_Costruzione.Text) + 1).ToString();
        }

        private void Set_0_Workshop_Scudi_Click(object sender, EventArgs e)
        {
            txt_Workshop_Scudi_Costruzione.Text = "0";
        }
        private void Add_1_Workshop_Scudi_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Workshop_Scudi_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Scudi_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Workshop_Scudi_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Scudi_Costruzione.Text) + 5).ToString();
            else
                txt_Workshop_Scudi_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Scudi_Costruzione.Text) + 1).ToString();
        }

        private void Set_0_Workshop_Armature_Click(object sender, EventArgs e)
        {
            txt_Workshop_Armature_Costruzione.Text = "0";
        }
        private void Add_1_Workshop_Armature_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Workshop_Armature_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Armature_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Workshop_Armature_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Armature_Costruzione.Text) + 5).ToString();
            else
                txt_Workshop_Armature_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Armature_Costruzione.Text) + 1).ToString();
        }

        private void Set_0_Workshop_Frecce_Click(object sender, EventArgs e)
        {
            txt_Workshop_Frecce_Costruzione.Text = "0";
        }
        private void Add_1_Workshop_Frecce_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Workshop_Frecce_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Frecce_Costruzione.Text) + 1).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Workshop_Frecce_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Frecce_Costruzione.Text) + 5).ToString();
            else
                txt_Workshop_Frecce_Costruzione.Text = (Convert.ToInt32(txt_Workshop_Frecce_Costruzione.Text) + 1).ToString();
        }
        #endregion
        #region Esercito
        private void Set_0_Guerriero_Click(object sender, EventArgs e)
        {
            txt_Guerriero_Reclutamento.Text = "0";
        }
        private void Add_1_Guerriero_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Guerriero_Reclutamento.Text = (Convert.ToInt32(txt_Guerriero_Reclutamento.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Guerriero_Reclutamento.Text = (Convert.ToInt32(txt_Guerriero_Reclutamento.Text) + 5).ToString();
            else
                txt_Guerriero_Reclutamento.Text = (Convert.ToInt32(txt_Guerriero_Reclutamento.Text) + 1).ToString();
        }
        private void Add_5_Guerriero_Click(object sender, EventArgs e)
        {
            txt_Guerriero_Reclutamento.Text = (Convert.ToInt32(txt_Guerriero_Reclutamento.Text) + 5).ToString();
        }
        private void Add_10_Guerriero_Click(object sender, EventArgs e)
        {
            txt_Guerriero_Reclutamento.Text = (Convert.ToInt32(txt_Guerriero_Reclutamento.Text) + 10).ToString();
        }


        private void Set_0_Lancere_Click(object sender, EventArgs e)
        {

            txt_Lancere_Reclutamento.Text = "0";
        }
        private void Add_1_Lancere_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Lancere_Reclutamento.Text = (Convert.ToInt32(txt_Lancere_Reclutamento.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Lancere_Reclutamento.Text = (Convert.ToInt32(txt_Lancere_Reclutamento.Text) + 5).ToString();
            else
                txt_Lancere_Reclutamento.Text = (Convert.ToInt32(txt_Lancere_Reclutamento.Text) + 1).ToString();
        }
        private void Add_5_Lancere_Click(object sender, EventArgs e)
        {
            txt_Lancere_Reclutamento.Text = (Convert.ToInt32(txt_Lancere_Reclutamento.Text) + 5).ToString();
        }
        private void Add_10_Lancere_Click(object sender, EventArgs e)
        {
            txt_Lancere_Reclutamento.Text = (Convert.ToInt32(txt_Lancere_Reclutamento.Text) + 10).ToString();
        }


        private void Set_0_Arcere_Click(object sender, EventArgs e)
        {
            txt_Arcere_Reclutamento.Text = "0";
        }
        private void Add_1_Arcere_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Arcere_Reclutamento.Text = (Convert.ToInt32(txt_Arcere_Reclutamento.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Arcere_Reclutamento.Text = (Convert.ToInt32(txt_Arcere_Reclutamento.Text) + 5).ToString();
            else
                txt_Arcere_Reclutamento.Text = (Convert.ToInt32(txt_Arcere_Reclutamento.Text) + 1).ToString();
        }
        private void Add_5_Arcere_Click(object sender, EventArgs e)
        {
            txt_Arcere_Reclutamento.Text = (Convert.ToInt32(txt_Arcere_Reclutamento.Text) + 5).ToString();
        }
        private void Add_10_Arcere_Click(object sender, EventArgs e)
        {
            txt_Arcere_Reclutamento.Text = (Convert.ToInt32(txt_Arcere_Reclutamento.Text) + 10).ToString();
        }


        private void Set_0_Catapulta_Click(object sender, EventArgs e)
        {
            txt_Catapulta_Reclutamento.Text = "0";
        }
        private void Add_1_Catapulta_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Catapulta_Reclutamento.Text = (Convert.ToInt32(txt_Catapulta_Reclutamento.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Catapulta_Reclutamento.Text = (Convert.ToInt32(txt_Catapulta_Reclutamento.Text) + 5).ToString();
            else
                txt_Catapulta_Reclutamento.Text = (Convert.ToInt32(txt_Catapulta_Reclutamento.Text) + 1).ToString();
        }
        private void Add_5_Catapulta_Click(object sender, EventArgs e)
        {
            txt_Catapulta_Reclutamento.Text = (Convert.ToInt32(txt_Catapulta_Reclutamento.Text) + 5).ToString();
        }
        private void Add_10_Catapulta_Click(object sender, EventArgs e)
        {
            txt_Catapulta_Reclutamento.Text = (Convert.ToInt32(txt_Catapulta_Reclutamento.Text) + 10).ToString();
        }
        #endregion
        #region Caserme
        private void Set_0_Caserme_Guerrieri_Click(object sender, EventArgs e)
        {
            txt_Caserme_Guerrieri_Costruzione.Text = "0";
        }
        private void Add_1_Caserme_Guerrieri_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Caserme_Guerrieri_Costruzione.Text = (Convert.ToInt32(txt_Caserme_Guerrieri_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Caserme_Guerrieri_Costruzione.Text = (Convert.ToInt32(txt_Caserme_Guerrieri_Costruzione.Text) + 5).ToString();
            else
                txt_Caserme_Guerrieri_Costruzione.Text = (Convert.ToInt32(txt_Caserme_Guerrieri_Costruzione.Text) + 1).ToString();
        }

        private void Set_0_Caserme_Lanceri_Click(object sender, EventArgs e)
        {
            txt_Caserme_Lanceri_Costruzione.Text = "0";
        }
        private void Add_1_Caserme_Lanceri_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Caserme_Arceri_Costruzione.Text = (Convert.ToInt32(txt_Caserme_Arceri_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Caserme_Arceri_Costruzione.Text = (Convert.ToInt32(txt_Caserme_Arceri_Costruzione.Text) + 5).ToString();
            else
                txt_Caserme_Arceri_Costruzione.Text = (Convert.ToInt32(txt_Caserme_Arceri_Costruzione.Text) + 1).ToString();
        }

        private void Set_0_Caserme_Arceri_Click(object sender, EventArgs e)
        {
            txt_Caserme_Arceri_Costruzione.Text = "0";
        }
        private void Add_1_Caserme_Arceri_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Caserme_Lanceri_Costruzione.Text = (Convert.ToInt32(txt_Caserme_Lanceri_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Caserme_Lanceri_Costruzione.Text = (Convert.ToInt32(txt_Caserme_Lanceri_Costruzione.Text) + 5).ToString();
            else
                txt_Caserme_Lanceri_Costruzione.Text = (Convert.ToInt32(txt_Caserme_Lanceri_Costruzione.Text) + 1).ToString();
        }


        private void Set_0_Caserme_Catapulte_Click(object sender, EventArgs e)
        {
            txt_Caserme_Catapulte_Costruzione.Text = "0";
        }
        private void Add_1_Caserme_Catapulte_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                txt_Caserme_Catapulte_Costruzione.Text = (Convert.ToInt32(txt_Caserme_Catapulte_Costruzione.Text) + 10).ToString();
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                txt_Caserme_Catapulte_Costruzione.Text = (Convert.ToInt32(txt_Caserme_Catapulte_Costruzione.Text) + 5).ToString();
            else
                txt_Caserme_Catapulte_Costruzione.Text = (Convert.ToInt32(txt_Caserme_Catapulte_Costruzione.Text) + 1).ToString();
        }
        #endregion
    }
}
