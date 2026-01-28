using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace DeveloperGoals.Services;

/// <summary>
/// Serwis do zarządzania globalnym stanem użytkownika w aplikacji
/// Zarejestrowany jako Scoped - oddzielna instancja per połączenie użytkownika
/// </summary>
public class UserStateService : IUserStateService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILogger<UserStateService> _logger;
    private UserState _currentState = new();

    public UserStateService(
        AuthenticationStateProvider authenticationStateProvider,
        ILogger<UserStateService> logger)
    {
        _authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public UserState CurrentState => _currentState;

    public event Action? OnStateChanged;

    public async Task InitializeAsync()
    {
        try
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                _currentState = new UserState
                {
                    IsAuthenticated = true,
                    Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
                    Name = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
                    GoogleId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    HasProfile = false // Będzie aktualizowane po sprawdzeniu API
                };

                _logger.LogInformation("Stan użytkownika zainicjalizowany: {Email}", _currentState.Email);
            }
            else
            {
                _currentState = new UserState
                {
                    IsAuthenticated = false
                };
                _logger.LogInformation("Użytkownik niezalogowany");
            }

            NotifyStateChanged();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas inicjalizacji stanu użytkownika");
            throw;
        }
    }

    public void UpdateState(UserState newState)
    {
        if (newState is null)
            throw new ArgumentNullException(nameof(newState));

        _currentState = newState;
        _logger.LogInformation("Stan użytkownika zaktualizowany: IsAuth={IsAuth}, HasProfile={HasProfile}", 
            newState.IsAuthenticated, newState.HasProfile);
        
        NotifyStateChanged();
    }

    public void SetHasProfile(bool hasProfile)
    {
        if (_currentState.HasProfile != hasProfile)
        {
            _currentState.HasProfile = hasProfile;
            _logger.LogInformation("Status profilu zmieniony na: {HasProfile}", hasProfile);
            NotifyStateChanged();
        }
    }

    public void ClearState()
    {
        _currentState = new UserState
        {
            IsAuthenticated = false
        };
        _logger.LogInformation("Stan użytkownika wyczyszczony");
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }
}
