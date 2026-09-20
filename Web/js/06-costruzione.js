/* ==========================================================
   Warrior & Wealth — Web Client — 06-costruzione.js
   ----------------------------------------------------------
   SCHERMATA COSTRUZIONE / ADDESTRAMENTO — stessa idea della
   schermata Main (liste costruite dai dati del server, vedi
   04-game-main.js), ma ogni riga ha uno stepper e non c'è
   azione per riga: UN pulsante in fondo a ciascun pannello
   invia tutte le quantità in un colpo solo, esattamente come i
   comandi "Costruzione" (16 campi) e "Reclutamento" (4 campi +
   livello) accettati dal server (e come fa il client desktop).

   Dipende da: WW.struttureCivili/struttureMilitari/caserme/
   COSTRUZIONE_ORDINE/unita/GAME (04-game-main.js), WW.fmtInt
   (00-core.js), WW.NET/WW.AUTH (01-net.js/02-auth.js), WW.
   descrizioni/WW.onDescrizione/WW.renderDescrizioneRicca
   (04-game-main.js: stesso box “info” già usato in
   09-ricerca.js, qui esteso a edifici e addestramento).
   Esporta: WW.renderStruttureListForm, WW.renderUnitaForm,
   WW.renderSbloccoUnita — usate da renderAllFromServer in
   04-game-main.js. */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  // Toggle Edifici/Addestramento su mobile (stesso pattern di #main-panel-toggle).
  // 20/09/2026, su richiesta dell'utente: spostato in cima allo script (era in
  // fondo al file) e reso indipendente da qualunque altra inizializzazione qui
  // sotto, così il pannello "Edifici" risulta visibile fin da subito quando si
  // apre la schermata Costruzione, anche se qualcos'altro nel file dovesse
  // fallire più avanti. La classe "is-visible" è comunque già presente anche
  // nell'HTML (index.html) come ulteriore rete di sicurezza, sullo stesso
  // principio del pulsante "Edifici" che ha già "is-active" hardcoded lì.
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

  // Titoli statici della schermata (18/09/2026, su richiesta dell'utente):
  // arrivano anche loro come "Descrizione|Label ...|<testo>" (vedi
  // Descrizioni.cs), stesso meccanismo già usato per le righe di
  // Strutture/Esercito (vedi WW.descrizioni in 04-game-main.js). Restano
  // sul valore italiano hardcoded nell'HTML finché il valore non arriva.
  function aggiornaTitoliCostruzione() {
    [
      ["costruzione-edifici-title", "Label Costruzione"],
      ["costruzione-civili-subtitle", "Label Strutture Civili"],
      ["costruzione-militari-subtitle", "Label Strutture Militari"],
      ["costruzione-caserme-subtitle", "Label Caserme"],
      ["costruzione-esercito-title", "Label Addestramento"],
      ["costruzione-toggle-addestramento", "Label Addestramento"],
      // Pulsante "Costruzione" nella tab-bar in basso (vedi index.html):
      // stesso testo/etichetta del titolo del pannello qui sopra, riusata
      // pari pari invece di chiedere un altro Label_* al server per lo
      // stesso identico testo.
      ["tab-btn-costruzione", "Label Costruzione"],
    ].forEach(([id, chiave]) => {
      const el = document.getElementById(id);
      const testo = WW.descrizioni[chiave];
      if (el && testo) el.textContent = testo;
    });
  }
  aggiornaTitoliCostruzione();
  WW.onDescrizione((chiave) => {
    if (!chiave.startsWith("Label ")) return;
    aggiornaTitoliCostruzione();
    // Ricostruisce le righe di Strutture/Addestramento se l'etichetta di
    // una di esse arriva dopo il primo render (vedi "force" più sotto).
    renderStruttureListForm("costruzione-civili-list", WW.struttureCivili, true);
    renderStruttureListForm("costruzione-militari-list", WW.struttureMilitari, true);
    renderStruttureListForm("costruzione-caserme-list", WW.caserme, true);
    renderUnitaForm(true);
  });

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
    const valori = WW.COSTRUZIONE_ORDINE.map((s) => qtyCostruzione[s.tipoServer] || 0);
    if (valori.every((v) => v === 0)) return;
    WW.NET.send("Costruzione", WW.AUTH.accessToken, ...valori);
    // Azzera gli stepper dopo l'invio: il conteggio costruito/in-coda
    // arriverà aggiornato dal prossimo "Update_Data".
    WW.COSTRUZIONE_ORDINE.forEach((s) => setQtyCostruzione(s.tipoServer, 0));
  }

  // Box descrizione ("i", costo/effetto per il prossimo livello): stesso
  // meccanismo già usato in 09-ricerca.js, riesportato qui perché la cache
  // e il comando "Descrizione" sono condivisi (WW.descrizioni/
  // WW.onDescrizione, 04-game-main.js) — vedi lì per i dettagli. Le chiavi
  // esatte ("Fattoria", "Produzione Spade", "Caserma Guerrieri", "Guerrieri
  // 3", ...) vengono da Descrizioni.cs e sono già in chiaveDesc/
  // chiaveDescPrefix sugli array condivisi (04-game-main.js), non derivate
  // qui per evitare di reinventare una mappa che può disallinearsi.
  function cssEscape(str) {
    return window.CSS && CSS.escape ? CSS.escape(str) : str.replace(/["\\]/g, "\\$&");
  }

  function popolaDescrizioneCostruzione(chiave, box) {
    const testo = WW.descrizioni[chiave];
    box.innerHTML = "";
    if (testo) {
      box.appendChild(WW.renderDescrizioneRicca(testo));
    } else {
      const p = document.createElement("span");
      p.className = "research-desc__vuoto";
      p.textContent = WW.t("descrizioneNonRicevuta");
      box.appendChild(p);
    }
  }

  // Se un box è già aperto quando arriva/aggiorna una Descrizione (es. dopo
  // che una costruzione in coda è completata), lo aggiorniamo subito invece
  // di aspettare che l'utente lo richiuda e riapra.
  WW.onDescrizione((chiave) => {
    const box = document.querySelector(`.research-desc[data-desc-per="${cssEscape(chiave)}"]`);
    if (box && !box.hidden) popolaDescrizioneCostruzione(chiave, box);
  });

  // Click sul pulsante "ⓘ" di una riga (Strutture o Addestramento): apre/
  // chiude il box descrizione, senza inviare nulla al server. Richiamata
  // dai due listener di delegazione sotto (Edifici/Esercito), prima del
  // controllo sui pulsanti +/−, perché condividono lo stesso pannello.
  function gestisciClickInfo(e) {
    const infoBtn = e.target.closest(".research-info-btn");
    if (!infoBtn) return false;
    const box = infoBtn.closest(".research-item").querySelector(".research-desc");
    const apri = box.hidden;
    box.hidden = !apri;
    infoBtn.classList.toggle("is-active", apri);
    if (apri) popolaDescrizioneCostruzione(infoBtn.dataset.infoTipo, box);
    return true;
  }

  // "force" (18/09/2026, su richiesta dell'utente: stesso meccanismo di
  // localizzazione già applicato alla schermata Main) ricostruisce la lista
  // anche se il numero di righe non è cambiato — serve per aggiornare i
  // nomi (s.labelKey, vedi WW.descrizioni) quando l'etichetta arriva dal
  // server DOPO il primo render, altrimenti la guardia sul children.length
  // qui sotto blocca per sempre il rebuild e i nomi restano in italiano.
  function renderStruttureListForm(listId, dati, force) {
    const ul = document.getElementById(listId);
    if (!ul) return;
    if (force || ul.children.length !== dati.length) {
      ul.innerHTML = dati
        .map(
          (s) => {
            const nome = (s.labelKey && WW.descrizioni[s.labelKey]) || s.nome;
            return `
        <li class="research-item">
          <div class="row-item row-item--form">
            <img src="assets/${s.icona}" alt="">
            <span class="row-item__label">${nome}</span>
            <button type="button" class="research-info-btn" data-info-tipo="${s.chiaveDesc}" title="${WW.t("descriptionAria")}" aria-label="${WW.t("descriptionAria")} ${nome}"><img src="assets/info.png" alt=""></button>
            <div class="qty-stepper" data-tipo-server="${s.tipoServer}">
              <button type="button" class="qty-btn qty-btn--minus" aria-label="${WW.t("decreaseQty")} ${nome}">−</button>
              <span class="qty-stepper__value">${qtyCostruzione[s.tipoServer] || 0}</span>
              <button type="button" class="qty-btn qty-btn--plus" aria-label="${WW.t("increaseQty")} ${nome}">+</button>
            </div>
          </div>
          <div class="research-desc" data-desc-per="${s.chiaveDesc}" hidden></div>
        </li>`;
          }
        )
        .join("");
    }
  }

  // Delegazione eventi sui pulsanti +/− delle Strutture (e sul pulsante "ⓘ"
  // di ogni riga): un solo listener per pannello invece di uno per
  // elemento, così funziona anche sulle righe ricostruite più tardi (es. al
  // primo arrivo dei dati dal server).
  const costruzioneEdificiPanel = document.querySelector('[data-panel="costruzione-edifici"]');
  if (costruzioneEdificiPanel) {
    costruzioneEdificiPanel.addEventListener("click", (e) => {
      if (gestisciClickInfo(e)) return;
      const btn = e.target.closest(".qty-btn");
      if (!btn) return;
      const stepper = btn.closest(".qty-stepper");
      const tipoServer = stepper.dataset.tipoServer;
      const attuale = qtyCostruzione[tipoServer] || 0;
      // Shift/Ctrl+click = passo più grande (WW.qtyStepDelta, 00-core.js) —
      // richiesto dall'utente 13/09/2026, uguale per tutti gli stepper.
      const passo = WW.qtyStepDelta(e);
      setQtyCostruzione(tipoServer, btn.classList.contains("qty-btn--plus") ? attuale + passo : attuale - passo);
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
    const [guerrieri, lanceri, arceri, catapulte] = WW.unita.map((u) => qtyReclutamento[u.prefisso] || 0);
    if (!guerrieri && !lanceri && !arceri && !catapulte) return;
    WW.NET.send("Reclutamento", WW.AUTH.accessToken, tierCostruzione, guerrieri, lanceri, arceri, catapulte);
    WW.unita.forEach((u) => setQtyReclutamento(u.prefisso, 0));
  }

  // La chiave di Descrizione per l'addestramento dipende dal TIER scelto
  // ("Guerrieri 1".."Guerrieri 5", ecc. — vedi Descrizioni.cs), quindi a
  // differenza delle Strutture qui bisogna ricostruire la riga (non solo il
  // valore) quando cambia tier: teniamo traccia dell'ultimo tier renderizzato
  // in ul.dataset.tier per capire quando serve.
  // "force": stesso motivo di renderStruttureListForm sopra.
  function renderUnitaForm(force) {
    const ul = document.getElementById("costruzione-unit-list");
    if (!ul) return;
    if (force || ul.children.length !== WW.unita.length || ul.dataset.tier !== String(tierCostruzione)) {
      ul.innerHTML = WW.unita
        .map(
          (u) => {
            const chiaveDesc = `${u.chiaveDescPrefix} ${tierCostruzione}`;
            const nome = (u.labelKey && WW.descrizioni[u.labelKey]) || u.nome;
            return `
        <li class="research-item">
          <div class="row-item row-item--form">
            <img src="assets/${u.icona}" alt="">
            <span class="row-item__label">${nome}</span>
            <button type="button" class="research-info-btn" data-info-tipo="${chiaveDesc}" title="${WW.t("descriptionAria")}" aria-label="${WW.t("descriptionAria")} ${nome}"><img src="assets/info.png" alt=""></button>
            <div class="qty-stepper" data-prefisso="${u.prefisso}">
              <button type="button" class="qty-btn qty-btn--minus" aria-label="${WW.t("decreaseQty")} ${nome}">−</button>
              <span class="qty-stepper__value">${qtyReclutamento[u.prefisso] || 0}</span>
              <button type="button" class="qty-btn qty-btn--plus" aria-label="${WW.t("increaseQty")} ${nome}">+</button>
            </div>
          </div>
          <div class="research-desc" data-desc-per="${chiaveDesc}" hidden></div>
        </li>`;
          }
        )
        .join("");
      ul.dataset.tier = String(tierCostruzione);
    }
  }

  // Delegazione eventi per i pulsanti +/− e "ⓘ" dell'Addestramento (stesso
  // principio della Costruzione qui sopra).
  const costruzioneEsercitoPanel = document.querySelector('[data-panel="costruzione-esercito"]');
  if (costruzioneEsercitoPanel) {
    costruzioneEsercitoPanel.addEventListener("click", (e) => {
      if (gestisciClickInfo(e)) return;
      const btn = e.target.closest(".qty-btn");
      if (!btn) return;
      const stepper = btn.closest(".qty-stepper");
      const prefisso = stepper.dataset.prefisso;
      const attuale = qtyReclutamento[prefisso] || 0;
      // Shift/Ctrl+click = passo più grande (WW.qtyStepDelta, 00-core.js) —
      // richiesto dall'utente 13/09/2026, uguale per tutti gli stepper.
      const passo = WW.qtyStepDelta(e);
      setQtyReclutamento(prefisso, btn.classList.contains("qty-btn--plus") ? attuale + passo : attuale - passo);
    });
  }

  document.querySelectorAll("#costruzione-tier-tabs .tier-btn").forEach((btn) => {
    btn.addEventListener("click", () => {
      document.querySelectorAll("#costruzione-tier-tabs .tier-btn").forEach((b) => b.classList.remove("is-active"));
      btn.classList.add("is-active");
      tierCostruzione = Number(btn.dataset.tier) || 1;
      // Le quantità in attesa riguardavano il tier precedente: azzeriamo per
      // evitare di addestrare al tier sbagliato per errore.
      WW.unita.forEach((u) => setQtyReclutamento(u.prefisso, 0));
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
      `${WW.t("sbloccoUnitaPrefix")} II: ${WW.fmtInt(WW.GAME.num("Unlock_Truppe_II"))} · ` +
      `III: ${WW.fmtInt(WW.GAME.num("Unlock_Truppe_III"))} · ` +
      `IV: ${WW.fmtInt(WW.GAME.num("Unlock_Truppe_IV"))} · ` +
      `V: ${WW.fmtInt(WW.GAME.num("Unlock_Truppe_V"))}`;
    document.querySelectorAll("[data-sblocco-unita]").forEach((el) => (el.textContent = testo));
  }

  WW.renderStruttureListForm = renderStruttureListForm;
  WW.renderUnitaForm = renderUnitaForm;
  WW.renderSbloccoUnita = renderSbloccoUnita;
})(window.WW);
