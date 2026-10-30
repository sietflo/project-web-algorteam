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

// Додаємо підтримку OpenAPI, ProblemDetails і валідації
builder.Services.AddProblemDetails().AddValidation().AddOpenApi();

// Сховище оголошень (в пам'яті)
builder.Services.AddSingleton<ListingStore>();

var app = builder.Build();

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

// Підключаемо правило CORS
app.UseCors("AllowBlazorClient");

// Базовий ендпойнт для Health Check + логування
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
