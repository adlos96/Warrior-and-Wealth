using Strategico_V2;

namespace Warrior_and_Wealth.GUI
{
    public partial class GamePassReward : Form
    {
        int selezione = 1;
        public GamePassReward()
        {
            InitializeComponent();
            Update_Guid();
        }

        private void GamePassReward_Load(object sender, EventArgs e)
        {
            lbl_Giorno_1.Text = "1";
            lbl_Giorno_2.Text = "2";
            lbl_Giorno_3.Text = "3";
            lbl_Giorno_4.Text = "4";
            lbl_Giorno_5.Text = "5";
            lbl_Giorno_6.Text = "6";
            lbl_Giorno_7.Text = "7";
            lbl_Giorno_8.Text = "8";
            lbl_Giorno_9.Text = "9";
            lbl_Giorno_10.Text = "10";
            lbl_Giorno_11.Text = "11";
            lbl_Giorno_12.Text = "12";
            lbl_Giorno_13.Text = "13";
            lbl_Giorno_14.Text = "14";
            lbl_Giorno_15.Text = "15";
            lbl_Giorno_16.Text = "16";
            lbl_Giorno_17.Text = "17";
            lbl_Giorno_18.Text = "18";
            lbl_Giorno_19.Text = "19";
            lbl_Giorno_20.Text = "20";
            lbl_Giorno_21.Text = "21";
            lbl_Giorno_22.Text = "22";
            lbl_Giorno_23.Text = "23";
            lbl_Giorno_24.Text = "24";
            lbl_Giorno_25.Text = "25";
            lbl_Giorno_26.Text = "26";
            lbl_Giorno_27.Text = "27";
            lbl_Giorno_28.Text = "28";
            lbl_Giorno_29.Text = "29";
            lbl_Giorno_30.Text = "30";

            lbl_Reward_1.Text = Variabili_Client.GamePass_Premi[0].ToString();
            lbl_Reward_2.Text = Variabili_Client.GamePass_Premi[1].ToString();
            lbl_Reward_3.Text = Variabili_Client.GamePass_Premi[2].ToString();
            lbl_Reward_4.Text = Variabili_Client.GamePass_Premi[3].ToString();
            lbl_Reward_5.Text = Variabili_Client.GamePass_Premi[4].ToString();
            lbl_Reward_6.Text = Variabili_Client.GamePass_Premi[5].ToString();
            lbl_Reward_7.Text = Variabili_Client.GamePass_Premi[6].ToString();
            lbl_Reward_8.Text = Variabili_Client.GamePass_Premi[7].ToString();
            lbl_Reward_9.Text = Variabili_Client.GamePass_Premi[8].ToString();
            lbl_Reward_10.Text = Variabili_Client.GamePass_Premi[9].ToString();

            lbl_Reward_11.Text = Variabili_Client.GamePass_Premi[10].ToString();
            lbl_Reward_12.Text = Variabili_Client.GamePass_Premi[11].ToString();
            lbl_Reward_13.Text = Variabili_Client.GamePass_Premi[12].ToString();
            lbl_Reward_14.Text = Variabili_Client.GamePass_Premi[13].ToString();
            lbl_Reward_15.Text = Variabili_Client.GamePass_Premi[14].ToString();
            lbl_Reward_16.Text = Variabili_Client.GamePass_Premi[15].ToString();
            lbl_Reward_17.Text = Variabili_Client.GamePass_Premi[16].ToString();
            lbl_Reward_18.Text = Variabili_Client.GamePass_Premi[17].ToString();
            lbl_Reward_19.Text = Variabili_Client.GamePass_Premi[18].ToString();
            lbl_Reward_20.Text = Variabili_Client.GamePass_Premi[19].ToString();

            lbl_Reward_21.Text = Variabili_Client.GamePass_Premi[20].ToString();
            lbl_Reward_22.Text = Variabili_Client.GamePass_Premi[21].ToString();
            lbl_Reward_23.Text = Variabili_Client.GamePass_Premi[22].ToString();
            lbl_Reward_24.Text = Variabili_Client.GamePass_Premi[23].ToString();
            lbl_Reward_25.Text = Variabili_Client.GamePass_Premi[24].ToString();
            lbl_Reward_26.Text = Variabili_Client.GamePass_Premi[25].ToString();
            lbl_Reward_27.Text = Variabili_Client.GamePass_Premi[26].ToString();
            lbl_Reward_28.Text = Variabili_Client.GamePass_Premi[27].ToString();
            lbl_Reward_29.Text = Variabili_Client.GamePass_Premi[28].ToString();
            lbl_Reward_30.Text = Variabili_Client.GamePass_Premi[29].ToString();
        }
        void Update_Guid()
        {
            if (selezione == 1)
                if (Variabili_Client.GamePass_Premi_Completati[0] == false && Variabili_Client.Utente.User_GamePass_Avanzato)
                {
                    btn_Reward_GamePass_1.Enabled = true;
                    btn_Reward_GamePass_1.BackColor = Color.Purple;
                    lbl_Giorno_1.BackColor = Color.Purple;
                    lbl_Reward_1.BackColor = Color.Purple;
                    lbl_Reward_1.ForeColor = Color.Chartreuse;
                }
                else
                {
                    btn_Reward_GamePass_1.Enabled = false;
                    btn_Reward_GamePass_1.BackColor = Color.FromArgb(50, 50, 50);
                    lbl_Giorno_1.BackColor = Color.FromArgb(50, 50, 50);
                    lbl_Reward_1.BackColor = Color.FromArgb(50, 50, 50);
                    lbl_Reward_1.ForeColor = Color.DarkGray;
                }

            for (int i = (1 * (selezione - 1) * 30) + 1; i < Variabili_Client.GamePass_Premi.Count() - 30 * (3 - selezione); i++)
            {
                Button btn_reward = null;
                Label lbl_Giorno = null;
                Label lbl_Premio = null;
                if (selezione == 1)
                {
                    btn_reward = (Button)this.Controls.Find($"btn_Reward_GamePass_{i - ((selezione - 1) * 30) + 1}", true)[0];
                    lbl_Giorno = (Label)this.Controls.Find($"lbl_Giorno_{i - ((selezione - 1) * 30) + 1}", true)[0];
                    lbl_Premio = (Label)this.Controls.Find($"lbl_Reward_{i - ((selezione - 1) * 30) + 1}", true)[0];
                }
                else
                {
                    btn_reward = (Button)this.Controls.Find($"btn_Reward_GamePass_{i - ((selezione - 1) * 30)}", true)[0];
                    lbl_Giorno = (Label)this.Controls.Find($"lbl_Giorno_{i - ((selezione - 1) * 30)}", true)[0];
                    lbl_Premio = (Label)this.Controls.Find($"lbl_Reward_{i - ((selezione - 1) * 30)}", true)[0];
                }

                if (Variabili_Client.GamePass_Premi_Completati[i] == false &&
                    Variabili_Client.GamePass_Premi_Completati[i - 1] == true &&
                    Variabili_Client.Giorni_Accessi_Consecutivi == i)
                {

                    btn_reward.Enabled = true;

                    btn_reward.BackColor = Color.Purple;
                    lbl_Giorno.BackColor = Color.Purple;
                    lbl_Premio.BackColor = Color.Purple;

                }
                else
                    if (Variabili_Client.GamePass_Premi_Completati[i] == true)
                    {
                        btn_reward.Enabled = false;
                        btn_reward.BackColor = Color.FromArgb(50, 50, 50);
                        lbl_Giorno.BackColor = Color.FromArgb(50, 50, 50);
                        lbl_Premio.BackColor = Color.FromArgb(50, 50, 50);
                        lbl_Premio.ForeColor = Color.DarkGray;
                    }
                    else
                    {
                        btn_reward.Enabled = false;
                        btn_reward.BackColor = Color.Transparent;
                        lbl_Giorno.BackColor = Color.Transparent;
                        lbl_Premio.BackColor = Color.Transparent;
                        lbl_Premio.ForeColor = Color.Violet;
                    }
            }
        }
        private void Btn_1_30_Click(object sender, EventArgs e)
        {
            lbl_Giorno_1.Text = "1";
            lbl_Giorno_2.Text = "2";
            lbl_Giorno_3.Text = "3";
            lbl_Giorno_4.Text = "4";
            lbl_Giorno_5.Text = "5";
            lbl_Giorno_6.Text = "6";
            lbl_Giorno_7.Text = "7";
            lbl_Giorno_8.Text = "8";
            lbl_Giorno_9.Text = "9";
            lbl_Giorno_10.Text = "10";
            lbl_Giorno_11.Text = "11";
            lbl_Giorno_12.Text = "12";
            lbl_Giorno_13.Text = "13";
            lbl_Giorno_14.Text = "14";
            lbl_Giorno_15.Text = "15";
            lbl_Giorno_16.Text = "16";
            lbl_Giorno_17.Text = "17";
            lbl_Giorno_18.Text = "18";
            lbl_Giorno_19.Text = "19";
            lbl_Giorno_20.Text = "20";
            lbl_Giorno_21.Text = "21";
            lbl_Giorno_22.Text = "22";
            lbl_Giorno_23.Text = "23";
            lbl_Giorno_24.Text = "24";
            lbl_Giorno_25.Text = "25";
            lbl_Giorno_26.Text = "26";
            lbl_Giorno_27.Text = "27";
            lbl_Giorno_28.Text = "28";
            lbl_Giorno_29.Text = "29";
            lbl_Giorno_30.Text = "30";

            lbl_Reward_1.Text = Variabili_Client.GamePass_Premi[0].ToString();
            lbl_Reward_2.Text = Variabili_Client.GamePass_Premi[1].ToString();
            lbl_Reward_3.Text = Variabili_Client.GamePass_Premi[2].ToString();
            lbl_Reward_4.Text = Variabili_Client.GamePass_Premi[3].ToString();
            lbl_Reward_5.Text = Variabili_Client.GamePass_Premi[4].ToString();
            lbl_Reward_6.Text = Variabili_Client.GamePass_Premi[5].ToString();
            lbl_Reward_7.Text = Variabili_Client.GamePass_Premi[6].ToString();
            lbl_Reward_8.Text = Variabili_Client.GamePass_Premi[7].ToString();
            lbl_Reward_9.Text = Variabili_Client.GamePass_Premi[8].ToString();
            lbl_Reward_10.Text = Variabili_Client.GamePass_Premi[9].ToString();

            lbl_Reward_11.Text = Variabili_Client.GamePass_Premi[10].ToString();
            lbl_Reward_12.Text = Variabili_Client.GamePass_Premi[11].ToString();
            lbl_Reward_13.Text = Variabili_Client.GamePass_Premi[12].ToString();
            lbl_Reward_14.Text = Variabili_Client.GamePass_Premi[13].ToString();
            lbl_Reward_15.Text = Variabili_Client.GamePass_Premi[14].ToString();
            lbl_Reward_16.Text = Variabili_Client.GamePass_Premi[15].ToString();
            lbl_Reward_17.Text = Variabili_Client.GamePass_Premi[16].ToString();
            lbl_Reward_18.Text = Variabili_Client.GamePass_Premi[17].ToString();
            lbl_Reward_19.Text = Variabili_Client.GamePass_Premi[18].ToString();
            lbl_Reward_20.Text = Variabili_Client.GamePass_Premi[19].ToString();

            lbl_Reward_21.Text = Variabili_Client.GamePass_Premi[20].ToString();
            lbl_Reward_22.Text = Variabili_Client.GamePass_Premi[21].ToString();
            lbl_Reward_23.Text = Variabili_Client.GamePass_Premi[22].ToString();
            lbl_Reward_24.Text = Variabili_Client.GamePass_Premi[23].ToString();
            lbl_Reward_25.Text = Variabili_Client.GamePass_Premi[24].ToString();
            lbl_Reward_26.Text = Variabili_Client.GamePass_Premi[25].ToString();
            lbl_Reward_27.Text = Variabili_Client.GamePass_Premi[26].ToString();
            lbl_Reward_28.Text = Variabili_Client.GamePass_Premi[27].ToString();
            lbl_Reward_29.Text = Variabili_Client.GamePass_Premi[28].ToString();
            lbl_Reward_30.Text = Variabili_Client.GamePass_Premi[29].ToString();

            selezione = 1;
            Update_Guid();
        }

