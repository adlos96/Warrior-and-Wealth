#!/bin/bash
# ── Rete di sicurezza CRLF ──────────────────────────────────
# Se questo file arriva da Windows con terminatori di riga CRLF, bash non
# riesce nemmeno a leggerlo ("syntax error: unexpected end of file"). La riga
# qui sotto ripara il file e riparte da sola. DEVE restare su una riga sola,
# commento finale compreso: e' l'unica forma che sopravvive al CRLF.
grep -q $'\r' "$0" && sed -i 's/\r$//' "$0" && exec bash "$0" "$@" # crlf-self-heal
# ============================================================================
#  setup-web.sh — nginx per il client web di Warrior & Wealth
# ----------------------------------------------------------------------------
#  Copia il client web in /var/www e configura nginx per servirlo sulla porta 80.
#  Viene richiamato da update-server.sh ad ogni deploy ed e' idempotente.
#
#  Perche' copia i file invece di servirli dal repo: nginx gira come utente
#  "www-data", e la home di root e' leggibile solo da root. Servire il sito
#  direttamente da /root/... produce un "403 Forbidden" su tutto, con nginx
#  perfettamente attivo e nessun errore evidente.
#
#  NON tocca MAI /etc/ssh/sshd_config ne' l'autenticazione SSH.
# ============================================================================
set -uo pipefail

# ── Configurazione ──────────────────────────────────────────
# Lasciare DOMAIN vuoto finche' non hai un dominio puntato su questa VPS.
# Appena lo hai: scrivi qui il nome (es. "gioco.tuodominio.it") e la tua email,
# e al deploy successivo lo script richiede da solo il certificato Let's
# Encrypt e passa a HTTPS, rinnovo automatico compreso.
DOMAIN="${WW_DOMAIN:-}"
LETSENCRYPT_EMAIL="${WW_LE_EMAIL:-}"

BASE_DIR="${WW_BASE_DIR:-$HOME/Warrior-and-Wealth}"
PUBLISH_DIR="/var/www/warrior-and-wealth"
SITE_NAME="warrior-and-wealth"
HTTP_PORT=80
WS_BACKEND="127.0.0.1:8444"      # gateway WebSocket del server di gioco
SNIPPET_DIR="/etc/nginx/snippets"
# ────────────────────────────────────────────────────────────

export DEBIAN_FRONTEND=noninteractive
export NEEDRESTART_MODE=a
APT_OPTS=(-y -o DPkg::Lock::Timeout=600 -o Dpkg::Options::=--force-confold)

fail() { echo "[ERRORE] $1" >&2; exit 1; }
log()  { echo "$1"; }

echo "============================================"
echo " Warrior & Wealth - Setup Client Web (nginx)"
echo "============================================"
echo ""

# ── 0. Root ─────────────────────────────────────────────────
if [ "$EUID" -ne 0 ]; then
    fail "Esegui questo script come root (o con sudo)."
fi

# ── 1. Cartella sorgente del client web ─────────────────────
# Il client web sta SOLO nella cartella "Web" del repository: nessun altro
# nome viene accettato.
WEB_DIR="$BASE_DIR/Web"
[ -d "$WEB_DIR" ] || fail "Cartella '$WEB_DIR' non trovata. Il client web deve stare nella cartella 'Web' del repository; esegui prima update-server.sh."
[ -f "$WEB_DIR/index.html" ] || fail "In '$WEB_DIR' non c'e' un index.html: non e' la cartella del client web."
log "[WEB] Sorgente: $WEB_DIR"

# ── 2. nginx installato ─────────────────────────────────────
if ! command -v nginx &>/dev/null; then
    log "[NGINX] nginx non trovato, installazione in corso..."
    apt-get update "${APT_OPTS[@]}" || fail "apt update fallito"
    apt-get install "${APT_OPTS[@]}" nginx || fail "Installazione nginx fallita"
fi
command -v rsync &>/dev/null || apt-get install "${APT_OPTS[@]}" rsync >/dev/null 2>&1 || true

# ── 3. Pubblicazione dei file in /var/www ───────────────────
log "[WEB] Copia dei file in $PUBLISH_DIR..."
mkdir -p "$PUBLISH_DIR" || fail "Impossibile creare $PUBLISH_DIR"
if command -v rsync &>/dev/null; then
    rsync -a --delete \
        --exclude '.git' --exclude '.git/**' --exclude 'node_modules' \
        "$WEB_DIR"/ "$PUBLISH_DIR"/ || fail "Copia dei file del client web fallita"
