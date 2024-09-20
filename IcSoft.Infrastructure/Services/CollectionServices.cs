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
    public class CollectionServices : ICollectionServices
    {
        private readonly ApplicationDbContext _context;

        public CollectionServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Collection> AddCollection(Collection collection)
        {
            _context.Add(collection);

            await _context.SaveChangesAsync();
            return collection;
        }
        public async Task<List<Collection>> GetListCollection()
        {
            return await _context.Collections.ToListAsync();
        }
        public async Task<Collection> FindCollection(int id)
        {
            return await _context.Collections.FirstOrDefaultAsync(c => c.CollectionId == id);
        }
    }
}
