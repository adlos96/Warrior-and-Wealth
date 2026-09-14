/* ==========================================================
   Warrior & Wealth — Web Client — 10-main.js
   ----------------------------------------------------------
   Bootstrap — va caricato PER ULTIMO in index.html, dopo tutti
   gli altri file (00-core.js ... 09-ricerca.js): a questo punto
   WW.renderStruttureListForm/WW.renderUnitaForm/WW.renderSbloccoUnita
   (06-costruzione.js), WW.renderCittaList (07-citta.js),
   WW.renderShop (08-shop.js) e WW.renderRicerca (09-ricerca.js)
   esistono già, quindi la prima chiamata a WW.renderAllFromServer()
   (definita in 04-game-main.js) trova tutto pronto.

   Ordine completo degli <script> in index.html:
     00-core.js, 01-net.js, 02-auth.js, 03-nav.js, 04-game-main.js,
     05-scambio.js, 06-costruzione.js, 07-citta.js, 08-shop.js,
     09-ricerca.js, 11-statistiche.js, 12-quest.js, 10-main.js

   NOTA: questo file sostituisce il precedente "09-main.js" (rinominato
   in "10-main.js" per lasciare spazio a "09-ricerca.js" nell'ordine di
   caricamento, stesso principio già usato per il vecchio "js/app.js").
   "09-main.js" resta fisicamente nella cartella ma non è più referenziato
   da index.html: può essere rimosso manualmente quando comodo.

   Si tenta subito la connessione al server: se c'è già un accesso
   "ricordato" (token salvati), AUTH.onSocketOpen() farà l'auto-login non
   appena il WebSocket è aperto, senza passare dal form. Se la
   connessione fallisce, NET va comunque in retry automatico (vedi
   scheduleReconnect) e l'utente resta sulla schermata di login con
   l'indicatore di stato. */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  WW.renderAllFromServer();

  if (WW.storage.get("ww_remember_user")) {
    document.getElementById("res-username").textContent = WW.storage.get("ww_remember_user");
  }
  WW.NET.connect();
})(window.WW);
