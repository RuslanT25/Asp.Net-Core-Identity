using Identity.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.Web.ClaimProviders
{
    public class UserClaimTransformation : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;
        //private readonly HttpContext _context;

        public UserClaimTransformation(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // User-i 2 cur almaq olar.1 cini islede bilmek ucun HttpContext di etmeliyem.(httpcontext controller-lerde hazir gelir)
            // var user = await _userManager.FindByNameAsync(_context.User.Identity!.Name!);

            var identity = principal.Identity as ClaimsIdentity;

            var user = await _userManager.FindByNameAsync(identity!.Name!);

            if (String.IsNullOrEmpty(user!.City))
            {
                return principal;
            }
              
            if (principal.HasClaim(x => x.Type != "city"))
            {
                Claim cityClaim = new("city", user.City);
                identity.AddClaim(cityClaim);
            }

            return principal;
        }
    }
}
