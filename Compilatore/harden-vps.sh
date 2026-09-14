#!/bin/bash
# ── Rete di sicurezza CRLF ──────────────────────────────────
# Se questo file arriva da Windows con terminatori di riga CRLF, bash non
# riesce nemmeno a leggerlo ("syntax error: unexpected end of file"). La riga
# qui sotto ripara il file e riparte da sola. DEVE restare su una riga sola,
# commento finale compreso: e' l'unica forma che sopravvive al CRLF.
grep -q $'\r' "$0" && sed -i 's/\r$//' "$0" && exec bash "$0" "$@" # crlf-self-heal
# ============================================================================
#  harden-vps.sh — Indurimento di sicurezza della VPS
#  Warrior & Wealth
# ----------------------------------------------------------------------------
#  Fa SOLO le cose che non possono chiuderti fuori dalla macchina:
#    1. fail2ban: blocca gli IP che provano password SSH a raffica
#    2. aggiornamenti di sicurezza automatici
#    3. parametri di rete del kernel piu' prudenti
#    4. firewall: nega tutto cio' che non e' esplicitamente aperto
#    5. rotazione del log del server di gioco
#
#  NON tocca MAI /etc/ssh/sshd_config ne' l'autenticazione SSH: quella va
#  cambiata a mano e verificata passo-passo PRIMA di disabilitare la password,
#  altrimenti si rischia di restare bloccati fuori dalla VPS.
#  Qui il file sshd_config viene solo LETTO, per sapere su quale porta gira SSH.
#
#  Idempotente: si puo' rilanciare ad ogni deploy.
# ============================================================================
set -uo pipefail

# ── Configurazione ──────────────────────────────────────────
BANTIME="1h"                     # durata del primo blocco
FINDTIME="10m"                   # finestra entro cui contare i tentativi
MAXRETRY=5                       # tentativi falliti prima del blocco
ENABLE_RECIDIVE="true"           # blocco lungo per chi torna piu' volte
ENABLE_NGINX_JAIL="false"        # blocco di chi cerca pagine di exploit sul sito
                                 # (lasciato spento: puo' bloccare utenti veri)
AUTO_REBOOT="false"              # riavvio automatico dopo gli aggiornamenti
                                 # (spento: un riavvio a sorpresa spegne il gioco)
SERVER_LOG="/var/log/warrior-server.log"
# Il tuo IP viene esentato dai blocchi, cosi' sbagliare 5 volte la password di
# root non ti chiude fuori da casa tua. Se non lo imposti, viene ricavato
# automaticamente dalla connessione SSH da cui stai lanciando lo script.
ADMIN_IP="${WW_ADMIN_IP:-}"
# ────────────────────────────────────────────────────────────

export DEBIAN_FRONTEND=noninteractive
export NEEDRESTART_MODE=a
export NEEDRESTART_SUSPEND=1
APT_OPTS=(-y -o DPkg::Lock::Timeout=600 -o Dpkg::Options::=--force-confold -o Dpkg::Options::=--force-confdef)

fail() { echo "[ERRORE] $1" >&2; exit 1; }
log()  { echo "$1"; }

echo "============================================"
echo " Warrior & Wealth - Indurimento VPS"
echo "============================================"
echo ""

if [ "$EUID" -ne 0 ]; then
    fail "Esegui questo script come root (o con sudo)."
fi

# ── 0. Porta SSH e IP amministratore ────────────────────────
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

if [ -z "$ADMIN_IP" ] && [ -n "${SSH_CLIENT:-}" ]; then
    ADMIN_IP="$(echo "$SSH_CLIENT" | awk '{print $1}')"
fi
IGNORE_IPS="127.0.0.1/8 ::1"
if [ -n "$ADMIN_IP" ]; then
    IGNORE_IPS="$IGNORE_IPS $ADMIN_IP"
    log "[SSH] Porta SSH: $SSH_PORT — il tuo IP ($ADMIN_IP) sara' esentato dai blocchi."
