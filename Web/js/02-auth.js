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

  formRecover.addEventListener("submit", (event) => {
    event.preventDefault();
    // TODO: comando "Reset Password|email" quando definito lato server
    // (non presente nell'elenco comandi individuato in ServerConnection.cs).
    recoverStatus.textContent = WW.t("recoverSent");
    recoverStatus.hidden = false;
  });

  function enterGame() {
    screenLogin.hidden = true;
    screenGame.hidden = false;
  }

  /* ---------- LINGUA (i18n) ----------
     18/09/2026, su richiesta dell'utente: il dizionario I18N/le funzioni
     t()/tFormat()/applyLanguage() e il riferimento a #lang-select sono
     stati spostati in Localizzazione.js (caricato subito prima di questo
     file, vedi index.html) per poterli controllare/estendere senza dover
     cercarli in mezzo al codice di login/registrazione. Qui restano solo
     i pochi punti che li usavano: WW.t(...) al posto di t(...), e
     WW.langSelect.value al posto di langSelect.value (vedi sopra in
     AUTH.onSocketOpen/login/register). Nessuna nuova chiave o dizionario:
     stesso identico WW.t()/WW.onLanguageChange di sempre, solo spostati. */

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

  /* ---------- Menu giocatore (14/09/2026, su richiesta dell'utente) ----------
     Cliccando su nome/avatar nella barra risorse si apre un popup:
     "Cambio giocatore" riporta alla schermata di login/registrazione
     riusando AUTH.logout() (stessa funzione già chiamata per
     TOKEN_NON_VALIDO — pulisce i token salvati e mostra di nuovo il form di
     login, coerente col resto del client). "Cambio immagine profilo" è per
     ora solo un segnaposto disabilitato in HTML: la funzione non esiste
     ancora lato server.
     Prima era un dropdown ancorato al pulsante (position:absolute dentro
     .resource-bar__player-wrap), ma dentro alla barra risorse — sticky, con
     overflow-x:auto — compariva schiacciato lì sotto invece che sopra a
     tutto (segnalato dall'utente: "compare sotto... nella stessa barra").
     Ora è #player-menu-overlay, lo stesso overlay generico .modal-overlay/
     .modal-box già usato per Feudi/Info Risorsa/Resoconto (vedi
     04-game-main.js/14-battaglia.js per lo stesso identico pattern
     apri/chiudi/click-fuori/Escape). */
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

  // WW.t/WW.tFormat/WW.onLanguageChange (18/09/2026): esportati da
  // Localizzazione.js, non più da qui — vedi commento sopra a "LINGUA (i18n)".
  WW.AUTH = AUTH;
  WW.screenLogin = screenLogin;
  WW.loginStatus = loginStatus;
})(window.WW);
