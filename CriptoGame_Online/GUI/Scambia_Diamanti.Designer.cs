namespace CriptoGame_Online.GUI
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
            ico_12 = new CriptoGame_Online.Strumenti.DoubleBufferedPanel();
            txt_Diamond_Blu = new TextBox();
            ico_11 = new CriptoGame_Online.Strumenti.DoubleBufferedPanel();
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
            txt_Diamond_Viola.Location = new Point(46, 81);
            txt_Diamond_Viola.Name = "txt_Diamond_Viola";
            txt_Diamond_Viola.ReadOnly = true;
            txt_Diamond_Viola.Size = new Size(79, 25);
            txt_Diamond_Viola.TabIndex = 15;
            txt_Diamond_Viola.Text = "0";
            txt_Diamond_Viola.TextAlign = HorizontalAlignment.Center;
            // 
            // ico_12
            // 
            ico_12.BackgroundImage = Properties.Resources.diamond_2;
            ico_12.BackgroundImageLayout = ImageLayout.Stretch;
            ico_12.Location = new Point(12, 78);
            ico_12.Name = "ico_12";
            ico_12.Size = new Size(30, 30);
            ico_12.TabIndex = 16;
            // 
            // txt_Diamond_Blu
            // 
            txt_Diamond_Blu.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txt_Diamond_Blu.Location = new Point(165, 81);
            txt_Diamond_Blu.Name = "txt_Diamond_Blu";
            txt_Diamond_Blu.ReadOnly = true;
            txt_Diamond_Blu.Size = new Size(77, 25);
            txt_Diamond_Blu.TabIndex = 13;
            txt_Diamond_Blu.Text = "0";
            txt_Diamond_Blu.TextAlign = HorizontalAlignment.Center;
            // 
            // ico_11
            // 
            ico_11.BackgroundImage = Properties.Resources.diamond_1;
            ico_11.BackgroundImageLayout = ImageLayout.Stretch;
            ico_11.Location = new Point(131, 78);
            ico_11.Name = "ico_11";
            ico_11.Size = new Size(30, 30);
            ico_11.TabIndex = 14;
            // 
            // btn_Scambia
            // 
            btn_Scambia.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Scambia.FlatAppearance.BorderSize = 0;
            btn_Scambia.FlatStyle = FlatStyle.Popup;
            btn_Scambia.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Scambia.Location = new Point(85, 114);
            btn_Scambia.Name = "btn_Scambia";
            btn_Scambia.Size = new Size(85, 32);
            btn_Scambia.TabIndex = 18;
            btn_Scambia.Text = "Scambia";
            btn_Scambia.UseVisualStyleBackColor = true;
            btn_Scambia.Click += btn_Scambia_Click;
            // 
            // txt_Testo
            // 
            txt_Testo.BorderStyle = BorderStyle.None;
            txt_Testo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txt_Testo.Location = new Point(12, 12);
            txt_Testo.Multiline = true;
            txt_Testo.Name = "txt_Testo";
            txt_Testo.ReadOnly = true;
            txt_Testo.Size = new Size(230, 37);
            txt_Testo.TabIndex = 33;
            txt_Testo.Text = "Descrizione\r\n1 Diamante Viola = 3 Diamanti Blu";
            txt_Testo.TextAlign = HorizontalAlignment.Center;
            // 
            // pictureBox_Più
            // 
            pictureBox_Più.BackColor = Color.Transparent;
            pictureBox_Più.BackgroundImage = (Image)resources.GetObject("pictureBox_Più.BackgroundImage");
            pictureBox_Più.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Più.Location = new Point(105, 55);
            pictureBox_Più.Name = "pictureBox_Più";
            pictureBox_Più.Size = new Size(20, 20);
            pictureBox_Più.TabIndex = 40;
            pictureBox_Più.TabStop = false;
            pictureBox_Più.Click += pictureBox_Più_Click;
            // 
            // pictureBox_Meno
            // 
            pictureBox_Meno.BackColor = Color.Transparent;
            pictureBox_Meno.BackgroundImage = Properties.Resources.minus_64x;
            pictureBox_Meno.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Meno.Location = new Point(46, 55);
            pictureBox_Meno.Name = "pictureBox_Meno";
            pictureBox_Meno.Size = new Size(20, 20);
            pictureBox_Meno.TabIndex = 41;
            pictureBox_Meno.TabStop = false;
            pictureBox_Meno.Click += pictureBox_Meno_Click;
            // 
            // Scambia_Diamanti
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.freepik__upload__73441_AAA;
            ClientSize = new Size(254, 156);
            Controls.Add(pictureBox_Meno);
            Controls.Add(pictureBox_Più);
            Controls.Add(txt_Testo);
            Controls.Add(btn_Scambia);
            Controls.Add(txt_Diamond_Viola);
            Controls.Add(ico_12);
            Controls.Add(txt_Diamond_Blu);
            Controls.Add(ico_11);
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