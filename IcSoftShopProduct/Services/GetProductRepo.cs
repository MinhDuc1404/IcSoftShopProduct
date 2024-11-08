using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using IcSoftShopProduct.Models;
using IcSoftShopProduct.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace IcSoftShopProduct.Services
{
    public class GetProductRepo : IGetProductRepo
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;

        public GetProductRepo(IProductServices productServices, ICategoryServices categoryServices)
        {
            _productServices = productServices ?? throw new ArgumentNullException(nameof(productServices));
            _categoryServices = categoryServices ?? throw new ArgumentNullException( nameof(categoryServices));
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

        public async Task<ProductShopViewModel> GetProductShop(int page, int pageSize)
        {
            var products = await _productServices.GetListProduct();
            var categories = await _categoryServices.GetListCategory();

            // Ensure the list is not null
            if (products == null || !products.Any())
            {
                throw new Exception("No products found.");
            }
            // Tổng số sản phẩm
            int totalProducts = products.Count();

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Lấy danh sách sản phẩm cho trang hiện tại
            var pagedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ProductShopViewModel
            {
                Products = pagedProducts,
                Categories = categories,
                CurrentPage = page,
                TotalPages = totalPages
            };
        }

        public async Task<ShopCategoryViewModel> GetProductShopSearch(string? searchname, int page, int pageSize)
        {
            var products = new List<Product>();
            var products1 = await _productServices.GetListProductByCategoryName(searchname);
            var products2 = await _productServices.GetListProductByCollectionName(searchname);

           products = products1.Count() != 0 ? products1 : products2;

        
            var categories = await _categoryServices.GetListCategory();

          
            // Tổng số sản phẩm
            int totalProducts = products.Count();

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Lấy danh sách sản phẩm cho trang hiện tại
            var pagedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ShopCategoryViewModel
            {
                Products = pagedProducts,
                SearchName = searchname,
                Categories = categories,
                CurrentPage = page,
                TotalPages = totalPages
            };
        }
        public async Task<List<Product>> GetProductShopFilter(string? searchname, string? priceRange, string? sortOption, int curentPage)
        {
            var products = new List<Product>();
            var products1 = await _productServices.GetListProductByCategoryName(searchname);
            var products2 = await _productServices.GetListProductByCollectionName(searchname);

            if (products1.Count() != 0)
            {
                products = products1;
            }
            else if (products2.Count() != 0)
            {
                products = products2;
            }
            else if (products1.Count() == 0 && products2.Count() == 0)
            {
                products = await _productServices.GetListProduct();
            }

            // Tổng số sản phẩm
            int totalProducts = products.Count();

            var pageSize = 4;
            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Lấy danh sách sản phẩm cho trang hiện tại
            var pagedProducts = products
                .Skip((curentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            // Lọc sản phẩm theo mức giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                if (priceRange == "under-100")
                {
                    pagedProducts = pagedProducts.Where(p => p.ProductPrice < 100000).ToList();
                }
                else if (priceRange == "100-500")
                {
                    pagedProducts = pagedProducts.Where(p => p.ProductPrice >= 100000 && p.ProductPrice <= 500000).ToList();
                }
                else if (priceRange == "500-1000")
                {
                    pagedProducts = pagedProducts.Where(p => p.ProductPrice >= 500000 && p.ProductPrice <= 1000000).ToList();
                }
                else if (priceRange == "above-1000")
                {
                    pagedProducts = pagedProducts.Where(p => p.ProductPrice > 1000000).ToList();
                }
            }

            // Lọc sản phẩm theo sắp xếp
            if (!string.IsNullOrEmpty(sortOption))
            {
                if (sortOption == "name-asc")
                {
                    pagedProducts = pagedProducts.OrderBy(p => p.ProductName).ToList();
                }
                else if (sortOption == "name-desc")
                {
                    pagedProducts = pagedProducts.OrderByDescending(p => p.ProductName).ToList();
                }
                else if (sortOption == "price-asc")
                {
                    pagedProducts = pagedProducts.OrderBy(p => p.ProductPrice).ToList();
                }
                else if (sortOption == "price-desc")
                {
                    pagedProducts = pagedProducts.OrderByDescending(p => p.ProductPrice).ToList();
                }
                else if (sortOption == "new-item")
                {
                    pagedProducts = pagedProducts.OrderBy(p => p.CreatedDate).ToList();
                }
            }

            return pagedProducts;
        }


    }

}
