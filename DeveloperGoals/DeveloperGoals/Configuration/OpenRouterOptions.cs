namespace DeveloperGoals.Configuration;

/// <summary>
/// Opcje konfiguracyjne dla OpenRouter API
/// </summary>
public class OpenRouterOptions
{
    public const string SectionName = "OpenRouter";

    /// <summary>
    /// Klucz API OpenRouter
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Bazowy URL API OpenRouter
    /// </summary>
    public string BaseUrl { get; set; } = "https://openrouter.ai/api/v1";

    /// <summary>
    /// Model AI do użycia
    /// </summary>
    public string Model { get; set; } = "anthropic/claude-3.5-sonnet";

    /// <summary>
    /// Timeout dla wywołań API w sekundach
    /// </summary>
    public int Timeout { get; set; } = 20;

    /// <summary>
    /// Maksymalna liczba tokenów w odpowiedzi
    /// </summary>
    public int MaxTokens { get; set; } = 2000;

    /// <summary>
    /// Temperatura dla generowania odpowiedzi (0.0 - 1.0)
    /// </summary>
    public double Temperature { get; set; } = 0.7;

    /// <summary>
    /// URL aplikacji dla HTTP-Referer
    /// </summary>
    public string AppUrl { get; set; } = "https://developergoals.app";

    /// <summary>
    /// Nazwa aplikacji dla X-Title header
    /// </summary>
    public string AppTitle { get; set; } = "DeveloperGoals";
}
