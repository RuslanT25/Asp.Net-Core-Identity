using Microsoft.AspNetCore.Identity;

namespace Identity.Web.Localizations
{
    public class LocalizatorIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = "DublicateUserName", Description = $"{userName} basqa istifade terefinden alinib." };
            // return base.DuplicateUserName(userName);
        }
    }
}
