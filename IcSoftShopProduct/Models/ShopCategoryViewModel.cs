using IcSoft.Infrastructure.Models;

namespace IcSoftShopProduct.Models
{
    public class ShopCategoryViewModel
    {
        public List<Product> Products { get; set; }

        public List<Category> Categories { get; set; }

        public string? SearchName { get; set; }


        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
