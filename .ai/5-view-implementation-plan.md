# API Endpoint Implementation Plan: POST /api/ai/recommendations

## 1. Przegląd punktu końcowego

Endpoint `POST /api/ai/recommendations` generuje inteligentne rekomendacje kolejnych technologii do nauki na podstawie aktualnej technologii użytkownika oraz opcjonalnego kontekstu innych technologii. System wykorzystuje AI (OpenRouter.ai) do analizy profilu użytkownika i generowania spersonalizowanych sugestii. Rekomendacje są cache'owane w pamięci przez 24 godziny dla optymalizacji wydajności i kosztów.

**Kluczowe funkcjonalności:**
- Generowanie rekomendacji AI na podstawie technologii źródłowej
- Uwzględnianie kontekstu dodatkowych technologii użytkownika, które zostały dodane do grafu
- Cache in-memory z TTL 24h
- Timeout 20 sekund dla wywołań AI
- Sprawdzanie czy rekomendowane technologie już istnieją w grafie użytkownika
- Walidacja kompletności profilu użytkownika

---

## 2. Szczegóły żądania

### Metoda HTTP
**POST**

### Struktura URL
```
/api/ai/recommendations
```

### Autentykacja
**Wymagana** - użytkownik musi być zalogowany (middleware autentykacji)

### Request Body
```json
{
  "fromTechnologyId": 1002,
  "contextTechnologyIds": [1001, 1002, 1003]
}
```

### Parametry

#### Wymagane:
- **fromTechnologyId** (int)
  - ID technologii użytkownika, od której generujemy rekomendacje
  - Musi istnieć w tabeli `UserTechnologies`
  - Musi należeć do zalogowanego użytkownika
  - Musi mieć status `Active`

#### Opcjonalne:
- **contextTechnologyIds** (List<int>)
  - Tablica ID technologii użytkownika dla dodatkowego kontekstu
  - Wszystkie ID muszą należeć do zalogowanego użytkownika
  - Mogą być puste lub null
  - Duplikaty są akceptowalne (zostaną zignorowane)

### Walidacja Request Body
- `fromTechnologyId` - Required, Range(1, int.MaxValue)
- `contextTechnologyIds` - Optional, każdy element Range(1, int.MaxValue)

---

## 3. Wykorzystywane typy

### DTOs i Command Models (z Types.cs)

#### Request:
```csharp
// Linie 363-367
public class GenerateRecommendationsCommand
{
    public int FromTechnologyId { get; set; }
    public List<int>? ContextTechnologyIds { get; set; }
}
```

#### Response - Sukces:
```csharp
// Linie 386-393
public class RecommendationsResponseDto
{
    public List<RecommendationDto> Recommendations { get; set; } = new();
    public int Count { get; set; }
    public bool FromCache { get; set; }
    public DateTime? CacheExpiresAt { get; set; }
    public DateTime GeneratedAt { get; set; }
}

// Linie 372-381
public class RecommendationDto
{
    public int TechnologyDefinitionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Prefix { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string SystemDescription { get; set; } = string.Empty;
    public string AiReasoning { get; set; } = string.Empty;
    public bool IsAlreadyInGraph { get; set; }
}
```

#### Response - Błąd:
```csharp
// Linie 676-682
public class ErrorResponseDto
{
    public string Error { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public List<ValidationErrorDetail>? Errors { get; set; }
}
```

### Encje bazodanowe (wykorzystywane):
- **Users** - weryfikacja użytkownika
- **UserProfiles** - sprawdzenie kompletności profilu
- **UserTechnologies** - walidacja technologii źródłowej i kontekstowych
- **TechnologyDefinitions** - mapowanie rekomendacji AI

---

## 4. Szczegóły odpowiedzi

### Response 200 OK - Sukces
```json
{
  "recommendations": [
    {
      "technologyDefinitionId": 45,
      "name": "DotNet - LINQ Advanced",
      "prefix": "DotNet",
      "tag": "Technologia",
      "systemDescription": "Advanced Language Integrated Query techniques for data manipulation",
      "aiReasoning": "Natural progression after Entity Framework, helps optimize database queries",
      "isAlreadyInGraph": false
    }
  ],
  "count": 10,
  "fromCache": false,
  "cacheExpiresAt": "2025-12-02T14:30:00Z",
  "generatedAt": "2025-12-01T14:30:00Z"
}
```

