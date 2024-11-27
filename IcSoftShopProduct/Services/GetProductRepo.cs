using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using IcSoftShopProduct.Models;
using IcSoftShopProduct.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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
                ProductsCategory = listproductcategory.Where(pc => pc.ProductQuantity > 0).ToList(),
                ProductsCollection = listproductcollection.Where(pc => pc.ProductQuantity > 0).ToList()
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
                Products = pagedProducts.Where(p => p.ProductQuantity > 0).ToList(),
                Categories = categories,
                CurrentPage = page,
                TotalPages = totalPages
            };
        }

        public async Task<ProductShopViewModel> GetProductShopSale(int page, int pageSize)
        {
            var products = await _productServices.GetListProduct();

            var productsale = products.Where(p => p.ProductSale > 0).ToList();
            var categories = await _categoryServices.GetListCategory();


            if (products == null || !products.Any())
            {
                throw new Exception("No products found.");
            }

            int totalProducts = productsale.Count();


            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);


            var pagedProducts = productsale
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ProductShopViewModel
            {
                Products = pagedProducts.Where(p => p.ProductQuantity > 0).ToList(),
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

            if (products1.Count() != 0)
            {
                products = products1;
            }
            else if (products2.Count() != 0)
            {
                products = products2;
            }

            products = products.Where(p => p.ProductQuantity > 0).ToList();
            var categories = await _categoryServices.GetListCategory();

          

            int totalProducts = products.Count();


            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);


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
        public async Task<ProductShopFilterViewModel> GetProductShopFilter(string? searchname, string? priceRange, string? sortOption, int pageNumber)
        {
            var products = new List<Product>();
            var pagedProducts = new List<Product>();
            var products1 = await _productServices.GetListProductByCategoryName(searchname);
            var products2 = await _productServices.GetListProductByCollectionName(searchname);
            if (!string.IsNullOrEmpty(searchname))
            {
                products = products1.Any() ? products1 : products2;
            }
            else
            {
                products = await _productServices.GetListProductByFilter(priceRange, sortOption);
            }


            products = products.Where(p => p.ProductQuantity > 0).ToList();

            int pagesizes = 6;
            int totalProducts = products.Count();


            int totalPages = (int)Math.Ceiling((double)totalProducts / pagesizes);

            if(priceRange == "all")
            {
                pagedProducts = products
                .Take(pagesizes)
                .ToList();
            }
            else
            {
                pagedProducts = products
                .Skip((pageNumber - 1) * pagesizes)
                .Take(pagesizes)
                .ToList(); ;
            }


            return new ProductShopFilterViewModel
            {
                Products = pagedProducts,
                TotalPages = totalPages
            };
        }

        public async Task<ProductShopFilterViewModel> GetProductShopSaleFilter(string? searchname, string? priceRange, string? sortOption, int pageNumber)
        {
            var products = new List<Product>();
            var pagedProducts = new List<Product>();
            var products1 = await _productServices.GetListProductByCategoryName(searchname);
            var products2 = await _productServices.GetListProductByCollectionName(searchname);
            if (!string.IsNullOrEmpty(searchname))
            {
                products = products1.Any() ? products1 : products2;
            }
            else
            {
                products = await _productServices.GetListProductByFilter(priceRange, sortOption);
            }


            products = products.Where(p => p.ProductQuantity > 0 && p.ProductSale > 0).ToList();

            int pagesizes = 6;
            int totalProducts = products.Count();


            int totalPages = (int)Math.Ceiling((double)totalProducts / pagesizes);

            if (priceRange == "all")
            {
                pagedProducts = products
                .Take(pagesizes)
                .ToList();
            }
            else
            {
                pagedProducts = products
                .Skip((pageNumber - 1) * pagesizes)
                .Take(pagesizes)
                .ToList(); ;
            }


            return new ProductShopFilterViewModel
            {
                Products = pagedProducts,
                TotalPages = totalPages
            };
        }




        public async Task<List<Product>> GetProductSearchQuery(string query)
        {
            var products = await _productServices.GetListProductByQuery(query);

            return products;
        }


    }

}
