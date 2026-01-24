# Tech Stack Frontend - DeveloperGoals

**Wersja:** 1.0  
**Data:** 24 stycznia 2026  
**Status:** AKTUALNY

---

## 📋 Przegląd

Ten dokument opisuje kompleksowy stack technologiczny warstwy frontendowej aplikacji DeveloperGoals. Projekt wykorzystuje **Blazor Web App** jako główny framework, co pozwala na programowanie zarówno frontendu jak i backendu w jednym języku - **C#**.

---

## 🎯 Główny Framework

### ASP.NET Core Blazor Web App (.NET 9)

- **Wersja:** .NET 9.0
- **Typ:** Blazor Web App z Interactive Auto
- **Model renderowania:** 
  - **Blazor Server** - pierwsze ładowanie (szybki start przez SignalR)
  - **Blazor WebAssembly** - kolejne wizyty (działa offline)
- **Język programowania:** C# (jeden język dla całej aplikacji)
- **Pliki komponentów:** `.razor`

**Zalety:**
- Jeden język (C#) dla całej aplikacji
- Minimalna ilość JavaScript (tylko dla bibliotek zewnętrznych)
- Szybki start dzięki Blazor Server
- Możliwość pracy offline dzięki WebAssembly
- Pełna integracja z ekosystemem .NET

---

## 🎨 Stylowanie i UI Framework

### Tailwind CSS 3.4.1

- **Wersja:** 3.4.1
- **Typ:** Utility-first CSS framework
- **Konfiguracja:** Plik `tailwind.config.js`
- **Build:** PostCSS z Autoprefixer

**Funkcjonalności:**
- Utility classes dla szybkiego stylowania
- Responsywność (sm:, md:, lg:, xl:, 2xl:)
- Dark mode support (dark: variant)
- Customizable theme
- Arbitrary values (np. `w-[123px]`)

**Pliki:**
- Input: `./Styles/app.css`
- Output: `./wwwroot/css/app.css`

**Skrypty npm:**
```json
"build:css": "npx tailwindcss -i ./Styles/app.css -o ./wwwroot/css/app.css --minify"
"watch:css": "npx tailwindcss -i ./Styles/app.css -o ./wwwroot/css/app.css --watch"
```

---

### Flowbite 2.2.1

- **Wersja:** 2.2.1
- **Typ:** Component library oparta na Tailwind CSS
- **Zastosowanie:** Gotowe komponenty UI (przyciski, formularze, modale, nawigacja, etc.)

**Komponenty dostępne:**
- Buttons
- Forms (inputs, selects, checkboxes, radios)
- Modals
- Dropdowns
- Navigation
- Cards
- Alerts
- I wiele innych

**Integracja:** Biblioteka JavaScript ładowana przez Blazor JS Interop

---

### Bootstrap Icons 1.11

- **Wersja:** 1.11
- **Typ:** Biblioteka ikon
- **Zastosowanie:** Ikony w całej aplikacji
- **Format:** SVG icons

---

## 📊 Wizualizacja Grafów

### vis.js

- **Typ:** Biblioteka JavaScript do wizualizacji grafów
- **Zastosowanie:** Wizualizacja grafu technologii (węzły i krawędzie)
- **Integracja:** Przez Blazor JavaScript Interop

**Funkcjonalności:**
- Interaktywne grafy sieciowe
- Zoom i pan
- Klikalne węzły
- Dynamiczne dodawanie/usuwanie węzłów i krawędzi
- Custom styling węzłów

**Alternatywa:** cytoscape.js (zarezerwowana jako backup)

---

## 🛠️ Narzędzia Build i Development

### PostCSS 8.4.35

- **Wersja:** 8.4.35
- **Zastosowanie:** Przetwarzanie CSS (Tailwind CSS)
- **Pluginy:** 
  - Tailwind CSS
  - Autoprefixer

---

### Autoprefixer 10.4.17

- **Wersja:** 10.4.17
- **Zastosowanie:** Automatyczne dodawanie vendor prefixes do CSS
- **Integracja:** Jako plugin PostCSS

---

## 📦 Struktura Zależności

### package.json

```json
{
  "name": "developergoals",
  "version": "1.0.0",
  "description": "DeveloperGoals - Blazor App with Tailwind CSS and Flowbite",
  "scripts": {
    "build:css": "npx tailwindcss -i ./Styles/app.css -o ./wwwroot/css/app.css --minify",
    "watch:css": "npx tailwindcss -i ./Styles/app.css -o ./wwwroot/css/app.css --watch"
  },
  "devDependencies": {
    "tailwindcss": "^3.4.1",
    "autoprefixer": "^10.4.17",
    "postcss": "^8.4.35"
  },
  "dependencies": {
    "flowbite": "^2.2.1"
  }
}
```

---

## 🏗️ Architektura Frontend

### Komponenty Blazor

- **Lokalizacja:** `DeveloperGoals/Components/`
- **Strony:** `Components/Pages/`
- **Layout:** `Components/Layout/`
- **Format:** `.razor` files

### Struktura katalogów:

```
DeveloperGoals/
├── DeveloperGoals/              # Projekt Server
│   ├── Components/              # Komponenty Blazor
│   │   ├── Pages/               # Strony aplikacji
│   │   └── Layout/              # Layouty
│   ├── Styles/                  # Pliki źródłowe CSS
│   │   └── app.css              # Główny plik Tailwind
│   └── wwwroot/                 # Statyczne pliki
│       └── css/
│           └── app.css          # Skompilowany CSS
│
└── DeveloperGoals.Client/       # Projekt WebAssembly (opcjonalny)
    └── Pages/                   # Komponenty WASM
```

---

## 🔧 Konfiguracja

### Tailwind CSS Config

Plik `tailwind.config.js` powinien zawierać:
- Content paths (gdzie szukać klas Tailwind)
- Theme customization
- Plugins (np. Flowbite plugin)
- Dark mode configuration

### PostCSS Config

Plik `postcss.config.js` powinien zawierać:
- Tailwind CSS plugin
- Autoprefixer plugin

---

## 🚀 Workflow Development

### 1. Rozwój CSS

```bash
# Watch mode - automatyczna kompilacja przy zmianach
npm run watch:css

# Build production - minifikacja
npm run build:css
```

### 2. Rozwój Blazor

```bash
# Uruchomienie aplikacji z hot reload
dotnet watch run
```

### 3. Integracja JavaScript (vis.js)

- Biblioteka vis.js ładowana przez `<script>` tag w `_Host.cshtml` lub `App.razor`
- Interakcja przez `IJSRuntime` w komponentach Blazor
- Przykład użycia JS Interop dla vis.js

---

## 📝 Najlepsze Praktyki

### Stylowanie

1. **Używaj Tailwind utility classes** zamiast custom CSS gdzie to możliwe
2. **Wykorzystuj Flowbite components** dla standardowych elementów UI
3. **Używaj @layer directive** do organizacji custom styles
4. **Implementuj dark mode** z wykorzystaniem `dark:` variant

### Komponenty Blazor

1. **Funkcjonalne komponenty** z hooks zamiast klas
2. **Separacja logiki** - użyj serwisów dla logiki biznesowej
3. **JS Interop** tylko gdy konieczne (np. vis.js)
4. **State management** przez Scoped Services

### Performance

1. **Minifikacja CSS** w produkcji
2. **Code splitting** przez Blazor WebAssembly
3. **Lazy loading** komponentów gdzie możliwe
4. **Optymalizacja obrazów** (jeśli używane)

---

## 🔗 Integracje

### JavaScript Interop

- **vis.js** - wizualizacja grafów
- **Flowbite** - komponenty UI (jeśli wymagają JS)

### Backend Integration

- **SignalR** - dla Blazor Server (automatyczny)
- **HTTP Client** - dla API calls
- **Entity Framework Core** - dostęp do danych

---

## 📚 Dokumentacja i Zasoby

### Oficjalna Dokumentacja

- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
- [Flowbite Documentation](https://flowbite.com/docs/getting-started/introduction/)
- [vis.js Documentation](https://visjs.org/)
- [Bootstrap Icons](https://icons.getbootstrap.com/)

### Projekt-Specific

- `.ai/prd.md` - Dokument wymagań produktu
- `.ai/22-ui-plan.md` - Plan architektury UI
- `.ai/23-all-view-implementation-plan.md` - Plan implementacji widoków

---

## ⚠️ Uwagi

1. **Brak TypeScript/JavaScript** - projekt celowo unika JS/TS, używając tylko C#
2. **JavaScript tylko dla bibliotek zewnętrznych** - vis.js i Flowbite wymagają JS
3. **Blazor Interactive Auto** - automatyczne przełączanie między Server a WebAssembly
4. **Responsywność** - projekt jest responsywny, ale MVP skupia się na Desktop

---

## 🔄 Aktualizacje i Wersje

| Komponent | Wersja | Data Aktualizacji |
|-----------|--------|-------------------|
| .NET | 9.0 | 2026-01-24 |
| Tailwind CSS | 3.4.1 | 2026-01-24 |
| Flowbite | 2.2.1 | 2026-01-24 |
| Bootstrap Icons | 1.11 | 2026-01-24 |
| PostCSS | 8.4.35 | 2026-01-24 |
| Autoprefixer | 10.4.17 | 2026-01-24 |

---

**Ostatnia aktualizacja:** 24 stycznia 2026  
**Autor:** Dokumentacja techniczna projektu DeveloperGoals
