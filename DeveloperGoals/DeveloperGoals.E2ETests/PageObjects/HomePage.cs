using Microsoft.Playwright;

namespace DeveloperGoals.E2ETests.PageObjects;

/// <summary>
/// Page Object dla strony głównej aplikacji
/// </summary>
public class HomePage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public HomePage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    // Locatory
    private ILocator LoginButton => _page.Locator("a[href='/login']").First;
    private ILocator WelcomeMessage => _page.GetByRole(AriaRole.Heading, new() { NameRegex = new System.Text.RegularExpressions.Regex("Witaj|Cześć|Brawo") });
    private ILocator DashboardLink => _page.Locator("a[href='/dashboard'], a[href='dashboard']").First;

    // Akcje
    public async Task NavigateAsync()
    {
        await _page.GotoAsync(_baseUrl, new PageGotoOptions 
        { 
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = PlaywrightSettings.NavigationTimeout 
        });
    }

    public async Task ClickLoginAsync()
    {
        await LoginButton.ClickAsync();
    }

    public async Task NavigateToDashboardAsync()
    {
        await DashboardLink.ClickAsync();
        await _page.WaitForURLAsync("**/dashboard", new PageWaitForURLOptions 
        { 
            Timeout = PlaywrightSettings.NavigationTimeout 
        });
    }

    // Asercje pomocnicze
    public ILocator GetWelcomeMessage() => WelcomeMessage;
    
    public async Task<bool> IsLoginButtonVisibleAsync()
    {
        return await LoginButton.IsVisibleAsync();
    }

    public async Task<bool> IsDashboardLinkVisibleAsync()
    {
        return await DashboardLink.IsVisibleAsync();
    }
}
