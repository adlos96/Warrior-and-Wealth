using static Server_Strategico.Gioco.Esercito;
using static Server_Strategico.Gioco.Ricerca;
using static Server_Strategico.Gioco.Strutture;
using static Server_Strategico.Server.Server;

namespace Server_Strategico.Gioco
{
    internal class Ricerca
    {
        public class dati
        {
            public double Cibo { get; set; }
            public double Legno { get; set; }
            public double Pietra { get; set; }
            public double Ferro { get; set; }
            public double Oro { get; set; }
            public double Popolazione { get; set; }

            public double Salute { get; set; }
            public double Difesa { get; set; }
            public double Attacco { get; set; }
            public double Guarnigione { get; set; }

            public double Incremento { get; set; }
            public double TempoCostruzione { get; set; }
        }
        public class Tipi
        {
            public static dati Costruzione = new dati
            {
                Cibo = 3500,
                Legno = 3250,
                Pietra = 3000,
                Ferro = 2750,
                Oro = 2550,
                TempoCostruzione = 180
            };
            public static dati Produzione = new dati
            {
                Cibo = 4500,
                Legno = 4250,
                Pietra = 4000,
                Ferro = 3750,
                Oro = 3500,
                TempoCostruzione = 180
            };
            public static dati Addestramento = new dati
            {
                Cibo = 5500,
                Legno = 5000,
                Pietra = 4500,
                Ferro = 4250,
                Oro = 4000,
                TempoCostruzione = 180
            };
            public static dati Popolazione = new dati
            {
                Cibo = 6500,
                Legno = 6000,
                Pietra = 5500,
                Ferro = 5250,
                Oro = 5000,
                TempoCostruzione = 180
            };

            public static CostoReclutamento Incremento = new CostoReclutamento
            {
                Cibo = 0.12,
                Legno = 0.10,
                Pietra = 0.08,
                Ferro = 0.06,
                Oro = 0.04,
                Popolazione = 0.001,
                Spade = 0.01,
                Lance = 0.01,
                Archi = 0.01,
                Scudi = 0.01,
                Armature = 0.01
            };

        }
        public class Soldati
        {
            public static dati Salute = new dati
            {
                Cibo = 4000,
                Legno = 3750,
                Pietra = 3500,
                Ferro = 3250,
                Oro = 3000,
                TempoCostruzione = 180
            };
            public static dati Difesa = new dati
            {
                Cibo = 3000,
                Legno = 2750,
                Pietra = 2500,
                Ferro = 2500,
                Oro = 2250,
                TempoCostruzione = 180
            };
            public static dati Attacco = new dati
            {
                Cibo = 5000,
                Legno = 4500,
                Pietra = 4000,
                Ferro = 3750,
                Oro = 3500,
                TempoCostruzione = 180
            };
            public static dati Livello = new dati
            {
                Cibo = 6000,
                Legno = 5750,
                Pietra = 5500,
                Ferro = 5250,
                Oro = 5000,
                TempoCostruzione = 180
            };

