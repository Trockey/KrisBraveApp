using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using DeveloperGoals.Configuration;
using DeveloperGoals.Data;
using DeveloperGoals.DTOs;
using DeveloperGoals.Exceptions;
using DeveloperGoals.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DeveloperGoals.Services;

/// <summary>
/// Implementacja serwisu generowania rekomendacji AI
/// </summary>
public class AIRecommendationService : IAIRecommendationService
{
    private readonly ApplicationDbContext _context;
    private readonly IOpenRouterService _openRouterService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<AIRecommendationService> _logger;
    private readonly CacheOptions _cacheOptions;

    public AIRecommendationService(
        ApplicationDbContext context,
        IOpenRouterService openRouterService,
        IMemoryCache cache,
        ILogger<AIRecommendationService> logger,
        IOptions<CacheOptions> cacheOptions)
    {
        _context = context;
        _openRouterService = openRouterService;
        _cache = cache;
        _logger = logger;
        _cacheOptions = cacheOptions.Value;
    }

    public async Task<RecommendationsResponseDto> GenerateRecommendationsAsync(
        BigInteger googleUserId,
        GenerateRecommendationsCommand command,
        CancellationToken cancellationToken = default)
    {
        // 0. Sprawdzenie istnienia użytkownika
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.GoogleId == googleUserId.ToString(), cancellationToken);

        if(user == null)
        {
            throw new ProfileIncompleteException(
                "User not found. Please complete your profile first.");
        }

        var userId = user.Id;

        // 1. Sprawdź cache
        var cacheKey = BuildCacheKey(googleUserId, command);
        if (_cache.TryGetValue<RecommendationsResponseDto>(cacheKey, out var cachedResult))
        {
            _logger.LogInformation("Cache hit for user {UserId}, technology {TechId}", 
                googleUserId, command.FromTechnologyId);
            
            cachedResult!.FromCache = true;
            return cachedResult;
        }

        _logger.LogInformation("Cache miss for user {UserId}, technology {TechId}", 
            googleUserId, command.FromTechnologyId);

        // 2. Walidacja profilu użytkownika
        var profile = await ValidateUserProfileAsync(userId, cancellationToken);

        // 3. Walidacja technologii źródłowej
        var sourceTech = await ValidateSourceTechnologyAsync(
            userId, command.FromTechnologyId, cancellationToken);

        // 4. Walidacja technologii kontekstowych
        var contextTechs = await ValidateContextTechnologiesAsync(
            userId, command.ContextTechnologyIds, cancellationToken);

        // 5. Pobranie istniejących technologii w grafie
        var existingTechIds = await GetExistingTechnologyIdsAsync(userId, cancellationToken);

        // 6. Wywołanie AI
        var aiRecommendations = await _openRouterService.GetRecommendationsAsync(
            profile, sourceTech, contextTechs, existingTechIds, cancellationToken);

        // 7. Mapowanie na TechnologyDefinitions i sprawdzenie isAlreadyInGraph
        var recommendations = await MapToRecommendationsAsync(
            aiRecommendations, existingTechIds, cancellationToken);

        // 8. Budowanie response
        var response = new RecommendationsResponseDto
        {
            Recommendations = recommendations,
            Count = recommendations.Count,
            FromCache = false,
            GeneratedAt = DateTime.UtcNow,
            CacheExpiresAt = DateTime.UtcNow.AddHours(_cacheOptions.RecommendationsTTL)
        };

        // 9. Zapis do cache
        SaveToCache(cacheKey, response);

        _logger.LogInformation(
            "Successfully generated {Count} recommendations for user {UserId}",
            recommendations.Count, userId);

