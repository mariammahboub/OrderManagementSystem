using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.api.Controllers;
using OrderManagement.Core.DTOs;
using OrderManagement.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.API.Controllers
{
    public class InvoicesController : ApiBaseController
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        #region GetInvoiceById
        // GET /api/invoices/{invoiceId}
        [HttpGet("{invoiceId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetInvoiceById(int invoiceId)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }
        #endregion

        #region GetAllInvoices
        // GET /api/invoices
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return Ok(invoices);
        } 
        #endregion
    }
}
