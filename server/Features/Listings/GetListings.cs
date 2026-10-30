namespace server.Features.Listings;

public static class GetListings
{
    public static void Map(RouteGroupBuilder listings) => listings.MapGet("/", Handle);

    public static Ok<ListingResponse[]> Handle(ListingStore store) =>
        TypedResults.Ok(store.All().Select(ListingResponse.From).ToArray());
}
