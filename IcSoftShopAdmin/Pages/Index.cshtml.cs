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

    public string MinDate { get; set; }
    public string MaxDate { get; set; }
    public IList<Order> Orders { get; set; } = new List<Order>();
    public IList<ShopUser> ShopUsers { get; set; } = new List<ShopUser>();

    // Properties to hold the selected start and end dates

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
                o.UserId.Contains(SearchString) ||                  // Search by UserId
                o.ShippingAddress.Contains(SearchString) ||          // Search by Shipping Address
                o.PaymentMethod.Contains(SearchString) ||            // Search by Payment Method
                (isNumericSearch && o.Id == orderId)                 // If numeric, search by Id
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

}
