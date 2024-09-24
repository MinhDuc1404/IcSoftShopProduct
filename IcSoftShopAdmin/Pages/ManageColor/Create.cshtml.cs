using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IcSoftShopAdmin.Pages.ManageColor
{
    public class CreateModel : PageModel
    {
        private readonly IColorServices _colorServices;

        public CreateModel(IColorServices colorServices)
        {
            _colorServices = colorServices;
        }

        [BindProperty]
        public Colors Colors { get; set; }

        public void OnGet()
        {
            // Chưa có dữ liệu để hiển thị trong OnGet
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _colorServices.AddColor(Colors);
            return RedirectToPage("/ManageColor/Index");
        }
    }
}
