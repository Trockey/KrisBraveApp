# Plan Testów - DeveloperGoals

<analysis>
## Analiza projektu DeveloperGoals

### 1. Kluczowe komponenty i moduły

#### 1.1 Warstwa prezentacji (Blazor Components)
1. **Dashboard.razor** - Główna strona z wizualizacją grafu technologii
   - Wyświetlanie grafu z węzłami i krawędziami
   - Interakcja z węzłami (kliknięcie, wybór)
   - Integracja z GraphVisualizer przez JS Interop
   - Modale: NodeOptionsModal, RecommendationModal
   - Zarządzanie stanem grafu (odświeżanie, ładowanie)

2. **Profile.razor / Onboarding.razor** - Profil użytkownika
   - Formularz tworzenia/edycji profilu
   - Wybór głównych technologii (minimum 5)
   - Wybór roli (Programista, Tester, Analityk, Data Science Specialist)
   - Wybór obszaru rozwoju (UI, Backend, Full Stack, Testing, DevOps, Data Science)
   - Walidacja formularza

3. **Login.razor** - Strona logowania
   - Przycisk logowania Google OAuth
   - Przekierowanie do Google
   - Obsługa powrotu z OAuth

4. **GraphVisualizer.razor** - Komponent wizualizacji grafu
   - Integracja z vis.js przez JS Interop
   - Renderowanie węzłów i krawędzi
   - Obsługa interakcji (kliknięcie węzła, tło)
   - Kalkulacja poziomów węzłów

5. **NodeOptionsModal.razor** - Modal zarządzania węzłem
   - Edycja postępu (0-100%)
   - Edycja opisu prywatnego
   - Usuwanie technologii
   - Otwieranie rekomendacji AI

6. **RecommendationModal.razor** - Modal rekomendacji AI
   - Wyświetlanie rekomendacji z OpenRouter
   - Dodawanie technologii do grafu (pojedynczo lub batch)
   - Obsługa ignorowanych technologii

#### 1.2 Warstwa API (Controllers)
1. **AuthController** - Autentykacja
   - GET /login/google - inicjacja OAuth
   - GET /login/google-response - callback OAuth
   - Tworzenie/aktualizacja użytkownika w bazie

2. **ProfileController** - Profil użytkownika
   - GET /api/profile - pobranie profilu
   - POST /api/profile - utworzenie profilu
   - PUT /api/profile - aktualizacja profilu

3. **TechnologyController** - Technologie
   - GET /api/technologies - lista technologii
   - GET /api/technologies/graph - graf technologii
   - POST /api/technologies - dodanie technologii
   - POST /api/technologies/batch - dodanie batch
   - POST /api/technologies/custom - utworzenie własnej technologii
   - PATCH /api/technologies/{id} - aktualizacja technologii
   - DELETE /api/technologies/{id} - usunięcie technologii

4. **AIRecommendationsController** - Rekomendacje AI
   - POST /api/ai/recommendations - generowanie rekomendacji
   - Walidacja profilu, technologii źródłowej i kontekstowych
   - Cache in-memory (24h TTL)

5. **IgnoredTechnologyController** - Ignorowane technologie
   - GET /api/ignored-technologies - lista ignorowanych
   - POST /api/ignored-technologies - ignorowanie technologii
   - DELETE /api/ignored-technologies/{id} - przywrócenie technologii

#### 1.3 Warstwa serwisów (Services)
1. **TechnologyService** - Logika biznesowa technologii
   - Komunikacja z API przez HttpClient
   - Mapowanie DTO

2. **AIRecommendationService** - Logika rekomendacji AI
   - Walidacja profilu użytkownika
   - Walidacja technologii źródłowej i kontekstowych
   - Zarządzanie cache (IMemoryCache)
   - Wywołanie OpenRouterService
   - Mapowanie rekomendacji na TechnologyDefinitions

3. **OpenRouterService** - Komunikacja z OpenRouter API
   - Budowanie promptu na podstawie profilu
   - HTTP POST do OpenRouter API
   - Parsowanie odpowiedzi JSON
   - Obsługa timeoutów (20s)
   - Obsługa błędów HTTP

4. **UserStateService** - Stan użytkownika
   - Inicjalizacja stanu (IsAuthenticated, HasProfile)
   - Synchronizacja z API

5. **IgnoredTechnologyService** - Zarządzanie ignorowanymi technologiami
   - Komunikacja z API

#### 1.4 Warstwa danych (Data)
1. **ApplicationDbContext** - Kontekst EF Core
   - Konfiguracja relacji między encjami
   - Indeksy na kluczowych polach
   - Seed data (TechnologyDefinition "Start")

2. **Modele danych**:
   - User (BigInteger Id, GoogleId, Email, Name)
   - UserProfile (MainTechnologies, Role, DevelopmentArea)
   - UserTechnology (technologie w grafie użytkownika)
   - TechnologyDependency (krawędzie grafu)
   - TechnologyDefinition (wspólny słownik technologii)
   - IgnoredTechnology (ignorowane technologie)

#### 1.5 Integracje zewnętrzne
1. **Google OAuth 2.0** - Autentykacja
   - Microsoft.AspNetCore.Authentication.Google
   - Cookie-based sessions

2. **OpenRouter.ai API** - Rekomendacje AI
   - Model: anthropic/claude-3.5-sonnet
   - Timeout: 20 sekund
   - Max tokens: 2000
   - Temperature: 0.7

