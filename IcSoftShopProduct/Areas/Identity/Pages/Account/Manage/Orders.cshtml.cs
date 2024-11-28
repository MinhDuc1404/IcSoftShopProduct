using IcSoft.Infrastructure.Migrations;
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
        private readonly ApplicationDbContext _context; 

    
        public Index1Model(UserManager<ShopUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public string SearchString { get; set; }
        public List<Order> UserOrders { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public int TotalOrders { get; set; }
        public int TotalPages { get; set; }
        public async Task<IActionResult> OnGetAsync(string searchString = null, int pageNumber = 1, string status = "all")
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return NotFound("User not found.");
            }

            var query = _context.Orders
    .AsNoTracking() 
    .Where(o => o.UserId == userId);
            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                query = status switch
                {
                    "Pending..." => query.Where(o => o.status == "Pending..."),
                    "Shipping" => query.Where(o => o.status == "Shipping"),
                    "Active" => query.Where(o => o.status == "Active"),
                    "Completed" => query.Where(o => o.status == "Completed"),
                    "Cancelled" => query.Where(o => o.status == "Cancelled"),
                    _ => query
                };
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                bool isNumericSearch = int.TryParse(searchString, out int orderId);
                query = query.Where(o =>
                    o.UserId.Contains(searchString) ||
                    o.ShippingAddress.Contains(searchString) || 
                    o.PaymentMethod.Contains(searchString) || 
                    (isNumericSearch && o.Id == orderId)); 
            }


            TotalOrders = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(TotalOrders / (double)PageSize);
            PageNumber = pageNumber;

            UserOrders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return Page();
        }



        public async Task<JsonResult> OnGetOrderDetails(int id)
        {
            var order = await _context.Orders
                .Include(o => o.ShopUser)
                .Include(o => o.Coupon)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return new JsonResult(new { success = false, message = "Order not found" });
            }


            var orderDetails = new
            {
                couponName = order.Coupon?.Code,
                discount = order.Coupon?.Discount,
                customerName = order.ShopUser?.FirstName,
                customerEmail = order.ShopUser?.Email,
                customerPhone = order.ShopUser?.PhoneNumber,
                customerAddress = order.ShippingAddress,
                totalAmount = order.TotalAmount,

                orderItems = order.OrderItems.Select(item => new
                {
                    productName = item.Product.ProductName,
                    quantity = item.Quantity,
                    price = item.Price,
                    color = item.Color,
                    size = item.Size,
                }).ToList()
            };

            return new JsonResult(orderDetails);
        }



    }

}