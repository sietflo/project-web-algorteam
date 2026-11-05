namespace server.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; //unique index
    public string AvatarUrl { get; set; } = string.Empty;
    public double Rating { get; set; }
    public int ItemsSoldCount { get; set; }
    public int MemberSinceYear { get; set; }
    public bool IsVerified { get; set; }

    // 1-to-many relationship with Listing
    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
}