using BudgetBuddy.Models.DTO;

namespace BudgetBuddy.DTO
{
    public class ShareBudgetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OwnerUserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public List<UserWithRoleDto> Users { get; set; } = new();
    }

    public class UserWithRoleDto
    {
        public UserDto User { get; set; } = new(); 
        public string Role { get; set; } = "Viewer";
    }
}