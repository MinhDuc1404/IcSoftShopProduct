using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IcSoftShopAdmin.Pages.ManageCategory
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryServices _categoryServices;

        public IndexModel(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        public List<Category> Categories { get; set; }
        public async Task OnGetAsync()
        {
            Categories = await _categoryServices.GetListCategory();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var Category = await _categoryServices.FindCategory(id);

            await _categoryServices.DeleteCategory(Category);

            return RedirectToPage("/ManageCategory/Index");
        }
    }
}
