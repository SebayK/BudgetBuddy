namespace BudgetBuddy.Models.DTO
{
    public class UpdateAccountDto
    {
        public int AccountNumber { get; set; }
        public int AccountTypesId { get; set; }
        public string CurrencyId { get; set; }
    }
}