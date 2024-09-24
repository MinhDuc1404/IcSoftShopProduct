using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IcSoftShopAdmin.Pages.ManageSize
{
    public class UpdateModel : PageModel
    {
        private readonly ISizeServices _sizeServices;
        public UpdateModel(ISizeServices sizeServices)
        {
            _sizeServices = sizeServices;
        }

        [BindProperty]
        public Size Sizes { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Sizes = await _sizeServices.FindSize(id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            await _sizeServices.UpdateSize(Sizes);

            return RedirectToPage("/ManageSize/Index");
        }
    }
}
