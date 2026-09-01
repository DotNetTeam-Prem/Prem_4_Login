using Microsoft.AspNetCore.Mvc;
using Prem_4_Login.Web.DTOs;
using Prem_4_Login.Web.IServices;

namespace Prem_4_Login.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthApiService _authApiService;

        public AuthController(
            IAuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var result =
                    await _authApiService.LoginAsync(request);

                if (result == null)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Invalid User Id or Password.");

                    return View(request);
                }


                HttpContext.Session.SetString(
                    "JwtToken",
                    result.Token);

                HttpContext.Session.SetInt32(
                    "UserId",
                    result.UserId);

                HttpContext.Session.SetString(
                    "LoginId",
                    result.LoginId);

                HttpContext.Session.SetString(
                    "ApplicantName",
                    result.ApplicantName);

                HttpContext.Session.SetInt32(
                    "RoleId",
                    result.RoleId);

                HttpContext.Session.SetString(
                    "RoleName",
                    result.RoleName);



                if (result.RoleName.Equals(
                    "Admin",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(
                        "Index",
                        "Admin");
                }


                return RedirectToAction(
                    "Profile",
                    "User");
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to login. Please try again.");

                return View(request);
            }
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            if (request.DOB >= DateTime.Today)
            {
                ModelState.AddModelError(
                    nameof(request.DOB),
                    "Date of Birth must be a past date.");

                return View(request);
            }


            string? profilePicPath = null;

            if (request.ProfilePic != null &&
                request.ProfilePic.Length > 0)
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
                        request.ProfilePic.FileName)
                        .ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        nameof(request.ProfilePic),
                        "Only JPG, JPEG and PNG files are allowed.");

                    return View(request);
                }

                if (request.ProfilePic.Length >
                    2 * 1024 * 1024)
                {
                    ModelState.AddModelError(
                        nameof(request.ProfilePic),
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

                using (var stream =
                       new FileStream(
                           filePath,
                           FileMode.Create))
                {
                    await request.ProfilePic
                        .CopyToAsync(stream);
                }

                profilePicPath =
                    $"/uploads/profiles/{fileName}";
            }


            try
            {
                var apiRequest =
                    new RegisterApiRequest
                    {
                        ApplicantName =
                            request.ApplicantName,

                        FatherName =
                            request.FatherName,

                        MobileNumber =
                            request.MobileNumber,

                        Email =
                            request.Email,

                        DOB =
                            request.DOB,

                        ProfilePic =
                            profilePicPath
                    };

                var result =
                    await _authApiService.RegisterAsync(
                        apiRequest);

                if (result == null)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Registration failed. Please try again.");

                    return View(request);
                }


                TempData["Success"] =
                    $"Registration successful. " +
                    $"Your User ID is {result.LoginId}. " +
                    $"Your default password is 12345.";


                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to process registration. Please try again.");

                return View(request);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}