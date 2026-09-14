/* ==========================================================
   Warrior & Wealth — Web Client — 12-quest.js
   ----------------------------------------------------------
   SCHERMATA QUEST MENSILI — rispecchia il protocollo server di
   Manager/QuestManager.cs, ma con una presentazione diversa da
   quella desktop (GUI/MontlyQuest.cs), su richiesta esplicita
   dell'utente (13/09/2026):

   - Le 20 ricompense Normali e le 20 VIP condividono lo STESSO
     array di soglie "Points" (vedi QuestRewardUpdate lato server):
     un'unica barra punti-esperienza con le ricompense posizionate
     sopra (Normali) e sotto (VIP, solo se il giocatore è VIP) di
     essa, invece di due liste testuali separate. Cliccando su una
     ricompensa già raggiunta (e non ancora ritirata) si invia
     "Quest_Reward|<token>|Normale|<indice 1-based>" (o "Vip").
   - Le quest (fino a 60 nel database server) sono mostrate 10 alla
     volta con un pulsante avanti/indietro, invece che tutte assieme
     o mescolate a rotazione come nel client desktop.

   Protocollo (NON un "comando|arg" ma JSON puro, gestito tramite
   WW.NET.onJson — vedi 01-net.js):
     - "QuestUpdate": { Type, Quests: [{ Id, Quest_Description,
       Experience, Require, Progress, Max_Complete, Completata }] }
       (solo le quest non ancora completate il numero massimo di
       volte — il server le filtra già lato suo).
     - "QuestRewards": { Type, Rewards_Normali: [20 int],
       Rewards_VIP: [20 int], Points: [20 int], Completo: [20 bool],
       Completo_Vip: [20 bool] }
   Il punteggio corrente del giocatore arriva invece come qualsiasi
   altro valore, dentro Update_Data: WW.GAME.raw.punti_quest (vedi
   PlayerSnapshot.cs, _currentState["punti_quest"]). Lo stato VIP è
   WW.GAME.raw.vip === "True" (stesso identico controllo già usato
   in 08-shop.js/11-statistiche.js).

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
    rewardsVip: [],
    points: [],
    claimNormal: [],
    claimVip: [],
    pagina: 0,
  };

  const elTrack = document.getElementById("quest-track");
  const elFill = document.getElementById("quest-track-fill");
  const elPunti = document.getElementById("quest-track-points");
  const elLegendaVip = document.getElementById("quest-legenda-vip");
  const elLista = document.getElementById("quest-list");
  const elPager = document.getElementById("quest-pager");
  const elPagerPrev = document.getElementById("quest-pager-prev");
  const elPagerNext = document.getElementById("quest-pager-next");
  const elPagerPagina = document.getElementById("quest-pager-pagina");

  if (!elTrack) return; // pannello non presente (non dovrebbe succedere)

  /* ---------- Ricompense (barra + marker) ---------- */

  // Metà larghezza del cerchietto marker (vedi .quest-marker in style.css,
  // width: 36px) — serve per tenere i marker agli estremi (0%/100%)
  // completamente dentro alla barra invece di uscire a metà dal bordo.
  const META_MARKER_PX = 18;

  // Distanza minima FISSA (in px, non in %) tra un marker e il successivo
  // (14/09/2026, su richiesta dell'utente: prima 38px/30px di diametro, ora
  // marker più grandi e più distanziati). Dando a #quest-track una
  // larghezza minima calcolata su questo valore, il pannello mostra una
  // decina abbondante di ricompense alla volta e il resto si raggiunge
  // scorrendo (vedi .quest-track-scroll in style.css) invece di stringere
  // tutto per farcelo stare.
  const DISTANZA_MARKER_PX = 48;

  // Icone delle ricompense (14/09/2026, su richiesta dell'utente): la
  // traccia mostrava SEMPRE l'icona Diamante Viola per ogni ricompensa,
  // Normale o VIP che fosse, ma non è così — confrontando con
  // QuestRewardSet in QuestManager.cs: alcune ricompense (indici 1-based,
  // gli stessi usati dal comando Quest_Reward) sono in Diamanti Blu, e la
  // ricompensa VIP #20 (l'ultima) è un Feudo Leggendario, non una valuta.
  // Tutte le altre restano Diamante Viola (comportamento invariato).
  const REWARD_DIAMANTE_BLU = {
    normale: new Set([2, 4, 7, 11, 14, 17]),
    vip: new Set([3]),
  };
  const REWARD_TERRENO = {
    vip: { 20: { file: "Leggendario.jpeg", nome: "Feudo Leggendario" } },
  };
  function infoIconaRicompensa(tipo, indiceUnoBased) {
    const terreno = REWARD_TERRENO[tipo] && REWARD_TERRENO[tipo][indiceUnoBased];
    if (terreno) return { file: terreno.file, alt: terreno.nome, etichetta: terreno.nome, terreno: true };
    const blu = REWARD_DIAMANTE_BLU[tipo] && REWARD_DIAMANTE_BLU[tipo].has(indiceUnoBased);
    return { file: blu ? "DiamanteBlu_V2.png" : "DiamanteViola_V2.png", alt: "", etichetta: null, terreno: false };
  }

  function renderMarkers() {
    // Rimuove i marker precedenti (i figli oltre alla barra stessa) e li
    // ricrea da zero: sono solo 20 (+20 VIP), un rebuild completo ad ogni
    // aggiornamento di QuestRewards non pesa e semplifica la gestione
    // rispetto a un diffing manuale come in shop/ricerca.
    elTrack.querySelectorAll(".quest-marker").forEach((el) => el.remove());

    const totale = stato.points.length;
    if (totale === 0) return;
    const isVip = WW.GAME.raw.vip === "True";
    elLegendaVip.hidden = false; // riga VIP sempre visibile (13/09/2026, su richiesta dell'utente), anche se il giocatore non è VIP

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
      // quella VIP della stessa coppia (un solo array "Points" condiviso,
      // vedi QuestRewardUpdate in QuestManager.cs) — un'unica etichetta
      // sulla barra stessa invece di ripeterla sopra E sotto (segnalato
      // dall'utente: era un doppione inutile).
      elTrack.appendChild(creaTickPunti(i, frac));
      elTrack.appendChild(creaMarker("normale", i, frac, stato.rewardsNormali[i], stato.claimNormal[i], true));
      // Riga VIP: sempre disegnata (sopra/sotto la stessa barra) così si
      // vede sempre a cosa si andrebbe incontro con il GamePass, ma se il
      // giocatore non è VIP resta bloccata anche se il punteggio è già
      // stato raggiunto (il server la rifiuterebbe comunque, vedi
      // ServerConnection.cs: "if (player.GamePass_Base == false) return;").
      elTrack.appendChild(creaMarker("vip", i, frac, stato.rewardsVip[i], stato.claimVip[i], isVip));
    }
  }

  // Un solo tick per coppia (Normale+VIP), sulla barra stessa, invece di
  // ripetere il punteggio richiesto sopra E sotto: le due ricompense della
  // stessa coppia condividono sempre la stessa soglia.
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
    if (info.terreno) el.classList.add("quest-marker--terreno");
    if (!raggiunta) el.classList.add("quest-marker--locked");
    else if (riscossa) el.classList.add("quest-marker--claimed");
    else el.classList.add("quest-marker--ready");
    el.style.left = `calc(${META_MARKER_PX}px + ${frazionePosizione} * (100% - ${META_MARKER_PX * 2}px))`;
    const etichettaValore = info.terreno ? info.etichetta : `${WW.fmtInt(valore || 0)} 💎`;
    el.title = !sbloccabile
      ? `VIP — ${etichettaValore}, richiede il GamePass e ${WW.fmtInt(stato.points[indice])} punti`
      : `${tipo === "vip" ? "VIP" : "Normale"} — ${etichettaValore}, richiede ${WW.fmtInt(stato.points[indice])} punti`;
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
      WW.NET.send("Quest_Reward", WW.AUTH.accessToken, tipo === "vip" ? "Vip" : "Normale", indice + 1);
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
    return `
      <li class="quest-item">
        <div class="quest-item__main">
          <p class="quest-item__desc">${escapeHtml(q.Quest_Description || "")}</p>
          <div class="quest-item__bar">
            <div class="quest-item__bar-fill" style="width:${percento}%"></div>
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

  /* ---------- Ricezione dati dal server ---------- */

  WW.NET.onJson("QuestUpdate", (msg) => {
    stato.quests = Array.isArray(msg.Quests) ? msg.Quests : [];
    renderQuestList();
  });

  WW.NET.onJson("QuestRewards", (msg) => {
    stato.rewardsNormali = msg.Rewards_Normali || [];
    stato.rewardsVip = msg.Rewards_VIP || [];
    stato.points = msg.Points || [];
    stato.claimNormal = msg.Completo || [];
    stato.claimVip = msg.Completo_Vip || [];
    renderRicompense();
  });

  // La barra dei punti va ricalcolata anche ad ogni "tick" generico
  // (Update_Data aggiorna punti_quest periodicamente, non solo quando
  // arriva un nuovo QuestRewards): la si riaggancia al ciclo generale
  // di refresh dell'app tramite WW.renderAllFromServer, se già definito.
  WW.renderQuestBarra = renderBarra;
})(window.WW);
