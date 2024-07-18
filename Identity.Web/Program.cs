using Identity.Web.ClaimProviders;
using Identity.Web.Extensions;
using Identity.Web.Models;
using Identity.Web.OptionModels;
using Identity.Web.Requirements;
using Identity.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Identity.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
            });

            builder.Services.AddIdentityCustom();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // hansi class-in constructorun-da EmailSettings class-i gonderilirse datalar appsetting.json-daki EmailSetting-den dolacaq.
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSetting"));

            // Indetity.Web(CurrentDirectory) folderinin icindeki folder-ler uzerinde rahat geze bilmek ucun yazilir.
            builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

            builder.Services.AddScoped<IClaimsTransformation, UserClaimTransformation>();
            builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();

            builder.Services.AddPolicy();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
