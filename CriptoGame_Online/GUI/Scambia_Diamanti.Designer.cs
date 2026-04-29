namespace Warrior_and_Wealth.GUI
{
    partial class Scambia_Diamanti
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Scambia_Diamanti));
            txt_Diamond_Viola = new TextBox();
            ico_12 = new Warrior_and_Wealth.Strumenti.DoubleBufferedPanel();
            txt_Diamond_Blu = new TextBox();
            ico_11 = new Warrior_and_Wealth.Strumenti.DoubleBufferedPanel();
            btn_Scambia = new Button();
            txt_Testo = new TextBox();
            pictureBox_Più = new PictureBox();
            pictureBox_Meno = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Più).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Meno).BeginInit();
            SuspendLayout();
            // 
            // txt_Diamond_Viola
            // 
            txt_Diamond_Viola.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txt_Diamond_Viola.Location = new Point(42, 86);
            txt_Diamond_Viola.Margin = new Padding(2, 3, 2, 3);
            txt_Diamond_Viola.Name = "txt_Diamond_Viola";
            txt_Diamond_Viola.ReadOnly = true;
            txt_Diamond_Viola.Size = new Size(73, 25);
            txt_Diamond_Viola.TabIndex = 15;
            txt_Diamond_Viola.Text = "0";
            txt_Diamond_Viola.TextAlign = HorizontalAlignment.Center;
            // 
            // ico_12
            // 
            ico_12.BackgroundImage = Properties.Resources.DiamanteViola_V2;
            ico_12.BackgroundImageLayout = ImageLayout.Stretch;
            ico_12.Location = new Point(11, 83);
            ico_12.Margin = new Padding(2, 3, 2, 3);
            ico_12.Name = "ico_12";
            ico_12.Size = new Size(27, 32);
            ico_12.TabIndex = 16;
            // 
            // txt_Diamond_Blu
            // 
            txt_Diamond_Blu.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txt_Diamond_Blu.Location = new Point(151, 86);
            txt_Diamond_Blu.Margin = new Padding(2, 3, 2, 3);
            txt_Diamond_Blu.Name = "txt_Diamond_Blu";
            txt_Diamond_Blu.ReadOnly = true;
            txt_Diamond_Blu.Size = new Size(70, 25);
            txt_Diamond_Blu.TabIndex = 13;
            txt_Diamond_Blu.Text = "0";
            txt_Diamond_Blu.TextAlign = HorizontalAlignment.Center;
            // 
            // ico_11
            // 
            ico_11.BackgroundImage = Properties.Resources.DiamanteBlu_V2;
            ico_11.BackgroundImageLayout = ImageLayout.Stretch;
            ico_11.Location = new Point(120, 83);
            ico_11.Margin = new Padding(2, 3, 2, 3);
            ico_11.Name = "ico_11";
            ico_11.Size = new Size(27, 32);
            ico_11.TabIndex = 14;
            // 
            // btn_Scambia
            // 
            btn_Scambia.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Scambia.FlatAppearance.BorderSize = 0;
            btn_Scambia.FlatStyle = FlatStyle.Popup;
            btn_Scambia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Scambia.Location = new Point(78, 122);
            btn_Scambia.Margin = new Padding(2, 3, 2, 3);
            btn_Scambia.Name = "btn_Scambia";
            btn_Scambia.Size = new Size(78, 34);
            btn_Scambia.TabIndex = 18;
            btn_Scambia.Text = "Scambia";
            btn_Scambia.UseVisualStyleBackColor = true;
            btn_Scambia.Click += btn_Scambia_Click;
            // 
            // txt_Testo
            // 
            txt_Testo.BorderStyle = BorderStyle.None;
            txt_Testo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txt_Testo.Location = new Point(2, 6);
            txt_Testo.Margin = new Padding(2, 3, 2, 3);
            txt_Testo.Multiline = true;
            txt_Testo.Name = "txt_Testo";
            txt_Testo.ReadOnly = true;
            txt_Testo.Size = new Size(226, 47);
            txt_Testo.TabIndex = 33;
            txt_Testo.Text = "Descrizione\r\n1 Diamante Viola = 3 Diamanti Blu";
            txt_Testo.TextAlign = HorizontalAlignment.Center;
            // 
            // pictureBox_Più
            // 
            pictureBox_Più.BackColor = Color.Transparent;
            pictureBox_Più.BackgroundImage = (Image)resources.GetObject("pictureBox_Più.BackgroundImage");
            pictureBox_Più.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Più.Location = new Point(96, 58);
            pictureBox_Più.Margin = new Padding(2, 3, 2, 3);
            pictureBox_Più.Name = "pictureBox_Più";
            pictureBox_Più.Size = new Size(18, 22);
            pictureBox_Più.TabIndex = 40;
            pictureBox_Più.TabStop = false;
            pictureBox_Più.Click += pictureBox_Più_Click;
            // 
            // pictureBox_Meno
            // 
            pictureBox_Meno.BackColor = Color.Transparent;
            pictureBox_Meno.BackgroundImage = Properties.Resources.minus_64x;
            pictureBox_Meno.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Meno.Location = new Point(42, 58);
            pictureBox_Meno.Margin = new Padding(2, 3, 2, 3);
            pictureBox_Meno.Name = "pictureBox_Meno";
            pictureBox_Meno.Size = new Size(18, 22);
            pictureBox_Meno.TabIndex = 41;
            pictureBox_Meno.TabStop = false;
            pictureBox_Meno.Click += pictureBox_Meno_Click;
            // 
            // Scambia_Diamanti
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackgroundImage = Properties.Resources.freepik__upload__73441_AAA;
            ClientSize = new Size(232, 166);
            Controls.Add(pictureBox_Meno);
            Controls.Add(pictureBox_Più);
            Controls.Add(txt_Testo);
            Controls.Add(btn_Scambia);
            Controls.Add(txt_Diamond_Viola);
            Controls.Add(ico_12);
            Controls.Add(txt_Diamond_Blu);
            Controls.Add(ico_11);
            Margin = new Padding(2, 3, 2, 3);
            Name = "Scambia_Diamanti";
            Text = "Scambia_Diamanti";
            Load += Scambia_Diamanti_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox_Più).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Meno).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txt_Diamond_Viola;
        private Strumenti.DoubleBufferedPanel ico_12;
        private TextBox txt_Diamond_Blu;
        private Strumenti.DoubleBufferedPanel ico_11;
        private Button btn_Scambia;
        private TextBox txt_Testo;
        private PictureBox pictureBox_Più;
        private PictureBox pictureBox_Meno;
    }
}