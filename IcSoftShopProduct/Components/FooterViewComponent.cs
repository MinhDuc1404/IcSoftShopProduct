using Microsoft.AspNetCore.Mvc;

namespace IcSoftShopProduct.Components
{
    public class FooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
            { 
            return View(); 
            }
    }
}
