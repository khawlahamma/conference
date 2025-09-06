using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConferenceManager.WPF.Models;
using Microsoft.Extensions.Configuration;

namespace ConferenceManager.WPF.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                options => options.EnableRetryOnFailure()
            );

            using var context = new ApplicationDbContext(optionsBuilder.Options);
            
            // Assurez-vous que la base de données est créée
            context.Database.EnsureCreated();
            
            // Appliquez les migrations si nécessaire
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.Users.AnyAsync())
                return;

            // Add default admin user
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@conference.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin",
                CreatedAt = DateTime.Now,
                FirstName = "Admin",
                LastName = "User",
                Phone = "N/A",
                ProfilePicture = "N/A"
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }
    }
} 