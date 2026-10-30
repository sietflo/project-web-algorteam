namespace server.Features.Listings;

public enum ListingStatus { Active, Sold, Archived }

public sealed record Listing(
    int Id, string Name, decimal Price, string Description, string Image,
    string Posted, int Views, int Inquiries, ListingStatus Status);

public sealed record ListingResponse(
    int Id, string Name, decimal Price, string Description, string Image,
    string Posted, int Views, int Inquiries, ListingStatus Status)
{
    public static ListingResponse From(Listing l) =>
        new(l.Id, l.Name, l.Price, l.Description, l.Image, l.Posted, l.Views, l.Inquiries, l.Status);
}
