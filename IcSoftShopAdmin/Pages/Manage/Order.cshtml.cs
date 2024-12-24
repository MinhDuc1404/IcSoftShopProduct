using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IcSoftShopAdmin.Pages.Manage
{
    [Authorize(Roles = "admin,manager")]
    public class OrderModel : PageModel
    {
        public readonly ApplicationDbContext _applicationDbContext;

        public OrderModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IList<Order> Orders { get; set; } = new List<Order>();
        public IList<ShopUser> ShopUsers { get; set; } = new List<ShopUser>();

        // Properties to hold the selected start and end dates
        public decimal TotalSales { get; set; }
        public string SearchString { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public int TotalOrders { get; set; }
        public int TotalPages { get; set; }
        public List<string> Dates { get; set; }
        public List<int> OrderCounts { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, string searchString = null, string status = "all")
        {


            ShopUsers = await _applicationDbContext.ShopUsers.ToListAsync();

            SearchString = searchString;


            var orderQuery = _applicationDbContext.Orders.AsQueryable();


            if (!string.IsNullOrEmpty(SearchString))
            {

                bool isNumericSearch = int.TryParse(SearchString, out int orderId);

                orderQuery = orderQuery.Where(o =>
                    o.UserId.Contains(SearchString) ||
                    o.ShippingAddress.Contains(SearchString) ||
                    o.PaymentMethod.Contains(SearchString) ||
                    (isNumericSearch && o.Id == orderId)
                );
            }


            TotalOrders = await orderQuery.CountAsync();

            TotalPages = (int)Math.Ceiling(TotalOrders / (double)PageSize);

            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                orderQuery = status switch
                {
                    "Pending..." => orderQuery.Where(o => o.status == "Pending..."),
                    "Shipping" => orderQuery.Where(o => o.status == "Shipping"),
                    "Active" => orderQuery.Where(o => o.status == "Active"),
                    "Completed" => orderQuery.Where(o => o.status == "Completed"),
                    "Cancelled" => orderQuery.Where(o => o.status == "Cancelled"),
                    _ => orderQuery
                };
            }

            Orders = await orderQuery
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            PageNumber = pageNumber;

            var orderData = await orderQuery
                .GroupBy(o => o.CreatedAt.Date)
                .OrderBy(g => g.Key)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

        }
        
        public async Task<IActionResult> OnGetDeleteAsync(int? id)
        {
            if (id == null)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy đơn hàng." });
            }

            var order = await _applicationDbContext.Orders.FindAsync(id);

            if (order != null)
            {
                _applicationDbContext.Orders.Remove(order);
                await _applicationDbContext.SaveChangesAsync();
                return new JsonResult(new { success = true });
            }
            return new JsonResult(new { success = false, message = "Xóa đơn thất bại." });
        }

        public async Task<JsonResult> OnGetUpdateStatusAsync(int id, string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return new JsonResult(new { success = false, message = "Đơn hàng không tồn tại." });
            }


            var order = await _applicationDbContext.Orders.FindAsync(id);

            if (order == null)
            {
                return new JsonResult(new { success = false, message = "Đơn hàng không tồn tại." });
            }
            order.status = status;
            _applicationDbContext.Orders.Update(order);

            var update = await _applicationDbContext.SaveChangesAsync();

            return new JsonResult(new { success = true, status = status });
        }

        public async Task<JsonResult> OnGetOrderDetails(int id)
        {
            var order = await _applicationDbContext.Orders
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
                discount = order.Coupon?.Discount,
                couponName = order.Coupon?.Code,
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
