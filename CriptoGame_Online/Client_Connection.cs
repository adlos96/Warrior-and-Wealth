using CriptoGame_Online;
using System.Globalization;
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
            public string Type { get; set; }
            public List<ClientQuestData> Quests { get; set; }
        }

        public class ClientQuestData
        {
            public int Id { get; set; }
            public string Quest_Description { get; set; }
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
            public static string _ServerIp = "localhost"; // adly.xed.im 185.229.236.183
            //public static string _ServerIp = "79.44.11.166"; // adly.xed.im 185.229.236.183
            private static int _ServerPort = 8443;
            private static bool _Ssl = false;
            private static string _CertFile = "";
            private static string _CertPass = "Password1";
            private static bool _DebugMessages = true;
            private static bool _AcceptInvalidCerts = true;
            private static bool _MutualAuth = false;
            public static WatsonTcpClient _Client = null;
            private static string _PresharedKey = null;


            public static Task InitializeClient()
            {
                return Task.Run(async () => //Crea un task e gli assegna un blocco istruzioni da eseguire.
                {
                    bool runForever = true;
                    bool success;

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

                string[] mess = null;
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

                        default: Console.WriteLine($"[Errore] >> [{messaggio}] Comando non riconosciuto"); break;
                    }
                    Console.WriteLine("");
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine($"Comando:        {mess[0]}");
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("");
                }

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

                            bool vipPlayer = root.TryGetProperty("VipPlayer", out var vipProp) && vipProp.GetBoolean();

                            // Aggiorna la memoria locale
                            MontlyQuest.CurrentRewardsNormali = rewardsNormali;
                            MontlyQuest.CurrentRewardsVip = rewardsVip;
                            MontlyQuest.CurrentRewardPoints = points;
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

                // Utente
                SetValue<string>("livello", v => Variabili_Client.Utente.Livello = v);
                SetValue<string>("esperienza", v => Variabili_Client.Utente.Esperienza = v);
                SetValue<bool>("vip", v => Variabili_Client.Utente.User_Vip = v);
                SetValue<string>("punti_quest", v => Variabili_Client.Utente.Montly_Quest_Point = v);

                SetValue<string>("costo_terreni_Virtuali", v => Variabili_Client.Utente.Costo_terreni_Virtuali = v);


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
                SetValue<string>("caserme_guerrieri", v => Variabili_Client.Costruzione.Caserme_Guerrieri.Quantità = v);
                SetValue<string>("caserme_lanceri", v => Variabili_Client.Costruzione.Caserme_Lanceri.Quantità = v);
                SetValue<string>("caserme_arceri", v => Variabili_Client.Costruzione.Caserme_arceri.Quantità = v);
                SetValue<string>("caserme_catapulte", v => Variabili_Client.Costruzione.Caserme_Catapulte.Quantità = v);

                SetValue<string>("caserme_guerrieri_coda", v => Variabili_Client.Costruzione_Coda.Caserme_Guerrieri.Quantità = v);
                SetValue<string>("caserme_lanceri_coda", v => Variabili_Client.Costruzione_Coda.Caserme_Lanceri.Quantità = v);
                SetValue<string>("caserme_arceri_coda", v => Variabili_Client.Costruzione_Coda.Caserme_arceri.Quantità = v);
                SetValue<string>("caserme_catapulte_coda", v => Variabili_Client.Costruzione_Coda.Caserme_Catapulte.Quantità = v);

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

                // Ricerca
                SetValue<string>("ricerca_produzione", v => Variabili_Client.Utente_Ricerca.Ricerca_Produzione = v);
                SetValue<string>("ricerca_costruzione", v => Variabili_Client.Utente_Ricerca.Ricerca_Costruzione = v);
                SetValue<string>("ricerca_addestramento", v => Variabili_Client.Utente_Ricerca.Ricerca_Addestramento = v);
                SetValue<string>("ricerca_popolazione", v => Variabili_Client.Utente_Ricerca.Ricerca_Popolazione = v);

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

                SetValue<string>("ricerca_cancello_salute", v => Variabili_Client.Utente_Ricerca.Ricerca_Cancello_Salute = v);
                SetValue<string>("ricerca_cancello_difesa", v => Variabili_Client.Utente_Ricerca.Ricerca_Cancello_Difesa = v);
                SetValue<string>("ricerca_cancello_guarnigione", v => Variabili_Client.Utente_Ricerca.Ricerca_Cancello_Guarnigione = v);

                SetValue<string>("ricerca_mura_salute", v => Variabili_Client.Utente_Ricerca.Ricerca_Mura_Salute = v);
                SetValue<string>("ricerca_mura_difesa", v => Variabili_Client.Utente_Ricerca.Ricerca_Mura_Difesa = v);
                SetValue<string>("ricerca_mura_guarnigione", v => Variabili_Client.Utente_Ricerca.Ricerca_Mura_Guarnigione = v);

                SetValue<string>("ricerca_torri_salute", v => Variabili_Client.Utente_Ricerca.Ricerca_Torri_Salute = v);
                SetValue<string>("ricerca_torri_difesa", v => Variabili_Client.Utente_Ricerca.Ricerca_Torri_Difesa = v);
                SetValue<string>("ricerca_torri_guarnigione", v => Variabili_Client.Utente_Ricerca.Ricerca_Torri_Guarnigione = v);

                SetValue<string>("ricerca_castello_salute", v => Variabili_Client.Utente_Ricerca.Ricerca_Castello_Salute = v);
                SetValue<string>("ricerca_castello_difesa", v => Variabili_Client.Utente_Ricerca.Ricerca_Castello_Difesa = v);
                SetValue<string>("ricerca_castello_guarnigione", v => Variabili_Client.Utente_Ricerca.Ricerca_Castello_Guarnigione = v);

                // Guarnigione Ingresso
                SetValue<int>("Guarnigione_Ingresso", v => Variabili_Client.Citta.Ingresso.Guarnigione = v);
                SetValue<int>("Guarnigione_IngressoMax", v => Variabili_Client.Citta.Ingresso.Guarnigione_Max = v);


                // Guerrieri Ingresso
                SetValue<int>("Guerrieri_1_Ingresso", v => Variabili_Client.Citta.Ingresso.Guerrieri_1 = v);
                SetValue<int>("Guerrieri_2_Ingresso", v => Variabili_Client.Citta.Ingresso.Guerrieri_2 = v);
                SetValue<int>("Guerrieri_3_Ingresso", v => Variabili_Client.Citta.Ingresso.Guerrieri_3 = v);
                SetValue<int>("Guerrieri_4_Ingresso", v => Variabili_Client.Citta.Ingresso.Guerrieri_4 = v);
                SetValue<int>("Guerrieri_5_Ingresso", v => Variabili_Client.Citta.Ingresso.Guerrieri_5 = v);

                // Lanceri Ingresso
                SetValue<int>("Lanceri_1_Ingresso", v => Variabili_Client.Citta.Ingresso.Lanceri_1 = v);
                SetValue<int>("Lanceri_2_Ingresso", v => Variabili_Client.Citta.Ingresso.Lanceri_2 = v);
                SetValue<int>("Lanceri_3_Ingresso", v => Variabili_Client.Citta.Ingresso.Lanceri_3 = v);
                SetValue<int>("Lanceri_4_Ingresso", v => Variabili_Client.Citta.Ingresso.Lanceri_4 = v);
                SetValue<int>("Lanceri_5_Ingresso", v => Variabili_Client.Citta.Ingresso.Lanceri_5 = v);

                // Arceri Ingresso
                SetValue<int>("Arceri_1_Ingresso", v => Variabili_Client.Citta.Ingresso.Arceri_1 = v);
                SetValue<int>("Arceri_2_Ingresso", v => Variabili_Client.Citta.Ingresso.Arceri_2 = v);
                SetValue<int>("Arceri_3_Ingresso", v => Variabili_Client.Citta.Ingresso.Arceri_3 = v);
                SetValue<int>("Arceri_4_Ingresso", v => Variabili_Client.Citta.Ingresso.Arceri_4 = v);
                SetValue<int>("Arceri_5_Ingresso", v => Variabili_Client.Citta.Ingresso.Arceri_5 = v);

                // Catapulte Ingresso
                SetValue<int>("Catapulte_1_Ingresso", v => Variabili_Client.Citta.Ingresso.Catapulte_1 = v);
                SetValue<int>("Catapulte_2_Ingresso", v => Variabili_Client.Citta.Ingresso.Catapulte_2 = v);
                SetValue<int>("Catapulte_3_Ingresso", v => Variabili_Client.Citta.Ingresso.Catapulte_3 = v);
                SetValue<int>("Catapulte_4_Ingresso", v => Variabili_Client.Citta.Ingresso.Catapulte_4 = v);
                SetValue<int>("Catapulte_5_Ingresso", v => Variabili_Client.Citta.Ingresso.Catapulte_5 = v);

                // Guarnigione Città
                SetValue<int>("Guarnigione_Citta", v => Variabili_Client.Citta.Città.Guarnigione = v);
                SetValue<int>("Guarnigione_CittaMax", v => Variabili_Client.Citta.Città.Guarnigione_Max = v);

                // Cancello
                SetValue<int>("Guarnigione_Cancello", v => Variabili_Client.Citta.Cancello.Guarnigione = v);
                SetValue<int>("Guarnigione_CancelloMax", v => Variabili_Client.Citta.Cancello.Guarnigione_Max = v);
                SetValue<int>("Salute_Cancello", v => Variabili_Client.Citta.Cancello.Salute = v);
                SetValue<int>("Salute_CancelloMax", v => Variabili_Client.Citta.Cancello.Salute_Max = v);
                SetValue<int>("Difesa_Cancello", v => Variabili_Client.Citta.Cancello.Difesa = v);
                SetValue<int>("Difesa_CancelloMax", v => Variabili_Client.Citta.Cancello.Difesa_Max = v);

                // Mura
                SetValue<int>("Guarnigione_Mura", v => Variabili_Client.Citta.Mura.Guarnigione = v);
                SetValue<int>("Guarnigione_MuraMax", v => Variabili_Client.Citta.Mura.Guarnigione_Max = v);
                SetValue<int>("Salute_Mura", v => Variabili_Client.Citta.Mura.Salute = v);
                SetValue<int>("Salute_MuraMax", v => Variabili_Client.Citta.Mura.Salute_Max = v);
                SetValue<int>("Difesa_Mura", v => Variabili_Client.Citta.Mura.Difesa = v);
                SetValue<int>("Difesa_MuraMax", v => Variabili_Client.Citta.Mura.Difesa_Max = v);

                // Torri
                SetValue<int>("Guarnigione_Torri", v => Variabili_Client.Citta.Torri.Guarnigione = v);
                SetValue<int>("Guarnigione_TorriMax", v => Variabili_Client.Citta.Torri.Guarnigione_Max = v);
                SetValue<int>("Salute_Torri", v => Variabili_Client.Citta.Torri.Salute = v);
                SetValue<int>("Salute_TorriMax", v => Variabili_Client.Citta.Torri.Salute_Max = v);
                SetValue<int>("Difesa_Torri", v => Variabili_Client.Citta.Torri.Difesa = v);
                SetValue<int>("Difesa_TorriMax", v => Variabili_Client.Citta.Torri.Difesa_Max = v);

                // Castello
                SetValue<int>("Guarnigione_Castello", v => Variabili_Client.Citta.Castello.Guarnigione = v);
                SetValue<int>("Guarnigione_CastelloMax", v => Variabili_Client.Citta.Castello.Guarnigione_Max = v);
                SetValue<int>("Salute_Castello", v => Variabili_Client.Citta.Castello.Salute = v);
                SetValue<int>("Salute_CastelloMax", v => Variabili_Client.Citta.Castello.Salute_Max = v);
                SetValue<int>("Difesa_Castello", v => Variabili_Client.Citta.Castello.Difesa = v);
                SetValue<int>("Difesa_CastelloMax", v => Variabili_Client.Citta.Castello.Difesa_Max = v);

                //Dati
                SetValue<string>("Code_Costruzioni", v => Variabili_Client.Utente.Code_Costruzione = v);
                SetValue<string>("Code_Reclutamenti", v => Variabili_Client.Utente.Code_Reclutamento = v);

                SetValue<string>("Tempo_Costruzione", v => Variabili_Client.Utente.Tempo_Costruzione = v);
                SetValue<string>("Tempo_Reclutamento", v => Variabili_Client.Utente.Tempo_Reclutamento = v);

                SetValue<string>("Tempo_Ricerca_Citta", v => Variabili_Client.Utente.Tempo_Ricerca = v);
                //SetValue<int>("Tempo_Ricerca_Globale", v => Variabili_Client.Utente.Tempo_Ricerca = v);

                SetValue<bool>("Ricerca_Attiva", v => Variabili_Client.Utente.Ricerca_Attiva = v);

                //Dati

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

                    case "Guerrieri":
                        Variabili_Client.Reclutamento.Guerrieri_1.Descrizione = desc;
                        break;
                    case "Lanceri":
                        Variabili_Client.Reclutamento.Lanceri_1.Descrizione = desc;
                        break;
                    case "Arceri":
                        Variabili_Client.Reclutamento.Arceri_1.Descrizione = desc;
                        break;
                    case "Catapulte":
                        Variabili_Client.Reclutamento.Catapulte_1.Descrizione = desc;
                        break;

                    case "Caserme Guerrieri":
                        Variabili_Client.Costruzione.Caserme_Guerrieri.Descrizione = desc;
                        break;
                    case "Caserme Lanceri":
                        Variabili_Client.Costruzione.Caserme_Lanceri.Descrizione = desc;
                        break;
                    case "Caserme Arceri":
                        Variabili_Client.Costruzione.Caserme_arceri.Descrizione = desc;
                        break;
                    case "Caserme Catapulte":
                        Variabili_Client.Costruzione.Caserme_Catapulte.Descrizione = desc;
                        break;
                }
            }
            static void Update_PVP_List(string[] mess)
            {
                if (mess[1] != "")
                    for (int i = 2; i <= mess.Count() - 1; i++)
                    {
                        if (!Variabili_Client.Giocatori_PVP.Contains(mess[i]) && !Variabili_Client.Utente.Username.Contains(mess[i]))
                            Variabili_Client.Giocatori_PVP.Add(mess[i]);
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
