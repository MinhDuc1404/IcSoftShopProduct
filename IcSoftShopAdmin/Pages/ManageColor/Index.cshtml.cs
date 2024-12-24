using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;

namespace IcSoftShopAdmin.Pages.ManageColor
{
    [Authorize(Roles = "admin,manager")]
    public class IndexModel : PageModel
    {
        private readonly IColorServices _colorServices;
        private const int PageSize = 7;

        public IndexModel(IColorServices colorServices)
        {
            _colorServices = colorServices;
        }
        public List<Colors> Colors { get; set; }
        [BindProperty]
        public Colors NewColor { get; set; }

        public List<Colors> TotalColors { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }
        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            var colors = await _colorServices.GetListColor();
            TotalColors = colors;

            CurrentPage = pageNumber;


            TotalPages = (int)System.Math.Ceiling(colors.Count / (double)PageSize);
            Colors = colors
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new
                {
                    colors = Colors,
                    totalPages = TotalPages,
                    currentPage = CurrentPage
                });
            }
            return Page();
        }
        public async Task<IActionResult> OnGetFilterAsync(int pageNumber = 1, string? search = "")
        {


            var colors = await _colorServices.GetListColor();
            if (!string.IsNullOrEmpty(search))
            {
                colors = colors
                    .Where(c => c.ColorName.Contains(search) || c.ColorCode.Contains(search))
                .ToList();
            }

            TotalPages = (int)System.Math.Ceiling(colors.Count / (double)PageSize);
            CurrentPage = pageNumber;
            Colors = colors
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();


            return new JsonResult(new
            {
                colors = Colors,
                totalPages = TotalPages,
                currentPage = CurrentPage
            });
        }
        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            var Colors = await _colorServices.FindColor(id);

            await _colorServices.DeleteColor(Colors);

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            await _colorServices.AddColor(NewColor);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            var color = await _colorServices.FindColor(NewColor.ColorId);
            if(color != null)
            {
                color.ColorId = NewColor.ColorId;
                color.ColorCode = NewColor.ColorCode;
                color.CreateAt = NewColor.CreateAt;
                color.ColorName = NewColor.ColorName;
            }
            await _colorServices.UpdateColor(color);
            return RedirectToPage();
        }
    }
}
