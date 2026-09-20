/* ==========================================================
   Warrior & Wealth — Web Client — 07-citta.js
   ----------------------------------------------------------
   SCHERMATA CITTÀ — strutture e spostamento truppe (Guarnigione).

   Ogni struttura (Ingresso, Cancello, Mura, Torri, Castello, Centro) ha
   una propria guarnigione, spostabile da/verso il Villaggio col comando
   "SpostamentoTruppe|token|From|To|guerrieri|lanceri|arceri|catapulte|
   tier" (un tier alla volta). Come nel client desktop, le quantità di
   ogni tier restano in memoria (non si azzerano cambiando tab) e un
   comando parte per ogni tier con almeno un valore diverso da zero
   quando si preme "Sposta". "Centro" in interfaccia = chiave server
   "Citta".

   In cima c'è anche una panoramica del villaggio (.city-map, sfondo
   assets/Village_1.jpg) con un marker per struttura (solo nome + strato
   difensivo, non interattivo: toccarlo scorre alla card vera sotto, dove
   si spostano davvero le truppe), posizionato con le stesse coordinate
   percentuali del client desktop ("pos" in STRUTTURE_CITTA).

   Dipende da: WW.GAME (04-game-main.js), WW.fmtInt (00-core.js),
   WW.NET/WW.AUTH (01-net.js/02-auth.js). Esporta: WW.renderCittaList
   — usata da renderAllFromServer in 04-game-main.js. */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  // "pos" (percentuali left/top) è il CENTRO del riquadro che il pannello
  // del client desktop assegna a ciascuna struttura (panel1, 658×611 —
  // vedi GUI/Citta_V2.Designer.cs: Location+Size di ogni panel_<Struttura>,
  // qui ridotti al solo punto centrale e convertiti in percentuale). Il
  // marker sulla mappa web viene centrato su questo punto (vedi
  // .city-map__marker, transform: translate(-50%,-50%)) invece di
  // occupare l'intero riquadro originale, molto più grande di quanto serva
  // per un marker che mostra solo nome e numero di strato.
  //
  // "strato" è l'ordine difensivo ufficiale da Wiki/Game/Componenti/Citta.md:
  // un attaccante deve attraversare le strutture in questa sequenza (1→6)
  // per arrivare al giocatore (strato 7, non una struttura qui). Mostrato
  // come numeretto sul marker della mappa così il giocatore capisce subito
  // "in che ordine" verrà attaccata ogni struttura.
  // "labelKey" (17/09/2026, su richiesta dell'utente): stesse Label già
  // usate in 09-ricerca.js per le stesse strutture/unità — nessuna nuova
  // Descrizione da aggiungere lato server. A differenza di Ricerca, qui il
  // nome viene riletto ad ogni tick da aggiornaCittaCard()/renderCittaMap()
  // (girano già continuamente), quindi non serve un listener onDescrizione
  // separato: se la Label arriva dopo la prima costruzione della card/
  // marker, il testo si aggiorna comunque al tick successivo.
  const STRUTTURE_CITTA = [
    { chiave: "Ingresso", nome: "Ingresso", salute: false, strato: 1, pos: { left: 75.55, top: 13.85 }, labelKey: "Label Ingresso" },
    { chiave: "Mura", nome: "Mura", salute: true, strato: 2, pos: { left: 83.65, top: 55.6 }, labelKey: "Label Mura" },
    { chiave: "Cancello", nome: "Cancello", salute: true, strato: 3, pos: { left: 54.65, top: 28.65 }, labelKey: "Label Cancello" },
    { chiave: "Torri", nome: "Torri", salute: true, strato: 4, pos: { left: 68.35, top: 88.8 }, labelKey: "Label Torri" },
    { chiave: "Citta", nome: "Centro", salute: false, strato: 5, pos: { left: 42.45, top: 59.85 }, labelKey: "Label Citta" },
    { chiave: "Castello", nome: "Castello", salute: true, strato: 6, pos: { left: 18.45, top: 35.9 }, labelKey: "Label Castello" },
  ];

  const UNITA_CITTA = [
    { nome: "Guerriero", icona: "Guerriero_V2.png", chiave: "g", prefissoVillaggio: "guerrieri", nomeServer: "Guerrieri", labelKey: "Label Guerrieri" },
    { nome: "Lanciere", icona: "Lanciere_V2.png", chiave: "l", prefissoVillaggio: "lanceri", nomeServer: "Lanceri", labelKey: "Label Lanceri" },
    { nome: "Arciere", icona: "Arciere_V2.png", chiave: "a", prefissoVillaggio: "arceri", nomeServer: "Arceri", labelKey: "Label Arceri" },
    { nome: "Catapulta", icona: "Catapulta_V2.png", chiave: "c", prefissoVillaggio: "catapulte", nomeServer: "Catapulte", labelKey: "Label Catapulte" },
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
    return direzione === "in" ? WW.GAME.num(`${u.prefissoVillaggio}_${tier}`) : WW.GAME.num(`${u.nomeServer}_${tier}_${s.chiave}`);
  }

  // Riparazione per struttura (Salute e Difesa separatamente, più "Ripara
  // Tutto" a livello di schermata — vedi templateBtnRiparaTutto più sotto):
  // replica del pattern del client desktop (Citta_V2.cs, ComandiInvio.
  // Riparazione/RiparaTutto → comando "Ripara|token|Struttura|Salute|Difesa"
  // gestito da Set_Riparazioni in ServerConnection.cs). La riparazione vera
  // e propria è graduale e lato server (Server.Ripara, ogni tempo_
  // Riparazione secondi consuma risorse e sana un po' alla volta): qui ci
  // limitiamo a impostare il flag Riparazioni[i]=true, come faceva il
  // client desktop — nessuna anteprima costi, il client originale non la
  // mostrava.
  function templateCittaCard(s) {
    // Stesso bordo dorato a sinistra/sfondo del banner "Ripara Tutto"
    // (15/09/2026, su richiesta dell'utente: "riproporre l'estetica del
    // pulsante Ripara Tutto anche per i singoli bottoni Ripara" — poi
    // corretto: solo lo stile del bottone, senza l'icona a chiave inglese,
    // tolta su richiesta) — vedi .btn-ripara in style.css.
    // Ripara/Salute/Difesa: stesse Label server già usate in Ricerca (Label
    // Ripara/Salute/Difesa) — fallback italiano subito, poi ricalcolate ad
    // ogni tick da aggiornaCittaCard() insieme al resto della card.
    const nomeRipara = WW.descrizioni["Label Ripara"] || "Ripara";
    const nomeSalute = WW.descrizioni["Label Salute"] || "Salute";
    const nomeDifesa = WW.descrizioni["Label Difesa"] || "Difesa";
    const barre = s.salute
      ? `
      <div class="stat-bar-row">
        <div class="stat-bar stat-bar--hp" data-campo="salute"><div class="stat-bar__fill"></div><span class="stat-bar__label"></span></div>
        <button type="button" class="btn-ripara" data-ripara="Salute" hidden title="${nomeRipara} ${nomeSalute}">${nomeRipara}</button>
      </div>
      <div class="stat-bar-row">
        <div class="stat-bar stat-bar--def" data-campo="difesa"><div class="stat-bar__fill"></div><span class="stat-bar__label"></span></div>
        <button type="button" class="btn-ripara" data-ripara="Difesa" hidden title="${nomeRipara} ${nomeDifesa}">${nomeRipara}</button>
      </div>`
      : "";
    const tierBtns = TIER_LABELS.map((label, i) => `<button type="button" class="tier-btn${i === 0 ? " is-active" : ""}" data-tier="${i + 1}">${label}</button>`).join("");
    const unitRows = UNITA_CITTA.map(
      (u) => `
      <li class="row-item row-item--form">
        <img src="assets/${u.icona}" class="icon-inline" alt="">
        <span class="row-item__label"${u.labelKey ? ` data-label-per="${u.labelKey}"` : ""}>${(u.labelKey && WW.descrizioni[u.labelKey]) || u.nome}</span>
        <span class="row-item__value" data-disponibili="${u.chiave}" title="${WW.t('disponibiliSpostamento')}">0</span>
        <div class="qty-stepper" data-unit-stepper="${u.chiave}">
          <button type="button" class="qty-btn qty-btn--minus" aria-label="Diminuisci">−</button>
          <span class="qty-stepper__value">0</span>
          <button type="button" class="qty-btn qty-btn--plus" aria-label="Aumenta">+</button>
        </div>
      </li>`
    ).join("");

    const nomeStruttura = (s.labelKey && WW.descrizioni[s.labelKey]) || s.nome;
    const nomeGuarnigione = WW.descrizioni["Label Guarnigione"] || "Guarnigione";
    return `
    <li class="city-card" data-struttura="${s.chiave}">
      <div class="city-card__header">
        <strong><span data-campo="nome">${nomeStruttura}</span> <span class="city-card__strato" title="${WW.t('stratoDifensivoPrefix').replace('{0}', s.strato)}">[${s.strato}]</span></strong>
        <span class="city-card__guarnigione" data-campo="guarnigione">…</span>
      </div>
      ${barre}
      <!-- Ridisegnato il 15/09/2026, su richiesta dell'utente ("possiamo
           migliorare i bottoni Guarnigione?"): prima era un .btn--ghost
           generico, uguale a un bottone qualunque e senza nessun indizio
           che aprisse/chiudesse qualcosa. Ora ha un layout dedicato
           (etichetta a sinistra, freccetta a destra che si capovolge) e
           uno stato "aperto" ben distinto (sfondo pieno) — vedi
           collegaEventiCitta() più sotto per l'aria-expanded. -->
      <button type="button" class="btn-toggle-guarnigione" aria-expanded="false">
        <span data-campo="guarnigione-label">${nomeGuarnigione}</span>
        <span class="btn-toggle-guarnigione__chevron" aria-hidden="true">▾</span>
      </button>
      <div class="mini-form form-guarnigione" hidden>
        <div class="section-toggle section-toggle--inline direzione-toggle">
          <button type="button" class="section-toggle__btn is-active" data-direzione="in">${WW.t('versoPrefix')} ${nomeStruttura}</button>
          <button type="button" class="section-toggle__btn" data-direzione="out">${WW.t('versoPrefix')} ${WW.t('villaggio')}</button>
        </div>
        <div class="tier-tabs">${tierBtns}</div>
        <ul class="unit-list unit-list--form">${unitRows}</ul>
        <p class="panel__hint pendenti-hint"></p>
        <!-- 15/09/2026, su richiesta dell'utente: avviso quando il
             giocatore prova a spostare più truppe di quelle disponibili
             (bug segnalato: "spawnano delle truppe dal nulla"). Nascosto
             di default, mostrato per qualche secondo da
             mostraAvvisoTruppeInsufficienti() — vedi inviaSpostamentoTruppe
             più sotto. -->
        <p class="panel__hint avviso-truppe" hidden>${WW.t('truppeNonDisponibiliAvviso')}</p>
        <button type="button" class="btn btn--primary btn--block btn-conferma-sposta">${WW.t('spostaBtn')}</button>
      </div>
    </li>`;
  }

  // Panoramica del villaggio (.city-map): un marker minimale per
  // struttura — solo nome e numero di strato, niente HP/DEF/Guarnigione
  // (quelli si leggono nella card sotto, evitando di duplicare dati che
  // cambiano ad ogni tick anche qui). Per questo il marker è del tutto
  // statico: costruito una sola volta, non serve più aggiornarlo quando
  // arrivano nuovi dati dal server. Toccare un marker scorre alla card
  // vera nella lista sotto invece di aprire qui un form: su schermi
  // piccoli un mini-form sopra l'immagine sarebbe troppo piccolo per
  // essere usabile.
  function templateCittaMarker(s) {
    // 18/09/2026: niente più "style" nell'HTML (CSP style-src-attr, vedi
    // renderCittaMap subito sotto, che imposta left/top con
    // marker.style.left/top dopo l'inserimento — a differenza
    // dell'attributo style="" scritto qui, questo non rientra nella
    // direttiva "style-src-attr"). "data-left"/"data-top" portano il
    // valore fino a lì.
    return `
    <button type="button" class="city-map__marker" data-struttura="${s.chiave}"
      data-left="${s.pos.left}" data-top="${s.pos.top}"
      title="${WW.t('stratoDifensivoPrefix').replace('{0}', s.strato)}">
      <span class="city-map__strato">${s.strato}</span>
      <span class="city-map__nome">${(s.labelKey && WW.descrizioni[s.labelKey]) || s.nome}</span>
    </button>`;
  }

  // Costruita una volta sola (il marker è statico, vedi sopra): a
  // differenza di renderCittaList/aggiornaCittaCard qui non c'è nulla da
  // aggiornare ad ogni tick, quindi renderCittaMap si limita a controllare
  // se la mappa va ancora costruita.
  function renderCittaMap() {
    const container = document.getElementById("city-map-markers");
    if (!container) return;
    if (container.children.length !== STRUTTURE_CITTA.length) {
      container.innerHTML = STRUTTURE_CITTA.map(templateCittaMarker).join("");
      // Posizione: vedi nota su "data-left"/"data-top" in templateCittaMarker().
      container.querySelectorAll(".city-map__marker").forEach((el) => {
        el.style.left = `${el.dataset.left}%`;
        el.style.top = `${el.dataset.top}%`;
      });
      // Toccare un marker scorre alla card corrispondente nella lista sotto
      // e la evidenzia per un attimo, per far capire "sei atterrato qui".
      container.addEventListener("click", (e) => {
        const marker = e.target.closest(".city-map__marker");
        if (!marker) return;
        const card = document.querySelector(`#city-list [data-struttura="${marker.dataset.struttura}"]`);
        if (!card) return;
        card.scrollIntoView({ behavior: "smooth", block: "center" });
        card.classList.add("city-card--evidenziata");
        setTimeout(() => card.classList.remove("city-card--evidenziata"), 1500);
      });
    }
    // 17/09/2026: i marker restano "statici" solo nel senso che non vengono
    // ricostruiti (vedi sopra) — il nome viene comunque riletto ad ogni
    // tick, così una Label server arrivata dopo la prima costruzione (es.
    // subito dopo il login) viene comunque applicata al giro successivo.
    STRUTTURE_CITTA.forEach((s) => {
      const nomeEl = container.querySelector(`[data-struttura="${s.chiave}"] .city-map__nome`);
      if (nomeEl) nomeEl.textContent = (s.labelKey && WW.descrizioni[s.labelKey]) || s.nome;
    });
  }

  // Fa lampeggiare il "pallino" numerato del marker sulla mappa quando la
  // struttura ha bisogno di attenzione (15/09/2026, su richiesta
  // dell'utente) — chiamata da aggiornaCittaCard() ad ogni tick con lo
  // stato calcolato lì ("critica"/"riparazione"/"danneggiata"/null).
  const STATI_MARKER = ["danneggiata", "critica", "riparazione"];
  function aggiornaMarkerCitta(s, stato) {
    const pallino = document.querySelector(`#city-map-markers [data-struttura="${s.chiave}"] .city-map__strato`);
    if (!pallino) return;
    STATI_MARKER.forEach((nome) => pallino.classList.toggle(`city-map__strato--${nome}`, nome === stato));
  }

  // Aggiorna i valori mostrati in una card: guarnigione, barre HP/DEF, e —
  // se il mini-form è aperto — gli stepper/disponibili del tier corrente.
  function aggiornaCittaCard(s) {
    const li = document.querySelector(`#city-list [data-struttura="${s.chiave}"]`);
    if (!li) return;
    const stato = cittaStato[s.chiave];

    // Nome struttura, "Guarnigione" e "Verso ..." (17/09/2026): riletti qui
    // ad ogni tick — insieme al resto della card — invece che una sola
    // volta in templateCittaCard, così una Label server arrivata dopo la
    // prima costruzione viene comunque applicata (stesso fallback nome
    // italiano usato lì).
    const nomeStruttura = (s.labelKey && WW.descrizioni[s.labelKey]) || s.nome;
    const nomeCampoEl = li.querySelector('[data-campo="nome"]');
    if (nomeCampoEl) nomeCampoEl.textContent = nomeStruttura;

    const nomeGuarnigione = WW.descrizioni["Label Guarnigione"] || "Guarnigione";
    const guarnLabelEl = li.querySelector('[data-campo="guarnigione-label"]');
    if (guarnLabelEl) guarnLabelEl.textContent = nomeGuarnigione;

    const btnVersoStruttura = li.querySelector('[data-direzione="in"]');
    if (btnVersoStruttura) btnVersoStruttura.textContent = `${WW.t("versoPrefix")} ${nomeStruttura}`;
    const btnVersoVillaggio = li.querySelector('[data-direzione="out"]');
    if (btnVersoVillaggio) btnVersoVillaggio.textContent = `${WW.t("versoPrefix")} ${WW.t("villaggio")}`;

    const guarnEl = li.querySelector('[data-campo="guarnigione"]');
    if (guarnEl) guarnEl.textContent = `${nomeGuarnigione}: ${WW.fmtInt(WW.GAME.num(`Guarnigione_${s.chiave}`))}/${WW.fmtInt(WW.GAME.num(`Guarnigione_${s.chiave}Max`))}`;

    let daRiparare = 0;
    // Stato del "pallino" numerato sulla mappa (15/09/2026, su richiesta
    // dell'utente: farlo lampeggiare quando la struttura ha bisogno di
    // attenzione) — tre stati possibili, in ordine di priorità: "critica"
    // (HP o DEF arrivati a 0 — vince sempre, resta urgente anche se la
    // riparazione è già partita), "riparazione" (danneggiata ma già in
    // riparazione — nessuna azione richiesta, vedi Riparazione_X_Salute/
    // Difesa in PlayerSnapshot.cs, prima non esposta al client), oppure
    // "danneggiata" (sotto al massimo ma riparazione non ancora avviata).
    // null = nessun problema, il pallino resta il solito colore fisso.
    let statoMarker = null;
    if (s.salute) {
      const salute = WW.GAME.num(`Salute_${s.chiave}`);
      const saluteMax = WW.GAME.num(`Salute_${s.chiave}Max`);
      const difesa = WW.GAME.num(`Difesa_${s.chiave}`);
      const difesaMax = WW.GAME.num(`Difesa_${s.chiave}Max`);
      const saluteDaRiparare = saluteMax > 0 && salute < saluteMax;
      const difesaDaRiparare = difesaMax > 0 && difesa < difesaMax;
      if (saluteDaRiparare) daRiparare++;
      if (difesaDaRiparare) daRiparare++;

      const critica = (saluteMax > 0 && salute <= 0) || (difesaMax > 0 && difesa <= 0);
      const riparando = WW.GAME.raw[`Riparazione_${s.chiave}_Salute`] === "True" || WW.GAME.raw[`Riparazione_${s.chiave}_Difesa`] === "True";
      if (critica) statoMarker = "critica";
      else if (riparando) statoMarker = "riparazione";
      else if (saluteDaRiparare || difesaDaRiparare) statoMarker = "danneggiata";

      const barraHp = li.querySelector('[data-campo="salute"]');
      if (barraHp) {
        barraHp.querySelector(".stat-bar__fill").style.width = saluteMax > 0 ? `${Math.min(100, (salute / saluteMax) * 100)}%` : "0%";
        barraHp.querySelector(".stat-bar__label").textContent = `HP: ${WW.fmtInt(salute)}/${WW.fmtInt(saluteMax)}`;
      }
      const barraDef = li.querySelector('[data-campo="difesa"]');
      if (barraDef) {
        barraDef.querySelector(".stat-bar__fill").style.width = difesaMax > 0 ? `${Math.min(100, (difesa / difesaMax) * 100)}%` : "0%";
        barraDef.querySelector(".stat-bar__label").textContent = `DEF: ${WW.fmtInt(difesa)}/${WW.fmtInt(difesaMax)}`;
      }
      const nomeRipara = WW.descrizioni["Label Ripara"] || "Ripara";
      const btnRiparaSalute = li.querySelector('[data-ripara="Salute"]');
      if (btnRiparaSalute) {
        btnRiparaSalute.hidden = !saluteDaRiparare;
        btnRiparaSalute.textContent = nomeRipara;
        btnRiparaSalute.title = `${nomeRipara} ${WW.descrizioni["Label Salute"] || "Salute"}`;
      }
      const btnRiparaDifesa = li.querySelector('[data-ripara="Difesa"]');
      if (btnRiparaDifesa) {
        btnRiparaDifesa.hidden = !difesaDaRiparare;
        btnRiparaDifesa.textContent = nomeRipara;
        btnRiparaDifesa.title = `${nomeRipara} ${WW.descrizioni["Label Difesa"] || "Difesa"}`;
      }
    }
    aggiornaMarkerCitta(s, statoMarker);

    UNITA_CITTA.forEach((u) => {
      const el = li.querySelector(`[data-disponibili="${u.chiave}"]`);
      if (el) el.textContent = WW.fmtInt(disponibiliCitta(u, s, stato.direzione, stato.tier));
      // Nome unità (18/09/2026: qui MANCAVA la rilettura ad ogni tick, a
      // differenza di nomeStruttura/nomeGuarnigione sopra — il mini-form
      // guarnigione viene costruito una sola volta in templateCittaCard,
      // quindi se "Label Guerrieri/Lanceri/Arceri/Catapulte" arrivava dal
      // server DOPO quella prima costruzione, il nome restava bloccato sul
      // fallback italiano per sempre. "data-label-per" era già presente nel
      // markup (vedi templateCittaCard) proprio per questo aggancio, non
      // ancora usato. Stesso principio già applicato a nomeStruttura poco
      // sopra.
      if (u.labelKey) {
        const labelEl = li.querySelector(`[data-label-per="${u.labelKey}"]`);
        if (labelEl) labelEl.textContent = WW.descrizioni[u.labelKey] || u.nome;
      }
    });

    aggiornaPendentiHint(s);
    return daRiparare;
  }

  // "Ripara Tutto" compare solo quando conviene davvero (>= 2 statistiche
  // da riparare in totale, sommando tutte le strutture) — stessa soglia del
  // client desktop (Citta_V2.cs: if (daRiparare >= 2) btn_Ripara_Tutto.
  // Visible = true).
  function aggiornaBtnRiparaTutto(totaleDaRiparare) {
    const banner = document.getElementById("ripara-tutto-banner");
    if (!banner) return;
    banner.hidden = totaleDaRiparare < 2;
    const testoEl = document.getElementById("ripara-tutto-testo");
    if (testoEl) testoEl.textContent = WW.t("struttureDanneggiatePrefix").replace("{0}", totaleDaRiparare);
  }

  // Mostra/nasconde "Ferma tutte le riparazioni" (15/09/2026, su richiesta
  // dell'utente): a differenza di aggiornaBtnRiparaTutto, qui non conta le
  // statistiche danneggiate ma controlla se una riparazione è REALMENTE in
  // corso da qualche parte (i flag Riparazione_X_Salute/Difesa esposti da
  // PlayerSnapshot.cs) — compare anche per una sola struttura in
  // riparazione, non solo da 2 in su.
  function aggiornaBtnRiparaStop() {
    const wrap = document.getElementById("ripara-stop-wrap");
    if (!wrap) return;
    const inCorso = STRUTTURE_CITTA.some(
      (s) => s.salute && (WW.GAME.raw[`Riparazione_${s.chiave}_Salute`] === "True" || WW.GAME.raw[`Riparazione_${s.chiave}_Difesa`] === "True")
    );
    wrap.hidden = !inCorso;
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
    hintEl.textContent = tierConValori.length > 0 ? WW.t("inAttesaInvioTierPrefix").replace("{0}", tierConValori.join(", ")) : "";
  }

  function renderCittaList() {
    // Titolo pannello (17/09/2026): stesso principio di "ricerca-generali-
    // title" in 09-ricerca.js — rilettura ad ogni tick, nessun listener
    // separato necessario.
    const titleEl = document.getElementById("citta-title");
    if (titleEl) titleEl.textContent = WW.descrizioni["Label Citta"] || "Città";

    renderCittaMap();
    const ul = document.getElementById("city-list");
    if (!ul) return;
    if (ul.children.length !== STRUTTURE_CITTA.length) {
      ul.innerHTML = STRUTTURE_CITTA.map(templateCittaCard).join("");
      collegaEventiCitta(ul);
      collegaBtnRiparaTutto();
      collegaBtnRiparaStop();
    }
    const totaleDaRiparare = STRUTTURE_CITTA.reduce((somma, s) => somma + aggiornaCittaCard(s), 0);
    aggiornaBtnRiparaTutto(totaleDaRiparare);
    aggiornaBtnRiparaStop();
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

      const btnToggleGuarnigione = e.target.closest(".btn-toggle-guarnigione");
      if (btnToggleGuarnigione) {
        const form = card.querySelector(".form-guarnigione");
        form.hidden = !form.hidden;
        btnToggleGuarnigione.setAttribute("aria-expanded", String(!form.hidden));
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
        const u = UNITA_CITTA.find((x) => x.chiave === chiaveUnita);
        const q = stato.quantita[stato.tier];
        // Shift/Ctrl+click = passo più grande (WW.qtyStepDelta, 00-core.js) —
        // richiesto dall'utente 13/09/2026, uguale per tutti gli stepper.
        const passo = WW.qtyStepDelta(e);
        const nuovoValore = Math.max(0, q[chiaveUnita] + (btnQty.classList.contains("qty-btn--plus") ? passo : -passo));
        // 15/09/2026, su richiesta dell'utente: non permettere di impostare
        // con lo stepper più truppe di quelle realmente disponibili (nel
        // villaggio o nella struttura, a seconda della direzione) — prima
        // non c'era nessun limite, ed era possibile chiedere lo spostamento
        // di truppe inesistenti ("spawnano dal nulla" lato server, ora
        // corretto anche in SpostamentoTruppe). disponibiliCitta() gestisce
        // già entrambe le direzioni e tutte le 6 strutture.
        const disponibili = disponibiliCitta(u, s, stato.direzione, stato.tier);
        q[chiaveUnita] = Math.min(nuovoValore, disponibili);
        stepperEl.querySelector(".qty-stepper__value").textContent = String(q[chiaveUnita]);
        aggiornaPendentiHint(s);
        return;
      }

      if (e.target.closest(".btn-conferma-sposta")) {
        inviaSpostamentoTruppe(s, stato);
      }

      const btnRipara = e.target.closest("[data-ripara]");
      if (btnRipara) {
        // "Ripara|token|Struttura|Salute|Difesa" (vedi Set_Riparazioni in
        // ServerConnection.cs): imposta solo il flag, la riparazione vera è
        // graduale e lato server. Il pulsante si nasconde da solo al
        // prossimo tick quando il valore torna al massimo.
        WW.NET.send("Ripara", WW.AUTH.accessToken, s.chiave, btnRipara.dataset.ripara);
      }
    });
  }

  // Un solo listener, collegato una volta sola insieme a quelli delle card
  // (stesso "if invariato non ricollegare" di renderCittaList).
  function collegaBtnRiparaTutto() {
    const btn = document.getElementById("btn-ripara-tutto");
    if (!btn) return;
    btn.addEventListener("click", () => {
      WW.NET.send("Ripara", WW.AUTH.accessToken, "Ripara Tutto");
    });
  }

  // "Ferma tutte le riparazioni" (15/09/2026, su richiesta dell'utente):
  // comando "Ripara Stop|token" -> case "Ripara Stop" in ServerConnection.cs
  // -> Stop_Riparazione(player), che riporta le 8 variabili bool di
  // Player.Riparazioni a false. Nessun altro parametro richiesto: ferma
  // TUTTE le strutture in un colpo solo, non una alla volta.
  function collegaBtnRiparaStop() {
    const btn = document.getElementById("btn-ripara-stop");
    if (!btn) return;
    btn.addEventListener("click", () => {
      WW.NET.send("Ripara Stop", WW.AUTH.accessToken);
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

  // Mostra per qualche secondo l'avviso "truppe non disponibili" nella card
  // indicata, poi lo nasconde da solo (15/09/2026, su richiesta dell'utente).
  function mostraAvvisoTruppeInsufficienti(card) {
    const avvisoEl = card && card.querySelector(".avviso-truppe");
    if (!avvisoEl) return;
    avvisoEl.hidden = false;
    clearTimeout(avvisoEl._timeoutAvviso);
    avvisoEl._timeoutAvviso = setTimeout(() => {
      avvisoEl.hidden = true;
    }, 4000);
  }

  // Invia UN comando "SpostamentoTruppe" per ogni tier che ha almeno una
  // quantità diversa da zero (così più tier vengono spostati "in un colpo
  // solo" dal punto di vista dell'utente, anche se il protocollo accetta un
  // tier alla volta) — vedi nota di apertura sezione.
  function inviaSpostamentoTruppe(s, stato) {
    const from = stato.direzione === "in" ? "Esercito Villaggio" : s.chiave;
    const to = stato.direzione === "in" ? s.chiave : "Esercito Villaggio";
    let inviato = false;
    let insufficiente = false;
    TIER_LABELS.forEach((_, i) => {
      const tier = i + 1;
      const q = stato.quantita[tier];
      if (q.g + q.l + q.a + q.c === 0) return;
      // 15/09/2026, su richiesta dell'utente ("avvisare lato client il
      // giocatore in caso di mancanza di truppe, tale azione non è
      // possibile"): ricontrolliamo qui le disponibilità reali invece di
      // fidarci ciecamente delle quantità impostate con lo stepper. Lo
      // stepper stesso ormai non permette più di superare il disponibile al
      // momento del click, ma tra quel momento e la pressione di "Sposta"
      // può passare del tempo (un tick del server, un'altra azione altrove)
      // durante il quale le truppe disponibili possono diminuire — in quel
      // caso correggiamo la quantità invece di inviare un valore non più
      // valido, e avvisiamo il giocatore. Vale per entrambe le direzioni
      // (Villaggio->struttura e viceversa) e per tutte e 6 le strutture,
      // dato che disponibiliCitta() le gestisce già tutte.
      UNITA_CITTA.forEach((u) => {
        const disponibili = disponibiliCitta(u, s, stato.direzione, tier);
        if (q[u.chiave] > disponibili) {
          q[u.chiave] = disponibili;
          insufficiente = true;
        }
      });
      if (q.g + q.l + q.a + q.c === 0) return;
      WW.NET.send("SpostamentoTruppe", WW.AUTH.accessToken, from, to, q.g, q.l, q.a, q.c, tier);
      inviato = true;
      stato.quantita[tier] = { g: 0, l: 0, a: 0, c: 0 };
    });
    const card = document.querySelector(`#city-list [data-struttura="${s.chiave}"]`);
    if (insufficiente) mostraAvvisoTruppeInsufficienti(card);
    if (!inviato) return;
    if (card) {
      aggiornaStepperVisibili(card, s, stato);
      card.querySelector(".form-guarnigione").hidden = true;
      const btnToggle = card.querySelector(".btn-toggle-guarnigione");
      if (btnToggle) btnToggle.setAttribute("aria-expanded", "false");
    }
    aggiornaCittaCard(s);
  }

  WW.renderCittaList = renderCittaList;
})(window.WW);
