using Server_Strategico.Gioco;
using static Server_Strategico.Manager.QuestManager;
using static Server_Strategico.ServerData.Moduli.Battaglie.Battaglia;

namespace Server_Strategico.ServerData.Moduli.Battaglie
{
    public class BattagliaPVP
    {
        private static void SendClient(Guid clientGuid, string message)
        {
            Server_Strategico.Server.Server.Send(clientGuid, message);
            Console.WriteLine(message.Replace("Log_Server|", ""));
        }
        static UnitGroup CaricaDatiStruttureDifensore(Giocatori.Player difensore, int struttura)
        {
            var defenderUnits = new UnitGroup
            {
                Guerrieri = [0, 0, 0, 0, 0],
                Lancieri = [0, 0, 0, 0, 0],
                Arcieri = [0, 0, 0, 0, 0],
                Catapulte = [0, 0, 0, 0, 0]
            };
            if (struttura == 1)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri_Ingresso;
                defenderUnits.Lancieri = difensore.Lanceri_Ingresso;
                defenderUnits.Arcieri = difensore.Arceri_Ingresso;
                defenderUnits.Catapulte = difensore.Catapulte_Ingresso;
            }
            if (struttura == 2)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri_Mura;
                defenderUnits.Lancieri = difensore.Lanceri_Mura;
                defenderUnits.Arcieri = difensore.Arceri_Mura;
                defenderUnits.Catapulte = difensore.Catapulte_Mura;
            }
            if (struttura == 3)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri_Cancello;
                defenderUnits.Lancieri = difensore.Lanceri_Cancello;
                defenderUnits.Arcieri = difensore.Arceri_Cancello;
                defenderUnits.Catapulte = difensore.Catapulte_Cancello;
            }
            if (struttura == 4)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri_Torri;
                defenderUnits.Lancieri = difensore.Lanceri_Torri;
                defenderUnits.Arcieri = difensore.Arceri_Torri;
                defenderUnits.Catapulte = difensore.Catapulte_Torri;
            }
            if (struttura == 5)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri_Citta;
                defenderUnits.Lancieri = difensore.Lanceri_Citta;
                defenderUnits.Arcieri = difensore.Arceri_Citta;
                defenderUnits.Catapulte = difensore.Catapulte_Citta;
            }
            if (struttura == 6)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri_Castello;
                defenderUnits.Lancieri = difensore.Lanceri_Castello;
                defenderUnits.Arcieri = difensore.Arceri_Castello;
                defenderUnits.Catapulte = difensore.Catapulte_Castello;
            }
            if (struttura == 7)
            {
                defenderUnits.Guerrieri = difensore.Guerrieri;
                defenderUnits.Lancieri = difensore.Lanceri;
                defenderUnits.Arcieri = difensore.Arceri;
                defenderUnits.Catapulte = difensore.Catapulte;
            }
            return defenderUnits;
        }
        static void AggiornaDatiStruttureDifensore(int struttura, Giocatori.Player difensore, RisultatoFase result)
        {
            if (struttura == 1)
            {
                difensore.Guerrieri_Ingresso = result.Difensore.Sopravvisuti.Guerrieri;
                difensore.Lanceri_Ingresso = result.Difensore.Sopravvisuti.Lancieri;
                difensore.Arceri_Ingresso = result.Difensore.Sopravvisuti.Arcieri;
                difensore.Catapulte_Ingresso = result.Difensore.Sopravvisuti.Catapulte;
            }
            if (struttura == 2)
            {
                difensore.Salute_Mura = result.Struttura.Salute;
                difensore.Difesa_Mura = result.Struttura.Difesa;
                if (result.Struttura.Salute == 0)
                {
                    result.Perdite_Crollo.Guerrieri = difensore.Guerrieri_Mura;
                    result.Perdite_Crollo.Lancieri = difensore.Lanceri_Mura;
                    result.Perdite_Crollo.Arcieri = difensore.Arceri_Mura;
                    result.Perdite_Crollo.Catapulte = difensore.Catapulte_Mura;

                    difensore.Guerrieri_Mura = [0, 0, 0, 0, 0];
                    difensore.Lanceri_Mura = [0, 0, 0, 0, 0];
                    difensore.Arceri_Mura = [0, 0, 0, 0, 0];
                    difensore.Catapulte_Mura = [0, 0, 0, 0, 0];
                }
                else
                {
                    difensore.Guerrieri_Mura = result.Difensore.Sopravvisuti.Guerrieri;
                    difensore.Lanceri_Mura = result.Difensore.Sopravvisuti.Lancieri;
                    difensore.Arceri_Mura = result.Difensore.Sopravvisuti.Arcieri;
                    difensore.Catapulte_Mura = result.Difensore.Sopravvisuti.Catapulte;
                }
            }
            if (struttura == 3)
            {
                difensore.Salute_Cancello = result.Struttura.Salute;
                difensore.Difesa_Cancello = result.Struttura.Difesa;
                if (result.Struttura.Salute == 0) //Distruzione struttura
                {
                    result.Perdite_Crollo.Guerrieri = difensore.Guerrieri_Cancello;
                    result.Perdite_Crollo.Lancieri = difensore.Lanceri_Cancello;
                    result.Perdite_Crollo.Arcieri = difensore.Arceri_Cancello;
                    result.Perdite_Crollo.Catapulte = difensore.Catapulte_Cancello;

                    difensore.Guerrieri_Cancello = [0, 0, 0, 0, 0];
                    difensore.Lanceri_Cancello = [0, 0, 0, 0, 0];
                    difensore.Arceri_Cancello = [0, 0, 0, 0, 0];
                    difensore.Catapulte_Cancello = [0, 0, 0, 0, 0];
                }
                else
                {
                    difensore.Guerrieri_Cancello = result.Difensore.Sopravvisuti.Guerrieri;
                    difensore.Lanceri_Cancello = result.Difensore.Sopravvisuti.Lancieri;
                    difensore.Arceri_Cancello = result.Difensore.Sopravvisuti.Arcieri;
                    difensore.Catapulte_Cancello = result.Difensore.Sopravvisuti.Catapulte;
                }
            }
            if (struttura == 4)
            {
                difensore.Salute_Torri = result.Struttura.Salute;
                difensore.Difesa_Torri = result.Struttura.Difesa;

                if (result.Struttura.Salute == 0)
                {
                    result.Perdite_Crollo.Guerrieri = difensore.Guerrieri_Torri;
                    result.Perdite_Crollo.Lancieri = difensore.Lanceri_Torri;
                    result.Perdite_Crollo.Arcieri = difensore.Arceri_Torri;
                    result.Perdite_Crollo.Catapulte = difensore.Catapulte_Torri;

                    difensore.Guerrieri_Torri = [0, 0, 0, 0, 0];
                    difensore.Lanceri_Torri = [0, 0, 0, 0, 0];
                    difensore.Arceri_Torri = [0, 0, 0, 0, 0];
                    difensore.Catapulte_Torri = [0, 0, 0, 0, 0];
                }
                else
                {
                    difensore.Guerrieri_Torri = result.Difensore.Sopravvisuti.Guerrieri;
                    difensore.Lanceri_Torri = result.Difensore.Sopravvisuti.Lancieri;
                    difensore.Arceri_Torri = result.Difensore.Sopravvisuti.Arcieri;
                    difensore.Catapulte_Torri = result.Difensore.Sopravvisuti.Catapulte;
                }
            }
            if (struttura == 5)
            {
                difensore.Guerrieri_Citta = result.Difensore.Sopravvisuti.Guerrieri;
                difensore.Lanceri_Citta = result.Difensore.Sopravvisuti.Lancieri;
                difensore.Arceri_Citta = result.Difensore.Sopravvisuti.Arcieri;
                difensore.Catapulte_Citta = result.Difensore.Sopravvisuti.Catapulte;
            }
            if (struttura == 6)
            {
                difensore.Salute_Castello = result.Struttura.Salute;
                difensore.Difesa_Castello = result.Struttura.Difesa;

                if (result.Struttura.Salute == 0)
                {
                    result.Perdite_Crollo.Guerrieri = difensore.Guerrieri_Castello;
                    result.Perdite_Crollo.Lancieri = difensore.Lanceri_Castello;
                    result.Perdite_Crollo.Arcieri = difensore.Arceri_Castello;
                    result.Perdite_Crollo.Catapulte = difensore.Catapulte_Castello;

                    difensore.Guerrieri_Castello = [0, 0, 0, 0, 0];
                    difensore.Lanceri_Castello = [0, 0, 0, 0, 0];
                    difensore.Arceri_Castello = [0, 0, 0, 0, 0];
                    difensore.Catapulte_Castello = [0, 0, 0, 0, 0];
                }
                else
                {
                    difensore.Guerrieri_Castello = result.Difensore.Sopravvisuti.Guerrieri;
                    difensore.Lanceri_Castello = result.Difensore.Sopravvisuti.Lancieri;
                    difensore.Arceri_Castello = result.Difensore.Sopravvisuti.Arcieri;
                    difensore.Catapulte_Castello = result.Difensore.Sopravvisuti.Catapulte;
                }
            }
            if (struttura == 7)
            {
                difensore.Guerrieri = result.Difensore.Sopravvisuti.Guerrieri;
                difensore.Lanceri = result.Difensore.Sopravvisuti.Lancieri;
                difensore.Arceri = result.Difensore.Sopravvisuti.Arcieri;
                difensore.Catapulte = result.Difensore.Sopravvisuti.Catapulte;
            }
        }
        private static int CalcolaEsperienzaPVP(UnitGroup casualties)
        {
            int esperienza = 0;
            for (int i = 0; i < 5; i++)
            {
                var stats = GetUnitStats(i);
                esperienza += casualties.Guerrieri[i] * stats.GuerrieriEsperienza;
                esperienza += casualties.Lancieri[i] * stats.LancieriEsperienza;
                esperienza += casualties.Arcieri[i] * stats.ArcieriEsperienza;
                esperienza += casualties.Catapulte[i] * stats.CatapulteEsperienza;
            }
            return esperienza;
        }
        private static (double GuerrieriAttacco, double GuerrieriDifesa, double GuerrieriSalute, int GuerrieriEsperienza,
                   double LancieriAttacco, double LancieriDifesa, double LancieriSalute, int LancieriEsperienza,
                   double ArcieriAttacco, double ArcieriDifesa, double ArcieriSalute, int ArcieriEsperienza,
                   double CatapulteAttacco, double CatapulteDifesa, double CatapulteSalute, int CatapulteEsperienza)
        GetUnitStats(int level)
        {
            return level switch
            {
                0 => (Esercito.Unità.Guerriero_1.Attacco, Esercito.Unità.Guerriero_1.Difesa,
                      Esercito.Unità.Guerriero_1.Salute, Esercito.Unità.Guerriero_1.Esperienza,
                      Esercito.Unità.Lancere_1.Attacco, Esercito.Unità.Lancere_1.Difesa,
                      Esercito.Unità.Lancere_1.Salute, Esercito.Unità.Lancere_1.Esperienza,
                      Esercito.Unità.Arcere_1.Attacco, Esercito.Unità.Arcere_1.Difesa,
                      Esercito.Unità.Arcere_1.Salute, Esercito.Unità.Arcere_1.Esperienza,
                      Esercito.Unità.Catapulta_1.Attacco, Esercito.Unità.Catapulta_1.Difesa,
                      Esercito.Unità.Catapulta_1.Salute, Esercito.Unità.Catapulta_1.Esperienza),

                1 => (Esercito.Unità.Guerriero_2.Attacco, Esercito.Unità.Guerriero_2.Difesa,
                      Esercito.Unità.Guerriero_2.Salute, Esercito.Unità.Guerriero_2.Esperienza,
                      Esercito.Unità.Lancere_2.Attacco, Esercito.Unità.Lancere_2.Difesa,
                      Esercito.Unità.Lancere_2.Salute, Esercito.Unità.Lancere_2.Esperienza,
                      Esercito.Unità.Arcere_2.Attacco, Esercito.Unità.Arcere_2.Difesa,
                      Esercito.Unità.Arcere_2.Salute, Esercito.Unità.Arcere_2.Esperienza,
                      Esercito.Unità.Catapulta_2.Attacco, Esercito.Unità.Catapulta_2.Difesa,
                      Esercito.Unità.Catapulta_2.Salute, Esercito.Unità.Catapulta_2.Esperienza),

                2 => (Esercito.Unità.Guerriero_3.Attacco, Esercito.Unità.Guerriero_3.Difesa,
                      Esercito.Unità.Guerriero_3.Salute, Esercito.Unità.Guerriero_3.Esperienza,
                      Esercito.Unità.Lancere_3.Attacco, Esercito.Unità.Lancere_3.Difesa,
                      Esercito.Unità.Lancere_3.Salute, Esercito.Unità.Lancere_3.Esperienza,
                      Esercito.Unità.Arcere_3.Attacco, Esercito.Unità.Arcere_3.Difesa,
                      Esercito.Unità.Arcere_3.Salute, Esercito.Unità.Arcere_3.Esperienza,
                      Esercito.Unità.Catapulta_3.Attacco, Esercito.Unità.Catapulta_3.Difesa,
                      Esercito.Unità.Catapulta_3.Salute, Esercito.Unità.Catapulta_3.Esperienza),

                3 => (Esercito.Unità.Guerriero_4.Attacco, Esercito.Unità.Guerriero_4.Difesa,
                      Esercito.Unità.Guerriero_4.Salute, Esercito.Unità.Guerriero_4.Esperienza,
                      Esercito.Unità.Lancere_4.Attacco, Esercito.Unità.Lancere_4.Difesa,
                      Esercito.Unità.Lancere_4.Salute, Esercito.Unità.Lancere_4.Esperienza,
                      Esercito.Unità.Arcere_4.Attacco, Esercito.Unità.Arcere_4.Difesa,
                      Esercito.Unità.Arcere_4.Salute, Esercito.Unità.Arcere_4.Esperienza,
                      Esercito.Unità.Catapulta_4.Attacco, Esercito.Unità.Catapulta_4.Difesa,
                      Esercito.Unità.Catapulta_4.Salute, Esercito.Unità.Catapulta_4.Esperienza),

                4 => (Esercito.Unità.Guerriero_5.Attacco, Esercito.Unità.Guerriero_5.Difesa,
                      Esercito.Unità.Guerriero_5.Salute, Esercito.Unità.Guerriero_5.Esperienza,
                      Esercito.Unità.Lancere_5.Attacco, Esercito.Unità.Lancere_5.Difesa,
                      Esercito.Unità.Lancere_5.Salute, Esercito.Unità.Lancere_5.Esperienza,
                      Esercito.Unità.Arcere_5.Attacco, Esercito.Unità.Arcere_5.Difesa,
                      Esercito.Unità.Arcere_5.Salute, Esercito.Unità.Arcere_5.Esperienza,
                      Esercito.Unità.Catapulta_5.Attacco, Esercito.Unità.Catapulta_5.Difesa,
                      Esercito.Unità.Catapulta_5.Salute, Esercito.Unità.Catapulta_5.Esperienza),

                _ => (0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
            };
        }
        private static (double GuerrieriAttacco, double GuerrieriDifesa, double GuerrieriSalute,
                  double LancieriAttacco, double LancieriDifesa, double LancieriSalute,
                  double ArcieriAttacco, double ArcieriDifesa, double ArcieriSalute,
                  double CatapulteAttacco, double CatapulteDifesa, double CatapulteSalute)
        GetPlayerUnitStats(int level, Giocatori.Player player)
        {

            var baseStats = level switch // Ottieni le statistiche base in base al livello
            {
                0 => (Esercito.Unità.Guerriero_1, Esercito.Unità.Lancere_1, Esercito.Unità.Arcere_1, Esercito.Unità.Catapulta_1),
                1 => (Esercito.Unità.Guerriero_2, Esercito.Unità.Lancere_2, Esercito.Unità.Arcere_2, Esercito.Unità.Catapulta_2),
                2 => (Esercito.Unità.Guerriero_3, Esercito.Unità.Lancere_3, Esercito.Unità.Arcere_3, Esercito.Unità.Catapulta_3),
                3 => (Esercito.Unità.Guerriero_4, Esercito.Unità.Lancere_4, Esercito.Unità.Arcere_4, Esercito.Unità.Catapulta_4),
                4 => (Esercito.Unità.Guerriero_5, Esercito.Unità.Lancere_5, Esercito.Unità.Arcere_5, Esercito.Unità.Catapulta_5),
                _ => (Esercito.Unità.Guerriero_1, Esercito.Unità.Lancere_1, Esercito.Unità.Arcere_1, Esercito.Unità.Catapulta_1)
            };

            return ( // Applica i bonus delle ricerche
                baseStats.Item1.Attacco + Ricerca.Soldati.Incremento.Attacco + (baseStats.Item1.Attacco * (1 + player.Bonus_Attacco_Guerrieri)), //Siamo siguri Incremento.Attacco * ? dovrebbe essere + ....
                baseStats.Item1.Difesa + Ricerca.Soldati.Incremento.Difesa + (baseStats.Item1.Difesa * (1 + player.Bonus_Difesa_Guerrieri)),
                baseStats.Item1.Salute + Ricerca.Soldati.Incremento.Salute + (baseStats.Item1.Salute * (1 + player.Bonus_Salute_Guerrieri)),

                baseStats.Item2.Attacco + Ricerca.Soldati.Incremento.Attacco + (baseStats.Item2.Attacco * (1 + player.Bonus_Attacco_Lanceri)),
                baseStats.Item2.Difesa + Ricerca.Soldati.Incremento.Difesa + (baseStats.Item2.Difesa * (1 + player.Bonus_Difesa_Lanceri)),
                baseStats.Item2.Salute + Ricerca.Soldati.Incremento.Salute + (baseStats.Item2.Salute * (1 + player.Bonus_Salute_Lanceri)),

                baseStats.Item3.Attacco + Ricerca.Soldati.Incremento.Attacco + (baseStats.Item3.Attacco * (1 + player.Bonus_Attacco_Arceri)),
                baseStats.Item3.Difesa + Ricerca.Soldati.Incremento.Difesa + (baseStats.Item3.Difesa * (1 + player.Bonus_Difesa_Arceri)),
                baseStats.Item3.Salute + Ricerca.Soldati.Incremento.Salute + (baseStats.Item3.Salute * (1 + player.Bonus_Salute_Arceri)),

                baseStats.Item4.Attacco + Ricerca.Soldati.Incremento.Attacco + (baseStats.Item4.Attacco * (1 + player.Bonus_Attacco_Catapulte)),
                baseStats.Item4.Difesa + Ricerca.Soldati.Incremento.Difesa + (baseStats.Item4.Difesa * (1 + player.Bonus_Difesa_Catapulte)),
                baseStats.Item4.Salute + Ricerca.Soldati.Incremento.Salute + (baseStats.Item4.Salute * (1 + player.Bonus_Salute_Catapulte))
            );
        }
        static int CalcoloFrecce(UnitGroup unità)
        {
            int frecce = unità.Arcieri[0] * Esercito.Unità.Arcere_1.Componente_Lancio + unità.Catapulte[0] * Esercito.Unità.Catapulta_1.Componente_Lancio;
            frecce += unità.Arcieri[1] * Esercito.Unità.Arcere_2.Componente_Lancio + unità.Catapulte[1] * Esercito.Unità.Catapulta_2.Componente_Lancio;
            frecce += unità.Arcieri[2] * Esercito.Unità.Arcere_3.Componente_Lancio + unità.Catapulte[2] * Esercito.Unità.Catapulta_3.Componente_Lancio;
            frecce += unità.Arcieri[3] * Esercito.Unità.Arcere_4.Componente_Lancio + unità.Catapulte[3] * Esercito.Unità.Catapulta_4.Componente_Lancio;
            frecce += unità.Arcieri[4] * Esercito.Unità.Arcere_5.Componente_Lancio + unità.Catapulte[4] * Esercito.Unità.Catapulta_5.Componente_Lancio;
            return frecce;
        }
        private static int RidurreNumeroSoldati(int numeroSoldati, double danno, double difesa, double salutePerSoldato)
        {
            if (numeroSoldati == 0 || salutePerSoldato == 0) return 0;
            double dannoEffettivo = Math.Max(0, danno - difesa);
            int soldatiPersi = (int)Math.Ceiling(dannoEffettivo / salutePerSoldato);
            return Math.Max(0, numeroSoldati - soldatiPersi);
        }
        private static BattagliaDistanza CalcolaAttaccoDistanza_(UnitGroup units, Giocatori.Player player, BattagliaDistanza result, bool difensore)
        {
            int totaleArceri = units.Arcieri.Sum();
            int totaleCatapulte = units.Catapulte.Sum();
            bool frecce_Insufficienti = false;

            if (totaleArceri == 0 && totaleCatapulte == 0)
            {
                //SendClient(clientGuid, "Log_Server|Nessuna unità a distanza disponibile.");
                return result;
            }

            int frecceNecessarie = CalcoloFrecce(units); // Calcola frecce necessarie

            // Calcola potenza d'attacco base
            int arcieriEffettivi = totaleArceri * 3 / 7;
            int catapulteEffettive = totaleCatapulte * 3 / 6;

            // Bonus per poche unità (sono più precise)
            if (totaleArceri > 0 && totaleArceri <= 15) arcieriEffettivi = totaleArceri * 3 / 6;
            if (totaleCatapulte > 0 && totaleCatapulte <= 10) catapulteEffettive = totaleCatapulte * 3 / 5;

            if (player.Frecce < frecceNecessarie) // Gestione frecce insufficienti
            {
                frecce_Insufficienti = true;
                arcieriEffettivi /= 3;
                catapulteEffettive /= 4;
            }

            int danno = arcieriEffettivi + catapulteEffettive;
            int attaccoTotale = danno > 0 ? danno : 0;
            if (attaccoTotale > 0)
            {
                int dannoGuerrieri = attaccoTotale * 3 / 5;
                int dannoLancieri = attaccoTotale * 2 / 5;

                if (difensore)
                {
                    result.Difensore_Danno_Guerrieri = dannoGuerrieri;
                    result.Difensore_Danno_Lancieri = dannoLancieri;
                }
                else
                {
                    result.Attaccante_Danno_Guerrieri = dannoGuerrieri;
                    result.Attaccante_Danno_Lancieri = dannoLancieri;
                }
            }

            int frecceUsate = frecce_Insufficienti ? (int)player.Frecce : frecceNecessarie; // Calcola frecce usate (tutte quelle disponibili se insufficienti)
            player.Frecce_Utilizzate += frecceUsate;
            player.Frecce -= frecceUsate;

            if (difensore)
            {
                result.Difensore_Frecce_Necessarie = frecceNecessarie;
                result.Difensore_Frecce_Usate = frecceUsate;
            }
            else
            {
                result.Attaccante_Frecce_Necessarie = frecceNecessarie;
                result.Attaccante_Frecce_Usate = frecceUsate;
            }
            return result;
        }
        private static void ApplicaDanni(int[] unita, int danno, int[] morti)
        {
            for (int i = 0; i < 5 && danno > 0; i++)
            {
                if (unita[i] <= 0) continue;
                int uccisi = Math.Min(unita[i], danno);
                unita[i] -= uccisi;
                morti[i] = uccisi;
                danno -= uccisi;
            }
        }
        private static UnitGroup ApplicaDanniDistanza_(UnitGroup units, int dannoGuerrieri, int dannoLancieri)
        {
            var morti = new UnitGroup();
            ApplicaDanni(units.Guerrieri, dannoGuerrieri, morti.Guerrieri);
            ApplicaDanni(units.Lancieri, dannoLancieri, morti.Lancieri);
            return morti;
        }
        private static double CalcolaDannoGiocatore(UnitGroup units, Giocatori.Player player, bool usaFrecce, bool attaccante, RisultatoFase result)
        {
            double dannoTotale = 0, moltiplicatoreDistanza = 1.0;
            int frecceNecessarie = CalcoloFrecce(units);

            if (usaFrecce)
            {
                bool frecceSufficienti = player.Frecce >= frecceNecessarie;
                int frecceUsate = frecceSufficienti ? frecceNecessarie : (int)player.Frecce;

                moltiplicatoreDistanza = frecceSufficienti ? 1.0 : 0.33;
                player.Frecce_Utilizzate += frecceUsate;
                player.Frecce = frecceSufficienti ? player.Frecce - frecceNecessarie : 0;

                if (attaccante)
                {
                    result.Fase_Distanza.Attaccante_Frecce_Usate = frecceUsate;
                    result.Fase_Distanza.Attaccante_Poche_Frecce = !frecceSufficienti;
                    if (!frecceSufficienti)
                        result.Fase_Distanza.Attaccante_Frecce_Necessarie = frecceNecessarie;
                }
                else
                {
                    result.Fase_Distanza.Difensore_Frecce_Usate = frecceUsate;
                    result.Fase_Distanza.Difensore_Poche_Frecce = !frecceSufficienti;
                    if (!frecceSufficienti)
                        result.Fase_Distanza.Difensore_Frecce_Necessarie = frecceNecessarie;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                var stats = GetPlayerUnitStats(i, player);
                dannoTotale += units.Guerrieri[i] * stats.GuerrieriAttacco;
                dannoTotale += units.Lancieri[i] * stats.LancieriAttacco;
                dannoTotale += units.Arcieri[i] * stats.ArcieriAttacco * moltiplicatoreDistanza;
                dannoTotale += units.Catapulte[i] * stats.CatapulteAttacco * moltiplicatoreDistanza;
            }
            return dannoTotale;
        }
        private static RisultatoFase ApplicaDanniGiocatore(RisultatoFase battle, UnitGroup units, Giocatori.Player player, double dannoPerTipo, double bonusUnità, bool attacco)
        {
            for (int i = 0; i < 5; i++)
            {
                var stats = GetPlayerUnitStats(i, player); //Qui ci sono stats unità con bonus applicati

                // Guerrieri
                int guerrieriIniziali = units.Guerrieri[i];
                units.Guerrieri[i] = RidurreNumeroSoldati(guerrieriIniziali, dannoPerTipo, stats.GuerrieriDifesa * guerrieriIniziali * bonusUnità, stats.GuerrieriSalute * bonusUnità);
                if (attacco == true)
                {
                    battle.Attaccante.Sopravvisuti.Guerrieri[i] = units.Guerrieri[i];
                    battle.Attaccante.Perdite.Guerrieri[i] += guerrieriIniziali - units.Guerrieri[i];
                }
                if (attacco == false)
                {
                    battle.Difensore.Sopravvisuti.Guerrieri[i] = units.Guerrieri[i];
                    battle.Difensore.Perdite.Guerrieri[i] += guerrieriIniziali - units.Guerrieri[i];
                }

                // Lancieri
                int lancieriIniziali = units.Lancieri[i];
                units.Lancieri[i] = RidurreNumeroSoldati(lancieriIniziali, dannoPerTipo, stats.LancieriDifesa * lancieriIniziali * bonusUnità, stats.LancieriSalute * bonusUnità);
                if (attacco == true)
                {
                    battle.Attaccante.Sopravvisuti.Lancieri[i] = units.Lancieri[i];
                    battle.Attaccante.Perdite.Lancieri[i] += lancieriIniziali - units.Lancieri[i];
                }
                if (attacco == false)
                {
                    battle.Difensore.Sopravvisuti.Lancieri[i] = units.Lancieri[i];
                    battle.Difensore.Perdite.Lancieri[i] += lancieriIniziali - units.Lancieri[i];
                }

                // Arcieri
                int arcieriIniziali = units.Arcieri[i];
                units.Arcieri[i] = RidurreNumeroSoldati(arcieriIniziali, dannoPerTipo, stats.ArcieriDifesa * arcieriIniziali * bonusUnità, stats.ArcieriSalute * bonusUnità);
                if (attacco == true)
                {
                    battle.Attaccante.Sopravvisuti.Arcieri[i] = units.Arcieri[i];
                    battle.Attaccante.Perdite.Arcieri[i] += arcieriIniziali - units.Arcieri[i];
                }
                if (attacco == false)
                {
                    battle.Difensore.Sopravvisuti.Arcieri[i] = units.Arcieri[i];
                    battle.Difensore.Perdite.Arcieri[i] += arcieriIniziali - units.Arcieri[i];
                }

                // Catapulte
                int catapulteIniziali = units.Catapulte[i];
                units.Catapulte[i] = RidurreNumeroSoldati(catapulteIniziali, dannoPerTipo, stats.CatapulteDifesa * catapulteIniziali * bonusUnità, stats.CatapulteSalute * bonusUnità);
                if (attacco == true)
                {
                    battle.Attaccante.Sopravvisuti.Catapulte[i] = units.Catapulte[i];
                    battle.Attaccante.Perdite.Catapulte[i] += catapulteIniziali - units.Catapulte[i];
                }
                if (attacco == false)
                {
                    battle.Difensore.Sopravvisuti.Catapulte[i] = units.Catapulte[i];
                    battle.Difensore.Perdite.Catapulte[i] += catapulteIniziali - units.Catapulte[i];
                }
            }
            return battle;
        }
        private static void AssegnaRisorseVittoria_PvP(Giocatori.Player attaccante, Giocatori.Player difensore, Guid attackerGuid, UnitGroup sopravvissuti)
        {
            int capacitàCarico = CapacitàCarico(sopravvissuti, attaccante);
            int capacitàOriginale = capacitàCarico;

            // Il 50% delle risorse del difensore può essere rubato
            double cibo = difensore.Cibo / 2;
            double legno = difensore.Legno / 2;
            double pietra = difensore.Pietra / 2;
            double ferro = difensore.Ferro / 2;
            double oro = difensore.Oro / 2;
            int diamantiViola = Variabili_Server.Max_Diamanti_Viola_PVP_Giocatore - difensore.Diamanti_Viola_PVP_Persi;
            int diamantiBlu = Variabili_Server.Max_Diamanti_Blu_PVP_Giocatore - difensore.Diamanti_Blu_PVP_Persi;

            var raccolte = RaccoliRisorseEquamente(capacitàCarico, cibo, legno, pietra, ferro, oro, 0, diamantiBlu, diamantiViola); // Raccogli risorse

            // Log dettagliato
            SendClient(attackerGuid, "Log_Server|╔══════════════════════════════════════════════════╗");
            SendClient(attackerGuid, "Log_Server|║        [highlight]RISORSE SACCHEGGIATE          ║");
            SendClient(attackerGuid, "Log_Server|╚══════════════════════════════════════════════════╝");

            //Controllo se è possibile rubare diamanti, se i limiti non sono stati superati
            if (attaccante.Diamanti_Viola_PVP_Ottenuti >= Variabili_Server.Max_Diamanti_Viola_PVP ||
                difensore.Diamanti_Viola_PVP_Persi >= Variabili_Server.Max_Diamanti_Viola_PVP_Giocatore)
            {
                raccolte.Diamanti_Viola = 0;
                SendClient(attackerGuid, $"Log_Server|Non è possibile saccheggiare ulteriori diamanti viola.\n" +
                    $"Limite giornaliero: [viola][icon:diamanteViola]{attaccante.Diamanti_Viola_PVP_Ottenuti}/{Variabili_Server.Max_Diamanti_Viola_PVP:N0}" +
                    $"Limite giocatore: [viola][icon:diamanteViola]{difensore.Diamanti_Viola_PVP_Persi}/{Variabili_Server.Max_Diamanti_Viola_PVP_Giocatore:N0}");

            }
            if (attaccante.Diamanti_Blu_PVP_Ottenuti >= Variabili_Server.max_Diamanti_Blu_PVP ||
                difensore.Diamanti_Blu_PVP_Persi >= Variabili_Server.Max_Diamanti_Blu_PVP_Giocatore)
            {
                raccolte.Diamanti_Blu = 0;
                SendClient(attackerGuid, $"Log_Server|Non è possibile saccheggiare ulteriori diamanti blu.\n" +
                    $"Limite giornaliero: [blu][icon:diamanteBlu]{attaccante.Diamanti_Blu_PVP_Ottenuti}/{Variabili_Server.max_Diamanti_Blu_PVP:N0}" +
                    $"Limite giocatore: [blu][icon:diamanteBlu]{difensore.Diamanti_Blu_PVP_Persi}/{Variabili_Server.Max_Diamanti_Blu_PVP_Giocatore:N0}");

            }

            // Assegna all'attaccante
            attaccante.Cibo += raccolte.Cibo;
            attaccante.Legno += raccolte.Legno;
            attaccante.Pietra += raccolte.Pietra;
            attaccante.Ferro += raccolte.Ferro;
            attaccante.Oro += raccolte.Oro;
            attaccante.Diamanti_Blu += raccolte.Diamanti_Blu;
            attaccante.Diamanti_Viola += raccolte.Diamanti_Viola;
            attaccante.Risorse_Razziate += raccolte.Cibo + raccolte.Legno + raccolte.Pietra + raccolte.Ferro + raccolte.Oro + raccolte.Diamanti_Blu + raccolte.Diamanti_Viola;

            // Rimuovi dal difensore
            difensore.Cibo -= raccolte.Cibo;
            difensore.Legno -= raccolte.Legno;
            difensore.Pietra -= raccolte.Pietra;
            difensore.Ferro -= raccolte.Ferro;
            difensore.Oro -= raccolte.Oro;
            difensore.Diamanti_Blu -= raccolte.Diamanti_Blu;
            difensore.Diamanti_Viola -= raccolte.Diamanti_Viola;

            difensore.Diamanti_Blu_PVP_Persi += raccolte.Diamanti_Blu;
            difensore.Diamanti_Viola_PVP_Persi += raccolte.Diamanti_Viola;
            attaccante.Diamanti_Viola_PVP_Ottenuti += raccolte.Diamanti_Viola;
            attaccante.Diamanti_Blu_PVP_Ottenuti += raccolte.Diamanti_Blu;

            // Calcola peso utilizzato
            int pesoUtilizzato =
                raccolte.Cibo * Variabili_Server.peso_Risorse_Cibo +
                raccolte.Legno * Variabili_Server.peso_Risorse_Legno +
                raccolte.Pietra * Variabili_Server.peso_Risorse_Pietra +
                raccolte.Ferro * Variabili_Server.peso_Risorse_Ferro +
                raccolte.Oro * Variabili_Server.peso_Risorse_Oro +
                raccolte.Diamanti_Blu * Variabili_Server.peso_Risorse_Diamante_Blu +
                raccolte.Diamanti_Viola * Variabili_Server.peso_Risorse_Diamante_Viola;

            SendClient(attackerGuid, $"Log_Server|Capacità di carico: [highlight]{capacitàOriginale:N0}");
            SendClient(attackerGuid, $"Log_Server|Capacità utilizzata: [verde]{pesoUtilizzato:N0}\n");

            if (raccolte.Cibo > 0) SendClient(attackerGuid, $"Log_Server|Cibo:   +[cibo][icon:cibo]{raccolte.Cibo:N0}[/cibo]");
            if (raccolte.Legno > 0) SendClient(attackerGuid, $"Log_Server|Legno:  +[legno][icon:legno]{raccolte.Legno:N0}[/legno]");
            if (raccolte.Pietra > 0) SendClient(attackerGuid, $"Log_Server|Pietra: +[pietra][icon:pietra]{raccolte.Pietra:N0}[/pietra]");
            if (raccolte.Ferro > 0) SendClient(attackerGuid, $"Log_Server|Ferro:  +[ferro][icon:ferro]{raccolte.Ferro:N0}[/ferro]");
            if (raccolte.Oro > 0) SendClient(attackerGuid, $"Log_Server|Oro:    +[oro][icon:oro]{raccolte.Oro:N0}[/oro]");
            if (raccolte.Diamanti_Blu > 0) SendClient(attackerGuid, $"Log_Server|Diamanti Blu:    +[blu][icon:diamanteBlu]{raccolte.Diamanti_Blu:N0}[/blu]");
            if (raccolte.Diamanti_Viola > 0) SendClient(attackerGuid, $"Log_Server|Diamanti Viola:    +[viola][icon:diamanteViola]{raccolte.Diamanti_Viola:N0}[/viola]");
            SendClient(attackerGuid, "Log_Server|════════════════════════════════════════════════════\n");
        }
        static int CapacitàCarico(UnitGroup playerUnits, Giocatori.Player player)
        {
            int capacitàCarico = 0;
            capacitàCarico += (int)(playerUnits.Guerrieri[0] * Esercito.Unità.Guerriero_1.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Guerrieri[1] * Esercito.Unità.Guerriero_2.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Guerrieri[2] * Esercito.Unità.Guerriero_3.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Guerrieri[3] * Esercito.Unità.Guerriero_4.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Guerrieri[4] * Esercito.Unità.Guerriero_5.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));

            capacitàCarico += (int)(playerUnits.Lancieri[0] * Esercito.Unità.Lancere_1.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Lancieri[1] * Esercito.Unità.Lancere_2.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Lancieri[2] * Esercito.Unità.Lancere_3.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Lancieri[3] * Esercito.Unità.Lancere_4.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Lancieri[4] * Esercito.Unità.Lancere_5.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));

            capacitàCarico += (int)(playerUnits.Arcieri[0] * Esercito.Unità.Arcere_1.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Arcieri[1] * Esercito.Unità.Arcere_2.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Arcieri[2] * Esercito.Unità.Arcere_3.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Arcieri[3] * Esercito.Unità.Arcere_4.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Arcieri[4] * Esercito.Unità.Arcere_5.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));

            capacitàCarico += (int)(playerUnits.Catapulte[0] * Esercito.Unità.Catapulta_1.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Catapulte[1] * Esercito.Unità.Catapulta_2.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Catapulte[2] * Esercito.Unità.Catapulta_3.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Catapulte[3] * Esercito.Unità.Catapulta_4.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            capacitàCarico += (int)(playerUnits.Catapulte[4] * Esercito.Unità.Catapulta_5.Trasporto * ((1 + player.Ricerca_Trasporto) * Ricerca.Tipi.Incremento.Trasporto));
            return (int)(capacitàCarico * (1 + player.Bonus_Capacità_Trasporto));//Aggiunge bonus trasporto
        }
        public static double CalcolaForza(UnitGroup units, double pesoAttacco = 0.8, double pesoDifesa = 0.5, double pesoSalute = 0.3)
        {
            double forza = 0;

            for (int i = 0; i < 5; i++)
            {
                var stats = GetUnitStats(i);

                forza += units.Guerrieri[i] * (stats.GuerrieriAttacco * pesoAttacco + stats.GuerrieriDifesa * pesoDifesa + stats.GuerrieriSalute * pesoSalute);
                forza += units.Lancieri[i] * (stats.LancieriAttacco * pesoAttacco + stats.LancieriDifesa * pesoDifesa + stats.LancieriSalute * pesoSalute);
                forza += units.Arcieri[i] * (stats.ArcieriAttacco * pesoAttacco + stats.ArcieriDifesa * pesoDifesa + stats.ArcieriSalute * pesoSalute);
                forza += units.Catapulte[i] * (stats.CatapulteAttacco * pesoAttacco + stats.CatapulteDifesa * pesoDifesa + stats.CatapulteSalute * pesoSalute);
            }
            //Aggiungere altri parametri come ricerca, bonus, etc...

            return Math.Round(forza);
        }
        private static RisorseRaccolte RaccoliRisorseEquamente(double capacitàCarico, double cibo, double legno, double pietra, double ferro, double oro, 
            int exp, int diamBlu, int diamViola)
        {
            var risultato = new RisorseRaccolte();
            int tipiRisorse = 0; // Conta quante risorse sono disponibili
            if (cibo > 0) tipiRisorse++;
            if (legno > 0) tipiRisorse++;
            if (pietra > 0) tipiRisorse++;
            if (ferro > 0) tipiRisorse++;
            if (oro > 0) tipiRisorse++;
            if (diamBlu > 0) tipiRisorse++;
            if (diamViola > 0) tipiRisorse++;
            if (tipiRisorse == 0) return risultato;
            double capacitàPerRisorsa = capacitàCarico / tipiRisorse; // Dividi equamente la capacità tra i tipi di risorse disponibili

            // FASE 1: Distribuisci equamente (dalle più leggere alle più pesanti)
            if (cibo > 0)
            {
                risultato.Cibo = (int)Math.Min(cibo, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Cibo);
                capacitàCarico -= risultato.Cibo * Variabili_Server.peso_Risorse_Cibo;
            }
            if (legno > 0)
            {
                risultato.Legno = (int)Math.Min(legno, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Legno);
                capacitàCarico -= risultato.Legno * Variabili_Server.peso_Risorse_Legno;
            }
            if (pietra > 0)
            {
                risultato.Pietra = (int)Math.Min(pietra, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Pietra);
                capacitàCarico -= risultato.Pietra * Variabili_Server.peso_Risorse_Pietra;
            }
            if (ferro > 0)
            {
                risultato.Ferro = (int)Math.Min(ferro, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Ferro);
                capacitàCarico -= risultato.Ferro * Variabili_Server.peso_Risorse_Ferro;
            }
            if (exp > 0) risultato.Esperienza = (int)Math.Min(exp, 0);
            if (oro > 0)
            {
                risultato.Oro = (int)Math.Min(oro, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Oro);
                capacitàCarico -= risultato.Oro * Variabili_Server.peso_Risorse_Oro;
            }
            if (diamBlu > 0)
            {
                risultato.Diamanti_Blu = (int)Math.Min(diamBlu, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Diamante_Blu);
                capacitàCarico -= risultato.Diamanti_Blu * Variabili_Server.peso_Risorse_Diamante_Blu;
            }
            if (diamViola > 0)
            {
                risultato.Diamanti_Viola = (int)Math.Min(diamViola, capacitàPerRisorsa / Variabili_Server.peso_Risorse_Diamante_Viola);
                capacitàCarico -= risultato.Diamanti_Viola * Variabili_Server.peso_Risorse_Diamante_Viola;
            }
            // FASE 2: Cicla finché c'è spazio disponibile e risorse da raccogliere
            bool haRaccolto = true;
            while (capacitàCarico >= Variabili_Server.peso_Risorse_Cibo && haRaccolto) // Minimo peso è 3
            {
                haRaccolto = false;

                if (cibo > risultato.Cibo && capacitàCarico >= Variabili_Server.peso_Risorse_Cibo)
                {
                    int extra = (int)Math.Min(cibo - risultato.Cibo, capacitàCarico / Variabili_Server.peso_Risorse_Cibo);
                    if (extra > 0)
                    {
                        risultato.Cibo += extra;
                        capacitàCarico -= extra * Variabili_Server.peso_Risorse_Cibo;
                        haRaccolto = true;
                    }
                }
                if (legno > risultato.Legno && capacitàCarico >= Variabili_Server.peso_Risorse_Legno)
                {
                    int extra = (int)Math.Min(legno - risultato.Legno, capacitàCarico / Variabili_Server.peso_Risorse_Legno);
                    if (extra > 0)
                    {
                        risultato.Legno += extra;
                        capacitàCarico -= extra * Variabili_Server.peso_Risorse_Legno;
                        haRaccolto = true;
                    }
                }
                if (pietra > risultato.Pietra && capacitàCarico >= Variabili_Server.peso_Risorse_Pietra)
                {
                    int extra = (int)Math.Min(pietra - risultato.Pietra, capacitàCarico / Variabili_Server.peso_Risorse_Pietra);
                    if (extra > 0)
                    {
                        risultato.Pietra += extra;
                        capacitàCarico -= extra * Variabili_Server.peso_Risorse_Pietra;
                        haRaccolto = true;
                    }
                }
                if (ferro > risultato.Ferro && capacitàCarico >= Variabili_Server.peso_Risorse_Ferro)
                {
                    int extra = (int)Math.Min(ferro - risultato.Ferro, capacitàCarico / Variabili_Server.peso_Risorse_Ferro);
                    if (extra > 0)
                    {
                        risultato.Ferro += extra;
                        capacitàCarico -= extra * Variabili_Server.peso_Risorse_Ferro;
                        haRaccolto = true;
                    }
                }
                if (oro > risultato.Oro && capacitàCarico >= Variabili_Server.peso_Risorse_Oro)
                {
                    int extra = (int)Math.Min(oro - risultato.Oro, capacitàCarico / Variabili_Server.peso_Risorse_Oro);
                    if (extra > 0)
                    {
                        risultato.Oro += extra;
                        capacitàCarico -= extra * Variabili_Server.peso_Risorse_Oro;
                        haRaccolto = true;
                    }
                }
                if (diamBlu > risultato.Diamanti_Blu && capacitàCarico >= Variabili_Server.peso_Risorse_Diamante_Blu)
                {
                    int extra = (int)Math.Min(diamBlu - risultato.Diamanti_Blu, capacitàCarico / Variabili_Server.peso_Risorse_Diamante_Blu);
                    if (extra > 0)
                    {
                        risultato.Diamanti_Blu += extra;
                        capacitàCarico -= extra * Variabili_Server.peso_Risorse_Diamante_Blu;
                        haRaccolto = true;
                    }
                }
                if (diamViola > risultato.Diamanti_Viola && capacitàCarico >= Variabili_Server.peso_Risorse_Diamante_Viola)
                {
                    int extra = (int)Math.Min(diamViola - risultato.Diamanti_Viola, capacitàCarico / Variabili_Server.peso_Risorse_Diamante_Viola);
                    if (extra > 0)
                    {
                        risultato.Diamanti_Viola += extra;
                        capacitàCarico -= extra * Variabili_Server.peso_Risorse_Diamante_Viola;
                        haRaccolto = true;
                    }
                }
            }
            return risultato;
        } // Distribuzione equa


        static async Task<bool> Battaglia(Giocatori.Player attaccante, Giocatori.Player difensore, UnitGroup attackerUnits)
        {
            var report = new Battaglia.Report();
            RisultatoFase fase = null;

            report.Tipo = "Battaglia";
            report.Data = DateTime.UtcNow.ToString();
            report.Aperto = false;
            report.Battaglia = new RisultatoBattaglia();
            report.Battaglia.Nome_Attaccante = attaccante.Username;
            report.Battaglia.Nome_Difensore = difensore.Username;
            report.Battaglia.Tipo_Battaglia = "PVP";

            int numeroFasi = 0;
            //1: Fase Battaglia
            for (int struttura = 1; struttura <= 6; struttura++)
            {
                fase = new RisultatoFase();
                numeroFasi = report.Battaglia.Fasi.Count;
                if (numeroFasi > 0) attackerUnits = report.Battaglia.Fasi[numeroFasi - 1].Attaccante.Sopravvisuti;

                fase = await Battaglia_Fase(attaccante, difensore, attackerUnits, fase, struttura);
                //Calcolo esperienza dal report...
                report.Battaglia.Xp_Attaccante += fase.Xp_Attaccante + fase.Fase_Distanza.Attaccante_XP;
                report.Battaglia.Xp_Difensore += fase.Xp_Difensore + fase.Fase_Distanza.Difensore_XP;
                report.Battaglia.Fasi.Add(fase);
            }

            //2: Villaggio Battaglia
            var fase_ultima = new RisultatoFase();
            fase_ultima = await Battaglia_Fase(attaccante, difensore, fase.Attaccante.Sopravvisuti, fase_ultima, 7);
            report.Battaglia.Fasi.Add(fase_ultima);
            report.Battaglia.Vittoria_Attaccante = fase_ultima.Vittoria_Attaccante;

            //Calcolo forza attaccante/difensore...
            report.Battaglia.Forza_Attaccante = CalcolaForza(attackerUnits);
            foreach (var f in report.Battaglia.Fasi)
                report.Battaglia.Forza_Difensore += CalcolaForza(f.Difensore.Schierati);

            //3: Risorse Battaglia
            if (report.Battaglia.Vittoria_Attaccante) AssegnaRisorseVittoria_PvP(attaccante, difensore, attaccante.guid_Player, fase.Attaccante.Sopravvisuti);

            //Aggiornamento dati attaccante
            numeroFasi = report.Battaglia.Fasi.Count;
            for (int i = 0; i < 5; i++)// aggiorna unità attaccante sottraendo le perdite totali (schierati - sopravvissuti) (Schierati inizio - Sopravvissuti finale)
            {
                attaccante.Guerrieri[i] -= report.Battaglia.Fasi[0].Attaccante.Schierati.Subtract(report.Battaglia.Fasi[numeroFasi - 1].Attaccante.Sopravvisuti).Guerrieri[i];
                attaccante.Lanceri[i] -= report.Battaglia.Fasi[0].Attaccante.Schierati.Subtract(report.Battaglia.Fasi[numeroFasi - 1].Attaccante.Sopravvisuti).Lancieri[i];
                attaccante.Arceri[i] -= report.Battaglia.Fasi[0].Attaccante.Schierati.Subtract(report.Battaglia.Fasi[numeroFasi - 1].Attaccante.Sopravvisuti).Arcieri[i];
                attaccante.Catapulte[i] -= report.Battaglia.Fasi[0].Attaccante.Schierati.Subtract(report.Battaglia.Fasi[numeroFasi - 1].Attaccante.Sopravvisuti).Catapulte[i];
            }
            int frecceAttaccante = report.Battaglia.Fasi.Sum(f => f.Fase_Distanza.Attaccante_Frecce_Usate);
            int frecceDifensore = report.Battaglia.Fasi.Sum(f => f.Fase_Distanza.Difensore_Frecce_Usate);

            //Salvare report dei giocatori interessati
            attaccante.Report.Add(report);
            difensore.Report.Add(report);

            AggiornaDatiGiocatori(attaccante, difensore, report);//Statistiche 

            //Quest
            OnEvent(attaccante, QuestEventType.Battaglie, "Attacco Giocatore", 1); //Quest

            OnEvent(attaccante, QuestEventType.Uccisioni, "Guerrieri", fase.Difensore.Perdite.Guerrieri.Sum());
            OnEvent(attaccante, QuestEventType.Uccisioni, "Lanceri", fase.Difensore.Perdite.Lancieri.Sum());
            OnEvent(attaccante, QuestEventType.Uccisioni, "Arceri", fase.Difensore.Perdite.Arcieri.Sum());
            OnEvent(attaccante, QuestEventType.Uccisioni, "Catapulte", fase.Difensore.Perdite.Catapulte.Sum());
            OnEvent(attaccante, QuestEventType.Risorse, "Frecce", frecceAttaccante);
            OnEvent(difensore, QuestEventType.Uccisioni, "Guerrieri", fase.Attaccante.Perdite.Guerrieri.Sum());
            OnEvent(difensore, QuestEventType.Uccisioni, "Lanceri", fase.Attaccante.Perdite.Lancieri.Sum());
            OnEvent(difensore, QuestEventType.Uccisioni, "Arceri", fase.Attaccante.Perdite.Arcieri.Sum());
            OnEvent(difensore, QuestEventType.Uccisioni, "Catapulte", fase.Attaccante.Perdite.Catapulte.Sum());
            OnEvent(difensore, QuestEventType.Risorse, "Frecce", frecceDifensore);

            return true;
        }
        static async void AggiornaDatiGiocatori(Giocatori.Player attaccante, Giocatori.Player difensore, Battaglia.Report report)
        {
            if (report.Battaglia.Vittoria_Attaccante)
            {
                attaccante.Battaglie_Vinte++; 
                difensore.Battaglie_Perse++;
            }
            else
            {
                attaccante.Battaglie_Perse++;
                difensore.Battaglie_Vinte++;
            }

            foreach (var item in report.Battaglia.Fasi) //Corpo a corpo + Distanza
            {
                attaccante.Guerrieri_Eliminati += item.Difensore.Perdite.Guerrieri.Sum() + item.Fase_Distanza.Difensore_Morti.Guerrieri.Sum();
                attaccante.Lanceri_Eliminati += item.Difensore.Perdite.Lancieri.Sum() + item.Fase_Distanza.Difensore_Morti.Lancieri.Sum();
                attaccante.Arceri_Eliminati += item.Difensore.Perdite.Arcieri.Sum() + item.Fase_Distanza.Difensore_Morti.Arcieri.Sum();
                attaccante.Catapulte_Eliminate += item.Difensore.Perdite.Catapulte.Sum() + item.Fase_Distanza.Difensore_Morti.Catapulte.Sum();
                attaccante.Guerrieri_Persi += item.Attaccante.Perdite.Guerrieri.Sum() + item.Fase_Distanza.Attaccante_Morti.Guerrieri.Sum();
                attaccante.Lanceri_Persi += item.Attaccante.Perdite.Lancieri.Sum() + item.Fase_Distanza.Attaccante_Morti.Lancieri.Sum();
                attaccante.Arceri_Persi += item.Attaccante.Perdite.Arcieri.Sum() + item.Fase_Distanza.Attaccante_Morti.Arcieri.Sum();
                attaccante.Catapulte_Perse += item.Attaccante.Perdite.Catapulte.Sum() + item.Fase_Distanza.Attaccante_Morti.Catapulte.Sum();
                attaccante.Unità_Eliminate += item.Difensore.Perdite.TotalUnits() + item.Fase_Distanza.Difensore_Morti.TotalUnits();
                attaccante.Unità_Perse += item.Attaccante.Perdite.TotalUnits() + item.Fase_Distanza.Attaccante_Morti.TotalUnits();
                attaccante.Frecce_Utilizzate += item.Fase_Distanza.Attaccante_Frecce_Usate;

                difensore.Guerrieri_Eliminati += item.Attaccante.Perdite.Guerrieri.Sum() + item.Fase_Distanza.Attaccante_Morti.Guerrieri.Sum();
                difensore.Lanceri_Eliminati += item.Attaccante.Perdite.Lancieri.Sum() + item.Fase_Distanza.Attaccante_Morti.Lancieri.Sum();
                difensore.Arceri_Eliminati += item.Attaccante.Perdite.Arcieri.Sum() + item.Fase_Distanza.Attaccante_Morti.Arcieri.Sum();
                difensore.Catapulte_Eliminate += item.Attaccante.Perdite.Catapulte.Sum() + item.Fase_Distanza.Attaccante_Morti.Catapulte.Sum();
                difensore.Guerrieri_Persi += item.Difensore.Perdite.Guerrieri.Sum() + item.Fase_Distanza.Difensore_Schierati.Guerrieri.Sum();
                difensore.Lanceri_Persi += item.Difensore.Perdite.Lancieri.Sum() + item.Fase_Distanza.Difensore_Schierati.Lancieri.Sum();
                difensore.Arceri_Persi += item.Difensore.Perdite.Arcieri.Sum() + item.Fase_Distanza.Difensore_Schierati.Arcieri.Sum();
                difensore.Catapulte_Perse += item.Difensore.Perdite.Catapulte.Sum() + item.Fase_Distanza.Difensore_Schierati.Catapulte.Sum();
                difensore.Unità_Eliminate += item.Attaccante.Perdite.TotalUnits() + item.Fase_Distanza.Attaccante_Morti.TotalUnits();
                difensore.Unità_Perse += item.Difensore.Perdite.TotalUnits() + item.Fase_Distanza.Difensore_Morti.TotalUnits();
                difensore.Frecce_Utilizzate += item.Fase_Distanza.Difensore_Frecce_Usate;
            }

            attaccante.Attacchi_Effettuati_PVP++;
            difensore.Attacchi_Subiti_PVP++;
        }
        static async Task<Battaglia.RisultatoFase> Battaglia_Fase(Giocatori.Player attaccante, Giocatori.Player difensore, UnitGroup attackerUnits, Battaglia.RisultatoFase fase, int struttura)
        {
            var defenderUnits = CaricaDatiStruttureDifensore(difensore, struttura);
            fase = new RisultatoFase
            {
                Struttura = struttura switch
                {
                    1 => new Edificio { Nome = "Ingresso", Guarnigione = defenderUnits.TotalUnits() },
                    2 => new Edificio { Nome = "Mura", Salute = fase.Struttura.Salute, Difesa = fase.Struttura.Difesa, Guarnigione = defenderUnits.TotalUnits() },
                    3 => new Edificio { Nome = "Cancello", Salute = fase.Struttura.Salute, Difesa = fase.Struttura.Difesa, Guarnigione = defenderUnits.TotalUnits() },
                    4 => new Edificio { Nome = "Torri", Salute = fase.Struttura.Salute, Difesa = fase.Struttura.Difesa, Guarnigione = defenderUnits.TotalUnits() },
                    5 => new Edificio { Nome = "Centro Villaggio", Guarnigione = defenderUnits.TotalUnits() },
                    6 => new Edificio { Nome = "Castello", Salute = fase.Struttura.Salute, Difesa = fase.Struttura.Difesa, Guarnigione = defenderUnits.TotalUnits() },
                    7 => new Edificio { Nome = "Villaggio", Guarnigione = defenderUnits.TotalUnits() }
                }
            };

            //Fase Distanza
            var rangedResult = BattagliaDistanza(attackerUnits, defenderUnits, attaccante, difensore);
            fase.Attaccante.Schierati = rangedResult.Attaccante_Sopravvisuti;
            fase.Difensore.Schierati = rangedResult.Difensore_Sopravvisuti;
            fase.Fase_Distanza = rangedResult;

            //Battaglia corpo a corpo
            fase = Battaglia_corpo_a_Corpo(fase.Attaccante.Schierati, fase.Difensore.Schierati, attaccante, difensore, struttura, fase);

            //Vittoria?
            if (fase.Struttura.Salute == 0 && fase.Struttura.Difesa == 0 || defenderUnits.TotalUnits() == 0) fase.Vittoria_Attaccante = true;

            AggiornaDatiStruttureDifensore(struttura, difensore, fase); //Aggiorna dati struttura difensore

            return fase;
        }
        private static BattagliaDistanza BattagliaDistanza(UnitGroup attackerUnits, UnitGroup defenderUnits, Giocatori.Player attaccante, Giocatori.Player difensore)
        {
            var result = new BattagliaDistanza();
            result.Attaccante_Schierati = attackerUnits;
            result.Difensore_Schierati = defenderUnits;

            if (defenderUnits.TotalUnits() == 0) result.Difensore_Unità_Presenti = false;
            else result.Difensore_Unità_Presenti = true;

            // ATTACCO DELL'ATTACCANTE
            result = CalcolaAttaccoDistanza_(attackerUnits, attaccante, result, false);
            result = CalcolaAttaccoDistanza_(defenderUnits, difensore, result, true);

            // Applica danni con tracciamento livelli
            var defenderMorti = ApplicaDanniDistanza_(defenderUnits.Clone(), result.Difensore_Danno_Guerrieri, result.Difensore_Danno_Lancieri);
            result.Difensore_Morti.Guerrieri = defenderMorti.Guerrieri;
            result.Difensore_Morti.Lancieri = defenderMorti.Lancieri;

            var attackerMorti = ApplicaDanniDistanza_(attackerUnits.Clone(), result.Attaccante_Danno_Guerrieri, result.Attaccante_Danno_Lancieri);
            result.Attaccante_Morti.Guerrieri = defenderMorti.Guerrieri;
            result.Attaccante_Morti.Lancieri = defenderMorti.Lancieri;

            for (int i = 0; i < 5; i++)
            {
                result.Attaccante_Sopravvisuti.Guerrieri[i] = result.Attaccante_Schierati.Guerrieri[i] - result.Attaccante_Morti.Guerrieri[i];
                result.Attaccante_Sopravvisuti.Lancieri[i] = result.Attaccante_Schierati.Lancieri[i] - result.Attaccante_Morti.Lancieri[i];
                result.Difensore_Sopravvisuti.Guerrieri[i] = result.Difensore_Schierati.Guerrieri[i] - result.Difensore_Morti.Guerrieri[i];
                result.Difensore_Sopravvisuti.Lancieri[i] = result.Difensore_Schierati.Lancieri[i] - result.Difensore_Morti.Lancieri[i];
            }
            result.Attaccante_Sopravvisuti.Arcieri = result.Attaccante_Schierati.Arcieri;
            result.Attaccante_Sopravvisuti.Catapulte = result.Attaccante_Schierati.Catapulte;

            result.Attaccante_XP = CalcolaEsperienzaPVP(result.Difensore_Morti);
            result.Difensore_XP = CalcolaEsperienzaPVP(result.Attaccante_Morti);

            return result;
        }
        private static RisultatoFase Battaglia_corpo_a_Corpo(UnitGroup attackerUnits, UnitGroup defenderUnits, Giocatori.Player attaccante, Giocatori.Player difensore, int struttura,
        RisultatoFase fase)
        {
            if (defenderUnits.TotalUnits() == 0) fase.Unità_Presenti_Difensore = false;
            else fase.Unità_Presenti_Difensore = true;

            int truppeAttaccante = attackerUnits.TotalUnits(); //Numero truppe
            int truppeDifensore = defenderUnits.TotalUnits();
            bool Frecce = false;

            if (truppeDifensore > 0 && fase.Struttura.Salute > 5) Frecce = true;
            double dannoAttaccante = CalcolaDannoGiocatore(attackerUnits, attaccante, Frecce, true, fase); // Calcola danno
            Frecce = false; //reset, per riutilizzo

            if (truppeAttaccante > 0 && struttura != 1 && struttura != 5) Frecce = true;
            else Frecce = false;
            double dannoDifensore = CalcolaDannoGiocatore(defenderUnits, difensore, Frecce, false, fase); // Calcola danno

            double dannotempDifesa = dannoAttaccante * 0.30; //Se struttura == 0, non serve a nulla questa variabile.  
            double dannotempSalute = 0; //Se struttura == 0, non serve a nulla questa variabile.
            double bonusUnità = 1;
            if (struttura != 0 && struttura != 5 && fase.Struttura.Salute > 5)
            {
                if (dannotempDifesa >= fase.Struttura.Difesa)
                {
                    dannotempDifesa -= fase.Struttura.Difesa;
                    dannoAttaccante -= fase.Struttura.Difesa;
                    fase.Struttura.Difesa = 0;
                }
                else
                {
                    fase.Struttura.Difesa -= (int)dannotempDifesa;
                    dannoAttaccante -= dannotempDifesa;
                }
                dannotempSalute = (dannoAttaccante - dannotempDifesa) * 0.20;
                if (dannotempSalute >= fase.Struttura.Salute)
                {
                    dannotempSalute -= fase.Struttura.Salute;
                    dannoAttaccante -= fase.Struttura.Salute;
                    fase.Struttura.Salute = 0;
                }
                else
                {
                    fase.Struttura.Salute -= (int)dannotempSalute;
                    dannoAttaccante -= dannotempSalute;
                }
            }
            if (struttura == 1)
            {
                if (difensore.Guarnigione_Cancello >= difensore.Guarnigione_CancelloMax * 0.40 || difensore.Guarnigione_Mura >= difensore.Guarnigione_MuraMax * 0.40) bonusUnità += 0.10;
                if (difensore.Guarnigione_Cancello >= difensore.Guarnigione_CancelloMax * 0.80 && difensore.Guarnigione_Mura > difensore.Guarnigione_MuraMax * 0.80) bonusUnità += 0.15;
                if (difensore.Guarnigione_Cancello == difensore.Guarnigione_CancelloMax && difensore.Guarnigione_Mura == difensore.Guarnigione_MuraMax) bonusUnità += 0.20;
            }

            double dannoPerTipoAttacker = dannoDifensore / attackerUnits.CountUnitTypes();
            double dannoPerTipoDefender = dannoAttaccante / defenderUnits.CountUnitTypes();
            fase = ApplicaDanniGiocatore(fase, attackerUnits.Clone(), attaccante, dannoPerTipoAttacker, 1, true); // Applica danni all'attaccante
            fase = ApplicaDanniGiocatore(fase, defenderUnits.Clone(), difensore, dannoPerTipoDefender, bonusUnità, false); //Difensore con bonus guarnigione

            // Calcola esperienza corpo a corpo per entrambi
            fase.Xp_Attaccante = CalcolaEsperienzaPVP(fase.Difensore.Perdite); // Per il log
            fase.Xp_Difensore = CalcolaEsperienzaPVP(fase.Attaccante.Perdite);

            fase.Vittoria_Attaccante = fase.Difensore.Sopravvisuti.TotalUnits() == 0; // Determina vittoria provvisoria (sarà confermata in base alle perdite totali)
            return fase;
        }

        public static async void TestBattaglia()
        {
            int[] guerrieri = new int[] { 60, 0, 0, 0, 0 };
            int[] picchieri = new int[] { 50, 0, 0, 0, 0 };
            int[] arcieri = new int[]   { 15, 0, 0, 0, 0 };
            int[] catapulte = new int[] { 10, 0, 0, 0, 0 };

            var unitàStrutture = new UnitGroup
            {
                Guerrieri = new int[] { 5, 0, 0, 0, 0 },
                Lancieri = new int[] { 5, 0, 0, 0, 0 },
                Arcieri = new int[] { 5, 0, 0, 0, 0 },
                Catapulte = new int[] { 0, 0, 0, 0, 0 }
            };
            var unitàDifensore = new UnitGroup
            {
                Guerrieri = new int[] { 25, 0, 0, 0, 0 },
                Lancieri = new int[] { 20, 0, 0, 0, 0 },
                Arcieri = new int[] { 15, 0, 0, 0, 0 },
                Catapulte = new int[] { 10, 0, 0, 0, 0 }
            };

            bool test1 = await Server.ServerConnection.New_Player("adly", "123", Guid.Empty);
            var difensore = Server.Server.servers_.GetPlayer_Data("adly");
            difensore.Guerrieri_Ingresso = unitàStrutture.Guerrieri;
            difensore.Lanceri_Ingresso = unitàStrutture.Lancieri;
            difensore.Arceri_Ingresso = unitàStrutture.Arcieri;

            difensore.Guerrieri_Mura = unitàStrutture.Guerrieri;
            difensore.Lanceri_Mura = unitàStrutture.Lancieri;
            difensore.Arceri_Mura = unitàStrutture.Arcieri;

            difensore.Guerrieri_Cancello = unitàStrutture.Guerrieri;
            difensore.Lanceri_Cancello = unitàStrutture.Lancieri;
            difensore.Arceri_Cancello = unitàStrutture.Arcieri;

            difensore.Guerrieri_Torri = unitàStrutture.Guerrieri;
            difensore.Lanceri_Torri = unitàStrutture.Lancieri;
            difensore.Arceri_Torri = unitàStrutture.Arcieri;

            difensore.Guerrieri_Citta = unitàStrutture.Guerrieri;
            difensore.Lanceri_Citta = unitàStrutture.Lancieri;
            difensore.Arceri_Citta = unitàStrutture.Arcieri;

            difensore.Guerrieri_Castello = unitàStrutture.Guerrieri;
            difensore.Lanceri_Castello = unitàStrutture.Lancieri;
            difensore.Arceri_Castello = unitàStrutture.Arcieri;

            difensore.Guerrieri = unitàDifensore.Guerrieri;
            difensore.Lanceri = unitàDifensore.Lancieri;
            difensore.Arceri = unitàDifensore.Arcieri;

            difensore.Frecce = 5000;

            //Aggiungere giocatore x test
            bool test2 = await Server.ServerConnection.New_Player("TEST", "123", Guid.Empty);

            var attaccante = Server.Server.servers_.GetPlayer_Data("TEST");
            attaccante.Frecce = 5000;

            var attackerUnits = new UnitGroup
            {
                Guerrieri = guerrieri,
                Lancieri = picchieri,
                Arcieri = arcieri,
                Catapulte = catapulte
            };
            var DefenderUnits = new UnitGroup()
            {
                Guerrieri = guerrieri,
                Lancieri = picchieri,
                Arcieri = arcieri,
                Catapulte = catapulte
            };

            await Battaglia(attaccante, difensore, attackerUnits);
        }
    }
}
