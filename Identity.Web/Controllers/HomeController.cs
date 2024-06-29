using Identity.Web.Models;
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

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
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
                TempData["SuccessMessage"] = "Successfully logged in.";
                return RedirectToAction(nameof(HomeController.SignUp));
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

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, false);
            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            return View();
        }
    }
}
