using System.ComponentModel.DataAnnotations;

namespace Yummyanime.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }

        [Required]
        public int AnimeId { get; set; }
        public Anime? Anime { get; set; }

        [Required(ErrorMessage = "Комментарий не может быть пустым")]
        [MaxLength(1000, ErrorMessage = "Длина комментария не может превышать 1000 символов")]
        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}