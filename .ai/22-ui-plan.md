# Architektura UI dla DeveloperGoals

## 1. Przegląd struktury UI

Architektura interfejsu użytkownika aplikacji DeveloperGoals została zaprojektowana w oparciu o technologię **Blazor Server** (.NET 8), wykorzystując bibliotekę **Flowbite** (opartą na Tailwind CSS) do stylowania komponentów oraz **vis.js** do zaawansowanej wizualizacji grafu technologii.

System opiera się na wzorcu warstwowym (Service Layer Pattern), oddzielając logikę prezentacji od logiki biznesowej i komunikacji z API. Interfejs jest w pełni responsywny (z naciskiem na Desktop w fazie MVP) i obsługuje tryb ciemny/jasny (Dark/Light Mode).

### Kluczowe filary architektury:
*   **Technologia:** ASP.NET Core Blazor Server.
*   **System Design:** Flowbite + Tailwind CSS.
*   **Wizualizacja:** Biblioteka `vis.js` integrowana przez JavaScript Interop.
*   **Zarządzanie stanem:** Serwisy Scoped (`GraphStateService`) synchronizujące stan między komponentami a API.
*   **Bezpieczeństwo:** `AuthorizeView`, ochrona tras i danych użytkownika na poziomie sesji.

## 2. Lista widoków

### 2.1. Strona Logowania (`/login`)
*   **Ścieżka:** `/login`
*   **Główny cel:** Umożliwienie użytkownikowi bezpiecznego dostępu do aplikacji poprzez OAuth.
*   **Kluczowe informacje:** Logo aplikacji, krótki opis wartości, przycisk logowania.
*   **Kluczowe komponenty:** `GoogleLoginButton`.
*   **UX/Bezpieczeństwo:** Minimalistyczny design, brak rozpraszaczy. Automatyczne przekierowanie do Google OAuth.

### 2.2. Profil Użytkownika / Onboarding (`/profile`)
*   **Ścieżka:** `/profile`
*   **Główny cel:** Zebranie kluczowych danych o użytkowniku (rola, stack technologiczny) w celu personalizacji AI. Pełni rolę onboardingu dla nowych użytkowników oraz edycji dla powracających.
*   **Kluczowe informacje:** Formularz z polami: Główne technologie (Multi-select), Rola (Radio), Obszar rozwoju (Radio).
*   **Kluczowe komponenty:** `ProfileForm`, `TechnologySelector` (chip-based selection).
*   **UX/Bezpieczeństwo:** Walidacja formularza w czasie rzeczywistym i "on submit". Blokada przejścia dalej bez wypełnienia wymaganych pól (min. 5 technologii).

### 2.3. Główny Graf Technologii (`/graph`)
*   **Ścieżka:** `/graph` (domyślna po zalogowaniu i onboardingu)
*   **Główny cel:** Centralny obszar roboczy do wizualizacji, planowania i zarządzania ścieżką rozwoju.
*   **Kluczowe informacje:** Interaktywny graf węzłów (technologii) i krawędzi (zależności). Węzeł "Start" jako punkt początkowy.
*   **Kluczowe komponenty:** 
    *   `GraphCanvas` (wrapper na vis.js).
    *   `NavMenu` (pasek nawigacji).
    *   `TechnologyOptionsModal` (Popup 1 - edycja/szczegóły).
    *   `AIRecommendationsModal` (Popup 2 - generowanie ścieżki).
*   **UX/Bezpieczeństwo:** 
    *   Tryb pełnoekranowy (Fullscreen) dla maksymalnej czytelności.
    *   Wizualne kodowanie postępu (kolory węzłów).
    *   Obsługa błędów timeoutu AI (spinner + toasty).
    *   Ochrona węzła "Start" przed usunięciem.

### 2.4. Panel Administratora (`/admin`) - Opcjonalny MVP
*   **Ścieżka:** `/admin`
*   **Główny cel:** Podgląd kluczowych metryk sukcesu MVP.
*   **Kluczowe informacje:** Liczba użytkowników, statystyki generowania AI, najpopularniejsze technologie.
*   **Kluczowe komponenty:** `KPIDashboard`, `UserStatsTable`.
*   **UX/Bezpieczeństwo:** Dostęp ograniczony (whitelist GoogleID w `appsettings.json`).

## 3. Mapa podróży użytkownika (User Journey)

### Scenariusz główny: Od rejestracji do rozbudowy grafu

1.  **Start:** Użytkownik wchodzi na stronę główną.
2.  **Logowanie:** Przekierowanie na `/login`. Klika "Zaloguj przez Google".
3.  **Weryfikacja Profilu (Backend):**
    *   *Nowy użytkownik:* Przekierowanie na `/profile`.
    *   *Powracający:* Przekierowanie na `/graph`.