### Response 400 Bad Request - Walidacja
```json
{
  "error": "ValidationError",
  "message": "Profile is incomplete. Please complete your profile first."
}
```

**Inne przypadki 400:**
- "fromTechnologyId is required"
- "Technology does not belong to user"
- "Context technology {id} does not belong to user"
- "Technology must have Active status"

### Response 404 Not Found
```json
{
  "error": "NotFound",
  "message": "Source technology not found"
}
```

### Response 408 Request Timeout
```json
{
  "error": "Timeout",
  "message": "AI service did not respond within 20 seconds. Please try again."
}
```

### Response 500 Internal Server Error
```json
{
  "error": "AIServiceError",
  "message": "Failed to generate recommendations. Please try again later.",
  "details": "OpenRouter API returned error 503"
}
```

### Response 502 Bad Gateway
```json
{
  "error": "BadGateway",
  "message": "AI service returned invalid response format"
}
```

---

## 5. Przepływ danych

### Diagram przepływu:

```
1. Client Request
   ↓
2. Authentication Middleware (sprawdzenie JWT/Cookie)
   ↓
3. Controller: AIRecommendationsController.GenerateRecommendations()
   ↓
4. Walidacja Command (DataAnnotations)
   ↓
5. AIRecommendationService.GenerateRecommendationsAsync()
   │
   ├─→ 5.1 Sprawdzenie cache (MemoryCache)
   │    └─→ Jeśli hit: zwróć z cache
   │
   ├─→ 5.2 Walidacja użytkownika i profilu (DbContext)
   │    ├─→ Sprawdź czy UserProfile istnieje
   │    └─→ Sprawdź kompletność profilu
   │
   ├─→ 5.3 Walidacja technologii źródłowej (DbContext)
   │    ├─→ Sprawdź czy technologia istnieje
   │    ├─→ Sprawdź czy należy do użytkownika
   │    └─→ Sprawdź status Active
   │
   ├─→ 5.4 Walidacja technologii kontekstowych (DbContext)
   │    └─→ Sprawdź czy wszystkie należą do użytkownika
   │
   ├─→ 5.5 Pobranie danych dla AI (DbContext)
   │    ├─→ Dane profilu użytkownika
   │    ├─→ Szczegóły technologii źródłowej
   │    ├─→ Szczegóły technologii kontekstowych
   │    └─→ Lista technologii już w grafie użytkownika
   │
   ├─→ 5.6 OpenRouterService.GetRecommendationsAsync()
   │    ├─→ Budowanie promptu AI
   │    ├─→ HTTP POST do OpenRouter.ai (timeout 20s)
   │    ├─→ Parsowanie odpowiedzi JSON
   │    └─→ Walidacja formatu odpowiedzi
   │
   ├─→ 5.7 Przetwarzanie rekomendacji
   │    ├─→ Mapowanie na TechnologyDefinitions
   │    ├─→ Sprawdzenie isAlreadyInGraph
   │    └─→ Budowanie RecommendationDto
   │
   ├─→ 5.8 Zapis do cache (24h TTL)
   │
   └─→ 5.9 Zwrócenie RecommendationsResponseDto
   ↓
6. Controller: Zwrócenie 200 OK
   ↓
7. Client Response
```

### Szczegóły interakcji z bazą danych:

**Query 1: Sprawdzenie profilu**
```sql
SELECT Id, MainTechnologies, Role, DevelopmentArea 
FROM UserProfiles 
WHERE UserId = @userId
```

**Query 2: Walidacja technologii źródłowej**
```sql
SELECT ut.Id, ut.Status, td.Name, td.Prefix, td.SystemDescription
FROM UserTechnologies ut
INNER JOIN TechnologyDefinitions td ON ut.TechnologyDefinitionId = td.Id
WHERE ut.Id = @fromTechnologyId AND ut.UserId = @userId
```

