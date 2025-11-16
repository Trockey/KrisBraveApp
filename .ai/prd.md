# Dokument wymagań produktu (PRD) - DeveloperGoals

Wersja: 1.0  
Data: 16 listopada 2025  
Status: DRAFT - MVP  
Autor: Product Manager  

---

## 1. Przegląd produktu

### 1.1 Nazwa produktu
DeveloperGoals - Inteligentna aplikacja do planowania ścieżki rozwoju programistów

### 1.2 Wizja produktu
DeveloperGoals to aplikacja webowa, która wykorzystuje sztuczną inteligencję do rekomendowania kolejnych technologii, frameworków i narzędzi do nauki, bazując na aktualnej wiedzy użytkownika, jego specjalizacji i trendach rynkowych. Aplikacja pomaga programistom na każdym poziomie zaawansowania planować swoją ścieżkę rozwoju zawodowego poprzez intuicyjną wizualizację w postaci grafu technologii.

### 1.3 Unikalna wartość
- Prostota obsługi i intuicyjny interfejs użytkownika
- Łatwe zarządzanie celami naukowymi
- Czytelna wizualizacja ścieżek rozwoju poprzez graf technologii
- Personalizowane rekomendacje oparte na AI, dostosowane do poziomu i specjalizacji użytkownika
- Brak konieczności bezpośredniej interakcji z AI - system działa automatycznie
- Respektowanie specjalizacji użytkownika (np. developer .NET nie otrzyma propozycji Java)

### 1.4 Grupa docelowa
Aplikacja jest przeznaczona dla programistów na wszystkich poziomach zaawansowania:
- Junior Developers - potrzebują kierunku rozwoju i struktury nauki
- Mid-level Developers - chcą poszerzać swoją wiedzę w określonym kierunku
- Senior Developers - szukają nowych trendów i zaawansowanych technologii

### 1.5 Stack technologiczny
- Frontend/Backend: ASP.NET Core z wykorzystaniem jednej z technologii Microsoft (MVC, Razor Pages lub Blazor)
- Autentykacja: Google OAuth
- Baza danych: Entity Framework (SQL Server lub PostgreSQL)
- AI Integration: API do modelu AI (standardowe ceny)
- Wizualizacja grafów: Biblioteka kompatybilna z wybraną technologią Microsoft (vis.js, cytoscape.js lub podobna)

### 1.6 Harmonogram i zasoby
- Timeline: 5 tygodni do wersji BETA
- Zespół: 1 developer (solo)
- Budżet AI: standardowe ceny (nie premium, nie freemium)
- Priorytet: szybki prototyp, sprawdzone technologie, minimum własnego kodu

---

## 2. Problem użytkownika

### 2.1 Główny problem
Programiści muszą być na bieżąco z najnowszymi technologiami, aby pozostać konkurencyjnymi na rynku pracy. Jednakże, napotykają na następujące wyzwania:

- Nie wiedzą w którym kierunku się rozwijać
- Brak wiedzy o tym, jakie technologie pasują do ich obecnej wiedzy i doświadczenia
- Trudność w identyfikacji trendów rynkowych i najważniejszych umiejętności
- Chaos informacyjny - zbyt wiele opcji i źródeł wiedzy
- Brak struktury i planu nauki

### 2.2 Obecne rozwiązania i ich ograniczenia
- Manualny research - czasochłonny i nieefektywny
- Roadmapy technologiczne - zbyt ogólne, nie uwzględniają indywidualnej wiedzy
- Kursy online - pokazują co można się nauczyć, ale nie sugerują kolejnych kroków
- Porady mentorów - nie zawsze dostępne, subiektywne

### 2.3 Proponowane rozwiązanie
DeveloperGoals oferuje spersonalizowany system rekomendacji oparty na AI, który:
- Analizuje aktualną wiedzę i doświadczenie użytkownika
- Śledzi trendy rynkowe, GitHub Trends i ogłoszenia o pracę
- Generuje rekomendacje kolejnych technologii do nauki
- Wizualizuje ścieżkę rozwoju w postaci grafu zależności
- Pozwala śledzić postęp w nauce
- Respektuje specjalizację i preferencje użytkownika

