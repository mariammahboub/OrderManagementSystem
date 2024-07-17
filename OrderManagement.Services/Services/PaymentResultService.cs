using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Services.Services
{
    public class PaymentResultService : IPaymentResultService
    {
        private readonly IGenericRepository<PaymentResult> _paymentResultRepository;

        public PaymentResultService(IGenericRepository<PaymentResult> paymentResultRepository)
        {
            _paymentResultRepository = paymentResultRepository;
        }

        #region GetByIdAsync
        public async Task<PaymentResult> GetByIdAsync(int id)
        {
            return await _paymentResultRepository.GetByIdAsync(id);
        }
        #endregion

        #region UpdatePaymentResultAsync
        public async Task<PaymentResult> UpdatePaymentResultAsync(PaymentResult paymentResult)
        {
            var existingPaymentResult = await _paymentResultRepository.GetByIdAsync(paymentResult.Id);

            if (existingPaymentResult != null)
            {
                existingPaymentResult.Items = paymentResult.Items;
                existingPaymentResult.DeliveryMethodId = paymentResult.DeliveryMethodId;
                existingPaymentResult.ShippingPrice = paymentResult.ShippingPrice;
                existingPaymentResult.PaymentIntentId = paymentResult.PaymentIntentId;
                existingPaymentResult.ClientSecret = paymentResult.ClientSecret;

                await _paymentResultRepository.UpdateAsync(existingPaymentResult);
            }
            else
            {
                await _paymentResultRepository.AddAsync(paymentResult);
            }

            return paymentResult;
        } 
        #endregion
    }
}
