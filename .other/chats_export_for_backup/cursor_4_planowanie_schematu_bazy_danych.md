# Planowanie schematu bazy danych PostgreSQL
_Exported on 11/23/2025 at 19:32:51 GMT+1 from Cursor (2.1.20)_

---

**User**

Jesteś asystentem AI, którego zadaniem jest pomoc w zaplanowaniu schematu bazy danych w PostgreSQL dla MVP (Minimum Viable Product) na podstawie dostarczonych informacji. Twoim celem jest wygenerowanie listy pytań i zaleceń, które zostaną wykorzystane w kolejnym promptowaniu do utworzenia schematu bazy danych, relacji i zasad bezpieczeństwa na poziomie wierszy (RLS).

Prosimy o uważne zapoznanie się z poniższymi informacjami:

<product_requirements>
@prd.md
</product_requirements>

<tech_stack>
@tech-stack.md
</tech_stack>

Przeanalizuj dostarczone informacje, koncentrując się na aspektach istotnych dla projektowania bazy danych. Rozważ następujące kwestie:

1. Zidentyfikuj kluczowe encje i ich atrybuty na podstawie wymagań produktu.
2. Określ potencjalne relacje między jednostkami.
3. Rozważ typów danych i ograniczeń, które mogą być konieczne.
4. Pomyśl o skalowalności i wpływie na wydajność.
5. Oceń wymagania bezpieczeństwa i ich wpływ na projekt bazy danych.
6. Rozważ wszelkie konkretne funkcje PostgreSQL, które mogą być korzystne dla projektu.

Na podstawie analizy wygeneruj listę 10 pytań i zaleceń w formie łączonej (pytanie + zalecenie). Powinny one dotyczyć wszelkich niejasności, potencjalnych problemów lub obszarów, w których potrzeba więcej informacji, aby stworzyć skuteczny schemat bazy danych. Rozważ pytania dotyczące:

1. Relacje i kardynalność jednostek
2. Typy danych i ograniczenia
3. Strategie indeksowania
4. Partycjonowanie (jeśli dotyczy)
5. Wymagania bezpieczeństwa na poziomie wierszy
6. Rozważania dotyczące wydajności
7. Kwestie skalowalności
8. Integralność i spójność danych

Dane wyjściowe powinny mieć następującą strukturę:

<pytania>
Wymień tutaj swoje pytania i zalecenia, ponumerowane dla przejrzystości:

Na przykład:
1. Czy encja „użytkownicy" powinna mieć powiązanie z „postami"?

Rekomendacja: Tak, encja „użytkownicy" powinna mieć powiązanie z „postami", ponieważ użytkownicy mogą tworzyć posty.
</pytania>

Pamiętaj, że Twoim celem jest dostarczenie kompleksowej listy pytań i zaleceń, które pomogą w stworzeniu solidnego schematu bazy danych PostgreSQL dla MVP. Skoncentruj się na jasności, trafności i dokładności swoich wyników. Nie dołączaj żadnych dodatkowych komentarzy ani wyjaśnień poza określonym formatem wyjściowym.

Kontynuuj ten proces, generując nowe pytania i rekomendacje w oparciu o przekazany kontekst i odpowiedzi użytkownika, dopóki użytkownik wyraźnie nie poprosi o podsumowanie.

Pamiętaj, aby skupić się na jasności, trafności i dokładności wyników. Nie dołączaj żadnych dodatkowych komentarzy ani wyjaśnień poza określonym formatem wyjściowym.

---

**Cursor**

Przeczytam teraz dokumenty PRD i tech-stack, aby przeanalizować wymagania dla bazy danych.

<pytania>

## Analiza wymagań i pytania dotyczące schematu bazy danych PostgreSQL

### 1. Czy tabela `Users` powinna przechowywać pełne dane z Google OAuth czy tylko niezbędne minimum?

**Rekomendacja:** Tabela `Users` powinna przechowywać tylko niezbędne dane: `Id` (GUID/UUID), `GoogleId` (unikalny identyfikator z Google), `Email`, `DisplayName`, `CreatedAt`, `LastLoginAt`. Unikaj duplikowania tokenów OAuth w bazie - zarządzaj nimi przez sesje. Dodaj indeks na `GoogleId` dla szybkiego wyszukiwania podczas logowania.

### 2. Czy profil użytkownika powinien być w oddzielnej tabeli `UserProfiles` czy jako część tabeli `Users`?

