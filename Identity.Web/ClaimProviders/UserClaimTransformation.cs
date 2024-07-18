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

        // bu metod seyfe refresh olanda,ve ya icaze verilen seyfelere her girende,signin de ves. isleyir.
        // Yeni her defe Cookie-den melumatlari alir ve Claim-e donusdurub Add edir.

        // bunun yerinde daha best practice olan : _signInManager.SignInWithClaimsAsync ile ancaq SignIn,Edit ves-de Claim olusar.
        // Yeni bu ifadeni harda yazacaqsansa orda da new Claim ile olusduracaqsan.
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
