using IcSoftShopProduct.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IcSoftShopProduct.Controllers
{
    public class ProductController : Controller
    {
        private readonly IGetProductRepo _getProductRepo;
        public ProductController(IGetProductRepo productRepo)
        {
            _getProductRepo = productRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("/{name}")]
        public async Task<IActionResult> Details(string name)
        { 
            return View(await _getProductRepo.GetProductDetials(name.Replace("-"," "))); 
        }
        [Route("/shop.html")]
        public async Task<IActionResult> Shop()
        {
            return View(await _getProductRepo.GetProductShop());
        }
    }
}