**Query 3: Walidacja technologii kontekstowych**
```sql
SELECT ut.Id, td.Name, td.Prefix, td.SystemDescription
FROM UserTechnologies ut
INNER JOIN TechnologyDefinitions td ON ut.TechnologyDefinitionId = td.Id
WHERE ut.Id IN @contextIds AND ut.UserId = @userId
```

**Query 4: Lista technologii w grafie użytkownika**
```sql
SELECT TechnologyDefinitionId 
FROM UserTechnologies 
WHERE UserId = @userId AND Status = 'Active'
```

### Szczegóły interakcji z OpenRouter API:

**Endpoint:** `https://openrouter.ai/api/v1/chat/completions`

**Request:**
```json
{
  "model": "anthropic/claude-3.5-sonnet",
  "messages": [
    {
      "role": "system",
      "content": "You are a technology learning advisor..."
    },
    {
      "role": "user",
      "content": "Based on user profile: Role=Backend, Area=Backend, MainTech=[DotNet, PostgreSQL]..."
    }
  ],
  "temperature": 0.7,
  "max_tokens": 2000
}
```

**Headers:**
```
Authorization: Bearer {API_KEY}
Content-Type: application/json
HTTP-Referer: {APP_URL}
X-Title: DeveloperGoals
```

---

## 6. Względy bezpieczeństwa

### 6.1 Autentykacja i Autoryzacja

**Middleware autentykacji:**
- Endpoint wymaga atrybutu `[Authorize]`
- Weryfikacja JWT token lub Cookie session
- Ekstrakcja `userId` z ClaimsPrincipal

**Autoryzacja na poziomie danych:**
- Wszystkie zapytania do `UserTechnologies` muszą filtrować po `UserId`
- Walidacja że `fromTechnologyId` należy do zalogowanego użytkownika
- Walidacja że wszystkie `contextTechnologyIds` należą do użytkownika
- Brak możliwości dostępu do technologii innych użytkowników

### 6.2 Walidacja danych wejściowych

**DataAnnotations na Command:**
```csharp
[Required(ErrorMessage = "fromTechnologyId is required")]
[Range(1, int.MaxValue, ErrorMessage = "Invalid technology ID")]
public int FromTechnologyId { get; set; }

[MaxLength(50, ErrorMessage = "Maximum 50 context technologies allowed")]
public List<int>? ContextTechnologyIds { get; set; }
```

**Dodatkowa walidacja w service:**
- Sprawdzenie czy technologia istnieje w bazie
- Sprawdzenie właściciela technologii
- Sprawdzenie statusu technologii (Active)
- Deduplikacja contextTechnologyIds
- Walidacja kompletności profilu

### 6.3 Ochrona przed atakami

**SQL Injection:**
- Używanie Entity Framework z parametryzowanymi zapytaniami
- Brak surowych zapytań SQL
- Walidacja wszystkich ID jako int

**Cache Poisoning:**
- Klucz cache zawiera userId: `ai_recommendations_{userId}_{fromTechId}_{contextHash}`
- Izolacja cache między użytkownikami
- TTL 24h zapobiega długotrwałemu przechowywaniu złych danych

**DoS Protection:**
- Timeout 20s dla wywołań AI
- Cache zmniejsza liczbę wywołań AI
- Brak rate limiting (jak w specyfikacji), ale można dodać w przyszłości

**API Key Security:**
- OpenRouter API key w `appsettings.json` (nie w kodzie)
- Używanie User Secrets w development
- Environment variables w production
- Nigdy nie zwracać API key w response

### 6.4 Data Leakage Prevention

- Nie zwracać danych innych użytkowników
- Nie logować wrażliwych danych (API keys, user data)
- Sanityzacja błędów przed zwróceniem do klienta
- Nie ujawniać szczegółów implementacji w error messages

---

## 7. Obsługa błędów

### 7.1 Hierarchia błędów

