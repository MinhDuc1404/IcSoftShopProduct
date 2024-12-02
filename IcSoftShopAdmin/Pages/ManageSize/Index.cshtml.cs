using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;

namespace IcSoftShopAdmin.Pages.ManageSize
{
    public class IndexModel : PageModel
    {
        private readonly ISizeServices _sizeServices;
        private const int PageSize = 5;

        public IndexModel(ISizeServices sizeServices)
        {
            _sizeServices = sizeServices;
        }
        public List<Size> Sizes { get; set; }
        public List<Size> TotalSizes { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            var sizes = await _sizeServices.GetListSize();
           TotalSizes = sizes;
            CurrentPage = pageNumber;

           TotalPages = (int)System.Math.Ceiling(sizes.Count / (double)PageSize);
            Sizes = sizes
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new
                {
                    sizes = Sizes,
                    totalPages = TotalPages,
                    currentPage = CurrentPage
                });
            }
            return Page();
        }

        public async Task<IActionResult> OnGetFilterAsync(int pageNumber = 1, string? search = "")
        {


            var sizes = await _sizeServices.GetListSize();
            if (!string.IsNullOrEmpty(search))
            {
                sizes = sizes
                    .Where(s => s.SizeName.Contains(search) || s.SizeId.ToString() == search)
                    .ToList();
            }

            TotalPages = (int)System.Math.Ceiling(sizes.Count / (double)PageSize);
            CurrentPage = pageNumber;


            Sizes = sizes
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();


            return new JsonResult(new
            {
                sizes = Sizes,
                totalPages = TotalPages,
                currentPage = CurrentPage
            });
        }
        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            var Sizes = await _sizeServices.FindSize(id);

            await _sizeServices.DeleteSize(Sizes);

            return new JsonResult(new { success = true });
        }
    }
}
