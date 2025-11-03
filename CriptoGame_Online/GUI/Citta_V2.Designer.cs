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
            label = new Label();
            btn_Citta = new Button();
            panel_Soldier_Citta = new Panel();
            label2 = new Label();
            label5 = new Label();
            btn_Castello = new Button();
            btn_Ingresso = new Button();
            panel_Def_Torri = new Panel();
            panel_Soldier_Ingresso = new Panel();
            btn_Mura = new Button();
            panel_Hp_Torri = new Panel();
            label1 = new Label();
            panel_Soldier_Torri = new Panel();
            panel_Soldier_Castello = new Panel();
            panel_Def_Cancello = new Panel();
            label4 = new Label();
            btn_Cancello = new Button();
            panel_Hp_Castello = new Panel();
            panel_Hp_Cancello = new Panel();
            label3 = new Label();
            panel_Def_Mura = new Panel();
            panel_Def_Castello = new Panel();
            btn_Torri = new Button();
            panel_Soldier_Mura = new Panel();
            panel_Hp_Mura = new Panel();
            panel_Soldier_Cancello = new Panel();
            panel_Sfondo.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
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
            txt_Testo.Location = new Point(12, 13);
            txt_Testo.Multiline = true;
            txt_Testo.Name = "txt_Testo";
            txt_Testo.ReadOnly = true;
            txt_Testo.Size = new Size(687, 49);
            txt_Testo.TabIndex = 33;
            txt_Testo.Text = "Acquista il tuo terreno virtuale e diventa proprietario di una porzione di terra. Ogni terreno genera una rendita giornaliera automatica. L’ammontare della rendita dipende dalla rarità.";
            txt_Testo.TextAlign = HorizontalAlignment.Center;
            // 
            // panel1
            // 
            panel1.BackgroundImage = (Image)resources.GetObject("panel1.BackgroundImage");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.Controls.Add(label);
            panel1.Controls.Add(btn_Citta);
            panel1.Controls.Add(panel_Soldier_Citta);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(btn_Castello);
            panel1.Controls.Add(btn_Ingresso);
            panel1.Controls.Add(panel_Def_Torri);
            panel1.Controls.Add(panel_Soldier_Ingresso);
            panel1.Controls.Add(btn_Mura);
            panel1.Controls.Add(panel_Hp_Torri);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(panel_Soldier_Torri);
            panel1.Controls.Add(panel_Soldier_Castello);
            panel1.Controls.Add(panel_Def_Cancello);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(btn_Cancello);
            panel1.Controls.Add(panel_Hp_Castello);
            panel1.Controls.Add(panel_Hp_Cancello);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(panel_Def_Mura);
            panel1.Controls.Add(panel_Def_Castello);
            panel1.Controls.Add(btn_Torri);
            panel1.Controls.Add(panel_Soldier_Mura);
            panel1.Controls.Add(panel_Hp_Mura);
            panel1.Controls.Add(panel_Soldier_Cancello);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 75);
            panel1.Name = "panel1";
            panel1.Size = new Size(719, 600);
            panel1.TabIndex = 39;
            // 
            // label
            // 
            label.AutoSize = true;
            label.BackColor = Color.Transparent;
            label.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label.ForeColor = Color.LightGray;
            label.Location = new Point(243, 328);
            label.Name = "label";
            label.Size = new Size(66, 15);
            label.TabIndex = 44;
            label.Text = "Città      [5]";
            // 
            // btn_Citta
            // 
            btn_Citta.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Citta.FlatAppearance.BorderSize = 0;
            btn_Citta.FlatStyle = FlatStyle.Popup;
            btn_Citta.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Citta.ForeColor = Color.Black;
            btn_Citta.Location = new Point(274, 366);
            btn_Citta.Name = "btn_Citta";
            btn_Citta.Size = new Size(65, 25);
            btn_Citta.TabIndex = 45;
            btn_Citta.Text = "Aggiungi";
            btn_Citta.UseVisualStyleBackColor = true;
            btn_Citta.Click += btn_Citta_Click;
            // 
            // panel_Soldier_Citta
            // 
            panel_Soldier_Citta.BackColor = Color.Transparent;
            panel_Soldier_Citta.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Soldier_Citta.ForeColor = Color.Transparent;
            panel_Soldier_Citta.Location = new Point(240, 346);
            panel_Soldier_Citta.Name = "panel_Soldier_Citta";
            panel_Soldier_Citta.Size = new Size(143, 17);
            panel_Soldier_Citta.TabIndex = 43;
            panel_Soldier_Citta.Paint += panel_Soldier_Citta_Paint;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.LightGray;
            label2.Location = new Point(398, 448);
            label2.Name = "label2";
            label2.Size = new Size(66, 15);
            label2.TabIndex = 17;
            label2.Text = "Torri      [4]";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.LightGray;
            label5.Location = new Point(493, 39);
            label5.Name = "label5";
            label5.Size = new Size(87, 15);
            label5.TabIndex = 41;
            label5.Text = "Ingresso      [1]";
            // 
            // btn_Castello
            // 
            btn_Castello.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Castello.FlatAppearance.BorderSize = 0;
            btn_Castello.FlatStyle = FlatStyle.Popup;
            btn_Castello.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Castello.ForeColor = Color.Black;
            btn_Castello.Location = new Point(53, 236);
            btn_Castello.Name = "btn_Castello";
            btn_Castello.Size = new Size(65, 25);
            btn_Castello.TabIndex = 35;
            btn_Castello.Text = "Aggiungi";
            btn_Castello.UseVisualStyleBackColor = true;
            btn_Castello.Click += btn_Castello_Click;
            // 
            // btn_Ingresso
            // 
            btn_Ingresso.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Ingresso.FlatAppearance.BorderSize = 0;
            btn_Ingresso.FlatStyle = FlatStyle.Popup;
            btn_Ingresso.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Ingresso.ForeColor = Color.Black;
            btn_Ingresso.Location = new Point(527, 77);
            btn_Ingresso.Name = "btn_Ingresso";
            btn_Ingresso.Size = new Size(65, 25);
            btn_Ingresso.TabIndex = 42;
            btn_Ingresso.Text = "Aggiungi";
            btn_Ingresso.UseVisualStyleBackColor = true;
            btn_Ingresso.Click += btn_Ingresso_Click;
            // 
            // panel_Def_Torri
            // 
            panel_Def_Torri.BackColor = Color.Transparent;
            panel_Def_Torri.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Def_Torri.ForeColor = Color.Transparent;
            panel_Def_Torri.Location = new Point(398, 489);
            panel_Def_Torri.Name = "panel_Def_Torri";
            panel_Def_Torri.Size = new Size(143, 17);
            panel_Def_Torri.TabIndex = 15;
            panel_Def_Torri.Paint += panel_Def_Torri_Paint;
            // 
            // panel_Soldier_Ingresso
            // 
            panel_Soldier_Ingresso.BackColor = Color.Transparent;
            panel_Soldier_Ingresso.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Soldier_Ingresso.ForeColor = Color.Transparent;
            panel_Soldier_Ingresso.Location = new Point(493, 57);
            panel_Soldier_Ingresso.Name = "panel_Soldier_Ingresso";
            panel_Soldier_Ingresso.Size = new Size(143, 17);
            panel_Soldier_Ingresso.TabIndex = 39;
            panel_Soldier_Ingresso.Paint += panel_Soldier_Ingresso_Paint;
            // 
            // btn_Mura
            // 
            btn_Mura.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Mura.FlatAppearance.BorderSize = 0;
            btn_Mura.FlatStyle = FlatStyle.Popup;
            btn_Mura.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Mura.ForeColor = Color.Black;
            btn_Mura.Location = new Point(592, 349);
            btn_Mura.Name = "btn_Mura";
            btn_Mura.Size = new Size(65, 25);
            btn_Mura.TabIndex = 38;
            btn_Mura.Text = "Aggiungi";
            btn_Mura.UseVisualStyleBackColor = true;
            btn_Mura.Click += btn_Mura_Click;
            // 
            // panel_Hp_Torri
            // 
            panel_Hp_Torri.BackColor = Color.Transparent;
            panel_Hp_Torri.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Hp_Torri.ForeColor = Color.Transparent;
            panel_Hp_Torri.Location = new Point(398, 466);
            panel_Hp_Torri.Name = "panel_Hp_Torri";
            panel_Hp_Torri.Size = new Size(143, 17);
            panel_Hp_Torri.TabIndex = 13;
            panel_Hp_Torri.Paint += panel_Hp_Torri_Paint;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.LightGray;
            label1.Location = new Point(45, 149);
            label1.Name = "label1";
            label1.Size = new Size(83, 15);
            label1.TabIndex = 16;
            label1.Text = "Castello      [6]";
            // 
            // panel_Soldier_Torri
            // 
            panel_Soldier_Torri.BackColor = Color.Transparent;
            panel_Soldier_Torri.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Soldier_Torri.ForeColor = Color.Transparent;
            panel_Soldier_Torri.Location = new Point(398, 512);
            panel_Soldier_Torri.Name = "panel_Soldier_Torri";
            panel_Soldier_Torri.Size = new Size(143, 17);
            panel_Soldier_Torri.TabIndex = 14;
            panel_Soldier_Torri.Paint += panel_Soldier_Torri_Paint;
            // 
            // panel_Soldier_Castello
            // 
            panel_Soldier_Castello.BackColor = Color.Transparent;
            panel_Soldier_Castello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Soldier_Castello.ForeColor = Color.Transparent;
            panel_Soldier_Castello.Location = new Point(42, 213);
            panel_Soldier_Castello.Name = "panel_Soldier_Castello";
            panel_Soldier_Castello.Size = new Size(143, 17);
            panel_Soldier_Castello.TabIndex = 14;
            panel_Soldier_Castello.Paint += panel_Soldier_Castello_Paint;
            // 
            // panel_Def_Cancello
            // 
            panel_Def_Cancello.BackColor = Color.Transparent;
            panel_Def_Cancello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Def_Cancello.ForeColor = Color.Transparent;
            panel_Def_Cancello.Location = new Point(308, 154);
            panel_Def_Cancello.Name = "panel_Def_Cancello";
            panel_Def_Cancello.Size = new Size(143, 17);
            panel_Def_Cancello.TabIndex = 12;
            panel_Def_Cancello.Paint += panel_Def_Cancello_Paint;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.LightGray;
            label4.Location = new Point(308, 109);
            label4.Name = "label4";
            label4.Size = new Size(86, 15);
            label4.TabIndex = 19;
            label4.Text = "Cancello      [3]";
            // 
            // btn_Cancello
            // 
            btn_Cancello.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Cancello.FlatAppearance.BorderSize = 0;
            btn_Cancello.FlatStyle = FlatStyle.Popup;
            btn_Cancello.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Cancello.ForeColor = Color.Black;
            btn_Cancello.Location = new Point(342, 197);
            btn_Cancello.Name = "btn_Cancello";
            btn_Cancello.Size = new Size(65, 25);
            btn_Cancello.TabIndex = 37;
            btn_Cancello.Text = "Aggiungi";
            btn_Cancello.UseVisualStyleBackColor = true;
            btn_Cancello.Click += btn_Cancello_Click;
            // 
            // panel_Hp_Castello
            // 
            panel_Hp_Castello.BackColor = Color.Transparent;
            panel_Hp_Castello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Hp_Castello.ForeColor = Color.Transparent;
            panel_Hp_Castello.Location = new Point(42, 167);
            panel_Hp_Castello.Name = "panel_Hp_Castello";
            panel_Hp_Castello.Size = new Size(143, 17);
            panel_Hp_Castello.TabIndex = 13;
            panel_Hp_Castello.Paint += panel_Hp_Castello_Paint;
            // 
            // panel_Hp_Cancello
            // 
            panel_Hp_Cancello.BackColor = Color.Transparent;
            panel_Hp_Cancello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Hp_Cancello.ForeColor = Color.Transparent;
            panel_Hp_Cancello.Location = new Point(308, 131);
            panel_Hp_Cancello.Name = "panel_Hp_Cancello";
            panel_Hp_Cancello.Size = new Size(143, 17);
            panel_Hp_Cancello.TabIndex = 3;
            panel_Hp_Cancello.Paint += panel_Hp_Cancello_Paint;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.LightGray;
            label3.Location = new Point(556, 257);
            label3.Name = "label3";
            label3.Size = new Size(69, 15);
            label3.TabIndex = 18;
            label3.Text = "Mura      [2]";
            // 
            // panel_Def_Mura
            // 
            panel_Def_Mura.BackColor = Color.Transparent;
            panel_Def_Mura.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Def_Mura.ForeColor = Color.Transparent;
            panel_Def_Mura.Location = new Point(556, 303);
            panel_Def_Mura.Name = "panel_Def_Mura";
            panel_Def_Mura.Size = new Size(143, 17);
            panel_Def_Mura.TabIndex = 15;
            panel_Def_Mura.Paint += panel_Def_Mura_Paint;
            // 
            // panel_Def_Castello
            // 
            panel_Def_Castello.BackColor = Color.Transparent;
            panel_Def_Castello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Def_Castello.ForeColor = Color.Transparent;
            panel_Def_Castello.Location = new Point(42, 190);
            panel_Def_Castello.Name = "panel_Def_Castello";
            panel_Def_Castello.Size = new Size(143, 17);
            panel_Def_Castello.TabIndex = 15;
            panel_Def_Castello.Paint += panel_Def_Castello_Paint;
            // 
            // btn_Torri
            // 
            btn_Torri.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Torri.FlatAppearance.BorderSize = 0;
            btn_Torri.FlatStyle = FlatStyle.Popup;
            btn_Torri.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Torri.ForeColor = Color.Black;
            btn_Torri.Location = new Point(429, 535);
            btn_Torri.Name = "btn_Torri";
            btn_Torri.Size = new Size(65, 25);
            btn_Torri.TabIndex = 36;
            btn_Torri.Text = "Aggiungi";
            btn_Torri.UseVisualStyleBackColor = true;
            btn_Torri.Click += btn_Torri_Click;
            // 
            // panel_Soldier_Mura
            // 
            panel_Soldier_Mura.BackColor = Color.Transparent;
            panel_Soldier_Mura.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Soldier_Mura.ForeColor = Color.Transparent;
            panel_Soldier_Mura.Location = new Point(556, 326);
            panel_Soldier_Mura.Name = "panel_Soldier_Mura";
            panel_Soldier_Mura.Size = new Size(143, 17);
            panel_Soldier_Mura.TabIndex = 14;
            panel_Soldier_Mura.Paint += panel_Soldier_Mura_Paint;
            // 
            // panel_Hp_Mura
            // 
            panel_Hp_Mura.BackColor = Color.Transparent;
            panel_Hp_Mura.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Hp_Mura.ForeColor = Color.Transparent;
            panel_Hp_Mura.Location = new Point(556, 280);
            panel_Hp_Mura.Name = "panel_Hp_Mura";
            panel_Hp_Mura.Size = new Size(143, 17);
            panel_Hp_Mura.TabIndex = 13;
            panel_Hp_Mura.Paint += panel_Hp_Mura_Paint;
            // 
            // panel_Soldier_Cancello
            // 
            panel_Soldier_Cancello.BackColor = Color.Transparent;
            panel_Soldier_Cancello.BackgroundImageLayout = ImageLayout.Stretch;
            panel_Soldier_Cancello.ForeColor = Color.Transparent;
            panel_Soldier_Cancello.Location = new Point(308, 177);
            panel_Soldier_Cancello.Name = "panel_Soldier_Cancello";
            panel_Soldier_Cancello.Size = new Size(143, 17);
            panel_Soldier_Cancello.TabIndex = 4;
            panel_Soldier_Cancello.Paint += panel_Soldier_Cancello_Paint;
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
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel_Sfondo;
        private Button btn_Cancello;
        private Panel panel1;
        private Button btn_Castello;
        private Button btn_Mura;
        private Label label1;
        private Panel panel_Soldier_Castello;
        private Label label4;
        private Panel panel_Hp_Castello;
        private Label label3;
        private Panel panel_Def_Castello;
        private Panel panel_Soldier_Mura;
        private Panel panel_Soldier_Cancello;
        private Panel panel_Hp_Mura;
        private Button btn_Torri;
        private Panel panel_Def_Mura;
        private Panel panel_Hp_Cancello;
        private Panel panel_Def_Cancello;
        private Label label2;
        private Panel panel_Soldier_Torri;
        private Panel panel_Hp_Torri;
        private Panel panel_Def_Torri;
        private Label label5;
        private Button btn_Ingresso;
        private Panel panel_Soldier_Ingresso;
        private Label label;
        private Button btn_Citta;
        private Panel panel_Soldier_Citta;
        private Panel panel2;
        private TextBox txt_Testo;
    }
}