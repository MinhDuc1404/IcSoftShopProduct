using IcSoft.Infrastructure.Models;

namespace IcSoftShopProduct.Models
{
    public class SearchViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; } 
        public string SearchName { get; set; }
    }
}
