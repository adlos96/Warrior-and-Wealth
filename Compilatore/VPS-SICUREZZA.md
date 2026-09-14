# VPS — Aggiornamento e Sicurezza

> Documento di riferimento per la VPS di produzione di Warrior & Wealth.
> Descrive cosa fanno gli script in `Compilatore/`, cosa è già stato messo in
> sicurezza, cosa manca, e i punti deboli noti. Va tenuto aggiornato ad ogni
> modifica agli script o alla configurazione della VPS — è pensato per essere
> letto sia da chi lavora al progetto sia da un'AI che riprende il lavoro in
> una sessione futura, senza dover ricostruire il contesto da zero.
>
> Ultimo aggiornamento: 14/09/2026.

## Obiettivo degli script `.sh`

Tutti gli script vivono in `Compilatore/` e sono pensati per una VPS Ubuntu
24 "fresh" (appena noleggiata). Il flusso normale, dal giorno 1 in poi, è
**un solo comando**: `bash update-server.sh`. Gli altri tre vengono
richiamati da lui in automatico quando serve; puoi comunque lanciarli a mano
singolarmente se vuoi rifare o ispezionare solo un pezzo.

```
update-server.sh   (comando che lanci tu, sempre)
  │
  ├─▶ update-VPS.sh     (solo se mancano git/screen/dotnet — es. VPS nuova)
  ├─▶ setup-web.sh      (sempre, se ENABLE_WEB_GATEWAY="true")
  └─▶ harden-vps.sh     (sempre, se ENABLE_HARDENING="true")
```

- **`update-VPS.sh`** — setup iniziale della macchina. Aggiorna i pacchetti
  di sistema, installa le dipendenze base (git, screen, curl, wget, unzip,
  htop, ufw, nginx), installa il .NET SDK necessario per compilare il
  server, apre le porte del firewall (ssh, 8443 client desktop, 8444
  gateway web, 80 client web). Va bene rilanciarlo quando vuoi: è
  idempotente (non rompe nulla se i pacchetti sono già installati).

- **`update-server.sh`** — lo script "quotidiano". Clona il repository al
  primo avvio (solo le cartelle `Server Strategico` e `Web`, via
  `git sparse-checkout`, per non scaricare tutto il resto del progetto —
  desktop client, wiki, ecc. — che sulla VPS non serve), oppure lo
  aggiorna (`git pull`) alle esecuzioni successive. Compila il server in
  Release. Chiude la sessione `screen` precedente se esiste. Richiama
  `setup-web.sh` e `harden-vps.sh`. Avvia il server in una sessione
  `screen` chiamata `warrior_server`, con la variabile d'ambiente
  `WW_WEB_GATEWAY` impostata secondo `ENABLE_WEB_GATEWAY` (vedi sotto
  "Come funziona `WW_WEB_GATEWAY`").

- **`setup-web.sh`** — configura nginx per servire staticamente la cartella
  `Web` (il client HTML/CSS/JS, nessun build step) sulla porta 80. Scrive
  la configurazione in `/etc/nginx/sites-available/warrior-and-wealth`,
  disabilita il sito di default di nginx (altrimenti va in conflitto sulla
  porta 80), abilita gzip e cache per gli asset statici. Idempotente:
  rilanciarlo sovrascrive solo la sua configurazione, non tocca altro.
  Richiede root.

- **`harden-vps.sh`** — indurimento di base della macchina: fail2ban
  (blocca gli IP che sbagliano troppe volte la password SSH), aggiornamenti
  di sicurezza automatici del sistema operativo, politiche di firewall
  esplicite. **Non tocca mai l'autenticazione SSH** (vedi "Criticità"
  sotto per il perché). Idempotente, richiede root.

### Come funziona `WW_WEB_GATEWAY`

Il gateway WebSocket per il client web (porta 8444, vedi
`WebSocketGateway.cs`) è disattivato di default nel codice
(`Variabili_Server.WebGatewayEnabled = false`) — su desktop/Windows va
acceso a mano dalla console del server col comando `webstart` (e spento con
`webstop`, che disconnette tutti i client web senza toccare i client
desktop). Su una VPS headless questo è scomodo, quindi `Program.cs` legge
all'avvio la variabile d'ambiente `WW_WEB_GATEWAY`: se vale `"true"`, forza
`WebGatewayEnabled = true` prima che il server parta. È `update-server.sh`
a impostarla (solo per il processo che avvia, non a livello di sistema),
tramite la variabile di configurazione `ENABLE_WEB_GATEWAY` in testa allo
script. Su Windows/desktop, dove nessuno imposta questa variabile, il
comportamento resta identico a prima (default `false`, `webstart` a mano).