            public static dati Incremento = new dati
            {
                Salute = 1,
                Difesa = 1,
                Attacco = 1
            };

        }
        public class Citta
        {
            public static dati Ingresso = new dati
            {
                Cibo = 4000,
                Legno = 3750,
                Pietra = 3500,
                Ferro = 3250,
                Oro = 3000,
                TempoCostruzione = 180,
                Guarnigione = 25,
                Popolazione = 35

            };
            public static dati Città = new dati
            {
                Cibo = 3000,
                Legno = 2750,
                Pietra = 2500,
                Ferro = 2500,
                Oro = 2250,
                TempoCostruzione = 180,
                Guarnigione = 50,
                Popolazione = 35
            };
            public static dati Cancello = new dati
            {
                Cibo = 5000,
                Legno = 4500,
                Pietra = 4000,
                Ferro = 3750,
                Oro = 3500,
                TempoCostruzione = 180,
                Salute = 50,
                Difesa = 30,
                Guarnigione = 25,
                Popolazione = 35
            };
            public static dati Mura = new dati
            {
                Cibo = 6000,
                Legno = 5750,
                Pietra = 5500,
                Ferro = 5250,
                Oro = 5000,
                TempoCostruzione = 180,
                Salute = 50,
                Difesa = 30,
                Guarnigione = 25,
                Popolazione = 35
            };
            public static dati Torri = new dati
            {
                Cibo = 6000,
                Legno = 5750,
                Pietra = 5500,
                Ferro = 5250,
                Oro = 5000,
                TempoCostruzione = 180,
                Salute = 50,
                Difesa = 30,
                Guarnigione = 25,
                Popolazione = 35
            };
            public static dati Castello = new dati
            {
                Cibo = 6000,
                Legno = 5750,
                Pietra = 5500,
                Ferro = 5250,
                Oro = 5000,
                TempoCostruzione = 180,
                Salute = 60,
                Difesa = 35,
                Guarnigione = 25,
                Popolazione = 35
            };
        }
        public static async Task<bool> Ricerca_Truppe(Giocatori.Player player, Guid clientGuid, string tipo, string unità)
        {
            int livello = 0, valore = 0;
            double costo_cibo = 0, costo_legno = 0, costo_pietra = 0, costo_ferro = 0, costo_oro = 0, costo_popolazione = 0;

            int livello_Ricerca = 0;
            int livello_Giocatore = player.Livello;

            if (unità == "Guerriero" && tipo == "Livello")
            {
                livello_Ricerca = player.Guerriero_Livello;
                costo_cibo = Soldati.Livello.Cibo;
                costo_legno = Soldati.Livello.Legno;
                costo_pietra = Soldati.Livello.Pietra;
                costo_ferro = Soldati.Livello.Ferro;
                costo_oro = Soldati.Livello.Oro;
            }
            if (unità == "Guerriero" && tipo == "Salute")
            {
                livello_Ricerca = player.Guerriero_Salute;
                costo_cibo = Soldati.Salute.Cibo;
                costo_legno = Soldati.Salute.Legno;
                costo_pietra = Soldati.Salute.Pietra;
                costo_ferro = Soldati.Salute.Ferro;
                costo_oro = Soldati.Salute.Oro;
            }
            if (unità == "Guerriero" && tipo == "Difesa")
            {
                livello_Ricerca = player.Guerriero_Difesa;
                costo_cibo = Soldati.Difesa.Cibo;
                costo_legno = Soldati.Difesa.Legno;
                costo_pietra = Soldati.Difesa.Pietra;
                costo_ferro = Soldati.Difesa.Ferro;
                costo_oro = Soldati.Difesa.Oro;
            }
            if (unità == "Guerriero" && tipo == "Attacco")
            {
                livello_Ricerca = player.Guerriero_Attacco;
                costo_cibo = Soldati.Attacco.Cibo;
                costo_legno = Soldati.Attacco.Legno;
                costo_pietra = Soldati.Attacco.Pietra;
                costo_ferro = Soldati.Attacco.Ferro;
                costo_oro = Soldati.Attacco.Oro;
            }

            if (unità == "Lancere" && tipo == "Livello")
            {
                livello_Ricerca = player.Lancere_Livello;
                costo_cibo = Soldati.Livello.Cibo;
                costo_legno = Soldati.Livello.Legno;
                costo_pietra = Soldati.Livello.Pietra;
                costo_ferro = Soldati.Livello.Ferro;
                costo_oro = Soldati.Livello.Oro;
            }
            if (unità == "Lancere" && tipo == "Salute")
            {
                livello_Ricerca = player.Lancere_Salute;
                costo_cibo = Soldati.Salute.Cibo;
                costo_legno = Soldati.Salute.Legno;
                costo_pietra = Soldati.Salute.Pietra;
                costo_ferro = Soldati.Salute.Ferro;
                costo_oro = Soldati.Salute.Oro;
            }
            if (unità == "Lancere" && tipo == "Difesa")
            {
                livello_Ricerca = player.Lancere_Difesa;
                costo_cibo = Soldati.Difesa.Cibo;
                costo_legno = Soldati.Difesa.Legno;
                costo_pietra = Soldati.Difesa.Pietra;
                costo_ferro = Soldati.Difesa.Ferro;
                costo_oro = Soldati.Difesa.Oro;
            }
            if (unità == "Lancere" && tipo == "Attacco")
            {
                livello_Ricerca = player.Lancere_Attacco;
                costo_cibo = Soldati.Attacco.Cibo;
                costo_legno = Soldati.Attacco.Legno;
                costo_pietra = Soldati.Attacco.Pietra;
                costo_ferro = Soldati.Attacco.Ferro;
                costo_oro = Soldati.Attacco.Oro;
            }

            if (unità == "Arcere" && tipo == "Livello")
            {
                livello_Ricerca = player.Arcere_Livello;
                costo_cibo = Soldati.Livello.Cibo;
                costo_legno = Soldati.Livello.Legno;
                costo_pietra = Soldati.Livello.Pietra;
                costo_ferro = Soldati.Livello.Ferro;
                costo_oro = Soldati.Livello.Oro;
            }
            if (unità == "Arcere" && tipo == "Salute")
            {
                livello_Ricerca = player.Arcere_Salute;
                costo_cibo = Soldati.Salute.Cibo;
                costo_legno = Soldati.Salute.Legno;
                costo_pietra = Soldati.Salute.Pietra;
                costo_ferro = Soldati.Salute.Ferro;
                costo_oro = Soldati.Salute.Oro;
            } 
                
            if (unità == "Arcere" && tipo == "Difesa")
            {
                livello_Ricerca = player.Arcere_Difesa;
                costo_cibo = Soldati.Difesa.Cibo;
                costo_legno = Soldati.Difesa.Legno;
                costo_pietra = Soldati.Difesa.Pietra;
                costo_ferro = Soldati.Difesa.Ferro;
                costo_oro = Soldati.Difesa.Oro;
            }
            if (unità == "Arcere" && tipo == "Attacco")
            {
                livello_Ricerca = player.Arcere_Attacco;
                costo_cibo = Soldati.Attacco.Cibo;
                costo_legno = Soldati.Attacco.Legno;
                costo_pietra = Soldati.Attacco.Pietra;
                costo_ferro = Soldati.Attacco.Ferro;
                costo_oro = Soldati.Attacco.Oro;
            }

            if (unità == "Catapulte" && tipo == "Livello")
            {
                livello_Ricerca = player.Catapulta_Livello;
                costo_cibo = Soldati.Livello.Cibo;
                costo_legno = Soldati.Livello.Legno;
                costo_pietra = Soldati.Livello.Pietra;
                costo_ferro = Soldati.Livello.Ferro;
                costo_oro = Soldati.Livello.Oro;
            }
            if (unità == "Catapulte" && tipo == "Salute")
            {
                livello_Ricerca = player.Catapulta_Salute;
                costo_cibo = Soldati.Salute.Cibo;
                costo_legno = Soldati.Salute.Legno;
                costo_pietra = Soldati.Salute.Pietra;
                costo_ferro = Soldati.Salute.Ferro;
                costo_oro = Soldati.Salute.Oro;
            }
            if (unità == "Catapulte" && tipo == "Difesa")
            {
                livello_Ricerca = player.Catapulta_Difesa;
                costo_cibo = Soldati.Difesa.Cibo;
                costo_legno = Soldati.Difesa.Legno;
                costo_pietra = Soldati.Difesa.Pietra;
                costo_ferro = Soldati.Difesa.Ferro;
                costo_oro = Soldati.Difesa.Oro;
            }
            if (unità == "Catapulte" && tipo == "Attacco")
            {
                livello_Ricerca = player.Catapulta_Attacco;
                costo_cibo = Soldati.Attacco.Cibo;
                costo_legno = Soldati.Attacco.Legno;
                costo_pietra = Soldati.Attacco.Pietra;
                costo_ferro = Soldati.Attacco.Ferro;
                costo_oro = Soldati.Attacco.Oro;
            }

            int valore_Target = livello_Ricerca + (livello_Ricerca * 2);
            livello_Ricerca++;

            if (livello_Ricerca < valore_Target)
            {
                Send(clientGuid, $"Log_Server|La ricerca [{tipo} {unità} {livello}], richiede che il {unità} sia almeno di livello: {valore * 2}\r\n");
                Console.WriteLine($"La ricerca {tipo} {unità} {livello}, richiede che il {unità} sia almeno di livello: {valore * 2}\r\n");
                return false;
            }

            if (player.Cibo >= costo_cibo * livello_Ricerca &&
                player.Legno >= costo_legno * livello_Ricerca &&
                player.Pietra >= costo_pietra * livello_Ricerca &&
                player.Ferro >= costo_ferro * livello_Ricerca &&
                player.Oro >= costo_oro * livello)
            {
                // Sottrai le risorse necessarie
                player.Cibo -= costo_cibo * livello_Ricerca;
                player.Legno -= costo_legno * livello_Ricerca;
                player.Pietra -= costo_pietra * livello_Ricerca;
                player.Ferro -= costo_ferro * livello_Ricerca;
                player.Oro -= costo_oro * livello_Ricerca;

                Send(clientGuid, $"Log_Server|Risorse utilizzate per la ricerca di {tipo} {unità} {livello_Ricerca}:\r\n " +
                    $"Cibo= {costo_cibo * livello_Ricerca}," +
                    $" Legno= {costo_legno * livello_Ricerca}," +
                    $" Pietra= {costo_pietra * livello_Ricerca}," +
                    $" Ferro= {costo_ferro * livello_Ricerca}," +
                    $" Oro= {costo_oro * livello_Ricerca}\r\n");
                Console.WriteLine($"Risorse utilizzate per la ricerca di {tipo} {unità} {livello_Ricerca}:\r\n " +
                    $"Cibo= {costo_cibo * livello_Ricerca}, " +
                    $"Legno= {costo_legno * livello_Ricerca}, " +
                    $"Pietra= {costo_pietra * livello_Ricerca}, " +
                    $"Ferro= {costo_ferro * livello_Ricerca}, " +
                    $"Oro= {costo_oro * livello_Ricerca}\r\n");
            }
            else
            {
                Send(clientGuid, $"Log_Server|Risorse insufficienti per la ricerca di {tipo} {unità} {livello_Ricerca}:\r\n " +
                    $"Cibo= {costo_cibo * livello_Ricerca}," +
                    $" Legno= {costo_legno * livello_Ricerca}," +
                    $" Pietra= {costo_pietra * livello_Ricerca}," +
                    $" Ferro= {costo_ferro * livello_Ricerca}," +
                    $" Oro= {costo_oro * livello_Ricerca}\r\n");
                Console.WriteLine($"Risorse insufficienti per la ricerca di {tipo} {unità} {livello_Ricerca}:\r\n " +
                    $"Cibo= {costo_cibo * livello_Ricerca}, " +
                    $"Legno= {costo_legno * livello_Ricerca}, " +
                    $"Pietra= {costo_pietra * livello_Ricerca}, " +
                    $"Ferro= {costo_ferro * livello_Ricerca}, " +
                    $"Oro= {costo_oro * livello_Ricerca}\r\n");
                return false;
            }

            switch (unità)
            {
                case "Guerriero":
                    if (tipo == "Salute")
                    {
                        player.Guerriero_Salute++;
                        return true;
                    }
                    if (tipo == "Difesa")
                    {
                        player.Guerriero_Difesa++;
                        return true;
                    }
                    if (tipo == "Attacco")
                    {
                        player.Guerriero_Attacco++;
                        return true;
                    }
                    if (tipo == "Livello")
                    {
                        player.Guerriero_Livello++;
                        return true;
                    }
                    Console.WriteLine($"Ricerca {tipo} {unità} completata!");
                    break;
                case "Lancere":
                    if (tipo == "Salute")
                    {
                        player.Lancere_Salute++;
                        return true;
                    }
                    else if (tipo == "Difesa")
                    {
                        player.Lancere_Difesa++;
                        return true;
                    }
                    else if (tipo == "Attacco")
                    {
                        player.Lancere_Attacco++;
                        return true;
                    }
                    else if (tipo == "Livello")
                    {
                        player.Lancere_Livello++;
                        return true;
                    }
                    Console.WriteLine($"Ricerca {tipo} {unità} completata!");
                    break;
                case "Arciere":
                    if (tipo == "Salute")
                    {
                        player.Arcere_Salute++;
                        return true;
                    }
                    else if (tipo == "Difesa")
                    {
                        player.Arcere_Difesa++;
                        return true;
                    }
                    else if (tipo == "Attacco")
                    {
                        player.Arcere_Attacco++;
                        return true;
                    }
                    else if (tipo == "Livello")
                    {
                        player.Arcere_Livello++;
                        return true;
                    }
                    Console.WriteLine($"Ricerca {tipo} {unità} completata!");
                    break;
                case "Catapulte":
                    if (tipo == "Salute")
                    {
                        player.Catapulta_Salute++;
                        return true;
                    }
                    else if (tipo == "Difesa")
                    {
                        player.Catapulta_Difesa++;
                        return true;
                    }
                    else if (tipo == "Attacco")
                    {
                        player.Catapulta_Attacco++;
                        return true;
                    }
                    else if (tipo == "Livello")
                    {
                        player.Catapulta_Livello++;
                        return true;
                    }
                    Console.WriteLine($"Ricerca {tipo} {unità} completata!");
                    break;
                default:
                    Console.WriteLine($"Il tipo di ricerca {tipo} {unità} non supportata!");
                    break;
            }
            return false;
        }
        public static async Task<bool> Ricerca_Citta(Giocatori.Player player, Guid clientGuid, string Edificio, string Stato)
        {
            int livello_Ricerca = 0;
            double costo_cibo = 0, costo_legno = 0, costo_pietra = 0, costo_ferro = 0, costo_oro = 0, costo_popolazione = 0;
            int livello_Giocatore = player.Livello;

            if (Edificio == "Ingresso" && Stato == "Guarnigione")
            { 
                livello_Ricerca = player.Ricerca_Ingresso_Guarnigione;
                costo_cibo = Citta.Ingresso.Cibo;
                costo_legno = Citta.Ingresso.Legno;
                costo_pietra = Citta.Ingresso.Pietra;
                costo_ferro = Citta.Ingresso.Ferro;
                costo_oro = Citta.Ingresso.Oro;
                costo_popolazione = Citta.Ingresso.Popolazione;

            }
            if (Edificio == "Citta" && Stato == "Guarnigione")
            {
                livello_Ricerca = player.Ricerca_Citta_Guarnigione;
                costo_cibo = Citta.Città.Cibo;
                costo_legno = Citta.Città.Legno;
                costo_pietra = Citta.Città.Pietra;
                costo_ferro = Citta.Città.Ferro;
                costo_oro = Citta.Città.Oro;
                costo_popolazione = Citta.Città.Popolazione;
            }
            if (Edificio == "Cancello" && Stato == "Guarnigione")
            {
                livello_Ricerca = player.Ricerca_Cancello_Guarnigione;
                costo_cibo = Citta.Cancello.Cibo;
                costo_legno = Citta.Cancello.Legno;
                costo_pietra = Citta.Cancello.Pietra;
                costo_ferro = Citta.Cancello.Ferro;
                costo_oro = Citta.Cancello.Oro;
                costo_popolazione = Citta.Cancello.Popolazione;
            }
            if (Edificio == "Cancello" && Stato == "Salute")
            {
                livello_Ricerca = player.Ricerca_Cancello_Salute;
                costo_cibo = Citta.Cancello.Cibo;
                costo_legno = Citta.Cancello.Legno;
                costo_pietra = Citta.Cancello.Pietra;
                costo_ferro = Citta.Cancello.Ferro;
                costo_oro = Citta.Cancello.Oro;
                costo_popolazione = Citta.Cancello.Popolazione;
            }
            if (Edificio == "Cancello" && Stato == "Difesa")
            {
                livello_Ricerca = player.Ricerca_Cancello_Difesa;
                costo_cibo = Citta.Cancello.Cibo;
                costo_legno = Citta.Cancello.Legno;
                costo_pietra = Citta.Cancello.Pietra;
                costo_ferro = Citta.Cancello.Ferro;
                costo_oro = Citta.Cancello.Oro;
                costo_popolazione = Citta.Cancello.Popolazione;
            }
            if (Edificio == "Mura" && Stato == "Guarnigione")
            {
                livello_Ricerca = player.Ricerca_Mura_Guarnigione;
                costo_cibo = Citta.Mura.Cibo;
                costo_legno = Citta.Mura.Legno;
                costo_pietra = Citta.Mura.Pietra;
                costo_ferro = Citta.Mura.Ferro;
                costo_oro = Citta.Mura.Oro;
                costo_popolazione = Citta.Mura.Popolazione;
            }
            if (Edificio == "Mura" && Stato == "Salute")
            {
                livello_Ricerca = player.Ricerca_Mura_Salute;
                costo_cibo = Citta.Mura.Cibo;
                costo_legno = Citta.Mura.Legno;
                costo_pietra = Citta.Mura.Pietra;
                costo_ferro = Citta.Mura.Ferro;
                costo_oro = Citta.Mura.Oro;
                costo_popolazione = Citta.Mura.Popolazione;
            }
            if (Edificio == "Mura" && Stato == "Difesa")
            {
                livello_Ricerca = player.Ricerca_Mura_Difesa;
                costo_cibo = Citta.Mura.Cibo;
                costo_legno = Citta.Mura.Legno;
                costo_pietra = Citta.Mura.Pietra;
                costo_ferro = Citta.Mura.Ferro;
                costo_oro = Citta.Mura.Oro;
                costo_popolazione = Citta.Mura.Popolazione;
            }

            if (Edificio == "Torri" && Stato == "Guarnigione") 
            {
                livello_Ricerca = player.Ricerca_Torri_Guarnigione;
                costo_cibo = Citta.Torri.Cibo;
                costo_legno = Citta.Torri.Legno;
                costo_pietra = Citta.Torri.Pietra;
                costo_ferro = Citta.Torri.Ferro;
                costo_oro = Citta.Torri.Oro;
                costo_popolazione = Citta.Torri.Popolazione;
            }
            if (Edificio == "Torri" && Stato == "Salute")
            {
                livello_Ricerca = player.Ricerca_Torri_Salute;
                costo_cibo = Citta.Torri.Cibo;
                costo_legno = Citta.Torri.Legno;
                costo_pietra = Citta.Torri.Pietra;
                costo_ferro = Citta.Torri.Ferro;
                costo_oro = Citta.Torri.Oro;
                costo_popolazione = Citta.Torri.Popolazione;
            }
            if (Edificio == "Torri" && Stato == "Difesa")
            {
                livello_Ricerca = player.Ricerca_Torri_Difesa;
                costo_cibo = Citta.Torri.Cibo;
                costo_legno = Citta.Torri.Legno;
                costo_pietra = Citta.Torri.Pietra;
                costo_ferro = Citta.Torri.Ferro;
                costo_oro = Citta.Torri.Oro;
                costo_popolazione = Citta.Torri.Popolazione;
            }
            if (Edificio == "Castello" && Stato == "Guarnigione")
            {
                livello_Ricerca = player.Ricerca_Castello_Guarnigione;
                costo_cibo = Citta.Castello.Cibo;
                costo_legno = Citta.Castello.Legno;
                costo_pietra = Citta.Castello.Pietra;
                costo_ferro = Citta.Castello.Ferro;
                costo_oro = Citta.Castello.Oro;
                costo_popolazione = Citta.Castello.Popolazione;
            }
            if (Edificio == "Castello" && Stato == "Salute")
            {
                livello_Ricerca = player.Ricerca_Castello_Salute;
                costo_cibo = Citta.Castello.Cibo;
                costo_legno = Citta.Castello.Legno;
                costo_pietra = Citta.Castello.Pietra;
                costo_ferro = Citta.Castello.Ferro;
                costo_oro = Citta.Castello.Oro;
                costo_popolazione = Citta.Castello.Popolazione;
            }
            if (Edificio == "Castello" && Stato == "Difesa")
            {
                livello_Ricerca = player.Ricerca_Castello_Difesa;
                costo_cibo = Citta.Castello.Cibo;
                costo_legno = Citta.Castello.Legno;
                costo_pietra = Citta.Castello.Pietra;
                costo_ferro = Citta.Castello.Ferro;
                costo_oro = Citta.Castello.Oro;
                costo_popolazione = Citta.Castello.Popolazione;
            }
            int valore_Target = livello_Ricerca + (livello_Ricerca * 2);
            livello_Ricerca++; //Temporaneo, solo x visualizzare il livello corretto nei messaggi inviati come log... 0_1_2 --> 1_2_3...

            if (livello_Giocatore < valore_Target)
            {
                Send(clientGuid, $"Log_Server|La ricerca [{Stato} {Edificio} {livello_Giocatore}], richiede che il giocatore sia almeno di livello: {valore_Target}\r\n");
                Console.WriteLine($"La ricerca [{Stato} {Edificio} {livello_Giocatore}], richiede che il giocatore sia almeno di livello: {valore_Target}\r\n");
                return false;
            }
            //Rimuovo le risorse se possibile... oppure fallisco ed invio
            if (player.Cibo >= costo_cibo * livello_Giocatore &&
                player.Legno >= costo_legno * livello_Giocatore &&
                player.Pietra >= costo_pietra * livello_Giocatore &&
                player.Ferro >= costo_ferro * livello_Giocatore &&
                player.Oro >= costo_oro * livello_Giocatore &&
                player.Popolazione >= costo_popolazione * livello_Giocatore)
            {
                // Sottrai le risorse necessarie
                player.Cibo -= costo_cibo * livello_Giocatore;
                player.Legno -= costo_legno * livello_Giocatore;
                player.Pietra -= costo_pietra * livello_Giocatore;
                player.Ferro -= costo_ferro * livello_Giocatore;
                player.Oro -= costo_oro * livello_Giocatore;
                player.Popolazione -= costo_popolazione * livello_Giocatore;

                Send(clientGuid, $"Log_Server|Risorse utilizzate per la ricerca di: [{Stato} {Edificio} Livello {livello_Ricerca}]\r\n " +
                    $"Cibo= {costo_cibo * livello_Giocatore}," +
                    $"Legno= {costo_legno * livello_Giocatore}," +
                    $"Pietra= {costo_pietra * livello_Giocatore}," +
                    $"Ferro= {costo_ferro * livello_Giocatore}," +
                    $"Oro= {costo_oro * livello_Giocatore}, " +
                    $"Popolazione= {costo_popolazione * livello_Giocatore}\r\n");
                Console.WriteLine($"Risorse utilizzate per la ricerca di {Stato} {Edificio} {livello_Ricerca}:\r\n " +
                    $"Cibo= {costo_cibo * livello_Giocatore}, " +
                    $"Legno= {costo_legno * livello_Giocatore}, " +
                    $"Pietra= {costo_pietra * livello_Giocatore}, " +
                    $"Ferro= {costo_ferro * livello_Giocatore}, " +
                    $"Oro= {costo_oro * livello_Giocatore}, " +
                    $"Popolazione= {costo_popolazione * livello_Giocatore}\r\n");
            }
            else
            {
                Send(clientGuid, $"Log_Server|Risorse insufficenti per la ricerca di: [{Stato} {Edificio} Livello {livello_Ricerca}]\r\n " +
                   $"Cibo= {costo_cibo * livello_Giocatore}," +
                   $"Legno= {costo_legno * livello_Giocatore}," +
                   $"Pietra= {costo_pietra * livello_Giocatore}," +
                   $"Ferro= {costo_ferro * livello_Giocatore}," +
                   $"Oro= {costo_oro * livello_Giocatore}, " +
                   $"Popolazione= {costo_popolazione * livello_Giocatore}\r\n");
                return false;
            }

            switch (Edificio)
            {
                case "Ingresso":
                    if (Stato == "Guarnigione")
                    {
                        player.Ricerca_Ingresso_Guarnigione++;
                        return true;
                    }
                    Console.WriteLine($"Ricerca {Edificio} {Stato} {valore_Target} completata!");
                    break;
                case "Citta":
                    if (Stato == "Guarnigione")
                    {
                        player.Ricerca_Ingresso_Guarnigione++;
                        return true;
                    }
                    Console.WriteLine($"Ricerca {Edificio} {Stato} {valore_Target} completata!");
                    break;
                case "Cancello":
                    if (Stato == "Guarnigione")
                    {
                        player.Ricerca_Cancello_Guarnigione++;
                        return true;
                    }
                    if (Stato == "Salute")
                    {
                        player.Ricerca_Cancello_Salute++;
                        return true;
                    }
                    if (Stato == "Difesa")
                    {
                        player.Ricerca_Castello_Difesa++;
                        return true;
                    }
                    Console.WriteLine($"Ricerca {Edificio} {Stato} {valore_Target} completata!");
                    break;
                case "Mura":
                    if (Stato == "Guarnigione")
                    {
                        player.Ricerca_Mura_Guarnigione++;
                        return true;
                    }
                    if (Stato == "Salute")
                    {
                        player.Ricerca_Mura_Salute++;
                        return true;
                    }
                    if (Stato == "Difesa")
                    {
                        player.Ricerca_Mura_Difesa++;
                        return true;
                    }
                    Console.WriteLine($"Ricerca {Edificio} {Stato} {valore_Target} completata!");
                    break;
                case "Torri":
                    if (Stato == "Guarnigione")
                    {
                        player.Ricerca_Torri_Guarnigione++;
                        return true;
                    }
                    if (Stato == "Salute")
                    {
                        player.Ricerca_Torri_Salute++;
                        return true;
                    }
                    if (Stato == "Difesa")
                    {
                        player.Ricerca_Torri_Difesa++;
                        return true;
                    }
                    Console.WriteLine($"Ricerca {Edificio} {Stato} {valore_Target} completata!");
                    break;
                case "Castello":
                    if (Stato == "Guarnigione")
                    {
                        player.Ricerca_Castello_Guarnigione++;
                        return true;
                    }
                    if (Stato == "Salute")
                    {
                        player.Ricerca_Castello_Salute++;
                        return true;
                    }
                    if (Stato == "Difesa")
                    {
                        player.Ricerca_Castello_Difesa++;
                        return true;
                    }
                    Console.WriteLine($"Ricerca {Edificio} {Stato} {valore_Target} completata!");
                    break;
                default:
                    Console.WriteLine($"Il tipo di ricerca {Edificio} {Stato} non supportata!");
                    return false;
                    break;
            }
            return false;
        }
    }
}
