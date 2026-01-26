# Architektura interfejsu użytkownika MVP
_Exported on 1/11/2026 at 20:14:38 GMT+1 from Cursor (2.3.34)_

---

**User**

Jesteś asystentem AI, którego zadaniem jest pomoc w zaplanowaniu architektury interfejsu użytkownika dla MVP (Minimum Viable Product) na podstawie dostarczonych informacji. Twoim celem jest wygenerowanie listy pytań i zaleceń, które zostaną wykorzystane w kolejnym promptowaniu do utworzenia szczegółowej architektury UI, map podróży użytkownika i struktury nawigacji.

Prosimy o uważne zapoznanie się z poniższymi informacjami:

<product_requirements>
@prd.md
</product_requirements>

<tech_stack>
@tech-stack.md
</tech_stack>

<api_plan>
@4-api-plan.md
</api_plan>

Przeanalizuj dostarczone informacje, koncentrując się na aspektach istotnych dla projektowania interfejsu użytkownika.
Rozważ następujące kwestie:

1. Zidentyfikuj kluczowe widoki i ekrany na podstawie wymagań produktu i dostępnych endpointów API.
2. Określ potencjalne przepływy użytkownika i nawigację między widokami, uwzględniając możliwości API.
3. Rozważ komponenty UI i wzorce interakcji, które mogą być konieczne do efektywnej komunikacji z API.
4. Pomyśl o responsywności i dostępności interfejsu.
5. Oceń wymagania bezpieczeństwa i uwierzytelniania w kontekście integracji z API.
6. Rozważ wszelkie konkretne biblioteki UI lub frameworki, które mogą być korzystne dla projektu.
7. Przeanalizuj, jak struktura API wpływa na projekt UI i przepływy danych w aplikacji.

Na podstawie analizy wygeneruj listę 10 pytań i zaleceń w formie łączonej (pytanie + zalecenie). Powinny one dotyczyć wszelkich niejasności, potencjalnych problemów lub obszarów, w których potrzeba więcej informacji, aby stworzyć efektywną architekturę UI. Rozważ pytania dotyczące:

1. Hierarchia i organizacja widoków w odniesieniu do struktury API
2. Przepływy użytkownika i nawigacja wspierane przez dostępne endpointy
3. Responsywność i adaptacja do różnych urządzeń
4. Dostępność i inkluzywność
5. Bezpieczeństwo i autoryzacja na poziomie UI w powiązaniu z mechanizmami API
6. Spójność designu i doświadczenia użytkownika
7. Strategia zarządzania stanem aplikacji i synchronizacji z API
8. Obsługa stanów błędów i wyjątków zwracanych przez API
9. Strategie buforowania i optymalizacji wydajności w komunikacji z API

Dane wyjściowe powinny mieć następującą strukturę:

<pytania>
W tym miejscu proszę wymienić pytania i zalecenia, dla przejrzystości opatrzone numerami:

Na przykład:
1. Czy na pocztówce powinno znajdować się nazwisko autora?

Rekomendacja: Tak, na pocztówce powinno znajdować się nazwisko autora.
</pytania>

Pamiętaj, że Twoim celem jest dostarczenie kompleksowej listy pytań i zaleceń, które pomogą w stworzeniu solidnej architektury UI dla MVP, w pełni zintegrowanej z dostępnymi endpointami API. Skoncentruj się na jasności, trafności i dokładności swoich wyników. Nie dołączaj żadnych dodatkowych komentarzy ani wyjaśnień poza określonym formatem wyjściowym.

Kontynuuj ten proces, generując nowe pytania i rekomendacje w oparciu o przekazany kontekst i odpowiedzi użytkownika, dopóki użytkownik wyraźnie nie poprosi o podsumowanie.

Pamiętaj, aby skupić się na jasności, trafności i dokładności wyników. Nie dołączaj żadnych dodatkowych komentarzy ani wyjaśnień poza określonym formatem wyjściowym.

---

**Cursor**

Przeanalizuję dostarczone dokumenty, aby wygenerować pytania i zalecenia dotyczące architektury UI dla MVP.

Na podstawie analizy dokumentów PRD, tech-stack oraz API plan, przygotowałem pytania i zalecenia dotyczące architektury UI dla MVP:

<pytania>

1. Czy główny widok grafu technologii powinien być renderowany jako komponent Blazor z integracją vis.js przez JavaScript Interop, czy jako osobna strona HTML z pełnym JS?

Rekomendacja: Użyj komponentu Blazor z JavaScript Interop dla vis.js. To zapewni spójność z architekturą Blazor Server, umożliwi łatwą komunikację z backendem i zachowa reaktywność interfejsu bez przeładowywania strony.

2. Czy Popup 1 (Opcje technologii) i Popup 2 (Rekomendacje AI) powinny być implementowane jako modalne okna Flowbite, czy jako osobne komponenty Blazor z własną logiką nawigacji?

Rekomendacja: Wykorzystaj komponenty modalne z Flowbite dla spójności designu, ale opakuj je w komponenty Blazor dla łatwego zarządzania stanem. Popup 1 może być prostym modalem, podczas gdy Popup 2 powinien obsługiwać dynamiczne ładowanie i checkboxy dla wielu technologii.

3. Jak powinna wyglądać nawigacja główna aplikacji - czy klasyczny navbar z linkami (Profil, Graf, Wyloguj), czy minimalistyczny interfejs z fokusem na graf i hamburger menu?

Rekomendacja: Zastosuj minimalistyczny navbar z logo aplikacji po lewej, ikoną profilu i przyciskiem wylogowania po prawej. Graf technologii powinien zajmować maksymalną przestrzeń ekranu. Użyj komponentu Navbar z Flowbite dla responsywności.

4. Czy formularz profilu (onboarding) powinien być implementowany jako wieloetapowy wizard (multi-step form), czy jako pojedyncza strona z wszystkimi polami?

Rekomendacja: Użyj pojedynczej strony z wszystkimi polami, ponieważ PRD zakłada maksymalnie 3 minuty wypełniania. Zastosuj komponent Select z Flowbite dla technologii (multi-select), Radio buttons dla roli i obszaru rozwoju. Dodaj walidację po stronie klienta dla lepszego UX.

5. Jak powinien być wizualizowany postęp nauki technologii na grafie - jako kolor węzła, pasek postępu, czy tekst procentowy?

Rekomendacja: Zastosuj kombinację: kolor węzła (gradient od czerwonego przez żółty do zielonego w zależności od postępu) + tekst procentowy widoczny przy hover. W Popup 1 użyj komponentu Range Slider z Flowbite dla edycji postępu.

6. Czy podczas generowania rekomendacji AI (timeout 20s) powinien być wyświetlany tylko spinner, czy również pasek postępu lub animowana informacja o trwającym procesie?

Rekomendacja: Użyj komponentu Spinner z Flowbite wraz z tekstem "Generowanie rekomendacji..." i opcjonalnie animowanym licznikiem sekund. Nie używaj paska postępu (brak możliwości określenia rzeczywistego postępu), ale dodaj informację "To może potrwać do 20 sekund".

7. Jak powinna być zorganizowana lista ignorowanych technologii w Popup 2 - jako osobna zakładka (tab), rozwijana sekcja (accordion), czy zawsze widoczna sekcja poniżej rekomendacji?

Rekomendacja: Użyj rozwijanej sekcji (Accordion z Flowbite) poniżej głównej listy rekomendacji. Sekcja powinna być domyślnie zwinięta z licznikiem ignorowanych technologii w nagłówku (np. "Ignorowane (5)"). To oszczędza przestrzeń i nie przytłacza użytkownika.

8. Czy aplikacja powinna obsługiwać tryb ciemny (dark mode) już w MVP, biorąc pod uwagę grupę docelową (programiści)?

Rekomendacja: Tak, dodaj przełącznik dark/light mode w navbarze. Flowbite ma wbudowane wsparcie dla dark mode z Tailwind CSS. Programiści często preferują ciemny motyw, a implementacja z Tailwind jest relatywnie prosta (klasa `dark:` dla stylów). Zapisuj preferencję w localStorage.

9. Jak powinna być zorganizowana responsywność dla urządzeń mobilnych - czy graf powinien być w pełni interaktywny na małych ekranach, czy lepiej pokazać uproszczony widok listy technologii?

Rekomendacja: Na ekranach > 768px pokazuj pełny graf. Na urządzeniach mobilnych (< 768px) wyświetl uproszczony widok listy z kartami technologii (Cards z Flowbite), zachowując możliwość kliknięcia dla Popup 1 i Popup 2. Dodaj przycisk "Pokaż graf" dla użytkowników, którzy chcą zobaczyć wizualizację na mobile.

10. Czy komunikaty o błędach (timeout AI, błędy walidacji, błędy serwera) powinny być wyświetlane jako toasty, alerty inline, czy modalne okna dialogowe?

Rekomendacja: Zastosuj system warstwowy: błędy walidacji formularzy - inline alerts (Alert z Flowbite) przy polach, błędy operacyjne (timeout AI, błędy serwera) - toast notifications w prawym górnym rogu (Toast z Flowbite) z auto-hide po 5 sekundach, krytyczne błędy wymagające akcji użytkownika - modalne dialogi. Użyj kolorów: czerwony (error), żółty (warning), zielony (success).

</pytania>

---

**User**

