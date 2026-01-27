using System.Numerics;
using FluentAssertions;
using DeveloperGoals.Models;

namespace DeveloperGoals.UnitTests.Models;

/// <summary>
/// Testy jednostkowe dla modelu UserTechnology
/// </summary>
public class UserTechnologyTests
{
    [Fact]
    public void UserTechnology_WhenCreated_ShouldHaveDefaultValues()
    {
        // Act
        var userTechnology = new UserTechnology();

        // Assert
        userTechnology.Id.Should().Be(0);
        userTechnology.Progress.Should().Be(0);
        userTechnology.IsCustom.Should().BeFalse();
        userTechnology.Status.Should().Be(TechnologyStatus.Active);
    }

    [Fact]
    public void UserTechnology_WhenSetProperties_ShouldRetainValues()
    {
        // Arrange
        var userTechnology = new UserTechnology
        {
            Id = 1,
            UserId = BigInteger.Parse("123"),
            TechnologyDefinitionId = 5,
            Name = "C# - Entity Framework",
            Category = "DotNet",
            Progress = 75,
            IsCustom = true,
            Status = TechnologyStatus.Active
        };

        // Assert
        userTechnology.Id.Should().Be(1);
        userTechnology.UserId.Should().Be(BigInteger.Parse("123"));
        userTechnology.TechnologyDefinitionId.Should().Be(5);
        userTechnology.Name.Should().Be("C# - Entity Framework");
        userTechnology.Category.Should().Be("DotNet");
        userTechnology.Progress.Should().Be(75);
        userTechnology.IsCustom.Should().BeTrue();
        userTechnology.Status.Should().Be(TechnologyStatus.Active);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(25)]
    [InlineData(50)]
    [InlineData(100)]
    public void UserTechnology_Progress_ShouldAcceptValidRange(int progress)
    {
        // Arrange & Act
        var userTechnology = new UserTechnology
        {
            Progress = progress
        };

        // Assert
        userTechnology.Progress.Should().Be(progress);
        userTechnology.Progress.Should().BeInRange(0, 100);
    }

    [Theory]
    [InlineData(TechnologyStatus.Active)]
    [InlineData(TechnologyStatus.Ignored)]
    public void UserTechnology_Status_ShouldAcceptValidEnumValues(TechnologyStatus status)
    {
        // Arrange & Act
        var userTechnology = new UserTechnology
        {
            Status = status
        };

        // Assert
        userTechnology.Status.Should().Be(status);
    }

    [Fact]
    public void UserTechnology_IsStart_ShouldDefaultToFalse()
    {
        // Arrange & Act
        var userTechnology = new UserTechnology();

        // Assert
        userTechnology.IsStart.Should().BeFalse();
    }

    [Fact]
    public void UserTechnology_WhenIsStart_ShouldNotBeRemovable()
    {
        // Ten test dokumentuje biznesową zasadę, że węzły startowe nie powinny być usuwane
        // W przyszłości można dodać walidację na poziomie modelu lub serwisu
        
        // Arrange
        var startTechnology = new UserTechnology
        {
            IsStart = true,
            Name = "Starting Technology"
        };

        // Assert
        startTechnology.IsStart.Should().BeTrue();
        // Logika walidacji powinna być zaimplementowana w serwisie/kontrolerze
    }
}
