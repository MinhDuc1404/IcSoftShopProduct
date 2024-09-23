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
