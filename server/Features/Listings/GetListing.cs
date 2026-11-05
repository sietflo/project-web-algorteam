using Microsoft.EntityFrameworkCore;
using server.Infrastructure;

namespace server.Features.Listings;

public static class GetListing
{
    public const string Name = "GetListing";

    public static void Map(RouteGroupBuilder listings) =>
        listings.MapGet("/{id:int}", Handle).WithName(Name);

    public static async Task<Results<Ok<ListingResponse>, NotFound>> Handle(
        int id,
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        var listing = await db.Listings
            .AsNoTracking()
            .Include(l => l.Images)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        if (listing is null)
            return TypedResults.NotFound();

        var response = new ListingResponse(
            listing.Id,
            listing.Title,
            listing.Price,
            listing.Description,
            listing.Images.FirstOrDefault(i => i.IsMain)?.Url
                ?? listing.Images.FirstOrDefault()?.Url
                ?? "",
            $"Posted {listing.CreatedAtUtc:yyyy-MM-dd}",
            listing.Views,
            listing.Inquiries,
            listing.Status switch
            {
                server.Entities.ListingStatus.Sold => ListingStatus.Sold,
                server.Entities.ListingStatus.Archived => ListingStatus.Archived,
                _ => ListingStatus.Active
            }
        );

        return TypedResults.Ok(response);
    }
}
