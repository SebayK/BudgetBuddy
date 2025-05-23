namespace BudgetBuddy.Models.DTO
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; } // Hasło tylko przy rejestracji
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int AccountId { get; set; }
        public string Role { get; set; } // Można przesyłać jako string, a potem mapować na UserRole
    }
}