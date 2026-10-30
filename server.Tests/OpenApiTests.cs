using System.Text.Json.Nodes;

public class OpenApiTests(ApiFactory app) : IClassFixture<ApiFactory>
{
    private readonly HttpClient client = app.CreateClient();

    [Theory]
    [InlineData("/api/listings", "get", "200")]
    [InlineData("/api/listings", "post", "201 400 409")]
    [InlineData("/api/listings/{id}", "get", "200 404")]
    [InlineData("/api/listings/{id}", "put", "204 400 404 409")]
    [InlineData("/api/listings/{id}", "delete", "204 404")]
    public async Task The_document_names_every_code_of_an_endpoint(string path, string method, string codes)
    {
        var document = await client.GetFromJsonAsync<JsonObject>("/openapi/v1.json");
        var responses = document!["paths"]![path]![method]!["responses"]!.AsObject();
        Assert.Equal(codes, string.Join(' ', responses.Select(r => r.Key).Order()));
    }

    [Fact]
    public async Task The_document_is_not_published_outside_Development()
    {
        using var production = app.WithWebHostBuilder(b => b.UseEnvironment("Production"));
        var response = await production.CreateClient().GetAsync("/openapi/v1.json");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
