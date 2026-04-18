namespace Yummyanime.Models
{
    public class AnimeDTO
    {
        public int Id { get; set; }
        public string? GenreName { get; set; }
        public string? Title { get; set; }
        public string? DescriptionShort { get; set; }
        public string? Description { get; set; }
        public string? PhotoFileName { get; set; }
        public int Year { get; set; }
        public decimal Rating { get; set; }
    }
}
