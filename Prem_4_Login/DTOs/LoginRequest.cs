using System.ComponentModel.DataAnnotations;

namespace Prem_4_Login.API.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "User Id is required")]
        public string LoginId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }

}