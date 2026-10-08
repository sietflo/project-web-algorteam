namespace client.Models;

public class Seller
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public double Rating { get; set; }
    public int ItemsSoldCount { get; set; }
    public int MemberSinceYear { get; set; }
    public bool IsVerified { get; set; }
}