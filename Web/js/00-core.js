/* ==========================================================
   Warrior & Wealth — Web Client — 00-core.js
   ----------------------------------------------------------
   Utilità di base condivise da tutti gli altri file (namespace
   globale WW, formattazione numeri, localStorage sicuro). Va
   caricato PRIMA di tutti gli altri script del client web.

   Perché un namespace condiviso (WW) e non tanti file separati
   con "import"/"export": i vari <script> di questa cartella non
   sono moduli ES (niente type="module"), quindi condividono lo
   stesso scope globale. Invece di lasciare tutto sparso su
   "window", ogni file mette le proprie funzioni/dati dentro
   window.WW (es. WW.NET, WW.GAME, WW.fmtInt...): gli altri file,
   caricati dopo, li leggono da lì. L'ordine di caricamento nel
   <body> di index.html è quindi importante — vedi il commento in
   fondo a 10-main.js.
   ========================================================== */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  /* ---------- FORMATTAZIONE NUMERI (localizzata) ----------
     I numeri NON vanno mai scritti "a mano" nell'HTML (es. "30.000"):
     il punto come separatore delle migliaia è la convenzione italiana/
     tedesca, ma non quella americana/inglese (dove è la virgola, con il
     punto per i decimali — es. 30,000 invece di 30.000). Teniamo solo il
     valore NUMERICO grezzo e lo formattiamo con Intl.NumberFormat usando
     la lingua/paese del dispositivo di chi sta giocando. */
  const numberLocale = navigator.language || "it-IT";

  function fmtInt(n) {
    return new Intl.NumberFormat(numberLocale).format(n);
  }
  function fmtDecimal(n, decimals) {
    return new Intl.NumberFormat(numberLocale, {
      minimumFractionDigits: decimals,
      maximumFractionDigits: decimals,
    }).format(n);
  }

  /* ---------- localStorage sicuro ----------
     Wrapper che non fa mai esplodere il resto del codice se lo storage
     non è disponibile (modalità privata, policy del browser, ecc.). */
  const storage = {
    get(key) {
      try { return localStorage.getItem(key); } catch (e) { return null; }
    },
    set(key, value) {
      try { localStorage.setItem(key, value); } catch (e) { /* ignora */ }
    },
    remove(key) {
      try { localStorage.removeItem(key); } catch (e) { /* ignora */ }
    },
  };

  /* ---------- Passo degli stepper +/- (qty-btn) ----------
     Su richiesta dell'utente (13/09/2026): Shift+click = ±5, Ctrl+click =
     ±10, click semplice = ±1 — uguale per TUTTI gli stepper dell'app
     (Costruzione, Reclutamento, Scambio, Velocizza, Sposta Truppe in
     Città...). Un'unica funzione condivisa invece di ripetere la stessa
     logica in ogni file: ognuno dei punti che gestisce un click su
     ".qty-btn" la usa per calcolare di quanto muovere il valore, invece
     di sommare/sottrarre sempre e solo 1. */
  function qtyStepDelta(event) {
    if (event && event.ctrlKey) return 10;
    if (event && event.shiftKey) return 5;
    return 1;
  }

  /* ---------- Passo dello stepper nelle schermate "Velocizza con Diamanti Blu" ----------
     Su richiesta dell'utente (22/09/2026): stessi passi di qtyStepDelta sopra, con
     l'aggiunta di Ctrl+Shift+click = ±50 — riservato alle schermate di velocizzazione
     (Costruzione, Reclutamento/addestramento, Ricerca) perché lì le quantità in gioco sono
     tipicamente molto più alte che negli altri stepper dell'app (Scambio Diamanti, Sposta
     Truppe...), che restano quindi su qtyStepDelta invariata. */
  function qtyStepDeltaVelocizza(event) {
    if (event && event.ctrlKey && event.shiftKey) return 50;
    return qtyStepDelta(event);
  }

  /* ---------- Escape per selettori CSS dinamici ----------
     Usata ovunque un valore arrivato dal server (una chiave di
     descrizione, ecc.) finisce dentro un selettore `[data-x="..."]`:
     senza, un valore con virgolette o backslash romperebbe il
     selettore. Era duplicata identica in 06-costruzione.js/08-shop.js/
     09-ricerca.js: centralizzata qui. */
  function cssEscape(str) {
    return window.CSS && CSS.escape ? CSS.escape(str) : str.replace(/["\\]/g, "\\$&");
  }

  /* ---------- Stima "tempo > 0" da una stringa già formattata ----------
     Su una stringa di tempo già formattata dal server ("2h 0m 0s",
     "45s", "hh:mm:ss"...), somma tutti i numeri che contiene per capire
     se rappresenta più di zero secondi: non serve un valore esatto, solo
     se mostrare o no una riga/pulsante. Era duplicata identica in
     04-game-main.js/09-ricerca.js: centralizzata qui. */
  function tempoMaggioreDiZero(str) {
    if (!str) return false;
    const numeri = str.match(/\d+/g);
    if (!numeri) return false;
    return numeri.some((n) => Number(n) > 0);
  }

  /* ---------- WIDGET "ESERCITO" (tab tier I-V + stepper quantità per unità) ----------
     22/09/2026, su richiesta dell'utente ("riutilizzare la schermata Esercito da
     Inviare"): il markup/comportamento (tab tier, stepper +/-, "disponibili" letto da
     WW.GAME) era duplicato quasi identico in 14-battaglia.js ("Esercito da Inviare",
     Barbari+PVP) e 16-raduni.js (form truppe del raduno) — centralizzato qui, stesso
     pattern di cssEscape/tempoMaggioreDiZero sopra. Lo STATO resta volutamente separato
     tra i due (scelta confermata dall'utente): ogni chiamante passa il proprio oggetto
     "stato" ({tier, quantita: {1:{g,l,a,c}, ..., 5:{...}}}), così le truppe preparate per
     un attacco singolo non si mischiano mai con quelle destinate a un raduno.
     opts:
       tabsId, listaId — id degli elementi <div>/<ul> da popolare (uno per chiamante)
       unita           — array [{nome, icona, chiave, campoServer}, ...] (4 unità)
       stato           — oggetto {tier, quantita} del chiamante, mutato in-place
       onChange        — richiamata (senza argomenti) dopo ogni variazione di quantità
                          o cambio tab tier, per aggiornare hint/bottoni propri del
                          chiamante (es. "Attacca"/"Conferma" disabilitato a 0 truppe)
     Ritorna { build, refreshStepper, refreshDisponibili, reset, totale }. */
  const TIER_LABELS_ESERCITO = ["I", "II", "III", "IV", "V"];

  function templateRigaEsercito(u) {
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

  function creaEsercitoWidget(opts) {
    const { tabsId, listaId, unita, stato, onChange } = opts;
    const notifica = () => { if (typeof onChange === "function") onChange(); };

    function refreshStepper() {
      const lista = document.getElementById(listaId);
      if (!lista) return;
      const q = stato.quantita[stato.tier];
      unita.forEach((u) => {
        const el = lista.querySelector(`[data-unit-stepper="${u.chiave}"] .qty-stepper__value`);
        if (el) el.textContent = String(q[u.chiave]);
      });
    }

    function refreshDisponibili() {
      const lista = document.getElementById(listaId);
      if (!lista) return;
      unita.forEach((u) => {
        const el = lista.querySelector(`[data-disponibili="${u.chiave}"]`);
        if (el) el.textContent = fmtInt(WW.GAME.num(`${u.campoServer}_${stato.tier}`));
      });
    }

    function reset() {
      TIER_LABELS_ESERCITO.forEach((_, i) => (stato.quantita[i + 1] = { g: 0, l: 0, a: 0, c: 0 }));
      refreshStepper();
      notifica();
    }

    function totale() {
      return Object.values(stato.quantita).reduce((tot, q) => tot + q.g + q.l + q.a + q.c, 0);
    }

    function build() {
      const tabs = document.getElementById(tabsId);
      const lista = document.getElementById(listaId);
      if (!tabs || !lista || lista.children.length === unita.length) return;

      tabs.innerHTML = TIER_LABELS_ESERCITO.map((l, i) => `<button type="button" class="tier-btn${i === 0 ? " is-active" : ""}" data-tier="${i + 1}">${l}</button>`).join("");
      lista.innerHTML = unita.map(templateRigaEsercito).join("");

      tabs.addEventListener("click", (e) => {
        const btn = e.target.closest(".tier-btn");
        if (!btn) return;
        stato.tier = Number(btn.dataset.tier) || 1;
        tabs.querySelectorAll(".tier-btn").forEach((b) => b.classList.toggle("is-active", b === btn));
        refreshStepper();
        refreshDisponibili();
        notifica();
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
        notifica();
      });
    }

    return { build, refreshStepper, refreshDisponibili, reset, totale };
  }

  WW.fmtInt = fmtInt;
  WW.fmtDecimal = fmtDecimal;
  WW.storage = storage;
  WW.qtyStepDelta = qtyStepDelta;
  WW.qtyStepDeltaVelocizza = qtyStepDeltaVelocizza;
  WW.cssEscape = cssEscape;
  WW.tempoMaggioreDiZero = tempoMaggioreDiZero;
  WW.creaEsercitoWidget = creaEsercitoWidget;
})(window.WW);
