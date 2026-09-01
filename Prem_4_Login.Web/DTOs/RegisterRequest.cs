using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Prem_4_Login.Web.DTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Applicant Name is required")]
        public string ApplicantName { get; set; }
            = string.Empty;


        [Required(ErrorMessage = "Father Name is required")]
        public string FatherName { get; set; }
            = string.Empty;


        [Required(ErrorMessage = "Mobile Number is required")]
        [RegularExpression(
            @"^[6-9]\d{9}$",
            ErrorMessage = "Enter valid 10 digit mobile number")]
        public string MobileNumber { get; set; }
            = string.Empty;


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string Email { get; set; }
            = string.Empty;


        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DOB { get; set; }


        public IFormFile? ProfilePic { get; set; }
    }
}