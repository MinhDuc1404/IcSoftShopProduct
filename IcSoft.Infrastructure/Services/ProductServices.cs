using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Services
{
    public class ProductServices : IProductServices
    {
        private readonly ApplicationDbContext _Context; 
        public ProductServices(ApplicationDbContext context) 
        {
            _Context = context;
        }
        public async Task<Product> AddProduct(Product product)
        {
           _Context.Add(product);
           await _Context.SaveChangesAsync();
            return product;
        }
        public async Task<ProductImage> AddProductImage(ProductImage productImage)
        {
            _Context.Add(productImage);
            await _Context.SaveChangesAsync();
            return productImage;
        }
        public async Task<ProductColor> AddProductColor(ProductColor productColor)
        {
            _Context.Add(productColor);
            await _Context.SaveChangesAsync();
            return productColor;
        }
        public async Task<ProductSize> AddProductSize(ProductSize productSize)
        {
            _Context.Add(productSize);
            await _Context.SaveChangesAsync();
            return productSize;
        }
        public async Task<List<Product>> GetListProduct()
        {
           return await _Context.Products.Select(e => new Product
            {
                ProductName = e.ProductName,
                ProductPrice = e.ProductPrice,
                ProductImage = e.ProductImage,
            }).ToListAsync();
        }
        public async Task<ProductImage> GetUrlHeaderImage(int id)
        {
            return await _Context.ProductImages
                .Where(p => p.ProductId == id)
                .FirstOrDefaultAsync();
        }
    }
}
