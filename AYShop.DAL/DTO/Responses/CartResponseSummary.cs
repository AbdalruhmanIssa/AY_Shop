using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.DAL.DTO.Responses
{
    public class CartResponseSummary
    {
        public List<CartResponse> CartResponseList { get; set; } = new List<CartResponse>();
        public decimal CartTotal => CartResponseList.Sum(x => x.Totalprice);
    }
}
