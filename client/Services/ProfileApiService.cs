
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
    }
}
