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
            var listcategory = await _categoryServices.GetListCategory();
            var listcollection = await _collectionServices.GetListCollection();
            if (user != null)
           { 
            var listcartitem = await _getCartRepo.GetListCartItems(user.Id);
 
            
                var cartitemcount = listcartitem.Sum(c => c.Quantity);

                return View(new HeaderViewModel()
                {
                    categories = listcategory,
                    collections = listcollection,
                    Cartitemcount = cartitemcount
                });
            }
            else
            {
                return View(new HeaderViewModel()
                {
                    categories = listcategory,
                    collections = listcollection,
                    Cartitemcount = 0
                });
            }
        }
        
    }
}
