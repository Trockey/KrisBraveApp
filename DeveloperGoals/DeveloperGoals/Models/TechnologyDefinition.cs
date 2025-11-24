namespace DeveloperGoals.Models;

/// <summary>
/// Wspólny słownik technologii w systemie.
/// Deduplikacja definicji technologii używanych przez wielu użytkowników.
/// </summary>
public class TechnologyDefinition
{
    /// <summary>
    /// Unikalny identyfikator definicji technologii.
    /// Id = 1 jest zarezerwowane dla węzła "Start".
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Pełna nazwa technologii z prefiksem.
    /// Przykład: "DotNet - Entity Framework"
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Prefix/kategoria główna technologii.
    /// Przykład: "DotNet", "Java", "JavaScript", "Python"
    /// </summary>
    public string Prefix { get; set; } = string.Empty;

    /// <summary>
    /// Tag technologii (np. Framework, Technologia, BazaDanych).
    /// </summary>
    public TechnologyTag Tag { get; set; }

    /// <summary>
    /// Opis systemowy generowany przez AI.
    /// </summary>
    public string SystemDescription { get; set; } = string.Empty;

    /// <summary>
    /// Data utworzenia definicji.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Lista instancji technologii użytkowników korzystających z tej definicji.
    /// </summary>
    public ICollection<UserTechnology> UserTechnologies { get; set; } = new List<UserTechnology>();
}

