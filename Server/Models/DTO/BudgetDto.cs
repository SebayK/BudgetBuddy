namespace BudgetBuddy.Models.DTO
{
    public class BudgetDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }

        // Lista Id użytkowników przypisanych do budżetu
        public List<string> UserIds { get; set; }

        // Można tu dodać więcej danych, np. ilość transakcji, nazwę, opis, itp. jeśli pojawią się w modelu
    }
}