```
Exception
├─ ValidationException (400)
│  ├─ ProfileIncompleteException
│  ├─ TechnologyNotOwnedException
│  └─ InvalidTechnologyStatusException
│
├─ NotFoundException (404)
│  └─ TechnologyNotFoundException
│
├─ TimeoutException (408)
│  └─ AIServiceTimeoutException
│
├─ AIServiceException (500)
│  ├─ OpenRouterApiException
│  └─ AIResponseParsingException
│
└─ BadGatewayException (502)
   └─ InvalidAIResponseFormatException
```

### 7.2 Szczegółowe scenariusze błędów

#### 400 Bad Request - Validation Errors

**Przypadek 1: Profil niekompletny**
```csharp
// Warunek: UserProfile == null lub brak MainTechnologies/Role/DevelopmentArea
throw new ProfileIncompleteException(
    "Profile is incomplete. Please complete your profile first."
);
```

**Przypadek 2: Technologia nie należy do użytkownika**
```csharp
// Warunek: fromTechnologyId.UserId != currentUserId
throw new TechnologyNotOwnedException(
    "Technology does not belong to user"
);
```

**Przypadek 3: Nieprawidłowy status technologii**
```csharp
// Warunek: technology.Status != TechnologyStatus.Active
throw new InvalidTechnologyStatusException(
    "Technology must have Active status"
);
```

**Przypadek 4: Technologia kontekstowa nie należy do użytkownika**
```csharp
// Warunek: contextTech.UserId != currentUserId
throw new TechnologyNotOwnedException(
    $"Context technology {contextTechId} does not belong to user"
);
```

#### 404 Not Found

**Przypadek: Technologia źródłowa nie istnieje**
```csharp
// Warunek: UserTechnologies.Find(fromTechnologyId) == null
throw new TechnologyNotFoundException(
    "Source technology not found"
);
```

#### 408 Request Timeout

**Przypadek: AI service timeout**
```csharp
// Warunek: HttpClient.Timeout (20s) exceeded
throw new AIServiceTimeoutException(
    "AI service did not respond within 20 seconds. Please try again."
);
```

#### 500 Internal Server Error

**Przypadek 1: OpenRouter API error**
```csharp
// Warunek: OpenRouter zwrócił 4xx/5xx
throw new OpenRouterApiException(
    "Failed to generate recommendations. Please try again later.",
    details: $"OpenRouter API returned error {statusCode}"
);
```

**Przypadek 2: Błąd parsowania odpowiedzi**
```csharp
// Warunek: JsonException podczas parsowania
throw new AIResponseParsingException(
    "Failed to parse AI response",
    details: ex.Message
);
```

#### 502 Bad Gateway

**Przypadek: Nieprawidłowy format odpowiedzi AI**
```csharp
// Warunek: Brak wymaganych pól w odpowiedzi AI
throw new InvalidAIResponseFormatException(
    "AI service returned invalid response format"
);
```

### 7.3 Exception Handling Middleware

```csharp
public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, error, message, details) = exception switch
        {
            ProfileIncompleteException ex => 
                (400, "ValidationError", ex.Message, null),
            
            TechnologyNotOwnedException ex => 
                (400, "ValidationError", ex.Message, null),
            
            TechnologyNotFoundException ex => 
                (404, "NotFound", ex.Message, null),
            
            AIServiceTimeoutException ex => 
                (408, "Timeout", ex.Message, null),
            
            OpenRouterApiException ex => 
                (500, "AIServiceError", ex.Message, ex.Details),
            
            InvalidAIResponseFormatException ex => 
                (502, "BadGateway", ex.Message, null),
            
            _ => (500, "InternalServerError", "An unexpected error occurred", null)
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new ErrorResponseDto
        {
            Error = error,
            Message = message,
            Details = details
        }, cancellationToken);

        return true;
    }
}
```

### 7.4 Logowanie błędów

**Poziomy logowania:**
- **Information:** Cache hit/miss, successful AI calls
- **Warning:** Validation errors (400), Not Found (404), Timeouts (408)
- **Error:** AI service errors (500), Bad Gateway (502)
- **Critical:** Unexpected exceptions, database connection failures

