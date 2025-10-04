using AYShop.DAL.DTO.Requests;
using AYShop.DAL.DTO.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.BLL.Services.Interface
{
    public interface ICheckOutService
    {
       Task<CheckOutResponse> ProcessPaymentAsync(CheckOutRequest request,string userId,HttpRequest httpRequest);
        Task<bool>HandlePaymentSuccessAsync(int orderId) ;
    }
}
