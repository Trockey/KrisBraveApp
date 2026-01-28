using DeveloperGoals.DTOs;

namespace DeveloperGoals.Services;

/// <summary>
/// Interfejs serwisu do zarządzania technologiami użytkownika
/// </summary>
public interface ITechnologyService
{
    /// <summary>
    /// Pobiera pełny graf technologii użytkownika (węzły + krawędzie + statystyki)
    /// </summary>
    Task<TechnologyGraphDto?> GetGraphAsync();
    
    /// <summary>
    /// Pobiera listę wszystkich technologii użytkownika
    /// </summary>
    Task<TechnologiesListDto?> GetTechnologiesAsync();
    
    /// <summary>
    /// Dodaje pojedynczą technologię do grafu użytkownika
    /// </summary>
    Task<CreateTechnologyResponseDto?> AddTechnologyAsync(CreateTechnologyCommand command);
    
    /// <summary>
    /// Dodaje wiele technologii naraz (batch)
    /// </summary>
    Task<HttpResponseMessage> AddTechnologiesBatchAsync(BatchAddTechnologiesCommand command);
    
    /// <summary>
    /// Tworzy niestandardową technologię
    /// </summary>
    Task<CreateCustomTechnologyResponseDto?> CreateCustomTechnologyAsync(CreateCustomTechnologyCommand command);
    
    /// <summary>
    /// Aktualizuje technologię (postęp, opis prywatny)
    /// </summary>
    Task<UpdateTechnologyResponseDto?> UpdateTechnologyAsync(int id, UpdateTechnologyCommand command);
    
    /// <summary>
    /// Usuwa technologię z grafu użytkownika
    /// </summary>
    Task<DeleteTechnologyResponseDto?> DeleteTechnologyAsync(int id);
}
