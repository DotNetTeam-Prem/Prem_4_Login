using System.ComponentModel.DataAnnotations;

namespace Prem_4_Login.API.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(100)]
        public string ApplicantName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FatherName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$",
            ErrorMessage = "Enter valid 10 digit mobile number")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public string? ProfilePic { get; set; }
    }
}   