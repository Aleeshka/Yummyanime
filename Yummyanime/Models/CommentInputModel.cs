using System.ComponentModel.DataAnnotations;

namespace Yummyanime.Models
{
    public class CommentInputModel
    {
        [Required]
        public int AnimeId { get; set; }

        [Required(ErrorMessage = "Комментарий не может быть пустым")]
        [MaxLength(1000, ErrorMessage = "Длина комментария не может превышать 1000 символов")]
        public string Text { get; set; } = string.Empty;
    }
}