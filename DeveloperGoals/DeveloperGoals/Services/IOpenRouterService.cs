using DeveloperGoals.DTOs;
using DeveloperGoals.Models;

namespace DeveloperGoals.Services;

/// <summary>
/// Serwis do komunikacji z OpenRouter AI API
/// </summary>
public interface IOpenRouterService
{
    /// <summary>
    /// Generuje rekomendacje technologii przy użyciu AI
    /// </summary>
    /// <param name="profile">Profil użytkownika</param>
    /// <param name="sourceTechnology">Technologia źródłowa</param>
    /// <param name="contextTechnologies">Technologie kontekstowe</param>
    /// <param name="existingTechDefinitionIds">ID definicji technologii już w grafie użytkownika</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Lista rekomendacji AI</returns>
    Task<List<AIRecommendationResult>> GetRecommendationsAsync(
        UserProfile profile,
        TechnologyDto sourceTechnology,
        List<TechnologyDto> contextTechnologies,
        HashSet<int> existingTechDefinitionIds,
        CancellationToken cancellationToken = default
    );
}

/// <summary>
/// Wynik rekomendacji z AI (przed mapowaniem na TechnologyDefinition)
/// </summary>
public class AIRecommendationResult
{
    public string Name { get; set; } = string.Empty;
    public string Prefix { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string SystemDescription { get; set; } = string.Empty;
    public string AiReasoning { get; set; } = string.Empty;
}
