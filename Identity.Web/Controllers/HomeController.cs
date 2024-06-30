using Identity.Web.Models;
using Identity.Web.Services;
using Identity.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;
using System.Diagnostics;

namespace Identity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var indentityResult = await _userManager.CreateAsync(new() { UserName = request.UserName, PhoneNumber = request.Phone, Email = request.Email }, request.ConfirmPassword);

            if (indentityResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Successfully registered.";
                return RedirectToAction(nameof(HomeController.SignIn));
            }

            foreach (IdentityError item in indentityResult.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM model,string? returnUrl=null)
        {
            returnUrl ??= Url.Action("Index", "Home");

            var hasUser = await _userManager.FindByEmailAsync(model.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Incorrect credentials.");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);
            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "You can login after 3 minutes");
                return View();
            }

            ModelState.AddModelError("", "Incorrect credentials"); // bunu password ucun yazmisam.
            ModelState.AddModelError("", $"Failed attempt count : {await _userManager.GetAccessFailedCountAsync(hasUser)}");

            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "This user does not exists.");
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // meselen https://localhost:7004?userId=12213&token=aajsdfjdsalkfjkdsfj bu qaydada link duzeldir.
            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = user.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

            await _emailService.ResetPassword(passwordResetLink, user.Email);

            TempData["Success"] = "Password reset link has been sent to your email.";

            return RedirectToAction(nameof(ForgetPassword));
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];
            if (userId==null || token == null)
            {
                throw new Exception("Error occured");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString()!);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, token.ToString()!, model.Password);
            if (result.Succeeded)
            {
                TempData["Success"] = "Password reset successfully";
            }
            else
            {
                ModelState.AddModelError("", result.Errors.Select(x => x.Description).ToString()!);
            }

            return View();
        }
    }
}
