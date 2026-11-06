// Happy path над справжньою базою: PostgreSQL у контейнері Testcontainers, сервер — ApiFactory.
public class RoundTripTests(ApiFactory app) : IClassFixture<ApiFactory>
{
    private readonly HttpClient client = app.CreateClient();

    [Fact]
    public async Task A_listing_created_through_the_API_is_read_back_from_PostgreSQL()
    {
        var body = new { name = "Книжкова шафа 401", price = 250, description = "Масив дуба", image = "/images/shelf.png", status = 0 };
        var created = await client.PostAsJsonAsync("/api/listings", body);
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);

        var read = await client.GetAsync(created.Headers.Location);
        Assert.Equal(HttpStatusCode.OK, read.StatusCode);

        var listing = await read.Content.ReadFromJsonAsync<ListingResponse>();
        Assert.Equal("Книжкова шафа 401", listing!.Name);
        Assert.Equal(250m, listing.Price);
        Assert.Equal("/images/shelf.png", listing.Image);
    }
}
