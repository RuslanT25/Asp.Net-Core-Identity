using Identity.Web.Models;
using Identity.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Identity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
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
    }
}
