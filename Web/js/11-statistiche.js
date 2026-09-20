/* ==========================================================
   Warrior & Wealth — Web Client — 11-statistiche.js
   ----------------------------------------------------------
   SCHERMATA STATISTICHE — rispecchia la schermata desktop
   "Statistiche" (GUI/Statistiche.cs): stato dei potenziamenti
   temporanei attivi, Potenza, Bonus percentuali, statistiche di
   produzione/costruzione e statistiche di guerra/unità.

   Nessun comando da inviare: sono tutti valori già presenti in
   WW.GAME.raw, mandati dal server con lo stesso "tick" di ogni
   altra schermata (PlayerSnapshot via Update_Data) — le chiavi
   qui sotto sono le stesse identiche lette dal client desktop in
   Variabili_Client.Utente/Statistiche/Bonus, prese da
   ClientMessageHandlers.cs (case "vip_Tempo", "Potenza_Totale",
   "Bonus_Costruzione", ecc.), non inventate.

   Bonus/Potenza mostrati con GAME.raw() grezzo, non GAME.num()+
   fmtInt: il client desktop li stampa così come arrivano dal
   server (Statistiche.cs, semplice interpolazione di stringa)
   senza riformattarli — potrebbero già includere un "%" o un
   formato che fmtInt romperebbe. I contatori (Statistiche
   Generali/Guerra/Unità) sono invece numeri interi puri: per
   quelli usiamo GAME.num()+fmtInt come nel resto dell'app, per
   coerenza visiva (separatore delle migliaia nella lingua di chi
   gioca) — unica differenza rispetto al testo grezzo del desktop.

   Localizzazione: stesso meccanismo "labelKey" già usato in 04-game-main.js/06-
   costruzione.js/07-citta.js/09-ricerca.js — WW.descrizioni[chiave]
   se presente, altrimenti fallback italiano hardcoded. Le chiavi
   già mandate dal server per altre schermate vengono RIUSATE qui
   pari pari (Guerrieri/Lanceri/Arceri/Catapulte, Attacco/Salute/
   Difesa, Costruzione/Addestramento/Ricerca): nessun nuovo Send()
   richiesto per quelle. Tutte le altre etichette di questa
   schermata sono nuove — vedi elenco "Label *" mancanti segnalato
   all'utente in chat, da aggiungere in Descrizioni.cs/ITA.cs/ENG.cs.
   Le liste renderStatisticheAttivi/Potenza/Generali/Guerra vengono
   ricostruite per intero ad OGNI tick (nessun guard su children.
   length), quindi lì la label si rilegge da sola: labelKey nella
   funzione rigaTesto() è sufficiente, senza bisogno di un
   WW.onDescrizione dedicato. renderStatisticheBonus/Unita invece
   costruiscono il markup una sola volta (per non perdere altro
   stato) — lì le label vengono riapplicate ad ogni tick tramite
   [data-label-per], stesso principio già usato in aggiornaCittaCard
   (07-citta.js) per le stesse ragioni.

   Dipende da: WW.GAME (04-game-main.js), WW.fmtInt (00-core.js),
   WW.descrizioni (04-game-main.js).
   Esporta: WW.renderStatistiche — usata da renderAllFromServer in
   04-game-main.js. */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  function num(chiave) {
    return WW.fmtInt(WW.GAME.num(chiave));
  }
  function raw(chiave) {
    const v = WW.GAME.raw[chiave];
    return v === undefined || v === null || v === "" ? "0" : v;
  }
  // Nome mostrato per una riga con labelKey opzionale: stesso fallback usato
  // nelle altre schermate — italiano hardcoded finché il server non manda
  // la Descrizione corrispondente.
  function nomeLabel(nome, labelKey) {
    return (labelKey && WW.descrizioni[labelKey]) || nome;
  }

  function rigaTesto(label, valore, labelKey) {
    const labelAttr = labelKey ? ` data-label-per="${labelKey}"` : "";
    return `<li class="row-item"><span class="row-item__label"${labelAttr}>${nomeLabel(label, labelKey)}</span><span class="row-item__value">${valore}</span></li>`;
  }

  // Rilegge tutte le [data-label-per] dentro un container e le riapplica —
  // usata dalle liste "costruite una sola volta" (Bonus/Unità sotto), che
  // altrimenti resterebbero bloccate sul fallback italiano se la Descrizione
  // arriva dal server DOPO la prima costruzione del markup.
  function riapplicaLabel(container) {
    container.querySelectorAll("[data-label-per]").forEach((el) => {
      const testo = WW.descrizioni[el.dataset.labelPer];
      if (testo) el.textContent = testo;
    });
  }

  // --- Stato Attivi: tempo rimanente dei potenziamenti temporanei ---
  const STATISTICHE_ATTIVI = [
    { nome: "VIP", chiave: "vip_Tempo", labelKey: "Label VIP" },
    { nome: "GamePass Silver", chiave: "GamePass_Base_Tempo", labelKey: "Label GamePass Silver" },
    { nome: "GamePass Gold", chiave: "GamePass_Avanzato_Tempo", labelKey: "Label GamePass Gold" },
    { nome: "Scudo della Pace", chiave: "Scudo_Tempo", labelKey: "Label Scudo Pace" },
    { nome: "Costruttori", chiave: "Costruttori_Tempo", labelKey: "Label Costruttori" },
    { nome: "Reclutatori", chiave: "Reclutatori_Tempo", labelKey: "Label Reclutatori" },
    { nome: "Quest Mensile", chiave: "QuestMensili_Tempo", labelKey: "Label Quest Mensile" },
    { nome: "Barbari", chiave: "Barbari_Tempo", labelKey: "Label Barbari" },
  ];

  function renderStatisticheAttivi() {
    const ul = document.getElementById("statistiche-attivi-list");
    if (!ul) return;
    ul.innerHTML = STATISTICHE_ATTIVI.map((r) => rigaTesto(r.nome, raw(r.chiave) || "0h 0m 0s", r.labelKey)).join("");
  }

  // --- Potenza ---
  const STATISTICHE_POTENZA = [
    { nome: "Edifici", chiave: "Potenza_Strutture", labelKey: "Label Edifici" },
    { nome: "Ricerca", chiave: "Potenza_Ricerca", labelKey: "Label Ricerca" }, // riusa la Label già mandata per la tab-bar/schermata Ricerca
    { nome: "Esercito", chiave: "Potenza_Esercito", labelKey: "Label Esercito" },
    { nome: "Totale", chiave: "Potenza_Totale", labelKey: "Label Totale" },
  ];

  function renderStatistichePotenza() {
    const ul = document.getElementById("statistiche-potenza-list");
    if (!ul) return;
    ul.innerHTML = STATISTICHE_POTENZA.map((r) => rigaTesto(r.nome, raw(r.chiave), r.labelKey)).join("");
  }

  // --- Bonus: generali + per struttura + per unità (4 unità x 3 stat) ---
  const BONUS_GENERALI = [
    { nome: "Costruzione", chiave: "Bonus_Costruzione", labelKey: "Label Costruzione" }, // riusa Costruzione/06-costruzione.js
    { nome: "Addestramento", chiave: "Bonus_Addestramento", labelKey: "Label Addestramento" }, // riusa Costruzione/06-costruzione.js
    { nome: "Ricerca", chiave: "Bonus_Ricerca", labelKey: "Label Ricerca" }, // riusa 09-ricerca.js
    { nome: "Riparazione", chiave: "Bonus_Riparazione", labelKey: "Label Riparazione" },
    { nome: "Produzione Risorse", chiave: "Bonus_Produzione_Risorse", labelKey: "Label Produzione Risorse" },
    { nome: "Capacità Trasporto", chiave: "Bonus_Capacità_Trasporto", labelKey: "Label Capacità Trasporto" },
  ];
  const BONUS_STRUTTURE = [
    { nome: "Salute Strutture", chiave: "Bonus_Salute_Strutture", labelKey: "Label Salute Strutture" },
    { nome: "Difesa Strutture", chiave: "Bonus_Difesa_Strutture", labelKey: "Label Difesa Strutture" },
    { nome: "Guarnigione Strutture", chiave: "Bonus_Guarnigione_Strutture", labelKey: "Label Guarnigione Strutture" },
  ];
  // tipoServer identico a quello già usato in 09-ricerca.js (RICERCA_UNITA):
  // le chiavi Bonus_*_<Nome> lato server usano gli stessi nomi (singolare/
  // irregolare per Lancere/Arcere), qui però le chiavi non cambiano in base
  // al nome — sono fisse ("Guerrieri"/"Lanceri"/"Arceri"/"Catapulte", vedi
  // ClientMessageHandlers.cs case "Bonus_Attacco_Guerrieri" ecc.). labelKey
  // riusa le stesse Label già mandate per le unità in Città/Costruzione.
  const BONUS_UNITA = [
    { nome: "Guerrieri", icona: "Guerriero_V2.png", labelKey: "Label Guerrieri" },
    { nome: "Lanceri", icona: "Lanciere_V2.png", labelKey: "Label Lanceri" },
    { nome: "Arceri", icona: "Arciere_V2.png", labelKey: "Label Arceri" },
    { nome: "Catapulte", icona: "Catapulta_V2.png", labelKey: "Label Catapulte" },
  ];

  function renderStatisticheBonus() {
    const container = document.getElementById("statistiche-bonus-container");
    if (!container) return;
    if (container.children.length) {
      // Già costruito: solo aggiornamento valori/label (vedi in fondo alla
      // funzione), non serve ricostruire il markup ad ogni tick.
    } else {
      container.innerHTML = `
        <div class="research-esercito-unit">
          <h3 class="panel__subtitle" data-label-per="Label Bonus Generali">${nomeLabel("Generali", "Label Bonus Generali")}</h3>
          <ul class="research-list" data-bonus-gruppo="generali">${BONUS_GENERALI.map((r) => rigaTesto(r.nome, "…", r.labelKey)).join("")}</ul>
        </div>
        <div class="research-esercito-unit">
          <h3 class="panel__subtitle" data-label-per="Label Bonus Strutture">${nomeLabel("Strutture", "Label Bonus Strutture")}</h3>
          <ul class="research-list" data-bonus-gruppo="strutture">${BONUS_STRUTTURE.map((r) => rigaTesto(r.nome, "…", r.labelKey)).join("")}</ul>
        </div>
        ${BONUS_UNITA.map(
          (u) => `
        <div class="research-esercito-unit">
          <h3 class="panel__subtitle"><img class="icon-inline" src="assets/${u.icona}" alt=""> <span data-label-per="${u.labelKey}">${nomeLabel(u.nome, u.labelKey)}</span></h3>
          <ul class="research-list" data-bonus-gruppo="unita-${u.nome}">
            ${rigaTesto("Attacco", "…", "Label Attacco")}
            ${rigaTesto("Salute", "…", "Label Salute")}
            ${rigaTesto("Difesa", "…", "Label Difesa")}
          </ul>
        </div>`
        ).join("")}`;
    }

    function aggiornaGruppo(selettore, righe, valori) {
      const ul = container.querySelector(selettore);
      if (!ul) return;
      const li = ul.querySelectorAll(".row-item__value");
      valori.forEach((v, i) => { if (li[i]) li[i].textContent = v; });
    }

    aggiornaGruppo('[data-bonus-gruppo="generali"]', BONUS_GENERALI, BONUS_GENERALI.map((r) => raw(r.chiave)));
    aggiornaGruppo('[data-bonus-gruppo="strutture"]', BONUS_STRUTTURE, BONUS_STRUTTURE.map((r) => raw(r.chiave)));
    BONUS_UNITA.forEach((u) => {
      aggiornaGruppo(`[data-bonus-gruppo="unita-${u.nome}"]`, null, [
        raw(`Bonus_Attacco_${u.nome}`),
        raw(`Bonus_Salute_${u.nome}`),
        raw(`Bonus_Difesa_${u.nome}`),
      ]);
    });
    riapplicaLabel(container);
  }

  // --- Statistiche Generali (produzione/costruzione/ricerca) ---
  const STATISTICHE_GENERALI = [
    { nome: "Edifici civili costruiti", chiave: "Strutture_Civili_Costruite", labelKey: "Label Edifici Civili Costruiti" },
    { nome: "Edifici militari costruiti", chiave: "Strutture_Militari_Costruite", labelKey: "Label Edifici Militari Costruiti" },
    { nome: "Caserme costruite", chiave: "Caserme_Costruite", labelKey: "Label Caserme Costruite" },
    { nome: "Risorse utilizzate", chiave: "Risorse_Utilizzate", labelKey: "Label Risorse Utilizzate" },
    // Le chiavi si chiamano "..._Risparmiato" ma il valore che il server manda NON è
    // tempo risparmiato — è il tempo EFFETTIVO passato dal giocatore con
    // una costruzione/addestramento/ricerca attiva (vedi Server.cs, il loop
    // che fa Tempo_Costruzione++/Tempo_Addestramento++/Tempo_Ricerca++ una
    // volta al secondo finché c'è una coda in corso). Il vero "tempo
    // risparmiato" grazie ai diamanti è tutt'altra riga qui sotto,
    // "Tempo_Sottratto_Diamanti" — da cui probabilmente il nome sbagliato
    // copiato per queste tre. Le chiavi del protocollo restano invariate
    // (già usate anche dal client desktop), cambia solo l'etichetta mostrata.
    { nome: "Tempo addestramento effettivo", chiave: "Tempo_Addestramento_Risparmiato", raw: true, labelKey: "Label Tempo Addestramento Effettivo" },
    { nome: "Tempo costruzione effettivo", chiave: "Tempo_Costruzione_Risparmiato", raw: true, labelKey: "Label Tempo Costruzione Effettivo" },
    { nome: "Tempo ricerca effettivo", chiave: "Tempo_Ricerca_Risparmiato", raw: true, labelKey: "Label Tempo Ricerca Effettivo" },
    { nome: "Tempo sottratto (Diamanti)", chiave: "Tempo_Sottratto_Diamanti", raw: true, labelKey: "Label Tempo Sottratto Diamanti" },
    { nome: "Frecce utilizzate", chiave: "Frecce_Utilizzate", labelKey: "Label Frecce Utilizzate" },
    { nome: "Danno HP Barbari", chiave: "Danno_HP_Barbaro", labelKey: "Label Danno HP Barbari" },
    { nome: "Danno DEF Barbari", chiave: "Danno_DEF_Barbaro", labelKey: "Label Danno DEF Barbari" },
    { nome: "Quest completate", chiave: "Quest_Completate", labelKey: "Label Quest Completate" },
  ];

  function renderStatisticheGenerali() {
    const ul = document.getElementById("statistiche-generali-list");
    if (!ul) return;
    ul.innerHTML = STATISTICHE_GENERALI.map((r) => rigaTesto(r.nome, r.raw ? raw(r.chiave) : num(r.chiave), r.labelKey)).join("");
  }

  // --- Guerra e Razzie ---
  const STATISTICHE_GUERRA = [
    { nome: "Risorse razziate", chiave: "Risorse_Razziate", labelKey: "Label Risorse Razziate" },
    { nome: "Barbari sconfitti", chiave: "Barbari_Sconfitti", labelKey: "Label Barbari Sconfitti" },
    { nome: "Battaglie vinte", chiave: "Battaglie_Vinte", labelKey: "Label Battaglie Vinte" },
    { nome: "Battaglie perse", chiave: "Battaglie_Perse", labelKey: "Label Battaglie Perse" },
    { nome: "Attacchi effettuati (PVP)", chiave: "Attacchi_Effettuati_PVP", labelKey: "Label Attacchi Effettuati PVP" },
    { nome: "Attacchi subiti (PVP)", chiave: "Attacchi_Subiti_PVP", labelKey: "Label Attacchi Subiti PVP" },
    { nome: "Accampamenti sconfitti", chiave: "Accampamenti_Barbari_Sconfitti", labelKey: "Label Accampamenti Sconfitti" },
    { nome: "Città sconfitte", chiave: "Città_Barbare_Sconfitte", labelKey: "Label Città Sconfitte" },
  ];

  function renderStatisticheGuerra() {
    const ul = document.getElementById("statistiche-guerra-list");
    if (!ul) return;
    ul.innerHTML = STATISTICHE_GUERRA.map((r) => rigaTesto(r.nome, num(r.chiave), r.labelKey)).join("");
  }

  // --- Unità: addestrate/eliminate/perse, totale + per tipo ---
  function renderStatisticheUnita() {
    const container = document.getElementById("statistiche-unita-container");
    if (!container) return;
    if (!container.children.length) {
      container.innerHTML = `
        <ul class="research-list" data-unita-gruppo="totali">
          ${rigaTesto("Unità addestrate", "…", "Label Unità Addestrate")}
          ${rigaTesto("Unità eliminate", "…", "Label Unità Eliminate")}
          ${rigaTesto("Unità perse", "…", "Label Unità Perse")}
        </ul>
        <div class="research-esercito-unit">
          <h3 class="panel__subtitle" data-label-per="Label Eliminate Per Tipo">${nomeLabel("Eliminate per tipo", "Label Eliminate Per Tipo")}</h3>
          <ul class="research-list" data-unita-gruppo="eliminate">
            ${BONUS_UNITA.map((u) => rigaTesto(u.nome, "…", u.labelKey)).join("")}
          </ul>
        </div>
        <div class="research-esercito-unit">
          <h3 class="panel__subtitle" data-label-per="Label Perse Per Tipo">${nomeLabel("Perse per tipo", "Label Perse Per Tipo")}</h3>
          <ul class="research-list" data-unita-gruppo="perse">
            ${BONUS_UNITA.map((u) => rigaTesto(u.nome, "…", u.labelKey)).join("")}
          </ul>
        </div>`;
    }
    function aggiornaGruppo(selettore, valori) {
      const ul = container.querySelector(selettore);
      if (!ul) return;
      const li = ul.querySelectorAll(".row-item__value");
      valori.forEach((v, i) => { if (li[i]) li[i].textContent = v; });
    }
    aggiornaGruppo('[data-unita-gruppo="totali"]', [num("Unità_Addestrate"), num("Unità_Eliminate"), num("Unità_Perse")]);
    aggiornaGruppo('[data-unita-gruppo="eliminate"]', BONUS_UNITA.map((u) => num(`${u.nome}_Eliminate`)));
    // Chiave server "<Nome>_Persi" (non "_Perse") per le 4 unità, verificato
    // in ClientMessageHandlers.cs — "Unità_Perse" (il totale, sopra) invece
    // usa "Perse". Due parole diverse lato server, non un refuso qui.
    aggiornaGruppo('[data-unita-gruppo="perse"]', BONUS_UNITA.map((u) => num(`${u.nome}_Persi`)));
    riapplicaLabel(container);
  }

  // Titoli dei 6 pannelli + pulsante "Statistiche" nella tab-bar in basso
  // (quest'ultimo riusa "Label Statistiche", già mandata dal server per
  // altre schermate — stesso principio di "tab-btn-costruzione"/"tab-btn-
  // ricerca": il bottone vive fuori da questo tab-panel, quindi va
  // aggiornato qui, non ricostruito da un [data-label-per] scoped altrove).
  // Rilettura ad ogni tick, nessun WW.onDescrizione dedicato necessario.
  function aggiornaTitoliStatistiche() {
    [
      ["statistiche-attivi-title", "Label Stato Attivi"],
      ["statistiche-potenza-title", "Label Potenza"],
      ["statistiche-bonus-title", "Label Bonus"],
      ["statistiche-generali-title", "Label Statistiche Generali"],
      ["statistiche-guerra-title", "Label Guerra e Razzie"],
      ["statistiche-unita-title", "Label Unità"],
      ["tab-btn-statistiche", "Label Statistiche"],
    ].forEach(([id, chiave]) => {
      const el = document.getElementById(id);
      const testo = WW.descrizioni[chiave];
      if (el && testo) el.textContent = testo;
    });
  }

  function renderStatistiche() {
    aggiornaTitoliStatistiche();
    renderStatisticheAttivi();
    renderStatistichePotenza();
    renderStatisticheBonus();
    renderStatisticheGenerali();
    renderStatisticheGuerra();
    renderStatisticheUnita();
  }

  WW.renderStatistiche = renderStatistiche;
})(window.WW);
