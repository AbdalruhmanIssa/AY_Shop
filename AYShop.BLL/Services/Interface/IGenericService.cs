using AYShop.DAL.DTO.Requests;
using AYShop.DAL.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.BLL.Services.Interface
{
    public interface IGenericService<TRequest,TResponse,TEntity>
    {
        IEnumerable<TResponse> GetAll(bool a = false);
        TResponse? GetById(int id);
        int Create(TRequest request);
        int Delete(int id);
        int Update(int id, TRequest request);
        bool ToggleStatus(int id);
    }
}
