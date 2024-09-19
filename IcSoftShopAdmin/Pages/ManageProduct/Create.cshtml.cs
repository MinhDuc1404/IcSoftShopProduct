using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IcSoftShopAdmin.Pages.ManageProduct
{
    public class ProductModel : PageModel
    {
        private readonly IProductServices _productServices;
        public ProductModel(IProductServices productServices)
        {
            _productServices = productServices;
        }
        [BindProperty]
        public Product Product { get; set; }

        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> Collections { get; set; }

        public IFormFile ProductImage { get; set; }
        public void OnGet()
        {
        }
    }
}