3. **vis.js** - Wizualizacja grafu
   - JavaScript Interop w Blazor
   - Renderowanie węzłów i krawędzi
   - Interakcje użytkownika

### 2. Frameworki i narzędzia testowe dla stosu technologicznego

#### 2.1 ASP.NET Core Blazor Server (.NET 9)
- **xUnit** - framework testowy (alternatywa: NUnit, MSTest)
- **Moq** lub **NSubstitute** - mockowanie zależności
- **bUnit** - testowanie komponentów Blazor
- **FluentAssertions** - asercje czytelne
- **Microsoft.AspNetCore.Mvc.Testing** - testy integracyjne API

#### 2.2 Entity Framework Core + PostgreSQL
- **Microsoft.EntityFrameworkCore.InMemory** - testy jednostkowe z bazą w pamięci
- **Testcontainers.PostgreSql** - testy integracyjne z rzeczywistą bazą PostgreSQL w kontenerze Docker
- **Respawn** - czyszczenie bazy danych między testami
- **Bogus** - generowanie danych testowych

#### 2.3 Google OAuth
- **Microsoft.AspNetCore.Authentication.Test** - mockowanie autentykacji
- **TestServer** z konfiguracją OAuth
- Mock ClaimsPrincipal dla testów autoryzacji

#### 2.4 OpenRouter.ai API
- **HttpClient mock** (Moq lub HttpClientFactory mock)
- **WireMock.NET** - mockowanie serwera HTTP dla testów integracyjnych
- **Polly** - testowanie obsługi błędów i retry

#### 2.5 vis.js / JavaScript Interop
- **bUnit** - testowanie komponentów z JS Interop
- **IJSRuntime mock** - mockowanie wywołań JavaScript
- **Playwright** lub **Selenium** - testy E2E z rzeczywistym przeglądarką

### 3. Krytyczne ścieżki funkcjonalne

1. **Onboarding nowego użytkownika**
   - Logowanie przez Google OAuth
   - Przekierowanie do /onboarding
   - Utworzenie profilu (min. 5 technologii)
   - Utworzenie węzła "Start" w grafie
   - Przekierowanie do /dashboard

2. **Dodawanie technologii do grafu**
   - Kliknięcie węzła na grafie
   - Otwarcie NodeOptionsModal
   - Wybór "Szukaj nowych technologii"
   - Wygenerowanie rekomendacji AI (OpenRouter)
   - Wybór i dodanie technologii (batch)
   - Odświeżenie grafu z nowymi węzłami i krawędziami

3. **Aktualizacja postępu technologii**
   - Kliknięcie węzła
   - Otwarcie NodeOptionsModal
   - Zmiana postępu (0-100%)
   - Zapisanie zmian
   - Aktualizacja wizualizacji (kolor węzła)

4. **Generowanie rekomendacji AI**
   - Walidacja profilu (kompletny)
   - Walidacja technologii źródłowej (istnieje, Active, należy do użytkownika)
   - Sprawdzenie cache
   - Wywołanie OpenRouter API
   - Parsowanie odpowiedzi JSON
   - Mapowanie na TechnologyDefinitions
   - Zwrócenie rekomendacji z flagą isAlreadyInGraph

5. **Usuwanie technologii**
   - Kliknięcie węzła (nie Start)
   - Otwarcie NodeOptionsModal
   - Usunięcie technologii
   - Kaskadowe usunięcie zależności
   - Odświeżenie grafu

### 4. Potencjalne obszary ryzyka

1. **Integracja z OpenRouter.ai API**
   - **Ryzyko**: Timeouty, błędy HTTP, nieprawidłowy format odpowiedzi JSON
   - **Uzasadnienie**: Zewnętrzna zależność, brak kontroli nad dostępnością i formatem odpowiedzi
   - **Testy**: Mockowanie odpowiedzi, testowanie timeoutów, walidacja parsowania JSON

2. **JavaScript Interop z vis.js**
   - **Ryzyko**: Problemy z synchronizacją między C# a JavaScript, błędy w renderowaniu grafu
   - **Uzasadnienie**: Komunikacja między dwoma środowiskami wykonawczymi, złożona logika wizualizacji
   - **Testy**: Mockowanie IJSRuntime, testy E2E z rzeczywistą przeglądarką

3. **Google OAuth Flow**
   - **Ryzyko**: Problemy z przekierowaniami, utrata stanu sesji, błędy autentykacji
   - **Uzasadnienie**: Zewnętrzna zależność, złożony flow OAuth z przekierowaniami
   - **Testy**: Testy integracyjne z mock OAuth, testy E2E z rzeczywistym Google OAuth

4. **Relacje w bazie danych (kaskadowe usuwanie)**
   - **Ryzyko**: Nieprawidłowe usuwanie danych, naruszenie integralności referencyjnej
   - **Uzasadnienie**: Złożone relacje między encjami (User → UserTechnology → TechnologyDependency)
   - **Testy**: Testy integracyjne z rzeczywistą bazą, weryfikacja kaskadowego usuwania

5. **Cache in-memory dla rekomendacji AI**
   - **Ryzyko**: Nieprawidłowe klucze cache, wygaśnięcie cache, problemy z współbieżnością
   - **Uzasadnienie**: Cache może zwracać nieaktualne dane lub powodować problemy z pamięcią
   - **Testy**: Testy jednostkowe cache, testy współbieżności

