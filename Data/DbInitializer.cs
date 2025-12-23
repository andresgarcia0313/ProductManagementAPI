using System.Security.Cryptography;
using System.Text;
using ProductManagementAPI.Models;

namespace ProductManagementAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
                return;

            var users = new User[]
            {
                new User
                {
                    Username = "admin",
                    Email = "admin@enterprise.com",
                    PasswordHash = HashPassword("admin123")
                },
                new User
                {
                    Username = "usuario1",
                    Email = "usuario1@enterprise.com",
                    PasswordHash = HashPassword("user123")
                },
                new User
                {
                    Username = "usuario2",
                    Email = "usuario2@enterprise.com",
                    PasswordHash = HashPassword("user456")
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        public static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
