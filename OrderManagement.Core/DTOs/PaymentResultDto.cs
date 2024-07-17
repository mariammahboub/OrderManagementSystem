using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.DTOs
{
    public class PaymentResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } // Optional message, such as error details
        public string TransactionId { get; set; } // Optional transaction ID for tracking

        public PaymentResultDto()
        {
        }

        public PaymentResultDto(bool success, string message = null, string transactionId = null)
        {
            Success = success;
            Message = message;
            TransactionId = transactionId;
        }
    }
}
