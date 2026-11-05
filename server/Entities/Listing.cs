namespace server.Entities;

public enum ListingStatus { Active, Pending, Sold, Archived }

public class Listing
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public ListingStatus Status { get; set; } = ListingStatus.Active;
    public string Location { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string PickupNote { get; set; } = string.Empty;
    
    public int Views { get; set; }
    public int Inquiries { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Properties from ItemDetails
    public string Material { get; set; } = string.Empty;
    public string Dimensions { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty;

    // Foreign Keys & Navigation Properties
    public int UserId { get; set; }
    public User User { get; set;} = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set;} = null!;

    public ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();
}