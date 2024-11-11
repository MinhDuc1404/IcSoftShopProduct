using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace IcSoftShopAdmin.Pages.Manage
{
    [Authorize(Roles = "admin")]
    public class CustomersModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ShopUser> _shopUser;

        public CustomersModel(ApplicationDbContext applicationDbContext, UserManager<ShopUser> shopUser)
        {
            _applicationDbContext = applicationDbContext;
            _shopUser = shopUser;
        }
        public Dictionary<string, int> OrderCountsByUser { get; set; } = new Dictionary<string, int>();
        public IList<ShopUser> ShopUsers { get; set; } = new List<ShopUser>();
        public List<int> AccountCounts { get; set; }
        public List<string> Dates { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
      
        public async Task OnGetAsync()
        {

            IQueryable<ShopUser> shopUsersQuery = _applicationDbContext.ShopUsers;

            // Apply search filter if search string is provided
            if (!string.IsNullOrEmpty(SearchString))
            {
                shopUsersQuery = shopUsersQuery.Where(m => m.FirstName.Contains(SearchString) || m.LastName.Contains(SearchString));
            }

            // Retrieve the list of users
            ShopUsers = await shopUsersQuery.ToListAsync();

            // Group by creation date and calculate account counts
            var accountData = await shopUsersQuery
                .GroupBy(u => u.CreatedDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            var orderCounts = await _applicationDbContext.Orders
               .GroupBy(o => o.UserId)
               .Select(g => new { UserId = g.Key, OrderCount = g.Count() })
               .ToListAsync();


            foreach (var user in ShopUsers)
            {
                var orderCount = orderCounts.FirstOrDefault(o => o.UserId == user.Id)?.OrderCount ?? 0;  // Set 0 if no orders
                OrderCountsByUser[user.Id] = orderCount;
            }

            // Determine the range of dates for the chart
            var earliestDate = accountData.Any() ? accountData.Min(a => a.Date) : DateTime.Today;
            var latestDate = DateTime.Today;

            // Populate the Dates and AccountCounts lists
            Dates = Enumerable.Range(0, (latestDate - earliestDate).Days + 1)
                .Select(offset => earliestDate.AddDays(offset).ToString("yyyy-MM-dd"))
                .ToList();

            AccountCounts = new List<int>(new int[Dates.Count]);

            foreach (var data in accountData)
            {
                int index = Dates.IndexOf(data.Date.ToString("yyyy-MM-dd"));
                if (index >= 0)
                {
                    AccountCounts[index] = data.Count;
                }
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _applicationDbContext.ShopUsers.FindAsync(id);
            if (user != null)
            {
                _applicationDbContext.ShopUsers.Remove(user);
              await  _applicationDbContext.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
