using AYShop.BLL.Services.Interface;
using AYShop.DAL.DTO.Requests;
using AYShop.DAL.DTO.Responses;
using AYShop.DAL.Models;
using AYShop.DAL.Repositories.Classes;
using AYShop.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.BLL.Services.Classes
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> AddToCart(CartRequest request, string UserId)
        {
            var newItem = new Cart
            {
                ProductId = request.ProductId,
                UserId = UserId,
                Count = 1
            };

            return await _cartRepository.Add(newItem) > 0;
        }

        public async Task<CartResponseSummary> CartResponseSummary(string userId)
        {
            var cartItems = await _cartRepository.GetUserCart(userId);

            var response = new CartResponseSummary
            {
                CartResponseList = cartItems.Select(ci => new CartResponse
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    Count = ci.Count,
                    Price = ci.Product.Price
                }).ToList()
            };

            return response;
        }

        public async Task<bool> ClearCart(string userId)
        {
          return await _cartRepository.ClearCart(userId);
        }
    }
}
