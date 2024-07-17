using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Entities
{
    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public string PaymentMethod { get; set; }
        public OrderStatus Status { get; set; }
        public Customer Customer { get; set; }
        public Invoice Invoice { get; set; }

        public enum OrderStatus
        {
            InCart,
            Preparing,
            Done,
            Decline
        }
    }
}