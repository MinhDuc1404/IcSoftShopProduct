using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IcSoftShopAdmin.Pages.ManageCategory
{
    public class UpdateModel : PageModel
    {
        private readonly ICategoryServices _categoryServices;
        public UpdateModel(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [BindProperty]
        public Category Category { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Category = await _categoryServices.FindCategory(id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            //if (await _categoryServices.CategoryExists(Category.CategoryName))
            //{
            //    return NotFound();
            //}
            Category.CreateAt = DateTime.Now;
           
            await _categoryServices.UpdateCategory(Category);

            return RedirectToPage("/ManageCategory/Index");
        }
    
    }
}