**Rekomendacja:** Stwórz oddzielną tabelę `UserProfiles` z relacją 1:1 do `Users`. Uzasadnienie: profil może być pusty podczas pierwszego logowania, a później wypełniony. Struktura: `UserId` (FK), `MainTechnologies` (JSONB lub relacja many-to-many), `Role` (enum/varchar), `DevelopmentArea` (enum/varchar), `IsCompleted` (boolean), `CompletedAt` (timestamp). To ułatwi walidację i oddzielenie concerns.

### 3. Jak modelować technologie - czy każda technologia użytkownika powinna być osobnym rekordem czy współdzielona między użytkownikami?

**Rekomendacja:** Stwórz dwie tabele: `TechnologyDefinitions` (wspólna definicja technologii: `Name`, `Prefix`, `Tag`, `SystemDescription`) i `UserTechnologies` (instancje technologii dla użytkowników: `UserId`, `TechnologyDefinitionId`, `PrivateDescription`, `ProgressPercent`, `Status`, `AddedAt`). To pozwoli uniknąć duplikacji definicji technologii i ułatwi agregację statystyk (admin panel), jednocześnie zachowując prywatność danych użytkowników.

### 4. Jak przechowywać zależności między technologiami w grafie - czy jako osobna tabela krawędzi?

**Rekomendacja:** Stwórz tabelę `TechnologyEdges` z kolumnami: `Id`, `UserId` (FK), `FromTechnologyId` (FK do `UserTechnologies`), `ToTechnologyId` (FK do `UserTechnologies`), `CreatedAt`, `OrderIndex` (dla zachowania kolejności dodawania). Dodaj UNIQUE constraint na (`UserId`, `FromTechnologyId`, `ToTechnologyId`) aby uniknąć duplikatów krawędzi. Dodaj indeksy na `UserId` i `FromTechnologyId` dla szybkiego pobierania grafu.

### 5. Jak obsłużyć węzeł "Start" w grafie - czy jako specjalny rekord w bazie czy logika aplikacyjna?

**Rekomendacja:** Stwórz specjalną definicję technologii w `TechnologyDefinitions` z `Id = 1` i `Name = "Start"`, następnie automatycznie dodawaj rekord w `UserTechnologies` dla każdego nowego użytkownika podczas wypełniania profilu. Dodaj constraint `CHECK` aby zapobiec usunięciu węzła Start (`WHERE TechnologyDefinitionId = 1`). To uprości logikę grafu i zapewni spójność danych.

### 6. Czy ignorowane technologie powinny być w osobnej tabeli czy jako status w `UserTechnologies`?

**Rekomendacja:** Stwórz osobną tabelę `IgnoredTechnologies` z kolumnami: `Id`, `UserId` (FK), `TechnologyDefinitionId` (FK), `IgnoredAt`, `OriginalContextTechnologyId` (opcjonalne - skąd została wygenerowana). Uzasadnienie: ignorowane technologie nigdy nie były w grafie użytkownika, więc nie pasują do `UserTechnologies`. Dodaj UNIQUE constraint na (`UserId`, `TechnologyDefinitionId`) i indeks na `UserId`.

### 7. Jak modelować cache rekomendacji AI - jakie klucze cache i czy przechowywać pełny JSON?

**Rekomendacja:** Jeśli implementujesz cache, stwórz tabelę `AIRecommendationCache` z kolumnami: `Id`, `CacheKey` (HASH z profilu + znanej technologii + kontekstu), `Recommendations` (JSONB - pełna odpowiedź AI), `CreatedAt`, `ExpiresAt`. Dodaj UNIQUE indeks na `CacheKey` i automatyczne czyszczenie wygasłych rekordów (PostgreSQL `pg_cron` lub logika aplikacji). Rozważ krótki TTL (7-14 dni) ze względu na zmieniające się trendy.

### 8. Jakie typy danych użyć dla pól `Prefix`, `Tag`, `Role`, `DevelopmentArea` - enum PostgreSQL czy varchar z constraint?

**Rekomendacja:** Użyj `VARCHAR(50)` z `CHECK` constraint zamiast natywnych enumów PostgreSQL. Uzasadnienie: EF Core ma ograniczone wsparcie dla PostgreSQL enums, a VARCHAR z constraint jest bardziej elastyczny przy dodawaniu nowych wartości (nie wymaga migracji typu ALTER TYPE). Przykład: `Role VARCHAR(50) CHECK (Role IN ('Programista', 'Tester', 'Analityk', ...))`. Dodatkowo, stwórz tabele słownikowe (`Roles`, `Tags`, `Prefixes`) dla łatwiejszego zarządzania.

