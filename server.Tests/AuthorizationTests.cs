using System.Net;
using System.Net.Http.Json;

public class AuthorizationTests(ApiFactory app) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Request_without_token_returns_401_Unauthorized()
    {
        var client = app.CreateAnonymousClient();

        var getResponse = await client.GetAsync("/api/listings");
        var postResponse = await client.PostAsJsonAsync("/api/listings", new
        {
            name = "Test Item",
            price = 100,
            description = "Desc",
            image = "/img.png",
            status = 0
        });

        Assert.Equal(HttpStatusCode.Unauthorized, getResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, postResponse.StatusCode);
    }

    [Fact]
    public async Task Request_with_token_for_another_API_returns_401_Unauthorized()
    {
        var client = app.CreateClientWithForeignApiToken();

        var getResponse = await client.GetAsync("/api/listings");
        var postResponse = await client.PostAsJsonAsync("/api/listings", new
        {
            name = "Test Item",
            price = 100,
            description = "Desc",
            image = "/img.png",
            status = 0
        });

        Assert.Equal(HttpStatusCode.Unauthorized, getResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, postResponse.StatusCode);
    }
}