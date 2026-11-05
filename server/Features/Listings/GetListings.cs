using Microsoft.EntityFrameworkCore;
using server.Infrastructure;

namespace server.Features.Listings;

public static class GetListings
{
    public static void Map(RouteGroupBuilder listings) =>
        listings.MapGet("/", Handle);

    public static async Task<Ok<ListingResponse[]>> Handle(
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        var listings = await db.Listings
            .AsNoTracking()
            .Include(l => l.Images)
            .OrderBy(l => l.Id)
            .ToListAsync(cancellationToken);

        var response = listings.Select(l => new ListingResponse(
            l.Id,
            l.Title,
            l.Price,
            l.Description,
            l.Images.FirstOrDefault(i => i.IsMain)?.Url
                ?? l.Images.FirstOrDefault()?.Url
                ?? "",
            $"Posted {l.CreatedAtUtc:yyyy-MM-dd}",
            l.Views,
            l.Inquiries,
            l.Status switch
            {
                server.Entities.ListingStatus.Sold => ListingStatus.Sold,
                server.Entities.ListingStatus.Archived => ListingStatus.Archived,
                _ => ListingStatus.Active
            }
        )).ToArray();

        return TypedResults.Ok(response);
    }
}
