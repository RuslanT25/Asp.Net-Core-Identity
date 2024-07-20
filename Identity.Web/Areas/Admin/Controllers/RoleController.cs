using Identity.Web.Areas.Admin.Models;
using Identity.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Web.Areas.Admin.Controllers
{
    [Authorize]
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

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(x=>new RoleVM()
            {
                Id = x.Id,
                Name = x.Name!
            }).ToListAsync();

            return View(roles);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(RoleAddVM model)
        {
            var result = await _roleManager.CreateAsync(new AppRole { Name = model.Name });
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Errors.Select(x => x.Description).ToString()!);
                return View();
            }

            TempData["SuccessMessage"] = "Role created successfully";

            return RedirectToAction(nameof(RoleController.Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(string id)
        {
            return View();  
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(RoleVM model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id) ?? throw new Exception("Role doesn't exists.");
            role.Name = model.Name;
            await _roleManager.UpdateAsync(role);

            TempData["SuccessMessage"] = "Role updated successfully";

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id) ?? throw new Exception("Role doesn't exists.");

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception("Role doesn't exists.");
            }

            TempData["SuccessMessage"] = "Role deleted successfully";

            return RedirectToAction(nameof(RoleController.Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RoleAssign(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ViewBag.userId = id;

            var roles = await _roleManager.Roles.ToListAsync();

            var roleAssignVMs = new List<RoleAssignVM>();

            var userRoles = await _userManager.GetRolesAsync(user!);

            foreach (var role in roles)
            {
                var roleAssignVM = new RoleAssignVM() { Id = role.Id, Name = role.Name! };
                if (userRoles.Contains(role.Name!))
                {
                    roleAssignVM.Exist = true;
                }

                roleAssignVMs.Add(roleAssignVM);
            }

            return View(roleAssignVMs);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RoleAssign(string id,List<RoleAssignVM> roleAssignVMs)
        {
            var user = await _userManager.FindByIdAsync(id);
            foreach (var role in roleAssignVMs)
            {
                if (role.Exist == true)
                {
                    await _userManager.AddToRoleAsync(user!, role.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user!, role.Name);
                }
            }

            // await _signInManager.RefreshSignInAsync(user);  // bunu yazsan logout olmadan da Claim-ler elave olunacaq.
            return RedirectToAction(nameof(HomeController.UserList), "Home");
        }
    }
}