        return response;
    }

    private async Task<UserProfile> ValidateUserProfileAsync(
        BigInteger userId,
        CancellationToken cancellationToken)
    {
        var profile = await _context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile == null)
        {
            throw new ProfileIncompleteException(
                "Profile is incomplete. Please complete your profile first.");
        }

        // Sprawdzenie kompletności profilu
        if (string.IsNullOrWhiteSpace(profile.MainTechnologies) ||
            profile.Role == 0 ||
            profile.DevelopmentArea == 0)
        {
            throw new ProfileIncompleteException(
                "Profile is incomplete. Please complete your profile first.");
        }

        return profile;
    }

    private async Task<TechnologyDto> ValidateSourceTechnologyAsync(
        BigInteger userId,
        int fromTechnologyId,
        CancellationToken cancellationToken)
    {
        var technology = await _context.UserTechnologies
            .AsNoTracking()
            .Include(ut => ut.TechnologyDefinition)
            .FirstOrDefaultAsync(ut => ut.Id == fromTechnologyId, cancellationToken);

        if (technology == null)
        {
            throw new TechnologyNotFoundException("Source technology not found");
        }

        if (technology.UserId != userId) 
        {
            throw new TechnologyNotOwnedException("Technology does not belong to user");
        }

        if (technology.Status != TechnologyStatus.Active)
        {
            throw new InvalidTechnologyStatusException("Technology must have Active status");
        }

        return MapToTechnologyDto(technology);
    }

    private async Task<List<TechnologyDto>> ValidateContextTechnologiesAsync(
        BigInteger userId,
        List<int>? contextTechnologyIds,
        CancellationToken cancellationToken)
    {
        if (contextTechnologyIds == null || contextTechnologyIds.Count == 0)
        {
            return new List<TechnologyDto>();
        }

        // Deduplikacja
        var uniqueIds = contextTechnologyIds.Distinct().ToList();

        var technologies = await _context.UserTechnologies
            .AsNoTracking()
            .Include(ut => ut.TechnologyDefinition)
            .Where(ut => uniqueIds.Contains(ut.Id))
            .ToListAsync(cancellationToken);

        // Sprawdzenie czy wszystkie technologie należą do użytkownika
        foreach (var tech in technologies)
        {
            if (tech.UserId != userId)
            {
                throw new TechnologyNotOwnedException(
                    $"Context technology {tech.Id} does not belong to user");
            }
        }

        // Sprawdzenie czy znaleziono wszystkie technologie
        if (technologies.Count != uniqueIds.Count)
        {
            var foundIds = technologies.Select(t => t.Id).ToHashSet();
            var missingIds = uniqueIds.Where(id => !foundIds.Contains(id)).ToList();
            
            _logger.LogWarning(
                "Some context technologies not found for user {UserId}: {MissingIds}",
                userId, string.Join(", ", missingIds));
        }

        return technologies.Select(MapToTechnologyDto).ToList();
    }

    private async Task<HashSet<int>> GetExistingTechnologyIdsAsync(
        BigInteger userId,
        CancellationToken cancellationToken)
    {
        return await _context.UserTechnologies
            .AsNoTracking()
            .Where(ut => ut.UserId == userId && ut.Status == TechnologyStatus.Active)
            .Select(ut => ut.TechnologyDefinitionId)
            .ToHashSetAsync(cancellationToken);
    }

    private async Task<List<RecommendationDto>> MapToRecommendationsAsync(
        List<AIRecommendationResult> aiRecommendations,
        HashSet<int> existingTechIds,
        CancellationToken cancellationToken)
    {
        var recommendations = new List<RecommendationDto>();

        foreach (var aiRec in aiRecommendations)
        {
            // Parsowanie tagu
            TechnologyTag tag;
            if (!Enum.TryParse<TechnologyTag>(aiRec.Tag, out tag))
            {
                _logger.LogWarning("Invalid tag '{Tag}' in AI recommendation, defaulting to Technologia", aiRec.Tag);
                tag = TechnologyTag.Technologia;
            }

            // Szukaj istniejącej definicji technologii
            var techDef = await _context.TechnologyDefinitions
                .AsNoTracking()
                .FirstOrDefaultAsync(td => 
                    td.Name == aiRec.Name && 
                    td.Prefix == aiRec.Prefix && 
                    td.Tag == tag,
                    cancellationToken);

            int techDefId;
            bool isAlreadyInGraph;

            if (techDef != null)
            {
                // Technologia już istnieje w słowniku
                techDefId = techDef.Id;
                isAlreadyInGraph = existingTechIds.Contains(techDefId);
            }
            else
            {
                // Nowa technologia - utworzy się przy dodawaniu przez użytkownika
                // Na razie używamy ID = 0 jako placeholder
                techDefId = 0;
                isAlreadyInGraph = false;
            }

            recommendations.Add(new RecommendationDto
            {
                TechnologyDefinitionId = techDefId,
                Name = aiRec.Name,
                Prefix = aiRec.Prefix,
                Tag = tag.ToString(),
                SystemDescription = aiRec.SystemDescription,
                AiReasoning = aiRec.AiReasoning,
                IsAlreadyInGraph = isAlreadyInGraph
            });
        }

        return recommendations;
    }

    private TechnologyDto MapToTechnologyDto(UserTechnology technology)
    {
        return new TechnologyDto
        {
            Id = technology.Id,
            UserId = (ulong)technology.UserId,
            TechnologyDefinitionId = technology.TechnologyDefinitionId,
            Name = technology.Name,
            Prefix = technology.Category,
            Tag = technology.Tag.ToString(),
            SystemDescription = technology.SystemDescription,
            PrivateDescription = technology.PrivateDescription,
            Progress = technology.Progress,
            Status = technology.Status.ToString(),
            IsCustom = technology.IsCustom,
            IsStart = technology.IsStart,
            AiReasoning = technology.AiReasoning,
            CreatedAt = technology.CreatedAt,
            UpdatedAt = technology.UpdatedAt
        };
    }

    private string BuildCacheKey(BigInteger userId, GenerateRecommendationsCommand command)
    {
        // Sortowanie contextTechnologyIds dla spójności klucza cache
        var contextIds = command.ContextTechnologyIds?.OrderBy(id => id).ToList() ?? new List<int>();
        var contextHash = ComputeHash(string.Join(",", contextIds));

        return $"ai_recommendations_{userId}_{command.FromTechnologyId}_{contextHash}";
    }

    private string ComputeHash(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return "empty";
        }

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes)[..16]; // Pierwsze 16 znaków
    }

    private void SaveToCache(string cacheKey, RecommendationsResponseDto response)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(_cacheOptions.RecommendationsTTL))
            .SetSize(1);

        _cache.Set(cacheKey, response, cacheEntryOptions);

        _logger.LogDebug("Saved recommendations to cache with key {CacheKey}", cacheKey);
    }
}
