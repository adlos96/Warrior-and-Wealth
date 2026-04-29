namespace Warrior_and_Wealth.GUI
{
    partial class Mappa
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
            Panel_Mappa = new Warrior_and_Wealth.Strumenti.DoubleBufferedPanel();
            SuspendLayout();
            // 
            // Panel_Mappa
            // 
            Panel_Mappa.Dock = DockStyle.Fill;
            Panel_Mappa.Location = new Point(0, 0);
            Panel_Mappa.Margin = new Padding(3, 4, 3, 4);
            Panel_Mappa.Name = "Panel_Mappa";
            Panel_Mappa.Size = new Size(914, 600);
            Panel_Mappa.TabIndex = 0;
            // 
            // Mappa
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(914, 600);
            Controls.Add(Panel_Mappa);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Mappa";
            Text = "Mappa";
            Load += Mappa_Load;
            ResumeLayout(false);
        }

        #endregion

        private Strumenti.DoubleBufferedPanel Panel_Mappa;
    }
}