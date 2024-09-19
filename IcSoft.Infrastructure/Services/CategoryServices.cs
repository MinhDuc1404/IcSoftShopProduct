using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ApplicationDbContext _context;

        public CategoryServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Category> AddCategory(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<List<Category>> GetListCategory()
        {
          return await _context.Categories.ToListAsync();
        }
        public async Task<bool> CategoryExists(string name)
        {
            return await _context.Categories.AnyAsync(e => e.CategoryName == name);
        }
        public async Task<Category> FindCategory(int id)
        {
            return await _context.Categories.FirstAsync(e => e.CategoryID == id);
        }
        public async Task<Category> UpdateCategory(Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<Category> DeleteCategory(Category category)
        {
            _context.Remove(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
