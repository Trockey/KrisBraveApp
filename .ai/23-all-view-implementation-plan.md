# Plan implementacji widoku aplikacji KrisBraveApp

## 1. Przegląd
Celem jest stworzenie kompletnego interfejsu użytkownika dla aplikacji wspierającej rozwój technologiczny programistów. Aplikacja opiera się na interaktywnym grafie technologii, systemie rekomendacji AI oraz profilowaniu użytkownika. Główny nacisk kładziony jest na intuicyjną wizualizację ścieżki rozwoju (Dashboard), proces onboardingu oraz interakcję z asystentem AI w celu odkrywania nowych technologii.

## 2. Routing widoku

Poniżej przedstawiono mapę routingu aplikacji Blazor:

| Ścieżka | Komponent strony | Dostęp | Opis |
|---------|------------------|--------|------|
| `/` | `Pages/Home.razor` | Publiczny | Landing page z przyciskiem logowania (jeśli niezalogowany) lub przekierowanie do Dashboardu. |
| `/login` | `Pages/Login.razor` | Publiczny | Strona inicjująca proces OAuth (lub obsługująca callback z Google). |
| `/dashboard` | `Pages/Dashboard.razor` | Prywatny | Główny widok z grafem technologii. Wymaga wypełnionego profilu. |
| `/onboarding` | `Pages/Onboarding.razor` | Prywatny | Pierwsze kroki po rejestracji. Formularz tworzenia profilu. |
| `/profile` | `Pages/Profile.razor` | Prywatny | Edycja istniejącego profilu użytkownika. |

## 3. Struktura komponentów

Hierarchia komponentów w projekcie:

```text
MainLayout
├── NavMenu (Stan uwierzytelnienia, Linki: Dashboard, Profil, Logout)
└── @Body
    ├── Home (Public)
    │   └── HeroSection (CTA do logowania)
    ├── Login (Auth Logic)
    ├── Onboarding
    │   └── ProfileForm (Tryb: Create)
    ├── Profile
    │   └── ProfileForm (Tryb: Edit)
    └── Dashboard (Smart Component - zarządza stanem grafu)
        ├── GraphVisualizer (Wrapper JS Interop - Cytoscape.js)
        ├── NodeOptionsModal (Popup 1: Edycja węzła, Progress, Delete)
        ├── RecommendationModal (Popup 2: AI Request, Lista wyników, Ignorowane)
        │   ├── LoadingSpinner
        │   ├── TechRecommendationList (Lista z checkboxami)
        │   ├── IgnoredTechList
        │   └── CustomTechForm (Dodawanie własnej technologii)
        └── FabMenu (Floating Action Button - opcjonalnie na mobile)
```

## 4. Szczegóły komponentów

### `Pages/Dashboard.razor` (Smart Component)
- **Opis**: Główny kontener logiki biznesowej. Zarządza stanem grafu, komunikacją z API oraz widocznością modali.
- **Główne elementy**: Kontener na `GraphVisualizer`, instancje modali (`NodeOptionsModal`, `RecommendationModal`).
- **Obsługiwane interakcje**:
  - `HandleNodeClick(nodeId)`: Otwiera opcje węzła.
  - `HandleBackgroundClick()`: Odznacza węzeł.
  - `HandleGetRecommendations(nodeId)`: Otwiera modal rekomendacji i inicjuje żądanie do AI.
  - `RefreshGraph()`: Pobiera najnowszy stan grafu z API.
- **Typy**: `GraphDataDto`, `GraphNodeDto`, `GraphEdgeDto`.
- **Zależności**: `ITechnologyService`, `IAIRecommendationService`.

### `Components/GraphVisualizer.razor`
- **Opis**: Komponent prezentacyjny odpowiedzialny za renderowanie grafu przy użyciu biblioteki JS (rekomendowane: Cytoscape.js).
- **Główne elementy**: `div` kontenera grafu, logika `IJSRuntime`.
- **Obsługiwane zdarzenia**:
  - `OnNodeSelected`: EventCallback zwracający ID klikniętego węzła.
  - `OnGraphLoaded`: Sygnał gotowości interfejsu.
- **Propsy**:
  - `Nodes`: `List<GraphNodeDto>`
  - `Edges`: `List<GraphEdgeDto>`

### `Components/ProfileForm.razor`
- **Opis**: Formularz używany zarówno w Onboardingu, jak i edycji profilu.
- **Główne elementy**:
  - Multiselect dla technologii (min. 5).
  - Radio/Dropdown dla Roli (Programista, Tester, etc.).
  - Radio/Dropdown dla Obszaru (Backend, Frontend, etc.).
  - Walidacja błędów pod polami.
- **Obsługiwana walidacja**:
  - `MainTechnologies`: min. 5 elementów.
  - `Role`: wymagane.
  - `DevelopmentArea`: wymagane.
