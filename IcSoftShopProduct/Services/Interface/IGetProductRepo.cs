using IcSoft.Infrastructure.Models;
using IcSoftShopProduct.Models;

namespace IcSoftShopProduct.Services.Interface
{
    public interface IGetProductRepo
    {
        Task<ProductDetailsViewModel> GetProductDetials(string name);
        Task<ProductShopViewModel> GetProductShop(int page, int pageSize);

        Task<ProductShopViewModel> GetProductShopSale(int page, int pageSize);

        Task<ShopCategoryViewModel> GetProductShopSearch(string? searchname, int page, int pageSize);
        Task<ProductShopFilterViewModel> GetProductShopFilter(string? priceRange, string? sortOption, int pageNumber);

        Task<ProductShopFilterViewModel> GetProductShopSaleFilter(string? priceRange, string? sortOption, int pageNumber);

        Task<List<Product>> GetProductSearchQuery(string query);


    }
}