**Przykład logowania:**
```csharp
_logger.LogWarning(
    "AI service timeout for user {UserId}, technology {TechId}",
    userId, fromTechnologyId
);

_logger.LogError(
    exception,
    "OpenRouter API error for user {UserId}: {StatusCode} - {Details}",
    userId, statusCode, details
);
```

---

## 8. Rozważania dotyczące wydajności

### 8.1 Optymalizacje cache

**In-Memory Cache Strategy:**
- **TTL:** 24 godziny
- **Klucz:** `ai_recommendations_{userId}_{fromTechId}_{contextHash}`
- **Rozmiar:** Limit 1000 wpisów, LRU eviction
- **Cache warming:** Brak (on-demand)

**Korzyści:**
- Redukcja kosztów API OpenRouter (płatne wywołania)
- Szybsze odpowiedzi (ms vs sekundy)
- Zmniejszenie obciążenia AI service

**Cache invalidation:**
- Automatyczna po 24h (TTL)
- Możliwość manualnego czyszczenia przez admina
- Brak invalidacji przy zmianach w grafie (akceptowalne dla MVP)

### 8.2 Optymalizacje zapytań do bazy danych

**Query 1: Sprawdzenie profilu i technologii (eager loading)**
```csharp
var user = await _context.Users
    .Include(u => u.Profile)
    .Include(u => u.Technologies.Where(t => t.Status == TechnologyStatus.Active))
        .ThenInclude(t => t.TechnologyDefinition)
    .AsNoTracking()
    .FirstOrDefaultAsync(u => u.Id == userId);
```
- Jeden query zamiast trzech
- `AsNoTracking()` dla read-only
- Eager loading eliminuje N+1 problem

**Query 2: Sprawdzenie isAlreadyInGraph (HashSet)**
```csharp
var existingTechIds = await _context.UserTechnologies
    .Where(ut => ut.UserId == userId && ut.Status == TechnologyStatus.Active)
    .Select(ut => ut.TechnologyDefinitionId)
    .ToHashSetAsync();
```
- HashSet dla O(1) lookup
- Tylko potrzebne kolumny (projection)

### 8.3 Optymalizacje wywołań AI

**Timeout configuration:**
```csharp
_httpClient.Timeout = TimeSpan.FromSeconds(20);
```

**Retry policy (opcjonalnie):**
```csharp
// Polly retry policy dla transient errors
var retryPolicy = Policy
    .Handle<HttpRequestException>()
    .WaitAndRetryAsync(2, retryAttempt => 
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
    );
```

**Prompt optimization:**
- Zwięzły prompt (mniej tokenów = niższy koszt)
- Structured output (JSON) dla łatwiejszego parsowania
- Limit max_tokens: 2000

### 8.4 Potencjalne wąskie gardła

**Wąskie gardło 1: OpenRouter API latency**
- **Problem:** Wywołania AI mogą trwać 5-15 sekund
- **Rozwiązanie:** Cache 24h, timeout 20s
- **Monitoring:** Logowanie czasu odpowiedzi

**Wąskie gardło 2: Parsowanie dużych odpowiedzi AI**
- **Problem:** Deserializacja JSON może być kosztowna
- **Rozwiązanie:** Limit max_tokens, streaming (przyszłość)
- **Monitoring:** Pomiar czasu parsowania

**Wąskie gardło 3: Memory cache size**
- **Problem:** Zbyt wiele wpisów w cache
- **Rozwiązanie:** LRU eviction, limit 1000 wpisów
- **Monitoring:** Cache hit rate, memory usage

**Wąskie gardło 4: Concurrent requests**
- **Problem:** Wiele równoczesnych requestów do AI
- **Rozwiązanie:** Connection pooling, rate limiting (przyszłość)
- **Monitoring:** Concurrent request count

### 8.5 Metryki wydajności (cele)

- **Cache hit rate:** > 60%
- **Response time (cache hit):** < 100ms
- **Response time (cache miss):** < 15s (średnio)
- **AI timeout rate:** < 5%
- **Error rate:** < 2%

---

## 9. Etapy wdrożenia

### Krok 1: Przygotowanie infrastruktury
**Czas: 1-2 godziny**

