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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;
        public OrderRepository(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<Order?> AddAsync(Order order)
        {
            await _applicationDBContext.Orders.AddAsync(order);
            await _applicationDBContext.SaveChangesAsync();
            return order;
        }

        public async Task<Order> GetUserByOrder(int orderId)
        {
            return await _applicationDBContext.Orders.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == orderId);
        }
        //public async Task<List<Order>> GetAllWithUserAsync(string userId)
        //{
        //    return await _applicationDBContext.Orders
        //        .Where(o => o.UserId == userId)
        //        .ToListAsync();
        //}

        public async Task<List<Order>> GetByStatusAsync(OrderStatusEnum status)
        {
            return await _applicationDBContext.Orders
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        public async Task<List<Order>> GetOrderByUserAsync(string userId)
        {
            return await _applicationDBContext.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<bool> ChangeStatusAsync(int orderId, OrderStatusEnum newStatus)
        {
            var order = await _applicationDBContext.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = newStatus;
            var result = await _applicationDBContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UserHasApprovedOrderForProductAsync(string userId, int productId)
        {
            return await _applicationDBContext.Orders
                .Include(o => o.orderItems)
                .AnyAsync(e => e.UserId == userId
                            && e.Status == OrderStatusEnum.Approved
                            && e.orderItems.Any(oi => oi.ProductId == productId));
        }

    }
}
