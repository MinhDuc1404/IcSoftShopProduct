using IcSoft.Infrastructure.Models;
using IcSoftShopProduct.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using IcSoftShopProduct.Helper;
using IcSoftShopProduct.Models;

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


        [Route("/shopall")]

        public async Task<IActionResult> Shop()
        {
            return View(await _getProductRepo.GetProductShop(1,6));
        }

        [HttpGet("/shopall/paging")]

        public async Task<IActionResult> ShopallPaging(int page = 1)
        {
            var shopviewmodel = await _getProductRepo.GetProductShop(page, 6);

            return Json(new
            {
                success = true,
                products = shopviewmodel.Products.Select(p => new
                {
                    productName = p.ProductName,
                    productPrice = p.ProductPrice,
                    productImage = p.ProductImage,
                    createDate = p.CreatedDate,
                    productSale = p.ProductSale
                }).ToList(),
                totalPages = shopviewmodel.TotalPages,
                currentPage = page
            });

        }

        [Route("/Sale")]
        public async Task<IActionResult> ShopSale()
        {
            return View(await _getProductRepo.GetProductShopSale(1, 6));
        }

        [HttpGet("/Sale/paging")]

        public async Task<IActionResult> ShopSalePaging(int page = 1)
        {
            var shopviewmodel = await _getProductRepo.GetProductShopSale(page, 6);

            return Json(new
            {
                success = true,
                products = shopviewmodel.Products.Select(p => new
                {
                    productName = p.ProductName,
                    productPrice = p.ProductPrice,
                    productImage = p.ProductImage,
                    createDate = p.CreatedDate,
                    productSale = p.ProductSale
                }).ToList(),
                totalPages = shopviewmodel.TotalPages,
                currentPage = page
            });

        }


        [Route("/shop/{searchname}")]
        public async Task<IActionResult> ShopSearch(string? searchname, int page=1, int pagesize = 6)
        {
            var convertedSearchName = SearchNameConverter.ConvertSearchName(searchname);

            return View(await _getProductRepo.GetProductShopSearch(convertedSearchName, page, pagesize));
        }



        [HttpGet("/Shop/filter")]
        public async Task<IActionResult> ShopFilter(string? priceRange, string? sortOption, int pageNumber = 1)
        {
            var productsviewmodel = await _getProductRepo.GetProductShopFilter(priceRange, sortOption, pageNumber);

            if (productsviewmodel == null)
            {
                productsviewmodel = new ProductShopFilterViewModel();
            }

            return Json(new { success = true,
                products = productsviewmodel.Products.Select(p => new
                {
                    productName = p.ProductName,
                    productPrice = p.ProductPrice,
                    productImage = p.ProductImage,
                    createDate = p.CreatedDate,
                    productSale = p.ProductSale
                }).ToList(),
                totalPages = productsviewmodel.TotalPages,
                currentPage = pageNumber
            });
        }

        [HttpGet("/ShopSale/filter")]
        public async Task<IActionResult> ShopSaleFilter(string? priceRange, string? sortOption, int pageNumber = 1)
        {
            var productsviewmodel = await _getProductRepo.GetProductShopSaleFilter(priceRange, sortOption, pageNumber);

            if (productsviewmodel == null)
            {
                productsviewmodel = new ProductShopFilterViewModel();
            }
          

            return Json(new
            {
                success = true,
                products = productsviewmodel.Products.Select(p => new
                {
                    productName = p.ProductName,
                    productPrice = p.ProductPrice,
                    productImage = p.ProductImage,
                    createDate = p.CreatedDate,
                    productSale = p.ProductSale
                }).ToList(),
                totalPages = productsviewmodel.TotalPages,
                currentPage = pageNumber
            });
        }

        [HttpGet("/Product/Search")]
        public async Task<IActionResult> SearchProduct(string query)
        {

            var products = await _getProductRepo.GetProductSearchQuery(query);

 

            return Json(new {success = true, products});
        }
    }
}
