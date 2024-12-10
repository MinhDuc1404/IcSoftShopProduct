using IcSoft.Infrastructure.Models;
using IcSoftShopProduct.Models;

namespace IcSoftShopProduct.Services.Interface
{
    public interface IGetCartRepo
    {
        Task<List<CartItem>> GetListCartItems(string userid);
        Task SaveCartItem(string userid, List<CartItem> cartItem);
        Task RemoveCartItem(string userId, string productName);
        Task ClearCart(string userid);
    }
}
