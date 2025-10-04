using AYShop.DAL.Data;
using AYShop.DAL.Models;
using AYShop.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.DAL.Repositories.Classes
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDBContext _context;

        public CartRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            return await _context.SaveChangesAsync();
        }
        public async Task<List<Cart>> GetUserCart(string UserId)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == UserId)
                .ToListAsync();
        }
        public async Task<bool> ClearCart(string UserId)
        {
            var items = _context.Carts.Where(c => c.UserId == UserId).ToList();
            _context.Carts.RemoveRange(items);

            await _context.SaveChangesAsync();
            return true;
        }

       
    }
}
