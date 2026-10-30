namespace server.Features.Listings;

public static class ReplaceListing
{
    public static void Map(RouteGroupBuilder listings) => listings.MapPut("/{id:int}", Handle)
        .ProducesValidationProblem().ProducesProblem(StatusCodes.Status409Conflict);

    public static Results<NoContent, NotFound, ProblemHttpResult> Handle(
        int id, CreateListingRequest request, ListingStore store)
    {
        if (store.Find(id) is not { } old)
        {
            return TypedResults.NotFound();
        }
        // Дата, перегляди й запити лишаються як були
        var listing = new Listing(id, request.Name, request.Price, request.Description ?? "",
            request.Image ?? "", old.Posted, old.Views, old.Inquiries, request.Status);
        return store.TrySave(listing) is null
            ? TypedResults.Problem($"Оголошення «{request.Name}» уже існує.", statusCode: 409)
            : TypedResults.NoContent();
    }
}
