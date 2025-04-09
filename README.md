# ChessGames

## Projekta mērķis
Šī projekta mērķis ir izveidot konsoles spēli, kurā lietotāji var spēlēt šahu un dambreti pret citu spēlētāju. Projekts piedāvā arī grafisko lietotāja interfeisu šaha spēlei, kā arī iespēju pārvaldīt spēlētājus un ielādēt saglabātas spēles.

## Funkcionalitāte
- **Šaha un dambretes spēle**: Lietotāji var izvēlēties starp šahu un dambreti.
- **Grafiskais interfeiss šaham**: Šaha spēlei ir pieejams grafiskais interfeiss ar pielāgotām šaha figūrām un lauciņiem.
- **Spēlētāju pārvaldība**: Iespēja pievienot jaunus spēlētājus un apskatīt esošos.
- **Saglabāto spēļu ielāde**: Lietotāji var ielādēt iepriekš saglabātas spēles no JSON failiem.
- **Galvenā izvēlne**: Lietotāji var izvēlēties starp jaunas spēles sākšanu, saglabātas spēles ielādi, spēlētāju pārvaldību vai iziešanu no programmas.

## Lietotāja interfeiss
- **Šaha dēlis**: 
  - Gaiši un tumši lauciņi (mahagonija un sarkanā koka dizains).
  - Figūras izgatavotas no gaiša bērza un tumša ozola.
  - Pelēki punkti norāda iespējamos gājienus izvēlētajai figūrai.
  - Karalis kļūst sarkans, ja tas ir apdraudēts, un apgāžas, ja tiek zaudēts.
- **Taimeris**: Iespēja iestatīt spēles laika ierobežojumu.

## Projekta struktūra
ChessGames/ \
├── ConsoleApp1/ \
│ ├── Games/ \
│ │ ├── CheckersLogic.cs \
│ │ └── ChessLogic.cs \
│ ├── Menus/ \
│ │ ├── NewGame.cs \
│ │ └── Players.cs \
│ ├── UI/ \
│ │ ├── ChessBoardForm.cs \
│ │ └── Image/ \
│ │ ├── white.png \
│ │ └── black.png \
│ ├── Program.cs \
│ └── ConsoleApp1.csproj \
├── .vscode/ \
│ ├── launch.json \
│ ├── settings.json \
│ └── tasks.json \
├── README.md \
└── .gitignore \

## Kā sākt
1. **Nepieciešamās prasības**:
   - .NET 6.0 SDK vai jaunāka versija.
   - Windows operētājsistēma (WinForms atbalstam).

2. **Projekta palaišana**:
   - Atveriet projektu Visual Studio vai jebkurā citā IDE, kas atbalsta .NET.
   - Izpildiet komandu `dotnet build`, lai izveidotu projektu.
   - Izpildiet komandu `dotnet run`, lai palaistu programmu.

3. **Failu struktūra**:
   - Grafiskā interfeisa attēli atrodas mapē `UI/Image/`.
   - Spēles loģika ir mapē `Games/`.

## Veidotāji
- Roberts Dāvidsons 
- Šarlote Tērmane

## Licences informācija
Šis projekts ir izstrādāts mācību nolūkiem un nav paredzēts komerciālai lietošanai.