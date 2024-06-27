using Identity.Web.Areas.Admin.Models;
using Identity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult > UserList()
        {
            var users = await _userManager.Users.ToListAsync();
            var userVMs = users.Select(x => new UserVM()
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email
            }).ToList();

            return View(userVMs);
        }
    }
}
