using Strategico_V2;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;
using static Strategico_V2.ClientConnection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CriptoGame_Online
{
    public partial class MontlyQuest : Form
    {
        // Lista di Quest_Reward attuali
        public static List<ClientQuestData> CurrentQuests { get; set; } = new List<ClientQuestData>();
        int currentIndex = 0;

        public static List<int> CurrentRewardsNormali { get; set; } = new(); // Ricompense Normali
        public static List<int> CurrentRewardsVip { get; set; } = new(); // Ricompense VIP
        public static List<int> CurrentRewardPoints { get; set; } = new(); //Punti necessari per sbloccare le ricompense
        public static List<bool> CurrentRewardClaimNormal { get; set; } = new(); // Ricompense già ritirate
        public static List<bool> CurrentRewardClaimVip { get; set; } = new(); // Ricompense già ritirate

        private CancellationTokenSource cts = new CancellationTokenSource();
        public MontlyQuest()
        {
            InitializeComponent();
            this.ActiveControl = Btn_Costruzione; // assegna il focus al bottone
        }

        private void MontlyQuest_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            AggiornaInterfacciaRewards();
            AggiornaInterfacciaQuest();
            Task.Run(() => Gui_Update(cts.Token), cts.Token);
        }

        async void Gui_Update(CancellationToken token)
        {
            this.ActiveControl = label1;
            while (!token.IsCancellationRequested)
            {
                if (panel1.IsHandleCreated && !panel1.IsDisposed)
                {
                    panel1.BeginInvoke((Action)(() =>
                    {
                        AggiornaInterfacciaQuest();
                        Update_Reward();
                        Check_Unlock_Reward();
                        Check_Unlock_Reward_GamePass_Base();
                        this.ActiveControl = null;
                        progressBar1.Maximum = Convert.ToInt32(CurrentRewardPoints[19]);

                        int point = Convert.ToInt32(Variabili_Client.Utente.Montly_Quest_Point);
                        if (point > progressBar1.Maximum)
                            point = progressBar1.Maximum;
                        progressBar1.Value = (int)point;
                    }));
                }
                await Task.Delay(750); // meglio di Thread.Sleep
            }

        }
        void AggiornaInterfacciaQuest()
        {
            if (CurrentQuests.Count > 0)
            {
                int count = Math.Min(10, CurrentQuests.Count);
                for (int i = 0; i < count; i++)
                {
                    int questIndex = (currentIndex + i) % CurrentQuests.Count;
                    if (i == 0)
                    {
                        txt_Quest_Desc_1.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_1.Text = $"[{CurrentQuests[questIndex].Experience:#,0.}] Exp   {CurrentQuests[questIndex].Progress:#,0.}/{CurrentQuests[questIndex].Require:#,0.}";
                    }
                    if (i == 1)
                    {
                        txt_Quest_Desc_2.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_2.Text = $"[{CurrentQuests[questIndex].Experience:#,0.}] Exp   {CurrentQuests[questIndex].Progress:#,0.}/{CurrentQuests[questIndex].Require:#,0.}";
                    }
                    if (i == 2)
                    {
                        txt_Quest_Desc_3.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_3.Text = $"[{CurrentQuests[questIndex].Experience:#,0.}] Exp   {CurrentQuests[questIndex].Progress:#,0.}/{CurrentQuests[questIndex].Require:#,0.}";
                    }
                    if (i == 3)
                    {
                        txt_Quest_Desc_4.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_4.Text = $"[{CurrentQuests[questIndex].Experience:#,0.}] Exp   {CurrentQuests[questIndex].Progress:#,0.}/{CurrentQuests[questIndex].Require:#,0.}";
                    }
                    if (i == 4)
                    {
                        txt_Quest_Desc_5.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_5.Text = $"[{CurrentQuests[questIndex].Experience:#,0.}] Exp   {CurrentQuests[questIndex].Progress:#,0.}/{CurrentQuests[questIndex].Require:#,0.}";
                    }
                    if (i == 5)
                    {
                        txt_Quest_Desc_6.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_6.Text = $"[{CurrentQuests[questIndex].Experience:#,0.}] Exp   {CurrentQuests[questIndex].Progress:#,0.}/{CurrentQuests[questIndex].Require:#,0.}";
                    }
                    if (i == 6)
                    {
                        txt_Quest_Desc_7.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_7.Text = $"[{CurrentQuests[questIndex].Experience:#,0.}] Exp   {CurrentQuests[questIndex].Progress:#,0.}/{CurrentQuests[questIndex].Require:#,0.}";
                    }
                    if (i == 7)
                    {
                        txt_Quest_Desc_8.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_8.Text = $"[{CurrentQuests[questIndex].Experience:#,0.}] Exp   {CurrentQuests[questIndex].Progress:#,0.}/{CurrentQuests[questIndex].Require:#,0.}";
                    }
                    if (i == 8)
                    {
                        txt_Quest_Desc_9.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_9.Text = $"[{CurrentQuests[questIndex].Experience:#,0.}] Exp   {CurrentQuests[questIndex].Progress:#,0.}/{CurrentQuests[questIndex].Require:#,0.}";
                    }
                    if (i == 9)
                    {
                        txt_Quest_Desc_10.Text = CurrentQuests[questIndex].Quest_Description;
                        txt_Quest_10.Text = $"[{CurrentQuests[questIndex].Experience:#,0.}] Exp   {CurrentQuests[questIndex].Progress:#,0.}/{CurrentQuests[questIndex].Require:#,0.}";
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
            int point = Convert.ToInt32(Variabili_Client.Utente.Montly_Quest_Point);

            txt_Punti_Reward_1.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_2.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_3.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_4.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_5.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_6.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_7.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_8.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_9.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_10.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_11.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_12.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_13.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_14.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_15.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_16.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_17.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_18.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_19.BackColor = Color.FromArgb(15, 123, 15);
            txt_Punti_Reward_20.BackColor = Color.FromArgb(15, 123, 15);

            if (point >= CurrentRewardPoints[0])
                if (CurrentRewardClaimNormal[0] == false)
                {
                    btn_Reward_1.Enabled = true;
                    btn_Reward_1.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[0] == false)
                    btn_Reward_1.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_1.BackColor = Color.FromArgb(90, 80, 70);  
            else txt_Punti_Reward_1.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[1])
                if (CurrentRewardClaimNormal[1] == false && CurrentRewardClaimNormal[0] == true)
                {
                    btn_Reward_2.Enabled = true;
                    btn_Reward_2.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[1] == false)
                    btn_Reward_2.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_2.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_2.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[2])
                if (CurrentRewardClaimNormal[2] == false && CurrentRewardClaimNormal[1] == true)
                {
                    btn_Reward_3.Enabled = true;
                    btn_Reward_3.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[2] == false)
                    btn_Reward_3.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_3.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_3.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[3])
                if (CurrentRewardClaimNormal[3] == false && CurrentRewardClaimNormal[2] == true)
                {
                    btn_Reward_4.Enabled = true;
                    btn_Reward_4.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[3] == false)
                    btn_Reward_4.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_4.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_4.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[4])
                if (CurrentRewardClaimNormal[4] == false && CurrentRewardClaimNormal[3] == true)
                {
                    btn_Reward_5.Enabled = true;
                    btn_Reward_5.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[4] == false)
                    btn_Reward_5.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_5.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_5.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[5])
                if (CurrentRewardClaimNormal[5] == false && CurrentRewardClaimNormal[4] == true)
                {
                    btn_Reward_6.Enabled = true;
                    btn_Reward_6.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[5] == false)
                    btn_Reward_6.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_6.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_6.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[6])
                if (CurrentRewardClaimNormal[6] == false && CurrentRewardClaimNormal[5] == true)
                {
                    btn_Reward_7.Enabled = true;
                    btn_Reward_7.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[6] == false)
                    btn_Reward_7.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_7.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_7.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[7])
                if (CurrentRewardClaimNormal[7] == false && CurrentRewardClaimNormal[6] == true)
                {
                    btn_Reward_8.Enabled = true;
                    btn_Reward_8.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[7] == false)
                    btn_Reward_8.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_8.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_8.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[8])
                if (CurrentRewardClaimNormal[8] == false && CurrentRewardClaimNormal[7] == true)
                {
                    btn_Reward_9.Enabled = true;
                    btn_Reward_9.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[8] == false)
                    btn_Reward_9.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_9.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_9.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[9])
                if (CurrentRewardClaimNormal[9] == false && CurrentRewardClaimNormal[8] == true)
                {
                    btn_Reward_10.Enabled = true;
                    btn_Reward_10.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[9] == false)
                    btn_Reward_10.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_10.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_10.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[10])
                if (CurrentRewardClaimNormal[10] == false && CurrentRewardClaimNormal[9] == true)
                {
                    btn_Reward_11.Enabled = true;
                    btn_Reward_11.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[10] == false)
                    btn_Reward_11.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_11.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_11.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[11])
                if (CurrentRewardClaimNormal[11] == false && CurrentRewardClaimNormal[10] == true)
                {
                    btn_Reward_12.Enabled = true;
                    btn_Reward_12.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[11] == false)
                    btn_Reward_12.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_12.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_12.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[12])
                if (CurrentRewardClaimNormal[12] == false && CurrentRewardClaimNormal[11] == true)
                {
                    btn_Reward_13.Enabled = true;
                    btn_Reward_13.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[12] == false)
                    btn_Reward_13.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_13.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_13.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[13])
                if (CurrentRewardClaimNormal[13] == false && CurrentRewardClaimNormal[12] == true)
                {
                    btn_Reward_14.Enabled = true;
                    btn_Reward_14.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[13] == false)
                    btn_Reward_14.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_14.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_14.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[14])
                if (CurrentRewardClaimNormal[14] == false && CurrentRewardClaimNormal[13] == true)
                {
                    btn_Reward_15.Enabled = true;
                    btn_Reward_15.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[14] == false)
                    btn_Reward_15.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_15.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_15.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[15])
                if (CurrentRewardClaimNormal[15] == false && CurrentRewardClaimNormal[14] == true)
                {
                    btn_Reward_16.Enabled = true;
                    btn_Reward_16.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[15] == false)
                    btn_Reward_16.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_16.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_16.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[16])
                if (CurrentRewardClaimNormal[16] == false && CurrentRewardClaimNormal[15] == true)
                {
                    btn_Reward_17.Enabled = true;
                    btn_Reward_17.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[16] == false)
                    btn_Reward_17.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_17.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_17.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[17])
                if (CurrentRewardClaimNormal[17] == false && CurrentRewardClaimNormal[16] == true)
                {
                    btn_Reward_18.Enabled = true;
                    btn_Reward_18.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[17] == false)
                    btn_Reward_18.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_18.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_18.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[18])
                if (CurrentRewardClaimNormal[18] == false && CurrentRewardClaimNormal[17] == true)
                {
                    btn_Reward_19.Enabled = true;
                    btn_Reward_19.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[18] == false)
                    btn_Reward_19.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_19.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_19.BackColor = Color.FromArgb(230, 230, 230);

            if (point >= CurrentRewardPoints[19])
                if (CurrentRewardClaimNormal[19] == false && CurrentRewardClaimNormal[18] == true)
                {
                    btn_Reward_20.Enabled = true;
                    btn_Reward_20.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimNormal[19] == false)
                    btn_Reward_20.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_20.BackColor = Color.FromArgb(90, 80, 70);
            else txt_Punti_Reward_20.BackColor = Color.FromArgb(230, 230, 230);
        }
        private void Check_Unlock_Reward_GamePass_Base()
        {
            if (Variabili_Client.Utente.User_GamePass_Base == false) return; // GamePass base attivo?
            int point = Convert.ToInt32(Variabili_Client.Utente.Montly_Quest_Point);

            if (point >= CurrentRewardPoints[0])
                if (CurrentRewardClaimVip[0] == false)
                {
                    btn_Reward_Vip_1.Enabled = true;
                    btn_Reward_Vip_1.BackColor = Color.FromArgb(6, 176, 37);
                }
                else btn_Reward_Vip_1.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[1])
                if (CurrentRewardClaimVip[0] == true && CurrentRewardClaimVip[1] == false)
                {
                    btn_Reward_Vip_2.Enabled = true;
                    btn_Reward_Vip_2.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[1] == false)
                    btn_Reward_Vip_2.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_2.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[2])
                if (CurrentRewardClaimVip[1] == true && CurrentRewardClaimVip[2] == false)
                {
                    btn_Reward_Vip_3.Enabled = true;
                    btn_Reward_Vip_3.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[2] == false)
                    btn_Reward_Vip_3.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_3.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[3])
                if (CurrentRewardClaimVip[2] == true && CurrentRewardClaimVip[3] == false)
                {
                    btn_Reward_Vip_4.Enabled = true;
                    btn_Reward_Vip_4.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[3] == false)
                    btn_Reward_Vip_4.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_4.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[4])
                if (CurrentRewardClaimVip[3] == true && CurrentRewardClaimVip[4] == false)
                {
                    btn_Reward_Vip_5.Enabled = true;
                    btn_Reward_Vip_5.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[4] == false)
                    btn_Reward_Vip_5.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_5.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[5])
                if (CurrentRewardClaimVip[4] == true && CurrentRewardClaimVip[5] == false)
                {
                    btn_Reward_Vip_6.Enabled = true;
                    btn_Reward_Vip_6.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[5] == false)
                    btn_Reward_Vip_6.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_6.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[6])
                if (CurrentRewardClaimVip[5] == true && CurrentRewardClaimVip[6] == false)
                {
                    btn_Reward_Vip_7.Enabled = true;
                    btn_Reward_Vip_7.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[6] == false)
                    btn_Reward_Vip_7.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_7.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[7])
                if (CurrentRewardClaimVip[6] == true && CurrentRewardClaimVip[7] == false)
                {
                    btn_Reward_Vip_8.Enabled = true;
                    btn_Reward_Vip_8.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[7] == false)
                    btn_Reward_Vip_8.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_8.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[8])
                if (CurrentRewardClaimVip[7] == true && CurrentRewardClaimVip[8] == false)
                {
                    btn_Reward_Vip_9.Enabled = true;
                    btn_Reward_Vip_9.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[8] == false)
                    btn_Reward_Vip_9.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_9.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[9])
                if (CurrentRewardClaimVip[8] == true && CurrentRewardClaimVip[9] == false)
                {
                    btn_Reward_Vip_10.Enabled = true;
                    btn_Reward_Vip_10.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[9] == false)
                    btn_Reward_Vip_10.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_10.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[10])
                if (CurrentRewardClaimVip[9] == true && CurrentRewardClaimVip[10] == false)
                {
                    btn_Reward_Vip_11.Enabled = true;
                    btn_Reward_Vip_11.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[10] == false)
                    btn_Reward_Vip_11.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_11.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[11])
                if (CurrentRewardClaimVip[10] == true && CurrentRewardClaimVip[11] == false)
                {
                    btn_Reward_Vip_12.Enabled = true;
                    btn_Reward_Vip_12.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[11] == false)
                    btn_Reward_Vip_12.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_12.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[12])
                if (CurrentRewardClaimVip[11] == true && CurrentRewardClaimVip[12] == false)
                {
                    btn_Reward_Vip_13.Enabled = true;
                    btn_Reward_Vip_13.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[12] == false)
                    btn_Reward_Vip_13.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_13.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[13])
                if (CurrentRewardClaimVip[12] == true && CurrentRewardClaimVip[13] == false)
                {
                    btn_Reward_Vip_14.Enabled = true;
                    btn_Reward_Vip_14.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[13] == false)
                    btn_Reward_Vip_14.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_14.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[14])
                if (CurrentRewardClaimVip[13] == true && CurrentRewardClaimVip[14] == false)
                {
                    btn_Reward_Vip_15.Enabled = true;
                    btn_Reward_Vip_15.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[14] == false)
                    btn_Reward_Vip_15.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_15.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[15])
                if (CurrentRewardClaimVip[14] == true && CurrentRewardClaimVip[15] == false)
                {
                    btn_Reward_Vip_16.Enabled = true;
                    btn_Reward_Vip_16.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[15] == false)
                    btn_Reward_Vip_16.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_16.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[16])
                if (CurrentRewardClaimVip[15] == true && CurrentRewardClaimVip[16] == false)
                {
                    btn_Reward_Vip_17.Enabled = true;
                    btn_Reward_Vip_17.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[16] == false)
                    btn_Reward_Vip_17.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_17.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[17])
                if (CurrentRewardClaimVip[16] == true && CurrentRewardClaimVip[17] == false)
                {
                    btn_Reward_Vip_18.Enabled = true;
                    btn_Reward_Vip_18.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[17] == false)
                    btn_Reward_Vip_18.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_18.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[18])
                if (CurrentRewardClaimVip[17] == true && CurrentRewardClaimVip[18] == false)
                {
                    btn_Reward_Vip_19.Enabled = true;
                    btn_Reward_Vip_19.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[18] == false)
                    btn_Reward_Vip_19.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_19.BackColor = Color.FromArgb(90, 80, 70);

            if (point >= CurrentRewardPoints[19])
                if (CurrentRewardClaimVip[18] == true && CurrentRewardClaimVip[19] == false)
                {
                    btn_Reward_Vip_20.Enabled = true;
                    btn_Reward_Vip_20.BackColor = Color.FromArgb(6, 176, 37);
                }
                else if (CurrentRewardClaimVip[19] == false)
                    btn_Reward_Vip_20.BackColor = Color.FromArgb(55, 47, 36);
                else btn_Reward_Vip_20.BackColor = Color.FromArgb(90, 80, 70);
        }
        private void Update_Reward()
        {
            //Reward 1
            txt_Punti_Reward_1.Text = CurrentRewardPoints[0].ToString();
            txt_Reward_1.Text = CurrentRewardsNormali[0].ToString();
            txt_Reward_Vip_1.Text = CurrentRewardsVip[0].ToString();
            btn_Reward_1.Enabled = false;
            btn_Reward_Vip_1.Enabled = false;

            //Reward 2
            txt_Punti_Reward_2.Text = CurrentRewardPoints[1].ToString();
            txt_Reward_2.Text = CurrentRewardsNormali[1].ToString();
            txt_Reward_Vip_2.Text = CurrentRewardsVip[1].ToString();
            btn_Reward_2.Enabled = false;
            btn_Reward_Vip_2.Enabled = false;


            //Reward 3
            txt_Punti_Reward_3.Text = CurrentRewardPoints[2].ToString();
            txt_Reward_3.Text = CurrentRewardsNormali[2].ToString();
            txt_Reward_Vip_3.Text = CurrentRewardsVip[2].ToString();
            btn_Reward_3.Enabled = false;
            btn_Reward_Vip_3.Enabled = false;

            //Reward 4
            txt_Punti_Reward_4.Text = CurrentRewardPoints[3].ToString();
            txt_Reward_4.Text = CurrentRewardsNormali[3].ToString();
            txt_Reward_Vip_4.Text = CurrentRewardsVip[3].ToString();
            btn_Reward_4.Enabled = false;
            btn_Reward_Vip_4.Enabled = false;

            //Reward 5
            txt_Punti_Reward_5.Text = CurrentRewardPoints[4].ToString();
            txt_Reward_5.Text = CurrentRewardsNormali[4].ToString();
            txt_Reward_Vip_5.Text = CurrentRewardsVip[4].ToString();
            btn_Reward_5.Enabled = false;
            btn_Reward_Vip_5.Enabled = false;

            //Reward 6
            txt_Punti_Reward_6.Text = CurrentRewardPoints[5].ToString();
            txt_Reward_6.Text = CurrentRewardsNormali[5].ToString();
            txt_Reward_Vip_6.Text = CurrentRewardsVip[5].ToString();
            btn_Reward_6.Enabled = false;
            btn_Reward_Vip_6.Enabled = false;

            //Reward 7
            txt_Punti_Reward_7.Text = CurrentRewardPoints[6].ToString();
            txt_Reward_7.Text = CurrentRewardsNormali[6].ToString();
            txt_Reward_Vip_7.Text = CurrentRewardsVip[6].ToString();
            btn_Reward_7.Enabled = false;
            btn_Reward_Vip_7.Enabled = false;

            //Reward 8
            txt_Punti_Reward_8.Text = CurrentRewardPoints[7].ToString();
            txt_Reward_8.Text = CurrentRewardsNormali[7].ToString();
            txt_Reward_Vip_8.Text = CurrentRewardsVip[7].ToString();
            btn_Reward_8.Enabled = false;
            btn_Reward_Vip_8.Enabled = false;

            //Reward 9
            txt_Punti_Reward_9.Text = CurrentRewardPoints[8].ToString();
            txt_Reward_9.Text = CurrentRewardsNormali[8].ToString();
            txt_Reward_Vip_9.Text = CurrentRewardsVip[8].ToString();
            btn_Reward_9.Enabled = false;
            btn_Reward_Vip_9.Enabled = false;

            //Reward 4
            txt_Punti_Reward_10.Text = CurrentRewardPoints[9].ToString();
            txt_Reward_10.Text = CurrentRewardsNormali[9].ToString();
            txt_Reward_Vip_10.Text = CurrentRewardsVip[9].ToString();
            btn_Reward_10.Enabled = false;
            btn_Reward_Vip_10.Enabled = false;

            //Reward 11
            txt_Punti_Reward_11.Text = CurrentRewardPoints[10].ToString();
            txt_Reward_11.Text = CurrentRewardsNormali[10].ToString();
            txt_Reward_Vip_11.Text = CurrentRewardsVip[10].ToString();
            btn_Reward_11.Enabled = false;
            btn_Reward_Vip_11.Enabled = false;

            //Reward 12
            txt_Punti_Reward_12.Text = CurrentRewardPoints[11].ToString();
            txt_Reward_12.Text = CurrentRewardsNormali[11].ToString();
            txt_Reward_Vip_12.Text = CurrentRewardsVip[11].ToString();
            btn_Reward_12.Enabled = false;
            btn_Reward_Vip_12.Enabled = false;

            //Reward 13
            txt_Punti_Reward_13.Text = CurrentRewardPoints[12].ToString();
            txt_Reward_13.Text = CurrentRewardsNormali[12].ToString();
            txt_Reward_Vip_13.Text = CurrentRewardsVip[12].ToString();
            btn_Reward_13.Enabled = false;
            btn_Reward_Vip_13.Enabled = false;

            //Reward 14
            txt_Punti_Reward_14.Text = CurrentRewardPoints[13].ToString();
            txt_Reward_14.Text = CurrentRewardsNormali[13].ToString();
            txt_Reward_Vip_14.Text = CurrentRewardsVip[13].ToString();
            btn_Reward_14.Enabled = false;
            btn_Reward_Vip_14.Enabled = false;

            //Reward 15
            txt_Punti_Reward_15.Text = CurrentRewardPoints[14].ToString();
            txt_Reward_15.Text = CurrentRewardsNormali[14].ToString();
            txt_Reward_Vip_15.Text = CurrentRewardsVip[14].ToString();
            btn_Reward_15.Enabled = false;
            btn_Reward_Vip_15.Enabled = false;

            //Reward 16
            txt_Punti_Reward_16.Text = CurrentRewardPoints[15].ToString();
            txt_Reward_16.Text = CurrentRewardsNormali[15].ToString();
            txt_Reward_Vip_16.Text = CurrentRewardsVip[15].ToString();
            btn_Reward_16.Enabled = false;
            btn_Reward_Vip_16.Enabled = false;

            //Reward 17
            txt_Punti_Reward_17.Text = CurrentRewardPoints[16].ToString();
            txt_Reward_17.Text = CurrentRewardsNormali[16].ToString();
            txt_Reward_Vip_17.Text = CurrentRewardsVip[16].ToString();
            btn_Reward_17.Enabled = false;
            btn_Reward_Vip_17.Enabled = false;

            //Reward 18
            txt_Punti_Reward_18.Text = CurrentRewardPoints[17].ToString();
            txt_Reward_18.Text = CurrentRewardsNormali[17].ToString();
            txt_Reward_Vip_18.Text = CurrentRewardsVip[17].ToString();
            btn_Reward_18.Enabled = false;
            btn_Reward_Vip_18.Enabled = false;

            //Reward 19
            txt_Punti_Reward_19.Text = CurrentRewardPoints[18].ToString();
            txt_Reward_19.Text = CurrentRewardsNormali[18].ToString();
            txt_Reward_Vip_19.Text = CurrentRewardsVip[18].ToString();
            btn_Reward_19.Enabled = false;
            btn_Reward_Vip_19.Enabled = false;

            //Reward 20
            txt_Punti_Reward_20.Text = CurrentRewardPoints[19].ToString();
            txt_Reward_20.Text = CurrentRewardsNormali[19].ToString();
            txt_Reward_Vip_20.Text = CurrentRewardsVip[19].ToString();
            btn_Reward_20.Enabled = false;
            btn_Reward_Vip_20.Enabled = false;
        }

        private void Btn_Costruzione_Click(object sender, EventArgs e)
        {
            currentIndex = (currentIndex + 10) % CurrentQuests.Count; //Varia l'index per cambiare le Quest_Reward mostrate
            AggiornaInterfacciaQuest();
        }

        async void Scroll_Panel(int Valore)
        {
            Thread.Sleep(400); // Attendi 100 millisecondi
            if ((Variabili_Client.Utente.User_Vip == true && CurrentRewardClaimVip[Valore] == true && CurrentRewardClaimNormal[Valore] == true) ||
                (Variabili_Client.Utente.User_Vip == false && CurrentRewardClaimNormal[Valore] == true))
                panel1.AutoScrollPosition = new Point(Math.Abs(panel1.AutoScrollPosition.X) + 80); // Scorri di 100 pixel verso destra


            Check_Unlock_Reward();
            Check_Unlock_Reward_GamePass_Base();
            this.ActiveControl = null;
        }

        #region btn_Reward_Click F2P
        private async void btn_Reward_1_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[0] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|1");
            else btn_Reward_1.Enabled = false;

            Scroll_Panel(0);
        }

        private void btn_Reward_2_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[1] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|2");
            else btn_Reward_2.Enabled = false;

            Scroll_Panel(1);
        }

        private void btn_Reward_3_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[2] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|3");
            else btn_Reward_3.Enabled = false;

            Scroll_Panel(2);
        }

        private void btn_Reward_4_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[3] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|4");
            else btn_Reward_4.Enabled = false;

            Scroll_Panel(3);
        }

        private void btn_Reward_5_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[4] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|5");
            else btn_Reward_5.Enabled = false;

            Scroll_Panel(4);
        }

        private void btn_Reward_6_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[5] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|6");
            else btn_Reward_6.Enabled = false;

            Scroll_Panel(5);
        }

        private void btn_Reward_7_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[6] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|7");
            else btn_Reward_7.Enabled = false;

            Scroll_Panel(6);
        }

        private void btn_Reward_8_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[7] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|8");
            else btn_Reward_8.Enabled = false;

            Scroll_Panel(7);
        }

        private void btn_Reward_9_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[8] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|9");
            else btn_Reward_9.Enabled = false;

            Scroll_Panel(8);
        }

        private void btn_Reward_10_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[9] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|10");
            else btn_Reward_10.Enabled = false;

            Scroll_Panel(9);
        }

        private void btn_Reward_11_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[10] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|11");
            else btn_Reward_11.Enabled = false;

            Scroll_Panel(10);
        }

        private void btn_Reward_12_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[11] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|12");
            else btn_Reward_12.Enabled = false;

            Scroll_Panel(11);
        }

        private void btn_Reward_13_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[12] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|13");
            else btn_Reward_13.Enabled = false;

            Scroll_Panel(12);
        }

        private void btn_Reward_14_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[13] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|14");
            else btn_Reward_14.Enabled = false;

            Scroll_Panel(13);
        }

        private void btn_Reward_15_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[14] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|15");
            else btn_Reward_15.Enabled = false;

            Scroll_Panel(14);
        }

        private void btn_Reward_16_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[15] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|16");
            else btn_Reward_16.Enabled = false;

            Scroll_Panel(15);
        }

        private void btn_Reward_17_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[16] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|17");
            else btn_Reward_17.Enabled = false;

            Scroll_Panel(16);
        }

        private void btn_Reward_18_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[17] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|18");
            else btn_Reward_18.Enabled = false;

            Scroll_Panel(17);
        }

        private void btn_Reward_19_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[18] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|19");
            else btn_Reward_19.Enabled = false;

            Scroll_Panel(18);
        }

        private void btn_Reward_20_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimNormal[19] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Normale|20");
            else btn_Reward_20.Enabled = false;

            Scroll_Panel(19);
        }
        #endregion
        #region btn_Reward_Click VIP
        private void btn_Reward_Vip_1_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[0] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|1");
            else btn_Reward_Vip_1.Enabled = false;

            Scroll_Panel(0);
        }

        private void btn_Reward_Vip_2_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[1] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|2");
            else btn_Reward_Vip_2.Enabled = false;

            Scroll_Panel(1);
        }

        private void btn_Reward_Vip_3_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[2] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|3");
            else btn_Reward_Vip_3.Enabled = false;

            Scroll_Panel(2);
        }

        private void btn_Reward_Vip_4_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[3] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|4");
            else btn_Reward_Vip_4.Enabled = false;

            Scroll_Panel(3);
        }

        private void btn_Reward_Vip_5_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[4] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|5");
            else btn_Reward_Vip_5.Enabled = false;

            Scroll_Panel(4);
        }

        private void btn_Reward_Vip_6_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[5] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|6");
            else btn_Reward_Vip_6.Enabled = false;

            Scroll_Panel(5);
        }

        private void btn_Reward_Vip_7_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[6] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|7");
            else btn_Reward_Vip_7.Enabled = false;

            Scroll_Panel(6);
        }

        private void btn_Reward_Vip_8_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[7] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|8");
            else btn_Reward_Vip_8.Enabled = false;

            Scroll_Panel(7);
        }

        private void btn_Reward_Vip_9_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[8] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|9");
            else btn_Reward_Vip_9.Enabled = false;

            Scroll_Panel(8);
        }

        private void btn_Reward_Vip_10_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[9] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|10");
            else btn_Reward_Vip_10.Enabled = false;

            Scroll_Panel(9);
        }

        private void btn_Reward_Vip_11_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[10] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|11");
            else btn_Reward_Vip_11.Enabled = false;

            Scroll_Panel(10);
        }

        private void btn_Reward_Vip_12_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[11] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|12");
            else btn_Reward_Vip_12.Enabled = false;

            Scroll_Panel(11);
        }

        private void btn_Reward_Vip_13_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[12] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|13");
            else btn_Reward_Vip_13.Enabled = false;

            Scroll_Panel(12);
        }

        private void btn_Reward_Vip_14_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[14] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|14");
            else btn_Reward_Vip_14.Enabled = false;

            Scroll_Panel(13);
        }

        private void btn_Reward_Vip_15_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[14] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|15");
            else btn_Reward_Vip_15.Enabled = false;

            Scroll_Panel(14);
        }

        private void btn_Reward_Vip_16_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[15] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|16");
            else btn_Reward_Vip_16.Enabled = false;

            Scroll_Panel(15);
        }

        private void btn_Reward_Vip_17_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[16] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|17");
            else btn_Reward_Vip_17.Enabled = false;

            Scroll_Panel(16);
        }

        private void btn_Reward_Vip_18_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[17] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|18");
            else btn_Reward_Vip_18.Enabled = false;

            Scroll_Panel(17);
        }

        private void btn_Reward_Vip_19_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[18] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|19");
            else btn_Reward_Vip_19.Enabled = false;

            Scroll_Panel(18);
        }

        private void btn_Reward_Vip_20_Click(object sender, EventArgs e)
        {
            if (CurrentRewardClaimVip[19] == false)
                ClientConnection.TestClient.Send($"Quest_Reward|{Variabili_Client.Utente.Username}|{Variabili_Client.Utente.Password}|Vip|20");
            else btn_Reward_Vip_20.Enabled = false;

            Scroll_Panel(19);
        }
        #endregion

        private void MontlyQuest_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts.Cancel();
        }
    }
}