- **Propsy**:
  - `InitialData`: `UserProfileDto` (opcjonalne).
  - `OnSubmit`: EventCallback z danymi formularza.
  - `IsLoading`: bool (blokada przycisku podczas zapisu).

### `Components/RecommendationModal.razor` (Popup 2)
- **Opis**: Modal obsługujący interakcję z AI. Wyświetla spinner, wyniki, obsługuje dodawanie/ignorowanie.
- **Główne elementy**:
  - Zakładki: "Rekomendacje", "Ignorowane", "Dodaj własną".
  - Lista rekomendacji z checkboxami i polem `Reasoning`.
  - Sekcja błędów (timeout/api error).
- **Obsługiwane interakcje**:
  - `AddSelectedToGraph()`: Wysyła wybrane ID do API.
  - `IgnoreSelected()`: Wysyła wybrane ID do listy ignorowanych.
  - `RetryRecommendation()`: Ponawia zapytanie do AI.
- **Propsy**:
  - `IsVisible`: bool.
  - `SourceNodeId`: long (węzeł źródłowy dla kontekstu).
  - `ContextTechIds`: `List<long>` (pełny kontekst grafu).

### `Components/NodeOptionsModal.razor` (Popup 1)
- **Opis**: Modal zarządzania pojedynczym węzłem.
- **Główne elementy**:
  - Slider postępu (0-100%).
  - Textarea na prywatny opis.
  - Przycisk "Usuń węzeł" (z potwierdzeniem).
  - Przycisk "Szukaj nowych technologii" (przekierowuje do Popup 2).
- **Propsy**:
  - `Node`: `GraphNodeDto`.
  - `OnSave`: Callback zapisu zmian.
  - `OnDelete`: Callback usunięcia.
  - `OnGetRecommendations`: Callback otwarcia Popup 2.

## 5. Typy (DTO i ViewModels)

Należy utworzyć/zaktualizować następujące rekordy/klasy w warstwie klienta:

```csharp
// DTO dla Grafu
public class GraphNodeDto {
    public long Id { get; set; }
    public string Name { get; set; }
    public string Prefix { get; set; } // np. "DotNet"
    public string Tag { get; set; } // "Framework", "Database"
    public int Progress { get; set; }
    public bool IsStart { get; set; }
    public string SystemDescription { get; set; }
    public string? PrivateDescription { get; set; }
}

public class GraphEdgeDto {
    public long Id { get; set; }
    public long From { get; set; } // Id węzła źródłowego
    public long To { get; set; } // Id węzła docelowego
}

public class GraphDataDto {
    public List<GraphNodeDto> Nodes { get; set; }
    public List<GraphEdgeDto> Edges { get; set; }
}

// DTO dla Profilu
public class UserProfileDto {
    [MinLength(5, ErrorMessage = "Wybierz minimum 5 technologii.")]
    public List<string> MainTechnologies { get; set; } = new();
    
    [Required]
    public string Role { get; set; }
    
    [Required]
    public string DevelopmentArea { get; set; }
}

// DTO dla Rekomendacji
public class RecommendationRequestDto {
    public long FromTechnologyId { get; set; }
    public List<long> ContextTechnologyIds { get; set; }
}

public class AIRecommendationDto {
    public long TechnologyDefinitionId { get; set; }
    public string Name { get; set; }
    public string Prefix { get; set; }
    public string Tag { get; set; }
    public string Description { get; set; }
    public string AiReasoning { get; set; }
    public bool IsSelected { get; set; } // ViewModel property
}
```

## 6. Zarządzanie stanem

W aplikacji Blazor Server/WebAssembly stan będzie zarządzany na dwóch poziomach:

1.  **Stan Użytkownika (Globalny)**:
    *   Implementacja serwisu `UserStateService` zarejestrowanego jako *Scoped*.
    *   Przechowuje: `IsAuthenticated`, `HasProfile`, `CurrentUserBasicInfo`.
    *   Używany przez `MainLayout` do aktualizacji menu oraz przez `Onboarding` do przekierowań.

2.  **Stan Grafu (Lokalny dla Dashboardu)**:
    *   Komponent `Dashboard.razor` przechowuje listę węzłów i krawędzi.
    *   Po dodaniu technologii (przez Popup 2) lub usunięciu (przez Popup 1), Dashboard odświeża dane i wywołuje metodę `GraphVisualizer.Refresh()` przez JS Interop.
    *   Stan Modali (`ShowRecommendationModal`, `SelectedNode`) jest trzymany w polach komponentu `Dashboard`.

## 7. Integracja API

Aplikacja frontendowa będzie komunikować się z backendem. Należy zaimplementować (lub upewnić się, że istnieją) następujące serwisy:

### AuthService
- `Login()`: `POST /api/auth/login` -> przekierowanie do Google.
- `Logout()`: `POST /api/auth/logout`.
- `GetMe()`: `GET /api/auth/me`.

