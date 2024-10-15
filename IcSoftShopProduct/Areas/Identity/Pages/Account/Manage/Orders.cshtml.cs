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
        private readonly ApplicationDbContext _context; // Assuming your DbContext is called ApplicationDbContext

        // Constructor for Dependency Injection
        public Index1Model(UserManager<ShopUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public List<Order> UserOrders { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            // Get the logged-in user's ID
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return NotFound("User not found.");
            }

            // Fetch the orders associated with the logged-in user
            UserOrders = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Page();
        }
    }
}
