/* ==========================================================
   Warrior & Wealth — Web Client — 05-scambio.js
   ----------------------------------------------------------
   SCAMBIO E VELOCIZZAZIONE (Diamanti) — piccoli form "a comparsa",
   stesso contenuto delle finestre dedicate del client desktop
   (Scambia_DiamantiViola.cs / Scambia_Tributi.cs / Velocizza.cs),
   qui inline sotto al pulsante che li apre. Uno stepper comune
   (creaStepperSemplice) gestisce +/- e tiene lo stato in memoria,
   visto che qui serve anche un "risultato" calcolato in tempo
   reale mentre si cambia la quantità.

   WW.VELOCIZZA_STEPPERS è usato da 04-game-main.js
   (aggiornaTempoECodaVelocizza) per azzerare davvero lo stepper
   (stepper.set(0)) quando il pulsante Velocizza sparisce perché
   non c'è più tempo in coda — non solo "ripulire" il testo a
   video, che lascerebbe lo stato interno disallineato. */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  function creaStepperSemplice(containerId, onChange) {
    const el = document.getElementById(containerId);
    if (!el) return null;
    const valueEl = el.querySelector(".qty-stepper__value");
    let valore = 0;
    function set(v) {
      valore = Math.max(0, v);
      valueEl.textContent = String(valore);
      onChange(valore);
    }
    // Shift/Ctrl+click = passo più grande (WW.qtyStepDelta, 00-core.js),
    // uguale per tutti gli stepper dell'app — richiesto dall'utente 13/09/2026.
    el.querySelector(".qty-btn--minus").addEventListener("click", (e) => set(valore - WW.qtyStepDelta(e)));
    el.querySelector(".qty-btn--plus").addEventListener("click", (e) => set(valore + WW.qtyStepDelta(e)));
    return { get: () => valore, set };
  }

  // Mostra/nasconde un mini-form al click del pulsante che lo attiva,
  // azzerando lo stepper quando si richiude (per non lasciare in giro una
  // quantità "dimenticata" da un utilizzo precedente).
  function collegaToggleMiniForm(bottoneId, formId, stepper) {
    const bottone = document.getElementById(bottoneId);
    const form = document.getElementById(formId);
    if (!bottone || !form) return;
    bottone.addEventListener("click", () => {
      form.hidden = !form.hidden;
      if (form.hidden && stepper) stepper.set(0);
    });
  }

  // --- Scambia Diamanti Viola -> Diamanti Blu ---
  const previewScambiaVB = document.querySelector('[data-preview="scambia-viola-blu"]');
  const stepperScambiaVB = creaStepperSemplice("stepper-scambia-viola-blu", (valore) => {
    if (previewScambiaVB) previewScambiaVB.textContent = WW.fmtInt(valore * WW.GAME.num("D_Viola_D_Blu"));
  });
  collegaToggleMiniForm("btn-scambia-viola-blu", "form-scambia-viola-blu", stepperScambiaVB);
  const btnConfermaScambiaVB = document.getElementById("btn-conferma-scambia-viola-blu");
  if (btnConfermaScambiaVB && stepperScambiaVB) {
    btnConfermaScambiaVB.addEventListener("click", () => {
      const quantita = stepperScambiaVB.get();
      if (quantita <= 0) return;
      WW.NET.send("Scambia_Diamanti", WW.AUTH.accessToken, quantita);
      stepperScambiaVB.set(0);
      document.getElementById("form-scambia-viola-blu").hidden = true;
    });
  }

  // --- Scambia Tributi -> Diamanti Viola ---
  const previewScambiaTV = document.querySelector('[data-preview="scambia-tributi-viola"]');
  const stepperScambiaTV = creaStepperSemplice("stepper-scambia-tributi-viola", (valore) => {
    if (previewScambiaTV) previewScambiaTV.textContent = WW.fmtInt(valore * WW.GAME.num("Tributi_D_Viola"));
  });
  collegaToggleMiniForm("btn-scambia-tributi-viola", "form-scambia-tributi-viola", stepperScambiaTV);
  const btnConfermaScambiaTV = document.getElementById("btn-conferma-scambia-tributi-viola");
  if (btnConfermaScambiaTV && stepperScambiaTV) {
    btnConfermaScambiaTV.addEventListener("click", () => {
      const quantita = stepperScambiaTV.get();
      if (quantita <= 0) return;
      WW.NET.send("Scambia_Tributi", WW.AUTH.accessToken, quantita);
      stepperScambiaTV.set(0);
      document.getElementById("form-scambia-tributi-viola").hidden = true;
    });
  }

  // --- Velocizza con Diamanti Blu (Costruzione / Reclutamento) ---
  // Comando "Velocizza_Diamanti|token|<Costruzione|Reclutamento|Ricerca>|n"
  // — vedi case "Velocizza_Diamanti" in ServerConnection.cs. La Ricerca non
  // ha ancora una schermata web, quindi per ora colleghiamo solo le due
  // già esistenti.
  const VELOCIZZA_STEPPERS = Object.create(null);
  function collegaVelocizza(contesto, bottoneToggleId, formId, stepperId, bottoneConfermaId) {
    const stepper = creaStepperSemplice(stepperId, () => {});
    VELOCIZZA_STEPPERS[bottoneToggleId] = stepper;
    collegaToggleMiniForm(bottoneToggleId, formId, stepper);
    const btnConferma = document.getElementById(bottoneConfermaId);
    if (btnConferma && stepper) {
      btnConferma.addEventListener("click", () => {
        const quantita = stepper.get();
        if (quantita <= 0) return;
        WW.NET.send("Velocizza_Diamanti", WW.AUTH.accessToken, contesto, quantita);
        stepper.set(0);
        document.getElementById(formId).hidden = true;
      });
    }
  }
  collegaVelocizza("Costruzione", "btn-toggle-velocizza-costruzione", "form-velocizza-costruzione", "stepper-velocizza-costruzione", "btn-conferma-velocizza-costruzione");
  collegaVelocizza("Reclutamento", "btn-toggle-velocizza-reclutamento", "form-velocizza-reclutamento", "stepper-velocizza-reclutamento", "btn-conferma-velocizza-reclutamento");

  WW.VELOCIZZA_STEPPERS = VELOCIZZA_STEPPERS;
})(window.WW);
