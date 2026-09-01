using Prem_4_Login.API.DTOs;

namespace Prem_4_Login.API.IServices
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(
            LoginRequest request,
            string? ipAddress);
    }
}