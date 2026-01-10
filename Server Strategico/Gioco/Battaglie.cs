using System;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Server.Server;


namespace Server_Strategico.Gioco
{
    internal class Battaglie
    {
        public static async Task<bool> Battaglia_PVP(Player player, Guid clientGuid, Player player2, Guid clientGuid2)
        {
            await Battaglia_Distanza(player, clientGuid, player2, clientGuid2); //Pre battaglia, attaccano le unità a distanza ed i mezzi d'assedio

            int[] guerrieri = player.Guerrieri;                   //Giocatore attaccante
            int[] picchieri = player.Lanceri;                    //Giocatore attaccante
            int[] arcieri = player.Arceri;                        //Giocatore attaccante
            int[] catapulte = player.Catapulte;                   //Giocatore attaccante

            int[] guerrieri_Temp = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Temp = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Temp = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Temp = new int[] { 0, 0, 0, 0, 0 };

            int[] guerrieri_Temp_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Temp_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Temp_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Temp_Morti = new int[] { 0, 0, 0, 0, 0 };


            int[] guerrieri_Enemy = player2.Guerrieri;     //GIcoatore in difesa
            int[] picchieri_Enemy = player2.Lanceri;      //GIcoatore in difesa
            int[] arcieri_Enemy = player2.Arceri;          //GIcoatore in difesa
            int[] catapulte_Enemy = player2.Catapulte;     //GIcoatore in difesa

            int[] guerrieri_Temp_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Temp_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Temp_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Temp_Enemy = new int[] { 0, 0, 0, 0, 0 };

            int[] guerrieri_Temp_Enemy_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Temp_Enemy_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Temp_Enemy_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Temp_Enemy_Morti = new int[] { 0, 0, 0, 0, 0 };

            int tipi_Di_Unità = ContareTipiDiUnità(guerrieri, picchieri, arcieri, catapulte);
            int tipi_Di_Unità_Att = ContareTipiDiUnità(guerrieri_Enemy, picchieri_Enemy, arcieri_Enemy, catapulte_Enemy);

            // Calcolo del danno per il giocatore e il nemico
            double dannoInflitto = 0;
            double dannoInflittoDalNemico = 0; //Difensore

            dannoInflitto += CalcolareDanno_Giocatore(arcieri[0], catapulte[0], guerrieri[0], picchieri[0], player, clientGuid, 1) / tipi_Di_Unità_Att;
            dannoInflitto += CalcolareDanno_Giocatore(arcieri[1], catapulte[1], guerrieri[1], picchieri[1], player, clientGuid, 2) / tipi_Di_Unità_Att;
            dannoInflitto += CalcolareDanno_Giocatore(arcieri[2], catapulte[2], guerrieri[2], picchieri[2], player, clientGuid, 3) / tipi_Di_Unità_Att;
            dannoInflitto += CalcolareDanno_Giocatore(arcieri[3], catapulte[3], guerrieri[3], picchieri[3], player, clientGuid, 4) / tipi_Di_Unità_Att;
            dannoInflitto += CalcolareDanno_Giocatore(arcieri[4], catapulte[4], guerrieri[4], picchieri[4], player, clientGuid, 5) / tipi_Di_Unità_Att;

            dannoInflittoDalNemico += CalcolareDanno_Giocatore(arcieri[0], catapulte[0], guerrieri[0], picchieri[0], player, clientGuid, 1) / tipi_Di_Unità_Att; //Difensore
            dannoInflittoDalNemico += CalcolareDanno_Giocatore(arcieri[1], catapulte[1], guerrieri[1], picchieri[1], player, clientGuid, 2) / tipi_Di_Unità_Att;
            dannoInflittoDalNemico += CalcolareDanno_Giocatore(arcieri[2], catapulte[2], guerrieri[2], picchieri[2], player, clientGuid, 3) / tipi_Di_Unità_Att;
            dannoInflittoDalNemico += CalcolareDanno_Giocatore(arcieri[3], catapulte[3], guerrieri[3], picchieri[3], player, clientGuid, 4) / tipi_Di_Unità_Att;
            dannoInflittoDalNemico += CalcolareDanno_Giocatore(arcieri[4], catapulte[4], guerrieri[4], picchieri[4], player, clientGuid, 5) / tipi_Di_Unità_Att;

            // Applicare il danno alle unità del giocatore
            guerrieri_Temp[0] = RidurreNumeroSoldati(guerrieri[0], dannoInflitto, (Esercito.Unità.Guerriero_1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[0], Esercito.Unità.Guerriero_1.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp[0] = RidurreNumeroSoldati(picchieri[0], dannoInflitto, (Esercito.Unità.Lancere_1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[0], Esercito.Unità.Lancere_1.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp[0] = RidurreNumeroSoldati(arcieri[0], dannoInflitto * 0.70, (Esercito.Unità.Arcere_1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[0], Esercito.Unità.Arcere_1.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp[0] = RidurreNumeroSoldati(catapulte[0], dannoInflitto, (Esercito.Unità.Catapulta_1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[0], Esercito.Unità.Catapulta_1.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            guerrieri_Temp[1] = RidurreNumeroSoldati(guerrieri[1], dannoInflitto, (Esercito.Unità.Guerriero_2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[1], Esercito.Unità.Guerriero_2.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp[1] = RidurreNumeroSoldati(picchieri[1], dannoInflitto, (Esercito.Unità.Lancere_2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[1], Esercito.Unità.Lancere_2.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp[1] = RidurreNumeroSoldati(arcieri[1], dannoInflitto * 0.70, (Esercito.Unità.Arcere_2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[1], Esercito.Unità.Arcere_2.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp[1] = RidurreNumeroSoldati(catapulte[1], dannoInflitto, (Esercito.Unità.Catapulta_2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[1], Esercito.Unità.Catapulta_2.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            guerrieri_Temp[2] = RidurreNumeroSoldati(guerrieri[2], dannoInflitto, (Esercito.Unità.Guerriero_3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[2], Esercito.Unità.Guerriero_3.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp[2] = RidurreNumeroSoldati(picchieri[2], dannoInflitto, (Esercito.Unità.Lancere_3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[2], Esercito.Unità.Lancere_3.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp[2] = RidurreNumeroSoldati(arcieri[2], dannoInflitto * 0.70, (Esercito.Unità.Arcere_3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[2], Esercito.Unità.Arcere_3.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp[2] = RidurreNumeroSoldati(catapulte[2], dannoInflitto, (Esercito.Unità.Catapulta_3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[2], Esercito.Unità.Catapulta_3.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            guerrieri_Temp[3] = RidurreNumeroSoldati(guerrieri[3], dannoInflitto, (Esercito.Unità.Guerriero_4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[3], Esercito.Unità.Guerriero_4.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp[3] = RidurreNumeroSoldati(picchieri[3], dannoInflitto, (Esercito.Unità.Lancere_4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[3], Esercito.Unità.Lancere_4.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp[3] = RidurreNumeroSoldati(arcieri[3], dannoInflitto * 0.70, (Esercito.Unità.Arcere_4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[3], Esercito.Unità.Arcere_4.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp[3] = RidurreNumeroSoldati(catapulte[3], dannoInflitto, (Esercito.Unità.Catapulta_4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[3], Esercito.Unità.Catapulta_4.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            guerrieri_Temp[4] = RidurreNumeroSoldati(guerrieri[4], dannoInflitto, (Esercito.Unità.Guerriero_5.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[4], Esercito.Unità.Guerriero_5.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp[4] = RidurreNumeroSoldati(picchieri[4], dannoInflitto, (Esercito.Unità.Lancere_5.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[4], Esercito.Unità.Lancere_5.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp[4] = RidurreNumeroSoldati(arcieri[4], dannoInflitto * 0.70, (Esercito.Unità.Arcere_5.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[4], Esercito.Unità.Arcere_5.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp[4] = RidurreNumeroSoldati(catapulte[4], dannoInflitto, (Esercito.Unità.Catapulta_5.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[4], Esercito.Unità.Catapulta_5.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);


            // Applicare il danno alle unità nemiche
            // Applicare il danno alle unità del giocatore
            guerrieri_Temp_Enemy[0] = RidurreNumeroSoldati(guerrieri[0], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[0], Esercito.Unità.Guerriero_1.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp_Enemy[0] = RidurreNumeroSoldati(picchieri[0], dannoInflittoDalNemico, (Esercito.Unità.Lancere_1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[0], Esercito.Unità.Lancere_1.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp_Enemy[0] = RidurreNumeroSoldati(arcieri[0], dannoInflittoDalNemico * 0.70, (Esercito.Unità.Arcere_1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[0], Esercito.Unità.Arcere_1.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp_Enemy[0] = RidurreNumeroSoldati(catapulte[0], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[0], Esercito.Unità.Catapulta_1.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            guerrieri_Temp_Enemy[1] = RidurreNumeroSoldati(guerrieri[1], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[1], Esercito.Unità.Guerriero_2.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp_Enemy[1] = RidurreNumeroSoldati(picchieri[1], dannoInflittoDalNemico, (Esercito.Unità.Lancere_2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[1], Esercito.Unità.Lancere_2.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp_Enemy[1] = RidurreNumeroSoldati(arcieri[1], dannoInflittoDalNemico * 0.70, (Esercito.Unità.Arcere_2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[1], Esercito.Unità.Arcere_2.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp_Enemy[1] = RidurreNumeroSoldati(catapulte[1], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[1], Esercito.Unità.Catapulta_2.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            guerrieri_Temp_Enemy[2] = RidurreNumeroSoldati(guerrieri[2], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[2], Esercito.Unità.Guerriero_3.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp_Enemy[2] = RidurreNumeroSoldati(picchieri[2], dannoInflittoDalNemico, (Esercito.Unità.Lancere_3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[2], Esercito.Unità.Lancere_3.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp_Enemy[2] = RidurreNumeroSoldati(arcieri[2], dannoInflittoDalNemico * 0.70, (Esercito.Unità.Arcere_3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[2], Esercito.Unità.Arcere_3.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp_Enemy[2] = RidurreNumeroSoldati(catapulte[2], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[2], Esercito.Unità.Catapulta_3.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            guerrieri_Temp_Enemy[3] = RidurreNumeroSoldati(guerrieri[3], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[3], Esercito.Unità.Guerriero_4.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp_Enemy[3] = RidurreNumeroSoldati(picchieri[3], dannoInflittoDalNemico, (Esercito.Unità.Lancere_4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[3], Esercito.Unità.Lancere_4.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp_Enemy[3] = RidurreNumeroSoldati(arcieri[3], dannoInflittoDalNemico * 0.70, (Esercito.Unità.Arcere_4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[3], Esercito.Unità.Arcere_4.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp_Enemy[3] = RidurreNumeroSoldati(catapulte[3], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[3], Esercito.Unità.Catapulta_4.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            guerrieri_Temp_Enemy[4] = RidurreNumeroSoldati(guerrieri[4], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_5.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[4], Esercito.Unità.Guerriero_5.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp_Enemy[4] = RidurreNumeroSoldati(picchieri[4], dannoInflittoDalNemico, (Esercito.Unità.Lancere_5.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[4], Esercito.Unità.Lancere_5.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp_Enemy[4] = RidurreNumeroSoldati(arcieri[4], dannoInflittoDalNemico * 0.70, (Esercito.Unità.Arcere_5.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[4], Esercito.Unità.Arcere_5.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp_Enemy[4] = RidurreNumeroSoldati(catapulte[4], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_5.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[4], Esercito.Unità.Catapulta_5.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            int esperienza1 = 0;
            int esperienza2 = 0;
            for (int i = 0; i < 5; i++)
            {
                //Giocatore
                if (guerrieri[i] > 0) guerrieri_Temp_Morti[i] = guerrieri[i] - guerrieri_Temp[i];
                if (guerrieri[i] > 0) picchieri_Temp_Morti[i] = picchieri[i] - picchieri_Temp[i];
                if (arcieri[i] > 0) arcieri_Temp_Morti[i] = arcieri[i] - arcieri_Temp[i];
                if (catapulte[i] > 0) catapulte_Temp_Morti[i] = catapulte[i] - catapulte_Temp[i];
                //Barbaro (Villaggio/Città)
                if (guerrieri_Enemy[i] > 0) guerrieri_Temp_Enemy_Morti[i] = guerrieri_Enemy[i] - guerrieri_Temp_Enemy[i];
                if (guerrieri_Enemy[i] > 0) picchieri_Temp_Enemy_Morti[i] = picchieri_Enemy[i] - picchieri_Temp_Enemy[i];
                if (arcieri_Enemy[i] > 0) arcieri_Temp_Enemy_Morti[i] = arcieri_Enemy[i] - arcieri_Temp_Enemy[i];
                if (catapulte_Enemy[i] > 0) catapulte_Temp_Enemy_Morti[i] = catapulte_Enemy[i] - catapulte_Temp_Enemy[i];
            }

            Send(clientGuid, $"Log_Server|Danno inflitto dal giocatore [{player.Username}]: {(dannoInflitto * tipi_Di_Unità_Att++).ToString("0.00")}\r\n");
            Send(clientGuid, $"Log_Server|Danno inflitto dal giocatore [{player2.Username}]: {(dannoInflittoDalNemico * tipi_Di_Unità++).ToString("0.00")}");

            Send(clientGuid, $"Log_Server|" +
                $"Guerrieri: {guerrieri_Temp_Morti[0]}/{guerrieri[0]} lv1 {guerrieri_Temp_Morti[1]}/{guerrieri[1]} lv2 {guerrieri_Temp_Morti[2]}/{guerrieri[2]} lv3 {guerrieri_Temp_Morti[3]}/{guerrieri[3]} lv4 {guerrieri_Temp_Morti[4]}/{guerrieri[4]} lv5\r\n " +
                $"Lancieri: {picchieri_Temp_Morti[0]}/{picchieri[0]} lv1 {picchieri_Temp_Morti[1]}/{picchieri[1]} lv2 {picchieri_Temp_Morti[2]}/{picchieri[2]} lv3 {picchieri_Temp_Morti[3]}/{picchieri[3]} lv4 {picchieri_Temp_Morti[4]}/{picchieri[4]} lv5\r\n " +
                $"Arcieri: {arcieri_Temp_Morti}/{arcieri[0]} lv1 {arcieri_Temp_Morti[1]}/{arcieri[1]} lv2 {arcieri_Temp_Morti[2]}/{arcieri[2]} lv3 {arcieri_Temp_Morti[3]}/{arcieri[3]} lv4 {arcieri_Temp_Morti[4]}/{arcieri[4]} lv5\r\n " +
                $"Catapulte: {catapulte_Temp_Morti[0]}/{catapulte[0]} lv1 {catapulte_Temp_Morti[1]}/{catapulte[1]} lv2 {catapulte_Temp_Morti[2]}/{catapulte[2]} lv3 {catapulte_Temp_Morti[3]}/{catapulte[3]} lv4 {catapulte_Temp_Morti[4]}/{catapulte[4]} lv5\r\n");
            Send(clientGuid, $"Log_Server|Soldati persi dal giocatore [{player.Username}]:");

            Send(clientGuid2, $"Log_Server|Danno inflitto dal giocatore [{player.Username}]: {(dannoInflitto * tipi_Di_Unità_Att++).ToString("0.00")}\r\n");
            Send(clientGuid2, $"Log_Server|Danno inflitto dal giocatore [{player2.Username}]: {(dannoInflittoDalNemico * tipi_Di_Unità++).ToString("0.00")}");

            Send(clientGuid2, $"Log_Server|" +
                $"Guerrieri: {guerrieri_Temp_Enemy_Morti[0]}/{guerrieri_Enemy[0]} lv1 {guerrieri_Temp_Enemy_Morti[1]}/{guerrieri_Enemy[1]} lv2 {guerrieri_Temp_Enemy_Morti[2]}/{guerrieri_Enemy[2]} lv3 {guerrieri_Temp_Enemy_Morti[3]}/{guerrieri_Enemy[3]} lv4 {guerrieri_Temp_Enemy_Morti[4]}/{guerrieri_Enemy[4]} lv5\r\n " +
                $"Lancieri: {picchieri_Temp_Enemy_Morti[0]}/{picchieri_Enemy[0]} lv1 {picchieri_Temp_Enemy_Morti[1]}/{picchieri_Enemy[1]} lv2 {picchieri_Temp_Enemy_Morti[2]}/{picchieri_Enemy[2]} lv3 {picchieri_Temp_Enemy_Morti[3]}/{picchieri_Enemy[3]} lv4 {picchieri_Temp_Enemy_Morti[4]}/{picchieri_Enemy[4]} lv5\r\n " +
                $"Arcieri: {arcieri_Temp_Enemy_Morti}/{arcieri_Enemy[0]} lv1 {arcieri_Temp_Morti[1]}/{arcieri[1]} lv2 {arcieri_Temp_Enemy_Morti[2]}/{arcieri_Enemy[2]} lv3 {arcieri_Temp_Enemy_Morti[3]}/{arcieri_Enemy[3]} lv4 {arcieri_Temp_Enemy_Morti[4]}/{arcieri_Enemy[4]} lv5\r\n " +
                $"Catapulte: {catapulte_Temp_Enemy_Morti[0]}/{catapulte_Enemy[0]} lv1 {catapulte_Temp_Enemy_Morti[1]}/{catapulte_Enemy[1]} lv2 {catapulte_Temp_Morti[2]}/{catapulte_Enemy[2]} lv3 {catapulte_Temp_Morti[3]}/{catapulte_Enemy[3]} lv4 {catapulte_Temp_Morti[4]}/{catapulte_Enemy[4]} lv5\r\n");
            Send(clientGuid2, $"Log_Server|Soldati persi dal giocatore [{player2.Username}]:");


            esperienza1 += guerrieri_Temp_Enemy_Morti[0] * Esercito.EsercitoNemico.Guerrieri_1.Esperienza +
                                 picchieri_Temp_Enemy_Morti[0] * Esercito.EsercitoNemico.Lanceri_1.Esperienza +
                                 arcieri_Temp_Enemy_Morti[0] * Esercito.EsercitoNemico.Arceri_1.Esperienza +
                                 catapulte_Temp_Enemy_Morti[0] * Esercito.EsercitoNemico.Arceri_1.Esperienza;

            esperienza1 += guerrieri_Temp_Enemy_Morti[1] * Esercito.EsercitoNemico.Guerrieri_2.Esperienza +
                                 picchieri_Temp_Enemy_Morti[1] * Esercito.EsercitoNemico.Lanceri_2.Esperienza +
                                 arcieri_Temp_Enemy_Morti[1] * Esercito.EsercitoNemico.Arceri_2.Esperienza +
                                 catapulte_Temp_Enemy_Morti[1] * Esercito.EsercitoNemico.Arceri_2.Esperienza;

            esperienza1 += guerrieri_Temp_Enemy_Morti[2] * Esercito.EsercitoNemico.Guerrieri_3.Esperienza +
                                 picchieri_Temp_Enemy_Morti[2] * Esercito.EsercitoNemico.Lanceri_3.Esperienza +
                                 arcieri_Temp_Enemy_Morti[2] * Esercito.EsercitoNemico.Arceri_3.Esperienza +
                                 catapulte_Temp_Enemy_Morti[2] * Esercito.EsercitoNemico.Arceri_3.Esperienza;

            esperienza1 += guerrieri_Temp_Enemy_Morti[3] * Esercito.EsercitoNemico.Guerrieri_4.Esperienza +
                                 picchieri_Temp_Enemy_Morti[3] * Esercito.EsercitoNemico.Lanceri_4.Esperienza +
                                 arcieri_Temp_Enemy_Morti[3] * Esercito.EsercitoNemico.Arceri_4.Esperienza +
                                 catapulte_Temp_Enemy_Morti[3] * Esercito.EsercitoNemico.Arceri_4.Esperienza;

            esperienza1 += guerrieri_Temp_Enemy_Morti[4] * Esercito.EsercitoNemico.Guerrieri_5.Esperienza +
                                 picchieri_Temp_Enemy_Morti[4] * Esercito.EsercitoNemico.Lanceri_5.Esperienza +
                                 arcieri_Temp_Enemy_Morti[4] * Esercito.EsercitoNemico.Arceri_5.Esperienza +
                                 catapulte_Temp_Enemy_Morti[4] * Esercito.EsercitoNemico.Arceri_5.Esperienza;

            esperienza2 += guerrieri_Temp_Morti[0] * Esercito.EsercitoNemico.Guerrieri_1.Esperienza +
                                 picchieri_Temp_Morti[0] * Esercito.EsercitoNemico.Lanceri_1.Esperienza +
                                 arcieri_Temp_Morti[0] * Esercito.EsercitoNemico.Arceri_1.Esperienza +
                                 catapulte_Temp_Morti[0] * Esercito.EsercitoNemico.Arceri_1.Esperienza;

            esperienza2 += guerrieri_Temp_Morti[1] * Esercito.EsercitoNemico.Guerrieri_2.Esperienza +
                                 picchieri_Temp_Morti[1] * Esercito.EsercitoNemico.Lanceri_2.Esperienza +
                                 arcieri_Temp_Morti[1] * Esercito.EsercitoNemico.Arceri_2.Esperienza +
                                 catapulte_Temp_Morti[1] * Esercito.EsercitoNemico.Arceri_2.Esperienza;

            esperienza2 += guerrieri_Temp_Morti[2] * Esercito.EsercitoNemico.Guerrieri_3.Esperienza +
                                 picchieri_Temp_Morti[2] * Esercito.EsercitoNemico.Lanceri_3.Esperienza +
                                 arcieri_Temp_Morti[2] * Esercito.EsercitoNemico.Arceri_3.Esperienza +
                                 catapulte_Temp_Morti[2] * Esercito.EsercitoNemico.Arceri_3.Esperienza;

            esperienza2 += guerrieri_Temp_Morti[3] * Esercito.EsercitoNemico.Guerrieri_4.Esperienza +
                                 picchieri_Temp_Morti[3] * Esercito.EsercitoNemico.Lanceri_4.Esperienza +
                                 arcieri_Temp_Morti[3] * Esercito.EsercitoNemico.Arceri_4.Esperienza +
                                 catapulte_Temp_Morti[3] * Esercito.EsercitoNemico.Arceri_4.Esperienza;

            esperienza2 += guerrieri_Temp_Morti[4] * Esercito.EsercitoNemico.Guerrieri_5.Esperienza +
                                 picchieri_Temp_Morti[4] * Esercito.EsercitoNemico.Lanceri_5.Esperienza +
                                 arcieri_Temp_Morti[4] * Esercito.EsercitoNemico.Arceri_5.Esperienza +
                                 catapulte_Temp_Morti[4] * Esercito.EsercitoNemico.Arceri_5.Esperienza;

            player.Esperienza += esperienza1;
            player2.Esperienza += esperienza2;

            Send(clientGuid, $"Log_Server|Esperienza ottenuta: [{esperienza1}] exp");
            Send(clientGuid2, $"Log_Server|Esperienza ottenuta: [{esperienza2}] exp");

            Send(clientGuid2, $"Log_Server|Battaglia PVP Completata\r\n");

            Console.WriteLine($"Danno inflitto dal giocatore [{player.Username}]: {(dannoInflitto * tipi_Di_Unità_Att++).ToString("0.00")}");
            Console.WriteLine($"Danno inflitto dal giocatore [{player2.Username}]: {(dannoInflittoDalNemico * tipi_Di_Unità++).ToString("0.00")}");

            Console.WriteLine($"Soldati persi dal giocatore [{player.Username}]:");
            Console.WriteLine($"" +
                $"Guerrieri: {guerrieri_Temp_Morti[0]}/{guerrieri[0]} lv1 {guerrieri_Temp_Morti[1]}/{guerrieri[1]} lv2 {guerrieri_Temp_Morti[2]}/{guerrieri[2]} lv3 {guerrieri_Temp_Morti[3]}/{guerrieri[3]} lv4 {guerrieri_Temp_Morti[4]}/{guerrieri[4]} lv5\r\n " +
                $"Lancieri: {picchieri_Temp_Morti[0]}/{picchieri[0]} lv1 {picchieri_Temp_Morti[1]}/{picchieri[1]} lv2 {picchieri_Temp_Morti[2]}/{picchieri[2]} lv3 {picchieri_Temp_Morti[3]}/{picchieri[3]} lv4 {picchieri_Temp_Morti[4]}/{picchieri[4]} lv5\r\n " +
                $"Arcieri: {arcieri_Temp_Morti}/{arcieri[0]} lv1 {arcieri_Temp_Morti[1]}/{arcieri[1]} lv2 {arcieri_Temp_Morti[2]}/{arcieri[2]} lv3 {arcieri_Temp_Morti[3]}/{arcieri[3]} lv4 {arcieri_Temp_Morti[4]}/{arcieri[4]} lv5\r\n " +
                $"Catapulte: {catapulte_Temp_Morti[0]}/{catapulte[0]} lv1 {catapulte_Temp_Morti[1]}/{catapulte[1]} lv2 {catapulte_Temp_Morti[2]}/{catapulte[2]} lv3 {catapulte_Temp_Morti[3]}/{catapulte[3]} lv4 {catapulte_Temp_Morti[4]}/{catapulte[4]} lv5\r\n");

            Console.WriteLine($"Soldati persi dal giocatore [{player2.Username}]:");
            Console.WriteLine($"" +
                $"Guerrieri: {guerrieri_Temp_Enemy_Morti[0]}/{guerrieri_Enemy[0]} lv1 {guerrieri_Temp_Enemy_Morti[1]}/{guerrieri_Enemy[1]} lv2 {guerrieri_Temp_Enemy_Morti[2]}/{guerrieri_Enemy[2]} lv3 {guerrieri_Temp_Enemy_Morti[3]}/{guerrieri_Enemy[3]} lv4 {guerrieri_Temp_Enemy_Morti[4]}/{guerrieri_Enemy[4]} lv5\r\n " +
                $"Lancieri: {picchieri_Temp_Enemy_Morti[0]}/{picchieri_Enemy[0]} lv1 {picchieri_Temp_Enemy_Morti[1]}/{picchieri_Enemy[1]} lv2 {picchieri_Temp_Enemy_Morti[2]}/{picchieri_Enemy[2]} lv3 {picchieri_Temp_Enemy_Morti[3]}/{picchieri_Enemy[3]} lv4 {picchieri_Temp_Enemy_Morti[4]}/{picchieri_Enemy[4]} lv5\r\n " +
                $"Arcieri: {arcieri_Temp_Enemy_Morti}/{arcieri_Enemy[0]} lv1 {arcieri_Temp_Morti[1]}/{arcieri[1]} lv2 {arcieri_Temp_Enemy_Morti[2]}/{arcieri_Enemy[2]} lv3 {arcieri_Temp_Enemy_Morti[3]}/{arcieri_Enemy[3]} lv4 {arcieri_Temp_Enemy_Morti[4]}/{arcieri_Enemy[4]} lv5\r\n " +
                $"Catapulte: {catapulte_Temp_Enemy_Morti[0]}/{catapulte_Enemy[0]} lv1 {catapulte_Temp_Enemy_Morti[1]}/{catapulte_Enemy[1]} lv2 {catapulte_Temp_Morti[2]}/{catapulte_Enemy[2]} lv3 {catapulte_Temp_Morti[3]}/{catapulte_Enemy[3]} lv4 {catapulte_Temp_Morti[4]}/{catapulte_Enemy[4]} lv5\r\n");
            Console.WriteLine($"Battaglia PVP Completata");

            // Aggiornare le quantità delle unità
            player.Guerrieri = guerrieri_Temp;
            player.Lanceri = picchieri_Temp;
            player.Arceri = arcieri_Temp;
            player.Catapulte = catapulte_Temp;

            player2.Guerrieri = guerrieri_Temp_Enemy;
            player2.Lanceri = picchieri_Temp_Enemy;
            player2.Arceri = arcieri_Temp_Enemy;
            player2.Catapulte = catapulte_Temp_Enemy;
            return false;
        }
        public static async Task<bool> Battaglia_Barbari(Player player, Guid clientGuid, string tipo, string livello)
        {
            //await Battaglia_Distanza(tipo, player, clientGuid, livello); //Pre battaglia, attaccano le unità a distanza ed i mezzi d'assedio

            //Giocatore
            int[] guerrieri   = player.Guerrieri;
            int[] picchieri   = player.Lanceri;
            int[] arcieri     = player.Arceri;
            int[] catapulte   = player.Catapulte;

            int[] guerrieri_Temp = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Temp = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Temp = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Temp = new int[] { 0, 0, 0, 0, 0 };

            int[] guerrieri_Temp_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Temp_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Temp_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Temp_Morti = new int[] { 0, 0, 0, 0, 0 };

            //Barbaro
            int[] guerrieri_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Enemy = new int[] { 0, 0, 0, 0, 0 };

            int[] guerrieri_Temp_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Temp_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Temp_Enemy = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Temp_Enemy = new int[] { 0, 0, 0, 0, 0 };

            int[] guerrieri_Temp_Enemy_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] picchieri_Temp_Enemy_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] arcieri_Temp_Enemy_Morti = new int[] { 0, 0, 0, 0, 0 };
            int[] catapulte_Temp_Enemy_Morti = new int[] { 0, 0, 0, 0, 0 };

            var citta = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == 1); // Supponiamo che stiamo attaccando la città di livello 1
            var vilaggio = player.VillaggiPersonali.FirstOrDefault(c => c.Livello == 1); // Supponiamo che stiamo attaccando la città di livello 1
            int liv = Convert.ToInt32(livello);

            if (liv >= 1 && liv <= 4)
            {
                guerrieri_Enemy[0] = citta.Guerrieri;
                picchieri_Enemy[0] = citta.Lancieri;
                arcieri_Enemy[0] = citta.Arcieri;
                catapulte_Enemy[0] = citta.Catapulte;
            }
            if (liv > 4 && liv <= 8)
            {
                guerrieri_Enemy[1] = citta.Guerrieri;
                picchieri_Enemy[1] = citta.Lancieri;
                arcieri_Enemy[1] = citta.Arcieri;
                catapulte_Enemy[1] = citta.Catapulte;
            }
            if (liv > 8 && liv <= 12)
            {
                guerrieri_Enemy[2] = citta.Guerrieri;
                picchieri_Enemy[2] = citta.Lancieri;
                arcieri_Enemy[2] = citta.Arcieri;
                catapulte_Enemy[2] = citta.Catapulte;
            }
            if (liv > 12 && liv <= 16)
            {
                guerrieri_Enemy[3] = citta.Guerrieri;
                picchieri_Enemy[3] = citta.Lancieri;
                arcieri_Enemy[3] = citta.Arcieri;
                catapulte_Enemy[3] = citta.Catapulte;
            }
            if (liv > 16 && liv <= 20)
            {
                guerrieri_Enemy[4] = citta.Guerrieri;
                picchieri_Enemy[4] = citta.Lancieri;
                arcieri_Enemy[4] = citta.Arcieri;
                catapulte_Enemy[4] = citta.Catapulte;
            }

            int tipi_Di_Unità = ContareTipiDiUnità(guerrieri, picchieri, arcieri, catapulte);
            int tipi_Di_Unità_Att = ContareTipiDiUnità(guerrieri_Enemy, picchieri_Enemy, arcieri_Enemy, catapulte_Enemy);

            // Calcolo del danno per il giocatore e il nemico
            double dannoInflittoDalNemico = CalcolareDanno_Invasore(arcieri_Enemy, catapulte_Enemy, guerrieri_Enemy, picchieri_Enemy, player) / tipi_Di_Unità;
            double dannoInflitto = 0;

            dannoInflitto += CalcolareDanno_Giocatore(arcieri[0], catapulte[0], guerrieri[0], picchieri[0], player, clientGuid, 1) / tipi_Di_Unità_Att;
            dannoInflitto += CalcolareDanno_Giocatore(arcieri[1], catapulte[1], guerrieri[1], picchieri[1], player, clientGuid, 2) / tipi_Di_Unità_Att;
            dannoInflitto += CalcolareDanno_Giocatore(arcieri[2], catapulte[2], guerrieri[2], picchieri[2], player, clientGuid, 3) / tipi_Di_Unità_Att;
            dannoInflitto += CalcolareDanno_Giocatore(arcieri[3], catapulte[3], guerrieri[3], picchieri[3], player, clientGuid, 4) / tipi_Di_Unità_Att;
            dannoInflitto += CalcolareDanno_Giocatore(arcieri[4], catapulte[4], guerrieri[4], picchieri[4], player, clientGuid, 5) / tipi_Di_Unità_Att;

            // Applicare il danno alle unità del giocatore
            guerrieri_Temp[0]   = RidurreNumeroSoldati(guerrieri[0], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[0], Esercito.Unità.Guerriero_1.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp[0]   = RidurreNumeroSoldati(picchieri[0], dannoInflittoDalNemico, (Esercito.Unità.Lancere_1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[0], Esercito.Unità.Lancere_1.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp[0]     = RidurreNumeroSoldati(arcieri[0], dannoInflittoDalNemico, (Esercito.Unità.Arcere_1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[0], Esercito.Unità.Arcere_1.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp[0]   = RidurreNumeroSoldati(catapulte[0], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_1.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[0], Esercito.Unità.Catapulta_1.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            guerrieri_Temp[1]   = RidurreNumeroSoldati(guerrieri[1], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[1], Esercito.Unità.Guerriero_2.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp[1] = RidurreNumeroSoldati(picchieri[1], dannoInflittoDalNemico, (Esercito.Unità.Lancere_2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[1], Esercito.Unità.Lancere_2.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp[1]  = RidurreNumeroSoldati(arcieri[1], dannoInflittoDalNemico, (Esercito.Unità.Arcere_2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[1], Esercito.Unità.Arcere_2.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp[1] = RidurreNumeroSoldati(catapulte[1], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_2.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[1], Esercito.Unità.Catapulta_2.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            guerrieri_Temp[2] = RidurreNumeroSoldati(guerrieri[2], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[2], Esercito.Unità.Guerriero_3.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp[2] = RidurreNumeroSoldati(picchieri[2], dannoInflittoDalNemico, (Esercito.Unità.Lancere_3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[2], Esercito.Unità.Lancere_3.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp[2] = RidurreNumeroSoldati(arcieri[2], dannoInflittoDalNemico, (Esercito.Unità.Arcere_3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[2], Esercito.Unità.Arcere_3.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp[2] = RidurreNumeroSoldati(catapulte[2], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_3.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[2], Esercito.Unità.Catapulta_3.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            guerrieri_Temp[3] = RidurreNumeroSoldati(guerrieri[3], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[3], Esercito.Unità.Guerriero_4.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp[3] = RidurreNumeroSoldati(picchieri[3], dannoInflittoDalNemico, (Esercito.Unità.Lancere_4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[3], Esercito.Unità.Lancere_4.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp[3] = RidurreNumeroSoldati(arcieri[3], dannoInflittoDalNemico, (Esercito.Unità.Arcere_4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[3], Esercito.Unità.Arcere_4.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp[3] = RidurreNumeroSoldati(catapulte[3], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_4.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[3], Esercito.Unità.Catapulta_4.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            guerrieri_Temp[4] = RidurreNumeroSoldati(guerrieri[4], dannoInflittoDalNemico, (Esercito.Unità.Guerriero_5.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Guerriero_Difesa) * guerrieri[4], Esercito.Unità.Guerriero_5.Salute + Ricerca.Soldati.Incremento.Salute * player.Guerriero_Salute);
            picchieri_Temp[4] = RidurreNumeroSoldati(picchieri[4], dannoInflittoDalNemico, (Esercito.Unità.Lancere_5.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Lancere_Difesa) * picchieri[4], Esercito.Unità.Lancere_5.Salute + Ricerca.Soldati.Incremento.Salute * player.Lancere_Salute);
            arcieri_Temp[4] = RidurreNumeroSoldati(arcieri[4], dannoInflittoDalNemico, (Esercito.Unità.Arcere_5.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Arcere_Difesa) * arcieri[4], Esercito.Unità.Arcere_5.Salute + Ricerca.Soldati.Incremento.Salute * player.Arcere_Salute);
            catapulte_Temp[4] = RidurreNumeroSoldati(catapulte[4], dannoInflittoDalNemico, (Esercito.Unità.Catapulta_5.Difesa + Ricerca.Soldati.Incremento.Difesa * player.Catapulta_Difesa) * catapulte[4], Esercito.Unità.Catapulta_5.Salute + Ricerca.Soldati.Incremento.Salute * player.Catapulta_Salute);

            // Applicare il danno alle unità nemiche
            guerrieri_Temp_Enemy[0] = RidurreNumeroSoldati(guerrieri_Enemy[0], dannoInflitto, Esercito.EsercitoNemico.Guerrieri_1.Difesa * guerrieri_Enemy[0], Esercito.EsercitoNemico.Guerrieri_1.Salute);
            picchieri_Temp_Enemy[1] = RidurreNumeroSoldati(picchieri_Enemy[1], dannoInflitto, Esercito.EsercitoNemico.Lanceri_1.Difesa * picchieri_Enemy[1], Esercito.EsercitoNemico.Lanceri_1.Salute);
            arcieri_Temp_Enemy[2] = RidurreNumeroSoldati(arcieri_Enemy[2], dannoInflitto, Esercito.EsercitoNemico.Arceri_1.Difesa * arcieri_Enemy[2], Esercito.EsercitoNemico.Arceri_1.Salute);
            catapulte_Temp_Enemy[3] = RidurreNumeroSoldati(catapulte_Enemy[3], dannoInflitto, Esercito.EsercitoNemico.Catapulte_1.Difesa * catapulte_Enemy[3], Esercito.EsercitoNemico.Catapulte_1.Salute);

            int esperienza = 0;
            for (int i = 0; i < 5; i++)
            {
                //Giocatore
                if (guerrieri[i] > 0) guerrieri_Temp_Morti[i] = guerrieri[i] - guerrieri_Temp[i];
                if (guerrieri[i] > 0) picchieri_Temp_Morti[i] = picchieri[i] - picchieri_Temp[i];
                if (arcieri[i] > 0) arcieri_Temp_Morti[i] = arcieri[i] - arcieri_Temp[i];
                if (catapulte[i] > 0) catapulte_Temp_Morti[i] = catapulte[i] - catapulte_Temp[i];
                //Barbaro (Villaggio/Città)
                if (guerrieri_Enemy[i] > 0) guerrieri_Temp_Enemy_Morti[i] = guerrieri_Enemy[i] - guerrieri_Temp_Enemy[i];
                if (guerrieri_Enemy[i] > 0) picchieri_Temp_Enemy_Morti[i] = picchieri_Enemy[i] - picchieri_Temp_Enemy[i];
                if (arcieri_Enemy[i] > 0) arcieri_Temp_Enemy_Morti[i] = arcieri_Enemy[i] - arcieri_Temp_Enemy[i];
                if (catapulte_Enemy[i] > 0) catapulte_Temp_Enemy_Morti[i] = catapulte_Enemy[i] - catapulte_Temp_Enemy[i];
            }
            

            esperienza += guerrieri_Temp_Enemy_Morti[0] * Esercito.EsercitoNemico.Guerrieri_1.Esperienza +
                                 picchieri_Temp_Enemy_Morti[0] * Esercito.EsercitoNemico.Lanceri_1.Esperienza +
                                 arcieri_Temp_Enemy_Morti[0] * Esercito.EsercitoNemico.Arceri_1.Esperienza +
                                 catapulte_Temp_Enemy_Morti[0] * Esercito.EsercitoNemico.Arceri_1.Esperienza;

            esperienza += guerrieri_Temp_Enemy_Morti[1] * Esercito.EsercitoNemico.Guerrieri_2.Esperienza +
                                 picchieri_Temp_Enemy_Morti[1] * Esercito.EsercitoNemico.Lanceri_2.Esperienza +
                                 arcieri_Temp_Enemy_Morti[1] * Esercito.EsercitoNemico.Arceri_2.Esperienza +
                                 catapulte_Temp_Enemy_Morti[1] * Esercito.EsercitoNemico.Arceri_2.Esperienza;

            esperienza += guerrieri_Temp_Enemy_Morti[2] * Esercito.EsercitoNemico.Guerrieri_3.Esperienza +
                                 picchieri_Temp_Enemy_Morti[2] * Esercito.EsercitoNemico.Lanceri_3.Esperienza +
                                 arcieri_Temp_Enemy_Morti[2] * Esercito.EsercitoNemico.Arceri_3.Esperienza +
                                 catapulte_Temp_Enemy_Morti[2] * Esercito.EsercitoNemico.Arceri_3.Esperienza;

            esperienza += guerrieri_Temp_Enemy_Morti[3] * Esercito.EsercitoNemico.Guerrieri_4.Esperienza +
                                 picchieri_Temp_Enemy_Morti[3] * Esercito.EsercitoNemico.Lanceri_4.Esperienza +
                                 arcieri_Temp_Enemy_Morti[3] * Esercito.EsercitoNemico.Arceri_4.Esperienza +
                                 catapulte_Temp_Enemy_Morti[3] * Esercito.EsercitoNemico.Arceri_4.Esperienza;

            esperienza += guerrieri_Temp_Enemy_Morti[4] * Esercito.EsercitoNemico.Guerrieri_5.Esperienza +
                                 picchieri_Temp_Enemy_Morti[4] * Esercito.EsercitoNemico.Lanceri_5.Esperienza +
                                 arcieri_Temp_Enemy_Morti[4] * Esercito.EsercitoNemico.Arceri_5.Esperienza +
                                 catapulte_Temp_Enemy_Morti[4] * Esercito.EsercitoNemico.Arceri_5.Esperienza;

            Send(clientGuid, $"Log_Server|Danno inflitto dal giocatore: {(dannoInflitto * tipi_Di_Unità_Att++).ToString("0.00")}\r\n");
            Send(clientGuid, $"Log_Server|Danno inflitto dal nemico: {(dannoInflittoDalNemico * tipi_Di_Unità++).ToString("0.00")}");
            Send(clientGuid, $"Log_Server|" +
                $"Guerrieri: {guerrieri_Temp_Morti[0]}/{guerrieri[0]} lv1 {guerrieri_Temp_Morti[1]}/{guerrieri[1]} lv2 {guerrieri_Temp_Morti[2]}/{guerrieri[2]} lv3 {guerrieri_Temp_Morti[3]}/{guerrieri[3]} lv4 {guerrieri_Temp_Morti[4]}/{guerrieri[4]} lv5\r\n " +
                $"Lancieri: {picchieri_Temp_Morti[0]}/{picchieri[0]} lv1 {picchieri_Temp_Morti[1]}/{picchieri[1]} lv2 {picchieri_Temp_Morti[2]}/{picchieri[2]} lv3 {picchieri_Temp_Morti[3]}/{picchieri[3]} lv4 {picchieri_Temp_Morti[4]}/{picchieri[4]} lv5\r\n " +
                $"Arcieri: {arcieri_Temp_Morti}/{arcieri[0]} lv1 {arcieri_Temp_Morti[1]}/{arcieri[1]} lv2 {arcieri_Temp_Morti[2]}/{arcieri[2]} lv3 {arcieri_Temp_Morti[3]}/{arcieri[3]} lv4 {arcieri_Temp_Morti[4]}/{arcieri[4]} lv5\r\n " +
                $"Catapulte: {catapulte_Temp_Morti[0]}/{catapulte[0]} lv1 {catapulte_Temp_Morti[1]}/{catapulte[1]} lv2 {catapulte_Temp_Morti[2]}/{catapulte[2]} lv3 {catapulte_Temp_Morti[3]}/{catapulte[3]} lv4 {catapulte_Temp_Morti[4]}/{catapulte[4]} lv5\r\n");
            Send(clientGuid, $"Log_Server|Soldati persi dal giocatore [{player.Username}]:");

            Send(clientGuid, $"Log_Server|" +
                $"Guerrieri: {guerrieri_Temp_Enemy_Morti}/{guerrieri_Enemy[0]} lv1 {guerrieri_Temp_Enemy_Morti[1]}/{guerrieri_Enemy[1]} lv2 {guerrieri_Temp_Enemy_Morti[2]}/{guerrieri_Enemy[2]} lv3 {guerrieri_Temp_Enemy_Morti[3]}/{guerrieri_Enemy[3]} lv4 {guerrieri_Temp_Enemy_Morti[4]}/{guerrieri_Enemy[4]} lv5\r\n " +
                $"Lancieri: {picchieri_Temp_Enemy_Morti[0]}/{picchieri_Enemy[0]} lv1 {picchieri_Temp_Enemy_Morti[1]}/{picchieri_Enemy[1]} lv2 {picchieri_Temp_Enemy_Morti[2]}/{picchieri_Enemy[2]} lv3 {picchieri_Temp_Enemy_Morti[3]}/{picchieri_Enemy[3]} lv4 {picchieri_Temp_Enemy_Morti[4]}/{picchieri_Enemy[4]} lv5\r\n " +
                $"Arcieri: {arcieri_Temp_Enemy_Morti[0]}/{arcieri_Enemy[0]} lv1 {arcieri_Temp_Enemy_Morti[1]}/{arcieri_Enemy[1]} lv2 {arcieri_Temp_Enemy_Morti[2]}/{arcieri_Enemy[2]} lv3 {arcieri_Temp_Enemy_Morti[3]}/{arcieri_Enemy[3]} lv4 {arcieri_Temp_Enemy_Morti[4]}/{arcieri_Enemy[4]} lv5\r\n " +
                $"Catapulte: {catapulte_Temp_Enemy_Morti[0]}/{catapulte_Enemy[0]} lv1 {catapulte_Temp_Enemy_Morti[1]}/{catapulte_Enemy[1]} lv2 {catapulte_Temp_Enemy_Morti[2]}/{catapulte_Enemy[2]} lv3 {catapulte_Temp_Enemy_Morti[3]}/{catapulte_Enemy[3]} lv4 {catapulte_Temp_Enemy_Morti[4]}/{catapulte_Enemy[4]} lv5\r\n");
            Send(clientGuid, $"Log_Server|Soldati persi dal nemico:");
            Send(clientGuid, $"Log_Server|Battaglia PVE Completata\r\n");

            player.Esperienza += esperienza;

            Console.WriteLine($"Danno inflitto dal nemico: {(dannoInflittoDalNemico * tipi_Di_Unità++).ToString("0.00")}");
            Console.WriteLine($"Danno inflitto dal giocatore: {(dannoInflitto * tipi_Di_Unità_Att++).ToString("0.00")}");

            Console.WriteLine("Giocatore: \r\n" +
                $"Guerrieri: {guerrieri_Temp_Morti[0]}/{guerrieri[0]} lv1 {guerrieri_Temp_Morti[1]}/{guerrieri[1]} lv2 {guerrieri_Temp_Morti[2]}/{guerrieri[2]} lv3 {guerrieri_Temp_Morti[3]}/{guerrieri[3]} lv4 {guerrieri_Temp_Morti[4]}/{guerrieri[4]} lv5\r\n " +
                $"Lancieri: {picchieri_Temp_Morti[0]}/{picchieri[0]} lv1 {picchieri_Temp_Morti[1]}/{picchieri[1]} lv2 {picchieri_Temp_Morti[2]}/{picchieri[2]} lv3 {picchieri_Temp_Morti[3]}/{picchieri[3]} lv4 {picchieri_Temp_Morti[4]}/{picchieri[4]} lv5\r\n " +
                $"Arcieri: {arcieri_Temp_Morti}/{arcieri[0]} lv1 {arcieri_Temp_Morti[1]}/{arcieri[1]} lv2 {arcieri_Temp_Morti[2]}/{arcieri[2]} lv3 {arcieri_Temp_Morti[3]}/{arcieri[3]} lv4 {arcieri_Temp_Morti[4]}/{arcieri[4]} lv5\r\n " +
                $"Catapulte: {catapulte_Temp_Morti[0]}/{catapulte[0]} lv1 {catapulte_Temp_Morti[1]}/{catapulte[1]} lv2 {catapulte_Temp_Morti[2]}/{catapulte[2]} lv3 {catapulte_Temp_Morti[3]}/{catapulte[3]} lv4 {catapulte_Temp_Morti[4]}/{catapulte[4]} lv5\r\n");

            Console.WriteLine("Barbaro: \r\n" +
                $"Guerrieri: {guerrieri_Temp_Enemy_Morti}/{guerrieri_Enemy[0]} lv1 {guerrieri_Temp_Enemy_Morti[1]}/{guerrieri_Enemy[1]} lv2 {guerrieri_Temp_Enemy_Morti[2]}/{guerrieri_Enemy[2]} lv3 {guerrieri_Temp_Enemy_Morti[3]}/{guerrieri_Enemy[3]} lv4 {guerrieri_Temp_Enemy_Morti[4]}/{guerrieri_Enemy[4]} lv5\r\n " +
                $"Lancieri: {picchieri_Temp_Enemy_Morti[0]}/{picchieri_Enemy[0]} lv1 {picchieri_Temp_Enemy_Morti[1]}/{picchieri_Enemy[1]} lv2 {picchieri_Temp_Enemy_Morti[2]}/{picchieri_Enemy[2]} lv3 {picchieri_Temp_Enemy_Morti[3]}/{picchieri_Enemy[3]} lv4 {picchieri_Temp_Enemy_Morti[4]}/{picchieri_Enemy[4]} lv5\r\n " +
                $"Arcieri: {arcieri_Temp_Enemy_Morti[0]}/{arcieri_Enemy[0]} lv1 {arcieri_Temp_Enemy_Morti[1]}/{arcieri_Enemy[1]} lv2 {arcieri_Temp_Enemy_Morti[2]}/{arcieri_Enemy[2]} lv3 {arcieri_Temp_Enemy_Morti[3]}/{arcieri_Enemy[3]} lv4 {arcieri_Temp_Enemy_Morti[4]}/{arcieri_Enemy[4]} lv5\r\n " +
                $"Catapulte: {catapulte_Temp_Enemy_Morti[0]}/{catapulte_Enemy[0]} lv1 {catapulte_Temp_Enemy_Morti[1]}/{catapulte_Enemy[1]} lv2 {catapulte_Temp_Enemy_Morti[2]}/{catapulte_Enemy[2]} lv3 {catapulte_Temp_Enemy_Morti[3]}/{catapulte_Enemy[3]} lv4 {catapulte_Temp_Enemy_Morti[4]}/{catapulte_Enemy[4]} lv5\r\n");

            Console.WriteLine($"Soldati persi dal nemico:");
            Console.WriteLine($"Battaglia PVE Completata");

            // Aggiornare le quantità delle unità
            player.Guerrieri = guerrieri_Temp;
            player.Lanceri = picchieri_Temp;
            player.Arceri = arcieri_Temp;
            player.Catapulte = catapulte_Temp;

            //Salvate soldati barbaro villaggio/città !!!!!!
            // Aggiorna i barbari PVP
            if (liv >= 1 && liv <= 4)
            {
                citta.Guerrieri = guerrieri_Temp_Enemy[0];
                citta.Lancieri = picchieri_Temp_Enemy[0];
                citta.Arcieri = arcieri_Temp_Enemy[0];
                citta.Catapulte = catapulte_Temp_Enemy[0];
            }
            if (liv > 4 && liv <= 8)
            {
                citta.Guerrieri = guerrieri_Temp_Enemy[1];
                citta.Lancieri = picchieri_Temp_Enemy[1];
                citta.Arcieri = arcieri_Temp_Enemy[1];
                citta.Catapulte = catapulte_Temp_Enemy[1];
            }
            if (liv > 8 && liv <= 12)
            {
                citta.Guerrieri = guerrieri_Temp_Enemy[2];
                citta.Lancieri = picchieri_Temp_Enemy[2];
                citta.Arcieri = arcieri_Temp_Enemy[2];
                citta.Catapulte = catapulte_Temp_Enemy[2];
            }
            if (liv > 12 && liv <= 16)
            {
                citta.Guerrieri = guerrieri_Temp_Enemy[3];
                citta.Lancieri = picchieri_Temp_Enemy[3];
                citta.Arcieri = arcieri_Temp_Enemy[3];
                citta.Catapulte = catapulte_Temp_Enemy[3];
            }
            if (liv > 16 && liv <= 20)
            {
                citta.Guerrieri = guerrieri_Temp_Enemy[4];
                citta.Lancieri = picchieri_Temp_Enemy[4];
                citta.Arcieri = arcieri_Temp_Enemy[4];
                citta.Catapulte = catapulte_Temp_Enemy[4];
            }

            return false;
        }

        public static double CalcolareDanno_Giocatore(int arcieri, int catapulte, int guerrieri, int picchieri, Player player, Guid guid, int livello)
        {
            double dannoArcieri = 0;  // supponiamo che ogni arciere infligga 5 danni
            double dannoCatapulte = 0;  // supponiamo che ogni catapulta infligga 15 danni

            if (player.Frecce < arcieri * Esercito.Unità.Arcere_1.Componente_Lancio + catapulte * Esercito.Unità.Catapulta_1.Componente_Lancio)
            {
                Send(guid, $"Log_Server|Gli arceri lv {livello} e le catapulte lv {livello} del giocatore subiscono una riduzione del danno per mancanza di frecce [{arcieri * Esercito.Unità.Arcere_1.Componente_Lancio + catapulte * Esercito.Unità.Catapulta_1.Componente_Lancio}/{player.Frecce}]:");
                dannoArcieri = 0.33 * arcieri * (Esercito.Unità.Arcere_1.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Arcere_Attacco);  // supponiamo che ogni arciere infligga 5 danni
                dannoCatapulte = 0.33 * catapulte * (Esercito.Unità.Catapulta_1.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Catapulta_Attacco);  // supponiamo che ogni catapulta infligga 15 danni
                player.Frecce = 0;
            }
            else
            {
                player.Frecce -= arcieri * Esercito.Unità.Arcere_1.Componente_Lancio + catapulte * Esercito.Unità.Catapulta_1.Componente_Lancio;
                dannoArcieri = arcieri * (Esercito.Unità.Arcere_1.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Arcere_Attacco);  // supponiamo che ogni arciere infligga 5 danni
                dannoCatapulte = catapulte * (Esercito.Unità.Catapulta_1.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Catapulta_Attacco);  // supponiamo che ogni catapulta infligga 15 danni
                Send(guid, $"Log_Server|Frecce utilizzate: {arcieri * Esercito.Unità.Arcere_1.Componente_Lancio + catapulte * Esercito.Unità.Catapulta_1.Componente_Lancio}/{player.Frecce} lv {livello}");
            }

            // Esempio di calcolo del danno combinato, può essere esteso con logiche più complesse
            double dannoGuerrieri = guerrieri * (Esercito.Unità.Guerriero_1.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Guerriero_Attacco);  // supponiamo che ogni Guerrieri_1 infligga 10 danni
            double dannoPicchieri = picchieri * (Esercito.Unità.Lancere_1.Attacco + Ricerca.Soldati.Incremento.Attacco * player.Lancere_Attacco);  // supponiamo che ogni picchiere infligga 8 danni
            return dannoArcieri + dannoCatapulte + dannoGuerrieri + dannoPicchieri;
        }
        public static double CalcolareDanno_Invasore(int[] arcieri, int[] catapulte, int[] guerrieri, int[] picchieri, Player player)
        {
            double dannoArcieri = 0;
            double dannoCatapulte = 0;
            double dannoGuerrieri = 0;
            double dannoPicchieri = 0;

            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    dannoArcieri += arcieri[i] * (Esercito.EsercitoNemico.Arceri_1.Attacco);  // supponiamo che ogni arciere infligga 5 danni
                    dannoCatapulte += catapulte[i] * (Esercito.EsercitoNemico.Catapulte_1.Attacco);  // supponiamo che ogni catapulta infligga 15 danni
                    dannoGuerrieri += guerrieri[i] * (Esercito.EsercitoNemico.Guerrieri_1.Attacco);  // supponiamo che ogni Guerrieri_1 infligga 10 danni
                    dannoPicchieri += picchieri[i] * (Esercito.EsercitoNemico.Lanceri_1.Attacco);  // supponiamo che ogni picchiere infligga 8 danni
                }
                if (i == 1)
                {
                    dannoArcieri += arcieri[i] * (Esercito.EsercitoNemico.Arceri_2.Attacco);  // supponiamo che ogni arciere infligga 5 danni
                    dannoCatapulte += catapulte[i] * (Esercito.EsercitoNemico.Catapulte_2.Attacco);  // supponiamo che ogni catapulta infligga 15 danni
                    dannoGuerrieri += guerrieri[i] * (Esercito.EsercitoNemico.Guerrieri_2.Attacco);  // supponiamo che ogni Guerrieri_1 infligga 10 danni
                    dannoPicchieri += picchieri[i] * (Esercito.EsercitoNemico.Lanceri_2.Attacco);  // supponiamo che ogni picchiere infligga 8 danni
                }
                if (i == 2)
                {
                    dannoArcieri += arcieri[i] * (Esercito.EsercitoNemico.Arceri_3.Attacco);  // supponiamo che ogni arciere infligga 5 danni
                    dannoCatapulte += catapulte[i] * (Esercito.EsercitoNemico.Catapulte_3.Attacco);  // supponiamo che ogni catapulta infligga 15 danni
                    dannoGuerrieri += guerrieri[i] * (Esercito.EsercitoNemico.Guerrieri_3.Attacco);  // supponiamo che ogni Guerrieri_1 infligga 10 danni
                    dannoPicchieri += picchieri[i] * (Esercito.EsercitoNemico.Lanceri_3.Attacco);  // supponiamo che ogni picchiere infligga 8 danni
                }
                if (i == 3)
                {
                    dannoArcieri += arcieri[i] * (Esercito.EsercitoNemico.Arceri_4.Attacco);  // supponiamo che ogni arciere infligga 5 danni
                    dannoCatapulte += catapulte[i] * (Esercito.EsercitoNemico.Catapulte_4.Attacco);  // supponiamo che ogni catapulta infligga 15 danni
                    dannoGuerrieri += guerrieri[i] * (Esercito.EsercitoNemico.Guerrieri_4.Attacco);  // supponiamo che ogni Guerrieri_1 infligga 10 danni
                    dannoPicchieri += picchieri[i] * (Esercito.EsercitoNemico.Lanceri_4.Attacco);  // supponiamo che ogni picchiere infligga 8 danni
                }
                if (i == 3)
                {
                    dannoArcieri += arcieri[i] * (Esercito.EsercitoNemico.Arceri_5.Attacco);  // supponiamo che ogni arciere infligga 5 danni
                    dannoCatapulte += catapulte[i] * (Esercito.EsercitoNemico.Catapulte_5.Attacco);  // supponiamo che ogni catapulta infligga 15 danni
                    dannoGuerrieri += guerrieri[i] * (Esercito.EsercitoNemico.Guerrieri_5.Attacco);  // supponiamo che ogni Guerrieri_1 infligga 10 danni
                    dannoPicchieri += picchieri[i] * (Esercito.EsercitoNemico.Lanceri_5.Attacco);  // supponiamo che ogni picchiere infligga 8 danni
                }
            }
            return dannoArcieri + dannoCatapulte + dannoGuerrieri + dannoPicchieri;
        }
        public static int RidurreNumeroSoldati(int numeroSoldati, double danno, double difesa, double salutePerSoldato)
        {
            // Calcolare il danno effettivo tenendo conto della difesa
            double dannoEffettivo = danno - difesa;
            dannoEffettivo = Math.Max(0, dannoEffettivo); // Assicurarsi che il danno non sia negativo

            int soldatiPersi = Convert.ToInt32(dannoEffettivo / salutePerSoldato);
            numeroSoldati -= soldatiPersi;
            return numeroSoldati < 0 ? 0 : numeroSoldati;
        }
        public static int ContareTipiDiUnità(int[] guerrieri, int[] picchieri, int[] arcieri, int[] catapulte)
        {
            int tipiDiUnità = 0;

            if (guerrieri.Count() > 0) tipiDiUnità++; // Ci sono unità di qualsiasi livello? 
            if (picchieri.Count() > 0) tipiDiUnità++;
            if (arcieri.Count() > 0) tipiDiUnità++;
            if (catapulte.Count() > 0) tipiDiUnità++;

            // Se non ci sono unità, forza a 1 per evitare divisioni per zero
            return tipiDiUnità == 0 ? 1 : tipiDiUnità;
        }

        public class Truppe
        {
            public int[] Guerrieri { get; set; }
            public int[] Lanceri { get; set; }
            public int[] Arcieri { get; set; }
            public int[] Catapulte { get; set; }

            public Truppe(int[] g, int[] l, int[] a, int[] c)
            {
                Guerrieri = g; Lanceri = l; Arcieri = a; Catapulte = c;
            }

            public int CalcolaFrecceNecessarie()
            {
                return Arcieri.Sum() * Esercito.Unità.Arcere_1.Componente_Lancio +
                       Catapulte.Sum() * Esercito.Unità.Catapulta_1.Componente_Lancio;
            }

            public void RiduciEfficienza(double fattore)
            {
                for (int i = 0; i < Arcieri.Length; i++)
                {
                    Arcieri[i] = (int)(Arcieri[i] * fattore);
                    Catapulte[i] = (int)(Catapulte[i] * fattore);
                }
            }

            public void ApplicaPerdite(Truppe perdite)
            {
                for (int i = 0; i < Guerrieri.Length; i++)
                {
                    Guerrieri[i] = Math.Max(0, Guerrieri[i] - perdite.Guerrieri[i]);
                    Lanceri[i] = Math.Max(0, Lanceri[i] - perdite.Lanceri[i]);
                }
            }
        }
        public static (Truppe DanniGiocatore, Truppe DanniNemico, int Esperienza) CalcolaDanno(Truppe att, Truppe def)
        {
            // Valuta la forza relativa
            int potenzaAtt = (att.Arcieri.Sum() + att.Catapulte.Sum()) * 4 / 5;
            int potenzaDef = (def.Arcieri.Sum() + def.Catapulte.Sum()) * 4 / 5;

            // Danno bilanciato
            int mortiNemiciGuerrieri = Math.Min(def.Guerrieri.Sum(), potenzaAtt * 2 / 3);
            int mortiNemiciLanceri = Math.Min(def.Lanceri.Sum(), potenzaAtt / 3);
            int mortiGiocatoreGuerrieri = Math.Min(att.Guerrieri.Sum(), potenzaDef / 3);
            int mortiGiocatoreLanceri = Math.Min(att.Lanceri.Sum(), potenzaDef * 2 / 3);

            var danniGiocatore = new Truppe(
                new[] { mortiGiocatoreGuerrieri },
                new[] { mortiGiocatoreLanceri },
                new int[1],
                new int[1]
            );

            var danniNemico = new Truppe(
                new[] { mortiNemiciGuerrieri },
                new[] { mortiNemiciLanceri },
                new int[1],
                new int[1]
            );

            int esperienza = mortiNemiciGuerrieri * Esercito.EsercitoNemico.Guerrieri_1.Esperienza +
                             mortiNemiciLanceri * Esercito.EsercitoNemico.Lanceri_1.Esperienza;

            return (danniGiocatore, danniNemico, esperienza);
        }

        public static async Task<bool> Battaglia_Distanza(string struttura, Player player, Guid clientGuid, string livello)
        {
            int liv = Convert.ToInt32(livello);

            // 🎯 Trova città o villaggio bersaglio
            Barbari.BarbarianBase target = Barbari.CittaGlobali.FirstOrDefault(c => c.Livello == liv);

            if (target == null)
            {
                target = player.VillaggiPersonali.FirstOrDefault(v => v.Livello == liv);
            }

            // 📊 Statistiche giocatore e nemico
            var truppeGiocatore = new Truppe(player.Guerrieri, player.Lanceri, player.Arceri, player.Catapulte);
            var truppeNemico = new Truppe(
                new[] { target.Guerrieri },
                new[] { target.Lancieri },
                new[] { target.Arcieri },
                new[] { target.Catapulte }
            );

            // 🧮 Calcola frecce richieste e riduzioni danni
            int frecceRichieste = truppeGiocatore.CalcolaFrecceNecessarie();
            if (player.Frecce < frecceRichieste)
            {
                Send(clientGuid, $"Log_Server|Poche frecce! Danno ridotto. [{player.Frecce}/{frecceRichieste}]");
                truppeGiocatore.RiduciEfficienza(0.33);
                player.Frecce = 0;
            }
            else player.Frecce -= frecceRichieste;

            var risultatoAttacco = CalcolaDanno(truppeGiocatore, truppeNemico); // ⚔️ Fase d’attacco a distanza

            // 📉 Aggiorna truppe dopo il combattimento
            truppeNemico.ApplicaPerdite(risultatoAttacco.DanniNemico);
            truppeGiocatore.ApplicaPerdite(risultatoAttacco.DanniGiocatore);

            // 💰 Esperienza e log
            player.Esperienza += risultatoAttacco.Esperienza;
            Send(clientGuid, $"Log_Server|{struttura}: Hai inflitto {risultatoAttacco.DanniNemico.Guerrieri} morti nemici");
            Console.WriteLine($"({struttura}) Danno inflitto: {risultatoAttacco.DanniNemico}");

            // 🔄 Salva stato aggiornato
            player.Guerrieri = truppeGiocatore.Guerrieri;
            player.Lanceri = truppeGiocatore.Lanceri;
            player.Arceri = truppeGiocatore.Arcieri;
            player.Catapulte = truppeGiocatore.Catapulte;

            target.Guerrieri = truppeNemico.Guerrieri.Sum();
            target.Lancieri = truppeNemico.Lanceri.Sum();

            return true;
        }
        public static async Task<bool> Battaglia_Distanza(Player player, Guid clientGuid, Player player2, Guid clientGuid2)
        {
            int guerrieri_Morti = 0, lancieri_Morti = 0, guerrieri_Morti_Att = 0, lancieri_Morti_Att = 0;

            int guerrieri = player.Guerrieri[0];
            int picchieri = player.Lanceri[0];
            int arcieri = player.Arceri[0];
            int catapulte = player.Catapulte[0];

            int guerrieri_Enemy = player2.Guerrieri[0];
            int picchieri_Enemy = player2.Lanceri[0];
            int arcieri_Enemy = player2.Arceri[0];
            int catapulte_Enemy = player2.Catapulte[0];

            int arcieri_Temp = arcieri * 2 / 3;
            int catapulte_Temp = catapulte * 2 / 3;
            int arcieri_Enemy_Temp = arcieri_Enemy * 2 / 3;
            int catapulte_Enemy_Temp = catapulte_Enemy * 2 / 3;

            if (player.Frecce < arcieri * Esercito.Unità.Arcere_1.Componente_Lancio + catapulte * Esercito.Unità.Catapulta_1.Componente_Lancio)
            {
                Send(clientGuid, $"Log_Server|Gli arceri e le catapulte del giocatore [{player.Username}] subiscono una riduzione del danno per mancanza di frecce [{arcieri * Esercito.Unità.Arcere_1.Componente_Lancio + catapulte * Esercito.Unità.Catapulta_1.Componente_Lancio}/{player.Frecce}]:");
                arcieri_Temp = arcieri_Temp / 3;
                catapulte_Temp = catapulte_Temp / 3;
                player.Frecce = 0;
            }
            else
                player.Frecce -= arcieri * Esercito.Unità.Arcere_1.Componente_Lancio + catapulte * Esercito.Unità.Catapulta_1.Componente_Lancio;

            if (player2.Frecce < arcieri_Enemy * Esercito.Unità.Arcere_1.Componente_Lancio + catapulte_Enemy * Esercito.Unità.Catapulta_1.Componente_Lancio)
            {
                Send(clientGuid, $"Log_Server|Gli arceri e le catapulte del giocatore [{player2.Username}] subiscono una riduzione del danno per mancanza di frecce [{arcieri_Enemy * Esercito.Unità.Arcere_1.Componente_Lancio + catapulte_Enemy * Esercito.Unità.Catapulta_1.Componente_Lancio}/{player.Frecce}]:");
                arcieri_Enemy_Temp = arcieri_Enemy_Temp / 3;
                catapulte_Enemy_Temp = catapulte_Enemy_Temp / 3;
                player2.Frecce = 0;
            }
            else
                player2.Frecce -= arcieri_Enemy * Esercito.Unità.Arcere_1.Componente_Lancio + catapulte_Enemy * Esercito.Unità.Catapulta_1.Componente_Lancio;

            #region Se poche unità a distanza e d'assedio
            if (arcieri <= 10) arcieri_Temp = arcieri * 2;
            if (catapulte <= 5) catapulte_Temp = catapulte * 6;
            if (arcieri == 0) arcieri_Temp = 0;
            if (catapulte == 0) catapulte_Temp = 0;

            if (arcieri_Enemy <= 10) arcieri_Enemy_Temp = arcieri_Enemy * 2;
            if (catapulte_Enemy <= 5) catapulte_Enemy_Temp = catapulte_Enemy * 6;
            if (arcieri_Enemy == 0) arcieri_Enemy_Temp = 0;
            if (catapulte_Enemy == 0) catapulte_Enemy_Temp = 0;
            #endregion

            if (arcieri > 0 || catapulte > 0)
            {
                int attacco = (catapulte_Temp + arcieri_Temp) * 4 / 5;
                if (guerrieri_Enemy > 0 && picchieri_Enemy > 0)
                {
                    if (guerrieri_Enemy > picchieri_Enemy)
                    {
                        guerrieri_Morti_Att = attacco * 2 / 3; //Danno 2/3 contro guerrieri
                        lancieri_Morti_Att = attacco / 3; //Danno 1/3 contro lancieri
                    } else
                    {
                        guerrieri_Morti_Att = attacco / 3; //Danno 2/3 contro guerrieri
                        lancieri_Morti_Att = attacco * 2 / 3; //Danno 1/3 contro lancieri
                    }
                }
                else if (guerrieri_Enemy > 0) guerrieri_Morti_Att = attacco * 4 / 5;
                else if (picchieri_Enemy > 0) lancieri_Morti_Att = attacco * 4 / 5;

                if (guerrieri_Morti_Att > guerrieri_Enemy)
                {
                    guerrieri_Morti_Att = guerrieri_Enemy;
                    guerrieri_Enemy = 0;
                }
                else
                    guerrieri_Enemy -= guerrieri_Morti_Att;

                if (lancieri_Morti_Att > picchieri_Enemy)
                {
                    lancieri_Morti_Att = picchieri_Enemy;
                    picchieri_Enemy = 0;
                }
                else
                    picchieri_Enemy -= lancieri_Morti_Att;

                player.Esperienza += guerrieri_Morti_Att * Esercito.Unità.Guerriero_1.Esperienza + lancieri_Morti_Att * Esercito.Unità.Lancere_1.Esperienza;
                int esperienza2 = guerrieri_Morti_Att * Esercito.Unità.Guerriero_1.Esperienza + lancieri_Morti_Att * Esercito.Unità.Lancere_1.Esperienza;

                Send(clientGuid2, $"Log_Server|Guerrieri morti: {guerrieri_Morti}/{player2.Guerrieri} Lancieri morti:  {lancieri_Morti}/{player2.Lanceri}\r\n Esperienza:  {esperienza2}\r\n");
                Send(clientGuid2, $"Log_Server|Gli arceri e le catapulte del giocatore [{player2.Username}] hanno causato:");
                Send(clientGuid2, $"Log_Server|Frecce utilizzate: {arcieri_Enemy * Esercito.Unità.Arcere_1.Componente_Lancio + catapulte_Enemy * Esercito.Unità.Catapulta_1.Componente_Lancio}");

                player2.Guerrieri[0] = guerrieri_Enemy;
                player2.Lanceri[0] = picchieri_Enemy;

            }
            Console.WriteLine($"Gli arceri e le catapulte del giocatore [{player.Username}] hanno causato:");
            Console.WriteLine($"Guerrieri morti: {guerrieri_Morti_Att} Lancieri morti:  {lancieri_Morti_Att}");

            Send(clientGuid, $"Log_Server|Guerrieri morti: {guerrieri_Morti_Att} Lancieri morti:  {lancieri_Morti_Att}\r\n");
            Send(clientGuid, $"Log_Server|Gli arceri e le catapulte del giocatore [{player.Username}] hanno causato:");
            Send(clientGuid2, $"Log_Server|Guerrieri morti: {guerrieri_Morti_Att} Lancieri morti:  {lancieri_Morti_Att}\r\n");
            Send(clientGuid2, $"Log_Server|Gli arceri e le catapulte del giocatore [{player.Username}] hanno causato:");

            if (arcieri_Enemy > 0 || catapulte_Enemy > 0)
            {
                int attacco = (catapulte_Enemy_Temp + arcieri_Enemy_Temp) * 4 / 5;
                if (guerrieri > 0 && picchieri > 0)
                {
                    if (guerrieri > picchieri)
                    {
                        guerrieri_Morti = attacco * 2 / 3;
                        lancieri_Morti = attacco / 3;
                    } else
                    {
                        guerrieri_Morti = attacco / 3;
                        lancieri_Morti = attacco * 2 / 3;
                    }
                }
                else if (guerrieri > 0) guerrieri_Morti = attacco * 4 / 5;
                else if (picchieri > 0) lancieri_Morti = attacco * 4 / 5;

                if (guerrieri_Morti > guerrieri)
                {
                    guerrieri_Morti = guerrieri;
                    guerrieri = 0;
                }
                else guerrieri -= guerrieri_Morti;

                if (lancieri_Morti > picchieri)
                {
                    lancieri_Morti = picchieri;
                    picchieri = 0;
                }
                else picchieri -= lancieri_Morti;

                player2.Esperienza += guerrieri_Morti * Esercito.Unità.Guerriero_1.Esperienza + lancieri_Morti * Esercito.Unità.Lancere_1.Esperienza;
                int esperienza1 = guerrieri_Morti * Esercito.Unità.Guerriero_1.Esperienza + lancieri_Morti * Esercito.Unità.Lancere_1.Esperienza;

                Send(clientGuid, $"Log_Server|Guerrieri morti: {guerrieri_Morti}/{player.Guerrieri} Lancieri morti:  {lancieri_Morti}/{player.Lanceri}\r\n Esperienza:  {esperienza1}\r\n");
                Send(clientGuid, $"Log_Server|Gli arceri e le catapulte del giocatore [{player2.Username}] hanno causato:");
                Send(clientGuid, $"Log_Server|Frecce utilizzate: {arcieri * Esercito.Unità.Arcere_1.Componente_Lancio + catapulte * Esercito.Unità.Catapulta_1.Componente_Lancio}");

                player.Guerrieri[0] = guerrieri;
                player.Lanceri[0] = picchieri;
            }

            Console.WriteLine($"Gli arceri e le catapulte del giocatore [{player.Username}] hanno causato:");
            Console.WriteLine($"Guerrieri morti: {guerrieri_Morti} Lancieri morti:  {lancieri_Morti}");
            return true;
        } // Arcieri e Mezzi d'assedio attaccano prima della battaglia (giocatore-giocatore)
    }
}
