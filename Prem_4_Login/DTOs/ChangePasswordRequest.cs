using System.ComponentModel.DataAnnotations;

namespace Prem_4_Login.API.DTOs
{
    public class ChangePasswordRequest
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(5)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}