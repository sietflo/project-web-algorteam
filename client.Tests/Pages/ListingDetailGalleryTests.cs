using Bunit;
using Xunit;
using client.Models;
using client.Pages;
using client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace client.Tests.Pages;

public class ListingDetailGalleryTests : DetailTestBase
{
    [Fact]
    public void ListingDetail_ShouldChangeMainImage_WhenThumbnailIsClicked()
    {
        // Arrange
        var stateService = new FakeDetailStateService { IsLoading = false };
        stateService.Items.Clear();
        stateService.Items.Add(new ListingItem
        {
            Id = 7,
            Title = "Test Bicycle",
            ImageUrls = new List<string>
            {
                "https://example.com/first.jpg",
                "https://example.com/second.jpg"
            }
        });
        Services.AddSingleton<ListingStateService>(stateService);

        var cut = Render<ListingDetail>(parameters => parameters.Add(p => p.Id, 7));
        Assert.Equal("https://example.com/first.jpg",
            cut.Find("[data-testid='gallery-main-image']").GetAttribute("src"));

        // Act
        cut.FindAll("[data-testid='gallery-thumb']")[1].Click();

        // Assert
        Assert.Equal("https://example.com/second.jpg",
            cut.Find("[data-testid='gallery-main-image']").GetAttribute("src"));
    }
}
