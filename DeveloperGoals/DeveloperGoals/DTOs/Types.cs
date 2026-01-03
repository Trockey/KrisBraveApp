using DeveloperGoals.Models;

namespace DeveloperGoals.DTOs;

// ============================================================================
// AUTHENTICATION DTOs (Section 2.1)
// ============================================================================

/// <summary>
/// Response dla POST /api/auth/login
/// </summary>
public class LoginResponseDto
{
    public string RedirectUrl { get; set; } = string.Empty;
}

/// <summary>
/// DTO użytkownika dla odpowiedzi auth
/// </summary>
public class AuthUserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string GoogleId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool HasProfile { get; set; }
}

/// <summary>
/// Response dla GET /api/auth/callback
/// </summary>
public class CallbackResponseDto
{
    public AuthUserDto User { get; set; } = null!;
    public string RedirectTo { get; set; } = string.Empty;
}

/// <summary>
/// Response dla POST /api/auth/logout
/// </summary>
public class LogoutResponseDto
{
    public string Message { get; set; } = string.Empty;
}

// ============================================================================
// PROFILE DTOs (Section 2.2)
// ============================================================================

/// <summary>
/// DTO profilu użytkownika - GET /api/profile
/// Mapuje UserProfile z bazy danych
/// </summary>
public class UserProfileDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<string> MainTechnologies { get; set; } = new();
    public string Role { get; set; } = string.Empty;
    public string DevelopmentArea { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Command do tworzenia profilu - POST /api/profile
/// </summary>
public class CreateProfileCommand
{
    public List<string> MainTechnologies { get; set; } = new();
    public string Role { get; set; } = string.Empty;
    public string DevelopmentArea { get; set; } = string.Empty;
}

/// <summary>
/// Response dla utworzenia profilu
/// </summary>
public class CreateProfileResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<string> MainTechnologies { get; set; } = new();
    public string Role { get; set; } = string.Empty;
    public string DevelopmentArea { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool StartNodeCreated { get; set; }
}

/// <summary>
/// Command do aktualizacji profilu - PUT /api/profile
/// </summary>
public class UpdateProfileCommand
{
    public List<string> MainTechnologies { get; set; } = new();
    public string Role { get; set; } = string.Empty;
    public string DevelopmentArea { get; set; } = string.Empty;
}

// ============================================================================
// TECHNOLOGY DTOs (Section 2.3)
// ============================================================================

/// <summary>
/// DTO pełnej technologii - mapuje UserTechnology
/// </summary>
public class TechnologyDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TechnologyDefinitionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Prefix { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string SystemDescription { get; set; } = string.Empty;
    public string? PrivateDescription { get; set; }
    public int Progress { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsCustom { get; set; }
    public bool IsStart { get; set; }
    public string? AiReasoning { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Response dla GET /api/technologies - lista technologii
/// </summary>
public class TechnologiesListDto
{
    public List<TechnologyDto> Technologies { get; set; } = new();
    public int Count { get; set; }
}

/// <summary>
/// Command do dodania technologii - POST /api/technologies
/// </summary>
public class CreateTechnologyCommand
{
    public int TechnologyDefinitionId { get; set; }
    public int FromTechnologyId { get; set; }
    public string? PrivateDescription { get; set; }
    public bool IsCustom { get; set; } = false;
}

/// <summary>
/// Response dla dodania technologii
/// </summary>
public class CreateTechnologyResponseDto
{
    public TechnologyDto Technology { get; set; } = null!;
    public DependencyDto Dependency { get; set; } = null!;
}

/// <summary>
/// Pojedyncza technologia w batch request
/// </summary>
public class BatchTechnologyItem
{
    public int TechnologyDefinitionId { get; set; }
    public string? PrivateDescription { get; set; }
}

/// <summary>
/// Command do dodania wielu technologii - POST /api/technologies/batch
/// </summary>
public class BatchAddTechnologiesCommand
{
    public int FromTechnologyId { get; set; }
    public List<BatchTechnologyItem> Technologies { get; set; } = new();
}

/// <summary>
/// Pojedynczy wynik w batch response
/// </summary>
public class BatchTechnologyResult
{
    public TechnologyDto Technology { get; set; } = null!;
    public DependencyDto Dependency { get; set; } = null!;
}

/// <summary>
/// Response dla batch add - sukces
/// </summary>
public class BatchAddSuccessResponseDto
{
    public List<BatchTechnologyResult> Added { get; set; } = new();
    public int Count { get; set; }
}

/// <summary>
/// Pojedynczy wynik w multi-status response
/// </summary>
public class BatchStatusResult
{
    public int TechnologyDefinitionId { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? TechnologyId { get; set; }
    public string? Error { get; set; }
}

/// <summary>
/// Response dla batch add - partial success (207)
/// </summary>
public class BatchAddMultiStatusResponseDto
{
    public List<BatchStatusResult> Results { get; set; } = new();
    public int SuccessCount { get; set; }
    public int ErrorCount { get; set; }
}

/// <summary>
/// Command do tworzenia custom technologii - POST /api/technologies/custom
/// </summary>
public class CreateCustomTechnologyCommand
{
    public string Name { get; set; } = string.Empty;
    public string Prefix { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string SystemDescription { get; set; } = string.Empty;
    public int FromTechnologyId { get; set; }
    public string? PrivateDescription { get; set; }
}

/// <summary>
/// DTO definicji technologii
/// </summary>
public class TechnologyDefinitionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Prefix { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string SystemDescription { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Response dla utworzenia custom technologii
/// </summary>
public class CreateCustomTechnologyResponseDto
{
    public TechnologyDefinitionDto TechnologyDefinition { get; set; } = null!;
    public TechnologyDto Technology { get; set; } = null!;
    public DependencyDto Dependency { get; set; } = null!;
}

/// <summary>
/// Command do aktualizacji technologii - PATCH /api/technologies/{id}
/// </summary>
public class UpdateTechnologyCommand
{
    public int? Progress { get; set; }
    public string? PrivateDescription { get; set; }
}

/// <summary>
/// Response dla aktualizacji technologii
/// </summary>
public class UpdateTechnologyResponseDto
{
    public int Id { get; set; }
    public int Progress { get; set; }
    public string? PrivateDescription { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Response dla usunięcia technologii - DELETE /api/technologies/{id}
/// </summary>
public class DeleteTechnologyResponseDto
{
    public string Message { get; set; } = string.Empty;
    public int DeletedDependencies { get; set; }
}

/// <summary>
/// Węzeł grafu - uproszczona wersja TechnologyDto
/// </summary>
public class GraphNodeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Prefix { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public int Progress { get; set; }
    public bool IsStart { get; set; }
    public string SystemDescription { get; set; } = string.Empty;
}

/// <summary>
/// Krawędź grafu - mapuje TechnologyDependency
/// </summary>
public class GraphEdgeDto
{
    public int Id { get; set; }
    public int From { get; set; }
    public int To { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Statystyki grafu
/// </summary>
public class GraphStatsDto
{
    public int TotalNodes { get; set; }
    public int TotalEdges { get; set; }
    public int AverageProgress { get; set; }
    public int CompletedCount { get; set; }
}

/// <summary>
/// Response dla GET /api/technologies/graph - pełny graf
/// </summary>
public class TechnologyGraphDto
{
    public List<GraphNodeDto> Nodes { get; set; } = new();
    public List<GraphEdgeDto> Edges { get; set; } = new();
    public GraphStatsDto Stats { get; set; } = null!;
}

// ============================================================================
// DEPENDENCY DTOs (Section 2.4)
// ============================================================================

/// <summary>
/// DTO zależności - mapuje TechnologyDependency
/// </summary>
public class DependencyDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? FromTechnologyId { get; set; }
    public int ToTechnologyId { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Command do tworzenia zależności - POST /api/technologies/{id}/dependencies
/// </summary>
public class CreateDependencyCommand
{
    public int ToTechnologyId { get; set; }
}

/// <summary>
/// Response dla usunięcia zależności
/// </summary>
public class DeleteDependencyResponseDto
{
    public string Message { get; set; } = string.Empty;
}

// ============================================================================
// AI RECOMMENDATIONS DTOs (Section 2.5)
// ============================================================================

/// <summary>
/// Command do generowania rekomendacji - POST /api/ai/recommendations
/// </summary>
public class GenerateRecommendationsCommand
{
    public int FromTechnologyId { get; set; }
    public List<int>? ContextTechnologyIds { get; set; }
}

/// <summary>
/// Pojedyncza rekomendacja AI
/// </summary>
public class RecommendationDto
{
    public int TechnologyDefinitionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Prefix { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string SystemDescription { get; set; } = string.Empty;
    public string AiReasoning { get; set; } = string.Empty;
    public bool IsAlreadyInGraph { get; set; }
}

/// <summary>
/// Response dla generowania rekomendacji
/// </summary>
public class RecommendationsResponseDto
{
    public List<RecommendationDto> Recommendations { get; set; } = new();
    public int Count { get; set; }
    public bool FromCache { get; set; }
    public DateTime? CacheExpiresAt { get; set; }
    public DateTime GeneratedAt { get; set; }
}

// ============================================================================
// IGNORED TECHNOLOGIES DTOs (Section 2.6)
// ============================================================================

/// <summary>
/// DTO ignorowanej technologii - mapuje IgnoredTechnology
/// </summary>
public class IgnoredTechnologyDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string SystemDescription { get; set; } = string.Empty;
    public string? AiReasoning { get; set; }
    public int? ContextTechnologyId { get; set; }
    public string? ContextTechnologyName { get; set; }
    public DateTime IgnoredAt { get; set; }
}

/// <summary>
/// Response dla GET /api/ignored-technologies - lista ignorowanych
/// </summary>
public class IgnoredTechnologiesListDto
{
    public List<IgnoredTechnologyDto> IgnoredTechnologies { get; set; } = new();
    public int Count { get; set; }
    public int Total { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
}

/// <summary>
/// Pojedyncza technologia do zignorowania
/// </summary>
public class IgnoreTechnologyItem
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string SystemDescription { get; set; } = string.Empty;
    public string? AiReasoning { get; set; }
    public int? ContextTechnologyId { get; set; }
}

/// <summary>
/// Command do dodania technologii do listy ignorowanych - POST /api/ignored-technologies
/// </summary>
public class AddIgnoredTechnologyCommand
{
    public List<IgnoreTechnologyItem> Technologies { get; set; } = new();
}

/// <summary>
/// Pojedynczy wynik dodania do ignorowanych
/// </summary>
public class AddedIgnoredTechnologyResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime IgnoredAt { get; set; }
}

/// <summary>
/// Response dla dodania do ignorowanych
/// </summary>
public class AddIgnoredTechnologyResponseDto
{
    public List<AddedIgnoredTechnologyResult> Added { get; set; } = new();
    public int Count { get; set; }
}

/// <summary>
/// Response dla usunięcia z ignorowanych - DELETE /api/ignored-technologies/{id}
/// </summary>
public class DeleteIgnoredTechnologyResponseDto
{
    public string Message { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Command do batch delete - DELETE /api/ignored-technologies/batch
/// </summary>
public class BatchDeleteIgnoredCommand
{
    public List<int> Ids { get; set; } = new();
}

/// <summary>
/// Pojedynczy wynik przywrócenia
/// </summary>
public class RestoredTechnologyResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Response dla batch delete - sukces
/// </summary>
public class BatchDeleteIgnoredResponseDto
{
    public string Message { get; set; } = string.Empty;
    public int RestoredCount { get; set; }
    public List<RestoredTechnologyResult> Restored { get; set; } = new();
}

/// <summary>
/// Pojedynczy wynik w batch delete multi-status
/// </summary>
public class BatchDeleteStatusResult
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Error { get; set; }
}

/// <summary>
/// Response dla batch delete - partial success (207)
/// </summary>
public class BatchDeleteIgnoredMultiStatusResponseDto
{
    public List<BatchDeleteStatusResult> Results { get; set; } = new();
    public int RestoredCount { get; set; }
}

// ============================================================================
// ADMIN DTOs (Section 2.7)
// ============================================================================

/// <summary>
/// KPI metryki dla admina
/// </summary>
public class AdminKpiDto
{
    public double ProfileCompletionRate { get; set; }
    public double ProfileCompletionTarget { get; set; }
    public bool ProfileCompletionMet { get; set; }
    public double TechnologyGenerationRate { get; set; }
    public double TechnologyGenerationTarget { get; set; }
    public bool TechnologyGenerationMet { get; set; }
}

/// <summary>
/// Statystyki użytkowników dla admina
/// </summary>
public class AdminUserStatsDto
{
    public int Total { get; set; }
    public int WithProfile { get; set; }
    public int WithoutProfile { get; set; }
    public int ActiveLastWeek { get; set; }
    public int ActiveLastMonth { get; set; }
}

/// <summary>
/// Statystyki technologii dla admina
/// </summary>
public class AdminTechnologyStatsDto
{
    public int TotalGenerated { get; set; }
    public int TotalInGraphs { get; set; }
    public int TotalIgnored { get; set; }
    public double AveragePerUser { get; set; }
    public double AverageProgress { get; set; }
    public int CompletedCount { get; set; }
}

/// <summary>
/// Statystyki AI dla admina
/// </summary>
public class AdminAiStatsDto
{
    public int TotalRequests { get; set; }
    public int CacheHits { get; set; }
    public double CacheHitRate { get; set; }
    public double AverageResponseTime { get; set; }
    public int Timeouts { get; set; }
    public int Errors { get; set; }
}

/// <summary>
/// Response dla GET /api/admin/metrics - metryki dashboardu
/// </summary>
public class AdminMetricsDto
{
    public AdminKpiDto Kpi { get; set; } = null!;
    public AdminUserStatsDto Users { get; set; } = null!;
    public AdminTechnologyStatsDto Technologies { get; set; } = null!;
    public AdminAiStatsDto Ai { get; set; } = null!;
    public DateTime GeneratedAt { get; set; }
}

/// <summary>
/// Pojedynczy użytkownik w liście admina
/// </summary>
public class AdminUserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool HasProfile { get; set; }
    public int TechnologiesCount { get; set; }
    public int AverageProgress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

/// <summary>
/// Response dla GET /api/admin/users - lista użytkowników
/// </summary>
public class AdminUsersListDto
{
    public List<AdminUserDto> Users { get; set; } = new();
    public int Count { get; set; }
    public int Total { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
}

/// <summary>
/// Popularny prefix/kategoria technologii
/// </summary>
public class PopularPrefixDto
{
    public string Prefix { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
    public int UniqueUsers { get; set; }
}

/// <summary>
/// Response dla GET /api/admin/technologies/popular
/// </summary>
public class PopularTechnologiesDto
{
    public List<PopularPrefixDto> PopularPrefixes { get; set; } = new();
    public int Total { get; set; }
    public DateTime GeneratedAt { get; set; }
}

/// <summary>
/// Statystyki dla pojedynczej wartości enum
/// </summary>
public class EnumStatDto
{
    public int Count { get; set; }
    public double Percentage { get; set; }
}

/// <summary>
/// Response dla GET /api/admin/statistics/profiles
/// </summary>
public class ProfileStatisticsDto
{
    public Dictionary<string, EnumStatDto> Roles { get; set; } = new();
    public Dictionary<string, EnumStatDto> DevelopmentAreas { get; set; } = new();
    public int TotalProfiles { get; set; }
    public DateTime GeneratedAt { get; set; }
}

// ============================================================================
// ERROR DTOs (Common)
// ============================================================================

/// <summary>
/// Pojedynczy błąd walidacji
/// </summary>
public class ValidationErrorDetail
{
    public string Field { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Standardowy format błędu (RFC 7807)
/// </summary>
public class ErrorResponseDto
{
    public string Error { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public List<ValidationErrorDetail>? Errors { get; set; }
}

/// <summary>
/// Response z prostą wiadomością
/// </summary>
public class MessageResponseDto
{
    public string Message { get; set; } = string.Empty;
}