1.1. Dodanie pakietów NuGet:
```bash
dotnet add package Microsoft.Extensions.Caching.Memory
dotnet add package System.Net.Http.Json
```

1.2. Konfiguracja w `appsettings.json`:
```json
{
  "OpenRouter": {
    "ApiKey": "sk-or-v1-...",
    "BaseUrl": "https://openrouter.ai/api/v1",
    "Model": "anthropic/claude-3.5-sonnet",
    "Timeout": 20,
    "MaxTokens": 2000
  },
  "Cache": {
    "RecommendationsTTL": 24
  }
}
```

1.3. Utworzenie klas konfiguracyjnych:
```csharp
// Configuration/OpenRouterOptions.cs
public class OpenRouterOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Timeout { get; set; } = 20;
    public int MaxTokens { get; set; } = 2000;
}
```

### Krok 2: Utworzenie custom exceptions
**Czas: 30 minut**

2.1. Utworzenie pliku `Exceptions/AIExceptions.cs`:
```csharp
public class ProfileIncompleteException : Exception
{
    public ProfileIncompleteException(string message) : base(message) { }
}

public class TechnologyNotOwnedException : Exception
{
    public TechnologyNotOwnedException(string message) : base(message) { }
}

public class TechnologyNotFoundException : Exception
{
    public TechnologyNotFoundException(string message) : base(message) { }
}

public class AIServiceTimeoutException : Exception
{
    public AIServiceTimeoutException(string message) : base(message) { }
}

public class OpenRouterApiException : Exception
{
    public string Details { get; }
    public OpenRouterApiException(string message, string details) : base(message)
    {
        Details = details;
    }
}

public class InvalidAIResponseFormatException : Exception
{
    public InvalidAIResponseFormatException(string message) : base(message) { }
}
```

### Krok 3: Implementacja OpenRouterService
**Czas: 2-3 godziny**

3.1. Utworzenie interfejsu `Services/IOpenRouterService.cs`:
```csharp
public interface IOpenRouterService
{
    Task<List<RecommendationDto>> GetRecommendationsAsync(
        UserProfile profile,
        TechnologyDto sourceTechnology,
        List<TechnologyDto> contextTechnologies,
        HashSet<int> existingTechDefinitionIds,
        CancellationToken cancellationToken = default
    );
}
```

3.2. Implementacja `Services/OpenRouterService.cs`:
- Budowanie promptu AI
- Wywołanie HTTP POST do OpenRouter
- Parsowanie odpowiedzi JSON
- Walidacja formatu odpowiedzi
- Mapowanie na RecommendationDto
- Obsługa timeoutów i błędów

**Kluczowe elementy:**
```csharp
private async Task<string> BuildPromptAsync(
    UserProfile profile,
    TechnologyDto sourceTech,
    List<TechnologyDto> contextTechs)
{
    var prompt = $@"
You are a technology learning advisor. Generate 10 technology recommendations.

User Profile:
- Role: {profile.Role}
- Development Area: {profile.DevelopmentArea}
- Main Technologies: {string.Join(", ", profile.MainTechnologies)}

Current Technology: {sourceTech.Name} ({sourceTech.Prefix})
Description: {sourceTech.SystemDescription}

Context Technologies:
{string.Join("\n", contextTechs.Select(t => $"- {t.Name}: {t.SystemDescription}"))}

Return JSON array with 10 recommendations:
[
  {{
    ""name"": ""Technology Name"",
    ""prefix"": ""Category"",
    ""tag"": ""Technologia|Framework|BazaDanych|Metodologia|Narzedzie"",
    ""systemDescription"": ""Brief description"",
    ""aiReasoning"": ""Why this is recommended""
  }}
]
";
    return prompt;
}
```

### Krok 4: Implementacja AIRecommendationService
**Czas: 3-4 godziny**

4.1. Utworzenie interfejsu `Services/IAIRecommendationService.cs`:
```csharp
public interface IAIRecommendationService
{
    Task<RecommendationsResponseDto> GenerateRecommendationsAsync(
        int userId,
        GenerateRecommendationsCommand command,
        CancellationToken cancellationToken = default
    );
}
```

4.2. Implementacja `Services/AIRecommendationService.cs`:

