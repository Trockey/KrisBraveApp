# 🚀 Quick Start Guide - DeveloperGoals Frontend

## ⚡ Szybkie uruchomienie (5 minut)

### 1. Wymagania wstępne
- [ ] .NET 9.0 SDK zainstalowany
- [ ] PostgreSQL działa
- [ ] Node.js i npm zainstalowane

### 2. Konfiguracja
```bash
# 1. Sklonuj repozytorium (jeśli jeszcze nie)
cd d:\Projects_prv_test\AI\KursBrave\KrisBraveApp

# 2. Zainstaluj zależności npm
cd DeveloperGoals\DeveloperGoals
npm install

# 3. Kompiluj Tailwind CSS
npm run build:css

# 4. Wróć do głównego katalogu
cd ..\..
```

### 3. Baza danych
```bash
cd DeveloperGoals\DeveloperGoals

# Zastosuj migracje
dotnet ef database update

cd ..\..
```

### 4. Uruchomienie
```bash
cd DeveloperGoals\DeveloperGoals

# Uruchom aplikację
dotnet run

# Lub z hot reload
dotnet watch run
```

### 5. Otwórz przeglądarkę
```
https://localhost:5001
```

---

## ✅ Checklist pierwszego uruchomienia

### Przed uruchomieniem
- [ ] PostgreSQL działa na localhost
- [ ] Connection string w `appsettings.json` jest poprawny
- [ ] Google OAuth credentials skonfigurowane
- [ ] OpenRouter API key skonfigurowany (dla AI)
- [ ] Tailwind CSS skompilowany (`npm run build:css`)

### Po uruchomieniu
- [ ] Strona główna ładuje się bez błędów
- [ ] Przekierowanie na `/login` działa
- [ ] Login przez Google działa
- [ ] Przekierowanie na `/onboarding` działa
- [ ] Formularz onboardingu się wyświetla

### Testowanie funkcjonalności
- [ ] **Onboarding:** Możesz wybrać technologie i utworzyć profil
- [ ] **Dashboard:** Graf się wyświetla po utworzeniu profilu
- [ ] **Węzeł START:** Jest widoczny na grafie
- [ ] **Kliknięcie węzła:** Otwiera panel informacyjny
- [ ] **NodeOptionsModal:** Otwiera się po kliknięciu "Zarządzaj"
- [ ] **RecommendationModal:** Otwiera się po kliknięciu "Szukaj nowych technologii"
- [ ] **AI Rekomendacje:** Generują się (do 20s)
- [ ] **Dodawanie technologii:** Nowe węzły pojawiają się na grafie
- [ ] **Aktualizacja postępu:** Kolor węzła zmienia się po zmianie postępu
- [ ] **Usuwanie węzła:** Węzeł znika z grafu
- [ ] **Ignorowanie:** Technologie trafiają do listy ignorowanych

---

## 🐛 Troubleshooting

### Problem: "vis is not defined"
**Rozwiązanie:** Sprawdź czy vis.js jest załadowany w `App.razor`:
```html
<script type="text/javascript" src="https://unpkg.com/vis-network@9.1.9/standalone/umd/vis-network.min.js"></script>
```

### Problem: "Nie można pobrać grafu"
**Rozwiązanie:** 
1. Sprawdź czy backend controller `TechnologyController` istnieje
2. Sprawdź czy endpoint `/api/technologies/graph` zwraca dane
3. Sprawdź logi aplikacji

### Problem: CSS nie działa
**Rozwiązanie:**
```bash
cd DeveloperGoals\DeveloperGoals
npm run build:css
```

### Problem: "Timeout podczas AI"
**Rozwiązanie:**
1. Sprawdź OpenRouter API key w `appsettings.json`
2. Sprawdź połączenie internetowe
3. Zwiększ timeout w konfiguracji (domyślnie 20s)

### Problem: "Nie można zalogować przez Google"
**Rozwiązanie:**
1. Sprawdź Google OAuth credentials w `appsettings.json`
2. Upewnij się że redirect URI jest poprawny w Google Console
3. Sprawdź czy HTTPS działa

---

## 📝 Szybkie polecenia

### Development
```bash
# Hot reload (automatyczne przeładowanie)
dotnet watch run

# Watch CSS (automatyczna kompilacja)
npm run watch:css

# Oba jednocześnie (2 terminale)
# Terminal 1:
dotnet watch run

# Terminal 2:
npm run watch:css
```

