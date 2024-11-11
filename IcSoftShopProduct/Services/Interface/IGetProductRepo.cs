using IcSoft.Infrastructure.Models;
using IcSoftShopProduct.Models;

namespace IcSoftShopProduct.Services.Interface
{
    public interface IGetProductRepo
    {
        Task<ProductDetailsViewModel> GetProductDetials(string name);
        Task<ProductShopViewModel> GetProductShop(int page, int pageSize);

        Task<ShopCategoryViewModel> GetProductShopSearch(string? searchname, int page, int pageSize);
        Task<List<Product>> GetProductShopFilter(string? searchname, string? priceRange, string? sortOption, int curentPage);


    }
}
