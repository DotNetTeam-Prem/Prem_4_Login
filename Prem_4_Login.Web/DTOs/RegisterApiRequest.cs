namespace Prem_4_Login.Web.DTOs
{
    public class RegisterApiRequest
    {
        public string ApplicantName { get; set; }
            = string.Empty;

        public string FatherName { get; set; }
            = string.Empty;

        public string MobileNumber { get; set; }
            = string.Empty;

        public string Email { get; set; }
            = string.Empty;

        public DateTime DOB { get; set; }

        public string? ProfilePic { get; set; }
    }
}