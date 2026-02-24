namespace Warrior_and_Wealth
{
    partial class Login
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
            lbl_Username_Login = new Label();
            panel1 = new Panel();
            lbl_Versione_Attuale = new Label();
            txt_Log = new TextBox();
            txt_Ip = new TextBox();
            label1 = new Label();
            lbl_Titolo = new Label();
            Btn_Login = new Button();
            Btn_New_Game = new Button();
            txt_Password_Login = new TextBox();
            lbl_Password_Login = new Label();
            txt_Username_Login = new TextBox();
            lbl_Aggiornamento_Disponibile = new Label();
            banner_2 = new Panel();
            banner_1 = new Panel();
            btn_Aggiorna = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lbl_Username_Login
            // 
            lbl_Username_Login.AutoSize = true;
            lbl_Username_Login.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lbl_Username_Login.Location = new Point(66, 73);
            lbl_Username_Login.Name = "lbl_Username_Login";
            lbl_Username_Login.Size = new Size(59, 13);
            lbl_Username_Login.TabIndex = 1;
            lbl_Username_Login.Text = "Username";
            // 
            // panel1
            // 
            panel1.Controls.Add(lbl_Versione_Attuale);
            panel1.Controls.Add(txt_Log);
            panel1.Controls.Add(txt_Ip);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(lbl_Titolo);
            panel1.Controls.Add(Btn_Login);
            panel1.Controls.Add(Btn_New_Game);
            panel1.Controls.Add(txt_Password_Login);
            panel1.Controls.Add(lbl_Password_Login);
            panel1.Controls.Add(txt_Username_Login);
            panel1.Controls.Add(lbl_Username_Login);
            panel1.Location = new Point(20, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(193, 276);
            panel1.TabIndex = 1;
            // 
            // lbl_Versione_Attuale
            // 
            lbl_Versione_Attuale.AutoSize = true;
            lbl_Versione_Attuale.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Versione_Attuale.Location = new Point(38, 24);
            lbl_Versione_Attuale.Name = "lbl_Versione_Attuale";
            lbl_Versione_Attuale.Size = new Size(121, 13);
            lbl_Versione_Attuale.TabIndex = 10;
            lbl_Versione_Attuale.Text = "Versione Attuale: 0.1.0";
            lbl_Versione_Attuale.Visible = false;
            // 
            // txt_Log
            // 
            txt_Log.BorderStyle = BorderStyle.None;
            txt_Log.Location = new Point(3, 193);
            txt_Log.Multiline = true;
            txt_Log.Name = "txt_Log";
            txt_Log.ReadOnly = true;
            txt_Log.Size = new Size(187, 38);
            txt_Log.TabIndex = 9;
            // 
            // txt_Ip
            // 
            txt_Ip.BorderStyle = BorderStyle.None;
            txt_Ip.Location = new Point(22, 171);
            txt_Ip.Name = "txt_Ip";
            txt_Ip.Size = new Size(152, 16);
            txt_Ip.TabIndex = 7;
            txt_Ip.Text = "AUTO";
            txt_Ip.TextAlign = HorizontalAlignment.Center;
            txt_Ip.MouseClick += txt_Ip_MouseClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            label1.Location = new Point(85, 155);
            label1.Name = "label1";
            label1.Size = new Size(17, 13);
            label1.TabIndex = 8;
            label1.Text = "IP";
            // 
            // lbl_Titolo
            // 
            lbl_Titolo.AutoSize = true;
            lbl_Titolo.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Titolo.Location = new Point(28, 0);
            lbl_Titolo.Name = "lbl_Titolo";
            lbl_Titolo.Size = new Size(93, 13);
            lbl_Titolo.TabIndex = 6;
            lbl_Titolo.Text = "Warrior - Wealth";
            // 
            // Btn_Login
            // 
            Btn_Login.FlatAppearance.BorderSize = 0;
            Btn_Login.FlatStyle = FlatStyle.Flat;
            Btn_Login.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Btn_Login.Location = new Point(98, 236);
            Btn_Login.Name = "Btn_Login";
            Btn_Login.Size = new Size(92, 32);
            Btn_Login.TabIndex = 5;
            Btn_Login.Text = "Login";
            Btn_Login.UseVisualStyleBackColor = true;
            Btn_Login.Click += Btn_Login_Click;
            // 
            // Btn_New_Game
            // 
            Btn_New_Game.FlatAppearance.BorderSize = 0;
            Btn_New_Game.FlatStyle = FlatStyle.Flat;
            Btn_New_Game.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Btn_New_Game.Location = new Point(3, 236);
            Btn_New_Game.Name = "Btn_New_Game";
            Btn_New_Game.Size = new Size(89, 32);
            Btn_New_Game.TabIndex = 4;
            Btn_New_Game.Text = "New Game";
            Btn_New_Game.UseVisualStyleBackColor = true;
            Btn_New_Game.Click += Btn_New_Game_Click;
            // 
            // txt_Password_Login
            // 
            txt_Password_Login.BorderStyle = BorderStyle.None;
            txt_Password_Login.Location = new Point(22, 131);
            txt_Password_Login.Name = "txt_Password_Login";
            txt_Password_Login.Size = new Size(152, 16);
            txt_Password_Login.TabIndex = 2;
            txt_Password_Login.TextAlign = HorizontalAlignment.Center;
            txt_Password_Login.MouseClick += txt_Password_Login_MouseClick;
            txt_Password_Login.TextChanged += txt_Password_Login_TextChanged;
            // 
            // lbl_Password_Login
            // 
            lbl_Password_Login.AutoSize = true;
            lbl_Password_Login.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            lbl_Password_Login.Location = new Point(68, 115);
            lbl_Password_Login.Name = "lbl_Password_Login";
            lbl_Password_Login.Size = new Size(57, 13);
            lbl_Password_Login.TabIndex = 3;
            lbl_Password_Login.Text = "Password";
            // 
            // txt_Username_Login
            // 
            txt_Username_Login.BorderStyle = BorderStyle.None;
            txt_Username_Login.Location = new Point(21, 89);
            txt_Username_Login.Name = "txt_Username_Login";
            txt_Username_Login.Size = new Size(152, 16);
            txt_Username_Login.TabIndex = 0;
            txt_Username_Login.TextAlign = HorizontalAlignment.Center;
            txt_Username_Login.MouseClick += txt_Username_Login_MouseClick;
            txt_Username_Login.TextChanged += txt_Username_Login_TextChanged;
            // 
            // lbl_Aggiornamento_Disponibile
            // 
            lbl_Aggiornamento_Disponibile.AutoSize = true;
            lbl_Aggiornamento_Disponibile.BackColor = Color.Transparent;
            lbl_Aggiornamento_Disponibile.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Aggiornamento_Disponibile.Location = new Point(32, 291);
            lbl_Aggiornamento_Disponibile.Name = "lbl_Aggiornamento_Disponibile";
            lbl_Aggiornamento_Disponibile.Size = new Size(178, 13);
            lbl_Aggiornamento_Disponibile.TabIndex = 11;
            lbl_Aggiornamento_Disponibile.Text = "Necessario aggiornamento : 0.1.1";
            lbl_Aggiornamento_Disponibile.Visible = false;
            // 
            // banner_2
            // 
            banner_2.BackgroundImage = Properties.Resources.Banner_Blue_removebg_preview;
            banner_2.BackgroundImageLayout = ImageLayout.Stretch;
            banner_2.Location = new Point(189, 2);
            banner_2.Name = "banner_2";
            banner_2.Size = new Size(33, 55);
            banner_2.TabIndex = 17;
            // 
            // banner_1
            // 
            banner_1.BackgroundImage = Properties.Resources.Banner_Red_removebg_preview;
            banner_1.BackgroundImageLayout = ImageLayout.Stretch;
            banner_1.Location = new Point(12, 2);
            banner_1.Name = "banner_1";
            banner_1.Size = new Size(33, 55);
            banner_1.TabIndex = 16;
            // 
            // btn_Aggiorna
            // 
            btn_Aggiorna.FlatAppearance.BorderSize = 0;
            btn_Aggiorna.FlatStyle = FlatStyle.Flat;
            btn_Aggiorna.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Aggiorna.Location = new Point(61, 311);
            btn_Aggiorna.Name = "btn_Aggiorna";
            btn_Aggiorna.Size = new Size(114, 27);
            btn_Aggiorna.TabIndex = 18;
            btn_Aggiorna.Text = "Aggiorna Client";
            btn_Aggiorna.UseVisualStyleBackColor = true;
            btn_Aggiorna.Visible = false;
            btn_Aggiorna.Click += btn_Aggiorna_Click;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.freepik__upload__73441_AAA;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(235, 342);
            Controls.Add(lbl_Aggiornamento_Disponibile);
            Controls.Add(btn_Aggiorna);
            Controls.Add(panel1);
            Controls.Add(banner_1);
            Controls.Add(banner_2);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Login";
            Text = "Login";
            FormClosing += Login_FormClosing;
            Load += Gioco_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbl_Username_Login;
        private Panel panel1;
        private TextBox txt_Username_Login;
        private Button Btn_New_Game;
        private TextBox txt_Password_Login;
        private Label lbl_Password_Login;
        private Button Btn_Login;
        private Label lbl_Titolo;
        private Panel banner_2;
        private Panel banner_1;
        private TextBox txt_Ip;
        private Label label1;
        private TextBox txt_Log;
        private Label lbl_Versione_Attuale;
        private Label lbl_Aggiornamento_Disponibile;
        private Button btn_Aggiorna;
    }
}