namespace BudgetBuddy.Models.DTO
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AccountNumber { get; set; }
        public int AccountTypesId { get; set; } // klucz obcy do typu konta
        public string CurrencyId { get; set; }
        // Opcjonalnie można dodać nazwę typu konta, jeśli chcemy wyświetlać na froncie:
        // public string AccountTypeName { get; set; }
    }
}