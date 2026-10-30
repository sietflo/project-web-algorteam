using System.Globalization;
using AngleSharp.Dom;
using Bunit;
using Xunit;
using client.Models;
using client.Pages;
using client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace client.Tests.Pages;

public class ContactSellerPriceCultureTests : DetailTestBase
{
    [Theory]
    [InlineData("uk-UA", "3,5")]
    [InlineData("en-US", "3.5")]
    public void ContactForm_ShouldAcceptFractionalOffer_ForCulture(string culture, string typedOffer)
    {
        var previousCulture = CultureInfo.CurrentCulture;
        var previousUiCulture = CultureInfo.CurrentUICulture;
        CultureInfo.CurrentCulture = new CultureInfo(culture);
        CultureInfo.CurrentUICulture = new CultureInfo(culture);

        try
        {
            // Arrange
            var stateService = new FakeDetailStateService { IsLoading = false };
            stateService.Items.Clear();
            stateService.Items.Add(new ListingItem
            {
                Id = 42,
                Title = "Test Bicycle",
                Seller = new Seller { Name = "Test Seller" }
            });
            Services.AddSingleton<ListingStateService>(stateService);
            var cut = Render<ListingDetail>(parameters => parameters.Add(p => p.Id, 42));

            // Act
            InputOf(cut, "name-field").Change("Anna");
            InputOf(cut, "email-field").Change("anna@example.com");
            InputOf(cut, "message-field").Change("Is it still available?");
            InputOf(cut, "offer-price").Change(typedOffer);
            cut.Find("[data-testid='contact-form']").Submit();

            // Assert
            var success = cut.Find("[data-testid='contact-success']");
            Assert.Contains($"${typedOffer}", success.TextContent);
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
            CultureInfo.CurrentUICulture = previousUiCulture;
        }
    }

    // Поле MudBlazor може тримати data-testid на обгортці, тому шукаємо сам input/textarea
    private static IElement InputOf(IRenderedComponent<ListingDetail> cut, string testId)
    {
        var field = cut.Find($"[data-testid='{testId}']");
        return field.TagName is "INPUT" or "TEXTAREA" ? field : field.QuerySelector("input, textarea")!;
    }
}
