using AccountManagment.Core.Models;
using AccountManagment.Core.Repositories;
using AccountManagment.Core.Services;
using AccountManagment.Repository;
using AccountManagment.Repository.Repositories;
using AccountManagment.Service.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AccountExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Geli�tirme ortam� i�in Razor sayfalar�n�n runtime s�ras�nda derlenmesini sa�layan ayar
            var mvcBuilder = builder.Services.AddControllersWithViews();
            if (builder.Environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

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

            var app = builder.Build();
            
            // Hata i�leme ve g�venlik ayarlar�
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Middleware ekleme
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Varsay�lan rota ayarlar�
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