1.  Użyj komponentu Blazor z JavaScript Interop dla vis.js
2.  Postępuj zgodnie z Twoimi rekomendacjami
3. Postępuj zgodnie z Twoimi rekomendacjami
4. Postępuj zgodnie z Twoimi rekomendacjami
5. Postępuj zgodnie z Twoimi rekomendacjami, ale postęp węzła chcę widzieć jako kolor, pasek postępu i procent, wszystko na raz widoczne
6. Postępuj zgodnie z Twoimi rekomendacjami
7.  Postępuj zgodnie z Twoimi rekomendacjami
8.  Postępuj zgodnie z Twoimi rekomendacjami
9. Nie wspieraj urządzeń mobilnych
10. Postępuj zgodnie z Twoimi rekomendacjami

---

**Cursor**

Dziękuję za potwierdzenie decyzji! Na podstawie Twoich odpowiedzi przygotowuję kolejny zestaw pytań i zaleceń dotyczących szczegółów implementacji architektury UI:

<pytania>

1. Jak powinna być zorganizowana struktura komponentów Blazor - czy jeden duży komponent `TechnologyGraph.razor` z całą logiką, czy podział na mniejsze komponenty (`GraphVisualization.razor`, `TechnologyPopup.razor`, `RecommendationsPopup.razor`)?

Rekomendacja: Zastosuj podział na mniejsze, reużywalne komponenty:
- `TechnologyGraph.razor` - główny kontener z logiką stanu
- `GraphCanvas.razor` - wrapper dla vis.js z JS Interop
- `TechnologyOptionsModal.razor` - Popup 1 (edycja, postęp, usuwanie)
- `AIRecommendationsModal.razor` - Popup 2 (rekomendacje + ignorowane)
- `TechnologyNode.razor` - komponent reprezentujący węzeł (dla potencjalnego widoku listy)
To ułatwi testowanie, utrzymanie kodu i reużywalność.

2. Gdzie powinna być przechowywana logika zarządzania stanem grafu - bezpośrednio w komponencie, w serwisie Blazor (scoped), czy w globalnym state management (np. Fluxor)?

Rekomendacja: Użyj dedykowanego serwisu `GraphStateService` (scoped) wstrzykiwanego do komponentów. Serwis powinien przechowywać aktualny stan grafu, obsługiwać operacje CRUD przez API i emitować eventy o zmianach. To oddziela logikę biznesową od prezentacji i ułatwia synchronizację między komponentami bez nadmiernej złożoności Fluxor dla MVP.

3. Jak powinna być obsługiwana synchronizacja danych między vis.js (JavaScript) a komponentami Blazor - czy przez polling, eventy JS Interop, czy dwukierunkowe bindowanie?

Rekomendacja: Użyj jednokierunkowego przepływu danych: Blazor → vis.js przez JS Interop. Kliknięcia na węzłach grafu powinny wywoływać callback do C# przez `DotNetObjectReference`. Unikaj pollingu. Implementuj metody JS: `updateGraph(nodes, edges)`, `onNodeClick(nodeId)`, `onNodeDoubleClick(nodeId)` z callbackami do Blazor.

4. Jak powinien wyglądać wizualnie węzeł na grafie vis.js - jako okrąg, prostokąt z zaokrąglonymi rogami, czy custom shape z HTML?

Rekomendacja: Użyj prostokąta z zaokrąglonymi rogami (`shape: 'box'` w vis.js). Węzeł powinien zawierać:
- Nazwę technologii (z prefiksem, np. "DotNet - EF Core")
- Pasek postępu poziomy na dole węzła (gradient kolorystyczny)
- Procent postępu w prawym dolnym rogu
- Kolor tła węzła zależny od postępu (0% - czerwony, 50% - żółty, 100% - zielony)
- Ikona kategorii (opcjonalnie) w lewym górnym rogu
Custom HTML shape da największą elastyczność dla wyświetlania wszystkich elementów.

5. Jak powinna być zorganizowana komunikacja z API - czy każdy komponent powinien wywoływać API bezpośrednio, czy przez dedykowany serwis HTTP?

Rekomendacja: Stwórz dedykowane serwisy API dla każdego zasobu:
- `ProfileApiService` - operacje na profilu
- `TechnologyApiService` - CRUD technologii
- `AIRecommendationApiService` - generowanie rekomendacji
- `IgnoredTechnologyApiService` - zarządzanie ignorowanymi
Każdy serwis powinien być wstrzykiwany jako scoped, obsługiwać błędy HTTP i zwracać typowane DTOs. Komponenty komunikują się tylko przez te serwisy.

6. Jak powinna być obsługiwana walidacja formularza profilu - przez wbudowane atrybuty DataAnnotations, FluentValidation, czy custom walidatory Blazor?

Rekomendacja: Użyj DataAnnotations na modelach DTO z komponentem `<EditForm>` i `<DataAnnotationsValidator>`. Dodaj custom walidatory dla specyficznych reguł (np. minimum 5 technologii). Wyświetlaj błędy przez `<ValidationMessage>` inline przy polach. To najprostsze rozwiązanie dla MVP, dobrze integruje się z Blazor i Flowbite.

7. Jak powinien być zorganizowany proces onboardingu - czy po zalogowaniu przez Google OAuth użytkownik bez profilu powinien być automatycznie przekierowany na stronę profilu, czy pokazać welcome screen z przyciskiem "Rozpocznij"?

Rekomendacja: Automatyczne przekierowanie na stronę profilu z przyjaznym nagłówkiem "Witaj! Uzupełnij swój profil, aby rozpocząć" i krótkim opisem (2-3 zdania) wyjaśniającym cel aplikacji. Pomiń welcome screen - PRD zakłada maksymalnie 3 minuty od rejestracji do wypełnienia profilu, więc każdy dodatkowy krok wydłuża proces.

8. Jak powinny być stylowane checkboxy w Popup 2 (wybór wielu technologii do dodania/ignorowania) - jako standardowe checkboxy Flowbite, czy jako karty z możliwością zaznaczenia (card selection)?

Rekomendacja: Użyj kart z możliwością zaznaczenia (Card z Flowbite + checkbox). Każda karta technologii powinna zawierać:
- Checkbox w prawym górnym rogu
- Nazwę z prefiksem
- Tag (badge Flowbite)
- Opis systemowy (2-3 linie)
- AI reasoning w osobnej sekcji (mniejszy font, kursywa)
Zaznaczona karta powinna mieć border w kolorze primary. To bardziej wizualne i przyjazne niż lista z checkboxami.

9. Jak powinna być obsługiwana nawigacja między widokami - przez Blazor Router z osobnymi stronami (@page), czy przez conditional rendering w jednym komponencie?

Rekomendacja: Użyj Blazor Router z osobnymi stronami:
- `/` - strona główna (redirect do `/login` lub `/graph`)
- `/login` - strona logowania (Google OAuth button)
- `/profile` - formularz profilu (onboarding + edycja)
- `/graph` - główny widok grafu technologii
- `/admin` - panel administratora (opcjonalnie)
Dodaj `<AuthorizeView>` dla chronionych stron i komponent `RedirectToLogin.razor` dla nieautoryzowanych użytkowników. To zgodne z konwencjami Blazor i ułatwia deep linking.

10. Jak powinna być zorganizowana obsługa błędów globalnych (500, 502, timeout) - czy przez Blazor Error Boundary, globalny exception handler, czy try-catch w każdym komponencie?

Rekomendacja: Zastosuj podejście warstwowe:
- `<ErrorBoundary>` w `App.razor` dla nieobsłużonych wyjątków renderowania
- Globalny middleware `GlobalExceptionHandler` (już istnieje w projekcie) dla błędów API
- Try-catch w serwisach API z mapowaniem na typowane wyjątki (`AITimeoutException`, `ValidationException`)
- Komponent `ToastService` (singleton) do wyświetlania toastów z błędami, wstrzykiwany do serwisów
Komponenty UI nie powinny zawierać try-catch, tylko obsługiwać zwracane Result<T> lub rzucane wyjątki przez eventy.

11. Jak powinna być zaimplementowana integracja vis.js - czy przez osobny plik JS w wwwroot, czy inline w komponencie Blazor przez `<script>` tag?

Rekomendacja: Stwórz dedykowany plik `wwwroot/js/graph-visualization.js` z obiektem `window.GraphVisualization` zawierającym metody:
- `initialize(containerId, dotNetHelper)` - inicjalizacja grafu
- `updateGraph(nodes, edges)` - aktualizacja danych
- `destroy()` - czyszczenie przy dispose
- `setOptions(options)` - konfiguracja wyglądu
Załaduj plik w `index.html` lub `App.razor`. Unikaj inline scripts - to ułatwi debugowanie, testowanie i reużywalność.

12. Jak powinien być obsługiwany stan ładowania podczas operacji asynchronicznych (pobieranie grafu, generowanie AI) - przez lokalny stan w komponencie, czy globalny loading indicator?

Rekomendacja: Użyj lokalnego stanu w komponentach dla specyficznych operacji (np. `isLoadingRecommendations` w Popup 2) z dedykowanymi spinnerami. Dodaj globalny `LoadingService` (singleton) dla operacji full-page (np. pierwsze załadowanie grafu po logowaniu) z overlay i spinnerem. Komponenty mogą subskrybować `LoadingService.IsLoading` dla wyświetlenia globalnego loadera.

13. Jak powinna być zorganizowana konfiguracja vis.js (kolory, fonty, layout) - hardcoded w JS, w appsettings.json, czy w osobnym pliku konfiguracyjnym?

