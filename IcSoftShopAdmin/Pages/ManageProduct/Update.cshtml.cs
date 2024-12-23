﻿using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IcSoftShopAdmin.Pages.ManageProduct
{
    [Authorize(Roles = "admin,manager")]
    public class UpdateModel : PageModel
    {
        private readonly IProductServices _productServices;
        private readonly IColorServices _colorServices;
        private readonly ISizeServices _sizeServices;
        private readonly ICategoryServices _categoryServices;
        private readonly ICollectionServices _collectionServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UpdateModel(IProductServices productServices, IColorServices colorServices, ISizeServices sizeServices, ICategoryServices categoryServices, ICollectionServices collectionServices, IWebHostEnvironment webHostEnvironment)
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

        [BindProperty]
        public List<int> RemovedImageIds { get; set; } 
        [BindProperty]
        public bool RemovedImageSizes { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Product = await _productServices.GetProductById(id);
            if (Product == null)
            {
                return NotFound();
            }

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

            AvailableColors = await _colorServices.GetListColor();
            SelectedColorIds = Product.ProductColors.Select(pc => pc.ColorId).ToList();
            AvailableSizes = await _sizeServices.GetListSize();
            SelectedSizeIds = Product.ProductSizes.Select(ps => ps.SizeId).ToList();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            string RootPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "images", "Product" + Product.ProductId.ToString());

            if (ProductImageSize != null)
            {
                var targetFilePath = Path.Combine(RootPath, ProductImageSize.FileName);


                try
                {
                    using (var stream = new FileStream(targetFilePath, FileMode.Create))
                    {
                        await ProductImageSize.CopyToAsync(stream);
                    }

                    var imageUrl = Path.Combine("images", "Product" + Product.ProductId.ToString(), ProductImageSize.FileName).Replace("\\", "/");

                    if (Product.ProductSizeImage != null)
                    {
                        var oldImagePath = Path.Combine(RootPath, Path.GetFileName(Product.ProductSizeImage));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    Product.ProductSizeImage = imageUrl;

                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Có lỗi xảy ra khi lưu hình ảnh: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi lưu hình ảnh");
                }
            }
            else if(RemovedImageSizes == true)
            {
                var oldImagePath = Path.Combine(RootPath, Path.GetFileName(Product.ProductSizeImage));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                Product.ProductSizeImage = null;
            }    
            await _productServices.UpdateProduct(Product);

            if (SelectedColorIds != null)
            {
                await _productServices.DeleteProductColor(Product.ProductId);
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
                await _productServices.DeleteProductSize(Product.ProductId);
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


            if (!Directory.Exists(RootPath))
            {
                Directory.CreateDirectory(RootPath);
            }


            var existingImages = await _productServices.GetListProductImages(Product.ProductId);



            if (RemovedImageIds != null && RemovedImageIds.Any())
            {
                foreach (var imageId in RemovedImageIds)
                {
                    var imageToDelete = existingImages.FirstOrDefault(img => img.ImageId == imageId);
                    if (imageToDelete != null)
                    {
                        await _productServices.DeleteProductImageById(imageId);

                        var filePath = Path.Combine(RootPath, Path.GetFileName(imageToDelete.ImageUrl));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
            }


            foreach (var image in existingImages)
            {

                var file = Request.Form.Files.FirstOrDefault(f => f.Name == $"ProductImages_{image.ImageId}");

                if (file != null && file.Length > 0)
                {

                    var oldImagePath = Path.Combine(RootPath, Path.GetFileName(image.ImageUrl));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath); 
                    }


                    var targetFilePath = Path.Combine(RootPath, file.FileName);
                    try
                    {
                        using (var stream = new FileStream(targetFilePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

          
                        var newImageUrl = Path.Combine("images", "Product" + Product.ProductId.ToString(), file.FileName).Replace("\\", "/");
                        image.ImageUrl = newImageUrl;

                        await _productServices.UpdateProductImage(image);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Có lỗi xảy ra khi lưu hình ảnh: {ex.Message}");
                        ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi lưu hình ảnh");
                    }
                }
            }

            // Thêm các ảnh mới
            if (ProductImages != null && ProductImages.Count > 0)
            {
                foreach (var file in ProductImages)
                {
                    if (file.Length > 0)
                    {
                        var targetFilePath = Path.Combine(RootPath, file.FileName);

                        try
                        {
                            using (var stream = new FileStream(targetFilePath, FileMode.Create))
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
                            // Xử lý lỗi (log hoặc thông báo cho người dùng)
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
