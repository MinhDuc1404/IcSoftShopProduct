using IcSoftShopProduct.Models;

namespace IcSoftShopProduct.Services.Interface
{
    public interface IGetHomeRepo
    {
        Task<HomeViewModel> GetHomeIndex();
    }
}
