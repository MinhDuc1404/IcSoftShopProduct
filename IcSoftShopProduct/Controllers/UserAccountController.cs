using Microsoft.AspNetCore.Mvc;

namespace IcSoftShopProduct.Controllers
{
    public class UserAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
