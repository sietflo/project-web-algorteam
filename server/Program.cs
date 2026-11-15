using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1. Конфігурація логування (в консоль і дебаггер)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// 2. Конфігурація CORS (дозволяємо запити від Blazor WASM)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5042", "https://localhost:7134") // Порти Blazor-клієнта (client/Properties/launchSettings.json)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 3. Аутентифікація та політики авторизації (Microsoft Entra ID — Блок 3)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        options.TokenValidationParameters.ValidAudiences = new[]
        {
            "6b99ada1-1ca0-4f7f-9a81-86cf13b17134",
            "api://6b99ada1-1ca0-4f7f-9a81-86cf13b17134"
        };
    },
    options => { builder.Configuration.Bind("AzureAd", options); });

builder.Services.AddAuthorizationBuilder()
    // 3а: Читання вимагає scope access_as_user
    .AddPolicy("ReadListings", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireScope("access_as_user");
    })
    // 3б: Кожна операція зміни вимагає ще й ролі з історій користувача
    .AddPolicy("WriteListings", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireScope("access_as_user");
        policy.RequireRole("User", "Admin");
    });

// Додаємо підтримку OpenAPI, ProblemDetails і валідації
builder.Services.AddProblemDetails().AddValidation().AddOpenApi();

// Підключення PostgreSQL через Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Database connection string is missing.");

    options.UseNpgsql(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DevelopmentDataSeeder.SeedAsync(db);
}

// Логер для відслідковування старту сервера
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Ініціалізація сервера NearBuy API...");

// Настройка HTTP-пайплайна
// Лог кожного запиту: метод, шлях, код і тривалість
app.UseMiddleware<RequestLog>();

// Помилки й коди відповіді повертаються як ProblemDetails
app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Підключаємо правило CORS (обов'язково ДО UseAuthentication)
app.UseCors("AllowBlazorClient");

// Підключаємо перевірку JWT-токенів та політик доступу
app.UseAuthentication();
app.UseAuthorization();

// Базовий ендпойнт для Health Check + логування (залишаємо відкритим)
app.MapGet("/health", (ILogger<Program> log) =>
{
    log.LogInformation("Перевірка стану сервера: Health check викликаний.");
    return Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
})
.WithName("HealthCheck");

// Оголошення: /api/listings
app.MapListings();

logger.LogInformation("Сервер успішно запущений!");

app.Run();

// Потрібно, щоб інтеграційні тести могли запустити сервер
public partial class Program;