else
    log "[SSH] Porta SSH: $SSH_PORT — nessun IP amministratore rilevato (stai lanciando lo script non via SSH?)."
    log "[SSH]   Se vuoi esentare il tuo IP: WW_ADMIN_IP=1.2.3.4 bash harden-vps.sh"
fi

# ── 1. fail2ban ─────────────────────────────────────────────
log ""
log "[FAIL2BAN] Installazione..."
if ! command -v fail2ban-client &>/dev/null; then
    apt-get update "${APT_OPTS[@]}" || fail "apt update fallito"
    apt-get install "${APT_OPTS[@]}" fail2ban || fail "Installazione fail2ban fallita"
else
    log "[FAIL2BAN] Gia' installato."
fi
# Serve a fail2ban per leggere i log direttamente dal journal di systemd.
# Senza questo pacchetto il backend "systemd" non funziona.
apt-get install "${APT_OPTS[@]}" python3-systemd >/dev/null 2>&1 \
    || log "[FAIL2BAN] (Attenzione) python3-systemd non installato: il backend systemd potrebbe non funzionare."

# Le versioni precedenti di questo script scrivevano /etc/fail2ban/jail.local.
# Ora si usa jail.d/, che e' il posto giusto per le configurazioni aggiuntive:
# si rimuove il vecchio file SOLO se lo avevamo generato noi.
if [ -f /etc/fail2ban/jail.local ] && grep -q "Generato da harden-vps.sh" /etc/fail2ban/jail.local; then
    rm -f /etc/fail2ban/jail.local
    log "[FAIL2BAN] Rimosso il vecchio jail.local generato da questo script."
fi

# NOTA IMPORTANTE (il motivo per cui prima non funzionava):
# su Ubuntu 24.04 le immagini per VPS non installano piu' rsyslog, quindi
# /var/log/auth.log NON esiste e fail2ban con la configurazione predefinita
# non trova nulla da leggere: la jail sshd muore all'avvio mentre il servizio
# fail2ban risulta "attivo". Con "backend = systemd" legge invece dal journal
# di sistema, che c'e' sempre.
mkdir -p /etc/fail2ban/jail.d
cat > /etc/fail2ban/jail.d/ww-hardening.local <<EOF
# Generato da harden-vps.sh — non modificare a mano, viene sovrascritto.
[DEFAULT]
ignoreip  = $IGNORE_IPS
bantime   = $BANTIME
findtime  = $FINDTIME
maxretry  = $MAXRETRY
# Chi insiste viene bloccato ogni volta piu' a lungo (1h, 2h, 4h... max 1 settimana).
bantime.increment = true
bantime.factor    = 2
bantime.maxtime   = 1w

[sshd]
enabled  = true
port     = $SSH_PORT
backend  = systemd
EOF

if [ "$ENABLE_RECIDIVE" = "true" ]; then
    cat >> /etc/fail2ban/jail.d/ww-hardening.local <<'EOF'

# Chi si fa bloccare piu' volte viene messo fuori per una settimana intera.
[recidive]
enabled  = true
backend  = auto
logpath  = /var/log/fail2ban.log
bantime  = 1w
findtime = 1d
maxretry = 3
EOF
fi

if [ "$ENABLE_NGINX_JAIL" = "true" ]; then
    cat >> /etc/fail2ban/jail.d/ww-hardening.local <<'EOF'

[nginx-botsearch]
enabled  = true
backend  = auto
logpath  = /var/log/nginx/access.log
maxretry = 10
EOF
fi

systemctl enable fail2ban >/dev/null 2>&1
systemctl restart fail2ban || fail "Avvio di fail2ban fallito"

# Verifica VERA: il servizio puo' risultare attivo mentre la jail e' morta.
# Prima questo controllo non c'era e l'errore restava invisibile.
sleep 2
if fail2ban-client status sshd >/dev/null 2>&1; then
    BANNED="$(fail2ban-client status sshd 2>/dev/null | awk -F: '/Currently banned/{print $2}' | tr -d ' \t')"
    log "[FAIL2BAN] Jail sshd ATTIVA (porta $SSH_PORT, IP attualmente bloccati: ${BANNED:-0})."
