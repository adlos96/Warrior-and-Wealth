/* ==========================================================
   Warrior & Wealth — Web Client — 16-raduni.js
   ----------------------------------------------------------
   PANNELLO RADUNI (22/09/2026) — dentro la schermata PVP/PVE
   (index.html, quinto pannello del toggle #battaglia-panel-toggle).
   Backend già completo e testato (Gioco/Raduni.cs, classe
   AttacchiCooperativi — il nome della classe non è stato rinominato
   per non allargare il diff, solo il protocollo sul wire usa
   "Raduno"): crea un raduno, partecipa con le proprie truppe,
   abbandona, e solo il creatore può avviarlo o chiuderlo.

   23/09/2026, su richiesta dell'utente ("invece di avere due
   schermate diverse per selezionare le truppe... non è possibile
   avere una schermata comune?"): questo pannello NON ha più un
   proprio stepper truppe (né per "Crea raduno" né per "Partecipa").
   Le truppe si scelgono UNA SOLA VOLTA nel pannello "Esercito da
   Inviare" (14-battaglia.js, già condiviso da Barbari/PVP) e i
   bottoni "Crea raduno"/"Partecipa" qui sotto usano quella stessa
   selezione — esposta da 14-battaglia.js su WW.esercitoInviare
   ({truppeArgs, totale, azzera}). Stessa idea già usata per
   WW.pvpListaGiocatori: un solo pannello scrive lo stato, gli altri
   lo leggono, invece di duplicare UI e stato in ogni schermata.

   Protocollo (ServerConnection.cs/Raduni.cs):
   - "Raduno|token|Crea|<Barbaro|PVP>|<bersaglio>|<alleanza true/false>|<durataMinuti>|<G1..G5>|<L1..L5>|<A1..A5>|<C1..C5>"
     (esteso il 22/09/2026: <bersaglio> è il livello 1-20 per "Barbaro", lo username del
     giocatore per "PVP". Esteso di nuovo il 23/09/2026: il creatore entra subito nel
     raduno con le truppe attualmente selezionate in "Esercito da Inviare" — 20 valori
     truppe in coda, stesso formato/ordine di "Partecipa" sotto. <durataMinuti> deve
     essere uno dei valori in Raduni.DURATE_MINUTI_VALIDE — 5/10/15/30/60/120)
   - "Raduno|token|Partecipa|<idAttacco>|<G1..G5>|<L1..L5>|<A1..A5>|<C1..C5>" (20 valori truppe,
     anche queste lette da WW.esercitoInviare — click diretto sul bottone "Partecipa" di una
     riga, niente più form/stepper intermedio da riempire)
   - "Raduno|token|Abbandona|<idAttacco>"
   - "Raduno|token|Inizia|<idAttacco>" (solo il creatore)
   - "Raduno|token|Chiudi|<idAttacco>" (23/09/2026, su richiesta dell'utente: il creatore chiude il
     raduno prima dello scadere del tempo — il server restituisce le truppe a TUTTI i partecipanti,
     diverso da "Abbandona" che riguarda solo le proprie)
   - Risposte/errori: "Log_Server|<messaggio>" — già mostrato in
     Cronologia da WW.NET.on("Log_Server", ...) in 04-game-main.js,
     nessuna gestione dedicata serve qui.
   - Stato "live": pacchetto JSON {"Type":"RaduniUpdate","Aperti":[...],
     "MiePartecipazioni":[...]} rimandato ad ogni tick da Update_Data
     (ServerConnection.cs) a TUTTI i giocatori connessi — stesso
     principio del resto della UI: non serve richiedere esplicitamente
     "Raduno|Lista"/"Raduno|MieiAttacchi", il pacchetto arriva da solo e
     la UI si aggiorna a ogni ricezione. Ogni riga porta anche
     "TipoBersaglio" ("Barbaro"/"PVP") e, per i raduni PVP,
     "BersaglioUsername" (LivelloTarget resta 0/non significativo per
     quelli PVP). "Aperti" è già filtrato lato server secondo l'Opzione B
     (un raduno Alleanza=true è visibile solo al suo creatore, finché non
     esiste un vero sistema di alleanze).
   - Lista giocatori selezionabili per un raduno PVP: stessa lista del
     pannello Battaglia PVP singolo (14-battaglia.js), esposta su
     WW.pvpListaGiocatori invece di registrare un secondo handler su
     "Update_PVP_Player" (WW.NET.on ne accetta uno solo per comando).

   Dipende da: WW.GAME/WW.NET/WW.AUTH (01-net.js/02-auth.js/04-game-main.js),
   WW.cssEscape (00-core.js), WW.pvpListaGiocatori e WW.esercitoInviare
   (14-battaglia.js — quest'ultimo è la selezione truppe condivisa, vedi sopra).
   Esporta:
   WW.renderRaduni — chiamata da renderAllFromServer in 04-game-main.js,
   stesso pattern lazy-build-poi-refresh di WW.renderBattaglia
   (14-battaglia.js): la UI statica (listener) si costruisce una sola
   volta al primo giro. */

