namespace BudgetBuddy.Models.DTO
{
    public class GoalDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TargetAmount { get; set; }
        public int BudgetId { get; set; }
        public string UserId { get; set; }
        // Można dodać więcej danych, np. ile już oszczędzono, deadline, opis itp.
    }
}