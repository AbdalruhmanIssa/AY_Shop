using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.DAL.Utilites
{
    public interface ISeedData
    {
        Task DataSeeding();
        Task IdentityDataSeedingAsync();
    }
}
