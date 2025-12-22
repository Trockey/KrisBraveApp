Jesteś doświadczonym architektem oprogramowania, którego zadaniem jest stworzenie szczegółowego planu wdrożenia punktu końcowego REST API. Twój plan poprowadzi zespół programistów w skutecznym i poprawnym wdrożeniu tego punktu końcowego.

Zanim zaczniemy, zapoznaj się z poniższymi informacjami:

1. Route API specification:
<route_api_specification>

      #### POST /api/ai/recommendations
      **Description:** Generates AI recommendations for next technologies to learn. Uses in-memory cache (24h TTL). No rate limiting.  
      **Authentication:** Required

      **Request Body:**
      ```json
      {
        "fromTechnologyId": 1002,
        "contextTechnologyIds": [1001, 1002, 1003]
      }
      ```

      **Validation Rules:**
      - `fromTechnologyId`: Required, technology must belong to user
      - `contextTechnologyIds`: Optional, array of user's technology IDs for additional context
      - User must have completed profile

      **Response 200:**
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
          },
          {
            "technologyDefinitionId": 67,
            "name": "PostgreSQL - Indexing Strategies",
            "prefix": "PostgreSQL",
            "tag": "BazaDanych",
            "systemDescription": "Performance optimization through proper database indexing",
            "aiReasoning": "Complements Entity Framework knowledge, highly relevant for backend developers",
            "isAlreadyInGraph": false
          }
        ],
        "count": 10,
        "fromCache": false,
        "cacheExpiresAt": "2025-12-02T14:30:00Z",
        "generatedAt": "2025-12-01T14:30:00Z"
      }
      ```

      **Response 400:**
      ```json
      {
        "error": "ValidationError",
        "message": "Profile is incomplete. Please complete your profile first."
      }
      ```

      **Response 404:**
      ```json
      {
        "error": "NotFound",
        "message": "Source technology not found"
      }
      ```

      **Response 408:**
      ```json
      {
        "error": "Timeout",
        "message": "AI service did not respond within 20 seconds. Please try again."
      }
      ```

      **Response 500:**
      ```json
      {
        "error": "AIServiceError",
        "message": "Failed to generate recommendations. Please try again later.",
        "details": "OpenRouter API returned error 503"
      }
      ```

      **Response 502:**
      ```json
      {
        "error": "BadGateway",
        "message": "AI service returned invalid response format"
      }
      ```

</route_api_specification>

2. Related database resources:
<related_db_resources>

      Kluczowe tabele i ich relacje

      ##### **A. Warstwa użytkowników**

      **Users** (tabela główna)
      ```
      - Id: int (PK)
      - GoogleId: varchar(255) UNIQUE, NOT NULL, indexed
      - Email: varchar(255), NOT NULL, indexed
      - Name: varchar(255), NOT NULL (DisplayName z Google)
      - CreatedAt: timestamp, NOT NULL
      - LastLoginAt: timestamp, NULL
      ```

      **UserProfiles** (1:1 z Users)
      ```
      - Id: int (PK)
      - UserId: int (FK → Users, UNIQUE, CASCADE)
      - MainTechnologies: varchar(1000), NOT NULL (CSV lub lista oddzielona przecinkami)
      - Role: int (enum → Programista, Tester, Analityk, DataScienceSpecialist)
      - DevelopmentArea: int (enum → UserInterface, Backend, FullStack, Testing, DataScience, DevOps)
      - CreatedAt: timestamp, NOT NULL
      - UpdatedAt: timestamp, NULL
      ```

      ##### **B. Warstwa technologii**

      **TechnologyDefinitions** (słownik wspólnych technologii)
      ```
      - Id: int (PK)
      - Name: varchar(255), NOT NULL (np. "DotNet - Entity Framework")
      - Prefix: varchar(100), NOT NULL (np. "DotNet", "Java", "JavaScript")
      - Tag: int (enum → Technologia, Framework, BazaDanych, Metodologia, Narzedzie)
      - SystemDescription: varchar(1000), NOT NULL (opis z AI)
      - CreatedAt: timestamp, NOT NULL
      - UNIQUE na (Name, Prefix, Tag) - jedna definicja
      - Specjalny rekord: Id=1, Name="Start" dla węzła startowego
      ```

      **UserTechnologies** (instancje technologii użytkownika)
      ```
      - Id: int (PK)
      - UserId: int (FK → Users, CASCADE)
      - TechnologyDefinitionId: int (FK → TechnologyDefinitions, RESTRICT)
      - PrivateDescription: varchar(2000), NULL (notatki użytkownika)
      - Progress: int (0-100), NOT NULL, default 0
      - Status: int (enum → Active, Ignored)
      - IsCustom: bool, NOT NULL, default false (dodana ręcznie vs AI)
      - IsStart: bool, NOT NULL, default false (węzeł startowy)
      - AiReasoning: varchar(1000), NULL (dlaczego AI polecił)
      - CreatedAt: timestamp, NOT NULL
      - UpdatedAt: timestamp, NULL
      - Index na (UserId, Status)
      - Constraint: CHECK (Progress >= 0 AND Progress <= 100)
      - Constraint: nie można usunąć jeśli IsStart = true (logika aplikacji)
      ```

      ##### **C. Warstwa grafu**

      **TechnologyDependencies** (krawędzie grafu)
      ```
      - Id: int (PK)
      - UserId: int (FK → Users, CASCADE)
      - FromTechnologyId: int (FK → UserTechnologies, RESTRICT), NULL (dla Start)
      - ToTechnologyId: int (FK → UserTechnologies, CASCADE), NOT NULL
      - CreatedAt: timestamp, NOT NULL (dla sortowania kolejności)
      - UNIQUE na (UserId, FromTechnologyId, ToTechnologyId)
      - Index na UserId
      - Index na FromTechnologyId
      ```

      **IgnoredTechnologies** (archiwum ignorowanych)
      ```
      - Id: int (PK)
      - UserId: int (FK → Users, CASCADE)
      - Name: varchar(255), NOT NULL
      - Category: varchar(100), NOT NULL
      - Tag: int (enum), NOT NULL
      - SystemDescription: varchar(1000), NOT NULL
      - AiReasoning: varchar(1000), NULL
      - ContextTechnologyId: int (FK → UserTechnologies, SET NULL), NULL
      - IgnoredAt: timestamp, NOT NULL
      - UNIQUE na (UserId, Name, Category)
      - Index na UserId
      ```

      ##### **D. Relacje między encjami**

      ```
      Users 1───1 UserProfiles
      Users 1───N UserTechnologies
      Users 1───N TechnologyDependencies
      Users 1───N IgnoredTechnologies
      TechnologyDefinitions 1───N UserTechnologies
      UserTechnologies 1───N TechnologyDependencies (jako FromTechnology)
      UserTechnologies 1───N TechnologyDependencies (jako ToTechnology)
      UserTechnologies 1───N IgnoredTechnologies (jako ContextTechnology, opcjonalne)
      ```


</related_db_resources>

3. Definicje typów:
<type_definitions>
Get from file @Types.cs
</type_definitions>

3. Tech stack:
<tech_stack>
@tech-stack.md
</tech_stack>

4. Implementation rules:
<implementation_rules>

@backend.mdc

</implementation_rules>

Twoim zadaniem jest stworzenie kompleksowego planu wdrożenia endpointu interfejsu API REST. Przed dostarczeniem ostatecznego planu użyj znaczników <analysis>, aby przeanalizować informacje i nakreślić swoje podejście. W tej analizie upewnij się, że:

1. Podsumuj kluczowe punkty specyfikacji API.
2. Wymień wymagane i opcjonalne parametry ze specyfikacji API.
3. Wymień niezbędne typy DTO i Command Modele.
4. Zastanów się, jak wyodrębnić logikę do service (istniejącego lub nowego, jeśli nie istnieje).
5. Zaplanuj walidację danych wejściowych zgodnie ze specyfikacją API endpointa, zasobami bazy danych i regułami implementacji.
6. Określenie sposobu rejestrowania błędów w tabeli błędów (jeśli dotyczy).
7. Identyfikacja potencjalnych zagrożeń bezpieczeństwa w oparciu o specyfikację API i stack technologiczny.
8. Nakreśl potencjalne scenariusze błędów i odpowiadające im kody stanu.

Po przeprowadzeniu analizy utwórz szczegółowy plan wdrożenia w formacie markdown. Plan powinien zawierać następujące sekcje:

1. Przegląd punktu końcowego
2. Szczegóły żądania
3. Szczegóły odpowiedzi
4. Przepływ danych
5. Względy bezpieczeństwa
6. Obsługa błędów
7. Wydajność
8. Kroki implementacji

W całym planie upewnij się, że
- Używać prawidłowych kodów stanu API:
  - 200 dla pomyślnego odczytu
  - 201 dla pomyślnego utworzenia
  - 400 dla nieprawidłowych danych wejściowych
  - 401 dla nieautoryzowanego dostępu
  - 404 dla nie znalezionych zasobów
  - 500 dla błędów po stronie serwera
- Dostosowanie do dostarczonego stacku technologicznego
- Postępuj zgodnie z podanymi zasadami implementacji

Końcowym wynikiem powinien być dobrze zorganizowany plan wdrożenia w formacie markdown. Oto przykład tego, jak powinny wyglądać dane wyjściowe:

``markdown
# API Endpoint Implementation Plan: [Nazwa punktu końcowego]

## 1. Przegląd punktu końcowego
[Krótki opis celu i funkcjonalności punktu końcowego]

## 2. Szczegóły żądania
- Metoda HTTP: [GET/POST/PUT/DELETE]
- Struktura URL: [wzorzec URL]
- Parametry:
  - Wymagane: [Lista wymaganych parametrów]
  - Opcjonalne: [Lista opcjonalnych parametrów]
- Request Body: [Struktura treści żądania, jeśli dotyczy]

## 3. Wykorzystywane typy
[DTOs i Command Modele niezbędne do implementacji]

## 3. Szczegóły odpowiedzi
[Oczekiwana struktura odpowiedzi i kody statusu]

## 4. Przepływ danych
[Opis przepływu danych, w tym interakcji z zewnętrznymi usługami lub bazami danych]

## 5. Względy bezpieczeństwa
[Szczegóły uwierzytelniania, autoryzacji i walidacji danych]

## 6. Obsługa błędów
[Lista potencjalnych błędów i sposób ich obsługi]

## 7. Rozważania dotyczące wydajności
[Potencjalne wąskie gardła i strategie optymalizacji]

## 8. Etapy wdrożenia
1. [Krok 1]
2. [Krok 2]
3. [Krok 3]
...
```

Końcowe wyniki powinny składać się wyłącznie z planu wdrożenia w formacie markdown i nie powinny powielać ani powtarzać żadnej pracy wykonanej w sekcji analizy.

Pamiętaj, aby zapisać swój plan wdrożenia jako .ai/view-implementation-plan.md. Upewnij się, że plan jest szczegółowy, przejrzysty i zapewnia kompleksowe wskazówki dla zespołu programistów.