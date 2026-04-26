using Microsoft.AspNetCore.Identity;

namespace Yummyanime.Domain.Entities
{
    public class UserAnimeFavorite
    {
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }

        public int AnimeId { get; set; }
        public Anime? Anime { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}