        private void Btn_31_60_Click(object sender, EventArgs e)
        {
            lbl_Giorno_1.Text = "31";
            lbl_Giorno_2.Text = "32";
            lbl_Giorno_3.Text = "33";
            lbl_Giorno_4.Text = "34";
            lbl_Giorno_5.Text = "35";
            lbl_Giorno_6.Text = "36";
            lbl_Giorno_7.Text = "37";
            lbl_Giorno_8.Text = "38";
            lbl_Giorno_9.Text = "39";
            lbl_Giorno_10.Text = "40";
            lbl_Giorno_11.Text = "41";
            lbl_Giorno_12.Text = "42";
            lbl_Giorno_13.Text = "43";
            lbl_Giorno_14.Text = "44";
            lbl_Giorno_15.Text = "45";
            lbl_Giorno_16.Text = "46";
            lbl_Giorno_17.Text = "47";
            lbl_Giorno_18.Text = "48";
            lbl_Giorno_19.Text = "49";
            lbl_Giorno_20.Text = "50";
            lbl_Giorno_21.Text = "51";
            lbl_Giorno_22.Text = "52";
            lbl_Giorno_23.Text = "53";
            lbl_Giorno_24.Text = "54";
            lbl_Giorno_25.Text = "55";
            lbl_Giorno_26.Text = "56";
            lbl_Giorno_27.Text = "57";
            lbl_Giorno_28.Text = "58";
            lbl_Giorno_29.Text = "59";
            lbl_Giorno_30.Text = "60";

            lbl_Reward_1.Text = Variabili_Client.GamePass_Premi[30].ToString();
            lbl_Reward_2.Text = Variabili_Client.GamePass_Premi[31].ToString();
            lbl_Reward_3.Text = Variabili_Client.GamePass_Premi[32].ToString();
            lbl_Reward_4.Text = Variabili_Client.GamePass_Premi[33].ToString();
            lbl_Reward_5.Text = Variabili_Client.GamePass_Premi[34].ToString();
            lbl_Reward_6.Text = Variabili_Client.GamePass_Premi[35].ToString();
            lbl_Reward_7.Text = Variabili_Client.GamePass_Premi[36].ToString();
            lbl_Reward_8.Text = Variabili_Client.GamePass_Premi[37].ToString();
            lbl_Reward_9.Text = Variabili_Client.GamePass_Premi[38].ToString();
            lbl_Reward_10.Text = Variabili_Client.GamePass_Premi[39].ToString();

            lbl_Reward_11.Text = Variabili_Client.GamePass_Premi[40].ToString();
            lbl_Reward_12.Text = Variabili_Client.GamePass_Premi[41].ToString();
            lbl_Reward_13.Text = Variabili_Client.GamePass_Premi[42].ToString();
            lbl_Reward_14.Text = Variabili_Client.GamePass_Premi[43].ToString();
            lbl_Reward_15.Text = Variabili_Client.GamePass_Premi[44].ToString();
            lbl_Reward_16.Text = Variabili_Client.GamePass_Premi[45].ToString();
            lbl_Reward_17.Text = Variabili_Client.GamePass_Premi[46].ToString();
            lbl_Reward_18.Text = Variabili_Client.GamePass_Premi[47].ToString();
            lbl_Reward_19.Text = Variabili_Client.GamePass_Premi[48].ToString();
            lbl_Reward_20.Text = Variabili_Client.GamePass_Premi[49].ToString();

            lbl_Reward_21.Text = Variabili_Client.GamePass_Premi[50].ToString();
            lbl_Reward_22.Text = Variabili_Client.GamePass_Premi[51].ToString();
            lbl_Reward_23.Text = Variabili_Client.GamePass_Premi[52].ToString();
            lbl_Reward_24.Text = Variabili_Client.GamePass_Premi[53].ToString();
            lbl_Reward_25.Text = Variabili_Client.GamePass_Premi[54].ToString();
            lbl_Reward_26.Text = Variabili_Client.GamePass_Premi[55].ToString();
            lbl_Reward_27.Text = Variabili_Client.GamePass_Premi[56].ToString();
            lbl_Reward_28.Text = Variabili_Client.GamePass_Premi[57].ToString();
            lbl_Reward_29.Text = Variabili_Client.GamePass_Premi[58].ToString();
            lbl_Reward_30.Text = Variabili_Client.GamePass_Premi[59].ToString();

            selezione = 2;
            Update_Guid();
        }

