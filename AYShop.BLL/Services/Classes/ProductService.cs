using AYShop.BLL.Services.Interface;
using AYShop.DAL.Data;
using AYShop.DAL.DTO.Requests;
using AYShop.DAL.DTO.Responses;
using AYShop.DAL.Models;
using AYShop.DAL.Repositories.Interface;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.BLL.Services.Classes
{
    public class ProductService: GenericService<ProductRequest, ProductResponse, DAL.Models.Product>, IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IFileService _fileService;
        private readonly ApplicationDBContext _applicationDBContext;

        public ProductService(IProductRepository repository, IFileService fileService,ApplicationDBContext applicationDBContext ) : base(repository)
        {
            _repository = repository;
            _fileService = fileService;
            _applicationDBContext = applicationDBContext;
        }

        public async Task<int> CreateFile(ProductRequest request)
        {
            var entity = request.Adapt<DAL.Models.Product>();
            entity.CreatedAt = DateTime.UtcNow;

            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                entity.MainImage = imagePath;
            }
            if (request.SubImages != null)
            {
                var subImagesPaths = await _fileService.UploadManyAsync(request.SubImages);
                entity.SubImages = subImagesPaths
                    .Select(img => new ProductImage { Name = img })
                    .ToList();
            }

            return _repository.Add(entity);
        }
        public async Task<List<ProductResponse>> GetAllProducts(HttpRequest request, bool onlyActive = false,int pagenum=1,int pagesize=1)
        {
            var products = _repository.GetAllProductsWithImage();

            if (onlyActive)
            {
                products = products
                    .Where(p => p.Status == Status.Active)
                    .ToList();
            }
            var pp=products.Skip((pagenum-1)*pagesize).Take(pagesize).ToList();
            return pp.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Quantity = p.Quantity,
                MainImageUrl = $"{request.Scheme}://{request.Host}/Images/{p.MainImage}",
                SubImagesUrls = p.SubImages
                    .Select(img => $"{request.Scheme}://{request.Host}/Images/{img.Name}")
                    .ToList(),
                Reviews = p.Reviews.Select(r => new ReviewResponse
                {
                    Id = r.Id,
                    Rate = r.Rate,
                    Comment = r.Comment,
                    FullName = r.User.FullName
                }).ToList()
            }).ToList();
        }



    }
}
