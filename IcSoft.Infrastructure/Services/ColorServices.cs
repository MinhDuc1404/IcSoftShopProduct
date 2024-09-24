using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcSoft.Infrastructure.Services
{
    public class ColorServices : IColorServices
    {
       private readonly ApplicationDbContext _context;
        public ColorServices(ApplicationDbContext context)
        {
             _context = context;
        }
        public async Task<Colors> AddColor(Colors colors)
        {
            _context.Add(colors);

            await _context.SaveChangesAsync();
            return colors;
        }
        public async Task<List<Colors>> GetListColor()
        {
            return await _context.Colors.ToListAsync();
        }
        public async Task<Colors> FindColor(int id)
        {
            return await _context.Colors.FirstOrDefaultAsync(c => c.ColorId == id);
        }
        public async Task<Colors> UpdateColor(Colors colors)
        {
            _context.Update(colors);
            await _context.SaveChangesAsync();
            return colors;
        }
        public async Task<Colors> DeleteColor(Colors colors)
        {
            _context.Remove(colors);
            await _context.SaveChangesAsync();
            return colors;
        }
    }
}
