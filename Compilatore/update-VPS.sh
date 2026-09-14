#!/bin/bash
# ── Rete di sicurezza CRLF ──────────────────────────────────
# Se questo file arriva da Windows con terminatori di riga CRLF, bash non
# riesce nemmeno a leggerlo ("syntax error: unexpected end of file"). La riga
# qui sotto ripara il file e riparte da sola. DEVE restare su una riga sola,
# commento finale compreso: e' l'unica forma che sopravvive al CRLF.
grep -q $'\r' "$0" && sed -i 's/\r$//' "$0" && exec bash "$0" "$@" # crlf-self-heal
# ============================================================================
#  update-VPS.sh — Setup iniziale di una VPS Ubuntu appena noleggiata
#  Warrior & Wealth
# ----------------------------------------------------------------------------
#  Da eseguire UNA VOLTA come root su una macchina nuova. E' comunque
#  idempotente: rieseguirlo non rompe niente e non duplica nulla.
#
#  NON tocca MAI /etc/ssh/sshd_config ne' l'autenticazione SSH.
#  Prima di attivare il firewall verifica esplicitamente che la regola per
#  la TUA porta SSH esista: se non c'e', si ferma invece di chiuderti fuori.
# ============================================================================
set -uo pipefail

# ── Configurazione ──────────────────────────────────────────
DOTNET_CHANNEL="8.0"             # canale .NET (SDK per compilare il server)
DOTNET_FALLBACK_DIR="/usr/local/dotnet"
GAME_TCP_PORT=8443               # WatsonTcp — client desktop
GAME_WS_PORT=8444                # gateway WebSocket — client web
HTTP_PORT=80                     # nginx — pagina del client web
# ────────────────────────────────────────────────────────────

# apt non interattivo: su Ubuntu 24 "needrestart" e i prompt di conflitto sui
# file di configurazione bloccano lo script all'infinito in un deploy
# automatico. Queste tre righe sono quelle che lo impediscono.
export DEBIAN_FRONTEND=noninteractive
export NEEDRESTART_MODE=a
export NEEDRESTART_SUSPEND=1
APT_OPTS=(-y -o DPkg::Lock::Timeout=600 -o Dpkg::Options::=--force-confold -o Dpkg::Options::=--force-confdef)

fail() { echo "[ERRORE] $1" >&2; exit 1; }
log()  { echo "$1"; }

echo "============================================"
echo " Warrior & Wealth - Setup Server Iniziale"
echo "============================================"
echo ""

# ── 0. Deve girare come root ────────────────────────────────
if [ "$EUID" -ne 0 ]; then
    fail "Esegui questo script come root (o con sudo): installa pacchetti e configura il firewall."
fi

# ── 0b. Porta SSH reale (sola lettura, non modifichiamo nulla) ──
# Se SSH e' su una porta non standard e aprissimo solo la 22, "ufw enable"
# ti chiuderebbe fuori dalla VPS all'istante.
detect_ssh_port() {
    local p=""
    p="$(sshd -T 2>/dev/null | awk '/^port /{print $2; exit}')"
    if [ -z "$p" ]; then
        p="$(awk '/^[[:space:]]*[Pp]ort[[:space:]]+[0-9]+/{print $2; exit}' /etc/ssh/sshd_config 2>/dev/null)"
    fi
    [ -z "$p" ] && p="22"
    echo "$p"
}
SSH_PORT="$(detect_ssh_port)"
log "[SSH] Porta SSH rilevata: $SSH_PORT (nessuna modifica alla configurazione SSH)."

# ── 1. Aggiornamento sistema ────────────────────────────────
log "[APT] Aggiornamento pacchetti..."
apt-get update "${APT_OPTS[@]}" || fail "apt update fallito"
apt-get upgrade "${APT_OPTS[@]}" || fail "apt upgrade fallito"
log "[APT] Sistema aggiornato."

# ── 2. Dipendenze base ──────────────────────────────────────
log "[APT] Installazione dipendenze base..."
apt-get install "${APT_OPTS[@]}" \
    ca-certificates \
    git \
    screen \
    curl \
    wget \
    unzip \
    htop \
    ufw \
    nginx \
    rsync \
    iproute2 \
    || fail "Installazione dipendenze fallita"
log "[APT] Dipendenze installate."

