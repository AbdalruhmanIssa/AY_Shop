using AYShop.BLL.Services.Interface;
using AYShop.DAL.DTO.Requests;
using AYShop.DAL.DTO.Responses;
using AYShop.DAL.Models;
using AYShop.DAL.Repositories.Classes;
using AYShop.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.BLL.Services.Classes
{
    public class CheckOutService : ICheckOutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailSender _emailSender;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;

        public CheckOutService(ICartRepository cartRepository,IOrderRepository orderRepository, IEmailSender emailSender,
            IOrderItemRepository orderItemRepository,IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _emailSender = emailSender;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
        }

        public IEmailSender EmailSender { get; }
        public IProductRepository ProductRepository { get; }

        public async Task<bool> HandlePaymentSuccessAsync(int orderId)
        {
            var order = await _orderRepository.GetUserByOrder(orderId);

            var subject = "";
            var body = "";

            if (order.PaymentMethod == PaymentMethodEnum.Visa)
            {
                order.Status =OrderStatusEnum.Approved;
                var carts=await _cartRepository.GetUserCart(order.UserId);
                var orderItems = new List<OrderItem>();
                var products=new List<(int productId,int quntuty )>();
                foreach (var cartItem in carts)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = cartItem.ProductId,
                        TotalPrice = cartItem.Product.Price * cartItem.Count,
                        Count = cartItem.Count,
                        Price=cartItem.Product.Price
                    };
                    orderItems.Add(orderItem);
                    products.Add((cartItem.ProductId,cartItem.Count));
                }
                await _orderItemRepository.Add(orderItems);
                await _cartRepository.ClearCart(order.UserId);
                await _productRepository.DecreaseQuantityAsync(products);

                subject = "Payment Successful - ayshop";
                body = $"<h1>Thank you for your payment </h1>" +
                       $"<p>Your payment for order {orderId}</p>" +
                       $"<p>Total Amount : {order.TotalPrice}</p>";
            }
            else if (order.PaymentMethod == PaymentMethodEnum.Cash)
            {
                subject = "Order placed successfully";
                body = $"<h1>Thank you for your order </h1>" +
                       $"<p>Your payment for order {orderId}</p>" +
                       $"<p>Total Amount : {order.TotalPrice}</p>";
            }


            await _emailSender.SendEmailAsync(order.User.Email, subject, body);

            return true;
        }

        public async Task<CheckOutResponse> ProcessPaymentAsync(
    CheckOutRequest request,
    string userId,
    HttpRequest httpRequest)
    {
        // 1) Load cart
        var cartItems = await _cartRepository.GetUserCart(userId);

        if (!cartItems.Any())
        {
                return new CheckOutResponse
                {
                    Success = false,
                    Message = "Cart is empty."
                };
        }
            Order order = new Order {UserId=userId , PaymentMethod=request.PaymentMethod,TotalPrice=cartItems.Sum(x=>x.Product.Price*x.Count)};
       await _orderRepository.AddAsync(order);
            // 2) Handle payment method branches (optional)
        if (request.PaymentMethod == PaymentMethodEnum.Cash)
        {
            // handle cash flow here if you support COD
            // return a response or continue to Stripe if you always use card for now
        }

        if (request.PaymentMethod == PaymentMethodEnum.Visa)
        {
            // 3) Build Stripe Checkout session options
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/api/Customer/CheckOuts/Success/{order.Id}",
                CancelUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/checkout/cancel",
            };

            foreach (var item in cartItems)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                            Description = item.Product.Description
                        },
                        UnitAmount = (long)item.Product.Price, // amount in the smallest currency unit
                    },
                    Quantity = item.Count,
                });
            }

            // 4) Create session and return URL
            var service = new SessionService();
            var session = service.Create(options);
                order.PaymentId=session.Id;
            return new CheckOutResponse
            {
                Success = true,
                Message = "Payment session created successfully.",
                PaymentId = session.Id, // uncomment if you need to store/use it
                URL = session.Url,
            };
        }

            return new CheckOutResponse { Success = false, Message = "invalod" };

       
    }

}
}