---

## 3. Wymagania funkcjonalne

### 3.1 System Profilu Użytkownika

#### 3.1.1 Dane profilu
Profil użytkownika zawiera następujące dane:
- Główne technologie (Java, .NET, JavaScript, Python, itd.)
- Rola: Programista / Tester / Analityk / Data Science Specialist itp.
- Obszar rozwoju: User Interface / Backend / Full Stack / Testing

#### 3.1.2 Zarządzanie profilem
- Użytkownik może edytować profil w dowolnym momencie
- Cały profil musi być wypełniony przed rozpoczęciem szukania technologii
- Zmiana profilu NIE powoduje automatycznej regeneracji grafu technologii
- Maksymalny czas wypełnienia profilu: 3 minuty

#### 3.1.3 Onboarding
- Prosty proces wypełniania profilu z predefiniowanymi opcjami (radiobuttons lub dropdown)
- Minimum 5 znanych technologii do wyboru w procesie onboardingu
- Po wypełnieniu profilu użytkownik przechodzi bezpośrednio do grafu technologii

### 3.2 System Technologii

#### 3.2.1 Model danych technologii
Każda technologia zawiera:
- Nazwa z prefiksem linii rozwoju (np. "DotNet - Entity Framework")
- kategoria główna (opisywana prefixem w nazwie): DotNet, Java, JavaScript, Python, Database, DevOps, Cloud, Virtualization, Data Science, Tools
- Tag technologii: Technologia / Framework / Baza danych / Metodologia
- Opis systemowy (krótki, generowany przez AI)
- Opis prywatny (edytowalny przez użytkownika, widoczny tylko dla niego)
- Status: Active / Ignored
- Procent postępu (0-100%, edytowalny suwakiem)

#### 3.2.2 Źródła technologii
- AI generuje technologie dynamicznie na podstawie profilu użytkownika
- Użytkownik może dodać własne niestandardowe technologie
- Użytkownik może wpisać technologie w Profilu, które już zna

#### 3.2.3 Prezentacja technologii
- Technologie prezentowane są w formie grafu z zależnościami
- Brak domyślnego sortowania grafu
- Każda technologia na grafie jest klikalna - posiada dwa przycisku, każdy uruchamia inne wyskakujące okno

### 3.3 Mechanizm Rekomendacji AI

#### 3.3.1 Trigger generowania
- Użytkownik klika przycisk "Szukaj nowych technologii" przy wybranej technologii na grafie
- Pojawia się popup z rekomendacjami (Popup 2)
- Rekomendacje są generowane na żądanie użytkownika
- Brak limitów anti-spam dla generowania rekomendacji

#### 3.3.2 Input dla AI
System przekazuje do AI:
- Pełny profil użytkownika (główne technologie, rola, obszar)
- Wszystkie znane technologie użytkownika z grafu (bez ignorowanych)
- Kontekst: wybrana technologia, od której ma szukać kolejnych
- Akcent na wybraną technologię, na której użytkownik kliknął "Szukaj nowych technologii"

#### 3.3.3 Output AI
AI zwraca:
- Lista około 10 rekomendowanych technologii
- Dla każdej technologii: nazwa, prefix, tag, opis, reasoning (dlaczego polecona)
- Format: JSON

#### 3.3.4 Źródła danych AI
AI bazuje na jednym źródle danych lub więcej, spośród
- Trendach rynkowych
- GitHub Trends
- Ogłoszeniach o pracę
- StackOverflow API
- Innych źródłach zgodnie z implementacją AI

#### 3.3.5 Parametry techniczne
- Timeout: maksymalnie 20 sekund na generowanie
- UI: Spinner/loader podczas ładowania
- Komunikat błędu po przekroczeniu timeoutu
- Brak limitów użycia (no anti-spam)
- Brak bezpośredniej interakcji użytkownika z AI (brak chatu)

