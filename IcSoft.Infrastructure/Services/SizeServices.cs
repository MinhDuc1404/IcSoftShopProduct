using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Services
{
    public class SizeServices : ISizeServices
    {
        private readonly ApplicationDbContext _context;
        public SizeServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Size> AddSize(Size Size)
        {
            _context.Add(Size);

            await _context.SaveChangesAsync();
            return Size;
        }
        public async Task<List<Size>> GetListSize()
        {
            return await _context.Sizes.ToListAsync();
        }
        public async Task<Size> FindSize(int id)
        {
            return await _context.Sizes.FirstOrDefaultAsync(c => c.SizeId == id);
        }
        public async Task<Size> UpdateSize(Size size)
        {
            _context.Update(size);
            await _context.SaveChangesAsync();
            return size;
        }
        public async Task<Size> DeleteSize(Size size)
        {
            _context.Remove(size);
            await _context.SaveChangesAsync();
            return size;
        }
    }
}
