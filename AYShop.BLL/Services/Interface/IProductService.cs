using AYShop.DAL.DTO.Requests;
using AYShop.DAL.DTO.Responses;
using AYShop.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.BLL.Services.Interface
{
    public interface IProductService : IGenericService<ProductRequest, ProductResponse, Product>
    {
        Task<int> CreateFile(ProductRequest request);
        Task<List<ProductResponse>> GetAllProducts(HttpRequest request, bool onlyActive = false, int pagenum = 1, int pagesize = 1);
    }
}
