using IcSoft.Infrastructure.Migrations;
using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;


public class IndexModel : PageModel
{
    public readonly ApplicationDbContext _applicationDbContext;

    public IndexModel(ApplicationDbContext applicationDbContext)
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


    public async Task OnGetAsync(int pageNumber = 1, string searchString = null)
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

        var earliestDate = orderData.Any() ? orderData.Min(a => a.Date) : DateTime.Today;
        var latestDate = DateTime.Today;

        TotalSales = await orderQuery.SumAsync(o => o.TotalAmount);

    }


    public async Task<IActionResult> OnPostDeleteAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var order = await _applicationDbContext.Orders.FindAsync(id);

        if (order != null)
        {
            _applicationDbContext.Orders.Remove(order);
            await _applicationDbContext.SaveChangesAsync();
        }
        return RedirectToPage();
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

    public async Task<JsonResult> OnGetFilterDataAsync(string startDate, string endDate)
    {
        DateTime start = DateTime.Parse(startDate);
        DateTime end = DateTime.Parse(endDate);

        var filteredOrders = await _applicationDbContext.Orders
            .Where(o => o.CreatedAt.Date >= start.Date && o.CreatedAt.Date <= end.Date)
            .GroupBy(o => o.CreatedAt.Date)
            .OrderBy(g => g.Key)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync();


        var allDates = Enumerable.Range(0, (end.Date - start.Date).Days + 1)
            .Select(offset => start.AddDays(offset).ToString("yyyy-MM-dd"))
            .ToList();


        var counts = new List<int>(new int[allDates.Count]);

        foreach (var order in filteredOrders)
        {
            int index = allDates.IndexOf(order.Date.ToString("yyyy-MM-dd"));
            if (index >= 0)
            {
                counts[index] = order.Count;
            }
        }

        return new JsonResult(new { dates = allDates, counts = counts });
    }
    public async Task<JsonResult> OnGetTopSellingProductsAsync(string period)
    {
        DateTime startDate;
        DateTime endDate = DateTime.Now;
        switch (period.ToLower())
        {
            case "daily":
                startDate = endDate.Date; 
                break;
            case "monthly":
                startDate = new DateTime(endDate.Year, endDate.Month, 1); 
                break;
            case "yearly":
                startDate = new DateTime(endDate.Year, 1, 1); 
                break;
            default:
                return new JsonResult(new { success = false, message = "Invalid period. Use 'daily', 'monthly', or 'yearly'." });
        }
        var topProducts = await _applicationDbContext.OrderItems
          .Where(oi => oi.Order.CreatedAt >= startDate && oi.Order.CreatedAt <= endDate)
          .GroupBy(oi => oi.ProductId)
          .Select(g => new
          {
              ProductId = g.Key,
              ProductName = g.First().Product.ProductName,
              TotalSales = g.Sum(oi => oi.Quantity * oi.Price),
              CategoryName = g.First().Product.Category.CategoryName, 
              CollectionName = g.First().Product.Collection.CollectionName,
              ProductImageUrl = g.First().Product.ProductImage.FirstOrDefault().ImageUrl ?? "default-image.jpg"
          })
          .OrderByDescending(p => p.TotalSales)  
          .Take(4) 
          .ToListAsync();

        return new JsonResult(new { success = true, products = topProducts });
    }
    public async Task<JsonResult> OnGetMonthlySaleAsync()
    {
        var currentDate = DateTime.Now;
        var fourMonthsAgo = currentDate.AddMonths(-3);

        var lastFourMonths = Enumerable.Range(0, 4)
       .Select(i => fourMonthsAgo.AddMonths(i))
       .ToList();

        var salesData = await _applicationDbContext.Orders
       .Where(o => o.CreatedAt >= fourMonthsAgo)
       .GroupBy(o => new { Month = o.CreatedAt.Month, Year = o.CreatedAt.Year })
       .Select(g => new
       {
           Year = g.Key.Year,
           Month = g.Key.Month,
           TotalSales = g.Sum(o => o.TotalAmount)
       })
       .ToListAsync();


        var monthlySales = lastFourMonths
         .GroupJoin(salesData,
                    month => new { Month = month.Month, Year = month.Year },
                    sales => new { sales.Month, sales.Year },
                    (month, salesGroup) => new
                    {
                        Year = month.Year,
                        Month = month.Month,
                        TotalSales = salesGroup.FirstOrDefault()?.TotalSales ?? 0
                    })
         .ToList();

        var salesResponse = monthlySales.Select(ms => new
        {
            Date = $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(ms.Month)}",
            TotalSales = ms.TotalSales
        }).ToList();
        var lastMonth = monthlySales[monthlySales.Count - 2];
        var currentMonth = monthlySales[monthlySales.Count - 1];

        if (lastMonth.TotalSales > 0)
        {
            var percentageChange = ((currentMonth.TotalSales - lastMonth.TotalSales) / lastMonth.TotalSales) * 100;
        }
        else
        {
            Console.WriteLine("Không thể tính phần trăm thay đổi vì tháng trước không có doanh thu.");
        }
        return new JsonResult(new { sales = salesResponse });
    }

    public async Task<JsonResult> OnGetTransactionAsync()
    {
        var salesByTransactionType = await _applicationDbContext.Orders
       .Where(o => o.PaymentMethod == "Chuyển Khoản" || o.PaymentMethod == "Thu Hộ (COD)") 
       .GroupBy(o => o.PaymentMethod)
       .Select(g => new
       {
           PaymentMethod = g.Key,
           TotalSales = g.Sum(o => o.TotalAmount)
       })
       .ToListAsync();

        return new JsonResult(new { salesByTransactionType });
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

