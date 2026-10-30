namespace server.Features.Listings;

public sealed record CreateListingRequest(
    [property: Required, StringLength(80)] string Name,
    [property: Range(0, 100000)] decimal Price,
    [property: StringLength(500)] string? Description,
    [property: StringLength(200)] string? Image,
    [property: EnumDataType(typeof(ListingStatus))] ListingStatus Status);

public static class CreateListing
{
    public static void Map(RouteGroupBuilder listings) => listings.MapPost("/", Handle)
        .ProducesValidationProblem().ProducesProblem(StatusCodes.Status409Conflict);

    public static Results<CreatedAtRoute<ListingResponse>, ProblemHttpResult> Handle(
        CreateListingRequest request, ListingStore store)
    {
        var listing = new Listing(0, request.Name, request.Price, request.Description ?? "",
            request.Image ?? "", "Posted today", 0, 0, request.Status);
        if (store.TrySave(listing) is not { } saved)
        {
            return TypedResults.Problem($"Оголошення «{request.Name}» уже існує.", statusCode: 409);
        }
        return TypedResults.CreatedAtRoute(ListingResponse.From(saved), GetListing.Name, new { id = saved.Id });
    }
}
