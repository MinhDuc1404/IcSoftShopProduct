using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using IcSoft.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectString);
});

var configuration = builder.Configuration;
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AdminApp.AuthCookie";  // Tên cookie riêng biệt cho ứng dụng admin
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

builder.Services.AddDefaultIdentity<ShopUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddScoped<IProductServices, ProductServices>();
    builder.Services.AddScoped<ICategoryServices, CategoryServices>();
    builder.Services.AddScoped<ICollectionServices, CollectionServices>();
    builder.Services.AddScoped<IColorServices, ColorServices>();
    builder.Services.AddScoped<ISizeServices, SizeServices>();

// Other service configurations...

var app = builder.Build();

// Seed roles at runtime with a check if they already exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await SeedRolesAsync(roleManager);  // Check and seed roles if they don't exist
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding roles: {ex.Message}");
    }
}

// Configure the HTTP request pipeline...
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Bật xác thực
app.UseAuthorization();

app.MapRazorPages();
app.Run();

// Role seeding method with existence check
async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
{
    if (!await roleManager.RoleExistsAsync("admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("admin") { NormalizedName = "ADMIN" });
    }
    if (!await roleManager.RoleExistsAsync("user"))
    {
        await roleManager.CreateAsync(new IdentityRole("user") { NormalizedName = "USER" });
    }
}
