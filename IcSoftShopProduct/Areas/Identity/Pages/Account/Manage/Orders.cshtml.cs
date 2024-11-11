using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IcSoftShopProduct.Areas.Identity.Pages.Account.Manage
{

    public class Index1Model : PageModel
    {
        private readonly UserManager<ShopUser> _userManager;
        private readonly ApplicationDbContext _context; // Assuming your DbContext is called ApplicationDbContext

        // Constructor for Dependency Injection
        public Index1Model(UserManager<ShopUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public List<Order> UserOrders { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            // Get the logged-in user's ID
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return NotFound("User not found.");
            }

            // Fetch the orders associated with the logged-in user
            UserOrders = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Page();
        }

        public async Task<JsonResult> OnGetOrderDetails(int id)
        {
            // Fetch the order details by ID
            var order = await _context.Orders
                .Include(o => o.ShopUser) // Include the customer information (ShopUser)
                .Include(o => o.OrderItems) // Include the order items
                .ThenInclude(oi => oi.Product) // Include product information for each item
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return new JsonResult(new { success = false, message = "Order not found" });
            }

            // Prepare the response data
            var orderDetails = new
            {
                customerName = order.ShopUser?.FirstName,
                customerEmail = order.ShopUser?.Email,
                customerPhone = order.ShopUser?.PhoneNumber,
                customerAddress = order.ShippingAddress,
                totalAmount = order.TotalAmount,
                orderItems = order.OrderItems.Select(item => new
                {
                    productName = item.Product.ProductName,
                    quantity = item.Quantity,
                    price = item.Price
                }).ToList()
            };

            return new JsonResult(orderDetails);
        }

    }

}