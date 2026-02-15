using Server_Strategico.ServerData.Moduli;
using static Server_Strategico.Manager.QuestManager;

namespace Server_Strategico.Gioco
{
    internal class Esperienza
    {
        public static int exp_Level_Up = 240;
        public static int cosa = 0;
        public static double moltiplicatore = 0.52;

        public static int LevelUp(Giocatori.Player player)
        {
            int esperienza = 0;

            switch (player.Livello)
            {
                case 1:
                    moltiplicatore = 0.35;
                    break;
                case 2:
                    moltiplicatore = 0.40;
                    break;
                case 3:
                    moltiplicatore = 0.45;
                    break;
                default:
                    if (player.Livello >= 10 && player.Livello < 20) cosa = 1;
                    else if (player.Livello >= 20 && player.Livello < 50) cosa = 2;
                    else if (player.Livello >= 50 && player.Livello < 80) cosa = 3;
                    else if (player.Livello >= 80 && player.Livello < 110) cosa = 4;
                    else if (player.Livello >= 110) moltiplicatore = 0.98;

                    Moltiplicatore(player);
                    break;
            }
            esperienza = exp_Level_Up + (int)(exp_Level_Up * player.Livello * moltiplicatore);
            if (player.Esperienza >= esperienza)
            {
                player.Esperienza -= esperienza;
                OnEvent(player, QuestEventType.Livelli, "non serve", 1);
                player.Livello++;
                if (Server.Server.Client_Connessi.Contains(player.guid_Player) && Server.Server.Client_Connessi.Count() == 0) Descrizioni.DescUpdate(player);
            }
            if (player.Livello > player.QuestProgress.CurrentProgress[59]) OnEvent(player, QuestEventType.Livelli, "non serve", 1);
            return esperienza;
        }
        public static void Moltiplicatore(Giocatori.Player player)
        {
            if (cosa == 1) moltiplicatore = 0.58;
            else if (cosa == 2) moltiplicatore = 0.64;
            else if (cosa == 3) moltiplicatore = 0.71;
            else if (cosa == 4) moltiplicatore = 0.79;
            cosa = 0;
        }
    }
}
