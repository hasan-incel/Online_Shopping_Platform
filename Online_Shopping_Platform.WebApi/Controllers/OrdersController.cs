using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Online_Shopping_Platform.Business.Operations.Order;
using Online_Shopping_Platform.Business.Operations.Order.Dtos;
using Online_Shopping_Platform.WebApi.Filters;
using Online_Shopping_Platform.WebApi.Models;
using System.Security.Claims;

namespace Online_Shopping_Platform.WebApi.Controllers
{
    [Route("api/[controller]")]  // Define route pattern for this controller
    [ApiController]  // Automatically handles model validation and error responses
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        // Constructor to inject IOrderService for order-related business logic
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Endpoint to get a single order by ID
        [HttpGet("{id}")]  // HTTP GET method to fetch a specific order
        [Authorize]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderService.GetOrder(id);

            // Return NotFound if order does not exist, otherwise return the order
            if (order is null)
                return NotFound();
            else
                return Ok(order);
        }

        // Endpoint to get all orders
        [HttpGet]  // HTTP GET method to fetch all orders
        [Authorize]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrders();

            // Return all orders
            return Ok(orders);
        }

        // Endpoint to add a new order
        [HttpPost]  // HTTP POST method to create a new order
        [Authorize]
        public async Task<IActionResult> AddOrder(AddOrderRequest request)
        {
            // Map request data to DTO (Data Transfer Object) for adding an order
            var addOrderDto = new AddOrderDto
            {
                OrderDate = request.OrderDate,
                TotalAmount = request.TotalAmount,
                PorductIds = request.PorductIds,
            };

            // Call service to add the order
            var result = await _orderService.AddOrder(addOrderDto);

            // Return appropriate response based on success or failure
            if (!result.IsSucceed)
                return BadRequest(result.Message);  // Failure response
            else
                return Ok();  // Success response
        }

        // Endpoint to adjust the total amount of an order
        [HttpPatch("{id}/totalAmount")]  // HTTP PATCH method to update a specific part of the order
        [Authorize]  // Requires authentication to access this endpoint
        public async Task<IActionResult> AdjustTotalAmount(int id, int changeTo)
        {
            var result = await _orderService.AdjustTotalAmount(id, changeTo);

            // Return NotFound if the order doesn't exist, otherwise return success
            if (!result.IsSucceed)
                return NotFound();
            else
                return Ok();
        }

        // Endpoint to delete an order
        [HttpDelete("{id}")]  // HTTP DELETE method to remove an order
        [Authorize]  // Requires authentication to access this endpoint
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrder(id);

            // Return NotFound if the order doesn't exist, otherwise return success
            if (!result.IsSucceed)
                return NotFound();
            else
                return Ok();
        }

        // Endpoint to update an existing order
        [HttpPut("{id}")]  // HTTP PUT method to update an existing order
        [Authorize]  // Requires authentication to access this endpoint
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderRequest request)
        {
            // Map request data to DTO for updating the order
            var updateOrderDto = new UpdateOrderDto
            {
                Id = id,
                OrderDate = request.OrderDate,
                TotalAmount = request.TotalAmount,
                PorductIds = request.PorductIds,
            };

            var result = await _orderService.UpdateOrder(updateOrderDto);

            // Return NotFound if the order doesn't exist, otherwise fetch and return the updated order
            if (!result.IsSucceed)
                return NotFound(result.Message);  // Failure response
            else
                return await GetOrder(id);  // Return the updated order
        }
    }

}
