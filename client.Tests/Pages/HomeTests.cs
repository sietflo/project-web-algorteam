using Bunit;
using Xunit;
using client.Pages;
using client.Services;
using client.Models;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace client.Tests.Pages;

public class HomeTests : BunitContext, IAsyncLifetime
{
    public HomeTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddMudServices();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    // Решаем ошибку DisposeAsync для MudBlazor сервисов
    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }

    [Fact]
    public void Home_ShouldRenderLoadingState_WhenIsLoadingIsTrue()
    {
        // Arrange
        var stateService = new FakeListingStateService { IsLoading = true };
        Services.AddSingleton<ListingStateService>(stateService);

        // Act
        var cut = Render<Home>();

        // Assert
        Assert.NotNull(cut.Find("[data-testid='loading-state']"));
        Assert.Contains("Loading fresh finds...", cut.Markup);
    }

    [Fact]
    public void Home_ShouldRenderErrorState_WhenErrorMessageIsSet()
    {
        // Arrange
        var stateService = new FakeListingStateService 
        { 
            IsLoading = false, 
            ErrorMessage = "Failed to load listings." 
        };
        Services.AddSingleton<ListingStateService>(stateService);

        // Act
        var cut = Render<Home>();

        // Assert
        Assert.NotNull(cut.Find("[data-testid='error-state']"));
        Assert.Contains("Failed to load listings.", cut.Markup);
    }

    [Fact]
    public void Home_ShouldRenderEmptyState_WhenNoItemsExist()
    {
        // Arrange
        var stateService = new FakeListingStateService { IsLoading = false };
        stateService.Items.Clear();
        Services.AddSingleton<ListingStateService>(stateService);

        // Act
        var cut = Render<Home>();

        // Assert
        Assert.NotNull(cut.Find("[data-testid='empty-state']"));
        Assert.Contains("No items found matching your search.", cut.Markup);
    }

    [Fact]
    public void Home_ShouldRenderDataState_WhenItemsArePresent()
    {
        // Arrange
        var stateService = new FakeListingStateService { IsLoading = false };
        stateService.Items.Add(new ListingItem 
        { 
            Id = 1, 
            Title = "Test Bicycle", 
            Price = 100, 
            Category = "Sports", 
            Location = "Campus",
            ImageUrls = new List<string> { "https://example.com/image1.jpg" }
        });
        Services.AddSingleton<ListingStateService>(stateService);

        // Act
        var cut = Render<Home>();

        // Assert
        Assert.NotNull(cut.Find("[data-testid='data-state']"));
        Assert.Contains("Test Bicycle", cut.Markup);
    }
}

// Фейковый сервіс, щоб OnInitializedAsync не затирав наш Arrange-стан
public class FakeListingStateService : ListingStateService
{
    public override Task LoadItemsAsync()
    {
        // Нічого не робимо в тестах, зберігаємо стан з Arrange
        return Task.CompletedTask;
    }
}