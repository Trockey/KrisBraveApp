using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using DeveloperGoals.E2ETests.PageObjects;
using DeveloperGoals.E2ETests.Helpers;

namespace DeveloperGoals.E2ETests.Tests;

/// <summary>
/// Testy E2E dla strony Dashboard
/// </summary>
[TestFixture]
public class DashboardTests : PageTest
{
    private string _baseUrl = null!;

    [SetUp]
    public async Task Setup()
    {
        _baseUrl = PlaywrightSettings.BaseUrl;
        SetDefaultExpectTimeout(PlaywrightSettings.DefaultTimeout);
    }

    [Test]
    public async Task Dashboard_WhenLoggedIn_ShouldLoadSuccessfully()
    {
        // Arrange
        await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);
        var dashboardPage = new DashboardPage(Page, _baseUrl);

        // Act
        await dashboardPage.NavigateAsync();
        
        // Poczekaj na załadowanie strony
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert - sprawdź że jesteśmy na dashboardzie
        await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*/dashboard"));
        
        // Sprawdź czy tytuł lub główny kontener dashboardu jest widoczny
        var mainContent = Page.Locator(".max-w-7xl");
        await Expect(mainContent).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 15000 });
    }

    [Test]
    [Ignore("Wymaga działającego grafu technologii - użytkownik testowy musi mieć profil i dane")]
    public async Task Dashboard_ShouldDisplayGraphVisualizer()
    {
        // Arrange
        await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);
        var dashboardPage = new DashboardPage(Page, _baseUrl);

        // Act
        await dashboardPage.NavigateAsync();
        await dashboardPage.WaitForGraphToLoadAsync();

        // Assert
        var isGraphVisible = await dashboardPage.IsGraphVisibleAsync();
        Assert.That(isGraphVisible, Is.True, "Graf technologii powinien być widoczny");
    }

    [Test]
    [Ignore("Wymaga działającego grafu technologii - użytkownik testowy musi mieć profil i dane")]
    public async Task Dashboard_GraphVisualizer_ShouldLoadVisJs()
    {
        // Ten test sprawdza czy vis.js jest zainicjalizowany
        // Arrange
        await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);
        var dashboardPage = new DashboardPage(Page, _baseUrl);

        // Act
        await dashboardPage.NavigateAsync();
        await dashboardPage.WaitForGraphToLoadAsync();

        // Assert - sprawdź czy canvas został utworzony przez vis.js
        var canvasExists = await Page.Locator("#graph-visualizer canvas").IsVisibleAsync();
        Assert.That(canvasExists, Is.True, "Canvas vis.js powinien być widoczny");
    }

    [Test]
    [Ignore("Wymaga danych testowych - użytkownik Id=2 musi mieć dodane technologie w bazie")]
    public async Task Dashboard_WhenUserHasTechnologies_ShouldDisplayInGraph()
    {
        // TODO: Przygotuj dane testowe (dodaj technologie dla użytkownika Id=2)
        // Arrange
        await TestAuthHelper.LoginAsTestUserAsync(Page, _baseUrl);
        var dashboardPage = new DashboardPage(Page, _baseUrl);

        // Act
        await dashboardPage.NavigateAsync();
        await dashboardPage.WaitForGraphToLoadAsync();

        // Assert
        var nodesCount = await dashboardPage.GetGraphNodesCountAsync();
        Assert.That(nodesCount, Is.GreaterThan(0), "Graf powinien zawierać węzły technologii");
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
