using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IcSoftShopAdmin.Pages.ManageSize
{
    public class IndexModel : PageModel
    {
        private readonly ISizeServices _sizeServices;

        public IndexModel(ISizeServices sizeServices)
        {
            _sizeServices = sizeServices;
        }
        public List<Size> Sizes { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Sizes = await _sizeServices.GetListSize();
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var Sizes = await _sizeServices.FindSize(id);

            await _sizeServices.DeleteSize(Sizes);

            return RedirectToPage("/ManangeSize/Index");
        }
    }
}
