using AYShop.BLL.Services.Interface;
using AYShop.DAL.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AYShop.PL.Areas.Customer.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddToCart(CartRequest request)
        {
            var userId = User.FindFirstValue("Id");
            var result =await _cartService.AddToCart(request, userId);

            return result ? Ok() : BadRequest();
        }
        [HttpGet("")]
        public async Task<IActionResult> GetUserCart()
        {
            var userId = User.FindFirstValue("Id");
            var result =await _cartService.CartResponseSummary(userId);

            return Ok(result);
        }

    }
}
