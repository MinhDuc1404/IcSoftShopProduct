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
        [BindProperty]
        public ShopUser ShopUser { get; set; } = default!;
        public List<int> AccountCounts { get; set; }
        public List<string> Dates { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public string? SearchStringOrder { get; set; }
        public IList<Order> UserOrders { get; set; } = new List<Order>();
        public async Task OnGetAsync()
        {

            IQueryable<ShopUser> shopUsersQuery = _applicationDbContext.ShopUsers;

           
            if (!string.IsNullOrEmpty(SearchString))
            {
                shopUsersQuery = shopUsersQuery.Where(m => m.FirstName.Contains(SearchString) || m.LastName.Contains(SearchString) || m.UserName.Contains(SearchString));
            }

            ShopUsers = await shopUsersQuery.ToListAsync();

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

            UserOrders = await _applicationDbContext.Orders
                .Where(o => o.UserId == id)
                .Include(o => o.OrderItems)  
                .ToListAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();


                var errorMessage = string.Join("; ", errors);


                throw new InvalidOperationException($"Model validation failed: {errorMessage}");
            }
            var user = await _applicationDbContext.ShopUsers.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
                return NotFound();

            user.FirstName = ShopUser.FirstName;
            user.LastName = ShopUser.LastName;
            user.Email = ShopUser.Email;
            user.Address = ShopUser.Address;
            user.PhoneNumber = ShopUser.PhoneNumber;
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToPage("./Customers");
        }

        public async Task<IActionResult> OnGetGetOrdersAsync(string id, string searchStringOrder)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

           
            var ordersQuery = _applicationDbContext.Orders
                .Where(o => o.UserId == id);

            if (!string.IsNullOrEmpty(searchStringOrder))
            {
               
                string lowerCaseSearch = searchStringOrder.ToLower();

             
                bool isNumericSearch = int.TryParse(searchStringOrder, out int orderId);

               
                bool isDateSearch = DateTime.TryParse(searchStringOrder, out DateTime searchDate);

              
                ordersQuery = ordersQuery.Where(o =>
                    (!string.IsNullOrEmpty(o.status) && o.status.ToLower().Contains(lowerCaseSearch)) ||
                    (!string.IsNullOrEmpty(o.ShippingAddress) && o.ShippingAddress.ToLower().Contains(lowerCaseSearch)) ||
                    (isNumericSearch && o.Id == orderId) || // Strictly match Id if search is numeric
                    (isDateSearch &&
                        (o.CreatedAt.Date == searchDate.Date || // Exact match for full date
                        o.CreatedAt.Month == searchDate.Month && o.CreatedAt.Day == searchDate.Day)) || // Match day and month
                    (!isNumericSearch && o.CreatedAt.ToString().Contains(searchStringOrder)) // Fallback for non-numeric, non-date searches
                );
            }

            
            var orders = await ordersQuery
                .Select(o => new
                {
                    o.Id,
                    o.CreatedAt,
                    o.TotalAmount,
                    o.ShippingAddress,
                    o.status
                })
                .ToListAsync();

            return new JsonResult(orders);
        }


    }
}