6. **Walidacja danych użytkownika**
   - **Ryzyko**: Nieprawidłowe dane w profilu, brak walidacji na poziomie API
   - **Uzasadnienie**: Dane wejściowe od użytkownika mogą być nieprawidłowe lub złośliwe
   - **Testy**: Testy walidacji, testy bezpieczeństwa (SQL injection, XSS)

7. **BigInteger jako ID użytkownika**
   - **Ryzyko**: Problemy z konwersją, przepełnienie, niekompatybilność z niektórymi bibliotekami
   - **Uzasadnienie**: Nietypowy typ dla ID, może powodować problemy w mapowaniu i serializacji
   - **Testy**: Testy konwersji, testy z dużymi wartościami

### 5. Rodzaje testów odpowiednie dla stosu technologicznego

1. **Testy jednostkowe (Unit Tests)**
   - Serwisy biznesowe (AIRecommendationService, TechnologyService)
   - Logika walidacji
   - Mapowanie DTO
   - Cache logic

2. **Testy integracyjne (Integration Tests)**
   - API Controllers z rzeczywistą bazą danych (Testcontainers)
   - Entity Framework Core z PostgreSQL
   - Integracja serwisów z bazą danych
   - Google OAuth flow (z mock)

3. **Testy komponentów Blazor (Component Tests)**
   - bUnit dla komponentów Razor
   - Mockowanie serwisów i zależności
   - Testowanie interakcji użytkownika (kliknięcia, formularze)

4. **Testy end-to-end (E2E Tests)**
   - Playwright lub Selenium
   - Pełny flow użytkownika (logowanie → onboarding → dashboard)
   - Integracja z vis.js w przeglądarce
   - Testy z rzeczywistym Google OAuth (w środowisku testowym)

5. **Testy wydajnościowe (Performance Tests)**
   - Load testing API (k6, NBomber)
   - Testy czasu odpowiedzi OpenRouter API
   - Testy obciążenia bazy danych

6. **Testy bezpieczeństwa (Security Tests)**
   - Autoryzacja i autentykacja
   - SQL injection
   - XSS w komponentach Blazor
   - CSRF protection

### 6. Struktura planu testów

Plan testów będzie zawierał:
1. Przegląd i zakres
2. Cele testowania
3. Strategia testowania
4. Kategorie testów (szczegółowe dla każdego typu)
5. Scenariusze testowe (konkretne przypadki)
6. Narzędzia i frameworki (szczegółowa lista)
7. Środowiska testowe
8. Kryteria akceptacji
</analysis>

<test_plan>
# Plan Testów - DeveloperGoals

## 1. Przegląd i zakres

### 1.1 Opis projektu
DeveloperGoals to aplikacja webowa do planowania ścieżki rozwoju programistów z wykorzystaniem sztucznej inteligencji. Aplikacja umożliwia użytkownikom:
- Tworzenie i zarządzanie profilem zawodowym
- Wizualizację grafu technologii do nauki
- Otrzymywanie inteligentnych rekomendacji technologii na podstawie profilu i aktualnego stanu grafu
- Śledzenie postępu nauki poszczególnych technologii

### 1.2 Zakres testowania
Plan testów obejmuje:
- **Funkcjonalność**: Wszystkie funkcje aplikacji od autentykacji po wizualizację grafu
- **Integracje**: Google OAuth, OpenRouter.ai API, vis.js
- **Baza danych**: Operacje CRUD, relacje, migracje
- **Wydajność**: Czas odpowiedzi API, obciążenie bazy danych
- **Bezpieczeństwo**: Autoryzacja, walidacja danych, ochrona przed atakami
- **Interfejs użytkownika**: Komponenty Blazor, interakcje, responsywność

### 1.3 Poziomy testowania
- Testy jednostkowe (Unit Tests)
- Testy integracyjne (Integration Tests)
- Testy komponentów (Component Tests)
- Testy end-to-end (E2E Tests)
- Testy wydajnościowe (Performance Tests)
- Testy bezpieczeństwa (Security Tests)

## 2. Cele testowania

### 2.1 Główne cele
1. **Zapewnienie jakości funkcjonalnej**: Weryfikacja, że wszystkie funkcje działają zgodnie z wymaganiami
2. **Weryfikacja integracji**: Potwierdzenie poprawnej współpracy wszystkich komponentów systemu
3. **Zapewnienie niezawodności**: Weryfikacja obsługi błędów i przypadków brzegowych
4. **Ochrona przed regresją**: Zapewnienie, że nowe zmiany nie psują istniejącej funkcjonalności
5. **Weryfikacja wydajności**: Zapewnienie akceptowalnego czasu odpowiedzi i wydajności
6. **Zapewnienie bezpieczeństwa**: Ochrona przed typowymi atakami i nieautoryzowanym dostępem

### 2.2 Metryki sukcesu
- **Pokrycie kodem**: Minimum 80% dla logiki biznesowej, 60% dla komponentów UI
- **Wskaźnik przejścia testów**: Minimum 95% testów przechodzi
- **Czas wykonania testów**: Wszystkie testy jednostkowe < 5 minut, integracyjne < 15 minut
- **Czas odpowiedzi API**: P95 < 500ms dla większości endpointów
- **Zero krytycznych błędów**: Brak błędów blokujących w środowisku produkcyjnym