### ProfileService
- `GetProfile()`: `GET /api/profile`.
- `CreateProfile(UserProfileDto)`: `POST /api/profile`.
- `UpdateProfile(UserProfileDto)`: `PUT /api/profile`.

### TechnologyService (Wymaga utworzenia kontrolera backendowego `TechnologyController`)
- `GetGraph()`: `GET /api/technologies/graph` -> zwraca `GraphDataDto`.
- `AddTechnology(TechDto)`: `POST /api/technologies` (pojedyncza) lub `/batch`.
- `UpdateTechnology(id, patchDto)`: `PATCH /api/technologies/{id}` (postęp, opis).
- `DeleteTechnology(id)`: `DELETE /api/technologies/{id}`.
- `CreateCustomTechnology(dto)`: `POST /api/technologies/custom`.

### AIRecommendationService
- `GetRecommendations(req)`: `POST /api/ai/recommendations`.

### IgnoredTechnologyService (Wymaga utworzenia kontrolera backendowego)
- `GetIgnored()`: `GET /api/ignored-technologies`.
- `Ignore(dto)`: `POST /api/ignored-technologies`.
- `Restore(id)`: `DELETE /api/ignored-technologies/{id}`.

## 8. Interakcje użytkownika

1.  **Pierwsze logowanie**: Użytkownik klika "Zaloguj przez Google". Po powrocie, jeśli `hasProfile == false` w odpowiedzi API, system przekierowuje na `/onboarding`.
2.  **Wypełnianie profilu**: Użytkownik wybiera tagi. Walidacja "na żywo" blokuje przycisk "Dalej" dopóki nie wybrano 5 technologii.
3.  **Graf - Start**: Użytkownik widzi tylko węzeł "Start". Klika go -> Popup 1 ("Szukaj nowych technologii") -> Popup 2 (Spinner -> Lista).
4.  **Wybór technologii**: Na liście w Popup 2 użytkownik zaznacza np. "C#" i "SQL". Klika "Dodaj do grafu". Modal się zamyka (lub czyści), graf się przerysowuje animacją, pojawiają się nowe węzły połączone ze "Start".
5.  **Aktualizacja postępu**: Użytkownik klika "C#". W Popup 1 przesuwa suwak na 50%. Klika "Zapisz". Kolor węzła na grafie zmienia się (np. wypełnia się w połowie lub zmienia odcień).

## 9. Warunki i walidacja

*   **Profil**:
    *   Multiselect musi mieć `Count >= 5`.
    *   Wszystkie pola enum (Rola, Obszar) muszą mieć wartość.
*   **Graf**:
    *   Nie można usunąć węzła "Start".
    *   Nie można wywołać AI dla technologii, która nie jest aktywna na grafie (choć w tym systemie widoczne są tylko aktywne).
*   **AI Request**:
    *   Wymaga `FromTechnologyId`.
    *   Timeout po 20s.

## 10. Obsługa błędów

*   **Globalna**: `MainLayout` powinien zawierać komponent `Toast/Snackbar` do wyświetlania błędów API (np. 500 Server Error).
*   **AI Timeout**: W `RecommendationModal`, jeśli request trwa > 20s lub wróci status 408/500, spinner znika i pojawia się komunikat "Nie udało się wygenerować rekomendacji. Spróbuj ponownie" z przyciskiem "Ponów".
*   **Brak danych**: Jeśli graf jest pusty (poza Start), wyświetlany jest dymek pomocniczy (tooltip) przy węźle Start.

## 11. Kroki implementacji

1.  **Setup Typów i Serwisów**: Utworzenie DTO w projekcie klienckim oraz serwisów HTTP do komunikacji z API.
2.  **Implementacja Backend (Brakujące Endpointy)**:
    *   Stworzenie `TechnologyController` (Graph endpoint, CRUD węzłów).
    *   Stworzenie `IgnoredTechnologyController`.
3.  **Widoki Podstawowe**:
    *   Dostosowanie `MainLayout` (User state).
    *   Implementacja `Login` i `Home` (Redirect logic).
4.  **Profil i Onboarding**:
    *   Stworzenie komponentu `ProfileForm`.
    *   Implementacja stron `Onboarding.razor` i `Profile.razor`.
5.  **Graf - Infrastruktura**:
    *   Konfiguracja JS Interop (np. dodanie Cytoscape.js do `index.html` / `App.razor`).
    *   Stworzenie `GraphVisualizer.razor`.
6.  **Graf - Logika Dashboardu**:
    *   Pobieranie danych grafu z API.
    *   Obsługa kliknięć w węzły.
7.  **Modale**:
    *   Implementacja `NodeOptionsModal` (edycja, usuwanie).
    *   Implementacja `RecommendationModal` (logika AI, checkboxy, API call).
8.  **Integracja i Testy**:
    *   Przejście pełnej ścieżki: Rejestracja -> Profil -> Graf -> AI -> Dodanie węzła.
    *   Stylowanie Tailwind CSS (Flowbite) dla spójnego wyglądu.
