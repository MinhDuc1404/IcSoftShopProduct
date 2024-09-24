using Microsoft.AspNetCore.Mvc;

namespace IcSoftShopProduct.Controllers
{
    public class ProductController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        { 
            return View(); 
        }
    }
}
