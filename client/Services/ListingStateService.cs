namespace client.Services;

using client.Models;

public class ListingStateService
{
    public List<ListingItem> Items { get; private set; } = new()
    {
        new ListingItem
        {
            Id = 1,
            Title = "Vintage Oak Chair",
            Price = 85,
            Status = "Available",
            Location = "North Park, San Diego",
            Condition = "Good vintage condition",
            Description = "A beautifully aged solid oak chair with a gently curved spindle back and a warm honey finish. The sturdy joinery and comfortable shaped seat make it ideal as a dining chair, a reading-corner accent, or a characterful desk chair. It shows light surface marks consistent with age, including a few small scratches on the legs, but remains solid and stable with no wobble.",
            PickupNote = "Local pickup preferred. Happy to help load it into your vehicle.",
            ImageUrls = new List<string>
            {
                "https://images.unsplash.com/photo-1580481077114-1e031b26f8eb?w=900&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1503602642458-232111445657?w=300&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=300&auto=format&fit=crop&q=80",
                "https://images.unsplash.com/photo-1592078615290-033ee584e267?w=300&auto=format&fit=crop&q=80"
            },
            Seller = new Seller
            {
                Id = 101,
                Name = "Maya R.",
                AvatarUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=120&auto=format&fit=crop&q=80",
                Rating = 4.9,
                ItemsSoldCount = 18,
                MemberSinceYear = 2022,
                IsVerified = true
            },
            Details = new ItemDetails
            {
                Material = "Solid oak",
                Dimensions = "34\" H × 18\" W × 19\" D",
                Style = "Mid-century rustic",
                PostedTimeAgo = "2 days ago"
            }
        },
        new ListingItem
        {
            Id = 2,
            Title = "Noise-Canceling Headphones",
            Price = 68,
            Status = "Available",
            Location = "North Campus",
            Condition = "Like new",
            Description = "Sony WH-1000XM4. Clean condition, includes charging case and cable.",
            ImageUrls = new List<string>
            {
                "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=900&auto=format&fit=crop&q=80"
            },
            Seller = new Seller
            {
                Id = 102,
                Name = "Alex D.",
                AvatarUrl = "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=120&auto=format&fit=crop&q=80",
                Rating = 5.0,
                ItemsSoldCount = 4,
                MemberSinceYear = 2023,
                IsVerified = true
            },
            Details = new ItemDetails
            {
                Material = "Plastic / Foam",
                Dimensions = "Over-ear",
                Style = "Modern",
                PostedTimeAgo = "Today"
            }
        }
        
    };
    public bool IsAuthenticated { get; private set; } = false;
    public string CurrentUserName { get; private set; } = "Guest";

    // Событие для оповещения компонентов о смене стейта
    public event Action? OnChange;

    public void Login(string userName = "Maya R.")
    {
        IsAuthenticated = true;
        CurrentUserName = userName;
        NotifyStateChanged();
    }

    public void Logout()
    {
        IsAuthenticated = false;
        CurrentUserName = "Guest";
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
    public string SearchQuery { get; set; } = string.Empty;

    public ListingItem? GetById(int id) => Items.FirstOrDefault(i => i.Id == id);

    public void AddItem(ListingItem item)
    {
        item.Id = Items.Count > 0 ? Items.Max(x => x.Id) + 1 : 1;
        Items.Add(item);
    }
}