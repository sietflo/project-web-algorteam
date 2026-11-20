// 4г: токен зі scope access_as_user, але без ролі — читати можна, змінювати не можна (403)
public class RoleTests(ApiFactory app) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Reading_without_role_returns_200_OK()
    {
        var client = app.CreateClientWithoutRole();

        var response = await client.GetAsync("/api/listings");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Changing_data_without_role_returns_403_Forbidden()
    {
        var client = app.CreateClientWithoutRole();
        var body = new
        {
            name = "Test Item",
            price = 100,
            description = "Desc",
            image = "/img.png",
            status = 0
        };

        var postResponse = await client.PostAsJsonAsync("/api/listings", body);
        var putResponse = await client.PutAsJsonAsync("/api/listings/1", body);
        var deleteResponse = await client.DeleteAsync("/api/listings/1");

        Assert.Equal(HttpStatusCode.Forbidden, postResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, putResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, deleteResponse.StatusCode);
    }
}
