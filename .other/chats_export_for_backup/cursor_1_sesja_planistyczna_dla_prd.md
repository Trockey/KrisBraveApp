# 1. Sesja planistyczna dla PRD
_Exported on 11/16/2025 at 23:45:32 GMT+1 from Cursor (2.0.77)_

---

**User**


Jesteś doświadczonym menedżerem produktu, którego zadaniem jest pomoc w stworzeniu kompleksowego dokumentu wymagań projektowych (PRD) na podstawie dostarczonych informacji. Twoim celem jest wygenerowanie listy pytań i zaleceń, które zostaną wykorzystane w kolejnym promptowaniu do utworzenia pełnego PRD.

Prosimy o uważne zapoznanie się z poniższymi informacjami:

<project_description>
@MVP.txt 
</project_description>

Przeanalizuj dostarczone informacje, koncentrując się na aspektach istotnych dla tworzenia PRD. Rozważ następujące kwestie:
<prd_analysis>
1. Zidentyfikuj główny problem, który produkt ma rozwiązać.
2. Określ kluczowe funkcjonalności MVP.
3. Rozważ potencjalne historie użytkownika i ścieżki korzystania z produktu.
4. Pomyśl o kryteriach sukcesu i sposobach ich mierzenia.
5. Oceń ograniczenia projektowe i ich wpływ na rozwój produktu.
</prd_analysis>

Na podstawie analizy wygeneruj listę 10 pytań i zaleceń w formie łączonej (pytanie + zalecenie). Powinny one dotyczyć wszelkich niejasności, potencjalnych problemów lub obszarów, w których potrzeba więcej informacji, aby stworzyć skuteczny PRD. Rozważ pytania dotyczące:

1. Szczegółów problemu użytkownika
2. Priorytetyzacji funkcjonalności
3. Oczekiwanego doświadczenia użytkownika
4. Mierzalnych wskaźników sukcesu
5. Potencjalnych ryzyk i wyzwań
6. Harmonogramu i zasobów

Dane wyjściowe z pierwszej rundy powinny mieć następującą strukturę:

<prd_planning_output>
	<pytania_runda_1>
		[Wymień tutaj swoje pytania, ponumerowane]
	</pytania_runda_1>

	<rekomendacje_runda_1>
		[Wymień tutaj swoje zalecenia, ponumerowane]
	</rekomendacje_runda_1>
</prd_planning_output>


Kontynuuj ten proces, generując nowe pytania i rekomendacje w oparciu o odpowiedzi użytkownika, dopóki użytkownik wyraźnie nie poprosi o podsumowanie.

Pamiętaj że Twoim celem jest dostarczenie kompleksowej listy pytań i zaleceń, które pomogą w stworzeniu solidnego PRD dla MVP. Skoncentruj się na jasności, trafności i dokładności swoich wyników.
Nie dołączaj żadnych dodatkowych komentarzy ani wyjaśnień poza określonym formatem wyjściowym zamieszczonym w proponowanym formacie XML <prd_planning_output>.


---

**Cursor**

<prd_planning_output>
	<pytania_runda_1>
		1. Kto jest główną grupą docelową aplikacji (juniorzy, mid-level, seniorzy, czy wszyscy poziomy) i jakie są ich konkretne potrzeby w zakresie rozwoju zawodowego?

		2. W jaki sposób AI będzie określać, które technologie i frameworki są "najnowsze" i warte polecenia - na podstawie trendów rynkowych, ogłoszeń o pracę, GitHub, czy innych źródeł?

		3. Jak szczegółowe informacje powinien zawierać profil użytkownika - czy to będzie prosty formularz z latami doświadczenia i listą technologii, czy bardziej zaawansowany system z poziomami biegłości i projektami?

		4. W jaki sposób użytkownik będzie wchodzić w interakcję z AI - poprzez chat, formularz z preferencjami, automatyczne sugestie, czy kombinację tych metod?

		5. Jakie są oczekiwania co do liczby użytkowników testowych/startowych w MVP i w jakim horyzoncie czasowym planujecie osiągnąć kryteria sukcesu (90% profili, 75% tematów)?

		6. Czy istnieją jakiekolwiek ograniczenia technologiczne lub budżetowe dotyczące integracji z AI (np. limit zapytań do API, koszt per użytkownika)?

		7. Jak często AI powinno generować nowe propozycje tematów - na żądanie użytkownika, cyklicznie (np. raz w tygodniu), czy po aktualizacji profilu?

		8. W jaki sposób będziecie weryfikować jakość rekomendacji AI - czy użytkownicy będą mogli oceniać sugestie, aby ulepszać system?

		9. Jakie kluczowe metryki (poza wskaźnikami sukcesu) będziecie śledzić - engagement rate, czas spędzony w aplikacji, ilość dodanych tematów na użytkownika?

		10. Czy istnieją już podobne rozwiązania konkurencyjne na rynku i jeśli tak, co będzie wyróżniać waszą aplikację (unique value proposition)?
	</pytania_runda_1>

	<rekomendacje_runda_1>
		1. REKOMENDACJA: Zdefiniuj szczegółowe persony użytkowników (minimum 2-3) z konkretnymi scenariuszami użycia, aby lepiej dopasować funkcjonalność AI do realnych potrzeb różnych grup programistów.

		2. REKOMENDACJA: Określ jasne kryteria i algorytm rankingowy dla technologii polecanych przez AI, uwzględniając takie czynniki jak popularność, perspektywy zatrudnienia, oraz zgodność z obecnymi umiejętnościami użytkownika.

		3. REKOMENDACJA: Stwórz strukturę danych dla profilu użytkownika, która będzie wystarczająco szczegółowa dla AI, ale jednocześnie szybka do wypełnienia (maksymalnie 5 minut), aby osiągnąć cel 90% wypełnionych profili.

		4. REKOMENDACJA: Zaprojektuj onboarding flow, który natychmiast pokazuje wartość aplikacji - np. po wypełnieniu podstawowego profilu użytkownik od razu otrzymuje pierwszą rekomendację AI.

		5. REKOMENDACJA: Ustaw realistyczne timeline dla MVP z konkretnymi kamieniami milowymi (np. prototyp za 4 tygodnie, beta za 8 tygodni, launch za 12 tygodni) wraz z zasobami ludzkimi potrzebnymi do realizacji.

		6. REKOMENDACJA: Zaplanuj system kategoryzacji tematów do nauki (języki programowania, frameworki, narzędzia, soft skills) oraz poziomy trudności, aby użytkownik mógł łatwo filtrować i organizować swoje cele.

		7. REKOMENDACJA: Uwzględnij w PRD prostą analitykę użytkownika pokazującą postęp w nauce (ile tematów dodano, ile ukończono), co zwiększy zaangażowanie i retention.

		8. REKOMENDACJA: Zdefiniuj minimal viable interface dla integracji AI - czy wystarczy jeden przycisk "Wygeneruj sugestie", czy potrzebny jest bardziej interaktywny proces z możliwością doprecyzowania oczekiwań?

		9. REKOMENDACJA: Rozważ dodanie do MVP prostego mechanizmu oznaczania tematów jako "w trakcie" lub "ukończone", co pomoże AI lepiej rozumieć progres użytkownika i dostosowywać przyszłe rekomendacje.

		10. REKOMENDACJA: Określ strategię monetyzacji już na etapie PRD (freemium, subscription, darmowa wersja) oraz jakie funkcje będą w poszczególnych tier'ach, aby mieć jasną wizję rozwoju produktu po MVP.
	</rekomendacje_runda_1>