Rekomendacja: Stwórz klasę `GraphVisualizationOptions.cs` z konfiguracją:
- Kolory węzłów dla różnych poziomów postępu
- Rozmiary fontów
- Opcje layoutu (hierarchical, physics)
- Kolory krawędzi
Załaduj z sekcji `appsettings.json` → `GraphVisualization`. Przekaż do JS przez JS Interop przy inicjalizacji. To umożliwi łatwą zmianę wyglądu bez modyfikacji kodu JS i różne konfiguracje dla dev/prod.

14. Jak powinno być obsługiwane cache rekomendacji AI po stronie klienta - czy komponenty powinny cachować odpowiedzi, czy polegać tylko na cache serwerowym (24h TTL)?

Rekomendacja: Polegaj wyłącznie na cache serwerowym. Nie implementuj dodatkowego cache po stronie klienta - to zwiększa złożoność, może prowadzić do nieaktualnych danych i problemów z synchronizacją. Blazor Server utrzymuje stan sesji, więc dane są dostępne między renderami. Jeśli użytkownik odświeży stronę, świeże pobranie z API trafi w cache serwera.

15. Jak powinna być zorganizowana struktura folderów dla komponentów UI - flat structure w `Components/`, czy hierarchiczna (`Components/Graph/`, `Components/Profile/`, `Components/Shared/`)?

Rekomendacja: Użyj hierarchicznej struktury:
```
Components/
├── Pages/           # Strony z @page directive
│   ├── Login.razor
│   ├── Profile.razor
│   ├── Graph.razor
│   └── Admin.razor
├── Graph/           # Komponenty związane z grafem
│   ├── GraphCanvas.razor
│   ├── TechnologyOptionsModal.razor
│   ├── AIRecommendationsModal.razor
│   └── TechnologyCard.razor
├── Profile/         # Komponenty profilu
│   ├── ProfileForm.razor
│   └── TechnologySelector.razor
├── Shared/          # Reużywalne komponenty
│   ├── Toast.razor
│   ├── LoadingSpinner.razor
│   └── ErrorBoundary.razor
└── Layout/          # Layouty (już istnieje)
    ├── MainLayout.razor
    └── NavMenu.razor
```
To ułatwia nawigację w projekcie i grupuje powiązane komponenty.

</pytania>

---

**User**

1. Postępuj zgodnie z Twoimi rekomendacjami
2. Postępuj zgodnie z Twoimi rekomendacjami
3. Postępuj zgodnie z Twoimi rekomendacjami
4. Postępuj zgodnie z Twoimi rekomendacjami
5. Postępuj zgodnie z Twoimi rekomendacjami
6. Postępuj zgodnie z Twoimi rekomendacjami
7. Postępuj zgodnie z Twoimi rekomendacjami
8. Postępuj zgodnie z Twoimi rekomendacjami
9. Postępuj zgodnie z Twoimi rekomendacjami
10. Postępuj zgodnie z Twoimi rekomendacjami
11. Postępuj zgodnie z Twoimi rekomendacjami
12. Postępuj zgodnie z Twoimi rekomendacjami
13. Postępuj zgodnie z Twoimi rekomendacjami
14. Postępuj zgodnie z Twoimi rekomendacjami
15. Postępuj zgodnie z Twoimi rekomendacjami



---

**Cursor**

Świetnie! Wszystkie kluczowe decyzje architektoniczne zostały potwierdzone. Przygotowuję ostatni zestaw pytań dotyczących szczegółów implementacyjnych i integracji:

<pytania>

1. Jak powinna być obsługiwana aktualizacja grafu po dodaniu nowych technologii - czy pełne przeładowanie danych z API, czy inkrementalna aktualizacja tylko nowych węzłów i krawędzi?

Rekomendacja: Użyj inkrementalnej aktualizacji. Po dodaniu technologii przez API, otrzymujesz nowe węzły i krawędzie w odpowiedzi. Dodaj je do lokalnego stanu w `GraphStateService` i wywołaj `GraphVisualization.addNodes(newNodes)` oraz `GraphVisualization.addEdges(newEdges)` zamiast `updateGraph()` z pełnym zestawem danych. To optymalizuje wydajność i zapewnia płynniejszą animację w vis.js.

2. Jak powinna być zorganizowana obsługa dark mode - czy przełącznik powinien być w NavMenu, osobny floating button, czy w menu użytkownika (dropdown przy ikonie profilu)?

Rekomendacja: Umieść przełącznik dark/light mode w NavMenu po prawej stronie, obok ikony profilu. Użyj ikony słońca/księżyca (Bootstrap Icons) z animowanym przejściem. Stwórz `ThemeService` (singleton) z metodami `ToggleTheme()`, `GetCurrentTheme()` i eventem `OnThemeChanged`. Zapisuj preferencję w localStorage przez JS Interop i aplikuj klasę `dark` na elemencie `<html>` zgodnie z konwencją Tailwind CSS.

3. Jak powinny być obsługiwane konflikty podczas operacji batch (POST /api/technologies/batch) - czy pokazać wszystkie błędy na raz, czy tylko pierwszy błąd?

Rekomendacja: Pokaż wszystkie błędy w jednym toaście z listą. Jeśli API zwróci 207 Multi-Status, wyświetl toast z tytułem "Dodano 3 z 5 technologii" i listą błędów dla nieudanych operacji. Dodaj przycisk "Pokaż szczegóły" rozwijający listę. Dla pełnego sukcesu (201) pokaż prosty toast "Dodano 5 technologii". To daje użytkownikowi pełny obraz sytuacji bez przytłaczania.

4. Jak powinna być zaimplementowana funkcjonalność "Szukaj nowych technologii" - jako przycisk na węźle grafu (wymaga hover/click), czy jako akcja w Popup 1 (Opcje)?

Rekomendacja: Zaimplementuj obie opcje dla lepszego UX:
- Przycisk bezpośrednio na węźle grafu (widoczny przy hover) - szybki dostęp
- Przycisk w Popup 1 (Opcje) - dla użytkowników preferujących menu kontekstowe
Oba przyciski otwierają ten sam Popup 2 (AIRecommendationsModal). W vis.js dodaj custom HTML do węzła z ukrytym przyciskiem pokazującym się przy hover.

5. Jak powinien wyglądać węzeł "Start" na grafie - czy identycznie jak inne węzły, czy z wyróżniającym się stylem (np. większy, inny kolor, ikona)?

Rekomendacja: Wyróżnij węzeł "Start" specjalnym stylem:
- Większy rozmiar (1.5x standardowego węzła)
- Unikalny kolor (gradient niebieski, niezależny od systemu postępu)
- Ikona flagi lub gwiazdy w centrum
- Border z efektem glow
- Postęp zawsze 100%, ale bez paska postępu (niepotrzebny)
- Brak przycisku "Usuń" w Popup 1 (chroniony przed usunięciem)
To wizualnie komunikuje, że jest to punkt początkowy ścieżki rozwoju.

6. Jak powinna być obsługiwana edycja prywatnego opisu technologii w Popup 1 - jako textarea z limitem znaków, rich text editor, czy markdown editor?

Rekomendacja: Użyj prostego `<textarea>` z Flowbite z licznikiem znaków (0/2000) na dole. Brak rich text ani markdown - to MVP, a PRD nie wymaga formatowania. Dodaj placeholder "Dodaj swoje notatki, przemyślenia lub linki jako tekst..." i auto-resize textarea przy wpisywaniu. Zapisuj zmiany przez przycisk "Zapisz" (nie auto-save) dla kontroli użytkownika.

7. Jak powinien być zorganizowany layout strony grafu - czy graf powinien zajmować 100% viewport (fullscreen), czy z marginesami i headerem?

Rekomendacja: Graf powinien zajmować maksymalną dostępną przestrzeń:
- NavMenu na górze (fixed, wysokość ~64px)
- Graf zajmuje `calc(100vh - 64px)` - pełna wysokość minus navbar
- Brak marginesów bocznych dla grafu
- Tło grafu: białe (light mode) / ciemnoszare (dark mode)
To maksymalizuje przestrzeń dla wizualizacji, szczególnie przy dużych grafach.

8. Jak powinna być obsługiwana walidacja po stronie klienta dla formularza dodawania custom technologii - czy walidować w czasie rzeczywistym (on input), przy utracie focusa (on blur), czy tylko przy submit?

Rekomendacja: Zastosuj walidację hybrydową:
- Walidacja długości pól: on input (pokazuj licznik znaków)
- Walidacja wymaganych pól: on blur (czerwony border + komunikat)
- Walidacja całego formularza: on submit (blokada przycisku jeśli błędy)
Użyj `<EditForm>` z `<ValidationMessage>` dla każdego pola. To balansuje między natychmiastowym feedbackiem a nieprzeszkadzaniem użytkownikowi podczas wpisywania.

9. Jak powinny być sortowane rekomendacje AI w Popup 2 - według kolejności zwróconej przez API, alfabetycznie, czy według kategorii (prefix)?

Rekomendacja: Zachowaj kolejność zwróconą przez API - AI generuje rekomendacje w kolejności priorytetowej (najbardziej istotne pierwsze). Dodaj opcjonalny dropdown "Sortuj według" z opcjami:
- Rekomendowane (domyślne, kolejność API)
- Alfabetycznie (A-Z)
- Według kategorii (grupowanie po prefix)
Zapisuj preferencję sortowania w state komponentu (nie localStorage - reset przy każdym otwarciu popup).

