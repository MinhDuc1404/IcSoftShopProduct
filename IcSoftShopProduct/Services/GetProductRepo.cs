using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using IcSoftShopProduct.Models;
using IcSoftShopProduct.Services.Interface;
using System;

namespace IcSoftShopProduct.Services
{
    public class GetProductRepo : IGetProductRepo
    {
        private readonly IProductServices _productServices;

        public GetProductRepo(IProductServices productServices)
        {
            _productServices = productServices ?? throw new ArgumentNullException(nameof(productServices));
        }

        public async Task<ProductDetailsViewModel> GetProductDetials(string name)
        {
            // Check if the provided name is valid
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be null or empty.", nameof(name));
            }
                
            var product = await _productServices.GetProductByName(name);

            // Check if product is null
            if (product == null)
            {
                throw new Exception($"Product with name '{name}' not found.");
            }

            // Retrieve products by category and collection, but only if the IDs are valid
            var listproductcategory = await _productServices.GetListProductByCategory(product.CategoryID);
            var listproductcollection = await _productServices.GetListProductByCollection(product.CollectionID);

            return new ProductDetailsViewModel
            {
                Product = product,
                ProductsCategory = listproductcategory,
                ProductsCollection = listproductcollection
            };
        }

        public async Task<ProductShopViewModel> GetProductShop()
        {
            var products = await _productServices.GetListProduct();

            // Ensure the list is not null
            if (products == null || !products.Any())
            {
                throw new Exception("No products found.");
            }

            return new ProductShopViewModel
            {
                Products = products
            };
        }
    }
}
