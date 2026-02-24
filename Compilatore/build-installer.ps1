# build-installer.ps1
# =================================================
# Script per pubblicare il client e compilare l'installer Inno Setup
# =================================================

# 1️⃣ Impostazioni percorsi
$projectPath = "C:\Users\TheCh\source\repos\Online-Cripto-Game\CriptoGame_Online\Warrior_and_Wealth.csproj"
$publishPath = "C:\Users\TheCh\source\repos\Online-Cripto-Game\CriptoGame_Online\bin\Release\net9.0-windows8.0\win-x64\publish"
$innoScript = "C:\Users\TheCh\Desktop\Compilatore\WarriorAndWealth.iss"
$isccPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
$exePath = "C:\Users\TheCh\source\repos\Online-Cripto-Game\CriptoGame_Online\bin\Release\net9.0-windows8.0\win-x64\publish\Warrior_and_Wealth.exe"

# Pulizia cartella
if (Test-Path $publishPath) {
    Remove-Item $publishPath -Recurse -Force
}

# 2️⃣ Esegui dotnet publish
Write-Host "Pubblico il client .NET..."
$publishProcess = Start-Process "dotnet" -ArgumentList "publish `"$projectPath`" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true" -NoNewWindow -PassThru -Wait

if ($publishProcess.ExitCode -ne 0) {
    Write-Host "Errore durante il publish. ExitCode: $($publishProcess.ExitCode)"
    exit $publishProcess.ExitCode
}

Write-Host "Publish completato."

# Legge la versione del file exe
$version = (Get-Item $exePath).VersionInfo.FileVersion

# Stampa la versione
Write-Host "Versione letta dal file exe: $version"

# Aggiorna il .iss
(Get-Content $innoScript) -replace '^AppVersion=.*', "AppVersion=$version" | Set-Content $innoScript

# 3️⃣ Compila lo script Inno Setup
Write-Host "Compilo l'installer con Inno Setup..."
$isccProcess = Start-Process "$isccPath" -ArgumentList "`"$innoScript`"" -NoNewWindow -PassThru -Wait

if ($isccProcess.ExitCode -ne 0) {
    Write-Host "Errore durante la compilazione dell'installer. ExitCode: $($isccProcess.ExitCode)"
    exit $isccProcess.ExitCode
}

Write-Host "Installer creato con successo!"