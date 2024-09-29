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

        [HttpGet]
        [Route("GetAllCartItems")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CartItemResponseModel>>>> GetAllCartItems(int userId)
        {
            var response = await _cartService.GetAllCartItems(userId);
            return response;
        }

        [HttpPost]
        [Route("AddItemsToCart")]
        public async Task<ActionResult<ApiResponse<CartItemResponseModel>>> AddItemsToCart(int productId, int userId, int quantity=1)
        {
            if (productId <= 0 || userId <= 0)
            {
                return BadRequest();
            }

            try
            {
                var result = await _cartService.AddItemToCartAsync(productId, userId, quantity);

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

        [HttpPut]
        [Route("UpdateItemQuantity")]
        public async Task<ActionResult<ApiResponse<CartItemResponseModel>>> UpdateItemQuantity(int productId, int userId, int quantity)
        {
            try
            {
                var updatedItem = await _cartService.UpdateItemQuantityAsync(productId, userId, quantity);
                return Ok(updatedItem);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Route("RemoveItemFromCart")]
        public async Task<ActionResult<ApiResponse<CartItemResponseModel>>> RemoveItemFromCart(int productId, int userId, int quantity)
        {
            try
            {
                var removedItem = await _cartService.RemoveItemFromCartAsync(productId, userId, quantity);
                return Ok(removedItem);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("DeleteItemFromCart")]
        public async Task<ActionResult<ApiResponse<CartItemResponseModel>>> DeleteItemFromCart(int productId, int userId)
        {
            try
            {
                var deletedItem = await _cartService.DeleteItemFromCartAsync(productId, userId);
                return Ok(deletedItem);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
