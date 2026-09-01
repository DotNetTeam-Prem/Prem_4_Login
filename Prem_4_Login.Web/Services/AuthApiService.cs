using Prem_4_Login.Web.DTOs;
using Prem_4_Login.Web.IServices;
using System.Net.Http.Json;

namespace Prem_4_Login.Web.Services
{
    public class AuthApiService : IAuthApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthApiService(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<LoginResponse?> LoginAsync(
            LoginRequest request)
        {
            var client =
                _httpClientFactory.CreateClient("API");


            var response =
                await client.PostAsJsonAsync(
                    "api/auth/login",
                    request);


            if (!response.IsSuccessStatusCode)
            {
                return null;
            }


            return await response.Content
                .ReadFromJsonAsync<LoginResponse>();
        }
        public async Task<RegisterResponse?> RegisterAsync(
    RegisterApiRequest request)
        {

            var client =
                _httpClientFactory.CreateClient("API");

            var response =
                await client.PostAsJsonAsync(
                    "api/User/register",
                    request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content
                .ReadFromJsonAsync<RegisterResponse>();
        }
    }
}