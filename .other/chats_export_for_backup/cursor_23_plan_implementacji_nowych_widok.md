# Plan implementacji nowych widoków
_Exported on 1/12/2026 at 00:15:58 GMT+1 from Cursor (2.3.34)_

---

**User**

 Jako starszy programista frontendu Twoim zadaniem jest stworzenie szczegółowego planu wdrożenia nowych widoków w aplikacji internetowej. Plan ten powinien być kompleksowy i wystarczająco jasny dla innego programisty frontendowego, aby mógł poprawnie i wydajnie wdrożyć wszystkie widoki.
Weź pod uwagę że niektóre widoki już istnieją - jeśli tak, zmodyfikuj je.|
Jeśli będzie brakowało end-pointów, utwórz je, tak jakbyś był back-endowcem.

Najpierw przejrzyj następujące informacje:

1. Product Requirements Document (PRD):
<prd>
@prd.md
</prd>

2. Opis widoku:
<view_description>
@22-ui-plan.md
</view_description>

3. User Stories:
<user_stories>

      ### US-001: Rejestracja i pierwsze logowanie
      Tytuł: Użytkownik rejestruje się w aplikacji przez Google OAuth

      Opis:  
      JAKO nowy użytkownik  
      CHCĘ zalogować się za pomocą konta Google  
      ABY szybko uzyskać dostęp do aplikacji bez tworzenia nowego konta

      Kryteria akceptacji:
      - System wyświetla przycisk "Zaloguj się przez Google"
      - Po kliknięciu przycisku użytkownik jest przekierowywany do strony logowania Google
      - Po zalogowaniu użytkownik jest przekierowywany z powrotem do aplikacji
      - System tworzy nowe konto użytkownika jeśli loguje się po raz pierwszy
      - System zapisuje podstawowe dane z konta Google (email, nazwa)
      - Użytkownik jest przekierowywany do strony wypełniania profilu jeśli profil jest pusty
      - Użytkownik jest przekierowywany do grafu technologii jeśli profil jest już wypełniony

      ### US-002: Wypełnianie profilu - onboarding
      Tytuł: Nowy użytkownik wypełnia swój profil technologiczny

      Opis:  
      JAKO nowy użytkownik  
      CHCĘ szybko wypełnić swój profil technologiczny  
      ABY otrzymać spersonalizowane rekomendacje

      Kryteria akceptacji:
      - Strona profilu zawiera formularz z następującymi polami:
        - Główne technologie (wielokrotny wybór z listy: Java, .NET, JavaScript, Python, itd.)
        - Minimum 5 technologii musi zostać wybranych
        - Rola (radiobuttons lub dropdown): Programista / Tester / Analityk
        - Obszar rozwoju (radiobuttons lub dropdown): User Interface / Backend / Full Stack / Testing
      - Wszystkie pola są wymagane
      - Czas wypełnienia nie powinien przekraczać 3 minut
      - System waliduje, czy wszystkie wymagane pola zostały wypełnione
      - Po zapisaniu profilu użytkownik jest przekierowywany do grafu technologii
      - Na grafie znajduje się domyślny węzeł "Start"

      ### US-003: Edycja profilu
      Tytuł: Użytkownik edytuje swój profil

      Opis:  
      JAKO zalogowany użytkownik  
      CHCĘ móc edytować swój profil w dowolnym momencie  
      ABY dostosować go do zmian w mojej karierze

      Kryteria akceptacji:
      - W aplikacji dostępny jest link/przycisk "Profil" lub "Edytuj profil"
      - Po kliknięciu użytkownik widzi formularz ze swoimi aktualnymi danymi
      - Użytkownik może zmienić:
        - Główne technologie
        - Rolę
        - Obszar rozwoju
      - Wszystkie pola pozostają wymagane
      - Po zapisaniu zmian system wyświetla komunikat potwierdzający
      - Zmiana profilu NIE powoduje automatycznej regeneracji grafu technologii
      - Istniejący graf pozostaje nienaruszony

      ### US-004: Wyświetlanie pustego grafu technologii
      Tytuł: Użytkownik po raz pierwszy widzi graf technologii

      Opis:  
      JAKO nowy użytkownik, który właśnie wypełnił profil  
      CHCĘ zobaczyć mój graf technologii  
      ABY rozpocząć budowanie swojej ścieżki rozwoju

      Kryteria akceptacji:
      - Po wypełnieniu profilu użytkownik jest przekierowywany do strony z grafem
      - Graf zawiera tylko jeden węzeł: "Start"
      - Węzeł "Start" jest klikalny
      - Po kliknięciu węzła "Start" wyświetlane są dwa przyciski:
        - "Szukaj nowych technologii"
        - "Opcje" (opcjonalnie dla węzła Start może być nieaktywny)
      - Interfejs jest intuicyjny i zawiera wskazówki dla nowego użytkownika

      ### US-005: Generowanie pierwszych rekomendacji AI
      Tytuł: Użytkownik generuje pierwsze rekomendacje technologii

      Opis:  
      JAKO użytkownik z grafem który zawiera tylko jeden node "Start"
      CHCĘ wygenerować pierwsze rekomendacje technologii  
      ABY rozpocząć planowanie swojej ścieżki rozwoju

      Kryteria akceptacji:
      - Użytkownik klika węzeł "Start" na grafie
      - Użytkownik klika przycisk "Szukaj nowych technologii"
      - System wyświetla spinner/loader z komunikatem "Generowanie rekomendacji..."
      - System wysyła request do AI zawierający:
        - Pełny profil użytkownika
        - Informację o węźle początkowym
      - Maksymalny czas oczekiwania: 20 sekund
      - Jeśli AI odpowie w czasie - system wyświetla Popup 2 z rekomendacjami
      - Jeśli timeout - system wyświetla komunikat błędu: "Nie udało się wygenerować rekomendacji. Spróbuj ponownie."
      - Popup 2 zawiera około 10 rekomendowanych technologii
      - Dla każdej technologii wyświetlane są:
        - Nazwa z prefiksem (np. "DotNet - ASP.NET Core")
        - Tag (Technologia/Framework/Baza danych)
        - Krótki opis
        - Reasoning (dlaczego jest polecana)

      ### US-006: Dodawanie technologii do grafu
      Tytuł: Użytkownik dodaje wybraną technologię do grafu

      Opis:  
      JAKO użytkownik przeglądający rekomendacje AI na Popupie 2
      CHCĘ dodać wybraną technologię do mojego grafu  
      ABY rozpocząć jej naukę i śledzić postęp

      Kryteria akceptacji:
      - W Popup 2 każda rekomendowana technologia posiada checkbox a na dole aplikacji jest przycisk "Dodaj do grafu". Dodawane są do grafu zaznaczone technologie przez checkbox
      - Po kliknięciu przycisku:
        - Technologia (jedna lub więcej) dodaje się do grafu jako nowy węzeł
        - Tworzona jest krawędź (połączenie) między węzłem źródłowym a nową technologią
        - System zapisuje zależność w bazie danych
        - Technologia znika z listy rekomendacji w Popup 2
      - Nowa technologia jest widoczna na grafie
      - Nowa technologia ma domyślny postęp 0%
      - Popup pozostaje otwarty, aby użytkownik mógł dodać więcej technologii
      - Użytkownik może zamknąć popup w dowolnym momencie

      ### US-007: Ignorowanie rekomendowanej technologii
      Tytuł: Użytkownik ignoruje nieinteresującą technologię

      Opis:  
      JAKO użytkownik przeglądający rekomendacje AI  
      CHCĘ zignorować technologie, które mnie nie interesują  
      ABY skupić się tylko na istotnych dla mnie technologiach

      Kryteria akceptacji:
      - W Popup 2  rekomendowana technologia posiada checkbox
      - Na dole popup 2 znaduje się przycisk "Ignoruj"
      - Po kliknięciu przycisku "Ignoruj":
        - Technologia przenosi się na listę "Ignorowane" w tym samym popupie
        - Technologia jest zapisywana w bazie danych ze statusem "Ignored"
        - Technologia znika z głównej listy rekomendacji a pojawia się na liście "Ignoruj"
      - Sekcja "Ignorowane" jest widoczna poniżej głównej listy rekomendacji
      - Użytkownik może przeglądać ignorowane technologie
      - Ignorowana technologia NIE pojawia się w przyszłych rekomendacjach (dla tego samego kontekstu)

      ### US-008: Przywracanie ignorowanej technologii
      Tytuł: Użytkownik przywraca wcześniej zignorowaną technologię

      Opis:  
      JAKO użytkownik, który wcześniej zignorował technologię  
      CHCĘ móc przywrócić tę technologię  
      ABY dodać ją do mojego grafu jeśli zmienię zdanie

      Kryteria akceptacji:
      - W sekcji "Ignorowane" (Popup 2) każda technologia posiada checkbox
      - Na dole popup 2, pod listą "Ignorowane" znajduje się przycisk "Aktywuj"
      - Po kliknięciu przycisku:
        - Technologie zaznaczone checkboxem przenoszą się z powrotem do głównej listy rekomendacji
        - Status w bazie danych zmienia się z "Ignored" na "Active"
        - Technologia jest dostępna do dodania do grafu
      - Użytkownik może teraz dodać technologię do grafu standardową akcją

      ### US-009: Przeglądanie listy ignorowanych technologii
      Tytuł: Użytkownik przegląda wszystkie ignorowane technologie

      Opis:  
      JAKO użytkownik  
      CHCĘ widzieć wszystkie technologie, które zignorowałem  
      ABY móc zarządzać swoimi preferencjami

      Kryteria akceptacji:
      - W Popup 2 sekcja "Ignorowane" wyświetla wszystkie zignorowane technologie
      - Lista zawiera technologie z informacjami: nazwa, prefix, tag, opis
      - Lista jest przeglądalna (scrollowalna jeśli jest długa)
      - Każda technologia ma przycisk "Przywróć"
      - Jeśli brak ignorowanych technologii - wyświetlany jest odpowiedni komunikat

      ### US-010: Generowanie kolejnych rekomendacji od wybranej technologii
      Tytuł: Użytkownik generuje rekomendacje od konkretnej technologii na grafie

      Opis:  
      JAKO użytkownik z technologiami na grafie  
      CHCĘ wygenerować kolejne rekomendacje od wybranej technologii  
      ABY rozwijać konkretną ścieżkę technologiczną

      Kryteria akceptacji:
      - Użytkownik klika dowolny węzeł technologii na grafie (nie tylko "Start")
      - Wyświetlane są dwa przyciski: "Opcje" i "Szukaj nowych technologii"
      - Po kliknięciu "Szukaj nowych technologii":
        - System wyświetla popup 2
        - System wyświetla spinner/loader
        - System wysyła do AI:
          - Pełny profil użytkownika
          - Wszystkie technologie z grafu (bez ignorowanych)
          - Akcent na wybraną technologię
        - Timeout: 20 sekund
        - System wyświetla Popup 2 z około 10 nowymi rekomendacjami
      - Rekomendacje są kontekstowo powiązane z wybraną technologią
      - Użytkownik może generować kolejne rekomendacje nawet jeśli technologia nie ma 100% postępu

      ### US-011: Edycja prywatnego opisu technologii
      Tytuł: Użytkownik dodaje lub edytuje prywatny opis technologii

      Opis:  
      JAKO użytkownik  
      CHCĘ dodać własny opis do technologii  
      ABY zapisać swoje notatki, przemyślenia lub linki (jako tekst)

      Kryteria akceptacji:
      - Użytkownik klika węzeł technologii na grafie
      - Użytkownik klika przycisk "Opcje"
      - Otwiera się Popup 1 zawierający:
        - Pole tekstowe z prywatnym opisem (textarea)
        - Aktualny opis użytkownika (jeśli istnieje)
        - Przycisk "Zapisz"
      - Użytkownik może wpisać lub edytować opis
      - Po kliknięciu "Zapisz":
        - System zapisuje opis w bazie danych
        - Popup zamyka się lub wyświetla komunikat potwierdzający
      - Prywatny opis jest widoczny tylko dla tego użytkownika
      - Opis może być pusty (opcjonalny)

      ### US-012: Aktualizacja postępu nauki technologii
      Tytuł: Użytkownik aktualizuje procent postępu w nauce technologii

      Opis:  
      JAKO użytkownik uczący się technologii  
      CHCĘ śledzić swój postęp w nauce  
      ABY mieć motywację i wiedzieć, ile już się nauczyłem

      Kryteria akceptacji:
      - Użytkownik klika węzeł technologii na grafie
      - Użytkownik klika przycisk "Opcje"
      - Popup 1 zawiera suwak (slider) do ustawienia postępu (0-100%)
      - Suwak pokazuje aktualny postęp
      - Użytkownik może przesunąć suwak na wybraną wartość procentową
      - Po kliknięciu "Zapisz":
        - System zapisuje nowy postęp w bazie danych
        - Postęp jest widoczny na grafie (np. jako kolor węzła, tekst, itp.)
      - Postęp może być aktualizowany wielokrotnie
      - Użytkownik może ustawić dowolną wartość od 0% do 100%

      ### US-013: Usuwanie technologii z grafu
      Tytuł: Użytkownik usuwa technologię z grafu

      Opis:  
      JAKO użytkownik  
      CHCĘ móc usunąć technologię z mojego grafu  
      ABY pozbyć się technologii, których już nie planuję się uczyć

      Kryteria akceptacji:
      - Użytkownik klika węzeł technologii na grafie
      - Użytkownik klika przycisk "Opcje"
      - Popup 1 zawiera przycisk "Usuń" lub "Usuń z grafu"
      - Po kliknięciu przycisku:
        - System wyświetla dialog potwierdzenia: "Czy na pewno chcesz usunąć tę technologię?"
        - Jeśli użytkownik potwierdzi:
          - Technologia jest usuwana z grafu
          - Zależności (krawędzie) związane z tą technologią są usuwane
          - Jeśli istnieją technologie "potomne", one również mogą zostać usunięte lub odłączone (do ustalenia)
          - Technologia jest usuwana z bazy danych (lub oznaczana jako nieaktywna)
          - Okno popup 1 zamyka się
      - Graf jest automatycznie aktualizowany
      - Węzeł "Start" NIE może być usunięty

      ### US-014: Wyświetlanie szczegółów technologii na grafie
      Tytuł: Użytkownik przegląda szczegóły technologii

      Opis:  
      JAKO użytkownik  
      CHCĘ widzieć szczegóły technologii na moim grafie  
      ABY przypomnieć sobie informacje o niej

      Kryteria akceptacji:
      - Na grafie każdy węzeł technologii wyświetla:
        - Nazwę z prefiksem (np. "DotNet - Entity Framework")
        - Procent postępu (jeśli > 0%)
      - Po kliknięciu węzła i przycisku "Opcje" w Popup 1 użytkownik widzi:
        - Pełną nazwę
        - Prefix/kategorię
        - Tag (Technologia/Framework/Baza danych)
        - Opis systemowy (wygenerowany przez AI)
        - Prywatny opis użytkownika (jeśli istnieje)
        - Aktualny postęp (%)
      - Wszystkie informacje są czytelnie zaprezentowane

      ### US-015: Nawigacja po grafie technologii (niektóre z wymieniinych tutaj opcji są opcjonalne)
      Tytuł: Użytkownik nawiguje po grafie technologii

      Opis:  
      JAKO użytkownik z dużym grafem technologii  
      CHCĘ móc łatwo nawigować po moim grafie  
      ABY znaleźć interesujące mnie technologie

      Kryteria akceptacji:
      - Graf jest responsywny i płynny
      - Użytkownik może:
        - Przybliżać (zoom in) i oddalać (zoom out) graf
        - Przesuwać widok grafu (pan)
        - Klikać węzły technologii
      - Graf automatycznie dopasowuje się do okna przeglądarki
      - Zależności między technologiami są wyraźnie widoczne (linie/krawędzie)
      - Graf ma estetyczny i czytelny układ (automatyczny layout)

      ### US-016: Wyświetlanie zależności między technologiami
      Tytuł: Użytkownik widzi ścieżki rozwoju technologicznego

      Opis:  
      JAKO użytkownik  
      CHCĘ widzieć zależności między technologiami  
      ABY rozumieć swoją ścieżkę rozwoju

      Kryteria akceptacji:
      - Graf wyświetla linie/krawędzie między technologiami
      - Każda linia pokazuje zależność: "od technologii A do technologii B"
      - Kierunek zależności jest wyraźny (strzałka lub orientacja)
      - Graf buduje się zgodnie z kolejnością dodawania technologii przez użytkownika
      - Użytkownik widzi, które technologie są "następne" w jego ścieżce rozwoju
      - Każda ścieżka zaczyna się od węzła "Start"

      ### US-017: Logout z aplikacji
      Tytuł: Użytkownik wylogowuje się z aplikacji

      Opis:  
      JAKO zalogowany użytkownik  
      CHCĘ móc się wylogować  
      ABY zakończyć sesję i zabezpieczyć moje dane

      Kryteria akceptacji:
      - W aplikacji dostępny jest przycisk/link "Wyloguj" lub "Logout"
      - Po kliknięciu:
        - System wylogowuje użytkownika
        - Sesja użytkownika jest zakończona
        - Użytkownik jest przekierowywany do strony logowania
      - Po wylogowaniu użytkownik nie ma dostępu do chronionych stron
      - Próba dostępu do chronionych stron bez logowania przekierowuje do strony logowania

      ### US-018: Powrót do aplikacji po wylogowaniu
      Tytuł: Użytkownik loguje się ponownie po wcześniejszym wylogowaniu

      Opis:  
      JAKO użytkownik, który wcześniej korzystał z aplikacji  
      CHCĘ zalogować się ponownie  
      ABY kontynuować pracę z moim grafem

      Kryteria akceptacji:
      - Użytkownik klika "Zaloguj się przez Google"
      - Po zalogowaniu:
        - System rozpoznaje użytkownika
        - Użytkownik jest przekierowywany do swojego grafu technologii
        - Wszystkie dane użytkownika (profil, graf, technologie) są zachowane
        - Graf wyświetla się w takim stanie, w jakim był przed wylogowaniem

      ### US-019: Obsługa błędu podczas generowania rekomendacji (timeout)
      Tytuł: System obsługuje timeout podczas generowania rekomendacji AI

      Opis:  
      JAKO użytkownik  
      CHCĘ otrzymać informację zwrotną gdy generowanie rekomendacji trwa zbyt długo  
      ABY wiedzieć co się dzieje i móc spróbować ponownie

      Kryteria akceptacji:
      - Podczas generowania rekomendacji system wyświetla spinner z informacją
      - Jeśli AI nie odpowie w ciągu 20 sekund:
        - Spinner znika
        - System wyświetla komunikat błędu: "Nie udało się wygenerować rekomendacji. Spróbuj ponownie."
        - Popup się nie otwiera lub zamyka (jeśli był otwarty)
      - Użytkownik może kliknąć ponownie "Szukaj nowych technologii" aby spróbować jeszcze raz
      - Błąd jest logowany po stronie serwera (dla celów diagnostycznych)

      ### US-020: Obsługa błędu podczas generowania rekomendacji (błąd AI)
      Tytuł: System obsługuje błędy zwracane przez AI

      Opis:  
      JAKO użytkownik  
      CHCĘ otrzymać informację gdy AI nie może wygenerować rekomendacji  
      ABY wiedzieć że to nie problem z moim połączeniem

      Kryteria akceptacji:
      - Jeśli AI zwróci błąd (np. 500, 503):
        - System wyświetla komunikat: "Wystąpił błąd podczas generowania rekomendacji. Spróbuj ponownie później."
      - Jeśli AI zwróci nieprawidłowy format JSON:
        - System wyświetla komunikat: "Otrzymano nieprawidłowe dane. Spróbuj ponownie."
      - Użytkownik może spróbować ponownie
      - Błędy są logowane po stronie serwera
      - Użytkownik nie widzi technicznych szczegółów błędu

      ### US-021: Responsywność aplikacji na urządzeniach mobilnych (opcjonalnie)
      Tytuł: Użytkownik korzysta z aplikacji na smartfonie

      Opis:  
      JAKO użytkownik korzystający ze smartfona  
      CHCĘ móc korzystać z aplikacji na małym ekranie  
      ABY śledzić postępy w nauce również mobilnie

      Kryteria akceptacji:
      - Aplikacja jest responsywna i działa na urządzeniach mobilnych
      - Graf technologii jest widoczny i klikalny na małym ekranie
      - Przyciski i kontrolki są wystarczająco duże do kliknięcia palcem
      - Formularz profilu jest czytelny na małym ekranie
      - Popupy (Popup 1, Popup 2) są responsywne
      - Nawigacja jest intuicyjna na urządzeniach dotykowych
      - Teksty są czytelne bez konieczności zoomowania

      ### US-022: Dodawanie własnych niestandardowych technologii
      Tytuł: Użytkownik dodaje własną technologię spoza rekomendacji AI

      Opis:  
      JAKO użytkownik  
      CHCĘ móc dodać własną technologię, której nie ma w rekomendacjach AI  
      ABY mieć pełną kontrolę nad moim grafem rozwoju

      Kryteria akceptacji:
      - W interfejsie (np. w Popup 2 lub osobna opcja) istnieje przycisk "Dodaj własną technologię"
      - Po kliknięciu wyświetla się formularz z polami:
        - Nazwa technologii (wymagane)
        - Prefix/kategoria (dropdown lub input, wymagane)
        - Tag: Technologia/Framework/Baza danych (dropdown, wymagane)
        - Opis (opcjonalny)
      - Po zapisaniu:
        - Technologia dodaje się do grafu jako nowy węzeł
        - Tworzona jest zależność od wybranego węzła źródłowego
        - Technologia jest zapisana w bazie danych
      - Użytkownik może dodać dowolną liczbę własnych technologii

      ### US-023: Walidacja wymaganych pól w profilu
      Tytuł: System waliduje wypełnienie profilu użytkownika

      Opis:  
      JAKO użytkownik wypełniający profil  
      CHCĘ otrzymać informację o brakujących polach  
      ABY wiedzieć co muszę jeszcze uzupełnić

      Kryteria akceptacji:
      - Przy próbie zapisania niekompletnego profilu:
        - System wyświetla komunikaty walidacyjne przy pustych polach
        - Komunikaty są jasne: "To pole jest wymagane"
      - Przed kliknięciem "Szukaj nowych technologii" system sprawdza czy profil jest kompletny:
        - Jeśli profil jest niekompletny - wyświetla komunikat: "Uzupełnij swój profil przed wyszukiwaniem technologii"
        - Użytkownik jest przekierowywany do strony profilu
      - Walidacja działa zarówno po stronie klienta (JavaScript) jak i serwera
      - Minimalna liczba technologii w profilu: 5

      ### US-024: Persistencja danych użytkownika
      Tytuł: System zachowuje dane użytkownika między sesjami

      Opis:  
      JAKO użytkownik  
      CHCĘ aby moje dane były zachowane po zamknięciu przeglądarki  
      ABY móc kontynuować pracę później

      Kryteria akceptacji:
      - Wszystkie dane użytkownika są zapisywane w bazie danych:
        - Profil użytkownika
        - Graf technologii (węzły i krawędzie)
        - Postęp w nauce
        - Prywatne opisy
        - Lista ignorowanych technologii
      - Po ponownym zalogowaniu użytkownik widzi dokładnie ten sam stan aplikacji
      - Dane są zapisywane automatycznie po każdej akcji użytkownika
      - Brak potrzeby ręcznego zapisywania

      ### US-025: Izolacja danych między użytkownikami
      Tytuł: Każdy użytkownik ma swoje własne dane

      Opis:  
      JAKO użytkownik  
      CHCĘ mieć pewność że moje dane są prywatne  
      ABY inni użytkownicy nie widzieli mojego grafu

      Kryteria akceptacji:
      - Każdy użytkownik widzi tylko swoje dane:
        - Własny profil
        - Własny graf technologii
        - Własne ignorowane technologie
        - Własne postępy
      - System nie udostępnia danych między użytkownikami
      - Brak funkcji przeglądania profili innych użytkowników
      - Zależności między technologiami są unikalne dla każdego użytkownika
      - Lista technologii nie jest współdzielona

      ### US-026: Generowanie rekomendacji bez limitów
      Tytuł: Użytkownik może generować rekomendacje bez ograniczeń

      Opis:  
      JAKO użytkownik  
      CHCĘ móc generować rekomendacje ile razy chcę  
      ABY eksplorować różne ścieżki rozwoju

      Kryteria akceptacji:
      - Brak limitów dziennych na generowanie rekomendacji
      - Brak mechanizmów anti-spam
      - Użytkownik może klikać "Szukaj nowych technologii" wielokrotnie
      - Każde kliknięcie generuje nowy request do AI
      - System nie blokuje użytkownika z powodu zbyt częstego używania funkcji
      - Jedyne ograniczenie: timeout 20 sekund na każdy request

      ### US-027: Sprawdzenie stanu aplikacji bez danych
      Tytuł: Nowy użytkownik widzi pusty stan aplikacji

      Opis:  
      JAKO nowy użytkownik  
      CHCĘ widzieć pomocne komunikaty gdy nie mam jeszcze danych  
      ABY wiedzieć jakie akcje powinienem wykonać

      Kryteria akceptacji:
      - Jeśli użytkownik nie ma wypełnionego profilu:
        - Widzi komunikat: "Uzupełnij swój profil aby rozpocząć"
        - Jest przekierowywany do strony profilu
      - Jeśli użytkownik ma profil ale pusty graf:
        - Widzi tylko węzeł "Start"
        - Widzi wskazówkę: "Kliknij Start i wybierz 'Szukaj nowych technologii' aby otrzymać pierwsze rekomendacje"
      - Komunikaty są przyjazne i motywujące

      ### US-028: Specjalizacja użytkownika jest respektowana
      Tytuł: AI generuje rekomendacje zgodne ze specjalizacją użytkownika

      Opis:  
      JAKO użytkownik specjalizujący się w konkretnej technologii  
      CHCĘ otrzymywać rekomendacje zgodne z moją specjalizacją  
      ABY nie tracić czasu na nieistotne technologie

      Kryteria akceptacji:
      - AI bierze pod uwagę pełny profil użytkownika przy generowaniu rekomendacji:
        - Główne technologie
        - Rolę
        - Obszar rozwoju
      - Przykład: użytkownik z profilem ".NET Backend Developer" nie otrzymuje rekomendacji Java
      - Przykład: użytkownik z profilem "Frontend Developer" otrzymuje rekomendacje JavaScript, React, Vue, ale nie C# backend frameworks
      - Rekomendacje są kontekstowo dopasowane do całej ścieżki rozwoju użytkownika
      - System nie sugeruje technologii z zupełnie innych obszarów niż profil użytkownika

      ### US-029: Reasoning dla rekomendacji AI
      Tytuł: Użytkownik widzi uzasadnienie dla rekomendowanych technologii

      Opis:  
      JAKO użytkownik przeglądający rekomendacje  
      CHCĘ wiedzieć dlaczego dana technologia jest mi polecana  
      ABY podejmować świadome decyzje o nauce

      Kryteria akceptacji:
      - Każda rekomendowana technologia w Popup 2 zawiera pole "reasoning" (uzasadnienie)
      - Reasoning zawiera krótkie wyjaśnienie (1-3 zdania) dlaczego ta technologia jest polecana
      - Przykłady reasoning:
        - "Naturalna kolejność po Entity Framework, pomaga w optymalizacji zapytań"
        - "Popularna technologia w 2025, wysoka liczba ofert pracy"
        - "Uzupełnia Twoją wiedzę o Backend, aktualny trend w architekturze"
      - Reasoning jest wygenerowane przez AI
      - Reasoning jest wyświetlany czytelnie pod opisem technologii

      ### US-030: Różnorodność rekomendacji AI
      Tytuł: AI generuje różnorodne rekomendacje

      Opis:  
      JAKO użytkownik  
      CHCĘ otrzymywać różnorodne rekomendacje przy każdym generowaniu  
      ABY móc eksplorować różne kierunki rozwoju

      Kryteria akceptacji:
      - AI generuje około 10 technologii przy każdym request
      - Technologie powinny być różnorodne (nie wszystkie z tej samej kategorii)
      - AI bierze pod uwagę trendy rynkowe, GitHub Trends, ogłoszenia o pracę
      - Rekomendacje mogą zawierać:
        - Technologie bezpośrednio powiązane z wybraną
        - Technologie komplementarne
        - Technologie popularne w danym obszarze
        - Technologie przyszłościowe (trendy)
      - Przy wielokrotnym generowaniu z tego samego węzła, rekomendacje mogą się różnić


