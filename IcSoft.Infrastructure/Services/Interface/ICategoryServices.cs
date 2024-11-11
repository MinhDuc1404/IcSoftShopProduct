using IcSoft.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Services.Interface
{
    public interface ICategoryServices
    {
        Task<Category> AddCategory(Category category);
        Task<List<Category>> GetListCategory();
        Task<bool> CategoryExists(string name);
        Task<Category> FindCategory(int id);
        Task<Category> UpdateCategory(Category category);
        Task<Category> DeleteCategory(Category category);

    }
}
