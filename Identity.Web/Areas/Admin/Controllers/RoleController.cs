using Identity.Web.Areas.Admin.Models;
using Identity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return View(roles);
        }

        public IActionResult Add()
        {
            return View();
        }

        public async Task<IActionResult> Add(RoleAddVM model)
        {
            var result = await _roleManager.CreateAsync(new AppRole { Name = model.Name });
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Errors.Select(x => x.Description).ToString()!);
                return View();
            }

            return RedirectToAction(nameof(RoleController.Index));
        }
    }
}
