using Microsoft.AspNetCore.Mvc;
using Prem_4_Login.Web.DTOs;
using Prem_4_Login.Web.IServices;

namespace Prem_4_Login.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiService _userApiService;

        public UserController(
            IUserApiService userApiService)
        {
            _userApiService = userApiService;
        }


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var token =
                HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }


            var profile =
                await _userApiService.GetProfileAsync(token);


            if (profile == null)
            {
                HttpContext.Session.Clear();

                return RedirectToAction(
                    "Login",
                    "Auth");
            }


            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(
            UserProfileResponse request,
            IFormFile? ProfilePic)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }


            var token =
                HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }


            string? profilePicPath =
                request.ProfilePic;


            if (ProfilePic != null &&
                ProfilePic.Length > 0)
            {
                var allowedExtensions =
                    new[]
                    {
                        ".jpg",
                        ".jpeg",
                        ".png"
                    };


                var extension =
                    Path.GetExtension(
                        ProfilePic.FileName)
                        .ToLowerInvariant();


                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        nameof(ProfilePic),
                        "Only JPG, JPEG and PNG files are allowed.");

                    return View(request);
                }


                if (ProfilePic.Length >
                    2 * 1024 * 1024)
                {
                    ModelState.AddModelError(
                        nameof(ProfilePic),
                        "Profile picture must be less than 2 MB.");

                    return View(request);
                }


                var uploadFolder =
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads",
                        "profiles");


                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }


                var fileName =
                    $"{Guid.NewGuid():N}{extension}";


                var filePath =
                    Path.Combine(
                        uploadFolder,
                        fileName);


                using var stream =
                    new FileStream(
                        filePath,
                        FileMode.Create);


                await ProfilePic.CopyToAsync(stream);


                profilePicPath =
                    $"/uploads/profiles/{fileName}";
            }


            var updateRequest =
                new UpdateProfileRequest
                {
                    ApplicantName =
                        request.ApplicantName,

                    FatherName =
                        request.FatherName,

                    Email =
                        request.Email,

                    DOB =
                        request.DOB,

                    ProfilePic =
                        profilePicPath
                };


            var result =
                await _userApiService.UpdateProfileAsync(
                    token,
                    updateRequest);


            if (!result)
            {
                TempData["Error"] =
                    "Unable to update profile.";

                return View(request);
            }


            TempData["Success"] =
                "Profile updated successfully.";

            return RedirectToAction("Profile");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var token =
                HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
    ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var token =
                HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            try
            {
                var result =
                    await _userApiService.ChangePasswordAsync(
                        token,
                        request);

                if (!result)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Current password is incorrect or password could not be changed.");

                    return View(request);
                }

                TempData["Success"] =
                    "Password changed successfully.";

                return RedirectToAction("Profile");
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to change password. Please try again.");

                return View(request);
            }
        }
    }
}