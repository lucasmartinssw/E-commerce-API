using Ecommerce.Domain.Enums;

namespace Ecommerce.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
        public string Telephone { get; set; } = string.Empty;

        public UserRoleType Role { get; set; } = UserRoleType.customer;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