        private void Btn_61_90_Click(object sender, EventArgs e)
        {
            lbl_Giorno_1.Text = "61";
            lbl_Giorno_2.Text = "62";
            lbl_Giorno_3.Text = "63";
            lbl_Giorno_4.Text = "64";
            lbl_Giorno_5.Text = "65";
            lbl_Giorno_6.Text = "66";
            lbl_Giorno_7.Text = "67";
            lbl_Giorno_8.Text = "68";
            lbl_Giorno_9.Text = "69";
            lbl_Giorno_10.Text = "70";
            lbl_Giorno_11.Text = "71";
            lbl_Giorno_12.Text = "72";
            lbl_Giorno_13.Text = "73";
            lbl_Giorno_14.Text = "74";
            lbl_Giorno_15.Text = "75";
            lbl_Giorno_16.Text = "76";
            lbl_Giorno_17.Text = "77";
            lbl_Giorno_18.Text = "78";
            lbl_Giorno_19.Text = "79";
            lbl_Giorno_20.Text = "80";
            lbl_Giorno_21.Text = "81";
            lbl_Giorno_22.Text = "82";
            lbl_Giorno_23.Text = "83";
            lbl_Giorno_24.Text = "84";
            lbl_Giorno_25.Text = "85";
            lbl_Giorno_26.Text = "86";
            lbl_Giorno_27.Text = "87";
            lbl_Giorno_28.Text = "88";
            lbl_Giorno_29.Text = "89";
            lbl_Giorno_30.Text = "90";

            lbl_Reward_1.Text = Variabili_Client.GamePass_Premi[60].ToString();
            lbl_Reward_2.Text = Variabili_Client.GamePass_Premi[61].ToString();
            lbl_Reward_3.Text = Variabili_Client.GamePass_Premi[62].ToString();
            lbl_Reward_4.Text = Variabili_Client.GamePass_Premi[63].ToString();
            lbl_Reward_5.Text = Variabili_Client.GamePass_Premi[64].ToString();
            lbl_Reward_6.Text = Variabili_Client.GamePass_Premi[65].ToString();
            lbl_Reward_7.Text = Variabili_Client.GamePass_Premi[66].ToString();
            lbl_Reward_8.Text = Variabili_Client.GamePass_Premi[67].ToString();
            lbl_Reward_9.Text = Variabili_Client.GamePass_Premi[68].ToString();
            lbl_Reward_10.Text = Variabili_Client.GamePass_Premi[69].ToString();

            lbl_Reward_11.Text = Variabili_Client.GamePass_Premi[70].ToString();
            lbl_Reward_12.Text = Variabili_Client.GamePass_Premi[71].ToString();
            lbl_Reward_13.Text = Variabili_Client.GamePass_Premi[72].ToString();
            lbl_Reward_14.Text = Variabili_Client.GamePass_Premi[73].ToString();
            lbl_Reward_15.Text = Variabili_Client.GamePass_Premi[74].ToString();
            lbl_Reward_16.Text = Variabili_Client.GamePass_Premi[75].ToString();
            lbl_Reward_17.Text = Variabili_Client.GamePass_Premi[76].ToString();
            lbl_Reward_18.Text = Variabili_Client.GamePass_Premi[77].ToString();
            lbl_Reward_19.Text = Variabili_Client.GamePass_Premi[78].ToString();
            lbl_Reward_20.Text = Variabili_Client.GamePass_Premi[79].ToString();

            lbl_Reward_21.Text = Variabili_Client.GamePass_Premi[80].ToString();
            lbl_Reward_22.Text = Variabili_Client.GamePass_Premi[81].ToString();
            lbl_Reward_23.Text = Variabili_Client.GamePass_Premi[82].ToString();
            lbl_Reward_24.Text = Variabili_Client.GamePass_Premi[83].ToString();
            lbl_Reward_25.Text = Variabili_Client.GamePass_Premi[84].ToString();
            lbl_Reward_26.Text = Variabili_Client.GamePass_Premi[85].ToString();
            lbl_Reward_27.Text = Variabili_Client.GamePass_Premi[86].ToString();
            lbl_Reward_28.Text = Variabili_Client.GamePass_Premi[87].ToString();
            lbl_Reward_29.Text = Variabili_Client.GamePass_Premi[88].ToString();
            lbl_Reward_30.Text = Variabili_Client.GamePass_Premi[89].ToString();

            selezione = 3;
            Update_Guid();
        }

        private async void btn_Reward_GamePass_1_Click(object sender, EventArgs e)
        {
            ClientConnection.TestClient.Send($"GamePass DailyReward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}");
            await Login.Sleep(4);
            Update_Guid();
        }

        private void btn_Reward_GamePass_2_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_3_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_4_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_5_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_6_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_7_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_8_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_9_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_10_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_11_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_12_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_13_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_14_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_15_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_16_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_17_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_18_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_19_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_20_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_21_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_22_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_23_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_24_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_25_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_26_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_27_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_28_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_29_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }

        private void btn_Reward_GamePass_30_Click(object sender, EventArgs e)
        {
            btn_Reward_GamePass_1_Click(sender, e);
        }
    }
}