</user_stories>

4. Endpoint Description:
<endpoint_description>

        ### 2.1 Authentication

        #### POST /api/auth/login
        **Description:** Initiates Google OAuth login flow  
        **Authentication:** None (public)

        **Response 200:**
        ```json
        {
          "redirectUrl": "https://accounts.google.com/o/oauth2/v2/auth?client_id=..."
        }
        ```

        **Response 500:**
        ```json
        {
          "error": "InternalServerError",
          "message": "Failed to initiate OAuth flow"
        }
        ```

        ---

        #### GET /api/auth/callback
        **Description:** Handles Google OAuth callback, creates or retrieves user  
        **Authentication:** None (public)  
        **Query Parameters:**
        - `code` (required): OAuth authorization code
        - `state` (optional): CSRF protection state

        **Response 200:**
        ```json
        {
          "user": {
            "id": 123,
            "email": "user@example.com",
            "name": "John Doe",
            "hasProfile": true
          },
          "redirectTo": "/dashboard" // or "/profile" if hasProfile=false
        }
        ```

        **Response 401:**
        ```json
        {
          "error": "Unauthorized",
          "message": "Invalid OAuth code"
        }
        ```

        ---

        #### POST /api/auth/logout
        **Description:** Logs out current user and terminates session  
        **Authentication:** Required

        **Response 200:**
        ```json
        {
          "message": "Successfully logged out"
        }
        ```

        ---

        #### GET /api/auth/me
        **Description:** Returns current authenticated user information  
        **Authentication:** Required

        **Response 200:**
        ```json
        {
          "id": 123,
          "email": "user@example.com",
          "name": "John Doe",
          "googleId": "1234567890",
          "createdAt": "2025-11-15T10:30:00Z",
          "lastLoginAt": "2025-12-01T08:15:00Z",
          "hasProfile": true
        }
        ```

        **Response 401:**
        ```json
        {
          "error": "Unauthorized",
          "message": "User not authenticated"
        }
        ```

        ---

        ### 2.2 User Profile

        #### GET /api/profile
        **Description:** Retrieves current user's profile  
        **Authentication:** Required

        **Response 200:**
        ```json
        {
          "id": 456,
          "userId": 123,
          "mainTechnologies": [
            "C#",
            ".NET Core",
            "PostgreSQL",
            "JavaScript",
            "Docker"
          ],
          "role": "Programista",
          "developmentArea": "Backend",
          "createdAt": "2025-11-15T10:35:00Z",
          "updatedAt": "2025-11-20T14:22:00Z"
        }
        ```

        **Response 404:**
        ```json
        {
          "error": "NotFound",
          "message": "Profile not found. Please complete onboarding."
        }
        ```

        ---

        #### POST /api/profile
        **Description:** Creates user profile (onboarding). Automatically creates "Start" node in user's technology graph.  
        **Authentication:** Required

        **Request Body:**
        ```json
        {
          "mainTechnologies": [
            "C#",
            ".NET Core",
            "PostgreSQL",
            "JavaScript",
            "Docker"
          ],
          "role": "Programista",
          "developmentArea": "Backend"
        }
        ```

        **Validation Rules:**
        - `mainTechnologies`: Array of strings, minimum 5 items, maximum 50 items
        - `role`: Enum - "Programista" | "Tester" | "Analityk" | "DataScienceSpecialist"
        - `developmentArea`: Enum - "UserInterface" | "Backend" | "FullStack" | "Testing" | "DataScience" | "DevOps"

        **Response 201:**
        ```json
        {
          "id": 456,
          "userId": 123,
          "mainTechnologies": ["C#", ".NET Core", "PostgreSQL", "JavaScript", "Docker"],
          "role": "Programista",
          "developmentArea": "Backend",
          "createdAt": "2025-11-15T10:35:00Z",
          "startNodeCreated": true
        }
        ```

        **Response 400:**
        ```json
        {
          "error": "ValidationError",
          "message": "Validation failed",
          "errors": [
            {
              "field": "mainTechnologies",
              "message": "Profile must contain at least 5 technologies"
            }
          ]
        }
        ```

        **Response 409:**
        ```json
        {
          "error": "Conflict",
          "message": "Profile already exists. Use PUT to update."
        }
        ```

        ---

        #### PUT /api/profile
        **Description:** Updates user profile. Does NOT regenerate technology graph.  
        **Authentication:** Required

        **Request Body:**
        ```json
        {
          "mainTechnologies": [
            "C#",
            ".NET Core",
            "PostgreSQL",
            "React",
            "Docker",
            "Kubernetes"
          ],
          "role": "Programista",
          "developmentArea": "FullStack"
        }
        ```

        **Response 200:**
        ```json
        {
          "id": 456,
          "userId": 123,
          "mainTechnologies": ["C#", ".NET Core", "PostgreSQL", "React", "Docker", "Kubernetes"],
          "role": "Programista",
          "developmentArea": "FullStack",
          "createdAt": "2025-11-15T10:35:00Z",
          "updatedAt": "2025-12-01T09:20:00Z"
        }
        ```

        **Response 400:** (Same validation as POST)

        **Response 404:**
        ```json
        {
          "error": "NotFound",
          "message": "Profile not found. Use POST to create."
        }
        ```

        ---

        ### 2.3 Technologies (Graph Nodes)

        #### GET /api/technologies
        **Description:** Retrieves all active technologies for current user  
        **Authentication:** Required  
        **Query Parameters:**
        - `status` (optional): "Active" | "All" (default: "Active")
        - `includeStart` (optional): boolean (default: true)

        **Response 200:**
        ```json
        {
          "technologies": [
            {
              "id": 1001,
              "userId": 123,
              "technologyDefinitionId": 1,
              "name": "Start",
              "prefix": "System",
              "tag": "Metodologia",
              "systemDescription": "Starting point for your technology journey",
              "privateDescription": null,
              "progress": 100,
              "status": "Active",
              "isCustom": false,
              "isStart": true,
              "aiReasoning": null,
              "createdAt": "2025-11-15T10:35:00Z",
              "updatedAt": null
            },
            {
              "id": 1002,
              "userId": 123,
              "technologyDefinitionId": 15,
              "name": "DotNet - Entity Framework",
              "prefix": "DotNet",
              "tag": "Framework",
              "systemDescription": "Object-relational mapper for .NET",
              "privateDescription": "Learning basics, completed tutorials",
              "progress": 45,
              "status": "Active",
              "isCustom": false,
              "isStart": false,
              "aiReasoning": "Essential ORM for .NET backend development",
              "createdAt": "2025-11-16T14:20:00Z",
              "updatedAt": "2025-11-28T16:45:00Z"
            }
          ],
          "count": 2
        }
        ```

        **Response 401:** (Standard unauthorized response)

        ---

        #### GET /api/technologies/{id}
        **Description:** Retrieves single technology by ID  
        **Authentication:** Required

        **Response 200:**
        ```json
        {
          "id": 1002,
          "userId": 123,
          "technologyDefinitionId": 15,
          "name": "DotNet - Entity Framework",
          "prefix": "DotNet",
          "tag": "Framework",
          "systemDescription": "Object-relational mapper for .NET",
          "privateDescription": "Learning basics, completed tutorials",
          "progress": 45,
          "status": "Active",
          "isCustom": false,
          "isStart": false,
          "aiReasoning": "Essential ORM for .NET backend development",
          "createdAt": "2025-11-16T14:20:00Z",
          "updatedAt": "2025-11-28T16:45:00Z"
        }
        ```

        **Response 404:**
        ```json
        {
          "error": "NotFound",
          "message": "Technology not found"
        }
        ```

        **Response 403:**
        ```json
        {
          "error": "Forbidden",
          "message": "This technology belongs to another user"
        }
        ```

        ---

        #### POST /api/technologies
        **Description:** Adds single technology to user's graph  
        **Authentication:** Required

        **Request Body:**
        ```json
        {
          "technologyDefinitionId": 15,
          "fromTechnologyId": 1001,
          "privateDescription": "Want to learn this for my project",
          "isCustom": false
        }
        ```

        **Validation Rules:**
        - `technologyDefinitionId`: Required, must exist in TechnologyDefinitions
        - `fromTechnologyId`: Required, must be user's existing technology
        - `privateDescription`: Optional, max 2000 characters
        - `isCustom`: Optional, boolean (default: false)

        **Response 201:**
        ```json
        {
          "technology": {
            "id": 1002,
            "userId": 123,
            "technologyDefinitionId": 15,
            "name": "DotNet - Entity Framework",
            "prefix": "DotNet",
            "tag": "Framework",
            "systemDescription": "Object-relational mapper for .NET",
            "privateDescription": "Want to learn this for my project",
            "progress": 0,
            "status": "Active",
            "isCustom": false,
            "isStart": false,
            "aiReasoning": null,
            "createdAt": "2025-12-01T10:15:00Z",
            "updatedAt": null
          },
          "dependency": {
            "id": 5001,
            "userId": 123,
            "fromTechnologyId": 1001,
            "toTechnologyId": 1002,
            "createdAt": "2025-12-01T10:15:00Z"
          }
        }
        ```

        **Response 400:**
        ```json
        {
          "error": "ValidationError",
          "message": "Invalid request",
          "errors": [
            {
              "field": "technologyDefinitionId",
              "message": "Technology definition does not exist"
            }
          ]
        }
        ```

        **Response 409:**
        ```json
        {
          "error": "Conflict",
          "message": "Dependency already exists in graph"
        }
        ```

        ---

        #### POST /api/technologies/batch
        **Description:** Adds multiple technologies to user's graph at once (for selecting multiple AI recommendations)  
        **Authentication:** Required

        **Request Body:**
        ```json
        {
          "fromTechnologyId": 1001,
          "technologies": [
            {
              "technologyDefinitionId": 15,
              "privateDescription": "Essential for my project"
            },
            {
              "technologyDefinitionId": 23,
              "privateDescription": null
            }
          ]
        }
        ```

        **Response 201:**
        ```json
        {
          "added": [
            {
              "technology": {
                "id": 1002,
                "name": "DotNet - Entity Framework",
                "progress": 0
              },
              "dependency": {
                "id": 5001,
                "fromTechnologyId": 1001,
                "toTechnologyId": 1002
              }
            },
            {
              "technology": {
                "id": 1003,
                "name": "PostgreSQL - Advanced Queries",
                "progress": 0
              },
              "dependency": {
                "id": 5002,
                "fromTechnologyId": 1001,
                "toTechnologyId": 1003
              }
            }
          ],
          "count": 2
        }
        ```

        **Response 207 (Multi-Status):**
        ```json
        {
          "results": [
            {
              "technologyDefinitionId": 15,
              "status": "success",
              "technologyId": 1002
            },
            {
              "technologyDefinitionId": 23,
              "status": "error",
              "error": "Dependency already exists"
            }
          ],
          "successCount": 1,
          "errorCount": 1
        }
        ```

        ---

        #### POST /api/technologies/custom
        **Description:** Creates custom technology (user-defined, not from AI)  
        **Authentication:** Required

        **Request Body:**
        ```json
        {
          "name": "My Custom Framework",
          "prefix": "JavaScript",
          "tag": "Framework",
          "systemDescription": "A framework I'm building",
          "fromTechnologyId": 1001,
          "privateDescription": "My own project"
        }
        ```

        **Validation Rules:**
        - `name`: Required, max 255 characters
        - `prefix`: Required, max 100 characters
        - `tag`: Required, Enum - "Technologia" | "Framework" | "BazaDanych" | "Metodologia" | "Narzedzie"
        - `systemDescription`: Required, max 1000 characters
        - `fromTechnologyId`: Required
        - `privateDescription`: Optional, max 2000 characters

        **Response 201:**
        ```json
        {
          "technologyDefinition": {
            "id": 789,
            "name": "My Custom Framework",
            "prefix": "JavaScript",
            "tag": "Framework",
            "systemDescription": "A framework I'm building",
            "createdAt": "2025-12-01T11:30:00Z"
          },
          "technology": {
            "id": 1005,
            "userId": 123,
            "technologyDefinitionId": 789,
            "name": "My Custom Framework",
            "progress": 0,
            "isCustom": true,
            "isStart": false,
            "createdAt": "2025-12-01T11:30:00Z"
          },
          "dependency": {
            "id": 5005,
            "fromTechnologyId": 1001,
            "toTechnologyId": 1005
          }
        }
        ```

        ---

        #### PATCH /api/technologies/{id}
        **Description:** Updates technology progress or private description  
        **Authentication:** Required

        **Request Body:**
        ```json
        {
          "progress": 75,
          "privateDescription": "Completed advanced course, working on project"
        }
        ```

        **Validation Rules:**
        - `progress`: Optional, integer 0-100
        - `privateDescription`: Optional, max 2000 characters, can be null

        **Response 200:**
        ```json
        {
          "id": 1002,
          "progress": 75,
          "privateDescription": "Completed advanced course, working on project",
          "updatedAt": "2025-12-01T12:45:00Z"
        }
        ```

        **Response 400:**
        ```json
        {
          "error": "ValidationError",
          "message": "Progress must be between 0 and 100"
        }
        ```

        **Response 404:** (Standard not found)

        ---

        #### DELETE /api/technologies/{id}
        **Description:** Deletes technology from user's graph. Cannot delete Start node. Deletes all related dependencies.  
        **Authentication:** Required

        **Response 200:**
        ```json
        {
          "message": "Technology deleted successfully",
          "deletedDependencies": 3
        }
        ```

        **Response 403:**
        ```json
        {
          "error": "Forbidden",
          "message": "Cannot delete Start node"
        }
        ```

        **Response 404:** (Standard not found)

        ---

        #### GET /api/technologies/graph
        **Description:** Retrieves complete technology graph for current user (nodes + edges)  
        **Authentication:** Required

        **Response 200:**
        ```json
        {
          "nodes": [
            {
              "id": 1001,
              "name": "Start",
              "prefix": "System",
              "tag": "Metodologia",
              "progress": 100,
              "isStart": true,
              "systemDescription": "Starting point for your technology journey"
            },
            {
              "id": 1002,
              "name": "DotNet - Entity Framework",
              "prefix": "DotNet",
              "tag": "Framework",
              "progress": 45,
              "isStart": false,
              "systemDescription": "Object-relational mapper for .NET"
            },
            {
              "id": 1003,
              "name": "PostgreSQL - Advanced Queries",
              "prefix": "PostgreSQL",
              "tag": "BazaDanych",
              "progress": 20,
              "isStart": false,
              "systemDescription": "Advanced SQL querying techniques"
            }
          ],
          "edges": [
            {
              "id": 5001,
              "from": 1001,
              "to": 1002,
              "createdAt": "2025-11-16T14:20:00Z"
            },
            {
              "id": 5002,
              "from": 1001,
              "to": 1003,
              "createdAt": "2025-11-17T09:15:00Z"
            },
            {
              "id": 5003,
              "from": 1002,
              "to": 1004,
              "createdAt": "2025-11-20T16:30:00Z"
            }
          ],
          "stats": {
            "totalNodes": 3,
            "totalEdges": 3,
            "averageProgress": 55,
            "completedCount": 1
          }
        }
        ```

        ---

        ### 2.4 Dependencies (Graph Edges)

        #### POST /api/technologies/{id}/dependencies
        **Description:** Creates new dependency (edge) from specified technology to another  
        **Authentication:** Required

        **Request Body:**
        ```json
        {
          "toTechnologyId": 1005
        }
        ```

        **Response 201:**
        ```json
        {
          "id": 5010,
          "userId": 123,
          "fromTechnologyId": 1002,
          "toTechnologyId": 1005,
          "createdAt": "2025-12-01T13:20:00Z"
        }
        ```

        **Response 400:**
        ```json
        {
          "error": "ValidationError",
          "message": "Target technology does not exist"
        }
        ```

        **Response 409:**
        ```json
        {
          "error": "Conflict",
          "message": "Dependency already exists"
        }
        ```

        ---

        #### DELETE /api/technologies/{id}/dependencies/{dependencyId}
        **Description:** Removes dependency (edge) from graph  
        **Authentication:** Required

        **Response 200:**
        ```json
        {
          "message": "Dependency deleted successfully"
        }
        ```

        **Response 404:**
        ```json
        {
          "error": "NotFound",
          "message": "Dependency not found"
        }
        ```

        ---

        ### 2.5 AI Recommendations

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

        ---

        ### 2.6 Ignored Technologies

        #### GET /api/ignored-technologies
        **Description:** Retrieves list of all technologies ignored by current user  
        **Authentication:** Required  
        **Query Parameters:**
        - `limit` (optional): integer, max 100 (default: 50)
        - `offset` (optional): integer (default: 0)

        **Response 200:**
        ```json
        {
          "ignoredTechnologies": [
            {
              "id": 3001,
              "userId": 123,
              "name": "Java - Spring Boot",
              "category": "Java",
              "tag": "Framework",
              "systemDescription": "Popular Java framework for building enterprise applications",
              "aiReasoning": "Commonly used with Java backend development",
              "contextTechnologyId": 1002,
              "contextTechnologyName": "DotNet - Entity Framework",
              "ignoredAt": "2025-11-18T10:20:00Z"
            },
            {
              "id": 3002,
              "userId": 123,
              "name": "Ruby on Rails",
              "category": "Ruby",
              "tag": "Framework",
              "systemDescription": "Web application framework written in Ruby",
              "aiReasoning": "Alternative backend framework",
              "contextTechnologyId": 1001,
              "contextTechnologyName": "Start",
              "ignoredAt": "2025-11-16T15:45:00Z"
            }
          ],
          "count": 2,
          "total": 2,
          "limit": 50,
          "offset": 0
        }
        ```

        ---

        #### POST /api/ignored-technologies
        **Description:** Adds technology to ignore list (single or batch)  
        **Authentication:** Required

        **Request Body:**
        ```json
        {
          "technologies": [
            {
              "name": "Java - Spring Boot",
              "category": "Java",
              "tag": "Framework",
              "systemDescription": "Popular Java framework for building enterprise applications",
              "aiReasoning": "Commonly used with Java backend development",
              "contextTechnologyId": 1002
            }
          ]
        }
        ```

        **Validation Rules:**
        - `technologies`: Array, minimum 1 item, maximum 20 items
        - `name`: Required, max 255 characters
        - `category`: Required, max 100 characters
        - `tag`: Required, Enum
        - `systemDescription`: Required, max 1000 characters
        - `aiReasoning`: Optional, max 1000 characters
        - `contextTechnologyId`: Optional, must be user's technology

        **Response 201:**
        ```json
        {
          "added": [
            {
              "id": 3001,
              "name": "Java - Spring Boot",
              "category": "Java",
              "ignoredAt": "2025-12-01T15:10:00Z"
            }
          ],
          "count": 1
        }
        ```

        **Response 409:**
        ```json
        {
          "error": "Conflict",
          "message": "Technology 'Java - Spring Boot' is already ignored"
        }
        ```

        ---

        #### DELETE /api/ignored-technologies/{id}
        **Description:** Removes technology from ignore list (restores it for future recommendations)  
        **Authentication:** Required

        **Response 200:**
        ```json
        {
          "message": "Technology restored successfully",
          "id": 3001,
          "name": "Java - Spring Boot"
        }
        ```

        **Response 404:**
        ```json
        {
          "error": "NotFound",
          "message": "Ignored technology not found"
        }
        ```

        ---

        #### DELETE /api/ignored-technologies/batch
        **Description:** Removes multiple technologies from ignore list at once  
        **Authentication:** Required

        **Request Body:**
        ```json
        {
          "ids": [3001, 3002, 3005]
        }
        ```

        **Response 200:**
        ```json
        {
          "message": "Technologies restored successfully",
          "restoredCount": 3,
          "restored": [
            {
              "id": 3001,
              "name": "Java - Spring Boot"
            },
            {
              "id": 3002,
              "name": "Ruby on Rails"
            },
            {
              "id": 3005,
              "name": "PHP - Laravel"
            }
          ]
        }
        ```

        **Response 207:**
        ```json
        {
          "results": [
            {
              "id": 3001,
              "status": "success"
            },
            {
              "id": 3002,
              "status": "success"
            },
            {
              "id": 3005,
              "status": "error",
              "error": "Not found"
            }
          ],
          "restoredCount": 2
        }
        ```

        ---

        ### 2.7 Admin Endpoints

        All admin endpoints require authenticated user with GoogleId in admin whitelist (configured in appsettings.json).

        #### GET /api/admin/metrics
        **Description:** Retrieves dashboard metrics for MVP KPIs  
        **Authentication:** Required (Admin only)

        **Response 200:**
        ```json
        {
          "kpis": {
            "profileCompletionRate": 92.5,
            "profileCompletionTarget": 90,
            "profileCompletionMet": true,
            "technologyGenerationRate": 78.3,
            "technologyGenerationTarget": 75,
            "technologyGenerationMet": true
          },
          "users": {
            "total": 1247,
            "withProfile": 1154,
            "withoutProfile": 93,
            "activeLastWeek": 856,
            "activeLastMonth": 1089
          },
          "technologies": {
            "totalGenerated": 15678,
            "totalInGraphs": 12345,
            "totalIgnored": 3333,
            "averagePerUser": 9.9,
            "averageProgress": 42.7,
            "completedCount": 3456
          },
          "ai": {
            "totalRequests": 8934,
            "cacheHits": 2145,
            "cacheHitRate": 24.0,
            "averageResponseTime": 4.2,
            "timeouts": 23,
            "errors": 12
          },
          "generatedAt": "2025-12-01T16:00:00Z"
        }
        ```

        **Response 403:**
        ```json
        {
          "error": "Forbidden",
          "message": "Admin access required"
        }
        ```

        ---

        #### GET /api/admin/users
        **Description:** Retrieves paginated list of users with basic stats  
        **Authentication:** Required (Admin only)  
        **Query Parameters:**
        - `limit` (optional): integer, max 100 (default: 50)
        - `offset` (optional): integer (default: 0)
        - `sortBy` (optional): "createdAt" | "lastLoginAt" | "email" (default: "createdAt")
        - `order` (optional): "asc" | "desc" (default: "desc")

        **Response 200:**
        ```json
        {
          "users": [
            {
              "id": 123,
              "email": "user@example.com",
              "name": "John Doe",
              "hasProfile": true,
              "technologiesCount": 12,
              "averageProgress": 56,
              "createdAt": "2025-11-15T10:30:00Z",
              "lastLoginAt": "2025-12-01T08:15:00Z"
            }
          ],
          "count": 50,
          "total": 1247,
          "limit": 50,
          "offset": 0
        }
        ```

        ---

        #### GET /api/admin/technologies/popular
        **Description:** Retrieves most popular technology prefixes/categories  
        **Authentication:** Required (Admin only)  
        **Query Parameters:**
        - `limit` (optional): integer, max 50 (default: 20)
        - `groupBy` (optional): "prefix" | "tag" (default: "prefix")

        **Response 200:**
        ```json
        {
          "popularPrefixes": [
            {
              "prefix": "DotNet",
              "count": 3456,
              "percentage": 28.0,
              "uniqueUsers": 892
            },
            {
              "prefix": "JavaScript",
              "count": 2987,
              "percentage": 24.2,
              "uniqueUsers": 845
            },
            {
              "prefix": "PostgreSQL",
              "count": 1876,
              "percentage": 15.2,
              "uniqueUsers": 678
            }
          ],
          "total": 12345,
          "generatedAt": "2025-12-01T16:15:00Z"
        }
        ```

        ---

        #### GET /api/admin/statistics/profiles
        **Description:** Retrieves profile statistics breakdown  
        **Authentication:** Required (Admin only)

        **Response 200:**
        ```json
        {
          "roles": {
            "Programista": {
              "count": 890,
              "percentage": 71.4
            },
            "Tester": {
              "count": 234,
              "percentage": 18.8
            },
            "Analityk": {
              "count": 89,
              "percentage": 7.1
            },
            "DataScienceSpecialist": {
              "count": 34,
              "percentage": 2.7
            }
          },
          "developmentAreas": {
            "Backend": {
              "count": 567,
              "percentage": 45.5
            },
            "FullStack": {
              "count": 345,
              "percentage": 27.7
            },
            "UserInterface": {
              "count": 234,
              "percentage": 18.8
            },
            "Testing": {
              "count": 67,
              "percentage": 5.4
            },
            "DataScience": {
              "count": 23,
              "percentage": 1.8
            },
            "DevOps": {
              "count": 11,
              "percentage": 0.9
            }
          },
          "totalProfiles": 1247,
          "generatedAt": "2025-12-01T16:20:00Z"
        }
        ```



