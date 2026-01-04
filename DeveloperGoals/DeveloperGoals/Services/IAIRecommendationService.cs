using DeveloperGoals.DTOs;

namespace DeveloperGoals.Services;

/// <summary>
/// Serwis do generowania rekomendacji AI technologii
/// </summary>
public interface IAIRecommendationService
{
    /// <summary>
    /// Generuje rekomendacje technologii dla użytkownika
    /// </summary>
    /// <param name="userId">ID użytkownika</param>
    /// <param name="command">Komenda z parametrami generowania</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Response z rekomendacjami</returns>
    Task<RecommendationsResponseDto> GenerateRecommendationsAsync(
        int userId,
        GenerateRecommendationsCommand command,
        CancellationToken cancellationToken = default
    );
}