</prd_planning_output>

---

**User**

Odpowiedzi:

1. Jest dla wazytkich programistów, juniorzy, mid, seniorzy. Ich potrzeby mają wynikać z ich aktualnej wiedzy. Np. jeśli ktoś zna Entity Framework, to następnym krokiem do nauki może być inne ORM jak nHibernate. Jeśłi ktoś zna narzędzie Visual Studio to następnym krokiem może być inne IDE jak Rider czy Visual Studio Code

2. Tak, na podsrawie trendów, trendów git hub, rozmów o pracę. Oczywiście jeśli ktoś specjalizuje się w .NET to taka ścieżka specjalizacji obowiązuje i raczej nie powinnien mu być proponowany język Java

3. Raczej prosty formularz z poziomem doswiadczenia, technologiami oraz zaawansowaniem w tych technologiach jak poziom. Niski, średni, wysoki

4. Użytkownik nie wchodzi w interakcję z AI. Po prostu na podstawie jego profilu dostaje listę proponowanych technologii. I wtedy wskazuje te które go interesują. I wpisuje te technologie na listę "TODO". Oraz podaje inne technologie na list "Ignore".

Czyli po wypelnieniu profilu, od razu dostaje rekomendacje.

Ewentialnie po wybraniu technologii X na listę TODO - dostaje kolejne technologie do nauki jak już skończy naukę technologii X.

5. Jeśli to jest pytanie o programistę to będzie tylko 1 osoba czyli ja. Czas reazlizacji to 5 tygodni do wersji BETA.

6. Raczej nie ma ograniczeń budżetowych, ale rozwiązania AI nie powinny być najdroższe, ale też nie najtańsze - AI za standardowe ceny.

7. Na żądanie użytkownika.

8. Użytkownicy nie będą mogli oceniać sugestii

9. Ilość wygenerowanych technologii. Dodatkowo użytkownik będzie mógł zaznaczyć procent wiedzy jaki zdobył podczas nauki poszczegółnej technologii. To będzie śledziło postęp użytkownika, ale dla aplikacji ma znaczenie ile technologii zaapceptował użytkownik i w ilu technologiaach oznczył postęp nauki

10. Nie wiem czy jest konkurencja. Unikalną wartością jest prostota aplikacji i łatwe nią zarządzanie, oraz czytelny i intuicyjny wybór kolejnych technologii. 

---

**Cursor**

<prd_planning_output>
	<pytania_runda_2>
		1. Jak szczegółowo użytkownik powinien opisywać swoje technologie w profilu - czy wystarczy nazwa (np. "C#"), czy też z kontekstem użycia (np. "C# - Web API, Entity Framework, LINQ")?

		2. W jaki sposób użytkownik będzie oznaczał postęp nauki technologii - poprzez suwak procentowy, wybór etapów (np. "podstawy", "średniozaawansowany", "zaawansowany"), czy ręczne wpisanie wartości procentowej?

		3. Czy technologie na liście "Ignore" będą ukryte na zawsze, czy użytkownik powinien mieć możliwość przejrzenia i odblokowania ich w przyszłości?

		4. Ile technologii powinno być jednorazowo generowanych przez AI - 5, 10, 20 propozycji, czy inna liczba, aby nie przytłoczyć użytkownika?

		5. Czy system powinien rozróżniać różne typy technologii (języki programowania, frameworki, narzędzia, bazy danych, metodologie) i prezentować je osobno, czy jedna wspólna lista?

		6. Jak będzie wyglądać proces onboardingu - czy użytkownik będzie musiał wypełnić cały profil od razu, czy może zacząć od minimum (np. 3 technologie) i rozbudowywać później?

		7. Czy aplikacja będzie webowa, desktopowa, mobilna, czy kombinacja tych rozwiązań, biorąc pod uwagę 5-tygodniowy timeline?

		8. Jakie źródło danych AI będzie wykorzystywać do określania trendów - publiczne API (StackOverflow, GitHub Trends), web scraping, czy gotowe bazy danych trendów technologicznych?

		9. Czy przewidujesz możliwość importu/eksportu profilu użytkownika i jego listy technologii (np. JSON, CSV) na potrzeby backup'u lub migracji?

		10. W jaki sposób użytkownik będzie zarządzał listą TODO - czy technologie będą sortowane według priorytetu, daty dodania, czy poziomu trudności, i czy użytkownik będzie mógł zmieniać kolejność?
	</pytania_runda_2>

	<rekomendacje_runda_2>
		1. REKOMENDACJA: Stwórz prosty wizard onboardingu składający się z 3 kroków: (1) Poziom doświadczenia ogólny, (2) Wybór minimum 5 znanych technologii z poziomu, (3) Automatyczne wygenerowanie pierwszych rekomendacji - maksymalny czas wypełnienia 3 minuty.

		2. REKOMENDACJA: Zaimplementuj system tagów/kategorii dla technologii (Backend, Frontend, Database, DevOps, Tools) aby AI mogło generować zrównoważone rekomendacje z różnych obszarów, a użytkownik mógł filtrować widok.

		3. REKOMENDACJA: Stwórz prostą tabelę/kartę dla każdej technologii zawierającą: nazwę, kategorię, opis (1-2 zdania), dlaczego jest rekomendowana ("Bo znasz X"), oraz akcje (Dodaj do TODO, Ignore).

		4. REKOMENDACJA: Zaplanuj wykorzystanie gotowego API do trendów technologicznych (np. GitHub Trending API, StackOverflow API) zamiast web scrapingu, aby zaoszczędzić czas developmentu w 5-tygodniowym timeframe.

		5. REKOMENDACJA: Dla MVP zaimplementuj aplikację webową (responsive) zamiast natywnych aplikacji mobilnych/desktopowych - szybszy development, jeden codebase, łatwiejsze testowanie i deployment.

		6. REKOMENDACJA: Dodaj dashboard z widokiem "W trakcie nauki" pokazującym technologie z listy TODO wraz z paskami postępu, co da użytkownikowi natychmiastowy wgląd w swój rozwój przy każdym logowaniu.

		7. REKOMENDACJA: Ograniczenie generowania AI do maksymalnie 10 technologii na raz, z możliwością "Wygeneruj więcej" - zapobiega to przytłoczeniu użytkownika i redukuje koszty API przy każdym zapytaniu.

		8. REKOMENDACJA: Zaimplementuj prosty system "Learning Path" - gdy użytkownik doda technologię do TODO i zaznaczy 100% postępu, system automatycznie proponuje 3-5 powiązanych technologii jako "Kolejny krok".

		9. REKOMENDACJA: Stwórz sekcję "Ignore List" z możliwością przeglądu i usuwania technologii z tej listy, ponieważ preferencje użytkownika mogą się zmieniać w czasie (np. po roku junior może chcieć nauczyć się zaawansowanych narzędzi).

		10. REKOMENDACJA: Zaprojektuj prosty stack technologiczny dla MVP: React/Vue (frontend), Node.js/Python (backend), PostgreSQL (database), OpenAI API lub podobne (AI), Vercel/Railway (hosting) - sprawdzone technologie skracające czas developmentu.
	</rekomendacje_runda_2>
