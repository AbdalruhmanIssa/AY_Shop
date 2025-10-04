using AYShop.BLL.Services.Interface;
using AYShop.DAL.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AYShop.PL.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Customer")]
   [Authorize(Roles = "Customer")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("")]
        public IActionResult GetAll() => Ok(_brandService.GetAll(true));

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var category = _brandService.GetById(id);
            if (category is null) return NotFound();
            return Ok(category);
        }
    }
}
