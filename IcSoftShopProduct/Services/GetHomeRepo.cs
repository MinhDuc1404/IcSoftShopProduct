using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using IcSoftShopProduct.Models;
using IcSoftShopProduct.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IcSoftShopProduct.Services
{
    public class GetHomeRepo : IGetHomeRepo
    {
        private readonly IProductServices _productServices;

        public GetHomeRepo(IProductServices productServices)
        {
            _productServices = productServices;
        }
        public async Task<HomeViewModel> GetHomeIndex()
        {
            var products = await _productServices.GetListProduct();

         

            return new HomeViewModel
            {
                Products = products
            };
        }
    }
}
