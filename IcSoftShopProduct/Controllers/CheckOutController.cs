using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;

namespace IcSoftShopProduct.Controllers
{
    [Authorize]
    [Route("CheckOut")]
    public class CheckOutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckOutController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View("~/Views/Pages/CheckOut.cshtml");
        }

     
       
      
        public async Task<IActionResult> Create(Order order)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                // Handle case where user is not logged in
                ModelState.AddModelError("", "You must be logged in to place an order.");
                return View("/Views/Pages/CheckOut.cshtml", order);
            }

            order.UserId = userId;
            
            order.CreatedAt = DateTime.Now;
            order.status = "Pending...";
            // Add the order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Redirect to OrderItems view, passing the newly created order ID
            return RedirectToAction("Index", "OrderItems");
        }

   
    [HttpPost("ApplyCoupon")]
    public async Task<IActionResult> ApplyCoupon([FromBody] string coupon)
        {
            var validCoupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == coupon && c.ValidUntil > DateTime.Now);
            decimal totalAmount = CalculateTotalAmount(); // Calculate total before applying the coupon
            decimal discountedTotal = totalAmount;
            string message;

            if (validCoupon != null)
            {
                decimal discount = validCoupon.Discount;
                decimal discountAmount = (discount / 100) * totalAmount;
                discountedTotal = totalAmount - discountAmount;
                message = $"Coupon '{coupon}' applied successfully! You saved {discountAmount:N0}₫ ({discount}% off).";
            }
            else
            {
                message = $"Coupon '{coupon}' is invalid or expired.";
            }

            // Return JSON response with the discounted total and message
            return Json(new { discountedTotal = discountedTotal, message = message });
        }

        private decimal CalculateTotalAmount()
        {
            return 2060000;
        }
    }
}