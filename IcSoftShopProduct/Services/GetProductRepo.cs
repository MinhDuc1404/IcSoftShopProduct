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

            var listproductcategory = await _productServices.GetListProductByCategory(product.CategoryID);
            var listproductcollection = await _productServices.GetListProductByCollection(product.CollectionID);
   
            return new ProductDetailsViewModel
            {
                Product = product,
                ProductsCategory = listproductcategory,
                ProductsCollection = listproductcollection


            };

        }
        public async Task<ProductShopViewModel> GetProductShop()
        {
            var product = await _productServices.GetListProduct();
            return new ProductShopViewModel
            {
                Products = product
            };
        }
    }
}
