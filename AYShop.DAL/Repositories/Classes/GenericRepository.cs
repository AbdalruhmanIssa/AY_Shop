using AYShop.DAL.Data;
using AYShop.DAL.Models;
using AYShop.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.DAL.Repositories.Classes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly ApplicationDBContext _context;

        public GenericRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public int Add(T Entity)
        {
            _context.Set<T>().Add(Entity);
            return _context.SaveChanges();
        }

        public IEnumerable<T> GetAll(bool withTracking = false)
        {
            if (withTracking)
                return _context.Set<T>().ToList();

            return _context.Set<T>().AsNoTracking().ToList();
        }


        public T? GetById(int id) => _context.Set<T>().Find(id);


        public int Remove(T Entity)
        {
            _context.Set<T>().Remove(Entity);
            return _context.SaveChanges();
        }

        public int Update(T Entity)
        {
            _context.Set<T>().Update(Entity);
            return _context.SaveChanges();
        }
    }
}
