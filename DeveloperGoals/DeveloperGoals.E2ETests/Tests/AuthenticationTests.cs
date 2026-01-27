using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using DeveloperGoals.E2ETests.Helpers;

namespace DeveloperGoals.E2ETests.Tests;

/// <summary>
/// Testy E2E dla funkcjonalności autentykacji
/// </summary>
[TestFixture]
public class AuthenticationTests : PageTest
{
    private string _baseUrl = null!;

    [SetUp]
    public async Task Setup()
    {
        _baseUrl = PlaywrightSettings.BaseUrl;
        SetDefaultExpectTimeout(PlaywrightSettings.DefaultTimeout);
    }

    [Test]
    public async Task TestLogin_ShouldLoginUserSuccessfully()
    {
        // Act
        await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);

        // Assert - sprawdź czy użytkownik jest na stronie głównej
        await Expect(Page).ToHaveURLAsync($"{_baseUrl}/");
        
        // Sprawdź czy użytkownik jest zalogowany
        var isLoggedIn = await TestAuthHelper.IsUserLoggedInAsync(Page);
        Assert.That(isLoggedIn, Is.True, "Użytkownik powinien być zalogowany");
    }

    [Test]
    public async Task TestLogin_WhenAccessingProtectedPage_ShouldAllowAccess()
    {
        // Arrange
        await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);

        // Act - spróbuj dostać się do chronionej strony (dashboard)
        await Page.GotoAsync($"{_baseUrl}/dashboard", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = PlaywrightSettings.NavigationTimeout
        });

        // Assert - powinien być dostęp do dashboardu (brak przekierowania do /login)
        await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*/dashboard.*"));
    }

    [Test]
    public async Task Logout_ShouldLogoutUserSuccessfully()
    {
        // Arrange
        await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);
        
        // Sprawdź że użytkownik jest zalogowany
        var isLoggedInBefore = await TestAuthHelper.IsUserLoggedInAsync(Page);
        Assert.That(isLoggedInBefore, Is.True, "Użytkownik powinien być zalogowany przed wylogowaniem");

        // Act
        await TestAuthHelper.LogoutAsync(Page, _baseUrl);

        // Assert - sprawdź czy użytkownik jest na stronie logowania
        await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*/login.*"));
        
        // Sprawdź czy użytkownik jest wylogowany
        var isLoggedInAfter = await TestAuthHelper.IsUserLoggedInAsync(Page);
        Assert.That(isLoggedInAfter, Is.False, "Użytkownik powinien być wylogowany");
    }

    [Test]
    public async Task TestLogin_WithInvalidParameter_ShouldNotLogin()
    {
        // Arrange - najpierw idź do strony logowania
        await Page.GotoAsync($"{_baseUrl}/login", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = PlaywrightSettings.NavigationTimeout
        });

        // Assert - użytkownik powinien być na stronie logowania i niezalogowany
        await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*/login$"));
        
        var isLoggedIn = await TestAuthHelper.IsUserLoggedInAsync(Page);
        Assert.That(isLoggedIn, Is.False, "Użytkownik nie powinien być zalogowany bez parametru test=true");
    }

    [TearDown]
    public async Task TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            var screenshotPath = Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "screenshots",
                $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            );
            
            Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)!);
            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath, FullPage = true });
            
            TestContext.WriteLine($"Screenshot saved: {screenshotPath}");
        }
    }
}
