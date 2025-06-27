namespace BudgetBuddy.DTO
{
    public class ShareBudgetCreateDto
    {
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Lista użytkowników do dodania przy tworzeniu budżetu (oprócz właściciela)
        /// </summary>
        public List<UserShareBudgetCreateDto> Users { get; set; } = new();
    }
}