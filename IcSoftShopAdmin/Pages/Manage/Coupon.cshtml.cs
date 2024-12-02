using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IcSoftShopAdmin.Pages.Manage
{

    public class CouponModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CouponModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IList<Coupon> Coupons { get; set; } = new List<Coupon>();

        [BindProperty]
        public Coupon NewCoupon { get; set; } = new Coupon();
        public async Task OnGetAsync()  
        {
            Coupons = await _applicationDbContext.Coupons.ToListAsync();
        }
        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _applicationDbContext.Coupons.Add(NewCoupon);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var coupon = await _applicationDbContext.Coupons.FindAsync(id);
            if (coupon != null)
            {
                _applicationDbContext.Coupons.Remove(coupon);
                await _applicationDbContext.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
