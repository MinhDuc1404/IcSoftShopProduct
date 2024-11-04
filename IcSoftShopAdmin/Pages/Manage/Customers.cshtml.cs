using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IcSoftShopAdmin.Pages.Manage
{
    public class CustomersModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ShopUser> _shopUser;

        public CustomersModel(ApplicationDbContext applicationDbContext, UserManager<ShopUser> shopUser)
        {
            _applicationDbContext = applicationDbContext;
            _shopUser = shopUser;
        }

        public IList<ShopUser> ShopUsers { get; set; } = new List<ShopUser>();
        public List<int> AccountCounts { get; set; }
        public List<string> Dates { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
      
        public async Task OnGetAsync()
        {
            IQueryable<ShopUser> shopUsersQuery = _applicationDbContext.ShopUsers;

            if (!string.IsNullOrEmpty(SearchString))
            {
                shopUsersQuery = shopUsersQuery.Where(m => m.FirstName.Contains(SearchString) || m.LastName.Contains(SearchString));
            }

           // var accountData = await shopUsersQuery
           // .GroupBy(o => o.CreatedDate.Date)
           // .OrderBy(g => g.Key)
           // .Select(g => new { Date = g.Key, Count = g.Count() })
           // .ToListAsync();

           // var earliestDate = accountData.Any() ? accountData.Min(a => a.Date) : DateTime.Today;
           // var latestDate = DateTime.Today;
           // Dates = Enumerable.Range(0, (latestDate - earliestDate).Days + 1)
           //.Select(offset => earliestDate.AddDays(offset).ToString("yyyy-MM-dd"))
           //.ToList();
           // foreach (var data in accountData)
           // {
           //     int index = Dates.IndexOf(data.Date.ToString("yyyy-MM-dd"));
           //     if (index >= 0)
           //     {
           //         AccountCounts[index] = data.Count;
           //     }
           // }
            ShopUsers = await shopUsersQuery.ToListAsync();
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
