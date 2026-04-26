namespace Yummyanime.Models
{
    public class ProfileViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<AnimeDTO> FavoriteAnime { get; set; } = new();
    }
}