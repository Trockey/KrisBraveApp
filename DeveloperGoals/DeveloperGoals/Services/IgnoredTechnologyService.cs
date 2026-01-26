using System.Net.Http.Json;
using DeveloperGoals.DTOs;

namespace DeveloperGoals.Services;

/// <summary>
/// Serwis do komunikacji z API ignorowanych technologii
/// </summary>
public class IgnoredTechnologyService : IIgnoredTechnologyService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<IgnoredTechnologyService> _logger;
    private const string BaseUrl = "/api/ignored-technologies";

    public IgnoredTechnologyService(HttpClient httpClient, ILogger<IgnoredTechnologyService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IgnoredTechnologiesListDto?> GetIgnoredAsync(int limit = 50, int offset = 0)
    {
        try
        {
            _logger.LogInformation("Pobieranie listy ignorowanych technologii (limit: {Limit}, offset: {Offset})", limit, offset);
            var response = await _httpClient.GetAsync($"{BaseUrl}?limit={limit}&offset={offset}");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Nie udało się pobrać listy ignorowanych. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<IgnoredTechnologiesListDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania listy ignorowanych technologii");
            throw;
        }
    }

    public async Task<AddIgnoredTechnologyResponseDto?> AddIgnoredAsync(AddIgnoredTechnologyCommand command)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        try
        {
            _logger.LogInformation("Dodawanie {Count} technologii do listy ignorowanych", command.Technologies.Count);
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, command);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Nie udało się dodać do ignorowanych. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<AddIgnoredTechnologyResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas dodawania do ignorowanych");
            throw;
        }
    }

    public async Task<DeleteIgnoredTechnologyResponseDto?> RestoreIgnoredAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID musi być większe od 0", nameof(id));

        try
        {
            _logger.LogInformation("Przywracanie technologii {Id} z listy ignorowanych", id);
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Nie udało się przywrócić technologii. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<DeleteIgnoredTechnologyResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas przywracania technologii {Id}", id);
            throw;
        }
    }

    public async Task<HttpResponseMessage> RestoreBatchAsync(BatchDeleteIgnoredCommand command)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        try
        {
            _logger.LogInformation("Przywracanie {Count} technologii (batch)", command.Ids.Count);
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/batch")
            {
                Content = JsonContent.Create(command)
            };
            
            return await _httpClient.SendAsync(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas przywracania technologii (batch)");
            throw;
        }
    }
}
