using AYShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.DAL.Repositories.Interface
{
    public interface ICartRepository
    {
        Task<int> Add(Cart cart);
        Task<List<Cart>>GetUserCart(string userId);  
        Task<bool>ClearCart (string userId);
    }
}
