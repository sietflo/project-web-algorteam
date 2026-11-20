using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

// Сервер у процесі тестів над справжньою базою:
// connection string бере з контейнера, а не з налаштувань.
public sealed class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string TestScheme = "TestJwt";
    private const string TestSigningSecret = "super-secret-test-signing-key-for-nearbuy-api-2026!";
    private const string TestIssuer = "https://login.microsoftonline.com/70a28522-969b-451f-bdb2-abfea3aaa5bf/v2.0";
    private const string ValidAudience = "api://6b99ada1-1ca0-4f7f-9a81-86cf13b17134";

    private static readonly SymmetricSecurityKey SigningKey =
        new(Encoding.UTF8.GetBytes(TestSigningSecret));

    public TestDatabase Database { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", Database.ConnectionString);

        builder.ConfigureTestServices(services =>
        {
            // default authentication scheme on our JwtBearer
            services.PostConfigure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = TestScheme;
                options.DefaultChallengeScheme = TestScheme;
                options.DefaultScheme = TestScheme;
            });

            services.AddAuthentication()
                .AddJwtBearer(TestScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = TestIssuer,
                        ValidateAudience = true,
                        ValidAudiences = new[]
                        {
                            "6b99ada1-1ca0-4f7f-9a81-86cf13b17134",
                            ValidAudience
                        },
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SigningKey,
                        ValidateLifetime = true,
                        RoleClaimType = ClaimTypes.Role
                    };
                });
        });
    }
    // default client
    protected override void ConfigureClient(HttpClient client)
    {
        var token = GenerateJwtToken(audience: ValidAudience, scope: "access_as_user", role: "User");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // 4а
    public HttpClient CreateAnonymousClient()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Remove("Authorization");
        return client;
    }
    //4b

    public HttpClient CreateClientWithForeignApiToken()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Remove("Authorization");
        var foreignToken = GenerateJwtToken(
            audience: "api://00000000-0000-0000-0000-other-api-id",
            scope: "access_as_user",
            role: "User");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", foreignToken);
        return client;
    }

    // 4в: усе чинне (підпис, audience, scope, роль), крім строку дії
    public HttpClient CreateClientWithExpiredToken()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Remove("Authorization");
        var expiredToken = GenerateJwtToken(
            audience: ValidAudience,
            scope: "access_as_user",
            role: "User",
            expires: DateTime.UtcNow.AddHours(-1)); // більше за ClockSkew (5 хв)
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", expiredToken);
        return client;
    }

    // 4г: scope є, ролі немає
    public HttpClient CreateClientWithoutRole()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Remove("Authorization");
        var tokenWithoutRole = GenerateJwtToken(
            audience: ValidAudience,
            scope: "access_as_user",
            role: null);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenWithoutRole);
        return client;
    }

    private static string GenerateJwtToken(string audience, string scope, string? role, DateTime? expires = null)
    {
        var credentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "test-user-id"),
            new Claim("scp", scope)
        };

        if (role is not null)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: TestIssuer,
            audience: audience,
            claims: claims,
            expires: expires ?? DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Task InitializeAsync() => Database.InitializeAsync();

    async Task IAsyncLifetime.DisposeAsync() => await Database.DisposeAsync();
}