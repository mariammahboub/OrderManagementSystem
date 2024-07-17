using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Entities
{
    public class PaymentResult:BaseEntity
    {
        public List<OrderItem> Items { get; set; }
        public int? DeliveryMethodId { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        public decimal  ShippingPrice { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public PaymentResult(int id)
        {
            Id = id;
            Items = new List<OrderItem>();

        }
    }
}