### 9. Jakie indeksy są kluczowe dla wydajności zapytań w kontekście grafu i rekomendacji?

**Rekomendacja:** Zaimplementuj następujące indeksy: 1) `CREATE INDEX idx_users_google_id ON Users(GoogleId)`, 2) `CREATE INDEX idx_user_technologies_user_id ON UserTechnologies(UserId) WHERE Status = 'Active'` (partial index), 3) `CREATE INDEX idx_technology_edges_user_id ON TechnologyEdges(UserId)`, 4) `CREATE INDEX idx_technology_edges_from ON TechnologyEdges(FromTechnologyId)`, 5) `CREATE INDEX idx_ignored_technologies_user_id ON IgnoredTechnologies(UserId)`. Rozważ indeks GIN na `MainTechnologies` jeśli używasz JSONB.

### 10. Jak zaimplementować Row Level Security (RLS) w PostgreSQL dla izolacji danych między użytkownikami?

**Rekomendacja:** Włącz RLS na kluczowych tabelach: `UserProfiles`, `UserTechnologies`, `TechnologyEdges`, `IgnoredTechnologies`, `AIRecommendationCache`. Stwórz policy: `CREATE POLICY user_isolation ON UserTechnologies FOR ALL USING (UserId = current_setting('app.current_user_id')::uuid)`. W ASP.NET Core ustaw `current_setting` przy każdym połączeniu z bazą używając `UserId` z sesji. To zapewni bezpieczeństwo na poziomie bazy danych, nawet jeśli logika aplikacji zawiedzie. Alternatywnie: zawsze używaj klauzuli `WHERE UserId = @userId` w zapytaniach EF Core.

</pytania>

---

**User**

Odpowiedzi:

1. Tabela Users powinna przechowywać tylko niezbędne dane: Id (GUID/UUID), GoogleId (unikalny identyfikator z Google), Email, DisplayName, CreatedAt, LastLoginAt. Unikaj duplikowania tokenów OAuth w bazie - zarządzaj nimi przez sesje. Dodaj indeks na GoogleId dla szybkiego wyszukiwania podczas logowania.

2. Stwórz oddzielną tabelę UserProfiles z relacją 1:1 do Users

3. Stwórz tabele: TechnologyDefinitions (wspólna definicja technologii: Name, Prefix, Tag, SystemDescription). To pozwoli uniknąć duplikacji definicji technologii i ułatwi agregację statystyk (admin panel), jednocześnie zachowując prywatność danych użytkowników.
Do istnijącej tabeli UserTechnologies dodaj pole TechnologyDefinitionId aby była relacja do TechnologyDefinitions

4. Mam już encję TechnologyDependency. Sprawdz czy posiada pola jak Id, UserId (FK), FromTechnologyId (FK do UserTechnologies), ToTechnologyId (FK do UserTechnologies), CreatedAt, OrderIndex (dla zachowania kolejności dodawania). Dodaj UNIQUE constraint na (UserId, FromTechnologyId, ToTechnologyId) aby uniknąć duplikatów krawędzi. Dodaj indeksy na UserId i FromTechnologyId dla szybkiego pobierania grafu.

5. Stwórz specjalną definicję technologii w TechnologyDefinitions z Id = 1 i Name = "Start", następnie automatycznie dodawaj rekord w Technologies dla każdego nowego użytkownika podczas wypełniania profilu. Dodaj constraint CHECK aby zapobiec usunięciu węzła Start (WHERE TechnologyDefinitionId = 1). To uprości logikę grafu i zapewni spójność danych.

6. Stwórz osobną tabelę IgnoredTechnologies z kolumnami: Id, UserId (FK), TechnologyDefinitionId (FK), IgnoredAt, OriginalContextTechnologyId (opcjonalne - skąd została wygenerowana). Uzasadnienie: ignorowane technologie nigdy nie były w grafie użytkownika, więc nie pasują do UserTechnologies. Dodaj UNIQUE constraint na (UserId, TechnologyDefinitionId) i indeks na UserId.

7. Jeśli implementujesz cache, stwórz tabelę AIRecommendationCache z kolumnami: Id, CacheKey (HASH z profilu + znanej technologii + kontekstu), Recommendations (JSONB - pełna odpowiedź AI), CreatedAt, ExpiresAt. Dodaj UNIQUE indeks na CacheKey i automatyczne czyszczenie wygasłych rekordów (PostgreSQL pg_cron lub logika aplikacji). Rozważ krótki TTL (7-14 dni) ze względu na zmieniające się trendy.

