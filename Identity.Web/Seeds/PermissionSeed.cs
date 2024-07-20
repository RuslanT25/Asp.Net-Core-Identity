﻿using Identity.Web.Areas.Admin.Models;
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
            var hasAdvancedRole = await roleManager.RoleExistsAsync("AdvancedRole");
            var hasAdminRole = await roleManager.RoleExistsAsync("AdminRole");

            if (!hasBasicRole)
            {
                await roleManager.CreateAsync(new() { Name = "BasicRole" });
                var basicRole = await roleManager.FindByNameAsync("BasicRole");

                await AddReadPermission(basicRole!, roleManager);
            }

            if (!hasAdvancedRole)
            {

                await roleManager.CreateAsync(new AppRole() { Name = "AdvancedRole" });
                var advancedRole = (await roleManager.FindByNameAsync("AdvancedRole"))!;

                await AddReadPermission(advancedRole, roleManager);
                await AddUpdateAndCreatePermission(advancedRole, roleManager);
            }

            if (!hasAdminRole)
            {

                await roleManager.CreateAsync(new AppRole() { Name = "AdminRole" });
                var adminRole = (await roleManager.FindByNameAsync("AdminRole"))!;

                await AddReadPermission(adminRole, roleManager);
                await AddUpdateAndCreatePermission(adminRole, roleManager);
                await AddDeletePermission(adminRole, roleManager);
            }
        }

        public static async Task AddReadPermission(AppRole role, RoleManager<AppRole> roleManager)
        {

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Stock.Read));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Order.Read));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Catalog.Read));

        }

        public static async Task AddUpdateAndCreatePermission(AppRole role, RoleManager<AppRole> roleManager)
        {

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Stock.Add));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Order.Add));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Catalog.Add));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Stock.Update));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Order.Update));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Catalog.Update));

        }
        public static async Task AddDeletePermission(AppRole role, RoleManager<AppRole> roleManager)
        {

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Stock.Delete));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Order.Delete));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permissions.Catalog.Delete));

        }
    }
}