10. Jak powinna być obsługiwana sytuacja, gdy użytkownik ma pusty graf (tylko węzeł Start) - czy pokazać tutorial/onboarding overlay, czy tylko tekst pomocniczy?

Rekomendacja: Pokaż subtelny overlay z instrukcją:
- Semi-transparentne tło z blur
- Karta (Card Flowbite) na środku ekranu z:
  - Tytułem "Rozpocznij swoją ścieżkę rozwoju"
  - Krokami: "1. Kliknij węzeł Start, 2. Wybierz 'Szukaj nowych technologii', 3. Dodaj interesujące technologie"
  - Przyciskiem "Rozumiem" zamykającym overlay
- Zapisz w localStorage `hasSeenGraphTutorial: true` aby nie pokazywać ponownie
To pomaga nowym użytkownikom bez irytowania doświadczonych.

11. Jak powinna być zorganizowana obsługa sesji i automatycznego wylogowania - czy pokazać modal z ostrzeżeniem przed wygaśnięciem sesji, czy po prostu wylogować i przekierować?

Rekomendacja: Implementuj automatyczne wylogowanie bez ostrzeżenia:
- Sesja wygasa po 7 dniach (sliding expiration) zgodnie z API plan
- Przy wygaśnięciu sesji: automatyczne przekierowanie do `/login`
- Toast z komunikatem "Sesja wygasła. Zaloguj się ponownie."
- Brak countdown timera (niepotrzebna złożoność dla 7-dniowej sesji)
Blazor Server automatycznie obsługuje reconnection, więc tylko obsłuż 401 Unauthorized z API.

12. Jak powinien być zaimplementowany przycisk "Wyloguj" - czy z potwierdzeniem (modal), czy bezpośrednie wylogowanie?

Rekomendacja: Bezpośrednie wylogowanie bez potwierdzenia:
- Kliknięcie przycisku → wywołanie `POST /api/auth/logout`
- Przekierowanie do `/login`
- Toast "Wylogowano pomyślnie"
- Brak modala potwierdzenia (wylogowanie nie jest destrukcyjną operacją, użytkownik może się zalogować ponownie)
Dla bezpieczeństwa upewnij się, że sesja jest całkowicie czyszczona po stronie serwera.

13. Jak powinna być obsługiwana aktualizacja postępu technologii - czy zmiany na suwaku powinny być zapisywane na bieżąco (auto-save), czy przez przycisk "Zapisz"?

Rekomendacja: Zapisywanie przez przycisk "Zapisz" w Popup 1:
- Suwak (Range Slider) aktualizuje lokalny stan w komponencie
- Wyświetlaj aktualną wartość obok suwaka w czasie rzeczywistym
- Przycisk "Zapisz" wysyła `PATCH /api/technologies/{id}`
- Po sukcesie: zamknij popup, odśwież węzeł na grafie, pokaż toast "Postęp zaktualizowany"
- Przycisk "Anuluj" zamyka popup bez zapisywania
To daje użytkownikowi kontrolę i unika niepotrzebnych requestów API przy każdym ruchu suwaka.

14. Jak powinny być obsługiwane długie nazwy technologii na węzłach grafu - czy truncate z ellipsis, word wrap, czy zmniejszenie fontu?

Rekomendacja: Użyj word wrap z maksymalną szerokością węzła:
- Maksymalna szerokość węzła: 200px
- Nazwa technologii: word wrap (max 2-3 linie)
- Font: 14px (normalny), 12px (jeśli nazwa > 30 znaków)
- Tooltip przy hover z pełną nazwą (jeśli truncated)
- Wysokość węzła dostosowuje się automatycznie do zawartości
Unikaj ellipsis - użytkownik powinien widzieć pełną nazwę technologii na grafie bez konieczności hover.

15. Jak powinna być zorganizowana obsługa błędów walidacji zwracanych przez API (400 ValidationError) - czy mapować na konkretne pola formularza, czy pokazać ogólny komunikat?

Rekomendacja: Mapuj błędy na konkretne pola:
- API zwraca `errors` array z `field` i `message`
- W Blazor: parsuj odpowiedź 400 i dodaj błędy do `EditContext.AddValidationMessage(fieldName, message)`
- Wyświetl błędy przez `<ValidationMessage For="@(() => model.Field)" />`
- Dodatkowo pokaż toast z ogólnym komunikatem "Popraw błędy w formularzu"
- Podświetl pola z błędami czerwonym borderem
To zapewnia najlepszy UX - użytkownik wie dokładnie, co poprawić.

</pytania>

---

**User**

1. Postępuj zgodnie z Twoimi rekomendacjami
2. Postępuj zgodnie z Twoimi rekomendacjami
3. Postępuj zgodnie z Twoimi rekomendacjami
4. Postępuj zgodnie z Twoimi rekomendacjami
5. Postępuj zgodnie z Twoimi rekomendacjami
6. Postępuj zgodnie z Twoimi rekomendacjami
7. Postępuj zgodnie z Twoimi rekomendacjami
8. Walidacja całego formularza: on submit (blokada przycisku jeśli błędy)
9. Postępuj zgodnie z Twoimi rekomendacjami, ale pomiń opcjonalny dro down z "Sortuj według" - nie chcę tego
10. Pokaż tylko tekst pomocniczy
11. Postępuj zgodnie z Twoimi rekomendacjami
12. Postępuj zgodnie z Twoimi rekomendacjami
13. Postępuj zgodnie z Twoimi rekomendacjami
14. Postępuj zgodnie z Twoimi rekomendacjami
15. Postępuj zgodnie z Twoimi rekomendacjami

To już koniec pytań.
---
Jesteś asystentem AI, którego zadaniem jest podsumowanie rozmowy na temat planowania architektury UI dla MVP i przygotowanie zwięzłego podsumowania dla następnego etapu rozwoju. W historii konwersacji znajdziesz następujące informacje:
1. Dokument wymagań produktu (PRD)
2. Informacje o stacku technologicznym
3. Plan API
4. Historia rozmów zawierająca pytania i odpowiedzi
5. Zalecenia dotyczące architektury UI

Twoim zadaniem jest:
1. Podsumować historię konwersacji, koncentrując się na wszystkich decyzjach związanych z planowaniem architektury UI.
2. Dopasować zalecenia modelu do odpowiedzi udzielonych w historii konwersacji. Zidentyfikować, które zalecenia są istotne w oparciu o dyskusję.
3. Przygotować szczegółowe podsumowanie rozmowy, które obejmuje:
   a. Główne wymagania dotyczące architektury UI
   b. Kluczowe widoki, ekrany i przepływy użytkownika
   c. Strategię integracji z API i zarządzania stanem
   d. Kwestie dotyczące responsywności, dostępności i bezpieczeństwa
   e. Wszelkie nierozwiązane kwestie lub obszary wymagające dalszego wyjaśnienia
4. Sformatować wyniki w następujący sposób:

<conversation_summary>
<decisions>
[Wymień decyzje podjęte przez użytkownika, ponumerowane].
</decisions>
<matched_recommendations>
[Lista najistotniejszych zaleceń dopasowanych do rozmowy, ponumerowanych]
</matched_recommendations>
<ui_architecture_planning_summary>
[Podaj szczegółowe podsumowanie rozmowy, w tym elementy wymienione w kroku 3].
</ui_architecture_planning_summary>
<unresolved_issues>
[Wymień wszelkie nierozwiązane kwestie lub obszary wymagające dalszych wyjaśnień, jeśli takie istnieją]
</unresolved_issues>
</conversation_summary>

Końcowy wynik powinien zawierać tylko treść w formacie markdown. Upewnij się, że Twoje podsumowanie jest jasne, zwięzłe i zapewnia cenne informacje dla następnego etapu planowania architektury UI i integracji z API.



---

**Cursor**

<conversation_summary>

# Podsumowanie planowania architektury UI dla DeveloperGoals MVP

## 1. Decisions (Decyzje użytkownika)

### Seria 1 - Fundamenty UI
1. **Graf technologii**: Komponent Blazor z JavaScript Interop dla vis.js
2. **Popupy**: Wykorzystanie komponentów modalnych Flowbite opakownych w komponenty Blazor (Popup 1 - Opcje, Popup 2 - Rekomendacje)
3. **Nawigacja**: Minimalistyczny navbar z logo, ikoną profilu i przyciskiem wylogowania
4. **Formularz profilu**: Pojedyncza strona z wszystkimi polami (Select multi-select dla technologii, Radio buttons dla roli i obszaru)
5. **Wizualizacja postępu**: Kombinacja - kolor węzła + pasek postępu + tekst procentowy (wszystko widoczne jednocześnie)
6. **Loader AI**: Spinner z tekstem "Generowanie rekomendacji..." i licznikiem sekund (do 20s)
7. **Lista ignorowanych**: Rozwijana sekcja (Accordion) poniżej rekomendacji z licznikiem
8. **Dark mode**: Przełącznik w navbarze z zapisem preferencji w localStorage
9. **Responsywność**: Brak wsparcia dla urządzeń mobilnych w MVP
10. **Obsługa błędów**: System warstwowy - inline alerts dla walidacji, toasty dla błędów operacyjnych, modale dla krytycznych błędów

