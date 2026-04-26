using Microsoft.AspNetCore.Identity;

namespace Yummyanime.Domain.Entities
{
    public class UserAnimeFavorite
    {
        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }

        public int AnimeId { get; set; }
        public Anime? Anime { get; set; }

        public UserAnimeListStatus Status { get; set; } = UserAnimeListStatus.Favorite;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}