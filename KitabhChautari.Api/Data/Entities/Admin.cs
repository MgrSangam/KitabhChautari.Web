using KitabhChautari.Shared;
using System.ComponentModel.DataAnnotations;


namespace KitabhChautari.Api.Data.Entities
{
    public class Admin
    {

        public int AdminId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = nameof(UserRole.Admin);
        public string PasswordHash { get; set; }

    }
}
