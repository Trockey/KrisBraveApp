# Projektowanie architektury interfejsu użytkownika
_Exported on 1/11/2026 at 23:35:42 GMT+1 from Cursor (2.3.34)_

---

**User**

Jesteś wykwalifikowanym architektem frontend, którego zadaniem jest stworzenie kompleksowej architektury interfejsu użytkownika w oparciu o dokument wymagań produktu (PRD), plan API i notatki z sesji planowania. Twoim celem jest zaprojektowanie struktury interfejsu użytkownika, która skutecznie spełnia wymagania produktu, jest zgodna z możliwościami API i zawiera spostrzeżenia z sesji planowania.

Najpierw dokładnie przejrzyj następujące dokumenty:

Dokument wymagań produktu (PRD):
<prd>
@prd.md
</prd>

Plan API:
<api_plan>
@4-api-plan.md
</api_plan>

Session Notes:
<session_notes>
@21-podsumowanie-sesji-planowania-architektury-ui.md
</session_notes>

Twoim zadaniem jest stworzenie szczegółowej architektury interfejsu użytkownika, która obejmuje niezbędne widoki, mapowanie podróży użytkownika, strukturę nawigacji i kluczowe elementy dla każdego widoku. Projekt powinien uwzględniać doświadczenie użytkownika, dostępność i bezpieczeństwo.

Wykonaj następujące kroki, aby ukończyć zadanie:

1. Dokładnie przeanalizuj PRD, plan API i notatki z sesji.
2. Wyodrębnij i wypisz kluczowe wymagania z PRD.
3. Zidentyfikuj i wymień główne punkty końcowe API i ich cele.
4. Utworzenie listy wszystkich niezbędnych widoków na podstawie PRD, planu API i notatek z sesji.
5. Określenie głównego celu i kluczowych informacji dla każdego widoku.
6. Zaplanuj podróż użytkownika między widokami, w tym podział krok po kroku dla głównego przypadku użycia.
7. Zaprojektuj strukturę nawigacji.
8. Zaproponuj kluczowe elementy interfejsu użytkownika dla każdego widoku, biorąc pod uwagę UX, dostępność i bezpieczeństwo.
9. Rozważ potencjalne przypadki brzegowe lub stany błędów.
10. Upewnij się, że architektura interfejsu użytkownika jest zgodna z planem API.
11. Przejrzenie i zmapowanie wszystkich historyjek użytkownika z PRD do architektury interfejsu użytkownika.
12. Wyraźne mapowanie wymagań na elementy interfejsu użytkownika.
13. Rozważ potencjalne punkty bólu użytkownika i sposób, w jaki interfejs użytkownika je rozwiązuje.

Dla każdego głównego kroku pracuj wewnątrz tagów <ui_architecture_planning> w bloku myślenia, aby rozbić proces myślowy przed przejściem do następnego kroku. Ta sekcja może być dość długa. To w porządku, że ta sekcja może być dość długa.

Przedstaw ostateczną architekturę interfejsu użytkownika w następującym formacie Markdown:

```markdown
# Architektura UI dla [Nazwa produktu]

## 1. Przegląd struktury UI

[Przedstaw ogólny przegląd struktury UI]

## 2. Lista widoków

[Dla każdego widoku podaj:
- Nazwa widoku
- Ścieżka widoku
- Główny cel
- Kluczowe informacje do wyświetlenia
- Kluczowe komponenty widoku
- UX, dostępność i względy bezpieczeństwa]

## 3. Mapa podróży użytkownika

[Opisz przepływ między widokami i kluczowymi interakcjami użytkownika]

## 4. Układ i struktura nawigacji

[Wyjaśnij, w jaki sposób użytkownicy będą poruszać się między widokami]

## 5. Kluczowe komponenty

[Wymień i krótko opisz kluczowe komponenty, które będą używane w wielu widokach].
```

Skup się wyłącznie na architekturze interfejsu użytkownika, podróży użytkownika, nawigacji i kluczowych elementach dla każdego widoku. Nie uwzględniaj szczegółów implementacji, konkretnego projektu wizualnego ani przykładów kodu, chyba że są one kluczowe dla zrozumienia architektury.

Końcowy rezultat powinien składać się wyłącznie z architektury UI w formacie Markdown w języku polskim, którą zapiszesz w pliku .ai/ui-plan.md. Nie powielaj ani nie powtarzaj żadnej pracy wykonanej w bloku myślenia.

---

**Cursor**

Stworzyłem kompleksową architekturę interfejsu użytkownika w oparciu o dostarczone dokumenty (PRD, Plan API oraz notatki z sesji). Plan został zapisany w pliku `.ai/ui-plan.md`.

Oto podsumowanie kluczowych decyzji architektonicznych zawartych w planie:

1.  **Technologia:** Aplikacja będzie oparta na **Blazor Server** z wykorzystaniem **Flowbite** (Tailwind CSS) dla komponentów UI oraz **vis.js** do wizualizacji grafu.
2.  **Struktura widoków:**
    *   `/login` - Minimalistyczne logowanie przez Google OAuth.
    *   `/profile` - Strona onboardingu do uzupełnienia profilu (wymagane min. 5 technologii).
    *   `/graph` - Główny widok "dashboardu" z pełnoekranowym grafem technologii.
3.  **Kluczowe komponenty:**
    *   **GraphCanvas:** Wrapper na bibliotekę vis.js obsługujący interakcje z grafem.
    *   **AIRecommendationsModal (Popup 2):** Modal do generowania ścieżek rozwoju, obsługujący stany ładowania i błędy timeoutu AI.
    *   **TechnologyOptionsModal (Popup 1):** Modal do zarządzania postępem (suwak 0-100%) i notatkami.
4.  **UX i Bezpieczeństwo:**
    *   Zastosowano **Service Layer Pattern** (np. `GraphStateService`) do oddzielenia logiki od prezentacji.
    *   Uwzględniono obsługę błędów (toasty, spinnery) oraz walidację formularzy zgodnie z wymaganiami API.
    *   Nawigacja została uproszczona do górnego paska (Navbar), aby zmaksymalizować przestrzeń roboczą grafu.

Pełny dokument znajduje się w: `.ai/ui-plan.md`.

