namespace server.Features.Listings;

public static class DeleteListing
{
    public static void Map(RouteGroupBuilder listings) => listings.MapDelete("/{id:int}", Handle);

    public static Results<NoContent, NotFound> Handle(int id, ListingStore store) =>
        store.Remove(id) ? TypedResults.NoContent() : TypedResults.NotFound();
}
