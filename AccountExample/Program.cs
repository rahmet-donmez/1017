using AccountManagment.Core.Models;
using AccountManagment.Core.Repositories;
using AccountManagment.Core.Services;
using AccountManagment.Repository;
using AccountManagment.Repository.Repositories;
using AccountManagment.Service.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AccountExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Geliþtirme ortamý için Razor sayfalarýnýn runtime sýrasýnda derlenmesini saðlayan ayar
            var mvcBuilder = builder.Services.AddControllersWithViews();
            if (builder.Environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }
            builder.Services.AddHttpContextAccessor(); // IHttpContextAccessor kullanýmý için ekleyin

            // Cookie kimlik doðrulamasýný yapýlandýrýn
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/User/Login"; // Giriþ sayfasý
                options.LogoutPath = "/User/Logout"; // Çýkýþ sayfasý
                options.AccessDeniedPath = "/User/AccessDenied"; // Eriþim reddedildi sayfasý
            });
            builder.Services.AddDbContext<AppDbContext>(x =>
            {
                x.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"), options =>
                {
                    options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
                });
             
            });
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddScoped<IGenericService<User>,GenericService<User>>();
            builder.Services.AddScoped<IGenericRepository<User>,GenericRepository<User>>();
            builder.Services.AddScoped<IGenericService<Account>,GenericService<Account>>();
            builder.Services.AddScoped<IGenericRepository<Account>,GenericRepository<Account>>();
            builder.Services.AddScoped<IGenericService<AccountTransaction>,GenericService<AccountTransaction>>();
            builder.Services.AddScoped<IGenericRepository<AccountTransaction>,GenericRepository<AccountTransaction>>();
            builder.Services.AddScoped<IGenericService<Transfer>,GenericService<Transfer>>();
            builder.Services.AddScoped<IGenericRepository<Transfer>,GenericRepository<Transfer>>();
            builder.Services.AddScoped<LoginService, LoginService>();

            var app = builder.Build();
            
            // Hata iþleme ve güvenlik ayarlarý
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Middleware ekleme
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Varsayýlan rota ayarlarý
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
