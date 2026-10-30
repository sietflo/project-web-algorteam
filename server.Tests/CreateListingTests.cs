public class CreateListingTests(ApiFactory app) : IClassFixture<ApiFactory>
{
    private readonly HttpClient client = app.CreateClient();

    [Fact]
    public async Task The_same_name_twice_gives_201_then_409()
    {
        var body = new { name = "Велосипед 201", price = 150, description = "Майже новий", image = "", status = 0 };
        var created = await client.PostAsJsonAsync("/api/listings", body);
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        Assert.NotNull(created.Headers.Location);

        var again = await client.PostAsJsonAsync("/api/listings", body);
        Assert.Equal(HttpStatusCode.Conflict, again.StatusCode);
        Assert.Equal("application/problem+json", again.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task An_empty_name_and_negative_price_give_400_naming_both_fields()
    {
        var body = new { name = "", price = -1, description = "", image = "", status = 0 };
        var response = await client.PostAsJsonAsync("/api/listings", body);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.Equal(["Name", "Price"], problem!.Errors.Keys.Order());
    }
}
