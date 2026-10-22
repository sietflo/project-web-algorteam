namespace client.Services;

using client.Models;

public class ListingStateService
{
    
    public List<ListingItem> Items { get; private set; } = new()
    {
        new ListingItem
        {
            Id = 1,
            Category = "Furniture",
            Title = "Vintage Oak Chair",
            Price = 85,
            Status = "Available",
            Location = "North Park, San Diego",
            Condition = "Good vintage condition",
            Description = "A beautifully aged solid oak chair with a gently curved spindle back and a warm honey finish. The sturdy joinery and comfortable shaped seat make it ideal as a dining chair, a reading-corner accent, or a characterful desk chair. It shows light surface marks consistent with age, including a few small scratches on the legs, but remains solid and stable with no wobble.",
            PickupNote = "Local pickup preferred. Happy to help load it into your vehicle.",
            ImageUrls = new List<string>
            {
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
            Category = "Electronics",
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
        },
        new ListingItem
        {
            Id = 3,
            Category = "Books",
            Title = "Intro Economics Textbooks",
            Price = 24,
            Status = "Available",
            Location = "College Hill",
            Condition = "Good",
            Description = "Set of micro and macroeconomics textbooks. Minimal highlighting, all pages intact. Great for first-year courses.",
            PickupNote = "Can meet near the main campus library.",
            ImageUrls = new List<string>
            {
                "https://core-media.electricbook.works/the-economy/assets/images/web/cover.jpg"
            },
            Seller = new Seller
            {
                Id = 103,
                Name = "David K.",
                AvatarUrl = "https://images.unsplash.com/photo-1570295999919-56ceb5ecca61?w=120&auto=format&fit=crop&q=80",
                Rating = 4.8,
                ItemsSoldCount = 9,
                MemberSinceYear = 2021,
                IsVerified = true
            },
            Details = new ItemDetails
            {
                Material = "Paperback",
                Dimensions = "Hardcover & Paperback",
                Style = "Academic",
                PostedTimeAgo = "3 hours ago"
            }
        },
        new ListingItem
        {
            Id = 4,
            Category = "Home",
            Title = "Adjustable Desk Lamp",
            Price = 18,
            Status = "Available",
            Location = "Oak Street",
            Condition = "Like new",
            Description = "Matte warm-orange architectural desk lamp. Multi-angle adjustable arm with warm LED bulb included. Perfect for study setups.",
            PickupNote = "Porch pickup available anytime.",
            ImageUrls = new List<string>
            {
                "https://images.unsplash.com/photo-1507473885765-e6ed057f782c?w=900&auto=format&fit=crop&q=80"
            },
            Seller = new Seller
            {
                Id = 104,
                Name = "Sarah M.",
                AvatarUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=120&auto=format&fit=crop&q=80",
                Rating = 4.9,
                ItemsSoldCount = 14,
                MemberSinceYear = 2022,
                IsVerified = false
            },
            Details = new ItemDetails
            {
                Material = "Metal / Steel",
                Dimensions = "18\" H",
                Style = "Industrial",
                PostedTimeAgo = "Yesterday"
            }
        },
        new ListingItem
        {
            Id = 5,
            Category = "Clothing",
            Title = "Everyday Campus Backpack",
            Price = 32,
            Status = "Available",
            Location = "Student Union",
            Condition = "Excellent",
            Description = "Durable olive green canvas backpack with padded 15-inch laptop sleeve and water bottle pocket. Clean interior.",
            PickupNote = "Can drop off near Student Union building.",
            ImageUrls = new List<string>
            {
                "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=900&auto=format&fit=crop&q=80"
            },
            Seller = new Seller
            {
                Id = 105,
                Name = "Chris P.",
                AvatarUrl = "https://images.unsplash.com/photo-1522075469751-3a6694fb2f61?w=120&auto=format&fit=crop&q=80",
                Rating = 4.7,
                ItemsSoldCount = 6,
                MemberSinceYear = 2023,
                IsVerified = true
            },
            Details = new ItemDetails
            {
                Material = "Canvas / Leather",
                Dimensions = "20L Capacity",
                Style = "Casual",
                PostedTimeAgo = "4 days ago"
            }
        },
        new ListingItem
        {
            Id = 6,
            Category = "Home",
            Title = "Compact Coffee Maker",
            Price = 28,
            Status = "Available",
            Location = "Maple Ave",
            Condition = "Good",
            Description = "Programmable drip coffee maker with glass carafe. Descaled and fully disinfected. Makes up to 5 cups.",
            PickupNote = "Buyer tests on pickup.",
            ImageUrls = new List<string>
            {
                "https://www.delonghi.ua/storage/_opt/shop/product/image/2067/normal/kavomashina-delonghi-ecam-470-85-mb_yh.jpg/768.webp"
            },
            Seller = new Seller
            {
                Id = 106,
                Name = "Emma L.",
                AvatarUrl = "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=120&auto=format&fit=crop&q=80",
                Rating = 5.0,
                ItemsSoldCount = 21,
                MemberSinceYear = 2021,
                IsVerified = true
            },
            Details = new ItemDetails
            {
                Material = "Stainless Steel / Glass",
                Dimensions = "10\" × 7\" × 12\"",
                Style = "Modern",
                PostedTimeAgo = "5 days ago"
            }
        },
        new ListingItem
        {
            Id = 7,
            Category = "Furniture",
            Title = "City Commuter Bicycle",
            Price = 140,
            Status = "Available",
            Location = "Riverside",
            Condition = "Good working condition",
            Description = "Classic 7-speed step-through city bike. Includes front basket, bell, and rear rack. Newly tuned brakes and smooth shifting.",
            PickupNote = "Test ride available in driveway.",
            ImageUrls = new List<string>
            {
                "https://images.unsplash.com/photo-1485965120184-e220f721d03e?w=900&auto=format&fit=crop&q=80"
            },
            Seller = new Seller
            {
                Id = 107,
                Name = "Jordan T.",
                AvatarUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=120&auto=format&fit=crop&q=80",
                Rating = 4.9,
                ItemsSoldCount = 11,
                MemberSinceYear = 2020,
                IsVerified = true
            },
            Details = new ItemDetails
            {
                Material = "Aluminum Frame",
                Dimensions = "Medium (54cm)",
                Style = "Vintage / Commuter",
                PostedTimeAgo = "1 week ago"
            }
        }
        
    };
    public bool IsLoading { get; set; } = false;
    public string? ErrorMessage { get; set; } = null;

    public string SearchQuery { get; set; } = string.Empty;
    public bool IsAuthenticated { get; private set; } = false;
    public string CurrentUserName { get; private set; } = string.Empty;

    public event Action? OnChange;
    public virtual async Task LoadItemsAsync()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            // Імітація затримки мережі
            await Task.Delay(500); 

            // Заповнення тестовими даними (якщо список порожній)
            if (Items.Count == 0)
            {
                Items = GetMockItems();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to load listings. Please try again later.";
        }
        finally
        {
            IsLoading = false;
        }
    }
    public ListingItem? GetById(int id) => Items.FirstOrDefault(i => i.Id == id);
    private List<ListingItem> GetMockItems()
    {
        return new List<ListingItem>
        {
            new ListingItem { Id = 1, Title = "Vintage Oak Chair", Price = 85, Category = "Furniture", Location = "North Park" },
            new ListingItem { Id = 2, Title = "Noise-Canceling Headphones", Price = 68, Category = "Electronics", Location = "North Campus" }
        };
    }
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


    public void AddItem(ListingItem item)
    {
        item.Id = Items.Count > 0 ? Items.Max(x => x.Id) + 1 : 1;
        Items.Add(item);
    }
}
