using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yummyanime.Domain.Entities;

namespace Yummyanime.Infrastructure
{
    public static class DevDataSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using IServiceScope scope = services.CreateScope();
            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            await dbContext.Database.MigrateAsync();

            const string adminRole = "admin";
            const string adminEmail = "admin@yummyanime.local";
            const string adminPassword = "Admin123!";

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            AppUser? admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new AppUser
                {
                    UserName = adminEmail,
                    DisplayName = "Administrator",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    AvatarFileName = null
                };

                IdentityResult createResult = await userManager.CreateAsync(admin, adminPassword);
                if (!createResult.Succeeded)
                {
                    return;
                }
            }

            if (!await userManager.IsInRoleAsync(admin, adminRole))
            {
                await userManager.AddToRoleAsync(admin, adminRole);
            }

            if (!await dbContext.Genres.AnyAsync())
            {
                dbContext.Genres.AddRange(
                    new Genre { Title = "Action" },
                    new Genre { Title = "Fantasy" },
                    new Genre { Title = "Romance" }
                );
                await dbContext.SaveChangesAsync();
            }

            if (!await dbContext.Animes.AnyAsync())
            {
                Dictionary<string, int> genreIds = await dbContext.Genres.ToDictionaryAsync(x => x.Title!, x => x.Id);
                dbContext.Animes.AddRange(
                    new Anime
                    {
                        Title = "Demon Slayer",
                        GenreId = genreIds["Action"],
                        Year = 2019,
                        Rating = 8.7m,
                        DescriptionShort = "Брат и сестра против демонов.",
                        Description = "Тандзиро отправляется в опасное путешествие, чтобы спасти сестру и отомстить за семью.",
                        Photo = null
                    },
                    new Anime
                    {
                        Title = "Frieren: Beyond Journey's End",
                        GenreId = genreIds["Fantasy"],
                        Year = 2023,
                        Rating = 9.1m,
                        DescriptionShort = "После победы над королём демонов приключение только начинается.",
                        Description = "Эльфийка-маги Фрирен исследует мир и заново учится ценить человеческие связи.",
                        Photo = null
                    },
                    new Anime
                    {
                        Title = "Your Name",
                        GenreId = genreIds["Romance"],
                        Year = 2016,
                        Rating = 8.4m,
                        DescriptionShort = "История о загадочной связи двух подростков.",
                        Description = "Мицуха и Таки внезапно начинают меняться телами и пытаются найти друг друга.",
                        Photo = null
                    }
                );

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
