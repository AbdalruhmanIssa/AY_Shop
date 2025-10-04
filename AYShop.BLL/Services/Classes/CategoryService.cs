using AYShop.BLL.Services.Interface;
using AYShop.DAL.DTO.Requests;
using AYShop.DAL.DTO.Responses;
using AYShop.DAL.Models;
using AYShop.DAL.Repositories.Interface;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.BLL.Services.Classes
{
    public class CategoryService : GenericService<CategoryRequest, CategoryResponse, Category>, ICategoryService
       
    {
        public CategoryService(ICategoryRepository categoryRepository) : base(categoryRepository) { }
       
    }
}