#### 3.3.6 Specjalizacja użytkownika
- System respektuje specjalizację użytkownika zapisaną w Profilu
- Przykład: developer .NET nie dostaje propozycji technologii Java
- AI bierze pod uwagę cały profil przy generowaniu rekomendacji

### 3.4 Graf Technologii (Główny Widok)

#### 3.4.1 Typ wizualizacji
- Statyczny graf z klikalną interakcją
- Implementacja przy użyciu biblioteki do wizualizacji grafów

#### 3.4.2 Struktura grafu
- Graf pokazuje zależności między technologiami, które użytkownik już wybrał
- Graf użytkownika jest jeden i zaczyna się od węzła "Start" (domyślny, pierwszy węzeł)
- Graf jest budowany stopniowo przez użytkownika poprzez dodawanie technologii
- Aplikacja przechowuje zależności między dodanymi technologiami w kolejności dodawania
- Każdy użytkownik ma swoją własną listę zależności technologii
- Lista zależności technologii NIE jest współdzielona między użytkownikami

#### 3.4.3 Interakcja z grafem
Kliknięcie węzła technologii na grafie:
- Przycisk "Opcje" → otwiera Popup 1 z opcjami:
  - Edycja prywatnego opisu (description)
  - Zmiana postępu nauki (suwak 0-100%)
  - Usunięcie technologii z grafu
  
- Przycisk "Szukaj nowych technologii" → otwiera Popup 2 zawierający:
  - Listę proponowanych technologii przez AI
  - Listę proponowanych technologii, które użytkownik wcześniej zignorował

#### 3.4.4 Workflow dodawania technologii
- Użytkownik wskazuje interesujące technologie → technologia dodaje się do grafu
- Nieinteresujące technologie → użytkownik dodaje do listy "Ignore"
- Po wybraniu technologii X i kliknięciu "Szukaj nowych technologii", system generuje około 10 kolejnych powiązanych technologii
- Użytkownik może generować kolejne sugestie nawet jeśli technologia nie jest oznaczona 100% postępu

### 3.5 Learning Dashboard

#### 3.5.1 Główny ekran
- Diagram jako graf technologii (główny widok)
- Dla każdej technologii wyświetlane są: nazwa, prefix, tag, procent postępu

#### 3.5.2 Lista "Ignorowane"
- Oddzielna sekcja na Popup 2, poniżej listy z proponowanymi technologiami
- Lista jest przeglądalna i edytowalna
- Użytkownik może przywrócić technologie z tej listy, aby je ponownie aktywować
- Ignorowane technologie pojawiają się na osobnej liście propozycji

#### 3.5.3 Zarządzanie postępem
- Postęp nauki oznaczany jest suwakiem procentowym (0-100%)
- Aktualizacja postępu odbywa się przez Popup 1 (kliknięcie węzła → "Opcje")
- Użytkownik może śledzić swój postęp wizualnie na grafie

#### 3.5.4 Akcje użytkownika
Użytkownik może:
- Dodać technologię do grafu (z listy rekomendacji AI)
- Dodać technologię do listy "Ignore" - robi to na popupie wyszukiwania technologii
- Zmienić aktualny postęp nauki (suwak 0-100%)
- Usunąć technologię z grafu
- Modyfikować technologię (prywatny opis, progress)
- Przywrócić technologię z listy "Ignore"

### 3.6 System Autentykacji

#### 3.6.1 Mechanizm logowania
- Autentykacja przez Google OAuth
- Brak własnego systemu rejestracji i logowania

#### 3.6.2 Izolacja danych
- Każdy użytkownik ma własny profil
- Każdy użytkownik ma własny graf technologii
- Dane użytkowników są całkowicie oddzielone

#### 3.6.3 Ograniczenia
- Brak funkcji backup/import/export profilu
- Brak udostępniania celów innym użytkownikom

### 3.7 System Caching (Opcjonalny)

