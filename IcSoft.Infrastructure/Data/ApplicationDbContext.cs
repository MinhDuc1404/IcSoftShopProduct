using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
public class ApplicationDbContext : IdentityDbContext<ShopUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        // Ensure that other configurations like roles are properly set up here
        var admin = new IdentityRole("admin");
        admin.NormalizedName = "ADMIN";

        var user = new IdentityRole("user");
        user.NormalizedName = "USER";

        modelBuilder.Entity<IdentityRole>().HasData(admin, user);
    }
  
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Collection> Collections { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }
    // Ensure this line is present and correctly configured
    public DbSet<ShopUser> ShopUsers { get; set; }
}