**Główne metody:**
- `GenerateRecommendationsAsync()` - główna metoda
- `ValidateUserProfileAsync()` - walidacja profilu
- `ValidateSourceTechnologyAsync()` - walidacja technologii źródłowej
- `ValidateContextTechnologiesAsync()` - walidacja kontekstu
- `GetExistingTechnologyIdsAsync()` - pobranie istniejących tech
- `MapToTechnologyDefinitionsAsync()` - mapowanie rekomendacji
- `BuildCacheKey()` - budowanie klucza cache
- `GetFromCacheAsync()` - odczyt z cache
- `SaveToCacheAsync()` - zapis do cache

**Struktura:**
```csharp
public class AIRecommendationService : IAIRecommendationService
{
    private readonly ApplicationDbContext _context;
    private readonly IOpenRouterService _openRouterService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<AIRecommendationService> _logger;
    private readonly IOptions<CacheOptions> _cacheOptions;

    public async Task<RecommendationsResponseDto> GenerateRecommendationsAsync(
        int userId,
        GenerateRecommendationsCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Sprawdź cache
        var cacheKey = BuildCacheKey(userId, command);
        if (_cache.TryGetValue(cacheKey, out RecommendationsResponseDto cachedResult))
        {
            _logger.LogInformation("Cache hit for user {UserId}", userId);
            cachedResult.FromCache = true;
            return cachedResult;
        }

        // 2. Walidacja profilu
        var profile = await ValidateUserProfileAsync(userId, cancellationToken);

        // 3. Walidacja technologii źródłowej
        var sourceTech = await ValidateSourceTechnologyAsync(
            userId, command.FromTechnologyId, cancellationToken);

        // 4. Walidacja technologii kontekstowych
        var contextTechs = await ValidateContextTechnologiesAsync(
            userId, command.ContextTechnologyIds, cancellationToken);

        // 5. Pobranie istniejących technologii
        var existingTechIds = await GetExistingTechnologyIdsAsync(
            userId, cancellationToken);

        // 6. Wywołanie AI
        var recommendations = await _openRouterService.GetRecommendationsAsync(
            profile, sourceTech, contextTechs, existingTechIds, cancellationToken);

        // 7. Mapowanie i sprawdzenie isAlreadyInGraph
        var mappedRecommendations = await MapToTechnologyDefinitionsAsync(
            recommendations, existingTechIds, cancellationToken);

        // 8. Budowanie response
        var response = new RecommendationsResponseDto
        {
            Recommendations = mappedRecommendations,
            Count = mappedRecommendations.Count,
            FromCache = false,
            GeneratedAt = DateTime.UtcNow,
            CacheExpiresAt = DateTime.UtcNow.AddHours(_cacheOptions.Value.RecommendationsTTL)
        };

        // 9. Zapis do cache
        await SaveToCacheAsync(cacheKey, response);

        return response;
    }
}
```

### Krok 5: Implementacja controllera
**Czas: 1 godzina**

5.1. Utworzenie `Controllers/AIRecommendationsController.cs`:
```csharp
[ApiController]
[Route("api/ai")]
[Authorize]
public class AIRecommendationsController : ControllerBase
{
    private readonly IAIRecommendationService _recommendationService;
    private readonly ILogger<AIRecommendationsController> _logger;

    [HttpPost("recommendations")]
    [ProducesResponseType(typeof(RecommendationsResponseDto), 200)]
    [ProducesResponseType(typeof(ErrorResponseDto), 400)]
    [ProducesResponseType(typeof(ErrorResponseDto), 404)]
    [ProducesResponseType(typeof(ErrorResponseDto), 408)]
    [ProducesResponseType(typeof(ErrorResponseDto), 500)]
    [ProducesResponseType(typeof(ErrorResponseDto), 502)]
    public async Task<IActionResult> GenerateRecommendations(
        [FromBody] GenerateRecommendationsCommand command,
        CancellationToken cancellationToken)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        _logger.LogInformation(
            "Generating recommendations for user {UserId}, technology {TechId}",
            userId, command.FromTechnologyId);

        var result = await _recommendationService.GenerateRecommendationsAsync(
            userId, command, cancellationToken);

        return Ok(result);
    }
}
```

