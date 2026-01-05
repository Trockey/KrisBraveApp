# Instrukcja: Jak uzyskać wartość cookie do testów API

## Metoda 1: Chrome / Edge (DevTools)

1. **Uruchom aplikację:**
   ```bash
   dotnet run --project DeveloperGoals/DeveloperGoals/DeveloperGoals.csproj
   ```

2. **Otwórz przeglądarkę i przejdź do:**
   ```
   http://localhost:5057
   ```

3. **Zaloguj się przez Google OAuth:**
   - Kliknij przycisk logowania
   - Zaloguj się przez Google
   - Po zalogowaniu zostaniesz przekierowany na stronę główną

4. **Otwórz DevTools:**
   - Naciśnij `F12` lub `Ctrl + Shift + I`
   - Przejdź do zakładki **Application** (Chrome) lub **Storage** (Edge)

5. **Znajdź cookie:**
   - W lewym panelu rozwiń **Cookies**
   - Kliknij na `http://localhost:5057`
   - Znajdź cookie o nazwie `.AspNetCore.Cookies` lub `.AspNetCore.Identity.Application`
   - Skopiuj wartość z kolumny **Value**

6. **Użyj wartości w skrypcie:**
   ```bash
   COOKIE_HEADER="Cookie: .AspNetCore.Cookies=SKOPIOWANA_WARTOŚĆ_TUTAJ"
   ```

## Metoda 2: Firefox (DevTools)

1. Wykonaj kroki 1-3 z metody 1

2. **Otwórz DevTools:**
   - Naciśnij `F12`
   - Przejdź do zakładki **Storage**

3. **Znajdź cookie:**
   - Rozwiń **Cookies** → `http://localhost:5057`
   - Znajdź cookie `.AspNetCore.Cookies`
   - Skopiuj wartość

## Metoda 3: PowerShell - Automatyczne pobranie cookie

Użyj skryptu PowerShell `Get-Cookie.ps1` (zobacz poniżej), który automatycznie pobierze cookie z przeglądarki Chrome.

## Metoda 4: Postman - Interceptor / Browser Extension

1. **Zainstaluj Postman Interceptor** (rozszerzenie przeglądarki)
2. **Włącz Interceptor w Postmanie:**
   - Settings → Interceptor → Enable
3. **Zaloguj się przez przeglądarkę** (z włączonym Interceptorem)
4. **Cookie zostanie automatycznie przechwycone** w Postmanie

## Metoda 5: curl z zapisaniem cookies

Możesz użyć curl do zalogowania i zapisania cookies:

```bash
# 1. Zaloguj się i zapisz cookies do pliku
curl -c cookies.txt -L "http://localhost:5057/login/google"

# 2. Użyj zapisanych cookies w requestach
curl -b cookies.txt -X POST "http://localhost:5057/api/ai/recommendations" \
  -H "Content-Type: application/json" \
  -d '{"fromTechnologyId": 1}'
```

**UWAGA:** Ta metoda może nie działać z OAuth, ponieważ wymaga interakcji użytkownika.

## Ważne informacje

- **Cookie wygasa:** Cookie ma określony czas życia (zwykle kilka godzin/dni)
- **Cookie jest specyficzne dla sesji:** Każde nowe logowanie tworzy nowy cookie
- **Cookie jest HttpOnly:** Nie można go odczytać przez JavaScript (tylko przez DevTools)
- **Cookie jest związane z domeną:** Cookie z `localhost:5057` nie zadziała z innymi portami

## Rozwiązywanie problemów

### Problem: Nie widzę cookie `.AspNetCore.Cookies`
**Rozwiązanie:** 
- Sprawdź czy jesteś zalogowany (odśwież stronę)
- Sprawdź czy aplikacja działa na właściwym porcie
- Wyczyść cookies i zaloguj się ponownie

### Problem: Cookie nie działa w curl/Postman
**Rozwiązanie:**
- Upewnij się, że skopiowałeś całą wartość cookie (może być bardzo długa)
- Sprawdź czy używasz właściwego formatu: `Cookie: .AspNetCore.Cookies=wartość`
- Sprawdź czy aplikacja działa i czy port jest poprawny

### Problem: Otrzymuję 401 Unauthorized
**Rozwiązanie:**
- Cookie wygasło - zaloguj się ponownie i skopiuj nowy cookie
- Sprawdź czy cookie jest w formacie: `Cookie: .AspNetCore.Cookies=wartość`
- Sprawdź czy aplikacja ma skonfigurowaną autoryzację
