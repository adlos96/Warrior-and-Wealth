/* ==========================================================
   Warrior & Wealth — Web Client — 15-avatar.js
   ----------------------------------------------------------
   Immagini profilo (18/09/2026, su richiesta dell'utente): popup
   "Cambia immagine profilo" aperto da #player-menu-cambio-avatar (vedi
   index.html/02-auth.js per il resto del Menu giocatore), stesso overlay
   generico .modal-overlay/.modal-box già usato altrove.

   Dati dal server (PlayerSnapshot.cs + Acquista_Avatar/Seleziona_Avatar in
   ServerConnection.cs):
   - GAME.raw.avatar            → id dell'avatar attuale ("" se non scelto)
   - GAME.raw.avatar_Sbloccati  → lista id sbloccati separati da virgola
                                  (i 2 gratuiti sono già dentro di default
                                  lato server, non serve distinguerli qui)
   - GAME.raw.Avatar_Costo_<id> → prezzo in Diamanti Viola di ogni avatar
                                  (mandato una volta al login, vedi
                                  Update_Data_OneTime)

   Comandi al server (WW.NET.send, stesso formato di "Shop" in 08-shop.js):
   - "Seleziona Avatar"|accessToken|id  → cambia l'avatar attuale (deve
                                          essere già sbloccato)
   - "Acquista Avatar"|accessToken|id   → sblocca un avatar bloccato
                                          scalando i Diamanti Viola
   ========================================================== */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  // 3 Lord + 2 Lady: id esattamente come li manda/riceve il server. Nota:
  // il file di "Lord_1" ha uno spazio nel nome ("Lord 1.png"), a differenza
  // degli altri 4 che usano l'underscore — inconsistenza nei file originali,
  // non un errore di battitura qui.
  const AVATAR_CONFIG = [
    { id: "Lord_1", nome: "Lord 1", file: "Lord 1.png" },
    { id: "Lord_2", nome: "Lord 2", file: "Lord_2.png" },
    { id: "Lord_3", nome: "Lord 3", file: "Lord_3.png" },
    { id: "Lord_4", nome: "Lord 3", file: "Lord_4.png" },
    { id: "Lord_5", nome: "Lord 3", file: "Lord_5.png" },
    { id: "Lord_6", nome: "Lord 3", file: "Lord_6.png" },
    { id: "Lord_7", nome: "Lord 3", file: "Lord_7.png" },
    { id: "Lord_1", nome: "Lord 3", file: "Lord_1.png" },
    { id: "Lord_2", nome: "Lord 3", file: "Lord_2.png" },
    { id: "Lord_3", nome: "Lord 3", file: "Lord_3.png" },
    { id: "Lord_4", nome: "Lord 3", file: "Lord_4.png" },
    { id: "Lord_5", nome: "Lord 3", file: "Lord_5.png" },
    { id: "Lord_6", nome: "Lord 3", file: "Lord_6.png" },

  ];

  const overlay = document.getElementById("avatar-picker-overlay");
  const griglia = document.getElementById("avatar-grid");
  const btnApri = document.getElementById("player-menu-cambio-avatar");
  const btnChiudi = document.getElementById("btn-chiudi-avatar-picker");
  const playerMenuOverlay = document.getElementById("player-menu-overlay");
  const resAvatarEmoji = document.getElementById("res-avatar-emoji");
  const resAvatarImg = document.getElementById("res-avatar-img");

  if (!overlay || !griglia || !btnApri) return; // markup non presente, niente da fare

  function apri() {
    if (playerMenuOverlay) playerMenuOverlay.hidden = true;
    overlay.hidden = false;
    renderGriglia();
  }

  function chiudi() {
    overlay.hidden = true;
  }

  btnApri.addEventListener("click", apri);
  btnChiudi.addEventListener("click", chiudi);
  overlay.addEventListener("click", (e) => {
    if (e.target === overlay) chiudi();
  });
  document.addEventListener("keydown", (e) => {
    if (e.key === "Escape" && !overlay.hidden) chiudi();
  });

  function avatarSbloccati() {
    const grezzo = WW.GAME.raw.avatar_Sbloccati || "";
    return grezzo.split(",").filter(Boolean);
  }

  // Stato del tick precedente (18/09/2026, su richiesta dell'utente: "un
  // effetto quando si acquista un avatar ed al cambio avatar"), per
  // accorgersi dei DUE momenti giusti confrontando con quello attuale:
  // - un id che entra in avatar_Sbloccati che prima non c'era → acquisto
  // - "avatar" (attuale) che cambia valore → selezione confermata
  // "inizializzato" evita di far scattare gli effetti al primo render dopo
  // il login, quando "precedente" è ancora vuoto per definizione.
  let inizializzato = false;
  let avatarPrecedente = "";
  let sbloccatiPrecedenti = [];
  let appenaSbloccatoId = null; // consumato dal prossimo renderGriglia()
  let appenaSelezionatoId = null;

  function templateCard(item, sbloccati, attuale) {
    // Prezzo 0 = avatar gratuito (18/09/2026, su richiesta dell'utente):
    // selezionabile subito, senza bisogno che sia già in avatar_Sbloccati —
    // niente lucchetto/prezzo mostrato, si comporta come uno già sbloccato.
    const prezzo = Number(WW.GAME.raw["Avatar_Costo_" + item.id] || 0);
    const bloccato = prezzo > 0 && !sbloccati.includes(item.id);
    const classi = ["avatar-card"];
    if (bloccato) classi.push("avatar-card--locked");
    if (item.id === attuale) classi.push("avatar-card--attuale");
    if (item.id === appenaSbloccatoId) classi.push("avatar-card--sbloccato");
    if (item.id === appenaSelezionatoId) classi.push("avatar-card--selezionato");

    // Nome mostrato: preferiamo "Avatar_Nome_<id>" mandato dal server (così
    // rinominare un avatar non richiede toccare il client), con l'id stesso
    // di AVATAR_CONFIG come fallback se il server non lo manda ancora.
    const nome = WW.GAME.raw["Avatar_Nome_" + item.id] || item.nome;

    const lucchetto = bloccato ? `<span class="avatar-card__lock">🔒</span>` : "";
    const prezzoHtml = bloccato
      ? `<span class="avatar-card__prezzo"><img src="assets/DiamanteViola_V2.png" class="icon-inline" alt=""> ${WW.fmtInt(prezzo)}</span>`
      : "";

    return `
    <li class="${classi.join(" ")}" data-avatar-id="${item.id}" data-bloccato="${bloccato}">
      ${lucchetto}
      <img class="avatar-card__img" src="assets/${item.file}" alt="${nome}">
      <span class="avatar-card__nome">${nome}</span>
      ${prezzoHtml}
    </li>`;
  }

  function renderGriglia() {
    const sbloccati = avatarSbloccati();
    const attuale = WW.GAME.raw.avatar || "";
    griglia.innerHTML = AVATAR_CONFIG.map((item) => templateCard(item, sbloccati, attuale)).join("");
    // "Consumati": l'animazione CSS parte una volta sola al disegno di
    // questo HTML — se restassero valorizzati, il prossimo giro di tick
    // (griglia ridisegnata da capo, stesso nodo DOM ricreato) la farebbe
    // ripartire da capo all'infinito finché il popup resta aperto.
    appenaSbloccatoId = null;
    appenaSelezionatoId = null;
  }

  // Click su una card: se è quella attuale non fa nulla, se è sbloccata la
  // seleziona, se è bloccata prova ad acquistarla (il server rifiuta se non
  // bastano i Diamanti Viola — vedi Acquista_Avatar).
  griglia.addEventListener("click", (e) => {
    const card = e.target.closest(".avatar-card");
    if (!card) return;
    const id = card.dataset.avatarId;
    const bloccato = card.dataset.bloccato === "true";
    if (!bloccato && id === (WW.GAME.raw.avatar || "")) return;

    const comando = bloccato ? "Acquista Avatar" : "Seleziona Avatar";
    WW.NET.send(comando, WW.AUTH.accessToken, id);
  });

  // Toglie e rimette la classe forzando un reflow (offsetWidth) in mezzo:
  // necessario perché un cambio rapido A→B→A lascerebbe la classe già
  // presente sull'elemento, e senza reflow l'animazione CSS non
  // ripartirebbe una seconda volta.
  function pulsa(el, classe) {
    if (!el) return;
    el.classList.remove(classe);
    void el.offsetWidth;
    el.classList.add(classe);
  }

  // Chiamata ad ogni tick da renderAllFromServer() (04-game-main.js):
  // aggiorna l'avatar mostrato nella barra risorse, e se il popup è aperto
  // ridisegna la griglia (per riflettere subito un acquisto/cambio appena
  // confermato dal server). Confronta anche con lo stato del tick
  // precedente per far scattare gli effetti di acquisto/cambio avatar
  // (18/09/2026, su richiesta dell'utente) solo nel momento giusto, non ad
  // ogni singolo render.
  function renderAvatar() {
    const attuale = WW.GAME.raw.avatar || "";
    const sbloccatiOra = avatarSbloccati();
    const config = AVATAR_CONFIG.find((a) => a.id === attuale);

    if (config && resAvatarImg && resAvatarEmoji) {
      resAvatarImg.src = "assets/" + config.file;
      resAvatarImg.hidden = false;
      resAvatarEmoji.hidden = true;
    } else if (resAvatarImg && resAvatarEmoji) {
      resAvatarImg.hidden = true;
      resAvatarEmoji.hidden = false;
    }

    if (inizializzato) {
      const nuovoSbloccato = sbloccatiOra.find((id) => !sbloccatiPrecedenti.includes(id));
      if (nuovoSbloccato) appenaSbloccatoId = nuovoSbloccato;

      if (attuale && attuale !== avatarPrecedente) {
        appenaSelezionatoId = attuale;
        pulsa(resAvatarImg, "avatar-img--cambiato");
      }
    }

    if (!overlay.hidden) renderGriglia();

    inizializzato = true;
    avatarPrecedente = attuale;
    sbloccatiPrecedenti = sbloccatiOra;
  }

  WW.renderAvatar = renderAvatar;
})(window.WW);
