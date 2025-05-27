namespace BudgetBuddy.Models.DTO
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int AccountId { get; set; }
        public string Role { get; set; }
    }
}