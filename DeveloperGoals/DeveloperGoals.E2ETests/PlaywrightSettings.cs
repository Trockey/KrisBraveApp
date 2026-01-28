using Microsoft.Playwright;

namespace DeveloperGoals.E2ETests;

/// <summary>
/// Konfiguracja Playwright dla testów E2E
/// </summary>
public static class PlaywrightSettings
{
    /// <summary>
    /// Domyślne opcje dla przeglądarki Chromium
    /// </summary>
    public static BrowserTypeLaunchOptions BrowserLaunchOptions => new()
    {
        Headless = true, // Zmień na false dla debugowania
        SlowMo = 0, // Zmień na np. 100 dla debugowania (opóźnienie w ms)
    };

    /// <summary>
    /// Domyślne opcje dla kontekstu przeglądarki
    /// </summary>
    public static BrowserNewContextOptions ContextOptions => new()
    {
        ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
        Locale = "pl-PL",
        TimezoneId = "Europe/Warsaw",
        // Opcjonalnie: zapisywanie wideo dla nieudanych testów
        // RecordVideoDir = "test-results/videos",
    };

    /// <summary>
    /// Podstawowy URL aplikacji (dla testów lokalnych)
    /// </summary>
    public static string BaseUrl => Environment.GetEnvironmentVariable("BASE_URL") 
                                     ?? "https://localhost:5001";

    /// <summary>
    /// Timeout dla asercji (w milisekundach)
    /// </summary>
    public static float DefaultTimeout => 10000; // 10 sekund

    /// <summary>
    /// Timeout dla nawigacji (w milisekundach)
    /// </summary>
    public static float NavigationTimeout => 30000; // 30 sekund
}
