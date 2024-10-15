using IcSoftShopProduct.Models;
using IcSoftShopProduct.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IcSoftShopProduct.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetHomeRepo _Homerepo;


        public HomeController(ILogger<HomeController> logger, IGetHomeRepo homeRepo)
        {
            _logger = logger;
            _Homerepo = homeRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _Homerepo.GetHomeIndex());
        }

        public IActionResult Privacy()
        {
            return View();
        }
       
     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
