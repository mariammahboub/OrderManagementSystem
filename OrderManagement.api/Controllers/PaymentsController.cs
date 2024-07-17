using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.api.Controllers
{
    [Authorize]
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        #region CreateOrUpdatePaymentIntent
        [HttpPost("basketId")]
        public async Task<ActionResult<PaymentResult>> CreateOrUpdatePaymentIntent(int basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest();
            return Ok(basket);
        } 
        #endregion
    }
}