else
    rm -rf "${PUBLISH_DIR:?}"/* 2>/dev/null
    cp -a "$WEB_DIR"/. "$PUBLISH_DIR"/ || fail "Copia dei file del client web fallita"
fi
# Leggibile da nginx (gruppo www-data), non scrivibile da lui, invisibile agli
# altri utenti della macchina.
chown -R root:www-data "$PUBLISH_DIR"
chmod -R u=rwX,g=rX,o= "$PUBLISH_DIR"
log "[WEB] $(find "$PUBLISH_DIR" -type f | wc -l) file pubblicati."

# ── 4. Limiti di richieste (protezione da flood) ────────────
# Le "zone" vanno dichiarate nel contesto http, quindi in conf.d e non nel
# server block. Valori volutamente generosi: meglio non bloccare giocatori veri.
cat > /etc/nginx/conf.d/ww-limits.conf <<'EOF'
# Generato da setup-web.sh — non modificare a mano.
limit_req_zone  $binary_remote_addr zone=ww_web:10m rate=30r/s;
limit_req_zone  $binary_remote_addr zone=ww_ws:10m  rate=2r/s;
limit_conn_zone $binary_remote_addr zone=ww_conn:10m;
EOF

# ── 5. Certificato gia' presente? ───────────────────────────
# Si usa "certbot certonly", che NON riscrive i file di nginx: la
# configurazione resta sempre quella generata da questo script, altrimenti ad
# ogni deploy le modifiche di certbot verrebbero sovrascritte e HTTPS si
# romperebbe silenziosamente.
CERT_DIR="/etc/letsencrypt/live/${DOMAIN}"
HAS_CERT=0
if [ -n "$DOMAIN" ] && [ -f "$CERT_DIR/fullchain.pem" ]; then HAS_CERT=1; fi

SERVER_NAME="_"
[ -n "$DOMAIN" ] && SERVER_NAME="$DOMAIN"

# Alcune VPS non hanno IPv6 attivo. In quel caso "listen [::]:80" non fa
# partire nginx DEL TUTTO ("Address family not supported by protocol"), quindi
# la riga si scrive solo se IPv6 esiste davvero su questa macchina.
if [ -f /proc/net/if_inet6 ]; then
    LISTEN6_HTTP="    listen [::]:$HTTP_PORT;"
    LISTEN6_HTTPS="    listen [::]:443 ssl;"
else
    LISTEN6_HTTP="    # (IPv6 non disponibile su questa VPS)"
    LISTEN6_HTTPS="    # (IPv6 non disponibile su questa VPS)"
fi

CONF_PATH="/etc/nginx/sites-available/$SITE_NAME"
LINK_PATH="/etc/nginx/sites-enabled/$SITE_NAME"
BACKUP_PATH="${CONF_PATH}.bak"

# ── 6. Generazione della configurazione ─────────────────────
write_security_snippet() {
    mkdir -p "$SNIPPET_DIR"
    cat > "$SNIPPET_DIR/ww-security-headers.conf" <<'EOF'
# Generato da setup-web.sh — non modificare a mano.
# Impedisce che il sito venga caricato dentro un iframe di terzi (clickjacking).
add_header X-Frame-Options "SAMEORIGIN" always;
# Impedisce al browser di "indovinare" il tipo di un file ignorando il server.
add_header X-Content-Type-Options "nosniff" always;
# Non manda l'indirizzo completo della pagina ai siti esterni.
add_header Referrer-Policy "strict-origin-when-cross-origin" always;
# Nega di default l'accesso a webcam, microfono e posizione.
add_header Permissions-Policy "camera=(), microphone=(), geolocation=()" always;
EOF
    # HSTS ha senso solo con HTTPS attivo: dice al browser di non usare mai
    # piu' http:// per questo dominio. Metterlo prima di avere il certificato
    # renderebbe il sito irraggiungibile.
    if [ "$HAS_CERT" -eq 1 ]; then
        echo 'add_header Strict-Transport-Security "max-age=15768000" always;' >> "$SNIPPET_DIR/ww-security-headers.conf"
    fi
}

# Corpo del sito, identico sia sulla 80 (senza certificato) sia sulla 443.
write_site_body() {
    cat >> "$CONF_PATH" <<EOF

    # Compressione: i file del client sono piccoli ma richiesti spesso.
    gzip on;
    gzip_vary on;
    gzip_min_length 512;
    gzip_types text/plain text/css application/javascript application/json image/svg+xml;

    # Gateway WebSocket del server di gioco, raggiungibile anche su /ws.
    # La porta 8444 resta comunque aperta: il client attuale continua a
    # funzionare senza modifiche. Quando il JavaScript passera' a /ws si
    # potra' chiudere la 8444 dall'esterno con: ufw delete allow 8444/tcp
    location /ws {
        proxy_pass http://$WS_BACKEND/;
        proxy_http_version 1.1;
        proxy_set_header Upgrade \$http_upgrade;
        proxy_set_header Connection "upgrade";
        proxy_set_header Host \$host;
        proxy_set_header X-Real-IP \$remote_addr;
        proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_read_timeout 3600s;
        proxy_send_timeout 3600s;
        limit_req  zone=ww_ws burst=10 nodelay;
        limit_conn ww_conn 20;
        include $SNIPPET_DIR/ww-security-headers.conf;
    }

    # File nascosti (.git, .env, ...): mai serviti.
    location ~ /\\. {
        deny all;
    }

    # La pagina principale non va messa in cache, altrimenti dopo un deploy
    # i giocatori continuerebbero a vedere la versione vecchia.
    location = /index.html {
        add_header Cache-Control "no-cache" always;
        include $SNIPPET_DIR/ww-security-headers.conf;
    }

    # Stessa cosa per il codice del client: cambia ad ogni aggiornamento.
    location ~* ^/(js|css)/ {
        try_files \$uri =404;
        add_header Cache-Control "no-cache" always;
        include $SNIPPET_DIR/ww-security-headers.conf;
    }

    # Immagini e icone: cambiano di rado, cache lunga.
    location ^~ /assets/ {
        try_files \$uri =404;
        expires 7d;
        add_header Cache-Control "public, max-age=604800" always;
        include $SNIPPET_DIR/ww-security-headers.conf;
    }

    location / {
        limit_req  zone=ww_web burst=60 nodelay;
        limit_conn ww_conn 20;
        try_files \$uri \$uri/ /index.html;
        include $SNIPPET_DIR/ww-security-headers.conf;
    }
}
EOF
}

write_site_conf() {
    write_security_snippet
    cat > "$CONF_PATH" <<EOF
# Generato da setup-web.sh — non modificare a mano, viene sovrascritto ad ogni
# deploy. Serve staticamente il client web di Warrior & Wealth.

server {
    listen $HTTP_PORT;
$LISTEN6_HTTP
    server_name $SERVER_NAME;

    root $PUBLISH_DIR;
    index index.html;

    # Non rivelare la versione esatta di nginx negli errori e negli header.
    server_tokens off;
    client_max_body_size 1m;

    include $SNIPPET_DIR/ww-security-headers.conf;

    # Serve a Let's Encrypt per verificare il dominio ad ogni rinnovo.
    location ^~ /.well-known/acme-challenge/ {
        root $PUBLISH_DIR;
        default_type "text/plain";
    }
EOF

    if [ "$HAS_CERT" -eq 1 ]; then
        # Con il certificato: la porta 80 rimanda tutto su HTTPS e il sito
        # vero vive nel blocco 443.
        cat >> "$CONF_PATH" <<EOF

    location / {
        return 301 https://\$host\$request_uri;
    }
}

server {
    listen 443 ssl;
$LISTEN6_HTTPS
    server_name $SERVER_NAME;

    ssl_certificate     $CERT_DIR/fullchain.pem;
    ssl_certificate_key $CERT_DIR/privkey.pem;
    ssl_protocols       TLSv1.2 TLSv1.3;
    ssl_session_cache   shared:SSL:10m;
    ssl_session_timeout 1d;

    root $PUBLISH_DIR;
    index index.html;
    server_tokens off;
    client_max_body_size 1m;

    include $SNIPPET_DIR/ww-security-headers.conf;
EOF
    fi

    write_site_body
}

log "[NGINX] Scrittura configurazione in $CONF_PATH..."
[ -f "$CONF_PATH" ] && cp -f "$CONF_PATH" "$BACKUP_PATH"
write_site_conf
ln -sf "$CONF_PATH" "$LINK_PATH"

# Il sito di default di nginx occupa la porta 80 e mostrerebbe "Welcome to
# nginx" al posto del gioco.
if [ -e "/etc/nginx/sites-enabled/default" ]; then
    rm -f "/etc/nginx/sites-enabled/default"
    log "[NGINX] Rimosso il sito di default (era in conflitto sulla porta $HTTP_PORT)."
fi

# ── 7. Test con ripristino automatico ───────────────────────
# Se la configurazione fosse sbagliata e la lasciassimo attiva, il prossimo
# riavvio (o reload) impedirebbe a nginx di partire DEL TUTTO: non un sito
# rotto, ma il server web spento. Quindi in caso di errore si torna indietro.
log "[NGINX] Verifica configurazione..."
if ! nginx -t; then
    log "[NGINX] Configurazione non valida: ripristino la versione precedente."
    if [ -f "$BACKUP_PATH" ]; then
        cp -f "$BACKUP_PATH" "$CONF_PATH"
    else
        rm -f "$LINK_PATH" "$CONF_PATH"
    fi
    if nginx -t >/dev/null 2>&1; then systemctl reload nginx >/dev/null 2>&1; fi
    fail "Configurazione nginx non valida (vedi errori sopra). La versione precedente e' stata ripristinata: nginx non e' stato rotto."
fi

systemctl enable nginx --now || fail "Impossibile abilitare/avviare nginx"
systemctl reload nginx || fail "Reload nginx fallito"
rm -f "$BACKUP_PATH"
log "[NGINX] Configurazione applicata."

# ── 8. Certificato HTTPS (solo se hai un dominio) ───────────
if [ -n "$DOMAIN" ] && [ "$HAS_CERT" -eq 0 ]; then
    log "[HTTPS] Dominio impostato ($DOMAIN) e nessun certificato presente: lo richiedo a Let's Encrypt..."
    if ! command -v certbot &>/dev/null; then
        apt-get update "${APT_OPTS[@]}" >/dev/null 2>&1
        apt-get install "${APT_OPTS[@]}" certbot || log "[HTTPS] (Attenzione) installazione certbot fallita."
    fi
    if command -v certbot &>/dev/null; then
        if [ -n "$LETSENCRYPT_EMAIL" ]; then
            CERTBOT_MAIL=(-m "$LETSENCRYPT_EMAIL")
        else
            CERTBOT_MAIL=(--register-unsafely-without-email)
        fi
        if certbot certonly --webroot -w "$PUBLISH_DIR" -d "$DOMAIN" \
               --non-interactive --agree-tos "${CERTBOT_MAIL[@]}"; then
            log "[HTTPS] Certificato ottenuto: riscrivo la configurazione con HTTPS attivo."
            HAS_CERT=1
            cp -f "$CONF_PATH" "$BACKUP_PATH"
            write_site_conf
            if nginx -t >/dev/null 2>&1; then
                systemctl reload nginx
                rm -f "$BACKUP_PATH"
                # Il rinnovo automatico e' gia' gestito dal timer di certbot;
                # qui si aggiunge solo il reload di nginx a rinnovo avvenuto.
                mkdir -p /etc/letsencrypt/renewal-hooks/deploy
                printf '#!/bin/sh\nsystemctl reload nginx\n' > /etc/letsencrypt/renewal-hooks/deploy/ww-reload-nginx.sh
                chmod 755 /etc/letsencrypt/renewal-hooks/deploy/ww-reload-nginx.sh
                log "[HTTPS] Attivo su https://$DOMAIN/ (rinnovo automatico configurato)."
            else
                log "[HTTPS] (Attenzione) configurazione HTTPS non valida, torno su HTTP."
                HAS_CERT=0
                cp -f "$BACKUP_PATH" "$CONF_PATH"
                nginx -t >/dev/null 2>&1 && systemctl reload nginx
                rm -f "$BACKUP_PATH"
            fi
        else
            log "[HTTPS] (Attenzione) richiesta del certificato fallita. Verifica che $DOMAIN punti davvero a questa VPS e che la porta 80 sia raggiungibile dall'esterno. Il sito resta su HTTP."
        fi
    fi
fi

# ── 9. Firewall ─────────────────────────────────────────────
if command -v ufw &>/dev/null; then
    ufw allow "$HTTP_PORT"/tcp comment 'nginx HTTP (client web)' >/dev/null 2>&1
    if [ "$HAS_CERT" -eq 1 ]; then
        ufw allow 443/tcp comment 'nginx HTTPS (client web)' >/dev/null 2>&1
    fi
    log "[UFW] Porta $HTTP_PORT aperta."
fi

# ── 10. Verifica finale ─────────────────────────────────────
echo ""
echo "============================================"
echo " Verifica client web"
echo "============================================"
echo -n " nginx    : "; nginx -v 2>&1
if systemctl is-active --quiet nginx; then echo " stato    : attivo"; else echo " stato    : NON attivo (controlla 'systemctl status nginx')"; fi
HTTP_CODE="$(curl -s -o /dev/null -w '%{http_code}' --max-time 5 "http://127.0.0.1:$HTTP_PORT/" 2>/dev/null)"
case "$HTTP_CODE" in
    200|301) echo " pagina   : HTTP $HTTP_CODE (ok)" ;;
    403)     echo " pagina   : HTTP 403 (ATTENZIONE: nginx non riesce a leggere i file in $PUBLISH_DIR)" ;;
    *)       echo " pagina   : HTTP $HTTP_CODE (ATTENZIONE: atteso 200)" ;;
esac
echo ""
if [ "$HAS_CERT" -eq 1 ]; then
    echo "[OK] Client web raggiungibile su https://$DOMAIN/"
else
    echo "[OK] Client web raggiungibile su http://<IP-DELLA-VPS>:$HTTP_PORT/"
fi
echo "     WebSocket: porta 8444 come prima (client invariato),"
echo "     e da ora anche tramite nginx all'indirizzo /ws."
