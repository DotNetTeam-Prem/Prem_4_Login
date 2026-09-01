using System.ComponentModel.DataAnnotations;

namespace Prem_4_Login.Web.DTOs
{
    public class UserProfileResponse
    {
        public int UserId { get; set; }

        public string LoginId { get; set; }
            = string.Empty;

        [Required(ErrorMessage = "Applicant Name is required")]
        public string ApplicantName { get; set; }
            = string.Empty;

        [Required(ErrorMessage = "Father Name is required")]
        public string FatherName { get; set; }
            = string.Empty;

        public string MobileNumber { get; set; }
            = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter valid email")]
        public string Email { get; set; }
            = string.Empty;

        [Required(ErrorMessage = "DOB is required")]
        public DateTime DOB { get; set; }

        public string? ProfilePic { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }
            = string.Empty;
    }
}