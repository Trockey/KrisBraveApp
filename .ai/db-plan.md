<conversation_summary>

# Podsumowanie planowania bazy danych dla DeveloperGoals MVP

## <decisions>

### Decyzje podjęte przez użytkownika:

1. **Tabela Users** - przechowywać tylko niezbędne dane: Id (int), GoogleId, Email, DisplayName, CreatedAt, LastLoginAt z indeksem na GoogleId

2. **UserProfiles** - stworzyć oddzielną tabelę z relacją 1:1 do Users

3. **TechnologyDefinitions** - stworzyć wspólny słownik technologii, ale **NIE migrować** istniejących danych z UserTechnologies

4. **UserTechnologies** - dodać pole TechnologyDefinitionId jako relację do TechnologyDefinitions, oraz pole TechnologyDefinition do TechnologyDefinitions

5. **TechnologyDependency** - wykorzystać istniejącą encję, **bez pola OrderIndex** (wystarczy CreatedAt)

6. **Węzeł "Start"** - stworzyć jako specjalną definicję w TechnologyDefinitions (Id=1), automatycznie dodawać do UserTechnologies **przez kod C# podczas rejestracji/wypełniania profilu**

7. **IgnoredTechnologies** - stworzyć osobną tabelę przechowującą **pełną definicję technologii** (nie tylko referencję)

8. **AIRecommendationCache** - **implementować prosty cache w pamięci (IMemoryCache)** zamiast tabeli w bazie, z kluczem: `$"ai-recommendations:{userId}:{technologyId}:{profileHash}"`, TTL: 24h

9. **Typy danych** - pozostać przy **C# enumach** mapowanych na VARCHAR z CHECK constraint w PostgreSQL

10. **Klucze główne** - pozostać przy typie **int** (nie Guid/UUID)

11. **Row Level Security (RLS)** - **NIE implementować dla MVP**, polegać na logice aplikacji z klauzulą WHERE UserId = @userId

12. **Indeksy** - zaimplementować zalecane indeksy dla wydajności zapytań grafowych

</decisions>

## <matched_recommendations>

### Najistotniejsze zalecenia dopasowane do decyzji:

1. **Architektura Users + UserProfiles (1:1)**
   - Tabela `Users`: podstawowe dane autentykacji z Google OAuth
   - Tabela `UserProfiles`: dane profilu zawodowego (MainTechnologies, Role, DevelopmentArea, IsCompleted)
   - Separacja concerns: autentykacja vs dane domenowe

2. **Model technologii z deduplikacją**
   - Tabela `TechnologyDefinitions`: wspólne definicje (Name, Prefix, Tag, SystemDescription) - unikalne technologie w systemie
   - Tabela `UserTechnologies`: instancje użytkownika (UserId, TechnologyDefinitionId, PrivateDescription, Progress, Status, IsStart)
   - Korzyści: brak duplikacji, łatwiejsza agregacja statystyk dla admin panelu

3. **Graf technologii jako krawędzie**
   - Tabela `TechnologyDependencies`: Id, UserId, FromTechnologyId (nullable dla Start), ToTechnologyId, CreatedAt
   - UNIQUE constraint na (UserId, FromTechnologyId, ToTechnologyId)
   - Indeksy na UserId, FromTechnologyId dla szybkiego pobierania grafu

4. **Ignorowane technologie - pełna archiwizacja**
   - Tabela `IgnoredTechnologies` z pełnymi danymi:
     - Id, UserId, Name, Category, Tag, SystemDescription, AiReasoning
     - ContextTechnologyId (opcjonalne - FK do UserTechnologies)
     - IgnoredAt (timestamp)
   - UNIQUE constraint na (UserId, Name, Category)
   - Index na UserId
   - Uzasadnienie: nie zaśmiecanie TechnologyDefinitions odrzuconymi technologiami

