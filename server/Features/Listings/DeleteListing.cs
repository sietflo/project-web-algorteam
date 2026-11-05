using Microsoft.EntityFrameworkCore;
using server.Infrastructure;

namespace server.Features.Listings;

public static class DeleteListing
{
    public static void Map(RouteGroupBuilder listings) =>
        listings.MapDelete("/{id:int}", Handle);

    public static async Task<Results<NoContent, NotFound>> Handle(
        int id,
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        var listing = await db.Listings
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        if (listing is null)
        {
            return TypedResults.NotFound();
        }

        db.Listings.Remove(listing);
        await db.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
}
