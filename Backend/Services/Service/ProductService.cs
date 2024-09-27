using GearUp.Models;
using GearUp.Models.ResponseModels;
using GearUp.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace GearUp.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly GearUpContext _context;

        public ProductService(GearUpContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<IEnumerable<Product>>> GetAllProductsAsync()
        {
            var response = new ApiResponse<IEnumerable<Product>>();
            try
            {
                response.Data = await _context.Products.ToListAsync();
                response.StatusCode = 200;
                response.Message = "Products retrieved successfully.";
            }
            catch (DbUpdateException ex)
            {
                response.StatusCode = 500;
                response.Message = "Database error occurred while retrieving products: " + ex.Message;
                response.Data = null;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "An unexpected error occurred: " + ex.Message;
                response.Data = null;
            }

            return response;
        }

        public async Task<ApiResponse<Product>> GetProductByIdAsync(int id)
        {
            var response = new ApiResponse<Product>();
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                response.StatusCode = 404;
                response.Message = "Product not found.";
                response.Data = null;
            }
            else
            {
                response.StatusCode = 200;
                response.Message = "Product retrieved successfully.";
                response.Data = product;
            }

            return response;
        }
    }
}
