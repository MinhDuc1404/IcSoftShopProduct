using IcSoft.Infrastructure.Models;

namespace IcSoftShopProduct.Models
{
    public class ProductShopViewModel
    {
        public List<Product> Products {  get; set; }
        public List<Category> Categories { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
