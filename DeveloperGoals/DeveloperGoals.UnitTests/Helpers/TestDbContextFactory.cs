using Microsoft.EntityFrameworkCore;
using DeveloperGoals.Data;

namespace DeveloperGoals.UnitTests.Helpers;

/// <summary>
/// Fabryka do tworzenia testowych instancji DbContext z InMemory database
/// </summary>
public static class TestDbContextFactory
{
    /// <summary>
    /// Tworzy nowy ApplicationDbContext z bazą danych InMemory
    /// </summary>
    /// <param name="databaseName">Nazwa bazy danych (powinna być unikalna dla każdego testu)</param>
    /// <returns>Skonfigurowany ApplicationDbContext</returns>
    public static ApplicationDbContext CreateInMemoryContext(string? databaseName = null)
    {
        var dbName = databaseName ?? Guid.NewGuid().ToString();
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new ApplicationDbContext(options);
    }

    /// <summary>
    /// Tworzy kontekst z przykładowymi danymi testowymi
    /// </summary>
    public static ApplicationDbContext CreateInMemoryContextWithData()
    {
        var context = CreateInMemoryContext();
        SeedTestData(context);
        return context;
    }

    /// <summary>
    /// Wypełnia kontekst przykładowymi danymi testowymi
    /// </summary>
    private static void SeedTestData(ApplicationDbContext context)
    {
        // Przykładowe dane testowe
        // TODO: Dodaj seeding danych w zależności od potrzeb testów
        
        context.SaveChanges();
    }
}