</prd_planning_output>

---

**User**

1. Tak, z kontekstem użycia np. "Entity Framework". Użytkonik będzie mógł wybrać technologię z puli technologii, albo wpisać swoją własną

Lista technologii nie będzie hierarchiczna, ale będzie miała prefix który harakteryzuję linię rozwoju np. "DotNet - Entity Framework", w tym przykładzie prefiksem jest "DotNet"

2. Suwak procentowy

3. Ma mieć możliwość przejrzenia w przyszłości. Oraz te ignorowane też będą się pojawiały na liście proponowanych ale to będzie druga lista poniżej "Ignorowane". Tak, technologie powinny mieć kategorie, tak jak ten prefix. Oraz opis technologii.

4. Musi być wykorzystanie AI, jeśłi API do trendów technologicznych są API z AI to można wykorzystać. Jeśli API nie są związane z AI - nie mogę tego wykorzystać. Musi być wykorzystane AI do szukania trendów.

5. Najlepiej jedna wsólna lista, ale może być TAG "Technologia", "Framework", "baza damych"

6. Im więcej user doda technologi które zna na początku tym lepiej. Oczywiście już podczas dodawania, ma propozycje kolenych technologi.

Albo inaczej - samo wypełnianie profilu to już praca właściwa z projektem. Właścwie to profil powinien zawierać:

a) główne technologie np. Java, DotNet, JavaScript, Pyton

b) Programista, tester, analityk

c) Obszar: User Interface, Backend, Full Stack

7. Aplikacje Webowa w technologii Microsoft, czyli MVC lub Blazor lub Razor Pages

8. Źródłem danych będzie AI - ale nie wiem jak AI ma sobie radzić - niech AI sięga gdzie mu lepiej, może być wszystko czyli:

publiczne API (StackOverflow, GitHub Trends), web scraping, czy gotowe bazy danych trendów technologicznych

9. Nie, bez backupu

10. Kolejność będzie dowolna. Wyobrażam sobie grid, który będzie sortowalny przez usera. Domyślnie może być posortowany według procentowej wiedzy na temat danej technologii, im lepsza procentowa wiedza tym wyżej na liście

Prezentowane technologie powinny być w formie grafu. Które technologie następne, a które po nich do nauki.



---

**Cursor**

<prd_planning_output>
	<pytania_runda_3>
		1. Czy graf technologii powinien być interaktywny (klikalne węzły, zoom, przeciąganie) czy statyczny obraz pokazujący zależności, i czy będzie to główny widok aplikacji czy dodatkowa wizualizacja?

		2. W jaki sposób AI będzie określać zależności między technologiami dla grafu - czy to ma być logika hardcoded'owa (np. "po Entity Framework idzie Dapper"), czy AI ma dynamicznie generować te ścieżki?

		3. Jak szczegółowy ma być opis każdej technologii - krótki opis (50-100 słów), czy rozbudowany z przykładami użycia i linkami do dokumentacji?

		4. Czy przy wyborze technologii z puli użytkownik będzie miał auto-complete/wyszukiwarkę, czy pełną listę do przewijania, i skąd ta pula będzie pochodzić (baza danych, generowana przez AI)?

		5. Czy użytkownik będzie mógł edytować swój profil (główne technologie, rola, obszar) po pierwszym wypełnieniu, i czy zmiana profilu powinna automatycznie generować nowe rekomendacje?

		6. Jakie dokładnie dane mają być przesyłane do AI przy generowaniu rekomendacji - cały profil użytkownika, tylko główne technologie, czy także procent postępu w już dodanych technologiach?

		7. Czy system autentykacji użytkowników ma być własny (email/hasło), czy przez zewnętrznych providerów (Microsoft, Google, GitHub), co może wpłynąć na timeline 5 tygodni?

		8. Jak długo może trwać generowanie rekomendacji przez AI - czy użytkownik zobaczy loader/spinner, i czy ma być limit czasowy po którym system wyświetli komunikat o błędzie?

		9. Czy przewidujesz jakieś podstawowe statystyki/analytics dla samego developera (Ciebie) - np. ile użytkowników, ile wygenerowano technologii, najpopularniejsze prefiksy?

		10. Czy MVP powinno zawierać jakiekolwiek mechanizmy zabezpieczające przed spam'em/nadużyciami (np. limit generowania rekomendacji na dzień), czy to zostanie dodane w późniejszych wersjach?
	</pytania_runda_3>

	<rekomendacje_runda_3>
		1. REKOMENDACJA: Użyj gotowej biblioteki do wizualizacji grafów (np. React Flow, vis.js, D3.js force graph) zamiast pisać własną implementację - zaoszczędzi to tydzień pracy przy 5-tygodniowym timeline.

		2. REKOMENDACJA: Zaprojektuj strukturę danych dla technologii zawierającą: ID, Nazwa, Prefix (kategoria główna), Tag (typ: technologia/framework/baza), Opis, Powiązane_Technologie (array ID), Co_Trzeba_Znać (prerequisites).

		3. REKOMENDACJA: Zaimplementuj ASP.NET Core z Blazor Server jako główną technologię - najszybsze do prototypowania, nie wymaga oddzielnego frontendu, łatwa integracja z Entity Framework, idealne dla solo developera.

		4. REKOMENDACJA: Stwórz prompt dla AI, który będzie zawierał: profil użytkownika (główne tech, rola, obszar), znane technologie z poziomem, oraz explicite żądanie zwrócenia JSON z technologiami wraz z polem "reasoning" wyjaśniającym dlaczego dana technologia jest rekomendowana.

		5. REKOMENDACJA: Zaprojektuj dwa główne widoki: (1) "Learning Dashboard" - grid z listą TODO sortowalny z suwakami postępu, (2) "Technology Graph" - wizualizacja zależności pokazująca ścieżki rozwoju od znanych do proponowanych technologii.

		6. REKOMENDACJA: Dla MVP użyj Microsoft Identity (ASP.NET Identity) z prostym email/password - szybkie w implementacji, sprawdzone, wystarczające dla beta, providerzy zewnętrzni mogą być dodani później.

		7. REKOMENDACJA: Stwórz bazową pulę ~200-300 popularnych technologii w bazie danych (można wygenerować listę przez AI jednorazowo przed rozpoczęciem projektu) z prefiksami DotNet, Java, JavaScript, Python, Database, DevOps, Tools.

		8. REKOMENDACJA: Zaimplementuj caching dla rekomendacji AI - gdy użytkownik nie zmienia profilu, nie generuj ponownie, tylko pokaż poprzednie wyniki. To oszczędzi koszty API i przyspieszy działanie aplikacji.

		9. REKOMENDACJA: Dodaj prostą walidację wywołań AI: maksymalnie 10 generowań rekomendacji dziennie na użytkownika w MVP, z informacją "Osiągnąłeś dzienny limit generowania" - zabezpiecza budżet i motywuje do przemyślanego korzystania.

		10. REKOMENDACJA: Zaplanuj prosty admin panel (może być hardcoded tylko dla Twojego konta) pokazujący: liczbę użytkowników, liczbę wygenerowanych technologii, najpopularniejsze prefiksy, średni procent wypełnienia profili - kluczowe dla weryfikacji kryteriów sukcesu MVP.
	</rekomendacje_runda_3>
