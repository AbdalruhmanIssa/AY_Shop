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
    public class ProductRepository:GenericRepository<Product>,IProductRepository
    {
        private readonly ApplicationDBContext _context;

        public ProductRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task DecreaseQuantityAsync(List<(int productId, int quantity)> items)
        {
            var productIds = items.Select(i => i.productId).ToList();

            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            foreach (var product in products)
            {
                var item = items.First(i => i.productId == product.Id);

                if (product.Quantity < item.quantity)
                {
                    throw new Exception("not enough stock available");
                }

                product.Quantity -= item.quantity;
            }

            await _context.SaveChangesAsync();
        }

        public List<Product> GetAllProductsWithImage()
        {
            return _context.Products
                .Include(p => p.SubImages).Include(p=>p.Reviews).ThenInclude(r=>r.User)
                .ToList();
        }
    }
}
