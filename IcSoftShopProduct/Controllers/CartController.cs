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
        public async Task<IActionResult> AddToCart(int productid, string productname, string color, string size, int quantity, decimal productPrice, string ProductImageUrl)
        {

            var user = await _userManager.GetUserAsync(User);

            var cartItems = await _cartRepo.GetListCartItems(user.Id);



            var existingItem = cartItems.FirstOrDefault(c => c.ProductName == productname);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = productid,
                    ProductName = productname,
                    Color = color,
                    Size = size,
                    Quantity = quantity,
                    Price = productPrice, 
                    ProductImageUrl = ProductImageUrl
                };
                cartItems.Add(newItem);
            }


            await _cartRepo.SaveCartItem(user.Id,cartItems);

            var cartTotalPrice = cartItems.Sum(x => x.TotalPrice);
            var cartquantity = cartItems.Sum(c => c.Quantity);
            return Json(new { success = true, cartTotalPrice, cartquantity });
        }

        [Route("/cart")]
        public async Task<IActionResult> CartIndex()
        {
            var user = await _userManager.GetUserAsync(User);
            var cartitems = await _cartRepo.GetListCartItems(user.Id);

            var cartviewmodel = new CartViewModel
            {
                CartItems = cartitems

            };
            return View(cartviewmodel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(string productname, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            var listcartitems = await _cartRepo.GetListCartItems(user.Id);
            var cartitem = listcartitems.FirstOrDefault(c => c.ProductName == productname);
            if (cartitem != null)
            {
                cartitem.Quantity = quantity;
               await _cartRepo.SaveCartItem(user.Id, listcartitems); 

            }

            var totalcartitem = quantity * cartitem.Price;
            var totalCartPrice = listcartitems.Sum(c => c.TotalPrice);
            var cartquantity = listcartitems.Sum(c => c.Quantity);


            return Json(new { success = true, totalCartPrice, cartquantity, totalcartitem });

      }
        [HttpPost]
        public async Task<IActionResult> RemoveItem(string productname)
        {
            var user = await _userManager.GetUserAsync(User);
            var listcartitems = await _cartRepo.GetListCartItems(user.Id);


            var cartitem = listcartitems.FirstOrDefault(c => c.ProductName == productname);
            if (cartitem != null)
            {
                listcartitems.Remove(cartitem); 
                await _cartRepo.RemoveCartItem(user.Id, productname); 
            }

            var totalCartPrice = listcartitems.Sum(c => c.TotalPrice); 
            var cartquantity = listcartitems.Sum(c => c.Quantity);

            return Json(new { success = true, totalCartPrice, cartquantity });

        }

    }
}
