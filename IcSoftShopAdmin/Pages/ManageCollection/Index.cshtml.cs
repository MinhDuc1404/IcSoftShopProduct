using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;

namespace IcSoftShopAdmin.Pages.ManageCollection
{
    public class IndexModel : PageModel
    {
        private readonly ICollectionServices _collectionServices;
        private const int PageSize = 5;
        public IndexModel(ICollectionServices collectionServices)
        {
            _collectionServices = collectionServices;
        }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public List<Collection> TotalCollections { get; set; }

        public List<Collection> Collections { get; set; }


        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            var collections = await _collectionServices.GetListCollection();
            CurrentPage = pageNumber;

            TotalCollections = collections;

            TotalPages = (int)System.Math.Ceiling(collections.Count / (double)PageSize);
            Collections = collections
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new
                {
                    collections = Collections,
                    totalPages = TotalPages,
                    currentPage = CurrentPage
                });
            }
            return Page();
        }

        public async Task<IActionResult> OnGetFilterAsync(int pageNumber = 1, string? search = "")
        {


            var collections = await _collectionServices.GetListCollection();
            if (!string.IsNullOrEmpty(search))
            {
                collections = collections
                    .Where(c => c.CollectionName.Contains(search) || c.CollectionId == Int64.Parse(search))
                .ToList();
            }

            TotalPages = (int)System.Math.Ceiling(collections.Count / (double)PageSize);
            CurrentPage = pageNumber;
            Collections = collections
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();


            return new JsonResult(new
            {
                collections = Collections,
                totalPages = TotalPages,
                currentPage = CurrentPage
            });
        }
        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            var Collection = await _collectionServices.FindCollection(id);

            await _collectionServices.DeleteCollection(Collection);

            return new JsonResult(new { success = true });
        }
    }
}
