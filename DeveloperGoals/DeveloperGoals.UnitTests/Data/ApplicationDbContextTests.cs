using System.Numerics;
using FluentAssertions;
using DeveloperGoals.Models;
using DeveloperGoals.UnitTests.Helpers;

namespace DeveloperGoals.UnitTests.Data;

/// <summary>
/// Testy integracyjne dla ApplicationDbContext z InMemory database
/// </summary>
public class ApplicationDbContextTests
{
    [Fact]
    public async Task DbContext_ShouldAddAndRetrieveUser()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateInMemoryContext();
        
        var user = new User
        {
            Id = BigInteger.Parse("123"),
            GoogleId = "google123",
            Email = "test@example.com",
            Name = "Test User",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var retrievedUser = await context.Users.FindAsync(user.Id);

        // Assert
        retrievedUser.Should().NotBeNull();
        retrievedUser!.Email.Should().Be("test@example.com");
        retrievedUser.Name.Should().Be("Test User");
    }

    [Fact]
    public async Task DbContext_ShouldAddUserTechnology_WithTechnologyDefinition()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateInMemoryContext();
        
        var userId = BigInteger.Parse("123");
        var user = new User
        {
            Id = userId,
            GoogleId = "google123",
            Email = "test@example.com",
            Name = "Test User"
        };

        var techDefinition = new TechnologyDefinition
        {
            Name = "C#"
        };

        var userTech = new UserTechnology
        {
            UserId = userId,
            User = user,
            TechnologyDefinition = techDefinition,
            Progress = 50,
            Category = "Language",
            Prefix = "DotNet"
        };

        // Act
        context.Users.Add(user);
        context.TechnologyDefinitions.Add(techDefinition);
        context.UserTechnologies.Add(userTech);
        await context.SaveChangesAsync();

        // Assert
        var retrievedUserTech = context.UserTechnologies
            .FirstOrDefault(ut => ut.UserId == userId);

        retrievedUserTech.Should().NotBeNull();
        retrievedUserTech!.Progress.Should().Be(50);
        retrievedUserTech.Category.Should().Be("Language");
    }

    [Fact]
    public async Task DbContext_ShouldRetrieveUserWithTechnologies()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateInMemoryContext();
        
        var userId = BigInteger.Parse("123");
        var user = new User
        {
            Id = userId,
            GoogleId = "google123",
            Email = "test@example.com",
            Name = "Test User"
        };

        var tech1 = new TechnologyDefinition { Name = "C#" };
        var tech2 = new TechnologyDefinition { Name = "TypeScript" };

        context.Users.Add(user);
        context.TechnologyDefinitions.AddRange(tech1, tech2);
        
        context.UserTechnologies.Add(new UserTechnology 
        { 
            User = user, 
            TechnologyDefinition = tech1,
            Progress = 40,
            Category = "Language",
            Prefix = "DotNet"
        });
        context.UserTechnologies.Add(new UserTechnology 
        { 
            User = user, 
            TechnologyDefinition = tech2,
            Progress = 30,
            Category = "Language",
            Prefix = "JavaScript"
        });
        
        await context.SaveChangesAsync();

        // Act
        var userTechnologies = context.UserTechnologies
            .Where(ut => ut.UserId == userId)
            .ToList();

        // Assert
        userTechnologies.Should().HaveCount(2);
        userTechnologies.Select(ut => ut.Progress).Should().Contain(new[] { 40, 30 });
    }

    [Fact]
    public async Task DbContext_ShouldDeleteUserTechnology()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateInMemoryContext();
        
        var userId = BigInteger.Parse("123");
        var user = new User 
        { 
            Id = userId, 
            GoogleId = "google123",
            Email = "test@example.com",
            Name = "Test User"
        };
        var tech = new TechnologyDefinition { Name = "C#" };
        var userTech = new UserTechnology 
        { 
            User = user, 
            TechnologyDefinition = tech,
            Progress = 40,
            Category = "Language",
            Prefix = "DotNet"
        };

        context.Users.Add(user);
        context.TechnologyDefinitions.Add(tech);
        context.UserTechnologies.Add(userTech);
        await context.SaveChangesAsync();

        // Act
        context.UserTechnologies.Remove(userTech);
        await context.SaveChangesAsync();

        var deletedTech = context.UserTechnologies.FirstOrDefault(ut => ut.Id == userTech.Id);

        // Assert
        deletedTech.Should().BeNull();
    }

    [Fact]
    public async Task DbContext_ShouldUpdateUserTechnology()
    {
        // Arrange
        await using var context = TestDbContextFactory.CreateInMemoryContext();
        
        var userId = BigInteger.Parse("123");
        var user = new User 
        { 
            Id = userId, 
            GoogleId = "google123",
            Email = "test@example.com",
            Name = "Test User"
        };
        var tech = new TechnologyDefinition { Name = "C#" };
        var userTech = new UserTechnology 
        { 
            User = user, 
            TechnologyDefinition = tech,
            Progress = 20,
            Category = "Language",
            Prefix = "DotNet"
        };

        context.Users.Add(user);
        context.TechnologyDefinitions.Add(tech);
        context.UserTechnologies.Add(userTech);
        await context.SaveChangesAsync();

        // Act
        userTech.Progress = 60;
        await context.SaveChangesAsync();

        var updatedTech = await context.UserTechnologies.FindAsync(userTech.Id);

        // Assert
        updatedTech.Should().NotBeNull();
        updatedTech!.Progress.Should().Be(60);
    }
}
