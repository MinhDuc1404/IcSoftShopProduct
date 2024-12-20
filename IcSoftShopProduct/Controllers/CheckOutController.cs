using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization; 
using IcSoftShopProduct.Services.Interface;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using IcSoftShopProduct.Models;

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
            var cartItems = await _getCartRepo.GetListCartItems(userId);

            var totalprice = cartItems.Sum(x => x.TotalPrice);

            var model = new CheckOutViewModel
            {
                CartItems = cartItems,
                TotalPrice = totalprice
            };
          

            if (cartItems == null || !cartItems.Any())
            {
                return View("~/Views/Pages/CheckOut.cshtml", null); 
            }
            return View("~/Views/Pages/CheckOut.cshtml", model);
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

            var model = new CheckOutViewModel
            {
                SingleItem = cartItems,
                TotalPrice = cartItems.TotalPrice
            };
           
            return View("~/Views/Pages/CheckOut.cshtml", model);
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


            HttpContext.Session.SetString("CartItems", JsonConvert.SerializeObject(cartItems));
            return Json(new { success = true, redirectUrl = Url.Action("ItemIndex", "CheckOut") });

        }
        [HttpPost("")]
        public async Task<IActionResult> Create(Order order, bool isSingleProduct, bool transferChecking = false)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                ModelState.AddModelError("", "You must be logged in to place an order.");
                return View("/Views/Pages/CheckOut.cshtml", order);
            }
            order.UserId = userId;
            order.CreatedAt = DateTime.Now;
            if (transferChecking)
            {
                order.status = "Active";
            }
            else {
                order.status = "Pending...";
            }
           
            var couponId = HttpContext.Session.GetInt32("CouponId");
            if (couponId.HasValue)
            {
                order.CouponId = couponId.Value;
            }
            List<CartItem> cartItems;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            if (isSingleProduct)
            {

                var cartItemJson = HttpContext.Session.GetString("CartItems");
                var singleCartItem = JsonConvert.DeserializeObject<CartItem>(cartItemJson);

                if (singleCartItem == null)
                {
                    ModelState.AddModelError("", "No item found to checkout.");
                    return View("/Views/Pages/CheckOut.cshtml", order);
                }


                cartItems = new List<CartItem> { singleCartItem };
            }
            else
            {

                cartItems = await _getCartRepo.GetListCartItems(userId);

                if (cartItems == null || !cartItems.Any())
                {
                    ModelState.AddModelError("", "Your cart is empty.");
                    return View("/Views/Pages/CheckOut.cshtml", order);
                }

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
                await _getCartRepo.ClearCart(userId);
            }
            HttpContext.Session.Remove("CouponId");
            return RedirectToAction("Index", "OrderItems", new { id = order.Id });
        }
        [HttpPost("TransferChecking")]
        public async Task<IActionResult> TransferChecking(decimal amount, string bankingMessage)
        {
            try
            {
                decimal totalAmount = await CalculateTotalAmount();

                if (amount != totalAmount)
                {
                    return Json(new { success = false, message = "Không đúng số tiền" });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }

                var latestOrder = await _context.Orders
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.CreatedAt)
                    .FirstOrDefaultAsync();

                if (latestOrder == null)
                {
                    return Json(new { success = false, message = "Mã đơn hàng không đúng" });
                }

                string orderCode = "DH" + latestOrder.Id;
                if (!bankingMessage.Contains(orderCode))
                {
                    return Json(new { success = false, message = "Order code mismatch in banking message." });
                }

                return Json(new { success = true, totalamount = totalAmount,  message = "Transfer successful!" });
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { success = false, message = "Internal Server Error." });
            }
        }


        [HttpPost("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon([FromBody] string coupon)
        {
            // Await the async method to get the actual decimal value
            decimal totalAmount = await CalculateTotalAmount();
            decimal discountedTotal = totalAmount;
            string message;

            var validCoupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == coupon && c.ValidUntil > DateTime.Now);

            if (validCoupon != null && validCoupon.UsageLimit > 0)
            {
                decimal discount = validCoupon.Discount;
                decimal discountAmount = (discount / 100) * totalAmount;
                discountedTotal = totalAmount - discountAmount;
                HttpContext.Session.SetInt32("CouponId", validCoupon.Id);
                message = $"Coupon '{coupon}' applied successfully! You saved {discountAmount:N0}₫ ({discount}% off).";

                // Reduce usage limit and save changes
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
            decimal totalAmount = await CalculateTotalAmount(); 
            return Json(new { totalAmount = totalAmount });
        }

        private async Task<decimal> CalculateTotalAmount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _getCartRepo.GetListCartItems(userId);

            decimal total = 0;

            if (cartItems != null && cartItems.Any())
            {
            
                total = cartItems.Sum(item => item.TotalPrice);
            }
            else
            {
           
                var cartItemJson = HttpContext.Session.GetString("CartItems");
                if (!string.IsNullOrEmpty(cartItemJson))
                {
                    var singleCartItem = JsonConvert.DeserializeObject<CartItem>(cartItemJson);
                    if (singleCartItem != null)
                    {
                        total = singleCartItem.Price * singleCartItem.Quantity;
                    }
                }
            }

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

            
            var user = await _context.ShopUsers.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }
           

            return Json(new { success = true, user = user });
        }

      

    }
}
