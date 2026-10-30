using Bunit;
using Xunit;
using client.Pages;
using client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace client.Tests.Pages;

public class ListingDetailLoadingTests : DetailTestBase
{
    [Fact]
    public void ListingDetail_ShouldRenderLoadingState_WhenIsLoadingIsTrue()
    {
        // Arrange
        var stateService = new FakeDetailStateService { IsLoading = true };
        Services.AddSingleton<ListingStateService>(stateService);

        // Act
        var cut = Render<ListingDetail>(parameters => parameters.Add(p => p.Id, 1));

        // Assert
        Assert.NotNull(cut.Find("[data-testid='loading-state']"));
        Assert.Contains("Loading listing...", cut.Markup);
    }
}