## 3. Strategia testowania

### 3.1 Piramida testów
```
        /\
       /E2E\        (10%) - Testy end-to-end
      /------\
     /Integr. \     (30%) - Testy integracyjne
    /----------\
   /  Unit      \   (60%) - Testy jednostkowe
  /--------------\
```

### 3.2 Podejście do testowania
1. **Test-Driven Development (TDD)** dla nowych funkcji
2. **Testy jednostkowe** jako podstawa - szybkie, izolowane, łatwe w utrzymaniu
3. **Testy integracyjne** dla krytycznych ścieżek i integracji zewnętrznych
4. **Testy E2E** dla głównych user journeys
5. **Testy regresyjne** przed każdym release

### 3.3 Priorytetyzacja
**Priorytet 1 (Krytyczne)**:
- Autentykacja i autoryzacja
- Tworzenie i zarządzanie profilem
- Generowanie rekomendacji AI
- Operacje CRUD na technologiach
- Integracja z OpenRouter API

**Priorytet 2 (Wysokie)**:
- Wizualizacja grafu (vis.js)
- Batch operations
- Cache dla rekomendacji
- Walidacja danych

**Priorytet 3 (Średnie)**:
- UI/UX komponentów
- Obsługa błędów
- Performance optimization
- Edge cases

## 4. Kategorie testów

### 4.1 Testy jednostkowe

#### 4.1.1 Serwisy biznesowe
**AIRecommendationService**
- ✅ Walidacja profilu użytkownika (kompletny, niekompletny)
- ✅ Walidacja technologii źródłowej (istnieje, należy do użytkownika, status Active)
- ✅ Walidacja technologii kontekstowych (deduplikacja, właściciel)
- ✅ Cache hit/miss scenarios
- ✅ Mapowanie rekomendacji AI na TechnologyDefinitions
- ✅ Sprawdzanie isAlreadyInGraph
- ✅ Obsługa wyjątków (ProfileIncompleteException, TechnologyNotFoundException)

**TechnologyService**
- ✅ GetGraphAsync - pobieranie grafu
- ✅ AddTechnologyAsync - dodawanie pojedynczej technologii
- ✅ AddTechnologiesBatchAsync - dodawanie batch
- ✅ UpdateTechnologyAsync - aktualizacja technologii
- ✅ DeleteTechnologyAsync - usuwanie technologii
- ✅ Obsługa błędów HTTP

**OpenRouterService**
- ✅ Budowanie promptu na podstawie profilu
- ✅ Parsowanie odpowiedzi JSON (z markdown i bez)
- ✅ Obsługa timeoutów (20s)
- ✅ Obsługa błędów HTTP (500, 503, timeout)
- ✅ Walidacja formatu odpowiedzi (dokładnie 3 rekomendacje)
- ✅ Obsługa nieprawidłowego JSON

**UserStateService**
- ✅ Inicjalizacja stanu (IsAuthenticated, HasProfile)
- ✅ Synchronizacja z API
- ✅ Obsługa błędów inicjalizacji

#### 4.1.2 Kontrolery API
**AuthController**
- ✅ LoginGoogle - zwraca redirect URL
- ✅ GoogleResponse - tworzenie nowego użytkownika
- ✅ GoogleResponse - aktualizacja istniejącego użytkownika
- ✅ Obsługa brakujących danych z Google OAuth

**ProfileController**
- ✅ GetProfile - pobieranie profilu (istnieje, nie istnieje)
- ✅ CreateProfile - tworzenie profilu (walidacja, zapis)
- ✅ UpdateProfile - aktualizacja profilu
- ✅ Walidacja danych wejściowych

**TechnologyController**
- ✅ GetTechnologies - lista technologii użytkownika
- ✅ GetGraph - graf technologii (węzły, krawędzie, statystyki)
- ✅ AddTechnology - dodanie technologii
- ✅ BatchAdd - dodanie batch technologii
- ✅ UpdateTechnology - aktualizacja (postęp, opis)
- ✅ DeleteTechnology - usunięcie (z kaskadowym usuwaniem zależności)
- ✅ CreateCustomTechnology - utworzenie własnej technologii

**AIRecommendationsController**
- ✅ GenerateRecommendations - generowanie rekomendacji
- ✅ Walidacja request (fromTechnologyId, contextTechnologyIds)
- ✅ Obsługa wyjątków (ProfileIncompleteException, TechnologyNotFoundException)
- ✅ Zwracanie odpowiedzi z cache

#### 4.1.3 Modele i DTO
- ✅ Mapowanie User → UserDto
- ✅ Mapowanie UserTechnology → TechnologyDto
- ✅ Mapowanie TechnologyGraphDto (węzły, krawędzie, statystyki)
- ✅ Walidacja DataAnnotations

#### 4.1.4 Entity Framework Core
- ✅ Konfiguracja relacji (1:1, 1:N)
- ✅ Kaskadowe usuwanie
- ✅ Indeksy na kluczowych polach
- ✅ Seed data (TechnologyDefinition "Start")

### 4.2 Testy integracyjne

#### 4.2.1 API + Baza danych
**Testcontainers.PostgreSql**
- ✅ Tworzenie użytkownika przez OAuth flow
- ✅ Tworzenie profilu użytkownika
- ✅ Dodawanie technologii do grafu
- ✅ Tworzenie zależności między technologiami
- ✅ Kaskadowe usuwanie (User → UserTechnology → TechnologyDependency)
- ✅ Batch operations
- ✅ Transakcje (rollback przy błędzie)

