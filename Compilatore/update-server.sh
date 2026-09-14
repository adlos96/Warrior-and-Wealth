#!/bin/bash
# ── Rete di sicurezza CRLF ──────────────────────────────────
# Se questo file arriva da Windows con terminatori di riga CRLF, bash non
# riesce nemmeno a leggerlo ("syntax error: unexpected end of file"). La riga
# qui sotto ripara il file e riparte da sola. DEVE restare su una riga sola,
# commento finale compreso: e' l'unica forma che sopravvive al CRLF.
grep -q $'\r' "$0" && sed -i 's/\r$//' "$0" && exec bash "$0" "$@" # crlf-self-heal
# ============================================================================
#  update-server.sh — Deploy di Warrior & Wealth sulla VPS
#  Warrior & Wealth
# ----------------------------------------------------------------------------
#  Questo e' lo script che lanci ad ogni aggiornamento. In ordine:
#    0. si ricopia in /tmp ed esegue da li' (altrimenti il git pull piu' sotto
#       riscriverebbe questo stesso file mentre bash lo sta ancora leggendo)
#    1. prende un lock: due deploy contemporanei non possono pestarsi i piedi
#    2. installa le dipendenze se mancano (update-VPS.sh)
#    3. clona o aggiorna il repository
#    4. compila il server
#    5. configura nginx per il client web (setup-web.sh)
#    6. applica l'indurimento di sicurezza (harden-vps.sh)
#    7. FERMA il server in modo pulito (prova prima a farlo uscire da solo)
#    8. fa un BACKUP dei salvataggi dei giocatori
#    9. riavvia il server e VERIFICA che sia davvero ripartito
#
#  NON tocca MAI /etc/ssh/sshd_config ne' l'autenticazione SSH.
# ============================================================================
set -uo pipefail

# ── Configurazione ──────────────────────────────────────────
REPO_URL="https://github.com/adlos96/Warrior-and-Wealth.git"
GIT_BRANCH="main"
BASE_DIR="${WW_BASE_DIR:-$HOME/Warrior-and-Wealth}"
SERVER_SUBDIR="Server Strategico"
SCREEN_NAME="warrior_server"
TFM="net8.0"                     # Target Framework del .csproj (cartella di build)
ENABLE_WEB_GATEWAY="true"        # variabile WW_WEB_GATEWAY passata al server (porta 8444)
ENABLE_WEB_SETUP="true"          # esegue setup-web.sh (nginx per il client web)
ENABLE_HARDENING="true"          # esegue harden-vps.sh (fail2ban, firewall, aggiornamenti)
KEEP_BACKUPS=20                  # quanti backup dei salvataggi conservare
STOP_TIMEOUT=30                  # secondi di attesa perche' il server si chiuda da solo
START_TIMEOUT=40                 # secondi di attesa perche' il server torni in ascolto
GAME_TCP_PORT=8443
LOG_FILE="/var/log/warrior-server.log"
BACKUP_DIR="${WW_BACKUP_DIR:-$HOME/ww-backups}"
# ────────────────────────────────────────────────────────────

SERVER_DIR="$BASE_DIR/$SERVER_SUBDIR"
WEB_DIR="$BASE_DIR/Web"
# Cartelle scaricate dal repo (sparse-checkout): solo queste vengono estratte
# sulla VPS, il resto del repository non viene nemmeno scaricato.
SPARSE_DIRS=("Server Strategico" "Web" "Compilatore")

fail() { echo ""; echo "[ERRORE] $1" >&2; exit 1; }
log()  { echo "$1"; }

