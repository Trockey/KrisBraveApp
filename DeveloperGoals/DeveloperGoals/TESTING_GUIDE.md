# Przewodnik Testowania AI Recommendations Endpoint

## Testowanie Manualne

### 1. Przygotowanie

Upewnij się, że:
- Baza danych jest uruchomiona (PostgreSQL)
- Użytkownik jest zalogowany i ma profil
- Użytkownik ma co najmniej jedną technologię w grafie
- OpenRouter API Key jest skonfigurowany w `appsettings.json` lub User Secrets

### 2. Testowanie przez Swagger UI

1. Uruchom aplikację:
```bash
dotnet run --project DeveloperGoals/DeveloperGoals
```

2. Otwórz Swagger UI: `https://localhost:7000/swagger`

3. Zaloguj się przez endpoint `/login/google`

4. Użyj endpointa `POST /api/ai/recommendations`:
```json
{
  "fromTechnologyId": 1,
  "contextTechnologyIds": [1, 2]
}
```

### 3. Testowanie przez curl

#### Sukces (200)
```bash
curl -X POST https://localhost:7000/api/ai/recommendations \
  -H "Content-Type: application/json" \
  -H "Cookie: .AspNetCore.Cookies={your-cookie}" \
  -d '{
    "fromTechnologyId": 1,
    "contextTechnologyIds": [1, 2, 3]
  }'
```

#### Profil niekompletny (400)
```bash
# Zaloguj się jako użytkownik bez profilu
curl -X POST https://localhost:7000/api/ai/recommendations \
  -H "Content-Type: application/json" \
  -H "Cookie: .AspNetCore.Cookies={your-cookie}" \
  -d '{
    "fromTechnologyId": 1
  }'

# Oczekiwany response:
{
  "error": "ValidationError",
  "message": "Profile is incomplete. Please complete your profile first.",
  "details": null,
  "errors": null
}
```

#### Technologia nie należy do użytkownika (400)
```bash
curl -X POST https://localhost:7000/api/ai/recommendations \
  -H "Content-Type: application/json" \
  -H "Cookie: .AspNetCore.Cookies={your-cookie}" \
  -d '{
    "fromTechnologyId": 999999
  }'

# Oczekiwany response:
{
  "error": "NotFound",
  "message": "Source technology not found",
  "details": null,
  "errors": null
}
```

#### Cache Hit
```bash
# Wywołaj endpoint dwa razy z tymi samymi parametrami
# Pierwszy raz: fromCache = false
# Drugi raz: fromCache = true (< 24h)
```

### 4. Testowanie Cache

```bash
# Request 1 - Cache miss
curl -X POST https://localhost:7000/api/ai/recommendations \
  -H "Content-Type: application/json" \
  -H "Cookie: .AspNetCore.Cookies={your-cookie}" \
  -d '{"fromTechnologyId": 1}' \
  | jq '.fromCache'
# Output: false

# Request 2 - Cache hit (natychmiast po pierwszym)
curl -X POST https://localhost:7000/api/ai/recommendations \
  -H "Content-Type: application/json" \
  -H "Cookie: .AspNetCore.Cookies={your-cookie}" \
  -d '{"fromTechnologyId": 1}' \
  | jq '.fromCache'
# Output: true
```

## Testowanie Automatyczne

### Przykład testu jednostkowego (xUnit)

```csharp
using DeveloperGoals.Services;
using DeveloperGoals.DTOs;
using DeveloperGoals.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

public class AIRecommendationServiceTests
{
    [Fact]
    public async Task GenerateRecommendationsAsync_ProfileIncomplete_ThrowsException()
    {
        // Arrange
        var mockContext = CreateMockDbContext(profileExists: false);
        var mockOpenRouter = new Mock<IOpenRouterService>();
        var mockCache = new Mock<IMemoryCache>();
        var mockLogger = new Mock<ILogger<AIRecommendationService>>();
        var mockOptions = Options.Create(new CacheOptions());

        var service = new AIRecommendationService(
            mockContext.Object,
            mockOpenRouter.Object,
            mockCache.Object,
            mockLogger.Object,
            mockOptions);

        var command = new GenerateRecommendationsCommand
        {
            FromTechnologyId = 1
        };

        // Act & Assert
        await Assert.ThrowsAsync<ProfileIncompleteException>(
            () => service.GenerateRecommendationsAsync(1, command));
    }

    [Fact]
    public async Task GenerateRecommendationsAsync_CacheHit_ReturnsCachedResult()
    {
        // Arrange
        var mockContext = CreateMockDbContext();
        var mockOpenRouter = new Mock<IOpenRouterService>();
        var mockLogger = new Mock<ILogger<AIRecommendationService>>();
        var mockOptions = Options.Create(new CacheOptions());

        var cachedResult = new RecommendationsResponseDto
        {
            Recommendations = new List<RecommendationDto>(),
            Count = 10,
            FromCache = false
        };

        var mockCache = new Mock<IMemoryCache>();
        object outValue = cachedResult;
        mockCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out outValue))
            .Returns(true);

        var service = new AIRecommendationService(
            mockContext.Object,
            mockOpenRouter.Object,
            mockCache.Object,
            mockLogger.Object,
            mockOptions);

        var command = new GenerateRecommendationsCommand
        {
            FromTechnologyId = 1
        };

        // Act
        var result = await service.GenerateRecommendationsAsync(1, command);

        // Assert
        Assert.True(result.FromCache);
        mockOpenRouter.Verify(x => x.GetRecommendationsAsync(
            It.IsAny<UserProfile>(),
            It.IsAny<TechnologyDto>(),
            It.IsAny<List<TechnologyDto>>(),
            It.IsAny<HashSet<int>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    private Mock<ApplicationDbContext> CreateMockDbContext(bool profileExists = true)
    {
        // Implementation...
        return new Mock<ApplicationDbContext>();
    }
}
```

