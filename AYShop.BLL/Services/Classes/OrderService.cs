using AYShop.BLL.Services.Interface;
using AYShop.DAL.Models;
using AYShop.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.BLL.Services.Classes
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order?> AddAsync(Order order)
        {
          return  await _orderRepository.AddAsync(order);
        }

        public async Task<bool> ChangeStatusAsync(int orderId, OrderStatusEnum newStatus)
        {
            return await _orderRepository.ChangeStatusAsync(orderId, newStatus);
        }

        public async Task<List<Order>> GetByStatusAsync(OrderStatusEnum status)
        {
            return await _orderRepository.GetByStatusAsync(status);
        }

        public async Task<List<Order>> GetOrderByUserAsync(string userId)
        {
            return await _orderRepository.GetOrderByUserAsync(userId);
        }

     

        public async Task<Order> GetUserByOrder(int orderId)
        {
            return await _orderRepository.GetUserByOrder(orderId);
        }

      
    }
}
