using Microsoft.Playwright;

namespace DeveloperGoals.E2ETests.Helpers;

/// <summary>
/// Helper do obsługi autentykacji testowej w testach E2E
/// </summary>
public static class TestAuthHelper
{
    /// <summary>
    /// Loguje użytkownika testowego (Id=2) używając parametru URL test=true
    /// </summary>
    /// <param name="page">Strona Playwright</param>
    /// <param name="baseUrl">Bazowy URL aplikacji</param>
    public static async Task LoginAsTestUserAsync(IPage page, string baseUrl)
    {
        // Nawiguj do endpointu testowego logowania
        await page.GotoAsync($"{baseUrl}/login/test?test=true", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = PlaywrightSettings.NavigationTimeout
        });

        // Poczekaj na przekierowanie do strony głównej
        await page.WaitForURLAsync($"{baseUrl}/", new PageWaitForURLOptions
        {
            Timeout = PlaywrightSettings.NavigationTimeout
        });

        // Dodatkowe oczekiwanie na załadowanie sesji i propagację AuthenticationState
        await page.WaitForTimeoutAsync(2000);
        
        // Sprawdź czy menu użytkownika jest widoczne (potwierdzenie zalogowania)
        var userMenuButton = page.Locator("#user-menu-button");
        try
        {
            await userMenuButton.WaitForAsync(new LocatorWaitForOptions 
            { 
                State = WaitForSelectorState.Visible,
                Timeout = 5000 
            });
        }
        catch
        {
            // Jeśli menu nie jest widoczne, spróbuj przeładować stronę
            await page.ReloadAsync(new PageReloadOptions { WaitUntil = WaitUntilState.NetworkIdle });
            await page.WaitForTimeoutAsync(1000);
        }
    }

    /// <summary>
    /// Wylogowuje użytkownika
    /// </summary>
    /// <param name="page">Strona Playwright</param>
    /// <param name="baseUrl">Bazowy URL aplikacji</param>
    public static async Task LogoutAsync(IPage page, string baseUrl)
    {
        await page.GotoAsync($"{baseUrl}/login/logout", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = PlaywrightSettings.NavigationTimeout
        });
    }

    /// <summary>
    /// Sprawdza czy użytkownik jest zalogowany
    /// </summary>
    /// <param name="page">Strona Playwright</param>
    /// <returns>True jeśli użytkownik jest zalogowany</returns>
    public static async Task<bool> IsUserLoggedInAsync(IPage page)
    {
        // Sprawdź czy istnieje przycisk menu użytkownika (widoczny tylko dla zalogowanych)
        var userMenuButton = page.Locator("#user-menu-button");
        
        try
        {
            await userMenuButton.WaitForAsync(new LocatorWaitForOptions 
            { 
                State = WaitForSelectorState.Visible,
                Timeout = 2000 
            });
            return true;
        }
        catch
        {
            return false;
        }
    }
}
