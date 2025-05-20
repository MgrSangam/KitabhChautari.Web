using KitabhChautari.Shared;
using System.ComponentModel.DataAnnotations;

namespace KitabhChautari.Api.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(10)]
        public string Phone { get; set; }

        [MaxLength(250)]
        public string PasswordHash { get; set; }
        [MaxLength(15)]
        public  string Role { get; set; } = nameof(UserRole.Member);

        public bool IsApproved { get; set; }

    }
}
