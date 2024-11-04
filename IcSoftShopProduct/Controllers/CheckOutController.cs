using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using IcSoftShopProduct.Services.Interface;

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

        [HttpGet("")]
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
                return View("~/Views/Pages/CheckOut.cshtml", null); // Pass null to view to indicate no items.
            }
            return View("~/Views/Pages/CheckOut.cshtml", cartItems);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(Order order)
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

            var cartItems = _getCartRepo.GetListCartItems(userId);

            if (cartItems == null || !cartItems.Any())
            {
                ModelState.AddModelError("", "Your cart is empty.");
                return View("/Views/Pages/CheckOut.cshtml", order);
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
                    Price = cartItem.Price
                };
                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();
            }

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
            decimal totalAmount = CalculateTotalAmount(); // This would fetch the current total amount from the cart
            return Json(new { totalAmount = totalAmount });
        }

        private decimal CalculateTotalAmount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _getCartRepo.GetListCartItems(userId);

            decimal total = cartItems.Sum(item => item.Price * item.Quantity);
            return total;
        }
    }
}
