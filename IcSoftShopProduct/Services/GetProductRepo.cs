using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using IcSoftShopProduct.Models;
using IcSoftShopProduct.Services.Interface;

namespace IcSoftShopProduct.Services
{
    public class GetProductRepo : IGetProductRepo
    {
        private readonly IProductServices _productServices;
        public GetProductRepo(IProductServices productServices)
        {
            _productServices = productServices;
        }
        public async Task<ProductDetailsViewModel> GetProductDetials(string name)
        {
            var product = await _productServices.GetProductByName(name);
   
            return new ProductDetailsViewModel
            {
                Product = product
            };

        }
    }
}
