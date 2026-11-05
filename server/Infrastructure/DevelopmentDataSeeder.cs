using Microsoft.EntityFrameworkCore;

namespace server.Infrastructure;

public static class DevelopmentDataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.ExecuteSqlRawAsync("""
            INSERT INTO "Users"
                ("Name", "Email", "AvatarUrl", "Rating",
                 "ItemsSoldCount", "MemberSinceYear", "IsVerified")
            VALUES
                ('Maya Rivera', 'maya@example.com', '', 4.9,
                 18, 2022, TRUE)
            ON CONFLICT ("Email") DO NOTHING;
            """);

        await db.Database.ExecuteSqlRawAsync("""
            INSERT INTO "Categories" ("Name")
            VALUES ('Home & Garden')
            ON CONFLICT ("Name") DO NOTHING;
            """);
    }
}
