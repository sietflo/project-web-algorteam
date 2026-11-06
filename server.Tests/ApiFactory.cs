using Microsoft.AspNetCore.Hosting;

// Сервер у процесі тестів над справжньою базою:
// connection string бере з контейнера, а не з налаштувань.
public sealed class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public TestDatabase Database { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.UseSetting("ConnectionStrings:DefaultConnection", Database.ConnectionString);

    public Task InitializeAsync() => Database.InitializeAsync();

    async Task IAsyncLifetime.DisposeAsync() => await Database.DisposeAsync();
}
