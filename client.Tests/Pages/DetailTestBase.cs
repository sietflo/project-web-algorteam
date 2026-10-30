using Bunit;
using Xunit;
using client.Services;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace client.Tests.Pages;

// Спільне налаштування для тестів сторінки деталей оголошення
public abstract class DetailTestBase : BunitContext, IAsyncLifetime
{
    protected DetailTestBase()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddMudServices();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    // Вирішуємо помилку DisposeAsync для MudBlazor сервісів
    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }
}

// Фейковий сервіс, щоб OnInitializedAsync не затирав наш Arrange-стан
public class FakeDetailStateService : ListingStateService
{
    public override Task LoadItemsAsync()
    {
        // Нічого не робимо в тестах, зберігаємо стан з Arrange
        return Task.CompletedTask;
    }
}
