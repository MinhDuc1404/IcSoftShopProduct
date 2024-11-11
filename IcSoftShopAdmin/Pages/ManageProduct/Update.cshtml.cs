using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IcSoftShopAdmin.Pages.ManageProduct
{
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
        public List<int> SelectedColorIds { get; set; } // Lưu danh sách ID màu đã chọn
        [BindProperty]
        [Required(ErrorMessage = "Bạn phải chọn ít nhất một kích thước")]
        public List<int> SelectedSizeIds { get; set; } // Lưu danh sách ID kích thước đã chọn
        public List<SelectListItem> Categories { get; set; }

        public List<SelectListItem> Collections { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Bạn phải chọn ít nhất một hình ảnh")]
        public List<IFormFile> ProductImages { get; set; } // Danh sách hình ảnh
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy thông tin sản phẩm từ cơ sở dữ liệu
            Product = await _productServices.GetProductById(id);
            if (Product == null)
            {
                return NotFound();
            }

            var categoryList = await _categoryServices.GetListCategory();

            // Chuyển đổi Category thành SelectListItem
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

            // Lấy màu sắc và kích thước có sẵn
            AvailableColors = await _colorServices.GetListColor();
            SelectedColorIds = Product.ProductColors.Select(pc => pc.ColorId).ToList();
            AvailableSizes = await _sizeServices.GetListSize();
            SelectedSizeIds = Product.ProductSizes.Select(ps => ps.SizeId).ToList();

            return Page();
        }
		public async Task<IActionResult> OnPostAsync()
		{
			// Kiểm tra các trường bắt buộc

			Product.CreatedDate = DateTime.Now;

			await _productServices.UpdateProduct(Product);

			// Lưu các màu đã chọn
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


			// Lưu các kích thước đã chọn
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

			// Xử lý hình ảnh
			if (ProductImages != null && ProductImages.Count > 0)
			{
				// Đường dẫn đến wwwroot của project hiện tại
				string currentProjectRoot = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Product" + Product.ProductId.ToString());

				// Đường dẫn đến wwwroot của project ShopProduct
				string targetProjectRoot = Path.Combine(_webHostEnvironment.ContentRootPath, "..", "IcSoftShopProduct", "wwwroot", "images", "Product" + Product.ProductId.ToString());

				if (!Directory.Exists(currentProjectRoot))
				{
					Directory.CreateDirectory(currentProjectRoot);
				}

				if (!Directory.Exists(targetProjectRoot))
				{
					Directory.CreateDirectory(targetProjectRoot);
				}

				foreach (var file in ProductImages)
				{
					if (file.Length > 0)
					{
						// Đường dẫn file trong project hiện tại
						var currentFilePath = Path.Combine(currentProjectRoot, file.FileName);

						// Đường dẫn file trong project ShopProduct
						var targetFilePath = Path.Combine(targetProjectRoot, file.FileName);

						try
						{

							using (var stream = new FileStream(currentFilePath, FileMode.Create))
							{
								await file.CopyToAsync(stream);
							}

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

							await _productServices.UpdateProductImage(productImage);
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



			return RedirectToPage("/Index"); // Chuyển hướng đến trang danh sách sản phẩm
		}

	}
}