### Seria 2 - Architektura komponentów
11. **Struktura komponentów**: Podział na mniejsze komponenty (GraphCanvas, TechnologyOptionsModal, AIRecommendationsModal)
12. **Zarządzanie stanem**: Dedykowany serwis `GraphStateService` (scoped)
13. **Synchronizacja z vis.js**: Jednokierunkowy przepływ danych Blazor → vis.js przez JS Interop z callbackami
14. **Wygląd węzła**: Prostokąt z zaokrąglonymi rogami, custom HTML shape z nazwą, paskiem postępu, procentem i kolorem tła
15. **Komunikacja z API**: Dedykowane serwisy API dla każdego zasobu (ProfileApiService, TechnologyApiService, AIRecommendationApiService, IgnoredTechnologyApiService)
16. **Walidacja formularzy**: DataAnnotations z `<EditForm>` i `<DataAnnotationsValidator>`
17. **Onboarding**: Automatyczne przekierowanie na stronę profilu z przyjaznym nagłówkiem
18. **Checkboxy w Popup 2**: Karty z możliwością zaznaczenia (Card + checkbox) zamiast standardowych checkboxów
19. **Routing**: Blazor Router z osobnymi stronami (`/login`, `/profile`, `/graph`, `/admin`)
20. **Obsługa błędów globalnych**: `<ErrorBoundary>` + GlobalExceptionHandler + ToastService

### Seria 3 - Szczegóły implementacji
21. **Integracja vis.js**: Dedykowany plik `wwwroot/js/graph-visualization.js`
22. **Stan ładowania**: Lokalny stan w komponentach + globalny LoadingService dla operacji full-page
23. **Konfiguracja vis.js**: Klasa `GraphVisualizationOptions.cs` z konfiguracją w appsettings.json
24. **Cache klienta**: Brak - poleganie wyłącznie na cache serwerowym (24h TTL)
25. **Struktura folderów**: Hierarchiczna (`Components/Pages/`, `Components/Graph/`, `Components/Profile/`, `Components/Shared/`)
26. **Aktualizacja grafu**: Inkrementalna aktualizacja (addNodes/addEdges) zamiast pełnego przeładowania
27. **Dark mode UI**: Przełącznik w NavMenu z ikoną słońca/księżyca, ThemeService (singleton)
28. **Konflikty batch**: Pokazanie wszystkich błędów w jednym toaście z listą
29. **Przycisk "Szukaj technologii"**: Obie opcje - na węźle grafu (hover) + w Popup 1
30. **Węzeł Start**: Wyróżniony styl (1.5x większy, gradient niebieski, ikona, glow, brak usuwania)
31. **Edycja opisu**: Prosty textarea z licznikiem znaków (0/2000), bez rich text
32. **Layout grafu**: Fullscreen - graf zajmuje `calc(100vh - 64px)`
33. **Walidacja custom tech**: Tylko on submit (blokada przycisku jeśli błędy) - **MODYFIKACJA UŻYTKOWNIKA**
34. **Sortowanie rekomendacji**: Zachowanie kolejności API (bez opcjonalnego dropdownu sortowania) - **MODYFIKACJA UŻYTKOWNIKA**
35. **Pusty graf**: Tylko tekst pomocniczy (bez tutorial overlay) - **MODYFIKACJA UŻYTKOWNIKA**
36. **Sesja**: Automatyczne wylogowanie po wygaśnięciu (7 dni) bez ostrzeżenia
37. **Przycisk wyloguj**: Bezpośrednie wylogowanie bez potwierdzenia
38. **Aktualizacja postępu**: Zapisywanie przez przycisk "Zapisz" (nie auto-save)
39. **Długie nazwy**: Word wrap z maksymalną szerokością 200px, tooltip przy hover
40. **Błędy walidacji API**: Mapowanie na konkretne pola formularza + ogólny toast

## 2. Matched Recommendations (Dopasowane zalecenia)

### Architektura komponentów
1. **Struktura komponentów Blazor**:
   ```
   Components/
   ├── Pages/           # Strony z @page
   │   ├── Login.razor
   │   ├── Profile.razor
   │   ├── Graph.razor
   │   └── Admin.razor
   ├── Graph/           # Komponenty grafu
   │   ├── GraphCanvas.razor
   │   ├── TechnologyOptionsModal.razor
   │   ├── AIRecommendationsModal.razor
   │   └── TechnologyCard.razor
   ├── Profile/
   │   ├── ProfileForm.razor
   │   └── TechnologySelector.razor
   ├── Shared/
   │   ├── Toast.razor
   │   ├── LoadingSpinner.razor
   │   └── ErrorBoundary.razor
   └── Layout/
       ├── MainLayout.razor
       └── NavMenu.razor
   ```

2. **Serwisy zarządzania stanem i API**:
   - `GraphStateService` (scoped) - stan grafu, operacje CRUD, eventy zmian
   - `ProfileApiService` (scoped) - operacje na profilu
   - `TechnologyApiService` (scoped) - CRUD technologii
   - `AIRecommendationApiService` (scoped) - generowanie rekomendacji
   - `IgnoredTechnologyApiService` (scoped) - zarządzanie ignorowanymi
   - `ThemeService` (singleton) - dark/light mode
   - `LoadingService` (singleton) - globalny loading indicator
   - `ToastService` (singleton) - wyświetlanie toastów

3. **JavaScript Interop dla vis.js** (`wwwroot/js/graph-visualization.js`):
   ```javascript
   window.GraphVisualization = {
     initialize(containerId, dotNetHelper),
     updateGraph(nodes, edges),
     addNodes(newNodes),
     addEdges(newEdges),
     setOptions(options),
     destroy()
   }
   ```
   - Callbacki do C#: `onNodeClick(nodeId)`, `onNodeDoubleClick(nodeId)`
   - Jednokierunkowy przepływ: Blazor → vis.js

### Wizualizacja i UX

4. **Węzeł technologii na grafie** (vis.js custom HTML shape):
   - Prostokąt z zaokrąglonymi rogami (200px max szerokość)
   - Nazwa z prefiksem (word wrap, max 2-3 linie)
   - Kolor tła zależny od postępu (0% czerwony → 50% żółty → 100% zielony)
   - Pasek postępu poziomy na dole (gradient kolorystyczny)
   - Procent postępu w prawym dolnym rogu
   - Przycisk "Szukaj technologii" widoczny przy hover
   - Tooltip z pełną nazwą przy hover (jeśli truncated)

5. **Węzeł "Start"** (specjalny styl):
   - 1.5x większy od standardowych węzłów
   - Gradient niebieski (niezależny od systemu postępu)
   - Ikona flagi/gwiazdy w centrum
   - Border z efektem glow
   - Postęp 100%, bez paska postępu
   - Chroniony przed usunięciem

6. **Popup 1 - Opcje technologii** (TechnologyOptionsModal):
   - Textarea dla prywatnego opisu (max 2000 znaków, licznik)
   - Range Slider dla postępu (0-100%)
   - Przyciski: "Zapisz", "Anuluj", "Usuń z grafu"
   - Potwierdzenie przed usunięciem
   - Brak przycisku "Usuń" dla węzła Start

7. **Popup 2 - Rekomendacje AI** (AIRecommendationsModal):
   - Karty technologii z checkboxami (Card Flowbite)
   - Każda karta: nazwa, prefix, tag (badge), opis, AI reasoning
   - Zaznaczona karta: border w kolorze primary
   - Przyciski na dole: "Dodaj do grafu", "Ignoruj"
   - Accordion "Ignorowane (X)" poniżej rekomendacji
   - Przycisk "Aktywuj" dla ignorowanych technologii
   - Spinner podczas generowania (do 20s)
   - Kolejność: zgodna z API (priorytet AI)

8. **NavMenu** (minimalistyczny):
   - Logo aplikacji po lewej
   - Przełącznik dark/light mode (ikona słońca/księżyca)
   - Ikona profilu (link do `/profile`)
   - Przycisk "Wyloguj"
   - Fixed position, wysokość ~64px

9. **Dark mode**:
   - ThemeService z metodami: `ToggleTheme()`, `GetCurrentTheme()`, event `OnThemeChanged`
   - Zapis w localStorage przez JS Interop
   - Aplikacja klasy `dark` na `<html>` (Tailwind convention)
   - Flowbite wspiera dark mode out-of-the-box

### Formularze i walidacja

10. **Formularz profilu** (onboarding):
    - Select multi-select dla głównych technologii (min 5, max 50)
    - Radio buttons dla roli: Programista / Tester / Analityk / Data Science Specialist
    - Radio buttons dla obszaru: Backend / Frontend / Full Stack / Testing / Data Science / DevOps
    - Walidacja: DataAnnotations + `<EditForm>` + `<ValidationMessage>`
    - Walidacja tylko on submit, blokada przycisku jeśli błędy
    - Automatyczne przekierowanie po wypełnieniu: `/profile` → `/graph`

11. **Formularz custom technologii**:
    - Pola: nazwa, prefix, tag, opis systemowy
    - Walidacja tylko on submit
    - Blokada przycisku "Dodaj" jeśli błędy
    - Mapowanie błędów API 400 na konkretne pola

12. **Obsługa błędów walidacji z API**:
    - Parsowanie `errors` array z API (field + message)
    - Dodawanie do `EditContext.AddValidationMessage()`
    - Wyświetlanie przez `<ValidationMessage For="..." />`
    - Toast z ogólnym komunikatem "Popraw błędy w formularzu"
    - Czerwony border na polach z błędami

### Routing i nawigacja

13. **Blazor Router** (struktura stron):
    - `/` - redirect do `/login` lub `/graph` (zależnie od auth)
    - `/login` - strona logowania (Google OAuth button)
    - `/profile` - formularz profilu (onboarding + edycja)
    - `/graph` - główny widok grafu technologii
    - `/admin` - panel administratora (opcjonalnie)
    - `<AuthorizeView>` dla chronionych stron
    - `RedirectToLogin.razor` dla nieautoryzowanych

