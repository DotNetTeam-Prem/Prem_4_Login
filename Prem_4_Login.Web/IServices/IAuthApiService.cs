using Prem_4_Login.Web.DTOs;

namespace Prem_4_Login.Web.IServices
{
    public interface IAuthApiService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<RegisterResponse?> RegisterAsync(
            RegisterApiRequest request);
    }
}