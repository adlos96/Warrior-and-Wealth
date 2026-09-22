/* ==========================================================
   Warrior & Wealth — Web Client — 16-raduni.js
   ----------------------------------------------------------
   PANNELLO RADUNI (22/09/2026) — dentro la schermata PVP/PVE
   (index.html, quinto pannello del toggle #battaglia-panel-toggle).
   Backend già completo e testato (Gioco/Raduni.cs, classe
   AttacchiCooperativi — il nome della classe non è stato rinominato
   per non allargare il diff, solo il protocollo sul wire usa
   "Raduno"): crea un raduno, partecipa con le proprie truppe
   (tier I-V, come "Esercito da Inviare" in 14-battaglia.js ma un
   contributo A PARTE, non l'esercito da battaglia singola),
   abbandona, e solo il creatore può avviarlo.

   Protocollo (ServerConnection.cs/Raduni.cs):
   - "Raduno|token|Crea|<Barbaro|PVP>|<bersaglio>|<alleanza true/false>"
     (esteso il 22/09/2026: <bersaglio> è il livello 1-20 per "Barbaro",
     lo username del giocatore per "PVP")
   - "Raduno|token|Partecipa|<idAttacco>|<G1..G5>|<L1..L5>|<A1..A5>|<C1..C5>" (20 valori truppe)
   - "Raduno|token|Abbandona|<idAttacco>"
   - "Raduno|token|Inizia|<idAttacco>" (solo il creatore)
   - Risposte/errori: "Log_Server|<messaggio>" — già mostrato in
     Cronologia da WW.NET.on("Log_Server", ...) in 04-game-main.js,
     nessuna gestione dedicata serve qui.
   - Stato "live": pacchetto JSON {"Type":"RaduniUpdate","Aperti":[...],
     "MiePartecipazioni":[...]} rimandato ad ogni tick da Update_Data
     (ServerConnection.cs) a TUTTI i giocatori connessi — stesso
     principio del resto della UI: non serve richiedere esplicitamente
     "Raduno|Lista"/"Raduno|MieiAttacchi", il pacchetto arriva da solo e
     la UI si aggiorna a ogni ricezione. Ogni riga porta anche
     "TipoBersaglio" ("Barbaro"/"PVP") e, per i raduni PVP,
     "BersaglioUsername" (LivelloTarget resta 0/non significativo per
     quelli PVP). "Aperti" è già filtrato lato server secondo l'Opzione B
     (un raduno Alleanza=true è visibile solo al suo creatore, finché non
     esiste un vero sistema di alleanze).
   - Lista giocatori selezionabili per un raduno PVP: stessa lista del
     pannello Battaglia PVP singolo (14-battaglia.js), esposta su
     WW.pvpListaGiocatori invece di registrare un secondo handler su
     "Update_PVP_Player" (WW.NET.on ne accetta uno solo per comando).

   Dipende da: WW.GAME/WW.NET/WW.AUTH (01-net.js/02-auth.js/04-game-main.js),
   WW.fmtInt/WW.qtyStepDeltaVelocizza/WW.cssEscape (00-core.js),
   WW.pvpListaGiocatori (14-battaglia.js, per il selettore bersaglio PVP).
   Esporta:
   WW.renderRaduni — chiamata da renderAllFromServer in 04-game-main.js,
   stesso pattern lazy-build-poi-refresh di WW.renderBattaglia
   (14-battaglia.js): la UI statica (tier tabs, listener) si costruisce
   una sola volta al primo giro. */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  const TIER_LABELS = ["I", "II", "III", "IV", "V"];
  const UNITA = [
    { nome: "Guerriero", icona: "Guerriero_V2.png", chiave: "g", campoServer: "guerrieri" },
    { nome: "Lanciere", icona: "Lanciere_V2.png", chiave: "l", campoServer: "lanceri" },
    { nome: "Arciere", icona: "Arciere_V2.png", chiave: "a", campoServer: "arceri" },
    { nome: "Catapulta", icona: "Catapulta_V2.png", chiave: "c", campoServer: "catapulte" },
  ];

  let aperti = []; // ultimo "Aperti" ricevuto da RaduniUpdate
  let mie = []; // ultimo "MiePartecipazioni" ricevuto da RaduniUpdate
  let idInJoin = null; // id del raduno per cui è aperto il form truppe (null = form chiuso)
  let tipoBersaglioCrea = "Barbaro"; // tab attiva nel form "Crea raduno" ("Barbaro" | "PVP")

  // Truppe da contribuire al raduno selezionato — stesso pattern "tier
  // persistenti tra i tab" di 14-battaglia.js, ma stato a parte: qui si
  // azzera a ogni "Conferma"/"Annulla", non resta impostato tra un raduno
  // e l'altro (a differenza dell'Esercito da Inviare per Barbari/PVP).
  const truppe = {
    tier: 1,
    quantita: { 1: { g: 0, l: 0, a: 0, c: 0 }, 2: { g: 0, l: 0, a: 0, c: 0 }, 3: { g: 0, l: 0, a: 0, c: 0 }, 4: { g: 0, l: 0, a: 0, c: 0 }, 5: { g: 0, l: 0, a: 0, c: 0 } },
  };

  function truppeTotale() {
    return Object.values(truppe.quantita).reduce((tot, q) => tot + q.g + q.l + q.a + q.c, 0);
  }

  // Ordine richiesto dal server (Raduni.GestisciComando, case "Partecipa"): G1-5, L1-5, A1-5, C1-5.
  function truppeArgs() {
    const per = (chiave) => [1, 2, 3, 4, 5].map((t) => truppe.quantita[t][chiave]);
    return [...per("g"), ...per("l"), ...per("a"), ...per("c")];
  }

  function azzeraTruppe() {
    TIER_LABELS.forEach((_, i) => (truppe.quantita[i + 1] = { g: 0, l: 0, a: 0, c: 0 }));
    aggiornaStepperVisibili();
    aggiornaBottoneConferma();
  }

  function aggiornaStepperVisibili() {
    const q = truppe.quantita[truppe.tier];
    UNITA.forEach((u) => {
      const el = document.querySelector(`#raduno-esercito-list [data-unit-stepper="${u.chiave}"] .qty-stepper__value`);
      if (el) el.textContent = String(q[u.chiave]);
    });
  }

  function aggiornaEsercitoDisponibili() {
    const lista = document.getElementById("raduno-esercito-list");
    if (!lista) return;
    UNITA.forEach((u) => {
      const el = lista.querySelector(`[data-disponibili="${u.chiave}"]`);
      if (el) el.textContent = WW.fmtInt(WW.GAME.num(`${u.campoServer}_${truppe.tier}`));
    });
  }

  function aggiornaBottoneConferma() {
    const btn = document.getElementById("btn-conferma-partecipa-raduno");
    if (btn) btn.disabled = truppeTotale() === 0;
  }

  function templateEsercitoRow(u) {
    return `
    <li class="row-item row-item--form">
      <img src="assets/${u.icona}" class="icon-inline" alt="">
      <span class="row-item__label">${u.nome}</span>
      <span class="row-item__value" data-disponibili="${u.chiave}" title="Disponibili">0</span>
      <div class="qty-stepper" data-unit-stepper="${u.chiave}">
        <button type="button" class="qty-btn qty-btn--minus" aria-label="Diminuisci">−</button>
        <span class="qty-stepper__value">0</span>
        <button type="button" class="qty-btn qty-btn--plus" aria-label="Aumenta">+</button>
      </div>
    </li>`;
  }

  function costruisciTruppeUI() {
    const tabs = document.getElementById("raduno-tier-tabs");
    const lista = document.getElementById("raduno-esercito-list");
    if (!tabs || !lista || lista.children.length === UNITA.length) return;

    tabs.innerHTML = TIER_LABELS.map((l, i) => `<button type="button" class="tier-btn${i === 0 ? " is-active" : ""}" data-tier="${i + 1}">${l}</button>`).join("");
    lista.innerHTML = UNITA.map(templateEsercitoRow).join("");

    tabs.addEventListener("click", (e) => {
      const btn = e.target.closest(".tier-btn");
      if (!btn) return;
      truppe.tier = Number(btn.dataset.tier) || 1;
      tabs.querySelectorAll(".tier-btn").forEach((b) => b.classList.toggle("is-active", b === btn));
      aggiornaStepperVisibili();
      aggiornaEsercitoDisponibili();
    });

    lista.addEventListener("click", (e) => {
      const btnQty = e.target.closest(".qty-btn");
      if (!btnQty) return;
      const stepperEl = btnQty.closest("[data-unit-stepper]");
      const chiave = stepperEl.dataset.unitStepper;
      const q = truppe.quantita[truppe.tier];
      const passo = WW.qtyStepDelta(e);
      q[chiave] = Math.max(0, q[chiave] + (btnQty.classList.contains("qty-btn--plus") ? passo : -passo));
      stepperEl.querySelector(".qty-stepper__value").textContent = String(q[chiave]);
      aggiornaBottoneConferma();
    });
  }

  /* ---------------------------------------------------------------
     Form "Partecipa" — apertura/chiusura/conferma
     --------------------------------------------------------------- */

  function apriFormPartecipa(id) {
    idInJoin = id;
    azzeraTruppe();
    aggiornaEsercitoDisponibili();

    const raduno = aperti.find((r) => String(r.Id) === String(id));
    const titolo = document.getElementById("raduno-form-truppe-titolo");
    if (titolo) {
      titolo.textContent = raduno
        ? `Truppe da inviare al raduno #${id} — creato da ${raduno.Creatore}, bersaglio ${descrizioneBersaglio(raduno)}.`
        : `Truppe da inviare al raduno #${id}.`;
    }
    const form = document.getElementById("raduno-form-truppe");
    if (form) {
      form.hidden = false;
      form.scrollIntoView({ behavior: "smooth", block: "nearest" });
    }
  }

  function chiudiFormPartecipa() {
    idInJoin = null;
    const form = document.getElementById("raduno-form-truppe");
    if (form) form.hidden = true;
  }

  function confermaPartecipa() {
    if (!idInJoin || truppeTotale() === 0) return;
    WW.NET.send("Raduno", WW.AUTH.accessToken, "Partecipa", idInJoin, ...truppeArgs());
    chiudiFormPartecipa();
  }

  /* ---------------------------------------------------------------
     Crea / Abbandona / Avvia
     --------------------------------------------------------------- */

  function creaRaduno() {
    const alleanzaInput = document.getElementById("raduno-crea-alleanza");
    const alleanza = !!(alleanzaInput && alleanzaInput.checked);

    if (tipoBersaglioCrea === "PVP") {
      const selectGiocatore = document.getElementById("raduno-crea-giocatore");
      const bersaglio = selectGiocatore && selectGiocatore.value;
      if (!bersaglio) return; // nessun giocatore selezionabile (select vuota/disabilitata)
      WW.NET.send("Raduno", WW.AUTH.accessToken, "Crea", "PVP", bersaglio, alleanza);
    } else {
      const livelloInput = document.getElementById("raduno-crea-livello");
      const livello = Math.max(1, Math.min(20, Number((livelloInput && livelloInput.value) || 1)));
      WW.NET.send("Raduno", WW.AUTH.accessToken, "Crea", "Barbaro", livello, alleanza);
    }
    if (alleanzaInput) alleanzaInput.checked = false;
  }

  // Popola il <select> bersaglio-giocatore da WW.pvpListaGiocatori (stringhe "username, Livello: X,
  // Potenza: Y" — stesso formato di Server.cs/Utenti_PVP): il valore inviato al server è SOLO lo
  // username (prima virgola), il testo mostrato è la stringa completa.
  function aggiornaSelectGiocatore() {
    const select = document.getElementById("raduno-crea-giocatore");
    if (!select) return;
    const lista = WW.pvpListaGiocatori || [];
    const valorePrecedente = select.value;
    select.innerHTML = lista.length > 0
      ? lista.map((u) => `<option value="${u.split(",")[0]}">${u}</option>`).join("")
      : `<option value="">Nessun giocatore disponibile</option>`;
    const usernames = lista.map((u) => u.split(",")[0]);
    if (usernames.includes(valorePrecedente)) select.value = valorePrecedente;
  }

  function selezionaTipoBersaglioCrea(tipo) {
    tipoBersaglioCrea = tipo === "PVP" ? "PVP" : "Barbaro";
    const tabs = document.getElementById("raduno-crea-tipo");
    if (tabs) tabs.querySelectorAll(".tier-btn").forEach((b) => b.classList.toggle("is-active", b.dataset.tipo === tipoBersaglioCrea));
    const wrapLivello = document.getElementById("raduno-crea-livello-wrap");
    const wrapGiocatore = document.getElementById("raduno-crea-giocatore-wrap");
    if (wrapLivello) wrapLivello.hidden = tipoBersaglioCrea === "PVP";
    if (wrapGiocatore) wrapGiocatore.hidden = tipoBersaglioCrea !== "PVP";
    if (tipoBersaglioCrea === "PVP") aggiornaSelectGiocatore();
  }

  function abbandonaRaduno(id) {
    WW.NET.send("Raduno", WW.AUTH.accessToken, "Abbandona", id);
  }

  function avviaRaduno(id) {
    WW.NET.send("Raduno", WW.AUTH.accessToken, "Inizia", id);
  }

  /* ---------------------------------------------------------------
     Render liste
     --------------------------------------------------------------- */

  function templateApertoRow(r) {
    const sonoCreatore = r.Creatore === WW.AUTH.username;
    const giaPartecipo = mie.some((m) => String(m.Id) === String(r.Id));
    return `
    <li class="research-item raduno-item" data-id="${WW.cssEscape(String(r.Id))}">
      <div class="raduno-item__info">
        <strong>#${r.Id}</strong> — ${descrizioneBersaglio(r)} — creato da ${r.Creatore}${sonoCreatore ? " (tu)" : ""}
        ${r.Alleanza ? `<span class="raduno-item__badge">Alleanza</span>` : ""}
        ${r.TipoBersaglio === "PVP" ? `<span class="raduno-item__badge">PVP</span>` : ""}
        <br><span class="panel__hint">Partecipanti: ${r.Partecipanti} — Tempo rimanente: ${r.MinutiRimanenti} min</span>
      </div>
      <button type="button" class="btn btn--ghost btn--partecipa-raduno" data-id="${WW.cssEscape(String(r.Id))}"${giaPartecipo ? " disabled" : ""}>${giaPartecipo ? "Già dentro" : "Partecipa"}</button>
    </li>`;
  }

  // Etichetta bersaglio comune alle due liste (aperti/mie): "Città Barbaro Lv.X" oppure, per i raduni
  // PVP (22/09/2026), "Giocatore <username>".
  function descrizioneBersaglio(r) {
    return r.TipoBersaglio === "PVP" ? `Giocatore ${r.BersaglioUsername}` : `Città Barbaro Lv.${r.LivelloTarget}`;
  }

  function templateMiaRow(m) {
    const sonoCreatore = m.Creatore === WW.AUTH.username;
    const totale = m.Guerrieri + m.Lanceri + m.Arceri + m.Catapulte;
    const azioni = m.AttaccoInCorso
      ? `<span class="panel__hint">Attacco in corso...</span>`
      : `<button type="button" class="btn btn--ghost btn--abbandona-raduno" data-id="${WW.cssEscape(String(m.Id))}">Abbandona</button>` +
        (sonoCreatore ? `<button type="button" class="btn btn--primary btn--avvia-raduno" data-id="${WW.cssEscape(String(m.Id))}">Avvia</button>` : "");
    return `
    <li class="research-item raduno-item" data-id="${WW.cssEscape(String(m.Id))}">
      <div class="raduno-item__info">
        <strong>#${m.Id}</strong> — ${descrizioneBersaglio(m)} — creato da ${m.Creatore}${sonoCreatore ? " (tu)" : ""}
        <br><span class="panel__hint">Le tue truppe: G:${m.Guerrieri} L:${m.Lanceri} A:${m.Arceri} C:${m.Catapulte} (tot. ${totale}) — Tempo rimanente: ${m.MinutiRimanenti} min</span>
      </div>
      ${azioni}
    </li>`;
  }

  function renderRaduniListe() {
    const listaAperti = document.getElementById("raduni-aperti-list");
    if (listaAperti) {
      listaAperti.innerHTML = aperti.length
        ? aperti.map(templateApertoRow).join("")
        : `<li class="panel__hint">Nessun raduno aperto al momento. Creane uno tu!</li>`;
    }

    const listaMie = document.getElementById("raduni-mie-list");
    if (listaMie) {
      listaMie.innerHTML = mie.length
        ? mie.map(templateMiaRow).join("")
        : `<li class="panel__hint">Non stai partecipando a nessun raduno.</li>`;
    }

    // Se il raduno per cui si stava compilando il form è sparito (scaduto,
    // annullato, o avviato dal creatore), il form resterebbe aperto su un
    // id ormai morto — lo si chiude da solo.
    if (idInJoin && !aperti.some((r) => String(r.Id) === String(idInJoin))) chiudiFormPartecipa();
  }

  WW.NET.onJson("RaduniUpdate", (msg) => {
    aperti = Array.isArray(msg.Aperti) ? msg.Aperti : [];
    mie = Array.isArray(msg.MiePartecipazioni) ? msg.MiePartecipazioni : [];
    renderRaduniListe();
  });

  function collegaEventiStatici() {
    const btnCrea = document.getElementById("btn-crea-raduno");
    if (btnCrea) btnCrea.addEventListener("click", creaRaduno);

    const listaAperti = document.getElementById("raduni-aperti-list");
    if (listaAperti) {
      listaAperti.addEventListener("click", (e) => {
        const btn = e.target.closest(".btn--partecipa-raduno");
        if (!btn || btn.disabled) return;
        apriFormPartecipa(btn.dataset.id);
      });
    }

    const listaMie = document.getElementById("raduni-mie-list");
    if (listaMie) {
      listaMie.addEventListener("click", (e) => {
        const btnAbb = e.target.closest(".btn--abbandona-raduno");
        if (btnAbb) { abbandonaRaduno(btnAbb.dataset.id); return; }
        const btnAvvia = e.target.closest(".btn--avvia-raduno");
        if (btnAvvia) avviaRaduno(btnAvvia.dataset.id);
      });
    }

    const btnAnnulla = document.getElementById("btn-annulla-partecipa-raduno");
    if (btnAnnulla) btnAnnulla.addEventListener("click", chiudiFormPartecipa);

    const btnConferma = document.getElementById("btn-conferma-partecipa-raduno");
    if (btnConferma) btnConferma.addEventListener("click", confermaPartecipa);

    const tabsTipo = document.getElementById("raduno-crea-tipo");
    if (tabsTipo) {
      tabsTipo.addEventListener("click", (e) => {
        const btn = e.target.closest(".tier-btn");
        if (!btn) return;
        selezionaTipoBersaglioCrea(btn.dataset.tipo);
      });
    }
  }

  let uiCostruita = false;
  function renderRaduni() {
    if (!uiCostruita) {
      costruisciTruppeUI();
      collegaEventiStatici();
      aggiornaBottoneConferma(); // stato iniziale (0 truppe): "Conferma" parte disabilitato
      uiCostruita = true;
    }
    if (idInJoin) aggiornaEsercitoDisponibili(); // le disponibilità cambiano ad ogni tick (truppe addestrate, perse, ecc.)
    // La lista giocatori PVP (WW.pvpListaGiocatori) arriva in modo asincrono da 14-battaglia.js: se il tab
    // "Giocatore" è già selezionato quando la lista cambia, la select va tenuta aggiornata ad ogni tick.
    if (tipoBersaglioCrea === "PVP") aggiornaSelectGiocatore();
  }

  WW.renderRaduni = renderRaduni;
})(window.WW);
