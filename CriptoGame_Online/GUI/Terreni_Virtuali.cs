
namespace Warrior_and_Wealth
{
    public partial class Terreni_Virtuali : Form
    {
        public Terreni_Virtuali()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void Terreni_Virtuali_Load(object sender, EventArgs e)
        {

            this.ActiveControl = ico_1; // assegna il focus al bottone
            txt_Rarita_1.Text = "Comune";
            txt_Rarita_2.Text = "Non Comune";
            txt_Rarita_3.Text = "Raro";
            txt_Rarita_4.Text = "Epico";
            txt_Rarita_5.Text = "Leggendario";

            txt_1.Text = "$ 0.00000000111 s";
            txt_2.Text = "$ 0.00000000222 s";
            txt_3.Text = "$ 0.00000000333 s";
            txt_4.Text = "$ 0.00000000444 s";
            txt_5.Text = "$ 0.00000000555 s";

            txt_Probabilita_1.Text = "50%";
            txt_Probabilita_2.Text = "20%";
            txt_Probabilita_3.Text = "15%";
            txt_Probabilita_4.Text = "10%";
            txt_Probabilita_5.Text = "5%";

            txt_Testo.BackColor = Color.FromArgb(235, 221, 192);
            txt_Testo.Font = new Font("Cinzel Decorative", 8, FontStyle.Bold);

            txt_Probabilita_1.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Probabilita_2.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Probabilita_3.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Probabilita_4.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Probabilita_5.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);

            txt_1.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_2.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_3.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_4.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_5.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);

            txt_Rarita_1.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Rarita_2.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Rarita_3.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Rarita_4.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Rarita_5.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);

            txt_Rarita_1.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Rarita_2.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Rarita_3.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Rarita_4.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);
            txt_Rarita_5.Font = new Font("Cinzel Decorative", 9, FontStyle.Bold);

            txt_1.BackColor = Color.FromArgb(235, 221, 192);
            txt_2.BackColor = Color.FromArgb(235, 221, 192);
            txt_3.BackColor = Color.FromArgb(235, 221, 192);
            txt_4.BackColor = Color.FromArgb(235, 221, 192);
            txt_5.BackColor = Color.FromArgb(235, 221, 192);

            // Sfondo Rarità
            txt_Rarita_1.BackColor = Color.FromArgb(235, 221, 192);
            txt_Rarita_2.BackColor = Color.FromArgb(235, 221, 192);
            txt_Rarita_3.BackColor = Color.FromArgb(235, 221, 192);
            txt_Rarita_4.BackColor = Color.FromArgb(235, 221, 192);
            txt_Rarita_5.BackColor = Color.FromArgb(235, 221, 192);

            txt_Probabilita_1.BackColor = Color.FromArgb(235, 221, 192);
            txt_Probabilita_2.BackColor = Color.FromArgb(235, 221, 192);
            txt_Probabilita_3.BackColor = Color.FromArgb(235, 221, 192);
            txt_Probabilita_4.BackColor = Color.FromArgb(235, 221, 192);
            txt_Probabilita_5.BackColor = Color.FromArgb(235, 221, 192);

            // Testo Rarità
            txt_Rarita_1.ForeColor = Color.FromArgb(128, 128, 128);   // Comune
            txt_Rarita_2.ForeColor = Color.FromArgb(0, 200, 0);       // Non Comune
            txt_Rarita_3.ForeColor = Color.FromArgb(0, 112, 255);     // Raro
            txt_Rarita_4.ForeColor = Color.FromArgb(160, 32, 240);    // Epico
            txt_Rarita_5.ForeColor = Color.FromArgb(205, 175, 0);     // Leggendario

            txt_Probabilita_1.ForeColor = Color.FromArgb(128, 128, 128);
            txt_Probabilita_2.ForeColor = Color.FromArgb(0, 200, 0);     
            txt_Probabilita_3.ForeColor = Color.FromArgb(0, 112, 255);   
            txt_Probabilita_4.ForeColor = Color.FromArgb(160, 32, 240);  
            txt_Probabilita_5.ForeColor = Color.FromArgb(205, 175, 0);   

            groupBox3.BackColor = Color.FromArgb(100, 235, 221, 192);
            panel1.BackColor = Color.FromArgb(100, 235, 221, 192);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            ActiveForm.Close();
        }
    }
}