4.  **Onboarding (jeśli nowy):**
    *   Użytkownik wypełnia formularz (wybiera np. ".NET", "Programista", "Backend").
    *   Zapisuje profil -> System tworzy węzeł "Start".
    *   Przekierowanie na `/graph`.
5.  **Interakcja z Grafem:**
    *   Użytkownik widzi węzeł "Start".
    *   Klika węzeł -> Wybiera "Szukaj nowych technologii".
6.  **Generowanie Rekomendacji (AI):**
    *   Wyświetla się modal (Popup 2) z loaderem (do 20s).
    *   System prezentuje ~10 rekomendowanych technologii (karty).
7.  **Wybór Ścieżki:**
    *   Użytkownik przegląda karty (widzi "AI Reasoning").
    *   Zaznacza interesujące technologie checkboxami.
    *   Klika "Ignoruj" dla niechcianych (trafiają do sekcji "Ignorowane").
    *   Klika "Dodaj do grafu".
8.  **Aktualizacja Grafu:**
    *   Modal się zamyka.
    *   Nowe węzły pojawiają się na grafie połączone z węzłem "Start".
    *   Pojawia się toast "Dodano X technologii".
9.  **Nauka i Progres:**
    *   Użytkownik klika w nową technologię.
    *   Wybiera "Opcje" (Popup 1).
    *   Przesuwa suwak postępu na 50%.
    *   Dodaje notatkę prywatną.
    *   Klika "Zapisz" -> Węzeł zmienia kolor (wizualizacja postępu).

## 4. Układ i struktura nawigacji

### Layout Główny (`MainLayout.razor`)
Aplikacja wykorzystuje układ z górnym, stałym paskiem nawigacyjnym (Navbar), maksymalizując przestrzeń roboczą dla grafu.

*   **Navbar (Top Bar - 64px height):**
    *   **Lewa strona:** Logo "DeveloperGoals".
    *   **Prawa strona:**
        *   Przełącznik motywu (Słońce/Księżyc).
        *   Link do Profilu (Ikona użytkownika / Avatar).
        *   Przycisk "Wyloguj" (Ikona wyjścia).

### Routing (Blazor Router)
*   Nawigacja odbywa się bez przeładowania strony (SPA feel).
*   Nieautoryzowane próby dostępu do `/graph` lub `/profile` przekierowują na `/login`.

## 5. Kluczowe komponenty

Poniższe komponenty stanowią fundament interfejsu użytkownika:

### 5.1. `GraphCanvas.razor`
*   **Opis:** Wrapper na bibliotekę `vis.js`. Zarządza renderowaniem canvasu HTML5.
*   **Funkcje:** Inicjalizacja grafu, obsługa zdarzeń kliknięcia/podwójnego kliknięcia, zoomowanie, przesuwanie (pan), aktualizacja danych (addNodes, updateEdges) przez JS Interop.
*   **Styl:** Wypełnia 100% dostępnej przestrzeni (`calc(100vh - 64px)`).

### 5.2. `AIRecommendationsModal.razor` (Popup 2)
*   **Opis:** Modal Flowbite służący do interakcji z AI.
*   **Funkcje:**
    *   Wyświetlanie stanu ładowania (spinner z licznikiem).
    *   Prezentacja listy kart technologicznych (Checkbox, Tytuł, Opis, Reasoning).
    *   Sekcja "Ignorowane" (Accordion).
    *   Obsługa błędów (Timeout, AI Error).
    *   Batch Actions: "Dodaj wybrane", "Ignoruj wybrane".

### 5.3. `TechnologyOptionsModal.razor` (Popup 1)
*   **Opis:** Modal szczegółów pojedynczego węzła.
*   **Funkcje:**
    *   Edycja postępu (Slider 0-100%).
    *   Edycja prywatnego opisu (Textarea).
    *   Wyświetlanie informacji statycznych (Opis systemowy, Kategoria).
    *   Usuwanie węzła (z wyjątkiem węzła "Start").

### 5.4. `GraphStateService.cs` (Service)
*   **Opis:** Serwis Scoped zarządzający stanem grafu w sesji użytkownika.
*   **Funkcje:** Przechowuje listę węzłów i krawędzi, zarządza aktualnie wybranym węzłem, pośredniczy w komunikacji między komponentami UI a API backendowym.

### 5.5. `Toast.razor` & `ToastService`
*   **Opis:** System powiadomień użytkownika.
*   **Funkcje:** Wyświetlanie tymczasowych komunikatów (Sukces, Błąd, Info) w prawym górnym rogu ekranu. Kluczowe dla potwierdzania akcji (np. "Zapisano zmiany") i raportowania błędów API.
