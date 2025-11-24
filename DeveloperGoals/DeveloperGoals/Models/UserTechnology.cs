namespace DeveloperGoals.Models;

/// <summary>
/// Reprezentuje technologię w grafie użytkownika.
/// </summary>
public class UserTechnology
{
    /// <summary>
    /// Unikalny identyfikator technologii.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID użytkownika (klucz obcy).
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Użytkownik (relacja N:1).
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// ID definicji technologii (klucz obcy do TechnologyDefinitions).
    /// </summary>
    public int TechnologyDefinitionId { get; set; }

    /// <summary>
    /// Definicja technologii (relacja N:1).
    /// </summary>
    public TechnologyDefinition TechnologyDefinition { get; set; } = null!;

    /// <summary>
    /// Nazwa technologii z prefiksem (denormalizacja dla performance).
    /// Przykład: "DotNet - Entity Framework"
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategoria główna (prefix).
    /// Przykład: DotNet, Java, JavaScript, Python, Database, DevOps, Cloud, etc.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Tag technologii.
    /// </summary>
    public TechnologyTag Tag { get; set; }

    /// <summary>
    /// Opis systemowy (generowany przez AI).
    /// </summary>
    public string SystemDescription { get; set; } = string.Empty;

    /// <summary>
    /// Opis prywatny (edytowalny przez użytkownika).
    /// </summary>
    public string? PrivateDescription { get; set; }

    /// <summary>
    /// Status technologii.
    /// </summary>
    public TechnologyStatus Status { get; set; } = TechnologyStatus.Active;

    /// <summary>
    /// Procent postępu nauki (0-100).
    /// </summary>
    public int Progress { get; set; } = 0;

    /// <summary>
    /// Czy technologia została dodana przez użytkownika (nie przez AI).
    /// </summary>
    public bool IsCustom { get; set; } = false;

    /// <summary>
    /// Czy to węzeł startowy grafu. Węzeł startowy nie może być usunięty.
    /// </summary>
    public bool IsStart { get; set; } = false;

    /// <summary>
    /// Data dodania technologii.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data ostatniej aktualizacji.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Reasoning z AI (dlaczego technologia została polecona).
    /// </summary>
    public string? AiReasoning { get; set; }
}

/// <summary>
/// Tag technologii.
/// </summary>
public enum TechnologyTag
{
    Technologia = 1,
    Framework = 2,
    BazaDanych = 3,
    Metodologia = 4,
    Narzedzie = 5
}

/// <summary>
/// Status technologii.
/// </summary>
public enum TechnologyStatus
{
    Active = 1
}
