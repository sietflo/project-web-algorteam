namespace server.Entities;

public class ListingImage
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsMain { get; set; } // main photo for listing flag

    public int ListingId { get; set; }
    public Listing Listing { get; set; } = null!;
}