8. Użyj VARCHAR(50) z CHECK constraint zamiast natywnych enumów PostgreSQL. Uzasadnienie: EF Core ma ograniczone wsparcie dla PostgreSQL enums, a VARCHAR z constraint jest bardziej elastyczny przy dodawaniu nowych wartości (nie wymaga migracji typu ALTER TYPE). Przykład: Role VARCHAR(50) CHECK (Role IN ('Programista', 'Tester', 'Analityk', ...)). Dodatkowo, stwórz tabele słownikowe (Roles, Tags, Prefixes) dla łatwiejszego zarządzania.

9. Zaimplementuj następujące indeksy: 1) CREATE INDEX idx_users_google_id ON Users(GoogleId), 2) CREATE INDEX idx_user_technologies_user_id ON Technologies(UserId) WHERE Status = 'Active' (partial index), 3) CREATE INDEX idx_technology_dependencies_user_id ON TechnologyDependencies(UserId), 4) CREATE INDEX idx_technology_dependencies_from ON TechnologyDependencies(FromTechnologyId), 5) CREATE INDEX idx_ignored_technologies_user_id ON IgnoredTechnologies(UserId). Rozważ indeks GIN na MainTechnologies jeśli używasz JSONB.

10.
Włącz RLS na kluczowych tabelach: UserProfiles, UserTechnologies, TechnologyDependiencies, IgnoredTechnologies, AIRecommendationCache. Stwórz policy: CREATE POLICY user_isolation ON UserTechnologies FOR ALL USING (UserId = current_setting('app.current_user_id')::uuid). W ASP.NET Core ustaw current_setting przy każdym połączeniu z bazą używając UserId z sesji. To zapewni bezpieczeństwo na poziomie bazy danych, nawet jeśli logika aplikacji zawiedzie. Alternatywnie: zawsze używaj klauzuli WHERE UserId = @userId w zapytaniach EF Core.



Po przeczytaniu odpowiedzi zadaj kolejne pytania ale tylko 5

---

**Cursor**

Przeczytam istniejący kod, aby zrozumieć aktualny stan schematu bazy danych i dostosować kolejne pytania.

<pytania>

## Kolejne pytania dotyczące implementacji schematu bazy danych

### 1. Typ danych dla klucza głównego `User.Id` - czy migrować z `int` na `Guid` (UUID)?

**Kontekst:** Aktualnie używasz `int` jako `User.Id`, ale wcześniej zaleciłem `GUID/UUID`. W kontekście Google OAuth i potencjalnej skalowalności, czy chcesz zostać przy `int` (prostsze, wydajniejsze dla małych aplikacji, sekwencyjne) czy migrować na `Guid` (bezpieczniejsze, lepsze dla distributed systems, niemożliwe do przewidzenia)?

**Rekomendacja:** Dla MVP solo developera zostań przy `int`. Jest prostszy w debugowaniu, wydajniejszy dla PostgreSQL (mniejsze indeksy), i wystarczający dla aplikacji która nie planuje distributed architecture. Zmiana na GUID zawsze możliwa później jeśli zajdzie potrzeba. **Pozostaw `int` jako klucz główny we wszystkich tabelach.**

### 2. Strategia dodania tabeli `TechnologyDefinitions` - czy migrować istniejące dane z `UserTechnologies` czy zacząć od nowa?

**Kontekst:** Masz już tabelę `UserTechnologies` z danymi technologii bezpośrednio w rekordach użytkowników. Aby dodać `TechnologyDefinitions` (wspólny słownik) i pole `TechnologyDefinitionId` w `UserTechnologies`, potrzebujesz strategii migracji. Czy: (A) Utworzyć nową tabelę i wyekstrahować unikalne technologie z `UserTechnologies` do `TechnologyDefinitions` z automatycznym przypisaniem `TechnologyDefinitionId`, czy (B) Usunąć wszystkie dane i zacząć od czystego schematu, czy (C) Zachować aktualne pole `Name` jako fallback i stopniowo migrować?

