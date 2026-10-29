
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
    private static BunitContext CreateContext(ProfileStateService profileState)
    {
        var context = new BunitContext();

        context.JSInterop.Mode = JSRuntimeMode.Loose;
        context.Services.AddMudServices();
        context.Services.AddSingleton(profileState);

        return context;
    }

    [Fact]
    public async Task Profile_ShouldRenderEmptyState_WhenNoListingsExist()
    {
        var profileState = new ProfileStateService();
        profileState.SetListings(new List<ProfileListing>());

        var context = CreateContext(profileState);

        try
        {
            var cut = context.Render<Profile>();

            Assert.NotNull(cut.Find("[data-testid='empty-state']"));

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
        var profileState = new ProfileStateService();
        profileState.SetLoading();

        var context = CreateContext(profileState);

        try
        {
            var cut = context.Render<Profile>();

            Assert.NotNull(cut.Find("[data-testid='loading-state']"));

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
        var profileState = new ProfileStateService();
        profileState.SetError("Failed to load profile.");

        var context = CreateContext(profileState);

        try
        {
            var cut = context.Render<Profile>();

            Assert.NotNull(cut.Find("[data-testid='error-state']"));

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

    [Fact]
    public async Task Profile_ShouldRenderDataState_WhenListingsExist()
    {
        var profileState = new ProfileStateService();

        profileState.SetListings(new List<ProfileListing>
        {
            new ProfileListing
            {
                Id = 101,
                Name = "TDD Test Bicycle",
                Price = 150m,
                Description = "Test listing",
                Status = ProfileListingStatus.Active
            }
        });

        var context = CreateContext(profileState);

        try
        {
            var cut = context.Render<Profile>();

            Assert.NotNull(cut.Find("[data-testid='data-state']"));

            Assert.Contains(
                "TDD Test Bicycle",
                cut.Markup
            );
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    // NEW TEST 1:
    // Loading must not display listings or error/empty states.

    [Fact]
    public async Task Profile_LoadingState_ShouldHideOtherStates()
    {
        var profileState = new ProfileStateService();
        profileState.SetLoading();

        var context = CreateContext(profileState);

        try
        {
            var cut = context.Render<Profile>();

            Assert.NotEmpty(
                cut.FindAll("[data-testid='loading-state']")
            );

            Assert.Empty(
                cut.FindAll("[data-testid='data-state']")
            );

            Assert.Empty(
                cut.FindAll("[data-testid='empty-state']")
            );

            Assert.Empty(
                cut.FindAll("[data-testid='error-state']")
            );

            Assert.Empty(
                cut.FindAll(".mud-tabs")
            );
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    // NEW TEST 2:
    // Error must not display listings or other states.

    [Fact]
    public async Task Profile_ErrorState_ShouldHideOtherStates()
    {
        var profileState = new ProfileStateService();
        profileState.SetError("API request failed.");

        var context = CreateContext(profileState);

        try
        {
            var cut = context.Render<Profile>();

            Assert.NotEmpty(
                cut.FindAll("[data-testid='error-state']")
            );

            Assert.Contains(
                "API request failed.",
                cut.Markup
            );

            Assert.Empty(
                cut.FindAll("[data-testid='data-state']")
            );

            Assert.Empty(
                cut.FindAll("[data-testid='empty-state']")
            );

            Assert.Empty(
                cut.FindAll("[data-testid='loading-state']")
            );

            Assert.Empty(
                cut.FindAll(".mud-tabs")
            );
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    // NEW TEST 3:
    // Empty must not display data, loading or error.

    [Fact]
    public async Task Profile_EmptyState_ShouldHideOtherStates()
    {
        var profileState = new ProfileStateService();
        profileState.SetListings(Array.Empty<ProfileListing>());

        var context = CreateContext(profileState);

        try
        {
            var cut = context.Render<Profile>();

            Assert.NotEmpty(
                cut.FindAll("[data-testid='empty-state']")
            );

            Assert.Empty(
                cut.FindAll("[data-testid='data-state']")
            );

            Assert.Empty(
                cut.FindAll("[data-testid='loading-state']")
            );

            Assert.Empty(
                cut.FindAll("[data-testid='error-state']")
            );
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    // NEW TEST 4:
    // Data must not display loading, error or empty.

    [Fact]
    public async Task Profile_DataState_ShouldHideOtherStates()
    {
        var profileState = new ProfileStateService();

        profileState.SetListings(new List<ProfileListing>
        {
            new ProfileListing
            {
                Id = 202,
                Name = "API Test Shelf",
                Price = 45m,
                Description = "Test description",
                Status = ProfileListingStatus.Active
            }
        });

        var context = CreateContext(profileState);

        try
        {
            var cut = context.Render<Profile>();

            Assert.NotEmpty(
                cut.FindAll("[data-testid='data-state']")
            );

            Assert.Contains(
                "API Test Shelf",
                cut.Markup
            );

            Assert.Empty(
                cut.FindAll("[data-testid='loading-state']")
            );

            Assert.Empty(
                cut.FindAll("[data-testid='error-state']")
            );

            Assert.Empty(
                cut.FindAll("[data-testid='empty-state']")
            );
        }
        finally
        {
            await context.DisposeAsync();
        }
    }
}
