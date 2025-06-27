namespace BudgetBuddy.Models.DTO
{
    public class UserShareBudgetCreateDto
    {
        public string UserId { get; set; } = string.Empty;
        
        // Rola użytkownika w budżecie (np. Viewer, Editor)
        public string Role { get; set; } = "Viewer";
    }
}