**Rekomendacja:** **Opcja A** - napisz migrację EF Core która: 1) tworzy tabelę `TechnologyDefinitions`, 2) dodaje kolumnę `TechnologyDefinitionId` (nullable) w `UserTechnologies`, 3) w metodzie migracją dodaj kod C# który wyekstrahuje unikalne kombinacje (Name, Category, Tag, SystemDescription) do `TechnologyDefinitions` i zaktualizuje `UserTechnologies.TechnologyDefinitionId`, 4) następnie ustaw `TechnologyDefinitionId` jako NOT NULL. To zachowa dane testowe.

### 3. Czy pole `OrderIndex` w `TechnologyDependency` jest konieczne dla MVP i jak je wykorzystać?

**Kontekst:** Twoja encja `TechnologyDependency` nie ma pola `OrderIndex` które rekomendowałem dla zachowania kolejności dodawania technologii. W kontekście wizualizacji grafu - czy kolejność dodawania krawędzi jest istotna, czy wystarczy samo pole `CreatedAt`? Czy planowana biblioteka vis.js wymaga określonej kolejności renderowania?

**Rekomendacja:** **Dla MVP pole `OrderIndex` jest zbędne.** Pole `CreatedAt` wystarczy do sortowania i odtworzenia kolejności dodawania. `OrderIndex` byłby potrzebny tylko gdybyś chciał pozwolić użytkownikowi ręcznie zmieniać kolejność krawędzi (co nie jest w wymaganiach). **Pozostaw tylko `CreatedAt` i sortuj po nim w zapytaniach.**

### 4. Implementacja tabeli `IgnoredTechnologies` - czy przechowywać pełną definicję technologii czy tylko referencję do potencjalnej `TechnologyDefinitions`?

**Kontekst:** Ignorowane technologie mogą pochodzić z rekomendacji AI które nigdy nie zostały zapisane do `TechnologyDefinitions`. Czy: (A) `IgnoredTechnologies` powinna przechowywać pełne dane (Name, Category, Tag, SystemDescription, AiReasoning) aby nie tworzyć rekordu w `TechnologyDefinitions` dla ignorowanych, czy (B) zawsze tworzyć rekord w `TechnologyDefinitions` i referencować przez FK?

**Rekomendacja:** **Opcja A** - tabela `IgnoredTechnologies` powinna zawierać pełne dane technologii: `Id`, `UserId`, `Name`, `Category`, `Tag`, `SystemDescription`, `AiReasoning`, `IgnoredAt`, `ContextTechnologyId` (opcjonalne - od której technologii została wygenerowana). NIE używaj FK do `TechnologyDefinitions` dla ignorowanych. Uzasadnienie: nie zaśmiecasz wspólnego słownika technologiami które użytkownik odrzucił, oszczędzasz miejsce, upraszczasz logikę.

### 5. Czy tworzyć osobne tabele słownikowe (`Roles`, `Tags`, `Prefixes`, `DevelopmentAreas`) czy pozostać przy C# enumach mapowanych na PostgreSQL?

**Kontekst:** Aktualnie używasz C# enumów (`UserRole`, `DevelopmentArea`, `TechnologyTag`, `TechnologyStatus`) które EF Core mapuje na PostgreSQL integers. Wcześniej zaleciłem VARCHAR z CHECK constraint i tabele słownikowe. Jakie podejście dla MVP: (A) pozostać przy enumach C# (prostsze, type-safe w kodzie), (B) zmienić na VARCHAR z CHECK constraint, (C) stworzyć tabele słownikowe z FK?

**Rekomendacja:** **Zostań przy C# enumach mapowanych na integers.** Dla MVP to wystarczające, type-safe, szybkie i łatwe w użyciu w Blazor components. Tabele słownikowe byłyby over-engineering dla niewielkiej liczby opcji. Jeśli później będziesz potrzebować dynamicznego dodawania nowych ról/obszarów przez admin panel, wtedy rozważ migrację na tabele słownikowe. **Zachowaj obecne enumy.**

</pytania>

---

**User**

1. Zostań przy int

2. Nie migrować nic z UserTechnologies  do TechnologyDefinitions 

3. OrderIndex nie jest potrzebne

4. Przechować Pełną definicję

5. Opcja B - zmień na VARCHAR  z CHECK constraint



Proszę o kolejne 5 pytań

---

**Cursor**

<pytania>

## Kolejne pytania dotyczące finalizacji schematu bazy danych

### 1. Migracja enumów na VARCHAR z CHECK constraint - czy zachować enums w C# i mapować na VARCHAR, czy usunąć enums całkowicie?

