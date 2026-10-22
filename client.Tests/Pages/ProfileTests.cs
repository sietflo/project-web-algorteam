
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bunit;
using Xunit;
using client.Pages;
using client.Services;
using Microsoft.Extensions.DependencyInjection;
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

            var profileState = new ProfileStateService();
            profileState.SetListings(new List<ProfileListing>());

            context.Services.AddSingleton(profileState);

            var cut = context.Render<Profile>();

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

    [Fact]
    public async Task Profile_ShouldRenderLoadingState_WhenDataIsLoading()
    {
        var context = new BunitContext();

        try
        {
            context.JSInterop.Mode = JSRuntimeMode.Loose;
            context.Services.AddMudServices();

            var profileState = new ProfileStateService();
            profileState.SetLoading();

            context.Services.AddSingleton(profileState);

            var cut = context.Render<Profile>();

            Assert.NotNull(
                cut.Find("[data-testid='loading-state']")
            );

            Assert.Contains(
                "Loading profile...",
                cut.Markup
            );
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Profile_ShouldRenderErrorState_WhenLoadingFails()
    {
        var context = new BunitContext();

        try
        {
            context.JSInterop.Mode = JSRuntimeMode.Loose;
            context.Services.AddMudServices();

            var profileState = new ProfileStateService();

            // Імітуємо помилку завантаження
            profileState.SetError("Failed to load profile.");

            context.Services.AddSingleton(profileState);

            var cut = context.Render<Profile>();

            Assert.NotNull(
                cut.Find("[data-testid='error-state']")
            );

            Assert.Contains(
                "Failed to load profile.",
                cut.Markup
            );
        }
        finally
        {
            await context.DisposeAsync();
        }
    }
}
