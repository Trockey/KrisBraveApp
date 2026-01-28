using DeveloperGoals.Models;
using Microsoft.EntityFrameworkCore;

namespace DeveloperGoals.Data;

/// <summary>
/// Kontekst bazy danych aplikacji DeveloperGoals.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Tabela użytkowników.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Tabela profili użytkowników.
    /// </summary>
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;

    /// <summary>
    /// Tabela technologii.
    /// </summary>
    public DbSet<UserTechnology> UserTechnologies { get; set; } = null!;

    /// <summary>
    /// Tabela zależności między technologiami.
    /// </summary>
    public DbSet<TechnologyDependency> TechnologyDependencies { get; set; } = null!;

    /// <summary>
    /// Tabela definicji technologii (wspólny słownik).
    /// </summary>
    public DbSet<TechnologyDefinition> TechnologyDefinitions { get; set; } = null!;

    /// <summary>
    /// Tabela ignorowanych technologii.
    /// </summary>
    public DbSet<IgnoredTechnology> IgnoredTechnologies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguracja User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.GoogleId).IsUnique();
            entity.HasIndex(e => e.Email);
            entity.Property(e => e.GoogleId).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.CreatedAt).IsRequired();

            // Relacja 1:1 z UserProfile
            entity.HasOne(e => e.Profile)
                .WithOne(e => e.User)
                .HasForeignKey<UserProfile>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja 1:N z UserTechnology
            entity.HasMany(e => e.Technologies)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja 1:N z TechnologyDependency
            entity.HasMany(e => e.TechnologyDependencies)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja 1:N z IgnoredTechnology
            entity.HasMany(e => e.IgnoredTechnologies)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Konfiguracja UserProfile
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.Property(e => e.MainTechnologies).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Role).IsRequired();
            entity.Property(e => e.DevelopmentArea).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        // Konfiguracja UserTechnology
        modelBuilder.Entity<UserTechnology>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.Name });
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.TechnologyDefinitionId).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Tag).IsRequired();
            entity.Property(e => e.SystemDescription).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.PrivateDescription).HasMaxLength(2000);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Progress).IsRequired();
            entity.Property(e => e.IsCustom).IsRequired();
            entity.Property(e => e.IsStart).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.AiReasoning).HasMaxLength(1000);

            // Relacja N:1 z TechnologyDefinition
            entity.HasOne(e => e.TechnologyDefinition)
                .WithMany(e => e.UserTechnologies)
                .HasForeignKey(e => e.TechnologyDefinitionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Konfiguracja TechnologyDependency
        modelBuilder.Entity<TechnologyDependency>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.FromTechnologyId, e.ToTechnologyId }).IsUnique();
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.FromTechnologyId);

            // Relacja z FromTechnology (opcjonalna - dla węzła Start)
            entity.HasOne(e => e.FromTechnology)
                .WithMany()
                .HasForeignKey(e => e.FromTechnologyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacja z ToTechnology (wymagana)
            entity.HasOne(e => e.ToTechnology)
                .WithMany()
                .HasForeignKey(e => e.ToTechnologyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.CreatedAt).IsRequired();
        });

        // Konfiguracja TechnologyDefinition
        modelBuilder.Entity<TechnologyDefinition>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.Name, e.Prefix, e.Tag }).IsUnique();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Prefix).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Tag).IsRequired();
            entity.Property(e => e.SystemDescription).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).IsRequired();

            // Seed data dla węzła "Start"
            entity.HasData(new TechnologyDefinition
            {
                Id = 1,
                Name = "Start",
                Prefix = "System",
                Tag = TechnologyTag.Technologia,
                SystemDescription = "Węzeł startowy grafu technologii. Automatycznie tworzony dla każdego użytkownika.",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
        });

        // Konfiguracja IgnoredTechnology
        modelBuilder.Entity<IgnoredTechnology>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.Name, e.Category }).IsUnique();
            entity.HasIndex(e => e.UserId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Tag).IsRequired();
            entity.Property(e => e.SystemDescription).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.AiReasoning).HasMaxLength(1000);
            entity.Property(e => e.IgnoredAt).IsRequired();

            // Relacja z ContextTechnology (opcjonalna)
            entity.HasOne(e => e.ContextTechnology)
                .WithMany()
                .HasForeignKey(e => e.ContextTechnologyId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
