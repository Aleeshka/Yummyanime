using Yummyanime.Domain.Entities;

namespace Yummyanime.Models
{
    public class AnimeShowViewModel
    {
        public AnimeDTO Anime { get; set; } = default!;
        public IReadOnlyList<Comment> Comments { get; set; } = Array.Empty<Comment>();
        public CommentInputModel NewComment { get; set; } = new();
    }
}