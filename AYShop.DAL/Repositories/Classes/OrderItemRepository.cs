using AYShop.DAL.Data;
using AYShop.DAL.Models;
using AYShop.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.DAL.Repositories.Classes
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;
        public OrderItemRepository(ApplicationDBContext applicationDBContext) {
            _applicationDBContext = applicationDBContext;
        }

        public async Task Add(List<OrderItem> item)
        {
            await _applicationDBContext.OrderItems.AddRangeAsync(item);
            await _applicationDBContext.SaveChangesAsync();
        }
    }
}
