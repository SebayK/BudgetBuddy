using BudgetBuddy.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BudgetBuddy.Models;

public class Budget
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }

    public ICollection<Goal> Goals { get; set; }
    public ICollection<Transaction> Transactions { get; set; }

    //  NOWE lista użytkowników przypisanych do budżetu
    public ICollection<UserBudget> UserBudgets { get; set; }
}

public static class BudgetEndpoints
{
    public static void MapBudgetEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Budget").WithTags(nameof(Budget));

        group.MapGet("/", async (BudgetContext db) =>
        {
            return await db.Budget
                .Include(b => b.UserBudgets) // opcjonalnie: wczytaj przypisanych użytkowników
                .ToListAsync();
        })
        .WithName("GetAllBudgets")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Budget>, NotFound>> (int id, BudgetContext db) =>
        {
            return await db.Budget
                .Include(b => b.UserBudgets) // opcjonalnie
                .AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Budget model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBudgetById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Budget budget, BudgetContext db) =>
        {
            var affected = await db.Budget
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.TotalAmount, budget.TotalAmount)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateBudget")
        .WithOpenApi();

        group.MapPost("/", async (Budget budget, BudgetContext db) =>
        {
            db.Budget.Add(budget);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Budget/{budget.Id}", budget);
        })
        .WithName("CreateBudget")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, BudgetContext db) =>
        {
            var affected = await db.Budget
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteBudget")
        .WithOpenApi();
    }
}
