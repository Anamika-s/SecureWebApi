using static SecureWebApi.Models.Student;
using System.Collections.Generic;
using SecureWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace SecureWebApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<User> Uers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().
                HasData(new Role
                {
                   RoleId=1,
                   RoleName="Admin"
                }, new Role
                {
                   RoleId=2,
                   RoleName="Customer"
                },
                new Role
                {
                    RoleId=3,
                    RoleName="Moderator"
                }
                );

            modelBuilder.Entity<User>().
               HasData(new User
               {
                   Id = 100,
                   username = "admin1",
                   password = "admin1",
                   RoleId=1
               }, new User
               {
                   username = "moderator1",
                   Id = 101,
                   password = "moderator1",
                   RoleId=3
               },
               new User
               {
                   username = "cust1",
                   password = "cust1",
                   Id = 102,
                   RoleId=2
               }); 
        }

    }
}
