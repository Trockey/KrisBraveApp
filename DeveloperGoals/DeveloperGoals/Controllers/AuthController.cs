using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperGoals.Controllers;

/// <summary>
/// Kontroler obsługujący autentykację użytkowników.
/// </summary>
[Route("login")]
public class AuthController : Controller
{
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

        if (!result.Succeeded)
        {
            return RedirectToAction(nameof(LoginGoogle));
        }

        // TODO: Zapisanie lub aktualizacja użytkownika w bazie danych
        // TODO: Sprawdzenie czy użytkownik ma wypełniony profil

        return Redirect("/");
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

