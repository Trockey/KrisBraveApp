# DeveloperGoals

Inteligentna aplikacja do planowania ścieżki rozwoju programistów.

## Stack technologiczny

- **Frontend/Backend**: Blazor Web App (.NET 9) z Interactive Auto
- **Baza danych**: PostgreSQL z Entity Framework Core 9.0
- **Autentykacja**: Google OAuth
- **Wizualizacja grafów**: (do wyboru: vis.js, cytoscape.js)
- **AI Integration**: (do skonfigurowania)

## Wymagania

- .NET 9.0 SDK
- PostgreSQL 14+
- Konto Google Cloud (dla OAuth)

## Instalacja

### 1. Sklonuj repozytorium

```bash
git clone <repository-url>
cd DeveloperGoals
```

### 2. Skonfiguruj bazę danych

Utwórz bazę danych PostgreSQL:

```sql
CREATE DATABASE developergoals_dev;
```

### 3. Zaktualizuj connection string

Edytuj plik `DeveloperGoals/appsettings.Development.json` i zaktualizuj connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=developergoals_dev;Username=twoj_user;Password=twoje_haslo"
  }
}
```

### 4. Uruchom migracje Entity Framework

```bash
cd DeveloperGoals
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Skonfiguruj Google OAuth

1. Przejdź do [Google Cloud Console](https://console.cloud.google.com/)
2. Utwórz nowy projekt lub wybierz istniejący
3. Włącz Google+ API
4. Utwórz OAuth 2.0 Client ID (Web application)
5. Dodaj Authorized redirect URIs:
   - `https://localhost:7xxx/signin-google` (sprawdź port w launchSettings.json)
6. Skopiuj Client ID i Client Secret
7. Zaktualizuj `appsettings.Development.json`:

```json
{
  "Authentication": {
    "Google": {
      "ClientId": "twoj-client-id.apps.googleusercontent.com",
      "ClientSecret": "twoj-client-secret"
    }
  }
}
```

### 6. Uruchom aplikację

```bash
dotnet run
```

Aplikacja będzie dostępna pod adresem: `https://localhost:7xxx` (sprawdź port w konsoli)

## Struktura projektu

```
DeveloperGoals/
├── DeveloperGoals/              # Projekt Server
│   ├── Components/              # Komponenty Blazor
│   │   ├── Pages/              # Strony
│   │   └── Layout/             # Layout
│   ├── Data/                   # DbContext
│   ├── Models/                 # Modele danych
│   ├── Services/               # Serwisy biznesowe
│   └── Program.cs              # Konfiguracja aplikacji
│
└── DeveloperGoals.Client/       # Projekt WebAssembly
    └── Pages/                  # Komponenty WASM
```

## Modele danych

### User
- Dane użytkownika z Google OAuth
- Relacja 1:1 z UserProfile
- Relacja 1:N z Technology

### UserProfile
- Główne technologie
- Rola (Programista, Tester, Analityk, Data Science Specialist)
- Obszar rozwoju (UI, Backend, Full Stack, Testing, DevOps, Data Science)

### Technology
- Nazwa z prefiksem (np. "DotNet - Entity Framework")
- Kategoria (DotNet, Java, JavaScript, Python, etc.)
- Tag (Technologia, Framework, Baza danych, Metodologia)
- Opis systemowy (z AI)
- Opis prywatny (użytkownika)
- Status (Active, Ignored)
- Procent postępu (0-100%)

### TechnologyDependency
- Reprezentuje krawędzie w grafie technologii
- FromTechnologyId → ToTechnologyId
- Każdy użytkownik ma własny graf

## Następne kroki

- [ ] Implementacja Google OAuth autentykacji
- [ ] Utworzenie layoutu i nawigacji
- [ ] Implementacja strony profilu użytkownika
- [ ] Integracja z biblioteką do wizualizacji grafów
- [ ] Implementacja serwisu AI do generowania rekomendacji
- [ ] Utworzenie komponentów grafu technologii
- [ ] Implementacja popupów (Opcje, Szukaj technologii)

## Dokumentacja

Pełna dokumentacja wymagań znajduje się w pliku `PRD.md` w katalogu głównym projektu.

## Licencja

[Do uzupełnienia]

