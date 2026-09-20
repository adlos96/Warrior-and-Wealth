/* ==========================================================
   Warrior & Wealth — Web Client — Localizzazione.js
   ----------------------------------------------------------
   Dizionario I18N, in un file a sé (separato da 02-auth.js) per poter
   controllare facilmente quali chiavi esistono e se hanno sia "it" che
   "en", senza cercarlo in mezzo al codice di login/registrazione.
   Stesso identico dizionario/funzione WW.t()/attributi data-i18n usati
   da tutte le altre schermate. In 02-auth.js i riferimenti locali
   diventano "WW.t(...)" e "WW.langSelect" (vedi commento lì).

   PERCHÉ QUESTO DIZIONARIO RESTA CLIENT-ONLY (a differenza del resto
   della localizzazione, ormai quasi tutta lato server con il
   meccanismo "Descrizione|Label X|..." — vedi WW.descrizioni/
   WW.onDescrizione in 04-game-main.js): la schermata di Login deve
   essere leggibile PRIMA che qualunque comando raggiunga il server
   (il server impara la lingua del giocatore solo dal comando stesso
   di Login/AutoLogin/New Player — vedi Lingua() in
   ServerConnection.cs), quindi non può dipendere da una Descrizione
   che arriva solo DOPO essersi autenticati. Per lo stesso motivo (le
   stringhe non sono differenziate tra schermate) qui sotto trovi anche
   le chiavi comuni a Panoramica/Costruzione/Ricerca/Città che non
   hanno ancora (o non avranno mai) una Label server equivalente.

   COME AGGIUNGERE UNA LINGUA (oggi supportate: "it", "en"):
   1. Aggiungi un nuovo blocco "xx: { ... }" qui sotto con TUTTE le
      stesse chiavi degli altri due (usa lo script di controllo nel
      punto 2 per trovare quelle mancanti).
   2. Aggiungi "xx" come <option> nel <select id="lang-select"> in
      index.html (schermata di Login).
   3. Lato server: il player.Lingua che arriva dal login viene
      validato contro Variabili_Server.lingue_Supportate (vedi
      Variabili_Server.cs) — aggiungi "xx" anche lì, altrimenti il
      server declassa sempre a "it" (vedi Lingua() in
      ServerConnection.cs). Serve anche una nuova classe ILocalization
      (es. FRA.cs sul modello di ITA.cs/ENG.cs) registrata in
      LocalizationManager._lingue.
   Le due liste di chiavi qui sotto DEVONO restare identiche (stesso
   set esatto in "it" ed "en", e in ogni lingua futura): una chiave
   presente in una sola lingua non dà errore, applyLanguage() ricade
   silenziosamente sul fallback italiano (vedi t() più sotto) — un
   typo qui non si vede a schermo, resta solo il testo italiano.

   Dipende da: WW.storage (00-core.js). Deve caricare PRIMA di ogni
   altro file che usa WW.t/WW.tFormat/WW.onLanguageChange (oggi:
   02-auth.js e a cascata quasi tutti gli altri) — vedi l'ordine degli
   script in index.html. ========================================================== */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  /* Ogni chiave qui sotto DEVE comparire in ENTRAMBI i blocchi "it" ed
     "en" (vedi nota sul controllo di completezza in cima al file). */
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
      recoverHint: "Inserisci nome utente ed email del tuo account: ti invieremo un codice per reimpostare la password.",
      sendLink: "Invia codice",
      recoverSent: "Se i dati sono corretti, riceverai a breve un'email con il codice.",
      recoverCode: "Codice ricevuto via email",
      recoverNewPassword: "Nuova password",
      recoverChangeBtn: "Cambia password",
      recoverFillAll: "Compila tutti i campi.",
      recoverDone: "Password cambiata. Ora puoi accedere.",
      recoverCodeInvalid: "Codice non valido o scaduto.",
      recoverPasswordChars: "I campi non possono contenere il carattere |.",
      netConnecting: "Connessione al server in corso…",
      netConnected: "Connesso al server.",
      netDisconnected: "Connessione al server persa, nuovo tentativo in corso…",
      netUnreachable: "Impossibile raggiungere il server.",
      netNotConnectedYet: "Non ancora connesso al server: riprova tra un istante.",
      loginGenericError: "Accesso non riuscito.",

      // Comuni a più schermate (Panoramica + Costruzione)
      edifici: "Edifici",
      costruisci: "Costruisci",
      recluta: "Recluta",
      tempoRimanente: "Tempo rimanente:",
      velocizza: "Velocizza",
      descrizioneNonRicevuta: "Descrizione non ancora ricevuta dal server (arriva subito dopo il login).",
      sbloccoUnitaPrefix: "Sblocco unità — livello",
      decreaseQty: "Diminuisci quantità:",
      increaseQty: "Aumenta quantità:",
      descriptionAria: "Descrizione",
      builtTooltip: "Costruite",
      queuedBuildTooltip: "In coda di costruzione",
      trainedLimitTooltip: "Addestrate / limite Caserma",
      queuedTrainTooltip: "In coda di addestramento",

      // Tab bar
      tabPanoramica: "Panoramica",

      // Barra risorse
      resCivile: "Civile",
      resMilitare: "Militare",

      // Toggle pannelli Panoramica (Feudi/Strutture/Esercito/Cronologia/Messaggi)
      navFeudi: "Feudi",
      navStrutture: "Strutture",
      navEsercito: "Esercito",
      navCronologia: "Cronologia",
      navMessaggi: "Messaggi",

      // Pannello Feudi
      feudiInfoAria: "Informazioni sui Feudi",
      acquista: "Acquista",
      scambioValute: "Scambio valute",
      scambia: "Scambia",
      scambiaTributi: "Scambia Tributi",
      exchangeBlueSuffix: "diamanti blu per ogni diamante viola.",
      exchangeVioletSuffix: "diamanti viola per ogni tributo.",

      // Pannello Strutture Civili/Militari/Caserme
      costruttori: "Costruttori:",
      civili: "Civili",
      militari: "Militari",
      caserme: "Caserme",
      velocizzaCostruzioneBtn: "Velocizza Costruzione",
      reduceTimeByPrefix: "Ogni diamante blu riduce il tempo di",

      // Pannello Esercito
      reclutatori: "Reclutatori:",
      velocizzaAddestramentoBtn: "Velocizza Addestramento",

      // Cronologia / Messaggi
      svuotaCronologiaAria: "Svuota cronologia",
      nessunEventoRecente: "Nessun evento recente.",
      paginaPrecedenteAria: "Pagina precedente",
      paginaSuccessivaAria: "Pagina successiva",
      indietro: "‹ Indietro",
      avanti: "Avanti ›",
      paginaDi: "Pagina {0} di {1}",
      messaggiPlaceholder: "Questa schermata sarà completata quando saranno definiti i comandi del protocollo lato server.",

      // Pannello Ricerca — "navEsercito"/"velocizza"/"reduceTimeByPrefix"/
      // "descrizioneNonRicevuta"/"descriptionAria" sopra sono riusate anche
      // qui, stessa parola in più schermate.
      ricercaHint: "La Ricerca rappresenta il progresso delle conoscenze del tuo regno. Investendo tempo e risorse potrai sbloccare nuove possibilità, migliorare strutture, eserciti e strategie.",
      tempoRicercaPrefix: "Tempo Ricerca:",
      navGenerali: "Generali",
      navCitta: "Città",
      velocizzaRicercaBtn: "Velocizza Ricerca",
      ricercaBtn: "Ricerca",
      livelloAttualeAria: "Livello attuale",

      // Pannello Città — nomi strutture/unità e "Ripara"/
      // "Salute"/"Difesa"/"Guarnigione" arrivano dal server (WW.descrizioni,
      // stesse Label già usate in Ricerca), qui solo il testo fisso rimasto.
      cittaHint: "Sposta le truppe tra il Villaggio e ogni struttura per rinforzarne la guarnigione.",
      riparaTuttoBtn: "Ripara Tutto",
      riparazioneInCorsoHint: "Riparazione in corso su una o più strutture.",
      fermaRiparazioniBtn: "Ferma tutte le riparazioni",
      versoPrefix: "Verso",
      villaggio: "Villaggio",
      disponibiliSpostamento: "Disponibili per lo spostamento",
      spostaBtn: "Sposta",
      truppeNonDisponibiliAvviso: "Truppe non disponibili: la quantità è stata corretta.",
      inAttesaInvioTierPrefix: "In attesa di invio: tier {0}.",
      stratoDifensivoPrefix: "Strato difensivo {0} di 6",
      struttureDanneggiatePrefix: "{0} strutture danneggiate",
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
      recoverHint: "Enter your username and the email linked to your account: we'll send you a code to reset your password.",
      sendLink: "Send code",
      recoverSent: "If the details are correct, you'll receive an email with the code shortly.",
      recoverCode: "Code received by email",
      recoverNewPassword: "New password",
      recoverChangeBtn: "Change password",
      recoverFillAll: "Please fill in all fields.",
      recoverDone: "Password changed. You can now sign in.",
      recoverCodeInvalid: "Invalid or expired code.",
      recoverPasswordChars: "Fields can't contain the | character.",
      netConnecting: "Connecting to the server…",
      netConnected: "Connected to the server.",
      netDisconnected: "Lost connection to the server, retrying…",
      netUnreachable: "Can't reach the server.",
      netNotConnectedYet: "Not connected to the server yet: try again in a moment.",
      loginGenericError: "Sign-in failed.",

      edifici: "Buildings",
      costruisci: "Build",
      recluta: "Recruit",
      tempoRimanente: "Time remaining:",
      velocizza: "Speed up",
      descrizioneNonRicevuta: "Description not received from the server yet (arrives right after login).",
      sbloccoUnitaPrefix: "Unit unlock — level",
      decreaseQty: "Decrease quantity:",
      increaseQty: "Increase quantity:",
      descriptionAria: "Description",
      builtTooltip: "Built",
      queuedBuildTooltip: "Queued for construction",
      trainedLimitTooltip: "Trained / barracks limit",
      queuedTrainTooltip: "Queued for training",

      tabPanoramica: "Overview",

      resCivile: "Civilian",
      resMilitare: "Military",

      navFeudi: "Strongholds",
      navStrutture: "Buildings",
      navEsercito: "Army",
      navCronologia: "History",
      navMessaggi: "Messages",

      feudiInfoAria: "Stronghold information",
      acquista: "Buy",
      scambioValute: "Currency exchange",
      scambia: "Exchange",
      scambiaTributi: "Exchange Tributes",
      exchangeBlueSuffix: "blue diamonds for each violet diamond.",
      exchangeVioletSuffix: "violet diamonds for each tribute.",

      costruttori: "Builders:",
      civili: "Civilian",
      militari: "Military",
      caserme: "Barracks",
      velocizzaCostruzioneBtn: "Speed Up Construction",
      reduceTimeByPrefix: "Every blue diamond reduces the time by",

      reclutatori: "Recruiters:",
      velocizzaAddestramentoBtn: "Speed Up Training",

      svuotaCronologiaAria: "Clear history",
      nessunEventoRecente: "No recent events.",
      paginaPrecedenteAria: "Previous page",
      paginaSuccessivaAria: "Next page",
      indietro: "‹ Back",
      avanti: "Next ›",
      paginaDi: "Page {0} of {1}",
      messaggiPlaceholder: "This screen will be completed once the server-side protocol commands are defined.",

      ricercaHint: "Research represents your kingdom's progress in knowledge. By investing time and resources you can unlock new possibilities and improve buildings, armies and strategies.",
      tempoRicercaPrefix: "Research Time:",
      navGenerali: "General",
      navCitta: "City",
      velocizzaRicercaBtn: "Speed Up Research",
      ricercaBtn: "Research",
      livelloAttualeAria: "Current level",

      cittaHint: "Move troops between the Village and each structure to reinforce its garrison.",
      riparaTuttoBtn: "Repair All",
      riparazioneInCorsoHint: "Repair in progress on one or more structures.",
      fermaRiparazioniBtn: "Stop all repairs",
      versoPrefix: "To",
      villaggio: "Village",
      disponibiliSpostamento: "Available to move",
      spostaBtn: "Move",
      truppeNonDisponibiliAvviso: "Troops not available: the quantity was adjusted.",
      inAttesaInvioTierPrefix: "Waiting to send: tier {0}.",
      stratoDifensivoPrefix: "Defensive layer {0} of 6",
      struttureDanneggiatePrefix: "{0} damaged structures",
    },
  };

  const langSelect = document.getElementById("lang-select");

  function t(key) {
    const lang = langSelect.value in I18N ? langSelect.value : "it";
    return I18N[lang][key] || I18N.it[key] || key;
  }

  // Sostituisce {0}, {1}, ... in una stringa I18N con gli argomenti dati
  // (es. t("paginaDi") = "Pagina {0} di {1}" -> tFormat("paginaDi", 2, 5)).
  function tFormat(key, ...args) {
    return args.reduce((s, v, i) => s.replace(`{${i}}`, v), t(key));
  }

  // Elementi che non bastano un textContent (bottone icona senza testo
  // visibile: title/aria-label sono l'unica etichetta) — stesso dizionario,
  // attributo diverso. "languageListeners" copre invece il testo generato
  // da JS che non è un elemento statico con data-i18n (es. il pulsante
  // Civile/Militare che si aggiorna in base allo stato, o "Pagina X di Y").
  const languageListeners = [];

  function applyLanguage() {
    document.querySelectorAll("[data-i18n]").forEach((el) => {
      el.textContent = t(el.dataset.i18n);
    });
    document.querySelectorAll("[data-i18n-title]").forEach((el) => {
      const valore = t(el.dataset.i18nTitle);
      el.title = valore;
      el.setAttribute("aria-label", valore);
    });
    document.querySelectorAll("[data-i18n-aria]").forEach((el) => {
      el.setAttribute("aria-label", t(el.dataset.i18nAria));
    });
    languageListeners.forEach((fn) => fn());
  }

  let savedLang = "it";
  savedLang = WW.storage.get("ww_lang") || "it";
  langSelect.value = savedLang in I18N ? savedLang : "it";
  applyLanguage();

  langSelect.addEventListener("change", () => {
    WW.storage.set("ww_lang", langSelect.value);
    applyLanguage();
  });

  WW.t = t;
  WW.tFormat = tFormat;
  WW.onLanguageChange = (fn) => languageListeners.push(fn);
  WW.langSelect = langSelect;
  // Esposto anche il dizionario grezzo — utile per un controllo rapido in
  // console, es.:
  //   Object.keys(WW.I18N.it).filter(k => !(k in WW.I18N.en))
  // elenca le chiavi presenti in italiano ma non (ancora) in inglese, e
  // viceversa scambiando it/en.
  WW.I18N = I18N;
})(window.WW);