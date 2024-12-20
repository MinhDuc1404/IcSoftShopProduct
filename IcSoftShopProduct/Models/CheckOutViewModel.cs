using IcSoft.Infrastructure.Models;

namespace IcSoftShopProduct.Models
{
    public class CheckOutViewModel
    {
        public List<CartItem> CartItems { get; set; }

        public decimal TotalPrice { get; set; }

        public CartItem SingleItem { get; set; }
    }
}
