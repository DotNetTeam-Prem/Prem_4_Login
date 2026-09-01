using System.Net.Http.Headers;
using System.Net.Http.Json;
using Prem_4_Login.Web.DTOs;
using Prem_4_Login.Web.IServices;

namespace Prem_4_Login.Web.Services
{
    public class UserApiService : IUserApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserApiService(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory =
                httpClientFactory;
        }


        public async Task<UserProfileResponse?>
            GetProfileAsync(string token)
        {
            var client =
                _httpClientFactory.CreateClient("API");


            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);


            var response =
                await client.GetAsync(
                    "api/User/profile");


            if (!response.IsSuccessStatusCode)
            {
                return null;
            }


            return await response.Content
                .ReadFromJsonAsync<UserProfileResponse>();
        }


        public async Task<bool> UpdateProfileAsync(
            string token,
            UpdateProfileRequest request)
        {
            var client =
                _httpClientFactory.CreateClient("API");


            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);


            var response =
                await client.PutAsJsonAsync(
                    "api/User/profile",
                    request);


            return response.IsSuccessStatusCode;
        }
        public async Task<bool> ChangePasswordAsync(
    string token,
    ChangePasswordRequest request)
        {
            var client =
                _httpClientFactory.CreateClient("API");

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var response =
                await client.PutAsJsonAsync(
                    "api/User/change-password",
                    request);

            return response.IsSuccessStatusCode;
        }
    }
}