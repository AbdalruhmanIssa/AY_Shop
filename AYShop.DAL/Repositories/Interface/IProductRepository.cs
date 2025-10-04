
using AYShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.DAL.Repositories.Interface
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task DecreaseQuantityAsync(List<(int productId, int quantity)>items);
        List<Product> GetAllProductsWithImage();
    }
}
