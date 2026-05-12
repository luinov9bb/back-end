using bookStore.DataAccess.Context;
using bookStore.Domain.Entities;
using bookStore.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace bookStore.BusinessLogic.Seed
{
    public static class AuthSeed
    {
        public static void EnsureAdmin(IConfiguration configuration)
        {
            var seed = configuration.GetSection("Seed");
            var enableAdmin = !bool.TryParse(seed["EnableAdmin"], out var parsedEnableAdmin) || parsedEnableAdmin;
            if (!enableAdmin)
            {
                return;
            }

            using var db = new UserContext();
            if (db.Users.Any(u => u.Role == UserRole.Admin))
            {
                return;
            }

            var username = (seed["AdminUsername"] ?? "admin").Trim();
            var email = (seed["AdminEmail"] ?? "admin@bookstore.local").Trim();
            var password = seed["AdminPassword"] ?? "Admin123!";

            if (username.Length is < 3 or > 20 || email.Length is < 3 or > 30 || password.Length < 8)
            {
                throw new InvalidOperationException("Seed admin credentials do not match UserData validation rules.");
            }

            var existing = db.Users.FirstOrDefault(u => u.Username == username || u.Email == email);
            if (existing != null)
            {
                existing.Role = UserRole.Admin;
                existing.IsActive = true;
                existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                db.SaveChanges();
                return;
            }

            db.Users.Add(new UserData
            {
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RegisteredOn = DateTime.UtcNow,
                Role = UserRole.Admin,
                IsActive = true
            });

            db.SaveChanges();
        }
    }
}
