# Przewodnik testowania dla DeveloperGoals

Ten dokument opisuje strategię testowania dla aplikacji DeveloperGoals, zawierającą testy jednostkowe i E2E.

## 📋 Spis treści

- [Architektura testów](#architektura-testów)
- [Testy jednostkowe](#testy-jednostkowe)
- [Testy E2E](#testy-e2e)
- [Uruchamianie testów](#uruchamianie-testów)
- [Najlepsze praktyki](#najlepsze-praktyki)
- [CI/CD](#cicd)

## 🏗️ Architektura testów

Projekt wykorzystuje dwa typy testów:

### Testy jednostkowe (Unit Tests)
- **Projekt**: `DeveloperGoals.UnitTests`
- **Framework**: xUnit
- **Biblioteki**: Moq, FluentAssertions, EF Core InMemory
- **Cel**: Testowanie izolowanych jednostek kodu (serwisy, kontrolery, modele)

### Testy E2E (End-to-End Tests)
- **Projekt**: `DeveloperGoals.E2ETests`
- **Framework**: NUnit + Playwright for .NET
- **Cel**: Testowanie scenariuszy użytkownika w rzeczywistym środowisku przeglądarki

## 🧪 Testy jednostkowe

### Struktura projektu

```
DeveloperGoals.UnitTests/
├── Controllers/
│   └── TechnologyControllerTests.cs
├── Services/
│   └── TechnologyServiceTests.cs
├── Models/
│   └── UserTechnologyTests.cs
└── Helpers/
    └── TestDbContextFactory.cs (opcjonalnie)
```

### Uruchamianie testów jednostkowych

```bash
# Uruchomienie wszystkich testów jednostkowych
dotnet test DeveloperGoals.UnitTests/DeveloperGoals.UnitTests.csproj

# Uruchomienie z pokryciem kodu (wymaga narzędzia coverlet)
dotnet test DeveloperGoals.UnitTests/DeveloperGoals.UnitTests.csproj /p:CollectCoverage=true

# Uruchomienie konkretnego testu
dotnet test --filter "FullyQualifiedName~TechnologyServiceTests.GetGraphAsync_WhenSuccessful"
```

### Przykład testu jednostkowego

```csharp
[Test]
public async Task GetGraphAsync_WhenSuccessful_ShouldReturnGraph()
{
    // Arrange - przygotowanie mocków i danych testowych
    var expectedGraph = new TechnologyGraphDto { /* ... */ };
    _serviceMock.Setup(x => x.GetGraphAsync()).ReturnsAsync(expectedGraph);

    // Act - wywołanie testowanej metody
    var result = await _sut.GetGraphAsync();

    // Assert - sprawdzenie wyników
    result.Should().NotBeNull();
    result.Nodes.Should().HaveCount(2);
}
```

### Co testujemy?

#### Serwisy (Services)
- ✅ Poprawność logiki biznesowej
- ✅ Obsługa błędów i wyjątków
- ✅ Interakcje z zależnościami (HTTP Client, Logger)
- ✅ Walidacja argumentów

#### Kontrolery (Controllers)
- ✅ Poprawność routingu i akcji
- ✅ Zwracane kody HTTP (200, 404, 400, etc.)
- ✅ Walidacja modeli
- ✅ Autoryzacja (gdy wymagana)

#### Modele (Models)
- ✅ Walidacja właściwości
- ✅ Logika biznesowa w modelach
- ✅ Relacje między encjami

## 🌐 Testy E2E

### Struktura projektu

```
DeveloperGoals.E2ETests/
├── PageObjects/
│   ├── HomePage.cs
│   ├── DashboardPage.cs
│   └── LoginPage.cs
├── Tests/
│   ├── HomePageTests.cs
│   ├── DashboardTests.cs
│   └── AuthenticationTests.cs
├── PlaywrightSettings.cs
└── test-results/
    ├── screenshots/
    └── traces/
```

### Instalacja przeglądarek Playwright

```bash
# Po pierwszej kompilacji projektu E2E
dotnet build DeveloperGoals.E2ETests/DeveloperGoals.E2ETests.csproj

# Instalacja przeglądarki Chromium
./DeveloperGoals.E2ETests/bin/Debug/net9.0/playwright.ps1 install chromium

# Lub wszystkie przeglądarki (Firefox, WebKit)
./DeveloperGoals.E2ETests/bin/Debug/net9.0/playwright.ps1 install
```

### Uruchamianie testów E2E

```bash
# Upewnij się, że aplikacja działa (np. na https://localhost:5001)
dotnet run --project DeveloperGoals/DeveloperGoals.csproj

# W osobnym terminalu uruchom testy E2E
dotnet test DeveloperGoals.E2ETests/DeveloperGoals.E2ETests.csproj

# Uruchomienie z headless=false (widoczna przeglądarka)
# Zmień w PlaywrightSettings.cs: Headless = false

# Uruchomienie z zapisem trace (do debugowania)
dotnet test DeveloperGoals.E2ETests/DeveloperGoals.E2ETests.csproj -- Playwright.BrowserOptions='{"tracing":"on"}'
```

### Autentykacja w testach E2E

Testy E2E używają specjalnego mechanizmu logowania testowego zamiast OAuth:

#### TestAuthHelper

Projekt zawiera helper `TestAuthHelper` do zarządzania autentykacją:

```csharp
// Logowanie użytkownika testowego (Id=2)
await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);

// Wylogowanie użytkownika
await TestAuthHelper.LogoutAsync(Page, _baseUrl);

// Sprawdzenie czy użytkownik jest zalogowany
bool isLoggedIn = await TestAuthHelper.IsUserLoggedInAsync(Page);
```

#### Użytkownik testowy

- **Id**: 2
- **GoogleId**: "108226413010999999999"
- **Email**: "tester@test.com"
- **Name**: "Test"

Użytkownik testowy jest automatycznie tworzony przy pierwszym logowaniu przez endpoint `/login/test?test=true`.

#### Przykład użycia w teście

```csharp
[Test]
public async Task Dashboard_WhenLoggedIn_ShouldLoadSuccessfully()
{
    // Arrange - zaloguj użytkownika testowego
    await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);
    var dashboardPage = new DashboardPage(Page, _baseUrl);

    // Act
    await dashboardPage.NavigateAsync();

    // Assert
    await Expect(dashboardPage.GetPageTitle()).ToBeVisibleAsync();
}
```

### Page Object Model (POM)

Wszystkie testy E2E powinny używać wzorca Page Object Model:

```csharp
public class HomePage
{
    private readonly IPage _page;
    
    public HomePage(IPage page) => _page = page;
    
    // Locatory
    private ILocator LoginButton => _page.GetByRole(AriaRole.Link, new() { Name = "Zaloguj" });
    
    // Akcje
    public async Task NavigateAsync() => await _page.GotoAsync("/");
    public async Task ClickLoginAsync() => await LoginButton.ClickAsync();
    
    // Asercje pomocnicze
    public async Task<bool> IsLoginButtonVisibleAsync() => await LoginButton.IsVisibleAsync();
}
```

### Co testujemy?

#### Scenariusze użytkownika
- ✅ Nawigacja po aplikacji
- ✅ Logowanie i wylogowywanie (Google OAuth)
- ✅ Dodawanie/edycja/usuwanie technologii
- ✅ Wizualizacja grafu (vis.js)
- ✅ Responsywność UI

#### Integracje
- ✅ Komunikacja z backendem (API)
- ✅ Aktualizacje UI po operacjach SignalR (Blazor Server)
- ✅ JavaScript Interop (vis.js)

## 🚀 Uruchamianie testów

### Wszystkie testy naraz

```bash
# Z poziomu głównego folderu solution
dotnet test DeveloperGoals.sln
```

### Tylko testy jednostkowe

```bash
dotnet test DeveloperGoals.UnitTests/DeveloperGoals.UnitTests.csproj
```

### Tylko testy E2E

```bash
dotnet test DeveloperGoals.E2ETests/DeveloperGoals.E2ETests.csproj
```

### Testy z raportem

```bash
# Wymaga: dotnet tool install --global dotnet-reportgenerator-globaltool
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage-report
```

## ✅ Najlepsze praktyki

### Testy jednostkowe

1. **AAA Pattern** - Arrange, Act, Assert
2. **Nazewnictwo**: `MetodName_WhenCondition_ShouldExpectedBehavior`
3. **Izolacja**: Każdy test powinien być niezależny
4. **Mocki**: Używaj mocków dla zależności zewnętrznych
5. **FluentAssertions**: Dla czytelnych asercji

### Testy E2E

1. **Page Object Model**: Oddziel logikę strony od testów
2. **Selektory semantyczne**: Używaj `GetByRole`, `GetByText` zamiast selektorów CSS
3. **Czekanie**: Wykorzystuj automatyczne czekanie Playwright
4. **Izolacja**: Każdy test powinien być niezależny od stanu poprzednich
5. **Screenshots**: Zapisuj screenshot przy błędzie testu
6. **Blazor Server specifics**: Uwzględnij opóźnienia SignalR

```csharp
// ❌ Unikaj
await _page.Locator("#submit-btn").ClickAsync();
await Task.Delay(1000); // Arbitrary delays

// ✅ Preferuj
await _page.GetByRole(AriaRole.Button, new() { Name = "Wyślij" }).ClickAsync();
await Expect(_page.GetByText("Sukces")).ToBeVisibleAsync(); // Auto-waiting
```

## 🔧 Konfiguracja

### Zmienne środowiskowe dla testów

```bash
# .env lub ustawienia systemowe
BASE_URL=https://localhost:5001
TEST_USER_EMAIL=test@example.com
TEST_USER_PASSWORD=TestPassword123!
```

### Konfiguracja Playwright

Edytuj `PlaywrightSettings.cs`:

```csharp
public static BrowserTypeLaunchOptions BrowserLaunchOptions => new()
{
    Headless = true,  // false dla debugowania
    SlowMo = 0,       // 100-500 dla debugowania
};
```

## 🎯 CI/CD

### GitHub Actions (przykład)

```yaml
name: Tests

on: [push, pull_request]

jobs:
  unit-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '9.0.x'
      - name: Run unit tests
        run: dotnet test DeveloperGoals.UnitTests/

  e2e-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '9.0.x'
      - name: Build E2E project
        run: dotnet build DeveloperGoals.E2ETests/
      - name: Install Playwright
        run: pwsh DeveloperGoals.E2ETests/bin/Debug/net9.0/playwright.ps1 install chromium --with-deps
      - name: Run E2E tests
        run: dotnet test DeveloperGoals.E2ETests/
```

## 📚 Dodatkowe zasoby

- [Playwright for .NET Documentation](https://playwright.dev/dotnet/)
- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)
- [FluentAssertions Documentation](https://fluentassertions.com/)

## 🐛 Debugowanie testów

### Debugowanie testów jednostkowych

1. Ustaw breakpoint w teście
2. Uruchom test w trybie debug (F5 w Visual Studio/Rider)
3. Wykorzystaj okno Watch do inspekcji zmiennych

### Debugowanie testów E2E

```csharp
// Wstrzymaj test dla inspekcji
await _page.PauseAsync();

// Wykonaj wolniej
// W PlaywrightSettings.cs: SlowMo = 500

// Zapisz trace
await _page.Context.Tracing.StartAsync(new() { Screenshots = true });
// ... wykonaj akcje ...
await _page.Context.Tracing.StopAsync(new() { Path = "trace.zip" });

// Obejrzyj trace
// pwsh bin/Debug/net9.0/playwright.ps1 show-trace trace.zip
```

## 📝 Dodatkowe notatki

- Testy jednostkowe powinny być szybkie (< 100ms per test)
- Testy E2E mogą być wolniejsze ale powinny być niezawodne
- Mocki bazy danych używają EF Core InMemory
- Dla pełnej integracji z PostgreSQL, rozważ użycie Testcontainers
- OAuth Google w testach E2E wymaga mockowania lub testowego konta

---

**Powodzenia z testowaniem! 🚀**
