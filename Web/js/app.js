/* ==========================================================
   Warrior & Wealth — Web Client — app.js
   ----------------------------------------------------------
   Stato: collegato al server vero via WebSocket (gateway
   WebSocketGateway.cs lato server, vedi game-design-overview.md).
   Protocollo identico a quello del client desktop: stringhe di
   testo "comando|arg1|arg2|..." — solo il trasporto cambia
   (WebSocket invece di WatsonTcp/TCP grezzo).

   Struttura del file:
   0) Formattazione numeri (localizzata)
   1) NET — connessione WebSocket, invio/ricezione comandi
   2) AUTH — login / registrazione / auto-login / refresh token
   3) Navigazione (tab bar in basso + toggle pannelli su mobile)
   4) Dati di gioco (Feudi / Strutture Civili / Esercito) — ancora
      con fallback ai valori "vuoti" finché non arriva il set
      completo dei comandi Update_Data lato server (vedi TODO)
   ========================================================== */

(function () {
  "use strict";

  /* ---------- 0) FORMATTAZIONE NUMERI (localizzata) ----------
     I numeri NON vanno mai scritti "a mano" nell'HTML (es. "30.000"):
     il punto come separatore delle migliaia è la convenzione italiana/
     tedesca, ma non quella americana/inglese (dove è la virgola, con il
     punto per i decimali — es. 30,000 invece di 30.000). Teniamo solo il
     valore NUMERICO grezzo e lo formattiamo con Intl.NumberFormat usando
     la lingua/paese del dispositivo di chi sta giocando. */
  const numberLocale = navigator.language || "it-IT";

  function fmtInt(n) {
    return new Intl.NumberFormat(numberLocale).format(n);
  }
  function fmtDecimal(n, decimals) {
    return new Intl.NumberFormat(numberLocale, {
      minimumFractionDigits: decimals,
      maximumFractionDigits: decimals,
    }).format(n);
  }

  /* ---------- localStorage sicuro ----------
     Wrapper che non fa mai esplodere il resto del codice se lo storage
     non è disponibile (modalità privata, policy del browser, ecc.). */
  const storage = {
    get(key) {
      try { return localStorage.getItem(key); } catch (e) { return null; }
    },
    set(key, value) {
      try { localStorage.setItem(key, value); } catch (e) { /* ignora */ }
    },
    remove(key) {
      try { localStorage.removeItem(key); } catch (e) { /* ignora */ }
    },
  };

  /* ==========================================================
     1) NET — connessione WebSocket al server di gioco
     ==========================================================
     Il server espone lo stesso protocollo testuale del client
     desktop su un secondo listener, dedicato al web (vedi
     WebSocketGateway.cs — porta di default 8444, separata dalla
     8443 usata da WatsonTcp per il client WinForms).

     Configurazione dell'indirizzo: di default si connette allo
     stesso host da cui è servita la pagina (così funziona sia in
     locale sia una volta pubblicato sul VPS), sulla porta indicata
     qui sotto. Se il server gira altrove durante lo sviluppo, si
     può forzare l'indirizzo con:
         localStorage.setItem('ww_ws_url', 'ws://IP:PORTA/')
     dalla console del browser. */
  const WS_DEFAULT_PORT = 8444;

  function resolveWsUrl() {
    const override = storage.get("ww_ws_url");
    if (override) return override;

    const proto = window.location.protocol === "https:" ? "wss:" : "ws:";
    // Da file:// (mockup aperto direttamente) non c'è un host valido:
    // in quel caso si assume che il server giri in locale.
    const host = window.location.hostname || "localhost";
    return `${proto}//${host}:${WS_DEFAULT_PORT}/`;
  }

  const NET = {
    socket: null,
    connected: false,
    // true finché non arriva la prima risposta di login/autologin: usato
    // per capire se un'eventuale disconnessione va segnalata come "server
    // irraggiungibile" oppure gestita in modo silenzioso durante il retry.
    reconnectAttempt: 0,
    reconnectTimer: null,
    handlers: Object.create(null), // comando -> function(argsSenzaComando)

    on(comando, handler) {
      NET.handlers[comando] = handler;
    },

    connect() {
      if (NET.socket && (NET.socket.readyState === WebSocket.OPEN || NET.socket.readyState === WebSocket.CONNECTING)) {
        return;
      }

      const url = resolveWsUrl();
      setConnectionStatus("connecting", t("netConnecting"));

      let socket;
      try {
        socket = new WebSocket(url);
      } catch (e) {
        console.error("[NET] Impossibile aprire il WebSocket:", e);
        setConnectionStatus("error", t("netUnreachable"));
        NET.scheduleReconnect();
        return;
      }
      NET.socket = socket;

      socket.addEventListener("open", () => {
        NET.connected = true;
        NET.reconnectAttempt = 0;
        setConnectionStatus("ok", t("netConnected"));
        console.log("[NET] Connesso a", url);
        AUTH.onSocketOpen();
      });

      socket.addEventListener("message", (event) => {
        NET.handleMessage(event.data);
      });

      socket.addEventListener("close", () => {
        const wasConnected = NET.connected;
        NET.connected = false;
        NET.socket = null;
        setConnectionStatus("error", t("netDisconnected"));
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

      // Alcuni messaggi del server (report, aggiornamenti città) sono JSON
      // puro invece del formato "comando|arg|arg"; li logghiamo/segnaliamo
      // ma senza farli esplodere nello split successivo.
      if (raw[0] === "{" || raw[0] === "[") {
        console.log("[NET] Messaggio JSON ricevuto (non ancora gestito):", raw);
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
        GAME.applyUpdateData(args);
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
    if (!screenLogin.hidden) {
      loginStatus.hidden = kind === "ok";
      loginStatus.textContent = message;
      loginStatus.classList.toggle("login-status--ok", kind === "ok");
    }
  }

  /* ==========================================================
     2) AUTH — login / registrazione / auto-login / token
     ========================================================== */

  const screenLogin = document.getElementById("screen-login");
  const screenGame = document.getElementById("screen-game");
  const formLogin = document.getElementById("form-login");
  const formRegister = document.getElementById("form-register");
  const formRecover = document.getElementById("form-recover");
  const loginStatus = document.getElementById("login-status");
  const recoverStatus = document.getElementById("recover-status");
  const chkRemember = document.getElementById("chk-remember");

  function showOnly(formToShow) {
    [formLogin, formRegister, formRecover].forEach((f) => (f.hidden = f !== formToShow));
  }

  document.getElementById("btn-show-register").addEventListener("click", () => showOnly(formRegister));
  document.getElementById("btn-show-login").addEventListener("click", () => showOnly(formLogin));
  document.getElementById("btn-show-recover").addEventListener("click", () => showOnly(formRecover));
  document.getElementById("btn-show-login-from-recover").addEventListener("click", () => showOnly(formLogin));

  const AUTH = {
    accessToken: storage.get("ww_access_token") || "",
    refreshToken: storage.get("ww_refresh_token") || "",
    username: storage.get("ww_remember_user") || "",
    // Ricordiamo cosa stiamo tentando ("login"/"register") mentre aspettiamo
    // la risposta del server, per sapere a chi appartiene un "Login|false|..."
    pendingUsername: "",

    // Chiamato ogni volta che il WebSocket si apre (prima connessione o dopo
    // un riconnect): se abbiamo già dei token salvati proviamo l'auto-login
    // invece di aspettare che l'utente riclicchi "Entra".
    onSocketOpen() {
      if (AUTH.accessToken && AUTH.refreshToken) {
        NET.send("AutoLogin", AUTH.accessToken, AUTH.refreshToken, langSelect.value || "it");
      }
    },

    saveTokens(accessToken, refreshToken) {
      AUTH.accessToken = accessToken;
      AUTH.refreshToken = refreshToken;
      storage.set("ww_access_token", accessToken);
      storage.set("ww_refresh_token", refreshToken);
    },

    clearTokens() {
      AUTH.accessToken = "";
      AUTH.refreshToken = "";
      storage.remove("ww_access_token");
      storage.remove("ww_refresh_token");
    },

    logout() {
      AUTH.clearTokens();
      storage.remove("ww_remember_user");
      screenGame.hidden = true;
      screenLogin.hidden = false;
      showOnly(formLogin);
    },
  };

  // L'access token è "base64(username|scadenza).base64(firma)" (vedi
  // TokenManager.GenerateAccessToken lato server): decodificando la prima
  // parte si ricava lo username senza doverlo ricordare a parte. Usato per
  // mostrare il nome giocatore anche dopo un AutoLogin (che non passa dal
  // form di login, quindi non c'è uno username "in corso" da usare).
  function usernameFromAccessToken(token) {
    if (!token || !token.includes(".")) return null;
    try {
      const payload = atob(token.split(".")[0]); // "username|scadenzaUnix"
      return payload.split("|")[0] || null;
    } catch (e) {
      return null;
    }
  }

  // "Login|true|accessToken|refreshToken" oppure "Login|false|motivo"
  // (il server risponde con lo stesso "Login|true|..." sia per un login
  // manuale sia per un AutoLogin riuscito).
  NET.on("Login", (args) => {
    const [ok, a, b] = args;
    if (ok === "true") {
      AUTH.saveTokens(a, b);
      const username = AUTH.pendingUsername || usernameFromAccessToken(a) || AUTH.username;
      document.getElementById("res-username").textContent = username;
      AUTH.username = username;

      if (chkRemember.checked) storage.set("ww_remember_user", AUTH.username);
      else storage.remove("ww_remember_user");

      loginStatus.hidden = true;
      enterGame();
    } else {
      const motivo = a || t("loginGenericError");
      loginStatus.hidden = false;
      loginStatus.classList.remove("login-status--ok");
      loginStatus.textContent = motivo;
    }
  });

  // Il server risponde "TOKEN_SCADUTO" quando l'access token è scaduto: si
  // richiede subito un token nuovo passando il refresh token, senza
  // disturbare l'utente con un nuovo login.
  NET.on("TOKEN_SCADUTO", () => {
    if (AUTH.refreshToken) NET.send("Refresh_Access_Token", AUTH.refreshToken);
    else AUTH.logout();
  });

  // Il server invalida la sessione (refresh token scaduto/revocato, player
  // non trovato, ecc.): non ha senso insistere, si torna al login.
  ["TOKEN_NON_VALIDO", "TOKEN_NON_VALIDO_PLAYER_NON_TROVATO"].forEach((comando) => {
    NET.on(comando, () => AUTH.logout());
  });

  // Risposta al refresh: il server (vedi ServerConnection.cs) manda
  // "Update_AccessToken|<token>". Aggiorniamo solo l'access token, il
  // refresh token resta quello già salvato.
  NET.on("Update_AccessToken", (args) => {
    if (args[0]) AUTH.saveTokens(args[0], AUTH.refreshToken);
  });

  formLogin.addEventListener("submit", (event) => {
    event.preventDefault();
    const data = new FormData(formLogin);
    const username = (data.get("username") || "").trim();
    const password = data.get("password") || "";
    const email = ""; // il Login non richiede l'email: il server la ricava dal player

    if (!username || !password) return;

    AUTH.pendingUsername = username;
    loginStatus.hidden = true;

    // Formato esatto usato dal client desktop (ComandiInvio.Login):
    // "Login|<access_token o vuoto>|username|password|lingua|email"
    const inviato = NET.send("Login", AUTH.accessToken, username, password, langSelect.value || "it", email);
    if (!inviato) {
      loginStatus.hidden = false;
      loginStatus.textContent = t("netNotConnectedYet");
    }
  });

  formRegister.addEventListener("submit", (event) => {
    event.preventDefault();
    const data = new FormData(formRegister);
    const username = (data.get("username") || "").trim();
    const email = (data.get("email") || "").trim();
    const password = data.get("password") || "";

    if (!username || !email || !password) return;

    AUTH.pendingUsername = username;

    // Formato esatto usato dal client desktop (ComandiInvio.NewGame):
    // "New Player|<access_token o vuoto>|username|password|lingua|email"
    NET.send("New Player", AUTH.accessToken, username, password, langSelect.value || "it", email);
  });

  formRecover.addEventListener("submit", (event) => {
    event.preventDefault();
    // TODO: comando "Reset Password|email" quando definito lato server
    // (non presente nell'elenco comandi individuato in ServerConnection.cs).
    recoverStatus.textContent = t("recoverSent");
    recoverStatus.hidden = false;
  });

  function enterGame() {
    screenLogin.hidden = true;
    screenGame.hidden = false;
  }

  /* ---------- 1b) LINGUA (i18n schermata login) ---------- */
  const I18N = {
    it: {
      username: "Nome utente",
      password: "Password",
      email: "Email",
      remember: "Ricordami (accesso automatico)",
      enter: "Entra",
      createAccount: "Crea un nuovo account",
      createAccountBtn: "Crea account",
      forgotPassword: "Password dimenticata?",
      backToLogin: "Torna al login",
      recoverHint: "Inserisci l'email associata al tuo account: ti invieremo un link per reimpostare la password.",
      sendLink: "Invia link di recupero",
      recoverSent: "Se l'indirizzo esiste, riceverai a breve un'email con le istruzioni.",
      netConnecting: "Connessione al server in corso…",
      netConnected: "Connesso al server.",
      netDisconnected: "Connessione al server persa, nuovo tentativo in corso…",
      netUnreachable: "Impossibile raggiungere il server.",
      netNotConnectedYet: "Non ancora connesso al server: riprova tra un istante.",
      loginGenericError: "Accesso non riuscito.",
    },
    en: {
      username: "Username",
      password: "Password",
      email: "Email",
      remember: "Remember me (auto sign-in)",
      enter: "Sign in",
      createAccount: "Create a new account",
      createAccountBtn: "Create account",
      forgotPassword: "Forgot password?",
      backToLogin: "Back to sign in",
      recoverHint: "Enter the email linked to your account: we'll send you a password reset link.",
      sendLink: "Send reset link",
      recoverSent: "If that address exists, you'll receive an email with instructions shortly.",
      netConnecting: "Connecting to the server…",
      netConnected: "Connected to the server.",
      netDisconnected: "Lost connection to the server, retrying…",
      netUnreachable: "Can't reach the server.",
      netNotConnectedYet: "Not connected to the server yet: try again in a moment.",
      loginGenericError: "Sign-in failed.",
    },
  };

  const langSelect = document.getElementById("lang-select");

  function t(key) {
    const lang = langSelect.value in I18N ? langSelect.value : "it";
    return I18N[lang][key] || I18N.it[key] || key;
  }

  function applyLanguage() {
    document.querySelectorAll("[data-i18n]").forEach((el) => {
      el.textContent = t(el.dataset.i18n);
    });
  }

  let savedLang = "it";
  savedLang = storage.get("ww_lang") || "it";
  langSelect.value = savedLang in I18N ? savedLang : "it";
  applyLanguage();

  langSelect.addEventListener("change", () => {
    storage.set("ww_lang", langSelect.value);
    applyLanguage();
  });

  /* ---------- 1c) TOGGLE RISORSE CIVILI / MILITARI ---------- */
  const btnToggleRisorse = document.getElementById("btn-toggle-risorse");
  const resGroupCivili = document.getElementById("res-group-civili");
  const resGroupMilitari = document.getElementById("res-group-militari");

  btnToggleRisorse.addEventListener("click", () => {
    const showingCivili = btnToggleRisorse.dataset.view === "civili";
    btnToggleRisorse.dataset.view = showingCivili ? "militari" : "civili";
    btnToggleRisorse.textContent = showingCivili ? "Militare" : "Civile";
    resGroupCivili.hidden = showingCivili;
    resGroupMilitari.hidden = !showingCivili;
  });

  /* ==========================================================
     3) NAVIGAZIONE
     ========================================================== */

  const tabButtons = document.querySelectorAll(".tab-bar__btn");
  const mainPanel = document.querySelector('[data-tab-panel="main"]');
  const costruzionePanel = document.querySelector('[data-tab-panel="costruzione"]');
  const cittaPanel = document.querySelector('[data-tab-panel="citta"]');
  const placeholderPanel = document.querySelector('[data-tab-panel="placeholder"]');
  const placeholderTitle = document.getElementById("placeholder-title");
  // Un tab-panel implementato per ogni voce qui dentro; le altre voci della
  // tab-bar (Negozio, Ricerca, ...) restano sul placeholder finché non
  // avranno anch'esse il loro protocollo/schermata dedicata.
  const tabPanels = { main: mainPanel, costruzione: costruzionePanel, citta: cittaPanel };

  tabButtons.forEach((btn) => {
    btn.addEventListener("click", () => {
      tabButtons.forEach((b) => b.classList.remove("is-active"));
      btn.classList.add("is-active");

      const tab = btn.dataset.tab;
      const panel = tabPanels[tab];
      Object.values(tabPanels).forEach((p) => { if (p) p.hidden = true; });
      if (panel) {
        panel.hidden = false;
        placeholderPanel.hidden = true;
      } else {
        placeholderPanel.hidden = false;
        placeholderTitle.textContent = btn.textContent;
      }
    });
  });

  const sectionToggleBtns = document.querySelectorAll("#main-panel-toggle .section-toggle__btn");
  const mainGridPanels = document.querySelectorAll(".main-grid [data-panel]");

  function showPanel(target) {
    mainGridPanels.forEach((p) => p.classList.toggle("is-visible", p.dataset.panel === target));
  }
  sectionToggleBtns.forEach((btn) => {
    btn.addEventListener("click", () => {
      sectionToggleBtns.forEach((b) => b.classList.remove("is-active"));
      btn.classList.add("is-active");
      showPanel(btn.dataset.panelTarget);
    });
  });
  showPanel("feudi");

  /* ==========================================================
     4) GAME — stato ricevuto dal server + dati di gioco
     ==========================================================
     GAME.raw accumula tutte le coppie chiave=valore ricevute via
     "Update_Data". Le chiavi qui sotto sono quelle vere, prese da
     PlayerSnapshot.BuildCurrentState lato server (il "tick" che il
     game loop manda ad ogni client connesso tramite
     ServerConnection.Update_Data(guid, player), non solo quelle
     "one time" del login): dalla porta 8444 arrivano quindi gli
     stessi identici valori che vede il client desktop.

     Nota sul formato dei numeri: il server serializza i valori con
     ToString("#,0"/"#,0.00"/ecc.) sotto cultura it-IT (impostata in
     Program.cs), quindi "." è il separatore delle migliaia e ","
     quello decimale (es. "30.000" = trentamila, "1.200,50" = milleduecento
     virgola cinquanta) — l'OPPOSTO della convenzione inglese. parseServerNumber
     qui sotto interpreta sempre i valori in arrivo come italiani, poi la UI li
     ri-formatta con Intl.NumberFormat nella lingua del dispositivo di chi
     gioca (vedi fmtInt/fmtDecimal in cima al file), così chi guarda da un
     paese anglosassone vede comunque "30,000" e non si confonde. */

  function parseServerNumber(str) {
    if (str === undefined || str === null) return 0;
    const pulito = String(str).replace(/%$/, "").trim().replace(/\./g, "").replace(",", ".");
    const n = Number(pulito);
    return Number.isNaN(n) ? 0 : n;
  }

  const GAME = {
    raw: Object.create(null),

    applyUpdateData(args) {
      args.forEach((coppia) => {
        const idx = coppia.indexOf("=");
        if (idx === -1) return;
        GAME.raw[coppia.slice(0, idx)] = coppia.slice(idx + 1);
      });
      renderAllFromServer();
    },

    // Legge una chiave grezza come numero (0 se non ancora arrivata dal server).
    num(chiave) {
      return parseServerNumber(GAME.raw[chiave]);
    },
  };

  function renderAllFromServer() {
    renderRisorseBar();
    renderFeudi();
    renderStruttureList("civili-list", struttureCivili);
    renderStruttureList("militari-list", struttureMilitari);
    renderStruttureList("caserme-list", caserme);
    renderUnita();
    renderVarie();
    renderStruttureListForm("costruzione-civili-list", struttureCivili);
    renderStruttureListForm("costruzione-militari-list", struttureMilitari);
    renderStruttureListForm("costruzione-caserme-list", caserme);
    renderUnitaForm();
    renderSbloccoUnita();
    renderCittaList();
  }

  // Costo del prossimo Feudo e code di costruzione/reclutamento in uso sul
  // totale disponibile: valori reali mandati dal server (rispettivamente
  // "costo_terreni_Virtuali" via Update_Data_OneTime al login, e
  // "Code_Costruzioni_Disponibili"/"Code_Costruzioni" — occupate/totali —
  // ad ogni tick tramite PlayerSnapshot), al posto dei numeri fissi che
  // c'erano nell'HTML del mockup.
  function renderVarie() {
    const elCosto = document.querySelector('[data-value="costo-feudo"]');
    if (elCosto) elCosto.textContent = fmtInt(GAME.num("costo_terreni_Virtuali"));

    const elCodeCostruzioni = document.querySelector('[data-value="code-costruzioni"]');
    if (elCodeCostruzioni)
      elCodeCostruzioni.textContent = `${fmtInt(GAME.num("Code_Costruzioni_Disponibili"))}/${fmtInt(GAME.num("Code_Costruzioni"))}`;

    const elCodeReclutamenti = document.querySelector('[data-value="code-reclutamenti"]');
    if (elCodeReclutamenti)
      elCodeReclutamenti.textContent = `${fmtInt(GAME.num("Code_Reclutamenti_Disponibili"))}/${fmtInt(GAME.num("Code_Reclutamenti"))}`;

    // Stessi due valori, mostrati anche nella schermata Costruzione.
    const elCodeCostruzioniForm = document.querySelector('[data-value="code-costruzioni-form"]');
    if (elCodeCostruzioniForm)
      elCodeCostruzioniForm.textContent = `${fmtInt(GAME.num("Code_Costruzioni_Disponibili"))}/${fmtInt(GAME.num("Code_Costruzioni"))}`;

    const elCodeReclutamentiForm = document.querySelector('[data-value="code-reclutamenti-form"]');
    if (elCodeReclutamentiForm)
      elCodeReclutamentiForm.textContent = `${fmtInt(GAME.num("Code_Reclutamenti_Disponibili"))}/${fmtInt(GAME.num("Code_Reclutamenti"))}`;

    // Tempo totale rimanente in coda (schermata Main, pannelli Strutture ed
    // Esercito): il server manda già una stringa pronta ("2h 0m 0s", vedi
    // BuildingManagerV2.Get_Total_Building_Time/UnitManagerV2.Get_Total_
    // Recruit_Time) — usiamo GAME.raw direttamente, senza passare da
    // GAME.num() che è pensato per i valori numerici. Riga e pulsante
    // "Velocizza" restano nascosti finché non c'è davvero tempo da mostrare
    // (niente "Tempo rimanente: 0h 0m 0s" quando non si sta costruendo/
    // addestrando nulla).
    aggiornaTempoECodaVelocizza("Tempo_Costruzione", "[data-tempo-costruzione-row]", "tempo-costruzione", "btn-toggle-velocizza-costruzione", "form-velocizza-costruzione");
    aggiornaTempoECodaVelocizza("Tempo_Reclutamento", "[data-tempo-reclutamento-row]", "tempo-reclutamento", "btn-toggle-velocizza-reclutamento", "form-velocizza-reclutamento");

    // Rapporti di scambio/velocizzazione: valori reali mandati una tantum al
    // login (Update_Data_OneTime → "D_Viola_D_Blu"/"Tributi_D_Viola"/
    // "Tempo_D_Blu" in ServerConnection.cs), non fissi in JS.
    const rapportoVB = fmtInt(GAME.num("D_Viola_D_Blu"));
    document.querySelectorAll('[data-value="ratio-viola-blu"]').forEach((el) => (el.textContent = rapportoVB));

    const rapportoTV = fmtInt(GAME.num("Tributi_D_Viola"));
    document.querySelectorAll('[data-value="ratio-tributi-viola"]').forEach((el) => (el.textContent = rapportoTV));

    const rapportoVelocizza = `${fmtInt(GAME.num("Tempo_D_Blu"))}s`;
    document
      .querySelectorAll('[data-value="ratio-velocizza-tempo"], [data-value="ratio-velocizza-tempo-2"]')
      .forEach((el) => (el.textContent = rapportoVelocizza));
  }

  // Stima se una stringa di tempo già formattata dal server ("2h 0m 0s")
  // rappresenta più di zero secondi, sommando tutti i numeri che contiene:
  // non serve un valore esatto, solo capire se mostrare o no la riga.
  function tempoMaggioreDiZero(str) {
    if (!str) return false;
    const numeri = str.match(/\d+/g);
    if (!numeri) return false;
    return numeri.some((n) => Number(n) > 0);
  }

  // Aggiorna la riga "Tempo rimanente" e mostra/nasconde riga + pulsante
  // "Velocizza" a seconda che ci sia davvero qualcosa in coda. Se il
  // pulsante viene nascosto mentre il suo mini-form era aperto, lo richiude
  // e azzera lo stepper (altrimenti resterebbe un form orfano visibile).
  function aggiornaTempoECodaVelocizza(chiaveGrezza, selettoreRiga, chiaveValore, idBottoneVelocizza, idFormVelocizza) {
    const testoTempo = GAME.raw[chiaveGrezza];
    const c_e_tempo = tempoMaggioreDiZero(testoTempo);

    const riga = document.querySelector(selettoreRiga);
    if (riga) {
      riga.hidden = !c_e_tempo;
      if (c_e_tempo) {
        const valoreEl = riga.querySelector(`[data-value="${chiaveValore}"]`);
        if (valoreEl) valoreEl.textContent = testoTempo;
      }
    }

    const bottoneVelocizza = document.getElementById(idBottoneVelocizza);
    if (bottoneVelocizza) bottoneVelocizza.hidden = !c_e_tempo;

    if (!c_e_tempo) {
      const form = document.getElementById(idFormVelocizza);
      if (form && !form.hidden) {
        form.hidden = true;
        const stepper = VELOCIZZA_STEPPERS[idBottoneVelocizza];
        if (stepper) stepper.set(0);
      }
    }
  }

  // Cronologia: il server manda "Log_Server|testo" per gli eventi di gioco,
  // dove "testo" usa una sintassi BBCode-like con tag colore ([tag]...[/tag])
  // e icone inline ([icon:nome]) — la stessa che il client desktop
  // interpreta in Strumenti/LogSupport.cs. Qui la replichiamo per il web:
  // stessa logica di parsing (un tag di chiusura qualsiasi torna al colore
  // di default, un tag sconosciuto viene ignorato), colori riadattati per
  // leggibilità su sfondo chiaro (pergamena) invece che sullo sfondo scuro
  // del client desktop.
  const LOG_COLORS = {
    default: "var(--ink)",
    black: "#000000",
    verde: "#2e7d32",
    rosso: "#8b0000",
    ferroScuro: "#32323c",
    verdeF: "#22502c",
    bluGotico: "#24345e",
    porporaReale: "#551e4b",
    acciaioBlu: "#3f5468",
    arancione: "#c04400",
    ocraDorata: "#9c6f0a",
    bluNotte: "#1a2a33",
    success: "#1e8f4e",
    warning: "#a16a00",
    error: "#a33f3f",
    cibo: "#a35b00",
    legno: "#6b4423",
    pietra: "#5f5f5f",
    ferro: "#5a5a63",
    oro: "#9c7a00",
    popolazione: "#3f5468",
    viola: "#7d3c98",
    blu: "#2874a6",
    info: "#1f6fa5",
    title: "#5a3a22",
    highlight: "#8a6d00",
    TerrenoComune: "#6b6b6b",
    TerrenoNoncomune: "#1e8e1e",
    TerrenoRaro: "#0056c7",
    TerrenoEpico: "#8a1cd6",
    TerrenoLeggendario: "#a68b00",
  };
  // Tag che nel client desktop indicano enfasi (titolo/evidenza): qui li
  // rendiamo anche in grassetto oltre che a colore, per farli risaltare
  // nello stesso modo.
  const LOG_BOLD_TAGS = new Set(["title", "highlight"]);

  // icon:nome -> file in assets/. Include anche i refusi che compaiono
  // davvero nei messaggi lato server (es. "icon:arcere", "icon:guerriero",
  // "icon:lancere" invece delle chiavi "ufficiali" arceri/guerrieri/lanceri):
  // nel client desktop restano senza immagine per la chiave mancante, qui
  // li mappiamo comunque così l'icona compare per davvero.
  const LOG_ICONS = {
    xp: "Exp_1.png",
    lv: "Livello_V2.png",
    cibo: "Grano_V2.png",
    legno: "Legna_V2.png",
    pietra: "Pietra_V2.png",
    ferro: "Ferro_V2.png",
    oro: "Oro_V2.png",
    popolazione: "Popolazione_V2.png",
    diamanteBlu: "DiamanteBlu_V2.png",
    diamanteViola: "DiamanteViola_V2.png",
    dollariVirtuali: "Tributi_V2.png",
    spade: "Spade_V2.png",
    lance: "Lance_V2.png",
    archi: "Archi_V2.png",
    scudi: "Scudi_V2.png",
    armature: "Armature_V2.png",
    frecce: "Frecce_V2.png",
    guerrieri: "Guerriero_V2.png",
    guerriero: "Guerriero_V2.png",
    lanceri: "Lanciere_V2.png",
    lancere: "Lanciere_V2.png",
    arceri: "Arciere_V2.png",
    arcere: "Arciere_V2.png",
    arciere: "Arciere_V2.png",
    catapulte: "Catapulta_V2.png",
    catapulta: "Catapulta_V2.png",
    fattoria: "Fattoria_V2.png",
    segheria: "Segheria_V2.png",
    cavaPietra: "CavaDiPietra_V2.png",
    minieraFerro: "MinieraFerro_V2.png",
    minieraOro: "MinieraOro_V2.png",
    case: "Abitazioni_V2.png",
    workshopSpade: "Workshop_Spade_V2.png",
    workshopLance: "Workshop_Lance_V2.png",
    workshopArchi: "Workshop_Archi_V2.png",
    workshopScudi: "Workshop_Scudi_V2.png",
    workshopArmture: "Workshop_Armature_V2.png",
    workshopFrecce: "Workshop_Frecce_V2.png",
    casermaGuerrieri: "Caserma_Guerieri_V2.png",
    casermaLanceri: "Caserma_Lanceri_V2.png",
    casermaArceri: "Caserma_Arcieri_V2.png",
    casermaCatapulte: "Caserma_Catapulte_V2.png",
    // "tempo", "usdt" e "scambio" sono gestiti a parte / senza asset, vedi sotto.
  };

  // Analizza il testo con la sintassi BBCode-like del server e restituisce
  // una lista di "segmenti" (testo colorato, o icona) da trasformare in DOM.
  function parseLogMessage(msg) {
    const segmenti = [];
    let i = 0;
    let coloreCorrente = LOG_COLORS.default;
    let grassettoCorrente = false;
    let testoCorrente = "";

    function flush() {
      if (testoCorrente.length > 0) {
        segmenti.push({ tipo: "testo", testo: testoCorrente, colore: coloreCorrente, grassetto: grassettoCorrente });
        testoCorrente = "";
      }
    }

    while (i < msg.length) {
      if (msg[i] === "[") {
        const closeIdx = msg.indexOf("]", i);
        if (closeIdx === -1) {
          // Tag non chiuso: trattalo come testo normale (stesso comportamento del client desktop).
          testoCorrente += msg[i];
          i++;
          continue;
        }
        flush();
        const tag = msg.slice(i + 1, closeIdx);
        if (tag.startsWith("/")) {
          coloreCorrente = LOG_COLORS.default;
          grassettoCorrente = false;
        } else if (tag.startsWith("icon:")) {
          const nome = tag.slice(5);
          if (nome === "tempo") {
            segmenti.push({ tipo: "icona-tempo" });
          } else if (LOG_ICONS[nome]) {
            segmenti.push({ tipo: "icona", file: LOG_ICONS[nome] });
          }
          // icona sconosciuta/senza asset: ignorata silenziosamente.
        } else if (Object.prototype.hasOwnProperty.call(LOG_COLORS, tag)) {
          coloreCorrente = LOG_COLORS[tag];
          grassettoCorrente = LOG_BOLD_TAGS.has(tag);
        }
        // Tag sconosciuto: ignorato, il testo prosegue con il colore corrente.
        i = closeIdx + 1;
      } else {
        testoCorrente += msg[i];
        i++;
      }
    }
    flush();
    return segmenti;
  }

  function renderLogSegments(segmenti) {
    const frag = document.createDocumentFragment();
    segmenti.forEach((seg) => {
      if (seg.tipo === "testo") {
        const span = document.createElement("span");
        span.textContent = seg.testo;
        span.style.color = seg.colore;
        if (seg.grassetto) span.style.fontWeight = "700";
        frag.appendChild(span);
      } else if (seg.tipo === "icona") {
        const img = document.createElement("img");
        img.src = `assets/${seg.file}`;
        img.alt = "";
        img.className = "log-icon";
        frag.appendChild(img);
      } else if (seg.tipo === "icona-tempo") {
        const span = document.createElement("span");
        span.className = "log-icon log-icon--tempo";
        frag.appendChild(span);
      }
    });
    return frag;
  }

  // Aggiunge una riga in cima alla Cronologia, con un tetto per non far
  // crescere il DOM all'infinito.
  const logBox = document.getElementById("log-box");
  function appendLog(testo) {
    if (!logBox || !testo) return;
    const vuoto = logBox.querySelector(".log-empty");
    if (vuoto) vuoto.remove();

    const riga = document.createElement("p");
    riga.className = "log-entry";
    riga.appendChild(renderLogSegments(parseLogMessage(testo)));
    logBox.prepend(riga);

    while (logBox.children.length > 50) logBox.removeChild(logBox.lastChild);
  }
  NET.on("Log_Server", (args) => appendLog(args.join("|")));

  // Barra risorse: chiave-locale (usata dall'HTML in data-value) -> chiave
  // esatta mandata dal server per il giocatore connesso.
  const RESOURCE_KEY_ALIASES = {
    cibo: "cibo",
    legno: "legna", // il server usa "legna", non "legno"
    pietra: "pietra",
    ferro: "ferro",
    oro: "oro",
    popolazione: "popolazione",
    spade: "spade",
    lance: "lance",
    archi: "archi",
    scudi: "scudi",
    armature: "armature",
    frecce: "frecce",
    diamantiBlu: "diamanti_blu",
    diamantiViola: "diamanti_viola",
    xp: "esperienza",
    livello: "livello",
  };

  function renderRisorseBar() {
    Object.keys(RESOURCE_KEY_ALIASES).forEach((chiaveLocale) => {
      const el = document.querySelector(`#resource-bar [data-value="${chiaveLocale}"]`);
      if (el) el.textContent = fmtInt(GAME.num(RESOURCE_KEY_ALIASES[chiaveLocale]));
    });
    // Tributi = "dollari_virtuali" lato server, mostrato con 10 decimali
    // (stessa precisione della produzione dei Feudi di rarità più bassa).
    const elTributi = document.querySelector('#resource-bar [data-value="tributi"]');
    if (elTributi) elTributi.textContent = fmtDecimal(GAME.num("dollari_virtuali"), 10);
  }

  // Feudi: il server manda solo il NUMERO posseduto per rarità (chiavi
  // "comune"/"noncomune"/"raro"/"epico"/"leggendario" — vedi
  // player.Terreno_* in PlayerSnapshot.cs), non un valore di tributi per
  // singolo feudo: il totale dei tributi generati è già in "dollari_virtuali",
  // mostrato nella barra risorse in alto.
  const feudi = [
    { nome: "Feudo Comune", chiave: "comune" },
    { nome: "Feudo Non Comune", chiave: "noncomune" },
    { nome: "Feudo Raro", chiave: "raro" },
    { nome: "Feudo Epico", chiave: "epico" },
    { nome: "Feudo Leggendario", chiave: "leggendario" },
  ];

  function renderFeudi() {
    const ul = document.getElementById("feudi-list");
    ul.innerHTML = feudi
      .map(
        (f) => `
      <li class="row-item">
        <span class="row-item__label">${f.nome}</span>
        <span class="row-item__value" title="Posseduti">${fmtInt(GAME.num(f.chiave))}</span>
      </li>`
      )
      .join("");
  }

  // "Acquista" (Feudi): comando "Costruzione_Terreni", nessun parametro
  // oltre al token — vedi ComandiInvio.Acquista_TerrenoVirtuale.
  const btnAcquistaFeudo = document.getElementById("btn-acquista-feudo");
  if (btnAcquistaFeudo) {
    btnAcquistaFeudo.addEventListener("click", () => {
      NET.send("Costruzione_Terreni", AUTH.accessToken);
    });
  }

  /* ==========================================================
     6) SCAMBIO E VELOCIZZAZIONE (Diamanti)
     ==========================================================
     Piccoli form "a comparsa" — stesso contenuto delle finestre dedicate
     del client desktop (Scambia_DiamantiViola.cs / Scambia_Tributi.cs /
     Velocizza.cs), qui inline sotto al pulsante che li apre. Uno stepper
     comune (creaStepperSemplice) gestisce +/- e tiene lo stato in memoria,
     visto che qui — a differenza della Costruzione — serve anche un
     "risultato" calcolato in tempo reale mentre si cambia la quantità.
     ========================================================== */

  function creaStepperSemplice(containerId, onChange) {
    const el = document.getElementById(containerId);
    if (!el) return null;
    const valueEl = el.querySelector(".qty-stepper__value");
    let valore = 0;
    function set(v) {
      valore = Math.max(0, v);
      valueEl.textContent = String(valore);
      onChange(valore);
    }
    el.querySelector(".qty-btn--minus").addEventListener("click", () => set(valore - 1));
    el.querySelector(".qty-btn--plus").addEventListener("click", () => set(valore + 1));
    return { get: () => valore, set };
  }

  // Mostra/nasconde un mini-form al click del pulsante che lo attiva,
  // azzerando lo stepper quando si richiude (per non lasciare in giro una
  // quantità "dimenticata" da un utilizzo precedente).
  function collegaToggleMiniForm(bottoneId, formId, stepper) {
    const bottone = document.getElementById(bottoneId);
    const form = document.getElementById(formId);
    if (!bottone || !form) return;
    bottone.addEventListener("click", () => {
      form.hidden = !form.hidden;
      if (form.hidden && stepper) stepper.set(0);
    });
  }

  // --- Scambia Diamanti Viola -> Diamanti Blu ---
  const previewScambiaVB = document.querySelector('[data-preview="scambia-viola-blu"]');
  const stepperScambiaVB = creaStepperSemplice("stepper-scambia-viola-blu", (valore) => {
    if (previewScambiaVB) previewScambiaVB.textContent = fmtInt(valore * GAME.num("D_Viola_D_Blu"));
  });
  collegaToggleMiniForm("btn-scambia-viola-blu", "form-scambia-viola-blu", stepperScambiaVB);
  const btnConfermaScambiaVB = document.getElementById("btn-conferma-scambia-viola-blu");
  if (btnConfermaScambiaVB && stepperScambiaVB) {
    btnConfermaScambiaVB.addEventListener("click", () => {
      const quantita = stepperScambiaVB.get();
      if (quantita <= 0) return;
      NET.send("Scambia_Diamanti", AUTH.accessToken, quantita);
      stepperScambiaVB.set(0);
      document.getElementById("form-scambia-viola-blu").hidden = true;
    });
  }

  // --- Scambia Tributi -> Diamanti Viola ---
  const previewScambiaTV = document.querySelector('[data-preview="scambia-tributi-viola"]');
  const stepperScambiaTV = creaStepperSemplice("stepper-scambia-tributi-viola", (valore) => {
    if (previewScambiaTV) previewScambiaTV.textContent = fmtInt(valore * GAME.num("Tributi_D_Viola"));
  });
  collegaToggleMiniForm("btn-scambia-tributi-viola", "form-scambia-tributi-viola", stepperScambiaTV);
  const btnConfermaScambiaTV = document.getElementById("btn-conferma-scambia-tributi-viola");
  if (btnConfermaScambiaTV && stepperScambiaTV) {
    btnConfermaScambiaTV.addEventListener("click", () => {
      const quantita = stepperScambiaTV.get();
      if (quantita <= 0) return;
      NET.send("Scambia_Tributi", AUTH.accessToken, quantita);
      stepperScambiaTV.set(0);
      document.getElementById("form-scambia-tributi-viola").hidden = true;
    });
  }

  // --- Velocizza con Diamanti Blu (Costruzione / Reclutamento) ---
  // Comando "Velocizza_Diamanti|token|<Costruzione|Reclutamento|Ricerca>|n"
  // — vedi case "Velocizza_Diamanti" in ServerConnection.cs. La Ricerca non
  // ha ancora una schermata web, quindi per ora colleghiamo solo le due
  // già esistenti. Gli stepper creati qui sono esposti in VELOCIZZA_STEPPERS
  // (chiave = idBottoneVelocizza) così aggiornaTempoECodaVelocizza() può
  // azzerarli davvero (stepper.set(0)) invece di limitarsi a "pulire" il
  // testo a video, lasciando lo stato interno (chiuso su "valore") disallineato.
  const VELOCIZZA_STEPPERS = Object.create(null);
  function collegaVelocizza(contesto, bottoneToggleId, formId, stepperId, bottoneConfermaId) {
    const stepper = creaStepperSemplice(stepperId, () => {});
    VELOCIZZA_STEPPERS[bottoneToggleId] = stepper;
    collegaToggleMiniForm(bottoneToggleId, formId, stepper);
    const btnConferma = document.getElementById(bottoneConfermaId);
    if (btnConferma && stepper) {
      btnConferma.addEventListener("click", () => {
        const quantita = stepper.get();
        if (quantita <= 0) return;
        NET.send("Velocizza_Diamanti", AUTH.accessToken, contesto, quantita);
        stepper.set(0);
        document.getElementById(formId).hidden = true;
      });
    }
  }
  collegaVelocizza("Costruzione", "btn-toggle-velocizza-costruzione", "form-velocizza-costruzione", "stepper-velocizza-costruzione", "btn-conferma-velocizza-costruzione");
  collegaVelocizza("Reclutamento", "btn-toggle-velocizza-reclutamento", "form-velocizza-reclutamento", "stepper-velocizza-reclutamento", "btn-conferma-velocizza-reclutamento");

  // Strutture Civili / Militari / Caserme: chiave "qta" (numero costruito) e
  // "coda" (in coda di costruzione), entrambe mandate dal server ad ogni
  // tick — vedi player.Fattoria/Segheria/... e buildingsQueue in
  // PlayerSnapshot.cs.
  // "tipoServer" è la stringa che il comando "Costruzione" si aspetta in
  // quella posizione (vedi ServerConnection.cs, case "Costruzione": 16
  // campi in quest'ordine esatto) — usata sia qui che nella schermata
  // Costruzione/Addestramento per costruire il messaggio da inviare.
  const struttureCivili = [
    { nome: "Fattoria", icona: "Fattoria_V2.png", qta: "fattorie", coda: "fattoria_coda", tipoServer: "Fattoria" },
    { nome: "Segheria", icona: "Segheria_V2.png", qta: "segherie", coda: "segheria_coda", tipoServer: "Segheria" },
    { nome: "Cava di Pietra", icona: "CavaDiPietra_V2.png", qta: "cave_pietra", coda: "cavapietra_coda", tipoServer: "CavaPietra" },
    { nome: "Miniera di Ferro", icona: "MinieraFerro_V2.png", qta: "miniere_ferro", coda: "minieraferro_coda", tipoServer: "MinieraFerro" },
    { nome: "Miniera d'Oro", icona: "MinieraOro_V2.png", qta: "miniere_oro", coda: "minieraoro_coda", tipoServer: "MinieraOro" },
    { nome: "Case", icona: "Abitazioni_V2.png", qta: "case", coda: "casa_coda", tipoServer: "Case" },
  ];

  const struttureMilitari = [
    { nome: "Workshop Spade", icona: "Workshop_Spade_V2.png", qta: "workshop_spade", coda: "workshop_spade_coda", tipoServer: "ProduzioneSpade" },
    { nome: "Workshop Lance", icona: "Workshop_Lance_V2.png", qta: "workshop_lance", coda: "workshop_lance_coda", tipoServer: "ProduzioneLance" },
    { nome: "Workshop Archi", icona: "Workshop_Archi_V2.png", qta: "workshop_archi", coda: "workshop_archi_coda", tipoServer: "ProduzioneArchi" },
    { nome: "Workshop Scudi", icona: "Workshop_Scudi_V2.png", qta: "workshop_scudi", coda: "workshop_scudi_coda", tipoServer: "ProduzioneScudi" },
    { nome: "Workshop Armature", icona: "Workshop_Armature_V2.png", qta: "workshop_armature", coda: "workshop_armature_coda", tipoServer: "ProduzioneArmature" },
    { nome: "Workshop Frecce", icona: "Workshop_Frecce_V2.png", qta: "workshop_frecce", coda: "workshop_frecce_coda", tipoServer: "ProduzioneFrecce" },
  ];

  const caserme = [
    { nome: "Caserma Guerrieri", icona: "Caserma_Guerieri_V2.png", qta: "caserma_guerrieri", coda: "caserma_guerrieri_coda", tipoServer: "CasermaGuerrieri" },
    { nome: "Caserma Lancieri", icona: "Caserma_Lanceri_V2.png", qta: "caserma_lanceri", coda: "caserma_lanceri_coda", tipoServer: "CasermaLanceri" },
    { nome: "Caserma Arcieri", icona: "Caserma_Arcieri_V2.png", qta: "caserma_arceri", coda: "caserma_arceri_coda", tipoServer: "CasermaArceri" },
    { nome: "Caserma Catapulte", icona: "Caserma_Catapulte_V2.png", qta: "caserma_catapulte", coda: "caserma_catapulte_coda", tipoServer: "CasermaCatapulte" },
  ];

  // Ordine ESATTO dei 16 campi richiesti dal comando "Costruzione" lato
  // server (msgArgs[3..18] in ServerConnection.cs). Deve restare in
  // quest'ordine: civili, poi militari, poi caserme.
  const COSTRUZIONE_ORDINE = [...struttureCivili, ...struttureMilitari, ...caserme];

  // Quantità da costruire scelte con lo stepper (+/-) per ogni riga, non un
  // campo di testo libero: più comodo su mobile e più simile allo stile del
  // client desktop. Chiave = tipoServer, valore = quantità in attesa di invio.
  const qtyCostruzione = Object.create(null);

  function setQtyCostruzione(tipoServer, valore) {
    qtyCostruzione[tipoServer] = Math.max(0, valore);
    const el = document.querySelector(`.qty-stepper[data-tipo-server="${tipoServer}"] .qty-stepper__value`);
    if (el) el.textContent = String(qtyCostruzione[tipoServer]);
  }

  // Invia UN SOLO comando "Costruzione" con tutte le 16 quantità (0 per le
  // righe non compilate) — stesso comportamento del pulsante "Costruzione"
  // nel client desktop.
  function inviaCostruzione() {
    const valori = COSTRUZIONE_ORDINE.map((s) => qtyCostruzione[s.tipoServer] || 0);
    if (valori.every((v) => v === 0)) return;
    NET.send("Costruzione", AUTH.accessToken, ...valori);
    // Azzera gli stepper dopo l'invio: il conteggio costruito/in-coda
    // arriverà aggiornato dal prossimo "Update_Data".
    COSTRUZIONE_ORDINE.forEach((s) => setQtyCostruzione(s.tipoServer, 0));
  }

  function renderStruttureList(listId, dati) {
    const ul = document.getElementById(listId);
    ul.innerHTML = dati
      .map(
        (s) => `
      <li class="row-item">
        <img src="assets/${s.icona}" alt="">
        <span class="row-item__label">${s.nome}</span>
        <span class="row-item__value" title="Costruite">${fmtInt(GAME.num(s.qta))}</span>
        <span class="row-item__queue" title="In coda di costruzione">${fmtInt(GAME.num(s.coda))}</span>
      </li>`
      )
      .join("");
  }

  const struttureToggleBtns = document.querySelectorAll("[data-strutture-view]");
  const struttureLists = document.querySelectorAll("[data-strutture-list]");
  const struttureTitle = document.getElementById("strutture-title");
  const struttureTitoli = { civili: "Strutture Civili", militari: "Strutture Militari", caserme: "Caserme" };

  struttureToggleBtns.forEach((btn) => {
    btn.addEventListener("click", () => {
      struttureToggleBtns.forEach((b) => b.classList.remove("is-active"));
      btn.classList.add("is-active");
      const view = btn.dataset.struttureView;
      struttureLists.forEach((ul) => (ul.hidden = ul.dataset.struttureList !== view));
      struttureTitle.textContent = struttureTitoli[view];
    });
  });

  // Esercito: il server manda una quantità e una coda PER TIER (1-5, chiavi
  // "guerrieri_1".."guerrieri_5", ecc. — vedi player.Guerrieri[]/Lanceri[]/
  // Arceri[]/Catapulte[] in PlayerSnapshot.cs), mentre il "massimo
  // addestrabile" (*_max) è per classe di unità, non per tier. La UI mostra
  // un tier alla volta: cambiare tab (I-V) rilegge le stesse chiavi con un
  // numero diverso, senza bisogno di dati separati per tier.
  let tierSelezionato = 1;
  const unita = [
    { nome: "Guerriero", icona: "Guerriero_V2.png", prefisso: "guerrieri", max: "guerrieri_max" },
    { nome: "Lanciere", icona: "Lanciere_V2.png", prefisso: "lanceri", max: "lanceri_max" },
    { nome: "Arciere", icona: "Arciere_V2.png", prefisso: "arceri", max: "arceri_max" },
    { nome: "Catapulta", icona: "Catapulta_V2.png", prefisso: "catapulte", max: "catapulte_max" },
  ];

  function renderUnita() {
    const ul = document.getElementById("unit-list");
    ul.innerHTML = unita
      .map((u) => {
        const qta = GAME.num(`${u.prefisso}_${tierSelezionato}`);
        const coda = GAME.num(`${u.prefisso}_${tierSelezionato}_coda`);
        const max = GAME.num(u.max);
        return `
      <li class="row-item">
        <img src="assets/${u.icona}" alt="">
        <span class="row-item__label">${u.nome}</span>
        <span class="row-item__value" title="Addestrate / limite Caserma">${fmtInt(qta)} / ${fmtInt(max)}</span>
        <span class="row-item__queue" title="In coda di addestramento">${fmtInt(coda)}</span>
      </li>`;
      })
      .join("");
  }

  // Tier tabs della schermata Main (Esercito): solo quelli dentro #tier-tabs,
  // per non interferire con #costruzione-tier-tabs (tier indipendente, vedi sotto).
  document.querySelectorAll("#tier-tabs .tier-btn").forEach((btn) => {
    btn.addEventListener("click", () => {
      document.querySelectorAll("#tier-tabs .tier-btn").forEach((b) => b.classList.remove("is-active"));
      btn.classList.add("is-active");
      tierSelezionato = Number(btn.dataset.tier) || 1;
      renderUnita();
    });
  });

  /* ==========================================================
     5) SCHERMATA COSTRUZIONE / ADDESTRAMENTO
     ==========================================================
     Stessa idea della schermata Main (liste costruite dai dati del
     server), ma ogni riga ha un input quantità e non c'è azione per
     riga: UN pulsante in fondo a ciascun pannello invia tutte le
     quantità in un colpo solo, esattamente come i comandi
     "Costruzione" (16 campi) e "Reclutamento" (4 campi + livello)
     accettati dal server (e come fa il client desktop).
     ========================================================== */

  // Struttura Civili/Militari/Caserme con uno stepper (−/+) per riga invece
  // di un campo di testo: si "osserva" solo il numero che si vuole
  // costruire, comodo anche da telefono. data-tipo-server identifica la
  // riga per inviaCostruzione()/setQtyCostruzione().
  function renderStruttureListForm(listId, dati) {
    const ul = document.getElementById(listId);
    if (!ul) return;
    if (ul.children.length !== dati.length) {
      ul.innerHTML = dati
        .map(
          (s) => `
        <li class="row-item row-item--form">
          <img src="assets/${s.icona}" alt="">
          <span class="row-item__label">${s.nome}</span>
          <span class="row-item__value" data-campo="qta" title="Costruite">${fmtInt(GAME.num(s.qta))}</span>
          <span class="row-item__queue" data-campo="coda" title="In coda di costruzione">${fmtInt(GAME.num(s.coda))}</span>
          <div class="qty-stepper" data-tipo-server="${s.tipoServer}">
            <button type="button" class="qty-btn qty-btn--minus" aria-label="Diminuisci quantità: ${s.nome}">−</button>
            <span class="qty-stepper__value">${qtyCostruzione[s.tipoServer] || 0}</span>
            <button type="button" class="qty-btn qty-btn--plus" aria-label="Aumenta quantità: ${s.nome}">+</button>
          </div>
        </li>`
        )
        .join("");
      return;
    }
    dati.forEach((s, i) => {
      const li = ul.children[i];
      li.querySelector('[data-campo="qta"]').textContent = fmtInt(GAME.num(s.qta));
      li.querySelector('[data-campo="coda"]').textContent = fmtInt(GAME.num(s.coda));
    });
  }

  // Delegazione eventi sui pulsanti +/− delle Strutture: un solo listener
  // per pannello invece di uno per bottone, così funziona anche sulle righe
  // ricostruite più tardi (es. al primo arrivo dei dati dal server).
  const costruzioneEdificiPanel = document.querySelector('[data-panel="costruzione-edifici"]');
  if (costruzioneEdificiPanel) {
    costruzioneEdificiPanel.addEventListener("click", (e) => {
      const btn = e.target.closest(".qty-btn");
      if (!btn) return;
      const stepper = btn.closest(".qty-stepper");
      const tipoServer = stepper.dataset.tipoServer;
      const attuale = qtyCostruzione[tipoServer] || 0;
      setQtyCostruzione(tipoServer, btn.classList.contains("qty-btn--plus") ? attuale + 1 : attuale - 1);
    });
  }

  const btnInviaCostruzione = document.getElementById("btn-invia-costruzione");
  if (btnInviaCostruzione) btnInviaCostruzione.addEventListener("click", inviaCostruzione);

  // Addestramento: stesso principio, ma per tier (indipendente dal tier
  // scelto nella schermata Main) e con l'ordine CORRETTO dei campi lato
  // server (Guerrieri, Lanceri, Arceri, Catapulte — vedi nota sotto).
  let tierCostruzione = 1;

  // Quantità da addestrare scelte con lo stepper, per unità (chiave =
  // prefisso). Si azzerano cambiando tier: le quantità in coda riguardano
  // solo il livello che si sta guardando in quel momento.
  const qtyReclutamento = Object.create(null);

  function setQtyReclutamento(prefisso, valore) {
    qtyReclutamento[prefisso] = Math.max(0, valore);
    const el = document.querySelector(`.qty-stepper[data-prefisso="${prefisso}"] .qty-stepper__value`);
    if (el) el.textContent = String(qtyReclutamento[prefisso]);
  }

  // NOTA: il client desktop (ComandiInvio.Addestramento) invia i parametri
  // nell'ordine Guerrieri/Arceri/Lanceri/Catapulte, ma il server (case
  // "Reclutamento" in ServerConnection.cs) li legge come Guerrieri/Lanceri/
  // Arceri/Catapulte: Arcieri e Lancieri sono scambiati nel client desktop
  // (bug esistente, da segnalare). Qui usiamo l'ordine che il server si
  // aspetta davvero, per non replicare il bug nel client web.
  function inviaReclutamento() {
    const [guerrieri, lanceri, arceri, catapulte] = unita.map((u) => qtyReclutamento[u.prefisso] || 0);
    if (!guerrieri && !lanceri && !arceri && !catapulte) return;
    NET.send("Reclutamento", AUTH.accessToken, tierCostruzione, guerrieri, lanceri, arceri, catapulte);
    unita.forEach((u) => setQtyReclutamento(u.prefisso, 0));
  }

  function renderUnitaForm() {
    const ul = document.getElementById("costruzione-unit-list");
    if (!ul) return;
    if (ul.children.length !== unita.length) {
      ul.innerHTML = unita
        .map(
          (u) => `
        <li class="row-item row-item--form">
          <img src="assets/${u.icona}" alt="">
          <span class="row-item__label">${u.nome}</span>
          <span class="row-item__value" data-campo="qta" title="Addestrate / limite Caserma">…</span>
          <span class="row-item__queue" data-campo="coda" title="In coda di addestramento">…</span>
          <div class="qty-stepper" data-prefisso="${u.prefisso}">
            <button type="button" class="qty-btn qty-btn--minus" aria-label="Diminuisci quantità: ${u.nome}">−</button>
            <span class="qty-stepper__value">${qtyReclutamento[u.prefisso] || 0}</span>
            <button type="button" class="qty-btn qty-btn--plus" aria-label="Aumenta quantità: ${u.nome}">+</button>
          </div>
        </li>`
        )
        .join("");
    }
    unita.forEach((u, i) => {
      const qta = GAME.num(`${u.prefisso}_${tierCostruzione}`);
      const coda = GAME.num(`${u.prefisso}_${tierCostruzione}_coda`);
      const max = GAME.num(u.max);
      const li = ul.children[i];
      li.querySelector('[data-campo="qta"]').textContent = `${fmtInt(qta)} / ${fmtInt(max)}`;
      li.querySelector('[data-campo="coda"]').textContent = fmtInt(coda);
    });
  }

  // Delegazione eventi per i pulsanti +/− dell'Addestramento (stesso
  // principio della Costruzione qui sopra).
  const costruzioneEsercitoPanel = document.querySelector('[data-panel="costruzione-esercito"]');
  if (costruzioneEsercitoPanel) {
    costruzioneEsercitoPanel.addEventListener("click", (e) => {
      const btn = e.target.closest(".qty-btn");
      if (!btn) return;
      const stepper = btn.closest(".qty-stepper");
      const prefisso = stepper.dataset.prefisso;
      const attuale = qtyReclutamento[prefisso] || 0;
      setQtyReclutamento(prefisso, btn.classList.contains("qty-btn--plus") ? attuale + 1 : attuale - 1);
    });
  }

  document.querySelectorAll("#costruzione-tier-tabs .tier-btn").forEach((btn) => {
    btn.addEventListener("click", () => {
      document.querySelectorAll("#costruzione-tier-tabs .tier-btn").forEach((b) => b.classList.remove("is-active"));
      btn.classList.add("is-active");
      tierCostruzione = Number(btn.dataset.tier) || 1;
      // Le quantità in attesa riguardavano il tier precedente: azzeriamo per
      // evitare di addestrare al tier sbagliato per errore.
      unita.forEach((u) => setQtyReclutamento(u.prefisso, 0));
      renderUnitaForm();
    });
  });

  const btnInviaReclutamento = document.getElementById("btn-invia-reclutamento");
  if (btnInviaReclutamento) btnInviaReclutamento.addEventListener("click", inviaReclutamento);

  // Livelli minimi di sblocco per addestrare II-V: valori VERI mandati dal
  // server (Update_Data_OneTime in ServerConnection.cs, chiavi
  // "Unlock_Truppe_II/III/IV/V" — presi da Variabili_Server.truppe_*), non
  // più fissi in JS. Mostrati sia in Main (Esercito) che in Addestramento.
  function renderSbloccoUnita() {
    const testo =
      `Sblocco unità — livello II: ${fmtInt(GAME.num("Unlock_Truppe_II"))} · ` +
      `III: ${fmtInt(GAME.num("Unlock_Truppe_III"))} · ` +
      `IV: ${fmtInt(GAME.num("Unlock_Truppe_IV"))} · ` +
      `V: ${fmtInt(GAME.num("Unlock_Truppe_V"))}`;
    document.querySelectorAll("[data-sblocco-unita]").forEach((el) => (el.textContent = testo));
  }

  // Toggle Edifici/Addestramento su mobile (stesso pattern di #main-panel-toggle).
  const costruzioneToggleBtns = document.querySelectorAll("#costruzione-panel-toggle .section-toggle__btn");
  const costruzioneGridPanels = document.querySelectorAll(".main-grid--costruzione [data-panel]");
  function showCostruzionePanel(target) {
    costruzioneGridPanels.forEach((p) => p.classList.toggle("is-visible", p.dataset.panel === target));
  }
  costruzioneToggleBtns.forEach((btn) => {
    btn.addEventListener("click", () => {
      costruzioneToggleBtns.forEach((b) => b.classList.remove("is-active"));
      btn.classList.add("is-active");
      showCostruzionePanel(btn.dataset.panelTarget);
    });
  });
  showCostruzionePanel("costruzione-edifici");

  /* ==========================================================
     7) SCHERMATA CITTÀ — strutture e spostamento truppe (Guarnigione)
     ==========================================================
     Ogni struttura (Ingresso, Cancello, Mura, Torri, Castello, Centro) ha
     una propria guarnigione, spostabile da/verso il Villaggio con il
     comando "SpostamentoTruppe|token|From|To|guerrieri|lanceri|arceri|
     catapulte|tier" (un tier alla volta — vedi SpostamentoTruppe in
     ServerConnection.cs). Nel client desktop il popup "Spostamento Truppe"
     permette però di preparare quantità su PIÙ tier contemporaneamente e
     inviarle tutte con un solo "Sposta": qui replichiamo lo stesso
     comportamento tenendo le quantità di ogni tier in memoria (invece di
     azzerarle cambiando tab) e inviando un comando per ogni tier che ha
     almeno un valore diverso da zero quando si preme "Sposta".
     "Centro" nell'interfaccia corrisponde alla chiave server "Citta".
     ========================================================== */

  const STRUTTURE_CITTA = [
    { chiave: "Ingresso", nome: "Ingresso", salute: false },
    { chiave: "Cancello", nome: "Cancello", salute: true },
    { chiave: "Mura", nome: "Mura", salute: true },
    { chiave: "Torri", nome: "Torri", salute: true },
    { chiave: "Castello", nome: "Castello", salute: true },
    { chiave: "Citta", nome: "Centro", salute: false },
  ];

  const UNITA_CITTA = [
    { nome: "Guerriero", icona: "Guerriero_V2.png", chiave: "g", prefissoVillaggio: "guerrieri", nomeServer: "Guerrieri" },
    { nome: "Lanciere", icona: "Lanciere_V2.png", chiave: "l", prefissoVillaggio: "lanceri", nomeServer: "Lanceri" },
    { nome: "Arciere", icona: "Arciere_V2.png", chiave: "a", prefissoVillaggio: "arceri", nomeServer: "Arceri" },
    { nome: "Catapulta", icona: "Catapulta_V2.png", chiave: "c", prefissoVillaggio: "catapulte", nomeServer: "Catapulte" },
  ];
  const TIER_LABELS = ["I", "II", "III", "IV", "V"];

  // Stato lato client per ogni struttura: tier attualmente mostrato,
  // direzione (verso la struttura o verso il villaggio) e le quantità in
  // attesa di invio per ciascun tier (si azzerano solo dopo un "Sposta"
  // riuscito o cambiando direzione — non cambiando semplicemente tab tier).
  const cittaStato = {};
  STRUTTURE_CITTA.forEach((s) => {
    cittaStato[s.chiave] = {
      tier: 1,
      direzione: "in", // "in" = Villaggio -> struttura, "out" = struttura -> Villaggio
      quantita: { 1: { g: 0, l: 0, a: 0, c: 0 }, 2: { g: 0, l: 0, a: 0, c: 0 }, 3: { g: 0, l: 0, a: 0, c: 0 }, 4: { g: 0, l: 0, a: 0, c: 0 }, 5: { g: 0, l: 0, a: 0, c: 0 } },
    };
  });

  function disponibiliCitta(u, s, direzione, tier) {
    return direzione === "in" ? GAME.num(`${u.prefissoVillaggio}_${tier}`) : GAME.num(`${u.nomeServer}_${tier}_${s.chiave}`);
  }

  function templateCittaCard(s) {
    const barre = s.salute
      ? `
      <div class="stat-bar stat-bar--hp" data-campo="salute"><div class="stat-bar__fill"></div><span class="stat-bar__label"></span></div>
      <div class="stat-bar stat-bar--def" data-campo="difesa"><div class="stat-bar__fill"></div><span class="stat-bar__label"></span></div>`
      : "";
    const tierBtns = TIER_LABELS.map((label, i) => `<button type="button" class="tier-btn${i === 0 ? " is-active" : ""}" data-tier="${i + 1}">${label}</button>`).join("");
    const unitRows = UNITA_CITTA.map(
      (u) => `
      <li class="row-item row-item--form">
        <img src="assets/${u.icona}" class="icon-inline" alt="">
        <span class="row-item__label">${u.nome}</span>
        <span class="row-item__value" data-disponibili="${u.chiave}" title="Disponibili per lo spostamento">0</span>
        <div class="qty-stepper" data-unit-stepper="${u.chiave}">
          <button type="button" class="qty-btn qty-btn--minus" aria-label="Diminuisci">−</button>
          <span class="qty-stepper__value">0</span>
          <button type="button" class="qty-btn qty-btn--plus" aria-label="Aumenta">+</button>
        </div>
      </li>`
    ).join("");

    return `
    <li class="city-card" data-struttura="${s.chiave}">
      <div class="city-card__header">
        <strong>${s.nome}</strong>
        <span class="row-item__value" data-campo="guarnigione">…</span>
      </div>
      ${barre}
      <button type="button" class="btn btn--ghost btn--block btn-toggle-guarnigione">Guarnigione</button>
      <div class="mini-form form-guarnigione" hidden>
        <div class="section-toggle section-toggle--inline direzione-toggle">
          <button type="button" class="section-toggle__btn is-active" data-direzione="in">Verso ${s.nome}</button>
          <button type="button" class="section-toggle__btn" data-direzione="out">Verso Villaggio</button>
        </div>
        <div class="tier-tabs">${tierBtns}</div>
        <ul class="unit-list unit-list--form">${unitRows}</ul>
        <p class="panel__hint pendenti-hint"></p>
        <button type="button" class="btn btn--primary btn--block btn-conferma-sposta">Sposta</button>
      </div>
    </li>`;
  }

  // Aggiorna i valori mostrati in una card: guarnigione, barre HP/DEF, e —
  // se il mini-form è aperto — gli stepper/disponibili del tier corrente.
  function aggiornaCittaCard(s) {
    const li = document.querySelector(`#city-list [data-struttura="${s.chiave}"]`);
    if (!li) return;
    const stato = cittaStato[s.chiave];

    const guarnEl = li.querySelector('[data-campo="guarnigione"]');
    if (guarnEl) guarnEl.textContent = `Guarnigione: ${fmtInt(GAME.num(`Guarnigione_${s.chiave}`))}/${fmtInt(GAME.num(`Guarnigione_${s.chiave}Max`))}`;

    if (s.salute) {
      const salute = GAME.num(`Salute_${s.chiave}`);
      const saluteMax = GAME.num(`Salute_${s.chiave}Max`);
      const difesa = GAME.num(`Difesa_${s.chiave}`);
      const difesaMax = GAME.num(`Difesa_${s.chiave}Max`);
      const barraHp = li.querySelector('[data-campo="salute"]');
      if (barraHp) {
        barraHp.querySelector(".stat-bar__fill").style.width = saluteMax > 0 ? `${Math.min(100, (salute / saluteMax) * 100)}%` : "0%";
        barraHp.querySelector(".stat-bar__label").textContent = `HP: ${fmtInt(salute)}/${fmtInt(saluteMax)}`;
      }
      const barraDef = li.querySelector('[data-campo="difesa"]');
      if (barraDef) {
        barraDef.querySelector(".stat-bar__fill").style.width = difesaMax > 0 ? `${Math.min(100, (difesa / difesaMax) * 100)}%` : "0%";
        barraDef.querySelector(".stat-bar__label").textContent = `DEF: ${fmtInt(difesa)}/${fmtInt(difesaMax)}`;
      }
    }

    UNITA_CITTA.forEach((u) => {
      const el = li.querySelector(`[data-disponibili="${u.chiave}"]`);
      if (el) el.textContent = fmtInt(disponibiliCitta(u, s, stato.direzione, stato.tier));
    });

    aggiornaPendentiHint(s);
  }

  // Riepilogo dei tier con quantità già impostate ma non ancora inviate,
  // per non perdersi cambiando tab (le quantità restano finché non si
  // preme "Sposta" o si cambia direzione).
  function aggiornaPendentiHint(s) {
    const li = document.querySelector(`#city-list [data-struttura="${s.chiave}"]`);
    const hintEl = li && li.querySelector(".pendenti-hint");
    if (!hintEl) return;
    const stato = cittaStato[s.chiave];
    const tierConValori = TIER_LABELS.map((label, i) => {
      const tier = i + 1;
      const q = stato.quantita[tier];
      return q.g + q.l + q.a + q.c > 0 ? label : null;
    }).filter(Boolean);
    hintEl.textContent = tierConValori.length > 0 ? `In attesa di invio: tier ${tierConValori.join(", ")}.` : "";
  }

  function renderCittaList() {
    const ul = document.getElementById("city-list");
    if (!ul) return;
    if (ul.children.length !== STRUTTURE_CITTA.length) {
      ul.innerHTML = STRUTTURE_CITTA.map(templateCittaCard).join("");
      collegaEventiCitta(ul);
    }
    STRUTTURE_CITTA.forEach(aggiornaCittaCard);
  }

  // Un solo listener delegato sull'intera lista invece di uno per
  // bottone/card: più semplice da mantenere e funziona anche se le card
  // vengono ricostruite (es. al primo arrivo dei dati dal server).
  function collegaEventiCitta(ul) {
    ul.addEventListener("click", (e) => {
      const card = e.target.closest(".city-card");
      if (!card) return;
      const chiave = card.dataset.struttura;
      const s = STRUTTURE_CITTA.find((x) => x.chiave === chiave);
      const stato = cittaStato[chiave];

      if (e.target.closest(".btn-toggle-guarnigione")) {
        const form = card.querySelector(".form-guarnigione");
        form.hidden = !form.hidden;
        return;
      }

      const btnDirezione = e.target.closest("[data-direzione]");
      if (btnDirezione) {
        stato.direzione = btnDirezione.dataset.direzione;
        card.querySelectorAll("[data-direzione]").forEach((b) => b.classList.toggle("is-active", b === btnDirezione));
        // Cambiare direzione azzera le quantità in attesa: "sposta 5 verso
        // la struttura" e "sposta 5 verso il villaggio" sono due operazioni
        // diverse, non ha senso mantenere il numero cambiando intenzione.
        TIER_LABELS.forEach((_, i) => (stato.quantita[i + 1] = { g: 0, l: 0, a: 0, c: 0 }));
        aggiornaStepperVisibili(card, s, stato);
        aggiornaCittaCard(s);
        return;
      }

      const btnTier = e.target.closest(".tier-tabs .tier-btn");
      if (btnTier) {
        stato.tier = Number(btnTier.dataset.tier) || 1;
        card.querySelectorAll(".tier-tabs .tier-btn").forEach((b) => b.classList.toggle("is-active", b === btnTier));
        aggiornaStepperVisibili(card, s, stato);
        aggiornaCittaCard(s);
        return;
      }

      const btnQty = e.target.closest(".qty-btn");
      if (btnQty) {
        const stepperEl = btnQty.closest("[data-unit-stepper]");
        const chiaveUnita = stepperEl.dataset.unitStepper;
        const q = stato.quantita[stato.tier];
        q[chiaveUnita] = Math.max(0, q[chiaveUnita] + (btnQty.classList.contains("qty-btn--plus") ? 1 : -1));
        stepperEl.querySelector(".qty-stepper__value").textContent = String(q[chiaveUnita]);
        aggiornaPendentiHint(s);
        return;
      }

      if (e.target.closest(".btn-conferma-sposta")) {
        inviaSpostamentoTruppe(s, stato);
      }
    });
  }

  // Rimette a video i valori (stepper + disponibili) del tier/direzione
  // correnti dopo un cambio tab o direzione.
  function aggiornaStepperVisibili(card, s, stato) {
    const q = stato.quantita[stato.tier];
    UNITA_CITTA.forEach((u) => {
      const stepperEl = card.querySelector(`[data-unit-stepper="${u.chiave}"] .qty-stepper__value`);
      if (stepperEl) stepperEl.textContent = String(q[u.chiave]);
    });
  }

  // Invia UN comando "SpostamentoTruppe" per ogni tier che ha almeno una
  // quantità diversa da zero (così più tier vengono spostati "in un colpo
  // solo" dal punto di vista dell'utente, anche se il protocollo accetta un
  // tier alla volta) — vedi nota di apertura sezione.
  function inviaSpostamentoTruppe(s, stato) {
    const from = stato.direzione === "in" ? "Esercito Villaggio" : s.chiave;
    const to = stato.direzione === "in" ? s.chiave : "Esercito Villaggio";
    let inviato = false;
    TIER_LABELS.forEach((_, i) => {
      const tier = i + 1;
      const q = stato.quantita[tier];
      if (q.g + q.l + q.a + q.c === 0) return;
      NET.send("SpostamentoTruppe", AUTH.accessToken, from, to, q.g, q.l, q.a, q.c, tier);
      inviato = true;
      stato.quantita[tier] = { g: 0, l: 0, a: 0, c: 0 };
    });
    if (!inviato) return;
    const card = document.querySelector(`#city-list [data-struttura="${s.chiave}"]`);
    if (card) {
      aggiornaStepperVisibili(card, s, stato);
      card.querySelector(".form-guarnigione").hidden = true;
    }
    aggiornaCittaCard(s);
  }

  renderAllFromServer();

  /* ---------- Avvio ----------
     Si tenta subito la connessione al server: se c'è già un accesso
     "ricordato" (token salvati), AUTH.onSocketOpen() farà l'auto-login non
     appena il WebSocket è aperto, senza passare dal form. Se la
     connessione fallisce, NET va comunque in retry automatico (vedi
     scheduleReconnect) e l'utente resta sulla schermata di login con
     l'indicatore di stato. */
  if (storage.get("ww_remember_user")) {
    document.getElementById("res-username").textContent = storage.get("ww_remember_user");
  }
  NET.connect();
})();
