using Microsoft.AspNetCore.Mvc;
using OrderManagement.api.Controllers;
using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.API.Controllers
{
    public class CustomersController : ApiBaseController
    {
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #region CreateCustomer
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            if (customerDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var customerId = await _customerService.CreateCustomerAsync(customerDto);
                return CreatedAtAction(nameof(GetCustomerOrders), new { customerId = customerId }, customerId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetCustomerOrders
        [HttpGet("{customerId}/orders")]
        public async Task<IActionResult> GetCustomerOrders(int customerId)
        {
            var orders = await _customerService.GetCustomerOrdersAsync(customerId);
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        } 
        #endregion

    }
}
