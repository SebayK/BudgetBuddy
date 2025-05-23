namespace BudgetBuddy.Models.DTO
{
    public class CreateAccountDto
    {
        public string UserId { get; set; }
        public int AccountNumber { get; set; }
        public int AccountTypesId { get; set; }
        public string CurrencyId { get; set; }
    }
}