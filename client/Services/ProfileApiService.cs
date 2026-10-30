
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace client.Services
{
    public class ProfileApiService
    {
        private readonly HttpClient _httpClient;

        public ProfileApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProfileListing>> GetListingsAsync()
        {
            var listings = await _httpClient
                .GetFromJsonAsync<List<ProfileListing>>("api/listings");

            return listings ?? new List<ProfileListing>();
        }

        public async Task UpdateListingAsync(ProfileListing listing)
        {
            var request = new
            {
                name = listing.Name,
                price = listing.Price,
                description = listing.Description,
                image = listing.Image,
                status = (int)listing.Status
            };

            using var response = await _httpClient.PutAsJsonAsync(
                $"api/listings/{listing.Id}", request);

            response.EnsureSuccessStatusCode();
        }

        public async Task MarkAsSoldAsync(ProfileListing listing)
        {
            var request = new
            {
                name = listing.Name,
                price = listing.Price,
                description = listing.Description,
                image = listing.Image,
                status = 1
            };

            using var response = await _httpClient.PutAsJsonAsync(
                $"api/listings/{listing.Id}", request);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteListingAsync(int id)
        {
            using var response = await _httpClient.DeleteAsync(
                $"api/listings/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