## Sicurezza implementata

- **Firewall (ufw)**: solo le porte strettamente necessarie sono aperte
  (ssh, 8443, 8444, 80) più una politica di default esplicita
  (`deny incoming` / `allow outgoing`) per qualsiasi altra cosa.
- **fail2ban su SSH**: blocca per 1 ora un IP dopo 5 tentativi di login
  falliti in 10 minuti (`/etc/fail2ban/jail.local`).
- **Aggiornamenti di sicurezza automatici** del sistema operativo
  (`/etc/apt/apt.conf.d/20auto-upgrades`), scritti esplicitamente dallo
  script invece di affidarsi a una risposta di default non garantita.
- **nginx** con configurazione minima e mirata (niente directory listing di
  default, sito di default disabilitato, gzip per ridurre banda).
- **Isolamento dei trasporti**: il gateway WebSocket (client web) gira su
  una porta separata (8444) da WatsonTcp (client desktop, 8443) e i due non
  condividono stato di connessione — un problema nell'uno non impatta
  l'altro (vedi commenti in `WebSocketGateway.cs`).
- **Sparse-checkout**: la VPS clona solo `Server Strategico` e `Web` dal
  repository pubblico/futuro pubblico — non porta con sé il resto (client
  desktop, chiavi di build, ecc.) anche se in futuro il repo dovesse
  diventare pubblico su GitHub.

## Sicurezza da implementare

In ordine di priorità:

1. **Autenticazione SSH a chiave, invece che a password, per l'utente
   root.** È il miglioramento più importante rimasto e volutamente NON è
   stato automatizzato: un errore in questo passaggio può bloccare fuori
   dalla VPS chi la amministra, quindi va fatto a mano, un passo alla
   volta, verificando che il login con chiave funzioni PRIMA di disattivare
   la password (`PasswordAuthentication no` in `/etc/ssh/sshd_config`).
   **Nota**: l'utente (Adly) ha provato in passato a usare chiavi SSH ma le
   ha perse, quindi per ora la password resta l'unico metodo di accesso —
   mitigata da fail2ban. **Prima di riprendere questo punto, andrebbe
   scritto un piccolo programma/utility per generare, gestire e salvare in
   modo affidabile la coppia di chiavi SSH** (es. backup cifrato locale,
   o gestione tramite un password manager), altrimenti il rischio di
   perdere di nuovo la chiave (e restare fuori dalla VPS, con la console
   di emergenza del provider che "crasha spesso") è concreto. Finché
   questo strumento non esiste, meglio non forzare il passaggio a sola
   chiave.
2. **Utente non-root dedicato con sudo**, da usare al posto del login
   diretto come root — riduce la superficie d'attacco (root è il bersaglio
   più ovvio per i bot). Da fare in combinazione col punto 1, non prima:
   due cambi di autenticazione insieme aumentano il rischio di errore.
3. **Backup automatico dei salvataggi** (`GameSave.SavePath`), su una
   destinazione diversa dalla VPS stessa — se la macchina si rompe o viene
   compromessa, oggi i progressi dei giocatori andrebbero persi. Vedi
   sezione "Idee future" sotto per le opzioni discusse.
4. **HTTPS/WSS** per il client web (oggi HTTP/WS in chiaro) — verosimilmente
   tramite un reverse proxy (nginx, già presente, può fare da terminazione
   TLS) con un certificato Let's Encrypt/Certbot. Non urgente finché il
   traffico non contiene dati sensibili in transito (già oggi però le
   password viaggiano in chiaro sul filo — vedi punto sotto).