5. **Węzeł Start z flagą systemową**
   - Dodanie pola `bool IsStart` do modelu UserTechnology
   - Automatyczne tworzenie przez kod C# po wypełnieniu profilu
   - Walidacja: `if (technology.IsStart) throw new InvalidOperationException("Nie można usunąć węzła Start")`
   - Uproszczenie logiki vs triggery bazodanowe

6. **Strategia cache - in-memory dla MVP**
   - IMemoryCache (.NET) zamiast tabeli PostgreSQL
   - Klucz: `$"ai-recommendations:{userId}:{technologyId}:{profileHash}"`
   - ProfileHash: MD5 z MainTechnologies + Role + DevelopmentArea
   - TTL: 24 godziny
   - Post-MVP: migracja na tabelę w bazie dla persistence między restartami

7. **Bezpieczeństwo przez logikę aplikacji**
   - Brak RLS dla MVP (uproszczenie)
   - Pattern: zawsze filtrować po UserId w serwisach
   - Extension method: `.ForCurrentUser(userId)` na IQueryable
   - Post-MVP: dodanie RLS jako dodatkowa warstwa defense in depth

8. **Enums C# z type-safety**
   - Zachowanie enumów: UserRole, DevelopmentArea, TechnologyTag, TechnologyStatus
   - Mapowanie na integer w PostgreSQL (domyślne EF Core)
   - Type-safety w kodzie Blazor
   - Możliwość późniejszej migracji na tabele słownikowe jeśli potrzeba dynamicznego zarządzania

9. **Strategia indeksowania**
   - `CREATE INDEX idx_users_google_id ON Users(GoogleId)` - logowanie
   - `CREATE INDEX idx_user_technologies_user_id ON UserTechnologies(UserId) WHERE Status = 'Active'` - partial index
   - `CREATE INDEX idx_technology_dependencies_user_id ON TechnologyDependencies(UserId)` - pobieranie grafu
   - `CREATE INDEX idx_technology_dependencies_from ON TechnologyDependencies(FromTechnologyId)` - traversal grafu
   - `CREATE INDEX idx_ignored_technologies_user_id ON IgnoredTechnologies(UserId)` - lista ignorowanych

10. **Constrainty integralności**
    - UNIQUE na Users.GoogleId - jeden użytkownik Google = jedno konto
    - UNIQUE na (UserId, FromTechnologyId, ToTechnologyId) w TechnologyDependencies - brak duplikatów krawędzi
    - UNIQUE na (UserId, Name, Category) w IgnoredTechnologies - nie ignoruj dwa razy tej samej technologii
    - CASCADE DELETE na większości FK - usunięcie użytkownika czyści wszystkie jego dane
    - RESTRICT/SET NULL na kontekstowych FK w IgnoredTechnologies

</matched_recommendations>

## <database_planning_summary>

### Szczegółowe podsumowanie planowania bazy danych

#### 1. Główne wymagania dotyczące schematu

**Aplikacja DeveloperGoals** wymaga bazy danych PostgreSQL wspierającej:
- System autentykacji przez Google OAuth
- Profile użytkowników z informacjami o specjalizacji (minimum 5 technologii)
- Graf technologii z zależnościami (wizualizacja vis.js)
- Zarządzanie postępem nauki (0-100%)
- System ignorowanych technologii
- Rekomendacje AI z cache'owaniem
- Pełna izolacja danych między użytkownikami
- Admin panel z metrykami (niski priorytet)

**Timeline:** 5 tygodni do BETA, solo developer, priorytet: szybki prototyp.

#### 2. Kluczowe encje i ich relacje

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

#### 3. Ważne kwestie dotyczące bezpieczeństwa i skalowalności

##### **Bezpieczeństwo**

1. **Izolacja danych użytkowników**
   - Brak RLS dla MVP - uproszczenie implementacji
   - Bezpieczeństwo przez logikę aplikacji: każde zapytanie filtruje po UserId
   - Pattern: extension method `.ForCurrentUser(userId)` na IQueryable
   - Wszystkie FK do Users z CASCADE DELETE - czyszczenie danych

