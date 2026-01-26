using System.Net.Http.Json;
using System.Text.Json;
using DeveloperGoals.DTOs;

namespace DeveloperGoals.Services;

/// <summary>
/// Serwis HTTP do komunikacji z API rekomendacji AI (dla komponentów Blazor)
/// </summary>
public class AIRecommendationClientService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AIRecommendationClientService> _logger;
    private const string BaseUrl = "/api/ai/recommendations";

    public AIRecommendationClientService(HttpClient httpClient, ILogger<AIRecommendationClientService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Generuje rekomendacje technologii przez API
    /// </summary>
    public async Task<RecommendationsResponseDto?> GetRecommendationsAsync(GenerateRecommendationsCommand command)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        try
        {
            _logger.LogInformation("Pobieranie rekomendacji AI dla technologii {TechId}", command.FromTechnologyId);
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, command);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Nie udało się pobrać rekomendacji. Status: {StatusCode}", response.StatusCode);
                
                // Odczytaj zawartość błędu dla debugowania
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Zawartość odpowiedzi błędu: {ErrorContent}", errorContent);
                
                return null;
            }

            // return await response.Content.ReadFromJsonAsync<RecommendationsResponseDto>();

            // Sprawdź content type
            var contentType = response.Content.Headers.ContentType?.MediaType;
            _logger.LogInformation("Content-Type odpowiedzi: {ContentType}", contentType);

            // Odczytaj surową zawartość przed parsowaniem
            var rawContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Surowa zawartość odpowiedzi (pierwsze 500 znaków): {RawContent}", 
                rawContent.Length > 500 ? rawContent.Substring(0, 500) + "..." : rawContent);

            // Spróbuj sparsować JSON z odczytanego stringa
            try
            {
                return JsonSerializer.Deserialize<RecommendationsResponseDto>(rawContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Błąd parsowania JSON. Pełna zawartość odpowiedzi: {FullContent}", rawContent);
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas pobierania rekomendacji AI");
            throw;
        }
    }
}
