using GearUp.Models;
using GearUp.Models.ResponseModels;
using GearUp.Services.IService;
using GearUp.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GearUp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartInterface _cartService;

        public CartController(ICartInterface cartService)
        {
            _cartService = cartService;
        }

        private int GetIdFromToken()
        {
            var id = HttpContext.User.FindFirstValue("userId");
            return int.Parse(id);
        }

        [HttpGet]
        [Route("GetAllCartItems")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Cart>>>> GetAllCartItems()
        {
            var response = await _cartService.GetAllCartItemsAsync(GetIdFromToken());
            return response;
        }

        [HttpPost]
        [Route("AddItemsToCart")]
        public async Task<IActionResult> AddItemsToCart(int productId, int userId)
        {
            if (productId <= 0 || userId <= 0)
            {
                return BadRequest();
            }

            try
            {
                var result = await _cartService.AddItemsToCartAsync(productId, userId);

                if (result == null)
                {
                    return Conflict();
                }

                return Ok(result);
            }

            catch (Exception)
            {
                return NoContent();
            }
        }
    }
}
