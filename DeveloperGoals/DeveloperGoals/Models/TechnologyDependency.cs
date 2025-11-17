namespace DeveloperGoals.Models;

/// <summary>
/// Reprezentuje zależność między technologiami w grafie (krawędź grafu).
/// </summary>
public class TechnologyDependency
{
    /// <summary>
    /// Unikalny identyfikator zależności.
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
    /// ID technologii źródłowej (od której prowadzi krawędź).
    /// Może być null dla węzła "Start".
    /// </summary>
    public int? FromTechnologyId { get; set; }

    /// <summary>
    /// Technologia źródłowa.
    /// </summary>
    public Technology? FromTechnology { get; set; }

    /// <summary>
    /// ID technologii docelowej (do której prowadzi krawędź).
    /// </summary>
    public int ToTechnologyId { get; set; }

    /// <summary>
    /// Technologia docelowa.
    /// </summary>
    public Technology ToTechnology { get; set; } = null!;

    /// <summary>
    /// Data utworzenia zależności.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

