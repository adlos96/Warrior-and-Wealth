using Warrior_and_Wealth;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.Json;
using WatsonTcp;
using static Strategico_V2.Variabili_Client;

namespace Strategico_V2
{
    public class ClientConnection
    {

        public class QuestUpdatePacket
        {
            public string? Type { get; set; }
            public List<ClientQuestData>? Quests { get; set; }
        }

        public class ClientQuestData
        {
            public int Id { get; set; }
            public string? Quest_Description { get; set; }
            public int Experience { get; set; }
            public int Require { get; set; }
            public int Max_Complete { get; set; }
            public int Progress { get; set; }
            public int Completata { get; set; }
        }

        public static string argomento_Invio = "";
        public static string argomento_Ricevuto = "";
        public static bool client_Connesso = false;

        internal class TestClient
        {
            public static string _ServerIp = "warriorandwealth.duckdns.org"; // adly.xed.im 185.229.236.183
            private static int _ServerPort = 8443;
            private static bool _Ssl = false;
            private static string _CertFile = "";
            private static string _CertPass = "Password1";
            private static bool _DebugMessages = true;
            private static bool _AcceptInvalidCerts = true;
            private static bool _MutualAuth = false;
            public static WatsonTcpClient? _Client = null;
            private static string? _PresharedKey = null;


            public static Task InitializeClient()
            {
                return Task.Run(async () => //Crea un task e gli assegna un blocco istruzioni da eseguire.
                {
                    Console.WriteLine("Client partito");
                    Console.WriteLine($"Use SSL: {_Ssl}");

                    if (_Ssl)
                    {
                        bool supplyCert = true;
                        Console.WriteLine($"Supply SSL certificate: {supplyCert}");

                        if (supplyCert)
                        {
                            _CertFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + $@"/Documents/Client.pfx";
                            _CertPass = "Password1";
                        }

                        _AcceptInvalidCerts = true;
                        _MutualAuth = true;
                        Console.WriteLine($"Accept invalid certs: {_AcceptInvalidCerts}");
                        Console.WriteLine($"Mutually authenticate: {_MutualAuth}");
                    }
                    await ConnectClient();
                });
            }
            public static Task ConnectClient()
            {
                return Task.Run(() => //Crea un task e gli assegna un blocco istruzioni da eseguire.
                {
                    if (_Client != null) _Client.Dispose();
                    if (!_Ssl) _Client = new WatsonTcpClient(_ServerIp, _ServerPort);
                    else
                    {
                        _Client = new WatsonTcpClient(_ServerIp, _ServerPort, _CertFile, _CertPass);
                        _Client.Settings.AcceptInvalidCertificates = _AcceptInvalidCerts;
                        _Client.Settings.MutuallyAuthenticate = _MutualAuth;
                    }
                    _Client.Events.AuthenticationFailure += AuthenticationFailure;
                    _Client.Events.AuthenticationSucceeded += AuthenticationSucceeded;
                    _Client.Events.ServerConnected += ServerConnected;
                    _Client.Events.ServerDisconnected += ServerDisconnected;
                    _Client.Events.MessageReceived += MessageReceived;
                    _Client.Events.ExceptionEncountered += ExceptionEncountered; //???

                    _Client.Callbacks.AuthenticationRequested = AuthenticationRequested;

                    // _Client.Settings.IdleServerTimeoutMs = 5000;
                    _Client.Settings.DebugMessages = _DebugMessages;
                    _Client.Settings.Logger = Logger;
                    _Client.Settings.NoDelay = true;

                    _Client.Keepalive.EnableTcpKeepAlives = true;
                    _Client.Keepalive.TcpKeepAliveInterval = 1;
                    _Client.Keepalive.TcpKeepAliveTime = 1;
                    _Client.Keepalive.TcpKeepAliveRetryCount = 3;

                    _Client.Connect();
                    client_Connesso = true;
                    Send("Connesso");
                });
            }
            public static void Send(string messaggio)
            {
                _Client.SendAsync(messaggio);
            }
            private static void ExceptionEncountered(object sender, ExceptionEventArgs e)
            {
                Console.WriteLine("*** Exception ***");
                Console.WriteLine(e.ToString());
            }
            private static string AuthenticationRequested()
            {
                // return "0000000000000000";
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("Server requests authentication");
                Console.WriteLine("Press ENTER and THEN enter your preshared key");
                if (String.IsNullOrEmpty(_PresharedKey)) _PresharedKey = _CertPass;
                return _PresharedKey;
            }
            private static void ServerConnected(object sender, ConnectionEventArgs args)
            {
                Console.WriteLine("Server connected"); // Controlla se c'è una connessione col server
                client_Connesso = true;
            }
            private static void ServerDisconnected(object sender, DisconnectionEventArgs args)
            {
                Console.WriteLine("Server disconnected: " + args.Reason.ToString());
                client_Connesso = false;
            }
            private static void Logger(Severity sev, string msg)
            {
                Console.WriteLine("[" + sev.ToString().PadRight(9) + "] " + msg);
            }

            private static void AuthenticationSucceeded(object sender, EventArgs args)
            {
                Console.WriteLine("Authentication succeeded");
            }
            private static void AuthenticationFailure(object sender, EventArgs args)
            {
                Console.WriteLine("Authentication failed");
            }
            private static void MessageReceived(object sender, MessageReceivedEventArgs args)
            {
                Console.Write("Message from server: ");
                if (args.Data == null)
                {
                    Console.WriteLine("[null]");
                    return;
                }
                string messaggio = Encoding.UTF8.GetString(args.Data);

                Console.WriteLine("Messaggio Ricevuto");
                Console.WriteLine("Ricevuto: " + messaggio);

                Quest(messaggio);
                AggiornaVillaggiDalServer(messaggio);

                string[]? mess = null;
                if (messaggio.Contains('|'))
                {
                    mess = messaggio.Split('|');
                    switch (mess[0])
                    {
                        case "Login":
                            if (mess[1] == "true") Variabili_Client.Utente.User_Login = true;
                            else Variabili_Client.Utente.User_Login = false;
                            if (mess.Count() >= 3) Login.login_data = mess[2];
                            break;
                        case "Update_Data": Update_Data(mess); break;
                        case "Log_Server": Update_Log(mess[1]); break;
                        case "Update_PVP_Player": Update_PVP_List(mess); break;
                        case "Descrizione": Update_Desc(mess[1], mess[2]); break;
                        case "Raduno": Update_Lista_Raduni(mess); break;
                        case "Raduni_Player": Update_Lista_Raduni_Player(mess); break;
                        case "RadunoPartecipo":
                            Update_Raduni_Partecipazione(mess);
                            break;
                        case "Tutorial":
                            if (mess[1] == "Dati")
                            {
                                var dati = JsonSerializer.Deserialize<List<dati>>(mess[2]);
                                Variabili_Client.tutorial_dati = dati;
                            }
                            break;
                        case "Gamepass_Premi":
                            for (int i = 1; i < mess.Count(); i++)
                                Variabili_Client.GamePass_Premi[i-1] = mess[i];
                            break;
                        case "Gamepass_Premi_Ottenuti":
                            for (int i = 1; i < mess.Count(); i++)
                                Variabili_Client.GamePass_Premi_Completati[i - 1] = Convert.ToBoolean(mess[i]);
                            break;

                        default: Console.WriteLine($"[Errore] >> [{messaggio}] Comando non riconosciuto"); break;
                    }
                    Console.WriteLine("");
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine($"Comando:        {mess[0]}");
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("");
                }

            }
            public static void Tutorial_Data(string messaggio)
            {

            }
            public static void AggiornaVillaggiDalServer(string messaggio)
            {
                if (messaggio.StartsWith("{"))
                {
                    var pacchetto = JsonSerializer.Deserialize<PacchettoVillaggi>(messaggio);
                    if (pacchetto?.Dati == null) return;

                    if (pacchetto.Type == "VillaggiPersonali")
                    {
                        VillaggiPersonali = pacchetto.Dati
                        .Select(v => new Variabili_Client.VillaggioClient
                        {
                            Id = v.Id,
                            Nome = v.Nome,
                            Livello = v.Livello,
                            Esperienza = v.Esperienza,
                            Esplorato = v.Esplorato,
                            Sconfitto = v.Sconfitto,
                            
                            Cibo = v.Cibo,
                            Legno = v.Legno,
                            Pietra = v.Pietra,
                            Ferro = v.Ferro,
                            Oro = v.Oro,
                            Diamanti_Viola = v.Diamanti_Viola,
                            Diamanti_Blu = v.Diamanti_Blu,

                            Guerrieri = v.Guerrieri,
                            Lancieri = v.Lancieri,
                            Arcieri = v.Arcieri,
                            Catapulte = v.Catapulte
                        })
                        .ToList();

                        Console.WriteLine($"Villaggi caricati: {VillaggiPersonali.Count}");
                    }
                    else if (pacchetto.Type == "CittaGlobali")
                    {
                        Variabili_Client.CittaGlobali = pacchetto.Dati
                        .Select(v => new Variabili_Client.VillaggioClient
                        {
                            Id = v.Id,
                            Nome = v.Nome,
                            Livello = v.Livello,
                            Esperienza = v.Esperienza,
                            Esplorato = v.Esplorato,
                            Sconfitto = v.Sconfitto,

                            Cibo = v.Cibo,
                            Legno = v.Legno,
                            Pietra = v.Pietra,
                            Ferro = v.Ferro,
                            Oro = v.Oro,
                            Diamanti_Viola = v.Diamanti_Viola,
                            Diamanti_Blu = v.Diamanti_Blu,

                            Guerrieri = v.Guerrieri,
                            Lancieri = v.Lancieri,
                            Arcieri = v.Arcieri,
                            Catapulte = v.Catapulte
                        })
                        .ToList();
                    }
                }
            }