14. **Przepływ onboardingu**:
    1. Użytkownik klika "Zaloguj przez Google"
    2. OAuth callback → API tworzy/pobiera użytkownika
    3. Jeśli `hasProfile = false` → redirect `/profile`
    4. Wypełnienie profilu → POST /api/profile → auto-tworzenie węzła "Start"
    5. Redirect `/graph` → wyświetlenie grafu z węzłem Start
    6. Tekst pomocniczy: "Kliknij Start i wybierz 'Szukaj nowych technologii'"

15. **Przepływ dodawania technologii**:
    1. Kliknięcie węzła → przyciski "Opcje" i "Szukaj nowych technologii"
    2. "Szukaj nowych technologii" → Popup 2 z spinnerem
    3. POST /api/ai/recommendations (timeout 20s)
    4. Wyświetlenie ~10 rekomendacji jako karty z checkboxami
    5. Zaznaczenie technologii → "Dodaj do grafu"
    6. POST /api/technologies/batch → dodanie do grafu
    7. Inkrementalna aktualizacja grafu (addNodes/addEdges)
    8. Toast "Dodano X technologii"

### Wydajność i optymalizacja

16. **Aktualizacja grafu**:
    - Inkrementalna: `GraphVisualization.addNodes()` + `addEdges()`
    - Unikanie pełnego `updateGraph()` po każdej operacji
    - Lokalna aktualizacja stanu w `GraphStateService`
    - Płynne animacje w vis.js

17. **Cache**:
    - Brak cache po stronie klienta
    - Poleganie na cache serwerowym (24h TTL dla rekomendacji AI)
    - Blazor Server utrzymuje stan sesji między renderami

18. **Konfiguracja vis.js**:
    - Klasa `GraphVisualizationOptions.cs` w C#
    - Sekcja `appsettings.json` → `GraphVisualization`
    - Przekazanie do JS przez JS Interop przy inicjalizacji
    - Parametry: kolory węzłów, rozmiary fontów, opcje layoutu, kolory krawędzi

### Obsługa błędów i stanów

19. **System obsługi błędów** (warstwowy):
    - `<ErrorBoundary>` w App.razor dla błędów renderowania
    - GlobalExceptionHandler (middleware) dla błędów API
    - Try-catch w serwisach API z mapowaniem na typowane wyjątki
    - ToastService dla wyświetlania błędów użytkownikowi
    - Komponenty UI nie zawierają try-catch

20. **Toasty** (Toast Flowbite):
    - Błędy walidacji formularzy: inline alerts przy polach
    - Błędy operacyjne (timeout AI, 500, 502): toasty w prawym górnym rogu
    - Auto-hide po 5 sekundach
    - Kolory: czerwony (error), żółty (warning), zielony (success)
    - Krytyczne błędy: modalne dialogi

21. **Stany ładowania**:
    - Lokalny stan w komponentach: `isLoadingRecommendations` w Popup 2
    - Globalny LoadingService dla full-page operations
    - Overlay z spinnerem dla pierwszego załadowania grafu
    - Spinner + tekst + licznik sekund dla AI (do 20s)

22. **Obsługa batch operations** (207 Multi-Status):
    - Toast z tytułem "Dodano X z Y technologii"
    - Lista błędów dla nieudanych operacji
    - Przycisk "Pokaż szczegóły" rozwijający listę
    - Pełny sukces (201): prosty toast "Dodano X technologii"

### Sesje i bezpieczeństwo

23. **Sesje**:
    - 7 dni sliding expiration (server-side)
    - Automatyczne wylogowanie po wygaśnięciu
    - Przekierowanie do `/login` + toast "Sesja wygasła"
    - Brak countdown timera
    - Blazor Server auto-reconnection

24. **Wylogowanie**:
    - Bezpośrednie (bez potwierdzenia)
    - POST /api/auth/logout → redirect `/login`
    - Toast "Wylogowano pomyślnie"
    - Czyszczenie sesji po stronie serwera

## 3. UI Architecture Planning Summary

### Przegląd architektury

Aplikacja DeveloperGoals MVP została zaprojektowana jako **Blazor Server** z wykorzystaniem **Flowbite** (komponenty UI), **Tailwind CSS** (styling) i **vis.js** (wizualizacja grafu). Architektura opiera się na wzorcu **Service Layer** z dedykowanymi serwisami dla zarządzania stanem i komunikacji z API.

### Główne widoki i ekrany

#### 1. Strona logowania (`/login`)
- Minimalistyczny design z logo aplikacji
- Przycisk "Zaloguj się przez Google" (Google OAuth 2.0)
- Przekierowanie do Google → callback → utworzenie/pobranie użytkownika
- Routing po logowaniu: użytkownik bez profilu → `/profile`, z profilem → `/graph`

#### 2. Strona profilu (`/profile`)
- **Onboarding** (pierwsze wypełnienie):
  - Nagłówek: "Witaj! Uzupełnij swój profil, aby rozpocząć"
  - Krótki opis celu aplikacji (2-3 zdania)
- **Formularz**:
  - Select multi-select: główne technologie (min 5, max 50)
  - Radio buttons: rola (Programista / Tester / Analityk / Data Science Specialist)
  - Radio buttons: obszar rozwoju (Backend / Frontend / Full Stack / Testing / Data Science / DevOps)
- **Walidacja**: DataAnnotations, tylko on submit, blokada przycisku
- **Edycja**: dostępna w każdym momencie przez NavMenu
- **Akcja po zapisie**: POST /api/profile → auto-tworzenie węzła "Start" → redirect `/graph`

#### 3. Główny widok grafu (`/graph`)
- **Layout**:
  - NavMenu na górze (fixed, 64px)
  - Graf zajmuje `calc(100vh - 64px)` - fullscreen
  - Brak marginesów bocznych
- **Graf technologii** (vis.js):
  - Węzły: prostokąty z zaokrąglonymi rogami, custom HTML
  - Węzeł "Start": wyróżniony (1.5x większy, gradient niebieski, ikona, glow)
  - Węzły technologii: nazwa, kolor tła (postęp), pasek postępu, procent
  - Krawędzie: strzałki pokazujące zależności
  - Interakcja: kliknięcie węzła → przyciski "Opcje" i "Szukaj nowych technologii"
  - Hover na węźle: przycisk "Szukaj nowych technologii"
- **Pusty graf** (tylko Start):
  - Tekst pomocniczy: "Kliknij Start i wybierz 'Szukaj nowych technologii' aby otrzymać pierwsze rekomendacje"
- **Dark mode**: przełącznik w NavMenu, zapisywany w localStorage

#### 4. Popup 1 - Opcje technologii (Modal)
- **Trigger**: Kliknięcie węzła → przycisk "Opcje"
- **Zawartość**:
  - Wyświetlanie: nazwa, prefix, tag, opis systemowy
  - Textarea: prywatny opis (max 2000 znaków, licznik)
  - Range Slider: postęp (0-100%), wyświetlanie aktualnej wartości
  - Przyciski: "Zapisz", "Anuluj", "Usuń z grafu"
- **Akcje**:
  - "Zapisz": PATCH /api/technologies/{id} → zamknięcie popup → odświeżenie węzła → toast
  - "Usuń": modal potwierdzenia → DELETE /api/technologies/{id} → usunięcie z grafu
  - Węzeł "Start": brak przycisku "Usuń" (chroniony)

#### 5. Popup 2 - Rekomendacje AI (Modal)
- **Trigger**: Kliknięcie węzła → przycisk "Szukaj nowych technologii"
- **Proces**:
  1. Wyświetlenie spinnera + "Generowanie rekomendacji..." + licznik sekund
  2. POST /api/ai/recommendations (timeout 20s)
  3. Wyświetlenie ~10 rekomendacji
- **Zawartość**:
  - **Sekcja rekomendacji**:
    - Karty technologii z checkboxami (Card Flowbite)
    - Każda karta: nazwa, prefix, tag (badge), opis, AI reasoning
    - Zaznaczona karta: border primary
    - Kolejność: zgodna z API (priorytet AI)
  - **Sekcja ignorowanych** (Accordion, domyślnie zwinięty):
    - Nagłówek: "Ignorowane (X)"
    - Lista ignorowanych technologii z checkboxami
    - Przycisk "Aktywuj" na dole
- **Przyciski**:
  - "Dodaj do grafu": POST /api/technologies/batch → inkrementalna aktualizacja grafu
  - "Ignoruj": POST /api/ignored-technologies → przeniesienie do sekcji ignorowanych
  - "Aktywuj" (w sekcji ignorowanych): DELETE /api/ignored-technologies/batch → przywrócenie do rekomendacji

#### 6. Panel administratora (`/admin`)
- Opcjonalny, niski priorytet
- Dostęp: hardcoded GoogleId w appsettings.json
- Metryki: liczba użytkowników, KPI, popularne kategorie, statystyki AI

### Przepływy użytkownika

#### Przepływ 1: Nowy użytkownik (onboarding)
```
1. Wejście na stronę → redirect /login
2. Kliknięcie "Zaloguj przez Google"
3. OAuth flow → callback → utworzenie użytkownika
4. Redirect /profile (hasProfile = false)
5. Wypełnienie formularza profilu (3 minuty max)
6. Submit → POST /api/profile → utworzenie węzła "Start"
7. Redirect /graph → wyświetlenie grafu z węzłem Start
8. Tekst pomocniczy widoczny
```

