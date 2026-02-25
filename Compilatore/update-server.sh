#!/bin/bash
# update-server.sh

BASE_DIR="/opt/warriorandwealth/Warrior-and-Wealth"
SERVER_DIR="$BASE_DIR/Server Strategico"
SCREEN_NAME="warriorandwealth"

echo "🔄 Aggiorno repository..."
cd "$BASE_DIR" || { echo "Directory base non trovata"; exit 1; }
git pull origin main || { echo "Errore git pull"; exit 1; }

echo "🛠 Compilo il server..."
cd "$SERVER_DIR" || { echo "Directory server non trovata"; exit 1; }
dotnet publish -c Release --self-contained false || { echo "Errore dotnet publish"; exit 1; }

echo "🛑 Chiudo eventuale screen precedente..."
screen -S "$SCREEN_NAME" -X quit 2>/dev/null

echo "🚀 Avvio il server in screen..."
screen -dmS "$SCREEN_NAME" bash -c "cd \"$SERVER_DIR\" && dotnet \"bin/Release/net8.0/publish/Server Strategico.dll\""

echo "✅ Server avviato!"
echo "Per rientrare: screen -r $SCREEN_NAME"