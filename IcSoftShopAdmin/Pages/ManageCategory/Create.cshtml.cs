using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IcSoftShopAdmin.Pages.ManageCategory
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryServices _categoryServices;
        public CreateModel(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [BindProperty]
        public Category Category { get; set; }

        public void OnGet()
        {
            // Chưa có dữ liệu để hiển thị trong OnGet
        }
        public async Task<IActionResult> Onpost()
        {
           
            Category.CreateAt = DateTime.Now;
           await _categoryServices.AddCategory(Category);
            return RedirectToPage("/ManageCategory/Index"); // Chuyển hướng đến trang danh sách danh mục sau khi lư
        }
    }
}