#### 3.7.1 Caching rekomendacji
- Rozważenie zapisywania wygenerowanych przez AI rekomendacji w bazie danych
- Cel: uniknięcie ponownego generowania dla tej samej konfiguracji
- Korzyść: oszczędność kosztów AI i czasu oczekiwania

### 3.8 Admin Panel (Niski Priorytet)

#### 3.8.1 Dostęp
- Prosty admin panel (może być hardcoded tylko dla administratora)
- Dostęp tylko dla konkretnego konta administratora

#### 3.8.2 Funkcjonalność
- Dashboard z podstawowymi metrykami
- Liczba użytkowników (aktywnych, zarejestrowanych)
- Najpopularniejsze prefiksy technologii
- Średni procent wypełnienia profili
- Weryfikacja kryteriów sukcesu MVP

---

## 4. Granice produktu

### 4.1 Co WCHODZI w zakres MVP

- Zapisywanie, odczytywanie, przeglądanie i usuwanie technologii do nauki
- Prosty system kont użytkowników do powiązania użytkownika z własnymi celami
- Strona profilu użytkownika do zapisywania doświadczenia i aktualnej wiedzy
- Integracja z AI umożliwiająca generowanie nowych celów nauki
- Graf technologii z wizualizacją zależności
- System zarządzania postępem nauki (suwak procentowy)
- Lista ignorowanych technologii
- Google OAuth autentykacja
- Możliwość edycji profilu w dowolnym momencie
- Dodawanie prywatnych opisów do technologii

### 4.2 Co NIE WCHODZI w zakres MVP

Następujące funkcjonalności są wyraźnie poza zakresem MVP:

- Przechowywanie adresów URL do źródeł wiedzy
- Udostępnianie swoich celów innym użytkownikom
- Funkcje społecznościowe (komentarze, polubienia, obserwowanie)
- Import/eksport profilu (backup użytkownika)
- Natywne aplikacje mobilne
- Automatyczna aktualizacja rekomendacji po zmianie profilu
- Limity anti-spam dla generowania rekomendacji
- Ocenianie jakości rekomendacji AI przez użytkowników
- Gotowe API do trendów bez AI
- Auto-complete dla technologii
- Bazowa pula technologii w bazie danych
- Regeneracja grafu po zmianie profilu
- System osiągnięć/gamifikacji
- Zaawansowane analytics
- Powiadomienia i alerty
- Integracja z zewnętrznymi platformami (GitHub, LinkedIn)
- Funkcja wyszukiwania technologii
- Zaawansowane filtrowanie i sortowanie

### 4.3 Przyszłe iteracje (Post-MVP)

Funkcjonalności rozważane w przyszłych wersjach:
- Przechowywanie URL do źródeł wiedzy dla każdej technologii
- Udostępnianie profili i grafów innym użytkownikom
- Funkcje społecznościowe i współpraca
- Import/eksport profilu
- System osiągnięć i gamifikacji
- Zaawansowane analytics i raporty
- Integracje z platformami zewnętrznymi
- Aplikacje mobilne (iOS, Android)

---

## 5. Historyjki użytkowników

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

---

## 6. Metryki sukcesu

### 6.1 Kryteria sukcesu MVP (KPI)

Następujące wskaźniki określają sukces wersji MVP aplikacji:

1. 90% użytkowników posiada wypełnioną sekcję Profil Użytkownika
   - Definicja: użytkownik wypełnił wszystkie wymagane pola profilu (główne technologie, rola, obszar)
   - Metoda pomiaru: analiza bazy danych, liczba kompletnych profili / liczba wszystkich użytkowników

2. 75% użytkowników wygenerowało przynajmniej jeden temat do nauki
   - Definicja: użytkownik kliknął "Szukaj nowych technologii" i dodał przynajmniej jedną technologię do grafu
   - Metoda pomiaru: analiza bazy danych, liczba użytkowników z min. 1 technologią w grafie / liczba wszystkich użytkowników

