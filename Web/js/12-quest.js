/* ==========================================================
   Warrior & Wealth — Web Client — 12-quest.js
   ----------------------------------------------------------
   SCHERMATA QUEST MENSILI — rispecchia il protocollo server di
   Manager/QuestManager.cs, ma con una presentazione diversa da
   quella desktop (GUI/MontlyQuest.cs), su richiesta esplicita
   dell'utente (13/09/2026):

   - Le 20 ricompense Normali e le 20 GamePass Silver condividono lo
     STESSO array di soglie "Points" (vedi QuestRewardUpdate lato
     server): un'unica barra punti-esperienza con le ricompense
     posizionate sopra (Normali) e sotto (GamePass Silver, solo se il
     giocatore ha il GamePass Silver attivo) di essa, invece di due
     liste testuali separate. Cliccando su una ricompensa già
     raggiunta (e non ancora ritirata) si invia
     "Quest_Reward|<token>|Normale|<indice 1-based>" (o "Silver").
   - Le quest (fino a 60 nel database server) sono mostrate 10 alla
     volta con un pulsante avanti/indietro, invece che tutte assieme
     o mescolate a rotazione come nel client desktop.

   Rinominato da "VIP" a "GamePass Silver" (19/09/2026, su richiesta
   dell'utente): la vecchia riga di ricompense premium usava lo stato
   "vip" indipendente (Vip_1/Vip_2 in Shop) invece del GamePass_Base
   già esistente (usato altrove come "GamePass Silver", vedi 11-
   statistiche.js/08-shop.js) — ora unificati sotto lo stesso nome e
   la stessa variabile di stato. Il server deve rispecchiare questo
   cambio: il case "Vip" del comando Quest_Reward diventa "Silver", e
   i campi "Rewards_VIP"/"Completo_Vip" del JSON "QuestRewards"
   diventano "Rewards_Silver"/"Completo_Silver" (vedi elenco protocollo
   sotto). Lato client non serve più leggere un flag "vip" a parte:
   si riusa GamePass_Base, già presente in ogni Update_Data.

   Protocollo (NON un "comando|arg" ma JSON puro, gestito tramite
   WW.NET.onJson — vedi 01-net.js):
     - "QuestUpdate": { Type, Quests: [{ Id, Quest_Description,
       Experience, Require, Progress, Max_Complete, Completata }] }
       (solo le quest non ancora completate il numero massimo di
       volte — il server le filtra già lato suo).
     - "QuestRewards": { Type, Rewards_Normali: [20 int],
       Rewards_Silver: [20 int], Points: [20 int], Completo: [20 bool],
       Completo_Silver: [20 bool] }
   Il punteggio corrente del giocatore arriva invece come qualsiasi
   altro valore, dentro Update_Data: WW.GAME.raw.punti_quest (vedi
   PlayerSnapshot.cs, _currentState["punti_quest"]). Lo stato GamePass
   Silver è WW.GAME.raw.GamePass_Base === "True" (stesso identico
   controllo già usato in 08-shop.js/11-statistiche.js/13-gamepass.js
   per GamePass_Avanzato).

   Dipende da: WW.NET (01-net.js), WW.GAME (04-game-main.js),
   WW.AUTH (02-auth.js), WW.fmtInt (00-core.js).
   Non esporta nulla: si auto-inizializza registrando i propri
   handler JSON e i click sul pager. */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  const QUEST_PER_PAGINA = 10;

  const stato = {
    quests: [],
    rewardsNormali: [],
    rewardsSilver: [],
    points: [],
    claimNormal: [],
    claimSilver: [],
    pagina: 0,
  };

  const elTrack = document.getElementById("quest-track");
  const elFill = document.getElementById("quest-track-fill");
  const elPunti = document.getElementById("quest-track-points");
  const elLegendaSilver = document.getElementById("quest-legenda-silver");
  const elLista = document.getElementById("quest-list");
  const elPager = document.getElementById("quest-pager");
  const elPagerPrev = document.getElementById("quest-pager-prev");
  const elPagerNext = document.getElementById("quest-pager-next");
  const elPagerPagina = document.getElementById("quest-pager-pagina");
  const elToast = document.getElementById("quest-toast");

  if (!elTrack) return; // pannello non presente (non dovrebbe succedere)

  // Diventa true dopo il primo "QuestRewards" ricevuto (vedi sotto): serve a
  // non far scattare l'animazione di riscossione per le ricompense che
  // arrivano GIÀ segnate come riscosse al primo caricamento della schermata
  // (altrimenti al login si vedrebbero "esplodere" tutte quelle prese nei
  // mesi/giorni precedenti).
  let rewardsCaricate = false;
  let toastTimeout = null;

  /* ---------- Ricompense (barra + marker) ---------- */

  // Metà larghezza del cerchietto marker (vedi .quest-marker in style.css,
  // width: 48px dopo l'ultimo ingrandimento) — serve per tenere i marker
  // agli estremi (0%/100%) completamente dentro alla barra invece di
  // uscire a metà dal bordo, e per posizionare le etichette del punteggio
  // richiesto (creaTickPunti) alla stessa distanza dal bordo dei marker.
  const META_MARKER_PX = 24;

  // Distanza minima FISSA (in px, non in %) tra un marker e il successivo
  // (14/09/2026, su richiesta dell'utente: prima 38px/30px di diametro, poi
  // via via più grandi e più distanziati ad ogni richiesta — icone, scritte
  // e bagliore — quindi anche la distanza è salita di pari passo per non
  // farli sovrapporre). Dando a #quest-track una larghezza minima calcolata
  // su questo valore, il pannello mostra una decina abbondante di
  // ricompense alla volta e il resto si raggiunge scorrendo (vedi
  // .quest-track-scroll in style.css) invece di stringere tutto per
  // farcelo stare.
  const DISTANZA_MARKER_PX = 62;

  // Icone delle ricompense (14/09/2026, su richiesta dell'utente): la
  // traccia mostrava SEMPRE l'icona Diamante Viola per ogni ricompensa,
  // Normale o GamePass Silver che fosse, ma non è così — confrontando con
  // QuestRewardSet in QuestManager.cs: alcune ricompense (indici 1-based,
  // gli stessi usati dal comando Quest_Reward) sono in Diamanti Blu, e la
  // ricompensa GamePass Silver #20 (l'ultima) è un Feudo Leggendario, non
  // una valuta. Tutte le altre restano Diamante Viola (comportamento
  // invariato).
  const REWARD_DIAMANTE_BLU = {
    normale: new Set([2, 4, 7, 11, 14, 17]),
    silver: new Set([3]),
  };
  const REWARD_TERRENO = {
    silver: { 20: { file: "Leggendario.jpeg", nome: "Feudo Leggendario" } },
  };
  function infoIconaRicompensa(tipo, indiceUnoBased) {
    const terreno = REWARD_TERRENO[tipo] && REWARD_TERRENO[tipo][indiceUnoBased];
    if (terreno) return { file: terreno.file, alt: terreno.nome, etichetta: terreno.nome, terreno: true };
    const blu = REWARD_DIAMANTE_BLU[tipo] && REWARD_DIAMANTE_BLU[tipo].has(indiceUnoBased);
    return { file: blu ? "DiamanteBlu_V2.png" : "DiamanteViola_V2.png", alt: "", etichetta: null, terreno: false };
  }

  function renderMarkers() {
    // Rimuove i marker precedenti (i figli oltre alla barra stessa) e li
    // ricrea da zero: sono solo 20 (+20 GamePass Silver), un rebuild
    // completo ad ogni aggiornamento di QuestRewards non pesa e semplifica
    // la gestione rispetto a un diffing manuale come in shop/ricerca.
    elTrack.querySelectorAll(".quest-marker").forEach((el) => el.remove());

    const totale = stato.points.length;
    if (totale === 0) return;
    const haSilver = WW.GAME.raw.GamePass_Base === "True";
    elLegendaSilver.hidden = false; // riga GamePass Silver sempre visibile (13/09/2026, su richiesta dell'utente), anche se il giocatore non lo ha attivo

    elTrack.style.minWidth = `${DISTANZA_MARKER_PX * (totale - 1) + META_MARKER_PX * 2}px`;

    // Posizionati a distanza REGOLARE (per indice), non in proporzione al
    // punteggio richiesto: i valori di "Points" non sono lineari (alcune
    // ricompense sono vicinissime tra loro, altre lontanissime — vedi
    // QuestRewardSet in QuestManager.cs), quindi posizionarli in base al
    // punteggio reale li ammucchiava tutti verso la fine della barra,
    // sovrapponendo icone ed etichette (segnalato dall'utente). Il punteggio
    // esatto richiesto resta comunque leggibile passandoci sopra (tooltip).
    for (let i = 0; i < totale; i++) {
      const frac = totale > 1 ? i / (totale - 1) : 0;
      // Il punteggio richiesto è lo STESSO per la ricompensa Normale e per
      // quella GamePass Silver della stessa coppia (un solo array "Points"
      // condiviso, vedi QuestRewardUpdate in QuestManager.cs) — un'unica
      // etichetta sulla barra stessa invece di ripeterla sopra E sotto
      // (segnalato dall'utente: era un doppione inutile).
      elTrack.appendChild(creaTickPunti(i, frac));
      elTrack.appendChild(creaMarker("normale", i, frac, stato.rewardsNormali[i], stato.claimNormal[i], true));
      // Riga GamePass Silver: sempre disegnata (sopra/sotto la stessa barra)
      // così si vede sempre a cosa si andrebbe incontro col GamePass, ma se
      // il giocatore non lo ha attivo resta bloccata anche se il punteggio è
      // già stato raggiunto (il server la rifiuterebbe comunque, vedi
      // ServerConnection.cs: "if (player.GamePass_Base == false) return;").
      elTrack.appendChild(creaMarker("silver", i, frac, stato.rewardsSilver[i], stato.claimSilver[i], haSilver));
    }
  }

  // Un solo tick per coppia (Normale+GamePass Silver), sulla barra stessa,
  // invece di ripetere il punteggio richiesto sopra E sotto: le due
  // ricompense della stessa coppia condividono sempre la stessa soglia.
  function creaTickPunti(indice, frazionePosizione) {
    const el = document.createElement("span");
    el.className = "quest-track__tick";
    el.style.left = `calc(${META_MARKER_PX}px + ${frazionePosizione} * (100% - ${META_MARKER_PX * 2}px))`;
    el.textContent = WW.fmtInt(stato.points[indice]);
    return el;
  }

  function creaMarker(tipo, indice, frazionePosizione, valore, riscossa, sbloccabile) {
    const punti = WW.GAME.num("punti_quest");
    const raggiunta = sbloccabile && punti >= stato.points[indice];
    const info = infoIconaRicompensa(tipo, indice + 1);

    const el = document.createElement("button");
    el.type = "button";
    el.className = `quest-marker quest-marker--${tipo}`;
    // Attributi usati da segnalaRiscossione() per ritrovare il marker giusto
    // dopo un rebuild di renderMarkers() e far partire l'animazione su di
    // esso (14/09/2026, su richiesta dell'utente).
    el.dataset.markerTipo = tipo;
    el.dataset.markerIndice = String(indice);
    if (info.terreno) el.classList.add("quest-marker--terreno");
    if (!raggiunta) el.classList.add("quest-marker--locked");
    else if (riscossa) el.classList.add("quest-marker--claimed");
    else el.classList.add("quest-marker--ready");
    el.style.left = `calc(${META_MARKER_PX}px + ${frazionePosizione} * (100% - ${META_MARKER_PX * 2}px))`;
    const etichettaValore = info.terreno ? info.etichetta : `${WW.fmtInt(valore || 0)} 💎`;
    el.title = !sbloccabile
      ? `GamePass Silver — ${etichettaValore}, richiede il GamePass Silver e ${WW.fmtInt(stato.points[indice])} punti`
      : `${tipo === "silver" ? "GamePass Silver" : "Normale"} — ${etichettaValore}, richiede ${WW.fmtInt(stato.points[indice])} punti`;
    el.disabled = !raggiunta || riscossa;

    // Il valore della ricompensa va nell'angolino dell'icona invece che in
    // una riga sotto: con 20 marker vicini, un'etichetta separata per
    // ognuno si sovrapponeva a quella dei vicini (segnalato dall'utente) —
    // il badge, attaccato all'icona stessa, non ha questo problema. Il
    // punteggio richiesto NON è più qui (era ripetuto identico sopra E
    // sotto, dato che le due file condividono le stesse soglie): vedi
    // creaTickPunti(), un'unica etichetta sulla barra.
    el.innerHTML = `<img src="assets/${info.file}" alt="${info.alt}"><span class="quest-marker__valore">${WW.fmtInt(valore || 0)}</span>`;

    el.addEventListener("click", () => {
      if (el.disabled) return;
      WW.NET.send("Quest_Reward", WW.AUTH.accessToken, tipo === "silver" ? "Silver" : "Normale", indice + 1);
    });

    return el;
  }

  function renderBarra() {
    const totale = stato.points.length;
    if (totale === 0) {
      elFill.style.width = "0%";
      elPunti.textContent = "0 / 0 punti";
      return;
    }
    const punti = WW.GAME.num("punti_quest");
    const maxPunti = stato.points[totale - 1] || 1;
    const percento = Math.max(0, Math.min(100, (punti / maxPunti) * 100));
    elFill.style.width = `${percento}%`;
    elPunti.textContent = `${WW.fmtInt(punti)} / ${WW.fmtInt(maxPunti)} punti`;
  }

  function renderRicompense() {
    renderBarra();
    renderMarkers();
  }

  /* ---------- Elenco quest (paginato) ---------- */

  function renderQuestList() {
    const totalePagine = Math.max(1, Math.ceil(stato.quests.length / QUEST_PER_PAGINA));
    if (stato.pagina >= totalePagine) stato.pagina = totalePagine - 1;
    if (stato.pagina < 0) stato.pagina = 0;

    const inizio = stato.pagina * QUEST_PER_PAGINA;
    const paginaQuests = stato.quests.slice(inizio, inizio + QUEST_PER_PAGINA);

    if (paginaQuests.length === 0) {
      elLista.innerHTML = `<li class="quest-item"><div class="quest-item__main"><p class="quest-item__desc">Nessuna quest attiva al momento.</p></div></li>`;
    } else {
      elLista.innerHTML = paginaQuests.map(rigaQuest).join("");
      // Percentuale della barra: vedi nota su "data-percento" in rigaQuest().
      elLista.querySelectorAll(".quest-item__bar-fill").forEach((el) => {
        el.style.width = `${el.dataset.percento}%`;
      });
    }

    elPager.hidden = stato.quests.length <= QUEST_PER_PAGINA;
    elPagerPagina.textContent = `${stato.pagina + 1} / ${totalePagine}`;
    elPagerPrev.disabled = stato.pagina === 0;
    elPagerNext.disabled = stato.pagina >= totalePagine - 1;
  }

  function rigaQuest(q) {
    const percento = q.Require > 0 ? Math.max(0, Math.min(100, (q.Progress / q.Require) * 100)) : 0;
    // Requisito e progresso scritti DENTRO la barra (come le barre HP/DEF
    // di Feudi/Città), invece che in una riga di testo piccola sotto — più
    // leggibile a colpo d'occhio. Il numero di volte completata diventa un
    // badge a parte, staccato dal numero di progresso (13/09/2026, su
    // richiesta dell'utente: prima erano tutti e due nella stessa riga).
    // 18/09/2026: niente più "style" nell'HTML (CSP style-src-attr, vedi
    // renderQuestList: la percentuale viene impostata subito dopo con
    // elFill.style.width, che a differenza dell'attributo style="" scritto
    // qui non rientra nella direttiva "style-src-attr" — stesso principio
    // già usato per la barra punti in renderBarra() poco sopra in questo
    // file). "data-percento" porta il valore fino a lì.
    return `
      <li class="quest-item">
        <div class="quest-item__main">
          <p class="quest-item__desc">${escapeHtml(q.Quest_Description || "")}</p>
          <div class="quest-item__bar">
            <div class="quest-item__bar-fill" data-percento="${percento}"></div>
            <span class="quest-item__bar-label">${WW.fmtInt(q.Progress)} / ${WW.fmtInt(q.Require)}</span>
          </div>
          <span class="quest-item__completate">Completata ${q.Completata}/${q.Max_Complete}</span>
        </div>
        <span class="quest-item__exp">+${WW.fmtInt(q.Experience)} Exp</span>
      </li>`;
  }

  function escapeHtml(testo) {
    const div = document.createElement("div");
    div.textContent = testo;
    return div.innerHTML;
  }

  elPagerPrev.addEventListener("click", () => {
    stato.pagina -= 1;
    renderQuestList();
  });
  elPagerNext.addEventListener("click", () => {
    stato.pagina += 1;
    renderQuestList();
  });

  /* ---------- Feedback grafico alla riscossione di una ricompensa ----------
     (14/09/2026, su richiesta dell'utente: "quando un premio viene raccolto,
     dovremmo mostrarlo graficamente in qualche modo... così è più gradevole
     per il giocatore e inoltre possiamo osservare eventuali bug"). Tre
     elementi, tutti innescati da segnalaRiscossione() quando un marker passa
     da "non riscosso" a "riscosso" tra un QuestRewards e il successivo:
       1) un piccolo "scatto" sul marker stesso (scala + bagliore);
       2) il valore che sale e sfuma sopra/sotto il marker;
       3) un toast in cima al pannello con tipo e valore riscosso — utile
          apposta per accorgersi a colpo d'occhio se il valore o il tipo
          (Normale/GamePass Silver) non sono quelli attesi. */

  function trovaIndiciAppenaRiscossi(vecchio, nuovo) {
    const risultato = [];
    for (let i = 0; i < nuovo.length; i++) {
      if (nuovo[i] && !(vecchio && vecchio[i])) risultato.push(i);
    }
    return risultato;
  }

  function mostraToastQuest(testo) {
    if (!elToast) return;
    elToast.textContent = testo;
    elToast.hidden = false;
    // Riavvia l'animazione di comparsa anche se il toast è già visibile
    // (riscossioni multiple ravvicinate, es. "Ripara Tutto"-style multi-click).
    elToast.classList.remove("quest-toast--anim");
    void elToast.offsetWidth;
    elToast.classList.add("quest-toast--anim");
    clearTimeout(toastTimeout);
    toastTimeout = setTimeout(() => { elToast.hidden = true; }, 2200);
  }

  function segnalaRiscossione(tipo, indiceZeroBased) {
    const valore = tipo === "silver" ? stato.rewardsSilver[indiceZeroBased] : stato.rewardsNormali[indiceZeroBased];
    const info = infoIconaRicompensa(tipo, indiceZeroBased + 1);
    const testoValore = info.terreno ? info.etichetta : `+${WW.fmtInt(valore || 0)} 💎`;

    const marker = elTrack.querySelector(`.quest-marker[data-marker-tipo="${tipo}"][data-marker-indice="${indiceZeroBased}"]`);
    if (marker) {
      marker.classList.add("quest-marker--collected");
      marker.addEventListener("animationend", () => marker.classList.remove("quest-marker--collected"), { once: true });

      const popup = document.createElement("span");
      popup.className = `quest-reward-popup quest-reward-popup--${tipo}`;
      popup.style.left = marker.style.left;
      popup.textContent = testoValore;
      elTrack.appendChild(popup);
      popup.addEventListener("animationend", () => popup.remove(), { once: true });
    }

    mostraToastQuest(`Ricompensa ${tipo === "silver" ? "GamePass Silver" : "Normale"} riscossa: ${testoValore}`);
  }

  /* ---------- Ricezione dati dal server ---------- */

  WW.NET.onJson("QuestUpdate", (msg) => {
    stato.quests = Array.isArray(msg.Quests) ? msg.Quests : [];
    renderQuestList();
  });

  WW.NET.onJson("QuestRewards", (msg) => {
    const nuoviClaimNormal = msg.Completo || [];
    const nuoviClaimSilver = msg.Completo_Silver || [];

    // Confronto PRIMA di sovrascrivere stato.claimNormal/claimSilver: un
    // indice passato da false a true è una ricompensa riscossa in questo
    // preciso aggiornamento (sia perché il giocatore l'ha appena cliccata,
    // sia se arrivasse "già riscossa" da un altro client/sessione).
    const appenaRiscosseNormali = rewardsCaricate ? trovaIndiciAppenaRiscossi(stato.claimNormal, nuoviClaimNormal) : [];
    const appenaRiscosseSilver = rewardsCaricate ? trovaIndiciAppenaRiscossi(stato.claimSilver, nuoviClaimSilver) : [];

    stato.rewardsNormali = msg.Rewards_Normali || [];
    stato.rewardsSilver = msg.Rewards_Silver || [];
    stato.points = msg.Points || [];
    stato.claimNormal = nuoviClaimNormal;
    stato.claimSilver = nuoviClaimSilver;
    renderRicompense();

    appenaRiscosseNormali.forEach((i) => segnalaRiscossione("normale", i));
    appenaRiscosseSilver.forEach((i) => segnalaRiscossione("silver", i));
    rewardsCaricate = true;
  });

  // BUGFIX (14/09/2026, segnalato dall'utente: "ho 222 punti ed il primo
  // premio non l'ho mai raccolto ma risulta non raccoglibile"): questo hook
  // prima richiamava solo renderBarra(). punti_quest cambia ad ogni "tick"
  // generico (Update_Data), ma lo stato raggiunta/bloccata di OGNI marker
  // viene deciso in creaMarker() solo quando i marker vengono ricreati da
  // renderMarkers() — cosa che accadeva solo all'arrivo di un nuovo
  // "QuestRewards" dal server. Risultato: se il punteggio superava la
  // soglia di un premio DOPO l'ultimo QuestRewards ricevuto, il marker
  // restava visivamente bloccato (e quindi non cliccabile) anche se il
  // giocatore aveva già i punti necessari, finché non arrivava un altro
  // QuestRewards (es. completando un'altra quest) a "sbloccarlo" di
  // riflesso. Ora l'hook richiama renderRicompense() (barra + marker),
  // così anche i marker restano aggiornati ad ogni tick, non solo la barra.
  WW.renderQuestBarra = renderRicompense;
})(window.WW);
