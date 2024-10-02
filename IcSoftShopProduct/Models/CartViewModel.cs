using IcSoft.Infrastructure.Models;

namespace IcSoftShopProduct.Models
{
    public class CartViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return CartItems.Sum(item => item.TotalPrice);
            }
        }
    }
}
