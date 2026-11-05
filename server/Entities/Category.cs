namespace server.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; //unique index

    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
}