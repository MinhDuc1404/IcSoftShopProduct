using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IcSoftShopAdmin.Pages.ManageCollection
{
    public class IndexModel : PageModel
    {
        private readonly ICollectionServices _collectionServices;
        
        public IndexModel(ICollectionServices collectionServices)
        {
            _collectionServices = collectionServices;
        }
        public List<Collection> Collection {  get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Collection = await _collectionServices.GetListCollection();
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var Collection = await _collectionServices.FindCollection(id);

            await _collectionServices.DeleteCollection(Collection);

            return RedirectToPage("/ManageCollection/Index");
        }
    }
}