</prd_planning_output>

---

**User**

1. Graf będzie statyczy, z możliwością klikanie - wtedy pokazuje się popup które ma umożliwić edycję technologii, jak zmiany description, zmiana progresu wiedzy, usunięcie.
Stack technologiczny to technologie microsoft, czyli NIE react, NIE Vue, ale MVC, Razor Pages albo Blazro
2. AI powinno dynamicznie generować ścieżki. Aplikacja nie będzie trzymać powiązanych technologii. To ma znaleść AI.
3. Opis technologii ma być krótki. Użytkonik ma mieć możliwość dodawania sobie Description do technologii które będzie widoczne tylko dla niego
4. Brak auto complete. AI znajdzie listę kolejnych technologii bazując na dotychczasowo wybranych technologiach użytkonika. A właściwie po wybraniu jedej już wybranej technologii przez użytkownika, pojawia się lista np. 5 następnych. Użytkoniwk dodaje to do grafu. Jak doda nową technologię to klikając na nowej "Szukaj", ma pojawić się kolejna lista, z następnymi technologiami. I tak dalej
5. Tak, może edytować swój profil. Po zmanie nic się nie dzieje w grafie technologii
6. Do AI ma być przekazany Profile Użytkownika, jak i wszystkie technologie które zna, ale akcentowana ma być ta ostatnie, na której użytkownika nacisnął przycisk "Szukaj nowych technologii"
7. Może być Google OAut
8. Limit czasowy to 20 sekund. Tak, powinien być spinner
9. Tak, statystyki mogą być ale to nie ma priortetu.
10. Aplikacja bez limitu spamu

To już ostatnia lista moich odpowiedzi. Nie generuj już nowych pytań.

Teraz przejdz do nowej roli:

Jesteś asystentem AI, którego zadaniem jest podsumowanie rozmowy na temat planowania PRD (Product Requirements Document) dla MVP i przygotowanie zwięzłego podsumowania dla następnego etapu rozwoju. W historii konwersacji znajdziesz następujące informacje:
1. Opis projektu
2. Zidentyfikowany problem użytkownika
3. Historia rozmów zawierająca pytania i odpowiedzi
4. Zalecenia dotyczące zawartości PRD

Twoim zadaniem jest:
1. Podsumować historię konwersacji, koncentrując się na wszystkich decyzjach związanych z planowaniem PRD.
2. Dopasowanie zaleceń modelu do odpowiedzi udzielonych w historii konwersacji. Zidentyfikuj, które zalecenia są istotne w oparciu o dyskusję.
3. Przygotuj szczegółowe podsumowanie rozmowy, które obejmuje:
   a. Główne wymagania funkcjonalne produktu
   b. Kluczowe historie użytkownika i ścieżki korzystania
   c. Ważne kryteria sukcesu i sposoby ich mierzenia
   d. Wszelkie nierozwiązane kwestie lub obszary wymagające dalszego wyjaśnienia
4. Sformatuj wyniki w następujący sposób:

<conversation_summary>
<decisions>
[Wymień decyzje podjęte przez użytkownika, ponumerowane].
</decisions>

<matched_recommendations>
[Lista najistotniejszych zaleceń dopasowanych do rozmowy, ponumerowanych]
</matched_recommendations>

<prd_planning_summary>
[Podaj szczegółowe podsumowanie rozmowy, w tym elementy wymienione w kroku 3].
</prd_planning_summary>

<unresolved_issues>
[Wymień wszelkie nierozwiązane kwestie lub obszary wymagające dalszych wyjaśnień, jeśli takie istnieją]
</unresolved_issues>
</conversation_summary>

Końcowy wynik powinien zawierać tylko treść w formacie markdown. Upewnij się, że Twoje podsumowanie jest jasne, zwięzłe i zapewnia cenne informacje dla następnego etapu tworzenia PRD.




---

**Cursor**

<conversation_summary>

<decisions>

**GRUPA DOCELOWA I PERSONALIZACJA**

1. Aplikacja jest przeznaczona dla wszystkich poziomów programistów (junior, mid, senior)
2. Potrzeby użytkowników wynikają z ich aktualnej wiedzy - system sugeruje kolejne kroki bazując na tym co już znają
3. Specjalizacja użytkownika jest respektowana (np. developer .NET nie dostaje propozycji Java)

**MECHANIZM REKOMENDACJI AI**

4. AI będzie określać technologie na podstawie trendów rynkowych, trendów GitHub i ogłoszeń o pracę
5. Użytkownik NIE wchodzi w bezpośrednią interakcję z AI (brak chatu) - system automatycznie generuje rekomendacje
6. Rekomendacje są generowane na żądanie użytkownika (przycisk "Szukaj nowych technologii")
7. AI dynamicznie generuje ścieżki rozwoju między technologiami - aplikacja nie przechowuje tych zależności
8. Do AI przekazywany jest cały profil użytkownika oraz wszystkie znane technologie, z akcentem na ostatnio wybraną technologię
9. Limit czasowy dla generowania przez AI: 20 sekund (z spinnerem)
10. Brak limitów anti-spam dla generowania rekomendacji