window.WW = window.WW || {};

(function (WW) {
  "use strict";

  let aperti = []; // ultimo "Aperti" ricevuto da RaduniUpdate
  let mie = []; // ultimo "MiePartecipazioni" ricevuto da RaduniUpdate
  let tipoBersaglioCrea = "Barbaro"; // tab attiva nel form "Crea raduno" ("Barbaro" | "PVP")

  /* ---------------------------------------------------------------
     Crea / Partecipa / Abbandona / Avvia / Chiudi
     --------------------------------------------------------------- */

  function creaRaduno() {
    // Le truppe si scelgono nel pannello "Esercito da Inviare" (WW.esercitoInviare, vedi
    // commento in testa al file) — il bottone è già disabilitato a 0 truppe (vedi
    // aggiornaBottoni), questo controllo è solo una seconda sicurezza.
    if (WW.esercitoInviare.totale() === 0) return;

    const alleanzaInput = document.getElementById("raduno-crea-alleanza");
    const alleanza = !!(alleanzaInput && alleanzaInput.checked);

    // Durata (23/09/2026, su richiesta dell'utente): valori fissi allineati a
    // Raduni.DURATE_MINUTI_VALIDE lato server — la <select> in index.html offre solo quelli, ma il
    // fallback 30 copre comunque il caso limite in cui l'elemento non fosse trovato.
    const durataInput = document.getElementById("raduno-crea-durata");
    const durataMinuti = Number((durataInput && durataInput.value) || 30);

    const truppeArgsCrea = WW.esercitoInviare.truppeArgs();

    if (tipoBersaglioCrea === "PVP") {
      const selectGiocatore = document.getElementById("raduno-crea-giocatore");
      const bersaglio = selectGiocatore && selectGiocatore.value;
      if (!bersaglio) return; // nessun giocatore selezionabile (select vuota/disabilitata)
      WW.NET.send("Raduno", WW.AUTH.accessToken, "Crea", "PVP", bersaglio, alleanza, durataMinuti, ...truppeArgsCrea);
    } else {
      const livelloInput = document.getElementById("raduno-crea-livello");
      const livello = Math.max(1, Math.min(20, Number((livelloInput && livelloInput.value) || 1)));
      WW.NET.send("Raduno", WW.AUTH.accessToken, "Crea", "Barbaro", livello, alleanza, durataMinuti, ...truppeArgsCrea);
    }
    if (alleanzaInput) alleanzaInput.checked = false;
    WW.esercitoInviare.azzera(); // le truppe appena inviate sono già partite col raduno
  }

  // 23/09/2026: click diretto sul bottone "Partecipa" di una riga — invia subito le truppe
  // attualmente selezionate in "Esercito da Inviare", niente più form/stepper intermedio da
  // riempire (era la fonte dei bug "il form si chiude da solo" risolti in questa stessa sessione:
  // eliminando il form, il problema semplicemente non esiste più).
  function partecipaRaduno(id) {
    if (WW.esercitoInviare.totale() === 0) return; // bottone già disabilitato in questo caso, vedi templateApertoRow
    WW.NET.send("Raduno", WW.AUTH.accessToken, "Partecipa", id, ...WW.esercitoInviare.truppeArgs());
    WW.esercitoInviare.azzera();
  }

  // Popola il <select> bersaglio-giocatore da WW.pvpListaGiocatori (stringhe "username, Livello: X,
  // Potenza: Y" — stesso formato di Server.cs/Utenti_PVP): il valore inviato al server è SOLO lo
  // username (prima virgola), il testo mostrato è la stringa completa.
  function aggiornaSelectGiocatore() {
    const select = document.getElementById("raduno-crea-giocatore");
    if (!select) return;
    const lista = WW.pvpListaGiocatori || [];
    const valorePrecedente = select.value;
    select.innerHTML = lista.length > 0
      ? lista.map((u) => `<option value="${u.split(",")[0]}">${u}</option>`).join("")
      : `<option value="">Nessun giocatore disponibile</option>`;
    const usernames = lista.map((u) => u.split(",")[0]);
    if (usernames.includes(valorePrecedente)) select.value = valorePrecedente;
  }

  function selezionaTipoBersaglioCrea(tipo) {
    tipoBersaglioCrea = tipo === "PVP" ? "PVP" : "Barbaro";
    const tabs = document.getElementById("raduno-crea-tipo");
    if (tabs) tabs.querySelectorAll(".tier-btn").forEach((b) => b.classList.toggle("is-active", b.dataset.tipo === tipoBersaglioCrea));
    const wrapLivello = document.getElementById("raduno-crea-livello-wrap");
    const wrapGiocatore = document.getElementById("raduno-crea-giocatore-wrap");
    if (wrapLivello) wrapLivello.hidden = tipoBersaglioCrea === "PVP";
    if (wrapGiocatore) wrapGiocatore.hidden = tipoBersaglioCrea !== "PVP";
    if (tipoBersaglioCrea === "PVP") aggiornaSelectGiocatore();
  }

  function abbandonaRaduno(id) {
    WW.NET.send("Raduno", WW.AUTH.accessToken, "Abbandona", id);
  }

  function avviaRaduno(id) {
    WW.NET.send("Raduno", WW.AUTH.accessToken, "Inizia", id);
  }

  // 23/09/2026, su richiesta dell'utente: il creatore può chiudere il raduno prima che scada il
  // tempo — il server restituisce le truppe a TUTTI i partecipanti (vedi Raduni.ChiudiAttaccoCooperativo).
  // Solo per il creatore: il bottone stesso compare solo nella sua riga in templateMiaRow.
  function chiudiRaduno(id) {
    WW.NET.send("Raduno", WW.AUTH.accessToken, "Chiudi", id);
  }

  /* ---------------------------------------------------------------
     Render liste
     --------------------------------------------------------------- */

  // Classe di "urgenza" sul tempo rimanente (23/09/2026, restyling su richiesta dell'utente): sotto i
  // 3 minuti l'evidenzia in rosso, così si nota subito quali raduni stanno per scadere/partire.
  function classeTempo(minutiRimanenti) {
    return minutiRimanenti <= 3 ? " raduno-item__meta-valore--urgente" : "";
  }

  function templateApertoRow(r) {
    const sonoCreatore = r.Creatore === WW.AUTH.username;
    const giaPartecipo = mie.some((m) => String(m.Id) === String(r.Id));
    // 23/09/2026: "Partecipa" resta disabilitato anche a 0 truppe selezionate in "Esercito da
    // Inviare" — click diretto, niente più form dove accorgersene dopo.
    const nessunaTruppa = WW.esercitoInviare.totale() === 0;
    const disabilitato = giaPartecipo || nessunaTruppa;
    const etichetta = giaPartecipo ? "Già dentro" : "Partecipa";
    const badgeTipo = r.TipoBersaglio === "PVP"
      ? `<span class="raduno-item__badge raduno-item__badge--pvp">PVP</span>`
      : `<span class="raduno-item__badge raduno-item__badge--barbaro">Barbaro</span>`;
    const badgeAlleanza = r.Alleanza ? `<span class="raduno-item__badge raduno-item__badge--alleanza">Alleanza</span>` : "";
    return `
    <li class="raduno-item" data-id="${WW.cssEscape(String(r.Id))}">
      <div class="raduno-item__info">
        <div class="raduno-item__titolo">
          <span class="raduno-item__id">#${r.Id}</span>
          <span class="raduno-item__bersaglio">${descrizioneBersaglio(r)}</span>
          ${badgeTipo}${badgeAlleanza}
        </div>
        <div class="raduno-item__meta">
          <span class="raduno-item__meta-voce" title="Creatore"><span class="raduno-item__meta-icona">👑</span>${r.Creatore}${sonoCreatore ? " (tu)" : ""}</span>
          <span class="raduno-item__meta-voce" title="Partecipanti"><span class="raduno-item__meta-icona">👥</span>${r.Partecipanti}</span>
          <span class="raduno-item__meta-voce" title="Tempo rimanente"><span class="raduno-item__meta-icona">⏳</span><span class="raduno-item__meta-valore${classeTempo(r.MinutiRimanenti)}">${r.MinutiRimanenti} min</span></span>
        </div>
      </div>
      <button type="button" class="btn btn--ghost btn--partecipa-raduno" data-id="${WW.cssEscape(String(r.Id))}"${disabilitato ? " disabled" : ""} title="${nessunaTruppa && !giaPartecipo ? "Seleziona almeno una truppa nel pannello Esercito da Inviare" : ""}">${etichetta}</button>
    </li>`;
  }

  // Etichetta bersaglio comune alle due liste (aperti/mie): "Città Barbaro Lv.X" oppure, per i raduni
  // PVP (22/09/2026), "Giocatore <username>".
  function descrizioneBersaglio(r) {
    return r.TipoBersaglio === "PVP" ? `Giocatore ${r.BersaglioUsername}` : `Città Barbaro Lv.${r.LivelloTarget}`;
  }

  function templateMiaRow(m) {
    const sonoCreatore = m.Creatore === WW.AUTH.username;
    const totale = m.Guerrieri + m.Lanceri + m.Arceri + m.Catapulte;
    // Bottone "Chiudi raduno" (23/09/2026, su richiesta dell'utente: "il creatore dovrebbe poter
    // chiudere il raduno prima dello scadere del tempo") — solo per il creatore, restituisce le
    // truppe a TUTTI i partecipanti (diverso da "Abbandona", che riguarda solo le proprie truppe).
    const azioni = m.AttaccoInCorso
      ? `<span class="raduno-item__stato">Attacco in corso…</span>`
      : `<button type="button" class="btn btn--ghost btn--abbandona-raduno" data-id="${WW.cssEscape(String(m.Id))}">Abbandona</button>` +
        (sonoCreatore ? `<button type="button" class="btn btn--ghost btn--danger btn--chiudi-raduno" data-id="${WW.cssEscape(String(m.Id))}">Chiudi raduno</button>` : "") +
        (sonoCreatore ? `<button type="button" class="btn btn--primary btn--avvia-raduno" data-id="${WW.cssEscape(String(m.Id))}">Avvia</button>` : "");
    return `
    <li class="raduno-item" data-id="${WW.cssEscape(String(m.Id))}">
      <div class="raduno-item__info">
        <div class="raduno-item__titolo">
          <span class="raduno-item__id">#${m.Id}</span>
          <span class="raduno-item__bersaglio">${descrizioneBersaglio(m)}</span>
          ${sonoCreatore ? `<span class="raduno-item__badge raduno-item__badge--creatore">Tuo raduno</span>` : ""}
        </div>
        <div class="raduno-item__meta">
          <span class="raduno-item__meta-voce" title="Creatore"><span class="raduno-item__meta-icona">👑</span>${m.Creatore}${sonoCreatore ? " (tu)" : ""}</span>
          <span class="raduno-item__meta-voce" title="Le tue truppe">⚔️ G:${m.Guerrieri} L:${m.Lanceri} A:${m.Arceri} C:${m.Catapulte} (tot. ${totale})</span>
          <span class="raduno-item__meta-voce" title="Tempo rimanente"><span class="raduno-item__meta-icona">⏳</span><span class="raduno-item__meta-valore${classeTempo(m.MinutiRimanenti)}">${m.MinutiRimanenti} min</span></span>
        </div>
      </div>
      <div class="raduno-item__azioni">${azioni}</div>
    </li>`;
  }

  function renderRaduniListe() {
    const listaAperti = document.getElementById("raduni-aperti-list");
    if (listaAperti) {
      listaAperti.innerHTML = aperti.length
        ? aperti.map(templateApertoRow).join("")
        : `<li class="panel__hint">Nessun raduno aperto al momento. Creane uno tu!</li>`;
    }

    const listaMie = document.getElementById("raduni-mie-list");
    if (listaMie) {
      listaMie.innerHTML = mie.length
        ? mie.map(templateMiaRow).join("")
        : `<li class="panel__hint">Non stai partecipando a nessun raduno.</li>`;
    }
  }

  WW.NET.onJson("RaduniUpdate", (msg) => {
    aperti = Array.isArray(msg.Aperti) ? msg.Aperti : [];
    mie = Array.isArray(msg.MiePartecipazioni) ? msg.MiePartecipazioni : [];
    renderRaduniListe();
  });

  function aggiornaBottoneCreaRaduno() {
    const btn = document.getElementById("btn-crea-raduno");
    if (btn) btn.disabled = WW.esercitoInviare.totale() === 0;
  }

  function collegaEventiStatici() {
    const btnCrea = document.getElementById("btn-crea-raduno");
    if (btnCrea) btnCrea.addEventListener("click", creaRaduno);

    const listaAperti = document.getElementById("raduni-aperti-list");
    if (listaAperti) {
      listaAperti.addEventListener("click", (e) => {
        const btn = e.target.closest(".btn--partecipa-raduno");
        if (!btn || btn.disabled) return;
        partecipaRaduno(btn.dataset.id);
      });
    }

    const listaMie = document.getElementById("raduni-mie-list");
    if (listaMie) {
      listaMie.addEventListener("click", (e) => {
        const btnAbb = e.target.closest(".btn--abbandona-raduno");
        if (btnAbb) { abbandonaRaduno(btnAbb.dataset.id); return; }
        const btnChiudi = e.target.closest(".btn--chiudi-raduno");
        if (btnChiudi) { chiudiRaduno(btnChiudi.dataset.id); return; }
        const btnAvvia = e.target.closest(".btn--avvia-raduno");
        if (btnAvvia) avviaRaduno(btnAvvia.dataset.id);
      });
    }

    const tabsTipo = document.getElementById("raduno-crea-tipo");
    if (tabsTipo) {
      tabsTipo.addEventListener("click", (e) => {
        const btn = e.target.closest(".tier-btn");
        if (!btn) return;
        selezionaTipoBersaglioCrea(btn.dataset.tipo);
      });
    }
  }

  let uiCostruita = false;
  function renderRaduni() {
    if (!uiCostruita) {
      collegaEventiStatici();
      uiCostruita = true;
    }
    // 23/09/2026: il bottone "Crea raduno" e i bottoni "Partecipa" (dentro renderRaduniListe,
    // richiamata sotto) riflettono la selezione corrente di WW.esercitoInviare ad ogni tick —
    // così restano coerenti anche se cambi le truppe stando sul pannello Raduni.
    aggiornaBottoneCreaRaduno();
    renderRaduniListe();
    // La lista giocatori PVP (WW.pvpListaGiocatori) arriva in modo asincrono da 14-battaglia.js: se il tab
    // "Giocatore" è già selezionato quando la lista cambia, la select va tenuta aggiornata ad ogni tick.
    if (tipoBersaglioCrea === "PVP") aggiornaSelectGiocatore();
  }

  WW.renderRaduni = renderRaduni;
})(window.WW);
