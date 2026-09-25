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
    // 23/09/2026: unico item USDT davvero collegato end-to-end al pagamento crypto (il resto
    // sopra resta "Prossimamente" finché non viene agganciato l'accredito automatico lato server,
    // vedi Shop.AccreditaAcquisto) — serve per i primi test reali su mainnet. Da rimuovere (o
    // lasciare, è innocuo) una volta finiti i test.
    {
      titolo: "Test pagamento USDT",
      items: [
        { nome: "Test USDT → Diamanti Viola", comandoServer: "Test_USDT", costoChiave: "Pacchetto_Test_USDT_Costo", rewardChiave: "Pacchetto_Test_USDT_Reward", rewardUnit: "diamanti", valuta: "usdt", funzionale: true },
      ],
    },
  ];

  // Box descrizione ("ⓘ" sulla card): stesso meccanismo già usato in
  // 06-costruzione.js/09-ricerca.js — cache e comando "Descrizione"
  // condivisi (WW.descrizioni/WW.onDescrizione, 04-game-main.js).
  const cssEscape = WW.cssEscape; // 00-core.js

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

  /* ---------- Feedback grafico all'acquisto ----------
     (20/09/2026, su richiesta dell'utente: "vorrei che quando un utente
     acquista un elemento dello shop ci possa essere qualche effetto visivo
     in più"). PRIMA VERSIONE: scattava confrontando il "tempo rimanente"
     prima/dopo ogni tick, per innescarsi solo alla conferma vera del
     server. Rimossa (20/09/2026, "lo shop pulsa... come se venisse
     aggiornato perennemente"): il testo del tempo rimanente mandato dal
     server (Giocatori.cs, FormatTime) OMETTE "h"/"m" quando sono zero
     ("45s" sotto il minuto, "12m 3s" sotto l'ora, aggiunge "Xd " sopra le
     24h) — un parsing basato su quel testo è quindi inaffidabile e può
     scattare a vuoto. Ora l'effetto è innescato direttamente dal click su
     "Acquista" (ottimistico: parte subito, senza aspettare conferma dal
     server), usando la durata già nota lato client (stesso valore mostrato
     sulla card, da rewardChiave — nessun parsing di stringhe di tempo). */
  let toastTimeout = null;

  function mostraToastShop(testo) {
    const elToast = document.getElementById("shop-toast");
    if (!elToast) return;
    elToast.textContent = testo;
    elToast.hidden = false;
    // Riavvia l'animazione di comparsa anche se il toast è già visibile
    // (acquisti multipli ravvicinati) — stesso trucco di mostraToastQuest().
    elToast.classList.remove("quest-toast--anim");
    void elToast.offsetWidth;
    elToast.classList.add("quest-toast--anim");
    clearTimeout(toastTimeout);
    toastTimeout = setTimeout(() => { elToast.hidden = true; }, 2200);
  }

  // Chiamata subito al click su "Acquista" (prima ancora della risposta del
  // server — vedi nota sopra). "li" è la card cliccata: essendo TUTTO il
  // markup ricostruito ad ogni tick (vedi renderShop), se un aggiornamento
  // arriva a metà animazione la card viene sostituita e l'effetto si
  // interrompe semplicemente un po' prima — innocuo, meglio che rischiare
  // di non vederlo mai scattare.
  function segnalaAcquisto(li, item) {
    li.classList.add("shop-card--acquistata");
    li.addEventListener("animationend", () => li.classList.remove("shop-card--acquistata"), { once: true });

    const popup = document.createElement("span");
    popup.className = "shop-reward-popup";
    popup.textContent = `+${testoDurata(item)}`;
    li.appendChild(popup);
    popup.addEventListener("animationend", () => popup.remove(), { once: true });

    mostraToastShop(`Acquisto inviato: ${item.nome} (+${testoDurata(item)})`);
  }

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
      const catIdx = Number(li.dataset.cat);
      const itemIdx = Number(li.dataset.item);
      const item = SHOP_CATEGORIE[catIdx].items[itemIdx];
      if (!item || !item.funzionale) return;

      // Gli item in USDT non accreditano nulla all'istante (serve prima il pagamento reale e le
      // conferme sulla rete, vedi apriPagamento più sotto): niente animazione "+ricompensa"
      // ottimistica come per i Diamanti, solo un avviso che la richiesta è partita — il popup di
      // pagamento si apre da solo appena il server risponde "Pagamento|Creato".
      if (item.valuta === "usdt") {
        mostraToastShop(`Richiesta di pagamento inviata: ${item.nome}…`);
      } else {
        segnalaAcquisto(li, item);
      }
      WW.NET.send("Shop", WW.AUTH.accessToken, item.comandoServer);
    });
  }

  /* ---------- Popup "Pagamento USDT" (QR + stato) ----------
     23/09/2026, su richiesta dell'utente ("puoi aggiornare il client...").
     Si apre da solo quando il server risponde a un acquisto USDT con
     "Pagamento|Creato|..." (vedi Shop.cs -> BlockchainManager.AvviaPagamentoItem),
     mostra il QR code (precompila la transazione nel wallet dell'utente, che resta
     comunque libero di ricontrollare/completare a mano — mai un invio automatico
     dal sito) e tiene lo stato aggiornato interrogando il server ogni 8s finché la
     bill non è pagata/scaduta/annullata. Gli errori (bill non trovata, hash già
     usato, ecc.) arrivano come "Log_Server|..." e sono già mostrati in Cronologia
     (WW.NET.on("Log_Server", ...) in 04-game-main.js) — qui non li duplichiamo. */
  const pagamentoOverlay = document.getElementById("pagamento-overlay");
  const pagamentoItemEl = document.getElementById("pagamento-item");
  const pagamentoQrEl = document.getElementById("pagamento-qr");
  const pagamentoImportoEl = document.getElementById("pagamento-importo");
  const pagamentoDestinatarioEl = document.getElementById("pagamento-destinatario");
  const pagamentoReteEl = document.getElementById("pagamento-rete");
  const pagamentoScadenzaEl = document.getElementById("pagamento-scadenza");
  const pagamentoStatoEl = document.getElementById("pagamento-stato");
  const pagamentoHashInput = document.getElementById("pagamento-hash-input");
  const btnPagamentoVerifica = document.getElementById("btn-pagamento-verifica");
  const btnPagamentoAnnulla = document.getElementById("btn-pagamento-annulla");
  const btnChiudiPagamento = document.getElementById("btn-chiudi-pagamento");
  const btnPagamentoSwitchQr = document.getElementById("btn-pagamento-switch-qr");
  const pagamentoSwitchQrTestoEl = document.getElementById("pagamento-switch-qr-testo");

  const CHAIN_NAMES = { 137: "Polygon (mainnet)", 80002: "Polygon Amoy (testnet)" };

  let billAttiva = null; // { orderId, expiresAt: Date }
  let pollTimer = null;
  let countdownTimer = null;

  // 25/09/2026: i due QR della bill attiva, per poter passare dall'uno all'altro col pulsante
  // senza dover richiedere nulla al server (sono già arrivati insieme in "Creato").
  let qrCompletoAttivo = null;
  let qrSempliceAttivo = null;
  let mostraQrSemplice = false; // riparte da "completo" ad ogni nuova bill aperta

  function fermaPolling() {
    if (pollTimer) { clearInterval(pollTimer); pollTimer = null; }
    if (countdownTimer) { clearInterval(countdownTimer); countdownTimer = null; }
  }

  function chiudiPagamento() {
    fermaPolling();
    billAttiva = null;
    if (pagamentoOverlay) pagamentoOverlay.hidden = true;
  }

  function impostaStatoPagamento(classeExtra, testo) {
    if (!pagamentoStatoEl) return;
    pagamentoStatoEl.textContent = testo;
    pagamentoStatoEl.className = "pagamento__stato" + (classeExtra ? ` pagamento__stato--${classeExtra}` : "");
  }

  function testoStatoBill(status) {
    switch (status) {
      case "pending": return { classe: "", testo: "In attesa del pagamento…" };
      case "confirming": return { classe: "confirming", testo: "Pagamento rilevato, in attesa delle conferme sulla rete…" };
      case "paid": return { classe: "paid", testo: "Pagamento confermato! Ricompensa accreditata." };
      case "expired": return { classe: "scaduto", testo: "Richiesta scaduta." };
      case "cancelled": return { classe: "scaduto", testo: "Richiesta annullata." };
      default: return { classe: "", testo: `Stato: ${status}` };
    }
  }

  function aggiornaCountdownPagamento() {
    if (!billAttiva || !pagamentoScadenzaEl) return;
    const ms = billAttiva.expiresAt.getTime() - Date.now();
    if (ms <= 0) {
      pagamentoScadenzaEl.textContent = "Richiesta scaduta.";
      impostaStatoPagamento("scaduto", "Richiesta scaduta — annulla e riprova.");
      fermaPolling();
      return;
    }
    const minuti = Math.floor(ms / 60000);
    const secondi = Math.floor((ms % 60000) / 1000);
    pagamentoScadenzaEl.textContent = `Scade tra ${minuti}m ${secondi}s`;
  }

  // Aggiorna solo l'immagine + il testo del pulsante in base a mostraQrSemplice, senza toccare
  // il resto del popup — richiamata sia all'apertura sia al click sul pulsante di switch.
  function aggiornaQrMostrato() {
    if (pagamentoQrEl) {
      const base64 = mostraQrSemplice ? qrSempliceAttivo : qrCompletoAttivo;
      if (base64) pagamentoQrEl.src = `data:image/png;base64,${base64}`;
    }
    if (pagamentoSwitchQrTestoEl) {
      pagamentoSwitchQrTestoEl.textContent = mostraQrSemplice
        ? "Torna al QR con importo precompilato"
        : "Prova la versione solo indirizzo";
    }
  }

  function apriPagamento(dati) {
    fermaPolling();
    billAttiva = { orderId: dati.orderId, expiresAt: new Date(dati.expiresAt) };
    qrCompletoAttivo = dati.qrCompletoBase64;
    qrSempliceAttivo = dati.qrSempliceBase64;
    mostraQrSemplice = false; // ogni nuova bill riparte mostrando il QR completo

    if (pagamentoItemEl) pagamentoItemEl.textContent = `Acquisto: ${dati.itemId}`;
    aggiornaQrMostrato();
    if (pagamentoImportoEl) pagamentoImportoEl.textContent = `${dati.importoEsatto} USDT`;
    if (pagamentoDestinatarioEl) pagamentoDestinatarioEl.textContent = dati.recipient;
    if (pagamentoReteEl) pagamentoReteEl.textContent = CHAIN_NAMES[dati.chainId] || `Chain ID ${dati.chainId}`;
    if (pagamentoHashInput) pagamentoHashInput.value = "";
    impostaStatoPagamento("", "In attesa del pagamento…");
    aggiornaCountdownPagamento();

    if (pagamentoOverlay) pagamentoOverlay.hidden = false;

    countdownTimer = setInterval(aggiornaCountdownPagamento, 1000);
    // Interroga lo stato ogni 8s finché la bill resta aperta — smette da sola su paid/expired/
    // cancelled (vedi handler "Stato" sotto) o quando l'utente chiude/annulla il popup.
    pollTimer = setInterval(() => {
      if (!billAttiva) return;
      WW.NET.send("Pagamento", WW.AUTH.accessToken, "Stato", billAttiva.orderId);
    }, 8000);
  }

  if (btnPagamentoSwitchQr) {
    btnPagamentoSwitchQr.addEventListener("click", () => {
      if (!qrSempliceAttivo || !qrCompletoAttivo) return; // bill non ancora aperta
      mostraQrSemplice = !mostraQrSemplice;
      aggiornaQrMostrato();
    });
  }

  if (btnChiudiPagamento) btnChiudiPagamento.addEventListener("click", chiudiPagamento);
  if (pagamentoOverlay) {
    pagamentoOverlay.addEventListener("click", (e) => { if (e.target === pagamentoOverlay) chiudiPagamento(); });
  }
  document.addEventListener("keydown", (e) => {
    if (e.key === "Escape" && pagamentoOverlay && !pagamentoOverlay.hidden) chiudiPagamento();
  });

  if (btnPagamentoAnnulla) {
    btnPagamentoAnnulla.addEventListener("click", () => {
      if (!billAttiva) { chiudiPagamento(); return; }
      WW.NET.send("Pagamento", WW.AUTH.accessToken, "Annulla", billAttiva.orderId);
    });
  }

  if (btnPagamentoVerifica) {
    btnPagamentoVerifica.addEventListener("click", () => {
      if (!billAttiva) return;
      const hash = ((pagamentoHashInput && pagamentoHashInput.value) || "").trim();
      if (!hash) { mostraToastShop("Inserisci l'hash della transazione prima di verificare."); return; }
      impostaStatoPagamento("", "Verifica in corso…");
      WW.NET.send("Pagamento", WW.AUTH.accessToken, "DichiaraHash", billAttiva.orderId, hash);
    });
  }

  // Copia rapida (importo/indirizzo): navigator.clipboard può non essere disponibile (pagina non
  // servita in HTTPS, browser datato) — fallback silenzioso, il testo resta comunque leggibile e
  // selezionabile a mano dentro il box.
  document.addEventListener("click", (e) => {
    const btn = e.target.closest(".pagamento__copia");
    if (!btn) return;
    const target = document.getElementById(btn.dataset.copyTarget);
    if (!target) return;
    if (navigator.clipboard && navigator.clipboard.writeText) {
      navigator.clipboard.writeText(target.textContent).catch(() => {});
    }
    const originale = btn.textContent;
    btn.textContent = "Copiato!";
    btn.classList.add("pagamento__copia--fatto");
    setTimeout(() => {
      btn.textContent = originale;
      btn.classList.remove("pagamento__copia--fatto");
    }, 1500);
  });

  WW.NET.on("Pagamento", (args) => {
    const sotto = args[0];
    switch (sotto) {
      case "Creato": {
        // 25/09/2026: il server ora manda DUE QR — qrCompletoBase64 (EIP-681, precompila
        // destinatario+importo) e qrSempliceBase64 (solo indirizzo, compatibile con più wallet/
        // app exchange) — vedi apriPagamento e il pulsante di switch qui sotto.
        const [, orderId, itemId, importoEsatto, recipient, chainId, expiresAt, uri, qrCompletoBase64, qrSempliceBase64] = args;
        apriPagamento({ orderId, itemId, importoEsatto, recipient, chainId: Number(chainId), expiresAt, uri, qrCompletoBase64, qrSempliceBase64 });
        break;
      }
      case "Stato": {
        const [, orderId, status] = args;
        if (!billAttiva || billAttiva.orderId !== orderId) return;
        const { classe, testo } = testoStatoBill(status);
        impostaStatoPagamento(classe, testo);
        if (status === "paid" || status === "expired" || status === "cancelled") fermaPolling();
        break;
      }
      case "Verificato": {
        const [, orderId] = args;
        if (!billAttiva || billAttiva.orderId !== orderId) return;
        impostaStatoPagamento("confirming", "Transazione trovata — in attesa delle conferme sulla rete…");
        break;
      }
      case "Annullato": {
        const [, orderId] = args;
        if (billAttiva && billAttiva.orderId === orderId) {
          mostraToastShop("Richiesta di pagamento annullata.");
          chiudiPagamento();
        }
        break;
      }
      case "Bill":
        // Elenco bill (risposta a "Pagamento|Lista") — non ancora usato in UI.
        break;
      default:
        console.log(`[Pagamento] Sotto-comando non gestito: "${sotto}"`, args);
    }
  });

  WW.renderShop = renderShop;
})(window.WW);
