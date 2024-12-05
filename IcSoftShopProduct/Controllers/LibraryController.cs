using Microsoft.AspNetCore.Mvc;

namespace IcSoftShopProduct.Controllers
{
    public class LibraryController : Controller
    {
        [Route("thu-vien")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
