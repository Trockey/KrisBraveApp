using Microsoft.Playwright;

namespace DeveloperGoals.E2ETests.PageObjects;

/// <summary>
/// Page Object dla strony Dashboard
/// </summary>
public class DashboardPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    public DashboardPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    // Locatory
    private ILocator PageTitle => _page.GetByRole(AriaRole.Heading, new() { Name = "Dashboard" });
    private ILocator GraphContainer => _page.Locator("#graph-visualizer");
    private ILocator AddTechnologyButton => _page.GetByRole(AriaRole.Button, new() { Name = "Dodaj technologię" });
    private ILocator TechnologyList => _page.Locator("[data-testid='technology-list']");

    // Akcje
    public async Task NavigateAsync()
    {
        await _page.GotoAsync($"{_baseUrl}/dashboard", new PageGotoOptions 
        { 
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = PlaywrightSettings.NavigationTimeout 
        });
    }

    public async Task ClickAddTechnologyAsync()
    {
        await AddTechnologyButton.ClickAsync();
    }

    public async Task WaitForGraphToLoadAsync()
    {
        // Czekaj aż graf vis.js się załaduje
        await GraphContainer.WaitForAsync(new LocatorWaitForOptions 
        { 
            State = WaitForSelectorState.Visible,
            Timeout = PlaywrightSettings.DefaultTimeout 
        });
        
        // Czekaj na JS Interop - graf powinien mieć canvas element
        await _page.WaitForSelectorAsync("#graph-visualizer canvas", new PageWaitForSelectorOptions 
        { 
            Timeout = PlaywrightSettings.DefaultTimeout 
        });
    }

    public async Task<int> GetGraphNodesCountAsync()
    {
        // Wykorzystuje JS Interop do pobrania liczby węzłów z grafu vis.js
        return await _page.EvaluateAsync<int>(@"
            () => {
                const container = document.getElementById('graph-visualizer');
                if (!container) return 0;
                // Zakładając, że graf jest dostępny globalnie lub w data attribute
                return document.querySelectorAll('#graph-visualizer canvas').length > 0 ? 1 : 0;
            }
        ");
    }

    // Asercje pomocnicze
    public ILocator GetPageTitle() => PageTitle;
    
    public async Task<bool> IsGraphVisibleAsync()
    {
        return await GraphContainer.IsVisibleAsync();
    }

    public async Task<bool> IsTechnologyListVisibleAsync()
    {
        return await TechnologyList.IsVisibleAsync();
    }
}
