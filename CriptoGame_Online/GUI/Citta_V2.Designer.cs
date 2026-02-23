namespace CriptoGame_Online.GUI
{
    partial class Citta_V2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Citta_V2));
            panel_Sfondo = new Panel();
            panel2 = new Panel();
            txt_Testo = new TextBox();
            panel1 = new Panel();
            panel_Centro = new Panel();
            lbl_Centro = new Label();
            panel_Soldier_Citta = new Panel();
            btn_Citta = new Button();
            panel_Castello = new Panel();
            lbl_Castello = new Label();
            panel_Def_Castello = new Panel();
            panel_Hp_Castello = new Panel();
            panel_Soldier_Castello = new Panel();
            btn_Castello = new Button();
            pictureBox_Castello_Salute = new PictureBox();
            pictureBox_Castello_Difesa = new PictureBox();
            panel_Torri = new Panel();
            lbl_Torri = new Label();
            btn_Torri = new Button();
            panel_Soldier_Torri = new Panel();
            panel_Hp_Torri = new Panel();
            panel_Def_Torri = new Panel();
            pictureBox_Torri_Difesa = new PictureBox();
            pictureBox_Torri_Salute = new PictureBox();
            panel_Mura = new Panel();
            lbl_Mura = new Label();
            panel_Hp_Mura = new Panel();
            panel_Soldier_Mura = new Panel();
            panel_Def_Mura = new Panel();
            btn_Mura = new Button();
            pictureBox_Mura_Salute = new PictureBox();
            pictureBox_Mura_Difesa = new PictureBox();
            panel_Ingresso = new Panel();
            lbl_Ingresso = new Label();
            panel_Soldier_Ingresso = new Panel();
            btn_Ingresso = new Button();
            panel_Cancello = new Panel();
            lbl_Cancello = new Label();
            panel_Soldier_Cancello = new Panel();
            panel_Hp_Cancello = new Panel();
            btn_Cancello = new Button();
            panel_Def_Cancello = new Panel();
            pictureBox_Cancello_Salute = new PictureBox();
            pictureBox_Cancello_Difesa = new PictureBox();
            btn_Ripara_Tutto = new Button();
            panel_Sfondo.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            panel_Centro.SuspendLayout();
            panel_Castello.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Castello_Salute).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Castello_Difesa).BeginInit();
            panel_Torri.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Torri_Difesa).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Torri_Salute).BeginInit();
            panel_Mura.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Mura_Salute).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Mura_Difesa).BeginInit();
            panel_Ingresso.SuspendLayout();
            panel_Cancello.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Cancello_Salute).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Cancello_Difesa).BeginInit();
            SuspendLayout();
            // 
            // panel_Sfondo
            // 
            panel_Sfondo.BackgroundImage = Properties.Resources._11111111111;
            panel_Sfondo.Controls.Add(panel2);
            panel_Sfondo.Controls.Add(panel1);
            panel_Sfondo.Dock = DockStyle.Top;
            panel_Sfondo.Location = new Point(0, 0);
            panel_Sfondo.Name = "panel_Sfondo";
            panel_Sfondo.Size = new Size(719, 675);
            panel_Sfondo.TabIndex = 4;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(txt_Testo);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(719, 75);
            panel2.TabIndex = 40;
            // 
            // txt_Testo
            // 
            txt_Testo.BorderStyle = BorderStyle.None;
            txt_Testo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txt_Testo.Location = new Point(3, 13);
            txt_Testo.Multiline = true;
            txt_Testo.Name = "txt_Testo";
            txt_Testo.ReadOnly = true;
            txt_Testo.Size = new Size(713, 49);
            txt_Testo.TabIndex = 33;
            txt_Testo.Text = resources.GetString("txt_Testo.Text");
            txt_Testo.TextAlign = HorizontalAlignment.Center;
            // 
            // panel1
            // 
            panel1.BackgroundImage = (Image)resources.GetObject("panel1.BackgroundImage");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.Controls.Add(panel_Centro);
            panel1.Controls.Add(panel_Castello);
            panel1.Controls.Add(panel_Torri);
            panel1.Controls.Add(panel_Mura);
            panel1.Controls.Add(panel_Ingresso);
            panel1.Controls.Add(panel_Cancello);
            panel1.Controls.Add(btn_Ripara_Tutto);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 75);
            panel1.Name = "panel1";
            panel1.Size = new Size(719, 600);
            panel1.TabIndex = 39;
            // 
            // panel_Centro
            // 
            panel_Centro.BackColor = Color.Transparent;
            panel_Centro.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Centro.Controls.Add(lbl_Centro);
            panel_Centro.Controls.Add(panel_Soldier_Citta);
            panel_Centro.Controls.Add(btn_Citta);
            panel_Centro.ForeColor = Color.Transparent;
            panel_Centro.Location = new Point(230, 306);
            panel_Centro.Name = "panel_Centro";
            panel_Centro.Size = new Size(152, 74);
            panel_Centro.TabIndex = 59;
            // 
            // lbl_Centro
            // 
            lbl_Centro.AutoSize = true;
            lbl_Centro.BackColor = Color.Transparent;
            lbl_Centro.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Centro.ForeColor = Color.LightGray;
            lbl_Centro.Location = new Point(7, 5);
            lbl_Centro.Name = "lbl_Centro";
            lbl_Centro.Size = new Size(78, 15);
            lbl_Centro.TabIndex = 44;
            lbl_Centro.Text = "Centro      [5]";
            // 
            // panel_Soldier_Citta
            // 
            panel_Soldier_Citta.BackColor = Color.Transparent;
            panel_Soldier_Citta.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Soldier_Citta.ForeColor = Color.Transparent;
            panel_Soldier_Citta.Location = new Point(4, 24);
            panel_Soldier_Citta.Name = "panel_Soldier_Citta";
            panel_Soldier_Citta.Size = new Size(143, 17);
            panel_Soldier_Citta.TabIndex = 43;
            panel_Soldier_Citta.Paint += panel_Soldier_Citta_Paint;
            // 
            // btn_Citta
            // 
            btn_Citta.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Citta.FlatAppearance.BorderSize = 0;
            btn_Citta.FlatStyle = FlatStyle.Popup;
            btn_Citta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Citta.ForeColor = Color.Black;
            btn_Citta.Location = new Point(32, 47);
            btn_Citta.Name = "btn_Citta";
            btn_Citta.Size = new Size(84, 25);
            btn_Citta.TabIndex = 45;
            btn_Citta.Text = "Guarnigione";
            btn_Citta.UseVisualStyleBackColor = true;
            btn_Citta.Click += btn_Citta_Click;
            // 
            // panel_Castello
            // 
            panel_Castello.BackColor = Color.Transparent;
            panel_Castello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Castello.Controls.Add(lbl_Castello);
            panel_Castello.Controls.Add(panel_Def_Castello);
            panel_Castello.Controls.Add(panel_Hp_Castello);
            panel_Castello.Controls.Add(panel_Soldier_Castello);
            panel_Castello.Controls.Add(btn_Castello);
            panel_Castello.Controls.Add(pictureBox_Castello_Salute);
            panel_Castello.Controls.Add(pictureBox_Castello_Difesa);
            panel_Castello.ForeColor = Color.Transparent;
            panel_Castello.Location = new Point(40, 144);
            panel_Castello.Name = "panel_Castello";
            panel_Castello.Size = new Size(185, 123);
            panel_Castello.TabIndex = 57;
            // 
            // lbl_Castello
            // 
            lbl_Castello.AutoSize = true;
            lbl_Castello.BackColor = Color.Transparent;
            lbl_Castello.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Castello.ForeColor = Color.LightGray;
            lbl_Castello.Location = new Point(7, 6);
            lbl_Castello.Name = "lbl_Castello";
            lbl_Castello.Size = new Size(141, 15);
            lbl_Castello.TabIndex = 16;
            lbl_Castello.Text = "Castello      [6]      Lv: [99]";
            // 
            // panel_Def_Castello
            // 
            panel_Def_Castello.BackColor = Color.Transparent;
            panel_Def_Castello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Def_Castello.ForeColor = Color.Transparent;
            panel_Def_Castello.Location = new Point(4, 48);
            panel_Def_Castello.Name = "panel_Def_Castello";
            panel_Def_Castello.Size = new Size(143, 17);
            panel_Def_Castello.TabIndex = 15;
            panel_Def_Castello.Paint += panel_Def_Castello_Paint;
            // 
            // panel_Hp_Castello
            // 
            panel_Hp_Castello.BackColor = Color.Transparent;
            panel_Hp_Castello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Hp_Castello.ForeColor = Color.Transparent;
            panel_Hp_Castello.Location = new Point(4, 25);
            panel_Hp_Castello.Name = "panel_Hp_Castello";
            panel_Hp_Castello.Size = new Size(143, 17);
            panel_Hp_Castello.TabIndex = 13;
            panel_Hp_Castello.Paint += panel_Hp_Castello_Paint;
            // 
            // panel_Soldier_Castello
            // 
            panel_Soldier_Castello.BackColor = Color.Transparent;
            panel_Soldier_Castello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Soldier_Castello.ForeColor = Color.Transparent;
            panel_Soldier_Castello.Location = new Point(4, 71);
            panel_Soldier_Castello.Name = "panel_Soldier_Castello";
            panel_Soldier_Castello.Size = new Size(143, 17);
            panel_Soldier_Castello.TabIndex = 14;
            panel_Soldier_Castello.Paint += panel_Soldier_Castello_Paint;
            // 
            // btn_Castello
            // 
            btn_Castello.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Castello.FlatAppearance.BorderSize = 0;
            btn_Castello.FlatStyle = FlatStyle.Popup;
            btn_Castello.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Castello.ForeColor = Color.Black;
            btn_Castello.Location = new Point(32, 94);
            btn_Castello.Name = "btn_Castello";
            btn_Castello.Size = new Size(84, 25);
            btn_Castello.TabIndex = 35;
            btn_Castello.Text = "Guarnigione";
            btn_Castello.UseVisualStyleBackColor = true;
            btn_Castello.Click += btn_Castello_Click;
            // 
            // pictureBox_Castello_Salute
            // 
            pictureBox_Castello_Salute.BackColor = Color.Transparent;
            pictureBox_Castello_Salute.BackgroundImage = Properties.Resources.Ripara_Strutture;
            pictureBox_Castello_Salute.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Castello_Salute.Location = new Point(153, 20);
            pictureBox_Castello_Salute.Name = "pictureBox_Castello_Salute";
            pictureBox_Castello_Salute.Size = new Size(26, 26);
            pictureBox_Castello_Salute.TabIndex = 46;
            pictureBox_Castello_Salute.TabStop = false;
            pictureBox_Castello_Salute.Visible = false;
            pictureBox_Castello_Salute.Click += pictureBox_Castello_Salute_Click;
            // 
            // pictureBox_Castello_Difesa
            // 
            pictureBox_Castello_Difesa.BackColor = Color.Transparent;
            pictureBox_Castello_Difesa.BackgroundImage = Properties.Resources.Ripara_Strutture;
            pictureBox_Castello_Difesa.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Castello_Difesa.Location = new Point(153, 48);
            pictureBox_Castello_Difesa.Name = "pictureBox_Castello_Difesa";
            pictureBox_Castello_Difesa.Size = new Size(26, 26);
            pictureBox_Castello_Difesa.TabIndex = 47;
            pictureBox_Castello_Difesa.TabStop = false;
            pictureBox_Castello_Difesa.Visible = false;
            pictureBox_Castello_Difesa.Click += pictureBox_Castello_Difesa_Click;
            // 
            // panel_Torri
            // 
            panel_Torri.BackColor = Color.Transparent;
            panel_Torri.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Torri.Controls.Add(lbl_Torri);
            panel_Torri.Controls.Add(btn_Torri);
            panel_Torri.Controls.Add(panel_Soldier_Torri);
            panel_Torri.Controls.Add(panel_Hp_Torri);
            panel_Torri.Controls.Add(panel_Def_Torri);
            panel_Torri.Controls.Add(pictureBox_Torri_Difesa);
            panel_Torri.Controls.Add(pictureBox_Torri_Salute);
            panel_Torri.ForeColor = Color.Transparent;
            panel_Torri.Location = new Point(399, 447);
            panel_Torri.Name = "panel_Torri";
            panel_Torri.Size = new Size(185, 123);
            panel_Torri.TabIndex = 58;
            // 
            // lbl_Torri
            // 
            lbl_Torri.AutoSize = true;
            lbl_Torri.BackColor = Color.Transparent;
            lbl_Torri.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Torri.ForeColor = Color.LightGray;
            lbl_Torri.Location = new Point(3, 6);
            lbl_Torri.Name = "lbl_Torri";
            lbl_Torri.Size = new Size(145, 15);
            lbl_Torri.TabIndex = 17;
            lbl_Torri.Text = "Torri      [4]             Lv: [99]";
            // 
            // btn_Torri
            // 
            btn_Torri.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Torri.FlatAppearance.BorderSize = 0;
            btn_Torri.FlatStyle = FlatStyle.Popup;
            btn_Torri.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Torri.ForeColor = Color.Black;
            btn_Torri.Location = new Point(31, 94);
            btn_Torri.Name = "btn_Torri";
            btn_Torri.Size = new Size(84, 25);
            btn_Torri.TabIndex = 36;
            btn_Torri.Text = "Guarnigione";
            btn_Torri.UseVisualStyleBackColor = true;
            btn_Torri.Click += btn_Torri_Click;
            // 
            // panel_Soldier_Torri
            // 
            panel_Soldier_Torri.BackColor = Color.Transparent;
            panel_Soldier_Torri.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Soldier_Torri.ForeColor = Color.Transparent;
            panel_Soldier_Torri.Location = new Point(3, 71);
            panel_Soldier_Torri.Name = "panel_Soldier_Torri";
            panel_Soldier_Torri.Size = new Size(143, 17);
            panel_Soldier_Torri.TabIndex = 14;
            panel_Soldier_Torri.Paint += panel_Soldier_Torri_Paint;
            // 
            // panel_Hp_Torri
            // 
            panel_Hp_Torri.BackColor = Color.Transparent;
            panel_Hp_Torri.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Hp_Torri.ForeColor = Color.Transparent;
            panel_Hp_Torri.Location = new Point(3, 25);
            panel_Hp_Torri.Name = "panel_Hp_Torri";
            panel_Hp_Torri.Size = new Size(143, 17);
            panel_Hp_Torri.TabIndex = 13;
            panel_Hp_Torri.Paint += panel_Hp_Torri_Paint;
            // 
            // panel_Def_Torri
            // 
            panel_Def_Torri.BackColor = Color.Transparent;
            panel_Def_Torri.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Def_Torri.ForeColor = Color.Transparent;
            panel_Def_Torri.Location = new Point(3, 48);
            panel_Def_Torri.Name = "panel_Def_Torri";
            panel_Def_Torri.Size = new Size(143, 17);
            panel_Def_Torri.TabIndex = 15;
            panel_Def_Torri.Paint += panel_Def_Torri_Paint;
            // 
            // pictureBox_Torri_Difesa
            // 
            pictureBox_Torri_Difesa.BackColor = Color.Transparent;
            pictureBox_Torri_Difesa.BackgroundImage = Properties.Resources.Ripara_Strutture;
            pictureBox_Torri_Difesa.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Torri_Difesa.Location = new Point(152, 48);
            pictureBox_Torri_Difesa.Name = "pictureBox_Torri_Difesa";
            pictureBox_Torri_Difesa.Size = new Size(26, 26);
            pictureBox_Torri_Difesa.TabIndex = 53;
            pictureBox_Torri_Difesa.TabStop = false;
            pictureBox_Torri_Difesa.Visible = false;
            pictureBox_Torri_Difesa.Click += pictureBox_Torri_Difesa_Click;
            // 
            // pictureBox_Torri_Salute
            // 
            pictureBox_Torri_Salute.BackColor = Color.Transparent;
            pictureBox_Torri_Salute.BackgroundImage = Properties.Resources.Ripara_Strutture;
            pictureBox_Torri_Salute.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Torri_Salute.Location = new Point(152, 20);
            pictureBox_Torri_Salute.Name = "pictureBox_Torri_Salute";
            pictureBox_Torri_Salute.Size = new Size(26, 26);
            pictureBox_Torri_Salute.TabIndex = 52;
            pictureBox_Torri_Salute.TabStop = false;
            pictureBox_Torri_Salute.Visible = false;
            pictureBox_Torri_Salute.Click += pictureBox_Torri_Salute_Click;
            // 
            // panel_Mura
            // 
            panel_Mura.BackColor = Color.Transparent;
            panel_Mura.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Mura.Controls.Add(lbl_Mura);
            panel_Mura.Controls.Add(panel_Hp_Mura);
            panel_Mura.Controls.Add(panel_Soldier_Mura);
            panel_Mura.Controls.Add(panel_Def_Mura);
            panel_Mura.Controls.Add(btn_Mura);
            panel_Mura.Controls.Add(pictureBox_Mura_Salute);
            panel_Mura.Controls.Add(pictureBox_Mura_Difesa);
            panel_Mura.ForeColor = Color.Transparent;
            panel_Mura.Location = new Point(510, 254);
            panel_Mura.Name = "panel_Mura";
            panel_Mura.Size = new Size(185, 128);
            panel_Mura.TabIndex = 57;
            // 
            // lbl_Mura
            // 
            lbl_Mura.AutoSize = true;
            lbl_Mura.BackColor = Color.Transparent;
            lbl_Mura.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Mura.ForeColor = Color.LightGray;
            lbl_Mura.Location = new Point(37, 10);
            lbl_Mura.Name = "lbl_Mura";
            lbl_Mura.Size = new Size(145, 15);
            lbl_Mura.TabIndex = 18;
            lbl_Mura.Text = "Mura      [2]            Lv: [99]";
            // 
            // panel_Hp_Mura
            // 
            panel_Hp_Mura.BackColor = Color.Transparent;
            panel_Hp_Mura.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Hp_Mura.ForeColor = Color.Transparent;
            panel_Hp_Mura.Location = new Point(37, 29);
            panel_Hp_Mura.Name = "panel_Hp_Mura";
            panel_Hp_Mura.Size = new Size(143, 17);
            panel_Hp_Mura.TabIndex = 13;
            panel_Hp_Mura.Paint += panel_Hp_Mura_Paint;
            // 
            // panel_Soldier_Mura
            // 
            panel_Soldier_Mura.BackColor = Color.Transparent;
            panel_Soldier_Mura.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Soldier_Mura.ForeColor = Color.Transparent;
            panel_Soldier_Mura.Location = new Point(37, 75);
            panel_Soldier_Mura.Name = "panel_Soldier_Mura";
            panel_Soldier_Mura.Size = new Size(143, 17);
            panel_Soldier_Mura.TabIndex = 14;
            panel_Soldier_Mura.Paint += panel_Soldier_Mura_Paint;
            // 
            // panel_Def_Mura
            // 
            panel_Def_Mura.BackColor = Color.Transparent;
            panel_Def_Mura.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Def_Mura.ForeColor = Color.Transparent;
            panel_Def_Mura.Location = new Point(37, 52);
            panel_Def_Mura.Name = "panel_Def_Mura";
            panel_Def_Mura.Size = new Size(143, 17);
            panel_Def_Mura.TabIndex = 15;
            panel_Def_Mura.Paint += panel_Def_Mura_Paint;
            // 
            // btn_Mura
            // 
            btn_Mura.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Mura.FlatAppearance.BorderSize = 0;
            btn_Mura.FlatStyle = FlatStyle.Popup;
            btn_Mura.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Mura.ForeColor = Color.Black;
            btn_Mura.Location = new Point(65, 98);
            btn_Mura.Name = "btn_Mura";
            btn_Mura.Size = new Size(84, 25);
            btn_Mura.TabIndex = 38;
            btn_Mura.Text = "Guarnigione";
            btn_Mura.UseVisualStyleBackColor = true;
            btn_Mura.Click += btn_Mura_Click;
            // 
            // pictureBox_Mura_Salute
            // 
            pictureBox_Mura_Salute.BackColor = Color.Transparent;
            pictureBox_Mura_Salute.BackgroundImage = Properties.Resources.Ripara_Strutture;
            pictureBox_Mura_Salute.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Mura_Salute.Location = new Point(5, 23);
            pictureBox_Mura_Salute.Name = "pictureBox_Mura_Salute";
            pictureBox_Mura_Salute.Size = new Size(26, 26);
            pictureBox_Mura_Salute.TabIndex = 50;
            pictureBox_Mura_Salute.TabStop = false;
            pictureBox_Mura_Salute.Visible = false;
            pictureBox_Mura_Salute.Click += pictureBox_Mura_Salute_Click;
            // 
            // pictureBox_Mura_Difesa
            // 
            pictureBox_Mura_Difesa.BackColor = Color.Transparent;
            pictureBox_Mura_Difesa.BackgroundImage = Properties.Resources.Ripara_Strutture;
            pictureBox_Mura_Difesa.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Mura_Difesa.Location = new Point(5, 51);
            pictureBox_Mura_Difesa.Name = "pictureBox_Mura_Difesa";
            pictureBox_Mura_Difesa.Size = new Size(26, 26);
            pictureBox_Mura_Difesa.TabIndex = 51;
            pictureBox_Mura_Difesa.TabStop = false;
            pictureBox_Mura_Difesa.Visible = false;
            pictureBox_Mura_Difesa.Click += pictureBox_Mura_Difesa_Click;
            // 
            // panel_Ingresso
            // 
            panel_Ingresso.BackColor = Color.Transparent;
            panel_Ingresso.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Ingresso.Controls.Add(lbl_Ingresso);
            panel_Ingresso.Controls.Add(panel_Soldier_Ingresso);
            panel_Ingresso.Controls.Add(btn_Ingresso);
            panel_Ingresso.ForeColor = Color.Transparent;
            panel_Ingresso.Location = new Point(468, 42);
            panel_Ingresso.Name = "panel_Ingresso";
            panel_Ingresso.Size = new Size(152, 74);
            panel_Ingresso.TabIndex = 55;
            // 
            // lbl_Ingresso
            // 
            lbl_Ingresso.AutoSize = true;
            lbl_Ingresso.BackColor = Color.Transparent;
            lbl_Ingresso.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Ingresso.ForeColor = Color.LightGray;
            lbl_Ingresso.Location = new Point(3, 3);
            lbl_Ingresso.Name = "lbl_Ingresso";
            lbl_Ingresso.Size = new Size(87, 15);
            lbl_Ingresso.TabIndex = 41;
            lbl_Ingresso.Text = "Ingresso      [1]";
            // 
            // panel_Soldier_Ingresso
            // 
            panel_Soldier_Ingresso.BackColor = Color.Transparent;
            panel_Soldier_Ingresso.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Soldier_Ingresso.ForeColor = Color.Transparent;
            panel_Soldier_Ingresso.Location = new Point(3, 22);
            panel_Soldier_Ingresso.Name = "panel_Soldier_Ingresso";
            panel_Soldier_Ingresso.Size = new Size(143, 17);
            panel_Soldier_Ingresso.TabIndex = 39;
            panel_Soldier_Ingresso.Paint += panel_Soldier_Ingresso_Paint;
            // 
            // btn_Ingresso
            // 
            btn_Ingresso.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Ingresso.FlatAppearance.BorderSize = 0;
            btn_Ingresso.FlatStyle = FlatStyle.Popup;
            btn_Ingresso.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Ingresso.ForeColor = Color.Black;
            btn_Ingresso.Location = new Point(32, 45);
            btn_Ingresso.Name = "btn_Ingresso";
            btn_Ingresso.Size = new Size(84, 25);
            btn_Ingresso.TabIndex = 42;
            btn_Ingresso.Text = "Guarnigione";
            btn_Ingresso.UseVisualStyleBackColor = true;
            btn_Ingresso.Click += btn_Ingresso_Click;
            // 
            // panel_Cancello
            // 
            panel_Cancello.BackColor = Color.Transparent;
            panel_Cancello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Cancello.Controls.Add(lbl_Cancello);
            panel_Cancello.Controls.Add(panel_Soldier_Cancello);
            panel_Cancello.Controls.Add(panel_Hp_Cancello);
            panel_Cancello.Controls.Add(btn_Cancello);
            panel_Cancello.Controls.Add(panel_Def_Cancello);
            panel_Cancello.Controls.Add(pictureBox_Cancello_Salute);
            panel_Cancello.Controls.Add(pictureBox_Cancello_Difesa);
            panel_Cancello.ForeColor = Color.Transparent;
            panel_Cancello.Location = new Point(301, 101);
            panel_Cancello.Name = "panel_Cancello";
            panel_Cancello.Size = new Size(185, 126);
            panel_Cancello.TabIndex = 56;
            // 
            // lbl_Cancello
            // 
            lbl_Cancello.AutoSize = true;
            lbl_Cancello.BackColor = Color.Transparent;
            lbl_Cancello.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Cancello.ForeColor = Color.LightGray;
            lbl_Cancello.Location = new Point(3, 8);
            lbl_Cancello.Name = "lbl_Cancello";
            lbl_Cancello.Size = new Size(144, 15);
            lbl_Cancello.TabIndex = 19;
            lbl_Cancello.Text = "Cancello      [3]      Lv: [99]";
            // 
            // panel_Soldier_Cancello
            // 
            panel_Soldier_Cancello.BackColor = Color.Transparent;
            panel_Soldier_Cancello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Soldier_Cancello.ForeColor = Color.Transparent;
            panel_Soldier_Cancello.Location = new Point(3, 73);
            panel_Soldier_Cancello.Name = "panel_Soldier_Cancello";
            panel_Soldier_Cancello.Size = new Size(143, 17);
            panel_Soldier_Cancello.TabIndex = 4;
            panel_Soldier_Cancello.Paint += panel_Soldier_Cancello_Paint;
            // 
            // panel_Hp_Cancello
            // 
            panel_Hp_Cancello.BackColor = Color.Transparent;
            panel_Hp_Cancello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Hp_Cancello.ForeColor = Color.Transparent;
            panel_Hp_Cancello.Location = new Point(3, 27);
            panel_Hp_Cancello.Name = "panel_Hp_Cancello";
            panel_Hp_Cancello.Size = new Size(143, 17);
            panel_Hp_Cancello.TabIndex = 3;
            panel_Hp_Cancello.Paint += panel_Hp_Cancello_Paint;
            // 
            // btn_Cancello
            // 
            btn_Cancello.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Cancello.FlatAppearance.BorderSize = 0;
            btn_Cancello.FlatStyle = FlatStyle.Popup;
            btn_Cancello.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Cancello.ForeColor = Color.Black;
            btn_Cancello.Location = new Point(31, 96);
            btn_Cancello.Name = "btn_Cancello";
            btn_Cancello.Size = new Size(84, 25);
            btn_Cancello.TabIndex = 37;
            btn_Cancello.Text = "Guarnigione";
            btn_Cancello.UseVisualStyleBackColor = true;
            btn_Cancello.Click += btn_Cancello_Click;
            // 
            // panel_Def_Cancello
            // 
            panel_Def_Cancello.BackColor = Color.Transparent;
            panel_Def_Cancello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Def_Cancello.ForeColor = Color.Transparent;
            panel_Def_Cancello.Location = new Point(3, 50);
            panel_Def_Cancello.Name = "panel_Def_Cancello";
            panel_Def_Cancello.Size = new Size(143, 17);
            panel_Def_Cancello.TabIndex = 12;
            panel_Def_Cancello.Paint += panel_Def_Cancello_Paint;
            // 
            // pictureBox_Cancello_Salute
            // 
            pictureBox_Cancello_Salute.BackColor = Color.Transparent;
            pictureBox_Cancello_Salute.BackgroundImage = Properties.Resources.Ripara_Strutture;
            pictureBox_Cancello_Salute.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Cancello_Salute.Location = new Point(152, 20);
            pictureBox_Cancello_Salute.Name = "pictureBox_Cancello_Salute";
            pictureBox_Cancello_Salute.Size = new Size(26, 26);
            pictureBox_Cancello_Salute.TabIndex = 48;
            pictureBox_Cancello_Salute.TabStop = false;
            pictureBox_Cancello_Salute.Visible = false;
            pictureBox_Cancello_Salute.Click += pictureBox_Cancello_Salute_Click;
            // 
            // pictureBox_Cancello_Difesa
            // 
            pictureBox_Cancello_Difesa.BackColor = Color.Transparent;
            pictureBox_Cancello_Difesa.BackgroundImage = Properties.Resources.Ripara_Strutture;
            pictureBox_Cancello_Difesa.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Cancello_Difesa.Location = new Point(152, 48);
            pictureBox_Cancello_Difesa.Name = "pictureBox_Cancello_Difesa";
            pictureBox_Cancello_Difesa.Size = new Size(26, 26);
            pictureBox_Cancello_Difesa.TabIndex = 49;
            pictureBox_Cancello_Difesa.TabStop = false;
            pictureBox_Cancello_Difesa.Visible = false;
            pictureBox_Cancello_Difesa.Click += pictureBox_Cancello_Difesa_Click;
            // 
            // btn_Ripara_Tutto
            // 
            btn_Ripara_Tutto.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Ripara_Tutto.FlatAppearance.BorderSize = 0;
            btn_Ripara_Tutto.FlatStyle = FlatStyle.Popup;
            btn_Ripara_Tutto.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Ripara_Tutto.ForeColor = Color.Black;
            btn_Ripara_Tutto.Location = new Point(632, 3);
            btn_Ripara_Tutto.Name = "btn_Ripara_Tutto";
            btn_Ripara_Tutto.Size = new Size(84, 25);
            btn_Ripara_Tutto.TabIndex = 54;
            btn_Ripara_Tutto.Text = "Ripara Tutto";
            btn_Ripara_Tutto.UseVisualStyleBackColor = true;
            btn_Ripara_Tutto.Visible = false;
            btn_Ripara_Tutto.Click += btn_Ripara_Tutto_Click;
            // 
            // Citta_V2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(719, 675);
            Controls.Add(panel_Sfondo);
            Name = "Citta_V2";
            Text = "Citta";
            FormClosing += Citta_V2_FormClosing;
            Load += Citta_Load;
            panel_Sfondo.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel_Centro.ResumeLayout(false);
            panel_Centro.PerformLayout();
            panel_Castello.ResumeLayout(false);
            panel_Castello.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Castello_Salute).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Castello_Difesa).EndInit();
            panel_Torri.ResumeLayout(false);
            panel_Torri.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Torri_Difesa).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Torri_Salute).EndInit();
            panel_Mura.ResumeLayout(false);
            panel_Mura.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Mura_Salute).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Mura_Difesa).EndInit();
            panel_Ingresso.ResumeLayout(false);
            panel_Ingresso.PerformLayout();
            panel_Cancello.ResumeLayout(false);
            panel_Cancello.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Cancello_Salute).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Cancello_Difesa).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel_Sfondo;
        private Button btn_Cancello;
        private Panel panel1;
        private Button btn_Castello;
        private Button btn_Mura;
        private Label lbl_Castello;
        private Panel panel_Soldier_Castello;
        private Label lbl_Cancello;
        private Panel panel_Hp_Castello;
        private Label lbl_Mura;
        private Panel panel_Def_Castello;
        private Panel panel_Soldier_Mura;
        private Panel panel_Soldier_Cancello;
        private Panel panel_Hp_Mura;
        private Button btn_Torri;
        private Panel panel_Def_Mura;
        private Panel panel_Hp_Cancello;
        private Panel panel_Def_Cancello;
        private Label lbl_Torri;
        private Panel panel_Soldier_Torri;
        private Panel panel_Hp_Torri;
        private Panel panel_Def_Torri;
        private Label lbl_Ingresso;
        private Button btn_Ingresso;
        private Panel panel_Soldier_Ingresso;
        private Label lbl_Centro;
        private Button btn_Citta;
        private Panel panel_Soldier_Citta;
        private Panel panel2;
        private TextBox txt_Testo;
        private PictureBox pictureBox_Castello_Salute;
        private PictureBox pictureBox_Castello_Difesa;
        private PictureBox pictureBox_Cancello_Difesa;
        private PictureBox pictureBox_Cancello_Salute;
        private PictureBox pictureBox_Torri_Difesa;
        private PictureBox pictureBox_Torri_Salute;
        private PictureBox pictureBox_Mura_Difesa;
        private PictureBox pictureBox_Mura_Salute;
        private Button btn_Ripara_Tutto;
        private Panel panel_Mura;
        private Panel panel_Ingresso;
        private Panel panel_Cancello;
        private Panel panel_Centro;
        private Panel panel_Castello;
        private Panel panel_Torri;
    }
}