**STRUKTURA PROFILU UŻYTKOWNIKA**

11. Profil zawiera: (a) główne technologie (Java, .NET, JavaScript, Python), (b) rola (programista, tester, analityk), (c) obszar (UI, Backend, Full Stack)
12. Użytkownik może edytować profil w dowolnym momencie
13. Zmiana profilu NIE powoduje automatycznej regeneracji grafu technologii
14. Im więcej technologii użytkownik doda na początku, tym lepiej
15. Podczas wypełniania profilu użytkownik od razu otrzymuje propozycje kolejnych technologii

**SYSTEM TECHNOLOGII**

16. Technologie mają format z prefiksem określającym linię rozwoju, np. "DotNet - Entity Framework"
17. Użytkownik może wybrać technologię z puli lub wpisać własną
18. Każda technologia ma: prefix/kategorię główną, tag (Technologia/Framework/Baza danych), opis (krótki)
19. Użytkownik może dodać prywatny opis technologii widoczny tylko dla niego
20. Technologie prezentowane są w jednej wspólnej liście z możliwością tagowania

**WORKFLOW UŻYTKOWNIKA**

21. Po wypełnieniu profilu użytkownik natychmiast dostaje pierwsze rekomendacje
22. Użytkownik wskazuje interesujące technologie → dodaje je do listy TODO
23. Nieinteresujące technologie → dodaje do listy "Ignore"
24. Po wybraniu technologii X (kliknięcie "Szukaj"), system generuje ~5 kolejnych powiązanych technologii
25. Gdy użytkownik oznaczy 100% postępu w technologii, może wygenerować kolejne sugestie

**ZARZĄDZANIE POSTĘPEM**

26. Postęp nauki oznaczany jest suwakiem procentowym
27. Lista "Ignore" jest dostępna do przeglądania i modyfikacji
28. Ignorowane technologie pojawiają się na oddzielnej liście poniżej ("Ignorowane")
29. Lista TODO prezentowana jako sortowalny grid
30. Domyślne sortowanie: według procentowej wiedzy (więcej wiedzy = wyżej na liście)

**WIZUALIZACJA - GRAF TECHNOLOGII**

31. Główna prezentacja to statyczny graf z zależnościami między technologiami
32. Graf jest klikalny - po kliknięciu pokazuje się popup z możliwością edycji (description, progress, usunięcie)
33. Graf pokazuje ścieżki rozwoju: które technologie są następne, a które po nich
34. Użytkownik buduje graf stopniowo poprzez dodawanie technologii po kliknięciu "Szukaj"

**STACK TECHNOLOGICZNY**

