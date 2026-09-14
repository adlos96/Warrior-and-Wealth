/* ==========================================================
   Warrior & Wealth — Web Client — 13-gamepass.js
   ----------------------------------------------------------
   SCHERMATA GAMEPASS GOLD — premi giornalieri (14/09/2026, su
   richiesta dell'utente): un solo premio al giorno per 90 giorni
   consecutivi di accesso col GamePass Gold (GamePass_Avanzato)
   attivo. Saltando un giorno, i premi ripartono dal Giorno 1
   (vedi ServerConnection.cs, Accesso_Giornaliero()).

   NOTA (14/09/2026): l'utente sta ancora verificando "sul client
   del gioco" se in pratica, comprato il GamePass e aspettando un
   giorno, si riesce a ritirare sia il premio del giorno corrente
   sia quello del giorno successivo aspettando un altro giorno —
   cioè se la logica desktop/server si comporta come ci si
   aspetta. Questa schermata è quindi per ora la parte ESTETICA
   (layout, stati dei premi, griglia) collegata ai dati REALI del
   server (non finti) così da poter verificare a colpo d'occhio
   se torna: quando client/server saranno confermati corretti,
   resterà solo da rifinire l'aspetto se necessario.

   Protocollo, comando|arg|arg classico (non JSON — vedi
   ServerConnection.cs, GamePass_Premi_Send()):
     - "Gamepass_Premi|<int>×90": il VALORE (sempre in Diamanti
       Viola) di ciascuno dei 90 premi giornalieri. È la stessa
       tabella fissa per tutti i giocatori (Variabili_Server.
       gamePass_DailyReward), non cambia da giocatore a giocatore.
     - "Gamepass_Premi_Ottenuti|<bool>×90": quali dei 90 premi
       questo giocatore ha già ritirato (player.GamePass_Premi[]).
   Il server manda entrambi ad ogni Login/AutoLogin e dopo ogni
   ritiro (GamePass_Premi_Send, chiamata anche da GamePass_Premi()
   lato server dopo un ritiro riuscito).

   Il giorno "corrente" (quello ritirabile, se non già ritirato) è
   WW.GAME.raw.Giorni_Consecutivi (= player.GamePass_Accessi_
   Consecutivi, generico via Update_Data — nessun bisogno di
   comandi dedicati). Lo stato del GamePass Gold è WW.GAME.raw.
   GamePass_Avanzato === "True", il tempo rimanente è già in
   WW.GAME.raw.GamePass_Avanzato_Tempo (stesso usato in
   11-statistiche.js).

   Ritiro: comando "GamePass DailyReward" col solo access token,
   stesso pattern di "Costruzione_Terreni" (vedi ServerConnection.
   cs, case "GamePass DailyReward" → GamePass_Premi(player), che
   ritira SOLO il giorno che combacia con Giorni_Consecutivi).

   Dipende da: WW.NET (01-net.js), WW.GAME (04-game-main.js),
   WW.AUTH (02-auth.js), WW.fmtInt (00-core.js).
   Non esporta nulla: si auto-inizializza registrando i propri
   handler e il click sulla griglia (delegato, un solo listener). */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  const GIORNI_TOTALI = 90;

  const stato = {
    valori: [], // int×90, Diamanti Viola per giorno (Gamepass_Premi)
    completati: [], // bool×90, già ritirati (Gamepass_Premi_Ottenuti)
  };

  const elHint = document.getElementById("gamepass-stato-hint");
  const elStreak = document.getElementById("gamepass-streak-valore");
  const elGrid = document.getElementById("gamepass-grid");
  if (!elGrid) return; // pagina senza la schermata GamePass (non dovrebbe succedere)

  function renderIntestazione() {
    const attivo = WW.GAME.raw.GamePass_Avanzato === "True";
    const tempo = WW.GAME.raw.GamePass_Avanzato_Tempo;
    elHint.textContent = attivo
      ? `GamePass Gold attivo — tempo rimanente: ${tempo || "0h 0m 0s"}`
      : "GamePass Gold non attivo. Visita lo Shop per acquistarlo e iniziare a ritirare i premi giornalieri.";
    // Giorni_Consecutivi è 0-based lato server (indice del premio corrente,
    // vedi GamePass_Premi() in ServerConnection.cs), +1 per il numero di
    // giorno mostrato all'utente ("Giorno 1" invece di "Giorno 0").
    const giorno = WW.GAME.num("Giorni_Consecutivi");
    elStreak.textContent = attivo ? WW.fmtInt(Math.min(giorno + 1, GIORNI_TOTALI)) : "–";
  }

  function renderGriglia() {
    if (stato.valori.length === 0) {
      elGrid.innerHTML = `<p class="research-desc__vuoto">Premi non ancora ricevuti dal server (arrivano subito dopo il login).</p>`;
      return;
    }
    const attivo = WW.GAME.raw.GamePass_Avanzato === "True";
    const giornoCorrente = WW.GAME.num("Giorni_Consecutivi");

    elGrid.innerHTML = stato.valori
      .map((valore, i) => {
        const completato = stato.completati[i] === true;
        const ritirabile = attivo && !completato && i === giornoCorrente;
        let classe = "gamepass-day--locked";
        if (completato) classe = "gamepass-day--claimed";
        else if (ritirabile) classe = "gamepass-day--ready";

        return `
        <button type="button" class="gamepass-day ${classe}" data-indice="${i}" ${ritirabile ? "" : "disabled"} title="Giorno ${i + 1} — ${WW.fmtInt(valore)} 💎${completato ? " (già ritirato)" : ""}">
          <span class="gamepass-day__giorno">${i + 1}</span>
          <img src="assets/DiamanteViola_V2.png" alt="">
          <span class="gamepass-day__valore">${WW.fmtInt(valore)}</span>
        </button>`;
      })
      .join("");
  }

  function renderTutto() {
    renderIntestazione();
    renderGriglia();
  }

  // Un solo listener delegato sulla griglia invece di uno per bottone (90
  // elementi ricreati ad ogni aggiornamento — stesso approccio di
  // renderMarkers() in 12-quest.js).
  elGrid.addEventListener("click", (e) => {
    const btn = e.target.closest(".gamepass-day");
    if (!btn || btn.disabled) return;
    WW.NET.send("GamePass DailyReward", WW.AUTH.accessToken);
  });

  WW.NET.on("Gamepass_Premi", (args) => {
    stato.valori = args.slice(0, GIORNI_TOTALI).map((v) => Number(v) || 0);
    renderTutto();
  });
  WW.NET.on("Gamepass_Premi_Ottenuti", (args) => {
    stato.completati = args.slice(0, GIORNI_TOTALI).map((v) => v === "True");
    renderTutto();
  });

  // Giorni_Consecutivi/GamePass_Avanzato arrivano genericamente con ogni
  // Update_Data (nessun comando dedicato): stesso pattern di WW.
  // renderQuestBarra in 12-quest.js, richiamata da renderAllFromServer()
  // in 04-game-main.js ad ogni tick, non solo quando arriva un nuovo
  // Gamepass_Premi/Gamepass_Premi_Ottenuti.
  WW.renderGamepass = renderTutto;
})(window.WW);
