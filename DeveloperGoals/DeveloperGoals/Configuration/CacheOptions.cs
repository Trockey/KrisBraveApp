namespace DeveloperGoals.Configuration;

/// <summary>
/// Opcje konfiguracyjne dla cache'owania
/// </summary>
public class CacheOptions
{
    public const string SectionName = "Cache";

    /// <summary>
    /// TTL dla rekomendacji AI w godzinach
    /// </summary>
    public int RecommendationsTTL { get; set; } = 24;

    /// <summary>
    /// Maksymalna liczba wpisów w cache
    /// </summary>
    public int MaxEntries { get; set; } = 1000;
}
