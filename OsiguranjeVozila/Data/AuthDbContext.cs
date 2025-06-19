using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace OsiguranjeVozila.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            string adminRoleId = "de0c8b1c-80d7-4b15-9732-7398684e9d8a";
            string zaposleniRoleId = "e1eea2da-e13a-4601-979d-18e5155aee6e";
            
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "Administrator",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "Zaposleni",
                    NormalizedName = "Zaposleni",
                    Id= zaposleniRoleId,
                    ConcurrencyStamp = zaposleniRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            string adminId = "5af46be6-2284-4d41-96c3-a914aa147ff0";

            var adminUser = new IdentityUser
            {
                UserName = "danilo",
                Email = "jankovic.danilo23@gmail.com",
                NormalizedUserName = "danilo".ToUpper(),
                NormalizedEmail = "jankovic.danilo23@gmail.com".ToUpper(),
                Id = adminId

            };

            adminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(adminUser, "tuv77tuv");

            builder.Entity<IdentityUser>().HasData(adminUser);

            var adminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = zaposleniRoleId,
                    UserId = adminId,
                },
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminId,
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}
