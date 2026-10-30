namespace server.Features.Listings;

public static class ListingEndpoints
{
    public static RouteGroupBuilder MapListings(this WebApplication app)
    {
        var listings = app.MapGroup("/api/listings");
        GetListings.Map(listings);
        GetListing.Map(listings);
        CreateListing.Map(listings);
        ReplaceListing.Map(listings);
        DeleteListing.Map(listings);
        return listings;
    }
}
