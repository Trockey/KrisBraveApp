namespace DeveloperGoals.Models;

/// <summary>
/// Archiwum technologii ignorowanych przez użytkownika.
/// Przechowuje pełną definicję technologii (bez referencji do TechnologyDefinitions).
/// </summary>
public class IgnoredTechnology
{
    /// <summary>
    /// Unikalny identyfikator ignorowanej technologii.
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
    /// Nazwa technologii.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategoria/prefix technologii.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Tag technologii.
    /// </summary>
    public TechnologyTag Tag { get; set; }

    /// <summary>
    /// Opis systemowy technologii.
    /// </summary>
    public string SystemDescription { get; set; } = string.Empty;

    /// <summary>
    /// Reasoning z AI (dlaczego została polecona).
    /// </summary>
    public string? AiReasoning { get; set; }

    /// <summary>
    /// ID technologii kontekstowej (z której wygenerowano rekomendację).
    /// Opcjonalne - może być null jeśli nieznany kontekst.
    /// </summary>
    public int? ContextTechnologyId { get; set; }

    /// <summary>
    /// Technologia kontekstowa (opcjonalna relacja).
    /// </summary>
    public UserTechnology? ContextTechnology { get; set; }

    /// <summary>
    /// Data zignorowania technologii.
    /// </summary>
    public DateTime IgnoredAt { get; set; } = DateTime.UtcNow;
}

