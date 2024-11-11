using IcSoft.Infrastructure.Models;
using IcSoftShopProduct.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using IcSoftShopProduct.Helper;

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


        [Route("/shopall/{page?}")]
        public async Task<IActionResult> Shop(int page, int pagesize = 4)
        {
            return View(await _getProductRepo.GetProductShop(page,pagesize));
        }


        [Route("{searchname}/{page:int?}")]
        public async Task<IActionResult> ShopSearch(string? searchname, int page, int pagesize = 4)
        {
            page = page > 0 ? page : 1;
            var convertedSearchName = SearchNameConverter.ConvertSearchName(searchname);

            return View(await _getProductRepo.GetProductShopSearch(convertedSearchName, page, pagesize));
        }



        [HttpGet("/Shop/filter")]
        public async Task<IActionResult> ShopFilter(string? searchname, string? priceRange, string? sortOption, int curentPage)
        {
            var products = await _getProductRepo.GetProductShopFilter(searchname, priceRange, sortOption, curentPage);

     
            // Kiểm tra nếu sản phẩm trả về null thì tạo mảng rỗng
            if (products == null)
            {
                products = new List<Product>();
            }

            return Json(new { success = true,
                products = products.Select(p => new
                {
                    productName = p.ProductName,
                    productPrice = p.ProductPrice,
                    productImage = p.ProductImage,
                    createDate = p.CreatedDate,
                    productSale = p.ProductSale
                }).ToList()
            });
        }
    }
}
