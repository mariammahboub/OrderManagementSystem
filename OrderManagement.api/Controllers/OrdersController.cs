using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.api.Controllers;
using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrderManagement.API.Controllers
{
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region CreateOrder
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            if (orderDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found.");
            }

            if (!int.TryParse(userId, out var customerId))
            {
                return BadRequest("Invalid User ID format.");
            }

            var orderId = await _orderService.CreateOrderAsync(orderDto, customerId);
            return CreatedAtAction(nameof(GetOrderById), new { orderId }, orderId);
        }

        #endregion

        #region GetOrderById
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        #endregion

        #region GetAllOrders
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        #endregion

        #region UpdateOrderStatus
        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatusDto statusDto)
        {
            if (statusDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _orderService.UpdateOrderStatusAsync(orderId, statusDto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
        #endregion
    }
}