5. **Problemi noti a livello di codice del server**, già individuati in una
   sessione precedente e volutamente rimandati fino a gioco completo e
   giocabile — **ma da tenere presente che con la VPS online per utenti
   veri, l'urgenza di questi sale**:
   - Password dei giocatori salvate e confrontate in chiaro
     (`Giocatori.cs`, `ValidatePassword`) — da spostare su hash
     (BCrypt/Argon2) con percorso di migrazione per gli account esistenti.
   - Chiave API di Resend.com **hardcoded e "viva"** nel codice
     (`EmailManager.cs`) — da ruotare (revocare sul pannello Resend) e
     spostare su variabile d'ambiente/file di secrets, indipendentemente
     da quando si affrontano gli altri punti.
   - Password del certificato TLS placeholder hardcoded (`Password.cs`).
   - Nessuna protezione anti-spam/anti-cheat lato client, nessuna gestione
     giocatori (ban temporaneo, perma ban, azzeramento account).

## Criticità ed altro

- **Punto singolo di fallimento**: un'unica VPS ospita sia il server di
  gioco che il client web. Se cade la macchina, cade tutto. Non c'è ancora
  ridondanza né un piano di disaster recovery oltre ai backup (da fare,
  vedi sopra).
- **Console del provider inaffidabile**: l'utente ha accesso alla
  console/VNC del pannello del provider come rete di sicurezza in caso SSH
  smetta di funzionare, ma riferisce che "crasha spesso" — questo è il
  motivo principale per cui il passaggio a SSH-a-sola-chiave va fatto con
  estrema cautela (vedi punto 1 sopra) e mai senza aver verificato ogni
  passaggio.
- **Root login diretto**: oggi si accede come `root` con password. Finché
  non si passa a un utente dedicato (punto 2 sopra), qualsiasi comando
  eseguito per errore ha potenzialmente accesso completo alla macchina.
- **fail2ban copre solo SSH**: non c'è ancora nessuna protezione a livello
  applicativo (rate limiting sul login di gioco, sul gateway WebSocket, o
  sulle API HTTP di nginx) — un bot che manda richieste valide (non
  fallimenti di autenticazione SSH) non viene rallentato da nulla di
  quello implementato finora.
- **Nessun monitoraggio/alerting**: se il server di gioco crasha dentro la
  sessione `screen`, o nginx smette di rispondere, o il disco si riempie,
  al momento nessuno viene avvisato automaticamente — ce ne si accorge solo
  controllando a mano o quando i giocatori segnalano problemi.
- **Nessuna rotazione dei log** configurata esplicitamente per l'output del
  server di gioco (quello di nginx/fail2ban/sistema è già gestito da
  `logrotate` di default su Ubuntu) — se il server stampa molto in
  console, l'output della sessione `screen` può crescere indefinitamente.
- **Bug di editing incontrato in questa sessione** (non relativo alla
  sicurezza della VPS, ma da tenere a mente lavorando su questi file): più
  volte, un file modificato e sincronizzato sul PC dell'utente è tornato
  silenziosamente alla versione precedente (probabile race con un
  altro processo, es. autosalvataggio di Visual Studio, che riscrive il
  file subito dopo la sincronizzazione). **Ogni modifica a questi script
  va sempre riverificata rileggendo il file dal PC dopo il commit**, mai
  dato per scontato solo perché lo strumento di scrittura ha risposto
  "successo".

## Idee future (non ancora progettate)

Raccolte qui come promemoria, da riprendere e progettare con calma quando
si arriva a quel punto — non ancora scritte né valutate in dettaglio:

- **Backup su blockchain (Celestia) come opzione a pagamento**: i
  giocatori che vogliono un salvataggio permanente e verificabile del
  proprio villaggio potrebbero spendere Diamanti Viola per "salvare" i
  propri dati su una blockchain (idea iniziale: Celestia, pensata come
  layer di disponibilità dati). Da valutare con calma: costo/complessità
  di integrazione, cosa esattamente andrebbe scritto on-chain (l'intero
  stato del villaggio è probabilmente troppo per una blockchain — più
  realistico un hash/snapshot periodico), e chi paga le fee di rete.
- **Salvataggio "normale" per i giocatori free-to-play**: indipendentemente
  dall'opzione blockchain (a pagamento), serve comunque un backup
  affidabile e gratuito per tutti gli altri giocatori — probabilmente la
  soluzione più semplice (backup periodico su storage esterno alla VPS,
  vedi "Sicurezza da implementare" punto 3) risolve già questo caso, la
  blockchain sarebbe un'opzione aggiuntiva/premium sopra quella base.
