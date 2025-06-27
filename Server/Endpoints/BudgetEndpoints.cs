using BudgetBuddy.Models;
using BudgetBuddy.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

public static class BudgetEndpoints
{
    public static void MapBudgetEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Budget").WithTags(nameof(Budget));

        group.MapGet("/", async (BudgetContext db) =>
            await db.Budgets.Include(b => b.UserBudgets).ToListAsync()
        ).WithName("GetAllBudgets").WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Budget>, NotFound>> (int id, BudgetContext db) =>
            await db.Budgets.Include(b => b.UserBudgets).AsNoTracking().FirstOrDefaultAsync(b => b.Id == id) is Budget b
                ? TypedResults.Ok(b)
                : TypedResults.NotFound()
        ).WithName("GetBudgetById").WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Budget budget, BudgetContext db) =>
        {
            var affected = await db.Budgets
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.TotalAmount, budget.TotalAmount));
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        }).WithName("UpdateBudget").WithOpenApi();

        group.MapPost("/", async (Budget budget, BudgetContext db) =>
        {
            db.Budgets.Add(budget);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Budget/{budget.Id}", budget);
        }).WithName("CreateBudget").WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, BudgetContext db) =>
        {
            var affected = await db.Budgets
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        }).WithName("DeleteBudget").WithOpenApi();
    }
}
