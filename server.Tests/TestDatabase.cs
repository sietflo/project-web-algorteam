using Microsoft.EntityFrameworkCore;
using server.Infrastructure;
using Testcontainers.PostgreSql;

// PostgreSQL 18 у контейнері на час прогону; схему створює та сама міграція, що й у команди.
public sealed class TestDatabase : IAsyncLifetime
{
    private readonly PostgreSqlContainer pg = new PostgreSqlBuilder("postgres:18-alpine").Build();

    public string ConnectionString => pg.GetConnectionString();

    public async Task InitializeAsync()
    {
        await pg.StartAsync();
        await using var db = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString).Options);
        await db.Database.MigrateAsync();

        // Користувач і категорія, без яких POST /api/listings відповідає 409
        await DevelopmentDataSeeder.SeedAsync(db);
    }

    public Task DisposeAsync() => pg.DisposeAsync().AsTask();
}