</endpoint_description>

6. Type Definitions:
<type_definitions>
@Types.cs
</type_definitions>

7. Tech Stack:
<tech_stack>
@tech-stack.md
</tech_stack>

Przed utworzeniem ostatecznego planu wdrożenia przeprowadź analizę i planowanie wewnątrz tagów <implementation_breakdown> w swoim bloku myślenia. Ta sekcja może być dość długa, ponieważ ważne jest, aby być dokładnym.

W swoim podziale implementacji wykonaj następujące kroki:
1. Dla każdej sekcji wejściowej (PRD, User Stories, Endpoint Description, Endpoint Implementation, Type Definitions, Tech Stack):
  - Podsumuj kluczowe punkty
 - Wymień wszelkie wymagania lub ograniczenia
 - Zwróć uwagę na wszelkie potencjalne wyzwania lub ważne kwestie
2. Wyodrębnienie i wypisanie kluczowych wymagań z PRD
3. Wypisanie wszystkich potrzebnych głównych komponentów, wraz z krótkim opisem ich opisu, potrzebnych typów, obsługiwanych zdarzeń i warunków walidacji
4. Stworzenie wysokopoziomowego diagramu drzewa komponentów
5. Zidentyfikuj wymagane DTO i niestandardowe typy ViewModel dla każdego komponentu widoku. Szczegółowo wyjaśnij te nowe typy, dzieląc ich pola i powiązane typy.
6. Zidentyfikuj potencjalne zmienne stanu i niestandardowe hooki, wyjaśniając ich cel i sposób ich użycia
7. Wymień wymagane wywołania API i odpowiadające im akcje frontendowe
8. Zmapuj każdej historii użytkownika do konkretnych szczegółów implementacji, komponentów lub funkcji
9. Wymień interakcje użytkownika i ich oczekiwane wyniki
10. Wymień warunki wymagane przez API i jak je weryfikować na poziomie komponentów
11. Zidentyfikuj potencjalne scenariusze błędów i zasugeruj, jak sobie z nimi poradzić
12. Wymień potencjalne wyzwania związane z wdrożeniem tego widoku i zasugeruj możliwe rozwiązania

