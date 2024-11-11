using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Printing;

namespace IcSoftShopAdmin.Pages.ManageProduct
{
    public class IndexModel : PageModel
    {
        private readonly IProductServices _productServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const int PageSize = 5; // Số sản phẩm mỗi trang
        public IndexModel(IProductServices productServices, IWebHostEnvironment webHostEnvironment)
        {
            _productServices = productServices;
            _webHostEnvironment = webHostEnvironment;
        }
        public List<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public async Task<IActionResult> OnGetAsync(int pageNumber)
        {
            var allProducts = await _productServices.GetListProduct();
            // Tính tổng số trang
            TotalPages = (int)System.Math.Ceiling(allProducts.Count / (double)PageSize);
            CurrentPage = pageNumber;

            // Lấy sản phẩm cho trang hiện tại
            Products = allProducts
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var Product = await _productServices.GetProductById(id);

            // Lấy đường dẫn đến thư mục chứa ảnh trong dự án hiện tại và dự án ShopProduct
            string currentProjectRoot = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Product" + id.ToString());
            string targetProjectRoot = Path.Combine(_webHostEnvironment.ContentRootPath, "..", "IcSoftShopProduct", "wwwroot", "images", "Product" + id.ToString());

            // Xóa các ảnh từ thư mục trong dự án hiện tại
            if (Directory.Exists(currentProjectRoot))
            {
                foreach (var file in Directory.GetFiles(currentProjectRoot))
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi xóa ảnh trong thư mục hiện tại: {ex.Message}");
                    }
                }

                // Xóa thư mục sau khi đã xóa tất cả ảnh
                Directory.Delete(currentProjectRoot);
            }

            // Xóa các ảnh từ thư mục trong dự án ShopProduct
            if (Directory.Exists(targetProjectRoot))
            {
                foreach (var file in Directory.GetFiles(targetProjectRoot))
                {
                    try
                    {
                        // Xóa ảnh trong thư mục ShopProduct
                        System.IO.File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi khi xóa
                        Console.WriteLine($"Lỗi khi xóa ảnh trong thư mục ShopProduct: {ex.Message}");
                    }
                }

                // Xóa thư mục sau khi đã xóa tất cả ảnh
                Directory.Delete(targetProjectRoot);
            }

            await _productServices.DeleteProduct(Product);

            await _productServices.DeleteProductColor(id);

            await _productServices.DeleteProductSize(id);

            await _productServices.DeleteProductImages(id);

            return RedirectToPage("/ManageProduct/Index");
        }
    }
}
