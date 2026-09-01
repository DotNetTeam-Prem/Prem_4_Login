using Prem_4_Login.API.DTOs;
using Prem_4_Login.API.IRepositories;
using Prem_4_Login.API.IServices;
using Prem_4_Login.API.Models;

namespace Prem_4_Login.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> RegisterAsync(
            RegisterRequest request,
            string? ipAddress)
        {
            var roleId =
                await _userRepository
                    .GetRoleIdByNameAsync("User");

            var passwordHash =
                BCrypt.Net.BCrypt.HashPassword("12345");

            var user = new User
            {
                LoginId = request.MobileNumber,

                ApplicantName = request.ApplicantName,

                FatherName = request.FatherName,

                MobileNumber = request.MobileNumber,

                Email = request.Email,

                DOB = request.DOB,

                ProfilePic = request.ProfilePic,

                Password = passwordHash,

                RoleId = roleId
            };

            var userId =
                await _userRepository.CreateAsync(user);

            await _userRepository.LogAuditAsync(
                userId,
                "REGISTRATION",
                $"User {request.MobileNumber} registered successfully.",
                ipAddress);

            return userId;
        }



        public async Task<User?> GetProfileAsync(
            int userId)
        {
            return await _userRepository
                .GetByIdAsync(userId);
        }


        public async Task<bool> UpdateProfileAsync(
            int userId,
            UpdateProfileRequest request,
            string? ipAddress)
        {
            var existingUser =
                await _userRepository
                    .GetByIdAsync(userId);

            if (existingUser == null)
            {
                return false;
            }

            existingUser.ApplicantName =
                request.ApplicantName;

            existingUser.FatherName =
                request.FatherName;

            existingUser.Email =
                request.Email;

            existingUser.DOB =
                request.DOB;

            existingUser.ProfilePic =
                request.ProfilePic;

            await _userRepository
                .UpdateProfileAsync(existingUser);

            await _userRepository.LogAuditAsync(
                userId,
                "UPDATE_PROFILE",
                "User profile updated successfully.",
                ipAddress);

            return true;
        }

        public async Task<bool> ChangePasswordAsync(
            int userId,
            ChangePasswordRequest request,
            string? ipAddress)
        {
            var user =
                await _userRepository
                    .GetByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            // Verify old password
            var valid =
                BCrypt.Net.BCrypt.Verify(
                    request.OldPassword,
                    user.Password);

            if (!valid)
            {
                return false;
            }

            // Hash new password
            var newPasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.NewPassword);

            await _userRepository.UpdatePasswordAsync(
                userId,
                newPasswordHash);

            await _userRepository.LogAuditAsync(
                userId,
                "CHANGE_PASSWORD",
                "Password changed successfully.",
                ipAddress);

            return true;
        }


        public async Task<(IEnumerable<User> Users, int TotalRecords)>
            GetUsersAsync(
                int pageNumber,
                int pageSize,
                string? search,
                string? ipAddress)
        {
            var result =
                await _userRepository.GetUsersAsync(
                    pageNumber,
                    pageSize,
                    search);

            await _userRepository.LogAuditAsync(
                null,
                "ADMIN_USER_LIST",
                $"Admin viewed user list. Page: {pageNumber}, PageSize: {pageSize}",
                ipAddress);

            return result;
        }
    }
}