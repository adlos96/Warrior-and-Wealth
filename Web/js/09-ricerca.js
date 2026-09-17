/* ==========================================================
   Warrior & Wealth — Web Client — 09-ricerca.js
   ----------------------------------------------------------
   SCHERMATA RICERCA — a differenza di Costruzione/Addestramento
   (dove UN pulsante invia tutte le quantità insieme), qui OGNI
   pulsante invia SUBITO un comando "Ricerca|token|<tipo>" per un
   singolo livello, esattamente come il client desktop (vedi
   screenshot Ricerca.JPG: tanti piccoli pulsanti "Nome: Livello").
   ResearchManager.cs applica costo/tempo/coda lato server; qui ci
   limitiamo a mostrare il livello attuale e inviare il tipo giusto.

   Tre gruppi, tre elenchi di dati sotto (le stringhe "tipo" sono
   ESATTE — devono combaciare con gli "switch" di ResearchManager.cs,
   non sono etichette a piacere):
     - RICERCA_GENERALI: le 8 ricerche di base (colonna sinistra).
     - RICERCA_UNITA + RICERCA_STATS_UNITA: Esercito, 4 unità x 4
       statistiche. Nota: "tipoServer" usa i nomi SINGOLARI/irregolari
       che il server si aspetta davvero ("Lancere"/"Arcere", non
       "Lanciere"/"Arciere" come nel resto della UI) — stesso tipo di
       cosa già segnalata per "Reclutamento" in 06-costruzione.js.
     - RICERCA_CITTA: le 6 strutture del villaggio. Ingresso e Centro
       hanno SOLO Livello/Guarnigione (quelle due strutture non hanno
       Salute/Difesa nel gioco — vedi Gioco/Strutture.cs e
       Wiki/Game/Componenti/Citta.md), le altre quattro hanno tutte e
       4 le statistiche. "Centro" nell'interfaccia corrisponde al tipo
       server "Citta" (stesso alias già usato in 07-citta.js).

   Il pulsante "Ricerca" di ogni riga si disabilita da solo quando
   "Ricerca_Attiva"="True" (una ricerca è già in corso — il server la
   rifiuterebbe comunque se non ci sono code extra, ma disabilitare il
   pulsante evita un giro a vuoto, stesso comportamento del client
   desktop). Il costo esatto dell'invio non è mostrato in riga (il
   server lo comunica comunque nel log/Cronologia dopo l'invio, tramite
   "Log_Server" — vedi Ricerca_Start/Ricerca_LivelloRichiesto in ITA.cs,
   già gestiti genericamente da WW.NET.on("Log_Server", ...) in
   04-game-main.js), MA ogni riga ha anche un pulsante "ⓘ" che apre una
   descrizione con costo/tempo per il prossimo livello: il server la
   manda da solo, senza che il client la richieda, con "Descrizione|
   Ricerca <tipo>|<testo>" (stessa sintassi BBCode-like di Log_Server —
   vedi Descrizioni.cs, chiamata dopo ogni login/AutoLogin e dopo ogni
   ricerca completata). "Ricerca <tipo>" è ESATTAMENTE "Ricerca " + la
   stessa stringa "tipo" già usata per il comando, quindi non serve
   nessuna mappa aggiuntiva chiave-per-chiave.

   Dipende da: WW.GAME (04-game-main.js), WW.fmtInt (00-core.js),
   WW.NET/WW.AUTH (01-net.js/02-auth.js), WW.renderDescrizioneRicca
   (04-game-main.js, renderer generico "a chip" per i testi di Descrizione,
   esportato apposta per essere riusato qui e in futuro anche altrove, es.
   Costruzione). Esporta: WW.renderRicerca — usata da renderAllFromServer
   in 04-game-main.js. */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  // "labelKey" (17/09/2026, su richiesta dell'utente): stesso meccanismo già
  // usato per struttureCivili/struttureMilitari/caserme/unita in
  // 04-game-main.js — (labelKey && WW.descrizioni[labelKey]) || nome, nessun
  // nuovo metodo. Le chiavi combaciano esattamente con i nuovi
  // "Descrizione|Label ...|" mandati da Descrizioni.cs. "Guarnigione" e i
  // nomi delle strutture di Città (Ingresso/Cancello/Mura/Torri/Centro/
  // Castello) non hanno ancora un Label lato server, quindi restano nome
  // fisso in italiano com'erano.
  const RICERCA_GENERALI = [
    { nome: "Costruzione", tipo: "Costruzione", chiave: "ricerca_costruzione", labelKey: "Label Costruzione" },
    { nome: "Produzione", tipo: "Produzione", chiave: "ricerca_produzione", labelKey: "Label Produzione" },
    { nome: "Addestramento", tipo: "Addestramento", chiave: "ricerca_addestramento", labelKey: "Label Addestramento" },
    { nome: "Popolazione", tipo: "Popolazione", chiave: "ricerca_popolazione", labelKey: "Label Popolazione" },
    { nome: "Trasporto", tipo: "Trasporto", chiave: "ricerca_trasporto", labelKey: "Label Trasporto" },
    { nome: "Ripara", tipo: "Riparazione", chiave: "ricerca_riparazione", labelKey: "Label Ripara" },
    { nome: "Spionaggio", tipo: "Spionaggio", chiave: "ricerca_Spionaggio", labelKey: "Label Spionaggio" },
    { nome: "Contro-Spionaggio", tipo: "Contro-Spionaggio", chiave: "ricerca_Contro_Spionaggio", labelKey: "Label Contro-Spionaggio" },
  ];

  const RICERCA_UNITA = [
    { nome: "Guerrieri", tipoServer: "Guerriero", chiave: "guerriero", icona: "Guerriero_V2.png", labelKey: "Label Guerrieri" },
    { nome: "Lancieri", tipoServer: "Lancere", chiave: "lancere", icona: "Lanciere_V2.png", labelKey: "Label Lanceri" },
    { nome: "Arcieri", tipoServer: "Arcere", chiave: "arcere", icona: "Arciere_V2.png", labelKey: "Label Arceri" },
    { nome: "Catapulte", tipoServer: "Catapulta", chiave: "catapulta", icona: "Catapulta_V2.png", labelKey: "Label Catapulte" },
  ];
  const RICERCA_STATS_UNITA = [
    { label: "Attacco", suffisso: "attacco", labelKey: "Label Attacco" },
    { label: "Salute", suffisso: "salute", labelKey: "Label Salute" },
    { label: "Difesa", suffisso: "difesa", labelKey: "Label Difesa" },
    { label: "Livello", suffisso: "livello", labelKey: "Label Livello" },
  ];

  // 17/09/2026: "Label Ingresso/Mura/Cancello/Torri/Castello/Citta" ora
  // disponibili (Descrizioni.cs) — stesso labelKey delle altre due liste.
  // "Guarnigione" resta senza Label server, fissa in italiano.
  const RICERCA_CITTA = [
    { nome: "Ingresso", tipoServer: "Ingresso", chiave: "ingresso", stats: ["livello", "guarnigione"], labelKey: "Label Ingresso" },
    { nome: "Cancello", tipoServer: "Cancello", chiave: "cancello", stats: ["livello", "salute", "difesa", "guarnigione"], labelKey: "Label Cancello" },
    { nome: "Mura", tipoServer: "Mura", chiave: "mura", stats: ["livello", "salute", "difesa", "guarnigione"], labelKey: "Label Mura" },
    { nome: "Torri", tipoServer: "Torri", chiave: "torri", stats: ["livello", "salute", "difesa", "guarnigione"], labelKey: "Label Torri" },
    { nome: "Centro", tipoServer: "Citta", chiave: "citta", stats: ["livello", "guarnigione"], labelKey: "Label Citta" },
    { nome: "Castello", tipoServer: "Castello", chiave: "castello", stats: ["livello", "salute", "difesa", "guarnigione"], labelKey: "Label Castello" },
  ];
  const STAT_LABELS = { livello: "Livello", salute: "Salute", difesa: "Difesa", guarnigione: "Guarnigione" };
  const STAT_LABEL_KEYS = { livello: "Label Livello", salute: "Label Salute", difesa: "Label Difesa" };

  // Descrizioni: il server manda "Descrizione|Ricerca <tipo>|<testo>" da solo
  // (dopo login/AutoLogin e dopo ogni ricerca completata — Descrizioni.
  // DescUpdate in ServerConnection.cs/ResearchManager.cs), con la stessa
  // sintassi BBCode-like di "Log_Server" (icone [icon:...], colori [tag]).
  // "Ricerca <tipo>" è ESATTAMENTE "Ricerca " + la stessa stringa "tipo" già
  // usata per il comando — non richiediamo nulla, arriva da sé. La cache e
  // la ricezione del comando "Descrizione" sono condivise fra tutte le
  // schermate (WW.descrizioni/WW.onDescrizione, 04-game-main.js): WW.NET.on
  // accetta un solo handler per comando, quindi ogni schermata legge dalla
  // cache comune invece di registrarsi da sola.
  function chiaveDescrizione(tipo) {
    return `Ricerca ${tipo}`;
  }

  WW.onDescrizione((chiave) => {
    // Se l'infobox di questa ricerca è già aperto, aggiorna subito il
    // contenuto invece di aspettare il prossimo click.
    const box = ricercaPanel && ricercaPanel.querySelector(`[data-desc-per="${cssEscape(chiave)}"]`);
    if (box && !box.hidden) popolaDescrizione(chiave, box);

    // Etichette (nomi ricerca/unità/statistiche, "labelKey" sopra): le liste
    // vengono ricostruite solo quando cambia il numero di righe (per non
    // perdere lo stato aperto delle descrizioni, vedi renderRicercaGenerali
    // ecc. sotto), quindi se un "Label ..." arriva DOPO il primo render lo
    // applichiamo qui, stesso principio di aggiornaStruttureTitle in
    // 04-game-main.js ma per più elementi in una volta.
    if (ricercaPanel) {
      ricercaPanel.querySelectorAll(`[data-label-per="${cssEscape(chiave)}"]`).forEach((el) => {
        el.textContent = WW.descrizioni[chiave];
      });
    }
  });

  // I "tipo" contengono spazi e trattini (es. "Contro-Spionaggio", "Guerriero
  // Salute"): CSS.escape se disponibile, altrimenti un fallback minimale che
  // basta per i caratteri che compaiono davvero in questi valori.
  function cssEscape(str) {
    return window.CSS && CSS.escape ? CSS.escape(str) : str.replace(/["\\]/g, "\\$&");
  }

  function popolaDescrizione(chiave, box) {
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

  function templateResearchRow(nome, tipo, chiaveLivello, labelKey) {
    const chiaveDesc = chiaveDescrizione(tipo);
    // Stesso fallback (labelKey && WW.descrizioni[labelKey]) || nome usato in
    // 04-game-main.js/06-costruzione.js: mostra subito il nome italiano,
    // sostituito appena arriva la Descrizione col Label server (vedi
    // WW.onDescrizione sopra, che aggiorna [data-label-per] a runtime).
    const nomeMostrato = (labelKey && WW.descrizioni[labelKey]) || nome;
    const labelAttr = labelKey ? ` data-label-per="${labelKey}"` : "";
    return `
    <li class="research-item">
      <div class="row-item research-row">
        <span class="row-item__label"${labelAttr}>${nomeMostrato}</span>
        <span class="row-item__value" data-ricerca-livello="${chiaveLivello}" title="${WW.t('livelloAttualeAria')}">…</span>
        <button type="button" class="research-info-btn" data-info-tipo="${chiaveDesc}" title="${WW.t('descriptionAria')}" aria-label="${WW.t('descriptionAria')} ${nomeMostrato}"><img src="assets/info.png" alt=""></button>
        <button type="button" class="btn btn--ghost btn--sm research-btn" data-ricerca-tipo="${tipo}">${WW.t('ricercaBtn')}</button>
      </div>
      <div class="research-desc" data-desc-per="${chiaveDesc}" hidden></div>
    </li>`;
  }

  function aggiornaLivelli(root, coppie) {
    coppie.forEach(([chiave]) => {
      const el = root.querySelector(`[data-ricerca-livello="${chiave}"]`);
      if (el) el.textContent = WW.fmtInt(WW.GAME.num(chiave));
    });
  }

  function renderRicercaGenerali() {
    // Titolo pannello: stesso Label server già usato per "strutture-title" in
    // Main (04-game-main.js) — qui basta rileggerlo ad ogni render, nessun
    // listener separato necessario (renderRicercaGenerali gira già ad ogni
    // tick tramite renderRicerca/renderAllFromServer).
    const titleEl = document.getElementById("ricerca-generali-title");
    if (titleEl) titleEl.textContent = WW.descrizioni["Label Strutture Civili"] || "Strutture Civili";

    const ul = document.getElementById("ricerca-generali-list");
    if (!ul) return;
    if (ul.children.length !== RICERCA_GENERALI.length) {
      ul.innerHTML = RICERCA_GENERALI.map((r) => templateResearchRow(r.nome, r.tipo, r.chiave, r.labelKey)).join("");
    }
    aggiornaLivelli(
      ul,
      RICERCA_GENERALI.map((r) => [r.chiave])
    );
  }

  function renderRicercaEsercito() {
    const container = document.getElementById("ricerca-esercito-container");
    if (!container) return;
    if (container.children.length !== RICERCA_UNITA.length) {
      container.innerHTML = RICERCA_UNITA.map(
        (u) => `
        <div class="research-esercito-unit">
          <h3 class="panel__subtitle"><img class="icon-inline" src="assets/${u.icona}" alt=""> <span${u.labelKey ? ` data-label-per="${u.labelKey}"` : ""}>${(u.labelKey && WW.descrizioni[u.labelKey]) || u.nome}</span></h3>
          <ul class="research-list">${RICERCA_STATS_UNITA.map((s) => templateResearchRow(s.label, `${u.tipoServer} ${s.label}`, `${u.chiave}_${s.suffisso}`, s.labelKey)).join("")}</ul>
        </div>`
      ).join("");
    }
    RICERCA_UNITA.forEach((u) => {
      aggiornaLivelli(
        container,
        RICERCA_STATS_UNITA.map((s) => [`${u.chiave}_${s.suffisso}`])
      );
    });
  }

  function renderRicercaCitta() {
    const container = document.getElementById("ricerca-citta-container");
    if (!container) return;
    if (container.children.length !== RICERCA_CITTA.length) {
      container.innerHTML = RICERCA_CITTA.map(
        (s) => `
        <div class="research-citta-struttura">
          <h3 class="panel__subtitle"${s.labelKey ? ` data-label-per="${s.labelKey}"` : ""}>${(s.labelKey && WW.descrizioni[s.labelKey]) || s.nome}</h3>
          <ul class="research-list">${s.stats
            .map((stat) => templateResearchRow(STAT_LABELS[stat], `${s.tipoServer} ${STAT_LABELS[stat]}`, `ricerca_${s.chiave}_${stat}`, STAT_LABEL_KEYS[stat]))
            .join("")}</ul>
        </div>`
      ).join("");
    }
    RICERCA_CITTA.forEach((s) => {
      aggiornaLivelli(
        container,
        s.stats.map((stat) => [`ricerca_${s.chiave}_${stat}`])
      );
    });
  }

  // Un click su un pulsante "Ricerca" (in una qualsiasi delle tre liste,
  // tutte dentro questo pannello) invia subito il comando — niente stepper
  // o conferma separata, stesso comportamento del client desktop. Un click
  // sul pulsante "ⓘ" apre/chiude invece la descrizione di quella riga
  // (vedi popolaDescrizione sopra), senza inviare nulla al server.
  const ricercaPanel = document.querySelector('[data-tab-panel="ricerca"]');
  if (ricercaPanel) {
    ricercaPanel.addEventListener("click", (e) => {
      const infoBtn = e.target.closest(".research-info-btn");
      if (infoBtn) {
        const box = infoBtn.closest(".research-item").querySelector(".research-desc");
        const apri = box.hidden;
        box.hidden = !apri;
        infoBtn.classList.toggle("is-active", apri);
        if (apri) popolaDescrizione(infoBtn.dataset.infoTipo, box);
        return;
      }

      const btn = e.target.closest(".research-btn");
      if (!btn || btn.disabled) return;
      WW.NET.send("Ricerca", WW.AUTH.accessToken, btn.dataset.ricercaTipo);
    });
  }

  // Disabilita tutti i pulsanti "Ricerca" mentre una ricerca è già in corso
  // (stesso flag che il client desktop usa per bloccare i pulsanti).
  function aggiornaBottoniRicerca() {
    const attiva = WW.GAME.raw["Ricerca_Attiva"] === "True";
    ricercaPanel && ricercaPanel.querySelectorAll(".research-btn").forEach((btn) => { btn.disabled = attiva; });
  }

  // Stessa stima "c'è tempo in coda?" usata in 04-game-main.js per
  // Costruzione/Reclutamento (qui duplicata: è privata a quel file), sulla
  // stringa già formattata dal server ("hh:mm:ss", ResearchManager.
  // GetTotalResearchTime).
  function tempoMaggioreDiZero(str) {
    if (!str) return false;
    const numeri = str.match(/\d+/g);
    if (!numeri) return false;
    return numeri.some((n) => Number(n) > 0);
  }

  const btnToggleVelocizzaRicerca = document.getElementById("btn-toggle-velocizza-ricerca");
  const formVelocizzaRicerca = document.getElementById("form-velocizza-ricerca");
  const stepperVelocizzaRicercaEl = document.getElementById("stepper-velocizza-ricerca");

  let qtyVelocizzaRicerca = 0;
  function setQtyVelocizzaRicerca(v) {
    qtyVelocizzaRicerca = Math.max(0, v);
    if (stepperVelocizzaRicercaEl) {
      const valEl = stepperVelocizzaRicercaEl.querySelector(".qty-stepper__value");
      if (valEl) valEl.textContent = String(qtyVelocizzaRicerca);
    }
  }

  if (stepperVelocizzaRicercaEl) {
    stepperVelocizzaRicercaEl.addEventListener("click", (e) => {
      const btn = e.target.closest(".qty-btn");
      if (!btn) return;
      // Shift/Ctrl+click = passo più grande (WW.qtyStepDelta, 00-core.js) —
      // richiesto dall'utente 13/09/2026, uguale per tutti gli stepper.
      const passo = WW.qtyStepDelta(e);
      setQtyVelocizzaRicerca(qtyVelocizzaRicerca + (btn.classList.contains("qty-btn--plus") ? passo : -passo));
    });
  }
  if (btnToggleVelocizzaRicerca && formVelocizzaRicerca) {
    btnToggleVelocizzaRicerca.addEventListener("click", () => {
      formVelocizzaRicerca.hidden = !formVelocizzaRicerca.hidden;
    });
  }
  const btnConfermaVelocizzaRicerca = document.getElementById("btn-conferma-velocizza-ricerca");
  if (btnConfermaVelocizzaRicerca) {
    btnConfermaVelocizzaRicerca.addEventListener("click", () => {
      if (qtyVelocizzaRicerca <= 0) return;
      WW.NET.send("Velocizza_Diamanti", WW.AUTH.accessToken, "Ricerca", qtyVelocizzaRicerca);
      setQtyVelocizzaRicerca(0);
      formVelocizzaRicerca.hidden = true;
    });
  }

  // Riga "Tempo Ricerca" in cima alla schermata + comparsa/scomparsa del
  // pulsante "Velocizza" a seconda che ci sia davvero qualcosa in coda
  // (stesso principio di aggiornaTempoECodaVelocizza in 04-game-main.js,
  // qui autonomo perché quella funzione è privata a quel file).
  function renderTempoEVelocizzaRicerca() {
    const testoTempo = WW.GAME.raw["Tempo_Ricerca_Citta"];
    const elTempo = document.querySelector('[data-value="tempo-ricerca"]');
    if (elTempo) elTempo.textContent = testoTempo || "00:00:00";

    const c_e_tempo = tempoMaggioreDiZero(testoTempo);
    if (btnToggleVelocizzaRicerca) btnToggleVelocizzaRicerca.hidden = !c_e_tempo;
    if (!c_e_tempo && formVelocizzaRicerca && !formVelocizzaRicerca.hidden) {
      formVelocizzaRicerca.hidden = true;
      setQtyVelocizzaRicerca(0);
    }

    // Stesso rapporto/secondo usato per Costruzione/Reclutamento
    // ("Tempo_D_Blu", mandato una tantum al login): la velocizzazione con
    // Diamanti Blu è un'unica costante server-wide, non specifica per coda.
    const rapportoEl = document.querySelector('[data-value="ratio-velocizza-tempo-ricerca"]');
    if (rapportoEl) rapportoEl.textContent = `${WW.fmtInt(WW.GAME.num("Tempo_D_Blu"))}s`;
  }

  function renderRicerca() {
    renderRicercaGenerali();
    renderRicercaEsercito();
    renderRicercaCitta();
    renderTempoEVelocizzaRicerca();
    aggiornaBottoniRicerca();
  }

  // Toggle Generali/Esercito/Città su mobile — tre pulsanti separati su
  // richiesta esplicita dell'utente (13/09/2026), non più "Esercito e
  // Città" in un unico pulsante. Stesso pattern generico di
  // #costruzione-panel-toggle in 06-costruzione.js: funziona con
  // qualunque numero di [data-panel] senza bisogno di adattare il codice.
  const ricercaToggleBtns = document.querySelectorAll("#ricerca-panel-toggle .section-toggle__btn");
  const ricercaGridPanels = document.querySelectorAll(".main-grid--ricerca [data-panel]");
  function showRicercaPanel(target) {
    ricercaGridPanels.forEach((p) => p.classList.toggle("is-visible", p.dataset.panel === target));
  }
  ricercaToggleBtns.forEach((btn) => {
    btn.addEventListener("click", () => {
      ricercaToggleBtns.forEach((b) => b.classList.remove("is-active"));
      btn.classList.add("is-active");
      showRicercaPanel(btn.dataset.panelTarget);
    });
  });
  showRicercaPanel("ricerca-generali");

  // Toggle Esercito/Città SOLO desktop (#ricerca-destra-toggle, richiesto
  // dall'utente 13/09/2026): indipendente dal toggle mobile qui sopra,
  // volutamente non lo riusa/modifica per non rischiare di rompere il
  // comportamento a 3 vie già funzionante su mobile. Agisce sugli stessi due
  // [data-panel] (".is-visible"), ma quella classe è innocua per Generali
  // (mai nascosto da CSS al di fuori della media query mobile) e per il
  // toggle mobile stesso: ognuno dei due toggle è visibile/cliccabile solo
  // alla sua soglia di schermo (vedi style.css), quindi non entrano mai in
  // conflitto in pratica — restano comunque due "fonti di verità" separate
  // di proposito, più semplice da ragionare che condividerne una.
  const ricercaDestraToggle = document.getElementById("ricerca-destra-toggle");
  if (ricercaDestraToggle) {
    const ricercaDestraBtns = ricercaDestraToggle.querySelectorAll(".section-toggle__btn");
    const ricercaDestraPanelli = [
      document.querySelector('[data-panel="ricerca-esercito"]'),
      document.querySelector('[data-panel="ricerca-citta"]'),
    ].filter(Boolean);
    function showRicercaDestraPanel(target) {
      ricercaDestraPanelli.forEach((p) => p.classList.toggle("is-visible", p.dataset.panel === target));
    }
    ricercaDestraBtns.forEach((btn) => {
      btn.addEventListener("click", () => {
        ricercaDestraBtns.forEach((b) => b.classList.remove("is-active"));
        btn.classList.add("is-active");
        showRicercaDestraPanel(btn.dataset.panelTarget);
      });
    });
    showRicercaDestraPanel("ricerca-esercito");
  }

  WW.renderRicerca = renderRicerca;
})(window.WW);
