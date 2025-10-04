using AYShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.BLL.Services.Interface
{
    public interface IOrderService
    {
        Task<Order?> GetUserByOrder(int orderId);
        Task<Order?> AddAsync(Order order);

        Task<List<Order>> GetByStatusAsync(OrderStatusEnum status);
        Task<List<Order>> GetOrderByUserAsync(string userId);
        Task<bool> ChangeStatusAsync(int orderId, OrderStatusEnum newStatus);
    }
}
