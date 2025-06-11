using System.Security.Claims;

namespace BudgetBuddy.Middlewares;

public class UserMiddleware(RequestDelegate next) {
  private readonly RequestDelegate _next = next;

  public async Task InvokeAsync(HttpContext context) {
    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!string.IsNullOrEmpty(userId)) {
      context.Items["UserId"] = userId;
    }
    
    await _next(context);
  }
}