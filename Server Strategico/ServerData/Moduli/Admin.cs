using static Server_Strategico.Server.Server;
using Server_Strategico.Gioco;

namespace Server_Strategico.ServerData.Moduli
{
    public class Admin
    {
        public static bool adminStart = true;
        public static void AvviaConsoleAdmin(string userInput)
        {
            Task.Run(async () =>
            {
                while (adminStart)
                {
                    string input = Console.ReadLine() ?? string.Empty;
                    if (input == "adminstart" || input == "") 
                        Console.WriteLine($"scrivi /comandi per la lista");
                    if (string.IsNullOrWhiteSpace(input) || userInput == "adminstop") continue;
                    await ProcessaComando(input);
                }
            });
        }
        public static double GetRisorsa(Giocatori.Player player, string risorsa)
        {
            switch (risorsa.ToLower())
            {
                case "oro": return player.Oro;
                case "cibo": return player.Cibo;
                case "legna": return player.Legno;
                case "pietra": return player.Pietra;
                case "ferro": return player.Ferro;
                case "diamanti_viola": return player.Diamanti_Viola;
                case "diamanti_blu": return player.Diamanti_Blu;
                default: return 0;
            }
        }
        async static Task ProcessaComando(string input)
        {
            // "/Give oro 185587 100" → ["Give", "oro", "185587", "100"]
            string[] parti = input.Trim().TrimStart('/').Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string comando = parti[0].ToLower();

            switch (comando)
            {
                case "comandi":
                    Console.WriteLine(
                        "/give <risorsa> <id> <quantità>   — aggiunge risorsa (numero negativo per togliere)\n" +
                        "/ban <id>                         — banna il giocatore\n" +
                        "/unban <id>                       — rimuove il ban\n" +
                        "/kick <id>                        — disconnette il giocatore\n" +
                        "/wipe <id>                        — resetta il giocatore"
                    );
                    break;

                case "give":
                    if (parti.Length < 4) { Console.WriteLine("Uso: /give <risorsa> <id> <quantità>"); break; }
                    await Give(parti[1], parti[2], parti[3]);
                    break;

                case "ban":
                    if (parti.Length < 2) { Console.WriteLine("Uso: /ban <id>"); break; }
                    await Ban(parti[1]);
                    break;

                case "unban":
                    if (parti.Length < 2) { Console.WriteLine("Uso: /unban <id>"); break; }
                    await Unban(parti[1]);
                    break;

                case "kick":
                    if (parti.Length < 2) { Console.WriteLine("Uso: /kick <id>"); break; }
                    await Kick(parti[1]);
                    break;

                case "wipe":
                    if (parti.Length < 2) { Console.WriteLine("Uso: /wipe <id>"); break; }
                    await Wipe(parti[1]);
                    break;

                default:
                    Console.WriteLine($"Comando sconosciuto: {comando} (scrivi /comandi per la lista)");
                    break;
            }
        }
        async static Task Give(string risorsa, string idTesto, string quantitaTesto)
        {
            if (!int.TryParse(idTesto, out int id)) { Console.WriteLine("ID non valido."); return; }
            if (!double.TryParse(quantitaTesto, out double quantita)) { Console.WriteLine("Quantità non valida."); return; }

            var player = await PlayerID(id);
            if (player == null) { Console.WriteLine("Giocatore non trovato."); return; }

            switch (risorsa.ToLower())
            {
                case "oro": player.Oro = Math.Max(0, player.Oro + quantita); break;
                case "cibo": player.Cibo = Math.Max(0, player.Cibo + quantita); break;
                case "legna": player.Legno = Math.Max(0, player.Legno + quantita); break;
                case "pietra": player.Pietra = Math.Max(0, player.Pietra + quantita); break;
                case "ferro": player.Ferro = Math.Max(0, player.Ferro + quantita); break;
                case "diamanti_viola": player.Diamanti_Viola = (int)Math.Max(0, player.Diamanti_Viola + quantita); break;
                case "diamanti_blu": player.Diamanti_Blu = (int)Math.Max(0, player.Diamanti_Blu + quantita); break;
                default:
                    Console.WriteLine($"Risorsa sconosciuta: {risorsa}");
                    return;
            }

            Console.WriteLine($"{risorsa} di {player.Username} (ID {id}): {quantita:+#;-#;0} → ora {GetRisorsa(player, risorsa)}");
        }
        async static Task Ban(string idTesto)
        {
            if (!int.TryParse(idTesto, out int id)) { Console.WriteLine("ID non valido."); return; }
            var player = await PlayerID(id);
            if (player == null) { Console.WriteLine("Giocatore non trovato."); return; }

            player.Banned_Giocatore = true;
            await DisconnettiGiocatore(player.Username);
            Console.WriteLine($"{player.Username} bannato.");
        }

        async static Task Unban(string idTesto)
        {
            if (!int.TryParse(idTesto, out int id)) { Console.WriteLine("ID non valido."); return; }
            var player = await PlayerID(id);
            if (player == null) { Console.WriteLine("Giocatore non trovato."); return; }

            player.Banned_Giocatore = false;
            Console.WriteLine($"{player.Username} sbannato.");
        }

        async static Task Kick(string idTesto)
        {
            if (!int.TryParse(idTesto, out int id)) { Console.WriteLine("ID non valido."); return; }
            var player = await PlayerID(id);
            if (player == null) { Console.WriteLine("Giocatore non trovato."); return; }

            await Server.Server.DisconnettiGiocatore(player.Username);
            Console.WriteLine($"{player.Username} disconnesso.");
        }

        async static Task Wipe(string idTesto)
        {
            if (!int.TryParse(idTesto, out int id)) { Console.WriteLine("ID non valido."); return; }
            var player = await PlayerID(id);
            if (player == null) { Console.WriteLine("Giocatore non trovato."); return; }

            player.Livello = 1;
            player.Esperienza = 0;
            player.Diamanti_Viola = 0;
            player.Diamanti_Blu = 0;
            player.Oro = 0;
            player.Cibo = 0;
            // ...gli altri campi che consideri parte del wipe

            Console.WriteLine($"{player.Username} resettato.");
        }
    }
}
