using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Claims;
using DeveloperGoals.Data;
using DeveloperGoals.DTOs;
using DeveloperGoals.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DeveloperGoals.Controllers;

/// <summary>
/// Kontroler obsługujący profil użytkownika
/// </summary>
[ApiController]
[Route("api/profile")]
// TODO: consider authorization for this controller
// [Authorize]
public class ProfileController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProfileController> _logger;
    private readonly IConfiguration _configuration;

    public ProfileController(
        ApplicationDbContext context,
        ILogger<ProfileController> logger,
        IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Pobiera profil bieżącego użytkownika
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(string? email = null)
    {
        // var userId = await GetCurrentUserIdAsync();
        // if (userId == null)
        // {
        //     return Unauthorized(new ErrorResponseDto
        //     {
        //         Error = "Unauthorized",
        //         Message = "User not authenticated"
        //     });
        // }

        (bool flowControl, IActionResult value, BigInteger? userId) = await GetUserId(email);
        if(!flowControl)
            return value;        

        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId.Value);

        if (profile == null)
        {
            return NotFound(new ErrorResponseDto
            {
                Error = "NotFound",
                Message = "Profile not found. Please complete onboarding."
            });
        }

        var dto = new UserProfileDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            MainTechnologies = profile.MainTechnologies
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToList(),
            Role = profile.Role.ToString(),
            DevelopmentArea = profile.DevelopmentArea.ToString(),
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt
        };

        return Ok(dto);
    }

    /// <summary>
    /// Tworzy profil użytkownika (onboarding)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateProfileResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileCommand command)
    {
        _logger.LogInformation("=== CreateProfile method called ===");
        _logger.LogInformation($"User authenticated: {User.Identity?.IsAuthenticated}");
        _logger.LogInformation($"User name: {User.Identity?.Name}");

        // TODO: consider to create method to get userId
        var email = command.Email;
        (bool flowControl, IActionResult value, BigInteger? userId) = await GetUserId(email);
        if(!flowControl)
            return value;

        // Sprawdź czy profil już istnieje
        var existingProfile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId.Value);

        if(existingProfile != null)
        {
            return Conflict(new ErrorResponseDto
            {
                Error = "Conflict",
                Message = "Profile already exists. Use PUT to update."
            });
        }

        // Walidacja
        var validationErrors = ValidateProfile(command);
        if(validationErrors.Any())
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "ValidationError",
                Message = "Validation failed",
                Errors = validationErrors
            });
        }

        // Konwersja enumów
        if(!Enum.TryParse<UserRole>(command.Role, out var role))
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "ValidationError",
                Message = "Invalid role value"
            });
        }

        if(!Enum.TryParse<DevelopmentArea>(command.DevelopmentArea, out var developmentArea))
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "ValidationError",
                Message = "Invalid development area value"
            });
        }

        // Utwórz profil
        var profile = new UserProfile
        {
            UserId = userId.Value,
            MainTechnologies = string.Join(",", command.MainTechnologies),
            Role = role,
            DevelopmentArea = developmentArea,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync();

        // Utwórz węzeł Start jeśli nie istnieje
        var startNodeExists = await _context.UserTechnologies
            .AnyAsync(t => t.UserId == userId.Value && t.IsStart);

        bool startNodeCreated = false;
        if(!startNodeExists)
        {
            var startDefinition = await _context.TechnologyDefinitions
                .FirstOrDefaultAsync(td => td.Id == 1);

            if(startDefinition != null)
            {
                var startTechnology = new UserTechnology
                {
                    UserId = userId.Value,
                    TechnologyDefinitionId = startDefinition.Id,
                    Name = startDefinition.Name,
                    Prefix = startDefinition.Prefix,
                    Tag = startDefinition.Tag,
                    SystemDescription = startDefinition.SystemDescription,
                    Status = TechnologyStatus.Active,
                    Progress = 0,
                    IsCustom = false,
                    IsStart = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.UserTechnologies.Add(startTechnology);
                await _context.SaveChangesAsync();
                startNodeCreated = true;
            }
        }

        var response = new CreateProfileResponseDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            MainTechnologies = command.MainTechnologies,
            Role = profile.Role.ToString(),
            DevelopmentArea = profile.DevelopmentArea.ToString(),
            CreatedAt = profile.CreatedAt,
            StartNodeCreated = startNodeCreated
        };

        return CreatedAtAction(nameof(GetProfile), response);
    }

    private async Task<(bool flowControl, IActionResult value, BigInteger? userId)> GetUserId(string? email = null)
    {
        BigInteger? userId = null;
        if(string.IsNullOrEmpty(email))
        {
            userId = await GetCurrentUserIdAsync();
            if(userId == null)
            {
                return (flowControl: false, value: Unauthorized(new ErrorResponseDto
                {
                    Error = "Unauthorized",
                    Message = "User not authenticated"
                }),
                null);
            }
        }
        else
        {
            userId = await GetCurrentUserIdAsyncByEmail(email);
            if(userId == null)
            {
                return (flowControl: false, value: Unauthorized(new ErrorResponseDto
                {
                    Error = "Unauthorized",
                    Message = "User not authenticated"
                })
                ,null);
            }
        }

        return (flowControl: true, value: null, userId: userId);
    }

    /// <summary>
    /// Aktualizuje profil użytkownika
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        //var userId = await GetCurrentUserIdAsync();
        //if (userId == null)
        //{
        //    return Unauthorized(new ErrorResponseDto
        //    {
        //        Error = "Unauthorized",
        //        Message = "User not authenticated"
        //    });
        //}

        var email = command.Email;
        (bool flowControl, IActionResult value, BigInteger? userId) = await GetUserId(email);
        if(!flowControl)
            return value;

        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId.Value);

        if (profile == null)
        {
            return NotFound(new ErrorResponseDto
            {
                Error = "NotFound",
                Message = "Profile not found. Use POST to create."
            });
        }

        // Walidacja
        var validationErrors = ValidateProfile(command);
        if (validationErrors.Any())
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "ValidationError",
                Message = "Validation failed",
                Errors = validationErrors
            });
        }

        // Konwersja enumów
        if (!Enum.TryParse<UserRole>(command.Role, out var role))
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "ValidationError",
                Message = "Invalid role value"
            });
        }

        if (!Enum.TryParse<DevelopmentArea>(command.DevelopmentArea, out var developmentArea))
        {
            return BadRequest(new ErrorResponseDto
            {
                Error = "ValidationError",
                Message = "Invalid development area value"
            });
        }

        // Aktualizuj profil
        profile.MainTechnologies = string.Join(",", command.MainTechnologies);
        profile.Role = role;
        profile.DevelopmentArea = developmentArea;
        profile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var dto = new UserProfileDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            MainTechnologies = command.MainTechnologies,
            Role = profile.Role.ToString(),
            DevelopmentArea = profile.DevelopmentArea.ToString(),
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt
        };

        return Ok(dto);
    }

    /// <summary>
    /// Pobiera ID bieżącego użytkownika z claims
    /// </summary>
    private async Task<BigInteger?> GetCurrentUserIdAsync()
    {
        // Pobierz email z claims (Google OAuth zapisuje email w ClaimTypes.Email)
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }

        // Znajdź użytkownika po emailu
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        return user?.Id;
    }

    private async Task<BigInteger?> GetCurrentUserIdAsyncByEmail(string email)
    {
        // Znajdź użytkownika po emailu
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        return user?.Id;
    }    

    /// <summary>
    /// Waliduje dane profilu
    /// </summary>
    private List<ValidationErrorDetail> ValidateProfile(CreateProfileCommand command)
    {
        var errors = new List<ValidationErrorDetail>();

        // Get configuration values
        var minTechnologies = _configuration.GetValue<int>("Profile:MinTechnologies", 2);

        if (command.MainTechnologies == null || command.MainTechnologies.Count < minTechnologies)
        {
            errors.Add(new ValidationErrorDetail
            {
                Field = "mainTechnologies",
                Message = $"Profile must contain at least {minTechnologies} technologies"
            });
        }

        if (command.MainTechnologies != null && command.MainTechnologies.Count > 50)
        {
            errors.Add(new ValidationErrorDetail
            {
                Field = "mainTechnologies",
                Message = "Maximum 50 technologies allowed"
            });
        }

        if (command.MainTechnologies != null)
        {
            var duplicates = command.MainTechnologies
                .GroupBy(t => t, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicates.Any())
            {
                errors.Add(new ValidationErrorDetail
                {
                    Field = "mainTechnologies",
                    Message = $"Duplicate technologies found: {string.Join(", ", duplicates)}"
                });
            }

            foreach (var tech in command.MainTechnologies)
            {
                if (string.IsNullOrWhiteSpace(tech))
                {
                    errors.Add(new ValidationErrorDetail
                    {
                        Field = "mainTechnologies",
                        Message = "Technology name cannot be empty"
                    });
                    break;
                }

                if (tech.Length > 100)
                {
                    errors.Add(new ValidationErrorDetail
                    {
                        Field = "mainTechnologies",
                        Message = $"Technology name '{tech}' exceeds maximum length of 100 characters"
                    });
                }
            }
        }

        if (string.IsNullOrWhiteSpace(command.Role))
        {
            errors.Add(new ValidationErrorDetail
            {
                Field = "role",
                Message = "Role is required"
            });
        }

        if (string.IsNullOrWhiteSpace(command.DevelopmentArea))
        {
            errors.Add(new ValidationErrorDetail
            {
                Field = "developmentArea",
                Message = "Development area is required"
            });
        }

        return errors;
    }

    /// <summary>
    /// Waliduje dane profilu (dla UpdateProfileCommand)
    /// </summary>
    private List<ValidationErrorDetail> ValidateProfile(UpdateProfileCommand command)
    {
        return ValidateProfile(new CreateProfileCommand
        {
            MainTechnologies = command.MainTechnologies,
            Role = command.Role,
            DevelopmentArea = command.DevelopmentArea
        });
    }
}
