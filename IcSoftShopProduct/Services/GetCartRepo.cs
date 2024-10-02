using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using IcSoftShopProduct.Models;
using IcSoftShopProduct.Services.Interface;
using Newtonsoft.Json;

namespace IcSoftShopProduct.Services
{
    public class GetCartRepo : IGetCartRepo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartCookieKey = "CartCookie";
        public GetCartRepo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

 
        public List<CartItemViewModel> GetCartItems(string userid)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var cartCookieKey = $"{CartCookieKey}_{userid}";

       
            var cartJson = httpContext.Request.Cookies[cartCookieKey];

   
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItemViewModel>();
            }

 
            var cartItems = JsonConvert.DeserializeObject<List<CartItemViewModel>>(cartJson);
            return cartItems;
        }


        public void SaveCartCookie(string userid, List<CartItemViewModel> cartItems)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var cartCookieKey = $"{CartCookieKey}_{userid}";

  
            var cartJson = JsonConvert.SerializeObject(cartItems);

   
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7), // Lưu cookie trong 7 ngày
                HttpOnly = true // Bảo mật hơn, chỉ cho phép truy cập thông qua HTTP
            };

          
            httpContext.Response.Cookies.Append(cartCookieKey, cartJson, cookieOptions);
        }

  
        public void ClearCart(string userid)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var cartCookieKey = $"{CartCookieKey}_{userid}";

         
            httpContext.Response.Cookies.Delete(cartCookieKey);
        }
    }
}
