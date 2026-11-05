using Microsoft.EntityFrameworkCore;
using server.Infrastructure;
using DbListing = server.Entities.Listing;
using DbListingStatus = server.Entities.ListingStatus;

namespace server.Features.Listings;

public sealed record CreateListingRequest(
    [property: Required, StringLength(80)] string Name,
    [property: Range(0, 100000)] decimal Price,
    [property: StringLength(500)] string? Description,
    [property: StringLength(200)] string? Image,
    [property: EnumDataType(typeof(ListingStatus))] ListingStatus Status);

public static class CreateListing
{
    public static void Map(RouteGroupBuilder listings) =>
        listings.MapPost("/", Handle)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);

    public static async Task<Results<CreatedAtRoute<ListingResponse>, ProblemHttpResult>> Handle(
        CreateListingRequest request,
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        var exists = await db.Listings
            .AnyAsync(l => l.Title == request.Name, cancellationToken);

        if (exists)
        {
            return TypedResults.Problem(
                "A listing with this name already exists.",
                statusCode: 409);
        }

        var user = await db.Users
            .FirstOrDefaultAsync(u => u.Email == "maya@example.com", cancellationToken);

        var category = await db.Categories
            .FirstOrDefaultAsync(c => c.Name == "Home & Garden", cancellationToken);

        if (user is null || category is null)
        {
            return TypedResults.Problem(
                "Default user or category was not found.",
                statusCode: 409);
        }

        var listing = new DbListing
        {
            Title = request.Name,
            Price = request.Price,
            Description = request.Description ?? "",
            Status = request.Status switch
            {
                ListingStatus.Sold => DbListingStatus.Sold,
                ListingStatus.Archived => DbListingStatus.Archived,
                _ => DbListingStatus.Active
            },
            UserId = user.Id,
            CategoryId = category.Id,
            CreatedAtUtc = DateTime.UtcNow
        };

        if (!string.IsNullOrWhiteSpace(request.Image))
        {
            listing.Images.Add(new server.Entities.ListingImage
            {
                Url = request.Image,
                IsMain = true
            });
        }

        db.Listings.Add(listing);
        await db.SaveChangesAsync(cancellationToken);

        var response = new ListingResponse(
            listing.Id,
            listing.Title,
            listing.Price,
            listing.Description,
            request.Image ?? "",
            $"Posted {listing.CreatedAtUtc:yyyy-MM-dd}",
            listing.Views,
            listing.Inquiries,
            request.Status
        );

        return TypedResults.CreatedAtRoute(
            response,
            GetListing.Name,
            new { id = listing.Id });
    }
}
