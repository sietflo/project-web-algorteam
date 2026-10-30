using Bunit;
using Xunit;
using client.Pages;
using client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace client.Tests.Pages;

public class ListingDetailErrorTests : DetailTestBase
{
    [Fact]
    public void ListingDetail_ShouldRenderErrorState_WhenErrorMessageIsSet()
    {
        // Arrange
        var stateService = new FakeDetailStateService
        {
            IsLoading = false,
            ErrorMessage = "Failed to load listing."
        };
        Services.AddSingleton<ListingStateService>(stateService);

        // Act
        var cut = Render<ListingDetail>(parameters => parameters.Add(p => p.Id, 1));

        // Assert
        Assert.NotNull(cut.Find("[data-testid='error-state']"));
        Assert.Contains("Failed to load listing.", cut.Markup);
    }
}
