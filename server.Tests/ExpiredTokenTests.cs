// 4в: прострочений токен — 401, хоча підпис, audience, scope і роль правильні
public class ExpiredTokenTests(ApiFactory app) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Request_with_expired_token_returns_401_Unauthorized()
    {
        var client = app.CreateClientWithExpiredToken();

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

        // 401 саме через строк дії, а не через щось інше
        var challenge = string.Join(" ", getResponse.Headers.GetValues("WWW-Authenticate"));
        Assert.Contains("expired", challenge);
    }
}
