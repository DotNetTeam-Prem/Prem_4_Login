using Prem_4_Login.API.DTOs;
using Prem_4_Login.API.Helpers;
using Prem_4_Login.API.IRepositories;
using Prem_4_Login.API.IServices;

namespace Prem_4_Login.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(
            IUserRepository userRepository,
            JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<LoginResponse?> LoginAsync(
            LoginRequest request,
            string? ipAddress)
        {
            var user =
                await _userRepository
                    .GetByLoginIdAsync(request.LoginId);

            if (user == null)
            {
                return null;
            }

            bool isPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.Password);

            if (!isPasswordValid)
            {
                return null;
            }

            var token =
                _jwtHelper.GenerateToken(user);

            await _userRepository.LogAuditAsync(
                user.UserId,
                "LOGIN",
                $"User {user.LoginId} logged in successfully.",
                ipAddress);

            return new LoginResponse
            {
                Token = token,
                UserId = user.UserId,
                LoginId = user.LoginId,
                ApplicantName = user.ApplicantName,
                RoleId = user.RoleId,
                RoleName = user.RoleName
            };
        }
    }
}