**Migracje**
- ✅ Weryfikacja migracji (up, down)
- ✅ Seed data po migracji
- ✅ Indeksy po migracji

#### 4.2.2 Integracja serwisów
- ✅ AIRecommendationService → OpenRouterService (z mock HTTP)
- ✅ TechnologyService → API Controller → Database
- ✅ UserStateService → ProfileController

#### 4.2.3 Google OAuth (z mock)
- ✅ Flow logowania (redirect → callback → tworzenie sesji)
- ✅ Tworzenie nowego użytkownika
- ✅ Aktualizacja istniejącego użytkownika
- ✅ Obsługa błędów OAuth

### 4.3 Testy komponentów Blazor

#### 4.3.1 Dashboard.razor
- ✅ Inicjalizacja komponentu (OnInitializedAsync)
- ✅ Ładowanie grafu (LoadGraphAsync)
- ✅ Wyświetlanie stanu ładowania
- ✅ Wyświetlanie stanu błędu
- ✅ Wyświetlanie pustego grafu
- ✅ Kliknięcie węzła (HandleNodeClick)
- ✅ Kliknięcie tła (HandleBackgroundClick)
- ✅ Odświeżanie grafu (RefreshGraph)
- ✅ Otwieranie/zamykanie modali
- ✅ Przekierowanie niezalogowanego użytkownika

#### 4.3.2 Profile.razor / Onboarding.razor
- ✅ Wyświetlanie formularza
- ✅ Walidacja formularza (min. 5 technologii)
- ✅ Zapisanie profilu
- ✅ Przekierowanie po zapisaniu
- ✅ Wyświetlanie błędów walidacji

#### 4.3.3 GraphVisualizer.razor
- ✅ Renderowanie węzłów i krawędzi
- ✅ Kalkulacja poziomów węzłów
- ✅ Obsługa kliknięcia węzła (JS Interop)
- ✅ Obsługa kliknięcia tła
- ✅ Aktualizacja grafu (Refresh)

#### 4.3.4 NodeOptionsModal.razor
- ✅ Wyświetlanie danych węzła
- ✅ Edycja postępu (0-100%)
- ✅ Edycja opisu prywatnego
- ✅ Zapisanie zmian
- ✅ Usunięcie technologii
- ✅ Otwieranie rekomendacji

#### 4.3.5 RecommendationModal.razor
- ✅ Wyświetlanie rekomendacji
- ✅ Dodawanie pojedynczej technologii
- ✅ Dodawanie batch technologii
- ✅ Ignorowanie technologii
- ✅ Obsługa stanu ładowania

### 4.4 Testy end-to-end

#### 4.4.1 Scenariusz: Onboarding nowego użytkownika
1. ✅ Użytkownik wchodzi na stronę główną
2. ✅ Przekierowanie do /login
3. ✅ Kliknięcie "Zaloguj się przez Google"
4. ✅ Przekierowanie do Google OAuth
5. ✅ Logowanie w Google
6. ✅ Powrót do aplikacji (/login/google-response)
7. ✅ Przekierowanie do /onboarding
8. ✅ Wypełnienie formularza profilu (min. 5 technologii)
9. ✅ Zapisanie profilu
10. ✅ Przekierowanie do /dashboard
11. ✅ Wyświetlenie grafu z węzłem "Start"

#### 4.4.2 Scenariusz: Dodawanie technologii przez AI
1. ✅ Zalogowany użytkownik na /dashboard
2. ✅ Kliknięcie węzła "Start"
3. ✅ Otwarcie NodeOptionsModal
4. ✅ Kliknięcie "Szukaj nowych technologii"
5. ✅ Otwarcie RecommendationModal
6. ✅ Wygenerowanie rekomendacji AI (POST /api/ai/recommendations)
7. ✅ Wyświetlenie 3 rekomendacji
8. ✅ Wybór 2 technologii
9. ✅ Dodanie technologii (POST /api/technologies/batch)
10. ✅ Odświeżenie grafu
11. ✅ Wyświetlenie nowych węzłów i krawędzi na grafie

#### 4.4.3 Scenariusz: Aktualizacja postępu
1. ✅ Zalogowany użytkownik na /dashboard
2. ✅ Kliknięcie węzła technologii
3. ✅ Otwarcie NodeOptionsModal
4. ✅ Zmiana postępu z 0% na 50%
5. ✅ Zapisanie zmian (PATCH /api/technologies/{id})
6. ✅ Odświeżenie grafu
7. ✅ Weryfikacja zmiany koloru węzła (żółty)

#### 4.4.4 Scenariusz: Usuwanie technologii
1. ✅ Zalogowany użytkownik z grafem zawierającym kilka technologii
2. ✅ Kliknięcie węzła (nie Start)
3. ✅ Otwarcie NodeOptionsModal
4. ✅ Kliknięcie "Usuń"
5. ✅ Potwierdzenie usunięcia
6. ✅ Usunięcie technologii (DELETE /api/technologies/{id})
7. ✅ Kaskadowe usunięcie zależności
8. ✅ Odświeżenie grafu
9. ✅ Weryfikacja usunięcia węzła i krawędzi

### 4.5 Testy wydajnościowe

