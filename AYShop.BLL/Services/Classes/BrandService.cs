using AYShop.BLL.Services.Interface;
using AYShop.DAL.DTO.Requests;
using AYShop.DAL.DTO.Responses;
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
    public class BrandService:GenericService<BrandRequest,BrandResponse,Brand>,IBrandService
    {
        private readonly IBrandRepository _repository;
        private readonly IFileService _fileService;

        public BrandService(IBrandRepository repository, IFileService fileService) : base(repository)
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task<int> CreateFile(BrandRequest request)
        {
            var entity = request.Adapt<Brand>();
            entity.CreatedAt = DateTime.UtcNow;

            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                entity.MainImage = imagePath;
            }

            return _repository.Add(entity);
        }

    }
}
