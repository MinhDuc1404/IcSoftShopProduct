using IcSoft.Infrastructure.Models;
using IcSoft.Infrastructure.Services;
using IcSoft.Infrastructure.Services.Interface;
using IcSoftShopProduct.Models;
using IcSoftShopProduct.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace IcSoftShopProduct.Services
{
    public class GetCartRepo : IGetCartRepo
    {
        private readonly ApplicationDbContext _context;

        public GetCartRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetListCartItems(string userId)
        {
            return await _context.CartItems
                                 .Where(c => c.UserId == userId)
                                 .ToListAsync();
        }

        public async Task SaveCartItem(string userId, List<CartItem> cartItems)
        {
            var existingItems = _context.CartItems.Where(c => c.UserId == userId).ToList();

            foreach (var item in cartItems)
            {
                var existingItem = existingItems.FirstOrDefault(c => c.ProductId == item.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity = item.Quantity;
                    existingItem.Price = item.Price;
                }
                else
                {
                    item.UserId = userId;
                    _context.CartItems.Add(item);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartItem(string userId, string productName)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductName == productName);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCart(string userId)
        {
            var items = _context.CartItems.Where(c => c.UserId == userId);
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
     } 
  }
