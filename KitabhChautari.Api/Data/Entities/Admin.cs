using Microsoft.AspNetCore.Identity;

namespace KitabhChautari.Api.Data.Entities
{
    public class Admin
    {

        public int AdminId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = nameof(UserRole.Admin);
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string PasswordHash { get; set; }

    }
}
