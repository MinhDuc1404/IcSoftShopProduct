using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IcSoftShopAdmin.Pages.ManageColor
{
    public class UpdateModel : PageModel
    {
        private readonly IColorServices _colorServices;

        public UpdateModel(IColorServices colorServices)
        {
            _colorServices = colorServices;
        }
        [BindProperty]
        public Colors colors { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            colors = await _colorServices.FindColor(id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            await _colorServices.UpdateColor(colors);

            return RedirectToPage("/ManageColor/Index");
        }
    }
}
