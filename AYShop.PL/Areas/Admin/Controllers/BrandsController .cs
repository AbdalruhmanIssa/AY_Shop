using AYShop.BLL.Services.Classes;
using AYShop.BLL.Services.Interface;
using AYShop.DAL.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AYShop.PL.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
 [Authorize(Roles = "Admin,SuperAdmin")]
    
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

       public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpGet("")]
        public IActionResult GetAll()
        {
            return Ok(_brandService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var brand = _brandService.GetById(id);
            if (brand is null) return NotFound();
            return Ok(brand);
        }
        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] BrandRequest request)
        {
            var result = await _brandService.CreateFile(request);
            return Ok(result);
        }
        [HttpPatch("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] BrandRequest request)
        {
            var updated = _brandService.Update(id, request);
            return updated > 0 ? Ok() : NotFound();
        }
        [HttpPatch("ToggleStatus/{id}")]
        public IActionResult ToggleStatus([FromRoute] int id)
        {
            var updated = _brandService.ToggleStatus(id);
            return updated
                ? Ok(new { message = "status toggled" })
                : NotFound(new { message = "category not found" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _brandService.Delete(id);
            return deleted > 0 ? Ok() : NotFound();
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