Po przeprowadzeniu analizy dostarcz plan wdrożenia w formacie Markdown z następującymi sekcjami:

1. Przegląd: Krótkie podsumowanie widoku i jego celu.
2. Routing widoku: Określenie ścieżki, na której widok powinien być dostępny.
3. Struktura komponentów: Zarys głównych komponentów i ich hierarchii.
4. Szczegóły komponentu: Dla każdego komponentu należy opisać:
 - Opis komponentu, jego przeznaczenie i z czego się składa
 - Główne elementy HTML i komponenty dzieci, które budują komponent
 - Obsługiwane zdarzenia
 - Warunki walidacji (szczegółowe warunki, zgodnie z API)
 - Typy (DTO i ViewModel) wymagane przez komponent
 - Propsy, które komponent przyjmuje od rodzica (interfejs komponentu)
5. Typy: Szczegółowy opis typów wymaganych do implementacji widoku, w tym dokładny podział wszelkich nowych typów lub modeli widoku według pól i typów.
6. Zarządzanie stanem: Szczegółowy opis sposobu zarządzania stanem w widoku, określenie, czy wymagany jest customowy hook.
7. Integracja API: Wyjaśnienie sposobu integracji z dostarczonym punktem końcowym. Precyzyjnie wskazuje typy żądania i odpowiedzi.
8. Interakcje użytkownika: Szczegółowy opis interakcji użytkownika i sposobu ich obsługi.
9. Warunki i walidacja: Opisz jakie warunki są weryfikowane przez interfejs, których komponentów dotyczą i jak wpływają one na stan interfejsu
10. Obsługa błędów: Opis sposobu obsługi potencjalnych błędów lub przypadków brzegowych.
11. Kroki implementacji: Przewodnik krok po kroku dotyczący implementacji widoku.