**Kontekst:** Obecnie masz enums: `UserRole`, `DevelopmentArea`, `TechnologyTag`, `TechnologyStatus`. Decyzja o zmianie na VARCHAR z CHECK constraint wymaga wyboru: (A) Zachować enums w C# jako warstwy domenowej i skonfigurować EF Core `HasConversion()` aby mapować enum→string w bazie, czy (B) Całkowicie usunąć enums i używać `string` w modelach C# z walidacją w kodzie?

**Rekomendacja:** **Opcja A - Zachowaj enums w C# i użyj `.HasConversion<string>()`** w `OnModelCreating`. Przykład: `entity.Property(e => e.Role).HasConversion<string>().HasMaxLength(50);` z dodatkowym constraint CHECK w migracji: `migrationBuilder.Sql("ALTER TABLE UserProfiles ADD CONSTRAINT CK_UserProfiles_Role CHECK (Role IN ('Programista', 'Tester', 'Analityk', 'DataScienceSpecialist'))");`. To daje type-safety w kodzie + elastyczność w bazie. Wartości w bazie będą nazwami enumów jako stringi.

### 2. Struktura tabeli `IgnoredTechnologies` - jakie dokładnie pola i constrainty potrzebne, uwzględniając pełne dane technologii?

**Kontekst:** Tabela `IgnoredTechnologies` ma przechowywać pełną definicję technologii. Potrzebne są: podstawowe pola (Id, UserId, Name, Category, Tag, SystemDescription, AiReasoning, IgnoredAt), opcjonalne pola (ContextTechnologyId - od jakiej technologii w grafie została wygenerowana), constrainty (UNIQUE na UserId+Name? czy można zignorować tę samą technologię wielokrotnie?), oraz czy potrzebne jest pole `Reasoning` osobno od `AiReasoning`?

**Rekomendacja:** Struktura tabeli `IgnoredTechnologies`:
- `Id` (int, PK)
- `UserId` (int, FK do Users, ON DELETE CASCADE)
- `Name` (varchar(255), NOT NULL)
- `Category` (varchar(100), NOT NULL)
- `Tag` (varchar(50), NOT NULL, CHECK constraint)
- `SystemDescription` (varchar(1000), NOT NULL)
- `AiReasoning` (varchar(1000), NULL)
- `ContextTechnologyId` (int, NULL, FK do UserTechnologies, ON DELETE SET NULL - jeśli technologia kontekstowa zostanie usunięta, zachowaj ignorowaną)
- `IgnoredAt` (timestamp, NOT NULL)
- **UNIQUE constraint na (UserId, Name, Category)** - użytkownik nie może zignorować dokładnie tej samej technologii dwa razy
- **Index na UserId** dla szybkiego pobierania listy ignorowanych

### 3. Row Level Security (RLS) - czy implementować teraz dla MVP czy polegać na logice aplikacji WHERE UserId = @userId?

**Kontekst:** RLS w PostgreSQL zapewnia bezpieczeństwo na poziomie bazy, ale wymaga dodatkowej konfiguracji (ustawienie `current_setting` przy każdym połączeniu, tworzenie policies). Dla solo developera MVP, alternatywą jest zawsze używać klauzuli `WHERE UserId = @userId` w LINQ/EF Core queries. Która opcja jest bardziej praktyczna dla 5-tygodniowego timeline?

**Rekomendacja:** **Dla MVP - NIE implementuj RLS, polegaj na logice aplikacji.** Uzasadnienie: RLS dodaje złożoność (konfiguracja connection string, middleware do ustawiania user_id, testowanie), a EF Core z Blazor Server ma łatwy dostęp do zalogowanego użytkownika przez session. Implementuj pattern: zawsze filtruj po `UserId` w serwisach/repositories, dodaj extension method np. `ForCurrentUser(userId)` do IQueryable. RLS możesz dodać w post-MVP jako dodatkową warstwę bezpieczeństwa. **Priorytet: działająca aplikacja > defense in depth.**

### 4. Tabela `AIRecommendationCache` - czy implementować w MVP czy odłożyć na później, i jaka strategia cache key?

**Kontekst:** Cache może znacznie obniżyć koszty AI, ale dodaje złożoność. Dla MVP z jednym użytkownikiem testowym i małą bazą, cache może być przedwczesną optymalizacją. Alternatywnie: prosty cache w pamięci (IMemoryCache) vs cache w bazie. Jeśli w bazie, to jaki algorytm generowania `CacheKey`?

