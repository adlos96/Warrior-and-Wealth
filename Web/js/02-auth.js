/* ==========================================================
   Warrior & Wealth — Web Client — 02-auth.js
   ----------------------------------------------------------
   AUTH — login / registrazione / auto-login / token, più la
   lingua (i18n schermata login) e il toggle risorse civili/
   militari della barra in alto.

   Dipende da: WW.storage (00-core.js), WW.NET (01-net.js, già
   caricato — qui usiamo WW.NET.on/send a "livello top" quindi
   l'ordine di caricamento conta davvero). ========================================================== */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

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
    accessToken: WW.storage.get("ww_access_token") || "",
    refreshToken: WW.storage.get("ww_refresh_token") || "",
    username: WW.storage.get("ww_remember_user") || "",
    // Ricordiamo cosa stiamo tentando ("login"/"register") mentre aspettiamo
    // la risposta del server, per sapere a chi appartiene un "Login|false|..."
    pendingUsername: "",

    // Chiamato ogni volta che il WebSocket si apre (prima connessione o dopo
    // un riconnect): se abbiamo già dei token salvati proviamo l'auto-login
    // invece di aspettare che l'utente riclicchi "Entra".
    onSocketOpen() {
      if (AUTH.accessToken && AUTH.refreshToken) {
        WW.NET.send("AutoLogin", AUTH.accessToken, AUTH.refreshToken, WW.langSelect.value || "it");
      }
    },

    saveTokens(accessToken, refreshToken) {
      AUTH.accessToken = accessToken;
      AUTH.refreshToken = refreshToken;
      WW.storage.set("ww_access_token", accessToken);
      WW.storage.set("ww_refresh_token", refreshToken);
    },

    clearTokens() {
      AUTH.accessToken = "";
      AUTH.refreshToken = "";
      WW.storage.remove("ww_access_token");
      WW.storage.remove("ww_refresh_token");
    },

    logout() {
      AUTH.clearTokens();
      WW.storage.remove("ww_remember_user");
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
  WW.NET.on("Login", (args) => {
    const [ok, a, b] = args;
    if (ok === "true") {
      AUTH.saveTokens(a, b);
      const username = AUTH.pendingUsername || usernameFromAccessToken(a) || AUTH.username;
      document.getElementById("res-username").textContent = username;
      AUTH.username = username;

      if (chkRemember.checked) WW.storage.set("ww_remember_user", AUTH.username);
      else WW.storage.remove("ww_remember_user");

      loginStatus.hidden = true;
      enterGame();
    } else {
      const motivo = a || WW.t("loginGenericError");
      loginStatus.hidden = false;
      loginStatus.classList.remove("login-status--ok");
      loginStatus.textContent = motivo;
    }
  });

  // Il server risponde "TOKEN_SCADUTO" quando l'access token è scaduto: si
  // richiede subito un token nuovo passando il refresh token, senza
  // disturbare l'utente con un nuovo login.
  WW.NET.on("TOKEN_SCADUTO", () => {
    if (AUTH.refreshToken) WW.NET.send("Refresh_Access_Token", AUTH.refreshToken);
    else AUTH.logout();
  });

  // Il server invalida la sessione (refresh token scaduto/revocato, player
  // non trovato, ecc.): non ha senso insistere, si torna al login.
  ["TOKEN_NON_VALIDO", "TOKEN_NON_VALIDO_PLAYER_NON_TROVATO"].forEach((comando) => {
    WW.NET.on(comando, () => AUTH.logout());
  });

  // Risposta al refresh: il server (vedi ServerConnection.cs) manda
  // "Update_AccessToken|<token>". Aggiorniamo solo l'access token, il
  // refresh token resta quello già salvato.
  WW.NET.on("Update_AccessToken", (args) => {
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
    const inviato = WW.NET.send("Login", AUTH.accessToken, username, password, WW.langSelect.value || "it", email);
    if (!inviato) {
      loginStatus.hidden = false;
      loginStatus.textContent = WW.t("netNotConnectedYet");
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
    WW.NET.send("New Player", AUTH.accessToken, username, password, WW.langSelect.value || "it", email);
  });

  /* ---------- RECUPERO PASSWORD (2 passi) ----------
     Server (Cambia_Password): "Reset Password|username|codice|nuovaPassword|email|modalita"
     modalita = "mail" (il server invia il codice via email) oppure "Change"
     (codice + nuova password). Risposta al passo 2: "Password change|Change|true/false". */
  const recoverStep2 = document.getElementById("recover-step-2");
  const recoverUsernameEl = document.getElementById("recover-username");
  const recoverEmailEl = document.getElementById("recover-email");
  const recoverCodeEl = document.getElementById("recover-code");
  const recoverNewPassEl = document.getElementById("recover-newpass");
  const recoverSubmitBtn = document.getElementById("btn-recover-submit");

  let recoverUsername = "";
  let recoverEmail = "";

  function setRecoverStatus(testo) {
    recoverStatus.textContent = testo;
    recoverStatus.hidden = false;
  }

  function setRecoverSubmitLabel(chiave) {
    recoverSubmitBtn.dataset.i18n = chiave; // resta corretto anche al cambio lingua
    recoverSubmitBtn.textContent = WW.t(chiave);
  }

  function resetRecover() {
    formRecover.reset();
    recoverStep2.hidden = true;
    recoverStatus.hidden = true;
    recoverUsername = "";
    recoverEmail = "";
    setRecoverSubmitLabel("sendLink");
  }

  document.getElementById("btn-show-recover").addEventListener("click", resetRecover);

  formRecover.addEventListener("submit", (event) => {
    event.preventDefault();

    if (recoverStep2.hidden) {
      // Passo 1: richiesta del codice via email
      recoverUsername = recoverUsernameEl.value.trim();
      recoverEmail = recoverEmailEl.value.trim();
      if (!recoverUsername || !recoverEmail) { setRecoverStatus(WW.t("recoverFillAll")); return; }
      if (recoverUsername.includes("|") || recoverEmail.includes("|")) { setRecoverStatus(WW.t("recoverPasswordChars")); return; }

      if (!WW.NET.send("Reset Password", recoverUsername, "", "", recoverEmail, "mail")) {
        setRecoverStatus(WW.t("netNotConnectedYet"));
        return;
      }
      recoverStep2.hidden = false;
      setRecoverSubmitLabel("recoverChangeBtn");
      setRecoverStatus(WW.t("recoverSent"));
    } else {
      // Passo 2: codice + nuova password
      const codice = recoverCodeEl.value.trim();
      const nuova = recoverNewPassEl.value;
      if (!codice || !nuova) { setRecoverStatus(WW.t("recoverFillAll")); return; }
      if (nuova.includes("|")) { setRecoverStatus(WW.t("recoverPasswordChars")); return; } // "|" romperebbe il protocollo

      if (!WW.NET.send("Reset Password", recoverUsername, codice, nuova, recoverEmail, "Change")) {
        setRecoverStatus(WW.t("netNotConnectedYet"));
      }
    }
  });

  WW.NET.on("Password change", (args) => {
    const [fase, esito] = args;
    if (fase !== "Change") return; // "mail": il passo 2 e' gia' visibile
    if (esito === "true") {
      setRecoverStatus(WW.t("recoverDone"));
      setTimeout(() => { resetRecover(); showOnly(formLogin); }, 1500);
    } else {
      setRecoverStatus(WW.t("recoverCodeInvalid"));
    }
  });

  function enterGame() {
    screenLogin.hidden = true;
    screenGame.hidden = false;
  }

  /* ---------- LINGUA (i18n) ----------
     Dizionario I18N e funzioni t()/tFormat()/applyLanguage() vivono in
     Localizzazione.js (caricato subito prima di questo file, vedi
     index.html); qui restano solo i punti che le usano: WW.t(...) e
     WW.langSelect.value (vedi AUTH.onSocketOpen/login/register sopra). */

  /* ---------- TOGGLE RISORSE CIVILI / MILITARI ---------- */
  const btnToggleRisorse = document.getElementById("btn-toggle-risorse");
  const resGroupCivili = document.getElementById("res-group-civili");
  const resGroupMilitari = document.getElementById("res-group-militari");

  // Testo del pulsante in base allo stato corrente (civili/militari
  // mostrati): estratto in una funzione perché va rieseguito anche al
  // cambio lingua (vedi WW.onLanguageChange, esportato da
  // Localizzazione.js), non solo al click.
  function aggiornaTestoToggleRisorse() {
    const showingCivili = btnToggleRisorse.dataset.view === "civili";
    btnToggleRisorse.textContent = showingCivili ? WW.t("resMilitare") : WW.t("resCivile");
  }

  btnToggleRisorse.addEventListener("click", () => {
    const showingCivili = btnToggleRisorse.dataset.view === "civili";
    btnToggleRisorse.dataset.view = showingCivili ? "militari" : "civili";
    resGroupCivili.hidden = showingCivili;
    resGroupMilitari.hidden = !showingCivili;
    aggiornaTestoToggleRisorse();
  });
  aggiornaTestoToggleRisorse();
  WW.onLanguageChange(aggiornaTestoToggleRisorse);

  /* ---------- Menu giocatore ----------
     Cliccando su nome/avatar nella barra risorse si apre un popup:
     "Cambio giocatore" riporta al login riusando AUTH.logout() (stessa
     funzione chiamata per TOKEN_NON_VALIDO). "Cambio immagine profilo" è
     solo un segnaposto disabilitato in HTML: non ancora supportato lato
     server. Usa #player-menu-overlay, lo stesso overlay generico
     .modal-overlay/.modal-box di Feudi/Info Risorsa/Resoconto (vedi
     04-game-main.js/14-battaglia.js per lo stesso pattern apri/chiudi/
     click-fuori/Escape) — non un dropdown ancorato al pulsante, perché
     dentro la barra risorse (sticky, overflow-x:auto) comparirebbe
     schiacciato invece che sopra a tutto. */
  const btnPlayerMenu = document.getElementById("btn-player-menu");
  const playerMenuOverlay = document.getElementById("player-menu-overlay");
  const btnChiudiPlayerMenu = document.getElementById("btn-chiudi-player-menu");
  const btnCambioGiocatore = document.getElementById("player-menu-cambio-giocatore");

  if (btnPlayerMenu && playerMenuOverlay) {
    btnPlayerMenu.addEventListener("click", () => { playerMenuOverlay.hidden = false; });
    btnChiudiPlayerMenu.addEventListener("click", () => { playerMenuOverlay.hidden = true; });
    // Click sullo sfondo scuro (non sul box) chiude il popup, come un normale modale.
    playerMenuOverlay.addEventListener("click", (e) => {
      if (e.target === playerMenuOverlay) playerMenuOverlay.hidden = true;
    });
    document.addEventListener("keydown", (e) => {
      if (e.key === "Escape" && !playerMenuOverlay.hidden) playerMenuOverlay.hidden = true;
    });

    btnCambioGiocatore.addEventListener("click", () => {
      playerMenuOverlay.hidden = true;
      AUTH.logout();
    });
  }

  WW.AUTH = AUTH;
  WW.screenLogin = screenLogin;
  WW.loginStatus = loginStatus;
})(window.WW);