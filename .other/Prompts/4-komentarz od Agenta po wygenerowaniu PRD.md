Stworzyłem kompleksowy dokument PRD dla aplikacji DeveloperGoals i zapisałem go w pliku `.ai/prd.md`.

Dokument zawiera:

## 1. Przegląd produktu
- Wizja i unikalna wartość aplikacji
- Grupa docelowa (junior, mid, senior developers)
- Stack technologiczny (ASP.NET Core, Google OAuth, Entity Framework)
- Harmonogram (5 tygodni) i zasoby (1 developer)

## 2. Problem użytkownika
- Szczegółowy opis głównego problemu
- Ograniczenia obecnych rozwiązań
- Proponowane rozwiązanie z wykorzystaniem AI

## 3. Wymagania funkcjonalne
8 szczegółowych sekcji obejmujących:
- System profilu użytkownika i onboarding
- System technologii z modelem danych
- Mechanizm rekomendacji AI (timeout 20s, około 10 technologii)
- Graf technologii jako główny widok
- Learning Dashboard z listą ignorowanych
- System autentykacji (Google OAuth)
- System caching i Admin Panel

## 4. Granice produktu
- Jasno określone co wchodzi w zakres MVP
- Lista 20+ funkcjonalności, które NIE wchodzą w zakres
- Planowane przyszłe iteracje

## 5. Historyjki użytkowników
30 kompletnych user stories (US-001 do US-030) obejmujących:
- Autentykację i bezpieczeństwo (US-001, US-017, US-018, US-025)
- Onboarding i zarządzanie profilem (US-002, US-003, US-023)
- Generowanie i zarządzanie rekomendacjami AI (US-005, US-010, US-026, US-028, US-029, US-030)
- Operacje na grafie (US-006, US-007, US-008, US-011, US-012, US-013, US-014, US-015, US-016)
- Obsługę błędów (US-019, US-020)
- Responsywność (US-021)
- I wiele innych scenariuszy

Każda historyjka zawiera:
- Unikalny identyfikator (US-XXX)
- Tytuł
- Opis w formacie "JAKO... CHCĘ... ABY..."
- Szczegółowe, testowalne kryteria akceptacji

## 6. Metryki sukcesu
- 2 główne KPI dla MVP (90% wypełnionych profili, 75% wygenerowanych technologii)
- 6 kategorii śledzonych metryk (użytkownicy, AI, akceptacja, postęp, popularność, zaangażowanie)
- Opis Admin Panelu
- Cel biznesowy MVP

Dokument jest w całości po polsku, bez pogrubień, sformatowany w markdown i gotowy do wykorzystania przez zespół deweloperski.