#### Przepływ 2: Generowanie pierwszych rekomendacji
```
1. Kliknięcie węzła "Start" na grafie
2. Kliknięcie "Szukaj nowych technologii"
3. Popup 2 → spinner (do 20s)
4. POST /api/ai/recommendations → ~10 technologii
5. Przeglądanie kart z rekomendacjami
6. Zaznaczenie interesujących (checkboxy)
7. Kliknięcie "Dodaj do grafu"
8. POST /api/technologies/batch → dodanie do grafu
9. Inkrementalna aktualizacja grafu (nowe węzły + krawędzie)
10. Toast "Dodano X technologii"
```

#### Przepływ 3: Ignorowanie technologii
```
1. W Popup 2 zaznaczenie nieinteresujących technologii
2. Kliknięcie "Ignoruj"
3. POST /api/ignored-technologies → zapis w bazie
4. Przeniesienie do sekcji "Ignorowane (X)" w tym samym popup
5. Technologie znikają z głównej listy rekomendacji
```

#### Przepływ 4: Przywracanie ignorowanych
```
1. W Popup 2 rozwinięcie Accordion "Ignorowane (X)"
2. Zaznaczenie technologii do przywrócenia
3. Kliknięcie "Aktywuj"
4. DELETE /api/ignored-technologies/batch
5. Przeniesienie z powrotem do głównej listy rekomendacji
6. Możliwość dodania do grafu
```

#### Przepływ 5: Aktualizacja postępu nauki
```
1. Kliknięcie węzła technologii na grafie
2. Kliknięcie "Opcje" → Popup 1
3. Przesunięcie suwaka postępu (0-100%)
4. Edycja prywatnego opisu (opcjonalnie)
5. Kliknięcie "Zapisz"
6. PATCH /api/technologies/{id}
7. Zamknięcie popup → odświeżenie węzła (nowy kolor, pasek, procent)
8. Toast "Postęp zaktualizowany"
```

#### Przepływ 6: Generowanie kolejnych rekomendacji
```
1. Kliknięcie węzła technologii (nie Start) na grafie
2. Kliknięcie "Szukaj nowych technologii"
3. POST /api/ai/recommendations z kontekstem:
   - fromTechnologyId: wybrany węzeł
   - contextTechnologyIds: wszystkie technologie w grafie
   - Pełny profil użytkownika
4. AI generuje ~10 technologii powiązanych z wybraną
5. Wyświetlenie w Popup 2 (w tym ignorowane z poprzednich sesji)
6. Dodawanie do grafu jak w przepływie 2
```

### Strategia integracji z API

#### Warstwa serwisów API

**ProfileApiService** (scoped):
- `GetProfileAsync()` → GET /api/profile
- `CreateProfileAsync(dto)` → POST /api/profile
- `UpdateProfileAsync(dto)` → PUT /api/profile
- Obsługa błędów: 404 (brak profilu), 400 (walidacja), 409 (konflikt)

**TechnologyApiService** (scoped):
- `GetTechnologiesAsync()` → GET /api/technologies
- `GetTechnologyAsync(id)` → GET /api/technologies/{id}
- `GetGraphAsync()` → GET /api/technologies/graph
- `AddTechnologyAsync(dto)` → POST /api/technologies
- `AddTechnologiesBatchAsync(dto)` → POST /api/technologies/batch
- `AddCustomTechnologyAsync(dto)` → POST /api/technologies/custom
- `UpdateTechnologyAsync(id, dto)` → PATCH /api/technologies/{id}
- `DeleteTechnologyAsync(id)` → DELETE /api/technologies/{id}
- Obsługa błędów: 403 (Start node), 404, 400 (walidacja), 409 (konflikt)

**AIRecommendationApiService** (scoped):
- `GenerateRecommendationsAsync(fromTechId, contextIds)` → POST /api/ai/recommendations
- Timeout: 20 sekund
- Obsługa błędów: 408 (timeout), 400 (brak profilu), 500 (AI error), 502 (invalid response)
- Zwraca: lista rekomendacji + metadata (fromCache, count, generatedAt)

**IgnoredTechnologyApiService** (scoped):
- `GetIgnoredTechnologiesAsync()` → GET /api/ignored-technologies
- `AddIgnoredTechnologiesAsync(dtos)` → POST /api/ignored-technologies
- `RestoreIgnoredTechnologyAsync(id)` → DELETE /api/ignored-technologies/{id}
- `RestoreIgnoredTechnologiesBatchAsync(ids)` → DELETE /api/ignored-technologies/batch
- Obsługa błędów: 404, 409 (duplicate)

#### Zarządzanie stanem

**GraphStateService** (scoped):
- **Stan**:
  - `List<TechnologyNode> Nodes`
  - `List<TechnologyEdge> Edges`
  - `TechnologyNode? SelectedNode`
- **Metody**:
  - `LoadGraphAsync()` - pobiera graf z API
  - `AddTechnologyAsync(dto)` - dodaje technologię
  - `AddTechnologiesBatchAsync(dtos)` - dodaje wiele technologii
  - `UpdateTechnologyAsync(id, dto)` - aktualizuje technologię
  - `DeleteTechnologyAsync(id)` - usuwa technologię
  - `SelectNode(id)` - ustawia wybrany węzeł
- **Eventy**:
  - `OnGraphChanged` - emitowany po każdej zmianie grafu
  - `OnNodeSelected` - emitowany po wyborze węzła
- **Integracja**: komponenty subskrybują eventy i reagują na zmiany

**ThemeService** (singleton):
- `ToggleTheme()` - przełącza dark/light
- `GetCurrentTheme()` - zwraca aktualny motyw
- `OnThemeChanged` - event emitowany po zmianie
- Zapis w localStorage przez JS Interop
- Aplikacja klasy `dark` na `<html>`

**LoadingService** (singleton):
- `IsLoading` (bool) - globalny stan ładowania
- `ShowLoading(message)` - pokazuje overlay z spinnerem
- `HideLoading()` - ukrywa overlay
- Komponenty subskrybują `IsLoading` dla wyświetlenia loadera

**ToastService** (singleton):
- `ShowSuccess(message)` - zielony toast
- `ShowError(message)` - czerwony toast
- `ShowWarning(message)` - żółty toast
- `ShowInfo(message)` - niebieski toast
- Auto-hide po 5 sekundach
- Pozycja: prawy górny róg

#### Przepływ danych

```
Komponent UI
    ↓ (wywołanie metody)
GraphStateService / Serwis API
    ↓ (HTTP request)
Backend API
    ↓ (response)
Serwis API (parsowanie, obsługa błędów)
    ↓ (aktualizacja stanu)
GraphStateService (emit event)
    ↓ (subskrypcja)
Komponent UI (re-render)
    ↓ (JS Interop)
vis.js (aktualizacja grafu)
```

### Integracja vis.js przez JavaScript Interop

#### Plik: `wwwroot/js/graph-visualization.js`

```javascript
window.GraphVisualization = {
    network: null,
    dotNetHelper: null,
    
    initialize: function(containerId, dotNetHelper, options) {
        this.dotNetHelper = dotNetHelper;
        const container = document.getElementById(containerId);
        
        // Konfiguracja vis.js
        const visOptions = {
            nodes: {
                shape: 'box',
                borderWidth: 2,
                borderWidthSelected: 3,
                font: { size: 14 },
                margin: 10
            },
            edges: {
                arrows: 'to',
                smooth: { type: 'cubicBezier' }
            },
            physics: {
                enabled: true,
                hierarchicalRepulsion: {
                    nodeDistance: 150
                }
            },
            interaction: {
                hover: true,
                tooltipDelay: 200
            }
        };
        
        // Merge z opcjami z C#
        const finalOptions = { ...visOptions, ...options };
        
        this.network = new vis.Network(container, { nodes: [], edges: [] }, finalOptions);
        
        // Event handlers
        this.network.on('click', (params) => {
            if (params.nodes.length > 0) {
                this.dotNetHelper.invokeMethodAsync('OnNodeClick', params.nodes[0]);
            }
        });
        
        this.network.on('doubleClick', (params) => {
            if (params.nodes.length > 0) {
                this.dotNetHelper.invokeMethodAsync('OnNodeDoubleClick', params.nodes[0]);
            }
        });
    },
    
    updateGraph: function(nodes, edges) {
        if (this.network) {
            this.network.setData({ nodes: nodes, edges: edges });
        }
    },
    
    addNodes: function(newNodes) {
        if (this.network) {
            const nodes = this.network.body.data.nodes;
            nodes.add(newNodes);
        }
    },
    
    addEdges: function(newEdges) {
        if (this.network) {
            const edges = this.network.body.data.edges;
            edges.add(newEdges);
        }
    },
    
    setOptions: function(options) {
        if (this.network) {
            this.network.setOptions(options);
        }
    },
    
    destroy: function() {
        if (this.network) {
            this.network.destroy();
            this.network = null;
        }
    }
};
```

#### Komponent: `GraphCanvas.razor`

