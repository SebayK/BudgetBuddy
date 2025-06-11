using BudgetBuddy.Models;
using BudgetBuddy.Models.DTO;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/Category
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllCategoriesAsync()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    // GET: api/Category/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    // PUT: api/Category/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
        var success = await _categoryService.UpdateCategoryAsync(id, category);

        if (!success)
            return NotFound();

        return NoContent();
    }

    // POST: api/Category
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Category>> PostCategory([FromBody] CreateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newCategory = new Category
        {
            Name = dto.Name,
            Type = dto.Type,
            UserId = dto.UserId
        };

        var createdCategory = await _categoryService.CreateCategoryAsync(newCategory);

        return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
    }
    
    // DELETE: api/Category/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var success = await _categoryService.DeleteCategoryAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
