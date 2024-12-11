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

        // Relationships for Product, Category, and Collection
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryID);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Collection)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CollectionID);

        modelBuilder.Entity<Order>()
              .HasMany(o => o.OrderItems)
              .WithOne(oi => oi.Order)
              .HasForeignKey(oi => oi.OrderId);

        // Primary keys for ProductColor and ProductSize
        modelBuilder.Entity<ProductColor>()
           .HasKey(pc => new { pc.ProductId, pc.ColorId });

        modelBuilder.Entity<ProductSize>()
            .HasKey(ps => new { ps.ProductId, ps.SizeId });

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);

        // Removed HasData for roles (we will seed roles programmatically in Program.cs)
    }

    public DbSet<Collection> Collections { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<ShopUser> ShopUsers { get; set; }
    public DbSet<Colors> Colors { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<ProductColor> ProductColors { get; set; }
    public DbSet<ProductSize> ProductSizes { get; set; }

    public DbSet<CartItem> CartItems { get; set; }
    
    public DbSet<OrderItem> OrderItems { get; set; }

}
