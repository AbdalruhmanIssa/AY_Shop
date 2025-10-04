using AYShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.DAL.Repositories.Interface
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        IEnumerable<T> GetAll(bool withTracking = false);
        T? GetById(int id);
        int Add(T Entity);
        int Update(T Entity);
        int Remove(T Entity);
    }
}
