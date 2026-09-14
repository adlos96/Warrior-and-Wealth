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

   Dipende da: WW.GAME (04-game-main.js), WW.fmtInt (00-core.js).
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

  function rigaTesto(label, valore) {
    return `<li class="row-item"><span class="row-item__label">${label}</span><span class="row-item__value">${valore}</span></li>`;
  }

  // --- Stato Attivi: tempo rimanente dei potenziamenti temporanei ---
  const STATISTICHE_ATTIVI = [
    { nome: "VIP", chiave: "vip_Tempo" },
    { nome: "GamePass Silver", chiave: "GamePass_Base_Tempo" },
    { nome: "GamePass Gold", chiave: "GamePass_Avanzato_Tempo" },
    { nome: "Scudo della Pace", chiave: "Scudo_Tempo" },
    { nome: "Costruttori", chiave: "Costruttori_Tempo" },
    { nome: "Reclutatori", chiave: "Reclutatori_Tempo" },
    { nome: "Quest Mensile", chiave: "QuestMensili_Tempo" },
    { nome: "Barbari", chiave: "Barbari_Tempo" },
  ];

  function renderStatisticheAttivi() {
    const ul = document.getElementById("statistiche-attivi-list");
    if (!ul) return;
    ul.innerHTML = STATISTICHE_ATTIVI.map((r) => rigaTesto(r.nome, raw(r.chiave) || "0h 0m 0s")).join("");
  }

  // --- Potenza ---
  const STATISTICHE_POTENZA = [
    { nome: "Edifici", chiave: "Potenza_Strutture" },
    { nome: "Ricerca", chiave: "Potenza_Ricerca" },
    { nome: "Esercito", chiave: "Potenza_Esercito" },
    { nome: "Totale", chiave: "Potenza_Totale" },
  ];

  function renderStatistichePotenza() {
    const ul = document.getElementById("statistiche-potenza-list");
    if (!ul) return;
    ul.innerHTML = STATISTICHE_POTENZA.map((r) => rigaTesto(r.nome, raw(r.chiave))).join("");
  }

  // --- Bonus: generali + per struttura + per unità (4 unità x 3 stat) ---
  const BONUS_GENERALI = [
    { nome: "Costruzione", chiave: "Bonus_Costruzione" },
    { nome: "Addestramento", chiave: "Bonus_Addestramento" },
    { nome: "Ricerca", chiave: "Bonus_Ricerca" },
    { nome: "Riparazione", chiave: "Bonus_Riparazione" },
    { nome: "Produzione Risorse", chiave: "Bonus_Produzione_Risorse" },
    { nome: "Capacità Trasporto", chiave: "Bonus_Capacità_Trasporto" },
  ];
  const BONUS_STRUTTURE = [
    { nome: "Salute Strutture", chiave: "Bonus_Salute_Strutture" },
    { nome: "Difesa Strutture", chiave: "Bonus_Difesa_Strutture" },
    { nome: "Guarnigione Strutture", chiave: "Bonus_Guarnigione_Strutture" },
  ];
  // tipoServer identico a quello già usato in 09-ricerca.js (RICERCA_UNITA):
  // le chiavi Bonus_*_<Nome> lato server usano gli stessi nomi (singolare/
  // irregolare per Lancere/Arcere), qui però le chiavi non cambiano in base
  // al nome — sono fisse ("Guerrieri"/"Lanceri"/"Arceri"/"Catapulte", vedi
  // ClientMessageHandlers.cs case "Bonus_Attacco_Guerrieri" ecc.).
  const BONUS_UNITA = [
    { nome: "Guerrieri", icona: "Guerriero_V2.png" },
    { nome: "Lanceri", icona: "Lanciere_V2.png" },
    { nome: "Arceri", icona: "Arciere_V2.png" },
    { nome: "Catapulte", icona: "Catapulta_V2.png" },
  ];

  function renderStatisticheBonus() {
    const container = document.getElementById("statistiche-bonus-container");
    if (!container) return;
    if (container.children.length) {
      // Già costruito: solo aggiornamento valori (vedi in fondo alla
      // funzione), non serve ricostruire il markup ad ogni tick.
    } else {
      container.innerHTML = `
        <div class="research-esercito-unit">
          <h3 class="panel__subtitle">Generali</h3>
          <ul class="research-list" data-bonus-gruppo="generali">${BONUS_GENERALI.map((r) => rigaTesto(r.nome, "…")).join("")}</ul>
        </div>
        <div class="research-esercito-unit">
          <h3 class="panel__subtitle">Strutture</h3>
          <ul class="research-list" data-bonus-gruppo="strutture">${BONUS_STRUTTURE.map((r) => rigaTesto(r.nome, "…")).join("")}</ul>
        </div>
        ${BONUS_UNITA.map(
          (u) => `
        <div class="research-esercito-unit">
          <h3 class="panel__subtitle"><img class="icon-inline" src="assets/${u.icona}" alt=""> ${u.nome}</h3>
          <ul class="research-list" data-bonus-gruppo="unita-${u.nome}">
            ${rigaTesto("Attacco", "…")}
            ${rigaTesto("Salute", "…")}
            ${rigaTesto("Difesa", "…")}
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
  }

  // --- Statistiche Generali (produzione/costruzione/ricerca) ---
  const STATISTICHE_GENERALI = [
    { nome: "Edifici civili costruiti", chiave: "Strutture_Civili_Costruite" },
    { nome: "Edifici militari costruiti", chiave: "Strutture_Militari_Costruite" },
    { nome: "Caserme costruite", chiave: "Caserme_Costruite" },
    { nome: "Risorse utilizzate", chiave: "Risorse_Utilizzate" },
    // Etichette corrette (14/09/2026, su segnalazione dell'utente): le chiavi
    // si chiamano "..._Risparmiato" ma il valore che il server manda NON è
    // tempo risparmiato — è il tempo EFFETTIVO passato dal giocatore con
    // una costruzione/addestramento/ricerca attiva (vedi Server.cs, il loop
    // che fa Tempo_Costruzione++/Tempo_Addestramento++/Tempo_Ricerca++ una
    // volta al secondo finché c'è una coda in corso). Il vero "tempo
    // risparmiato" grazie ai diamanti è tutt'altra riga qui sotto,
    // "Tempo_Sottratto_Diamanti" — da cui probabilmente il nome sbagliato
    // copiato per queste tre. Le chiavi del protocollo restano invariate
    // (già usate anche dal client desktop), cambia solo l'etichetta mostrata.
    { nome: "Tempo addestramento effettivo", chiave: "Tempo_Addestramento_Risparmiato", raw: true },
    { nome: "Tempo costruzione effettivo", chiave: "Tempo_Costruzione_Risparmiato", raw: true },
    { nome: "Tempo ricerca effettivo", chiave: "Tempo_Ricerca_Risparmiato", raw: true },
    { nome: "Tempo sottratto (Diamanti)", chiave: "Tempo_Sottratto_Diamanti", raw: true },
    { nome: "Frecce utilizzate", chiave: "Frecce_Utilizzate" },
    { nome: "Danno HP Barbari", chiave: "Danno_HP_Barbaro" },
    { nome: "Danno DEF Barbari", chiave: "Danno_DEF_Barbaro" },
    { nome: "Quest completate", chiave: "Quest_Completate" },
  ];

  function renderStatisticheGenerali() {
    const ul = document.getElementById("statistiche-generali-list");
    if (!ul) return;
    ul.innerHTML = STATISTICHE_GENERALI.map((r) => rigaTesto(r.nome, r.raw ? raw(r.chiave) : num(r.chiave))).join("");
  }

  // --- Guerra e Razzie ---
  const STATISTICHE_GUERRA = [
    { nome: "Risorse razziate", chiave: "Risorse_Razziate" },
    { nome: "Barbari sconfitti", chiave: "Barbari_Sconfitti" },
    { nome: "Battaglie vinte", chiave: "Battaglie_Vinte" },
    { nome: "Battaglie perse", chiave: "Battaglie_Perse" },
    { nome: "Attacchi effettuati (PVP)", chiave: "Attacchi_Effettuati_PVP" },
    { nome: "Attacchi subiti (PVP)", chiave: "Attacchi_Subiti_PVP" },
    { nome: "Accampamenti sconfitti", chiave: "Accampamenti_Barbari_Sconfitti" },
    { nome: "Città sconfitte", chiave: "Città_Barbare_Sconfitte" },
  ];

  function renderStatisticheGuerra() {
    const ul = document.getElementById("statistiche-guerra-list");
    if (!ul) return;
    ul.innerHTML = STATISTICHE_GUERRA.map((r) => rigaTesto(r.nome, num(r.chiave))).join("");
  }

  // --- Unità: addestrate/eliminate/perse, totale + per tipo ---
  function renderStatisticheUnita() {
    const container = document.getElementById("statistiche-unita-container");
    if (!container) return;
    if (!container.children.length) {
      container.innerHTML = `
        <ul class="research-list" data-unita-gruppo="totali">
          ${rigaTesto("Unità addestrate", "…")}
          ${rigaTesto("Unità eliminate", "…")}
          ${rigaTesto("Unità perse", "…")}
        </ul>
        <div class="research-esercito-unit">
          <h3 class="panel__subtitle">Eliminate per tipo</h3>
          <ul class="research-list" data-unita-gruppo="eliminate">
            ${BONUS_UNITA.map((u) => rigaTesto(u.nome, "…")).join("")}
          </ul>
        </div>
        <div class="research-esercito-unit">
          <h3 class="panel__subtitle">Perse per tipo</h3>
          <ul class="research-list" data-unita-gruppo="perse">
            ${BONUS_UNITA.map((u) => rigaTesto(u.nome, "…")).join("")}
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
  }

  function renderStatistiche() {
    renderStatisticheAttivi();
    renderStatistichePotenza();
    renderStatisticheBonus();
    renderStatisticheGenerali();
    renderStatisticheGuerra();
    renderStatisticheUnita();
  }

  WW.renderStatistiche = renderStatistiche;
})(window.WW);
