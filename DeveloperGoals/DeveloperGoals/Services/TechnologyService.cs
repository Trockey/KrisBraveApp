using System.Net.Http.Json;
using DeveloperGoals.DTOs;

namespace DeveloperGoals.Services;

/// <summary>
/// Serwis do komunikacji z API technologii
/// </summary>
public class TechnologyService : ITechnologyService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TechnologyService> _logger;
    private readonly IUserStateService _userStateService;
    private const string BaseUrl = "/api/technologies";

    public TechnologyService(
        HttpClient httpClient, 
        ILogger<TechnologyService> logger,
        IUserStateService userStateService)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userStateService = userStateService ?? throw new ArgumentNullException(nameof(userStateService));
    }

    public async Task<TechnologyGraphDto?> GetGraphAsync()
    {
        try
        {
            _logger.LogInformation("Pobieranie grafu technologii");
            
            var email = _userStateService.CurrentState.Email;
            var url = string.IsNullOrEmpty(email) 
                ? $"{BaseUrl}/graph" 
                : $"{BaseUrl}/graph?email={Uri.EscapeDataString(email)}";
            
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Nie udało się pobrać grafu. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TechnologyGraphDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania grafu technologii");
            throw;
        }
    }

    public async Task<TechnologiesListDto?> GetTechnologiesAsync()
    {
        try
        {
            _logger.LogInformation("Pobieranie listy technologii");
            var response = await _httpClient.GetAsync(BaseUrl);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Nie udało się pobrać listy technologii. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TechnologiesListDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania listy technologii");
            throw;
        }
    }

    public async Task<CreateTechnologyResponseDto?> AddTechnologyAsync(CreateTechnologyCommand command)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        try
        {
            _logger.LogInformation("Dodawanie technologii: {TechDefId}", command.TechnologyDefinitionId);
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, command);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Nie udało się dodać technologii. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<CreateTechnologyResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas dodawania technologii");
            throw;
        }
    }

    public async Task<HttpResponseMessage> AddTechnologiesBatchAsync(BatchAddTechnologiesCommand command)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        try
        {
            _logger.LogInformation("Dodawanie {Count} technologii (batch)", command.Technologies.Count);
            return await _httpClient.PostAsJsonAsync($"{BaseUrl}/batch", command);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas dodawania technologii (batch)");
            throw;
        }
    }

    public async Task<CreateCustomTechnologyResponseDto?> CreateCustomTechnologyAsync(CreateCustomTechnologyCommand command)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        try
        {
            _logger.LogInformation("Tworzenie własnej technologii: {Name}", command.Name);
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/custom", command);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Nie udało się utworzyć własnej technologii. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<CreateCustomTechnologyResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas tworzenia własnej technologii");
            throw;
        }
    }

    public async Task<UpdateTechnologyResponseDto?> UpdateTechnologyAsync(int id, UpdateTechnologyCommand command)
    {
        if (id <= 0)
            throw new ArgumentException("ID musi być większe od 0", nameof(id));
        
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        try
        {
            _logger.LogInformation("Aktualizacja technologii {Id}", id);
            var response = await _httpClient.PatchAsJsonAsync($"{BaseUrl}/{id}", command);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Nie udało się zaktualizować technologii. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<UpdateTechnologyResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas aktualizacji technologii {Id}", id);
            throw;
        }
    }

    public async Task<DeleteTechnologyResponseDto?> DeleteTechnologyAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID musi być większe od 0", nameof(id));

        try
        {
            _logger.LogInformation("Usuwanie technologii {Id}", id);
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Nie udało się usunąć technologii. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<DeleteTechnologyResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas usuwania technologii {Id}", id);
            throw;
        }
    }
}
