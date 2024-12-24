using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IcSoft.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using IcSoft.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace IcSoftShopAdmin.Pages.Manage
{
   [Authorize(Roles = "admin")]
    public class CustomersModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ShopUser> _shopUser;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ShopUser> _signInManager;
        private readonly IUserStore<ShopUser> _userStore;
        private readonly IUserEmailStore<ShopUser> _emailStore;

        public CustomersModel(ApplicationDbContext applicationDbContext, UserManager<ShopUser> shopUser, RoleManager<IdentityRole> roleManager, IUserStore<ShopUser> userStore,
            SignInManager<ShopUser> signInManager)
        {
            _applicationDbContext = applicationDbContext;
            _shopUser = shopUser;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            // Initialize RoleManager
        }

        public Dictionary<string, List<string>> UserRoles { get; set; } = new Dictionary<string, List<string>>();  // Store user roles
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

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        public string SelectedRole { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public int TotalCustomers { get; set; }
        public int TotalPages { get; set; }

        public List<string?> AvailableRoles { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage ="Vui lòng không để trống")]
            [EmailAddress(ErrorMessage = "Hãy nhập đúng form địa chỉ email")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Vui lòng không để trống")]
            [StringLength(100, ErrorMessage = "Mật khẩu phải có độ dài lớn hơn hoặc bằng 6 kí tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Mật khẩu phải chứa tối thiểu 1 kí tự thường, 1 kí tự viết hoa và 1 chữ số.")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Vui lòng không để trống")]

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Mật khẩu không khớp.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Vui lòng không để trống")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Vui lòng không để trống")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Vui lòng không để trống")]
            [Phone]
            [Display(Name = "Phone Number")]
            public string Phone { get; set; }

            // Make Address optional
            [Display(Name = "Address")]
            public string? Address { get; set; }
        }
        public async Task OnGetAsync(int pageNumber = 1, string searchString = null)
        {
            IQueryable<ShopUser> shopUsersQuery = _applicationDbContext.ShopUsers;

            SearchString = searchString;
            if (!string.IsNullOrEmpty(SearchString))
            {
                shopUsersQuery = shopUsersQuery.Where(m => m.FirstName.Contains(SearchString) || m.LastName.Contains(SearchString) || m.UserName.Contains(SearchString));
            }

            TotalCustomers = await shopUsersQuery.CountAsync();
            TotalPages = (int)Math.Ceiling(TotalCustomers / (double)PageSize);

            ShopUsers = await shopUsersQuery.OrderByDescending(o => o.CreatedDate)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            PageNumber = pageNumber;

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
                var orderCount = orderCounts.FirstOrDefault(o => o.UserId == user.Id)?.OrderCount ?? 0;
                OrderCountsByUser[user.Id] = orderCount;
                var roles = await _shopUser.GetRolesAsync(user);
                UserRoles[user.Id] = roles.ToList();
            }

            AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        }

        public async Task<IActionResult> OnGetDeleteAsync(string id)
        {
            if (id == null)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy khách hàng." });
            }
            var user = await _applicationDbContext.ShopUsers.FindAsync(id);
            if (user != null)
            {
                _applicationDbContext.ShopUsers.Remove(user);
                await _applicationDbContext.SaveChangesAsync();
                return new JsonResult(new { success = true });
            }

            return new JsonResult(new { success = false, message = "Xóa khách hàng thất bại." });
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
                    (isNumericSearch && o.Id == orderId) ||
                    (isDateSearch &&
                        (o.CreatedAt.Date == searchDate.Date ||
                        o.CreatedAt.Month == searchDate.Month && o.CreatedAt.Day == searchDate.Day)) ||
                    (!isNumericSearch && o.CreatedAt.ToString().Contains(searchStringOrder))
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

        public async Task<JsonResult> OnGetChangeRoleAsync(string userId, string selectedRole)
        {
            var user = await _shopUser.FindByIdAsync(userId);
            if (user == null)
            {
                return new JsonResult(new { success = false, message = "Người dùng không tồn tại." });
            }
            var currentRole = await _shopUser.GetRolesAsync(user);
            await _shopUser.RemoveFromRolesAsync(user, currentRole);

            await _shopUser.AddToRoleAsync(user, selectedRole);
          
            return new JsonResult(new { success = true, role = selectedRole });
        }

        public async Task<IActionResult> OnPostAddAsync()
        {

                var user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.PhoneNumber = Input.Phone;
                user.Address = Input.Address;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _shopUser.CreateAsync(user, Input.Password);

                await _shopUser.AddToRoleAsync(user, SelectedRole);



                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            

            return RedirectToPage("./Customers");
        }


        private ShopUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ShopUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ShopUser)}'. " +
                    $"Ensure that '{nameof(ShopUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ShopUser> GetEmailStore()
        {
            if (!_shopUser.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ShopUser>)_userStore;
        }
    }
}
