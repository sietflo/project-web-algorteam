namespace server.Features.Listings;

public static class ListingEndpoints
{
    public static RouteGroupBuilder MapListings(this WebApplication app)
    {
        var listings = app.MapGroup("/api/listings");

        // 3а: Читання вимагає scope access_as_user
        var readGroup = listings.MapGroup("")
            .RequireAuthorization("ReadListings");
        GetListings.Map(readGroup);
        GetListing.Map(readGroup);

        // 3б: Операції зміни вимагають scope + роль (User або Admin)
        var writeGroup = listings.MapGroup("")
            .RequireAuthorization("WriteListings");
        CreateListing.Map(writeGroup);
        ReplaceListing.Map(writeGroup);
        DeleteListing.Map(writeGroup);

        return listings;
    }
}