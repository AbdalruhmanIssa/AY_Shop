using AYShop.DAL.DTO.Requests;
using AYShop.DAL.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.BLL.Services.Interface
{
    public interface ICartService
    {
       Task <bool> AddToCart(CartRequest cartRequest, string userId);
       Task <CartResponseSummary> CartResponseSummary(string userId);
        Task<bool> ClearCart(string userId);

    }
}
