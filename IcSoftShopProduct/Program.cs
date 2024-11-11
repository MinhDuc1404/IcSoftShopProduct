
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using IcSoft.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using IcSoftShopProduct.Services.Interface;
using IcSoftShopProduct.Services;

namespace IcSoftShopProduct
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;


           
            builder.Services.AddDistributedMemoryCache(); // Cáº¥u hÃ¬nh cache cho session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thá»i gian háº¿t háº¡n session
                options.Cookie.HttpOnly = true; // Äáº£m báº£o cookie chá» ÄÆ°á»£c truy cáº­p bá»i server
                options.Cookie.IsEssential = true; // Chá» Äá»nh session cookie lÃ  cáº§n thiáº¿t cho á»©ng dá»¥ng
            });


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "ShopApp.AuthCookie";  // Tên cookie riêng bi?t cho ?ng d?ng shop
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.SlidingExpiration = true;
                })
                .AddGoogle(googleOption =>
                {
                    googleOption.ClientId = configuration["Authentication:Google:ClientId"];
                    googleOption.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                    googleOption.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                    googleOption.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                    googleOption.SaveTokens = true;
                });


            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
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

            // Configure the HTTP request pipeline...
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // B?t xác th?c
            app.UseAuthorization();


            app.UseSession(); 

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
