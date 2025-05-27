namespace BudgetBuddy.Models.DTO
{
    public class UserDto
    {
        public string Id { get; set; }              // z IdentityUser
        public string Email { get; set; }           // z IdentityUser
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int AccountId { get; set; }
        public string Role { get; set; }            // Z UserRole (jako string)
    }
}