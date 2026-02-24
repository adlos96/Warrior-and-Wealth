namespace Warrior_and_Wealth.GUI
{
    partial class Tutorial
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
            panel2 = new Warrior_and_Wealth.Strumenti.DoubleBufferedPanel();
            textBox1 = new TextBox();
            panel_Log = new Warrior_and_Wealth.Strumenti.DoubleBufferedPanel();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackgroundImage = Properties.Resources.Sfondo_Chat_V2_removebg_preview;
            panel2.BackgroundImageLayout = ImageLayout.Zoom;
            panel2.Controls.Add(textBox1);
            panel2.Controls.Add(panel_Log);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(679, 370);
            panel2.TabIndex = 41;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(32, 26, 14);
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox1.ForeColor = Color.Silver;
            textBox1.Location = new Point(179, 15);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(323, 26);
            textBox1.TabIndex = 40;
            textBox1.Text = "Tutorial";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // panel_Log
            // 
            panel_Log.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel_Log.BackColor = Color.FromArgb(32, 26, 14);
            panel_Log.Location = new Point(31, 62);
            panel_Log.Name = "panel_Log";
            panel_Log.Size = new Size(613, 276);
            panel_Log.TabIndex = 39;
            // 
            // Tutorial
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(679, 370);
            Controls.Add(panel2);
            Name = "Tutorial";
            Text = "Tutorial";
            FormClosing += Tutorial_FormClosing;
            Load += Tutorial_Load;
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Strumenti.DoubleBufferedPanel panel2;
        private TextBox textBox1;
        private Strumenti.DoubleBufferedPanel panel_Log;
    }
}