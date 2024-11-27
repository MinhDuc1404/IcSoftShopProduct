using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IcSoftShopAdmin.Pages.Manage
{
    [Authorize(Roles = "admin")]
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
        public string SearchString { get; set; }

        public IList<Order> OrdersWithCoupon { get; set; } = new List<Order>();

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 3;
        public int TotalCoupons { get; set; }
        public int TotalPages { get; set; }
        public async Task OnGetAsync(int pageNumber = 1, string searchString = null)
        {
            SearchString = searchString;
          
            var couponQuery = _applicationDbContext.Coupons.AsQueryable();

           
            if (!string.IsNullOrEmpty(searchString))
            {
                couponQuery = couponQuery.Where(c => c.Code.Contains(searchString));
            }

     
            
            TotalCoupons = await couponQuery.CountAsync();
            TotalPages = (int)Math.Ceiling(TotalCoupons / (double)PageSize);

          Coupons  = await couponQuery
            .OrderByDescending(o => o.ValidFrom)
            .Skip((pageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

            PageNumber = pageNumber;
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

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var coupon = await _applicationDbContext.Coupons.FindAsync(id);
            if (coupon != null)
            {
                coupon.Code = NewCoupon.Code;
                coupon.Discount = NewCoupon.Discount;
                coupon.ValidFrom = NewCoupon.ValidFrom;
                coupon.ValidUntil = NewCoupon.ValidUntil;
                coupon.UsageLimit = NewCoupon.UsageLimit;

                await _applicationDbContext.SaveChangesAsync();
            }
                return RedirectToPage();
        }

        public async Task<IActionResult> OnGetOrdersByCouponAsync(int couponId)
        {
            if ((int)couponId == 0)
            {
                return NotFound();
            }
            var ordersQuery = _applicationDbContext.Orders
       .Where(o => o.CouponId == couponId);
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
