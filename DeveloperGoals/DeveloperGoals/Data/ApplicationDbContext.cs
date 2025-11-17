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
    public DbSet<Technology> Technologies { get; set; } = null!;

    /// <summary>
    /// Tabela zależności między technologiami.
    /// </summary>
    public DbSet<TechnologyDependency> TechnologyDependencies { get; set; } = null!;

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

            // Relacja 1:N z Technology
            entity.HasMany(e => e.Technologies)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja 1:N z TechnologyDependency
            entity.HasMany(e => e.TechnologyDependencies)
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

        // Konfiguracja Technology
        modelBuilder.Entity<Technology>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.Name });
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Tag).IsRequired();
            entity.Property(e => e.SystemDescription).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.PrivateDescription).HasMaxLength(2000);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Progress).IsRequired();
            entity.Property(e => e.IsCustom).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.AiReasoning).HasMaxLength(1000);
        });

        // Konfiguracja TechnologyDependency
        modelBuilder.Entity<TechnologyDependency>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.FromTechnologyId, e.ToTechnologyId }).IsUnique();

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
    }
}

