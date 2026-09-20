/* ==========================================================
   Warrior & Wealth — Web Client — 03-nav.js
   ----------------------------------------------------------
   Navigazione: tab bar in basso (Main / Costruzione / Città / ...)
   e toggle dei pannelli Feudi/Strutture/Esercito su mobile.
   File autonomo: non dipende da nessun altro e nessun altro
   file dipende da lui (solo DOM, nessun dato condiviso via WW).
   ========================================================== */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  const tabButtons = document.querySelectorAll(".tab-bar__btn");
  const mainPanel = document.querySelector('[data-tab-panel="main"]');
  const costruzionePanel = document.querySelector('[data-tab-panel="costruzione"]');
  const cittaPanel = document.querySelector('[data-tab-panel="citta"]');
  const negozioPanel = document.querySelector('[data-tab-panel="negozio"]');
  const ricercaPanel = document.querySelector('[data-tab-panel="ricerca"]');
  const statistichePanel = document.querySelector('[data-tab-panel="statistiche"]');
  const questPanel = document.querySelector('[data-tab-panel="quest"]');
  const gamepassPanel = document.querySelector('[data-tab-panel="gamepass"]');
  const pvppvePanel = document.querySelector('[data-tab-panel="pvppve"]');
  const placeholderPanel = document.querySelector('[data-tab-panel="placeholder"]');
  const placeholderTitle = document.getElementById("placeholder-title");
  // Un tab-panel implementato per ogni voce qui dentro; le altre voci della
  // tab-bar (Mercato, Mappa, ...) restano sul placeholder finché non
  // avranno anch'esse il loro protocollo/schermata dedicata.
  const tabPanels = { main: mainPanel, costruzione: costruzionePanel, citta: cittaPanel, negozio: negozioPanel, ricerca: ricercaPanel, statistiche: statistichePanel, quest: questPanel, gamepass: gamepassPanel, pvppve: pvppvePanel };

  tabButtons.forEach((btn) => {
    btn.addEventListener("click", () => {
      tabButtons.forEach((b) => b.classList.remove("is-active"));
      btn.classList.add("is-active");

      const tab = btn.dataset.tab;
      const panel = tabPanels[tab];
      Object.values(tabPanels).forEach((p) => { if (p) p.hidden = true; });
      if (panel) {
        panel.hidden = false;
        placeholderPanel.hidden = true;
      } else {
        placeholderPanel.hidden = false;
        placeholderTitle.textContent = btn.textContent;
      }
    });
  });

  const sectionToggleBtns = document.querySelectorAll("#main-panel-toggle .section-toggle__btn");
  const mainGridPanels = document.querySelectorAll(".main-grid [data-panel]");

  // Toggle Cronologia/Messaggi: un solo pannello .main-grid (data-panel=
  // "cronologia"), due viste interne selezionate con #cronologia-messaggi-
  // toggle. Su desktop resta l'unico modo per passare dall'una all'altra
  // (.section-toggle--inline, sempre visibile — vedi style.css). Su mobile
  // "Messaggi" è invece un pulsante diretto nel toggle Feudi/Strutture/
  // Esercito/Cronologia qui sotto (il toggle interno viene nascosto via
  // CSS), ma la funzione che sceglie la vista resta la stessa, richiamata
  // anche da showPanel().
  const cronologiaMessaggiToggle = document.getElementById("cronologia-messaggi-toggle");
  const cmBtns = cronologiaMessaggiToggle ? cronologiaMessaggiToggle.querySelectorAll(".section-toggle__btn") : [];
  const cmViste = document.querySelectorAll("[data-cronologia-messaggi-view]");

  function setCronologiaMessaggiView(view) {
    cmBtns.forEach((b) => b.classList.toggle("is-active", b.dataset.viewTarget === view));
    cmViste.forEach((v) => { v.hidden = v.dataset.cronologiaMessaggiView !== view; });
  }
  cmBtns.forEach((btn) => {
    btn.addEventListener("click", () => setCronologiaMessaggiView(btn.dataset.viewTarget));
  });

  function showPanel(target) {
    // "messaggi" non è un [data-panel] a sé: vive dentro "cronologia" (vedi
    // sopra) — mostra quel pannello e ci passa dentro sulla vista giusta.
    const panelReale = target === "messaggi" ? "cronologia" : target;
    mainGridPanels.forEach((p) => p.classList.toggle("is-visible", p.dataset.panel === panelReale));
    if (target === "messaggi" || target === "cronologia") setCronologiaMessaggiView(target);
  }
  sectionToggleBtns.forEach((btn) => {
    btn.addEventListener("click", () => {
      sectionToggleBtns.forEach((b) => b.classList.remove("is-active"));
      btn.classList.add("is-active");
      showPanel(btn.dataset.panelTarget);
    });
  });
  showPanel("feudi");
})(window.WW);