**Rekomendacja:** **Implementuj prosty cache w pamięci (IMemoryCache .NET) zamiast tabeli w bazie dla MVP.** Klucz cache: `$"ai-recommendations:{userId}:{technologyId}:{profileHash}"` gdzie `profileHash` to hash MD5 z `MainTechnologies + Role + DevelopmentArea`. TTL: 24 godziny. To wystarczy dla MVP, prostsze niż tabela w bazie. Tabelę `AIRecommendationCache` w PostgreSQL dodaj dopiero gdy będziesz miał wielu użytkowników i potrzeba cache'u między restartami aplikacji. **Dla MVP: IMemoryCache, dla produkcji: tabela w bazie.**

### 5. Czy węzeł "Start" w grafie powinien być automatycznie tworzony przez trigger bazodanowy, kod C# podczas rejestracji, czy przez seed data?

**Kontekst:** Każdy użytkownik potrzebuje węzła "Start" (jako specjalna technologia) w swojej tabeli `UserTechnologies` gdy wypełnia profil. Opcje implementacji: (A) PostgreSQL trigger na INSERT w `UserProfiles` automatycznie dodaje rekord "Start" do `UserTechnologies`, (B) Kod C# w serwisie po zapisaniu profilu, (C) Seed data z hardcoded Id=1 dla technologii "Start" w `TechnologyDefinitions` (ale ty nie chcesz tej tabeli), (D) Specjalna flaga `IsStart` w `UserTechnology`.

**Rekomendacja:** **Opcja B + D - Kod C# podczas wypełniania profilu + flaga `IsStart` w modelu.** Dodaj pole `bool IsStart { get; set; } = false;` do `UserTechnology`. W serwisie po zapisaniu `UserProfile`, automatycznie utwórz rekord: `new UserTechnology { UserId = userId, Name = "Start", Category = "System", Tag = TechnologyTag.Technologia, IsStart = true, Progress = 0, SystemDescription = "Punkt startowy Twojej ścieżki rozwoju" }`. W logice usuwania dodaj walidację: `if (technology.IsStart) throw new InvalidOperationException("Nie można usunąć węzła Start")`. To prostsze niż triggery, bardziej testowalne, i explicitne w kodzie C#.

</pytania>

---

**User**

1. Zachowaj Enums

2. Tak, zrób rekomendowaną strukturę tabeli

3. Dla MVP, nie implmentuj RLS

4. Implementuj prosty cache w pamięci, zgodnie z kluczem który podałeś

5. Prze kod C# podczas rejestrcji



To już koniec pytań.



Teraz:



Jesteś asystentem AI, którego zadaniem jest podsumowanie rozmowy na temat planowania bazy danych dla MVP i przygotowanie zwięzłego podsumowania dla następnego etapu rozwoju. W historii konwersacji znajdziesz następujące informacje:

1. Dokument wymagań produktu (PRD)

2. Informacje o stacku technologicznym

3. Historia rozmów zawierająca pytania i odpowiedzi

4. Zalecenia dotyczące modelu



Twoim zadaniem jest:

1. Podsumować historii konwersacji, koncentrując się na wszystkich decyzjach związanych z planowaniem bazy danych.

2. Dopasowanie zaleceń modelu do odpowiedzi udzielonych w historii konwersacji. Zidentyfikuj, które zalecenia są istotne w oparciu o dyskusję.

3. Przygotuj szczegółowe podsumowanie rozmowy, które obejmuje:

   a. Główne wymagania dotyczące schematu bazy danych

   b. Kluczowe encje i ich relacje

   c. Ważne kwestie dotyczące bezpieczeństwa i skalowalności

   d. Wszelkie nierozwiązane kwestie lub obszary wymagające dalszego wyjaśnienia

4. Sformatuj wyniki w następujący sposób:



<conversation_summary>

<decisions>

[Wymień decyzje podjęte przez użytkownika, ponumerowane].

</decisions>



<matched_recommendations>

[Lista najistotniejszych zaleceń dopasowanych do rozmowy, ponumerowanych]

</matched_recommendations>



<database_planning_summary> [Podsumowanie planowania bazy danych]

[Podaj szczegółowe podsumowanie rozmowy, w tym elementy wymienione w kroku 3].

</database_planning_summary>



<unresolved_issues>

[Wymień wszelkie nierozwiązane kwestie lub obszary wymagające dalszych wyjaśnień, jeśli takie istnieją]

</unresolved_issues>

