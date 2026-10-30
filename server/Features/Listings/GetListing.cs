namespace server.Features.Listings;

public static class GetListing
{
    public const string Name = "GetListing";

    public static void Map(RouteGroupBuilder listings) =>
        listings.MapGet("/{id:int}", Handle).WithName(Name);

    public static Results<Ok<ListingResponse>, NotFound> Handle(int id, ListingStore store) =>
        store.Find(id) is { } listing
            ? TypedResults.Ok(ListingResponse.From(listing))
            : TypedResults.NotFound();
}
