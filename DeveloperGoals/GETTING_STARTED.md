# Jak rozpocząć pracę z projektem DeveloperGoals

## ✅ Co zostało zrobione

### 1. Utworzenie projektu Blazor Web App
- ✅ Projekt z trybem **Interactive Auto** (.NET 9)
- ✅ Struktura Server + Client (WebAssembly)
- ✅ Bootstrap 5 dla stylowania

### 2. Modele danych
- ✅ `User` - dane użytkownika z Google OAuth
- ✅ `UserProfile` - profil z technologiami, rolą i obszarem rozwoju
- ✅ `Technology` - technologie w grafie użytkownika
- ✅ `TechnologyDependency` - zależności między technologiami (krawędzie grafu)

### 3. Entity Framework Core
- ✅ `ApplicationDbContext` z konfiguracją relacji
- ✅ PostgreSQL jako baza danych
- ✅ Connection strings w `appsettings.json`

### 4. Google OAuth Autentykacja
- ✅ Konfiguracja Google Authentication
- ✅ `AuthController` z endpointami login/logout
- ✅ Strona logowania (`/login`)
- ✅ Komponent `LoginDisplay` w nawigacji
- ✅ Ochrona stron przez `AuthorizeRouteView`

### 5. Layout i nawigacja
- ✅ Zaktualizowane menu nawigacji
- ✅ Strona główna z grafem technologii (placeholder)
- ✅ Strona profilu użytkownika (formularz)
- ✅ Strona ignorowanych technologii

### 6. Konfiguracja projektu
- ✅ `.editorconfig` ze standardami kodowania
- ✅ README.md z dokumentacją
- ✅ Struktura folderów (Data, Models, Services, Controllers)

## 🚀 Następne kroki - Uruchomienie projektu

### Krok 1: Instalacja PostgreSQL

Jeśli nie masz PostgreSQL, zainstaluj go:
- Windows: https://www.postgresql.org/download/windows/
- Lub użyj Docker:
```bash
docker run --name postgres-dev -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres:14
```

### Krok 2: Utworzenie bazy danych

```sql
CREATE DATABASE developergoals_dev;
```

### Krok 3: Aktualizacja connection string

Edytuj `DeveloperGoals/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=developergoals_dev;Username=postgres;Password=twoje_haslo"
  }
}
```

### Krok 4: Uruchomienie migracji Entity Framework

```bash
cd DeveloperGoals/DeveloperGoals
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Uwaga:** Jeśli nie masz narzędzi EF, zainstaluj je:
```bash
dotnet tool install --global dotnet-ef
```

### Krok 5: Konfiguracja Google OAuth

1. Przejdź do [Google Cloud Console](https://console.cloud.google.com/)
2. Utwórz nowy projekt lub wybierz istniejący
3. Włącz **Google+ API**
4. Przejdź do **Credentials** → **Create Credentials** → **OAuth 2.0 Client ID**
5. Typ aplikacji: **Web application**
6. Dodaj **Authorized redirect URIs**:
   ```
   https://localhost:7xxx/signin-google
   ```
   (Sprawdź dokładny port w `Properties/launchSettings.json`)

7. Skopiuj **Client ID** i **Client Secret**

8. Zaktualizuj `appsettings.Development.json`:
```json
{
  "Authentication": {
    "Google": {
      "ClientId": "123456789-abcdefg.apps.googleusercontent.com",
      "ClientSecret": "GOCSPX-twoj_secret"
    }
  }
}
```

### Krok 6: Uruchomienie aplikacji

```bash
cd DeveloperGoals/DeveloperGoals
dotnet run
```

Aplikacja będzie dostępna pod adresem wyświetlonym w konsoli, np.:
```
https://localhost:7123
```

## 📋 Co dalej - Funkcjonalności do implementacji

### Priorytet 1: Podstawowa funkcjonalność
- [ ] Serwis do zarządzania użytkownikami (zapisywanie po logowaniu)
- [ ] Serwis do zarządzania profilem użytkownika
- [ ] Zapisywanie i odczyt profilu z bazy danych
- [ ] Walidacja formularza profilu

### Priorytet 2: Graf technologii
- [ ] Integracja z biblioteką do wizualizacji grafów (vis.js lub cytoscape.js)
- [ ] Wyświetlanie węzła "Start" dla nowego użytkownika
- [ ] Dodawanie technologii do grafu
- [ ] Wyświetlanie zależności między technologiami
- [ ] Interakcja z węzłami (kliknięcie → popup)

### Priorytet 3: Integracja z AI
- [ ] Wybór i konfiguracja API AI (OpenAI, Anthropic, Azure OpenAI)
- [ ] Serwis do generowania rekomendacji
- [ ] Popup z rekomendacjami AI
- [ ] Obsługa timeout i błędów AI
- [ ] Zapisywanie reasoning z AI

### Priorytet 4: Zarządzanie technologiami
- [ ] Popup "Opcje" - edycja opisu, postępu, usuwanie
- [ ] Popup "Szukaj nowych technologii" - lista rekomendacji
- [ ] Dodawanie technologii do listy "Ignore"
- [ ] Przywracanie technologii z listy "Ignore"
- [ ] Dodawanie własnych niestandardowych technologii

### Priorytet 5: Dodatkowe funkcjonalności
- [ ] Admin panel (opcjonalny)
- [ ] Metryki i analytics
- [ ] Caching rekomendacji AI
- [ ] Testy jednostkowe
- [ ] Testy integracyjne

## 🛠️ Przydatne komendy

### Entity Framework
```bash
# Dodanie nowej migracji
dotnet ef migrations add NazwaMigracji