Upewnij się, że Twój plan jest zgodny z PRD, historyjkami użytkownika i uwzględnia dostarczony stack technologiczny.

Ostateczne wyniki powinny być w języku polskim i zapisane w pliku o nazwie .ai/all-view-implementation-plan.md. Nie uwzględniaj żadnej analizy i planowania w końcowym wyniku.

Oto przykład tego, jak powinien wyglądać plik wyjściowy (treść jest do zastąpienia):

```markdown
# Plan implementacji widoku [Nazwa widoku]

## 1. Przegląd
[Krótki opis widoku i jego celu]

## 2. Routing widoku
[Ścieżka, na której widok powinien być dostępny]

## 3. Struktura komponentów
[Zarys głównych komponentów i ich hierarchii]

## 4. Szczegóły komponentów
### [Nazwa komponentu 1]
- Opis komponentu [opis]
- Główne elementy: [opis]
- Obsługiwane interakcje: [lista]
- Obsługiwana walidacja: [lista, szczegółowa]
- Typy: [lista]
- Propsy: [lista]

### [Nazwa komponentu 2]
[...]

## 5. Typy
[Szczegółowy opis wymaganych typów]

## 6. Zarządzanie stanem
[Opis zarządzania stanem w widoku]

## 7. Integracja API
[Wyjaśnienie integracji z dostarczonym endpointem, wskazanie typów żądania i odpowiedzi]

## 8. Interakcje użytkownika
[Szczegółowy opis interakcji użytkownika]

## 9. Warunki i walidacja
[Szczegółowy opis warunków i ich walidacji]

## 10. Obsługa błędów
[Opis obsługi potencjalnych błędów]

## 11. Kroki implementacji
1. [Krok 1]
2. [Krok 2]
3. [...]
```