### 6.2 Metryki śledzone (Analytics)

#### 6.2.1 Metryki użytkowników
- Liczba zarejestrowanych użytkowników (total)
- Liczba aktywnych użytkowników (logowali się w ostatnich 7/30 dniach)
- Średni czas od rejestracji do wypełnienia profilu
- Procent użytkowników, którzy wypełnili profil w ciągu 24h od rejestracji

#### 6.2.2 Metryki generowania rekomendacji AI
- Całkowita liczba wygenerowanych technologii przez AI (total)
- Średnia liczba generowań na użytkownika
- Średni czas odpowiedzi AI (w sekundach)
- Liczba timeoutów AI (przekroczenie 20 sekund)
- Liczba błędów AI (error responses)

#### 6.2.3 Metryki akceptacji technologii
- Liczba zaakceptowanych technologii (dodanych do grafu) total
- Średnia liczba technologii w grafie na użytkownika
- Procent akceptacji: (technologie dodane do grafu / technologie wygenerowane przez AI) × 100%
- Liczba technologii dodanych do listy "Ignore"
- Procent ignorowania: (technologie ignorowane / technologie wygenerowane) × 100%

#### 6.2.4 Metryki postępu nauki
- Liczba technologii z postępem > 0%
- Liczba technologii z postępem = 100% (ukończonych)
- Średni procent postępu wszystkich technologii
- Średni czas od dodania technologii do osiągnięcia 100%

#### 6.2.5 Metryki popularności kategorii
- Najpopularniejsze prefiksy/kategorie (DotNet, Java, JavaScript, etc.)
  - Liczba technologii w każdej kategorii
  - Procent użytkowników korzystających z danej kategorii
- Najpopularniejsze tagi (Technologia, Framework, Baza danych)
- Najpopularniejsze role w profilach (Programista, Tester, Analityk)
- Najpopularniejsze obszary rozwoju (UI, Backend, Full Stack)

#### 6.2.6 Metryki zaangażowania
- Średnia liczba wizyt na użytkownika (w miesiącu)
- Średni czas spędzony w aplikacji (per sesja)
- Częstotliwość aktualizacji postępu nauki
- Liczba edycji profilu (per użytkownik)
- Retencja użytkowników (użytkownicy aktywni po 7/30/90 dniach od rejestracji)

### 6.3 Admin Panel (Niski priorytet)

W ramach admin panelu (jeśli zostanie zaimplementowany) dostępne będą następujące widoki:

1. Dashboard z podsumowaniem:
   - Liczba użytkowników (total, aktywni)
   - Weryfikacja KPI (90% wypełnionych profili, 75% z wygenerowanymi technologiami)
   - Statystyki AI (liczba requestów, timeouty, błędy)

2. Najpopularniejsze kategorie:
   - Wykres/lista najpopularniejszych prefiksów technologii
   - Procent użytkowników w każdej kategorii

3. Średni postęp:
   - Średni procent wypełnienia profili
   - Średni postęp nauki technologii
   - Liczba ukończonych technologii

4. Dostęp:
   - Hardcoded tylko dla konta administratora
   - Brak interfejsu zarządzania użytkownikami

### 6.4 Cel biznesowy MVP

Głównym celem wersji MVP jest walidacja produktu:
- Czy programiści potrzebują takiego narzędzia?
- Czy AI generuje wartościowe rekomendacje?
- Czy użytkownicy korzystają z aplikacji regularnie?
- Czy graf technologii jest intuicyjny i użyteczny?

Sukces MVP będzie oznaczał gotowość do dalszych iteracji i rozbudowy funkcjonalności w kierunku wersji 1.0.

---

## Koniec dokumentu PRD

Wersja: 1.0  
Ostatnia aktualizacja: 16 listopada 2025  
Status: DRAFT - MVP  

Ten dokument stanowi kompleksowy zbiór wymagań dla wersji MVP aplikacji DeveloperGoals i jest przeznaczony dla zespołu deweloperskiego jako podstawa implementacji.

