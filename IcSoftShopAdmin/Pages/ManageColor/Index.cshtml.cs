using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IcSoftShopAdmin.Pages.ManageColor
{
    public class IndexModel : PageModel
    {
        private readonly IColorServices _colorServices;

        public IndexModel(IColorServices colorServices)
        {
            _colorServices = colorServices;
        }
        public List<Colors> Colors { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Colors = await _colorServices.GetListColor();
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var Colors = await _colorServices.FindColor(id);

            await _colorServices.DeleteColor(Colors);

            return RedirectToPage("/ManageColor/Index");
        }
    }
}