### Przykład testu integracyjnego

```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;

public class AIRecommendationsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AIRecommendationsIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task POST_Recommendations_Unauthorized_Returns401()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new GenerateRecommendationsCommand
        {
            FromTechnologyId = 1
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/ai/recommendations", command);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task POST_Recommendations_ValidRequest_Returns200()
    {
        // Arrange
        var client = _factory.CreateClient();
        await AuthenticateClient(client); // Helper method to login

        var command = new GenerateRecommendationsCommand
        {
            FromTechnologyId = 1,
            ContextTechnologyIds = new List<int> { 1, 2 }
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/ai/recommendations", command);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RecommendationsResponseDto>();
        
        Assert.NotNull(result);
        Assert.Equal(10, result.Count);
        Assert.NotEmpty(result.Recommendations);
    }
}
```

## Monitorowanie

### Sprawdzanie logów

```bash
# Logi aplikacji
dotnet run --project DeveloperGoals/DeveloperGoals | grep "AI"

# Przykładowe logi:
# [Information] Sending request to OpenRouter API for user profile with role Backend
# [Information] Successfully received 10 recommendations from OpenRouter API
# [Information] Cache hit for user 1, technology 2
# [Warning] OpenRouter API request timed out after 20s
# [Error] OpenRouter API returned error 500: Internal Server Error
```

### Metryki do monitorowania

1. **Cache Hit Rate**
```sql
-- Policz ile requestów było z cache
SELECT 
    COUNT(*) FILTER (WHERE from_cache = true) * 100.0 / COUNT(*) as cache_hit_rate
FROM ai_recommendation_logs;
```

2. **Response Time**
```sql
-- Średni czas odpowiedzi
SELECT 
    AVG(response_time_ms) as avg_response_time,
    PERCENTILE_CONT(0.95) WITHIN GROUP (ORDER BY response_time_ms) as p95
FROM ai_recommendation_logs;
```

3. **Error Rate**
```sql
-- Procent błędów
SELECT 
    COUNT(*) FILTER (WHERE status_code >= 400) * 100.0 / COUNT(*) as error_rate
FROM ai_recommendation_logs;
```

## Rozwiązywanie Problemów

### Problem: Timeout (408)

**Przyczyna**: OpenRouter API nie odpowiedział w ciągu 20s

**Rozwiązanie**:
1. Sprawdź status OpenRouter API: https://status.openrouter.ai
2. Zwiększ timeout w `appsettings.json` (nie zalecane)
3. Sprawdź połączenie internetowe
4. Spróbuj ponownie - może być cache hit

### Problem: Invalid API Key (500)

**Przyczyna**: Nieprawidłowy klucz API OpenRouter

**Rozwiązanie**:
1. Sprawdź klucz w `appsettings.json` lub User Secrets
2. Wygeneruj nowy klucz na https://openrouter.ai
3. Upewnij się, że klucz zaczyna się od `sk-or-v1-`

### Problem: Profile Incomplete (400)

**Przyczyna**: Użytkownik nie ma kompletnego profilu

**Rozwiązanie**:
1. Użytkownik musi wypełnić profil: MainTechnologies, Role, DevelopmentArea
2. Endpoint: `POST /api/profile`

### Problem: Technology Not Found (404)

**Przyczyna**: Technologia o podanym ID nie istnieje lub nie należy do użytkownika

**Rozwiązanie**:
1. Sprawdź czy technologia istnieje: `GET /api/technologies`
2. Upewnij się, że używasz ID z tabeli UserTechnologies, nie TechnologyDefinitions
3. Sprawdź czy technologia ma status Active

## Najlepsze Praktyki

1. **Zawsze testuj cache** - wywołaj endpoint dwa razy z tymi samymi parametrami
2. **Testuj edge cases** - puste contextTechnologyIds, duplikaty, nieistniejące ID
3. **Monitoruj koszty** - każde wywołanie AI kosztuje, cache zmniejsza koszty
4. **Loguj wszystko** - ułatwia debugowanie problemów z AI
5. **Testuj timeout** - symuluj wolne połączenie
6. **Waliduj response** - sprawdź czy wszystkie 10 rekomendacji jest zwracanych

## Checklist Testowania

- [ ] Sukces - generowanie rekomendacji (200)
- [ ] Cache hit - zwrócenie z cache (200)
- [ ] Profil niekompletny (400)
- [ ] Technologia nie należy do użytkownika (400)
- [ ] Nieprawidłowy status technologii (400)
- [ ] Technologia nie znaleziona (404)
- [ ] Timeout AI (408)
- [ ] Błąd API OpenRouter (500)
- [ ] Nieprawidłowy format odpowiedzi (502)
- [ ] Unauthorized - brak autentykacji (401)
- [ ] Walidacja - fromTechnologyId required (400)
- [ ] Walidacja - max 50 contextTechnologyIds (400)
- [ ] Cache expiration - po 24h nowy request do AI
- [ ] Deduplikacja contextTechnologyIds
- [ ] isAlreadyInGraph - sprawdzenie czy technologia w grafie
