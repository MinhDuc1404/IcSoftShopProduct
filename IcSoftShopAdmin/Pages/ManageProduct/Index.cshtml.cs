using IcSoft.Infrastructure.Migrations;
using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Printing;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.WebRequestMethods;

namespace IcSoftShopAdmin.Pages.ManageProduct
{
    public class IndexModel : PageModel
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        private readonly ICollectionServices _collectionServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const int PageSize = 6; // Số sản phẩm mỗi trang


        public IndexModel(IProductServices productServices, IWebHostEnvironment webHostEnvironment, ICategoryServices categoryServices, ICollectionServices collectionServices)
        {
            _productServices = productServices;
            _webHostEnvironment = webHostEnvironment;
            _categoryServices = categoryServices;
            _collectionServices = collectionServices;
        }
        public List<Product> Products { get; set; }
        public List<Product> TotalsProducts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

   

        public List<SelectListItem> SelectedCategory { get; set; }

        public List<SelectListItem> SelectedCollections { get; set; }


        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
          
            var allProducts = await _productServices.GetListProduct();
            TotalsProducts = allProducts;


            TotalPages = (int)System.Math.Ceiling(allProducts.Count / (double)PageSize);
            CurrentPage = pageNumber;

            var listCategories = await _categoryServices.GetListCategory();
            var listCollections = await _collectionServices.GetListCollection();

   
            SelectedCategory = listCategories.Select(c => new SelectListItem
            {
                Value = c.CategoryID.ToString(),
                Text = c.CategoryName
            }).ToList();

            SelectedCollections = listCollections.Select(c => new SelectListItem
            {
                Value = c.CollectionId.ToString(),
                Text = c.CollectionName
            }).ToList();


            Products = allProducts
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new
                {
                    products = Products,
                    totalPages = TotalPages,
                    currentPage = CurrentPage
                });
            }
            return Page();
        }
        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            var Product = await _productServices.GetProductById(id);

            string RootPath = Path.Combine(_webHostEnvironment.ContentRootPath,"wwwroot", "images", "Product" + id.ToString());

    
            if (Directory.Exists(RootPath))
            {
                foreach (var file in Directory.GetFiles(RootPath))
                {
                    try
                    {
                  
                        System.IO.File.Delete(file);
                    }
                    catch (Exception ex)
                    {
       
                        Console.WriteLine($"Lỗi khi xóa ảnh trong thư mục ShopProduct: {ex.Message}");
                    }
                }

           
                Directory.Delete(RootPath);
            }

     
            await _productServices.DeleteProduct(Product);
            await _productServices.DeleteProductColor(id);
            await _productServices.DeleteProductSize(id);
            await _productServices.DeleteProductImages(id);

       
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnGetFilterAsync(int pageNumber = 1, string? search ="", string? category="", string? price="", string? collections="")
        {


            var allProducts = await _productServices.GetListProduct();
            if (!string.IsNullOrEmpty(search))
            {
                allProducts = allProducts
                    .Where(p => p.ProductName.Contains(search))
                    .ToList();
            }


            if (!string.IsNullOrEmpty(category))
            {
                allProducts = allProducts.Where(p => p.CategoryID.ToString() == category).ToList();
            }


            if (!string.IsNullOrEmpty(price))
            {
                switch (price)
                {
                    case "0-500": 
                        allProducts = allProducts.Where(p => p.ProductPrice < 500000).ToList();
                        break;
                    case "500-1000":
                        allProducts = allProducts.Where(p => p.ProductPrice >= 500000 && p.ProductPrice <= 1000000).ToList();
                        break;
                    case "1000": 
                        allProducts = allProducts.Where(p => p.ProductPrice > 1000000).ToList();
                        break;
                }
            }

 
            if (!string.IsNullOrEmpty(collections))
            {
                allProducts = allProducts.Where(p => p.CollectionID.ToString() == collections).ToList();
            }


            TotalPages = (int)System.Math.Ceiling(allProducts.Count / (double)PageSize);
            CurrentPage = pageNumber;

     
            Products = allProducts
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

 
            return new JsonResult(new
            {
                products = Products,
                totalPages = TotalPages,
                currentPage = CurrentPage
            });
        }

        public async Task<IActionResult> OnGetDetailsAsync(int id)
        {


            var Products = await _productServices.GetProductById(id);
           
        
                return new JsonResult(new
                {
                    success = true,
                    product = Products
                });

        }

    }
}