#### 4.5.1 API Performance
- ✅ GET /api/technologies/graph - P95 < 200ms (dla użytkownika z 100 technologiami)
- ✅ POST /api/ai/recommendations - P95 < 2s (z cache), P95 < 5s (bez cache)
- ✅ POST /api/technologies/batch - P95 < 500ms (dla 10 technologii)
- ✅ GET /api/profile - P95 < 100ms

#### 4.5.2 Baza danych
- ✅ Zapytania z JOIN (graf technologii) - optymalizacja
- ✅ Indeksy na kluczowych polach (UserId, GoogleId)
- ✅ Batch operations - wydajność

#### 4.5.3 Cache
- ✅ Cache hit rate > 80% dla powtarzających się zapytań
- ✅ Cache expiration (24h TTL)
- ✅ Memory usage (max 1000 entries)

### 4.6 Testy bezpieczeństwa

#### 4.6.1 Autoryzacja i autentykacja
- ✅ Ochrona endpointów API ([Authorize])
- ✅ Przekierowanie niezalogowanego użytkownika
- ✅ Walidacja sesji (cookie expiration)
- ✅ Ochrona przed CSRF (Antiforgery tokens)

#### 4.6.2 Walidacja danych
- ✅ SQL injection protection (parametryzowane zapytania EF Core)
- ✅ XSS protection (encoding w Blazor)
- ✅ Walidacja długości pól (MaxLength)
- ✅ Walidacja zakresów (Progress 0-100%)

#### 4.6.3 Autoryzacja zasobów
- ✅ Użytkownik może modyfikować tylko swoje technologie
- ✅ Użytkownik nie może zobaczyć technologii innych użytkowników
- ✅ Walidacja UserId w kontrolerach

## 5. Scenariusze testowe

### 5.1 Autentykacja

**TC-AUTH-001: Logowanie nowego użytkownika przez Google OAuth**
- **Warunki wstępne**: Użytkownik nie jest zalogowany
- **Kroki**:
  1. Wejdź na /login
  2. Kliknij "Zaloguj się przez Google"
  3. Zaloguj się w Google
  4. Wróć do aplikacji
- **Oczekiwany rezultat**: 
  - Użytkownik jest zalogowany
  - Utworzony rekord w tabeli Users
  - Przekierowanie do /onboarding (jeśli brak profilu) lub /dashboard

**TC-AUTH-002: Logowanie istniejącego użytkownika**
- **Warunki wstępne**: Użytkownik istnieje w bazie
- **Kroki**: Jak TC-AUTH-001
- **Oczekiwany rezultat**:
  - Użytkownik jest zalogowany
  - Zaktualizowany LastLoginAt w bazie
  - Przekierowanie do /dashboard

**TC-AUTH-003: Ochrona przed nieautoryzowanym dostępem**
- **Warunki wstępne**: Użytkownik nie jest zalogowany
- **Kroki**: Próba dostępu do /dashboard
- **Oczekiwany rezultat**: Przekierowanie do /login

### 5.2 Profil użytkownika

**TC-PROFILE-001: Tworzenie profilu (onboarding)**
- **Warunki wstępne**: Zalogowany użytkownik bez profilu
- **Kroki**:
  1. Wejdź na /onboarding
  2. Wybierz 5 technologii
  3. Wybierz rolę: Programista
  4. Wybierz obszar: Backend
  5. Kliknij "Rozpocznij przygodę!"
- **Oczekiwany rezultat**:
  - Utworzony profil w bazie
  - Utworzony węzeł "Start" w grafie
  - Przekierowanie do /dashboard

**TC-PROFILE-002: Walidacja profilu - za mało technologii**
- **Warunki wstępne**: Zalogowany użytkownik bez profilu
- **Kroki**:
  1. Wejdź na /onboarding
  2. Wybierz 4 technologie (mniej niż minimum)
  3. Kliknij "Rozpocznij przygodę!"
- **Oczekiwany rezultat**: 
  - Błąd walidacji: "Wybierz minimum 5 technologii"
  - Formularz nie jest zapisany

**TC-PROFILE-003: Aktualizacja profilu**
- **Warunki wstępne**: Zalogowany użytkownik z profilem
- **Kroki**:
  1. Wejdź na /profile
  2. Zmień rolę na "Tester"
  3. Dodaj 2 technologie
  4. Kliknij "Zapisz"
- **Oczekiwany rezultat**:
  - Zaktualizowany profil w bazie
  - Zaktualizowany UpdatedAt

### 5.3 Graf technologii

**TC-GRAPH-001: Wyświetlanie pustego grafu**
- **Warunki wstępne**: Użytkownik z profilem, ale bez technologii w grafie
- **Kroki**: Wejdź na /dashboard
- **Oczekiwany rezultat**: 
  - Wyświetlony komunikat "Twój graf jest pusty"
  - Link do /profile

**TC-GRAPH-002: Wyświetlanie grafu z węzłem Start**
- **Warunki wstępne**: Użytkownik z profilem, węzeł Start w grafie
- **Kroki**: Wejdź na /dashboard
- **Oczekiwany rezultat**:
  - Wyświetlony graf z 1 węzłem "Start"
  - Komunikat informacyjny o kliknięciu węzła

**TC-GRAPH-003: Wyświetlanie grafu z wieloma technologiami**
- **Warunki wstępne**: Użytkownik z 10 technologiami w grafie
- **Kroki**: Wejdź na /dashboard
- **Oczekiwany rezultat**:
  - Wyświetlony graf z 10 węzłami
  - Wyświetlone krawędzie między technologiami
  - Statystyki: 10 technologii, średni postęp

