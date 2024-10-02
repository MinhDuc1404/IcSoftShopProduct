using IcSoftShopProduct.Models;

namespace IcSoftShopProduct.Services.Interface
{
    public interface IGetCartRepo
    {
        List<CartItemViewModel> GetCartItems(string userid);
        void SaveCartCookie(string userid, List<CartItemViewModel> cartItems);
        void ClearCart(string userid);
    }
}
