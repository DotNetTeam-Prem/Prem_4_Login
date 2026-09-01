namespace Prem_4_Login.API.DTOs
{
    public class RegisterResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public int UserId { get; set; }

        public string LoginId { get; set; } = string.Empty;
    }
}