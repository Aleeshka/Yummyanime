using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Yummyanime.Domain.Entities
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Anime> Animes { get; set; }
        public DbSet<UserAnimeFavorite> UserAnimeFavorites { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Anime>()
                .Property(x => x.Rating)
                .HasPrecision(3, 1);

            builder.Entity<Anime>()
                .HasOne(x => x.Genre)
                .WithMany(x => x.Animes)
                .HasForeignKey(x => x.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserAnimeFavorite>()
                .HasKey(x => new { x.UserId, x.AnimeId });

            builder.Entity<UserAnimeFavorite>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserAnimeFavorite>()
                .HasOne(x => x.Anime)
                .WithMany()
                .HasForeignKey(x => x.AnimeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
