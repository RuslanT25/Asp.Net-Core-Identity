using Identity.Web.Models;
using Identity.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);
            UserVM userVM = new()
            {
                Email = user.Email,
                UserName = user.UserName,
                Phone = user.PhoneNumber
            };

            return View(userVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);
            bool checkOldPassword = await _userManager.CheckPasswordAsync(user!, model.PasswordOld);
            if (!checkOldPassword)
            {
                ModelState.AddModelError("", "Incorrect old password");
                return View();
            }
                
            var result = await _userManager.ChangePasswordAsync(user!, model.PasswordOld, model.PasswordNew);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Errors.Select(x => x.Description).ToString()!);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(user!);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(user!, model.PasswordNew, true, false);
            TempData["SuccessMessage"] = "Password changed successfully.";

            return View();
        }
    }
}
