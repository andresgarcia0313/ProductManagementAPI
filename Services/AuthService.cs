using ProductManagementAPI.Data;
using ProductManagementAPI.Models;

namespace ProductManagementAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public User? ValidateUser(string username, string password)
        {
            var passwordHash = DbInitializer.HashPassword(password);
            return _context.Users.FirstOrDefault(u =>
                u.Username == username && u.PasswordHash == passwordHash);
        }
    }
}  