using Bunit;
using Xunit;
using client.Pages;
using client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace client.Tests.Pages;

public class ListingDetailNotFoundTests : DetailTestBase
{
    [Fact]
    public void ListingDetail_ShouldRenderEmptyState_WhenListingDoesNotExist()
    {
        // Arrange
        var stateService = new FakeDetailStateService { IsLoading = false };
        Services.AddSingleton<ListingStateService>(stateService);

        // Act
        var cut = Render<ListingDetail>(parameters => parameters.Add(p => p.Id, 999));

        // Assert
        Assert.NotNull(cut.Find("[data-testid='empty-state']"));
        Assert.Contains("Listing not found.", cut.Markup);
    }
}
