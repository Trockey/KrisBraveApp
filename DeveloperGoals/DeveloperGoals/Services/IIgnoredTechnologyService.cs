using DeveloperGoals.DTOs;

namespace DeveloperGoals.Services;

/// <summary>
/// Interfejs serwisu do zarządzania ignorowanymi technologiami
/// </summary>
public interface IIgnoredTechnologyService
{
    /// <summary>
    /// Pobiera listę ignorowanych technologii
    /// </summary>
    Task<IgnoredTechnologiesListDto?> GetIgnoredAsync(int limit = 50, int offset = 0);
    
    /// <summary>
    /// Dodaje technologie do listy ignorowanych
    /// </summary>
    Task<AddIgnoredTechnologyResponseDto?> AddIgnoredAsync(AddIgnoredTechnologyCommand command);
    
    /// <summary>
    /// Przywraca technologię z listy ignorowanych (usuwa z listy)
    /// </summary>
    Task<DeleteIgnoredTechnologyResponseDto?> RestoreIgnoredAsync(int id);
    
    /// <summary>
    /// Przywraca wiele technologii naraz (batch)
    /// </summary>
    Task<HttpResponseMessage> RestoreBatchAsync(BatchDeleteIgnoredCommand command);
}