### 5.4 Rekomendacje AI

**TC-AI-001: Generowanie rekomendacji dla nowego użytkownika**
- **Warunki wstępne**: 
  - Zalogowany użytkownik z kompletnym profilem
  - Węzeł Start w grafie
- **Kroki**:
  1. Kliknij węzeł Start
  2. Kliknij "Szukaj nowych technologii"
  3. Poczekaj na rekomendacje
- **Oczekiwany rezultat**:
  - Wyświetlone 3 rekomendacje technologii
  - Każda rekomendacja zawiera: name, prefix, tag, systemDescription, aiReasoning
  - Rekomendacje są logiczne dla profilu użytkownika

**TC-AI-002: Cache dla rekomendacji**
- **Warunki wstępne**: Jak TC-AI-001, ale rekomendacje były już generowane
- **Kroki**: Jak TC-AI-001
- **Oczekiwany rezultat**:
  - Rekomendacje zwrócone z cache (szybciej)
  - Flaga FromCache = true w odpowiedzi

**TC-AI-003: Błąd - niekompletny profil**
- **Warunki wstępne**: Zalogowany użytkownik bez profilu
- **Kroki**: Próba wygenerowania rekomendacji
- **Oczekiwany rezultat**:
  - Błąd 400: ProfileIncompleteException
  - Komunikat: "Please complete your profile first"

**TC-AI-004: Timeout OpenRouter API**
- **Warunki wstępne**: Zalogowany użytkownik z profilem
- **Kroki**: Wygenerowanie rekomendacji (z mock timeout)
- **Oczekiwany rezultat**:
  - Błąd 408: AIServiceTimeoutException
  - Komunikat: "AI service did not respond within 20 seconds"

### 5.5 Zarządzanie technologiami

**TC-TECH-001: Dodawanie pojedynczej technologii**
- **Warunki wstępne**: Zalogowany użytkownik z grafem
- **Kroki**:
  1. Otwórz RecommendationModal
  2. Wybierz 1 technologię
  3. Kliknij "Dodaj"
- **Oczekiwany rezultat**:
  - Technologia dodana do grafu
  - Utworzona zależność od węzła źródłowego
  - Odświeżony graf

**TC-TECH-002: Batch dodawanie technologii**
- **Warunki wstępne**: Jak TC-TECH-001
- **Kroki**:
  1. Otwórz RecommendationModal
  2. Wybierz 5 technologii
  3. Kliknij "Dodaj wszystkie"
- **Oczekiwany rezultat**:
  - 5 technologii dodanych do grafu
  - Utworzone zależności
  - Odświeżony graf

**TC-TECH-003: Aktualizacja postępu**
- **Warunki wstępne**: Zalogowany użytkownik z technologią w grafie
- **Kroki**:
  1. Kliknij węzeł technologii
  2. Otwórz NodeOptionsModal
  3. Zmień postęp na 75%
  4. Kliknij "Zapisz"
- **Oczekiwany rezultat**:
  - Zaktualizowany postęp w bazie
  - Zmiana koloru węzła (niebieski)
  - Odświeżony graf

**TC-TECH-004: Usuwanie technologii**
- **Warunki wstępne**: Zalogowany użytkownik z technologią w grafie (nie Start)
- **Kroki**:
  1. Kliknij węzeł technologii
  2. Otwórz NodeOptionsModal
  3. Kliknij "Usuń"
  4. Potwierdź usunięcie
- **Oczekiwany rezultat**:
  - Technologia usunięta z grafu
  - Kaskadowe usunięcie zależności
  - Odświeżony graf

**TC-TECH-005: Próba usunięcia węzła Start**
- **Warunki wstępne**: Zalogowany użytkownik z węzłem Start
- **Kroki**: Próba usunięcia węzła Start
- **Oczekiwany rezultat**:
  - Błąd: "Nie można usunąć węzła Start"
  - Węzeł Start pozostaje w grafie

## 6. Narzędzia i frameworki

### 6.1 Frameworki testowe

**xUnit** - Framework testowy dla .NET
- Wersja: 2.6.0+
- Użycie: Wszystkie testy jednostkowe i integracyjne
- Konfiguracja: `xunit.runner.json`

**bUnit** - Testowanie komponentów Blazor
- Wersja: 1.23.0+
- Użycie: Testy komponentów Razor
- Konfiguracja: Mockowanie serwisów, IJSRuntime

**Moq** - Mockowanie zależności
- Wersja: 4.20.0+
- Użycie: Mockowanie serwisów, HttpClient, IMemoryCache

**FluentAssertions** - Czytelne asercje
- Wersja: 6.12.0+
- Użycie: Wszystkie testy

### 6.2 Narzędzia do testów integracyjnych

**Testcontainers.PostgreSql** - PostgreSQL w Docker
- Wersja: 3.7.0+
- Użycie: Testy integracyjne z bazą danych
- Konfiguracja: Automatyczne tworzenie/usuwanie kontenera

**Microsoft.AspNetCore.Mvc.Testing** - Testowanie API
- Wersja: 9.0.0+
- Użycie: Testy integracyjne kontrolerów
- Konfiguracja: WebApplicationFactory

