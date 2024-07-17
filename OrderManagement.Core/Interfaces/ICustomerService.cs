using OrderManagement.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<int> CreateCustomerAsync(CustomerDto customerDto);
        Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(int customerId);
    }
}
