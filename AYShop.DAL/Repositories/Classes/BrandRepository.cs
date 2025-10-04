using AYShop.DAL.Data;
using AYShop.DAL.Models;
using AYShop.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.DAL.Repositories.Classes
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}
