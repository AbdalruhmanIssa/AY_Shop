using AYShop.BLL.Services.Interface;
using AYShop.DAL.DTO.Requests;
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
    public class ReviewService : IReviewService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IOrderRepository orderRepository, IReviewRepository reviewRepository)
        {
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<bool> AddReviewAsync(ReviewRequest reviewRequest, string userId)
        {
            // Check if user actually purchased the product (approved order)
            var hasOrder = await _orderRepository
                .UserHasApprovedOrderForProductAsync(userId, reviewRequest.ProductId);

            if (!hasOrder) return false;

            // Check if user already reviewed this product
            var alreadyReviewed = await _reviewRepository
                .HasUserReviewedProduct(userId, reviewRequest.ProductId);

            if (alreadyReviewed) return false;

            // Map DTO -> Entity
            var review = reviewRequest.Adapt<Review>();

            // Save
            await _reviewRepository.AddReviewAsync(review, userId);

            return true;
        }
    }
}