# ── 3. .NET SDK ─────────────────────────────────────────────
# Prima si prova il pacchetto ufficiale Ubuntu. Solo se manca si usa lo
# script Microsoft, installando in una cartella di sistema ($DOTNET_FALLBACK_DIR)
# e NON sotto /root: cosi' il percorso di dotnet non dipende da quale utente
# ha lanciato lo script.
if command -v dotnet >/dev/null 2>&1; then
    log "[DOTNET] Gia' presente: $(command -v dotnet) ($(dotnet --version 2>/dev/null))"
else
    log "[DOTNET] Installazione .NET SDK ${DOTNET_CHANNEL} da apt..."
    if ! apt-get install "${APT_OPTS[@]}" "dotnet-sdk-${DOTNET_CHANNEL}"; then
        log "[DOTNET] Pacchetto apt non disponibile, uso lo script ufficiale Microsoft."
        DOTNET_TMP="$(mktemp)"
        curl -fsSL --proto '=https' --tlsv1.2 https://dot.net/v1/dotnet-install.sh -o "$DOTNET_TMP" \
            || { rm -f "$DOTNET_TMP"; fail "Download di dotnet-install.sh fallito"; }
        # Controllo minimo di sanita': deve essere davvero uno script di shell,
        # non una pagina di errore HTML del proxy o un file vuoto.
        if ! head -n 1 "$DOTNET_TMP" | grep -q '^#!'; then
            rm -f "$DOTNET_TMP"
            fail "Il file scaricato non e' uno script valido (download corrotto o bloccato)."
        fi
        bash "$DOTNET_TMP" --channel "$DOTNET_CHANNEL" --install-dir "$DOTNET_FALLBACK_DIR" \
            || { rm -f "$DOTNET_TMP"; fail "Installazione .NET tramite script Microsoft fallita"; }
        rm -f "$DOTNET_TMP"
        ln -sf "$DOTNET_FALLBACK_DIR/dotnet" /usr/local/bin/dotnet
    fi
    log "[DOTNET] .NET installato."
fi

# ── 4. Verifica dotnet ──────────────────────────────────────
DOTNET_BIN="$(command -v dotnet)"
[ -n "$DOTNET_BIN" ] || fail "dotnet non disponibile dopo l'installazione"
"$DOTNET_BIN" --version >/dev/null 2>&1 || fail "dotnet installato ma non funzionante"
log "[DOTNET] Eseguibile: $DOTNET_BIN"

# ── 5. Firewall ─────────────────────────────────────────────
# Ordine obbligatorio: PRIMA si apre SSH, POI si verifica che la regola ci
# sia davvero, e solo a quel punto si abilita il firewall.
log "[UFW] Configurazione firewall..."
# "limit" invece di "allow": consente SSH ma rallenta chi tenta piu' di 6
# connessioni in 30 secondi dallo stesso IP (blocco temporaneo, non un ban).
ufw limit "$SSH_PORT"/tcp comment 'SSH (rate limited)'      || fail "ufw: regola SSH non applicata"
ufw allow "$GAME_TCP_PORT"/tcp comment 'Warrior&Wealth WatsonTcp (client desktop)'
ufw allow "$GAME_WS_PORT"/tcp  comment 'Warrior&Wealth WebSocket (client web)'
ufw allow "$HTTP_PORT"/tcp     comment 'nginx HTTP (client web)'

if ! ufw status | grep -qE "(^|[[:space:]])${SSH_PORT}/tcp"; then
    fail "La regola per la porta SSH ($SSH_PORT) non risulta presente: NON abilito il firewall per non chiuderti fuori. Controlla 'ufw status' a mano."
fi

ufw --force enable || fail "Abilitazione ufw fallita"
ufw logging low
log "[UFW] Firewall attivo (SSH $SSH_PORT, $GAME_TCP_PORT, $GAME_WS_PORT, $HTTP_PORT)."

# ── 6. Verifica finale ──────────────────────────────────────
echo ""
echo "============================================"
echo " Verifica installazioni"
echo "============================================"
echo -n " git     : "; git --version
echo -n " screen  : "; screen --version | head -1
echo -n " dotnet  : "; "$DOTNET_BIN" --version
echo -n " nginx   : "; nginx -v 2>&1
echo -n " rsync   : "; rsync --version | head -1
echo -n " ufw     : "; ufw status | head -1
echo ""
echo "[OK] Setup completato. Puoi ora eseguire update-server.sh"
