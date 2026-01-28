Twoim zadaniem jest wygenerowanie kompleksowego i wysokiej jakości planu testów dla projektu programistycznego. Plan testów musi być dostosowany do specyfiki projektu oraz wykorzystywanych technologii.

Oto stos technologiczny używany w projekcie:

<tech_stack>
@tech-stack.md
</tech_stack>

Oto baza kodu projektu, dla którego masz wygenerować plan testów:

<codebase>
@Codebase
</codebase>

Aby stworzyć odpowiedni plan testów, wykonaj następujące kroki:

1. Przeanalizuj dostarczony kod projektu, aby zrozumieć:
   - Architekturę aplikacji
   - Główne komponenty i moduły
   - Zależności między komponentami
   - Krytyczne ścieżki funkcjonalne
   - Potencjalne obszary ryzyka

2. Zbadaj stos technologiczny, aby zidentyfikować:
   - Specyficzne wymagania testowe dla każdej technologii
   - Odpowiednie narzędzia i frameworki testowe dla danego stosu
   - Najlepsze praktyki testowania w kontekście użytych technologii
   - Specyficzne wzorce testowania rekomendowane dla tych technologii

3. Zaplanuj kompleksową strategię testowania, która obejmuje:
   - Różne poziomy testowania (jednostkowe, integracyjne, systemowe, end-to-end)
   - Testy funkcjonalne i niefunkcjonalne
   - Obszary wymagające szczególnej uwagi ze względu na użyte technologie
   - Priorytyzację testów na podstawie ryzyka i krytyczności

Przed napisaniem ostatecznego planu testów, przeprowadź szczegółową analizę w tagach <analysis>. W analizie:
- Wypisz i ponumeruj wszystkie kluczowe komponenty/moduły zidentyfikowane w kodzie
- Dla każdej technologii w stosie technologicznym, wymień konkretne frameworki i narzędzia testowe, które są dla niej odpowiednie
- Zidentyfikuj i wypisz krytyczne ścieżki funkcjonalne w aplikacji
- Wypisz potencjalne obszary ryzyka i uzasadnij, dlaczego stanowią one ryzyko
- Określ, jakie rodzaje testów są najbardziej odpowiednie dla danego stosu technologicznego
- Zaplanuj strukturę planu testów
To jest dobra okazja, aby szczegółowo rozpisać swoją analizę - ta sekcja może być dość długa.

Twój finalny plan testów powinien być przedstawiony w tagach <test_plan> i zawierać następujące sekcje:

1. **Przegląd i zakres**: Krótki opis projektu i celów testowania
2. **Cele testowania**: Co chcemy osiągnąć poprzez testowanie
3. **Strategia testowania**: Podejście do testowania dostosowane do stosu technologicznego
4. **Kategorie testów**: 
   - Testy jednostkowe
   - Testy integracyjne
   - Testy end-to-end
   - Testy wydajnościowe (jeśli stosowne)
   - Testy bezpieczeństwa (jeśli stosowne)
   - Inne typy testów specyficzne dla projektu
5. **Scenariusze testowe**: Konkretne przypadki testowe dla kluczowych funkcjonalności
6. **Narzędzia i frameworki**: Specyficzne narzędzia testowe odpowiednie dla stosu technologicznego
7. **Środowiska testowe**: Wymagania dotyczące środowisk testowych
8. **Kryteria akceptacji**: Jak określić, czy testowanie zakończyło się sukcesem

Przykładowa struktura wyjścia:

<analysis>
[Tutaj przeprowadź szczegółową analizę kodu i stosu technologicznego, zidentyfikuj kluczowe obszary do testowania, rozważ specyficzne wymagania technologiczne]
</analysis>

<test_plan>
## 1. Przegląd i zakres
[Opis projektu i zakresu testowania]

## 2. Cele testowania
[Lista celów]

## 3. Strategia testowania
[Opis strategii dostosowanej do technologii]

## 4. Kategorie testów
### 4.1 Testy jednostkowe
[Opis i narzędzia]

### 4.2 Testy integracyjne
[Opis i narzędzia]

[... inne kategorie ...]

## 5. Scenariusze testowe
[Konkretne przypadki testowe]

## 6. Narzędzia i frameworki
[Lista narzędzi specyficznych dla stosu technologicznego]

## 7. Środowiska testowe
[Wymagania środowiskowe]

## 8. Kryteria akceptacji
[Kryteria sukcesu]
</test_plan>

Upewnij się, że plan testów jest:
- Specyficzny dla dostarczonego projektu i jego kodu
- Dostosowany do użytych technologii
- Praktyczny i możliwy do wdrożenia
- Kompleksowy i pokrywający wszystkie krytyczne obszary
- Zawiera konkretne rekomendacje narzędzi pasujących do stosu technologicznego