**Respawn** - Czyszczenie bazy danych
- Wersja: 6.0.0+
- Użycie: Reset bazy między testami

### 6.3 Narzędzia do testów E2E

**Playwright** - Testy end-to-end
- Wersja: 1.40.0+
- Użycie: Testy E2E z rzeczywistą przeglądarką
- Konfiguracja: Headless Chrome/Firefox

**Selenium** (alternatywa) - Testy E2E
- Wersja: 4.15.0+
- Użycie: Jeśli Playwright nie wystarczy

### 6.4 Narzędzia do testów wydajnościowych

**k6** - Load testing
- Wersja: Latest
- Użycie: Testy obciążenia API
- Konfiguracja: Skrypty JavaScript

**NBomber** - Load testing dla .NET
- Wersja: 3.0.0+
- Użycie: Alternatywa dla k6

### 6.5 Narzędzia pomocnicze

**Bogus** - Generowanie danych testowych
- Wersja: 35.6.0+
- Użycie: Generowanie użytkowników, technologii

**WireMock.NET** - Mockowanie serwerów HTTP
- Wersja: 1.5.0+
- Użycie: Mockowanie OpenRouter API w testach integracyjnych

**Polly** - Obsługa błędów i retry
- Wersja: 8.3.0+
- Użycie: Testowanie resilience patterns

### 6.6 Narzędzia do analizy pokrycia

**coverlet** - Code coverage
- Wersja: 6.0.0+
- Użycie: Pomiar pokrycia kodu
- Integracja: z xUnit

**ReportGenerator** - Raporty coverage
- Wersja: 5.2.0+
- Użycie: Generowanie raportów HTML

## 7. Środowiska testowe

### 7.1 Środowisko lokalne (Development)
- **Aplikacja**: Localhost (https://localhost:7123)
- **Baza danych**: PostgreSQL lokalna lub Docker
- **OpenRouter API**: Mock (WireMock) lub rzeczywiste API z testowym kluczem
- **Google OAuth**: Testowe credentials z Google Cloud Console
- **Przeglądarka**: Chrome/Firefox (dla testów E2E)

### 7.2 Środowisko testowe (Test/Staging)
- **Aplikacja**: Serwer testowy (np. Azure App Service)
- **Baza danych**: PostgreSQL testowa (izolowana)
- **OpenRouter API**: Rzeczywiste API z testowym kluczem
- **Google OAuth**: Testowe credentials
- **Dane**: Automatyczne seedowanie danych testowych

### 7.3 Środowisko CI/CD
- **Platforma**: GitHub Actions / Azure DevOps
- **Baza danych**: Testcontainers.PostgreSql (Docker)
- **OpenRouter API**: Mock (WireMock)
- **Google OAuth**: Mock
- **Wykonanie**: Automatyczne przy każdym PR i commit

### 7.4 Wymagania środowiskowe
- **.NET 9 SDK**: Wersja 9.0.0+
- **PostgreSQL**: Wersja 16+ (lub Docker)
- **Docker**: Dla Testcontainers (opcjonalnie)
- **Node.js**: Dla k6 (opcjonalnie)

## 8. Kryteria akceptacji

### 8.1 Kryteria funkcjonalne
- ✅ Wszystkie testy jednostkowe przechodzą (100%)
- ✅ Wszystkie testy integracyjne przechodzą (100%)
- ✅ Wszystkie testy komponentów przechodzą (100%)
- ✅ Wszystkie testy E2E dla krytycznych ścieżek przechodzą (100%)
- ✅ Zero krytycznych błędów (P0, P1)
- ✅ Maksymalnie 5 błędów średniej ważności (P2) na release

### 8.2 Kryteria jakościowe
- ✅ Pokrycie kodem: Minimum 80% dla logiki biznesowej
- ✅ Pokrycie kodem: Minimum 60% dla komponentów UI
- ✅ Zero duplikacji kodu testowego (DRY principle)
- ✅ Wszystkie testy są deterministyczne (nie flaky)

### 8.3 Kryteria wydajnościowe
- ✅ GET /api/technologies/graph: P95 < 200ms
- ✅ POST /api/ai/recommendations: P95 < 2s (z cache), P95 < 5s (bez cache)
- ✅ POST /api/technologies/batch: P95 < 500ms
- ✅ Cache hit rate > 80% dla powtarzających się zapytań

### 8.4 Kryteria bezpieczeństwa
- ✅ Wszystkie endpointy API chronione autoryzacją
- ✅ Walidacja danych wejściowych na wszystkich endpointach
- ✅ Ochrona przed SQL injection (parametryzowane zapytania)
- ✅ Ochrona przed XSS (encoding w Blazor)
- ✅ CSRF protection (Antiforgery tokens)

### 8.5 Kryteria utrzymania
- ✅ Wszystkie testy mają czytelne nazwy i opisy
- ✅ Testy są zorganizowane w logiczne grupy
- ✅ Dokumentacja testów jest aktualna
- ✅ Czas wykonania wszystkich testów < 15 minut

### 8.6 Proces akceptacji
1. **Code Review**: Wszystkie testy muszą być zrecenzowane
2. **CI/CD Pipeline**: Wszystkie testy muszą przejść w pipeline
3. **Manual Testing**: Krytyczne ścieżki przetestowane manualnie przed release
4. **Sign-off**: QA Lead i Tech Lead zatwierdzają release

</test_plan>