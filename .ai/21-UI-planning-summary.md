<conversation_summary>
Podsumowanie planowania architektury UI dla DeveloperGoals MVP
1. Decisions (Decyzje użytkownika)
Seria 1 - Fundamenty UI
Graf technologii: Komponent Blazor z JavaScript Interop dla vis.js
Popupy: Wykorzystanie komponentów modalnych Flowbite opakownych w komponenty Blazor (Popup 1 - Opcje, Popup 2 - Rekomendacje)
Nawigacja: Minimalistyczny navbar z logo, ikoną profilu i przyciskiem wylogowania
Formularz profilu: Pojedyncza strona z wszystkimi polami (Select multi-select dla technologii, Radio buttons dla roli i obszaru)
Wizualizacja postępu: Kombinacja - kolor węzła + pasek postępu + tekst procentowy (wszystko widoczne jednocześnie)
Loader AI: Spinner z tekstem "Generowanie rekomendacji..." i licznikiem sekund (do 20s)
Lista ignorowanych: Rozwijana sekcja (Accordion) poniżej rekomendacji z licznikiem
Dark mode: Przełącznik w navbarze z zapisem preferencji w localStorage
Responsywność: Brak wsparcia dla urządzeń mobilnych w MVP
Obsługa błędów: System warstwowy - inline alerts dla walidacji, toasty dla błędów operacyjnych, modale dla krytycznych błędów
Seria 2 - Architektura komponentów
Struktura komponentów: Podział na mniejsze komponenty (GraphCanvas, TechnologyOptionsModal, AIRecommendationsModal)
Zarządzanie stanem: Dedykowany serwis GraphStateService (scoped)
Synchronizacja z vis.js: Jednokierunkowy przepływ danych Blazor → vis.js przez JS Interop z callbackami
Wygląd węzła: Prostokąt z zaokrąglonymi rogami, custom HTML shape z nazwą, paskiem postępu, procentem i kolorem tła
Komunikacja z API: Dedykowane serwisy API dla każdego zasobu (ProfileApiService, TechnologyApiService, AIRecommendationApiService, IgnoredTechnologyApiService)
Walidacja formularzy: DataAnnotations z <EditForm> i <DataAnnotationsValidator>
Onboarding: Automatyczne przekierowanie na stronę profilu z przyjaznym nagłówkiem
Checkboxy w Popup 2: Karty z możliwością zaznaczenia (Card + checkbox) zamiast standardowych checkboxów
Routing: Blazor Router z osobnymi stronami (/login, /profile, /graph, /admin)
Obsługa błędów globalnych: <ErrorBoundary> + GlobalExceptionHandler + ToastService
Seria 3 - Szczegóły implementacji
Integracja vis.js: Dedykowany plik wwwroot/js/graph-visualization.js
Stan ładowania: Lokalny stan w komponentach + globalny LoadingService dla operacji full-page
Konfiguracja vis.js: Klasa GraphVisualizationOptions.cs z konfiguracją w appsettings.json
Cache klienta: Brak - poleganie wyłącznie na cache serwerowym (24h TTL)
Struktura folderów: Hierarchiczna (Components/Pages/, Components/Graph/, Components/Profile/, Components/Shared/)
Aktualizacja grafu: Inkrementalna aktualizacja (addNodes/addEdges) zamiast pełnego przeładowania
Dark mode UI: Przełącznik w NavMenu z ikoną słońca/księżyca, ThemeService (singleton)
Konflikty batch: Pokazanie wszystkich błędów w jednym toaście z listą
Przycisk "Szukaj technologii": Obie opcje - na węźle grafu (hover) + w Popup 1
Węzeł Start: Wyróżniony styl (1.5x większy, gradient niebieski, ikona, glow, brak usuwania)
Edycja opisu: Prosty textarea z licznikiem znaków (0/2000), bez rich text
Layout grafu: Fullscreen - graf zajmuje calc(100vh - 64px)
Walidacja custom tech: Tylko on submit (blokada przycisku jeśli błędy) - MODYFIKACJA UŻYTKOWNIKA
Sortowanie rekomendacji: Zachowanie kolejności API (bez opcjonalnego dropdownu sortowania) - MODYFIKACJA UŻYTKOWNIKA
Pusty graf: Tylko tekst pomocniczy (bez tutorial overlay) - MODYFIKACJA UŻYTKOWNIKA
Sesja: Automatyczne wylogowanie po wygaśnięciu (7 dni) bez ostrzeżenia
Przycisk wyloguj: Bezpośrednie wylogowanie bez potwierdzenia
Aktualizacja postępu: Zapisywanie przez przycisk "Zapisz" (nie auto-save)
Długie nazwy: Word wrap z maksymalną szerokością 200px, tooltip przy hover
Błędy walidacji API: Mapowanie na konkretne pola formularza + ogólny toast
2. Matched Recommendations (Dopasowane zalecenia)
Architektura komponentów
Struktura komponentów Blazor:
   Components/   ├── Pages/           # Strony z @page   │   ├── Login.razor   │   ├── Profile.razor   │   ├── Graph.razor   │   └── Admin.razor   ├── Graph/           # Komponenty grafu   │   ├── GraphCanvas.razor   │   ├── TechnologyOptionsModal.razor   │   ├── AIRecommendationsModal.razor   │   └── TechnologyCard.razor   ├── Profile/   │   ├── ProfileForm.razor   │   └── TechnologySelector.razor   ├── Shared/   │   ├── Toast.razor   │   ├── LoadingSpinner.razor   │   └── ErrorBoundary.razor   └── Layout/       ├── MainLayout.razor       └── NavMenu.razor
