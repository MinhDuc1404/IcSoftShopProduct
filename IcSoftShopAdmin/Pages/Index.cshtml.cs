using IcSoft.Infrastructure.Migrations;
using IcSoft.Infrastructure.Models;
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
    public string SearchString {  get; set; }
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

        // Prepare base query for orders
        var orderQuery = _applicationDbContext.Orders.AsQueryable();

        // Apply search filter if search string is provided
        if (!string.IsNullOrEmpty(SearchString))
        {
            // Try to parse the SearchString into an integer (for matching Id)
            bool isNumericSearch = int.TryParse(SearchString, out int orderId);

            orderQuery = orderQuery.Where(o =>
                o.UserId.Contains(SearchString) ||                  
                o.ShippingAddress.Contains(SearchString) ||        
                o.PaymentMethod.Contains(SearchString) ||           
                (isNumericSearch && o.Id == orderId)                 
            );
        }

        // Get total number of filtered orders
        TotalOrders = await orderQuery.CountAsync();

        // Calculate total pages based on filtered orders
        TotalPages = (int)Math.Ceiling(TotalOrders / (double)PageSize);

        // Retrieve the filtered, paginated orders
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

        Dates = Enumerable.Range(0, (latestDate - earliestDate).Days + 1)
            .Select(offset => earliestDate.AddDays(offset).ToString("yyyy-MM-dd"))
            .ToList();

        OrderCounts = new List<int>(new int[Dates.Count]);

        foreach (var data in orderData)
        {
            int index = Dates.IndexOf(data.Date.ToString("yyyy-MM-dd"));
            if (index >= 0)
            {
                OrderCounts[index] = data.Count;
            }
        }
        TotalSales = await orderQuery.SumAsync(o => o.TotalAmount);

    }


    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var order = await _applicationDbContext.Orders.FindAsync(id);

        if(order != null)
        {
            _applicationDbContext.Orders.Remove(order);
            await _applicationDbContext.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(int id, string status)
    {
        if (id == 0 || string.IsNullOrEmpty(status))
        {
            return NotFound();
        }

        // Find the order by ID
        var order = await _applicationDbContext.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        // Update the order status
        order.status = status;

        // Save the changes
        await _applicationDbContext.SaveChangesAsync();

        return RedirectToPage();
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

        // Generate all dates between the range
        var allDates = Enumerable.Range(0, (end.Date - start.Date).Days + 1)
            .Select(offset => start.AddDays(offset).ToString("yyyy-MM-dd"))
            .ToList();

        // Initialize counts for all dates
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


    public async Task<JsonResult> OnGetOrderDetails(int id)
    {
        // Fetch the order details by ID
        var order = await _applicationDbContext.Orders
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