### Krok 6: Rejestracja serwisów w DI
**Czas: 15 minut**

6.1. Aktualizacja `Program.cs`:
```csharp
// Configuration
builder.Services.Configure<OpenRouterOptions>(
    builder.Configuration.GetSection("OpenRouter"));
builder.Services.Configure<CacheOptions>(
    builder.Configuration.GetSection("Cache"));

// Memory Cache
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 1000; // Max 1000 entries
});

// HttpClient for OpenRouter
builder.Services.AddHttpClient<IOpenRouterService, OpenRouterService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<OpenRouterOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(options.Timeout);
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.ApiKey}");
    client.DefaultRequestHeaders.Add("HTTP-Referer", "https://developergoals.app");
    client.DefaultRequestHeaders.Add("X-Title", "DeveloperGoals");
});

// Services
builder.Services.AddScoped<IAIRecommendationService, AIRecommendationService>();
builder.Services.AddScoped<IOpenRouterService, OpenRouterService>();

// Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
```

### Krok 7: Implementacja GlobalExceptionHandler
**Czas: 1 godzina**

7.1. Utworzenie `Middleware/GlobalExceptionHandler.cs` (patrz sekcja 7.3)

### Krok 8: Testy jednostkowe
**Czas: 3-4 godziny**

8.1. Testy dla `OpenRouterService`:
- Test poprawnego wywołania API
- Test parsowania odpowiedzi
- Test obsługi timeoutu
- Test obsługi błędów API

8.2. Testy dla `AIRecommendationService`:
- Test cache hit
- Test cache miss
- Test walidacji profilu
- Test walidacji technologii
- Test mapowania rekomendacji
- Test obsługi błędów

8.3. Testy dla `AIRecommendationsController`:
- Test sukcesu (200)
- Test błędów walidacji (400)
- Test not found (404)
- Test timeout (408)

### Krok 9: Testy integracyjne
**Czas: 2-3 godziny**

9.1. Test end-to-end z mock OpenRouter API
9.2. Test cache behavior
9.3. Test autoryzacji
9.4. Test performance (response time)

### Krok 10: Dokumentacja i deployment
**Czas: 1 godzina**

10.1. Aktualizacja Swagger documentation
10.2. Dodanie przykładów request/response
10.3. Dokumentacja konfiguracji OpenRouter
10.4. Deployment do środowiska testowego
10.5. Weryfikacja w środowisku produkcyjnym

---

## 10. Podsumowanie i checklist

### Checklist implementacji:

- [ ] Konfiguracja OpenRouter (appsettings.json, User Secrets)
- [ ] Pakiety NuGet (MemoryCache, HttpClient)
- [ ] Custom exceptions (6 typów)
- [ ] OpenRouterService (interface + implementacja)
- [ ] AIRecommendationService (interface + implementacja)
- [ ] AIRecommendationsController
- [ ] GlobalExceptionHandler
- [ ] Rejestracja w DI (Program.cs)
- [ ] Testy jednostkowe (min. 15 testów)
- [ ] Testy integracyjne (min. 5 testów)
- [ ] Dokumentacja Swagger
- [ ] Deployment i weryfikacja

### Szacowany czas implementacji:
**Łącznie: 15-20 godzin** (2-3 dni robocze)

### Kluczowe metryki sukcesu:
- ✅ Cache hit rate > 60%
- ✅ Response time (cache hit) < 100ms
- ✅ Response time (cache miss) < 15s
- ✅ Error rate < 2%
- ✅ Test coverage > 80%

### Potencjalne rozszerzenia (przyszłość):
1. Rate limiting (np. 10 requestów/godzinę na użytkownika)
2. Streaming AI responses (Server-Sent Events)
3. Personalizacja liczby rekomendacji (parametr w request)
4. Cache warming dla aktywnych użytkowników
5. A/B testing różnych promptów AI
6. Analytics i tracking użycia rekomendacji
7. Feedback loop (czy użytkownik dodał rekomendację)