else
    log "[FAIL2BAN] ============================================"
    log "[FAIL2BAN] ATTENZIONE: la jail sshd NON risulta attiva!"
    log "[FAIL2BAN] La VPS non e' protetta dai tentativi di password."
    log "[FAIL2BAN] Dettagli:"
    fail2ban-client status 2>&1 | sed 's/^/[FAIL2BAN]   /'
    journalctl -u fail2ban -n 15 --no-pager 2>/dev/null | sed 's/^/[FAIL2BAN]   /'
    log "[FAIL2BAN] ============================================"
fi

# ── 2. Aggiornamenti di sicurezza automatici ────────────────
log ""
log "[UPDATES] Configurazione aggiornamenti automatici..."
apt-get install "${APT_OPTS[@]}" unattended-upgrades || fail "Installazione unattended-upgrades fallita"

# Si scrive la configurazione a mano invece di affidarsi a "dpkg-reconfigure":
# la risposta predefinita non e' garantita e cambia tra le versioni di Ubuntu.
cat > /etc/apt/apt.conf.d/20auto-upgrades <<'EOF'
// Generato da harden-vps.sh
APT::Periodic::Update-Package-Lists "1";
APT::Periodic::Unattended-Upgrade "1";
APT::Periodic::AutocleanInterval "7";
EOF

cat > /etc/apt/apt.conf.d/51ww-unattended <<EOF
// Generato da harden-vps.sh
Unattended-Upgrade::Remove-Unused-Kernel-Packages "true";
Unattended-Upgrade::Remove-Unused-Dependencies "true";
// Riavvio automatico: tenuto SPENTO di proposito, altrimenti la VPS potrebbe
// riavviarsi nel mezzo di una partita. Quando serve un riavvio lo script di
// deploy te lo segnala alla fine.
Unattended-Upgrade::Automatic-Reboot "$AUTO_REBOOT";
EOF
chmod 644 /etc/apt/apt.conf.d/20auto-upgrades /etc/apt/apt.conf.d/51ww-unattended
systemctl enable unattended-upgrades >/dev/null 2>&1
systemctl restart unattended-upgrades >/dev/null 2>&1
log "[UPDATES] Aggiornamenti di sicurezza automatici attivi (riavvio automatico: $AUTO_REBOOT)."

# ── 3. Parametri di rete del kernel ─────────────────────────
# Impostazioni standard e conservative: nessuna di queste tocca SSH o le porte
# del gioco, servono a non farsi ingannare da pacchetti costruiti ad arte.
log ""
log "[KERNEL] Applicazione parametri di rete..."
cat > /etc/sysctl.d/99-ww-hardening.conf <<'EOF'
# Generato da harden-vps.sh — non modificare a mano.
# Resiste agli attacchi SYN flood.
net.ipv4.tcp_syncookies = 1
# Scarta i pacchetti con indirizzo mittente falsificato.
net.ipv4.conf.all.rp_filter = 1
net.ipv4.conf.default.rp_filter = 1
# Ignora i "redirect" ICMP: possono essere usati per dirottare il traffico.
net.ipv4.conf.all.accept_redirects = 0
net.ipv4.conf.default.accept_redirects = 0
net.ipv4.conf.all.secure_redirects = 0
net.ipv4.conf.all.send_redirects = 0
net.ipv4.conf.default.send_redirects = 0
net.ipv6.conf.all.accept_redirects = 0
net.ipv6.conf.default.accept_redirects = 0
# Rifiuta i pacchetti che dettano il proprio percorso di ritorno.
net.ipv4.conf.all.accept_source_route = 0
net.ipv4.conf.default.accept_source_route = 0
net.ipv6.conf.all.accept_source_route = 0
# Non risponde ai ping inviati a tutta la rete (usati per amplificare attacchi).
net.ipv4.icmp_echo_ignore_broadcasts = 1
net.ipv4.icmp_ignore_bogus_error_responses = 1
# Posizioni di memoria casuali ad ogni avvio di un programma.
kernel.randomize_va_space = 2
# Impedisce un vecchio trucco per far leggere file altrui ai servizi di sistema.
fs.protected_hardlinks = 1
fs.protected_symlinks = 1
EOF
sysctl --system >/dev/null 2>&1 || log "[KERNEL] (Attenzione) alcuni parametri non sono stati applicati (normale se IPv6 e' disattivato)."
log "[KERNEL] Parametri applicati."

