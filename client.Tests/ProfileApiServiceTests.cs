
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using client.Services;
using Xunit;

namespace client.Tests;

public class ProfileApiServiceTests
{
    private sealed class FakeHttpHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _responseBody;

        public HttpMethod? LastMethod { get; private set; }
        public Uri? LastRequestUri { get; private set; }

        public FakeHttpHandler(
            HttpStatusCode statusCode,
            string responseBody)
        {
            _statusCode = statusCode;
            _responseBody = responseBody;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastMethod = request.Method;
            LastRequestUri = request.RequestUri;

            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(
                    _responseBody,
                    Encoding.UTF8,
                    "application/json")
            };

            return Task.FromResult(response);
        }
    }

    private static HttpClient CreateHttpClient(
        HttpMessageHandler handler)
    {
        return new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost:5000/")
        };
    }

    [Fact]
    public async Task GetListingsAsync_ShouldSendGetRequest()
    {
        var handler = new FakeHttpHandler(
            HttpStatusCode.OK,
            "[]");

        using var httpClient = CreateHttpClient(handler);

        var service = new ProfileApiService(httpClient);

        await service.GetListingsAsync();

        Assert.Equal(HttpMethod.Get, handler.LastMethod);

        Assert.Equal(
            "http://localhost:5000/api/listings",
            handler.LastRequestUri?.ToString());
    }

    [Fact]
    public async Task GetListingsAsync_ShouldDeserializeListings()
    {
        var json = """
        [
            {
                "id": 101,
                "name": "Test Bicycle",
                "price": 150,
                "description": "A test bicycle",
                "image": "/images/bicycle.png",
                "posted": "Today",
                "views": 12,
                "inquiries": 3,
                "status": 0
            }
        ]
        """;

        var handler = new FakeHttpHandler(
            HttpStatusCode.OK,
            json);

        using var httpClient = CreateHttpClient(handler);

        var service = new ProfileApiService(httpClient);

        var listings = await service.GetListingsAsync();

        Assert.Single(listings);

        Assert.Equal(101, listings[0].Id);
        Assert.Equal("Test Bicycle", listings[0].Name);
        Assert.Equal(150m, listings[0].Price);
        Assert.Equal("A test bicycle", listings[0].Description);
        Assert.Equal(12, listings[0].Views);
        Assert.Equal(3, listings[0].Inquiries);
        Assert.Equal(
            ProfileListingStatus.Active,
            listings[0].Status);
    }

    [Fact]
    public async Task GetListingsAsync_ShouldReturnEmptyList()
    {
        var handler = new FakeHttpHandler(
            HttpStatusCode.OK,
            "[]");

        using var httpClient = CreateHttpClient(handler);

        var service = new ProfileApiService(httpClient);

        var listings = await service.GetListingsAsync();

        Assert.NotNull(listings);
        Assert.Empty(listings);
    }

    [Fact]
    public async Task GetListingsAsync_ShouldThrow_WhenServerReturnsError()
    {
        var handler = new FakeHttpHandler(
            HttpStatusCode.InternalServerError,
            """
            {
                "title": "Internal Server Error",
                "status": 500
            }
            """);

        using var httpClient = CreateHttpClient(handler);

        var service = new ProfileApiService(httpClient);

        await Assert.ThrowsAsync<HttpRequestException>(
            () => service.GetListingsAsync());
    }
}