```csharp
@inject IJSRuntime JS
@implements IAsyncDisposable

<div id="graph-container" style="width: 100%; height: 100%;"></div>

@code {
    [Parameter] public EventCallback<int> OnNodeClick { get; set; }
    [Parameter] public EventCallback<int> OnNodeDoubleClick { get; set; }
    
    private DotNetObjectReference<GraphCanvas>? dotNetHelper;
    private IJSObjectReference? jsModule;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./js/graph-visualization.js");
            dotNetHelper = DotNetObjectReference.Create(this);
            
            var options = new {
                // Opcje z GraphVisualizationOptions
            };
            
            await jsModule.InvokeVoidAsync("GraphVisualization.initialize", "graph-container", dotNetHelper, options);
        }
    }
    
    public async Task UpdateGraphAsync(List<NodeDto> nodes, List<EdgeDto> edges)
    {
        if (jsModule != null)
        {
            await jsModule.InvokeVoidAsync("GraphVisualization.updateGraph", nodes, edges);
        }
    }
    
    public async Task AddNodesAsync(List<NodeDto> nodes)
    {
        if (jsModule != null)
        {
            await jsModule.InvokeVoidAsync("GraphVisualization.addNodes", nodes);
        }
    }
    
    [JSInvokable]
    public async Task OnNodeClickHandler(int nodeId)
    {
        await OnNodeClick.InvokeAsync(nodeId);
    }
    
    [JSInvokable]
    public async Task OnNodeDoubleClickHandler(int nodeId)
    {
        await OnNodeDoubleClick.InvokeAsync(nodeId);
    }
    
    public async ValueTask DisposeAsync()
    {
        if (jsModule != null)
        {
            await jsModule.InvokeVoidAsync("GraphVisualization.destroy");
            await jsModule.DisposeAsync();
        }
        dotNetHelper?.Dispose();
    }
}
```

### Responsywność i dostępność

#### Responsywność
- **Decyzja**: Brak wsparcia dla urządzeń mobilnych w MVP
- **Desktop only**: Minimalna szerokość ekranu 1024px
- **Layout**: Fullscreen graf bez marginesów
- **Flowbite**: Komponenty responsywne out-of-the-box (dla przyszłych iteracji)

#### Dostępność
- **Keyboard navigation**: Obsługa Tab, Enter, Escape w modalach
- **ARIA labels**: Dla przycisków, formularzy, modali
- **Kontrast kolorów**: Zgodność z WCAG 2.1 AA (Tailwind + Flowbite)
- **Focus indicators**: Widoczne dla użytkowników klawiatury
- **Screen readers**: Semantyczny HTML, role ARIA

### Bezpieczeństwo na poziomie UI

#### Autentykacja i autoryzacja
- **Google OAuth 2.0**: Jedyna metoda logowania
- **Session management**: Server-side (Blazor Server)
- **AuthorizeView**: Ochrona komponentów i stron
- **RedirectToLogin**: Automatyczne przekierowanie dla nieautoryzowanych
- **Admin access**: Hardcoded GoogleId w appsettings.json

#### Izolacja danych
- **UserId**: Zawsze z authenticated context (nigdy z klienta)
- **API calls**: Wszystkie zawierają session cookie
- **Brak cross-user access**: Każdy użytkownik widzi tylko swoje dane
- **Walidacja**: Po stronie serwera (API) + klienta (UI)

#### Ochrona przed atakami
- **XSS**: Blazor automatycznie escapuje output
- **CSRF**: Built-in protection w Blazor Server
- **SQL Injection**: Entity Framework parametryzowane zapytania
- **HTTPS**: Wymagane w produkcji (Google OAuth requirement)

### Konfiguracja i ustawienia

#### appsettings.json
```json
{
  "GraphVisualization": {
    "NodeColors": {
      "Progress0": "#EF4444",
      "Progress50": "#F59E0B",
      "Progress100": "#10B981"
    },
    "FontSizes": {
      "NodeTitle": 14,
      "NodeSubtitle": 12
    },
    "Layout": {
      "Type": "hierarchical",
      "NodeDistance": 150
    },
    "EdgeColors": {
      "Default": "#6B7280",
      "Hover": "#3B82F6"
    }
  },
  "AdminSettings": {
    "GoogleIds": ["1234567890", "0987654321"]
  }
}
```

#### GraphVisualizationOptions.cs
```csharp
public class GraphVisualizationOptions
{
    public NodeColorsOptions NodeColors { get; set; }
    public FontSizesOptions FontSizes { get; set; }
    public LayoutOptions Layout { get; set; }
    public EdgeColorsOptions EdgeColors { get; set; }
}
```

### Testowanie UI

#### Priorytety testowania
1. **Krytyczne**:
   - Przepływ onboardingu (login → profil → graf)
   - Generowanie rekomendacji AI (timeout, błędy)
   - Dodawanie technologii do grafu (single + batch)
   - Aktualizacja postępu (suwak, zapis)
   - Integracja vis.js (renderowanie, interakcje)

2. **Wysokie**:
   - Ignorowanie i przywracanie technologii
   - Edycja profilu (bez regeneracji grafu)
   - Usuwanie technologii (ochrona Start node)
   - Dark mode (przełączanie, localStorage)
   - Walidacja formularzy (inline, API errors)

3. **Średnie**:
   - Wylogowanie i sesje
   - Toasty i komunikaty błędów
   - Custom technologie
   - Panel administratora

#### Typy testów
- **Unit tests**: Serwisy API, GraphStateService, walidatory
- **Integration tests**: Przepływy użytkownika, komunikacja z API
- **E2E tests**: Kluczowe scenariusze (Playwright/Selenium)
- **Manual testing**: Wizualizacja grafu, UX, responsywność

## 4. Unresolved Issues (Nierozwiązane kwestie)

### Techniczne

1. **Orphaned technologies po usunięciu węzła**:
   - Czy technologie bez incoming edges powinny być automatycznie usuwane?
   - **Decyzja MVP**: NIE - użytkownik usuwa ręcznie
   - **Do rozważenia**: Pokazać ostrzeżenie o orphaned nodes przy usuwaniu

2. **Limity wydajnościowe vis.js**:
   - Maksymalna liczba węzłów na grafie przed spadkiem wydajności?
   - **Rekomendacja**: Testowanie z 100+ węzłami
   - **Opcja**: Lazy loading lub paginacja dla bardzo dużych grafów (post-MVP)

3. **Strategia aktualizacji grafu przy długich sesjach**:
   - Czy implementować periodic refresh z API?
   - **Decyzja MVP**: Brak auto-refresh, użytkownik odświeża ręcznie (F5)
   - **Post-MVP**: SignalR dla real-time updates

4. **Obsługa konfliktów wersji (optimistic concurrency)**:
   - Czy implementować versioning dla technologii?
   - **Decyzja MVP**: Brak - last write wins
   - **Post-MVP**: ETag lub timestamp-based concurrency

### UX/UI

5. **Animacje przejść między widokami**:
   - Czy dodać page transitions?
   - **Decyzja MVP**: Brak animacji (focus na funkcjonalność)
   - **Post-MVP**: Smooth transitions z Tailwind animations

6. **Onboarding dla zaawansowanych funkcji**:
   - Czy dodać tooltips/hints dla mniej oczywistych funkcji?
   - **Decyzja MVP**: Tylko tekst pomocniczy dla pustego grafu
   - **Post-MVP**: Interactive tour (np. Shepherd.js)

7. **Eksport/import grafu**:
   - Czy użytkownik powinien móc eksportować graf jako obraz/PDF?
   - **Poza zakresem MVP** (PRD line 273)
   - **Post-MVP**: Export do PNG/SVG, udostępnianie linku

### Biznesowe

8. **Metryki użytkowania UI**:
   - Jakie eventy trackować (analytics)?
   - **Rekomendacja**: Google Analytics lub Application Insights
   - **Eventy**: kliknięcia węzłów, generowanie AI, dodawanie technologii, czas sesji

9. **Feedback loop dla rekomendacji AI**:
   - Czy użytkownik powinien móc oceniać jakość rekomendacji?
   - **Poza zakresem MVP** (PRD line 279)
   - **Post-MVP**: Thumbs up/down, komentarze

10. **Limity użytkowania**:
    - Czy wprowadzić limity liczby technologii na użytkownika?
    - **Decyzja MVP**: Brak limitów (PRD line 791)
    - **Monitoring**: Śledzenie średniej liczby technologii per user

### Wydajność

11. **Strategia cache dla dużych grafów**:
    - Czy cachować graf po stronie klienta (IndexedDB)?
    - **Decyzja MVP**: Brak - poleganie na Blazor Server state
    - **Post-MVP**: Rozważyć dla bardzo dużych grafów (>200 węzłów)

12. **Optymalizacja renderowania vis.js**:
    - Czy używać canvas czy SVG rendering?
    - **Rekomendacja**: Canvas (domyślne w vis.js, lepsze dla dużych grafów)
    - **Testowanie**: Porównanie wydajności canvas vs SVG

### Integracja

13. **Fallback dla JavaScript disabled**:
    - Czy obsługiwać użytkowników bez JS?
    - **Decyzja MVP**: NIE - Blazor Server wymaga JS
    - **Komunikat**: "JavaScript is required" jeśli wyłączony

14. **Offline mode**:
    - Czy aplikacja powinna działać offline?
    - **Poza zakresem MVP** (wymaga Blazor WebAssembly + PWA)
    - **Post-MVP**: Rozważyć migrację na Blazor WASM lub hybrid

15. **Browser compatibility**:
    - Minimalne wersje przeglądarek?
    - **Rekomendacja**: Chrome 90+, Firefox 88+, Edge 90+, Safari 14+
    - **Testowanie**: Cross-browser testing dla kluczowych funkcji

</conversation_summary>

