namespace client.Models;

public class ListingItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Status { get; set; } = "Available"; // e.g. "Available", "Pending", "Sold"
    public string Location { get; set; } = string.Empty; // e.g. "North Park, San Diego"
    public string Condition { get; set; } = string.Empty; // e.g. "Good vintage condition"
    public string Description { get; set; } = string.Empty;
    public string PickupNote { get; set; } = string.Empty; // e.g. "Local pickup preferred..."
    
    // Multi-image gallery support (main image + 4 thumbnails)
    public List<string> ImageUrls { get; set; } = new();

    // Nested complex objects
    public Seller Seller { get; set; } = new();
    public ItemDetails Details { get; set; } = new();
}