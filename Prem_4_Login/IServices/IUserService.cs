using Prem_4_Login.API.DTOs;
using Prem_4_Login.API.Models;

namespace Prem_4_Login.API.IServices
{
    public interface IUserService
    {
        Task<int> RegisterAsync(
            RegisterRequest request,
            string? ipAddress);

        Task<User?> GetProfileAsync(
            int userId);

        Task<bool> UpdateProfileAsync(
            int userId,
            UpdateProfileRequest request,
            string? ipAddress);

        Task<bool> ChangePasswordAsync(
            int userId,
            ChangePasswordRequest request,
            string? ipAddress);

        Task<(IEnumerable<User> Users, int TotalRecords)>
            GetUsersAsync(
                int pageNumber,
                int pageSize,
                string? search,
                string? ipAddress);
    }
}   