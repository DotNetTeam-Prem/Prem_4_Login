using System.ComponentModel.DataAnnotations;

namespace Prem_4_Login.Web.DTOs
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;


        [Required(ErrorMessage = "New password is required")]
        [StringLength(
            50,
            MinimumLength = 5,
            ErrorMessage = "Password must be between 5 and 50 characters")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;


        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare(
            "NewPassword",
            ErrorMessage = "New password and confirm password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}