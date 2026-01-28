using System.Numerics;
using System.Security.Claims;
using DeveloperGoals.Data;
using DeveloperGoals.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeveloperGoals.Controllers;

/// <summary>
/// Kontroler obsługujący autentykację użytkowników.
/// </summary>
[Route("login")]
public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }
    /// <summary>
    /// Inicjuje proces logowania testowego przez parametr URL.
    /// </summary>
    [HttpGet("test")]
    public async Task<IActionResult> LoginTest([FromQuery] bool test = false)
    {
        if (!test)
        {
            return Redirect("/login");
        }

        // Sprawdź czy użytkownik o Id=2 istnieje w bazie
        var testUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == BigInteger.Parse("2"));

        if (testUser == null)
        {
            // Utwórz testowego użytkownika
            testUser = new User
            {
                Id = BigInteger.Parse("2"),
                GoogleId = "108226413010999999999",
                Email = "tester@test.com",
                Name = "Test",
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };

            _context.Users.Add(testUser);
            await _context.SaveChangesAsync();

        }
        else
        {
            // Aktualizuj datę ostatniego logowania
            testUser.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        // Zaloguj użytkownika
        await SignInUser(testUser);

        return Redirect("/");
    }

    /// <summary>
    /// Inicjuje proces logowania przez Google OAuth.
    /// </summary>
    [HttpGet("google")]
    public IActionResult LoginGoogle()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleResponse))
        };

        return Challenge(properties, "Google");
    }

    /// <summary>
    /// Obsługuje odpowiedź z Google OAuth.
    /// </summary>
    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if(!result.Succeeded)
        {
            return RedirectToAction(nameof(LoginGoogle));
        }

        // Pobierz dane użytkownika z claims
        // Google OAuth zwraca "sub" jako identyfikator użytkownika, który jest mapowany na ClaimTypes.NameIdentifier
        var googleId = result.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                       result.Principal?.FindFirst("sub")?.Value;
        var email = result.Principal?.FindFirst(ClaimTypes.Email)?.Value;
        var name = result.Principal?.FindFirst(ClaimTypes.Name)?.Value ??
                   result.Principal?.FindFirst("name")?.Value ??
                   email ?? "Użytkownik";

        if(string.IsNullOrEmpty(googleId) || string.IsNullOrEmpty(email))
        {
            // Brak wymaganych danych z Google OAuth
            return RedirectToAction(nameof(LoginGoogle));
        }

        var user = await AddOrUpdateUser(googleId, email, name);

        // Zaloguj użytkownika (uwierzytelnienie już jest przez Google OAuth, więc tylko aktualizujemy claims jeśli potrzeba)
        await SignInUser(user);

        return Redirect("/");
    }

    private async Task<User> AddOrUpdateUser(string googleId, string email, string name)
    {
        // Sprawdź, czy użytkownik już istnieje w bazie
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.GoogleId == googleId);

        User user;

        if(existingUser != null)
        {
            // Aktualizuj dane użytkownika i datę ostatniego logowania
            existingUser.Email = email;
            existingUser.Name = name;
            existingUser.LastLoginAt = DateTime.UtcNow;
            user = existingUser;
        }
        else
        {
            // Utwórz nowego użytkownika
            var newUserId = await GenerateNewUserIdAsync();

            var newUser = new User
            {
                Id = newUserId,
                GoogleId = googleId,
                Email = email,
                Name = name,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            user = newUser;
        }

        await _context.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Zalogowuje użytkownika tworząc sesję cookie z claims.
    /// </summary>
    private async Task SignInUser(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.GoogleId),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim("UserId", user.Id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            authProperties);
    }

    /// <summary>
    /// Generuje nowy unikalny ID dla użytkownika.
    /// </summary>
    private async Task<BigInteger> GenerateNewUserIdAsync()
    {
        // Sprawdź czy są użytkownicy w bazie
        var hasUsers = await _context.Users.AnyAsync();
        
        if (!hasUsers)
        {
            // Jeśli baza jest pusta, zwróć 1
            return BigInteger.One;
        }

        // Pobierz maksymalne ID używając OrderByDescending
        var maxIdUser = await _context.Users
            .OrderByDescending(u => u.Id)
            .Select(u => u.Id)
            .FirstOrDefaultAsync();

        // Zwróć maksymalne ID + 1
        return maxIdUser + 1;
    }

    /// <summary>
    /// Wylogowuje użytkownika.
    /// </summary>
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/login");
    }
}

