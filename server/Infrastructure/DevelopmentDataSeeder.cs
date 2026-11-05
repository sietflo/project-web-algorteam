using Microsoft.EntityFrameworkCore;
using server.Entities;

namespace server.Infrastructure;

public static class DevelopmentDataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!await db.Users.AnyAsync(u => u.Email == "maya@example.com"))
        {
            db.Users.Add(new User
            {
                Name = "Maya Rivera",
                Email = "maya@example.com",
                AvatarUrl = "",
                Rating = 4.9,
                ItemsSoldCount = 18,
                MemberSinceYear = 2022,
                IsVerified = true
            });
        }

        if (!await db.Categories.AnyAsync(c => c.Name == "Home & Garden"))
        {
            db.Categories.Add(new Category
            {
                Name = "Home & Garden"
            });
        }

        await db.SaveChangesAsync();
    }
}
