using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using IcSoft.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IcSoftShopProduct.Services.Interface;
using IcSoftShopProduct.Services;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;



namespace IcSoftShopProduct
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            builder.Services.AddAuthentication().AddGoogle(googleOption =>
            {
                googleOption.ClientId = configuration["Authentication:Google:ClientId"];
                googleOption.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                googleOption.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                googleOption.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                googleOption.SaveTokens = true;
            });

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDefaultIdentity<ShopUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<SignInManager<ShopUser>>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddScoped<IProductServices, ProductServices>();
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();
            builder.Services.AddScoped<ICollectionServices, CollectionServices>();
            builder.Services.AddScoped<IColorServices, ColorServices>();
            builder.Services.AddScoped<ISizeServices, SizeServices>();

            builder.Services.AddScoped<IGetHomeRepo, GetHomeRepo>();
            builder.Services.AddScoped<IGetProductRepo, GetProductRepo>();
            builder.Services.AddScoped<IGetCartRepo, GetCartRepo>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
