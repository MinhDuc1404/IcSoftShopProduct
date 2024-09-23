using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace IcSoftShopAdmin.Pages.Manage
{
    public class EditModel : PageModel
    {
      private readonly ApplicationDbContext _context;
        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public ShopUser ShopUser { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if(string.IsNullOrWhiteSpace(id)) return NotFound();

            var shopUser = await _context.ShopUsers
                .FirstOrDefaultAsync(m => m.Id == id);

            if(shopUser == null) return NotFound();

            ShopUser = shopUser;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
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
            var user = await _context.ShopUsers.FirstOrDefaultAsync(m => m.Id == id);
            if(user == null)
                return NotFound();
            user.FirstName = ShopUser.FirstName;
            user.LastName = ShopUser.LastName;
            user.Email = ShopUser.Email;
            user.Address = ShopUser.Address;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Customers");
        }
    }
}
