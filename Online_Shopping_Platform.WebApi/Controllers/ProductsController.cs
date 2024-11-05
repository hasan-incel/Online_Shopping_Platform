using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Shopping_Platform.Business.Operations.Product;
using Online_Shopping_Platform.Business.Operations.Product.Dtos;
using Online_Shopping_Platform.WebApi.Filters;
using Online_Shopping_Platform.WebApi.Models;

namespace Online_Shopping_Platform.WebApi.Controllers
{
    [Route("api/[controller]")]  // Route for API endpoint, dynamic controller name
    [ApiController]  // Automatically handles model validation and response formatting
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        // Constructor to inject IProductService for product-related business logic
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // Endpoint to add a new product, restricted to Admin role
        [HttpPost]  // HTTP POST method to create a new resource
        [Authorize(Roles = "Admin")]  // Restrict access to Admin role
        [TimeControlFilter]  // Custom filter to control execution time.
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            // Map request data to DTO (Data Transfer Object) for adding a product
            var addProductDto = new AddProductDto
            {
                ProductName = request.ProductName,
                StockQuantity = request.StockQuantity,
                Price = request.Price,
            };

            // Call service method to add the product
            var result = await _productService.AddProduct(addProductDto);

            // Return appropriate response based on the result
            if (result.IsSucceed)
                return Ok(result);  // Success response with result data
            else
                return BadRequest(result.Message);  // Failure response with error message
        }
    }
}
