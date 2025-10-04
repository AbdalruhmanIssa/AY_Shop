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
    public class ReviewRepository : IReviewRepository
    {

        private readonly ApplicationDBContext _context;

        public ReviewRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> HasUserReviewedProduct(string userId, int productId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }

        public async Task AddReviewAsync(Review request, string userId)
        {
            request.UserId = userId;
            request.ReviewDate = DateTime.Now;

            await _context.Reviews.AddAsync(request);
            await _context.SaveChangesAsync();
        }
    }

}