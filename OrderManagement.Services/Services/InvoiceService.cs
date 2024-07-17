using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Services.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IGenericRepository<Invoice> _invoiceRepository;

        public InvoiceService(IGenericRepository<Invoice> invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        #region GetInvoiceByIdAsync
        public async Task<InvoiceDto> GetInvoiceByIdAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice == null)
            {
                return null;
            }

            return new InvoiceDto
            {
                Id = invoice.Id,
                OrderId = invoice.OrderId,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount
            };
        }
        #endregion

        #region GetAllInvoicesAsync

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            return invoices.Select(i => new InvoiceDto
            {
                Id = i.Id,
                OrderId = i.OrderId,
                InvoiceDate = i.InvoiceDate,
                TotalAmount = i.TotalAmount
            }).ToList();
        } 
        #endregion
    }
}