            static void Quest(string messaggio)
                            {
                // Se il messaggio è JSON di quest
                if (messaggio.StartsWith("{") && messaggio.Contains("\"Quests\""))
                    try
                    {
                        var packet = JsonSerializer.Deserialize<QuestUpdatePacket>(messaggio);
                        if (packet != null)
                            MontlyQuest.CurrentQuests = packet.Quests;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Errore deserializzazione JSON: " + ex.Message);
                        return;
                    }
                
                if (messaggio.StartsWith("{") && messaggio.Contains("\"QuestRewards\""))
                    try
                    {
                        using JsonDocument doc = JsonDocument.Parse(messaggio);
                        var root = doc.RootElement;

                        // Controlla che il tipo sia corretto
                        if (root.TryGetProperty("Type", out var typeProp) && typeProp.GetString() == "QuestRewards")
                        {
                            // 🔹 Converte sia numeri che stringhe in int in modo sicuro
                            static int ToInt(JsonElement e)
                            {
                                return e.ValueKind switch
                                {
                                    JsonValueKind.Number => e.GetInt32(),
                                    JsonValueKind.String => int.TryParse(e.GetString(), out int val) ? val : 0,
                                    _ => 0
                                };
                            }

                            var rewardsNormali = root.GetProperty("Rewards_Normali").EnumerateArray()
                                                     .Select(ToInt).ToList();

                            var rewardsVip = root.GetProperty("Rewards_VIP").EnumerateArray()
                                                 .Select(ToInt).ToList();

                            var points = root.GetProperty("Points").EnumerateArray()
                                             .Select(ToInt).ToList();

                            var completo = root.GetProperty("Completo").EnumerateArray()
                                             .Select(e => e.GetBoolean())
                                             .ToList();

                            var completo_Vip = root.GetProperty("Completo_Vip").EnumerateArray()
                                             .Select(e => e.GetBoolean())
                                             .ToList();

                            // Aggiorna la memoria locale
                            MontlyQuest.CurrentRewardsNormali = rewardsNormali;
                            MontlyQuest.CurrentRewardsVip = rewardsVip;
                            MontlyQuest.CurrentRewardPoints = points;
                            MontlyQuest.CurrentRewardClaimNormal = completo;
                            MontlyQuest.CurrentRewardClaimVip = completo_Vip;

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Errore deserializzazione QuestRewards: {ex.Message}");
                    }
            }
            static void Update_Data(string[] mess)
            {
                var dict = new Dictionary<string, string>();
                for (int i = 1; i < mess.Length; i++)
                {
                    var parts = mess[i].Split('=');
                    if (parts.Length == 2)
                    {
                        dict[parts[0]] = parts[1];
                    }
                }

                // Helper per assegnare valori in sicurezza
                void SetValue<T>(string key, Action<T> setter)
                {
                    if (!dict.TryGetValue(key, out var valueStr) || valueStr == null)
                        return;

                    var targetType = typeof(T);

                    // INT
                    if (targetType == typeof(int))
                    {
                        if (int.TryParse(valueStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                            setter((T)(object)intValue);
                        return;
                    }

                    // STRING
                    if (targetType == typeof(string))
                    {
                        setter((T)(object)valueStr);
                        return;
                    }

                    // BOOL (molti formati accettati)
                    if (targetType == typeof(bool))
                    {
                        var v = valueStr.Trim();
                        if (bool.TryParse(v, out var boolValue))
                        {
                            setter((T)(object)boolValue);
                            return;
                        }

                        var lower = v.ToLowerInvariant();
                        if (lower == "1" || lower == "true" || lower == "t" || lower == "yes" || lower == "y" || lower == "si" || lower == "s")
                        {
                            setter((T)(object)true);
                            return;
                        }
                        if (lower == "0" || lower == "false" || lower == "f" || lower == "no" || lower == "n")
                        {
                            setter((T)(object)false);
                            return;
                        }

                        // fallback numerico (es. "2" -> true)
                        if (int.TryParse(lower, out var iv))
                        {
                            setter((T)(object)(iv != 0));
                            return;
                        }

                        return; // non parsabile -> non assegnare
                    }

                    // DOUBLE
                    if (targetType == typeof(double))
                    {
                        if (double.TryParse(valueStr, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var d))
                            setter((T)(object)d);
                        return;
                    }

                    // ENUM
                    if (targetType.IsEnum)
                    {
                        try
                        {
                            var enumVal = (T)Enum.Parse(targetType, valueStr, ignoreCase: true);
                            setter(enumVal);
                        }
                        catch
                        {
                            // ignore parse failures
                        }
                        return;
                    }

                    // fallback generico
                    try
                    {
                        var converted = (T)Convert.ChangeType(valueStr, targetType, CultureInfo.InvariantCulture);
                        setter(converted);
                    }
                    catch
                    {
                        // non convertibile -> ignoriamo
                    }
                }
                //Server
                SetValue<string>("versione_Client_Necessario", v => Variabili_Client.versione_Client_Necessario = v);

                // Utente
                SetValue<string>("livello", v => Variabili_Client.Utente.Livello = v);
                SetValue<string>("esperienza", v => Variabili_Client.Utente.Esperienza = v);
                SetValue<string>("punti_quest", v => Variabili_Client.Utente.Montly_Quest_Point = v);
                SetValue<string>("costo_terreni_Virtuali", v => Variabili_Client.Utente.Costo_terreni_Virtuali = v);

                SetValue<bool>("vip", v => Variabili_Client.Utente.User_Vip = v);
                SetValue<string>("vip_Tempo", v => Variabili_Client.Utente.User_Vip_Tempo = v);
                SetValue<bool>("GamePass_Base", v => Variabili_Client.Utente.User_GamePass_Base = v);
                SetValue<string>("GamePass_Base_Tempo", v => Variabili_Client.Utente.User_GamePass_Base_Tempo = v);
                SetValue<bool>("GamePass_Avanzato", v => Variabili_Client.Utente.User_GamePass_Avanzato = v);
                SetValue<string>("GamePass_Avanzato_Tempo", v => Variabili_Client.Utente.User_GamePass_Avanzato_Tempo = v);
                SetValue<string>("Scudo_Tempo", v => Variabili_Client.Utente.Scudo_Pace_Tempo = v);
                SetValue<string>("Costruttori_Tempo", v => Variabili_Client.Utente.Costruttori_Tempo = v);
                SetValue<string>("Reclutatori_Tempo", v => Variabili_Client.Utente.Reclutatori_Tempo = v);
                SetValue<string>("QuestMensili_Tempo", v => Variabili_Client.Utente.Montly_Quest_Tempo = v);
                SetValue<string>("Barbari_Tempo", v => Variabili_Client.Utente.Barbari_Tempo = v);

                SetValue<int>("Giorni_Consecutivi", v => Variabili_Client.Giorni_Accessi_Consecutivi = v);

                //Sblocco truppe
                SetValue<string>("Unlock_Truppe_II", v => Variabili_Client.truppe_II = v);
                SetValue<string>("Unlock_Truppe_III", v => Variabili_Client.truppe_III = v);
                SetValue<string>("Unlock_Truppe_IV", v => Variabili_Client.truppe_IV = v);
                SetValue<string>("Unlock_Truppe_V", v => Variabili_Client.truppe_V = v);
                SetValue<string>("Unlock_Città_Barbare", v => Variabili_Client.unlock_Città_Barbare = v);
                SetValue<string>("Unlock_PVP", v => Variabili_Client.unlock_PVP = v);

                //Shop
                SetValue<string>("Pacchetto_Vip_1_Reward", v => Variabili_Client.Shop.Vip_1.Reward = v);
                SetValue<string>("Pacchetto_Vip_1_Costo", v => Variabili_Client.Shop.Vip_1.Costo = v);
                SetValue<string>("Pacchetto_Vip_2_Reward", v => Variabili_Client.Shop.Vip_2.Reward = v);
                SetValue<string>("Pacchetto_Vip_2_Costo", v => Variabili_Client.Shop.Vip_2.Costo = v);

                SetValue<string>("Pacchetto_Diamanti_1_Reward", v => Variabili_Client.Shop.Pacchetto_Diamanti_1.Reward = v);
                SetValue<string>("Pacchetto_Diamanti_1_Costo", v => Variabili_Client.Shop.Pacchetto_Diamanti_1.Costo = v);
                SetValue<string>("Pacchetto_Diamanti_2_Reward", v => Variabili_Client.Shop.Pacchetto_Diamanti_2.Reward = v);
                SetValue<string>("Pacchetto_Diamanti_2_Costo", v => Variabili_Client.Shop.Pacchetto_Diamanti_2.Costo = v);
                SetValue<string>("Pacchetto_Diamanti_3_Reward", v => Variabili_Client.Shop.Pacchetto_Diamanti_3.Reward = v);
                SetValue<string>("Pacchetto_Diamanti_3_Costo", v => Variabili_Client.Shop.Pacchetto_Diamanti_3.Costo = v);
                SetValue<string>("Pacchetto_Diamanti_4_Reward", v => Variabili_Client.Shop.Pacchetto_Diamanti_4.Reward = v);
                SetValue<string>("Pacchetto_Diamanti_4_Costo", v => Variabili_Client.Shop.Pacchetto_Diamanti_4.Costo = v);

                SetValue<string>("Pacchetto_Scudo_Pace_8h_Reward", v => Variabili_Client.Shop.Scudo_Pace_8h.Reward = v);
                SetValue<string>("Pacchetto_Scudo_Pace_8h_Costo", v => Variabili_Client.Shop.Scudo_Pace_8h.Costo = v);
                SetValue<string>("Pacchetto_Scudo_Pace_24h_Reward", v => Variabili_Client.Shop.Scudo_Pace_24h.Reward = v);
                SetValue<string>("Pacchetto_Scudo_Pace_24h_Costo", v => Variabili_Client.Shop.Scudo_Pace_24h.Costo = v);
                SetValue<string>("Pacchetto_Scudo_Pace_72h_Reward", v => Variabili_Client.Shop.Scudo_Pace_72h.Reward = v);
                SetValue<string>("Pacchetto_Scudo_Pace_72h_Costo", v => Variabili_Client.Shop.Scudo_Pace_72h.Costo = v);

                SetValue<string>("Pacchetto_Costruttore_24h_Reward", v => Variabili_Client.Shop.Costruttore_24h.Reward = v);
                SetValue<string>("Pacchetto_Costruttore_24h_Costo", v => Variabili_Client.Shop.Costruttore_24h.Costo = v);
                SetValue<string>("Pacchetto_Costruttore_48h_Reward", v => Variabili_Client.Shop.Costruttore_48h.Reward = v);
                SetValue<string>("Pacchetto_Costruttore_48h_Costo", v => Variabili_Client.Shop.Costruttore_48h.Costo = v);

                SetValue<string>("Pacchetto_Reclutatore_24h_Reward", v => Variabili_Client.Shop.Reclutatore_24h.Reward = v);
                SetValue<string>("Pacchetto_Reclutatore_24h_Costo", v => Variabili_Client.Shop.Reclutatore_24h.Costo = v);
                SetValue<string>("Pacchetto_Reclutatore_48h_Reward", v => Variabili_Client.Shop.Reclutatore_48h.Reward = v);
                SetValue<string>("Pacchetto_Reclutatore_48h_Costo", v => Variabili_Client.Shop.Reclutatore_48h.Costo = v);

                SetValue<string>("Pacchetto_GamePass_Base_Reward", v => Variabili_Client.Shop.GamePass_Base.Reward = v);
                SetValue<string>("Pacchetto_GamePass_Base_Costo", v => Variabili_Client.Shop.GamePass_Base.Costo = v);
                SetValue<string>("Pacchetto_GamePass_Avanzato_Reward", v => Variabili_Client.Shop.GamePass_Avanzato.Reward = v);
                SetValue<string>("Pacchetto_GamePass_Avanzato_Costo", v => Variabili_Client.Shop.GamePass_Avanzato.Costo = v);

                // Risorse
                SetValue<string>("cibo", v => Variabili_Client.Utente_Risorse.Cibo = v);
                SetValue<string>("legna", v => Variabili_Client.Utente_Risorse.Legna = v);
                SetValue<string>("pietra", v => Variabili_Client.Utente_Risorse.Pietra = v);
                SetValue<string>("ferro", v => Variabili_Client.Utente_Risorse.Ferro = v);
                SetValue<string>("oro", v => Variabili_Client.Utente_Risorse.Oro = v);
                SetValue<string>("popolazione", v => Variabili_Client.Utente_Risorse.Popolazione = v);

                SetValue<string>("spade", v => Variabili_Client.Utente_Risorse.Spade = v);
                SetValue<string>("lance", v => Variabili_Client.Utente_Risorse.Lance = v);
                SetValue<string>("archi", v => Variabili_Client.Utente_Risorse.Archi = v);
                SetValue<string>("scudi", v => Variabili_Client.Utente_Risorse.Scudi = v);
                SetValue<string>("armature", v => Variabili_Client.Utente_Risorse.Armature = v);
                SetValue<string>("frecce", v => Variabili_Client.Utente_Risorse.Frecce = v);

                SetValue<string>("dollari_virtuali", v => Variabili_Client.Utente_Risorse.Virtual_Dolla = v);
                SetValue<string>("diamanti_blu", v => Variabili_Client.Utente_Risorse.Diamond_Blu = v);
                SetValue<string>("diamanti_viola", v => Variabili_Client.Utente_Risorse.Diamond_Viola = v);

                //Produzione Risorse
                SetValue<string>("cibo_s", v => Variabili_Client.Utente_Risorse.Cibo_s = v);
                SetValue<string>("legna_s", v => Variabili_Client.Utente_Risorse.Legna_s = v);
                SetValue<string>("pietra_s", v => Variabili_Client.Utente_Risorse.Pietra_s = v);
                SetValue<string>("ferro_s", v => Variabili_Client.Utente_Risorse.Ferro_s = v);
                SetValue<string>("oro_s", v => Variabili_Client.Utente_Risorse.Oro_s = v);
                SetValue<string>("popolazione_s", v => Variabili_Client.Utente_Risorse.Popolazione_s = v);

                SetValue<string>("spade_s", v => Variabili_Client.Utente_Risorse.Spade_s = v);
                SetValue<string>("lance_s", v => Variabili_Client.Utente_Risorse.Lance_s = v);
                SetValue<string>("archi_s", v => Variabili_Client.Utente_Risorse.Archi_s = v);
                SetValue<string>("scudi_s", v => Variabili_Client.Utente_Risorse.Scudi_s = v);
                SetValue<string>("armature_s", v => Variabili_Client.Utente_Risorse.Armature_s = v);
                SetValue<string>("frecce_s", v => Variabili_Client.Utente_Risorse.Frecce_s = v);

                SetValue<string>("consumo_cibo_s", v => Variabili_Client.Utente_Risorse.Mantenimento_Cibo = v);
                SetValue<string>("consumo_oro_s", v => Variabili_Client.Utente_Risorse.Mantenimento_Oro = v);

                SetValue<string>("consumo_cibo_strutture", v => Variabili_Client.Utente_Risorse.Strutture_Cibo = v);
                SetValue<string>("consumo_legno_strutture", v => Variabili_Client.Utente_Risorse.Strutture_Legna = v);
                SetValue<string>("consumo_pietra_strutture", v => Variabili_Client.Utente_Risorse.Strutture_Pietra = v);
                SetValue<string>("consumo_ferro_strutture", v => Variabili_Client.Utente_Risorse.Strutture_Ferro = v);
                SetValue<string>("consumo_oro_strutture", v => Variabili_Client.Utente_Risorse.Strutture_Oro = v);

                // Limite Risorse
                SetValue<string>("cibo_limite", v => Variabili_Client.Utente_Risorse.Cibo_Limite = v);
                SetValue<string>("legna_limite", v => Variabili_Client.Utente_Risorse.Legna_Limite = v);
                SetValue<string>("pietra_limite", v => Variabili_Client.Utente_Risorse.Pietra_Limite = v);
                SetValue<string>("ferro_limite", v => Variabili_Client.Utente_Risorse.Ferro_Limite = v);
                SetValue<string>("oro_limite", v => Variabili_Client.Utente_Risorse.Oro_Limite = v);
                SetValue<string>("popolazione_limite", v => Variabili_Client.Utente_Risorse.Popolazione_Limite = v);

                SetValue<string>("spade_limite", v => Variabili_Client.Utente_Risorse.Spade_Limite = v);
                SetValue<string>("lance_limite", v => Variabili_Client.Utente_Risorse.Lance_Limite = v);
                SetValue<string>("archi_limite", v => Variabili_Client.Utente_Risorse.Archi_Limite = v);
                SetValue<string>("scudi_limite", v => Variabili_Client.Utente_Risorse.Scudi_Limite = v);
                SetValue<string>("armature_limite", v => Variabili_Client.Utente_Risorse.Armature_Limite = v);
                SetValue<string>("frecce_limite", v => Variabili_Client.Utente_Risorse.Frecce_Limite = v);

                // Costruzioni
                SetValue<string>("fattorie", v => Variabili_Client.Costruzione.Fattorie.Quantità = v);
                SetValue<string>("segherie", v => Variabili_Client.Costruzione.Segherie.Quantità = v);
                SetValue<string>("cave_pietra", v => Variabili_Client.Costruzione.CaveDiPietra.Quantità = v);
                SetValue<string>("miniere_ferro", v => Variabili_Client.Costruzione.Miniera_Ferro.Quantità = v);
                SetValue<string>("miniere_oro", v => Variabili_Client.Costruzione.Miniera_Oro.Quantità = v);
                SetValue<string>("case", v => Variabili_Client.Costruzione.Case.Quantità = v);

                SetValue<string>("fattoria_coda", v => Variabili_Client.Costruzione_Coda.Fattorie.Quantità = v);
                SetValue<string>("segheria_coda", v => Variabili_Client.Costruzione_Coda.Segherie.Quantità = v);
                SetValue<string>("cavapietra_coda", v => Variabili_Client.Costruzione_Coda.CaveDiPietra.Quantità = v);
                SetValue<string>("minieraferro_coda", v => Variabili_Client.Costruzione_Coda.Miniera_Ferro.Quantità = v);
                SetValue<string>("minieraoro_coda", v => Variabili_Client.Costruzione_Coda.Miniera_Oro.Quantità = v);
                SetValue<string>("casa_coda", v => Variabili_Client.Costruzione_Coda.Case.Quantità = v);

                SetValue<string>("comune", v => Variabili_Client.Terreni_Virtuali.Comune.Quantità = v);
                SetValue<string>("noncomune", v => Variabili_Client.Terreni_Virtuali.NonComune.Quantità = v);
                SetValue<string>("raro", v => Variabili_Client.Terreni_Virtuali.Raro.Quantità = v);
                SetValue<string>("epico", v => Variabili_Client.Terreni_Virtuali.Epico.Quantità = v);
                SetValue<string>("leggendario", v => Variabili_Client.Terreni_Virtuali.Leggendario.Quantità = v);

                // Workshop
                SetValue<string>("workshop_spade", v => Variabili_Client.Costruzione.Workshop_Spade.Quantità = v);
                SetValue<string>("workshop_lance", v => Variabili_Client.Costruzione.Workshop_Lance.Quantità = v);
                SetValue<string>("workshop_archi", v => Variabili_Client.Costruzione.Workshop_Archi.Quantità = v);
                SetValue<string>("workshop_scudi", v => Variabili_Client.Costruzione.Workshop_Scudi.Quantità = v);
                SetValue<string>("workshop_armature", v => Variabili_Client.Costruzione.Workshop_Armature.Quantità = v);
                SetValue<string>("workshop_frecce", v => Variabili_Client.Costruzione.Workshop_Frecce.Quantità = v);

                SetValue<string>("workshop_spade_coda", v => Variabili_Client.Costruzione_Coda.Workshop_Spade.Quantità = v);
                SetValue<string>("workshop_lance_coda", v => Variabili_Client.Costruzione_Coda.Workshop_Lance.Quantità = v);
                SetValue<string>("workshop_archi_coda", v => Variabili_Client.Costruzione_Coda.Workshop_Archi.Quantità = v);
                SetValue<string>("workshop_scudi_coda", v => Variabili_Client.Costruzione_Coda.Workshop_Scudi.Quantità = v);
                SetValue<string>("workshop_armature_coda", v => Variabili_Client.Costruzione_Coda.Workshop_Armature.Quantità = v);
                SetValue<string>("workshop_frecce_coda", v => Variabili_Client.Costruzione_Coda.Workshop_Frecce.Quantità = v);

                // Caserme
                SetValue<string>("caserma_guerrieri", v => Variabili_Client.Costruzione.Caserme_Guerrieri.Quantità = v);
                SetValue<string>("caserma_lanceri", v => Variabili_Client.Costruzione.Caserme_Lanceri.Quantità = v);
                SetValue<string>("caserma_arceri", v => Variabili_Client.Costruzione.Caserme_arceri.Quantità = v);
                SetValue<string>("caserma_catapulte", v => Variabili_Client.Costruzione.Caserme_Catapulte.Quantità = v);

                SetValue<string>("caserma_guerrieri_coda", v => Variabili_Client.Costruzione_Coda.Caserme_Guerrieri.Quantità = v);
                SetValue<string>("caserma_lanceri_coda", v => Variabili_Client.Costruzione_Coda.Caserme_Lanceri.Quantità = v);
                SetValue<string>("caserma_arceri_coda", v => Variabili_Client.Costruzione_Coda.Caserme_arceri.Quantità = v);
                SetValue<string>("caserma_catapulte_coda", v => Variabili_Client.Costruzione_Coda.Caserme_Catapulte.Quantità = v);

                // Addestramento
                SetValue<string>("guerrieri_1", v => Variabili_Client.Reclutamento.Guerrieri_1.Quantità = v);
                SetValue<string>("guerrieri_2", v => Variabili_Client.Reclutamento.Guerrieri_2.Quantità = v);
                SetValue<string>("guerrieri_3", v => Variabili_Client.Reclutamento.Guerrieri_3.Quantità = v);
                SetValue<string>("guerrieri_4", v => Variabili_Client.Reclutamento.Guerrieri_4.Quantità = v);
                SetValue<string>("guerrieri_5", v => Variabili_Client.Reclutamento.Guerrieri_5.Quantità = v);

                SetValue<string>("lanceri_1", v => Variabili_Client.Reclutamento.Lanceri_1.Quantità = v);
                SetValue<string>("lanceri_2", v => Variabili_Client.Reclutamento.Lanceri_2.Quantità = v);
                SetValue<string>("lanceri_3", v => Variabili_Client.Reclutamento.Lanceri_3.Quantità = v);
                SetValue<string>("lanceri_4", v => Variabili_Client.Reclutamento.Lanceri_4.Quantità = v);
                SetValue<string>("lanceri_5", v => Variabili_Client.Reclutamento.Lanceri_5.Quantità = v);

                SetValue<string>("arceri_1", v => Variabili_Client.Reclutamento.Arceri_1.Quantità = v);
                SetValue<string>("arceri_2", v => Variabili_Client.Reclutamento.Arceri_2.Quantità = v);
                SetValue<string>("arceri_3", v => Variabili_Client.Reclutamento.Arceri_3.Quantità = v);
                SetValue<string>("arceri_4", v => Variabili_Client.Reclutamento.Arceri_4.Quantità = v);
                SetValue<string>("arceri_5", v => Variabili_Client.Reclutamento.Arceri_5.Quantità = v);

                SetValue<string>("catapulte_1", v => Variabili_Client.Reclutamento.Catapulte_1.Quantità = v);
                SetValue<string>("catapulte_2", v => Variabili_Client.Reclutamento.Catapulte_2.Quantità = v);
                SetValue<string>("catapulte_3", v => Variabili_Client.Reclutamento.Catapulte_3.Quantità = v);
                SetValue<string>("catapulte_4", v => Variabili_Client.Reclutamento.Catapulte_4.Quantità = v);
                SetValue<string>("catapulte_5", v => Variabili_Client.Reclutamento.Catapulte_5.Quantità = v);

                // Addestramento Coda
                SetValue<string>("guerrieri_1_coda", v => Variabili_Client.Reclutamento_Coda.Guerrieri_1.Quantità = v);
                SetValue<string>("guerrieri_2_coda", v => Variabili_Client.Reclutamento_Coda.Guerrieri_2.Quantità = v);
                SetValue<string>("guerrieri_3_coda", v => Variabili_Client.Reclutamento_Coda.Guerrieri_3.Quantità = v);
                SetValue<string>("guerrieri_4_coda", v => Variabili_Client.Reclutamento_Coda.Guerrieri_4.Quantità = v);
                SetValue<string>("guerrieri_5_coda", v => Variabili_Client.Reclutamento_Coda.Guerrieri_5.Quantità = v);

                SetValue<string>("lanceri_1_coda", v => Variabili_Client.Reclutamento_Coda.Lanceri_1.Quantità = v);
                SetValue<string>("lanceri_2_coda", v => Variabili_Client.Reclutamento_Coda.Lanceri_2.Quantità = v);
                SetValue<string>("lanceri_3_coda", v => Variabili_Client.Reclutamento_Coda.Lanceri_3.Quantità = v);
                SetValue<string>("lanceri_4_coda", v => Variabili_Client.Reclutamento_Coda.Lanceri_4.Quantità = v);
                SetValue<string>("lanceri_5_coda", v => Variabili_Client.Reclutamento_Coda.Lanceri_5.Quantità = v);

                SetValue<string>("arceri_1_coda", v => Variabili_Client.Reclutamento_Coda.Arceri_1.Quantità = v);
                SetValue<string>("arceri_2_coda", v => Variabili_Client.Reclutamento_Coda.Arceri_2.Quantità = v);
                SetValue<string>("arceri_3_coda", v => Variabili_Client.Reclutamento_Coda.Arceri_3.Quantità = v);
                SetValue<string>("arceri_4_coda", v => Variabili_Client.Reclutamento_Coda.Arceri_4.Quantità = v);
                SetValue<string>("arceri_5_coda", v => Variabili_Client.Reclutamento_Coda.Arceri_5.Quantità = v);

                SetValue<string>("catapulte_1_coda", v => Variabili_Client.Reclutamento_Coda.Catapulte_1.Quantità = v);
                SetValue<string>("catapulte_2_coda", v => Variabili_Client.Reclutamento_Coda.Catapulte_2.Quantità = v);
                SetValue<string>("catapulte_3_coda", v => Variabili_Client.Reclutamento_Coda.Catapulte_3.Quantità = v);
                SetValue<string>("catapulte_4_coda", v => Variabili_Client.Reclutamento_Coda.Catapulte_4.Quantità = v);
                SetValue<string>("catapulte_5_coda", v => Variabili_Client.Reclutamento_Coda.Catapulte_5.Quantità = v);

                SetValue<string>("guerrieri_max", v => Variabili_Client.Reclutamento.Guerrieri_Max.Quantità = v);
                SetValue<string>("lanceri_max", v => Variabili_Client.Reclutamento.Lanceri_Max.Quantità = v);
                SetValue<string>("arceri_max", v => Variabili_Client.Reclutamento.Arceri_Max.Quantità = v);
                SetValue<string>("catapulte_max", v => Variabili_Client.Reclutamento.Catapulte_Max.Quantità = v);

                // Ricerca
                SetValue<string>("ricerca_produzione", v => Variabili_Client.Utente_Ricerca.Ricerca_Produzione = v);
                SetValue<string>("ricerca_costruzione", v => Variabili_Client.Utente_Ricerca.Ricerca_Costruzione = v);
                SetValue<string>("ricerca_addestramento", v => Variabili_Client.Utente_Ricerca.Ricerca_Addestramento = v);
                SetValue<string>("ricerca_popolazione", v => Variabili_Client.Utente_Ricerca.Ricerca_Popolazione = v);
                SetValue<string>("ricerca_riparazione", v => Variabili_Client.Utente_Ricerca.Ricerca_Riparazione = v);
                SetValue<string>("ricerca_trasporto", v => Variabili_Client.Utente_Ricerca.Ricerca_Trasporto = v);

                SetValue<string>("guerriero_salute", v => Variabili_Client.Utente_Ricerca.Salute_Spadaccini = v);
                SetValue<string>("guerriero_difesa", v => Variabili_Client.Utente_Ricerca.Difesa_Spadaccini = v);
                SetValue<string>("guerriero_attacco", v => Variabili_Client.Utente_Ricerca.Attacco_Spadaccini = v);
                SetValue<string>("guerriero_livello", v => Variabili_Client.Utente_Ricerca.Livello_Spadaccini = v);

                SetValue<string>("lancere_salute", v => Variabili_Client.Utente_Ricerca.Salute_Lanceri = v);
                SetValue<string>("lancere_difesa", v => Variabili_Client.Utente_Ricerca.Difesa_Lanceri = v);
                SetValue<string>("lancere_attacco", v => Variabili_Client.Utente_Ricerca.Attacco_Lanceri = v);
                SetValue<string>("lancere_livello", v => Variabili_Client.Utente_Ricerca.Livello_Lanceri = v);

                SetValue<string>("arcere_salute", v => Variabili_Client.Utente_Ricerca.Salute_Arceri = v);
                SetValue<string>("arcere_difesa", v => Variabili_Client.Utente_Ricerca.Difesa_Arceri = v);
                SetValue<string>("arcere_attacco", v => Variabili_Client.Utente_Ricerca.Attacco_Arceri = v);
                SetValue<string>("arcere_livello", v => Variabili_Client.Utente_Ricerca.Livello_Arceri = v);

                SetValue<string>("catapulta_salute", v => Variabili_Client.Utente_Ricerca.Salute_Catapulte = v);
                SetValue<string>("catapulta_difesa", v => Variabili_Client.Utente_Ricerca.Difesa_Catapulte = v);
                SetValue<string>("catapulta_attacco", v => Variabili_Client.Utente_Ricerca.Attacco_Catapulte = v);
                SetValue<string>("catapulta_livello", v => Variabili_Client.Utente_Ricerca.Livello_Catapulte = v);

                //Ricerca citta
                SetValue<string>("ricerca_ingresso_guarnigione", v => Variabili_Client.Utente_Ricerca.Ricerca_Ingresso_Guarnigione = v);
                SetValue<string>("ricerca_citta_guarnigione", v => Variabili_Client.Utente_Ricerca.Ricerca_Citta_Guarnigione = v);

                SetValue<string>("ricerca_cancello_livello", v => Variabili_Client.Utente_Ricerca.Ricerca_Cancello_Livello = v);
                SetValue<string>("ricerca_cancello_salute", v => Variabili_Client.Utente_Ricerca.Ricerca_Cancello_Salute = v);
                SetValue<string>("ricerca_cancello_difesa", v => Variabili_Client.Utente_Ricerca.Ricerca_Cancello_Difesa = v);
                SetValue<string>("ricerca_cancello_guarnigione", v => Variabili_Client.Utente_Ricerca.Ricerca_Cancello_Guarnigione = v);

                SetValue<string>("ricerca_mura_livello", v => Variabili_Client.Utente_Ricerca.Ricerca_Mura_Livello = v);
                SetValue<string>("ricerca_mura_salute", v => Variabili_Client.Utente_Ricerca.Ricerca_Mura_Salute = v);
                SetValue<string>("ricerca_mura_difesa", v => Variabili_Client.Utente_Ricerca.Ricerca_Mura_Difesa = v);
                SetValue<string>("ricerca_mura_guarnigione", v => Variabili_Client.Utente_Ricerca.Ricerca_Mura_Guarnigione = v);

                SetValue<string>("ricerca_torri_livello", v => Variabili_Client.Utente_Ricerca.Ricerca_Torri_Livello = v);
                SetValue<string>("ricerca_torri_salute", v => Variabili_Client.Utente_Ricerca.Ricerca_Torri_Salute = v);
                SetValue<string>("ricerca_torri_difesa", v => Variabili_Client.Utente_Ricerca.Ricerca_Torri_Difesa = v);
                SetValue<string>("ricerca_torri_guarnigione", v => Variabili_Client.Utente_Ricerca.Ricerca_Torri_Guarnigione = v);

                SetValue<string>("ricerca_castello_livello", v => Variabili_Client.Utente_Ricerca.Ricerca_Castello_Livello = v);
                SetValue<string>("ricerca_castello_salute", v => Variabili_Client.Utente_Ricerca.Ricerca_Castello_Salute = v);
                SetValue<string>("ricerca_castello_difesa", v => Variabili_Client.Utente_Ricerca.Ricerca_Castello_Difesa = v);
                SetValue<string>("ricerca_castello_guarnigione", v => Variabili_Client.Utente_Ricerca.Ricerca_Castello_Guarnigione = v);

                // Guarnigione Ingresso
                SetValue<int>("Guarnigione_Ingresso", v => Variabili_Client.Citta.Ingresso.Guarnigione = v);
                SetValue<int>("Guarnigione_IngressoMax", v => Variabili_Client.Citta.Ingresso.Guarnigione_Max = v);

                SetValue<int>("Guerrieri_1_Ingresso", v => Variabili_Client.Citta.Ingresso.Guerrieri_1 = v);
                SetValue<int>("Guerrieri_2_Ingresso", v => Variabili_Client.Citta.Ingresso.Guerrieri_2 = v);
                SetValue<int>("Guerrieri_3_Ingresso", v => Variabili_Client.Citta.Ingresso.Guerrieri_3 = v);
                SetValue<int>("Guerrieri_4_Ingresso", v => Variabili_Client.Citta.Ingresso.Guerrieri_4 = v);
                SetValue<int>("Guerrieri_5_Ingresso", v => Variabili_Client.Citta.Ingresso.Guerrieri_5 = v);

                SetValue<int>("Lanceri_1_Ingresso", v => Variabili_Client.Citta.Ingresso.Lanceri_1 = v);
                SetValue<int>("Lanceri_2_Ingresso", v => Variabili_Client.Citta.Ingresso.Lanceri_2 = v);
                SetValue<int>("Lanceri_3_Ingresso", v => Variabili_Client.Citta.Ingresso.Lanceri_3 = v);
                SetValue<int>("Lanceri_4_Ingresso", v => Variabili_Client.Citta.Ingresso.Lanceri_4 = v);
                SetValue<int>("Lanceri_5_Ingresso", v => Variabili_Client.Citta.Ingresso.Lanceri_5 = v);

                SetValue<int>("Arceri_1_Ingresso", v => Variabili_Client.Citta.Ingresso.Arceri_1 = v);
                SetValue<int>("Arceri_2_Ingresso", v => Variabili_Client.Citta.Ingresso.Arceri_2 = v);
                SetValue<int>("Arceri_3_Ingresso", v => Variabili_Client.Citta.Ingresso.Arceri_3 = v);
                SetValue<int>("Arceri_4_Ingresso", v => Variabili_Client.Citta.Ingresso.Arceri_4 = v);
                SetValue<int>("Arceri_5_Ingresso", v => Variabili_Client.Citta.Ingresso.Arceri_5 = v);

                SetValue<int>("Catapulte_1_Ingresso", v => Variabili_Client.Citta.Ingresso.Catapulte_1 = v);
                SetValue<int>("Catapulte_2_Ingresso", v => Variabili_Client.Citta.Ingresso.Catapulte_2 = v);
                SetValue<int>("Catapulte_3_Ingresso", v => Variabili_Client.Citta.Ingresso.Catapulte_3 = v);
                SetValue<int>("Catapulte_4_Ingresso", v => Variabili_Client.Citta.Ingresso.Catapulte_4 = v);
                SetValue<int>("Catapulte_5_Ingresso", v => Variabili_Client.Citta.Ingresso.Catapulte_5 = v);


                // Guarnigione Città
                SetValue<int>("Guarnigione_Citta", v => Variabili_Client.Citta.Città.Guarnigione = v);
                SetValue<int>("Guarnigione_CittaMax", v => Variabili_Client.Citta.Città.Guarnigione_Max = v);

                SetValue<int>("Guerrieri_1_Citta", v => Variabili_Client.Citta.Città.Guerrieri_1 = v);
                SetValue<int>("Guerrieri_2_Citta", v => Variabili_Client.Citta.Città.Guerrieri_2 = v);
                SetValue<int>("Guerrieri_3_Citta", v => Variabili_Client.Citta.Città.Guerrieri_3 = v);
                SetValue<int>("Guerrieri_4_Citta", v => Variabili_Client.Citta.Città.Guerrieri_4 = v);
                SetValue<int>("Guerrieri_5_Citta", v => Variabili_Client.Citta.Città.Guerrieri_5 = v);

                SetValue<int>("Lanceri_1_Citta", v => Variabili_Client.Citta.Città.Lanceri_1 = v);
                SetValue<int>("Lanceri_2_Citta", v => Variabili_Client.Citta.Città.Lanceri_2 = v);
                SetValue<int>("Lanceri_3_Citta", v => Variabili_Client.Citta.Città.Lanceri_3 = v);
                SetValue<int>("Lanceri_4_Citta", v => Variabili_Client.Citta.Città.Lanceri_4 = v);
                SetValue<int>("Lanceri_5_Citta", v => Variabili_Client.Citta.Città.Lanceri_5 = v);

                SetValue<int>("Arceri_1_Citta", v => Variabili_Client.Citta.Città.Arceri_1 = v);
                SetValue<int>("Arceri_2_Citta", v => Variabili_Client.Citta.Città.Arceri_2 = v);
                SetValue<int>("Arceri_3_Citta", v => Variabili_Client.Citta.Città.Arceri_3 = v);
                SetValue<int>("Arceri_4_Citta", v => Variabili_Client.Citta.Città.Arceri_4 = v);
                SetValue<int>("Arceri_5_Citta", v => Variabili_Client.Citta.Città.Arceri_5 = v);

                SetValue<int>("Catapulte_1_Citta", v => Variabili_Client.Citta.Città.Catapulte_1 = v);
                SetValue<int>("Catapulte_2_Citta", v => Variabili_Client.Citta.Città.Catapulte_2 = v);
                SetValue<int>("Catapulte_3_Citta", v => Variabili_Client.Citta.Città.Catapulte_3 = v);
                SetValue<int>("Catapulte_4_Citta", v => Variabili_Client.Citta.Città.Catapulte_4 = v);
                SetValue<int>("Catapulte_5_Citta", v => Variabili_Client.Citta.Città.Catapulte_5 = v);

                // Cancello
                SetValue<int>("Guarnigione_Cancello", v => Variabili_Client.Citta.Cancello.Guarnigione = v);
                SetValue<int>("Guarnigione_CancelloMax", v => Variabili_Client.Citta.Cancello.Guarnigione_Max = v);
                SetValue<int>("Salute_Cancello", v => Variabili_Client.Citta.Cancello.Salute = v);
                SetValue<int>("Salute_CancelloMax", v => Variabili_Client.Citta.Cancello.Salute_Max = v);
                SetValue<int>("Difesa_Cancello", v => Variabili_Client.Citta.Cancello.Difesa = v);
                SetValue<int>("Difesa_CancelloMax", v => Variabili_Client.Citta.Cancello.Difesa_Max = v);

                SetValue<int>("Guerrieri_1_Cancello", v => Variabili_Client.Citta.Cancello.Guerrieri_1 = v);
                SetValue<int>("Guerrieri_2_Cancello", v => Variabili_Client.Citta.Cancello.Guerrieri_2 = v);
                SetValue<int>("Guerrieri_3_Cancello", v => Variabili_Client.Citta.Cancello.Guerrieri_3 = v);
                SetValue<int>("Guerrieri_4_Cancello", v => Variabili_Client.Citta.Cancello.Guerrieri_4 = v);
                SetValue<int>("Guerrieri_5_Cancello", v => Variabili_Client.Citta.Cancello.Guerrieri_5 = v);

                SetValue<int>("Lanceri_1_Cancello", v => Variabili_Client.Citta.Cancello.Lanceri_1 = v);
                SetValue<int>("Lanceri_2_Cancello", v => Variabili_Client.Citta.Cancello.Lanceri_2 = v);
                SetValue<int>("Lanceri_3_Cancello", v => Variabili_Client.Citta.Cancello.Lanceri_3 = v);
                SetValue<int>("Lanceri_4_Cancello", v => Variabili_Client.Citta.Cancello.Lanceri_4 = v);
                SetValue<int>("Lanceri_5_Cancello", v => Variabili_Client.Citta.Cancello.Lanceri_5 = v);

                SetValue<int>("Arceri_1_Cancello", v => Variabili_Client.Citta.Cancello.Arceri_1 = v);
                SetValue<int>("Arceri_2_Cancello", v => Variabili_Client.Citta.Cancello.Arceri_2 = v);
                SetValue<int>("Arceri_3_Cancello", v => Variabili_Client.Citta.Cancello.Arceri_3 = v);
                SetValue<int>("Arceri_4_Cancello", v => Variabili_Client.Citta.Cancello.Arceri_4 = v);
                SetValue<int>("Arceri_5_Cancello", v => Variabili_Client.Citta.Cancello.Arceri_5 = v);

                SetValue<int>("Catapulte_1_Cancello", v => Variabili_Client.Citta.Cancello.Catapulte_1 = v);
                SetValue<int>("Catapulte_2_Cancello", v => Variabili_Client.Citta.Cancello.Catapulte_2 = v);
                SetValue<int>("Catapulte_3_Cancello", v => Variabili_Client.Citta.Cancello.Catapulte_3 = v);
                SetValue<int>("Catapulte_4_Cancello", v => Variabili_Client.Citta.Cancello.Catapulte_4 = v);
                SetValue<int>("Catapulte_5_Cancello", v => Variabili_Client.Citta.Cancello.Catapulte_5 = v);

                // Mura
                SetValue<int>("Guarnigione_Mura", v => Variabili_Client.Citta.Mura.Guarnigione = v);
                SetValue<int>("Guarnigione_MuraMax", v => Variabili_Client.Citta.Mura.Guarnigione_Max = v);
                SetValue<int>("Salute_Mura", v => Variabili_Client.Citta.Mura.Salute = v);
                SetValue<int>("Salute_MuraMax", v => Variabili_Client.Citta.Mura.Salute_Max = v);
                SetValue<int>("Difesa_Mura", v => Variabili_Client.Citta.Mura.Difesa = v);
                SetValue<int>("Difesa_MuraMax", v => Variabili_Client.Citta.Mura.Difesa_Max = v);

                SetValue<int>("Guerrieri_1_Mura", v => Variabili_Client.Citta.Mura.Guerrieri_1 = v);
                SetValue<int>("Guerrieri_2_Mura", v => Variabili_Client.Citta.Mura.Guerrieri_2 = v);
                SetValue<int>("Guerrieri_3_Mura", v => Variabili_Client.Citta.Mura.Guerrieri_3 = v);
                SetValue<int>("Guerrieri_4_Mura", v => Variabili_Client.Citta.Mura.Guerrieri_4 = v);
                SetValue<int>("Guerrieri_5_Mura", v => Variabili_Client.Citta.Mura.Guerrieri_5 = v);

                SetValue<int>("Lanceri_1_Mura", v => Variabili_Client.Citta.Mura.Lanceri_1 = v);
                SetValue<int>("Lanceri_2_Mura", v => Variabili_Client.Citta.Mura.Lanceri_2 = v);
                SetValue<int>("Lanceri_3_Mura", v => Variabili_Client.Citta.Mura.Lanceri_3 = v);
                SetValue<int>("Lanceri_4_Mura", v => Variabili_Client.Citta.Mura.Lanceri_4 = v);
                SetValue<int>("Lanceri_5_Mura", v => Variabili_Client.Citta.Mura.Lanceri_5 = v);

                SetValue<int>("Arceri_1_Mura", v => Variabili_Client.Citta.Mura.Arceri_1 = v);
                SetValue<int>("Arceri_2_Mura", v => Variabili_Client.Citta.Mura.Arceri_2 = v);
                SetValue<int>("Arceri_3_Mura", v => Variabili_Client.Citta.Mura.Arceri_3 = v);
                SetValue<int>("Arceri_4_Mura", v => Variabili_Client.Citta.Mura.Arceri_4 = v);
                SetValue<int>("Arceri_5_Mura", v => Variabili_Client.Citta.Mura.Arceri_5 = v);

                SetValue<int>("Catapulte_1_Mura", v => Variabili_Client.Citta.Mura.Catapulte_1 = v);
                SetValue<int>("Catapulte_2_Mura", v => Variabili_Client.Citta.Mura.Catapulte_2 = v);
                SetValue<int>("Catapulte_3_Mura", v => Variabili_Client.Citta.Mura.Catapulte_3 = v);
                SetValue<int>("Catapulte_4_Mura", v => Variabili_Client.Citta.Mura.Catapulte_4 = v);
                SetValue<int>("Catapulte_5_Mura", v => Variabili_Client.Citta.Mura.Catapulte_5 = v);

                // Torri
                SetValue<int>("Guarnigione_Torri", v => Variabili_Client.Citta.Torri.Guarnigione = v);
                SetValue<int>("Guarnigione_TorriMax", v => Variabili_Client.Citta.Torri.Guarnigione_Max = v);
                SetValue<int>("Salute_Torri", v => Variabili_Client.Citta.Torri.Salute = v);
                SetValue<int>("Salute_TorriMax", v => Variabili_Client.Citta.Torri.Salute_Max = v);
                SetValue<int>("Difesa_Torri", v => Variabili_Client.Citta.Torri.Difesa = v);
                SetValue<int>("Difesa_TorriMax", v => Variabili_Client.Citta.Torri.Difesa_Max = v);

                SetValue<int>("Guerrieri_1_Torri", v => Variabili_Client.Citta.Torri.Guerrieri_1 = v);
                SetValue<int>("Guerrieri_2_Torri", v => Variabili_Client.Citta.Torri.Guerrieri_2 = v);
                SetValue<int>("Guerrieri_3_Torri", v => Variabili_Client.Citta.Torri.Guerrieri_3 = v);
                SetValue<int>("Guerrieri_4_Torri", v => Variabili_Client.Citta.Torri.Guerrieri_4 = v);
                SetValue<int>("Guerrieri_5_Torri", v => Variabili_Client.Citta.Torri.Guerrieri_5 = v);

                SetValue<int>("Lanceri_1_Torri", v => Variabili_Client.Citta.Torri.Lanceri_1 = v);
                SetValue<int>("Lanceri_2_Torri", v => Variabili_Client.Citta.Torri.Lanceri_2 = v);
                SetValue<int>("Lanceri_3_Torri", v => Variabili_Client.Citta.Torri.Lanceri_3 = v);
                SetValue<int>("Lanceri_4_Torri", v => Variabili_Client.Citta.Torri.Lanceri_4 = v);
                SetValue<int>("Lanceri_5_Torri", v => Variabili_Client.Citta.Torri.Lanceri_5 = v);

                SetValue<int>("Arceri_1_Torri", v => Variabili_Client.Citta.Torri.Arceri_1 = v);
                SetValue<int>("Arceri_2_Torri", v => Variabili_Client.Citta.Torri.Arceri_2 = v);
                SetValue<int>("Arceri_3_Torri", v => Variabili_Client.Citta.Torri.Arceri_3 = v);
                SetValue<int>("Arceri_4_Torri", v => Variabili_Client.Citta.Torri.Arceri_4 = v);
                SetValue<int>("Arceri_5_Torri", v => Variabili_Client.Citta.Torri.Arceri_5 = v);

                SetValue<int>("Catapulte_1_Torri", v => Variabili_Client.Citta.Torri.Catapulte_1 = v);
                SetValue<int>("Catapulte_2_Torri", v => Variabili_Client.Citta.Torri.Catapulte_2 = v);
                SetValue<int>("Catapulte_3_Torri", v => Variabili_Client.Citta.Torri.Catapulte_3 = v);
                SetValue<int>("Catapulte_4_Torri", v => Variabili_Client.Citta.Torri.Catapulte_4 = v);
                SetValue<int>("Catapulte_5_Torri", v => Variabili_Client.Citta.Torri.Catapulte_5 = v);

                // Castello
                SetValue<int>("Guarnigione_Castello", v => Variabili_Client.Citta.Castello.Guarnigione = v);
                SetValue<int>("Guarnigione_CastelloMax", v => Variabili_Client.Citta.Castello.Guarnigione_Max = v);
                SetValue<int>("Salute_Castello", v => Variabili_Client.Citta.Castello.Salute = v);
                SetValue<int>("Salute_CastelloMax", v => Variabili_Client.Citta.Castello.Salute_Max = v);
                SetValue<int>("Difesa_Castello", v => Variabili_Client.Citta.Castello.Difesa = v);
                SetValue<int>("Difesa_CastelloMax", v => Variabili_Client.Citta.Castello.Difesa_Max = v);

                SetValue<int>("Guerrieri_1_Castello", v => Variabili_Client.Citta.Castello.Guerrieri_1 = v);
                SetValue<int>("Guerrieri_2_Castello", v => Variabili_Client.Citta.Castello.Guerrieri_2 = v);
                SetValue<int>("Guerrieri_3_Castello", v => Variabili_Client.Citta.Castello.Guerrieri_3 = v);
                SetValue<int>("Guerrieri_4_Castello", v => Variabili_Client.Citta.Castello.Guerrieri_4 = v);
                SetValue<int>("Guerrieri_5_Castello", v => Variabili_Client.Citta.Castello.Guerrieri_5 = v);

                SetValue<int>("Lanceri_1_Castello", v => Variabili_Client.Citta.Castello.Lanceri_1 = v);
                SetValue<int>("Lanceri_2_Castello", v => Variabili_Client.Citta.Castello.Lanceri_2 = v);
                SetValue<int>("Lanceri_3_Castello", v => Variabili_Client.Citta.Castello.Lanceri_3 = v);
                SetValue<int>("Lanceri_4_Castello", v => Variabili_Client.Citta.Castello.Lanceri_4 = v);
                SetValue<int>("Lanceri_5_Castello", v => Variabili_Client.Citta.Castello.Lanceri_5 = v);

                SetValue<int>("Arceri_1_Castello", v => Variabili_Client.Citta.Castello.Arceri_1 = v);
                SetValue<int>("Arceri_2_Castello", v => Variabili_Client.Citta.Castello.Arceri_2 = v);
                SetValue<int>("Arceri_3_Castello", v => Variabili_Client.Citta.Castello.Arceri_3 = v);
                SetValue<int>("Arceri_4_Castello", v => Variabili_Client.Citta.Castello.Arceri_4 = v);
                SetValue<int>("Arceri_5_Castello", v => Variabili_Client.Citta.Castello.Arceri_5 = v);

                SetValue<int>("Catapulte_1_Castello", v => Variabili_Client.Citta.Castello.Catapulte_1 = v);
                SetValue<int>("Catapulte_2_Castello", v => Variabili_Client.Citta.Castello.Catapulte_2 = v);
                SetValue<int>("Catapulte_3_Castello", v => Variabili_Client.Citta.Castello.Catapulte_3 = v);
                SetValue<int>("Catapulte_4_Castello", v => Variabili_Client.Citta.Castello.Catapulte_4 = v);
                SetValue<int>("Catapulte_5_Castello", v => Variabili_Client.Citta.Castello.Catapulte_5 = v);

                // Statistiche
                SetValue<string>("Potenza_Totale", v => Variabili_Client.Statistiche.Potenza_Totale = v);
                SetValue<string>("Potenza_Strutture", v => Variabili_Client.Statistiche.Potenza_Edifici = v);
                SetValue<string>("Potenza_Ricerca", v => Variabili_Client.Statistiche.Potenza_Ricerca = v);
                SetValue<string>("Potenza_Esercito", v => Variabili_Client.Statistiche.Potenza_Esercito = v);

                SetValue<string>("Unità_Eliminate", v => Variabili_Client.Statistiche.Unità_Eliminate = v);
                SetValue<string>("Guerrieri_Eliminate", v => Variabili_Client.Statistiche.Guerrieri_Eliminate = v);
                SetValue<string>("Lanceri_Eliminate", v => Variabili_Client.Statistiche.Lanceri_Eliminate = v);
                SetValue<string>("Arceri_Eliminate", v => Variabili_Client.Statistiche.Arceri_Eliminate = v);
                SetValue<string>("Catapulte_Eliminate", v => Variabili_Client.Statistiche.Catapulte_Eliminate = v);

                SetValue<string>("Unità_Perse", v => Variabili_Client.Statistiche.Unità_Perse = v);
                SetValue<string>("Guerrieri_Persi", v => Variabili_Client.Statistiche.Guerrieri_Persi = v);
                SetValue<string>("Lanceri_Persi", v => Variabili_Client.Statistiche.Lanceri_Persi = v);
                SetValue<string>("Arceri_Persi", v => Variabili_Client.Statistiche.Arceri_Persi = v);
                SetValue<string>("Catapulte_Persi", v => Variabili_Client.Statistiche.Catapulte_Perse = v);
                SetValue<string>("Risorse_Razziate", v => Variabili_Client.Statistiche.Risorse_Razziate = v);

                SetValue<string>("Strutture_Civili_Costruite", v => Variabili_Client.Statistiche.Strutture_Civili_Costruite = v);
                SetValue<string>("Strutture_Militari_Costruite", v => Variabili_Client.Statistiche.Strutture_Militari_Costruite = v);
                SetValue<string>("Caserme_Costruite", v => Variabili_Client.Statistiche.Caserme_Costruite = v);

                SetValue<string>("Frecce_Utilizzate", v => Variabili_Client.Statistiche.Frecce_Utilizzate = v);
                SetValue<string>("Battaglie_Vinte", v => Variabili_Client.Statistiche.Battaglie_Vinte = v);
                SetValue<string>("Battaglie_Perse", v => Variabili_Client.Statistiche.Battaglie_Perse = v);
                SetValue<string>("Quest_Completate", v => Variabili_Client.Statistiche.Quest_Completate = v);
                SetValue<string>("Attacchi_Subiti_PVP", v => Variabili_Client.Statistiche.Attacchi_Subiti_PVP = v);
                SetValue<string>("Attacchi_Effettuati_PVP", v => Variabili_Client.Statistiche.Attacchi_Effettuati_PVP = v);

                SetValue<string>("Barbari_Sconfitti", v => Variabili_Client.Statistiche.Barbari_Sconfitti = v);
                SetValue<string>("Accampamenti_Barbari_Sconfitti", v => Variabili_Client.Statistiche.Accampamenti_Barbari_Sconfitti = v);
                SetValue<string>("Città_Barbare_Sconfitte", v => Variabili_Client.Statistiche.Città_Barbare_Sconfitte = v);
                SetValue<string>("Danno_HP_Barbaro", v => Variabili_Client.Statistiche.Danno_HP_Barbaro = v);
                SetValue<string>("Danno_DEF_Barbaro", v => Variabili_Client.Statistiche.Danno_DEF_Barbaro = v);

                SetValue<string>("Unità_Addestrate", v => Variabili_Client.Statistiche.Unità_Addestrate = v);
                SetValue<string>("Risorse_Utilizzate", v => Variabili_Client.Statistiche.Risorse_Utilizzate = v);
                SetValue<string>("Tempo_Addestramento_Risparmiato", v => Variabili_Client.Statistiche.Tempo_Addestramento_Totale = v);
                SetValue<string>("Tempo_Costruzione_Risparmiato", v => Variabili_Client.Statistiche.Tempo_Costruzione_Totale = v);
                SetValue<string>("Tempo_Ricerca_Risparmiato", v => Variabili_Client.Statistiche.Tempo_Ricerca_Totale = v);
                SetValue<string>("Tempo_Sottratto_Diamanti", v => Variabili_Client.Statistiche.Tempo_Sottratto_Diamanti = v);

                //Bonus 
                SetValue<string>("Bonus_Costruzione", v => Variabili_Client.Bonus.Bonus_Costruzione = v);
                SetValue<string>("Bonus_Addestramento", v => Variabili_Client.Bonus.Bonus_Addestramento = v);
                SetValue<string>("Bonus_Ricerca", v => Variabili_Client.Bonus.Bonus_Ricerca = v);
                SetValue<string>("Bonus_Riparazione", v => Variabili_Client.Bonus.Bonus_Riparazione = v);
                SetValue<string>("Bonus_Produzione_Risorse", v => Variabili_Client.Bonus.Bonus_Produzione_Risorse = v);
                SetValue<string>("Bonus_Capacità_Trasporto", v => Variabili_Client.Bonus.Bonus_Capacità_Trasporto = v);

                SetValue<string>("Bonus_Salute_Strutture", v => Variabili_Client.Bonus.Bonus_Salute_Strutture = v);
                SetValue<string>("Bonus_Difesa_Strutture", v => Variabili_Client.Bonus.Bonus_Difesa_Strutture = v);
                SetValue<string>("Bonus_Guarnigione_Strutture", v => Variabili_Client.Bonus.Bonus_Guarnigione_Strutture = v);

                SetValue<string>("Bonus_Attacco_Guerrieri", v => Variabili_Client.Bonus.Bonus_Attacco_Guerrieri = v);
                SetValue<string>("Bonus_Salute_Guerrieri", v => Variabili_Client.Bonus.Bonus_Salute_Guerrieri = v);
                SetValue<string>("Bonus_Difesa_Guerrieri", v => Variabili_Client.Bonus.Bonus_Difesa_Guerrieri = v);
                SetValue<string>("Bonus_Attacco_Lanceri", v => Variabili_Client.Bonus.Bonus_Attacco_Lanceri = v);
                SetValue<string>("Bonus_Salute_Lanceri", v => Variabili_Client.Bonus.Bonus_Salute_Lanceri = v);
                SetValue<string>("Bonus_Difesa_Lanceri", v => Variabili_Client.Bonus.Bonus_Difesa_Lanceri = v);
                SetValue<string>("Bonus_Attacco_Arceri", v => Variabili_Client.Bonus.Bonus_Attacco_Arceri = v);
                SetValue<string>("Bonus_Salute_Arceri", v => Variabili_Client.Bonus.Bonus_Salute_Arceri = v);
                SetValue<string>("Bonus_Difesa_Arceri", v => Variabili_Client.Bonus.Bonus_Difesa_Arceri = v);
                SetValue<string>("Bonus_Attacco_Catapulte", v => Variabili_Client.Bonus.Bonus_Attacco_Catapulte = v);
                SetValue<string>("Bonus_Salute_Catapulte", v => Variabili_Client.Bonus.Bonus_Salute_Catapulte = v);
                SetValue<string>("Bonus_Difesa_Catapulte", v => Variabili_Client.Bonus.Bonus_Difesa_Catapulte = v);

                //Tutorial
                SetValue<bool>("Tutorial", v => Variabili_Client.tutorial_Attivo = v);

                SetValue<bool>("Tutorial_1", v => Variabili_Client.tutorial[0] = v);
                SetValue<bool>("Tutorial_2", v => Variabili_Client.tutorial[1] = v);
                SetValue<bool>("Tutorial_3", v => Variabili_Client.tutorial[2] = v);
                SetValue<bool>("Tutorial_4", v => Variabili_Client.tutorial[3] = v);
                SetValue<bool>("Tutorial_5", v => Variabili_Client.tutorial[4] = v);
                SetValue<bool>("Tutorial_6", v => Variabili_Client.tutorial[5] = v);
                SetValue<bool>("Tutorial_7", v => Variabili_Client.tutorial[6] = v);
                SetValue<bool>("Tutorial_8", v => Variabili_Client.tutorial[7] = v);
                SetValue<bool>("Tutorial_9", v => Variabili_Client.tutorial[8] = v);
                SetValue<bool>("Tutorial_10", v => Variabili_Client.tutorial[9] = v);
                SetValue<bool>("Tutorial_11", v => Variabili_Client.tutorial[10] = v);
                SetValue<bool>("Tutorial_12", v => Variabili_Client.tutorial[11] = v);
                SetValue<bool>("Tutorial_13", v => Variabili_Client.tutorial[12] = v);
                SetValue<bool>("Tutorial_14", v => Variabili_Client.tutorial[13] = v);
                SetValue<bool>("Tutorial_15", v => Variabili_Client.tutorial[14] = v);
                SetValue<bool>("Tutorial_16", v => Variabili_Client.tutorial[15] = v);
                SetValue<bool>("Tutorial_17", v => Variabili_Client.tutorial[16] = v);
                SetValue<bool>("Tutorial_18", v => Variabili_Client.tutorial[17] = v);
                SetValue<bool>("Tutorial_19", v => Variabili_Client.tutorial[18] = v);
                SetValue<bool>("Tutorial_20", v => Variabili_Client.tutorial[19] = v);
                SetValue<bool>("Tutorial_21", v => Variabili_Client.tutorial[20] = v);
                SetValue<bool>("Tutorial_22", v => Variabili_Client.tutorial[21] = v);
                SetValue<bool>("Tutorial_23", v => Variabili_Client.tutorial[22] = v);
                SetValue<bool>("Tutorial_24", v => Variabili_Client.tutorial[23] = v);
                SetValue<bool>("Tutorial_25", v => Variabili_Client.tutorial[24] = v);
                SetValue<bool>("Tutorial_26", v => Variabili_Client.tutorial[25] = v);
                SetValue<bool>("Tutorial_27", v => Variabili_Client.tutorial[26] = v);
                SetValue<bool>("Tutorial_28", v => Variabili_Client.tutorial[27] = v);
                SetValue<bool>("Tutorial_29", v => Variabili_Client.tutorial[28] = v);
                SetValue<bool>("Tutorial_30", v => Variabili_Client.tutorial[29] = v);
                SetValue<bool>("Tutorial_31", v => Variabili_Client.tutorial[30] = v);
                SetValue<bool>("Tutorial_32", v => Variabili_Client.tutorial[31] = v);

                //Dati
                SetValue<string>("Code_Costruzioni", v => Variabili_Client.Utente.Code_Costruzione = v);
                SetValue<string>("Code_Reclutamenti", v => Variabili_Client.Utente.Code_Reclutamento = v);

                SetValue<string>("Code_Costruzioni_Disponibili", v => Variabili_Client.Utente.Code_Costruzione_Disponibili = v);
                SetValue<string>("Code_Reclutamenti_Disponibili", v => Variabili_Client.Utente.Code_Reclutamento_Disponibili = v);

                SetValue<string>("Tempo_Costruzione", v => Variabili_Client.Utente.Tempo_Costruzione = v);
                SetValue<string>("Tempo_Reclutamento", v => Variabili_Client.Utente.Tempo_Reclutamento = v);
                SetValue<string>("Tempo_Ricerca_Citta", v => Variabili_Client.Utente.Tempo_Ricerca = v);
                //SetValue<int>("Tempo_Ricerca_Globale", v => Variabili_Client.Utente.Tempo_Ricerca = v);

                SetValue<bool>("Ricerca_Attiva", v => Variabili_Client.Utente.Ricerca_Attiva = v);

                SetValue<string>("D_Viola_D_Blu", v => Variabili_Client.D_Viola_D_Blu = v);
                SetValue<string>("Tempo_D_Blu", v => Variabili_Client.Tempo_D_Blu = v);
            }
            static void Update_Log(string mes)
            {
                Gioco.Log_Update(mes);
            }
            static void Update_Desc(string tipo, string desc)
            {
                switch (tipo)
                {
                    case "Esperienza":
                        Variabili_Client.Esperienza_Desc = desc;
                        break;
                    case "Livello":
                        Variabili_Client.Livello_Desc = desc;
                        break;

                    case "Giocatore":
                        Variabili_Client.Giocatore_Desc = desc;
                        break;
                    case "Diamanti Blu":
                        Variabili_Client.Diamanti_Blu_Desc = desc;
                        break;
                    case "Diamanti Viola":
                        Variabili_Client.Diamanti_Viola_Desc = desc;
                        break;
                    case "Dollari Virtuali":
                        Variabili_Client.Dollari_VIrtuali_Desc = desc;
                        break;

                    case "Cibo":
                        Variabili_Client.Cibo_Desc = desc;
                        break;
                    case "Legno":
                        Variabili_Client.Legno_Desc = desc;
                        break;
                    case "Pietra":
                        Variabili_Client.Pietra_Desc = desc;
                        break;
                    case "Ferro":
                        Variabili_Client.Ferro_Desc = desc;
                        break;
                    case "Oro":
                        Variabili_Client.Oro_Desc = desc;
                        break;
                    case "Popolazione":
                        Variabili_Client.Popolazione_Desc = desc;
                        break;

                    case "Spade":
                        Variabili_Client.Spade_Desc = desc;
                        break;
                    case "Lance":
                        Variabili_Client.Lance_Desc = desc;
                        break;
                    case "Archi":
                        Variabili_Client.Archi_Desc = desc;
                        break;
                    case "Scudi":
                        Variabili_Client.Scudi_Desc = desc;
                        break;
                    case "Armature":
                        Variabili_Client.Armature_Desc = desc;
                        break;
                    case "Frecce":
                        Variabili_Client.Frecce_Desc = desc;
                        break;

                    case "Fattoria":
                        Variabili_Client.Costruzione.Fattorie.Descrizione = desc;
                        break;
                    case "Segheria":
                        Variabili_Client.Costruzione.Segherie.Descrizione = desc;
                        break;
                    case "Cava di Pietra":
                        Variabili_Client.Costruzione.CaveDiPietra.Descrizione = desc;
                        break;
                    case "Miniera di Ferro":
                        Variabili_Client.Costruzione.Miniera_Ferro.Descrizione = desc;
                        break;
                    case "Miniera d'Oro":
                        Variabili_Client.Costruzione.Miniera_Oro.Descrizione = desc;
                        break;
                    case "Case":
                        Variabili_Client.Costruzione.Case.Descrizione = desc;
                        break;

                    case "Produzione Spade":
                        Variabili_Client.Costruzione.Workshop_Spade.Descrizione = desc;
                        break;
                    case "Produzione Lance":
                        Variabili_Client.Costruzione.Workshop_Lance.Descrizione = desc;
                        break;
                    case "Produzione Archi":
                        Variabili_Client.Costruzione.Workshop_Archi.Descrizione = desc;
                        break;
                    case "Produzione Scudi":
                        Variabili_Client.Costruzione.Workshop_Scudi.Descrizione = desc;
                        break;
                    case "Produzione Armature":
                        Variabili_Client.Costruzione.Workshop_Armature.Descrizione = desc;
                        break;
                    case "Produzione Frecce":
                        Variabili_Client.Costruzione.Workshop_Frecce.Descrizione = desc;
                        break;

                    case "Guerrieri 1":
                        Variabili_Client.Reclutamento.Guerrieri_1.Descrizione = desc;
                        break;
                    case "Guerrieri 2":
                        Variabili_Client.Reclutamento.Guerrieri_2.Descrizione = desc;
                        break;
                    case "Guerrieri 3":
                        Variabili_Client.Reclutamento.Guerrieri_3.Descrizione = desc;
                        break;
                    case "Guerrieri 4":
                        Variabili_Client.Reclutamento.Guerrieri_4.Descrizione = desc;
                        break;
                    case "Guerrieri 5":
                        Variabili_Client.Reclutamento.Guerrieri_5.Descrizione = desc;
                        break;

                    case "Lanceri 1":
                        Variabili_Client.Reclutamento.Lanceri_1.Descrizione = desc;
                        break;
                    case "Lanceri 2":
                        Variabili_Client.Reclutamento.Lanceri_2.Descrizione = desc;
                        break;
                    case "Lanceri 3":
                        Variabili_Client.Reclutamento.Lanceri_3.Descrizione = desc;
                        break;
                    case "Lanceri 4":
                        Variabili_Client.Reclutamento.Lanceri_4.Descrizione = desc;
                        break;
                    case "Lanceri 5":
                        Variabili_Client.Reclutamento.Lanceri_5.Descrizione = desc;
                        break;

                    case "Arceri 1":
                        Variabili_Client.Reclutamento.Arceri_1.Descrizione = desc;
                        break;
                    case "Arceri 2":
                        Variabili_Client.Reclutamento.Arceri_2.Descrizione = desc;
                        break;
                    case "Arceri 3":
                        Variabili_Client.Reclutamento.Arceri_3.Descrizione = desc;
                        break;
                    case "Arceri 4":
                        Variabili_Client.Reclutamento.Arceri_4.Descrizione = desc;
                        break;
                    case "Arceri 5":
                        Variabili_Client.Reclutamento.Arceri_5.Descrizione = desc;
                        break;

                    case "Catapulte 1":
                        Variabili_Client.Reclutamento.Catapulte_1.Descrizione = desc;
                        break;
                    case "Catapulte 2":
                        Variabili_Client.Reclutamento.Catapulte_2.Descrizione = desc;
                        break;
                    case "Catapulte 3":
                        Variabili_Client.Reclutamento.Catapulte_3.Descrizione = desc;
                        break;
                    case "Catapulte 4":
                        Variabili_Client.Reclutamento.Catapulte_4.Descrizione = desc;
                        break;
                    case "Catapulte 5":
                        Variabili_Client.Reclutamento.Catapulte_5.Descrizione = desc;
                        break;

                    case "Caserma Guerrieri":
                        Variabili_Client.Costruzione.Caserme_Guerrieri.Descrizione = desc;
                        break;
                    case "Caserma Lanceri":
                        Variabili_Client.Costruzione.Caserme_Lanceri.Descrizione = desc;
                        break;
                    case "Caserma Arceri":
                        Variabili_Client.Costruzione.Caserme_arceri.Descrizione = desc;
                        break;
                    case "Caserma Catapulte":
                        Variabili_Client.Costruzione.Caserme_Catapulte.Descrizione = desc;
                        break;

                    case "Mura Salute":
                        Variabili_Client.Citta.Mura.Descrizione = desc;
                        break;
                    case "Mura Difesa":
                        Variabili_Client.Citta.Mura.DescrizioneB = desc;
                        break;

                    case "Cancello Salute":
                        Variabili_Client.Citta.Cancello.Descrizione = desc;
                        break;
                    case "Cancello Difesa":
                        Variabili_Client.Citta.Cancello.DescrizioneB = desc;
                        break;

                    case "Torri Salute":
                        Variabili_Client.Citta.Torri.Descrizione = desc;
                        break;
                    case "Torri Difesa":
                        Variabili_Client.Citta.Torri.DescrizioneB = desc;
                        break;

                    case "Castello Salute":
                        Variabili_Client.Citta.Castello.Descrizione = desc;
                        break;
                    case "Castello Difesa":
                        Variabili_Client.Citta.Castello.DescrizioneB = desc;
                        break;

                    case "Ricerca Addestramento":
                        Variabili_Client.Ricerca_Addestramento_Desc = desc;
                        break;
                    case "Ricerca Costruzione":
                        Variabili_Client.Ricerca_Costruzione_Desc = desc;
                        break;
                    case "Ricerca Produzione":
                        Variabili_Client.Ricerca_Produzione_Desc = desc;
                        break;
                    case "Ricerca Popolazione":
                        Variabili_Client.Ricerca_Popolazione_Desc = desc;
                        break;
                    case "Ricerca Trasporto":
                        Variabili_Client.Ricerca_Trasporto_Desc = desc;
                        break;
                    case "Ricerca Riparazione":
                        Variabili_Client.Ricerca_Riparazione_Desc = desc;
                        break;

                    case "Ricerca Guerriero Livello":
                        Variabili_Client.Ricerca_Guerrieri_Livello_Desc = desc;
                        break;
                    case "Ricerca Guerriero Salute":
                        Variabili_Client.Ricerca_Guerrieri_Salute_Desc = desc;
                        break;
                    case "Ricerca Guerriero Attacco":
                        Variabili_Client.Ricerca_Guerrieri_Attacco_Desc = desc;
                        break;
                    case "Ricerca Guerriero Difesa":
                        Variabili_Client.Ricerca_Guerrieri_Difesa_Desc = desc;
                        break;

                    case "Ricerca Lancere Livello":
                        Variabili_Client.Ricerca_Lanceri_Livello_Desc = desc;
                        break;
                    case "Ricerca Lancere Salute":
                        Variabili_Client.Ricerca_Lanceri_Salute_Desc = desc;
                        break;
                    case "Ricerca Lancere Attacco":
                        Variabili_Client.Ricerca_Lanceri_Attacco_Desc = desc;
                        break;
                    case "Ricerca Lancere Difesa":
                        Variabili_Client.Ricerca_Lanceri_Difesa_Desc = desc;
                        break;

                    case "Ricerca Arcere Livello":
                        Variabili_Client.Ricerca_Arceri_Livello_Desc = desc;
                        break;
                    case "Ricerca Arcere Salute":
                        Variabili_Client.Ricerca_Arceri_Salute_Desc = desc;
                        break;
                    case "Ricerca Arcere Attacco":
                        Variabili_Client.Ricerca_Arceri_Attacco_Desc = desc;
                        break;
                    case "Ricerca Arcere Difesa":
                        Variabili_Client.Ricerca_Arceri_Difesa_Desc = desc;
                        break;

                    case "Ricerca Catapulta Livello":
                        Variabili_Client.Ricerca_Catapulte_Livello_Desc = desc;
                        break;
                    case "Ricerca Catapulta Salute":
                        Variabili_Client.Ricerca_Catapulte_Salute_Desc = desc;
                        break;
                    case "Ricerca Catapulta Attacco":
                        Variabili_Client.Ricerca_Catapulte_Attacco_Desc = desc;
                        break;
                    case "Ricerca Catapulta Difesa":
                        Variabili_Client.Ricerca_Catapulte_Difesa_Desc = desc;
                        break;

                    case "Ricerca Ingresso Guarnigione":
                        Variabili_Client.Ricerca_Ingresso_Guarnigione_Desc = desc;
                        break;
                    case "Ricerca Citta Guarnigione":
                        Variabili_Client.Ricerca_Citta_Guarnigione_Desc = desc;
                        break;

                    case "Ricerca Cancello Livello":
                        Variabili_Client.Ricerca_Cancello_Livello_Desc = desc;
                        break;
                    case "Ricerca Cancello Salute":
                        Variabili_Client.Ricerca_Cancello_Salute_Desc = desc;
                        break;
                    case "Ricerca Cancello Difesa":
                        Variabili_Client.Ricerca_Cancello_Difesa_Desc = desc;
                        break;
                    case "Ricerca Cancello Guarnigione":
                        Variabili_Client.Ricerca_Cancello_Guarnigione_Desc = desc;
                        break;

                    case "Ricerca Mura Livello":
                        Variabili_Client.Ricerca_Mura_Livello_Desc = desc;
                        break;
                    case "Ricerca Mura Salute":
                        Variabili_Client.Ricerca_Mura_Salute_Desc = desc;
                        break;
                    case "Ricerca Mura Difesa":
                        Variabili_Client.Ricerca_Mura_Difesa_Desc = desc;
                        break;
                    case "Ricerca Mura Guarnigione":
                        Variabili_Client.Ricerca_Mura_Guarnigione_Desc = desc;
                        break;

                    case "Ricerca Torri Livello":
                        Variabili_Client.Ricerca_Torri_Livello_Desc = desc;
                        break;
                    case "Ricerca Torri Salute":
                        Variabili_Client.Ricerca_Torri_Salute_Desc = desc;
                        break;
                    case "Ricerca Torri Difesa":
                        Variabili_Client.Ricerca_Torri_Difesa_Desc = desc;
                        break;
                    case "Ricerca Torri Guarnigione":
                        Variabili_Client.Ricerca_Torri_Guarnigione_Desc = desc;
                        break;

                    case "Ricerca Castello Livello":
                        Variabili_Client.Ricerca_Castello_Livello_Desc = desc;
                        break;
                    case "Ricerca Castello Salute":
                        Variabili_Client.Ricerca_Castello_Salute_Desc = desc;
                        break;
                    case "Ricerca Castello Difesa":
                        Variabili_Client.Ricerca_Castello_Difesa_Desc = desc;
                        break;
                    case "Ricerca Castello Guarnigione":
                        Variabili_Client.Ricerca_Castello_Guarnigione_Desc = desc;
                        break;

                    //Shop
                    case "Shop Vip 1":
                        Variabili_Client.Shop.Vip_1.desc = desc;
                        break;
                    case "Shop Vip 2":
                        Variabili_Client.Shop.Vip_2.desc = desc;
                        break;

                    case "Shop Costruttore 24h":
                        Variabili_Client.Shop.Costruttore_24h.desc = desc;
                        break;
                    case "Shop Costruttore 48h":
                        Variabili_Client.Shop.Costruttore_48h.desc = desc;
                        break;
                    case "Shop Reclutatore 24h":
                        Variabili_Client.Shop.Reclutatore_24h.desc = desc;
                        break;
                    case "Shop Reclutatore 48h":
                        Variabili_Client.Shop.Reclutatore_48h.desc = desc;
                        break;

                    case "Shop Scudo Pace 8h":
                        Variabili_Client.Shop.Scudo_Pace_8h.desc = desc;
                        break;
                    case "Shop Scudo Pace 24h":
                        Variabili_Client.Shop.Scudo_Pace_24h.desc = desc;
                        break;
                    case "Shop Scudo Pace 72h":
                        Variabili_Client.Shop.Scudo_Pace_72h.desc = desc;
                        break;

                    case "Shop GamePass Base":
                        Variabili_Client.Shop.GamePass_Base.desc = desc;
                        break;
                    case "Shop GamePass Avanzato":
                        Variabili_Client.Shop.GamePass_Avanzato.desc = desc;
                        break;
                }
            }
            static void Update_PVP_List(string[] mess)
            {
                var listaTemp = new List<string>();
                if (mess.Length > 2) 
                    for (int i = 2; i < mess.Length; i++) // Saltiamo i primi due elementi (mess[0] e mess[1]) e prendiamo il resto
                        listaTemp.Add(mess[i]);

                if (listaTemp.Count != 0)
                    Variabili_Client.Giocatori_PVP = listaTemp.OrderByDescending(player => GetPotenzaValue(player)).ToList(); // Ordiniamo la lista prima di assegnarla
            }

            // Funzione di supporto per estrarre la potenza dalla stringa
            static int GetPotenzaValue(string playerString)
            {
                try
                {
                    // Cerchiamo la parte dopo "Potenza: "
                    string searchTag = "Potenza: ";
                    int startIndex = playerString.IndexOf(searchTag) + searchTag.Length;
                    string potenzaStr = playerString.Substring(startIndex);

                    return int.Parse(potenzaStr);
                }
                catch
                {
                    return 0; // In caso di errore nel formato, lo mette in fondo
                }
            }
            static void Update_Lista_Raduni(string[] mess)
            {
                // Ignora il primo elemento (che è "Lista_Raduni")
                // e ricostruisci la stringa originale
                var datiCompleti = string.Join("|", mess.Skip(1)).Split('-');

                if (Variabili_Client.Raduni_Creati.Count == 0)
                    foreach (string attacco in datiCompleti)
                    {
                        var dato = attacco.Split('|');
                        if (!string.IsNullOrEmpty(attacco))
                            Variabili_Client.Raduni_Creati.Add(dato[0] + " - " + dato[1] + " - " + dato[2]);
                    }
                else
                {
                    if (datiCompleti.Count() - 1 < Variabili_Client.Raduni_Creati.Count)
                        Variabili_Client.Raduni_Creati.Clear();

                    foreach (string attacco in datiCompleti)
                    {
                        var dato = attacco.Split('|');
                        if (!string.IsNullOrEmpty(attacco))
                            if (!Variabili_Client.Raduni_Creati.Contains(dato[0] + " - " + dato[1] + " - " + dato[2]))
                                Variabili_Client.Raduni_Creati.Add(dato[0] + " - " + dato[1] + " - " + dato[2]);
                    }
                }
            }
            static void Update_Lista_Raduni_Player(string[] mess)
            {
                // Ignora il primo elemento (che è "Lista_Raduni")
                // e ricostruisci la stringa originale
                var datiCompleti = string.Join("|", mess.Skip(1)).Split('-');

                if (Variabili_Client.Raduni_InCorso.Count == 0)
                    foreach (string attacco in datiCompleti)
                    {
                        if (attacco == "") return;
                        var dato = attacco.Split('|');
                        if (!string.IsNullOrEmpty(attacco))
                            Variabili_Client.Raduni_InCorso.Add(dato[0] + " - " + dato[1] + " - " + dato[2] + " - " + dato[3] + " - " + dato[4] + " - " + dato[5] + " - " + dato[6]);
                    }
                else
                {
                    if (datiCompleti.Count() - 1 < Variabili_Client.Raduni_InCorso.Count)
                        Variabili_Client.Raduni_InCorso.Clear();

                    foreach (string attacco in datiCompleti)
                    {
                        var dato = attacco.Split('|');
                        if (!string.IsNullOrEmpty(attacco))
                            if (!Variabili_Client.Raduni_InCorso.Contains(dato[0] + " - " + dato[1] + " - " + dato[2] + " - " + dato[3] + " - " + dato[4] + " - " + dato[5] + " - " + dato[6]))
                                Variabili_Client.Raduni_InCorso.Add(dato[0] + " - " + dato[1] + " - " + dato[2] + " - " + dato[3] + " - " + dato[4] + " - " + dato[5] + " - " + dato[6]);
                    }
                }
            }
            static async void Update_Raduni_Partecipazione(string[] mess)
            {
                // Formato: CreatoreUsername|ID|NumPartecipanti|MieiGuerrieri|MieiLancieri|MieiArcieri|MieiCatapulte|TempoRimanente
                var raduno = new Variabili_Client.AttaccoPartecipazione
                {
                    Creatore = mess[1],
                    ID = mess[2],
                    NumPartecipanti = int.Parse(mess[3]),
                    MieiGuerrieri = int.Parse(mess[4]),
                    MieiLancieri = int.Parse(mess[5]),
                    MieiArcieri = int.Parse(mess[6]),
                    MieiCatapulte = int.Parse(mess[7]),
                    TempoRimanente = int.Parse(mess[8])
                };
            }
        }

    }
}
