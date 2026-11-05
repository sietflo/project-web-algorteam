using Microsoft.EntityFrameworkCore;
using server.Entities; // <-- підключаємо папку Entities

namespace server.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<server.Entities.Listing> Listings => Set<server.Entities.Listing>(); // явний namespace
    public DbSet<ListingImage> Images => Set<ListingImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Унікальні індекси (2a)
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        // Правила OnDelete (2б)
        modelBuilder.Entity<server.Entities.Listing>()
            .HasMany(l => l.Images)
            .WithOne(i => i.Listing)
            .HasForeignKey(i => i.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Listings)
            .WithOne(l => l.Category)
            .HasForeignKey(l => l.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Listings)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}