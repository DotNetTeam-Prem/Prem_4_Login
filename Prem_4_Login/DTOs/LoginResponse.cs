namespace Prem_4_Login.API.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;

        public int UserId { get; set; }

        public string LoginId { get; set; } = string.Empty;

        public string ApplicantName { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;
    }
}