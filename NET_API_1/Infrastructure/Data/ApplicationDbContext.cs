using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NET_API_1.Models.Entity;

namespace NET_API_1.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> User { get; set; } = default!;
        public DbSet<Role> Role { get; set; } = default!;
        public DbSet<RefreshToken> RefreshToken { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");

            modelBuilder.Entity<Role>().ToTable("Role")
                .HasOne<User>(u => u.User)
                .WithOne(r => r.Role)
                .HasForeignKey<User>(fk => fk.RoleId);

            modelBuilder.Entity<RefreshToken>().ToTable("RefreshToken")
                .HasOne<User>(u => u.User)
                .WithOne(r => r.RefreshToken)
                .HasForeignKey<RefreshToken>(fk => fk.UserId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }


    }
}
