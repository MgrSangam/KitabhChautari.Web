using KitabhChauta.Model;
using KitabhChautari.Api.Data.Entities;
using KitabhChautari.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace KitabhChautari.Api.Data
{
    public class KitabhChautariDBContext : DbContext
    {
        private readonly IPasswordHasher<Admin> _passwordHasher;
        public KitabhChautariDBContext(DbContextOptions<KitabhChautariDBContext> options, IPasswordHasher<Admin> passwordHasher) : base(options)
        {
            _passwordHasher = passwordHasher;
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Author> authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var adminUser = new Admin
            {
                AdminId = 1,
                Name = "Noel Prince",
                Email = "noel@gmail.com",
                Phone = "9817108031",
                Role = nameof(UserRole.Admin),
                IsApproved = true,
            };

            adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, "9817108031");
            modelBuilder.Entity<Admin>()
                .HasData(adminUser);
            
       }
    } 
}
