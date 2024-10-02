using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using IcSoftShopProduct.Models;
using IcSoftShopProduct.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IcSoftShopProduct.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<ShopUser> _userManager;
        private readonly IGetCartRepo _cartRepo;


        public CartController(IGetCartRepo getCartRepo, UserManager<ShopUser> userManager)
        {
            _userManager = userManager;
            _cartRepo = getCartRepo;
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(string productname, string color, string size, int quantity, decimal productPrice, string ProductImageUrl)
        {

            var user = await _userManager.GetUserAsync(User);
            // Lấy giỏ hàng từ session
            var cartItems = _cartRepo.GetCartItems(user.Id);

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var existingItem = cartItems.FirstOrDefault();
            if (existingItem != null)
            {
                // Nếu sản phẩm đã có trong giỏ hàng, tăng số lượng
                existingItem.Quantity += quantity;
            }
            else
            {
                // Thêm sản phẩm mới vào giỏ hàng
                var newItem = new CartItemViewModel
                {
                    ProductName = productname,
                    Color = color,
                    Size = size,
                    Quantity = quantity,
                    Price = productPrice, 
                    ProductImageUrl = ProductImageUrl
                };
                cartItems.Add(newItem);
            }

            // Lưu giỏ hàng vào session
            _cartRepo.SaveCartCookie(user.Id,cartItems);

            // Trả về dữ liệu giỏ hàng mới cho client
            var cartTotalPrice = cartItems.Sum(x => x.TotalPrice);
            return Json(new { success = true, cartTotalPrice });
        }
        public async Task<IActionResult> CartIndex()
        {
            var user = await _userManager.GetUserAsync(User);
            var cartitems = _cartRepo.GetCartItems(user.Id);

            var cartviewmodel = new CartViewModel
            {
                CartItems = cartitems

            };
            return View(cartviewmodel);
        }


    }
}