Rozpocznij analizę i planowanie już teraz. Twój ostateczny wynik powinien składać się wyłącznie z planu wdrożenia w języku polskim w formacie markdown, który zapiszesz w pliku .ai/all-view-implementation-plan.md i nie powinien powielać ani powtarzać żadnej pracy wykonanej w podziale implementacji.

---

**Cursor**

<implementation_breakdown>
**1. Podsumowanie i analiza danych wejściowych:**

*   **PRD & User Stories:** Aplikacja to "KrisBraveApp" - narzędzie do planowania ścieżki rozwoju technologicznego (drzewo/graf umiejętności) z rekomendacjami AI.
*   **Tech Stack:** .NET 9, Blazor (Web App), Tailwind CSS, Flowbite. Baza: PostgreSQL/SQL Server (EF Core).
*   **Istniejący stan:** Projekt istnieje, ma podstawową strukturę Blazor (`Components/Pages`), kontrolery (`AuthController`, `ProfileController`, `AIRecommendationsController`), modele bazodanowe.
*   **Braki:**
    *   Brak kontrolerów do zarządzania samym grafem technologii (`TechnologyController`) oraz ignorowanymi technologiami (`IgnoredTechnologyController`).
    *   Widoki są szczątkowe (`Home`, `Login`, `Profile`).
    *   Brak logiki wizualizacji grafu.

