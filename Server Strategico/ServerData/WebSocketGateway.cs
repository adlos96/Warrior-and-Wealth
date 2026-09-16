using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace Server_Strategico.Server
{
    /// <summary>
    /// Gateway WebSocket per il client web di Warrior &amp; Wealth.
    ///
    /// Isolato di proposito in questo file: apre un proprio HttpListener su
    /// una porta separata da quella di WatsonTcp (8443) e parla lo stesso
    /// identico protocollo testuale "comando|arg1|arg2|..." usato dal client
    /// WinForms. Non condivide il listener con WatsonTcpServer: l'unica cosa
    /// che le due vie di trasporto condividono è lo stato di gioco, tramite
    /// ServerConnection.HandleClientMessage (il core estratto da
    /// HandleClientRequest) e Server.Send (che instrada in base al guid).
    ///
    /// Attivazione: Variabili_Server.WebGatewayEnabled all'avvio, oppure a
    /// runtime dalla console del server coi comandi "webstart"/"webstop"
    /// (vedi Server.cs). Start()/Stop() sono avvolti in try/catch dal
    /// chiamante: un problema qui (porta occupata, eccezione nel parsing,
    /// ecc.) resta isolato e non deve mai impattare il loop di gioco o le
    /// connessioni WatsonTcp esistenti.
    /// </summary>
    public static class WebSocketGateway
    {
        // Guid -> WebSocket fisico del client web + lock di invio dedicato.
        // Tenuta separata dalle mappe di Server.cs (Client_Connessi /
        // Client_Connessi_Map), che restano agnostiche rispetto al trasporto:
        // qui teniamo solo "come" mandare i byte a un dato guid quando è un
        // client web.
        //
        // Il SemaphoreSlim serve perché WebSocket (sia lato HttpListener che
        // ClientWebSocket) supporta al più una SendAsync in volo per istanza:
        // due invii concorrenti sulla STESSA connessione (es. il tick del
        // game loop che manda Update_Data mentre parte la risposta a un
        // comando) fanno fallire il secondo con "There is already one
        // outstanding 'SendAsync' call...". Serializzando gli invii per
        // singola connessione (non con un lock globale, che rallenterebbe
        // tutti i client per gli invii di uno solo) il problema sparisce.
        private sealed class ClientEntry
        {
            public required WebSocket Socket { get; init; }
            public SemaphoreSlim SendLock { get; } = new SemaphoreSlim(1, 1);
        }

        private static readonly System.Collections.Concurrent.ConcurrentDictionary<Guid, ClientEntry> _clients =
            new System.Collections.Concurrent.ConcurrentDictionary<Guid, ClientEntry>();

        private static HttpListener? _listener;
        private static CancellationTokenSource? _cts;
        private static volatile bool _running = false;

        public static bool IsRunning => _running;

        public static bool IsWebSocketClient(Guid guid) => _clients.ContainsKey(guid);

        /// <summary>
        /// Avvia il listener HTTP/WebSocket sulla porta indicata. Se è già
        /// in esecuzione non fa nulla (idempotente, così "webstart" ripetuto
        /// per errore dalla console non causa un'eccezione).
        /// </summary>
        public static void Start(int port = 8444)
        {
            if (_running)
            {
                Console.WriteLine("[WebSocketGateway] Già in esecuzione.");
                return;
            }

            // Su Windows, HttpListener si appoggia a http.sys: un prefisso
            // jolly ("+"/tutte le interfacce, necessario per farsi raggiungere
            // da un telefono sulla stessa rete) richiede una URL ACL registrata
            // in anticipo ("netsh http add urlacl") o privilegi da amministratore,
            // altrimenti Start() lancia "Accesso negato". Su Linux (VPS di
            // produzione) l'implementazione gestita di HttpListener non ha
            // questa limitazione. Proviamo quindi sempre prima "+"; solo se
            // fallisce per permessi (tipicamente su Windows senza urlacl/admin)
            // ripieghiamo su "localhost", che è sempre pre-autorizzato ma
            // raggiungibile solo dalla stessa macchina — utile comunque per non
            // bloccare del tutto lo sviluppo in locale.
            string host = "+";
            try
            {
                _cts = new CancellationTokenSource();
                _listener = new HttpListener();
                _listener.Prefixes.Add($"http://{host}:{port}/");
                _listener.Start();
            }
            catch (HttpListenerException) when (OperatingSystem.IsWindows())
            {
                Console.WriteLine("[WebSocketGateway] Accesso negato per http://+:" + port + "/ (serve 'netsh http add urlacl url=http://+:" + port + "/ user=Everyone' da un prompt da amministratore, oppure avviare il server come amministratore, per essere raggiungibile da altri dispositivi in rete/LAN).");
                Console.WriteLine("[WebSocketGateway] Ripiego su 'localhost': funziona solo per test sulla stessa macchina.");
                host = "localhost";
                _listener = new HttpListener();
                _listener.Prefixes.Add($"http://{host}:{port}/");
                _listener.Start();
            }
            _running = true;

            Console.WriteLine($"[WebSocketGateway] In ascolto su http://{host}:{port}/");
            _ = AcceptLoopAsync(_listener, _cts.Token);
        }

        /// <summary>
        /// Ferma il listener e chiude tutte le connessioni web attive. Non
        /// tocca in alcun modo WatsonTcpServer o le connessioni del client
        /// desktop.
        /// </summary>
        public static void Stop()
        {
            if (!_running)
            {
                Console.WriteLine("[WebSocketGateway] Non è in esecuzione.");
                return;
            }

            _running = false;

            try { _cts?.Cancel(); } catch { }
            try { _listener?.Stop(); } catch { }
            try { _listener?.Close(); } catch { }

            foreach (var kv in _clients)
            {
                try { kv.Value.Socket.Abort(); } catch { }
                kv.Value.SendLock.Dispose();
                Server.Client_Connessi.Remove(kv.Key);
                Server.Client_Connessi_Map.TryRemove(kv.Key, out _);
            }
            _clients.Clear();

            Console.WriteLine("[WebSocketGateway] Arrestato.");
        }

        /// <summary>Chiude forzatamente la connessione di un singolo client web (usato da "disconnetti").</summary>
        public static void Disconnect(Guid guid)
        {
            if (_clients.TryGetValue(guid, out var entry))
            {
                try { entry.Socket.Abort(); } catch { }
            }
        }

        private static async Task AcceptLoopAsync(HttpListener listener, CancellationToken token)
        {
            while (_running && !token.IsCancellationRequested)
            {
                HttpListenerContext context;
                try
                {
                    context = await listener.GetContextAsync();
                }
                catch
                {
                    break; // listener fermato (Stop()) o socket chiuso: esce dal loop
                }

                _ = HandleContextAsync(context, token);
            }
        }

        private static async Task HandleContextAsync(HttpListenerContext context, CancellationToken token)
        {
            if (!context.Request.IsWebSocketRequest)
            {
                try
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
                catch { }
                return;
            }

            Guid clientGuid = Guid.NewGuid();
            WebSocket socket;
            try
            {
                var wsContext = await context.AcceptWebSocketAsync(subProtocol: null);
                socket = wsContext.WebSocket;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WebSocketGateway] Upgrade WebSocket fallito: {ex.Message}");
                try { context.Response.StatusCode = 500; context.Response.Close(); } catch { }
                return;
            }

            string clientDescription = "";
            if (OperatingSystem.IsWindows())
            {
                clientDescription = $"WS:{context.Request.RemoteEndPoint}"; //
                RegisterClient(clientGuid, socket, clientDescription);
            }
            else
            {
                string realIp = context.Request.Headers["X-Real-IP"]; //Per nginx
                clientDescription = $"WS:{realIp}"; //
                RegisterClient(clientGuid, socket, clientDescription);
            }

            if(clientDescription == "")
            {
                Console.WriteLine($"[WebSocketGateway] Connessione da client sconosciuto: {context.Request.RemoteEndPoint}");
                return;
            }
            
            var buffer = new byte[8192];
            try
            {
                while (socket.State == WebSocketState.Open && !token.IsCancellationRequested)
                {
                    using var ms = new MemoryStream();
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), token);
                        if (result.MessageType == WebSocketMessageType.Close) break;
                        ms.Write(buffer, 0, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close) break;

                    string messaggio = Encoding.UTF8.GetString(ms.ToArray());
                    if (!string.IsNullOrWhiteSpace(messaggio))
                    {
                        try
                        {
                            // Stesso core usato dai client WatsonTcp: nessuna
                            // duplicazione della logica di gioco/autenticazione.
                            ServerConnection.HandleClientMessage(clientGuid, messaggio, clientDescription);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[WebSocketGateway] Errore gestendo il messaggio da {clientDescription}: {ex.Message}");
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Stop() chiamato mentre questo client era in ascolto: normale.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WebSocketGateway] Connessione interrotta ({clientDescription}): {ex.Message}");
            }
            finally
            {
                UnregisterClient(clientGuid, clientDescription);
                try
                {
                    if (socket.State == WebSocketState.Open)
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "bye", CancellationToken.None);
                }
                catch { }
                socket.Dispose();
            }
        }

        private static void RegisterClient(Guid guid, WebSocket socket, string description)
        {
            _clients.TryAdd(guid, new ClientEntry { Socket = socket });

            // Stesso "aggancio" che Server.ClientConnected fa per i client
            // WatsonTcp: da qui in poi il resto del server vede questo guid
            // come un client qualsiasi, senza sapere che è arrivato via web.
            if (!Server.Client_Connessi.Contains(guid)) Server.Client_Connessi.Add(guid);
            Server.Client_Connessi_Map.TryAdd(guid, description);

            Console.WriteLine($"[WebSocketGateway] Client connesso: {description} [{guid}]");

            Server.Send(guid, $"Update_Data|versione_Client_Necessario={Server_Strategico.Gioco.Variabili_Server.versione_Client_Necessario}");
        }

        private static void UnregisterClient(Guid guid, string description)
        {
            if (_clients.TryRemove(guid, out var entry))
                entry.SendLock.Dispose();
            Server.Client_Connessi.Remove(guid);
            Server.Client_Connessi_Map.TryRemove(guid, out _);
            Console.WriteLine($"[WebSocketGateway] Client disconnesso: {description} [{guid}]");
        }

        /// <summary>
        /// Invia un messaggio testuale al client web identificato dal guid. No-op se non è (più) connesso.
        ///
        /// Gli invii verso la STESSA connessione vengono serializzati tramite
        /// il SendLock dell'entry: WebSocket non ammette più di una SendAsync
        /// in volo per istanza, quindi senza questo lock il game loop e una
        /// risposta diretta a un comando potevano scontrarsi e far fallire
        /// l'invio con "There is already one outstanding 'SendAsync' call...".
        /// Resta "async void" perché i chiamanti (Server.Send) trattano già
        /// l'invio come fire-and-forget; l'ordine di consegna verso un dato
        /// client è comunque garantito dal lock (FIFO su SemaphoreSlim).
        /// </summary>
        public static async void Send(Guid guid, string msg)
        {
            if (!_clients.TryGetValue(guid, out var entry)) return;

            bool lockPreso = false;
            try
            {
                await entry.SendLock.WaitAsync();
                lockPreso = true;

                if (entry.Socket.State != WebSocketState.Open) return;

                var bytes = Encoding.UTF8.GetBytes(msg);
                await entry.Socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (ObjectDisposedException)
            {
                // Il client si è disconnesso (UnregisterClient ha già fatto
                // il Dispose del lock/socket) esattamente mentre stavamo per
                // inviare: non è un errore, il messaggio va semplicemente perso.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WebSocketGateway] Invio fallito verso {guid}: {ex.Message}");
            }
            finally
            {
                if (lockPreso)
                {
                    try { entry.SendLock.Release(); } catch (ObjectDisposedException) { }
                }
            }
        }
    }
}
