using Microsoft.Extensions.Configuration;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = OrderManagement.Core.Entities.Product;

namespace OrderManagement.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentResultService _paymentResultService;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IPaymentResultService paymentResultService, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _paymentResultService = paymentResultService;
            _unitOfWork = unitOfWork;
        }

        #region CreateOrUpdatePaymentIntent
        public async Task<PaymentResult> CreateOrUpdatePaymentIntent(int basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Publishablekey"];
            var basket = await _paymentResultService.GetByIdAsync(basketId);
            if (basket == null) return null;
            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Cost;
                shippingPrice = deliveryMethod.Cost;
            }
            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                    if (item.UnitPrice != product.Price)
                    {
                        item.UnitPrice = product.Price;
                    }
                }
            }
            PaymentIntentService paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var createoptions = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.UnitPrice * 100 * item.Quantity) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await paymentIntentService.CreateAsync(createoptions);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var updateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.UnitPrice * 100 * item.Quantity) + (long)shippingPrice * 100,

                };
                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, updateOptions);

            }


            await _paymentResultService.UpdatePaymentResultAsync(basket);
            return basket;
        } 
        #endregion
    }
}
