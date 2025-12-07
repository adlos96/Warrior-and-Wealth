namespace Server_Strategico.Gioco
{
    internal class Esperienza
    {
        public static int exp_Level_Up = 240;
        public static int cosa = 0;
        public static double moltiplicatore = 0.62;

        public static async Task<int> LevelUp(Giocatori.Player player)
        {
            Moltiplicatore(player);
            int esperienza = 0;

            switch (player.Livello)
            {
                case 1:
                    if (player.Esperienza >= exp_Level_Up + exp_Level_Up * player.Livello * 0.35)
                    {
                        player.Esperienza -= Convert.ToInt32(exp_Level_Up + exp_Level_Up * player.Livello * 0.35);
                        player.Livello++;
                    }
                        esperienza = exp_Level_Up + Convert.ToInt32(exp_Level_Up * player.Livello * 0.35);
                    break;
                case 2:
                    if (player.Esperienza >= exp_Level_Up + exp_Level_Up * player.Livello * 0.40)
                    {
                        player.Esperienza -= Convert.ToInt32(exp_Level_Up + exp_Level_Up * player.Livello * 0.40);
                        player.Livello++;
                    }
                        esperienza = exp_Level_Up + Convert.ToInt32(exp_Level_Up * player.Livello * 0.40);
                    break;
                case 3:
                    if (player.Esperienza >= exp_Level_Up + exp_Level_Up * player.Livello * 0.45)
                    {
                        player.Esperienza -= Convert.ToInt32(exp_Level_Up + exp_Level_Up * player.Livello * 0.45);
                        player.Livello++;
                    }
                        esperienza = exp_Level_Up + Convert.ToInt32(exp_Level_Up * player.Livello * 0.45);
                    break;
                default:
                    if (player.Esperienza >= exp_Level_Up + exp_Level_Up * player.Livello * moltiplicatore)
                    {
                        player.Esperienza -= Convert.ToInt32(exp_Level_Up + exp_Level_Up * player.Livello * moltiplicatore);
                        player.Livello++;
                        if (player.Livello >= 10 && player.Livello < 11) cosa = 1;
                        else if (player.Livello >= 20 && player.Livello < 50) cosa = 2;
                        else if (player.Livello >= 50 && player.Livello < 80) cosa = 3;
                        else if (player.Livello >= 80 && player.Livello < 110) cosa = 4;
                        else if (player.Livello >= 110) moltiplicatore = 0.98;
                    }
                        esperienza = exp_Level_Up + Convert.ToInt32(exp_Level_Up * player.Livello * moltiplicatore);
                    break;
            }
            return esperienza;
        }
        public static void Moltiplicatore(Giocatori.Player player)
        {
            if (player.Livello >= 10 && player.Livello < 20 && cosa == 1)
            {
                moltiplicatore = 0.61;
                cosa = 0;
            }
            else if (player.Livello >= 20 && player.Livello < 40 && cosa == 2)
            {
                moltiplicatore = 0.67;
                cosa = 0;
            }
            else if (player.Livello >= 40 && cosa == 3)
            {
                moltiplicatore = 0.74;
                cosa = 0;
            }
            else if (player.Livello >= 40 && cosa == 4)
            {
                moltiplicatore = 0.81;
                cosa = 0;
            }
        }
    }
}
