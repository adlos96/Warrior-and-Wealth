/* ==========================================================
   Warrior & Wealth — Web Client — 04-game-main.js
   ----------------------------------------------------------
   GAME — stato ricevuto dal server (Update_Data) — e schermata
   MAIN: barra risorse, Feudi, Strutture/Esercito (sola lettura,
   il form con gli stepper per costruire/addestrare è in
   06-costruzione.js), Cronologia (log), tempo rimanente +
   pulsanti Velocizza.

   GAME.raw accumula tutte le coppie chiave=valore ricevute via
   "Update_Data". Le chiavi qui sotto sono quelle vere, prese da
   PlayerSnapshot.BuildCurrentState lato server (il "tick" che il
   game loop manda ad ogni client connesso tramite
   ServerConnection.Update_Data(guid, player), non solo quelle
   "one time" del login): dalla porta 8444 arrivano quindi gli
   stessi identici valori che vede il client desktop.

   Nota sul formato dei numeri: il server serializza i valori con
   ToString("#,0"/"#,0.00"/ecc.) sotto cultura it-IT (impostata in
   Program.cs), quindi "." è il separatore delle migliaia e ","
   quello decimale (es. "30.000" = trentamila, "1.200,50" = milleduecento
   virgola cinquanta) — l'OPPOSTO della convenzione inglese. parseServerNumber
   qui sotto interpreta sempre i valori in arrivo come italiani, poi la UI li
   ri-formatta con Intl.NumberFormat nella lingua del dispositivo di chi
   gioca (vedi WW.fmtInt/WW.fmtDecimal in 00-core.js), così chi guarda da un
   paese anglosassone vede comunque "30,000" e non si confonde.

   renderAllFromServer() è l'orchestratore chiamato ad ogni "tick": richiama
   sia le render locali a questo file (Main) sia quelle di Costruzione
   (06-costruzione.js), Città (07-citta.js), Negozio (08-shop.js) e Ricerca
   (09-ricerca.js) tramite WW.renderStruttureListForm/WW.renderUnitaForm/
   WW.renderSbloccoUnita/WW.renderCittaList/WW.renderShop/WW.renderRicerca —
   per questo quei file devono essere caricati PRIMA che renderAllFromServer()
   venga davvero invocata la prima volta (avviene in 10-main.js, caricato per
   ultimo: l'ordine relativo tra questo file e costruzione/città/negozio/
   ricerca non conta, basta che siano tutti pronti prima del bootstrap finale). */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  function parseServerNumber(str) {
    if (str === undefined || str === null) return 0;
    const pulito = String(str).replace(/%$/, "").trim().replace(/\./g, "").replace(",", ".");
    const n = Number(pulito);
    return Number.isNaN(n) ? 0 : n;
  }

  const GAME = {
    raw: Object.create(null),

    applyUpdateData(args) {
      // Caso speciale: "Update_Data|Report_Lista|<json>" (referti di battaglia — vedi
      // Player.Report, serializzato in JSON da ServerConnection.cs al login e ora anche
      // subito dopo ogni battaglia PVP/PVE, BattagliaPVP.cs/BattagliaPVE.cs). Non è una
      // lista di coppie chiave=valore come il resto di Update_Data: va intercettato PRIMA
      // del parsing generico sotto, altrimenti il JSON (senza "=") viene scartato in
      // silenzio dal forEach (bug segnalato dall'utente il 14/09/2026, mai risolto finché
      // non è servito per la schermata PVP/PVE — vedi js/14-battaglia.js).
      if (args[0] === "Report_Lista") {
        const jsonPayload = args.slice(1).join("|"); // il JSON stesso non contiene mai "|", ma per sicurezza si ricompone comunque
        let reports;
        try {
          reports = JSON.parse(jsonPayload);
        } catch (e) {
          console.warn("[GAME] Report_Lista: JSON non valido", e, jsonPayload);
          return;
        }
        if (WW.BATTLE && WW.BATTLE.setReports) WW.BATTLE.setReports(reports);
        return;
      }

      // Caso speciale: "Update_Data|Cronologia_Lista|<json>" (cronologia messaggi
      // salvata lato server, player.Cronologia — vedi Server.cs/ServerConnection.cs,
      // 16/09/2026 su richiesta dell'utente). Stesso motivo del caso Report_Lista sopra:
      // JSON, non coppie chiave=valore, va intercettato prima del parsing generico.
      // Sostituisce sempre l'intero contenuto del log-box (non accoda) sia al login
      // sia dopo "Elimina_Cronologia", perché il server è la fonte di verità.
      if (args[0] === "Cronologia_Lista") {
        const jsonPayload = args.slice(1).join("|");
        let cronologia;
        try {
          cronologia = JSON.parse(jsonPayload);
        } catch (e) {
          console.warn("[GAME] Cronologia_Lista: JSON non valido", e, jsonPayload);
          return;
        }
        if (typeof setCronologia === "function") setCronologia(cronologia);
        return;
      }

      args.forEach((coppia) => {
        const idx = coppia.indexOf("=");
        if (idx === -1) return;
        GAME.raw[coppia.slice(0, idx)] = coppia.slice(idx + 1);
      });
      renderAllFromServer();
    },

    // Legge una chiave grezza come numero (0 se non ancora arrivata dal server).
    num(chiave) {
      return parseServerNumber(GAME.raw[chiave]);
    },
  };

  function renderAllFromServer() {
    renderRisorseBar();
    renderFeudi();
    renderStruttureList("civili-list", struttureCivili);
    renderStruttureList("militari-list", struttureMilitari);
    renderStruttureList("caserme-list", caserme);
    renderUnita();
    renderVarie();
    WW.renderStruttureListForm("costruzione-civili-list", struttureCivili);
    WW.renderStruttureListForm("costruzione-militari-list", struttureMilitari);
    WW.renderStruttureListForm("costruzione-caserme-list", caserme);
    WW.renderUnitaForm();
    WW.renderSbloccoUnita();
    WW.renderCittaList();
    WW.renderShop();
    WW.renderRicerca();
    WW.renderStatistiche();
    WW.renderQuestBarra(); // barra punti + marker Quest (12-quest.js): ricalcolati ad ogni tick, non solo su un nuovo QuestRewards (bugfix 14/09/2026)
    if (WW.renderGamepass) WW.renderGamepass(); // griglia premi GamePass (13-gamepass.js)
    if (WW.renderBattaglia) WW.renderBattaglia(); // schermata PVP/PVE (14-battaglia.js)
  }

  // Costo del prossimo Feudo e Costruttori/Reclutatori attualmente occupati
  // sul totale: valori reali mandati dal server (rispettivamente
  // "costo_terreni_Virtuali" via Update_Data_OneTime al login, e
  // "Code_Costruzioni_Disponibili"/"Code_Costruzioni" ad ogni tick tramite
  // PlayerSnapshot), al posto dei numeri fissi che c'erano nell'HTML del
  // mockup.
  //
  // ATTENZIONE al nome "Code_Costruzioni_Disponibili": nonostante il nome
  // NON è già il numero pronto da mostrare — mostrarlo direttamente (come
  // faceva questo file prima) restava fisso al totale massimo anche con
  // strutture davvero in costruzione (bug segnalato dall'utente 13/09/2026:
  // "1/1" invece di "0/1" mentre il tempo scorreva). Il client desktop
  // (GUI/Gioco.cs, lbl_Coda_Costruzione/lbl_Coda_Reclutamento, già corretto
  // lì dall'utente) calcola invece Attuali = Totale - Disponibili, quindi
  // qui replichiamo la stessa sottrazione invece di usare "Disponibili"
  // così com'è.
  function renderVarie() {
    const elCosto = document.querySelector('[data-value="costo-feudo"]');
    if (elCosto) elCosto.textContent = WW.fmtInt(GAME.num("costo_terreni_Virtuali"));

    const elCodeCostruzioni = document.querySelector('[data-value="code-costruzioni"]');
    if (elCodeCostruzioni) {
      const totaleCostruzioni = GAME.num("Code_Costruzioni");
      const attualiCostruzioni = totaleCostruzioni - GAME.num("Code_Costruzioni_Disponibili");
      elCodeCostruzioni.textContent = `${WW.fmtInt(attualiCostruzioni)}/${WW.fmtInt(totaleCostruzioni)}`;
    }

    const elCodeReclutamenti = document.querySelector('[data-value="code-reclutamenti"]');
    if (elCodeReclutamenti) {
      const totaleReclutamenti = GAME.num("Code_Reclutamenti");
      const attualiReclutamenti = totaleReclutamenti - GAME.num("Code_Reclutamenti_Disponibili");
      elCodeReclutamenti.textContent = `${WW.fmtInt(attualiReclutamenti)}/${WW.fmtInt(totaleReclutamenti)}`;
    }

    // Tempo totale rimanente in coda (schermata Main, pannelli Strutture ed
    // Esercito): il server manda già una stringa pronta ("2h 0m 0s", vedi
    // BuildingManagerV2.Get_Total_Building_Time/UnitManagerV2.Get_Total_
    // Recruit_Time) — usiamo GAME.raw direttamente, senza passare da
    // GAME.num() che è pensato per i valori numerici. Riga e pulsante
    // "Velocizza" restano nascosti finché non c'è davvero tempo da mostrare
    // (niente "Tempo rimanente: 0h 0m 0s" quando non si sta costruendo/
    // addestrando nulla).
    aggiornaTempoECodaVelocizza("Tempo_Costruzione", "[data-tempo-costruzione-row]", "tempo-costruzione", "btn-toggle-velocizza-costruzione", "form-velocizza-costruzione");
    aggiornaTempoECodaVelocizza("Tempo_Reclutamento", "[data-tempo-reclutamento-row]", "tempo-reclutamento", "btn-toggle-velocizza-reclutamento", "form-velocizza-reclutamento");

    // Rapporti di scambio/velocizzazione: valori reali mandati una tantum al
    // login (Update_Data_OneTime → "D_Viola_D_Blu"/"Tributi_D_Viola"/
    // "Tempo_D_Blu" in ServerConnection.cs), non fissi in JS.
    const rapportoVB = WW.fmtInt(GAME.num("D_Viola_D_Blu"));
    document.querySelectorAll('[data-value="ratio-viola-blu"]').forEach((el) => (el.textContent = rapportoVB));

    const rapportoTV = WW.fmtInt(GAME.num("Tributi_D_Viola"));
    document.querySelectorAll('[data-value="ratio-tributi-viola"]').forEach((el) => (el.textContent = rapportoTV));

    const rapportoVelocizza = `${WW.fmtInt(GAME.num("Tempo_D_Blu"))}s`;
    document
      .querySelectorAll('[data-value="ratio-velocizza-tempo"], [data-value="ratio-velocizza-tempo-2"]')
      .forEach((el) => (el.textContent = rapportoVelocizza));
  }

  // Stima se una stringa di tempo già formattata dal server ("2h 0m 0s")
  // rappresenta più di zero secondi, sommando tutti i numeri che contiene:
  // non serve un valore esatto, solo capire se mostrare o no la riga.
  function tempoMaggioreDiZero(str) {
    if (!str) return false;
    const numeri = str.match(/\d+/g);
    if (!numeri) return false;
    return numeri.some((n) => Number(n) > 0);
  }

  // Aggiorna la riga "Tempo rimanente" e mostra/nasconde riga + pulsante
  // "Velocizza" a seconda che ci sia davvero qualcosa in coda. Se il
  // pulsante viene nascosto mentre il suo mini-form era aperto, lo richiude
  // e azzera lo stepper (altrimenti resterebbe un form orfano visibile).
  function aggiornaTempoECodaVelocizza(chiaveGrezza, selettoreRiga, chiaveValore, idBottoneVelocizza, idFormVelocizza) {
    const testoTempo = GAME.raw[chiaveGrezza];
    const c_e_tempo = tempoMaggioreDiZero(testoTempo);

    const riga = document.querySelector(selettoreRiga);
    if (riga) {
      riga.hidden = !c_e_tempo;
      if (c_e_tempo) {
        const valoreEl = riga.querySelector(`[data-value="${chiaveValore}"]`);
        if (valoreEl) valoreEl.textContent = testoTempo;
      }
    }

    const bottoneVelocizza = document.getElementById(idBottoneVelocizza);
    if (bottoneVelocizza) bottoneVelocizza.hidden = !c_e_tempo;

    if (!c_e_tempo) {
      const form = document.getElementById(idFormVelocizza);
      if (form && !form.hidden) {
        form.hidden = true;
        const stepper = WW.VELOCIZZA_STEPPERS[idBottoneVelocizza];
        if (stepper) stepper.set(0);
      }
    }
  }

  // Cronologia: il server manda "Log_Server|testo" per gli eventi di gioco,
  // dove "testo" usa una sintassi BBCode-like con tag colore ([tag]...[/tag])
  // e icone inline ([icon:nome]) — la stessa che il client desktop
  // interpreta in Strumenti/LogSupport.cs. Qui la replichiamo per il web:
  // stessa logica di parsing (un tag di chiusura qualsiasi torna al colore
  // di default, un tag sconosciuto viene ignorato), colori riadattati per
  // leggibilità su sfondo chiaro (pergamena) invece che sullo sfondo scuro
  // del client desktop.
  const LOG_COLORS = {
    default: "var(--ink)",
    black: "#000000",
    verde: "#2e7d32",
    rosso: "#8b0000",
    ferroScuro: "#32323c",
    verdeF: "#22502c",
    bluGotico: "#24345e",
    porporaReale: "#551e4b",
    acciaioBlu: "#3f5468",
    arancione: "#c04400",
    ocraDorata: "#9c6f0a",
    bluNotte: "#1a2a33",
    success: "#1e8f4e",
    warning: "#a16a00",
    error: "#a33f3f",
    cibo: "#a35b00",
    legno: "#6b4423",
    pietra: "#5f5f5f",
    ferro: "#5a5a63",
    oro: "#9c7a00",
    popolazione: "#3f5468",
    viola: "#7d3c98",
    blu: "#2874a6",
    info: "#1f6fa5",
    title: "#5a3a22",
    highlight: "#8a6d00",
    TerrenoComune: "#6b6b6b",
    TerrenoNoncomune: "#1e8e1e",
    TerrenoRaro: "#0056c7",
    TerrenoEpico: "#8a1cd6",
    TerrenoLeggendario: "#a68b00",
  };
  // Tag che nel client desktop indicano enfasi (titolo/evidenza): qui li
  // rendiamo anche in grassetto oltre che a colore, per farli risaltare
  // nello stesso modo.
  const LOG_BOLD_TAGS = new Set(["title", "highlight"]);

  // icon:nome -> file in assets/. Include anche i refusi che compaiono
  // davvero nei messaggi lato server (es. "icon:arcere", "icon:guerriero",
  // "icon:lancere" invece delle chiavi "ufficiali" arceri/guerrieri/lanceri):
  // nel client desktop restano senza immagine per la chiave mancante, qui
  // li mappiamo comunque così l'icona compare per davvero.
  const LOG_ICONS = {
    xp: "Exp_1.png",
    lv: "Livello_V2.png",
    cibo: "Grano_V2.png",
    legno: "Legna_V2.png",
    pietra: "Pietra_V2.png",
    ferro: "Ferro_V2.png",
    oro: "Oro_V2.png",
    popolazione: "Popolazione_V2.png",
    diamanteBlu: "DiamanteBlu_V2.png",
    diamanteViola: "DiamanteViola_V2.png",
    dollariVirtuali: "Tributi_V2.png",
    spade: "Spade_V2.png",
    lance: "Lance_V2.png",
    archi: "Archi_V2.png",
    scudi: "Scudi_V2.png",
    armature: "Armature_V2.png",
    frecce: "Frecce_V2.png",
    guerrieri: "Guerriero_V2.png",
    guerriero: "Guerriero_V2.png",
    lanceri: "Lanciere_V2.png",
    lancere: "Lanciere_V2.png",
    arceri: "Arciere_V2.png",
    arcere: "Arciere_V2.png",
    arciere: "Arciere_V2.png",
    catapulte: "Catapulta_V2.png",
    catapulta: "Catapulta_V2.png",
    fattoria: "Fattoria_V2.png",
    segheria: "Segheria_V2.png",
    cavaPietra: "CavaDiPietra_V2.png",
    minieraFerro: "MinieraFerro_V2.png",
    minieraOro: "MinieraOro_V2.png",
    case: "Abitazioni_V2.png",
    workshopSpade: "Workshop_Spade_V2.png",
    workshopLance: "Workshop_Lance_V2.png",
    workshopArchi: "Workshop_Archi_V2.png",
    workshopScudi: "Workshop_Scudi_V2.png",
    workshopArmture: "Workshop_Armature_V2.png",
    workshopFrecce: "Workshop_Frecce_V2.png",
    casermaGuerrieri: "Caserma_Guerieri_V2.png",
    casermaLanceri: "Caserma_Lanceri_V2.png",
    casermaArceri: "Caserma_Arcieri_V2.png",
    casermaCatapulte: "Caserma_Catapulte_V2.png",
    // "tempo", "usdt" e "scambio" sono gestiti a parte / senza asset, vedi sotto.
  };

  // Analizza il testo con la sintassi BBCode-like del server e restituisce
  // una lista di "segmenti" (testo colorato, o icona) da trasformare in DOM.
  function parseLogMessage(msg) {
    const segmenti = [];
    let i = 0;
    let coloreCorrente = LOG_COLORS.default;
    let grassettoCorrente = false;
    let testoCorrente = "";

    function flush() {
      if (testoCorrente.length > 0) {
        segmenti.push({ tipo: "testo", testo: testoCorrente, colore: coloreCorrente, grassetto: grassettoCorrente });
        testoCorrente = "";
      }
    }

    while (i < msg.length) {
      if (msg[i] === "[") {
        const closeIdx = msg.indexOf("]", i);
        if (closeIdx === -1) {
          // Tag non chiuso: trattalo come testo normale (stesso comportamento del client desktop).
          testoCorrente += msg[i];
          i++;
          continue;
        }
        flush();
        const tag = msg.slice(i + 1, closeIdx);
        if (tag.startsWith("/")) {
          coloreCorrente = LOG_COLORS.default;
          grassettoCorrente = false;
        } else if (tag.startsWith("icon:")) {
          const nome = tag.slice(5);
          if (nome === "tempo") {
            segmenti.push({ tipo: "icona-tempo" });
          } else if (LOG_ICONS[nome]) {
            segmenti.push({ tipo: "icona", file: LOG_ICONS[nome] });
          }
          // icona sconosciuta/senza asset: ignorata silenziosamente.
        } else if (Object.prototype.hasOwnProperty.call(LOG_COLORS, tag)) {
          coloreCorrente = LOG_COLORS[tag];
          grassettoCorrente = LOG_BOLD_TAGS.has(tag);
        }
        // Tag sconosciuto: ignorato, il testo prosegue con il colore corrente.
        i = closeIdx + 1;
      } else {
        testoCorrente += msg[i];
        i++;
      }
    }
    flush();
    return segmenti;
  }

  function renderLogSegments(segmenti) {
    const frag = document.createDocumentFragment();
    segmenti.forEach((seg) => {
      if (seg.tipo === "testo") {
        const span = document.createElement("span");
        span.textContent = seg.testo;
        span.style.color = seg.colore;
        if (seg.grassetto) span.style.fontWeight = "700";
        frag.appendChild(span);
      } else if (seg.tipo === "icona") {
        const img = document.createElement("img");
        img.src = `assets/${seg.file}`;
        img.alt = "";
        img.className = "log-icon";
        frag.appendChild(img);
      } else if (seg.tipo === "icona-tempo") {
        const span = document.createElement("span");
        span.className = "log-icon log-icon--tempo";
        frag.appendChild(span);
      }
    });
    return frag;
  }

  // 16/09/2026, su richiesta dell'utente: la Cronologia ora è paginata (10 alla
  // volta, con "‹ Indietro"/"Avanti ›") invece di un'unica lista che cresceva a
  // dismisura con la barra laterale. cronologiaAll tiene TUTTI i messaggi
  // conosciuti (indice 0 = il più recente), il DOM (#log-box) mostra sempre e
  // solo i 10 della pagina corrente — stesso pattern usato per i Report in
  // js/14-battaglia.js. Tetto lato client a 300 voci (invariato da 14/09/2026)
  // solo come valvola di sicurezza: il server ne manda già al massimo 200
  // (vedi Server.cs/GameSave.cs), quindi in pratica non scatta mai.
  const CRONOLOGIA_PAGE_SIZE = 10;
  const LOG_MAX_VOCI = 300;
  const logBox = document.getElementById("log-box");
  let cronologiaAll = [];
  let cronologiaPage = 0;

  function aggiornaPagerCronologia(totalPages) {
    const pager = document.getElementById("log-box-pager");
    if (!pager) return;
    pager.hidden = cronologiaAll.length <= CRONOLOGIA_PAGE_SIZE;
    const info = document.getElementById("log-box-page-info");
    if (info) info.textContent = WW.t("paginaDi").replace("{0}", cronologiaPage + 1).replace("{1}", totalPages);
    const btnPrev = document.getElementById("log-box-prev");
    const btnNext = document.getElementById("log-box-next");
    if (btnPrev) btnPrev.disabled = cronologiaPage <= 0;
    if (btnNext) btnNext.disabled = cronologiaPage >= totalPages - 1;
  }

  // Ridisegna SOLO la pagina corrente (10 righe al massimo) a partire da
  // cronologiaAll. Va richiamata ogni volta che cambia pagina o che arriva un
  // nuovo messaggio mentre si è sulla pagina 1 (la più recente).
  function renderCronologiaPage() {
    if (!logBox) return;
    const totalPages = Math.max(1, Math.ceil(cronologiaAll.length / CRONOLOGIA_PAGE_SIZE));
    if (cronologiaPage >= totalPages) cronologiaPage = totalPages - 1;
    if (cronologiaPage < 0) cronologiaPage = 0;

    logBox.innerHTML = "";
    const inizio = cronologiaPage * CRONOLOGIA_PAGE_SIZE;
    const pagina = cronologiaAll.slice(inizio, inizio + CRONOLOGIA_PAGE_SIZE);
    if (pagina.length === 0) {
      const vuoto = document.createElement("p");
      vuoto.className = "log-empty";
      vuoto.textContent = WW.t("nessunEventoRecente");
      logBox.appendChild(vuoto);
    } else {
      pagina.forEach((testo) => {
        const riga = document.createElement("p");
        riga.className = "log-entry";
        riga.appendChild(renderLogSegments(parseLogMessage(testo)));
        logBox.appendChild(riga);
      });
    }
    aggiornaPagerCronologia(totalPages);
  }

  // Aggiunge un nuovo messaggio in cima a cronologiaAll (il più recente resta
  // sempre in indice 0). Ridisegna subito solo se si è sulla pagina 1 — se il
  // giocatore sta sfogliando pagine più vecchie, la vista resta ferma dov'è e
  // si aggiorna solo l'indicatore "Pagina X di Y" (il totale pagine può
  // crescere) per non interrompere la lettura.
  function appendLog(testo) {
    if (!testo) return;
    cronologiaAll.unshift(testo);
    if (cronologiaAll.length > LOG_MAX_VOCI) cronologiaAll.length = LOG_MAX_VOCI;
    if (cronologiaPage === 0) renderCronologiaPage();
    else aggiornaPagerCronologia(Math.max(1, Math.ceil(cronologiaAll.length / CRONOLOGIA_PAGE_SIZE)));
  }
  WW.NET.on("Log_Server", (args) => appendLog(args.join("|")));

  // 16/09/2026, su richiesta dell'utente: popola la Cronologia dalla lista salvata
  // lato server (arriva al login e dopo "Elimina_Cronologia" — vedi applyUpdateData
  // sopra). Sostituisce sempre l'intero contenuto (non accoda) perché il server
  // manda SEMPRE la lista intera: senza questo, un login successivo duplicherebbe
  // le righe già presenti. L'array arriva in ordine cronologico (più vecchio ->
  // più recente dal server): lo invertiamo per avere indice 0 = più recente,
  // coerente con l'ordine usato da appendLog qui sopra.
  function setCronologia(cronologia) {
    cronologiaAll = Array.isArray(cronologia) ? cronologia.slice().reverse() : [];
    cronologiaPage = 0;
    renderCronologiaPage();
  }

  // Pulsante "Svuota cronologia" (vedi index.html, .btn-elimina-tondo): azione
  // server-autoritativa come "Elimina_Report" — svuota subito la vista locale in
  // ottimistico (l'utente vede l'effetto immediato) e manda il comando al server,
  // che risponderà comunque con un nuovo "Cronologia_Lista" (vuoto) a conferma.
  const btnEliminaCronologia = document.getElementById("btn-elimina-cronologia");
  if (btnEliminaCronologia) {
    btnEliminaCronologia.addEventListener("click", () => {
      if (!confirm("Svuotare tutta la cronologia messaggi?")) return;
      setCronologia([]);
      if (WW.NET && WW.NET.send) WW.NET.send("Elimina_Cronologia", WW.AUTH.accessToken);
    });
  }

  // Pulsanti "‹ Indietro"/"Avanti ›" della Cronologia (visibili solo con più di
  // 10 elementi — vedi aggiornaPagerCronologia).
  const btnLogPrev = document.getElementById("log-box-prev");
  if (btnLogPrev) btnLogPrev.addEventListener("click", () => { cronologiaPage--; renderCronologiaPage(); });
  const btnLogNext = document.getElementById("log-box-next");
  if (btnLogNext) btnLogNext.addEventListener("click", () => { cronologiaPage++; renderCronologiaPage(); });

  // Cache generica di tutte le "Descrizione|<chiave>|<testo>" ricevute dal
  // server (costo/effetto di una ricerca, di un edificio, di un
  // addestramento, ...): il server ne manda decine ad ogni login/AutoLogin
  // e altre dopo ogni azione completata (vedi Descrizioni.cs), tutte con lo
  // stesso comando "Descrizione". WW.NET.on supporta UN SOLO handler per
  // comando (sovrascrive, non accoda), quindi il punto di ricezione deve
  // essere unico e condiviso: ogni schermata (Ricerca, Costruzione, in
  // futuro altre) legge da WW.descrizioni e si registra con WW.onDescrizione
  // per sapere quando aggiornare un box già aperto, invece di chiamare
  // WW.NET.on("Descrizione", ...) per conto proprio.
  const descrizioni = Object.create(null);
  const descrizioneListeners = [];
  WW.NET.on("Descrizione", (args) => {
    const chiave = args[0];
    const testo = args.slice(1).join("|");
    if (!chiave) return;
    descrizioni[chiave] = testo;
    descrizioneListeners.forEach((fn) => fn(chiave, testo));
  });
  WW.descrizioni = descrizioni;
  WW.onDescrizione = (fn) => descrizioneListeners.push(fn);

  // Etichette UI localizzate (18/09/2026, su richiesta dell'utente): NON
  // arrivano più con un messaggio dedicato ("UI_Labels", rimosso
  // dall'utente da ServerConnection.cs — vedi nota del 17/09/2026, il
  // server "invia già i dati nel modo corretto"), ma riusano lo stesso
  // canale "Descrizione|<chiave>|<testo>" già gestito sopra: ogni etichetta
  // breve arriva con una chiave che inizia per "Label " (es. "Descrizione|
  // Label Fattoria|Fattoria", vedi Descrizioni.cs) per non scontrarsi con
  // la chiave OMONIMA già usata dalla descrizione narrativa lunga della
  // stessa struttura (es. "Descrizione|Fattoria|<testo lungo>"): stessa
  // chiave per entrambe avrebbe fatto sovrascrivere l'una con l'altra in
  // WW.descrizioni, a seconda di quale arriva per ultima. Le etichette
  // finiscono quindi semplicemente in WW.descrizioni come tutto il resto:
  // niente più WW.LABELS separato, i punti che leggevano un "labelKey" ora
  // leggono WW.descrizioni[labelKey] con lo stesso fallback al nome
  // italiano finché il valore non è ancora arrivato dal server.

  // Barra risorse: chiave-locale (usata dall'HTML in data-value) -> chiave
  // esatta mandata dal server per il giocatore connesso.
  const RESOURCE_KEY_ALIASES = {
    cibo: "cibo",
    legno: "legna", // il server usa "legna", non "legno"
    pietra: "pietra",
    ferro: "ferro",
    oro: "oro",
    popolazione: "popolazione",
    spade: "spade",
    lance: "lance",
    archi: "archi",
    scudi: "scudi",
    armature: "armature",
    frecce: "frecce",
    diamantiBlu: "diamanti_blu",
    diamantiViola: "diamanti_viola",
    xp: "esperienza",
    livello: "livello",
  };

  function renderRisorseBar() {
    Object.keys(RESOURCE_KEY_ALIASES).forEach((chiaveLocale) => {
      const el = document.querySelector(`#resource-bar [data-value="${chiaveLocale}"]`);
      if (el) el.textContent = WW.fmtInt(GAME.num(RESOURCE_KEY_ALIASES[chiaveLocale]));
    });
    // Tributi = "dollari_virtuali" lato server, mostrato con 10 decimali
    // (stessa precisione della produzione dei Feudi di rarità più bassa).
    const elTributi = document.querySelector('#resource-bar [data-value="tributi"]');
    if (elTributi) elTributi.textContent = WW.fmtDecimal(GAME.num("dollari_virtuali"), 10);
  }

  // Feudi: il server manda solo il NUMERO posseduto per rarità (chiavi
  // "comune"/"noncomune"/"raro"/"epico"/"leggendario" — vedi
  // player.Terreno_* in PlayerSnapshot.cs), non un valore di tributi per
  // singolo feudo: il totale dei tributi generati è già in "dollari_virtuali",
  // mostrato nella barra risorse in alto.
  // Icone (14/09/2026, su richiesta dell'utente: "puoi aggiungere le icone
  // ai feudi? nella schermata panoramica?"): stesse immagini già usate nel
  // popup "Feudi Info" qui sotto (FEUDI_INFO/Comune.jpeg ecc., screenshot
  // Feudi_Info.JPG del client desktop), riusate qui per coerenza visiva
  // invece di introdurre asset nuovi.
  const feudi = [
    { nome: "Feudo Comune", chiave: "comune", file: "Comune.jpeg", labelKey: "Label Feudo Comune" },
    { nome: "Feudo Non Comune", chiave: "noncomune", file: "NonComune.jpeg", labelKey: "Label Feudo NonComune" },
    { nome: "Feudo Raro", chiave: "raro", file: "Raro.jpeg", labelKey: "Label Feudo Raro" },
    { nome: "Feudo Epico", chiave: "epico", file: "Epico.jpeg", labelKey: "Label Feudo Epico" },
    { nome: "Feudo Leggendario", chiave: "leggendario", file: "Leggendario.jpeg", labelKey: "Label Feudo Leggendario" },
  ];

  const feudiTitleEl = document.getElementById("feudi-title");

  function renderFeudi() {
    if (feudiTitleEl) feudiTitleEl.textContent = WW.descrizioni["Label Feudi"] || "Feudi";
    const ul = document.getElementById("feudi-list");
    ul.innerHTML = feudi
      .map(
        (f) => `
      <li class="row-item">
        <img src="assets/${f.file}" alt="">
        <span class="row-item__label">${(f.labelKey && WW.descrizioni[f.labelKey]) || f.nome}</span>
        <span class="row-item__value" title="Posseduti">${WW.fmtInt(GAME.num(f.chiave))}</span>
      </li>`
      )
      .join("");
  }

  // "Acquista" (Feudi): comando "Costruzione_Terreni", nessun parametro
  // oltre al token — vedi ComandiInvio.Acquista_TerrenoVirtuale.
  const btnAcquistaFeudo = document.getElementById("btn-acquista-feudo");
  if (btnAcquistaFeudo) {
    btnAcquistaFeudo.addEventListener("click", () => {
      WW.NET.send("Costruzione_Terreni", WW.AUTH.accessToken);
    });
  }

  // Popup "Feudi" (screenshot Feudi_Info.JPG del client desktop, GUI/
  // Terreni_Virtuali.cs): le 5 rarità con tasso di generazione e probabilità
  // sono COSTANTI FISSE lato client, non arrivano dal server (stessi valori
  // hardcoded nel form desktop — non cambiano mai in partita). L'unica parte
  // dinamica è il testo introduttivo, mandato dal server come tutte le altre
  // Descrizioni ("Descrizione|Feudi Info|<testo>", Descrizioni.cs) e già
  // disponibile in WW.descrizioni senza bisogno di richiederla.
  const FEUDI_INFO = [
    { nome: "Comune", file: "Comune.jpeg", tasso: "$ 0.00000000111 s", probabilita: "50%", classe: "comune", labelKey: "Label Feudo Comune" },
    { nome: "Non Comune", file: "NonComune.jpeg", tasso: "$ 0.00000000222 s", probabilita: "20%", classe: "non-comune", labelKey: "Label Feudo NonComune" },
    { nome: "Raro", file: "Raro.jpeg", tasso: "$ 0.00000000333 s", probabilita: "15%", classe: "raro", labelKey: "Label Feudo Raro" },
    { nome: "Epico", file: "Epico.jpeg", tasso: "$ 0.00000000444 s", probabilita: "10%", classe: "epico", labelKey: "Label Feudo Epico" },
    { nome: "Leggendario", file: "Leggendario.jpeg", tasso: "$ 0.00000000555 s", probabilita: "5%", classe: "leggendario", labelKey: "Label Feudo Leggendario" },
  ];

  const feudiInfoOverlay = document.getElementById("feudi-info-overlay");
  const feudiInfoList = document.getElementById("feudi-info-list");
  // Funzione (non più un blocco statico eseguito una volta sola): i nomi
  // delle rarità arrivano da WW.descrizioni (vedi labelKey sopra) e possono
  // non essere ancora arrivati al primo render — va richiamata anche quando
  // le relative "Label Feudo ..." arrivano (vedi listener più sotto), non
  // solo all'avvio, altrimenti resterebbe bloccata sul fallback italiano.
  function renderFeudiInfoList() {
    if (!feudiInfoList) return;
    feudiInfoList.innerHTML = FEUDI_INFO.map(
      (r) => `
      <li class="feudi-info-item feudi-info-item--${r.classe}">
        <img src="assets/${r.file}" alt="">
        <div class="feudi-info-item__mid">
          <span class="feudi-info-item__tasso">${r.tasso}</span>
          <span class="feudi-info-item__nome">${(r.labelKey && WW.descrizioni[r.labelKey]) || r.nome}</span>
        </div>
        <span class="feudi-info-item__prob">${r.probabilita}</span>
      </li>`
    ).join("");
  }
  renderFeudiInfoList();

  function popolaFeudiInfoIntro() {
    const el = document.getElementById("feudi-info-intro");
    if (!el) return;
    const testo = WW.descrizioni["Feudi Info"];
    el.innerHTML = "";
    if (testo) {
      el.appendChild(renderLogSegments(parseLogMessage(testo)));
    } else {
      const span = document.createElement("span");
      span.className = "research-desc__vuoto";
      span.textContent = WW.t("descrizioneNonRicevuta");
      el.appendChild(span);
    }
  }

  const btnFeudiInfo = document.getElementById("btn-feudi-info");
  if (btnFeudiInfo && feudiInfoOverlay) {
    btnFeudiInfo.addEventListener("click", () => {
      popolaFeudiInfoIntro();
      feudiInfoOverlay.hidden = false;
    });
  }
  const btnChiudiFeudiInfo = document.getElementById("btn-chiudi-feudi-info");
  if (btnChiudiFeudiInfo && feudiInfoOverlay) {
    btnChiudiFeudiInfo.addEventListener("click", () => { feudiInfoOverlay.hidden = true; });
  }
  if (feudiInfoOverlay) {
    // Click sullo sfondo scuro (non sul box) chiude il popup, come un normale modale.
    feudiInfoOverlay.addEventListener("click", (e) => {
      if (e.target === feudiInfoOverlay) feudiInfoOverlay.hidden = true;
    });
  }
  document.addEventListener("keydown", (e) => {
    if (e.key === "Escape" && feudiInfoOverlay && !feudiInfoOverlay.hidden) feudiInfoOverlay.hidden = true;
  });
  // Se il testo arriva/aggiorna mentre il popup è già aperto, si aggiorna subito.
  WW.onDescrizione((chiave) => {
    if (chiave === "Feudi Info" && feudiInfoOverlay && !feudiInfoOverlay.hidden) popolaFeudiInfoIntro();
    if (chiave.startsWith("Label Feudo ")) renderFeudiInfoList();
  });

  // Popup "Info Risorsa" (14/09/2026, su richiesta dell'utente): stesso
  // meccanismo del popup Feudi qui sopra, ma condiviso da 9 risorse diverse
  // (Cibo/Legno/Pietra/Ferro/Oro/Popolazione/Diamanti Blu/Diamanti Viola/
  // Tributi) invece di uno per ciascuna. La descrizione (narrativa, uguale
  // per tutte) arriva dal server come "Descrizione|<chiave>|<testo>" (stessa
  // chiave usata da Update_Desc in ClientMessageHandlers.cs, es. "Cibo",
  // "Diamanti Viola", "Dollari Virtuali" per i Tributi) ed è già in
  // WW.descrizioni. Le RIGHE di statistica (Produzione/Edifici/Esercito/
  // Limite) non arrivano invece come testo: sono calcolate qui dai valori
  // già presenti in GAME.raw (stesse formule del client desktop, vedi
  // Main.cs — "Produzione netta" = produzione grezza meno consumo edifici
  // meno consumo esercito) e poi RISCRITTE nella stessa sintassi
  // "Label: [icon:x][colore]valore" che il server usa per le sue descrizioni
  // ricche, così da riusare renderDescrizioneRicca() (04-game-main.js) senza
  // bisogno di markup/CSS nuovi: intro narrativa come paragrafo, righe
  // "Label: valore" raggruppate automaticamente in chip.
  // Diamanti Blu/Viola e Tributi non hanno statistiche (solo la
  // descrizione): "campi" resta null per loro.
  const RISORSA_INFO_CONFIG = {
    cibo: {
      titolo: "Cibo", labelKey: "Label Cibo", chiaveDesc: "Cibo", icona: "cibo",
      campi: () => {
        const grezza = GAME.num("cibo_s");
        const edifici = GAME.num("consumo_cibo_strutture");
        const esercito = GAME.num("consumo_cibo_s");
        const netta = grezza - edifici - esercito;
        return [
          `Produzione: [icon:cibo][arancione]${WW.fmtDecimal(netta, 2)}[black]/[verde]${WW.fmtDecimal(grezza, 2)}[/verde][black]s`,
          `Edifici: [icon:cibo][rosso]${WW.fmtDecimal(edifici, 2)}[/rosso][black]s`,
          `Esercito: [icon:cibo][rosso]${WW.fmtDecimal(esercito, 2)}[/rosso][black]s`,
          `Limite: [icon:cibo][ferroScuro]${WW.fmtInt(GAME.num("cibo_limite"))}`,
        ];
      },
    },
    legno: {
      titolo: "Legno", labelKey: "Label Legno", chiaveDesc: "Legno", icona: "legno",
      campi: () => {
        const grezza = GAME.num("legna_s");
        const edifici = GAME.num("consumo_legno_strutture");
        return [
          `Produzione: [icon:legno][arancione]${WW.fmtDecimal(grezza - edifici, 2)}[black]/[verde]${WW.fmtDecimal(grezza, 2)}[/verde][black]s`,
          `Edifici: [icon:legno][rosso]${WW.fmtDecimal(edifici, 2)}[/rosso][black]s`,
          `Limite: [icon:legno][ferroScuro]${WW.fmtInt(GAME.num("legna_limite"))}`,
        ];
      },
    },
    pietra: {
      titolo: "Pietra", labelKey: "Label Pietra", chiaveDesc: "Pietra", icona: "pietra",
      campi: () => {
        const grezza = GAME.num("pietra_s");
        const edifici = GAME.num("consumo_pietra_strutture");
        return [
          `Produzione: [icon:pietra][arancione]${WW.fmtDecimal(grezza - edifici, 2)}[black]/[verde]${WW.fmtDecimal(grezza, 2)}[/verde][black]s`,
          `Edifici: [icon:pietra][rosso]${WW.fmtDecimal(edifici, 2)}[/rosso][black]s`,
          `Limite: [icon:pietra][ferroScuro]${WW.fmtInt(GAME.num("pietra_limite"))}`,
        ];
      },
    },
    ferro: {
      titolo: "Ferro", labelKey: "Label Ferro", chiaveDesc: "Ferro", icona: "ferro",
      campi: () => {
        const grezza = GAME.num("ferro_s");
        const edifici = GAME.num("consumo_ferro_strutture");
        return [
          `Produzione: [icon:ferro][arancione]${WW.fmtDecimal(grezza - edifici, 2)}[black]/[verde]${WW.fmtDecimal(grezza, 2)}[/verde][black]s`,
          `Edifici: [icon:ferro][rosso]${WW.fmtDecimal(edifici, 2)}[/rosso][black]s`,
          `Limite: [icon:ferro][ferroScuro]${WW.fmtInt(GAME.num("ferro_limite"))}`,
        ];
      },
    },
    oro: {
      titolo: "Oro", labelKey: "Label Oro", chiaveDesc: "Oro", icona: "oro",
      campi: () => {
        const grezza = GAME.num("oro_s");
        const edifici = GAME.num("consumo_oro_strutture");
        const esercito = GAME.num("consumo_oro_s");
        const netta = grezza - edifici - esercito;
        return [
          `Produzione: [icon:oro][arancione]${WW.fmtDecimal(netta, 2)}[black]/[verde]${WW.fmtDecimal(grezza, 2)}[/verde][black]s`,
          `Edifici: [icon:oro][rosso]${WW.fmtDecimal(edifici, 2)}[/rosso][black]s`,
          `Esercito: [icon:oro][rosso]${WW.fmtDecimal(esercito, 2)}[/rosso][black]s`,
          `Limite: [icon:oro][ferroScuro]${WW.fmtInt(GAME.num("oro_limite"))}`,
        ];
      },
    },
    popolazione: {
      titolo: "Popolazione", labelKey: "Label Popolazione", chiaveDesc: "Popolazione", icona: "popolazione",
      // Niente Edifici/Esercito: la Popolazione non si consuma (stesso
      // comportamento del client desktop, vedi Main.cs).
      campi: () => [
        `Produzione: [icon:popolazione][verde]${WW.fmtDecimal(GAME.num("popolazione_s"), 4)}[/verde][black]s`,
        `Limite: [icon:popolazione][ferroScuro]${WW.fmtInt(GAME.num("popolazione_limite"))}`,
      ],
    },
    diamantiBlu: { titolo: "Diamanti Blu", chiaveDesc: "Diamanti Blu", icona: "diamanteBlu", campi: null },
    diamantiViola: { titolo: "Diamanti Viola", chiaveDesc: "Diamanti Viola", icona: "diamanteViola", campi: null },
    tributi: { titolo: "Tributi", chiaveDesc: "Dollari Virtuali", icona: "dollariVirtuali", campi: null },

    // Risorse militari (14/09/2026, su richiesta dell'utente: "andrebbe fatto
    // anche per le risorse militari"): stesso meccanismo, ma qui il client
    // desktop (Main.cs, ramo "Militare") mostra solo Produzione + Limite,
    // senza le righe Edifici/Esercito (le armi non hanno mantenimento).
    spade: {
      titolo: "Spade", labelKey: "Label Spade", chiaveDesc: "Spade", icona: "spade",
      campi: () => [
        `Produzione: [icon:spade][verde]${WW.fmtDecimal(GAME.num("spade_s"), 3)}[/verde][black]s`,
        `Limite: [icon:spade][ferroScuro]${WW.fmtInt(GAME.num("spade_limite"))}`,
      ],
    },
    lance: {
      // 17/09/2026, su richiesta esplicita dell'utente: qui usiamo Label_Lancie()
      // del server anche se la parola non è identica ("Lance" qui vs "Lancie"
      // in ITA.cs/ENG.cs) — stesso concetto (unità Lancieri), a differenza delle
      // altre etichette sopra che hanno una corrispondenza esatta parola per parola.
      titolo: "Lance", labelKey: "Label Lancie", chiaveDesc: "Lance", icona: "lance",
      campi: () => [
        `Produzione: [icon:lance][verde]${WW.fmtDecimal(GAME.num("lance_s"), 3)}[/verde][black]s`,
        `Limite: [icon:lance][ferroScuro]${WW.fmtInt(GAME.num("lance_limite"))}`,
      ],
    },
    archi: {
      titolo: "Archi", labelKey: "Label Archi", chiaveDesc: "Archi", icona: "archi",
      campi: () => [
        `Produzione: [icon:archi][verde]${WW.fmtDecimal(GAME.num("archi_s"), 3)}[/verde][black]s`,
        `Limite: [icon:archi][ferroScuro]${WW.fmtInt(GAME.num("archi_limite"))}`,
      ],
    },
    scudi: {
      titolo: "Scudi", labelKey: "Label Scudi", chiaveDesc: "Scudi", icona: "scudi",
      campi: () => [
        `Produzione: [icon:scudi][verde]${WW.fmtDecimal(GAME.num("scudi_s"), 3)}[/verde][black]s`,
        `Limite: [icon:scudi][ferroScuro]${WW.fmtInt(GAME.num("scudi_limite"))}`,
      ],
    },
    armature: {
      titolo: "Armature", labelKey: "Label Armature", chiaveDesc: "Armature", icona: "armature",
      campi: () => [
        `Produzione: [icon:armature][verde]${WW.fmtDecimal(GAME.num("armature_s"), 3)}[/verde][black]s`,
        `Limite: [icon:armature][ferroScuro]${WW.fmtInt(GAME.num("armature_limite"))}`,
      ],
    },
    frecce: {
      titolo: "Frecce", labelKey: "Label Frecce", chiaveDesc: "Frecce", icona: "frecce",
      campi: () => [
        `Produzione: [icon:frecce][verde]${WW.fmtDecimal(GAME.num("frecce_s"), 3)}[/verde][black]s`,
        `Limite: [icon:frecce][ferroScuro]${WW.fmtInt(GAME.num("frecce_limite"))}`,
      ],
    },

    // Esperienza e Livello (17/09/2026, su richiesta dell'utente): stesso
    // meccanismo delle risorse sopra, ma senza "campi" di produzione/limite
    // (solo l'intro narrativa, come diamantiBlu/diamantiViola/tributi) — il
    // server manda già il testo completo, valore incluso, in
    // "Descrizione|Esperienza|..."/"Descrizione|Livello|..." (vedi
    // Descrizioni.cs, chiamate a Desc_Esperienza/Desc_Livello). labelKey
    // "Esperienza" è una nuova etichetta aggiunta a ITA.cs/ENG.cs apposta per
    // questo popup (non esisteva prima, a differenza di "Livello" che era già
    // tra le Label_* inutilizzate).
    xp: { titolo: "Esperienza", labelKey: "Label Esperienza", chiaveDesc: "Esperienza", icona: "xp", campi: null },
    livello: { titolo: "Livello", labelKey: "Label Livello", chiaveDesc: "Livello", icona: "livello", campi: null },
  };

  const risorsaInfoOverlay = document.getElementById("risorsa-info-overlay");
  const risorsaInfoTitolo = document.getElementById("risorsa-info-title");
  const risorsaInfoContent = document.getElementById("risorsa-info-content");
  let risorsaInfoApertaChiave = null; // per aggiornare il popup se resta aperto mentre i valori cambiano

  function popolaRisorsaInfo(chiaveRes) {
    const config = RISORSA_INFO_CONFIG[chiaveRes];
    if (!config || !risorsaInfoContent || !risorsaInfoTitolo) return;
    risorsaInfoApertaChiave = chiaveRes;
    risorsaInfoTitolo.textContent = (config.labelKey && WW.descrizioni[config.labelKey]) || config.titolo;

    const testoDesc = WW.descrizioni[config.chiaveDesc];
    const righe = [];
    righe.push(testoDesc || WW.t("descrizioneNonRicevuta"));
    if (config.campi) {
      righe.push(""); // riga vuota: separa l'intro narrativa dalle statistiche (vedi renderDescrizioneRicca)
      righe.push(...config.campi());
    }

    risorsaInfoContent.innerHTML = "";
    risorsaInfoContent.appendChild(WW.renderDescrizioneRicca(righe.join("\n")));
  }

  document.querySelectorAll("#resource-bar .res--clickable[data-res]").forEach((el) => {
    el.addEventListener("click", () => {
      if (!RISORSA_INFO_CONFIG[el.dataset.res] || !risorsaInfoOverlay) return;
      popolaRisorsaInfo(el.dataset.res);
      risorsaInfoOverlay.hidden = false;
    });
  });
  const btnChiudiRisorsaInfo = document.getElementById("btn-chiudi-risorsa-info");
  if (btnChiudiRisorsaInfo && risorsaInfoOverlay) {
    btnChiudiRisorsaInfo.addEventListener("click", () => {
      risorsaInfoOverlay.hidden = true;
      risorsaInfoApertaChiave = null;
    });
  }
  if (risorsaInfoOverlay) {
    risorsaInfoOverlay.addEventListener("click", (e) => {
      if (e.target === risorsaInfoOverlay) { risorsaInfoOverlay.hidden = true; risorsaInfoApertaChiave = null; }
    });
  }
  document.addEventListener("keydown", (e) => {
    if (e.key === "Escape" && risorsaInfoOverlay && !risorsaInfoOverlay.hidden) {
      risorsaInfoOverlay.hidden = true;
      risorsaInfoApertaChiave = null;
    }
  });
  // Se la descrizione arriva/cambia mentre il popup è già aperto su quella
  // risorsa, si aggiorna subito invece di restare col messaggio "non ancora
  // ricevuta" finché non lo si riapre.
  WW.onDescrizione((chiave) => {
    if (!risorsaInfoApertaChiave || risorsaInfoOverlay.hidden) return;
    const config = RISORSA_INFO_CONFIG[risorsaInfoApertaChiave];
    if (config && (config.chiaveDesc === chiave || config.labelKey === chiave)) popolaRisorsaInfo(risorsaInfoApertaChiave);
  });

  // Strutture Civili / Militari / Caserme: chiave "qta" (numero costruito) e
  // "coda" (in coda di costruzione), entrambe mandate dal server ad ogni
  // tick — vedi player.Fattoria/Segheria/... e buildingsQueue in
  // PlayerSnapshot.cs.
  // "tipoServer" è la stringa che il comando "Costruzione" si aspetta in
  // quella posizione (vedi ServerConnection.cs, case "Costruzione": 16
  // campi in quest'ordine esatto) — usata sia qui che nella schermata
  // Costruzione/Addestramento (06-costruzione.js) per costruire il
  // messaggio da inviare, ecco perché è esportata su WW.
  // "chiaveDesc" è invece la chiave ESATTA usata dal server per la
  // corrispondente "Descrizione|<chiave>|<testo>" (vedi Descrizioni.cs) —
  // NON sempre coincide con tipoServer (es. "Cava di Pietra" con spazi
  // contro "CavaPietra"), quindi la teniamo esplicita invece di derivarla.
  const struttureCivili = [
    { nome: "Fattoria", icona: "Fattoria_V2.png", qta: "fattorie", coda: "fattoria_coda", tipoServer: "Fattoria", chiaveDesc: "Fattoria", labelKey: "Label Fattoria" },
    { nome: "Segheria", icona: "Segheria_V2.png", qta: "segherie", coda: "segheria_coda", tipoServer: "Segheria", chiaveDesc: "Segheria", labelKey: "Label Segheria" },
    { nome: "Cava di Pietra", icona: "CavaDiPietra_V2.png", qta: "cave_pietra", coda: "cavapietra_coda", tipoServer: "CavaPietra", chiaveDesc: "Cava di Pietra", labelKey: "Label Cava di Pietra" },
    { nome: "Miniera di Ferro", icona: "MinieraFerro_V2.png", qta: "miniere_ferro", coda: "minieraferro_coda", tipoServer: "MinieraFerro", chiaveDesc: "Miniera di Ferro", labelKey: "Label Miniera di Ferro" },
    { nome: "Miniera d'Oro", icona: "MinieraOro_V2.png", qta: "miniere_oro", coda: "minieraoro_coda", tipoServer: "MinieraOro", chiaveDesc: "Miniera d'Oro", labelKey: "Label Miniera d'Oro" },
    { nome: "Case", icona: "Abitazioni_V2.png", qta: "case", coda: "casa_coda", tipoServer: "Abitazioni", chiaveDesc: "Abitazioni", labelKey: "Label Abitazioni" },
  ];

  const struttureMilitari = [
    { nome: "Workshop Spade", icona: "Workshop_Spade_V2.png", qta: "workshop_spade", coda: "workshop_spade_coda", tipoServer: "ProduzioneSpade", chiaveDesc: "Produzione Spade", labelKey: "Label Workshop Spade" },
    { nome: "Workshop Lance", icona: "Workshop_Lance_V2.png", qta: "workshop_lance", coda: "workshop_lance_coda", tipoServer: "ProduzioneLance", chiaveDesc: "Produzione Lance", labelKey: "Label Workshop Lancie" },
    { nome: "Workshop Archi", icona: "Workshop_Archi_V2.png", qta: "workshop_archi", coda: "workshop_archi_coda", tipoServer: "ProduzioneArchi", chiaveDesc: "Produzione Archi", labelKey: "Label Workshop Archi" },
    { nome: "Workshop Scudi", icona: "Workshop_Scudi_V2.png", qta: "workshop_scudi", coda: "workshop_scudi_coda", tipoServer: "ProduzioneScudi", chiaveDesc: "Produzione Scudi", labelKey: "Label Workshop Scudi" },
    { nome: "Workshop Armature", icona: "Workshop_Armature_V2.png", qta: "workshop_armature", coda: "workshop_armature_coda", tipoServer: "ProduzioneArmature", chiaveDesc: "Produzione Armature", labelKey: "Label Workshop Armature" },
    { nome: "Workshop Frecce", icona: "Workshop_Frecce_V2.png", qta: "workshop_frecce", coda: "workshop_frecce_coda", tipoServer: "ProduzioneFrecce", chiaveDesc: "Produzione Frecce", labelKey: "Label Workshop Frecce" },
  ];

  const caserme = [
    { nome: "Caserma Guerrieri", icona: "Caserma_Guerieri_V2.png", qta: "caserma_guerrieri", coda: "caserma_guerrieri_coda", tipoServer: "CasermaGuerrieri", chiaveDesc: "Caserma Guerrieri", labelKey: "Label Caserma Guerrieri" },
    { nome: "Caserma Lancieri", icona: "Caserma_Lanceri_V2.png", qta: "caserma_lanceri", coda: "caserma_lanceri_coda", tipoServer: "CasermaLanceri", chiaveDesc: "Caserma Lanceri", labelKey: "Label Caserma Lanceri" },
    { nome: "Caserma Arcieri", icona: "Caserma_Arcieri_V2.png", qta: "caserma_arceri", coda: "caserma_arceri_coda", tipoServer: "CasermaArceri", chiaveDesc: "Caserma Arceri", labelKey: "Label Caserma Arceri" },
    { nome: "Caserma Catapulte", icona: "Caserma_Catapulte_V2.png", qta: "caserma_catapulte", coda: "caserma_catapulte_coda", tipoServer: "CasermaCatapulte", chiaveDesc: "Caserma Catapulte", labelKey: "Label Caserma Catapulte" },
  ];

  // Ordine ESATTO dei 16 campi richiesti dal comando "Costruzione" lato
  // server (msgArgs[3..18] in ServerConnection.cs). Deve restare in
  // quest'ordine: civili, poi militari, poi caserme.
  const COSTRUZIONE_ORDINE = [...struttureCivili, ...struttureMilitari, ...caserme];

  function renderStruttureList(listId, dati) {
    const ul = document.getElementById(listId);
    ul.innerHTML = dati
      .map(
        (s) => `
      <li class="row-item">
        <img src="assets/${s.icona}" alt="">
        <span class="row-item__label">${(s.labelKey && WW.descrizioni[s.labelKey]) || s.nome}</span>
        <span class="row-item__value" title="${WW.t('builtTooltip')}">${WW.fmtInt(GAME.num(s.qta))}</span>
        <span class="row-item__queue" title="${WW.t('queuedBuildTooltip')}">${WW.fmtInt(GAME.num(s.coda))}</span>
      </li>`
      )
      .join("");
  }

  const struttureToggleBtns = document.querySelectorAll("[data-strutture-view]");
  const struttureLists = document.querySelectorAll("[data-strutture-list]");
  const struttureTitle = document.getElementById("strutture-title");
  const struttureTitoli = { civili: "Strutture Civili", militari: "Strutture Militari", caserme: "Caserme" };
  // 18/09/2026, su richiesta dell'utente: anche questi titoli arrivano ora
  // come "Descrizione|Label ...|<testo>" (vedi Descrizioni.cs) — stesso
  // fallback al nome italiano finché non arrivano.
  const struttureTitoliLabelKey = { civili: "Label Strutture Civili", militari: "Label Strutture Militari", caserme: "Label Caserme" };
  let struttureViewAttiva = "civili";

  function aggiornaStruttureTitle() {
    struttureTitle.textContent = WW.descrizioni[struttureTitoliLabelKey[struttureViewAttiva]] || struttureTitoli[struttureViewAttiva];
  }

  struttureToggleBtns.forEach((btn) => {
    btn.addEventListener("click", () => {
      struttureToggleBtns.forEach((b) => b.classList.remove("is-active"));
      btn.classList.add("is-active");
      struttureViewAttiva = btn.dataset.struttureView;
      struttureLists.forEach((ul) => (ul.hidden = ul.dataset.struttureList !== struttureViewAttiva));
      aggiornaStruttureTitle();
    });
  });

  WW.onDescrizione((chiave) => {
    if (chiave === struttureTitoliLabelKey[struttureViewAttiva]) aggiornaStruttureTitle();
  });

  // Esercito: il server manda una quantità e una coda PER TIER (1-5, chiavi
  // "guerrieri_1".."guerrieri_5", ecc. — vedi player.Guerrieri[]/Lanceri[]/
  // Arceri[]/Catapulte[] in PlayerSnapshot.cs), mentre il "massimo
  // addestrabile" (*_max) è per classe di unità, non per tier. La UI mostra
  // un tier alla volta: cambiare tab (I-V) rilegge le stesse chiavi con un
  // numero diverso, senza bisogno di dati separati per tier. Anche questo
  // array è condiviso con 06-costruzione.js (Addestramento).
  // "chiaveDescPrefix" + " " + tier (1-5) è la chiave ESATTA delle
  // Descrizioni di addestramento mandate dal server (vedi Descrizioni.cs:
  // "Guerrieri 1".."Guerrieri 5", ecc. — plurale, non il "prefisso" usato
  // altrove per le chiavi di quantità/coda, anche se qui coincidono).
  let tierSelezionato = 1;
  const unita = [
    { nome: "Guerriero", icona: "Guerriero_V2.png", prefisso: "guerrieri", max: "guerrieri_max", chiaveDescPrefix: "Guerrieri", labelKey: "Label Guerrieri" },
    { nome: "Lanciere", icona: "Lanciere_V2.png", prefisso: "lanceri", max: "lanceri_max", chiaveDescPrefix: "Lanceri", labelKey: "Label Lanceri" },
    { nome: "Arciere", icona: "Arciere_V2.png", prefisso: "arceri", max: "arceri_max", chiaveDescPrefix: "Arceri", labelKey: "Label Arceri" },
    { nome: "Catapulta", icona: "Catapulta_V2.png", prefisso: "catapulte", max: "catapulte_max", chiaveDescPrefix: "Catapulte", labelKey: "Label Catapulte" },
  ];

  function renderUnita() {
    const ul = document.getElementById("unit-list");
    ul.innerHTML = unita
      .map((u) => {
        const qta = GAME.num(`${u.prefisso}_${tierSelezionato}`);
        const coda = GAME.num(`${u.prefisso}_${tierSelezionato}_coda`);
        const max = GAME.num(u.max);
        return `
      <li class="row-item">
        <img src="assets/${u.icona}" alt="">
        <span class="row-item__label">${(u.labelKey && WW.descrizioni[u.labelKey]) || u.nome}</span>
        <span class="row-item__value" title="${WW.t('trainedLimitTooltip')}">${WW.fmtInt(qta)} / ${WW.fmtInt(max)}</span>
        <span class="row-item__queue" title="${WW.t('queuedTrainTooltip')}">${WW.fmtInt(coda)}</span>
      </li>`;
      })
      .join("");
  }

  // Tier tabs della schermata Main (Esercito): solo quelli dentro #tier-tabs,
  // per non interferire con #costruzione-tier-tabs (tier indipendente, vedi
  // 06-costruzione.js).
  document.querySelectorAll("#tier-tabs .tier-btn").forEach((btn) => {
    btn.addEventListener("click", () => {
      document.querySelectorAll("#tier-tabs .tier-btn").forEach((b) => b.classList.remove("is-active"));
      btn.classList.add("is-active");
      tierSelezionato = Number(btn.dataset.tier) || 1;
      renderUnita();
    });
  });

  // Esportate perché 09-ricerca.js le riusa per mostrare le descrizioni
  // ("Descrizione|chiave|testo", stessa sintassi BBCode-like di Log_Server —
  // vedi Descrizioni.cs) senza duplicare il parser qui sopra.
  // Renderer "ricco" per i testi di Descrizione (costo/effetto di una
  // ricerca, di un edificio, di un addestramento, ...): a differenza della
  // Cronologia (una riga di log), qui il server manda un blocco multi-riga
  // con una struttura ricorrente — testo di intro, riga vuota, poi righe
  // "Label:" seguite da righe "[icon:x]valore" (vedi Descrizioni.cs). Invece
  // di scaricare il tutto come testo piatto con \n → andata a capo, isoliamo
  // le righe con icona in una griglia di "chip" (icona+valore affiancati) e
  // teniamo il resto (intro, etichette) come paragrafi normali: stessa
  // sintassi BBCode-like di sempre per i colori, quindi riusa
  // parseLogMessage/renderLogSegments riga per riga. Generico apposta per
  // essere riusato ovunque arrivi un "Descrizione|..." (Ricerca, Costruzione,
  // Addestramento, ...), non solo nella schermata Ricerca dove è nato.
  function renderDescrizioneRicca(testo) {
    const frag = document.createDocumentFragment();
    let bufferChip = [];

    function flushChip() {
      if (bufferChip.length === 0) return;
      const grid = document.createElement("div");
      grid.className = "research-desc__grid";
      bufferChip.forEach((riga) => {
        const chip = document.createElement("span");
        chip.className = "research-desc__chip";
        chip.appendChild(renderLogSegments(parseLogMessage(riga)));
        grid.appendChild(chip);
      });
      frag.appendChild(grid);
      bufferChip = [];
    }

    // Una riga va in un chip solo se è un vero "Label: valore" corto (righe
    // di costo/effetto/statistica, con o senza icona), non una frase di
    // testo narrativo che contiene un'icona in mezzo alla frase (es.
    // Desc_Fattoria: "...per la produzione di [icon:cibo]Cibo,
    // fondamentale..." — vedi ITA.cs): senza il controllo di lunghezza
    // quella frase intera finiva come UNICO chip in una griglia a colonne
    // strette, con il testo spezzato su decine di righe altissime. Il testo
    // "pulito" (senza tag [...]) delle righe di costo/statistica è sempre
    // breve, quello narrativo no. Serve anche riconoscere le righe SENZA
    // icona ma comunque "Label: valore" (es. "Livello: 6", "Salute: [verde]
    // 8[black]" nel blocco "Statistiche:" delle unità — vedi Descrizioni.cs):
    // un vero "titolo di sezione" come "Statistiche:"/"Costo Costruzione:"
    // ha i due punti alla fine e nient'altro dopo, quindi non passa il test.
    function eTestoBreve(riga) {
      return riga.replace(/\[[^\]]*\]/g, "").trim().length <= 60;
    }
    function eEtichettaValore(riga) {
      return /:\s*\S/.test(riga.replace(/\[[^\]]*\]/g, ""));
    }
    // 14/09/2026 (bug segnalato dall'utente: descrizione risorse militari
    // "spezzata con spazi random"): esistono anche righe SENZA i due punti
    // ma comunque "Label [icon:x]valore" senza etichetta testuale (es.
    // "Cibo [icon:cibo]500", "Mantenimento cibo [icon:cibo][rosso]-5[black]
    // s" nei costi di Descrizioni.cs) — per queste il solo controllo
    // eTestoBreve+icona (senza richiedere i due punti) le riconosceva come
    // chip, ma la STESSA condizione intercettava anche frasi narrative
    // brevi con un'icona in mezzo (Desc_Spade/Lance/Archi/Scudi/Armature in
    // ITA.cs: "Le Spade[icon:spade] sono necessarie per l'addestramento
    // dei guerrieri.", 59 caratteri puliti, sotto la soglia di 60), che
    // finivano anch'esse nella griglia chip stretta. Le vere righe di
    // costo/valore non terminano MAI con un punto (finiscono con un
    // numero, un'unità o una parentesi), le frasi narrative sì: usare
    // questo per distinguerle senza richiedere i due punti.
    function eValoreSenzaPunto(riga) {
      return !/\.\s*$/.test(riga.replace(/\[[^\]]*\]/g, "").trim());
    }

    testo.split("\n").forEach((riga) => {
      if (riga.trim() === "") {
        flushChip(); // riga vuota: solo un separatore tra i gruppi, niente paragrafo vuoto
        return;
      }
      if (eTestoBreve(riga) && eValoreSenzaPunto(riga) && (riga.includes("[icon:") || eEtichettaValore(riga))) {
        bufferChip.push(riga);
      } else {
        flushChip();
        const p = document.createElement("p");
        p.className = "research-desc__testo";
        p.appendChild(renderLogSegments(parseLogMessage(riga)));
        frag.appendChild(p);
      }
    });
    flushChip();
    return frag;
  }

  WW.parseLogMessage = parseLogMessage;
  WW.renderLogSegments = renderLogSegments;
  WW.renderDescrizioneRicca = renderDescrizioneRicca;

  WW.GAME = GAME;
  WW.struttureCivili = struttureCivili;
  WW.struttureMilitari = struttureMilitari;
  WW.caserme = caserme;
  WW.COSTRUZIONE_ORDINE = COSTRUZIONE_ORDINE;
  WW.unita = unita;
  WW.renderAllFromServer = renderAllFromServer;
})(window.WW);
