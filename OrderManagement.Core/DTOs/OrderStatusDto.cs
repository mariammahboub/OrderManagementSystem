using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OrderManagement.Core.Entities.Order;

namespace OrderManagement.Core.DTOs
{
    public class OrderStatusDto
    {
        public OrderStatus Status { get; set; }
    }
 
}
