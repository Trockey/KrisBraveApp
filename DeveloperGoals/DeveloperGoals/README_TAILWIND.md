# Tailwind CSS + Flowbite - Instrukcje

## Instalacja

Po sklonowaniu repozytorium, zainstaluj zależności npm:

```bash
cd DeveloperGoals/DeveloperGoals
npm install
```

## Budowanie CSS

### Tryb produkcyjny (minifikacja)
```bash
npm run build:css
```

### Tryb deweloperski (watch mode)
```bash
npm run watch:css
```

W trybie watch, Tailwind będzie automatycznie przebudowywał CSS przy każdej zmianie w plikach Razor.

## Struktura plików

- `Styles/app.css` - Źródłowy plik CSS z dyrektywami Tailwind
- `wwwroot/css/app.css` - Wygenerowany plik CSS (nie commitować)
- `tailwind.config.js` - Konfiguracja Tailwind CSS
- `postcss.config.js` - Konfiguracja PostCSS

## Używanie Tailwind CSS

### Klasy utility
Używaj klas Tailwind bezpośrednio w komponentach Razor:

```razor
<div class="bg-white border border-gray-200 rounded-lg shadow-sm p-6">
    <h1 class="text-3xl font-bold text-gray-900 mb-4">Tytuł</h1>
    <p class="text-gray-600">Treść</p>
</div>
```

### Komponenty Flowbite
Flowbite dostarcza gotowe komponenty UI. Przykład dropdownu:

```razor
<button data-dropdown-toggle="dropdown-menu" class="...">
    Menu
</button>
<div id="dropdown-menu" class="hidden ...">
    <!-- Zawartość -->
</div>
```

### Ikony Bootstrap Icons
Ikony są dostępne przez CDN:

```razor
<i class="bi bi-person-circle text-lg"></i>
```

## Kolory projektu

Projekt używa niestandardowej palety primary (niebieski):

- `primary-50` do `primary-950`
- Główny kolor: `primary-600`
- Hover: `primary-700`

## Responsive Design

Tailwind używa mobile-first approach:

- Domyślnie: mobile
- `sm:` - 640px+
- `md:` - 768px+
- `lg:` - 1024px+
- `xl:` - 1280px+

Przykład:
```razor
<div class="w-full md:w-1/2 lg:w-1/3">
    <!-- Responsive width -->
</div>
```

## Debugowanie

Jeśli style nie działają:

1. Sprawdź czy plik `wwwroot/css/app.css` istnieje
2. Uruchom `npm run build:css`
3. Sprawdź czy ścieżka w `tailwind.config.js` zawiera wszystkie pliki Razor
4. Upewnij się, że używasz klas Tailwind (nie własnych CSS)

## Dokumentacja

- [Tailwind CSS](https://tailwindcss.com/docs)
- [Flowbite](https://flowbite.com/docs/getting-started/introduction/)
- [Bootstrap Icons](https://icons.getbootstrap.com/)
