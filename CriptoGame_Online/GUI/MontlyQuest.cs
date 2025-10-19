using Strategico_V2;
using static Strategico_V2.ClientConnection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CriptoGame_Online
{
    public partial class MontlyQuest : Form
    {
        // Lista di quest attuali
        public static List<ClientQuestData> CurrentQuests { get; set; } = new List<ClientQuestData>();
        int currentIndex = 0;

        public static List<int> CurrentRewardsNormali { get; set; } = new();
        public static List<int> CurrentRewardsVip { get; set; } = new();
        public static List<int> CurrentRewardPoints { get; set; } = new();

        public MontlyQuest()
        {
            InitializeComponent();
            this.ActiveControl = btn_Reward_1; // assegna il focus al bottone
        }

        private void SetProgressValue(int newValue)
        {
            progressBar1.Value += newValue;
            OnProgressValueChanged();

        }

        private void OnProgressValueChanged()
        {
            if (progressBar1.Value > 20)
                txt_Punti_Reward_1.BackColor = Color.FromArgb(6, 176, 37);
        }

        private void MontlyQuest_Load(object sender, EventArgs e)
        {
            Update_Reward();
            Update_Quest();
            Check_Unlock_Reward();
            Check_Unlock_Reward_Vip();
            AggiornaInterfacciaQuest();
            AggiornaInterfacciaRewards();
            Task.Run(() => Gui_Update());
        }
        async void Gui_Update()
        {
            while (true)
            {
                Thread.Sleep(1000);
                txt_Punti_Reward_1.Invoke((Action)(async () =>
                {
                    AggiornaInterfacciaQuest();
                }));
            }
            currentIndex = (currentIndex + 10) % CurrentQuests.Count; //Varia l'index per cambiare le quest mostrate
        }
        void AggiornaInterfacciaQuest()
        {
            if (CurrentQuests.Count > 0)
            {
                int count = Math.Min(10, CurrentQuests.Count);
                for (int i = 0; i <= 9; i++)
                {
                    int questIndex = (currentIndex + i) % CurrentQuests.Count;
                    if (i == 0)
                    {
                        txt_Quest_Desc_1.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_1.Text = $"[{CurrentQuests[questIndex].Experience}] Exp   " + CurrentQuests[questIndex].Progress + "/" + CurrentQuests[questIndex].Require;
                    }
                    if (i == 1)
                    {
                        txt_Quest_Desc_2.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_2.Text = $"[{CurrentQuests[questIndex].Experience}] Exp   " + CurrentQuests[questIndex].Progress + "/" + CurrentQuests[questIndex].Require;
                    }
                    if (i == 2)
                    {
                        txt_Quest_Desc_3.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_3.Text = $"[{CurrentQuests[questIndex].Experience}] Exp   " + CurrentQuests[questIndex].Progress + "/" + CurrentQuests[questIndex].Require;
                    }
                    if (i == 3)
                    {
                        txt_Quest_Desc_4.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_4.Text = $"[{CurrentQuests[questIndex].Experience}] Exp   " + CurrentQuests[questIndex].Progress + "/" + CurrentQuests[questIndex].Require;
                    }
                    if (i == 4)
                    {
                        txt_Quest_Desc_5.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_5.Text = $"[{CurrentQuests[questIndex].Experience}] Exp   " + CurrentQuests[questIndex].Progress + "/" + CurrentQuests[questIndex].Require;
                    }
                    if (i == 5)
                    {
                        txt_Quest_Desc_6.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_6.Text = $"[{CurrentQuests[questIndex].Experience}] Exp  " + CurrentQuests[questIndex].Progress + "/" + CurrentQuests[questIndex].Require;
                    }
                    if (i == 6)
                    {
                        txt_Quest_Desc_7.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_7.Text = $"[{CurrentQuests[questIndex].Experience}] Exp   " + CurrentQuests[questIndex].Progress + "/" + CurrentQuests[questIndex].Require;
                    }
                    if (i == 7)
                    {
                        txt_Quest_Desc_8.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_8.Text = $"[{CurrentQuests[questIndex].Experience}] Exp   " + CurrentQuests[questIndex].Progress + "/" + CurrentQuests[questIndex].Require;
                    }
                    if (i == 8)
                    {
                        txt_Quest_Desc_9.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_9.Text = $"[{CurrentQuests[questIndex].Experience}] Exp   " + CurrentQuests[questIndex].Progress + "/" + CurrentQuests[questIndex].Require;
                    }
                    if (i == 9)
                    {
                        txt_Quest_Desc_10.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_10.Text = $"[{CurrentQuests[questIndex].Experience}] Exp  " + CurrentQuests[questIndex].Progress + "/" + CurrentQuests[questIndex].Require;
                    }
                }

            }
        }
        void AggiornaInterfacciaRewards()
        {
            if (CurrentRewardPoints.Count == 0) return;

            txt_Punti_Reward_1.Text = CurrentRewardPoints[0].ToString();
            txt_Reward_1.Text = CurrentRewardsNormali[0].ToString();
            txt_Reward_Vip_1.Text = CurrentRewardsVip[0].ToString();

            txt_Punti_Reward_2.Text = CurrentRewardPoints[1].ToString();
            txt_Reward_2.Text = CurrentRewardsNormali[1].ToString();
            txt_Reward_Vip_2.Text = CurrentRewardsVip[1].ToString();

            txt_Punti_Reward_3.Text = CurrentRewardPoints[2].ToString();
            txt_Reward_3.Text = CurrentRewardsNormali[2].ToString();
            txt_Reward_Vip_3.Text = CurrentRewardsVip[2].ToString();

            txt_Punti_Reward_4.Text = CurrentRewardPoints[3].ToString();
            txt_Reward_4.Text = CurrentRewardsNormali[3].ToString();
            txt_Reward_Vip_4.Text = CurrentRewardsVip[3].ToString();

            txt_Punti_Reward_5.Text = CurrentRewardPoints[4].ToString();
            txt_Reward_5.Text = CurrentRewardsNormali[4].ToString();
            txt_Reward_Vip_5.Text = CurrentRewardsVip[4].ToString();

            txt_Punti_Reward_6.Text = CurrentRewardPoints[5].ToString();
            txt_Reward_6.Text = CurrentRewardsNormali[5].ToString();
            txt_Reward_Vip_6.Text = CurrentRewardsVip[5].ToString();

            txt_Punti_Reward_7.Text = CurrentRewardPoints[6].ToString();
            txt_Reward_7.Text = CurrentRewardsNormali[6].ToString();
            txt_Reward_Vip_7.Text = CurrentRewardsVip[6].ToString();

            txt_Punti_Reward_8.Text = CurrentRewardPoints[7].ToString();
            txt_Reward_8.Text = CurrentRewardsNormali[7].ToString();
            txt_Reward_Vip_8.Text = CurrentRewardsVip[7].ToString();

            txt_Punti_Reward_9.Text = CurrentRewardPoints[8].ToString();
            txt_Reward_9.Text = CurrentRewardsNormali[8].ToString();
            txt_Reward_Vip_9.Text = CurrentRewardsVip[8].ToString();

            txt_Punti_Reward_10.Text = CurrentRewardPoints[9].ToString();
            txt_Reward_10.Text = CurrentRewardsNormali[9].ToString();
            txt_Reward_Vip_10.Text = CurrentRewardsVip[9].ToString();

            txt_Punti_Reward_11.Text = CurrentRewardPoints[10].ToString();
            txt_Reward_11.Text = CurrentRewardsNormali[10].ToString();
            txt_Reward_Vip_11.Text = CurrentRewardsVip[10].ToString();

            txt_Punti_Reward_12.Text = CurrentRewardPoints[11].ToString();
            txt_Reward_12.Text = CurrentRewardsNormali[11].ToString();
            txt_Reward_Vip_12.Text = CurrentRewardsVip[11].ToString();

            txt_Punti_Reward_13.Text = CurrentRewardPoints[12].ToString();
            txt_Reward_13.Text = CurrentRewardsNormali[12].ToString();
            txt_Reward_Vip_13.Text = CurrentRewardsVip[12].ToString();

            txt_Punti_Reward_14.Text = CurrentRewardPoints[13].ToString();
            txt_Reward_14.Text = CurrentRewardsNormali[13].ToString();
            txt_Reward_Vip_14.Text = CurrentRewardsVip[13].ToString();

            txt_Punti_Reward_15.Text = CurrentRewardPoints[14].ToString();
            txt_Reward_15.Text = CurrentRewardsNormali[14].ToString();
            txt_Reward_Vip_15.Text = CurrentRewardsVip[14].ToString();

            txt_Punti_Reward_16.Text = CurrentRewardPoints[15].ToString();
            txt_Reward_16.Text = CurrentRewardsNormali[15].ToString();
            txt_Reward_Vip_16.Text = CurrentRewardsVip[15].ToString();

            txt_Punti_Reward_17.Text = CurrentRewardPoints[16].ToString();
            txt_Reward_17.Text = CurrentRewardsNormali[16].ToString();
            txt_Reward_Vip_17.Text = CurrentRewardsVip[16].ToString();

            txt_Punti_Reward_18.Text = CurrentRewardPoints[17].ToString();
            txt_Reward_18.Text = CurrentRewardsNormali[17].ToString();
            txt_Reward_Vip_18.Text = CurrentRewardsVip[17].ToString();

            txt_Punti_Reward_19.Text = CurrentRewardPoints[18].ToString();
            txt_Reward_19.Text = CurrentRewardsNormali[18].ToString();
            txt_Reward_Vip_19.Text = CurrentRewardsVip[18].ToString();

            txt_Punti_Reward_20.Text = CurrentRewardPoints[19].ToString();
            txt_Reward_20.Text = CurrentRewardsNormali[19].ToString();
            txt_Reward_Vip_20.Text = CurrentRewardsVip[19].ToString();
        }

        private void Check_Unlock_Reward()
        {
            int point = Convert.ToInt32(Variabili_Client.Quest_Reward.Point_Montly.Points_1);

            if (point >= Convert.ToInt32(txt_Punti_Reward_1.Text))
            {
                btn_Reward_1.Enabled = true;
                btn_Reward_1.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_2.Text))
            {
                btn_Reward_2.Enabled = true;
                btn_Reward_2.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_3.Text))
            {
                btn_Reward_3.Enabled = true;
                btn_Reward_3.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_4.Text))
            {
                btn_Reward_4.Enabled = true;
                btn_Reward_4.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_5.Text))
            {
                btn_Reward_5.Enabled = true;
                btn_Reward_5.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_6.Text))
            {
                btn_Reward_6.Enabled = true;
                btn_Reward_6.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_7.Text))
            {
                btn_Reward_7.Enabled = true;
                btn_Reward_7.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_8.Text))
            {
                btn_Reward_8.Enabled = true;
                btn_Reward_8.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_9.Text))
            {
                btn_Reward_9.Enabled = true;
                btn_Reward_9.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_10.Text))
            {
                btn_Reward_10.Enabled = true;
                btn_Reward_10.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_11.Text))
            {
                btn_Reward_11.Enabled = true;
                btn_Reward_11.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_12.Text))
            {
                btn_Reward_12.Enabled = true;
                btn_Reward_12.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_13.Text))
            {
                btn_Reward_13.Enabled = true;
                btn_Reward_13.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_14.Text))
            {
                btn_Reward_14.Enabled = true;
                btn_Reward_14.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_15.Text))
            {
                btn_Reward_15.Enabled = true;
                btn_Reward_15.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_16.Text))
            {
                btn_Reward_16.Enabled = true;
                btn_Reward_16.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_17.Text))
            {
                btn_Reward_17.Enabled = true;
                btn_Reward_17.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_18.Text))
            {
                btn_Reward_18.Enabled = true;
                btn_Reward_18.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_19.Text))
            {
                btn_Reward_19.Enabled = true;
                btn_Reward_19.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_20.Text))
            {
                btn_Reward_20.Enabled = true;
                btn_Reward_20.BackColor = Color.FromArgb(6, 176, 37);
            }
        }
        private void Check_Unlock_Reward_Vip()
        {
            if (Variabili_Client.Utente.User_Vip == false) return; // Vip attivo?
            int point = Convert.ToInt32(Variabili_Client.Quest_Reward.Point_Montly.Points_1);

            if (point >= Convert.ToInt32(txt_Punti_Reward_1.Text))
            {
                btn_Reward_Vip_1.Enabled = true;
                btn_Reward_Vip_1.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_2.Text))
            {
                btn_Reward_Vip_2.Enabled = true;
                btn_Reward_Vip_2.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_3.Text))
            {
                btn_Reward_Vip_3.Enabled = true;
                btn_Reward_Vip_3.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_4.Text))
            {
                btn_Reward_Vip_4.Enabled = true;
                btn_Reward_Vip_4.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_5.Text))
            {
                btn_Reward_Vip_5.Enabled = true;
                btn_Reward_Vip_5.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_6.Text))
            {
                btn_Reward_Vip_6.Enabled = true;
                btn_Reward_Vip_6.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_7.Text))
            {
                btn_Reward_Vip_7.Enabled = true;
                btn_Reward_Vip_7.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_8.Text))
            {
                btn_Reward_Vip_8.Enabled = true;
                btn_Reward_Vip_8.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_9.Text))
            {
                btn_Reward_Vip_9.Enabled = true;
                btn_Reward_Vip_9.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_10.Text))
            {
                btn_Reward_Vip_10.Enabled = true;
                btn_Reward_Vip_10.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_11.Text))
            {
                btn_Reward_Vip_11.Enabled = true;
                btn_Reward_Vip_11.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_12.Text))
            {
                btn_Reward_Vip_12.Enabled = true;
                btn_Reward_Vip_12.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_13.Text))
            {
                btn_Reward_Vip_13.Enabled = true;
                btn_Reward_Vip_13.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_14.Text))
            {
                btn_Reward_Vip_14.Enabled = true;
                btn_Reward_Vip_14.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_15.Text))
            {
                btn_Reward_Vip_15.Enabled = true;
                btn_Reward_Vip_15.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_16.Text))
            {
                btn_Reward_Vip_16.Enabled = true;
                btn_Reward_Vip_16.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_17.Text))
            {
                btn_Reward_Vip_17.Enabled = true;
                btn_Reward_Vip_17.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_18.Text))
            {
                btn_Reward_Vip_18.Enabled = true;
                btn_Reward_Vip_18.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_19.Text))
            {
                btn_Reward_Vip_19.Enabled = true;
                btn_Reward_Vip_19.BackColor = Color.FromArgb(6, 176, 37);
            }
            if (point >= Convert.ToInt32(txt_Punti_Reward_20.Text))
            {
                btn_Reward_Vip_20.Enabled = true;
                btn_Reward_Vip_20.BackColor = Color.FromArgb(6, 176, 37);
            }
        }
        private void Update_Reward()
        {
            //Reward 1
            txt_Punti_Reward_1.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_1;
            txt_Reward_1.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_1;
            txt_Reward_Vip_1.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_1;
            btn_Reward_1.Enabled = false;
            btn_Reward_Vip_1.Enabled = false;

            //Reward 2
            txt_Punti_Reward_2.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_2;
            txt_Reward_2.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_2;
            txt_Reward_Vip_2.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_2;
            btn_Reward_2.Enabled = false;
            btn_Reward_Vip_2.Enabled = false;


            //Reward 3
            txt_Punti_Reward_3.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_3;
            txt_Reward_3.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_3;
            txt_Reward_Vip_3.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_3;
            btn_Reward_3.Enabled = false;
            btn_Reward_Vip_3.Enabled = false;

            //Reward 4
            txt_Punti_Reward_4.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_4;
            txt_Reward_4.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_4;
            txt_Reward_Vip_4.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_4;
            btn_Reward_4.Enabled = false;
            btn_Reward_Vip_4.Enabled = false;

            //Reward 5
            txt_Punti_Reward_5.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_5;
            txt_Reward_5.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_5;
            txt_Reward_Vip_5.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_5;
            btn_Reward_5.Enabled = false;
            btn_Reward_Vip_5.Enabled = false;

            //Reward 6
            txt_Punti_Reward_6.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_6;
            txt_Reward_6.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_6;
            txt_Reward_Vip_6.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_6;
            btn_Reward_6.Enabled = false;
            btn_Reward_Vip_6.Enabled = false;

            //Reward 7
            txt_Punti_Reward_7.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_7;
            txt_Reward_7.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_7;
            txt_Reward_Vip_7.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_7;
            btn_Reward_7.Enabled = false;
            btn_Reward_Vip_7.Enabled = false;

            //Reward 8
            txt_Punti_Reward_8.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_8;
            txt_Reward_8.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_8;
            txt_Reward_Vip_8.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_8;
            btn_Reward_8.Enabled = false;
            btn_Reward_Vip_8.Enabled = false;

            //Reward 9
            txt_Punti_Reward_9.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_9;
            txt_Reward_9.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_9;
            txt_Reward_Vip_9.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_9;
            btn_Reward_9.Enabled = false;
            btn_Reward_Vip_9.Enabled = false;

            //Reward 4
            txt_Punti_Reward_10.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_10;
            txt_Reward_10.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_10;
            txt_Reward_Vip_10.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_10;
            btn_Reward_10.Enabled = false;
            btn_Reward_Vip_10.Enabled = false;

            //Reward 11
            txt_Punti_Reward_11.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_11;
            txt_Reward_11.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_11;
            txt_Reward_Vip_11.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_11;
            btn_Reward_11.Enabled = false;
            btn_Reward_Vip_11.Enabled = false;

            //Reward 12
            txt_Punti_Reward_12.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_12;
            txt_Reward_12.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_12;
            txt_Reward_Vip_12.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_12;
            btn_Reward_12.Enabled = false;
            btn_Reward_Vip_12.Enabled = false;

            //Reward 13
            txt_Punti_Reward_13.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_13;
            txt_Reward_13.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_13;
            txt_Reward_Vip_13.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_13;
            btn_Reward_13.Enabled = false;
            btn_Reward_Vip_13.Enabled = false;

            //Reward 14
            txt_Punti_Reward_14.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_14;
            txt_Reward_14.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_14;
            txt_Reward_Vip_14.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_14;
            btn_Reward_14.Enabled = false;
            btn_Reward_Vip_14.Enabled = false;

            //Reward 15
            txt_Punti_Reward_15.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_15;
            txt_Reward_15.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_15;
            txt_Reward_Vip_15.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_15;
            btn_Reward_15.Enabled = false;
            btn_Reward_Vip_15.Enabled = false;

            //Reward 16
            txt_Punti_Reward_16.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_16;
            txt_Reward_16.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_16;
            txt_Reward_Vip_16.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_16;
            btn_Reward_16.Enabled = false;
            btn_Reward_Vip_16.Enabled = false;

            //Reward 17
            txt_Punti_Reward_17.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_17;
            txt_Reward_17.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_17;
            txt_Reward_Vip_17.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_17;
            btn_Reward_17.Enabled = false;
            btn_Reward_Vip_17.Enabled = false;

            //Reward 18
            txt_Punti_Reward_18.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_18;
            txt_Reward_18.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_18;
            txt_Reward_Vip_18.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_18;
            btn_Reward_18.Enabled = false;
            btn_Reward_Vip_18.Enabled = false;

            //Reward 19
            txt_Punti_Reward_19.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_19;
            txt_Reward_19.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_19;
            txt_Reward_Vip_19.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_19;
            btn_Reward_19.Enabled = false;
            btn_Reward_Vip_19.Enabled = false;

            //Reward 20
            txt_Punti_Reward_20.Text = Variabili_Client.Quest_Reward.Point_Montly.Points_20;
            txt_Reward_20.Text = Variabili_Client.Quest_Reward.Normali_Montly.Reward_20;
            txt_Reward_Vip_20.Text = Variabili_Client.Quest_Reward.Vip_Montly.Reward_20;
            btn_Reward_20.Enabled = false;
            btn_Reward_Vip_20.Enabled = false;
        }
        private void Update_Quest()
        {
            txt_Quest_Desc_2.Text = "[75 punti] Quest_Reward di prova";
            txt_Quest_2.Text = "0/4";
        }

        private void Btn_Costruzione_Click(object sender, EventArgs e)
        {
            SetProgressValue(5);
        }


        #region btn_Reward_Click F2P
        private void btn_Reward_1_Click(object sender, EventArgs e)
        {
            btn_Reward_1.Enabled = false;
            btn_Reward_1.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_2_Click(object sender, EventArgs e)
        {
            btn_Reward_2.Enabled = false;
            btn_Reward_2.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_3_Click(object sender, EventArgs e)
        {
            btn_Reward_3.Enabled = false;
            btn_Reward_3.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_4_Click(object sender, EventArgs e)
        {
            btn_Reward_4.Enabled = false;
            btn_Reward_4.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_5_Click(object sender, EventArgs e)
        {
            btn_Reward_5.Enabled = false;
            btn_Reward_5.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_6_Click(object sender, EventArgs e)
        {
            btn_Reward_6.Enabled = false;
            btn_Reward_6.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_7_Click(object sender, EventArgs e)
        {
            btn_Reward_7.Enabled = false;
            btn_Reward_7.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_8_Click(object sender, EventArgs e)
        {
            btn_Reward_8.Enabled = false;
            btn_Reward_8.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_9_Click(object sender, EventArgs e)
        {
            btn_Reward_9.Enabled = false;
            btn_Reward_9.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_10_Click(object sender, EventArgs e)
        {
            btn_Reward_10.Enabled = false;
            btn_Reward_10.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_11_Click(object sender, EventArgs e)
        {
            btn_Reward_11.Enabled = false;
            btn_Reward_11.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_12_Click(object sender, EventArgs e)
        {
            btn_Reward_12.Enabled = false;
            btn_Reward_12.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_13_Click(object sender, EventArgs e)
        {
            btn_Reward_13.Enabled = false;
            btn_Reward_13.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_14_Click(object sender, EventArgs e)
        {
            btn_Reward_14.Enabled = false;
            btn_Reward_14.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_15_Click(object sender, EventArgs e)
        {
            btn_Reward_15.Enabled = false;
            btn_Reward_15.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_16_Click(object sender, EventArgs e)
        {
            btn_Reward_16.Enabled = false;
            btn_Reward_16.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_17_Click(object sender, EventArgs e)
        {
            btn_Reward_17.Enabled = false;
            btn_Reward_17.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_18_Click(object sender, EventArgs e)
        {
            btn_Reward_18.Enabled = false;
            btn_Reward_18.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_19_Click(object sender, EventArgs e)
        {
            btn_Reward_19.Enabled = false;
            btn_Reward_19.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_20_Click(object sender, EventArgs e)
        {
            btn_Reward_20.Enabled = false;
            btn_Reward_20.BackColor = Color.FromArgb(55, 47, 36);
        }
        #endregion
        #region btn_Reward_Click VIP
        private void btn_Reward_Vip_1_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_1.Enabled = false;
            btn_Reward_Vip_1.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_2_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_2.Enabled = false;
            btn_Reward_Vip_2.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_3_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_3.Enabled = false;
            btn_Reward_Vip_3.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_4_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_4.Enabled = false;
            btn_Reward_Vip_4.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_5_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_5.Enabled = false;
            btn_Reward_Vip_5.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_6_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_6.Enabled = false;
            btn_Reward_Vip_6.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_7_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_7.Enabled = false;
            btn_Reward_Vip_7.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_8_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_8.Enabled = false;
            btn_Reward_Vip_8.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_9_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_9.Enabled = false;
            btn_Reward_Vip_9.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_10_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_10.Enabled = false;
            btn_Reward_Vip_10.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_11_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_11.Enabled = false;
            btn_Reward_Vip_11.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_12_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_12.Enabled = false;
            btn_Reward_Vip_12.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_13_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_13.Enabled = false;
            btn_Reward_Vip_13.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_14_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_14.Enabled = false;
            btn_Reward_Vip_14.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_15_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_15.Enabled = false;
            btn_Reward_Vip_15.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_16_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_16.Enabled = false;
            btn_Reward_Vip_16.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_17_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_17.Enabled = false;
            btn_Reward_Vip_17.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_18_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_18.Enabled = false;
            btn_Reward_Vip_18.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_19_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_19.Enabled = false;
            btn_Reward_Vip_19.BackColor = Color.FromArgb(55, 47, 36);
        }

        private void btn_Reward_Vip_20_Click(object sender, EventArgs e)
        {
            btn_Reward_Vip_20.Enabled = false;
            btn_Reward_Vip_20.BackColor = Color.FromArgb(55, 47, 36);
        }
        #endregion
    }
}
