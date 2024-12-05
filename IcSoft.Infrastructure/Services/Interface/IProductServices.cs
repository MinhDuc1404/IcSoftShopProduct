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
        Task<Product> UpdateProduct(Product product);

        Task DeleteProduct(Product product);

        Task<ProductImage> AddProductImage(ProductImage productImage);
        Task<ProductImage> UpdateProductImage(ProductImage productImage);
        Task<ProductImage> FindProductImageByImageId(int id);
        Task DeleteProductImages(int productId);

        Task<ProductColor> AddProductColor(ProductColor productColor);
        Task DeleteProductColor(int productId);
		Task<ProductSize> AddProductSize(ProductSize productSize);
        Task DeleteProductSize(int productId);


		Task<List<Product>> GetListProduct();

        Task<List<Product>> GetListProductByFilter(string? priceRange, string? sortOption);

        Task<ProductImage> GetUrlHeaderImage(int id);
        Task<Product> GetProductByName(string name);

        Task<Product> GetProductById(int id);
        Task<List<Product>> GetListProductByCategory(int categoryid);
        Task<List<Product>> GetListProductByCollection(int collectionid);

        Task<List<Product>> GetListProductByCategoryName(string categoryname);

        Task<List<Product>> GetListProductByCollectionName(string collectionname);

        Task<List<Product>> GetListProductByQuery(string query);

        Task<List<ProductImage>> GetListProductImages(int id);

        Task DeleteProductImageById(int id);



    }
}
