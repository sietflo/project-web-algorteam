using Bunit;
using Xunit;
using client.Models;
using client.Pages;
using client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace client.Tests.Pages;

public class ListingDetailDataTests : DetailTestBase
{
    [Fact]
    public void ListingDetail_ShouldRenderDataState_WhenListingExists()
    {
        // Arrange
        var stateService = new FakeDetailStateService { IsLoading = false };
        stateService.Items.Clear();
        stateService.Items.Add(new ListingItem
        {
            Id = 42,
            Title = "Test Bicycle",
            Price = 100,
            Category = "Sports",
            Location = "Campus",
            Description = "Well kept city bike.",
            ImageUrls = new List<string> { "https://example.com/image1.jpg" },
            Seller = new Seller { Name = "Test Seller", Rating = 4.8, ItemsSoldCount = 3, MemberSinceYear = 2022 }
        });
        Services.AddSingleton<ListingStateService>(stateService);

        // Act
        var cut = Render<ListingDetail>(parameters => parameters.Add(p => p.Id, 42));

        // Assert
        Assert.NotNull(cut.Find("[data-testid='data-state']"));
        Assert.Contains("Test Bicycle", cut.Markup);
        Assert.Contains("Well kept city bike.", cut.Markup);
        Assert.Contains("Test Seller", cut.Markup);
    }
}
