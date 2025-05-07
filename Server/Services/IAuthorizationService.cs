using BudgetBuddy.Models;
using BudgetBuddy.Enums;
namespace BudgetBuddy.Services;

public interface IAuthorizationService {
  Task<(int, string)> Registration(RegistrationModel model, UserRole role);
  Task<(int, string)> Login(LoginModel model);
}