# ── 4. Firewall ─────────────────────────────────────────────
log ""
log "[UFW] Impostazione politiche..."
if ! command -v ufw &>/dev/null; then
    apt-get install "${APT_OPTS[@]}" ufw || fail "Installazione ufw fallita"
fi
# Anche qui: PRIMA si garantisce l'accesso SSH, POI si nega tutto il resto.
ufw limit "$SSH_PORT"/tcp comment 'SSH (rate limited)' >/dev/null 2>&1
ufw default deny incoming  >/dev/null 2>&1
ufw default allow outgoing >/dev/null 2>&1
ufw logging low >/dev/null 2>&1

if ufw status | head -1 | grep -q inactive; then
    if ufw status | grep -qE "(^|[[:space:]])${SSH_PORT}/tcp"; then
        ufw --force enable >/dev/null 2>&1 && log "[UFW] Firewall attivato."
    else
        log "[UFW] (Attenzione) firewall NON attivato: manca la regola per la porta SSH $SSH_PORT."
        log "[UFW]   Non lo abilito per non chiuderti fuori. Esegui prima update-VPS.sh."
    fi
else
    log "[UFW] Firewall gia' attivo."
fi

# ── 5. Rotazione del log del server di gioco ────────────────
# Senza questo il file cresce all'infinito fino a riempire il disco.
cat > /etc/logrotate.d/warrior-server <<EOF
$SERVER_LOG {
    weekly
    rotate 8
    compress
    delaycompress
    missingok
    notifempty
    # copytruncate: il log e' tenuto aperto da screen, non si puo' rinominare.
    copytruncate
}
EOF
log ""
log "[LOG] Rotazione settimanale configurata per $SERVER_LOG."

# ── 6. Verifica finale ──────────────────────────────────────
echo ""
echo "============================================"
echo " Riepilogo sicurezza"
echo "============================================"
if systemctl is-active --quiet fail2ban; then
    if fail2ban-client status sshd >/dev/null 2>&1; then
        echo " fail2ban SSH        : ATTIVO (porta $SSH_PORT)"
    else
        echo " fail2ban SSH        : servizio attivo ma JAIL NON FUNZIONANTE (vedi sopra)"
    fi
else
    echo " fail2ban SSH        : NON attivo"
fi
if [ "$ENABLE_RECIDIVE" = "true" ]; then
    fail2ban-client status recidive >/dev/null 2>&1 \
        && echo " fail2ban recidivi   : attivo" \
        || echo " fail2ban recidivi   : non attivo (non critico)"
fi
[ -f /etc/apt/apt.conf.d/20auto-upgrades ] && echo " aggiornamenti auto  : attivi" || echo " aggiornamenti auto  : NON attivi"
[ -f /etc/sysctl.d/99-ww-hardening.conf ] && echo " parametri kernel    : applicati" || echo " parametri kernel    : NON applicati"
echo " IP esentati         : $IGNORE_IPS"
echo ""
ufw status verbose
echo ""
if [ -f /var/run/reboot-required ]; then
    echo " [!] E' in sospeso un riavvio della VPS per completare gli aggiornamenti."
    echo ""
fi
echo "[OK] Indurimento completato."
echo "     Ricorda: l'autenticazione SSH non e' stata toccata (password di root"
echo "     ancora attiva). E' il prossimo passo consigliato, da fare a mano."