# ── 0. Auto-rilancio da una copia temporanea ────────────────
# Se il repo contiene anche la cartella "Compilatore" (e ora la contiene),
# il "git pull" del punto 3 puo' riscrivere QUESTO file mentre e' in
# esecuzione: bash legge lo script a blocchi e riprenderebbe dal vecchio
# offset in byte, eseguendo pezzi di codice a caso. Girare da una copia
# in /tmp elimina il problema alla radice.
SCRIPT_DIR="$(cd -P "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
if [ "${WW_RELAUNCHED:-0}" != "1" ]; then
    WW_TMP_DIR="$(mktemp -d /tmp/ww-deploy.XXXXXX)" || fail "Impossibile creare la cartella temporanea"
    cp "$SCRIPT_DIR"/*.sh "$WW_TMP_DIR"/ 2>/dev/null || { rm -rf "$WW_TMP_DIR"; fail "Nessuno script .sh trovato in $SCRIPT_DIR"; }
    # Rete di sicurezza CRLF: se un file e' stato salvato da Windows con
    # terminatori CRLF, le continuazioni di riga "\" si rompono. Qui li
    # normalizziamo sulla copia temporanea, senza toccare gli originali.
    sed -i 's/\r$//' "$WW_TMP_DIR"/*.sh
    chmod +x "$WW_TMP_DIR"/*.sh
    export WW_RELAUNCHED=1
    export WW_ORIG_SCRIPT_DIR="$SCRIPT_DIR"
    bash "$WW_TMP_DIR/update-server.sh" "$@"
    rc=$?
    rm -rf "$WW_TMP_DIR"
    exit $rc
fi
ORIG_SCRIPT_DIR="${WW_ORIG_SCRIPT_DIR:-$SCRIPT_DIR}"

echo "============================================"
echo " Warrior & Wealth - Deploy Server"
echo "============================================"
echo ""

# ── 0b. Deve girare come root ───────────────────────────────
# Serve per apt, nginx, fail2ban e il firewall. In piu' garantisce che il
# server riparta sempre con lo stesso $HOME, quindi sempre con la stessa
# cartella di salvataggi: lanciarlo a volte da root e a volte da un altro
# utente farebbe "sparire" i dati dei giocatori (in realta' finirebbero
# in un'altra home).
if [ "$EUID" -ne 0 ]; then
    fail "Esegui questo script come root (o con sudo). Lanciarlo da utenti diversi cambierebbe la cartella dei salvataggi."
fi

# ── 1. Lock: un solo deploy alla volta ──────────────────────
LOCK_FILE="/var/lock/warrior-deploy.lock"
exec 9>"$LOCK_FILE" || fail "Impossibile aprire il file di lock $LOCK_FILE"
if ! flock -n 9; then
    fail "Un altro deploy e' gia' in corso (lock: $LOCK_FILE). Attendi che finisca."
fi

# ── 2. Auto-setup se mancano i requisiti ────────────────────
SETUP_SCRIPT="$SCRIPT_DIR/update-VPS.sh"
if ! command -v git &>/dev/null || ! command -v screen &>/dev/null || ! command -v dotnet &>/dev/null \
   || ! command -v rsync &>/dev/null || ! command -v ss &>/dev/null; then
    log "[SETUP] Dipendenze mancanti. Avvio setup iniziale..."
    [ -f "$SETUP_SCRIPT" ] || fail "update-VPS.sh non trovato accanto a questo script."
    bash "$SETUP_SCRIPT" || fail "Setup iniziale fallito"
fi

# Percorso di dotnet ricavato a runtime, mai scritto a mano: a seconda di come
# e' stato installato puo' essere /usr/bin/dotnet oppure /usr/local/bin/dotnet.
DOTNET_BIN="$(command -v dotnet)"
[ -n "$DOTNET_BIN" ] || fail "dotnet non trovato nel PATH nemmeno dopo il setup"
log "[DOTNET] Uso: $DOTNET_BIN"

# ── 3. Clone o aggiornamento repository ─────────────────────
# Il controllo e' su .git E sulla cartella del server: se un clone precedente
# si e' interrotto a meta' resta un .git valido con il contenuto non ancora
# estratto, e senza questo doppio controllo ogni deploy successivo fallirebbe
# per sempre nello stesso punto.
if [ ! -d "$BASE_DIR/.git" ] || [ ! -d "$SERVER_DIR" ]; then
    if [ -d "$BASE_DIR/.git" ]; then
        log "[GIT] Repository presente ma incompleto (clone interrotto?): completo l'estrazione."
        cd "$BASE_DIR" || fail "Directory non accessibile: $BASE_DIR"
    else
        log "[GIT] Repository non trovato. Clone in corso..."
        # Si clona in una cartella temporanea e la si rinomina solo a lavoro
        # finito: un'interruzione non lascia mai una cartella a meta'.
        TMP_CLONE="${BASE_DIR}.tmp.$$"
        rm -rf "$TMP_CLONE"
        git clone --no-checkout "$REPO_URL" "$TMP_CLONE" || { rm -rf "$TMP_CLONE"; fail "git clone fallito"; }
        mv "$TMP_CLONE" "$BASE_DIR" || { rm -rf "$TMP_CLONE"; fail "Impossibile spostare il clone in $BASE_DIR"; }
        cd "$BASE_DIR" || fail "Directory non accessibile: $BASE_DIR"
        git sparse-checkout init --cone || fail "git sparse-checkout init fallito"
    fi
    git sparse-checkout set "${SPARSE_DIRS[@]}" || fail "git sparse-checkout set fallito"
    git checkout "$GIT_BRANCH" || fail "git checkout $GIT_BRANCH fallito"
    log "[GIT] Clone completato."
else
    log "[GIT] Aggiornamento repository..."
    cd "$BASE_DIR" || fail "Directory non accessibile: $BASE_DIR"
    git sparse-checkout set "${SPARSE_DIRS[@]}" || fail "git sparse-checkout set fallito"
    git fetch origin "$GIT_BRANCH" || fail "git fetch fallito"
    # reset --hard invece di pull: questa e' una macchina di deploy, non una di
    # sviluppo. Un merge fallito o un file modificato per sbaglio sulla VPS
    # bloccherebbe ogni aggiornamento futuro; cosi' il contenuto e' sempre
    # identico a quello su GitHub. (I salvataggi dei giocatori NON stanno qui
    # dentro, stanno sotto la home: non vengono toccati.)
    git reset --hard "origin/$GIT_BRANCH" || fail "git reset fallito"
    log "[GIT] Aggiornamento completato."
fi

# Se gli script di deploy sono stati lanciati da una cartella FUORI dal repo,
# si aggiorna quella copia con la versione appena scaricata, cosi' il prossimo
# deploy usa gli script nuovi senza doverli ricaricare a mano.
REPO_SCRIPT_DIR="$BASE_DIR/Compilatore"
if [ -d "$REPO_SCRIPT_DIR" ] && [ "$ORIG_SCRIPT_DIR" != "$REPO_SCRIPT_DIR" ]; then
    if ! diff -q "$REPO_SCRIPT_DIR/update-server.sh" "$ORIG_SCRIPT_DIR/update-server.sh" >/dev/null 2>&1; then
        cp "$REPO_SCRIPT_DIR"/*.sh "$ORIG_SCRIPT_DIR"/ 2>/dev/null \
            && log "[GIT] Script di deploy aggiornati in $ORIG_SCRIPT_DIR (attivi dal prossimo lancio)."
    fi
fi

# Cartella del client web: e' solo ed esclusivamente "Web".
WEB_OK=0
if [ -f "$WEB_DIR/index.html" ]; then
    WEB_OK=1
    log "[WEB] Client web trovato in: $WEB_DIR"
elif [ -d "$WEB_DIR" ]; then
    log "[WEB] (ATTENZIONE) la cartella '$WEB_DIR' esiste ma non contiene index.html: il client web non verra' pubblicato."
else
    log "[WEB] (ATTENZIONE) cartella '$WEB_DIR' non trovata nel repository: il client web non verra' pubblicato."
fi

# ── 4. Compilazione ─────────────────────────────────────────
log "[BUILD] Compilazione server in corso..."
cd "$SERVER_DIR" || fail "Directory server non trovata: $SERVER_DIR"
CSPROJ="$SERVER_DIR/$SERVER_SUBDIR.csproj"
[ -f "$CSPROJ" ] || fail "Progetto non trovato: $CSPROJ"
"$DOTNET_BIN" publish "$CSPROJ" -c Release --self-contained false --nologo || fail "dotnet publish fallito"

DLL="$SERVER_DIR/bin/Release/$TFM/publish/$SERVER_SUBDIR.dll"
[ -f "$DLL" ] || fail "Compilazione riuscita ma la dll non e' dove me l'aspetto: $DLL"
log "[BUILD] Compilazione completata."

# ── 5. nginx per il client web ──────────────────────────────
if [ "$ENABLE_WEB_SETUP" = "true" ] && [ "$WEB_OK" -eq 1 ]; then
    SETUP_WEB_SCRIPT="$SCRIPT_DIR/setup-web.sh"
    if [ -f "$SETUP_WEB_SCRIPT" ]; then
        log "[WEB] Configurazione nginx per il client web..."
        WW_BASE_DIR="$BASE_DIR" bash "$SETUP_WEB_SCRIPT" \
            || log "[WEB] (ATTENZIONE) setup-web.sh ha restituito un errore: il client web potrebbe non essere raggiungibile. Il server di gioco viene avviato comunque."
    else
        log "[WEB] (Attenzione) setup-web.sh non trovato: il client web non verra' servito da nginx."
    fi
fi

# ── 6. Indurimento della VPS ────────────────────────────────
if [ "$ENABLE_HARDENING" = "true" ]; then
    HARDEN_SCRIPT="$SCRIPT_DIR/harden-vps.sh"
    if [ -f "$HARDEN_SCRIPT" ]; then
        log "[HARDENING] Indurimento della VPS..."
        bash "$HARDEN_SCRIPT" \
            || log "[HARDENING] (ATTENZIONE) harden-vps.sh ha restituito un errore: la VPS potrebbe NON essere protetta da fail2ban. Controlla i messaggi qui sopra. Il server di gioco viene avviato comunque."
    else
        log "[HARDENING] (Attenzione) harden-vps.sh non trovato, indurimento saltato."
    fi
fi

# ── 7. Arresto pulito del server ────────────────────────────
# Il vecchio "screen -X quit" ammazzava il processo di netto: tutto cio' che
# GameSave non aveva ancora scritto su disco andava perso ad ogni deploy.
# Qui si prova in tre passi sempre piu' decisi.
#
# Come si individua il processo del server: si prendono SOLO i processi il cui
# eseguibile si chiama esattamente "dotnet", e tra quelli si tengono quelli che
# hanno la nostra dll nella riga di comando.
# Cercare direttamente la stringa "Server Strategico.dll" con "pgrep -f"
# sarebbe pericoloso: troverebbe anche un "nano", un "grep" o un "tail -f"
# aperti su quel percorso in un'altra sessione, e lo script li ucciderebbe.
server_pids() {
    local pid cmd
    for pid in $(pgrep -x dotnet 2>/dev/null); do
        cmd="$(tr '\0' ' ' < "/proc/$pid/cmdline" 2>/dev/null)"
        case "$cmd" in
            *"$SERVER_SUBDIR.dll"*) echo "$pid" ;;
        esac
    done
}

# Termina i processi del server con il segnale indicato. Restituisce sempre
# successo: l'esito vero si verifica con server_pids poco dopo.
signal_server() {
    local sig="$1" pids
    pids="$(server_pids | tr '\n' ' ')"
    [ -n "${pids// /}" ] || return 0
    # shellcheck disable=SC2086
    kill "-$sig" $pids 2>/dev/null || true
}

screen_session_exists() {
    screen -ls 2>/dev/null | grep -qE "[0-9]+\.${SCREEN_NAME}[[:space:]]"
}

log "[SCREEN] Arresto del server in corso..."
waited=0
if screen_session_exists; then
    # 1) si chiede al server di uscire dalla sua console (se il comando non
    #    esiste il server lo ignora: nessun danno)
    screen -S "$SCREEN_NAME" -p 0 -X stuff "quit$(printf '\r')" 2>/dev/null || true
    while [ -n "$(server_pids)" ] && [ "$waited" -lt 10 ]; do sleep 1; waited=$((waited+1)); done
fi
if [ -n "$(server_pids)" ]; then
    # 2) SIGTERM: chiusura ordinata richiesta dal sistema operativo
    log "[SCREEN] Il server non e' uscito da solo, invio SIGTERM..."
    signal_server TERM
    while [ -n "$(server_pids)" ] && [ "$waited" -lt "$STOP_TIMEOUT" ]; do sleep 1; waited=$((waited+1)); done
fi
if [ -n "$(server_pids)" ]; then
    # 3) SIGKILL: ultima spiaggia
    log "[SCREEN] Ancora attivo dopo ${STOP_TIMEOUT}s: terminazione forzata."
    signal_server KILL
    sleep 2
fi
if screen_session_exists; then
    screen -S "$SCREEN_NAME" -X quit 2>/dev/null || true
    sleep 1
fi
log "[SCREEN] Server fermo."

# ── 8. Backup dei salvataggi ────────────────────────────────
# Il server scrive i salvataggi sotto la home dell'utente che lo avvia
# (Environment.SpecialFolder.LocalApplicationData in GameSave.cs).
SAVE_DIR="${XDG_DATA_HOME:-$HOME/.local/share}/Server Strategico/Saves_Test"
if [ -d "$SAVE_DIR" ]; then
    mkdir -p "$BACKUP_DIR" && chmod 700 "$BACKUP_DIR"
    ARCHIVE="$BACKUP_DIR/saves-$(date +%Y%m%d-%H%M%S).tar.gz"
    if tar czf "$ARCHIVE" -C "$(dirname "$SAVE_DIR")" "$(basename "$SAVE_DIR")" 2>/dev/null; then
        chmod 600 "$ARCHIVE"
        log "[BACKUP] Salvataggi archiviati: $ARCHIVE"
        # rotazione: si tengono solo gli ultimi $KEEP_BACKUPS
        mapfile -t OLD_BACKUPS < <(ls -1t "$BACKUP_DIR"/saves-*.tar.gz 2>/dev/null | tail -n +$((KEEP_BACKUPS + 1)))
        if [ "${#OLD_BACKUPS[@]}" -gt 0 ]; then
            rm -f "${OLD_BACKUPS[@]}"
            log "[BACKUP] Rimossi ${#OLD_BACKUPS[@]} backup vecchi (ne conservo $KEEP_BACKUPS)."
        fi
    else
        rm -f "$ARCHIVE"
        log "[BACKUP] (Attenzione) backup dei salvataggi non riuscito, proseguo comunque."
    fi
else
    log "[BACKUP] Nessuna cartella salvataggi trovata in '$SAVE_DIR' (prima esecuzione?): backup saltato."
fi

# ── 9. Avvio del server ─────────────────────────────────────
if ! touch "$LOG_FILE" 2>/dev/null; then
    LOG_FILE="$BASE_DIR/warrior-server.log"
    touch "$LOG_FILE" 2>/dev/null || true
fi
chmod 640 "$LOG_FILE" 2>/dev/null || true

log "[SCREEN] Avvio server nella sessione '$SCREEN_NAME' (gateway web: $ENABLE_WEB_GATEWAY)..."
# I percorsi sono passati come ARGOMENTI separati, non incollati dentro la
# stringa del comando: cosi' nomi di cartella con spazi (o con caratteri
# speciali) non possono rompere o alterare il comando eseguito.
if ! screen -L -Logfile "$LOG_FILE" -dmS "$SCREEN_NAME" \
        env WW_WEB_GATEWAY="$ENABLE_WEB_GATEWAY" HOME="$HOME" \
        bash -c 'cd "$1" || exit 1; exec "$2" "$3"' _ "$SERVER_DIR" "$DOTNET_BIN" "$DLL"; then
    # screen molto vecchio senza -Logfile: si riprova senza log su file
    log "[SCREEN] (Nota) log su file non supportato da questa versione di screen, avvio senza."
    screen -dmS "$SCREEN_NAME" \
        env WW_WEB_GATEWAY="$ENABLE_WEB_GATEWAY" HOME="$HOME" \
        bash -c 'cd "$1" || exit 1; exec "$2" "$3"' _ "$SERVER_DIR" "$DOTNET_BIN" "$DLL" \
        || fail "Avvio della sessione screen fallito"
fi

# ── 10. Verifica che sia DAVVERO partito ────────────────────
# Prima lo script stampava "[OK] Server avviato" anche quando il processo era
# morto un secondo dopo (porta occupata, dll mancante, eccezione all'avvio).
log "[VERIFICA] Attendo che il server si metta in ascolto sulla porta $GAME_TCP_PORT..."
started=0
waited=0
while [ "$waited" -lt "$START_TIMEOUT" ]; do
    if ss -ltn 2>/dev/null | grep -qE "[:.]${GAME_TCP_PORT}[[:space:]]"; then started=1; break; fi
    if [ -z "$(server_pids)" ] && [ "$waited" -gt 3 ]; then break; fi
    sleep 1
    waited=$((waited + 1))
done

if [ "$started" -ne 1 ]; then
    echo ""
    echo "---- ultime righe del log del server ----"
    tail -n 30 "$LOG_FILE" 2>/dev/null || echo "(nessun log disponibile)"
    echo "----------------------------------------"
    fail "Il server NON e' in ascolto sulla porta $GAME_TCP_PORT dopo ${START_TIMEOUT}s. Il deploy e' fallito: controlla il log qui sopra."
fi

# ── 11. Riepilogo ───────────────────────────────────────────
echo ""
echo "============================================"
echo " Deploy completato"
echo "============================================"
echo " Server in ascolto  : porta $GAME_TCP_PORT (desktop)"
ss -ltn 2>/dev/null | grep -qE "[:.]8444[[:space:]]" \
    && echo " Gateway WebSocket  : porta 8444 (client web) - attivo" \
    || echo " Gateway WebSocket  : porta 8444 - NON in ascolto (WW_WEB_GATEWAY=$ENABLE_WEB_GATEWAY)"
echo " Log del server     : $LOG_FILE"
echo " Backup salvataggi  : $BACKUP_DIR"
echo " Rientra in console : screen -r $SCREEN_NAME  (per uscire: Ctrl+A poi D)"
if [ -f /var/run/reboot-required ]; then
    echo ""
    echo " [!] Sono stati installati aggiornamenti che richiedono un RIAVVIO"
    echo "     della VPS (probabilmente il kernel). Quando puoi permetterti"
    echo "     qualche minuto di gioco offline: 'reboot'."
fi
echo ""
