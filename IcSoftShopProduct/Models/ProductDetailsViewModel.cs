using IcSoft.Infrastructure.Models;

namespace IcSoftShopProduct.Models
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }

        public List<Product> ProductsCategory { get; set; }
        public List<Product> ProductsCollection { get; set; }
    }
}
