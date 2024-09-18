using Microsoft.AspNetCore.Mvc;

namespace IcSoftShopProduct.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
            { 
            return View(); 
            }
    }
}
