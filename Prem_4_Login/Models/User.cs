namespace Prem_4_Login.API.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string LoginId { get; set; } = string.Empty;

        public string ApplicantName { get; set; } = string.Empty;

        public string FatherName { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime DOB { get; set; }

        public string? ProfilePic { get; set; }

        public string Password { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}