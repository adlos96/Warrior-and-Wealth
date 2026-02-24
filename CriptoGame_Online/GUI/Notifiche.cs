using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Warrior_and_Wealth.GUI
{
    public partial class Notifiche : Form
    {
        private int clickedRow = -1;
        public Notifiche()
        {
            InitializeComponent();
        }

        private void Notifiche_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add("Esplorazione", "Città Barbaro", "01-01-2026", "Dettagli");
            dataGridView1.Rows.Add("Esplorazione", "Villaggio Barbaro", "01-01-2026", "Dettagli");
            dataGridView1.Rows.Add("Attacco", "Adlos", "01-01-2026", "Dettagli");
            dataGridView1.Rows.Add("Difesa", "Franco", "01-01-2026", "Dettagli");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "Col_Bottone")
            {
                clickedRow = e.RowIndex; // salva la riga cliccata
                                                                         // qui puoi aggiungere l'azione del bottone
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }
    }
}
