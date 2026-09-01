using System.ComponentModel.DataAnnotations;

namespace Prem_4_Login.API.DTOs
{
    public class UpdateProfileRequest
    {
        [Required]
        public string ApplicantName { get; set; } = string.Empty;

        [Required]
        public string FatherName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime DOB { get; set; }

        public string? ProfilePic { get; set; }
    }
}