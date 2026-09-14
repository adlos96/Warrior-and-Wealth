# Web — Client web

Client web (HTML/CSS/JS puro, nessun build step) di Warrior & Wealth.
Login, registrazione e auto-login sono ora **collegati al server vero**
tramite il gateway WebSocket (`Server Strategico/ServerData/WebSocketGateway.cs`);
i dati di gioco veri e propri (Feudi, Strutture, Esercito) restano invece a
zero finché non arriva l'elenco completo dei comandi/formati dal lato
server — vedi "Prossimi passi".

## Struttura

- `index.html` — schermata di login/registrazione + schermata di gioco
  (barra risorse in alto, contenuto, barra di navigazione in basso).
  Solo la tab "Main" ha un contenuto vero (Feudi, Strutture Civili,
  Esercito, Cronologia); le altre tab mostrano un placeholder in attesa
  del protocollo.
- `css/style.css` — design tokens (colori/font) e layout responsive.
  Sotto ~760px di larghezza i pannelli della tab Main passano da
  affiancati a "uno alla volta", selezionabili con i pulsanti in alto
  (stesso pattern del client desktop per risorse/unità civili vs
  militari).
- `js/app.js` — diviso in sezioni: NET (connessione WebSocket, invio e
  ricezione dei comandi testuali `Comando|arg1|arg2|...`), AUTH
  (login/registrazione/auto-login/refresh token, con gli stessi formati
  usati dal client desktop in `ComandiInvio.cs`), navigazione,
  GAME (applica i messaggi `Update_Data` allo stato locale) e i dati di
  Feudi/Strutture/Esercito (ancora a zero, vedi sopra).
- `assets/` — icone e sfondi copiati da `CriptoGame_Online/Resources`
  del progetto, per restare coerenti con lo stile del client desktop.

## Come provarlo dal PC

1. Avviare `Server Strategico` e digitare `webstart` nella console
   (oppure impostare `Variabili_Server.WebGatewayEnabled = true` per
   farlo partire da solo). Il gateway prova prima ad ascoltare su tutte
   le interfacce (`ws://+:8444/`); su Windows, se non ha i permessi,
   scrive un messaggio in console e ripiega automaticamente su
   `ws://localhost:8444/` (raggiungibile solo dalla stessa macchina —
   niente telefono, vedi sotto per abilitarlo).
2. Aprire `index.html` in un browser sulla stessa macchina del server.

## Come provarlo dal telefono (stessa rete Wi-Fi del PC)

Il telefono deve raggiungere **due cose** sul PC: la pagina (`index.html`
coi suoi css/js/assets) e il gateway WebSocket (porta 8444). Da telefono
non si può aprire `index.html` come file locale (il browser bloccherebbe
comunque il WebSocket verso il PC), quindi serve un piccolo server HTTP
anche solo per i file statici.

1. **Permessi Windows per la LAN** (una tantum): aprire un prompt dei
   comandi **come amministratore** ed eseguire:
   ```
   netsh http add urlacl url=http://+:8444/ user=Everyone
   ```
   Da qui in poi `webstart` potrà ascoltare su tutte le interfacce anche
   senza eseguire il server come amministratore.
2. **Servire i file statici** con un piccolo server HTTP nella cartella
   `Web` (uno qualsiasi va bene, sceglierne uno disponibile sul PC):
   ```
   cd "Web"
   python -m http.server 8080 --bind 0.0.0.0
   ```
   oppure, se non c'è Python ma c'è Node:
   ```
   npx http-server -a 0.0.0.0 -p 8080
   ```
3. **Trovare l'IP del PC sulla rete locale**: `ipconfig` (cercare
   "Indirizzo IPv4", tipicamente `192.168.x.x`).
4. **Firewall di Windows**: alla prima esecuzione compare quasi sempre
   un popup per consentire la connessione in rete al programma
   (server HTTP e/o `Server Strategico.exe`) — va accettato, altrimenti
   il telefono non riesce a connettersi anche con IP e porte corrette.
5. Sul telefono (stessa Wi-Fi del PC), aprire il browser su
   `http://<IP-DEL-PC>:8080/`. La pagina si connetterà da sola al
   gateway su `ws://<IP-DEL-PC>:8444/` (usa lo stesso host della pagina,
   vedi `resolveWsUrl()` in `app.js` — non serve configurare nulla a
   mano, a meno di voler forzare un indirizzo diverso con
   `localStorage.setItem('ww_ws_url', 'ws://ALTRO-IP:8444/')` dalla
   console del browser).

Login e registrazione funzionano esattamente come dal PC: username,
password, lingua ed email vengono inviati con lo stesso formato del
client desktop, quindi gli account sono condivisi tra i due client.

## Prossimi passi

1. Il server invierà l'elenco completo/aggiornato dei comandi e dei
   formati esatti dei messaggi di gioco (risorse, feudi, strutture,
   esercito): al loro arrivo, ampliare `GAME.applyUpdateData` e
   `RESOURCE_KEY_ALIASES` in `app.js` (e aggiungere gestori per i
   messaggi JSON come `CittaGlobali`, oggi solo loggati in console).
2. Completare le altre schermate (Costruzione, Città, Ricerca, PVP/PVE,
   Quest Mensili, GamePass, Negozio, Mappa) una alla volta, man mano che
   arrivano i comandi corrispondenti.
3. Valutare, solo se necessario in produzione su Linux, l'uso di HTTPS/WSS
   (oggi il gateway è in chiaro HTTP/WS) — verosimilmente tramite un
   reverse proxy (nginx/Caddy) davanti alla porta 8444, così il
   certificato si gestisce in un solo posto insieme al resto del sito.
