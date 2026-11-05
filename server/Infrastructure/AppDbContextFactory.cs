using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using server.Entities; // <-- підключаємо папку Entities

namespace server.Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        optionsBuilder.UseNpgsql("Host=localhost;Database=nearbuy;Username=postgres;Password=postgres");

        return new AppDbContext(optionsBuilder.Options);
    }
}