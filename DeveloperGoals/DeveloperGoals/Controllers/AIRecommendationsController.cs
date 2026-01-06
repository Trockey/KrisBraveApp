using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Claims;
using DeveloperGoals.DTOs;
using DeveloperGoals.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperGoals.Controllers;

/// <summary>
/// Kontroler obsługujący rekomendacje AI technologii
/// </summary>
[ApiController]
[Route("api/ai")]
// TODO: Włączyć autoryzację po dodaniu uwierzytelniania ??
// [Authorize]
public class AIRecommendationsController : ControllerBase
{
    private readonly IAIRecommendationService _recommendationService;
    private readonly ILogger<AIRecommendationsController> _logger;

    public AIRecommendationsController(
        IAIRecommendationService recommendationService,
        ILogger<AIRecommendationsController> logger)
    {
        _recommendationService = recommendationService;
        _logger = logger;
    }

    /// <summary>
    /// Generuje rekomendacje technologii do nauki przy użyciu AI
    /// </summary>
    /// <param name="command">Komenda z parametrami generowania</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Lista rekomendacji z informacjami o cache</returns>
    /// <response code="200">Rekomendacje wygenerowane pomyślnie</response>
    /// <response code="400">Błąd walidacji (profil niekompletny, technologia nie należy do użytkownika, itp.)</response>
    /// <response code="404">Technologia źródłowa nie znaleziona</response>
    /// <response code="408">Timeout AI service (przekroczono 20s)</response>
    /// <response code="500">Błąd AI service</response>
    /// <response code="502">Nieprawidłowy format odpowiedzi AI</response>
    [HttpPost("recommendations")]
    [ProducesResponseType(typeof(RecommendationsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status408RequestTimeout)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> GenerateRecommendations(
        [FromBody] GenerateRecommendationsCommandWithValidation command,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      
        BigInteger googleUserId;
        if (string.IsNullOrEmpty(userIdClaim) || !BigInteger.TryParse(userIdClaim, out googleUserId))
        {
            _logger.LogWarning("User ID not found in claims");
            return Unauthorized(new ErrorResponseDto
            {
                Error = "Unauthorized",
                Message = "User not authenticated"
            });
        }

        _logger.LogInformation(
            "Generating recommendations for user {UserId}, technology {TechId}",
            googleUserId, command.FromTechnologyId);

        var result = await _recommendationService.GenerateRecommendationsAsync(
            googleUserId,
            new GenerateRecommendationsCommand
            {
                FromTechnologyId = command.FromTechnologyId,
                ContextTechnologyIds = command.ContextTechnologyIds
            },
            cancellationToken);

        return Ok(result);
    }
}

/// <summary>
/// Command z walidacją DataAnnotations
/// </summary>
public class GenerateRecommendationsCommandWithValidation
{
    /// <summary>
    /// ID technologii źródłowej
    /// </summary>
    [Required(ErrorMessage = "fromTechnologyId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid technology ID")]
    public int FromTechnologyId { get; set; }

    /// <summary>
    /// Lista ID technologii kontekstowych (opcjonalne)
    /// </summary>
    [MaxLength(50, ErrorMessage = "Maximum 50 context technologies allowed")]
    public List<int>? ContextTechnologyIds { get; set; }
}
