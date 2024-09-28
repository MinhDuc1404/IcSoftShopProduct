using IcSoft.Infrastructure.Services.Interface;
using IcSoftShopProduct.Models;
using Microsoft.AspNetCore.Mvc;

namespace IcSoftShopProduct.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ICategoryServices _categoryServices;
        private readonly ICollectionServices _collectionServices;
        public HeaderViewComponent(ICollectionServices collectionServices, ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
            _collectionServices = collectionServices;
        }
        public async Task<IViewComponentResult> InvokeAsync()
            { 
                return View(new HeaderViewModel()
                {
                   categories = await _categoryServices.GetListCategory(),
                   collections = await _collectionServices.GetListCollection()
                }); 
            }
    }
}