</conversation_summary>



Końcowy wynik powinien zawierać tylko treść w formacie markdown. Upewnij się, że Twoje podsumowanie jest jasne, zwięzłe i zapewnia cenne informacje dla następnego etapu planowania bazy danych.



---

**Cursor**

<conversation_summary>

# Podsumowanie planowania bazy danych dla DeveloperGoals MVP

## <decisions>

### Decyzje podjęte przez użytkownika:

1. **Tabela Users** - przechowywać tylko niezbędne dane: Id (int), GoogleId, Email, DisplayName, CreatedAt, LastLoginAt z indeksem na GoogleId

2. **UserProfiles** - stworzyć oddzielną tabelę z relacją 1:1 do Users

3. **TechnologyDefinitions** - stworzyć wspólny słownik technologii, ale **NIE migrować** istniejących danych z UserTechnologies

4. **UserTechnologies** - dodać pole TechnologyDefinitionId jako relację do TechnologyDefinitions

5. **TechnologyDependency** - wykorzystać istniejącą encję, **bez pola OrderIndex** (wystarczy CreatedAt)

6. **Węzeł "Start"** - stworzyć jako specjalną definicję w TechnologyDefinitions (Id=1), automatycznie dodawać do UserTechnologies **przez kod C# podczas rejestracji/wypełniania profilu**

7. **IgnoredTechnologies** - stworzyć osobną tabelę przechowującą **pełną definicję technologii** (nie tylko referencję)

8. **AIRecommendationCache** - **implementować prosty cache w pamięci (IMemoryCache)** zamiast tabeli w bazie, z kluczem: `$"ai-recommendations:{userId}:{technologyId}:{profileHash}"`, TTL: 24h

9. **Typy danych** - pozostać przy **C# enumach** mapowanych na integer w PostgreSQL (finalna decyzja - użytkownik początkowo wybrał VARCHAR z CHECK constraint, ale następnie zmienił decyzję na zachowanie enumów)

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

```plaintext
- Id: int (PK)
- GoogleId: varchar(255) UNIQUE, NOT NULL, indexed
- Email: varchar(255), NOT NULL, indexed
- Name: varchar(255), NOT NULL (DisplayName z Google)
- CreatedAt: timestamp, NOT NULL
- LastLoginAt: timestamp, NULL
```

```plaintext
- Id: int (PK)
- UserId: int (FK → Users, UNIQUE, CASCADE)
- MainTechnologies: varchar(1000), NOT NULL (CSV lub lista oddzielona przecinkami)
- Role: int (enum → Programista, Tester, Analityk, DataScienceSpecialist)
- DevelopmentArea: int (enum → UserInterface, Backend, FullStack, Testing, DataScience, DevOps)
- CreatedAt: timestamp, NOT NULL
- UpdatedAt: timestamp, NULL
```

```plaintext
- Id: int (PK)
- Name: varchar(255), NOT NULL (np. "DotNet - Entity Framework")
- Prefix: varchar(100), NOT NULL (np. "DotNet", "Java", "JavaScript")
- Tag: int (enum → Technologia, Framework, BazaDanych, Metodologia, Narzedzie)
- SystemDescription: varchar(1000), NOT NULL (opis z AI)
- CreatedAt: timestamp, NOT NULL
- UNIQUE na (Name, Prefix, Tag) - jedna definicja
- Specjalny rekord: Id=1, Name="Start" dla węzła startowego
```

```plaintext
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

```plaintext
- Id: int (PK)
- UserId: int (FK → Users, CASCADE)
- FromTechnologyId: int (FK → UserTechnologies, RESTRICT), NULL (dla Start)
- ToTechnologyId: int (FK → UserTechnologies, CASCADE), NOT NULL
- CreatedAt: timestamp, NOT NULL (dla sortowania kolejności)
- UNIQUE na (UserId, FromTechnologyId, ToTechnologyId)
- Index na UserId
- Index na FromTechnologyId
```

```plaintext
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

```plaintext
Users 1───1 UserProfiles
Users 1───N UserTechnologies
Users 1───N TechnologyDependencies
Users 1───N IgnoredTechnologies
TechnologyDefinitions 1───N UserTechnologies
UserTechnologies 1───N TechnologyDependencies (jako FromTechnology)
UserTechnologies 1───N TechnologyDependencies (jako ToTechnology)
UserTechnologies 1───N IgnoredTechnologies (jako ContextTechnology, opcjonalne)
```

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

