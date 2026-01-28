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
/// Kontroler obsługujący ignorowane technologie użytkownika
/// </summary>
[ApiController]
[Route("api/ignored-technologies")]
// TODO: consider authorization for this controller
// [Authorize]
public class IgnoredTechnologyController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IgnoredTechnologyController> _logger;

    public IgnoredTechnologyController(
        ApplicationDbContext context,
        ILogger<IgnoredTechnologyController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Pobiera listę ignorowanych technologii użytkownika z paginacją
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IgnoredTechnologiesListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetIgnored([FromQuery] int limit = 50, [FromQuery] int offset = 0, [FromQuery] string? email = null)
    {
        var (flowControl, value, userId) = await GetUserId(email);
        if (!flowControl)
            return value;

        // Walidacja parametrów paginacji
        if (limit < 1 || limit > 100)
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "ValidationError",
                Message = "Limit must be between 1 and 100"
            });
        }

        if (offset < 0)
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "ValidationError",
                Message = "Offset must be non-negative"
            });
        }

        // Pobierz całkowitą liczbę ignorowanych technologii
        var total = await _context.IgnoredTechnologies
            .CountAsync(it => it.UserId == userId.Value);

        // Pobierz ignorowane technologie z paginacją
        var ignoredTechnologies = await _context.IgnoredTechnologies
            .Where(it => it.UserId == userId.Value)
            .Include(it => it.ContextTechnology)
            .OrderByDescending(it => it.IgnoredAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        var dto = new IgnoredTechnologiesListDto
        {
            IgnoredTechnologies = ignoredTechnologies.Select(MapToDto).ToList(),
            Count = ignoredTechnologies.Count,
            Total = total,
            Limit = limit,
            Offset = offset
        };

        _logger.LogInformation("Pobrano {Count} ignorowanych technologii (z {Total} łącznie) dla użytkownika {UserId}",
            dto.Count, total, userId);

        return Ok(dto);
    }

    /// <summary>
    /// Dodaje technologie do listy ignorowanych
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AddIgnoredTechnologyResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddIgnored([FromBody] AddIgnoredTechnologyCommand command, [FromQuery] string? email = null)
    {
        var (flowControl, value, userId) = await GetUserId(email);
        if (!flowControl)
            return value;

        if (command.Technologies == null || !command.Technologies.Any())
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "ValidationError",
                Message = "Technologies list is empty"
            });
        }

        var addedResults = new List<AddedIgnoredTechnologyResult>();

        foreach (var item in command.Technologies)
        {
            // Walidacja
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                _logger.LogWarning("Pominięto technologię z pustą nazwą dla użytkownika {UserId}", userId);
                continue;
            }

            // Parse Tag enum
            if (!Enum.TryParse<TechnologyTag>(item.Tag, out var tag))
            {
                _logger.LogWarning("Nieprawidłowy tag {Tag} dla technologii {Name}, użyto wartości domyślnej",
                    item.Tag, item.Name);
                tag = TechnologyTag.Technologia; // Wartość domyślna
            }

            // Sprawdź czy ContextTechnologyId jest prawidłowe (jeśli podane)
            if (item.ContextTechnologyId.HasValue && item.ContextTechnologyId.Value > 0)
            {
                var contextTech = await _context.UserTechnologies
                    .FirstOrDefaultAsync(t => t.Id == item.ContextTechnologyId.Value && t.UserId == userId.Value);

                if (contextTech == null)
                {
                    _logger.LogWarning("Nieprawidłowe ContextTechnologyId {ContextTechId} dla użytkownika {UserId}",
                        item.ContextTechnologyId, userId);
                    // Kontynuuj bez context technology
                }
            }

            // Sprawdź czy technologia już nie jest ignorowana
            var alreadyIgnored = await _context.IgnoredTechnologies
                .AnyAsync(it => it.UserId == userId.Value && it.Name == item.Name);

            if (alreadyIgnored)
            {
                _logger.LogInformation("Technologia {Name} jest już ignorowana dla użytkownika {UserId}",
                    item.Name, userId);
                continue;
            }

            // Utwórz wpis ignorowanej technologii
            var ignoredTechnology = new IgnoredTechnology
            {
                UserId = userId.Value,
                Name = item.Name,
                Category = item.Category,
                Tag = tag,
                SystemDescription = item.SystemDescription,
                AiReasoning = item.AiReasoning,
                ContextTechnologyId = item.ContextTechnologyId,
                IgnoredAt = DateTime.UtcNow
            };

            _context.IgnoredTechnologies.Add(ignoredTechnology);
            await _context.SaveChangesAsync();

            addedResults.Add(new AddedIgnoredTechnologyResult
            {
                Id = ignoredTechnology.Id,
                Name = ignoredTechnology.Name,
                Category = ignoredTechnology.Category,
                IgnoredAt = ignoredTechnology.IgnoredAt
            });
        }

        var response = new AddIgnoredTechnologyResponseDto
        {
            Added = addedResults,
            Count = addedResults.Count
        };

        _logger.LogInformation("Dodano {Count} technologii do listy ignorowanych dla użytkownika {UserId}",
            response.Count, userId);

        return CreatedAtAction(nameof(GetIgnored), response);
    }

    /// <summary>
    /// Przywraca technologię z listy ignorowanych (usuwa z archiwum)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DeleteIgnoredTechnologyResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RestoreIgnored(int id, [FromQuery] string? email = null)
    {
        var (flowControl, value, userId) = await GetUserId(email);
        if (!flowControl)
            return value;

        var ignoredTechnology = await _context.IgnoredTechnologies
            .FirstOrDefaultAsync(it => it.Id == id && it.UserId == userId.Value);

        if (ignoredTechnology == null)
        {
            return NotFound(new ErrorResponseDto
            {
                Error = "NotFound",
                Message = "Ignored technology not found or does not belong to user"
            });
        }

        var name = ignoredTechnology.Name;

        _context.IgnoredTechnologies.Remove(ignoredTechnology);
        await _context.SaveChangesAsync();

        var response = new DeleteIgnoredTechnologyResponseDto
        {
            Message = "Technology restored successfully (removed from ignored list)",
            Id = id,
            Name = name
        };

        _logger.LogInformation("Przywrócono technologię {TechName} (ID: {Id}) dla użytkownika {UserId}",
            name, id, userId);

        return Ok(response);
    }

    /// <summary>
    /// Przywraca wiele technologii z listy ignorowanych jednocześnie (batch)
    /// </summary>
    [HttpDelete("batch")]
    [ProducesResponseType(typeof(BatchDeleteIgnoredResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BatchDeleteIgnoredMultiStatusResponseDto), StatusCodes.Status207MultiStatus)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RestoreBatch([FromBody] BatchDeleteIgnoredCommand command, [FromQuery] string? email = null)
    {
        var (flowControl, value, userId) = await GetUserId(email);
        if (!flowControl)
            return value;

        if (command.Ids == null || !command.Ids.Any())
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "ValidationError",
                Message = "Ids list is empty"
            });
        }

        var restoredResults = new List<RestoredTechnologyResult>();
        var statusResults = new List<BatchDeleteStatusResult>();
        var restoredCount = 0;

        foreach (var id in command.Ids)
        {
            try
            {
                var ignoredTechnology = await _context.IgnoredTechnologies
                    .FirstOrDefaultAsync(it => it.Id == id && it.UserId == userId.Value);

                if (ignoredTechnology == null)
                {
                    statusResults.Add(new BatchDeleteStatusResult
                    {
                        Id = id,
                        Status = "NotFound",
                        Error = "Ignored technology not found or does not belong to user"
                    });
                    continue;
                }

                var name = ignoredTechnology.Name;

                _context.IgnoredTechnologies.Remove(ignoredTechnology);
                await _context.SaveChangesAsync();

                restoredResults.Add(new RestoredTechnologyResult
                {
                    Id = id,
                    Name = name
                });

                statusResults.Add(new BatchDeleteStatusResult
                {
                    Id = id,
                    Status = "Success"
                });

                restoredCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas przywracania technologii {Id}", id);
                statusResults.Add(new BatchDeleteStatusResult
                {
                    Id = id,
                    Status = "Error",
                    Error = ex.Message
                });
            }
        }

        _logger.LogInformation("Batch restore: przywrócono {Count} technologii dla użytkownika {UserId}",
            restoredCount, userId);

        // Jeśli wszystkie się powiodły, zwróć 200
        if (restoredCount == command.Ids.Count)
        {
            return Ok(new BatchDeleteIgnoredResponseDto
            {
                Message = $"Successfully restored {restoredCount} technologies from ignored list",
                RestoredCount = restoredCount,
                Restored = restoredResults
            });
        }

        // Jeśli były błędy, zwróć 207 Multi-Status
        return StatusCode(StatusCodes.Status207MultiStatus, new BatchDeleteIgnoredMultiStatusResponseDto
        {
            Results = statusResults,
            RestoredCount = restoredCount
        });
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

    private static IgnoredTechnologyDto MapToDto(IgnoredTechnology ignoredTechnology)
    {
        return new IgnoredTechnologyDto
        {
            Id = ignoredTechnology.Id,
            UserId = ignoredTechnology.UserId,
            Name = ignoredTechnology.Name,
            Category = ignoredTechnology.Category,
            Tag = ignoredTechnology.Tag.ToString(),
            SystemDescription = ignoredTechnology.SystemDescription,
            AiReasoning = ignoredTechnology.AiReasoning,
            ContextTechnologyId = ignoredTechnology.ContextTechnologyId,
            ContextTechnologyName = ignoredTechnology.ContextTechnology?.Name,
            IgnoredAt = ignoredTechnology.IgnoredAt
        };
    }

    #endregion
}
