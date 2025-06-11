using BudgetBuddy.Models;
using BudgetBuddy.Models.DTO;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;

        public InvoicesController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET: api/Invoices
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetAllInvoicesAsync()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return Ok(invoices);
        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);

            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }

        // PUT: api/Invoices/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
        {
            var success = await _invoiceService.UpdateInvoiceAsync(id, invoice);

            if (!success)
                return NotFound();

            return NoContent();
        }

        // POST: api/Invoices
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Invoice>> PostInvoice([FromBody] CreateInvoiceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newInvoice = new Invoice
            {
                FilePath = dto.FilePath,
                UploadDate = dto.UploadDate,
                ExpenseId = dto.ExpenseId
            };

            var createdInvoice = await _invoiceService.CreateInvoiceAsync(newInvoice);
            return CreatedAtAction(nameof(GetInvoice), new { id = createdInvoice.Id }, createdInvoice);
        }

        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var success = await _invoiceService.DeleteInvoiceAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
