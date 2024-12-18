using IcSoft.Infrastructure.Migrations;
using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace IcSoftShopAdmin.Pages.ManageCategory
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryServices _categoryServices;
        private const int PageSize = 6; 

        public IndexModel(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        public List<Category> Categories { get; set; }
        [BindProperty]
        public Category NewCategory { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public int TotalCategories { get; set; }


        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            var categories = await _categoryServices.GetListCategory();
            CurrentPage = pageNumber;

            TotalCategories = categories.Count();

            TotalPages = (int)System.Math.Ceiling(TotalCategories / (double)PageSize);

            Categories = categories
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new
                {
                    categories = Categories,
                    totalPages = TotalPages,
                    currentPage = CurrentPage
                });
            }
            return Page();
        }

        public async Task<IActionResult> OnGetFilterAsync(int pageNumber = 1, string? search = "")
        {


            var categories = await _categoryServices.GetListCategory();
            if (!string.IsNullOrEmpty(search))
            {
                categories = categories
                    .Where(c => c.CategoryName.Contains(search) || c.CategoryID.ToString() == search)
                    .ToList();
            }

            TotalPages = (int)System.Math.Ceiling(categories.Count / (double)PageSize);
            CurrentPage = pageNumber;


            Categories = categories
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();


            return new JsonResult(new
            {
                categories = Categories,
                totalPages = TotalPages,
                currentPage = CurrentPage
            });
        }

        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            var Category = await _categoryServices.FindCategory(id);

            await _categoryServices.DeleteCategory(Category);

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostAddAsync()
        {

            NewCategory.CategoryID = 0;

            await _categoryServices.AddCategory(NewCategory);

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostEditAsync()
        {
            var category = await _categoryServices.FindCategory(NewCategory.CategoryID);
            if (category != null)
            {
               category.CategoryID = NewCategory.CategoryID;
                category.CategoryName = NewCategory.CategoryName;
                category.CreateAt = NewCategory.CreateAt;
            }
            await _categoryServices.UpdateCategory(category);
            return RedirectToPage();

        }
    }
}
