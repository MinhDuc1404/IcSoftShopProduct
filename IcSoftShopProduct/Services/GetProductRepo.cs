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

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be null or empty.", nameof(name));
            }
                
            var product = await _productServices.GetProductByName(name);
 

            if (product == null)
            {
                throw new Exception($"Product with name '{name}' not found.");
            }

            var listproductcategory = await _productServices.GetListProductByCategory(product.CategoryID);

            listproductcategory = listproductcategory.Where(pc => pc.ProductQuantity > 0).ToList();

            var listproductcollection = await _productServices.GetListProductByCollection(product.CollectionID);

            listproductcollection = listproductcollection.Where(pc => pc.ProductQuantity > 0).ToList();

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

            products = products.Where(p => p.ProductQuantity > 0).ToList();
            var categories = await _categoryServices.GetListCategory();

 
            if (products == null || !products.Any())
            {
                throw new Exception("No products found.");
            }

            int totalProducts = products.Count();

     
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

       
            products = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ProductShopViewModel
            {
                Products = products,
                Categories = categories,
                CurrentPage = page,
                TotalPages = totalPages
            };
        }

        public async Task<ProductShopViewModel> GetProductShopSale(int page, int pageSize)
        {
            var products = await _productServices.GetListProduct();
            products = products.Where(p => p.ProductQuantity > 0).ToList();

            var productsale = products.Where(p => p.ProductSale > 0).ToList();
            var categories = await _categoryServices.GetListCategory();


            if (products == null || !products.Any())
            {
                throw new Exception("No products found.");
            }

            int totalProducts = productsale.Count();


            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);


            products = productsale
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ProductShopViewModel
            {
                Products = products,
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


             products = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ShopCategoryViewModel
            {
                Products = products,
                SearchName = searchname,
                Categories = categories,
                CurrentPage = page,
                TotalPages = totalPages
            };
        }
        public async Task<ProductShopFilterViewModel> GetProductShopFilter(string? priceRange, string? sortOption, int pageNumber)
        {
           var products = await _productServices.GetListProductByFilter(priceRange, sortOption);
        


            products = products.Where(p => p.ProductQuantity > 0).ToList();

            int pagesizes = 6;
            int totalProducts = products.Count();


            int totalPages = (int)Math.Ceiling((double)totalProducts / pagesizes);

            products = products
                .Skip((pageNumber - 1) * pagesizes)
                .Take(pagesizes)
                .ToList(); ;


            return new ProductShopFilterViewModel
            {
                Products = products,
                TotalPages = totalPages
            };
        }

        public async Task<ProductShopFilterViewModel> GetProductShopSaleFilter(string? priceRange, string? sortOption, int pageNumber)
        {

          var products = await _productServices.GetListProductByFilter(priceRange, sortOption);
     

            products = products.Where(p => p.ProductQuantity > 0 && p.ProductSale > 0).ToList();

            int pagesizes = 6;
            int totalProducts = products.Count();


            int totalPages = (int)Math.Ceiling((double)totalProducts / pagesizes);

            products = products
                .Skip((pageNumber - 1) * pagesizes)
                .Take(pagesizes)
                .ToList(); ;
    


            return new ProductShopFilterViewModel
            {
                Products = products,
                TotalPages = totalPages
            };
        }




        public async Task<SearchViewModel> GetProductSearchQuery(string query)
        {
            var products = await _productServices.GetListProductByQuery(query);
            var categories = await _categoryServices.GetListCategory();

            return new SearchViewModel
            {
                Products = products,
                Categories = categories,
                SearchName = query
            };
        }


    }

}
