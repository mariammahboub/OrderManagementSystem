using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly OrderManagementDbContext _context;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IUserRepository _userRepository;

        public CustomerService(OrderManagementDbContext context, IGenericRepository<Customer> customerRepository, IUserRepository userRepository)
        {
            _context = context;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }

        #region CreateCustomerAsync
        public async Task<int> CreateCustomerAsync(CustomerDto customerDto)
        {
            // Validate name
            var existingUser = await _userRepository.GetByUsernameAsync(customerDto.Name);
            if (existingUser != null && existingUser.Role == User.UserRole.Customer)
            {
                throw new ArgumentException("A customer with the same username already exists.");
            }

            // Validate email
            if (!IsValidEmail(customerDto.Email))
            {
                throw new ArgumentException("Invalid email format.");
            }

            var existingCustomer = _customerRepository.Find(c => c.Email == customerDto.Email).FirstOrDefault();
            if (existingCustomer != null)
            {
                throw new ArgumentException("A customer with the same email already exists.");
            }

            var customer = new Customer
            {
                Name = customerDto.Name,
                Email = customerDto.Email,
                Orders = new List<Order>()
            };

            await _customerRepository.AddAsync(customer);
            return customer.Id;
        }
        #endregion

        #region GetCustomerOrdersAsync
        public async Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    PaymentMethod = o.PaymentMethod,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        Discount = oi.Discount
                    }).ToList()
                }).ToListAsync();
        }
        #endregion

        #region IsValidEmail
        private bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return mail.Address == email;
            }
            catch
            {
                return false;
            }
        } 
        #endregion
    }
}
