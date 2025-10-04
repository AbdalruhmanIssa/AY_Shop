using AYShop.BLL.Services.Classes;
using AYShop.BLL.Services.Interface;
using AYShop.DAL.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AYShop.PL.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
  [Authorize(Roles = "Admin,SuperAdmin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] ProductRequest request)
        {
            var result = await _productService.CreateFile(request);
            return Ok(result);
        }
        [HttpGet("")]
        public IActionResult GetAll(
      [FromQuery] int pageNumber = 1,
      [FromQuery] int pageSize = 5)
        {
            var products = _productService.GetAllProducts(Request, false, pageNumber, pageSize );
            return Ok(products);
        }

    }
}
