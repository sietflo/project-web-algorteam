namespace server.Features.Listings;

public sealed class ListingStore
{
    private readonly Lock gate = new();
    private readonly Dictionary<int, Listing> items = [];
    private int lastId;

    // Стартові оголошення, такі самі як у клієнті (Profile)
    public ListingStore()
    {
        TrySave(new Listing(0, "Heirloom Tomato Basket", 12m, "Fresh heirloom tomatoes from a local garden.",
            "/images/tomato-basket.png", "Posted 2 days ago", 48, 3, ListingStatus.Active));
        TrySave(new Listing(0, "Walnut Floating Shelf", 38m, "Handmade walnut floating shelf in great condition.",
            "/images/walnut-shelf.png", "Posted 5 days ago", 31, 2, ListingStatus.Active));
        TrySave(new Listing(0, "Linen Picnic Blanket", 24m, "Soft linen picnic blanket, perfect for outdoor use.",
            "/images/linen-blanket.png", "Posted 1 week ago", 76, 6, ListingStatus.Active));
    }

    public IReadOnlyList<Listing> All() { lock (gate) { return [.. items.Values]; } }
    public Listing? Find(int id) { lock (gate) { return items.GetValueOrDefault(id); } }
    public bool Remove(int id) { lock (gate) { return items.Remove(id); } }

    public Listing? TrySave(Listing listing)
    {
        lock (gate)
        {
            if (items.Values.Any(l => l.Name == listing.Name && l.Id != listing.Id)) { return null; }
            var saved = listing.Id == 0 ? listing with { Id = ++lastId } : listing;
            items[saved.Id] = saved;
            return saved;
        }
    }
}
