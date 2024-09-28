using IcSoftShopProduct.Models;

namespace IcSoftShopProduct.Services.Interface
{
    public interface IGetProductRepo
    {
        Task<ProductDetailsViewModel> GetProductDetials(string name);
    }
}