# Aktualizacja bazy danych
dotnet ef database update

# Usunięcie ostatniej migracji
dotnet ef migrations remove

# Wygenerowanie skryptu SQL
dotnet ef migrations script
```

### Budowanie i uruchamianie
```bash
# Uruchomienie w trybie development
dotnet run

# Uruchomienie z hot reload
dotnet watch run

# Budowanie projektu
dotnet build

# Publikacja
dotnet publish -c Release
```

### Przywracanie pakietów
```bash
dotnet restore
```

## 📚 Dokumentacja

- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Google OAuth Setup](https://developers.google.com/identity/protocols/oauth2)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)

## ❓ Rozwiązywanie problemów

### Problem: "Cannot find path DeveloperGoals/DeveloperGoals"
**Rozwiązanie:** Upewnij się, że jesteś w odpowiednim katalogu:
```bash
cd D:\Projects_prv_test\AI\KursBrave\KrisBraveApp\DeveloperGoals\DeveloperGoals
```

### Problem: Błąd połączenia z bazą danych
**Rozwiązanie:** 
1. Sprawdź czy PostgreSQL działa
2. Zweryfikuj connection string
3. Upewnij się, że baza danych została utworzona

### Problem: Google OAuth nie działa
**Rozwiązanie:**
1. Sprawdź czy ClientId i ClientSecret są poprawne
2. Zweryfikuj redirect URI w Google Cloud Console
3. Upewnij się, że używasz HTTPS (wymagane przez Google)

### Problem: Błędy kompilacji
**Rozwiązanie:**
```bash
dotnet clean
dotnet restore
dotnet build
```

## 🎯 Struktura projektu

```
DeveloperGoals/
├── DeveloperGoals/              # Projekt Server
│   ├── Components/
│   │   ├── Layout/             # Layout i nawigacja
│   │   ├── Pages/              # Strony Razor
│   │   └── RedirectToLogin.razor
│   ├── Controllers/            # Kontrolery API
│   │   └── AuthController.cs
│   ├── Data/                   # DbContext
│   │   └── ApplicationDbContext.cs
│   ├── Models/                 # Modele danych
│   │   ├── User.cs
│   │   ├── UserProfile.cs
│   │   ├── Technology.cs
│   │   └── TechnologyDependency.cs
│   ├── Services/               # Serwisy (do implementacji)
│   ├── Program.cs              # Konfiguracja aplikacji
│   └── appsettings.json
│
├── DeveloperGoals.Client/       # Projekt WebAssembly
│   └── Pages/
│       └── Counter.razor
│
├── .editorconfig               # Standardy kodowania
└── README.md                   # Dokumentacja
```

## 💡 Wskazówki

1. **Rozpocznij od migracji** - upewnij się, że baza danych działa
2. **Skonfiguruj Google OAuth** - bez tego nie będziesz mógł się zalogować
3. **Testuj na bieżąco** - uruchamiaj aplikację po każdej większej zmianie
4. **Używaj hot reload** - `dotnet watch run` przyspiesza rozwój
5. **Czytaj PRD** - plik `.ai/prd.md` zawiera pełną specyfikację

Powodzenia w dalszym rozwoju aplikacji! 🚀

