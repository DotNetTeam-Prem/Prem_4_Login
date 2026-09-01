namespace Prem_4_Login.Web.DTOs
{
    public class RegisterResponse
    {
        public string Message { get; set; }
            = string.Empty;

        public int UserId { get; set; }

        public string LoginId { get; set; }
            = string.Empty;

        public string DefaultPassword { get; set; }
            = string.Empty;
    }
}