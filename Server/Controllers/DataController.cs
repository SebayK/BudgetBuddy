using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using BudgetBuddy.Models.Dto;
using System.Text;

namespace BudgetBuddy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly BudgetContext _context;

        public DataController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/data/export?userId=1
        [HttpGet("export")]
        public async Task<ActionResult<IEnumerable<TransactionExportDto>>> ExportData([FromQuery] int userId)
        {
            var transactions = await _context.Transaction
                .Where(t => t.UserId == userId)
                .Select(t => new TransactionExportDto
                {
                    Amount = t.Amount,
                    Type = t.Type,
                    Description = t.Description,
                    Date = t.Date,
                    CategoryId = t.CategoryId,
                    BudgetId = t.BudgetId
                })
                .ToListAsync();

            return Ok(transactions);
        }

        // POST: api/data/import
        [HttpPost("import")]
        public async Task<IActionResult> ImportData([FromBody] List<TransactionExportDto> transactions)
        {
            if (transactions == null || !transactions.Any())
                return BadRequest("Brak danych do importu.");

            foreach (var t in transactions)
            {
                var transaction = new Transaction
                {
                    Amount = t.Amount,
                    Type = t.Type,
                    Description = t.Description,
                    Date = t.Date,
                    CategoryId = t.CategoryId,
                    BudgetId = t.BudgetId,
                    // Możesz dodać domyślny UserId jeśli potrzebujesz
                };

                _context.Transaction.Add(transaction);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = $"{transactions.Count} transakcji zaimportowano." });
        }

        // GET: api/data/export/csv?userId=1
        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportToCsv([FromQuery] int userId)
        {
            var transactions = await _context.Transaction
                .Where(t => t.UserId == userId)
                .Select(t => new
                {
                    t.Amount,
                    t.Type,
                    t.Description,
                    t.Date,
                    t.CategoryId,
                    t.BudgetId
                })
                .ToListAsync();

            if (!transactions.Any())
                return NotFound("Brak transakcji do wyeksportowania.");

            var csv = new StringBuilder();
            csv.AppendLine("Amount,Type,Description,Date,CategoryId,BudgetId");

            foreach (var t in transactions)
            {
                csv.AppendLine($"{t.Amount},{t.Type},{Escape(t.Description)},{t.Date:yyyy-MM-dd},{t.CategoryId},{t.BudgetId}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "transactions.csv");
        }

        private string Escape(string value)
        {
            if (value.Contains(",") || value.Contains("\""))
            {
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }
            return value;
        }
    }
}
