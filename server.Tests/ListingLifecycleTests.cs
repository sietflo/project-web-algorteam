public class ListingLifecycleTests(ApiFactory app) : IClassFixture<ApiFactory>
{
    private readonly HttpClient client = app.CreateClient();

    private async Task<Uri> CreateAsync(string name)
    {
        var body = new { name, price = 100, description = "Опис", image = "", status = 0 };
        return (await client.PostAsJsonAsync("/api/listings", body)).Headers.Location!;
    }

    [Fact]
    public async Task Read_replace_delete_give_200_204_204_then_404()
    {
        var location = await CreateAsync("Лампа 301");
        var body = new { name = "Лампа 301", price = 120, description = "Нова ціна", image = "", status = 1 };

        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/api/listings")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync(location)).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await client.PutAsJsonAsync(location, body)).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await client.DeleteAsync(location)).StatusCode);

        var gone = await client.GetAsync(location);
        var problem = await gone.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal(404, problem!.Status);
    }

    [Fact]
    public async Task The_same_put_twice_gives_204_and_the_same_state()
    {
        var location = await CreateAsync("Стіл 302");
        var body = new { name = "Стіл 302", price = 80, description = "Дубовий", image = "", status = 0 };

        Assert.Equal(HttpStatusCode.NoContent, (await client.PutAsJsonAsync(location, body)).StatusCode);
        var first = await client.GetFromJsonAsync<ListingResponse>(location);
        Assert.Equal(HttpStatusCode.NoContent, (await client.PutAsJsonAsync(location, body)).StatusCode);
        var second = await client.GetFromJsonAsync<ListingResponse>(location);

        Assert.Equal(first, second);
    }

    [Fact]
    public async Task The_same_delete_twice_gives_204_then_404()
    {
        var location = await CreateAsync("Крісло 303");

        Assert.Equal(HttpStatusCode.NoContent, (await client.DeleteAsync(location)).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.DeleteAsync(location)).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync(location)).StatusCode);
    }

    [Fact]
    public async Task A_missing_listing_gives_404_for_get_put_and_delete()
    {
        var body = new { name = "Немає 304", price = 10, description = "", image = "", status = 0 };

        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/api/listings/9999")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.PutAsJsonAsync("/api/listings/9999", body)).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.DeleteAsync("/api/listings/9999")).StatusCode);
    }

    [Fact]
    public async Task Put_with_a_taken_name_gives_409()
    {
        await CreateAsync("Полиця 305");
        var location = await CreateAsync("Полиця 306");
        var body = new { name = "Полиця 305", price = 50, description = "", image = "", status = 0 };

        var response = await client.PutAsJsonAsync(location, body);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Put_with_an_empty_name_gives_400()
    {
        var location = await CreateAsync("Шафа 307");
        var body = new { name = "", price = 50, description = "", image = "", status = 0 };

        var response = await client.PutAsJsonAsync(location, body);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.Equal(["Name"], problem!.Errors.Keys.Order());
    }
}
