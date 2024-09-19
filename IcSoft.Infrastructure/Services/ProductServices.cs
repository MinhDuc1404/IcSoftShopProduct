using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Services
{
    public class ProductServices : IProductServices
    {
        private readonly ApplicationDbContext _Context; 
        public ProductServices(ApplicationDbContext context) 
        {
            _Context = context;
        }
        public async Task<Product> AddProduct(Product product)
        {
           _Context.Add(product);
           await _Context.SaveChangesAsync();
            return product;
        }
    }
}
