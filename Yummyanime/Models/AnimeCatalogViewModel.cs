namespace Yummyanime.Models
{
    public class AnimeCatalogViewModel
    {
        public IEnumerable<AnimeDTO> Items { get; set; } = Enumerable.Empty<AnimeDTO>();
        public string? Search { get; set; }
        public string Sort { get; set; } = "year_desc";
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
