# ChessGames

## Projekta apraksts
ChessGames ir C#/.NET 6.0 projekts, kas ļauj spēlēt šahu un dambreti ar grafisko (WinForms) interfeisu. Lietotāji var pārvaldīt spēlētājus, saglabāt un ielādēt spēles, kā arī sekot līdzi uzvarām.

## Funkcionalitāte
- **Šahs un dambrete**: Spēlē pret citu spēlētāju.
- **Grafiskais interfeiss šaham un dambretei**: WinForms logs ar vizuālu dēli, figūrām un taimeriem.
- **Spēlētāju pārvaldība**: Pievieno jaunus spēlētājus, skati uzvaras.
- **Saglabāšana un ielāde**: Spēles stāvokli var saglabāt un atjaunot vēlāk.
- **Galvenā izvēlne**: Jauna spēle, ielādēt spēli, spēlētāju pārvaldība, iziešana.

## Projekta struktūra
```
ChessGames/
├── ConsoleApp1/
│   ├── Games/
│   │   ├── ChessLogic.cs
│   │   └── CheckersLogic.cs
│   ├── Menus/
│   │   ├── GameOptions.cs
│   │   ├── LoadGameMenu.cs
│   │   ├── Players.cs
│   │   └── Players/
│   │       └── PlayerCreate.cs
│   ├── Saving/
│   │   ├── GameSaving.cs
│   │   ├── PlayerSaving.cs
│   │   └── Saves/
│   ├── UI/
│   │   ├── GameBoardForm.cs
│   │   └── Image/
│   ├── Program.cs
│   └── ConsoleApp1.csproj
├── .vscode/
│   ├── launch.json
│   ├── settings.json
│   └── tasks.json
├── README.md
└── .gitignore
```

## Uzsākšana
1. **Prasības**:
   - .NET 6.0 SDK vai jaunāka versija
   - Windows (WinForms atbalstam)

2. **Palaišana**:
   - Atveriet projektu Visual Studio vai VS Code.
   - Kompilējiet:  
     `dotnet build ChessGames/ConsoleApp1/ConsoleApp1.csproj`
   - Palaidiet:  
     `dotnet run --project ChessGames/ConsoleApp1/ConsoleApp1.csproj`

3. **Failu struktūra**:
   - Spēles loģika: `Games/`
   - Saglabāšana: `Saving/`
   - Grafiskie attēli: `UI/Image/`

## Lietotāja interfeiss
- **Dēlis**: Gaiši/tumši lauciņi, pielāgotas figūras.
- **Taimeris**: Katram spēlētājam atsevišķs laiks.
- **Uzvaras**: Uzvarētājs tiek reģistrēts JSON failā.

## Autori
- Roberts Dāvidsons
- Šarlote Tērmane

## Licence
Projekts paredzēts mācību nolūkiem.