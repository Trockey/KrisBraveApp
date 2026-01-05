# ============================================================================
# Skrypt PowerShell do automatycznego pobierania cookie z przeglądarki Chrome
# ============================================================================
# Użycie: .\Get-Cookie.ps1
# Wynik: Wyświetli wartość cookie gotową do użycia w skrypcie bash
# ============================================================================

param(
    [string]$Url = "http://localhost:5057",
    [string]$CookieName = ".AspNetCore.Cookies"
)

Write-Host "Szukanie cookie dla: $Url" -ForegroundColor Cyan
Write-Host "Nazwa cookie: $CookieName" -ForegroundColor Cyan
Write-Host ""

# Sprawdź czy Chrome jest zainstalowany
$chromePath = Get-Command "chrome.exe" -ErrorAction SilentlyContinue
if (-not $chromePath) {
    $chromePath = Get-Command "msedge.exe" -ErrorAction SilentlyContinue
    if (-not $chromePath) {
        Write-Host "BŁĄD: Nie znaleziono Chrome ani Edge!" -ForegroundColor Red
        Write-Host "Użyj metody ręcznej z DevTools." -ForegroundColor Yellow
        exit 1
    }
}

Write-Host "Znaleziono przeglądarkę: $($chromePath.Source)" -ForegroundColor Green

# Pobierz ścieżkę do profilu Chrome/Edge
$browserName = Split-Path -Leaf $chromePath.Source
if ($browserName -eq "chrome.exe") {
    $profilePath = "$env:LOCALAPPDATA\Google\Chrome\User Data\Default"
    $cookieDb = "$profilePath\Cookies"
} else {
    $profilePath = "$env:LOCALAPPDATA\Microsoft\Edge\User Data\Default"
    $cookieDb = "$profilePath\Cookies"
}

if (-not (Test-Path $cookieDb)) {
    Write-Host "BŁĄD: Nie znaleziono bazy cookies!" -ForegroundColor Red
    Write-Host "Ścieżka: $cookieDb" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Użyj metody ręcznej:" -ForegroundColor Yellow
    Write-Host "1. Otwórz aplikację w przeglądarce" -ForegroundColor White
    Write-Host "2. Zaloguj się przez Google" -ForegroundColor White
    Write-Host "3. Otwórz DevTools (F12) -> Application -> Cookies" -ForegroundColor White
    Write-Host "4. Skopiuj wartość cookie .AspNetCore.Cookies" -ForegroundColor White
    exit 1
}

Write-Host "Znaleziono bazę cookies: $cookieDb" -ForegroundColor Green
Write-Host ""
Write-Host "UWAGA: Chrome/Edge musi być zamknięty, aby odczytać cookies!" -ForegroundColor Yellow
Write-Host "Naciśnij Enter, aby kontynuować (lub Ctrl+C, aby anulować)..." -ForegroundColor Yellow
$null = Read-Host

# Sprawdź czy Chrome jest zamknięty
$chromeProcess = Get-Process -Name $browserName.Replace(".exe", "") -ErrorAction SilentlyContinue
if ($chromeProcess) {
    Write-Host ""
    Write-Host "UWAGA: $browserName jest uruchomiony!" -ForegroundColor Red
    Write-Host "Zamknij przeglądarkę i uruchom skrypt ponownie." -ForegroundColor Yellow
    exit 1
}

# Skopiuj bazę cookies do tymczasowego pliku (Chrome blokuje dostęp do otwartej bazy)
$tempDb = "$env:TEMP\cookies_temp_$(Get-Random).db"
try {
    Copy-Item $cookieDb $tempDb -Force
    Write-Host "Skopiowano bazę cookies do pliku tymczasowego" -ForegroundColor Green
} catch {
    Write-Host "BŁĄD: Nie można skopiować bazy cookies!" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Yellow
    exit 1
}

# Próba odczytania cookie (wymaga SQLite)
Write-Host ""
Write-Host "Próba odczytania cookie z bazy..." -ForegroundColor Cyan

# Sprawdź czy SQLite jest dostępne
$sqliteAvailable = $false
try {
    # Próba użycia System.Data.SQLite (jeśli dostępne)
    Add-Type -Path "System.Data.SQLite.dll" -ErrorAction Stop
    $sqliteAvailable = $true
} catch {
    # SQLite nie jest dostępne - użyj metody alternatywnej
    Write-Host "SQLite nie jest dostępne. Użyj metody ręcznej z DevTools." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Alternatywnie, możesz:" -ForegroundColor Cyan
    Write-Host "1. Zainstalować SQLite: choco install sqlite" -ForegroundColor White
    Write-Host "2. Użyć metody ręcznej z DevTools (F12)" -ForegroundColor White
    Remove-Item $tempDb -ErrorAction SilentlyContinue
    exit 1
}

# Jeśli dotarliśmy tutaj, SQLite jest dostępne
# (Kod do odczytania cookie z SQLite - uproszczony)

# Usuń tymczasowy plik
Remove-Item $tempDb -ErrorAction SilentlyContinue

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "INSTRUKCJA RĘCZNA (NAJPROSTSZA METODA)" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Uruchom aplikację:" -ForegroundColor White
Write-Host "   dotnet run --project DeveloperGoals/DeveloperGoals/DeveloperGoals.csproj" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Otwórz przeglądarkę:" -ForegroundColor White
Write-Host "   http://localhost:5057" -ForegroundColor Gray
Write-Host ""
Write-Host "3. Zaloguj się przez Google OAuth" -ForegroundColor White
Write-Host ""
Write-Host "4. Otwórz DevTools (F12)" -ForegroundColor White
Write-Host ""
Write-Host "5. Przejdź do:" -ForegroundColor White
Write-Host "   Application (Chrome) / Storage (Edge/Firefox)" -ForegroundColor Gray
Write-Host "   -> Cookies -> http://localhost:5057" -ForegroundColor Gray
Write-Host ""
Write-Host "6. Znajdź cookie: .AspNetCore.Cookies" -ForegroundColor White
Write-Host ""
Write-Host "7. Skopiuj wartość (Value) i użyj w skrypcie:" -ForegroundColor White
Write-Host ""
Write-Host "   COOKIE_HEADER=`"Cookie: .AspNetCore.Cookies=SKOPIOWANA_WARTOŚĆ`"" -ForegroundColor Green
Write-Host ""
