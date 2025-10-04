using AYShop.DAL.DTO.Requests;
using AYShop.DAL.DTO.Responses;
using AYShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.BLL.Services.Interface
{
    public interface ICategoryService:IGenericService<CategoryRequest, CategoryResponse,Category>
    {
         
    }
}
