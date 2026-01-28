using System.Numerics;
using System.Security.Claims;
using DeveloperGoals.Data;
using DeveloperGoals.DTOs;
using DeveloperGoals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeveloperGoals.Controllers;

/// <summary>
/// Kontroler obsługujący technologie użytkownika
/// </summary>
[ApiController]
[Route("api/technologies")]
// TODO: consider authorization for this controller
// [Authorize]
public class TechnologyController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TechnologyController> _logger;

    public TechnologyController(
        ApplicationDbContext context,
        ILogger<TechnologyController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Pobiera listę technologii użytkownika
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(TechnologiesListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTechnologies([FromQuery] string? email = null)
    {
        var (flowControl, value, userId) = await GetUserId(email);
        if (!flowControl)
            return value;

        var technologies = await _context.UserTechnologies
            .Where(t => t.UserId == userId.Value)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        var dto = new TechnologiesListDto
        {
            Technologies = technologies.Select(t => MapToDto(t)).ToList(),
            Count = technologies.Count
        };

        _logger.LogInformation("Pobrano {Count} technologii dla użytkownika {UserId}", dto.Count, userId);
        return Ok(dto);
    }

    /// <summary>
    /// Pobiera graf technologii użytkownika
    /// </summary>
    [HttpGet("graph")]
    [ProducesResponseType(typeof(TechnologyGraphDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGraph([FromQuery] string? email = null)
    {
        var (flowControl, value, userId) = await GetUserId(email);
        if (!flowControl)
            return value;

        var technologies = await _context.UserTechnologies
            .Where(t => t.UserId == userId.Value)
            .ToListAsync();

        var dependencies = await _context.TechnologyDependencies
            .Where(d => d.UserId == userId.Value)
            .ToListAsync();

        var nodes = technologies.Select(t => new GraphNodeDto
        {
            Id = t.Id,
            Name = t.Name,
            Prefix = t.Prefix,
            Tag = t.Tag.ToString(),
            Progress = t.Progress,
            IsStart = t.IsStart,
            SystemDescription = t.SystemDescription
        }).ToList();

        var edges = dependencies.Select(d => new GraphEdgeDto
        {
            Id = d.Id,
            From = d.FromTechnologyId ?? 0,
            To = d.ToTechnologyId,
            CreatedAt = d.CreatedAt
        }).ToList();

        var stats = new GraphStatsDto
        {
            TotalNodes = nodes.Count,
            TotalEdges = edges.Count,
            AverageProgress = technologies.Any() ? (int)technologies.Average(t => t.Progress) : 0,
            CompletedCount = technologies.Count(t => t.Progress == 100)
        };

        var dto = new TechnologyGraphDto
        {
            Nodes = nodes,
            Edges = edges,
            Stats = stats
        };

        _logger.LogInformation("Pobrano graf dla użytkownika {UserId}: {NodeCount} węzłów, {EdgeCount} krawędzi",
            userId, nodes.Count, edges.Count);

        return Ok(dto);
    }

    /// <summary>
    /// Dodaje nową technologię do grafu użytkownika
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateTechnologyResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddTechnology([FromBody] CreateTechnologyCommand command, [FromQuery] string? email = null)
    {
        var (flowControl, value, userId) = await GetUserId(email);
        if (!flowControl)
            return value;

        // Sprawdź czy technologia już nie istnieje w grafie użytkownika
        var exists = await _context.UserTechnologies
            .AnyAsync(t => t.UserId == userId.Value && t.TechnologyDefinitionId == command.TechnologyDefinitionId);

        if (exists)
        {
            return Conflict(new ErrorResponseDto
            {
                Error = "Conflict",
                Message = "Technology already exists in user graph"
            });
        }

        // Pobierz definicję technologii
        var definition = await _context.TechnologyDefinitions
            .FirstOrDefaultAsync(td => td.Id == command.TechnologyDefinitionId);

        if (definition == null)
        {
            return NotFound(new ErrorResponseDto
            {
                Error = "NotFound",
                Message = "Technology definition not found"
            });
        }

        // Sprawdź czy FromTechnologyId istnieje i należy do użytkownika
        if (command.FromTechnologyId > 0)
        {
            var fromTech = await _context.UserTechnologies
                .FirstOrDefaultAsync(t => t.Id == command.FromTechnologyId && t.UserId == userId.Value);

            if (fromTech == null)
            {
                return BadRequest(new ErrorResponseDto
                {
                    Error = "BadRequest",
                    Message = "Source technology not found or does not belong to user"
                });
            }
        }

        // Utwórz nową technologię
        var technology = new UserTechnology
        {
            UserId = userId.Value,
            TechnologyDefinitionId = definition.Id,
            Name = definition.Name,
            Prefix = definition.Prefix,
            Category = definition.Prefix,
            Tag = definition.Tag,
            SystemDescription = definition.SystemDescription,
            PrivateDescription = command.PrivateDescription,
            Status = TechnologyStatus.Active,
            Progress = 0,
            IsCustom = command.IsCustom,
            IsStart = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserTechnologies.Add(technology);
        await _context.SaveChangesAsync();

        // Utwórz zależność
        var dependency = new TechnologyDependency
        {
            UserId = userId.Value,
            FromTechnologyId = command.FromTechnologyId > 0 ? command.FromTechnologyId : null,
            ToTechnologyId = technology.Id,
            CreatedAt = DateTime.UtcNow
        };

        _context.TechnologyDependencies.Add(dependency);
        await _context.SaveChangesAsync();

        var response = new CreateTechnologyResponseDto
        {
            Technology = MapToDto(technology),
            Dependency = MapToDependencyDto(dependency)
        };

        _logger.LogInformation("Dodano technologię {TechId} dla użytkownika {UserId}", technology.Id, userId);

        return CreatedAtAction(nameof(GetTechnologies), response);
    }

    /// <summary>
    /// Dodaje wiele technologii jednocześnie (batch)
    /// </summary>
    [HttpPost("batch")]
    [ProducesResponseType(typeof(BatchAddSuccessResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BatchAddMultiStatusResponseDto), StatusCodes.Status207MultiStatus)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTechnologiesBatch([FromBody] BatchAddTechnologiesCommand command, [FromQuery] string? email = null)
    {
        BigInteger? userId = BigInteger.Parse(command.GoogleId);
        if(userId == null)
        {
            _logger.LogError("Google ID is required at method AddTechnologiesBatch: {GoogleId}", command.GoogleId);

            return BadRequest(new ErrorResponseDto
            {
                Error = "BadRequest",
                Message = "User ID is required at method AddTechnologiesBatch"
            });
        }

        // var (flowControl, value, userId2) = await GetUserId(email);
        // if (!flowControl)
        //     return value;

        if (command.Technologies == null || !command.Technologies.Any())
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "BadRequest",
                Message = "Technologies list is empty"
            });
        }

        var user = await _context.Users.FirstOrDefaultAsync(t => t.GoogleId == command.GoogleId);
        if(user == null)
        {
            _logger.LogError("User not found at method AddTechnologiesBatch: {GoogleId}", command.GoogleId);
            return BadRequest(new ErrorResponseDto
            {
                Error = "BadRequest",
                Message = "User not found at method AddTechnologiesBatch"
            });
        }

        // Sprawdź czy FromTechnologyId istnieje
        if (command.FromTechnologyId > 0)
        {
            var fromTech = await _context.UserTechnologies
                .FirstOrDefaultAsync(t => t.Id == command.FromTechnologyId && t.UserId == user.Id);

            if (fromTech == null)
            {
                return BadRequest(new ErrorResponseDto
                {
                    Error = "BadRequest",
                    Message = "Source technology not found or does not belong to user"
                });
            }
        }

        var results = new List<BatchTechnologyResult>();
        var statusResults = new List<BatchStatusResult>();
        var successCount = 0;
        var errorCount = 0;

        foreach (var item in command.Technologies)
        {
            try
            {
                // Sprawdź czy technologia już nie istnieje
                var exists = await _context.UserTechnologies
                    .AnyAsync(t => t.UserId == userId.Value && t.TechnologyDefinitionId == item.TechnologyDefinitionId);

                if (exists)
                {
                    statusResults.Add(new BatchStatusResult
                    {
                        TechnologyDefinitionId = item.TechnologyDefinitionId,
                        Status = "Conflict",
                        Error = "Technology already exists in user graph"
                    });
                    errorCount++;
                    continue;
                }

                // Pobierz definicję
                var definition = await _context.TechnologyDefinitions
                    .FirstOrDefaultAsync(td => td.Id == item.TechnologyDefinitionId);

                // TODO:
                // if (definition == null)
                // {
                //     statusResults.Add(new BatchStatusResult
                //     {
                //         TechnologyDefinitionId = item.TechnologyDefinitionId,
                //         Status = "NotFound",
                //         Error = "Technology definition not found"
                //     });
                //     errorCount++;
                //     continue;
                // }


                var technologyDefinition = new TechnologyDefinition
                {
                    Name = item.SystemDescription,
                    // Prefix = item.Prefix,
                    // Tag = item.Tag,
                    SystemDescription = item.AiReasoning,
                    CreatedAt = DateTime.UtcNow
                };

                _context.TechnologyDefinitions.Add(technologyDefinition);
                await _context.SaveChangesAsync();

                definition = technologyDefinition;

                // Utwórz technologię
                var technology = new UserTechnology
                {
                    UserId = new BigInteger((decimal)user.Id),
                    TechnologyDefinitionId = definition.Id,
                    Name = definition.Name,
                    Prefix = definition.Prefix,
                    Category = definition.Prefix,
                    Tag = definition.Tag,
                    SystemDescription = definition.SystemDescription,
                    AiReasoning = item.AiReasoning ?? string.Empty,
                    PrivateDescription = item.PrivateDescription,
                    Status = TechnologyStatus.Active,
                    Progress = 0,
                    IsCustom = false,
                    IsStart = false,
                    CreatedAt = DateTime.UtcNow
                };

                _context.UserTechnologies.Add(technology);
                await _context.SaveChangesAsync();

                // Utwórz zależność
                var dependency = new TechnologyDependency
                {
                    UserId = new BigInteger((decimal)user.Id),
                    FromTechnologyId = command.FromTechnologyId > 0 ? command.FromTechnologyId : null,
                    ToTechnologyId = technology.Id,
                    CreatedAt = DateTime.UtcNow
                };

                _context.TechnologyDependencies.Add(dependency);
                await _context.SaveChangesAsync();

                results.Add(new BatchTechnologyResult
                {
                    Technology = MapToDto(technology),
                    Dependency = MapToDependencyDto(dependency)
                });

                statusResults.Add(new BatchStatusResult
                {
                    TechnologyDefinitionId = item.TechnologyDefinitionId,
                    Status = "Success",
                    TechnologyId = technology.Id
                });

                successCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas dodawania technologii {TechDefId}", item.TechnologyDefinitionId);
                statusResults.Add(new BatchStatusResult
                {
                    TechnologyDefinitionId = item.TechnologyDefinitionId,
                    Status = "Error",
                    Error = ex.Message
                });
                errorCount++;
            }
        }

        _logger.LogInformation("Batch add: {Success} sukces, {Error} błędów dla użytkownika {UserId}",
            successCount, errorCount, userId);

        // Jeśli wszystkie się powiodły, zwróć 200
        if (errorCount == 0)
        {
            return Ok(new BatchAddSuccessResponseDto
            {
                Added = results,
                Count = results.Count
            });
        }

        // Jeśli były błędy, zwróć 207 Multi-Status
        return StatusCode(StatusCodes.Status207MultiStatus, new BatchAddMultiStatusResponseDto
        {
            Results = statusResults,
            SuccessCount = successCount,
            ErrorCount = errorCount
        });
    }

    /// <summary>
    /// Tworzy własną (custom) technologię użytkownika
    /// </summary>
    [HttpPost("custom")]
    [ProducesResponseType(typeof(CreateCustomTechnologyResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCustomTechnology([FromBody] CreateCustomTechnologyCommand command, [FromQuery] string? email = null)
    {
        var (flowControl, value, userId) = await GetUserId(email);
        if (!flowControl)
            return value;

        // Walidacja
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "ValidationError",
                Message = "Technology name is required"
            });
        }

        // Sprawdź czy FromTechnologyId istnieje
        if (command.FromTechnologyId > 0)
        {
            var fromTech = await _context.UserTechnologies
                .FirstOrDefaultAsync(t => t.Id == command.FromTechnologyId && t.UserId == userId.Value);

            if (fromTech == null)
            {
                return BadRequest(new ErrorResponseDto
                {
                    Error = "BadRequest",
                    Message = "Source technology not found or does not belong to user"
                });
            }
        }

        // Parse Tag enum
        if (!Enum.TryParse<TechnologyTag>(command.Tag, out var tag))
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "ValidationError",
                Message = "Invalid tag value"
            });
        }

        // Utwórz definicję technologii
        var definition = new TechnologyDefinition
        {
            Name = command.Name,
            Prefix = command.Prefix,
            Tag = tag,
            SystemDescription = command.SystemDescription,
            CreatedAt = DateTime.UtcNow
        };

        _context.TechnologyDefinitions.Add(definition);
        await _context.SaveChangesAsync();

        // Utwórz technologię użytkownika
        var technology = new UserTechnology
        {
            UserId = userId.Value,
            TechnologyDefinitionId = definition.Id,
            Name = definition.Name,
            Prefix = definition.Prefix,
            Category = definition.Prefix,
            Tag = definition.Tag,
            SystemDescription = definition.SystemDescription,
            PrivateDescription = command.PrivateDescription,
            Status = TechnologyStatus.Active,
            Progress = 0,
            IsCustom = true,
            IsStart = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserTechnologies.Add(technology);
        await _context.SaveChangesAsync();

        // Utwórz zależność
        var dependency = new TechnologyDependency
        {
            UserId = userId.Value,
            FromTechnologyId = command.FromTechnologyId > 0 ? command.FromTechnologyId : null,
            ToTechnologyId = technology.Id,
            CreatedAt = DateTime.UtcNow
        };

        _context.TechnologyDependencies.Add(dependency);
        await _context.SaveChangesAsync();

        var response = new CreateCustomTechnologyResponseDto
        {
            TechnologyDefinition = new TechnologyDefinitionDto
            {
                Id = definition.Id,
                Name = definition.Name,
                Prefix = definition.Prefix,
                Tag = definition.Tag.ToString(),
                SystemDescription = definition.SystemDescription,
                CreatedAt = definition.CreatedAt
            },
            Technology = MapToDto(technology),
            Dependency = MapToDependencyDto(dependency)
        };

        _logger.LogInformation("Utworzono własną technologię {TechId} dla użytkownika {UserId}", technology.Id, userId);

        return CreatedAtAction(nameof(GetTechnologies), response);
    }

    /// <summary>
    /// Aktualizuje technologię użytkownika
    /// </summary>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(UpdateTechnologyResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTechnology(int id, [FromBody] UpdateTechnologyCommand command, [FromQuery] string? email = null)
    {
        var (flowControl, value, userId) = await GetUserId(email);
        if (!flowControl)
            return value;

        var technology = await _context.UserTechnologies
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId.Value);

        if (technology == null)
        {
            return NotFound(new ErrorResponseDto
            {
                Error = "NotFound",
                Message = "Technology not found or does not belong to user"
            });
        }

        // Aktualizuj pola
        if (command.Progress.HasValue)
        {
            if (command.Progress.Value < 0 || command.Progress.Value > 100)
            {
                return BadRequest(new ErrorResponseDto
                {
                    Error = "ValidationError",
                    Message = "Progress must be between 0 and 100"
                });
            }

            technology.Progress = command.Progress.Value;
        }

        if (command.PrivateDescription != null)
        {
            technology.PrivateDescription = command.PrivateDescription;
        }

        technology.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var response = new UpdateTechnologyResponseDto
        {
            Id = technology.Id,
            Progress = technology.Progress,
            PrivateDescription = technology.PrivateDescription,
            UpdatedAt = technology.UpdatedAt.Value
        };

        _logger.LogInformation("Zaktualizowano technologię {TechId} dla użytkownika {UserId}", id, userId);

        return Ok(response);
    }

    /// <summary>
    /// Usuwa technologię użytkownika (i związane z nią zależności)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DeleteTechnologyResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTechnology(int id, [FromQuery] string? email = null)
    {
        var (flowControl, value, userId) = await GetUserId(email);
        if (!flowControl)
            return value;

        var technology = await _context.UserTechnologies
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId.Value);

        if (technology == null)
        {
            return NotFound(new ErrorResponseDto
            {
                Error = "NotFound",
                Message = "Technology not found or does not belong to user"
            });
        }

        // Nie można usunąć węzła startowego
        if (technology.IsStart)
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "BadRequest",
                Message = "Cannot delete start node"
            });
        }

        // Usuń wszystkie zależności związane z tą technologią
        var dependencies = await _context.TechnologyDependencies
            .Where(d => d.UserId == userId.Value && (d.FromTechnologyId == id || d.ToTechnologyId == id))
            .ToListAsync();

        var dependencyCount = dependencies.Count;

        _context.TechnologyDependencies.RemoveRange(dependencies);
        _context.UserTechnologies.Remove(technology);

        await _context.SaveChangesAsync();

        var response = new DeleteTechnologyResponseDto
        {
            Message = "Technology and related dependencies deleted successfully",
            DeletedDependencies = dependencyCount
        };

        _logger.LogInformation("Usunięto technologię {TechId} i {DepCount} zależności dla użytkownika {UserId}",
            id, dependencyCount, userId);

        return Ok(response);
    }

    #region Helper Methods

    private async Task<(bool flowControl, IActionResult value, BigInteger? userId)> GetUserId(string? email = null)
    {
        BigInteger? userId = null;
        if (string.IsNullOrEmpty(email))
        {
            userId = await GetCurrentUserIdAsync();
            if (userId == null)
            {
                return (flowControl: false, value: Unauthorized(new ErrorResponseDto
                {
                    Error = "Unauthorized",
                    Message = "User not authenticated"
                }), null);
            }
        }
        else
        {
            userId = await GetCurrentUserIdAsyncByEmail(email);
            if (userId == null)
            {
                return (flowControl: false, value: Unauthorized(new ErrorResponseDto
                {
                    Error = "Unauthorized",
                    Message = "User not authenticated"
                }), null);
            }
        }

        return (flowControl: true, value: null, userId: userId);
    }

    private async Task<BigInteger?> GetCurrentUserIdAsync()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        return user?.Id;
    }

    private async Task<BigInteger?> GetCurrentUserIdAsyncByEmail(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        return user?.Id;
    }

    private static TechnologyDto MapToDto(UserTechnology technology)
    {
        return new TechnologyDto
        {
            Id = technology.Id,
            UserId = technology.UserId,
            TechnologyDefinitionId = technology.TechnologyDefinitionId,
            Name = technology.Name,
            Prefix = technology.Prefix,
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

    private static DependencyDto MapToDependencyDto(TechnologyDependency dependency)
    {
        return new DependencyDto
        {
            Id = dependency.Id,
            UserId = dependency.UserId,
            FromTechnologyId = dependency.FromTechnologyId,
            ToTechnologyId = dependency.ToTechnologyId,
            CreatedAt = dependency.CreatedAt
        };
    }

    #endregion
}
