using IcSoft.Infrastructure.Models;

namespace IcSoftShopProduct.Models
{
    public class ProductShopFilterViewModel
    {
        public List<Product> Products { get; set; }

        public int TotalPages { get; set; }
    }
}
