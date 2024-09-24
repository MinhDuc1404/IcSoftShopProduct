using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Collection = IcSoft.Infrastructure.Models.Collection;

namespace IcSoftShopAdmin.Pages.ManageCollection
{
    public class CreateModel : PageModel
    {
        private readonly ICollectionServices _collectionServices;

        public CreateModel(ICollectionServices collectionServices)
        {
            _collectionServices = collectionServices;
        }

        [BindProperty]
        public Collection Collections { get; set; }

        public void OnGet()
        {
            // Chưa có dữ liệu để hiển thị trong OnGet
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Collections.CreateAt = DateTime.Now;
            await _collectionServices.AddCollection(Collections);
            return RedirectToPage("/ManageCollection/Index"); // Chuyển hướng đến trang danh sách danh mục sau khi lư
        }
    }
}