**2. Kluczowe wymagania z PRD (Frontend):**
*   **Rejestracja/Logowanie (US-001):** Google OAuth. Przycisk, przekierowanie, obsługa powrotu.
*   **Onboarding (US-002):** Formularz profilu (wymuszony przy pierwszym wejściu). Min. 5 technologii, rola, obszar.
*   **Graf Technologii (US-004, US-015, US-016):** Interaktywny graf (węzły, krawędzie). Zoom, Pan, Click.
    *   *Decyzja techniczna:* Użycie biblioteki JS (np. Cytoscape.js) przez JS Interop w Blazorze dla płynności interakcji (drag & drop, zoom). Czysty C#/SVG może być za wolny/trudny w obsłudze złożonych layoutów.
*   **Popup Opcji (US-011, US-012, US-013):** Edycja postępu, notatki prywatne, usuwanie.
*   **Popup Rekomendacji (US-005, US-006, US-019):** Wywołanie AI (spinner), lista checkboxów, dodawanie do grafu, ignorowanie.
*   **Custom Tech (US-022):** Formularz dodawania własnej technologii.

**3. Główne Komponenty i Hierarchia:**
*   `MainLayout`: Navbar (Logout, Linki).
*   `Pages/Home.razor`: Landing page (dla niezalogowanych).
*   `Pages/Dashboard.razor`: Główny widok z grafem (Protected).
    *   `GraphVisualizer.razor`: Wrapper na bibliotekę graficzną.
    *   `NodeOptionsModal.razor`: Edycja węzła.
    *   `RecommendationsModal.razor`: Interakcja z AI.
        *   `RecommendationList.razor`: Lista wyników.
        *   `IgnoredList.razor`: Lista ignorowanych.
