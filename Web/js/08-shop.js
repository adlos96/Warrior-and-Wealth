/* ==========================================================
   Warrior & Wealth — Web Client — 08-shop.js
   ----------------------------------------------------------
   SCHERMATA NEGOZIO — comando "Shop|token|<comando>", gestito
   lato server da Shop.Shop_Call() (ServerData/Moduli/Shop.cs).

   Solo una parte degli acquisti del client desktop è oggi
   davvero funzionante lato server:
   - VIP 24H (comando "Vip_1"): paga in Diamanti Viola, funziona.
   - Costruttori 24H/48H, Reclutatori 24H/48H, Scudo della Pace
     8H/24H/72H: pagano in Diamanti Blu, funzionano.
   Il resto richiede un pagamento reale (USDT) non ancora collegato
   a una blockchain — nel codice server sono "stub":
   - "Vip_2" (VIP via USDT) e "Pacchetto_1" (unico pacchetto
     diamanti con un case nel server, gli altri 3 non esistono
     nemmeno) controllano una variabile locale
     payment_Status_Blockchain/conferma_Transazione che è SEMPRE
     false: il comando arriva, il server logga, ma non succede
     nulla — nessun acquisto, nessun errore visibile all'utente.
   - "GamePass_Base"/"GamePass_Avanzato" sono invece un bug più
     serio da segnalare: il case non controlla NESSUN pagamento,
     assegna il GamePass gratis a chiunque mandi il comando. Per
     questo qui le card GamePass e quelle a pagamento reale sono
     mostrate ma disattivate ("Prossimamente"), senza inviare
     alcun comando al server finché il pagamento reale non sarà
     implementato.

   Dipende da: WW.GAME (04-game-main.js), WW.fmtInt/WW.fmtDecimal
   (00-core.js), WW.NET/WW.AUTH (01-net.js/02-auth.js). Esporta:
   WW.renderShop — usata da renderAllFromServer in 04-game-main.js. */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  // costoChiave/rewardChiave sono le chiavi mandate una tantum dal server
  // in Update_Data_OneTime (ServerConnection.cs) — prezzi reali, non fissi
  // in JS. rewardUnit "h" = il server manda già le ore, "g" = il server
  // manda i secondi (va diviso per 86400 per i giorni, es. GamePass).
  //
  // descChiave (13/09/2026, su richiesta dell'utente): stesso meccanismo
  // già usato per Ricerca/Costruzione — il server manda da solo
  // "Descrizione|<descChiave>|<testo>" (BBCode-like, dopo login/AutoLogin),
  // catturato nella cache comune WW.descrizioni (04-game-main.js). Le
  // stringhe sono ESATTE, prese da ClientMessageHandlers.cs (case "Shop
  // ..."), non etichette a piacere. I 4 pacchetti "Diamanti Viola" NON
  // hanno un case lì — il server non manda mai una descrizione per loro —
  // quindi restano senza descChiave e senza pulsante "ⓘ".
  const SHOP_CATEGORIE = [
    {
      titolo: "VIP",
      statoTesto: () =>
        `Stato: ${WW.GAME.raw.vip === "True" ? "attivo" : "non attivo"} — tempo rimanente: ${WW.GAME.raw.vip_Tempo || "0h 0m 0s"}`,
      items: [
        { nome: "VIP 24H", comandoServer: "Vip_1", costoChiave: "Pacchetto_Vip_1_Costo", rewardChiave: "Pacchetto_Vip_1_Reward", rewardUnit: "h", valuta: "viola", funzionale: true, descChiave: "Shop Vip 1", icona: "Vip.png" },
        { nome: "VIP 24H", comandoServer: "Vip_2", costoChiave: "Pacchetto_Vip_2_Costo", rewardChiave: "Pacchetto_Vip_2_Reward", rewardUnit: "h", valuta: "usdt", funzionale: false, descChiave: "Shop Vip 2", icona: "Vip.png" },
      ],
    },
    {
      titolo: "Diamanti Viola",
      items: [
        { nome: "Diamanti Viola", comandoServer: "Pacchetto_1", costoChiave: "Pacchetto_Diamanti_1_Costo", rewardChiave: "Pacchetto_Diamanti_1_Reward", rewardUnit: "diamanti", valuta: "usdt", funzionale: false },
        { nome: "Diamanti Viola", comandoServer: "Pacchetto_2", costoChiave: "Pacchetto_Diamanti_2_Costo", rewardChiave: "Pacchetto_Diamanti_2_Reward", rewardUnit: "diamanti", valuta: "usdt", funzionale: false },
        { nome: "Diamanti Viola", comandoServer: "Pacchetto_3", costoChiave: "Pacchetto_Diamanti_3_Costo", rewardChiave: "Pacchetto_Diamanti_3_Reward", rewardUnit: "diamanti", valuta: "usdt", funzionale: false },
        { nome: "Diamanti Viola", comandoServer: "Pacchetto_4", costoChiave: "Pacchetto_Diamanti_4_Costo", rewardChiave: "Pacchetto_Diamanti_4_Reward", rewardUnit: "diamanti", valuta: "usdt", funzionale: false },
      ],
    },
    {
      titolo: "Costruttori",
      statoTesto: () => `Tempo extra disponibile: ${WW.GAME.raw.Costruttori_Tempo || "0h 0m 0s"}`,
      items: [
        { nome: "Costruttori 24H", comandoServer: "Costruttori_24H", costoChiave: "Pacchetto_Costruttore_24h_Costo", rewardChiave: "Pacchetto_Costruttore_24h_Reward", rewardUnit: "h", valuta: "blu", funzionale: true, descChiave: "Shop Costruttore 24h", icona: "Addestratori.png" },
        { nome: "Costruttori 48H", comandoServer: "Costruttori_48H", costoChiave: "Pacchetto_Costruttore_48h_Costo", rewardChiave: "Pacchetto_Costruttore_48h_Reward", rewardUnit: "h", valuta: "blu", funzionale: true, descChiave: "Shop Costruttore 48h", icona: "Addestratori.png" },
      ],
    },
    {
      titolo: "Reclutatori",
      statoTesto: () => `Tempo extra disponibile: ${WW.GAME.raw.Reclutatori_Tempo || "0h 0m 0s"}`,
      items: [
        { nome: "Reclutatori 24H", comandoServer: "Reclutatori_24H", costoChiave: "Pacchetto_Reclutatore_24h_Costo", rewardChiave: "Pacchetto_Reclutatore_24h_Reward", rewardUnit: "h", valuta: "blu", funzionale: true, descChiave: "Shop Reclutatore 24h", icona: "Costruttori.jpeg" },
        { nome: "Reclutatori 48H", comandoServer: "Reclutatori_48H", costoChiave: "Pacchetto_Reclutatore_48h_Costo", rewardChiave: "Pacchetto_Reclutatore_48h_Reward", rewardUnit: "h", valuta: "blu", funzionale: true, descChiave: "Shop Reclutatore 48h", icona: "Costruttori.jpeg" },
      ],
    },
    {
      titolo: "Scudo della Pace",
      statoTesto: () => `Scudo attivo per: ${WW.GAME.raw.Scudo_Tempo || "0h 0m 0s"}`,
      items: [
        { nome: "Scudo della Pace 8H", comandoServer: "Scudo_Pace_8H", costoChiave: "Pacchetto_Scudo_Pace_8h_Costo", rewardChiave: "Pacchetto_Scudo_Pace_8h_Reward", rewardUnit: "h", valuta: "blu", funzionale: true, descChiave: "Shop Scudo Pace 8h", icona: "Scudo_Pace.png" },
        { nome: "Scudo della Pace 24H", comandoServer: "Scudo_Pace_24H", costoChiave: "Pacchetto_Scudo_Pace_24h_Costo", rewardChiave: "Pacchetto_Scudo_Pace_24h_Reward", rewardUnit: "h", valuta: "blu", funzionale: true, descChiave: "Shop Scudo Pace 24h", icona: "Scudo_Pace.png" },
        { nome: "Scudo della Pace 72H", comandoServer: "Scudo_Pace_72H", costoChiave: "Pacchetto_Scudo_Pace_72h_Costo", rewardChiave: "Pacchetto_Scudo_Pace_72h_Reward", rewardUnit: "h", valuta: "blu", funzionale: true, descChiave: "Shop Scudo Pace 72h", icona: "Scudo_Pace.png" },
      ],
    },
    {
      titolo: "GamePass",
      items: [
        { nome: "GamePass Silver", comandoServer: "GamePass_Base", costoChiave: "Pacchetto_GamePass_Base_Costo", rewardChiave: "Pacchetto_GamePass_Base_Reward", rewardUnit: "g", valuta: "usdt", funzionale: false, descChiave: "Shop GamePass Base", icona: "GamePass_Silver.png" },
        { nome: "GamePass Gold", comandoServer: "GamePass_Avanzato", costoChiave: "Pacchetto_GamePass_Avanzato_Costo", rewardChiave: "Pacchetto_GamePass_Avanzato_Reward", rewardUnit: "g", valuta: "usdt", funzionale: false, descChiave: "Shop GamePass Avanzato", icona: "GamePass_Gold.png" },
      ],
    },
  ];

  // Box descrizione ("ⓘ" sulla card): stesso meccanismo già usato in
  // 06-costruzione.js/09-ricerca.js — cache e comando "Descrizione"
  // condivisi (WW.descrizioni/WW.onDescrizione, 04-game-main.js).
  function cssEscape(str) {
    return window.CSS && CSS.escape ? CSS.escape(str) : str.replace(/["\\]/g, "\\$&");
  }

  function popolaDescrizioneShop(chiave, box) {
    const testo = WW.descrizioni[chiave];
    box.innerHTML = "";
    if (testo) {
      box.appendChild(WW.renderDescrizioneRicca(testo));
    } else {
      const p = document.createElement("span");
      p.className = "research-desc__vuoto";
      p.textContent = "Descrizione non ancora ricevuta dal server (arriva subito dopo il login).";
      box.appendChild(p);
    }
  }

  // Se un box è già aperto quando arriva/aggiorna una Descrizione, lo
  // aggiorniamo subito invece di aspettare che l'utente lo richiuda e riapra.
  WW.onDescrizione((chiave) => {
    const box = document.querySelector(`.shop-card__desc[data-desc-per="${cssEscape(chiave)}"]`);
    if (box && !box.hidden) popolaDescrizioneShop(chiave, box);
  });

  function testoDurata(item) {
    const val = WW.GAME.num(item.rewardChiave);
    if (item.rewardUnit === "g") return `${WW.fmtInt(val / 86400)} giorni`;
    if (item.rewardUnit === "diamanti") return `${WW.fmtInt(val)} Diamanti Viola`;
    return `${WW.fmtInt(val)}h`;
  }

  function testoPrezzo(item) {
    const val = WW.GAME.num(item.costoChiave);
    if (item.valuta === "viola") return `<img src="assets/DiamanteViola_V2.png" class="icon-inline" alt=""> ${WW.fmtInt(val)}`;
    if (item.valuta === "blu") return `<img src="assets/DiamanteBlu_V2.png" class="icon-inline" alt=""> ${WW.fmtInt(val)}`;
    return `$ ${WW.fmtDecimal(val, 2)}`;
  }

  // Badge rotondo in cima alla card: di norma la stessa icona della valuta
  // mostrata nel prezzo (Diamante Viola/Blu/USDT — migliorata su richiesta
  // dell'utente il 13/09/2026: "più spessore e una grafica migliore",
  // assets/USDT_Logo.png è il logo ufficiale fornito dall'utente stesso,
  // prima c'era un monogramma "T" disegnato a mano). "icona" (18/09/2026,
  // su richiesta dell'utente): per Costruttori/Reclutatori/Scudo della
  // Pace/GamePass mostra l'immagine specifica del pacchetto invece
  // dell'icona generica della valuta.
  function iconBadge(item) {
    if (item.icona) return `<span class="shop-card__icon"><img src="assets/${item.icona}" alt=""></span>`;
    if (item.valuta === "viola") return `<span class="shop-card__icon"><img src="assets/DiamanteViola_V2.png" alt=""></span>`;
    if (item.valuta === "blu") return `<span class="shop-card__icon"><img src="assets/DiamanteBlu_V2.png" alt=""></span>`;
    return `<span class="shop-card__icon shop-card__icon--usdt"><img src="assets/USDT_Logo.png" alt="USDT"></span>`;
  }

  // Pulsante "ⓘ" discreto in un angolo della card (stessa icona info.png di
  // Ricerca/Costruzione) — su richiesta dell'utente (13/09/2026: aggiungere
  // la descrizione "senza rovinare l'estetica pulita"), niente testo
  // sempre visibile: si apre solo se richiesto, e solo per i pacchetti che
  // hanno davvero una descrizione dal server (vedi nota su descChiave sopra).
  function infoBtnEDesc(item) {
    if (!item.descChiave) return "";
    return `
      <button type="button" class="shop-card__info-btn" data-info-tipo="${item.descChiave}" title="Descrizione" aria-label="Descrizione ${item.nome}"><img src="assets/info.png" alt=""></button>
      <div class="research-desc shop-card__desc" data-desc-per="${item.descChiave}" hidden></div>`;
  }

  function templateShopCard(item, indiceCategoria, indiceItem) {
    if (item.funzionale) {
      return `
      <li class="shop-card" data-cat="${indiceCategoria}" data-item="${indiceItem}" data-valuta="${item.valuta}">
        ${infoBtnEDesc(item)}
        ${iconBadge(item)}
        <span class="shop-card__nome">${item.nome}</span>
        <span class="shop-card__durata">${testoDurata(item)}</span>
        <span class="shop-card__prezzo">${testoPrezzo(item)}</span>
        <button type="button" class="btn btn--primary btn-shop-acquista">Acquista</button>
      </li>`;
    }
    return `
    <li class="shop-card shop-card--disabled" data-cat="${indiceCategoria}" data-item="${indiceItem}" data-valuta="${item.valuta}">
      ${infoBtnEDesc(item)}
      ${iconBadge(item)}
      <span class="shop-card__nome">${item.nome}</span>
      <span class="shop-card__durata">${testoDurata(item)}</span>
      <span class="shop-card__prezzo shop-card__prezzo--usdt">${testoPrezzo(item)}</span>
      <button type="button" class="btn btn--ghost" disabled>Acquista</button>
      <span class="shop-card__prossimamente">Pagamento reale non ancora attivo</span>
    </li>`;
  }

  function templateCategoria(cat, indiceCategoria) {
    const hint = cat.statoTesto ? `<p class="panel__hint">${cat.statoTesto()}</p>` : "";
    const cards = cat.items.map((item, i) => templateShopCard(item, indiceCategoria, i)).join("");
    return `
    <h3 class="panel__subtitle">${cat.titolo}</h3>
    ${hint}
    <ul class="shop-list">${cards}</ul>`;
  }

  function renderShop() {
    const container = document.getElementById("shop-container");
    if (!container) return;
    // Ricostruiamo tutto il markup ad ogni tick: qui, a differenza di
    // Costruzione/Città, non c'è uno stato di input dell'utente da
    // preservare tra un render e l'altro (niente stepper), quindi non
    // serve la logica "costruisci solo alla prima volta". L'unico stato da
    // salvare (13/09/2026, aggiunta dei box "ⓘ") sono i box descrizione
    // già aperti: senza questo si richiuderebbero da soli al tick
    // successivo, un attimo dopo averli aperti.
    const aperti = Array.from(container.querySelectorAll(".shop-card__desc:not([hidden])")).map((b) => b.dataset.descPer);
    container.innerHTML = SHOP_CATEGORIE.map(templateCategoria).join("");
    aperti.forEach((chiave) => {
      const box = container.querySelector(`.shop-card__desc[data-desc-per="${cssEscape(chiave)}"]`);
      const btn = container.querySelector(`.shop-card__info-btn[data-info-tipo="${cssEscape(chiave)}"]`);
      if (!box) return;
      box.hidden = false;
      if (btn) btn.classList.add("is-active");
      popolaDescrizioneShop(chiave, box);
    });
  }

  // Un solo listener delegato sul contenitore: gestisce il click su
  // "Acquista" per qualsiasi card, comprese quelle ricostruite ad ogni
  // renderShow(). Le card disattivate hanno il pulsante con "disabled",
  // quindi il click non genera nemmeno l'evento.
  const shopContainer = document.getElementById("shop-container");
  if (shopContainer) {
    shopContainer.addEventListener("click", (e) => {
      const infoBtn = e.target.closest(".shop-card__info-btn");
      if (infoBtn) {
        const box = infoBtn.closest(".shop-card").querySelector(".shop-card__desc");
        const apri = box.hidden;
        box.hidden = !apri;
        infoBtn.classList.toggle("is-active", apri);
        if (apri) popolaDescrizioneShop(infoBtn.dataset.infoTipo, box);
        return;
      }

      const btn = e.target.closest(".btn-shop-acquista");
      if (!btn) return;
      const li = btn.closest(".shop-card");
      const cat = SHOP_CATEGORIE[Number(li.dataset.cat)];
      const item = cat.items[Number(li.dataset.item)];
      if (!item || !item.funzionale) return;
      WW.NET.send("Shop", WW.AUTH.accessToken, item.comandoServer);
    });
  }

  WW.renderShop = renderShop;
})(window.WW);
