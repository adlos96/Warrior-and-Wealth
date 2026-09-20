/* ==========================================================
   Warrior & Wealth — Web Client — 01-net.js
   ----------------------------------------------------------
   NET — connessione WebSocket al server di gioco.

   Il server espone lo stesso protocollo testuale del client
   desktop su un secondo listener, dedicato al web (vedi
   WebSocketGateway.cs — porta di default 8444, separata dalla
   8443 usata da WatsonTcp per il client WinForms).

   Configurazione dell'indirizzo: di default si connette allo
   stesso host da cui è servita la pagina (così funziona sia in
   locale sia una volta pubblicato sul VPS). Due casi:
   - pagina servita in HTTP (sviluppo locale, o VPS senza ancora un
     dominio/certificato): si continua a parlare DIRETTAMENTE con
     WebSocketGateway.cs sulla sua porta (WS_DEFAULT_PORT), come
     prima — in locale non c'è nginx davanti;
   - pagina servita in HTTPS (VPS con dominio+certificato, vedi
     setup-web.sh): WebSocketGateway.cs su Linux NON sa fare TLS
     (HttpListener managed, nessun supporto SSL), quindi si passa
     dal proxy WebSocket già pronto in nginx sulla stessa porta 443
     (location "/ws", vedi setup-web.sh) invece che dalla porta
     8444 diretta. nginx fa da terminazione TLS e inoltra in locale
     (127.0.0.1:8444) in WS in chiaro — nessuna modifica lato
     server serve per questo, il proxy c'era già.
   Se il server gira altrove durante lo sviluppo, si può comunque
   forzare l'indirizzo con:
       localStorage.setItem('ww_ws_url', 'ws://IP:PORTA/')
   dalla console del browser (ha sempre la precedenza su entrambi i casi).

   Dipende da: WW.storage (00-core.js). Usa WW.t / WW.screenLogin /
   WW.loginStatus (02-auth.js) e WW.AUTH / WW.GAME solo dentro
   funzioni chiamate più avanti (mai a livello "top level"), quindi
   funziona anche se questo file è caricato prima di quelli — l'unico
   vincolo è che TUTTI i file siano caricati prima che NET.connect()
   venga davvero chiamato (avviene in 10-main.js, per ultimo). */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  const WS_DEFAULT_PORT = 8444;
  const WS_SERVER_PORT = 8448;

  function resolveWsUrl() {
    const override = WW.storage.get("ww_ws_url");
    if (override) return override;

    const isHttps = window.location.protocol === "https:";
    // Da file:// (mockup aperto direttamente) non c'è un host valido:
    // in quel caso si assume che il server giri in locale.
    const host = window.location.hostname || "localhost";
    // HTTPS -> passa dal proxy WebSocket di nginx (path "/ws", porta 443
    // implicita, stessa origine della pagina): la porta 8444 diretta non fa
    // TLS su Linux (vedi commento in cima al file). HTTP -> parla ancora
    // direttamente con WebSocketGateway.cs sulla sua porta, come prima.
    return isHttps ? `wss://${host}:${WS_SERVER_PORT}/ws` : `ws://${host}:${WS_DEFAULT_PORT}/`;
  }

  const NET = {
    socket: null,
    connected: false,
    reconnectAttempt: 0,
    reconnectTimer: null,
    handlers: Object.create(null), // comando -> function(argsSenzaComando)
    jsonHandlers: Object.create(null), // Type (dentro il JSON) -> function(oggetto)

    on(comando, handler) {
      NET.handlers[comando] = handler;
    },

    // Alcuni messaggi del server sono JSON puro invece di "comando|arg|arg"
    // (es. QuestUpdate, QuestRewards — vedi QuestManager.cs lato server).
    // Si riconoscono e si smistano dal campo "Type" dentro l'oggetto.
    onJson(tipo, handler) {
      NET.jsonHandlers[tipo] = handler;
    },

    connect() {
      if (NET.socket && (NET.socket.readyState === WebSocket.OPEN || NET.socket.readyState === WebSocket.CONNECTING)) {
        return;
      }

      const url = resolveWsUrl();
      setConnectionStatus("connecting", WW.t("netConnecting"));

      let socket;
      try {
        socket = new WebSocket(url);
      } catch (e) {
        console.error("[NET] Impossibile aprire il WebSocket:", e);
        setConnectionStatus("error", WW.t("netUnreachable"));
        NET.scheduleReconnect();
        return;
      }
      NET.socket = socket;

      socket.addEventListener("open", () => {
        NET.connected = true;
        NET.reconnectAttempt = 0;
        setConnectionStatus("ok", WW.t("netConnected"));
        console.log("[NET] Connesso a", url);
        WW.AUTH.onSocketOpen();
      });

      socket.addEventListener("message", (event) => {
        NET.handleMessage(event.data);
      });

      socket.addEventListener("close", () => {
        const wasConnected = NET.connected;
        NET.connected = false;
        NET.socket = null;
        setConnectionStatus("error", WW.t("netDisconnected"));
        if (wasConnected) console.warn("[NET] Connessione chiusa.");
        NET.scheduleReconnect();
      });

      socket.addEventListener("error", (event) => {
        // "close" segue quasi sempre "error": qui ci limitiamo a loggare,
        // la gestione (retry, UI) è centralizzata in "close".
        console.error("[NET] Errore WebSocket:", event);
      });
    },

    scheduleReconnect() {
      if (NET.reconnectTimer) return;
      // Backoff semplice: 1s, 2s, 4s, ... fino a un massimo di 15s, per non
      // martellare il server se resta giù per un po'.
      const delay = Math.min(15000, 1000 * Math.pow(2, NET.reconnectAttempt));
      NET.reconnectAttempt += 1;
      NET.reconnectTimer = setTimeout(() => {
        NET.reconnectTimer = null;
        NET.connect();
      }, delay);
    },

    // Invia un comando nel formato testuale del protocollo:
    // NET.send("Login", "", username, password, lang, email)
    // equivale alla stringa "Login||username|password|lang|email".
    send(comando, ...args) {
      if (!NET.socket || NET.socket.readyState !== WebSocket.OPEN) {
        console.warn(`[NET] Impossibile inviare "${comando}": non connesso.`);
        return false;
      }
      const messaggio = [comando, ...args].join("|");
      NET.socket.send(messaggio);
      return true;
    },

    handleMessage(raw) {
      if (typeof raw !== "string" || raw.length === 0) return;

      // Alcuni messaggi del server (Quest, in futuro altri report) sono
      // JSON puro invece del formato "comando|arg|arg": si smistano in base
      // al campo "Type" dentro l'oggetto verso NET.jsonHandlers.
      if (raw[0] === "{" || raw[0] === "[") {
        let oggetto;
        try {
          oggetto = JSON.parse(raw);
        } catch (e) {
          console.warn("[NET] Messaggio JSON non valido:", raw);
          return;
        }
        const tipo = oggetto && oggetto.Type;
        const jsonHandler = tipo && NET.jsonHandlers[tipo];
        if (jsonHandler) {
          jsonHandler(oggetto);
        } else {
          console.log(`[NET] Messaggio JSON non gestito (Type="${tipo}"):`, oggetto);
        }
        return;
      }

      if (!raw.includes("|")) {
        console.log("[NET] Messaggio ricevuto:", raw);
        return;
      }

      const parts = raw.split("|");
      const comando = parts[0];
      const args = parts.slice(1);

      const handler = NET.handlers[comando];
      if (handler) {
        handler(args);
      } else if (comando === "Update_Data") {
        WW.GAME.applyUpdateData(args);
      } else {
        // Comando non ancora cablato lato web: lo teniamo comunque visibile
        // in console così, quando arriverà l'elenco completo dal server,
        // sarà facile aggiungere il case corrispondente.
        console.log(`[NET] Comando non gestito: "${comando}"`, args);
      }
    },
  };

  /* Indicatore di stato connessione: riusa il paragrafo di stato della
     schermata di login quando visibile; altrimenti (già in gioco) si limita
     alla console, per non disturbare l'interfaccia con un banner permanente
     finché non decidiamo un punto fisso in UI per questo indicatore. */
  function setConnectionStatus(kind, message) {
    console.log(`[NET] Stato: ${kind} — ${message}`);
    if (!WW.screenLogin.hidden) {
      WW.loginStatus.hidden = kind === "ok";
      WW.loginStatus.textContent = message;
      WW.loginStatus.classList.toggle("login-status--ok", kind === "ok");
    }
  }

  WW.NET = NET;
})(window.WW);
