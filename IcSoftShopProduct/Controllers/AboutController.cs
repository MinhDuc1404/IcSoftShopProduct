using Microsoft.AspNetCore.Mvc;

namespace IcSoftShopProduct.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
