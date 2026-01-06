using DeveloperGoals.Client.Pages;
using DeveloperGoals.Components;
using DeveloperGoals.Configuration;
using DeveloperGoals.Data;
using DeveloperGoals.Middleware;
using DeveloperGoals.Services;
using Microsoft.EntityFrameworkCore;

namespace DeveloperGoals;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        // Add DbContext with PostgreSQL
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions => npgsqlOptions.MigrationsAssembly("DeveloperGoals")));

        // Add Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Cookies";
            options.DefaultChallengeScheme = "Google";
        })
        .AddCookie("Cookies")
        .AddGoogle("Google", options =>
        {
            options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? throw new InvalidOperationException("Google ClientId not configured");
            options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret not configured");
            // options.CallbackPath = "/login/google-response";
            options.SaveTokens = true;
        });

        builder.Services.AddAuthorization();
        builder.Services.AddCascadingAuthenticationState();

        // Configuration Options
        builder.Services.Configure<OpenRouterOptions>(
            builder.Configuration.GetSection(OpenRouterOptions.SectionName));
        builder.Services.Configure<CacheOptions>(
            builder.Configuration.GetSection(CacheOptions.SectionName));

        // Memory Cache
        builder.Services.AddMemoryCache(options =>
        {
            options.SizeLimit = 1000; // Max 1000 entries
        });

        // HttpClient for OpenRouter
        builder.Services.AddHttpClient<IOpenRouterService, OpenRouterService>((sp, client) =>
        {
            var config = builder.Configuration.GetSection(OpenRouterOptions.SectionName);
            var apiKey = config["ApiKey"] ?? throw new InvalidOperationException("OpenRouter ApiKey not configured");
            var baseUrl = config["BaseUrl"] ?? "https://openrouter.ai/api/v1";
            var timeout = int.Parse(config["Timeout"] ?? "20");
            var appUrl = config["AppUrl"] ?? "https://developergoals.app";
            var appTitle = config["AppTitle"] ?? "DeveloperGoals";

            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(timeout);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            client.DefaultRequestHeaders.Add("HTTP-Referer", appUrl);
            client.DefaultRequestHeaders.Add("X-Title", appTitle);
        });

        // HttpClient for Blazor Server components
        builder.Services.AddScoped(sp =>
        {
            var navigationManager = sp.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();
            return new HttpClient
            // TODO: check if code below is necesary
            // (new HttpClientHandler
            //     {
            //         UseCookies = true,
            //         CookieContainer = new System.Net.CookieContainer()
            //     }
            // )
            {
                BaseAddress = new Uri(navigationManager.BaseUri)
            };
        });

        // AI Services
        builder.Services.AddScoped<IAIRecommendationService, AIRecommendationService>();

        // Exception Handler
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapControllers();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

        app.Run();
    }
}
