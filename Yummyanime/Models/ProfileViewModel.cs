namespace Yummyanime.Models
{
    public class ProfileViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AvatarFileName { get; set; }

        public List<AnimeDTO> FavoriteAnime { get; set; } = new();
        public List<AnimeDTO> WatchingAnime { get; set; } = new();
        public List<AnimeDTO> PlannedAnime { get; set; } = new();
        public List<AnimeDTO> CompletedAnime { get; set; } = new();
        public List<AnimeDTO> PausedAnime { get; set; } = new();
    }
}