### Database
```bash
# Nowa migracja
dotnet ef migrations add MigrationName

# Zastosuj migracje
dotnet ef database update

# Rollback do poprzedniej migracji
dotnet ef database update PreviousMigrationName

# Usuń bazę danych (UWAGA!)
dotnet ef database drop
```

### Build
```bash
# Build release
dotnet build -c Release

# Publish
dotnet publish -c Release -o ./publish
```

---

## 🔍 Gdzie szukać logów

### Console
- Wszystkie logi wyświetlają się w konsoli podczas `dotnet run`
- Logi JavaScript w konsoli przeglądarki (F12)

### Blazor
- Błędy renderowania: Konsola przeglądarki
- Błędy JS Interop: Konsola przeglądarki
- Błędy serwisów: Konsola aplikacji

### Typowe logi:
```
# OK
info: DeveloperGoals.Components.Pages.Dashboard[0]
      Załadowano graf: 5 węzłów, 4 krawędzi

# Błąd
fail: DeveloperGoals.Services.TechnologyService[0]
      Błąd podczas pobierania grafu technologii
```

---

## 📚 Przydatne komendy testowe

### Test API ręcznie (Postman/curl)
```bash
# Pobierz graf
curl -X GET https://localhost:5001/api/technologies/graph \
  -H "Cookie: .AspNetCore.Cookies=..." \
  -H "Accept: application/json"

# Pobierz profil
curl -X GET "https://localhost:5001/api/profile?email=test@example.com" \
  -H "Cookie: .AspNetCore.Cookies=..." \
  -H "Accept: application/json"
```

### Test graf wizualizacji (Console przeglądarki)
```javascript
// Sprawdź czy vis.js jest załadowany
console.log(typeof vis !== 'undefined' ? 'vis.js loaded' : 'vis.js NOT loaded');

// Sprawdź instancje grafów
// (tylko jeśli graf-visualizer.js eksportuje graphInstances)
```

---

## ⚙️ Konfiguracja dla developmentu

### appsettings.Development.json (przykład)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "DeveloperGoals": "Debug"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=developergoals_dev;Username=postgres;Password=postgres"
  },
  "Profile": {
    "MinTechnologies": 2
  },
  "OpenRouter": {
    "Timeout": 30
  }
}
```

**Uwaga:** Zmniejsz `MinTechnologies` na 2 dla szybszego testowania!

---

## 🎨 Live Development Tips

### 1. Dwa terminale jednocześnie
- **Terminal 1:** `dotnet watch run` (hot reload C#)
- **Terminal 2:** `npm run watch:css` (hot reload CSS)

### 2. Browser DevTools
- **F12** → Console: logi JavaScript
- **F12** → Network: requesty HTTP
- **F12** → Application → Cookies: sprawdź auth cookie

### 3. Blazor DevTools
- Zainstaluj: [Blazor DevTools Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.blazor-devtools)

### 4. Hot Reload
- `dotnet watch` automatycznie przeładowuje przy zmianach .cs/.razor
- Dla CSS musisz odświeżyć stronę (Ctrl+R)

---

## 🚦 Status checklist

Po uruchomieniu sprawdź:

### Routing
- [ ] `/` → przekierowuje na `/login` lub `/dashboard`
- [ ] `/login` → strona logowania
- [ ] `/onboarding` → formularz dla nowych użytkowników
- [ ] `/dashboard` → graf technologii
- [ ] `/profile` → edycja profilu

### UI Components
- [ ] NavMenu wyświetla się poprawnie
- [ ] Graf jest responsywny
- [ ] Modale otwierają się i zamykają
- [ ] Buttony są klikalne
- [ ] Formularze działają

### API Integration
- [ ] Pobieranie grafu działa
- [ ] Dodawanie technologii działa
- [ ] AI rekomendacje działają
- [ ] Ignorowanie technologii działa
- [ ] Aktualizacja węzła działa
- [ ] Usuwanie węzła działa

---

## 📞 Wsparcie

Jeśli napotkasz problemy:

1. **Sprawdź logi** w konsoli aplikacji i przeglądarki
2. **Sprawdź dokumentację** w `FRONTEND_IMPLEMENTATION_SUMMARY.md`
3. **Sprawdź plan implementacji** w `.ai/23-all-view-implementation-plan.md`
4. **Sprawdź kontrolery backend** - czy są zaimplementowane zgodnie z API

---

**Powodzenia! 🎉**
