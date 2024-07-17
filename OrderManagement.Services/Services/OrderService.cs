using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Invoice> _invoiceRepository;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IEmailService _emailService;

        public OrderService(
            IGenericRepository<Order> orderRepository,
            IGenericRepository<Product> productRepository,
            IGenericRepository<Invoice> invoiceRepository,
            IGenericRepository<Customer> customerRepository,
            IEmailService emailService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _invoiceRepository = invoiceRepository;
            _customerRepository = customerRepository;
            _emailService = emailService;
        }

        #region CreateOrderAsync

        public async Task<int> CreateOrderAsync(OrderDto orderDto, int customerId)
        {
            if (!await ValidateStockAsync(orderDto.OrderItems))
            {
                throw new ArgumentException("Insufficient stock for one or more products.");
            }

            var totalAmount = orderDto.OrderItems.Sum(item => item.UnitPrice * item.Quantity);
            totalAmount = ApplyDiscounts(totalAmount);

            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                PaymentMethod = orderDto.PaymentMethod,
                Status = orderDto.Status.Status,
                OrderItems = orderDto.OrderItems.Select(oi => new OrderItem
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Discount = oi.Discount
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
            await GenerateInvoiceAsync(order);
            await SendOrderStatusChangedEmailAsync(customerId, order);

            return order.Id;
        }
        #endregion

        #region GetOrderByIdAsync

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }

            return new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                Status = new OrderStatusDto { Status = order.Status },
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Discount = oi.Discount
                }).ToList()
            };
        }
        #endregion

        #region GetAllOrdersAsync
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                PaymentMethod = o.PaymentMethod,
                Status = new OrderStatusDto { Status = o.Status },
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Discount = oi.Discount
                }).ToList()
            }).ToList();
        }
        #endregion

        #region UpdateOrderStatusAsync
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatusDto statusDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return false;
            }

            order.Status = statusDto.Status;
            await _orderRepository.UpdateAsync(order);
            await SendOrderStatusChangedEmailAsync(order.CustomerId, order);

            return true;
        }
        #endregion

        #region ValidateStockAsync
        private async Task<bool> ValidateStockAsync(List<OrderItemDto> orderItems)
        {
            foreach (var item in orderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region ApplyDiscounts
        private decimal ApplyDiscounts(decimal totalAmount)
        {
            if (totalAmount > 200)
                return totalAmount * 0.90m; // 10% off
            else if (totalAmount > 100)
                return totalAmount * 0.95m; // 5% off
            return totalAmount; // No discount
        }
        #endregion

        #region GenerateInvoiceAsync
        private async Task<Invoice> GenerateInvoiceAsync(Order order)
        {
            var invoice = new Invoice
            {
                OrderId = order.Id,
                InvoiceDate = DateTime.UtcNow,
                TotalAmount = order.TotalAmount
            };
            await _invoiceRepository.AddAsync(invoice);
            return invoice;
        }
        #endregion

        #region SendOrderStatusChangedEmailAsync
        private async Task SendOrderStatusChangedEmailAsync(int customerId, Order order)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            var emailBody = $"Your order #{order.Id} status has changed to {order.Status}.";
            await _emailService.SendEmailAsync(customer.Email, "Order Status Update", emailBody);
        } 
        #endregion
    }
}
