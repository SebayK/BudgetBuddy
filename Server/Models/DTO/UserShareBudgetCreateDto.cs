namespace BudgetBuddy.DTO
{
    public class UserShareBudgetCreateDto
    {
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Rola użytkownika w budżecie (np. Viewer, Editor)
        /// </summary>
        public string Role { get; set; } = "Viewer";
    }
}