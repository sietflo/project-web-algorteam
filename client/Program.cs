
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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

// Profile API configuration
var profileApiEnabled =
    builder.Configuration.GetValue<bool>("ProfileApi:Enabled");

var profileApiBaseUrl =
    builder.Configuration["ProfileApi:BaseUrl"];

builder.Services.AddScoped<ProfileApiService>(sp =>
{
    var httpClient = new HttpClient
    {
        BaseAddress = profileApiEnabled &&
                      !string.IsNullOrWhiteSpace(profileApiBaseUrl)
            ? new Uri(profileApiBaseUrl)
            : new Uri(builder.HostEnvironment.BaseAddress)
    };

    return new ProfileApiService(httpClient);
});

builder.Services.AddMudServices();

builder.Services.AddScoped<ListingStateService>();
builder.Services.AddScoped<ProfileStateService>();

await builder.Build().RunAsync();
