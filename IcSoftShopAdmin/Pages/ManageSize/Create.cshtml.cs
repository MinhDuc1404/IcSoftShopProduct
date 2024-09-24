using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IcSoftShopAdmin.Pages.ManageSize
{
    public class CreateModel : PageModel
    {
        private readonly ISizeServices _sizeServices;

        public CreateModel(ISizeServices sizeServices)
        {
            _sizeServices = sizeServices;
        }

        [BindProperty]
        public Size Sizes { get; set; }

        public void OnGet()
        {
            // Chưa có dữ liệu để hiển thị trong OnGet
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _sizeServices.AddSize(Sizes);
            return RedirectToPage("/ManageSize/Index");
        }
    }
}
