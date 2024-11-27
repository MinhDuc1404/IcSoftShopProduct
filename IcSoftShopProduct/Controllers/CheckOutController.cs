using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using IcSoftShopProduct.Services.Interface;
using System.Drawing;
using IcSoft.Infrastructure.Migrations;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace IcSoftShopProduct.Controllers
{
    [Authorize]
    [Route("CheckOut")]
    public class CheckOutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGetCartRepo _getCartRepo;
        public CheckOutController(ApplicationDbContext context, IGetCartRepo getCartRepo)
        {
            _context = context;
            _getCartRepo = getCartRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            var cartItems = _getCartRepo.GetListCartItems(userId);

            if (cartItems == null || !cartItems.Any())
            {
                return View("~/Views/Pages/CheckOut.cshtml", null); 
            }
            return View("~/Views/Pages/CheckOut.cshtml", cartItems);
        }

        [HttpGet("ItemIndex")]
        public async Task<IActionResult> ItemIndex()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
         
            var cartItemsJson = HttpContext.Session.GetString("CartItems");
            var cartItems = JsonConvert.DeserializeObject<CartItem>(cartItemsJson);
           
            return View("~/Views/Pages/CheckOut.cshtml", cartItems);
        }
        [HttpPost("CheckoutItem")]
        public async Task<IActionResult> CheckoutItem(int productid, string productname, string color, string size, int quantity, decimal productPrice, string ProductImageUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            var cartItems = new CartItem();
            cartItems.ProductId = productid;
            cartItems.ProductName = productname;
            cartItems.Color = color;
            cartItems.Size = size;
            cartItems.Quantity = quantity;
            cartItems.Price = productPrice;
            cartItems.ProductImageUrl = ProductImageUrl;

            // Lưu cartItems vào Session trong action Item
            HttpContext.Session.SetString("CartItems", JsonConvert.SerializeObject(cartItems));
            return Json(new { success = true, redirectUrl = Url.Action("ItemIndex", "CheckOut") });
        }
        [HttpPost("")]
        public async Task<IActionResult> Create(Order order, bool isSingleProduct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                ModelState.AddModelError("", "You must be logged in to place an order.");
                return View("/Views/Pages/CheckOut.cshtml", order);
            }
            order.UserId = userId;
            order.CreatedAt = DateTime.Now;
            order.status = "Pending...";
            var couponId = HttpContext.Session.GetInt32("CouponId");
            if (couponId.HasValue)
            {
                order.CouponId = couponId.Value;
            }
            List<CartItem> cartItems;

            if (isSingleProduct)
            {
                // Lấy cartItem từ Session (một sản phẩm duy nhất)
                var cartItemJson = HttpContext.Session.GetString("CartItems");
                var singleCartItem = JsonConvert.DeserializeObject<CartItem>(cartItemJson);

                if (singleCartItem == null)
                {
                    ModelState.AddModelError("", "No item found to checkout.");
                    return View("/Views/Pages/CheckOut.cshtml", order);
                }

                // Chuyển singleCartItem thành danh sách có một sản phẩm
                cartItems = new List<CartItem> { singleCartItem };
            }
            else
            {
                // Lấy toàn bộ giỏ hàng từ cookies
                cartItems = _getCartRepo.GetListCartItems(userId);

                if (cartItems == null || !cartItems.Any())
                {
                    ModelState.AddModelError("", "Your cart is empty.");
                    return View("/Views/Pages/CheckOut.cshtml", order);
                }
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price,
                    Color = cartItem.Color,
                    Size = cartItem.Size,
                };
                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();
            }

            _getCartRepo.ClearCart(userId);
            HttpContext.Session.Remove("CouponId");
            return RedirectToAction("Index", "OrderItems", new { id = order.Id });
        }
        [HttpPost("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon([FromBody] string coupon)
        {
            var validCoupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == coupon && c.ValidUntil > DateTime.Now);
            decimal totalAmount = CalculateTotalAmount();
            decimal discountedTotal = totalAmount;
            string message;

            if (validCoupon != null && validCoupon.UsageLimit > 0)
            {
                decimal discount = validCoupon.Discount;
                decimal discountAmount = (discount / 100) * totalAmount;
                discountedTotal = totalAmount - discountAmount;
                HttpContext.Session.SetInt32("CouponId", validCoupon.Id);
                message = $"Coupon '{coupon}' applied successfully! You saved {discountAmount:N0}₫ ({discount}% off).";
                validCoupon.UsageLimit -= 1;
                await _context.SaveChangesAsync();
            }
            else
            {
                message = $"Coupon '{coupon}' is invalid or expired.";
            }

            return Json(new { discountedTotal = discountedTotal, message = message });
        }

        [HttpGet("GetTotalAmount")]
        public async Task<IActionResult> GetTotalAmount()
        {
            decimal totalAmount = CalculateTotalAmount(); 
            return Json(new { totalAmount = totalAmount });
        }

        private decimal CalculateTotalAmount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _getCartRepo.GetListCartItems(userId);

            decimal total = cartItems.Sum(item => item.Price * item.Quantity);
            return total;
        }

        [HttpGet("GetUserProfileAndCart")]
        public async Task<IActionResult> GetUserProfileAndCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            // Ensure the 'await' keyword is used here
            var user = await _context.ShopUsers.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }
           

            return Json(new { success = true, user = user });
        }

    }
}
