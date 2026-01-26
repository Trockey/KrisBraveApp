namespace DeveloperGoals.Services;

/// <summary>
/// Model stanu użytkownika
/// </summary>
public class UserState
{
    public bool IsAuthenticated { get; set; }
    public bool HasProfile { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? GoogleId { get; set; }
}

/// <summary>
/// Interfejs serwisu do zarządzania stanem użytkownika
/// </summary>
public interface IUserStateService
{
    /// <summary>
    /// Aktualny stan użytkownika
    /// </summary>
    UserState CurrentState { get; }
    
    /// <summary>
    /// Zdarzenie wywoływane gdy stan użytkownika się zmienia
    /// </summary>
    event Action? OnStateChanged;
    
    /// <summary>
    /// Inicjalizuje stan użytkownika na podstawie AuthenticationState
    /// </summary>
    Task InitializeAsync();
    
    /// <summary>
    /// Aktualizuje stan użytkownika
    /// </summary>
    void UpdateState(UserState newState);
    
    /// <summary>
    /// Oznacza że użytkownik ma profil
    /// </summary>
    void SetHasProfile(bool hasProfile);
    
    /// <summary>
    /// Czyści stan użytkownika (wylogowanie)
    /// </summary>
    void ClearState();
}
