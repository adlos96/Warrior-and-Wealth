/* ==========================================================
   Warrior & Wealth — Web Client — 14-battaglia.js
   ----------------------------------------------------------
   SCHERMATA PVP/PVE — replica la schermata desktop "PVE-PVP":
   esplorazione e attacco contro Villaggi/Città Barbare, attacco
   PVP, e i Report (un solo pannello per battaglie e spionaggi,
   Report.Tipo == "Battaglia"/"Spionaggio" in Battaglia.cs — vedi
   commento sopra templateReportRow). I Raduni restano fuori da
   questa schermata (backend non ancora completato).

   Protocollo (ServerConnection.cs):
   - "Esplora|token|PVE|<tipo>|<livello>" — ATTENZIONE: qui <tipo>
     è SENZA accento ("Citta Barbaro", incoerenza nota col resto
     del server, replicata di proposito per non toccare l'altro
     percorso). Supporta anche "Esplora|token|PVP|<username>"
     (stesso motore di Spionaggio.EseguiSpionaggioPVP) ma senza un
     flusso client dedicato. Nessuna risposta JSON: il server
     accoda un Report di Tipo "Spionaggio" e lo rimanda col normale
     "Update_Data|Report_Lista|<json>" (vedi sotto); gli errori
     arrivano via "Log_Server|<messaggio>" (già gestito in
     04-game-main.js).
   - "Battaglia|token|<tipo>|<target>|G1..G5|L1..L5|A1..A5|C1..C5"
     (20 valori truppe) — ATTENZIONE: qui <tipo> è CON accento
     ("Città Barbaro"/"Villaggio Barbaro"/"PVP"); target = livello
     per i barbari, username per il PVP. Comando condiviso da
     entrambi i tipi di bersaglio (un solo pannello "Esercito da
     Inviare").
   - "Update_PVP_Player|<count>|<user1>|<user2>|..." — lista
     giocatori PVP disponibili (solo se Player.Livello >=
     Unlock_PVP).
   - "Update_Data|Report_Lista|<json>" — i Report del giocatore,
     intercettato PRIMA del parsing generico in
     WW.GAME.applyUpdateData (04-game-main.js) e passato qui via
     WW.BATTLE.setReports.

   Dipende da: WW.GAME/WW.NET/WW.AUTH/WW.fmtInt/WW.qtyStepDelta.
   Esporta: WW.renderBattaglia (da renderAllFromServer in
   04-game-main.js) e WW.BATTLE (setReports). */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  const UNITA = [
    { nome: "Guerriero", icona: "Guerriero_V2.png", chiave: "g", campoServer: "guerrieri", campoReport: "Guerrieri" },
    { nome: "Lanciere", icona: "Lanciere_V2.png", chiave: "l", campoServer: "lanceri", campoReport: "Lancieri" },
    { nome: "Arciere", icona: "Arciere_V2.png", chiave: "a", campoServer: "arceri", campoReport: "Arcieri" },
    { nome: "Catapulta", icona: "Catapulta_V2.png", chiave: "c", campoServer: "catapulte", campoReport: "Catapulte" },
  ];
  const TIER_LABELS = ["I", "II", "III", "IV", "V"];

  // Esercito da inviare: stesso pattern "tier persistenti tra i tab" di
  // 07-citta.js (Guarnigione) — condiviso tra attacco Barbari e attacco
  // PVP, dato che il comando "Battaglia" accetta sempre le stesse 20
  // quantità qualunque sia il tipo di bersaglio.
  const stato = {
    tier: 1,
    quantita: { 1: { g: 0, l: 0, a: 0, c: 0 }, 2: { g: 0, l: 0, a: 0, c: 0 }, 3: { g: 0, l: 0, a: 0, c: 0 }, 4: { g: 0, l: 0, a: 0, c: 0 }, 5: { g: 0, l: 0, a: 0, c: 0 } },
    tipoBarbaro: "Villaggio Barbaro", // valore usato per il comando "Battaglia" (default: Villaggio, 17/09/2026)
    livelloEsplora: 1,
    targetBarbaro: "",
    targetPvp: "",
  };

  // 16/09/2026, su richiesta dell'utente: bug segnalato — passando da Villaggio a Città
  // (o viceversa) la tendina mostrava sempre "Nessun bersaglio", anche se il server
  // aveva già mandato entrambe le liste al login (vedi ServerConnection.
  // AggiornaVillaggiClient, che manda SIA "VillaggiPersonali" SIA "CittaGlobali" ad
  // ogni Login/AutoLogin). La causa: i due handler "onJson" più sotto scartavano il
  // messaggio se non corrispondeva al tipo attivo IN QUEL MOMENTO, quindi una delle
  // due liste veniva sempre persa. Ora si tengono entrambe sempre aggiornate in due
  // variabili separate, e "barbariLista" (quella mostrata) è solo un riferimento a
  // quella corrispondente al tipo attualmente selezionato.
  let cittaGlobaliDati = []; // ultima lista "CittaGlobali" ricevuta dal server
  let villaggiPersonaliDati = []; // ultima lista "VillaggiPersonali" ricevuta dal server
  let barbariLista = []; // riferimento a cittaGlobaliDati o villaggiPersonaliDati, secondo stato.tipoBarbaro
  let pvpLista = []; // ultima lista di username da "Update_PVP_Player"
  let reports = []; // ultimo player.Report ricevuto da "Report_Lista"

  // 16/09/2026, su richiesta dell'utente: lista Report paginata a 10 alla volta
  // (ora senza alcun limite sul totale salvato — vedi GameSave.cs — la
  // schermata continuerebbe altrimenti a crescere all'infinito). reportPage è
  // 0-based, 0 = pagina più recente.
  const REPORT_PAGE_SIZE = 10;
  let reportPage = 0;

  function esploraTipo(tipoBattaglia) {
    return tipoBattaglia === "Città Barbaro" ? "Citta Barbaro" : "Villaggio Barbaro";
  }
  function sum(arr) { return (arr || []).reduce((a, b) => a + b, 0); }

  /* ---------------------------------------------------------------
     ESERCITO DA INVIARE — tier I-V, quantità persistenti tra i tab
     --------------------------------------------------------------- */

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

  function aggiornaPendentiHint() {
    const el = document.getElementById("battaglia-pendenti-hint");
    if (el) {
      const tierConValori = TIER_LABELS.map((label, i) => {
        const q = stato.quantita[i + 1];
        return q.g + q.l + q.a + q.c > 0 ? label : null;
      }).filter(Boolean);
      el.textContent = tierConValori.length > 0 ? `Truppe pronte sui tier: ${tierConValori.join(", ")}.` : "Nessuna truppa selezionata.";
    }

    // 16/09/2026, su richiesta dell'utente: il pulsante Attacca (Barbari) deve essere
    // attivo solo se è stata selezionata almeno 1 unità — prima si poteva premere
    // sempre e l'errore compariva solo dopo il click. Richiamata da qui perché
    // aggiornaPendentiHint gira già ad ogni cambio quantità (stepper) e dopo ogni
    // invio truppe (azzeraTruppe più sotto).
    const btnAttaccaBarbari = document.getElementById("btn-attacca-barbari");
    if (btnAttaccaBarbari) btnAttaccaBarbari.disabled = truppeTotale() === 0;
  }

  function aggiornaEsercitoDisponibili() {
    const lista = document.getElementById("battaglia-esercito-list");
    if (!lista) return;
    UNITA.forEach((u) => {
      const el = lista.querySelector(`[data-disponibili="${u.chiave}"]`);
      if (el) el.textContent = WW.fmtInt(WW.GAME.num(`${u.campoServer}_${stato.tier}`));
    });
  }

  function aggiornaStepperVisibili() {
    const q = stato.quantita[stato.tier];
    UNITA.forEach((u) => {
      const el = document.querySelector(`#battaglia-esercito-list [data-unit-stepper="${u.chiave}"] .qty-stepper__value`);
      if (el) el.textContent = String(q[u.chiave]);
    });
  }

  function truppeTotale() {
    return Object.values(stato.quantita).reduce((tot, q) => tot + q.g + q.l + q.a + q.c, 0);
  }

  // Ordine richiesto dal server (ServerConnection.Battaglia): G1-5, L1-5, A1-5, C1-5.
  function truppeArgsPerAttacco() {
    const per = (chiave) => [1, 2, 3, 4, 5].map((t) => stato.quantita[t][chiave]);
    return [...per("g"), ...per("l"), ...per("a"), ...per("c")];
  }

  function azzeraTruppe() {
    TIER_LABELS.forEach((_, i) => (stato.quantita[i + 1] = { g: 0, l: 0, a: 0, c: 0 }));
    aggiornaStepperVisibili();
    aggiornaPendentiHint();
  }

  function costruisciEsercitoUI() {
    const tabs = document.getElementById("battaglia-tier-tabs");
    const lista = document.getElementById("battaglia-esercito-list");
    if (!tabs || !lista || lista.children.length === UNITA.length) return;

    tabs.innerHTML = TIER_LABELS.map((l, i) => `<button type="button" class="tier-btn${i === 0 ? " is-active" : ""}" data-tier="${i + 1}">${l}</button>`).join("");
    lista.innerHTML = UNITA.map(templateEsercitoRow).join("");

    tabs.addEventListener("click", (e) => {
      const btn = e.target.closest(".tier-btn");
      if (!btn) return;
      stato.tier = Number(btn.dataset.tier) || 1;
      tabs.querySelectorAll(".tier-btn").forEach((b) => b.classList.toggle("is-active", b === btn));
      aggiornaStepperVisibili();
      aggiornaEsercitoDisponibili();
    });

    lista.addEventListener("click", (e) => {
      const btnQty = e.target.closest(".qty-btn");
      if (!btnQty) return;
      const stepperEl = btnQty.closest("[data-unit-stepper]");
      const chiave = stepperEl.dataset.unitStepper;
      const q = stato.quantita[stato.tier];
      const passo = WW.qtyStepDelta(e);
      q[chiave] = Math.max(0, q[chiave] + (btnQty.classList.contains("qty-btn--plus") ? passo : -passo));
      stepperEl.querySelector(".qty-stepper__value").textContent = String(q[chiave]);
      aggiornaPendentiHint();
    });
  }

  /* ---------------------------------------------------------------
     VILLAGGI BARBARI — toggle Città/Villaggio, Esplora, Attacca
     --------------------------------------------------------------- */

  // Nasconde l'avviso "seleziona almeno una truppa" (vedi attaccaBarbaro più sotto):
  // richiamata ogni volta che il bersaglio Barbari cambia, così un vecchio avviso non
  // resta visibile dopo che il giocatore ha cambiato scelta.
  function nascondiErroreBarbari() {
    const info = document.getElementById("barbari-attacco-errore");
    if (info) info.hidden = true;
  }

  function renderBarbariSelect() {
    const select = document.getElementById("barbari-target-select");
    if (!select) return;
    nascondiErroreBarbari();
    const valorePrecedente = select.value;
    if (barbariLista.length === 0) {
      // Testo accorciato (bug segnalato dall'utente il 14/09/2026): la colonna Villaggi
      // Barbari è più stretta di quella PVP, quindi anche con l'ellissi (.pvp-select in
      // style.css) la frase più lunga usata prima veniva tagliata troppo presto.
      select.innerHTML = `<option value="">Nessun bersaglio</option>`;
      select.disabled = true;
      // Bug segnalato dall'utente il 14/09/2026: cambiando Città/Villaggio la lista viene
      // svuotata (sopra, dal chiamante) ma qui si usciva subito senza azzerare anche il
      // target selezionato — restava impostato sul valore del tipo precedente finché non
      // si ri-esplorava, come se il messaggio "esplora il barbaro" arrivasse in ritardo.
      // Ora si azzera subito insieme alla select.
      stato.targetBarbaro = "";
      return;
    }
    select.disabled = false;
    // v.Nome include già il livello (es. "Citta Barbare Lv3", vedi Barbari.cs) — non
    // va ripetuto qui, altrimenti si legge "Citta Barbare Lv1 — Lv.1" (bug segnalato
    // dall'utente il 14/09/2026).
    select.innerHTML = barbariLista
      .map((v) => `<option value="${v.Livello}">${v.Nome}${v.Sconfitto ? " ✓ sconfitto" : ""}</option>`)
      .join("");
    // Ripristina la selezione precedente se ancora presente, altrimenti il primo bersaglio non sconfitto.
    if (barbariLista.some((v) => String(v.Livello) === valorePrecedente)) {
      select.value = valorePrecedente;
    } else {
      const primoDisponibile = barbariLista.find((v) => !v.Sconfitto) || barbariLista[barbariLista.length - 1];
      select.value = String(primoDisponibile.Livello);
    }
    stato.targetBarbaro = select.value;
  }

  // 16/09/2026, su richiesta dell'utente: rimossa la casella "Bottino stimato" sotto
  // la select Città/Villaggi Barbari — era puramente lato client (i valori venivano
  // letti da v.Cibo/v.Legno/... già presenti nell'oggetto ricevuto con la lista
  // Esplora, non da un comando server dedicato/deprecato), ma ridondante ora che il
  // resoconto completo (truppe comprese) arriva comunque nei Report dopo "Esplora".
  // Città e Villaggi Barbari restano invariati: select, "già sconfitto" ed Esplora/
  // Attacca continuano a funzionare come prima.

  function esploraBarbaro() {
    const livelloInput = document.getElementById("barbari-livello-input");
    const livello = Math.max(1, Number((livelloInput && livelloInput.value) || stato.targetBarbaro || 1));
    stato.livelloEsplora = livello;
    WW.NET.send("Esplora", WW.AUTH.accessToken, "PVE", esploraTipo(stato.tipoBarbaro), livello);
  }

  function attaccaBarbaro() {
    if (!stato.targetBarbaro) return;
    if (truppeTotale() === 0) {
      // 16/09/2026: la casella "Bottino stimato" non c'è più (vedi sopra), quindi questo
      // avviso ora usa il suo piccolo div dedicato (#barbari-attacco-errore, nascosto di
      // default e mostrato solo quando serve — non è la casella eliminata).
      const info = document.getElementById("barbari-attacco-errore");
      if (info) {
        info.innerHTML = `<span class="testo-errore">Seleziona almeno una truppa da inviare (pannello Esercito).</span>`;
        info.hidden = false;
      }
      return;
    }
    WW.NET.send("Battaglia", WW.AUTH.accessToken, stato.tipoBarbaro, stato.targetBarbaro, ...truppeArgsPerAttacco());
    azzeraTruppe();
  }

  /* ---------------------------------------------------------------
     PVP — lista giocatori, Attacca
     --------------------------------------------------------------- */

  function renderPvpSelect() {
    const select = document.getElementById("pvp-target-select");
    if (!select) return;
    const livelloOk = WW.GAME.num("livello") >= WW.GAME.num("Unlock_PVP");
    const sezione = document.getElementById("pvp-panel");
    const bloccoMsg = document.getElementById("pvp-locked-msg");
    if (bloccoMsg) bloccoMsg.hidden = livelloOk;
    if (sezione) sezione.classList.toggle("is-locked", !livelloOk);
    select.disabled = !livelloOk || pvpLista.length === 0;

    if (!livelloOk) return;
    const valorePrecedente = select.value;
    select.innerHTML = pvpLista.length > 0
      ? pvpLista.map((u) => `<option value="${u}">${u}</option>`).join("")
      : `<option value="">Nessun avversario disponibile</option>`;
    if (pvpLista.includes(valorePrecedente)) select.value = valorePrecedente;
    stato.targetPvp = select.value;
    // 17/09/2026, su richiesta dell'utente: da desktop il testo può restare troncato
    // anche dopo la riduzione del font in style.css — il title mostra il testo
    // completo al passaggio del mouse (nessun effetto su telefono, dove non serve).
    select.title = select.options[select.selectedIndex]?.text || "";
  }

  function attaccaPvp() {
    if (!stato.targetPvp) return;
    if (truppeTotale() === 0) {
      const info = document.getElementById("pvp-target-info");
      if (info) info.innerHTML = `<span class="testo-errore">Seleziona almeno una truppa da inviare (pannello Esercito).</span>`;
      return;
    }
    WW.NET.send("Battaglia", WW.AUTH.accessToken, "PVP", stato.targetPvp, ...truppeArgsPerAttacco());
    azzeraTruppe();
  }

  /* ---------------------------------------------------------------
     REPORT — battaglie e spionaggio, lista + dettaglio (modale).
     Report.Tipo (Battaglia.cs) vale "Battaglia" o "Spionaggio", entrambi
     mostrati qui. Il ramo Spionaggio copre tutto RisultatoSpionaggio
     (Fasi e Bonus incluse). Il PVE-barbaro (Esplora ->
     Spionaggio.SpionaggioPVE) genera questi stessi referti ma senza
     Strutture_Civili/Workshop/Caserme/Ricerca_Civile/Ricerca_Militare/
     Bonus (un barbaro non li ha — restano null, le sezioni corrispondenti
     spariscono da sole grazie agli "|| {}" sotto) e con solo 2 Fasi ("A
     Distanza"/"Corpo a Corpo") invece delle 7 del PVP tra giocatori. */

  function templateReportRow(r, indice) {
    if (r.Tipo === "Spionaggio" && r.Spionaggio) {
      const s = r.Spionaggio;
      const esito = s.Spionaggio_Riuscito ? "Riuscito" : "Fallito";
      const esitoClasse = s.Spionaggio_Riuscito ? "report-row__esito--vittoria" : "report-row__esito--sconfitta";
      return `
      <li class="row-item report-row" data-report-index="${indice}">
        <span class="report-row__tipo">Spionaggio</span>
        <span class="report-row__vs">vs ${(s.Giocatore && s.Giocatore.Nome) || "?"}</span>
        <span class="report-row__esito ${esitoClasse}">${esito}</span>
        <button type="button" class="btn-elimina-tondo" data-elimina-index="${indice}" title="Elimina referto" aria-label="Elimina referto">✕</button>
        <span class="report-row__data">${formattaData(r.Data)}</span>
      </li>`;
    }
    const b = r.Battaglia;
    if (!b) return "";
    // Bug segnalato dall'utente il 14/09/2026: lo stesso Report viene aggiunto SIA
    // all'attaccante SIA al difensore (BattagliaPVP.cs: attaccante.Report.Add(report);
    // difensore.Report.Add(report);), ma esito/avversario/XP erano sempre calcolati dal
    // punto di vista dell'attaccante — un difensore che vinceva vedeva comunque "vs se
    // stesso" e "Vittoria" scritto per l'altro. Va confrontato con l'username di chi sta
    // guardando (WW.AUTH.username) per sapere se il "mio" risultato è quello dell'attaccante
    // o quello del difensore.
    const ioAttaccante = b.Nome_Attaccante === (WW.AUTH && WW.AUTH.username);
    const vittoriaMia = ioAttaccante ? b.Vittoria_Attaccante : !b.Vittoria_Attaccante;
    const avversario = ioAttaccante ? b.Nome_Difensore : b.Nome_Attaccante;
    const esito = vittoriaMia ? "Vittoria" : "Sconfitta";
    const esitoClasse = vittoriaMia ? "report-row__esito--vittoria" : "report-row__esito--sconfitta";
    return `
    <li class="row-item report-row" data-report-index="${indice}">
      <span class="report-row__tipo">${b.Tipo_Battaglia}</span>
      <span class="report-row__vs">vs ${avversario}</span>
      <span class="report-row__esito ${esitoClasse}">${esito}</span>
      <button type="button" class="btn-elimina-tondo" data-elimina-index="${indice}" title="Elimina referto" aria-label="Elimina referto">✕</button>
      <span class="report-row__data">${formattaData(r.Data)}</span>
    </li>`;
  }

  // Nota 14/09/2026: il campo Report.Data arrivava in un formato non-ISO
  // (DateTime.UtcNow.ToString() lato server, dipendente dalla cultura), che
  // new Date(...) non riusciva a interpretare — la data spariva sempre dalla
  // riga. Corretto lato server (ora usa ToString("o"), ISO 8601); qui restiamo
  // comunque tolleranti (Number.isNaN sotto) nel caso arrivi un vecchio
  // formato da un report già salvato prima della correzione.
  function formattaData(iso) {
    const d = new Date(iso);
    if (Number.isNaN(d.getTime())) return "";
    // Formato compatto (senza secondi) invece di toLocaleString() completo: nella riga
    // referto c'è spazio solo per una seconda riga stretta sotto tipo/avversario/esito.
    return `${d.toLocaleDateString()} ${d.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })}`;
  }

  // 16/09/2026, su richiesta dell'utente: mostra solo REPORT_PAGE_SIZE (10) referti
  // alla volta, i più recenti prima, con "‹ Indietro"/"Avanti ›" per sfogliare gli
  // altri (il numero di referti non ha più alcun limite lato salvataggio, quindi la
  // lista può crescere molto). indiceOriginale è tenuto esplicito (invece di dedurlo
  // dalla posizione, come faceva la versione precedente) per restare corretto anche
  // se in futuro un referto non passasse il filtro sotto.
  function renderReportLista() {
    const ul = document.getElementById("battaglia-report-list");
    if (!ul) return;
    const utili = reports
      .map((r, indiceOriginale) => ({ r, indiceOriginale }))
      .filter(({ r }) => (r.Tipo === "Battaglia" && r.Battaglia) || (r.Tipo === "Spionaggio" && r.Spionaggio))
      .reverse(); // più recenti prima

    const totalPages = Math.max(1, Math.ceil(utili.length / REPORT_PAGE_SIZE));
    if (reportPage >= totalPages) reportPage = totalPages - 1;
    if (reportPage < 0) reportPage = 0;

    const inizio = reportPage * REPORT_PAGE_SIZE;
    const pagina = utili.slice(inizio, inizio + REPORT_PAGE_SIZE);
    ul.innerHTML = pagina.length > 0
      ? pagina.map(({ r, indiceOriginale }) => templateReportRow(r, indiceOriginale)).join("")
      : `<li class="panel__hint">Nessun report ancora.</li>`;

    aggiornaPagerReport(utili.length, totalPages);
  }

  function aggiornaPagerReport(totale, totalPages) {
    const pager = document.getElementById("report-list-pager");
    if (!pager) return;
    pager.hidden = totale <= REPORT_PAGE_SIZE;
    const info = document.getElementById("report-list-page-info");
    if (info) info.textContent = `Pagina ${reportPage + 1} di ${totalPages}`;
    const btnPrev = document.getElementById("report-list-prev");
    const btnNext = document.getElementById("report-list-next");
    if (btnPrev) btnPrev.disabled = reportPage <= 0;
    if (btnNext) btnNext.disabled = reportPage >= totalPages - 1;
  }

  function tabellaUnita(schierati, caduti, superstiti) {
    const righe = UNITA.map((u) => {
      const s = sum(schierati[u.campoReport]);
      const c = sum(caduti[u.campoReport]);
      const sup = sum(superstiti[u.campoReport]);
      if (s === 0 && c === 0 && sup === 0) return "";
      return `<tr><td>${u.nome}</td><td>${WW.fmtInt(s)}</td><td class="report-table__caduti">${WW.fmtInt(c)}</td><td class="report-table__superstiti">${WW.fmtInt(sup)}</td></tr>`;
    }).join("");
    return `<table class="report-table"><thead><tr><th>Unità</th><th>Schierati</th><th>Caduti</th><th>Superstiti</th></tr></thead><tbody>${righe || '<tr><td colspan="4">Nessuna unità</td></tr>'}</tbody></table>`;
  }

  function templateRisorseRaccolte(r, lato) {
    if (!r) return "";
    const righe = [
      ["Cibo", r.Cibo, "Grano_V2.png"],
      ["Legno", r.Legno, "Legna_V2.png"],
      ["Pietra", r.Pietra, "Pietra_V2.png"],
      ["Ferro", r.Ferro, "Ferro_V2.png"],
      ["Oro", r.Oro, "Oro_V2.png"],
      ["Diamanti Blu", r.Diamanti_Blu, "DiamanteBlu_V2.png"],
      ["Diamanti Viola", r.Diamanti_Viola, "DiamanteViola_V2.png"],
    ].filter(([, v]) => v > 0);
    if (righe.length === 0) return "";
    // Titolo e segno/colore dipendono dal LATO SELEZIONATO col toggle Attaccante/Difensore
    // (non da chi guarda il report): lato "attaccante" = risorse saccheggiate (verde, "+"),
    // lato "difensore" = risorse perse (rosso, "-", stesso .report-res-item__value--negativo
    // usato per l'HP/DEF struttura). Per questo va ri-renderizzata ad ogni click del toggle
    // (vedi renderBattagliaRisorseContent), non costruita una sola volta in apriReportDettaglio.
    const attaccante = lato !== "difensore";
    const segno = attaccante ? "+" : "-";
    const classeValore = attaccante ? "report-res-item__value--positivo" : "report-res-item__value--negativo";
    return `
    <div class="report-risorse">
      <h3>${attaccante ? "Risorse Saccheggiate" : "Risorse Perse"}</h3>
      <div class="report-risorse__grid">
        ${righe.map(([nome, valore, icona]) => rigaChip(nome, `${segno}${WW.fmtInt(valore)}`, icona, classeValore)).join("")}
      </div>
    </div>`;
  }

  // Riga di statistiche sotto una tabella Schierati/Caduti/Superstiti (Frecce usate,
  // Esperienza fase, ...): prima testo libero (.panel__hint), ora le stesse pillole usate
  // altrove nel referto — più leggibile e coerente (14/09/2026).
  function rigaStatFase(coppie) {
    return `<div class="report-risorse__grid report-risorse__grid--compact">${coppie.map(([nome, valoreTesto, classeValore]) => rigaChip(nome, valoreTesto, null, classeValore)).join("")}</div>`;
  }

  // Lato attualmente selezionato nel dettaglio Battaglia ("attaccante"/"difensore"): i dati
  // di entrambi i lati esistono già nel modello (RisultatoFase.Attaccante/Difensore,
  // BattagliaDistanza.Attaccante_*/Difensore_*), ma finora la UI mostrava sempre e solo
  // quelli dell'attaccante. Aggiunto il 14/09/2026 su richiesta dell'utente: "dovrei avere
  // almeno due pulsanti per osservare attacco e difesa in maniera indipendente".
  let battagliaAttiva = null;
  let battagliaLato = "attaccante";
  // 20/09/2026, su richiesta dell'utente: tier selezionato per la tabella "Statistiche
  // Truppe Barbare" (solo referti PVE) — stesso pattern di spiaTier per lo spionaggio.
  let battagliaTier = 1;

  // Sezione "Statistiche Truppe Barbare" (solo PVE — vedi Stats_Difensore in
  // BattagliaPVE.cs): stessa tabella Livello/Salute/Difesa/Attacco già usata per lo
  // spionaggio (tabellaStatUnita), con lo stesso selettore a tab per tier. Città e
  // Villaggi mostrano di proposito gli stessi numeri (condividono le statistiche lato
  // server, GetEnemyUnitStats) — non è un errore di questa tabella.
  function templateBattagliaStatsBarbaro() {
    const tierTabsHtml = TIER_LABELS.map((l, i) => `<button type="button" class="tier-btn${i === 0 ? " is-active" : ""}" data-battaglia-tier="${i + 1}">${l}</button>`).join("");
    return `
    <div class="report-fase">
      <h3>Statistiche Truppe Barbare</h3>
      <div class="tier-tabs">${tierTabsHtml}</div>
      <div id="battaglia-stats-barbaro-content"></div>
    </div>`;
  }

  function renderBattagliaStatsBarbaroContent() {
    const el = document.getElementById("battaglia-stats-barbaro-content");
    if (!el || !battagliaAttiva) return;
    const stats = (battagliaAttiva.Fasi && battagliaAttiva.Fasi[0] && battagliaAttiva.Fasi[0].Stats_Difensore) || {};
    const fonteTier = {};
    UNITA.forEach((u) => {
      const arr = stats[CAMPO_SPIA[u.chiave].stats];
      fonteTier[CAMPO_SPIA[u.chiave].stats] = arr && arr[battagliaTier - 1];
    });
    el.innerHTML = tabellaStatUnita(fonteTier, (u) => CAMPO_SPIA[u.chiave].stats, false);
  }

  function templateFase(fase, indice, lato) {
    const nomeStruttura = (fase.Struttura && fase.Struttura.Nome) || `Fase ${indice + 1}`;
    const fd = fase.Fase_Distanza;
    const pref = lato === "difensore" ? "Difensore_" : "Attaccante_";
    const unitaCorpoACorpo = lato === "difensore" ? fase.Difensore : fase.Attaccante;
    const xpFase = lato === "difensore" ? fase.Xp_Difensore : fase.Xp_Attaccante;
    const schieratiDistanza = fd && fd[`${pref}Schierati`];
    const distanzaHtml = fd && schieratiDistanza && (sum(schieratiDistanza.Guerrieri) + sum(schieratiDistanza.Lancieri) > 0)
      ? `
      <div class="report-sottofase">
        <h4>Fase a Distanza</h4>
        ${tabellaUnita(
          { Guerrieri: schieratiDistanza.Guerrieri, Lancieri: schieratiDistanza.Lancieri, Arcieri: [], Catapulte: [] },
          { Guerrieri: fd[`${pref}Morti`].Guerrieri, Lancieri: fd[`${pref}Morti`].Lancieri, Arcieri: [], Catapulte: [] },
          { Guerrieri: fd[`${pref}Sopravvisuti`].Guerrieri, Lancieri: fd[`${pref}Sopravvisuti`].Lancieri, Arcieri: [], Catapulte: [] }
        )}
        ${rigaStatFase([["Frecce usate", WW.fmtInt(fd[`${pref}Frecce_Usate`])], ["Esperienza", `+${WW.fmtInt(fd[`${pref}XP`])}`, "report-res-item__value--positivo"]])}
      </div>`
      : "";
    // Distanza e Corpo a Corpo affiancate su schermi larghi invece che sempre impilate
    // (replica avvicinata al Log_Battaglie desktop, dove le due tabelle stanno una accanto
    // all'altra — vedi Report Battaglia.JPG), impilate su mobile (14/09/2026).
    return `
    <div class="report-fase">
      <h3>${nomeStruttura}</h3>
      ${templateStrutturaStato(fase.Struttura)}
      <div class="report-fase__sottofasi">
        ${distanzaHtml}
        <div class="report-sottofase">
          <h4>Corpo a Corpo</h4>
          ${tabellaUnita(unitaCorpoACorpo.Schierati, unitaCorpoACorpo.Perdite, unitaCorpoACorpo.Sopravvisuti)}
          ${rigaStatFase([["Esperienza fase", `+${WW.fmtInt(xpFase)}`, "report-res-item__value--positivo"]])}
        </div>
      </div>
    </div>`;
  }

  // HP/DEF della struttura difesa in questa fase (Mura, Cancello, Torri,
  // Castello — Ingresso/Centro non hanno Salute/Difesa proprie, solo
  // Guarnigione, quindi qui non mostrano nulla). Aggiunto il 14/09/2026 su
  // richiesta dell'utente: "sarebbe carino mostrare anche il passaggio di
  // stato, es. da 30/30 hp a 15/30 hp" — richiede SaluteIniziale/
  // DifesaIniziale lato server (Battaglia.cs/BattagliaPVP.cs), lo
  // "snapshot" di Salute/Difesa prima del combattimento, dato che i campi
  // Salute/Difesa vengono decrementati in place durante lo scontro e a
  // fine battaglia rappresentano già solo il valore finale.
  function templateStrutturaStato(struttura) {
    if (!struttura || !struttura.SaluteMax) return "";
    const riga = (nome, prima, dopo, max) => {
      const persa = prima > dopo;
      const testo = prima === dopo ? `${WW.fmtInt(dopo)}/${WW.fmtInt(max)}` : `${WW.fmtInt(prima)}/${WW.fmtInt(max)} → ${WW.fmtInt(dopo)}/${WW.fmtInt(max)}`;
      return rigaChip(nome, testo, null, persa ? "report-res-item__value--negativo" : undefined);
    };
    return `
    <div class="report-risorse__grid report-risorse__grid--compact">
      ${riga("HP struttura", struttura.SaluteIniziale, struttura.Salute, struttura.SaluteMax)}
      ${riga("DEF struttura", struttura.DifesaIniziale, struttura.Difesa, struttura.DifesaMax)}
    </div>`;
  }

  function templateBattagliaFasiToggle() {
    return `
    <div class="section-toggle section-toggle--inline">
      <button type="button" class="section-toggle__btn${battagliaLato === "attaccante" ? " is-active" : ""}" data-battaglia-lato="attaccante">Attaccante</button>
      <button type="button" class="section-toggle__btn${battagliaLato === "difensore" ? " is-active" : ""}" data-battaglia-lato="difensore">Difensore</button>
    </div>`;
  }

  function renderBattagliaFasiContent() {
    const el = document.getElementById("battaglia-fasi-content");
    if (!el || !battagliaAttiva) return;
    el.innerHTML = (battagliaAttiva.Fasi || []).map((fase, i) => templateFase(fase, i, battagliaLato)).join("");
  }

  // "Risorse Saccheggiate"/"Risorse Perse" segue anche lei il toggle Attaccante/Difensore
  // (non è fissa in base a chi guarda, vedi nota in templateRisorseRaccolte) quindi va
  // ricostruita a ogni click del toggle esattamente come le Fasi.
  function renderBattagliaRisorseContent() {
    const el = document.getElementById("battaglia-risorse-content");
    if (!el || !battagliaAttiva) return;
    el.innerHTML = templateRisorseRaccolte(battagliaAttiva.Risorse_Raccolte, battagliaLato);
  }

  // Dettaglio Spionaggio (RisultatoSpionaggio in Battaglia.cs) — copre Fasi (bersagli/
  // strutture spiate, truppe per tier min/reale/max) e Bonus.
  //
  // ATTENZIONE, quirk di nomenclatura (confermato nel codice, Battaglia.cs): a differenza
  // di Unità/UnitGroup (referti di battaglia, "Lancieri" CON la i — vedi UNITA/campoReport
  // sopra), le strutture dello spionaggio (StatsUnità, RicercaMilitare, Caserme,
  // SpionaggioFase) usano "Lanceri" SENZA la i (stessa convenzione di Giocatori.Player/
  // PlayerSnapshot), e la classe Bonus usa "Arceri" (non "Arcieri"). CAMPO_SPIA sotto
  // centralizza questa differenza invece di sparpagliare le tre grafie nel codice.
  const CAMPO_SPIA = {
    g: { stats: "Guerrieri", bonus: "Guerrieri" },
    l: { stats: "Lanceri", bonus: "Lanceri" },
    a: { stats: "Arcieri", bonus: "Arceri" },
    c: { stats: "Catapulte", bonus: "Catapulte" },
  };

  // Reale = -1 → precisione insufficiente: si mostra SOLO il range stimato (min–max), mai
  // il -1 letterale. Reale >= 0 → si mostra solo quel valore (min==max==reale). Vale per
  // ogni campo con questa struttura Reale/Min/Max.
  //
  // "????" jolly: nasconde il DATO quando lo STADIO raggiunto non lo sblocca ancora (la
  // sezione resta visibile, solo il valore diventa "????", per far capire al giocatore che
  // deve aumentare la forza di spionaggio). `bloccato` (dal chiamante, Stadio vs soglia) ha
  // sempre la precedenza sul calcolo Reale/-1/range.
  function fmtStima(reale, min, max, bloccato) {
    if (bloccato) return `<span class="report-tv-range">????</span>`;
    if (reale === -1 || reale === undefined || reale === null) {
      // Range collassato su un solo valore (es. "~0–0" per un edificio che vale davvero 0 —
      // succede spesso perché 0 × qualunque errore fa sempre 0): mostrarlo tradirebbe il
      // valore esatto anche se la precisione non lo giustificava, quindi anche qui "????".
      if (min === max) return `<span class="report-tv-range">????</span>`;
      return `<span class="report-tv-range">~ ${WW.fmtInt(min)}–${WW.fmtInt(max)}</span>`;
    }
    return WW.fmtInt(reale);
  }
  function tv(x, bloccato) { return bloccato ? fmtStima(0, 0, 0, true) : (x ? fmtStima(x.Reale, x.Min, x.Max) : "0"); }

  // Pillola condivisa da Risorse Raccolte (Battaglia) e Magazzino/Scorta Militare/Ricerca
  // Civile/Bonus Villaggio (Spionaggio): icona opzionale + etichetta + valore allineato a
  // destra, invece del vecchio testo libero "Nome: Valore" che andava a capo disordinatamente
  // (segnalato dall'utente il 14/09/2026 sullo screenshot del Resoconto Spionaggio, poi esteso
  // al Resoconto Battaglia per coerenza visiva tra i due tipi di referto).
  const rigaChip = (nome, valoreTesto, icona, classeValore) =>
    `<span class="report-res-item">${icona ? `<img class="icon-inline" src="assets/${icona}" alt="">` : ""}<span class="report-res-item__label">${nome}</span><span class="report-res-item__value${classeValore ? ` ${classeValore}` : ""}">${valoreTesto}</span></span>`;

  function tabellaTripleValue(titolo, righe, bloccato) {
    const corpo = righe.filter(([, v]) => v).map(([nome, v, icona]) => `<tr><td>${icona ? `<img class="icon-inline" src="assets/${icona}" alt="">` : ""}${nome}</td><td>${tv(v, bloccato)}</td></tr>`).join("");
    if (!corpo) return "";
    return `<div class="report-fase"><h3>${titolo}</h3><table class="report-table"><thead><tr><th>Voce</th><th>Valore</th></tr></thead><tbody>${corpo}</tbody></table></div>`;
  }

  // Tabella Livello/Salute/Difesa/Attacco per le 4 unità, usata sia per Ricerca Militare
  // che per Bonus Truppe (stessa forma di dato, TipiStatistiche, solo chiave diversa —
  // vedi CAMPO_SPIA per il perché di due chiavi diverse).
  function tabellaStatUnita(fonte, chiaveDi, bloccato) {
    const cella = (v) => (bloccato ? '<span class="report-tv-range">????</span>' : WW.fmtInt(v));
    const righe = UNITA.map((u) => {
      const st = fonte[chiaveDi(u)];
      if (!st) return "";
      return `<tr><td>${u.nome}</td><td>${cella(st.Livello)}</td><td>${cella(st.Salute)}</td><td>${cella(st.Difesa)}</td><td>${cella(st.Attacco)}</td></tr>`;
    }).join("");
    return `<table class="report-table"><thead><tr><th>Unità</th><th>Lv.</th><th>Salute</th><th>Difesa</th><th>Attacco</th></tr></thead><tbody>${righe || '<tr><td colspan="5">Nessun dato</td></tr>'}</tbody></table>`;
  }

  // Stato del referto di spionaggio attualmente aperto nel modale (per i tab Tier/
  // Strutture, che ridisegnano solo la loro porzione senza richiudere il modale).
  // spionaggioStadio serve a questi due render "parziali" per sapere se lo stadio del
  // referto sblocca davvero quel contenuto o se va mostrato "????" (vedi fmtStima sopra).
  let spionaggioAttivo = null;
  let spionaggioStadio = 0;
  let spiaTier = 1;
  let spiaFaseIndice = 0;

  function renderSpiaTierContent() {
    const el = document.getElementById("spia-tier-content");
    if (!el || !spionaggioAttivo) return;
    const stats = spionaggioAttivo.Stats_Unità || {};
    // Stats_Unità è un array di 5 TipiStatistiche (uno per tier) per ciascuna unità.
    // Popolata solo da Stadio 6 in su (Spionaggio.cs, Load_Truppe_Stats).
    const fonteTier = {};
    UNITA.forEach((u) => {
      const arr = stats[CAMPO_SPIA[u.chiave].stats];
      fonteTier[CAMPO_SPIA[u.chiave].stats] = arr && arr[spiaTier - 1];
    });
    el.innerHTML = tabellaStatUnita(fonteTier, (u) => CAMPO_SPIA[u.chiave].stats, spionaggioStadio < 6);
  }

  function renderSpiaFaseContent() {
    const el = document.getElementById("spia-fase-content");
    if (!el || !spionaggioAttivo) return;
    const fase = (spionaggioAttivo.Fasi || [])[spiaFaseIndice];
    if (!fase) { el.innerHTML = ""; return; }
    // Popolata solo da Stadio 2 in su (Spionaggio.cs, Load_Villaggio + Load_Truppe); sotto
    // quella soglia i nomi delle strutture sono già segnaposto ("Fase N") e qui i valori
    // diventano "????" invece dei numeri di default.
    const bloccato = spionaggioStadio < 2;
    const v = fase.Struttura || {};
    // 19/09/2026, su richiesta dell'utente: nello spionaggio PVE (barbari) Salute/Difesa contano
    // solo nella fase Corpo a Corpo (vedi Spionaggio.cs) — sulla fase "A Distanza" non arrivano più
    // dal server, quindi qui vanno del tutto omesse invece di mostrare "0" (che sembrerebbe un
    // barbaro senza vita, non "qui non si applica"). "A Distanza" è un nome usato solo per questa
    // fase PVE, quindi il controllo non tocca gli strati reali del PVP (Ingresso/Mura/ecc.).
    const rigaSaluteDifesa = v.Nome === "A Distanza" ? "" : `
        <div><strong>${v.Nome || "?"}</strong><br>
          Salute: ${fmtStima(v.Salute, v.SaluteMin, v.SaluteMax, bloccato)}</div>
        <div>Difesa: ${fmtStima(v.Difesa, v.DifesaMin, v.DifesaMax, bloccato)}<br>
          Guarnigione: ${tv(v.Guarnigione, bloccato)}${v.Guarnigione_Max ? ` / ${WW.fmtInt(v.Guarnigione_Max)} max` : ""}</div>`;
    const righeTruppe = UNITA.map((u) => {
      const arr = fase[CAMPO_SPIA[u.chiave].stats];
      const t = arr && arr[spiaTier - 1];
      if (!t) return "";
      return `<tr><td>${u.nome}</td><td>${tv(t, bloccato)}</td></tr>`;
    }).join("");
    el.innerHTML = `
      <div class="report-header">
        ${rigaSaluteDifesa || `<div><strong>${v.Nome || "?"}</strong></div>`}
      </div>
      <table class="report-table"><thead><tr><th>Unità (tier ${TIER_LABELS[spiaTier - 1]})</th><th>Valore</th></tr></thead>
      <tbody>${righeTruppe || '<tr><td colspan="2">Nessun dato</td></tr>'}</tbody></table>`;
  }

  function templateSpionaggioDettaglio(s) {
    spionaggioAttivo = s;
    spiaTier = 1;
    spiaFaseIndice = 0;

    const g = s.Giocatore || {};
    const rc = s.Risorse_Civili || {};
    const rm = s.Risorse_Militari || {};
    const rs = s.Risorse_Speciali || {};
    const ec = s.Strutture_Civili || {};
    const wk = s.Workshop || {};
    const cs = s.Caserme || {};
    const ricC = s.Ricerca_Civile || {};
    const ricM = s.Ricerca_Militare || {};
    const bonus = s.Bonus || {};
    const fasi = s.Fasi || [];

    // Sblocco per Stadio (Spionaggio.cs, CalcolaLivelloSpionaggio/Spionaggioo()): 1 Risorse,
    // 2 Strutture, 3 Edifici, 4 Ricerca Civile, 5 Ricerca Militare, 6 Truppe per tier + Bonus.
    // Ogni sezione ≥ Stadio 1 va SEMPRE mostrata (il giocatore deve vedere che quella
    // statistica esiste), ma con "????" al posto dei numeri finché lo stadio non la sblocca
    // davvero — non va più nascosta del tutto: prima mostravamo dati fasulli (0 di default),
    // poi (fraintendendo la richiesta) nascondevamo la sezione intera; quello che voleva
    // l'utente è un terzo comportamento, "vedi che c'è ma non sai quanto vale ancora".
    spionaggioStadio = s.Stadio || 0;
    const stadio = spionaggioStadio;
    // (Il blocco della sezione "Strutture", stadio < 2, è gestito dentro
    // renderSpiaFaseContent tramite spionaggioStadio — non serve qui.)
    const bloccoEdifici = stadio < 3;
    const bloccoRicercaCivile = stadio < 4;
    const bloccoRicercaMilitare = stadio < 5;
    const bloccoTruppeBonus = stadio < 6;

    // Magazzino/Scorta Militare: come il bottino di battaglia, ma senza filtrare gli zeri —
    // qui è la fotografia dell'intero magazzino del bersaglio, uno 0 è un'informazione
    // ("non ha diamanti"), non "niente da mostrare".
    const rigaIcona = (nome, v, icona) => rigaChip(nome, WW.fmtInt(v || 0), icona);
    const magazzinoHtml = [
      rigaIcona("Cibo", rc.Cibo, "Grano_V2.png"), rigaIcona("Legno", rc.Legno, "Legna_V2.png"), rigaIcona("Pietra", rc.Pietra, "Pietra_V2.png"),
      rigaIcona("Ferro", rc.Ferro, "Ferro_V2.png"), rigaIcona("Oro", rc.Oro, "Oro_V2.png"), rigaIcona("Popolazione", rc.Popolazione, "Popolazione_V2.png"),
      rigaIcona("Diamanti Viola", rs.Diamanti_Viola, "DiamanteViola_V2.png"), rigaIcona("Diamanti Blu", rs.Diamanti_Blu, "DiamanteBlu_V2.png"),
    ].join("");
    const militariHtml = [
      rigaIcona("Spade", rm.Spade, "Spade_V2.png"), rigaIcona("Lance", rm.Lance, "Lance_V2.png"), rigaIcona("Archi", rm.Archi, "Archi_V2.png"),
      rigaIcona("Scudi", rm.Scudi, "Scudi_V2.png"), rigaIcona("Armature", rm.Armature, "Armature_V2.png"), rigaIcona("Frecce", rm.Frecce, "Frecce_V2.png"),
    ].join("");

    // Quando bloccata dallo stadio, la sezione mostra comunque tutte le voci (con "????"),
    // non solo quelle diverse da zero — altrimenti un valore di default a 0 (mai spiato)
    // sparirebbe invece di segnalare "esiste ma non lo sai ancora".
    const ricercaCivileHtml = [
      ["Produzione", ricC.Produzione], ["Costruzione", ricC.Costruzione], ["Addestramento", ricC.Addestramento], ["Popolazione", ricC.Popolazione],
      ["Trasporto", ricC.Trasporto], ["Riparazione", ricC.Riparazione], ["Spionaggio", ricC.Spionaggio], ["Contro-Spionaggio", ricC.Contro_Spionaggio],
    ].filter(([, v]) => bloccoRicercaCivile || v).map(([nome, v]) => rigaChip(nome, bloccoRicercaCivile ? "????" : `Lv.${WW.fmtInt(v)}`)).join("");

    // Bonus Villaggio: percentuali/moltiplicatori globali (Bonus.* non legati a una singola
    // unità). Valore mostrato così com'è, senza moltiplicare per 100: il formato esatto
    // (frazione 0.1 = +10%, o già una percentuale) non è verificabile senza un referto
    // reale — da confermare quando lo spionaggio sarà davvero in uso lato server.
    const bonusVillaggioHtml = [
      ["Salute strutture", bonus.Salute_Strutture], ["Difesa strutture", bonus.Difesa_Strutture], ["Guarnigione strutture", bonus.Guarnigione_Strutture],
      ["Produzione risorse", bonus.Produzione_Risorse], ["Costruzione", bonus.Costruzione], ["Addestramento", bonus.Addestramento],
      ["Capacità trasporto", bonus.Capacità_Trasporto], ["Ricerca", bonus.Ricerca], ["Riparazione", bonus.Riparazione],
      ["Spionaggio", bonus.Spionaggio], ["Contro-Spionaggio", bonus.Contro_Spionaggio],
    ].filter(([, v]) => bloccoTruppeBonus || v).map(([nome, v]) => rigaChip(nome, bloccoTruppeBonus ? "????" : WW.fmtDecimal(v, 2))).join("");

    const tierTabsHtml = TIER_LABELS.map((l, i) => `<button type="button" class="tier-btn${i === 0 ? " is-active" : ""}" data-spia-tier="${i + 1}">${l}</button>`).join("");
    // Tab "Strutture" (Ingresso/Mura/Cancello/Torri/Centro/Castello/Player nello screenshot
    // desktop): riusa lo stesso stile del toggle Città/Villaggio (.section-toggle--inline,
    // già corretto per non tagliare/scrollare — vedi style.css) invece di introdurne uno
    // nuovo. Assente se il referto non ha Fasi (es. spionaggio fallito ad uno stadio
    // troppo basso per rivelare le strutture).
    const fasiTabsHtml = fasi.length > 0
      ? `<div class="section-toggle section-toggle--inline">${fasi.map((f, i) => `<button type="button" class="section-toggle__btn${i === 0 ? " is-active" : ""}" data-spia-fase="${i}">${(f.Struttura && f.Struttura.Nome) || `Fase ${i + 1}`}</button>`).join("")}</div>`
      : "";

    // Intestazione: le pillole (usate invece sotto per Magazzino/Scorta Militare, dove i
    // nomi sono corti) troncavano le etichette più lunghe qui ("Forza spionag...",
    // "Esperienza bers..." — segnalato dall'utente il 14/09/2026). Righe etichetta/valore
    // a tutta larghezza invece: niente più troncamento, ed è la forma richiesta
    // esplicitamente ("Giocatore spiato:", "Livello:", ...). Le due righe di ricerca del
    // bersaglio compaiono solo da stadio 4 in su, quando Ricerca_Civile viene davvero
    // popolata (Spionaggio.cs, Load_Ricerca_Civile) — altrimenti sarebbero "Lv.0"
    // fuorvianti (sembrerebbe ricerca 0 invece di "non spiata").
    const rigaStat = (nome, valoreTesto) => `<div class="report-header__stat"><span>${nome}</span><strong>${valoreTesto}</strong></div>`;
    const statHeaderRighe = [
      rigaStat("Giocatore spiato", g.Nome || "?"),
      rigaStat("Livello", WW.fmtInt(g.Livello)),
      rigaStat("Esperienza", WW.fmtInt(g.Esperienza)),
      rigaStat("Forza di spionaggio", WW.fmtInt(s.Forza_Spionaggio)),
      rigaStat("Stadio", WW.fmtInt(s.Stadio)),
    ];
    if (ricC.Spionaggio !== undefined) statHeaderRighe.push(rigaStat("Ricerca Spionaggio (bersaglio)", `Lv.${WW.fmtInt(ricC.Spionaggio)}`));
    if (ricC.Contro_Spionaggio !== undefined) statHeaderRighe.push(rigaStat("Ricerca Contro-Spionaggio (bersaglio)", `Lv.${WW.fmtInt(ricC.Contro_Spionaggio)}`));

    // Il giocatore fallito (Stadio 0) non vede alcuna sezione, come da wiki ("il rapporto si
    // limita a segnalare l'insuccesso") — solo intestazione ed esito.
    if (stadio <= 0) {
      return `
        <div class="report-header report-header--spia">${statHeaderRighe.join("")}</div>
        <div class="report-esito report-esito--sconfitta">SPIONAGGIO FALLITO</div>`;
    }

    // Da Stadio 1 in su tutte le sezioni successive sono SEMPRE mostrate (il giocatore deve
    // vedere che quella statistica esiste, come promemoria di cosa può ancora scoprire) — solo
    // i valori diventano "????" (vedi bloccoX sopra e fmtStima/tabellaStatUnita/ecc.) finché lo
    // stadio raggiunto non li sblocca davvero.
    return `
      <div class="report-header report-header--spia">
        ${statHeaderRighe.join("")}
      </div>

      <div class="report-risorse"><h3>Magazzino</h3><div class="report-risorse__grid">${magazzinoHtml}</div></div>
      <div class="report-risorse"><h3>Scorta Militare</h3><div class="report-risorse__grid">${militariHtml}</div></div>

      <div class="report-fase">
        <h3>Strutture</h3>
        ${fasiTabsHtml}
        <div id="spia-fase-content"></div>
      </div>

      ${tabellaTripleValue("Strutture Civili", [
        ["Fattoria", ec.Fattoria, "Fattoria_V2.png"], ["Segheria", ec.Segheria, "Segheria_V2.png"], ["Cava", ec.Cava, "CavaDiPietra_V2.png"],
        ["Miniera di Ferro", ec.Miniera_Ferrro, "MinieraFerro_V2.png"], ["Miniera d'Oro", ec.Miniera_Oro, "MinieraOro_V2.png"], ["Abitazioni", ec.Abitazioni, "Abitazioni_V2.png"],
      ], bloccoEdifici)}
      ${tabellaTripleValue("Workshop", [
        ["Spade", wk.Spade, "Workshop_Spade_V2.png"], ["Lance", wk.Lance, "Workshop_Lance_V2.png"], ["Archi", wk.Archi, "Workshop_Archi_V2.png"],
        ["Scudi", wk.Scudi, "Workshop_Scudi_V2.png"], ["Armature", wk.Armature, "Workshop_Armature_V2.png"], ["Frecce", wk.Frecce, "Workshop_Frecce_V2.png"],
      ], bloccoEdifici)}
      ${tabellaTripleValue("Caserme (capacità)", [
        ["Guerrieri", cs.Guerrieri, "Caserma_Guerieri_V2.png"], ["Lancieri", cs.Lanceri, "Caserma_Lanceri_V2.png"],
        ["Arcieri", cs.Arcieri, "Caserma_Arcieri_V2.png"], ["Catapulte", cs.Catapulte, "Caserma_Catapulte_V2.png"],
      ], bloccoEdifici)}

      ${ricercaCivileHtml ? `<div class="report-risorse"><h3>Ricerca Civile</h3><div class="report-risorse__grid">${ricercaCivileHtml}</div></div>` : ""}
      <div class="report-fase"><h3>Ricerca Militare</h3>${tabellaStatUnita(ricM, (u) => CAMPO_SPIA[u.chiave].stats, bloccoRicercaMilitare)}</div>

      <div class="report-fase">
        <h3>Truppe per tier</h3>
        <div class="tier-tabs">${tierTabsHtml}</div>
        <div id="spia-tier-content"></div>
      </div>
      <div class="report-fase"><h3>Bonus Truppe</h3>${tabellaStatUnita(bonus, (u) => CAMPO_SPIA[u.chiave].bonus, bloccoTruppeBonus)}</div>
      ${bonusVillaggioHtml ? `<div class="report-risorse"><h3>Bonus Villaggio</h3><div class="report-risorse__grid">${bonusVillaggioHtml}</div></div>` : ""}

      ${stadio < 6 ? `<p class="target-info">Incrementa la forza dello spionaggio per rivelare i valori "????" e sbloccare maggiori dettagli.</p>` : ""}
      ${s.Precisione_Insufficiente ? `<p class="target-info">Aumenta la forza dello spionaggio per migliorare la precisione dei valori mostrati.</p>` : ""}

      <div class="report-esito ${s.Spionaggio_Riuscito ? "report-esito--vittoria" : "report-esito--sconfitta"}">
        ${s.Spionaggio_Riuscito ? "SPIONAGGIO RIUSCITO" : "SPIONAGGIO FALLITO"}
      </div>`;
  }

  // 16/09/2026, su richiesta dell'utente: elimina un referto dalla propria lista.
  // L'indice è quello dell'ultimo Report_Lista ricevuto (stesso usato per aprire il
  // dettaglio, vedi data-report-index) — il server lo rivalida comunque per intero e
  // range prima di rimuovere davvero (vedi ServerConnection.Elimina_Report). Non tocca
  // l'array "reports" locale: aspetta che il server rimandi il Report_Lista aggiornato
  // (WW.BATTLE.setReports sotto), stessa logica "server-autoritativo" già usata per il
  // resto di questo pannello — nessuno stato ottimistico da tenere allineato.
  function eliminaReport(indice) {
    if (!confirm("Eliminare questo referto? L'operazione non è reversibile.")) return;
    WW.NET.send("Elimina_Report", WW.AUTH.accessToken, indice);
  }

  function apriReportDettaglio(indice) {
    const r = reports[indice];
    if (!r) return;
    const overlay = document.getElementById("report-overlay");
    const content = document.getElementById("report-content");
    const titolo = document.getElementById("report-title");
    if (!overlay || !content) return;

    if (r.Tipo === "Spionaggio" && r.Spionaggio) {
      if (titolo) titolo.textContent = "Resoconto Spionaggio";
      content.innerHTML = templateSpionaggioDettaglio(r.Spionaggio);
      // I contenitori #spia-tier-content/#spia-fase-content vanno popolati DOPO aver
      // messo l'HTML nel DOM (esistono solo da questo momento in poi).
      renderSpiaTierContent();
      renderSpiaFaseContent();
      overlay.hidden = false;
      return;
    }

    if (!r.Battaglia) return;
    const b = r.Battaglia;
    if (titolo) titolo.textContent = "Resoconto Battaglia";

    // Stesso bug del punteggio nella lista report: esito ed esperienza vanno letti dal
    // punto di vista di CHI STA GUARDANDO (attaccante o difensore, confrontando il proprio
    // username), non sempre da quello dell'attaccante — RisultatoBattaglia porta già i dati
    // di entrambi i lati (Vittoria_Attaccante/Xp_Attaccante vs il loro opposto per il
    // difensore: Xp_Difensore esiste in Battaglia.cs, sconfitta dell'attaccante = vittoria
    // del difensore e viceversa).
    const ioAttaccante = b.Nome_Attaccante === (WW.AUTH && WW.AUTH.username);
    const vittoriaMia = ioAttaccante ? b.Vittoria_Attaccante : !b.Vittoria_Attaccante;
    const xpMio = ioAttaccante ? b.Xp_Attaccante : b.Xp_Difensore;

    // Di default mostra il lato di chi sta guardando (comodità: il difensore apre il report
    // e vede subito le proprie truppe, non quelle dell'attaccante) — i due pulsanti restano
    // comunque disponibili per controllare entrambi i lati indipendentemente.
    battagliaAttiva = b;
    battagliaLato = ioAttaccante ? "attaccante" : "difensore";
    battagliaTier = 1; // reset tra un referto e l'altro, vedi Statistiche Truppe Barbare sotto

    content.innerHTML = `
      <div class="report-header">
        <div><strong>Attaccante:</strong> ${b.Nome_Attaccante}<br>Forza: ${WW.fmtInt(b.Forza_Attaccante)} → ${WW.fmtInt(b.Forza_Attaccante_Finale)}</div>
        <div><strong>Difensore:</strong> ${b.Nome_Difensore}<br>Forza: ${WW.fmtInt(b.Forza_Difensore)} → ${WW.fmtInt(b.Forza_Difensore_Finale)}</div>
      </div>
      ${templateBattagliaFasiToggle()}
      <div id="battaglia-fasi-content"></div>
      <div id="battaglia-risorse-content"></div>
      ${b.Tipo_Battaglia === "PVE" ? templateBattagliaStatsBarbaro() : ""}
      <div class="report-esito ${vittoriaMia ? "report-esito--vittoria" : "report-esito--sconfitta"}">
        Esperienza totale guadagnata: ${WW.fmtInt(xpMio)} — ${vittoriaMia ? "VITTORIA!" : "SCONFITTA"}
      </div>`;
    renderBattagliaFasiContent();
    renderBattagliaRisorseContent();
    if (b.Tipo_Battaglia === "PVE") renderBattagliaStatsBarbaroContent();
    overlay.hidden = false;
  }

  /* ---------------------------------------------------------------
     Orchestrazione
     --------------------------------------------------------------- */

  function collegaEventiStatici() {
    const toggleTipo = document.getElementById("barbari-tipo-toggle");
    if (toggleTipo) {
      toggleTipo.addEventListener("click", (e) => {
        const btn = e.target.closest("[data-tipo]");
        if (!btn || btn.dataset.tipo === stato.tipoBarbaro) return; // click sul tipo già attivo: nessun cambiamento
        stato.tipoBarbaro = btn.dataset.tipo;
        toggleTipo.querySelectorAll("[data-tipo]").forEach((b) => b.classList.toggle("is-active", b === btn));
        // 16/09/2026: non si azzera più a [] — si passa subito alla lista già ricevuta
        // dal server per l'altro tipo (vedi nota su cittaGlobaliDati/villaggiPersonaliDati
        // sopra), così la tendina mostra subito i bersagli disponibili invece di
        // "Nessun bersaglio" finché non si preme di nuovo Esplora.
        barbariLista = stato.tipoBarbaro === "Città Barbaro" ? cittaGlobaliDati : villaggiPersonaliDati;
        renderBarbariSelect();
        // NON auto-esplorare qui (tentativo fatto e tolto il 14/09/2026): Esplora resta
        // un'azione esplicita scelta dal giocatore, con "Esplora" da premere a mano —
        // anche ora che il nuovo flusso (Spionaggio.SpionaggioPVE, 16/09/2026) non ha
        // ancora un costo in oro configurato, esplorare in automatico ad ogni cambio
        // Città/Villaggio o ad ogni apertura del pannello resterebbe comunque scorretto
        // (report generati senza una scelta del giocatore) — bug già segnalato dall'utente
        // quando Esplora aveva un costo reale.
      });
    }

    const selectBarbari = document.getElementById("barbari-target-select");
    if (selectBarbari) selectBarbari.addEventListener("change", () => { stato.targetBarbaro = selectBarbari.value; nascondiErroreBarbari(); });

    const selectPvp = document.getElementById("pvp-target-select");
    if (selectPvp) selectPvp.addEventListener("change", () => { stato.targetPvp = selectPvp.value; });

    const btnEsplora = document.getElementById("btn-esplora-barbari");
    if (btnEsplora) btnEsplora.addEventListener("click", esploraBarbaro);

    const btnAttaccaBarbari = document.getElementById("btn-attacca-barbari");
    if (btnAttaccaBarbari) btnAttaccaBarbari.addEventListener("click", attaccaBarbaro);

    const btnAttaccaPvp = document.getElementById("btn-attacca-pvp");
    if (btnAttaccaPvp) btnAttaccaPvp.addEventListener("click", attaccaPvp);

    const listaReport = document.getElementById("battaglia-report-list");
    if (listaReport) listaReport.addEventListener("click", (e) => {
      const btnElimina = e.target.closest("[data-elimina-index]");
      if (btnElimina) {
        eliminaReport(Number(btnElimina.dataset.eliminaIndex));
        return; // non aprire anche il dettaglio del referto
      }
      const riga = e.target.closest("[data-report-index]");
      if (!riga) return;
      apriReportDettaglio(Number(riga.dataset.reportIndex));
    });

    // Pulsanti "‹ Indietro"/"Avanti ›" della lista Report (visibili solo con più
    // di 10 referti — vedi aggiornaPagerReport).
    const btnReportPrev = document.getElementById("report-list-prev");
    if (btnReportPrev) btnReportPrev.addEventListener("click", () => { reportPage--; renderReportLista(); });
    const btnReportNext = document.getElementById("report-list-next");
    if (btnReportNext) btnReportNext.addEventListener("click", () => { reportPage++; renderReportLista(); });

    const btnChiudiReport = document.getElementById("btn-chiudi-report");
    if (btnChiudiReport) btnChiudiReport.addEventListener("click", () => { document.getElementById("report-overlay").hidden = true; });
    const overlay = document.getElementById("report-overlay");
    if (overlay) overlay.addEventListener("click", (e) => { if (e.target === overlay) overlay.hidden = true; });

    // Tab "Truppe per tier"/"Strutture" nel dettaglio Spionaggio: #report-content è un
    // contenitore statico (solo il suo innerHTML cambia ad ogni apertura del modale),
    // quindi un solo listener qui basta per tutte le aperture, senza doverlo riattaccare.
    const reportContent = document.getElementById("report-content");
    if (reportContent) {
      reportContent.addEventListener("click", (e) => {
        const tierBtn = e.target.closest("[data-spia-tier]");
        if (tierBtn) {
          spiaTier = Number(tierBtn.dataset.spiaTier) || 1;
          reportContent.querySelectorAll("[data-spia-tier]").forEach((b) => b.classList.toggle("is-active", b === tierBtn));
          renderSpiaTierContent();
          renderSpiaFaseContent();
          return;
        }
        const faseBtn = e.target.closest("[data-spia-fase]");
        if (faseBtn) {
          spiaFaseIndice = Number(faseBtn.dataset.spiaFase) || 0;
          reportContent.querySelectorAll("[data-spia-fase]").forEach((b) => b.classList.toggle("is-active", b === faseBtn));
          renderSpiaFaseContent();
          return;
        }
        const battagliaTierBtn = e.target.closest("[data-battaglia-tier]");
        if (battagliaTierBtn) {
          battagliaTier = Number(battagliaTierBtn.dataset.battagliaTier) || 1;
          reportContent.querySelectorAll("[data-battaglia-tier]").forEach((b) => b.classList.toggle("is-active", b === battagliaTierBtn));
          renderBattagliaStatsBarbaroContent();
          return;
        }

        const latoBtn = e.target.closest("[data-battaglia-lato]");
        if (latoBtn) {
          battagliaLato = latoBtn.dataset.battagliaLato === "difensore" ? "difensore" : "attaccante";
          reportContent.querySelectorAll("[data-battaglia-lato]").forEach((b) => b.classList.toggle("is-active", b === latoBtn));
          renderBattagliaFasiContent();
          renderBattagliaRisorseContent();
        }
      });
    }

    // Toggle pannelli su mobile (stesso schema di #ricerca-panel-toggle, vedi 09-ricerca.js):
    // il pannello iniziale ("pvp-barbari") va reso visibile via JS all'avvio, non hardcodato
    // nell'HTML — la regola generica ".main-grid > [data-panel]{display:none}" su mobile
    // nasconde TUTTI i data-panel finché non ricevono ".is-visible", "ricerca-generali" compreso.
    const mobileToggle = document.querySelectorAll("#battaglia-panel-toggle .section-toggle__btn");
    const mobilePanels = document.querySelectorAll(".main-grid--pvppve [data-panel]");
    function showBattagliaPanel(target) {
      mobilePanels.forEach((p) => p.classList.toggle("is-visible", p.dataset.panel === target));
    }
    mobileToggle.forEach((btn) => {
      btn.addEventListener("click", () => {
        mobileToggle.forEach((b) => b.classList.toggle("is-active", b === btn));
        showBattagliaPanel(btn.dataset.panelTarget);
      });
    });
    showBattagliaPanel("pvp-barbari");
  }

  // 16/09/2026: aggiorna SEMPRE la lista corrispondente (arrivano entrambe già al
  // login, non solo dopo Esplora — vedi nota su cittaGlobaliDati sopra), e ridisegna
  // la tendina solo se il tipo appena arrivato è quello attualmente selezionato.
  WW.NET.onJson("CittaGlobali", (obj) => {
    cittaGlobaliDati = obj.Dati || [];
    if (stato.tipoBarbaro === "Città Barbaro") { barbariLista = cittaGlobaliDati; renderBarbariSelect(); }
  });
  WW.NET.onJson("VillaggiPersonali", (obj) => {
    villaggiPersonaliDati = obj.Dati || [];
    if (stato.tipoBarbaro === "Villaggio Barbaro") { barbariLista = villaggiPersonaliDati; renderBarbariSelect(); }
  });
  // Errori di Esplora (bersaglio non trovato, oro insufficiente, livello non valido, ecc.)
  // non arrivano più come JSON dedicato ma via "Log_Server|<messaggio>" — già gestito
  // genericamente in 04-game-main.js (WW.NET.on("Log_Server", ...)).
  WW.NET.on("Update_PVP_Player", (args) => {
    const count = Number(args[0]) || 0;
    pvpLista = args.slice(1, 1 + count);
    renderPvpSelect();
  });

  let uiCostruita = false;
  function renderBattaglia() {
    if (!uiCostruita) {
      costruisciEsercitoUI();
      collegaEventiStatici();
      aggiornaPendentiHint(); // stato iniziale (0 truppe selezionate): "Attacca" (Barbari) parte disabilitato
      uiCostruita = true;
    }
    // NON auto-esplorare qui (tentativo fatto e tolto il 14/09/2026, vedi commento in
    // collegaEventiStatici): Esplora resta un'azione esplicita scelta dal giocatore, non
    // qualcosa da far scattare da solo ad ogni apertura del pannello.
    aggiornaEsercitoDisponibili();
    renderPvpSelect();
  }

  WW.renderBattaglia = renderBattaglia;
  WW.BATTLE = {
    setReports(nuoviReports) {
      reports = Array.isArray(nuoviReports) ? nuoviReports : [];
      renderReportLista();
    },
  };
})(window.WW);
