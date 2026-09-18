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

  WW.fmtInt = fmtInt;
  WW.fmtDecimal = fmtDecimal;
  WW.storage = storage;
  WW.qtyStepDelta = qtyStepDelta;
})(window.WW);
