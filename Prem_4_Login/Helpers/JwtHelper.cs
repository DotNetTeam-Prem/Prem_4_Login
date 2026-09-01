using Microsoft.IdentityModel.Tokens;
using Prem_4_Login.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Prem_4_Login.API.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var key =
                _configuration["Jwt:Key"]!;

            var issuer =
                _configuration["Jwt:Issuer"];

            var audience =
                _configuration["Jwt:Audience"];

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.ApplicantName),

                new Claim(
                    "LoginId",
                    user.LoginId),

                new Claim(
                    ClaimTypes.Role,
                    user.RoleName),

                new Claim(
                    "RoleId",
                    user.RoleId.ToString())
            };


            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key));

            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}