Serwisy zarządzania stanem i API:
GraphStateService (scoped) - stan grafu, operacje CRUD, eventy zmian
ProfileApiService (scoped) - operacje na profilu
TechnologyApiService (scoped) - CRUD technologii
AIRecommendationApiService (scoped) - generowanie rekomendacji
IgnoredTechnologyApiService (scoped) - zarządzanie ignorowanymi
ThemeService (singleton) - dark/light mode
LoadingService (singleton) - globalny loading indicator
ToastService (singleton) - wyświetlanie toastów
JavaScript Interop dla vis.js (wwwroot/js/graph-visualization.js):
   window.GraphVisualization = {     initialize(containerId, dotNetHelper),     updateGraph(nodes, edges),     addNodes(newNodes),     addEdges(newEdges),     setOptions(options),     destroy()   }
Callbacki do C#: onNodeClick(nodeId), onNodeDoubleClick(nodeId)
Jednokierunkowy przepływ: Blazor → vis.js
Wizualizacja i UX
Węzeł technologii na grafie (vis.js custom HTML shape):
Prostokąt z zaokrąglonymi rogami (200px max szerokość)
Nazwa z prefiksem (word wrap, max 2-3 linie)
Kolor tła zależny od postępu (0% czerwony → 50% żółty → 100% zielony)
Pasek postępu poziomy na dole (gradient kolorystyczny)
Procent postępu w prawym dolnym rogu
Przycisk "Szukaj technologii" widoczny przy hover
Tooltip z pełną nazwą przy hover (jeśli truncated)
Węzeł "Start" (specjalny styl):
1.5x większy od standardowych węzłów
Gradient niebieski (niezależny od systemu postępu)
Ikona flagi/gwiazdy w centrum
Border z efektem glow
Postęp 100%, bez paska postępu
Chroniony przed usunięciem
Popup 1 - Opcje technologii (TechnologyOptionsModal):
Textarea dla prywatnego opisu (max 2000 znaków, licznik)
Range Slider dla postępu (0-100%)
Przyciski: "Zapisz", "Anuluj", "Usuń z grafu"
Potwierdzenie przed usunięciem
Brak przycisku "Usuń" dla węzła Start
Popup 2 - Rekomendacje AI (AIRecommendationsModal):
Karty technologii z checkboxami (Card Flowbite)
Każda karta: nazwa, prefix, tag (badge), opis, AI reasoning
Zaznaczona karta: border w kolorze primary
Przyciski na dole: "Dodaj do grafu", "Ignoruj"
Accordion "Ignorowane (X)" poniżej rekomendacji
Przycisk "Aktywuj" dla ignorowanych technologii
Spinner podczas generowania (do 20s)
Kolejność: zgodna z API (priorytet AI)
NavMenu (minimalistyczny):
Logo aplikacji po lewej
Przełącznik dark/light mode (ikona słońca/księżyca)
Ikona profilu (link do /profile)
Przycisk "Wyloguj"
Fixed position, wysokość ~64px
Dark mode:
ThemeService z metodami: ToggleTheme(), GetCurrentTheme(), event OnThemeChanged
Zapis w localStorage przez JS Interop
Aplikacja klasy dark na <html> (Tailwind convention)
Flowbite wspiera dark mode out-of-the-box
Formularze i walidacja
Formularz profilu (onboarding):
Select multi-select dla głównych technologii (min 5, max 50)
Radio buttons dla roli: Programista / Tester / Analityk / Data Science Specialist
Radio buttons dla obszaru: Backend / Frontend / Full Stack / Testing / Data Science / DevOps
Walidacja: DataAnnotations + <EditForm> + <ValidationMessage>
Walidacja tylko on submit, blokada przycisku jeśli błędy
Automatyczne przekierowanie po wypełnieniu: /profile → /graph
Formularz custom technologii:
Pola: nazwa, prefix, tag, opis systemowy
Walidacja tylko on submit
Blokada przycisku "Dodaj" jeśli błędy
Mapowanie błędów API 400 na konkretne pola
Obsługa błędów walidacji z API:
Parsowanie errors array z API (field + message)
Dodawanie do EditContext.AddValidationMessage()
Wyświetlanie przez <ValidationMessage For="..." />
Toast z ogólnym komunikatem "Popraw błędy w formularzu"
Czerwony border na polach z błędami
Routing i nawigacja
Blazor Router (struktura stron):
/ - redirect do /login lub /graph (zależnie od auth)
/login - strona logowania (Google OAuth button)
/profile - formularz profilu (onboarding + edycja)
/graph - główny widok grafu technologii
/admin - panel administratora (opcjonalnie)
<AuthorizeView> dla chronionych stron
RedirectToLogin.razor dla nieautoryzowanych
Przepływ onboardingu:
Użytkownik klika "Zaloguj przez Google"
OAuth callback → API tworzy/pobiera użytkownika
Jeśli hasProfile = false → redirect /profile
Wypełnienie profilu → POST /api/profile → auto-tworzenie węzła "Start"
Redirect /graph → wyświetlenie grafu z węzłem Start
Tekst pomocniczy: "Kliknij Start i wybierz 'Szukaj nowych technologii'"
Przepływ dodawania technologii:
Kliknięcie węzła → przyciski "Opcje" i "Szukaj nowych technologii"
"Szukaj nowych technologii" → Popup 2 z spinnerem
POST /api/ai/recommendations (timeout 20s)
Wyświetlenie ~10 rekomendacji jako karty z checkboxami
Zaznaczenie technologii → "Dodaj do grafu"
POST /api/technologies/batch → dodanie do grafu
Inkrementalna aktualizacja grafu (addNodes/addEdges)
Toast "Dodano X technologii"
Wydajność i optymalizacja
Aktualizacja grafu:
Inkrementalna: GraphVisualization.addNodes() + addEdges()
Unikanie pełnego updateGraph() po każdej operacji
Lokalna aktualizacja stanu w GraphStateService
Płynne animacje w vis.js
Cache:
Brak cache po stronie klienta
Poleganie na cache serwerowym (24h TTL dla rekomendacji AI)
Blazor Server utrzymuje stan sesji między renderami
Konfiguracja vis.js:
Klasa GraphVisualizationOptions.cs w C#
Sekcja appsettings.json → GraphVisualization
Przekazanie do JS przez JS Interop przy inicjalizacji
Parametry: kolory węzłów, rozmiary fontów, opcje layoutu, kolory krawędzi
Obsługa błędów i stanów
System obsługi błędów (warstwowy):
<ErrorBoundary> w App.razor dla błędów renderowania
GlobalExceptionHandler (middleware) dla błędów API
Try-catch w serwisach API z mapowaniem na typowane wyjątki
ToastService dla wyświetlania błędów użytkownikowi
Komponenty UI nie zawierają try-catch
Toasty (Toast Flowbite):
Błędy walidacji formularzy: inline alerts przy polach
Błędy operacyjne (timeout AI, 500, 502): toasty w prawym górnym rogu
Auto-hide po 5 sekundach
Kolory: czerwony (error), żółty (warning), zielony (success)
Krytyczne błędy: modalne dialogi
Stany ładowania:
Lokalny stan w komponentach: isLoadingRecommendations w Popup 2
Globalny LoadingService dla full-page operations
Overlay z spinnerem dla pierwszego załadowania grafu
Spinner + tekst + licznik sekund dla AI (do 20s)
Obsługa batch operations (207 Multi-Status):
Toast z tytułem "Dodano X z Y technologii"
Lista błędów dla nieudanych operacji
Przycisk "Pokaż szczegóły" rozwijający listę
Pełny sukces (201): prosty toast "Dodano X technologii"
Sesje i bezpieczeństwo
Sesje:
7 dni sliding expiration (server-side)
Automatyczne wylogowanie po wygaśnięciu
Przekierowanie do /login + toast "Sesja wygasła"
Brak countdown timera
Blazor Server auto-reconnection
Wylogowanie:
Bezpośrednie (bez potwierdzenia)
POST /api/auth/logout → redirect /login
Toast "Wylogowano pomyślnie"
Czyszczenie sesji po stronie serwera
3. UI Architecture Planning Summary
Przegląd architektury
Aplikacja DeveloperGoals MVP została zaprojektowana jako Blazor Server z wykorzystaniem Flowbite (komponenty UI), Tailwind CSS (styling) i vis.js (wizualizacja grafu). Architektura opiera się na wzorcu Service Layer z dedykowanymi serwisami dla zarządzania stanem i komunikacji z API.
Główne widoki i ekrany
1. Strona logowania (/login)
Minimalistyczny design z logo aplikacji
Przycisk "Zaloguj się przez Google" (Google OAuth 2.0)
Przekierowanie do Google → callback → utworzenie/pobranie użytkownika
Routing po logowaniu: użytkownik bez profilu → /profile, z profilem → /graph
2. Strona profilu (/profile)
Onboarding (pierwsze wypełnienie):
Nagłówek: "Witaj! Uzupełnij swój profil, aby rozpocząć"
Krótki opis celu aplikacji (2-3 zdania)
Formularz:
Select multi-select: główne technologie (min 5, max 50)
Radio buttons: rola (Programista / Tester / Analityk / Data Science Specialist)
Radio buttons: obszar rozwoju (Backend / Frontend / Full Stack / Testing / Data Science / DevOps)
Walidacja: DataAnnotations, tylko on submit, blokada przycisku
Edycja: dostępna w każdym momencie przez NavMenu
Akcja po zapisie: POST /api/profile → auto-tworzenie węzła "Start" → redirect /graph
3. Główny widok grafu (/graph)
Layout:
NavMenu na górze (fixed, 64px)
Graf zajmuje calc(100vh - 64px) - fullscreen
Brak marginesów bocznych
Graf technologii (vis.js):
Węzły: prostokąty z zaokrąglonymi rogami, custom HTML
Węzeł "Start": wyróżniony (1.5x większy, gradient niebieski, ikona, glow)
Węzły technologii: nazwa, kolor tła (postęp), pasek postępu, procent
Krawędzie: strzałki pokazujące zależności
Interakcja: kliknięcie węzła → przyciski "Opcje" i "Szukaj nowych technologii"
Hover na węźle: przycisk "Szukaj nowych technologii"
Pusty graf (tylko Start):
Tekst pomocniczy: "Kliknij Start i wybierz 'Szukaj nowych technologii' aby otrzymać pierwsze rekomendacje"
Dark mode: przełącznik w NavMenu, zapisywany w localStorage
4. Popup 1 - Opcje technologii (Modal)
Trigger: Kliknięcie węzła → przycisk "Opcje"
Zawartość:
Wyświetlanie: nazwa, prefix, tag, opis systemowy
Textarea: prywatny opis (max 2000 znaków, licznik)
Range Slider: postęp (0-100%), wyświetlanie aktualnej wartości
Przyciski: "Zapisz", "Anuluj", "Usuń z grafu"
Akcje:
"Zapisz": PATCH /api/technologies/{id} → zamknięcie popup → odświeżenie węzła → toast
"Usuń": modal potwierdzenia → DELETE /api/technologies/{id} → usunięcie z grafu
Węzeł "Start": brak przycisku "Usuń" (chroniony)
5. Popup 2 - Rekomendacje AI (Modal)
Trigger: Kliknięcie węzła → przycisk "Szukaj nowych technologii"
Proces:
Wyświetlenie spinnera + "Generowanie rekomendacji..." + licznik sekund
POST /api/ai/recommendations (timeout 20s)
Wyświetlenie ~10 rekomendacji
Zawartość:
Sekcja rekomendacji:
Karty technologii z checkboxami (Card Flowbite)
Każda karta: nazwa, prefix, tag (badge), opis, AI reasoning
Zaznaczona karta: border primary
Kolejność: zgodna z API (priorytet AI)
Sekcja ignorowanych (Accordion, domyślnie zwinięty):
Nagłówek: "Ignorowane (X)"
Lista ignorowanych technologii z checkboxami
Przycisk "Aktywuj" na dole
Przyciski:
"Dodaj do grafu": POST /api/technologies/batch → inkrementalna aktualizacja grafu
"Ignoruj": POST /api/ignored-technologies → przeniesienie do sekcji ignorowanych
"Aktywuj" (w sekcji ignorowanych): DELETE /api/ignored-technologies/batch → przywrócenie do rekomendacji
6. Panel administratora (/admin)
Opcjonalny, niski priorytet
Dostęp: hardcoded GoogleId w appsettings.json
Metryki: liczba użytkowników, KPI, popularne kategorie, statystyki AI
Przepływy użytkownika
Przepływ 1: Nowy użytkownik (onboarding)
1. Wejście na stronę → redirect /login2. Kliknięcie "Zaloguj przez Google"3. OAuth flow → callback → utworzenie użytkownika4. Redirect /profile (hasProfile = false)5. Wypełnienie formularza profilu (3 minuty max)6. Submit → POST /api/profile → utworzenie węzła "Start"7. Redirect /graph → wyświetlenie grafu z węzłem Start8. Tekst pomocniczy widoczny
Przepływ 2: Generowanie pierwszych rekomendacji
1. Kliknięcie węzła "Start" na grafie2. Kliknięcie "Szukaj nowych technologii"3. Popup 2 → spinner (do 20s)4. POST /api/ai/recommendations → ~10 technologii5. Przeglądanie kart z rekomendacjami6. Zaznaczenie interesujących (checkboxy)7. Kliknięcie "Dodaj do grafu"8. POST /api/technologies/batch → dodanie do grafu9. Inkrementalna aktualizacja grafu (nowe węzły + krawędzie)10. Toast "Dodano X technologii"
Przepływ 3: Ignorowanie technologii
1. W Popup 2 zaznaczenie nieinteresujących technologii2. Kliknięcie "Ignoruj"3. POST /api/ignored-technologies → zapis w bazie4. Przeniesienie do sekcji "Ignorowane (X)" w tym samym popup5. Technologie znikają z głównej listy rekomendacji
Przepływ 4: Przywracanie ignorowanych
1. W Popup 2 rozwinięcie Accordion "Ignorowane (X)"2. Zaznaczenie technologii do przywrócenia3. Kliknięcie "Aktywuj"4. DELETE /api/ignored-technologies/batch5. Przeniesienie z powrotem do głównej listy rekomendacji6. Możliwość dodania do grafu
Przepływ 5: Aktualizacja postępu nauki
1. Kliknięcie węzła technologii na grafie2. Kliknięcie "Opcje" → Popup 13. Przesunięcie suwaka postępu (0-100%)4. Edycja prywatnego opisu (opcjonalnie)5. Kliknięcie "Zapisz"6. PATCH /api/technologies/{id}7. Zamknięcie popup → odświeżenie węzła (nowy kolor, pasek, procent)8. Toast "Postęp zaktualizowany"
Przepływ 6: Generowanie kolejnych rekomendacji
1. Kliknięcie węzła technologii (nie Start) na grafie2. Kliknięcie "Szukaj nowych technologii"3. POST /api/ai/recommendations z kontekstem:   - fromTechnologyId: wybrany węzeł   - contextTechnologyIds: wszystkie technologie w grafie   - Pełny profil użytkownika4. AI generuje ~10 technologii powiązanych z wybraną5. Wyświetlenie w Popup 2 (w tym ignorowane z poprzednich sesji)6. Dodawanie do grafu jak w przepływie 2
Strategia integracji z API
Warstwa serwisów API
ProfileApiService (scoped):
GetProfileAsync() → GET /api/profile
CreateProfileAsync(dto) → POST /api/profile
UpdateProfileAsync(dto) → PUT /api/profile
Obsługa błędów: 404 (brak profilu), 400 (walidacja), 409 (konflikt)
TechnologyApiService (scoped):
GetTechnologiesAsync() → GET /api/technologies
GetTechnologyAsync(id) → GET /api/technologies/{id}
GetGraphAsync() → GET /api/technologies/graph
AddTechnologyAsync(dto) → POST /api/technologies
AddTechnologiesBatchAsync(dto) → POST /api/technologies/batch
AddCustomTechnologyAsync(dto) → POST /api/technologies/custom
UpdateTechnologyAsync(id, dto) → PATCH /api/technologies/{id}
DeleteTechnologyAsync(id) → DELETE /api/technologies/{id}
Obsługa błędów: 403 (Start node), 404, 400 (walidacja), 409 (konflikt)
AIRecommendationApiService (scoped):
GenerateRecommendationsAsync(fromTechId, contextIds) → POST /api/ai/recommendations
Timeout: 20 sekund
Obsługa błędów: 408 (timeout), 400 (brak profilu), 500 (AI error), 502 (invalid response)
Zwraca: lista rekomendacji + metadata (fromCache, count, generatedAt)
IgnoredTechnologyApiService (scoped):
GetIgnoredTechnologiesAsync() → GET /api/ignored-technologies
AddIgnoredTechnologiesAsync(dtos) → POST /api/ignored-technologies
RestoreIgnoredTechnologyAsync(id) → DELETE /api/ignored-technologies/{id}
RestoreIgnoredTechnologiesBatchAsync(ids) → DELETE /api/ignored-technologies/batch
Obsługa błędów: 404, 409 (duplicate)
Zarządzanie stanem
GraphStateService (scoped):
Stan:
List<TechnologyNode> Nodes
List<TechnologyEdge> Edges
TechnologyNode? SelectedNode
Metody:
LoadGraphAsync() - pobiera graf z API
AddTechnologyAsync(dto) - dodaje technologię
AddTechnologiesBatchAsync(dtos) - dodaje wiele technologii
UpdateTechnologyAsync(id, dto) - aktualizuje technologię
DeleteTechnologyAsync(id) - usuwa technologię
SelectNode(id) - ustawia wybrany węzeł
Eventy:
OnGraphChanged - emitowany po każdej zmianie grafu
OnNodeSelected - emitowany po wyborze węzła
Integracja: komponenty subskrybują eventy i reagują na zmiany
ThemeService (singleton):
ToggleTheme() - przełącza dark/light
GetCurrentTheme() - zwraca aktualny motyw
OnThemeChanged - event emitowany po zmianie
Zapis w localStorage przez JS Interop
Aplikacja klasy dark na <html>
LoadingService (singleton):
IsLoading (bool) - globalny stan ładowania
ShowLoading(message) - pokazuje overlay z spinnerem
HideLoading() - ukrywa overlay
Komponenty subskrybują IsLoading dla wyświetlenia loadera
ToastService (singleton):
ShowSuccess(message) - zielony toast
ShowError(message) - czerwony toast
ShowWarning(message) - żółty toast
ShowInfo(message) - niebieski toast
Auto-hide po 5 sekundach
Pozycja: prawy górny róg
Przepływ danych
Komponent UI    ↓ (wywołanie metody)GraphStateService / Serwis API    ↓ (HTTP request)Backend API    ↓ (response)Serwis API (parsowanie, obsługa błędów)    ↓ (aktualizacja stanu)GraphStateService (emit event)    ↓ (subskrypcja)Komponent UI (re-render)    ↓ (JS Interop)vis.js (aktualizacja grafu)
Integracja vis.js przez JavaScript Interop
Plik: wwwroot/js/graph-visualization.js
window.GraphVisualization = {    network: null,    dotNetHelper: null,        initialize: function(containerId, dotNetHelper, options) {        this.dotNetHelper = dotNetHelper;        const container = document.getElementById(containerId);                // Konfiguracja vis.js        const visOptions = {            nodes: {                shape: 'box',                borderWidth: 2,                borderWidthSelected: 3,                font: { size: 14 },                margin: 10            },            edges: {                arrows: 'to',                smooth: { type: 'cubicBezier' }            },            physics: {                enabled: true,                hierarchicalRepulsion: {                    nodeDistance: 150                }            },            interaction: {                hover: true,                tooltipDelay: 200            }        };                // Merge z opcjami z C#        const finalOptions = { ...visOptions, ...options };                this.network = new vis.Network(container, { nodes: [], edges: [] }, finalOptions);                // Event handlers        this.network.on('click', (params) => {            if (params.nodes.length > 0) {                this.dotNetHelper.invokeMethodAsync('OnNodeClick', params.nodes[0]);            }        });                this.network.on('doubleClick', (params) => {            if (params.nodes.length > 0) {                this.dotNetHelper.invokeMethodAsync('OnNodeDoubleClick', params.nodes[0]);            }        });    },        updateGraph: function(nodes, edges) {        if (this.network) {            this.network.setData({ nodes: nodes, edges: edges });        }    },        addNodes: function(newNodes) {        if (this.network) {            const nodes = this.network.body.data.nodes;            nodes.add(newNodes);        }    },        addEdges: function(newEdges) {        if (this.network) {            const edges = this.network.body.data.edges;            edges.add(newEdges);        }    },        setOptions: function(options) {        if (this.network) {            this.network.setOptions(options);        }    },        destroy: function() {        if (this.network) {            this.network.destroy();            this.network = null;        }    }};
Komponent: GraphCanvas.razor
@inject IJSRuntime JS@implements IAsyncDisposable<div id="graph-container" style="width: 100%; height: 100%;"></div>@code {    [Parameter] public EventCallback<int> OnNodeClick { get; set; }    [Parameter] public EventCallback<int> OnNodeDoubleClick { get; set; }        private DotNetObjectReference<GraphCanvas>? dotNetHelper;    private IJSObjectReference? jsModule;        protected override async Task OnAfterRenderAsync(bool firstRender)    {        if (firstRender)        {            jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./js/graph-visualization.js");            dotNetHelper = DotNetObjectReference.Create(this);                        var options = new {                // Opcje z GraphVisualizationOptions            };                        await jsModule.InvokeVoidAsync("GraphVisualization.initialize", "graph-container", dotNetHelper, options);        }    }        public async Task UpdateGraphAsync(List<NodeDto> nodes, List<EdgeDto> edges)    {        if (jsModule != null)        {            await jsModule.InvokeVoidAsync("GraphVisualization.updateGraph", nodes, edges);        }    }        public async Task AddNodesAsync(List<NodeDto> nodes)    {        if (jsModule != null)        {            await jsModule.InvokeVoidAsync("GraphVisualization.addNodes", nodes);        }    }        [JSInvokable]    public async Task OnNodeClickHandler(int nodeId)    {        await OnNodeClick.InvokeAsync(nodeId);    }        [JSInvokable]    public async Task OnNodeDoubleClickHandler(int nodeId)    {        await OnNodeDoubleClick.InvokeAsync(nodeId);    }        public async ValueTask DisposeAsync()    {        if (jsModule != null)        {            await jsModule.InvokeVoidAsync("GraphVisualization.destroy");            await jsModule.DisposeAsync();        }        dotNetHelper?.Dispose();    }}
Responsywność i dostępność
Responsywność
Decyzja: Brak wsparcia dla urządzeń mobilnych w MVP
Desktop only: Minimalna szerokość ekranu 1024px
Layout: Fullscreen graf bez marginesów
Flowbite: Komponenty responsywne out-of-the-box (dla przyszłych iteracji)
Dostępność
Keyboard navigation: Obsługa Tab, Enter, Escape w modalach
ARIA labels: Dla przycisków, formularzy, modali
Kontrast kolorów: Zgodność z WCAG 2.1 AA (Tailwind + Flowbite)
Focus indicators: Widoczne dla użytkowników klawiatury
Screen readers: Semantyczny HTML, role ARIA
Bezpieczeństwo na poziomie UI
Autentykacja i autoryzacja
Google OAuth 2.0: Jedyna metoda logowania
Session management: Server-side (Blazor Server)
AuthorizeView: Ochrona komponentów i stron
RedirectToLogin: Automatyczne przekierowanie dla nieautoryzowanych
Admin access: Hardcoded GoogleId w appsettings.json
Izolacja danych
UserId: Zawsze z authenticated context (nigdy z klienta)
API calls: Wszystkie zawierają session cookie
Brak cross-user access: Każdy użytkownik widzi tylko swoje dane
Walidacja: Po stronie serwera (API) + klienta (UI)
Ochrona przed atakami
XSS: Blazor automatycznie escapuje output
CSRF: Built-in protection w Blazor Server
SQL Injection: Entity Framework parametryzowane zapytania
HTTPS: Wymagane w produkcji (Google OAuth requirement)
Konfiguracja i ustawienia
appsettings.json
{  "GraphVisualization": {    "NodeColors": {      "Progress0": "#EF4444",      "Progress50": "#F59E0B",      "Progress100": "#10B981"    },    "FontSizes": {      "NodeTitle": 14,      "NodeSubtitle": 12    },    "Layout": {      "Type": "hierarchical",      "NodeDistance": 150    },    "EdgeColors": {      "Default": "#6B7280",      "Hover": "#3B82F6"    }  },  "AdminSettings": {    "GoogleIds": ["1234567890", "0987654321"]  }}
GraphVisualizationOptions.cs
public class GraphVisualizationOptions{    public NodeColorsOptions NodeColors { get; set; }    public FontSizesOptions FontSizes { get; set; }    public LayoutOptions Layout { get; set; }    public EdgeColorsOptions EdgeColors { get; set; }}
Testowanie UI
Priorytety testowania
Krytyczne:
Przepływ onboardingu (login → profil → graf)
Generowanie rekomendacji AI (timeout, błędy)
Dodawanie technologii do grafu (single + batch)
Aktualizacja postępu (suwak, zapis)
Integracja vis.js (renderowanie, interakcje)
Wysokie:
Ignorowanie i przywracanie technologii
Edycja profilu (bez regeneracji grafu)
Usuwanie technologii (ochrona Start node)
Dark mode (przełączanie, localStorage)
Walidacja formularzy (inline, API errors)
Średnie:
Wylogowanie i sesje
Toasty i komunikaty błędów
Custom technologie
Panel administratora
Typy testów
Unit tests: Serwisy API, GraphStateService, walidatory
Integration tests: Przepływy użytkownika, komunikacja z API
E2E tests: Kluczowe scenariusze (Playwright/Selenium)
Manual testing: Wizualizacja grafu, UX, responsywność
4. Unresolved Issues (Nierozwiązane kwestie)
Techniczne
Orphaned technologies po usunięciu węzła:
Czy technologie bez incoming edges powinny być automatycznie usuwane?
Decyzja MVP: NIE - użytkownik usuwa ręcznie
Do rozważenia: Pokazać ostrzeżenie o orphaned nodes przy usuwaniu
Limity wydajnościowe vis.js:
Maksymalna liczba węzłów na grafie przed spadkiem wydajności?
Rekomendacja: Testowanie z 100+ węzłami
Opcja: Lazy loading lub paginacja dla bardzo dużych grafów (post-MVP)
Strategia aktualizacji grafu przy długich sesjach:
Czy implementować periodic refresh z API?
Decyzja MVP: Brak auto-refresh, użytkownik odświeża ręcznie (F5)
Post-MVP: SignalR dla real-time updates
Obsługa konfliktów wersji (optimistic concurrency):
Czy implementować versioning dla technologii?
Decyzja MVP: Brak - last write wins
Post-MVP: ETag lub timestamp-based concurrency
UX/UI
Animacje przejść między widokami:
Czy dodać page transitions?
Decyzja MVP: Brak animacji (focus na funkcjonalność)
Post-MVP: Smooth transitions z Tailwind animations
Onboarding dla zaawansowanych funkcji:
Czy dodać tooltips/hints dla mniej oczywistych funkcji?
Decyzja MVP: Tylko tekst pomocniczy dla pustego grafu
Post-MVP: Interactive tour (np. Shepherd.js)
Eksport/import grafu:
Czy użytkownik powinien móc eksportować graf jako obraz/PDF?
Poza zakresem MVP (PRD line 273)
Post-MVP: Export do PNG/SVG, udostępnianie linku
Biznesowe
Metryki użytkowania UI:
Jakie eventy trackować (analytics)?
Rekomendacja: Google Analytics lub Application Insights
Eventy: kliknięcia węzłów, generowanie AI, dodawanie technologii, czas sesji
Feedback loop dla rekomendacji AI:
Czy użytkownik powinien móc oceniać jakość rekomendacji?
Poza zakresem MVP (PRD line 279)
Post-MVP: Thumbs up/down, komentarze
Limity użytkowania:
Czy wprowadzić limity liczby technologii na użytkownika?
Decyzja MVP: Brak limitów (PRD line 791)
Monitoring: Śledzenie średniej liczby technologii per user
Wydajność
Strategia cache dla dużych grafów:
Czy cachować graf po stronie klienta (IndexedDB)?
Decyzja MVP: Brak - poleganie na Blazor Server state
Post-MVP: Rozważyć dla bardzo dużych grafów (>200 węzłów)
Optymalizacja renderowania vis.js:
Czy używać canvas czy SVG rendering?
Rekomendacja: Canvas (domyślne w vis.js, lepsze dla dużych grafów)
Testowanie: Porównanie wydajności canvas vs SVG
Integracja
Fallback dla JavaScript disabled:
Czy obsługiwać użytkowników bez JS?
Decyzja MVP: NIE - Blazor Server wymaga JS
Komunikat: "JavaScript is required" jeśli wyłączony
Offline mode:
Czy aplikacja powinna działać offline?
Poza zakresem MVP (wymaga Blazor WebAssembly + PWA)
Post-MVP: Rozważyć migrację na Blazor WASM lub hybrid
Browser compatibility:
Minimalne wersje przeglądarek?
Rekomendacja: Chrome 90+, Firefox 88+, Edge 90+, Safari 14+
Testowanie: Cross-browser testing dla kluczowych funkcji
</conversation_summary>