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
        // Cấu hình mối quan hệ giữa Product và Category
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryID);

        // Cấu hình mối quan hệ giữa Product và Collection
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Collection)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CollectionID);

        modelBuilder.Entity<Order>()
              .HasMany(o => o.OrderItems)
              .WithOne(oi => oi.Order)
              .HasForeignKey(oi => oi.OrderId);
        // Định nghĩa khóa chính cho bảng ProductColor
        modelBuilder.Entity<ProductColor>()
           .HasKey(pc => new { pc.ProductId, pc.ColorId });

        // Định nghĩa khóa chính cho bảng ProductSize
        modelBuilder.Entity<ProductSize>()
            .HasKey(ps => new { ps.ProductId, ps.SizeId });

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);
        // Ensure that other configurations like roles are properly set up here
        var admin = new IdentityRole("admin");
        admin.NormalizedName = "ADMIN";

        var user = new IdentityRole("user");
        user.NormalizedName = "USER";

        modelBuilder.Entity<IdentityRole>().HasData(admin, user);
    }
        public DbSet<Collection> Collections { get; set; }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }

    public DbSet<ProductImage> ProductImages { get; set; }

    public DbSet<Coupon> Coupons { get; set; }
    // Ensure this line is present and correctly configured
    public DbSet<ShopUser> ShopUsers { get; set; }
    public DbSet<Colors> Colors { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<ProductColor> ProductColors { get; set; }
    public DbSet<ProductSize> ProductSizes { get; set; }

}
