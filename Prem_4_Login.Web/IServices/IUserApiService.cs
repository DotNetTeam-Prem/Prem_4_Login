using Prem_4_Login.Web.DTOs;

namespace Prem_4_Login.Web.IServices
{
    public interface IUserApiService
    {
        Task<UserProfileResponse?> GetProfileAsync(
            string token);

        Task<bool> UpdateProfileAsync(
            string token,
            UpdateProfileRequest request);
        Task<bool> ChangePasswordAsync(
            string token,
            ChangePasswordRequest request);
    }
}