35. Aplikacja webowa w technologiach Microsoft: MVC, Razor Pages lub Blazor (NIE React, NIE Vue)
36. Autentykacja: Google OAuth
37. Brak funkcji importu/eksportu profilu (bez backup'u)
38. AI bez konkretnych ograniczeń źródłowych - może wykorzystać StackOverflow API, GitHub Trends, web scraping, lub gotowe bazy

**HARMONOGRAM I ZASOBY**

39. Timeline: 5 tygodni do wersji BETA
40. Zespół: 1 osoba (solo developer)
41. Budżet AI: standardowe ceny (nie najdroższe, nie najtańsze)

**METRYKI I ANALYTICS**

42. Kluczowe metryki: ilość wygenerowanych technologii przez użytkowników, liczba zaakceptowanych technologii, oznaczony postęp nauki
43. Statystyki admin panel (niski priorytet): liczba użytkowników, najpopularniejsze prefiksy, średni procent wypełnienia profili
44. Kryteria sukcesu MVP: 90% użytkowników z wypełnionym profilem, 75% użytkowników wygenerowało przynajmniej jeden temat do nauki

</decisions>

<matched_recommendations>

**WYSOKIE DOPASOWANIE - KRYTYCZNE DLA MVP**

1. **Onboarding z natychmiastową wartością**: Zaprojektuj wizard onboardingu z 3 krokami (poziom doświadczenia, minimum 5 znanych technologii, automatyczne pierwsze rekomendacje) - maksymalny czas wypełnienia: 3 minuty. ✅ Dopasowane do decyzji 15, 21

2. **Stack technologiczny Microsoft**: Zaimplementuj ASP.NET Core z Blazor Server - najszybsze do prototypowania dla solo developera, nie wymaga oddzielnego frontendu, łatwa integracja z Entity Framework. ✅ Dopasowane do decyzji 35, 39, 40

3. **Biblioteka grafów**: Użyj gotowej biblioteki do wizualizacji grafów (vis.js dla statycznych grafów, lub Blazor-kompatybilnej biblioteki) zamiast pisać własną - zaoszczędzi tydzień pracy przy 5-tygodniowym timeline. ✅ Dopasowane do decyzji 31-33, 39

4. **Struktura danych dla technologii**: Zaprojektuj model zawierający: ID, Nazwa, Prefix (kategoria główna), Tag (typ: technologia/framework/baza), Opis (krótki + prywatny użytkownika), Status (TODO/Completed/Ignored), Procent_Postępu. ✅ Dopasowane do decyzji 16-19, 26

5. **AI Prompt Engineering**: Stwórz szczegółowy prompt dla AI zawierający: profil użytkownika (główne tech, rola, obszar), wszystkie znane technologie, wybraną technologię do rozwinięcia, oraz żądanie zwrócenia JSON z 5 technologiami wraz z polem "reasoning". ✅ Dopasowane do decyzji 4, 6, 8, 24

6. **Dwa główne widoki**: (1) "Learning Dashboard" - grid z listą TODO z suwakami postępu i sortowaniem, (2) "Technology Graph" - wizualizacja zależności z klikalnym popup do edycji. ✅ Dopasowane do decyzji 27, 29, 31-33

7. **Google OAuth**: Implementuj Google OAuth dla autentykacji - szybkie w realizacji, sprawdzone, odpowiednie dla MVP. ✅ Dopasowane do decyzji 36, 39

8. **System kategorii/tagów**: Implementuj system prefiksów (DotNet, Java, JavaScript, Python, Database, DevOps, Tools) oraz tagów (Technologia, Framework, Baza danych) dla łatwiejszej organizacji. ✅ Dopasowane do decyzji 16-18, 20

**ŚREDNIE DOPASOWANIE - WAŻNE DLA FUNKCJONALNOŚCI**

9. **Caching rekomendacji AI**: Rozważ zapisywanie wygenerowanych przez AI rekomendacji w bazie, aby nie generować ponownie dla tej samej konfiguracji - oszczędność kosztów i czasu. ⚠️ Częściowo dopasowane - użytkownik nie wspomniał, ale nie sprzeciwił się

10. **Learning Path automatyzacja**: Gdy użytkownik zaznaczy 100% postępu w technologii, system może automatycznie zaproponować generowanie kolejnych technologii. ✅ Dopasowane do decyzji 25

11. **Sekcja "Ignore List"**: Stwórz przeglądalnąi edytowalną sekcję dla ignorowanych technologii, która pokazuje je jako drugą listę poniżej głównych rekomendacji. ✅ Dopasowane do decyzji 23, 27, 28

12. **Spinner i timeout**: Implementuj loader/spinner podczas generowania przez AI z timeout 20 sekund i komunikatem błędu po przekroczeniu. ✅ Dopasowane do decyzji 9

13. **Admin analytics**: Prosty admin panel (może być hardcoded tylko dla Twojego konta) z podstawowymi metrykami - niski priorytet, ale pomocny do weryfikacji kryteriów sukcesu. ✅ Dopasowane do decyzji 43, 44

**NISKIE DOPASOWANIE - ODRZUCONE LUB NIEISTOTNE**

14. ❌ Import/eksport profilu - użytkownik wyraźnie odrzucił (decyzja 37)

15. ❌ Limit generowań dziennie (anti-spam) - użytkownik wyraźnie odrzucił (decyzja 10)

16. ❌ Ocenianie sugestii AI przez użytkowników - nie będzie w MVP

17. ❌ Gotowe API do trendów (bez AI) - odrzucone, musi być AI (decyzja 38)

18. ❌ Auto-complete dla technologii - odrzucone (decyzja 4, 24)

19. ❌ Bazowa pula technologii w DB - AI generuje dynamicznie

20. ❌ Regeneracja grafu po zmianie profilu - wyraźnie odrzucone (decyzja 13)

</matched_recommendations>

<prd_planning_summary>

## Podsumowanie Planowania PRD dla MVP - DeveloperGoals

### 1. GŁÓWNY PROBLEM I WIZJA PRODUKTU

**Problem**: Programiści muszą być na bieżąco z najnowszymi technologiami, ale nie wiedzą w którym kierunku się rozwijać lub jakie technologie pasują do ich obecnej wiedzy i doświadczenia.

**Rozwiązanie**: Aplikacja webowa wykorzystująca AI do inteligentnego rekomendowania kolejnych technologii, frameworków i narzędzi do nauki, bazując na aktualnej wiedzy użytkownika, jego specjalizacji i trendach rynkowych.

**Unikalna Wartość**: Prostota aplikacji, łatwe zarządzanie, czytelna i intuicyjna wizualizacja ścieżek rozwoju poprzez graf technologii.

### 2. KLUCZOWE WYMAGANIA FUNKCJONALNE

#### A. System Profilu Użytkownika
- **Podstawowe dane profilu**:
  - Główne technologie (Java, .NET, JavaScript, Python itp.)
  - Rola: Programista / Tester / Analityk
  - Obszar: User Interface / Backend / Full Stack
- **Edytowalność**: Użytkownik może modyfikować profil w dowolnym momencie
- **Onboarding**: Prosty 3-minutowy proces wypełniania profilu z natychmiastowymi rekomendacjami

#### B. System Technologii
- **Model danych technologii**:
  - Nazwa z prefiksem linii rozwoju (np. "DotNet - Entity Framework")
  - Prefix/Kategoria (DotNet, Java, JavaScript, Python, Database, DevOps, Tools)
  - Tag typu (Technologia / Framework / Baza danych)
  - Opis systemowy (krótki, generowany przez AI)
  - Opis prywatny (edytowalny przez użytkownika)
  - Status (TODO / Completed / Ignored)
  - Procent postępu (0-100%, suwak)
  
- **Źródła technologii**: 
  - AI generuje technologie dynamicznie
  - Użytkownik może dodać własne niestandardowe technologie

#### C. Mechanizm Rekomendacji AI
- **Trigger**: Użytkownik klika przycisk "Szukaj nowych technologii" przy wybranej technologii
- **Input dla AI**: 
  - Pełny profil użytkownika
  - Wszystkie znane technologie użytkownika
  - Kontekst: wybrana technologia, od której ma szukać kolejnych
- **Output AI**: 
  - Lista ~5 kolejnych rekomendowanych technologii
  - Dla każdej: nazwa, prefix, tag, opis, reasoning (dlaczego polecona)
- **Źródła danych AI**: Trendy rynkowe, GitHub Trends, ogłoszenia o pracę, StackOverflow
- **Parametry techniczne**: 
  - Timeout: 20 sekund
  - UI: Spinner podczas ładowania
  - Brak limitów użycia (no anti-spam)

#### D. Graf Technologii (Główny Widok)
- **Typ**: Statyczna wizualizacja z klikalną interakcją
- **Funkcjonalność**:
  - Pokazuje zależności między technologiami (które są następne w ścieżce rozwoju)
  - Graf budowany stopniowo przez użytkownika poprzez dodawanie technologii
  - Kliknięcie węzła → popup z opcjami:
    - Edycja description (prywatnego)
    - Zmiana postępu (suwak %)
    - Usunięcie technologii
    - Przycisk "Szukaj nowych technologii"

#### E. Learning Dashboard
- **Lista TODO**:
  - Grid z technologiami w statusie TODO
  - Wyświetla: nazwa, prefix, tag, procent postępu, opis
  - Sortowalny przez użytkownika
  - Domyślne sortowanie: według % postępu (więcej % = wyżej)
  
- **Lista "Ignorowane"**:
  - Oddzielna sekcja poniżej
  - Przeglądalna i edytowalna
  - Użytkownik może przywrócić technologie z tej listy

- **Akcje użytkownika**:
  - Dodaj do TODO
  - Dodaj do Ignore
  - Zmień postęp (suwak 0-100%)
  - Usuń technologię

#### F. System Autentykacji
- Google OAuth
- Każdy użytkownik ma własny profil i graf technologii
- Brak funkcji backup/import/export

### 3. KLUCZOWE HISTORIE UŻYTKOWNIKA

**Historia 1: Nowy użytkownik - Onboarding**
```
JAKO nowy użytkownik  
CHCĘ szybko wypełnić profil i otrzymać pierwsze rekomendacje  
ABY od razu zobaczyć wartość aplikacji

KROKI:
1. Użytkownik loguje się przez Google OAuth
2. Wypełnia profil (główne technologie, rola, obszar) - max 3 minuty
3. System automatycznie generuje pierwsze rekomendacje AI
4. Użytkownik widzi ~5 technologii do rozważenia
5. Wybiera interesujące → dodaje do TODO
6. Odrzuca nieinteresujące → dodaje do Ignore

REZULTAT: Użytkownik ma wypełniony profil i pierwszą listę TODO
```

**Historia 2: Eksploracja ścieżki rozwoju**
```
JAKO programista .NET  
CHCĘ zobaczyć jakie technologie mogę nauczyć się po Entity Framework  
ABY zaplanować swoją ścieżkę rozwoju

KROKI:
1. Użytkownik ma w profilu "DotNet - Entity Framework" jako znaną technologię
2. Klika na tę technologię w grafie
3. W popupie klika "Szukaj nowych technologii"
4. Widzi spinner (max 20s)
5. AI generuje 5 powiązanych technologii, np.:
   - DotNet - Dapper (lżejsze ORM)
   - DotNet - NHibernate (zaawansowane ORM)
   - Database - Query Optimization
   - Backend - Repository Pattern
   - DotNet - CQRS with EF Core
6. Dla każdej technologii widzi krótki opis i reasoning
7. Dodaje wybrane do TODO, resztę może zignorować

REZULTAT: Graf rozrasta się o nowe węzły, użytkownik ma jasną ścieżkę
```

**Historia 3: Zarządzanie postępem nauki**
```
JAKO użytkownik uczący się nowej technologii  
CHCĘ śledzić swój postęp w nauce  
ABY mieć motywację i widzieć swój rozwój

KROKI:
1. Użytkownik ma technologie na liście TODO
2. Otwiera Learning Dashboard
3. Widzi grid z technologiami i suwakami postępu
4. Uczy się "DotNet - Dapper"
5. Co jakiś czas aktualizuje suwak: 20%, 50%, 80%, 100%
6. Grid sortuje się automatycznie (więcej % = wyżej)
7. Przy 100% użytkownik może kliknąć "Szukaj kolejnych" dla tej technologii

REZULTAT: Użytkownik ma przejrzysty widok swojego postępu
```

**Historia 4: Zmiana specjalizacji**
```
JAKO użytkownik zmieniający specjalizację  
CHCĘ zaktualizować swój profil  
ABY otrzymać rekomendacje w nowym obszarze

KROKI:
1. Użytkownik był Backend Developer w .NET
2. Decyduje się przejść na Full Stack
3. Edytuje profil: zmienia obszar na "Full Stack"
4. Dodaje nowe główne technologie: JavaScript, React (własne wpisy)
5. Klika na "JavaScript" w grafie → "Szukaj nowych"
6. Otrzymuje rekomendacje frontend'owe dopasowane do jego doświadczenia
7. Stary graf .NET pozostaje nienaruszony

REZULTAT: Graf rozszerza się o nowy obszar, stare dane są zachowane
```

**Historia 5: Zarządzanie ignorowanymi technologiami**
```
JAKO junior developer  
CHCĘ móc przywrócić wcześniej zignorowane technologie  
ABY móc do nich wrócić gdy będę bardziej zaawansowany

KROKI:
1. Rok temu użytkownik zignorował "Kubernetes" (za trudne)
2. Teraz jest bardziej doświadczony
3. Otwiera sekcję "Ignorowane" poniżej głównej listy
4. Widzi wszystkie technologie z listy Ignore
5. Znajduje "DevOps - Kubernetes"
6. Klika "Przywróć" lub "Dodaj do TODO"
7. Technologia przenosi się na listę TODO

REZULTAT: Użytkownik może elastycznie zarządzać swoimi preferencjami
```

### 4. KRYTERIA SUKCESU I METRYKI

#### Kryteria Sukcesu MVP (KPI):
1. **90% użytkowników** posiada wypełnioną sekcję Profil Użytkownika
2. **75% użytkowników** wygenerowało przynajmniej jeden temat do nauki

#### Metryki Śledzone:
1. **Liczba użytkowników** (aktywnych, zarejestrownych)
2. **Ilość wygenerowanych technologii** przez AI (total)
3. **Liczba zaakceptowanych technologii** (dodanych do TODO)
4. **Oznaczony postęp nauki** (średni % postępu, liczba ukończonych)
5. **Najpopularniejsze prefiksy** (które linie rozwoju są najczęściej wybierane)
6. **Średni procent wypełnienia profili** (weryfikacja KPI #1)

#### Admin Panel (Niski Priorytet):
- Dashboard z powyższymi metrykami
- Dostęp tylko dla dewelopera (hardcoded)
- Przydatny do monitorowania sukcesu MVP

### 5. OGRANICZENIA I WYMAGANIA NIEFUNKCJONALNE

#### Techniczne:
- **Stack**: ASP.NET Core z MVC/Razor Pages/Blazor (wybór jednego)
- **Autentykacja**: Google OAuth
- **Baza danych**: Entity Framework (SQL Server / PostgreSQL)
- **AI Integration**: API do modelu AI (OpenAI, Claude, lub podobne) - standardowe ceny
- **Graf**: Biblioteka do wizualizacji grafów kompatybilna z Blazorem (vis.js, cytoscape.js)
- **Timeout AI**: Maksymalnie 20 sekund na generowanie
- **Brak**: Import/export, backup, external API bez AI

#### Czasowe:
- **Timeline**: 5 tygodni do wersji BETA
- **Zespół**: 1 developer (solo)
- **Priorytet**: Szybki prototyp, sprawdzone technologie, minimum custom code

#### Biznesowe:
- **Budżet AI**: Standardowy (nie premium, nie freemium)
- **Brak limitów**: Użytkownicy mogą generować rekomendacje bez ograniczeń
- **Bez monetyzacji w MVP**: Focus na walidację produktu

### 6. ARCHITEKTURA I PRZEPŁYW DANYCH

```
[Użytkownik] 
    ↓ Logowanie
[Google OAuth] → [ASP.NET Backend]
    ↓
[Profile Database]
    ↓ Wypełnienie profilu
[AI Service]
    ↓ Request: Profil + Technologie + Wybrana Tech
    ↓ (20s timeout)
    ↓ Response: 5 technologii + reasoning
[Technology Graph + Dashboard]
    ↓ Użytkownik wybiera
[TODO List / Ignore List]
    ↓ Śledzenie postępu
[Progress Tracking]
```

### 7. POZA ZAKRESEM MVP (Przyszłe Iteracje)

- Przechowywanie URL do źródeł wiedzy
- Udostępnianie celów innym użytkownikom
- Funkcje społecznościowe
- Automatyczna aktualizacja rekomendacji po zmianie profilu
- Limity anti-spam
- Import/Export profilu
- Natywne aplikacje mobilne
- Ocenianie jakości rekomendacji AI
- Zaawansowane analytics
- System osiągnięć/gamifikacji

</prd_planning_summary>

<unresolved_issues>

### Kwestie Wymagające Decyzji Przed Rozpoczęciem Implementacji:

1. **Wybór konkretnej technologii Microsoft**:
   - MVC vs Razor Pages vs Blazor Server vs Blazor WebAssembly?
   - **Zalecenie**: Blazor Server (najbardziej nowoczesne, reaktywne, idealne dla grafów)
   
2. **Konkretny model AI do użycia**:
   - OpenAI GPT-4 / GPT-3.5?
   - Anthropic Claude?
   - Azure OpenAI Service (lepsze dla Microsoft stack)?
   - **Wymaga**: Sprawdzenia cen i limitów API
   
3. **Konkretna biblioteka do grafów**:
   - Blazor.Diagrams?
   - vis.js z JS interop?
   - Cytoscape.js z JS interop?
   - **Wymaga**: Prototyp/POC dla sprawdzenia kompatybilności z Blazorem

4. **Struktura JSON response od AI**:
   - Potrzebne dokładne określenie formatu JSON dla technologii
   - Przykład prompt dla AI
   - Obsługa błędów gdy AI zwróci niepoprawny format
   
5. **Baza danych**:
   - SQL Server (standardowo dla Microsoft stack)?
   - PostgreSQL (open source, często tańsze w hostingu)?
   - **Wymaga**: Decyzji o hostingu (Azure, AWS, inne)

6. **Hosting i deployment**:
   - Azure App Service (droższe, ale integracja z ekosystemem)?
   - Railway / Render (tańsze alternatywy)?
   - **Wymaga**: Oszacowania kosztów miesięcznych

7. **Szczegóły admin panelu**:
   - Osobna strona czy wbudowane w główną aplikację?
   - Czy w MVP czy dopiero po?
   - **Status**: Niski priorytet, można odłożyć

8. **Testowanie**:
   - Czy planowane jest ręczne testowanie czy automatyczne testy jednostkowe?
   - Kto będzie testerem beta (sam developer czy zewnętrzni użytkownicy)?
   - **Wymaga**: Określenia strategii QA dla 5-tygodniowego timeline

### Drobne Kwestie do Doprecyzowania:

- Maksymalna liczba technologii na grafie (performance considerations)
- Czy graf ma mieć automatyczny layout czy użytkownik rozmieszcza węzły?
- Jak obsłużyć sytuację gdy AI nie zwróci odpowiedzi w 20s (retry? komunikat? zaproponowanie cache'owanych)?
- Czy edycja prywatnego opisu technologii powinna być inline czy w popupie?
- Format eksportu danych zgodnie z RODO (nawet jeśli nie ma import/export, backup użytkownika)

</unresolved_issues>

</conversation_summary>

```plaintext
JAKO nowy użytkownik  
CHCĘ szybko wypełnić profil i otrzymać pierwsze rekomendacje  
ABY od razu zobaczyć wartość aplikacji

KROKI:
1. Użytkownik loguje się przez Google OAuth
2. Wypełnia profil (główne technologie, rola, obszar) - max 3 minuty
3. System automatycznie generuje pierwsze rekomendacje AI
4. Użytkownik widzi ~5 technologii do rozważenia
5. Wybiera interesujące → dodaje do TODO
6. Odrzuca nieinteresujące → dodaje do Ignore

REZULTAT: Użytkownik ma wypełniony profil i pierwszą listę TODO
```

```plaintext
JAKO programista .NET  
CHCĘ zobaczyć jakie technologie mogę nauczyć się po Entity Framework  
ABY zaplanować swoją ścieżkę rozwoju

KROKI:
1. Użytkownik ma w profilu "DotNet - Entity Framework" jako znaną technologię
2. Klika na tę technologię w grafie
3. W popupie klika "Szukaj nowych technologii"
4. Widzi spinner (max 20s)
5. AI generuje 5 powiązanych technologii, np.:
   - DotNet - Dapper (lżejsze ORM)
   - DotNet - NHibernate (zaawansowane ORM)
   - Database - Query Optimization
   - Backend - Repository Pattern
   - DotNet - CQRS with EF Core
6. Dla każdej technologii widzi krótki opis i reasoning
7. Dodaje wybrane do TODO, resztę może zignorować

REZULTAT: Graf rozrasta się o nowe węzły, użytkownik ma jasną ścieżkę
```

```plaintext
JAKO użytkownik uczący się nowej technologii  
CHCĘ śledzić swój postęp w nauce  
ABY mieć motywację i widzieć swój rozwój

KROKI:
1. Użytkownik ma technologie na liście TODO
2. Otwiera Learning Dashboard
3. Widzi grid z technologiami i suwakami postępu
4. Uczy się "DotNet - Dapper"
5. Co jakiś czas aktualizuje suwak: 20%, 50%, 80%, 100%
6. Grid sortuje się automatycznie (więcej % = wyżej)
7. Przy 100% użytkownik może kliknąć "Szukaj kolejnych" dla tej technologii

REZULTAT: Użytkownik ma przejrzysty widok swojego postępu
```

```plaintext
JAKO użytkownik zmieniający specjalizację  
CHCĘ zaktualizować swój profil  
ABY otrzymać rekomendacje w nowym obszarze

KROKI:
1. Użytkownik był Backend Developer w .NET
2. Decyduje się przejść na Full Stack
3. Edytuje profil: zmienia obszar na "Full Stack"
4. Dodaje nowe główne technologie: JavaScript, React (własne wpisy)
5. Klika na "JavaScript" w grafie → "Szukaj nowych"
6. Otrzymuje rekomendacje frontend'owe dopasowane do jego doświadczenia
7. Stary graf .NET pozostaje nienaruszony

REZULTAT: Graf rozszerza się o nowy obszar, stare dane są zachowane
```

```plaintext
JAKO junior developer  
CHCĘ móc przywrócić wcześniej zignorowane technologie  
ABY móc do nich wrócić gdy będę bardziej zaawansowany

KROKI:
1. Rok temu użytkownik zignorował "Kubernetes" (za trudne)
2. Teraz jest bardziej doświadczony
3. Otwiera sekcję "Ignorowane" poniżej głównej listy
4. Widzi wszystkie technologie z listy Ignore
5. Znajduje "DevOps - Kubernetes"
6. Klika "Przywróć" lub "Dodaj do TODO"
7. Technologia przenosi się na listę TODO

REZULTAT: Użytkownik może elastycznie zarządzać swoimi preferencjami
```

```plaintext
[Użytkownik] 
    ↓ Logowanie
[Google OAuth] → [ASP.NET Backend]
    ↓
[Profile Database]
    ↓ Wypełnienie profilu
[AI Service]
    ↓ Request: Profil + Technologie + Wybrana Tech
    ↓ (20s timeout)
    ↓ Response: 5 technologii + reasoning
[Technology Graph + Dashboard]
    ↓ Użytkownik wybiera
[TODO List / Ignore List]
    ↓ Śledzenie postępu
[Progress Tracking]
```

