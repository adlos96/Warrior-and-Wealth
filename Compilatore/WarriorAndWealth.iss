[Setup]
AppId={{8F5C2C6A-9A6E-4F3B-9D2A-123456789ABC}
AppName=Warrior and Wealth
AppVersion=0.1.12.0
//Cartella Installazione
DefaultDirName={pf}\Warrior and Wealth
DefaultGroupName=Warrior and Wealth
OutputBaseFilename=Warrior_&_Wealth_installer
UninstallDisplayIcon={app}\Warrior_and_Wealth.exe

//Compressione 
Compression=lzma2/ultra64 
SolidCompression=yes 
LZMANumBlockThreads=4 
LZMADictionarySize=65536

//Architettura
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

WizardStyle=modern
AppComments=Benvenuto in Warrior and Wealth!

[Files]
Source: "C:\Users\TheCh\source\repos\Online-Cripto-Game\CriptoGame_Online\bin\Release\net9.0-windows8.0\win-x64\publish\Warrior_and_Wealth.exe"; DestDir: "{app}"
Source: "C:\Users\TheCh\source\repos\Online-Cripto-Game\CriptoGame_Online\bin\Release\net9.0-windows8.0\win-x64\publish\Assets\*"; DestDir: "{app}\Assets"; Flags: recursesubdirs ignoreversion

[Code]
function IsDotNetInstalled(): Boolean;
begin
  Result := RegKeyExists(HKLM, 'SOFTWARE\dotnet\Setup\InstalledVersions\x64\sharedfx\Microsoft.NETCore.App\9.0.0');
end;

[Icons]
Name: "{group}\Warrior and Wealth"; Filename: "{app}\Warrior_and_Wealth.exe"
Name: "{commondesktop}\Warrior and Wealth"; Filename: "{app}\Warrior_and_Wealth.exe"

[Run]
Filename: "{app}\Warrior_and_Wealth.exe"; Description: "Avvia il gioco"; Flags: postinstall
