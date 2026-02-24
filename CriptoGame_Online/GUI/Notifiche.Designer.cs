namespace Warrior_and_Wealth.GUI
{
    partial class Notifiche
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dataGridView1 = new DataGridView();
            Col_Stato = new DataGridViewTextBoxColumn();
            Col_Villaggio = new DataGridViewTextBoxColumn();
            Col_Data = new DataGridViewTextBoxColumn();
            Col_Bottone = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.Silver;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.DimGray;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionBackColor = Color.DimGray;
            dataGridViewCellStyle1.SelectionForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Col_Stato, Col_Villaggio, Col_Data, Col_Bottone });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.Gray;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlLightLight;
            dataGridViewCellStyle2.SelectionBackColor = Color.Gray;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.ControlLightLight;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.GridColor = Color.Gray;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.Gray;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlLightLight;
            dataGridViewCellStyle3.SelectionBackColor = Color.Gray;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.ControlLightLight;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView1.RowTemplate.DefaultCellStyle.BackColor = Color.Gray;
            dataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.Gray;
            dataGridView1.RowTemplate.Resizable = DataGridViewTriState.False;
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView1.Size = new Size(590, 207);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellPainting += dataGridView1_CellPainting;
            // 
            // Col_Stato
            // 
            Col_Stato.FillWeight = 88.5754242F;
            Col_Stato.HeaderText = "Stato";
            Col_Stato.Name = "Col_Stato";
            Col_Stato.ReadOnly = true;
            Col_Stato.Resizable = DataGridViewTriState.False;
            // 
            // Col_Villaggio
            // 
            Col_Villaggio.FillWeight = 115.103157F;
            Col_Villaggio.HeaderText = "Villaggio";
            Col_Villaggio.Name = "Col_Villaggio";
            Col_Villaggio.ReadOnly = true;
            Col_Villaggio.Resizable = DataGridViewTriState.False;
            // 
            // Col_Data
            // 
            Col_Data.FillWeight = 115.103157F;
            Col_Data.HeaderText = "Giorno";
            Col_Data.Name = "Col_Data";
            Col_Data.ReadOnly = true;
            Col_Data.Resizable = DataGridViewTriState.False;
            // 
            // Col_Bottone
            // 
            Col_Bottone.FillWeight = 81.21828F;
            Col_Bottone.HeaderText = "Apri";
            Col_Bottone.Name = "Col_Bottone";
            Col_Bottone.ReadOnly = true;
            Col_Bottone.Resizable = DataGridViewTriState.False;
            // 
            // Notifiche
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(590, 207);
            Controls.Add(dataGridView1);
            Name = "Notifiche";
            Text = "Notifiche";
            Load += Notifiche_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Col_Stato;
        private DataGridViewTextBoxColumn Col_Villaggio;
        private DataGridViewTextBoxColumn Col_Data;
        private DataGridViewButtonColumn Col_Bottone;
    }
}