2. **Autentykacja**
   - Google OAuth jako jedyny mechanizm logowania
   - GoogleId jako unikalny identyfikator zewnętrzny
   - Brak przechowywania tokenów OAuth w bazie (zarządzane przez sesje)
   - LastLoginAt dla trackingu aktywności

3. **Walidacja danych**
   - Enums C# mapowane na integer dla type-safety
   - CHECK constraints na Progress (0-100)
   - UNIQUE constraints zapobiegające duplikatom
   - NOT NULL na kluczowych polach

##### **Skalowalność**

1. **Indeksowanie**
   - Indeksy na kluczach obcych dla JOIN performance
   - Partial index na UserTechnologies (WHERE Status = 'Active')
   - Composite index na (UserId, Name) dla unikalności w kontekście użytkownika
   - Index na GoogleId dla szybkiego logowania

2. **Strategia cache**
   - IMemoryCache dla MVP (24h TTL)
   - Klucz: userId + technologyId + profileHash
   - Migracja na PostgreSQL cache w post-MVP dla persistence

3. **Optymalizacje zapytań**
   - Eager loading z Include() dla grafu (Users → Technologies → Dependencies)
   - Lazy loading wyłączony dla kontroli nad N+1 queries
   - AsNoTracking() dla read-only operations (lista, graf)

4. **Rozważania dot. wzrostu**
   - Int jako PK wystarczający dla MVP (do ~2 miliardów rekordów)
   - Możliwość późniejszej migracji na Guid jeśli distributed architecture
   - TechnologyDefinitions deduplikuje dane - oszczędność miejsca
   - Archiwizacja ignorowanych bez zaśmiecania głównego słownika

#### 4. Workflow aplikacji a schemat bazy

##### **Onboarding użytkownika**
1. Logowanie przez Google OAuth → zapis do Users (GoogleId, Email, Name)
2. Wypełnienie profilu → zapis do UserProfiles
3. Automatyczne utworzenie węzła "Start" → UserTechnologies (IsStart=true, TechnologyDefinitionId=1)

##### **Generowanie rekomendacji**
1. Kliknięcie "Szukaj nowych technologii" na węźle
2. Sprawdzenie cache w IMemoryCache
3. Request do AI z profilem + kontekstem
4. Otrzymanie ~10 technologii z reasoning
5. Zapis do cache (24h)

##### **Dodawanie technologii do grafu**
1. Wybór z listy rekomendacji (checkbox + "Dodaj do grafu")
2. Utworzenie/pobranie TechnologyDefinition (jeśli istnieje)
3. Zapis do UserTechnologies
4. Utworzenie krawędzi w TechnologyDependencies (FromTechnologyId → ToTechnologyId)

##### **Ignorowanie technologii**
1. Checkbox + "Ignoruj" w popup rekomendacji
2. Zapis pełnych danych do IgnoredTechnologies
3. ContextTechnologyId = węzeł z którego wygenerowano
4. Nie pojawia się w przyszłych rekomendacjach (filtr w logice AI)

##### **Przywracanie ignorowanej**
1. Checkbox w sekcji "Ignorowane" + "Aktywuj"
2. Usunięcie z IgnoredTechnologies
3. Ponowne wyświetlenie w liście rekomendacji

#### 5. Metryki i admin panel

**Wymagane agregacje:**
- Liczba użytkowników (total, aktywni 7/30 dni)
- % użytkowników z wypełnionym profilem (KPI: 90%)
- % użytkowników z ≥1 technologią w grafie (KPI: 75%)
- Najpopularniejsze prefiksy (group by Category w UserTechnologies)
- Średni postęp nauki (avg Progress)
- Statystyki AI (liczba requestów - logika aplikacji, nie baza)

**Zapytania potrzebne dla admin:**
```sql
-- Użytkownicy z wypełnionym profilem
SELECT COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Users) 
FROM UserProfiles WHERE MainTechnologies IS NOT NULL;

-- Najpopularniejsze kategorie
SELECT ut.Category, COUNT(*) as Count
FROM UserTechnologies ut
WHERE ut.Status = 1 AND ut.IsStart = false
GROUP BY ut.Category
ORDER BY Count DESC;
```

