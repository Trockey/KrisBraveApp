using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using DeveloperGoals.E2ETests.PageObjects;
using DeveloperGoals.E2ETests.Helpers;

namespace DeveloperGoals.E2ETests.Tests;

/// <summary>
/// Testy E2E dla strony głównej
/// </summary>
[TestFixture]
public class HomePageTests : PageTest
{
    private string _baseUrl = null!;

    [SetUp]
    public async Task Setup()
    {
        _baseUrl = PlaywrightSettings.BaseUrl;
        
        // Ustawienie domyślnych timeoutów
        SetDefaultExpectTimeout(PlaywrightSettings.DefaultTimeout);
    }

    [Test]
    public async Task HomePage_ShouldLoad_Successfully()
    {
        // Arrange
        var homePage = new HomePage(Page, _baseUrl);

        // Act
        await homePage.NavigateAsync();

        // Assert - strona powinna się załadować
        await Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("DeveloperGoals|Brawo"));
    }

    [Test]
    public async Task HomePage_WhenNotLoggedIn_ShouldShowLoginButton()
    {
        // Arrange
        var homePage = new HomePage(Page, _baseUrl);

        // Act
        await homePage.NavigateAsync();

        // Assert - przycisk logowania powinien być widoczny
        var isLoginVisible = await homePage.IsLoginButtonVisibleAsync();
        Assert.That(isLoginVisible, Is.True, "Przycisk logowania powinien być widoczny dla niezalogowanych użytkowników");
    }

    [Test]
    public async Task HomePage_AfterLogin_ShouldShowDashboard()
    {
        // Arrange
        await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);
        var homePage = new HomePage(Page, _baseUrl);

        // Act
        await homePage.NavigateAsync();

        // Assert - link do dashboardu powinien być widoczny dla zalogowanych użytkowników
        var isDashboardLinkVisible = await homePage.IsDashboardLinkVisibleAsync();
        Assert.That(isDashboardLinkVisible, Is.True, "Link do dashboardu powinien być widoczny dla zalogowanych użytkowników");
        
        // Sprawdź czy użytkownik jest zalogowany
        var isLoggedIn = await TestAuthHelper.IsUserLoggedInAsync(Page);
        Assert.That(isLoggedIn, Is.True, "Użytkownik powinien być zalogowany");
    }

    [TearDown]
    public async Task TearDown()
    {
        // Zrzut ekranu przy błędzie testu
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
