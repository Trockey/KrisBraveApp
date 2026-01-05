# ============================================================================
# Skrypt pomocniczy - wyświetla instrukcje jak uzyskać cookie
# ============================================================================

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  JAK UZYSKAĆ COOKIE DO TESTOW API" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "KROK 1: Uruchom aplikację" -ForegroundColor Green
Write-Host "  dotnet run --project DeveloperGoals/DeveloperGoals/DeveloperGoals.csproj" -ForegroundColor Gray
Write-Host ""

Write-Host "KROK 2: Otwórz przeglądarkę" -ForegroundColor Green
Write-Host "  http://localhost:5057" -ForegroundColor Gray
Write-Host ""

Write-Host "KROK 3: Zaloguj się przez Google OAuth" -ForegroundColor Green
Write-Host "  Kliknij przycisk logowania i zaloguj się przez Google" -ForegroundColor Gray
Write-Host ""

Write-Host "KROK 4: Otwórz DevTools" -ForegroundColor Green
Write-Host "  Naciśnij F12 lub Ctrl+Shift+I" -ForegroundColor Gray
Write-Host ""

Write-Host "KROK 5: Znajdź cookie" -ForegroundColor Green
Write-Host "  Chrome/Edge:" -ForegroundColor White
Write-Host "    -> Zakładka 'Application' (Chrome) lub 'Storage' (Edge)" -ForegroundColor Gray
Write-Host "    -> Rozwiń 'Cookies'" -ForegroundColor Gray
Write-Host "    -> Kliknij 'http://localhost:5057'" -ForegroundColor Gray
Write-Host "    -> Znajdź cookie: '.AspNetCore.Cookies'" -ForegroundColor Gray
Write-Host ""
Write-Host "  Firefox:" -ForegroundColor White
Write-Host "    -> Zakładka 'Storage'" -ForegroundColor Gray
Write-Host "    -> Rozwiń 'Cookies' -> 'http://localhost:5057'" -ForegroundColor Gray
Write-Host "    -> Znajdź cookie: '.AspNetCore.Cookies'" -ForegroundColor Gray
Write-Host ""

Write-Host "KROK 6: Skopiuj wartość cookie" -ForegroundColor Green
Write-Host "  Kliknij dwukrotnie na wartość (Value) cookie" -ForegroundColor Gray
Write-Host "  Skopiuj całą wartość (może być bardzo długa!)" -ForegroundColor Gray
Write-Host ""

Write-Host "KROK 7: Użyj w skrypcie TestAI.sh" -ForegroundColor Green
Write-Host "  Otwórz plik: .other/Postman/TestAI.sh" -ForegroundColor Gray
Write-Host "  Znajdź linię:" -ForegroundColor Gray
Write-Host "    COOKIE_HEADER=`"Cookie: .AspNetCore.Cookies=TWÓJ_COOKIE_TUTAJ`"" -ForegroundColor Yellow
Write-Host "  Zamień 'TWÓJ_COOKIE_TUTAJ' na skopiowaną wartość" -ForegroundColor Gray
Write-Host ""

Write-Host "PRZYKŁAD:" -ForegroundColor Cyan
Write-Host '  COOKIE_HEADER="Cookie: .AspNetCore.Cookies=CfDJ8N...długa_wartość...xyz123"' -ForegroundColor Green
Write-Host ""

Write-Host "UWAGI:" -ForegroundColor Yellow
Write-Host "  - Cookie wygasa po pewnym czasie (zwykle kilka godzin)" -ForegroundColor White
Write-Host "  - Jeśli otrzymujesz 401 Unauthorized, zaloguj się ponownie" -ForegroundColor White
Write-Host "  - Cookie jest specyficzne dla sesji - każde logowanie tworzy nowy cookie" -ForegroundColor White
Write-Host ""

Write-Host "POMOC:" -ForegroundColor Cyan
Write-Host "  Szczegółowa instrukcja: .other/Postman/INSTRUKCJA_COOKIE.md" -ForegroundColor Gray
Write-Host ""

# Opcjonalnie: otwórz przeglądarkę
$openBrowser = Read-Host "Czy chcesz otworzyć przeglądarkę teraz? (T/N)"
if ($openBrowser -eq "T" -or $openBrowser -eq "t") {
    Start-Process "http://localhost:5057"
    Write-Host ""
    Write-Host "Przeglądarka otwarta! Po zalogowaniu użyj F12, aby otworzyć DevTools." -ForegroundColor Green
}

Write-Host ""
