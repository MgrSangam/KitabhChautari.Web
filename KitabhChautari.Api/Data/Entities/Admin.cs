using KitabhChautari.Shared;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KitabhChautari.Api.Data.Entities
{
    public class Admin
    {
        public int AdminId { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(10)]
        public string Phone { get; set; }

        [MaxLength(50)]
        public string Role { get; set; } = nameof(UserRole.Admin);

        [MaxLength(250)]
        public string PasswordHash { get; set; }

        public bool IsApproved { get;  set; } = true; 
    }
}
