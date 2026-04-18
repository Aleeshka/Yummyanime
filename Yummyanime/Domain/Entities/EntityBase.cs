using System.ComponentModel.DataAnnotations;

namespace Yummyanime.Domain.Entities
{
    public abstract class EntityBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заполните названия")]
        [Display(Name = "Название")]
        [MaxLength(200)]
        public string? Title { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