</database_planning_summary>

## <unresolved_issues>

### Obszary wymagające dalszych wyjaśnień lub decyzji:

1. **Sprzeczność enums vs VARCHAR**
   - W rundzie 2 użytkownik wybrał VARCHAR z CHECK constraint
   - W rundzie 3 użytkownik wybrał zachowanie enumów C#
   - **Finalna decyzja przyjęta:** Zachowanie enumów C# (ostatnia decyzja)
   - **Wymaga potwierdzenia** w implementacji

2. **Strategia migracji TechnologyDefinitions**
   - Decyzja: NIE migrować istniejących danych
   - **Wymaga wyjaśnienia:** czy zacząć od pustej TechnologyDefinitions i dodawać dynamicznie przy dodawaniu technologii przez AI?
   - **Propozycja:** Seed data z węzłem "Start" (Id=1), reszta dodawana on-demand

3. **Związek UserTechnologies z TechnologyDefinitions**
   - Aktualnie UserTechnologies ma pola: Name, Category, SystemDescription (duplikacja)
   - Po dodaniu TechnologyDefinitionId - czy usunąć te pola z UserTechnologies?
   - **Zalecenie:** Pozostawić Name jako denormalizację dla performance (uniknięcie JOIN), reszta z TechnologyDefinitions

4. **Status "Ignored" w TechnologyStatus enum**
   - Obecnie enum ma wartość "Ignored"
   - Ale ignorowane technologie są w osobnej tabeli IgnoredTechnologies
   - **Wymaga decyzji:** czy usunąć "Ignored" z enuma (pozostawić tylko "Active")?
   - **Zalecenie:** Usunąć "Ignored" - nie jest już potrzebne

5. **Usuwanie technologii z grafu - polityka cascade**
   - Co się dzieje z "potomkami" usuwanej technologii?
   - Opcja A: CASCADE DELETE - usunięcie wszystkich zależnych
   - Opcja B: Tylko usunięcie krawędzi, technologie pozostają jako "sieroty"
   - **Wymaga decyzji UX:** czy użytkownik może mieć odłączone technologie?

6. **MainTechnologies w UserProfile - format danych**
   - Aktualnie: varchar(1000) z CSV
   - Czy wystarczy dla MVP?
   - Alternatywa: JSONB array dla lepszego queryowania
   - **Zalecenie dla MVP:** Pozostać przy CSV, migracja na JSONB w razie potrzeby

7. **Walidacja minimum 5 technologii w profilu**
   - Wymaganie PRD: minimum 5 technologii w onboardingu
   - Jak walidować w bazie? CHECK constraint na LENGTH?
   - **Zalecenie:** Walidacja tylko w logice aplikacji (Blazor form validation), nie w bazie

8. **Admin panel - hardcoded access**
   - PRD wspomina: "hardcoded tylko dla administratora"
   - Czy potrzebne pole IsAdmin w Users?
   - **Zalecenie:** Hardcoded GoogleId w appsettings.json, sprawdzanie w middleware

9. **Soft delete vs hard delete**
   - Czy użytkownicy mogą usunąć konto (RODO)?
   - Czy przechowywać historię usuniętych technologii?
   - **MVP:** Hard delete wystarczający, soft delete w przyszłości

10. **Migracje EF Core - strategia deployment**
    - Czy używać `dotnet ef database update` manualnie?
    - Czy auto-migrate przy starcie aplikacji?
    - **Zalecenie dla MVP:** Auto-migrate w Program.cs z `context.Database.Migrate()`

</unresolved_issues>

---

**Status:** Gotowe do implementacji po rozwiązaniu nierozwiązanych kwestii 1-10.  
**Następny krok:** Utworzenie migracji EF Core dla nowych tabel i modyfikacji istniejących.

</conversation_summary>