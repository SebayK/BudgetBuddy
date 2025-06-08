using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class CategoryService {
  private readonly BudgetContext _context;

  public CategoryService(BudgetContext context) {
    _context = context;
  }

  public async Task<IEnumerable<Category>> GetAllCategoriesAsync() {
    return await _context.Category.AsNoTracking().ToListAsync();
  }

  public async Task<Category?> GetCategoryByIdAsync(int id) {
    return await _context.Category.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
  }

  public async Task<bool> UpdateCategoryAsync(int id, Category category) {
    if (id != category.Id)
      return false;

    _context.Entry(category).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (DbUpdateConcurrencyException) {
      if (!await CategoryExistsAsync(id))
        return false;

      throw;
    }
  }

  public async Task<Category> CreateCategoryAsync(Category category) {
    _context.Category.Add(category);
    await _context.SaveChangesAsync();
    return category;
  }

  public async Task<bool> DeleteCategoryAsync(int id) {
    var category = await _context.Category.FindAsync(id);
    if (category == null)
      return false;

    _context.Category.Remove(category);
    await _context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> CategoryExistsAsync(int id) {
    return await _context.Category.AnyAsync(c => c.Id == id);
  }
}