using Identity.Web.Localizations;
using Identity.Web.Models;
using Identity.Web.Requirements;
using Identity.Web.Validations;
using Microsoft.AspNetCore.Identity;

namespace Identity.Web.Extensions
{
    public static class StartupExtension
    {
        public static void AddIdentityCustom(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });

            services.ConfigureApplicationCookie(options =>
            {
                CookieBuilder cookieBuilder = new()
                {
                    Name = "MyCookie"
                };
                options.LoginPath = new PathString("/Home/SignIn");
                options.LogoutPath = new PathString("/Member/Logout");
                options.AccessDeniedPath = new PathString("/Member/AccessDenied");
                options.Cookie = cookieBuilder;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
            });

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
                .AddDefaultTokenProviders()
               .AddEntityFrameworkStores<AppDbContext>();
        }

        public static void AddPolicy(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy("BakuPolicy", policy =>
                {
                    policy.RequireClaim("city", "Baku");
                });

            services.AddAuthorizationBuilder()
               .AddPolicy("ExchangePolicy", policy =>
               {
                   policy.AddRequirements(new ExchangeExpireRequirement());
               });

            services.AddAuthorizationBuilder()
              .AddPolicy("ViolencePolicy", policy =>
              {
                  policy.AddRequirements(new ViolenceRequirement() { Age = 18 });
              });
        }
    }
}
