using Identity.Web.Models;
using Identity.Web.Services;
using Identity.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;
using System.Diagnostics;
using System.Security.Claims;

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

            if (!indentityResult.Succeeded)
            {
                foreach (var item in indentityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View();
            }

            var exchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());
            var user = await _userManager.FindByNameAsync(request.UserName);
            var claimResult = await _userManager.AddClaimAsync(user!, exchangeExpireClaim);
            if (!claimResult.Succeeded)
            {
                foreach (var item in claimResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View();
            }

            TempData["SuccessMessage"] = "Successfully registered.";
            return RedirectToAction(nameof(HomeController.SignIn));
        }

        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            returnUrl ??= Url.Action("Index", "Home");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Incorrect credentials.");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Incorrect credentials"); // bunu password ucun yazmisam.
                ModelState.AddModelError("", $"Failed attempt count : {await _userManager.GetAccessFailedCountAsync(user)}");
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "You can login after 3 minutes");
                return View();
            }

            if (user.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(user, model.RememberMe, [new Claim("BirthDate", user.BirthDate.Value.ToString())]);
            }

            return Redirect(returnUrl!);
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

            // meselen https://localhost:7004/Home/ResetPassword?userId=<userId>&token=<token> bu qaydada link duzeldir.
            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = user.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

            await _emailService.ResetPassword(passwordResetLink, user.Email);

            TempData["Success"] = "Password reset link has been sent to your email.";

            return RedirectToAction(nameof(ForgetPassword));
        }

        // methodun aldigi parametrler ForgetPassword-dan passwordResetLink bunun qurdugu query string-den avtomatik dola bilir.UserId ve Token taniyir ozu.
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
            if (userId == null || token == null)
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
