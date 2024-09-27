using GearUp.Models;
using GearUp.Models.ResponseModels;
using GearUp.Services.IService;
using GearUp.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GearUp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetAllProducts()
        
        {
            var response = await _productService.GetAllProductsAsync();
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Product>>> GetProductById(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            return response;
        }
    }
}
