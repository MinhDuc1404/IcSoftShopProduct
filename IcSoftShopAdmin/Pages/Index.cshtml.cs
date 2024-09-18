using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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
    public string StartDate { get; set; }
    public string EndDate { get; set; }

    public async Task OnGetAsync()
    {
        // Set the minimum date as January 8, 2024
        MinDate = new DateTime(2024, 1, 8).ToString("yyyy-MM-dd");

        // Set the maximum date as the current date
        MaxDate = DateTime.Now.ToString("yyyy-MM-dd");

        // Get the list of ShopUsers from the database
        ShopUsers = await _applicationDbContext.ShopUsers.ToListAsync();
    }

    public void OnPost(string startDate, string endDate)
    {
        // Assign the submitted values to the model properties
        StartDate = startDate;
        EndDate = endDate;

        // Handle additional logic here, e.g., validation, processing
    }
}
