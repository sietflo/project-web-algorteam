using Microsoft.EntityFrameworkCore;
using server.Infrastructure;
using DbListingStatus = server.Entities.ListingStatus;

namespace server.Features.Listings;

public static class ReplaceListing
{
    public static void Map(RouteGroupBuilder listings) =>
        listings.MapPut("/{id:int}", Handle)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);

    public static async Task<Results<NoContent, NotFound, ProblemHttpResult>> Handle(
        int id,
        CreateListingRequest request,
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        var listing = await db.Listings
            .Include(l => l.Images)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        if (listing is null)
        {
            return TypedResults.NotFound();
        }

        var duplicateExists = await db.Listings
            .AnyAsync(l => l.Title == request.Name && l.Id != id,
                cancellationToken);

        if (duplicateExists)
        {
            return TypedResults.Problem(
                "A listing with this name already exists.",
                statusCode: 409);
        }

        listing.Title = request.Name;
        listing.Price = request.Price;
        listing.Description = request.Description ?? "";
        listing.Status = request.Status switch
        {
            ListingStatus.Sold => DbListingStatus.Sold,
            ListingStatus.Archived => DbListingStatus.Archived,
            _ => DbListingStatus.Active
        };

        var mainImage = listing.Images.FirstOrDefault(i => i.IsMain)
            ?? listing.Images.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(request.Image))
        {
            if (mainImage is not null)
            {
                db.Images.Remove(mainImage);
            }
        }
        else if (mainImage is not null)
        {
            mainImage.Url = request.Image;
            mainImage.IsMain = true;
        }
        else
        {
            listing.Images.Add(new server.Entities.ListingImage
            {
                Url = request.Image,
                IsMain = true
            });
        }

        await db.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
}
