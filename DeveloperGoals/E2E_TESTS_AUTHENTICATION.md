# Autentykacja w testach E2E

Dokumentacja mechanizmu logowania testowego dla testów End-to-End.

## 🎯 Przegląd

Testy E2E używają specjalnego mechanizmu logowania testowego zamiast OAuth Google. Pozwala to na uruchamianie testów bez konieczności konfiguracji prawdziwego konta Google lub mockowania OAuth.

## 🔑 Endpoint testowego logowania

### URL
```
/login/test?test=true
```

### Jak działa?

1. Sprawdza parametr URL `test=true`
2. Szuka w bazie użytkownika o Id=2
3. Jeśli użytkownik nie istnieje, tworzy go z następującymi danymi:
   - **Id**: 2
   - **GoogleId**: "108226413010999999999"
   - **Email**: "tester@test.com"
   - **Name**: "Test"
4. Tworzy sesję cookie dla użytkownika
5. Przekierowuje do strony głównej (`/`)

### Uwaga o bezpieczeństwie

**⚠️ Ten endpoint powinien być dostępny tylko w środowisku deweloperskim i testowym!**

W produkcji należy:
- Wyłączyć ten endpoint
- Lub dodać dodatkowe zabezpieczenia (np. sprawdzanie środowiska)

## 🛠️ TestAuthHelper

Klasa pomocnicza do zarządzania autentykacją w testach E2E.

### Lokalizacja
```
DeveloperGoals.E2ETests/Helpers/TestAuthHelper.cs
```

### Metody

#### LoginAsTestUserAsync
Loguje użytkownika testowego (Id=2).

```csharp
await TestAuthHelper.LoginAsTestUserAsync(Page, baseUrl);
```

**Parametry:**
- `page` - strona Playwright
- `baseUrl` - bazowy URL aplikacji (np. "https://localhost:5001")

**Działanie:**
1. Nawiguje do `/login/test?test=true`
2. Czeka na przekierowanie do strony głównej
3. Czeka dodatkową sekundę na załadowanie sesji

#### LogoutAsync
Wylogowuje użytkownika.

```csharp
await TestAuthHelper.LogoutAsync(Page, baseUrl);
```

**Parametry:**
- `page` - strona Playwright
- `baseUrl` - bazowy URL aplikacji

**Działanie:**
1. Nawiguje do `/login/logout`
2. Czeka na przekierowanie do strony logowania

#### IsUserLoggedInAsync
Sprawdza czy użytkownik jest zalogowany.

```csharp
bool isLoggedIn = await TestAuthHelper.IsUserLoggedInAsync(Page);
```

**Zwraca:**
- `true` - jeśli znaleziono element wylogowania (użytkownik zalogowany)
- `false` - jeśli nie znaleziono elementu (użytkownik wylogowany)

## 📝 Przykłady użycia

### Przykład 1: Prosty test z logowaniem

```csharp
[Test]
public async Task Dashboard_WhenLoggedIn_ShouldLoadSuccessfully()
{
    // Arrange
    await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);
    var dashboardPage = new DashboardPage(Page, _baseUrl);

    // Act
    await dashboardPage.NavigateAsync();

    // Assert
    await Expect(dashboardPage.GetPageTitle()).ToBeVisibleAsync();
}
```

### Przykład 2: Test logowania i wylogowania

```csharp
[Test]
public async Task Logout_ShouldLogoutUserSuccessfully()
{
    // Arrange - zaloguj użytkownika
    await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);
    
    var isLoggedInBefore = await TestAuthHelper.IsUserLoggedInAsync(Page);
    Assert.That(isLoggedInBefore, Is.True);

    // Act - wyloguj użytkownika
    await TestAuthHelper.LogoutAsync(Page, _baseUrl);

    // Assert - sprawdź czy użytkownik jest wylogowany
    var isLoggedInAfter = await TestAuthHelper.IsUserLoggedInAsync(Page);
    Assert.That(isLoggedInAfter, Is.False);
}
```

### Przykład 3: Test dostępu do chronionej strony

```csharp
[Test]
public async Task ProtectedPage_WhenLoggedIn_ShouldAllowAccess()
{
    // Arrange
    await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);

    // Act
    await Page.GotoAsync($"{_baseUrl}/dashboard");

    // Assert - brak przekierowania do /login
    await Expect(Page).ToHaveURLAsync(new Regex(".*/dashboard.*"));
}
```

## 🧪 Dostępne testy autentykacji

Projekt zawiera dedykowany plik testowy `AuthenticationTests.cs` z testami:

1. **TestLogin_ShouldLoginUserSuccessfully**
   - Sprawdza czy logowanie testowe działa poprawnie

2. **TestLogin_WhenAccessingProtectedPage_ShouldAllowAccess**
   - Sprawdza czy zalogowany użytkownik ma dostęp do chronionych stron

3. **Logout_ShouldLogoutUserSuccessfully**
   - Sprawdza czy wylogowanie działa poprawnie

4. **TestLogin_WithInvalidParameter_ShouldNotLogin**
   - Sprawdza czy brak parametru `test=true` blokuje logowanie

## 🔄 Aktualizowane testy

Następujące testy zostały zaktualizowane, aby używać `TestAuthHelper`:

### HomePageTests.cs
- ✅ `HomePage_AfterLogin_ShouldShowDashboard` - odblokowany

### DashboardTests.cs
- ✅ `Dashboard_WhenLoggedIn_ShouldLoadSuccessfully` - odblokowany
- ✅ `Dashboard_ShouldDisplayGraphVisualizer` - odblokowany
- ✅ `Dashboard_GraphVisualizer_ShouldLoadVisJs` - odblokowany
- ⏸️ `Dashboard_WhenUserHasTechnologies_ShouldDisplayInGraph` - wymaga danych testowych

## 📊 Uruchamianie testów

### Wszystkie testy E2E

```bash
# Uruchom aplikację
dotnet run --project DeveloperGoals/DeveloperGoals.csproj

# W osobnym terminalu uruchom testy
dotnet test DeveloperGoals.E2ETests/DeveloperGoals.E2ETests.csproj
```

### Tylko testy autentykacji

```bash
dotnet test DeveloperGoals.E2ETests/DeveloperGoals.E2ETests.csproj --filter "FullyQualifiedName~AuthenticationTests"
```

### Konkretny test

```bash
dotnet test --filter "FullyQualifiedName~TestLogin_ShouldLoginUserSuccessfully"
```

## 🐛 Debugowanie

Jeśli testy nie przechodzą:

1. **Sprawdź czy aplikacja działa**
   ```bash
   dotnet run --project DeveloperGoals/DeveloperGoals.csproj
   ```

2. **Sprawdź URL w PlaywrightSettings.cs**
   ```csharp
   public static string BaseUrl => "https://localhost:5001";
   ```

3. **Uruchom testy z widoczną przeglądarką**
   - Zmień w `PlaywrightSettings.cs`: `Headless = false`

4. **Sprawdź screenshoty**
   - Screenshoty błędnych testów znajdują się w `screenshots/`

5. **Sprawdź logi aplikacji**
   - Logi aplikacji powinny pokazać wywołanie endpointu `/login/test?test=true`

## 📚 Dodatkowe zasoby

- [TESTING_README.md](./TESTING_README.md) - pełna dokumentacja testowania
- [TESTS_QUICK_START.md](./TESTS_QUICK_START.md) - szybki start z testami
- [Playwright Documentation](https://playwright.dev/dotnet/) - oficjalna dokumentacja Playwright
