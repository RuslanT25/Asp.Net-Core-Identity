using Identity.Web.Localizations;
using Identity.Web.Models;
using Identity.Web.Validations;

namespace Identity.Web.Extensions
{
    public static class StartupExtension
    {
        public static void AddIdentityCustom(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 3;
            })
                .AddPasswordValidator<PasswordValidator>()
                .AddUserValidator<UserValidator>()
                .AddErrorDescriber<LocalizatorIdentityErrorDescriber>()
               .AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
