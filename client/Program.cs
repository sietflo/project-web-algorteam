
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Authentication.WebAssembly.Msal.Models;
using MudBlazor.Services;
using client;
using client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HTTP client for the Blazor application
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// Microsoft Entra ID authentication
builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);

    options.ProviderOptions.DefaultAccessTokenScopes.Add(
        "api://6b99ada1-1ca0-4f7f-9a81-86cf13b17134/access_as_user");

    options.UserOptions.RoleClaim = "roles";
});

// Profile API configuration
var profileApiEnabled =
    builder.Configuration.GetValue<bool>("ProfileApi:Enabled");

var profileApiBaseUrl =
    builder.Configuration["ProfileApi:BaseUrl"];

builder.Services.AddScoped<ProfileApiService>(sp =>
{
    if (profileApiEnabled && !string.IsNullOrWhiteSpace(profileApiBaseUrl))
    {
        var handler = sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(
                authorizedUrls: new[] { profileApiBaseUrl },
                scopes: new[] { "api://6b99ada1-1ca0-4f7f-9a81-86cf13b17134/access_as_user" }
            );

        // Without IHttpClientFactory the inner handler must be set manually
        handler.InnerHandler = new HttpClientHandler();

        return new ProfileApiService(new HttpClient(handler)
        {
            BaseAddress = new Uri(profileApiBaseUrl)
        });
    }

    return new ProfileApiService(new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });
});

builder.Services.AddMudServices();

builder.Services.AddScoped<ListingStateService>();
builder.Services.AddScoped<ProfileStateService>();

await builder.Build().RunAsync();
