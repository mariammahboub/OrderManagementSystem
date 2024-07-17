using OrderManagement.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> GetInvoiceByIdAsync(int invoiceId);
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
    }
}
