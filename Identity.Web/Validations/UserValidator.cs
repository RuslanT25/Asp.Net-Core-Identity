using Identity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Web.Validations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();
            var isNumeric = int.TryParse(user.UserName[0].ToString(), out _);

            if (isNumeric)
            {
                errors.Add(new() { Code = "UserNameFirstLetterDigit", Description = "Username's first character can't be digit" });
            }

            if (errors.Count != 0)
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);

        }
    }
}
