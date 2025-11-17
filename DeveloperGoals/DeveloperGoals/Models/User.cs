namespace DeveloperGoals.Models;

/// <summary>
/// Reprezentuje użytkownika aplikacji.
/// </summary>
public class User
{
    /// <summary>
    /// Unikalny identyfikator użytkownika.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID użytkownika z Google OAuth.
    /// </summary>
    public string GoogleId { get; set; } = string.Empty;

    /// <summary>
    /// Email użytkownika z konta Google.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Nazwa użytkownika z konta Google.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Data utworzenia konta.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data ostatniego logowania.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Profil użytkownika (relacja 1:1).
    /// </summary>
    public UserProfile? Profile { get; set; }

    /// <summary>
    /// Lista technologii użytkownika.
    /// </summary>
    public ICollection<Technology> Technologies { get; set; } = new List<Technology>();

    /// <summary>
    /// Lista zależności między technologiami (graf).
    /// </summary>
    public ICollection<TechnologyDependency> TechnologyDependencies { get; set; } = new List<TechnologyDependency>();
}

