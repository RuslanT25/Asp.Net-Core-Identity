using Azure.Core;
using Identity.Web.Models;
using Identity.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

namespace Identity.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);
            UserVM userVM = new()
            {
                Email = user.Email,
                UserName = user.UserName,
                Phone = user.PhoneNumber,
                Picture = user.Picture
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

        public async Task<IActionResult> EditUser()
        {
            ViewBag.GenderList = new SelectList(Enum.GetNames(typeof(Gender)));
            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var model = new UserEditVM()
            {
                UserName = user!.UserName!,
                Email = user.Email!,
                Phone = user.PhoneNumber!,
                BirthDate = user.BirthDate,
                City = user.City,
                Gender = user.Gender
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);
            user!.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.BirthDate = model.BirthDate;
            user.City = model.City;
            user.Gender = model.Gender;

            if (model.Picture != null && model.Picture.Length > 0)
            {
                // Identity.web(solution) içinde almaq istediyin folderin adini yazirsan. var wwwroot artiq solution icindeki folderi tutur.
                var wwwroot = _fileProvider.GetDirectoryContents("wwwroot"); // Directory
                var uploads = wwwroot.FirstOrDefault(x => x.Name == "uploads")?.PhysicalPath;  // string
                if (uploads == null)
                {
                    return View();
                }

                var userPictures = Path.Combine(uploads, "userpictures");
                var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Picture.FileName)}";
                var newPicturePath = Path.Combine(userPictures, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await model.Picture.CopyToAsync(stream);

                user.Picture = randomFileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Errors.Select(x => x.Description).ToString()!);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            if (model.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(user, true, [new Claim("BirthDate", user.BirthDate!.Value.ToString())]);
            }
            else
            {
                await _signInManager.SignInAsync(user, true);
            }

            TempData["SuccessMessage"] = "User updated successfully."; 

            return View();
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            string message = "You are not authorized to view this page. You can contact your administrator to get authorization.";
            ViewBag.message = message;

            return View();
        }

        public IActionResult Claims()
        {
            var userClaims = User.Claims.Select(x => new ClaimVM
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();

            return View(userClaims);
        }

        [Authorize(Policy ="BakuPolicy")]
        public IActionResult BakuPage()
        {
            return View();
        }

        [Authorize(Policy = "ExchangePolicy")]
        public IActionResult ExchangePage()
        {
            return View();
        }

        [Authorize(Policy = "ViolencePolicy")]
        public IActionResult ViolencePage()
        {
            return View();
        }
    }
}
