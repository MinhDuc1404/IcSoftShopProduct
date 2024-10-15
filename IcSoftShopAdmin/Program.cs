using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectString);
});

builder.Services.AddDefaultIdentity<ShopUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<ICollectionServices, CollectionServices>();
builder.Services.AddScoped<IColorServices, ColorServices>();
builder.Services.AddScoped<ISizeServices, SizeServices>();

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

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

// Role seeding method with existence check
async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
{
    // Check if the "admin" role exists
    if (!await roleManager.RoleExistsAsync("admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("admin") { NormalizedName = "ADMIN" });
    }

    // Check if the "user" role exists
    if (!await roleManager.RoleExistsAsync("user"))
    {
        await roleManager.CreateAsync(new IdentityRole("user") { NormalizedName = "USER" });
    }
}
