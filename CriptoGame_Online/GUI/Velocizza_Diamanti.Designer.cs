namespace Warrior_and_Wealth.GUI
{
    partial class Velocizza_Diamanti
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Velocizza_Diamanti));
            pictureBox_Meno = new PictureBox();
            pictureBox_Più = new PictureBox();
            txt_Testo = new TextBox();
            btn_Velocizza = new Button();
            txt_Diamond_Blu = new TextBox();
            ico_12 = new Warrior_and_Wealth.Strumenti.DoubleBufferedPanel();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Meno).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Più).BeginInit();
            SuspendLayout();
            // 
            // pictureBox_Meno
            // 
            pictureBox_Meno.BackColor = Color.Transparent;
            pictureBox_Meno.BackgroundImage = Properties.Resources.minus_64x;
            pictureBox_Meno.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Meno.Location = new Point(38, 64);
            pictureBox_Meno.Margin = new Padding(2, 3, 2, 3);
            pictureBox_Meno.Name = "pictureBox_Meno";
            pictureBox_Meno.Size = new Size(18, 22);
            pictureBox_Meno.TabIndex = 49;
            pictureBox_Meno.TabStop = false;
            pictureBox_Meno.Click += pictureBox_Meno_Click;
            // 
            // pictureBox_Più
            // 
            pictureBox_Più.BackColor = Color.Transparent;
            pictureBox_Più.BackgroundImage = (Image)resources.GetObject("pictureBox_Più.BackgroundImage");
            pictureBox_Più.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox_Più.Location = new Point(171, 64);
            pictureBox_Più.Margin = new Padding(2, 3, 2, 3);
            pictureBox_Più.Name = "pictureBox_Più";
            pictureBox_Più.Size = new Size(18, 22);
            pictureBox_Più.TabIndex = 48;
            pictureBox_Più.TabStop = false;
            pictureBox_Più.Click += pictureBox_Più_Click;
            // 
            // txt_Testo
            // 
            txt_Testo.BorderStyle = BorderStyle.None;
            txt_Testo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txt_Testo.Location = new Point(11, 13);
            txt_Testo.Margin = new Padding(2, 3, 2, 3);
            txt_Testo.Multiline = true;
            txt_Testo.Name = "txt_Testo";
            txt_Testo.ReadOnly = true;
            txt_Testo.Size = new Size(210, 39);
            txt_Testo.TabIndex = 47;
            txt_Testo.Text = "Seleziona il numero di diamanti da utilizzare";
            txt_Testo.TextAlign = HorizontalAlignment.Center;
            // 
            // btn_Velocizza
            // 
            btn_Velocizza.BackgroundImage = Properties.Resources.Texture_Wood_2;
            btn_Velocizza.FlatAppearance.BorderSize = 0;
            btn_Velocizza.FlatStyle = FlatStyle.Popup;
            btn_Velocizza.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Velocizza.Location = new Point(38, 95);
            btn_Velocizza.Margin = new Padding(2, 3, 2, 3);
            btn_Velocizza.Name = "btn_Velocizza";
            btn_Velocizza.Size = new Size(151, 34);
            btn_Velocizza.TabIndex = 46;
            btn_Velocizza.Text = "Velocizza Costruzione";
            btn_Velocizza.UseVisualStyleBackColor = true;
            btn_Velocizza.Click += btn_Velocizza_Click;
            // 
            // txt_Diamond_Blu
            // 
            txt_Diamond_Blu.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txt_Diamond_Blu.Location = new Point(94, 62);
            txt_Diamond_Blu.Margin = new Padding(2, 3, 2, 3);
            txt_Diamond_Blu.Name = "txt_Diamond_Blu";
            txt_Diamond_Blu.ReadOnly = true;
            txt_Diamond_Blu.Size = new Size(73, 25);
            txt_Diamond_Blu.TabIndex = 44;
            txt_Diamond_Blu.Text = "0";
            txt_Diamond_Blu.TextAlign = HorizontalAlignment.Center;
            // 
            // ico_12
            // 
            ico_12.BackgroundImage = Properties.Resources.DiamanteBlu_V2;
            ico_12.BackgroundImageLayout = ImageLayout.Stretch;
            ico_12.Location = new Point(62, 58);
            ico_12.Margin = new Padding(2, 3, 2, 3);
            ico_12.Name = "ico_12";
            ico_12.Size = new Size(27, 32);
            ico_12.TabIndex = 45;
            // 
            // Velocizza_Diamanti
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackgroundImage = Properties.Resources.freepik__upload__73441_AAA;
            ClientSize = new Size(232, 140);
            Controls.Add(pictureBox_Meno);
            Controls.Add(pictureBox_Più);
            Controls.Add(txt_Testo);
            Controls.Add(btn_Velocizza);
            Controls.Add(txt_Diamond_Blu);
            Controls.Add(ico_12);
            Margin = new Padding(2, 3, 2, 3);
            Name = "Velocizza_Diamanti";
            Text = "Velocizza_Diamanti";
            Load += Velocizza_Diamanti_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox_Meno).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Più).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox_Meno;
        private PictureBox pictureBox_Più;
        private TextBox txt_Testo;
        private Button btn_Velocizza;
        private TextBox txt_Diamond_Blu;
        private Strumenti.DoubleBufferedPanel ico_12;
    }
}