*   `Pages/Onboarding.razor`: Wymuszony redirect jeśli brak profilu.
    *   `ProfileForm.razor`: Współdzielony formularz (dla onboardingu i edycji).
*   `Pages/Profile.razor`: Edycja profilu (Protected).
    *   `ProfileForm.razor` (Reuse).

**4. Analiza brakujących Endpointów (Backend):**
W dokumentacji brakuje definicji endpointów dla operacji CRUD na węzłach grafu (poza AI) oraz zarządzania ignorowanymi.
Muszę zdefiniować (w planie):
*   `TechnologyController`: GET /graph, POST (custom), DELETE, PATCH (progress/desc).
*   `IgnoredTechnologyController`: GET, POST, DELETE.

**5. Typy DTO (C# -> Blazor):**
*   `UserProfileDto`: Dane profilu.
*   `GraphNodeDto`, `GraphEdgeDto`: Struktura grafu.
*   `RecommendationDto`: Wynik z AI.
*   `TechnologyDetailsDto`: Pełne dane węzła (systemowe + prywatne).

**6. Zarządzanie stanem:**
Blazor jest stanowy.
*   `UserState`: CascadingParameter lub serwis scoped trzymający info o zalogowanym użytkowniku i czy ma profil.
*   Lokalny stan w `Dashboard.razor`: Lista węzłów, aktualnie wybrany węzeł, widoczność modali.

**7. Wyzwania:**
*   **JS Interop:** Synchronizacja stanu grafu (JS) ze stanem Blazora (C#).
*   **Responsywność:** Graf na mobile (US-021).
*   **Asynchroniczność:** Długie zapytanie do AI (20s timeout). Obsługa Loadera.

</implementation_breakdown>

I will now verify that the target directory `.ai` exists. It does (from file list).
I will generate the plan.



Plan wdrożenia został pomyślnie wygenerowany i zapisany w pliku `.ai/all-view-implementation-plan.md`.

Plan zawiera:
1.  **Analizę routingu** i struktury widoków.
2.  **Szczegółowy opis komponentów** (w tym podział na Smart Components i komponenty prezentacyjne).
3.  **Definicje brakujących endpointów** (Backend), które należy zaimplementować, aby obsłużyć graf technologii i listę ignorowanych elementów.
4.  **Modele DTO** wymagane po stronie klienta.
5.  **Logikę zarządzania stanem** (Auth + Graph State).
6.  **Scenariusze interakcji** z systemem rekomendacji AI (w tym obsługę błędów i timeoutów).

Możesz teraz przejść do implementacji zgodnie z krokami opisanymi w sekcji 11 wygenerowanego pliku.

