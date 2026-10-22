
using System.Collections.Generic;
using System.Linq;

namespace client.Services;

public enum ProfileLoadState
{
    Loading,
    Error,
    Empty,
    Data
}

public enum ProfileListingStatus
{
    Active,
    Sold,
    Archived
}

public class ProfileListing
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
    public string Image { get; set; } = "";
    public string Posted { get; set; } = "";
    public int Views { get; set; }
    public int Inquiries { get; set; }

    public ProfileListingStatus Status { get; set; }
        = ProfileListingStatus.Active;
}

public class ProfileStateService
{
    public ProfileLoadState State { get; private set; }
        = ProfileLoadState.Data;

    public string ErrorMessage { get; private set; } = "";

    public List<ProfileListing> Listings { get; private set; }
        = new()
        {
            new ProfileListing
            {
                Id = 1,
                Name = "Heirloom Tomato Basket",
                Price = 12m,
                Description = "Fresh heirloom tomatoes from a local garden.",
                Image = "/images/tomato-basket.png",
                Posted = "Posted 2 days ago",
                Views = 48,
                Inquiries = 3
            },
            new ProfileListing
            {
                Id = 2,
                Name = "Walnut Floating Shelf",
                Price = 38m,
                Description = "Handmade walnut floating shelf in great condition.",
                Image = "/images/walnut-shelf.png",
                Posted = "Posted 5 days ago",
                Views = 31,
                Inquiries = 2
            },
            new ProfileListing
            {
                Id = 3,
                Name = "Linen Picnic Blanket",
                Price = 24m,
                Description = "Soft linen picnic blanket, perfect for outdoor use.",
                Image = "/images/linen-blanket.png",
                Posted = "Posted 1 week ago",
                Views = 76,
                Inquiries = 6
            }
        };

    public int ActiveCount =>
        Listings.Count(x => x.Status == ProfileListingStatus.Active);

    public int SoldAndArchivedCount =>
        Listings.Count(x => x.Status != ProfileListingStatus.Active);

    public void SetLoading()
    {
        State = ProfileLoadState.Loading;
        ErrorMessage = "";
    }

    public void SetError(string message)
    {
        State = ProfileLoadState.Error;
        ErrorMessage = message;
    }

    public void SetListings(IEnumerable<ProfileListing> listings)
    {
        Listings = listings.ToList();

        State = Listings.Count == 0
            ? ProfileLoadState.Empty
            : ProfileLoadState.Data;

        ErrorMessage = "";
    }

    public void MarkAsSold(int id)
    {
        var item = Listings.FirstOrDefault(x => x.Id == id);

        if (item is null || item.Status != ProfileListingStatus.Active)
            return;

        item.Status = ProfileListingStatus.Sold;
        UpdateState();
    }

    public void Archive(int id)
    {
        var item = Listings.FirstOrDefault(x => x.Id == id);

        if (item is null || item.Status != ProfileListingStatus.Active)
            return;

        item.Status = ProfileListingStatus.Archived;
        UpdateState();
    }

    public void Restore(int id)
    {
        var item = Listings.FirstOrDefault(x => x.Id == id);

        if (item is null || item.Status != ProfileListingStatus.Archived)
            return;

        item.Status = ProfileListingStatus.Active;
        UpdateState();
    }

    public void Delete(int id)
    {
        var item = Listings.FirstOrDefault(x => x.Id == id);

        if (item is null)
            return;

        Listings.Remove(item);
        UpdateState();
    }

    private void UpdateState()
    {
        State = Listings.Count == 0
            ? ProfileLoadState.Empty
            : ProfileLoadState.Data;
    }
}
