using IcSoft.Infrastructure.Migrations;
using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IcSoftShopAdmin.Pages.ManageProduct
{
    public class ProductModel : PageModel
    {
        private readonly IProductServices _productServices;
        private readonly IColorServices _colorServices;
        private readonly ISizeServices _sizeServices;
        private readonly ICategoryServices _categoryServices;
        private readonly ICollectionServices _collectionServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductModel(IProductServices productServices, IColorServices colorServices, ISizeServices sizeServices, ICategoryServices categoryServices, ICollectionServices collectionServices, IWebHostEnvironment webHostEnvironment)
        {
            _productServices = productServices;
            _colorServices = colorServices;
            _sizeServices = sizeServices;
            _categoryServices = categoryServices;
            _collectionServices = collectionServices;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<Colors> AvailableColors { get; set; }
        public List<Size> AvailableSizes { get; set; }
        [BindProperty]
        public Product Product { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Bạn phải chọn ít nhất một màu sắc")]
        public List<int> SelectedColorIds { get; set; } 
        [BindProperty]
        [Required(ErrorMessage = "Bạn phải chọn ít nhất một kích thước")]
        public List<int> SelectedSizeIds { get; set; } 
        public List<SelectListItem> Categories { get; set; }

        public List<SelectListItem> Collections { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Bạn phải chọn ít nhất một hình ảnh")]
        public List<IFormFile> ProductImages { get; set; }
        [BindProperty]
        public IFormFile ProductImageSize { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            AvailableColors = await _colorServices.GetListColor();
            AvailableSizes = await _sizeServices.GetListSize();
            var categoryList = await _categoryServices.GetListCategory();

   
            Categories = categoryList.Select(c => new SelectListItem
            {
                Value = c.CategoryID.ToString(),
                Text = c.CategoryName
            }).ToList();

            var collectionlist = await _collectionServices.GetListCollection();

            Collections = collectionlist.Select(c => new SelectListItem
            {
                Value = c.CollectionId.ToString(),
                Text = c.CollectionName
            }).ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
           

            Product.CreatedDate = DateTime.Now;

        
            await _productServices.AddProduct(Product);


            string RootPath = Path.Combine(_webHostEnvironment.ContentRootPath,"wwwroot","images","Product" + Product.ProductId.ToString());

            if (ProductImageSize != null)
            {


                if (!Directory.Exists(RootPath))
                {
                    Directory.CreateDirectory(RootPath);
                }

                var ProductImagesFilePath = Path.Combine(RootPath, ProductImageSize.FileName);

                try
                {


                    using (var stream = new FileStream(ProductImagesFilePath, FileMode.Create))
                    {
                        await ProductImageSize.CopyToAsync(stream);
                    }
                    var imageUrl = Path.Combine("images", "Product" + Product.ProductId.ToString(), ProductImageSize.FileName).Replace("\\", "/");

                    Product.ProductSizeImage = imageUrl;


                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Có lỗi xảy ra khi lưu hình ảnh: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi lưu hình ảnh");
                }
            }


          


            if (SelectedColorIds != null)
            {
                foreach (var colorId in SelectedColorIds)
                {
                    ProductColor productColor = new ProductColor
                    {
                        ProductId = Product.ProductId,
                        Product = Product,
                        ColorId = colorId,
                        Color = await _colorServices.FindColor(colorId)
                    };

                    await _productServices.AddProductColor(productColor);
                }
            }
         


            if (SelectedSizeIds != null)
            {
                foreach (var sizeId in SelectedSizeIds)
                {
                    var productSize = new ProductSize
                    {
                        ProductId = Product.ProductId,
                        Product = Product,
                        Size = await _sizeServices.FindSize(sizeId),
                        SizeId = sizeId
                    };
                    await _productServices.AddProductSize(productSize);
                }
            }

   
            if (ProductImages != null && ProductImages.Count > 0)
            {

              
                if (!Directory.Exists(RootPath))
                {
                    Directory.CreateDirectory(RootPath);
                }

                foreach (var file in ProductImages)
                {
                    if (file.Length > 0)
                    {

                        var ProductImagesFilePath = Path.Combine(RootPath, file.FileName);

                        try
                        {

    
                            using (var stream = new FileStream(ProductImagesFilePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            var imageUrl = Path.Combine("images", "Product" + Product.ProductId.ToString(), file.FileName).Replace("\\", "/");

                            var productImage = new ProductImage
                            {
                                ImageUrl = imageUrl, 
                                ProductId = Product.ProductId
                            };

                            await _productServices.AddProductImage(productImage);
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine($"Có lỗi xảy ra khi lưu hình ảnh: {ex.Message}");
                            ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi lưu hình ảnh");
                        }
                    }
                }
            }

         





            return RedirectToPage("/Index"); 
        }
    }

}
