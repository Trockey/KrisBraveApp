namespace DeveloperGoals.Models;

/// <summary>
/// Profil użytkownika zawierający informacje o specjalizacji i doświadczeniu.
/// </summary>
public class UserProfile
{
    /// <summary>
    /// Unikalny identyfikator profilu.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID użytkownika (klucz obcy).
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Użytkownik (relacja 1:1).
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Główne technologie użytkownika (rozdzielone przecinkami).
    /// Przykład: ".NET,JavaScript,Python"
    /// </summary>
    public string MainTechnologies { get; set; } = string.Empty;

    /// <summary>
    /// Rola użytkownika.
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// Obszar rozwoju użytkownika.
    /// </summary>
    public DevelopmentArea DevelopmentArea { get; set; }

    /// <summary>
    /// Data utworzenia profilu.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data ostatniej aktualizacji profilu.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Rola użytkownika w projekcie.
/// </summary>
public enum UserRole
{
    Programista = 1,
    Tester = 2,
    Analityk = 3,
    DataScienceSpecialist = 4
}

/// <summary>
/// Obszar rozwoju użytkownika.
/// </summary>
public enum DevelopmentArea
{
    UserInterface = 1,
    Backend = 2,
    FullStack = 3,
    Testing = 4,
    DataScience = 5,
    DevOps = 6
}

