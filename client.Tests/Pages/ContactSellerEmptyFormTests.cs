using Bunit;
using Xunit;
using client.Models;
using client.Pages;
using client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace client.Tests.Pages;

public class ContactSellerEmptyFormTests : DetailTestBase
{
    [Fact]
    public void ContactForm_ShouldShowMessageUnderEachRequiredField_WhenSubmittedEmpty()
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
        cut.Find("[data-testid='contact-form']").Submit();

        // Assert
        Assert.Contains("Name is required.", cut.Markup);
        Assert.Contains("Email is required.", cut.Markup);
        Assert.Contains("Message is required.", cut.Markup);
    }
}
