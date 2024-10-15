using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using IcSoftShopProduct.Models;
using IcSoftShopProduct.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IcSoftShopProduct.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ICategoryServices _categoryServices;
        private readonly ICollectionServices _collectionServices;
        private readonly IGetCartRepo _getCartRepo;
        private readonly UserManager<ShopUser> _userManager;
     
        public HeaderViewComponent(ICollectionServices collectionServices, ICategoryServices categoryServices, IGetCartRepo getCartRepo, UserManager<ShopUser> userManager)
        {
            _categoryServices = categoryServices;
            _collectionServices = collectionServices;
            _getCartRepo = getCartRepo;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            if(user != null)
           { 
            var listcartitem = _getCartRepo.GetListCartItems(user.Id);
 
            
                var cartitemcount = listcartitem.Sum(c => c.Quantity);

                return View(new HeaderViewModel()
                {
                    categories = await _categoryServices.GetListCategory(),
                    collections = await _collectionServices.GetListCollection(),
                    Cartitemcount = cartitemcount
                });
            }
            else
            {
                return View(new HeaderViewModel()
                {
                    categories = await _categoryServices.GetListCategory(),
                    collections = await _collectionServices.GetListCollection(),
                    Cartitemcount = 0
                });
            }
        }
        
    }
}
