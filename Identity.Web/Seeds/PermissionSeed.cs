using Identity.Web.Areas.Admin.Models;
using Identity.Web.Models;
using Identity.Web.PermissionRoot;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.Web.Seeds
{
    public class PermissionSeed
    {
        public static async Task Seed(RoleManager<AppRole> roleManager)
        {
            var hasBasicRole = await roleManager.RoleExistsAsync("BasicRole");
            if (!hasBasicRole)
            {
                await roleManager.CreateAsync(new() { Name = "BasicRole" });
                var basicRole = await roleManager.FindByNameAsync("BasicRole");
                await roleManager.AddClaimAsync(basicRole!, new Claim("Permission", Permissions.Stock.Read));
                await roleManager.AddClaimAsync(basicRole!, new Claim("Permission", Permissions.Order.Read));
                await roleManager.AddClaimAsync(basicRole!, new Claim("Permission", Permissions.Catalog.Read));
            }
        }
    }
}
