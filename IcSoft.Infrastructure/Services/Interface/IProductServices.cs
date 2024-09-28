using IcSoft.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Services.Interface
{
    public interface IProductServices
    {
        Task<Product> AddProduct(Product product);
        Task<ProductImage> AddProductImage(ProductImage productImage);
        Task<ProductColor> AddProductColor(ProductColor productColor);
        Task<ProductSize> AddProductSize(ProductSize productSize);

        Task<List<Product>> GetListProduct();

        Task<ProductImage> GetUrlHeaderImage(int id);
        Task<Product> GetProductByName(string name);

    }
}
