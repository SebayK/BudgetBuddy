using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using BudgetBuddy.Models.Dto;

namespace BudgetBuddy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly BudgetContext _context;

        public CategoriesController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/categories?userId=u123
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("UserId is required.");

            var categories = await _context.Category
                .Where(c => c.UserId == userId)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ColorHex = c.ColorHex
                })
                .ToListAsync();

            return categories;
        }

        // GET: api/categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
                return NotFound();

            var dto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ColorHex = category.ColorHex
            };

            return dto;
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory([FromBody] CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                ColorHex = dto.ColorHex,
                UserId = dto.UserId
            };

            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            var result = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ColorHex = category.ColorHex
            };

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, result);
        }

        // PUT: api/categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
                return NotFound();

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.ColorHex = dto.ColorHex;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
