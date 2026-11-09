using AYShop.BLL.Services.Interface;
using AYShop.DAL.DTO.Responses;
using AYShop.DAL.Models;
using AYShop.DAL.Repositories.Classes;
using AYShop.DAL.Repositories.Interface;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.BLL.Services.Classes
{
    public class GenericService<TRequest, TResponse, TEntity> : IGenericService<TRequest, TResponse, TEntity>
        where TEntity : BaseModel
    {
        private readonly IGenericRepository<TEntity> _geniricService;
        public GenericService(IGenericRepository<TEntity> geniricService)
        {
            _geniricService = geniricService;
        }
        public int Create(TRequest request)
        {
            var entity = request.Adapt<TEntity>();
            return _geniricService.Add(entity);
        }

        public int Delete(int id)
        {
            var entity = _geniricService.GetById(id);
            if (entity is null) return 0;

            return _geniricService.Remove(entity);
        }

        public IEnumerable<TResponse> GetAll(bool a=false)
        {
            var entities = _geniricService.GetAll();
            if (a) { entities = entities.Where(e => e.Status ==Status.Active); }
            return entities.Adapt<IEnumerable<TResponse>>();
        }

        public TResponse? GetById(int id)
        {
            var entity = _geniricService.GetById(id);
            return entity is null ? default : entity.Adapt<TResponse>();
        }
        public int Update(int id, TRequest request)
        {
            var entity = _geniricService.GetById(id);
            if (entity is null) return 0;
            var updated = request.Adapt(entity);
            return _geniricService.Update(updated);
        }
        public bool ToggleStatus(int id)
        {
            var category = _geniricService.GetById(id);
            if (category is null) return false;

            category.Status = category.Status == Status.Active ? Status.Inactive : Status.Active;
            _geniricService.Update(category);
            return true;
        }

     
    }
}
