using AYShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.DAL.Repositories.Interface
{
    public interface IOrderRepository
    {
         Task<Order?> GetUserByOrder(int orderId);
        Task<Order?>AddAsync(Order order);
       
        Task<List<Order>> GetByStatusAsync(OrderStatusEnum status);
        Task<List<Order>> GetOrderByUserAsync(string userId);
       Task<bool> ChangeStatusAsync(int orderId, OrderStatusEnum newStatus);
        Task<bool> UserHasApprovedOrderForProductAsync(string userId, int productId);
    }
}
