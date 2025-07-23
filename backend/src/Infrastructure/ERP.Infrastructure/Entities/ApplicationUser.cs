using Microsoft.AspNetCore.Identity;

namespace ERP.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public string? ProfilePictureUrl { get; set; }
        public DateTime? LastLoginDate { get; set; }

        // Navigation properties
        // Ajouter ici les relations avec d'autres entités si nécessaire
        // public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}