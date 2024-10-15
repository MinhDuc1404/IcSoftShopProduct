using IcSoft.Infrastructure.Models;
using IcSoftShopProduct.Models;

namespace IcSoftShopProduct.Services.Interface
{
    public interface IGetCartRepo
    {
        List<CartItem> GetListCartItems(string userid);
        void SaveCartCookie(string userid, List<CartItem> cartItems);
        void ClearCart(string userid);
    }
}
