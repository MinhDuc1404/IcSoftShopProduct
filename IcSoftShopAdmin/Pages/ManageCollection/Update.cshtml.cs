using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IcSoftShopAdmin.Pages.ManageCollection
{
    public class UpdateModel : PageModel
    {
        private readonly ICollectionServices _collectionServices;
        public UpdateModel(ICollectionServices collectionServices)
        {
            _collectionServices = collectionServices;
        }
        [BindProperty]
        public Collection Collections { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Collections = await _collectionServices.FindCollection(id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
           
            Collections.CreateAt = DateTime.Now;

            await _collectionServices.UpdateCollection(Collections);

            return RedirectToPage("/ManageCollection/Index");
        }
    }
}
