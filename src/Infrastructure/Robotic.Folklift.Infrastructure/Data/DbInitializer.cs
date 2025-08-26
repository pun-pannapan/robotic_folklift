using Microsoft.EntityFrameworkCore;
using Robotic.Folklift.Domain.Entities;

namespace Robotic.Folklift.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task EnsureCreatedAndSeedAsync(AppDbContext db)
        {
            await db.Database.EnsureCreatedAsync();
            if (!await db.Users.AnyAsync())
            {
                db.Users.Add(new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
