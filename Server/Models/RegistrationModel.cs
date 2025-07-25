using System.ComponentModel.DataAnnotations;
using BudgetBuddy.Enums;

namespace BudgetBuddy.Models
{
  public class RegistrationModel
  {
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Role is required")]
    public UserRole Role { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
  }
}