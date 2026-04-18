namespace Yummyanime.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<AnimeDTO> TopAnime { get; set; } = Enumerable.Empty<AnimeDTO>();
        public IEnumerable<AnimeDTO> Updates { get; set; } = Enumerable.Empty<AnimeDTO>();
    }
}
