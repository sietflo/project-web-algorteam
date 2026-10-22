
using System.Threading.Tasks;
using Bunit;
using Xunit;
using client.Pages;
using MudBlazor.Services;

namespace client.Tests.Pages;

public class ProfileTests
{
    [Fact]
    public async Task Profile_ShouldRenderEmptyState_WhenNoListingsExist()
    {
        var context = new BunitContext();

        try
        {
            context.JSInterop.Mode = JSRuntimeMode.Loose;
            context.Services.AddMudServices();

            // Act
            var cut = context.Render<Profile>();

            // Assert
            Assert.NotNull(
                cut.Find("[data-testid='empty-state']")
            );

            Assert.Contains(
                "You have no active listings.",
                cut.Markup
            );
        }
        finally
        {
            await context.DisposeAsync();
        }
    }
}
