using Prem_4_Login.API.Models;

namespace Prem_4_Login.API.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetByLoginIdAsync(string loginId);

        Task<User?> GetByIdAsync(int userId);

        Task<int> GetRoleIdByNameAsync(string roleName);

        Task<int> CreateAsync(User user);

        Task UpdateProfileAsync(User user);

        Task UpdatePasswordAsync( int userId, string password);

        Task<long> LogAuditAsync( int? userId, string action, string description, string? ipAddress);

        Task<(IEnumerable<User> Users, int TotalRecords)>
            GetUsersAsync( int pageNumber, int pageSize, string? search);

    }
}